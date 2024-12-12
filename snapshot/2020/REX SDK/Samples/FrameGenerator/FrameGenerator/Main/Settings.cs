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
using System.Threading;
using System.Reflection;

using REX.Common;
using Autodesk.REX.Framework;

namespace REX.FrameGenerator
{
    /// <summary>
    /// Allows parsing module settings located in Configuration/settings.xml file.
    /// </summary>
    internal class ExtensionSettings : REXExtensionSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionSettings"/> class.
        /// </summary>
        /// <param name="Ext">The reference to REXExtension object.</param>
        public ExtensionSettings(REXExtension Ext)
            : base(Ext)
        {
        }

        /// <summary>
        /// Gets the main extension.
        /// </summary>
        /// <value>The main extension.</value>
        internal Extension ThisMainExtension
        {
            get
            {
                return (Extension)ThisExtension;
            }
        }

        /// <summary>
        /// Called when parsing Configuration / settings.xml
        /// file. Concerns only tags which is not supported by the
        /// class.
        /// </summary>
        /// <param name="Node">The XML node.</param>
        /// <returns>
        /// <c>true</c>If is successfully; otherwise, <c>false</c> -
        /// reserved only for exceptions. 
        /// </returns>                                                  
        public override bool OnParse(System.Xml.XmlNode Node)
        {
            return true;
        }
    }
}
