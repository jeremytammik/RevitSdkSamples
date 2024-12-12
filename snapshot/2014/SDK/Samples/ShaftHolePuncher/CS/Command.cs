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
using System.Text;
using System.Collections;
using System.Drawing;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.ShaftHolePuncher.CS
{
    /// <summary>
    /// The entrance of this example, implements the Execute method of IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        #region IExternalCommand Members Implementation
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
            try
            {
                Wall wall = null;
                Floor floor = null;
                FamilyInstance familyInstance = null;
                ElementSet elems = commandData.Application.ActiveUIDocument.Selection.Elements;

                #region selection handle -- select one floor, wall, beam or nothing
                //if user had some wrong selection, give user an Error message
                string errorMessage =
                    "Please select one Floor (Beam or Wall) to create opening or select nothing to create Shaft Opening";
                if (elems.Size > 1)
                {
                    message = errorMessage;
                    return Autodesk.Revit.UI.Result.Cancelled;
                }

                Autodesk.Revit.DB.Element selectElem = null;
                if (1 == elems.Size)
                {
                    IEnumerator iter = elems.GetEnumerator();
                    iter.Reset();
                    if (iter.MoveNext())
                    {
                        selectElem = (Autodesk.Revit.DB.Element)iter.Current;
                    }

                    if (selectElem is Wall)
                    {
                        wall = selectElem as Wall;
                    }
                    else if (selectElem is Floor)
                    {
                        floor = selectElem as Floor;
                    }
                    else if (selectElem is FamilyInstance)
                    {
                        familyInstance = selectElem as FamilyInstance;
                        if (familyInstance.StructuralType !=
                            Autodesk.Revit.DB.Structure.StructuralType.Beam)
                        {
                            message = errorMessage;
                            return Autodesk.Revit.UI.Result.Cancelled;
                        }
                    }
                    else
                    {
                        message = errorMessage;
                        return Autodesk.Revit.UI.Result.Cancelled;
                    }
                }
                #endregion

                try
                {
                    if (null != wall)
                    {
                        ProfileWall profileWall = new ProfileWall(wall, commandData);
                        ShaftHolePuncherForm shaftHolePuncherForm = 
                            new ShaftHolePuncherForm(profileWall);
                        shaftHolePuncherForm.ShowDialog();
                    }
                    else if (null != floor)
                    {
                        ProfileFloor profileFloor = new ProfileFloor(floor, commandData);
                        ShaftHolePuncherForm shaftHolePuncherForm =
                            new ShaftHolePuncherForm(profileFloor);
                        shaftHolePuncherForm.ShowDialog();
                    }
                    else if (null != familyInstance)
                    {
                        ProfileBeam profileBeam = new ProfileBeam(familyInstance, commandData);
                        ShaftHolePuncherForm shaftHolePuncherForm =
                            new ShaftHolePuncherForm(profileBeam);
                        shaftHolePuncherForm.ShowDialog();
                    }
                    else
                    {
                        ProfileNull profileNull = new ProfileNull(commandData);
                        ShaftHolePuncherForm shaftHolePuncherForm =
                            new ShaftHolePuncherForm(profileNull);
                        shaftHolePuncherForm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return Autodesk.Revit.UI.Result.Cancelled;
                }

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
        #endregion
    }
}
