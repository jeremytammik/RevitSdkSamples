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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using REX.Common;
using Autodesk.REX.Framework;

namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
    /// <summary>
    /// Main form
    /// </summary>
    public partial class MainForm : REXExtensionForm
    {
        public MainForm()
        {
        }
        public MainForm(REXExtension Ext)
            : base(Ext)
        {
            InitializeComponent();            
        }

        private void ButtonApply_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;   
            REXCommand Cmd = new REXCommand(REXCommandType.OK);
            ThisExtension.Command(ref Cmd);
        }
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            REXCommand Cmd = new REXCommand(REXCommandType.Close);
            Cmd.CommandObject = false;
            ThisExtension.Command(ref Cmd);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            REXCommand Cmd = new REXCommand(REXCommandType.Close);
            Cmd.CommandObject = true;
            if (((bool)ThisExtension.Command(ref Cmd)) == true)
                e.Cancel = false;
            else
                e.Cancel = true;
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

        private void About_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            REXCommand Cmd = new REXCommand(REXCommandType.About);
            ThisExtension.Command(ref Cmd);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button_Options_Click(object sender, EventArgs e)
        {
            ((Extension)ThisExtension).ShowOptionsDialog();
        }

    }
}
