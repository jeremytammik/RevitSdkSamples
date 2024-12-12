//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
using System.Xml.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.CS.PointCloudEngine
{
    /// <summary>
    /// This class is used to calculate and store points for a given cell.
    /// </summary>
    public class PointCloudCellStorage
    {
        #region  Class Member Variables
        [Flags]
        private enum PointDirections
        {
            PlusX = 1,
            MinusX = 2,
            PlusY = 4,
            MinusY = 8,
            PlusZ = 16,
            MinusZ = 32
        }

        private CloudPoint[] m_pointsBuffer;
        private int m_numberOfPoints;
        private XYZ m_lowerLeft;
        private XYZ m_upperRight;
        private int m_color;
        private bool m_randomize;
        private const int s_maxNumberOfPoints = 1000000;
        private const float s_delta = 0.1f;
        private Random m_random = new Random();
        #endregion

        #region Class Property
        /// <summary>
        /// The number of points in the cell.
        /// </summary>
        public int NumberOfPoints
        {
            get { return m_numberOfPoints; }
        }

        /// <summary>
        /// The lower left point of the cell.
        /// </summary>
        public XYZ LowerLeft
        {
            get { return m_lowerLeft; }
        }

        /// <summary>
        /// The upper right point of the cell.
        /// </summary>
        public XYZ UpperRight
        {
            get { return m_upperRight; }
        }

        /// <summary>
        /// The points in the cell.
        /// </summary>
        public CloudPoint[] PointsBuffer
        {
            get { return m_pointsBuffer; }
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Creates a new instance of a rectangular cell.
        /// </summary>
        /// <param name="lowerLeft">The lower left point of the cell.</param>
        /// <param name="upperRight">The upper right point of the cell.</param>
        /// <param name="color">The color used for points in the cell.</param>
        /// <param name="randomize">True to apply randomization to the number and location of points, false for a regular arrangement of points.</param>
        public PointCloudCellStorage(XYZ lowerLeft, XYZ upperRight, int color, bool randomize)
        {
            m_lowerLeft = lowerLeft;
            m_upperRight = upperRight;
            m_color = color;
            m_randomize = randomize;

            m_pointsBuffer = new CloudPoint[s_maxNumberOfPoints];
            m_numberOfPoints = 0;
        }

        /// <summary>
        /// Invokes the calculation for all points in the cell.
        /// </summary>
        public void GeneratePoints()
        {
            // X direction lines
            float xDistance = (float)(m_upperRight.X - m_lowerLeft.X);
            AddLine(m_lowerLeft, XYZ.BasisX, PointDirections.PlusY | PointDirections.PlusZ, xDistance);
            AddLine(new XYZ(m_lowerLeft.X, m_lowerLeft.Y, m_upperRight.Z), XYZ.BasisX, PointDirections.PlusY | PointDirections.MinusZ, xDistance);
            AddLine(new XYZ(m_lowerLeft.X, m_upperRight.Y, m_lowerLeft.Z), XYZ.BasisX, PointDirections.MinusY | PointDirections.PlusZ, xDistance);
            AddLine(new XYZ(m_lowerLeft.X, m_upperRight.Y, m_upperRight.Z), XYZ.BasisX, PointDirections.MinusY | PointDirections.MinusZ, xDistance);

            // Y direction lines
            float yDistance = (float)(m_upperRight.Y - m_lowerLeft.Y);
            AddLine(m_lowerLeft, XYZ.BasisY, PointDirections.PlusX | PointDirections.PlusZ, yDistance);
            AddLine(new XYZ(m_lowerLeft.X, m_lowerLeft.Y, m_upperRight.Z), XYZ.BasisY, PointDirections.PlusX | PointDirections.MinusZ, yDistance);
            AddLine(new XYZ(m_upperRight.X, m_lowerLeft.Y, m_lowerLeft.Z), XYZ.BasisY, PointDirections.MinusX | PointDirections.PlusZ, yDistance);
            AddLine(new XYZ(m_upperRight.X, m_lowerLeft.Y, m_upperRight.Z), XYZ.BasisY, PointDirections.MinusX | PointDirections.MinusZ, yDistance);

            // Z direction lines
            float zDistance = (float)(m_upperRight.Z - m_lowerLeft.Z);
            AddLine(m_lowerLeft, XYZ.BasisZ, PointDirections.PlusX | PointDirections.PlusY, zDistance);
            AddLine(new XYZ(m_lowerLeft.X, m_upperRight.Y, m_lowerLeft.Z), XYZ.BasisZ, PointDirections.PlusX | PointDirections.MinusY, zDistance);
            AddLine(new XYZ(m_upperRight.X, m_lowerLeft.Y, m_lowerLeft.Z), XYZ.BasisZ, PointDirections.MinusX | PointDirections.PlusY, zDistance);
            AddLine(new XYZ(m_upperRight.X, m_upperRight.Y, m_lowerLeft.Z), XYZ.BasisZ, PointDirections.MinusX | PointDirections.MinusY, zDistance);
        }

        private int AddLine(XYZ startPoint, XYZ direction, PointDirections directions, float distance)
        {
            float deltaX = 0.0f;
            int totalRead = 0;

            while (deltaX < distance)
            {
                AddPoints(startPoint + direction * deltaX, directions);
                deltaX += s_delta;
            }

            return totalRead;
        }

        private void AddModifiedPoint(XYZ point, XYZ modification, double transverseDelta, int pointNumber)
        {
            XYZ cloudPointXYZ = point + modification * Math.Pow(transverseDelta * pointNumber, 4.0);
            CloudPoint cp = new CloudPoint((float)cloudPointXYZ.X, (float)cloudPointXYZ.Y, (float)cloudPointXYZ.Z, m_color);
            m_pointsBuffer[m_numberOfPoints] = cp;
            m_numberOfPoints++;
            if (m_numberOfPoints == s_maxNumberOfPoints)
            {
                TaskDialog.Show("Point  cloud engine", "A single cell is requiring more than the maximum hardcoded number of points for one cell: " + s_maxNumberOfPoints);
                throw new Exception("Reached maximum number of points.");
            }
        }

        private void AddPoints(XYZ point, PointDirections directions)
        {
            // Two random items: number of points, and delta
            int numberOfPoints = 5;
            double transverseDelta = 0.1;
            if (m_randomize)
            {
                numberOfPoints = 5 + m_random.Next(10);
                transverseDelta = m_random.NextDouble() * 0.1;
            }

            for (int i = 1; i < numberOfPoints; i++)
            {
                if ((directions & PointDirections.PlusX) == PointDirections.PlusX)
                {
                    AddModifiedPoint(point, XYZ.BasisX, transverseDelta, i);                  
                }

                if ((directions & PointDirections.MinusX) == PointDirections.MinusX)
                {
                    AddModifiedPoint(point, -XYZ.BasisX, transverseDelta, i); 
                }

                if ((directions & PointDirections.PlusY) == PointDirections.PlusY)
                {
                    AddModifiedPoint(point, XYZ.BasisY, transverseDelta, i); 
                }

                if ((directions & PointDirections.MinusY) == PointDirections.MinusY)
                {
                    AddModifiedPoint(point, -XYZ.BasisY, transverseDelta, i); 
                }

                if ((directions & PointDirections.PlusZ) == PointDirections.PlusZ)
                {
                    AddModifiedPoint(point, XYZ.BasisZ, transverseDelta, i); 
                }

                if ((directions & PointDirections.MinusZ) == PointDirections.MinusZ)
                {
                    AddModifiedPoint(point, -XYZ.BasisZ, transverseDelta, i); 
                }
            }
        }

        /// <summary>
        /// Serializes the properties of the cell to an XML element.
        /// </summary>
        /// <param name="rootElement">The element to which the properties are added as subelements.</param>
        public void SerializeObjectData(XElement rootElement)
        {
            rootElement.Add(XmlUtils.GetXElement(m_lowerLeft, "LowerLeft"));
            rootElement.Add(XmlUtils.GetXElement(m_upperRight, "UpperRight"));
            rootElement.Add(XmlUtils.GetColorXElement(m_color, "Color"));
            rootElement.Add(XmlUtils.GetXElement(m_randomize, "Randomize"));
        }

        /// <summary>
        /// Constructs a new instance of a rectangular cell from an XML element.
        /// </summary>
        /// <param name="rootElement">The XML element representing the cell.</param>
        public PointCloudCellStorage(XElement rootElement)
        {
            m_lowerLeft = XmlUtils.GetXYZ(rootElement.Element("LowerLeft"));
            m_upperRight = XmlUtils.GetXYZ(rootElement.Element("UpperRight"));
            m_color = XmlUtils.GetColor(rootElement.Element("Color"));
            m_randomize = XmlUtils.GetBoolean(rootElement.Element("Randomize"));

            m_pointsBuffer = new CloudPoint[s_maxNumberOfPoints];
            m_numberOfPoints = 0;
        }
        #endregion
    }
}