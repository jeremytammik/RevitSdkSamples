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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit; // Revit API namespace
using Autodesk.Revit.Elements; 
using Autodesk.Revit.Geometry;
using System.Drawing.Drawing2D; 

namespace Revit.SDK.Samples.BlendVertexConnectTable.CS
{
    /// <summary>
    /// Vertex connect table form, shows all vertexes connect information and preview blend edges
    /// </summary>
    public partial class VertexConnectTableForm : Form
    {
        #region VertexConnectTableForm member variable
        // external command data
        ExternalCommandData m_commandData;

        // vertex connect data generator
        VertexConnectDataGenerator m_vertexConGen;

        // scale transform of converting figure in 3d view to picture box
        double m_scaleTransform = 0.0; 
        #endregion


        #region VertexConnectTableForm Public 
        /// <summary>
        /// Construction of VertexConnectTableForm class
        /// </summary>
        /// <param name="commandData"> External command data used to get blend and view </param>
        public VertexConnectTableForm(ExternalCommandData commandData)
        {
            // UI initialization
            InitializeComponent();
            
            // command data initialization
            m_commandData = commandData;

            // generate all vertices & vertex connect data of Blend
            m_vertexConGen = new VertexConnectDataGenerator(m_commandData);

            //  transform geometry data to current view and picture box
            m_vertexConGen.GeometryDataTransform(m_commandData.Application.ActiveDocument.ActiveView);
            m_scaleTransform = m_vertexConGen.GetTransformScale(geomPictureBox.Size);

            // show the vertex connect table in the data grid view
            vertexConnectGridView.DataSource = new DataView(CreateDataTable());
            this.vertexConnectGridView.Columns[0].Width = this.vertexConnectGridView.Width / 3;
            this.vertexConnectGridView.Columns[1].Width = this.vertexConnectGridView.Width / 3;
            this.vertexConnectGridView.Columns[2].Width = this.vertexConnectGridView.Width / 3;
            vertexConnectGridView.Sort(vertexConnectGridView.Columns[0], ListSortDirection.Ascending);
        }
        #endregion


        #region VertexConnectTableForm Implementations
        /// <summary>
        /// Create data table from vertex connect data list
        /// </summary>
        /// <returns>The created DataTable to be shown in DataGridView</returns>
        private DataTable CreateDataTable()
        {
            // Create new DataTable and DataSource objects.
            DataTable vertexConnectDataTable = new DataTable();

            // Create new DataColumn, set DataType, ColumnName and add to DataTable.    
            CreateDataColumn(ref vertexConnectDataTable, "Index");
            CreateDataColumn(ref vertexConnectDataTable, "Top Index");
            CreateDataColumn(ref vertexConnectDataTable, "Bottom Index");

            // add data to this table
            for (int i = 0; i < m_vertexConGen.VertexConnectTableList.Count; i++)
            {
                // get the each data in list
                VertexConnectTableData item = m_vertexConGen.VertexConnectTableList[i];

                // create data row and add cells' data
                DataRow vertexConnectRow            = vertexConnectDataTable.NewRow();
                vertexConnectRow["Index"]           = item.TableIndex;
                vertexConnectRow["Top Index"]       = item.TopIndex;
                vertexConnectRow["Bottom Index"]    = item.BottomIndex;
                vertexConnectDataTable.Rows.Add(vertexConnectRow);
            }

            return vertexConnectDataTable;
        }


        /// <summary>
        /// Create column for DataGridView
        /// </summary>
        /// <param name="dataTable">Data table to be added new rows</param>
        /// <param name="columnName">Name for new column</param>
        private void CreateDataColumn(ref DataTable dataTable, string columnName)
        {
            // Create new DataColumn, set DataType, ColumnName and add to DataTable.    
            DataColumn column   = new DataColumn();
            column.DataType     = System.Type.GetType("System.Int32");
            column.ColumnName   = columnName;
            column.ReadOnly     = true;
            dataTable.Columns.Add(column);
        }


        /// <summary>
        /// Paints the picture box control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void geomPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // clear back group to white color
            e.Graphics.Clear(System.Drawing.Color.White);

            // move the coordinate system to center of picture box        
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            g.Transform = new Matrix(1, 0, 0, 1, rect.Width / 2, rect.Height / 2);
            g.SmoothingMode = SmoothingMode.HighSpeed;

            // scale transform for the pain rectangle 
            g.ScaleTransform((float)m_scaleTransform, (float)m_scaleTransform);

            // create pen and mark font for drawing
            Pen m_edgePen = new Pen(Brushes.Blue, (float)(1.0 / m_scaleTransform));
            Pen m_sketchPen = new Pen(Brushes.Black, (float)(1.0 / m_scaleTransform));
            Font m_markFont = new Font("Times New Roman", (float)(7.5 / m_scaleTransform), FontStyle.Bold);

            // transform the coordinate system to the center of all blend vertexes to let all edges fit the picture window
            g.TranslateTransform(-(float)(m_vertexConGen.VertexesCenter.X), (float)(m_vertexConGen.VertexesCenter.Y));

            // draw all vertex connect edges
            for (int i = 0; i < m_vertexConGen.VertexConnectList.Count; i++)
            {
                VertexConnectData item = m_vertexConGen.VertexConnectList[i];
                PointF pnt1 = new PointF((float)m_vertexConGen.TopVertexList[item.TopIndex].X, 
                    -(float)m_vertexConGen.TopVertexList[item.TopIndex].Y);
                PointF pnt2 = new PointF((float)m_vertexConGen.BottomVertexList[item.BottomIndex].X, 
                    -(float)m_vertexConGen.BottomVertexList[item.BottomIndex].Y);

                g.DrawLine(m_edgePen, pnt1, pnt2);
            } 

            // draw the edges of top sketch
            for (int i = 0; i < m_vertexConGen.TopVertexList.Count; i++)
            {
                // get the start and end points of line
                XYZ xyzFirst = m_vertexConGen.TopVertexList[i];
                XYZ xyzSecond;
                if (i == m_vertexConGen.TopVertexList.Count - 1)
                {
                    xyzSecond = m_vertexConGen.TopVertexList[0];
                }
                else
                {
                    xyzSecond = m_vertexConGen.TopVertexList[i + 1];
                }

                // draw the line and vertex mark: T1, t2, ....
                PointF pnt1 = new PointF((float)xyzFirst.X, -(float)xyzFirst.Y);
                PointF pnt2 = new PointF((float)xyzSecond.X, -(float)xyzSecond.Y);
                g.DrawLine(m_sketchPen, pnt1, pnt2);

                // draw mark string
                if (m_vertexConGen.TopIndexDict.ContainsKey(i))
                {
                    int index = 0;
                    m_vertexConGen.TopIndexDict.TryGetValue(i, out index);
                    e.Graphics.DrawString("T" + index.ToString(), m_markFont, SystemBrushes.WindowText, pnt1.X, pnt1.Y);
                }
            }

            // draw the edges of bottom sketch
            for (int i = 0; i < m_vertexConGen.BottomVertexList.Count; i++)
            {
                // get the start and end points of line
                XYZ xyzFirst = m_vertexConGen.BottomVertexList[i];
                XYZ xyzSecond;
                if (i == m_vertexConGen.BottomVertexList.Count - 1)
                {
                    xyzSecond = m_vertexConGen.BottomVertexList[0];
                }
                else
                {
                    xyzSecond = m_vertexConGen.BottomVertexList[i + 1];
                }

                // draw the line and vertex mark: B1, B2, ....
                PointF pnt1 = new PointF((float)xyzFirst.X, -(float)xyzFirst.Y);
                PointF pnt2 = new PointF((float)xyzSecond.X, -(float)xyzSecond.Y);
                g.DrawLine(m_sketchPen, pnt1, pnt2);

                // draw mark string for bottom vertexes
                if (m_vertexConGen.BottomIndexDict.ContainsKey(i))
                {
                    int index;
                    m_vertexConGen.BottomIndexDict.TryGetValue(i, out index);                
                    e.Graphics.DrawString("B" + index.ToString(), m_markFont, SystemBrushes.WindowText, pnt1.X, pnt1.Y);
                }
            } 
        }


        /// <summary>
        /// Invalidates the preview picture when move form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VertexConnectTableForm_Move(object sender, EventArgs e)
        {
            geomPictureBox.Invalidate();
        }


        /// <summary>
        /// Close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}