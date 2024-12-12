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
using Autodesk.Revit.Utility;

namespace Revit.SDK.Samples.Materials.CS
{
    /// <summary>
    /// A Form used to display all Asset retrieved from Application
    /// </summary>
    public partial class RenderAppearancesFrom : Form
    {
        #region Fields
        /// <summary>
        /// A reference to Application's property "Assets"
        /// </summary>
        private static AssetSet m_asssets;

        /// <summary>
        /// A map between Asset and its name
        /// </summary>
        private static Dictionary<String,Asset> m_assetDict;

        /// <summary>
        /// A list store names of all Asset retrieved from m_assets
        /// </summary>
        private static List<String> m_assetNameList;

        /// <summary>
        /// RenderAppearanceDescriptor of current Asset
        /// </summary>
        private RenderAppearanceDescriptor m_descriptor;

        /// <summary>
        /// Current selected Asset
        /// </summary>
        private Asset m_selectedAsset;
        #endregion

        #region Properties
        /// <summary>
        /// Property to get selected Asset
        /// </summary>
        public Asset SelectedAsset
        {
            get
            {
                return m_selectedAsset;
            }
        }

        /// <summary>
        /// Property to get or set AssetSet
        /// </summary>
        public static AssetSet Asssets
        {
            get 
            { 
                return m_asssets;
            }
            set 
            { 
                if(null == value)
                {
                    return;
                }
                m_asssets = value;
                LoadAssets();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// public class Constructor
        /// </summary>
        public RenderAppearancesFrom()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Occurs before a form is displayed for the first time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenderAppearancesFrom_Load(object sender, EventArgs e)
        {
           if(null == m_assetDict || null == m_assetNameList)
           {
               LoadAssets();
           }

            //Deal with ListBox used to display all Asset
            renderAppearancesListBox.DataSource = m_assetNameList;
            
            if (0 == renderAppearancesListBox.Items.Count)
            {
                m_selectedAsset = null;
                return;
            }
            else
            {
                renderAppearancesListBox.SetSelected(0, true);
            }
        }

        /// <summary>
        /// Occurs when the SelectedIndex property has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renderAppearancesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get selected Asset's name
            String selectedAssetName = renderAppearancesListBox.SelectedItem as String;
            
            //Try to find the Asset with name in dictionary
            if(m_assetDict.TryGetValue(selectedAssetName,out m_selectedAsset))
            {
                if(null != m_selectedAsset)
                {
                    m_descriptor = new RenderAppearanceDescriptor(m_selectedAsset);
                }
                else
                {
                    m_descriptor = null;
                }                
            }
            else
            {
                m_selectedAsset = null;
                m_descriptor = null;
            }
            renderAppearancePropertyGrid.SelectedObject = m_descriptor;
        }

        /// <summary>
        /// Collect Asset information from AssetSet
        /// </summary>
        private static void LoadAssets()
        {            
            if (null == m_assetDict)
            {
                m_assetDict = new Dictionary<string, Asset>();
            }

            if(null == m_assetNameList)
            {
                m_assetNameList = new List<string>();
            }

            if(null == m_asssets)
            {                
                return;
            }

            //Clear their members, prepare to add new members.
            m_assetDict.Clear();
            m_assetNameList.Clear();

            if(0 == m_asssets.Size)
            {                
                return;
            }

            //For each Asset, add their name to a list
            //and build the map between Asset and its name;
            foreach (Asset asset in m_asssets)
            {
                if (null != asset && null != asset.Name)
                {
                    m_assetDict.Add(asset.Name, asset);
                    m_assetNameList.Add(asset.Name);
                }
            }
            m_assetNameList.Sort();
        }
        #endregion
    }        
}