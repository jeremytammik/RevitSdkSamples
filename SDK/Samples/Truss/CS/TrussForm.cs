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
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit;
using System.Collections;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using System.Drawing.Drawing2D;
using System.Threading;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.Truss.CS
{
    /// <summary>
    /// window form contains one three picture box to show the 
    /// profile of truss geometry and profile and tabControl.
    /// User can create truss, edit profile of truss and change type of truss members.
    /// </summary>
    public partial class TrussForm : System.Windows.Forms.Form
    {
        ExternalCommandData m_commandData; //object which contains reference of Revit Application
        ArrayList m_trussTypes; //stores all the truss types
        IEnumerable<Autodesk.Revit.DB.FamilySymbol> m_beamTypes; //stores all the beam types (FamilySymbol)
        IEnumerable<Autodesk.Revit.DB.ViewPlan> m_views; //stores all the ViewPlan use to create truss
        TrussGeometry trussGeometry; //TrussGeometry object store geometry info of Truss
        TrussType m_selectedTrussType; //selected truss type
        Autodesk.Revit.DB.Structure.Truss m_truss; //store the truss created by this sample
        bool m_topChord = false; //allows to draw top chord when it's true, otherwise forbids to do it.
        bool m_bottomChord = false; //draw bottom chord when it's true, otherwise forbids to do it.
        int m_selectMemberIndex; //index of selected truss member
        Autodesk.Revit.DB.ViewPlan m_selectedView; //store the selected view
        FamilyInstance m_selecedtBeam; //store the selected beam
        FamilySymbol m_selectedBeamType; //store the selected beam type
        UIDocument m_activeDocument; //active document in Revit
        FamilyInstance column1; //one of 2 columns which truss build on
        FamilyInstance column2; //one of 2 columns which truss build on
        const String NoAssociatedLevel = "<not associated>"; //when select truss doesn't have associate level

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="commandData">object which contains reference of Revit Application</param>
        public TrussForm(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_activeDocument = m_commandData.Application.ActiveUIDocument;
            InitializeComponent();
            m_trussTypes = new ArrayList();
            //Get user selection
            if (!GetSelectTrussOrColumns())
            {
                TaskDialog.Show("Select", "Please select 1 existing truss or 2 columns before load this application.");
                this.Close();
            }
            // get all the beam types, truss types and all the view plans from the active document
            DataInitialize();
        }

        /// <summary>
        /// Get 1 truss or 2 columns from selection
        /// </summary>
        /// <returns>return false if selection incorrect</returns>
        private bool GetSelectTrussOrColumns()
        {
            if (m_activeDocument.Selection.GetElementIds().Count > 2 ||
                0 == m_activeDocument.Selection.GetElementIds().Count)
            { return false; }

            ElementSet es = new ElementSet();
            foreach (ElementId elementId in m_activeDocument.Selection.GetElementIds())
            {
               es.Insert(m_activeDocument.Document.GetElement(elementId));
            }
            IEnumerator iter = es.GetEnumerator();
            iter.Reset();
            while (iter.MoveNext())
            {
                if (iter.Current is Autodesk.Revit.DB.Structure.Truss)
                {
                    if (null == m_truss)
                    { m_truss = iter.Current as Autodesk.Revit.DB.Structure.Truss; }
                    else { return false; }
                }
                else if (iter.Current is Autodesk.Revit.DB.FamilyInstance)
                {
                    FamilyInstance familyInstance = iter.Current as FamilyInstance;
                    if (StructuralType.Column == familyInstance.StructuralType)
                    {
                        if (null == column1) { column1 = familyInstance; }
                        else { column2 = familyInstance; }
                    }
                    else
                    { return false; }
                }
                else { return false; }
            }
            if (null == m_truss && (null == column1 || null == column2))
            { return false; }
            return true;
        }

        /// <summary>
        /// get all the beam types, truss types and all the view plans from the active document
        /// </summary>
        public void DataInitialize()
        {
            // get all the beam types
            GetBeamTypes();

            // get all the truss types
            // there's no truss type in the active document
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(m_activeDocument.Document);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_Truss);
            IList<TrussType> trussTypes = filteredElementCollector.Cast<TrussType>().ToList<TrussType>();

            if (null == trussTypes || 0 == trussTypes.Count)
            {
                TaskDialog.Show("Load Truss Type", "Please load at least one truss type into your project.");
                this.Close();
            }

            foreach (TrussType trussType in trussTypes)
            {
                if (null == trussType)
                {
                    continue;
                }

                String trussTypeName = trussType.get_Parameter
                        (BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM).AsString();
                this.TrussTypeComboBox.Items.Add(trussTypeName);
                m_trussTypes.Add(trussType);
            }

            // get all the views
            // Skip view templates because they're behind-the-scene and invisible in project browser; also invalid for API..., etc.

            m_views = from elem in
                          new FilteredElementCollector(m_activeDocument.Document).OfClass(typeof(ViewPlan)).ToElements()
                      let viewPlan = elem as ViewPlan
                      where viewPlan != null && !viewPlan.IsTemplate
                      select viewPlan;
            foreach (Autodesk.Revit.DB.View view in m_views)
            {
                this.ViewComboBox.Items.Add(view.Name);
            }

        }

        /// <summary>
        /// get all the beam types
        /// </summary>
        private void GetBeamTypes()
        {
            m_beamTypes = from elem in
                              new FilteredElementCollector(m_activeDocument.Document).OfClass(
                              typeof(FamilySymbol)).OfCategory(Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFraming)
                          let type = elem as FamilySymbol
                          select type;

            // can't obtain beam types from the active document
            if (null == m_beamTypes ||
                0 == m_beamTypes.Count())
            {
                TaskDialog.Show("Load Structural Framing Family", "No Structural Framing Family loaded. Please load one of the Structure Framing Family.");
                this.Close();
            }

            foreach (FamilySymbol familySymbol in m_beamTypes)
            {
                String beamTypeName = familySymbol.get_Parameter(
                             BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM).AsString();
                this.BeamTypeComboBox.Items.Add(beamTypeName);
            }
        }

        /// <summary>
        /// initialize the UI ComboBox's appearance
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void TrussForm_Load(object sender, EventArgs e)
        {
            this.TrussTypeComboBox.SelectedIndex = 0;
            this.ViewComboBox.SelectedIndex = 0;
            //user pre-select a truss in Revit UI
            if (m_truss != null)
            {
                // show the truss type and level of pre-selected truss in the ComboBox
                String nameOfTrussType = m_truss.TrussType.get_Parameter
                        (BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM).AsString();
                this.TrussTypeComboBox.SelectedIndex = this.TrussTypeComboBox.Items.IndexOf(nameOfTrussType);
                Parameter viewName = m_truss.get_Parameter(BuiltInParameter.SKETCH_PLANE_PARAM);
                if (null == viewName || 0 == viewName.AsString().CompareTo(NoAssociatedLevel))
                {
                    this.ViewComboBox.Items.Add(NoAssociatedLevel);
                    this.ViewComboBox.SelectedIndex = this.ViewComboBox.Items.IndexOf(NoAssociatedLevel);
                }
                else
                {
                    String nameOfViewPlane = viewName.AsString().Substring(8);
                    this.ViewComboBox.SelectedIndex = this.ViewComboBox.Items.IndexOf(nameOfViewPlane);
                }

                this.TrussTypeComboBox.Enabled = false;
                this.CreateButton.Enabled = false;
                this.ViewComboBox.Enabled = false;
                trussGeometry = new TrussGeometry(m_truss, m_commandData);
                this.TrussGraphicsTabControl.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// get selected truss type, this data will be used in truss creation
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void TrussTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_selectedTrussType = (TrussType)m_trussTypes[this.TrussTypeComboBox.SelectedIndex];
        }

        /// <summary>
        /// create new truss
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void CreateButton_Click(object sender, EventArgs e)
        {
            try
            {
                Transaction transaction = new Transaction(m_commandData.Application.ActiveUIDocument.Document, "CreateTruss");
                transaction.Start();
                // create the truss
                m_truss = CreateTruss();
                m_truss.Location.Move(new Autodesk.Revit.DB.XYZ(0, 0, m_selectedView.GenLevel.Elevation));
                transaction.Commit();
                trussGeometry = new TrussGeometry(m_truss, m_commandData);
                this.TrussGraphicsTabControl.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }

            this.TrussTypeComboBox.Enabled = false;
            this.CreateButton.Enabled = false;
            this.ViewComboBox.Enabled = false;
        }

      /// <summary>
      /// create truss in Revit
      /// </summary>
      /// <returns>new created truss</returns>
      public Autodesk.Revit.DB.Structure.Truss CreateTruss()
      {
         Autodesk.Revit.DB.Document document = m_commandData.Application.ActiveUIDocument.Document;
         Autodesk.Revit.Creation.Document createDoc = document.Create;
         Autodesk.Revit.Creation.Application createApp = m_commandData.Application.Application.Create;
         //sketchPlane
         Autodesk.Revit.DB.XYZ origin = new Autodesk.Revit.DB.XYZ(0, 0, 0);
         Autodesk.Revit.DB.XYZ xDirection = new Autodesk.Revit.DB.XYZ(1, 0, 0);
         Autodesk.Revit.DB.XYZ yDirection = new Autodesk.Revit.DB.XYZ(0, 1, 0);
         Plane plane = Plane.CreateByOriginAndBasis(xDirection, yDirection, origin);
         SketchPlane sketchPlane = SketchPlane.Create(document, plane);
         //new base Line
         Curve frame1Curve = null;
         Curve frame2Curve = null;
         if (column1.Location is LocationCurve)
         {
            frame1Curve = (column1.Location as LocationCurve).Curve;
         }
         if (column2.Location is LocationCurve)
         {
            frame2Curve = (column2.Location as LocationCurve).Curve;
         }
         
         Autodesk.Revit.DB.XYZ centerPoint1 = (frame1Curve as Line).GetEndPoint(0);
         

         Autodesk.Revit.DB.XYZ centerPoint2 = (frame2Curve as Line).GetEndPoint(0);
         Autodesk.Revit.DB.XYZ startPoint = new Autodesk.Revit.DB.XYZ(centerPoint1.X, centerPoint1.Y, 0);
         Autodesk.Revit.DB.XYZ endPoint = new Autodesk.Revit.DB.XYZ(centerPoint2.X, centerPoint2.Y, 0);
         Autodesk.Revit.DB.Line baseLine = null;

         try
         { baseLine = Line.CreateBound(startPoint, endPoint); }
         catch (System.ArgumentException)
         {
            TaskDialog.Show("Argument Exception", "Two column you selected are too close to create truss.");
         }

         return Autodesk.Revit.DB.Structure.Truss.Create(document, m_selectedTrussType.Id, sketchPlane.Id, baseLine);
      }

      /// <summary>
      /// draw profile, top chord and bottom of truss
      /// </summary>
      /// <param name="sender">object who sent this event</param>
      /// <param name="e">event args</param>
      private void ProfileEditPictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            if (trussGeometry != null)
            { trussGeometry.Draw2D(e.Graphics, Pens.Blue); }

            Font font = new Font("Verdana", 10, FontStyle.Regular);
            string indicator = "Draw Top Chord and Bottom Chord here:";
            e.Graphics.DrawString(indicator, font, Brushes.Blue, new PointF(20, 10));
        }

        /// <summary>
        /// add point to top chord and bottom chord
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void ProfileEditPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            // draw top chord line
            if (m_topChord)
            { trussGeometry.AddTopChordPoint(e.X, e.Y); }
            // draw bottom chord line
            else if (m_bottomChord)
            { trussGeometry.AddBottomChordPoint(e.X, e.Y); }

            this.ProfileEditPictureBox.Refresh();
        }

        /// <summary>
        /// change move point of top chord lineTool and bottom chord lineTool
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void ProfileEditPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_topChord)
            { trussGeometry.AddTopChordMovePoint(e.X, e.Y); }
            else if (m_bottomChord)
            { trussGeometry.AddBottomChordMovePoint(e.X, e.Y); }

            this.ProfileEditPictureBox.Refresh();
        }

        /// <summary>
        /// begin to draw top chord
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void TopChordButton_Click(object sender, EventArgs e)
        {
            m_topChord = true;
            m_bottomChord = false;
            this.ProfileEditPictureBox.Cursor = Cursors.Cross;
            this.ProfileEditPictureBox.Refresh();
        }

        /// <summary>
        /// begin to draw bottom chord
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void BottomChordButton_Click(object sender, EventArgs e)
        {
            m_topChord = false;
            m_bottomChord = true;
            this.ProfileEditPictureBox.Cursor = Cursors.Cross;
            this.ProfileEditPictureBox.Refresh();
        }

        /// <summary>
        /// update the truss according to the top chord line and the bottom chord line
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            Transaction transaction = new Transaction(m_activeDocument.Document, "SetProfile");
            transaction.Start();
            //update the truss
            trussGeometry.SetProfile(m_commandData);
            transaction.Commit();
            this.ProfileEditPictureBox.Refresh();
        }

        /// <summary>
        /// restore profile of truss
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void RestoreButton_Click(object sender, EventArgs e)
        {
            Transaction transaction = new Transaction(m_activeDocument.Document, "RemoveProfile");
            transaction.Start();
            // restore the profile
            trussGeometry.RemoveProfile();
            transaction.Commit();
            this.ProfileEditPictureBox.Refresh();
        }

        /// <summary>
        /// clear points of top and bottom chord line
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void CleanChordbutton_Click(object sender, EventArgs e)
        {
            trussGeometry.ClearChords();
            m_topChord = false;
            m_bottomChord = false;
            this.ProfileEditPictureBox.Cursor = Cursors.Default;
            this.ProfileEditPictureBox.Refresh();
        }

        /// <summary>
        /// draw geometry of truss, and draw selected line red
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void TrussGeometryPictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            if (trussGeometry != null)
            { trussGeometry.Draw2D(e.Graphics, Pens.Blue); }

            Font font = new Font("Verdana", 10, FontStyle.Regular);
            string indicator = "Select a beam from the truss:";
            e.Graphics.DrawString(indicator, font, Brushes.Blue, new PointF(20, 10));
        }

        /// <summary>
        /// get selected truss member (Beam)
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void TrussGeometryPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            // indicates whether the mouse moves over or hovers on one beam
            // if true, paints the beam to red
            m_selectMemberIndex = trussGeometry.SelectTrussMember(e.X, e.Y);
            this.TrussMembersPictureBox.Refresh();
        }

        /// <summary>
        /// select truss member (beam)
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void TrussGeometryPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            // clicks in the canvas but doesn't select anything
            if (-1 == m_selectMemberIndex)
            {
                this.BeamTypeComboBox.Enabled = false;
                this.ChangeBeamTypeButton.Enabled = false;
                return;
            }

            // clicks in the canvas and selects a beam
            m_selecedtBeam = trussGeometry.GetSelectedBeam(m_commandData);
            if (null != m_selecedtBeam)
            {
                FamilySymbol symbol = m_selecedtBeam.Symbol;
                // get the type of the selected beam
                String nameOfSymbol = symbol.get_Parameter(
                    BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM).AsString();
                int index = this.BeamTypeComboBox.Items.IndexOf(nameOfSymbol);
                // show the beam type in the ComboBox
                this.BeamTypeComboBox.SelectedIndex = index;
                this.BeamTypeComboBox.Enabled = true;
                this.ChangeBeamTypeButton.Enabled = true;
                this.TrussMembersPictureBox.Refresh();
            }
        }

        /// <summary>
        /// change selected beam type
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void BeamTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // choose a new beam type for the selected beam
            m_selectedBeamType = m_beamTypes.ElementAt(this.BeamTypeComboBox.SelectedIndex);
        }

        /// <summary>
        /// change type of selected beam
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void ChangeBeamTypeButton_Click(object sender, EventArgs e)
        {
            // apply the beam type change
            Transaction transaction = new Transaction(m_activeDocument.Document, "ChangeSelectedBeamType");
            transaction.Start();
            if (null != m_selecedtBeam)
            { m_selecedtBeam.Symbol = m_selectedBeamType; }
            transaction.Commit();
        }

        /// <summary>
        /// change selected ViewPlan
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void ViewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // choose another view for the truss creation
            if (this.ViewComboBox.SelectedIndex < m_views.Count())
            { m_selectedView = m_views.ElementAt(this.ViewComboBox.SelectedIndex); }
        }

        /// <summary>
        /// quit the "top chord" or "bottom chord" drawing operation
        /// by pressing the "ESC" key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrussGraphicsTabControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            // the "Profile Edit" tab page is active 
            // and the user presses the "ESC" key (e.KeyChar == 27)
            if (TrussGraphicsTabControl.SelectedTab == ProfileEditTabPage &&
                e.KeyChar == (char)27)
            {
                // quit the "chord" drawing and remove the move indicating line
                m_topChord = false;
                m_bottomChord = false;
                this.ProfileEditPictureBox.Cursor = Cursors.Default;
                trussGeometry.ClearMovePoint();
                this.ProfileEditPictureBox.Refresh();
            }
        }

        /// <summary>
        /// won't let open truss member tab until truss create successfully
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrussGraphicsTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // if no truss is created, locks the tab pages to the 1st tab page
            if (null == trussGeometry)
            {
                this.TrussGraphicsTabControl.SelectedIndex = 0;
            }
            else
            {
                trussGeometry.Reset();
                m_selecedtBeam = null;
            }
        }

        /// <summary>
        /// Close dialogue box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
