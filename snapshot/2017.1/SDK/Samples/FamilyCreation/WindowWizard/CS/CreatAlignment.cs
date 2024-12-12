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
using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.WindowWizard.CS
{
    /// <summary>
    /// The class allows users to create alignment
    /// </summary>
    class CreateAlignment
    {
        #region Class Memeber Variables
        /// <summary>
        /// store the document
        /// </summary>
        Document m_document;
        
        /// <summary>
        /// store the family item factory of creation
        /// </summary>        
        Autodesk.Revit.Creation.FamilyItemFactory m_familyCreator;
        #endregion

        /// <summary>
        /// The constructor of CreateAlignment class
        /// </summary>        
        /// <param name="doc">the document</param>
        public CreateAlignment(Document doc)
        {
            m_document = doc;          
            m_familyCreator = m_document.FamilyCreate;
        }

        #region Class Implementation
        /// <summary>
        /// The method is used to create alignment between two faces
        /// </summary>
        /// <param name="view">the view</param>
        /// <param name="face1">face1</param>
        /// <param name="face2">face2</param>
        public void AddAlignment(View view, Face face1, Face face2)
        {
            PlanarFace pFace1 = null;
            PlanarFace pFace2 = null;
            if (face1 is PlanarFace)
                pFace1 = face1 as PlanarFace;
            if (face2 is PlanarFace)
                pFace2 = face2 as PlanarFace;
            if (pFace1 != null && pFace2 != null)
            {
                SubTransaction subTransaction = new SubTransaction(m_document);
                subTransaction.Start();
                m_familyCreator.NewAlignment(view, pFace1.Reference, pFace2.Reference);
                subTransaction.Commit();
            }
        }
        #endregion
    }
}
