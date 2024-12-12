//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// This is the main form for user to operate the Rebar creation.
    /// </summary>
    public partial class NewRebarForm : System.Windows.Forms.Form
    {
        ///// <summary>
        ///// Revit Application object.
        ///// </summary>
        //Autodesk.Revit.ApplicationServices.Application m_rvtApp;

        /// <summary>
        /// Revit Document object.
        /// </summary>
        Document m_rvtDoc;

        /// <summary>
        /// All RebarBarTypes of Revit current document.
        /// </summary>
        List<RebarBarType> m_rebarBarTypes = new List<RebarBarType>();

        /// <summary>
        /// All RebarShape of Revit current document.
        /// </summary>
        List<RebarShape> m_rebarShapes = new List<RebarShape>();

        /// <summary>
        /// Control binding source, provides data source for RebarBarType list box.
        /// </summary>
        BindingSource m_barTypesBinding = new BindingSource();

        /// <summary>
        /// Control binding source, provides data source for RebarShapes list box. 
        /// </summary>
        BindingSource m_shapesBinding = new BindingSource();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NewRebarForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor, initialize fields of RebarBarTypes and RebarShapes.
        /// </summary>
        /// <param name="rvtApp"></param>
        public NewRebarForm(Autodesk.Revit.DB.Document rvtDoc)
            : this()
        {
            m_rvtDoc = rvtDoc;

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(m_rvtDoc);
            filteredElementCollector.OfClass(typeof(RebarBarType));
            m_rebarBarTypes = filteredElementCollector.Cast<RebarBarType>().ToList<RebarBarType>();

            filteredElementCollector = new FilteredElementCollector(m_rvtDoc);
            filteredElementCollector.OfClass(typeof(RebarShape));
            m_rebarShapes = filteredElementCollector.Cast<RebarShape>().ToList<RebarShape>();
        }

        /// <summary>
        /// Return RebarBarType from selection of barTypesComboBox.
        /// </summary>
        public RebarBarType RebarBarType
        {
            get
            {
                return barTypesComboBox.SelectedItem as RebarBarType;
            }
        }

        /// <summary>
        /// Return RebarShape from selection of shapesComboBox.
        /// </summary>
        public RebarShape RebarShape
        {
            get
            {
                return shapesComboBox.SelectedItem as RebarShape;
            }
        }


        /// <summary>
        /// OK Button, return DialogResult.OK and close this form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Cancel Button, return DialogResult.Cancel and close this form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Present a dialog to customize a RebarShape.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createShapeButton_Click(object sender, EventArgs e)
        {
            // Make sure the name is not null or empty.
            if (string.IsNullOrEmpty(nameTextBox.Text.Trim()))
            {
                TaskDialog.Show("Revit", "Please give a name to create a rebar shape.");
                return;
            }

            // Make sure the input name is started with letter and 
            // just contains letters, numbers and underlines.
            Regex regex = new Regex("^[a-zA-Z]\\w+$");
            if (!regex.IsMatch(nameTextBox.Text.Trim()))
            {
                TaskDialog.Show("Revit", "Please input the name starting with letter and just containing letters, numbers and underlines. String is " + nameTextBox.Text.ToString());
                nameTextBox.Focus();
                return;
            }

            // Create a RebarShapeDefinition.
            RebarShapeDef shapeDef = null;

            if (byArcradioButton.Checked)
            {
                // Create arc shape.
                RebarShapeDefinitionByArc arcShapeDefinition = null;
                RebarShapeDefinitionByArcType arcType = (RebarShapeDefinitionByArcType)Enum.Parse(typeof(RebarShapeDefinitionByArcType), arcTypecomboBox.Text);
                if (arcType != RebarShapeDefinitionByArcType.Spiral)
                {
                    arcShapeDefinition = new RebarShapeDefinitionByArc(m_rvtDoc, arcType);
                }
                else
                {
                    // Set default value for Spiral-Shape definition.
                    arcShapeDefinition = new RebarShapeDefinitionByArc(m_rvtDoc, 10.0, 3.0, 0, 0);
                }
                shapeDef = new RebarShapeDefByArc(arcShapeDefinition);
            }
            else if (bySegmentsradioButton.Checked)
            {
                // Create straight segments shape.
                int segmentCount = 0;
                if (int.TryParse(segmentCountTextBox.Text, out segmentCount) && segmentCount > 0)
                {
                    shapeDef = new RebarShapeDefBySegment(new RebarShapeDefinitionBySegments(m_rvtDoc, segmentCount));
                }
                else
                {
                    TaskDialog.Show("Revit", "Please input a valid positive integer as segments count.");
                    return;
                }
            }

            int startHookAngle = 0;
            int endHookAngle = 0;
            RebarHookOrientation startHookOrientation = RebarHookOrientation.Left;
            RebarHookOrientation endHookOrientation = RebarHookOrientation.Left;

            bool doCreate = false;

            using (NewRebarShapeForm form = new NewRebarShapeForm(m_rvtDoc, shapeDef))
            {
                // Present a form to customize the shape.
                if (DialogResult.OK == form.ShowDialog())
                {
                    doCreate = true;
                    if (form.NeedSetHooks)
                    {
                        // Set hooks for rebar shape.
                        startHookAngle = form.StartHookAngle;
                        endHookAngle = form.EndHookAngle;
                        startHookOrientation = form.StartHookOrientation;
                        endHookOrientation = form.EndHookOrientation;
                    }
                }
            }

            if (doCreate)
            {
                // Create the RebarShape.
                RebarShape createdRebarShape = RebarShape.Create(m_rvtDoc, shapeDef.RebarshapeDefinition, null,
                    RebarStyle.Standard, StirrupTieAttachmentType.InteriorFace,
                    startHookAngle, startHookOrientation,
                    endHookAngle, endHookOrientation,
                    0);
                createdRebarShape.Name = nameTextBox.Text.Trim();

                // Add the created shape to the candidate list.
                m_rebarShapes.Add(createdRebarShape);
                m_shapesBinding.ResetBindings(false);
                shapesComboBox.SelectedItem = createdRebarShape;
            }

        }

        /// <summary>
        /// Update the status of some controls.
        /// </summary>
        private void UpdateUIStatus()
        {
            segmentCountTextBox.Enabled = bySegmentsradioButton.Checked;
            arcTypecomboBox.Enabled = byArcradioButton.Checked;
        }

        /// <summary>
        /// byArcradioButton check status change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void byArcradioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIStatus();
        }

        /// <summary>
        /// bySegmentsradioButton check status change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bySegmentsradioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIStatus();
        }

        /// <summary>
        /// Load event, Initialize controls data source.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRebarForm_Load(object sender, EventArgs e)
        {
            m_barTypesBinding.DataSource = m_rebarBarTypes;
            m_shapesBinding.DataSource = m_rebarShapes;

            arcTypecomboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            arcTypecomboBox.DataSource = Enum.GetNames(typeof(RebarShapeDefinitionByArcType));

            shapesComboBox.Sorted = true;
            barTypesComboBox.Sorted = true;
            shapesComboBox.DataSource = m_shapesBinding;
            shapesComboBox.DisplayMember = "Name";
            barTypesComboBox.DataSource = m_barTypesBinding;
            barTypesComboBox.DisplayMember = "Name";
        }
    }
}