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
using Autodesk.Revit.MEP;
using Autodesk.Revit.Geometry;
using Autodesk.Revit;
using System.Diagnostics;
using Autodesk.Revit.Enums;
using Element = Autodesk.Revit.Element;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using System.Collections;

namespace Revit.SDK.Samples.AvoidObstruction.CS
{
    /// <summary>
    /// This class implement the algorithm to detect the obstruction and resolve it.
    /// </summary>
    class Resolver
    {
        /// <summary>
        /// Revit Document.
        /// </summary>
        private Document m_rvtDoc;

        /// <summary>
        /// Revit Application.
        /// </summary>
        private Application m_rvtApp;

        /// <summary>
        /// Detector to detect the obstructions.
        /// </summary>
        private Detector m_detector;

        /// <summary>
        /// Constructor, initialize all the fields of this class.
        /// </summary>
        /// <param name="data">Revit ExternalCommandData from external command entrance</param>
        public Resolver(ExternalCommandData data)
        {
            m_rvtDoc = data.Application.ActiveDocument;
            m_rvtApp = data.Application;
            m_detector = new Detector(m_rvtDoc);
        }

        /// <summary>
        /// Detect and resolve the obstructions of all Pipes.
        /// </summary>
        public void Resolve()
        {
            List<Element> pipes = new List<Element>();
            m_rvtDoc.get_Elements(typeof(Pipe), pipes);

            foreach (Element pipe in pipes)
            {
                Resolve(pipe as Pipe);
            }
        }

        /// <summary>
        /// Calculate the uniform perpendicular directions with inputting direction "dir".
        /// </summary>
        /// <param name="dir">Direction to calculate</param>
        /// <param name="count">How many perpendicular directions will be calculated</param>
        /// <returns>The calculated perpendicular directions with dir</returns>
        private List<XYZ> PerpendicularDirs(XYZ dir, int count)
        {
            List<XYZ> dirs = new List<XYZ>();
            Plane plane = m_rvtApp.Create.NewPlane(dir, XYZ.Zero);
            Arc arc = m_rvtApp.Create.NewArc(plane, 1.0, 0, 6.28);

            double delta = 1.0 / (double)count;
            for (int i = 1; i <= count; i++)
            {
                XYZ pt = arc.Evaluate(delta * i, true);
                dirs.Add(pt);
            }

            return dirs;
        }

        /// <summary>
        /// Detect the obstructions of pipe and resolve them.
        /// </summary>
        /// <param name="pipe">Pipe to resolve</param>
        private void Resolve(Pipe pipe)
        {
            // Get the centerline of pipe.
            Line pipeLine = (pipe.Location as LocationCurve).Curve as Line;

            // Calculate the intersection references with pipe's centerline.
            List<Reference> obstructionRefArr = m_detector.Obstructions(pipeLine);

            // Filter out the references, just allow Pipe, Beam, and Duct.
            Filter(pipe, obstructionRefArr);

            if (obstructionRefArr.Count == 0)
            {
                // There are no intersection found.
                return;
            }

            // Calculate the direction of pipe's centerline.
            XYZ dir = pipeLine.get_EndPoint(1) - pipeLine.get_EndPoint(0);

            // Build the sections from the intersection references.
            List<Section> sections = Section.BuildSections(obstructionRefArr, dir.Normalized);

            // Merge the neighbor sections if the distance of them is too close.
            for (int i = sections.Count - 2; i >= 0; i--)
            {
                XYZ detal = sections[i].End - sections[i + 1].Start;
                if (detal.Length < pipe.Diameter * 3)
                {
                    sections[i].Refs.AddRange(sections[i + 1].Refs);
                    sections.RemoveAt(i + 1);
                }
            }

            // Resolve the obstructions one by one.
            foreach (Section sec in sections)
            {
                Resolve(pipe, sec);
            }

            // Connect the neighbor sections with pipe and elbow fittings.
            //
            for (int i = 1; i < sections.Count; i++)
            {
                // Get the end point from the previous section.
                XYZ start = sections[i - 1].End;

                // Get the start point from the current section.
                XYZ end = sections[i].Start;

                // Create a pipe between two neighbor section.
                Pipe tmpPipe = m_rvtDoc.Create.NewPipe(start, end, pipe.PipeType);

                // Copy pipe's parameters values to tmpPipe.
                CopyParameters(pipe, tmpPipe);

                // Create elbow fitting to connect previous section with tmpPipe.
                Connector conn1 = FindConnector(sections[i - 1].Pipes[2], start);
                Connector conn2 = FindConnector(tmpPipe, start);
                FamilyInstance fi = m_rvtDoc.Create.NewElbowFitting(conn1, conn2);

                // Create elbow fitting to connect current section with tmpPipe.
                Connector conn3 = FindConnector(sections[i].Pipes[0], end);
                Connector conn4 = FindConnector(tmpPipe, end);
                FamilyInstance f2 = m_rvtDoc.Create.NewElbowFitting(conn3, conn4);
            }

            // Find two connectors which pipe's two ends connector connected to. 
            Connector startConn = FindConnectedTo(pipe, pipeLine.get_EndPoint(0));
            Connector endConn = FindConnectedTo(pipe, pipeLine.get_EndPoint(1));
            
            Pipe startPipe = null;
            if (null != startConn)
            {
                // Create a pipe between pipe's start connector and pipe's start section.
                startPipe = m_rvtDoc.Create.NewPipe(sections[0].Start, startConn, pipe.PipeType);
            }
            else
            {
                // Create a pipe between pipe's start point and pipe's start section.
                startPipe = m_rvtDoc.Create.NewPipe(sections[0].Start, pipeLine.get_EndPoint(0), pipe.PipeType);
            }

            // Copy parameters from pipe to startPipe. 
            CopyParameters(pipe, startPipe);

            // Connect the startPipe and first section with elbow fitting.
            Connector connStart1 = FindConnector(startPipe, sections[0].Start);
            Connector connStart2 = FindConnector(sections[0].Pipes[0], sections[0].Start);
            FamilyInstance fii = m_rvtDoc.Create.NewElbowFitting(connStart1, connStart2);

            Pipe endPipe = null;
            int count = sections.Count;
            if (null != endConn)
            {
                // Create a pipe between pipe's end connector and pipe's end section.
                endPipe = m_rvtDoc.Create.NewPipe(sections[count - 1].End, endConn, pipe.PipeType);
            }
            else
            {
                // Create a pipe between pipe's end point and pipe's end section.
                endPipe = m_rvtDoc.Create.NewPipe(sections[count - 1].End, pipeLine.get_EndPoint(1), pipe.PipeType);
            }

            // Copy parameters from pipe to endPipe.
            CopyParameters(pipe, endPipe);

            // Connect the endPipe and last section with elbow fitting.
            Connector connEnd1 = FindConnector(endPipe, sections[count - 1].End);
            Connector connEnd2 = FindConnector(sections[count - 1].Pipes[2], sections[count - 1].End);
            FamilyInstance fiii = m_rvtDoc.Create.NewElbowFitting(connEnd1, connEnd2);

            // Delete the pipe after resolved.
            m_rvtDoc.Delete(pipe);
        }

        /// <summary>
        /// Filter the inputting References, just allow Pipe, Duct and Beam References.
        /// </summary>
        /// <param name="pipe">Pipe</param>
        /// <param name="refs">References to filter</param>
        private void Filter(Pipe pipe, List<Reference> refs)
        {
            for (int i = refs.Count - 1; i >= 0; i--)
            {
                Reference cur = refs[i];
                if (cur.Element.Id.Value == pipe.Id.Value ||
                (!(cur.Element is Pipe) && !(cur.Element is Duct) &&
                cur.Element.Category.Id.Value != (int)BuiltInCategory.OST_StructuralFraming))
                {
                    refs.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// This method will find out a route to avoid the obstruction. 
        /// </summary>
        /// <param name="pipe">Pipe to resolve</param>
        /// <param name="section">Pipe's one obstruction</param>
        /// <returns>A route which can avoid the obstruction</returns>
        private Line FindRoute(Pipe pipe, Section section)
        {
            // Perpendicular direction minimal length.
            double minLength = pipe.Diameter * 2;
            
            // Parallel direction jump step. 
            double jumpStep = pipe.Diameter;

            // Calculate the directions in which to find the solution.
            List<XYZ> dirs = new List<XYZ>();
            XYZ crossDir = null;
            foreach (Reference gref in section.Refs)
            {
                Line locationLine = (gref.Element.Location as LocationCurve).Curve as Line;
                XYZ refDir = locationLine.get_EndPoint(1) - locationLine.get_EndPoint(0);
                refDir = refDir.Normalized;
                if (refDir.AlmostEqual(section.PipeCenterLineDirection) || refDir.AlmostEqual(-section.PipeCenterLineDirection))
                {
                    continue;
                }
                crossDir = refDir.Cross(section.PipeCenterLineDirection);
                dirs.Add(crossDir.Normalized);
                break;              
            }
            
            // When all the obstruction are parallel with the centerline of the pipe,
            // We can't calculate the direction from the vector.Cross method.
            if (dirs.Count == 0)
            {
                // Calculate perpendicular directions with dir in four directions.
                List<XYZ> perDirs = PerpendicularDirs(section.PipeCenterLineDirection, 4);
                dirs.Add(perDirs[0]);
                dirs.Add(perDirs[1]);
            }

            Line foundLine = null;
            while (null == foundLine)
            {        
                // Extend the section interval by jumpStep.
                section.Inflate(0, jumpStep);
                section.Inflate(1, jumpStep);

                // Find solution in the given directions.
                for (int i = 0; null == foundLine && i < dirs.Count; i++)
                {
                    // Calculate the intersections.
                    List<Reference> obs1 = m_detector.Obstructions(section.Start, dirs[i]);
                    List<Reference> obs2 = m_detector.Obstructions(section.End, dirs[i]);

                    // Filter out the intersection result.
                    Filter(pipe, obs1);
                    Filter(pipe, obs2);

                    // Find out the minimal intersections in two opposite direction.
                    Reference[] mins1 = GetClosestSectionsToOrigin(obs1);
                    Reference[] mins2 = GetClosestSectionsToOrigin(obs2);

                    // Find solution in the given direction and its opposite direction.
                    for (int j = 0; null == foundLine && j < 2; j++)
                    {
                        if (mins1[j] != null && Math.Abs(mins1[j].ProximityParameter) < minLength ||
                            mins2[j] != null && Math.Abs(mins2[j].ProximityParameter) < minLength)
                        {
                            continue;
                        }

                        // Calculate the maximal height that the parallel line can be reached.
                        double maxHight = 1000 * pipe.Diameter;
                        if (mins1[j] != null && mins2[j] != null)                            
                        {
                            maxHight = Math.Min(Math.Abs(mins1[j].ProximityParameter), Math.Abs(mins2[j].ProximityParameter));
                        }
                        else if(mins1[j] != null)
                        {
                            maxHight = Math.Abs(mins1[j].ProximityParameter);
                        }
                        else if (mins2[j] != null)
                        {
                            maxHight = Math.Abs(mins2[j].ProximityParameter);
                        }

                        XYZ dir = (j == 1) ? dirs[i] : -dirs[i];

                        // Calculate the parallel line which can avoid obstructions.
                        foundLine = FindParallelLine(pipe, section, dir, maxHight);
                    }
                }
            }
            return foundLine;
        }

        /// <summary>
        /// Find a line Parallel to pipe's centerline to avoid the obstruction.
        /// </summary>
        /// <param name="pipe">Pipe who has obstructions</param>
        /// <param name="section">Pipe's one obstruction</param>
        /// <param name="dir">Offset Direction of the parallel line</param>
        /// <param name="maxLength">Maximum offset distance</param>
        /// <returns>Parallel line which can avoid the obstruction</returns>
        private Line FindParallelLine(Pipe pipe, Section section, XYZ dir, double maxLength)
        {
            double step = pipe.Diameter;
            double hight = 2 * pipe.Diameter;
            while (hight <= maxLength)
            {                
                XYZ detal = dir * hight;
                Line line = Line.get_Bound(section.Start + detal, section.End + detal);
                List<Reference> refs = m_detector.Obstructions(line);
                Filter(pipe, refs);

                if (refs.Count == 0)
                {
                    return line;
                }
                hight += step;
            }
            return null;
        }

        /// <summary>
        /// Find out two References, whose ProximityParameter is negative or positive,
        /// And Get the minimal value from all positive reference, and get the maximal value 
        /// from the negative reference. if there are no such reference, using null instead.
        /// </summary>
        /// <param name="refs">References</param>
        /// <returns>Reference array</returns>
        private Reference[] GetClosestSectionsToOrigin(List<Reference> refs)
        {
            Reference[] mins = new Reference[2];
            if (refs.Count == 0)
            {
                return mins;
            }

            if (refs[0].ProximityParameter > 0)
            {
                mins[1] = refs[0];
                return mins;
            }

            for (int i = 0; i < refs.Count - 1; i++)
            {
                if (refs[i].ProximityParameter < 0 && refs[i + 1].ProximityParameter > 0)
                {
                    mins[0] = refs[i];
                    mins[1] = refs[i + 1];
                    return mins;
                }
            }

            mins[0] = refs[refs.Count - 1];

            return mins;
        }


        /// <summary>
        /// Resolve one obstruction of Pipe.
        /// </summary>
        /// <param name="pipe">Pipe to resolve</param>
        /// <param name="section">One pipe's obstruction</param>
        private void Resolve(Pipe pipe, Section section)
        {           
            // Find out a parallel line of pipe centerline, which can avoid the obstruction.
            Line offset = FindRoute(pipe, section);

            // Construct a section line according to the given section.
            Line sectionLine = Line.get_Bound(section.Start, section.End);

            // Construct two side lines, which can avoid the obstruction too.
            Line side1 = Line.get_Bound(sectionLine.get_EndPoint(0), offset.get_EndPoint(0));
            Line side2 = Line.get_Bound(offset.get_EndPoint(1), sectionLine.get_EndPoint(1));

            //
            // Create an "U" shape, which connected with three pipes and two elbows, to round the obstruction.
            //
            PipeType pipeType = pipe.PipeType;
            XYZ start = side1.get_EndPoint(0);
            XYZ startOffset = offset.get_EndPoint(0);
            XYZ endOffset = offset.get_EndPoint(1);
            XYZ end = side2.get_EndPoint(1);

            // Create three side pipes of "U" shape.
            Pipe pipe1 = m_rvtDoc.Create.NewPipe(start, startOffset, pipeType);
            Pipe pipe2 = m_rvtDoc.Create.NewPipe(startOffset, endOffset, pipeType);
            Pipe pipe3 = m_rvtDoc.Create.NewPipe(endOffset, end, pipeType);

            // Copy parameters from pipe to other three created pipes.
            CopyParameters(pipe, pipe1);
            CopyParameters(pipe, pipe2);
            CopyParameters(pipe, pipe3);

            // Add the created three pipes to current section.
            section.Pipes.Add(pipe1);
            section.Pipes.Add(pipe2);
            section.Pipes.Add(pipe3);

            // Create the first elbow to connect two neighbor pipes of "U" shape.
            Connector conn1 = FindConnector(pipe1, startOffset);
            Connector conn2 = FindConnector(pipe2, startOffset);
            m_rvtDoc.Create.NewElbowFitting(conn1, conn2);

            // Create the second elbow to connect another two neighbor pipes of "U" shape.
            Connector conn3 = FindConnector(pipe2, endOffset);
            Connector conn4 = FindConnector(pipe3, endOffset);
            m_rvtDoc.Create.NewElbowFitting(conn3, conn4);
        }

        /// <summary>
        /// Copy parameters from source pipe to target pipe.
        /// </summary>
        /// <param name="sorce">Coping source</param>
        /// <param name="target">Coping target</param>
        private void CopyParameters(Pipe sorce, Pipe target)
        {
            double diameter = sorce.get_Parameter(BuiltInParameter.RBS_PIPE_DIAMETER_PARAM).AsDouble();
            target.get_Parameter(BuiltInParameter.RBS_PIPE_DIAMETER_PARAM).Set(diameter);
        }

        /// <summary>
        /// Find out a connector from pipe with a specified point.
        /// </summary>
        /// <param name="pipe">Pipe to find the connector</param>
        /// <param name="conXYZ">Specified point</param>
        /// <returns>Connector whose origin is conXYZ</returns>
        private Connector FindConnector(Pipe pipe, XYZ conXYZ)
        {
            ConnectorSet conns = pipe.ConnectorManager.Connectors;
            foreach (Connector conn in conns)
            {
                if (conn.Origin.AlmostEqual(conXYZ))
                {
                    return conn;
                }
            }
            return null;
        }

        /// <summary>
        /// Find out the connector which the pipe's specified connector connected to.
        /// The pipe's specified connector is given by point conXYZ.
        /// </summary>
        /// <param name="pipe">Pipe to find the connector</param>
        /// <param name="conXYZ">Specified point</param>
        /// <returns>Connector whose origin is conXYZ</returns>
        private Connector FindConnectedTo(Pipe pipe, XYZ conXYZ)
        {
            Connector connItself = FindConnector(pipe, conXYZ);
            ConnectorSet connSet = connItself.AllRefs;
            foreach (Connector conn in connSet)
            {
                if (conn.Owner.Id.Value != pipe.Id.Value &&
                    conn.ConnectorType == Autodesk.Revit.MEP.Enums.ConnectorType.EndConn)
                {
                    return conn;
                }
            }
            return null;
        } 
    }
}
