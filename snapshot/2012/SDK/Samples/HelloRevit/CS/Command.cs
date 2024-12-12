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
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.HelloRevit.CS
{
    /// <summary>
    /// Demonstrate how a basic ExternalCommand can be added to the Revit user interface. 
    /// And demonstrate how to create a Revit style dialog.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        #region IExternalCommand Members

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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
            ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            // NOTES: Anything can be done in this method, such as create a message box, 
            // a task dialog or fetch some information from revit and so on.
            // We mainly use the task dialog for example.

            // Get the application and document from external command data.
            Application app = commandData.Application.Application;
            Document activeDoc = commandData.Application.ActiveUIDocument.Document;

            #region Task Dialog Sample
            // Study how to create a revit style dialog using task dialog API by following
            // code snippet.  

            // Creates a Revit task dialog to communicate information to the interactive user.
            TaskDialog mainDialog = new TaskDialog("Hello, Revit!");
            mainDialog.MainInstruction = "Hello, Revit!";
            mainDialog.MainContent =
                "This sample shows how a basic ExternalCommand can be added to the Revit user interface."
                + " It uses a Revit task dialog to communicate information to the interactive user.\n"
                + "The command links below open additional task dialogs with more information.";

            // Add commmandLink to task dialog
            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1,
                                      "View information about the Revit installation");
            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2,
                                      "View information about the active document");

            // Set common buttons and default button. If no CommonButton or CommandLink is added,
            // task dialog will show a Close button by default.
            mainDialog.CommonButtons = TaskDialogCommonButtons.Close;
            mainDialog.DefaultButton = TaskDialogResult.Close;

            // Set footer text. Footer text is usually used to link to the help document.
            mainDialog.FooterText =
                "<a href=\"http://usa.autodesk.com/adsk/servlet/index?siteID=123112&id=2484975 \">"
                + "Click here for the Revit API Developer Center</a>";

            TaskDialogResult tResult = mainDialog.Show();

            // If the user clicks the first command link, a simple Task Dialog 
            // with only a Close button shows information about the Revit installation. 
            if (TaskDialogResult.CommandLink1 == tResult)
            {
                TaskDialog dialog_CommandLink1 = new TaskDialog("Revit Build Information");
                dialog_CommandLink1.MainInstruction =
                    "Revit Version Name is: " + app.VersionName + "\n"
                    + "Revit Version Number is: " + app.VersionNumber + "\n"
                    + "Revit Version Build is: " + app.VersionBuild;

                dialog_CommandLink1.Show();

            }

            // If the user clicks the second command link, a simple Task Dialog 
            // created by static method shows information about the active document.
            else if (TaskDialogResult.CommandLink2 == tResult)
            {
                TaskDialog.Show("Active Document Information",
                    "Active document: " + activeDoc.Title + "\n"
                    + "Active view name: " + activeDoc.ActiveView.Name);
            }
            #endregion

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        #endregion
    }
}
