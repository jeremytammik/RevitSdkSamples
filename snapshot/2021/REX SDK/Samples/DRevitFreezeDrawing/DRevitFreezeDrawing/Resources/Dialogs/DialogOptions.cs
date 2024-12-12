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
using System.Drawing;
using System.Windows.Forms;
using global::System.IO;

using REX.Common;

namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
   /// <summary>
   /// Option dialog class
   /// </summary>
   partial class DialogOptions : REXExtensionForm
   {
      private Autodesk.Revit.DB.ImportColorMode m_ColMode;
      string m_Directory;
      string m_ProjectName;
      int m_ComboVersionItem;

      string m_SavedDirectory;
      string m_SavedProjectName;
      int m_SavedComboVersionItem;
      bool m_CopyCheck;
      bool m_CopyCheckNow;
      bool m_deleteViews;

      //**********************************************************************
      public Autodesk.Revit.DB.ImportColorMode ColMode
      {
         get
         {
            return m_ColMode;
         }
      }
      //**********************************************************************
      public string Browse
      {
         get
         {
            return m_SavedDirectory;
         }
      }
      //**********************************************************************
      public string BaseName
      {
         get
         {
            return m_SavedProjectName;
         }
      }
      //**********************************************************************
      public bool Copy
      {
         get
         {
            return m_CopyCheck;
         }
      }
      //**********************************************************************
      public bool CopyNow
      {
         get
         {
            return m_CopyCheckNow;
         }
      }
      //**********************************************************************
      public bool DeleteView
      {
         get
         {
            return m_deleteViews;
         }
      }
      //**********************************************************************
      public DialogOptions(REXExtension argExt) : base(argExt)
      {
         InitializeComponent();
         Init();
      }
      //**********************************************************************
      private void Init()
      {
         radio_BandW.Checked = true;
         m_ColMode = Autodesk.Revit.DB.ImportColorMode.BlackAndWhite;

         //folderBrowser
         string path = ((Data)(ThisExtension.GetData())).OptionPath;
         if (!Directory.Exists(path))
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

         folderBrowserDialogDwg.SelectedPath = path;
         m_Directory = folderBrowserDialogDwg.SelectedPath;
         m_SavedDirectory = m_Directory;
         folderBrowserDialogDwg.Description = Resources.Strings.Texts.ChooseFolder;

         //combo version            
         combo_Version.Items.Add("2000");
         combo_Version.Items.Add("2004");
         combo_Version.Items.Add("2007");
         m_ComboVersionItem = 2;
         m_SavedComboVersionItem = 2;

         check_CopyDwg.Checked = false;
         m_CopyCheck = false;

         CopyCheckChange(false);
         m_deleteViews = false;
         check_Delete.Checked = false;

         this.Icon = ThisExtension.GetIcon();
      }

      //**********************************************************************
      public void SetProjectName(string name)
      {
         m_ProjectName = name;
         m_SavedProjectName = name;
      }
      //***********************************************************************
      public Autodesk.Revit.DB.ACADVersion GetVersion()
      {
         //if (m_SavedComboVersionItem == 0)
         //    return Autodesk.Revit.DB.ACADVersion.R2000;
         //else if (m_SavedComboVersionItem == 1)
         //    return Autodesk.Revit.DB.ACADVersion.R2004;
         //else
         return Autodesk.Revit.DB.ACADVersion.R2007;
      }
      //**********************************************************************
      private void Button_OK_Click(object sender, EventArgs e)
      {
         if (radio_BandW.Checked)
            m_ColMode = Autodesk.Revit.DB.ImportColorMode.BlackAndWhite;
         else if (radio_Preserve.Checked)
            m_ColMode = Autodesk.Revit.DB.ImportColorMode.Preserved;
         else
            m_ColMode = Autodesk.Revit.DB.ImportColorMode.Inverted;

         m_CopyCheck = check_CopyDwg.Checked;

         m_SavedComboVersionItem = m_ComboVersionItem;
         m_SavedDirectory = m_Directory;
         m_SavedProjectName = m_ProjectName;
         m_deleteViews = check_Delete.Checked;
         this.Hide();
      }
      //**********************************************************************
      private void DialogOptions_Load(object sender, EventArgs e)
      {
         if (m_ColMode == Autodesk.Revit.DB.ImportColorMode.BlackAndWhite)
            radio_BandW.Checked = true;
         else if (m_ColMode == Autodesk.Revit.DB.ImportColorMode.Preserved)
            radio_Preserve.Checked = true;
         else
            radio_Invert.Checked = true;

         check_CopyDwg.Checked = m_CopyCheck;
         m_ComboVersionItem = m_SavedComboVersionItem;
         m_Directory = m_SavedDirectory;
         m_ProjectName = m_SavedProjectName;
         CopyCheckChange(m_CopyCheck);

         check_Delete.Checked = m_deleteViews;
      }
      //**********************************************************************
      private void Button_Cancel_Click(object sender, EventArgs e)
      {
         this.Hide();
      }
      //**********************************************************************
      private void button_Browse_Click(object sender, EventArgs e)
      {
         folderBrowserDialogDwg.SelectedPath = textBox_Browse.Text;
         folderBrowserDialogDwg.ShowDialog();
         m_Directory = folderBrowserDialogDwg.SelectedPath;
         textBox_Browse.Text = m_Directory;
      }
      //**********************************************************************

      private void CopyCheckChange(bool argCheck)
      {
         if (argCheck)
         {
            label_Browse.Enabled = true;
            label_DwgBaseName.Enabled = true;
            label_Version.Enabled = true;
            button_Browse.Enabled = true;

            textBox_Browse.Enabled = true;
            textBox_Browse.BackColor = SystemColors.Window;
            textBox_Browse.Text = m_Directory;

            text_DwgBaseName.Enabled = true;
            text_DwgBaseName.BackColor = SystemColors.Window;
            text_DwgBaseName.Text = m_ProjectName;

            combo_Version.Enabled = true;
            combo_Version.SelectedIndex = m_ComboVersionItem;
         }
         else
         {
            label_Browse.Enabled = false;
            label_DwgBaseName.Enabled = false;
            label_Version.Enabled = false;
            button_Browse.Enabled = false;

            textBox_Browse.Enabled = false;
            textBox_Browse.BackColor = SystemColors.Control;
            textBox_Browse.Text = "";

            text_DwgBaseName.Enabled = false;
            text_DwgBaseName.BackColor = SystemColors.Control;
            text_DwgBaseName.Text = "";

            combo_Version.Enabled = false;
            combo_Version.SelectedIndex = -1;
         }
      }
      //**********************************************************************
      private void check_CopyDwg_CheckedChanged(object sender, EventArgs e)
      {
         CopyCheckChange(check_CopyDwg.Checked);
         m_CopyCheckNow = check_CopyDwg.Checked;
      }
      //**********************************************************************
      private void text_DwgBaseName_TextChanged(object sender, EventArgs e)
      {
         if (check_CopyDwg.Checked)
            m_ProjectName = text_DwgBaseName.Text;
      }
      //**********************************************************************
      private void textBox_Browse_TextChanged(object sender, EventArgs e)
      {
         if (check_CopyDwg.Checked)
            m_Directory = textBox_Browse.Text;
      }
      //**********************************************************************
      private void combo_Version_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (check_CopyDwg.Checked)
            m_ComboVersionItem = combo_Version.SelectedIndex;
      }
      //**********************************************************************
      private void button_ExportOptions_Click(object sender, EventArgs e)
      {
         ((Extension)ThisExtension).ShowExportOptionsDialog();
      }
      //**********************************************************************
      public Autodesk.Revit.DB.DWGImportOptions GetImportOptions()
      {
         Autodesk.Revit.DB.DWGImportOptions dwgImpOpt = new Autodesk.Revit.DB.DWGImportOptions();
         dwgImpOpt.ColorMode = m_ColMode;
         dwgImpOpt.Placement = Autodesk.Revit.DB.ImportPlacement.Origin;
         dwgImpOpt.OrientToView = true;
         dwgImpOpt.VisibleLayersOnly = false;
         return dwgImpOpt;
      }

      private void Help_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         // help will be launched directly from path related to exe file
         string filePath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
         var path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filePath), @"en-US\Help\contexthelp\DREVITFREEZEDRAWING.htm");
         System.Diagnostics.Process.Start(path);

         //REXCommand Cmd = new REXCommand(REXCommandType.Help);
         //ThisExtension.Command(ref Cmd);
      }
   }
}