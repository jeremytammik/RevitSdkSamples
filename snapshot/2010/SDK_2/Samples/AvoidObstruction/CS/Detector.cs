//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using Autodesk.Revit.Geometry;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Enums;
using Autodesk.Revit.MEP;
using Element = Autodesk.Revit.Element;

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
            List<Element> views = new List<Element>();
            m_rvtDoc.get_Elements(typeof(View3D), views);
            m_view3d = views[0] as View3D;
        }

        /// <summary>
        /// Return all the obstructions which intersect with a ray given by an origin and a direction.
        /// </summary>
        /// <param name="origin">Ray's origin</param>
        /// <param name="dir">Ray's direction</param>
        /// <returns>Obstructions intersected with the given ray</returns>
        public List<Reference> Obstructions(XYZ origin, XYZ dir)
        {
            List<Reference> result = new List<Reference>();    
            ReferenceArray obstructionsOnUnboundLine = m_rvtDoc.FindReferencesByDirection(origin, dir, m_view3d);
            foreach (Reference gRef in obstructionsOnUnboundLine)
            {
                if (gRef.ElementReferenceType == ElementReferenceType.REFERENCE_TYPE_SURFACE)
                {
                    if (!InArray(result, gRef))
                    {
                        result.Add(gRef);
                    }
                }
            }

            result.Sort(CompareReferences);
            return result;
        }

        /// <summary>
        /// Return all the obstructions which intersect with a bound line.
        /// </summary>
        /// <param name="boundLine">Bound line</param>
        /// <returns>Obstructions intersected with the bound line</returns>
        public List<Reference> Obstructions(Line boundLine)
        {
            List<Reference> result = new List<Reference>();            
            XYZ startPt = boundLine.get_EndPoint(0);
            XYZ endPt = boundLine.get_EndPoint(1);
            XYZ dir = (endPt - startPt).Normalized;
            ReferenceArray obstructionsOnUnboundLine = m_rvtDoc.FindReferencesByDirection(startPt, dir, m_view3d);
            foreach (Reference gRef in obstructionsOnUnboundLine)
            {
                // Judge whether the point is in the bound line or not, if the distance between the point and line
                // is Zero, then the point is in the bound line.
                if (boundLine.Distance(gRef.GlobalPoint) < 1e-9 &&
                    gRef.ElementReferenceType == ElementReferenceType.REFERENCE_TYPE_SURFACE)
                {
                    if (!InArray(result, gRef))
                    {
                        result.Add(gRef);
                    }
                }
            }

            result.Sort(CompareReferences);
            return result;
        }      

        /// <summary>
        /// Judge whether a given Reference is in a Reference list.
        /// Give two References, if their ProximityParameter and Element Id is equal, 
        /// we say the two reference is equal.
        /// </summary>
        /// <param name="arr">Reference Array</param>
        /// <param name="entry">Reference</param>
        /// <returns>True of false</returns>
        private bool InArray(List<Reference> arr, Reference entry)
        {
            foreach (Reference tmp in arr)
            {
                if (tmp.ProximityParameter == entry.ProximityParameter &&
                    tmp.Element.Id.Value == entry.Element.Id.Value)
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
        private int CompareReferences(Reference a, Reference b)
        {
            if (a.ProximityParameter > b.ProximityParameter)
            {
                return 1;
            }

            if (a.ProximityParameter < b.ProximityParameter)
            {
                return -1;
            }

            return 0;
        }
    }
}
