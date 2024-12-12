//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using Autodesk.Revit.DB.Plumbing;

namespace Revit.SDK.Samples.AvoidObstruction.CS
{
    /// <summary>
    /// This class presents an obstruction of a Pipe.
    /// </summary>
    class Section
    {   
        /// <summary>
        /// Pipe centerline's direction.
        /// </summary>
        private Autodesk.Revit.DB.XYZ  m_dir;

        /// <summary>
        /// Extend factor in negative direction.
        /// </summary>
        private double m_startFactor;

        /// <summary>
        /// Extend factor in positive direction.
        /// </summary>
        private double m_endFactor;

        /// <summary>
        /// References contained in this obstruction.
        /// </summary>
        private List<ReferenceWithContext> m_refs;

        /// <summary>
        /// Pipes to avoid this obstruction, it is assigned when resolving this obstruction.
        /// Its count will be three if resolved, the three pipe constructs a "U" shape to round the obstruction.
        /// </summary>
        private List<Pipe> m_pipes;        

        /// <summary>
        /// Private constructor, just be called in static factory method BuildSections.
        /// </summary>
        /// <param name="dir">Pipe's direction</param>
        private Section(Autodesk.Revit.DB.XYZ  dir)
        {
            m_dir = dir;
            m_startFactor = 0;
            m_endFactor = 0;
            m_refs = new List<ReferenceWithContext>();
            m_pipes = new List<Pipe>();
        }

        /// <summary>
        /// Pipe centerline's direction.
        /// </summary>
        public Autodesk.Revit.DB.XYZ  PipeCenterLineDirection
        {
            get { return m_dir; }
        }

        /// <summary>
        /// Pipes to avoid this obstruction, it is assigned when resolving this obstruction.
        /// Its count will be three if resolved, the three pipe constructs a "U" shape to round the obstruction.
        /// </summary>
        public List<Pipe> Pipes
        {
            get { return m_pipes; }
        }

        /// <summary>
        /// Start point of this obstruction.
        /// </summary>
        public Autodesk.Revit.DB.XYZ  Start
        {
            get
            {
                return m_refs[0].GetReference().GlobalPoint + m_dir * m_startFactor;
            }
        }

        /// <summary>
        /// End point of this obstruction.
        /// </summary>
        public Autodesk.Revit.DB.XYZ  End
        {
            get
            {
                return m_refs[m_refs.Count - 1].GetReference().GlobalPoint + m_dir * m_endFactor;
            }
        }

        /// <summary>
        /// References contained in this obstruction.
        /// </summary>
        public List<ReferenceWithContext> Refs
        {
            get { return m_refs; }
        }

        /// <summary>
        /// Extend this obstruction's interval in one direction.
        /// </summary>
        /// <param name="index">index of direction, 0 => start, 1 => end</param>
        public void Inflate(int index, double value)
        {
            if (index == 0)
            {
                m_startFactor -= value;
            }
            else if(index == 1)
            {
                m_endFactor += value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Index should be 0 or 1.");
            }            
        }

        /// <summary>
        /// Build sections for References, it's a factory method to build sections.
        /// A section contains several points through which the ray passes the obstruction(s). 
        /// for example, a section may contain 2 points when the obstruction is stand alone, 
        /// or contain 4 points if 2 obstructions are intersects with each other in the direction of the ray.
        /// </summary>
        /// <param name="allrefs">References</param>
        /// <param name="dir">Pipe's direction</param>
        /// <returns>List of Section</returns>
        public static List<Section> BuildSections(List<ReferenceWithContext> allrefs, Autodesk.Revit.DB.XYZ  dir)
        {
            List<ReferenceWithContext> buildStack = new List<ReferenceWithContext>();            
            List<Section> sections = new List<Section>();
            Section current = null;
            foreach (ReferenceWithContext geoRef in allrefs)
            {                
                if (buildStack.Count == 0)
                {
                    current = new Section(dir);
                    sections.Add(current);
                }

                current.Refs.Add(geoRef);

                ReferenceWithContext tmp = Find(buildStack, geoRef);
                if (tmp != null)
                {
                    buildStack.Remove(tmp);
                }
                else
                    buildStack.Add(geoRef);
            }

            return sections;
        }

        /// <summary>
        /// Judge whether a Reference is already in the list of Reference, return the founded value.
        /// </summary>
        /// <param name="arr">List of Reference</param>
        /// <param name="entry">Reference to test</param>
        /// <returns>One Reference has the same element's Id with entry</returns>
        private static ReferenceWithContext Find(List<ReferenceWithContext> arr, ReferenceWithContext entry)
        {
            foreach (ReferenceWithContext tmp in arr)
            {
                if (tmp.GetReference().ElementId == entry.GetReference().ElementId)
                {
                    return tmp;
                }
            }
            return null;
        }
    }
}
