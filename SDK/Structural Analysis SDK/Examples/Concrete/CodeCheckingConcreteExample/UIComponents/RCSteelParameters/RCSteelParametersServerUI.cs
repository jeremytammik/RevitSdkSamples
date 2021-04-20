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

namespace CodeCheckingConcreteExample.UIComponents.RCSteelParameters
{
#pragma warning disable 1591
   class RCSteelParametersServerUI : Autodesk.Revit.UI.ExtensibleStorage.Framework.IServerUI
   {
      #region IServerUI Members


      private RCSteelParametersSchema schema = null;

      private void resetSchema( Autodesk.Revit.UI.ExtensibleStorage.Framework.SchemaEditorEventArgs e )
      {
         schema = new RCSteelParametersSchema(e.Entity as Entity, e.Document);
      }

      public System.Collections.IList GetDataSource(string key, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.DisplayUnitType unitType)
      {
         return null;
      }

      public void LayoutInitialized(object sender, Autodesk.Revit.UI.ExtensibleStorage.Framework.LayoutInitializedEventArgs e)
      {
         resetMaterialCombo(e);
      }

      public void ValueChanged(object sender, Autodesk.Revit.UI.ExtensibleStorage.Framework.ValueChangedEventArgs e)
      {
         switch((e as Autodesk.Revit.UI.ExtensibleStorage.Framework.ValueChangedEventArgs).FieldName)
         {
            case "Material":
               onMaterialComboChange(e);
               break;
            case "RebarBarType":
               onBarChange(e);
               break;
         }
      }

      private void resetMaterialCombo(Autodesk.Revit.UI.ExtensibleStorage.Framework.SchemaEditorEventArgs e)
      {
         resetSchema(e);
         IList<ElementId> materials = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetListMaterialOfRebars(e.Document);
         e.Editor.SetAttribute("Material", Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.ComboBoxAttribute.PropertyDataSource, materials, DisplayUnitType.DUT_UNDEFINED);
         e.Editor.SetValue("Material", schema.Material, DisplayUnitType.DUT_UNDEFINED);
         onMaterialComboChange(e);
      }

      private void onMaterialComboChange(Autodesk.Revit.UI.ExtensibleStorage.Framework.SchemaEditorEventArgs e)
      {
         resetSchema(e);
         // update barTypeCombo

         // update strength field
         if (schema.Material != null)
         {
            double yield = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(schema.Material).MinimumYieldStress;
            yield = Autodesk.Revit.DB.UnitUtils.ConvertFromInternalUnits(yield, DisplayUnitType.DUT_PASCALS);
            e.Editor.SetValue("MinimumYieldStress", yield, DisplayUnitType.DUT_PASCALS);

            IList<ElementId> barsIds = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetListRebarForMaterial(e.Document, schema.Material.Id);
            List<RebarBarType> barTypesSource = (from elementId in barsIds select e.Document.GetElement(elementId) as RebarBarType).ToList();
            var btypes = barTypesSource.Select(s => new { myDiam = s.BarDiameter, mybType=s  }).OrderBy(s => s.myDiam);
            List<RebarBarType> bars = btypes.Select(s => s.mybType).ToList();
            RebarBarType currentBar = schema.RebarBarType;
            e.Editor.SetAttribute("RebarBarType", Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.ComboBoxAttribute.PropertyDataSource, bars, DisplayUnitType.DUT_UNDEFINED);
            if (currentBar == null || !barsIds.Contains(currentBar.Id))
            {
               e.Editor.SetValue("RebarBarType", null, DisplayUnitType.DUT_UNDEFINED);
            }
            else
            {
               e.Editor.SetValue("RebarBarType", currentBar, DisplayUnitType.DUT_UNDEFINED);
               onBarChange(e);
            }

         }
         else
         {
            e.Editor.SetValue("MinimumYieldStress", 0.0, DisplayUnitType.DUT_PASCALS);
         }

      }

      private void onBarChange(Autodesk.Revit.UI.ExtensibleStorage.Framework.SchemaEditorEventArgs e)
      {
         resetSchema(e);
         if (schema.RebarBarType != null)
         {
            double diameter = Autodesk.Revit.DB.UnitUtils.ConvertFromInternalUnits(schema.RebarBarType.BarDiameter, DisplayUnitType.DUT_METERS);
            e.Editor.SetValue("BarDiameter", diameter, DisplayUnitType.DUT_METERS);
            e.Editor.SetValue("DeformationType", schema.RebarBarType.DeformationType == RebarDeformationType.Plain ? Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.ConcreteTypes.SteelSurface.Plain : Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.ConcreteTypes.SteelSurface.Deformed, DisplayUnitType.DUT_UNDEFINED);
         }
         else
         {
            e.Editor.SetValue("BarDiameter", 0.0, DisplayUnitType.DUT_METERS);
            e.Editor.SetValue("DeformationType", Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.ConcreteTypes.SteelSurface.Unknown, DisplayUnitType.DUT_UNDEFINED);
         }
      }

      #endregion

      #region IResource Members

      public string GetResource(string key, string context)
      {
         return key;
      }

      public Uri GetResourceImage(string key, string context)
      {
         return null;
      }



      #endregion

   }
}
