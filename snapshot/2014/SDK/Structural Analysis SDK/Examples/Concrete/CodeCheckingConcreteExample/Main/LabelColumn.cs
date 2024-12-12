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
using CodeCheckingConcreteExample.ConcreteTypes;
using Autodesk.Revit.DB.ExtensibleStorage.Framework;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework;

namespace CodeCheckingConcreteExample.Main
{
   /// <summary>
   /// Values validator for column label.
   /// </summary>
   public class ValueValidatorColumn : IFieldValidator
   {
      /// <summary>
      /// Validates the value.
      /// </summary>
      /// <param name="entity">The entity.</param>
      /// <param name="field">The field.</param>
      /// <param name="value">The value.</param>
      /// <param name="unit">The unit.</param>
      /// <returns></returns>
      public bool ValidateValue(object entity, string field, object value, DisplayUnitType unit)
      {
         if (field == "CreepCoefficient")
         {
            return (double)value > 0;
         }

         if (field == "LengthCoefficientY" || field == "LengthCoefficientZ")
         {
            return (double)value >= 0;
         }

         return true;
      }
   }

   /// <summary>
   /// Container for RC column element material and calculation options
   /// </summary>
   [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("LabelColumn", "68099b9b-ed86-443a-859c-49cb40842a50")]
   [Categories(new string[] { "CalculationOptions", "Buckling", "LongitudinalReinforcement", "TransversalReinforcement" }, new int[] { 1, 2, 3, 4 })]
   public class LabelColumn : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
   {
      /// <summary>
      /// Collection of simple calculation type objects
      /// representing calculation options chosen by the user
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "ColumnCalculationType")]
      [EnumControl(Description = "EnabledInternalForces", Category = "CalculationOptions", EnumType = typeof(ConcreteTypes.EnabledInternalForces), Presentation = PresentationMode.OptionList, Item = PresentationItem.ImageWithText, ImageSize = ImageSize.Medium, Context = "ColumnLabel")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ListValue(Name = "EnabledInternalForces", Index = 1, Localizable = true, LocalizableValue = true)]
      public List<ConcreteTypes.EnabledInternalForces> EnabledInternalForces { get; set; }

      /// <summary>
      /// Creep coefficient
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "CreepCoefficient", Unit = Autodesk.Revit.DB.UnitType.UT_Number, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_GENERAL)]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.TextBox(Description = "CreepCoefficient", Category = "CalculationOptions", IsVisible = true, IsEnabled = true, Index = -1, Localizable = true, FieldValidator = typeof(ValueValidatorColumn))]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "CreepCoefficient", Index = 2, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public Double CreepCoefficient { get; set; }

      /// <summary>
      /// Take into account buckling on direction Y when calculating element
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "BucklingDirectionY")]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.CheckBox(Category = "Buckling", Description = "BucklingDirectionY", IsVisible = true, IsEnabled = true, Index = 2, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "BucklingDirectionY", Index = 3, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public bool BucklingDirectionY { get; set; }

      /// <summary>
      /// Value of the buckling coefficietn on direction Y
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "LengthCoefficientY", Unit = Autodesk.Revit.DB.UnitType.UT_Number, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_GENERAL)]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.TextBox(Category = "Buckling", Description = "LengthCoefficientY", IsVisible = true, IsEnabled = true, Index = 3, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "LengthCoefficientY", Index = 4, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public Double LengthCoefficientY { get; set; }


      /// <summary>
      /// Structure type on direction Y: Sway or No-sway
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "ColumnStructureTypeY")]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.EnumControl(EnumType = typeof(ConcreteTypes.ColumnStructureType), Presentation = Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.PresentationMode.Combobox, Item = Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.PresentationItem.Text, Category = "Buckling", Description = "ColumnStructureType", IsVisible = true, IsEnabled = true, Index = 4, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "ColumnStructureType", Index = 5, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public ConcreteTypes.ColumnStructureType ColumnStructureTypeY { get; set; }

      /// <summary>
      /// Take into account buckling on direction Z when calculating element
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "BucklingDirectionZ")]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.CheckBox(Category = "Buckling", Description = "BucklingDirectionZ", IsVisible = true, IsEnabled = true, Index = 5, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "BucklingDirectionZ", Index = 6, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public bool BucklingDirectionZ { get; set; }

      /// <summary>
      /// Value of the buckling coefficietn on direction Z
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "LengthCoefficientZ", Unit = Autodesk.Revit.DB.UnitType.UT_Number, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_GENERAL)]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.TextBox(Category = "Buckling", Description = "LengthCoefficientZ", IsVisible = true, IsEnabled = true, Index = 6, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "LengthCoefficientZ", Index = 7, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public Double LengthCoefficientZ { get; set; }

      /// <summary>
      /// Structure type on direction Z: Sway or No-sway
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "ColumnStructureTypeZ")]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.EnumControl(EnumType = typeof(ConcreteTypes.ColumnStructureType), Presentation = Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.PresentationMode.Combobox, Item = Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.PresentationItem.Text, Category = "Buckling", Description = "ColumnStructureType", IsVisible = true, IsEnabled = true, Index = 7, Localizable = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "ColumnStructureType", Index = 8, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public ConcreteTypes.ColumnStructureType ColumnStructureTypeZ { get; set; }

      /// <summary>
      /// Rebar parameters component for longitudinal reinforcement
      ///</summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "LongitudinalReinforcement")]
      [CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParameters(Category = "LongitudinalReinforcement")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.SubSchemaElement(LocalizableValue = true, Name = "LongitudinalReinforcement", Index = 9, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema LongitudinalReinforcement { get; set; }

      /// <summary>
      /// Rebar parameters component for transversal reinforcement
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "TransversalReinforcement")]
      [CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParameters(Category = "TransversalReinforcement")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.SubSchemaElement(LocalizableValue = true, Name = "TransversalReinforcement", Index = 10, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema TransversalReinforcement { get; set; }

      /// <summary>
      /// Creates default LabelColumn
      /// </summary>
      public LabelColumn()
      {
         LongitudinalReinforcement = new CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema();
         CreepCoefficient = 2.0;
         TransversalReinforcement = new CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema();
         EnabledInternalForces = new List<ConcreteTypes.EnabledInternalForces>();
         EnabledInternalForces.Add(ConcreteTypes.EnabledInternalForces.MY);
         EnabledInternalForces.Add(ConcreteTypes.EnabledInternalForces.FX);
         LengthCoefficientY = 1.0;
         LengthCoefficientZ = 1.0;
         BucklingDirectionY = true;
         BucklingDirectionZ = true;
         ColumnStructureTypeY = ConcreteTypes.ColumnStructureType.NonSway;
         ColumnStructureTypeZ = ConcreteTypes.ColumnStructureType.NonSway;
      }

      /// <summary>
      /// Transversal calculation type ( compression, bending, excentric bending....)
      /// </summary>
      public ConcreteTypes.CalculationType TransversalCalculationType
      {
         get { return EnabledInternalForces.GetTransversalCalculationType(); }
      }

      /// <summary>
      /// Longitudinal calculation type (shearing, tortion, ...)
      /// </summary>
      public ConcreteTypes.CalculationType LongitudinalCalculationType
      {
         get { return EnabledInternalForces.GetLongitudinalCalculationType(); }
      }
   }
}
