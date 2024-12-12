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
using System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.MoveLinear.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : Autodesk.Revit.UI.IExternalCommand
    {
        #region IExternalCommand Members Implementation
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="cmdData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="msg">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="eleSet">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet eleSet)
        {
            Autodesk.Revit.UI.Result res = Autodesk.Revit.UI.Result.Succeeded;
            try
            {
                System.Collections.IEnumerator iter;
                Autodesk.Revit.UI.Selection.Selection sel;
                sel = cmdData.Application.ActiveUIDocument.Selection;

                ElementSet elemSet;
                elemSet = new ElementSet();
                foreach (ElementId elementId in sel.GetElementIds())
                {
                   elemSet.Insert(cmdData.Application.ActiveUIDocument.Document.GetElement(elementId));
                }

                //Check whether user has selected only one element
                if (0 == elemSet.Size)
                {
                    TaskDialog.Show("MoveLinear", "Please select an element");
                    return res;
                }

                if (1 < elemSet.Size)
                {
                    TaskDialog.Show("MoveLinear", "Please select only one element");
                    return res;
                }

                iter = elemSet.ForwardIterator();

                iter.MoveNext();
                Autodesk.Revit.DB.Element element;

                element = (Autodesk.Revit.DB.Element)iter.Current;

                if (element != null)
                {
                    Autodesk.Revit.DB.LocationCurve lineLoc;
                    lineLoc = element.Location as LocationCurve;

                    if (null == lineLoc)
                    {
                        TaskDialog.Show("MoveLinear", "Please select an element which based on a Line");
                        return res;
                    }

                    Autodesk.Revit.DB.Line line;
                    //get start point via "get_EndPoint(0)"
                    Autodesk.Revit.DB.XYZ newStart = new XYZ(
                        lineLoc.Curve.GetEndPoint(0).X + 100,
                        lineLoc.Curve.GetEndPoint(0).Y,
                        lineLoc.Curve.GetEndPoint(0).Z);
                    //get end point via "get_EndPoint(1)"
                    Autodesk.Revit.DB.XYZ newEnd = new XYZ(
                        lineLoc.Curve.GetEndPoint(1).X,
                        lineLoc.Curve.GetEndPoint(1).Y + 100,
                        lineLoc.Curve.GetEndPoint(1).Z);


                    //get a new line and use it to move current element 
                    //with property "Autodesk.Revit.DB.LocationCurve.Curve"
                    line = Line.CreateBound(newStart, newEnd);
                    lineLoc.Curve = line;
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("MoveLinear", ex.Message);
                res = Autodesk.Revit.UI.Result.Failed;
            }
            finally
            {
            }
            return res;
        }
        #endregion IExternalCommand Members Implementation
    }
}
