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
using CodeCheckingConcreteExample.Utility;

namespace CodeCheckingConcreteExample.Main
{
   /// <summary>
   /// Container for RC linear element results data
   /// </summary>
   [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("ResultLinearElement", "94c5ab24-8e77-47a2-b437-750f43451b23")]
   public class ResultLinearElement : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
   {

      /// <summary>
      /// Collection of values representing element results in raw format
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(Unit = Autodesk.Revit.DB.UnitType.UT_Length, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_METERS)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ListValue(Name = "ValuesInPoints", LocalizableValue = false, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General, Localizable = false)]
      public List<Double> ValuesInPointsData { get; set; }

      /// <summary>
      /// Gets results in point formatted as a list of ResultInPointLinear elements
      /// </summary>
      /// <returns>List of ResultInPointLinear</returns>
      public List<ResultInPointLinear> GetResultsInPoints()
      {
         IEnumerable<ResultTypeLinear> linearResultTypes = Enum.GetValues(typeof(ResultTypeLinear)).OfType<ResultTypeLinear>();

         int numberOfResultTypes = linearResultTypes.Count(),
             numberOfPoints = ValuesInPointsData.Count / numberOfResultTypes;

         List<ResultInPointLinear> resultsInPoints = new List<ResultInPointLinear>();
         for (int ptId = 0; ptId < numberOfPoints; ptId++)
         {
            resultsInPoints.Add(new ResultInPointLinear(ValuesInPointsData.GetRange(ptId * numberOfResultTypes, numberOfResultTypes)));
         }

         return resultsInPoints;
      }

      /// <summary>
      /// Creates default ResultLinearElement
      /// </summary>
      public ResultLinearElement()
      {
         ValuesInPointsData = new List<double>();
      }

   }
}
