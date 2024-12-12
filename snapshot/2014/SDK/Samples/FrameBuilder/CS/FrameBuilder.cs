//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.FrameBuilder.CS
{
   using System;
   using System.Collections.Generic;
   using System.Text;
   using System.Diagnostics;

   using Autodesk.Revit;
   using Autodesk.Revit.DB;
   using Autodesk.Revit.UI;
   using Autodesk.Revit.DB.Structure;
   using Autodesk.Revit.ApplicationServices;

   using ModelElement = Autodesk.Revit.DB.Element;

   /// <summary>
   /// create columns, beams and braces to create framing
   /// </summary>
   public class FrameBuilder
   {
      FrameData m_data;        // necessary data to create frame
      Autodesk.Revit.Creation.Document m_docCreator;        // buffer of API object
      Autodesk.Revit.Creation.Application m_appCreator;    // buffer of API object

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="data">data necessary to initialize object</param>
      public FrameBuilder(FrameData data)
      {
         // initialize members
         if (null == data)
         {
            throw new ArgumentNullException("data",
                "constructor FrameBuilder(FrameData data)'s parameter shouldn't be null ");
         }
         m_data = data;

         m_appCreator = data.CommandData.Application.Application.Create;
         m_docCreator = data.CommandData.Application.ActiveUIDocument.Document.Create;
      }

      /// <summary>
      /// create framing according to FramingData
      /// </summary>
      /// <returns>columns, beams and braces</returns>
      public void CreateFraming()
      {
         Transaction t = new Transaction(m_data.CommandData.Application.ActiveUIDocument.Document, Guid.NewGuid().GetHashCode().ToString());
         t.Start();
         m_data.UpdateLevels();
         List<FamilyInstance> frameElems = new List<FamilyInstance>();
         Autodesk.Revit.DB.UV[,] matrixUV = CreateMatrix(m_data.XNumber, m_data.YNumber, m_data.Distance);

         // iterate levels from lower one to higher one by one according to FloorNumber
         for (int ii = 0; ii < m_data.FloorNumber; ii++)
         {
            Level baseLevel = m_data.Levels.Values[ii];
            Level topLevel = m_data.Levels.Values[ii + 1];

            int matrixXSize = matrixUV.GetLength(0);    //length of matrix's x range
            int matrixYSize = matrixUV.GetLength(1);    //length of matrix's y range

            // insert columns in an array format according to the calculated matrix
            foreach (Autodesk.Revit.DB.UV point2D in matrixUV)
            {
               frameElems.Add(NewColumn(point2D, baseLevel, topLevel));
            }

            // insert beams between the tops of each adjacent column in the X and Y direction
            for (int j = 0; j < matrixYSize; j++)
            {
               for (int i = 0; i < matrixXSize; i++)
               {
                  //create beams in x direction
                  if (i != (matrixXSize - 1))
                  {
                     frameElems.Add(NewBeam(matrixUV[i, j], matrixUV[i + 1, j], topLevel));
                  }
                  //create beams in y direction
                  if (j != (matrixYSize - 1))
                  {
                     frameElems.Add(NewBeam(matrixUV[i, j], matrixUV[i, j + 1], topLevel));
                  }
               }
            }

            // insert braces between the mid point of each column 
            // and the mid point of each adjoining beam
            for (int j = 0; j < matrixYSize; j++)
            {
               for (int i = 0; i < matrixXSize; i++)
               {
                  //create braces in x direction
                  if (i != (matrixXSize - 1))
                  {
                     frameElems.AddRange(
                         NewBraces(matrixUV[i, j], matrixUV[i + 1, j], baseLevel, topLevel));
                  }
                  //create braces in y direction
                  if (j != (matrixYSize - 1))
                  {
                     frameElems.AddRange(
                         NewBraces(matrixUV[i, j], matrixUV[i, j + 1], baseLevel, topLevel));
                  }
               }
            }
         }

         MoveRotateFrame(frameElems);
         t.Commit();
      }

      /// <summary>
      /// constructor without parameter is forbidden
      /// </summary>
      private FrameBuilder()
      {
      }

      /// <summary>
      /// create a 2D matrix of coordinates to form an array format
      /// </summary>
      /// <param name="xNumber">number of Columns in the X direction</param>
      /// <param name="yNumber">number of Columns in the Y direction</param>
      /// <param name="distance">distance between columns</param>
      private static Autodesk.Revit.DB.UV[,] CreateMatrix(int xNumber, int yNumber, double distance)
      {
         Autodesk.Revit.DB.UV[,] result = new Autodesk.Revit.DB.UV[xNumber, yNumber];

         for (int i = 0; i < xNumber; i++)
         {
            for (int j = 0; j < yNumber; j++)
            {
               result[i, j] = new Autodesk.Revit.DB.UV(i * distance, j * distance);
            }
         }
         return result;
      }

      /// <summary>
      /// create column of certain type in given position
      /// </summary>
      /// <param name="point2D">2D coordinate of the column</param>
      /// <param name="columnType">specified type of the column</param>
      /// <param name="baseLevel">base level of the column</param>
      /// <param name="topLevel">top level of the colunm</param>
      private FamilyInstance NewColumn(Autodesk.Revit.DB.UV point2D, Level baseLevel, Level topLevel)
      {
         //create column of specified type with certain level and start point 
         Autodesk.Revit.DB.XYZ point = new Autodesk.Revit.DB.XYZ(point2D.U, point2D.V, 0);

         FamilyInstance column =
             m_docCreator.NewFamilyInstance(point, m_data.ColumnSymbol, baseLevel, StructuralType.Column);

         //set baselevel & toplevel of the column            
         SetParameter(column, BuiltInParameter.FAMILY_TOP_LEVEL_PARAM, topLevel.Id);
         SetParameter(column, BuiltInParameter.FAMILY_BASE_LEVEL_PARAM, baseLevel.Id);
         SetParameter(column, BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM, 0.0);
         SetParameter(column, BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM, 0.0);
         return column;
      }

      /// <summary>
      /// create beam of certain type in given position
      /// </summary>
      /// <param name="point2D1">first point of the location line in 2D</param>
      /// <param name="point2D2">second point of the location line in 2D</param>
      /// <param name="baseLevel">base level of the beam</param>
      /// <param name="topLevel">top level of the beam</param>
      /// <returns>nothing</returns>
      private FamilyInstance NewBeam(Autodesk.Revit.DB.UV point2D1, Autodesk.Revit.DB.UV point2D2, Level topLevel)
      {
         // calculate the start point and end point of Beam's location line in 3D
         double height = topLevel.Elevation;
         Autodesk.Revit.DB.XYZ startPoint = new Autodesk.Revit.DB.XYZ(point2D1.U, point2D1.V, height);
         Autodesk.Revit.DB.XYZ endPoint = new Autodesk.Revit.DB.XYZ(point2D2.U, point2D2.V, height);
         // create Beam and set its location
         Line baseLine = Line.CreateBound(startPoint, endPoint);
         FamilyInstance beam =
             m_docCreator.NewFamilyInstance(baseLine, m_data.BeamSymbol, topLevel, StructuralType.Beam);
         return beam;
      }

      /// <summary>
      /// create 2 braces between the mid point of 2 column and the mid point of adjoining beam
      /// </summary>
      /// <param name="point2D1">first point of the location line in 2D</param>
      /// <param name="point2D2">second point of the location line in 2D</param>
      /// <param name="baseLevel">the base level of the brace</param>
      /// <param name="topLevel">the top level of the brace</param>
      private List<FamilyInstance> NewBraces(Autodesk.Revit.DB.UV point2D1, Autodesk.Revit.DB.UV point2D2, Level baseLevel, Level topLevel)
      {
         // calculate the start point and end point of the location lines of two braces
         double topHeight = topLevel.Elevation;
         double baseHeight = baseLevel.Elevation;
         double middleElevation = (topHeight + baseHeight) / 2;
         Autodesk.Revit.DB.XYZ startPoint = new Autodesk.Revit.DB.XYZ(point2D1.U, point2D1.V, middleElevation);
         Autodesk.Revit.DB.XYZ endPoint = new Autodesk.Revit.DB.XYZ(point2D2.U, point2D2.V, middleElevation);
         Autodesk.Revit.DB.XYZ middlePoint = new Autodesk.Revit.DB.XYZ((point2D1.U + point2D2.U) / 2, (point2D1.V + point2D2.V) / 2, topHeight);

         Autodesk.Revit.DB.ElementId levelId = topLevel.Id;
         // create two brace; then set their location line and reference level
         Line firstBaseLine = Line.CreateBound(startPoint, middlePoint);
         FamilyInstance firstBrace =
             m_docCreator.NewFamilyInstance(firstBaseLine, m_data.BraceSymbol, topLevel, StructuralType.Brace);

         Line secondBaseLine = Line.CreateBound(endPoint, middlePoint);
         FamilyInstance secondBrace =
             m_docCreator.NewFamilyInstance(secondBaseLine, m_data.BraceSymbol, topLevel, StructuralType.Brace);
         List<FamilyInstance> result = new List<FamilyInstance>();
         result.Add(firstBrace);
         result.Add(secondBrace);
         return result;
      }

      /// <summary>
      /// set parameter whose storage type is Autodesk.Revit.DB.ElementId 
      /// </summary>
      /// <param name="elem">Element has parameter</param>
      /// <param name="builtInPara">BuiltInParameter to find parameter</param>
      /// <param name="value">value to set</param>
      /// <returns>is successful</returns>
      private bool SetParameter(ModelElement elem,
          BuiltInParameter builtInPara, Autodesk.Revit.DB.ElementId value)
      {
         Parameter para = elem.get_Parameter(builtInPara);
         if (null != para && para.StorageType == StorageType.ElementId && !para.IsReadOnly)
         {
            var result = para.Set(value);
            return result;
         }
         return false;
      }

      /// <summary>
      /// set parameter whose storage type is double
      /// </summary>
      /// <param name="elem">Element has parameter</param>
      /// <param name="builtInPara">BuiltInParameter to find parameter</param>
      /// <param name="value">value to set</param>
      /// <returns>is successful</returns>
      private bool SetParameter(ModelElement elem,
          BuiltInParameter builtInPara, double value)
      {
         Parameter para = elem.get_Parameter(builtInPara);
         if (null != para && para.StorageType == StorageType.Double && !para.IsReadOnly)
         {
            var result = para.Set(value);
            return result;
         }
         return false;
      }


      /// <summary>
      /// move and rotate the Frame
      /// </summary>
      /// <param name="frameElems">columns, beams and braces included in frame</param>
      private void MoveRotateFrame(List<FamilyInstance> frameElems)
      {
         Application app = m_data.CommandData.Application.Application;
         Document doc = m_data.CommandData.Application.ActiveUIDocument.Document;
         foreach (FamilyInstance elem in frameElems)
         {
            MoveElement(doc, elem, m_data.FrameOrigin);
            RotateElement(m_data.CommandData.Application, elem, m_data.FrameOrigin, m_data.FrameOriginAngle);
         }
      }

      /// <summary>
      /// move an element in horizontal plane
      /// </summary>
      /// <param name="elem">element to be moved</param>
      /// <param name="translation2D">the 2D vector by which the element is to be moved</param>
      /// <returns>is successful</returns>
      private void MoveElement(Document doc, Autodesk.Revit.DB.Element elem, Autodesk.Revit.DB.UV translation2D)
      {
         Autodesk.Revit.DB.XYZ translation3D = new Autodesk.Revit.DB.XYZ(translation2D.U, translation2D.V, 0.0);
         ElementTransformUtils.MoveElement(doc, elem.Id, translation3D);
      }

      /// <summary>
      /// rotate an element a specified number of degrees 
      /// around a given center in horizontal plane
      /// </summary>
      /// <param name="elem">element to be rotated</param>
      /// <param name="center">the center of rotation</param>
      /// <param name="angle">the number of degrees, in radians, 
      /// by which the element is to be rotated around the specified axis</param>
      /// <returns>is successful</returns>
      private void RotateElement(UIApplication app, Autodesk.Revit.DB.Element elem, Autodesk.Revit.DB.UV center, double angle)
      {
         Autodesk.Revit.DB.XYZ axisPnt1 = new Autodesk.Revit.DB.XYZ(center.U, center.V, 0.0);
         Autodesk.Revit.DB.XYZ axisPnt2 = new Autodesk.Revit.DB.XYZ(center.U, center.V, 1.0);
         Line axis = Line.CreateBound(axisPnt1, axisPnt2);
         //axis.
         ElementTransformUtils.RotateElement(elem.Document, elem.Id, axis, angle);
      }
   }
}
