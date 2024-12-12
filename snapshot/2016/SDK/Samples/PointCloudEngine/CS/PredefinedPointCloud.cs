//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.IO;
using System.Runtime.Serialization;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;

namespace Revit.SDK.Samples.CS.PointCloudEngine
{

    /// <summary>
    /// An implementation for a non file-based point cloud.  In this implementaiton, the location of the cells, including their colors and options, 
    /// are hardcoded.
    /// </summary>
    public class PredefinedPointCloud : PointCloudAccessBase, IPointCloudAccess
    {
        #region  Class Member Variables
        String m_identifier;
        #endregion

        #region Class Methods
        /// <summary>
        /// Constructs a new predefined point cloud access instance.
        /// </summary>
        /// <param name="identifier">The identifier of the point cloud.</param>
        public PredefinedPointCloud(String identifier)
        {
            m_identifier = identifier;

            Setup(false);
        }

        /// <summary>
        /// Constructs a new predefined point cloud access instance.
        /// </summary>
        /// <param name="identifier">The identifier of the point cloud.</param>
        /// <param name="randomizedPoints">True to use randomization for the point location and number, false otherwise.</param>
        public PredefinedPointCloud(String identifier, bool randomizedPoints)
        {
            m_identifier = identifier;

            Setup(randomizedPoints);
        }

        /// <summary>
        /// Sets up the predefined point cloud.
        /// </summary>
        /// <param name="randomizedPoints">True to use randomization for the point location and number, false otherwise.</param>
        private void Setup(bool randomizedPoints)
        {
            AddCell(new XYZ(0, 0, 0), new XYZ(0.5, 100, 10), 0x00CCCC, randomizedPoints);
            AddCell(new XYZ(0, 0, 0), new XYZ(50, 0.5, 10), 0x00CCCC, randomizedPoints);
            AddCell(new XYZ(49.5, 0, 0), new XYZ(50, 100, 10), 0x00CCCC, randomizedPoints);
            AddCell(new XYZ(0, 99.5, 0), new XYZ(50, 100, 10), 0x00CCCC, randomizedPoints);
            AddCell(new XYZ(10, 0, 0), new XYZ(14, 0.5, 7), 0xCC99CC, randomizedPoints);
            AddCell(new XYZ(30, 0, 3), new XYZ(33, 0.5, 8), 0xA0A0A0, randomizedPoints);
            AddCell(new XYZ(33, 0, 3), new XYZ(36, 0.5, 8), 0xA0A0A0, randomizedPoints);
            AddCell(new XYZ(0, 24, 3), new XYZ(0.5, 27, 8), 0xA0A0A0, randomizedPoints);
            AddCell(new XYZ(0, 27, 3), new XYZ(0.5, 30, 8), 0xA0A0A0, randomizedPoints);
            AddCell(new XYZ(0, 46, 0), new XYZ(0.5, 50, 7), 0xCC99CC, randomizedPoints);
            AddCell(new XYZ(0, 50, 0), new XYZ(0.5, 54, 7), 0xCC99CC, randomizedPoints);
            AddCell(new XYZ(0, 70, 3), new XYZ(0.5, 73, 8), 0xA0A0A0, randomizedPoints);
            AddCell(new XYZ(0, 73, 3), new XYZ(0.5, 76, 8), 0xA0A0A0, randomizedPoints);
        }
        #endregion
        
        #region IPointCloudAccess Members

        /// <summary>
        /// The implementation of IPointCloudAccess.GetName().
        /// </summary>
        /// <returns>The name (the file name).</returns>
        public String GetName()
        {
            return "apipc: " + m_identifier;
        }

        /// <summary>
        /// The implementation of IPointCloudAccess.GetColorEncoding()
        /// </summary>
        /// <returns>The color encoding.</returns>
        public PointCloudColorEncoding GetColorEncoding()
        {
            return PointCloudColorEncoding.ABGR;
        }

        /// <summary>
        /// The implementation of IPointCloudAccess.CreatePointSetIterator().
        /// </summary>
        /// <param name="rFilter">The filter.</param>
        /// <param name="viewId">The view id (unused).</param>
        /// <returns>The new iterator.</returns>
        public IPointSetIterator CreatePointSetIterator(PointCloudFilter rFilter, ElementId viewId)
        {
            return new PointCloudAccessBase.PointCloudAccessBaseIterator(this, rFilter);
        }

        /// <summary>
        /// The implementation of IPointCloudAccess.CreatePointSetIterator().
        /// </summary>
        /// <param name="rFilter">The filter.</param>
        /// <param name="density">The density.</param>
        /// <param name="viewId">The view id (unused).</param>
        /// <returns>The new iterator.</returns>
        public IPointSetIterator CreatePointSetIterator(PointCloudFilter rFilter, double density, ElementId viewId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The implementation of IPointCloudAccess.GetExtent().
        /// </summary>
        /// <returns>The extents of the point cloud.</returns>
        public Outline GetExtent()
        {
            return GetOutline();
        }

        /// <summary>
        /// The implementation of IPointCloudAccess.GetOffset().
        /// </summary>
        /// <remarks>This method is not used by Revit and will be removed in a later pre-release build.</remarks>
        /// <returns>Zero.</returns>
        public XYZ GetOffset()
        {
            return XYZ.Zero;
        }

        /// <summary>
        /// The implementation of IPointCloudAccess.GetUnitsToFeetConversionFactor().
        /// </summary>
        /// <returns>The scale.</returns>
        public double GetUnitsToFeetConversionFactor()
        {
            return GetScale();
        }

        /// <summary>
        /// The implementation of IPointCloudAccess.ReadPoints().
        /// </summary>
        /// <param name="rFilter">The filter.</param>
        /// <param name="viewId">The view id (unused).</param>
        /// <param name="buffer">The point cloud buffer.</param>
        /// <param name="nBufferSize">The maximum number of points.</param>
        /// <returns>The number of points read.</returns>
        public int ReadPoints(PointCloudFilter rFilter, ElementId viewId, IntPtr buffer, int nBufferSize)
        {
            int read = ReadSomePoints(rFilter, buffer, nBufferSize, 0);

            return read;
        }

        /// <summary>
        /// The implementation of IPointCloudAccess.Free().
        /// </summary>
        public void Free()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}