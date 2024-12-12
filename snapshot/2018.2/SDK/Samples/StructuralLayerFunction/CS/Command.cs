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
using System.Collections;
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;


namespace Revit.SDK.Samples.StructuralLayerFunction.CS
{
    /// <summary>
    /// With the selected floor, display the function of each of its structural layers
    /// in order from outside to inside in a dialog box
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        #region Private data members
        Autodesk.Revit.DB.Floor m_slab = null;    //Store the selected floor
        ArrayList m_functions;                            //Store the function of each floor
        #endregion


        #region class public property
        /// <summary>
        /// With the selected floor, export the function of each of its structural layers
        /// </summary>
        public ArrayList Functions
        {
            get
            {
                return m_functions;
            }
        }
        #endregion


        #region class public method
        /// <summary>
        /// Default constructor of StructuralLayerFunction
        /// </summary>
        public Command()
        {
            //Construct the data members for the property
            m_functions = new ArrayList();
        }
        #endregion


        #region Interface implemetation
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
        public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData,
                                                ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Autodesk.Revit.UI.UIApplication revit = commandData.Application;

            // Get the selected floor
            UIDocument project = revit.ActiveUIDocument;
            Selection choices = project.Selection;
            ElementSet collection = new ElementSet();
            foreach (ElementId elementId in choices.GetElementIds())
            {
               collection.Insert(project.Document.GetElement(elementId));
            }

            // Only allow to select one floor, or else report the failure
            if (1 != collection.Size)
            {
                message = "Please select a floor.";
                return Autodesk.Revit.UI.Result.Failed;
            }
            foreach (Element e in collection)
            {
                m_slab = e as Autodesk.Revit.DB.Floor;
                if (null == m_slab)
                {
                    message = "Please select a floor.";
                    return Autodesk.Revit.UI.Result.Failed;
                }
            }

            // Get the function of each of its structural layers
            foreach (CompoundStructureLayer e in m_slab.FloorType.GetCompoundStructure().GetLayers())
            {
                // With the selected floor, judge if the function of each of its structural layers
                // is exist, if it's not exist, there should be zero.
                if (0 == e.Function)
                {
                    m_functions.Add("No function");
                }
                else
                {
                    m_functions.Add(e.Function.ToString());
                }

            }

            // Display them in a form
            StructuralLayerFunctionForm displayForm = new StructuralLayerFunctionForm(this);
            displayForm.ShowDialog();

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion
    }
}
