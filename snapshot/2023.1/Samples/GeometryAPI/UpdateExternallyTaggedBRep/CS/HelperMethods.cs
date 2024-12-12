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
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.UpdateExternallyTaggedBRep.CS
{
   static class HelperMethods
   {
      /// <summary>
      /// Makes the main part of the CreateBRep external command actions.
      /// See CreateBRep.Execute method summary for the details.
      /// </summary>
      /// <param name="document">A Document that will be used for Transaction and DirectShape creation.</param>
      public static Result executeCreateBRepCommand(Document document)
      {
         // Create the ExternallyTaggedBRep named "Podium".
         ExternallyTaggedBRep taggedBRep = HelperMethods.createExternallyTaggedPodium(40.0, 12.0, 30.0);
         if (null == taggedBRep)
            return Result.Failed;

         using (Autodesk.Revit.DB.Transaction transaction = new Autodesk.Revit.DB.Transaction(document, "CreateExternallyTaggedBRep"))
         {
            transaction.Start();

            // Create the new DirectShape for this open Document and add the created ExternallyTaggedBRep to this DirectShape.
            CreateBRep.CreatedDirectShape = HelperMethods.createDirectShapeWithExternallyTaggedBRep(document, taggedBRep);
            if (null == CreateBRep.CreatedDirectShape)
               return Result.Failed;

            // Retrieve the ExternallyTaggedBRep by its ExternalId from the DirectShape and check that the returned BRep is valid.
            ExternallyTaggedBRep retrievedBRep = CreateBRep.CreatedDirectShape.GetExternallyTaggedGeometry(taggedBRep.ExternalId) as ExternallyTaggedBRep;
            if (null == retrievedBRep)
               return Result.Failed;

            // Retrieve the Face by its ExternalGeometryId from the ExternallyTaggedBRep and check that the returned face is valid.
            // "faceRiser1" is a hardcoded ExternalGeometryId of the one Face in the "Podium" BRep, see Podium.cs file.
            Face retrievedFace = retrievedBRep.GetTaggedGeometry(new ExternalGeometryId("faceRiser1")) as Face;
            if (null == retrievedFace)
               return Result.Failed;

            // Retrieve the Edge by its ExternalGeometryId from the ExternallyTaggedBRep and check that the returned edge is valid.
            // "edgeLeftRiser1" is a hardcoded ExternalGeometryId of the one Edge in the "Podium" BRep, see Podium.cs file.
            Edge retrievedEdge = retrievedBRep.GetTaggedGeometry(new ExternalGeometryId("edgeLeftRiser1")) as Edge;
            if (null == retrievedEdge)
               return Result.Failed;

            transaction.Commit();

            return Result.Succeeded;
         }
      }

      /// <summary>
      /// Creates stairs BRep as ExternallyTaggedBRep.
      /// </summary>
      /// <param name="width">The width of the stairs BRep.</param>
      /// <param name="height">The height of the stairs BRep.</param>
      /// <param name="depth">The depth of the stairs BRep.</param>
      public static ExternallyTaggedBRep createExternallyTaggedPodium(double width, double height, double depth)
      {
         Podium podium = new Podium(width, height, depth);
         return podium.CreateStairs();
      }

      /// <summary>
      /// Creates the new DirectShape and adds the ExternallyTaggedBRep to it.
      /// </summary>
      /// <param name="document">A Document that will be used for the DirectShape creation.</param>
      /// <param name="taggedBRep">An ExternallyTaggedBRep that will be added to the created DirectShape.</param>
      public static DirectShape createDirectShapeWithExternallyTaggedBRep(Document document, ExternallyTaggedBRep taggedBRep)
      {
         DirectShape directShape = DirectShape.CreateElement(document, new ElementId(BuiltInCategory.OST_Stairs));
         if (null == directShape)
            return null;
         directShape.ApplicationId = "TestCreateExternallyTaggedBRep";
         directShape.ApplicationDataId = "ExternallyTaggedBRep";

         directShape.AddExternallyTaggedGeometry(taggedBRep);
         return directShape;
      }
   }
}
