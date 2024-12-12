//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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

using Autodesk.Revit;

namespace Revit.SDK.Samples.ElementsFilter.CS
{
    /// <summary>
    /// All category filter modes
    /// </summary>
    public enum CategoryFilterMode
    {
        /// <summary>
        /// List all the built in categories within Revit.
        /// </summary>
        BuiltInCategory,
        /// <summary>
        /// List all categories or sub categories to which an element belongs.
        /// </summary>
        Category
    }

    /// <summary>
    /// Category filter configure data management.
    /// </summary>
    public class CategoryFilterConfigure : FilterConfigure
    {
        private List<Autodesk.Revit.Category> m_categories;
        private Autodesk.Revit.BuiltInCategory m_selectedBuiltInCategory;
        private Autodesk.Revit.Category m_selectedCategory = null;
        private CategoryFilterMode m_mode = CategoryFilterMode.BuiltInCategory;

        /// <summary>
        /// Generate a new FilterInfo object according to the configure.
        /// </summary>
        /// <param name="filterName">What's the filter name</param>
        /// <returns></returns>
        public override FilterInfo GenerateFilterInfo(string filterName)
        {
            FilterInfo filterInfo = new FilterInfo(filterName);
            StringBuilder sb = new StringBuilder();
            switch (m_mode)
            {
                case CategoryFilterMode.BuiltInCategory:
                        // create the category filter via RevitAPI.
                        filterInfo.Filter = m_commandData.Application.Create.Filter.NewCategoryFilter(m_selectedBuiltInCategory);
                        sb.AppendFormat("{0} {1} {2}", filterInfo.Name, ResourceMgr.GetString("BuiltInCategoryDes")
                            , m_selectedBuiltInCategory.ToString()); 
                        filterInfo.TipMessage = sb.ToString();
                        break;
                case CategoryFilterMode.Category:
                        if (null == m_selectedCategory)
                            return null;

                        // create the category filter via RevitAPI.
                        filterInfo.Filter = m_commandData.Application.Create.Filter.NewCategoryFilter(m_selectedCategory);
                        sb.AppendFormat("{0} {1} {2}", filterInfo.Name, ResourceMgr.GetString("CategoryDes")
                            , m_selectedCategory.Name);
                        filterInfo.TipMessage = sb.ToString();
                        break;
                default:
                    return null;
            }

            return (null == filterInfo.Filter) ? null : filterInfo;
        }

        /// <summary>
        /// Fill out the data to generate category filter.
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        public override void Fill(ExternalCommandData commandData)
        {
            if (null != m_commandData)
                return;
            m_commandData = commandData;

            if (null != m_categories)
                return;
            m_categories = new List<Category>();
            foreach (Category cat in commandData.Application.ActiveDocument.Settings.Categories)
            {
                m_categories.Add(cat);
            }
        }

        /// <summary>
        /// Get all built-in category.
        /// </summary>
        public Array BuiltInCategories
        {
            get
            {
                return Enum.GetValues(typeof(BuiltInCategory));
            }
        }

        /// <summary>
        ///  Get all categories
        /// </summary>
        public List<Autodesk.Revit.Category> Categories
        {
            get
            {
                return m_categories;
            }
        }

        /// <summary>
        /// Get all modes to create category filter.
        /// </summary>
        public Array Modes
        {
            get
            {
                return Enum.GetValues(typeof(CategoryFilterMode));
            }
        }

        /// <summary>
        /// Get/Set the selected built-in category.
        /// </summary>
        public Autodesk.Revit.BuiltInCategory SelectedBuiltInCategory
        {
            get
            {
                return m_selectedBuiltInCategory;
            }
            set
            {
                m_selectedBuiltInCategory = value;
            }
        }

        /// <summary>
        /// Get/Set the selected category.
        /// </summary>
        public Autodesk.Revit.Category SelectedCategory
        {
            get
            {
                return m_selectedCategory;
            }
            set
            {
                m_selectedCategory = value;
            }
        }

        /// <summary>
        /// Get/Set the mode to create category filter.
        /// </summary>
        public CategoryFilterMode Mode
        {
            get
            {
                return m_mode;
            }
            set
            {
                m_mode = value;
            }
        }        
    }
}
