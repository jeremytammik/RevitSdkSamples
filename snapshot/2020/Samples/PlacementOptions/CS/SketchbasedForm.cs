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
    /// The dialog for setting the SketchGalleryOptions option of the face based family instance.
    /// </summary>
    public partial class SketchbasedForm : System.Windows.Forms.Form
    {
        List<FamilySymbol> m_familySymbolList;
        FamilySymbol m_selectedSymbol;
        PromptForFamilyInstancePlacementOptions m_placementOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        public SketchbasedForm(List<FamilySymbol> symbolList)
        {
            InitializeComponent();

            m_placementOptions = new PromptForFamilyInstancePlacementOptions();
            m_placementOptions.SketchGalleryOptions = SketchGalleryOptions.SGO_Line;

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
        /// Select the family symbol for family instance placement.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void comboBoxFamilySymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_selectedSymbol = m_familySymbolList[comboBoxFamilySymbol.SelectedIndex];
        }

        /// <summary>
        /// Use the SketchGalleryOptions.SGO_Line option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButtonLine_CheckedChanged(object sender, EventArgs e)
        {
            m_placementOptions.SketchGalleryOptions = SketchGalleryOptions.SGO_Line;
        }

        /// <summary>
        /// Use the SketchGalleryOptions.SGO_Arc3Point option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButtonArc3P_CheckedChanged(object sender, EventArgs e)
        {
            m_placementOptions.SketchGalleryOptions = SketchGalleryOptions.SGO_Arc3Point;
        }

        /// <summary>
        /// Use the SketchGalleryOptions.SGO_ArcCenterEnds option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButtonArcC_CheckedChanged(object sender, EventArgs e)
        {
            m_placementOptions.SketchGalleryOptions = SketchGalleryOptions.SGO_ArcCenterEnds;
        }

        /// <summary>
        /// Use the SketchGalleryOptions.SGO_Spline option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButtonSpline_CheckedChanged(object sender, EventArgs e)
        {
            m_placementOptions.SketchGalleryOptions = SketchGalleryOptions.SGO_Spline;
        }

        /// <summary>
        /// Use the SketchGalleryOptions.SGO_PartialEllipse option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButtonPEllipse_CheckedChanged(object sender, EventArgs e)
        {
            m_placementOptions.SketchGalleryOptions = SketchGalleryOptions.SGO_PartialEllipse;
        }

        /// <summary>
        /// Use the SketchGalleryOptions.SGO_PickLines option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButtonPickLine_CheckedChanged(object sender, EventArgs e)
        {
            m_placementOptions.SketchGalleryOptions = SketchGalleryOptions.SGO_PickLines;
        }
    }
}
