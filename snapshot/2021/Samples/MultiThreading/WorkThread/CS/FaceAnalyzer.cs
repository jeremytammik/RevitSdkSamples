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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Analysis;


namespace Revit.SDK.Samples.WorkThread.CS
{
    /// <summary>
    ///   This class handles displaying results of an analysis of a wall surface.
    /// </summary>
    /// <remarks>
    ///   The analyzer only displays the results, it does not calculate them.
    ///   The calculation is delegated to a work-thread. The analyzer kicks
    ///   off the calculation and then returns to Revit. Periodically Revit
    ///   checks (during an Idling event) if there is more results available.
    ///   If there is, the analyzer grabs them from the work-thread and takes
    ///   care of the visualization. The thread lets the analyzer to know when 
    ///   the calculation is finally over. When that happens, the application
    ///   can unregister from Idling since it does not need it anymore.
    ///   <para>
    ///   Optionally, Revit can interrupt the analysis, which it typically does
    ///   when the element being analyzed changes (or gets deleted). Depending
    ///   on that change the application requests the analysis to restart
    ///   or to stop completely. The analyzer makes sure the requests
    ///   are delivered to the work-thread.
    ///   </para>
    /// </remarks>
    /// 
    class FaceAnalyzer
    {
        #region class member variables
        // ActiveView
        private View m_view = null;
        // string representation of reference object
        private String m_sreference = null;
        // SpatialFieldManager
        private SpatialFieldManager m_SFManager = null;
        // schema Id
        private int m_schemaId = -1;
        // field Id
        private int m_fieldId = -1;
        // ThreadAgent
        private ThreadAgent m_threadAgent = null;
        // Results
        private SharedResults m_results;
        // If the result manager needs to be initialized
        private bool m_needInitialization = true;
        #endregion

        #region class member methods
        /// <summary>
        ///   Constructor
        /// </summary>
        /// 
        public FaceAnalyzer(View view, String sref)
        {
            m_sreference = sref;
            m_view = view;
        }

        /// <summary>
        ///   Simple convention method to get an actual reference object
        ///   from its stable representation we keep around.
        /// </summary>
        private Reference GetReference()
        {
            if ((m_view != null) && (m_sreference.Length > 0))
            {
                return Reference.ParseFromStableRepresentation(m_view.Document, m_sreference);
            }
            return null;
        }


        /// <summary>
        ///   Getting the face object corresponding to the reference we have stored
        /// </summary>
        /// <remarks>
        ///   The face may change during the time it takes our calculation to finish,
        ///   thus we always get it from the reference right when we actually need it;
        /// </remarks>
        private Face GetReferencedFace()
        {
            Reference faceref = GetReference();
            if (faceref != null)
            {
                return m_view.Document.GetElement(faceref).GetGeometryObjectFromReference(faceref) as Face;
            }
            return null;
        }


        /// <summary>
        ///   Getting ready to preform an analysis for the given in view
        /// </summary>
        /// <remarks>
        ///   This initializes the Spatial Field Manager,
        ///   adds a field primitive corresponding to the face,
        ///   and registers our result schema we want to use.
        ///   The method clears any previous results in the view.
        /// </remarks>
        /// 
        public void Initialize()
        {
            // create of get field manager for the view

            m_SFManager = SpatialFieldManager.GetSpatialFieldManager(m_view);
            if (m_SFManager == null)
            {
                m_SFManager = SpatialFieldManager.CreateSpatialFieldManager(m_view, 1);
            }

            // For the sake of simplicity, we remove any previous results

            m_SFManager.Clear();

            // register schema for the results

            AnalysisResultSchema schema = new AnalysisResultSchema("E4623E91-8044-4721-86EA-2893642F13A9", "SDK2014-AL, Sample Schema");
            m_schemaId = m_SFManager.RegisterResult(schema);

            // Add a spatial field for our face reference

            m_fieldId = m_SFManager.AddSpatialFieldPrimitive(GetReference());

            m_needInitialization = false;
        }


        /// <summary>
        ///   Returns the Id of the element being analyzed
        /// </summary>
        /// <value>
        ///   Id of the element of which face the analysis was set up for.
        /// </value>
        /// 
        public ElementId AnalyzedElementId
        {
            get
            {
                Reference faceref = GetReference();

                if (faceref != null)
                {
                    return faceref.ElementId;
                }

                return ElementId.InvalidElementId;
            }
        }


        /// <summary>
        ///   Updating results on the surface being analyzed
        /// </summary>
        /// <remarks>
        ///   This is called periodically by the Idling event
        ///   until there is no more results to be updated. 
        /// </remarks>
        /// <returns>
        ///   Returns True if there is still more to be processed.
        /// </returns>
        public bool UpdateResults()
        {
            // If we still need to initialize the result manager
            // it means we were interrupted and we restarted.
            // Therefore we know we do not have any results to pick up.
            // Instead, we initialize the manager and start a new calculation.
            // We will have first results from this new calculation process
            // next time we get called here.

            if (m_needInitialization)
            {
                Initialize();
                return StartCalculation();
            }

            // We aks the Result instance if there are results available
            // The methods returns True only if there has been results added
            // since the last time we called that method.

            IList<UV> points;
            IList<ValueAtPoint> values;

            if (m_results.GetResults(out points, out values))
            {
                FieldDomainPointsByUV fieldPoints = new FieldDomainPointsByUV(points);
                FieldValues fieldValues = new FieldValues(values);
                m_SFManager.UpdateSpatialFieldPrimitive(m_fieldId, fieldPoints, fieldValues, m_schemaId);
            }

            // if the thread is not around anymore to result more data,
            // it means the analysis is finished for the FaceAnalyzer too.

            return ((m_threadAgent != null) && (m_threadAgent.IsThreadAlive));
        }


        /// <summary>
        ///   Starting a work-thread to perform the calculation
        /// </summary>
        /// <returns>
        ///   True if the thread started off successfully.
        /// </returns>
        public bool StartCalculation()
        {
            // we need to get the face from the reference we track
            Face theface = GetReferencedFace();
            if (theface == null)
            {
                return false;
            }

            // An instance for the result exchange
            m_results = new SharedResults();

            // The agent does not need the face nor the reference.
            // It can work with just the bounding box and a density of the grid.
            // We also pass the Results as an argument. The thread will be adding calculated results
            // to that object, while here in the analyzer we will read from it. Both operations are thread-safe;

            m_threadAgent = new ThreadAgent(theface.GetBoundingBox(), 10, m_results);

            // now we can ask the agent to start the work thread
            return m_threadAgent.Start();
        }


        /// <summary>
        ///   Stopping the calculation if still in progress
        /// </summary>
        /// <remarks>
        ///   This is typically to be called when there have been changes
        ///   made in the model resulting in changes to the element being analyzed.
        /// </remarks>
        /// 
        public void StopCalculation()
        {
            // We first signal we do not want more results,
            // so the work-thread knows to stop if it is still around.

            m_results.SetCompleted();

            // If the thread is alive, we'll wait for it to finish
            // It will not take longer than one calculation cycle.

            if (m_threadAgent != null)
            {
                if (m_threadAgent.IsThreadAlive)
                {
                    m_threadAgent.WaitToFinish();
                }
                m_threadAgent = null;
            }
        }


        /// <summary>
        ///   Restarting the calculation.
        /// </summary>
        /// <remarks>
        ///   This is probably caused by a change in the face being analyzed,
        ///   typically when the DocumentChanged event indicates modification were made.
        /// </remarks>
        /// 
        public void RestartCalculation()
        {
            // First we make sure we start the ongoing calculation
            StopCalculation();

            // For this might have been called during times when the document
            // is in a non-modifiable state (e.g. during an undo operation)
            // we cannot really start the new calculation just yet. We can only
            // set a flag that a re-initialization is yet to be made,
            // which will be then picked up at the next update. That will happen
            // on a regular Idling event, during which the document is modifiable.
            m_needInitialization = true;
        }
        #endregion


    }  // class

}
