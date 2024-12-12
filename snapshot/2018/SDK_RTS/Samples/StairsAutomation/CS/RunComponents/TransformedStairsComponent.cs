//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.StairsAutomation.CS
{
	/// <summary>
	/// An abstract base class for stairs components which can be moved via a translation and rotation.
	/// </summary>
	public class TransformedStairsComponent
	{
		private Transform m_transform;
		
        /// <summary>
        /// Constructs a transformed component with the identity transform.
        /// </summary>
		public TransformedStairsComponent()
		{
			m_transform = Autodesk.Revit.DB.Transform.Identity;
		}

        /// <summary>
        /// Constructs a transformed component with the specified transform.
        /// </summary>
		public TransformedStairsComponent(Transform transform)
		{
			m_transform = transform;
		}
		
        /// <summary>
        /// Transforms the given XYZ point by the stored transform.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The transformed point.</returns>
		public XYZ TransformPoint(XYZ point)
		{
			XYZ xyz = m_transform.OfPoint(point);
			return xyz;
		}

        /// <summary>
        /// Transforms the given curve by the stored transform.
        /// </summary>
        /// <param name="curve">The curve.</param>
        /// <returns>The transformed curve.</returns>
		public Curve Transform(Curve curve)
		{
            return GeometryUtils.TransformCurve(curve, m_transform);
		}

        /// <summary>
        /// Transforms the given line by the stored transform.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>The transformed line.</returns>
		public Line Transform(Line line)
		{
			return Transform(line as Curve) as Line;
		}
		
        /// <summary>
        /// Transforms all members of the given curve array by the stored transform.
        /// </summary>
        /// <param name="inputs">The input curves.</param>
        /// <returns>The transformed curves.</returns>
        public IList<Curve> Transform(IList<Curve> inputs)
        {
            return GeometryUtils.TransformCurves(inputs, m_transform);
        }
	}
}
