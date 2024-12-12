//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI;


namespace Revit.SDK.Samples.AnalysisVisualizationFramework.CS
{
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class DistanceToSurfaces : IExternalApplication
   {
      public Result OnStartup(UIControlledApplication uiControlledApplication)
      {
         uiControlledApplication.ControlledApplication.DocumentOpened += new EventHandler<DocumentOpenedEventArgs>(docOpen);
         return Result.Succeeded;
      }

      private void docOpen(object sender, DocumentOpenedEventArgs e)
      {
         Autodesk.Revit.ApplicationServices.Application app = sender as Autodesk.Revit.ApplicationServices.Application;
         UIApplication uiApp = new UIApplication(app);
         Document doc = uiApp.ActiveUIDocument.Document;

         FilteredElementCollector collector = new FilteredElementCollector(doc);
         collector.WherePasses(new ElementClassFilter(typeof(FamilyInstance)));
         var sphereElements = from element in collector where element.Name == "sphere" select element;
         if (sphereElements.Count() == 0)
         {
            TaskDialog.Show("Error", "Sphere family must be loaded");
            return;
         }
         FamilyInstance sphere = sphereElements.Cast<FamilyInstance>().First<FamilyInstance>();
         FilteredElementCollector viewCollector = new FilteredElementCollector(doc);
         ICollection<Element> views = viewCollector.OfClass(typeof(View3D)).ToElements();
         var viewElements = from element in viewCollector where element.Name == "AVF" select element;
         if (viewElements.Count() == 0)
         {
            TaskDialog.Show("Error", "A 3D view named 'AVF' must exist to run this application.");
            return;
         }
         View view = viewElements.Cast<View>().First<View>();

         SpatialFieldUpdater updater = new SpatialFieldUpdater(uiApp.ActiveAddInId, sphere.Id, view.Id);
         if (!UpdaterRegistry.IsUpdaterRegistered(updater.GetUpdaterId())) UpdaterRegistry.RegisterUpdater(updater);
         ElementCategoryFilter wallFilter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
         ElementClassFilter familyFilter = new ElementClassFilter(typeof(FamilyInstance));
         ElementCategoryFilter massFilter = new ElementCategoryFilter(BuiltInCategory.OST_Mass);
         IList<ElementFilter> filterList = new List<ElementFilter>();
         filterList.Add(wallFilter);
         filterList.Add(familyFilter);
         filterList.Add(massFilter);
         LogicalOrFilter filter = new LogicalOrFilter(filterList);

         UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), filter, Element.GetChangeTypeGeometry());
         UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), filter, Element.GetChangeTypeElementDeletion());
      }
      public Result OnShutdown(UIControlledApplication application) {return Result.Succeeded;}
   }

   public class SpatialFieldUpdater : IUpdater
   {
      AddInId addinID;
      UpdaterId updaterID;
      ElementId sphereID;
      ElementId viewID;
      public SpatialFieldUpdater(AddInId id, ElementId sphere, ElementId view)
      {
         addinID = id;
         sphereID = sphere;
         viewID = view;
         updaterID = new UpdaterId(addinID, new Guid("FBF2F6B2-4C06-42d4-97C1-D1B4EB593EFF"));
      }
      public void Execute(UpdaterData data)
      {
         Document doc = data.GetDocument();
         Autodesk.Revit.ApplicationServices.Application app = doc.Application;

         View view = doc.GetElement(viewID) as View;
         FamilyInstance sphere = doc.GetElement(sphereID) as FamilyInstance;
         LocationPoint sphereLP = sphere.Location as LocationPoint;
         XYZ sphereXYZ = sphereLP.Point;

         SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(view);
         if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(view, 3); // Three measurement values for each point
         sfm.Clear();

         FilteredElementCollector collector = new FilteredElementCollector(doc,view.Id);
         ElementCategoryFilter wallFilter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
         ElementCategoryFilter massFilter = new ElementCategoryFilter(BuiltInCategory.OST_Mass);
         LogicalOrFilter filter = new LogicalOrFilter(wallFilter, massFilter);
         ICollection<Element> elements = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

         foreach (Face face in GetFaces(elements))
         {
            int idx = sfm.AddSpatialFieldPrimitive(face.Reference);
            List<double> doubleList = new List<double>();
            IList<UV> uvPts = new List<UV>();
            IList<ValueAtPoint> valList = new List<ValueAtPoint>();
            BoundingBoxUV bb = face.GetBoundingBox();
            for (double u = bb.Min.U; u < bb.Max.U; u = u + (bb.Max.U - bb.Min.U) / 15)
            {
               for (double v = bb.Min.V; v < bb.Max.V; v = v + (bb.Max.V - bb.Min.V) / 15)
               {
                  UV uvPnt = new UV(u, v);
                  uvPts.Add(uvPnt);
                  XYZ faceXYZ = face.Evaluate(uvPnt);
                   // Specify three values for each point
                  doubleList.Add(faceXYZ.DistanceTo(sphereXYZ));
                  doubleList.Add(-faceXYZ.DistanceTo(sphereXYZ));
                  doubleList.Add(faceXYZ.DistanceTo(sphereXYZ) * 10);
                  valList.Add(new ValueAtPoint(doubleList));
                  doubleList.Clear();
               }
            }
            FieldDomainPointsByUV pnts = new FieldDomainPointsByUV(uvPts);
            FieldValues vals = new FieldValues(valList);

            AnalysisResultSchema resultSchema1 = new AnalysisResultSchema("Schema 1", "Schema 1 Description");
            IList<int> registeredResults = new List<int>();
            registeredResults = sfm.GetRegisteredResults();
            int idx1 = 0;
            if (registeredResults.Count == 0)
            {
                idx1 = sfm.RegisterResult(resultSchema1);
            }
            else
            {
                idx1 = registeredResults.First();
            }
            sfm.UpdateSpatialFieldPrimitive(idx, pnts, vals, idx1);
         }
      }
      public string GetAdditionalInformation() { return "Calculate distance from sphere to walls and display results"; }
      public ChangePriority GetChangePriority() { return ChangePriority.FloorsRoofsStructuralWalls; }
      public UpdaterId GetUpdaterId() { return updaterID; }
      public string GetUpdaterName() { return "Distance to Surfaces"; }

      private FaceArray GetFaces(ICollection<Element> elements)
      {
         FaceArray faceArray = new FaceArray();
         Options options = new Options();
         options.ComputeReferences = true;
         foreach (Element element in elements)
         {
            GeometryElement geomElem = element.get_Geometry(options);
            if (geomElem != null)
            {
                foreach (GeometryObject geomObj in geomElem)
               {
                  Solid solid = geomObj as Solid;
                  if (solid != null)
                  {
                     foreach (Face f in solid.Faces)
                     {
                        faceArray.Append(f);
                     }
                  }
                  GeometryInstance inst = geomObj as GeometryInstance;
                  if (inst != null) // in-place family walls
                  {
                     foreach (Object o in inst.SymbolGeometry)
                     {
                        Solid s = o as Solid;
                        if (s != null)
                        {
                           foreach (Face f in s.Faces)
                           {
                              faceArray.Append(f);
                           }
                        }
                     }
                  }
               }
            }
         }
         return faceArray;
      }
   }
}
 