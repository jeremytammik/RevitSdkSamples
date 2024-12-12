//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Collections;
using System.Drawing;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.PathReinforcement.CS
{
    /// <summary>
    /// The entrance of this example, implement the Execute method of IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        /// <summary>
        /// static field used to store all RebarBarType in document.
        /// </summary>
        private static Hashtable s_rebarBarTypes = new Hashtable();

        /// <summary>
        /// static property corresponding to s_rebarBarTypes field.
        /// </summary>
        public static Hashtable BarTypes
        {
            get
            {
                return s_rebarBarTypes;
            }
        }

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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
            ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Transaction transaction = new Transaction(commandData.Application.ActiveUIDocument.Document, "External Tool");
            try
            {
                transaction.Start();
                ElementSet elems = commandData.Application.ActiveUIDocument.Selection.Elements;
                #region selection error handle
                //if user have some wrong selection, give user an Error message
                if (1 != elems.Size)
                {
                    message = "please select one PathReinforcement.";
                    return Autodesk.Revit.UI.Result.Cancelled;
                }

                Autodesk.Revit.DB.Element selectElem = null;
                foreach (Autodesk.Revit.DB.Element e in elems)
                {
                    selectElem = e;
                }

                if (!(selectElem is Autodesk.Revit.DB.Structure.PathReinforcement))
                {
                    message = "please select one PathReinforcement.";
                    return Autodesk.Revit.UI.Result.Cancelled;
                }
                #endregion

                //clear all rebar bar type.
                if (s_rebarBarTypes.Count > 0)
                {
                    s_rebarBarTypes.Clear();
                }

                //get all bar type.
                FilteredElementCollector collector = new FilteredElementCollector(commandData.Application.ActiveUIDocument.Document);
                FilteredElementIterator itor = collector.OfClass(typeof(RebarBarType)).GetElementIterator();
                itor.Reset();
                while (itor.MoveNext())
                {
                    RebarBarType bartype = itor.Current as RebarBarType;
                    if (null != bartype)
                    {
                        Autodesk.Revit.DB.ElementId id = bartype.Id;
                        String name = bartype.Name;
                        s_rebarBarTypes.Add(name, id);
                    }
                }

                //Create a form to view the path reinforcement.
                Autodesk.Revit.DB.Structure.PathReinforcement pathRein = selectElem as
                                       Autodesk.Revit.DB.Structure.PathReinforcement;
                using (PathReinforcementForm form = new PathReinforcementForm(pathRein, commandData))
                {
                    form.ShowDialog();
                }
            }
            catch (Exception e)
            {
                transaction.RollBack();
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
            finally
            {
                transaction.Commit();
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        #endregion
    }
}
