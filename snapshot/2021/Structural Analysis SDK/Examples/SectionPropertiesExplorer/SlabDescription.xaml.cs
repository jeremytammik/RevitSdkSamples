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
    /// Interaction logic for SlabDescription.xaml
    /// </summary>
    public partial class SlabDescriptionControl : UserControl
    {
        public SlabDescriptionControl()
        {
            InitializeComponent();
        }

        public void DoBinding(ElementSlabsInfo slabsInfo, Autodesk.Revit.DB.DisplayUnit? unitSystem, double beamLength = 0)
        {
            this.slabsInfo = slabsInfo;
            this.unitSystem = unitSystem;
            this.beamLength = beamLength;
            if (slabsInfo.AsElement != null)
            {
                tSectionViewer.Visibility = System.Windows.Visibility.Collapsed;
                
                familyNameLabel.Visibility = System.Windows.Visibility.Visible;
                familyName.Visibility = System.Windows.Visibility.Visible;
                
                typeNameLabel.Visibility = System.Windows.Visibility.Visible;
                typeName.Visibility = System.Windows.Visibility.Visible;
                
                levelNameLabel.Visibility = System.Windows.Visibility.Visible;
                levelName.Visibility = System.Windows.Visibility.Visible;

                double bottomMargin = leftPanel.MinHeight - 
                                      familyName.Height - typeName.Height - levelName.Height -
                                      pointSupportSymbolViewer.Height - pLineSupportSymbolViewer.Height - wallSupportSymbolViewer.Height;
                levelNameLabel.Margin = new Thickness(levelNameLabel.Margin.Left, 
                                                      levelNameLabel.Margin.Top, 
                                                      levelNameLabel.Margin.Right, 
                                                      bottomMargin);
                levelName.Margin = new Thickness(levelName.Margin.Left,
                                                 levelName.Margin.Top,
                                                 levelName.Margin.Right,
                                                 bottomMargin);

                familyName.Content = slabsInfo.AsElement.FamilyName;
                typeName.Content = slabsInfo.AsElement.TypeName;
                levelName.Content = slabsInfo.AsElement.LevelName;
            }

            if (slabsInfo.TSection != null && slabsInfo.TSection.Adjoinings.Count > 0)
            {
                distansFromBeamStart = -1.0;
                tSectionSupportBegEnd = null;

                tSectionViewer.Visibility = System.Windows.Visibility.Visible;
                tSectionViewer.Height = tSectionViewer.MinHeight + familyName.Height + typeName.Height + levelName.Height;

                familyNameLabel.Visibility = System.Windows.Visibility.Collapsed;
                familyName.Visibility = System.Windows.Visibility.Collapsed;
                
                typeNameLabel.Visibility = System.Windows.Visibility.Collapsed;
                typeName.Visibility = System.Windows.Visibility.Collapsed;
                
                levelNameLabel.Visibility = System.Windows.Visibility.Collapsed;
                levelName.Visibility = System.Windows.Visibility.Collapsed;

            }

            AddToRenderInSlabsViewer(1.0);
            AddToRenderInTSectionViewer(1.0);
        }

        #region private main methods
        //--------------------------------------------------------------

        private void AddToRenderInSlabsViewer(double scale)
        {
            List<SlabInfo> slabs = new List<SlabInfo>(); 

            BoundaryBox boundingBox = new BoundaryBox();
            
            if (null == slabsInfo.AsElement)
            {
                if (null == slabsInfo.TSection || null == slabsInfo.TSection.Adjoinings || slabsInfo.TSection.Adjoinings.Count < 1)
                {
                    return;
                }

                slabs.AddRange(slabsInfo.TSection.Adjoinings);

                for (int s = 0; s < slabs.Count; s++)
                {
                    XYZ min = new XYZ(slabs[s].MinimumBoundary.X, slabs[s].MinimumBoundary.Y, slabs[s].MinimumBoundary.Z);
                    XYZ max = new XYZ(slabs[s].MaximumBoundary.X, slabs[s].MaximumBoundary.Y, slabs[s].MaximumBoundary.Z);

                    if (s > 0)
                    {
                        min = slabs[s].Transform.Point2Global(min);
                        max = slabs[s].Transform.Point2Global(max);
                        min = slabs[0].Transform.Point2Local(min);
                        max = slabs[0].Transform.Point2Local(max);
                    }

                    boundingBox.AddPoint(min);
                    boundingBox.AddPoint(max);
                }
            }
            else
            {
                slabs.Add(slabsInfo.AsElement);
                boundingBox.AddPoint(slabsInfo.AsElement.MaximumBoundary);
                boundingBox.AddPoint(slabsInfo.AsElement.MinimumBoundary);
            }

            slabsViewer.Children.Clear();
            
            double horizontalShift, verticalShift;
            slabsViewerStretchCoeff = CalculStretchCoeffAndShifting(boundingBox.Min, boundingBox.Max, scale, slabsViewer,
                                                                    out horizontalShift, out verticalShift);
            
            List<System.Windows.Shapes.Shape> contours = new List<System.Windows.Shapes.Shape>();
            List<System.Windows.Shapes.Shape> holes = new List<System.Windows.Shapes.Shape>();
            List<System.Windows.Shapes.Shape> pointSupports = new List<System.Windows.Shapes.Shape>();
            List<System.Windows.Shapes.Shape> pLineSupports = new List<System.Windows.Shapes.Shape>();
            List<System.Windows.Shapes.Shape> wallSupports = new List<System.Windows.Shapes.Shape>();

            XYZ xyz_min = boundingBox.Min;
            XYZ xyz_max = boundingBox.Max;

            Point pointBeamBeg = new Point();
            if (slabsInfo.TSection != null)
            {
                XYZ beamCenter = slabsInfo.TSection.BeamSections.Transform.Point2Global(slabsInfo.TSection.BeamSections.GetCenter());
                XYZ beamBeg = slabs[0].Transform.Point2Local(beamCenter);
                pointBeamBeg = new Point((beamBeg.X - xyz_min.X) * slabsViewerStretchCoeff + horizontalShift, (xyz_max.Y - beamBeg.Y) * slabsViewerStretchCoeff + verticalShift);
            }

            for (int s = 0; s < slabs.Count; s++)
            {
                foreach (SlabContour contour in slabs[s].Contours)
                {
                    if (contour.ContourType == SlabContourType.Contour &&
                        slabsInfo.TSection != null && slabsInfo.TSection.ContextId == contour.ContextId)
                    {
                        continue;
                    }

                    Polygon polygon = new Polygon();
                    PointCollection points = new PointCollection();
                    foreach (SlabEdge edge in contour.Edges)
                    {
                        foreach (SlabNode node in edge.Nodes)
                        {
                            XYZ xyz = node.Coordinates;

                            if (s > 0)
                            {
                                xyz = slabs[0].Transform.Point2Local(node.GetGlobalCoordinates());
                            }

                            points.Add(new Point(Math.Abs(xyz.X - xyz_min.X) * slabsViewerStretchCoeff,
                                                 Math.Abs(xyz_max.Y - xyz.Y) * slabsViewerStretchCoeff));
                        }
                    }
                    polygon.Points = points;
                    polygon.Stretch = Stretch.None;
                    
                    switch (contour.ContourType)
                    {
                        case SlabContourType.Main:
                            polygon.Fill = Brushes.LightSteelBlue;
                            polygon.Stroke = Brushes.Black;
                            polygon.StrokeThickness = 1;
                            contours.Add(polygon);
                            break;
                        case SlabContourType.Contour:
                            polygon.Fill = null;
                            polygon.Stroke = Brushes.Gray;
                            polygon.StrokeThickness = 1;
                            contours.Add(polygon);
                            break;
                        case SlabContourType.Hole:
                            polygon.Fill = Brushes.White;
                            polygon.Stroke = Brushes.Black;
                            polygon.StrokeThickness = 1;
                            holes.Add(polygon);
                            break;
                        case SlabContourType.Segment:
                            break;
                        default:
                            break;
                    }
                }

                if (contours.Count() < 1)
                {
                    return;
                }

                foreach (SlabNode node in slabs[s].Nodes)
                {
                    if (node.ParentContours.Count > 0 || node.ParentEdges.Count > 0)
                        continue;

                    System.Windows.Shapes.Shape point = CreatePointSupportSymbol(node.AdjoiningElementType);

                    XYZ xyz = node.Coordinates;

                    if (s > 0)
                    {
                        xyz = slabs[0].Transform.Point2Local(node.Coordinates);
                    }

                    double left = Math.Abs(xyz.X - xyz_min.X) * slabsViewerStretchCoeff - (point.Width / 2);
                    double top = Math.Abs(xyz_max.Y - xyz.Y) * slabsViewerStretchCoeff - (point.Height / 2);
                    point.Margin = new Thickness(left, top, 0, 0);

                    pointSupports.Add(point);
                }

                foreach (SlabEdge edge in slabs[s].Edges)
                {
                    if (edge.ParentContour != null && edge.ParentContour.ContourType != SlabContourType.Segment)
                        continue;

                    PointCollection points = new PointCollection();
                    foreach (SlabNode node in edge.Nodes)
                    {
                        XYZ xyz = node.Coordinates;

                        if (s > 0)
                        {
                            xyz = slabs[0].Transform.Point2Local(node.GetGlobalCoordinates());
                        }

                        points.Add(new Point(Math.Abs(xyz.X - xyz_min.X) * slabsViewerStretchCoeff,
                                             Math.Abs(xyz_max.Y - xyz.Y) * slabsViewerStretchCoeff));
                    }

                    List<System.Windows.Shapes.Shape> symbol;

                    switch (edge.AdjoiningElementType)
                    {
                        case SlabAdjoiningRevitElementType.FamilyInstance:
                            bool tSectionSupport = false;

                            if (slabsInfo.TSection != null && slabsInfo.TSection.Adjoinings != null && slabsInfo.TSection.Adjoinings.Count > 0)
                            {
                                if (edge.ContextId == slabsInfo.TSection.ContextId)
                                {
                                    tSectionSupport = true;
                                }
                            }

                            if (true == tSectionSupport)
                            {
                                Point pointBeg = new Point(points[0].X + horizontalShift, points[0].Y + verticalShift);
                                Point pointEnd = new Point(points[1].X + horizontalShift, points[1].Y + verticalShift);

                                if (null == tSectionSupportBegEnd)
                                {
                                    tSectionSupportBegEnd = new Point[2];

                                    tSectionSupportBegEnd[0] = pointBeg;
                                    tSectionSupportBegEnd[1] = pointEnd;
                                    
                                }
                                else
                                {
                                    XYZ tSectionSupportBeg = new XYZ(tSectionSupportBegEnd[0].X, tSectionSupportBegEnd[0].Y, 0);
                                    XYZ tSectionSupportEnd = new XYZ(tSectionSupportBegEnd[1].X, tSectionSupportBegEnd[1].Y, 0);
                                    XYZ ptBeg = new XYZ(pointBeg.X, pointBeg.Y, 0);
                                    XYZ ptEnd = new XYZ(pointEnd.X, pointEnd.Y, 0);

                                    if (!Geometry.IsPointOnLine(tSectionSupportBeg.getDblArray(), tSectionSupportEnd.getDblArray(), ptBeg.getDblArray()))
                                    {
                                        double distBeg = Geometry.Distance2D(tSectionSupportBeg.getDblArray(), ptBeg.getDblArray());
                                        double distEnd = Geometry.Distance2D(tSectionSupportEnd.getDblArray(), ptBeg.getDblArray());
                                        if (distBeg < distEnd)
                                        {
                                            tSectionSupportBegEnd[0] = pointBeg;
                                        }
                                        else
                                        {
                                            tSectionSupportBegEnd[1] = pointBeg;
                                        }
                                    }

                                    if (!Geometry.IsPointOnLine(tSectionSupportBeg.getDblArray(), tSectionSupportEnd.getDblArray(), ptEnd.getDblArray()))
                                    {
                                        double distBeg = Geometry.Distance2D(tSectionSupportBeg.getDblArray(), ptEnd.getDblArray());
                                        double distEnd = Geometry.Distance2D(tSectionSupportEnd.getDblArray(), ptEnd.getDblArray());
                                        if (distBeg < distEnd)
                                        {
                                            tSectionSupportBegEnd[0] = pointEnd;
                                        }
                                        else
                                        {
                                            tSectionSupportBegEnd[1] = pointEnd;
                                        }
                                    }
                                }

                                double distFromBeg = Geometry.Distance2D(new XYZ(pointBeamBeg.X, pointBeamBeg.Y, 0).getDblArray(),
                                                                         new XYZ(tSectionSupportBegEnd[0].X, tSectionSupportBegEnd[0].Y, 0).getDblArray());
                                if (distansFromBeamStart < 0 || distansFromBeamStart > distFromBeg)
                                {
                                    distansFromBeamStart = distFromBeg;
                                }
                            }

                            symbol = CreatePolyLineSupportSymbol(points, tSectionSupport);
                            pLineSupports.AddRange(symbol);
                            break;
                        case SlabAdjoiningRevitElementType.LowerFamilyInstance:
                        case SlabAdjoiningRevitElementType.CrossingFamilyInstance:
                        case SlabAdjoiningRevitElementType.UpperFamilyInstance:
                            break;
                        case SlabAdjoiningRevitElementType.UpperWall:
                        case SlabAdjoiningRevitElementType.Wall:
                        case SlabAdjoiningRevitElementType.LowerWall:
                            symbol = CreateWallSupportSymbol(points, edge.AdjoiningElementType);
                            wallSupports.AddRange(symbol);
                            break;
                    }
                }
            }

            if (distansFromBeamStart < 0)
            {
                distansFromBeamStart = 0.0;
            }
            else
            if (tSectionSupportBegEnd != null &&
                Geometry.IsPointOnLine(new XYZ(tSectionSupportBegEnd[0].X, tSectionSupportBegEnd[0].Y, 0).getDblArray(),
                                        new XYZ(tSectionSupportBegEnd[1].X, tSectionSupportBegEnd[1].Y, 0).getDblArray(),
                                        new XYZ(pointBeamBeg.X, pointBeamBeg.Y, 0).getDblArray()))
            {
                distansFromBeamStart *= -1.0;
            }

            foreach (System.Windows.Shapes.Shape contour in contours)
            {
                Canvas.SetLeft(contour, horizontalShift);
                Canvas.SetTop(contour, verticalShift);
                slabsViewer.Children.Add(contour);
            }

            foreach (System.Windows.Shapes.Shape hole in holes)
            {
                Canvas.SetLeft(hole, horizontalShift);
                Canvas.SetTop(hole, verticalShift);
                slabsViewer.Children.Add(hole);
            }

            foreach (System.Windows.Shapes.Shape plineSupport in pLineSupports)
            {
                Canvas.SetLeft(plineSupport, horizontalShift);
                Canvas.SetTop(plineSupport, verticalShift);
                slabsViewer.Children.Add(plineSupport);
            }

            foreach (System.Windows.Shapes.Shape wallSupport in wallSupports)
            {
                Canvas.SetLeft(wallSupport, horizontalShift);
                Canvas.SetTop(wallSupport, verticalShift);
                slabsViewer.Children.Add(wallSupport);
            }

            foreach (System.Windows.Shapes.Shape pointSupport in pointSupports)
            {
                Canvas.SetLeft(pointSupport, horizontalShift);
                Canvas.SetTop(pointSupport, verticalShift);
                slabsViewer.Children.Add(pointSupport);
            }

            if (null == slabsInfo.AsElement && null == slider)
            {
                double viewerWidth, viewerHeight;
                slabsViewer.GetViewerWidthAndHeight(out viewerWidth, out viewerHeight);

                TextBlock info = new TextBlock();
                info.Text = "Click on the red beam to set the calculation point !!!";
                info.Foreground = new SolidColorBrush(Colors.Red);
                info.Width = viewerWidth;
                info.TextAlignment = TextAlignment.Center;
                slabsViewer.Children.Add(info);
                dominantInfoText = info;
            }

            double dim = Math.Abs(xyz_max.Y - xyz_min.Y);

            double x = horizontalShift + Math.Abs(xyz_max.X - xyz_min.X) * slabsViewerStretchCoeff + 10;
            Point start = new Point(x, verticalShift);
            Point end = new Point(x, start.Y + dim * slabsViewerStretchCoeff);
            CreateDimensionLine("Width", dim, start, end, slabsViewerStretchCoeff, slabsViewer);

            dim = Math.Abs(xyz_max.X - xyz_min.X);

            double y = verticalShift + Math.Abs(xyz_max.Y - xyz_min.Y) * slabsViewerStretchCoeff + 10;
            start = new Point(horizontalShift, y);
            end = new Point(start.X + dim * slabsViewerStretchCoeff, y);
            CreateDimensionLine("Length", dim, start, end, slabsViewerStretchCoeff, slabsViewer);
            
            ShowPointSupportSymbolsViewer(pointSupports.Count > 0);

            if (pLineSupports.Count > 0)
            {
                pLineSupportSymbolViewer.Children.Clear();

                PointCollection points = new PointCollection();
                points.Add(new Point(0, 0));
                points.Add(new Point(0.6 * pLineSupportSymbolViewer.Width, 0));
                List<System.Windows.Shapes.Shape> pLineSupportSymbol = CreatePolyLineSupportSymbol(points);

                double symbolWidth = 0.6 * pLineSupportSymbolViewer.Width;
                double symbolHeight = 4;
                foreach (System.Windows.Shapes.Shape shape in pLineSupportSymbol)
                {
                    Canvas.SetLeft(shape, pLineSupportSymbolViewer.Width / 2 - symbolWidth / 2);
                    Canvas.SetTop(shape, pLineSupportSymbolViewer.Height / 2 - symbolHeight / 2);
                    pLineSupportSymbolViewer.Children.Add(shape);
                }

                pLineSupportSymbolViewer.Visibility = System.Windows.Visibility.Visible;
                pLineSupportSymbolLabel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                pLineSupportSymbolViewer.Visibility = System.Windows.Visibility.Collapsed;
                pLineSupportSymbolLabel.Visibility = System.Windows.Visibility.Collapsed;
            }

            ShowWallSupportSymbolsViewer(wallSupports.Count > 0);
        }

        private void AddToRenderInTSectionViewer(double scale)
        {
            if (slabsInfo.AsElement != null)
            {
                return;
            }
            else
                if (null == slabsInfo.TSection || null == slabsInfo.TSection.Adjoinings || slabsInfo.TSection.Adjoinings.Count < 1)
            {
                return;
            }
            
            tSectionViewer.Children.Clear();

            XYZ xyz_min = slabsInfo.TSection.BeamSections.GetMinimumBoundary();
            XYZ xyz_max = slabsInfo.TSection.BeamSections.GetMaximumBoundary();
            
            BoundaryBox bBBox = new BoundaryBox();
            bBBox.Min = new XYZ(xyz_min.X, xyz_min.Y, xyz_min.Z);
            bBBox.Max = new XYZ(xyz_max.X, xyz_max.Y, xyz_max.Z);

            double xc = xyz_min.X + Math.Abs(xyz_max.X - xyz_min.X) / 2;

            List<ToLocalProjection> projections = new List<ToLocalProjection>();

            double startRelativePosition = distansFromBeamStart < 0.0 ? 0.0 : distansFromBeamStart;
            startRelativePosition /= ((distansFromBeamStart > 0.0 ? 0.0 : -distansFromBeamStart) + (beamLength * slabsViewerStretchCoeff));
            startRelativePosition += 0.01;

            SlabInfo leftSlab = slabsInfo.TSection.AdjoiningSlabOnTheLeftSide(startRelativePosition);
            SlabInfo rightSlab = slabsInfo.TSection.AdjoiningSlabOnTheRightSide(startRelativePosition);
            
            bool rightSide = false;
            for (SlabInfo slab = leftSlab;  !(true == rightSide && null == slab); rightSide = true, slab = rightSlab)
            {
                if (null == slab)
                    continue;

                ToLocalProjection projection = new ToLocalProjection();
                projections.Add(projection);
                foreach (SlabContour contour in slab.Contours)
                {
                    if(contour.ContourType != SlabContourType.Main)
                    {
                        continue;                    
                    }

                    foreach (SlabEdge edge in contour.Edges)
                    {
                        foreach (SlabNode node in edge.Nodes)
                        {
                            XYZ xyz = slabsInfo.TSection.BeamSections.Transform.Point2Local(node.GetGlobalCoordinates());
                            
                            if (xyz.X < (xyz_min.X - Math.Abs(xyz_max.X - xyz_min.X)))
                            {
                                xyz = new XYZ(xyz_min.X - Math.Abs(xyz_max.X - xyz_min.X), xyz.Y, xyz.Z);
                            }
                            
                            if (xyz.X > (xyz_max.X + Math.Abs(xyz_max.X - xyz_min.X)))
                            {
                                xyz = new XYZ(xyz_max.X + Math.Abs(xyz_max.X - xyz_min.X), xyz.Y, xyz.Z);
                            }

                            if (leftSlab != rightSlab)
                            {
                                if (true == rightSide)
                                {
                                    if (xyz.X < xyz_min.X)
                                    {
                                        projection.RightHorizontalReversed = true;                                    
                                    }
                                }
                                else
                                {
                                    if (xyz.X > xyz_max.X)
                                    {
                                        projection.LeftHorizontalReversed = true;
                                    }
                                }
                            }
                            
                            bBBox.AddPoint(xyz);
                            
                            if (xyz.X < xc)
                            {
                                bBBox.AddPoint(new XYZ(xyz.X + 2*(xc - xyz.X), xyz.Y, xyz.Z));
                            }
                            else
                            {
                                bBBox.AddPoint(new XYZ(xyz.X - 2 * (xyz.X - xc), xyz.Y, xyz.Z));
                            }

                            projection.PointsTop.Add(xyz);
                            projection.BBoxTop.AddPoint(xyz);

                            xyz = slabsInfo.TSection.BeamSections.Transform.Point2Global(xyz);
                            xyz = slab.Transform.Point2Local(xyz);
                            xyz = new XYZ(xyz.X, xyz.Y, xyz.Z - slab.Thickness());
                            xyz = slab.Transform.Point2Global(xyz);
                            xyz = slabsInfo.TSection.BeamSections.Transform.Point2Local(xyz);

                            if (leftSlab != rightSlab)
                            {
                                if (true == rightSide)
                                {
                                    if (xyz.X < xyz_min.X)
                                    {
                                        projection.RightHorizontalReversed = true;
                                    }
                                }
                                else
                                {
                                    if (xyz.X > xyz_max.X)
                                    {
                                        projection.LeftHorizontalReversed = true;
                                    }
                                }
                            }

                            bBBox.AddPoint(xyz);
                            
                            if (xyz.X < xc)
                            {
                                bBBox.AddPoint(new XYZ(xyz.X + 2 * (xc - xyz.X), xyz.Y, xyz.Z));
                            }
                            else
                            {
                                bBBox.AddPoint(new XYZ(xyz.X - 2 * (xyz.X - xc), xyz.Y, xyz.Z));
                            }
                            
                            projection.PointsBottom.Add(xyz);
                            projection.BBoxBottom.AddPoint(xyz);
                        }
                    }
                }

                if (true == rightSide)
                    break;
            }

            double horizontalShift, verticalShift;
            double stretchCoeff = CalculStretchCoeffAndShifting(bBBox.Min, bBBox.Max, scale, tSectionViewer,
                                                                out horizontalShift, out verticalShift);

            List<System.Windows.Shapes.Shape> contours = new List<System.Windows.Shapes.Shape>();
            List<System.Windows.Shapes.Shape> holes = new List<System.Windows.Shapes.Shape>();
            List<System.Windows.Shapes.Shape> slabs = new List<System.Windows.Shapes.Shape>();
                     
            foreach (ToLocalProjection projection in projections)
            {
                Polygon polygon;
                PointCollection points;

                for (object proj = projection.PointsTop; ; proj = projection.PointsBottom)
                {
                    polygon = new Polygon();
                    points = new PointCollection();

                    double rx;

                    foreach (XYZ xyz in (List<XYZ>)proj)
                    {
                        rx = Math.Abs(xyz.X - bBBox.Min.X);
                        
                        if (true == projection.LeftHorizontalReversed)
                        {
                            rx = rx - bBBox.Max.X;
                        }

                        if (true == projection.RightHorizontalReversed)
                        {
                            rx = Math.Abs(bBBox.Max.X - bBBox.Min.X) - rx;
                        }
                        
                        points.Add(new Point(rx * stretchCoeff,
                                             Math.Abs(bBBox.Max.Y - xyz.Y) * stretchCoeff));
                    }

                    polygon.Points = points;
                    polygon.Stretch = Stretch.None;
                    polygon.Stroke = Brushes.Black;
                    polygon.StrokeThickness = 2;

                    slabs.Add(polygon);

                    if (proj == projection.PointsBottom)
                    {
                        break;
                    }
                }
            }

            foreach (Section section in slabsInfo.TSection.BeamSections.Sections)
            {
                Polygon polygon = new Polygon();
                PointCollection points = new PointCollection();
                foreach (XYZ xyz in section.Contour.Points)
                {
                    points.Add(new Point(Math.Abs(xyz.X - bBBox.Min.X) * stretchCoeff, Math.Abs(bBBox.Max.Y - xyz.Y) * stretchCoeff));
                }
                polygon.Points = points;
                polygon.Fill = Brushes.White;
                polygon.Stretch = Stretch.None;
                polygon.Stroke = Brushes.Black;
                polygon.StrokeThickness = 2;

                contours.Add(polygon);

                foreach (Autodesk.Revit.DB.CodeChecking.Engineering.Shape shape in section.Holes)
                {
                    polygon = new Polygon();
                    points = new PointCollection();
                    foreach (XYZ xyz in shape.Points)
                    {
                        points.Add(new Point(Math.Abs(xyz.X - bBBox.Min.X) * stretchCoeff, Math.Abs(bBBox.Max.Y - xyz.Y) * stretchCoeff));
                    }
                    polygon.Points = points;
                    polygon.Fill = Brushes.White;
                    polygon.Stretch = Stretch.None;
                    polygon.Stroke = Brushes.Black;
                    polygon.StrokeThickness = 2;

                    holes.Add(polygon);
                }
            }

            foreach (System.Windows.Shapes.Shape slab in slabs)
            {
                Canvas.SetLeft(slab, horizontalShift);
                Canvas.SetTop(slab, verticalShift);
                tSectionViewer.Children.Add(slab);
            }

            foreach (System.Windows.Shapes.Shape contour in contours)
            {
                Canvas.SetLeft(contour, horizontalShift);
                Canvas.SetTop(contour, verticalShift);
                tSectionViewer.Children.Add(contour);
            }

            foreach (System.Windows.Shapes.Shape hole in holes)
            {
                Canvas.SetLeft(hole, horizontalShift);
                Canvas.SetTop(hole, verticalShift);
                tSectionViewer.Children.Add(hole);
            }

            if (projections.Count > 0)
            {
                ToLocalProjection leftProjection = projections[0];
                ToLocalProjection rightProjection = projections[projections.Count - 1];

                double dim = Math.Abs(slabsInfo.TSection.BeamWidth());
                double y = verticalShift + Math.Abs(bBBox.Max.Y - xyz_min.Y) * stretchCoeff + 10;
                Point start = new Point(horizontalShift + Math.Abs(xyz_min.X - bBBox.Min.X) * stretchCoeff, y);
                Point end = new Point(start.X + dim * stretchCoeff, y);
                CreateDimensionLine("b", dim, start, end, stretchCoeff, tSectionViewer);

                double x = 0;
                bool isLeft = false;
                dim = slabsInfo.TSection.SlabThicknessOnTheLeftSide(startRelativePosition);
                if (dim > 0.0)
                {
                    isLeft = true;
                    start = new Point(x, verticalShift + Math.Abs(bBBox.Max.Y - (leftProjection.BBoxTop.Min.Y + leftProjection.BBoxTop.Max.Y) / 2) * stretchCoeff);
                    end = new Point(x, verticalShift + Math.Abs(bBBox.Max.Y - (leftProjection.BBoxBottom.Min.Y + leftProjection.BBoxBottom.Max.Y) / 2) * stretchCoeff);
                    CreateDimensionLine("hl", dim, start, end, stretchCoeff, tSectionViewer);
                }

                dim = slabsInfo.TSection.SlabThicknessOnTheRightSide(startRelativePosition);
                if (dim > 0.0)
                {
                    x = horizontalShift + ((!rightProjection.RightHorizontalReversed ? rightProjection.BBoxTop.Max.X :
                                                                                       (!isLeft ? bBBox.Max.X : leftProjection.BBoxTop.Max.X)) - bBBox.Min.X) * stretchCoeff;
                    start = new Point(x, verticalShift + Math.Abs(bBBox.Max.Y - (rightProjection.BBoxTop.Min.Y + rightProjection.BBoxTop.Max.Y) / 2) * stretchCoeff);
                    end = new Point(x, verticalShift + Math.Abs(bBBox.Max.Y - (rightProjection.BBoxBottom.Min.Y + rightProjection.BBoxBottom.Max.Y) / 2) * stretchCoeff);
                    CreateDimensionLine("hr", dim, start, end, stretchCoeff, tSectionViewer);
                }

                dim = slabsInfo.TSection.OverlappingDepthOnTheLeftSide(startRelativePosition);
                if (dim > 0.0)
                {
                    x = horizontalShift + Math.Abs(xyz_min.X - bBBox.Min.X) * stretchCoeff - 5;
                    start = new Point(x, verticalShift + Math.Abs(bBBox.Max.Y - xyz_max.Y) * stretchCoeff);
                    end = new Point(x, verticalShift + Math.Abs(bBBox.Max.Y - (leftProjection.BBoxBottom.Min.Y + leftProjection.BBoxBottom.Max.Y) / 2) * stretchCoeff);
                    CreateDimensionLine("wl", dim, start, end, stretchCoeff, tSectionViewer);
                }

                dim = slabsInfo.TSection.OverlappingDepthOnTheRightSide(startRelativePosition);
                if (dim > 0.0)
                {
                    x = horizontalShift + Math.Abs(xyz_max.X - bBBox.Min.X) * stretchCoeff + 5;
                    start = new Point(x, verticalShift + Math.Abs(bBBox.Max.Y - xyz_max.Y) * stretchCoeff);
                    end = new Point(x, verticalShift + Math.Abs(bBBox.Max.Y - (rightProjection.BBoxBottom.Min.Y + rightProjection.BBoxBottom.Max.Y) / 2) * stretchCoeff);
                    CreateDimensionLine("wr", dim, start, end, stretchCoeff, tSectionViewer);
                }
            }
        }

        private void MoveSliderToPointAndShowPosition(Point clickedPoint)
        {
            XYZ start = new XYZ(tSectionSupportBegEnd[0].X, tSectionSupportBegEnd[0].Y, 0);
            XYZ end = new XYZ(tSectionSupportBegEnd[1].X, tSectionSupportBegEnd[1].Y, 0);

            XYZ v = end - start;
            XYZ vp = new XYZ(-v.Y, v.X, 0);

            double[] pt;

            if (!Geometry.IsIntersectionFirstLine2D(start.getDblArray(), end.getDblArray(),
                                                    new XYZ(clickedPoint.X, clickedPoint.Y, 0).getDblArray(),
                                                    (new XYZ(clickedPoint.X, clickedPoint.Y, 0) + vp).getDblArray(),
                                                    out pt))
            {
                return;
            }
            
            double l = Geometry.Distance2D(start.getDblArray(), end.getDblArray());

            double relativePosition = (distansFromBeamStart < 0.0 ? 0.0 : distansFromBeamStart) + Geometry.Distance2D(start.getDblArray(), pt);
            relativePosition /= ((distansFromBeamStart > 0.0 ? 0.0 : -distansFromBeamStart) + (beamLength * slabsViewerStretchCoeff));

            Point ps = new Point(pt[0], pt[1]);

            double distance = slabsInfo.TSection.DistanceToNearestObjectOnTheLeftSide(relativePosition) * slabsViewerStretchCoeff;
            Point pe = new Point(ps.X + (v.Y / l) * distance, ps.Y + (-v.X / l) * distance); 

            leftSliderArrow = CreateSliderArrowSymbol(ps, pe, "left");
            slabsViewer.Children.Add(leftSliderArrow);

            distance = slabsInfo.TSection.DistanceToNearestObjectOnTheRightSide(relativePosition) * slabsViewerStretchCoeff;
            pe = new Point(ps.X + (-v.Y / l) * distance, ps.Y + (v.X / l) * distance); 

            rightSliderArrow = CreateSliderArrowSymbol(ps, pe, "right");
            slabsViewer.Children.Add(rightSliderArrow);

            slider = CreateSliderSymbol();

            Canvas.SetLeft(slider, pt[0] - slider.Width / 2);
            Canvas.SetTop(slider, pt[1] - slider.Height / 2);

            slabsViewer.Children.Add(slider);

            double viewerWidth, viewerHeight;
            slabsViewer.GetViewerWidthAndHeight(out viewerWidth, out viewerHeight);
            TextBlock info = new TextBlock();
            info.Text = "Relative position on beam = " + Math.Round(relativePosition, 2, MidpointRounding.AwayFromZero).ToString();
            info.Foreground = new SolidColorBrush(Colors.Red);
            info.Width = viewerWidth;
            info.TextAlignment = TextAlignment.Center;
            if (dominantInfoText != null)
            {
                slabsViewer.Children.Remove(dominantInfoText);
            }
            slabsViewer.Children.Add(info);
            dominantInfoText = info;

            ShowDistancesToNearestObjects(relativePosition);
        }

        private void ShowDistancesToNearestObjects(double relativePosition = 0.0)
        {
            double width, height;
            tSectionViewer.GetViewerWidthAndHeight(out width, out height);

            double y = 10;

            Point start;
            Point end;
            TSectionNearestObjectType nearestObjectType;

            double distance = slabsInfo.TSection.DistanceToNearestObjectOnTheLeftSide(relativePosition);
            if (distance > 0.0)
            {
                nearestObjectType = slabsInfo.TSection.TypeOfNearestObjectOnTheLeftSide(relativePosition);
                start = new Point(width / 2, y);
                end = new Point(1, y);
                CreateDimensionLine("DistanceToNearestObject", distance, start, end, 1.0, nearestObjectType, tSectionViewer);
            }

            distance = slabsInfo.TSection.DistanceToNearestObjectOnTheRightSide(relativePosition);
            if (distance > 0.0)
            {
                nearestObjectType = slabsInfo.TSection.TypeOfNearestObjectOnTheRightSide(relativePosition);
                start = new Point(width / 2, y);
                end = new Point(width, y);
                CreateDimensionLine("DistanceToNearestObject", distance, start, end, 1.0, nearestObjectType, tSectionViewer);
            }
        }

        private double CalculStretchCoeffAndShifting(XYZ xyz_min, XYZ xyz_max, double scale, Canvas viewer,
                                                    out double horizontalShift, out double verticalShift)
        {
            double viewerWidth, viewerHeight;
            viewer.GetViewerWidthAndHeight(out viewerWidth, out viewerHeight);

            double horizontalMargin = viewerWidth / 10;
            double verticalMargin = viewerHeight / 10;

            double width = viewerWidth - 2 * horizontalMargin;
            double height = viewerHeight - 2 * verticalMargin;

            horizontalMargin += (width - width * scale) / 2.0;
            verticalMargin += (height - height * scale) / 2.0;

            width = Math.Abs(xyz_max.X - xyz_min.X);
            height = Math.Abs(xyz_max.Y - xyz_min.Y);

            double stretchCoeff = Math.Min((viewerWidth - 2 * horizontalMargin) / width,
                                           (viewerHeight - 2 * verticalMargin) / height);

            horizontalShift = horizontalMargin;
            verticalShift = verticalMargin;

            horizontalShift += (viewerWidth - 2 * horizontalMargin - width * stretchCoeff) / 2;
            verticalShift += (viewerHeight - 2 * verticalMargin - height * stretchCoeff) / 2;

            return stretchCoeff;
        }

        //--------------------------------------------------------------
        #endregion

        #region creating symbols and dimensiom lines
        //--------------------------------------------------------------

        private List<System.Windows.Shapes.Shape> CreatePolyLineSupportSymbol(PointCollection points, bool TSectionSupport = false)
        {
            List<System.Windows.Shapes.Shape> listShapes = new List<System.Windows.Shapes.Shape>();

            Polyline polyline = new Polyline();

            polyline.Points = points;
            polyline.Stretch = Stretch.None;
            polyline.Stroke = TSectionSupport ? Brushes.DarkRed : Brushes.Black;
            polyline.StrokeThickness = TSectionSupport ? 5 : 4;

            listShapes.Add(polyline);

            polyline = new Polyline();

            polyline.Points = points;
            polyline.Stretch = Stretch.None;
            polyline.Stroke = TSectionSupport ? Brushes.Red : Brushes.LightSeaGreen;
            polyline.StrokeThickness = TSectionSupport ? 3 : 2;

            listShapes.Add(polyline);

            return listShapes;
        }

        private List<System.Windows.Shapes.Shape> CreateWallSupportSymbol(PointCollection points, SlabAdjoiningRevitElementType elementOriginType)
        {
            List<System.Windows.Shapes.Shape> listShapes = new List<System.Windows.Shapes.Shape>();

            Polyline polyline1 = new Polyline();
            Polyline polyline2 = new Polyline();

            polyline1.Points = points;
            polyline1.Stretch = Stretch.None;

            polyline2.Points = points;
            polyline2.Stretch = Stretch.None;

            switch (elementOriginType)
            {
                case SlabAdjoiningRevitElementType.UpperFamilyInstance:
                    polyline1.StrokeThickness = 4;
                    polyline2.StrokeThickness = 2;
                    polyline1.Stroke = Brushes.Gray;
                    polyline2.Stroke = Brushes.DarkOrange;
                    break;
                case SlabAdjoiningRevitElementType.CrossingFamilyInstance:
                    polyline1.StrokeThickness = 4;
                    polyline2.StrokeThickness = 2;
                    polyline1.Stroke = Brushes.Black;
                    polyline2.Stroke = Brushes.Yellow;
                    break;
                case SlabAdjoiningRevitElementType.LowerFamilyInstance:
                default:
                    polyline1.StrokeThickness = 4;
                    polyline2.StrokeThickness = 2;
                    polyline1.Stroke = Brushes.Black;
                    polyline2.Stroke = Brushes.Orange;
                    break;
            }

            listShapes.Add(polyline1);
            listShapes.Add(polyline2);

            return listShapes;
        }

        private System.Windows.Shapes.Shape CreatePointSupportSymbol(SlabAdjoiningRevitElementType elementOriginType)
        {
            if (elementOriginType != SlabAdjoiningRevitElementType.LowerFamilyInstance)
            {
                Ellipse circle = new Ellipse();

                circle.Width = 6;
                circle.Height = 6;
                circle.Stretch = Stretch.Fill;
                circle.StrokeThickness = 1;

                switch (elementOriginType)
                {
                    case SlabAdjoiningRevitElementType.UpperFamilyInstance:
                        circle.Stroke = null;
                        circle.StrokeThickness = 0;
                        circle.Fill = Brushes.Magenta;
                        break;
                    default:
                    case SlabAdjoiningRevitElementType.CrossingFamilyInstance:
                        circle.Stroke = Brushes.Black;
                        circle.StrokeThickness = 1;
                        circle.Fill = Brushes.Magenta;
                        break;
                }
                return circle;
            }
            else 
            {
                Ellipse circle = new Ellipse();

                circle.Width = 8;
                circle.Height = 8;
                circle.Stretch = Stretch.Fill;
                circle.StrokeThickness = 1;
                circle.Stroke = Brushes.Black;
                circle.StrokeThickness = 1;
                circle.Fill = Brushes.Black;

                return circle;
            }
        }
        
        private Ellipse CreateSliderSymbol()
        {
            if (slider != null)
            {
                slabsViewer.Children.Remove(slider);
            }

            Ellipse circle = new Ellipse();

            circle.Width = 13;
            circle.Height = 13;
            circle.Fill = Brushes.Red;
            circle.Stretch = Stretch.Fill;
            circle.Stroke = Brushes.DarkRed;
            circle.StrokeThickness = 1;

            return circle;
        }

        private Polyline CreateSliderArrowSymbol(Point start, Point end, string side)
        {
            double dx = 3;

            Polyline arrow = new Polyline();

            switch (side)
            {
                case "left":
                    if (leftSliderArrow != null)
                    {
                        slabsViewer.Children.Remove(leftSliderArrow);
                    }
                    break;
                case "right":
                    if (rightSliderArrow != null)
                    {
                        slabsViewer.Children.Remove(rightSliderArrow);
                    }
                    break;
            }

            PointCollection points = new PointCollection();

            double [] vs = { start.X, start.Y };
            double [] ve = { end.X, end.Y };
            double l = Geometry.Distance2D(vs, ve);

            points.Add(new Point(start.X, start.Y));
            points.Add(new Point(start.X + l, start.Y));

            points.Add(new Point(start.X + l - dx, start.Y - 1));
            points.Add(new Point(start.X + l, start.Y));
            points.Add(new Point(start.X + l - 2.5 * dx, start.Y + 2));

            arrow.Points = points;
            arrow.Stretch = Stretch.None;
            arrow.Stroke = Brushes.White;
            arrow.StrokeThickness = 1;

            
            double angle = Geometry.AngleBetweenX(vs, ve);

            arrow.RenderTransform = new RotateTransform(angle * 360 / (2 * Math.PI), start.X, start.Y);

            return arrow;
        }

        private void ShowPointSupportSymbolsViewer(bool show)
        {
            SlabAdjoiningRevitElementType originElementType = SlabAdjoiningRevitElementType.CrossingFamilyInstance;
            Canvas viewer = pointSupportSymbolViewer;

            for (int n = 1; n <= 3; n++)
            {
                switch (n)
                {
                    case 1:
                        originElementType = SlabAdjoiningRevitElementType.UpperFamilyInstance;
                        viewer = pointUpperSupportSymbolViewer;
                        break;
                    case 2:
                        originElementType = SlabAdjoiningRevitElementType.CrossingFamilyInstance;
                        viewer = pointSupportSymbolViewer;
                        break;
                    case 3:
                        originElementType = SlabAdjoiningRevitElementType.LowerFamilyInstance;
                        viewer = pointLowerSupportSymbolViewer;
                        break;
                    default:
                        break;
                }


                if (show)
                {
                    viewer.Children.Clear();

                    System.Windows.Shapes.Shape pointSupportSymbol = CreatePointSupportSymbol(originElementType);

                    Canvas.SetLeft(pointSupportSymbol, viewer.Width / 2 - pointSupportSymbol.Width / 2);
                    Canvas.SetTop(pointSupportSymbol, viewer.Height / 2 - pointSupportSymbol.Height / 2);
                    viewer.Children.Add(pointSupportSymbol);

                    viewer.Visibility = System.Windows.Visibility.Visible;
                    if (n == 1)
                    {
                        pointSupportSymbolLabel.Visibility = System.Windows.Visibility.Visible;
                    }
                }
                else
                {
                    viewer.Visibility = System.Windows.Visibility.Collapsed;
                    if (n == 1)
                    {
                        pointSupportSymbolLabel.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
            }
        }

        private void ShowWallSupportSymbolsViewer(bool show)
        {
            SlabAdjoiningRevitElementType originElementType = SlabAdjoiningRevitElementType.CrossingFamilyInstance;
            Canvas viewer = wallSupportSymbolViewer;

            for (int n = 1; n <= 3; n++)
            {
                switch (n)
                {
                    case 1:
                        originElementType = SlabAdjoiningRevitElementType.UpperFamilyInstance;
                        viewer = wallUpperSupportSymbolViewer;
                        break;
                    case 2:
                        originElementType = SlabAdjoiningRevitElementType.CrossingFamilyInstance;
                        viewer = wallSupportSymbolViewer;
                        break;
                    case 3:
                        originElementType = SlabAdjoiningRevitElementType.LowerFamilyInstance;
                        viewer = wallLowerSupportSymbolViewer;
                        break;
                    default:
                        break;
                }


                if (show)
                {
                    viewer.Children.Clear();

                    viewer.Children.Clear();

                    PointCollection points = new PointCollection();
                    points.Add(new Point(0, 0));
                    points.Add(new Point(0, 0.6 * viewer.Height));
                    List<System.Windows.Shapes.Shape> wallSupportSymbol = CreateWallSupportSymbol(points, originElementType);

                    double symbolHeight = 0.6 * viewer.Height;
                    foreach (System.Windows.Shapes.Shape shape in wallSupportSymbol)
                    {
                        Canvas.SetLeft(shape, viewer.Width / 2);
                        Canvas.SetTop(shape, viewer.Height / 2 - symbolHeight / 2);
                        viewer.Children.Add(shape);
                    }

                    viewer.Visibility = System.Windows.Visibility.Visible;
                    if (n == 1)
                    {
                        wallSupportSymbolLabel.Visibility = System.Windows.Visibility.Visible;
                    }
                }
                else
                {
                    viewer.Visibility = System.Windows.Visibility.Collapsed;
                    if (n == 1)
                    {
                        wallSupportSymbolLabel.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
            }
        }

        private void CreateDimensionLine(string dimName, double dim,
                                         Point start, Point end,
                                         double stretchCoeff,
                                         Canvas viewer)
        {
            CreateDimensionLine(dimName, dim, start, end, stretchCoeff, TSectionNearestObjectType.Unknown, viewer);
        }

        private void CreateDimensionLine(string dimName, double dim,
                                         Point start, Point end,
                                         double stretchCoeff,
                                         TSectionNearestObjectType nearestObjectType,
                                         Canvas viewer)
        {
            bool horizontal = Math.Abs(end.Y - start.Y) < 1 ? true : false;
            bool vertical = Math.Abs(end.X - start.X) < 1 ? true : false;

            bool inversePosition = false;

            bool dominantDimension = false;

            double dimValueWidth = dim * stretchCoeff;

            bool downAllignment = false;

            string dimValueText = "";
            UnitsAssignment[] assignments;
            switch (dimName)
            {
                case "Length":
                    dimName = "SlabDimension";
                    assignments = ElementInfoUnits.Assignments;
                    break;
                case "Width":
                    dimName = "SlabDimension";
                    assignments = ElementInfoUnits.Assignments;
                    break;
                case "b":
                    dimValueText = "b = ";
                    assignments = SectionDimensionsUnits.Assignments;
                    break;
                case "hl":
                    dimValueText = "hl = ";
                    assignments = ElementInfoUnits.Assignments;
                    dimName = "SlabThickness";
                    break;
                case "hr":
                    dimValueText = "hr = ";
                    assignments = ElementInfoUnits.Assignments;
                    dimName = "SlabThickness";
                    break;
                case "wl":
                    dimValueText = "wl = ";
                    assignments = ElementInfoUnits.Assignments;
                    dimName = "SlabDimension";
                    inversePosition = true;
                    downAllignment = true;
                    break;
                case "wr":
                    dimValueText = "wr = ";
                    assignments = ElementInfoUnits.Assignments;
                    dimName = "SlabDimension";
                    downAllignment = true;
                    break;
                case "DistanceToNearestObject":
                    dimValueText = end.X - start.X > 0 ? "Lr = " : "Ll = ";
                    dimValueWidth = Math.Abs(end.X - start.X);
                    assignments = ElementInfoUnits.Assignments;
                    dimName = "SlabDimension";
                    dominantDimension = true;
                    break;
                default:
                    return;
            }

            if (null == unitSystem)
            {
                dimValueText += UnitsAssignment.FormatToRevitUI(dimName, dim, assignments, true);
            }
            else
            {
                double val = Math.Round(dim, 6, MidpointRounding.AwayFromZero);
                dimValueText += val.ToString();
                dimValueText += " " + UnitsAssignment.GetUnitSymbol(dimName, assignments, (Autodesk.Revit.DB.DisplayUnit)unitSystem);
            }

            TextBlock dimValue = new TextBlock();
            dimValue.Text = dimValueText;
            dimValue.Foreground = new SolidColorBrush(dominantDimension ? Colors.Red : Colors.Gray);
            dimValue.Width = dimValueWidth;
            dimValue.TextAlignment = downAllignment ? TextAlignment.Left : TextAlignment.Center;

            double viewerWidth, viewerHeight;
            viewer.GetViewerWidthAndHeight(out viewerWidth, out viewerHeight);

            double dimValueStartX = start.X;
            if (horizontal)
            {
                if (!dominantDimension)
                {
                    double dx = viewerWidth - dimValueStartX - dimValueWidth;
                    if (dx > dimValueStartX)
                        dx = dimValueStartX;
                    dimValueStartX -= dx;
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
                dimValue.Width += 2 * dy;

                dimValue.RenderTransform = new RotateTransform(-90);

                Canvas.SetTop(dimValue, dimValueStartY);
            }

            Canvas.SetLeft(dimValue, (dominantDimension && start.X > end.X ? end.X : dimValueStartX) - (inversePosition && vertical ? dimValue.FontSize + 8 : 0));

            if (dominantDimension)
            {
                string text = "to ";
                switch (nearestObjectType)
                {
                    case TSectionNearestObjectType.Beam:
                        text += "beam";
                        break;
                    case TSectionNearestObjectType.UpperColumn:
                    case TSectionNearestObjectType.CrossingColumn:
                    case TSectionNearestObjectType.LowerColumn:
                        text += "column";
                        break;
                    case TSectionNearestObjectType.FreeEdgeOfSlab:
                        text += "edge of slab";
                        break;
                    case TSectionNearestObjectType.Hole:
                        text += "hole";
                        break;
                    case TSectionNearestObjectType.Unknown:
                        text = "";
                        break;
                    case TSectionNearestObjectType.UpperWall:
                    case TSectionNearestObjectType.Wall:
                    case TSectionNearestObjectType.LowerWall:
                        text += "wall";
                        break;
                    default:
                        break;
                }

                TextBlock dimInfo = new TextBlock();
                dimInfo.Text = text;
                dimInfo.Foreground = new SolidColorBrush(Colors.Red);
                dimInfo.Width = dimValue.Width;
                dimInfo.TextAlignment = TextAlignment.Center;

                Canvas.SetTop(dimInfo, start.Y + dimValue.FontSize + 8);
                Canvas.SetLeft(dimInfo, start.X > end.X ? end.X : dimValueStartX);

                if (end.X > start.X)
                {
                    viewer.Children.Remove(rightDominantTextBlock);
                    rightDominantTextBlock = dimValue;

                    viewer.Children.Remove(rightDominantInfoTextBlock);
                    rightDominantInfoTextBlock = dimInfo;
                }
                else
                {
                    viewer.Children.Remove(leftDominantTextBlock);
                    leftDominantTextBlock = dimValue;

                    viewer.Children.Remove(leftDominantInfoTextBlock);
                    leftDominantInfoTextBlock = dimInfo;
                }

                viewer.Children.Add(dimInfo);
            }

            viewer.Children.Add(dimValue);

            Polyline polyline = new Polyline();

            PointCollection points = new PointCollection();
            double offset = dimValue.FontSize + 5;

            if (inversePosition)
            {
                offset = -3;
            }

            if (horizontal)
            {
                double dlx = end.X > start.X ? 6 : -6;
                double dx = end.X > start.X ? 0.5 : -0.5;
                double dy = 1;

                if (!dominantDimension)
                {
                    dx *= 2;
                    points.Add(new Point(start.X - dlx, start.Y + offset));
                    points.Add(new Point(start.X, start.Y + offset));
                    points.Add(new Point(start.X + dx, start.Y - dy + offset));
                    points.Add(new Point(start.X - dx, start.Y + dy + offset));
                    points.Add(new Point(start.X, start.Y + offset));
                }
                else
                {
                    dx = end.X > start.X ? 3 : -3;
                    dy = 1;

                    points.Add(new Point(start.X + dx, start.Y + offset));
                }

                points.Add(new Point(end.X, end.Y + offset));

                if (dominantDimension)
                {
                    dx *= -1;
                    points.Add(new Point(end.X + dx, end.Y - 1 + offset));
                    points.Add(new Point(end.X, end.Y + offset));
                    points.Add(new Point(end.X + 2.5 * dx, end.Y + 2 + offset));
                }
                else
                {
                    points.Add(new Point(end.X + dx, start.Y - dy + offset));
                    points.Add(new Point(end.X - dx, start.Y + dy + offset));
                    points.Add(new Point(end.X, start.Y + offset));
                    points.Add(new Point(end.X + dlx, start.Y + offset));
                }
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
                if (!downAllignment)
                {
                    points.Add(new Point(end.X + offset, end.Y + dly));
                }
                else
                {
                    points.Add(new Point(end.X + offset, dimValueStartY));
                }
            }

            polyline.Points = points;
            polyline.Stretch = Stretch.None;
            polyline.Stroke = dominantDimension ? Brushes.Red : Brushes.Gray;
            polyline.StrokeThickness = 1;

            if (dominantDimension)
            {
                if (end.X > start.X)
                {
                    viewer.Children.Remove(rightDominantPolyline);
                    rightDominantPolyline = polyline;
                }
                else
                {
                    viewer.Children.Remove(leftDominantPolyline);
                    leftDominantPolyline = polyline;
                }
            }

            viewer.Children.Add(polyline);
        }

        //--------------------------------------------------------------
        #endregion

        #region mouse events
        //--------------------------------------------------------------

        private void slabsViewer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool test = TSectionSupportMouseTest(sender, e);

            if (true == test)
            {
                Point clickedPoint = e.GetPosition(slabsViewer);
                mouseLefButtonDown = true;
                MoveSliderToPointAndShowPosition(clickedPoint);
            }
        }

        private void slabsViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseLefButtonDown = false;
        }

        private void slabsViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseLefButtonDown)
            {
                return;            
            }
            
            bool test = TSectionSupportMouseTest(sender, e);
                                    
            if (true == test)
            {
                Point clickedPoint = e.GetPosition(slabsViewer);
                MoveSliderToPointAndShowPosition(clickedPoint);
            }
            else
            {
                mouseLefButtonDown = false;
            }
        }

        private bool TSectionSupportMouseTest(object sender, MouseEventArgs e)
        {
            if (null != sender)
            {
                Point clickedPoint = e.GetPosition(slabsViewer);
                HitTestResult SelectedCanvasItem = System.Windows.Media.VisualTreeHelper.HitTest(slabsViewer, clickedPoint);
                if (SelectedCanvasItem != null)
                {
                    if (SelectedCanvasItem.VisualHit.GetType() == typeof(Polyline))
                    {
                        var colorBrush = SelectedCanvasItem.VisualHit.GetValue(Polyline.StrokeProperty);
                        var thickness = SelectedCanvasItem.VisualHit.GetValue(Polyline.StrokeThicknessProperty);
                        if (colorBrush != null && thickness != null &&
                            (((SolidColorBrush)colorBrush == Brushes.DarkRed && (double)thickness == 5.0) ||
                            ((SolidColorBrush)colorBrush == Brushes.Red && (double)thickness == 3.0)))
                        {
                            return true;
                        }
                    }

                    if (SelectedCanvasItem.VisualHit.GetType() == typeof(Ellipse))
                    {
                        var strokeColorBrush = SelectedCanvasItem.VisualHit.GetValue(Ellipse.StrokeProperty);
                        var fillColorBrush = SelectedCanvasItem.VisualHit.GetValue(Polyline.FillProperty);
                        if (strokeColorBrush != null && (SolidColorBrush)strokeColorBrush == Brushes.DarkRed &&
                            fillColorBrush != null && (SolidColorBrush)fillColorBrush == Brushes.Red)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //--------------------------------------------------------------
        #endregion
   
        #region private members
        //--------------------------------------------------------------

        private class ToLocalProjection
        {
            public ToLocalProjection()
            {
                LeftHorizontalReversed = false;
                RightHorizontalReversed = false;
                PointsTop = new List<XYZ>();
                PointsBottom = new List<XYZ>();
                BBoxTop = new BoundaryBox();
                BBoxBottom = new BoundaryBox();
            }
            public bool LeftHorizontalReversed { get; set; }
            public bool RightHorizontalReversed { get; set; }
            public List<XYZ> PointsTop { get; private set; }
            public List<XYZ> PointsBottom { get; private set; }
            public BoundaryBox BBoxTop { get; private set; }
            public BoundaryBox BBoxBottom { get; private set; }
        }

        private double beamLength = 0.0;
        private double distansFromBeamStart = 0.0;
        private double slabsViewerStretchCoeff = 0.0;
        private ElementSlabsInfo slabsInfo;
        private Autodesk.Revit.DB.DisplayUnit? unitSystem;
        private Polyline leftDominantPolyline = null, rightDominantPolyline = null;
        private TextBlock dominantInfoText = null, 
                          leftDominantTextBlock = null, rightDominantTextBlock = null, 
                          leftDominantInfoTextBlock = null, rightDominantInfoTextBlock = null;
        private Ellipse slider = null;
        private Polyline leftSliderArrow = null, rightSliderArrow = null;
        private bool mouseLefButtonDown = false;
        
        Point [] tSectionSupportBegEnd;

        //--------------------------------------------------------------
        #endregion
    }
}
