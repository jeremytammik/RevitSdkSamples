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
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.BRepBuilderExample.CS
{
   /// <summary>
   /// Implement method Execute of this class as an external command for Revit.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   public class CreatePeriodic : IExternalCommand
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
      /// Cancelled can be used to signify that the user canceled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with
      /// the operation.</returns>
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         _dbdocument = commandData.Application.ActiveUIDocument.Document;


         try
         {
            CreateCylinder();
            CreateTruncatedCone();
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }

         return Result.Succeeded;
      }

      /// <summary>
      /// Create a DirectShape element from a BRepBuilder object and keep it in the _dbdocument.
      /// The main purpose is to display the BRepBuilder objects created.
      /// In this function, the BrepBuilder is directly set to a DirectShape.
      /// </summary>
      /// <param name="myBRepBuilder"> The BRepBuilder object.</param>
      /// <param name="name"> Name of the BRepBuilder object, which will be passed on to the DirectShape creation method.</param>
      private void createDirectShapeElementFromBrepBuilderObject(BRepBuilder myBRepBuilder, String name)
      {
         if (!myBRepBuilder.IsResultAvailable())
            return;

         using (Transaction tr = new Transaction(_dbdocument, "Create a DirectShape"))
         {
            tr.Start();

            DirectShape myDirectShape = DirectShape.CreateElement(_dbdocument, new ElementId(BuiltInCategory.OST_GenericModel));
            myDirectShape.ApplicationId = "TestBRepBuilder";
            myDirectShape.ApplicationDataId = name;
            if (null != myDirectShape)
               myDirectShape.SetShape(myBRepBuilder);
            tr.Commit();
         }
      }


      private void CreateCylinder()
      {
         // Naming convention for faces and edges: we assume that x is to the left and pointing down, y is horizontal and pointing to the right, z is up
         BRepBuilder brepBuilder = new BRepBuilder(BRepType.Solid);

         // The surfaces of the four faces.
         Frame basis = new Frame(new XYZ(50, -100, 0), new XYZ(0, 1, 0), new XYZ(-1, 0, 0), new XYZ(0, 0, 1));
         // Note that we do not have to create two identical surfaces here. The same surface can be used for multiple faces, 
         // since BRepBuilderSurfaceGeometry::Create() copies the input surface.
         // Thus, potentially we could have only one surface here, 
         // but we must create at least two faces below to account for periodicity. 
         CylindricalSurface frontCylSurf = CylindricalSurface.Create(basis, 50);
         CylindricalSurface backCylSurf = CylindricalSurface.Create(basis, 50);
         Plane top = Plane.CreateByNormalAndOrigin(new XYZ(0, 0, 1), new XYZ(0, 0, 100));  // normal points outside the cylinder
         Plane bottom = Plane.CreateByNormalAndOrigin(new XYZ(0, 0, 1), new XYZ(0, 0, 0)); // normal points inside the cylinder
                                                                                           // Note that the alternating of "inside/outside" matches the alternating of "true/false" in the next block that defines faces. 
                                                                                           // There must be a correspondence to ensure that all faces are correctly oriented to point out of the solid.

         // Add the four faces
         BRepBuilderGeometryId frontCylFaceId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(frontCylSurf, null), false);
         BRepBuilderGeometryId backCylFaceId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(backCylSurf, null), false);
         BRepBuilderGeometryId topFaceId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(top, null), false);
         BRepBuilderGeometryId bottomFaceId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(bottom, null), true);

         // Geometry for the four semi-circular edges and two vertical linear edges
         BRepBuilderEdgeGeometry frontEdgeBottom = BRepBuilderEdgeGeometry.Create(Arc.Create(new XYZ(0, -100, 0), new XYZ(100, -100, 0), new XYZ(50, -50, 0)));
         BRepBuilderEdgeGeometry backEdgeBottom = BRepBuilderEdgeGeometry.Create(Arc.Create(new XYZ(100, -100, 0), new XYZ(0, -100, 0), new XYZ(50, -150, 0)));

         BRepBuilderEdgeGeometry frontEdgeTop = BRepBuilderEdgeGeometry.Create(Arc.Create(new XYZ(0, -100, 100), new XYZ(100, -100, 100), new XYZ(50, -50, 100)));
         BRepBuilderEdgeGeometry backEdgeTop = BRepBuilderEdgeGeometry.Create(Arc.Create(new XYZ(0, -100, 100), new XYZ(100, -100, 100), new XYZ(50, -150, 100)));

         BRepBuilderEdgeGeometry linearEdgeFront = BRepBuilderEdgeGeometry.Create(new XYZ(100, -100, 0), new XYZ(100, -100, 100));
         BRepBuilderEdgeGeometry linearEdgeBack = BRepBuilderEdgeGeometry.Create(new XYZ(0, -100, 0), new XYZ(0, -100, 100));

         // Add the six edges
         BRepBuilderGeometryId frontEdgeBottomId = brepBuilder.AddEdge(frontEdgeBottom);
         BRepBuilderGeometryId frontEdgeTopId = brepBuilder.AddEdge(frontEdgeTop);
         BRepBuilderGeometryId linearEdgeFrontId = brepBuilder.AddEdge(linearEdgeFront);
         BRepBuilderGeometryId linearEdgeBackId = brepBuilder.AddEdge(linearEdgeBack);
         BRepBuilderGeometryId backEdgeBottomId = brepBuilder.AddEdge(backEdgeBottom);
         BRepBuilderGeometryId backEdgeTopId = brepBuilder.AddEdge(backEdgeTop);

         // Loops of the four faces
         BRepBuilderGeometryId loopId_Top = brepBuilder.AddLoop(topFaceId);
         BRepBuilderGeometryId loopId_Bottom = brepBuilder.AddLoop(bottomFaceId);
         BRepBuilderGeometryId loopId_Front = brepBuilder.AddLoop(frontCylFaceId);
         BRepBuilderGeometryId loopId_Back = brepBuilder.AddLoop(backCylFaceId);

         // Add coedges for the loop of the front face
         brepBuilder.AddCoEdge(loopId_Front, linearEdgeBackId, false);
         brepBuilder.AddCoEdge(loopId_Front, frontEdgeTopId, false);
         brepBuilder.AddCoEdge(loopId_Front, linearEdgeFrontId, true);
         brepBuilder.AddCoEdge(loopId_Front, frontEdgeBottomId, true);
         brepBuilder.FinishLoop(loopId_Front);
         brepBuilder.FinishFace(frontCylFaceId);

         // Add coedges for the loop of the back face
         brepBuilder.AddCoEdge(loopId_Back, linearEdgeBackId, true);
         brepBuilder.AddCoEdge(loopId_Back, backEdgeBottomId, true);
         brepBuilder.AddCoEdge(loopId_Back, linearEdgeFrontId, false);
         brepBuilder.AddCoEdge(loopId_Back, backEdgeTopId, true);
         brepBuilder.FinishLoop(loopId_Back);
         brepBuilder.FinishFace(backCylFaceId);

         // Add coedges for the loop of the top face
         brepBuilder.AddCoEdge(loopId_Top, backEdgeTopId, false);
         brepBuilder.AddCoEdge(loopId_Top, frontEdgeTopId, true);
         brepBuilder.FinishLoop(loopId_Top);
         brepBuilder.FinishFace(topFaceId);

         // Add coedges for the loop of the bottom face
         brepBuilder.AddCoEdge(loopId_Bottom, frontEdgeBottomId, false);
         brepBuilder.AddCoEdge(loopId_Bottom, backEdgeBottomId, false);
         brepBuilder.FinishLoop(loopId_Bottom);
         brepBuilder.FinishFace(bottomFaceId);

         brepBuilder.Finish();

         createDirectShapeElementFromBrepBuilderObject(brepBuilder, "Full cylinder");
      }

      private void CreateTruncatedCone()
      {
         BRepBuilder brepBuilder = new BRepBuilder(BRepType.Solid);
         Plane bottom = Plane.CreateByNormalAndOrigin(new XYZ(0, 0, -1), new XYZ(0, 0, 0));
         Plane top = Plane.CreateByNormalAndOrigin(new XYZ(0, 0, 1), new XYZ(0, 0, 50));

         Frame basis = new Frame(new XYZ(0, 0, 100), new XYZ(0, 1, 0), new XYZ(1, 0, 0), new XYZ(0, 0, -1));

         // Note that we do not have to create two identical surfaces here. The same surface can be used for multiple faces, 
         // since BRepBuilderSurfaceGeometry::Create() copies the input surface.
         // Thus, potentially we could have only one surface here, 
         // but we must create at least two faces below to account for periodicity. 
         ConicalSurface rightConicalSurface = ConicalSurface.Create(basis, Math.Atan(0.5));
         ConicalSurface leftConicalSurface = ConicalSurface.Create(basis, Math.Atan(0.5));

         // Create 4 faces of the cone
         BRepBuilderGeometryId topFaceId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(top, null), false);
         BRepBuilderGeometryId bottomFaceId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(bottom, null), false);
         BRepBuilderGeometryId rightSideFaceId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(rightConicalSurface, null), false);
         BRepBuilderGeometryId leftSideFaceId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(leftConicalSurface, null), false);

         // Create 2 edges at the bottom of the cone
         BRepBuilderEdgeGeometry bottomRightEdgeGeom = BRepBuilderEdgeGeometry.Create(Arc.Create(new XYZ(-50, 0, 0), new XYZ(50, 0, 0), new XYZ(0, 50, 0)));
         BRepBuilderEdgeGeometry bottomLeftEdgeGeom = BRepBuilderEdgeGeometry.Create(Arc.Create(new XYZ(50, 0, 0), new XYZ(-50, 0, 0), new XYZ(0, -50, 0)));

         // Create 2 edges at the top of the cone
         BRepBuilderEdgeGeometry topLeftEdgeGeom = BRepBuilderEdgeGeometry.Create(Arc.Create(new XYZ(-25, 0, 50), new XYZ(25, 0, 50), new XYZ(0, -25, 50)));
         BRepBuilderEdgeGeometry topRightEdgeGeom = BRepBuilderEdgeGeometry.Create(Arc.Create(new XYZ(25, 0, 50), new XYZ(-25, 0, 50), new XYZ(0, 25, 50)));

         // Create 2 side edges of the cone
         BRepBuilderEdgeGeometry sideFrontEdgeGeom = BRepBuilderEdgeGeometry.Create(new XYZ(25, 0, 50), new XYZ(50, 0, 0));
         BRepBuilderEdgeGeometry sideBackEdgeGeom = BRepBuilderEdgeGeometry.Create(new XYZ(-25, 0, 50), new XYZ(-50, 0, 0));

         BRepBuilderGeometryId bottomRightId = brepBuilder.AddEdge(bottomRightEdgeGeom);
         BRepBuilderGeometryId bottomLeftId = brepBuilder.AddEdge(bottomLeftEdgeGeom);
         BRepBuilderGeometryId topRightEdgeId = brepBuilder.AddEdge(topRightEdgeGeom);
         BRepBuilderGeometryId topLeftEdgeId = brepBuilder.AddEdge(topLeftEdgeGeom);
         BRepBuilderGeometryId sideFrontEdgeid = brepBuilder.AddEdge(sideFrontEdgeGeom);
         BRepBuilderGeometryId sideBackEdgeId = brepBuilder.AddEdge(sideBackEdgeGeom);


         // Create bottom face
         BRepBuilderGeometryId bottomLoopId = brepBuilder.AddLoop(bottomFaceId);
         brepBuilder.AddCoEdge(bottomLoopId, bottomRightId, false);
         brepBuilder.AddCoEdge(bottomLoopId, bottomLeftId, false);
         brepBuilder.FinishLoop(bottomLoopId);
         brepBuilder.FinishFace(bottomFaceId);

         // Create top face
         BRepBuilderGeometryId topLoopId = brepBuilder.AddLoop(topFaceId);
         brepBuilder.AddCoEdge(topLoopId, topLeftEdgeId, false);
         brepBuilder.AddCoEdge(topLoopId, topRightEdgeId, false);
         brepBuilder.FinishLoop(topLoopId);
         brepBuilder.FinishFace(topFaceId);

         // Create right face
         BRepBuilderGeometryId rightLoopId = brepBuilder.AddLoop(rightSideFaceId);
         brepBuilder.AddCoEdge(rightLoopId, topRightEdgeId, true);
         brepBuilder.AddCoEdge(rightLoopId, sideFrontEdgeid, false);
         brepBuilder.AddCoEdge(rightLoopId, bottomRightId, true);
         brepBuilder.AddCoEdge(rightLoopId, sideBackEdgeId, true);
         brepBuilder.FinishLoop(rightLoopId);
         brepBuilder.FinishFace(rightSideFaceId);

         // Create left face
         BRepBuilderGeometryId leftLoopId = brepBuilder.AddLoop(leftSideFaceId);
         brepBuilder.AddCoEdge(leftLoopId, topLeftEdgeId, true);
         brepBuilder.AddCoEdge(leftLoopId, sideBackEdgeId, false);
         brepBuilder.AddCoEdge(leftLoopId, bottomLeftId, true);
         brepBuilder.AddCoEdge(leftLoopId, sideFrontEdgeid, true);
         brepBuilder.FinishLoop(leftLoopId);
         brepBuilder.FinishFace(leftSideFaceId);

         brepBuilder.Finish();
         createDirectShapeElementFromBrepBuilderObject(brepBuilder, "Cone surface");
      }

      private Document _dbdocument = null;

   }
}
