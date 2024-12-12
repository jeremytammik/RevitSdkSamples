//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Geometry;

using Element = Autodesk.Revit.Element;
using GeomElement = Autodesk.Revit.Geometry.Element;
using GeomInstance = Autodesk.Revit.Geometry.Instance;

namespace Revit.SDK.Samples.ObjectViewer.CS
{
    /// <summary>
    /// generate GraphicsData by give geometry object
    /// </summary>
    public class ModelData
    {
        // BoundingBox of the geometry
        private BoundingBoxXYZ m_bbox;
        private List<XYZArray> m_curve3Ds = new List<XYZArray>();
        private List<UVArray> m_curve2Ds = new List<UVArray>();

        /// <summary>
        /// 3D graphics data of the geometry
        /// </summary>
        public Graphics3DData Data3D
        {
            get
            {
                return new Graphics3DData(new List<XYZArray>(m_curve3Ds), m_bbox);
            }
        }

        /// <summary>
        /// 2D graphics data of the geometry
        /// </summary>
        public Graphics2DData Data2D
        {
            get
            {
                return new Graphics2DData(new List<UVArray>(m_curve2Ds));
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
                View currentView = Command.CommandData.Application.ActiveDocument.ActiveView;
                m_bbox = element.get_BoundingBox(currentView);

                if (null == analyticalMode)
                {
                    return;
                }

                switch (analyticalMode.GetType().Name)
                {
                    case "AnalyticalModel3D":
                        AnalyticalModel3D model3D = analyticalMode as AnalyticalModel3D;
                        GetModelData(model3D);
                        break;
                    case "AnalyticalModelFloor":
                        AnalyticalModelFloor modelFloor = analyticalMode as AnalyticalModelFloor;
                        GetModelData(modelFloor);
                        break;
                    case "AnalyticalModelWall":
                        AnalyticalModelWall modelWall = analyticalMode as AnalyticalModelWall;
                        GetModelData(modelWall);
                        break;
                    case "AnalyticalModelFrame":
                        AnalyticalModelFrame modelFrame = analyticalMode as AnalyticalModelFrame;
                        GetModelData(modelFrame);
                        break;
                    case "AnalyticalModelLocation":
                        AnalyticalModelLocation modelLocation = analyticalMode as AnalyticalModelLocation;
                        GetModelData(modelLocation);
                        break;
                    case "AnalyticalModelProfile":
                        AnalyticalModelProfile modelProfile = analyticalMode as AnalyticalModelProfile;
                        GetModelData(modelProfile);
                        break;
                    default:
                        break;
                }
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
            AnalyticalModel analyticalModel = null;
            //find out the type of the element
            switch (element.GetType().Name.ToString())
            {
                //it is a family instance
                case "FamilyInstance":
                    FamilyInstance familyComponet = element as FamilyInstance;
                    if (null != element as FamilyInstance)
                    {
                        // get the familyinstance's analytical model
                        analyticalModel = familyComponet.AnalyticalModel;
                    }
                    break;
                //it is a wall
                case "Wall":
                    Wall wall = element as Wall;
                    if (null != wall)
                    {
                        // get the wall's analytical model
                        analyticalModel = wall.AnalyticalModel;
                    }
                    break;
                //it is a cont footing
                case "ContFooting":
                    ContFooting contFooting = element as ContFooting;
                    if (null != contFooting)
                    {
                        // get the contfoot's analytical model
                        analyticalModel = contFooting.AnalyticalModel;
                    }
                    break;
                //it is a floor
                case "Floor":
                    Floor floor = element as Floor;
                    if (null != floor)
                    {
                        // get the floor's analytical model
                        analyticalModel = floor.AnalyticalModel;
                    }
                    break;
                //else break
                default:
                    analyticalModel = null;
                    break;
            }
            return analyticalModel;
        }

        /// <summary>
        /// create GraphicsData of give AnalyticalModel3D
        /// </summary>
        /// <param name="model">AnalyticalModel3D contains geometry data</param>
        /// <returns>A graphics data object appropriate for GDI.</returns>
        private void GetModelData(AnalyticalModel3D model)
        {
            foreach (Curve curve in model.Curves)
            {
                try
                {
                    m_curve3Ds.Add(curve.Tessellate());
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// create GraphicsData of give AnalyticalModelFloor
        /// </summary>
        /// <param name="model">AnalyticalModelFloor contains geometry data</param>
        /// <returns>A graphics data object appropriate for GDI.</returns>
        private void GetModelData(AnalyticalModelFloor model)
        {
            foreach (Curve curve in model.Curves)
            {
                try
                {
                    m_curve3Ds.Add(curve.Tessellate());
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// create GraphicsData of give AnalyticalModelWall
        /// </summary>
        /// <param name="model">AnalyticalModelWall contains geometry data</param>
        /// <returns>A graphics data object appropriate for GDI.</returns>
        private void GetModelData(AnalyticalModelWall model)
        {
            foreach (Curve curve in model.Curves)
            {
                try
                {
                    m_curve3Ds.Add(curve.Tessellate());
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// create GraphicsData of give AnalyticalModelFrame
        /// </summary>
        /// <param name="model">AnalyticalModelFrame contains geometry data</param>
        /// <returns>A graphics data object appropriate for GDI.</returns>
        private void GetModelData(AnalyticalModelFrame model)
        {
            Curve modelCurve = model.Curve;
            if (null != modelCurve && modelCurve.IsBound)
            {
                m_curve3Ds.Add(modelCurve.Tessellate());
            }
            GetModelData(model.Profile);
        }

        /// <summary>
        /// create GraphicsData of give AnalyticalModelLocation
        /// </summary>
        /// <param name="model">AnalyticalModelLocation contains geometry data</param>
        /// <returns>A graphics data object appropriate for GDI.</returns>
        private void GetModelData(AnalyticalModelLocation model)
        {
            try
            {
                XYZArray points = new XYZArray();
                XYZ point = model.Point;
                points.Append(point);
                m_curve3Ds.Add(points);
            }
            catch
            {
            }
        }

        /// <summary>
        /// create GraphicsData of give AnalyticalModelProfile
        /// </summary>
        /// <param name="model">AnalyticalModelProfile contains geometry data</param>
        /// <returns>A graphics data object appropriate for GDI.</returns>
        private void GetModelData(AnalyticalModelProfile model)
        {
            const int NumAverage = 10;
            Curve curve = model.DrivingCurve;
            XYZArray drivingPnts = new XYZArray();
            double length = model.EndSetBack - model.StartSetBack;
            double interval = length / NumAverage;
            for (int i = 0; i <= NumAverage; i++)
            {
                XYZ pnt = curve.Evaluate(model.StartSetBack + interval * i, false);
                drivingPnts.Append(pnt);
            }
            foreach (Curve profileCurve in model.SweptProfile.Curves)
            {
                try
                {
                    UVArray pnts = new UVArray();
                    foreach (XYZ pnt in profileCurve.Tessellate())
                    {
                        UV uv = new UV(pnt.X, pnt.Y);
                        pnts.Append(uv);
                    }
                    m_curve2Ds.Add(pnts);
                }
                catch
                {
                }
            }
        }
    }
}
