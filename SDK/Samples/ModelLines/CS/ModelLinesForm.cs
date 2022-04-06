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
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ModelLines.CS
{
    public partial class ModelLinesForm : System.Windows.Forms.Form
    {
        #region Enum Define

        /// <summary>
        /// Define the model line types in revit
        /// </summary>
        public enum LineType
        {
            ModelLine = 0,          // ModelLine
            ModelArc = 1,           // ModelArc
            ModelEllipse = 2,       // ModelEllipse
            ModelHermiteSpline = 3, // ModelHermiteSpline
            ModelNurbSpline = 4     // ModelNurbSpline
        };

        #endregion

        // Private members
        ModelLines m_dataBuffer;   // A reference of ModelLines.

        #region Constructor

        /// <summary>
        /// Constructor of ModelLinesForm
        /// </summary>
        /// <param name="dataBuffer">A reference of ModelLines class</param>
        public ModelLinesForm(ModelLines dataBuffer)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            //Get a reference of ModelLines
            m_dataBuffer = dataBuffer;

            // Initialize the information data grid view control
            InitializeInformationGrid();

            // Initialize the sketch plane comboBox control
            BindComboBox(sketchPlaneComboBox, m_dataBuffer.SketchPlaneIDArray);
            sketchPlaneComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            // Initialize the creation group information
            lineRadioButton.Checked = true;
        }

        #endregion


        #region helper Fuctions

        /// <summary>
        /// Bind the DataSource of the information DataGridView
        /// </summary>
        void InitializeInformationGrid()
        {
            // In order to change column name, disable AutoGenerateColumns property
            informationDataGridView.AutoGenerateColumns = false;
            // Bind the DataSource to corresponding property.
            informationDataGridView.DataSource = m_dataBuffer.InformationMap;
            typeColumn.DataPropertyName = "TypeName";   // set data property name
            typeColumn.Width = informationDataGridView.Width * 3 / 5;   // set column width

            numberColumn.DataPropertyName = "Number";   // set data property name
            numberColumn.Width = informationDataGridView.Width * 2 / 5 - 2; // set column width
        }

        /// <summary>
        /// Check the data which the user input are integrated or not 
        /// </summary>
        /// <param name="createType"></param>
        /// <returns>If the data are integrated return true, otherwise false</returns>
        bool AssertDataIntegrity(LineType createType)
        {
            // check whether the user has selected a sketch plane
            if (null == sketchPlaneComboBox.SelectedValue)
            {
                return false;
            }

            // check integrity according to the curve type
            switch (createType)
            {
                case LineType.ModelLine:
                    // If the user want to create a line, check first and second points
                    return (firstPointUserControl.AssertPointIntegrity()
                        && secondPointUserControl.AssertPointIntegrity());
                case LineType.ModelArc:
                    // If the user create an arc, must check first, second and third points
                    return (firstPointUserControl.AssertPointIntegrity()
                    && secondPointUserControl.AssertPointIntegrity()
                    && thirdPointUserControl.AssertPointIntegrity());
                case LineType.ModelEllipse:             // ellipse
                case LineType.ModelHermiteSpline:       // hermite spline
                case LineType.ModelNurbSpline:          // nurb spline
                    // If the user create ellipse, hermite or nurb spline, 
                    // check offset point and whether an element id has been selected
                    if (null == elementIdComboBox.SelectedValue)
                    {
                        // Because this is a combobox, only when no element id to be selected, 
                        // the SelectedValue property is null. So give information as following
                        Autodesk.Revit.UI.TaskDialog.Show("Revit", "Can't create line, please draw a same line first.");
                        return false;
                    }
                    return (offsetPointUserControl.AssertPointIntegrity());

                default:
                    Autodesk.Revit.UI.TaskDialog.Show("Revit", "Invalid create type has been found.");
                    return false;
            }
        }

        /// <summary>
        /// Get the curve type for creation from the UI
        /// </summary>
        /// <returns>the curve type enum selected by the user</returns>
        LineType GetCurveType()
        {
            if (lineRadioButton.Checked)    // the user check lineRadioButton
            {
                return LineType.ModelLine;
            }
            else if (arcRadioButton.Checked)// the user check lineRadioButton
            {
                return LineType.ModelArc;
            }
            else if (ellipseRadioButton.Checked)// the user check ellipseRadioButton
            {
                return LineType.ModelEllipse;
            }
            else if (hermiteSplineRadioButton.Checked)  // the user check hermiteSplineRadioButton
            {
                return LineType.ModelHermiteSpline;
            }
            else                                // the user check nurbSplineRadioButton
            {
                return LineType.ModelNurbSpline;
            }
        }

        /// <summary>
        /// Bind the data source of the ComboBox
        /// </summary>
        /// <param name="control">the object of the ComboBox</param>
        /// <param name="dataSource">the data source object</param>
        void BindComboBox(ComboBox control, Object dataSource)
        {
            control.DataSource = null;              // clear the DataSource first
            control.DataSource = dataSource;        // rebind data source
            control.DisplayMember = "DisplayText";  // reset the DisplayMember 
            control.ValueMember = "Id";             // reset the ValueMember
        }


        /// <summary>
        /// Rebind the data source of the elementIdComboBox control according to the line type
        /// </summary>
        /// <param name="type">indicate which type element(line)</param>
        void ReBindElementIdComboBox(LineType type)
        {
            // Store the selected index property
            int selectedIndex = elementIdComboBox.SelectedIndex;
            switch (type)
            {
                case LineType.ModelEllipse:     // if it is model ellipse
                    BindComboBox(elementIdComboBox, m_dataBuffer.EllispeIDArray);
                    break;
                case LineType.ModelHermiteSpline:   // if it is model hermite spline
                    BindComboBox(elementIdComboBox, m_dataBuffer.HermiteSplineIDArray);
                    break;
                case LineType.ModelNurbSpline:      // if it is model nurb spline
                    BindComboBox(elementIdComboBox, m_dataBuffer.NurbSplineIDArray);
                    break;
                default:
                    return;
            }

            // Reset the selected index property
            elementIdComboBox.SelectedIndex = selectedIndex;
        }

        #endregion


        #region UI Event

        /// <summary>
        /// When the user click the create button, invoke method to create ReferencePlane
        /// </summary>
        void createButton_Click(object sender, EventArgs e)
        {
            // Define some local data
            Autodesk.Revit.DB.XYZ firstPoint;     // Store the data of first point for line or arc       
            Autodesk.Revit.DB.XYZ secondPoint;    // Store the data of second point for line or arc
            Autodesk.Revit.DB.XYZ thirdPoint;     // Store the data of third point only for arc
            Autodesk.Revit.DB.XYZ offsetPoint;    // Store the data of offset point for other lines
            ElementId modelLineId;    // Store the selected element id using in creation
            ElementId sketchPlaneId;  // Store the selected sketch id using in creation

            // First, get the create curve type.
            LineType createType = GetCurveType();

            // Second, check data integrity          
            if (!AssertDataIntegrity(createType)) // Check whether the data are integrity 
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", "Please make the data integrated first.");
                return;
            }

            // Third, get the data from UI and then invoke method to create.
            try
            {
                // Get the sketch plane id from the combobox control
                sketchPlaneId = (ElementId)sketchPlaneComboBox.SelectedValue;

                // get other necessary information to create model lines
                switch (createType)
                {
                    case LineType.ModelArc:
                        firstPoint = firstPointUserControl.GetPointData();  // first point
                        secondPoint = secondPointUserControl.GetPointData();// second point
                        thirdPoint = thirdPointUserControl.GetPointData();  // third point
                        // Invoke the CreateArc method to create a model arc
                        m_dataBuffer.CreateArc(sketchPlaneId, firstPoint, secondPoint, thirdPoint);
                        break;
                    case LineType.ModelLine:
                        firstPoint = firstPointUserControl.GetPointData();  // first point
                        secondPoint = secondPointUserControl.GetPointData();// second point
                        // Invoke the CreateLine method to create a model line
                        m_dataBuffer.CreateLine(sketchPlaneId, firstPoint, secondPoint);
                        break;
                    case LineType.ModelEllipse:         // to create model ellipse
                    case LineType.ModelHermiteSpline:   // to create model hermite spline
                    case LineType.ModelNurbSpline:      // to create model nurb spline
                        // Get the selected element id which copy curve from
                        modelLineId = (ElementId)elementIdComboBox.SelectedValue;
                        offsetPoint = offsetPointUserControl.GetPointData();// offset point
                        m_dataBuffer.CreateOthers(sketchPlaneId, modelLineId, offsetPoint);
                        // Rebind the data source of the elementIdComboBox to refresh it
                        ReBindElementIdComboBox(createType);
                        break;
                    default:    // the route should never arrive
                        Autodesk.Revit.UI.TaskDialog.Show("Revit", "Invalid create type has been found.");
                        return;
                }
            }
            catch (Exception ex)
            {
                // If some error occur during the creation, just show it
                Autodesk.Revit.UI.TaskDialog.Show("Revit", ex.Message);
            }

            // Refresh the form display.
            this.Refresh();
        }

        /// <summary>
        /// When the user click the close button, just close the form
        /// </summary>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// When the lineRadioButton checked state changed, this method is called
        /// If it is checked, make lineArcPanel visible and disable third point
        /// </summary>
        private void lineRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (false == lineRadioButton.Checked)
            {
                return;
            }

            // Set the prompt information
            lineArcInfoLabel.Text = "New line need information:";

            // Disable the input TextBox for third point
            thirdPointUserControl.Enabled = false;

            // Change the panel visible property
            lineArcPanel.Visible = true;    // make the lineArcPanel visible
            otherPanel.Visible = false;     // make the otherPanel not visible
        }

        /// <summary>
        /// When the arcRadioButton checked state changed, this method is called
        /// If it is checked, make lineArcPanel visible and able third point
        /// </summary>
        private void arcRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (false == arcRadioButton.Checked)
            {
                return;
            }

            // Set the prompt information
            lineArcInfoLabel.Text = "New arc need information:";

            // Enable the input TextBox for third point
            thirdPointUserControl.Enabled = true;

            // Change the panel visible property
            lineArcPanel.Visible = true;    // make the lineArcPanel visible
            otherPanel.Visible = false;     // make the otherPanel not visible
        }

        /// <summary>
        /// When the ellipseRadioButton checked state changed, this method is called
        /// If it is checked, make otherPanel visible and reset the elementIdComboBox
        /// </summary>
        private void ellipseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (false == ellipseRadioButton.Checked)
            {
                return;
            }

            // Set the prompt information
            otherInfoLabel.Text = "New ellipse need information:";

            // Bing the elementIdComboBox DataSource to EllispeIDArray
            BindComboBox(elementIdComboBox, m_dataBuffer.EllispeIDArray);

            // Change the panel visible property
            otherPanel.Visible = true;      // make the otherPanel visible
            lineArcPanel.Visible = false;   // make the lineArcPanel not visible          
        }

        /// <summary>
        /// When the hermiteSplineRadioButton checked state changed, this method is called
        /// If it is checked, make otherPanel visible and reset the elementIdComboBox
        /// </summary>
        private void hermiteSplineRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (false == hermiteSplineRadioButton.Checked)
            {
                return;
            }

            // Set the prompt information
            otherInfoLabel.Text = "New hermite spline need information:";

            // Bing the elementIdComboBox DataSource to HermiteSplineIDArray
            BindComboBox(elementIdComboBox, m_dataBuffer.HermiteSplineIDArray);

            // Change the panel visible property
            lineArcPanel.Visible = false;   // make the lineArcPanel not visible
            otherPanel.Visible = true;      // make the otherPanel visible
        }

        /// <summary>
        /// When the NurbSplineRadioButton checked state changed, this method is called
        /// If it is checked, make otherPanel visible and reset the elementIdComboBox
        /// </summary>
        private void NurbSplineRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (false == NurbSplineRadioButton.Checked)
            {
                return;
            }

            // Set the prompt information
            otherInfoLabel.Text = "New nurb spline need information:";

            // Bing the elementIdComboBox DataSource to NurbSplineIDArray
            BindComboBox(elementIdComboBox, m_dataBuffer.NurbSplineIDArray);

            // Change the panel visible property
            lineArcPanel.Visible = false;   // make the lineArcPanel not visible
            otherPanel.Visible = true;      // make the otherPanel visible
        }

        /// <summary>
        /// When click the createSketchPlaneButton, create a new sketch plane in revit
        /// </summary>
        private void createSketchPlaneButton_Click(object sender, EventArgs e)
        {
            // Display a form to collect some necessary data
            using (SketchPlaneForm displayForm = new SketchPlaneForm(m_dataBuffer))
            {
                displayForm.ShowDialog();
            }

            // Rebind the data source of the sketchPlaneComboBox to refresh data
            BindComboBox(sketchPlaneComboBox, m_dataBuffer.SketchPlaneIDArray);
            // Set the selected Item to be the new created one
            sketchPlaneComboBox.SelectedIndex = sketchPlaneComboBox.Items.Count - 1;

            // Refresh the form display.
            this.Refresh();
        }

        #endregion
    }
}
