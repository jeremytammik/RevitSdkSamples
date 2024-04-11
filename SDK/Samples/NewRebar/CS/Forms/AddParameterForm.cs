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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// This form provides an entrance for user to add parameters to RebarShape.
    /// </summary>
    public partial class AddParameter : System.Windows.Forms.Form
    {
        List<RebarShapeParameter> m_parameterList;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddParameter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor, initialize fields.
        /// </summary>
        /// <param name="list"></param>
        public AddParameter(List<RebarShapeParameter> list)
            : this()
        {
            m_parameterList = list;
        }

        /// <summary>
        /// Is it formula parameter or not?
        /// </summary>
        public bool IsFormula
        {
            get
            {
                return paramFormulaRadioButton.Checked;
            }
        }

        /// <summary>
        /// Parameter name from paramNameTextBox.
        /// </summary>
        public string ParamName
        {
            get
            {
                return paramNameTextBox.Text;
            }
        }

        /// <summary>
        /// Parameter value from paramValueTextBox.
        /// </summary>
        public string ParamValue
        {
            get
            {
                return paramValueTextBox.Text;
            }
        }

        /// <summary>
        /// Cancel Button, Return DialogResult.Cancel and close this form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// OK button, check the correction of data, then Return DialogResult.OK
        /// and close this form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ParamName) || string.IsNullOrEmpty(ParamValue))
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", "Parameter Name and Value should not be null.");
                return;
            }

            if (!IsFormula)
            {
                try
                {
                    Double.Parse(ParamValue);
                }
                catch
                {
                    Autodesk.Revit.UI.TaskDialog.Show("Revit", "Input value - " + ParamValue + " - should be double.");
                    return;
                }
            }

            // Make sure Parameter name should be started with letter
            // And just contains letters, numbers and underlines 
            Regex regex = new Regex("^[a-zA-Z]\\w*$");
            if (!regex.IsMatch(ParamName))
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", "Parameter name should be started with letter \r\n And just contains letters, numbers and underlines.");
                paramNameTextBox.Focus();
                return;
            }

            // Make sure the name is unique.
            foreach (RebarShapeParameter param in m_parameterList)
            {
                if (param.Name.Equals(ParamName))
                {
                    Autodesk.Revit.UI.TaskDialog.Show("Revit", "The name is already exist, please input again.");
                    paramNameTextBox.Focus();
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}