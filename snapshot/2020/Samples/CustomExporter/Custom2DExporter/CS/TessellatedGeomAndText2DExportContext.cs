using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.Custom2DExporter.CS
{
   /// <summary>
   /// This is an implementation of IExportContext2D, to be passed to an instance of CustomExporter object. It exports:
   /// - (if CustomExporter has IncludeGeometricObjects == true) exports all model geometry, tessellated into lines and represented by a list of points  contained in m_points.
   /// - (if CustomExporter has Export2DIncludingAnnotationObjects == true) exports all annotation geometry, tessellated into lines represented by a list of points contained in m_points.
   /// - (if CustomExporter has Export2DIncludingAnnotationObjects == true) export all text strings in text notes, divided by a newline and collected in m_texts.
   /// - (if CustomExporter has Export2DGeometricObjectsIncludingPatternLines == true) exports all patterns, tessellated into lines and represented by a list of points  contained in m_points.
   /// - the exporter also keeps a tally of all exported elements in m_numElements.
   /// The methods OnCurve, OnPolyline, OnFaceBegin, OnFaceEdge2D and OnFaceSilhouette2D return RenderNodeAction.Proceed, 
   /// will makes sure that all geometry comes in tessellated in OnLineSegment and OnPolylineSegments.
   /// Note1: annnotation geometry in Wireframe iz exported in OnCurve and OnPolyline, where it is tessellated and stored in m_points.
   /// Note2: If you wish to process geometry without tessellating it, then you need to implement curve export in
   /// OnCurve, OnPolyline, OnFaceBegin, OnFaceEdge2D and OnFaceSilhouette2D and return RenderNodeAction.Skip.
   /// Note3: Special instance transforms may not always be taken into account.
   /// </summary>
   class TessellatedGeomAndText2DExportContext : IExportContext2D
   {
      #region Data
      /// <summary>
      /// The list of (start, end) points for all tessellated lines
      /// </summary>
      private IList<XYZ> m_points = new List<XYZ>();

      /// <summary>
      /// The number of all processed elements, as well as breakdown by some element kinds
      /// </summary>
      private int m_numElements = 0;
      private int m_numTexts = 0;

      /// <summary>
      /// All text collected in the view, with a newline between each TextNode.
      /// </summary>
      private string m_texts;

      Element m_currentElem = null;

      public int NumElements
      {
         get
         {
            return m_numElements;
         }
      }

      public int NumTexts
      {
         get
         {
            return m_numTexts;
         }
      }

      public string Texts
      {
         get
         {
            return m_texts;
         }
      }
      #endregion

      #region IExportContext2DOverrides
      public TessellatedGeomAndText2DExportContext(out IList<XYZ> points)
      {
         points = m_points;
      }

      public bool Start()
      {
         return true;
      }

      public void Finish()
      {
      }

      public bool IsCanceled()
      {
         return false;
      }

      public RenderNodeAction OnViewBegin(ViewNode node)
      {
         return RenderNodeAction.Proceed;
      }

      public void OnViewEnd(ElementId elementId)
      {
      }

      public RenderNodeAction OnInstanceBegin(InstanceNode node)
      {
         return RenderNodeAction.Proceed;
      }

      public void OnInstanceEnd(InstanceNode node)
      {
      }

      public RenderNodeAction OnLinkBegin(LinkNode node)
      {
         return RenderNodeAction.Proceed;
      }

      public void OnLinkEnd(LinkNode node)
      {
      }

      public RenderNodeAction OnElementBegin(ElementId elementId)
      {
         return RenderNodeAction.Skip;
      }

      public void OnElementEnd(ElementId elementId)
      {
      }

      public RenderNodeAction OnElementBegin2D(ElementNode node)
      {
         m_numElements++;

         m_currentElem = node.Document.GetElement(node.ElementId);

         return RenderNodeAction.Proceed;
      }

      public void OnElementEnd2D(ElementNode node)
      {
         m_currentElem = null;
      }


      public RenderNodeAction OnFaceBegin(FaceNode node)
      {
         return RenderNodeAction.Proceed;
      }

      public void OnFaceEnd(FaceNode node)
      {
      }

      public RenderNodeAction OnCurve(CurveNode node)
      {
         // tessellate annotations OnCurve to support Wireframe annotation export
         if (m_currentElem.Category.CategoryType == CategoryType.Annotation)
         {
            IList<XYZ> list = new List<XYZ>();

            Curve curve = node.GetCurve();
            if (curve is Line)
            {
               Line l = curve as Line;
               list.Add(l.GetEndPoint(0));
               list.Add(l.GetEndPoint(1));
            }
            else
            {
               list = curve.Tessellate();
            }

            Utilities.addTo(m_points, list);
         }

         return RenderNodeAction.Proceed;
      }

      public RenderNodeAction OnPolyline(PolylineNode node)
      {
         // tessellate annotations OnPolyline to support Wireframe annotation export
         if (m_currentElem.Category.CategoryType == CategoryType.Annotation)
         {
            PolyLine pLine = node.GetPolyline();
            IList<XYZ> list = pLine.GetCoordinates();
            Utilities.addTo(m_points, list);
         }

         return RenderNodeAction.Proceed;
      }

      public RenderNodeAction OnFaceEdge2D(FaceEdgeNode node)
      {
         // tessellate annotations OnFaceEdge2D to support Wireframe annotation export
         //if (m_currentElem.Category.CategoryType == CategoryType.Annotation)
         //{
         //   Curve curve = node.GetFaceEdge().AsCurve();
         //   if (curve != null)
         //   {
         //      curve = curve.CreateTransformed(node.GetInstanceTransform());
         //      IList<XYZ> list = curve.Tessellate();
         //      Utilities.addTo(m_points, list);
         //   }
         //}
         return RenderNodeAction.Proceed;
      }

      public RenderNodeAction OnFaceSilhouette2D(FaceSilhouetteNode node)
      {
         return RenderNodeAction.Proceed;
      }

      public void OnText(Autodesk.Revit.DB.TextNode node)
      {
         m_texts += "\n" + node.Text;
         ++m_numTexts;
      }

      public void OnLight(LightNode node)
      {
      }

      public void OnRPC(RPCNode node)
      {
      }

      public void OnMaterial(MaterialNode node)
      {
      }

      public void OnPolymesh(PolymeshTopology node)
      {
      }

      public void OnLineSegment(LineSegment segment)
      {
         XYZ segmentStart = segment.StartPoint;
         XYZ segmentEnd = segment.EndPoint;

         IList<XYZ> list = new List<XYZ>();
         list.Add(segmentStart);
         list.Add(segmentEnd);
         Utilities.addTo(m_points, list);
      }

      public void OnPolylineSegments(PolylineSegments segments)
      {
         IList<XYZ> segPoints = segments.GetVertices();
         Utilities.addTo(m_points, segPoints);
      }
	  #endregion
   }
}
