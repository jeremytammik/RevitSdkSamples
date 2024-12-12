//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Diagnostics;


namespace Revit.SDK.Samples.RoutingPreferenceTools.CS
{
    /// <summary>
    /// A command to read a routing preference builder xml file and add pipe types, schedules, segments, and sizes, and routing preferences rules to the
    /// document from the xml data.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandReadPreferences : Autodesk.Revit.UI.IExternalCommand
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
        public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            if (!Validation.ValidateMep(commandData.Application.Application))
            {
                Validation.MepWarning();
                return Autodesk.Revit.UI.Result.Succeeded;
            }

            if (!Validation.ValidatePipesDefined(commandData.Application.ActiveUIDocument.Document))
            {
                Validation.PipesDefinedWarning();
                return Autodesk.Revit.UI.Result.Succeeded;
            }

            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".xml";
            ofd.Filter = "RoutingPreference Builder Xml files (*.xml)|*.xml";

            if (ofd.ShowDialog() == true)
            {
                StreamReader reader = new StreamReader(ofd.FileName);
                XDocument routingPreferenceBuilderDoc = XDocument.Load(new XmlTextReader(reader));
                reader.Close();

                //Distribute the .xsd file to routing preference builder xml authors as necessary.
                string xmlValidationMessage;
                if (!SchemaValidationHelper.ValidateRoutingPreferenceBuilderXml(routingPreferenceBuilderDoc, out xmlValidationMessage))
                {
                    Autodesk.Revit.UI.TaskDialog.Show("RoutingPreferenceBuilder", "Xml file is not a valid RoutingPreferenceBuilder xml document.  Please check RoutingPreferenceBuilderData.xsd.  " + xmlValidationMessage);
                    return Autodesk.Revit.UI.Result.Succeeded;
                }
                try
                {
                    RoutingPreferenceBuilder builder = new RoutingPreferenceBuilder(commandData.Application.ActiveUIDocument.Document);
                    builder.ParseAllPipingPoliciesFromXml(routingPreferenceBuilderDoc);
                    Autodesk.Revit.UI.TaskDialog.Show("RoutingPreferenceBuilder", "Routing Preferences imported successfully.");
                }
                catch (RoutingPreferenceDataException ex)
                {
                    Autodesk.Revit.UI.TaskDialog.Show("RoutingPreferenceBuilder error: ", ex.ToString());
                }
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}
