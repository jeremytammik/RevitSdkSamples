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

namespace Revit.SDK.Samples.ApplicationEvents.CS
{
    /// <summary>
    /// A class inherits IExternalApplication interface to create a custom menu "Track Revit Application Events" 
    /// of which the corresponding external command is the command in this project.
    /// </summary>
    public class TopLevelMenu : IExternalApplication
    {
        #region IExternalApplication Members

        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit starts before a file or default template is actually loaded.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully started. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If false is returned then Revit should inform the user that the external application 
        /// failed to load and the release the internal reference.</returns>
        public IExternalApplication.Result OnStartup(ControlledApplication application)
        {
            try
            {
                // create a top level menu named "CustomMenu_Janet";
                MenuItem topMenu = application.CreateTopMenu("MyCustomTopMenu");

                // create a basic sub-menu of this top level menu named "Track Revit Application Events".
                topMenu.Append(MenuItem.MenuType.BasicMenu, 
                               "Track Revit Application Events",
                                System.Reflection.Assembly.GetExecutingAssembly().Location, 
                                "Revit.SDK.Samples.ApplicationEvents.CS.Command");
            }
            catch (Exception)
            {
                return IExternalApplication.Result.Failed;
            }

            return IExternalApplication.Result.Succeeded;
        }

        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit is about to exit,Any documents must have been closed before this method is called.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully shutdown. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If false is returned then the Revit user should be warned of the failure of the external 
        /// application to shut down correctly.</returns>
        public IExternalApplication.Result OnShutdown(ControlledApplication application)
        {   
            return IExternalApplication.Result.Succeeded;
        }

        #endregion
    }
}
