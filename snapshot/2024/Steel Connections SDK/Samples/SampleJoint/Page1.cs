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

using Autodesk.AdvanceSteel.DotNetRoots;
using System;
using System.Windows.Forms;

using AstSTEELAUTOMATIONLib;

namespace SteelConnectionsJointExample
{
   public partial class Page1 : Form
   {
      private CreatePlate m_CurRule = null;

      private const int WS_CHILD = 0x40000000;

      public Page1()
      {
         InitializeComponent();
         ScaleAxControls();
      }

      public Page1(CreatePlate rule)
      {
         InitializeComponent();
         ScaleAxControls();

         m_CurRule = rule;

         axAstUnitPlateThickness.DoubleValue = m_CurRule.m_dPlateThickness;
         axAstUnitPlateLength.DoubleValue = m_CurRule.m_dPlateLength;
         axAstUnitPlateWidth.DoubleValue = m_CurRule.m_dPlateWidth;
         axAstUnitCutBack.DoubleValue = m_CurRule.m_dCutBack;

         axAnchor.BoltStandard = m_CurRule.m_sAnchorType;
         axAnchor.BoltMaterial = m_CurRule.m_sAnchorMaterial;
         axAnchor.BoltSet = m_CurRule.m_sAnchorAssembly;
         axAnchor.BoltDiameter = m_CurRule.m_dAnchorDiameter;
         axAnchor.AnchorLength = m_CurRule.m_dAnchorLength;
      }

      protected override CreateParams CreateParams
      {
         get
         {
            CreateParams cp = base.CreateParams;
            cp.Style = cp.Style | WS_CHILD;
            return cp;
         }
      }

      private void axAstUnitPlateThickness_ValueChanged(object sender, EventArgs e)
      {
         // TODO: Add your message handler code here
         if (m_CurRule != null)
         {
            IJoint curJoint = m_CurRule.Joint;
            if (axAstUnitPlateThickness.DoubleValue != m_CurRule.m_dPlateThickness)
            {
               m_CurRule.m_dPlateThickness = axAstUnitPlateThickness.DoubleValue;
               curJoint.SaveData(m_CurRule);
               curJoint.UpdateDrivenConstruction();
            }
         }
      }

      private void axAstUnitPlateLength_ValueChanged(object sender, EventArgs e)
      {
         // TODO: Add your message handler code here
         if (m_CurRule != null)
         {
            IJoint curJoint = m_CurRule.Joint;
            if (axAstUnitPlateLength.DoubleValue != m_CurRule.m_dPlateLength)
            {
               m_CurRule.m_dPlateLength = axAstUnitPlateLength.DoubleValue;
               curJoint.SaveData(m_CurRule);
               curJoint.UpdateDrivenConstruction();
            }
         }
      }

      private void axAstUnitPlateWidth_ValueChanged(object sender, EventArgs e)
      {
         // TODO: Add your message handler code here
         if (m_CurRule != null)
         {
            IJoint curJoint = m_CurRule.Joint;
            if (axAstUnitPlateWidth.DoubleValue != m_CurRule.m_dPlateWidth)
            {
               m_CurRule.m_dPlateWidth = axAstUnitPlateWidth.DoubleValue;
               curJoint.SaveData(m_CurRule);
               curJoint.UpdateDrivenConstruction();
            }
         }
      }

      private void axAstUnitCutBack_ValueChanged(object sender, EventArgs e)
      {
         // TODO: Add your message handler code here
         if (m_CurRule != null)
         {
            IJoint curJoint = m_CurRule.Joint;
            if (axAstUnitCutBack.DoubleValue != m_CurRule.m_dCutBack)
            {
               m_CurRule.m_dCutBack = axAstUnitCutBack.DoubleValue;
               curJoint.SaveData(m_CurRule); //Save data to joint
               curJoint.UpdateDrivenConstruction(); //Update the joint
            }
         }
      }

      private void axAnchor_BoltChanged(object sender, EventArgs e)
      {
         // TODO: Add your message handler code here
         if (m_CurRule != null)
         {
            IJoint curJoint = m_CurRule.Joint;

            if (axAnchor.AnchorLength != m_CurRule.m_dAnchorLength ||
               axAnchor.BoltDiameter != m_CurRule.m_dAnchorDiameter ||
               axAnchor.BoltMaterial != m_CurRule.m_sAnchorMaterial ||
               axAnchor.BoltStandard != m_CurRule.m_sAnchorType ||
               axAnchor.BoltSet != m_CurRule.m_sAnchorAssembly)
            {
               m_CurRule.m_sAnchorType = axAnchor.BoltStandard;
               m_CurRule.m_sAnchorMaterial = axAnchor.BoltMaterial;
               m_CurRule.m_sAnchorAssembly = axAnchor.BoltSet;
               m_CurRule.m_dAnchorDiameter = axAnchor.BoltDiameter;
               m_CurRule.m_dAnchorLength = axAnchor.AnchorLength;

               curJoint.SaveData(m_CurRule); //Save data to joint
               curJoint.UpdateDrivenConstruction(); //Update the joint
            }
         }
      }

      private void ScaleAxControls()
      {
         axAstUnitPlateThickness.Location = AstDpi.Scale(axAstUnitPlateThickness.Location);
         axAstUnitPlateLength.Location = AstDpi.Scale(axAstUnitPlateLength.Location);
         axAstUnitPlateWidth.Location = AstDpi.Scale(axAstUnitPlateWidth.Location);
         axAstUnitCutBack.Location = AstDpi.Scale(axAstUnitCutBack.Location);
         axAnchor.Location = AstDpi.Scale(axAnchor.Location);
         axAstControls1.Location = AstDpi.Scale(axAstControls1.Location);
      }
   }
}
