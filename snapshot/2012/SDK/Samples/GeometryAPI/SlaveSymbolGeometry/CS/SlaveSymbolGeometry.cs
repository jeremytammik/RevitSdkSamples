//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using Autodesk.Revit.DB.Analysis;

namespace Revit.SDK.Samples.SlaveSymbolGeometry.CS
{
    class SlaveSymbolGeometry
    {
        /// <summary>
        /// Revit document
        /// </summary>
        private Document RevitDoc;

        /// <summary>
        /// Options for geometry
        /// </summary>
        private Options m_options;

        /// <summary>
        /// Schema Id
        /// </summary>
        private int m_schemaId = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="doc">Revit Document</param>
        public SlaveSymbolGeometry(Document doc)
        {
            RevitDoc = doc;
            m_options = new Options();
        }

        /// <summary>
        /// Get geometry of family instances and show them in Revit views
        /// </summary>
        public void GetInstanceGeometry()
        {
            FilteredElementCollector instanceCollector = new FilteredElementCollector(RevitDoc);
            instanceCollector.OfClass(typeof(FamilyInstance));

            // create views by different names

            View instanceView = RevitDoc.Create.NewView3D(new XYZ(1, 1, -1));
            instanceView.Name = "InstanceGeometry";

            View originalView = RevitDoc.Create.NewView3D(new XYZ(0, 1, -1));
            originalView.Name = "OriginalGeometry";

            View transView = RevitDoc.Create.NewView3D(new XYZ(-1, 1, -1));
            transView.Name = "TransformedGeometry";

            foreach (FamilyInstance instance in instanceCollector)
            {
                GeometryElement instanceGeo = instance.get_Geometry(m_options);
                GeometryElement slaveGeo = instance.GetOriginalGeometry(m_options);
                GeometryElement transformGeo = slaveGeo.GetTransformed(instance.GetTransform());

                // show family instance geometry
                foreach (GeometryObject obj in instanceGeo.Objects)
                {
                    if (obj is Solid)
                    {
                        Solid solid = obj as Solid;
                        PaintSolid(solid, instanceView);
                    }
                }

                // show geometry that is original geometry
                foreach (GeometryObject obj in slaveGeo.Objects)
                {
                    if (obj is Solid)
                    {
                        Solid solid = obj as Solid;
                        PaintSolid(solid, originalView);
                    }
                }

                // show geometry that was transformed
                foreach (GeometryObject obj in transformGeo.Objects)
                {
                    if (obj is Solid)
                    {
                        Solid solid = obj as Solid;
                        PaintSolid(solid, transView);
                    }
                }
            }
            // remove original instances to view point results.
            RevitDoc.Delete(instanceCollector.ToElementIds());
        }

        /// <summary>
        /// Paint solid by AVF
        /// </summary>
        /// <param name="solid">Solid to be painted</param>
        /// <param name="view">The view that shows solid</param>
        private void PaintSolid(Solid solid, View view)
        {
            String viewName = view.Name;
            SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(view);
            if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(view, 1);

            if (m_schemaId != -1)
            {
                IList<int> results = sfm.GetRegisteredResults();

                if (!results.Contains(m_schemaId))
                {
                    m_schemaId = -1;
                }
            }

            // set up the display style
            if (m_schemaId == -1)
            {
                AnalysisResultSchema resultSchema1 = new AnalysisResultSchema("PaintedSolid " + viewName, "Description");

                AnalysisDisplayStyle displayStyle = AnalysisDisplayStyle.CreateAnalysisDisplayStyle(this.RevitDoc, "Real_Color_Surface" + viewName, new AnalysisDisplayColoredSurfaceSettings(), new AnalysisDisplayColorSettings(), new AnalysisDisplayLegendSettings());

                resultSchema1.AnalysisDisplayStyleId = displayStyle.Id;

                m_schemaId = sfm.RegisterResult(resultSchema1);
            }

            // get points of all faces in the solid
            FaceArray faces = solid.Faces;
            Transform trf = Transform.Identity;
            foreach (Face face in faces)
            {
                int idx = sfm.AddSpatialFieldPrimitive(face, trf);
                IList<UV> uvPts = null;
                IList<ValueAtPoint> valList = null;
                ComputeValueAtPointForFace(face, out uvPts, out valList, 1);

                FieldDomainPointsByUV pnts = new FieldDomainPointsByUV(uvPts);
                FieldValues vals = new FieldValues(valList);
                sfm.UpdateSpatialFieldPrimitive(idx, pnts, vals, m_schemaId);
            }
        }

        /// <summary>
        /// Compute values at point for face
        /// </summary>
        /// <param name="face">Give face</param>
        /// <param name="uvPts">UV points</param>
        /// <param name="valList">Values at point</param>
        /// <param name="measurementNo"></param>
        private void ComputeValueAtPointForFace(Face face, out IList<UV> uvPts, 
            out IList<ValueAtPoint> valList, int measurementNo)
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
