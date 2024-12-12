//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit;
using Element = Autodesk.Revit.DB.Element;

namespace Revit.SDK.Samples.AvoidObstruction.CS
{
    /// <summary>
    /// This class is used to detect the obstructions of a Line or a ray.
    /// </summary>
    class Detector
    {
        /// <summary>
        /// Revit Document.
        /// </summary>
        private Document m_rvtDoc;

        /// <summary>
        /// Revit 3D view.
        /// </summary>
        private View3D m_view3d;

        /// <summary>
        /// Constructor, initialize all the fields.
        /// </summary>
        /// <param name="rvtDoc">Revit Document</param>
        public Detector(Document rvtDoc)
        {
            m_rvtDoc = rvtDoc;
            FilteredElementCollector collector = new FilteredElementCollector(m_rvtDoc);
            FilteredElementIterator iter = collector.OfClass(typeof(View3D)).GetElementIterator();
            iter.Reset();
            while (iter.MoveNext())
            {
                m_view3d = iter.Current as View3D;
                if (null != m_view3d && !m_view3d.IsTemplate)
                    break;
            }
        }

        /// <summary>
        /// Return all the obstructions which intersect with a ray given by an origin and a direction.
        /// </summary>
        /// <param name="origin">Ray's origin</param>
        /// <param name="dir">Ray's direction</param>
        /// <returns>Obstructions intersected with the given ray</returns>
        public List<ReferenceWithContext> Obstructions(Autodesk.Revit.DB.XYZ origin, Autodesk.Revit.DB.XYZ dir)
        {
            List<ReferenceWithContext> result = new List<ReferenceWithContext>();
            ReferenceIntersector referenceIntersector = new ReferenceIntersector(m_view3d);
            referenceIntersector.TargetType = FindReferenceTarget.Face;
            IList<ReferenceWithContext> obstructionsOnUnboundLine = referenceIntersector.Find(origin, dir);
            foreach (ReferenceWithContext gRef in obstructionsOnUnboundLine)
            {
                if (!InArray(result, gRef))
                {
                    result.Add(gRef);
                }
            }

            result.Sort(CompareReferencesWithContext);
            return result;
        }

        /// <summary>
        /// Return all the obstructions which intersect with a bound line.
        /// </summary>
        /// <param name="boundLine">Bound line</param>
        /// <returns>Obstructions intersected with the bound line</returns>
        public List<ReferenceWithContext> Obstructions(Line boundLine)
        {
            List<ReferenceWithContext> result = new List<ReferenceWithContext>();
            Autodesk.Revit.DB.XYZ startPt = boundLine.GetEndPoint(0);
            Autodesk.Revit.DB.XYZ endPt = boundLine.GetEndPoint(1);
            Autodesk.Revit.DB.XYZ dir = (endPt - startPt).Normalize();
            ReferenceIntersector referenceIntersector = new ReferenceIntersector(m_view3d);
            referenceIntersector.TargetType = FindReferenceTarget.Face;
            IList<ReferenceWithContext> obstructionsOnUnboundLine = referenceIntersector.Find(startPt, dir);
            foreach (ReferenceWithContext gRefWithContext in obstructionsOnUnboundLine)
            {
                Reference gRef = gRefWithContext.GetReference();
                // Judge whether the point is in the bound line or not, if the distance between the point and line
                // is Zero, then the point is in the bound line.
                if (boundLine.Distance(gRef.GlobalPoint) < 1e-9)
                {
                    if (!InArray(result, gRefWithContext))
                    {
                        result.Add(gRefWithContext);
                    }
                }
            }

            result.Sort(CompareReferencesWithContext);
            return result;
        }

        /// <summary>
        /// Judge whether a given Reference is in a Reference list.
        /// Give two References, if their Proximity and Element Id is equal, 
        /// we say the two reference is equal.
        /// </summary>
        /// <param name="arr">Reference Array</param>
        /// <param name="entry">Reference</param>
        /// <returns>True of false</returns>
        private bool InArray(List<ReferenceWithContext> arr, ReferenceWithContext entry)
        {
            foreach (ReferenceWithContext tmp in arr)
            {
                if (Math.Abs(tmp.Proximity - entry.Proximity) < 1e-9 &&
                    tmp.GetReference().ElementId == entry.GetReference().ElementId)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Used to compare two references, just compare their ProximityParameter.
        /// </summary>
        /// <param name="a">First Reference to compare</param>
        /// <param name="b">Second Reference to compare</param>
        /// <returns>-1, 0, or 1</returns>
        private int CompareReferencesWithContext(ReferenceWithContext a, ReferenceWithContext b)
        {
            if (a.Proximity > b.Proximity)
            {
                return 1;
            }

            if (a.Proximity < b.Proximity)
            {
                return -1;
            }

            return 0;
        }
    }
}
