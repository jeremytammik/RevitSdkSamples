using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    public partial class ReNameForm : System.Windows.Forms.Form
    {
        public ReNameForm(ISettingNameOperation settingWithNameOperation)
        {
            InitializeComponent();
            m_settingWithNameOperation = settingWithNameOperation;
            previousNameTextBox.Text = 
            newNameTextBox.Text = 
            m_settingWithNameOperation.SettingName;
        }

        ISettingNameOperation m_settingWithNameOperation;

        private void okButton_Click(object sender, EventArgs e)
        {
            m_settingWithNameOperation.Rename(newNameTextBox.Text);
        }
    }
}