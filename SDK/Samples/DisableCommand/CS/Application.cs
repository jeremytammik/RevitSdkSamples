//
// (C) Copyright 2003-2019 by Autodesk, Inc. All rights reserved.
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
using System.Text;
using System.Windows.Forms;
using System.IO;

using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.DisableCommand.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication
    /// </summary>
    public class Application : IExternalApplication
    {
        #region IExternalApplication Members
       
        /// <summary>
        /// Implements the OnStartup event
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnStartup(UIControlledApplication application)
        {
            // Lookup the desired command by name
            s_commandId = RevitCommandId.LookupCommandId(s_commandToDisable);

            // Confirm that the command can be overridden
            if (!s_commandId.CanHaveBinding)
            {
                ShowDialog("Error", "The target command " + s_commandToDisable + 
                            " selected for disabling cannot be overridden");
				return Result.Failed;
            }

            // Create a binding to override the command.
            // Note that you could also implement .CanExecute to override the accessibiliy of the command.
            // Doing so would allow the command to be grayed out permanently or selectively, however, 
            // no feedback would be available to the user about why the command is grayed out.
            try
            {
                AddInCommandBinding commandBinding = application.CreateAddInCommandBinding(s_commandId);
                commandBinding.Executed += DisableEvent;
            }
            // Most likely, this is because someone else has bound this command already.
            catch (Exception)
            {
                ShowDialog("Error", "This add-in is unable to disable the target command " + s_commandToDisable +
                            "; most likely another add-in has overridden this command.");
            }

            return Result.Succeeded;
        }

        /// <summary>
        /// Implements the OnShutdown event
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            // Remove the command binding on shutdown
            if (s_commandId.HasBinding)
                application.RemoveAddInCommandBinding(s_commandId);
            return Result.Succeeded;
        }

        #endregion

        /// <summary>
        /// A command execution method which disables any command it is applied to (with a user-visible message).
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="args">Arguments.</param>
        private void DisableEvent(object sender, ExecutedEventArgs args)
        {
            ShowDialog("Disabled", "Use of this command has been disabled.");
        }

        /// <summary>
        /// Show a task dialog with a message and title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        private static void ShowDialog(string title, string message)
        {
            // Show the user a message.
            TaskDialog td = new TaskDialog(title)
            {
                MainInstruction = message,
                TitleAutoPrefix = false
            };
            td.Show();
        }

        /// <summary>
        /// The string name of the command to disable.  To lookup a command id string, open a session of Revit, 
        /// invoke the desired command, close Revit, then look to the journal from that session.  The command
        /// id string will be toward the end of the journal, look for the "Jrn.Command" entry that was recorded
        /// when it was selected.
        /// </summary>
        static String s_commandToDisable = "ID_EDIT_DESIGNOPTIONS";

        /// <summary>
        /// The command id, stored statically to allow for removal of the command binding.
        /// </summary>
        static RevitCommandId s_commandId;

    }
}
