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
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.Loads.CS
{
    /// <summary>
    /// Deal the LoadCase class which give methods to connect Revit and the user operation on the form
    /// </summary>
    public class LoadCaseDeal
    {
        // Private Members
        Autodesk.Revit.ApplicationServices.Application m_revit; // Store the reference of revit application
        Loads m_dataBuffer;                 
        List<string> m_newLoadNaturesName; //store all the new nature's name that should be added

        // Methods
        /// <summary>
        /// Default constructor of LoadCaseDeal
        /// </summary>
        public LoadCaseDeal(Loads dataBuffer)
        {
            m_dataBuffer = dataBuffer;
            m_revit = dataBuffer.RevitApplication;
            m_newLoadNaturesName = new List<string>();

            m_newLoadNaturesName.Add("EQ1");
            m_newLoadNaturesName.Add("EQ2");
            m_newLoadNaturesName.Add("W1");
            m_newLoadNaturesName.Add("W2");
            m_newLoadNaturesName.Add("W3");
            m_newLoadNaturesName.Add("W4");
            m_newLoadNaturesName.Add("Other");
        }

        /// <summary>
        /// prepare data for the dialog
        /// </summary>
        public void PrepareData()
        {
            //Create seven Load Natures first
            if (!CreateLoadNatures())
            {
                return;
            }

            //get all the categories of load cases
            UIApplication uiapplication = new UIApplication(m_revit);
            Categories categories = uiapplication.ActiveUIDocument.Document.Settings.Categories;
            Category category = categories.get_Item(BuiltInCategory.OST_LoadCases);
            CategoryNameMap categoryNameMap = category.SubCategories;
            System.Collections.IEnumerator iter = categoryNameMap.GetEnumerator();
            iter.Reset();
            while (iter.MoveNext())
            {
                Category temp = iter.Current as Category;
                if (null == temp)
                {
                    continue;
                }
                m_dataBuffer.LoadCasesCategory.Add(temp);
            }

            //get all the loadnatures name
            IList<Element> elements = new FilteredElementCollector(uiapplication.ActiveUIDocument.Document).OfClass(typeof(LoadNature)).ToElements();
            foreach (Element e in elements)
            {
                LoadNature nature = e as LoadNature;
                if (null != nature)
                {
                    m_dataBuffer.LoadNatures.Add(nature);
                    LoadNaturesMap newLoadNaturesMap = new LoadNaturesMap(nature);
                    m_dataBuffer.LoadNaturesMap.Add(newLoadNaturesMap);

                }
            }
            elements = new FilteredElementCollector(uiapplication.ActiveUIDocument.Document).OfClass(typeof(LoadCase)).ToElements();
            foreach (Element e in elements)
            {
                //get all the loadcases
                LoadCase loadCase = e as LoadCase;
                if (null != loadCase)
                {
                    m_dataBuffer.LoadCases.Add(loadCase);
                    LoadCasesMap newLoadCaseMap = new LoadCasesMap(loadCase);
                    m_dataBuffer.LoadCasesMap.Add(newLoadCaseMap);
                }
            }
        }

        /// <summary>
        /// create some load case natures named EQ1, EQ2, W1, W2, W3, W4, Other
        /// </summary>
        /// <returns></returns>
        private bool CreateLoadNatures()
        {
            //try to add some new load natures
            try
            {
                UIApplication uiapplication = new UIApplication(m_revit);
                for (int i = 0; i < m_newLoadNaturesName.Count; i++)
                {
                    string temp = m_newLoadNaturesName[i];
                    uiapplication.ActiveUIDocument.Document.Create.NewLoadNature(temp);
                }

            }
            catch (Exception e)
            {
                m_dataBuffer.ErrorInformation += e.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// add a new load nature
        /// </summary>
        /// <param name="index">the selected nature's index in the nature map</param>
        /// <returns></returns>
        public bool AddLoadNature(int index)
        {

            string natureName = null;  //the load nature's name to be added
            bool isUnique = false;     // check if the name is unique    
            LoadNaturesMap myLoadNature = null;

            //try to get out the loadnature from the map
            try
            {
                myLoadNature = m_dataBuffer.LoadNaturesMap[index];
            }
            catch (Exception e)
            {
                m_dataBuffer.ErrorInformation += e.ToString();
                return false;
            }

            //Can not get the load nature
            if (null == myLoadNature)
            {
                m_dataBuffer.ErrorInformation += "Can't find the nature";
                return false;
            }

            //check if the name is unique
            natureName = myLoadNature.LoadNaturesName;
            while (!isUnique)
            {
                natureName += "(1)";
                isUnique = IsNatureNameUnique(natureName);
            }

            //try to create a load nature
            try
            {
                UIApplication uiapplication = new UIApplication(m_revit);
                LoadNature newLoadNature = uiapplication.ActiveUIDocument.Document.Create.NewLoadNature(natureName);
                if (null == newLoadNature)
                {
                    m_dataBuffer.ErrorInformation += "Create Failed";
                    return false;
                }

                //add the load nature into the list and maps
                m_dataBuffer.LoadNatures.Add(newLoadNature);
                LoadNaturesMap newMap = new LoadNaturesMap(newLoadNature);
                m_dataBuffer.LoadNaturesMap.Add(newMap);
            }
            catch (Exception e)
            {
                m_dataBuffer.ErrorInformation += e.ToString();
                return false;
            }
            return true;
        }
      
        /// <summary>
        /// Duplicate a new load case
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool DuplicateLoadCase(int index)
        {
            LoadCasesMap myLoadCase = null;
            bool isUnique = false;
            string caseName = null;

            //try to get the load case from the map
            try
            {
                myLoadCase = m_dataBuffer.LoadCasesMap[index];
            }
            catch (Exception e)
            {
                m_dataBuffer.ErrorInformation += e.ToString();
                return false;
            }

            //get nothing 
            if (null == myLoadCase)
            {
                m_dataBuffer.ErrorInformation += "Can not find the load case";
                return false;
            }

            //check the name
            caseName = myLoadCase.LoadCasesName;
            while (!isUnique)
            {
                caseName += "(1)";
                isUnique = IsCaseNameUnique(caseName);
            }

            //get the selected case's nature
            Category caseCategory = null;
            LoadNature caseNature = null;
            Autodesk.Revit.DB.ElementId categoryId = myLoadCase.LoadCasesCategoryId;
            Autodesk.Revit.DB.ElementId natureId = myLoadCase.LoadCasesNatureId;

            UIApplication uiapplication = new UIApplication(m_revit);
            caseNature = uiapplication.ActiveUIDocument.Document.GetElement(natureId) as LoadNature;

            //get the selected case's category
            Categories categories = uiapplication.ActiveUIDocument.Document.Settings.Categories;
            Category category = categories.get_Item(BuiltInCategory.OST_LoadCases);
            CategoryNameMap categoryNameMap = category.SubCategories;
            System.Collections.IEnumerator iter = categoryNameMap.GetEnumerator();
            iter.Reset();
            while (iter.MoveNext())
            {
                Category tempC = iter.Current as Category;
                if (null != tempC && (categoryId.IntegerValue == tempC.Id.IntegerValue))
                {
                    caseCategory = tempC;
                    break;
                }
            }

            //check if lack of the information
            if (null == caseNature || null == caseCategory || null == caseName)
            {
                m_dataBuffer.ErrorInformation += "Can't find the load case";
                return false;
            }

            //try to create a load case
            try
            {
                LoadCase newLoadCase = uiapplication.ActiveUIDocument.Document.Create.NewLoadCase(caseName, caseNature, caseCategory);
                if (null == newLoadCase)
                {
                    m_dataBuffer.ErrorInformation += "Create Load Case Failed";
                    return false;
                }
                //add the new case into list and map
                m_dataBuffer.LoadCases.Add(newLoadCase);
                LoadCasesMap newLoadCaseMap = new LoadCasesMap(newLoadCase);
                m_dataBuffer.LoadCasesMap.Add(newLoadCaseMap);
            }
            catch (Exception e)
            {
                m_dataBuffer.ErrorInformation += e.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// check if the case's name is unique
        /// </summary>
        /// <param name="name">the name to be checked</param>
        /// <returns>true will be returned if the name is unique</returns>
        public bool IsCaseNameUnique(string name)
        {
            //compare the name with the name of each case in the map
            for (int i = 0; i < m_dataBuffer.LoadCasesMap.Count; i++)
            {
                string nameTemp = m_dataBuffer.LoadCasesMap[i].LoadCasesName;
                if (name == nameTemp)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// check if the nature's name is unique
        /// </summary>
        /// <param name="name">the name to be checked</param>
        /// <returns>true will be returned if the name is unique</returns>
        public bool IsNatureNameUnique(string name)
        {
            //compare the name with the name of each nature in the map
            for (int i = 0; i < m_dataBuffer.LoadNatures.Count; i++)
            {
                string nameTemp = m_dataBuffer.LoadNaturesMap[i].LoadNaturesName;
                if (name == nameTemp)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
