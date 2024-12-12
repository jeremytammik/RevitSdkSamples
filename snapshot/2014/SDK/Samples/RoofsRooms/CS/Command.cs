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
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;

namespace Revit.SDK.Samples.RoofsRooms.CS
{
    /// <summary>
    /// This class inherits from IExternalCommand, 
    /// used to check if room can cut roof by geometry relationship
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class Command : IExternalCommand
    {
        #region Class Variables
        // Revit application
        Autodesk.Revit.ApplicationServices.Application m_application;
        // Current document in Revit
        Document m_document;
        #endregion

        #region Implement IExternalCommand
        ///<summary>
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
            ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string log = assemblyLocation + "." + DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (File.Exists(log)) File.Delete(log);
            TraceListener txtListener = new TextWriterTraceListener(log);
            Trace.Listeners.Add(txtListener);
            try
            {
                // variable initialization
                m_application = commandData.Application.Application;
                m_document = commandData.Application.ActiveUIDocument.Document;
                Environment.CurrentDirectory = Path.GetDirectoryName(assemblyLocation);

                FindRoomBoundingRoofs(ref message, elements);

                // Not show TaskDialog in regression mode
                if (0 == commandData.JournalData.Count)
                {
                    TaskDialog.Show("Roofs Rooms", message);
                }

                // Insert result to journal data for regression purpose.
                const string DataKey = "Results";
                if (!commandData.JournalData.ContainsKey(DataKey))
                {
                    // In normal/recording mode 
                    commandData.JournalData.Add(DataKey, message);
                }
                else
                {
                    // In regression/replaying mode
                    commandData.JournalData[DataKey] = message;
                }

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                message = ex.ToString();
                return Autodesk.Revit.UI.Result.Failed;
            }
            finally
            {
                Trace.Flush();
                txtListener.Close();
                Trace.Close();
                Trace.Listeners.Remove(txtListener);
            }
        }

        /// <summary>
        /// Test whether each room has a roof to bound it.
        /// </summary>
        /// <param name="message">Error message to be dumped.</param>
        /// <param name="elements">Some elements to return.</param>
        /// <returns></returns>
        private bool FindRoomBoundingRoofs(ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            // Get all rooms
            List<Element> rooms = GetRoomsElements();
            if (rooms.Count == 0)
            {
                message = "Unable to identify any rooms, please create room first!";
                return false;
            }

            // Represents the criteria for boundary elements to be considered bounding roofs
            LogicalOrFilter categoryFilter = new LogicalOrFilter(new ElementCategoryFilter(BuiltInCategory.OST_Roofs),
                                                                    new ElementCategoryFilter(BuiltInCategory.OST_RoofSoffit));

            // Calculator for room/space geometry.
            SpatialElementGeometryCalculator calculator = new SpatialElementGeometryCalculator(m_document);

            // Stores the resulting room->roof relationships
            Dictionary<Element, List<ElementId>> roomsAndRoofs = new Dictionary<Element, List<ElementId>>();

            foreach (Element room in rooms)
            {
                // Get room geometry & boundaries          
                SpatialElementGeometryResults results = calculator.CalculateSpatialElementGeometry((SpatialElement)room);

                // Get solid geometry so we can examine each face
                Solid geometry = results.GetGeometry();

                foreach (Face face in geometry.Faces)
                {
                    // Get list of roof boundary subfaces for a given face
                    IList<SpatialElementBoundarySubface> boundaryFaces = results.GetBoundaryFaceInfo(face);
                    foreach (SpatialElementBoundarySubface boundaryFace in boundaryFaces)
                    {
                        // Get boundary element
                        LinkElementId boundaryElementId = boundaryFace.SpatialBoundaryElement;

                        // Only considering local file room bounding elements
                        ElementId localElementId = boundaryElementId.HostElementId;

                        // Evaluate if element meets criteria using PassesFilter()
                        if (localElementId != ElementId.InvalidElementId && categoryFilter.PassesFilter(m_document, localElementId))
                        {
                            // Room already has roofs, add more
                            if (roomsAndRoofs.ContainsKey(room))
                            {
                                List<ElementId> roofs = roomsAndRoofs[room];
                                if (!roofs.Contains(localElementId))
                                    roofs.Add(localElementId);
                            }
                            // Room found first roof
                            else
                            {
                                List<ElementId> roofs = new List<ElementId>();
                                roofs.Add(localElementId);
                                roomsAndRoofs.Add(room, roofs);
                            }
                            break;
                        }
                    }
                }
            }

            // Format results
            if (roomsAndRoofs.Count > 0)
            {
                String logs = String.Format("Rooms that have a bounding roof:");
                message += logs + "\t\r\n";
                Trace.WriteLine(logs);
                foreach (KeyValuePair<Element, List<ElementId>> kvp in roomsAndRoofs)
                {
                    // remove this room from all rooms list
                    rooms.Remove(kvp.Key);

                    List<ElementId> roofs = kvp.Value;
                    String roofsString;

                    // Single roof boundary
                    if (roofs.Count == 1)
                    {
                        Element roof = m_document.GetElement(roofs[0]);
                        roofsString = String.Format("Roof: Id = {0}, Name = {1}", roof.Id.IntegerValue, roof.Name);
                    }
                    // Multiple roofs
                    else
                    {
                        roofsString = "Roofs ids = " + string.Join(", ", Array.ConvertAll<ElementId, string>(roofs.ToArray(), i => i.ToString()));
                    }

                    // Save results
                    logs = String.Format(
                        "  Room: Id = {0}, Name = {1} --> {2}",
                        kvp.Key.Id.IntegerValue, kvp.Key.Name, roofsString);
                    message += logs + "\t\r\n";
                    Trace.WriteLine(logs);
                }
            }

            // Format the rooms that have no bounding roof
            Trace.WriteLine("Geometry relationship checking finished...");
            if (rooms.Count != 0)
            {
                String logs = String.Format("Below rooms don't have bounding roofs:");
                message += logs + "\t\r\n";
                Trace.WriteLine(logs);
                foreach (Element room in rooms)
                {
                    elements.Insert(room);
                    logs = String.Format("  Room Id: {0}, Room Name: {1}",
                        room.Id.IntegerValue, room.Name);
                    message += logs + "\t\r\n";
                    Trace.WriteLine(logs);
                }
            }

            return true;
        }

        /// <summary>
        /// Retrieve all Rooms and Spaces elements from active document.
        /// </summary>
        /// <returns>Element list retrieved from current document.</returns>
        private List<Element> GetRoomsElements()
        {
            List<Element> array = new List<Element>();
            ElementFilter roomSpaceFilter = new LogicalOrFilter(new RoomFilter(), new SpaceFilter());
            FilteredElementCollector collector = new FilteredElementCollector(m_document);
            array.AddRange(collector.WherePasses(roomSpaceFilter).ToElements());
            return array;
        }

        #endregion
    }
}
