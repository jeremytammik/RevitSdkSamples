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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.Samples.DirectionCalculation
{
    /// <summary>
    /// Implementation class for utilities to find south facing windows in a project.
    /// </summary>
    public class FindSouthFacingWindows : FindSouthFacingBase
    {
        /// <summary>
        /// The implementation of the FindSouthFacingWindows command.
        /// </summary>
        /// <param name="useProjectLocationNorth">true to use the active project location's north/south direction.  
        /// false to use the default coordinate system's north/south (Y-axis).</param>
        protected void Execute(bool useProjectLocationNorth)
        {
           UIDocument uiDoc = new UIDocument(Document);
           Autodesk.Revit.DB.ElementSet selElements = new ElementSet();
           foreach (ElementId elementId in uiDoc.Selection.GetElementIds())
           {
              selElements.Insert(uiDoc.Document.GetElement(elementId));
           }

            IEnumerable<FamilyInstance> windows = CollectWindows();
            foreach (FamilyInstance window in windows)
            {
                XYZ exteriorDirection = GetWindowDirection(window);

                if (useProjectLocationNorth)
                    exteriorDirection = TransformByProjectLocation(exteriorDirection);

                bool isSouthFacing = IsSouthFacing(exteriorDirection);
                if (isSouthFacing)
                    selElements.Insert(window);
            }

            // Select all windows which had the proper direction.
           List<ElementId> elemIdList = new List<ElementId>();
           foreach(Element element in selElements)
           {
              elemIdList.Add(element.Id);
           }
           uiDoc.Selection.SetElementIds(elemIdList);
        }

        /// <summary>
        /// Finds all windows in the active document.
        /// </summary>
        /// <returns>An enumerable containing all windows.</returns>
        protected IEnumerable<FamilyInstance> CollectWindows()
        {
            // Windows are family instances whose category is correctly set.

            ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter windowCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
            LogicalAndFilter andFilter = new LogicalAndFilter(familyInstanceFilter, windowCategoryFilter);
            FilteredElementCollector collector = new FilteredElementCollector(Document); 
           ICollection<Element> elementsToProcess = collector.WherePasses(andFilter).ToElements();

            // Convert to IEnumerable of FamilyInstance using LINQ
            IEnumerable<FamilyInstance> windows = from window in elementsToProcess.Cast<FamilyInstance>() select window;

            return windows;
        }

        /// <summary>
        /// Obtains the facing direction of the window.
        /// </summary>
        /// <param name="wall">The window.</param>
        /// <returns>A normalized XYZ direction vector.</returns>
        protected XYZ GetWindowDirection(FamilyInstance window)
        {
            Options options = new Options();

            // Extract the geometry of the window.
            GeometryElement geomElem = window.get_Geometry(options);

            //foreach (GeometryObject geomObj in geomElem.Objects)
            IEnumerator<GeometryObject> Objects = geomElem.GetEnumerator();
            while(Objects.MoveNext())
            {
               GeometryObject geomObj = Objects.Current;

                // We expect there to be one main Instance in each window.  Ignore the rest of the geometry.
                GeometryInstance instance = geomObj as GeometryInstance;
                if (instance != null)
                {
                    // Obtain the Instance's transform and the nominal facing direction (Y-direction).
                    Transform t = instance.Transform;
                    XYZ facingDirection = t.BasisY;

                    // If the window is flipped in one direction, but not the other, the transform is left handed.  
                    // The Y direction needs to be reversed to obtain the facing direction.
                    if ((window.FacingFlipped && !window.HandFlipped) || (!window.FacingFlipped && window.HandFlipped))
                        facingDirection = -facingDirection;

                    // Because the need to perform this operation on instances is so common, 
                    // the Revit API exposes this calculation directly as the FacingOrientation property 
                    // as shown in GetWindowDirectionAlternate()
 
                    return facingDirection;
                }
            }
            return XYZ.BasisZ;
        }

        /// <summary>
        /// Alternate way to obtain the facing direction for the window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns>A normalized XYZ direction vector.</returns>
        protected XYZ GetWindowDirectionAlternate(FamilyInstance window)
        {
            return window.FacingOrientation;
        }

    }
}
