//
// (C) Copyright 2003-2014 by Autodesk, Inc. All rights reserved.
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
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Events;


namespace Revit.SDK.Samples.SinePlotter.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {
        private string assemblyPath;
        private string assemblyName;
        private string imageFolder;

        private static double periodVal = 0.2;
        private static double cyclesVal = 10;
        private static double amplitudeVal = 3;
        private static double partitionsVal = 3;
        private static ComboBox prismComboBox;

        private TextBox periodBox;
        private TextBox cyclesBox;
        private TextBox amplitudeBox;
        private TextBox partitionsBox;


        #region IExternalApplication Members
        /// <summary>
        /// Implements the OnShutdown event
        /// </summary>
        /// <param name="application"></param>
        /// <returns>Indicates if the application completes its work successfully.</returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }


        /// <summary>
        /// Implements the OnStartup event
        /// </summary>
        /// <param name="application"></param>
        /// <returns>Indicates if the application completes its work successfully.</returns>
        public Result OnStartup(UIControlledApplication application)
        {
            assemblyPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            assemblyName = Assembly.GetExecutingAssembly().GetName().Name + ".dll";
            imageFolder = GetProjectDirectory() + "/Resources/";

            //add a panel to go on the Add-In tab
            RibbonPanel panel = application.CreateRibbonPanel("ArrayPrismsOnASineCurve");
            //add panel with text fields for user input for period, amplitude and cycles of sine curve
            AddCurvePropertiesTextFields(panel);
            //add panel with text field for user input for number of partitions along the curve
            AddPartitionsTextField(panel);
            panel.AddSeparator();
            //add drop down menu for type of prism selection
            AddPrismComboBox(panel);
            panel.AddSeparator();
            //add RUN button
            AddRunButton(panel);

            return Result.Succeeded;
        }


        /// <summary>
        /// Gets the name of the currently selected family symbol.
        /// </summary>
        /// <returns>A string representing the name of the currently selected family symbol</returns>
        public static String GetFamilySymbolName()
        {
            return prismComboBox.Current.Name;
        }


        /// <summary>
        /// Gets the value corresponding to the current text field input about the curve period.
        /// </summary>
        /// <returns>The double value corresponding to the curent text field input 
        /// for the curve period parameter.</returns>
        public static double GetPeriod()
        {
            return periodVal;
        }


        /// <summary>
        /// Gets the value corresponding to the current text field input about the curve
        /// number of cycles.
        /// </summary>
        /// <returns>The double value corresponding to the curent text field input 
        /// for the curve number of cycles parameter.</returns>
        public static double GetNumberOfCycles()
        {
            return cyclesVal;
        }


        /// <summary>
        /// Gets the value corresponding to the current text field about the curve amplitude.
        /// </summary>
        /// <returns>The double value corresponding to the curent text field input 
        /// for the curve amplitude parameter.</returns>
        public static double GetAplitude()
        {
            return amplitudeVal;
        }


        /// <summary>
        /// Gets the value corresponding to the current text field about the number of partitions.
        /// </summary>
        /// <returns>The double value corresponding to the curent text field input for the
        /// number of partitions.</returns>
        public static double GetNumberOfPartitions()
        {
            return partitionsVal;
        }


        /// <summary>
        /// Adds a drop down menu for selection of a type of prism. This four types
        /// correspond to 4 family symbols already loaded in the working Revit document.
        /// </summary>
        /// <param name="panel">the RibbonPanel where the UI element is added</param>
        private void AddPrismComboBox(RibbonPanel panel)
        {
            // create a four members combo box for family instance selection 
            //Family instance #1
            ComboBoxMemberData comboBoxMemberData1 = new ComboBoxMemberData("cylinder", "cylinder prism");
            //Family instance #2
            ComboBoxMemberData comboBoxMemberData2 = new ComboBoxMemberData("rectangle", "rectangular prism");
            //Family instance #3 
            ComboBoxMemberData comboBoxMemberData3 = new ComboBoxMemberData("regularpolygon", "regular polygon prism");
            //Family instance #4
            ComboBoxMemberData comboBoxMemberData4 = new ComboBoxMemberData("isotriangle", "isotriangle prism");

            //make a combo box group group 
            ComboBoxData comboBxData = new ComboBoxData("ComboBox");
            prismComboBox = panel.AddItem(comboBxData) as ComboBox;
            prismComboBox.ToolTip = "select a prism to array on a curve";
            prismComboBox.AddItem(comboBoxMemberData1);
            prismComboBox.AddItem(comboBoxMemberData2);
            prismComboBox.AddItem(comboBoxMemberData3);
            prismComboBox.AddItem(comboBoxMemberData4);
        }


        /// <summary>
        /// Adds a group of text fields for accepting user input for the various parameters
        /// of the sine curve (cycles, period, amplitude).
        /// </summary>
        /// <param name="panel">the RibbonPanel where the UI element is added</param>
        private void AddCurvePropertiesTextFields(RibbonPanel panel)
        {
            //Inactive textfields that just display information about the active input fields
            TextBoxData periodLabelData = new TextBoxData("curve period field");
            TextBoxData cyclesLabelData = new TextBoxData("curve cycles field");
            TextBoxData amplitudeLabelData = new TextBoxData("curve amplitude field");
            //Stack them horizontally
            IList<RibbonItem> textFieldList = panel.AddStackedItems(periodLabelData, cyclesLabelData, amplitudeLabelData);
            //Call method to customize the text boxes and make them inactive
            String periodToolTip = "A double value denoting the period of the curve, i.e. how often the curve goes a full repition around the unit circle.";
            CustomizeTextBox(panel, (TextBox)textFieldList[0], periodToolTip, "curve period:", 0, false, 130);
            String cyclesToolTip = "A double value denoting the number of circles the curve makes.";
            CustomizeTextBox(panel, (TextBox)textFieldList[1], cyclesToolTip, "curve cycles:", 0, false, 130);
            String amplitudeToolTip = "A double value denoting how far the curve gets away from the x-axis.";
            CustomizeTextBox(panel, (TextBox)textFieldList[2], amplitudeToolTip, "curve amplitude:", 0, false, 130);

            //Active text fields for user input
            TextBoxData periodBoxData = new TextBoxData("curve period");
            TextBoxData cyclesBoxData = new TextBoxData("curve cycles");
            TextBoxData amplitudeBoxData = new TextBoxData("curve amplitude");
            //Stack them horizontally
            IList<RibbonItem> textBoxList = panel.AddStackedItems(periodBoxData, cyclesBoxData, amplitudeBoxData);

            periodBox = (TextBox)textBoxList[0];
            cyclesBox = (TextBox)textBoxList[1];
            amplitudeBox = (TextBox)textBoxList[2];

            //Call method to customize the text boxes and make them active
            CustomizeTextBox(panel, periodBox,
                "Define the period of the sine curve", "" + periodVal, periodVal, true, 50);
            CustomizeTextBox(panel, cyclesBox,
                "Define the number of cycles of the sine curve", "" + cyclesVal, cyclesVal, true, 50);
            CustomizeTextBox(panel, amplitudeBox,
                "Define the amplitude of the sine curve", "" + amplitudeVal, amplitudeVal, true, 50);
        }


        /// <summary>
        /// Add a text field for accepting user input for how many family instances to array on
        /// the curve.
        /// </summary>
        /// <param name="panel">the RibbonPanel where the UI element is added</param>
        private void AddPartitionsTextField(RibbonPanel panel)
        {
            TextBoxData partitionsFieldData = new TextBoxData("curve partitions field");
            TextBox partitionsFieldBox = panel.AddItem(partitionsFieldData) as TextBox;
            //Call method to customize the text box and set it as inactive
            CustomizeTextBox(panel, partitionsFieldBox, null, "number of partitions:", 0, false, 130);
            //Call method to customize the text box and set it as active
            TextBoxData partitionsBoxData = new TextBoxData("curve partitions box");
            partitionsBox = panel.AddItem(partitionsBoxData) as TextBox;
            CustomizeTextBox(panel, partitionsBox, "Define the number of partitions", "" + partitionsVal, partitionsVal, true, 50);

        }


        /// <summary>
        /// Adds button that arrays family instances of curves given the various
        /// user inputs. 
        /// </summary>
        /// <param name="panel">the RibbonPanel where the UI element is added</param>
        private void AddRunButton(RibbonPanel panel)
        {
            PushButtonData pushButtonData = new PushButtonData("arrayPrisms", "run",
                assemblyPath + "\\" + assemblyName, "Revit.SDK.Samples.SinePlotter.CS.Command");
            pushButtonData.LargeImage = new BitmapImage(new Uri(imageFolder + "Start.png"));
            PushButton pushButton = panel.AddItem(pushButtonData) as PushButton;
        }


        private void CustomizeTextBox(RibbonPanel panel, TextBox txtBox, String tip, String displayedText,
            double defaultVal, Boolean isEnabled, int width)
        {
            txtBox.Value = displayedText;
            txtBox.ToolTip = tip;
            txtBox.Width = width;
            txtBox.Enabled = isEnabled;

            if (isEnabled)
            {
                txtBox.EnterPressed += new EventHandler<Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs>(TextBoxEnterPressed);
            }
        }


        /// <summary>
        /// Handles the action of the text box user input. Checks if the input value is an
        /// acceptable double value and if it lies among the acceptable range of values; if
        /// it is it updates the corresponding field variable, if not it displays a warning 
        /// message to the user and retains the previous value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The event arguments used by ComboBox's CurrentChanged event.
        /// </param>
        private void TextBoxEnterPressed(object sender, TextBoxEnterPressedEventArgs e)
        {
            //cast sender as TextBox to retrieve text value
            TextBox textBox = sender as TextBox;
            double value;
            if (double.TryParse(textBox.Value.ToString(), out value))
            {
                if ((textBox.Name).Equals(periodBox.Name))
                {
                    if (value < 0.1 || value > 3) TaskDialog.Show("TextBox Input",
                        "The input value for " + textBox.Name + " has to be between 0.1 and 3.0");
                    else periodVal = value;
                }
                else if ((textBox.Name).Equals(cyclesBox.Name))
                {
                    if (value <= 0) TaskDialog.Show("TextBox Input",
                        "The input value for " + textBox.Name + " has to be greater than zero.");
                    else cyclesVal = value;
                }
                else if ((textBox.Name).Equals(amplitudeBox.Name))
                {
                    if (value < -4 || value > 4) TaskDialog.Show("TextBox Input",
                        "The input value for " + textBox.Name + " has to be between -4.0 and 4.0");
                    else amplitudeVal = value;
                }
                else
                {
                    if (value <= 0) TaskDialog.Show("TextBox Input",
                        "The input value for " + textBox.Name + " has to be greater than zero.");
                    else partitionsVal = value;
                }
            }
            else
            {
                TaskDialog.Show("TextBox Input", "The input value for " + textBox.Name + " has to be a double.");
            }
        }


        /// <summary>
        /// Returns the path of the main project directory.
        /// </summary>
        /// <returns>A string object corresponding to the full path of the main project directory.</returns>
        private String GetProjectDirectory()
        {
            //get the absolut path of the assembly
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //move two directory levels back
            String grandParentDir = Directory.GetParent(assemblyPath).Parent.FullName;
            return grandParentDir;
        }

        #endregion
    }
}
