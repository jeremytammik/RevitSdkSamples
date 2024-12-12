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
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Elements;
using Autodesk.Revit;

namespace Revit.SDK.Samples.WindowWizard.CS
{
    /// <summary>
    /// The class is used to create solid extrusion 
    /// </summary>
    class CreateExtrusion
    {
        #region Class Memeber Variables
        /// <summary>
        /// store the document
        /// </summary>
        Document m_document;

        /// <summary>
        /// store the application of creation
        /// </summary>
        Autodesk.Revit.Creation.Application m_appCreator;

        /// <summary>
        /// store the FamilyItemFactory of creation
        /// </summary>
        Autodesk.Revit.Creation.FamilyItemFactory m_familyCreator;
        #endregion

        /// <summary>
        /// The constructor of CreateExtrusion
        /// </summary>
        /// <param name="app">the application</param>
        /// <param name="doc">the document</param>
        public CreateExtrusion(Application app, Document doc)
        {
            m_document = doc;
            m_appCreator = app.Create;
            m_familyCreator = doc.FamilyCreate;
        }

        #region Class Implementation
        /// <summary>
        /// The method is used to create a CurveArray with four double parameters and one y coordinate value
        /// </summary>
        /// <param name="left">the left value</param>
        /// <param name="right">the right value</param>
        /// <param name="top">the top value</param>
        /// <param name="bottom">the bottom value</param>
        /// <param name="y_coordinate">the y_coordinate value</param>
        /// <returns>CurveArray</returns>
        public CurveArray CreateRectangle(double left, double right, double top, double bottom, double y_coordinate)
        {
            CurveArray curveArray = m_appCreator.NewCurveArray();
            try
            {
                XYZ p0 = new XYZ(left, y_coordinate, top);
                XYZ p1 = new XYZ(right, y_coordinate, top);
                XYZ p2 = new XYZ(right, y_coordinate, bottom);
                XYZ p3 = new XYZ(left, y_coordinate, bottom);
                Line line1 = m_appCreator.NewLineBound(p0, p1);
                Line line2 = m_appCreator.NewLineBound(p1, p2);
                Line line3 = m_appCreator.NewLineBound(p2, p3);
                Line line4 = m_appCreator.NewLineBound(p3, p0);
                curveArray.Append(line1);
                curveArray.Append(line2);
                curveArray.Append(line3);
                curveArray.Append(line4); 
                return curveArray;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }
 
        /// <summary>
        /// The method is used to create a CurveArray along to an origin CurveArray and an offset value
        /// </summary>
        /// <param name="origin">the original CurveArray</param>
        /// <param name="offset">the offset value</param>
        /// <returns>CurveArray</returns>
        public CurveArray CreateCurveArrayByOffset(CurveArray origin, double offset)
        {
            Line line;
            Line temp;
            int counter = 0;
            CurveArray curveArr = m_appCreator.NewCurveArray();
            XYZ offsetx = new XYZ(offset, 0, 0);
            XYZ offsetz = new XYZ(0, 0, offset);
            XYZ p0 = new XYZ();
            XYZ p1 = new XYZ(); ;
            XYZ p2 = new XYZ();
            XYZ p3 = new XYZ();
            foreach (Curve curve in origin)
            {
                temp = curve as Line;
                if (temp != null)
                {
                    if (counter == 0)
                    {
                        p0 = temp.get_EndPoint(0).Subtract(offsetz).Subtract(offsetx);
                    }
                    else if (counter == 1)
                    {
                        p1 = temp.get_EndPoint(0).Subtract(offsetz).Add(offsetx);
                    }
                    else if (counter == 2)
                    {
                        p2 = temp.get_EndPoint(0).Add(offsetx).Add(offsetz);
                    }
                    else
                    {
                        p3 = temp.get_EndPoint(0).Subtract(offsetx).Add(offsetz);
                    }
                }
                counter++;
            }
            line = m_appCreator.NewLineBound(p0, p1);
            curveArr.Append(line);
            line = m_appCreator.NewLineBound(p1, p2);
            curveArr.Append(line);
            line = m_appCreator.NewLineBound(p2, p3);
            curveArr.Append(line);
            line = m_appCreator.NewLineBound(p3, p0);
            curveArr.Append(line);
            return curveArr;
        }

        /// <summary>
        /// The method is used to create extrusion using FamilyItemFactory.NewExtrusion()
        /// </summary>
        /// <param name="curveArrArray">the CurveArrArray parameter</param>
        /// <param name="workPlane">the reference plane is used to create SketchPlane</param>
        /// <param name="startOffset">the extrusion's StartOffset property</param>
        /// <param name="endOffset">the extrusion's EndOffset property</param>
        /// <returns>the new extrusion</returns>
        public Extrusion NewExtrusion(CurveArrArray curveArrArray, ReferencePlane workPlane, double startOffset, double endOffset)
        {
            Extrusion rectExtrusion = null;
            try
            {
                m_document.BeginTransaction();
                SketchPlane sketch = m_familyCreator.NewSketchPlane(workPlane.Plane);
                rectExtrusion = m_familyCreator.NewExtrusion(true, curveArrArray, sketch, Math.Abs(endOffset - startOffset));
                rectExtrusion.StartOffset = startOffset;
                rectExtrusion.EndOffset = endOffset;
                m_document.EndTransaction();
                return rectExtrusion;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
