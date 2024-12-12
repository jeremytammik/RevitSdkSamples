//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Linq;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;


namespace Revit.SDK.Samples.SinePlotter.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
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
        public virtual Result Execute(ExternalCommandData commandData
            , ref string message, ElementSet elements)
        {
            try
            {
                Document document = commandData.Application.ActiveUIDocument.Document;

                //instantiate a finder to locate the FamilySymbol of the class instance we want to array
                FilteredElementCollector familySymbolFinder = new FilteredElementCollector(document);
                familySymbolFinder.OfClass(typeof(FamilySymbol));

                //the name of the family symbol we are looking for
                String fsName = Application.GetFamilySymbolName();
                FamilySymbol fs = null;
                try
                {
                    fs = familySymbolFinder.ToElements().Single(s => s.Name == fsName) as FamilySymbol;
                }
                catch (InvalidOperationException)
                {
                    TaskDialog.Show("FamilySymbol Loading Error", "The family symbol is not loaded in the project file.");
                    return Result.Failed;
                }

                //instantiate an instance plotter object
                FamilyInstancePlotter plotter = new FamilyInstancePlotter(fs, document);

                //reference the necessary inputs for the placeInstancesOnCurve method
                int partitions = (int)Application.GetNumberOfPartitions();
                double period = Application.GetPeriod();
                double amplitude = Application.GetAplitude();
                double numOfCircles = Application.GetNumberOfCycles();
                //place the instances of the family objects along a curve         
                plotter.PlaceInstancesOnCurve(partitions, period, amplitude, numOfCircles);

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}

