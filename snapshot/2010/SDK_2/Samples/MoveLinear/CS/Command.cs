//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.MoveLinear.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    public class Command : Autodesk.Revit.IExternalCommand
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
        public Autodesk.Revit.IExternalCommand.Result Execute(ExternalCommandData cmdData, ref string msg, Autodesk.Revit.ElementSet eleSet)
        {
            IExternalCommand.Result res = IExternalCommand.Result.Succeeded;
            try
            {
                System.Collections.IEnumerator iter;
                Autodesk.Revit.Selection sel;
                sel = cmdData.Application.ActiveDocument.Selection;
           
                Autodesk.Revit.ElementSet elemSet;
                elemSet = sel.Elements;

                //Check whether user has selected only one element
                if (0 == elemSet.Size)
                {
                    MessageBox.Show("Please select an element", "MoveLinear");
                    return res;
                }

                if (1 < elemSet.Size)
                {
                    MessageBox.Show("Please select only one element", "MoveLinear");
                    return res;
                }

                iter = elemSet.ForwardIterator();
           
                iter.MoveNext();
                Autodesk.Revit.Element element;
           
                element = (Autodesk.Revit.Element)iter.Current;
           
                if( element != null )
                {
                    Autodesk.Revit.LocationCurve lineLoc;
                    lineLoc = element.Location as LocationCurve;

                    if (null == lineLoc)
                    {
                        MessageBox.Show("Please select an element which based on a Line", "MoveLinear");
                        return res;
                    }
                 
                    Autodesk.Revit.Geometry.Line line;
                    Autodesk.Revit.Geometry.XYZ newStart;
                    Autodesk.Revit.Geometry.XYZ newEnd;

                    //get start point via "get_EndPoint(0)"
                    newStart = lineLoc.Curve.get_EndPoint(0);
                    //get end point via "get_EndPoint(1)"
                    newEnd = lineLoc.Curve.get_EndPoint(1);
              
                    newStart.X = newStart.X + 100;
                    newEnd.Y = newEnd.Y + 100;

                    //get a new line and use it to move current element 
                    //with property "Autodesk.Revit.LocationCurve.Curve"
                    line = cmdData.Application.Create.NewLineBound(newStart, newEnd);
                    lineLoc.Curve = line;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MoveLinear");
                res = IExternalCommand.Result.Failed;
            }
            finally
            {
            }
            return res;
        }
        #endregion IExternalCommand Members Implementation
    }
}
