//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Data;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.ViewFilters.CS
{
    /// <summary>
    /// UI form to display the view filters information
    /// Some controls provide interfaces to create or modify filters and rules.
    /// </summary>
    public partial class ViewFiltersForm : System.Windows.Forms.Form
    {
        #region Class Member Variables
        /// <summary>
        /// Revit active document
        /// </summary>
        Autodesk.Revit.DB.Document m_doc;

        /// <summary>
        /// Dictionary of filter and its filter data(categories, filter rules)
        /// </summary>
        Dictionary<String, FilterData> m_dictFilters = new Dictionary<String, FilterData>();

        /// <summary>
        /// Current filter data maps active Revit filter.
        /// </summary>
        FilterData m_currentFilterData;

        /// <summary>
        /// Indicates if categories were changed by programming, 
        /// It's used to suppress ItemCheck event for Categories controls
        /// </summary>
        bool m_catChangedEventSuppress;

        /// <summary>
        /// Const name for invalid parameter 
        /// </summary>
        const String m_noneParam = "(none)";

        /// <summary>
        /// Sample custom rule name prefix, filter rules will be displayed with this name + #(1, 2, ...)
        /// </summary>
        const String m_ruleNamePrefix = "Filter Rule ";
        #endregion

        /// <summary>
        /// Overload the constructor
        /// </summary>
        /// <param name="commandData">an instance of Data class</param>
        public ViewFiltersForm(Autodesk.Revit.UI.ExternalCommandData commandData)
        {
            InitializeComponent();
            m_doc = commandData.Application.ActiveUIDocument.Document;
        }

        /// <summary>
        /// When the form was loaded, display the information of existing filters 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewFiltersForm_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            // Get all existing filters and initialize class members
            InitializeFilterData();
            //
            // Get all applicable categories and fill in category CheckListBox
            AddAppliableCategories();
            //
            // Set 1st item selected, then track according event
            if (filtersListBox.Items.Count > 0)
                filtersListBox.SetSelected(0, true);  // set first item to be selected to raise control event
            else
                ResetControls_NoFilter();
            this.ResumeLayout();
        }

        /// <summary>
        /// Select one filter, it will reset categories and accordingly rules.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filtersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if no item yet do nothing
            if (filtersListBox.Items.Count == 0 || filtersListBox.SelectedItems.Count == 0)
                return;
            //
            // Get current selected filter, use 1st item because only one item can be selected for control.
            String filterName = filtersListBox.SelectedItems[0] as String;
            if (!m_dictFilters.TryGetValue(filterName, out m_currentFilterData))
                return;
            //
            // Reset categoryCheckedListBox:
            // Show all categories of document, also check categories belong to this filter
            m_catChangedEventSuppress = true; // suppress some events when checking categories during reset 
            ResetCategoriesControl(false); // don't need to re-add all items
            m_catChangedEventSuppress = false;
            //
            // Initialize all supported parameters for selected categories
            ResetRule_CategoriesChanged();
        }

        /// <summary>
        /// Create new filter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newButton_Click(object sender, EventArgs e)
        {
            // Show new name for filter, unique name is required for creating filters
            List<String> inUseKeys = new List<string>();
            inUseKeys.AddRange(m_dictFilters.Keys);
            NewFilterForm newForm = new NewFilterForm(inUseKeys);
            DialogResult result = newForm.ShowDialog();
            if (result != DialogResult.OK)
                return;
            //
            // Create new filter data now(the filter data will be reflected to Revit filter when Ok button is clicked).
            String newFilterName = newForm.FilterName;
            m_currentFilterData = new FilterData(m_doc, new List<BuiltInCategory>(), new List<FilterRuleBuilder>());
            m_dictFilters.Add(newFilterName, m_currentFilterData);
            filtersListBox.Items.Add(newFilterName);
            filtersListBox.SetSelected(filtersListBox.Items.Count - 1, true);
            ResetControls_HasFilter();
        }

        /// <summary>
        /// Delete one filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            // delete current selected filter
            if (filtersListBox.Items.Count == 0 || filtersListBox.SelectedItems.Count == 0)
                return;
            // 
            // Remove selected items
            String curFilter = filtersListBox.Items[filtersListBox.SelectedIndex] as String;
            m_dictFilters.Remove(curFilter);
            filtersListBox.Items.Remove(curFilter);
            if (filtersListBox.Items.Count > 0)
                filtersListBox.SetSelected(0, true);
            else
                ResetControls_NoFilter();
        }

        /// <summary>
        /// Categories for filter were changed, clean all old filter rules.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void categoryCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // if item is checked by programming, suppress this event
            if (m_catChangedEventSuppress)
                return;
            //
            // Get all selected categories, include one which is going to be checked
            List<BuiltInCategory> selCats = new List<BuiltInCategory>();
            List<ElementId> selCatIds = new List<ElementId>();
            int itemCount = categoryCheckedListBox.Items.Count;
            for (int ii = 0; ii < itemCount; ii++)
            {
                // Skip current check item if it's going to be unchecked;
                // add current check item it's going to be checked
                bool addItemToChecked = false;
                if (null != e && e.Index == ii)
                {
                    addItemToChecked = (e.NewValue == CheckState.Checked);
                    if (!addItemToChecked)
                        continue;
                }
                //
                // add item checked and item is going to be checked
                if (addItemToChecked || categoryCheckedListBox.GetItemChecked(ii))
                {
                    String curCat = categoryCheckedListBox.GetItemText(categoryCheckedListBox.Items[ii]);
                    BuiltInCategory param = EnumParseUtility<BuiltInCategory>.Parse(curCat);
                    selCats.Add(param);
                    selCatIds.Add(new ElementId(param));
                }
            }
            //
            // Reset accordingly controls
            bool changed = m_currentFilterData.SetNewCategories(selCatIds);
            if (!changed)
                return;
            //
            // Update rules controls
            ResetRule_CategoriesChanged();
        }

        /// <summary>
        /// Check all items of categories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAllButton_Click(object sender, EventArgs e)
        {
            m_catChangedEventSuppress = true;
            for (int ii = 0; ii < categoryCheckedListBox.Items.Count; ii++)
            {
                categoryCheckedListBox.SetItemChecked(ii, true);
            }
            m_catChangedEventSuppress = false;
            //
            // force call event handler to update accordingly
            categoryCheckedListBox_ItemCheck(null, null);
        }

        /// <summary>
        /// Cancel check for all items of categories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkNoneButton_Click(object sender, EventArgs e)
        {
            m_catChangedEventSuppress = true;
            for (int ii = 0; ii < categoryCheckedListBox.Items.Count; ii++)
            {
                categoryCheckedListBox.SetItemChecked(ii, false);
            }
            m_catChangedEventSuppress = false;
            categoryCheckedListBox_ItemCheck(null, null);
        }

        /// <summary>
        /// Hide or show un-checked categories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hideUnCheckCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!HasFilterData()) return;

            // reset categories controls, re-add all applicable categories if needed
            ResetCategoriesControl(true);
            //
            // Call event handler to refresh filter rules
            categoryCheckedListBox_ItemCheck(null, null);
        }

        /// <summary>
        /// Select one filter rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rulesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // do nothing if no any filter yet
            if (!HasFilterData())
                return;
            //
            // Change parameter for selected filter rule
            FilterRuleBuilder currentRule = GetCurrentRuleData();
            if (null == currentRule) return;
            String paramName = EnumParseUtility<BuiltInParameter>.Parse(currentRule.Parameter);
            paramerComboBox.SelectedItem = paramName;
        }

        /// <summary>
        /// Select parameter for current filter rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paramerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get criteria for selected parameter and reset rule criteria and rule values
            String selectItem = paramerComboBox.SelectedItem.ToString();
            bool isNone = (selectItem == m_noneParam);
            criteriaComboBox.Enabled = ruleValueComboBox.Enabled = newRuleButton.Enabled = !isNone;
            if (isNone)// is (none) selected
            {
                ResetControlByParamType(StorageType.None);
                return;
            }
            //
            // Check to see if this parameter is in use:
            // Switch to this parameter if parameter is in use, and reset controls with criteria and value for this parameter.
            BuiltInParameter curParam = EnumParseUtility<BuiltInParameter>.Parse(selectItem);
            StorageType paramType = m_doc.get_TypeOfStorage(curParam);
            ResetControlByParamType(paramType);
            ParameterInUse(curParam);
            //
            // New parameter was selected, reset controls with new criteria
            ICollection<String> possibleCritias = RuleCriteraNames.Criterions(paramType);
            criteriaComboBox.Items.Clear();
            foreach (String criteria in possibleCritias)
            {
                criteriaComboBox.Items.Add(criteria);
            }
            // 
            // Display parameter values for current filter rule, 
            // If current selected parameter equal to current filter rule's, reset controls with rule data
            FilterRuleBuilder currentRule = GetCurrentRuleData();
            if (null != currentRule && currentRule.Parameter == curParam)
            {
                criteriaComboBox.SelectedItem = currentRule.RuleCriteria;
                ruleValueComboBox.Text = currentRule.RuleValue;
                caseSensitiveCheckBox.Checked = currentRule.CaseSensitive;
                epsilonTextBox.Text = String.Format("{0:N6}", currentRule.Epsilon);
            }
            else
            {
                // set with default value
                criteriaComboBox.SelectedIndex = 0;
                ruleValueComboBox.Text = String.Empty;
                caseSensitiveCheckBox.Checked = false;
                epsilonTextBox.Text = "0.0";
            }
        }

        /// <summary>
        /// Create one new rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newRuleButton_Click(object sender, EventArgs e)
        {
            if (!HasFilterData() || paramerComboBox.SelectedText == m_noneParam)
                return;
            //
            // check if rule value is specified or exist
            BuiltInParameter curParam = EnumParseUtility<BuiltInParameter>.Parse(paramerComboBox.SelectedItem as String);
            if (ParameterAlreadyExist(curParam))
            {
                MyMessageBox("Filter rule for this parameter already exists, no sense to add new rule for this parameter again.");
                return;
            }
            FilterRuleBuilder newRule = CreateNewFilterRule(curParam);
            if (null == newRule)
                return;
            // 
            // Create and reserve this rule and reset controls
            m_currentFilterData.RuleData.Add(newRule);
            String ruleName = String.Format(String.Format("{0} {1}", m_ruleNamePrefix, rulesListBox.Items.Count + 1));
            rulesListBox.Items.Add(ruleName);
            rulesListBox.SelectedIndex = rulesListBox.Items.Count - 1;
            rulesListBox.Enabled = true;
            ViewFiltersForm_MouseMove(null, null);
        }

        /// <summary>
        /// Delete some filter rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleRuleButton_Click(object sender, EventArgs e)
        {
            if (rulesListBox.SelectedItems.Count == 0)
            {
                MyMessageBox("Please select filter rule you want to delete.");
                return;
            }
            //
            // Remove the selected item and set the 1st item to be selected auto
            int ruleIndex = rulesListBox.SelectedIndex;
            m_currentFilterData.RuleData.RemoveAt(ruleIndex);
            rulesListBox.Items.RemoveAt(ruleIndex);
            if (rulesListBox.Items.Count > 0)
                rulesListBox.SetSelected(0, true);
        }

        /// <summary>
        /// Update change rule criteria and values for current filter rule
        /// Filter criteria and values won't be changed until you click this button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateButton_Click(object sender, EventArgs e)
        {
            FilterRuleBuilder currentRule = GetCurrentRuleData();
            if (null == currentRule) return;
            BuiltInParameter selParam = currentRule.Parameter;
            FilterRuleBuilder newRule = CreateNewFilterRule(selParam);
            if (null == newRule) return;
            int oldRuleIndex = GetCurrentRuleDataIndex();
            if (oldRuleIndex > m_currentFilterData.RuleData.Count) return;
            //
            // Update rule value
            m_currentFilterData.RuleData[oldRuleIndex] = newRule;
        }

        /// <summary>
        /// Enable or disable some buttons according to current status/values of controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewFiltersForm_MouseMove(object sender, MouseEventArgs e)
        {
            // Enable and disable some buttons:
            // . If current selected parameter in rulesListBox equals to parameter in Parameter ComboBox:
            // Update/Delete is allowed; Otherwise, New and delete is allowed.
            // . If rulesListBox has no any rule, only New button is allowed
            FilterRuleBuilder currentRule = GetCurrentRuleData();
            if (null == currentRule)
            {
                updateButton.Enabled = deleRuleButton.Enabled = false;
                if (!HasFilterData() || null == paramerComboBox.SelectedItem ||
                    (paramerComboBox.SelectedItem as String) == m_noneParam)
                    newRuleButton.Enabled = false;
                else
                    newRuleButton.Enabled = true;
            }
            else
            {
                BuiltInParameter selParam = currentRule.Parameter;
                BuiltInParameter selComboxParam = EnumParseUtility<BuiltInParameter>.Parse(paramerComboBox.SelectedItem as String);
                bool paramEquals = (selParam == selComboxParam);
                //
                // new button is available only when user select new parameter
                newRuleButton.Enabled = !paramEquals;
                updateButton.Enabled = deleRuleButton.Enabled = paramEquals;
            }
        }

        /// <summary>
        /// Update ParameterFilterElements in the Revit document with data from the dialog box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      private void okButton_Click(object sender, EventArgs e)
        {
            // Reserve how many Revit filters need to be updated/removed
            ICollection<String> updatedFilters = new List<String>();
            ICollection<ElementId> deleteElemIds = new List<ElementId>();
            FilteredElementCollector collector = new FilteredElementCollector(m_doc);
            ICollection<Element> oldFilters = collector.OfClass(typeof(ParameterFilterElement)).ToElements();
            //
            // Start transaction to update filters now
            Transaction tran = new Transaction(m_doc, "Update View Filter");
            tran.Start();
            try
            {
                // 1. Update existing filters
                foreach (ParameterFilterElement filter in oldFilters)
                {
                    FilterData filterData;
                    bool bExist = m_dictFilters.TryGetValue(filter.Name, out filterData);
                    if (!bExist)
                    {
                        deleteElemIds.Add(filter.Id);
                        continue;
                    }
                    //
                    // Update Filter categories for this filter
                    ICollection<ElementId> newCatIds = filterData.GetCategoryIds();
                    if (!ListCompareUtility<ElementId>.Equals(filter.GetCategories(), newCatIds))
                    {
                        filter.SetCategories(newCatIds);
                    }

                    // Update filter rules for this filter
                    IList<FilterRule> newRules = new List<FilterRule>();
                    foreach (FilterRuleBuilder ruleData in filterData.RuleData)
                    {
                        newRules.Add(ruleData.AsFilterRule());
                    }

                    ElementFilter elemFilter = FiltersUtil.CreateElementFilterFromFilterRules(newRules);
                    // Set this filter's list of rules.
                    filter.SetElementFilter(elemFilter);

                    // Remember that we updated this filter so that we do not try to create it again below.
                    updatedFilters.Add(filter.Name);
                }
                //
                // 2. Delete some filters
                if (deleteElemIds.Count > 0)
                    m_doc.Delete(deleteElemIds);
                //
                // 3. Create new filters(if have)
                foreach (KeyValuePair<String, FilterData> myFilter in m_dictFilters)
                {
                    // If this filter was updated in the previous step, do nothing.
                    if (updatedFilters.Contains(myFilter.Key))
                        continue;

                    // Create a new filter.
                    // Collect the FilterRules, create an ElementFilter representing the
                    // conjunction ("ANDing together") of the FilterRules, and use the ElementFilter
                    // to create a ParameterFilterElement
                    IList<FilterRule> rules = new List<FilterRule>();
                    foreach (FilterRuleBuilder ruleData in myFilter.Value.RuleData)
                    {
                        rules.Add(ruleData.AsFilterRule());
                    }
                    ElementFilter elemFilter = FiltersUtil.CreateElementFilterFromFilterRules(rules);

                    // Check that the ElementFilter is valid for use by a ParameterFilterElement.
                    IList<ElementId> categoryIdList = myFilter.Value.GetCategoryIds();
                    ISet<ElementId> categoryIdSet = new HashSet<ElementId>(categoryIdList);
                    if (!ParameterFilterElement.ElementFilterIsAcceptableForParameterFilterElement(
                       m_doc, categoryIdSet, elemFilter))
                    {
                       // In case the UI allowed invalid rules, issue a warning to the user.
                       MyMessageBox("The combination of filter rules is not acceptable for a View Filter.");
                    }
                    else
                    {
                       ParameterFilterElement.Create(m_doc, myFilter.Key, categoryIdSet, elemFilter);
                    }
                }
                // 
                // Commit change now
                tran.Commit();
            }
            catch (Exception ex)
            {
                String failMsg = String.Format("Revit filters update failed and was aborted: " + ex.ToString());
                MyMessageBox(failMsg);
                tran.RollBack();
            }
        }

        /// <summary>
        /// Dynamically adjust the width of parameter so that all parameters can be visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paramerComboBox_DropDown(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            foreach (string s in ((ComboBox)sender).Items)
            {
                int newWidth = (int)g.MeasureString(s, font).Width
                    + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }
            senderComboBox.DropDownWidth = width;
        }

        #region Class Implementations
        /// <summary>
        /// Initialize filter data with existing view filters
        /// </summary>
        void InitializeFilterData()
        {
            // Get all existing filters
            ICollection<ParameterFilterElement> filters = FiltersUtil.GetViewFilters(m_doc);
            foreach (ParameterFilterElement filter in filters)
            {
                // Get all data of the current filter and create my FilterData
                ICollection<ElementId> catIds = filter.GetCategories();

                // Get the ElementFilter representing the set of FilterRules.
                ElementFilter elemFilter = filter.GetElementFilter();
                // Check that the ElementFilter represents a conjunction of ElementFilters.
                // We will then check that each child ElementFilter contains just one FilterRule.
                IList<FilterRule> filterRules = FiltersUtil.GetConjunctionOfFilterRulesFromElementFilter(elemFilter);
                int numFilterRules = filterRules.Count;
                if (0 == numFilterRules)
                   return; // Error
                
                // Create filter rule data now 
                List<FilterRuleBuilder> ruleDataSet = new List<FilterRuleBuilder>();
                foreach (FilterRule filterRule in filterRules)
                {
                   ElementId paramId = filterRule.GetRuleParameter();
                   int parameterIdAsInt = paramId.IntegerValue;
                   BuiltInParameter bip = (BuiltInParameter)parameterIdAsInt;
                   FilterRuleBuilder ruleData = FiltersUtil.CreateFilterRuleBuilder(bip, filterRule);
                   ruleDataSet.Add(ruleData);
                }
                
                // 
                // Create Filter data
                FilterData filterData = new FilterData(m_doc, catIds, ruleDataSet);
                m_dictFilters.Add(filter.Name, filterData);
                //
                // also add to control 
                filtersListBox.Items.Add(filter.Name);
            }
        }

        /// <summary>
        /// Check if some filter exists 
        /// </summary>
        /// <returns>True if filter already exists, otherwise false.</returns>
        bool HasFilterData()
        {
            return (null != m_currentFilterData);
        }

        /// <summary>
        /// This method will reset Category check list box with all applicable categories with document
        /// </summary>
        void AddAppliableCategories()
        {
            categoryCheckedListBox.Items.Clear();
            ICollection<ElementId> filterCatIds = ParameterFilterUtilities.GetAllFilterableCategories();
            foreach (ElementId id in filterCatIds)
            {
                categoryCheckedListBox.Items.Add(EnumParseUtility<BuiltInCategory>.Parse((BuiltInCategory)id.IntegerValue));
            }
        }

        /// <summary>
        /// Reset categories CheckListBox control
        /// </summary>
        /// <param name="reAddAll">Indicates if it's needed to reset control with all applicable categories.</param>
        void ResetCategoriesControl(bool reAddAll)
        {
            m_catChangedEventSuppress = true; // suppress some events when checking categories during reset 
            ICollection<BuiltInCategory> filterCat = m_currentFilterData.FilterCategories;
            if (hideUnCheckCheckBox.Checked)
            {
                categoryCheckedListBox.Items.Clear();
                foreach (BuiltInCategory cat in filterCat)
                {
                    String newCat = EnumParseUtility<BuiltInCategory>.Parse(cat);
                    categoryCheckedListBox.Items.Add(newCat, true);
                }
            }
            else
            {
                // Add all items firstly if needed, happen when user plan to hide none
                if (reAddAll)
                    AddAppliableCategories();
                //
                // Check those categories of current filter
                for (int ii = 0; ii < categoryCheckedListBox.Items.Count; ii++)
                {
                    // set all to unchecked firstly
                    categoryCheckedListBox.SetItemChecked(ii, false);
                    String catName = categoryCheckedListBox.Items[ii] as String;
                    BuiltInCategory curCat = EnumParseUtility<BuiltInCategory>.Parse(catName);
                    if (filterCat.Contains(curCat))
                        categoryCheckedListBox.SetItemChecked(ii, true);
                }
            }
            m_catChangedEventSuppress = false;
        }

        /// <summary>
        /// Reset controls when no any filter
        /// </summary>
        void ResetControls_NoFilter()
        {
            // uncheck categories, disable/hide controls
            categoryCheckedListBox.Enabled = false;
            for (int ii = 0; ii < categoryCheckedListBox.Items.Count; ii++)
                categoryCheckedListBox.SetItemChecked(ii, false);
            checkAllButton.Enabled = false;
            checkNoneButton.Enabled = false;
            rulesListBox.Enabled = false;
            paramerComboBox.Enabled = false;
            criteriaComboBox.Enabled = false;
            ruleValueComboBox.Enabled = false;
            caseSensitiveCheckBox.Visible = false;
            epsilonLabel.Visible = false;
            epsilonTextBox.Visible = false;
            newRuleButton.Enabled = false;
            deleRuleButton.Enabled = false;
            updateButton.Enabled = false;

        }

        /// <summary>
        /// Update controls when has filter
        /// </summary>
        void ResetControls_HasFilter()
        {
            categoryCheckedListBox.Enabled = true;
            checkAllButton.Enabled = true;
            checkNoneButton.Enabled = true;
            paramerComboBox.Enabled = true;
        }

        /// <summary>
        /// Update control when no filter rule
        /// </summary>
        void ResetControls_NoFilterRule()
        {
            criteriaComboBox.Enabled = false;
            ruleValueComboBox.Enabled = false;
            caseSensitiveCheckBox.Enabled = false;
            newRuleButton.Enabled = false;
            deleRuleButton.Enabled = false;
        }

        /// <summary>
        /// Reset rules because categories were changed
        /// </summary>
        void ResetRule_CategoriesChanged()
        {
            // Initialize all supported parameters for selected categories
            ICollection<BuiltInCategory> filterCat = m_currentFilterData.FilterCategories;
            ICollection<ElementId> filterCatIds = new List<ElementId>();
            foreach (BuiltInCategory curCat in filterCat)
            {
                filterCatIds.Add(new ElementId(curCat));
            }
            ICollection<ElementId> supportedParams =
                ParameterFilterUtilities.GetFilterableParametersInCommon(m_doc, filterCatIds);
            ResetParameterCombox(supportedParams);
            //
            // Reset filter rules controls and select 1st by default(if have)
            rulesListBox.Items.Clear();
            List<FilterRuleBuilder> ruleData = m_currentFilterData.RuleData;
            for (int ii = 1; ii <= ruleData.Count; ii++)
            {
                rulesListBox.Items.Add(m_ruleNamePrefix + ii);
            }
            if (rulesListBox.Items.Count > 0)
                rulesListBox.SetSelected(0, true);
            else
                ResetControls_NoFilterRule();
        }

        /// <summary>
        /// Update controls' status and visibility according to storage type of current parameter
        /// </summary>
        /// <param name="paramType"></param>
        void ResetControlByParamType(StorageType paramType)
        {
            if (paramType == StorageType.String)
            {
                caseSensitiveCheckBox.Visible = true;
                caseSensitiveCheckBox.Enabled = true;
                epsilonLabel.Visible = epsilonTextBox.Visible = false;
            }
            else if (paramType == StorageType.Double)
            {
                caseSensitiveCheckBox.Visible = false;
                epsilonLabel.Visible = epsilonTextBox.Visible = true;
                epsilonLabel.Enabled = epsilonTextBox.Enabled = true;
            }
            else
            {
                caseSensitiveCheckBox.Visible = false;
                epsilonLabel.Visible = epsilonTextBox.Visible = false;
            }
        }

        /// <summary>
        /// Check to see if rule for this parameter exists or not
        /// </summary>
        /// <param name="param">Parameter to be checked.</param>
        /// <returns>True if this parameter already has filter rule, otherwise false.</returns>
        bool ParameterAlreadyExist(BuiltInParameter param)
        {
            if (m_currentFilterData == null || m_currentFilterData.RuleData.Count == 0)
                return false;
            for (int ruleNo = 0; ruleNo < m_currentFilterData.RuleData.Count; ruleNo++)
            {
                if (m_currentFilterData.RuleData[ruleNo].Parameter == param)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get FilterRuleBuilder for current filter
        /// </summary>
        /// <returns></returns>
        FilterRuleBuilder GetCurrentRuleData()
        {
            if (!HasFilterData()) return null;

            int ruleIndex = GetCurrentRuleDataIndex();
            //
            // Se current selected parameters
            if (ruleIndex >= 0)
                return m_currentFilterData.RuleData[ruleIndex];
            else
                return null;
        }

        /// <summary>
        /// Get index for selected filter rule
        /// </summary>
        /// <returns>Index of current rule.</returns>
        int GetCurrentRuleDataIndex()
        {
            int ruleIndex = -1;
            for (int ii = 0; ii < rulesListBox.Items.Count; ii++)
            {
                if (rulesListBox.GetSelected(ii))
                {
                    ruleIndex = ii;
                    break;
                }
            }
            return ruleIndex;
        }

        /// <summary>
        /// Reset paramerComboBox with new parameters 
        /// </summary>
        /// <param name="paramSet"></param>
        void ResetParameterCombox(ICollection<ElementId> paramSet)
        {
            paramerComboBox.Items.Clear();
            foreach (ElementId paramId in paramSet)
            {
                paramerComboBox.Items.Add(EnumParseUtility<BuiltInParameter>.Parse(paramId.IntegerValue));
            }
            //
            // always added one (none) 
            paramerComboBox.Items.Add(m_noneParam);
            paramerComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Check if selected parameter already in use(has criteria)
        /// </summary>
        /// <param name="selParameter">Parameter to be checked.</param>
        /// <returns>True if this parameter already has criteria, otherwise false.</returns>
        bool ParameterInUse(BuiltInParameter selParameter)
        {
            if (!HasFilterData() || m_currentFilterData.RuleData.Count == 0
                || rulesListBox.Items.Count == 0)
                return false;
            //
            // Get all existing rules and check if this parameter is in used
            int paramIndex = 0;
            bool paramIsInUse = false;
            ICollection<FilterRuleBuilder> rules = m_currentFilterData.RuleData;
            foreach (FilterRuleBuilder rule in rules)
            {
                if (rule.Parameter == selParameter)
                {
                    paramIsInUse = true;
                    break;
                }
                paramIndex++;
            }
            //
            // If parameter is in use, switch to this parameter and update criteria and rules
            if (paramIsInUse)
                rulesListBox.SetSelected(paramIndex, true);
            return paramIsInUse;
        }

        /// <summary>
        /// Create new FilterRuleBuilder for current parameter
        /// </summary>
        /// <param name="curParam">Current selected parameter.</param>
        /// <returns>New FilterRuleBuilder for this parameter, null if parameter is not recognizable.</returns>
        FilterRuleBuilder CreateNewFilterRule(BuiltInParameter curParam)
        {
            StorageType paramType = m_doc.get_TypeOfStorage(curParam);
            String criteria = criteriaComboBox.SelectedItem as String;
            if (paramType == StorageType.String)
            {
                return new FilterRuleBuilder(curParam, criteria,
                    ruleValueComboBox.Text, caseSensitiveCheckBox.Checked);
            }
            else if (paramType == StorageType.Double)
            {
                double ruleValue = 0, epsilon = 0;
                if (!GetRuleValueDouble(false, ref ruleValue)) return null;
                if (!GetRuleValueDouble(true, ref epsilon)) return null;
                return new FilterRuleBuilder(curParam, criteria,
                    ruleValue, epsilon);
            }
            else if (paramType == StorageType.Integer)
            {
                int ruleValue = 0;
                if (!GetRuleValueInt(ref ruleValue)) return null;
                return new FilterRuleBuilder(curParam, criteria, ruleValue);
            }
            else if (paramType == StorageType.ElementId)
            {
                int ruleValue = 0;
                if (!GetRuleValueInt(ref ruleValue)) return null;
                return new FilterRuleBuilder(curParam, criteria, new ElementId(ruleValue));
            }
            else
                return null;
        }

        /// <summary>
        /// Get rule value of integer type
        /// </summary>
        /// <param name="ruleValue">Integer rule value.</param>
        /// <returns>True if control's text is valid int rule value.</returns>
        bool GetRuleValueInt(ref int ruleValue)
        {
            try
            {
                ruleValue = int.Parse(ruleValueComboBox.Text);
            }
            catch (System.Exception)
            {
                MyMessageBox("Rule value is wrong, please input valid value.");
                ruleValueComboBox.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get rule value of double type from control
        /// </summary>
        /// <param name="isEpsilon">Indicate if method will get value from epsilon control.</param>
        /// <param name="ruleValue">Integer rule value.</param>
        /// <returns>True if control's text is valid int rule value.</returns>
        bool GetRuleValueDouble(bool isEpsilon, ref double ruleValue)
        {
            try
            {
                if (isEpsilon)
                    ruleValue = double.Parse(epsilonTextBox.Text);
                else
                    ruleValue = double.Parse(ruleValueComboBox.Text);
            }
            catch (System.Exception)
            {
                if (isEpsilon)
                {
                    MyMessageBox("Epsilon value is invalid, please input valid value!");
                    epsilonTextBox.Focus();
                }
                else
                {
                    MyMessageBox("Rule value is invalid, please input valid value!");
                    ruleValueComboBox.Focus();
                }
                return false;
            }
            //
            // Check the value is valid
            if (double.IsInfinity(ruleValue) || double.IsNaN(ruleValue))
            {
                MyMessageBox("The input value is invalid float value!");
                if (isEpsilon) epsilonTextBox.Focus();
                else ruleValueComboBox.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Custom MessageBox for this sample, with special caption/button/icon.
        /// </summary>
        /// <param name="strMsg">Message to be displayed.</param>
        public static void MyMessageBox(String strMsg)
        {
            Autodesk.Revit.UI.TaskDialog.Show("View Filters", strMsg, Autodesk.Revit.UI.TaskDialogCommonButtons.Ok | Autodesk.Revit.UI.TaskDialogCommonButtons.Cancel);
        }
        #endregion
    }
}