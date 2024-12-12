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


namespace Revit.SDK.Samples.CreateBeamSystem.CS
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Windows.Forms;
	using System.Diagnostics;

	using Autodesk.Revit;

	/// <summary>
	/// external applications' only entry point class that supports the IExternalCommand interface
	/// </summary>
	public class Command : IExternalCommand
	{
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
		public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			try
			{
                GeometryUtil.CreApp = commandData.Application.Create;

				// initialize precondition data of the program
				BeamSystemData data = new BeamSystemData(commandData);
				// display form to collect user's setting for beam system
                using (BeamSystemForm form = new BeamSystemForm(data))
                {
                    if (form.ShowDialog() != DialogResult.OK)
                    {
                        return IExternalCommand.Result.Cancelled;
                    }
                }

				// create beam system using the parameters saved in BeamSystemData
				BeamSystemBuilder builder = new BeamSystemBuilder(data);
				builder.CreateBeamSystem();
			}
			catch (ErrorMessageException errorEx)
			{
				// checked exception need to show in error messagebox
				message = errorEx.Message;
				return IExternalCommand.Result.Failed;
			}
			catch(Exception ex)
			{
				// unchecked exception cause command failed
				message = "Command is failed for unexpected reason.";
				Trace.WriteLine(ex.ToString());
				return IExternalCommand.Result.Failed;
			}
			return IExternalCommand.Result.Succeeded;
		}
	}
}