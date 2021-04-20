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
    /// create model line options form
    /// </summary>
    public partial class CreateModelLineOptionsForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public CreateModelLineOptionsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// constructor of CreateModelLineOptionsForm
        /// </summary>
        /// <param name="openingInfos">a list of OpeningInfo</param>
        /// /// <param name="selectOpening">current displayed (in preview) Opening</param>
        public CreateModelLineOptionsForm(List<OpeningInfo> openingInfos, 
            OpeningInfo selectOpening)
        {
            InitializeComponent();

            m_openingInfos = openingInfos;
            m_selectedOpeningInfo = selectOpening;
        }

        private List<OpeningInfo> m_openingInfos; //store all the OpeningInfo class
        private OpeningInfo m_selectedOpeningInfo; //current displayed (in preview) OpeningInfo

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (CreateDisplayRadioButton.Checked)
            {
                m_selectedOpeningInfo.BoundingBox.CreateLines(m_selectedOpeningInfo.Revit);
            }
            else if (CreateAllRadioButton.Checked)
            {
                foreach (OpeningInfo openingInfo in m_openingInfos)
                {
                    openingInfo.BoundingBox.CreateLines(m_selectedOpeningInfo.Revit);
                }
            }
            else if (CreateShaftRadioButton.Checked)
            {
                foreach (OpeningInfo openingInfo in m_openingInfos)
                {
                    if (openingInfo.IsShaft)
                    {
                        openingInfo.BoundingBox.CreateLines(m_selectedOpeningInfo.Revit);
                    }
                }
            }

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}