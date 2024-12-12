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
using REX.ElementReportHTML.Main;
using Autodesk.REX.Framework;

namespace REX.ElementReportHTML.Resources.Dialogs
{
    /// <summary>
    /// Represents the control with a browser for a note.
    /// </summary>
    public partial class ReportControl : REX.Common.REXExtensionControl
    {
        IXHTMLDocument HtmlDocument;
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
        /// Get the Data structure.
        /// </summary>
        /// <value>The main Data.</value>
        internal Data ThisMainData
        {
            get
            {
                return ThisMainExtension.Data;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportControl"/> class.
        /// </summary>
        public ReportControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportControl"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public ReportControl(REX.Common.REXExtension extension)
            : base(extension)
        {
            InitializeComponent();            
        }
        /// <summary>
        /// Initializes the dialog with current settings.
        /// </summary>
        public void SetDialog()
        {
            FillNote();
        }
        /// <summary>
        /// Fills the note.
        /// </summary>
        public void FillNote()
        {
            if (HtmlDocument != null)
            {
                HtmlDocument = null;
                ThisExtension.ROHTMLReleaseDocument();
            }

            HtmlDocument = ThisExtension.ROHTMLDocument();            

            FillHeaderNote();

            if (ThisMainExtension.Data.IdMode)
            {
                if (ThisMainExtension.Data.SelectedElement != null)
                    AddElementToNote(ThisMainExtension.Data.SelectedElement);
            }
            else
            {
                FillGeneralInformation();

                foreach (Node categoryNode in ThisMainExtension.Data.MainNode.Nodes.Values)
                {
                    if (!ThisMainExtension.Data.SelectedCategories.Contains(categoryNode))
                        continue;

                    AddCategoryToNote(categoryNode);

                    foreach (Node elementNode in categoryNode.Nodes.Values)
                    {
                        AddElementToNote(elementNode);
                    }
                }
            }

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

            HtmlDocument.SaveAsSingleFile(DocumentPath);
            webBrowser.Navigate(new Uri(DocumentPath));            
        }
        /// <summary>
        /// Adds the category to the note.
        /// </summary>
        /// <param name="categoryNode">The category node.</param>
        private void AddCategoryToNote(Node categoryNode)
        {
            HtmlDocument.Body.AddHeader(2, "Category: " + categoryNode.Name);
            HtmlDocument.Body.AddValue2("Number of elements", categoryNode.Nodes.Count.ToString());
        }
        /// <summary>
        /// Adds the element to the note.
        /// </summary>
        /// <param name="elementNode">The element node.</param>
        private void AddElementToNote(Node elementNode)
        {
            HtmlDocument.Body.AddHeader(3, "Element: " + elementNode.Name);
            HtmlDocument.Body.AddValue2("Id", elementNode.Id);

            if (elementNode.Nodes.Count > 0)
            {
                HtmlDocument.Body.AddHeader(4, "Parameters");

                XHTMLTable table = HtmlDocument.Body.AddTable();
                table.HeaderColumns = 0;
                table.HeaderRows = 1;
                table.AutoFormat(ROHTMLTableFormats.ROTF_REPORT_1);

                //header
                table.AddRow();
                table.get_Cell(1, 1).AddText("Parameter");
                table.get_Cell(1, 2).AddText("Value");

                int row = 1;
                foreach (Node parameterNode in elementNode.Nodes.Values)
                {
                    table.AddRow();
                    row++;

                    table.get_Cell(row, 1).AddText(parameterNode.Name);
                    table.get_Cell(row, 2).AddText(parameterNode.Value);
                }
            }
        }
        /// <summary>
        /// Fills the header note.
        /// </summary>
        private void FillHeaderNote()
        {
            Autodesk.REX.Framework.REXEnvironment Env = new Autodesk.REX.Framework.REXEnvironment(REX.Common.REXConst.ENG_DedicatedVersionName);
            string path = Env.GetModulePath(Autodesk.REX.Framework.REXEnvironment.PathType.Configuration, ThisExtension.ThisApplication.Context.Name);
            string imagePath = REX.Common.REXController.GetProductBitmapFullPath(ThisContext, ThisExtension.GetREXEnvironment(), (REXInterfaceType)ThisExtension.ExtensionContext.Control.StandardOwner, false);
            string ModVersion = ThisExtension.ThisApplication.GetType().Assembly.GetName().Version.Major.ToString() + "." + ThisExtension.ThisApplication.GetType().Assembly.GetName().Version.Minor.ToString();

            string createHeader = "<table border = 1 style = \"border:#eeeeee\" width =\"100%\">" +
                                       "<tr height=\"40\">" +
                                           "<td rowspan = \"4\" align=\"center\" valign=\"center\" width = \"10%\" style = \"border: 1px solid #eeeeee\"><img  src=\"" + imagePath + "\"" + "</td>" +
                                           "<td align=\"left\" valign=\"center\" width = \"90%\" bgcolor = \"#eeeeee\" style = \"border: 0px solid #eeeeee\" >" +
                                                "<font size = 4 face=\"Arial\">Element Report</font></br>" + "</td>" +
                                       "</tr>" +                                   
                                       "<tr height=\"20\">" +
                                            "<td align=\"left\" valign=\"center\" width = \"90%\" bgcolor = \"#eeeeee\" style = \"border: 0px solid #eeeeee\" >" +
                                                "<font size = 1 color = \"gray\" >REXSDK 2012</font></br>" + "</td>" +
                                       "</tr>" +
                                   "</table>";
            HtmlDocument.Body.AddLine(createHeader);

            HtmlDocument.Body.AddLineBreak();
        }
        /// <summary>
        /// Fills general information.
        /// </summary>
        private void FillGeneralInformation()
        {
            HtmlDocument.Body.AddHeader(2, "General information");

            HtmlDocument.Body.AddText2("Number of categories", ThisMainExtension.Data.MainNode.Nodes.Count.ToString());
            
            int countAllElems = 0;
            int countSelCats = 0;
            int countSelElems = 0;

            foreach (Node cat in ThisMainExtension.Data.MainNode.Nodes.Values)
            {
                countAllElems += cat.Nodes.Count;

                if (ThisMainExtension.Data.SelectedCategories.Contains(cat))
                {
                    countSelCats++;
                    countSelElems += cat.Nodes.Count;
                }
            }

            HtmlDocument.Body.AddText2("Number of all elements", countAllElems.ToString());
            HtmlDocument.Body.AddText2("Number of selected categories", countSelCats.ToString());
            HtmlDocument.Body.AddText2("Number of selected elements", countSelElems.ToString());
            HtmlDocument.Body.AddLineBreak();
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

            path = path + "\\ElementReportHTML";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;

        }
        /// <summary>
        /// Handles the Click event of the ToolStripButtonSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ToolStripButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if(HtmlDocument == null)
                return;

            Microsoft.Win32.SaveFileDialog SaveFileDialog = new Microsoft.Win32.SaveFileDialog();

            SaveFileDialog.Filter = "MHT (*.mht) file|*.mht|HTML (*.html) file|*.html;";
            SaveFileDialog.OverwritePrompt = true;

            if (SaveFileDialog.ShowDialog(ThisExtension.GetWindowForParent()) == true)
            {
                if (System.IO.Path.GetExtension(SaveFileDialog.FileName).ToLower() == ".mht")
                    HtmlDocument.SaveAsSingleFile(SaveFileDialog.FileName);
                else
                    HtmlDocument.Save(SaveFileDialog.FileName);
            }
        }
    }
}
