using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.EditSketch.CS
{
   internal class WallSketchEditor
   {
      private Document document;
      private Wall wall;
      

      /// <summary>
      /// constructor
      /// </summary>
      /// 
      public WallSketchEditor(Document document, Wall wall)
      {
         this.document = document;
         this.wall = wall;
      }

      // Must call a Failures Preprocessor Class to exit a SketchEditScope
      class EmptyFailuresPreprocessor : Autodesk.Revit.DB.IFailuresPreprocessor
      {
         public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
         {
            return FailureProcessingResult.Continue;
         }

      }

      public void ReplaceTopOfWallWithArc()
      {

         // Opening a sketch edit scope to prepare to edit wall sketch			
         using (SketchEditScope editscope = new SketchEditScope(this.document, "Adding Hole In Wall"))
         {
            Parameter h = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);
            double height = h.AsDouble();

            // Begin Editing profile sketch of the wall
            editscope.Start(wall.SketchId);
            Sketch sketch = this.document.GetElement(wall.SketchId) as Sketch;

            // End1/0 will be the ends of the line we are replacing with an arc
            XYZ end0 = null;
            XYZ end1 = null;
            double width = 0;

            using (Transaction t = new Transaction(this.document, "Adding Top Arc to the wall"))
            {

               t.Start();

               // Retrieve the curves of the sketch
               // The sketch is defined by the Profile or the sequence of closed loops (CurveArrArray)
               // These closed loops are further defined by the curves that comprise them (CurveArray)
               CurveArrArray closedLoops = sketch.Profile;
               foreach (CurveArray loop in closedLoops)
               {
                  foreach (Curve curve in loop)
                  {
                     Line line = curve as Line;

                     // The top line will have a starting height equal to the height of the wall
                     if (line.GetEndPoint(0).Z == height)
                     {

                        // For adding the arc
                        end0 = line.GetEndPoint(0);
                        end1 = line.GetEndPoint(1);
                        width = line.Length;

                        // Delete existing top wall line
                        this.document.Delete(line.Reference.ElementId);
                        break;
                     }
                  }
               }

               // Add arc to top of wall. 
               XYZ pointOnCurve = new XYZ(end0.X - width / 2, end0.Y, end0.Z + width / 2);
               Arc arc = Arc.Create(end0, end1, pointOnCurve);

               this.document.Create.NewModelCurve(arc, sketch.SketchPlane);

               t.Commit();
            }

            editscope.Commit(new EmptyFailuresPreprocessor());
         }
      }

      public void AddHoleToSketch(XYZ circleOrigin, double circleRadius)
      {
         using (SketchEditScope editscope = new SketchEditScope(this.document, "Adding Hole In Wall"))
         {
           
            // Begin Editing profile sketch of the wall
            editscope.Start(wall.SketchId);
            Sketch sketch = this.document.GetElement(wall.SketchId) as Sketch;

            using (Transaction t = new Transaction(this.document, "Creating Hole in Wall Profile Sketch"))
            {

               t.Start();

               // Add hole with the center being the center of the arc
               Curve circle = Ellipse.CreateCurve(circleOrigin, circleRadius, circleRadius, XYZ.BasisX, XYZ.BasisZ, -Math.PI, Math.PI);
               this.document.Create.NewModelCurve(circle, sketch.SketchPlane);

               t.Commit();
            }

            editscope.Commit(new EmptyFailuresPreprocessor());
         }
      }

      // Assuming wall has a top arc and a free floating circle in the middle
      public void AddLinearDimensionBetweenArcsInSketch()
      {

         using (SketchEditScope editscope = new SketchEditScope(document, "Adding linear dimension"))
         {
            
            Parameter h = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);
            double height = h.AsDouble();
            double circleRadius = 0;

            // Begin Editing profile sketch of the wall
            editscope.Start(wall.SketchId);
            Sketch sketch = document.GetElement(wall.SketchId) as Sketch;

            using (Transaction t = new Transaction(document, "Creating dimension between arcs"))
            {

               t.Start();

               // Retriving all the Model Lines of the sketch
               IList<ElementId> modelLineIds = sketch.GetAllElements();

               // We will use this in creating the linear dimension 
               Reference innerCircle = null;
               Reference topArc = null;

               foreach (ElementId id in modelLineIds)
               {
                  Element element = document.GetElement(id);

                  if (element is ModelArc)
                  {
                     ModelArc modelArc = element as ModelArc;

                     // This will help us distinguish between the inner circle and 
                     // Top arc, since we know that the inner circle touches no elements
                     if (modelArc.GetAdjoinedCurveElements(0).Count() == 0)
                     {
                        innerCircle = new Reference(element);
                        circleRadius = modelArc.GeometryCurve.ApproximateLength;

                     }
                     else
                     {
                        topArc = new Reference(element);
                     }
                  }
               }


               ReferenceArray rArray = new ReferenceArray();
               rArray.Insert(innerCircle, 0);
               rArray.Insert(topArc, 0);

               // Add dim between two references
               Dimension dimension = document.Create.NewDimension(null, Line.CreateBound(new XYZ(height / 2, 0, height / 2), new XYZ(height / 2, 0, height + circleRadius)), rArray);
               dimension.Suffix = "  Between Arcs";

               t.Commit();
            }

            editscope.Commit(new EmptyFailuresPreprocessor());
         }
      }

      public void AddAngularDimensionToWallBase()
      {
         using (SketchEditScope editscope = new SketchEditScope(document, "Adding linear dimension"))
         {
            
            Parameter h = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);

            // Begin Editing profile sketch of the wall
            editscope.Start(wall.SketchId);
            Sketch sketch = document.GetElement(wall.SketchId) as Sketch;

            using (Transaction t = new Transaction(document, "Creating dimension between arcs"))
            {

               t.Start();

               // Retrieving all the Model Lines of the sketch
               IList<ElementId> modelLineIds = sketch.GetAllElements();

               Reference rLine1 = null;
               Reference rLine2 = null;

               foreach (ElementId id in modelLineIds)
               {
                  Element element = document.GetElement(id);
                  if (element is ModelLine)
                  {
                     // Select the first two model lines
                     ModelLine modelLine = element as ModelLine;
                     if (rLine1 == null)
                     {
                        rLine1 = modelLine.GeometryCurve.Reference;
                     }
                     else if (rLine2 == null)
                     {
                        rLine2 = modelLine.GeometryCurve.Reference;
                     }
                     else
                     {
                        break;
                     }

                  }
               }

               IList<Reference> references = new List<Reference>();
               references.Add(rLine1);
               references.Add(rLine2);

               // This arc determines the size of the dimension
               XYZ arcTop = new XYZ(2, 0, 2);
               XYZ arcStart = new XYZ(0, 0, 1);
               XYZ arcEnd = new XYZ(1, 0, 0);
               Arc a = Arc.Create(arcStart, arcEnd, arcTop);

               // Getting default angular dimension tyoe
               DimensionType type = new FilteredElementCollector(document)
             .OfClass(typeof(DimensionType)).Cast<DimensionType>()
             .FirstOrDefault(x => x.StyleType == DimensionStyleType.Angular);

               // Creating angular dimension 
               AngularDimension ad = AngularDimension.Create(document, document.ActiveView, a, references, type);

               t.Commit();
            }

            editscope.Commit(new EmptyFailuresPreprocessor());
         }
      }


   }
}
