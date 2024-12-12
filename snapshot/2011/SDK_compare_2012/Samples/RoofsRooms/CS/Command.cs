//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.Windows.Forms;
using System.Reflection;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using AElement = Autodesk.Revit.DB.Element;

namespace Revit.SDK.Samples.RoofsRooms.CS
{
    /// <summary>
    /// This class inherits from IExternalCommand, 
    /// used to check if room can cut roof by geometry relationship
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
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
            Transaction transaction = new Transaction(commandData.Application.ActiveUIDocument.Document, "External Tool");
            transaction.Start();
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
                //
                // just call the entry, don't care the return 
                Test(ref message, elements);
                //
                // show the check result
                MessageBox.Show(message, "Roofs Rooms");
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
                transaction.Commit();
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
        private bool Test(ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            // Get all rooms
            List<AElement> rooms = GetRoomsElements();
            if (rooms.Count == 0)
            {
                message = "Unable to identify any rooms, please create room first!";
                return false;
            }
                        
            // Check if one room can cut the roof(that's, room has bounding roof).
            // Will use BoundingBox firstly to kick out those exception cases promptly.
            // Unplaced room(without bounding box) won't be calculated.
            Dictionary<AElement, AElement> roomsAndRoofs = new Dictionary<AElement, AElement>();
            foreach (AElement room in rooms)
            {
                BoundingBoxXYZ roomBBox = room.get_BoundingBox(null);
                if (room.Level == null || roomBBox == null)
                {
                    continue;
                }
                // 
                // Use room to cut every roof, break checking if room can be cut by any roof.
                // Skip the roof if it doesn't intersect the bounding box of the room.
               
                //This combination of filters finds: (1. elements whose category is "Roof" or "RoofSoffit" 2. elements which are not 'type' elements 3. elements whose bounding box intersects the room).
                LogicalOrFilter categoryFilter = new LogicalOrFilter(new ElementCategoryFilter(BuiltInCategory.OST_Roofs), new ElementCategoryFilter(BuiltInCategory.OST_RoofSoffit));
                Outline myOutLn = new Outline(roomBBox.Min, roomBBox.Max);
                BoundingBoxIntersectsFilter intersectFilter = new BoundingBoxIntersectsFilter(myOutLn);
                FilteredElementCollector collector = new FilteredElementCollector(m_document);
                ICollection<Element> roofs = collector.WherePasses(categoryFilter).WhereElementIsNotElementType().WherePasses(intersectFilter).ToElements();

                foreach (Element roof in roofs)
                {
                    BoundingBoxXYZ roofBBox = roof.get_BoundingBox(null);
                    if (roofBBox == null)
                    {
                        continue;
                    }
                    // check the relationship between room and roof by solid geometry.
                    // Break the checking for room if room can be cut by anyone roof.
                    if (GeomUtil.RoofCutElement(roof, room))
                    {
                        // remember the room cut and break the next checking
                        roomsAndRoofs.Add(room, roof);
                        break; // break next checking for room for performance
                    }
                }
            }

            // 
            // Dump the checking results-->
            //
            // dump the rooms which have bounding roof
            if(roomsAndRoofs.Count > 0) 
            {
                String logs = String.Format("Below rooms have bounding roof:");
                message += logs + "\t\r\n";
                Trace.WriteLine(logs);
                foreach (KeyValuePair<AElement, AElement> kvp in roomsAndRoofs)
                {
                    // remove this room from all rooms list
                    rooms.Remove(kvp.Key);
                    //
                    // dump information
                    logs = String.Format(
                        "  Room: Id = {0}, Name = {1} --> Roof: Id = {2}, Name = {3}", 
                        kvp.Key.Id.IntegerValue, kvp.Key.Name, kvp.Value.Id.IntegerValue, kvp.Value.Name);
                    message += logs + "\t\r\n";
                    Trace.WriteLine(logs);                    
                }
            }

            // Dump the rooms don't have bounding roof.
            Trace.WriteLine("Geometry relationship checking finished...");
            if (rooms.Count != 0)
            {
                String logs = String.Format("Below rooms don't have bounding roofs:");
                message += logs + "\t\r\n";
                Trace.WriteLine(logs);
                foreach (AElement room in rooms)
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
        private List<AElement> GetRoomsElements()
        {
            List<AElement> array = new List<AElement>();
            ElementFilter roomSpaceFilter = new LogicalOrFilter(new RoomFilter(), new SpaceFilter());
            FilteredElementCollector collector = new FilteredElementCollector(m_document);
            array.AddRange(collector.WherePasses(roomSpaceFilter).ToElements());
            return array;
        }

        #endregion
    }
}
