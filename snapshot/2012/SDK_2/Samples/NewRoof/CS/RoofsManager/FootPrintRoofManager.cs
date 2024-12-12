//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.NewRoof.RoofsManager.CS
{
    /// <summary>
    /// The FootPrintRoofManager class is to manage the creation of the footprint roof.
    /// </summary>
    class FootPrintRoofManager
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
        public FootPrintRoofManager(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_creationDoc = m_commandData.Application.ActiveUIDocument.Document.Create;
            m_creationApp = m_commandData.Application.Application.Create;        
        }

        /// <summary>
        /// Create a footprint roof.
        /// </summary>
        /// <param name="footPrint">The footprint is a curve loop, or a wall loop, or loops combined of walls and curves</param>
        /// <param name="level">The base level of the roof to be created.</param>
        /// <param name="roofType">The type of the newly created roof.</param>
        /// <returns>Return a new created footprint roof.</returns>
        public FootPrintRoof CreateFootPrintRoof(CurveArray footPrint, Level level, RoofType roofType)
        {
            FootPrintRoof footprintRoof = null;
            Transaction createRoofTransaction = new Transaction(m_commandData.Application.ActiveUIDocument.Document, "FootPrintRoof");
            createRoofTransaction.Start();
            try
            {
                ModelCurveArray footPrintToModelCurveMapping = new ModelCurveArray();
                footprintRoof = m_creationDoc.NewFootPrintRoof(footPrint, level, roofType, out footPrintToModelCurveMapping);
                createRoofTransaction.Commit();
            }
            catch (System.Exception e)
            {
                createRoofTransaction.RollBack();
                throw e;
            }
            
            return footprintRoof;
        }

    }
}
