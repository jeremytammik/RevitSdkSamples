//
// (C) Copyright 2003-2014 by Autodesk, Inc. All rights reserved.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

namespace Revit.SDK.Samples.PostCommandWorkflow.CS
{
    /// <summary>
    /// The external command to set up the revision monitor and execute tasks related to it.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class PostCommandRevisionMonitorCommand : IExternalCommand
    {
        #region IExternalCommand Members

        /// <summary>
        /// The external command callback.
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            if (monitor == null)
            {
                monitor = new PostCommandRevisionMonitor(doc);
                monitor.Activate();
                commandButton.ItemText = "Remove Revision Monitor";
            }
            else
            { 
                monitor.Deactivate();
                monitor = null;
                commandButton.ItemText = "Setup Revision Monitor";
            }

            return Result.Succeeded;
        }

        #endregion

        /// <summary>
        /// Sets the handle to the command's pushbutton.
        /// </summary>
        /// <param name="pushButton"></param>
        public static void SetPushButton(PushButton pushButton)
        {
            commandButton = pushButton;
        }

        /// <summary>
        /// The monitor.
        /// </summary>
        private static PostCommandRevisionMonitor monitor = null;

        /// <summary>
        /// The handle to the command's PushButton.
        /// </summary>
        private static PushButton commandButton;
    }
       
}
