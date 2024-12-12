//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    public partial class PrintMgrForm : System.Windows.Forms.Form
    {
        private PrintMgr m_printMgr;

        public PrintMgrForm(PrintMgr printMgr)
        {
            if (null == printMgr)
            {
                throw new ArgumentNullException("printMgr");
            }
            else
            {
                m_printMgr = printMgr;
            }

            InitializeComponent();
        }

        private void setupButton_Click(object sender, EventArgs e)
        {
            m_printMgr.ChangePrintSetup();
            printSetupNameLabel.Text = m_printMgr.PrintSetupName;
        }

        /// <summary>
        /// Initialize the UI data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintMgrForm_Load(object sender, EventArgs e)
        {
            printerNameComboBox.DataSource = m_printMgr.InstalledPrinterNames;
            // the selectedValueChange event have to add event handler after
            // data source be set, or else the delegate method will be invoked meaningless.
            this.printerNameComboBox.SelectedValueChanged += new System.EventHandler(this.printerNameComboBox_SelectedValueChanged);
            printerNameComboBox.SelectedItem = m_printMgr.PrinterName;
            if (m_printMgr.VerifyPrintToFile(printToFileCheckBox))
            {
                printToFileCheckBox.Checked = m_printMgr.IsPrintToFile;
            }

            System.Collections.ObjectModel.Collection<System.Windows.Forms.Control> controlsToEnableOrNot =
                new System.Collections.ObjectModel.Collection<System.Windows.Forms.Control>();
            controlsToEnableOrNot.Add(copiesNumericUpDown);
            controlsToEnableOrNot.Add(numberofcoyiesLabel);
            m_printMgr.VerifyCopies(controlsToEnableOrNot);

            controlsToEnableOrNot.Clear();
            controlsToEnableOrNot.Add(printToFileNameLabel);
            controlsToEnableOrNot.Add(printToFileNameTextBox);
            controlsToEnableOrNot.Add(browseButton);
            m_printMgr.VerifyPrintToFileName(controlsToEnableOrNot);

            m_printMgr.VerifyPrintToSingleFile(singleFileRadioButton);  

            if (m_printMgr.VerifyPrintToSingleFile(singleFileRadioButton))
            {
                singleFileRadioButton.Checked = m_printMgr.IsCombinedFile;
                separateFileRadioButton.Checked = !m_printMgr.IsCombinedFile;
            }
            

            if (!m_printMgr.VerifyPrintToSingleFile(singleFileRadioButton)
                && m_printMgr.VerifyPrintToSeparateFile(separateFileRadioButton))
            {
                separateFileRadioButton.Checked = true;
            } 
            this.singleFileRadioButton.CheckedChanged += new System.EventHandler(this.combineRadioButton_CheckedChanged);
                        
            switch (m_printMgr.PrintRange)
            {
                case PrintRange.Current:
                    currentWindowRadioButton.Checked = true;
                    break;
                case PrintRange.Select:
                    selectedViewsRadioButton.Checked = true;
                    break;
                case PrintRange.Visible:
                    visiblePortionRadioButton.Checked = true;
                    break;
                default:
                    break;
            }
            this.currentWindowRadioButton.CheckedChanged += new System.EventHandler(this.currentWindowRadioButton_CheckedChanged);
            this.visiblePortionRadioButton.CheckedChanged += new System.EventHandler(this.visiblePortionRadioButton_CheckedChanged);
            this.selectedViewsRadioButton.CheckedChanged += new System.EventHandler(this.selectedViewsRadioButton_CheckedChanged);

            this.printToFileNameTextBox.Text = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments) + "\\" + m_printMgr.DocumentTitle;
            controlsToEnableOrNot.Clear();
            controlsToEnableOrNot.Add(selectedViewSheetSetLabel);
            controlsToEnableOrNot.Add(selectedViewSheetSetButton);
            if (m_printMgr.VerifySelectViewSheetSet(controlsToEnableOrNot))
            {
                this.selectedViewSheetSetLabel.Text = m_printMgr.SelectedViewSheetSetName;
            }

            orderCheckBox.Checked = m_printMgr.PrintOrderReverse;
            this.orderCheckBox.CheckedChanged += new System.EventHandler(this.orderCheckBox_CheckedChanged);

            if (m_printMgr.VerifyCollate(collateCheckBox))
            {
                collateCheckBox.Checked = m_printMgr.Collate;
            }
            this.collateCheckBox.CheckedChanged += new System.EventHandler(this.collateCheckBox_CheckedChanged);

            printSetupNameLabel.Text = m_printMgr.PrintSetupName;
        }

        private void printerNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_printMgr.PrinterName = printerNameComboBox.SelectedItem as string;

            // Verify the relative controls is enable or not, according to the printer changed.
            m_printMgr.VerifyPrintToFile(printToFileCheckBox);

            System.Collections.ObjectModel.Collection<System.Windows.Forms.Control> controlsToEnableOrNot =
                new System.Collections.ObjectModel.Collection<System.Windows.Forms.Control>();
            controlsToEnableOrNot.Add(copiesNumericUpDown);
            controlsToEnableOrNot.Add(numberofcoyiesLabel);
            m_printMgr.VerifyCopies(controlsToEnableOrNot);

            controlsToEnableOrNot.Clear();
            controlsToEnableOrNot.Add(printToFileNameLabel);
            controlsToEnableOrNot.Add(printToFileNameTextBox);
            controlsToEnableOrNot.Add(browseButton);

            if (!string.IsNullOrEmpty(printToFileNameTextBox.Text))
            {
                printToFileNameTextBox.Text = printToFileNameTextBox.Text.Remove(
                    printToFileNameTextBox.Text.LastIndexOf(".")) + m_printMgr.PostFix;
            }
            
            m_printMgr.VerifyPrintToFileName(controlsToEnableOrNot);

            m_printMgr.VerifyPrintToSingleFile(singleFileRadioButton);
            m_printMgr.VerifyPrintToSeparateFile(separateFileRadioButton);
            
        }

        private void printToFileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_printMgr.IsPrintToFile = printToFileCheckBox.Checked;

            // Verify the relative controls is enable or not, according to the print to file
            // check box is checked or not.
            System.Collections.ObjectModel.Collection<System.Windows.Forms.Control> controlsToEnableOrNot =
                new System.Collections.ObjectModel.Collection<System.Windows.Forms.Control>();
            controlsToEnableOrNot.Add(copiesNumericUpDown);
            controlsToEnableOrNot.Add(numberofcoyiesLabel);
            m_printMgr.VerifyCopies(controlsToEnableOrNot);

            controlsToEnableOrNot.Clear();
            controlsToEnableOrNot.Add(printToFileNameLabel);
            controlsToEnableOrNot.Add(printToFileNameTextBox);
            controlsToEnableOrNot.Add(browseButton);
            m_printMgr.VerifyPrintToFileName(controlsToEnableOrNot);

            m_printMgr.VerifyPrintToSingleFile(singleFileRadioButton);            
        }

        private void combineRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (m_printMgr.VerifyPrintToSingleFile(singleFileRadioButton))
            {
                m_printMgr.IsCombinedFile = singleFileRadioButton.Checked;
            }            
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            string newName = m_printMgr.ChangePrintToFileName();
            if (!string.IsNullOrEmpty(newName))
            {
                printToFileNameTextBox.Text = newName;
            }
        }

        private void currentWindowRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (currentWindowRadioButton.Checked)
            {
                m_printMgr.PrintRange = Autodesk.Revit.DB.PrintRange.Current;

                System.Collections.ObjectModel.Collection<System.Windows.Forms.Control> controlsToEnableOrNot =
                new System.Collections.ObjectModel.Collection<System.Windows.Forms.Control>();
                controlsToEnableOrNot.Add(selectedViewSheetSetLabel);
                controlsToEnableOrNot.Add(selectedViewSheetSetButton);
                m_printMgr.VerifySelectViewSheetSet(controlsToEnableOrNot);

                if (m_printMgr.VerifyPrintToSingleFile(singleFileRadioButton))
                {
                    m_printMgr.IsCombinedFile = true;
                    singleFileRadioButton.Checked = true;
                    separateFileRadioButton.Checked = false;
                }
                m_printMgr.VerifyPrintToSeparateFile(separateFileRadioButton);
                m_printMgr.VerifyCollate(collateCheckBox);
            }
        }

        private void visiblePortionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (visiblePortionRadioButton.Checked)
            {
                m_printMgr.PrintRange = Autodesk.Revit.DB.PrintRange.Visible;

                System.Collections.ObjectModel.Collection<System.Windows.Forms.Control> controlsToEnableOrNot =
                new System.Collections.ObjectModel.Collection<System.Windows.Forms.Control>();
                controlsToEnableOrNot.Add(selectedViewSheetSetLabel);
                controlsToEnableOrNot.Add(selectedViewSheetSetButton);
                m_printMgr.VerifySelectViewSheetSet(controlsToEnableOrNot);

                if (m_printMgr.VerifyPrintToSingleFile(singleFileRadioButton))
                {
                    m_printMgr.IsCombinedFile = true;
                    singleFileRadioButton.Checked = true;
                    separateFileRadioButton.Checked = false;
                }
                m_printMgr.VerifyPrintToSeparateFile(separateFileRadioButton);
                m_printMgr.VerifyCollate(collateCheckBox);
            }
        }

        private void selectedViewsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedViewsRadioButton.Checked)
            {
                m_printMgr.PrintRange = Autodesk.Revit.DB.PrintRange.Select;

                System.Collections.ObjectModel.Collection<System.Windows.Forms.Control> controlsToEnableOrNot =
                new System.Collections.ObjectModel.Collection<System.Windows.Forms.Control>();
                controlsToEnableOrNot.Add(selectedViewSheetSetLabel);
                controlsToEnableOrNot.Add(selectedViewSheetSetButton);
                m_printMgr.VerifySelectViewSheetSet(controlsToEnableOrNot);

                m_printMgr.VerifyPrintToSingleFile(singleFileRadioButton);
                if (m_printMgr.VerifyPrintToSeparateFile(separateFileRadioButton))
                {
                    separateFileRadioButton.Checked = true;
                }
                m_printMgr.VerifyPrintToSeparateFile(separateFileRadioButton);
                m_printMgr.VerifyCollate(collateCheckBox);
            }
            
        }

        private void orderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_printMgr.PrintOrderReverse = orderCheckBox.Checked;
        }

        private void collateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_printMgr.Collate = collateCheckBox.Checked;
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            m_printMgr.SelectViewSheetSet();
            selectedViewSheetSetLabel.Text = m_printMgr.SelectedViewSheetSetName;
        }

        private void copiesNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_printMgr.CopyNumber = (int)(copiesNumericUpDown.Value);
            }
            catch (InvalidOperationException)
            {
                collateCheckBox.Enabled = false;
                return;
            }

            m_printMgr.VerifyCollate(collateCheckBox);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                m_printMgr.SubmitPrint();
            }
            catch (Exception)
            {
                PrintMgr.MyMessageBox("Print Failed");
            }
        }
    }
}