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
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework;
using Autodesk.Revit.UI.ExtensibleStorage;
using CodeCheckingConcreteExample.ConcreteTypes;
using CodeCheckingConcreteExample.Properties;
using CodeCheckingConcreteExample.Main;

#pragma warning disable 1591

namespace CodeCheckingConcreteExample.Server
{
   [Autodesk.Revit.DB.CodeChecking.Attributes.CalculationParamsStructure(typeof(CalculationParameter))]
   [Autodesk.Revit.DB.CodeChecking.Attributes.LabelStructure(typeof(LabelBeam), BuiltInCategory.OST_BeamAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.LabelStructure(typeof(LabelColumn), BuiltInCategory.OST_ColumnAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   public class ServerUI : Autodesk.Revit.UI.CodeChecking.MultiStructureServer
   {
      #region ICodeCheckingServerUI Members
      public override string GetResource(string key, string context)
      {
         string txt = "";
         string resname = "";
         if (context != null)
         {
            switch (context)
            {
               default:
                  break;
               case "ColumnLabel":
                  {
                     switch (key)
                     {
                        case "FX":
                        case "FY":
                        case "FZ":
                        case "MX":
                        case "MY":
                        case "MZ":
                           resname = "Column" + key;
                           break;
                     }
                     break;
                  }
               case "BeamLabel":
                  {
                     switch (key)
                     {
                        case "FX":
                        case "FY":
                        case "FZ":
                        case "MX":
                        case "MY":
                        case "MZ":
                           resname = "Beam" + key;
                           break;
                     }
                     break;
                  }
            }
         }

         if (!string.IsNullOrEmpty(resname))
            txt = Resources.ResourceManager.GetString(resname);

         if (string.IsNullOrEmpty(txt))
            txt = Resources.ResourceManager.GetString(key);

         if (!string.IsNullOrEmpty(txt))
            return txt;

         return key;
      }

      public override Uri GetResourceImage(string key, string context)
      {
         String contextKey = "";
         if (context != null)
         {
            switch (context)
            {
               default:
                  break;
               case "ColumnLabel":
                  {
                     switch (key)
                     {
                        case "FX":
                           contextKey = "CFX_32";
                           break;
                        case "FY":
                           contextKey = "CFY_32";
                           break;
                        case "FZ":
                           contextKey = "CFZ_32";
                           break;
                        case "MX":
                           contextKey = "CMX_32";
                           break;
                        case "MY":
                           contextKey = "CMY_32";
                           break;
                        case "MZ":
                           contextKey = "CMZ_32";
                           break;
                        case "DirectionY":
                           contextKey = "buckling_Y";
                           break;
                        case "DirectionZ":
                           contextKey = "buckling_Z";
                           break;
                     }
                     break;
                  }
               case "BeamLabel":
                  {
                     switch (key)
                     {
                        case "FX":
                           contextKey = "BFX_32";
                           break;
                        case "FY":
                           contextKey = "BFY_32";
                           break;
                        case "FZ":
                           contextKey = "BFZ_32";
                           break;
                        case "MX":
                           contextKey = "BMX_32";
                           break;
                        case "MY":
                           contextKey = "BMY_32";
                           break;
                        case "MZ":
                           contextKey = "BMZ_32";
                           break;
                     }
                     break;
                  }
            }
         }

         if (contextKey == "")
         {
            switch (key)
            {
               case "WithSlabBeamInteraction":
                  contextKey = "BWithSlabBeamInteraction";
                  break;
               case "WithoutSlabBeamInteraction":
                  contextKey = "BWithoutSlabBeamInteraction";
                  break;
               default:
                  contextKey = context != null ? context + key : key;
                  break;
            }
         }

         String currAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString();
         return new Uri("pack://application:,,,/" + currAssemblyName + ";component/UIComponents/ICons/" + contextKey + ".png");
      }

      public override void LayoutInitialized(object sender, Autodesk.Revit.UI.ExtensibleStorage.Framework.LayoutInitializedEventArgs e)
      {
         onChange(e);

         base.LayoutInitialized(sender, e);
      }

      public override void ValueChanged(object sender, Autodesk.Revit.UI.ExtensibleStorage.Framework.ValueChangedEventArgs e)
      {
         onChange(e);
      }

      private void onChange(Autodesk.Revit.UI.ExtensibleStorage.Framework.SchemaEditorEventArgs e)
      {
         String elementTypeName = e.Editor.GetEntity().Schema.SchemaName;
         if (elementTypeName == "LabelColumn")
         {
            LabelColumn obj = new LabelColumn();
            obj.SetProperties(e.Entity as Autodesk.Revit.DB.ExtensibleStorage.Entity, e.Document);

            e.Editor.SetAttribute("LengthCoefficientY", FieldUIAttribute.PropertyIsEnabled, obj.BucklingDirectionY, DisplayUnitType.DUT_UNDEFINED);
            e.Editor.SetAttribute("ColumnStructureTypeY", FieldUIAttribute.PropertyIsEnabled, obj.BucklingDirectionY, DisplayUnitType.DUT_UNDEFINED);
            e.Editor.SetAttribute("LengthCoefficientZ", FieldUIAttribute.PropertyIsEnabled, obj.BucklingDirectionZ, DisplayUnitType.DUT_UNDEFINED);
            e.Editor.SetAttribute("ColumnStructureTypeZ", FieldUIAttribute.PropertyIsEnabled, obj.BucklingDirectionZ, DisplayUnitType.DUT_UNDEFINED);
         }
      }

      public override Layout BuildLabelLayout(Schema schema, Document document)
      {
         Layout layout = null;

         if (schema.SchemaName == "LabelBeam")
         {
            layout = Layout.Build(typeof(LabelBeam), this);
         }

         if (schema.SchemaName == "LabelColumn")
         {
            layout = Layout.Build(typeof(LabelColumn), this);
            if (layout != null)
            {
               Autodesk.Revit.UI.ExtensibleStorage.Framework.Category category = layout.GetCategory("Buckling");
               String currAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString();
               Image image = new Image() { Source = new Uri("pack://application:,,,/" + currAssemblyName + ";component/UIComponents/ICons/" + "buckling.png"), Index = 1 };
               category.Controls.Add(image);
               layout.SortIndexBased();
            }
         }
         return layout;
      }

      public override System.Collections.IList GetDataSource(string key, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.DisplayUnitType unitType)
      {
         return base.GetDataSource(key, document, unitType);
      }

      #endregion
   }
}
