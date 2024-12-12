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

using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.WinderStairs.CS
{
    /// <summary>
    /// It represents a winder region used to connect straight runs. For L-shape stairs, there is
    /// one WinderCorner, For U-shape stairs, there are two such components.
    /// </summary>
    abstract class WinderCorner
    {
        /*   
        *   Direction1
        *    |      
        *    |
        *    V    
        *    |
        *    |
        *    |----->---- Direction2
        * CornerPoint   
        */

        /// <summary>
        /// Corner point of the winder region.
        /// </summary>
        public XYZ CornerPoint { get; private set; }

        /// <summary>
        /// The enter direction of the winder region.
        /// </summary>
        public XYZ Direction1 { get; private set; }

        /// <summary>
        /// The leave direction of the winder region.
        /// </summary>
        public XYZ Direction2 { get; private set; }

        /// <summary>
        /// Number of steps in the winder region.
        /// </summary>
        public uint NumSteps { get; private set; }

        /// <summary>
        /// The distance from start point to corner point.
        /// </summary>
        public double Distance1 { get; protected set; }

        /// <summary>
        /// The distance from corner point to end point.
        /// </summary>
        public double Distance2 { get; protected set; }

        /// <summary>
        /// Start delimiter of the winder region.
        /// </summary>
        public XYZ StartPoint { get { return CornerPoint - Direction1 * Distance1; } }

        /// <summary>
        /// End delimiter of the winder region.
        /// </summary>
        public XYZ EndPoint { get { return CornerPoint + Direction2 * Distance2; } }

        /// <summary>
        /// Constructor to initialize the basic fields of a winder region.
        /// </summary>
        /// <param name="cornerPnt">Corner Point</param>
        /// <param name="dir1">Enter direction</param>
        /// <param name="dir2">Leave direction</param>
        /// <param name="steps">Number of steps</param>
        protected WinderCorner(XYZ cornerPnt, XYZ dir1, XYZ dir2, uint steps)
        {
            CornerPoint = cornerPnt;
            Direction1 = dir1;
            Direction2 = dir2;
            NumSteps = steps;
        }

        /// <summary>
        /// Move the whole winder region by the given vector.
        /// </summary>
        /// <param name="vector">Move delta vector</param>
        public virtual void Move(XYZ vector)
        {
            CornerPoint += vector;
        }

        /// <summary>
        /// Generate the sketch in the winder region including the outer boundary, walk path, 
        /// inner boundary and riser lines. Subclass need to implement this method 
        /// to fill the input sketch using its winder layout algorithm.
        /// </summary>
        /// <param name="runWidth">Runwidth</param>
        /// <param name="outerBoundary">Outer boundary</param>
        /// <param name="walkPath">Walk path</param>
        /// <param name="innerBoundary">Inner boundary</param>
        /// <param name="riserLines">Riser lines</param>
        public abstract void GenerateSketch(double runWidth,
            IList<Curve> outerBoundary, IList<Curve> walkPath,
            IList<Curve> innerBoundary, IList<Curve> riserLines);
    }
}
