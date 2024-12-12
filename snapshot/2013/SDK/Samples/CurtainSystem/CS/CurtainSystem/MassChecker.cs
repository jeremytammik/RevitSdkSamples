//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Revit.SDK.Samples.CurtainSystem.CS.Data;

namespace Revit.SDK.Samples.CurtainSystem.CS.CurtainSystem
{
   /// <summary>
   /// check whether the selected element is a mass and whether the mass is kind of parallelepiped
   /// (only mass of parallelepiped supported in this sample)
   /// </summary>
   class MassChecker
   {
      // the document containing all the data used in the sample
      MyDocument m_mydocument;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="mydoc">
      /// the document of the sample
      /// </param>
      public MassChecker(MyDocument mydoc)
      {
         m_mydocument = mydoc;
      }

      /// <summary>
      /// check whether the selection is a parallelepiped mass
      /// </summary>
      public bool CheckSelectedMass()
      {
         // get the selected mass
         FamilyInstance mass = GetSelectedMass();
         // start the sample without making a parallelepiped mass selected
         if (null == mass)
         {
            m_mydocument.FatalErrorMsg = Properties.Resources.MSG_InvalidSelection;
            return false;
         }

         // check whether the mass is parallelepiped
         bool isMassParallelepiped = IsMassParallelepiped(mass);
         if (false == isMassParallelepiped)
         {
            m_mydocument.FatalErrorMsg = Properties.Resources.MSG_InvalidSelection;
            return false;
         }

         return true;
      }

      /// <summary>
      /// check whether the mass is a parallelepiped mass by checking the faces' normals
      /// (if it's a parallelepiped mass, it will have 3 groups of parallel faces)
      /// </summary>
      /// <param name="mass">
      /// the mass to be checked
      /// </param>
      /// <returns>
      /// return true if the mass is parallelepiped; otherwise false
      /// </returns>
      private bool IsMassParallelepiped(FamilyInstance mass)
      {
         FaceArray faces = GetMassFaceArray(mass);

         // a parallelepiped always has 6 faces
         if (null == faces ||
             6 != faces.Size)
         {
            return false;
         }

         bool isFacesParallel = IsFacesParallel(faces);

         // store the face array
         if (true == isFacesParallel)
         {
            m_mydocument.MassFaceArray = faces;
         }

         return isFacesParallel;
      }

      /// <summary>
      /// check whether the faces of the mass are one-one parallel
      /// </summary>
      /// <param name="faces">
      /// the 6 faces of the mass
      /// </param>
      /// <returns>
      /// if the 6 faces are one-one parallel, return true; otherwise false
      /// </returns>
      private bool IsFacesParallel(FaceArray faces)
      {
         // step1: get the normals of the 6 faces
         List<Utility.Vector4> normals = new List<Utility.Vector4>();
         foreach (Face face in faces)
         {
            EdgeArrayArray edgeArrayArray = face.EdgeLoops;
            EdgeArray edges = edgeArrayArray.get_Item(0);

            if (null == edges ||
                2 > edges.Size)
            {
               return false;
            }

            // we use the cross product of 2 non-parallel vectors as the normal
            for (int i = 0; i < edges.Size - 1; i++)
            {
               Edge edgeA = edges.get_Item(i);
               Edge edgeB = edges.get_Item(i + 1);

               // if edgeA & edgeB are parallel, can't compute  the cross product
               bool isLinesParallel = IsLinesParallel(edgeA, edgeB);

               if (true == isLinesParallel)
               {
                  continue;
               }

               Utility.Vector4 vec4 = ComputeCrossProduct(edgeA, edgeB);
               normals.Add(vec4);
               break;
            }
         }

         // step 2: the 6 normals should be one-one parallel pairs
         if (null == normals ||
             6 != normals.Count)
         {
            return false;
         }

         bool[] matchedList = new bool[6];
         for (int i = 0; i < matchedList.Length; i++)
         {
            matchedList[i] = false;
         }

         // check whether the normal has another matched parallel normal
         for (int i = 0; i < matchedList.Length; i++)
         {
            if (true == matchedList[i])
            {
               continue;
            }

            Utility.Vector4 vec4A = normals[i];

            for (int j = 0; j < matchedList.Length; j++)
            {
               if (j == i ||
                   true == matchedList[j])
               {
                  continue;
               }

               Utility.Vector4 vec4B = normals[j];

               if (true == IsLinesParallel(vec4A, vec4B))
               {
                  matchedList[i] = true;
                  matchedList[j] = true;
                  break;
               }
            }
         }

         // step 3: check each of the 6 normals has matched parallel normal
         for (int i = 0; i < matchedList.Length; i++)
         {
            if (false == matchedList[i])
            {
               return false;
            }
         }

         // all the normals have matched parallel normals
         return true;
      }

      /// <summary>
      /// check whether 2 edges are parallel
      /// </summary>
      /// <param name="edgeA">
      /// the edge to be checked
      /// </param>
      /// <param name="edgeB">
      /// the edge to be checked
      /// </param>
      /// <returns>
      /// if they're parallel, return true; otherwise false
      /// </returns>
      private bool IsLinesParallel(Edge edgeA, Edge edgeB)
      {
         List<XYZ> pointsA = edgeA.Tessellate() as List<XYZ>;
         List<XYZ> pointsB = edgeB.Tessellate() as List<XYZ>;
         Autodesk.Revit.DB.XYZ vectorA = pointsA[1] - pointsA[0];
         Autodesk.Revit.DB.XYZ vectorB = pointsB[1] - pointsB[0];
         Utility.Vector4 vec4A = new Utility.Vector4(vectorA);
         Utility.Vector4 vec4B = new Utility.Vector4(vectorB);
         return IsLinesParallel(vec4A, vec4B);
      }

      /// <summary>
      /// check whether 2 vectors are parallel
      /// </summary>
      /// <param name="vec4A">
      /// the vector to be checked
      /// </param>
      /// <param name="vec4B">
      /// the vector to be checked
      /// </param>
      /// <returns>
      /// if they're parallel, return true; otherwise false
      /// </returns>
      private bool IsLinesParallel(Utility.Vector4 vec4A, Utility.Vector4 vec4B)
      {
         // if 2 vectors are parallel, they should be like the following formula:
         // vec4A.X    vec4A.Y    vec4A.Z
         // ------- == ------- == -------
         // vec4B.X    vec4B.Y    vec4B.Z
         // change to multiply, it's 
         // vec4A.X * vec4B.Y == vec4A.Y * vec4B.X &&
         // vec4A.Y * vec4B.Z == vec4A.Z * vec4B.Y
         double aa = vec4A.X * vec4B.Y;
         double bb = vec4A.Y * vec4B.X;
         double cc = vec4A.Y * vec4B.Z;
         double dd = vec4A.Z * vec4B.Y;
         double ee = vec4A.X * vec4B.Z;
         double ff = vec4A.Z * vec4B.X;

         const double tolerance = 0.0001d;

         if (Math.Abs(aa - bb) < tolerance &&
             Math.Abs(cc - dd) < tolerance &&
             Math.Abs(ee - ff) < tolerance)
         {
            return true;
         }

         return false;
      }

      /// <summary>
      /// compute the cross product of 2 edges
      /// </summary>
      /// <param name="edgeA">
      /// the edge for the cross product
      /// </param>
      /// <param name="edgeB">
      /// the edge for the cross product
      /// </param>
      /// <returns>
      /// the cross product of 2 edges
      /// </returns>
      private Utility.Vector4 ComputeCrossProduct(Edge edgeA, Edge edgeB)
      {
         List<XYZ> pointsA = edgeA.Tessellate() as List<XYZ>;
         List<XYZ> pointsB = edgeB.Tessellate() as List<XYZ>;
         Autodesk.Revit.DB.XYZ vectorA = pointsA[1] - pointsA[0];
         Autodesk.Revit.DB.XYZ vectorB = pointsB[1] - pointsB[0];
         Utility.Vector4 vec4A = new Utility.Vector4(vectorA);
         Utility.Vector4 vec4B = new Utility.Vector4(vectorB);
         return Utility.Vector4.CrossProduct(vec4A, vec4B);
      }

      /// <summary>
      /// get the faces of the mass
      /// </summary>
      /// <param name="mass">
      /// the source mass
      /// </param>
      /// <returns>
      /// the faces of the mass
      /// </returns>
      private FaceArray GetMassFaceArray(FamilyInstance mass)
      {
         // Obtain the gemotry information of the mass
         Autodesk.Revit.DB.Options opt = m_mydocument.CommandData.Application.Application.Create.NewGeometryOptions();
         opt.DetailLevel = ViewDetailLevel.Fine;
         opt.ComputeReferences = true;
         Autodesk.Revit.DB.GeometryElement geoElement = null;
         try
         {
            geoElement = mass.get_Geometry(opt);
         }
         catch (System.Exception)
         {
            return null;
         }

         if (null == geoElement)
         {
            return null;
         }

         GeometryObjectArray objectarray = geoElement.Objects;
         foreach (GeometryObject obj in objectarray)
         {
            Solid solid = obj as Solid;

            if (null != solid &&
                null != solid.Faces &&
                0 != solid.Faces.Size)
            {
               return solid.Faces;
            }
         }

         return null;
      }

      /// <summary>
      /// get the selected mass
      /// </summary>
      /// <returns>
      /// return the selected mass; if it's not a mass, return null
      /// </returns>
      private FamilyInstance GetSelectedMass()
      {
         FamilyInstance resultMass = null;

         // check whether a mass was selected before launching this sample
         Selection selection = m_mydocument.UIDocument.Selection;
         if (null == selection ||
             null == selection.Elements ||
             true == selection.Elements.IsEmpty ||
             1 != selection.Elements.Size)
         {
            //m_mydocument.FatalErrorMsg = Properties.Resources.MSG_InvalidSelection;
            return null;
         }

         foreach (Autodesk.Revit.DB.Element selElement in selection.Elements)
         {
            FamilyInstance inst = selElement as FamilyInstance;
            if (null != inst &&
                "Mass" == inst.Category.Name)
            {
               resultMass = inst;
               break;
            }
         }

         // nothing selected or the selected element is not a mass
         if (null == resultMass)
         {
            //m_mydocument.FatalErrorMsg = Properties.Resources.MSG_InvalidSelection;
            return null;
         }

         return resultMass;
      }

   }
}
