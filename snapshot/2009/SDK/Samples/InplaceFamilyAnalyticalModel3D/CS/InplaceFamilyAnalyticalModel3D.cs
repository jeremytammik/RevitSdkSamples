//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Geometry;

namespace Revit.SDK.Samples.InplaceFamilyAnalyticalModel3D.CS
{
    /// <summary>
    /// A short sample that shows how to read an analytical model 3D object 
    /// from an inplace family.
    /// </summary>
    public class Command : IExternalCommand
    {
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
        public IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            Document doc = revit.Application.ActiveDocument;

            //iterate through the selection picking out family instances that have a 3D analytical model
            SelElementSet selElements = doc.Selection.Elements;

            if (0 == selElements.Size)
            {
                message = "Please selected some in-place family instance with AnalyticalMode.";
                return IExternalCommand.Result.Cancelled;
            }

            foreach (Autodesk.Revit.Element element in selElements)
            {
                FamilyInstance familyInstance = element as FamilyInstance;

                if (null == familyInstance)
                {
                    continue;
                }

                AnalyticalModel analyticalModel = familyInstance.AnalyticalModel;
                if (null == analyticalModel)
                {
                    continue;
                }

                AnalyticalModel3D analyticalModel3D = analyticalModel as AnalyticalModel3D;

                if (null == analyticalModel3D)
                {
                    Debug.Print("we should select analytical model 3D family instance, but this familyInstance.AnalyticalModel type is " + analyticalModel.GetType().Name);
                    continue;
                }
               
                //Output the family instance information and the curves of the analytical model. 
                DumpFamilyInstance(familyInstance);
                DumpAnalyticalModel3D(analyticalModel3D);     
            }     
            return IExternalCommand.Result.Succeeded;
        }

        /// <summary>
        /// dump the names of the family, symbol and instance. Since the cross 
        /// section is not available it is expected that these will return something meaningful
        /// </summary>
        /// <param name="familyInstance">a fammilyInstance that have a 3D analytical model</param>
        private void DumpFamilyInstance(FamilyInstance familyInstance)
        {
          
            Debug.Print("Family Name : " + familyInstance.Symbol.Family.Name);
            Debug.Print("Family Symbol Name : " + familyInstance.Symbol.Name);
            Debug.Print("Family Instance Name : " + familyInstance.Name);
        }

        /// <summary>
        /// dump each curve of this FamilyInstance's AnalyticalModel
        /// </summary>
        /// <param name="analyticalModel3D"></param>
        private void DumpAnalyticalModel3D(AnalyticalModel3D analyticalModel3D)
        {
            int counter = 1;

            // the 3D analytical model has a curves property that reports all the 
            // analytical model curves within the in place family instance
            foreach (Curve curve in analyticalModel3D.Curves)
            {
                Debug.Print("Curve" + counter);
                
                // use the tesselate method to fragment all types of curves including lines and arcs etc.
                XYZArray points = curve.Tessellate();

                foreach( XYZ point in points)
                {
                    Debug.Print(point.X.ToString(), point.Y.ToString(), point.Z.ToString());
                }

                counter += 1;
            }
        }
    }
}
