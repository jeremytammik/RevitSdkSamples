//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Windows.Forms;

using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.WinderStairs.CS
{
    /// <summary>
    /// This form is used to collect parameters for UWinder creation. It also validates the input
    /// parameters and will warn if there is any invalid parameters when trying to submit the form.
    /// </summary>
    partial class UWinderOptions : Form
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public UWinderOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Number of straight steps at start.
        /// </summary>
        public uint NumStepsAtStart
        {
            get
            {
                return uint.Parse(numAtStartTextBox.Text);
            }
            set
            {
                numAtStartTextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// Number of steps in the first winder corner.
        /// </summary>
        public uint NumStepsInCorner1
        {
            get
            {
                return uint.Parse(numInCorner1TextBox.Text);
            }
            set
            {
                numInCorner1TextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// Number of straight steps in middle.
        /// </summary>
        public uint NumStepsInMiddle
        {
            get
            {
                return uint.Parse(numInMiddleTextBox.Text);
            }
            set
            {
                numInMiddleTextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// Number of steps in the second winder corner.
        /// </summary>
        public uint NumStepsInCorner2
        {
            get
            {
                return uint.Parse(numInCorner2TextBox.Text);
            }
            set
            {
                numInCorner2TextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// Number of straight steps at end.
        /// </summary>
        public uint NumStepsAtEnd
        {
            get
            {
                return uint.Parse(numAtEndTextBox.Text);
            }
            set
            {
                numAtEndTextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// Winder stairs run width.
        /// </summary>
        public double RunWidth
        {
            get
            {
                return double.Parse(runWidthTextBox.Text);
            }
            set
            {
                runWidthTextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// Center point offset distance from the first corner.
        /// </summary>
        public double CenterOffsetE1
        {
            get
            {
                return double.Parse(centerOffsetE1TextBox.Text);
            }
            set
            {
                centerOffsetE1TextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// Center point offset distance from the first corner.
        /// </summary>
        public double CenterOffsetF1
        {
            get
            {
                return double.Parse(centerOffsetF1TextBox.Text);
            }
            set
            {
                centerOffsetF1TextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// Center point offset distance from the second corner.
        /// </summary>
        public double CenterOffsetE2
        {
            get
            {
                return double.Parse(centerOffsetE2TextBox.Text);
            }
            set
            {
                centerOffsetE2TextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// Center point offset distance from the second corner.
        /// </summary>
        public double CenterOffsetF2
        {
            get
            {
                return double.Parse(centerOffsetF2TextBox.Text);
            }
            set
            {
                centerOffsetF2TextBox.Text = value.ToString();
            }
        }

        /// <summary>
        /// A switch indicates whether to support DMU(dynamic model update).
        /// </summary>
        public bool DMU
        {
            get
            {
                return dmuCheckBox.Checked;
            }
        }

        /// <summary>
        /// A switch to control the sketch drawing of winder stairs.
        /// </summary>
        public bool Sketch
        {
            get
            {
                return sketchCheckBox.Checked;
            }
        }

        /// <summary>
        /// Validate the UI input and it will warn if there are invalid user inputs.
        /// </summary>
        /// <returns>true if all input is fine.</returns>
        bool ValidateInput()
        {
            double runWidth;
            if (!double.TryParse(runWidthTextBox.Text, out runWidth) || runWidth < 1.0e-6)
            {
                TaskDialog.Show("U-Winder Warning", "Run Width should be positive double", TaskDialogCommonButtons.Ok);
                runWidthTextBox.Focus();
                runWidthTextBox.SelectAll();
                return false;
            }

            uint numAtStart;
            if (!uint.TryParse(numAtStartTextBox.Text, out numAtStart))
            {
                TaskDialog.Show("U-Winder Warning", "Start steps should be unsigned integer", TaskDialogCommonButtons.Ok);
                numAtStartTextBox.Focus();
                numAtStartTextBox.SelectAll();
                return false;
            }

            uint numInMiddle;
            if (!uint.TryParse(numInMiddleTextBox.Text, out numInMiddle))
            {
                TaskDialog.Show("U-Winder Warning", "Middle steps should be unsigned integer", TaskDialogCommonButtons.Ok);
                numInMiddleTextBox.Focus();
                numInMiddleTextBox.SelectAll();
                return false;
            }

            uint numAtEnd;
            if (!uint.TryParse(numAtEndTextBox.Text, out numAtEnd))
            {
                TaskDialog.Show("U-Winder Warning", "End steps should be unsigned integer", TaskDialogCommonButtons.Ok);
                numAtEndTextBox.Focus();
                numAtEndTextBox.SelectAll();
                return false;
            }

            uint numInCorner1;
            if (!uint.TryParse(numInCorner1TextBox.Text, out numInCorner1) || numInCorner1 < 1)
            {
                TaskDialog.Show("U-Winder Warning", "The first corner steps should be unsigned integer and >= 1", TaskDialogCommonButtons.Ok);
                numInCorner1TextBox.Focus();
                numInCorner1TextBox.SelectAll();
                return false;
            }

            double offsetE1;
            if (!double.TryParse(centerOffsetE1TextBox.Text, out offsetE1) || offsetE1 < 0.0)
            {
                TaskDialog.Show("U-Winder Warning", "Center offset (E1) should be non-negative double", TaskDialogCommonButtons.Ok);
                centerOffsetE1TextBox.Focus();
                centerOffsetE1TextBox.SelectAll();
                return false;
            }

            double offsetF1;
            if (!double.TryParse(centerOffsetF1TextBox.Text, out offsetF1) || offsetF1 < 0.0)
            {
                TaskDialog.Show("U-Winder Warning", "Center offset (F1) should be non-negative double", TaskDialogCommonButtons.Ok);
                centerOffsetF1TextBox.Focus();
                centerOffsetF1TextBox.SelectAll();
                return false;
            }

            uint numInCorner2;
            if (!uint.TryParse(numInCorner2TextBox.Text, out numInCorner2) || numInCorner2 < 1)
            {
                TaskDialog.Show("U-Winder Warning", "The second corner steps should be unsigned integer and >= 1", TaskDialogCommonButtons.Ok);
                numInCorner2TextBox.Focus();
                numInCorner2TextBox.SelectAll();
                return false;
            }

            double offsetE2;
            if (!double.TryParse(centerOffsetE2TextBox.Text, out offsetE2) || offsetE2 < 0.0)
            {
                TaskDialog.Show("U-Winder Warning", "Center offset (E2) should be non-negative double", TaskDialogCommonButtons.Ok);
                centerOffsetE2TextBox.Focus();
                centerOffsetE2TextBox.SelectAll();
                return false;
            }

            double offsetF2;
            if (!double.TryParse(centerOffsetF2TextBox.Text, out offsetF2) || offsetF2 < 0.0)
            {
                TaskDialog.Show("U-Winder Warning", "Center offset (F2) should be non-negative double", TaskDialogCommonButtons.Ok);
                centerOffsetF2TextBox.Focus();
                centerOffsetF2TextBox.SelectAll();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Submit the UI input.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                this.Close();
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Cancel the UI input.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
