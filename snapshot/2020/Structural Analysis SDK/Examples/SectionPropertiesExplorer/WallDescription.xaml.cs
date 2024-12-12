//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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

using XYZ = Autodesk.Revit.DB.XYZ;

using Autodesk.Revit.DB.CodeChecking.Engineering;

namespace SectionPropertiesExplorer
{
    /// <summary>
    /// Interaction logic for WallDescription.xaml
    /// </summary>
    public partial class WallDescriptionControl : UserControl
    {
        public WallDescriptionControl()
        {
            InitializeComponent();
        }

        public void DoBinding(WallInfo wallInfo, Autodesk.Revit.DB.DisplayUnit? unitSystem, double beamLength = 0)
        {
            familyNameLabel.Visibility = System.Windows.Visibility.Visible;
            familyName.Visibility = System.Windows.Visibility.Visible;

            typeNameLabel.Visibility = System.Windows.Visibility.Visible;
            typeName.Visibility = System.Windows.Visibility.Visible;

            familyName.Content = wallInfo.FamilyName;
            typeName.Content = wallInfo.TypeName;

            wallViewer.Children.Clear();

            double scale = 1.2;

            double actualWidth = wallViewer.MinWidth;
            double actualHeight = wallViewer.MinHeight;

            if (wallViewer.ActualWidth > 0)
            {
                actualWidth = wallViewer.ActualWidth;
            }

            if (wallViewer.ActualHeight > 0)
            {
                actualHeight = wallViewer.ActualHeight;
            }

            double horizontalMargin = actualWidth / 8;
            double verticalMargin = actualHeight / 6;

            double width = actualWidth - 2 * horizontalMargin;
            double height = actualHeight - 2 * verticalMargin;

            horizontalMargin += (width - width * scale) / 2.0;
            verticalMargin += (height - height * scale) / 2.0;

            double horizontalShift = horizontalMargin;
            double verticalShift = verticalMargin;

            XYZ xyz_min = wallInfo.Contour.GetMinimumBoundary();
            XYZ xyz_max = wallInfo.Contour.GetMaximumBoundary();

            width = Math.Abs(xyz_max.X - xyz_min.X);
            height = Math.Abs(xyz_max.Y - xyz_min.Y);

            double stretchCoeff = 0;
            if (height > width)
            {
                stretchCoeff = (actualHeight - 2 * verticalMargin) / height;
                horizontalShift += ((actualWidth - 2 * horizontalMargin) - (width * stretchCoeff)) / 2.0;
            }
            else
            {
                stretchCoeff = (actualWidth - 2 * horizontalMargin) / width;
                verticalShift += ((actualHeight - 2 * verticalMargin) - (height * stretchCoeff)) / 2;
            }

            if (xyz_min.X < 0)
                horizontalShift -= stretchCoeff * xyz_min.X;
            if (xyz_min.Y < 0)
                verticalShift -= stretchCoeff * xyz_min.Y;

            System.Windows.Shapes.Shape contour = null;
            List<System.Windows.Shapes.Shape> openings = new List<System.Windows.Shapes.Shape>();

            Polygon polygon = null;
            PointCollection points = null;

            if (wallInfo.Contour != null && wallInfo.Contour.Points.Count > 2)
            {
                polygon = new Polygon();
                points = new PointCollection();
                foreach (XYZ xyz in wallInfo.Contour.Points)
                {
                    points.Add(new Point(xyz.X * stretchCoeff, xyz.Y * stretchCoeff));
                }
                polygon.Points = points;
                polygon.Fill = Brushes.LightSlateGray;
                polygon.Stretch = Stretch.None;
                polygon.Stroke = Brushes.Black;
                polygon.StrokeThickness = 1;

                contour = polygon;
            }

            foreach (Autodesk.Revit.DB.CodeChecking.Engineering.Shape opening in wallInfo.Openings)
            {
                polygon = new Polygon();
                points = new PointCollection();
                foreach (XYZ xyz in opening.Points)
                {
                    points.Add(new Point(xyz.X * stretchCoeff, xyz.Y * stretchCoeff));
                }
                polygon.Points = points;
                polygon.Fill = Brushes.White;
                polygon.Stretch = Stretch.None;
                polygon.Stroke = Brushes.Black;
                polygon.StrokeThickness = 1;

                openings.Add(polygon);
            }

            if (contour != null)
            {
                Canvas.SetLeft(contour, horizontalShift);
                Canvas.SetTop(contour, verticalShift);
                wallViewer.Children.Add(contour);
            }

            foreach (System.Windows.Shapes.Shape opening in openings)
            {
                Canvas.SetLeft(opening, horizontalShift);
                Canvas.SetTop(opening, verticalShift);
                wallViewer.Children.Add(opening);
            }

            double viewerWidth, viewerHeight;
            wallViewer.GetViewerWidthAndHeight(out viewerWidth, out viewerHeight);

            double dimHeight = Math.Abs(xyz_max.Y - xyz_min.Y);
            double dimWidth = Math.Abs(xyz_max.X - xyz_min.X);

            if(viewerWidth/2.0 > horizontalShift)
            {
                horizontalShift += dimWidth * stretchCoeff;
            }

            double offset = 0.0;
            if (viewerHeight / 2.0 > verticalShift)
            {
                offset = dimHeight * stretchCoeff;
            }
            else
            {
                verticalShift -= dimHeight * stretchCoeff;
            }

            double x = horizontalShift + 10;
            Point start = new Point(x, verticalShift);
            Point end = new Point(x, start.Y + dimHeight * stretchCoeff);
            CreateDimensionLine(dimHeight, start, end, stretchCoeff, unitSystem);

            double y = verticalShift + dimHeight * stretchCoeff + 10;
            start = new Point(horizontalShift - (dimWidth * stretchCoeff), y);
            end = new Point(start.X + dimWidth * stretchCoeff, y);
            CreateDimensionLine(dimWidth, start, end, stretchCoeff, unitSystem);
        }
   
        private void CreateDimensionLine(double dim, 
                                         Point start, Point end, 
                                         double stretchCoeff,
                                         Autodesk.Revit.DB.DisplayUnit? unitSystem)
        {
            bool horizontal = Math.Abs(end.Y - start.Y) < 1 ? true : false;
            bool vertical = Math.Abs(end.X - start.X) < 1 ? true : false;

            double dimValueWidth = dim * stretchCoeff;

            string dimValueText = "";
          
            if (null == unitSystem)
            {
                dimValueText += UnitsAssignment.FormatToRevitUI("WallDimension", dim, ElementInfoUnits.Assignments, true);
            }
            else
            {
                double val = Math.Round(dim, 6, MidpointRounding.AwayFromZero);
                dimValueText += val.ToString();
                dimValueText += " " + UnitsAssignment.GetUnitSymbol("WallDimension", ElementInfoUnits.Assignments, (Autodesk.Revit.DB.DisplayUnit)unitSystem);
            }

            TextBlock dimValue = new TextBlock();
            dimValue.Text = dimValueText;
            dimValue.Foreground = new SolidColorBrush(Colors.Gray);
            dimValue.Width = dimValueWidth;
            dimValue.TextAlignment = TextAlignment.Center;

            double viewerWidth, viewerHeight;
            wallViewer.GetViewerWidthAndHeight(out viewerWidth, out viewerHeight);

            double dimValueStartX = start.X;
            if (horizontal)
            {
                double dx = viewerWidth - dimValueStartX - dimValueWidth;
                if (dx > dimValueStartX)
                    dx = dimValueStartX;
                dimValueStartX -= dx;
                if (dx > 0.0)
                {
                    dimValue.Width += 2 * dx;
                }
 
                Canvas.SetTop(dimValue, start.Y);
            }

            double dimValueStartY = start.Y;
            if (vertical)
            {
                double dy = viewerHeight - dimValueStartY - dimValueWidth;
                if (dy > dimValueStartY)
                    dy = dimValueStartY;
                dimValueStartY += dy + dimValueWidth;
                if (dy > 0.0)
                {
                    dimValue.Width += 2 * dy;
                }

                dimValue.RenderTransform = new RotateTransform(-90);

                Canvas.SetTop(dimValue, dimValueStartY);
            }

            Canvas.SetLeft(dimValue, dimValueStartX);
            
            wallViewer.Children.Add(dimValue);

            Polyline polyline = new Polyline();

            PointCollection points = new PointCollection();
            double offset = dimValue.FontSize + 5;

            if (horizontal)
            {
                double dlx = end.X > start.X ? 6 : -6;
                double dx = end.X > start.X ? 0.5 : -0.5;
                double dy = 1;

                dx *= 2;
                points.Add(new Point(start.X - dlx, start.Y + offset));
                points.Add(new Point(start.X, start.Y + offset));
                points.Add(new Point(start.X + dx, start.Y - dy + offset));
                points.Add(new Point(start.X - dx, start.Y + dy + offset));
                points.Add(new Point(start.X, start.Y + offset));
                
                points.Add(new Point(end.X, end.Y + offset));

                points.Add(new Point(end.X + dx, start.Y - dy + offset));
                points.Add(new Point(end.X - dx, start.Y + dy + offset));
                points.Add(new Point(end.X, start.Y + offset));
                points.Add(new Point(end.X + dlx, start.Y + offset));
            }

            if (vertical)
            {
                double dx = 1;
                double dy = end.Y > start.Y ? 1 : -1;
                double dly = end.Y > start.Y ? 6 : -6;

                points.Add(new Point(start.X + offset, start.Y - dly));
                points.Add(new Point(start.X + offset, start.Y));
                points.Add(new Point(start.X - dx + offset, start.Y - dy));
                points.Add(new Point(start.X + dx + offset, start.Y + dy));
                points.Add(new Point(start.X + offset, start.Y));

                points.Add(new Point(start.X + offset, start.Y));
                points.Add(new Point(end.X + offset, end.Y));

                points.Add(new Point(end.X - dx + offset, end.Y - dy));
                points.Add(new Point(end.X + dx + offset, end.Y + dy));
                points.Add(new Point(end.X + offset, end.Y));
                points.Add(new Point(end.X + offset, end.Y + dly));
            }

            polyline.Points = points;
            polyline.Stretch = Stretch.None;
            polyline.Stroke = Brushes.Gray;
            polyline.StrokeThickness = 1;

            wallViewer.Children.Add(polyline);
        }
    }
}
