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
using ROHTMLLib;
using System.IO;

namespace REX.PyramidGenerator.Resources.Dialogs
{
    /// <summary>
    /// Interaction logic for SubControl.xaml
    /// </summary>
    public partial class PyramidNoteControl : REX.Common.REXExtensionControl
    {
        //Step 3.2.: Preparing layouts
        /// <summary>
        /// The ROHTML document object
        /// </summary>
        IXHTMLDocument HtmlDocument;
        /// <summary>
        /// The path of the current document
        /// </summary>
        string DocumentPath;

        /// <summary>
        /// Get the main extension.
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
        /// Initializes a new instance of the <see cref="PyramidNoteControl"/> class.
        /// </summary>
        public PyramidNoteControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PyramidNoteControl"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public PyramidNoteControl(REX.Common.REXExtension extension)
            : base(extension)
        {
            InitializeComponent();
        }
        /// <summary>
        /// Fills the note according current settings
        /// </summary>
        public void SetDialog()
        {
            if (HtmlDocument != null)
            {
                HtmlDocument = null;
                ThisExtension.ROHTMLReleaseDocument();
            }
            HtmlDocument = ThisExtension.ROHTMLDocument();

            if (File.Exists(DocumentPath))
            {
                try
                {
                    File.Delete(DocumentPath);
                }
                catch
                {
                }
            }

            Guid g = System.Guid.NewGuid();
            DocumentPath = GetTempPath() + "\\" + g.ToString() + ".mht";

            HtmlDocument.Body.AddHeader(1, "Pyramid parameters");
            string textH = ThisExtension.Units.DisplayTextFromBase(ThisMainExtension.Data.H, Autodesk.REX.Framework.EUnitType.Dimensions_StructureDim, true);
            HtmlDocument.Body.AddValue2("H", textH);
            string textB = ThisExtension.Units.DisplayTextFromBase(ThisMainExtension.Data.B, Autodesk.REX.Framework.EUnitType.Dimensions_StructureDim, true);
            HtmlDocument.Body.AddValue2("B", textB);

            HtmlDocument.SaveAsSingleFile(DocumentPath);
            webBrowser.Navigate(new Uri(DocumentPath));     

        }
        /// <summary>
        /// Returns the path to temporary Windows folder.
        /// </summary>
        /// <returns>The path.</returns>
        public string GetTempPath()
        {
            string path = Environment.GetEnvironmentVariable("temp");
            if (!Directory.Exists(path))
            {
                path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..\\Temp"));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            path = path + "\\PyramidGenerator";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;

        }
        /// <summary>
        /// Clears resources before closing the module
        /// </summary>
        public void Release()
        {
            if (File.Exists(DocumentPath))
            {
                try
                {
                    File.Delete(DocumentPath);
                }
                catch
                {
                }
            }

            string tempPath = GetTempPath();

            if (Directory.Exists(tempPath))
            {
                try
                {
                    Directory.Delete(tempPath);
                }
                catch
                {
                }
            }
        }
    }
}
