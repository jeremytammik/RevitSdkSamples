//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;

namespace Revit.SDK.Samples.NewRoof.RoofsManager.CS
{
    /// <summary>
    /// The ExtrusionRoofManager class is to manage the creation of the Extrusion roof.
    /// </summary>
    class ExtrusionRoofManager
    {
        // To store a reference to the commandData
        ExternalCommandData m_commandData;
        // To store a reference to the creation document
        Autodesk.Revit.Creation.Document m_creationDoc;
        // To store a reference to the creation application
        Autodesk.Revit.Creation.Application m_creationApp;

        /// <summary>
        /// The construct of ExtrusionRoofManager class.
        /// </summary>
        /// <param name="commandData">A reference to the commandData.</param>
        public ExtrusionRoofManager(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_creationDoc = m_commandData.Application.ActiveDocument.Create;
            m_creationApp = m_commandData.Application.Create;        
        }

        /// <summary>
        /// Create a extrusion roof.
        /// </summary>
        /// <param name="profile">The profile combined of straight lines and arcs.</param>
        /// <param name="refPlane">The reference plane for the extrusion roof.</param>
        /// <param name="level">The reference level of the roof to be created.</param>
        /// <param name="roofType">The type of the newly created roof.</param>
        /// <param name="extrusionStart">The extrusion start point.</param>
        /// <param name="extrusionEnd">The extrusion end point.</param>
        /// <returns>Return a new created extrusion roof.</returns>
        public ExtrusionRoof CreateExtrusionRoof(CurveArray profile, ReferencePlane refPlane, Level level, RoofType roofType,
            double extrusionStart, double extrusionEnd)
        {
            ExtrusionRoof extrusionRoof = m_creationDoc.NewExtrusionRoof(profile, refPlane, level, roofType, extrusionStart, extrusionEnd);
            return extrusionRoof;
        }
    }
}
