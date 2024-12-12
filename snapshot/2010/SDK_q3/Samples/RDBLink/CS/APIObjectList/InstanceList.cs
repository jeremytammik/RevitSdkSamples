//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.Data;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides export and import functions for Revit instances.
    /// </summary>
    public class InstanceList : SymbolList
    {
        #region Properties
        /// <summary>
        /// whether user can create a new record in a table
        /// </summary>
        public override bool SupportCreate
        {
            get
            {
                return false;
            }
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Create an element in Revit using the information stored in a DataRow
        /// </summary>
        /// <param name="row">DataRow stores the creation information</param>
        /// <returns>An element if create successfully, otherwise null</returns>
        protected override Element CreateNewElement(DataRow row)
        {
            return null;
        } 
        #endregion
    };
}
