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
using System.Text;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Revit.SDK.Samples.NewPathReinforcement.CS
{
    /// <summary>
    /// ProfileFloor class contains the information about profile of floor,
    /// and contains method used to create PathReinforcement on floor
    /// </summary>
    public class ProfileFloor : Profile
    {
        private Floor m_data = null;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="floor">floor to create reinforcement on</param>
        /// <param name="commandData">object which contains reference to Revit Application</param>
        public ProfileFloor(Floor floor, ExternalCommandData commandData)
            : base(commandData)
        {
            m_data = floor;
            List<List<Edge>> faces = GetFaces(m_data);
            m_points = GetNeedPoints(faces);
            m_to2DMatrix = GetTo2DMatrix();
        }

        /// <summary>
        /// Get points of the first face
        /// </summary>
        /// <param name="faces">edges in all faces</param>
        /// <returns>points of first face</returns>
        public override List<List<XYZ>> GetNeedPoints(List<List<Edge>> faces)
        {
            List<List<XYZ>> needPoints = new List<List<XYZ>>();
            foreach (Edge edge in faces[0])
            {
                List<XYZ> edgexyzs = edge.Tessellate() as List<XYZ>;
                needPoints.Add(edgexyzs);
            }
            return needPoints;
        }

        /// <summary>
        /// Get a matrix which can transform points to 2D
        /// </summary>
        /// <returns>matrix which can transform points to 2D</returns>
        public override Matrix4 GetTo2DMatrix()
        {
            View viewLevel2 = null;

            // select the view which is named "Level 2"
            // Once use the View type to filterrrr the element ,please skip the view templates 
            // because they're behind-the-scene and invisible in project browser; also invalid for API.
            IEnumerable<View> views = from elem in
                                          (new FilteredElementCollector(m_commandData.Application.ActiveUIDocument.Document)).OfClass(typeof(ViewPlan)).ToElements()
                                      let view = elem as View
                                      where (view != null) && (!view.IsTemplate) && (view.Name == "Level 2")
                                      select view;
            viewLevel2 = views.First();

            Vector4 xAxis = new Vector4(viewLevel2.RightDirection);
            //Because Y axis in windows UI is downward, so we should Multiply(-1) here
            Vector4 yAxis = new Vector4(viewLevel2.UpDirection.Multiply(-1));
            Vector4 zAxis = new Vector4(viewLevel2.ViewDirection);

            Matrix4 result = new Matrix4(xAxis, yAxis, zAxis);
            return result;
        }

        /// <summary>
        /// Create PathReinforcement on floor
        /// </summary>
        /// <param name="points">points used to create PathReinforcement</param>
        /// <param name="flip">used to specify whether new PathReinforcement is Filp</param>
        /// <returns>new created PathReinforcement</returns>
        public override PathReinforcement CreatePathReinforcement(List<Vector4> points, bool flip)
        {
            Autodesk.Revit.DB.XYZ p1, p2; Line curve;
            IList<Curve> curves = new List<Curve>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                p1 = new Autodesk.Revit.DB.XYZ(points[i].X, points[i].Y, points[i].Z);
                p2 = new Autodesk.Revit.DB.XYZ(points[i + 1].X, points[i + 1].Y, points[i + 1].Z);
                curve = Line.CreateBound(p1, p2);
                curves.Add(curve);
            }
            ElementId pathReinforcementTypeId = PathReinforcementType.CreateDefaultPathReinforcementType(m_document);
            ElementId rebarBarTypeId = RebarBarType.CreateDefaultRebarBarType(m_document);
            ElementId rebarHookTypeId = RebarHookType.CreateDefaultRebarHookType(m_document);
            return PathReinforcement.Create(m_document, m_data, curves, flip, pathReinforcementTypeId, rebarBarTypeId, rebarHookTypeId, rebarHookTypeId);
        }
    }
}
