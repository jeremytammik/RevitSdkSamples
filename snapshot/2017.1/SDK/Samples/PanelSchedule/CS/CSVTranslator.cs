//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.IO;
using System.Text;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;

namespace Revit.SDK.Samples.PanelSchedule.CS
{
    /// <summary>
    /// Translate the panel schedule view data from Revit to CSV.
    /// </summary>
    class CSVTranslator : Translator
    {
        /// <summary>
        /// create a CSVTranslator instance for a PanelScheduleView instance.
        /// </summary>
        /// <param name="psView">the exporting panel schedule view instance.</param>
        public CSVTranslator(PanelScheduleView psView)
        {
            m_psView = psView;
        }

        /// <summary>
        /// export to a CSV file that contains the PanelScheduleView instance data.
        /// </summary>
        /// <returns>the exported file path</returns>
        public override string Export()
        {
            string asemblyName = System.Reflection.Assembly.GetExecutingAssembly().Location;

            string panelScheduleCSVFile = asemblyName.Replace("PanelSchedule.dll", ReplaceIllegalCharacters(m_psView.Name) + ".csv");

            if (File.Exists(panelScheduleCSVFile))
            {
                File.Delete(panelScheduleCSVFile);
            }

            using (StreamWriter sw = File.CreateText(panelScheduleCSVFile))
            {
                //sw.WriteLine("This is my file.");
                DumpPanelScheduleData(sw);
                sw.Close();
            }

            return panelScheduleCSVFile;
        }

        /// <summary>
        /// dump PanelScheduleData to comma delimited.
        /// </summary>
        /// <param name="sw"></param>
        private void DumpPanelScheduleData(StreamWriter sw)
        {
            DumpSectionData(sw, m_psView, SectionType.Header);
            DumpSectionData(sw, m_psView, SectionType.Body);
            DumpSectionData(sw, m_psView, SectionType.Summary);
            DumpSectionData(sw, m_psView, SectionType.Footer);
        }

        /// <summary>
        /// dump SectionData to comma delimited.
        /// </summary>
        /// <param name="sw">exporting file stream</param>
        /// <param name="psView">the PanelScheduleView instance is exporting.</param>
        /// <param name="sectionType">which section is exporting, it can be Header, Body, Summary or Footer.</param>
        private void DumpSectionData(StreamWriter sw, PanelScheduleView psView, SectionType sectionType)
        {
            int nRows_Section = 0;
            int nCols_Section = 0;
            getNumberOfRowsAndColumns(m_psView.Document, m_psView, sectionType, ref nRows_Section, ref nCols_Section);

            for (int ii = 0; ii < nRows_Section; ++ii)
            {
                StringBuilder oneRow = new StringBuilder();
                for (int jj = 0; jj < nCols_Section; ++jj)
                {
                    try
                    {
                        oneRow.AppendFormat("{0},", m_psView.GetCellText(sectionType, ii, jj));
                    }
                    catch (Exception)
                    {
                        // do nothing.
                    }
                }

                sw.WriteLine(oneRow.ToString());
            }
        }
    }
}
