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
using System.Data;
using System.Text;
using System.Collections;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

using Material = Autodesk.Revit.DB.Material;

namespace Revit.SDK.Samples.MaterialProperties.CS
{
    /// <summary>
    /// get the material physical properties of the selected beam, column or brace
    /// get all material types and their sub types to the user and then change the material type of the selected beam to the one chosen by the user
    /// with a selected concrete beam, column or brace, change its unit weight to 145 P/ft3
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class MaterialProperties : Autodesk.Revit.UI.IExternalCommand
    {
        const double ToMetricUnitWeight = 0.010764;            //coefficient of converting unit weight from internal unit to metric unit 
        const double ToMetricStress = 0.334554;            //coefficient of converting stress from internal unit to metric unit
        const double ToImperialUnitWeight = 6.365827;            //coefficient of converting unit weight from internal unit to imperial unit
        const double ChangedUnitWeight = 14.5;                //the value of unit weight of selected component to be set

        Autodesk.Revit.UI.UIApplication m_revit = null;
        Hashtable m_allMaterialMap = new Hashtable();    //hashtable contains all materials with index of their ElementId
        FamilyInstance m_selectedComponent = null;                //selected beam, column or brace
        Parameter m_currentMaterial = null;                //current material of selected beam, column or brace
        Material m_cacheMaterial;
        ArrayList m_steels = new ArrayList();    //arraylist of all materials belonging to steel type
        ArrayList m_concretes = new ArrayList();    //arraylist of all materials belonging to concrete type


        /// <summary>
        /// get the material type of selected element
        /// </summary>
        public StructuralAssetClass CurrentType
        {
            get
            {
                Material material = CurrentMaterial as Material;
                ElementId materialId = new ElementId(0);
                if (material != null)
                {
                    materialId = material.Id;
                }
                if (materialId.IntegerValue <= 0)
                {
                   return StructuralAssetClass.Generic;
                }
                Autodesk.Revit.DB.Material materialElem = (Autodesk.Revit.DB.Material)m_allMaterialMap[materialId];
                if (null == materialElem)
                {
                   return StructuralAssetClass.Generic;
                }
                return GetMaterialType(materialElem);
            }
        }

        /// <summary>
        /// get the material attribute of selected element
        /// </summary>
        public object CurrentMaterial
        {
            get
            {
                m_cacheMaterial = GetCurrentMaterial();
                return m_cacheMaterial;
            }
        }

        /// <summary>
        /// arraylist of all materials belonging to steel type
        /// </summary>
        public ArrayList SteelCollection
        {
            get
            {
                return m_steels;
            }
        }

        /// <summary>
        /// arraylist of all materials belonging to concrete type
        /// </summary>
        public ArrayList ConcreteCollection
        {
            get
            {
                return m_concretes;
            }
        }

        /// <summary>
        /// three basic material types in Revit
        /// </summary>
        public ArrayList MaterialTypes
        {
            get
            {
                ArrayList typeAL = new ArrayList();
                typeAL.Add("Undefined");
                typeAL.Add("Basic");
                typeAL.Add("Generic");
                typeAL.Add("Metal");
                typeAL.Add("Concrete");
                typeAL.Add("Wood");
                typeAL.Add("Liquid");
                typeAL.Add("Gas");
                typeAL.Add("Plastic");
                return typeAL;
            }
        }

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
            ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Autodesk.Revit.UI.UIApplication revit = commandData.Application;
            m_revit = revit;
            if (!Init())
            {
                // there must be exactly one beam, column or brace selected
                TaskDialog.Show("Revit", "You should select only one beam, structural column or brace.");
                return Autodesk.Revit.UI.Result.Failed;
            }

            Transaction documentTransaction = new Transaction(commandData.Application.ActiveUIDocument.Document, "Document");
            documentTransaction.Start();
            MaterialPropertiesForm displayForm = new MaterialPropertiesForm(this);
            try
            {
                displayForm.ShowDialog();
            }
            catch
            {
                TaskDialog.Show("Revit", "Sorry that your command failed.");
                return Autodesk.Revit.UI.Result.Failed;
            }
            documentTransaction.Commit();
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// get a datatable contains parameters' information of certain element
        /// </summary>
        /// <param name="o">Revit element</param>
        /// <param name="substanceKind">the material type of this element</param>
        /// <returns>datatable contains parameters' names and values</returns>
        public DataTable GetParameterTable(object o, StructuralAssetClass substanceKind)
        {
            //create an empty datatable
            DataTable parameterTable = CreateTable();
            //if failed to convert object
            Autodesk.Revit.DB.Material material = o as Autodesk.Revit.DB.Material;
            if (material == null)
            {
                return parameterTable;
            }

            Parameter temporaryAttribute = null;        // hold each parameter
            string temporaryValue = "";                    // hold each value

            #region Get all material element parameters

            //- Behavior
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_BEHAVIOR);
            switch (temporaryAttribute.AsInteger())
            {
                case 0:
                    AddDataRow(temporaryAttribute.Definition.Name, "Isotropic", parameterTable);
                    break;
                case 1:
                    AddDataRow(temporaryAttribute.Definition.Name, "Orthotropic", parameterTable);
                    break;
                default:
                    AddDataRow(temporaryAttribute.Definition.Name, "None", parameterTable);
                    break;
            }
            //- Young's Modulus
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD1);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD2);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD3);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);

            // - Poisson Modulus
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD1);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD2);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD3);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);

            // - Shear Modulus
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD1);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD2);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD3);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);

            //- Thermal Expansion Coefficient
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF1);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF2);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF3);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);

            //- Unit Weight
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_UNIT_WEIGHT);
            temporaryValue = temporaryAttribute.AsValueString();
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);

            //- Bending Reinforcement
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_BENDING_REINFORCEMENT);
            if (null != temporaryAttribute)
            {
                temporaryValue = temporaryAttribute.AsValueString();
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            }

            //- Shear Reinforcement
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_REINFORCEMENT);
            if (null != temporaryAttribute)
            {
                temporaryValue = temporaryAttribute.AsValueString();
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            }

            //- Resistance Calc Strength
            temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_RESISTANCE_CALC_STRENGTH);
            if (null != temporaryAttribute)
            {
                temporaryValue = temporaryAttribute.AsValueString();
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            }

            // For Steel only: 
            if (StructuralAssetClass.Metal == substanceKind)
            {
                //- Minimum Yield Stress
                temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_MINIMUM_YIELD_STRESS);
                temporaryValue = temporaryAttribute.AsValueString();
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);

                //- Minimum Tensile Strength
                temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_MINIMUM_TENSILE_STRENGTH);
                temporaryValue = temporaryAttribute.AsValueString();
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);

                //- Reduction Factor
                temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_REDUCTION_FACTOR);
                temporaryValue = temporaryAttribute.AsValueString();
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            }

            // For Concrete only:
            if (StructuralAssetClass.Concrete == substanceKind)
            {
                //- Concrete Compression     
                temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_CONCRETE_COMPRESSION);
                temporaryValue = temporaryAttribute.AsValueString();
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);

                //- Lightweight
                temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_LIGHT_WEIGHT);
                temporaryValue = temporaryAttribute.AsValueString();
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);

                //- Shear Strength Reduction
                temporaryAttribute = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_STRENGTH_REDUCTION);
                temporaryValue = temporaryAttribute.AsValueString();
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable);
            }
            #endregion
            return parameterTable;
        }

        /// <summary>
        /// Update cache material 
        /// </summary>
        /// <param name="obj">new material</param>
        public void UpdateMaterial(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException();
            }
            {
                m_cacheMaterial = obj as Material;
            }
        }

        /// <summary>
        /// set the material of selected component
        /// </summary>
        public void SetMaterial()
        {
            if (null == m_cacheMaterial || null == m_currentMaterial)
            {
                return;
            }

            Autodesk.Revit.DB.ElementId identity = m_cacheMaterial.Id;
            m_currentMaterial.Set(identity);
        }

        /// <summary>
        /// change unit weight of selected component to 14.50 kN/m3
        /// </summary>
        public bool ChangeUnitWeight()
        {
            Autodesk.Revit.DB.Material material = GetCurrentMaterial();
            if (material == null)
            {
                return false;
            }

            Parameter weightPara = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_UNIT_WEIGHT);

            weightPara.Set(ChangedUnitWeight / ToMetricUnitWeight);

            return true;
        }

        /// <summary>
        /// firstly, check whether only one beam, column or brace is selected
        /// then initialize some member variables
        /// </summary>
        /// <returns>is the initialize successful</returns>
        private bool Init()
        {
            //selected 0 or more than 1 component
            if (m_revit.ActiveUIDocument.Selection.GetElementIds().Count != 1)
            {
                return false;
            }

            try
            {
                GetSelectedComponent();
                //selected component isn't beam, column or brace
                if (m_selectedComponent == null)
                {
                    return false;
                }

                //initialize some member variables
                GetAllMaterial();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// get current material of selected component
        /// </summary>
        private Autodesk.Revit.DB.Material GetCurrentMaterial()
        {
            if (null != m_cacheMaterial)
                return m_cacheMaterial;

            ElementId identityValue = new ElementId(0);
            if (m_currentMaterial != null)
                identityValue = m_currentMaterial.AsElementId();    //get the value of current material's ElementId
            //material has no value
            if (identityValue.IntegerValue <= 0)
            {
                return null;
            }
            Autodesk.Revit.DB.Material material = (Autodesk.Revit.DB.Material)m_allMaterialMap[identityValue];

            return material;
        }

        /// <summary>
        /// get selected beam, column or brace
        /// </summary>
        /// <returns></returns>
        private void GetSelectedComponent()
        {
           ElementSet componentCollection = new ElementSet();
            foreach (ElementId elementId in m_revit.ActiveUIDocument.Selection.GetElementIds())
            {
               componentCollection.Insert(m_revit.ActiveUIDocument.Document.GetElement(elementId));
            }

            if (componentCollection.Size != 1)
            {
                return;
            }

            //if the selection is a beam, column or brace, find out its parameters for display
            foreach (object o in componentCollection)
            {
                FamilyInstance component = o as FamilyInstance;
                if (component == null)
                {
                    continue;
                }

                if (component.StructuralType == StructuralType.Beam
                    || component.StructuralType == StructuralType.Brace
                    || component.StructuralType == StructuralType.Column)
                {
                    //get selected beam, column or brace
                    m_selectedComponent = component;
                }

                //selection is a beam, column or brace, find out its parameters
                foreach (object p in component.Parameters)
                {
                    Parameter attribute = p as Parameter;
                    if (attribute == null)
                    {
                        continue;
                    }

                    string parameterName = attribute.Definition.Name;
                    // The "Beam Material" and "Column Material" family parameters have been replaced
                    // by the built-in parameter "Structural Material".
                    //if (parameterName == "Column Material" || parameterName == "Beam Material")
                    if (parameterName == "Structural Material")
                    {
                        //get current material of selected component
                        m_currentMaterial = attribute;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// get all materials exist in current document
        /// </summary>
        /// <returns></returns>
        private void GetAllMaterial()
        {
            FilteredElementCollector collector = new FilteredElementCollector(m_revit.ActiveUIDocument.Document);
            FilteredElementIterator i = collector.OfClass(typeof(Material)).GetElementIterator();
            i.Reset();
            bool moreValue = i.MoveNext();
            while (moreValue)
            {
                Autodesk.Revit.DB.Material material = i.Current as Autodesk.Revit.DB.Material;
                if (material == null)
                {
                    moreValue = i.MoveNext();
                    continue;
                }
                //get the type of the material
                StructuralAssetClass materialType = GetMaterialType(material);

                //add materials to different ArrayList according to their types
                switch (materialType)
                {
                   case StructuralAssetClass.Metal:
                        {
                            m_steels.Add(new MaterialMap(material));
                            break;
                        }
                   case StructuralAssetClass.Concrete:
                        {
                            m_concretes.Add(new MaterialMap(material));
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                //map between materials and their elementId
                m_allMaterialMap.Add(material.Id, material);
                moreValue = i.MoveNext();
            }
        }

        /// <summary>
        /// Create an empty table with parameter's name column and value column
        /// </summary>
        /// <returns></returns>
        private DataTable CreateTable()
        {
            // Create a new DataTable.
            DataTable propDataTable = new DataTable("ParameterTable");

            // Create parameter column and add to the DataTable.
            DataColumn paraDataColumn = new DataColumn();
            paraDataColumn.DataType = System.Type.GetType("System.String");
            paraDataColumn.ColumnName = "Parameter";
            paraDataColumn.Caption = "Parameter";
            paraDataColumn.ReadOnly = true;
            // Add the column to the DataColumnCollection.
            propDataTable.Columns.Add(paraDataColumn);

            // Create value column and add to the DataTable.
            DataColumn valueDataColumn = new DataColumn();
            valueDataColumn.DataType = System.Type.GetType("System.String");
            valueDataColumn.ColumnName = "Value";
            valueDataColumn.Caption = "Value";
            valueDataColumn.ReadOnly = false;
            propDataTable.Columns.Add(valueDataColumn);

            return propDataTable;
        }

        /// <summary>
        /// add one row to datatable of parameter
        /// </summary>
        /// <param name="parameterName">name of parameter</param>
        /// <param name="parameterValue">value of parameter</param>
        /// <param name="parameterTable">datatable to be added row</param>
        private void AddDataRow(string parameterName, string parameterValue, DataTable parameterTable)
        {
            DataRow newRow = parameterTable.NewRow();
            newRow["Parameter"] = parameterName;
            newRow["Value"] = parameterValue;
            parameterTable.Rows.Add(newRow);
        }

        /// <summary>
        /// Get the material type via giving material.
        /// According to my knowledge, the material type can be retrieved by two ways now:
        /// 1. If the PropertySetElement exists, retrieve it by PHY_MATERIAL_PARAM_CLASS parameter. (via PropertySetElement class)
        /// 2. If it's indenpendent, retrieve it by PHY_MATERIAL_PARAM_TYPE parameter(via Material class)
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        private StructuralAssetClass GetMaterialType(Material material)
        {
           if (material.StructuralAssetId != ElementId.InvalidElementId)
           {
              PropertySetElement propElem = m_revit.ActiveUIDocument.Document.GetElement(material.StructuralAssetId) as PropertySetElement;
              Parameter propElemPara = propElem.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_CLASS);
              if (propElemPara != null)
              {
                 return (StructuralAssetClass)propElemPara.AsInteger();
              }
           }
           return StructuralAssetClass.Undefined;
            //ElementId propElemId = material.GetMaterialAspectPropertySet(MaterialAspect.Structural);
            //if (ElementId.InvalidElementId == propElemId)
            //{
            //    Parameter independentPara = material.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_TYPE);
            //    if (null == independentPara)
            //    {
            //        return MaterialType.Generic;
            //    }
            //    return (MaterialType)independentPara.AsInteger();
            //}
            //PropertySetElement propElem = m_revit.ActiveUIDocument.Document.GetElement(propElemId) as PropertySetElement;
            //Parameter propElemPara = propElem.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_CLASS);
            //if (null == propElemPara)
            //{
            //    return MaterialType.Generic;
            //}
            //return (MaterialType)propElemPara.AsInteger();
        }
    }


    /// <summary>
    /// assistant class contains material and its name
    /// </summary>
    public class MaterialMap
    {
        string m_materialName;
        Autodesk.Revit.DB.Material m_material;

        /// <summary>
        /// constructor without parameter is forbidden
        /// </summary>
        private MaterialMap() { }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="material"></param>
        public MaterialMap(Autodesk.Revit.DB.Material material)
        {
            m_materialName = material.Name;
            m_material = material;
        }

        /// <summary>
        /// Get the material name
        /// </summary>
        public string MaterialName
        {
            get
            {
                return m_materialName;
            }
        }

        /// <summary>
        /// Get the material
        /// </summary>
        public Autodesk.Revit.DB.Material Material
        {
            get
            {
                return m_material;
            }
        }
    }
}
