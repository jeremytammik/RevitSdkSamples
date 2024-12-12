//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
namespace Revit.SDK.Samples.CreateComplexAreaRein.CS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;


    /// <summary>
    /// simple business process of UI
    /// </summary>
    public partial class CreateComplexAreaReinForm : System.Windows.Forms.Form
    {
        private AreaReinData m_dataBuffer;

        /// <summary>
        /// constructor; initialize member data
        /// </summary>
        /// <param name="dataBuffer"></param>
        public CreateComplexAreaReinForm(AreaReinData dataBuffer)
        {
            InitializeComponent();

            m_dataBuffer = dataBuffer;
        }

        /// <summary>
        /// bind data to controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateComplexAreaReinForm_Load(object sender, EventArgs e)
        {
            layoutRuleComboBox.DataSource = Enum.GetNames(typeof(LayoutRules));
        }

        /// <summary>
        /// to create
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            m_dataBuffer.LayoutRule = (LayoutRules)Enum.Parse(typeof(LayoutRules), 
                layoutRuleComboBox.SelectedItem.ToString());
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// cancel the command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
