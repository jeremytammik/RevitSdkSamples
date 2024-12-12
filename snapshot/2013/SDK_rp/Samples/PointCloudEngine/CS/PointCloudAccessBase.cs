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
using System.Xml.Linq;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;
using Autodesk.Revit.UI;



namespace Revit.SDK.Samples.CS.PointCloudEngine
{
    /// <summary>
    ///  The base class for all IPointCloudAccess implementations in this sample.
    /// </summary>
    public class PointCloudAccessBase
    {
        #region  Class Member Variables
        private double m_scale = 1.0;
        private List<PointCloudCellStorage> m_storedCells;
        private Outline m_outline = null;
        #endregion

        #region Class Methods
        /// <summary>
        /// Constructs a new instance of the base class.
        /// </summary>
        protected PointCloudAccessBase()
        {
            m_storedCells = new List<PointCloudCellStorage>();
        }

        /// <summary>
        /// Adds a new cell to the point cloud.
        /// </summary>
        /// <param name="lowerLeft">The lower left point.</param>
        /// <param name="upperRight">The upper right point.</param>
        /// <param name="color">The color.</param>
        /// <param name="randomize">True to randomize point number and location, false for a regular arrangement of points.</param>
        protected void AddCell(XYZ lowerLeft, XYZ upperRight, int color, bool randomize)
        {
            PointCloudCellStorage storage = new PointCloudCellStorage(lowerLeft, upperRight, color, randomize);
            storage.GeneratePoints();
            m_storedCells.Add(storage);

            AddCellToOutline(storage);
        }

        /// <summary>
        /// Adds a new cell to the point cloud.
        /// </summary>
        /// <param name="lowerLeft">The lower left point.</param>
        /// <param name="upperRight">The upper right point.</param>
        /// <param name="color">The color.</param>
        protected void AddCell(XYZ lowerLeft, XYZ upperRight, int color)
        {
            AddCell(lowerLeft, upperRight, color, false);
        }

        /// <summary>
        /// Adds a cell to the stored outline of the point cloud.  If the cell boundaries extend beyond the current outline, the outline 
        /// is adjusted.
        /// </summary>
        /// <param name="storage"></param>
        private void AddCellToOutline(PointCloudCellStorage storage)
        {
            XYZ lowerLeft = storage.LowerLeft;
            XYZ upperRight = storage.UpperRight;
            if (m_outline == null)
                m_outline = new Outline(lowerLeft, upperRight);
            else
            {
                XYZ minimumPoint = m_outline.MinimumPoint;

                m_outline.MinimumPoint = new XYZ(Math.Min(minimumPoint.X, lowerLeft.X),
                                                Math.Min(minimumPoint.Y, lowerLeft.Y),
                                                Math.Min(minimumPoint.Z, lowerLeft.Z));

                XYZ maximumPoint = m_outline.MaximumPoint;
                m_outline.MaximumPoint = new XYZ(Math.Max(maximumPoint.X, upperRight.X),
                                                Math.Max(maximumPoint.Y, upperRight.Y),
                                                Math.Max(maximumPoint.Z, upperRight.Z));
            }
        }

        /// <summary>
        /// Gets the outline calculated from all cells in the point cloud.
        /// </summary>
        /// <returns></returns>
        protected Outline GetOutline()
        {
            return m_outline;
        }

        /// <summary>
        /// Gets the scale stored for this point cloud.
        /// </summary>
        /// <returns></returns>
        protected double GetScale()
        {
            return m_scale;
        }

        /// <summary>
        /// Saves the contents of the point cloud into an XML element.
        /// </summary>
        /// <param name="rootElement">The XML element in which to save the point cloud properties.</param>
        public virtual void SerializeObjectData(XElement rootElement)
        {
            XElement scaleElement = XmlUtils.GetXElement(m_scale, "Scale");
            rootElement.Add(scaleElement);
            
            int count = m_storedCells.Count;
            for (int i = 0; i < count; i++)
            {
                XElement cellElement = new XElement("Cell");
                m_storedCells[i].SerializeObjectData(cellElement);
                rootElement.Add(cellElement);
            }
        }

        /// <summary>
        /// The internal implementation for point cloud read requests from Revit.  
        /// </summary>
        /// <remarks>Both IPointCloudAccess.ReadPoints() and IPointSetIterator.ReadPoints() are served by this method.</remarks>
        /// <param name="rFilter">The point cloud filter.</param>
        /// <param name="buffer">The point cloud buffer.</param>
        /// <param name="nBufferSize">The maximum number of points in the buffer.</param>
        /// <param name="startIndex">The start index for points.  Pass 0 if called from IPointCloudAccess.ReadPoints() or if this is the first 
        /// call to IPointSetIterator.ReadPoints().  Pass the previous cumulative number of read points for second and successive calls to 
        /// IPointSetIterator.ReadPoints().</param>
        /// <returns>The number of points read.</returns>
        protected unsafe int ReadSomePoints(PointCloudFilter rFilter, IntPtr buffer, int nBufferSize, int startIndex)
        {
            // Get the pointer to the buffer.
            CloudPoint* cpBuffer = (CloudPoint*)buffer.ToPointer();
            int pointIndex = 0;
            int currentIndex = startIndex;
            int totalPoints = 0;
            int startCell = 0;
            Outline fullOutline = GetOutline();

            // Test each cell until the first cell with needed points is found.
            for (int i = 0; i < m_storedCells.Count; i++)
            {
                PointCloudCellStorage cell = m_storedCells[i];

                // Pass the cell outline to the filter.
                int filterResult = rFilter.TestCell(cell.LowerLeft, cell.UpperRight);

                // Filter result == -1 means the cell is completely out of scope for the filter.
                if (filterResult == -1)
                    continue;

                // The cell is at least partially in scope.  If the cell has more points than
                // the number read in previous calls, we should start with this cell.
                // If it has less points than the number read, the cell was already processed and we
                // should move to the next one.
                totalPoints += cell.NumberOfPoints;
                if (currentIndex < totalPoints)
                {
                    startCell = i;
                    currentIndex = Math.Max(0, startIndex - totalPoints);
                    break;
                }
            }

            // Start with the current candidate cell and read cells until there are no more to read.
            for (int i = startCell; i < m_storedCells.Count; i++)
            {
                // Test the cell against the filter.
                PointCloudCellStorage cell = m_storedCells[i];
                int filterResult = rFilter.TestCell(cell.LowerLeft, cell.UpperRight);

                // Filter result == -1 means the cell is entirely out of scope, skip it.
                if (filterResult == -1)
                    continue;

                // Filter result == 0 means some part of the cell is in scope.
                // Prepare for cell is called to process the cell's points.
                if (filterResult == 0)
                    rFilter.PrepareForCell(fullOutline.MinimumPoint, fullOutline.MaximumPoint, cell.NumberOfPoints);

                // Loop through all points in the cell.
                for (int j = currentIndex; j < cell.NumberOfPoints; j++)
                {
                    // If we need to test the point for acceptance, use the filter to do so.
                    // If filter result == 1 the entire cell was in scope, no need to test.
                    if (filterResult == 0)
                    {
                        if (!rFilter.TestPoint(cell.PointsBuffer[j]))
                            continue;
                    }

                    // Add the point to the buffer and increment the counter
                    *(cpBuffer + pointIndex) = cell.PointsBuffer[j];
                    pointIndex++;
                    
                    // Stop when the max number of points is reached
                    if (pointIndex >= nBufferSize)
                    {
                        break;
                    }
                }

                // Stop when the max number of points is reached
                if (pointIndex >= nBufferSize)
                {
                    break;
                }
                currentIndex = 0;
            }

            return pointIndex;
        }

        /// <summary>
        /// Sets up a point cloud from an XML root element.
        /// </summary>
        /// <param name="rootElement">The root element.</param>
        protected void SetupFrom(XElement rootElement)
        {  
            // Read scale, if it exists.
            foreach (XElement scaleElement in rootElement.Elements("Scale"))
            {
                double scale = XmlUtils.GetDouble(scaleElement);
                if (scale < 0.0)
                {
                    TaskDialog.Show("Scale error", "The value of scale is not a valid number greater than zero.");
                }
                else
                {
                    m_scale = scale;
                }
            }

            // Read cells.
            m_storedCells = new List<PointCloudCellStorage>();
            foreach (XElement cellElement in rootElement.Elements("Cell"))
            {
                PointCloudCellStorage cell = new PointCloudCellStorage(cellElement);
                m_storedCells.Add(cell);
                AddCellToOutline(cell);
                cell.GeneratePoints();
            }
        }
        #endregion

        /// <summary>
        /// The implementation for an IPointSetIterator for a file-based or predefined point cloud.
        /// </summary>
        protected class PointCloudAccessBaseIterator : IPointSetIterator
        {
            #region  Class Member Variables
            private PointCloudFilter m_filter;
            private int m_currentIndex;
            private PointCloudAccessBase m_access;
            private bool m_done = false;
            #endregion

            #region Class Methods
            /// <summary>
            /// Constructs a new instance of the point cloud iterator.
            /// </summary>
            /// <param name="access">The access.</param>
            /// <param name="filter">The filter used for this iteration.</param>
            public PointCloudAccessBaseIterator(PointCloudAccessBase access, PointCloudFilter filter)
            {
                m_access = access;
                m_filter = filter;
                m_currentIndex = 0;
            }
            #endregion

            #region IPointSetIterator Members

            /// <summary>
            /// Implementation of IPointSetIterator.ReadPoints()
            /// </summary>
            /// <param name="buffer">The point buffer.</param>
            /// <param name="nBufferSize">The buffer size.</param>
            /// <returns>The number of points read.</returns>
            public int ReadPoints(IntPtr buffer, int nBufferSize)
            {
                if (m_done)
                {
                    return 0;
                }

                int found = m_access.ReadSomePoints(m_filter, buffer, nBufferSize, m_currentIndex);
                m_currentIndex += found;

                if (found > nBufferSize)
                {
                    m_done = true;
                }

                return found;
            }

            /// <summary>
            /// Implementation of IPointSetIterator.Free()
            /// </summary>
            public void Free()
            {
                m_done = true;
            }

            #endregion
        }
    }
}