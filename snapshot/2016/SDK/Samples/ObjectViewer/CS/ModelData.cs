//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

using Element = Autodesk.Revit.DB.Element;
using GeomElement = Autodesk.Revit.DB.GeometryElement;
using GeomInstance = Autodesk.Revit.DB.Instance;

namespace Revit.SDK.Samples.ObjectViewer.CS
{
    /// <summary>
    /// generate GraphicsData by give geometry object
    /// </summary>
    public class ModelData
    {
        // BoundingBox of the geometry
        private BoundingBoxXYZ m_bbox;
        private List<List<XYZ>> m_curve3Ds = new List<List<XYZ>>();
        private List<List<UV>> m_curve2Ds = new List<List<UV>>();

        /// <summary>
        /// 3D graphics data of the geometry
        /// </summary>
        public Graphics3DData Data3D
        {
            get
            {
                return new Graphics3DData(new List<List<XYZ>>(m_curve3Ds), m_bbox);
            }
        }

        /// <summary>
        /// 2D graphics data of the geometry
        /// </summary>
        public Graphics2DData Data2D
        {
            get
            {
                return new Graphics2DData(new List<List<UV>>(m_curve2Ds));
            }
        }

        /// <summary>
        /// Generate appropriate graphics data object from exact analytical model type.
        /// </summary>
        /// <param name="element">The selected element maybe has analytical model lines</param>
        /// <returns>A graphics data object appropriate for GDI.</returns>
        public ModelData(Element element)
        {
            try
            {
                AnalyticalModel analyticalMode = GetAnalyticalModel(element);
                View currentView = Command.CommandData.Application.ActiveUIDocument.Document.ActiveView;
                m_bbox = element.get_BoundingBox(currentView);

                if (null == analyticalMode)
                {
                    return;
                }

                GetModelData(analyticalMode);

                
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Get the analytical model object from an element.
        /// </summary>
        /// <param name="element">The selected element maybe has analytical model lines</param>
        /// <returns>Return analytical model object, or else return null.</returns>
        private AnalyticalModel GetAnalyticalModel(Element element)
        {
            return element.GetAnalyticalModel();
        }

        /// <summary>
        /// create GraphicsData of give AnalyticalModel
        /// </summary>
        /// <param name="model">AnalyticalModel contains geometry data</param>
        /// <returns>A graphics data object appropriate for GDI.</returns>
        private void GetModelData(AnalyticalModel model)
        {
            foreach (Curve curve in model.GetCurves(AnalyticalCurveType.RawCurves))
            {
                try
                {
                    m_curve3Ds.Add(curve.Tessellate() as List<XYZ>);
                }
                catch
                {
                }
            }
        }
    }
}
