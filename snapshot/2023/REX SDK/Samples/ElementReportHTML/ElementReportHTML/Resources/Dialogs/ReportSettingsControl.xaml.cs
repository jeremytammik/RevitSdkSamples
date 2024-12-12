//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using REX.ElementReportHTML.Main;

namespace REX.ElementReportHTML.Resources.Dialogs
{
    /// <summary>
    /// Represents the control which allows user to define settings for the note.
    /// </summary>
    public partial class ReportSettingsControl : REX.Common.REXExtensionControl
    {
        /// <summary>
        /// Get the main extension.
        /// </summary>
        /// <value>The main extension.</value>
        internal Extension ThisMainExtension
        {
            get
            {
                return (Extension)ThisExtension;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportSettingsControl"/> class.
        /// </summary>
        public ReportSettingsControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportSettingsControl"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public ReportSettingsControl(REX.Common.REXExtension extension)
            : base(extension)
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes the dialog with current settings.
        /// </summary>
        public void SetDialog()
        {
            FillCategoriesList();
            FillIdsCombo();
            idCheckBox.IsChecked = false;
            idComboBox.IsEnabled = false;
        }
        /// <summary>
        /// Fills the list of categories.
        /// </summary>
        private void FillCategoriesList()
        {
            categoriesListBox.Items.Clear();
            foreach (Node node in ThisMainExtension.Data.MainNode.Nodes.Values)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.IsChecked = true;
                checkBox.Checked += new RoutedEventHandler(checkBox_Checked);
                checkBox.Unchecked += new RoutedEventHandler(checkBox_Unchecked);
                checkBox.Content = node.Name;
                checkBox.Tag = node;
                categoriesListBox.Items.Add(checkBox);
            }
        }
        /// <summary>
        /// Fills the list of ids.
        /// </summary>
        private void FillIdsCombo()
        {
            idComboBox.Items.Clear();

            foreach (Node catNode in ThisMainExtension.Data.MainNode.Nodes.Values)
            {
                foreach (Node elemNode in catNode.Nodes.Values)
                {
                    idComboBox.Items.Add(elemNode);
                }                
            }
        }
        /// <summary>
        /// Handles the Unchecked event of the checkBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ThisMainExtension.Data.SelectedCategories.Remove(checkBox.Tag as Node);
            ThisMainExtension.RefreshNote();           
        }
        /// <summary>
        /// Handles the Checked event of the checkBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void checkBox_Checked(object sender, RoutedEventArgs e)
        {
             CheckBox checkBox = sender as CheckBox;
            ThisMainExtension.Data.SelectedCategories.Add(checkBox.Tag as Node);
            ThisMainExtension.RefreshNote();
        }
        /// <summary>
        /// Handles the SelectionChanged event of the idComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void idComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThisMainExtension.Data.SelectedElement = idComboBox.SelectedItem as Node;
            ThisMainExtension.RefreshNote();
        }
        /// <summary>
        /// Handles the 1 event of the CheckBox_Checked control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ThisMainExtension.Data.IdMode = checkBox.IsChecked.Value;
            idComboBox.IsEnabled = checkBox.IsChecked.Value;
            categoriesListBox.IsEnabled = !checkBox.IsChecked.Value;
            ThisMainExtension.RefreshNote();
        }
    }
}
