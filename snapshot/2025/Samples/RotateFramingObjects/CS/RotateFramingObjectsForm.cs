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
using System.Windows.Forms;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.RotateFramingObjects.CS
{
    /// <summary>
    /// Summary description for PutDialog.
    /// </summary>
    public partial class RotateFramingObjectsForm : System.Windows.Forms.Form
    {
        private RotateFramingObjects m_instance;
        private bool m_isReset;
        public bool IsReset
        {
            get 
            {
                return m_isReset;
            }
            set
            {
                m_isReset = value;
            }
        }

        /// <summary>
        /// new form, retrieve relevant data from instance
        /// </summary>
        /// <param name="Inst">RotateFramingObjects instance</param>
        public RotateFramingObjectsForm(RotateFramingObjects Inst)
        {
            m_isReset = false;
            m_instance = Inst;
            if (null == m_instance)
            {
                TaskDialog.Show("Revit", "Load Application Failed");
            }    
            InitializeComponent();
            //this.rotationTextBox.Text = "Value";
        }

        private void okButton_Click(object sender, System.EventArgs e)
        {
            if(IsReset)
            {
                m_instance.RotateElement();
                
            }
            this.DialogResult=DialogResult.OK;
            this.Close();

        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult=DialogResult.Cancel;
            this.Close();
        }

        private void singleRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            m_isReset = true;
            m_instance.IsAbsoluteChecked = false;
        }

        private void allRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            m_isReset = true;
            m_instance.IsAbsoluteChecked = true;
        }

        private void rotationTextBox_TextChanged(object sender, System.EventArgs e)
        {
            if("" != this.rotationTextBox.Text)
            {
                try
                {
                    m_instance.ReceiveRotationTextBox = Convert.ToDouble(this.rotationTextBox.Text);
                }
                catch (Exception)
                {
                    //this.DialogResult=DialogResult.Cancel;
                    TaskDialog.Show("Revit", "Please input number.");
                    this.rotationTextBox.Clear();
                }

            }
            else
            {
                m_instance.ReceiveRotationTextBox = 0;
            }
            m_isReset = true;
        }

        private void rotationTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (13 == e.KeyChar)
            {
                okButton_Click(sender,e);
            }
            else
                rotationTextBox_TextChanged(sender,e);
        }
    }
}
