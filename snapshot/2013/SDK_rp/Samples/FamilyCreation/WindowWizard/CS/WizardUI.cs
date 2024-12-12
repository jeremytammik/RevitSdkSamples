//
// (C) Copyright 2003-2012 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace Revit.SDK.Samples.WindowWizard.CS
{
    /// <summary>
    /// The wizard form
    /// </summary>
    public partial class WizardForm : System.Windows.Forms.Form
    {
        #region Class Memeber Variables
        /// <summary>
        /// store the wizard parameter
        /// </summary>
        WizardParameter m_para;

        /// <summary>
        /// store the family types list
        /// </summary>
        List<String> m_types = new List<string>();

        /// <summary>
        /// store the bindSource
        /// </summary>
        BindingSource bindSource = new BindingSource();
        
        /// <summary>
        /// store the new type button tooltip
        /// </summary>
        ToolTip m_newTip = new ToolTip();
        
        /// <summary>
        /// store the copy type button tooltip
        /// </summary>
        ToolTip m_copyTip = new ToolTip();
        
        /// <summary>
        /// store the error tooltip
        /// </summary>
        ToolTip m_errorTip = new ToolTip();
        
        /// <summary>
        /// store the font
        /// </summary>
        Font m_highFont, m_commonFont;
        
        /// <summary>
        /// store DoubleHungWinPara list
        /// </summary>
        BindingList<DoubleHungWinPara> paraList = new BindingList<DoubleHungWinPara>();
        #endregion
        
        /// <summary>
        /// constructor of WizardForm
        /// </summary>
        /// <param name="para">the WizardParameter</param>
        public WizardForm(WizardParameter para)
        {
            m_para = para;
            InitializeComponent();           
            InitializePara();
            m_newTip.SetToolTip(button_newType, "Add new type");
            m_copyTip.SetToolTip(button_duplicateType, "Duplicate the type");
            m_errorTip.ShowAlways = false;
            m_highFont = InputDimensionLabel.Font;
            m_commonFont = WindowPropertyLabel.Font;
            SetPanelVisibility(2);
        }

        /// <summary>
        /// The nextbutton click function
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void Step1_NextButton_Click(object sender, EventArgs e)
        {
            if (this.panel1.Visible)
            {
                InitializePara();
                SetPanelVisibility(2);
            }
            else if (this.panel2.Visible)
            {
                transforData();
                SetPanelVisibility(3);
            }
            else if (this.panel3.Visible)
            {
                SetPanelVisibility(4);
                SetGridData();
            }
            else if (this.panel4.Visible)
            {
                m_para.PathName = this.m_pathName.Text;
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// set panel visibility
        /// </summary>
        /// <param name="panelNum">panel number</param>
        private void SetPanelVisibility(int panelNum)
        {
            switch (panelNum)
            {
                case 1:
                    this.panel1.Visible = true;
                    this.panel2.Visible = false;
                    this.panel3.Visible = false;
                    this.panel4.Visible = false;
                    this.Step1_BackButton.Enabled = false;
                    break;
                case 2:
                    this.panel1.Visible = false;
                    this.panel2.Visible = true;
                    this.panel3.Visible = false;
                    this.panel4.Visible = false;                    
                    this.Step1_BackButton.Enabled = false;
                    this.InputDimensionLabel.ForeColor = System.Drawing.Color.Black;
                    this.InputDimensionLabel.Font = m_highFont;
                    this.WindowPropertyLabel.ForeColor = System.Drawing.Color.Gray;
                    this.WindowPropertyLabel.Font = m_commonFont;
                    this.InputPathLabel.ForeColor = System.Drawing.Color.Gray;
                    this.InputPathLabel.Font = m_commonFont;
                    break;
                case 3:
                    this.panel3.Visible = true;
                    this.panel1.Visible = false;
                    this.panel2.Visible = false;                    
                    this.panel4.Visible = false;
                    this.Step1_BackButton.Enabled = true;
                    this.Step1_NextButton.Text = "Next >";                    
                    this.InputPathLabel.ForeColor = System.Drawing.Color.Gray;
                    this.InputDimensionLabel.Font = m_commonFont;
                    this.WindowPropertyLabel.ForeColor = System.Drawing.Color.Black;
                    this.WindowPropertyLabel.Font = m_highFont;
                    this.InputPathLabel.ForeColor = System.Drawing.Color.Gray;
                    this.InputPathLabel.Font = m_commonFont;
                    break;
                case 4:
                    this.panel1.Visible = false;
                    this.panel2.Visible = false;
                    this.panel3.Visible = false;
                    this.panel4.Visible = true;
                    this.Step1_BackButton.Enabled = true;
                    this.Step1_NextButton.Text = "Finish";
                    this.InputPathLabel.ForeColor = System.Drawing.Color.Gray;
                    this.InputDimensionLabel.Font = m_commonFont;
                    this.WindowPropertyLabel.ForeColor = System.Drawing.Color.Gray;
                    this.WindowPropertyLabel.Font = m_commonFont;
                    this.InputPathLabel.ForeColor = System.Drawing.Color.Black;
                    this.InputPathLabel.Font = m_highFont;
                    break;
            }
        }

        /// <summary>
        /// the backbutton click function
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void Step1_BackButton_Click(object sender, EventArgs e)
        {
            if (this.panel2.Visible)
            {
                SetPanelVisibility(1);
            }
            else if (this.panel3.Visible)
            {
                SetPanelVisibility(2);
            }
            else if (this.panel4.Visible)
            {
                SetPanelVisibility(3);
            }
        }

        /// <summary>
        /// transfer data
        /// </summary>
        private void transforData()
        {
            if (m_para.m_template == "DoubleHung")
            {
                DoubleHungWinPara dbhungPara = new DoubleHungWinPara(m_para.Validator.IsMetric);
                dbhungPara.Height = Convert.ToDouble(m_height.Text);
                dbhungPara.Width = Convert.ToDouble(m_width.Text);
                dbhungPara.Inset = Convert.ToDouble(m_inset.Text);
                dbhungPara.SillHeight = Convert.ToDouble(m_sillHeight.Text);
                dbhungPara.Type = m_comboType.Text;
                m_para.CurrentPara = dbhungPara;
                if (!m_para.WinParaTab.Contains(dbhungPara.Type))
                {
                    m_para.WinParaTab.Add(dbhungPara.Type, dbhungPara);                       
                    m_comboType.Items.Add(dbhungPara.Type);
                }
                else
                {
                    m_para.WinParaTab[dbhungPara.Type] = dbhungPara;
                }
            }
            Update();
        }

        /// <summary>
        /// Initialize data
        /// </summary>
        private void InitializePara()
        {
            DoubleHungWinPara dbhungPara = new DoubleHungWinPara(m_para.Validator.IsMetric);            
            if (!m_para.WinParaTab.Contains(dbhungPara.Type))
            {
                m_para.WinParaTab.Add(dbhungPara.Type, dbhungPara);
                m_types.Add(dbhungPara.Type);
            }
            else
            {
                m_para.WinParaTab[dbhungPara.Type] = dbhungPara;
            }           
            bindSource.DataSource = m_types;            
            this.m_comboType.Items.Add(m_para.CurrentPara.Type);
            this.m_comboType.SelectedIndex = 0;
            this.m_glassMat.DataSource = m_para.GlassMaterials;
            this.m_sashMat.DataSource = m_para.FrameMaterials;
            this.m_pathName.Text = m_para.PathName;
            SetParaText(dbhungPara);
        }

        /// <summary>
        /// The newtype button click function
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void button_newType_Click(object sender, EventArgs e)
        {
            transforData();
            DoubleHungWinPara newPara = new DoubleHungWinPara(m_para.Validator.IsMetric);
            SetParaText(newPara);
            this.m_comboType.Focus();
        }

        /// <summary>
        /// The duplicatebutton click function
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void button_duplicateType_Click(object sender, EventArgs e)
        {
            transforData();
            DoubleHungWinPara copyPara = new DoubleHungWinPara((DoubleHungWinPara)m_para.CurrentPara);
            SetParaText(copyPara);
            this.m_comboType.Focus();
        }

        /// <summary>
        /// set WindowParameter text
        /// </summary>
        /// <param name="para">the WindowParameter</param>
        private void SetParaText(WindowParameter para)
        { 
            DoubleHungWinPara dbhungPara = para as DoubleHungWinPara;
            m_sillHeight.Text = dbhungPara.SillHeight.ToString();
            m_width.Text = dbhungPara.Width.ToString();
            m_height.Text = dbhungPara.Height.ToString();
            m_inset.Text = dbhungPara.Inset.ToString();
            m_comboType.Text = dbhungPara.Type;
        }

        /// <summary>
        /// set grid data
        /// </summary>
        private void SetGridData()
        {
            paraList.Clear();
            foreach (String key in m_para.WinParaTab.Keys)
            { 
                DoubleHungWinPara para = m_para.WinParaTab[key] as DoubleHungWinPara;
                if (null == para)
                {
                    continue;
                }
                paraList.Add(para);
            }
            this.dataGridView1.DataSource = paraList;
        }

        /// <summary>
        /// m_height textbox's text changed event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_height_TextChanged(object sender, EventArgs e)
        {
            ValidateInput(m_height);
        }

        /// <summary>
        /// m_width textbox's text changed event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_width_TextChanged(object sender, EventArgs e)
        {
            ValidateInput(m_width);
        }

        /// <summary>
        /// m_inset textbox's text changed event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_inset_TextChanged(object sender, EventArgs e)
        {
            ValidateInput(m_inset);
        }

        /// <summary>
        /// m_sillHeight textbox's text changed event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_sillHeight_TextChanged(object sender, EventArgs e)
        {
            ValidateInput(m_sillHeight);
        }

        /// <summary>
        /// m_comboType SelectedIndexChanged event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_para.CurrentPara = m_para.WinParaTab[m_comboType.SelectedItem.ToString()] as WindowParameter;
            SetParaText(m_para.CurrentPara);
        }

        /// <summary>
        /// m_glassMat SelectedIndexChanged event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_glassMat_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_para.GlassMat = m_glassMat.SelectedItem.ToString();
        }

        /// <summary>
        /// m_sashMat SelectedIndexChanged event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_sashMat_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_para.SashMat = m_sashMat.SelectedItem.ToString();
        }

        /// <summary>
        /// validate control input value
        /// </summary>
        /// <param name="control">the control</param>
        private bool ValidateInput(Control control)
        {
            if (null == control)
            {
                return true;
            }
            if (String.IsNullOrEmpty(control.Text))
            {
                this.Step1_NextButton.Enabled = false;
                return false;
            }
            TextBox textbox = control as TextBox;
            if (null == textbox)
            {
                return true;
            }
            double value = 0.0;
            String result = m_para.Validator.IsDouble(textbox.Text, ref value);
            if (!String.IsNullOrEmpty(result))
            {
                m_errorTip.SetToolTip(textbox, result);
                textbox.Text = String.Empty;
                this.Step1_NextButton.Enabled = false;
                return false;
            }
            m_errorTip.RemoveAll();
            switch (textbox.Name)
            {
                case "m_height":
                    result = m_para.Validator.IsHeightInRange(value);
                    break;
                case "m_width":
                    result = m_para.Validator.IsWidthInRange(value);
                    break;
                case "m_inset":
                    result = m_para.Validator.IsInsetInRange(value);
                    break;
                case "m_sillHeight":
                    result = m_para.Validator.IsSillHeightInRange(value);
                    break;
                default:
                    break;
            }
            if (!String.IsNullOrEmpty(result))
            {
                m_errorTip.SetToolTip(textbox, result);
                this.Step1_NextButton.Enabled = false;
                return false;
            }
            m_errorTip.RemoveAll();
            this.Step1_NextButton.Enabled = true;
            return true;
        }

        /// <summary>
        /// m_buttonBrowser click event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_buttonBrowser_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.CheckPathExists = true;
            saveDialog.SupportMultiDottedExtensions = true; 
            saveDialog.OverwritePrompt = true;
            saveDialog.ValidateNames = true;
            saveDialog.Filter = "Family file(*.rfa)|*.rfa|All files(*.*)|*.*";
            saveDialog.FilterIndex = 2;
            if (DialogResult.OK == saveDialog.ShowDialog())
            {
                if (!String.IsNullOrEmpty(saveDialog.FileName))
                {
                    m_pathName.Text = saveDialog.FileName;
                }
            }
        }

        /// <summary>
        /// m_height leave event 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_height_Leave(object sender, EventArgs e)
        {
            CheckValue(m_height);
        }

        /// <summary>
        /// check input 
        /// </summary>
        /// <param name="control">the host control</param>
        private void CheckValue(Control control)
        {
            if (String.IsNullOrEmpty(control.Text))
            {
                control.Focus();
                m_errorTip.SetToolTip(control, "Please input a valid value");
            }
            if (!ValidateInput(control))
            {
                control.Focus();
                control.Text = String.Empty;
            }
        }

        /// <summary>
        /// m_width leave event 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_width_Leave(object sender, EventArgs e)
        {
            CheckValue(m_width);
        }

        /// <summary>
        /// m_inset leave event 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_inset_Leave(object sender, EventArgs e)
        {
            CheckValue(m_inset);
        }

        /// <summary>
        /// m_sillHeight leave event 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_sillHeight_Leave(object sender, EventArgs e)
        {
            CheckValue(m_sillHeight);
        }

        /// <summary>
        /// m_comboType leave event 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void m_comboType_Leave(object sender, EventArgs e)
        {
            CheckValue(m_comboType);
        }

        /// <summary>
        /// Step1_HelpButton click event to open the help document
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        private void Step1_HelpButton_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            char sp = System.IO.Path.DirectorySeparatorChar;//{'\\'};
            path = path.Substring(0, path.LastIndexOf(sp));
            path = path.Substring(0, path.LastIndexOf(sp)) + sp + "ReadMe_WindowWizard.rtf";
            System.Diagnostics.Process.Start(path);            
        }
    }
}
