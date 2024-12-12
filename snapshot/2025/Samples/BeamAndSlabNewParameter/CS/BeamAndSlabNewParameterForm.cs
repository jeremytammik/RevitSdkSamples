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

using System.Windows.Forms;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.BeamAndSlabNewParameter.CS
{
    /// <summary>
    /// User Interface.
    /// </summary>
    public partial class BeamAndSlabParametersForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dataBuffer"></param>
        public BeamAndSlabParametersForm(Command dataBuffer)
        {
            InitializeComponent();
            m_dataBuffer = dataBuffer;
        }
    
        // an instance of Command class
        Command m_dataBuffer = null;

        /// <summary>
        /// Call SetNewParameterToBeamsAndSlabs function 
        /// which is belongs to BeamAndSlabParameters class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addParameterButton_Click(object sender, System.EventArgs e)
        {
            bool successAddParameter = m_dataBuffer.SetNewParameterToBeamsAndSlabs();

            if (successAddParameter)
            {
                this.DialogResult = DialogResult.OK;
                m_dataBuffer.SetValueToUniqueIDParameter();
                TaskDialog.Show("Revit", "Done");
            }
            else
            {
                this.DialogResult = DialogResult.None;
                m_dataBuffer.SetValueToUniqueIDParameter();
                TaskDialog.Show("Revit", "Unique ID parameter exist");
            }
        }    
        
        /// <summary>
        /// Call SetValueToUniqueIDParameter function 
        /// which is belongs to BeamAndSlabNewParameters class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findButton_Click(object sender, System.EventArgs e)
        {
            if (null != attributeValueListBox.SelectedItem)
            {
                m_dataBuffer.FindElement(attributeValueListBox.SelectedItem.ToString());
            }
        }

        /// <summary>
        /// Call SendValueToListBox function which is belongs to BeamAndSlabNewParameters class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayValueButton_Click(object sender, System.EventArgs e)
        {
            attributeValueListBox.DataSource = m_dataBuffer.SendValueToListBox();

            //If we displayed nothing, give possible reasons
            if (0 == attributeValueListBox.Items.Count)
            {
                string message = "";
                message = "There was an error executing the command.\r\n";
                message = message + "Possible reasons for this are:\r\n\r\n";
                message = message + "1. No parameter was added.\r\n";
                message = message + "2. No beam or slab was selected.\r\n";
                message = message + "3. The value was blank.\r\n";
                TaskDialog.Show("Revit", message);
            }
        }

        /// <summary>
        /// Close this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
