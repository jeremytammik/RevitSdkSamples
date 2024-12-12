//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// This form provides an entrance for user to add constraints to RebarShape.
    /// </summary>
    public partial class AddConstraint : Form
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddConstraint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor, Initialize fields.
        /// </summary>
        /// <param name="constraintTypes"></param>
        public AddConstraint(List<Type> constraintTypes)
            : this()
        {
            constraintTypesComboBox.DataSource = constraintTypes;
            constraintTypesComboBox.DisplayMember = "Name";
        }

        /// <summary>
        /// Return the type from constraintTypesComboBox selection.
        /// </summary>
        public Type ConstraintType
        {
            get
            {
                return constraintTypesComboBox.SelectedItem as Type;
            }
        }

        /// <summary>
        /// OK Button, Return DialogResult.OK and close this form. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
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
    }
}