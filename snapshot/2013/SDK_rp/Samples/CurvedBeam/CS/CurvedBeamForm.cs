//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.CurvedBeam.CS
{
    /// <summary>
    /// new beam form
    /// </summary>
    public partial class CurvedBeamForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// default construction is forbidden
        /// </summary>
        private CurvedBeamForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// get relevant data from Revit
        /// </summary>
        /// <param name="dataBuffer">relevant review data</param>
        public CurvedBeamForm(Command dataBuffer)
        {
            m_dataBuffer = dataBuffer;
            InitializeComponent();
        }
        
        /// <summary>
        /// create Arc beam
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newArcButton_Click(object sender, EventArgs e)
        {
            Level locLev = LevelCB.SelectedValue as Level;
            Arc beamArc = m_dataBuffer.CreateArc(locLev.Elevation);
            bool succeed = m_dataBuffer.CreateCurvedBeam(BeamTypeCB.SelectedValue as FamilySymbol,
                beamArc, locLev);
            if (succeed)
            {
                MessageBox.Show("Succeeded to create Arc beam.", "Revit");
            }
        }


        /// <summary>
        /// create Nurbspline beam
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newNurbSplineButton_Click(object sender, EventArgs e)
        {
            Level locLev = LevelCB.SelectedValue as Level;
            NurbSpline beamNs = m_dataBuffer.CreateNurbSpline(locLev.Elevation);
            bool succeed = m_dataBuffer.CreateCurvedBeam(BeamTypeCB.SelectedValue as FamilySymbol,
                beamNs, locLev);
            if (succeed)
            {
                MessageBox.Show("Succeeded to create NurbSpline beam.", "Revit");
            }
        }


        /// <summary>
        /// create nurb spline beam
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newEllipseButton_Click(object sender, EventArgs e)
        {
            Level locLev = LevelCB.SelectedValue as Level;
            Ellipse beamEllipse = m_dataBuffer.CreateEllipse(locLev.Elevation);
            bool succeed = m_dataBuffer.CreateCurvedBeam(BeamTypeCB.SelectedValue as FamilySymbol,
                beamEllipse, locLev);
            if (succeed)
            {
                MessageBox.Show("Succeeded to create Ellipse beam.", "Revit");
            }
        }
    }
}