//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.WindowWizard.CS
{
   /// <summary>
   /// The class is used to create solid extrusion 
   /// </summary>
   class CreateExtrusion
   {
      #region Class Memeber Variables
      /// <summary>
      /// store the document
      /// </summary>
      Document m_document;

      /// <summary>
      /// store the application of creation
      /// </summary>
      Autodesk.Revit.Creation.Application m_appCreator;

      /// <summary>
      /// store the FamilyItemFactory of creation
      /// </summary>
      Autodesk.Revit.Creation.FamilyItemFactory m_familyCreator;
      #endregion

      /// <summary>
      /// The constructor of CreateExtrusion
      /// </summary>
      /// <param name="app">the application</param>
      /// <param name="doc">the document</param>
      public CreateExtrusion(Application app, Document doc)
      {
         m_document = doc;
         m_appCreator = app.Create;
         m_familyCreator = doc.FamilyCreate;
      }

      #region Class Implementation
      /// <summary>
      /// The method is used to create a CurveArray with four double parameters and one y coordinate value
      /// </summary>
      /// <param name="left">the left value</param>
      /// <param name="right">the right value</param>
      /// <param name="top">the top value</param>
      /// <param name="bottom">the bottom value</param>
      /// <param name="y_coordinate">the y_coordinate value</param>
      /// <returns>CurveArray</returns>
      public CurveArray CreateRectangle(double left, double right, double top, double bottom, double y_coordinate)
      {
         CurveArray curveArray = m_appCreator.NewCurveArray();
         try
         {
            Autodesk.Revit.DB.XYZ p0 = new Autodesk.Revit.DB.XYZ(left, y_coordinate, top);
            Autodesk.Revit.DB.XYZ p1 = new Autodesk.Revit.DB.XYZ(right, y_coordinate, top);
            Autodesk.Revit.DB.XYZ p2 = new Autodesk.Revit.DB.XYZ(right, y_coordinate, bottom);
            Autodesk.Revit.DB.XYZ p3 = new Autodesk.Revit.DB.XYZ(left, y_coordinate, bottom);
            Line line1 = Line.CreateBound(p0, p1);
            Line line2 = Line.CreateBound(p1, p2);
            Line line3 = Line.CreateBound(p2, p3);
            Line line4 = Line.CreateBound(p3, p0);
            curveArray.Append(line1);
            curveArray.Append(line2);
            curveArray.Append(line3);
            curveArray.Append(line4);
            return curveArray;
         }
         catch (Exception e)
         {
            System.Diagnostics.Debug.WriteLine(e.Message);
            return null;
         }
      }

      /// <summary>
      /// The method is used to create a CurveArray along to an origin CurveArray and an offset value
      /// </summary>
      /// <param name="origin">the original CurveArray</param>
      /// <param name="offset">the offset value</param>
      /// <returns>CurveArray</returns>
      public CurveArray CreateCurveArrayByOffset(CurveArray origin, double offset)
      {
         Line line;
         Line temp;
         int counter = 0;
         CurveArray curveArr = m_appCreator.NewCurveArray();
         Autodesk.Revit.DB.XYZ offsetx = new Autodesk.Revit.DB.XYZ(offset, 0, 0);
         Autodesk.Revit.DB.XYZ offsetz = new Autodesk.Revit.DB.XYZ(0, 0, offset);
         Autodesk.Revit.DB.XYZ p0 = new Autodesk.Revit.DB.XYZ();
         Autodesk.Revit.DB.XYZ p1 = new Autodesk.Revit.DB.XYZ(); ;
         Autodesk.Revit.DB.XYZ p2 = new Autodesk.Revit.DB.XYZ();
         Autodesk.Revit.DB.XYZ p3 = new Autodesk.Revit.DB.XYZ();
         foreach (Curve curve in origin)
         {
            temp = curve as Line;
            if (temp != null)
            {
               if (counter == 0)
               {
                  p0 = temp.GetEndPoint(0).Subtract(offsetz).Subtract(offsetx);
               }
               else if (counter == 1)
               {
                  p1 = temp.GetEndPoint(0).Subtract(offsetz).Add(offsetx);
               }
               else if (counter == 2)
               {
                  p2 = temp.GetEndPoint(0).Add(offsetx).Add(offsetz);
               }
               else
               {
                  p3 = temp.GetEndPoint(0).Subtract(offsetx).Add(offsetz);
               }
            }
            counter++;
         }
         line = Line.CreateBound(p0, p1);
         curveArr.Append(line);
         line = Line.CreateBound(p1, p2);
         curveArr.Append(line);
         line = Line.CreateBound(p2, p3);
         curveArr.Append(line);
         line = Line.CreateBound(p3, p0);
         curveArr.Append(line);
         return curveArr;
      }

      /// <summary>
      /// The method is used to create extrusion using FamilyItemFactory.NewExtrusion()
      /// </summary>
      /// <param name="curveArrArray">the CurveArrArray parameter</param>
      /// <param name="workPlane">the reference plane is used to create SketchPlane</param>
      /// <param name="startOffset">the extrusion's StartOffset property</param>
      /// <param name="endOffset">the extrusion's EndOffset property</param>
      /// <returns>the new extrusion</returns>
      public Extrusion NewExtrusion(CurveArrArray curveArrArray, ReferencePlane workPlane, double startOffset, double endOffset)
      {
         Extrusion rectExtrusion = null;
         try
         {
            SubTransaction subTransaction = new SubTransaction(m_document);
            subTransaction.Start();
            SketchPlane sketch = SketchPlane.Create(m_document, workPlane.GetPlane());
            rectExtrusion = m_familyCreator.NewExtrusion(true, curveArrArray, sketch, Math.Abs(endOffset - startOffset));
            rectExtrusion.StartOffset = startOffset;
            rectExtrusion.EndOffset = endOffset;
            subTransaction.Commit();
            return rectExtrusion;
         }
         catch
         {
            return null;
         }
      }
      #endregion
   }
}
