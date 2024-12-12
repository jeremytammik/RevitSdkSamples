//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;


namespace Revit.SDK.Samples.InPlaceMembers.CS
{
    /// <summary>
    /// This command shows how to get In-place Family instance properties and
    /// paint it AnalyticalModel profile on a PictureBox.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        static ExternalCommandData m_commandData;

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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
            ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            m_commandData = commandData;
            Transaction transaction = new Transaction(commandData.Application.ActiveUIDocument.Document, "External Tool");

            FamilyInstance inPlace = null;
            AnalyticalModel model = null; 
            try
            {
                transaction.Start();
                if (!PrepareData(ref inPlace, ref model))
                {
                    message = "You should select only one in place member which have analytical model.";
                    return Autodesk.Revit.UI.Result.Failed;
                }

                GraphicsData graphicsData = GraphicsDataFactory.CreateGraphicsData(model);
                Properties instanceProperties = new Properties(inPlace);
                InPlaceMembersForm form = new InPlaceMembersForm(instanceProperties, graphicsData);
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.Abort)
                {
                    return Autodesk.Revit.UI.Result.Failed;
                }

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
            finally
            {
                transaction.Commit();
            }
        }

        /// <summary>
        /// Search for the In-Place family instance's properties data to be listed
        /// and graphics data to be drawn.
        /// </summary>
        /// <param name="inPlaceMember">properties data to be listed</param>
        /// <param name="model">graphics data to be draw</param>
        /// <returns>Returns true if retrieved this data</returns>
        private bool PrepareData(ref FamilyInstance inPlaceMember, ref AnalyticalModel model)
        {
            ElementSet selected = m_commandData.Application.ActiveUIDocument.Selection.Elements;

            if (selected.Size != 1)
            {
                return false;
            }

            foreach (object o in selected)
            {
                inPlaceMember = o as FamilyInstance;
                if (null == inPlaceMember)
                {
                    return false;
                }
            }

            model = inPlaceMember.GetAnalyticalModel();

            if (null==model)
            {
                return false;
            }

            return true;
        }
    }
}
