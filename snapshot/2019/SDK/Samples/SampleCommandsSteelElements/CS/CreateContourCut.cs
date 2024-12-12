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


using Autodesk.Revit.UI.Selection;
using Autodesk.AdvanceSteel.CADAccess;
using Autodesk.Revit.DB.Steel;
using Autodesk.AdvanceSteel.Geometry;
using Autodesk.AdvanceSteel.Modelling;
using RvtDwgAddon;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;



namespace Revit.SDK.Samples.SampleCommandsSteelElements.CreateContourCut.CS
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
         // Get the document from external command data.
         UIDocument activeDoc = commandData.Application.ActiveUIDocument;
         Autodesk.Revit.DB.Document doc = activeDoc.Document;

         if (null == doc)
         {
            return Result.Failed;
         }

         try
         {
            // Starting the transaction, using CYBORG's FabricationTransaction class
            using (FabricationTransaction trans = new FabricationTransaction(doc, false, "Create contour cut"))
            {

               // for more details, please consult http://www.autodesk.com/adv-steel-api-walkthroughs-2019-enu

               // Selecting the elements to create the contour cut on
               Reference eRef = activeDoc.Selection.PickObject(ObjectType.Element, "Please pick an element to create the contour cut on");
               // getting the selected element
               Element elem = null;
               if (eRef != null && eRef.ElementId != ElementId.InvalidElementId)
               {
                  elem = doc.GetElement(eRef.ElementId);
               }

               if (null == elem)
                  return Result.Failed;


               // ensuring the element has FabricationData
               SteelElementProperties cell = SteelElementProperties.GetSteelElementProperties(elem);
               if (null == cell)
               {
                  List<ElementId> elemsIds = new List<ElementId>();
                  elemsIds.Add(elem.Id);
                  SteelElementProperties.AddFabricationInformationForRevitElements(doc, elemsIds);
               }
               FilerObject filerObj = Utilities.Functions.GetFilerObject(doc, eRef);
               if (null == filerObj)
               {
                  return Result.Failed;
               }

               if (!(filerObj is Plate) && !(filerObj is Beam))
               {
                  return Result.Failed;
               }

               // The point where the element was hit when selected.
               Point3d p1 = new Point3d(eRef.GlobalPoint.X, eRef.GlobalPoint.Y, eRef.GlobalPoint.Z);
               // Contour coordinates, according to the point where the element was hit when selected
               double x = Utilities.Functions.FEET_TO_MM * p1.x;    // x of upper-left corner of the contour;
               double y = Utilities.Functions.FEET_TO_MM * p1.y;    // y of upper-left corner of the contour;
               double z = Utilities.Functions.FEET_TO_MM * p1.z;    // z of upper-left corner of the contour;


               // Adding the contour cut, using AdvanceSteel classes.

               Polyline3d plContour;
               Polyline3d contour = new Polyline3d();

               if (filerObj is Plate)
               {
                  double w = 500;   // width of the contour;
                  double h = 500;   // height of the contour;
                  contour.Append(new Point3d(x, y, z));
                  contour.Append(new Point3d(x + w, y, z));
                  contour.Append(new Point3d(x + w, y + h, z));
                  contour.Append(new Point3d(x, y + h, z));
                  if (contour.VertexCount <= 0)
                  {
                     return Result.Failed;
                  }
                  Plate plate = filerObj as Plate;
                  plate.GetBaseContourPolygon(1, out plContour);
                  plContour.Project(new Autodesk.AdvanceSteel.Geometry.Plane(contour.Vertices[0], contour.PolyNormal), contour.PolyNormal);
                  PlateContourNotch plateContour = new PlateContourNotch(plate, 0, contour, contour.Normal, contour.Normal.GetPerpVector());
                  plate.AddFeature(plateContour);
               }
               else if (filerObj is Beam)
               {
                  double w = 50;   // width of the contour;
                  double h = 50;   // height of the contour;
                  contour.Append(new Point3d(x, y, z));
                  contour.Append(new Point3d(x + w, y, z));
                  contour.Append(new Point3d(x + w, y, z + h));
                  contour.Append(new Point3d(x, y, z + h));
                  if (contour.VertexCount <= 0)
                  {
                     return Result.Failed;
                  }
                  Beam beam = filerObj as Beam;
                  Point3d ptClosest = beam.GetClosestPointToSystemline(contour.Vertices[0]);
                  double d1 = ptClosest.DistanceTo(beam.GetPointAtStart());
                  double d2 = ptClosest.DistanceTo(beam.GetPointAtEnd());

                  Beam.eEnd beamEnd = d1 > d2 ? Beam.eEnd.kEnd : Beam.eEnd.kStart;

                  Vector3d normal = contour.Normal;
                  Vector3d xVec = normal.GetPerpVector();
                  BeamMultiContourNotch beamContour = new BeamMultiContourNotch(beam, beamEnd, contour, normal, xVec);
                  beam.AddFeature(beamContour);
               }
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

