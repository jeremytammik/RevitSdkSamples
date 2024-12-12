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
        String m_loadCasesName;     //Store the load case's name
        string m_loadCasesNumber;   //Store the load cases number
        Autodesk.Revit.DB.ElementId m_loadCasesNatureId; //Store the Id of the load case's nature
        Autodesk.Revit.DB.ElementId m_loadCasesCategoryId;//Store the Id of the load case's category
 
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
                SetLoadCasesName();
            }
        }

        /// <summary>
        /// LoadCasesNumber property.
        /// </summary>
        public string LoadCasesNumber
        {
            get
            {
                return m_loadCasesNumber;
            }
            set
            {
                m_loadCasesNumber = value;
                SetLoadCasesNumber();
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
                SetLoadCasesNatureId();
            }
        }

        /// <summary>
        /// CategoryId property.
        /// </summary>
        public Autodesk.Revit.DB.ElementId LoadCasesCategoryId
        {
            get
            {
                return m_loadCasesCategoryId;
            }
            set
            {
                m_loadCasesCategoryId = value;
                SetLoadCasesCategoryId();
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
            FindProperty();
        }

        /// <summary>
        /// Find the Nature and Category of the Load Case.
        /// </summary>
        /// <returns>A value that signifies whether success(true) or not.</returns>
        private bool FindProperty()
        {
            Parameter pNumber = m_loadCase.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.LOAD_CASE_NUMBER);
            if (null == pNumber)
            {
                m_loadCasesNumber = "";
            }
            m_loadCasesNumber = pNumber.AsInteger().ToString();

            Parameter pCategoryid = m_loadCase.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.LOAD_CASE_CATEGORY);
            m_loadCasesCategoryId = pCategoryid.AsElementId();

            Parameter pNatureId = m_loadCase.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.LOAD_CASE_NATURE);
            m_loadCasesNatureId = pNatureId.AsElementId();
            return true;
        }

        /// <summary>
        /// Set a new name for the Load Case
        /// </summary>
        /// <returns>A value that signifies whether success(true) or not.</returns>
        private bool SetLoadCasesName()
        {
            Parameter pName = m_loadCase.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.LOAD_CASE_NAME);
            pName.Set(m_loadCasesName);
            return true;
        }

        /// <summary>
        /// Set a new number for the Load Case
        /// </summary>
        /// <returns>A value that signifies whether success(true) or not.</returns>
        private bool SetLoadCasesNumber()
        {
            Parameter pNumber = m_loadCase.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.LOAD_CASE_NUMBER);
            pNumber.Set(m_loadCasesNumber);
            return true;
        }

        /// <summary>
        /// Set a new Nature for the Load Case
        /// </summary>
        /// <returns>A value that signifies whether success(true) or not.</returns>
        private bool SetLoadCasesNatureId()
        {
            Parameter pNature = m_loadCase.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.LOAD_CASE_NATURE);
            pNature.Set(m_loadCasesNatureId);
            return true;
        }

        /// <summary>
        /// Set a new category for the Load Case
        /// </summary>
        /// <returns>A value that signifies whether success(true) or not.</returns>
        private bool SetLoadCasesCategoryId()
        {
            Parameter pCategory = m_loadCase.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.LOAD_CASE_CATEGORY);
            pCategory.Set(m_loadCasesCategoryId);
            return true;
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
                SetLoadNaturesName();
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

        /// <summary>
        /// Set a new name for the Load Nature
        /// </summary>
        /// <returns>A value that signifies whether success(true) or not.</returns>
        private bool SetLoadNaturesName()
        {
            
            ParameterSet propeties = m_loadNature.Parameters;
            foreach (Parameter p in propeties)
            {
                if ("Name" == p.Definition.Name)
                {
                    p.Set(m_loadNaturesName);
                }
            }
            return true;
        }
    }
}
