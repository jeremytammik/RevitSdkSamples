//
// (C) Copyright 2003-2020 by Autodesk, Inc.
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
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
   /// <summary>
   /// Data class which stores the main information for exporting pdf format
   /// </summary>
   public class ExportPDFData : ExportDataWithViews
   {
      #region Class Member Variables
      private bool m_combine;
      #endregion

      #region Class Properties
      public bool Combine
      {
         get
         {
            return m_combine;
         }
         set
         {
            m_combine = value;
         }
      }

      #endregion

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="commandData">Revit command data</param>
      /// <param name="exportFormat">Format to export</param>
      public ExportPDFData(ExternalCommandData commandData, ExportFormat exportFormat)
          : base(commandData, exportFormat)
      {
         m_filter = "PDF Files |*.pdf";
         m_title = "Export PDF";

         m_combine = true;
      }

      /// <summary>
      /// Export PDF format
      /// </summary>
      /// <returns></returns>
      public override bool Export()
      {
         base.Export();

         bool exported = false;
         // Parameter: The list of view/sheet id to export
         IList<ElementId> views = new List<ElementId>();
         if (m_currentViewOnly)
         {
            views.Add(m_activeDoc.ActiveView.Id);
         }
         else
         {
            ViewSet viewSet = m_selectViewsData.SelectedViews;
            foreach (View v in viewSet)
            {
               views.Add(v.Id);
            }
         }

         // Parameter: The exporting options, including paper size, orientation, file name or naming rule and etc.
         PDFExportOptions options = new PDFExportOptions();
         options.FileName = m_exportFileName;
         options.Combine = m_combine;  // If not combined, PDFs will be exported with default naming rule "Type-ViewName"
         exported = m_activeDoc.Export(m_exportFolder, views, options);

         return exported;
      }
   }
}