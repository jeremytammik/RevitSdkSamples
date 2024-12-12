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
using System.Linq;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace Revit.SDK.Samples.BRepBuilderExample.CS
{
   /// <summary>
   /// Implement method Execute of this class as an external command for Revit.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   public class CreateCube : IExternalCommand
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
            Solid mySolid = CreateCubeImpl().GetResult();
            if(null == mySolid)
               return Result.Failed;

            using (Autodesk.Revit.DB.Transaction tran = new Autodesk.Revit.DB.Transaction(_dbdocument, "CreateCube"))
            {
               tran.Start();
               DirectShape dsCubed = DirectShape.CreateElement(_dbdocument, new ElementId(BuiltInCategory.OST_Walls));
               if (null == dsCubed)
                  return Result.Failed;
               dsCubed.ApplicationId = "TestCreateCube";
               dsCubed.ApplicationDataId = "Cube";
               List<GeometryObject> shapes = new List<GeometryObject>();
               shapes.Add(mySolid);
               dsCubed.SetShape(shapes, DirectShapeTargetViewType.Default);

               tran.Commit();
            }
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }

         return Result.Succeeded;
      }

      private BRepBuilder CreateCubeImpl()
      {
         // create a BRepBuilder; add faces to build a cube

         BRepBuilder brepBuilder = new BRepBuilder(BRepType.Solid);

         // a cube 100x100x100, from (0,0,0) to (100, 100, 100)

         // 1. Planes.
         // naming convention for faces and planes:
         // We are looking at this cube in an isometric view. X is down and to the left of us, Y is horizontal and points to the right, Z is up.
         // front and back faces are along the X axis, left and right are along the Y axis, top and bottom are along the Z axis.
         Plane bottom = Plane.CreateByOriginAndBasis(new XYZ(50, 50, 0), new XYZ(1, 0, 0), new XYZ(0, 1, 0)); // bottom. XY plane, Z = 0, normal pointing inside the cube.
         Plane top = Plane.CreateByOriginAndBasis(new XYZ(50, 50, 100), new XYZ(1, 0, 0), new XYZ(0, 1, 0)); // top. XY plane, Z = 100, normal pointing outside the cube.
         Plane front = Plane.CreateByOriginAndBasis(new XYZ(100, 50, 50), new XYZ(0, 0, 1), new XYZ(0, 1, 0)); // front side. ZY plane, X = 0, normal pointing inside the cube.
         Plane back = Plane.CreateByOriginAndBasis(new XYZ(0, 50, 50), new XYZ(0, 0, 1), new XYZ(0, 1, 0)); // back side. ZY plane, X = 0, normal pointing outside the cube.
         Plane left = Plane.CreateByOriginAndBasis(new XYZ(50, 0, 50), new XYZ(0, 0, 1), new XYZ(1, 0, 0)); // left side. ZX plane, Y = 0, normal pointing inside the cube
         Plane right = Plane.CreateByOriginAndBasis(new XYZ(50, 100, 50), new XYZ(0, 0, 1), new XYZ(1, 0, 0)); // right side. ZX plane, Y = 100, normal pointing outside the cube
                                                               //Note that the alternating of "inside/outside" matches the alternating of "true/false" in the next block that defines faces. 
                                                               //There must be a correspondence to ensure that all faces are correctly oriented to point out of the solid.
         // 2. Faces.
         BRepBuilderGeometryId faceId_Bottom = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(bottom, null), true);
         BRepBuilderGeometryId faceId_Top = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(top, null), false);
         BRepBuilderGeometryId faceId_Front = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(front, null), true);
         BRepBuilderGeometryId faceId_Back = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(back, null), false);
         BRepBuilderGeometryId faceId_Left = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(left, null), true);
         BRepBuilderGeometryId faceId_Right = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(right, null), false);

         // 3. Edges.

         // 3.a (define edge geometry)
         // walk around bottom face
         BRepBuilderEdgeGeometry edgeBottomFront = BRepBuilderEdgeGeometry.Create(new XYZ(100, 0, 0), new XYZ(100, 100, 0));
         BRepBuilderEdgeGeometry edgeBottomRight = BRepBuilderEdgeGeometry.Create(new XYZ(100, 100, 0), new XYZ(0, 100, 0));
         BRepBuilderEdgeGeometry edgeBottomBack = BRepBuilderEdgeGeometry.Create(new XYZ(0, 100, 0), new XYZ(0, 0, 0));
         BRepBuilderEdgeGeometry edgeBottomLeft = BRepBuilderEdgeGeometry.Create(new XYZ(0, 0, 0), new XYZ(100, 0, 0));

         // now walk around top face
         BRepBuilderEdgeGeometry edgeTopFront = BRepBuilderEdgeGeometry.Create(new XYZ(100, 0, 100), new XYZ(100, 100, 100));
         BRepBuilderEdgeGeometry edgeTopRight = BRepBuilderEdgeGeometry.Create(new XYZ(100, 100, 100), new XYZ(0, 100, 100));
         BRepBuilderEdgeGeometry edgeTopBack = BRepBuilderEdgeGeometry.Create(new XYZ(0, 100, 100), new XYZ(0, 0, 100));
         BRepBuilderEdgeGeometry edgeTopLeft = BRepBuilderEdgeGeometry.Create(new XYZ(0, 0, 100), new XYZ(100, 0, 100));

         // sides
         BRepBuilderEdgeGeometry edgeFrontRight = BRepBuilderEdgeGeometry.Create(new XYZ(100, 100, 0), new XYZ(100, 100, 100));
         BRepBuilderEdgeGeometry edgeRightBack = BRepBuilderEdgeGeometry.Create(new XYZ(0, 100, 0), new XYZ(0, 100, 100));
         BRepBuilderEdgeGeometry edgeBackLeft = BRepBuilderEdgeGeometry.Create(new XYZ(0, 0, 0), new XYZ(0, 0, 100));
         BRepBuilderEdgeGeometry edgeLeftFront = BRepBuilderEdgeGeometry.Create(new XYZ(100, 0, 0), new XYZ(100, 0, 100));

         // 3.b (define the edges themselves)
         BRepBuilderGeometryId edgeId_BottomFront = brepBuilder.AddEdge(edgeBottomFront);
         BRepBuilderGeometryId edgeId_BottomRight = brepBuilder.AddEdge(edgeBottomRight);
         BRepBuilderGeometryId edgeId_BottomBack = brepBuilder.AddEdge(edgeBottomBack);
         BRepBuilderGeometryId edgeId_BottomLeft = brepBuilder.AddEdge(edgeBottomLeft);
         BRepBuilderGeometryId edgeId_TopFront = brepBuilder.AddEdge(edgeTopFront);
         BRepBuilderGeometryId edgeId_TopRight = brepBuilder.AddEdge(edgeTopRight);
         BRepBuilderGeometryId edgeId_TopBack = brepBuilder.AddEdge(edgeTopBack);
         BRepBuilderGeometryId edgeId_TopLeft = brepBuilder.AddEdge(edgeTopLeft);
         BRepBuilderGeometryId edgeId_FrontRight = brepBuilder.AddEdge(edgeFrontRight);
         BRepBuilderGeometryId edgeId_RightBack = brepBuilder.AddEdge(edgeRightBack);
         BRepBuilderGeometryId edgeId_BackLeft = brepBuilder.AddEdge(edgeBackLeft);
         BRepBuilderGeometryId edgeId_LeftFront = brepBuilder.AddEdge(edgeLeftFront);

         // 4. Loops.
         BRepBuilderGeometryId loopId_Bottom = brepBuilder.AddLoop(faceId_Bottom);
         BRepBuilderGeometryId loopId_Top = brepBuilder.AddLoop(faceId_Top);
         BRepBuilderGeometryId loopId_Front = brepBuilder.AddLoop(faceId_Front);
         BRepBuilderGeometryId loopId_Back = brepBuilder.AddLoop(faceId_Back);
         BRepBuilderGeometryId loopId_Right = brepBuilder.AddLoop(faceId_Right);
         BRepBuilderGeometryId loopId_Left = brepBuilder.AddLoop(faceId_Left);

         // 5. Co-edges. 
         // Bottom face. All edges reversed
         brepBuilder.AddCoEdge(loopId_Bottom, edgeId_BottomFront, true); // other direction in front loop
         brepBuilder.AddCoEdge(loopId_Bottom, edgeId_BottomLeft, true);  // other direction in left loop
         brepBuilder.AddCoEdge(loopId_Bottom, edgeId_BottomBack, true);  // other direction in back loop
         brepBuilder.AddCoEdge(loopId_Bottom, edgeId_BottomRight, true); // other direction in right loop
         brepBuilder.FinishLoop(loopId_Bottom);
         brepBuilder.FinishFace(faceId_Bottom);

         // Top face. All edges NOT reversed.
         brepBuilder.AddCoEdge(loopId_Top, edgeId_TopFront, false);  // other direction in front loop.
         brepBuilder.AddCoEdge(loopId_Top, edgeId_TopRight, false);  // other direction in right loop
         brepBuilder.AddCoEdge(loopId_Top, edgeId_TopBack, false);   // other direction in back loop
         brepBuilder.AddCoEdge(loopId_Top, edgeId_TopLeft, false);   // other direction in left loop
         brepBuilder.FinishLoop(loopId_Top);
         brepBuilder.FinishFace(faceId_Top);

         // Front face.
         brepBuilder.AddCoEdge(loopId_Front, edgeId_BottomFront, false); // other direction in bottom loop
         brepBuilder.AddCoEdge(loopId_Front, edgeId_FrontRight, false);  // other direction in right loop
         brepBuilder.AddCoEdge(loopId_Front, edgeId_TopFront, true); // other direction in top loop.
         brepBuilder.AddCoEdge(loopId_Front, edgeId_LeftFront, true); // other direction in left loop.
         brepBuilder.FinishLoop(loopId_Front);
         brepBuilder.FinishFace(faceId_Front);

         // Back face
         brepBuilder.AddCoEdge(loopId_Back, edgeId_BottomBack, false); // other direction in bottom loop
         brepBuilder.AddCoEdge(loopId_Back, edgeId_BackLeft, false);   // other direction in left loop.
         brepBuilder.AddCoEdge(loopId_Back, edgeId_TopBack, true); // other direction in top loop
         brepBuilder.AddCoEdge(loopId_Back, edgeId_RightBack, true); // other direction in right loop.
         brepBuilder.FinishLoop(loopId_Back);
         brepBuilder.FinishFace(faceId_Back);

         // Right face
         brepBuilder.AddCoEdge(loopId_Right, edgeId_BottomRight, false); // other direction in bottom loop
         brepBuilder.AddCoEdge(loopId_Right, edgeId_RightBack, false);  // other direction in back loop
         brepBuilder.AddCoEdge(loopId_Right, edgeId_TopRight, true);   // other direction in top loop
         brepBuilder.AddCoEdge(loopId_Right, edgeId_FrontRight, true); // other direction in front loop
         brepBuilder.FinishLoop(loopId_Right);
         brepBuilder.FinishFace(faceId_Right);

         // Left face
         brepBuilder.AddCoEdge(loopId_Left, edgeId_BottomLeft, false); // other direction in bottom loop
         brepBuilder.AddCoEdge(loopId_Left, edgeId_LeftFront, false); // other direction in front loop
         brepBuilder.AddCoEdge(loopId_Left, edgeId_TopLeft, true);   // other direction in top loop
         brepBuilder.AddCoEdge(loopId_Left, edgeId_BackLeft, true);  // other direction in back loop
         brepBuilder.FinishLoop(loopId_Left);
         brepBuilder.FinishFace(faceId_Left);

         brepBuilder.Finish();
         return brepBuilder;
      }

      private Document _dbdocument = null;

   }
}
