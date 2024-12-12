using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.Samples.DirectionCalculation
{
    /// <summary>
    /// Implementation class for utilities to find south facing exterior walls in a project.
    /// </summary>
    /// 
      [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
      [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

   public class FindSouthFacingWalls : FindSouthFacingBase
    {
        /// <summary>
        /// The implementation of the FindSouthFacingWalls command.
        /// </summary>
        /// <param name="useProjectLocationNorth">true to use the active project location's north/south direction.  
        /// false to use the default coordinate system's north/south (Y-axis).</param>
        protected void Execute(bool useProjectLocationNorth)
        {
           UIDocument uiDoc = new UIDocument(Document);
            Autodesk.Revit.UI.Selection.SelElementSet selElements = uiDoc.Selection.Elements;

            IEnumerable<Wall> walls = CollectExteriorWalls();
            foreach (Wall wall in walls)
            {
                XYZ exteriorDirection = GetExteriorWallDirection(wall);

                if (useProjectLocationNorth)
                    exteriorDirection = TransformByProjectLocation(exteriorDirection);

                bool isSouthFacing = IsSouthFacing(exteriorDirection);
                if (isSouthFacing)
                    selElements.Insert(wall);
            }

            // Select all walls which had the proper direction.
            uiDoc.Selection.Elements = selElements;
        }

        /// <summary>
        /// Finds all exterior walls in the active document.
        /// </summary>
        /// <returns>An enumerable containing exterior walls.</returns>
        protected IEnumerable<Wall> CollectExteriorWalls()
        {
            FilteredElementCollector collector = new FilteredElementCollector(Document);
            IList<Element> elementsToProcess = collector.OfClass(typeof(Wall)).ToElements();
            // Use a LINQ query to filter out only Exterior walls
            IEnumerable<Wall> exteriorWalls = from wall in elementsToProcess.Cast<Wall>()
                                              where IsExterior(Document.GetElement(wall.GetTypeId()) as ElementType)
                                              select wall;
            return exteriorWalls;
        }

        /// <summary>
        /// Test method to see if the wall type is exterior.
        /// </summary>
        /// <param name="wallType">The wall type.</param>
        /// <returns>true if the wall is exterior, else false.</returns>
        protected bool IsExterior(ElementType wallType)
        {
            Parameter wallFunction = wallType.get_Parameter(BuiltInParameter.FUNCTION_PARAM);

            WallFunction value = (WallFunction)wallFunction.AsInteger();

            return (value == WallFunction.Exterior);
        }

        /// <summary>
        /// Obtains the outward direction of the exterior wall.
        /// </summary>
        /// <param name="wall">The wall.</param>
        /// <returns>A normalized XYZ direction vector.</returns>
        protected XYZ GetExteriorWallDirection(Wall wall)
        {
            LocationCurve locationCurve = wall.Location as LocationCurve;
            XYZ exteriorDirection = XYZ.BasisZ;

            if (locationCurve != null)
            {
                Curve curve = locationCurve.Curve;

                //Write("Wall line endpoints: ", curve);

                XYZ direction = XYZ.BasisX;
                if (curve is Line)
                {
                    // Obtains the tangent vector of the wall.
                    direction = curve.ComputeDerivatives(0, true).BasisX.Normalize();
                }
                else
                {
                    // An assumption, for non-linear walls, that the "tangent vector" is the direction
                    // from the start of the wall to the end.
                    direction = (curve.GetEndPoint(1) - curve.GetEndPoint(0)).Normalize();
                }
                // Calculate the normal vector via cross product.
                exteriorDirection = XYZ.BasisZ.CrossProduct(direction);

                // Flipped walls need to reverse the calculated direction
                if (wall.Flipped) exteriorDirection = -exteriorDirection;
            }

            return exteriorDirection;
        }


    }
}
