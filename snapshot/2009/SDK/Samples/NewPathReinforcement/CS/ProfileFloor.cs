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
using Autodesk.Revit.Elements;
using Autodesk.Revit;
using System.Drawing;
using Autodesk.Revit.Geometry;
using System.Drawing.Drawing2D;
using Autodesk.Revit.Symbols;

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
            :base(commandData)
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
        public override List<XYZArray> GetNeedPoints(List<List<Edge>> faces)
        {
            List<XYZArray> needPoints = new List<XYZArray>();
            foreach (Edge edge in faces[0])
            {
                XYZArray edgexyzs = edge.Tessellate();
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
            Type type = typeof(Autodesk.Revit.Elements.ViewPlan);
            ElementIterator iter = m_commandData.Application.ActiveDocument.get_Elements(type);
            iter.Reset();
            while (iter.MoveNext())
            {
                View view = iter.Current as View;
                if ("Level 2" == view.ViewName)
                {
                    viewLevel2 = view;
                }
            }

            Vector4 xAxis = new Vector4(viewLevel2.RightDirection);
            //Because Y axis in windows UI is downward, so we shoud Multipy(-1) here
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
            XYZ p1, p2; Line curve;
            CurveArray curves = m_appCreator.NewCurveArray();
            for (int i = 0; i < points.Count - 1; i++)
            {
                p1 = new XYZ(points[i].X, points[i].Y, points[i].Z);
                p2 = new XYZ(points[i + 1].X, points[i + 1].Y, points[i + 1].Z);
                curve = m_appCreator.NewLine(p1, p2, true);
                curves.Append(curve);
            }

            return m_docCreator.NewPathReinforcement(m_data, curves, flip);
        }
    }
}
