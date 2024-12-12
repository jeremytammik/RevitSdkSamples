//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using System.Drawing;
using Autodesk.Revit;
using System.Collections.ObjectModel;

namespace Revit.SDK.Samples.Openings.CS
{
    /// <summary>
    /// This class contain the data about the Opening (get from Revit)
    /// Such as BoundingBox, Profile Curve...
    /// </summary>
    public class OpeningInfo
    {
        private UIApplication m_revit; //Application of Revit
        private List<Line3D> m_lines = new List<Line3D>(); //contains lines in curve
        private Opening m_opening; //Opening got from Revit
        
        //OpeningProperty class which can use in PropertyGrid control
        private OpeningProperty m_property;
        private WireFrame m_sketch; //Profile information of opening
        private BoundingBox m_boundingBox;  //BoundingBox of Opening

        //property
        /// <summary>
        /// Property to get and set Application of Revit
        /// </summary>
        public UIApplication Revit
        {
            get
            {
                return m_revit;
            }
            set
            {
                if (value != m_revit)
                    m_revit = value;
            }
        }

        /// <summary>
        /// Property to get Opening store in OpeningInfo
        /// </summary>
        public Opening Opening
        {
            get
            {
                return m_opening;
            }
        }

        /// <summary>
        /// Property to get Name and Id 
        /// eg: "Opening Cut (114389)"
        /// </summary>
        public string NameAndId
        {
            get
            {
                return String.Concat(m_opening.Name, " (", m_opening.Id.IntegerValue.ToString(), ")");
            }
        }

        /// <summary>
        /// Property to get bool the define whether opening is Shaft Opening
        /// </summary>
        public bool IsShaft
        {
            get
            {
                if (null != m_opening.Category)
                {
                    if ("Shaft Openings" == m_opening.Category.Name)
                        return true;
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Property to get OpeningProperty class 
        /// which can use in PropertyGrid control
        /// </summary>
        public OpeningProperty Property
        {
            get
            {
                return m_property;
            }
        }

        /// <summary>
        /// Property to get Profile information of opening
        /// </summary>
        public WireFrame Sketch
        {
            get
            {
                return m_sketch;
            }
        }

        /// <summary>
        /// Property to get BoundingBox of Opening
        /// </summary>
        public BoundingBox BoundingBox
        {
            get
            {
                return m_boundingBox;
            }
        }

        /// <summary>
        /// The default constructor, 
        /// get the information we want from Opening
        /// get OpeningProperty, BoundingBox and Profile
        /// </summary>
        /// <param name="opening">an opening in revit</param>
        /// <param name="app">application object</param>
        public OpeningInfo(Opening opening, UIApplication app)
        {
            m_opening = opening;
            m_revit = app;

            //get OpeningProperty which can use in PropertyGrid control
            OpeningProperty openingProperty = new OpeningProperty(m_opening);
            m_property = openingProperty;

            //get BoundingBox of Opening
            BoundingBoxXYZ boxXYZ = m_opening.get_BoundingBox(m_revit.ActiveUIDocument.Document.ActiveView);
            BoundingBox boundingBox = new BoundingBox(boxXYZ);
            m_boundingBox = boundingBox;

            //get profile
            GetProfile();
        }

        /// <summary>
        /// get Profile of Opening
        /// </summary>
        private void GetProfile()
        {
            CurveArray curveArray = m_opening.BoundaryCurves;
            if (null != curveArray)
            {
                m_lines.Clear();
                foreach (Curve curve in curveArray)
                {
                    List<XYZ> points = curve.Tessellate() as List<XYZ>;
                    AddLine(points);
                }
                WireFrame wireFrameSketch = new WireFrame(new ReadOnlyCollection<Line3D>(m_lines));
                m_sketch = wireFrameSketch;
            }
            else if (m_opening.IsRectBoundary)
            {
                //if opening profile is RectBoundary, 
                //just can get profile info from BoundaryRect Property
                m_lines.Clear();
                List<XYZ> boundRect = m_opening.BoundaryRect as List<XYZ>;
                List<XYZ> RectPoints = GetPoints(boundRect);
                AddLine(RectPoints);
                WireFrame wireFrameSketch = new WireFrame(new ReadOnlyCollection<Line3D>(m_lines));
                m_sketch = wireFrameSketch;
            }
            else
            {
                m_sketch = null;
            }
        }

        /// <summary>
        /// get four corner points of a rectangular in same plane
        /// </summary>
        /// <param name="boundRect">an array contain two Autodesk.Revit.DB.XYZ struct store the max and min 
        /// coordinate of rectangular</param>
        private List<XYZ> GetPoints(List<XYZ> boundRect)
        {
            List<XYZ> points = new List<XYZ>();
            Autodesk.Revit.DB.XYZ p1 = boundRect[0];
            points.Add(p1);

            Autodesk.Revit.DB.XYZ p2 = new Autodesk.Revit.DB.XYZ(
                boundRect[0].X,
                boundRect[0].Y,
                boundRect[1].Z);
            points.Add(p2);

            Autodesk.Revit.DB.XYZ p3 = boundRect[1];
            points.Add(p3);

            Autodesk.Revit.DB.XYZ p4 = new Autodesk.Revit.DB.XYZ (
                boundRect[1].X,
                boundRect[1].Y,
                boundRect[0].Z);
            points.Add(p4);

            //make rectangle close
            Autodesk.Revit.DB.XYZ p5 = boundRect[0];
            points.Add(p5);

            return points;
        }

        /// <summary>
        /// get line from List<XYZ>(points) and add line to m_lines list
        /// </summary>
        /// <param name="points">a List<XYZ> contain points of the Curve</param>
        private void AddLine(List<XYZ> points)
        {
            if (null == points || 0 == points.Count)
            {
                return;
            }

            Autodesk.Revit.DB.XYZ previousPoint;
            previousPoint = points[0];

            for (int i = 1; i < points.Count; i++)
            {
                Autodesk.Revit.DB.XYZ point;
                point = points[i];

                Line3D line = new Line3D();
                Vector pointStart = new Vector();
                Vector pointEnd = new Vector();
                for (int j = 0; j < 3; j++)
                {
                    pointStart[j] = previousPoint[j];
                    pointEnd[j] = point[j];
                }
                line.StartPoint = pointStart;
                line.EndPoint = pointEnd;

                m_lines.Add(line);

                previousPoint = point;
            }
        }
    }
}
