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

namespace CodeCheckingConcreteExample
{
   /// <structural_toolkit_2015>

   /// <summary>
   /// Values validator for beam label.
   /// </summary>
   public class ValueValidatorWall : IFieldValidator
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
         return true;
      }
   }
   /// <summary>
   /// Represents labels.
   /// </summary>
   [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("LabelWall", "47899fc7-3396-4c02-b8a1-98d720d540d2")]
   public class LabelWall : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
   {
      /// <summary>
      /// Classification of available forces for column
      /// </summary>
      public enum EnabledInternalForcesForWall
      {
         /// <summary>
         /// Axial force. Fxx and Fyy.
         /// </summary>
         FX = ConcreteTypes.EnabledInternalForces.FX,
         MX = ConcreteTypes.EnabledInternalForces.MY,
      }
      /// <summary>
      /// Collection of beam simple calculation 
      /// representing calculation options chosen by the user
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "WallCalculationType")]
      [EnumControl(Description = "EnabledInternalForces", Category = "CalculationOptions", EnumType = typeof(EnabledInternalForcesForWall), Presentation = PresentationMode.OptionList, Item = PresentationItem.ImageWithText, ImageSize = ImageSize.Medium, Context = "WallLabel")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ListValue(Name = "EnabledInternalForces", Index = 1, Localizable=true, LocalizableValue = true)]
      public List<EnabledInternalForcesForWall> EnabledInternalForcesWall { get; set; }

      /// <summary>
      /// Rebar parameters component for vertical reinforcement
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "VerticalRnfSteelParameters")]
      [CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParameters(Category = "VerticalRnfSteelParameters")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "VerticalRnfSteelParameters", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema VerticalReinforcement { get; set; }

      /// <summary>
      /// Rebar parameters component for horizontal reinforcement
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "HorizontalRnfSteelParameters")]
      [CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParameters(Category = "HorizontalRnfSteelParameters")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "HorizontalRnfSteelParameters", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema HorizontalReinforcement { get; set; }

      /// <summary>
      /// Creep coefficient
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "CreepCoefficient", Unit = Autodesk.Revit.DB.UnitType.UT_Number, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_GENERAL)]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.TextBox(Description = "CreepCoefficient", Category = "CalculationOptions", IsVisible = true, IsEnabled = true, Index = -1, Localizable = true, FieldValidator = typeof(ValueValidatorWall))]
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
            foreach (EnabledInternalForcesForWall enabledInternalForcesWall in EnabledInternalForcesWall)
            {
               EInternalForces.Add((ConcreteTypes.EnabledInternalForces)enabledInternalForcesWall);
            }
            return EInternalForces;
         }
      }
      /// <summary>
      /// Creates default LabelWall
      /// </summary>
      public LabelWall()
      {
         VerticalReinforcement = new CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema();
         HorizontalReinforcement = new CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema();
         CreepCoefficient = 2.0;
         EnabledInternalForcesWall = new List<EnabledInternalForcesForWall>();
         EnabledInternalForcesWall.Add(EnabledInternalForcesForWall.FX);
         EnabledInternalForcesWall.Add(EnabledInternalForcesForWall.MX);
      }
   }
   /// </structural_toolkit_2015>
}
