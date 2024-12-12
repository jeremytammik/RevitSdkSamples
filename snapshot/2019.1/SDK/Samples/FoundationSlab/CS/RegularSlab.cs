//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
//

using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.FoundationSlab.CS
{
    /// <summary>
    /// A class of regular slab.
    /// </summary>
    public class RegularSlab
    {
        bool m_selected = true; // Be selected or not.
        string m_mark;   // The mark of the slab.
        Level m_level;   // The level of the slab.
        ElementType m_type; // The type of the slab.

        Autodesk.Revit.DB.ElementId m_id; // The id of the slab.
        CurveArray m_profile;  // The profile of the slab.
        CurveArray m_octagonalProfile; // The octagonal profile of the slab.
        BoundingBoxXYZ m_maxBBox; // The max bounding box of the slab.

        /// <summary>
        /// Selected property.
        /// </summary>
        public bool Selected
        {
            get { return m_selected; }
            set { m_selected = value; }
        }

        /// <summary>
        /// Mark property.
        /// </summary>
        public string Mark
        {
            get { return m_mark; }
        }

        /// <summary>
        /// LevelName property.
        /// </summary>
        public string LevelName
        {
            get { return m_level.Name; }
        }

        /// <summary>
        /// SlabTypeName property.
        /// </summary>
        public string SlabTypeName
        {
            get { return m_type.Name; }
        }

        /// <summary>
        /// Id property.
        /// </summary>
        public Autodesk.Revit.DB.ElementId Id
        {
            get { return m_id; }
        }

        /// <summary>
        /// Profile property.
        /// </summary>
        public CurveArray Profile
        {
            get { return m_profile; }
        }

        /// <summary>
        /// OctagonalProfile property.
        /// </summary>
        public CurveArray OctagonalProfile
        {
            get { return m_octagonalProfile; }
        }

        /// <summary>
        /// BBox property.
        /// </summary>
        public BoundingBoxXYZ BBox
        {
            get { return m_maxBBox; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="floor">The floor object.</param>
        /// <param name="floorProfile">The floor's profile.</param>
        /// <param name="bBox">The floor's bounding box.</param>
        public RegularSlab(Floor floor, CurveArray floorProfile, BoundingBoxXYZ bBox)
        {
            // Get floor's properties.
            if (null != floor)
            {
                // Get floor's Mark property.
                Parameter markPara = floor.get_Parameter(BuiltInParameter.ALL_MODEL_MARK);
                if (null != markPara)
                {
                    m_mark = markPara.AsString();
                }

                m_level = floor.Document.GetElement(floor.LevelId) as Level;   // Get floor's level.
                m_type = floor.Document.GetElement(floor.GetTypeId()) as ElementType;// Get floor's type.
                m_id = floor.Id; // Get floor's Id.
                m_profile = floorProfile;  // Get floor's profile.
            }

            // Create an octagonal profile for the floor according to it's bounding box.
            if (null != bBox)
            {
                CreateOctagonProfile(bBox.Min, bBox.Max);
            }
        }

        /// <summary>
        /// Create an octagonal profile.
        /// </summary>
        /// <param name="min">The min point of the floor's bounding box.</param>
        /// <param name="max">The max point of the floor's bounding box.</param>
        /// <returns>The bool value suggests successful or not.</returns>
        private bool CreateOctagonProfile(Autodesk.Revit.DB.XYZ min, Autodesk.Revit.DB.XYZ max)
        {
            // Calculate the x/y offset.
            double xOffset = Math.Abs(max.Y - min.Y) / 8;
            double yOffset = Math.Abs(max.X - min.X) / 8;
            double z = max.Z;

            // Calculate the eight points of the octagon.
            Autodesk.Revit.DB.XYZ[] points = new Autodesk.Revit.DB.XYZ[8];
            points[0] = new Autodesk.Revit.DB.XYZ(min.X, min.Y, z);
            points[1] = new Autodesk.Revit.DB.XYZ((min.X + max.X) / 2, (min.Y - yOffset), z);
            points[2] = new Autodesk.Revit.DB.XYZ(max.X, min.Y, z);
            points[3] = new Autodesk.Revit.DB.XYZ((max.X + xOffset), (min.Y + max.Y) / 2, z);
            points[4] = new Autodesk.Revit.DB.XYZ(max.X, max.Y, z);
            points[5] = new Autodesk.Revit.DB.XYZ((min.X + max.X) / 2, (max.Y + yOffset), z);
            points[6] = new Autodesk.Revit.DB.XYZ(min.X, max.Y, z);
            points[7] = new Autodesk.Revit.DB.XYZ((min.X - xOffset), (min.Y + max.Y) / 2, z);

            // Get the octagonal profile.
            m_octagonalProfile = new CurveArray();
            for (int i = 0; i < 8; i++)
            {
                Line line;
                if (7 == i)
                    line = Line.CreateBound(points[i], points[0]);
                else
                    line = Line.CreateBound(points[i], points[i + 1]);
                m_octagonalProfile.Append(line);
            }

            // Get the octagonal profile's bounding box.
            Autodesk.Revit.DB.XYZ newMin = new Autodesk.Revit.DB.XYZ(min.X - xOffset, min.Y - yOffset, z);
            Autodesk.Revit.DB.XYZ newMax = new Autodesk.Revit.DB.XYZ(max.X + xOffset, max.Y + yOffset, z);
            m_maxBBox = new BoundingBoxXYZ();
            m_maxBBox.Min = newMin;
            m_maxBBox.Max = newMax;

            return true;
        }

    }
}
