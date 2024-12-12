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
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Collections;

namespace Revit.SDK.Samples.GridCreation.CS
{
    /// <summary>
    /// Class to validate input data before creating grids
    /// </summary>
    public static class Validation
    {
        // Get the resource contains strings
        static ResourceManager resManager = Properties.Resources.ResourceManager;

        /// <summary>
        /// Validate numbers in UI
        /// </summary>
        /// <param name="number1Ctrl">Control contains number information</param>
        /// <param name="number2Ctrl">Control contains another number information</param>
        /// <returns>Whether the numbers are validated</returns>
        public static bool ValidateNumbers(Control number1Ctrl, Control number2Ctrl)
        {
            if (!ValidateNumber(number1Ctrl) || !ValidateNumber(number2Ctrl))
            {
                return false;
            }

            if (Convert.ToUInt32(number1Ctrl.Text) == 0 && Convert.ToUInt32(number2Ctrl.Text) == 0)
            {
                ShowWarningMessage(resManager.GetString("NumbersCannotBeBothZero"),
                                   Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                number1Ctrl.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate number value
        /// </summary>
        /// <param name="numberCtrl">Control contains number information</param>
        /// <returns>Whether the number value is validated</returns>
        public static bool ValidateNumber(Control numberCtrl)
        {
            if (!ValidateNotNull(numberCtrl, "Number"))
            {
                return false;
            }

            try
            {
                uint number = Convert.ToUInt32(numberCtrl.Text);
                if (number > 200)
                {
                    ShowWarningMessage(resManager.GetString("NumberBetween0And200"),
                                       Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                    numberCtrl.Focus();
                    return false;
                }
            }
            catch(OverflowException)
            {
                ShowWarningMessage(resManager.GetString("NumberBetween0And200"),
                                   Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                numberCtrl.Focus();
                return false;
            }
            catch (Exception)
            {
                ShowWarningMessage(resManager.GetString("NumberFormatWrong"),
                                   Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                numberCtrl.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate length value
        /// </summary>
        /// <param name="lengthCtrl">Control contains length information</param>
        /// <param name="typeName">Type of length</param>
        /// <param name="canBeZero">Whether the length can be zero</param>
        /// <returns>Whether the length value is validated</returns>
        public static bool ValidateLength(Control lengthCtrl, String typeName, bool canBeZero)
        {
            if (!ValidateNotNull(lengthCtrl, typeName))
            {
                return false;
            }

            try
            {
                double length = Convert.ToDouble(lengthCtrl.Text);
                if (length <= 0 && !canBeZero)
                {
                    ShowWarningMessage(resManager.GetString(typeName + "CannotBeNegativeOrZero"),
                                       Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                    lengthCtrl.Focus();
                    return false;
                }
                else if (length < 0 && canBeZero)
                {
                    ShowWarningMessage(resManager.GetString(typeName + "CannotBeNegative"),
                                       Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                    lengthCtrl.Focus();
                    return false;
                }
            }
            catch (Exception)
            {
                ShowWarningMessage(resManager.GetString(typeName + "FormatWrong"),
                                   Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                lengthCtrl.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate coordinate value
        /// </summary>
        /// <param name="coordCtrl">Control contains coordinate information</param>
        /// <returns>Whether the coordinate value is validated</returns>
        public static bool ValidateCoord(Control coordCtrl)
        {
            if (!ValidateNotNull(coordCtrl, "Coordinate"))
            {
                return false;
            }

            try
            {
                Convert.ToDouble(coordCtrl.Text);
            }
            catch (Exception)
            {
                ShowWarningMessage(resManager.GetString("CoordinateFormatWrong"),
                                   Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                coordCtrl.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate start degree and end degree
        /// </summary>
        /// <param name="startDegree">Control contains start degree information</param>
        /// <param name="endDegree">Control contains end degree information</param>
        /// <returns>Whether the degree values are validated</returns>
        public static bool ValidateDegrees(Control startDegree, Control endDegree)
        {
            if (!ValidateDegree(startDegree) || !ValidateDegree(endDegree))
            {
                return false;
            }

            if (Math.Abs(Convert.ToDouble(startDegree.Text) - Convert.ToDouble(endDegree.Text)) <= Double.Epsilon)
            {
                ShowWarningMessage(resManager.GetString("DegreesAreTooClose"),
                                   Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                startDegree.Focus();
                return false;
            }

            if (Convert.ToDouble(startDegree.Text) >= Convert.ToDouble(endDegree.Text))
            {
                ShowWarningMessage(resManager.GetString("StartDegreeShouldBeLessThanEndDegree"),
                                   Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                startDegree.Focus();
                return false;
            }            

            return true;
        }

        /// <summary>
        /// Validate degree value
        /// </summary>
        /// <param name="degreeCtrl">Control contains degree information</param>
        /// <returns>Whether the degree value is validated</returns>
        public static bool ValidateDegree(Control degreeCtrl)
        {
            if (!ValidateNotNull(degreeCtrl, "Degree"))
            {
                return false;
            }

            try
            {
                double startDegree = Convert.ToDouble(degreeCtrl.Text);
                if (startDegree < 0 || startDegree > 360)
                {
                    ShowWarningMessage(resManager.GetString("DegreeWithin0To360"),
                                       Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                    degreeCtrl.Focus();
                    return false;
                }
            }
            catch (Exception)
            {
                ShowWarningMessage(resManager.GetString("DegreeFormatWrong"),
                                   Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                degreeCtrl.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate label
        /// </summary>
        /// <param name="labelCtrl">Control contains label information</param>
        /// <param name="allLabels">List contains all labels in Revit document</param>
        /// <returns>Whether the label value is validated</returns>
        public static bool ValidateLabel(Control labelCtrl, ArrayList allLabels)
        {
            if (!ValidateNotNull(labelCtrl, "Label"))
            {
                return false;
            }

            String labelToBeValidated = labelCtrl.Text;
            foreach (String label in allLabels)
            {
                if (label == labelToBeValidated)
                {
                    ShowWarningMessage(resManager.GetString("LabelExisted"),
                                       Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                    labelCtrl.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Assure value is not null
        /// </summary>
        /// <param name="control">Control contains information needs to be checked</param>
        /// <param name="typeName">Type of information</param>
        /// <returns>Whether the value is not null</returns>
        public static bool ValidateNotNull(Control control, String typeName)
        {
            if (String.IsNullOrEmpty(control.Text.TrimStart(' ').TrimEnd(' ')))
            {
                ShowWarningMessage(resManager.GetString(typeName + "CannotBeNull"),
                                   Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                control.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Assure two labels are not same
        /// </summary>
        /// <param name="label1Ctrl">Control contains label information</param>
        /// <param name="label2Ctrl">Control contains label information</param>
        /// <returns>Whether the labels are same</returns>
        public static bool ValidateLabels(Control label1Ctrl, Control label2Ctrl)
        {
            if (label1Ctrl.Text.TrimStart(' ').TrimEnd(' ') == label2Ctrl.Text.TrimStart(' ').TrimEnd(' '))
            {
                ShowWarningMessage(resManager.GetString("LabelsCannotBeSame"),
                                   Properties.Resources.ResourceManager.GetString("FailureCaptionInvalidValue"));
                label1Ctrl.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Show a warning message box
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="caption">title of message box</param>
        public static void ShowWarningMessage(String message, String caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
