//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using Autodesk.Revit.Collections;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;

using CreApp = Autodesk.Revit.Creation.Application;
using Element = Autodesk.Revit.Element;
using GElement = Autodesk.Revit.Geometry.Element;

namespace Revit.SDK.Samples.FamilyExplorer.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
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
        public virtual IExternalCommand.Result Execute(ExternalCommandData commandData
            , ref string message, ElementSet elements)
        {
            try
            {
                if (null == commandData)
                {
                    throw new ArgumentNullException("commandData");
                }

                FamilyMgr rfaMgr = new FamilyMgr(commandData.Application.ActiveDocument
                    , commandData.Application.Create);

                using (FamilyExplorerForm dlg = new FamilyExplorerForm(rfaMgr))
                {
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return IExternalCommand.Result.Cancelled;
                    }
                }

                return IExternalCommand.Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return IExternalCommand.Result.Failed;
            }
        }

        #endregion IExternalCommand Members Implementation

    }
}
