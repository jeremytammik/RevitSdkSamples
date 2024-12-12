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

namespace CodeCheckingConcreteExample.Main
{
   /// <summary>
   /// Container for code Calculation parameters 
   /// </summary>
   [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("CalculationParam", "b3004f6e-cf05-4cad-8945-dba92d09372a")]
   public class CalculationParameter : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
   {
      /// <summary>
      /// Calculation points selection component
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "CalcPointSelector")]
      [CodeCheckingConcreteExample.UIComponents.CalculationPointsSelector.CalculationPointsSelector(Category = "CalcPointSelector")]
      public CodeCheckingConcreteExample.UIComponents.CalculationPointsSelector.CalculationPointsSelectorSchema CalculationPointsSelector { get; set; }

      /// <summary>
      /// Creates default CalculationParameter
      /// </summary>
      public CalculationParameter()
      {
         CalculationPointsSelector = new CodeCheckingConcreteExample.UIComponents.CalculationPointsSelector.CalculationPointsSelectorSchema();
         CalculationPointsSelector.ElementDivisionType = CodeCheckingConcreteExample.UIComponents.CalculationPointsSelector.CalculationPointsSelectorSchema.DivisionType.Points;
         CalculationPointsSelector.UniformDistribution = 11;
      }
   }
}
