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
using XYZ = Autodesk.Revit.DB.XYZ;
using Autodesk.Revit.DB.CodeChecking.Engineering;

namespace SectionPropertiesExplorer
{
    /// <summary>
    /// Interaction logic for MaterialParameters.xaml
    /// </summary>
    public partial class MaterialParametersControl : UserControl
    {
        public MaterialParametersControl()
        {
            InitializeComponent();
        }

        public void DoBinding(MaterialInfo materialParams, 
                              MaterialLayerDescription layerDescription, 
                              Autodesk.Revit.DB.DisplayUnit? unitSystem)
        {
            if (layerDescription != null)
            {
                string unitThicknessName = "LayerThickness";
                string unitOffsetName = "LayerOffset";

                string unitThickness;
                string unitOffset;
                if (null == unitSystem)
                {
                    unitThickness = UnitsAssignment.GetUnitSymbol(unitThicknessName, ElementInfoUnits.Assignments);
                    unitOffset = UnitsAssignment.GetUnitSymbol(unitOffsetName, ElementInfoUnits.Assignments);
                }
                else
                {
                    unitThickness = UnitsAssignment.GetUnitSymbol(unitThicknessName, ElementInfoUnits.Assignments, (Autodesk.Revit.DB.DisplayUnit)unitSystem);
                    unitOffset = UnitsAssignment.GetUnitSymbol(unitOffsetName, ElementInfoUnits.Assignments, (Autodesk.Revit.DB.DisplayUnit)unitSystem);
                }

                string stringThicknessValue = layerDescription.LayerThickness.ToString();
                string stringOffsetValue = layerDescription.LayerOffset.ToString();

                if (null == unitSystem)
                {
                    stringThicknessValue = UnitsAssignment.FormatToRevitUI(unitThicknessName, layerDescription.LayerThickness, ElementInfoUnits.Assignments);
                    stringOffsetValue = UnitsAssignment.FormatToRevitUI(unitOffsetName, layerDescription.LayerOffset, ElementInfoUnits.Assignments);
                }

                LayerThickness.Content = stringThicknessValue + " " + unitThickness;
                LayerOffset.Content = stringOffsetValue + " " + unitOffset;

                LayerDescription.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                LayerDescription.Visibility = System.Windows.Visibility.Collapsed;
            }

            List<MaterialParameterDescription> matParDescrList = new List<MaterialParameterDescription>();

            if (materialParams.Category != MaterialCategory.Undefined)
            {
                MaterialName.Content = materialParams.Properties.Name;
                MaterialDescription.Content = materialParams.Properties.Description;

                MaterialCharacteristics materialCharacteristics = materialParams.Characteristics;
                foreach (object o in materialCharacteristics)
                {
                    List<MaterialParameterDescription> matParDescr = ParameterBinding(o, materialCharacteristics, MaterialCharacteristicsUnits.Assignments, unitSystem);
                    if (null == matParDescr)
                        continue;
                    matParDescrList.AddRange(matParDescr);
                }
            }

            switch (materialParams.Category)
            {
                case MaterialCategory.Concrete:
                    MaterialConcreteCharacteristics concrete = (MaterialConcreteCharacteristics)materialParams.Characteristics.Specific;
                    foreach (object o in concrete)
                    {
                        List<MaterialParameterDescription> matParDescr = ParameterBinding(o, concrete, MaterialConcreteCharacteristicsUnits.Assignments, unitSystem);
                        if (matParDescr != null)
                            matParDescrList.AddRange(matParDescr);
                    }
                    break;
                case MaterialCategory.Metal:
                    MaterialMetalCharacteristics metal = (MaterialMetalCharacteristics)materialParams.Characteristics.Specific;
                    foreach (object o in metal)
                    {
                        List<MaterialParameterDescription> matParDescr = ParameterBinding(o, metal, MaterialMetalCharacteristicsUnits.Assignments, unitSystem);
                        if (matParDescr != null)
                            matParDescrList.AddRange(matParDescr);
                    }

                    break;
                case MaterialCategory.Timber:
                    MaterialTimberCharacteristics timber = (MaterialTimberCharacteristics)materialParams.Characteristics.Specific;
                    foreach (object o in timber)
                    {
                        List<MaterialParameterDescription> matParDescr = ParameterBinding(o, timber, MaterialTimberCharacteristicsUnits.Assignments, unitSystem);
                        if (matParDescr != null)
                            matParDescrList.AddRange(matParDescr);
                    }
                    break;
            }

            materialParameters.ItemsSource = matParDescrList;
        }

        private List<MaterialParameterDescription> ParameterBinding(object property, 
                                                                    object propertOwnerObject, 
                                                                    UnitsAssignment[] unitsAssignment, 
                                                                    Autodesk.Revit.DB.DisplayUnit? unitSystem)
        {
            System.Reflection.PropertyInfo pi = property as System.Reflection.PropertyInfo;
            string name = pi.Name;
            object oValue = pi.GetValue(propertOwnerObject, null);

            if (!(oValue is double) && !(oValue is XYZ))
                return null;

            List<MaterialParameterDescription> listMatParDescr = new List<MaterialParameterDescription>();

            int max_i = 3;
            string dirTxt = "";
            for (int i = 0; i < max_i; i++)
            {
                double value = 0.0;

                if (oValue is double)
                {
                    value = (double)oValue;
                    max_i = 1;
                }
                else
                if (oValue is XYZ)
                {
                    switch (i)
	                {
                        case 0:
                            dirTxt = " X";
                            value = (double)(((XYZ)oValue).X);
                            break;
                        case 1:
                            dirTxt = " Y";
                            value = (double)(((XYZ)oValue).Y);
                            break;
                        case 2:
                            dirTxt = " Z";
                            value = (double)(((XYZ)oValue).Z);
                            break;
		            default:
                        break;
	                }
                }
                
                if (Math.Abs(value) < double.Epsilon)
                    continue;

                string unit;
                if (null == unitSystem)
                {
                    unit = UnitsAssignment.GetUnitSymbol(name, unitsAssignment);
                }
                else
                {
                    unit = UnitsAssignment.GetUnitSymbol(name, unitsAssignment, (Autodesk.Revit.DB.DisplayUnit)unitSystem);
                }

                string stringValue = value.ToString();

                if (null == unitSystem)
                {
                    stringValue = UnitsAssignment.FormatToRevitUI(name, value, unitsAssignment);
                }

                listMatParDescr.Add(new MaterialParameterDescription(name + dirTxt, stringValue, unit));
            }

            if(listMatParDescr.Count < 1)
                return null;

            return listMatParDescr;
        }
    }

    public class MaterialLayerDescription
    {
        public MaterialLayerDescription(double layerThickness, double layerOffset)
        {
            LayerOffset = layerOffset;
            LayerThickness = layerThickness;
        }

          public double LayerThickness { get; private set; }
          public double LayerOffset { get; private set; }
    }

    class MaterialParameterDescription
    {
        public MaterialParameterDescription(string name, string value, string unit)
        {
            Name = name;
            Value = value;
            Unit = unit;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
        public string Unit { get; private set; }
    }
}
