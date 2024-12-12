//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.Reinforcement.CS
{
    /// <summary>
    /// The geometry support for reinforcement creation on beam.
    /// It can prepare the geometry information for top rebar, bottom and transverse rebar creation
    /// </summary>
    public class BeamGeometrySupport : GeometrySupport
    {
        // Private members
        double m_beamLength; //the length of the beam
        double m_beamWidth;  //the width of the beam
        double m_beamHeight; //the height of the beam

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="element">the beam which the rebars are placed on</param>
        /// <param name="geoOptions">the geometry option</param>
        public BeamGeometrySupport(FamilyInstance element, Options geoOptions)
            : base(element, geoOptions)
        {
            // assert the host element is a beam
            if (!element.StructuralType.Equals(StructuralType.Beam))
            {
                throw new Exception("BeamGeometrySupport can only work for beam instance.");
            }

            // Get the length, width and height of the beam.
            m_beamLength = GetDrivingLineLength();
            m_beamWidth = GetBeamWidth();
            m_beamHeight = GetBeamHeight();
        }

        /// <summary>
        /// Get the geometry information for top rebar
        /// </summary>
        /// <param name="location">indicate where top rebar is placed</param>
        /// <returns>the gotten geometry information</returns>
        public RebarGeometry GetTopRebar(TopRebarLocation location)
        {
            // sort the points of the swept profile
            XYZHeightComparer comparer = new XYZHeightComparer();
            m_points.Sort(comparer);

            // Get the normal parameter for rebar creation
            List<Autodesk.Revit.DB.XYZ > directions = GetRelatedVectors(m_points[3]);
            directions.Sort(comparer);
            Autodesk.Revit.DB.XYZ normal = directions[1];

            double offset = 0;      //the offset from the beam surface to the rebar
            double startPointOffset = 0;    // the offset of start point from swept profile
            double rebarLength = m_beamLength / 3; //the length of the rebar
            int rebarNumber = BeamRebarData.TopRebarNumber; //the number of the rebar

            // set offset and startPointOffset according to the location of rebar
            switch (location)
            {
                case TopRebarLocation.Start:    // top start rebar
                    offset = BeamRebarData.TopEndOffset;
                    break;
                case TopRebarLocation.Center:   // top center rebar
                    offset = BeamRebarData.TopCenterOffset;
                    startPointOffset = m_beamLength / 3 - 0.5;
                    rebarLength = m_beamLength / 3 + 1;
                    break;
                case TopRebarLocation.End:      // top end rebar
                    offset = BeamRebarData.TopEndOffset;
                    startPointOffset = m_beamLength * 2 / 3;
                    break;
                default:
                    throw new Exception("The program should never go here.");
            }

            // Get the curve which define the shape of the top rebar curve
            List<Autodesk.Revit.DB.XYZ > movedPoints = OffsetPoints(offset);
            Autodesk.Revit.DB.XYZ startPoint = movedPoints[movedPoints.Count - 1];

            // offset the start point according startPointOffset
            startPoint = GeomUtil.OffsetPoint(startPoint, m_drivingVector, startPointOffset);
            // get the coordinate of endpoint 
            Autodesk.Revit.DB.XYZ endPoint = GeomUtil.OffsetPoint(startPoint, m_drivingVector, rebarLength);
            IList<Curve> curves = new List<Curve>(); //the profile of the top rebar
            curves.Add(Line.get_Bound(startPoint, endPoint));

            // the spacing of the rebar
            double spacing = spacing = (m_beamWidth - 2 * offset) / (rebarNumber - 1);

            // return the rebar geometry information
            return new RebarGeometry(normal, curves, rebarNumber, spacing);
        }

        /// <summary>
        /// Get the geometry information of bottom rebar
        /// </summary>
        /// <returns>the gotten geometry information</returns>
        public RebarGeometry GetBottomRebar()
        {
            // sort the points of the swept profile
            XYZHeightComparer comparer = new XYZHeightComparer();
            m_points.Sort(comparer);

            // Get the normal parameter for bottom rebar creation
            List<Autodesk.Revit.DB.XYZ > directions = GetRelatedVectors(m_points[0]);
            directions.Sort(comparer);
            Autodesk.Revit.DB.XYZ normal = directions[0];

            double offset = BeamRebarData.BottomOffset;     //offset value of the rebar
            int rebarNumber = BeamRebarData.BottomRebarNumber; //the number of the rebar
            // the spacing of the rebar
            double spacing = (m_beamWidth - 2 * offset) / (rebarNumber - 1);

            // Get the curve which define the shape of the bottom rebar curve
            List<Autodesk.Revit.DB.XYZ > movedPoints = OffsetPoints(offset);
            Autodesk.Revit.DB.XYZ startPoint = movedPoints[0]; //get the coordinate of startpoint 
            //get the coordinate of endpoint  
            Autodesk.Revit.DB.XYZ endPoint = GeomUtil.OffsetPoint(startPoint, m_drivingVector, m_beamLength);

            IList<Curve> curves = new List<Curve>(); //the profile of the bottom rebar
            curves.Add(Line.get_Bound(startPoint, endPoint));

            // return the rebar geometry information
            return new RebarGeometry(normal, curves, rebarNumber, spacing);
        }

        /// <summary>
        /// Get the geometry information of transverse rebar
        /// </summary>
        /// <param name="location">indicate which part of transverse rebar</param>
        /// <param name="spacing">the spacing of the rebar</param>
        /// <returns>the gotten geometry information</returns>
        public RebarGeometry GetTransverseRebar(TransverseRebarLocation location, double spacing)
        {
            // sort the points of the swept profile
            XYZHeightComparer comparer = new XYZHeightComparer();
            m_points.Sort(comparer);

            // the offset from the beam surface to the rebar
            double offset = BeamRebarData.TransverseOffset;
            // the offset from the beam end to the transverse end
            double endOffset = BeamRebarData.TransverseEndOffset;
            // the offset between two transverses
            double betweenOffset = BeamRebarData.TransverseSpaceBetween;
            // the length of the transverse rebar
            double rebarLength = (m_beamLength - 2 * endOffset - 2 * betweenOffset) / 3;
            // the number of the transverse rebar
            int rebarNumber = (int)(rebarLength / spacing) + 1;

            // get the origin and normal parameter for rebar creation
            Autodesk.Revit.DB.XYZ normal = m_drivingVector;
            double curveOffset = 0;

            //judge the coordinate of transverse rebar according to the location
            switch (location)
            {
                case TransverseRebarLocation.Start:     // start transverse rebar
                    curveOffset = endOffset;
                    break;
                case TransverseRebarLocation.Center:    // center transverse rebar
                    curveOffset = endOffset + rebarLength + betweenOffset;
                    curveOffset = curveOffset + (rebarLength % spacing) / 2;
                    break;
                case TransverseRebarLocation.End:       // end transverse rebar
                    curveOffset = m_beamLength - endOffset - rebarLength + (rebarLength % spacing);
                    break;
                default:
                    throw new Exception("The program should never go here.");
            }

            // get the profile of the transverse rebar
            List<Autodesk.Revit.DB.XYZ > movedPoints = OffsetPoints(offset);

            // Translate curves points
            List<Autodesk.Revit.DB.XYZ > translatedPoints = new List<Autodesk.Revit.DB.XYZ >();
            foreach (Autodesk.Revit.DB.XYZ point in movedPoints)
            {
                translatedPoints.Add(GeomUtil.OffsetPoint(point, m_drivingVector, curveOffset));
            }

            IList<Curve> curves = new List<Curve>();
            Autodesk.Revit.DB.XYZ first = translatedPoints[0];
            Autodesk.Revit.DB.XYZ second = translatedPoints[1];
            Autodesk.Revit.DB.XYZ third = translatedPoints[2];
            Autodesk.Revit.DB.XYZ fourth = translatedPoints[3];

            curves.Add(Line.get_Bound(first, second));
            curves.Add(Line.get_Bound(second, fourth));
            curves.Add(Line.get_Bound(fourth, third));
            curves.Add(Line.get_Bound(third, first));

            // return the rebar geometry information
            return new RebarGeometry(normal, curves, rebarNumber, spacing);
        }


        /// <summary>
        /// Get the down direction, which stand for the top hook direction
        /// </summary>
        /// <returns>the down direction</returns>
        public Autodesk.Revit.DB.XYZ GetDownDirection()
        {
            XYZHeightComparer comparer = new XYZHeightComparer();
            m_points.Sort(comparer);

            Autodesk.Revit.DB.XYZ refPoint = m_points[3];
            List<Autodesk.Revit.DB.XYZ > directions = GetRelatedVectors(refPoint);
            directions.Sort(comparer);

            return directions[0];
        }


        /// <summary>
        /// Get the width of the beam
        /// </summary>
        /// <returns>the width data</returns>
        private double GetBeamWidth()
        {
            XYZHeightComparer comparer = new XYZHeightComparer();
            m_points.Sort(comparer);

            Autodesk.Revit.DB.XYZ refPoint = m_points[0];
            List<Autodesk.Revit.DB.XYZ > directions = GetRelatedVectors(refPoint);
            directions.Sort(comparer);

            return GeomUtil.GetLength(directions[0]);
        }


        /// <summary>
        /// Get the height of the beam
        /// </summary>
        /// <returns>the height data</returns>
        private double GetBeamHeight()
        {
            XYZHeightComparer comparer = new XYZHeightComparer();
            m_points.Sort(comparer);

            Autodesk.Revit.DB.XYZ refPoint = m_points[0];
            List<Autodesk.Revit.DB.XYZ > directions = GetRelatedVectors(refPoint);
            directions.Sort(comparer);

            return GeomUtil.GetLength(directions[1]);
        }
    }
}
