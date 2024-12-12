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
using System.Xml;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.PanelSchedule.CS
{
    /// <summary>
    /// Translate the panel schedule view data from Revit to HTML table.
    /// </summary>
    class HTMLTranslator : Translator
    {   
        /// <summary>
        /// create a Translator instance for a PanelScheduleView instance.
        /// </summary>
        /// <param name="psView">the exporting panel schedule view instance.</param>
        public HTMLTranslator(PanelScheduleView psView)
        {
            m_psView = psView;
        }

        /// <summary>
        /// export to a HTML page that contains the PanelScheduleView instance data.
        /// </summary>
        /// <returns>the exported file path</returns>
        public override string Export()
        {
            string asemblyName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string tempFile = asemblyName.Replace("PanelSchedule.dll", "template.html");

            if (!System.IO.File.Exists(tempFile))
            {
                TaskDialog messageDlg = new TaskDialog("Warnning Message");
                messageDlg.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                messageDlg.MainContent = "Can not find 'template.html', please make sure the 'template.html' file is in the same folder as the external command assembly.";
                messageDlg.Show();
                return null;
            }
            

            string panelScheduleFile = asemblyName.Replace("PanelSchedule.dll", ReplaceIllegalCharacters(m_psView.Name) + ".html");

            XmlDocument doc = new XmlDocument();
            XmlTextWriter tw = new XmlTextWriter(panelScheduleFile, null);
            doc.Load(tempFile);

            XmlNode psTable = doc.DocumentElement.SelectSingleNode("//div/table[1]");
            DumpPanelScheduleData(psTable, doc);
            
            doc.Save(tw);

            return panelScheduleFile;
        }

        /// <summary>
        /// dump PanelScheduleData to a 'table' node in HTML.
        /// </summary>
        /// <param name="panelScheduleDataNode">a 'table' node in HTML.</param>
        /// <param name="doc"></param>
        private void DumpPanelScheduleData(XmlNode panelScheduleDataNode, XmlDocument doc)
        {
            DumpSectionData(panelScheduleDataNode, doc, m_psView, SectionType.Header);
            DumpSectionData(panelScheduleDataNode, doc, m_psView, SectionType.Body);
            DumpSectionData(panelScheduleDataNode, doc, m_psView, SectionType.Summary);
            DumpSectionData(panelScheduleDataNode, doc, m_psView, SectionType.Footer);
        }

        /// <summary>
        /// dump SectionData to the 'tr' nodes in HTML. 
        /// </summary>
        /// <param name="panelScheduleDataNode">a 'table' node in HTML.</param>
        /// <param name="doc">HTML page</param>
        /// <param name="psView">the PanelScheduleView instance is exporting.</param>
        /// <param name="sectionType">which section is exporting, it can be Header, Body, Summary or Footer.</param>
        private void DumpSectionData(XmlNode panelScheduleDataNode, XmlDocument doc, PanelScheduleView psView, SectionType sectionType)
        {
            int nRows_Section = 0;
            int nCols_Section = 0;
            getNumberOfRowsAndColumns(m_psView.Document, m_psView, sectionType, ref nRows_Section, ref nCols_Section);

            for (int ii = 0; ii < nRows_Section; ++ii)
            {
                // add a <tr> node for each row
                XmlElement trNode = doc.CreateElement("tr");
                panelScheduleDataNode.AppendChild(trNode);

                for (int jj = 0; jj < nCols_Section; ++jj)
                {
                    // add <td> node for each cell
                    XmlElement tdNode = doc.CreateElement("td");

                    try
                    {
                        tdNode.InnerText = m_psView.GetCellText(sectionType, ii, jj);
                    }
                    catch (Exception)
                    {
                        // do nothing.
                    }

                    trNode.AppendChild(tdNode);
                }

            }
        }

    }
}
