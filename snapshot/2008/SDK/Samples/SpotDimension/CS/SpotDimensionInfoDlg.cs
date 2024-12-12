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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;


namespace Revit.SDK.Samples.SpotDimension.CS
{
    using Autodesk.Revit.Elements;
    /// <summary>
    /// spot dimension form to display information
    /// </summary>
    public partial class SpotDimensionInfoDlg : Form
    {
        /// <summary>
        /// Get the last selected spotdimension
        /// </summary>
        public Autodesk.Revit.Elements.SpotDimension SelectedSpotDimension
        {
            get
            {
                return m_lastSelectDimention;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SpotDimensionInfoDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Overload constructor
        /// </summary>
        /// <param name="data"></param>
        public SpotDimensionInfoDlg(ExternalCommandData data)
        {
            this.m_data = new SpotDimensionsData(data);
            this.m_typeParamsData = new SpotDimensionParams(data.Application.ActiveDocument);
            InitializeComponent();
            InitializeCustomComponent();
        }
       
        /// <summary>
        /// Initialize custom component
        /// </summary>
        private void InitializeCustomComponent()
        {
            this.viewsComboBox.DataSource = this.m_data.Views;
            if (this.viewsComboBox.Items.Count > 0)
            {
                this.viewsComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Display all the spotdimensions in the selected view 
        /// </summary>
        /// <param name="viewName"></param>
        private void DisplaySpotDimensionInfos(string viewName)
        {
            this.spotDimensionsListView.Items.Clear();
            
            //add SpotDimensions to the listview
            foreach (SpotDimension tmpSpotDimension in m_data.SpotDimensions)
            {
                if (tmpSpotDimension.View.ViewName == viewName)
                {
                     //create a list view Item
                    ListViewItem tmpItem = new ListViewItem(tmpSpotDimension.Id.Value.ToString());
                    tmpItem.Tag = tmpSpotDimension;

                    //add the item to the listview
                    this.spotDimensionsListView.Items.Add(tmpItem);
                }
            }
        }

        /// <summary>
        /// When selected another spot dimension then update the DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spotDimensionsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.spotDimensionsListView.FocusedItem != null)
            {
                m_lastSelectDimention = this.spotDimensionsListView.FocusedItem.Tag as SpotDimension;
                this.typeParamsDataGridView.DataSource 
                    = this.m_typeParamsData.GetParameterTable(m_lastSelectDimention);
            }
        }

        /// <summary>
        /// When selected view changed, then update the list box to show the spot dimensions in that view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectViewName = this.viewsComboBox.SelectedItem as string;
            if (selectViewName != null)
            {
                DisplaySpotDimensionInfos(selectViewName);
            }
        }
    }
}