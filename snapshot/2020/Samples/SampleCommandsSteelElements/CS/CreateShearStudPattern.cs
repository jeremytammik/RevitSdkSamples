//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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


using Autodesk.Revit.UI.Selection;
using Autodesk.AdvanceSteel.CADAccess;
using Autodesk.AdvanceSteel.Geometry;
using RvtDwgAddon;
using Autodesk.AdvanceSteel.Arrangement;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;



namespace Revit.SDK.Samples.SampleCommandsSteelElements.CreateShearStudPattern.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
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
      public virtual Result Execute(ExternalCommandData commandData
          , ref string message, ElementSet elements)
      {
         //Get the document from external command data.
         UIDocument activeDoc = commandData.Application.ActiveUIDocument;
         Autodesk.Revit.DB.Document doc = activeDoc.Document;

         if (null == doc)
         {
            return Result.Failed;
         }

         try
         {
            // The list of elements to create the shear stud pattern on
            Reference eRef = activeDoc.Selection.PickObject(ObjectType.Face, "Please pick an element to create the shear stud pattern on");

            // Start detailed steel modeling transaction
            using (FabricationTransaction trans = new FabricationTransaction(doc, false, "Create shear stud pattern"))
            {
               // Creating the shear stud pattern involves using AdvanceSteel classes and objects only.
               // for more details, please consult http://www.autodesk.com/adv-steel-api-walkthroughs-2019-enu
               FilerObject filerObj = Utilities.Functions.GetFilerObject(doc, eRef);

               if (null == filerObj)
               {
                  return Result.Failed;
               }

               Autodesk.AdvanceSteel.Modelling.Connector shearStud = new Autodesk.AdvanceSteel.Modelling.Connector();
               shearStud.WriteToDb();

               // Creating a rectangle for the shear stud pattern. A Polyline3d is required for the rectangle. We will use it for the Arranger and the Matrix3d objects.
               Polyline3d polyLine = new Polyline3d();
               Matrix3d matCS = new Matrix3d();
               Arranger arranger = null;

               double x = eRef.GlobalPoint.X * Utilities.Functions.FEET_TO_MM;
               double y = eRef.GlobalPoint.Y * Utilities.Functions.FEET_TO_MM;
               double z = eRef.GlobalPoint.Z * Utilities.Functions.FEET_TO_MM;
               double w = 500;   // width of the contour;
               double h = 500;   // height of the contour;

               polyLine.Append(new Point3d(x, y, z));
               polyLine.Append(new Point3d(x + w, y, z));
               polyLine.Append(new Point3d(x + w, y + h, z));
               polyLine.Append(new Point3d(x, y + h, z));

               Point3d[] vertices = polyLine.Vertices;

               if (vertices.Length == 4) //rectangular shear stud pattern
               {
                  Vector3d xAxis1 = vertices[0].Subtract(vertices[1]);
                  Vector3d xAxis2 = vertices[0].Subtract(vertices[3]);
                  Vector3d zAxis = new Vector3d(0, 0, 1);
                  Vector3d vDiag = vertices[0].Subtract(vertices[2]);
                  Point3d orig = new Point3d(vertices[2]);
                  orig = orig.Add(vDiag * 0.5);
                  if (xAxis1.GetLength() > xAxis2.GetLength())
                  {
                     arranger = new BoundedRectArranger(xAxis1.GetLength(), xAxis2.GetLength());
                     matCS.SetCoordSystem(orig, xAxis1.Normalize(), zAxis.CrossProduct(xAxis1).Normalize(), zAxis.Normalize());
                  }
                  else
                  {
                     arranger = new BoundedRectArranger(xAxis2.GetLength(), xAxis1.GetLength());
                     matCS.SetCoordSystem(orig, xAxis2.Normalize(), zAxis.CrossProduct(xAxis2).Normalize(), zAxis.Normalize());
                  }
                  arranger.Nx = 2;
                  arranger.Ny = 2;
               }

               shearStud.Arranger = arranger;
               shearStud.SetCS(matCS);
               shearStud.Connect(filerObj, matCS);
               shearStud.Material = "Mild Steel";
               shearStud.Standard = "Nelson S3L-Inch";
               shearStud.Diameter = 19.05;
               shearStud.Length = 101.6;

               trans.Commit();
            }
         }
         catch (Autodesk.Revit.Exceptions.OperationCanceledException)
         {
            return Result.Cancelled;
         }
         return Result.Succeeded;
      }
   }
}

