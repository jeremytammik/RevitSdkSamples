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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.BoundaryConditions.CS
{
    /// <summary>
    /// user select a element. If the selected element has boundary conditions, display 
    /// its parameter values else create one.
    /// this class prepare the needed data(the selected element type and its BC information) 
    /// and operate the Revit API
    /// </summary>
    public class BoundaryConditionsData
    {
        #region "Members"

        // the selected Element
        private Autodesk.Revit.DB.Element m_hostElement;

        // store all the corresponding BCs of the current selected host element 
        // and use the BC Id value as the key
        private Dictionary<int, Autodesk.Revit.DB.Structure.BoundaryConditions> m_bCsDictionary =
                new Dictionary<int, Autodesk.Revit.DB.Structure.BoundaryConditions>();

        // the object for which the grid in UI displays.
        private BCProperties m_bCProperties;

        #endregion

        #region "Properties"

        /// <summary>
        /// gets or sets the object for which the grid in UI displays. 
        /// </summary>
        public BCProperties BCProperties
        {
            get
            {
                return m_bCProperties;
            }
            set
            {
                m_bCProperties = value;
            }
        }

        /// <summary>
        /// get current host element
        /// </summary>
        public Autodesk.Revit.DB.Element HostElement
        {
            get
            {
                return m_hostElement;
            }
        }

        /// <summary>
        /// get all the BCs correspond with current host
        /// </summary>
        public Dictionary<int, Autodesk.Revit.DB.Structure.BoundaryConditions> BCs
        {
            get
            {
                return m_bCsDictionary;
            }
        }

        #endregion

        #region "Delegate" 

        //A delegate for create boundary condition with different type
        private delegate Autodesk.Revit.DB.Structure.BoundaryConditions
                CreateBCHandler(Autodesk.Revit.DB.Element HostElement);

        #endregion

        #region "Methods"

        /// <summary>
        /// construct function
        /// </summary>
        /// <param name="element"> host element</param>
        public BoundaryConditionsData(Autodesk.Revit.DB.Element element)
        {
            // store the selected element and its BCs
            SetBCHostMap(element);   
        }

        /// <summary>
        /// According to the selected element create corresponding Boundary Conditions.
        /// Add it into m_bCsDictionary.
        /// </summary>
        public bool CreateBoundaryConditions()
        {
            CreateBCHandler createBCH = null;
            
            // judge the type of the HostElement
            if (m_hostElement is FamilyInstance)
            {  
                FamilyInstance familyInstance = m_hostElement as FamilyInstance;
                StructuralType structuralType = familyInstance.StructuralType;

                if (structuralType == StructuralType.Beam)
                {
                    // create Line BC for beam
                    createBCH = new CreateBCHandler(CreateLineBC);
                }
                else if (structuralType == StructuralType.Brace  ||
                         structuralType == StructuralType.Column ||
                         structuralType == StructuralType.Footing)
                {
                    // create point BC for Column/brace
                    createBCH = new CreateBCHandler(CreatePointBC);
                }
            }
            else if (m_hostElement is Wall)
            {
                // create line BC for wall
                createBCH = new CreateBCHandler(CreateLineBC);
            }
            else if (m_hostElement is Floor)
            {
                // create area BC for Floor
                createBCH = new CreateBCHandler(CreateAreaBC);
            }
            else if (m_hostElement is WallFoundation)
            {
                // create line BC for WallFoundation
                createBCH = new CreateBCHandler(CreateLineBC);
            }

            // begin create
            Autodesk.Revit.DB.Structure.BoundaryConditions NewBC = null;
            try
            {
                NewBC = createBCH(m_hostElement);
                if (null == NewBC)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            // add the created Boundary Conditions into m_bCsDictionary
            m_bCsDictionary.Add(NewBC.Id.IntegerValue, NewBC);
            return true;
        }

        /// <summary>
        /// store the selected element and its corresponding BCs
        /// </summary>
        /// <param name="element"> use selected element in Revit UI(the host element)</param>
        private void SetBCHostMap(Autodesk.Revit.DB.Element element)
        {
            // set the Host element with current selected element
            m_hostElement = element;
            // retrieve the Document in which the Element resides.
            Document doc = element.Document;

            IEnumerable<Autodesk.Revit.DB.Structure.BoundaryConditions> boundaryConditions = from elem in
                                                                                       new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.Structure.BoundaryConditions)).ToElements()
                                                                                   let bC = elem as Autodesk.Revit.DB.Structure.BoundaryConditions
                                                                                   where bC != null && m_hostElement.Id.IntegerValue == bC.HostElement.Id.IntegerValue
                                                                                   select bC;
            foreach (Autodesk.Revit.DB.Structure.BoundaryConditions bC in boundaryConditions)
            {
                m_bCsDictionary.Add(bC.Id.IntegerValue, bC);
            }
        }

        /// <summary>
        /// Create a new Point BoundaryConditions Element. 
        /// All the parameter default as Fixed.
        /// </summary>
        /// <param name="hostElement"> 
        /// structural element which provide the analytical line end reference
        /// </param>
        /// <returns> the created Point BoundaryConditions Element</returns>
        private Autodesk.Revit.DB.Structure.BoundaryConditions CreatePointBC(Autodesk.Revit.DB.Element hostElement)
        {
            if (!(hostElement is FamilyInstance))
            {
                return null;
            }

            FamilyInstance familyInstance   = hostElement as FamilyInstance;
            AnalyticalModel analyticalModel = familyInstance.GetAnalyticalModel();
            Reference endReference          = null;

            Curve refCurve = analyticalModel.GetCurve();
            if (null != refCurve)
            {

                endReference = analyticalModel.GetReference(new AnalyticalModelSelector(refCurve,AnalyticalCurveSelector.EndPoint));
            }
            else
            {
                return null;
            }

            Autodesk.Revit.Creation.Document createDoc = hostElement.Document.Create;

            // invoke Document.NewPointBoundaryConditions Method 
            Autodesk.Revit.DB.Structure.BoundaryConditions createdBC = 
                    createDoc.NewPointBoundaryConditions(endReference, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            return createdBC;
        }

        /// <summary>
        /// Create a new Line BoundaryConditions Element. 
        /// All the parameter default as Fixed.
        /// </summary>
        /// <param name="hostElement">structural element which provide the hostElementId</param>
        /// <returns>the created Point BoundaryConditions Element</returns>
        private Autodesk.Revit.DB.Structure.BoundaryConditions CreateLineBC(Autodesk.Revit.DB.Element hostElement)
        {
            Autodesk.Revit.Creation.Document createDoc = hostElement.Document.Create;

            // invoke Document.NewLineBoundaryConditions Method
            Autodesk.Revit.DB.Structure.BoundaryConditions createdBC = 
                    createDoc.NewLineBoundaryConditions(hostElement.GetAnalyticalModel(), 0, 0, 0, 0, 0, 0, 0, 0);

            return createdBC;
        }

        /// <summary>
        /// Create a new Area BoundaryConditions Element. 
        /// All the parameter default as Fixed.
        /// </summary>
        /// <param name="hostElement">structural element which provide the hostElementId</param>
        /// <returns>the created Point BoundaryConditions Element</returns>
        private Autodesk.Revit.DB.Structure.BoundaryConditions CreateAreaBC(Autodesk.Revit.DB.Element hostElement)
        {
            Autodesk.Revit.Creation.Document createDoc = hostElement.Document.Create;

            // invoke Document.NewAreaBoundaryConditions Method
            Autodesk.Revit.DB.Structure.BoundaryConditions createdBC =
                    createDoc.NewAreaBoundaryConditions(hostElement.GetAnalyticalModel(), 0, 0, 0, 0, 0, 0);

            return createdBC;
        } 

        #endregion
    }
}
