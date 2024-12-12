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
	using System.Windows.Forms;

	using Autodesk.Revit.Geometry;

	/// <summary>
	/// Sketch the profile of beam system on canvas
	/// Code here have nothing with Revit API
	/// </summary>
	public class BeamSystemSketch : ObjectSketch
	{
		/// <summary>
		/// ratio of margin to canvas widgh
		/// </summary>
		private const float MarginRatio = 0.1f;

		/// <summary>
		/// the control to draw beam system
		/// </summary>
		private Control m_canvas;

		/// <summary>
		/// defines a local geometric inverse transform
		/// </summary>
		private Matrix m_inverse;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="canvas">the control to draw beam system</param>
		public BeamSystemSketch(Control canvas)
		{
			m_canvas = canvas;
		}

		/// <summary>
		/// draw the profile in the canvas
		/// </summary>
		/// <param name="profile">the profile of the beam system</param>
		public void DrawProfile(IList<Line> profile)
		{
			Initialize(profile);
			CalculateTransform();
			m_canvas.Paint += new PaintEventHandler(this.Paint);
			m_canvas.Refresh();
		}

		/// <summary>
		/// draw beam system
		/// </summary>
		/// <param name="g">encapsulates a GDI+ drawing surface</param>
		/// <param name="translate">translation matrix to canvas coordinates</param>
		public override void Draw(Graphics g, Matrix translate)
		{
			foreach (LineSketch sketch in m_objects)
			{
				sketch.Draw(g, m_transform);
			}
		}

		/// <summary>
		/// draw beam system on canvas control
		/// </summary>
		/// <param name="sender">canvas control</param>
		/// <param name="e">data for the Paint event</param>
		protected void Paint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(Color.White);
			Draw(g, m_transform);
		}

		/// <summary>
		/// generate a Line2D instance using a Line's data
		/// </summary>
		/// <param name="line">where new Line2D get data</param>
		/// <returns>new Line2D</returns>
		private static Line2D GetLine2D(Line line)
		{
			Line2D result   = new Line2D();
			result.StartPnt = new PointF((float)line.get_EndPoint(0).X, (float)line.get_EndPoint(0).Y);
            result.EndPnt   = new PointF((float)line.get_EndPoint(1).X, (float)line.get_EndPoint(1).Y);
			return result;
		}

		/// <summary>
		/// calculate the transform between canvas and geometry objects
		/// </summary>
		private void CalculateTransform()
		{
			PointF[] plgpts = CalculateCanvasRegion();
			m_transform     = new Matrix(BoundingBox, plgpts);
			m_inverse       = m_transform.Clone();

			if (m_inverse.IsInvertible)
			{
				m_inverse.Invert();
			}
		}

		/// <summary>
		/// initialize geometry objects and bounding box
		/// </summary>
		/// <param name="profile">the profile of the beam system</param>
		private void Initialize(IList<Line> profile)
		{
			// deal with first line in profile
			m_objects.Clear();
			LineSketch firstSketch  = new LineSketch(GetLine2D(profile[0]));
			m_boundingBox           = firstSketch.BoundingBox;
			firstSketch.IsDirection = true;
			m_objects.Add(firstSketch);

			// all other lines
			for (int i = 1; i < profile.Count; i++)
			{
				LineSketch sketch = new LineSketch(GetLine2D(profile[i]));
				m_boundingBox = RectangleF.Union(BoundingBox, sketch.BoundingBox);
				m_objects.Add(sketch);
			}
		}

		/// <summary>
		/// get the display region, adjust the proportion and location
		/// </summary>
		/// <returns>upper-left, upper-right, and lower-left corners of the rectangle </returns>
		private PointF[] CalculateCanvasRegion()
		{
			// get the area without margin
			float realWidth   = m_canvas.Width * (1 - 2 * MarginRatio);
			float realHeight  = m_canvas.Height * (1 - 2 * MarginRatio);
			float minX        = m_canvas.Width * MarginRatio;
			float minY        = m_canvas.Height * MarginRatio;
			// ratio of width to height
			float originRate  = m_boundingBox.Width / m_boundingBox.Height;
			float displayRate = realWidth / realHeight;
			
			if (originRate > displayRate)
			{	
				// display area in canvas need move to center in height
				float goalHeight = realWidth / originRate;
				minY             = minY + (realHeight - goalHeight) / 2;
				realHeight       = goalHeight;
			}
			else
			{
				// display area in canvas need move to center in width
				float goalWidth = realHeight * originRate;
				minX            = minX + (realWidth - goalWidth) / 2;
				realWidth       = goalWidth;
			}

			PointF[] plgpts = new PointF[3];
			plgpts[0] = new PointF(minX, realHeight + minY);				// upper-left point	
			plgpts[1] = new PointF(realWidth + minX, realHeight + minY);	// upper-right point
			plgpts[2] = new PointF(minX, minY);								// lower-left point

			return plgpts;
		}
	}
}