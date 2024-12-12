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
using System.Collections;
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

using Autodesk.Revit.DB.CodeChecking.Engineering;

namespace SectionPropertiesExplorer
{
    /// <summary>
    /// Interaction logic for SectionParameters.xaml
    /// </summary>
    public partial class SectionParametersControl : UserControl
    {
        public SectionParametersControl()
        {
            InitializeComponent();
        }

        public void DoBinding(string sectionName, ElementSectionsParamsInfo sectionsParams, Autodesk.Revit.DB.DisplayUnit? unitSystem)
        {
            this.sectionName.Content = sectionName;
            
            if (null == sectionsParams)
            {
                sectionParameters.ItemsSource = null;
                return;
            }

            IEnumerable itemsSource = CreateItemSource(0, sectionsParams.AtTheBeg.Dimensions,
                                                          sectionsParams.Tapered ? sectionsParams.AtTheEnd.Dimensions : null,
                                                          sectionsParams.AtTheBeg.Characteristics,
                                                          sectionsParams.Tapered ? sectionsParams.AtTheEnd.Characteristics : null,
                                                          unitSystem);

            if (sectionsParams.ShapeType != SectionShapeType.DoubleSection && sectionsParams.ShapeType != SectionShapeType.CompoundSection)
            {
                tabControl.Visibility = System.Windows.Visibility.Collapsed;

                compoundSectionParameters.Visibility = System.Windows.Visibility.Collapsed;
                compoundSectionParameters.ItemsSource = null;
                
                sectionParameters.Visibility = System.Windows.Visibility.Visible;
                sectionParameters.ItemsSource = itemsSource;
            }
            else
            {
                tabControl.Visibility = System.Windows.Visibility.Visible;

                compoundSectionParameters.Visibility = System.Windows.Visibility.Visible;
                compoundSectionParameters.ItemsSource = itemsSource;

                sectionParameters.Visibility = System.Windows.Visibility.Collapsed;
                sectionParameters.ItemsSource = null;
            }

            bool isCompoundSection = SectionShapeType.CompoundSection == sectionsParams.ShapeType;

            int maxIndex = (isCompoundSection ? 1 : 0);

            if (sectionsParams.ShapeType == SectionShapeType.DoubleSection || isCompoundSection)
            {
                if (sectionsParams.AtTheBeg.ComponentsDimensions.Count > maxIndex &&
                    sectionsParams.AtTheBeg.ComponentsCharacteristics.Count > maxIndex &&
                    (sectionsParams.Tapered ? sectionsParams.AtTheEnd.ComponentsDimensions.Count > maxIndex : true) &&
                    (sectionsParams.Tapered ? sectionsParams.AtTheEnd.ComponentsCharacteristics.Count > maxIndex : true))
                {
                    if (sectionsParams.ComponentsShapeType.Count > maxIndex)
                    {
                        tabSection1.Header = Tools.SectionShapeTypeName(sectionsParams.ComponentsShapeType[maxIndex]) + " component";
                    }
                    else
                    {
                        tabSection2.Header = "Unknown type";
                    }
        
                    section1Parameters.ItemsSource = CreateItemSource(isCompoundSection ? 1 : 0,
                                                                      sectionsParams.AtTheBeg.ComponentsDimensions[maxIndex],
                                                                      sectionsParams.Tapered ? sectionsParams.AtTheEnd.ComponentsDimensions[maxIndex] : null,
                                                                      sectionsParams.AtTheBeg.ComponentsCharacteristics[maxIndex],
                                                                      sectionsParams.Tapered ? sectionsParams.AtTheEnd.ComponentsCharacteristics[maxIndex] : null,
                                                                      unitSystem);
                    tabSection1.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    tabSection1.Visibility = System.Windows.Visibility.Collapsed;
                    section1Parameters.ItemsSource = null;
                }
            }
            else
            {
                tabSection1.Visibility = System.Windows.Visibility.Collapsed;
                section1Parameters.ItemsSource = null;
            }

            if (sectionsParams.ShapeType == SectionShapeType.CompoundSection)
            {
                if (sectionsParams.AtTheBeg.ComponentsDimensions.Count > 0 &&
                    sectionsParams.AtTheBeg.ComponentsCharacteristics.Count > 0 &&
                    sectionsParams.Tapered ? sectionsParams.AtTheEnd.ComponentsDimensions.Count > 0 : true &&
                    sectionsParams.Tapered ? sectionsParams.AtTheEnd.ComponentsCharacteristics.Count > 0 : true)
                {
                    if (sectionsParams.ComponentsShapeType.Count > 0)
                    {
                        tabSection2.Header = Tools.SectionShapeTypeName(sectionsParams.ComponentsShapeType[0]) + " component";
                    }
                    else
                    {
                        tabSection2.Header = "Unknown type";
                    }
                    
                    section2Parameters.ItemsSource = CreateItemSource(2, sectionsParams.AtTheBeg.ComponentsDimensions[0],
                                                                         sectionsParams.Tapered ? sectionsParams.AtTheEnd.ComponentsDimensions[0] : null,
                                                                         sectionsParams.AtTheBeg.ComponentsCharacteristics[0],
                                                                         sectionsParams.Tapered ? sectionsParams.AtTheEnd.ComponentsCharacteristics[0] : null,
                                                                         unitSystem);
                    tabSection2.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    tabSection2.Visibility = System.Windows.Visibility.Collapsed;
                    section2Parameters.ItemsSource = null;
                }
            }
            else
            {
                tabSection2.Visibility = System.Windows.Visibility.Collapsed;
                section2Parameters.ItemsSource = null;
            }
        }

        private List<SectionParameterDescription> CreateItemSource(int componentNo,
                                                                   SectionDimensions dimensionsAtTheBeg, 
                                                                   SectionDimensions dimensionsAtTheEnd,
                                                                   SectionCharacteristics characteristicsAtTheBeg,
                                                                   SectionCharacteristics characteristicsAtTheEnd,
                                                                   Autodesk.Revit.DB.DisplayUnit? unitSystem)
        {
            List<SectionParameterDescription> secParDescrList = new List<SectionParameterDescription>();

            foreach (object oBeg in dimensionsAtTheBeg)
            {
                System.Reflection.PropertyInfo piBeg = oBeg as System.Reflection.PropertyInfo;
                string name = piBeg.Name;

                double valueAtTheBeg = (double)piBeg.GetValue(dimensionsAtTheBeg, null);
                if (Math.Abs(valueAtTheBeg) < double.Epsilon)
                    continue;

                double valueAtTheEnd = valueAtTheBeg;

                string unit;
                if (null == unitSystem)
                {
                    unit = UnitsAssignment.GetUnitSymbol(name, SectionDimensionsUnits.Assignments);
                }
                else
                {
                    unit = UnitsAssignment.GetUnitSymbol(name, SectionDimensionsUnits.Assignments, (Autodesk.Revit.DB.DisplayUnit)unitSystem);
                }

                if (dimensionsAtTheEnd != null)
                {
                    foreach (object oEnd in dimensionsAtTheEnd)
                    {
                        System.Reflection.PropertyInfo piEnd = oEnd as System.Reflection.PropertyInfo;
                        if (name.CompareTo(piEnd.Name) == 0)
                        {
                            valueAtTheEnd = (double)piBeg.GetValue(dimensionsAtTheEnd, null);
                            break;
                        }
                    }
                }

                string stringValueAtTheBeg = valueAtTheBeg.ToString();
                string stringValueAtTheEnd = valueAtTheEnd.ToString();

                if (null == unitSystem)
                {
                    stringValueAtTheBeg = UnitsAssignment.FormatToRevitUI(name, valueAtTheBeg, SectionDimensionsUnits.Assignments);
                    stringValueAtTheEnd = UnitsAssignment.FormatToRevitUI(name, valueAtTheEnd, SectionDimensionsUnits.Assignments);
                }

                if (componentNo > 0)
                {
                    name += componentNo.ToString();
                }
                
                SectionParameterDescription secParDescr = new SectionParameterDescription(name, stringValueAtTheBeg, stringValueAtTheEnd, unit);
                secParDescrList.Add(secParDescr);
            }

            foreach (object oBeg in characteristicsAtTheBeg)
            {
                System.Reflection.PropertyInfo piBeg = oBeg as System.Reflection.PropertyInfo;
                string name = piBeg.Name;

                double valueAtTheBeg = (double)piBeg.GetValue(characteristicsAtTheBeg, null);
                if (Math.Abs(valueAtTheBeg) < double.Epsilon)
                    continue;

                double valueAtTheEnd = valueAtTheBeg;

                string unit;
                if (null == unitSystem)
                {
                    unit = UnitsAssignment.GetUnitSymbol(name, SectionCharacteristicsUnits.Assignments);
                }
                else
                {
                    unit = UnitsAssignment.GetUnitSymbol(name, SectionCharacteristicsUnits.Assignments, (Autodesk.Revit.DB.DisplayUnit)unitSystem);
                }

                if (characteristicsAtTheEnd != null)
                {
                    foreach (object oEnd in characteristicsAtTheEnd)
                    {
                        System.Reflection.PropertyInfo piEnd = oEnd as System.Reflection.PropertyInfo;
                        if (name.CompareTo(piEnd.Name) == 0)
                        {
                            valueAtTheEnd = (double)piBeg.GetValue(characteristicsAtTheEnd, null);
                            break;
                        }
                    }
                }

                string stringValueAtTheBeg = valueAtTheBeg.ToString();
                string stringValueAtTheEnd = valueAtTheEnd.ToString();

                if (null == unitSystem)
                {
                    stringValueAtTheBeg = UnitsAssignment.FormatToRevitUI(name, valueAtTheBeg, SectionCharacteristicsUnits.Assignments);
                    stringValueAtTheEnd = UnitsAssignment.FormatToRevitUI(name, valueAtTheEnd, SectionCharacteristicsUnits.Assignments);
                }

                if (componentNo > 0)
                {
                    name += componentNo.ToString();
                }

                SectionParameterDescription secParDescr = new SectionParameterDescription(name, stringValueAtTheBeg, stringValueAtTheEnd, unit);
                secParDescrList.Add(secParDescr);
            }

            return secParDescrList;
        }
    }

    class SectionParameterDescription
    {
        public SectionParameterDescription(string name, string valueAtTheBeg, string valueAtTheEnd, string unit)
        {
            Name = name;
            ValueAtTheBeg = valueAtTheBeg;
            ValueAtTheEnd = valueAtTheEnd;
            Unit = unit;
        }

        public string Name { get; private set; }
        public string ValueAtTheBeg { get; private set; }
        public string ValueAtTheEnd { get; private set; }
        public string Unit { get; private set; }
    }
}
