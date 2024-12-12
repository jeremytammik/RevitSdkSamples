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

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.RoomSchedule
{
    #region Class Interface Implementation
    /// <summary>
    /// To add an external command to Autodesk Revit, 
    /// the developer must define a class which implements the IExternalCommand interface. 
    /// This class is used as the connection of Revit and external program
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
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
        public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData,
                                               ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Transaction tranSample = null;
            try
            {
                tranSample = new Transaction(commandData.Application.ActiveUIDocument.Document, "Sample Start");
                tranSample.Start();
                // create a form to display the information of Revit rooms and xls based rooms
                using (RoomScheduleForm infoForm = new RoomScheduleForm(commandData))
                {
                    infoForm.ShowDialog();
                }
                tranSample.Commit();
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                if (null != tranSample) tranSample.RollBack();
                // if there are something wrong, give error information and return failed
                message = ex.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
    }
    #endregion
}
