//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.Openings.CS
{
    /// <summary>
    /// sketch line and any tag on it
    /// </summary>
    public class LineSketch : ObjectSketch
    {
        private Line2D m_line = new Line2D();    // geometry line to draw

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="line"></param>
        public LineSketch(Line2D line)
        {
            m_line = line;
            m_boundingBox = line.BoundingBox;
            m_pen.Color = System.Drawing.Color.Yellow;
            m_pen.Width = 1f;
        }

        /// <summary>
        /// draw the line
        /// </summary>
        /// <param name="g">drawing object</param>
        /// <param name="translate">translation between drawn sketch and geometry object</param>
        public override void Draw(Graphics g, Matrix translate)
        {
            m_transform = translate;
            GraphicsPath path = new GraphicsPath();
            path.AddLine(m_line.StartPnt, m_line.EndPnt);
            path.Transform(translate);
            g.DrawPath(m_pen, path);
        }
    }
}
