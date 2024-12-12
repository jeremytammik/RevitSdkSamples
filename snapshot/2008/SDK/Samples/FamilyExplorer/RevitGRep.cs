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
using System.Collections.Generic;
using System.Drawing;

using Autodesk.Revit;
using Autodesk.Revit.Creation;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;

using Element = Autodesk.Revit.Element;
using GElement = Autodesk.Revit.Geometry.Element;
using Instance = Autodesk.Revit.Elements.Instance;
using GInstance = Autodesk.Revit.Geometry.Instance;

namespace Revit.SDK.Samples.FamilyExplorer.CS
{
    /// <summary>
    /// Represents the RevitAPI geometry primitives.
    /// </summary>
    public abstract class RevitGRep
    {
        /// <summary>
        /// the dictionary of geometry objects and its methods to process
        /// </summary>
        private Dictionary<Type, GeometryTypeHandler> m_geometryTypes;

        /// <summary>
        ///  the delegate of geometry primitives handler
        /// </summary>
        /// <param name="gObject"></param>
        private delegate void GeometryTypeHandler(GeometryObject gObject);

        /// <summary>
        /// parse the primitives of geometry element
        /// </summary>
        /// <param name="element">geometry element</param> 
        public void ElementPro(GElement element)
        {
            if (null == element)
            {
                return;
            }

            InitializeGDictionary();
            foreach (GeometryObject go in element.Objects)
            {
                GeometryObjectPro(go);
            }
        }
        
        /// <summary>
        /// initialize the geometry dictinary
        /// </summary>
        protected void InitializeGDictionary()
        {
            if (null != m_geometryTypes)
                return;

            m_geometryTypes = new Dictionary<Type, GeometryTypeHandler>();

            m_geometryTypes.Add(typeof(Curve), new GeometryTypeHandler(ParseCurve));
            m_geometryTypes.Add(typeof(Arc), new GeometryTypeHandler(ParseArc));
            m_geometryTypes.Add(typeof(Line), new GeometryTypeHandler(ParseLine));
            m_geometryTypes.Add(typeof(Edge), new GeometryTypeHandler(ParseEdge));
            m_geometryTypes.Add(typeof(Face), new GeometryTypeHandler(ParseFace));
            m_geometryTypes.Add(typeof(GInstance), new GeometryTypeHandler(ParseInstance));
            m_geometryTypes.Add(typeof(Mesh), new GeometryTypeHandler(ParseMesh));
            m_geometryTypes.Add(typeof(Profile), new GeometryTypeHandler(ParseProfile));
            m_geometryTypes.Add(typeof(Solid), new GeometryTypeHandler(ParseSolid));
        }

        /// <summary>
        /// parse the geometry object's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected void GeometryObjectPro(GeometryObject gObject)
        {
            GeometryTypeHandler run = m_geometryTypes[gObject.GetType()];
            run(gObject);
        }

        /// <summary>
        /// parse the curve's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseCurve(GeometryObject gObject)
        {
            GeometryTypeHandler handler = m_geometryTypes[gObject.GetType()];
            handler(gObject);
        }

        /// <summary>
        /// parse the arc's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseArc(GeometryObject gObject) { }

        /// <summary>
        /// parse the line's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseLine(GeometryObject gObject) { }

        /// <summary>
        /// parse the ellipse's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseEllipse(GeometryObject gObject) { }

        /// <summary>
        /// parse the nurbSpline's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseNurbSpline(GeometryObject gObject) { }

        /// <summary>
        /// parse the edge's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseEdge(GeometryObject gObject)
        {
        }

        /// <summary>
        /// parse the face's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseFace(GeometryObject gObject)
        {
            Face face = gObject as Face;

            foreach (EdgeArray edges in face.EdgeLoops)
            {
                foreach (Edge edge in edges)
                {
                    ParseEdge(edge);
                }
            }
        }

        /// <summary>
        /// parse the instance's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseInstance(GeometryObject gObject)
        {
            GInstance ins = gObject as GInstance;
            ElementPro(ins.SymbolGeometry);
        }

        /// <summary>
        /// parse the mesh's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseMesh(GeometryObject gObject) { }

        /// <summary>
        /// parse the profile's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseProfile(GeometryObject gObject)
        {
            Profile profile = gObject as Profile;
            foreach (Curve curve in profile.Curves)
            {
                ParseCurve(curve);
            }
        }

        /// <summary>
        /// parse the solid's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected virtual void ParseSolid(GeometryObject gObject)
        {
            Solid solid = gObject as Solid;

            foreach (Face face in solid.Faces)
            {
                ParseFace(face);
            }
        }
    }

    /// <summary>
    /// collapse all points of a RevitAPI geometry object to display.
    /// </summary>
    public class PrimitivesParser : RevitGRep
    {
        FamilyWireFrame m_familiyShow;
        XYZArray m_wireFrame3D;
        Transform m_trf;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="element">geometry element</param>
        /// <param name="wireFrame3D">all primitives parsed</param>
        /// <param name="trf">the transform of this element</param>
        /// <param name="FamilyWireFrame">the family's wire frame</param>
        public void CollapsePoints(GElement element, XYZArray wireFrame3D, Transform trf, FamilyWireFrame familiyShow)
        {
            m_wireFrame3D = wireFrame3D;
            m_trf = trf;
            m_familiyShow = familiyShow;
            ElementPro(element);
        }

        /// <summary>
        /// parse the edge's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected override void ParseEdge(GeometryObject gObject)
        {
            Edge edge = gObject as Edge;
            if (m_trf.IsIdentity)
            {
                foreach (XYZ point in edge.Tessellate())
                {
                    XYZ item = point;
                    m_familiyShow.UpdateBoundingBox(item);
                    m_wireFrame3D.Append(ref item);
                }
            }
            else
            {
                foreach (XYZ point in edge.Tessellate())
                {
                    XYZ item = point;
                    XYZ itemTrf = m_trf.OfPoint(ref item);
                    m_familiyShow.UpdateBoundingBox(itemTrf);
                    m_wireFrame3D.Append(ref itemTrf);
                }
            }
        }

        /// <summary>
        /// parse the line's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected override void ParseLine(GeometryObject gObject)
        {
            Line line = gObject as Line;
            if (m_trf.IsIdentity)
            {
                foreach (XYZ point in line.Tessellate())
                {
                    XYZ item = point;
                    m_familiyShow.UpdateBoundingBox(item);
                    m_wireFrame3D.Append(ref item);
                }
            }
            else
            {
                foreach (XYZ point in line.Tessellate())
                {
                    XYZ item = point;
                    XYZ itemTrf = m_trf.OfPoint(ref item);
                    m_familiyShow.UpdateBoundingBox(itemTrf);
                    m_wireFrame3D.Append(ref itemTrf);
                }
            }
        }

        /// <summary>
        /// parse the arc's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected override void ParseArc(GeometryObject gObject)
        {
            Arc arc = gObject as Arc;
            if (m_trf.IsIdentity)
            {
                foreach (XYZ point in arc.Tessellate())
                {
                    XYZ item = point;
                    m_familiyShow.UpdateBoundingBox(item);
                    m_wireFrame3D.Append(ref item);
                }
            }
            else
            {
                foreach (XYZ point in arc.Tessellate())
                {
                    XYZ item = point;
                    XYZ itemTrf = m_trf.OfPoint(ref item);
                    m_familiyShow.UpdateBoundingBox(itemTrf);
                    m_wireFrame3D.Append(ref itemTrf);
                }
            }
        }

        /// <summary>
        /// parse the ellipse's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected override void ParseEllipse(GeometryObject gObject)
        {
            Ellipse ellipse = gObject as Ellipse;
            if (m_trf.IsIdentity)
            {
                foreach (XYZ point in ellipse.Tessellate())
                {
                    XYZ item = point;
                    m_familiyShow.UpdateBoundingBox(item);
                    m_wireFrame3D.Append(ref item);
                }
            }
            else
            {
                foreach (XYZ point in ellipse.Tessellate())
                {
                    XYZ item = point;
                    XYZ itemTrf = m_trf.OfPoint(ref item);
                    m_familiyShow.UpdateBoundingBox(itemTrf);
                    m_wireFrame3D.Append(ref itemTrf);
                }
            }
        }

        /// <summary>
        /// parse the nurbspline's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected override void ParseNurbSpline(GeometryObject gObject)
        {
            NurbSpline nurbSpline = gObject as NurbSpline;
            if (m_trf.IsIdentity)
            {
                bool isStart = true;
                foreach (XYZ point in nurbSpline.Tessellate())
                {
                    XYZ item = point;
                    m_familiyShow.UpdateBoundingBox(item);
                    m_wireFrame3D.Append(ref item);
                    if (!isStart)
                    {
                        m_wireFrame3D.Append(ref item);
                        isStart = false;
                    }
                }
            }
            else
            {
                bool isStart = true;
                foreach (XYZ point in nurbSpline.Tessellate())
                {
                    XYZ item = point;
                    XYZ itemTrf = m_trf.OfPoint(ref item);
                    m_familiyShow.UpdateBoundingBox(itemTrf);
                    m_wireFrame3D.Append(ref itemTrf);
                    if (!isStart)
                    {
                        m_wireFrame3D.Append(ref item);
                        isStart = false;
                    }
                }
            }
        }

        /// <summary>
        /// parse the mesh's primitives.
        /// </summary>
        /// <param name="gObject"></param>
        protected override void ParseMesh(GeometryObject gObject)
        {
            Mesh mesh = gObject as Mesh;
            if (m_trf.IsIdentity)
            {
                for (int i = 0; i < mesh.NumTriangles; i = i + 1)
                {
                    MeshTriangle mt = mesh.get_Triangle(i);
                    XYZ item_0 = mt.get_Vertex(0);
                    XYZ item_1 = mt.get_Vertex(1);
                    XYZ item_2 = mt.get_Vertex(2);
                    m_familiyShow.UpdateBoundingBox(item_0);
                    m_familiyShow.UpdateBoundingBox(item_1);
                    m_familiyShow.UpdateBoundingBox(item_2);
                    m_wireFrame3D.Append(ref item_0);
                    m_wireFrame3D.Append(ref item_1);
                    m_wireFrame3D.Append(ref item_1);
                    m_wireFrame3D.Append(ref item_2);
                    m_wireFrame3D.Append(ref item_2);
                    m_wireFrame3D.Append(ref item_0);
                }
            }
            else
            {
                for (int i = 0; i < mesh.NumTriangles; i = i + 1)
                {
                    MeshTriangle mt = mesh.get_Triangle(i);
                    XYZ item_0 = mt.get_Vertex(0);
                    XYZ item_1 = mt.get_Vertex(1);
                    XYZ item_2 = mt.get_Vertex(2);
                    XYZ item_0Trf = m_trf.OfPoint(ref item_0);
                    XYZ item_1Trf = m_trf.OfPoint(ref item_1);
                    XYZ item_2Trf = m_trf.OfPoint(ref item_2);
                    m_familiyShow.UpdateBoundingBox(item_0Trf);
                    m_familiyShow.UpdateBoundingBox(item_1Trf);
                    m_familiyShow.UpdateBoundingBox(item_2Trf);
                    m_wireFrame3D.Append(ref item_0Trf);
                    m_wireFrame3D.Append(ref item_1Trf);
                    m_wireFrame3D.Append(ref item_1Trf);
                    m_wireFrame3D.Append(ref item_2Trf);
                    m_wireFrame3D.Append(ref item_2Trf);
                    m_wireFrame3D.Append(ref item_0Trf);
                }
            }
        }
    }
}
