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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ProximityDetection_WallJoinControl.CS
{
   /// <summary>
   /// The class that is responsible for proximity detection
   /// </summary>
   public class ProximityDetection
   {
      /// <summary>
      /// The singleton instance of ProximityDetection
      /// </summary>
      private static ProximityDetection Instance;

      /// <summary>
      /// revit application
      /// </summary>
      private Autodesk.Revit.ApplicationServices.Application m_app;
      /// <summary>
      /// revit document
      /// </summary>
      private Autodesk.Revit.DB.Document m_doc;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="app">Revit application</param>
      /// <param name="doc">Revit document</param>
      private ProximityDetection(
         Autodesk.Revit.ApplicationServices.Application app, 
         Autodesk.Revit.DB.Document doc)
      {
         m_app = app;
         m_doc = doc;
      }

      /// <summary>
      /// Get the singleton instance of ProximityDetection
      /// </summary>
      /// <param name="app">Revit application</param>
      /// <param name="doc">Revit document</param>
      /// <returns>The singleton instance of ProximityDetection</returns>
      public static ProximityDetection getInstance(
         Autodesk.Revit.ApplicationServices.Application app, 
         Autodesk.Revit.DB.Document doc)
      {
         if (Instance == null)
         {
            Instance = new ProximityDetection(app, doc);
         }
         return Instance;
      }

      /// <summary>
      /// Find columns in wall
      /// </summary>
      /// <param name="walls">The walls to be detected</param>
      /// <returns>The detection result</returns>
      public XElement findColumnsInWall(IEnumerable<Wall> walls)
      {
         // create a node that place all walls.
         XElement wallsNode = new XElement("Walls", new XAttribute("Name", "Walls"));

         try
         {
            foreach (Wall wall in walls)
            {
               XElement wallNode = new XElement("Wall", new XAttribute("Name", wall.Name));

               // Iterate to find columns and structural columns
               FilteredElementCollector collector = new FilteredElementCollector(m_doc);
               List<BuiltInCategory> columnCategories = new List<BuiltInCategory>();
               columnCategories.Add(BuiltInCategory.OST_Columns);
               columnCategories.Add(BuiltInCategory.OST_StructuralColumns);
               collector.WherePasses(new ElementMulticategoryFilter(columnCategories));

               // Apply element intersection filter
               ElementIntersectsElementFilter testElementIntersectsElementFilter = 
                  new ElementIntersectsElementFilter(wall);

               collector.WherePasses(testElementIntersectsElementFilter);

               XElement columnsNode = new XElement("columns", 
                  new XAttribute("Count", collector.Count().ToString()));

               foreach (Element column in collector)
               {
                  columnsNode.Add(new XElement("column", new XAttribute("Name", column.Name)));
               }

               wallNode.Add(columnsNode);
               wallsNode.Add(wallNode);
            }
         }
         catch (Exception ex)
         {
            wallsNode.Add(new XElement("Error", new XAttribute("Exception", ex.ToString())));
         }

         // return the whole walls Node
         return wallsNode;

      }

      /// <summary>
      /// Find elements blocking egress
      /// </summary>
      /// <param name="egresses">The egresses to be detected</param>
      /// <returns>The detection result</returns>
      public XElement findBlockingElements(ICollection<Element> egresses)
      {
         // create a node that place all egresses.
         XElement egressesNode = new XElement("Egresses", new XAttribute("Name", "Egresses"));

         try
         {
            // find the elements blocking egress
            foreach (Element egressElement in egresses)
            {
               XElement egressNode = new XElement("Egress", 
                  new XAttribute("Name", egressElement.Name));

               int count = 1;
               IEnumerator<GeometryObject> Objects = egressElement.get_Geometry(new Autodesk.Revit.DB.Options()).GetEnumerator();
               Objects.MoveNext();
               GeometryInstance gi = Objects.Current as GeometryInstance;
               IEnumerator<GeometryObject> Objects1 = gi.GetInstanceGeometry().GetEnumerator();


               //foreach (GeometryObject egressGObj in 
               //   (egressElement.get_Geometry(new Autodesk.Revit.DB.Options()).Objects.get_Item(0) as GeometryInstance).GetInstanceGeometry().Objects)
               while (Objects1.MoveNext())
               {
                  GeometryObject egressGObj = Objects1.Current;

                  if (egressGObj is Solid)
                  {
                     Solid egressVolume = egressGObj as Solid; //calculated from shape and location of a given door

                     XElement solidNode = new XElement("ElementSolid" + count.ToString());
                     // Iterate to find all instance types
                     FilteredElementCollector blockingcollector = new FilteredElementCollector(m_doc);
                     blockingcollector.WhereElementIsNotElementType();

                     // Apply geometric filter
                     ElementIntersectsSolidFilter testElementIntersectsSolidFilter = 
                        new ElementIntersectsSolidFilter(egressVolume);
                     blockingcollector.WherePasses(testElementIntersectsSolidFilter);

                     IEnumerable<Element> blockingElement = blockingcollector;

                     // Exclude the door itself  
                     List<ElementId> exclusions = new List<ElementId>();
                     exclusions.Add(egressElement.Id);
                     blockingcollector.Excluding(exclusions);

                     XElement blockingegressNode = new XElement("blocking_egress_elements", 
                        new XAttribute("Count", blockingElement.Count().ToString()));

                     foreach (Element blockingelement in blockingElement)
                     {
                        blockingegressNode.Add(new XElement("blocking_egress_element", 
                           new XAttribute("Name", blockingelement.Name)));
                     }

                     solidNode.Add(blockingegressNode);
                     egressNode.Add(solidNode);

                     count++;
                  }
               }
               egressesNode.Add(egressNode);
            }
         }
         catch (Exception ex)
         {
            egressesNode.Add(new XElement("Error", new XAttribute("Exception", ex.ToString())));
         }

         // return the whole Egresses Node
         return egressesNode;
      }

      /// <summary>
      /// Find walls (nearly joined to) end of walls
      /// </summary>
      /// <param name="walls">The walls to be detected</param>
      /// <returns>The detection result</returns>
      public XElement findNearbyWalls(IEnumerable<Wall> walls)
      {
         // create a node that place all walls.
         XElement wallsNode = new XElement("Walls", new XAttribute("Name", "Walls"));

         try
         {
            foreach (Wall wall in walls)
            {
               XElement wallNode = new XElement("Wall", new XAttribute("Name", wall.Name));

               // Start
               XElement endNode = new XElement("Start", new XAttribute("Name", "Start"));

               XYZ wallEndPoint = (wall.Location as LocationCurve).Curve.GetEndPoint(0);
               double wallHeight = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();

               FilteredElementCollector collector = nearbyWallsFilter(wallEndPoint, wallHeight, 10.0); // 10 ft

               // Exclude the wall itself
               List<ElementId> exclusions = new List<ElementId>();
               exclusions.Add(wall.Id);
               collector.Excluding(exclusions);

               IEnumerable<Wall> nearbyWalls = collector.OfType<Wall>();

               XElement nearbyWallsNode = new XElement("near_by_walls", 
                  new XAttribute("Count", nearbyWalls.Count().ToString()));

               foreach (Wall nearbywall in nearbyWalls)
               {
                  nearbyWallsNode.Add(new XElement("near_by_wall", 
                     new XAttribute("Name", nearbywall.Name)));
               }

               endNode.Add(nearbyWallsNode);
               wallNode.Add(endNode);

               // End
               endNode = new XElement("End", new XAttribute("Name", "End"));

               wallEndPoint = (wall.Location as LocationCurve).Curve.GetEndPoint(1);

               collector = nearbyWallsFilter(wallEndPoint, wallHeight, 10.0);

               // Exclude the wall itself
               exclusions = new List<ElementId>();
               exclusions.Add(wall.Id);
               collector.Excluding(exclusions);

               nearbyWalls = collector.OfType<Wall>();

               nearbyWallsNode = new XElement("near_by_walls", 
                  new XAttribute("Count", nearbyWalls.Count().ToString()));

               foreach (Wall nearbywall in nearbyWalls)
               {
                  nearbyWallsNode.Add(new XElement("near_by_wall", 
                     new XAttribute("Name", nearbywall.Name)));
               }

               endNode.Add(nearbyWallsNode);
               wallNode.Add(endNode);

               wallsNode.Add(wallNode);
            }
         }
         catch (Exception ex)
         {
            wallsNode.Add(new XElement("Error", new XAttribute("Exception", ex.ToString())));
         }

         // return the whole walls Node
         return wallsNode;
      }

      /// <summary>
      /// Find the nearby walls on specific point and in specific height
      /// </summary>
      /// <param name="point">The given point</param>
      /// <param name="height">The given height</param>
      /// <param name="radius">The radius in which walls can be detected</param>
      /// <returns>The detection result</returns>
      private FilteredElementCollector nearbyWallsFilter(XYZ point, double height, double radius)
      {
         // build cylindrical shape around wall endpoint
         List<CurveLoop> curveloops = new List<CurveLoop>();
         CurveLoop circle = new CurveLoop();
         circle.Append(Arc.Create(point, radius
                                           , 0, Math.PI, 
                                           XYZ.BasisX, XYZ.BasisY));
         circle.Append(Arc.Create(point, radius
                                           , Math.PI, 2 * Math.PI, 
                                           XYZ.BasisX, XYZ.BasisY));
         curveloops.Add(circle);

         Solid wallEndCylinder = 
            GeometryCreationUtilities.CreateExtrusionGeometry(curveloops, XYZ.BasisZ, height);

         // Iterate document to find walls
         FilteredElementCollector collector = new FilteredElementCollector(m_doc);
         collector.OfCategory(BuiltInCategory.OST_Walls);

         // Apply geometric filter
         ElementIntersectsSolidFilter testElementIntersectsSolidFilter = 
            new ElementIntersectsSolidFilter(wallEndCylinder);
         collector.WherePasses(testElementIntersectsSolidFilter);

         return collector;
      }
   }
}
