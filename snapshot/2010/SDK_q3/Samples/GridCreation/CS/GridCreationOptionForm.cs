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

namespace Revit.SDK.Samples.GridCreation.CS
{
    /// <summary>
    /// The dialog which lets user choose the way to create grids
    /// </summary>
    public partial class GridCreationOptionForm : Form
    {
        // data class object
        private GridCreationOptionData m_gridCreationOption;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="opt">Data class object</param>
        public GridCreationOptionForm(GridCreationOptionData opt)
        {
            m_gridCreationOption = opt;

            InitializeComponent();
            // Set state of controls
            InitializeControls();            
        }

        /// <summary>
        /// Set state of controls
        /// </summary>
        private void InitializeControls()
        {
            if (!m_gridCreationOption.HasSelectedLinesOrArcs)
            {
                radioButtonSelect.Enabled = false;
                radioButtonOrthogonalGrids.Checked = true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Transfer data back into data class
            SetData();
        }

        /// <summary>
        /// Transfer data back into data class
        /// </summary>
        private void SetData()
        {
            m_gridCreationOption.CreateGridsMode = radioButtonSelect.Checked ? CreateMode.Select :
                (radioButtonOrthogonalGrids.Checked ? CreateMode.Orthogonal : CreateMode.RadialAndArc);
        }
    }
}