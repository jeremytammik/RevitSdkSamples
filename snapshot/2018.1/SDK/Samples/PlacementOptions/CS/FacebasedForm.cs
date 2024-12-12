//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.PlacementOptions.CS
{
    /// <summary>
    /// The dialog for setting the FaceBasedPlacementType option of the face based family instance.
    /// </summary>
    public partial class FacebasedForm : System.Windows.Forms.Form
    {
        List<FamilySymbol> m_familySymbolList;
        FamilySymbol m_selectedSymbol;
        PromptForFamilyInstancePlacementOptions m_placementOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        public FacebasedForm(List<FamilySymbol> symbolList)
        {
            InitializeComponent();

            radioButtonFace.Checked = true;
            m_placementOptions = new PromptForFamilyInstancePlacementOptions();
            m_placementOptions.FaceBasedPlacementType = FaceBasedPlacementType.PlaceOnFace;

            m_familySymbolList = symbolList;
            List<String> nameList = new List<string>();
            foreach (FamilySymbol symbol in m_familySymbolList)
            {
                nameList.Add(symbol.Name);
            }
            comboBoxFamilySymbol.DataSource = nameList;
            comboBoxFamilySymbol.SelectedIndex = 0;
            m_selectedSymbol = m_familySymbolList[comboBoxFamilySymbol.SelectedIndex];
        }

        /// <summary>
        /// The family instance placement options for placement.
        /// </summary>
        public PromptForFamilyInstancePlacementOptions FIPlacementOptions
        {
            get
            {
                return m_placementOptions;
            }
        }

        /// <summary>
        /// The family symbol for placement.
        /// </summary>
        public FamilySymbol SelectedFamilySymbol
        {
            get
            {
                return m_selectedSymbol;
            }
        }

        /// <summary>
        /// Use the FaceBasedPlacementType.Default option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButtonDefault_CheckedChanged(object sender, EventArgs e)
        {
            m_placementOptions.FaceBasedPlacementType = FaceBasedPlacementType.Default;
        }

        /// <summary>
        /// Use the FaceBasedPlacementType.PlaceOnFace option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButtonFace_CheckedChanged(object sender, EventArgs e)
        {
            m_placementOptions.FaceBasedPlacementType = FaceBasedPlacementType.PlaceOnFace;
        }

        /// <summary>
        /// Use the FaceBasedPlacementType.PlaceOnVerticalFace option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButtonVF_CheckedChanged(object sender, EventArgs e)
        {
            m_placementOptions.FaceBasedPlacementType = FaceBasedPlacementType.PlaceOnVerticalFace;
        }

        /// <summary>
        /// Use the FaceBasedPlacementType.PlaceOnWorkPlane option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButtonWP_CheckedChanged(object sender, EventArgs e)
        {
            m_placementOptions.FaceBasedPlacementType = FaceBasedPlacementType.PlaceOnWorkPlane;
        }

        /// <summary>
        /// Select the family symbol for family instance placement.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void comboBoxFamilySymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_selectedSymbol = m_familySymbolList[comboBoxFamilySymbol.SelectedIndex];
        }
    }
}
