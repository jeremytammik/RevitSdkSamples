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
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.WindowWizard.CS
{
    /// <summary>
    /// The class allows users to create dimension using Document.FamilyCreate.NewDimension() function
    /// </summary>
    class CreateDimension
    {
        #region Class Memeber Variables
        /// <summary>
        /// store the document
        /// </summary>
        Document m_document;

        /// <summary>
        /// store the application
        /// </summary>
        Application m_application;
        #endregion

        /// <summary>
        /// constructor of CreateDimension class
        /// </summary>
        /// <param name="app">the application</param>
        /// <param name="doc">the document</param>
        public CreateDimension(Application app, Document doc)
        {
            m_application = app;
            m_document = doc;
        }

        #region Class Implementation
        /// <summary>
        /// This method is used to create dimension among three reference planes
        /// </summary>
        /// <param name="view">the view</param>
        /// <param name="refPlane1">the first reference plane</param>
        /// <param name="refPlane2">the second reference plane</param>
        /// <param name="refPlane">the middle reference plane</param>
        /// <returns>the new dimension</returns>
        public Dimension AddDimension(View view, ReferencePlane refPlane1, ReferencePlane refPlane2, ReferencePlane refPlane)
        {
            Dimension dim;
            Autodesk.Revit.DB.XYZ startPoint = new Autodesk.Revit.DB.XYZ ();
            Autodesk.Revit.DB.XYZ endPoint = new Autodesk.Revit.DB.XYZ ();
            Line line;
            Reference ref1;
            Reference ref2;
            Reference ref3;
            ReferenceArray refArray = new ReferenceArray();
            ref1 = refPlane1.Reference;
            ref2 = refPlane2.Reference;
            ref3 = refPlane.Reference;
            startPoint = refPlane1.FreeEnd;
            endPoint = refPlane2.FreeEnd;
            line = m_application.Create.NewLineBound(startPoint, endPoint);
            if (null != ref1 && null != ref2 && null != ref3)
            {
                refArray.Append(ref1);
                refArray.Append(ref3);
                refArray.Append(ref2);
            }
            SubTransaction subTransaction = new SubTransaction(m_document);
            subTransaction.Start();
            dim = m_document.FamilyCreate.NewDimension(view, line, refArray);
            subTransaction.Commit();
            return dim;
        }
      
        /// <summary>
        /// The method is used to create dimension between referenceplane and face
        /// </summary>
        /// <param name="view">the view in which the dimension is created</param>
        /// <param name="refPlane">the reference plane</param>
        /// <param name="face">the face</param>
        /// <returns>the new dimension</returns>
        public Dimension AddDimension(View view, ReferencePlane refPlane, Face face)
        {
            Dimension dim;
            Autodesk.Revit.DB.XYZ startPoint = new Autodesk.Revit.DB.XYZ ();
            Autodesk.Revit.DB.XYZ endPoint = new Autodesk.Revit.DB.XYZ ();
            Line line;
            Reference ref1;
            Reference ref2;
            ReferenceArray refArray = new ReferenceArray();
            ref1 = refPlane.Reference;
            PlanarFace pFace = face as PlanarFace;
            ref2 = pFace.Reference;
            if (null != ref1 && null != ref2)
            {
                refArray.Append(ref1);
                refArray.Append(ref2);
            }
            startPoint = refPlane.FreeEnd;
            endPoint = new Autodesk.Revit.DB.XYZ (startPoint.X, pFace.Origin.Y, startPoint.Z );
            SubTransaction subTransaction = new SubTransaction(m_document);
            subTransaction.Start();
            line = m_application.Create.NewLineBound(startPoint, endPoint);
            dim = m_document.FamilyCreate.NewDimension(view, line, refArray);
            subTransaction.Commit();
            return dim;
        }

        /// <summary>
        /// The method is used to create dimension between two faces
        /// </summary>
        /// <param name="view">the view</param>
        /// <param name="face1">the first face</param>
        /// <param name="face2">the second face</param>
        /// <returns>the new dimension</returns>
        public Dimension AddDimension(View view, Face face1, Face face2)
        {
            Dimension dim;
            Autodesk.Revit.DB.XYZ startPoint = new Autodesk.Revit.DB.XYZ ();
            Autodesk.Revit.DB.XYZ endPoint = new Autodesk.Revit.DB.XYZ ();
            Line line;
            Reference ref1;
            Reference ref2;
            ReferenceArray refArray = new ReferenceArray();            
            PlanarFace pFace1 = face1 as PlanarFace;
            ref1 = pFace1.Reference;
            PlanarFace pFace2 = face2 as PlanarFace;
            ref2 = pFace2.Reference;
            if (null != ref1 && null != ref2)
            {
                refArray.Append(ref1);
                refArray.Append(ref2);
            }
            startPoint = pFace1.Origin;
            endPoint = new Autodesk.Revit.DB.XYZ (startPoint.X, pFace2.Origin.Y, startPoint.Z);
            SubTransaction subTransaction = new SubTransaction(m_document);
            subTransaction.Start();
            line = m_application.Create.NewLineBound(startPoint, endPoint);
            dim = m_document.FamilyCreate.NewDimension(view, line, refArray);
            subTransaction.Commit();
            return dim;
        }
        #endregion
    }
}
