//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using System.Xml;
using System.Xml.Linq;

namespace Revit.SDK.Samples.RoutingPreferenceTools.CS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="application"></param>
        public MainWindow(Autodesk.Revit.UI.UIApplication application)
        {
            InitializeComponent();
            m_application = application;
            if (m_application.ActiveUIDocument == null)
            {
                Autodesk.Revit.UI.TaskDialog.Show("RP Analysis", "Null Document");
                this.Close();
            }
            PopulatePipeTypeList();
            PopulateSizeList();

        }

        /// <summary>
        /// Get the selected pipe size (in inches) from the dialog
        /// </summary>
        private double GetSelectedSize()
        {
            return double.Parse(comboBox_Sizes.SelectedItem.ToString());
        }

        /// <summary>
        /// Display all sizes available in given pipe type in the dialog.
        /// </summary>
        private void PopulateSizeList()
        {

            comboBox_Sizes.Items.Clear();
            List<double> sizes = Analyzer.GetAvailableSegmentSizes(GetSelectedPipeType().RoutingPreferenceManager, m_application.ActiveUIDocument.Document);
            foreach (double size in sizes)
            {
                double unitAdjustedSize = Convert.ConvertValueDocumentUnits(size, m_application.ActiveUIDocument.Document);
                comboBox_Sizes.Items.Add(unitAdjustedSize.ToString());
            }
            comboBox_Sizes.SelectedIndex = 0;
        }



        /// <summary>
        /// Get all the PipeTypes in the document and display them in the window's combo box.
        /// </summary>
        private void PopulatePipeTypeList()
        {
            FilteredElementCollector collector = new FilteredElementCollector(m_application.ActiveUIDocument.Document);
            collector.OfClass(typeof(PipeType));
            foreach (PipeType pipeType in collector.ToElements().Cast<PipeType>())
            {
                comboBox_PipeTypes.Items.Add(pipeType.Name + ", Id: " + pipeType.Id);
            }
            comboBox_PipeTypes.SelectedIndex = 0;


        }
        /// <summary>
        /// Look up a pipe type by ElementId string and return it.
        /// </summary>
        /// <returns></returns>
        private PipeType GetSelectedPipeType()
        {
            string selectedString = comboBox_PipeTypes.SelectedItem.ToString();
            string[] selectedsplit = selectedString.Split(':');
            if (selectedsplit.Length != 2)
                throw new ArgumentException();

            ElementId id = new ElementId(int.Parse(selectedsplit[1]));
            return m_application.ActiveUIDocument.Document.GetElement(id) as PipeType;
        }


        /// <summary>
        /// Get the selected PipeType from the combo box in the window, get its RoutingPreferenceManager, and analyze it for problems.
        /// Write all potential problems to the window's text field as formatted XML.
        /// </summary>
        private void button_CheckWarnings_Click(object sender, RoutedEventArgs e)
        {
            Analyzer routingPrefernceAnalyzer = new Analyzer(GetSelectedPipeType().RoutingPreferenceManager, this.m_application.ActiveUIDocument.Document);
            XDocument results = routingPrefernceAnalyzer.GetWarnings();

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.NewLineOnAttributes = false;
            StringBuilder output = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(output, xmlWriterSettings);
            results.WriteTo(writer);
            writer.Flush();
            writer.Close();
            this.textBox_output.Text = output.ToString();

        }

        /// <summary>
        /// Button CheckSpecificSize click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CheckSpecificSize_Click(object sender, RoutedEventArgs e)
        {
            Analyzer routingPrefernceAnalyzer = new Analyzer(GetSelectedPipeType().RoutingPreferenceManager, GetSelectedSize(), this.m_application.ActiveUIDocument.Document);
            XDocument results = routingPrefernceAnalyzer.GetSpecificSizeQuery();

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.NewLineOnAttributes = false;
            StringBuilder output = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(output, xmlWriterSettings);
            results.WriteTo(writer);
            writer.Flush();
            writer.Close();
            this.textBox_output.Text = output.ToString();
        }

        /// <summary>
        /// ComboBox PipeTypes selection changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_PipeTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateSizeList();
        }

        private Autodesk.Revit.UI.UIApplication m_application;

    }



}
