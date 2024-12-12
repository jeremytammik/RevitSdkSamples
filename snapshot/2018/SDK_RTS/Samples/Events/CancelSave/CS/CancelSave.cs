//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB.Events;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.RevitAddIns;

namespace Revit.SDK.Samples.CancelSave.CS
{
    /// <summary>
    /// This class is an external application which checks whether "Project Status" is updated 
    /// once the project is about to be saved. If updated pass the save else cancel the save and inform user with one message.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CancelSave : IExternalApplication
    {
        #region Memeber Variables
        
        const string thisAddinFileName = "CancelSave.addin";
        // The dictionary contains document hashcode and its original "Project Status" pair.
        Dictionary<int, string> documentOriginalStatusDic = new Dictionary<int, string>();
        int hashcodeofCurrentClosingDoc;

        #endregion

        #region IExternalApplication Members

        /// <summary>
        /// Implement OnStartup method of IExternalApplication interface.
        /// This method subscribes to DocumentOpened, DocumentCreated, DocumentSaving and DocumentSavingAs events.
        /// The first two events are used to reserve "Project Status" original value; 
        /// The last two events are used to check whether "Project Status" has been updated, and re-reserve current value as new original value for next compare.
        /// </summary>
        /// <param name="application">Controlled application to be loaded to Revit process.</param>
        /// <returns>The status of the external application</returns>
        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            // subscribe to DocumentOpened, DocumentCreated, DocumentSaving and DocumentSavingAs events
            application.ControlledApplication.DocumentOpened   += new EventHandler<DocumentOpenedEventArgs>(ReservePojectOriginalStatus);
            application.ControlledApplication.DocumentCreated  += new EventHandler<DocumentCreatedEventArgs>(ReservePojectOriginalStatus);
            application.ControlledApplication.DocumentSaving   += new EventHandler<DocumentSavingEventArgs>(CheckProjectStatusUpdate);
            application.ControlledApplication.DocumentSavingAs += new EventHandler<DocumentSavingAsEventArgs>(CheckProjectStatusUpdate);
            application.ControlledApplication.DocumentClosing += new EventHandler<DocumentClosingEventArgs>(MemClosingDocumentHashCode);
            application.ControlledApplication.DocumentClosed += new EventHandler<DocumentClosedEventArgs>(RemoveStatusofClosedDocument);

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// Implement OnShutdown method of IExternalApplication interface. 
        /// </summary>
        /// <param name="application">Controlled application to be shutdown.</param>
        /// <returns>The status of the external application.</returns>
        public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
        {
            // unsubscribe to DocumentOpened, DocumentCreated, DocumentSaving and DocumentSavingAs events
            application.ControlledApplication.DocumentOpened   -= new EventHandler<DocumentOpenedEventArgs>(ReservePojectOriginalStatus);
            application.ControlledApplication.DocumentCreated  -= new EventHandler<DocumentCreatedEventArgs>(ReservePojectOriginalStatus);
            application.ControlledApplication.DocumentSaving   -= new EventHandler<DocumentSavingEventArgs>(CheckProjectStatusUpdate);
            application.ControlledApplication.DocumentSavingAs -= new EventHandler<DocumentSavingAsEventArgs>(CheckProjectStatusUpdate);

            // finalize the log file.
            LogManager.LogFinalize();

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        #endregion

        #region EventHandler

        /// <summary>
        /// Event handler method for DocumentOpened and DocumentCreated events.
        /// This method will reserve "Project Status" value after document has been opened or created.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments that contains the event data.</param>
        private void ReservePojectOriginalStatus(Object sender, RevitAPIPostDocEventArgs args)
        {
            // The document associated with the event. Here means which document has been created or opened.
            Document doc = args.Document;

            // Project information is unavailable for Family document.
            if (doc.IsFamilyDocument)
            {
                return;
            }

            // write log file. 
            LogManager.WriteLog(args, doc);
            
            // get the hashCode of this document.
            int docHashCode = doc.GetHashCode();

            // retrieve the current value of "Project Status". 
            string currentProjectStatus = RetrieveProjectCurrentStatus(doc);
            // reserve "Project Status" current value in one dictionary, and use this project's hashCode as key.
            documentOriginalStatusDic.Add(docHashCode, currentProjectStatus);

            // write log file. 
            LogManager.WriteLog("   Current Project Status: " + currentProjectStatus);
        }

        /// <summary>
        /// Event handler method for DocumentSaving and DocumentSavingAs events.
        /// This method will check whether "Project Status" has been updated, and reserve current value as original value.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments that contains the event data.</param>
        private void CheckProjectStatusUpdate(Object sender, RevitAPIPreDocEventArgs args)
        {
            // The document associated with the event. Here means which document is about to save / save as.
            Document doc = args.Document;

            // Project information is unavailable for Family document.
            if (doc.IsFamilyDocument)
            {
                return;
            }

            // write log file.
            LogManager.WriteLog(args, doc);

            // retrieve the current value of "Project Status". 
            string currentProjectStatus = RetrieveProjectCurrentStatus(args.Document);

            // get the old value of "Project Status" for one dictionary.
            string originalProjectStatus = documentOriginalStatusDic[doc.GetHashCode()];

            // write log file.
            LogManager.WriteLog("   Current Project Status: " + currentProjectStatus + "; Original Project Status: " + originalProjectStatus);

            // project status has not been updated.
            if ((string.IsNullOrEmpty(currentProjectStatus) && string.IsNullOrEmpty(originalProjectStatus)) || 
                (0 == string.Compare(currentProjectStatus, originalProjectStatus,true)))
            {
                DealNotUpdate(args);
                return;
            }

            // update "Project Status" value reserved in the dictionary.
            documentOriginalStatusDic.Remove(doc.GetHashCode());
            documentOriginalStatusDic.Add(doc.GetHashCode(), currentProjectStatus);
        }

        private void MemClosingDocumentHashCode(Object sender, DocumentClosingEventArgs args)
        {
           hashcodeofCurrentClosingDoc = args.Document.GetHashCode();
        }

        private void RemoveStatusofClosedDocument(Object sender, DocumentClosedEventArgs args)
        {
           if (args.Status.Equals(RevitAPIEventStatus.Succeeded) && (documentOriginalStatusDic.ContainsKey(hashcodeofCurrentClosingDoc)))
           {
              documentOriginalStatusDic.Remove(hashcodeofCurrentClosingDoc);
           }
        }
        #endregion

        /// <summary>
        /// Deal with the case that the project status wasn't updated.
        /// If the event is Cancellable, cancel it and inform user else just inform user the status.
        /// </summary>
        /// <param name="args">Event arguments that contains the event data.</param>
        private static void DealNotUpdate(RevitAPIPreDocEventArgs args)
        {
            string mainMessage;
            string additionalText;
            TaskDialog taskDialog = new TaskDialog("CancelSave Sample");

            if (args.Cancellable)
            {
                args.Cancel(); // cancel this event if it is cancellable. 

                mainMessage = "CancelSave sample detected that the Project Status parameter on Project Info has not been updated. The file will not be saved."; // prompt to user.              
            }
            else
            {
                // will not cancel this event since it isn't cancellable. 
               mainMessage = "The file is about to save. But CancelSave sample detected that the Project Status parameter on Project Info has not been updated."; // prompt to user.              
            }

            // taskDialog will not show when do regression test.
            if (!LogManager.RegressionTestNow)
            {
               additionalText = "You can disable this permanently by uninstaling the CancelSave sample from Revit. Remove or rename CancelSave.addin from the addins directory.";

               // use one taskDialog to inform user current situation.     
               taskDialog.MainInstruction = mainMessage;
               taskDialog.MainContent     = additionalText;
               taskDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Open the addins directory");
               taskDialog.CommonButtons = TaskDialogCommonButtons.Close;
               taskDialog.DefaultButton = TaskDialogResult.Close;
               TaskDialogResult tResult = taskDialog.Show();
               if (TaskDialogResult.CommandLink1 == tResult)
               {
                  System.Diagnostics.Process.Start("explorer.exe", DetectAddinFileLocation(args.Document.Application));
               }
            }

            // write log file.
            LogManager.WriteLog("   Project Status is not updated, taskDialog informs user: " + mainMessage);
        }

        /// <summary>
        /// Retrieve current value of Project Status.
        /// </summary>
        /// <param name="doc">Document of which the Project Status will be retrieved.</param>
        /// <returns>Current value of Project Status.</returns>
        private static string RetrieveProjectCurrentStatus(Document doc)
        {
            // Project information is unavailable for Family document.
            if (doc.IsFamilyDocument)
            {
                return null;
            }

            // get project status stored in project information object and return it.
            return doc.ProjectInformation.Status;
        }

        private static string DetectAddinFileLocation(Autodesk.Revit.ApplicationServices.Application applictaion)
        {
           string addinFileFolderLocation = null;
           IList<RevitProduct> installedRevitList = RevitProductUtility.GetAllInstalledRevitProducts();

           foreach (RevitProduct revit in installedRevitList)
           {
              if (revit.Version.ToString().Contains(applictaion.VersionNumber))
              {
                 string allUsersAddInFolder = revit.AllUsersAddInFolder;
                 string currentUserAddInFolder = revit.CurrentUserAddInFolder;

                 if (File.Exists(Path.Combine(allUsersAddInFolder, thisAddinFileName)))
                 {
                    addinFileFolderLocation = allUsersAddInFolder;
                 }
                 else if (File.Exists(Path.Combine(currentUserAddInFolder, thisAddinFileName)))
                 {
                    addinFileFolderLocation = currentUserAddInFolder;
                 }

                 break;                
              }
           }

           return addinFileFolderLocation;
        }
    }
}
