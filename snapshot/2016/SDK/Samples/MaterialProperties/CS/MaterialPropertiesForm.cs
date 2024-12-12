//
// (C) Copyright 2003-2015 by Autodesk, Inc. 
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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.MaterialProperties.CS
{
    /// <summary>
    /// Summary description for MaterialPropFrm.
    /// </summary>
    public class MaterialPropertiesForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label typeLable;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.ComboBox subTypeComboBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button changeButton;
        private System.Windows.Forms.DataGrid parameterDataGrid;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private MaterialPropertiesForm()
        {
        }

        /// <summary>
        /// material properties from
        /// </summary>
        /// <param name="dataBuffer">material properties from Revit</param>
        public MaterialPropertiesForm(MaterialProperties dataBuffer)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            parameterDataGrid.PreferredColumnWidth = parameterDataGrid.Width / 2 - 2;

            m_dataBuffer = dataBuffer;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private MaterialProperties m_dataBuffer = null;

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.typeLable = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.subTypeComboBox = new System.Windows.Forms.ComboBox();
            this.parameterDataGrid = new System.Windows.Forms.DataGrid();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.changeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.parameterDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // typeLable
            // 
            this.typeLable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.typeLable.Location = new System.Drawing.Point(24, 16);
            this.typeLable.Name = "typeLable";
            this.typeLable.Size = new System.Drawing.Size(80, 23);
            this.typeLable.TabIndex = 0;
            this.typeLable.Text = "Material Type:";
            // 
            // typeComboBox
            // 
            this.typeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.typeComboBox.Location = new System.Drawing.Point(112, 16);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(264, 21);
            this.typeComboBox.TabIndex = 2;
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // subTypeComboBox
            // 
            this.subTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.subTypeComboBox.Location = new System.Drawing.Point(112, 48);
            this.subTypeComboBox.MaxDropDownItems = 30;
            this.subTypeComboBox.Name = "subTypeComboBox";
            this.subTypeComboBox.Size = new System.Drawing.Size(264, 21);
            this.subTypeComboBox.TabIndex = 3;
            this.subTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.subTypeComboBox_SelectedIndexChanged);
            // 
            // parameterDataGrid
            // 
            this.parameterDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterDataGrid.CaptionVisible = false;
            this.parameterDataGrid.DataMember = "";
            this.parameterDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.parameterDataGrid.Location = new System.Drawing.Point(16, 88);
            this.parameterDataGrid.Name = "parameterDataGrid";
            this.parameterDataGrid.ReadOnly = true;
            this.parameterDataGrid.RowHeadersVisible = false;
            this.parameterDataGrid.Size = new System.Drawing.Size(480, 380);
            this.parameterDataGrid.TabIndex = 4;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(104, 480);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(192, 480);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applyButton.Location = new System.Drawing.Point(280, 480);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 7;
            this.applyButton.Text = "&Apply";
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // changeButton
            // 
            this.changeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.changeButton.Location = new System.Drawing.Point(368, 480);
            this.changeButton.Name = "changeButton";
            this.changeButton.Size = new System.Drawing.Size(128, 23);
            this.changeButton.TabIndex = 8;
            this.changeButton.Text = "Change &Unit Weight";
            this.changeButton.Click += new System.EventHandler(this.changeButton_Click);
            // 
            // MaterialPropertiesForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(512, 512);
            this.Controls.Add(this.changeButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.parameterDataGrid);
            this.Controls.Add(this.subTypeComboBox);
            this.Controls.Add(this.typeComboBox);
            this.Controls.Add(this.typeLable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialPropertiesForm";
            this.ShowInTaskbar = false;
            this.Text = "Material Properties";
            this.Load += new System.EventHandler(this.MaterialPropFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.parameterDataGrid)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaterialPropFrm_Load(object sender, System.EventArgs e)
        {
            LoadCurrentMaterial();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void typeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
           if ((StructuralAssetClass)typeComboBox.SelectedIndex == StructuralAssetClass.Metal)
            {
                applyButton.Enabled = true;
                changeButton.Enabled = true;
                subTypeComboBox.Enabled = true;
                subTypeComboBox.DataSource = m_dataBuffer.SteelCollection;
                subTypeComboBox.DisplayMember = "MaterialName";
                subTypeComboBox.ValueMember = "Material";
                parameterDataGrid.DataSource = m_dataBuffer.GetParameterTable(subTypeComboBox.SelectedValue, (StructuralAssetClass)typeComboBox.SelectedIndex);
            }
           else if ((StructuralAssetClass)typeComboBox.SelectedIndex == StructuralAssetClass.Concrete)
            {
                applyButton.Enabled = true;
                changeButton.Enabled = true;
                subTypeComboBox.Enabled = true;
                subTypeComboBox.DataSource = m_dataBuffer.ConcreteCollection;
                subTypeComboBox.DisplayMember = "MaterialName";
                subTypeComboBox.ValueMember = "Material";
                parameterDataGrid.DataSource = m_dataBuffer.GetParameterTable(subTypeComboBox.SelectedValue, (StructuralAssetClass)typeComboBox.SelectedIndex);
            }
            else
            {
                applyButton.Enabled = false;
                changeButton.Enabled = false;
                subTypeComboBox.DataSource = new ArrayList();
                subTypeComboBox.Enabled = false;
                parameterDataGrid.DataSource = new DataTable();
            }
        }


        /// <summary>
        /// change the content in datagrid according to selected material type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subTypeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (null != subTypeComboBox.SelectedValue)
            {
                m_dataBuffer.UpdateMaterial(subTypeComboBox.SelectedValue);
            }

            parameterDataGrid.DataSource = m_dataBuffer.GetParameterTable(subTypeComboBox.SelectedValue, (StructuralAssetClass)typeComboBox.SelectedIndex);
        }

        /// <summary>
        /// close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// set selected element's material to current selection and close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, System.EventArgs e)
        {
            if (null != subTypeComboBox.SelectedValue)
            {
                m_dataBuffer.UpdateMaterial(subTypeComboBox.SelectedValue);
                m_dataBuffer.SetMaterial();
            }
            this.Close();
        }


        /// <summary>
        /// set selected element's material to current selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyButton_Click(object sender, System.EventArgs e)
        {
            if (null != subTypeComboBox.SelectedValue)
            {
                m_dataBuffer.UpdateMaterial(subTypeComboBox.SelectedValue);
                m_dataBuffer.SetMaterial();
            }
        }


        /// <summary>
        /// change unit weight all instances of the elements that use this material
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeButton_Click(object sender, System.EventArgs e)
        {
            TaskDialog.Show("Revit", "This will change the unit weight of all instances that use this material in current document.");

            if (!m_dataBuffer.ChangeUnitWeight())
            {
                TaskDialog.Show("Revit", "Failed to change the unit weight.");
                return;
            }
            LoadCurrentMaterial();
        }

        /// <summary>
        /// update display data to selected element's material
        /// </summary>
        private void LoadCurrentMaterial()
        {
            typeComboBox.DataSource = m_dataBuffer.MaterialTypes;

            typeComboBox.SelectedIndex = (int)m_dataBuffer.CurrentType;

            if (null == m_dataBuffer.CurrentMaterial || (m_dataBuffer.CurrentType != StructuralAssetClass.Metal
               && m_dataBuffer.CurrentType != StructuralAssetClass.Concrete))
            {
                return;
            }
            Autodesk.Revit.DB.Material tmp = m_dataBuffer.CurrentMaterial as Autodesk.Revit.DB.Material;
            if (null == tmp)
                return;

            subTypeComboBox.SelectedValue = tmp;
            parameterDataGrid.DataSource = m_dataBuffer.GetParameterTable(subTypeComboBox.SelectedValue, (StructuralAssetClass)typeComboBox.SelectedIndex);
        }
    }
}