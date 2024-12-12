//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Elements;
using Instance = Autodesk.Revit.Geometry.Instance;

namespace Revit.SDK.Samples.DistanceToPanels.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand.
    /// This class shows how to compute the distance from divided surface panels to a user-specified point
    /// </summary>
    /// 
    public class SetDistanceParam : IExternalCommand
    {
        /// <summary>
        /// The Revit application instance
        /// </summary>
        Autodesk.Revit.Application m_app;
        /// <summary>
        /// The active Revit document
        /// </summary>
        Autodesk.Revit.Document m_doc;

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            m_app = commandData.Application;
            m_doc = m_app.ActiveDocument;

            // get the target element to be used for the Distance computation
            ElementSet collection = m_doc.Selection.Elements;
            Parameter param = null;
            XYZ targetPoint = getTargetPoint(m_doc.Selection.Elements);

            // get all the divided surfaces in the Revit document
            List<DividedSurface> dsList = GetElements<DividedSurface>();

            foreach (DividedSurface ds in dsList)
            {
                GridNode gn = new GridNode();
                int u = 0;
                while (u < ds.NumberOfUGridlines)
                {
                    gn.UIndex = u;
                    int v = 0;
                    while (v < ds.NumberOfVGridlines)
                    {
                        gn.VIndex = v;
                        if (ds.IsSeedNode(gn))
                        {
                            FamilyInstance familyinstance = ds.GetTileFamilyInstance(gn, 0);
                            if (familyinstance != null)
                            {
                                param = familyinstance.get_Parameter("Distance");
                                if (param == null) throw new Exception("Panel family must have a Distance instance parameter");
                                else
                                {
                                    Point geomobjPoint = ds.GetGridNodeReference(gn).GeometryObject as Point;
                                    XYZ panelPoint = geomobjPoint.Coord;
                                    double d = Math.Sqrt(Math.Pow((targetPoint.X - panelPoint.X), 2) + Math.Pow((targetPoint.Y - panelPoint.Y), 2) + Math.Pow((targetPoint.Z - panelPoint.Z), 2));
                                    param.Set(d);

                                    // uncomment the following lines to create points and lines showing where the distance measurement is made
                                    //ReferencePoint rp = m_doc.FamilyCreate.NewReferencePoint(panelPoint);
                                    //Line line = m_app.Create.NewLine(targetPoint, panelPoint, true);
                                    //Plane plane = m_app.Create.NewPlane(targetPoint.Cross(panelPoint), panelPoint);
                                    //SketchPlane skplane = m_doc.FamilyCreate.NewSketchPlane(plane);
                                    //ModelCurve modelcurve = m_doc.FamilyCreate.NewModelCurve(line, skplane);
                                }
                            }
                        }
                        v = v + 1;
                    }
                    u = u + 1;
                }
            }
            return IExternalCommand.Result.Succeeded;
        }

        /// <summary>
        /// Get the XYZ point of the selected target element
        /// </summary>
        /// <param name="collection">Selected elements</param>
        /// <returns>the XYZ point of the selected target element</returns>
        XYZ getTargetPoint(ElementSet collection)
        {
            FamilyInstance targetElement = null;
            if (collection.Size != 1)
            {
                throw new Exception("You must select one component from which the distance to panels will be measured");
            }
            else
            {
                foreach (Autodesk.Revit.Element e in collection)
                {
                    targetElement = e as FamilyInstance;
                }
            }

            if (null == targetElement)
            {
                throw new Exception("You must select one family instance from which the distance to panels will be measured");
            }
            LocationPoint targetLocation = targetElement.Location as LocationPoint;
            return targetLocation.Point;
        }

        /// <summary>
        /// Get all elements by Type
        /// </summary>
        /// <param name="T">The specified type</param>
        /// <returns>All the elements of that type</returns>
        private List<T> GetElements<T>() where T : Autodesk.Revit.Element
        {
            List<T> elements = new List<T>();
            ElementIterator eit = m_doc.get_Elements(m_app.Create.Filter.NewTypeFilter(typeof(T), true));
            eit.Reset();
            while (eit.MoveNext())
            {
                T element = eit.Current as T;
                if (element != null)
                {
                    elements.Add(element);
                }
            }
            return elements;
        }
    }
}
