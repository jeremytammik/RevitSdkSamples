//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Symbols;

using GeoInstance = Autodesk.Revit.Geometry.Instance;
using GeoElement = Autodesk.Revit.Geometry.Element;
using RevitElement = Autodesk.Revit.Element;

namespace Revit.SDK.Samples.PlaceFamilyInstanceByFace.CS
{
    /// <summary>
    /// This is main data class for creating family Instance by face
    /// </summary>
    public class FamilyInstanceCreator
    {
        #region Fields
        // Revit document
        private Document m_revitDoc;
        private Autodesk.Revit.Creation.Application m_appCreator;
        // all face names
        private List<String> m_faceNameList = new List<String>();
        // all face instances
        private List<Face> m_faceList = new List<Face>();
        // all family symbols
        private List<FamilySymbol> m_familySymbolList = new List<FamilySymbol>();
        // all family symbol names
        private List<String> m_familySymbolNameList = new List<String>();
        // the index default family symbol in family list
        private int m_defaultIndex = -1; 
        #endregion

        #region Properties
        /// <summary>
        /// Store the all face names, they will be displayed in a combo box
        /// </summary>
        public List<String> FaceNameList
        {
            get
            {
                return m_faceNameList;
            }
        }

        /// <summary>
        /// Store all face instances for convenience to create a face-based family instance 
        /// </summary>
        public List<Face> FaceList
        {
            get
            {
                return m_faceList;
            }
        }

        /// <summary>
        /// Store all family symbol in current Revit document
        /// </summary>
        public List<FamilySymbol> FamilySymbolList
        {
            get
            {
                return m_familySymbolList;
            }
        }

        /// <summary>
        /// Store all family symbol names
        /// </summary>
        public List<String> FamilySymbolNameList
        {
            get
            {
                return m_familySymbolNameList;
            }
        }

        /// <summary>
        /// The index of default family symbol, will set it as default value when initializing UI 
        /// For based point, its name is "Point-based"
        /// For based line, its name is "Line-based"
        /// The prepared rfa files provide them 
        /// </summary>
        public int DefaultFamilySymbolIndex
        {
            get
            {
                return m_defaultIndex;
            }
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Constructor, Store the Revit application
        /// </summary>
        /// <param name="app"></param>
        public FamilyInstanceCreator(Autodesk.Revit.Application app)
        {
            m_revitDoc = app.ActiveDocument;
            m_appCreator = app.Create;
            if (!CheckSelectedElementSet())
            {
                throw new Exception("Please select an element with face geometry.");
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// 1. Find all family symbols in current Revit document and store them
        /// 2. Find the index of default family symbol
        /// Point("Point-based"); Line("Line-based")
        /// </summary>
        public void CheckFamilySymbol(BasedType type)
        {
            m_defaultIndex = -1;
            m_familySymbolList.Clear();

            Autodesk.Revit.ElementIterator familySymbolItor = m_revitDoc.get_Elements(typeof(FamilySymbol));

            String defaultSymbolName = String.Empty;
            switch (type)
            {
                case BasedType.Point:
                    defaultSymbolName = "Point-based";
                    break;
                case BasedType.Line:
                    defaultSymbolName = "Line-based";
                    break;
                default:
                    break;
            }

            bool hasDefaultSymbol = false;
            int ii = 0;

            while (familySymbolItor.MoveNext())
            {
                FamilySymbol symbol = (FamilySymbol)familySymbolItor.Current;
                if(null == symbol)
                {
                    continue;
                }

                if (!hasDefaultSymbol && 0 == String.Compare(defaultSymbolName, symbol.Name))
                {
                    hasDefaultSymbol = true;
                    m_defaultIndex = ii;
                }

                // family symbol
                m_familySymbolList.Add(symbol);

                // family symbol name
                String familyCategoryname = String.Empty;
                if (null != symbol.Family.FamilyCategory)
                {
                    familyCategoryname = symbol.Family.FamilyCategory.Name + " : ";
                }
                m_familySymbolNameList.Add(String.Format("{0}{1} : {2}"
                    , familyCategoryname, symbol.Family.Name, symbol.Name));
                ii++;
            }

            if (!hasDefaultSymbol)
            {
                FamilySymbol loadedfamilySymbol = null;
                try
                {
                    m_revitDoc.LoadFamilySymbol(String.Format(@"{0}.rfa", defaultSymbolName)
                        , defaultSymbolName
                        , out loadedfamilySymbol);
                }
                catch(Exception)
                {
                    MessageBox.Show("Can't load the prepared rfa.");
                }


                if (null == loadedfamilySymbol)
                {
                    return;
                }
                m_familySymbolList.Add(loadedfamilySymbol);

                String familyCategoryname = String.Empty;
                if (null != loadedfamilySymbol.Family.FamilyCategory)
                {
                    familyCategoryname = loadedfamilySymbol.Family.FamilyCategory.Name +": ";
                }
                m_familySymbolNameList.Add(String.Format("{0}{1}: {2}"
                    , familyCategoryname, loadedfamilySymbol.Family.Name, loadedfamilySymbol.Name));
                m_defaultIndex = m_familySymbolList.Count - 1;
            }

            return;
        }

        /// <summary>
        /// Create a based-point family instance by face
        /// </summary>
        /// <param name="locationP">the location point</param>
        /// <param name="directionP">the direction</param>
        /// <param name="faceIndex">the index of the selected face</param>
        /// <param name="familySymbolIndex">the index of the selected family symbol</param>
        /// <returns></returns>
        public bool CreatePointFamilyInstance(XYZ locationP, XYZ directionP, int faceIndex
            , int familySymbolIndex)
        {
            Face face = m_faceList[faceIndex];

            FamilyInstance instance = m_revitDoc.Create.NewFamilyInstance(face
                , locationP, directionP, m_familySymbolList[familySymbolIndex]);

            m_revitDoc.Selection.Elements.Clear();
            m_revitDoc.Selection.Elements.Add(instance);
            return true;
        }

        /// <summary>
        /// Create a based-line family instance by face
        /// </summary>
        /// <param name="startP">the start point</param>
        /// <param name="endP">the end point</param>
        /// <param name="faceIndex">the index of the selected face</param>
        /// <param name="familySymbolIndex">the index of the selected family symbol</param>
        /// <returns></returns>
        public bool CreateLineFamilyInstance(XYZ startP, XYZ endP, int faceIndex
            , int familySymbolIndex)
        {
            Face face = m_faceList[faceIndex];
            XYZ projectedStartP = Project(face.Triangulate().Vertices, startP);
            XYZ projectedEndP = Project(face.Triangulate().Vertices, endP);

            if (projectedStartP.AlmostEqual(projectedEndP))
            {
                return false;
            }

            Line line = m_appCreator.NewLine(projectedStartP, projectedEndP, true);
            FamilyInstance instance = m_revitDoc.Create.NewFamilyInstance(face, line
                , m_familySymbolList[familySymbolIndex]);

            m_revitDoc.Selection.Elements.Clear();
            m_revitDoc.Selection.Elements.Add(instance);
            return true;
        }

        /// <summary>
        /// Judge whether the selected elementSet has face geometry
        /// </summary>
        /// <returns>true is having face geometry, false is having no face geometry</returns>
        public bool CheckSelectedElementSet()
        {
            // judge whether an or more element is selected
            if (1 != m_revitDoc.Selection.Elements.Size)
            {
                return false;
            }

            m_faceList.Clear();
            m_faceNameList.Clear();

            // judge whether the selected element has face geometry
            foreach (Autodesk.Revit.Element elem in m_revitDoc.Selection.Elements)
            {
                CheckSelectedElement(elem);
                break;
            }

            if (0 >= m_faceList.Count)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get the bounding box of a face, the boundingBoxXYZ will be set in UI as default value
        /// </summary>
        /// <param name="indexFace">the index of face</param>
        /// <returns>the bounding box</returns>
        public BoundingBoxXYZ GetFaceBoundingBox(int indexFace)
        {
            Mesh mesh = m_faceList[indexFace].Triangulate();

            XYZ maxP = new XYZ(double.MinValue, double.MinValue, double.MinValue);
            XYZ minP = new XYZ(double.MaxValue, double.MaxValue, double.MaxValue);
            foreach (XYZ tempXYZ in mesh.Vertices)
            {
                minP.X = Math.Min(minP.X, tempXYZ.X);
                minP.Y = Math.Min(minP.Y, tempXYZ.Y);
                minP.Z = Math.Min(minP.Z, tempXYZ.Z);

                maxP.X = Math.Max(maxP.X, tempXYZ.X);
                maxP.Y = Math.Max(maxP.Y, tempXYZ.Y);
                maxP.Z = Math.Max(maxP.Z, tempXYZ.Z);
            }

            BoundingBoxXYZ retBounding = new BoundingBoxXYZ();
            retBounding.Max = maxP;
            retBounding.Min = minP;
            return retBounding;
        }

        /// <summary>
        /// Judge whether an element has face geometry
        /// </summary>
        /// <param name="elem">the element to be checked</param>
        /// <returns>true is having face geometry, false is having no face geometry</returns>
        private bool CheckSelectedElement(RevitElement elem)
        {
            if(null == elem)
            {
                return false;
            }
            Autodesk.Revit.Geometry.Options opts = new Autodesk.Revit.Geometry.Options();
            opts.View = m_revitDoc.ActiveView;
            opts.ComputeReferences = true;
            // Get geometry of the element
            GeoElement geoElement = elem.get_Geometry(opts);
            InquireGeometry(geoElement, elem);

            return true;
        }

        /// <summary>
        /// Inquire an geometry element to get all face instances
        /// </summary>
        /// <param name="geoElement">the geometry element</param>
        /// <param name="elem">the element, it provides the prefix of face name</param>
        /// <returns></returns>
        private bool InquireGeometry(GeoElement geoElement, RevitElement elem)
        {
            if(null == geoElement || null == elem)
            {
                return false;
            }

            GeometryObjectArray geoArray = null;

            if (null != geoElement && null != geoElement.Objects)
            {
                geoArray = geoElement.Objects;
            }
            else
            {
                return false;
            }

            foreach (GeometryObject obj in geoArray)
            {
                if (obj is GeoInstance)
                {
                    GeoInstance instance = (GeoInstance)obj;
                    InquireGeometry(instance.SymbolGeometry, elem);
                }
                else if (!(obj is Solid))
                {
                    // is not Solid instance
                    continue;
                }

                // continue when obj is Solid instance
                Solid solid = obj as Solid;
                if (null == solid)
                {
                    continue;
                }
                FaceArray faces = solid.Faces;
                if (faces.IsEmpty)
                {
                    continue;
                }

                // get the face name list
                String category = String.Empty;
                if (null != elem.Category && null != elem.Name)
                {
                    category = elem.Category.Name;
                }

                int ii = 0;
                foreach (Face tempFace in faces)
                {
                    if (tempFace is PlanarFace)
                    {
                        m_faceNameList.Add(
                            String.Format("{0} : {1} ({2})", category, elem.Name, ii));
                        m_faceList.Add(tempFace);
                        ii++;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Project a point on a face
        /// </summary>
        /// <param name="xyzArray">the face points, them fix a face </param>
        /// <param name="point">the point</param>
        /// <returns>the projected point on this face</returns>
        static private XYZ Project(XYZArray xyzArray, XYZ point)
        {
            XYZ a = xyzArray.get_Item(0) - xyzArray.get_Item(1);
            XYZ b = xyzArray.get_Item(0) - xyzArray.get_Item(2);
            XYZ c = point - xyzArray.get_Item(0);

            XYZ normal = (a.Cross(b));

            try
            {
                normal = normal.Normalized;
            }
            catch (Exception)
            {
                normal = XYZ.Zero;
            }

            XYZ retProjectedPoint = point - (normal.Dot(c)) * normal;
            return retProjectedPoint;
        }
        #endregion

    }
}
