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
using CodeCheckingConcreteExample.Engine;
using Autodesk.Revit.DB;

namespace CodeCheckingConcreteExample.Main.Calculation
{
   /// <summary>
   /// Represents user's calculation scenario.
   /// </summary>
   public class CalculationScenario : ICalculationScenario
   {

      #region ICalculationScenario Members

      /// <summary>
      /// Creates list of cref="ICalculationObject".
      /// </summary>
      /// <returns>List of cref="ICalculationObject".</returns>
      public List<ICalculationObject> CalculationScenarioList()
      {
         List<ICalculationObject> scenario = new List<ICalculationObject>();

         {
            PrepareSectionData calcObj = new PrepareSectionData();
            calcObj.Type = CalculationObjectType.Section;
            calcObj.ErrorResponse = ErrorResponse.SkipOnError;
            calcObj.Categories = new List<BuiltInCategory>() { BuiltInCategory.OST_BeamAnalytical, BuiltInCategory.OST_ColumnAnalytical };
            scenario.Add(calcObj);
         }

         {
            ModifyElementForces calcObj = new ModifyElementForces();
            calcObj.Type = CalculationObjectType.Element;
            calcObj.ErrorResponse = ErrorResponse.SkipOnError;
            calcObj.Categories = new List<BuiltInCategory>() { BuiltInCategory.OST_BeamAnalytical, BuiltInCategory.OST_ColumnAnalytical };
            scenario.Add(calcObj);
         }

         {
            CalculateSection calcObj = new CalculateSection();
            calcObj.Type = CalculationObjectType.Section;
            calcObj.ErrorResponse = ErrorResponse.SkipOnError;
            calcObj.Categories = new List<BuiltInCategory>() { BuiltInCategory.OST_BeamAnalytical, BuiltInCategory.OST_ColumnAnalytical };
            scenario.Add(calcObj);
         }


         {
            CalculateDeflection calcObj = new CalculateDeflection();
            calcObj.Type = CalculationObjectType.Element;
            calcObj.ErrorResponse = ErrorResponse.SkipOnError;
            calcObj.Categories = new List<BuiltInCategory>() { BuiltInCategory.OST_BeamAnalytical };
            scenario.Add(calcObj);
         }

         {
            FillResultData calcObj = new FillResultData();
            calcObj.Type = CalculationObjectType.Element;
            calcObj.ErrorResponse = ErrorResponse.RunOnError;
            calcObj.Categories = new List<BuiltInCategory>() { BuiltInCategory.OST_BeamAnalytical, BuiltInCategory.OST_ColumnAnalytical };
            scenario.Add(calcObj);
         }

         return scenario;
      }

      #endregion
   }
}
