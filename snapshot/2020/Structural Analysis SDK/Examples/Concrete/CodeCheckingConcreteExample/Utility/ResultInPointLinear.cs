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
using Autodesk.Revit.DB;

namespace CodeCheckingConcreteExample.Utility
{
   /// <summary>
   /// Type of results for rc section
   /// </summary>
   public enum ResultTypeLinear
   {
      /// <summary>
      /// linear position(absolute) 
      /// </summary>
      X,
      /// <summary>
      /// linear position(relative) 
      /// </summary>
      X_Rel,
      /// <summary>
      /// Bending moment , x-axis, min value
      /// </summary>
      MxMin,
      /// <summary>
      /// Bending moment , x-axis, max value
      /// </summary>
      MxMax,
      /// <summary>
      /// Bending moment , y-axis, min value
      /// </summary>
      MyMin,
      /// <summary>
      /// Bending moment , y-axis, max value
      /// </summary>
      MyMax,
      /// <summary>
      /// Bending moment , z-axis, min value
      /// </summary>
      MzMin,
      /// <summary>
      /// Bending moment , z-axis, max value
      /// </summary>
      MzMax,
      /// <summary>
      /// Transversal force , x-direction, min value
      /// </summary>
      FxMin,
      /// <summary>
      /// Transversal force , x-direction, max value
      /// </summary>
      FxMax,
      /// <summary>
      /// Transversal force , y-direction, min value
      /// </summary>
      FyMin,
      /// <summary>
      /// Transversal force , y-direction, max value
      /// </summary>
      FyMax,
      /// <summary>
      /// Transversal force , z-direction, min value
      /// </summary>
      FzMin,
      /// <summary>
      /// Transversal force , z-direction, max value
      /// </summary>
      FzMax,
      /// <summary>
      /// Longitudinal reinforcement, bottom
      /// </summary>
      Abottom,
      /// <summary>
      /// Longitudinal reinforcement, top
      /// </summary>
      Atop,
      /// <summary>
      /// Longitudinal reinforcement, left
      /// </summary>
      Aleft,
      /// <summary>
      /// Longitudinal reinforcement, right
      /// </summary>
      Aright,
      /// <summary>
      /// Stirrups spacing
      /// </summary>
      StirrupsSpacing,
      /// <summary>
      /// Transversal reinforcement density
      /// </summary>
      TransversalReinforcemenDensity,
      /// <summary>
      /// Deflection, x-direction, min value
      /// </summary>
      UxMin,
      /// <summary>
      /// Deflection, x-direction, max value
      /// </summary>
      UxMax,
      /// <summary>
      /// Deflection, y-direction, min value
      /// </summary>
      UyMin,
      /// <summary>
      /// Deflection, y-direction, max value
      /// </summary>
      UyMax,
      /// <summary>
      /// Deflection, z-direction, min value
      /// </summary>
      UzMin,
      /// <summary>
      /// Deflection, z-direction, max value
      /// </summary>
      UzMax,
      /// <summary>
      /// Calculated deflection, x-direction, max value
      /// </summary>
      UxRealMin,
      /// <summary>
      /// Calculated deflection, x-direction, min value
      /// </summary>
      UxRealMax,
      /// <summary>
      /// Calculated deflection, y-direction, max value
      /// </summary>
      UyRealMin,
      /// <summary>
      /// Calculated deflection, y-direction, min value
      /// </summary>
      UyRealMax,
      /// <summary>
      /// Calculated deflection, z-direction, max value
      /// </summary>
      UzRealMin,
      /// <summary>
      /// Calculated deflection, z-direction, min value
      /// </summary>
      UzRealMax,

   }

   static class ResultTypeLinearHelper
   {
      /// <summary>
      /// Converts value from cref="ResultTypeLinear" into value from cref="ResultType".
      /// </summary>
      /// <param name="forceType">Type of force.</param>
      /// <returns>Type of result.</returns>
      static public Autodesk.Revit.DB.CodeChecking.Engineering.ResultType GetResultType(this ResultTypeLinear forceType)
      {
         Autodesk.Revit.DB.CodeChecking.Engineering.ResultType resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.Unknown;
         switch (forceType)
         {
            // reinforcement
            case ResultTypeLinear.Abottom: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.Abottom; break;
            case ResultTypeLinear.Atop: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.Atop; break;
            case ResultTypeLinear.Aleft: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.Aleft; break;
            case ResultTypeLinear.Aright: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.Aright; break;
            // deflection
            case ResultTypeLinear.UxRealMax: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UxMax; break;
            case ResultTypeLinear.UxRealMin: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UxMin; break;
            case ResultTypeLinear.UyRealMax: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UyMax; break;
            case ResultTypeLinear.UyRealMin: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UyMin; break;
            case ResultTypeLinear.UzRealMax: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UzMax; break;
            case ResultTypeLinear.UzRealMin: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UzMin; break;
         }
         return resultType;
      }

      /// <summary>
      /// Gets unit for a specific Result type
      /// </summary>
      /// <param name="forceType"></param>
      /// <returns></returns>
      static public UnitType GetUnitType(this ResultTypeLinear forceType)
      {
         return ResultInPointLinear.ValueType[(int)forceType];
      }

      /// <summary>
      /// Gets reinforcement result types
      /// </summary>
      static public List<ResultTypeLinear> ReinforcementResults
      {
         get { return new List<ResultTypeLinear> { ResultTypeLinear.Abottom, ResultTypeLinear.Aleft, ResultTypeLinear.Aright, ResultTypeLinear.Atop, ResultTypeLinear.StirrupsSpacing, ResultTypeLinear.TransversalReinforcemenDensity }; }
      }

      /// <summary>
      /// Gets displacement result types
      /// </summary>
      static public List<ResultTypeLinear> DisplacementResults
      {
         get
         {
            return new List<ResultTypeLinear> 
            { 
               ResultTypeLinear.UxMax, ResultTypeLinear.UxMin, 
               ResultTypeLinear.UyMax, ResultTypeLinear.UyMin,
               ResultTypeLinear.UzMax, ResultTypeLinear.UzMin,
            };
         }
      }
      /// <summary>
      /// Gets real deflection result types
      /// </summary>
      static public List<ResultTypeLinear> RealDeflectionResults
      {
         get
         {
            return new List<ResultTypeLinear> 
            { 
               ResultTypeLinear.UxRealMax, ResultTypeLinear.UxRealMin,
               ResultTypeLinear.UyRealMax, ResultTypeLinear.UyRealMin,
               ResultTypeLinear.UzRealMax, ResultTypeLinear.UzRealMin
            };
         }
      }
      /// <summary>
      /// Gets internal forces result types
      /// </summary>
      static public List<ResultTypeLinear> InternalForcesResults
      {
         get
         {
            return new List<ResultTypeLinear> 
            { 
               ResultTypeLinear.FxMin,  ResultTypeLinear.FxMax,  ResultTypeLinear.FyMin,  ResultTypeLinear.FyMax,   ResultTypeLinear.FzMin,  ResultTypeLinear.FzMax,
            };
         }
      }
      /// <summary>
      /// Gets internal moments result types
      /// </summary>
      static public List<ResultTypeLinear> InternalMomentsResults
      {
         get
         {
            return new List<ResultTypeLinear> 
            { 
               ResultTypeLinear.MxMin,  ResultTypeLinear.MxMax,  ResultTypeLinear.MyMin,  ResultTypeLinear.MyMax,   ResultTypeLinear.MzMin,  ResultTypeLinear.MzMax,
            };
         }
      }

   }

   /// <summary>
   /// Result Format
   /// </summary>
   public enum ResultFormat
   {
      /// <summary>
      /// Internal format in which data is stored internally
      /// </summary>
      Internal,
      /// <summary>
      /// External format in which data is presented on UI
      /// </summary>
      External
   };

   /// <summary>
   /// Container representing analitical results in RC bar section
   /// Results are represented as numbers along with their respective units
   /// </summary>
   public class ResultInPointLinear
   {
      private double[] data = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
      static internal UnitType[] ValueType = new UnitType[] { UnitType.UT_Length, 
                                                              UnitType.UT_Number,
                                                              UnitType.UT_Moment,
                                                              UnitType.UT_Moment,
                                                              UnitType.UT_Moment,
                                                              UnitType.UT_Moment,
                                                              UnitType.UT_Moment,
                                                              UnitType.UT_Moment,
                                                              UnitType.UT_Force,
                                                              UnitType.UT_Force,
                                                              UnitType.UT_Force,
                                                              UnitType.UT_Force,
                                                              UnitType.UT_Force,
                                                              UnitType.UT_Force,
                                                              UnitType.UT_Reinforcement_Area,
                                                              UnitType.UT_Reinforcement_Area,
                                                              UnitType.UT_Reinforcement_Area,
                                                              UnitType.UT_Reinforcement_Area,
                                                              UnitType.UT_Section_Dimension,
                                                              UnitType.UT_Reinforcement_Area_per_Unit_Length,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                              UnitType.UT_Displacement_Deflection,
                                                            };

      /// <summary>
      /// Creates default ResultInPointLinear
      /// </summary>
      public ResultInPointLinear() { }
      /// <summary>
      /// Creates default ResultInPointLinear
      /// </summary>
      /// <param name="data">List of result according to ResultTypeLinear order</param>
      public ResultInPointLinear(IEnumerable<double> data) { DataRaw = data.ToArray(); }

      /// <summary>
      /// Access to the raw format of section results
      /// </summary>
      public double[] DataRaw
      {
         get { return data; }
         set
         {
            int reqSize = Enum.GetValues(typeof(ResultTypeLinear)).Length;
            if (value.Count() != reqSize)
            {
               throw new ArgumentException("Value list lenght should be equal to " + reqSize);
            }
            else
            {
               data = value;
            }
         }
      }


      /// <summary>
      /// Access to a specific result
      /// </summary>
      /// <param name="type">Result type</param>
      /// <returns>Formatted result value</returns>
      public double this[ResultTypeLinear type]
      {
         get { return data[(int)type]; }
         set
         {
            if (Double.IsNaN(value))
            {
               throw new ArgumentOutOfRangeException(type.ToString() + "Cannot be NAN");
            }
            data[(int)type] = value;
         }
      }

   }
}
