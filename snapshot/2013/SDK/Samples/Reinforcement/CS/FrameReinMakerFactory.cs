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
using System.Text;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.Reinforcement.CS
{
    /// <summary>
    /// The factory to create the corresponding FrameReinMaker, such as BeamFramReinMaker.
    /// </summary>
    class FrameReinMakerFactory
    {
        // Private members
        ExternalCommandData m_commandData;  // the ExternalCommandData reference
        FamilyInstance m_hostObject;        // the host object

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="commandData">the ExternalCommandData reference</param>
        public FrameReinMakerFactory(ExternalCommandData commandData)
        {
            m_commandData = commandData;

            if (!GetHostObject())
            {
                throw new Exception("Please select a beam or column.");
            }
        }


        /// <summary>
        /// check the condition of host object and see whether the rebars can be placed on
        /// </summary>
        /// <returns></returns>
        public bool AssertData()
        {
            // judge whether is any rebar exist in the beam or column
            if (new FilteredElementCollector(m_commandData.Application.ActiveUIDocument.Document)
                .OfClass(typeof(Rebar))
                .Cast<Rebar>()
                .Where(x => x.Host.Id.IntegerValue == m_hostObject.Id.IntegerValue).Count() > 0)
                return false;
            
            return true;
        }

        /// <summary>
        /// The main method which create the corresponding FrameReinMaker according to 
        /// the host object type, and invoke Run() method to create reinforcement rebars
        /// </summary>
        /// <returns>true if the creation is successful, otherwise false</returns>
        public bool work()
        {
            // define an IFrameReinMaker interface to create reinforcement rebars
            IFrameReinMaker maker = null;

            // create FrameReinMaker instance according to host object type
            switch (m_hostObject.StructuralType)
            {
                case StructuralType.Beam:   // if host object is a beam
                    maker = new BeamFramReinMaker(m_commandData, m_hostObject);
                    break;
                case StructuralType.Column: // if host object is a column
                    maker = new ColumnFramReinMaker(m_commandData, m_hostObject);
                    break;
                default:
                    break;
            }

            // invoke Run() method to do the reinforcement creation
            maker.Run();

            return true;
        }


        /// <summary>
        /// Get the selected element as the host object, also check if the selected element is expected host object
        /// </summary>
        /// <returns>true if get the selected element, otherwise false.</returns>
        private bool GetHostObject()
        {
            List<ElementId> selectedIds = new List<ElementId>();
            foreach (Autodesk.Revit.DB.Element elem in m_commandData.Application.ActiveUIDocument.Selection.Elements)
            {
                selectedIds.Add(elem.Id);
            }
            if (selectedIds.Count != 1)
                return false;
            //
            // Construct filters to find expected host object: 
            // . Host should be Beam/Column structural type.
            // . and it's material type should be Concrete
            // . and it should be FamilyInstance
            //
            // Structural type filters firstly
            LogicalOrFilter stFilter = new LogicalOrFilter(
                new ElementStructuralTypeFilter(StructuralType.Beam),
                new ElementStructuralTypeFilter(StructuralType.Column));
            // StructuralMaterialType should be Concrete
            LogicalAndFilter hostFilter = new LogicalAndFilter(stFilter,
                new StructuralMaterialTypeFilter(StructuralMaterialType.Concrete));
            //
            // Expected host object
            FilteredElementCollector collector = new FilteredElementCollector(m_commandData.Application.ActiveUIDocument.Document, selectedIds);
            m_hostObject = collector
                .OfClass(typeof(FamilyInstance)) // FamilyInstance
                .WherePasses(hostFilter) // Filters
                .FirstElement() as FamilyInstance;
            return (null != m_hostObject);
        }
    }
}
