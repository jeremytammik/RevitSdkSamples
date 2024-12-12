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
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.TagBeam.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
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
        public Autodesk.Revit.UI.Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            try
            {
                //prepare data
                TagBeamData dataBuffer = new TagBeamData(commandData);

                // show UI
                using (TagBeamForm displayForm = new TagBeamForm(dataBuffer))
                {
                    DialogResult result = displayForm.ShowDialog();
                    if (DialogResult.OK != result)
                    {
                        return Autodesk.Revit.UI.Result.Cancelled;
                    }
                }

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
        #endregion IExternalCommand Members Implementation
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class TagRebar : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                // Get the active document and view
                UIDocument revitDoc = commandData.Application.ActiveUIDocument;
                Autodesk.Revit.DB.View view = revitDoc.Document.ActiveView;
                foreach (ElementId elemId in revitDoc.Selection.GetElementIds())
                {
                   Element elem = revitDoc.Document.GetElement(elemId);
                    if (elem.GetType() == typeof(Autodesk.Revit.DB.Structure.Rebar))
                    {
                        // cast to Rebar and get its first curve
                        Autodesk.Revit.DB.Structure.Rebar rebar = (Autodesk.Revit.DB.Structure.Rebar)elem;
                        Autodesk.Revit.DB.Curve curve = rebar.GetCenterlineCurves(false, false, false)[0];

                        // create a rebar tag at the first end point of the first curve
                        using( Transaction t = new Transaction(revitDoc.Document))
                        {
                           t.Start("Create new tag");
                           IndependentTag tag = revitDoc.Document.Create.NewTag(view, rebar, true,
                               Autodesk.Revit.DB.TagMode.TM_ADDBY_CATEGORY,
                               Autodesk.Revit.DB.TagOrientation.Horizontal, curve.GetEndPoint(0));
                           t.Commit();
                        }
                        return Autodesk.Revit.UI.Result.Succeeded;
                    }
                }
                message = "No rebar selected!";
                return Autodesk.Revit.UI.Result.Failed;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CreateText : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                // get the active document and view
                UIDocument revitDoc = commandData.Application.ActiveUIDocument;
                Autodesk.Revit.DB.View view = revitDoc.ActiveView;
                Document dbDoc = revitDoc.Document;
                ElementId currentTextTypeId = dbDoc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

                foreach (ElementId elemId in revitDoc.Selection.GetElementIds())
                {
                   Element elem = dbDoc.GetElement(elemId);
                    if (elem.GetType() == typeof(Autodesk.Revit.DB.Structure.Rebar))
                    {
                        // cast to Rebar and get its first curve
                        Autodesk.Revit.DB.Structure.Rebar rebar = (Autodesk.Revit.DB.Structure.Rebar)elem;
                        Autodesk.Revit.DB.Curve curve = rebar.GetCenterlineCurves(false, false, false)[0];

                        // calculate necessary arguments
                        Autodesk.Revit.DB.XYZ origin = new XYZ(
                            curve.GetEndPoint(0).X + curve.Length,
                            curve.GetEndPoint(0).Y,
                            curve.GetEndPoint(0).Z);
                        string strText = "This is " + rebar.Category.Name + " : " + rebar.Name;

                        // create the text
                        using( Transaction t = new Transaction(dbDoc))
                        {
                           t.Start("New text note");
                           TextNote.Create(dbDoc, view.Id, origin, strText, currentTextTypeId);
                           t.Commit();
                        }
                        return Autodesk.Revit.UI.Result.Succeeded;
                    }
                }
                message = "No rebar selected!";
                return Autodesk.Revit.UI.Result.Failed;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
    }
}
