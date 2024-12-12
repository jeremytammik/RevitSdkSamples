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
   public class CreateNURBS : IExternalCommand
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
            using (Autodesk.Revit.DB.Transaction tr = new Autodesk.Revit.DB.Transaction(_dbdocument, "CreateNURBS"))
            {
               tr.Start();

               DirectShape myDirectShape = DirectShape.CreateElement(_dbdocument, new ElementId(BuiltInCategory.OST_GenericModel));
               myDirectShape.ApplicationId = "TestCreateNURBS";
               myDirectShape.ApplicationDataId = "NURBS";

               if (null != myDirectShape)
                  myDirectShape.SetShape(CreateNurbsSurface());
               tr.Commit();
            }
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }

         return Result.Succeeded;
      }



      private BRepBuilder CreateNurbsSurface()
      {
         //Note that we are not creating a closed solid here. It is an open shell.
         BRepBuilder brepBuilder = new BRepBuilder(BRepType.OpenShell);

         // Create NURBS surface
         IList<double> knotsU = new List<double> { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 };
         IList<double> knotsV = new List<double> { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 };
         int degreeU = 4;
         int degreeV = 4;

         IList<XYZ> controlPoints = new List<XYZ>
            {
                new XYZ(0, 0, 0), new XYZ(0, 20, 0), new XYZ(0, 40, 0), new XYZ(0, 60, 0), new XYZ(0, 80, 0),
                new XYZ(20, 0, 100), new XYZ(20, 20, 200), new XYZ(20, 40, 300), new XYZ(20, 60, 200), new XYZ(20, 80, 100),
                new XYZ(40, 0, -100), new XYZ(40, 20, -250), new XYZ(40, 40, -300), new XYZ(40, 60, -250), new XYZ(40, 80, -100),
                new XYZ(60, 0, 100), new XYZ(60, 20, 200), new XYZ(60, 40, 300), new XYZ(60, 60, 200), new XYZ(60, 80, 100),
                new XYZ(80, 0, 0), new XYZ(80, 20, 0), new XYZ(80, 40, 0), new XYZ(80, 60, 0), new XYZ(80, 80, 0)
            };

         BRepBuilderSurfaceGeometry nurbsSurface = BRepBuilderSurfaceGeometry.CreateNURBSSurface(degreeU, degreeV, knotsU, knotsV, controlPoints, false/*bReverseOrientation*/, null /*pSurfaceEnvelope*/);

         // Create 4 NURBS curves defining the boundary of the NURBS surface that has just been created
         IList<double> weights = new List<double> { 1, 1, 1, 1, 1 };

         IList<XYZ> backEdgeControlPoints = new List<XYZ> { new XYZ(0, 0, 0), new XYZ(0, 20, 0), new XYZ(0, 40, 0), new XYZ(0, 60, 0), new XYZ(0, 80, 0) };
         Curve backNurbs = NurbSpline.CreateCurve(4, knotsU, backEdgeControlPoints, weights);
         BRepBuilderEdgeGeometry backEdge = BRepBuilderEdgeGeometry.Create(backNurbs);

         IList<XYZ> frontEdgeControlPoints = new List<XYZ> { new XYZ(80, 0, 0), new XYZ(80, 20, 0), new XYZ(80, 40, 0), new XYZ(80, 60, 0), new XYZ(80, 80, 0) };
         Curve frontNurbs = NurbSpline.CreateCurve(4, knotsU, frontEdgeControlPoints, weights);
         BRepBuilderEdgeGeometry frontEdge = BRepBuilderEdgeGeometry.Create(frontNurbs);

         IList<XYZ> leftEdgeControlPoints = new List<XYZ> { new XYZ(0, 0, 0), new XYZ(20, 0, 100), new XYZ(40, 0, -100), new XYZ(60, 0, 100), new XYZ(80, 0, 0) };
         Curve leftNurbs = NurbSpline.CreateCurve(4, knotsU, leftEdgeControlPoints, weights);
         BRepBuilderEdgeGeometry leftEdge = BRepBuilderEdgeGeometry.Create(leftNurbs);

         IList<XYZ> rightEdgeControlPoints = new List<XYZ> { new XYZ(0, 80, 0), new XYZ(20, 80, 100), new XYZ(40, 80, -100), new XYZ(60, 80, 100), new XYZ(80, 80, 0) };
         Curve rightNurbs = NurbSpline.CreateCurve(4, knotsU, rightEdgeControlPoints, weights);
         BRepBuilderEdgeGeometry rightEdge = BRepBuilderEdgeGeometry.Create(rightNurbs);

         // Add the geometries to the brepBuilder
         BRepBuilderGeometryId nurbSplineFaceId = brepBuilder.AddFace(nurbsSurface, false);
         BRepBuilderGeometryId loopId = brepBuilder.AddLoop(nurbSplineFaceId);

         BRepBuilderGeometryId backEdgeId = brepBuilder.AddEdge(backEdge);
         BRepBuilderGeometryId frontEdgeId = brepBuilder.AddEdge(frontEdge);
         BRepBuilderGeometryId leftEdgeId = brepBuilder.AddEdge(leftEdge);
         BRepBuilderGeometryId rightEdgeId = brepBuilder.AddEdge(rightEdge);

         // Add each edge to the loop
         brepBuilder.AddCoEdge(loopId, backEdgeId, true);
         brepBuilder.AddCoEdge(loopId, leftEdgeId, false);
         brepBuilder.AddCoEdge(loopId, frontEdgeId, false);
         brepBuilder.AddCoEdge(loopId, rightEdgeId, true);
         brepBuilder.FinishLoop(loopId);
         brepBuilder.FinishFace(nurbSplineFaceId);

         brepBuilder.Finish();
         return brepBuilder;
      }

      private Document _dbdocument = null;

   }
}
