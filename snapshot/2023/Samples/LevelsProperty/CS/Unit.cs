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
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.LevelsProperty.CS
{
    /// <summary>
    /// Provides static functions to convert unit
    /// </summary>
    static class Unit
    {
        #region Methods
        /// <summary>
        /// Convert the value get from RevitAPI to the value indicated by ForgeTypeId
        /// </summary>
        /// <param name="to">ForgeTypeId indicates unit of target value</param>
        /// <param name="value">value get from RevitAPI</param>
        /// <returns>Target value</returns>
        public static double CovertFromAPI(ForgeTypeId to, double value)
        {
            return UnitUtils.ConvertFromInternalUnits(value, to);
        }

        /// <summary>
        /// Convert a value indicated by ForgeTypeId to the value used by RevitAPI
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="from">ForgeTypeId indicates the unit of the value to be converted</param>
        /// <returns>Target value</returns>
        public static double CovertToAPI(double value, ForgeTypeId from)
        {
            return UnitUtils.ConvertToInternalUnits(value, from);
        }
        #endregion
    }
}
