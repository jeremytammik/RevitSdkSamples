//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// This form is provided for user to define a new RebarShape.
    /// </summary>
    public partial class NewRebarShapeForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// RebarShapeDef object.
        /// </summary>
        private RebarShapeDef m_rebarShapeDef;

        /// <summary>
        /// Autodesk Revit Application object.
        /// </summary>
        private Autodesk.Revit.ApplicationServices.Application m_rvtApp;

        /// <summary>
        /// Autodesk Revit Document Object.
        /// </summary>
        private Autodesk.Revit.DB.Document m_rvtDoc;

        /// <summary>
        /// Binding source for parameters ListBox.
        /// </summary>
        private BindingSource m_parametersListBoxBinding = new BindingSource();

        /// <summary>
        /// Binding source for constraints ListBox.
        /// </summary>
        private BindingSource m_constraintsListBoxBinding = new BindingSource();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NewRebarShapeForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor, Initialize the fields.
        /// </summary>
        /// <param name="rvtApp">Revit Application object</param>
        /// <param name="shapeDef">RebarShapeDef object</param>
        public NewRebarShapeForm(Autodesk.Revit.DB.Document rvtDoc, RebarShapeDef shapeDef)
            : this()
        {
            m_rebarShapeDef = shapeDef;
            m_rvtDoc = rvtDoc;
            m_rvtApp = rvtDoc.Application;
        }

        /// <summary>
        /// If this return true, the hook of RebarShape will be set, otherwise not.
        /// </summary>
        public bool NeedSetHooks
        {
            get
            {
                return useHooksCheckBox.Checked;
            }
        }

        /// <summary>
        /// Return start hook angle.
        /// </summary>
        public int StartHookAngle
        {
            get
            {
                return  int.Parse(startHookAngleComboBox.Text);
            }
        }

        /// <summary>
        /// Return end hook angle.
        /// </summary>
        public int EndHookAngle
        {
            get
            {
                return int.Parse(endHookAngleComboBox.Text);
            }
        }

        /// <summary>
        /// Return start hook orientation.
        /// </summary>
        public RebarHookOrientation StartHookOrientation
        {
            get
            {
                return (RebarHookOrientation)Enum.Parse(
                    typeof(RebarHookOrientation), startHookOrientationComboBox.Text);
            }
        }

        /// <summary>
        /// Return end hook orientation.
        /// </summary>
        public RebarHookOrientation EndHookOrientation
        {
            get
            {
                return (RebarHookOrientation)Enum.Parse(
                    typeof(RebarHookOrientation), endHookOrientationcomboBox.Text);
            }
        }

        /// <summary>
        /// Get a definition group if there exists one, otherwise, a new one will be created. 
        /// </summary>
        /// <returns>Definition group</returns>
        private DefinitionGroup GetOrCreateDefinitionGroup()
        {
            DefinitionFile file = null;

            int count = 0;
            // A count is to avoid infinite loop
            while (null == file && count < 100)
            {
                file = m_rvtApp.OpenSharedParameterFile();
                if (file == null)
                {
                    // If Shared parameter file does not exist, then create a new one.
                    string shapeFile =
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                        + "\\ParameterFile.txt";

                    // Fill Schema data of Revit shared parameter file.
                    // If no this schema data, OpenSharedParameterFile may alway return null.
                    System.Text.StringBuilder contents = new System.Text.StringBuilder();
                    contents.AppendLine("# This is a Revit shared parameter file.");
                    contents.AppendLine("# Do not edit manually.");
                    contents.AppendLine("*META	VERSION	MINVERSION");
                    contents.AppendLine("META	2	1");
                    contents.AppendLine("*GROUP	ID	NAME");
                    contents.AppendLine("*PARAM	GUID	NAME	DATATYPE	DATACATEGORY	GROUP	VISIBLE");

                    // Write Schema data of Revit shared parameter file.
                    File.WriteAllText(shapeFile, contents.ToString());

                    // Set Revit shared parameter file
                    m_rvtApp.SharedParametersFilename = shapeFile;
                }

                // To avoid infinite loop.
                ++count;
            }            

            // Get or create a definition group "Rebar Shape Parameters".
            DefinitionGroup group = file.Groups.get_Item("Rebar Shape Parameters");
            if (group == null)
                group = file.Groups.Create("Rebar Shape Parameters");
            return group;
        }

        /// <summary>
        /// Present a dialog to add a parameter to RebarShapeDef.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addParameterButton_Click(object sender, EventArgs e)
        {
            using (AddParameter addParam = new AddParameter(m_rebarShapeDef.Parameters))
            {
                if( DialogResult.OK == addParam.ShowDialog())
                {
                    Type paramType = addParam.IsFormula ? 
                        typeof(RebarShapeParameterFormula) : 
                        typeof(RebarShapeParameterDouble);

                    object paramName = addParam.ParamName;
                    object paramValue = addParam.ParamValue;
                    if (!addParam.IsFormula)
                        paramValue = double.Parse(addParam.ParamValue);

                    // Add the parameter to RebarShapeDef.
                    RebarShapeParameter param = m_rebarShapeDef.AddParameter(paramType, paramName, paramValue);
                    propertyGrid.SelectedObject = param;
                    m_parametersListBoxBinding.ResetBindings(false);
                    parameterListBox.SelectedItem = param;
                }
            }
        }

        /// <summary>
        /// Present a dialog to add a constraint to RebarShapeDef.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addConstraintButton_Click(object sender, EventArgs e)
        {
            using (AddConstraint addConstraint = new AddConstraint(m_rebarShapeDef.AllowedConstraintTypes()))
            {
                if(DialogResult.OK == addConstraint.ShowDialog())
                {
                    Type constraintType = addConstraint.ConstraintType;

                    // Add the constraint to RebarShapeDef.
                    ConstraintOnRebarShape constraint = m_rebarShapeDef.AddConstraint(constraintType);
                    propertyGrid.SelectedObject = constraint;
                    m_constraintsListBoxBinding.ResetBindings(false);
                    constraintListBox.SelectedItem = constraint;
                }
            }
        }

        /// <summary>
        /// OK Button, commit the RebarShapeDef, if there are not any exception,
        /// The RebarShape will be added to Revit Document successfully.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                m_rebarShapeDef.Commit(m_rvtDoc, GetOrCreateDefinitionGroup());
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Revit", "Rebar shape creation failed:" + ex.ToString());
                return;
            }
            
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Cancel Button, Cancel the creation of RebarShape.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Load event, Initialize the controls data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRebarShapeForm_Load(object sender, EventArgs e)
        {
            // Initialize parameters' and constraints' listBox data source.
            m_parametersListBoxBinding.DataSource = m_rebarShapeDef.Parameters;
            m_constraintsListBoxBinding.DataSource = m_rebarShapeDef.Constraints;
            parameterListBox.DataSource = m_parametersListBoxBinding;
            constraintListBox.DataSource = m_constraintsListBoxBinding;
            parameterListBox.DisplayMember = "Name";
            constraintListBox.DisplayMember = "Name";
            m_parametersListBoxBinding.ResetBindings(true);
            m_constraintsListBoxBinding.ResetBindings(true);

            // Initialize start and end hook angles comboBox data source.
            int[] startAngles = new int[4] { 0, 90, 135, 180 };
            int[] endAngles = new int[4] { 0, 90, 135, 180 };
            startHookAngleComboBox.DataSource = startAngles;            
            endHookAngleComboBox.DataSource = endAngles;

            // Initialize start and end hook orientation comboBox data source.
            string[] startOritationNames = Enum.GetNames(typeof(RebarHookOrientation));
            string[] endOritationNames = Enum.GetNames(typeof(RebarHookOrientation));
            startHookOrientationComboBox.DataSource = startOritationNames;            
            endHookOrientationcomboBox.DataSource = endOritationNames;
        }

        /// <summary>
        /// When the selection in the constraint listBox is changed, the property grid's
        /// content should be changed too.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void constraintListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = constraintListBox.SelectedItem;
        }

        /// <summary>
        /// When the selection in the parameter listBox is changed, the property grid's
        /// content should be changed too.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parameterListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = parameterListBox.SelectedItem;
        }

        /// <summary>
        /// When the check status of useHooksCheckBox is changed, other comboBox's 
        /// enabled status should be changed too.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void useHooksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnalbled = useHooksCheckBox.Checked;
            startHookAngleComboBox.Enabled = isEnalbled;
            startHookOrientationComboBox.Enabled = isEnalbled;
            endHookAngleComboBox.Enabled = isEnalbled;
            endHookOrientationcomboBox.Enabled = isEnalbled;
        }
    }
}