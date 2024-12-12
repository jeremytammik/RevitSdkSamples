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
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Revit.SDK.Samples.WinderStairs.CS
{
    /// <summary>
    /// It represents an L-shape winder and consists of two straight runs and one winder corner.
    /// </summary>
    class LWinder : Winder
    {
        /// <summary>
        /// Number of straight steps at start.
        /// </summary>
        public uint NumStepsAtStart { get; set; }

        /// <summary>
        /// Number of straight steps at end.
        /// </summary>
        public uint NumStepsAtEnd { get; set; }

        /// <summary>
        /// Number of winder steps in corner.
        /// </summary>
        public uint NumStepsInCorner { get; set; }

        /// <summary>
        /// CenterPoint Offset distance from the first inner boundary line of the corner.
        /// </summary>
        public double CenterOffsetE { get; set; }

        /// <summary>
        /// CenterPoint Offset distance from the second inner boundary line of the corner.
        /// </summary>
        public double CenterOffsetF { get; set; }

        /// <summary>
        /// Three points to determine the L-shape of the stairs.
        /// </summary>
        public override IList<XYZ> ControlPoints
        {
            get
            {
                return base.ControlPoints;
            }
            set
            {
                if (value.Count != 3)
                {
                    throw new ArgumentException("The control points count must be 3 for LWinder.");
                }
                base.ControlPoints = value;
            }
        }
        
        #region PRIVATE
        /// <summary>
        /// Winder Corner, connecting the first and the second straight runs.
        /// </summary>
        private WinderSinglePoint m_corner;

        /// <summary>
        /// The first straight run at start.
        /// </summary>
        private WinderStraight m_straightAtStart;

        /// <summary>
        /// The second straight run at end.
        /// </summary>
        private WinderStraight m_straightAtEnd;

        /// <summary>
        /// This method constructs the winder corner and two straight runs.
        /// Please be sure the input properties being set properly before calling this method.
        /// </summary>
        private void Construct()
        {
            //
            // Construct the winder corner.
            //
            XYZ dir1 = (ControlPoints[1] - ControlPoints[0]).Normalize();
            XYZ dir2 = (ControlPoints[2] - ControlPoints[1]).Normalize();
            m_corner = new WinderSinglePoint(ControlPoints[1], dir1, dir2, NumStepsInCorner);
            m_corner.Construct(RunWidth, CenterOffsetE, CenterOffsetF);

            //
            // Construct two straight runs to connect to the winder corner.
            //
            XYZ startPnt = m_corner.StartPoint - TreadDepth * NumStepsAtStart * dir1;
            XYZ endPnt = m_corner.EndPoint + TreadDepth * NumStepsAtEnd * dir2;
            XYZ bisectDir = (dir2 - dir1).Normalize();
            XYZ perpendicularDir1 = new XYZ(-dir1.Y, dir1.X, 0);
            XYZ perpendicularDir2 = new XYZ(-dir2.Y, dir2.X, 0);
            if (bisectDir.DotProduct(perpendicularDir1) < 0)
            {
                perpendicularDir1 = perpendicularDir1.Negate();
                perpendicularDir2 = perpendicularDir2.Negate();
            }
            m_straightAtStart = new WinderStraight(
                startPnt, m_corner.StartPoint, perpendicularDir1, NumStepsAtStart);
            m_straightAtEnd = new WinderStraight(
                m_corner.EndPoint, endPnt, perpendicularDir2, NumStepsAtEnd);
        }
        #endregion

        /// <summary>
        /// This method generates the sketch for L-shape winder stairs.
        /// </summary>
        protected override void GenerateSketch()
        {
            // Instantiate the corner and the two straight runs.
            Construct();

            // Generate sketch for winder corner and straight runs.
            m_straightAtStart.GenerateSketch(
                RunWidth, OuterBoundary, CenterWalkpath, InnerBoundary, RiserLines);
            m_corner.GenerateSketch(
                RunWidth, OuterBoundary, CenterWalkpath, InnerBoundary, RiserLines);
            m_straightAtEnd.GenerateSketch(
                RunWidth, OuterBoundary, CenterWalkpath, InnerBoundary, RiserLines);
        }
    }
}
