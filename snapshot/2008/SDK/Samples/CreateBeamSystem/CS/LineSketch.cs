//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.CreateBeamSystem.CS
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Drawing;
	using System.Drawing.Drawing2D;

	using Autodesk.Revit.Geometry;

	/// <summary>
	/// sketch line and any tag on it
	/// </summary>
	public class LineSketch : ObjectSketch
	{		
		/// <summary>
		/// the rate of direction tag's distance to the line
		/// </summary>
		private const float DirectionTag_Distance_Ratio = 0.02f;
		/// <summary>
		/// the rate of direction tag's length to the line
		/// </summary>
		private const float DirectionTag_Length_Ratio = 0.1f;
		private Line2D m_line = new Line2D();	// geometry line to draw
		private bool m_isDirection;				// whether has direction tag

		/// <summary>
		/// whether has direction tag
		/// </summary>
		public bool IsDirection
		{
			get 
			{
				return m_isDirection;
			}
			set 
			{ 
				m_isDirection = value; 
			}
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="line"></param>
		public LineSketch(Line2D line)
		{
			m_line        = line;
			m_boundingBox = line.BoundingBox;
			m_pen.Color   = Color.DarkGreen;
			m_pen.Width   = 1f;
		}

		/// <summary>
		/// draw the line
		/// </summary>
		/// <param name="g">drawing object</param>
		/// <param name="translate">translation between drawn sketch and geometry object</param>
		public override void Draw(Graphics g, Matrix translate)
		{
			GraphicsPath path = new GraphicsPath();
			path.AddLine(m_line.StartPnt, m_line.EndPnt);

			if (m_isDirection)
			{
				DrawDirectionTag(path);
			}

			path.Transform(translate);
			g.DrawPath(m_pen, path);
		}

		/// <summary>
		/// draw 2 shorter parallel lines on each side of the line
		/// </summary>
		/// <param name="path"></param>
		private void DrawDirectionTag(GraphicsPath path)
		{
			Line2D leftLine  = m_line.Clone();
			Line2D rightLine = m_line.Clone();
			leftLine.Scale(DirectionTag_Length_Ratio);
			leftLine.Shift(DirectionTag_Distance_Ratio * m_line.Length);
			rightLine.Scale(DirectionTag_Length_Ratio);
			rightLine.Shift(-DirectionTag_Distance_Ratio * m_line.Length);
			GraphicsPath leftPath = new GraphicsPath();
			leftPath.AddLine(leftLine.StartPnt, leftLine.EndPnt);
			GraphicsPath rightPath = new GraphicsPath();
			rightPath.AddLine(rightLine.StartPnt, rightLine.EndPnt);
			path.AddPath(leftPath, false);
			path.AddPath(rightPath, false);
		}
	}
}
