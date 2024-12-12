//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.PanelSchedule.CS
{
    /// <summary>
    /// Translate the panel schedule view data from Revit to some formats, HTML, CSV etc.
    /// </summary>
    abstract class Translator
    {
        /// <summary>
        /// the panel schedule view instance to be exported.
        /// </summary>
        protected PanelScheduleView m_psView;

        public abstract string Export();

        /// <summary>
        /// An utility method to replace illegal characters of the Panel Schedule view name.
        /// </summary>
        /// <param name="stringWithIllegalChar">the Panel Schedule view name.</param>
        /// <returns>the updated string without illegal characters.</returns>
        protected string ReplaceIllegalCharacters(string stringWithIllegalChar)
        {
            char[] illegalChars = System.IO.Path.GetInvalidFileNameChars();

            string updated = stringWithIllegalChar;
            foreach (char ch in illegalChars)
            {
                updated = updated.Replace(ch, '_');
            }

            return updated;
        }

        /// <summary>
        /// An utility method to get the number of rows and columns of the section which is exporting.
        /// </summary>
        /// <param name="doc">Revit document.</param>
        /// <param name="psView">the exporting panel schedule view</param>
        /// <param name="sectionType">the exporting section of the panel schedule.</param>
        /// <param name="nRows">the number of rows.</param>
        /// <param name="nCols">the number of columns.</param>
        protected void getNumberOfRowsAndColumns(Autodesk.Revit.DB.Document doc, PanelScheduleView psView, SectionType sectionType, ref int nRows, ref int nCols)
        {
            Transaction openSectionData = new Transaction(doc, "openSectionData");
            openSectionData.Start();

            TableSectionData sectionData = psView.GetSectionData(sectionType);
            nRows = sectionData.NumberOfRows;
            nCols = sectionData.NumberOfColumns;

            openSectionData.RollBack();
        }
    }
}
