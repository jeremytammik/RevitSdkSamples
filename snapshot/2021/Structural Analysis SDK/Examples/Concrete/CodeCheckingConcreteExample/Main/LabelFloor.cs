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
using CT = CodeCheckingConcreteExample.ConcreteTypes;
using Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.ConcreteTypes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework;

/// <structural_toolkit_2015>
namespace CodeCheckingConcreteExample
{
   /// <summary>
   /// Values validator for beam label.
   /// </summary>
   public class ValueValidatorFloor : IFieldValidator
   {
      /// <summary>
      /// Validates the value.
      /// </summary>
      /// <param name="entity">The entity.</param>
      /// <param name="field">The field.</param>
      /// <param name="value">The value.</param>
      /// <param name="unit">The unit.</param>
      /// <returns>Information about data validation.</returns>
      public bool ValidateValue(object entity, string field, object value, DisplayUnitType unit)
      {
         if (field == "CreepCoefficient")
         {
            return (double)value > 0;
         }
         return true;
      }
   }
   /// <summary>
   /// Container for RC floor and slab foundation elements material and calculation options
   /// </summary>
   [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("LabelFloor", "7606E4E9-7A81-4975-A231-0052599F6939")]
   public class LabelFloor : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
   {

      /// <summary>
      /// Classification of available forces for column
      /// </summary>
      public enum EnabledInternalForcesForFloor
      {
         /// <summary>
         /// Axial force. The force acting along the element.
         /// </summary>
         FX = ConcreteTypes.EnabledInternalForces.FX,
         /// <summary>
         /// Bending moment. Bending around the Y axis.
         /// </summary>
         MY = ConcreteTypes.EnabledInternalForces.MY,
      }

      /// <summary>
      /// Collection of beam simple calculation 
      /// representing calculation options chosen by the user
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "FloorCalculationType")]
      [EnumControl(Description = "EnabledInternalForces", Category = "CalculationOptions", EnumType = typeof(EnabledInternalForcesForFloor), Presentation = PresentationMode.OptionList, Item = PresentationItem.ImageWithText, ImageSize = ImageSize.Medium, Context = "FloorLabel")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ListValue(Name = "EnabledInternalForces", Index = 1, Localizable=true, LocalizableValue = true)]
      public List<EnabledInternalForcesForFloor> EnabledInternalForcesFloor { get; set; }

      /// <summary>
      /// Rebar parameters component for primary reinforcement
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "PrimaryRnfSteelParameters")]
      [CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParameters(Category = "PrimaryRnfSteelParameters")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "PrimaryRnfSteelParameters", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema PrimaryReinforcement { get; set; }

      /// <summary>
      /// Rebar parameters component for secondary reinforcement
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "SecondaryRnfSteelParameters")]
      [CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParameters(Category = "SecondaryRnfSteelParameters")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "SecondaryRnfSteelParameters", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema SecondaryReinforcement { get; set; }

      /// <summary>
      /// Creep coefficient
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "CreepCoefficient", Unit = Autodesk.Revit.DB.UnitType.UT_Number, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_GENERAL)]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.TextBox(Description = "CreepCoefficient", Category = "CalculationOptions", IsVisible = true, IsEnabled = true, Index = -1, Localizable = true, FieldValidator = typeof(ValueValidatorFloor))]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "CreepCoefficient", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public Double CreepCoefficient { get; set; }

      /// <summary>
      /// Longitudinal calculation type (shearing, tortion, ...)
      /// </summary>
      public ConcreteTypes.CalculationType LongitudinalCalculationType { get { return EnabledInternalForces.GetLongitudinalCalculationType(); } }

      /// <summary>
      /// Returns EnabledInternalForces based on EnabledInternalForcesFloor
      /// </summary>
      public List<ConcreteTypes.EnabledInternalForces> EnabledInternalForces
      {
         get
         {
            List<ConcreteTypes.EnabledInternalForces> EInternalForces = new List<ConcreteTypes.EnabledInternalForces>();
            foreach (EnabledInternalForcesForFloor enabledInternalForcesFloor in EnabledInternalForcesFloor)
            {
               EInternalForces.Add((ConcreteTypes.EnabledInternalForces)enabledInternalForcesFloor);
            }
            return EInternalForces;
         }
      }
      /// <summary>
      /// Creates default LabelFloor
      /// </summary>
      public LabelFloor()
      {
         PrimaryReinforcement = new CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema();
         SecondaryReinforcement = new CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema();
         CreepCoefficient = 2.0;
         EnabledInternalForcesFloor = new List<EnabledInternalForcesForFloor>();
         EnabledInternalForcesFloor.Add(EnabledInternalForcesForFloor.MY);
      }
   }
}
/// </structural_toolkit_2015>
