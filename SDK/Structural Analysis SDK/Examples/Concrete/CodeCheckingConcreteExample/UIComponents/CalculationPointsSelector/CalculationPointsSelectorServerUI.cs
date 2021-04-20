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

namespace CodeCheckingConcreteExample.UIComponents.CalculationPointsSelector
{
#pragma warning disable 1591
   class CalculationPointsSelectorServerUI : Autodesk.Revit.UI.ExtensibleStorage.Framework.IServerUI
   {
      #region IServerUI Members

      public System.Collections.IList GetDataSource(string key, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.DisplayUnitType unitType)
      {
         return null;
      }

      public void LayoutInitialized(object sender, Autodesk.Revit.UI.ExtensibleStorage.Framework.LayoutInitializedEventArgs e)
      {
         onChange(e);
      }

      public void ValueChanged(object sender, Autodesk.Revit.UI.ExtensibleStorage.Framework.ValueChangedEventArgs e)
      {
         onChange(e);
      }

      private void onChange(Autodesk.Revit.UI.ExtensibleStorage.Framework.SchemaEditorEventArgs e)
      {
         CalculationPointsSelectorSchema pointsSelectorSchema = new CalculationPointsSelectorSchema();
         pointsSelectorSchema.SetProperties(e.Entity as Autodesk.Revit.DB.ExtensibleStorage.Entity);

         CalculationPointsSelectorSchema.DivisionType divisionType = pointsSelectorSchema.ElementDivisionType;
         bool isUniformDistance = divisionType == CalculationPointsSelectorSchema.DivisionType.Points,
              isUserSegmentDistance = divisionType == CalculationPointsSelectorSchema.DivisionType.Segments;

         e.Editor.SetAttribute("UniformDistribution", Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.FieldUIAttribute.PropertyIsVisible, isUniformDistance, Autodesk.Revit.DB.DisplayUnitType.DUT_UNDEFINED);
         e.Editor.SetAttribute("UserSegmentDivision", Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.FieldUIAttribute.PropertyIsVisible, isUserSegmentDistance, Autodesk.Revit.DB.DisplayUnitType.DUT_UNDEFINED);
      }

      #endregion

      #region IResource Members

      public string GetResource(string key, string context)
      {
         String txt = null;
         if (context == "CalculationPointsSelector")
         {
            if (key == "Points" || key == "Segments")
            {
               txt = CodeCheckingConcreteExample.Properties.Resources.ResourceManager.GetString(key);
            }
         }

         return txt != null ? txt : key;
      }

      public Uri GetResourceImage(string key, string context)
      {
         return null;
      }



      #endregion


   }
}
