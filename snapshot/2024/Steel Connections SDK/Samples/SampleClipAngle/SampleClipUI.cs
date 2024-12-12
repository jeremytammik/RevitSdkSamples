using AstSTEELAUTOMATIONLib;
using Autodesk.AdvanceSteel.DotNetRoots;
using System;
using System.Windows.Forms;

namespace SampleClipAngle
{
   public partial class SampleClipUI : Form
   {
      private const int WS_CHILD = 0x40000000;
      private SampleClipAngle m_pCurRule;
      private bool m_bIsInitialising = true;
      public SampleClipUI(SampleClipAngle clipAngleRule)
      {
         InitializeComponent();

         axAstComboProfile1.Location = AstDpi.Scale(axAstComboProfile1.Location);
         axBoltStandards1.Location = AstDpi.Scale(axBoltStandards1.Location);
         axAstBMP1.Location = AstDpi.Scale(axAstBMP1.Location);

         m_pCurRule = clipAngleRule;

         axAstComboProfile1.CurrentProfileName = "AISC 15.0 Angle equal#@§@#L4X4X3/8";
         axAstComboProfile1.UseFilterClass = true;
         axAstComboProfile1.ShowHideAllSections = true;
         axAstComboProfile1.AppendAcceptedClassGroup("W");
         axAstComboProfile1.CurrentClass = m_pCurRule.m_sProfType;
         axAstComboProfile1.CurrentSection = m_pCurRule.m_sProfSize;

         axBoltStandards1.BoltStandard = m_pCurRule.m_sBoltsStandard;
         axBoltStandards1.BoltMaterial = m_pCurRule.m_sBoltsMaterial;
         axBoltStandards1.BoltSet = m_pCurRule.m_sBoltsSet;
         axBoltStandards1.BoltDiameter = m_pCurRule.m_dBoltsDiameter;

         m_bIsInitialising = false;
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

      private void axAstComboProfile1_ProfileChanged(object sender, EventArgs e)
      {
         if (!m_bIsInitialising && m_pCurRule != null)
         {
            IJoint curJoint = m_pCurRule.Joint;

            if (!axAstComboProfile1.Enabled)
            {
               return;
            }

            if (m_pCurRule.m_sProfType != axAstComboProfile1.CurrentClass ||
                m_pCurRule.m_sProfSize != axAstComboProfile1.CurrentSection)
            {
               m_pCurRule.m_sProfType = axAstComboProfile1.CurrentClass;
               m_pCurRule.m_sProfSize = axAstComboProfile1.CurrentSection;

               curJoint.SaveData(m_pCurRule);
               curJoint.UpdateDrivenConstruction();
            }
         }
      }

      private void axBoltStandards1_BoltChanged(object sender, EventArgs e)
      {
         if (!m_bIsInitialising && m_pCurRule != null)
         {
            IJoint curJoint = m_pCurRule.Joint;

            double tempValueDiameter = axBoltStandards1.BoltDiameter;
            string tempValueGrade = axBoltStandards1.BoltMaterial;
            string tempValueAssembly = axBoltStandards1.BoltSet;
            string tempValueType = axBoltStandards1.BoltStandard;

            string jointGrade = m_pCurRule.m_sBoltsMaterial;
            string jointAssembly = m_pCurRule.m_sBoltsSet;
            string jointType = m_pCurRule.m_sBoltsStandard;

            if ((tempValueDiameter != m_pCurRule.m_dBoltsDiameter) ||
                (tempValueGrade != jointGrade) ||
                (tempValueAssembly != jointAssembly) ||
                (tempValueType != jointType))
            {
               m_pCurRule.m_dBoltsDiameter = tempValueDiameter;
               m_pCurRule.m_sBoltsMaterial = tempValueGrade;
               m_pCurRule.m_sBoltsSet = tempValueAssembly;
               m_pCurRule.m_sBoltsStandard = tempValueType;

               curJoint.SaveData(m_pCurRule);
               curJoint.UpdateDrivenConstruction();
            }
         }
      }
   }
}
