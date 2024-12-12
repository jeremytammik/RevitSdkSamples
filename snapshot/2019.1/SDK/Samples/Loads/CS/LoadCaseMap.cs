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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.Loads.CS
{
    /// <summary>
    /// A class to store Load Case and it's properties.
    /// </summary>
    public class LoadCasesMap
    {
        LoadCase m_loadCase;
        string m_loadCasesName;     //Store the load case's name
        string m_loadCasesNumber;   //Store the load cases number
        Autodesk.Revit.DB.ElementId m_loadCasesNatureId; //Store the Id of the load case's nature
        Autodesk.Revit.DB.ElementId m_loadCasesSubcategoryId;//Store the Id of the load case's category
 
        /// <summary>
        /// LoadCasesName
        /// </summary>
        public string LoadCasesName
        {
            get
            {
                return m_loadCasesName;
            }
            set
            {
                m_loadCasesName = value;
                m_loadCase.Name = m_loadCasesName;
            }
        }

        /// <summary>
        /// LoadCasesNumber property.
        /// </summary>
        public string LoadCasesNumber
        {
            get
            {
                return m_loadCase.Number.ToString();
            }
        }

        /// <summary>
        /// LoadCasesNatureId property.
        /// </summary>
        public Autodesk.Revit.DB.ElementId LoadCasesNatureId
        {
            get
            {
                return m_loadCasesNatureId;
            }
            set
            {
                m_loadCasesNatureId = value;
                m_loadCase.NatureId = m_loadCasesNatureId;
            }
        }

        /// <summary>
        /// LoadCasesCategoryId property.
        /// </summary>
        public Autodesk.Revit.DB.ElementId LoadCasesSubCategoryId
        {
            get
            {
                return m_loadCasesSubcategoryId;
            }
            set
            {
                m_loadCasesSubcategoryId = value;
                m_loadCase.SubcategoryId = m_loadCasesSubcategoryId;
            }
        }

        /// <summary>
        /// Overload the constructor
        /// </summary>
        /// <param name="loadCase">Load Case</param>
        public LoadCasesMap(LoadCase loadCase)
        {
            m_loadCase = loadCase;
            m_loadCasesName = m_loadCase.Name;
            m_loadCasesNumber = m_loadCase.Number.ToString();
            m_loadCasesNatureId = m_loadCase.NatureId;
            m_loadCasesSubcategoryId = m_loadCase.SubcategoryId;
        }
    }

    /// <summary>
    /// A class to store Load Nature name
    /// </summary>
    public class LoadNaturesMap
    {
        LoadNature m_loadNature = null;
        string m_loadNaturesName = null;

        /// <summary>
        /// Get or set a load nature name.
        /// </summary>
        public string LoadNaturesName
        {
            get
            {
                return m_loadNaturesName;
            }
            set
            {
                m_loadNaturesName = value;
                m_loadNature.Name = m_loadNaturesName;
            }
        }

        /// <summary>
        /// constructor of LoadNaturesMap class 
        /// </summary>
        /// <param name="loadNature"></param>
        public LoadNaturesMap(LoadNature loadNature)
        {
            m_loadNature = loadNature;
            m_loadNaturesName = loadNature.Name;
        }
    }
}
