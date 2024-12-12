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
using AxASTCONTROLSLib;

namespace SampleDesign
{
	public partial class SimpleTorsors : UserControl
	{
		#region private members

		private CreatePlateDesign m_CreatePlateDesign;
		double m_dMaxM;
		double m_dMaxN;
		double m_dMaxV;

		#endregion

		#region Constructors

		public SimpleTorsors()
		{
			InitializeComponent();
			this.Visible = false;

			m_dMaxM = 0;
			m_dMaxN = 0;
			m_dMaxV = 0;

			m_CreatePlateDesign = null;
		}

		public void setJoint(CreatePlateDesign pVal)
		{
			m_dMaxM = 0;
			m_dMaxN = 0;
			m_dMaxV = 0;

			m_CreatePlateDesign = pVal;
			setJoint(m_CreatePlateDesign, 1);
			setEnable(1, m_CreatePlateDesign);
		}

		#endregion

		#region Private_Methods

		private void axSimpleTorsors_OnGridCellEdited(object sender, _DAstOCXGridEvents_OnGridCellEditedEvent e)
		{
			double newM = 0;
			double newN = 0;
			double newT = 0;

			int nRow = axSimpleTorsors.GetFocusCellRow();

			newM = axSimpleTorsors.get_ItemDoubleValue(nRow, 0);
			newN = axSimpleTorsors.get_ItemDoubleValue(nRow, 1);
			newT = axSimpleTorsors.get_ItemDoubleValue(nRow, 2);

			if (1 == nRow)
				setJointTorsorsValues(m_CreatePlateDesign, newM, newN, newT, "SimpleMaxTorsor");
		}

		private void setJointTorsorsValues(INodeStressAppModule m_pCurSMRule, double dMaxM, double dMaxN, double dMaxT, string sLoadingCaseName)
		{
			IStressModuleJoint curSMJoint = m_pCurSMRule.Joint;

			bool bFoundLoadingCase = false;

			//get now the module loading cases
			JointLoadingCases spJointLoadingCases = curSMJoint.GetJointNSAModuleLoadingCases(m_pCurSMRule);

			if (null != spJointLoadingCases)
			{
				for (int i = 0; i < (int)spJointLoadingCases.Count; i++)
				{
					IJointLoadingCase spJointLoadingCase = spJointLoadingCases[i];
					IJointEfforts spJointEfforts = spJointLoadingCase.JointEfforts;

					if ((null != spJointEfforts) && (spJointLoadingCase.Name == sLoadingCaseName))
					{
						if (3 == (int)spJointEfforts.Count)
						{
							spJointEfforts[0].Value = dMaxM;
							spJointEfforts[1].Value = dMaxN;
							spJointEfforts[2].Value = dMaxT;
							bFoundLoadingCase = true;
						}
					}
				}

				//set back joint loading cases
				curSMJoint.SetJointNSAModuleLoadingCases(m_pCurSMRule, spJointLoadingCases);
			}

			if (!bFoundLoadingCase)
			{
				//create loading case
				JointLoadingCase spJointLoadingCase = curSMJoint.CreateJointLoadingCase();

				//create joint efforts
				JointEfforts spJointEfforts = curSMJoint.CreateJointEfforts();

				JointEffort spEffortM = curSMJoint.CreateJointEffort();
				JointEffort spEffortN = curSMJoint.CreateJointEffort();
				JointEffort spEffortT = curSMJoint.CreateJointEffort();

				spEffortM.Value = dMaxM;
				spEffortN.Value = dMaxN;
				spEffortT.Value = dMaxT;

				spJointEfforts.Add(spEffortM);
				spJointEfforts.Add(spEffortN);
				spJointEfforts.Add(spEffortT);

				spJointLoadingCase.JointEfforts = spJointEfforts;
				spJointLoadingCase.Name = sLoadingCaseName;

				spJointLoadingCases.Add(spJointLoadingCase);

				curSMJoint.SetJointNSAModuleLoadingCases(m_pCurSMRule, spJointLoadingCases);
			}
		}

		private void setGridTorsorsValues(INodeStressAppModule m_pCurSMRule, int nRowIndex, string sLoadingCaseName)
		{
			IStressModuleJoint curSMJoint = m_pCurSMRule.Joint;

			//get now the module loading cases
			IJointLoadingCases spJointLoadingCases = curSMJoint.GetJointNSAModuleLoadingCases(m_pCurSMRule);

			if (null != spJointLoadingCases)
			{
				if ((int)spJointLoadingCases.Count > 0)
				{
					bool bFoundLoadingCase = false;

					for (int i = 0; i < (int)spJointLoadingCases.Count; i++)
					{
						IJointLoadingCase spJointLoadingCase = spJointLoadingCases[i];
						IJointEfforts spJointEfforts = spJointLoadingCase.JointEfforts;

						if ((null != spJointEfforts) && (spJointLoadingCase.Name == sLoadingCaseName))
						{
							if (3 == (int)spJointEfforts.Count)
							{
								bFoundLoadingCase = true;

								m_dMaxM = (double)spJointEfforts[0].Value;
								m_dMaxN = (double)spJointEfforts[1].Value;
								m_dMaxV = (double)spJointEfforts[2].Value;
							}
						}
					}

					if (!bFoundLoadingCase)
					{
						m_dMaxM = 0;
						m_dMaxN = 0;
						m_dMaxV = 0;
					}
				}
				else
				{
					m_dMaxM = 0;
					m_dMaxN = 0;
					m_dMaxV = 0;
				}
			}

			//fill grid with found values
			axSimpleTorsors.set_ItemDoubleValue(nRowIndex, 0, m_dMaxM);
			axSimpleTorsors.set_ItemDoubleValue(nRowIndex, 1, m_dMaxN);
			axSimpleTorsors.set_ItemDoubleValue(nRowIndex, 2, m_dMaxV);

			//set the values in the joint loading cases
			setJointTorsorsValues(m_CreatePlateDesign, m_dMaxM, m_dMaxN, m_dMaxV, sLoadingCaseName);
		}

		private void setJoint(INodeStressAppModule m_pCurSMRule, int nNoRows)
		{
			IStressModuleJoint curSMJoint = m_pCurSMRule.Joint;

			//get current units for Moment and Force
			IUnitsManager units = curSMJoint.Units;
      IUnit momentUnit = (AstSTEELAUTOMATIONLib.IUnit)new DSCROOTSCOMLib.UnitClass();
			IUnit forceUnit = (AstSTEELAUTOMATIONLib.IUnit)new DSCROOTSCOMLib.UnitClass();
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

			//fill torsors grid
			axSimpleTorsors.DeleteAllItems();
			axSimpleTorsors.FixedRowCount = 1;
			axSimpleTorsors.ColumnCount = 3;
			axSimpleTorsors.RowCount = nNoRows + 1;

			axSimpleTorsors.Height = 20 + (20 * nNoRows);
			axSimpleTorsors.ExpandToFit();

			axSimpleTorsors.set_ItemTextValue(0, 0, strCol_M_Name);
			axSimpleTorsors.set_ItemTextValue(0, 1, strCol_N_Name);
			axSimpleTorsors.set_ItemTextValue(0, 2, strCol_V_Name);

      //column "M"
      axSimpleTorsors.set_ColumnType(0, ASTCONTROLSLib.eColumnType.kEditBox); //kEditBox
      axSimpleTorsors.set_ColumnDataType(0, ASTCONTROLSLib.eCellDataType.kUnits); //kUnits
      axSimpleTorsors.set_ColumnUnits(0, ASTCONTROLSLib.eCellUnitType.kUnitMoment); //kUnitMoment

      //column "N"
      axSimpleTorsors.set_ColumnType(1, ASTCONTROLSLib.eColumnType.kEditBox); //kEditBox
      axSimpleTorsors.set_ColumnDataType(1, ASTCONTROLSLib.eCellDataType.kUnits); //kUnits
      axSimpleTorsors.set_ColumnUnits(1, ASTCONTROLSLib.eCellUnitType.kUnitForce); //kUnitForce

      //column "T"
      axSimpleTorsors.set_ColumnType(2, ASTCONTROLSLib.eColumnType.kEditBox); //kEditBox
      axSimpleTorsors.set_ColumnDataType(2, ASTCONTROLSLib.eCellDataType.kUnits); //kUnits
      axSimpleTorsors.set_ColumnUnits(2, ASTCONTROLSLib.eCellUnitType.kUnitForce); //kUnitForce

			//check if the detailed torsors should be used
			if (true == curSMJoint.UseDetailedTorsors)
			{
				for (int j = 1; j <= nNoRows; j++)
				{
					axSimpleTorsors.EnableCell(j, 0, 0);
					axSimpleTorsors.EnableCell(j, 1, 0);
					axSimpleTorsors.EnableCell(j, 2, 0);
				}
			}
			else
			{
				for (int j = 1; j <= nNoRows; j++)
				{
					axSimpleTorsors.EnableCell(j, 0, 1);
					axSimpleTorsors.EnableCell(j, 1, 1);
					axSimpleTorsors.EnableCell(j, 2, 1);
				}
			}

			setGridTorsorsValues(m_pCurSMRule, 1, "SimpleMaxTorsor");
		}

		private void setEnable(int nRow, INodeStressAppModule m_pCurSMRule)
		{
			axSimpleTorsors.EnableCell(nRow, 0, 0);
			axSimpleTorsors.EnableCell(nRow, 1, 0);
			axSimpleTorsors.EnableCell(nRow, 2, 0);

			if (false == m_pCurSMRule.Joint.UseDetailedTorsors)
			{
				axSimpleTorsors.EnableCell(nRow, 0, (short)1);
				axSimpleTorsors.EnableCell(nRow, 1, (short)1);
				axSimpleTorsors.EnableCell(nRow, 2, (short)1);
			}

			axSimpleTorsors.Refresh();
		}

		#endregion
	}
}