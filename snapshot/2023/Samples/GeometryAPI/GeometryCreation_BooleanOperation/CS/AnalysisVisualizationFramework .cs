//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using Autodesk.Revit.DB.Analysis;

namespace Revit.SDK.Samples.GeometryCreation_BooleanOperation.CS
{
    class AnalysisVisualizationFramework
    {
        /// <summary>
        /// The singleton instance of AnalysisVisualizationFramework
        /// </summary>
        private static AnalysisVisualizationFramework Instance;

        /// <summary>
        /// revit document
        /// </summary>
        private Autodesk.Revit.DB.Document m_doc;

        /// <summary>
        /// The created view list
        /// </summary>
        private List<String> viewNameList = new List<string>();

        /// <summary>
        /// The ID of schema which SpatialFieldManager register
        /// </summary>
        private static int SchemaId = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="doc">Revit document</param>
        private AnalysisVisualizationFramework(Autodesk.Revit.DB.Document doc)
        {
            m_doc = doc;
        }

        /// <summary>
        /// Get the singleton instance of AnalysisVisualizationFramework
        /// </summary>
        /// <param name="doc">Revit document</param>
        /// <returns>The singleton instance of AnalysisVisualizationFramework</returns>
        public static AnalysisVisualizationFramework getInstance(Autodesk.Revit.DB.Document doc)
        {
            if (Instance == null)
            {
                Instance = new AnalysisVisualizationFramework(doc);
            }
            return Instance;
        }

        /// <summary>
        /// Paint a solid in a new named view
        /// </summary>
        /// <param name="s">solid</param>
        /// <param name="viewName">Given the name of view</param>
        public void PaintSolid(Solid s, String viewName)
        {
            View view;
            if (!viewNameList.Contains(viewName))
            {
                IList<Element> viewFamilyTypes = new FilteredElementCollector(m_doc).OfClass(typeof(ViewFamilyType)).ToElements();
                ElementId View3DId = new ElementId(-1);
                foreach (Element e in viewFamilyTypes)
                {
                    if (e.Name == "3D View")
                    {
                        View3DId = e.Id;
                    }
                }

                //view = m_doc.Create.NewView3D(new XYZ(1, 1, 1));
                view = View3D.CreateIsometric(m_doc, View3DId);
                ViewOrientation3D viewOrientation3D = new ViewOrientation3D(new XYZ(1, -1, -1), new XYZ(1, 1, 1), new XYZ(1, 1, -2));
                (view as View3D).SetOrientation(viewOrientation3D);
                (view as View3D).SaveOrientation();
                view.Name = viewName;
                viewNameList.Add(viewName);
            }
            else
            {
                view = (((new FilteredElementCollector(m_doc).
                   OfClass(typeof(View))).Cast<View>()).
                   Where(e => e.Name == viewName)).First<View>();
            }

            SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(view);
            if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(view, 1);

            if (SchemaId != -1)
            {
                IList<int> results = sfm.GetRegisteredResults();

                if (!results.Contains(SchemaId))
                {
                    SchemaId = -1;
                }
            }

            if (SchemaId == -1)
            {
                AnalysisResultSchema resultSchema1 = new AnalysisResultSchema("PaintedSolid" + viewName, "Description");

                AnalysisDisplayStyle displayStyle = AnalysisDisplayStyle.CreateAnalysisDisplayStyle(
                   m_doc,
                   "Real_Color_Surface" + viewName,
                   new AnalysisDisplayColoredSurfaceSettings(),
                   new AnalysisDisplayColorSettings(),
                   new AnalysisDisplayLegendSettings());

                resultSchema1.AnalysisDisplayStyleId = displayStyle.Id;

                SchemaId = sfm.RegisterResult(resultSchema1);
            }

            FaceArray faces = s.Faces;
            Transform trf = Transform.Identity;

            foreach (Face face in faces)
            {
                int idx = sfm.AddSpatialFieldPrimitive(face, trf);

                IList<UV> uvPts = null;
                IList<ValueAtPoint> valList = null;
                ComputeValueAtPointForFace(face, out uvPts, out valList, 1);

                FieldDomainPointsByUV pnts = new FieldDomainPointsByUV(uvPts);

                FieldValues vals = new FieldValues(valList);

                sfm.UpdateSpatialFieldPrimitive(idx, pnts, vals, SchemaId);
            }
        }

        /// <summary>
        /// Compute the value of face on specific point
        /// </summary>
        /// <param name="face"></param>
        /// <param name="uvPts"></param>
        /// <param name="valList"></param>
        /// <param name="measurementNo"></param>
        private static void ComputeValueAtPointForFace(Face face, out IList<UV> uvPts, out IList<ValueAtPoint> valList, int measurementNo)
        {
            List<double> doubleList = new List<double>();
            uvPts = new List<UV>();
            valList = new List<ValueAtPoint>();
            BoundingBoxUV bb = face.GetBoundingBox();
            for (double u = bb.Min.U; u < bb.Max.U + 0.0000001; u = u + (bb.Max.U - bb.Min.U) / 1)
            {
                for (double v = bb.Min.V; v < bb.Max.V + 0.0000001; v = v + (bb.Max.V - bb.Min.V) / 1)
                {
                    UV uvPnt = new UV(u, v);
                    uvPts.Add(uvPnt);
                    XYZ faceXYZ = face.Evaluate(uvPnt);
                    // Specify three values for each point
                    for (int ii = 1; ii <= measurementNo; ii++)
                        doubleList.Add(faceXYZ.DistanceTo(XYZ.Zero) * ii);
                    valList.Add(new ValueAtPoint(doubleList));
                    doubleList.Clear();
                }
            }
        }
    }
}
