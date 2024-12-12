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

namespace Revit.SDK.Samples.NewOpenings.CS
{
    /// <summary>
    /// ProfileFloor class contain the information about profile of floor,
    /// and contain method to create Opening on floor
    /// </summary>
    public class ProfileFloor : Profile
    {
        private Floor m_data;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="floor">selected floor</param>
        /// <param name="commandData">ExternalCommandData</param>
        public ProfileFloor(Floor floor, ExternalCommandData commandData)
            :base(floor, commandData)            
        {
            m_data = floor;
        }

        /// <summary>
        /// Create Opening on floor
        /// </summary>
        /// <param name="points">points use to create Opening</param>
        /// <param name="type">tool type</param>
        public override void DrawOpening(List<Vector4> points, ToolType type)
        {
            switch(type)
            {
                case ToolType.Line :
                case ToolType.Rectangle:
                    DrawPlineOpening(points);
                    break;

                case ToolType.Circle:
                    DrawCircleOpening(points);
                    break;

                case ToolType.Arc:
                    DrawArcOpening(points);
                    break;
                default: break;
            }            
        }

        /// <summary>
        /// Create Opening which make up of line on floor
        /// </summary>
        /// <param name="points">points use to create Opening</param>
        private void DrawPlineOpening(List<Vector4> points)
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

            p1 = new XYZ(points[points.Count - 1].X, 
                points[points.Count - 1].Y, points[points.Count - 1].Z);
            p2 = new XYZ(points[0].X, points[0].Y, points[0].Z);
            curve = m_appCreator.NewLine(p1, p2, true);
            curves.Append(curve);

            m_docCreator.NewOpening(m_data, curves, true);
        }

        /// <summary>
        /// Create Opening which make up of Circle on floor
        /// </summary>
        /// <param name="points">points use to create Opening</param>
        private void DrawCircleOpening(List<Vector4> points)
        {
            CurveArray curves = m_appCreator.NewCurveArray();
            XYZ p1 = new XYZ(points[0].X, points[0].Y, points[0].Z);
            XYZ p2 = new XYZ(points[1].X, points[1].Y, points[1].Z);
            XYZ p3 = new XYZ(points[2].X, points[2].Y, points[2].Z);
            XYZ p4 = new XYZ(points[3].X, points[3].Y, points[3].Z);
            Arc arc  = m_appCreator.NewArc(p1, p3, p2);
            Arc arc2 = m_appCreator.NewArc(p1, p3, p4);
            curves.Append(arc);
            curves.Append(arc2);
            m_docCreator.NewOpening(m_data, curves, true);
        }

        /// <summary>
        /// Create Opening which make up of Arc on floor
        /// </summary>
        /// <param name="points">points use to create Opening</param>
        private void DrawArcOpening(List<Vector4> points)
        {
            CurveArray curves = m_appCreator.NewCurveArray();
            Arc arc; XYZ p1, p2, p3;
            p1 = new XYZ(points[0].X, points[0].Y, points[0].Z);
            p2 = new XYZ(points[1].X, points[1].Y, points[1].Z);
            p3 = new XYZ(points[2].X, points[2].Y, points[2].Z);
            arc = m_appCreator.NewArc(p1, p2, p3);
            curves.Append(arc);
            for (int i = 1; i < points.Count - 3; i += 2)
            {
                p1 = new XYZ(points[i].X, points[i].Y, points[i].Z);
                p2 = new XYZ(points[i + 2].X, points[i + 2].Y, points[i + 2].Z);
                p3 = new XYZ(points[i + 3].X, points[i + 3].Y, points[i + 3].Z);
                arc = m_appCreator.NewArc(p1, p2, p3);
                curves.Append(arc);
            }
            m_docCreator.NewOpening(m_data, curves, true);
        }
    }
}
