//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.PhysicalProp.CS
{
    /// <summary>
    /// Define a command to dump physical material properties of 
    /// a structural element such as column, beam or brace. 
    /// </summary>
    public class DumpMaterialPhysicalParameters : Autodesk.Revit.IExternalCommand
    {
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
        public Autodesk.Revit.IExternalCommand.Result Execute(ExternalCommandData cmdData,
            ref string msg, Autodesk.Revit.ElementSet eleSet)
        {
            IExternalCommand.Result res = IExternalCommand.Result.Succeeded;
            try
            {
                Document activeDoc = cmdData.Application.ActiveDocument;
                string str;

                Material materialElement = null;

                SelElementSet selection = activeDoc.Selection.Elements;
                if (selection.Size != 1)
                {
                    msg = "Please select only one element.";
                    res = IExternalCommand.Result.Failed;
                    return res;
                }

                System.Collections.IEnumerator iter;
                iter = activeDoc.Selection.Elements.ForwardIterator();
                iter.MoveNext();

                // we need verify the selected element is a family instance
                FamilyInstance famIns = iter.Current as FamilyInstance;
                if (famIns == null)
                {
                    MessageBox.Show("Not a type of FamilyInsance!");
                    res = IExternalCommand.Result.Failed;
                    return res;
                }

                // we need select a column instance
                foreach (Parameter p in famIns.Parameters)
                {
                    string parName = p.Definition.Name;
                    if (parName == "Column Material" || parName == "Beam Material")
                    {
                        Autodesk.Revit.ElementId elemId = p.AsElementId();
                        materialElement = activeDoc.get_Element(ref elemId) as Material;
                        break;
                    }
                }

                if (materialElement == null)
                {
                    MessageBox.Show("Not a column!");
                    res = IExternalCommand.Result.Failed;
                    return res;
                }

                // the PHY_MATERIAL_PARAM_TYPE built in parameter contains a number 
                // that represents the type of material
                Autodesk.Revit.Parameter materialType =
                    materialElement.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_TYPE);

                str = "Material type: " +
                    (materialType.AsInteger() == 0 ?
                    "Generic" : (materialType.AsInteger() == 1 ? "Concrete" : "Steel")) + "\n";

                // A material type of more than 0 : 0 = Generic, 1 = Concrete, 2 = Steel
                if (materialType.AsInteger() > 0)
                {
                    // Common to all types

                    // Young's Modulus
                    double[] youngsModulus = new double[3];

                    youngsModulus[0] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD1).AsDouble();
                    youngsModulus[1] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD2).AsDouble();
                    youngsModulus[2] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD3).AsDouble();
                    str = str + "Young's modulus: " + youngsModulus[0].ToString() + 
                        "," + youngsModulus[1].ToString() + "," + youngsModulus[2].ToString() + 
                        "\n";

                    // Poisson Modulus
                    double[] poissonModulus = new double[3];

                    poissonModulus[0] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD1).AsDouble();
                    poissonModulus[1] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD2).AsDouble();
                    poissonModulus[2] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD3).AsDouble();
                    str = str + "Poisson modulus: " + poissonModulus[0].ToString() + 
                        "," + poissonModulus[1].ToString() + "," + poissonModulus[2].ToString() + 
                        "\n";

                    // Shear Modulus
                    double[] shearModulus = new double[3];

                    shearModulus[0] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD1).AsDouble();
                    shearModulus[1] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD2).AsDouble();
                    shearModulus[2] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD3).AsDouble();
                    str = str + "Shear modulus: " + shearModulus[0].ToString() + 
                        "," + shearModulus[1].ToString() + "," + shearModulus[2].ToString() + "\n";

                    // Thermal Expansion Coefficient
                    double[] thermalExpCoeff = new double[3];

                    thermalExpCoeff[0] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF1).AsDouble();
                    thermalExpCoeff[1] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF2).AsDouble();
                    thermalExpCoeff[2] = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF3).AsDouble();
                    str = str + "Thermal expansion coefficient: " + thermalExpCoeff[0].ToString() +
                        "," + thermalExpCoeff[1].ToString() + "," + thermalExpCoeff[2].ToString() +
                        "\n";

                    // Unit Weight
                    double unitWeight;
                    unitWeight = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_UNIT_WEIGHT).AsDouble();
                    str = str + "Unit weight: " + unitWeight.ToString() + "\n";

                    // Damping Ratio
                    double dampingRatio;
                    dampingRatio = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_DAMPING_RATIO).AsDouble();
                    str = str + "Damping ratio: " + dampingRatio.ToString() + "\n";

                    // Behavior 0 = Isotropic, 1 = Orthotropic
                    int behaviour;
                    behaviour = materialElement.get_Parameter(
                        BuiltInParameter.PHY_MATERIAL_PARAM_BEHAVIOR).AsInteger();
                    str = str + "Behavior: " + behaviour.ToString() + "\n";

                    // Concrete Only
                    if (materialType.AsInteger() == 1)
                    {
                        // Concrete Compression
                        double concreteCompression;
                        concreteCompression = materialElement.get_Parameter(
                            BuiltInParameter.PHY_MATERIAL_PARAM_CONCRETE_COMPRESSION).AsDouble();
                        str = str + "Concrete compression: " + concreteCompression.ToString() + "\n";

                        // Lightweight
                        double lightWeight;
                        lightWeight = materialElement.get_Parameter(
                            BuiltInParameter.PHY_MATERIAL_PARAM_LIGHT_WEIGHT).AsDouble();
                        str = str + "Lightweight: " + lightWeight.ToString() + "\n";

                        // Shear Strength Reduction
                        double shearStrengthReduction;
                        shearStrengthReduction = materialElement.get_Parameter(
                            BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_STRENGTH_REDUCTION).AsDouble();
                        str = str + "Shear strength reduction: " + shearStrengthReduction.ToString() + "\n";
                    }
                    // Steel only
                    else if (materialType.AsInteger() == 2)
                    {
                        // Minimum Yield Stress
                        double minimumYieldStress;
                        minimumYieldStress = materialElement.get_Parameter(
                            BuiltInParameter.PHY_MATERIAL_PARAM_MINIMUM_YIELD_STRESS).AsDouble();
                        str = str + "Minimum yield stress: " + minimumYieldStress.ToString() + "\n";

                        // Minimum Tensile Strength
                        double minimumTensileStrength;
                        minimumTensileStrength = materialElement.get_Parameter(
                            BuiltInParameter.PHY_MATERIAL_PARAM_MINIMUM_TENSILE_STRENGTH).AsDouble();
                        str = str + "Minimum tensile strength: " + 
                            minimumTensileStrength.ToString() + "\n";

                        // Reduction Factor
                        double reductionFactor;
                        reductionFactor = materialElement.get_Parameter(
                            BuiltInParameter.PHY_MATERIAL_PARAM_REDUCTION_FACTOR).AsDouble();
                        str = str + "Reduction factor: " + reductionFactor.ToString() + "\n";
                    } // end of if/else materialType.Integer == 1
                } // end if materialType.Integer > 0

                MessageBox.Show(str, "Physical materials");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PhysicalProp");
                res = IExternalCommand.Result.Failed;
            }
            finally
            {
            }

            return res;
        } // end command
    } // end class
}