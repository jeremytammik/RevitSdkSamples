//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Collections;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural.Enums;

namespace Revit.SDK.Samples.DeleteObject.CS
{
	/// <summary>
	/// Delete the elements that were selected
	/// </summary>
    public class Command : Autodesk.Revit.IExternalCommand
	{
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
		public Autodesk.Revit.IExternalCommand.Result Execute(
            Autodesk.Revit.ExternalCommandData commandData, 
            ref string message, ElementSet elements)
		{
         Autodesk.Revit.Application revit = commandData.Application;

			ElementSet collection = revit.ActiveDocument.Selection.Elements;
			// check user selection
			if (collection.Size < 1) 
			{
				message = "Please select object(s) before delete.";
				return IExternalCommand.Result.Cancelled;
			}

			bool error = true;
			try
			{
				error = true;
				
				// delete selection
				IEnumerator e = collection.GetEnumerator();
				bool MoreValue = e.MoveNext();
				while (MoreValue)
				{
					Element component = e.Current as Element;
					revit.ActiveDocument.Delete(component);
					MoreValue = e.MoveNext();
				}
				
				error = false;
			}
			catch
			{
				// if revit threw an exception, try to catch it
				foreach (Element c in collection)
				{
					elements.Insert(c);
				}
                message = "object(s) can't be deleted.";
				return IExternalCommand.Result.Failed;
			}
			finally
			{
				// if revit threw an exception, display error and return failed
				if (error)
				{
					MessageBox.Show("Delete failed.");
				}
			}
						
			return IExternalCommand.Result.Succeeded;
		}
	}
}
