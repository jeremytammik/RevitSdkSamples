//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Structural.Enums;

namespace Revit.SDK.Samples.Reinforcement.CS
{
    // using GeoInstance as Autodesk.Revit.Geometry.Instance
    using GeoInstance = Autodesk.Revit.Geometry.Instance;

    /// <summary>
    /// The base class which support beamGeometrySupport and ColumnGeometrySupport etc.
    /// it store some common geometry information, and give some helper fuctions
    /// </summary>
    public class GeometrySupport
    {
        /// <summary>
        /// store the solid of beam or column
        /// </summary>
        protected Solid m_solid;

        /// <summary>
        /// the extend or sweep path of the beam or column
        /// </summary>
        protected Line m_drivingLine;

        /// <summary>
        /// the director vector of beam or column
        /// </summary>
        protected XYZ m_drivingVector;

        /// <summary>
        /// a list to store the edges 
        /// </summary>
        protected List<Line> m_edges = new List<Line>();

        /// <summary>
        /// a list to store the point
        /// </summary>
        protected List<XYZ> m_points = new List<XYZ>();

        /// <summary>
        /// the transform value of the solid
        /// </summary>
        protected Transform m_transform;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="element">the host object, must be family instance</param>
        /// <param name="geoOptions">the geometry option</param>
        public GeometrySupport(FamilyInstance element, Options geoOptions)
        {
            // get the geometry element of the selected element
            Autodesk.Revit.Geometry.Element geoElement = element.get_Geometry(geoOptions);
            if (null == geoElement || 0 == geoElement.Objects.Size)
            {
                throw new Exception("Can't get the gometry of selected element.");
            }

            //get the geometry object
            foreach (GeometryObject geoObject in geoElement.Objects)
            {
                // get the driving path and vector of the beam or column
                Line line = geoObject as Line;
                if (null != line)
                {
                    m_drivingLine = line;   // driving path
                    m_drivingVector = GeomUtil.SubXYZ(line.get_EndPoint(1), line.get_EndPoint(0));
                    continue;
                }

                //get the geometry instance which contain the geometry information
                GeoInstance instance = geoObject as GeoInstance;
                if (null != instance)
                {
                    foreach (GeometryObject o in instance.SymbolGeometry.Objects)
                    {
                        // get the solid of beam of column
                        Solid solid = o as Solid;

                        // do some checks.
                        if (null == solid)
                        {
                            continue;
                        }
                        if (0 == solid.Faces.Size || 0 == solid.Edges.Size)
                        {
                            continue;
                        }

                        m_solid = solid;
                        //get the transform value of instance
                        m_transform = instance.Transform;

                        // Get the swept profile curves information
                        if (!GetSweptProfile(solid))
                        {
                            throw new Exception("Can't get the swept profile curves.");
                        }
                        break;
                    }
                }

            }

            // do some checks about profile curves information
            if (null == m_edges)
            {
                throw new Exception("Can't get the geometry edge information.");
            }
            if (4 != m_points.Count)
            {
                throw new Exception("The sample only work for rectangle beam or column.");
            }
        }


        /// <summary>
        /// transform the point to new coordinates
        /// </summary>
        /// <param name="point">the point need to transform</param>
        /// <returns>the changed point</returns>
        protected XYZ Transform(XYZ point)
        {
            // only invoke the TransformPoint() method.
            return GeomUtil.TransformPoint(point, m_transform);
        }


        /// <summary>
        /// Get the length of driving line
        /// </summary>
        /// <returns>the length of the driving line</returns>
        protected double GetDrivingLineLength()
        {
            return GeomUtil.GetLength(m_drivingVector);
        }

        /// <summary>
        /// Get two vectors, which indicate some edge direction which contain given point, 
        /// set the given point as the start point, the other end point of the edge as end
        /// </summary>
        /// <param name="point">a point of the swept profile</param>
        /// <returns>two vectors indicate edge direction</returns>
        protected List<XYZ> GetRelatedVectors(XYZ point)
        {
            // Initialize the return vector list.
            List<XYZ> vectors = new List<XYZ>();

            // Get all the edge which contain this point.
            // And get the vector from this point to another point
            foreach (Line line in m_edges)
            {
                if (GeomUtil.IsEqual(point, line.get_EndPoint(0)))
                {
                    XYZ vector = GeomUtil.SubXYZ(line.get_EndPoint(1), line.get_EndPoint(0));
                    vectors.Add(vector);
                }
                if (GeomUtil.IsEqual(point, line.get_EndPoint(1)))
                {
                    XYZ vector = GeomUtil.SubXYZ(line.get_EndPoint(0), line.get_EndPoint(1));
                    vectors.Add(vector);
                }
            }

            // only two vector(direction) should be found
            if (2 != vectors.Count)
            {
                throw new Exception("a point on swept profile should have only two direction.");
            }

            return vectors;
        }


        /// <summary>
        /// Offset the points of the swept profile to make the points inside swept profile
        /// </summary>
        /// <param name="offset">indicate how long to offset on two directions</param>
        /// <returns>the offset points</returns>
        protected List<XYZ> OffsetPoints(double offset)
        {
            // Initialize the offset point list.
            List<XYZ> points = new List<XYZ>();

            // Get all points of the swept profile, and offset it in two related direction
            foreach (XYZ point in m_points)
            {
                // Get two related directions
                List<XYZ> directions = GetRelatedVectors(point);
                XYZ firstDir = directions[0];
                XYZ secondDir = directions[1];

                // offset the point in two direction
                XYZ movedPoint = GeomUtil.OffsetPoint(point, firstDir, offset);
                movedPoint = GeomUtil.OffsetPoint(movedPoint, secondDir, offset);

                // add the offset point into the array
                points.Add(movedPoint);
            }

            return points;
        }


        /// <summary>
        /// Find the inforamtion of the swept profile(face), 
        /// and store the points and edges of the profile(face) 
        /// </summary>
        /// <param name="solid">the solid reference</param>
        /// <returns>true if the swept profile can be gotten, otherwise false</returns>
        private bool GetSweptProfile(Solid solid)
        {
            // get the swept face
            Face sweptFace = GetSweptProfileFace(solid);
            // do some checks
            if (null == sweptFace || 1 != sweptFace.EdgeLoops.Size)
            {
                return false;
            }

            // get the points of the swept face
            foreach (XYZ point in sweptFace.Triangulate().Vertices)
            {
                m_points.Add(Transform(point));
            }

            // get the edges of the swept face
            m_edges = ChangeEdgeToLine(sweptFace.EdgeLoops.get_Item(0));

            // do some checks
            return (null != m_edges);
        }

        /// <summary>
        /// Get the swept profile(face) of the host object(family instance)
        /// </summary>
        /// <param name="solid">the solid reference</param>
        /// <returns>the swept profile</returns>
        private Face GetSweptProfileFace(Solid solid)
        {
            // Get a point on the swept profile from all points in solid
            XYZ refPoint = new XYZ();   // the point on swept profile
            foreach (Edge edge in solid.Edges)
            {
                XYZArray points = edge.Tessellate();    //get end points of the edge
                if (2 != points.Size)                   // make sure all edges are lines
                {
                    throw new Exception("All edge should be line.");
                }

                // get two points of the edge. All points in solid should be transform first
                XYZ first = Transform(points.get_Item(0));  // start point of edge
                XYZ second = Transform(points.get_Item(1)); // end point of edge

                // some edges should be parallelled with the driving line,
                // and the start point of that edge should be the wanted point
                XYZ edgeVector = GeomUtil.SubXYZ(second, first);
                if (GeomUtil.IsSameDirection(edgeVector, m_drivingVector))
                {
                    refPoint = first;
                    break;
                }
                if (GeomUtil.IsOppositeDirection(edgeVector, m_drivingVector))
                {
                    refPoint = second;
                    break;
                }
            }

            // Find swept profile(face)
            Face sweptFace = null;  // define the swept face
            foreach (Face face in solid.Faces)
            {
                if (null != sweptFace)
                {
                    break;
                }
                // the swept face should be perpendicular with the driving line
                if (!GeomUtil.IsVertical(face, m_drivingLine, m_transform, null))
                {
                    continue;
                }
                // use the gotted point to get the swept face
                foreach (XYZ point in face.Triangulate().Vertices)
                {
                    XYZ pnt = Transform(point); // all points in solid should be transform
                    if (GeomUtil.IsEqual(refPoint, pnt))
                    {
                        sweptFace = face;
                        break;
                    }
                }
            }

            return sweptFace;
        }

        /// <summary>
        /// Change the swept profile edges from EdgeArray type to line list
        /// </summary>
        /// <param name="edges">the swept profile edges</param>
        /// <returns>the line list which stores the swept profile edges</returns>
        private List<Line> ChangeEdgeToLine(EdgeArray edges)
        {
            // create the line list instance.
            List<Line> edgeLines = new List<Line>();

            // get each edge from swept profile,
            // and changed the geometry information in line list
            foreach (Edge edge in edges)
            {
                //get the two points of each edge
                XYZArray points = edge.Tessellate();
                XYZ first = Transform(points.get_Item(0));
                XYZ second = Transform(points.get_Item(1));

                // create new line and add them into line list
                edgeLines.Add(Line.get_Bound(ref first, ref second));
            }

            return edgeLines;
        }
    }
}
