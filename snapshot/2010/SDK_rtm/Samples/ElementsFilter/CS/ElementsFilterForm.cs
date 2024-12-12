//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Enums;

namespace Revit.SDK.Samples.ElementsFilter.CS
{
    /// <summary>
    /// The main UI form.
    /// </summary>
    public partial class ElementsFilterForm : Form
    {
        // private fields
        private ExternalCommandData m_commandData;
        private Dictionary<TabPage, FilterConfigure> m_controlToConfigure;
        private FilterMgr m_filterMgr;
        private CategoryFilterConfigure m_categoryFilterConfigure;
        private FamilyFilterConfigure m_familyFilterConfigure;
        private SymbolFilterConfigure m_symbolFilterConfigure;
        private ParameterFilterConfigure m_parameterFilterConfigure;
        private TypeFilterConfigure m_typeFilterConfigure;
        private StructureFilterConfigure m_structureFilterConfigure;
        private bool m_isInitialized;

        /// <summary>
        /// Initializes application data
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        public ElementsFilterForm(ExternalCommandData commandData)
        {
            m_isInitialized = false;
            InitializeComponent();            
            criteriaListBox.SelectedIndex = -1;    // no criteria is selected first.

            // application data initialized here
            m_commandData = commandData;
            m_filterMgr = new FilterMgr(m_commandData);

            // bonding tab page with its filter configure object
            m_controlToConfigure = new Dictionary<TabPage, FilterConfigure>();
            m_categoryFilterConfigure = new CategoryFilterConfigure();
            m_controlToConfigure.Add(categoryFilterTabPage, m_categoryFilterConfigure);
            m_familyFilterConfigure = new FamilyFilterConfigure();
            m_controlToConfigure.Add(familyFilterTabPage, m_familyFilterConfigure);
            m_symbolFilterConfigure = new SymbolFilterConfigure();
            m_controlToConfigure.Add(symbolFilterTabPage, m_symbolFilterConfigure);
            m_parameterFilterConfigure = new ParameterFilterConfigure();
            m_controlToConfigure.Add(parameterFilterTabPage, m_parameterFilterConfigure);
            m_typeFilterConfigure = new TypeFilterConfigure();
            m_controlToConfigure.Add(typeFilterTabPage, m_typeFilterConfigure);
            m_structureFilterConfigure = new StructureFilterConfigure();
            m_controlToConfigure.Add(structureFilterTabPage, m_structureFilterConfigure);

            m_categoryFilterConfigure.Fill(m_commandData);
            if (null == m_ModeComboBox.DataSource || null == m_categoryListBox.DataSource)
            {
                m_ModeComboBox.DataSource = m_categoryFilterConfigure.Modes;
                m_categoryListBox.DataSource = m_categoryFilterConfigure.BuiltInCategories;
            }
        }

        /// <summary>
        /// Create a new filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newFilterButton_Click(object sender, EventArgs e)
        {
            FilterConfigure activeFilterConfigure = m_controlToConfigure[m_filtersTabControl.SelectedTab];

            if (!m_filterMgr.IsValidName(filterNameTextBox.Text))
            {
                MessageBox.Show(ResourceMgr.GetString("InvalidWarning"));
                return;
            }

            if (m_filterMgr.IsUsedName(filterNameTextBox.Text))
            {
                string message = ResourceMgr.GetString("NewNameMessage");
                string caption = ResourceMgr.GetString("NewNameCaption");
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(this, message, caption, buttons);
                if (result == DialogResult.Yes)
                {
                    filterNameTextBox.Text = m_filterMgr.AUID;
                    return;
                }
            }

            try
            {
                FilterInfo newFilterInfo = activeFilterConfigure.GenerateFilterInfo(filterNameTextBox.Text);
                if (null == newFilterInfo)
                {
                    MessageBox.Show(ResourceMgr.GetString("FailedNewFilterWarning"));
                }
                else
                {
                    m_filterMgr.Filters.Add(newFilterInfo);
                    filterNameTextBox.Text = m_filterMgr.AUID;
                    filtersListBox.DataSource = null;
                    filtersListBox.DataSource = m_filterMgr.Filters;
                    filtersListBox.DisplayMember = "Name";
                    filtersListBox.SelectedIndex = filtersListBox.Items.Count - 1;
                }
            }
            catch(Exception )
            {
                MessageBox.Show(ResourceMgr.GetString("FailedNewFilterWarning"));
            }
        }

        /// <summary>
        /// Delete the filter which is the selected item in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteFilterButton_Click(object sender, EventArgs e)
        {
            FilterInfo toBeDeletedFilterInfo = filtersListBox.SelectedValue as FilterInfo;
            if (null != toBeDeletedFilterInfo && 0 == toBeDeletedFilterInfo.UsedCount)
            {
                m_filterMgr.Filters.Remove(toBeDeletedFilterInfo);
                filterNameTextBox.Text = m_filterMgr.AUID;
                filtersListBox.DataSource = null;
                filtersListBox.DataSource = m_filterMgr.Filters;
                filtersListBox.DisplayMember = "Name";
                if (filtersListBox.Items.Count >= 1)
                {
                    filtersListBox.SelectedIndex = filtersListBox.Items.Count - 1;
                }
            }
            else
            {
                MessageBox.Show(ResourceMgr.GetString("DeleteFilterWarning"));
            }
        }

        /// <summary>
        /// Add the selected filter to the boolean expression.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addExecutingFilterButton_Click(object sender, EventArgs e)
        {
            m_filterMgr.BooleanOperate(BooleanOperator.Add);
            booleanExpressionTextBox.Text = m_filterMgr.BooleanExpression;
        }

        /// <summary>
        /// Add the selected filter to the boolean expression.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filtersListBox_DoubleClick(object sender, EventArgs e)
        {
            m_filterMgr.BooleanOperate(BooleanOperator.Add);
            booleanExpressionTextBox.Text = m_filterMgr.BooleanExpression;
        }

        /// <summary>
        /// Run the selected filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterButton_Click(object sender, EventArgs e)
        {
            if (null == m_filterMgr.SelectedFilterInfo)
            {
                MessageBox.Show(ResourceMgr.GetString("NoSelectedWarning"));
                return;
            }

            List<Element> elements = new List<Element>();
            m_commandData.Application.ActiveDocument.get_Elements(m_filterMgr.SelectedFilterInfo.Filter
                , elements);
            
            DisplayElementSet(elements);                 
        }

        /// <summary>
        /// List all elements retrieved in a new form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyFilterButton_Click(object sender, EventArgs e)
        {
            if (!m_filterMgr.CanBeExecuted)
            {
                MessageBox.Show(ResourceMgr.GetString("InvalidBooleanExpWarning"));
                return;
            }

            List<Element> elements = new List<Element>();
            m_commandData.Application.ActiveDocument.get_Elements(m_filterMgr.ToBeExecutedFilter
                , elements);
            
            DisplayElementSet(elements);
        }

        /// <summary>
        /// Change the selected filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filtersListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_filterMgr.SelectedFilterInfo = filtersListBox.SelectedValue as FilterInfo;
            if (null == m_filterMgr.SelectedFilterInfo)
            {
                filterButton.Enabled = false;
                filterButton.Text = ResourceMgr.GetString("Run");
                filterInfoLabel.Text = ResourceMgr.GetString("FilterInfo");
                return;
            }
            filterButton.Enabled = true;
            filterButton.Text = ResourceMgr.GetString("Run") + " " + m_filterMgr.SelectedFilterInfo.Name;
            tipMessageToolTip.RemoveAll();
            tipMessageToolTip.SetToolTip(filtersListBox, m_filterMgr.SelectedFilterInfo.TipMessage);
            filterInfoLabel.Text = m_filterMgr.SelectedFilterInfo.TipMessage;
        }

        /// <summary>
        /// Add an 'And' operator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void andButton_Click(object sender, EventArgs e)
        {
            m_filterMgr.BooleanOperate(BooleanOperator.And);
            booleanExpressionTextBox.Text = m_filterMgr.BooleanExpression;
        }

        /// <summary>
        /// Add a 'Or' operator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orButton_Click(object sender, EventArgs e)
        {
            m_filterMgr.BooleanOperate(BooleanOperator.Or);
            booleanExpressionTextBox.Text = m_filterMgr.BooleanExpression;
        }

        /// <summary>
        /// Add a 'not' operator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notButton_Click(object sender, EventArgs e)
        {
            m_filterMgr.BooleanOperate(BooleanOperator.Not);
            booleanExpressionTextBox.Text = m_filterMgr.BooleanExpression;
        }

        /// <summary>
        /// Empty the boolean expression.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reSetButton_Click(object sender, EventArgs e)
        {
            m_filterMgr.BooleanOperate(BooleanOperator.None);
            booleanExpressionTextBox.Text = m_filterMgr.BooleanExpression;
        }

        /// <summary>
        /// Display the elements.
        /// </summary>
        /// <param name="elements"></param>
        private void DisplayElementSet(ICollection<Element> elements)
        {
            try
            {
                DataTable elementSetTable = new DataTable();

                // Add three column objects to the table.
                DataColumn idColumn = new DataColumn();
                idColumn.DataType = System.Type.GetType("System.Int32");
                idColumn.ColumnName = ResourceMgr.GetString("Id");
                idColumn.AutoIncrement = true;
                elementSetTable.Columns.Add(idColumn);

                DataColumn nameColumn = new DataColumn();
                nameColumn.DataType = System.Type.GetType("System.String");
                nameColumn.ColumnName = ResourceMgr.GetString("Name");
                elementSetTable.Columns.Add(nameColumn);

                DataColumn typeColumn = new DataColumn();
                typeColumn.DataType = System.Type.GetType("System.String");
                typeColumn.ColumnName = ResourceMgr.GetString("Type");
                elementSetTable.Columns.Add(typeColumn);

                DataColumn categoryColumn = new DataColumn();
                categoryColumn.DataType = System.Type.GetType("System.String");
                categoryColumn.ColumnName = ResourceMgr.GetString("Category");
                elementSetTable.Columns.Add(categoryColumn);

                if (0 < elements.Count)
                {
                    using (ProcessForm dlg = new ProcessForm(elements, ref elementSetTable))
                    {
                        dlg.ShowDialog();
                    }
                }

                using (ElementSetForm elementsDlg = new ElementSetForm(elementSetTable))
                {
                    elementsDlg.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(ResourceMgr.GetString("FailedAcquiring"));
                return;
            }   
        }

        #region CategoryFilter tab page configure

        /// <summary>
        /// CategoryFilter tabpage: List category or builtInCategory determined this selected value change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_ModeComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_categoryFilterConfigure.Mode = (CategoryFilterMode)(m_ModeComboBox.SelectedItem);

            if (m_categoryFilterConfigure.Mode == CategoryFilterMode.BuiltInCategory)
            {
                m_categoryListBox.DataSource = m_categoryFilterConfigure.BuiltInCategories;
            }
            else
            {
                m_categoryListBox.DataSource = m_categoryFilterConfigure.Categories;
                m_categoryListBox.DisplayMember = "Name";
            }
        }

        /// <summary>
        /// Select value as criteria to filter elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_categoryListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((CategoryFilterMode)(m_ModeComboBox.SelectedValue) == CategoryFilterMode.BuiltInCategory)
            {
                m_categoryFilterConfigure.SelectedBuiltInCategory = (BuiltInCategory)(m_categoryListBox.SelectedItem);
                m_categoryFilterConfigure.SelectedCategory = null;
            }
            else
            {
                m_categoryFilterConfigure.SelectedCategory = m_categoryListBox.SelectedItem as Category;
                m_categoryFilterConfigure.SelectedBuiltInCategory = BuiltInCategory.INVALID;
            }
        }
        #endregion

        #region FamilyFilter tab page configure

        /// <summary>
        /// Initialize the family filter configure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void familyFilterTabPage_Enter(object sender, EventArgs e)
        {
            string text = this.Text;
            try
            {
                this.Text = ResourceMgr.GetString("Loading");
                m_familyFilterConfigure.Fill(m_commandData);
                if (null == familyNameListBox.DataSource)
                {
                    familyNameListBox.DataSource = m_familyFilterConfigure.FamilyNames;
                    familyNameListBox.DisplayMember = "FriendlyName";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                this.Text = text;
            }
        }  

        /// <summary>
        /// Change a family to select.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void familyNameListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_familyFilterConfigure.SelectedName = familyNameListBox.SelectedItem as FriendlyFamilyName;
        }
        #endregion

        #region SymbolFilter tab page configure

        /// <summary>
        /// Initialize the symbol filter configure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void symbolFilterTabPage_Enter(object sender, EventArgs e)
        {
            string text = this.Text;
            try
            {
                this.Text = ResourceMgr.GetString("Loading");
                m_symbolFilterConfigure.Fill(m_commandData);
                if (null == symbolListBox.DataSource)
                {
                    symbolListBox.DataSource = m_symbolFilterConfigure.SymbolNames;
                    symbolListBox.DisplayMember = "FriendlyName";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                this.Text = text;
            }
        }

        /// <summary>
        /// Change a symbol to select.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void symbolListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_symbolFilterConfigure.SelectedSymbolName = symbolListBox.SelectedItem as FriendlySymbolName;
        }
        #endregion

        #region TypeFilter tab page configure

        /// <summary>
        /// Initialize the type filter configure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void typeFilterTabPage_Enter(object sender, EventArgs e)
        {
            string text = this.Text;
            try
            {
                this.Text = ResourceMgr.GetString("Loading");
                m_typeFilterConfigure.Fill(m_commandData);
                if (null == typeListBox.DataSource)
                {
                    typeListBox.DataSource = m_typeFilterConfigure.RevitAPITypes;
                    typeListBox.DisplayMember = "FullName";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                this.Text = text;
            }
        }  

        /// <summary>
        /// Change a type to select.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void typeListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_typeFilterConfigure.SelectedType = typeListBox.SelectedItem as Type;
        }
        #endregion

        #region ParameterFilter tab page configure

        /// <summary>
        /// Initialize the parameter filter configure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parameterFilterTabPage_Enter(object sender, EventArgs e)
        {
            string text = this.Text;
            try
            {
                this.Text = ResourceMgr.GetString("Loading");
                if (null == m_parameterFilterConfigure)
                    return;
                m_parameterFilterConfigure.Fill(m_commandData);
                if (null == builtInParasListBox.DataSource)
                {
                    builtInParasListBox.DataSource = m_parameterFilterConfigure.BuiltInParas;
                }
                if (null == criteriaListBox.DataSource)
                {
                    criteriaListBox.DataSource = m_parameterFilterConfigure.CriteriaFilterTypes;
                    criteriaListBox.SelectedIndex = -1;
                }
                if (null == valueTypeComboBox.DataSource)
                {
                    valueTypeComboBox.DataSource = m_parameterFilterConfigure.StorageTypes;
                }

                m_isInitialized = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                this.Text = text;
            }
        }

        /// <summary>
        /// Change a built-in parameter to select.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void builtInParasListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == builtInParasListBox.SelectedItem)
            {
                return;
            }
            string strRep = builtInParasListBox.SelectedItem as string ;
            BuiltInParameter bp = (BuiltInParameter)(Enum.Parse(typeof(BuiltInParameter), strRep));
            m_parameterFilterConfigure.SelectedBuiltInPara = bp;
            object valueType;
            StorageType st;
            try
            {
                st = (StorageType)m_commandData.Application.ActiveDocument.get_TypeOfStorage(bp);
            }
            catch (InvalidOperationException)
            {
                st = StorageType.None;
            }
            valueType = (object)st;

            if (null != valueType)
            {
                valueTypeComboBox.SelectedItem = valueType;
            }

            List<CriteriaFilterType> availableCFT = new List<CriteriaFilterType>();
            foreach (object value in Enum.GetValues(typeof(CriteriaFilterType)))
            {
                CriteriaFilterType enumVlaue = (CriteriaFilterType)(value);
                bool isSupported = m_commandData.Application.ActiveDocument.get_FilterTypeSupported(bp, enumVlaue);
                if (isSupported)
                {
                    availableCFT.Add(enumVlaue);
                }
            }

            criteriaListBox.DataSource = availableCFT;
            if (0 == availableCFT.Count)
            {
                valueTextBox.Enabled = false;
            }
            else
            {
                valueTextBox.Enabled = true;
            }

        }

        /// <summary>
        /// Select a criterion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void criteriaListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (null == builtInParasListBox.SelectedItem
                || null == criteriaListBox.SelectedItem
                || !m_isInitialized)
            {
                return;
            }

            m_parameterFilterConfigure.SelectedCriteriaFilterType = criteriaListBox.SelectedItem;
        }

        /// <summary>
        /// Select a type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void valueTypeComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_parameterFilterConfigure.SelectedStorageType = valueTypeComboBox.SelectedItem;
            valueTextBox.Text = null;
        }

        /// <summary>
        /// Check the input text is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            string checkResult = CheckInputValue(valueTextBox.Text);
            if (null == checkResult)
            {
                m_parameterFilterConfigure.InputValue = valueTextBox.Text;
            }
            else
            {
                MessageBox.Show(checkResult, ResourceMgr.GetString("InputError"));
                valueTextBox.Text = m_parameterFilterConfigure.InputValue;
            }
        }

        /// <summary>
        /// Check the input string is valid, or else, return the old string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string CheckInputValue(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            switch ((Autodesk.Revit.Parameters.StorageType)m_parameterFilterConfigure.SelectedStorageType)
            {
                case Autodesk.Revit.Parameters.StorageType.Double:
                    {
                        double doubleValue;
                        if (double.TryParse(str, out doubleValue))
                            return null;
                        return ResourceMgr.GetString("DoubleInputMessage");
                    }
                case Autodesk.Revit.Parameters.StorageType.ElementId:
                case Autodesk.Revit.Parameters.StorageType.Integer:
                    {
                        Int32 intValue;
                        if (Int32.TryParse(str, out intValue))
                            return null;
                        return ResourceMgr.GetString("IntInputMessage");
                    }
                case Autodesk.Revit.Parameters.StorageType.String:
                    {
                        return null;
                    }
                default:
                    break;
            }

            return ResourceMgr.GetString("InputInvalid");
        }
        #endregion

        #region StructureFilter tab page configure

        /// <summary>
        /// Initialize the structure filter configure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void structureFilterTabPage_Enter(object sender, EventArgs e)
        {
            string text = this.Text;
            try
            {
                this.Text = ResourceMgr.GetString("Loading");
                m_structureFilterConfigure.Fill(m_commandData);
                if (null == classificationComboBox.DataSource)
                {
                    classificationComboBox.DataSource = m_structureFilterConfigure.ClassificationFilters;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                this.Text = text;
            }
        }
        
        /// <summary>
        /// Select a structure classification.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void classificationComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_structureFilterConfigure.SelectedClassificationFilter = classificationComboBox.SelectedItem;
            structureEnumListBox.DataSource = null;
            switch ((ClassificationFilter)m_structureFilterConfigure.SelectedClassificationFilter)
            {
                case ClassificationFilter.InstanceUsage:
                    structureEnumListBox.DataSource = m_structureFilterConfigure.InstanceUsages;
                    break;
                case ClassificationFilter.Material:
                    structureEnumListBox.DataSource = m_structureFilterConfigure.Materials;
                    break;
                case ClassificationFilter.StructuralType:
                    structureEnumListBox.DataSource = m_structureFilterConfigure.StructuralTypes;
                    break;
                case ClassificationFilter.WallUsage:
                    structureEnumListBox.DataSource = m_structureFilterConfigure.WallUsages;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Select a value for the new filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void structureEnumListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (null == structureEnumListBox.SelectedItem)
                return;

            switch ((ClassificationFilter)m_structureFilterConfigure.SelectedClassificationFilter)
            {
                case ClassificationFilter.InstanceUsage:
                    m_structureFilterConfigure.SelectedInstanceUsage = structureEnumListBox.SelectedItem;
                    break;
                case ClassificationFilter.Material:
                    m_structureFilterConfigure.SelectedMaterial = structureEnumListBox.SelectedItem;
                    break;
                case ClassificationFilter.StructuralType:
                    m_structureFilterConfigure.SelectedStructuralType = structureEnumListBox.SelectedItem;
                    break;
                case ClassificationFilter.WallUsage:
                    m_structureFilterConfigure.SelectedWallUsage = structureEnumListBox.SelectedItem;
                    break;
                default:
                    break;
            }
        }
        #endregion        
    }
}
