//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.WindowWizard.CS
{
    /// <summary>
    /// class is used to validate window parameters
    /// </summary>
    public class ValidateWindowParameter
    {
        #region Class Memeber Variables
        /// <summary>
        /// store the wall's height
        /// </summary>
        private double m_wallHeight=10;
        
        /// <summary>
        /// store the wall's width
        /// </summary>        
        private double m_wallWidth=10;

        /// <summary>
        /// indicate the template file is metric or not
        /// </summary>
        public bool IsMetric;
        #endregion

       /// <summary>
        /// constructor of ValidateWindowParameter
       /// </summary>
       /// <param name="wallHeight">wall height parameter</param>
       /// <param name="wallWidth">wall width parameter</param>
        public ValidateWindowParameter(double wallHeight, double wallWidth)
        {
            if (wallHeight >= 0)
            {
                m_wallHeight = wallHeight;
            }
            if (wallWidth >= 0)
            {
                m_wallWidth = wallWidth;
            }

        }

        #region Class Implementation
        /// <summary>
        /// This method is used to check whether a value string is double type
        /// </summary>
        /// <param name="value">>the string value</param>
        /// <param name="result">the double result</param>
        /// <returns>the validation result message</returns>
        public string IsDouble(string value, ref double result)
        {
            if (Double.TryParse("0" + value, out result))
            {
                return string.Empty;
            }
            else
            {
                return "Please input a double value.";
            }
        }

        /// <summary>
        /// This method is used to check whether the width value is out of range
        /// </summary>
        /// <param name="value">the string value</param>
        /// <returns>the validation result message</returns>
        public string IsWidthInRange(double value)
        {
            if (IsMetric)
            {
                value = Utility.MetricToImperial(value);
                if (value >= 0.23 && value < m_wallWidth)
                    return string.Empty;
                else
                    return "The width should be between 69 and " + Convert.ToInt32(Utility.ImperialToMetric(m_wallWidth));
            }
            else
            {
                if (value >= 0.4 && value < m_wallWidth)
                    return string.Empty;
                else
                    return "The width should be between 0.4 and " + m_wallWidth;
            }
        }

        /// <summary>
        /// This method is used to check whether the height value is out of range
        /// </summary>
        /// <param name="value">the string value</param>
        /// <returns>the validation result message</returns>
        public string IsHeightInRange(double value)
        {
            if (IsMetric)
            {
                value = Utility.MetricToImperial(value);
                if (value >= 0.23)
                    return string.Empty;
                else
                    return "The height should > 69";
            }
            else
            {
                if (value >= 0.4)
                    return string.Empty;
                else
                    return "The height should > 0.4";
            }
        }

        /// <summary>
        ///  This method is used to check whether the inset value is out of range
        /// </summary>
        /// <param name="value">the string value</param>
        /// <returns>the validation result message</returns>
        public string IsInsetInRange(double value)
        {
            if (IsMetric)
                value = Utility.MetricToImperial(value);
            if (value >= 0)
                return string.Empty;
            else
                return "The Inset should > 0";
        }

        /// <summary>
        ///  This method is used to check whether the sillheight value is out of range
        /// </summary>
        /// <param name="value">the string value</param>
        /// <returns>the validation result message</returns>
        public string IsSillHeightInRange(double value)
        {
            if (IsMetric)
            {
                value = Utility.MetricToImperial(value);
                if (value < m_wallHeight)
                    return string.Empty;
                else
                    return "The sillheight should be < " + Convert.ToInt32(Utility.ImperialToMetric(m_wallHeight));
            }
            else
            {              
                if (value < m_wallHeight)
                    return string.Empty;
                else
                    return "The sillheight should be < " + m_wallHeight;
            }
        }        
        #endregion
    }
}
