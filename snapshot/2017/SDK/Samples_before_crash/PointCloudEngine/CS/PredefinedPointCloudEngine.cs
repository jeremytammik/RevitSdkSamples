//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;

namespace Revit.SDK.Samples.CS.PointCloudEngine
{
    /// <summary>
    /// The type of engine.
    /// </summary>
    /// <remarks>Because the same engine implementation is used for all types of engines in this sample, a member of this enumerated type
    /// is used to determine the logic necessary to create the IPointCloudAccess instance.</remarks>
    public enum PointCloudEngineType
    {
        /// <summary>
        /// A predefined point cloud engine (non-randomized).
        /// </summary>
        Predefined,
        /// <summary>
        /// A predefined point cloud engine (randomized).
        /// </summary>
        RandomizedPoints,
        /// <summary>
        /// A file based point cloud engine.
        /// </summary>
        FileBased
    }

    /// <summary>
    /// An implementation of IPointCloudEngine used by all the custom engines in this sample.
    /// </summary>
    public class BasicPointCloudEngine : IPointCloudEngine
    {
        #region  Class Member Variables
        private PointCloudEngineType m_type;
        #endregion

        #region Class Methods
        /// <summary>
        /// Constructs a new instance of the engine.
        /// </summary>
        /// <param name="type">The type of point cloud served by this engine instance.</param>
        public BasicPointCloudEngine(PointCloudEngineType type)
        {
            m_type = type;
        }
        #endregion

        #region IPointCloudEngine Members

        /// <summary>
        /// Implementation of IPointCloudEngine.CreatePointCloudAccess().
        /// </summary>
        /// <param name="identifier">The identifier (or file name) for the desired point cloud.</param>
        /// <returns>The IPointCloudAccess implementation serving this point cloud.</returns>
        public IPointCloudAccess CreatePointCloudAccess(string identifier)
        {
            switch (m_type)
            {
                case PointCloudEngineType.RandomizedPoints:
                    return new PredefinedPointCloud(identifier, true);
                case PointCloudEngineType.FileBased:
                    return new FileBasedPointCloud(identifier);
                case PointCloudEngineType.Predefined:
                default:
                    return new PredefinedPointCloud(identifier);
            }
        }

        /// <summary>
        /// Implementation of IPointCloudEngine.Free().
        /// </summary>
        public void Free()
        {
            //Nothing to do
        }

        #endregion
    }
}