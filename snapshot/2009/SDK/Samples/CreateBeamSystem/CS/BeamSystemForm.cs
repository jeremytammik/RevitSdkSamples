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


namespace Revit.SDK.Samples.CreateBeamSystem.CS
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;

	/// <summary>
	/// display beam system to be created and allow user to set its properties
	/// </summary>
	public partial class BeamSystemForm : Form
	{
		/// <summary>
		/// buffer of data related to UI
		/// </summary>
		private BeamSystemData m_data;

		/// <summary>
		/// class to draw profile of beam system in PictureBox
		/// </summary>
		private BeamSystemSketch m_sketch;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="data">data related to UI</param>
		public BeamSystemForm(BeamSystemData data)
		{
			InitializeComponent();
			m_data = data;
			// bound PictureBox to display the profile
			m_sketch = new BeamSystemSketch(previewPictureBox);
		}

		/// <summary>
		/// update PropertyGrid when BeamSystemParams bound to beam system updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ParamsUpdated(object sender, EventArgs e)
		{
			beamSystemPropertyGrid.SelectedObject = null;
			beamSystemPropertyGrid.SelectedObject = m_data.Param;
		}

		/// <summary>
		/// form is loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BeamSystemForm_Load(object sender, EventArgs e)
		{
			// bound PropertyGrid to show beam system's properties
			beamSystemPropertyGrid.SelectedObject = m_data.Param;
			m_data.ParamsUpdated                 += new EventHandler(ParamsUpdated);
			// draw the profile
			m_sketch.DrawProfile(m_data.Lines);
		}

		/// <summary>
		/// to create beam system
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OKButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// cancel all command
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		/// <summary>
		/// change the direction of beam system to the next line in the profile
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void changeDirectionButton_Click(object sender, EventArgs e)
		{
			m_data.ChangeProfileDirection();
			m_sketch.DrawProfile(m_data.Lines);
		}
	}
}