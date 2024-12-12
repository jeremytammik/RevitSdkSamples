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

using Autodesk.Revit.DB.CodeChecking.Engineering;

using XYZ = Autodesk.Revit.DB.XYZ;

namespace SectionPropertiesExplorer
{
    /// <summary>
    /// Interaction logic for SectionDescription.xaml
    /// </summary>
    public partial class SectionDescriptionControl : UserControl
    {
        public SectionDescriptionControl()
        {
            InitializeComponent();
        }

        public void DoBinding(SectionShapeType shapeType, ElementSectionsInfo sectionsInfo, bool isSteel = false)
        {
            sectionTypeName.Content = Tools.SectionShapeTypeName(shapeType);
            
            BitmapImage source = new BitmapImage();
            source.BeginInit();
            string uriString = "/SectionPropertiesExplorer;component/Resources/Images/section";
            string typeName = Tools.SectionShapeTypeName(shapeType);
            string steelTxt = "";
            if (shapeType == SectionShapeType.Z && isSteel)
            {
                steelTxt = "steel";
            }
            uriString += typeName + steelTxt + ".png";
            source.UriSource = new Uri(uriString, UriKind.Relative);
            source.EndInit();
            sectionDefinition.Source = source;

            AddShapesToRender(sectionsInfo);
        }

        private double ScaleBetweenSections(SectionsInfo secAtTheBeg, SectionsInfo secAtTheEnd)
        {
            XYZ xyz_min_Beg = secAtTheBeg.GetMinimumBoundary();
            XYZ xyz_max_Beg = secAtTheBeg.GetMaximumBoundary();
            XYZ xyz_min_End = secAtTheEnd.GetMinimumBoundary();
            XYZ xyz_max_End = secAtTheEnd.GetMaximumBoundary();

            double widthBeg = Math.Abs(xyz_max_Beg.X - xyz_min_Beg.X);
            double widthEnd = Math.Abs(xyz_max_End.X - xyz_min_End.X);

            double heightBeg = Math.Abs(xyz_max_Beg.Y - xyz_min_Beg.Y);
            double heightEnd = Math.Abs(xyz_max_End.Y - xyz_min_End.Y);

            double widthScale = widthEnd / widthBeg;
            double heightScale = heightEnd / heightBeg;

            if (widthScale > 1.0)
            {
                if (heightScale > 1.0)
                {
                    return Math.Max(widthScale, heightScale);
                }
                else
                {
                    if (Math.Max(widthScale, 1.0 / heightScale) > widthScale)
                    {
                        return heightScale;
                    }
                    else
                    {
                        return widthScale;
                    }
                }
            }
            else
            {
                if (heightScale < 1.0)
                {
                    return Math.Min(widthScale, heightScale);
                }
                else
                {
                    if (Math.Min(widthScale, 1.0 / heightScale) < widthScale)
                    {
                        return heightScale;
                    }
                    else 
                    {
                        return widthScale;
                    }
                }
            }
        }

        private void AddShapesToRender(ElementSectionsInfo info)
        {
            double scale = ScaleBetweenSections(info.AtTheBeg, info.AtTheEnd);
            AddShapesToRender(info.AtTheBeg, sectionAtTheBegViewer, scale > 1.0 ? 1.0/scale : 1.0);
            AddShapesToRender(info.AtTheEnd, sectionAtTheEndViewer, scale > 1.0 ? 1.0 : scale);
        }

        private void AddShapesToRender(SectionsInfo sectionsInfo, Canvas sectionViewer, double scale)
        {
            sectionViewer.Children.Clear();

            double actualWidth = sectionViewer.Width;
            double actualHeight = sectionViewer.MinHeight;

            if (sectionViewer.ActualWidth > 0)
            {
                actualWidth = sectionViewer.ActualWidth;
            }
            
            if (sectionViewer.ActualHeight > 0)
            {
                actualHeight = sectionViewer.ActualHeight;
            }

            double horizontalMargin = actualWidth / 6;
            double verticalMargin = actualHeight / 6;

            double width = actualWidth - 2 * horizontalMargin;
            double height = actualHeight - 2 * verticalMargin;
            
            horizontalMargin += (width - width * scale) / 2.0;
            verticalMargin += (height - height * scale) / 2.0;

            double horizontalShift = horizontalMargin;
            double verticalShift = verticalMargin;

            XYZ xyz_min = sectionsInfo.GetMinimumBoundary();
            XYZ xyz_max = sectionsInfo.GetMaximumBoundary();
            
            width = Math.Abs(xyz_max.X - xyz_min.X);
            height = Math.Abs(xyz_max.Y - xyz_min.Y);

            double stretchCoeff = 0;
            if (height > width)
            {
                stretchCoeff = (actualHeight - 2 * verticalMargin) / height;
                horizontalShift += (height - width) * stretchCoeff / 2; 
            }
            else
            {
                stretchCoeff = (actualWidth - 2 * horizontalMargin) / width;
                verticalShift += (width - height) * stretchCoeff / 2;
            }
            
            if (xyz_min.X < 0)
                horizontalShift -= stretchCoeff * xyz_min.X;
            if (xyz_min.Y < 0)
                verticalShift -= stretchCoeff * xyz_min.Y; 

            List<System.Windows.Shapes.Shape> contours = new List<System.Windows.Shapes.Shape>();
            List<System.Windows.Shapes.Shape> holes = new List<System.Windows.Shapes.Shape>();

            XYZ maxB = sectionsInfo.GetMaximumBoundary();
            XYZ minB = sectionsInfo.GetMinimumBoundary();

            foreach (Autodesk.Revit.DB.CodeChecking.Engineering.Section section in sectionsInfo.Sections)
            {
                Polygon polygon = new Polygon();
                PointCollection points = new PointCollection();
                foreach (XYZ xyz in section.Contour.Points)
                {
                    points.Add(new Point(xyz.X * stretchCoeff, (minB.Y + (maxB.Y - xyz.Y)) * stretchCoeff));
                }
                polygon.Points = points;
                polygon.Fill = Brushes.LightSteelBlue;
                polygon.Stretch = Stretch.None;
                polygon.Stroke = Brushes.Black;
                polygon.StrokeThickness = 1;

                contours.Add(polygon);

                foreach (Autodesk.Revit.DB.CodeChecking.Engineering.Shape shape in section.Holes)
                {
                    polygon = new Polygon();
                    points = new PointCollection();
                    foreach (XYZ xyz in shape.Points)
                    {
                        points.Add(new Point(xyz.X * stretchCoeff, (minB.Y + (maxB.Y - xyz.Y)) * stretchCoeff));
                    }
                    polygon.Points = points;
                    polygon.Fill = Brushes.White;
                    polygon.Stretch = Stretch.None;
                    polygon.Stroke = Brushes.Black;
                    polygon.StrokeThickness = 1;

                    holes.Add(polygon);
                }
            }

            if (contours.Count() < 1)
            {
                horizontalShift = (actualWidth - defaultShapeToRender.Width) / 2;
                verticalShift = (actualHeight - defaultShapeToRender.Height) / 2;
                Canvas.SetLeft(defaultShapeToRender, horizontalShift);
                Canvas.SetTop(defaultShapeToRender, verticalShift);
                sectionViewer.Children.Add(defaultShapeToRender);
                return;
            }

            foreach (System.Windows.Shapes.Shape contour in contours)
            {
                Canvas.SetLeft(contour, horizontalShift);
                Canvas.SetTop(contour, verticalShift);
                sectionViewer.Children.Add(contour);
            }

            foreach (System.Windows.Shapes.Shape hole in holes)
            {
                Canvas.SetLeft(hole, horizontalShift);
                Canvas.SetTop(hole, verticalShift);
                sectionViewer.Children.Add(hole);
            }
        }

        private void DefaultShapeToRender()
        {
            defaultShapeToRender = new Rectangle()
            {
                Fill = Brushes.LightSteelBlue,
                Height = 100,
                Width = 100,
                RadiusX = 5,
                RadiusY = 5,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
        }

        System.Windows.Shapes.Shape defaultShapeToRender = null;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (null == defaultShapeToRender)
                DefaultShapeToRender();
        }
    }
}
