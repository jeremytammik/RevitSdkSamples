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
using System.Text;
using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;


namespace Revit.SDK.Samples.Openings.CS
{
    /// <summary>
    /// The entrance of this example, implement the Execute method of IExternalCommand
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
            Transaction transaction = new Transaction(commandData.Application.ActiveUIDocument.Document, "External Tool");
            try
            {
                transaction.Start();
                Application app = commandData.Application.Application;
                bool haveOpening = false;

                //search Opening in Revit
                List<OpeningInfo> openingInfos = new List<OpeningInfo>();
                FilteredElementIterator iter = (new FilteredElementCollector(commandData.Application.ActiveUIDocument.Document)).OfClass(typeof(Opening)).GetElementIterator();
                iter.Reset();
                while (iter.MoveNext())
                {
                    Object obj = iter.Current;
                    if (obj is Opening)
                    {
                        haveOpening = true;
                        Opening opening = obj as Opening;
                        OpeningInfo openingInfo = new OpeningInfo(opening, commandData.Application);
                        openingInfos.Add(openingInfo);
                    }
                }

                if (!haveOpening)
                {
                    message = "don't have opening in the project";
                    return Autodesk.Revit.UI.Result.Cancelled;
                }

                //show dialogue
                using (OpeningForm openingForm = new OpeningForm(openingInfos))
                {
                    openingForm.ShowDialog();
                }
            }
            catch (Exception e)
            {
                message = e.ToString();
                return Autodesk.Revit.UI.Result.Failed;
            }
            finally
            {
                transaction.Commit();
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion
    }
}
