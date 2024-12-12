//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Diagnostics;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.FreeFormElement.CS
{
    /// <summary>
    /// A command to create a new family block representing a negative of a selected element. 
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CreateNegativeBlockCommand : IExternalCommand
    {
        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
			
            // Select target element
            Reference target = uiDoc.Selection.PickObject(ObjectType.Element,
                                                          new TargetElementSelectionFilter(),
                                                          "Select target");
            Element targetElement = doc.GetElement(target);

            // Get height for block based on target element.
            BoundingBoxXYZ bbox = targetElement.get_BoundingBox(null);
            double height = bbox.Max.Z - bbox.Min.Z + 1;

            // Select boundaries
            IList<Reference> boundaries = uiDoc.Selection.PickObjects(ObjectType.Element,
                                                                      new BoundarySelectionFilter(),
                                                                      "Select boundary");
			String familyPath = FreeFormElementUtils.FindGenericModelTemplate(doc.Application.FamilyTemplatePath);

            if (String.IsNullOrEmpty(familyPath))
            {
				message = "Unable to find a template named 'GenericModel.rft' in family template path.";
				return Result.Failed;
            }

            FreeFormElementUtils.FailureCondition condition = FreeFormElementUtils.CreateNegativeBlock(targetElement, boundaries, UIDocument.GetRevitUIFamilyLoadOptions(), familyPath);

            // Show error message for failure condition
            if (condition != FreeFormElementUtils.FailureCondition.Success)
            {
                switch (condition)
                {
                    case FreeFormElementUtils.FailureCondition.CurvesNotContigous:
                        message = "Could not create the block as the boundary curves do not make a contiguous closed boundary.";
                        break;
                    case FreeFormElementUtils.FailureCondition.CurveLoopAboveTarget:
                        message = "Could not create the block as the boundary curves lie above their target element.";
                        break;
                    case FreeFormElementUtils.FailureCondition.NoIntersection:
                        message = "Could not create the block as the curves and the target element does not intersect.";
                        break;
                }
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        #endregion
    }

    /// <summary>
    /// Selection filter for selection of a target object to use as a template for the negative block.
    /// </summary>
    class TargetElementSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            // Element must have at least one usable solid
            IList<Solid> solids = FreeFormElementUtils.GetTargetSolids(element);

            return solids.Count > 0;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return true;
        }
    }

    /// <summary>
    /// Selection filter for selection of the boundary curves for the block extents.
    /// </summary>
    class BoundarySelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            // Allow only curve elements
            CurveElement curveElement = element as CurveElement;
            if (curveElement == null)
                return false;

            Curve curve = curveElement.GeometryCurve;

            // Curves must support the utilities used by the tool (e.g. ReverseCurve)
            if (!FreeFormElementUtils.SupportsLoopUtilities(curve))
                return false;

            // Curves must be in XY plane
            return FreeFormElementUtils.IsCurveInXYPlane(curve);
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return true;
        }
    }
}
