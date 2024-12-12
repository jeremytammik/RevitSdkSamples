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
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.Revit.DB.CodeChecking.Engineering.Tools;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework;

namespace CodeCheckingConcreteExample.UIComponents.CalculationPointsSelector
{
   /// <summary>
   /// Component for calculation points selection
   /// Available selection methods are: UniformDistribution,UserCoordinates,UserSegmentDivision,Points of extreme values
   /// </summary>
   [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("CalculationPointsSelectorSchema", "88b169e5-cfb4-440a-830f-20958f71663b")]
   public class CalculationPointsSelectorSchema : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
   {
      // UniformDistribution

      /// <summary>
      /// Division type
      /// </summary>
      public enum DivisionType 
      { 
         /// <summary>
         /// Uniform distribution defined by a given number of points
         /// </summary>
         Points,
         /// <summary>
         /// Uniform distribution defined by segments
         /// </summary>
         Segments 
      }

      /// <summary>
      /// How the bar should be divided into calculation points
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(FieldName = "ElementDivisionType")]
      [EnumControl(Description = "ElementDivisionType", EnumType = typeof(ConcreteTypes.EnabledInternalForces), Presentation = PresentationMode.OptionList, Item = PresentationItem.Text, Localizable = true, Context = "CalculationPointsSelector")]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.DefaultElement(Localizable = true)]
      public DivisionType ElementDivisionType { get; set; }

      /// <summary>
      /// Number points in Uniform distribution
      /// </summary>
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.TextBox(Description = "UniformDistribution", Index = 2)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(Name = "UniformDistribution", Localizable = true, LocalizableValue = true)]
      public int UniformDistribution { get; set; }
      
      /// <summary>
      /// Length of user segment(Absolute)
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(Unit = UnitType.UT_Length, DisplayUnit = DisplayUnitType.DUT_METERS)]
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.UnitTextBox(Description = "UserSegmentDivision", Index = 3, MinimumValue = 1.0e-12, ValidateMinimumValue = true)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithName(Name = "UserSegmentDivision", Localizable = true, LocalizableValue = true, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General)]
      public double UserSegmentDivision { get; set; }

      /// <summary>
      /// Points of extreme values flag
      /// </summary>
      [Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes.CheckBox(Description = "IsPointsOfExtremeValues", Index = 4)]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty]
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes.ValueWithDescription(Description = "IsPointsOfExtremeValues", Localizable = true, LocalizableValue = true, Level = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DetailLevel.General)]
      public bool IsPointsOfExtremeValues { get; set; }

      /// <summary>
      /// Creates default CalculationPointsSelectorSchema
      /// </summary>
      public CalculationPointsSelectorSchema()
      {
         UserSegmentDivision = 1.0;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="doc">Revit document</param>
      /// <param name="isOutputRelative">Flag indicating whether returned points should have relative coordinates</param>
      /// <param name="absoluteElementLength">Element length</param>
      /// <param name="elementId">Revit ElementId</param>
      /// <param name="combinations">List of Revit ElementId for combinations</param>
      /// <param name="forceTypes">Force type vector</param>
      /// <returns></returns>
      public List<double> GetPointCoordinates(Document doc, bool isOutputRelative, double absoluteElementLength, ElementId elementId, List<ElementId> combinations, List<ForceType> forceTypes)
      {
         List<double> coordinates = new List<double>();
         bool isNonzeroElement = absoluteElementLength > 0.01;
               
      
         // don't calculate if element length is zero and coordinates in absolute points
         if (!isOutputRelative || isNonzeroElement)
         {
      
            // get points from uniform distribution
            if (ElementDivisionType == DivisionType.Points)
            {
                  int numberOfPoints = UniformDistribution,
                      lastId = numberOfPoints - 1;
                  if (lastId > 0)
                  {
                     double segmentLength = ( isOutputRelative ? 1.0 : absoluteElementLength ) / (lastId);
                     for (int i = 0; i <= lastId; i++)
                     {
                        coordinates.Add(i * segmentLength);
                     }
                  }
               }
      
            // get points from user segment division
            else if (ElementDivisionType == DivisionType.Segments)
            {
                  if ( isNonzeroElement)
                  {
                     double segmentLength = (UserSegmentDivision / absoluteElementLength),
                            distance   = 0.0;
                     while (distance < 1.0)
                     {
                        coordinates.Add(distance);
                        distance += segmentLength;
                     }
                     coordinates.Add(1.0);
                  }
               }

            // get points where forces have extreme values
            if (IsPointsOfExtremeValues && forceTypes!=null && forceTypes.Count!=0)
            {
               List<double> extremeValueCoordinates = (ForceResultsUtils.GetExtremumValueCoordinates(doc, combinations, forceTypes, new List<ElementId>() { elementId }).Select(s => s.Item4)).ToList();

               if (isOutputRelative)
               {
                  if (isNonzeroElement)
                  {
                     for (int i = 0; i < extremeValueCoordinates.Count; i++)
                     {
                        extremeValueCoordinates[i] /= absoluteElementLength;
                     }
                  }
                  else
                  {
                     throw new ArgumentException("Nonzero element length required for relative output");
                  }
               }


               coordinates.AddRange(extremeValueCoordinates);
            }

           
      
            coordinates.Sort();
         }


         return coordinates.Distinct(new DoubleComparer()).ToList();
      }

   }

   internal class DoubleComparer : IEqualityComparer<double>
   {
      public bool Equals(double a, double b) { return Math.Abs(a - b) < 0.0001; }
      public int GetHashCode(double a) { return (int)Math.Floor(a * 100000 + 0.5); }
   }
}
