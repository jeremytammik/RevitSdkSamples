//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.GenerateFloor.CS
{
    /// <summary>
    /// User interface.
    /// </summary>
    public partial class GenerateFloorForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// the data get/set with revit. 
        /// </summary>
        private Data m_data;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="data"></param>
        public GenerateFloorForm(Data data)
        {
            m_data = data;
            InitializeComponent();            
        }

        /// <summary>
        /// paint the floor's profile.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewPictureBox_Paint(object sender, PaintEventArgs e)
        {
            double maxLength = previewPictureBox.Width > previewPictureBox.Height ? previewPictureBox.Width : previewPictureBox.Height;
            float scale = (float)(maxLength / m_data.MaxLength * 0.8);
            e.Graphics.ScaleTransform(scale, scale);
            e.Graphics.DrawLines(new Pen(System.Drawing.Color.Red, 1), m_data.Points);
        }

        /// <summary>
        /// initialize the data binding with revit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateFloorForm_Load(object sender, EventArgs e)
        {
            floorTypesComboBox.DataSource = m_data.FloorTypesName;
            m_data.ChooseFloorType(floorTypesComboBox.Text);
        }

        /// <summary>
        /// set the floor type to be create.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void floorTypesComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            m_data.ChooseFloorType(floorTypesComboBox.Text);
        }

        /// <summary>
        /// set if the floor to be create is structural.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void structralCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_data.Structural = structuralCheckBox.Checked;
        }
    }
}