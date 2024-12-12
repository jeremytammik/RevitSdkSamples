//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation;
using Autodesk.Revit.UI;
using Document = Autodesk.Revit.DB.Document;

namespace ExtensibleStorageDocumentation
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        public static readonly string DefaultLogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData,ref string message, ElementSet elements)
        {
            Document docRevit = commandData.Application.ActiveUIDocument.Document;
            // Report main object creation creation 
            var docReport = new Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Document();

            // Content table creation
            var documentContentTable = new DocumentContentTable(docReport);
            docReport.Main.Elements.Add(documentContentTable);
            
            // Fill the using API 
            var reportApi = new ReportApi(docRevit, docReport);
            reportApi.AddSection("Documentation built using API", 1);
            reportApi.CreateDocumentApi();
            reportApi.AddSection("Documentation built using Schemas", 1);

            // Fill the using Schema defined in DocumentationSchema class  
            var reportSchemas = new ReportSchemas(docRevit, docReport);
            reportSchemas.CreateReportSchema();

            // Build the Content table 
            documentContentTable.Build(DetailLevel.Detail);
            
            //  A Builder object could be a mht, html or text builder based on the output needed  
            var builderMht = new MhtBuilder();

            // Method to build and save the report on a specific location on disk 
            builderMht.BuildDocument(docReport, docRevit, DefaultLogFilePath + "\\Documentation", DetailLevel.Detail);
            
            // Call Internet Explorer to  open the report  
            Process.Start("IEXPLORE.EXE", DefaultLogFilePath + "\\Documentation.mht");
            return Result.Succeeded;
        }

        #endregion
    }
}