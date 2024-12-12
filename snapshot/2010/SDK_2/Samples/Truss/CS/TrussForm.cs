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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit;
using System.Collections;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Elements;
using System.Drawing.Drawing2D;
using System.Threading;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Structural;

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
        ArrayList m_beamTypes; //stores all the beam types (FamilySymbol)
        ArrayList m_views; //stores all the ViewPlan use to create truss
        TrussGeometry trussGeometry; //TrussGeometry object store geometry info of Truss
        TrussType m_selectedTrussType; //selected truss type
        Autodesk.Revit.Elements.Truss m_truss; //store the truss created by this sample
        bool m_topChord = false; //allowes to draw top chord when it's true, otherwise forbids to do it.
        bool m_bottomChord = false; //draw bottom chord when it's true, otherwise forbids to do it.
        int m_selectMemberIndex; //index of selected truss member
        Autodesk.Revit.Elements.ViewPlan m_selectedView; //store the selected view
        FamilyInstance m_selecedtBeam; //store the selected beam
        FamilySymbol m_selectedBeamType; //store the selected beam type
        Document m_activeDocument; //active document in Revit
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
            m_activeDocument = m_commandData.Application.ActiveDocument;
            InitializeComponent();
            m_views = new ArrayList();
            m_trussTypes = new ArrayList();
            m_beamTypes = new ArrayList();
            //Get user selection
            if (!GetSelectTrussOrColumns())
            {
                MessageBox.Show("Please select 1 existing truss or 2 columns before load this application.");
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
            if (m_activeDocument.Selection.Elements.Size > 2 || 
                0 == m_activeDocument.Selection.Elements.Size)
            { return false; }

            IEnumerator iter = m_activeDocument.Selection.Elements.GetEnumerator();
            iter.Reset();
            while (iter.MoveNext())
            {
                if (iter.Current is Autodesk.Revit.Elements.Truss)
                {
                    if (null == m_truss)
                    { m_truss = iter.Current as Autodesk.Revit.Elements.Truss; }
                    else { return false; }
                }
                else if (iter.Current is Autodesk.Revit.Elements.FamilyInstance)
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
            //because GetBeamTypes() takes a long time, so call it in a new thread
            Thread newThread = new Thread(new ThreadStart(GetBeamTypes));
            newThread.Start();

            // get all the truss types
            // there's no truss type in the active document
            if (null == m_activeDocument.TrussTypes ||
                0 == m_activeDocument.TrussTypes.Size)
            {
                MessageBox.Show("Please load at least one truss type into your project.");
                this.Close();
            }

            foreach (TrussType trussType in m_activeDocument.TrussTypes)
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
            ElementIterator filterIterator = m_activeDocument.get_Elements(typeof(ViewPlan));
            filterIterator.Reset();
            while (filterIterator.MoveNext())
            {
                Autodesk.Revit.Elements.View view = filterIterator.Current as Autodesk.Revit.Elements.View;
                this.ViewComboBox.Items.Add(view.Name);
                m_views.Add(view);
            }
        }

        /// <summary>
        /// get all the beam types
        /// </summary>
        private void GetBeamTypes()
        {
            ElementIterator filterIter = m_activeDocument.get_Elements(typeof(FamilySymbol));
            filterIter.Reset();

            while (filterIter.MoveNext())
            {
                FamilySymbol familySymbol = filterIter.Current as FamilySymbol;
                if (null == familySymbol)
                {
                    continue;
                }

                Category category = familySymbol.Category;
                if (category != null)
                {
                    if (0 == String.Compare(category.Name, "Structural Framing"))
                    {
                        String beamTypeName = familySymbol.get_Parameter(
                            BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM).AsString();
                        this.BeamTypeComboBox.Items.Add(beamTypeName);
                        m_beamTypes.Add(familySymbol);
                    }
                }
            }

            // can't obtain beam types from the active document
            if (null == m_beamTypes ||
                0 == m_beamTypes.Count)
            {
                MessageBox.Show("No Structural Framing Family loaded. Please load one of the Structure Framing Family.");
                this.Close();
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
                m_commandData.Application.ActiveDocument.BeginTransaction();
                // create the truss
                m_truss = CreateTruss();
                m_truss.Location.Move(new XYZ(0, 0, m_selectedView.GenLevel.Elevation));
                m_commandData.Application.ActiveDocument.EndTransaction();
                trussGeometry = new TrussGeometry(m_truss, m_commandData);
                this.TrussGraphicsTabControl.SelectedIndex = 1;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

            this.TrussTypeComboBox.Enabled = false;
            this.CreateButton.Enabled = false;
            this.ViewComboBox.Enabled = false;
        }

        /// <summary>
        /// create truss in Revit
        /// </summary>
        /// <returns>new created truss</returns>
        public Autodesk.Revit.Elements.Truss CreateTruss()
        {
            Autodesk.Revit.Creation.Document createDoc = m_commandData.Application.ActiveDocument.Create;
            Autodesk.Revit.Creation.Application createApp = m_commandData.Application.Create;
            //sketchPlane
            XYZ origin = new XYZ(0, 0, 0);
            XYZ xDirection = new XYZ(1, 0, 0);
            XYZ yDirection = new XYZ(0, 1, 0);
            Plane plane = createApp.NewPlane(xDirection, yDirection, origin);
            SketchPlane sketchPlane = createDoc.NewSketchPlane(plane);
            //new base Line
            AnalyticalModelFrame frame1 = column1.AnalyticalModel as AnalyticalModelFrame;
            XYZ centerPoint1 = (frame1.Curve as Line).get_EndPoint(0);
            AnalyticalModelFrame frame2 = column2.AnalyticalModel as AnalyticalModelFrame;
            XYZ centerPoint2 = (frame2.Curve as Line).get_EndPoint(0);
            XYZ startPoint = new XYZ(centerPoint1.X, centerPoint1.Y, 0);
            XYZ endPoint = new XYZ(centerPoint2.X, centerPoint2.Y, 0);
            Autodesk.Revit.Geometry.Line baseLine = null;

            try
            { baseLine = createApp.NewLineBound(startPoint, endPoint); }
            catch (System.ArgumentException)
            { MessageBox.Show("Two column you selected are too close to create truss."); }

            return createDoc.NewTruss(m_selectedTrussType, sketchPlane, baseLine, m_selectedView);
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
        /// change move point of top chord lineTool and bottom chod lineTool
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
            m_activeDocument.BeginTransaction();
            //update the truss
            trussGeometry.SetProfile(m_commandData);
            m_activeDocument.EndTransaction();
            this.ProfileEditPictureBox.Refresh();
        }

        /// <summary>
        /// restore profile of truss
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void RestoreButton_Click(object sender, EventArgs e)
        {
            m_activeDocument.BeginTransaction();
            // restore the profile
            trussGeometry.RemoveProfile();
            m_activeDocument.EndTransaction();
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
            m_selectedBeamType = (FamilySymbol)m_beamTypes[
            this.BeamTypeComboBox.SelectedIndex];
        }

        /// <summary>
        /// change type of selected beam
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void ChangeBeamTypeButton_Click(object sender, EventArgs e)
        {
            // apply the beam type change
            m_activeDocument.BeginTransaction();
            if (null != m_selecedtBeam)
            { m_selecedtBeam.Symbol = m_selectedBeamType; }
            m_activeDocument.EndTransaction();
        }

        /// <summary>
        /// change selected ViewPlan
        /// </summary>
        /// <param name="sender">object who sent this event</param>
        /// <param name="e">event args</param>
        private void ViewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // choose another view for the truss creation
            if (this.ViewComboBox.SelectedIndex < m_views.Count)
            { m_selectedView = (ViewPlan)m_views[this.ViewComboBox.SelectedIndex]; }
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