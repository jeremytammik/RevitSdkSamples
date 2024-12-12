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

namespace Revit.SDK.Samples.PlacementOptions.CS
{
    /// <summary>
    /// The dialog for choosing the face based family instance or sketch based family instance.
    /// </summary>
    public partial class OptionsForm : Form
    {
        PlacementOptionsEnum m_optionType;
        /// <summary>
        /// Constructor
        /// </summary>
        public OptionsForm()
        {
            InitializeComponent();

            radioButton1.Checked = true;
            m_optionType = PlacementOptionsEnum.FaceBased;
        }

        /// <summary>
        /// The option for choosing the face based family instance or sketch based family instance.
        /// </summary>
        public PlacementOptionsEnum OptionType
        {
            get
            {
                return m_optionType;
            }
        }

        /// <summary>
        /// Use the PlacementOptionsEnum.FaceBased option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            m_optionType = PlacementOptionsEnum.FaceBased;
        }

        /// <summary>
        /// Use the PlacementOptionsEnum.SketchBased option or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arg.</param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            m_optionType = PlacementOptionsEnum.SketchBased;
        }
    }
}
