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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.MultiplanarRebar.CS
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        #region Implement IExternalCommand
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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
            ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            // A List to store the Corbels which are suitable to be reinforced.
            List<CorbelFrame> corbelsToReinforce = new List<CorbelFrame>();            

            // Filter out the Corbels which can be reinforced by this sample
            // from the selected elements.
            ElementSet elems = new ElementSet();
            foreach (ElementId elementId in commandData.Application.ActiveUIDocument.Selection.GetElementIds())
            {
               elems.Insert(commandData.Application.ActiveUIDocument.Document.GetElement(elementId));
            }
            foreach (Element elem in elems)
            {
                FamilyInstance corbel = elem as FamilyInstance;

                // Make sure it's a Corbel firstly.
                if (corbel != null && IsCorbel(corbel))
                {
                    try
                    {
                        // If the Corbel is sloped, this should return a non-null object.
                        CorbelFrame frame = CorbelFrame.Parse(corbel);
                        corbelsToReinforce.Add(frame);
                    }
                    // If the Corbel is not sloped, it will throw exception.
                    catch (System.Exception ex)
                    {
                        // Collect the error message, in case there is no any suitable corbel to be reinforced,
                        // Let user know what's happened.
                        message += ex.ToString();
                    }
                }
            }

            // Check to see if there is any Corbel to be reinforced.            
            if (corbelsToReinforce.Count == 0)
            {
                // If there is no suitable Corbel to be reinforced, prompt a message.
                if (string.IsNullOrEmpty(message))
                    message += "Please select sloped corbels.";

                // Return cancelled for invalid selection.
                return Result.Cancelled;
            }

            // Show a model dialog to get Rebar creation options.
            Document revitDoc = commandData.Application.ActiveUIDocument.Document;
            CorbelReinforcementOptions reinforcementOptions = new CorbelReinforcementOptions(revitDoc);
            using (CorbelReinforcementOptionsForm reinforcementOptionsForm = 
                new CorbelReinforcementOptionsForm(reinforcementOptions))
            {
                if (reinforcementOptionsForm.ShowDialog() == DialogResult.Cancel)
                {
                    // Cancelled by user.
                    return Result.Cancelled;
                }
            }


            // Encapsulate operation "Reinforce Corbels" into one transaction. 
            Transaction reinforceTransaction = new Transaction(revitDoc);
            try
            {
                // Start the transaction.
                reinforceTransaction.Start("Reinforce Corbels");

                // Reinforce all the corbels in list.
                foreach (CorbelFrame corbel in corbelsToReinforce)
                {
                    // Reinforce the sloped Corbel.
                    corbel.Reinforce(reinforcementOptions);
                }

                // Submit the transaction
                reinforceTransaction.Commit();
            }
            catch (System.Exception ex)
            {
                // Rollback the transaction for any exception.
                reinforceTransaction.RollBack();
                message += ex.ToString();

                // Return failed for any exception.
                return Result.Failed;
            }

            // No any error, return succeeded.
            return Result.Succeeded;
        }

        /// <summary>
        /// Test to see if the given family instance is a Corbel.
        /// </summary>
        /// <param name="corbel">Given Family instance</param>
        /// <returns>True if the given family instance is Corbel, otherwise, false.</returns>
        bool IsCorbel(FamilyInstance corbel)
        {
            // Families of category "Structural Connection" support the Structural Material Type parameter. 
            // Structural Connection families of type Concrete or Precast Concrete are considered corbels. 
            // Corbels support the following features: 
            // •Hosting Rebar.
            // •Autojoining to columns and walls.
            // •Manual joining to other concrete elements.
            return (corbel.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructConnections &&
                    (corbel.StructuralMaterialType == StructuralMaterialType.Concrete ||
                    corbel.StructuralMaterialType == StructuralMaterialType.PrecastConcrete));
        }

        #endregion
    }
}
