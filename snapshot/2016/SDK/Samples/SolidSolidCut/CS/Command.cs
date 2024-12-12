//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.IO;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.SolidSolidCut.CS
{
    /// <summary>
    /// Demonstrate how to use the SolidSolidCut API to make one solid cut another.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Cut : IExternalCommand
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
            // NOTES: Anything can be done in this method, such as the solid-solid cut operation.

            // Get the application and document from external command data.
            Application app = commandData.Application.Application;
            Document activeDoc = commandData.Application.ActiveUIDocument.Document;

            #region Demo how to use the SolidSolidCut API to make one solid cut another.

            int solidToBeCutElementId = 30481; //The cube
            int cuttingSolidElementId = 30809; //The sphere

            //Get element by ElementId
            Element solidToBeCut = activeDoc.GetElement(new ElementId(solidToBeCutElementId));
            Element cuttingSolid = activeDoc.GetElement(new ElementId(cuttingSolidElementId));

            //If the two elements do not exist, notify user to open the family file then try this command.
            if (solidToBeCut == null || cuttingSolid == null)
            {
                TaskDialog.Show("Notice", "Please open the family file SolidSolidCut.rfa, then try to run this command.");

                return Autodesk.Revit.UI.Result.Succeeded;
            }

            //Check whether the cuttingSolid can cut the solidToBeCut
            CutFailureReason cutFailureReason = new CutFailureReason();
            if (SolidSolidCutUtils.CanElementCutElement(cuttingSolid, solidToBeCut, out cutFailureReason))
            {
                //cuttingSolid can cut solidToBeCut

                //Do the solid-solid cut operation
                //Start a transaction
                Transaction transaction = new Transaction(activeDoc);
                transaction.Start("AddCutBetweenSolids(activeDoc, solidToBeCut, cuttingSolid)");

                //Let the cuttingSolid cut the solidToBeCut
                SolidSolidCutUtils.AddCutBetweenSolids(activeDoc, solidToBeCut, cuttingSolid);

                transaction.Commit();
            }

            #endregion

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion
    }

    /// <summary>
    /// Demonstrate how to use the SolidSolidCut API to uncut two solids which have the cutting relationship.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Uncut : IExternalCommand
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
            // NOTES: Anything can be done in this method, such as the solid-solid uncut operation.

            // Get the application and document from external command data.
            Application app = commandData.Application.Application;
            Document activeDoc = commandData.Application.ActiveUIDocument.Document;

            #region Demo how to use the SolidSolidCut API to uncut two solids which have the cutting relationship.

            int solidToBeCutElementId = 30481; //The cube
            int cuttingSolidElementId = 30809; //The sphere

            //Get element by ElementId
            Element solidToBeCut = activeDoc.GetElement(new ElementId(solidToBeCutElementId));
            Element cuttingSolid = activeDoc.GetElement(new ElementId(cuttingSolidElementId));

            //If the two elements do not exist, notify user to open the family file then try this command.
            if (solidToBeCut == null || cuttingSolid == null)
            {
                TaskDialog.Show("Notice", "Please open the family file SolidSolidCut.rfa, then try to run this command.");

                return Autodesk.Revit.UI.Result.Succeeded;
            }

            //Remove the solid-solid cut (Uncut)
            //Start a transaction
            Transaction transaction = new Transaction(activeDoc);
            transaction.Start("RemoveCutBetweenSolids(activeDoc, solidToBeCut, cuttingSolid)");

            //Remove the cutting relationship between solidToBeCut and cuttingSolid (Uncut)
            SolidSolidCutUtils.RemoveCutBetweenSolids(activeDoc, solidToBeCut, cuttingSolid);

            transaction.Commit();

            #endregion

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion
    }
}
