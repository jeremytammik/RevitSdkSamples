//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.SlabProperties.CS
{
    /// <summary>
    /// Show some properties of a slab in Revit Structure 5, including Level, Type name, Span divection,
    /// Material name, Thickness, and Young Modulus for each layer of the slab's materiral. 
    /// </summary>
    public class SlabPropertiesForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox layerGroupBox;
        private System.Windows.Forms.RichTextBox layerRichTextBox;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.Label typeNameLabel;
        private System.Windows.Forms.Label spanDirectionLabel;
        private System.Windows.Forms.TextBox levelTextBox;
        private System.Windows.Forms.TextBox typeNameTextBox;
        private System.Windows.Forms.TextBox spanDirectionTextBox;
        private System.Windows.Forms.Button closeButton;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Label degreeLabel;

        // To store the datas
        private Command m_dataBuffer;


        private SlabPropertiesForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// overload the constructor
        /// </summary>
        /// <param name="dataBuffer">To store the datas of a slab</param>
        public SlabPropertiesForm(Command dataBuffer)
        {
            InitializeComponent();
            
            // get all the data
            m_dataBuffer = dataBuffer;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != components)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layerGroupBox = new System.Windows.Forms.GroupBox();
            this.layerRichTextBox = new System.Windows.Forms.RichTextBox();
            this.levelLabel = new System.Windows.Forms.Label();
            this.levelTextBox = new System.Windows.Forms.TextBox();
            this.typeNameTextBox = new System.Windows.Forms.TextBox();
            this.spanDirectionTextBox = new System.Windows.Forms.TextBox();
            this.typeNameLabel = new System.Windows.Forms.Label();
            this.spanDirectionLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.degreeLabel = new System.Windows.Forms.Label();
            this.layerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // layerGroupBox
            // 
            this.layerGroupBox.Controls.Add(this.layerRichTextBox);
            this.layerGroupBox.Location = new System.Drawing.Point(22, 86);
            this.layerGroupBox.Name = "layerGroupBox";
            this.layerGroupBox.Size = new System.Drawing.Size(375, 265);
            this.layerGroupBox.TabIndex = 29;
            this.layerGroupBox.TabStop = false;
            this.layerGroupBox.Text = "Layers:";
            // 
            // layerRichTextBox
            // 
            this.layerRichTextBox.Location = new System.Drawing.Point(6, 19);
            this.layerRichTextBox.Name = "layerRichTextBox";
            this.layerRichTextBox.ReadOnly = true;
            this.layerRichTextBox.Size = new System.Drawing.Size(359, 232);
            this.layerRichTextBox.TabIndex = 2;
            this.layerRichTextBox.Text = "";
            // 
            // levelLabel
            // 
            this.levelLabel.Location = new System.Drawing.Point(13, 7);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(98, 23);
            this.levelLabel.TabIndex = 27;
            this.levelLabel.Text = "Level:";
            this.levelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // levelTextBox
            // 
            this.levelTextBox.Location = new System.Drawing.Point(117, 8);
            this.levelTextBox.Name = "levelTextBox";
            this.levelTextBox.ReadOnly = true;
            this.levelTextBox.Size = new System.Drawing.Size(280, 20);
            this.levelTextBox.TabIndex = 24;
            // 
            // typeNameTextBox
            // 
            this.typeNameTextBox.Location = new System.Drawing.Point(117, 34);
            this.typeNameTextBox.Name = "typeNameTextBox";
            this.typeNameTextBox.ReadOnly = true;
            this.typeNameTextBox.Size = new System.Drawing.Size(280, 20);
            this.typeNameTextBox.TabIndex = 22;
            // 
            // spanDirectionTextBox
            // 
            this.spanDirectionTextBox.Location = new System.Drawing.Point(117, 60);
            this.spanDirectionTextBox.Name = "spanDirectionTextBox";
            this.spanDirectionTextBox.ReadOnly = true;
            this.spanDirectionTextBox.Size = new System.Drawing.Size(224, 20);
            this.spanDirectionTextBox.TabIndex = 23;
            // 
            // typeNameLabel
            // 
            this.typeNameLabel.Location = new System.Drawing.Point(13, 34);
            this.typeNameLabel.Name = "typeNameLabel";
            this.typeNameLabel.Size = new System.Drawing.Size(98, 23);
            this.typeNameLabel.TabIndex = 25;
            this.typeNameLabel.Text = "Type Name:";
            this.typeNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // spanDirectionLabel
            // 
            this.spanDirectionLabel.Location = new System.Drawing.Point(13, 60);
            this.spanDirectionLabel.Name = "spanDirectionLabel";
            this.spanDirectionLabel.Size = new System.Drawing.Size(98, 23);
            this.spanDirectionLabel.TabIndex = 26;
            this.spanDirectionLabel.Text = "Span Direction:";
            this.spanDirectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(322, 367);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // degreeLabel
            // 
            this.degreeLabel.Location = new System.Drawing.Point(347, 59);
            this.degreeLabel.Name = "degreeLabel";
            this.degreeLabel.Size = new System.Drawing.Size(50, 23);
            this.degreeLabel.TabIndex = 26;
            this.degreeLabel.Text = "Degree";
            this.degreeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SlabPropertiesForm
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(411, 402);
            this.Controls.Add(this.layerGroupBox);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.levelTextBox);
            this.Controls.Add(this.typeNameTextBox);
            this.Controls.Add(this.spanDirectionTextBox);
            this.Controls.Add(this.typeNameLabel);
            this.Controls.Add(this.degreeLabel);
            this.Controls.Add(this.spanDirectionLabel);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SlabPropertiesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Slab Properties";
            this.Load += new System.EventHandler(this.SlabPropertiesForm_Load);
            this.layerGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// Close the Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Display the properties on the form when the form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlabPropertiesForm_Load(object sender, System.EventArgs e)
        {
            this.levelTextBox.Text = m_dataBuffer.Level;
            this.typeNameTextBox.Text = m_dataBuffer.TypeName;
            this.spanDirectionTextBox.Text = m_dataBuffer.SpanDirection;

            int numberOfLayers = m_dataBuffer.NumberOfLayers;

            this.layerRichTextBox.Text = "";

            for (int i = 0; i < numberOfLayers; i++)
            {
                // Get each layer's Material name and Young Modulus properties
                m_dataBuffer.SetLayer(i);

                this.layerRichTextBox.Text += "Layer " + (i + 1).ToString() + "\r\n";
                this.layerRichTextBox.Text += "Material name:  " + m_dataBuffer.LayerMaterialName + "\r\n";
                this.layerRichTextBox.Text += "Thickness: " + m_dataBuffer.LayerThickness + "\r\n";
                this.layerRichTextBox.Text += "YoungModulus X:  " + m_dataBuffer.LayerYoungModulusX + "\r\n";
                this.layerRichTextBox.Text += "YoungModulus Y:  " + m_dataBuffer.LayerYoungModulusY + "\r\n";
                this.layerRichTextBox.Text += "YoungModulus Z:  " + m_dataBuffer.LayerYoungModulusZ + "\r\n";
                this.layerRichTextBox.Text += "-----------------------------------------------------------" + "\r\n";
            }
        }
    }
}
