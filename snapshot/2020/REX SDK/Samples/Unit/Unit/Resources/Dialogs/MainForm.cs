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

namespace REX.Unit.Resources.Dialogs
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
            REXCommand Cmd = new REXCommand(REXCommandType.Help);
            ThisExtension.Command(ref Cmd);
        }

        private void About_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            REXCommand Cmd = new REXCommand(REXCommandType.About);
            ThisExtension.Command(ref Cmd);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

    }
}