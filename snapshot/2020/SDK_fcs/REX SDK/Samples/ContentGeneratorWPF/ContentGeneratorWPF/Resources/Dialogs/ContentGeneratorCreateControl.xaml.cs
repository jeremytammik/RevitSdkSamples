//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using REX.Common;

namespace REX.ContentGeneratorWPF.Resources.Dialogs
{
    /// <summary>
    /// Represents the control which allows the user to select a new type which will be applied to the selected element in Revit.
    /// </summary>
    public partial class ContentGeneratorCreateControl : REX.Common.REXExtensionControl
    {
        /// <summary>
        /// Help flag to freeze events.
        /// </summary>
        bool Freeze = false;
        /// <summary>
        /// The parametric section.
        /// </summary>
        REX.ContentGenerator.Families.REXFamilyType_ParamSection ParametricSection;
        /// <summary>
        /// The database section.
        /// </summary>
        REX.ContentGenerator.Families.REXFamilyType_DBSection DatabaseSection;
        /// <summary>
        /// The object which manages the database browser dialog.
        /// </summary>
        REX.ContentGenerator.Dialog.REXContentDialogManager DBExplorerDialogManager;
        /// <summary>
        /// The settings of the database browser dialog.
        /// </summary>
        REX.ContentGenerator.Dialog.REXContentDialogSettings DBExplorerSettings;
        /// <summary>
        /// Gets the main extension.
        /// </summary>
        /// <value>The main extension.</value>
        internal Extension ThisMainExtension
        {
            get
            {
                return (Extension)ThisExtension;
            }
        }
        /// <summary>
        /// Gets the parametric description.
        /// </summary>
        /// <value>The parametric description.</value>
        REX.ContentGenerator.Families.REXSectionParamDescription ParametricDescription
        {
            get
            {
                return ParametricSection.Parameters;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentGeneratorCreateControl"/> class.
        /// </summary>
        public ContentGeneratorCreateControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentGeneratorCreateControl"/> class.
        /// </summary>
        /// <param name="extension">The extension object.</param>
        public ContentGeneratorCreateControl(REX.Common.REXExtension extension)
            : base(extension)
        {
            InitializeComponent();
            SetDefaultParamDescription();
            
        }
        /// <summary>
        /// Initializes the dialog with current settings.
        /// </summary>
        public void SetDialog()
        {
            //combo box
            comboParametricTypes.Items.Add(new REX.Controls.WPF.REXImageComboBoxItem()
                        { Image = REXLibrary.GetResourceImage(GetType().Assembly,"Resources/Other/Images/bmp_I.png")
                        , Text = "I"});
            comboParametricTypes.Items.Add(new REX.Controls.WPF.REXImageComboBoxItem() { Image = REXLibrary.GetResourceImage(GetType().Assembly, "Resources/Other/Images/bmp_L.png") 
                        , Text = "Angle"});
            comboParametricTypes.Items.Add(new REX.Controls.WPF.REXImageComboBoxItem() { Image = REXLibrary.GetResourceImage(GetType().Assembly, "Resources/Other/Images/bmp_RECT.png") 
                        , Text = "Rectangle"});

            //edit boxes
            editBoxB.UnitEngine = ThisExtension.System.Units.UnitsBase;
            editBoxH.UnitEngine = ThisExtension.System.Units.UnitsBase;
            editBoxTF.UnitEngine = ThisExtension.System.Units.UnitsBase;
            editBoxTW.UnitEngine = ThisExtension.System.Units.UnitsBase;
            editBoxA.UnitEngine = ThisExtension.System.Units.UnitsBase;
            editBoxIy.UnitEngine = ThisExtension.System.Units.UnitsBase;
            editBoxIz.UnitEngine = ThisExtension.System.Units.UnitsBase;
            
            editBoxB.RangeMinCheck = true;
            editBoxB.SetBaseMinValue(0.001);//1mm
            
            editBoxH.RangeMinCheck = true;
            editBoxH.SetBaseMinValue(0.001);//1mm           

            editBoxTF.RangeMinCheck = true;
            editBoxTF.SetBaseMinValue(0.001);//1mm            

            editBoxTW.RangeMinCheck = true;
            editBoxTW.SetBaseMinValue(0.001);//1mm     

            UpdateParamControls();
            UpdateParamViewer();

            SetDBBrowser();
        }
        /// <summary>
        /// Updates data based on current settings.
        /// </summary>
        public void SetData()
        {
            if (radioDatabase.IsChecked.Value)
                ThisMainExtension.Data.NewSection = DatabaseSection;
            else
                ThisMainExtension.Data.NewSection = ParametricSection;
        }
        /// <summary>
        /// Initializes the database browser.
        /// </summary>
        private void SetDBBrowser()
        {
            DBExplorerDialogManager = new REX.ContentGenerator.Dialog.REXContentDialogManager(ThisExtension.Context);

            DBExplorerSettings = new REX.ContentGenerator.Dialog.REXContentDialogSettings();
            REX.ContentGenerator.Dialog.REXContentDialogCategorySettings catSet = DBExplorerSettings.SectionDBSettings;
            DBExplorerSettings.DialogMode = REX.ContentGenerator.Dialog.EContentDialogMode.Selection;
            DBExplorerSettings.SetOneCategoryVisible(REX.ContentGenerator.Families.ECategoryType.SECTION_DB);
            DBExplorerSettings.RegionalSettings = true;
            catSet.SelectedElement = DatabaseSection;
            DBExplorerSettings.Caption = "Selection";
            DBExplorerSettings.ValidateTypes = false;

            //In order to omit long initialization of the browser when it is first launched (after first button click)
            //the browser is initialized when module starts. It doesn't make it faster but it is not so annoying for
            //the final user.
            DBExplorerDialogManager.SetApplicationInstance(DBExplorerSettings);
        }
        /// <summary>
        /// Initializes the default parametric description.
        /// </summary>
        private void SetDefaultParamDescription()
        {
            ParametricSection = new REX.ContentGenerator.Families.REXFamilyType_ParamSection();
            ParametricDescription.Dimensions.b = 0.3;//300mm
            ParametricDescription.Dimensions.h = 0.3;//300mm
            ParametricDescription.Dimensions.tw = 0.01;//10mm
            ParametricDescription.Dimensions.tf = 0.01;//10mm
            ParametricDescription.SectionType = REX.ContentGenerator.Families.ESectionType.I;
            ParametricDescription.CalculataMainAxisAndCharacteristics();
        }
        /// <summary>
        /// Updates the parametric controls.
        /// </summary>
        private void UpdateParamControls()
        {
            if (!Freeze)
            {
                Freeze = true;
                if (ParametricDescription.SectionType == REX.ContentGenerator.Families.ESectionType.I)
                    comboParametricTypes.SelectedIndex = 0;
                else if (ParametricDescription.SectionType == REX.ContentGenerator.Families.ESectionType.L)
                    comboParametricTypes.SelectedIndex = 1;
                else if (ParametricDescription.SectionType == REX.ContentGenerator.Families.ESectionType.RECT)
                    comboParametricTypes.SelectedIndex = 2;

                editBoxB.SetDataBaseValue(ParametricDescription.Dimensions.b);
                editBoxH.SetDataBaseValue(ParametricDescription.Dimensions.h);
                editBoxTF.SetDataBaseValue(ParametricDescription.Dimensions.tf);
                editBoxTW.SetDataBaseValue(ParametricDescription.Dimensions.tw);
                editBoxA.SetDataBaseValue(ParametricDescription.Characteristics.A);
                editBoxIy.SetDataBaseValue(ParametricDescription.Characteristics.IY);
                editBoxIz.SetDataBaseValue(ParametricDescription.Characteristics.IZ);

                editBoxTF.IsEnabled = (ParametricDescription.SectionType != REX.ContentGenerator.Families.ESectionType.RECT);
                editBoxTW.IsEnabled = (ParametricDescription.SectionType != REX.ContentGenerator.Families.ESectionType.RECT);
                Freeze = false;
            }
        }
        /// <summary>
        /// Updates the parametric description.
        /// </summary>
        private void UpdateParamDescription()
        {
            ParametricDescription.Dimensions.b = editBoxB.GetDataBaseValue();
            ParametricDescription.Dimensions.h = editBoxH.GetDataBaseValue();
            ParametricDescription.Dimensions.tw = editBoxTW.GetDataBaseValue();
            ParametricDescription.Dimensions.tf = editBoxTF.GetDataBaseValue();

            switch(comboParametricTypes.SelectedIndex)
            {
                case 0:
                    ParametricDescription.SectionType = REX.ContentGenerator.Families.ESectionType.I;
                    break;
                case 1:
                    ParametricDescription.SectionType = REX.ContentGenerator.Families.ESectionType.L;
                    break;
                case 2:
                    ParametricDescription.SectionType = REX.ContentGenerator.Families.ESectionType.RECT;
                    break;
            }

            ParametricDescription.CalculataMainAxisAndCharacteristics();
        }
        /// <summary>
        /// Updates the parametric viewer.
        /// </summary>
        private void UpdateParamViewer()
        {
            REX.ContentGenerator.Geometry.Contour_Section section = ParametricDescription.GetContour(true);

            if (section.Shape.Count > 0)
            {
                List<REX.Common.Geometry.REXxyz> points = section.Shape[0].GetPointList(true);
                viewer.DrawPolygon(points);
            }
        }
        /// <summary>
        /// Updates the database section properties.
        /// </summary>
        private void UpdateDatabaseSectionProperties()
        {
            if (DatabaseSection != null)
            {
                databasesProperties.ClearProperties();

                foreach (System.Reflection.PropertyInfo pi in DatabaseSection.Parameters.Description)
                {
                    object val = DatabaseSection.Parameters.Description.GetValue(pi.Name);

                    if (val != null && pi.Name != "Names")
                        databasesProperties.AddProperty(pi.Name, val.ToString());
                }

                foreach (System.Reflection.PropertyInfo pi in DatabaseSection.Parameters.Dimensions)
                {
                    object val = DatabaseSection.Parameters.Dimensions.GetValue(pi.Name);

                    AddFieldToDatabaseProperty(pi, val);
                }

                foreach (System.Reflection.PropertyInfo pi in DatabaseSection.Parameters.Characteristics)
                {
                    object val = DatabaseSection.Parameters.Characteristics.GetValue(pi.Name);

                    AddFieldToDatabaseProperty(pi, val);
                }
            }
        }
        /// <summary>
        /// Adds the field to the database property list.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <param name="val">The val.</param>
        private void AddFieldToDatabaseProperty(System.Reflection.PropertyInfo pi, object val)
        {
            REX.ContentGenerator.Converters.FieldParams fieldParams = ThisMainExtension.Converter.Settings.GetParamsForField(pi.Name, REX.ContentGenerator.Families.ECategoryType.SECTION_DB);

            if (val != null && fieldParams != null)
            {
                double valDbl = Convert.ToDouble(val);
                string txt = ThisMainExtension.System.Units.DisplayTextFromBase(valDbl, fieldParams.unitName, fieldParams.power, true);
                databasesProperties.AddProperty(pi.Name, txt);
            }
        }
        /// <summary>
        /// Handles the LostKeyboardFocus event of the editBoxH control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyboardFocusChangedEventArgs"/> instance containing the event data.</param>
        private void editBoxH_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!Freeze)
            {
                UpdateParamDescription();
                UpdateParamViewer();
            }
        }
        /// <summary>
        /// Handles the SelectionChanged event of the comboParametricTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void comboParametricTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Freeze)
            {
                UpdateParamDescription();
                UpdateParamControls();
                UpdateParamViewer();
            }
        }
        /// <summary>
        /// Handles the Checked event of the radioParametric control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioParametric_Checked(object sender, RoutedEventArgs e)
        {
            if (paramGroupBox != null && dbGroupBox != null)
            {
                gridParam.Visibility = radioParametric.IsChecked.Value? Visibility.Visible:Visibility.Hidden;
                gridDB.Visibility = radioDatabase.IsChecked.Value?Visibility.Visible:Visibility.Hidden;
            }
        }
        /// <summary>
        /// Handles the Click event of the buttonDBSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonDBSelect_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(ThisExtension.GetWindowForParent());

            if (DBExplorerDialogManager.ShowContentDialogParentIntPtr(DBExplorerSettings, helper.Handle))
            {
                this.DatabaseSection = DBExplorerSettings.SectionDBSettings.SelectedElement as REX.ContentGenerator.Families.REXFamilyType_DBSection;

                UpdateDatabaseSectionProperties();
            }
        }
    }
}
