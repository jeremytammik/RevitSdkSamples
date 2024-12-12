//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.TagBeam.CS
{
    /// <summary>
    /// Form to get input from user to create beam tags.
    /// </summary>
    public partial class TagBeamForm : System.Windows.Forms.Form
    {
        //Required designer variable.
        private TagBeamData m_dataBuffer;

        /// <summary>
        /// Initialize GUI with TagBeamData 
        /// </summary>
        /// <param name="dataBuffer">relevant data from revit</param>
        public TagBeamForm(TagBeamData dataBuffer)
        {
            InitializeComponent();
            m_dataBuffer = dataBuffer;
            InitializeComboBoxes();           
        }

        /// <summary>
        /// Initialize the combo boxes. 
        /// </summary>
        private void InitializeComboBoxes()
        {
            //Initialize the tag mode comboBox.
            tagComboBox.DataSource = Enum.GetValues(typeof(TagMode));
            //Initialize the tag orientation comboBox.
            tagOrientationComboBox.DataSource = Enum.GetValues(typeof(TagOrientation));

            //Set DropDownStyle of combo boxes to "DropDownList"
            this.tagComboBox.DropDownStyle =
            this.tagSymbolComboBox.DropDownStyle =
            this.tagOrientationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Create tags on beam's start and end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tagSymbolComboBox.SelectedItem != null)
                {
                    m_dataBuffer.CreateTag((TagMode)tagComboBox.SelectedItem,
                        tagSymbolComboBox.SelectedItem as FamilySymbolWrapper,
                        this.leadercheckBox.Checked,
                        (TagOrientation)tagOrientationComboBox.SelectedItem);
                }
                else
                {
                    throw new ApplicationException("No tag type selected.");
                }
    
                this.Close();
            }
            catch(Exception ex)
            {
                TaskDialog.Show("Revit", ex.Message);
            }           
        }

        /// <summary>
        /// Close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Update tag types in tagComboBox according to the selected tag mode.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tagComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            tagSymbolComboBox.DataSource = m_dataBuffer[(TagMode)tagComboBox.SelectedItem];
            tagSymbolComboBox.DisplayMember = "Name";
        }
    }
}