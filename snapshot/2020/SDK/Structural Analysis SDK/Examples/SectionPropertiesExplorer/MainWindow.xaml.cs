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
using System.ComponentModel;

using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.Revit.DB.CodeChecking.Engineering.Tools;

namespace SectionPropertiesExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Autodesk.Revit.DB.Element> revitElements = null;
        ElementAnalyser revitElementAnalyser = null;

        #region constructors
        //--------------------------------------------------------------

        public MainWindow()
        {
            revitUnitsUI = true;
            unitSystem = Autodesk.Revit.DB.DisplayUnit.IMPERIAL;
            InitializeComponent();
        }

        public MainWindow(List<Autodesk.Revit.DB.Element> revitElements, EventHandler<ElementSelectedEventArgs> ElementSelectedEventHandler = null)
            : this()
        {
            this.revitElements = revitElements;
            if (revitElements.Count > 0)
            {
                ConnectionViewModel cvm = new ConnectionViewModel(revitElements);
                DataContext = cvm;
            }

            this.ElementSelectedEventHandler = ElementSelectedEventHandler;
        }

        //--------------------------------------------------------------
        #endregion

        #region events
        //--------------------------------------------------------------

        public class ElementSelectedEventArgs : EventArgs
        {
            public ElementSelectedEventArgs(Autodesk.Revit.DB.Element selectedElement)
            {
                this.SelectedElement = selectedElement;
            }

            public Autodesk.Revit.DB.Element SelectedElement { get; private set; }
        }
        
        public event EventHandler<ElementSelectedEventArgs> ElementSelectedEventHandler;

        //--------------------------------------------------------------
        #endregion
        
        #region public methods
        //--------------------------------------------------------------
   
        public void DataBinding(Autodesk.Revit.DB.Element revitElement)
        {
            UnitsAssignment.RevitUnits = revitElement.Document.GetUnits();

            if (null == revitElementAnalyser)
            {
                revitElementAnalyser = revitUnitsUI ? new ElementAnalyser() : new ElementAnalyser(unitSystem);
                revitElementAnalyser.TSectionAnalysis = true;
            }

            ElementInfo elementInfo = revitElementAnalyser.Analyse(revitElement);

            switch (elementInfo.Type)
            {
                case ElementType.Beam:
                case ElementType.Column:
                    elementSizeLabel.Content = "Length:";
                    if (revitUnitsUI)
                    {
                        elementSizeValue.Content = UnitsAssignment.FormatToRevitUI("GeomLength", elementInfo.GeomLength(), ElementInfoUnits.Assignments, true);
                    }
                    else
                    {
                        elementSizeValue.Content = elementInfo.GeomLength().ToString();
                        elementSizeValue.Content += "  " + UnitsAssignment.GetUnitSymbol("GeomLength", ElementInfoUnits.Assignments, unitSystem);
                    }
                    
                    SectionShapeType shapeType = SectionShapeType.Unusual;
                    if (elementInfo.SectionsParams != null)
                    {
                       shapeType = elementInfo.SectionsParams.ShapeType;
                    }

                    bool tSectionCollapsedWhenSelected = false;
                    tabSlab.Visibility = System.Windows.Visibility.Collapsed;
                    tabWall.Visibility = System.Windows.Visibility.Collapsed;
                    if (elementInfo.Slabs != null && elementInfo.Slabs.TSection != null)
                    {
                        tabTSection.Visibility = System.Windows.Visibility.Visible;
                        tSectionDescription.DoBinding(elementInfo.Slabs, revitUnitsUI ? null : (Autodesk.Revit.DB.DisplayUnit?)unitSystem, elementInfo.GeomLength()); 
                    }
                    else
                    {
                        if (tabTSection.IsSelected)
                        {
                            tSectionCollapsedWhenSelected = true;
                        }
                        
                        tabTSection.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    tabProfile.Visibility = System.Windows.Visibility.Visible;

                    if (tabSlab.IsSelected || true == tSectionCollapsedWhenSelected || tabWall.IsSelected)
                    {
                        IInputElement previousFocusedElement = FocusManager.GetFocusedElement(this);
                        tabProfile.Focus();
                        if (previousFocusedElement != null)
                            previousFocusedElement.Focus();
                    }

                    sectionDescription.DoBinding(shapeType, 
                                                 elementInfo.Sections, 
                                                 elementInfo.Material != null ? elementInfo.Material.Category == MaterialCategory.Metal : false);
                    sectionParameters.DoBinding(revitElement.Name, elementInfo.SectionsParams, revitUnitsUI ? null : (Autodesk.Revit.DB.DisplayUnit?)unitSystem);
                    break;
                case ElementType.Slab:
                    elementSizeLabel.Content = "Thickness:";
                    if (revitUnitsUI)
                    {
                        if (elementInfo.Slabs.AsElement != null)
                        {
                            elementSizeValue.Content = UnitsAssignment.FormatToRevitUI("SlabThickness", elementInfo.Slabs.AsElement.Thickness(), ElementInfoUnits.Assignments, true);
                        }
                    }
                    else
                    {
                        if (elementInfo.Slabs.AsElement != null)
                        {
                            elementSizeValue.Content = elementInfo.Slabs.AsElement.Thickness().ToString();
                            elementSizeValue.Content += "  " + UnitsAssignment.GetUnitSymbol("SlabThickness", ElementInfoUnits.Assignments, unitSystem);
                        }
                    }

                    tabProfile.Visibility = System.Windows.Visibility.Collapsed;
                    tabTSection.Visibility = System.Windows.Visibility.Collapsed;
                    tabWall.Visibility = System.Windows.Visibility.Collapsed;
                    tabSlab.Visibility = System.Windows.Visibility.Visible;

                    if (tabProfile.IsSelected || tabTSection.IsSelected || tabWall.IsSelected)
                    {
                        IInputElement previousFocusedElement = FocusManager.GetFocusedElement(this);
                        tabSlab.Focus();
                        if (previousFocusedElement != null)
                            previousFocusedElement.Focus();
                    }

                    slabDescription.DoBinding(elementInfo.Slabs, revitUnitsUI ? null : (Autodesk.Revit.DB.DisplayUnit?)unitSystem);
                    break;
                case ElementType.Wall:
                    elementSizeLabel.Content = "Thickness:";
                    if (revitUnitsUI)
                    {
                        elementSizeValue.Content = UnitsAssignment.FormatToRevitUI("WallThickness", elementInfo.Walls.AsElement.Thickness(), ElementInfoUnits.Assignments, true);
                    }
                    else
                    {
                        elementSizeValue.Content = elementInfo.Walls.AsElement.Thickness().ToString();
                        elementSizeValue.Content += "  " + UnitsAssignment.GetUnitSymbol("SlabThickness", ElementInfoUnits.Assignments, unitSystem);
                    }

                    tabProfile.Visibility = System.Windows.Visibility.Collapsed;
                    tabTSection.Visibility = System.Windows.Visibility.Collapsed;
                    tabSlab.Visibility = System.Windows.Visibility.Collapsed;
                    tabWall.Visibility = System.Windows.Visibility.Visible;

                    if (tabProfile.IsSelected || tabSlab.IsSelected || tabTSection.IsSelected)
                    {
                        IInputElement previousFocusedElement = FocusManager.GetFocusedElement(this);
                        tabWall.Focus();
                        if (previousFocusedElement != null)
                            previousFocusedElement.Focus();
                    }

                    wallDescription.DoBinding(elementInfo.Walls.AsElement, revitUnitsUI ? null : (Autodesk.Revit.DB.DisplayUnit?)unitSystem);
                    break;
                case ElementType.Unknown:
                default:
                    elementSizeLabel.Content = "";
                    elementSizeValue.Content = "";

                    tabProfile.Visibility = System.Windows.Visibility.Collapsed;
                    tabSlab.Visibility = System.Windows.Visibility.Collapsed;
                    tabTSection.Visibility = System.Windows.Visibility.Collapsed;
                    tabWall.Visibility = System.Windows.Visibility.Collapsed;

                    if (tabProfile.IsSelected || tabSlab.IsSelected || tabTSection.IsSelected || tabWall.IsSelected)
                    {
                        IInputElement previousFocusedElement = FocusManager.GetFocusedElement(this);
                        tabMaterial.Focus();
                        if (previousFocusedElement != null)
                            previousFocusedElement.Focus();
                    }

                    break;
            }

            MaterialLayerDescription layerDescription = null;
            Layer structuralLayer = null;

            if (elementInfo.Type == ElementType.Slab)
            {
                if (elementInfo.Slabs.AsElement != null)
                {
                    structuralLayer = elementInfo.Slabs.AsElement.StructuralLayer();
                }
            }
            if (elementInfo.Type == ElementType.Wall)
            {
                structuralLayer = elementInfo.Walls.AsElement.StructuralLayer();
            }

            if (structuralLayer != null)
            {
                layerDescription = new MaterialLayerDescription(structuralLayer.Thickness, structuralLayer.Offset);
            }

            materialParameters.DoBinding(elementInfo.Material, layerDescription, revitUnitsUI ? null : (Autodesk.Revit.DB.DisplayUnit?)unitSystem);
        }
 
        //--------------------------------------------------------------
        #endregion

        #region private members and methods
        //--------------------------------------------------------------

        private void EnableElementsNavigateButtons()
        {
            if(elements.Items.CurrentPosition == 0)
            {
                previous.IsEnabled = false;
                next.IsEnabled = elements.Items.Count > 1 ? true : false;
            }
            else
            if (elements.Items.CurrentPosition == elements.Items.Count - 1)
            {
                previous.IsEnabled = true;
                next.IsEnabled = false;
            }
            else
            {
                previous.IsEnabled = true;
                next.IsEnabled = true;
            }

            SetImageToNavigateButton(previous_, previous.IsEnabled);
            SetImageToNavigateButton(next_, next.IsEnabled);
        }

        private void SetImageToNavigateButton(Image image, bool enabled)
        {
            BitmapImage source = new BitmapImage();
            source.BeginInit();
            string uriString = "/SectionPropertiesExplorer;component/Resources/Images/";
            uriString += image.Name + (enabled ? "enabled" : "disabled") + ".png";
            source.UriSource = new Uri(uriString, UriKind.Relative);
            source.EndInit();
            image.Source = source;
        }

        bool revitUnitsUI;
        private Autodesk.Revit.DB.DisplayUnit unitSystem;

        //--------------------------------------------------------------
        #endregion
        
        private void previous_Click(object sender, RoutedEventArgs e)
        {
            if(elements.Items.CurrentPosition > 0)
            {
                elements.Items.MoveCurrentToPrevious();
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if(elements.Items.CurrentPosition < elements.Items.Count -1)
            {
                elements.Items.MoveCurrentToNext();
            }
        }

        private void elements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableElementsNavigateButtons();
            if (elements.Items.CurrentPosition >= 0 && elements.Items.CurrentPosition < revitElements.Count)
            {
                DataBinding(revitElements[elements.Items.CurrentPosition]);
                if (ElementSelectedEventHandler != null)
                    ElementSelectedEventHandler(elements, new ElementSelectedEventArgs(revitElements[elements.Items.CurrentPosition]));
            }
        }

        private void radioRevit_Checked(object sender, RoutedEventArgs e)
        {
            revitElementAnalyser = null;
            revitUnitsUI = true;
            unitSystem = Autodesk.Revit.DB.DisplayUnit.IMPERIAL;
            if (revitElements != null && revitElements.Count > 0)
            {
                DataBinding(revitElements[elements.Items.CurrentPosition]);
            }
        }

        private void radioImperial_Checked(object sender, RoutedEventArgs e)
        {
            revitElementAnalyser = null;
            revitUnitsUI = false;
            unitSystem = Autodesk.Revit.DB.DisplayUnit.IMPERIAL;
            if (revitElements != null && revitElements.Count > 0)
            {
                DataBinding(revitElements[elements.Items.CurrentPosition]);
            }
        }

        private void radioMetric_Checked(object sender, RoutedEventArgs e)
        {
            revitElementAnalyser = null;
            revitUnitsUI = false;
            unitSystem = Autodesk.Revit.DB.DisplayUnit.METRIC;
            if (revitElements != null && revitElements.Count > 0)
            {
                DataBinding(revitElements[elements.Items.CurrentPosition]);
            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }

    class ElementEntry
    {
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public ElementEntry(Autodesk.Revit.DB.Element revitElement)
        {
            string elementDescription;
            Autodesk.Revit.DB.BuiltInCategory cat = (Autodesk.Revit.DB.BuiltInCategory)revitElement.Category.Id.IntegerValue;
            switch (cat)
            {
                case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralColumns:
                    elementDescription = "Column: ";
                    break;
                case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFraming:
                    elementDescription = "Beam: ";
                    break;
                case Autodesk.Revit.DB.BuiltInCategory.OST_Floors:
                case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFoundation:
                    elementDescription = "Slab: ";
                    break;
                case Autodesk.Revit.DB.BuiltInCategory.OST_Walls:
                case Autodesk.Revit.DB.BuiltInCategory.OST_WallAnalytical:
                    elementDescription = "Wall: ";
                    break;
                default:
                    elementDescription = "UnKnown: ";
                    break;
            }

            elementDescription += "( " + revitElement.Id.ToString() + " )";

            if (revitElement.Name.Length > 0)
            {
                elementDescription += "  ";
                elementDescription += revitElement.Name;
            }
            this.name = elementDescription;
        }

        private string name;
    }

    class ConnectionViewModel : INotifyPropertyChanged
    {
        public ConnectionViewModel(List<Autodesk.Revit.DB.Element> revitElements)
        {
            IList<ElementEntry> list = new List<ElementEntry>();
            foreach (Autodesk.Revit.DB.Element e in revitElements)
            {
                list.Add(new ElementEntry(e));
            }
            ElementEntries = new CollectionView(list);
            if(list.Count > 0)
               elementEntry = list[0].Name;
        }

        public CollectionView ElementEntries { get; private set; }
        
        public string ElementEntry
        {
            get
            {
                return elementEntry;
            }
            set
            {
                if (elementEntry == value)
                    return; 
                elementEntry = value;
                OnPropertyChanged("ElementEntry");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string elementEntry;
    }
}
