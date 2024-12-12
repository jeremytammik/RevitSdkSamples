using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    public partial class SaveAsForm : System.Windows.Forms.Form
    {
        public SaveAsForm(ISettingNameOperation settingNameOperation)
        {
            InitializeComponent();
            m_settingNameOperation = settingNameOperation;
            newNameTextBox.Text = m_settingNameOperation.Prefix
                + m_settingNameOperation.SettingCount.ToString();
        }
        ISettingNameOperation m_settingNameOperation;

        private void okButton_Click(object sender, EventArgs e)
        {
            m_settingNameOperation.SaveAs(newNameTextBox.Text);
        }
    }
}