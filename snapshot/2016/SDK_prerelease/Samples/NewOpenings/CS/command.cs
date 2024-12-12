//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
using System.Collections;
using System.Drawing;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.NewOpenings.CS
{
    /// <summary>
    /// The entrance of this example, implement the Execute method of IExternalCommand
    /// Show how to create Opening in Revit by RevitAPI
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command:IExternalCommand
    {
        #region IExternalCommand Members Implementation
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
            Transaction transaction = new Transaction(commandData.Application.ActiveUIDocument.Document, "External Tool");
            try
            {
                transaction.Start();
                Wall wall = null;
                Floor floor = null;

                ElementSet elems = new ElementSet();
                foreach (ElementId elementId in commandData.Application.ActiveUIDocument.Selection.GetElementIds())
                {
                   elems.Insert(commandData.Application.ActiveUIDocument.Document.GetElement(elementId));
                }
                #region selection error handle
                //if user have some wrong selection, give user an error message
                if (1 != elems.Size)
                {
                    message = "please selected one Object (Floor or Wall) to create Opening.";
                    return Autodesk.Revit.UI.Result.Cancelled;
                }

                Autodesk.Revit.DB.Element selectElem = null;
                foreach (Autodesk.Revit.DB.Element e in elems)
                {
                    selectElem = e;
                }

                if (!(selectElem is Wall) && !(selectElem is Floor))
                {
                    message = "please selected one Object (Floor or Wall) to create Opening.";
                    return Autodesk.Revit.UI.Result.Cancelled;
                }
                #endregion
                try
                {
                    if (selectElem is Wall)
                    {
                        wall = selectElem as Wall;
                        ProfileWall profileWall = new ProfileWall(wall, commandData);
                        NewOpeningsForm newOpeningsForm = new NewOpeningsForm(profileWall);
                        newOpeningsForm.ShowDialog();
                    }
                    else if (selectElem is Floor)
                    {
                        floor = selectElem as Floor;
                        ProfileFloor profileFloor = new ProfileFloor(floor, commandData);
                        NewOpeningsForm newOpeningsForm = new NewOpeningsForm(profileFloor);
                        newOpeningsForm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return Autodesk.Revit.UI.Result.Cancelled;
                }

                return Autodesk.Revit.UI.Result.Succeeded; 
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
            finally
            {
                transaction.Commit();
            }
        }
        #endregion
    }
}
