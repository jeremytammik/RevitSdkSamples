//
//////////////////////////////////////////////////////////////////////////////
//
//  Copyright 2015 Autodesk, Inc.  All rights reserved.
//
//  Use of this software is subject to the terms of the Autodesk license 
//  agreement provided at the time of installation or download, or which 
//  otherwise accompanies this software in either electronic or hard copy form.   
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AstSTEELAUTOMATIONLib;

namespace SampleDesign
{
	public partial class Settings : UserControl
	{

		#region Constructor

		public Settings()
		{
			InitializeComponent();
			this.Visible = false;

			cbReportFormat.Items.Clear();
			cbReportFormat.Items.Add("RTF");
			cbReportFormat.Items.Add("HTML");
		}

		#endregion

		#region Private_Members

		private INodeStressAppModule m_pCurSMRule;

		#endregion

		#region Public_Methods

		public void setJoint(INodeStressAppModule pVal)
		{
			m_pCurSMRule = pVal;


			int nSelIndex = -1;
			try
			{
				nSelIndex = (int)m_pCurSMRule.Joint.GetJointNSAModuleSpecificData(m_pCurSMRule, "ReportFormat");
			}
			catch (Exception)
			{
				m_pCurSMRule.Joint.SetJointNSAModuleSpecificData(m_pCurSMRule, 0, "ReportFormat");
			}
			if (nSelIndex > -1 && cbReportFormat.Items.Count > nSelIndex)
				cbReportFormat.SelectedIndex = nSelIndex;
		}
		#endregion

		#region Private_Methods
		private void cbReportFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_pCurSMRule.Joint.SetJointNSAModuleSpecificData(m_pCurSMRule, this.cbReportFormat.SelectedIndex, "ReportFormat");
		}
		#endregion
	}
}
