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
using System.Linq;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Reference = Autodesk.Revit.DB.Reference;
using Exceptions = Autodesk.Revit.Exceptions;

namespace Revit.SDK.Samples.Selections.CS
{
    /// <summary>
    /// A enum class for specific selection type.
    /// </summary>
    public enum SelectionType
    {
        /// <summary>
        /// type for select element.
        /// </summary>
        Element,
        /// <summary>
        /// type for select face.
        /// </summary>
        Face,
        /// <summary>
        /// type for select edge.
        /// </summary>
        Edge,
        /// <summary>
        /// type for select point.
        /// </summary>
        Point
    }

    /// <summary>
    /// A class for object selection and storage.
    /// </summary>
    public class SelectionManager
    {
        /// <summary>
        /// To store a reference to the commandData.
        /// </summary>
        ExternalCommandData m_commandData;
        /// <summary>
        /// store the application
        /// </summary>
        UIApplication m_application;
        /// <summary>
        /// store the document
        /// </summary>
        UIDocument m_document;
        /// <summary>
        /// For basic creation.
        /// </summary>
        Autodesk.Revit.Creation.ItemFactoryBase m_CreationBase;
        /// <summary>
        /// The picked point of element.
        /// </summary>
        XYZ m_elemPickedPoint;
                
        SelectionType m_selectionType = SelectionType.Element;
        /// <summary>
        /// For specific selection type.
        /// </summary>
        public SelectionType SelectionType
        {
            get 
            { 
                return m_selectionType; 
            }
            set 
            { 
                m_selectionType = value; 
            }
        }

        Element m_selectedElement;
        /// <summary>
        /// Store the selected element.
        /// </summary>
        public Element SelectedElement
        {
            get
            { 
                return m_selectedElement;
            }
            set 
            { 
                m_selectedElement = value; 
            }
        }

        XYZ m_selectedPoint;
        /// <summary>
        /// Store the selected point. 
        /// When the point is picked, move the element to the point.
        /// </summary>
        public XYZ SelectedPoint
        {
            get 
            { 
                return m_selectedPoint; 
            }
            set 
            { 
                m_selectedPoint = value; 
                if (m_selectedElement != null && m_selectedPoint != null)
                {
                    MoveElement(m_selectedElement, m_selectedPoint);
                }
            }
        }       

        /// <summary>
        /// constructor of SelectionManager
        /// </summary>
        /// <param name="commandData"></param>
        public SelectionManager(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_application = m_commandData.Application;
            m_document = m_application.ActiveUIDocument;

            if (m_document.Document.IsFamilyDocument)
            {
                m_CreationBase = m_document.Document.FamilyCreate;
            }
            else
            {
                m_CreationBase = m_document.Document.Create;
            }
        }

        /// <summary>
        /// Select objects according to the selection type.
        /// </summary>
        public void SelectObjects()
        {
            switch (m_selectionType)
            {
                case SelectionType.Element:
                    PickElement(); // pick element
                    break;
                case SelectionType.Face:
                    break;
                case SelectionType.Edge:
                    break;
                case SelectionType.Point:
                    PickPoint(); // pick point
                    break;
            }
        }

        /// <summary>
        /// Pick the element from UI.
        /// </summary>
        internal void PickElement()
        {
            try
            {
                // Pick an element.
                Reference eRef = m_document.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Please pick an element.");
                if (eRef != null && eRef.ElementId != ElementId.InvalidElementId)
                {
                    SelectedElement = m_document.Document.GetElement(eRef);
                    m_elemPickedPoint = eRef.GlobalPoint;
                }
            }
            catch (Exceptions.OperationCanceledException)
            {
                // Element selection cancelled.
                SelectedElement = null;
            }
        }

        /// <summary>
        /// Pick the point from UI.
        /// </summary>
        internal void PickPoint()
        {
            try
            {
                // Pick a point.
                XYZ targetPoint = m_document.Selection.PickPoint("Please pick a point.");
                SelectedPoint = targetPoint;
            }
            catch (Exceptions.OperationCanceledException)
            {
                // Point selection cancelled.
                SelectedPoint = null;
            }
        }

        /// <summary>
        /// Move an element to the point.
        /// </summary>
        /// <param name="elem">The element to be moved.</param>
        /// <param name="targetPoint">The location element to be moved.</param>
        internal void MoveElement(Element elem, XYZ targetPoint)
        {
            XYZ vecToMove = targetPoint - m_elemPickedPoint;
            m_elemPickedPoint = targetPoint;
            ElementTransformUtils.MoveElement(m_document.Document,elem.Id, vecToMove);
        }
    }
}
