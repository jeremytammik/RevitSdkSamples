//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.Openings.CS
{
    /// <summary>
    /// Main form use to show the selected opening.
    /// </summary>
    public partial class OpeningForm : System.Windows.Forms.Form
    {
        //constructor
        /// <summary>
        /// The default constructor
        /// </summary>
        public OpeningForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// constructor of OpeningForm
        /// </summary>
        /// <param name="openingInfos">a list of OpeningInFo</param>
        public OpeningForm(List<OpeningInfo> openingInfos)
        {
            InitializeComponent();

            m_openingInfos = openingInfos;
        }

        //private member
        private readonly List<OpeningInfo> m_openingInfos; //store all the OpeningInfo class
        private OpeningInfo m_selectedOpeningInfo; //current displayed (in preview) OpeningInfo

        private void OpeningForm_Load(object sender, EventArgs e)
        {
            this.OpeningListComboBox.DataSource = m_openingInfos;
            this.OpeningListComboBox.DisplayMember = "NameAndId";

            m_selectedOpeningInfo = (OpeningInfo)this.OpeningListComboBox.SelectedItem;
            this.OpeningPropertyGrid.SelectedObject = m_selectedOpeningInfo.Property;
        }

        private void PreviewPictureBox_Paint(object sender, PaintEventArgs e)
        {
            int width = this.PreviewPictureBox.Width;
            int height = this.PreviewPictureBox.Height;
            if (m_selectedOpeningInfo.Sketch != null)
            {
                m_selectedOpeningInfo.Sketch.Draw2D(width, 
                    height, e.Graphics);
            }
            else
            {
                //if profile is a circle (or ellipse), can not get curve from API
                //so draw an Arc according to boundingBox of the Opening
                double widthBoundBox = m_selectedOpeningInfo.BoundingBox.Width;
                double lengthBoundBox = m_selectedOpeningInfo.BoundingBox.Length;
                double scale = height * 0.8 / lengthBoundBox;
                e.Graphics.Clear(System.Drawing.Color.Black);
                Pen yellowPen = new Pen(System.Drawing.Color.Yellow, 1);
                Rectangle rect = new Rectangle((int)(width / 2 - widthBoundBox * scale / 2),
                    (int)(height / 2 - lengthBoundBox * scale / 2), (int)(widthBoundBox * scale),
                    (int)(lengthBoundBox * scale));
                // Draw circle to screen.
                e.Graphics.DrawArc(yellowPen, rect, 0, 360);
            }
        }

        private void Createbutton_Click(object sender, EventArgs e)
        {
            CreateModelLineOptionsForm optionForm = 
                new CreateModelLineOptionsForm(m_openingInfos, m_selectedOpeningInfo);
            optionForm.ShowDialog();
        }

        private void OpeningListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_selectedOpeningInfo = (OpeningInfo)this.OpeningListComboBox.SelectedItem;
            this.OpeningPropertyGrid.SelectedObject = m_selectedOpeningInfo.Property;
            this.PreviewPictureBox.Refresh();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}