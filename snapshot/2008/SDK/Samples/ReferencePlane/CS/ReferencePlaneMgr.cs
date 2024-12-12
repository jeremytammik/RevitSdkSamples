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
using System.Diagnostics;
using System.Data;
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;

using Element = Autodesk.Revit.Element;
using GElement = Autodesk.Revit.Geometry.Element;    

namespace Revit.SDK.Samples.ReferencePlane.CS
{
    /// <summary>
    /// A object to manage reference plane.
    /// </summary>
    public class ReferencePlaneMgr
    {
        private Document m_document;   //the currently active project
        private Options m_options;  //User preferences for parsing of geometry.
        //The datasource for a DataGridView control.
        private DataTable m_referencePlanes;        
        //A dictionary for create reference plane with different host element.
        private Dictionary<Type, CreateDelegate> m_createHandler;

        /// <summary>
        /// The datasource for a DataGridView control.
        /// </summary>
        public DataTable ReferencePlanes
        {
            get
            {
                GetAllReferencePlanes();
                return m_referencePlanes;
            }
        }

        //A delegate for create reference plane with different host element.
        private delegate void CreateDelegate(Element host);

        /// <summary>
        /// A ReferencePlaneMgr object constructor.
        /// </summary>
        /// <param name="commandData">The ExternalCommandData object for the active
        /// instance of Autodesk Revit.</param>
        public ReferencePlaneMgr(ExternalCommandData commandData)
        {
            Debug.Assert(null != commandData);

            m_document = commandData.Application.ActiveDocument;

            //Get an instance of this class from Application.Create
            m_options = commandData.Application.Create.NewGeometryOptions();
            //Set your preferences and pass it to Element.Geometry or Instance.Geometry.
            m_options.ComputeReferences = true;
            //m_options.DetailLevel = Options.DetailLevels.Fine;
            m_options.View = m_document.ActiveView;
            

            m_createHandler = new Dictionary<Type, CreateDelegate>();
            m_createHandler.Add(typeof(Wall), new CreateDelegate(OperateWall));
            m_createHandler.Add(typeof(Floor), new CreateDelegate(OperateSlab));

            InitializeDataTable();
        }

        /// <summary>
        /// Create reference plane with the selected element.
        /// the selected element must be wall or slab at this sample code.
        /// </summary>
        public void Create()
        {
            foreach (Element e in m_document.Selection.Elements)
            {
                try
                {
                    CreateDelegate createDelegate = m_createHandler[e.GetType()];
                    createDelegate(e);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Initialize a DataTable object which is datasource of a DataGridView control.
        /// </summary>
        private void InitializeDataTable()
        {
            m_referencePlanes = new DataTable("ReferencePlanes");
            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;

            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            // Add the Column to the DataColumnCollection.
            m_referencePlanes.Columns.Add(column);

            // Create second column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "BubbleEnd";
            // Add the column to the table.
            m_referencePlanes.Columns.Add(column);

            // Create third column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "FreeEnd";
            // Add the column to the table.
            m_referencePlanes.Columns.Add(column);

            // Create fourth column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Normal";
            // Add the column to the table.
            m_referencePlanes.Columns.Add(column);


            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = m_referencePlanes.Columns["ID"];
            m_referencePlanes.PrimaryKey = PrimaryKeyColumns;
        }

        /// <summary>
        /// Format the output string for a point.
        /// </summary>
        /// <param name="point">A point to show in UI.</param>
        /// <returns>The display string for a point.</returns>
        private string Format(XYZ point)
        {
            return "(" + Math.Round(point.X, 2).ToString() +
                    ", " + Math.Round(point.Y, 2).ToString() +
                    ", " + Math.Round(point.Z, 2).ToString() + ")";
        }

        /// <summary>
        /// Get all reference planes in current revit project.
        /// </summary>
        /// <returns>The number of all reference planes.</returns>
        private int GetAllReferencePlanes()
        {
            m_referencePlanes.Clear();
            DataRow row;

            ElementIterator itor = m_document.Elements;
            Autodesk.Revit.Elements.ReferencePlane refPlane = null;

            itor.Reset();
            while (itor.MoveNext())
            {
                refPlane = itor.Current as Autodesk.Revit.Elements.ReferencePlane;
                if (null == refPlane)
                {
                    continue;
                }
                else
                {
                    row = m_referencePlanes.NewRow();
                    row["ID"] = refPlane.Id.Value;
                    row["BubbleEnd"] = Format(refPlane.BubbleEnd);
                    row["FreeEnd"] = Format(refPlane.FreeEnd);
                    row["Normal"] = Format(refPlane.Normal);
                    m_referencePlanes.Rows.Add(row);
                }
            }

            return m_referencePlanes.Rows.Count;
        }

        /// <summary>
        /// Create reference plane for a wall.
        /// </summary>
        /// <param name="host">A wall element.</param>
        private void OperateWall(Element host)
        {
            Wall wall = host as Wall;
            XYZ bubbleEnd = new XYZ();
            XYZ freeEnd = new XYZ();
            XYZ cutVec = new XYZ();

            LocateWall(wall, ref bubbleEnd, ref freeEnd, ref cutVec);
            m_document.Create.NewReferencePlane(bubbleEnd, freeEnd, cutVec, m_document.ActiveView);
        }

        /// <summary>
        /// Create reference plane for a slab.
        /// </summary>
        /// <param name="host">A floor element.</param>
        private void OperateSlab(Element host)
        {
            Floor floor = host as Floor;
            XYZ bubbleEnd = new XYZ();
            XYZ freeEnd = new XYZ();
            XYZ thirdPnt = new XYZ();
            LocateSlab(floor, ref bubbleEnd, ref freeEnd, ref thirdPnt);
            m_document.Create.NewReferencePlane2(bubbleEnd, freeEnd, thirdPnt, m_document.ActiveView);
        }

        /// <summary>
        /// Located the exterior of a wall object.
        /// </summary>
        /// <param name="wall">A wall object</param>
        /// <param name="bubbleEnd">The bubble end of new reference plane.</param>
        /// <param name="freeEnd">The free end of new reference plane.</param>
        /// <param name="cutVec">The cut vector of new reference plane.</param>
        private void LocateWall(Wall wall, ref XYZ bubbleEnd, ref XYZ freeEnd, ref XYZ cutVec)
        {
            LocationCurve location = wall.Location as LocationCurve;
            Curve locaCurve = location.Curve;

            //Not work for wall without location.
            if (null == locaCurve)
            {
                throw new Exception("This wall has no location.");
            }

            //Not work for arc wall.
            Line line = locaCurve as Line;
            if (null == line)
            {
                throw new Exception("Just work for straight wall.");
            }

            //Calculate offset by law of cosines.
            double halfThickness = wall.Width / 2;
            double length = GeoHelper.GetLength(locaCurve.get_EndPoint(0), locaCurve.get_EndPoint(1));
            double xAxis = GeoHelper.GetDistance(locaCurve.get_EndPoint(0).X, locaCurve.get_EndPoint(1).X);
            double yAxis = GeoHelper.GetDistance(locaCurve.get_EndPoint(0).Y, locaCurve.get_EndPoint(1).Y);

            double xOffset = yAxis * halfThickness / length;
            double yOffset = xAxis * halfThickness / length;

            if (locaCurve.get_EndPoint(0).X < locaCurve.get_EndPoint(1).X
                && locaCurve.get_EndPoint(0).Y < locaCurve.get_EndPoint(1).Y)
            {
                xOffset = -xOffset;
            }
            if (locaCurve.get_EndPoint(0).X > locaCurve.get_EndPoint(1).X
                && locaCurve.get_EndPoint(0).Y > locaCurve.get_EndPoint(1).Y)
            {
                yOffset = -yOffset;
            }
            if (locaCurve.get_EndPoint(0).X > locaCurve.get_EndPoint(1).X
                && locaCurve.get_EndPoint(0).Y < locaCurve.get_EndPoint(1).Y)
            {
                xOffset = -xOffset;
                yOffset = -yOffset;
            }

            //Three necessary parameters for generate a reference plane.
            bubbleEnd = new XYZ(locaCurve.get_EndPoint(0).X + xOffset,
                locaCurve.get_EndPoint(0).Y + yOffset, locaCurve.get_EndPoint(0).Z);
            freeEnd = new XYZ(locaCurve.get_EndPoint(1).X + xOffset,
                locaCurve.get_EndPoint(1).Y + yOffset, locaCurve.get_EndPoint(1).Z);
            cutVec = new XYZ(0, 0, 1);
        }

        /// <summary>
        /// Located the buttom of a slab object.
        /// </summary>
        /// <param name="floor">A floor object.</param>
        /// <param name="bubbleEnd">The bubble end of new reference plane.</param>
        /// <param name="freeEnd">The free end of new reference plane.</param>
        /// <param name="thirdPnt">The third point of new reference plane.</param>
        private void LocateSlab(Floor floor, ref XYZ bubbleEnd, ref XYZ freeEnd, ref XYZ thirdPnt)
        {
            //Obtain the geometry data of the floor.
            GElement geometry = floor.get_Geometry(m_options);
            Face buttomFace = null;

            foreach (GeometryObject go in geometry.Objects)
            {
                Solid solid = go as Solid;
                if (null == solid)
                {
                    continue;
                }
                else
                {
                    //Get the bottom face of this floor.
                    buttomFace = GeoHelper.GetBottomFace(solid.Faces);
                }
            }

            Mesh mesh = buttomFace.Triangulate();
            GeoHelper.Distribute(mesh, ref bubbleEnd, ref freeEnd, ref thirdPnt);
        }
    }
}
