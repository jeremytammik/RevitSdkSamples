using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.Custom2DExporter.CS
{
   /// <summary>
   /// Collection of helpful utility classes. 
   /// </summary>
   public class Utilities
   {
      /// <summary>
      /// Utility to properly append points from 1 list to another. 
      /// </summary>
      /// <param name="to"></param>
      /// <param name="from"></param>
      public static void addTo(IList<XYZ> to, IList<XYZ> from)
      {
         int cnt = from.Count;
         for (int ii = 0; ii < cnt; ii++)
         {
            if (ii < cnt - 1)
            {
               XYZ p0 = from[ii];
               XYZ p1 = from[ii + 1];
               to.Add(p0);
               to.Add(p1);
            }
         }
      }

      static Plane getAppropriatePlane(View view)
      {
         Plane plane = Plane.CreateByNormalAndOrigin(view.ViewDirection, view.Origin);
         return plane;
      }

      /// <summary>
      /// Draw all lines defined by the list of (start, end) points, which are bigger than the given tolerance.
      /// The lines are drawn as detail lines (DetailLine).
      /// </summary>
      /// <param name="view">The view to draw the lines in.</param>
      /// <param name="points">The list of (start, end) points of the lines.</param>
      /// <param name="tolerance">The smallest curve length allowed to be drawn. Lines smaller than this are not drawn.</param>
      static void drawLines(View view, IList<XYZ> points, double tolerance)
      {
         Plane plane = getAppropriatePlane(view);
         if (plane != null)
         {
            for (int ii = 0; ii < points.Count; ii++)
            {
               UV uvStart, uvEnd;
               double distance = double.MaxValue;
               plane.Project(points[ii], out uvStart, out distance);
               plane.Project(points[ii + 1], out uvEnd, out distance);

               XYZ projectionStart;
               XYZ projectionEnd;
               projectionStart = uvStart.U * plane.XVec + uvStart.V * plane.YVec + plane.Origin;
               projectionEnd = uvEnd.U * plane.XVec + uvEnd.V * plane.YVec + plane.Origin;

               if (projectionStart.DistanceTo(projectionEnd) < tolerance)
               {
                  ii++;
                  continue;
               }

               Line geomLine = Line.CreateBound(projectionStart, projectionEnd);
               DetailLine line = view.Document.Create.NewDetailCurve(view, geomLine) as DetailLine;

               ii++;
            }
         }
      }

      /// <summary>
      /// Hide all elements visibe in the view.
      /// </summary>
      /// <param name="view">The view</param>
      static void hideAllInView(View view)
      {
         FilteredElementCollector viewElems = new FilteredElementCollector(view.Document, view.Id);
         ICollection<ElementId> elementIds = viewElems.ToElementIds();
         ICollection<ElementId> hideElemIds = new List<ElementId>();
         foreach (ElementId id in elementIds)
         {
            if (view.Document.GetElement(id).CanBeHidden(view))
               hideElemIds.Add(id);
         }

         view.HideElements(hideElemIds);
      }

      /// <summary>
      /// Displays the exported list of points by drawing detail lines with (start, end) defined by the points.
      /// All elements besides the drawn lines will be hidden.
      /// </summary>
      /// <param name="view">The view in which to draw lines and hide elements.</param>
      /// <param name="points">The points which define the lines (start, end) points</param>
      public static void displayExport(View view, IList<XYZ> points)
      {
         Document doc = view.Document;
         using (Transaction tran = new Transaction(view.Document, "ExportViewGeometry"))
         {
            tran.Start("Draw Exported Lines and turn off everything but Lines");
            hideAllInView(view);
            drawLines(view, points, doc.Application.ShortCurveTolerance);
            tran.Commit();
         }
      }
   }
}
