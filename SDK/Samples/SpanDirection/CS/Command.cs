//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.SpanDirection.CS
{
    /// <summary>
    /// Get Span direction of Floor and all the SpanDirection Symbols
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        public Document m_docment;
        #region Interface implementation
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
        public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Autodesk.Revit.UI.UIApplication application = commandData.Application;
            m_docment = application.ActiveUIDocument.Document;
            try
            {
                // user should select one slab firstly. 
                if (application.ActiveUIDocument.Selection.GetElementIds().Count == 0)
                {
                    TaskDialog.Show("Revit", "Please select one slab firstly.", TaskDialogCommonButtons.Ok);
                    return Autodesk.Revit.UI.Result.Cancelled;
                }

                // get the selected slab and show its span direction
                ElementSet elementSet = new ElementSet();
                foreach (ElementId elementId in application.ActiveUIDocument.Selection.GetElementIds())
                {
                   elementSet.Insert(application.ActiveUIDocument.Document.GetElement(elementId));
                }
                ElementSetIterator elemIter = elementSet.ForwardIterator();
                elemIter.Reset();
                while (elemIter.MoveNext())
                {
                    Floor floor = elemIter.Current as Floor;
                    if (floor != null)
                    {
                        GetSpanDirectionAndSymobls(floor);
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return Autodesk.Revit.UI.Result.Failed;
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion


        /// <summary>
        /// Get SpanDirection and SpanDirectionSymobols of Floor
        /// </summary>
        /// <param name="floor"></param>
        void GetSpanDirectionAndSymobls(Floor floor)
        {
            if (null != floor)
            {
                // get SpanDirection angle of Floor(Slab)
                // The angle returned is in radians. An exception will be thrown if the floor
                // is non structural.
                String spanDirAngle = "Span direction angle: " + floor.SpanDirectionAngle.ToString() + "\r\n";

                // get span direction symbols of Floor(Slab)
                String symbols = "Span direction symbols: \r\n\t";
                System.Collections.Generic.ICollection<ElementId> symbolArray = floor.GetSpanDirectionSymbolIds();
                //ElementArrayIterator symbolIter = symbolArray.ForwardIterator();
                //symbolIter.Reset();
                //while (symbolIter.MoveNext())
                foreach (ElementId eid in symbolArray)
                {
                    Element elem = m_docment.GetElement(eid);
                    if (elem != null)
                    {
                        symbols += (m_docment.GetElement(elem.GetTypeId()) as ElementType).Name + "\r\n";
                    }
                }

                TaskDialog.Show("Revit Direction", spanDirAngle + symbols, TaskDialogCommonButtons.Ok);
            }
            else
            {
                new Exception("Get Floor and SpanDirectionAngle and Symbols failed!");
            }
        }
    }
}
