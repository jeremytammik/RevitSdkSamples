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
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.NewForm.CS
{
   /// <summary>
   /// A class inherits IExternalCommand interface.
   /// This class show how to create extrusion form by Revit API.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class MakeExtrusionForm : IExternalCommand
   {
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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         ExternalCommandData cdata = commandData;
         Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
         app = commandData.Application.Application;
         Document doc = commandData.Application.ActiveUIDocument.Document;

         Transaction transaction = new Transaction(doc, "MakeExtrusionForm");
         transaction.Start();

         // Create one profile
         ReferenceArray ref_ar = new ReferenceArray();

         Autodesk.Revit.DB.XYZ ptA = new Autodesk.Revit.DB.XYZ(10, 10, 0);
         Autodesk.Revit.DB.XYZ ptB = new Autodesk.Revit.DB.XYZ(90, 10, 0);
         ModelCurve modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         ptA = new Autodesk.Revit.DB.XYZ(90, 10, 0);
         ptB = new Autodesk.Revit.DB.XYZ(10, 90, 0);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         ptA = new Autodesk.Revit.DB.XYZ(10, 90, 0);
         ptB = new Autodesk.Revit.DB.XYZ(10, 10, 0);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         // The extrusion form direction
         Autodesk.Revit.DB.XYZ direction = new Autodesk.Revit.DB.XYZ(0, 0, 50);

         Autodesk.Revit.DB.Form form = doc.FamilyCreate.NewExtrusionForm(true, ref_ar, direction);

         transaction.Commit();

         return Autodesk.Revit.UI.Result.Succeeded;
      }
      #endregion
   }

   /// <summary>
   /// A class inherits IExternalCommand interface.
   /// This class show how to create cap form by Revit API.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class MakeCapForm : IExternalCommand
   {
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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         ExternalCommandData cdata = commandData;
         Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
         app = commandData.Application.Application;
         Document doc = commandData.Application.ActiveUIDocument.Document;

         Transaction transaction = new Transaction(doc, "MakeCapForm");
         transaction.Start();

         // Create one profile
         ReferenceArray ref_ar = new ReferenceArray();

         Autodesk.Revit.DB.XYZ ptA = new Autodesk.Revit.DB.XYZ(10, 10, 0);
         Autodesk.Revit.DB.XYZ ptB = new Autodesk.Revit.DB.XYZ(100, 10, 0);
         Line line = Line.CreateBound(ptA, ptB);
         ModelCurve modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         ptA = new Autodesk.Revit.DB.XYZ(100, 10, 0);
         ptB = new Autodesk.Revit.DB.XYZ(50, 50, 0);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         ptA = new Autodesk.Revit.DB.XYZ(50, 50, 0);
         ptB = new Autodesk.Revit.DB.XYZ(10, 10, 0);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         Autodesk.Revit.DB.Form form = doc.FamilyCreate.NewFormByCap(true, ref_ar);

         transaction.Commit();

         return Autodesk.Revit.UI.Result.Succeeded;
      }
      #endregion
   }

   /// <summary>
   /// A class inherits IExternalCommand interface.
   /// This class show how to create revolve form by Revit API.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class MakeRevolveForm : IExternalCommand
   {
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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         ExternalCommandData cdata = commandData;
         Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
         app = commandData.Application.Application;
         Document doc = commandData.Application.ActiveUIDocument.Document;

         Transaction transaction = new Transaction(doc, "MakeRevolveForm");
         transaction.Start();

         // Create one profile
         ReferenceArray ref_ar = new ReferenceArray();
         Autodesk.Revit.DB.XYZ norm = Autodesk.Revit.DB.XYZ.BasisZ;

         Autodesk.Revit.DB.XYZ ptA = new Autodesk.Revit.DB.XYZ(0, 0, 10);
         Autodesk.Revit.DB.XYZ ptB = new Autodesk.Revit.DB.XYZ(100, 0, 10);
         ModelCurve modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB, norm);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         ptA = new Autodesk.Revit.DB.XYZ(100, 0, 10);
         ptB = new Autodesk.Revit.DB.XYZ(100, 100, 10);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB, norm);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         ptA = new Autodesk.Revit.DB.XYZ(100, 100, 10);
         ptB = new Autodesk.Revit.DB.XYZ(0, 0, 10);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB, norm);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         // Create axis for revolve form
         ptA = new Autodesk.Revit.DB.XYZ(-5, 0, 10);
         ptB = new Autodesk.Revit.DB.XYZ(-5, 10, 10);
         ModelCurve axis = FormUtils.MakeLine(commandData.Application, ptA, ptB, norm);
         axis.ChangeToReferenceLine();

         Autodesk.Revit.DB.FormArray form = doc.FamilyCreate.NewRevolveForms(true, ref_ar, axis.GeometryCurve.Reference, 0, Math.PI / 4);

         transaction.Commit();

         return Autodesk.Revit.UI.Result.Succeeded;
      }
      #endregion
   }

   /// <summary>
   /// A class inherits IExternalCommand interface.
   /// This class show how to create swept blend form by Revit API.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class MakeSweptBlendForm : IExternalCommand
   {
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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         ExternalCommandData cdata = commandData;
         Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
         app = commandData.Application.Application;
         Document doc = commandData.Application.ActiveUIDocument.Document;

         Transaction transaction = new Transaction(doc, "MakeSweptBlendForm");
         transaction.Start();

         // Create first profile
         ReferenceArray ref_ar = new ReferenceArray();
         Autodesk.Revit.DB.XYZ ptA = new Autodesk.Revit.DB.XYZ(10, 10, 0);
         Autodesk.Revit.DB.XYZ ptB = new Autodesk.Revit.DB.XYZ(50, 10, 0);
         ModelCurve modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         ptA = new Autodesk.Revit.DB.XYZ(50, 10, 0);
         ptB = new Autodesk.Revit.DB.XYZ(10, 50, 0);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         ptA = new Autodesk.Revit.DB.XYZ(10, 50, 0);
         ptB = new Autodesk.Revit.DB.XYZ(10, 10, 0);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);

         // Create second profile
         ReferenceArray ref_ar2 = new ReferenceArray();
         ptA = new Autodesk.Revit.DB.XYZ(10, 10, 90);
         ptB = new Autodesk.Revit.DB.XYZ(80, 10, 90);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar2.Append(modelcurve.GeometryCurve.Reference);

         ptA = new Autodesk.Revit.DB.XYZ(80, 10, 90);
         ptB = new Autodesk.Revit.DB.XYZ(10, 50, 90);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar2.Append(modelcurve.GeometryCurve.Reference);

         ptA = new Autodesk.Revit.DB.XYZ(10, 50, 90);
         ptB = new Autodesk.Revit.DB.XYZ(10, 10, 90);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         ref_ar2.Append(modelcurve.GeometryCurve.Reference);

         // Add profiles
         ReferenceArrayArray profiles = new ReferenceArrayArray();
         profiles.Append(ref_ar);
         profiles.Append(ref_ar2);

         // Create path for swept blend form
         ReferenceArray path = new ReferenceArray();
         ptA = new Autodesk.Revit.DB.XYZ(10, 10, 0);
         ptB = new Autodesk.Revit.DB.XYZ(10, 10, 90);
         modelcurve = FormUtils.MakeLine(commandData.Application, ptA, ptB);
         path.Append(modelcurve.GeometryCurve.Reference);

         Autodesk.Revit.DB.Form form = doc.FamilyCreate.NewSweptBlendForm(true, path, profiles);

         transaction.Commit();

         return Autodesk.Revit.UI.Result.Succeeded;
      }
      #endregion
   }

   /// <summary>
   /// A class inherits IExternalCommand interface.
   /// This class show how to create loft form by Revit API.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class MakeLoftForm : IExternalCommand
   {
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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         ExternalCommandData cdata = commandData;
         Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
         app = commandData.Application.Application;
         Document doc = commandData.Application.ActiveUIDocument.Document;

         Transaction transaction = new Transaction(doc, "MakeLoftForm");
         transaction.Start();

         // Create profiles array
         ReferenceArrayArray ref_ar_ar = new ReferenceArrayArray();

         // Create first profile
         ReferenceArray ref_ar = new ReferenceArray();

         int y = 100;
         int x = 50;
         Autodesk.Revit.DB.XYZ ptA = new Autodesk.Revit.DB.XYZ(-x, y, 0);
         Autodesk.Revit.DB.XYZ ptB = new Autodesk.Revit.DB.XYZ(x, y, 0);
         Autodesk.Revit.DB.XYZ ptC = new Autodesk.Revit.DB.XYZ(0, y + 10, 10);
         ModelCurve modelcurve = FormUtils.MakeArc(commandData.Application, ptA, ptB, ptC);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);
         ref_ar_ar.Append(ref_ar);


         // Create second profile
         ref_ar = new ReferenceArray();

         y = 40;
         ptA = new Autodesk.Revit.DB.XYZ(-x, y, 5);
         ptB = new Autodesk.Revit.DB.XYZ(x, y, 5);
         ptC = new Autodesk.Revit.DB.XYZ(0, y, 25);
         modelcurve = FormUtils.MakeArc(commandData.Application, ptA, ptB, ptC);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);
         ref_ar_ar.Append(ref_ar);

         // Create third profile
         ref_ar = new ReferenceArray();

         y = -20;
         ptA = new Autodesk.Revit.DB.XYZ(-x, y, 0);
         ptB = new Autodesk.Revit.DB.XYZ(x, y, 0);
         ptC = new Autodesk.Revit.DB.XYZ(0, y, 15);
         modelcurve = FormUtils.MakeArc(commandData.Application, ptA, ptB, ptC);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);
         ref_ar_ar.Append(ref_ar);

         // Create fourth profile
         ref_ar = new ReferenceArray();

         y = -60;
         ptA = new Autodesk.Revit.DB.XYZ(-x, y, 0);
         ptB = new Autodesk.Revit.DB.XYZ(x, y, 0);
         ptC = new Autodesk.Revit.DB.XYZ(0, y + 10, 20);
         modelcurve = FormUtils.MakeArc(commandData.Application, ptA, ptB, ptC);
         ref_ar.Append(modelcurve.GeometryCurve.Reference);
         ref_ar_ar.Append(ref_ar);
         ref_ar = new ReferenceArray();
         ref_ar_ar.Append(ref_ar);

         Autodesk.Revit.DB.Form form = doc.FamilyCreate.NewLoftForm(true, ref_ar_ar);

         transaction.Commit();

         return Autodesk.Revit.UI.Result.Succeeded;
      }
      #endregion
   }

   /// <summary>
   /// This class is utility class for form creation.
   /// </summary>
   public class FormUtils
   {
      #region Class Implementation
      /// <summary>
      /// Create arc element by three points
      /// </summary>
      /// <param name="app">revit application</param>
      /// <param name="ptA">point a</param>
      /// <param name="ptB">point b</param>
      /// <param name="ptC">point c</param>
      /// <returns></returns>
      public static ModelCurve MakeArc(UIApplication app, Autodesk.Revit.DB.XYZ ptA, Autodesk.Revit.DB.XYZ ptB, Autodesk.Revit.DB.XYZ ptC)
      {
         Document doc = app.ActiveUIDocument.Document;
         Arc arc = Arc.Create(ptA, ptB, ptC);
         // Create three lines and a plane by the points
         Line line1 = Line.CreateBound(ptA, ptB);
         Line line2 = Line.CreateBound(ptB, ptC);
         Line line3 = Line.CreateBound(ptC, ptA);
         CurveLoop ca = new CurveLoop();
         ca.Append(line1);
         ca.Append(line2);
         ca.Append(line3);

            Plane plane = ca.GetPlane();// app.Application.Create.NewPlane(ca);
         SketchPlane skplane = SketchPlane.Create(doc, plane);
         // Create arc here
         ModelCurve modelcurve = doc.FamilyCreate.NewModelCurve(arc, skplane);
         return modelcurve;
      }

      /// <summary>
      /// Create line element
      /// </summary>
      /// <param name="app">revit application</param>
      /// <param name="ptA">start point</param>
      /// <param name="ptB">end point</param>
      /// <returns></returns>
      public static ModelCurve MakeLine(UIApplication app, Autodesk.Revit.DB.XYZ ptA, Autodesk.Revit.DB.XYZ ptB)
      {
         Document doc = app.ActiveUIDocument.Document;
         // Create plane by the points
         Line line = Line.CreateBound(ptA, ptB);
         Autodesk.Revit.DB.XYZ norm = ptA.CrossProduct(ptB);
         if (norm.GetLength() == 0) norm = Autodesk.Revit.DB.XYZ.BasisZ;
         Plane plane = Plane.CreateByNormalAndOrigin(norm, ptB);
         SketchPlane skplane = SketchPlane.Create(doc, plane);
         // Create line here
         ModelCurve modelcurve = doc.FamilyCreate.NewModelCurve(line, skplane);
         return modelcurve;
      }

      /// <summary>
      /// Create line element
      /// </summary>
      /// <param name="app">revit application</param>
      /// <param name="ptA">start point</param>
      /// <param name="ptB">end point</param>
      /// <returns></returns>
      public static ModelCurve MakeLine(UIApplication app, Autodesk.Revit.DB.XYZ ptA, Autodesk.Revit.DB.XYZ ptB, Autodesk.Revit.DB.XYZ norm)
      {
         Document doc = app.ActiveUIDocument.Document;
         // Create plane by the points
         Line line = Line.CreateBound(ptA, ptB);
         Plane plane = Plane.CreateByNormalAndOrigin(norm, ptB);
         SketchPlane skplane = SketchPlane.Create(doc, plane);
         // Create line here
         ModelCurve modelcurve = doc.FamilyCreate.NewModelCurve(line, skplane);
         return modelcurve;
      }
      #endregion
   }
}
