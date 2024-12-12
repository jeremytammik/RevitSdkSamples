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
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.PanelSchedule.CS
{
    /// <summary>
    /// Export Panel Schedule View form Revit to CSV or HTML file.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class PanelScheduleExport : IExternalCommand
    {
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public virtual Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData
            , ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Autodesk.Revit.DB.Document doc = commandData.Application.ActiveUIDocument.Document;

            // get all PanelScheduleView instances in the Revit document.
            FilteredElementCollector fec = new FilteredElementCollector(doc);
            ElementClassFilter PanelScheduleViewsAreWanted = new ElementClassFilter(typeof(PanelScheduleView));
            fec.WherePasses(PanelScheduleViewsAreWanted);
            List<Element> psViews = fec.ToElements() as List<Element>;

            bool noPanelScheduleInstance = true;

            foreach (Element element in psViews)
            {
                PanelScheduleView psView = element as PanelScheduleView;
                if (psView.IsPanelScheduleTemplate())
                {
                    // ignore the PanelScheduleView instance which is a template.
                    continue;
                }
                else
                {
                    noPanelScheduleInstance = false;
                }

                // choose what format export to, it can be CSV or HTML.
                TaskDialog alternativeDlg = new TaskDialog("Choose Format to export");
                alternativeDlg.MainContent = "Click OK to export in .CSV format, Cancel to export in HTML format.";
                alternativeDlg.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;
                alternativeDlg.AllowCancellation = true;
                TaskDialogResult exportToCSV = alternativeDlg.Show();
                
                Translator translator = TaskDialogResult.Cancel == exportToCSV ? new HTMLTranslator(psView) : new CSVTranslator(psView) as Translator;
                string exported = translator.Export();

                // open the file if export successfully.
                if (!string.IsNullOrEmpty(exported))
                {
                    System.Diagnostics.Process.Start(exported);
                }
            }

            if (noPanelScheduleInstance)
            {
                TaskDialog messageDlg = new TaskDialog("Warnning Message");
                messageDlg.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                messageDlg.MainContent = "No panel schedule view is in the current document.";
                messageDlg.Show();
                return Result.Cancelled;
            }

            return Result.Succeeded;
        }

    }
}
