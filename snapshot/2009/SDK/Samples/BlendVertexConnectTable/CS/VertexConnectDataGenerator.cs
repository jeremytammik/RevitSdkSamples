//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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

using Autodesk.Revit; // Revit API namespace
using Autodesk.Revit.Elements; 
using Autodesk.Revit.Geometry;
using System.Drawing.Drawing2D;
using System.Drawing; 

namespace Revit.SDK.Samples.BlendVertexConnectTable.CS
{
    /// <summary>
    /// Generate the vertices and vertex connection data
    /// </summary>
    class VertexConnectDataGenerator
    {
        #region VertexConnectDataGenerator Member Variables
        // double epsilon for double values comparison
        const double DOUBLE_EPS = 1.0E-9;

        // external command data
        ExternalCommandData m_commandData;

        // the selected blend object
        Autodesk.Revit.Elements.Blend m_blend;

        // top vertex array
        List<XYZ> m_topVertexList = new List<XYZ>();

        // bottom bottom vertex list
        List<XYZ> m_bottomVertexList = new List<XYZ>();

        // vertex connect data and table data list
        // the dictionary are used to get real index and virtual index
        SortedList<int, int> m_topIndexDict = new SortedList<int, int>();
        SortedList<int, int> m_bottomIndexDict = new SortedList<int, int>();
        List<VertexConnectData> m_vertexConnectList = new List<VertexConnectData>();
        List<VertexConnectTableData> m_vertexConnectTableList = new List<VertexConnectTableData>();

        // the center of all vertices of blend, z coordinate is ignored
        bool m_vertexMaxMinVertexOk = false;
        XYZ m_maxVertex = new XYZ();
        XYZ m_minVertex = new XYZ();
        XYZ m_vertexesCenter = new XYZ();
        #endregion


        #region VertexConnectDataGenerator Public Properties
        /// <summary>
        /// Top vertexes list of blend
        /// </summary>
        public List<XYZ> TopVertexList
        {
            get
            {
                return m_topVertexList;
            }
        }


        /// <summary>
        /// Bottom vertexes list of blend
        /// </summary>
        public List<XYZ> BottomVertexList
        {
            get
            {
                return m_bottomVertexList;
            }
        }


        /// <summary>
        /// Sorted top vertexes index 
        /// </summary>
        public SortedList<int, int> TopIndexDict
        {
            get
            {
                return m_topIndexDict;
            }
        }


        /// <summary>
        /// Sorted bottom vertexes index
        /// </summary>
        public SortedList<int, int> BottomIndexDict
        {
            get
            {
                return m_bottomIndexDict;
            }
        }


        /// <summary>
        /// Vertex connect data list
        /// </summary>
        public List<VertexConnectData> VertexConnectList
        {
            get
            {
                return m_vertexConnectList;
            }
        }


        /// <summary>
        /// Vertex connect table data list
        /// </summary>
        public List<VertexConnectTableData> VertexConnectTableList
        {
            get
            {
                return m_vertexConnectTableList;
            }
        }


        /// <summary>
        /// Center vetices of all vertexes
        /// </summary>
        public XYZ VertexesCenter
        {
            get
            {
                if (false == m_vertexMaxMinVertexOk)
                {
                    GetMaxMinVertexes();
                }
                return m_vertexesCenter;
            }
        }
        #endregion


        #region VertexConnectDataGenerator Ctor & Public Methods


        /// <summary>
        /// Generate vertex connect data
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        public VertexConnectDataGenerator(ExternalCommandData commandData)
        {
            // retrieve blend object from selected element
            bool bSucceed = GetBlendElement((m_commandData = commandData), ref m_blend);
            if (!bSucceed)
            {
                throw new Exception("Failed to find any blend object. Please select a blend object.");
            }

            // get vertexes information: vertices and vertex connect data
            GetVertexConnectList();
        }


        /// <summary>
        /// transform all the geometry data to be painted in picture box
        /// </summary>
        public void GeometryDataTransform(Autodesk.Revit.Elements.View currentView)
        {
            // create projection matrix of view and inverse it for preview the edges
            XYZ origin = new XYZ(currentView.Origin.X, currentView.Origin.Y, currentView.Origin.Z);
            Transform viewTransform = Transform.get_Translation(origin);
            viewTransform.BasisX = currentView.RightDirection;
            viewTransform.BasisY = currentView.UpDirection;
            viewTransform.BasisZ = currentView.ViewDirection;

            // inverse the view projection matrix
            viewTransform = viewTransform.Inverse;

            // update all vertexes in top sketch and bottom sketch
            for (int i = 0; i < m_bottomVertexList.Count; i++)
            {
                XYZ xyz = m_bottomVertexList[i];
                m_bottomVertexList[i] = viewTransform.OfPoint(xyz);
            }
            for (int i = 0; i < m_topVertexList.Count; i++)
            {
                XYZ xyz = m_topVertexList[i];
                m_topVertexList[i] = viewTransform.OfPoint(xyz);
            }

            // reset max and min vertexes of all vertexes
            m_vertexMaxMinVertexOk = false;
            GetMaxMinVertexes();
        }


        /// <summary>
        /// Get scale transform
        /// </summary>
        /// <param name="size">Size as base to get scale, sometimes it is size of picture box</param>
        /// <returns></returns>
        public double GetTransformScale(Size size)
        {
            // whether max & min vertexes are available now
            if (m_vertexMaxMinVertexOk == false)
            {
                GetMaxMinVertexes();
            }

            // get width & height of blend
            double m_selectionBoxWidth = Math.Abs(m_maxVertex.X - m_minVertex.X);
            double m_selectionBoxHeight = Math.Abs(m_maxVertex.Y - m_minVertex.Y);

            // get scale transform, white margin (1 - 0.85 = 0.15) is for showing string
            double scaleX = size.Width / m_selectionBoxWidth;
            double scaleY = size.Height / m_selectionBoxHeight;
            double scaleTransform = ((scaleX <= scaleY) ? (scaleX) : (scaleY)) * 0.85;
            return scaleTransform;
        }
        #endregion


        #region VertexConnectDataGenerator Implementations
        /// <summary>
        /// Retrieves blend element instance form external command
        /// </summary>
        /// <param name="commandData"> Command data of revit used to find blend element </param>
        /// <param name="blendObj">The retrieved object </param>
        /// <returns>true: get blend instance successfully, false: fail to get blend instance </returns>
        private bool GetBlendElement(ExternalCommandData commandData, 
            ref Autodesk.Revit.Elements.Blend blendObj)
        {
            // get the selected Blend object: blend is a family instance too.
            FamilyInstance familyInstance = null;
            SelElementSet selections = commandData.Application.ActiveDocument.Selection.Elements;
            ElementSetIterator iter = selections.ForwardIterator();
            iter.Reset();
            while (iter.MoveNext())
            {
                familyInstance = iter.Current as FamilyInstance;
                if (null != familyInstance)
                {
                    break;
                }
            }

            // get blend object from family instance via solidform in symbol's family of solid blend mass
            if (familyInstance != null)
            {
                GenericFormSet genFormSet = familyInstance.Symbol.Family.SolidForms;
                if (null == genFormSet || genFormSet.Size != 1)
                {
                    return false;
                }
                
                // get iterator of solid form
                GenericFormSetIterator formIter = genFormSet.ForwardIterator();
                formIter.Reset();
                if (!formIter.MoveNext())
                {
                    return false;
                }

                // get the blend object
                blendObj = formIter.Current as Autodesk.Revit.Elements.Blend;
            }

            // check whether retrieved the blend successfully
            if (null == blendObj)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Get all vertex connect data from blend object,
        /// then convert these connect vertexes to table data which will shown in DataGridView control
        /// </summary>
        private void GetVertexConnectList()
        {
            #region Get All Vertices of Bottom Sketch
            //
            // get all vertices in bottom sketch
            CurveArray bottomSketchCurves = m_blend.BottomSketch.CurveLoop;
            foreach (Curve cur in bottomSketchCurves)
            {
                // add the first vertices 
                if (cur.IsBound)
                {
                    XYZArray points = cur.Tessellate();
                    foreach (XYZ point in points)
                    {
                        XYZ newPoint = new XYZ(point.X, point.Y, point.Z);
                        newPoint.Z = m_blend.FirstEnd;
                        if (-1 == ListContainVertex(m_bottomVertexList, newPoint))
                        {
                            m_bottomVertexList.Add(newPoint);
                        }
                    }
                }
            }
            #endregion


            #region Get All Vertices of Top Sketch
            //
            // get all vertices in top sketch
            CurveArray topSketchCurves = m_blend.TopSketch.CurveLoop;
            foreach (Curve cur in topSketchCurves)
            {
                // add the first vertices 
                if (cur.IsBound)
                {
                    XYZArray points = cur.Tessellate();
                    foreach (XYZ point in points)
                    {
                        XYZ newPoint = new XYZ(point.X, point.Y, point.Z);
                        newPoint.Z = m_blend.SecondEnd;
                        if (-1 == ListContainVertex(m_topVertexList, newPoint))
                        {
                            m_topVertexList.Add(newPoint);
                        }
                    }
                }
            }
            #endregion


            #region Get Vertex Connect Table of Blend
            //
            // get all vertex connect edges
            // if the two vertexes of edge belong to different sketches, it's a vertex connect edge.
            Autodesk.Revit.Geometry.Options geomOpt = m_commandData.Application.Create.NewGeometryOptions();
            //geomOpt.DetailLevel = Autodesk.Revit.Geometry.Options.DetailLevels.Medium;
            geomOpt.View = m_commandData.Application.ActiveDocument.ActiveView;
            Autodesk.Revit.Geometry.Element geomElem = m_blend.get_Geometry(geomOpt);
            Autodesk.Revit.Geometry.GeometryObjectArray geomObjs = geomElem.Objects;
            foreach (Autodesk.Revit.Geometry.GeometryObject geomObj in geomObjs)
            {
                // get solid object
                Autodesk.Revit.Geometry.Solid solid = geomObj as Autodesk.Revit.Geometry.Solid;
                if (null == solid)
                {
                    continue;
                }

                // get the edges of solid
                Autodesk.Revit.Geometry.EdgeArrayIterator editor = solid.Edges.ForwardIterator();
                while (editor.MoveNext())
                {
                    // the edge should be line
                    Autodesk.Revit.Geometry.Edge crv = editor.Current as Autodesk.Revit.Geometry.Edge;

                    if (crv != null)
                    {
                        // check the points are top or bottom vertexes
                        int topVertexIndex = -1;
                        int bottomVertexIndex = -1;
                        XYZ startPoint = crv.Evaluate(0.0);
                        XYZ endPoint = crv.Evaluate(1.0);

                        // skip the edge in same sketch
                        if (DoubleEqual(startPoint.Z, endPoint.Z))
                        {
                            continue;
                        }

                        // check which point is bottom vertex, which one is top vertex
                        if (DoubleEqual(startPoint.Z, m_blend.FirstEnd))
                        {
                            // startPoint is bottom sketch vertex, endPoint is top sketch vertex
                            bottomVertexIndex = ListContainVertex(m_bottomVertexList, startPoint);
                            topVertexIndex = ListContainVertex(m_topVertexList, endPoint);
                        }
                        else
                        {
                            // startPoint is top sketch vertex, endPoint is bottom sketch vertex
                            bottomVertexIndex = ListContainVertex(m_bottomVertexList, endPoint);
                            topVertexIndex = ListContainVertex(m_topVertexList, startPoint);
                        }

                        // check if positions of the two vertices are valid: both of them should not be -1
                        if (-1 == bottomVertexIndex || -1 == topVertexIndex)
                        {
                            new Exception("Error occurs: Vertex doesn't belong to sketches of blend.");
                        }
                        else
                        {
                            // new one vertex connect data item and add it to list
                            VertexConnectData item = new VertexConnectData(bottomVertexIndex, topVertexIndex);
                            m_vertexConnectList.Add(item);
                        }
                    }
                }
            }
            #endregion


            #region Get Vertex Connect Table Data
            //
            // create temporary vertex connect dictionaries.
            // the dictionary key only reserve the index of vertexes.
            // the value of key is ignored.
            SortedList<int, int> tmpBottomIndexDict = new SortedList<int, int>();
            SortedList<int, int> tmpTopIndexDict = new SortedList<int, int>();
            for (int i = 0; i < m_vertexConnectList.Count; i++)
            {
                VertexConnectData connectData = m_vertexConnectList[i];

                // get all bottom index of vertexes which are connect vertexes.
                if (false == tmpBottomIndexDict.ContainsKey(connectData.BottomIndex))
                {
                    tmpBottomIndexDict.Add(connectData.BottomIndex, -1);
                }

                // get all top index of vertexes which are connect vertexes.
                if (false == tmpTopIndexDict.ContainsKey(connectData.TopIndex))
                {
                    tmpTopIndexDict.Add(connectData.TopIndex, -1);
                }
            }

            // update the value of key in sorted list, value will start from 0, accelerating 1 for next key.
            // then, create the vertex connect table data list with keys and values
            int tableIndex = 0;
            for (int i = 0; i < m_vertexConnectList.Count; i++)
            {
                VertexConnectData connectData = m_vertexConnectList[i];

                // check whether the vertex connect: the IndexOfKey should return a value not less than 0.
                int virBottomValue = tmpBottomIndexDict.IndexOfKey(connectData.BottomIndex) + 1;
                int virtualTopIndex = tmpTopIndexDict.IndexOfKey(connectData.TopIndex) + 1;
                if (virBottomValue <= 0 || virtualTopIndex <= 0)
                {
                    throw new Exception("Fatal error: failed to get vertex index");
                }

                // check whether vertex index exists in list, if not, add it to list
                if (false == m_topIndexDict.ContainsKey(connectData.TopIndex))
                {
                    m_topIndexDict.Add(connectData.TopIndex, virtualTopIndex);
                }
                if (false == m_bottomIndexDict.ContainsKey(connectData.BottomIndex))
                {
                    m_bottomIndexDict.Add(connectData.BottomIndex, virBottomValue);
                }

                // new vertex connect table data and add it to list
                VertexConnectTableData newTableData = 
                    new VertexConnectTableData(++tableIndex, virBottomValue, virtualTopIndex);
                m_vertexConnectTableList.Add(newTableData);
            }
            #endregion
        }

        /// <summary>
        /// Check whether vertex is already contained in vertex list
        /// </summary>
        /// <param name="vertexList">Vertexes list</param>
        /// <param name="xyz">The vertices to be checked </param>
        /// <returns>-1, the vertex doesn't exist in the vertex list; 
        /// other value: the vertex exists in a vertex array, the value is the position of vertex in list </returns>
        private int ListContainVertex(List<XYZ> vertexList, XYZ xyz)
        {
            int pos = 0;
            foreach (XYZ val in vertexList)
            {
                if (XYZEqual(val, xyz))
                {
                    return pos;
                }
                pos++;
            }

            // the list doesn't have this vertices
            return -1;
        }


        /// <summary>
        /// Check whether two vertexes are equal
        /// </summary>
        /// <param name="val1"> First vertex </param>
        /// <param name="val2"> Second vertex </param>
        /// <returns>true: the two vertexes are equal, false: different vertexes</returns>
        private bool XYZEqual(XYZ val1, XYZ val2)
        {
            if (DoubleEqual(val1.X, val2.X) && DoubleEqual(val1.Y, val2.Y) && DoubleEqual(val1.Z, val2.Z))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Check whether two vertexes are equal
        /// </summary>
        /// <param name="val1">First double value</param>
        /// <param name="val2">Second double value</param>
        /// <returns></returns>
        private bool DoubleEqual(double val1, double val2)
        {
            return (Math.Abs(val1 - val2) <= DOUBLE_EPS);
        }


        /// <summary>
        /// Get max and min vertexes of all vertexes of blend object
        /// </summary>
        /// <returns></returns>
        private void GetMaxMinVertexes()
        {
            if (false == m_vertexMaxMinVertexOk)
            {
                // update flag
                m_vertexMaxMinVertexOk = true;

                // Get max and min points in top vertexes and bottom vertexes.
                m_maxVertex.X = m_maxVertex.Y = m_maxVertex.Z = -double.MaxValue;
                m_minVertex.X = m_minVertex.Y = m_minVertex.Z = double.MaxValue;
                foreach (XYZ xyz in m_topVertexList)
                {
                    // get max point
                    if (xyz.X >= m_maxVertex.X)
                    {
                        m_maxVertex.X = xyz.X;
                    }
                    if (xyz.Y >= m_maxVertex.Y)
                    {
                        m_maxVertex.Y = xyz.Y;
                    }

                    // get min point
                    if (xyz.X <= m_minVertex.X)
                    {
                        m_minVertex.X = xyz.X;
                    }
                    if (xyz.Y <= m_minVertex.Y)
                    {
                        m_minVertex.Y = xyz.Y;
                    }
                }
                // update them once more from bottom vertexes
                foreach (XYZ xyz in m_bottomVertexList)
                {
                    // max point
                    if (xyz.X >= m_maxVertex.X)
                    {
                        m_maxVertex.X = xyz.X;
                    }
                    if (xyz.Y >= m_maxVertex.Y)
                    {
                        m_maxVertex.Y = xyz.Y;
                    }

                    // min point
                    if (xyz.X <= m_minVertex.X)
                    {
                        m_minVertex.X = xyz.X;
                    }
                    if (xyz.Y <= m_minVertex.Y)
                    {
                        m_minVertex.Y = xyz.Y;
                    }
                }

                // get the center point of all vertex
                // z coordinate value is ignored
                m_vertexesCenter.X = (m_maxVertex.X + m_minVertex.X) / 2.0;
                m_vertexesCenter.Y = (m_maxVertex.Y + m_minVertex.Y) / 2.0;
            }
        }
        #endregion
    }
}
