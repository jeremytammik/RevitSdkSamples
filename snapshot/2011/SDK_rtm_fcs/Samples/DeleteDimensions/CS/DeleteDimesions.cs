//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.DeleteDimensions.CS
{
    /// <summary>
    ///Add a command that given a selection deletes all the unpinned dimensions 
    /// that are found in that selection.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Automatic)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        /// <summary>
        /// Overload this method to implement the external command within Revit.
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
            ElementSet selections = commandData.Application.ActiveUIDocument.Selection.Elements;
            ElementSet dimsToDelete = new Autodesk.Revit.DB.ElementSet();

            //warning if nothing selected
            if (0 == selections.Size)
            {
                message = "Please select dimensions";
                return Autodesk.Revit.UI.Result.Failed;
            }

            //find all unpinned dimensions in the current selection 
            foreach (Element e in selections)
            {
               Dimension dimesionTemp = e as Dimension;
               if (null != dimesionTemp && !dimesionTemp.Pinned)
               {
                  dimsToDelete.Insert(dimesionTemp);
               }
            }

            //warning if could not find any unpinned dimension
            if (0 == dimsToDelete.Size)
            {
               message = "There are no unpinned dimensions currently selected";
               return Autodesk.Revit.UI.Result.Failed;
            }

            //delete all the unpinned dimensions
            commandData.Application.ActiveUIDocument.Document.Delete(dimsToDelete);
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}