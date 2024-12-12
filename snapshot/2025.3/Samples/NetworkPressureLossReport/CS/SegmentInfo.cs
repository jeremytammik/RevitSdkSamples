using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Revit.SDK.Samples.NetworkPressureLossReport
{
   internal class SegmentInfo : IDisposable
   {
      private MEPNetworkSegmentId m_id;
      private MEPAnalyticalSegmentType m_segmentType;
      private bool m_isCriticalPath;
      private double m_length;
      private double m_size;
      private double m_flow;
      private double m_velocity;
      private double m_velocityPressure;
      private double m_coefficients;
      private double m_pressureDrop;
      private double m_reynolds;
      private XYZ m_startPt;
      private XYZ m_endPt;
      const double Tolerance = 0.0000001;
      private bool isDisposed;

      public double Flow
      {
         get { return m_flow; }
      }
      public double PressureDrop
      {
         get { return m_pressureDrop; }
      }
      public MEPAnalyticalSegmentType SegmentType
      {
         get { return m_segmentType; }
      }
      public bool IsCriticalPath
      {
         get { return m_isCriticalPath; }
      }
      public string Id  
      {
         get { return m_id.ElementId.ToString() + @"_" + m_id.SegmentId.ToString(); }
      }
      public ElementId RevitElementId
      {
         get { return m_id.ElementId; }
      }
      public double Length
      {
         get { return m_length; }
      }
      public double Size
      {
         get { return m_size; }
      }
      public double Velocity
      {
         get { return m_velocity; }
      }
      public double VelocityPressure
      {
         get { return m_velocityPressure; }
      }
      public double Coefficients
      {
         get { return m_coefficients; }
      }
      public double Friction
      {
         get { return m_length < Tolerance ? 0.0 : m_pressureDrop / m_length; }
      }
      public double ReynoldsNumber
      {
         get { return m_reynolds; }
      }
      public XYZ Start
      {
         get { return m_startPt; }
      }
      public XYZ End
      {
         get { return m_endPt; }
      }
      public SegmentInfo(Document doc, MEPAnalyticalSegment seg, MEPNetworkSegmentData data)
      {
         m_id = new MEPNetworkSegmentId(seg.RevitElementId, seg.Id);

         // Be aware that the flow and pressure may be negative.
         // It means the flow is from the end node to the start node.
         m_segmentType = seg.SegmentType;
         m_size = seg.InnerDiameter;        // Hydraulic diameter for rectangular or oval profile.
         m_flow = Math.Abs(data.Flow);
         m_pressureDrop = data.Flow > 0 ? data.PressureDrop : -1 * data.PressureDrop;

         m_velocity = Math.Abs(data.Velocity);
         m_velocityPressure = Math.Abs(data.VelocityPressure);
         m_coefficients = data.Coefficient;
         m_isCriticalPath = data.IsCriticalPath;
         m_reynolds = data.ReynoldsNumber;

         m_length = 0.0;
         Element thisElem = doc.GetElement(seg.RevitElementId);
         if (thisElem != null)
         {
            MEPAnalyticalModelData thisModel = MEPAnalyticalModelData.GetMEPAnalyticalModelData(thisElem);
            MEPAnalyticalNode start = thisModel.GetNodeById(seg.StartNode);
            MEPAnalyticalNode end = thisModel.GetNodeById(seg.EndNode);
            if (start != null && end != null)
            {
               m_startPt = data.Flow > 0 ? start.Location : end.Location;
               m_endPt = data.Flow > 0 ? end.Location : start.Location;
               m_length = m_startPt.DistanceTo(m_endPt);
            }
         }
      }

      public void Dispose()
      {
         // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      protected virtual void Dispose(bool disposing)
      {
         if (!disposing || isDisposed)
            return;

         (m_id as IDisposable)?.Dispose();

         isDisposed = true;
      }
   }

   internal class CompareNetworkSegmentId : IEqualityComparer<MEPNetworkSegmentId>
   {
      public bool Equals(MEPNetworkSegmentId left, MEPNetworkSegmentId right)
      {
         return left.ElementId == right.ElementId
             && left.SegmentId == right.SegmentId;
      }

      public int GetHashCode(MEPNetworkSegmentId idSeg)
      {
         // A simple way to combine the element id and segment id into one hash code.
         int hash = 17;
         hash = hash * 31 + idSeg.ElementId.GetHashCode();
         hash = hash * 31 + idSeg.SegmentId.GetHashCode();
         return hash;
      }
   }
}
