//
// (C) Copyright 2003-2017 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Autodesk.Revit.DB;

using RevitFreeFormElement = Autodesk.Revit.DB.FreeFormElement;

namespace Revit.SDK.Samples.FreeFormElement.CS
{
    /// <summary>
    /// Utilities supporting the creation of a family containing a FreeFormElement which is cut out from existing geometry
    /// </summary>
    class FreeFormElementUtils
    {
        public enum FailureCondition
        {
            Success,
            CurvesNotContigous,
            CurveLoopAboveTarget,
            NoIntersection
        };

        /// <summary>
        /// Creates a negative block family from the geometry of the target element and boundaries.
        /// </summary>
        /// <remarks>This is the main implementation of the sample command.</remarks>
        /// <param name="targetElement">The target solid element.</param>
        /// <param name="boundaries">The selected curve element boundaries.</param>
        /// <param name="familyLoadOptions">The family load options when loading the new family.</param>
        /// <param name="familyTemplate">The family template.</param>
        public static FailureCondition CreateNegativeBlock(Element targetElement, IList<Reference> boundaries, IFamilyLoadOptions familyLoadOptions, String familyTemplate)
        {
            Document doc = targetElement.Document;
            Autodesk.Revit.ApplicationServices.Application app = doc.Application;

            // Get curve loop for boundary
            IList<Curve> curves = GetContiguousCurvesFromSelectedCurveElements(doc, boundaries);
            CurveLoop loop = null;
            try
            {
                loop = CurveLoop.Create(curves);
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                // Curves are not contiguous
                return FailureCondition.CurvesNotContigous;
            }
            List<CurveLoop> loops = new List<CurveLoop>();
            loops.Add(loop);

            // Get elevation of loop
            double elevation = curves[0].GetEndPoint(0).Z;

            // Get height for extrusion
            BoundingBoxXYZ bbox = targetElement.get_BoundingBox(null);
            double height = bbox.Max.Z - elevation;

            if (height <= 1e-5)
                return FailureCondition.CurveLoopAboveTarget;

            height += 1;
            
            // Create family
            Document familyDoc = app.NewFamilyDocument(familyTemplate);

            // Create block from boundaries
            Solid block = GeometryCreationUtilities.CreateExtrusionGeometry(loops, XYZ.BasisZ, height);

            // Subtract target element
            IList<Solid> fromElement = GetTargetSolids(targetElement);

            int solidCount = fromElement.Count;

            // Merge all found solids into single one
            Solid toSubtract = null;
            if (solidCount == 1)
            {
                toSubtract = fromElement[0];
            }

            else if (solidCount > 1)
            {
                toSubtract =
                    BooleanOperationsUtils.ExecuteBooleanOperation(fromElement[0], fromElement[1], BooleanOperationsType.Union);
            }

            if (solidCount > 2)
            {
                for (int i = 2; i < solidCount; i++)
                {
                    toSubtract = BooleanOperationsUtils.ExecuteBooleanOperation(toSubtract, fromElement[i],
                                                                                         BooleanOperationsType.Union);
                }
            }

            // Subtract merged solid from overall block
            try
            {
                BooleanOperationsUtils.ExecuteBooleanOperationModifyingOriginalSolid(block, toSubtract,
                                                                                         BooleanOperationsType.Difference);
            }
            catch (Autodesk.Revit.Exceptions.InvalidOperationException)
            {
                return FailureCondition.NoIntersection;
            }

            // Create freeform element
            using (Transaction t = new Transaction(familyDoc, "Add element"))
            {
                t.Start();
                RevitFreeFormElement element = Autodesk.Revit.DB.FreeFormElement.Create(familyDoc, block);
                t.Commit();
            }

            // Load family into document
            Family family = familyDoc.LoadFamily(doc, familyLoadOptions);

            familyDoc.Close(false);

            // Get symbol as first symbol of loaded family
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.WherePasses(new FamilySymbolFilter(family.Id));
            FamilySymbol fs = collector.FirstElement() as FamilySymbol;


            // Place instance at location of original curves
            using (Transaction t2 = new Transaction(doc, "Place instance"))
            {
                t2.Start();
                if (!fs.IsActive)
                   fs.Activate();
                doc.Create.NewFamilyInstance(XYZ.Zero, fs, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                t2.Commit();
            }

            return FailureCondition.Success;
        }

        /// <summary>
        /// Gets a list of curves which are ordered correctly and oriented correctly to form a closed loop.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="boundaries">The list of curve element references which are the boundaries.</param>
        /// <returns>The list of curves.</returns>
        public static IList<Curve> GetContiguousCurvesFromSelectedCurveElements(Document doc, IList<Reference> boundaries)
        {
            List<Curve> curves = new List<Curve>();

            // Build a list of curves from the curve elements
            foreach (Reference reference in boundaries)
            {
                CurveElement curveElement = doc.GetElement(reference) as CurveElement;
                curves.Add(curveElement.GeometryCurve.Clone());
            }

            // Walk through each curve (after the first) to match up the curves in order
            for (int i = 0; i < curves.Count; i++)
            {
                Curve curve = curves[i];
                XYZ endPoint = curve.GetEndPoint(1);

                // find curve with start point = end point
                for (int j = i + 1; j < curves.Count; j++)
                {
                    // Is there a match end->start, if so this is the next curve
                    if (curves[j].GetEndPoint(0).IsAlmostEqualTo(endPoint, 1e-05))
                    {
                        Curve tmpCurve = curves[i + 1];
                        curves[i + 1] = curves[j];
                        curves[j] = tmpCurve;
                        continue;
                    }
                    // Is there a match end->end, if so, reverse the next curve
                    else if (curves[j].GetEndPoint(1).IsAlmostEqualTo(endPoint, 1e-05))
                    {
                        Curve tmpCurve = curves[i + 1];
                        curves[i + 1] = CreateReversedCurve(curves[j]);
                        curves[j] = tmpCurve;
                        continue;
                    }
                }
            }

            return curves;
        }

        /// <summary>
        /// Utility to create a new curve with the same geometry but in the reverse direction.
        /// </summary>
        /// <param name="orig">The original curve.</param>
        /// <returns>The reversed curve.</returns>
        /// <throws cref="NotImplementedException">If the curve type is not supported by this utility.</throws>
        private static Curve CreateReversedCurve(Curve orig)
        {
            if (!SupportsLoopUtilities(orig))
            {
                throw new NotImplementedException("CreateReversedCurve for type " + orig.GetType().Name);
            }

            if (orig is Line)
            {
                return Line.CreateBound(orig.GetEndPoint(1), orig.GetEndPoint(0));
            }
            else if (orig is Arc)
            {
                return Arc.Create(orig.GetEndPoint(1), orig.GetEndPoint(0), orig.Evaluate(0.5, true));
            }
            else
            {
                throw new Exception("CreateReversedCurve - Unreachable");
            }

        }

        /// <summary>
        /// Identifies if the curve type is supported in these utilities.
        /// </summary>
        /// <param name="curve">The curve.</param>
        /// <returns>True if the curve type is supported, false otherwise.</returns>
        public static bool SupportsLoopUtilities(Curve curve)
        {
            return curve is Line || curve is Arc;
        }

        /// <summary>
        /// Identifies if the curve lies entirely in an XY plane (Z = constant)
        /// </summary>
        /// <param name="curve">The curve.</param>
        /// <returns>True if the curve lies in an XY plane, false otherwise.</returns>
        public static bool IsCurveInXYPlane(Curve curve)
        {
            // quick reject - are endpoints at same Z
            double zDelta = curve.GetEndPoint(1).Z - curve.GetEndPoint(0).Z;

            if (Math.Abs(zDelta) > 1e-05)
                return false;

            if (!(curve is Line) && !curve.IsCyclic)
            {
                // Create curve loop from curve and connecting line to get plane
                List<Curve> curves = new List<Curve>();
                curves.Add(curve);
                curves.Add(Line.CreateBound(curve.GetEndPoint(1), curve.GetEndPoint(0)));
                CurveLoop curveLoop = CurveLoop.Create(curves);

                XYZ normal = curveLoop.GetPlane().Normal.Normalize();
                if (!normal.IsAlmostEqualTo(XYZ.BasisZ) && !normal.IsAlmostEqualTo(XYZ.BasisZ.Negate()))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets all target solids in a given element.
        /// </summary>
        /// <remarks>Includes solids and solids in first level instances only.  Deeper levels are ignored.  Empty solids are not returned.</remarks>
        /// <param name="element">The element.</param>
        /// <returns>The list of solids.</returns>
        public static IList<Solid> GetTargetSolids(Element element)
        {
            List<Solid> solids = new List<Solid>();


            Options options = new Options();
            options.DetailLevel = ViewDetailLevel.Fine;
            GeometryElement geomElem = element.get_Geometry(options);
            foreach (GeometryObject geomObj in geomElem)
            {
                if (geomObj is Solid)
                {
                    Solid solid = (Solid)geomObj;
                    if (solid.Faces.Size > 0 && solid.Volume > 0.0)
                    {
                        solids.Add(solid);
                    }
                    // Single-level recursive check of instances. If viable solids are more than
                    // one level deep, this example ignores them.
                }
                else if (geomObj is GeometryInstance)
                {
                    GeometryInstance geomInst = (GeometryInstance)geomObj;
                    GeometryElement instGeomElem = geomInst.GetInstanceGeometry();
                    foreach (GeometryObject instGeomObj in instGeomElem)
                    {
                        if (instGeomObj is Solid)
                        {
                            Solid solid = (Solid)instGeomObj;
                            if (solid.Faces.Size > 0 && solid.Volume > 0.0)
                            {
                                solids.Add(solid);
                            }
                        }
                    }
                }
            }
            return solids;
        }

        /// <summary>
        /// Finds the Generic Model template from the family template directory path, if it exists.
        /// </summary>
        /// <param name="familyPath">The family template directory path.</param>
        /// <returns>The template path, or empty string if not found.</returns>
        public static String FindGenericModelTemplate(String familyPath)
        {
			string hardCodedResult = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(CreateNegativeBlockCommand).Assembly.Location), "Generic Model.rft");
			try
			{
				IEnumerable<string> files = Directory.EnumerateFiles(familyPath, "Generic Model.rft", SearchOption.AllDirectories);

				string result = files.FirstOrDefault<string>();
				if (!String.IsNullOrEmpty(result))
					return result;

				files = Directory.EnumerateFiles(familyPath, "Metric Generic Model.rft", SearchOption.AllDirectories);

				result = files.FirstOrDefault<string>();
				if (!String.IsNullOrEmpty(result))
					return result;
				return hardCodedResult;
			}
			catch (Exception)
			{
				return hardCodedResult;
			}
        }
    }
}
