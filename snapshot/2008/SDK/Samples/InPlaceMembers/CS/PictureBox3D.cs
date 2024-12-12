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


using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;

namespace Revit.SDK.Samples.InPlaceMembers.CS
{
	/// <summary>
	/// picturebox wich can display 3D geometry outline
	/// </summary>
	public class PictureBox3D : Button
	{
		IGraphicsData m_sourceData;		//datasource
		Matrix m_transform;				//transform matrix between origin data and display data

		/// <summary>
		/// paint outline
		/// </summary>
		/// <param name="pe"></param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			
			if (null == m_sourceData)
			{
				return;
			}

			//prepare data
			Graphics g = pe.Graphics;
			g.Clear(Color.White);
			Pen pen = new Pen(Color.DarkGreen);
			GraphicsPath path = new GraphicsPath();

			//draw curves one by one
			List<PointF[]> curves = m_sourceData.PointCurves();
			foreach (PointF[] curve in curves)
			{
				path.Reset();
				path.AddLines(curve);
				path.Transform(m_transform);
				g.DrawPath(pen, path);
			}
		}

		/// <summary>
		/// scale the view by default value
		/// </summary>
		/// <param name="zoomIn">zomme in or zoom out</param>
		public void Scale(bool zoomIn)
		{
			float ratio = 1.0f;
			if(zoomIn)
			{
				ratio = 10.0f/11.0f;
			}
			else
			{
				ratio = 11.0f/10.0f;
			}
			m_transform.Scale(ratio, ratio, MatrixOrder.Append);
			Invalidate();
		}

		/// <summary>
		/// move view in horizontal direction
		/// </summary>
		/// <param name="left">left or right</param>
		public void MoveX(bool left)
		{
			float len = 0.0f;
			if (left)
			{
				len = -5.0f;
			}
			else
			{
				len = 5.0f;
			}
			m_transform.Translate(len, 0, MatrixOrder.Append);
			Invalidate();
		}

		/// <summary>
		/// move view in vertical direction
		/// </summary>
		/// <param name="up">up or down</param>
		public void MoveY(bool up)
		{
			float len = 0.0f;
			if (up)
			{
				len = 5.0f;
			}
			else
			{
				len = -5.0f;
			}
			m_transform.Translate(0, len, MatrixOrder.Append);
			Invalidate();
		}

		/// <summary>
		/// datasource which can be any class inherited from IGraphicsData
		/// </summary>
		public IGraphicsData DataSource
		{
			get
			{
				return m_sourceData;
			}
			set
			{
				if (null != value)
				{
					m_sourceData = value;
					RectangleF rec = m_sourceData.Region;
					PointF[] plgpts = GetDisplayRegion();
					m_transform = new Matrix(rec, plgpts);
					m_sourceData.UpdateViewEvent += new UpdateViewDelegate(Invalidate);
				}
			}
		}

		/// <summary>
		/// get the display region, adjust the proportion and location
		/// </summary>
		/// <returns></returns>
		private PointF[] GetDisplayRegion()
		{
			RectangleF rec = m_sourceData.Region;
			const float MARGIN = 8.0f;

			float realWidth = this.Width - MARGIN *2;
			float realHeight = this.Height - MARGIN*2;
			float minX = MARGIN;
			float minY = MARGIN;
			float ratioRec = rec.Height / rec.Width;
			float ratioBox = realHeight / realWidth;

			if (ratioRec > ratioBox)
			{
				float temp = realWidth;
				realWidth = realHeight * rec.Width /rec.Height;
				minX = (temp - realWidth)/2.0f;
			}
			else
			{
				float temp = realHeight;
				realHeight = realWidth * rec.Height/rec.Width;
				minY = (temp - realHeight) / 2.0f;	
			}

			if (rec.Width < (GraphicsData.MINEDGElENGTH + 1) && 
                rec.Height < (GraphicsData.MINEDGElENGTH + 1))
			{
				minX = realWidth / 2.0f;
				minY = realHeight / 2.0f;
			}

			PointF[] plgpts = new PointF[3];
			plgpts[0] = new PointF(minX, minY);
			plgpts[1] = new PointF(realWidth + minX, minY);
			plgpts[2] = new PointF(minX, realHeight + minY);

			return plgpts;
		}
	}
}
