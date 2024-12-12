//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Drawing;
using System.Drawing.Drawing2D;

using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using System.Reflection;

namespace Revit.SDK.Samples.InPlaceMembers.CS
{
    /// <summary>
    /// generate GraphicsData by given geometry object
    /// </summary>
    public class GraphicsDataFactory
    {
        /// <summary>
        /// create GraphicsData of given AnalyticalModel3D
        /// </summary>
        /// <param name="model">AnalyticalModel3D contains geometry data</param>
        /// <returns></returns>
        public static GraphicsData CreateGraphicsData(AnalyticalModel model)
        {
            IList<Curve> curveList = model.GetCurves(AnalyticalCurveType.ActiveCurves);

            if (curveList.Count > 0)
            {
                GraphicsData data = new GraphicsData();

                IEnumerator<Curve> curves = curveList.GetEnumerator();
                curves.Reset();
                while (curves.MoveNext())
                {
                    Curve curve = curves.Current as Curve;

                    try
                    {
                        List<XYZ> points = curve.Tessellate() as List<XYZ>;
                        data.InsertCurve(points);
                    }
                    catch
                    {
                    }
                }

                data.UpdataData();

                return data;
            }
            else
            {
                throw new Exception("Can't get curves.");
            }
        }
    }
}
