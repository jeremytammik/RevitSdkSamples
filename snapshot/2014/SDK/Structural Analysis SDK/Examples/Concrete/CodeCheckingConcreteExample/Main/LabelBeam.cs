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

namespace CodeCheckingConcreteExample.Main
{
   /// <summary>
   /// Values validator for beam label.
   /// </summary>
   public class ValueValidatorBeam : IFieldValidator
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
   /// Container for RC beam element material and calculation options
   /// </summary>
   [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("LabelBeam", "969f2bcf-b1b6-44c2-94af-f6aeff453705")]
   public class LabelBeam : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
   {
      /// <summary>
      /// Classification of available forces for beam
      /// </summary>
      public enum EnabledInternalForcesForBeam
      {
         /// <summary>
         /// Axial force. The force acting along the element.
         /// </summary>
         FX = ConcreteTypes.EnabledInternalForces.FX,
         /// <summary>
         /// Shear force. The force acting perpendicular to the element along Z axis.
         /// </summary>
         FZ = ConcreteTypes.EnabledInternalForces.FZ,
         /// <summary>
         /// Torsional moment.
         /// </summary>
         MX = ConcreteTypes.EnabledInternalForces.MX,
         /// <summary>
         /// Bending moment. Bending around the Y axis.
         /// </summary>
         MY = ConcreteTypes.EnabledInternalForces.MY,
      }
      /// <summary>
      /// Collection of beam simple calculation 
      /// representing calculation options chosen by the user
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "BeamCalculationType")]
      [EnumControl(Description = "EnabledInternalForces", Category = "CalculationOptions", EnumType = typeof(EnabledInternalForcesForBeam), Presentation = PresentationMode.OptionList, Item = PresentationItem.ImageWithText, ImageSize = ImageSize.Medium, Context = "BeamLabel")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ListValue(Name = "EnabledInternalForces", Localizable = true, LocalizableValue = true)]
      public List<EnabledInternalForcesForBeam> EnabledInternalForcesBeam { get; set; }


      /// <summary>
      /// Creep coefficient
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "CreepCoefficient", Unit = Autodesk.Revit.DB.UnitType.UT_Number, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_GENERAL)]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.TextBox(Description = "CreepCoefficient", Category = "CalculationOptions", IsVisible = true, IsEnabled = true, Index = -1, Localizable = true, FieldValidator = typeof(ValueValidatorBeam))]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "CreepCoefficient", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public Double CreepCoefficient { get; set; }

      /// <summary>
      /// Section type object(flanges/no flanges) representing
      /// option chosen by the user
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty]
      [EnumControl(Description = "SlabBeamInteraction", Category = "CalculationOptions", Index = -1, EnumType = typeof(ConcreteTypes.BeamSectionType), Presentation = PresentationMode.ToggleButton, Item = PresentationItem.Image, ImageSize = ImageSize.Medium, Context = "BeamLabel")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(LocalizableValue = true, Name = "SlabBeamInteraction", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public ConcreteTypes.BeamSectionType SlabBeamInteraction { get; set; }

      /// <summary>
      /// Rebar parameters component for longitudinal reinforcement
      ///</summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "LongitudinalReinforcement")]
      [CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParameters(Category = "LongitudinalReinforcement")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.SubSchemaElement(LocalizableValue = true, Name = "LongitudinalReinforcement", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema LongitudinalReinforcement { get; set; }

      /// <summary>
      /// Rebar parameters component for transversal reinforcement
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "TransversalReinforcement")]
      [CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParameters(Category = "TransversalReinforcement")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.SubSchemaElement(LocalizableValue = true, Name = "TransversalReinforcement", Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = true)]
      public CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema TransversalReinforcement { get; set; }

      /// <summary>
      /// Creates default LabelBeam
      /// </summary>
      public LabelBeam()
      {
         LongitudinalReinforcement = new CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema();
         CreepCoefficient = 2.0;
         TransversalReinforcement = new CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema();
         EnabledInternalForcesBeam = new List<EnabledInternalForcesForBeam>();
         EnabledInternalForcesBeam.Add(EnabledInternalForcesForBeam.MY);
         EnabledInternalForcesBeam.Add(EnabledInternalForcesForBeam.FZ);
         SlabBeamInteraction = ConcreteTypes.BeamSectionType.WithSlabBeamInteraction;
      }

      /// <summary>
      /// Transversal calculation type ( compression, bending, excentric bending....)
      /// </summary>
      public ConcreteTypes.CalculationType TransversalCalculationType { get { return EnabledInternalForces.GetTransversalCalculationType(); } }

      /// <summary>
      /// Longitudinal calculation type (shearing, tortion, ...)
      /// </summary>
      public ConcreteTypes.CalculationType LongitudinalCalculationType { get { return EnabledInternalForces.GetLongitudinalCalculationType(); } }

      /// <summary>
      /// Returns EnabledInternalForces based on EnabledInternalForcesBeam
      /// </summary>
      public List<ConcreteTypes.EnabledInternalForces> EnabledInternalForces
      {
         get
         {
            List<ConcreteTypes.EnabledInternalForces> EInternalForces = new List<ConcreteTypes.EnabledInternalForces>();
            foreach (EnabledInternalForcesForBeam enabledInternalForcesBeam in EnabledInternalForcesBeam)
            {
               EInternalForces.Add((EnabledInternalForces)enabledInternalForcesBeam);
            }
            return EInternalForces;
         }
      }
   }
}
