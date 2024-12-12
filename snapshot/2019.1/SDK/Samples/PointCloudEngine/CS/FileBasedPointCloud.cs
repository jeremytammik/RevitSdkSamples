//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.IO;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;

namespace Revit.SDK.Samples.CS.PointCloudEngine
{
    /// <summary>
    /// An implementation for a file-based point cloud.
    /// </summary>
    /// <example>
    /// The file format is based upon XML.  A sample XML looks like:
    /// <code>
    ///    <PointCloud>
    ///      <Scale value="2.5"/>
    ///      <Cell>
    ///        <LowerLeft X="-30" Y="-30" Z="0" />
    ///        <UpperRight X="30" Y="30" Z="200" />
    ///        <Color value="#000000" />
    ///        <Randomize value="True" />
    ///      </Cell>
    ///      <Cell>
    ///         <LowerLeft X="-30" Y="-10" Z="10" />
    ///         <UpperRight X="-29" Y="10" Z="150" />
    ///         <Color value="#CC3300" />
    ///         <Randomize value="False" />
    ///       </Cell>
    ///    </PointCloud>
    /// </code>
    /// The scale value applies to the entire point cloud.  One or more cell values should be supplied,
    /// with the coordinates of the opposing corners, a color, and an option whether or not to randomize 
    /// the generated points.
    /// </example>
    class FileBasedPointCloud : PointCloudAccessBase, IPointCloudAccess
    {
        #region  Class Member Variables
        String m_fileName;
        #endregion

        #region Class Methods
        /// <summary>
        /// Constructs a new XML-based point cloud access.
        /// </summary>
        /// <param name="fileName">The full path to the file.</param>
        public FileBasedPointCloud(String fileName)
        {
            m_fileName = fileName;

            Setup();
        }

        /// <summary>
        /// Sets up the file-based point cloud.
        /// </summary>
        private void Setup()
        {
            if (File.Exists(m_fileName))
            {
                StreamReader reader = new StreamReader(m_fileName);
                XDocument xmlDoc = XDocument.Load(new XmlTextReader(reader));
                reader.Close();

                SetupFrom(xmlDoc.Element("PointCloud"));
            }
        }
        #endregion

        #region IPointCloudAccess Members

        /// <summary>
        /// The implementation of IPointCloudAccess.GetName().
        /// </summary>
        /// <returns>The name (the file name).</returns>
        public String GetName()
        {
            return m_fileName;
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
