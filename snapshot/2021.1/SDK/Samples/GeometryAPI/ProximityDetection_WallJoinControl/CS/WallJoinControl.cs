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
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ProximityDetection_WallJoinControl.CS
{
   /// <summary>
   /// The class that is responsible for controlling the joint of walls
   /// </summary>
   public class WallJoinControl
   {
      /// <summary>
      /// The singleton instance of WallJoinControl
      /// </summary>
      private static WallJoinControl Instance;

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
      private WallJoinControl(
         Autodesk.Revit.ApplicationServices.Application app, 
         Autodesk.Revit.DB.Document doc)
      {
         m_app = app;
         m_doc = doc;
      }

      /// <summary>
      /// Get the singleton instance of WallJoinControl
      /// </summary>
      /// <param name="app">Revit application</param>
      /// <param name="doc">Revit document</param>
      /// <returns>The singleton instance of WallJoinControl</returns>
      public static WallJoinControl getInstance(
         Autodesk.Revit.ApplicationServices.Application app, 
         Autodesk.Revit.DB.Document doc)
      {
         if (Instance == null)
         {
            Instance = new WallJoinControl(app, doc);
         }
         return Instance;
      }

      /// <summary>
      /// Check every wall's joined walls by WallUtils method in initial, after disallow join, then after allow join states
      /// </summary>
      /// <param name="walls">The walls to be checked</param>
      /// <returns>The check result</returns>
      public XElement checkJoinedWalls(IEnumerable<Wall> walls)
      {
         // create a node that place all walls.
         XElement wallsNode = new XElement("Walls", new XAttribute("Name", "Walls"));

         try
         {
            foreach (Wall wall in walls)
            {
               // create a wall node
               XElement wallNode = new XElement("Wall", 
                  new XAttribute("name", wall.Name), 
                  new XAttribute("Type", wall.WallType.Kind.ToString()));

               LocationCurve locationCurve = wall.Location as LocationCurve;
               if (null == locationCurve)
               {
                  wallNode.Add(new XElement("Error", 
                     new XAttribute("Exception", "This wall has not a LocationCurve!")));
               }
               else
               {
                  // start
                  XElement endNode = new XElement("Start", new XAttribute("Name", "Start"));
                  wallNode.Add(checkWallEnd(wall, locationCurve, 0, endNode));

                  // end
                  endNode = new XElement("End", new XAttribute("Name", "End"));
                  wallNode.Add(checkWallEnd(wall, locationCurve, 1, endNode));
               }
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
      /// Check wall's two ends in three states
      /// </summary>
      /// <param name="wall">The wall to be checked</param>
      /// <param name="locationCurve">The wall's location curve</param>
      /// <param name="end">The index indicates the start or end of this wall</param>
      /// <param name="endnode">Result XML node</param>
      /// <returns>The check result</returns>
      private XElement checkWallEnd(Wall wall, LocationCurve locationCurve, int end, XElement endnode)
      {
         // Initial state
         XElement stateNode = new XElement("Initial", new XAttribute("Name", "Initial"));
         endnode.Add(GetState(wall, locationCurve, end, stateNode));

         // Disallow join
         WallUtils.DisallowWallJoinAtEnd(wall, end);

         // After DisallowWallJoinAtEnd Evoked state
         stateNode = new XElement("After_DisallowWallJoinAtEnd_Evoked", 
            new XAttribute("Name", "After_DisallowWallJoinAtEnd_Evoked"));
         endnode.Add(GetState(wall, locationCurve, end, stateNode));

         // Allow join
         WallUtils.AllowWallJoinAtEnd(wall, end);

         // After AllowWallJoinAtEnd Evoked state
         stateNode = new XElement("After_DisallowWallJoinAtEnd_Evoked", 
            new XAttribute("Name", "After_DisallowWallJoinAtEnd_Evoked"));
         endnode.Add(GetState(wall, locationCurve, end, stateNode));

         return endnode;
      }

      /// <summary>
      /// Get wall's one end state
      /// </summary>
      /// <param name="wall">The wall to be checked</param>
      /// <param name="locationCurve">The wall's location curve</param>
      /// <param name="end">The index indicates the start or end of this wall</param>
      /// <param name="statenode">Result XML node</param>
      /// <returns>The check result</returns>
      private XElement GetState(Wall wall, LocationCurve locationCurve, int end, XElement statenode)
      {
         // Get geometry information
         statenode.Add(GetGeometryInfo(locationCurve, end));
         // Get IsWallJoinAllowedAtEnd API method's result
         statenode.Add(GetIsWallJoinAllowedAtEnd(wall, end));
         // Get all joined walls
         statenode.Add(GetJoinedWalls(locationCurve, end));

         return statenode;
      }

      /// <summary>
      /// Get wall's one end geometry information
      /// </summary>
      /// <param name="locationCurve">The wall's location curve</param>
      /// <param name="end">The index indicates the start or end of this wall</param>
      /// <returns>The check result</returns>
      private XElement GetGeometryInfo(LocationCurve locationCurve, int end)
      {
         XElement geometryinfoNode = new XElement("GeometryInfo");
         // Output LocationCurve end point coordinate
         XYZ endpoint = locationCurve.Curve.GetEndPoint(end);
         geometryinfoNode.Add(new XElement("PointCoordinate", 
            new XAttribute("X", Math.Round(endpoint.X, 9).ToString()),
            new XAttribute("Y", Math.Round(endpoint.Y, 9).ToString()),
            new XAttribute("Z", Math.Round(endpoint.Z, 9).ToString())));

         return geometryinfoNode;
      }

      /// <summary>
      /// Get wall's one end IsWallJoinAllowedAtEnd API method's result
      /// </summary>
      /// <param name="wall">The wall to be checked</param>
      /// <param name="end">The index indicates the start or end of this wall</param>
      /// <returns>The check result</returns>
      private XElement GetIsWallJoinAllowedAtEnd(Wall wall, int end)
      {
         return new XElement("IsWallJoinAllowedAtEnd", 
            new XAttribute("Value", WallUtils.IsWallJoinAllowedAtEnd(wall, end)));
      }

      /// <summary>
      /// Get wall's one end all joined walls
      /// </summary>
      /// <param name="locationCurve">The wall's location curve</param>
      /// <param name="end">The index indicates the start or end of this wall</param>
      /// <returns>The check result</returns>
      private XElement GetJoinedWalls(LocationCurve locationCurve, int end)
      {
         // retrieve joined elements 
         ElementArray array = locationCurve.get_ElementsAtJoin(end);

         XElement joinedwallsNode = new XElement("JoinedWalls", 
            new XAttribute("Count", array.Size.ToString()));

         // output array
         foreach (Element ele in array)
         {
            if (ele is Wall)
            {
               joinedwallsNode.Add(new XElement("JoinedWall", 
                  new XAttribute("Name", ele.Name)));
            }
         }

         return joinedwallsNode;
      }
   }
}
