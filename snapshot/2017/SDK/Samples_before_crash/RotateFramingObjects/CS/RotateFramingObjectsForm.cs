//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.RotateFramingObjects.CS
{
    /// <summary>
    /// Summary description for PutDialog.
    /// </summary>
    public class RotateFramingObjectsForm : System.Windows.Forms.Form
    {
        
        private RotateFramingObjects m_instance;
        private System.ComponentModel.Container m_components = null;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        public System.Windows.Forms.RadioButton absoluteRadio;
        private System.Windows.Forms.RadioButton relativeRadio;
        private System.Windows.Forms.Label rotationLabel;
        public System.Windows.Forms.TextBox rotationTextBox;
        private bool m_isReset;
        
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(m_components != null)
                {
                    m_components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.absoluteRadio = new System.Windows.Forms.RadioButton();
            this.relativeRadio = new System.Windows.Forms.RadioButton();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.rotationLabel = new System.Windows.Forms.Label();
            this.rotationTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // absoluteRadio
            // 
            this.absoluteRadio.Location = new System.Drawing.Point(85, 35);
            this.absoluteRadio.Name = "absoluteRadio";
            this.absoluteRadio.Size = new System.Drawing.Size(72, 24);
            this.absoluteRadio.TabIndex = 0;
            this.absoluteRadio.Text = "Absolute";
            this.absoluteRadio.CheckedChanged += new System.EventHandler(this.allRadio_CheckedChanged);
            // 
            // relativeRadio
            // 
            this.relativeRadio.Checked = true;
            this.relativeRadio.Location = new System.Drawing.Point(15, 35);
            this.relativeRadio.Name = "relativeRadio";
            this.relativeRadio.Size = new System.Drawing.Size(64, 24);
            this.relativeRadio.TabIndex = 1;
            this.relativeRadio.TabStop = true;
            this.relativeRadio.Text = "Relative";
            this.relativeRadio.CheckedChanged += new System.EventHandler(this.singleRadio_CheckedChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(93, 65);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(15, 65);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // rotationLabel
            // 
            this.rotationLabel.Location = new System.Drawing.Point(12, 12);
            this.rotationLabel.Name = "rotationLabel";
            this.rotationLabel.Size = new System.Drawing.Size(50, 16);
            this.rotationLabel.TabIndex = 10;
            this.rotationLabel.Text = "Rotation";
            // 
            // rotationTextBox
            // 
            this.rotationTextBox.Location = new System.Drawing.Point(68, 9);
            this.rotationTextBox.Name = "rotationTextBox";
            this.rotationTextBox.Size = new System.Drawing.Size(100, 20);
            this.rotationTextBox.TabIndex = 1;
            this.rotationTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rotationTextBox_KeyPress);
            this.rotationTextBox.TextChanged += new System.EventHandler(this.rotationTextBox_TextChanged);
            // 
            // RotateFramingObjectsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(185, 101);
            this.Controls.Add(this.rotationTextBox);
            this.Controls.Add(this.relativeRadio);
            this.Controls.Add(this.rotationLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.absoluteRadio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RotateFramingObjectsForm";
            this.ShowInTaskbar = false;
            this.Text = "Rotate Framing Objects";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

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
