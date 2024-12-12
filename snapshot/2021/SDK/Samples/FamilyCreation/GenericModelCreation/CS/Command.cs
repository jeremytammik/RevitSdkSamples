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


using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.GenericModelCreation.CS
{
   /// <summary>
   /// A class inherits IExternalCommand interface.
   /// This class show how to create Generic Model Family by Revit API.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class Command : IExternalCommand
   {
      #region Class Memeber Variables
      // Application of Revit
      private Autodesk.Revit.ApplicationServices.Application m_revit;
      // the document to create generic model family
      private Autodesk.Revit.DB.Document m_familyDocument;
      // FamilyItemFactory used to create family
      private Autodesk.Revit.Creation.FamilyItemFactory m_creationFamily = null;
      // Count error numbers
      private int m_errCount = 0;
      // Error information
      private string m_errorInfo = "";
      #endregion

      #region Class Interface Implementation
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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
                                             ref string message,
                                             ElementSet elements)
      {
         try
         {
            m_revit = commandData.Application.Application;
            m_familyDocument = commandData.Application.ActiveUIDocument.Document;
            // create new family document if active document is not a family document
            if (!m_familyDocument.IsFamilyDocument)
            {
               m_familyDocument = m_revit.NewFamilyDocument("Generic Model.rft");
               if (null == m_familyDocument)
               {
                  message = "Cannot open family document";
                  return Autodesk.Revit.UI.Result.Failed;
               }
            }
            m_creationFamily = m_familyDocument.FamilyCreate;
            // create generic model family in the document
            CreateGenericModel();
            if (0 == m_errCount)
            {
               return Autodesk.Revit.UI.Result.Succeeded;
            }
            else
            {
               message = m_errorInfo;
               return Autodesk.Revit.UI.Result.Failed;
            }
         }
         catch (Exception e)
         {
            message = e.ToString();
            return Autodesk.Revit.UI.Result.Failed;
         }

      }
      #endregion

      #region Class Implementation

      /// <summary>
      /// Examples for form creation in generic model families.
      /// Create extrusion, blend, revolution, sweep, swept blend
      /// </summary>
      public void CreateGenericModel()
      {
         // use transaction if the family document is not active document
         Transaction transaction = new Transaction(m_familyDocument, "CreateGenericModel");
         transaction.Start();
         CreateExtrusion();
         CreateBlend();
         CreateRevolution();
         CreateSweep();
         CreateSweptBlend();
         transaction.Commit();
         return;
      }

      /// <summary>
      /// Create one rectangular extrusion
      /// </summary>
      private void CreateExtrusion()
      {
         try
         {
            #region Create rectangle profile
            CurveArrArray curveArrArray = new CurveArrArray();
            CurveArray curveArray1 = new CurveArray();

            Autodesk.Revit.DB.XYZ normal = Autodesk.Revit.DB.XYZ.BasisZ;
            SketchPlane sketchPlane = CreateSketchPlane(normal, Autodesk.Revit.DB.XYZ.Zero);

            // create one rectangular extrusion
            Autodesk.Revit.DB.XYZ p0 = Autodesk.Revit.DB.XYZ.Zero;
            Autodesk.Revit.DB.XYZ p1 = new Autodesk.Revit.DB.XYZ(10, 0, 0);
            Autodesk.Revit.DB.XYZ p2 = new Autodesk.Revit.DB.XYZ(10, 10, 0);
            Autodesk.Revit.DB.XYZ p3 = new Autodesk.Revit.DB.XYZ(0, 10, 0);
            Line line1 = Line.CreateBound(p0, p1);
            Line line2 = Line.CreateBound(p1, p2);
            Line line3 = Line.CreateBound(p2, p3);
            Line line4 = Line.CreateBound(p3, p0);
            curveArray1.Append(line1);
            curveArray1.Append(line2);
            curveArray1.Append(line3);
            curveArray1.Append(line4);

            curveArrArray.Append(curveArray1);
            #endregion
            // here create rectangular extrusion
            Extrusion rectExtrusion = m_creationFamily.NewExtrusion(true, curveArrArray, sketchPlane, 10);
            // move to proper place
            Autodesk.Revit.DB.XYZ transPoint1 = new Autodesk.Revit.DB.XYZ(-16, 0, 0);
            ElementTransformUtils.MoveElement(m_familyDocument, rectExtrusion.Id, transPoint1);
         }
         catch (Exception e)
         {
            m_errCount++;
            m_errorInfo += "Unexpected exceptions occur in CreateExtrusion: " + e.ToString() + "\r\n";
         }
      }

      /// <summary>
      /// Create one blend
      /// </summary>
      private void CreateBlend()
      {
         try
         {
            #region Create top and base profiles
            CurveArray topProfile = new CurveArray();
            CurveArray baseProfile = new CurveArray();

            Autodesk.Revit.DB.XYZ normal = Autodesk.Revit.DB.XYZ.BasisZ;
            SketchPlane sketchPlane = CreateSketchPlane(normal, Autodesk.Revit.DB.XYZ.Zero);

            // create one blend
            Autodesk.Revit.DB.XYZ p00 = Autodesk.Revit.DB.XYZ.Zero;
            Autodesk.Revit.DB.XYZ p01 = new Autodesk.Revit.DB.XYZ(10, 0, 0);
            Autodesk.Revit.DB.XYZ p02 = new Autodesk.Revit.DB.XYZ(10, 10, 0);
            Autodesk.Revit.DB.XYZ p03 = new Autodesk.Revit.DB.XYZ(0, 10, 0);
            Line line01 = Line.CreateBound(p00, p01);
            Line line02 = Line.CreateBound(p01, p02);
            Line line03 = Line.CreateBound(p02, p03);
            Line line04 = Line.CreateBound(p03, p00);

            baseProfile.Append(line01);
            baseProfile.Append(line02);
            baseProfile.Append(line03);
            baseProfile.Append(line04);

            Autodesk.Revit.DB.XYZ p10 = new Autodesk.Revit.DB.XYZ(5, 2, 10);
            Autodesk.Revit.DB.XYZ p11 = new Autodesk.Revit.DB.XYZ(8, 5, 10);
            Autodesk.Revit.DB.XYZ p12 = new Autodesk.Revit.DB.XYZ(5, 8, 10);
            Autodesk.Revit.DB.XYZ p13 = new Autodesk.Revit.DB.XYZ(2, 5, 10);
            Line line11 = Line.CreateBound(p10, p11);
            Line line12 = Line.CreateBound(p11, p12);
            Line line13 = Line.CreateBound(p12, p13);
            Line line14 = Line.CreateBound(p13, p10);

            topProfile.Append(line11);
            topProfile.Append(line12);
            topProfile.Append(line13);
            topProfile.Append(line14);
            #endregion
            // here create one blend
            Blend blend = m_creationFamily.NewBlend(true, topProfile, baseProfile, sketchPlane);
            // move to proper place
            Autodesk.Revit.DB.XYZ transPoint1 = new Autodesk.Revit.DB.XYZ(0, 11, 0);
            ElementTransformUtils.MoveElement(m_familyDocument, blend.Id, transPoint1);
         }
         catch (Exception e)
         {
            m_errCount++;
            m_errorInfo += "Unexpected exceptions occur in CreateBlend: " + e.ToString() + "\r\n";
         }
      }

      /// <summary>
      /// Create one rectangular profile revolution
      /// </summary>
      private void CreateRevolution()
      {
         try
         {
            #region Create rectangular profile
            CurveArrArray curveArrArray = new CurveArrArray();
            CurveArray curveArray = new CurveArray();

            Autodesk.Revit.DB.XYZ normal = Autodesk.Revit.DB.XYZ.BasisZ;
            SketchPlane sketchPlane = CreateSketchPlane(normal, Autodesk.Revit.DB.XYZ.Zero);

            // create one rectangular profile revolution
            Autodesk.Revit.DB.XYZ p0 = Autodesk.Revit.DB.XYZ.Zero;
            Autodesk.Revit.DB.XYZ p1 = new Autodesk.Revit.DB.XYZ(10, 0, 0);
            Autodesk.Revit.DB.XYZ p2 = new Autodesk.Revit.DB.XYZ(10, 10, 0);
            Autodesk.Revit.DB.XYZ p3 = new Autodesk.Revit.DB.XYZ(0, 10, 0);
            Line line1 = Line.CreateBound(p0, p1);
            Line line2 = Line.CreateBound(p1, p2);
            Line line3 = Line.CreateBound(p2, p3);
            Line line4 = Line.CreateBound(p3, p0);

            Autodesk.Revit.DB.XYZ pp = new Autodesk.Revit.DB.XYZ(1, -1, 0);
            Line axis1 = Line.CreateBound(Autodesk.Revit.DB.XYZ.Zero, pp);
            curveArray.Append(line1);
            curveArray.Append(line2);
            curveArray.Append(line3);
            curveArray.Append(line4);

            curveArrArray.Append(curveArray);
            #endregion
            // here create rectangular profile revolution
            Revolution revolution1 = m_creationFamily.NewRevolution(true, curveArrArray, sketchPlane, axis1, -Math.PI, 0);
            // move to proper place
            Autodesk.Revit.DB.XYZ transPoint1 = new Autodesk.Revit.DB.XYZ(0, 32, 0);
            ElementTransformUtils.MoveElement(m_familyDocument, revolution1.Id, transPoint1);
         }
         catch (Exception e)
         {
            m_errCount++;
            m_errorInfo += "Unexpected exceptions occur in CreateRevolution: " + e.ToString() + "\r\n";
         }
      }

      /// <summary>
      /// Create one sweep
      /// </summary>
      private void CreateSweep()
      {
         try
         {
            #region Create rectangular profile and path curve
            CurveArrArray arrarr = new CurveArrArray();
            CurveArray arr = new CurveArray();

            Autodesk.Revit.DB.XYZ normal = Autodesk.Revit.DB.XYZ.BasisZ;
            SketchPlane sketchPlane = CreateSketchPlane(normal, Autodesk.Revit.DB.XYZ.Zero);

            Autodesk.Revit.DB.XYZ pnt1 = new Autodesk.Revit.DB.XYZ(0, 0, 0);
            Autodesk.Revit.DB.XYZ pnt2 = new Autodesk.Revit.DB.XYZ(2, 0, 0);
            Autodesk.Revit.DB.XYZ pnt3 = new Autodesk.Revit.DB.XYZ(1, 1, 0);
            arr.Append(Arc.Create(pnt2, 1.0d, 0.0d, 180.0d, Autodesk.Revit.DB.XYZ.BasisX, Autodesk.Revit.DB.XYZ.BasisY));
            arr.Append(Arc.Create(pnt1, pnt3, pnt2));
            arrarr.Append(arr);
            SweepProfile profile = m_revit.Create.NewCurveLoopsProfile(arrarr);

            Autodesk.Revit.DB.XYZ pnt4 = new Autodesk.Revit.DB.XYZ(10, 0, 0);
            Autodesk.Revit.DB.XYZ pnt5 = new Autodesk.Revit.DB.XYZ(0, 10, 0);
            Curve curve = Line.CreateBound(pnt4, pnt5);

            CurveArray curves = new CurveArray();
            curves.Append(curve);
            #endregion
            // here create one sweep with two arcs formed the profile
            Sweep sweep1 = m_creationFamily.NewSweep(true, curves, sketchPlane, profile, 0, ProfilePlaneLocation.Start);
            // move to proper place
            Autodesk.Revit.DB.XYZ transPoint1 = new Autodesk.Revit.DB.XYZ(11, 0, 0);
            ElementTransformUtils.MoveElement(m_familyDocument, sweep1.Id, transPoint1);
         }
         catch (Exception e)
         {
            m_errCount++;
            m_errorInfo += "Unexpected exceptions occur in CreateSweep: " + e.ToString() + "\r\n";
         }
      }

      /// <summary>
      /// Create one SweptBlend
      /// </summary>
      private void CreateSweptBlend()
      {
         try
         {
            #region Create top and bottom profiles and path curve
            Autodesk.Revit.DB.XYZ pnt1 = new Autodesk.Revit.DB.XYZ(0, 0, 0);
            Autodesk.Revit.DB.XYZ pnt2 = new Autodesk.Revit.DB.XYZ(1, 0, 0);
            Autodesk.Revit.DB.XYZ pnt3 = new Autodesk.Revit.DB.XYZ(1, 1, 0);
            Autodesk.Revit.DB.XYZ pnt4 = new Autodesk.Revit.DB.XYZ(0, 1, 0);
            Autodesk.Revit.DB.XYZ pnt5 = new Autodesk.Revit.DB.XYZ(0, 0, 1);

            CurveArrArray arrarr1 = new CurveArrArray();
            CurveArray arr1 = new CurveArray();
            arr1.Append(Line.CreateBound(pnt1, pnt2));
            arr1.Append(Line.CreateBound(pnt2, pnt3));
            arr1.Append(Line.CreateBound(pnt3, pnt4));
            arr1.Append(Line.CreateBound(pnt4, pnt1));
            arrarr1.Append(arr1);

            Autodesk.Revit.DB.XYZ pnt6 = new Autodesk.Revit.DB.XYZ(0.5, 0, 0);
            Autodesk.Revit.DB.XYZ pnt7 = new Autodesk.Revit.DB.XYZ(1, 0.5, 0);
            Autodesk.Revit.DB.XYZ pnt8 = new Autodesk.Revit.DB.XYZ(0.5, 1, 0);
            Autodesk.Revit.DB.XYZ pnt9 = new Autodesk.Revit.DB.XYZ(0, 0.5, 0);
            CurveArrArray arrarr2 = new CurveArrArray();
            CurveArray arr2 = new CurveArray();
            arr2.Append(Line.CreateBound(pnt6, pnt7));
            arr2.Append(Line.CreateBound(pnt7, pnt8));
            arr2.Append(Line.CreateBound(pnt8, pnt9));
            arr2.Append(Line.CreateBound(pnt9, pnt6));
            arrarr2.Append(arr2);

            SweepProfile bottomProfile = m_revit.Create.NewCurveLoopsProfile(arrarr1);
            SweepProfile topProfile = m_revit.Create.NewCurveLoopsProfile(arrarr2);

            Autodesk.Revit.DB.XYZ pnt10 = new Autodesk.Revit.DB.XYZ(5, 0, 0);
            Autodesk.Revit.DB.XYZ pnt11 = new Autodesk.Revit.DB.XYZ(0, 20, 0);
            Curve curve = Line.CreateBound(pnt10, pnt11);

            Autodesk.Revit.DB.XYZ normal = Autodesk.Revit.DB.XYZ.BasisZ;
            SketchPlane sketchPlane = CreateSketchPlane(normal, Autodesk.Revit.DB.XYZ.Zero);
            #endregion
            // here create one swept blend
            SweptBlend newSweptBlend1 = m_creationFamily.NewSweptBlend(true, curve, sketchPlane, bottomProfile, topProfile);
            // move to proper place
            Autodesk.Revit.DB.XYZ transPoint1 = new Autodesk.Revit.DB.XYZ(11, 32, 0);
            ElementTransformUtils.MoveElement(m_familyDocument, newSweptBlend1.Id, transPoint1);
         }
         catch (Exception e)
         {
            m_errCount++;
            m_errorInfo += "Unexpected exceptions occur in CreateSweptBlend: " + e.ToString() + "\r\n";
         }
      }


      /// <summary>
      /// Get element by its id
      /// </summary>
      private T GetElement<T>(int eid) where T : Autodesk.Revit.DB.Element
      {
         Autodesk.Revit.DB.ElementId elementId = new ElementId(eid);
         return m_familyDocument.GetElement(elementId) as T;
      }

      /// <summary>
      /// Create sketch plane for generic model profile
      /// </summary>
      /// <param name="normal">plane normal</param>
      /// <param name="origin">origin point</param>
      /// <returns></returns>
      internal SketchPlane CreateSketchPlane(Autodesk.Revit.DB.XYZ normal, Autodesk.Revit.DB.XYZ origin)
      {
         // First create a Geometry.Plane which need in NewSketchPlane() method
         Plane geometryPlane = Plane.CreateByNormalAndOrigin(normal, origin);
         if (null == geometryPlane)  // assert the creation is successful
         {
            throw new Exception("Create the geometry plane failed.");
         }
         // Then create a sketch plane using the Geometry.Plane
         SketchPlane plane = SketchPlane.Create(m_familyDocument, geometryPlane);
         // throw exception if creation failed
         if (null == plane)
         {
            throw new Exception("Create the sketch plane failed.");
         }
         return plane;
      }

      #endregion

   }
}
