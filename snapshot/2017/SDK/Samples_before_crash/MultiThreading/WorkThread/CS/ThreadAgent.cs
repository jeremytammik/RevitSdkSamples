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
using System.Threading;
using Autodesk.Revit.DB;


namespace Revit.SDK.Samples.WorkThread.CS
{
    /// <summary>
    ///   A main class of the delegated thread.
    /// </summary>
    /// <remarks>
    ///   It has few data the calculations needs and
    ///   one method that will run on a separate thread.
    /// </remarks>
    /// 
    class ThreadAgent
    {
        #region class member variables
        // The main method for calculating results for the face analysis
        private Thread m_thread = null;
        // Results
        private SharedResults m_results;
        // BoundingBoxUV
        private BoundingBoxUV m_bbox;
        // Density
        private int m_density;
        #endregion

        #region class member methods
        /// <summary>
        ///  A constructor initializes a bounding box and
        ///  the density of the grid for the values to be calculated at.
        /// </summary>
        /// 
        public ThreadAgent(BoundingBoxUV bbox, int density, SharedResults results)
        {
            m_bbox = bbox;
            m_density = density;
            m_results = results;
        }

        /// <summary>
        ///   Creates and starts a work thread operating upon the given  shared results.
        /// </summary>
        /// <returns>
        ///   True if a work thread could be started successfully.
        /// </returns>
        /// 
        public bool Start()
        {
            if (IsThreadAlive)
            {
                return false;
            }

            m_thread = new Thread(new ParameterizedThreadStart(this.Run));
            m_thread.Start(m_results);
            return true;
        }


        /// <summary>
        ///   A property to test whether the calculation thread is still alive.
        /// </summary>
        /// 
        public bool IsThreadAlive
        {
            get
            {
                return (m_thread != null) && (m_thread.IsAlive);
            }
        }


        /// <summary>
        ///   Waits for the work thread to finish
        /// </summary>
        /// 
        public void WaitToFinish()
        {
            if (IsThreadAlive)
            {
                m_thread.Join();
            }
        }


        /// <summary>
        ///   The main method for calculating results for the face analysis.
        /// </summary>
        /// <remarks>
        ///   The calculated values do not mean anything particular.
        ///   They are just to demonstrate how to process a potentially
        ///   time-demanding analysis in a delegated work-thread.
        /// </remarks>
        /// <param name="data">
        ///   The instance of a Result object to which the results
        ///   will be periodically delivered until we either finish
        ///   the process or are asked to stop.
        /// </param>
        /// 
        private void Run(Object data)
        {
            SharedResults results = data as SharedResults;

            double uRange = m_bbox.Max.U - m_bbox.Min.U;
            double vRange = m_bbox.Max.V - m_bbox.Min.V;
            double uStep = uRange / m_density;
            double vStep = vRange / m_density;

            for (int u = 0; u <= m_density; u++)
            {
                double uPos = m_bbox.Min.U + (u * uStep);
                double uVal = (double)(u * (m_density - u));

                for (int v = 0; v <= m_density; v++)
                {
                    double vPos = m_bbox.Min.V + (v * vStep);
                    double vVal = (double)(v * (m_density - v));

                    UV point = new UV(uPos, vPos);
                    double value = Math.Min(uVal, vVal);

                    // We pretend the calculation of values is far more complicated
                    // while what we really do is taking a nap for a few milliseconds 

                    Thread.Sleep(100);

                    // If adding the result is not accepted it means the analysis
                    // have been interrupted and we are supposed to get out ASAP

                    if (!results.AddResult(point, value))
                    {
                        return;
                    }
                }

            }  // for
        }
        #endregion
    }  // class

}
