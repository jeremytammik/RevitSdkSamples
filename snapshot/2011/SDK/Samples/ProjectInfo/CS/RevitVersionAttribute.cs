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
using System.Text;
using System.Collections.ObjectModel;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// Attribute which designates Revit version names
    /// </summary>
    public sealed class RevitVersionAttribute : Attribute
    {
        #region Fields
        /// <summary>
        /// Revit version name array
        /// </summary>
        List<ProductType> m_products = new List<ProductType>(); 
        #endregion

        #region Properties
        /// <summary>
        /// Gets Revit version names
        /// </summary>
        public ReadOnlyCollection<ProductType> Names
        {
            get { return m_products.AsReadOnly(); }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes Revit version name array
        /// </summary>
        /// <param name="names"></param>
        public RevitVersionAttribute(params ProductType[] names)
        {
            m_products.AddRange(names);
        } 
        #endregion
    };
}
