using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using REX.Common;
using Autodesk.REX.Framework;

using REX.Unit;

namespace REX.Unit.Resources.Dialogs
{
    public partial class MainControl : REXExtensionMainControl
    {
        public MainControl(REXExtension Ext)
            : base(Ext)
        {
            InitializeComponent();
        }

        public override IContainer Components
        {
            get
            {
                return components;
            }
        }

        private void Toolbox_SelectItem_1(object sender, REX.Controls.Forms.CREXToolbox.ItemSelectionEventArgs e)
        {
            base.Toolbox_SelectItem(sender, e);
        }

        private void VSplit_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ThisExtension.Layout.Update();
        }

        private void ToolBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            RunCommand(e.ClickedItem.Name);
        }
        private void ToolMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            RunCommand(e.ClickedItem.Name);
        }

        private void RunCommand(string Name)
        {
            REXCommand Cmd = null;
            switch (Name)
            {
                case "MenuItemOpen":
                case "ToolStripButtonOpen":
                case "ToolStripMenuItemOpen":
                    ThisExtension.System.LoadFromFile(true);
                    return;
                case "MenuItemSave":
                case "ToolStripButtonSave":
                case "ToolStripMenuItemSave":
                    ThisExtension.System.SaveToFile(false, true);
                    return;
                case "MenuItemSaveAs":
                case "ToolStripMenuItemSaveAs":
                    ThisExtension.System.SaveToFile(true);
                    return;
                case "MenuItemCalc":
                case "ToolStripButtonCalc":
                case "ToolStripMenuItemCalc":
                    Cmd = new REXCommand(REXCommandType.Run);
                    break;
                case "MenuItemHelpIndex":
                case "ToolStripButtonHelp":
                case "ToolStripMenuItemHelp":
                    Cmd = new REXCommand(REXCommandType.Help);
                    break;
                case "MenuItemClose":
                case "ToolStripMenuItemClose":
                    Cmd = new REXCommand(REXCommandType.Close);
                    break;
                case "MenuItemAbout":
                case "ToolStripMenuItemAbout":
                    Cmd = new REXCommand(REXCommandType.About);
                    break;
            }
            if (Cmd != null)
                ThisExtension.Command(ref Cmd);
        }

        private void MenuItemSave_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void MenuItemSaveAs_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void MenuItemPrint_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void MenuItemClose_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void MenuItemCalc_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void MenuItemHelpIndex_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void MenuItemAbout_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }
        private void MenuItemOpen_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void ToolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void ToolStripMenuItemPrint_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void ToolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void ToolStripMenuItemCalc_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void ToolStripMenuItemHelp_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void ToolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripMenuItem)sender).Name);
        }

        private void ToolStripButtonOpen_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripButton)sender).Name);
        }

        private void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripButton)sender).Name);
        }

        private void ToolStripButtonCalc_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripButton)sender).Name);
        }

        private void ToolStripButtonHelp_Click(object sender, EventArgs e)
        {
            RunCommand(((ToolStripButton)sender).Name);
        }

        private void ToolStripContainer_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }
    }
}
