//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.BoundaryConditions.CS
{
    /// <summary>
    /// UI which display the information and interact with users
    /// </summary>
    public partial class BoundaryConditionsForm : System.Windows.Forms.Form
    { 
        // an instance of BoundaryConditionsData class which deal with the need data
        private BoundaryConditionsData m_dataBuffer;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dataBuffer"></param>
        public BoundaryConditionsForm(BoundaryConditionsData dataBuffer)
        {
            InitializeComponent();

            m_dataBuffer = dataBuffer;
        }

        /// <summary>
        /// display the information about the host element and the BC parameter value 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoundaryConditionsForm_Load(object sender, EventArgs e)
        {
            // display host element id in the text box
            hostTextBox.Text = m_dataBuffer.HostElement.Id.ToString();

            // if the selected element has not a BC, create one and display the default parameter values
            // else list the BC ids in the combox and display the first BC's parameter values
            if (0 == m_dataBuffer.BCs.Count)
            {
                bCComboBox.Visible       = false;
                bCLabel.Visible          = false;
                bool isCreatedSuccessful = m_dataBuffer.CreateBoundaryConditions();
                m_dataBuffer.HostElement.Document.Regenerate();

                if (!isCreatedSuccessful)
                {
                    this.DialogResult = DialogResult.Retry;
                    return;
                }     
            }
            else
            {
                bCComboBox.Visible = true;
                bCLabel.Visible    = true;
            }

            // list the boundary conditions Id values to the combobox
            System.Collections.ICollection bCIdValues = m_dataBuffer.BCs.Keys;
            foreach (Autodesk.Revit.DB.ElementId bCIdValue in bCIdValues)
            {
                bCComboBox.Items.Add(bCIdValue);
            }

            bCComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// display the BC's property value according the selected BC ID in combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bCComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // create a BCProperties instance according to current selected BC Id
            m_dataBuffer.BCProperties = new BCProperties(m_dataBuffer.BCs[Autodesk.Revit.DB.ElementId.Parse(bCComboBox.Text)]);
            
            // set the display object
            bCPropertyGrid.SelectedObject = m_dataBuffer.BCProperties;

            // set the browsable attributes of the property grid
            Attribute[] attributes = null;
            if (BCType.Point == m_dataBuffer.BCProperties.BoundaryConditionsType)
            {
                attributes = new Attribute[] { new BCTypeAttribute(new BCType[] { BCType.Point }) };
            }
            else if (BCType.Line == m_dataBuffer.BCProperties.BoundaryConditionsType)
            {
                attributes = new Attribute[] { new BCTypeAttribute(new BCType[] { BCType.Line }) };
            }
            else if (BCType.Area == m_dataBuffer.BCProperties.BoundaryConditionsType)
            {
                attributes = new Attribute[] { new BCTypeAttribute(new BCType[] { BCType.Area }) };
            }

            bCPropertyGrid.BrowsableAttributes = new AttributeCollection(attributes);
        }

        /// <summary>
        /// deal with operations that user set these parameters with other valid value.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void bCPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label.Contains("Translation") || e.ChangedItem.Label.Contains("Rotation"))
            {
                // invoke SetSpringModulus method in BCProperties class to
                // let user enter a positive number as the SpringModulus 
                // when set any Translation/Rotation parameter to Spring 
                BCTranslationRotation value = (BCTranslationRotation)e.ChangedItem.Value;
                if (BCTranslationRotation.Spring == value)
                {
                    m_dataBuffer.BCProperties.SetSpringModulus(e.ChangedItem.Label);
                }
            }
        }

        /// <summary>
        /// confirm the changed and exist the UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
