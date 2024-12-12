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

using System.Collections;
using System.Drawing;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.GenerateFloor.CS
{
    /// <summary>
    /// obtain all data for this sample.
    /// all possible types for floor
    /// the level that walls based
    /// </summary>
    public class Data
    {
        private Hashtable m_floorTypes;
        private List<string> m_floorTypesName;
        private FloorType m_floorType;
        private Level m_level;
        private CurveArray m_profile;
        private bool m_structural;
        private System.Drawing.PointF[] m_points;
        private double m_maxLength;
        private const double PRECISION = 0.00000001;
        private Autodesk.Revit.Creation.Application m_creApp;

        /// <summary>
        /// A floor type to be used by the new floor instead of the default type.
        /// </summary>
        public FloorType FloorType
        {
            get
            {
                return m_floorType;
            }
            set
            {
                m_floorType = value;
            }
        }

        /// <summary>
        /// The level on which the floor is to be placed.
        /// </summary>
        public Level Level
        {
            get
            {
                return m_level;
            }
            set
            {
                m_level = value;
            }
        }

        /// <summary>
        /// A array of planar lines and arcs that represent the horizontal profile of the floor.
        /// </summary>
        public CurveArray Profile
        {
            get
            {
                return m_profile;
            }
            set
            {
                m_profile = value;
            }
        }

        /// <summary>
        /// If set, specifies that the floor is structural in nature.
        /// </summary>
        public bool Structural
        {
            get
            {
                return m_structural;
            }
            set
            {
                m_structural = value;
            }
        }

        /// <summary>
        /// Points to be draw.
        /// </summary>
        public System.Drawing.PointF[] Points
        {
            get
            {
                return m_points;
            }
            set
            {
                m_points = value;
            }
        }

        /// <summary>
        /// the graphics' max length
        /// </summary>
        public double MaxLength
        {
            get
            {
                return m_maxLength;
            }
            set
            {
                m_maxLength = value;
            }
        }

        /// <summary>
        /// List of all floor types name could be used by the floor.
        /// </summary>
        public List<string> FloorTypesName
        {
            get
            {
                return m_floorTypesName;
            }
            set
            {
                m_floorTypesName = value;
            }
        }

        /// <summary>
        /// Obtain all data which is necessary for generate floor.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        public void ObtainData(ExternalCommandData commandData)
        {
            if (null == commandData)
            {
                throw new ArgumentNullException("commandData");
            }

            UIDocument doc = commandData.Application.ActiveUIDocument;
            ElementSet walls = WallFilter(doc.Selection.Elements);
            m_creApp = commandData.Application.Application.Create;
            Profile = m_creApp.NewCurveArray();

            FilteredElementIterator iter = (new FilteredElementCollector(doc.Document)).OfClass(typeof(FloorType)).GetElementIterator();
            ObtainFloorTypes(iter);
            ObtainProfile(walls);
            ObtainLevel(walls);
            Generate2D();
            Structural = true;
        }

        /// <summary>
        /// Set the floor type to generate by its name.
        /// </summary>
        /// <param name="typeName">the floor type's name</param>
        public void ChooseFloorType(string typeName)
        {
            FloorType = m_floorTypes[typeName] as FloorType;
        }

        /// <summary>
        /// Obtain all types are available for floor.
        /// </summary>
        /// <param name="elements">all elements within the Document.</param>
        private void ObtainFloorTypes(FilteredElementIterator elements)
        {
            m_floorTypes = new Hashtable();
            FloorTypesName = new List<string>();

            elements.Reset();
            while (elements.MoveNext())
            {
                Autodesk.Revit.DB.FloorType ft = elements.Current as Autodesk.Revit.DB.FloorType;

                if (null == ft || null == ft.Category || !ft.Category.Name.Equals("Floors"))
                {
                    continue;
                }

                m_floorTypes.Add(ft.Name, ft);
                FloorTypesName.Add(ft.Name);
                FloorType = ft;
            }
        }

        /// <summary>
        /// Obtain the wall's level
        /// </summary>
        /// <param name="walls">the selection of walls that make a closed outline </param>
        private void ObtainLevel(ElementSet walls)
        {
            Autodesk.Revit.DB.Level temp = null;

            foreach (Wall w in walls)
            {
                if (null == temp)
                {
                    temp = w.Level;
                    Level = temp;
                }

                if (Level.Elevation != w.Level.Elevation)
                {
                    throw new InvalidOperationException("All walls should base the same level.");
                }
            }
        }

        /// <summary>
        /// Obtain a profile to generate floor.
        /// </summary>
        /// <param name="walls">the selection of walls that make a closed outline</param>
        private void ObtainProfile(ElementSet walls)
        {
            CurveArray temp = new CurveArray();
            foreach (Wall w in walls)
            {
                LocationCurve curve = w.Location as LocationCurve;
                temp.Append(curve.Curve);
            }
            SortCurves(temp);
        }

        /// <summary>
        /// Generate 2D data for preview pane.
        /// </summary>
        private void Generate2D()
        {
            ArrayList tempArray = new ArrayList();
            double xMin = 0;
            double xMax = 0;
            double yMin = 0;
            double yMax = 0;

            foreach (Curve c in Profile)
            {
               List<XYZ> xyzArray = c.Tessellate() as List<XYZ>;
               foreach (Autodesk.Revit.DB.XYZ xyz in xyzArray)
                {
                    Autodesk.Revit.DB.XYZ temp = new Autodesk.Revit.DB.XYZ (xyz.X, -xyz.Y, xyz.Z);
                    FindMinMax(temp, ref xMin, ref xMax, ref yMin, ref yMax);
                    tempArray.Add(temp);
                }
            }

            MaxLength = ((xMax - xMin) > (yMax - yMin)) ? (xMax - xMin) : (yMax - yMin);

            Points = new PointF[tempArray.Count / 2 + 1];
            for (int i = 0; i < tempArray.Count; i = i + 2)
            {
                Autodesk.Revit.DB.XYZ point = (Autodesk.Revit.DB.XYZ)tempArray[i];
                Points.SetValue(new PointF((float)(point.X - xMin), (float)(point.Y - yMin)), i / 2);
            }
            PointF end = (PointF)Points.GetValue(0);
            Points.SetValue(end, tempArray.Count / 2);
        }

        /// <summary>
        /// Estimate the current point is left_bottom or right_up.
        /// </summary>
        /// <param name="point">current point</param>
        /// <param name="xMin">left</param>
        /// <param name="xMax">right</param>
        /// <param name="yMin">bottom</param>
        /// <param name="yMax">up</param>
        static private void FindMinMax(Autodesk.Revit.DB.XYZ point, ref double xMin, ref double xMax, ref double yMin, ref double yMax)
        {
            if (point.X < xMin)
            {
                xMin = point.X;
            }
            if (point.X > xMax)
            {
                xMax = point.X;
            }
            if (point.Y < yMin)
            {
                yMin = point.Y;
            }
            if (point.Y > yMax)
            {
                yMax = point.Y;
            }
        }

        /// <summary>
        /// Filter none-wall elements.
        /// </summary>
        /// <param name="miscellanea">The currently selected Elements in Autodesk Revit</param>
        /// <returns></returns>
        static private ElementSet WallFilter(ElementSet miscellanea)
        {
            ElementSet walls = new ElementSet();
            foreach (Autodesk.Revit.DB.Element e in miscellanea)
            {
                Wall w = e as Wall;
                if (null != w)
                {
                    walls.Insert(w);
                }
            }

            if (0 == walls.Size)
            {
                throw new InvalidOperationException("Please select wall first.");
            }

            return walls;
        }

        /// <summary>
        /// Chaining the profile.
        /// </summary>
        /// <param name="lines">none-chained profile</param>
        private void SortCurves(CurveArray lines)
        {
            Autodesk.Revit.DB.XYZ temp = lines.get_Item(0).get_EndPoint(1);
            Curve temCurve = lines.get_Item(0);

            Profile.Append(temCurve);

            while (Profile.Size != lines.Size)
            {

                temCurve = GetNext(lines, temp, temCurve);

                if (Math.Abs(temp.X - temCurve.get_EndPoint(0).X) < PRECISION
                    && Math.Abs(temp.Y - temCurve.get_EndPoint(0).Y) < PRECISION)
                {
                    temp = temCurve.get_EndPoint(1);
                }
                else
                {
                    temp = temCurve.get_EndPoint(0);
                }

                Profile.Append(temCurve);
            }

            if (Math.Abs(temp.X - lines.get_Item(0).get_EndPoint(0).X) > PRECISION
                || Math.Abs(temp.Y - lines.get_Item(0).get_EndPoint(0).Y) > PRECISION
                || Math.Abs(temp.Z - lines.get_Item(0).get_EndPoint(0).Z) > PRECISION)
            {
                throw new InvalidOperationException("The selected walls should be closed.");
            }
        }

        /// <summary>
        /// Get the connected curve for current curve
        /// </summary>
        /// <param name="profile">a closed outline made by the selection of walls</param>
        /// <param name="connected">current curve's end point</param>
        /// <param name="line">current curve</param>
        /// <returns>a appropriate curve for generate floor</returns>
        private Curve GetNext(CurveArray profile, Autodesk.Revit.DB.XYZ connected, Curve line)
        {
            foreach (Curve c in profile)
            {
                if (c.Equals(line))
                {
                    continue;
                }
                if ((Math.Abs(c.get_EndPoint(0).X - line.get_EndPoint(1).X) < PRECISION && Math.Abs(c.get_EndPoint(0).Y - line.get_EndPoint(1).Y) < PRECISION && Math.Abs(c.get_EndPoint(0).Z - line.get_EndPoint(1).Z) < PRECISION)
                    && (Math.Abs(c.get_EndPoint(1).X - line.get_EndPoint(0).X) < PRECISION && Math.Abs(c.get_EndPoint(1).Y - line.get_EndPoint(0).Y) < PRECISION && Math.Abs(c.get_EndPoint(1).Z - line.get_EndPoint(0).Z) < PRECISION)
                    && 2 != profile.Size)
                {
                    continue;
                }

                if (Math.Abs(c.get_EndPoint(0).X - connected.X) < PRECISION && Math.Abs(c.get_EndPoint(0).Y - connected.Y) < PRECISION && Math.Abs(c.get_EndPoint(0).Z - connected.Z) < PRECISION)
                {
                    return c;
                }
                else if (Math.Abs(c.get_EndPoint(1).X - connected.X) < PRECISION && Math.Abs(c.get_EndPoint(1).Y - connected.Y) < PRECISION && Math.Abs(c.get_EndPoint(1).Z - connected.Z) < PRECISION)
                {
                    if (c.GetType().Name.Equals("Line"))
                    {
                        Autodesk.Revit.DB.XYZ start = c.get_EndPoint(1);
                        Autodesk.Revit.DB.XYZ end = c.get_EndPoint(0);
                        return m_creApp.NewLineBound(start, end);
                    }
                    else if (c.GetType().Name.Equals("Arc"))
                    {
                        int size = c.Tessellate().Count;
                        Autodesk.Revit.DB.XYZ start = c.Tessellate()[0];
                        Autodesk.Revit.DB.XYZ middle = c.Tessellate()[size / 2];
                        Autodesk.Revit.DB.XYZ end = c.Tessellate()[size];

                        return m_creApp.NewArc(start, end, middle);
                    }
                }
            }

            throw new InvalidOperationException("The selected walls should be closed.");
        }
    }
}
