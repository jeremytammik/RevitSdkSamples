//
// (C) Copyright 2003-2015 by Autodesk, Inc. All rights reserved.
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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.DuplicateViews.CS
{
    /// <summary>
    /// A command to copy schedules and drafting views across documents.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class DuplicateAcrossDocumentsCommand : IExternalCommand
    {
        #region IExternalCommand Members

        /// <summary>
        /// The command implementation.
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application application = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Find target document - it must be the only other open document in session
            Document toDocument = null;
            IEnumerable<Document> documents = application.Documents.Cast<Document>();
            if (documents.Count<Document>() != 2)
            {
                TaskDialog.Show("No target document",
                                "This tool can only be used if there are two documents (a source document and target document).");
                return Result.Cancelled;
            }
            foreach (Document loadedDoc in documents)
            {
                if (loadedDoc.Title != doc.Title)
                {
                    toDocument = loadedDoc;
                    break;
                }
            }

            // Collect schedules and drafting views
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            List<Type> viewTypes = new List<Type>();
            viewTypes.Add(typeof(ViewSchedule));
            viewTypes.Add(typeof(ViewDrafting));
            ElementMulticlassFilter filter = new ElementMulticlassFilter(viewTypes);
            collector.WherePasses(filter);

            collector.WhereElementIsViewIndependent(); // skip view-specfic schedules (e.g. Revision Schedules);
            // These should not be copied as they are associated to another view that cannot be copied
   
            // Copy all schedules together so that any dependency elements are copied only once
            IEnumerable<ViewSchedule> schedules = collector.OfType<ViewSchedule>();
            DuplicateViewUtils.DuplicateSchedules(doc, schedules, toDocument);
            int numSchedules = schedules.Count<ViewSchedule>();
            
            // Copy drafting views together
            IEnumerable<ViewDrafting> draftingViews = collector.OfType<ViewDrafting>();
            int numDraftingElements = 
                DuplicateViewUtils.DuplicateDraftingViews(doc, draftingViews, toDocument);
            int numDrafting = draftingViews.Count<ViewDrafting>();

            // Show results
            TaskDialog.Show("Statistics",
                   String.Format("Copied: \n" + 
                                "\t{0} schedules.\n" +
                                "\t{1} drafting views.\n"+
                                "\t{2} new drafting elements created.",
                   numSchedules, numDrafting, numDraftingElements));

            return Result.Succeeded;
        }

        #endregion
    }
}
