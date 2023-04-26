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
using Autodesk.AdvanceSteel.DotNetRoots.DatabaseAccess;

namespace SampleDesign
{
	public partial class Torsors : UserControl
	{
		#region Constructor

		public Torsors()
		{
			InitializeComponent();
			this.Visible = false;
		}

		#endregion

		#region private members

		private CreatePlateDesign m_JointDesignSample;

		#endregion

		public void setJoint(CreatePlateDesign pVal)
		{
			m_JointDesignSample = pVal;

			if (m_JointDesignSample != null)
				callFillGridData(m_JointDesignSample);
		}

		private void axTorsors_OnGridCellEdited(object sender, AxASTCONTROLSLib._DAstOCXGridEvents_OnGridCellEditedEvent e)
		{
			int nRow = axTorsors.GetFocusCellRow();
			int nCol = axTorsors.GetFocusCellColumn();

			if (m_JointDesignSample != null)
				callGridCellEditedTorsors(m_JointDesignSample, nRow, nCol);
		}

		private void btnAddLoading_Click(object sender, EventArgs e)
		{
			if (m_JointDesignSample != null)
				callAddLoading(m_JointDesignSample);
		}

		private void btnDelLoading_Click(object sender, EventArgs e)
		{
			if (m_JointDesignSample != null)
				callDelLoading(m_JointDesignSample);
		}

		private void callAddLoading(INodeStressAppModule m_pCurSMRule)
		{
			//add new loading in joint
			IStressModuleJoint curSMJoint = m_pCurSMRule.Joint;

			//get now the module loading cases / efforts
			JointLoadingCases spJointLoadingCases = curSMJoint.GetJointNSAModuleLoadingCases(m_pCurSMRule);
			if (null != spJointLoadingCases)
			{
				JointLoadingCase spNewCase;
				JointEfforts spNewEfforts;
				JointEffort spNewEffortM, spNewEffortN, spNewEffortV;

				spNewEffortM = curSMJoint.CreateJointEffort();
				spNewEffortN = curSMJoint.CreateJointEffort();
				spNewEffortV = curSMJoint.CreateJointEffort();
				spNewEfforts = curSMJoint.CreateJointEfforts();
				spNewCase = curSMJoint.CreateJointLoadingCase();

				double newVal = 0.0;

				spNewEffortM.Name = "M";
				spNewEffortM.Value = newVal;

				spNewEffortN.Name = "N";
				spNewEffortN.Value = newVal;

				spNewEffortV.Name = "V";
				spNewEffortV.Value = newVal;

				spNewEfforts.Add(spNewEffortM);
				spNewEfforts.Add(spNewEffortN);
				spNewEfforts.Add(spNewEffortV);

				string strLoadingName = createInternalLoadingName("-");

				spNewCase.Name = strLoadingName;
				spNewCase.JointEfforts = spNewEfforts;

				spJointLoadingCases.Add(spNewCase);
				curSMJoint.SetJointNSAModuleLoadingCases(m_pCurSMRule, spJointLoadingCases);
			}


			//add new line in grid
			string lineHeader = getNextLineHeader().ToString();
			axTorsors.InsertRow(ref lineHeader, axTorsors.RowCount - 1);

			//description
			axTorsors.set_ItemTextValue(axTorsors.RowCount - 1, 1, "-");
			//M
			axTorsors.set_ItemDoubleValue(axTorsors.RowCount - 1, 2, 0);
			//N
			axTorsors.set_ItemDoubleValue(axTorsors.RowCount - 1, 3, 0);
			//V
			axTorsors.set_ItemDoubleValue(axTorsors.RowCount - 1, 4, 0);
		}

		private void callDelLoading(INodeStressAppModule m_pCurSMRule)
		{
			int selectedRow = axTorsors.GetFocusCellRow();

			if (selectedRow > (axTorsors.FixedRowCount - 1))
			{
				//get the real index of the torsor in the NSA module data
				int realTorsorIndex = getTorsorIndex(selectedRow, 0);
				if (-1 == realTorsorIndex)
					return;

				IStressModuleJoint curSMJoint = m_pCurSMRule.Joint;
				//get now the module loading cases / efforts
				IJointLoadingCases spJointLoadingCases = curSMJoint.GetJointNSAModuleLoadingCases(m_pCurSMRule);

				if (null != spJointLoadingCases)
				{
					if ((int)spJointLoadingCases.Count > 0)
					{
						JointLoadingCases spNewCases = curSMJoint.CreateJointLoadingCases();

						if (null != spNewCases)
						{
							for (int i = 0; i < (int)spJointLoadingCases.Count; i++)
							{
								if (i != realTorsorIndex) //remove selected torsor
								{
									JointLoadingCase spOldCase = spJointLoadingCases[i];
									spNewCases.Add(spOldCase);
								}
							}

							//set back the remaining torsors
							curSMJoint.SetJointNSAModuleLoadingCases(m_pCurSMRule, spNewCases);
						}
						else
							return;
					}
					else
						return;
				}
				else
					return;

				axTorsors.DeleteRow(selectedRow);
			}
		}

		private void callGridCellEditedTorsors(INodeStressAppModule m_pCurSMRule, int nRow, int nCol)
		{
			//get the real index of the torsor in the NSA module data
			int realTorsorIndex = getTorsorIndex(nRow, nCol);
			if (-1 == realTorsorIndex)
				return;

			IStressModuleJoint curSMJoint = m_pCurSMRule.Joint;

			//get now the module loading cases
			try
			{
				JointLoadingCases spJointLoadingCases = curSMJoint.GetJointNSAModuleLoadingCases(m_pCurSMRule);

				if (null != spJointLoadingCases)
				{
					if ((int)spJointLoadingCases.Count > 0)
					{
						IJointLoadingCase spJointLoadingCase = spJointLoadingCases[realTorsorIndex];

						string strCaseName = spJointLoadingCase.Name;
						IJointEfforts spJointEfforts = spJointLoadingCase.JointEfforts;

						if (null != spJointEfforts)
						{
							if (3 == (int)spJointEfforts.Count)
							{
								IJointEffort spEffortM, spEffortN, spEffortV; //M, N, V

								spEffortM = spJointEfforts[0];
								spEffortN = spJointEfforts[1];
								spEffortV = spJointEfforts[2];

								if (1 == nCol) // "Description" was modified
								{
									string strNew = axTorsors.get_ItemTextValue(nRow, nCol);
									spJointLoadingCase.Name = createInternalLoadingName(strNew);
								}
								else // "M", "N" or "V"	modified
								{
									double dfNew = axTorsors.get_ItemDoubleValue(nRow, nCol);
									if (2 == nCol) // "M"
										spEffortM.Value= dfNew;
									if (3 == nCol) // "N"
										spEffortN.Value = dfNew;
									if (4 == nCol) // "V"
										spEffortV.Value = dfNew;
								}
							}
						}

					}
					//set back joint loading cases
					curSMJoint.SetJointNSAModuleLoadingCases(m_pCurSMRule, spJointLoadingCases);
				} 
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.Write(e);
			}
		}


		private void callFillGridData(INodeStressAppModule m_pCurSMRule)
		{
			//fill torsors grid
			axTorsors.DeleteAllItems();
			axTorsors.RowCount = 1;
			axTorsors.ColumnCount = 5;
			axTorsors.FixedRowCount = 1;
			axTorsors.FixedColumnCount = 1;

			IStressModuleJoint curSMJoint = m_pCurSMRule.Joint;

			//get current units for Moment and Force
			IUnitsManager units = curSMJoint.Units;
			IUnit momentUnit = (IUnit)(new DSCROOTSCOMLib.UnitClass());
			IUnit forceUnit = (IUnit)(new DSCROOTSCOMLib.UnitClass());
			units.getUnitOfMoment(momentUnit);
			units.getUnitOfForce(forceUnit);

			string strMomentUnit = "???", strForceUnit = "???";
			strMomentUnit = momentUnit.getSymbol();
			strForceUnit = forceUnit.getSymbol();

			string strM_txt = AstCrtlDb.Instance.readUPS_PRP(41200);//"M"
			string strN_txt = AstCrtlDb.Instance.readUPS_PRP(41201);//"N"
			string strV_txt = AstCrtlDb.Instance.readUPS_PRP(41202);//"V"

			string strCol_M_Name = strM_txt + " (" + strMomentUnit + ")";
			string strCol_N_Name = strN_txt + " (" + strForceUnit + ")";
			string strCol_V_Name = strV_txt + " (" + strForceUnit + ")";

			string strIndexName = "Case", strDescriptionName = "Name";
			strDescriptionName = AstCrtlDb.Instance.readUPS_PRP(147);
			strIndexName = AstCrtlDb.Instance.readUPS_PRP(146);

			axTorsors.set_ItemTextValue(0, 0, strIndexName);
			axTorsors.set_ItemTextValue(0, 1, strDescriptionName);
			axTorsors.set_ItemTextValue(0, 2, strCol_M_Name);
			axTorsors.set_ItemTextValue(0, 3, strCol_N_Name);
			axTorsors.set_ItemTextValue(0, 4, strCol_V_Name);

			axTorsors.set_ColumnWidth(0, 50);
			axTorsors.set_ColumnWidth(1, 95);
			axTorsors.set_ColumnWidth(2, 50);
			axTorsors.set_ColumnWidth(3, 50);
			axTorsors.set_ColumnWidth(4, 50);

      //column "Description"
      axTorsors.set_ColumnType(1, ASTCONTROLSLib.eColumnType.kEditBox); //kEditBox
      axTorsors.set_ColumnDataType(1, ASTCONTROLSLib.eCellDataType.kString); //kString

      //column "M"
      axTorsors.set_ColumnType(2, ASTCONTROLSLib.eColumnType.kEditBox); //kEditBox
      axTorsors.set_ColumnDataType(2, ASTCONTROLSLib.eCellDataType.kUnits); //kUnits
      axTorsors.set_ColumnUnits(2, ASTCONTROLSLib.eCellUnitType.kUnitMoment); //kUnitMoment

      //column "N"
      axTorsors.set_ColumnType(3, ASTCONTROLSLib.eColumnType.kEditBox); //kEditBox
      axTorsors.set_ColumnDataType(3, ASTCONTROLSLib.eCellDataType.kUnits); //kUnits
      axTorsors.set_ColumnUnits(3, ASTCONTROLSLib.eCellUnitType.kUnitForce); //kUnitForce

      //column "V"
      axTorsors.set_ColumnType(4, ASTCONTROLSLib.eColumnType.kEditBox); //kEditBox
      axTorsors.set_ColumnDataType(4, ASTCONTROLSLib.eCellDataType.kUnits); //kUnits
      axTorsors.set_ColumnUnits(4, ASTCONTROLSLib.eCellUnitType.kUnitForce); //kUnitForce


			//get now the module loading cases
			IJointLoadingCases spJointLoadingCases = curSMJoint.GetJointNSAModuleLoadingCases(m_pCurSMRule);

			if (null != spJointLoadingCases)
			{
				if ((int)spJointLoadingCases.Count > 0)
				{
					for (int i = 0; i < (int)spJointLoadingCases.Count; i++)
					{
						IJointLoadingCase spJointLoadingCase = spJointLoadingCases[i];
						IJointEfforts spJointEfforts = spJointLoadingCase.JointEfforts;
						string strCaseName = spJointLoadingCase.Name;

						if ((null != spJointEfforts) &&(strCaseName != "SimpleMaxTorsor"))
						{
							if (3 == (int)spJointEfforts.Count)
							{
								JointEffort spEffortM, spEffortN, spEffortV; //M, N, V
								double dfEffM, dfEffN, dfEffV;

								spEffortM = spJointEfforts[0];
								spEffortN = spJointEfforts[1];
								spEffortV = spJointEfforts[2];

								dfEffM = (double)spEffortM.Value;
								dfEffN = (double)spEffortN.Value;
								dfEffV = (double)spEffortV.Value;

								//add new line in grid
								string strLineHeader = getNextLineHeader().ToString();
								axTorsors.InsertRow(ref strLineHeader, axTorsors.RowCount - 1);

								//description
								axTorsors.set_ItemTextValue(axTorsors.RowCount - 1, 1, getUserLoadingName(strCaseName));
								//M
								axTorsors.set_ItemDoubleValue(axTorsors.RowCount - 1, 2, dfEffM);
								//N
								axTorsors.set_ItemDoubleValue(axTorsors.RowCount - 1, 3, dfEffN);
								//T
								axTorsors.set_ItemDoubleValue(axTorsors.RowCount - 1, 4, dfEffV);

							}
						}
					}
				}
			}

			axTorsors.AutoSize();
			axTorsors.GridBkColor = SystemColors.Window;
		}

		private long getNextLineHeader()
		{
			long nextLineNumber = 0;

			//add a line with the next largest number
			long existingMax = 0;

			nextLineNumber = axTorsors.RowCount;

			//get the existing max number
			long rowCount = axTorsors.RowCount;
			if (rowCount > 1)
			{
				for (int i = 1; i < rowCount; i++)
				{
					int thisVal = 0;
					thisVal = int.Parse(axTorsors.get_ItemTextValue(i, 0));
					if (thisVal > existingMax)
						existingMax = thisVal;
				}
			}

			if (existingMax >= axTorsors.RowCount)
				nextLineNumber = existingMax + 1;

			return nextLineNumber;
		}

		private int getTorsorIndex(int gridRow, int gridCol)
		{
			int result = -1;

			if (m_JointDesignSample != null)
				result = callGetTorsorsIndex(m_JointDesignSample, gridRow, gridCol);

			return result;
		}

		private int callGetTorsorsIndex(INodeStressAppModule m_pCurSMRule, int gridRow, int gridCol)
		{
			int result = -1;

			IStressModuleJoint curSMJoint = m_pCurSMRule.Joint;

			//get now the module loading cases
			IJointLoadingCases spJointLoadingCases = curSMJoint.GetJointNSAModuleLoadingCases(m_pCurSMRule);

			if (null != spJointLoadingCases)
			{
				if ((int)spJointLoadingCases.Count > 0)
				{
					int nLocatingIndex = 0;

					for (int i = 0; i < (int)spJointLoadingCases.Count; i++)
					{
						JointLoadingCase spJointLoadingCase = spJointLoadingCases[i];
						JointEfforts spJointEfforts = spJointLoadingCase.JointEfforts;

						if ((null != spJointEfforts) && (spJointLoadingCase.Name != "SimpleMaxTorsor"))
						{
							if ((gridRow - axTorsors.FixedRowCount) == nLocatingIndex) //found the index we want
							{
								result = i;
							}
							nLocatingIndex++;
						}
					}
				}
			} 

			return result;
		}

		private string createInternalLoadingName(string strLoadingName)
		{
			string strInternalName = string.Format("Beam {0:D} #@#@#", 1);
			strInternalName = strInternalName + strLoadingName;
			return strInternalName;
		}

		private string getUserLoadingName(string strInternalName)
		{
			int nStartIndex = 12;
			bool bSeparatorExists = strInternalName.Contains("#@#@#");
			bool bLongSeparatorExists = strInternalName.Contains("#@#@#@#");
			if (bLongSeparatorExists)
			{
				bSeparatorExists = true;
				nStartIndex = 14;
			}
			if (bSeparatorExists)
				return strInternalName.Substring(nStartIndex);
			else
				return strInternalName;
		}
	}
}
