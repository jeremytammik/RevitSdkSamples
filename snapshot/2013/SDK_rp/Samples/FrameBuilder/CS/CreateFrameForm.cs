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

namespace Revit.SDK.Samples.FrameBuilder.CS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Reflection;

    using Autodesk.Revit.DB;

    /// <summary>
    /// main form to create framing
    /// </summary>
    public partial class CreateFrameForm : System.Windows.Forms.Form
    {
        private FrameData m_frameData;        // necessary data to create framing

        /// <summary>
        /// constructor without parameter is forbidden
        /// </summary>
        private CreateFrameForm()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="data">necessary data to create</param>
        public CreateFrameForm(FrameData data)
        {
            InitializeComponent();
            m_frameData = data;
        }

        /// <summary>
        /// initialize controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFramingForm_Load(object sender, EventArgs e)
        {
            distanceTextBox.Text = m_frameData.Distance.ToString();
            xNumberTextBox.Text = m_frameData.XNumber.ToString();
            yNumberTextBox.Text = m_frameData.YNumber.ToString();
            floorNumberTextBox.Text = m_frameData.FloorNumber.ToString();
            levelHeightTextBox.Text = m_frameData.LevelHeight.ToString();
            originXtextBox.Text = m_frameData.FrameOrigin.U.ToString();
            originYtextBox.Text = m_frameData.FrameOrigin.V.ToString();
            originAngletextBox.Text = m_frameData.FrameOriginAngle.ToString();

            RefreshListControl(columnTypeComboBox, m_frameData.ColumnSymbolsMgr);
            RefreshListControl(beamTypeComboBox, m_frameData.BeamSymbolsMgr);
            RefreshListControl(braceTypeComboBox, m_frameData.BraceSymbolsMgr);
        }

        /// <summary>
        /// duplicate column type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void columnDuplicateButton_Click(object sender, EventArgs e)
        {
            if (DuplicateSymbol(m_frameData.ColumnSymbolsMgr, columnTypeComboBox.SelectedValue))
            {
                RefreshListControl(columnTypeComboBox, m_frameData.ColumnSymbolsMgr);
            }
        }

        /// <summary>
        /// duplicate beam type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beamDuplicateButton_Click(object sender, EventArgs e)
        {
            if (DuplicateSymbol(m_frameData.BeamSymbolsMgr, beamTypeComboBox.SelectedValue))
            {
                RefreshListControl(beamTypeComboBox, m_frameData.BeamSymbolsMgr);
                RefreshListControl(braceTypeComboBox, m_frameData.BraceSymbolsMgr);
            }
        }

        /// <summary>
        /// duplicate brace type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void braceDuplicateButton_Click(object sender, EventArgs e)
        {
            if (DuplicateSymbol(m_frameData.BraceSymbolsMgr, braceTypeComboBox.SelectedValue))
            {
                RefreshListControl(beamTypeComboBox, m_frameData.BeamSymbolsMgr);
                RefreshListControl(braceTypeComboBox, m_frameData.BraceSymbolsMgr);
            }
        }

        /// <summary>
        /// provide user UI to duplicate FamilySymbols
        /// </summary>
        /// <param name="typesMgr">data manager of FamilySymbols</param>
        /// <param name="symbol">FamilySymbol to be copied</param>
        /// <returns>does duplicate</returns>
        private static bool DuplicateSymbol(FrameTypesMgr typesMgr, Object symbol)
        {
            bool result = false;
            using (DuplicateTypeForm typeFrm = new DuplicateTypeForm(symbol, typesMgr))
            {
                if (typeFrm.ShowDialog() == DialogResult.OK)
                {
                    result = true;
                }
            }
            return result;
        }
        
        /// <summary>
        /// refresh the ListControl's datasource
        /// </summary>
        /// <param name="list">ListControl to be refreshed</param>
        private static void RefreshListControl(ListControl list, FrameTypesMgr typesMgr)
        {
            // refresh control's data
            list.DataSource = null;
            list.DataSource = typesMgr.FramingSymbols;
            list.DisplayMember = "Name";
            list.SelectedIndex = 0;
        }

        /// <summary>
        /// validate the input value
        /// set back to its old value if input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void distanceTextBox_Validating(object sender, CancelEventArgs e)
        {
            double value;
            try
            {
                value = double.Parse(distanceTextBox.Text);
                m_frameData.Distance = value;
            }
            catch (FormatException formatEx)
            {
                Debug.WriteLine(formatEx.Message);
                MessageBox.Show("Please input a integer.");
                e.Cancel = true;
            }
            catch (ErrorMessageException msgEx)
            {
                distanceTextBox.Text = m_frameData.Distance.ToString();
                MessageBox.Show(msgEx.Message, "Revit");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// validate the input value
        /// set back to its old value if input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xNumberTextBox_Validating(object sender, CancelEventArgs e)
        {
            int value;
            try
            {
                value = int.Parse(xNumberTextBox.Text);
                m_frameData.XNumber = value;
            }
            catch (FormatException formatEx)
            {
                Debug.WriteLine(formatEx.Message);
                MessageBox.Show("Please input a integer.");
                e.Cancel = true;
            }
            catch (ErrorMessageException msgEx)
            {
                xNumberTextBox.Text = m_frameData.XNumber.ToString();
                MessageBox.Show(msgEx.Message, "Revit");
                e.Cancel = true;
            }
         catch (Exception ex)
         {
            Debug.WriteLine(ex.Message);
            MessageBox.Show("Please input a valid integer.");
            e.Cancel = true;
         }
        }

        /// <summary>
        /// validate the input value
        /// set back to its old value if input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void yNumberTextBox_Validating(object sender, CancelEventArgs e)
        {
            int value;
            try
            {
                value = int.Parse(yNumberTextBox.Text);
                m_frameData.YNumber = value;
            }
            catch (FormatException formatEx)
            {
                Debug.WriteLine(formatEx.Message);
                MessageBox.Show("Please input a integer.");
                e.Cancel = true;
            }
            catch (ErrorMessageException msgEx)
            {
                yNumberTextBox.Text = m_frameData.YNumber.ToString();
                MessageBox.Show(msgEx.Message, "Revit");
                e.Cancel = true;
            }
         catch (Exception ex)
         {
            Debug.WriteLine(ex.Message);
            MessageBox.Show("Please input a valid integer.");
            e.Cancel = true;
         }
        }

        /// <summary>
        /// validate the input value
        /// set back to its old value if input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void floorNumberTextBox_Validating(object sender, CancelEventArgs e)
        {
            int value;
         try
         {
            value = int.Parse(floorNumberTextBox.Text);
            m_frameData.FloorNumber = value;
         }
         catch (FormatException formatEx)
         {
            Debug.WriteLine(formatEx.Message);
            MessageBox.Show("Please input a integer.");
            e.Cancel = true;
         }
         catch (ErrorMessageException msgEx)
         {
            floorNumberTextBox.Text = m_frameData.FloorNumber.ToString();
            MessageBox.Show(msgEx.Message, "Revit");
            e.Cancel = true;
         }
         catch (Exception ex)
         {
            Debug.WriteLine(ex.Message);
            MessageBox.Show("Please input a valid integer.");
            e.Cancel = true;
         }
            // change readonly property of levelHeightTextBox
            if ((m_frameData.FloorNumber + 1) > m_frameData.Levels.Count)
            {
                levelHeightTextBox.Enabled = true;
            }
            else
            {
                levelHeightTextBox.Enabled = false;
            }
        }

        /// <summary>
        /// validate the input value
        /// set back to its old value if input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelHeightTextBox_Validating(object sender, CancelEventArgs e)
        {
            double value;
            try
            {
                value = double.Parse(levelHeightTextBox.Text);
                m_frameData.LevelHeight = value;
            }
            catch (FormatException formatEx)
            {
                Debug.WriteLine(formatEx.Message);
                MessageBox.Show("Please input a integer.");
                e.Cancel = true;
            }
            catch (ErrorMessageException msgEx)
            {
                levelHeightTextBox.Text = m_frameData.LevelHeight.ToString();
                MessageBox.Show(msgEx.Message, "Revit");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// validate the input value
        /// set back to its old value if input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void originXtextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                double value = double.Parse(originXtextBox.Text);
                Autodesk.Revit.DB.UV oldValue = m_frameData.FrameOrigin;
                m_frameData.FrameOrigin = new Autodesk.Revit.DB.UV (value, oldValue.V);
            }
            catch (FormatException formatEx)
            {
                Debug.WriteLine(formatEx.Message);
                MessageBox.Show("Please input a number.");
                e.Cancel = true;
            }
            catch (ErrorMessageException msgEx)
            {
                originXtextBox.Text = m_frameData.FrameOrigin.U.ToString();
                MessageBox.Show(msgEx.Message, "Revit");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// validate the input value
        /// set back to its old value if input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void originYtextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                double value = double.Parse(originYtextBox.Text);
                Autodesk.Revit.DB.UV oldValue = m_frameData.FrameOrigin;
                m_frameData.FrameOrigin = new Autodesk.Revit.DB.UV (oldValue.U, value);
            }
            catch (FormatException formatEx)
            {
                Debug.WriteLine(formatEx.Message);
                MessageBox.Show("Please input a number.");
                e.Cancel = true;
            }
            catch (ErrorMessageException msgEx)
            {
                originYtextBox.Text = m_frameData.FrameOrigin.V.ToString();
                MessageBox.Show(msgEx.Message, "Revit");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// validate the input value
        /// set back to its old value if input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void originAngletextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                PropertyInfo pInfo = typeof(FrameData).GetProperty("FrameOriginAngle");
            }
            catch
            {
                Debug.Assert(true, "Reflection binding failed because interface of FrameData changed");
            }
            try
            {
                m_frameData.FrameOriginAngle = double.Parse(originAngletextBox.Text);
            }
            catch (FormatException formatEx)
            {
                Debug.WriteLine(formatEx.Message);
                MessageBox.Show("Please input a number.");
                e.Cancel = true;
            }
            catch (ErrorMessageException msgEx)
            {
                originAngletextBox.Text = m_frameData.FrameOriginAngle.ToString();
                MessageBox.Show(msgEx.Message, "Revit");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// create the Frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// cancel all command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// set type of column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void columnTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_frameData.SetColumnSymbol(columnTypeComboBox.SelectedItem);
        }

        /// <summary>
        /// set type of beam
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beamTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_frameData.SetBeamSymbol(beamTypeComboBox.SelectedItem);
        }

        /// <summary>
        /// set type of brace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void braceTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_frameData.SetBraceSymbol(braceTypeComboBox.SelectedItem);
        }

    }
}