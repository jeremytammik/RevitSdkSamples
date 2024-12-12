//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    public partial class PrintSetupForm : System.Windows.Forms.Form
    {
        private PrintSTP m_printSetup;
        private bool m_stopUpdateFlag;

        public PrintSetupForm(PrintSTP printSetup)
        {
            m_printSetup = printSetup;
            InitializeComponent();
        }

        private void PrintSetupForm_Load(object sender, EventArgs e)
        {
            printerNameLabel.Text = m_printSetup.PrinterName;
            printSetupsComboBox.DataSource = m_printSetup.PrintSettingNames;
            printSetupsComboBox.SelectedItem = m_printSetup.SettingName;
            this.printSetupsComboBox.SelectedValueChanged += new System.EventHandler(this.printSetupsComboBox_SelectedValueChanged);
            renameButton.Enabled = deleteButton.Enabled =
                m_printSetup.SettingName.Equals("<In-Session>") ? false : true;

            paperSizeComboBox.DataSource = m_printSetup.PaperSizes;
            paperSizeComboBox.SelectedItem = m_printSetup.PaperSize;
            this.paperSizeComboBox.SelectedValueChanged += new System.EventHandler(this.sizeComboBox_SelectedValueChanged);
            
            paperSourceComboBox.DataSource = m_printSetup.PaperSources;
            paperSourceComboBox.SelectedItem = m_printSetup.PaperSource;
            this.paperSourceComboBox.SelectedValueChanged += new System.EventHandler(this.sourceComboBox_SelectedValueChanged);
            
            if (m_printSetup.PageOrientation == PageOrientationType.Landscape)
            {
                landscapeRadioButton.Checked = true;
            }
            else
            {
                portraitRadioButton.Checked = true;
            }
            this.landscapeRadioButton.CheckedChanged += new System.EventHandler(this.landscapeRadioButton_CheckedChanged);
            this.portraitRadioButton.CheckedChanged += new System.EventHandler(this.portraitRadioButton_CheckedChanged);
            
            marginTypeComboBox.DataSource = m_printSetup.MarginTypes;
            this.offsetRadioButton.CheckedChanged += new System.EventHandler(this.offsetRadioButton_CheckedChanged);
            this.centerRadioButton.CheckedChanged += new System.EventHandler(this.centerRadioButton_CheckedChanged);
            this.userDefinedMarginYTextBox.TextChanged += new System.EventHandler(this.userDefinedMarginYTextBox_TextChanged);
            this.userDefinedMarginXTextBox.TextChanged += new System.EventHandler(this.userDefinedMarginXTextBox_TextChanged);

            marginTypeComboBox.SelectedItem = m_printSetup.SelectedMarginType;
            this.marginTypeComboBox.SelectedValueChanged += new System.EventHandler(this.marginTypeComboBox_SelectedValueChanged);
            
            if (m_printSetup.PaperPlacement == PaperPlacementType.Center)
            {
                centerRadioButton.Checked = true;
                offsetRadioButton.Checked = false;
            }
            else
            {
                offsetRadioButton.Checked = true;
                centerRadioButton.Checked = false;
            }

            if (m_printSetup.HiddenLineViews == HiddenLineViewsType.RasterProcessing)
            {
                rasterRadioButton.Checked = true;
            }
            else
            {
                vectorRadioButton.Checked = true;
            }
            this.rasterRadioButton.CheckedChanged += new System.EventHandler(this.rasterRadioButton_CheckedChanged);
            this.vectorRadioButton.CheckedChanged += new System.EventHandler(this.vectorRadioButton_CheckedChanged);

            if (m_printSetup.ZoomType == ZoomType.Zoom)
            {
                zoomRadioButton.Checked = true;
                zoomPercentNumericUpDown.Value = m_printSetup.Zoom;
            }
            else
            {
                fitToPageRadioButton.Checked = true;
            }
            this.zoomRadioButton.CheckedChanged += new System.EventHandler(this.zoomRadioButton_CheckedChanged);
            this.fitToPageRadioButton.CheckedChanged += new System.EventHandler(this.fitToPageRadioButton_CheckedChanged);
            

            rasterQualityComboBox.DataSource = m_printSetup.RasterQualities;
            rasterQualityComboBox.SelectedItem = m_printSetup.RasterQuality;
            this.rasterQualityComboBox.SelectedValueChanged += new System.EventHandler(this.rasterQualityComboBox_SelectedValueChanged);
            
            colorsComboBox.DataSource = m_printSetup.Colors;
            colorsComboBox.SelectedItem = m_printSetup.Color;
            this.colorsComboBox.SelectedValueChanged += new System.EventHandler(this.colorsComboBox_SelectedValueChanged);

            ViewLinksInBlueCheckBox.Checked = m_printSetup.ViewLinksinBlue;
            this.ViewLinksInBlueCheckBox.CheckedChanged += new System.EventHandler(this.ViewLinksInBlueCheckBox_CheckedChanged);

            hideScopeBoxedCheckBox.Checked = m_printSetup.HideScopeBoxes;
            this.hideScopeBoxedCheckBox.CheckedChanged += new System.EventHandler(this.hideScopeBoxedCheckBox_CheckedChanged);

            hideRefWorkPlanesCheckBox.Checked = m_printSetup.HideReforWorkPlanes;
            this.hideRefWorkPlanesCheckBox.CheckedChanged += new System.EventHandler(this.hideRefWorkPlanesCheckBox_CheckedChanged);

            hideCropBoundariesCheckBox.Checked = m_printSetup.HideCropBoundaries;
            this.hideCropBoundariesCheckBox.CheckedChanged += new System.EventHandler(this.hideCropBoundariesCheckBox_CheckedChanged);

            hideUnreferencedViewTagsCheckBox.Checked = m_printSetup.HideUnreferencedViewTages;
            this.hideUnreferencedViewTagsCheckBox.CheckedChanged += new System.EventHandler(this.hideUnreferencedViewTagsCheckBox_CheckedChanged);
            
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            m_printSetup.Save();
        }

        private void printSetupsComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (m_stopUpdateFlag)
                return;

            m_printSetup.SettingName = printSetupsComboBox.SelectedItem as string;

            paperSizeComboBox.SelectedItem = m_printSetup.PaperSize;
            paperSourceComboBox.SelectedItem = m_printSetup.PaperSource;
            if (m_printSetup.PageOrientation == PageOrientationType.Landscape)
            {
                landscapeRadioButton.Checked = true;
            }
            else
            {
                portraitRadioButton.Checked = true;
            }
            if (m_printSetup.PaperPlacement == PaperPlacementType.Center)
            {
                centerRadioButton.Checked = true;
            }
            else
            {
                offsetRadioButton.Checked = true;
            }
            if (m_printSetup.VerifyMarginType(marginTypeComboBox))
            {
                marginTypeComboBox.SelectedItem = m_printSetup.SelectedMarginType;
            }

            if (m_printSetup.HiddenLineViews == HiddenLineViewsType.RasterProcessing)
            {
                rasterRadioButton.Checked = true;
            }
            else
            {
                vectorRadioButton.Checked = true;
            }
            if (m_printSetup.ZoomType == ZoomType.Zoom)
            {
                zoomRadioButton.Checked = true;
                zoomPercentNumericUpDown.Value = m_printSetup.Zoom;
            }
            else
            {
                fitToPageRadioButton.Checked = true;
                m_printSetup.ZoomType = ZoomType.Zoom;
                zoomPercentNumericUpDown.Value = m_printSetup.Zoom;
                m_printSetup.ZoomType = ZoomType.FitToPage;
            }

            rasterQualityComboBox.SelectedItem = m_printSetup.RasterQuality;
            colorsComboBox.SelectedItem = m_printSetup.Color;
            ViewLinksInBlueCheckBox.Checked = m_printSetup.ViewLinksinBlue;
            hideScopeBoxedCheckBox.Checked = m_printSetup.HideScopeBoxes;
            hideRefWorkPlanesCheckBox.Checked = m_printSetup.HideReforWorkPlanes;
            hideCropBoundariesCheckBox.Checked = m_printSetup.HideCropBoundaries;
            hideUnreferencedViewTagsCheckBox.Checked = m_printSetup.HideUnreferencedViewTages;

            renameButton.Enabled = deleteButton.Enabled =
                m_printSetup.SettingName.Equals("<In-Session>") ? false : true;

            revertButton.Enabled = false;
        }

        private void sizeComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_printSetup.PaperSize = paperSizeComboBox.SelectedItem as string;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void sourceComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_printSetup.PaperSource = paperSourceComboBox.SelectedItem as string;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void portraitRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (portraitRadioButton.Checked)
            {
                m_printSetup.PageOrientation = PageOrientationType.Portrait;
            }

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void landscapeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (landscapeRadioButton.Checked)
            {
                m_printSetup.PageOrientation = PageOrientationType.Landscape;
            }

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void centerRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!centerRadioButton.Checked)
                return;

            m_printSetup.PaperPlacement = PaperPlacementType.Center;

            m_printSetup.VerifyMarginType(marginTypeComboBox);

            System.Collections.ObjectModel.Collection<System.Windows.Forms.Control> controlsToEnableOrNot =
                new System.Collections.ObjectModel.Collection<System.Windows.Forms.Control>();
            controlsToEnableOrNot.Add(userDefinedMarginXTextBox);
            controlsToEnableOrNot.Add(userDefinedMarginYTextBox);
            if (m_printSetup.VerifyUserDefinedMargin(controlsToEnableOrNot))
            {
                userDefinedMarginXTextBox.Text = m_printSetup.UserDefinedMarginX.ToString();
                userDefinedMarginYTextBox.Text = m_printSetup.UserDefinedMarginY.ToString();

                if (!revertButton.Enabled)
                {
                    revertButton.Enabled = true;
                }
            }
            
        }

        private void offsetRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!offsetRadioButton.Checked)
                return;

            m_printSetup.PaperPlacement = PaperPlacementType.Margins;

            m_printSetup.VerifyMarginType(marginTypeComboBox);

            System.Collections.ObjectModel.Collection<System.Windows.Forms.Control> controlsToEnableOrNot =
                new System.Collections.ObjectModel.Collection<System.Windows.Forms.Control>();
            controlsToEnableOrNot.Add(userDefinedMarginXTextBox);
            controlsToEnableOrNot.Add(userDefinedMarginYTextBox);
            if (m_printSetup.VerifyUserDefinedMargin(controlsToEnableOrNot))
            {
                userDefinedMarginXTextBox.Text = m_printSetup.UserDefinedMarginX.ToString();
                userDefinedMarginYTextBox.Text = m_printSetup.UserDefinedMarginY.ToString();

                if (!revertButton.Enabled)
                {
                    revertButton.Enabled = true;
                }
            }            
        }

        private void marginTypeComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_printSetup.SelectedMarginType = (MarginType)marginTypeComboBox.SelectedItem;

            System.Collections.ObjectModel.Collection<System.Windows.Forms.Control> controlsToEnableOrNot =
                new System.Collections.ObjectModel.Collection<System.Windows.Forms.Control>();
            controlsToEnableOrNot.Add(userDefinedMarginXTextBox);
            controlsToEnableOrNot.Add(userDefinedMarginYTextBox);
            if (m_printSetup.VerifyUserDefinedMargin(controlsToEnableOrNot))
            {
                userDefinedMarginXTextBox.Text = m_printSetup.UserDefinedMarginX.ToString();
                userDefinedMarginYTextBox.Text = m_printSetup.UserDefinedMarginY.ToString();
            }

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void vectorRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (vectorRadioButton.Checked)
            {
                m_printSetup.HiddenLineViews = HiddenLineViewsType.VectorProcessing;

                if (!revertButton.Enabled)
                {
                    revertButton.Enabled = true;
                }
            }

        }

        private void rasterRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (rasterRadioButton.Checked)
            {
                m_printSetup.HiddenLineViews = HiddenLineViewsType.RasterProcessing;

                if (!revertButton.Enabled)
                {
                    revertButton.Enabled = true;
                }
            }
        }

        private void fitToPageRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (fitToPageRadioButton.Checked)
            {
                m_printSetup.ZoomType = ZoomType.FitToPage;
                centerRadioButton.Checked = true;

                if (!revertButton.Enabled)
                {
                    revertButton.Enabled = true;
                }
            }
        }

        private void zoomRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (zoomRadioButton.Checked)
            {
                m_printSetup.ZoomType = ZoomType.Zoom;
                offsetRadioButton.Checked = true;
                m_printSetup.Zoom = (int)zoomPercentNumericUpDown.Value;

                if (!revertButton.Enabled)
                {
                    revertButton.Enabled = true;
                }
            }
        }

        private void zoomPercentNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (zoomRadioButton.Checked)
            {
                m_printSetup.Zoom = (int)zoomPercentNumericUpDown.Value;

                if (!revertButton.Enabled)
                {
                    revertButton.Enabled = true;
                }
            }
        }

        private void rasterQualityComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_printSetup.RasterQuality = (RasterQualityType)rasterQualityComboBox.SelectedItem;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void colorsComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_printSetup.Color = (ColorDepthType)colorsComboBox.SelectedItem;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void ViewLinksInBlueCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_printSetup.ViewLinksinBlue = ViewLinksInBlueCheckBox.Checked;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void hideScopeBoxedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_printSetup.HideScopeBoxes = hideScopeBoxedCheckBox.Checked;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void hideRefWorkPlanesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_printSetup.HideReforWorkPlanes = hideRefWorkPlanesCheckBox.Checked;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void hideCropBoundariesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_printSetup.HideCropBoundaries = hideCropBoundariesCheckBox.Checked;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void hideUnreferencedViewTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_printSetup.HideUnreferencedViewTages = hideUnreferencedViewTagsCheckBox.Checked;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void userDefinedMarginXTextBox_TextChanged(object sender, EventArgs e)
        {
            double doubleValue;
            if (!double.TryParse(userDefinedMarginXTextBox.Text, out doubleValue))
            {
                PrintMgr.MyMessageBox("Invalid input");
                return;
            }
            m_printSetup.UserDefinedMarginX = doubleValue;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void userDefinedMarginYTextBox_TextChanged(object sender, EventArgs e)
        {
            double doubleValue;
            if (!double.TryParse(userDefinedMarginYTextBox.Text, out doubleValue))
            {
                PrintMgr.MyMessageBox("Invalid input");
                return;
            }
            m_printSetup.UserDefinedMarginY = doubleValue;

            if (!revertButton.Enabled)
            {
                revertButton.Enabled = true;
            }
        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            using (SaveAsForm dlg = new SaveAsForm(m_printSetup))
            {
                dlg.ShowDialog();
            }

            m_stopUpdateFlag = true;
            printSetupsComboBox.DataSource = m_printSetup.PrintSettingNames;
            printSetupsComboBox.Update();
            m_stopUpdateFlag = false;

            printSetupsComboBox.SelectedItem = m_printSetup.SettingName;
        }

        private void renameButton_Click(object sender, EventArgs e)
        {
            using (ReNameForm dlg = new ReNameForm(m_printSetup))
            {
                dlg.ShowDialog();
            }

            m_stopUpdateFlag = true;
            printSetupsComboBox.DataSource = m_printSetup.PrintSettingNames;
            printSetupsComboBox.Update();
            m_stopUpdateFlag = false;

            printSetupsComboBox.SelectedItem = m_printSetup.SettingName;
        }

        private void revertButton_Click(object sender, EventArgs e)
        {
            m_printSetup.Revert();

            printSetupsComboBox_SelectedValueChanged(null, null);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            m_printSetup.Delete();

            m_stopUpdateFlag = true;
            printSetupsComboBox.DataSource = m_printSetup.PrintSettingNames;
            printSetupsComboBox.Update();
            m_stopUpdateFlag = false;

            printSetupsComboBox.SelectedItem = m_printSetup.SettingName;
        }
    }
}