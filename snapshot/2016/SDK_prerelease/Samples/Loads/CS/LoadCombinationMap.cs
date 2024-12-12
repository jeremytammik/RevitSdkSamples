//
// (C) Copyright 2003-2014 by Autodesk, Inc. 
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

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.Loads.CS
{
    /// <summary>
    /// A class to store Load Combination and it's properties.
    /// </summary>
    public class LoadCombinationMap
    {
        // Private Members
        String m_name;      // Indicate name column of LoadCombination DataGridView control
        String m_formula;   // Indicate formula column of LoadCombination DataGridView control
        String m_type;      // Indicate type column of LoadCombination DataGridView control
        String m_state;     // Indicate state column of LoadCombination DataGridView control
        String m_usage;     // Indicate usage column of LoadCombination DataGridView control

        /// <summary>
        /// Name property of LoadCombinationMap
        /// </summary>
        public String Name
        {
            get
            {
                return m_name;
            }
        }

        /// <summary>
        /// Formula property of LoadCombinationMap
        /// </summary>
        public String Formula
        {
            get
            {
                return m_formula;
            }
        }

        /// <summary>
        /// Type property of LoadCombinationMap
        /// </summary>
        public String Type
        {
            get
            {
                return m_type;
            }
        }

        /// <summary>
        /// State property of LoadCombinationMap
        /// </summary>
        public String State
        {
            get
            {
                return m_state;
            }
        }

        /// <summary>
        /// Usage property of LoadCombinationMap
        /// </summary>
        public String Usage
        {
            get
            {
                return m_usage;
            }
            set
            {
                m_usage = value;
            }
        }

        /// <summary>
        /// Default constructor of LoadCombinationMap
        /// </summary>
        /// <param name="combination">the reference of LoadCombination</param>
        public LoadCombinationMap(LoadCombination combination)
        {
            m_name = combination.Name;
            m_type = combination.Type.ToString();
            m_state = combination.State.ToString();
            Autodesk.Revit.DB.Document m_document = combination.Document;

            // Generate the formula field.
            StringBuilder formulaString = new StringBuilder();
            IList<LoadComponent> components = combination.GetComponents();
            foreach (LoadComponent component in components)
            {
                formulaString.Append(component.Factor);
                formulaString.Append("*");
                formulaString.Append(m_document.GetElement(component.LoadCaseOrCombinationId).Name);

                if (components.IndexOf(component) < components.Count - 1)
                {
                    formulaString.Append(" + ");
                }
            }

            m_formula = formulaString.ToString();
            
            // Generate the usage field.
            StringBuilder usageString = new StringBuilder();
            IList<ElementId> usageIds = combination.GetUsageIds();
            foreach (ElementId id in usageIds)
            {
                Element element = m_document.GetElement(id);
                usageString.Append(m_document.GetElement(id).Name);

                if (usageIds.IndexOf(id) < usageIds.Count - 1)
                {
                    usageString.Append(";");
                }
            }

            m_usage = usageString.ToString();
        }
    }
}
