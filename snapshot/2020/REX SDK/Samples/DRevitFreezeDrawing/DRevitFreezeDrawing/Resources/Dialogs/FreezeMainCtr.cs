//
// (C) Copyright 2016 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System;
using REX.Common;

namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
   /// <summary>
   /// Dialog control for freeze view choosing
   /// </summary>
   public partial class FreezeMainCtr : REXExtensionControl
   {
      #region<members>

      private bool m_CurrentView;
      #endregion

      #region<properties>
      public bool CurrentView
      {
         get
         {
            return m_CurrentView;
         }
      }

      #endregion
      public FreezeMainCtr(REXExtension argExt)
          : base(argExt)
      {
         InitializeComponent();
      }

      private void FreezeMainCtr_Load(object sender, EventArgs e)
      {
         radio_CurrentV.Checked = true;
      }

      private void radio_CurrentV_CheckedChanged(object sender, EventArgs e)
      {
         if (radio_CurrentV.Checked)
         {
            m_CurrentView = true;
            button_SelectV.Enabled = false;
         }
         else
         {
            m_CurrentView = false;
            button_SelectV.Enabled = true;
         }
      }

      private void radio_SelectedV_CheckedChanged(object sender, EventArgs e)
      {
         if (radio_CurrentV.Checked)
         {
            m_CurrentView = true;
            button_SelectV.Enabled = false;
         }
         else
         {
            m_CurrentView = false;
            button_SelectV.Enabled = true;
         }
      }

      private void button_SelectV_Click(object sender, EventArgs e)
      {
         ((Extension)ThisExtension).ShowSelectViews();
      }

      private void group_Range_Enter(object sender, EventArgs e)
      {
      }
   }
}
