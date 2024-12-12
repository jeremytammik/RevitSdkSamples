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
using System.Diagnostics;
using System.Runtime.InteropServices;

using AstSTEELAUTOMATIONLib;
using JointDesignUtils;


namespace SampleDesign
{
	[ComVisible(true)]
	[Guid("C64ECF63-D01C-451F-B0E0-E3F85DFAC05E")]

    public class CreatePlateDesign : INodeStressAppModule
    {
		#region Private_Members
		private StressModuleJoint m_joint;

		private Torsors m_pTorsorsPage;
		private SimpleTorsors m_SimpleTorsorsPage;
		private Settings m_SettingsPage;

		#endregion

		#region Standard_Methods

		public eJointStatus CheckJoint()
		{
			eJointStatus ret = eJointStatus.JointSupported;

			//report format
			int nReportFormat = (int)getJointNSAModuleSpecificData(m_joint, this, "ReportFormat", 0);

			//get efforts
			List<double> dM, dN, dV;
			List<string> sCaseName;
			getEfforts(m_joint, m_joint.GetJointNSAModuleLoadingCases(this), m_joint.UseDetailedTorsors, out dM, out dN, out dV, out sCaseName);


			//read some values from joint
			bool bModifiable = false;
			double dPlateThickness = (double)m_joint.GetJointParam("PlateThickness", ref bModifiable);
            double dPlateLength = (double)m_joint.GetJointParam("PlateLength", ref bModifiable);
            double dPlateWidth = (double)m_joint.GetJointParam("PlateWidth", ref bModifiable);


			return ret;
		}

		public bool DesignEngineAvailable()
		{
			return true;
		}

		public eJointStatus ExportJoint()
		{
			return eJointStatus.JointUnknownStatus;
		}

		public void FreeUserPages()
		{
			if (m_pTorsorsPage != null)
			{
				m_pTorsorsPage.Dispose();
				m_pTorsorsPage = null;
			}
			if (m_SettingsPage != null)
			{
				m_SettingsPage.Dispose();
				m_SettingsPage = null;
			}
		}

		public void FreeUserTorsorsPage()
		{
			if (m_SimpleTorsorsPage != null)
			{
				m_SimpleTorsorsPage.Dispose();
				m_SimpleTorsorsPage = null;
			}
		}

		public string GetExportName()
		{
			return "";
		}

		public bool GetFeatureName(ref string FeatureName)
		{
			FeatureName = "JOINT_DESIGN_EC3";
			return true;
		}

		public string GetImportName()
		{
			return "";
		}

		public string GetLastReportFilename(bool bQuickReportFilename)
		{
			if (bQuickReportFilename)
				return m_joint.GetModuleOutputFileName(this) + ".txt";
			else
				return m_joint.GetModuleOutputFileName(this) + ".html";
		}

		public void GetProcessingModes(out bool bCheckAvailable, out bool bPresizeAvailable, out bool bImportAvailable, out bool bExportAvailable, out bool bUseLoadCasesAvailable)
		{
			bCheckAvailable = true;
			bPresizeAvailable = false;
			bImportAvailable = false;
			bExportAvailable = false;
			bUseLoadCasesAvailable = true;
		}

		public void GetUserPages(RulePageArray pagesRet, PropertySheetData pPropSheetData)
		{
			m_pTorsorsPage = new Torsors();
			m_pTorsorsPage.setJoint(this);

			RulePage pgTorsors = m_joint.CreateRulePage();
			pgTorsors.title = 88136;
			pgTorsors.hWnd = (int)m_pTorsorsPage.Handle;
			pagesRet.Add(pgTorsors);
		}

		public void GetUserSettingsPages(RulePageArray pagesSettingsRet, PropertySheetData pPropSheetData)
		{
			pPropSheetData.SheetPrompt = 88140;

			m_SettingsPage = new Settings();
			m_SettingsPage.setJoint(this);

			RulePage pgSettings = m_joint.CreateRulePage();
			pgSettings.title = 88140;
			pgSettings.hWnd = (int)m_SettingsPage.Handle;
			pagesSettingsRet.Add(pgSettings);
		}

		public void GetUserTorsorsPage(RulePage pageRet)
		{
			m_SimpleTorsorsPage = new SimpleTorsors();
			m_SimpleTorsorsPage.setJoint(this);
			pageRet.hWnd = (int)m_SimpleTorsorsPage.Handle;
		}

		public eJointStatus ImportJoint()
		{
			return eJointStatus.JointUnknownStatus;
		}

		public void InvalidFeature(int reserved)
		{

		}

		public StressModuleJoint Joint
		{
			get
			{
				return m_joint;
			}
			set
			{
				m_joint = value;
			}
		}

		public string ModuleName
		{
			get { return "EC3 Design"; }
		}

		public eJointStatus PresizeJoint()
		{
			return eJointStatus.JointUnknownStatus;
		}

		public string Standard
		{
			get
			{
				return "Standard";
			}
			set
			{

			}
		}

		#endregion

		#region Private_Methods

		private object getJointNSAModuleSpecificData(StressModuleJoint pCurJoint, INodeStressAppModule NSAModule, string sKey, object defaultValue)
		{
			object param = defaultValue;

			try
			{
				param = pCurJoint.GetJointNSAModuleSpecificData(NSAModule, sKey);
			}
			catch (Exception e)
			{
				Debug.Write(e.Message);
				try
				{
					pCurJoint.SetJointNSAModuleSpecificData(NSAModule, defaultValue, sKey);
				}
				catch (Exception ex)
				{
					Debug.Write(ex.Message);
				}
			}

			return param;
		}

		private void getEfforts(StressModuleJoint pCurJoint, IJointLoadingCases spJointLoadingCases, bool bUseDetailedTorsors,
								out List<double> dfEffM, out List<double> dfEffN, out List<double> dfEffV, out  List<string> sCaseName)
		{

			dfEffM = new List<double>();
			dfEffN = new List<double>();
			dfEffV = new List<double>();
			sCaseName = new List<string>();

			//get the efforts from SIMPLE TORSORS
			try
			{
				if (null != spJointLoadingCases && false == bUseDetailedTorsors)
				{
					if ((int)spJointLoadingCases.Count > 0)
					{
						for (int i = 0; i < (int)spJointLoadingCases.Count; i++)
						{
							IJointLoadingCase spJointLoadingCase = spJointLoadingCases[i];
							IJointEfforts spJointEfforts = spJointLoadingCase.JointEfforts;

							if (null != spJointEfforts && spJointLoadingCase.Name == "SimpleMaxTorsor")
							{
								if (3 == (int)spJointEfforts.Count)
								{
									sCaseName.Add(spJointLoadingCase.Name);
									dfEffM.Add((double)spJointEfforts[0].Value);
									dfEffN.Add((double)spJointEfforts[1].Value);
									dfEffV.Add((double)spJointEfforts[2].Value);
								}
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.Write(e);
			}


			//get the efforts from DETAILED TORSORS
			try
			{
				char[] splitChars = new char[2];
				splitChars[0] = '#';
				splitChars[1] = '@';

				if (bUseDetailedTorsors)
				{
					if (null != spJointLoadingCases)
					{
						if ((int)spJointLoadingCases.Count > 0)
						{
							for (int i = 0; i < (int)spJointLoadingCases.Count; i++)
							{
								IJointLoadingCase spJointLoadingCase = spJointLoadingCases[i];
								IJointEfforts spJointEfforts = spJointLoadingCase.JointEfforts;

								if (null != spJointEfforts && spJointLoadingCase.Name != "SimpleMaxTorsor")
								{
									if (3 == (int)spJointEfforts.Count)
									{
										string[] sSplitName = spJointLoadingCase.Name.Split(splitChars);
										sCaseName.Add(sSplitName[sSplitName.Length - 1]);
										dfEffM.Add((double)spJointEfforts[0].Value);
										dfEffN.Add((double)spJointEfforts[1].Value);
										dfEffV.Add((double)spJointEfforts[2].Value);
									}
								}
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.Write(e);
			}
		}

		public void InitializeDesignModuleData()
		{
			if (m_joint == null)
				return;

			Utils.InitializeEC3JointDesignData(m_joint, this);
		}
		#endregion
	}
}
