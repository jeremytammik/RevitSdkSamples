//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Provide a dialog which lets users choose views to export.
    /// </summary>
    public partial class SelectViewsForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Data class
        /// </summary>
        private SelectViewsData m_selectViewsData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="selectViewsData"></param>
        public SelectViewsForm(SelectViewsData selectViewsData)
        {
            InitializeComponent();
            m_selectViewsData = selectViewsData;
            InitializeControls();
        }

        /// <summary>
        /// Initialize values and status of controls
        /// </summary>
        void InitializeControls()
        {
            UpdateViews();
        }

        /// <summary>
        /// Check all items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < checkedListBoxViews.Items.Count; ++i)
            {
                checkedListBoxViews.SetItemChecked(i, true);
            }
        }

        /// <summary>
        /// Un-check all items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCheckNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxViews.Items.Count; ++i)
            {
                checkedListBoxViews.SetItemChecked(i, false);
            }
        }

        /// <summary>
        /// Whether to show the sheets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSheets_CheckedChanged(object sender, EventArgs e)
        {
            UpdateViews();
        }

        /// <summary>
        /// Whether to show the views (except sheets)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxViews_CheckedChanged(object sender, EventArgs e)
        {
            UpdateViews();
        }

        /// <summary>
        /// Update the views in the checked list box
        /// </summary>
        private void UpdateViews()
        {
            checkedListBoxViews.Items.Clear();
            if (checkBoxViews.Checked)
            {
                foreach (Autodesk.Revit.DB.View view in m_selectViewsData.PrintableViews)
                {
                    checkedListBoxViews.Items.Add(view.ViewType.ToString() + ": " + view.ViewName);
                }
            }

            if (checkBoxSheets.Checked)
            {
                foreach (Autodesk.Revit.DB.ViewSheet viewSheet in m_selectViewsData.PrintableSheets)
                {
                    checkedListBoxViews.Items.Add("Drawing Sheet: " + viewSheet.SheetNumber + " - " +
                        viewSheet.ViewName);
                }
            }
            checkedListBoxViews.Sorted = true;
        }

        /// <summary>
        /// OK button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            GetSelectedViews();
            this.Close();           
        }

        /// <summary>
        /// Transfer information back to SelectViewsData class
        /// </summary>
        /// <returns></returns>
        private void GetSelectedViews()
        {
            m_selectViewsData.Contain3DView = false;

            foreach(int index in checkedListBoxViews.CheckedIndices)
            {
                String text = checkedListBoxViews.Items[index].ToString();
                String sheetPrefix = "Drawing Sheet: ";
                if (text.StartsWith(sheetPrefix))
                {
                    text = text.Substring(sheetPrefix.Length);
                    String sheetNumber;
                    String sheetViewName;
                    sheetNumber = text.Substring(0, text.IndexOf(" - "));
                    sheetViewName = text.Substring(text.IndexOf(" - ") + 3);
                    foreach(Autodesk.Revit.DB.ViewSheet viewSheet in m_selectViewsData.PrintableSheets)
                    {
                        if(viewSheet.SheetNumber == sheetNumber && viewSheet.ViewName == sheetViewName)
                        {
                            m_selectViewsData.SelectedViews.Insert(viewSheet);
                            break;
                        }
                    }
                }
                else
                {
                    String viewType = text.Substring(0, text.IndexOf(": "));
                    String viewName = text.Substring(text.IndexOf(": ") + 2);
                    foreach (Autodesk.Revit.DB.View view in m_selectViewsData.PrintableViews)
                    {
                        Autodesk.Revit.DB.ViewType vt = view.ViewType;
                        if(viewType == vt.ToString() && viewName == view.ViewName)
                        {
                            m_selectViewsData.SelectedViews.Insert(view);
                            if (vt == Autodesk.Revit.DB.ViewType.ThreeD)
                            {
                                m_selectViewsData.Contain3DView = true;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
