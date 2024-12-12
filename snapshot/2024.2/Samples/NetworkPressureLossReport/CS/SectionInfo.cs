using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;

namespace Revit.SDK.Samples.NetworkPressureLossReport
{
   internal class SectionInfo
   {
      private double m_totalLoss;
      private IList<SegmentInfo> m_segments;
      const double Epsilon = 0.0001;    // The small tolerance within which two flow values may be considered equal.

      public SectionInfo()
      {
         m_totalLoss = 0.0;
         m_segments = new List<SegmentInfo>();
      }
      public double TotalPressureLoss
      {
         get { return m_totalLoss; }
         set { m_totalLoss = value; }
      }
      public int NumberOfSegments
      {
         get { return m_segments.Count; }
      }
      public int NumberOfStraights
      {
         get { return m_segments.Count(x => x.SegmentType == MEPAnalyticalSegmentType.Segment); }
      }
      public int NumberOfFittingsOrAccessories
      {
         get { return m_segments.Count(x => x.SegmentType == MEPAnalyticalSegmentType.Fitting); }
      }
      public double Flow
      {
         get { return m_segments.FirstOrDefault().Flow; }
      }
      public double Size
      {
         get { return m_segments.FirstOrDefault().Size; }
      }
      public double Velocity
      {
         get { return m_segments.FirstOrDefault().Velocity; }
      }
      public double VelocityPressure
      {
         get { return m_segments.FirstOrDefault().VelocityPressure; }
      }
      public double Friction
      {
         get { return m_segments.FirstOrDefault().Friction; }
      }
      public bool IsCriticalPath
      {
         get { return m_segments.FirstOrDefault().IsCriticalPath; }
      }

      public SegmentInfo AddSegment(Document doc, MEPAnalyticalSegment segment, MEPNetworkSegmentData segmentData)
      {
         SegmentInfo newSegmentInfo = new SegmentInfo(doc, segment, segmentData);
         m_segments.Add(newSegmentInfo);
         return newSegmentInfo;
      }

      public void ExportCSV(CSVExporter ex, int sectionNumber)
      {
         // "Section, Type/No, Element, Flow, Size, Velocity, Velocity Pressure, Length, Coefficients, Friction, Pressure Loss, Section Pressure Loss");
         string sNull = null;
         int straightCount = 0, fittingCount = 0;
         double totalStraightLength = 0.0;
         double totalFittingCoeff = 0.0;
         double totalStraightLoss = 0.0;
         double totalFittingLoss = 0.0;

         foreach (SegmentInfo segInfo in m_segments)
         {
            if (ex.IsItemized == true)
            {
               ex.Writer.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}",
                  sectionNumber, segInfo.SegmentType, segInfo.Id, ex.ConvertFromInternalFlow(segInfo.Flow), ex.ConvertFromInternalSize(segInfo.Size),
                  ex.ConvertFromInternalVelocity(segInfo.Velocity), ex.ConvertFromInternalPressure(segInfo.VelocityPressure), ex.ConvertFromInternalLength(segInfo.Length),
                  segInfo.Coefficients, ex.ConvertFromInternalFriction(segInfo.Friction), ex.ConvertFromInternalPressure(segInfo.PressureDrop), sNull));
            }
            if (segInfo.SegmentType == MEPAnalyticalSegmentType.Segment)
            {
               straightCount++;
               totalStraightLength += segInfo.Length;
               totalStraightLoss += segInfo.PressureDrop;
            }
            else if (segInfo.SegmentType == MEPAnalyticalSegmentType.Fitting)
            {
               fittingCount++;
               totalFittingCoeff += segInfo.Coefficients;
               totalFittingLoss = segInfo.PressureDrop;
            }
         }
         TotalPressureLoss = totalStraightLoss + totalFittingLoss;

         // "Section, Type/No, Element, Flow, Size, Velocity, Velocity Pressure, Length, Coefficients, Friction, Pressure Loss, Section Pressure Loss");
         ex.Writer.WriteLine(string.Format("{0}, {1}, Straights, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}",
            sectionNumber, straightCount, ex.ConvertFromInternalFlow(Flow), ex.ConvertFromInternalSize(Size),
            ex.ConvertFromInternalVelocity(Velocity), sNull, ex.ConvertFromInternalLength(totalStraightLength), sNull,
            ex.ConvertFromInternalFriction(Friction), ex.ConvertFromInternalPressure(totalStraightLoss), sNull));

         ex.Writer.WriteLine(string.Format("{0}, {1}, Fittings, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}",
            sectionNumber, fittingCount, ex.ConvertFromInternalFlow(Flow), ex.ConvertFromInternalSize(Size),
            ex.ConvertFromInternalVelocity(Velocity), ex.ConvertFromInternalPressure(VelocityPressure), sNull,
            totalFittingCoeff, sNull, ex.ConvertFromInternalPressure(totalFittingLoss), ex.ConvertFromInternalPressure(TotalPressureLoss)));
      }
      public void UpdateView(AVFViewer viewer, List<XYZ> points, List<VectorAtPoint> valList, double maxFlow)
      {
         double coeff = viewer.Scale / 12.0;

         double maxX = -Double.MaxValue;
         double maxY = -Double.MaxValue;
         double maxZ = -Double.MaxValue;

         foreach (SegmentInfo seg in m_segments)
         {
            // With the flow value being the scaled vector length, the fittings typically get much longer vector than its actual length.
            // As such, we skip the fitting flow display. 
            if (seg.SegmentType != MEPAnalyticalSegmentType.Segment)
               continue;

            // Skip the zero flow segments.
            if (seg.Flow < 0.0000001)
               continue;

            XYZ p0 = seg.Start;
            XYZ p1 = seg.End;

            maxX = Math.Max(Math.Max(maxX, p0.X), p1.X);
            maxY = Math.Max(Math.Max(maxY, p0.Y), p1.Y);
            maxZ = Math.Max(Math.Max(maxZ, p0.Z), p1.Z);

            points.Add(p1);

            List<XYZ> xyzList = new List<XYZ>();
            XYZ vec = (p1 - p0) / coeff;     // This is the exact segment length at the current view scale.

            // Convert the vector length to the flow value in scale.
            vec = vec.Normalize();
            vec *= seg.Flow / maxFlow;

            xyzList.Add(vec) ;
            
            valList.Add(new VectorAtPoint(xyzList));

            if (points.Count >= 1000) // 1000 is the limit on the number of points for one spatial field primitive
               viewer.AddData(points, valList);

            // Only display the first segment in the section unless checked "Itemized", since the flow value and direction are the same in one section.
            if (!viewer.IsItemized)
               break;
         }

         viewer.AddCorner(maxX, maxY, maxZ);

      }
   }
}
