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
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using CodeCheckingConcreteExample.Properties;

namespace CodeCheckingConcreteExample.UIComponents.RCSteelParameters
{
   /// <summary>
   /// Component for rebar parameters selection
   /// </summary>
   [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("RCSteelParametersSchema", "9afbd3c6-5eff-4712-adce-e8eef1788081")]
   public class RCSteelParametersSchema : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
   {
      Material _Material;
      /// <summary>
      /// Revit ElementId of Revit material
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "Material")]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.ElementComboBox(Description = "Material", Index = 0, IsVisible = true, IsEnabled = true, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "Material", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public Material Material
      {
         get
         {
            return _Material;
         }
         set
         {
            if (value != null && value.MaterialClass == "Metal")
            {
               _Material = value;
               double yield = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(value).MinimumYieldStress;
               MinimumYieldStress = Autodesk.Revit.DB.UnitUtils.ConvertFromInternalUnits(yield, DisplayUnitType.DUT_PASCALS);
            }
            else
            {
               _Material = null;
               MinimumYieldStress = 0.0;
            }
         }
      }

      /// <summary>
      /// Steel yield stress
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Unit(Unit = Autodesk.Revit.DB.UnitType.UT_Stress, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_NEWTONS_PER_SQUARE_METER)]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.UnitTextBox(Description = "MinimumYieldStress", Index = 1, ValidateMinimumValue = false, ValidateMaximumValue = false, AttributeUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_NEWTONS_PER_SQUARE_METER, Category = "Trans reinforcement parameters", IsVisible = true, IsEnabled = false, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "MinimumYieldStress", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public Double MinimumYieldStress { get; set; }

      RebarBarType _RebarBarType;
      /// <summary>
      /// RebarBarType element representing the selected rebar
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "RebarBarType")]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.ElementComboBox(Description = "RebarBarType", Index = 2, IsVisible = true, IsEnabled = true, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "RebarBarType", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public RebarBarType RebarBarType
      {
         get
         {
            return _RebarBarType;
         }
         set
         {
            if (value != null && Material != null && value.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.MATERIAL_ID_PARAM).AsElementId().IntegerValue == Material.Id.IntegerValue)
            {
               _RebarBarType = value;
               BarDiameter = Autodesk.Revit.DB.UnitUtils.ConvertFromInternalUnits(value.BarDiameter, DisplayUnitType.DUT_METERS);
               DeformationType = value.DeformationType == RebarDeformationType.Plain ? Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.ConcreteTypes.SteelSurface.Plain : Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.ConcreteTypes.SteelSurface.Deformed;
            }
            else
            {
               _RebarBarType = null;
               BarDiameter = 0.0;
               DeformationType = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.ConcreteTypes.SteelSurface.Unknown;
            }
         }
      }

      /// <summary>
      /// Steel surface(Ribbed/Plain) for the selected rebar
      /// </summary>
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.TextBox(Description = "DeformationType", Index = 3, IsVisible = true, IsEnabled = false, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "DeformationType", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.ConcreteTypes.SteelSurface DeformationType { get; set; }

      /// <summary>
      /// Rebar diameter
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Unit(Unit = Autodesk.Revit.DB.UnitType.UT_Bar_Diameter, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_METERS)]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.UnitTextBox(Description = "BarDiameter", Index = 4, ValidateMinimumValue = false, ValidateMaximumValue = false, AttributeUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_METERS, IsVisible = true, IsEnabled = false, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "BarDiameter", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public Double BarDiameter { get; set; }



      /// <summary>
      /// Rebar crosssection area
      /// </summary>
      public Double Area { get { return BarDiameter * BarDiameter * System.Math.PI / 4; } }


      /// <summary>
      /// Creates default RCSteelParametersSchema
      /// </summary>
      public RCSteelParametersSchema()
      {
      }

      /// <summary>
      /// Creates default RCSteelParametersSchema
      /// </summary>
      /// <param name="entity"></param>
      /// <param name="document"></param>
      public RCSteelParametersSchema(Entity entity, Document document)
         : base(entity, document)
      {
      }

   }
}
