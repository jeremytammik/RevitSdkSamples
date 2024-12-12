//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using REX.Common.Geometry;

namespace REX.ContentGeneratorWPF.Resources.Dialogs
{
    /// <summary>
    /// Represents control which allows the user to draw polygons.
    /// </summary>
    public partial class PolygonViewer : UserControl
    {
        /// <summary>
        /// The bounding box of the polygon.
        /// </summary>
        BoundingBoxREXxyz BoundBox;
        /// <summary>
        /// The shape of the polygon.
        /// </summary>
        List<REXxyz> Points;
        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonViewer"/> class.
        /// </summary>
        public PolygonViewer()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Draws the specified polygon.
        /// </summary>
        /// <param name="points">The points.</param>
        public void DrawPolygon(List<REXxyz> points)
        {
            Points = points;
            SetBoundingBox();
            DrawPolygon();
        }
        /// <summary>
        /// Initializes the bounding box of the polygon.
        /// </summary>
        private void SetBoundingBox()
        {
            BoundBox = new BoundingBoxREXxyz();
            foreach (REXxyz pt in Points)
                BoundBox.AddPoint(pt);
        }
        /// <summary>
        /// Draws the polygon based on the saved point list.
        /// </summary>
        private void DrawPolygon()
        {
            //In order to draw the whole drawing in the center of the viewer some additional
            //calculation is required.

            if (Points != null && BoundBox != null)
            {
                REXxyz dx = BoundBox.Max - BoundBox.Min;
                PolyLineSegment polyline = new PolyLineSegment();

                //The assumption is that there will be empty frame of 0.1*Max(width,height) around
                //the drawing. To get the most extreme case the coefficients in x any y direction are
                //calculated.
                double coeffX = 0.8 * this.ActualWidth / dx.x;
                double coeffY = 0.8 * this.ActualHeight / dx.y;

                double coeff;
                double xstart;
                double ystart;
                if (coeffX < coeffY)
                {
                    coeff = coeffX;
                    xstart = 0.1 * this.ActualWidth;
                    ystart = 0.1 * this.ActualHeight + (0.8*this.ActualHeight -(coeffX * dx.y)) / 2;
                }
                else
                {
                    coeff = coeffY;
                    ystart = 0.1 * this.ActualHeight;
                    xstart = 0.1 * this.ActualWidth + (0.8 * this.ActualWidth - (coeffY * dx.x)) / 2;
                }

                for (int i = 0; i < Points.Count; i++ )
                {
                    REXxyz pt = Points[i];
                    polyline.Points.Add(new Point(xstart + (pt.x - BoundBox.Min.x) * coeff,this.ActualHeight - (ystart + (pt.y - BoundBox.Min.y) * coeff)));
                }
                pathGeometry.Figures.Clear();

                REXxyz startPt = Points[0];

                PathFigure pathFigure = new PathFigure(new Point(xstart + (startPt.x - BoundBox.Min.x) * coeff,this.ActualHeight - (ystart + (startPt.y - BoundBox.Min.y) * coeff)),new List<PathSegment>(){polyline},true);
                pathFigure.Segments.Add(polyline);

                pathGeometry.Figures.Add(pathFigure);
            }
        }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.SizeChanged"/> event, using the specified information as part of the eventual event data.
        /// </summary>
        /// <param name="sizeInfo">Details of the old and new size involved in the change.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            DrawPolygon();
        }

        
    }
}
