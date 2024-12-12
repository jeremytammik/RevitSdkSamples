//
// (C) Copyright 2003-2016 by Autodesk, Inc. All rights reserved.
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
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI.Events;

namespace Revit.SDK.Samples.PostCommandWorkflow.CS
{
    /// <summary>
    /// This class has the capabilities to monitor when a document is saved, and prompt the user to 
    /// add a revision before save takes place.
    /// </summary>
    class PostCommandRevisionMonitor
    {
        /// <summary>
        /// Constructs a new revision monitor for the given document.
        /// </summary>
        /// <param name="doc">The document.</param>
        public PostCommandRevisionMonitor(Document doc)
        {
            document = doc;
        }

        /// <summary>
        /// Activates the revision monitor for the stored document.
        /// </summary>
        public void Activate()
        {
            // Save the number of revisions as an initial count.
            storedRevisionCount = GetRevisionCount(document);

            // Setup event for saving.
            document.DocumentSaving += OnSavingPromptForRevisions;
        }

        /// <summary>
        /// Deactivates the revision monitor for the stored document.
        /// </summary>
        public void Deactivate()
        {
            // Remove the event for saving.
            document.DocumentSaving -= OnSavingPromptForRevisions;
        }

        #region RevitAPI event callbacks

        /// <summary>
        /// The DocumentSaving callback.  This callback checks if at least one new revision has been added, and if not
        /// shows instructions to the user to deal with the situation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnSavingPromptForRevisions(object sender, DocumentSavingEventArgs args)
        {
            Document doc = (Document)sender;
            UIApplication uiApp = new UIDocument(doc).Application;

            if (doc.IsModified)
            {
                // Compare number of revisions with saved count
                int revisionCount = GetRevisionCount(doc);
                if (revisionCount <= storedRevisionCount)
                {
                    // Show dialog with explanation and options
                    TaskDialog td = new TaskDialog("Revisions not created.");
                    td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                    td.MainInstruction = "Changes have been made to this document, but no new revision has been created.";
                    td.ExpandedContent = "Because the document has been released, it is typically required to issue a new " +
                                    "revision number with any change.";
                    td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Add revision now");
                    td.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Cancel save");
                    td.AddCommandLink(TaskDialogCommandLinkId.CommandLink3, "Proceed with save (not recommended).");
                    td.TitleAutoPrefix = false;
                    td.AllowCancellation = false;
                    TaskDialogResult result = td.Show();

                    switch (result)
                    {
                        case TaskDialogResult.CommandLink1:  // Add revision now
                            {
                                // cancel first save
                                args.Cancel();

                                // add event to hide the default "Document not saved" dialog
                                uiApp.DialogBoxShowing += HideDocumentNotSaved;

                                // post command for editing revisions
                                PromptToEditRevisionsAndResave(uiApp);
                                break;
                            }
                        case TaskDialogResult.CommandLink2:  // Cancel save
                            {
                                // cancel saving only
                                args.Cancel();
                                break;
                            }
                        case TaskDialogResult.CommandLink3:  // Proceed with save
                            {
                                // do nothing
                                break;
                            }
                    }
                }
                else
                {
                    storedRevisionCount = revisionCount;
                }
            }
        }

        /// <summary>
        /// The DialogBoxShowing callback to cancel display of the "Document not saved" message when saving is cancelled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void HideDocumentNotSaved(object sender, DialogBoxShowingEventArgs args)
        {
            TaskDialogShowingEventArgs tdArgs = args as TaskDialogShowingEventArgs;
            // The "Document not saved" dialog does not have a usable id, so we are forced to look at the text instead. 
            if (tdArgs != null && tdArgs.Message.Contains("not saved"))
                args.OverrideResult(0x0008);
        }

        /// <summary>
        /// The BeforeExecuted callback for the command, to setup the post action after the revision command is run.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ReactToRevisionsAndSchedulesCommand(object sender, BeforeExecutedEventArgs args)
        {
            if (externalEvent != null)
                externalEvent.Raise();
        }

        /// <summary>
        /// The external event raised to finalize the workflow after the user has been prompted to enter the 
        /// Sheet Revisions/Issues command.
        /// </summary>
        class PostCommandRevisionMonitorEvent : IExternalEventHandler
        {
            #region IExternalEventHandler Members

            /// <summary>
            /// The external event callback to finalize the workflow.
            /// </summary>
            /// <param name="app"></param>
            public void Execute(UIApplication app)
            {
                monitor.CleanupAfterRevisionEdit(app);
            }

            /// <summary>
            /// The external event name.
            /// </summary>
            /// <returns></returns>
            public string GetName()
            {
                return "PostCommandRevisionMonitorEvent";
            }

            /// <summary>
            /// The constructor for the event instance.
            /// </summary>
            /// <param name="monitor">The instance of the command.</param>
            public PostCommandRevisionMonitorEvent(PostCommandRevisionMonitor monitor)
            {
                this.monitor = monitor;
            }

            /// <summary>
            /// Handle to the revision monitor.
            /// </summary>
            private PostCommandRevisionMonitor monitor;

            #endregion
        }

        #endregion

        /// <summary>
        /// Prompts to edit the revision and resave.
        /// </summary>
        /// <param name="application"></param>
        private void PromptToEditRevisionsAndResave(UIApplication application)
        {
            // Setup external event to be notified when activity is done
            externalEvent = ExternalEvent.Create(new PostCommandRevisionMonitorEvent(this));

            // Setup event to be notified when revisions command starts (this is a good place to raise this external event)
            RevitCommandId id = RevitCommandId.LookupPostableCommandId(PostableCommand.SheetIssuesOrRevisions);
            if (binding == null)
            {
                binding = application.CreateAddInCommandBinding(id);
            }
            binding.BeforeExecuted += ReactToRevisionsAndSchedulesCommand;

            // Post the revision editing command
            application.PostCommand(id);
        }

        /// <summary>
        /// The function that is called to finish the execution of the workflow.  Cleans up subscribed events,
        /// and posts the save command.
        /// </summary>
        /// <param name="uiApp"></param>
        private void CleanupAfterRevisionEdit(UIApplication uiApp)
        {
            // Remove dialog box showing
            uiApp.DialogBoxShowing -= HideDocumentNotSaved;

            if (binding != null)
                binding.BeforeExecuted -= ReactToRevisionsAndSchedulesCommand;
            externalEvent = null;

            // Repost the save command
            uiApp.PostCommand(RevitCommandId.LookupPostableCommandId(PostableCommand.Save));
        }


        /// <summary>
        /// Storage to remember the number of revisions when last checked.
        /// </summary>
        private int storedRevisionCount = 0;

        /// <summary>
        /// The handle to the external event instance to be invoked after the revision editing completes.
        /// </summary>
        private Autodesk.Revit.UI.ExternalEvent externalEvent = null;

        /// <summary>
        /// The binding to the revision command.
        /// </summary>
        private AddInCommandBinding binding = null;

        /// <summary>
        /// The document.
        /// </summary>
        private Document document;

        /// <summary>
        /// Counts the number of revision elements in the document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns>The number of elements found.</returns>
        private static int GetRevisionCount(Document doc)
        {
            // Find revision objects
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_Revisions);
            return collector.ToElementIds().Count;
        }
    }
}
