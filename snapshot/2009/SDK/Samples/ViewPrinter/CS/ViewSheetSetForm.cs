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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    public partial class viewSheetSetForm : Form
    {
        public viewSheetSetForm(ViewSheets viewSheets)
        {            
            InitializeComponent();

            m_viewSheets = viewSheets;
        }

        private ViewSheets m_viewSheets;
        private bool m_stopUpdateFlag;

        private void ViewSheetSetForm_Load(object sender, EventArgs e)
        {
            viewSheetSetNameComboBox.DataSource = m_viewSheets.ViewSheetSetNames;            
            this.viewSheetSetNameComboBox.SelectedValueChanged += new System.EventHandler(this.viewSheetSetNameComboBox_SelectedValueChanged);
            viewSheetSetNameComboBox.SelectedItem = m_viewSheets.SettingName;

            showSheetsCheckBox.Checked = true;
            showViewsCheckBox.Checked = true;
            ListViewSheetSet();
            this.viewSheetSetListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.viewSheetSetListView_ItemChecked);
        }

        private void ListViewSheetSet()
        {
            VisibleType vt;
            if (showSheetsCheckBox.Checked && showViewsCheckBox.Checked)
            {
                vt = VisibleType.VT_BothViewAndSheet;
            }
            else if (showSheetsCheckBox.Checked && !showViewsCheckBox.Checked)
            {
                vt = VisibleType.VT_SheetOnly;
            }
            else if (!showSheetsCheckBox.Checked && showViewsCheckBox.Checked)
            {
                vt = VisibleType.VT_ViewOnly;
            }
            else
            {
                vt = VisibleType.VT_None;
            }

            List<Autodesk.Revit.Elements.View> views = m_viewSheets.AvailableViewSheetSet(vt);
            viewSheetSetListView.Items.Clear();
            foreach (Autodesk.Revit.Elements.View view in views)
            {
                ListViewItem item = new ListViewItem(view.ViewType.ToString() + ": " + view.ViewName);
                item.Checked = m_viewSheets.IsSelected(item.Text);
                viewSheetSetListView.Items.Add(item);
            }
        }

        private void viewSheetSetNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (m_stopUpdateFlag)
                return;

            m_viewSheets.SettingName = viewSheetSetNameComboBox.SelectedItem as string;            
            ListViewSheetSet();

            saveButton.Enabled = revertButton.Enabled = false;

            reNameButton.Enabled = deleteButton.Enabled =
                m_viewSheets.SettingName.Equals("<In-Session>") ? false : true;
        }

        private void showSheetsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ListViewSheetSet();
        }

        private void showViewsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ListViewSheetSet();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            foreach (ListViewItem item in viewSheetSetListView.Items)
            {
                if (item.Checked)
                {
                    names.Add(item.Text);
                }
            }

            m_viewSheets.ChangeCurrentViewSheetSet(names);

            m_viewSheets.Save();
        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            using (SaveAsForm dlg = new SaveAsForm(m_viewSheets))
            {
                dlg.ShowDialog();
            }

            m_stopUpdateFlag = true;
            viewSheetSetNameComboBox.DataSource = m_viewSheets.ViewSheetSetNames;            
            viewSheetSetNameComboBox.Update();
            m_stopUpdateFlag = false;

            viewSheetSetNameComboBox.SelectedItem = m_viewSheets.SettingName;
        }

        private void revertButton_Click(object sender, EventArgs e)
        {
            m_viewSheets.Revert();
            ViewSheetSetForm_Load(null, null);
        }

        private void reNameButton_Click(object sender, EventArgs e)
        {
            using (ReNameForm dlg = new ReNameForm(m_viewSheets))
            {
                dlg.ShowDialog();
            }

            m_stopUpdateFlag = true;
            viewSheetSetNameComboBox.DataSource = m_viewSheets.ViewSheetSetNames;            
            viewSheetSetNameComboBox.Update();
            m_stopUpdateFlag = false;

            viewSheetSetNameComboBox.SelectedItem = m_viewSheets.SettingName;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            m_viewSheets.Delete();

            m_stopUpdateFlag = true;
            viewSheetSetNameComboBox.DataSource = m_viewSheets.ViewSheetSetNames;
            viewSheetSetNameComboBox.Update();
            m_stopUpdateFlag = false;

            viewSheetSetNameComboBox.SelectedItem = m_viewSheets.SettingName;
        }

        private void checkAllButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in viewSheetSetListView.Items)
            {
                item.Checked = true;
            }
        }

        private void checkNoneButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in viewSheetSetListView.Items)
            {
                item.Checked = false;
            }
        }

        private void viewSheetSetListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!m_viewSheets.SettingName.Equals("<In-Session>")
                && !saveButton.Enabled)
            {
                saveButton.Enabled = revertButton.Enabled
                    = reNameButton.Enabled = true;
            }
        }
    }
}