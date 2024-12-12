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
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Utility;

namespace Revit.SDK.Samples.Materials.CS
{
    /// <summary>
    /// Display all materials and its parameters.
    /// </summary>
    public partial class MaterialsForm : Form
    {
        /// <summary>
        /// A object for Revit materials management.
        /// </summary>
        private MaterialsMgr m_materialsMgr;

        /// <summary>
        /// A object used to display RenderAppearance in PropertyGrid
        /// </summary>
        private RenderAppearanceDescriptor m_descriptor;

        /// <summary>
        /// Indicate whether can get RenderAppearance information
        /// </summary>
        private bool m_canGetRenderAppearance;

        /// <summary>
        /// Get a buffer of a MaterialMgr object
        /// </summary>
        /// <param name="materialsMgr">A buffer for MaterialMgr object</param>
        /// <param name="commandData">Revit external command.</param>
        public MaterialsForm(MaterialsMgr materialsMgr, ExternalCommandData commandData)
        {
            Debug.Assert(null != materialsMgr);
            // Obtain a object for Revit materials management from revit.
            m_materialsMgr = materialsMgr;
            //Form initialize itself.
            InitializeComponent();
            MaterialParameters.ReloadMaterialParametersEvent += new MaterialParameters.ReloadMaterailParametersHandle(ReloadMaterialParameters);
            categoryComboBox.SelectedIndex = 0;
            m_canGetRenderAppearance = true;
            m_descriptor = null;
        }

        /// <summary>
        /// Reset PropertyGrid's property "SelectedObject" to update ReadOnly state
        /// </summary>
        /// <param name="materialParameters">new materialParameters</param>
        public void ReloadMaterialParameters(MaterialParameters materialParameters)
        {
            if (null != parametersPropertyGrid && null != materialParameters)
            {
                parametersPropertyGrid.SelectedObject = materialParameters;
            }
        }

        /// <summary>
        /// Occurs before a form is displayed for the first time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaterialsForm_Load(object sender, EventArgs e)
        {
            // Fill out list box with all available materials' names.
            m_materialsMgr.GetAllMaterials();
            materialsListBox.DataSource = m_materialsMgr.AllMaterials;

            if (0 == materialsListBox.Items.Count)
            {
                return;
            }
            else
            {
                materialsListBox.SetSelected(0, true);
            }
        }

        /// <summary>
        /// Occurs when the selected item has changed
        /// and that change is displayed in the ComboBox. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void categoryComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Classify material by selected name.
            m_materialsMgr.Classify(categoryComboBox.SelectedItem.ToString());

            // Update list box.
            // DataSource should be set null first, or else it display obsolete data.
            materialsListBox.DataSource = null;
            materialsListBox.DataSource = m_materialsMgr.AllMaterials;
            materialsListBox.Update();

            // If there is no available materials here,  select nothing.
            if (0 == materialsListBox.Items.Count)
            {
                return;
            }
            else
            {
                materialsListBox.SetSelected(0, true);
            }
        }

        /// <summary>
        /// Occurs when the SelectedIndex property has changed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void materialsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Fill out the property grid with all material parameters of
            //the selected one.
            m_materialsMgr.SelectNewMaterial(materialsListBox.Text);

            //Check if can get Render Appearance, if not,disable related UI.
            if(m_canGetRenderAppearance)
            {
                //If there is nothing be selected, the property grid show nothing.
                if (null == materialsListBox.SelectedItem)
                {
                    UpdateMaterialInfo(null,null);
                }
                else
                {
                    try
                    {
                        UpdateMaterialInfo(m_materialsMgr.ActiveMaterialParams, m_materialsMgr.ActiveMaterial.RenderAppearance);
                    }
                    catch
                    {
                        MessageBox.Show("Can't get Render Appearance, make sure has installed Rendering library.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        m_canGetRenderAppearance = false;
                        this.replaceButton.Enabled = false;
                    }                                              
                }
            }
            else
            {
                if (null == materialsListBox.SelectedItem)
                {
                    UpdateMaterialInfo(null, null);
                }
                else
                {
                    UpdateMaterialInfo(m_materialsMgr.ActiveMaterialParams, null);
                }
            }

            // Judge if this material is allowed to duplicate.
            duplicateButton.Enabled = m_materialsMgr.CanDuplicate;
        }

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void duplicateButton_Click(object sender, EventArgs e)
        {
            // Duplicate a concrete material. 
            m_materialsMgr.DuplicateNewMaterial(materialsListBox.Text);

            // Update data source of list box.
            // DataSource should be set null first, or else it display obsolete data.
            materialsListBox.DataSource = null;
            materialsListBox.DataSource = m_materialsMgr.AllMaterials;
            materialsListBox.Update();

            materialsListBox.SetSelected(materialsListBox.Items.Count - 1, true);
        }

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            //Set new material to the selected structure elements.
            m_materialsMgr.SetMaterial(materialsListBox.Text);
        }

        /// <summary>
        /// Occurs when the button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void replaceButton_Click(object sender, EventArgs e)
        {
            using (RenderAppearancesFrom renderAppearanceForm = new RenderAppearancesFrom())
            {
                //If selected Asset equals to original Asset, do nothing
                if (renderAppearanceForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (AssetEquals(m_materialsMgr.ActiveMaterial.RenderAppearance, renderAppearanceForm.SelectedAsset))
                    {
                        return;
                    }
                    m_materialsMgr.ActiveMaterial.RenderAppearance = renderAppearanceForm.SelectedAsset;
                    UpdateMaterialAsset(renderAppearanceForm.SelectedAsset);
                }
            }
        }

        /// <summary>
        /// Update after Material is changed
        /// </summary>
        /// <param name="parameters">Material parameter object to be updated</param>
        /// <param name="newAsset">New value of RenderAppearance</param>
        private void UpdateMaterialInfo(MaterialParameters parameters, Asset newAsset)
        {
            parametersPropertyGrid.SelectedObject = parameters;
            UpdateMaterialAsset(newAsset);
        }

        /// <summary>
        /// Update after Material or its RenderAppearance is changed
        /// </summary>
        /// <param name="newAsset">New value of RenderAppearance</param>
        private void UpdateMaterialAsset(Asset newAsset)
        {
            if (null != newAsset)
            {
                m_descriptor = new RenderAppearanceDescriptor(newAsset);
                renderAppearancePropertyGrid.SelectedObject = m_descriptor;
                renderAppearanceNameTextBox.Text = newAsset.Name;              
            }
            else
            {
                m_descriptor = null;
                renderAppearancePropertyGrid.SelectedObject = null;
                renderAppearanceNameTextBox.Text = "";
            }
        }

        /// <summary>
        /// Compare two Assets whether are same
        /// </summary>
        /// <param name="leftAsset">One Asset</param>
        /// <param name="rightAsset">Other Asset</param>
        /// <returns>Return true,if they are same; otherwise,return false</returns>
        private bool AssetEquals(Asset leftAsset,Asset rightAsset)
        {
            if(leftAsset.Name == rightAsset.Name && leftAsset.Size == rightAsset.Size)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}