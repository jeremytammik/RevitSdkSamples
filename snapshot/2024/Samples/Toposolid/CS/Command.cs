//
// (C) Copyright 2003-2022 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.Toposolid.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ToposolidCreation : IExternalCommand
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
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public virtual Result Execute(ExternalCommandData commandData
         , ref string message, ElementSet elements)
      {
         try
         {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            ElementId typeId = new FilteredElementCollector(doc).OfClass(typeof(ToposolidType)).OfType<ToposolidType>().FirstOrDefault()?.Id;
            if (typeId == null)
            {
               TaskDialog.Show("Error", "Can not find a valid ToposolidType");
               return Result.Failed;
            }
            ElementId levelId = new FilteredElementCollector(doc).OfClass(typeof(Level)).OfType<Level>().FirstOrDefault()?.Id;
            if (levelId == null)
            {
               TaskDialog.Show("Error", "Can not find a valid Level");
               return Result.Failed;
            }

            XYZ pt1 = XYZ.Zero;
            XYZ pt2 = new XYZ(100, 0, 0);
            XYZ pt3 = new XYZ(100, 100, 0);
            XYZ pt4 = new XYZ(0, 100, 0);
            XYZ pt5 = new XYZ(20, 50, 20);
            XYZ pt6 = new XYZ(50, 150, 20);
            List<XYZ> points = new List<XYZ> { pt1, pt2, pt3, pt4, pt5, pt6 };
            Line l1 = Line.CreateBound(pt1, pt2);
            Line l2 = Line.CreateBound(pt2, pt3);
            Line l3 = Line.CreateBound(pt3, pt4);
            Line l4 = Line.CreateBound(pt4, pt1);
            CurveLoop profile = CurveLoop.Create(new List<Curve> { l1, l2, l3, l4 });

            using (Transaction transaction = new Transaction(doc, "create"))
            {
               transaction.Start();
               //Toposolid topo = Toposolid.Create(doc, new List<CurveLoop> { m_Profile}, typeId, levelId);
               //Toposolid topo = Toposolid.Create(doc, m_Points, typeId, levelId);
               Autodesk.Revit.DB.Toposolid topo = Autodesk.Revit.DB.Toposolid.Create(doc, new List<CurveLoop> { profile }, points, typeId, levelId);
               Autodesk.Revit.DB.Toposolid subTopo = topo.CreateSubDivision(doc, new List<CurveLoop> { CurveLoop.CreateViaOffset(profile, -20, XYZ.BasisZ) });
               transaction.Commit();
            }

            return Result.Succeeded;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }
   }

   /// <summary>
   /// Create toposolid from dwg
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ToposolidFromDWG : IExternalCommand
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
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         var uidoc = commandData.Application.ActiveUIDocument;
         var doc = uidoc.Document;
         var sel = uidoc.Selection;
         ElementId typeId = new FilteredElementCollector(doc).OfClass(typeof(ToposolidType)).OfType<ToposolidType>().FirstOrDefault()?.Id;
         if (typeId == null)
         {
            TaskDialog.Show("Error", "Can not find a valid ToposolidType");
            return Result.Failed;
         }
         ElementId levelId = new FilteredElementCollector(doc).OfClass(typeof(Level)).OfType<Level>().FirstOrDefault()?.Id;
         if (levelId == null)
         {
            TaskDialog.Show("Error", "Can not find a valid Level");
            return Result.Failed;
         }
         List<XYZ> ptList = new List<XYZ>();
         Element element = doc.GetElement(sel.PickObject(ObjectType.Element, new ImportInstanceFilter(), "pick an imported dwg file").ElementId);

         //List<Curve> curves = sel.PickObjects(ObjectType.Element, new ModelCurveFilter()).Select(e => (doc.GetElement(e.ElementId) as ModelCurve).GeometryCurve).ToList();
         //CurveLoop cl = CurveLoop.Create(curves);
         List<GeometryObject> objects = element.get_Geometry(new Options()).ToList();
         foreach (GeometryObject gObject in objects)
         {
            GeometryInstance gInstance = gObject as GeometryInstance;
            if (gInstance != null)
            {
               GeometryElement ge = gInstance.GetSymbolGeometry();
               var glist = ge.ToList();
               foreach (GeometryObject obj in glist)
               {
                  if (obj is PolyLine polyLine)
                  {
                     ptList.AddRange(polyLine.GetCoordinates());
                  }
                  else if (obj is Line line)
                  {
                     ptList.Add(line.GetEndPoint(0));
                     ptList.Add(line.GetEndPoint(1));
                  }
               }
            }
         }
         double xMin = ptList.Min(pt => pt.Z);
         List<XYZ> offsetPtList = new List<XYZ>();
         ptList.ForEach(pt => offsetPtList.Add(pt - new XYZ(0, 0, xMin)));
         using (Transaction transaction = new Transaction(doc, "create"))
         {
            transaction.Start();
            Autodesk.Revit.DB.Toposolid topo = Autodesk.Revit.DB.Toposolid.Create(doc, offsetPtList, typeId, levelId);
            //Autodesk.Revit.DB.Toposolid topo = Autodesk.Revit.DB.Toposolid.Create(doc, new List<CurveLoop> { cl }, offsetPtList, typeId, levelId);
            topo.get_Parameter(BuiltInParameter.TOPOSOLID_HEIGHTABOVELEVEL_PARAM).Set(xMin);
            transaction.Commit();
         }
         return Result.Succeeded;
      }
   }


   /// <summary>
   /// Add contour setting items to toposolid type
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ContourSettingCreation : IExternalCommand
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
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         var doc = commandData.Application.ActiveUIDocument.Document;
         ToposolidType topoType = new FilteredElementCollector(doc).OfClass(typeof(ToposolidType)).OfType<ToposolidType>().FirstOrDefault();
         if (topoType == null)
         {
            TaskDialog.Show("Error", "Can not find a valid ToposolidType");
            return Result.Failed;
         }
         ContourSetting contourSetting = topoType.GetContourSetting();
         using (Transaction trans = new Transaction(doc, "contour"))
         {
            trans.Start();
            ContourSettingItem itemRange = contourSetting.AddContourRange(1.0, 9.0, 2.0, new ElementId(BuiltInCategory.OST_ToposolidContours));
            ContourSettingItem item1 = contourSetting.AddSingleContour(10, new ElementId(BuiltInCategory.OST_ToposolidSecondaryContours));
            ContourSettingItem item2 = contourSetting.AddSingleContour(11.5, new ElementId(BuiltInCategory.OST_ToposolidSplitLines));
            ContourSettingItem item3 = contourSetting.AddSingleContour(13, new ElementId(BuiltInCategory.OST_ToposolidSplitLines));
            trans.Commit();
         }
         return Result.Succeeded;
      }
   }

   /// <summary>
   /// Modify the current contoursetting of the toposolid type
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ContourSettingModification : IExternalCommand
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
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         var doc = commandData.Application.ActiveUIDocument.Document;
         ToposolidType topoType = new FilteredElementCollector(doc).OfClass(typeof(ToposolidType)).OfType<ToposolidType>().FirstOrDefault();
         if (topoType == null)
         {
            TaskDialog.Show("Error", "Can not find a valid ToposolidType");
            return Result.Failed;
         }
         ContourSetting contourSetting = topoType.GetContourSetting();
         List<ContourSettingItem> items = contourSetting.GetContourSettingItems().ToList();
         if (items.Count != 4)
         {
            TaskDialog.Show("Error", "Not expected contour setting items count");
            return Result.Failed;
         }
         using (Transaction trans = new Transaction(doc, "contour"))
         {
            trans.Start();
            contourSetting.DisableItem(items[0]);
            //contourSetting.EnableItem(items[0]);
            //contourSetting.RemoveItem(items[1]);
            trans.Commit();
         }
         return Result.Succeeded;
      }
   }

   /// <summary>
   /// Create a toposolid from an existing topography surface
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ToposolidFromSurface : IExternalCommand
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
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         var uidoc = commandData.Application.ActiveUIDocument;
         var doc = uidoc.Document;
         var sel = uidoc.Selection;

         ElementId typeId = new FilteredElementCollector(doc).OfClass(typeof(ToposolidType)).OfType<ToposolidType>().FirstOrDefault()?.Id;
         if (typeId == null)
         {
            TaskDialog.Show("Error", "Can not find a valid ToposolidType");
            return Result.Failed;
         }
         ElementId levelId = new FilteredElementCollector(doc).OfClass(typeof(Level)).OfType<Level>().FirstOrDefault()?.Id;
         if (levelId == null)
         {
            TaskDialog.Show("Error", "Can not find a valid Level");
            return Result.Failed;
         }

         TopographySurface surface = doc.GetElement(sel.PickObject(ObjectType.Element, new TopographySurfaceFilter(), "pick a topography surface")) as TopographySurface;

         using (Transaction transaction = new Transaction(doc, "create"))
         {
            transaction.Start();
            Autodesk.Revit.DB.Toposolid topo = Autodesk.Revit.DB.Toposolid.CreateFromTopographySurface(doc, surface.Id, typeId, levelId);
            transaction.Commit();
            var ids = topo.GetSubDivisionIds().ToList();
            //TaskDialog.Show("test", ids.Count.ToString());
         }
         return Result.Succeeded;
      }
   }

   /// <summary>
   /// Set the SSE point visibility
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class SSEPointVisibility : IExternalCommand
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
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         var uidoc = commandData.Application.ActiveUIDocument;
         var doc = uidoc.Document;
         using (Transaction transaction = new Transaction(doc, "modify"))
         {
            transaction.Start();
            SSEPointVisibilitySettings.SetVisibility(doc, new ElementId(BuiltInCategory.OST_Toposolid), false);
            transaction.Commit();
         }
         bool visible = SSEPointVisibilitySettings.GetVisibility(doc, new ElementId(BuiltInCategory.OST_Toposolid));
         //TaskDialog.Show("test", visible.ToString());
         return Result.Succeeded;
      }
   }

   /// <summary>
   /// Split a toposolid by selected model curves
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class SplitToposolid : IExternalCommand
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
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         var uidoc = commandData.Application.ActiveUIDocument;
         var doc = uidoc.Document;
         var sel = uidoc.Selection;

         Autodesk.Revit.DB.Toposolid topo = doc.GetElement(sel.PickObject(ObjectType.Element, new ToposolidFilter())) as Autodesk.Revit.DB.Toposolid;

         List<Curve> curveList = new List<Curve>();
         sel.PickObjects(ObjectType.Element, new ModelCurveFilter()).ToList().ForEach(x => curveList.Add((doc.GetElement(x) as ModelCurve).GeometryCurve));
         CurveLoop cl = CurveLoop.Create(curveList);

         using (Transaction transaction = new Transaction(doc, "split"))
         {
            transaction.Start();
            topo.Split(new List<CurveLoop> { cl });
            transaction.Commit();
         }

         return Result.Succeeded;
      }
   }

   /// <summary>
   /// Simplify a toposolid by reducing its inner vertices.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class SimplifyToposolid : IExternalCommand
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
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         var uidoc = commandData.Application.ActiveUIDocument;
         var doc = uidoc.Document;
         var sel = uidoc.Selection;

         Autodesk.Revit.DB.Toposolid topo = doc.GetElement(sel.PickObject(ObjectType.Element, new ToposolidFilter())) as Autodesk.Revit.DB.Toposolid;

         using (Transaction transaction = new Transaction(doc, "simplify"))
         {
            transaction.Start();
            topo.Simplify(0.6);
            transaction.Commit();
         }

         return Result.Succeeded;
      }
   }

   /// <summary>
   /// ImportInstanceFilter
   /// </summary>
   public class ImportInstanceFilter : ISelectionFilter
   {
      /// <summary>
      /// Interface implementation
      /// </summary>
      /// <param name="elem"></param>
      /// <returns></returns>
      public bool AllowElement(Element elem)
      {
         return elem is ImportInstance;
      }

      /// <summary>
      /// Interface implementation
      /// </summary>
      /// <param name="reference"></param>
      /// <param name="position"></param>
      /// <returns></returns>
      public bool AllowReference(Reference reference, XYZ position)
      {
         return false;
      }
   }


   /// <summary>
   /// TopographySurfaceFilter
   /// </summary>
   public class TopographySurfaceFilter : ISelectionFilter
   {
      /// <summary>
      /// Interface implementation
      /// </summary>
      /// <param name="elem"></param>
      /// <returns></returns>
      public bool AllowElement(Element elem)
      {
         return elem is TopographySurface;
      }

      /// <summary>
      /// Interface implementation
      /// </summary>
      /// <param name="reference"></param>
      /// <param name="position"></param>
      /// <returns></returns>
      public bool AllowReference(Reference reference, XYZ position)
      {
         return false;
      }
   }

   /// <summary>
   /// ModelCurveFilter
   /// </summary>
   public class ModelCurveFilter : ISelectionFilter
   {
      /// <summary>
      /// Interface implementation
      /// </summary>
      /// <param name="elem"></param>
      /// <returns></returns>
      public bool AllowElement(Element elem)
      {
         return elem is ModelCurve;
      }

      /// <summary>
      /// Interface implementation
      /// </summary>
      /// <param name="reference"></param>
      /// <param name="position"></param>
      /// <returns></returns>
      public bool AllowReference(Reference reference, XYZ position)
      {
         return false;
      }
   }

   /// <summary>
   /// ToposolidFilter
   /// </summary>
   public class ToposolidFilter : ISelectionFilter
   {
      /// <summary>
      /// Interface implementation
      /// </summary>
      /// <param name="elem"></param>
      /// <returns></returns>
      public bool AllowElement(Element elem)
      {
         return elem is Autodesk.Revit.DB.Toposolid;
      }

      /// <summary>
      /// Interface implementation
      /// </summary>
      /// <param name="reference"></param>
      /// <param name="position"></param>
      /// <returns></returns>
      public bool AllowReference(Reference reference, XYZ position)
      {
         return false;
      }
   }
}

