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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace REX.ContentGeneratorWPF.Resources.Dialogs
{
    /// <summary>
    /// Represents the control which allows the user to see the properties of the selected element.
    /// </summary>
    public partial class SelectedElementControl : REX.Common.REXExtensionControl
    {
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
        /// Initializes a new instance of the <see cref="SelectedElementControl"/> class.
        /// </summary>
        public SelectedElementControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedElementControl"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public SelectedElementControl(REX.Common.REXExtension extension)
            : base(extension)
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes the dialog with current settings.
        /// </summary>
        public void SetDialog()
        {
            propertiesControl.ClearProperties();

            foreach (Main.PropertyItem item in ThisMainExtension.Data.SelectedElementProperties)
                propertiesControl.AddProperty(item);

            viewer.DrawGeometry(ThisMainExtension.Data.SelectedElementGeometry);
        }
    }
}
