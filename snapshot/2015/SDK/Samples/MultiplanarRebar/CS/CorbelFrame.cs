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
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.MultiplanarRebar.CS
{
    /// <summary>
    /// This class represents the trapezoid wire frame profile of corbel.
    /// Its two main functionalities are to create a multi-planar rebar shape and
    /// to calculate the location for rebar creation when reinforcing corbel.  
    /// </summary>
    class Trapezoid
    {
        //
        //             TOP       
        //         |---------\      
        // Vertical|          \Slanted
        //         |   Bottom  \
        //---------|------------\
        //              
        // Top -> Vertical -> Bottom -> Slanted form counter clockwise orientation.

        /// <summary>
        /// Top bound line of this trapezoid.
        /// </summary>
        public Line Top { get; set; }

        /// <summary>
        /// Left vertical bound line of this trapezoid. 
        /// </summary>
        public Line Vertical { get; set; }

        /// <summary>
        /// Bottom bound line of this trapezoid.
        /// </summary>
        public Line Bottom { get; set; }

        /// <summary>
        /// Right slanted bound line of this trapezoid.
        /// </summary>
        public Line Slanted { get; set; }

        /// <summary>
        /// Constructor to initialize the fields.
        /// </summary>
        /// <param name="top">Top Line</param>
        /// <param name="vertical">Left Vertical Line</param>
        /// <param name="bottom">Bottom Line</param>
        /// <param name="slanted">Right slanted Line</param>
        public Trapezoid(Line top, Line vertical, Line bottom, Line slanted)
        {
            Top = top;
            Vertical = vertical;
            Bottom = bottom;
            Slanted = slanted;
        }

        /// <summary>
        /// Draw the trapezoid wire-frame with Revit Model curves.
        /// It's for debug use, to help developer see the exact location.
        /// </summary>
        /// <param name="revitDoc">Revit DB Document</param>
        public void Draw(Document revitDoc)
        {
            XYZ topDir = (Top.GetEndPoint(1) - Top.GetEndPoint(0)).Normalize();
            XYZ verticalDir = (Vertical.GetEndPoint(0) - Vertical.GetEndPoint(1)).Normalize();
            XYZ normal = topDir.CrossProduct(verticalDir);

            SketchPlane sketchplane = SketchPlane.Create(revitDoc, new Plane(normal, Vertical.GetEndPoint(0)));

            CurveArray curves = new CurveArray();
            curves.Append(Top.Clone());
            curves.Append(Vertical.Clone());
            curves.Append(Bottom.Clone());
            curves.Append(Slanted.Clone());
            revitDoc.Create.NewModelCurveArray(curves, sketchplane);
        }

        /// <summary>
        /// Offset the top line with given value, if the value is positive,
        /// the offset direction is outside, otherwise inside.
        /// </summary>
        /// <param name="offset">Offset value</param>
        public void OffsetTop(double offset)
        {
            XYZ verticalDir = (Vertical.GetEndPoint(0) - Vertical.GetEndPoint(1)).Normalize();
            XYZ verticalDelta = verticalDir * offset;
            XYZ verticalFinal = Vertical.GetEndPoint(0) + verticalDelta;

            double verticalLengthNew = Vertical.Length + offset;
            double slantedLengthNew = verticalLengthNew * Slanted.Length / Vertical.Length;

            XYZ slantedDir = (Slanted.GetEndPoint(1) - Slanted.GetEndPoint(0)).Normalize();
            XYZ slantedFinal = Slanted.GetEndPoint(0) + slantedDir * slantedLengthNew;

            Vertical = Line.CreateBound(verticalFinal, Vertical.GetEndPoint(1));
            Top = Line.CreateBound(slantedFinal, verticalFinal);
            Slanted = Line.CreateBound(Slanted.GetEndPoint(0), slantedFinal);
        }

        /// <summary>
        /// Offset the Left Vertical line with given value, if the value is positive,
        /// the offset direction is outside, otherwise inside.
        /// </summary>
        /// <param name="offset">Offset value</param>
        public void OffsetLeft(double offset)
        {
            XYZ topDir = (Top.GetEndPoint(1) - Top.GetEndPoint(0)).Normalize();
            XYZ topDelta = topDir * offset;

            XYZ topFinal = Top.GetEndPoint(1) + topDelta;
            XYZ bottomFinal = Bottom.GetEndPoint(0) + topDelta;

            Vertical = Line.CreateBound(topFinal, bottomFinal);
            Bottom = Line.CreateBound(bottomFinal, Bottom.GetEndPoint(1));
            Top = Line.CreateBound(Top.GetEndPoint(0), topFinal);
        }

        /// <summary>
        /// Offset the bottom line with given value, if the value is positive,
        /// the offset direction is outside, otherwise inside.
        /// </summary>
        /// <param name="offset">Offset value</param>
        public void OffsetBottom(double offset)
        {
            XYZ verticalDir = (Vertical.GetEndPoint(1) - Vertical.GetEndPoint(0)).Normalize();
            XYZ verticalDelta = verticalDir * offset;
            XYZ verticalFinal = Vertical.GetEndPoint(1) + verticalDelta;

            double verticalLengthNew = Vertical.Length + offset;

            double slantedLengthNew = verticalLengthNew * Slanted.Length / Vertical.Length;

            XYZ slantedDir = (Slanted.GetEndPoint(0) - Slanted.GetEndPoint(1)).Normalize();
            XYZ slantedFinal = Slanted.GetEndPoint(1) + slantedDir * slantedLengthNew;

            Vertical = Line.CreateBound(Vertical.GetEndPoint(0), verticalFinal);
            Bottom = Line.CreateBound(verticalFinal, slantedFinal);
            Slanted = Line.CreateBound(slantedFinal, Slanted.GetEndPoint(1));
        }

        /// <summary>
        /// Offset the right slanted line with given value, if the value is positive,
        /// the offset direction is outside, otherwise inside.
        /// </summary>
        /// <param name="offset">Offset value</param>
        public void OffsetRight(double offset)
        {
            XYZ bottomDir = (Bottom.GetEndPoint(1) - Bottom.GetEndPoint(0)).Normalize();
            XYZ bottomDelta = bottomDir * (offset * Slanted.Length / Vertical.Length);

            XYZ topFinal = Top.GetEndPoint(0) + bottomDelta;
            XYZ bottomFinal = Bottom.GetEndPoint(1) + bottomDelta;

            Top = Line.CreateBound(topFinal, Top.GetEndPoint(1));
            Bottom = Line.CreateBound(Bottom.GetEndPoint(0), bottomFinal);
            Slanted = Line.CreateBound(bottomFinal, topFinal);
        }

        /// <summary>
        /// Deep clone, to avoid mess up the original data during offsetting the boundary.
        /// </summary>
        /// <returns>Cloned object</returns>
        public Trapezoid Clone()
        {
            return new Trapezoid(
                Top.Clone() as Line,
                Vertical.Clone() as Line,
                Bottom.Clone() as Line,
                Slanted.Clone() as Line);
        }

        /// <summary>
        /// Create the multi-planar Rebar Shape according to the trapezoid wire-frame.
        /// </summary>
        /// <param name="revitDoc">Revit DB Document</param>
        /// /// <param name="bendDiameter">OutOfPlaneBendDiameter for multi-planar shape</param>
        /// <returns>Created multi-planar Rebar Shape</returns>
        public RebarShape ConstructMultiplanarRebarShape(Document revitDoc, double bendDiameter)
        {
            // Construct a segment definition with 2 lines.
            RebarShapeDefinitionBySegments shapedef = new RebarShapeDefinitionBySegments(revitDoc, 2);

            // Define parameters for the dimension.
            ElementId B = SharedParameterUtil.GetOrCreateDef("B", revitDoc);
            ElementId H = SharedParameterUtil.GetOrCreateDef("H", revitDoc);
            ElementId K = SharedParameterUtil.GetOrCreateDef("K", revitDoc);
            ElementId MM = SharedParameterUtil.GetOrCreateDef("MM", revitDoc);


            // Set parameters default values according to the size Trapezoid shape. 
            shapedef.AddParameter(B, Top.Length);
            shapedef.AddParameter(H, Bottom.Length - Top.Length);
            shapedef.AddParameter(K, Vertical.Length);
            shapedef.AddParameter(MM, 15);

            // Rebar shape geometry curves consist of Line S0 and Line S1.
            // 
            //
            //         |Y       V1
            //         |--S0(B)--\      
            //         |          \S1(H, K)
            //         |           \
            //---------|O-----------\----X
            //         |       

            // Define Segment 0 (S0)
            //
            // S0's direction is fixed in positive X Axis.
            shapedef.SetSegmentFixedDirection(0, 1, 0);
            // S0's length is determined by parameter B
            shapedef.AddConstraintParallelToSegment(0, B, false, false);

            // Define Segment 1 (S1)
            //
            // Fix S1's direction.
            shapedef.SetSegmentFixedDirection(1, Bottom.Length - Top.Length, -Vertical.Length);
            // S1's length in positive X Axis is parameter H.            
            shapedef.AddConstraintToSegment(1, H, 1, 0, 1, false, false);
            // S1's length in negative Y Axis is parameter K.
            shapedef.AddConstraintToSegment(1, K, 0, -1, 1, false, false);

            // Define Vertex 1 (V1)
            //
            // S1 at V1 is turn to right and the angle is acute.
            shapedef.AddBendDefaultRadius(1, RebarShapeVertexTurn.Right, RebarShapeBendAngle.Acute);

            // Check to see if it's full constrained.  
            if (!shapedef.Complete)
            {
                throw new Exception("Shape was not completed.");
            }

            // Try to solve it to make sure the shape can be resolved with default parameter value.
            if (!shapedef.CheckDefaultParameterValues(0, 0))
            {
                throw new Exception("Can't resolve rebar shape.");
            }

            // Define multi-planar definition
            RebarShapeMultiplanarDefinition multiPlanarDef = new RebarShapeMultiplanarDefinition(bendDiameter);
            multiPlanarDef.DepthParamId = MM;

            // Realize the Rebar shape with creation static method.
            // The RebarStype is stirrupTie, and it will attach to the top cover.
            RebarShape newshape = RebarShape.Create(revitDoc, shapedef, multiPlanarDef,
                RebarStyle.StirrupTie, StirrupTieAttachmentType.InteriorFace,
                0, RebarHookOrientation.Left, 0, RebarHookOrientation.Left, 0);

            // Give a readable name
            newshape.Name = "API Corbel Multi-Shape " + newshape.Id;

            // Make sure we can see the created shape from the browser.
            IList<Curve> curvesForBrowser = newshape.GetCurvesForBrowser();
            if (curvesForBrowser.Count == 0)
            {
                throw new Exception("The Rebar shape is invisible in browser.");
            }

            return newshape;
        }

        /// <summary>
        /// Calculate the boundary coordinate of the wire-frame.
        /// </summary>
        /// <param name="origin">Origin coordinate</param>
        /// <param name="vX">X Vector</param>
        /// <param name="vY">Y Vector</param>
        public void Boundary(out XYZ origin, out XYZ vX, out XYZ vY)
        {
            origin = Vertical.GetEndPoint(1);
            vX = Bottom.GetEndPoint(1) - Bottom.GetEndPoint(0);
            vY = Vertical.GetEndPoint(0) - Vertical.GetEndPoint(1);
        }
    }

    /// <summary>
    /// It represents the frame of Corbel, which is consist of a trapezoid profile and a extrusion line.
    /// Corbel can be constructed by sweeping a trapezoid profile along the extrusion line.
    /// </summary>
    class CorbelFrame
    {
        /// <summary>
        /// Trapezoid profile of corbel family instance.
        /// </summary>
        private Trapezoid m_profile;

        /// <summary>
        /// Extrusion line of corbel family instance.
        /// </summary>
        private Line m_extrusionLine;

        /// <summary>
        /// Corbel family instance.
        /// </summary>
        private FamilyInstance m_corbel;

        /// <summary>
        /// Depth of corbel host.
        /// </summary>
        private double m_hostDepth;

        /// <summary>
        /// Cover distance of corbel family instance.
        /// </summary>
        private double m_corbelCoverDistance;

        /// <summary>
        /// Cover distance of corbel host.
        /// </summary>
        private double m_hostCoverDistance;

        /// <summary>
        /// Constructor to initialize the fields.
        /// </summary>
        /// <param name="corbel">Corbel family instance</param>
        /// <param name="profile">Trapezoid profile</param>
        /// <param name="path">Extrusion Line</param>
        /// <param name="hostDepth">Corbel Host Depth</param>
        /// <param name="hostTopCorverDistance">Corbel Host cover distance</param>
        public CorbelFrame(FamilyInstance corbel, Trapezoid profile,
                Line path, double hostDepth, double hostTopCorverDistance)
        {
            m_profile = profile;
            m_extrusionLine = path;
            m_corbel = corbel;
            m_hostDepth = hostDepth;
            m_hostCoverDistance = hostTopCorverDistance;

            // Get the cover distance of corbel from CommonCoverType.
            RebarHostData rebarHost = RebarHostData.GetRebarHostData(m_corbel);
            m_corbelCoverDistance = rebarHost.GetCommonCoverType().CoverDistance;
        }

        /// <summary>
        /// Parse the geometry of given Corbel and create a CorbelFrame if the corbel is slopped,
        /// otherwise exception thrown.
        /// </summary>
        /// <param name="corbel">Corbel to parse</param>
        /// <returns>A created CorbelFrame</returns>
        public static CorbelFrame Parse(FamilyInstance corbel)
        {
            // This just delegates a call to GeometryUtil class.
            return GeometryUtil.ParseCorbelGeometry(corbel);
        }

        /// <summary>
        /// Add bars to reinforce the Corbel FamilyInstance with given options.
        /// The bars including:
        /// a multi-planar bar, 
        /// top straight bars, 
        /// stirrup bars, 
        /// and host straight bars.
        /// </summary>
        /// <param name="rebarOptions">Options for Rebar Creation</param>
        public void Reinforce(CorbelReinforcementOptions rebarOptions)
        {
            PlaceStraightBars(rebarOptions);

            PlaceMultiplanarRebar(rebarOptions);

            PlaceStirrupBars(rebarOptions);

            PlaceCorbelHostBars(rebarOptions);
        }

        /// <summary>
        /// Add straight bars into corbel with given options. 
        /// </summary>
        /// <param name="options">Options for Rebar Creation</param>
        private void PlaceStraightBars(CorbelReinforcementOptions options)
        {
            Trapezoid profileCopy = m_profile.Clone();
            profileCopy.OffsetTop(-m_corbelCoverDistance);
            profileCopy.OffsetLeft(-m_corbelCoverDistance
                - options.MultiplanarBarType.BarDiameter
                - options.TopBarType.BarDiameter * 0.5);
            profileCopy.OffsetBottom(m_hostDepth - m_hostCoverDistance
                - options.StirrupBarType.BarDiameter
                - options.HostStraightBarType.BarDiameter);
            profileCopy.OffsetRight(-m_corbelCoverDistance);

            //m_profile.Draw(options.RevitDoc);
            //profileCopy.Draw(options.RevitDoc);

            XYZ extruDir = (m_extrusionLine.GetEndPoint(1) - m_extrusionLine.GetEndPoint(0)).Normalize();
            double offset = m_corbelCoverDistance +
                options.StirrupBarType.BarDiameter +
                options.MultiplanarBarType.BarDiameter +
                0.5 * options.TopBarType.BarDiameter;

            Line vetical = profileCopy.Vertical;
            XYZ delta = extruDir * offset;
            Curve barLine = Line.CreateBound(vetical.GetEndPoint(1) + delta, vetical.GetEndPoint(0) + delta);
            IList<Curve> barCurves = new List<Curve>();
            barCurves.Add(barLine);

            Rebar bars = Rebar.CreateFromCurves(options.RevitDoc, RebarStyle.Standard,
                options.TopBarType, null, null, m_corbel, extruDir, barCurves,
                RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);

            bars.SetLayoutAsFixedNumber(options.TopBarCount + 2,
                m_extrusionLine.Length - 2 * offset, true, false, false);
            ShowRebar3d(bars);
        }

        /// <summary>
        /// Add a multi-planar bar into corbel with given options.
        /// </summary>
        /// <param name="options">Options for Rebar Creation</param>
        private void PlaceMultiplanarRebar(CorbelReinforcementOptions options)
        {
            Trapezoid profileCopy = m_profile.Clone();
            profileCopy.OffsetTop(-m_corbelCoverDistance
                - options.StirrupBarType.BarDiameter - 0.5 * options.MultiplanarBarType.BarDiameter);
            profileCopy.OffsetLeft(-m_corbelCoverDistance - 0.5 * options.MultiplanarBarType.BarDiameter);
            profileCopy.OffsetBottom(m_hostDepth - m_hostCoverDistance
                - options.HostStraightBarType.BarDiameter * 4
                - options.StirrupBarType.BarDiameter);
            profileCopy.OffsetRight(-m_corbelCoverDistance - options.StirrupBarType.BarDiameter
                - 0.5 * options.StirrupBarType.BarDiameter);

            //m_profile.Draw(options.RevitDoc);
            //profileCopy.Draw(options.RevitDoc);

            XYZ origin, vx, vy;
            profileCopy.Boundary(out origin, out vx, out vy);

            XYZ vecX = vx.Normalize();
            XYZ vecY = vy.Normalize();
            RebarShape barshape = profileCopy.ConstructMultiplanarRebarShape(options.RevitDoc,
                0.5 * options.MultiplanarBarType.StirrupTieBendDiameter);
            Rebar newRebar = Rebar.CreateFromRebarShape(
                options.RevitDoc, barshape,
                options.MultiplanarBarType,
                m_corbel, origin, vecX, vecY);

            XYZ extruDir = (m_extrusionLine.GetEndPoint(1) - m_extrusionLine.GetEndPoint(0)).Normalize();
            double offset = m_corbelCoverDistance +
                options.StirrupBarType.BarDiameter +
                0.5 * options.MultiplanarBarType.BarDiameter;

            newRebar.ScaleToBoxFor3D(origin + extruDir * (m_extrusionLine.Length - offset),
                vx, vy, m_extrusionLine.Length - 2 * offset);
            ShowRebar3d(newRebar);
        }

        /// <summary>
        /// Add stirrup bars into corbel with given options.
        /// </summary>
        /// <param name="options">Options for Rebar Creation</param>
        private void PlaceStirrupBars(CorbelReinforcementOptions options)
        {
            var filter = new FilteredElementCollector(options.RevitDoc)
                .OfClass(typeof(RebarShape)).ToElements().Cast<RebarShape>()
                .Where<RebarShape>(shape => shape.RebarStyle == RebarStyle.StirrupTie);

            RebarShape stirrupShape = null;
            foreach (RebarShape shape in filter)
            {
                if (shape.Name.Equals("T1"))
                {
                    stirrupShape = shape; break;
                }
            }

            Trapezoid profileCopy = m_profile.Clone();
            profileCopy.OffsetTop(-m_corbelCoverDistance - 0.5 * options.StirrupBarType.BarDiameter);
            profileCopy.OffsetLeft(-m_corbelCoverDistance - 0.5 * options.StirrupBarType.BarDiameter);
            profileCopy.OffsetBottom(m_hostDepth - m_hostCoverDistance - 0.5 * options.StirrupBarType.BarDiameter);
            profileCopy.OffsetRight(-m_corbelCoverDistance - 0.5 * options.StirrupBarType.BarDiameter);

            XYZ extruDir = (m_extrusionLine.GetEndPoint(1) - m_extrusionLine.GetEndPoint(0)).Normalize();
            double offset = m_corbelCoverDistance + 0.5 * options.StirrupBarType.BarDiameter;

            XYZ origin = profileCopy.Vertical.GetEndPoint(0) + extruDir * offset;
            XYZ xAxis = extruDir;
            XYZ yAxis = (profileCopy.Vertical.GetEndPoint(1) - profileCopy.Vertical.GetEndPoint(0)).Normalize();

            Rebar stirrupBars = Rebar.CreateFromRebarShape(options.RevitDoc, stirrupShape,
                        options.StirrupBarType, m_corbel, origin, xAxis, yAxis);

            double xLength = m_extrusionLine.Length - 2 * offset;
            double yLength = profileCopy.Vertical.Length;

            stirrupBars.SetLayoutAsFixedNumber(options.StirrupBarCount + 1, profileCopy.Top.Length, false, false, true);
            stirrupBars.ScaleToBox(origin, xAxis * xLength, yAxis * yLength);
            ShowRebar3d(stirrupBars);

            double space = profileCopy.Top.Length / options.StirrupBarCount;
            double step = space * m_profile.Vertical.Length / (m_profile.Bottom.Length - m_profile.Top.Length);

            XYZ dirTop = (m_profile.Top.GetEndPoint(0) - m_profile.Top.GetEndPoint(1)).Normalize();
            XYZ dirVertical = yAxis;
            XYZ deltaStep = dirTop * space + dirVertical * step;

            origin = profileCopy.Top.GetEndPoint(0) + extruDir * offset;
            int count = (int)((m_profile.Vertical.Length - m_corbelCoverDistance - 0.5 * options.StirrupBarType.BarDiameter) / step);
            for (int i = 1; i <= count; i++)
            {
                origin += deltaStep;
                Rebar stirrupBars2 = Rebar.CreateFromRebarShape(options.RevitDoc, stirrupShape,
                    options.StirrupBarType, m_corbel, origin, xAxis, yAxis);

                stirrupBars2.ScaleToBox(origin, xAxis * xLength, yAxis * (yLength - i * step));
                ShowRebar3d(stirrupBars2);
            }
        }

        /// <summary>
        /// Add straight bars into corbel Host to anchor corbel stirrup bars.
        /// </summary>
        /// <param name="options">Options for Rebar Creation</param>
        private void PlaceCorbelHostBars(CorbelReinforcementOptions options)
        {
            Trapezoid profileCopy = m_profile.Clone();
            profileCopy.OffsetBottom(m_hostDepth - m_hostCoverDistance
                - options.HostStraightBarType.BarDiameter * 0.5
                - options.StirrupBarType.BarDiameter);

            //profileCopy.Draw(options.RevitDoc);

            XYZ extruDir = (m_extrusionLine.GetEndPoint(1) - m_extrusionLine.GetEndPoint(0)).Normalize();
            double offset = m_corbelCoverDistance + options.StirrupBarType.BarDiameter
                + options.HostStraightBarType.BarDiameter * 0.5;
            XYZ delta = extruDir * offset;

            XYZ pt1 = profileCopy.Bottom.GetEndPoint(0) + delta;
            XYZ pt2 = profileCopy.Bottom.GetEndPoint(1) + delta;

            Curve barLine = Line.CreateBound(pt1, pt2);
            IList<Curve> barCurves = new List<Curve>();
            barCurves.Add(barLine);

            Rebar bars = Rebar.CreateFromCurves(
                options.RevitDoc, RebarStyle.Standard,
                options.HostStraightBarType, null, null, m_corbel.Host, extruDir, barCurves,
                RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);

            bars.SetLayoutAsFixedNumber(2, m_extrusionLine.Length - 2 * offset, true, true, true);

            ShowRebar3d(bars);
        }

        /// <summary>
        /// Show the given rebar as solid in 3d view.
        /// </summary>
        /// <param name="rebar">Rebar to show in 3d view as solid</param>
        private void ShowRebar3d(Rebar rebar)
        {
            var filter = new FilteredElementCollector(rebar.Document)
                .OfClass(typeof(View3D));

            foreach (View3D view in filter)
            {
                rebar.IsUnobscuredInView(view);
                rebar.SetSolidInView(view, true);
            }
        }
    }
}
