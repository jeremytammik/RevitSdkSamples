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
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.UI.Selection;
using System.Diagnostics;

namespace Revit.SDK.Samples.MultithreadedCalculation.CS
{
    /// <summary>
    /// Command to set the target element and begin the multithreaded calculation.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class MultithreadedCalculation : IExternalCommand
    {
        static UpdaterId s_updaterId;
        static int s_spatialFieldId;
        static int s_oldSpatialFieldId;
        static string s_docName;

        static ElementId s_oldViewId;
        static ElementId s_activeViewId;

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;
            s_docName = doc.PathName;

            Element element = null;
            try
            {
                element = doc.GetElement(uiDoc.Selection.PickObject(ObjectType.Element, "Select an element for the AVF demonstration."));
            }
            catch (System.Exception)
            {
                message = "User aborted the tool.";
                return Result.Cancelled;
            }
            
            // Set up SpatialFieldManager to hold results
            s_activeViewId = doc.ActiveView.Id; 
            SpatialFieldManager oldSfm = null; 
            View oldView = null;
            if (s_oldViewId != null) oldView = doc.GetElement(s_oldViewId) as View;
            if (oldView != null) oldSfm = SpatialFieldManager.GetSpatialFieldManager(oldView);
            // If a previous SFM was being managed, delete it
            if (oldSfm != null) oldSfm.RemoveSpatialFieldPrimitive(s_oldSpatialFieldId);

            // Setup container object for executing the calculation
            MultithreadedCalculationContainer container = CreateContainer(element);

            // Register updater to watch for geometry changes
            SpatialFieldUpdater updater = new SpatialFieldUpdater(container,uiApp.ActiveAddInId);
            if (!UpdaterRegistry.IsUpdaterRegistered(updater.GetUpdaterId())) 
                UpdaterRegistry.RegisterUpdater(updater, doc);
            IList<ElementId> idCollection = new List<ElementId>();
            idCollection.Add(element.Id);
            UpdaterRegistry.RemoveAllTriggers(s_updaterId);
            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), doc, idCollection, Element.GetChangeTypeGeometry());

            // Register idling event
            uiApp.Idling += new EventHandler<IdlingEventArgs>(container.UpdateWhileIdling);

            // Start new thread
            Thread thread = new Thread(new ThreadStart(container.Run));
            thread.Start();

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// Prepares a container object that carries out the calculations without invoking Revit API calls.
        /// </summary>
        /// <param name="element">The element for the calculations.</param>
        /// <returns>The container.</returns>
        public static MultithreadedCalculationContainer CreateContainer(Element element)
        {
            Document doc = element.Document;
            View activeView = doc.GetElement(s_activeViewId) as View;

            // Figure out which is the largest face facing the user
            XYZ viewDirection = activeView.ViewDirection.Normalize();   
            Face biggestFace = GetBiggestFaceFacingUser(element, viewDirection);
                 
            // Get or create SpatialFieldManager for AVF results
            SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(activeView);
            if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(activeView, 1);

            // Reference the target face
            s_spatialFieldId = sfm.AddSpatialFieldPrimitive(biggestFace.Reference);

            // Compute the range of U and V for the calculation
            BoundingBoxUV bbox = biggestFace.GetBoundingBox();

            return new MultithreadedCalculationContainer(doc.PathName, bbox.Min, bbox.Max);
       }

        /// <summary>
        /// Gets the biggest face which faces the user.  Assumes that the element is a wall, or floor, or other "2-sided" element, and that
        /// one of the two biggest faces will be facing roughly towards the viewer.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="viewDirection">The view direction.</param>
        /// <returns>The face.  Face.Reference will also be populated.</returns>
        private static Face GetBiggestFaceFacingUser(Element element, XYZ viewDirection)
        {
            
            // Holds the faces sorted by area
            SortedDictionary<double, List<Face>> faceAreas = new SortedDictionary<double, List<Face>>();

            // Get the element geometry
            Options options = new Options();
            options.ComputeReferences = true;
            GeometryElement geomElem = element.get_Geometry(options);
            
            // Look at the faces in each solid
            foreach (GeometryObject geomObj in geomElem)
            {
                Solid solid = geomObj as Solid;
                if (solid != null)
                {
                    foreach (Face face in solid.Faces)
                    {
                        double area = face.Area;
                        // Save the face to the collection
                        if (faceAreas.ContainsKey(area))
                        {
                            faceAreas[area].Add(face);
                        }
                        else
                        {
                            List<Face> faces = new List<Face>();
                            faces.Add(face);
                            faceAreas.Add(area, faces);
                        }
                    }
                }
            }

            // Get biggest two faces.  There might be two faces in the last item, or one face in the last item.
            int count = faceAreas.Count;
            KeyValuePair<double, List<Face>> faceCollection1 = faceAreas.ElementAt<KeyValuePair<double, List<Face>>>(count - 1);
            KeyValuePair<double, List<Face>> faceCollection2 = faceAreas.ElementAt<KeyValuePair<double, List<Face>>>(count - 2);

            Face face1 = null;
            Face face2 = null;
            // Two or more equal faces.  Use the first two.
            if (faceCollection1.Value.Count > 1)
            {
                face1 = faceCollection1.Value[0];
                face2 = faceCollection1.Value[1];
            }
            // One largest face.  Use the first face from the next item for comparison.
            else
            {
                face1 = faceCollection1.Value[0];
                face2 = faceCollection2.Value[0];
            }

            // Compute face normal
            BoundingBoxUV box = face1.GetBoundingBox();
            UV faceCenter = (box.Max + box.Min) / 2;
            XYZ faceNormal = face1.ComputeNormal(faceCenter).Normalize();

            // Compute angle to the view direction.  If less than 90 degrees, keep this face.
            double angle = viewDirection.AngleTo(faceNormal);

            Face biggestFace = null;
            if (Math.Abs(angle) < Math.PI / 2)
                biggestFace = face1;
            else
                biggestFace = face2;

            return biggestFace;
        }

        /// <summary>
        /// Updater called when wall geometry changes, so analysis results can update.
        /// </summary>
        public class SpatialFieldUpdater : IUpdater
        {
            // The old container object.
            MultithreadedCalculationContainer containerOld;

            public SpatialFieldUpdater(MultithreadedCalculationContainer _container, AddInId addinId)
            {
                containerOld = _container;
                s_updaterId = new UpdaterId(addinId, new Guid("FBF2F6B2-4C06-42d4-97C1-D1B4EB593EFF"));
            }

            // Execution method for the updater
            public void Execute(UpdaterData data)
            {
                // Remove old idling event callback
                UIApplication uiApp = new UIApplication(data.GetDocument().Application);
                uiApp.Idling -= containerOld.UpdateWhileIdling;
                containerOld.Stop();

                // Clear the current AVF results
                Document doc = data.GetDocument();
                View activeView = doc.GetElement(s_activeViewId) as View;
                SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(activeView);
                sfm.Clear();

                // Restart the multithread calculation with a new container
                Element modifiedElem = doc.GetElement(data.GetModifiedElementIds().First<ElementId>());
                MultithreadedCalculationContainer container = MultithreadedCalculation.CreateContainer(modifiedElem);
                containerOld = container;

                // Setup the new idling callback
                uiApp.Idling += new EventHandler<IdlingEventArgs>(container.UpdateWhileIdling);

                // Start the thread
                Thread threadNew = new Thread(new ThreadStart(container.Run));
                threadNew.Start();
            }

            public string GetAdditionalInformation() { return "AVF DMU Thread sample"; }
            public ChangePriority GetChangePriority() { return ChangePriority.FloorsRoofsStructuralWalls; }
            public UpdaterId GetUpdaterId() { return s_updaterId; }
            public string GetUpdaterName() { return "AVF DMU Thread"; }
        }

        /// <summary>
        /// Container class that manages the multithreaded calculation and idling activity.
        /// </summary>
        public class MultithreadedCalculationContainer
        {
            private volatile bool m_stop = false;
            UV m_min;
            UV m_max;
            
            string m_docName;
            IList<ResultsData> results = new List<ResultsData>();
            IList<UV> m_uvToCalculate = new List<UV>();
            int m_uvToCalculateCount;
            IList<UV> uvPts = new List<UV>();
            IList<ValueAtPoint> valList = new List<ValueAtPoint>();

            public MultithreadedCalculationContainer(string _docName, UV _min, UV _max)
            {
                m_docName = _docName;
                m_min = _min;
                m_max = _max;
            }

            public void Run()
            {
                m_uvToCalculate = DetermineFacePoints();
                m_uvToCalculateCount = m_uvToCalculate.Count;
                Calculate();
            }

            /// <summary>
            /// Stops the thread/calculation and application via idling.
            /// </summary>
            public void Stop()
            {
                m_stop = true;
            }


            /// <summary>
            /// The idling callback which adds data to the AVF results.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void UpdateWhileIdling(object sender, IdlingEventArgs e)
            {
                UIApplication uiApp = sender as UIApplication;

                // Get SpatialFieldManager

                AnalysisResultSchema resultSchema = new AnalysisResultSchema("Schema Name", "Description");
                SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(uiApp.ActiveUIDocument.Document.ActiveView);
                
                if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(uiApp.ActiveUIDocument.Document.ActiveView, 1);
                int schemaIndex = sfm.RegisterResult(resultSchema);
                // If stopping, clear results and unset event.
                if (m_stop)
                {
                    lock (results)
                    {
                        results.Clear();
                    }
                    uiApp.Idling -= UpdateWhileIdling;
                    return;
                }

                // If document was closed and new document opened, do not run the update.
                if (uiApp.ActiveUIDocument.Document.PathName == m_docName) 
                {
                    // Lock access to current calculated results
                    lock (results)
                    {
                        if (results.Count == 0) return;

                        // Turn each result to an AVF ValueAtPoint
                        foreach (ResultsData rData in results)
                        {
                            uvPts.Add(new UV(rData.UV.U, rData.UV.V));
                            IList<double> doubleList = new List<double>();
                            doubleList.Add(rData.Value);
                            valList.Add(new ValueAtPoint(doubleList));
                        }
                        FieldDomainPointsByUV pntsByUV = new FieldDomainPointsByUV(uvPts);
                        FieldValues fieldValues = new FieldValues(valList);

                        // Update with calculated values
                        Transaction t = new Transaction(uiApp.ActiveUIDocument.Document);
                        t.SetName("AVF");
                        t.Start();
                        if (!m_stop)
                            sfm.UpdateSpatialFieldPrimitive(s_spatialFieldId, pntsByUV, fieldValues, schemaIndex);
                        t.Commit();  

                        // Clear results already processed.
                        results.Clear();

                        // If no more results to process, remove the idling event
                        if (m_uvToCalculateCount == 0)
                        {
                            uiApp.Idling -= UpdateWhileIdling;
                            s_oldViewId = s_activeViewId;
                            s_oldSpatialFieldId = s_spatialFieldId;
                        }
                    }
                }
            }

            // Calculate the results in a loop 
            void Calculate()
            {
                foreach (UV uv in m_uvToCalculate)
                {
                    if (m_stop)
                    {
                        m_uvToCalculateCount = 0;
                        return;
                    }
                    // Lock access to results while the data is added
                    lock (results)
                    {
                        results.Add(new ResultsData(uv, 1000 * Math.Sin(Math.Abs(uv.U * uv.V))));
                        Thread.Sleep(500); // to simulate the effect of a complex computation
                        m_uvToCalculateCount--;
                    }
                }
            }

            private const int numberOfUPnts = 10;
            private const int numberOfVPnts = 5;

            // Setup the list of UV points to calculate results for
            IList<UV> DetermineFacePoints()
            {
                IList<UV> uvList = new List<UV>();
                double upnt = m_min.U;
                double incrementU = (m_max.U - m_min.U) / (numberOfUPnts - 1);
                double incrementV = (m_max.V - m_min.V) / (numberOfVPnts - 1);
                while (upnt <= m_max.U)
                {
                    double vpnt = m_min.V;
                    while (vpnt <= m_max.V)
                    {
                        uvList.Add(new UV(upnt,vpnt));
                        vpnt = vpnt + incrementV;
                    }
                    upnt = upnt + incrementU;
                }
                return uvList;
            }
        }

        // Represents a set of results for the calculation
        public class ResultsData
        {
            public UV UV;
            public double Value;
            public ResultsData(UV uv, double value)
            {
                this.UV = uv;
                Value = value;
            }
        }
    }
}