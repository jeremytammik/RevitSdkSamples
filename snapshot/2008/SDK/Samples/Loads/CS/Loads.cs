//
// (C) Copyright 2003-2007 by Autodesk, Inc. 
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
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.Loads.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    public class Loads : IExternalCommand
    {
        #region Private Data Members
        // Mainly used data definition
        Autodesk.Revit.Application m_revit;     // Store the reference of revit
        LoadCombinationDeal m_combinationDeal;  // the deal class on load combination page
        LoadCaseDeal m_loadCaseDeal;            // the deal class on load case page
        String m_errorInformation;              // Store the error information

        // Define the data mainly used in LoadCombinationDeal class
        List<String> m_usageNameList;       // Store all the usage names in current document
        List<LoadUsage> m_loadUsageList;    // Used to store all the load usages
        List<String> m_combinationNameList; // Store all the combination names in current document 
        List<LoadCombinationMap> m_LoadCombinationMap;
                                // Store all the Load Combination information include the user add.
        List<FormulaMap> m_formulaMap;      // Store the formula information the user add
        List<UsageMap> m_usageMap;

        // Define the data mainly used in LoadCaseDeal class             
        List<Category> m_loadCasesCategory;     //Store the load case's categoty
        List<LoadCase> m_loadCases;             //Store all the load cases in current document
        List<LoadNature> m_loadNatures;         //Store all the load natures in current document
        List<LoadCasesMap> m_loadCasesMap;      // Store all the load case information include the user add.
        List<LoadNaturesMap> m_loadNaturesMap;  //Store all the load natures information
        #endregion
        
        #region Properties
        /// <summary>
        /// Used as the dataSource of load cases DataGridView control,
        /// and the information which support load case creation also.
        /// </summary>
        public List<LoadCasesMap> LoadCasesMap
        {
            get
            {
                return m_loadCasesMap;
            }
        }

		/// <summary>
		/// Used as the dataSource of load natures DataGridView control,
		/// and the information which support load nature creation also.
		/// </summary>
        public List<LoadNaturesMap> LoadNaturesMap
        {
            get
            {
                return m_loadNaturesMap;
            }
        }

		/// <summary>
		/// save all loadnature object in current project
		/// </summary>
        public List<LoadNature> LoadNatures
        {
            get
            {
                return m_loadNatures;
            }
        }

		/// <summary>
		/// save all loadcase object in current project
		/// </summary>
        public List<LoadCase> LoadCases
        {
            get
            {
                return m_loadCases;
            }
        }

		/// <summary>
		/// save all loadcase category in current project
		/// </summary>
        public List<Category> LoadCasesCategory
        {
            get
            {
                return m_loadCasesCategory;
            }
        }

		/// <summary>
		/// object which do add, delete and update command on load related objects
		/// </summary>
        public LoadCaseDeal LoadCasesDeal
        {
            get
            {
                return m_loadCaseDeal;
            }
        }

        /// <summary>
        /// Store the reference of revit
        /// </summary>
        public Autodesk.Revit.Application RevitApplication
        {
            get
            {
                return m_revit;
            }
        }

        /// <summary>
        /// LoadUsageNames property, used to store all the usage names in current document
        /// </summary>
        public List<String> LoadUsageNames
        {
            get
            {
                return m_usageNameList;
            }
        }

        /// <summary>
        /// Used to store all the load usages in current document, include the user add
        /// </summary>
        public List<LoadUsage> LoadUsages
        {
            get
            {
                return m_loadUsageList;
            }
        }

        /// <summary>
        /// LoadCombinationNames property, used to store all the combination names in current document
        /// </summary>
        public List<String> LoadCombinationNames
        {
            get
            {
                return m_combinationNameList;
            }
        }

        /// <summary>
        /// Show the error information while contact with revit
        /// </summary>
        public String ErrorInformation
        {
            get
            {
                return m_errorInformation;
            }
            set
            {
                m_errorInformation = value;
            }
        }

        /// <summary>
        /// Used as the dataSource of load combination DataGridView control,
        /// and the information which support load combination creation also.
        /// </summary>
        public List<LoadCombinationMap> LoadCombinationMap
        {
            get
            {
                return m_LoadCombinationMap;
            }
        }

		/// <summary>
		/// Store all load combination formular names
		/// </summary>
        public List<FormulaMap> FormulaMap
        {
            get
            {
                return m_formulaMap;
            }
        }

		/// <summary>
		/// Store all load usage
		/// </summary>
        public List<UsageMap> UsageMap
        {
            get
            {
                return m_usageMap;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Default constructor of Loads
        /// </summary>
        public Loads()
        {
            m_usageNameList = new List<string>();
            m_combinationNameList = new List<string>();
            m_LoadCombinationMap = new List<LoadCombinationMap>();
            m_loadUsageList = new List<LoadUsage>();
            m_formulaMap = new List<FormulaMap>();
            m_usageMap = new List<UsageMap>();

            m_loadCasesCategory = new List<Category>();
            m_loadCases = new List<LoadCase>();
            m_loadNatures = new List<LoadNature>();
            m_loadCasesMap = new List<LoadCasesMap>();
            m_loadNaturesMap = new List<LoadNaturesMap>();
        }

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public IExternalCommand.Result Execute(ExternalCommandData commandData,
                                                    ref string message, ElementSet elements)
        {
            m_revit = commandData.Application;

            // Initialize the helper classes.
            m_combinationDeal = new LoadCombinationDeal(this);
            m_loadCaseDeal = new LoadCaseDeal(this);

            // Prepare some data for the form displaying
            PrepareData();


            // Display the form and wait for the user's operate.
            // This class give some public methods to add or delete LoadUsage and delete LoadCombination
            // The form will use these methods to add or delete dynamically.
            // If the user press cancel button, return Cancelled to roll back All the changes.
            using (LoadsForm displayForm = new LoadsForm(this))
            {
                if (DialogResult.OK != displayForm.ShowDialog())
                {
                    return IExternalCommand.Result.Cancelled;
                }
            }

            // If everything goes right, return succeeded.
            return IExternalCommand.Result.Succeeded;
        }

        /// <summary>
        /// Prepare the data for the form displaying.
        /// </summary>
        void PrepareData()
        {
            // Prepare the data of the LoadCase page on form
            m_loadCaseDeal.PrepareData();

            //Prepare the data of the LoadCombination page on form
            m_combinationDeal.PrepareData();
        }

        /// <summary>
        /// Create new Load Combination
        /// </summary>
        /// <param name="name">The new Load Combination name</param>
        /// <param name="typeId">The index of new Load Combination Type</param>
        /// <param name="stateId">The index of new Load Combination State</param>
        /// <returns>true if the creation was successful; otherwise, false</returns>
        public Boolean NewLoadCombination(String name, int typeId, int stateId)
        {
            // In order to refresh the combination DataGridView,
            // We should do like as follow
            m_LoadCombinationMap = new List<LoadCombinationMap>(m_LoadCombinationMap);

            // Just go to run NewLoadCombination method of LoadCombinationDeal class
            return m_combinationDeal.NewLoadCombination(name, typeId, stateId);
        }

        /// <summary>
        /// Delete the selected Load Combination
        /// </summary>
        /// <param name="index">The selected index in the DataGridView</param>
        /// <returns>true if the delete operation was successful; otherwise, false</returns>
        public Boolean DeleteCombination(int index)
        {
            // Just go to run DeleteCombination method of LoadCombinationDeal class
            return m_combinationDeal.DeleteCombination(index);
        }

        /// <summary>
        /// Create a new load combination usage
        /// </summary>
        /// <param name="usageName">The new Load Usage name</param>
        /// <returns>true if the process is successful; otherwise, false</returns> 
        public Boolean NewLoadUsage(String usageName)
        {
            // In order to refresh the usage DataGridView,
            // We should do like as follow
            m_usageMap = new List<UsageMap>(m_usageMap);

            // Just go to run NewLoadUsage method of LoadCombinationDeal class
            return m_combinationDeal.NewLoadUsage(usageName);
        }

        /// <summary>
        /// Delete the selected Load Usage
        /// </summary>
        /// <param name="index">The selected index in the DataGridView</param>
        /// <returns>true if the delete operation was successful; otherwise, false</returns>
        public Boolean DeleteUsage(int index)
        {
            // Just go to run DeleteUsage method of LoadCombinationDeal class
            if (false == m_combinationDeal.DeleteUsage(index))
            {
                return false;
            }

            // In order to refresh the usage DataGridView,
            // We should do like as follow
            if (0 == m_usageMap.Count)
            {
                m_usageMap = new List<UsageMap>();
            }
            return true;
        }

		/// <summary>
		/// Change usage name when the user modify it on the form
		/// </summary>
        /// <param name="oldName">The name before modification</param>
        /// <param name="newName">The name after modification</param>
        /// <returns>true if the modification was successful; otherwise, false</returns>
        public Boolean ModifyUsageName(String oldName, String newName)
        {
            // Just go to run ModifyUsageName method of LoadCombinationDeal class
            return m_combinationDeal.ModifyUsageName(oldName, newName);
        }

        /// <summary>
        /// Add a formula when the user click Add button to new a formula
        /// </summary>
        /// <returns>true if the creation is successful; otherwise, false</returns>
        public Boolean AddFormula()
        {
            // Get the first member in LoadCases as the Case
            LoadCase loadCase = m_loadCases[0];
            if (null == loadCase)
            {
                m_errorInformation = "Can't not find a LoadCase.";
                return false;
            }
            String caseName = loadCase.Name;

            // In order to refresh the formula DataGridView,
            // We should do like as follow
            m_formulaMap = new List<FormulaMap>(m_formulaMap);

            // Run AddFormula method of LoadCombinationDeal class
            return m_combinationDeal.AddFormula(caseName);
        }

        /// <summary>
        /// Delete the selected Load Formula
        /// </summary>
        /// <param name="index">The selected index in the DataGridView</param>
        /// <returns>true if the delete operation was successful; otherwise, false</returns>
        public Boolean DeleteFormula(int index)
        {
            // Just remove that data.
            try
            {
                m_formulaMap.RemoveAt(index);
            }
            catch (Exception e)
            {
                m_errorInformation = e.ToString();
                return false;
            }
            return true;
        }
        #endregion
    }
}
