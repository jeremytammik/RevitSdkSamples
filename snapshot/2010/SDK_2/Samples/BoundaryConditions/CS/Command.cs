//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Structural.Enums;

namespace Revit.SDK.Samples.BoundaryConditions.CS
{
    /// <summary>
    /// A ExternalCommand class inherited IExternalCommand interface
    /// </summary>
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
        public IExternalCommand.Result Execute(ExternalCommandData commandData,
                                               ref string message,
                                               ElementSet elements)
        {
            try
            {
                //Retrieves the currently active project.
                Document doc = commandData.Application.ActiveDocument;

                // must select a element first
                ElementSet elementSet = doc.Selection.Elements;
                if (1 != elementSet.Size)
                {
                    message = "Please select one structural element which is listed as follows: \r\n" +
                              "Columns/braces/Beams/Walls/Wall Foundations/Slabs/Foundation Slabs";
                    return IExternalCommand.Result.Cancelled;
                }

                // deal with the selected element
                foreach (Element element in elementSet)
                {
                    // the selected element must be a structural element
                    if (!IsStructuralElement(element))
                    {
                        message = "Please select one structural element which is listed as follows: \r\n" +
                                  "Columns/braces/Beams/Walls/Wall Foundations/ \r\n" +
                                  "Slabs/Foundation Slabs";
                        return IExternalCommand.Result.Cancelled;
                    }

                    // prepare the relative data
                    BoundaryConditionsData dataBuffer = new BoundaryConditionsData(element);

                    // show UI
                    using (BoundaryConditionsForm displayForm = new BoundaryConditionsForm(dataBuffer))
                    {
                        DialogResult result = displayForm.ShowDialog();
                        if (DialogResult.OK == result)
                        {
                            return IExternalCommand.Result.Succeeded;
                        }
                        else if (DialogResult.Retry == result)
                        {
                            message = "failded to create BoundaryConditions.";
                            return IExternalCommand.Result.Failed;
                        }
                    }    
                }

                // user cancel the operation
                return IExternalCommand.Result.Cancelled;                
            }
            catch (Exception e)
            {
                message = e.Message;
                return IExternalCommand.Result.Failed;
            }
        }

        /// <summary>
        /// the selected element must be a structural element
        /// </summary>
        /// <returns></returns>
        private bool IsStructuralElement(Element element)
        {
            AnalyticalModel analyticalModel = null;

            // judge the element's type. If it is any type of FamilyInstance, Wall, Floor or 
            // WallFoundation, then get judge if it has a AnalyticalModel.
            FamilyInstance familyInstance = element as FamilyInstance;
            if ((null != familyInstance) && (StructuralType.Footing != familyInstance.StructuralType))
            {
                analyticalModel = familyInstance.AnalyticalModel;
            }

            Wall wall = element as Wall;
            if (null != wall)
            {
                analyticalModel = wall.AnalyticalModel;
            }

            Floor floor = element as Floor;
            if (null != floor)
            {
                analyticalModel = floor.AnalyticalModel;
            }

            ContFooting wallFoundation = element as ContFooting;
            if (null != wallFoundation)
            {
                analyticalModel = wallFoundation.AnalyticalModel;
            }

            if (null == analyticalModel)
            {
                return false;
            }

            return true;
        }
    }
}
