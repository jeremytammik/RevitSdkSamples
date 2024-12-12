//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.


/// <summary>
/// </summary>

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

using Autodesk.REX.Framework;
using REX.Common;

namespace REX.Unit
{
    /// <summary>
    /// Main class for REX manager and interactions with system.
    /// </summary>
    public class Application : REXFoundationApplication
    {
        protected override void OnCreateApplication()
        {
            base.OnCreateApplication();

            ApplicationRef = new FoundationApplication();
        }
    }

    /// <summary>
    /// Main class for REX manager and interactions with system
    /// </summary>
    /// <remarks></remarks>
    public class FoundationApplication : REXApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// Creates Extension class.
        /// </summary>
        public FoundationApplication()
        {
            AppExtension = new Extension(this);
        }

        /// <summary>
        /// Called when create. Set ExtensionContext structure and enable or disable some functionalities.
        /// </summary>
        /// <returns>Returns true if succeeded.</returns>
        public override bool OnCreate()
        {
            // insert code here

            return true;
        }

        /// <summary>
        /// Get the extension.
        /// </summary>
        /// <returns>Returns reference to Extension object.</returns>
        public override REXExtension GetExtension()
        {
            // insert code here

            return AppExtension;
        }

        /// <summary>
        /// Get the minimum required version of specified engine's component.
        /// </summary>
        /// <param name="Version">The version.</param>
        /// <returns>Returns version string - "Major.Minor".</returns>
        public override string GetVersion(REXVersionType Version)
        {
            switch (Version)
            {
                case REXVersionType.Common:
                case REXVersionType.Controls:
                case REXVersionType.Math:
                    return "1.0";
                case REXVersionType.Engine:
                    return "2.3";
                case REXVersionType.CommercialEngine:
                    return "2.3";
            }
            return "";
        }

        /// <summary>
        /// Get the text. See <see cref="Extension.GetText()"/> method.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns>Returns specified text.</returns>
        public override string GetText(string Name)
        {
            return AppExtension.OnGetText(Name);
        }

        #region Private members

        private Extension AppExtension;

        #endregion
    }
}
