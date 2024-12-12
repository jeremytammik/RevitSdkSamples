//
// (C) Copyright 2003-2020 by Autodesk, Inc.
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

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.UpdateExternallyTaggedBRep.CS
{
   class Podium
   {
      // Dimension data for the various podium parts.
      private double m_podiumWidth;
      private double m_halfPodiumWidth;
      private double m_podiumHeight;
      private double m_halfPodiumHeight;
      private double m_podiumDepth;
      private double m_halfPodiumDepth;
      private double m_stepDepth;
      private double m_halfStepDepth;
      private double m_stepHeight;
      private double m_halfStepHeight;
      private double m_riser1DepthPos;
      private double m_riser2DepthPos;
      private double m_riser3DepthPos;
      private double m_tread1HeightPos;
      private double m_tread2HeightPos;

      public Podium(double width, double height, double depth)
      {
         UpdatePodiumDimensions(width, height, depth);
      }

      void UpdatePodiumDimensions(double width, double height, double depth)
      {
         m_podiumWidth = width;
         m_halfPodiumWidth = m_podiumWidth / 2.0;
         m_podiumHeight = height;
         m_halfPodiumHeight = m_podiumHeight / 2.0;
         m_podiumDepth = depth;
         m_halfPodiumDepth = m_podiumDepth / 2.0;
         m_stepDepth = width / 10.0;
         m_halfStepDepth = m_stepDepth / 2.0;
         m_stepHeight = height / 3.0;
         m_halfStepHeight = m_stepHeight / 2.0;
         m_riser1DepthPos = -m_halfPodiumDepth;
         m_riser2DepthPos = m_riser1DepthPos + m_stepDepth;
         m_riser3DepthPos = m_riser2DepthPos + m_stepDepth;
         m_tread1HeightPos = m_stepHeight;
         m_tread2HeightPos = m_tread1HeightPos * 2;
      }

      public ExternallyTaggedBRep CreateStairs()
      {
         BRepBuilder brepBuilder = new BRepBuilder(BRepType.Solid);
         BRepBuilderPersistentIds persistentIds = new BRepBuilderPersistentIds(brepBuilder);

         // 1. Planes
         Plane riser1 = Plane.CreateByOriginAndBasis(new XYZ(0, m_riser1DepthPos, m_halfStepHeight), new XYZ(1, 0, 0), new XYZ(0, 0, 1)); // XZ plane
         Plane riser2 = Plane.CreateByOriginAndBasis(new XYZ(0, m_riser2DepthPos, m_tread1HeightPos + m_halfStepHeight), new XYZ(1, 0, 0), new XYZ(0, 0, 1)); // XZ plane
         Plane riser3 = Plane.CreateByOriginAndBasis(new XYZ(0, m_riser3DepthPos, m_tread2HeightPos + m_halfStepHeight), new XYZ(1, 0, 0), new XYZ(0, 0, 1)); // XZ plane
         Plane back = Plane.CreateByOriginAndBasis(new XYZ(0, m_halfPodiumDepth, m_halfPodiumHeight), new XYZ(1, 0, 0), new XYZ(0, 0, 1)); // XZ plane
         Plane bottom = Plane.CreateByOriginAndBasis(new XYZ(0, 0, 0), new XYZ(1, 0, 0), new XYZ(0, 1, 0)); // XY plane
         Plane tread1 = Plane.CreateByOriginAndBasis(new XYZ(0, m_riser1DepthPos + m_halfStepDepth, m_tread1HeightPos), new XYZ(1, 0, 0), new XYZ(0, 1, 0)); // XY plane
         Plane tread2 = Plane.CreateByOriginAndBasis(new XYZ(0, m_riser2DepthPos + m_halfStepDepth, m_tread2HeightPos), new XYZ(1, 0, 0), new XYZ(0, 1, 0)); // XY plane
         Plane top = Plane.CreateByOriginAndBasis(new XYZ(0, 0, m_podiumHeight), new XYZ(1, 0, 0), new XYZ(0, 1, 0)); // XY plane
         Plane left = Plane.CreateByOriginAndBasis(new XYZ(-m_halfPodiumWidth, 0, m_halfPodiumHeight), new XYZ(0, 1, 0), new XYZ(0, 0, 1)); // YZ plane
         Plane right = Plane.CreateByOriginAndBasis(new XYZ(m_halfPodiumWidth, 0, m_halfPodiumHeight), new XYZ(0, 1, 0), new XYZ(0, 0, 1)); // YZ plane

         // 2. Faces
         BRepBuilderGeometryId faceRiser1Id = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(riser1, null), false /*bFaceIsReversed*/);
         persistentIds.AddSubTag(new ExternalGeometryId("faceRiser1"), faceRiser1Id);
         BRepBuilderGeometryId faceRiser2Id = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(riser2, null), false /*bFaceIsReversed*/);
         persistentIds.AddSubTag(new ExternalGeometryId("faceRiser2"), faceRiser2Id);
         BRepBuilderGeometryId faceRiser3Id = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(riser3, null), false /*bFaceIsReversed*/);
         persistentIds.AddSubTag(new ExternalGeometryId("faceRiser3"), faceRiser3Id);
         BRepBuilderGeometryId faceBackId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(back, null), true /*bFaceIsReversed*/);
         persistentIds.AddSubTag(new ExternalGeometryId("faceBack"), faceBackId);
         BRepBuilderGeometryId faceBottomId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(bottom, null), true /*bFaceIsReversed*/);
         persistentIds.AddSubTag(new ExternalGeometryId("faceBottom"), faceBottomId);
         BRepBuilderGeometryId faceTread1Id = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(tread1, null), false /*bFaceIsReversed*/);
         persistentIds.AddSubTag(new ExternalGeometryId("faceTread1"), faceTread1Id);
         BRepBuilderGeometryId faceTread2Id = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(tread2, null), false /*bFaceIsReversed*/);
         persistentIds.AddSubTag(new ExternalGeometryId("faceTread2"), faceTread2Id);
         BRepBuilderGeometryId faceTopId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(top, null), false /*bFaceIsReversed*/);
         persistentIds.AddSubTag(new ExternalGeometryId("faceTop"), faceTopId);
         BRepBuilderGeometryId faceLeftId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(left, null), true /*bFaceIsReversed*/);
         persistentIds.AddSubTag(new ExternalGeometryId("faceLeft"), faceLeftId);
         BRepBuilderGeometryId faceRightId = brepBuilder.AddFace(BRepBuilderSurfaceGeometry.Create(right, null), false /*bFaceIsReversed*/);
         persistentIds.AddSubTag(new ExternalGeometryId("faceRight"), faceRightId);

         // 3. Edge geometry
         // Bottom horizontal edges
         BRepBuilderEdgeGeometry edgeBottomFront = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_riser1DepthPos, 0), new XYZ(m_halfPodiumWidth, m_riser1DepthPos, 0));
         BRepBuilderEdgeGeometry edgeBottomRight = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_riser1DepthPos, 0), new XYZ(m_halfPodiumWidth, m_halfPodiumDepth, 0));
         BRepBuilderEdgeGeometry edgeBottomBack = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_halfPodiumDepth, 0), new XYZ(-m_halfPodiumWidth, m_halfPodiumDepth, 0));
         BRepBuilderEdgeGeometry edgeBottomLeft = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_halfPodiumDepth, 0), new XYZ(-m_halfPodiumWidth, m_riser1DepthPos, 0));

         // Tread 1 horizontal edges
         BRepBuilderEdgeGeometry edgeTread1Front = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_riser1DepthPos, m_stepHeight), new XYZ(m_halfPodiumWidth, m_riser1DepthPos, m_stepHeight));
         BRepBuilderEdgeGeometry edgeTread1Right = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_riser1DepthPos, m_stepHeight), new XYZ(m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight));
         BRepBuilderEdgeGeometry edgeTread1Back = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight), new XYZ(-m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight));
         BRepBuilderEdgeGeometry edgeTread1Left = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight), new XYZ(-m_halfPodiumWidth, m_riser1DepthPos, m_stepHeight));

         // Tread 2 horizontal edges
         BRepBuilderEdgeGeometry edgeTread2Front = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight * 2), new XYZ(m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight * 2));
         BRepBuilderEdgeGeometry edgeTread2Right = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight * 2), new XYZ(m_halfPodiumWidth, m_riser3DepthPos, m_stepHeight * 2));
         BRepBuilderEdgeGeometry edgeTread2Back = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_riser3DepthPos, m_stepHeight * 2), new XYZ(-m_halfPodiumWidth, m_riser3DepthPos, m_stepHeight * 2));
         BRepBuilderEdgeGeometry edgeTread2Left = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_riser3DepthPos, m_stepHeight * 2), new XYZ(-m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight * 2));

         // Top horizontal edges
         BRepBuilderEdgeGeometry edgeTopFront = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_riser3DepthPos, m_podiumHeight), new XYZ(m_halfPodiumWidth, m_riser3DepthPos, m_podiumHeight));
         BRepBuilderEdgeGeometry edgeTopRight = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_riser3DepthPos, m_podiumHeight), new XYZ(m_halfPodiumWidth, m_halfPodiumDepth, m_podiumHeight));
         BRepBuilderEdgeGeometry edgeTopBack = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_halfPodiumDepth, m_podiumHeight), new XYZ(-m_halfPodiumWidth, m_halfPodiumDepth, m_podiumHeight));
         BRepBuilderEdgeGeometry edgeTopLeft = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_halfPodiumDepth, m_podiumHeight), new XYZ(-m_halfPodiumWidth, m_riser3DepthPos, m_podiumHeight));

         // Left vertical edges
         BRepBuilderEdgeGeometry edgeLeftRiser1 = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_riser1DepthPos, 0), new XYZ(-m_halfPodiumWidth, m_riser1DepthPos, m_stepHeight));
         BRepBuilderEdgeGeometry edgeLeftRiser2 = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight), new XYZ(-m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight * 2));
         BRepBuilderEdgeGeometry edgeLeftRiser3 = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_riser3DepthPos, m_stepHeight * 2), new XYZ(-m_halfPodiumWidth, m_riser3DepthPos, m_podiumHeight));
         BRepBuilderEdgeGeometry edgeLeftBack = BRepBuilderEdgeGeometry.Create(new XYZ(-m_halfPodiumWidth, m_halfPodiumDepth, 0), new XYZ(-m_halfPodiumWidth, m_halfPodiumDepth, m_podiumHeight));

         // Right vertical edges
         BRepBuilderEdgeGeometry edgeRightRiser1 = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_riser1DepthPos, 0), new XYZ(m_halfPodiumWidth, m_riser1DepthPos, m_stepHeight));
         BRepBuilderEdgeGeometry edgeRightRiser2 = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight), new XYZ(m_halfPodiumWidth, m_riser2DepthPos, m_stepHeight * 2));
         BRepBuilderEdgeGeometry edgeRightRiser3 = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_riser3DepthPos, m_stepHeight * 2), new XYZ(m_halfPodiumWidth, m_riser3DepthPos, m_podiumHeight));
         BRepBuilderEdgeGeometry edgeRightBack = BRepBuilderEdgeGeometry.Create(new XYZ(m_halfPodiumWidth, m_halfPodiumDepth, 0), new XYZ(m_halfPodiumWidth, m_halfPodiumDepth, m_podiumHeight));

         // 4. Edges
         BRepBuilderGeometryId edgeBottomFrontId = brepBuilder.AddEdge(edgeBottomFront);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeBottom"), edgeBottomFrontId);
         BRepBuilderGeometryId edgeBottomRightId = brepBuilder.AddEdge(edgeBottomRight);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeBottomRight"), edgeBottomRightId);
         BRepBuilderGeometryId edgeBottomBackId = brepBuilder.AddEdge(edgeBottomBack);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeBottomBack"), edgeBottomBackId);
         BRepBuilderGeometryId edgeBottomLeftId = brepBuilder.AddEdge(edgeBottomLeft);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeBottomLeft"), edgeBottomLeftId);
         BRepBuilderGeometryId edgeTread1FrontId = brepBuilder.AddEdge(edgeTread1Front);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTread1Front"), edgeTread1FrontId);
         BRepBuilderGeometryId edgeTread1RightId = brepBuilder.AddEdge(edgeTread1Right);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTread1Right"), edgeTread1RightId);
         BRepBuilderGeometryId edgeTread1BackId = brepBuilder.AddEdge(edgeTread1Back);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTread1Back"), edgeTread1BackId);
         BRepBuilderGeometryId edgeTread1LeftId = brepBuilder.AddEdge(edgeTread1Left);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTread1Left"), edgeTread1LeftId);
         BRepBuilderGeometryId edgeTread2FrontId = brepBuilder.AddEdge(edgeTread2Front);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTread2Front"), edgeTread2FrontId);
         BRepBuilderGeometryId edgeTread2RightId = brepBuilder.AddEdge(edgeTread2Right);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTread2Right"), edgeTread2RightId);
         BRepBuilderGeometryId edgeTread2BackId = brepBuilder.AddEdge(edgeTread2Back);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTread2Back"), edgeTread2BackId);
         BRepBuilderGeometryId edgeTread2LeftId = brepBuilder.AddEdge(edgeTread2Left);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTread2Left"), edgeTread2LeftId);
         BRepBuilderGeometryId edgeTopFrontId = brepBuilder.AddEdge(edgeTopFront);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTopFront"), edgeTopFrontId);
         BRepBuilderGeometryId edgeTopRightId = brepBuilder.AddEdge(edgeTopRight);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTopRight"), edgeTopRightId);
         BRepBuilderGeometryId edgeTopBackId = brepBuilder.AddEdge(edgeTopBack);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTopBack"), edgeTopBackId);
         BRepBuilderGeometryId edgeTopLeftId = brepBuilder.AddEdge(edgeTopLeft);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeTopLeft"), edgeTopLeftId);
         BRepBuilderGeometryId edgeLeftRiser1Id = brepBuilder.AddEdge(edgeLeftRiser1);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeLeftRiser1"), edgeLeftRiser1Id);
         BRepBuilderGeometryId edgeLeftRiser2Id = brepBuilder.AddEdge(edgeLeftRiser2);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeLeftRiser2"), edgeLeftRiser2Id);
         BRepBuilderGeometryId edgeLeftRiser3Id = brepBuilder.AddEdge(edgeLeftRiser3);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeLeftRiser3"), edgeLeftRiser3Id);
         BRepBuilderGeometryId edgeLeftBackId = brepBuilder.AddEdge(edgeLeftBack);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeLeftBack"), edgeLeftBackId);
         BRepBuilderGeometryId edgeRightRiser1Id = brepBuilder.AddEdge(edgeRightRiser1);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeRightRiser1"), edgeRightRiser1Id);
         BRepBuilderGeometryId edgeRightRiser2Id = brepBuilder.AddEdge(edgeRightRiser2);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeRightRiser2"), edgeRightRiser2Id);
         BRepBuilderGeometryId edgeRightRiser3Id = brepBuilder.AddEdge(edgeRightRiser3);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeRightRiser3"), edgeRightRiser3Id);
         BRepBuilderGeometryId edgeRightBackId = brepBuilder.AddEdge(edgeRightBack);
         persistentIds.AddSubTag(new ExternalGeometryId("edgeRightBack"), edgeRightBackId);

         // 5. Loops
         BRepBuilderGeometryId loopBottomId = brepBuilder.AddLoop(faceBottomId);
         BRepBuilderGeometryId loopTread1Id = brepBuilder.AddLoop(faceTread1Id);
         BRepBuilderGeometryId loopTread2Id = brepBuilder.AddLoop(faceTread2Id);
         BRepBuilderGeometryId loopTopId = brepBuilder.AddLoop(faceTopId);
         BRepBuilderGeometryId loopRiser1Id = brepBuilder.AddLoop(faceRiser1Id);
         BRepBuilderGeometryId loopRiser2Id = brepBuilder.AddLoop(faceRiser2Id);
         BRepBuilderGeometryId loopRiser3Id = brepBuilder.AddLoop(faceRiser3Id);
         BRepBuilderGeometryId loopBackId = brepBuilder.AddLoop(faceBackId);
         BRepBuilderGeometryId loopLeftId = brepBuilder.AddLoop(faceLeftId);
         BRepBuilderGeometryId loopRightId = brepBuilder.AddLoop(faceRightId);

         // 6. Co-edges
         // Bottom face
         BRepBuilderGeometryId coEdgeId = brepBuilder.AddCoEdge(loopBottomId, edgeBottomFrontId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopBottomId, edgeBottomLeftId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopBottomId, edgeBottomBackId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopBottomId, edgeBottomRightId, true /*bCoEdgeIsReversed*/);
         brepBuilder.FinishLoop(loopBottomId);
         brepBuilder.FinishFace(faceBottomId);

         // Tread 1 face
         brepBuilder.AddCoEdge(loopTread1Id, edgeTread1FrontId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopTread1Id, edgeTread1RightId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopTread1Id, edgeTread1BackId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopTread1Id, edgeTread1LeftId, false /*bCoEdgeIsReversed*/);
         brepBuilder.FinishLoop(loopTread1Id);
         brepBuilder.FinishFace(faceTread1Id);

         // Tread 2 face
         brepBuilder.AddCoEdge(loopTread2Id, edgeTread2FrontId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopTread2Id, edgeTread2RightId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopTread2Id, edgeTread2BackId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopTread2Id, edgeTread2LeftId, false /*bCoEdgeIsReversed*/);
         brepBuilder.FinishLoop(loopTread2Id);
         brepBuilder.FinishFace(faceTread2Id);

         // Top face
         brepBuilder.AddCoEdge(loopTopId, edgeTopFrontId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopTopId, edgeTopRightId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopTopId, edgeTopBackId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopTopId, edgeTopLeftId, false /*bCoEdgeIsReversed*/);
         brepBuilder.FinishLoop(loopTopId);
         brepBuilder.FinishFace(faceTopId);

         // Riser1 face
         brepBuilder.AddCoEdge(loopRiser1Id, edgeBottomFrontId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRiser1Id, edgeRightRiser1Id, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRiser1Id, edgeTread1FrontId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRiser1Id, edgeLeftRiser1Id, true /*bCoEdgeIsReversed*/);
         brepBuilder.FinishLoop(loopRiser1Id);
         brepBuilder.FinishFace(faceRiser1Id);

         // Riser2 face
         brepBuilder.AddCoEdge(loopRiser2Id, edgeTread1BackId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRiser2Id, edgeRightRiser2Id, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRiser2Id, edgeTread2FrontId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRiser2Id, edgeLeftRiser2Id, true /*bCoEdgeIsReversed*/);
         brepBuilder.FinishLoop(loopRiser2Id);
         brepBuilder.FinishFace(faceRiser2Id);

         // Riser3 face
         brepBuilder.AddCoEdge(loopRiser3Id, edgeTread2BackId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRiser3Id, edgeRightRiser3Id, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRiser3Id, edgeTopFrontId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRiser3Id, edgeLeftRiser3Id, true /*bCoEdgeIsReversed*/);
         brepBuilder.FinishLoop(loopRiser3Id);
         brepBuilder.FinishFace(faceRiser3Id);

         // Back face
         brepBuilder.AddCoEdge(loopBackId, edgeBottomBackId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopBackId, edgeLeftBackId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopBackId, edgeTopBackId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopBackId, edgeRightBackId, true /*bCoEdgeIsReversed*/);
         brepBuilder.FinishLoop(loopBackId);
         brepBuilder.FinishFace(faceBackId);

         // Right face
         brepBuilder.AddCoEdge(loopRightId, edgeBottomRightId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRightId, edgeRightBackId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRightId, edgeTopRightId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRightId, edgeRightRiser3Id, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRightId, edgeTread2RightId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRightId, edgeRightRiser2Id, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRightId, edgeTread1RightId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopRightId, edgeRightRiser1Id, true /*bCoEdgeIsReversed*/);
         brepBuilder.FinishLoop(loopRightId);
         brepBuilder.FinishFace(faceRightId);

         // Left face
         brepBuilder.AddCoEdge(loopLeftId, edgeBottomLeftId, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopLeftId, edgeLeftRiser1Id, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopLeftId, edgeTread1LeftId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopLeftId, edgeLeftRiser2Id, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopLeftId, edgeTread2LeftId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopLeftId, edgeLeftRiser3Id, false /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopLeftId, edgeTopLeftId, true /*bCoEdgeIsReversed*/);
         brepBuilder.AddCoEdge(loopLeftId, edgeLeftBackId, true /*bCoEdgeIsReversed*/);
         brepBuilder.FinishLoop(loopLeftId);
         brepBuilder.FinishFace(faceLeftId);

         brepBuilder.Finish();

         return brepBuilder.GetResult(Podium.ExternalId, persistentIds);
      }

      /// <summary>
      /// ExternalGeometryId of the created ExternallyTaggedBRep by Podium class.
      /// </summary>
      public static readonly ExternalGeometryId ExternalId = new ExternalGeometryId("externalID");
   }
}
