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
using System.Windows.Forms;
using REX.Common;

namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
   /// <summary>
   /// Overwrite dialog for existing file
   /// </summary>
   partial class DialogMessageExists : REXExtensionForm
   {
      public REX.DRevitFreezeDrawing.Main.ReplaceContext Context;

      public DialogMessageExists(REXExtension argExt) : base(argExt)
      {
         InitializeComponent();
         Context = REX.DRevitFreezeDrawing.Main.ReplaceContext.Replace;
         this.Icon = ThisExtension.GetIcon();
      }

      private void button_Replace_Click(object sender, EventArgs e)
      {
         Context = REX.DRevitFreezeDrawing.Main.ReplaceContext.Replace;
         this.Hide();
      }

      private void button_Leave_Click(object sender, EventArgs e)
      {
         Context = REX.DRevitFreezeDrawing.Main.ReplaceContext.Leave;
         this.Hide();
      }

      private void button_ReplaceAll_Click(object sender, EventArgs e)
      {
         Context = REX.DRevitFreezeDrawing.Main.ReplaceContext.ReplaceAll;
         this.Hide();
      }

      private void button_LeaveAll_Click(object sender, EventArgs e)
      {
         Context = REX.DRevitFreezeDrawing.Main.ReplaceContext.LeaveAll;
         this.Hide();
      }

      public void ShowMessageDialog(string file, IWin32Window parent)
      {
         label_File.Text = file + "?";
         if (label_File.Width > this.Width - 20)
            label_File.Text = GetFileDir(file) + "?";

         this.ShowDialog(parent);
      }

      public string GetFileDir(string fileName)
      {
         int indeks1 = fileName.IndexOf("\\");
         string directory = fileName.Substring(0, indeks1 + 1);

         int indeks2 = fileName.LastIndexOf("\\");
         string file = fileName.Substring(indeks2);

         string name = directory + "..." + file;
         return name;
      }
   }
}