//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.Text;
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.VersionChecking.CS
{   
    /// <summary>
    /// Get the product name, version and build number about Revit main program.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command: IExternalCommand
    {
        string m_productName = ""; // product name of Revit main program
        string m_version     = ""; // version number of Revit main program
        string m_buildNumber = ""; // build number of Revit main program

        // properties
        /// <summary>
        /// get product name of Revit main program
        /// </summary>
        public string ProductName
        {
            get
            {
                return m_productName;
            }
        }

        /// <summary>
        /// get version number of current Revit main program
        /// </summary>
        public string ProductVersion
        {
            get
            {
                return m_version;
            }
        }

        /// <summary>
        /// get build number of current Revit main program
        /// </summary>
        public string BuildNumner
        {
            get
            {
                return m_buildNumber;
            }
        }

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
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
        public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            // get currently executable application
            Application revitApplication = revit.Application.Application;

            // get product name, version number and build number information
            // via corresponding Properties of Autodesk.Revit.ApplicationServices.Application class
            m_productName = revitApplication.VersionName;
            m_version     = revitApplication.VersionNumber;
            m_buildNumber = revitApplication.VersionBuild;

            //Show forms dialog which is a UI
            using (versionCheckingForm displayForm = new versionCheckingForm(this))
            {
                displayForm.ShowDialog();
            }
            
            return Autodesk.Revit.UI.Result.Succeeded;
        }

    }
}
