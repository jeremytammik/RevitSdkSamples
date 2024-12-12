//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections;

namespace Revit.SDK.Samples.SlabShapeEditing.CS
{
    /// <summary>
    /// The entrance of this example, implements the Execute method of IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        #region IExternalCommand Members Implementation
        ///<summary>
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
            Floor selectFloor = GetSelectFloor(commandData);
            if (null == selectFloor)
            {
                message = "Make sure selected only one floor (Slab) in Revit.";
                return Autodesk.Revit.UI.Result.Failed;
            }

            SlabShapeEditingForm slabShapeEditingForm = 
                new SlabShapeEditingForm(selectFloor, commandData);
            slabShapeEditingForm.ShowDialog();

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// get selected floor (slab)
        /// </summary>
        /// <param name="commandData">object which contains reference of Revit Application.</param>
        /// <returns>selected floor (slab)</returns>
        private Floor GetSelectFloor(ExternalCommandData commandData)
        {
           ElementSet eleSet = new ElementSet();
            foreach (ElementId elementId in commandData.Application.ActiveUIDocument.Selection.GetElementIds())
            {
               eleSet.Insert(commandData.Application.ActiveUIDocument.Document.GetElement(elementId));
            }
            if (eleSet.Size != 1) { return null; }

            IEnumerator iter = eleSet.GetEnumerator();
            iter.Reset();
            while (iter.MoveNext())
            {
                return iter.Current as Floor;
            }
            return null;
        }
        #endregion
    }
}
