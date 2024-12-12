//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.Reflection;
using System.IO;
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI.Selection;
using Application = Autodesk.Revit.ApplicationServices.Application;
using Element = Autodesk.Revit.DB.Element;

namespace Revit.SDK.Samples.TraverseSystem.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Automatic)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
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
        public virtual Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData
            , ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            try
            {
                // Verify if the active document is null
                UIDocument activeDoc = commandData.Application.ActiveUIDocument;
                if (activeDoc == null)
                {
                    MessageBox.Show("There's no active document in Revit.", "No Active Document",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return Autodesk.Revit.UI.Result.Failed;
                }

                // Verify the number of selected elements
                SelElementSet selElements = activeDoc.Selection.Elements;
                if (selElements.Size != 1)
                {
                    message = "Please select ONLY one element from current project.";
                    return Autodesk.Revit.UI.Result.Failed;
                }

                // Get the selected element
                Element selectedElement = null;
                foreach (Element element in selElements)
                {
                    selectedElement = element;
                    break;
                }

                // Get the expected mechanical or piping system from selected element
                // Some elements in a non-well-connected system may get lost when traversing 
                //the system in the direction of flow; and
                // flow direction of elements in a non-well-connected system may not be right, 
                // therefore the sample will only support well-connected system.
                MEPSystem system = ExtractMechanicalOrPipingSystem(selectedElement);
                if (system == null)
                {
                    message = "The selected element does not belong to any well-connected mechanical or piping system. " +
                        "The sample will not support well-connected systems for the following reasons: " +
                        Environment.NewLine + 
                        "- Some elements in a non-well-connected system may get lost when traversing the system in the " +
                        "direction of flow" + Environment.NewLine +
                        "- Flow direction of elements in a non-well-connected system may not be right";
                    return Autodesk.Revit.UI.Result.Failed;
                }

                // Traverse the system and dump the traversal into an XML file
                TraversalTree tree = new TraversalTree(activeDoc.Document, system);
                tree.Traverse();
                String fileName;
                fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "traversal.xml");
                tree.DumpIntoXML(fileName);

                System.Diagnostics.Process.Start( fileName ); // jeremy

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }

        /// <summary>
        /// Get the mechanical or piping system from selected element
        /// </summary>
        /// <param name="selectedElement">Selected element</param>
        /// <returns>The extracted mechanical or piping system. Null if no expected system is found.</returns>
        private MEPSystem ExtractMechanicalOrPipingSystem(Element selectedElement)
        {
            MEPSystem system = null;

            if (selectedElement is MEPSystem)
            {
                if (selectedElement is MechanicalSystem || selectedElement is PipingSystem)
                {
                    system = selectedElement as MEPSystem;
                    return system;
                }
            }
            else // Selected element is not a system
            {
                FamilyInstance fi = selectedElement as FamilyInstance;
                //
                // If selected element is a family instance, iterate its connectors and get the expected system
                if (fi != null) 
                {
                    MEPModel mepModel = fi.MEPModel;
                    ConnectorSet connectors = null;
                    try
                    {
                        connectors = mepModel.ConnectorManager.Connectors;
                    }
                    catch (System.Exception)
                    {
                        system = null;
                    }

                    system = ExtractSystemFromConnectors(connectors);
                }
                else 
                {
                    //
                    // If selected element is a MEPCurve (e.g. pipe or duct), 
                    // iterate its connectors and get the expected system
                    MEPCurve mepCurve = selectedElement as MEPCurve;
                    if (mepCurve != null)
                    {
                        ConnectorSet connectors = null;
                        connectors = mepCurve.ConnectorManager.Connectors;
                        system = ExtractSystemFromConnectors(connectors);
                    }
                }
            }

            return system;
        }

        /// <summary>
        /// Get the mechanical or piping system from the connectors of selected element
        /// </summary>
        /// <param name="connectors">Connectors of selected element</param>
        /// <returns>The found mechanical or piping system</returns>
        static private MEPSystem ExtractSystemFromConnectors(ConnectorSet connectors)
        {
            MEPSystem system = null;

            if (connectors == null || connectors.Size == 0)
            {
                return null;
            }

            // Get well-connected mechanical or piping systems from each connector
            List<MEPSystem> systems = new List<MEPSystem>();
            foreach (Connector connector in connectors)
            {
                MEPSystem tmpSystem = connector.MEPSystem;
                if (tmpSystem == null)
                {
                    continue;
                }

                MechanicalSystem ms = tmpSystem as MechanicalSystem;
                if (ms != null)
                {
                    if (ms.IsWellConnected)
                    {
                        systems.Add(tmpSystem);
                    }
                }
                else
                {
                    PipingSystem ps = tmpSystem as PipingSystem;
                    if (ps != null && ps.IsWellConnected)
                    {
                        systems.Add(tmpSystem);
                    }
                }
            }

            // If more than one system is found, get the system contains the most elements
            int countOfSystem = systems.Count;
            if (countOfSystem != 0)
            {
                int countOfElements = 0;
                foreach (MEPSystem sys in systems)
                {
                    if (sys.Elements.Size > countOfElements)
                    {
                        system = sys;
                        countOfElements = sys.Elements.Size;
                    }
                }
            }

            return system;
        }
    }
}


