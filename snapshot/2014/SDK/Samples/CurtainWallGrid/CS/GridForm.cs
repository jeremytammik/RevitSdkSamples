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
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.CurtainWallGrid.CS
{
   /// <summary>
   /// the main windows form for users to operate on
   /// users can create curtain walls in the 1st tab page
   /// and manipulate the grid details in the 2nd tab page
   /// </summary>
   public partial class GridForm : System.Windows.Forms.Form
   {
      #region Fields
      // store the document of this sample
      MyDocument m_myDocument;

      // store whether the line to be moved has been selected
      // if the line to be moved has been selected, it's true; otherwise false
      private bool m_lineToMoveSelected = false;

      // store the mouse location of last execution operation (happened when the mouse clicks down)
      private System.Drawing.Point m_lastPoint = System.Drawing.Point.Empty;

      // stores the Ctrl key status: if Ctrl is down, the value of the variable true; otherwise false
      private bool m_ctrlKeyDown = false;
      #endregion

      #region Constructors
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="myDoc">
      /// store the document of this sample
      /// </param>
      public GridForm(MyDocument myDoc)
      {
         m_myDocument = myDoc;
         if (null == m_myDocument.UIDocument)
         {
            this.Close();
         }

         InitializeComponent();
         InitializeCustomComponent();
      }
      #endregion

      #region Private methods
      /// <summary>
      /// initialize the special UI controls which needed to be initialized manually
      /// </summary>
      private void InitializeCustomComponent()
      {
         // moniter the sample message change status
         m_myDocument.MessageChanged += new MyDocument.MessageChangedHandler(m_myDocument_MessageChanged);

         foreach (WallType type in m_myDocument.WallTypes)
         {
            wallTypeComboBox.Items.Add(type.Name);
         }

         // add items to the views combo box
         foreach (Autodesk.Revit.DB.View view in m_myDocument.Views)
         {
            string name = view.ViewType + " : " + view.Name;
            viewComboBox.Items.Add(name);
         }

         // add items to the lineOperationsComboBox
         lineOperationsComboBox.Items.Add(new LineOperation(LineOperationType.AddULine));
         lineOperationsComboBox.Items.Add(new LineOperation(LineOperationType.AddVLine));
         lineOperationsComboBox.Items.Add(new LineOperation(LineOperationType.LockOrUnlockLine));
         lineOperationsComboBox.Items.Add(new LineOperation(LineOperationType.MoveLine));
         lineOperationsComboBox.Items.Add(new LineOperation(LineOperationType.AddSegment));
         lineOperationsComboBox.Items.Add(new LineOperation(LineOperationType.RemoveSegment));
         lineOperationsComboBox.Items.Add(new LineOperation(LineOperationType.AddAllSegments));
         lineOperationsComboBox.Items.Add(new LineOperation(LineOperationType.AddAllMullions));
         lineOperationsComboBox.Items.Add(new LineOperation(LineOperationType.DeleteAllMullions));

         // initialize the wall drawing's canvas information
         {
            m_myDocument.WallGeometry.Drawing.Boundary = curtainWallPictureBox.ClientRectangle;
            System.Drawing.Point rectLocation = curtainWallPictureBox.ClientRectangle.Location;
            int rectWidth = curtainWallPictureBox.ClientRectangle.Width;
            int rectHeight = curtainWallPictureBox.ClientRectangle.Height;
            int midX = rectLocation.X + rectWidth / 2;
            int midY = rectLocation.X + rectHeight / 2;
            System.Drawing.Point originPoint = new System.Drawing.Point(midX, midY);
            m_myDocument.WallGeometry.Drawing.Origin = originPoint;
         }

         // initialize the grid drawing's information
         {
            GridDrawing gridDrawing = m_myDocument.GridGeometry.Drawing;
            gridDrawing.MouseInGridEvent += new GridDrawing.MouseInGridHandler(Drawing_MouseInGridEvent);
            gridDrawing.MouseOutGridEvent += new GridDrawing.MouseOutGridHandler(Drawing_MouseOutGridEvent);
            gridDrawing.Boundary = curtainGridPictureBox.ClientRectangle;
            // get the midpoint of the canvas region
            System.Drawing.Point rectLocation = gridDrawing.Boundary.Location;
            int rectWidth = gridDrawing.Boundary.Width;
            int rectHeight = gridDrawing.Boundary.Height;
            int midX = rectLocation.X + rectWidth / 2;
            int midY = rectLocation.Y + rectHeight / 2;
            System.Drawing.Point curtainGridMidpoint = new System.Drawing.Point(midX, midY);
            gridDrawing.Center = curtainGridMidpoint;
         }
      }

      /// <summary>
      /// update the status hints in the status strip
      /// </summary>
      void m_myDocument_MessageChanged()
      {
         //if it's an error / warning message, set the color of the text to red
         KeyValuePair<string, bool> message = m_myDocument.Message;
         if (true == message.Value)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Red;
         }
         // it's a common hint message, set the color to black
         else
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
         }
         this.operationStatusLabel.Text = message.Key;
         this.statusStrip.Refresh();
      }

      /// <summary>
      /// initialize the Windows Form's appearance
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void GridForm_Load(object sender, EventArgs e)
      {
         this.viewComboBox.SelectedIndex = 0;
         this.wallTypeComboBox.SelectedIndex = 0;
         this.operationStatusLabel.Text = "Specify the baseline of the curtain wall by clicking in the canvas area";
         this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
      }

      /// <summary>
      /// the user specifies a view plan to create the curtain wall
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void viewComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         // find which view is selected in the comboBox by obtaining its SelectedIndex value
         int indexSelected = viewComboBox.SelectedIndex;
         // the item selected in the ComboBox and the corresponding ViewPlan in the list have the same index
         // as we added them in the same order
         m_myDocument.WallGeometry.SelectedView = (ViewPlan)m_myDocument.Views[indexSelected];
      }

      /// <summary>
      /// the user specifies a wall type to create the curtain wall
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void wallTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         // find which wall type is selected in the comboBox by obtaining its SelectedIndex value
         int indexSelected = wallTypeComboBox.SelectedIndex;
         // the item selected in the ComboBox and the corresponding WallType in the list have the same index
         // as we added them in the same order
         m_myDocument.WallGeometry.SelectedWallType = m_myDocument.WallTypes[indexSelected];
      }

      /// <summary>
      /// the user clicks on the canvas to draw the baseline of the curtain wall
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// the mouse click event args
      /// </param>
      private void curtainWallPictureBox_MouseClick(object sender, MouseEventArgs e)
      {
         // if the curtain wall has been created, disable the mouse operation
         if (true == m_myDocument.WallCreated)
         {
            return;
         }

         // if there's no end point or just start point specified, allowes to add new points
         // if both start point and end point specified, just return
         m_myDocument.WallGeometry.Drawing.AddPoint(e.Location);

         // both start point and end point for the curtain wall creation have been specified.
         // allows to create the curtain wall now
         if (System.Drawing.Point.Empty != m_myDocument.WallGeometry.Drawing.WallLine2D.StartPoint &&
             System.Drawing.Point.Empty != m_myDocument.WallGeometry.Drawing.WallLine2D.EndPoint)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Click \"Create Curtain Wall\" button to create the curtain wall.";
            this.createButton.Enabled = true;
            this.clearButton.Enabled = true;
            this.curtainWallPictureBox.Refresh();
         }
      }

      /// <summary>
      /// the user moves the mouse in the canvas to find a point for the baseline of the curtain wall
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// the mouse move event args
      /// </param>
      private void curtainWallPictureBox_MouseMove(object sender, MouseEventArgs e)
      {
         // if the curtain wall has been created, disable the mouse operation
         if (true == m_myDocument.WallCreated)
         {
            return;
         }

         // record the mouse location and draw an assistant line and draw hint text to show the current corrdinate
         m_myDocument.WallGeometry.Drawing.AddMousePosition(e.Location);
         this.curtainWallPictureBox.Refresh();
      }

      /// <summary>
      /// redraw the baseline of the curtain wall
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void curtainWallPictureBox_Paint(object sender, PaintEventArgs e)
      {
         e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
         m_myDocument.WallGeometry.Drawing.Draw(e.Graphics, Pens.Blue);
      }

      /// <summary>
      /// clear the baseline in the canvas area
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void clearButton_Click(object sender, EventArgs e)
      {
         // clear the baseline of the curtain wall
         m_myDocument.WallGeometry.Drawing.RemovePoints();
         this.curtainWallPictureBox.Refresh();

         // disable the "create curtain wall" button and update the status hint
         this.clearButton.Enabled = false;
         this.createButton.Enabled = false;
         this.operationStatusLabel.Text = "Specify the baseline of the curtain wall by clicking in the canvas area";
         this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
      }

      /// <summary>
      /// click "Create Curtain Wall" button will create the curtain wall
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void createButton_Click(object sender, EventArgs e)
      {
         // if the wall has been created, return directly
         if (true == m_myDocument.WallCreated)
         {
            return;
         }

         // create the wall
         Wall wall = m_myDocument.WallGeometry.CreateCurtainWall();
         if (null == wall)
         {
            return;
         }
         m_myDocument.CurtainWall = wall;
         m_myDocument.WallCreated = true;

         // obtain the geometry data of the curtain grid
         m_myDocument.GridGeometry.ReloadGeometryData();

         // refresh the UI components' status
         this.wallTypeComboBox.Enabled = false;
         this.viewComboBox.Enabled = false;
         this.clearButton.Enabled = false;
         this.createButton.Enabled = false;
         this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
         this.operationStatusLabel.Text = "Switch to \"Curtain Grid\" tab page to manipulate the curtain grid.";
      }

      /// <summary>
      /// the mouse is out of the curtain grid area, change the cursor to default
      /// </summary>
      void Drawing_MouseOutGridEvent()
      {
         if (false == m_myDocument.WallCreated)
         {
            return;
         }

         m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
         this.Cursor = Cursors.Default;
      }

      /// <summary>
      /// the mouse moves into the curtain grid area, change the cursor to cross
      /// </summary>
      void Drawing_MouseInGridEvent()
      {
         if (false == m_myDocument.WallCreated)
         {
            return;
         }

         this.Cursor = Cursors.Cross;
      }

      /// <summary>
      /// the user wants to change to another tab page (for example, change from "Create Curtain Wall" to "Curtain Grid")
      /// if the curtain wall hasn't been created, don't allow to change to "Curtain Grid" tab page
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void mainTabControl_Selecting(object sender, TabControlCancelEventArgs e)
      {
         // if no curtain wall is created, limits the tab pages to the 1st tab page
         if (false == m_myDocument.WallCreated)
         {
            this.mainTabControl.SelectedIndex = 0;
         }
      }

      /// <summary>
      /// the user changes the tab page, update the data in the active tab page
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
      {
         // if the curtain wall hasn't been created, doesn't allow to change the tab page
         if (false == m_myDocument.WallCreated)
         {
            return;
         }

         // change to the curtain grid tab page, reload the properties of the curtain grid
         if (mainTabControl.SelectedTab == curtainGridTabPage)
         {
            UpdatePropertyGrid();
            this.operationStatusLabel.Text = "Operate the curtain grid by selecting an item from the bottom-left drop down list";
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
         }
         // change back to the curtain wall tab page, update the hint
         else
         {
            // stop the line operation and set it to "Waiting"
            m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
            ResetLineOpApprearances();

            // update the status hint
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Switch to \"Curtain Grid\" tab page to manipulate the curtain grid.";
         }
      }

      /// <summary>
      /// redraw the curtain grid canvas
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void curtainGridPictureBox_Paint(object sender, PaintEventArgs e)
      {
         m_myDocument.GridGeometry.Drawing.DrawCurtainGrid(e.Graphics);
      }

      /// <summary>
      /// the user specifies a grid operation / grid line operation
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void lineOperationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         m_myDocument.ActiveOperation = (LineOperation)lineOperationsComboBox.SelectedItem;
         // update the status hints for the different grid/grid line operations
         if (LineOperationType.AddULine == m_myDocument.ActiveOperation.OpType)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Specify a point within the curtain grid to locate new grid line";
         }
         else if (LineOperationType.AddVLine == m_myDocument.ActiveOperation.OpType)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Specify a point within the curtain grid to locate new grid line";
         }
         else if (LineOperationType.LockOrUnlockLine == m_myDocument.ActiveOperation.OpType)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Click on an existing grid line to switch its \"Lock\" status";
         }
         else if (LineOperationType.MoveLine == m_myDocument.ActiveOperation.OpType)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Select a grid line & move the mouse, left-click to finish the movement";
         }
         else if (LineOperationType.AddSegment == m_myDocument.ActiveOperation.OpType)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Add a segment to the place where a segment has been deleted previously";
         }
         else if (LineOperationType.RemoveSegment == m_myDocument.ActiveOperation.OpType)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Remove the segment of the grid line";

         }
         else if (LineOperationType.AddAllSegments == m_myDocument.ActiveOperation.OpType)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Add all the segments of the selected grid line";
         }
         else if (LineOperationType.AddAllMullions == m_myDocument.ActiveOperation.OpType)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Add mullions to all segments";
            m_myDocument.GridGeometry.AddAllMullions();
            ResetLineOpApprearances();
            UpdatePropertyGrid();
         }
         else if (LineOperationType.DeleteAllMullions == m_myDocument.ActiveOperation.OpType)
         {
            this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.operationStatusLabel.Text = "Delete all the mullions of the curtain grid";
            m_myDocument.GridGeometry.DeleteAllMullions();
            ResetLineOpApprearances();
            UpdatePropertyGrid();
         }
      }

      /// <summary>
      /// reset the curtain grid operation to "Waiting"
      /// </summary>
      private void ResetLineOpApprearances()
      {
         // the user doesn't hold Ctrl key down
         if (false == m_ctrlKeyDown)
         {
            m_myDocument.ActiveOperation = new LineOperation(LineOperationType.Waiting);
            this.Cursor = Cursors.Default;
         }
         // cancel the hanged operation : for example: the line to-be-moved has been selected
         // and press "Esc", cancel the selection
         m_lineToMoveSelected = false;

         // refresh the canvas area
         curtainGridPictureBox.Refresh();
      }

      /// <summary>
      /// reload the properties of the curtain grid
      /// </summary>
      private void UpdatePropertyGrid()
      {
         m_myDocument.GridGeometry.ReloadGridProperties();
         curtainGridPropertyGrid.SelectedObject = m_myDocument.GridGeometry.GridProperties;
      }

      /// <summary>
      /// the user clicks on the curtain grid canvas, executes the grid operation / grid line operation
      /// </summary>
      /// <param name="sender">object who sent this event</param>
      /// <param name="e">event args</param>
      private void curtainGridPictureBox_MouseClick(object sender, MouseEventArgs e)
      {
         // add a new U line to the canvas and to Revit
         if (LineOperationType.AddULine == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.AddUGridLine();
            ResetLineOpApprearances();
            UpdatePropertyGrid();
            m_lastPoint = e.Location;
         }
         // add a new V line to the canvas and to Revit
         else if (LineOperationType.AddVLine == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.AddVGridLine();
            ResetLineOpApprearances();
            UpdatePropertyGrid();
            m_lastPoint = e.Location;
         }
         // toggle the Lock status of the selected grid line
         else if (LineOperationType.LockOrUnlockLine == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.LockOrUnlockSelectedGridLine();
            ResetLineOpApprearances();
            UpdatePropertyGrid();
            //m_lastPoint = e.Location;
         }
         // move the selected grid line to a new place
         else if (LineOperationType.MoveLine == m_myDocument.ActiveOperation.OpType)
         {
            // during moving a grid line, the mouse will click twice
            // the 1st time is to select a grid line to move
            // the 2nd time is to determine the destination place for the selected grid line

            // the 1st time, select a grid line to move
            if (false == m_lineToMoveSelected)
            {
               // get the line which will be moved
               bool lineObtained = m_myDocument.GridGeometry.GetLineToBeMoved();
               if (true == lineObtained)
               {
                  m_lineToMoveSelected = true;
                  this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
                  this.operationStatusLabel.Text = "Move the mouse to the expected location, left click to finish the operation";
               }
            }
            // the 2nd time, the to-be-moved grid line specified, specify the destination location
            else
            {
               // the destination point has been set, move to that place
               bool succeeded = m_myDocument.GridGeometry.MoveGridLine(e.Location);
               if (true == succeeded)
               {
                  m_lineToMoveSelected = false;
                  ResetLineOpApprearances();
                  //UpdatePropertyGrid();
               }
            }
            m_lastPoint = e.Location;
         }
         // add a segment to the selected place
         else if (LineOperationType.AddSegment == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.AddSegment();
            ResetLineOpApprearances();
            UpdatePropertyGrid();
            m_lastPoint = e.Location;
         }
         // remove the selected segment
         else if (LineOperationType.RemoveSegment == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.RemoveSegment();
            ResetLineOpApprearances();
            UpdatePropertyGrid();
            m_lastPoint = e.Location;
         }
         // add all the segments of the selected grid line
         else if (LineOperationType.AddAllSegments == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.AddAllSegments();
            ResetLineOpApprearances();
            UpdatePropertyGrid();
            m_lastPoint = e.Location;
         }
      }

      /// <summary>
      /// move the mouse in the curtain grid canvas, select the grid line / segment
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void curtainGridPictureBox_MouseMove(object sender, MouseEventArgs e)
      {
         if (LineOperationType.Waiting == m_myDocument.ActiveOperation.OpType)
         {
            return;
         }

         // minimize the occurence of this method, if the move offset of the mouse is less than 1
         // don't regard the mouse has moved
         if (System.Drawing.Point.Empty != m_lastPoint)
         {
            System.Drawing.Point currentPoint = e.Location;
            if (Math.Abs(currentPoint.X - m_lastPoint.X) < 1 &&
                Math.Abs(currentPoint.Y - m_lastPoint.Y) < 1)
            {
               return;
            }
            else
            {
               m_lastPoint = System.Drawing.Point.Empty;
            }
         }

         if (LineOperationType.AddULine == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
            m_myDocument.GridGeometry.Drawing.AddDashULine(e.Location);
            this.curtainGridPictureBox.Refresh();
         }
         else if (LineOperationType.AddVLine == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
            m_myDocument.GridGeometry.Drawing.AddDashVLine(e.Location);
            this.curtainGridPictureBox.Refresh();
         }
         else if (LineOperationType.LockOrUnlockLine == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
            m_myDocument.GridGeometry.Drawing.SelectLine(e.Location, false, false);
            this.curtainGridPictureBox.Refresh();
         }
         else if (LineOperationType.MoveLine == m_myDocument.ActiveOperation.OpType)
         {
            // during moving a grid line, the mouse will click twice
            // the 1st time is to select a grid line to move
            // the 2nd time is to determine the destination place for the selected grid line
            if (false == m_lineToMoveSelected)
            {
               // get the line which will be moved
               m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
               m_myDocument.GridGeometry.Drawing.SelectLine(e.Location, true, false);
               this.curtainGridPictureBox.Refresh();
            }
            else
            {
               // determine the destination place for the selected grid line
               m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
               m_myDocument.GridGeometry.Drawing.AddDashLine(e.Location);
               this.curtainGridPictureBox.Refresh();
            }

         }
         else if (LineOperationType.AddSegment == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
            m_myDocument.GridGeometry.Drawing.SelectSegment(e.Location);
            this.curtainGridPictureBox.Refresh();
         }
         else if (LineOperationType.RemoveSegment == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
            m_myDocument.GridGeometry.Drawing.SelectSegment(e.Location);
            this.curtainGridPictureBox.Refresh();
         }
         else if (LineOperationType.AddAllSegments == m_myDocument.ActiveOperation.OpType)
         {
            m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
            m_myDocument.GridGeometry.Drawing.SelectLine(e.Location, false, true);
            this.curtainGridPictureBox.Refresh();
         }
      }

      /// <summary>
      /// when Ctrl key keeps down, don't set the operation to "default" after an operation finished
      /// for example, if the selected operation is "Add U grid line" and the Ctrl key keeps down
      /// after the user draws a U line, he can continue to draw another line, as long as the Ctrl is down
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void GridForm_KeyDown(object sender, KeyEventArgs e)
      {
         // "e.KeyValue == 17" return true only when "Ctrl" presses down
         if (e.KeyValue == 17)
         {
            m_ctrlKeyDown = true;
         }
      }

      /// <summary>
      /// reset the CtrlKeyDown flag to false, indicating that the Ctrl key is no longer down
      /// so the "continuous" operations will be stopped
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void GridForm_KeyUp(object sender, KeyEventArgs e)
      {
         // "e.KeyValue == 17" return true only when "Ctrl" presses down
         if (e.KeyValue == 17)
         {
            m_ctrlKeyDown = false;
         }
      }

      /// <summary>
      /// the user presses the "ESC" key, quit the current operation
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void mainTabControl_KeyPress(object sender, KeyPressEventArgs e)
      {
         // check whether the key is"ESC" key (for "ESC" key: e.KeyChar == 27)
         if (e.KeyChar != (char)27)
         {
            return;
         }
         if (mainTabControl.SelectedTab == curtainWallTabPage)
         {
            // context: press "ESC" during drawing the baseline for the curtain wall and not finished the drawing
            // (not both start point and end point for the curtain wall creation have been specified)
            // result: clear the baseline points(if have) and disable the "Create" button
            if (System.Drawing.Point.Empty == m_myDocument.WallGeometry.Drawing.WallLine2D.StartPoint ||
                System.Drawing.Point.Empty == m_myDocument.WallGeometry.Drawing.WallLine2D.EndPoint)
            {
               clearButton_Click(null, null);
            }
         }
         else
         {
            // context: press "ESC" during operating the curtain grid
            // result: stop the current operation and set the operation to "Waiting"
            m_myDocument.GridGeometry.Drawing.DrawObject.Clear();
            ResetLineOpApprearances();
         }
      }

      /// <summary>
      /// the user clicks the "Exit" button, quit the dialog
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void exitButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      /// <summary>
      /// the user clicks the "Exit" button, quit the dialog
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void gridExitButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }
      #endregion
   }// end of class

   /// <summary>
   /// lists all the supported line operations in this sample
   /// </summary>
   public enum LineOperationType
   {
      Waiting,
      AddULine,
      AddVLine,
      LockOrUnlockLine,
      MoveLine,
      AddSegment,
      RemoveSegment,
      AddAllSegments,
      AddAllMullions,
      DeleteAllMullions
   }

   /// <summary>
   /// stores the specified line operation
   /// </summary>
   public struct LineOperation
   {
      #region Fields
      // the current line operation type
      private LineOperationType m_opType;
      #endregion

      #region Properties
      /// <summary>
      /// the current line operation type
      /// </summary>
      public LineOperationType OpType
      {
         get
         {
            return m_opType;
         }
      }
      #endregion

      #region Constructors
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="type">
      /// the current line operation type
      /// </param>
      public LineOperation(LineOperationType type)
      {
         this.m_opType = type;
      }
      #endregion

      #region Public methods
      /// <summary>
      /// convert to string
      /// </summary>
      /// <returns>
      /// the result string
      /// </returns>
      public override string ToString()
      {
         string resultString = string.Empty;
         switch (m_opType)
         {
            case LineOperationType.AddULine:
               resultString = "Add horizontal grid line";
               break;
            case LineOperationType.AddVLine:
               resultString = "Add vertical grid line";
               break;
            case LineOperationType.LockOrUnlockLine:
               resultString = "Lock or unlock grid line";
               break;
            case LineOperationType.MoveLine:
               resultString = "Move grid line";
               break;
            case LineOperationType.AddSegment:
               resultString = "Add segment";
               break;
            case LineOperationType.RemoveSegment:
               resultString = "Delete segment";
               break;
            case LineOperationType.AddAllSegments:
               resultString = "Add segments of entire grid line";
               break;
            case LineOperationType.AddAllMullions:
               resultString = "Add mullions to all segments";
               break;
            case LineOperationType.DeleteAllMullions:
               resultString = "Delete all mullions";
               break;
            default:
               resultString = base.ToString();
               break;
         }

         return resultString;
      }
      #endregion
   }

}
