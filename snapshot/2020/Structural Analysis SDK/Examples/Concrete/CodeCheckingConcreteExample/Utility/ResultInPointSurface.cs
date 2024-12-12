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

/// <structural_toolkit_2015>
namespace CodeCheckingConcreteExample.Utility
{
   /// <summary>
   /// Type of results for rc section
   /// </summary>
   public enum ResultTypeSurface
   {
      /// <summary>
      /// X position (absolute) 
      /// </summary>
      X,
      /// <summary>
      /// Y position (absolute) 
      /// </summary>  
      Y,
      /// <summary>
      /// Z position (absolute) 
      /// </summary> 
      Z,
      /// <summary>
      /// Bending moment , x-axis, min value [moment per unit length]
      /// </summary> 
      MxxMin,
      /// <summary>
      /// Bending moment , x-axis, max value [moment per unit length]
      /// </summary>
      MxxMax,
      /// <summary>
      /// Bending moment , y-axis, min value [moment per unit length]
      /// </summary> 
      MyyMin,
      /// <summary>
      /// Bending moment , x-axis, min value [moment per unit length]
      /// </summary> 
      MyyMax,
      /// <summary>
      /// Twisting moments, , x-axis/y-axis, min value [moment per unit length]
      /// </summary> 
      MxyMin,
      /// <summary>
      /// Twisting moments, , x-axis/y-axis, min value [moment per unit length]
      /// </summary> 
      MxyMax,
      /// <summary>
      /// In-plane forces tension/compresion , x-direction, max value [moment per unit length]
      /// </summary>
      FxxMin,
      /// <summary>
      /// In-plane forces tension/compresion, x-direction, min value [force per unit length]
      /// </summary>
      FxxMax,
      /// <summary>
      /// In-plane forces tension/compresion, y-direction, max value [force per unit length]
      /// </summary>
      FyyMin,
      /// <summary>
      /// In-plane forces tension/compresion, y-direction, min value [force per unit length]
      /// </summary>
      FyyMax,
      /// <summary>
      /// In-plane shear forces , y-direction, max value [force per unit length]
      /// </summary>
      FxyMin,
      /// <summary>
      /// In-plane shear forces , y-direction, min value [force per unit length]
      /// </summary>
      FxyMax,
      /// <summary>
      /// Shear forces , x-direction, max value [force per unit length]
      /// </summary>
      QxxMin,
      /// <summary>
      /// Shear forces , x-direction, min value [force per unit length]
      /// </summary>
      QxxMax,
      /// <summary>
      /// Shear forces , y-direction, max value [force per unit length]
      /// </summary>
      QyyMin,
      /// <summary>
      /// Shear forces , y-direction, max value [force per unit length]
      /// </summary>
      QyyMax,
      /// <summary>
      /// Longitudinal reinforcement, bottom [area per unit length], according to Mxx
      /// </summary>
      AxxBottom,
      /// <summary>
      /// Longitudinal reinforcement, top [area per unit length], according to Mxx
      /// </summary>
      AxxTop,
      /// <summary>
      /// Longitudinal reinforcement, bottom [area per unit length], according to Myy
      /// </summary>
      AyyBottom,
      /// <summary>
      /// Longitudinal reinforcement, top [area per unit length], according to Myy
      /// </summary>
      AyyTop,
      /// <summary>
      /// Deflection, z-direction, max value
      /// </summary>
      UzMax,
      /// <summary>
      /// Deflection, z-direction, min value
      /// </summary>
      UzMin,
      /// <summary>
      /// Calculated deflection, z-direction, min value
      /// </summary>
      UzRealMax,
      /// <summary>
      /// Calculated deflection, z-direction, max value
      /// </summary>
      UzRealMin,
   }
   static class ResultTypeSurfaceHelper
   {
      /// <summary>
      /// Converts value from cref="ResultTypeSurface" into value from cref="ResultType".
      /// </summary>
      /// <param name="forceType">Type of force.</param>
      /// <returns>Type of result.</returns>
      static public Autodesk.Revit.DB.CodeChecking.Engineering.ResultType GetResultType(this ResultTypeSurface forceType)
      {
         Autodesk.Revit.DB.CodeChecking.Engineering.ResultType resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.Unknown;
         switch (forceType)
         {
            // reinforcement
            case ResultTypeSurface.AxxBottom: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.AxxBottom; break;
            case ResultTypeSurface.AxxTop: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.AxxTop; break;
            case ResultTypeSurface.AyyBottom: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.AyyBottom; break;
            case ResultTypeSurface.AyyTop: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.AyyTop; break;
            // deflection
               ///TMP
               ///
            /*
            case ResultTypeSurface.UxRealMax: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UxMax; break;
            case ResultTypeSurface.UxRealMin: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UxMin; break;
            case ResultTypeSurface.UyRealMax: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UyMax; break;
            case ResultTypeSurface.UyRealMin: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UyMin; break;
            case ResultTypeSurface.UzRealMax: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UzMax; break;
            case ResultTypeSurface.UzRealMin: resultType = Autodesk.Revit.DB.CodeChecking.Engineering.ResultType.UzMin; break;
             * */
         }
         return resultType;
      }

      /// <summary>
      /// Gets unit for a specific Result type
      /// </summary>
      /// <param name="forceType"></param>
      /// <returns></returns>
      static public UnitType GetUnitType(this ResultTypeSurface forceType)
      {
         return ResultInPointSurface.ValueType[(int)forceType];
      }

      /// <summary>
      /// Gets reinforcement result types
      /// </summary>
      static public List<ResultTypeSurface> ReinforcementResults
      {
         get { return new List<ResultTypeSurface> { ResultTypeSurface.AxxBottom, ResultTypeSurface.AxxTop, ResultTypeSurface.AyyBottom, ResultTypeSurface.AxxTop}; }
      }

      /// <summary>
      /// Gets displacement result types
      /// </summary>
      static public List<ResultTypeSurface> DisplacementResults
      {
         get
         {
            return new List<ResultTypeSurface> 
            { 
               ResultTypeSurface.UzMax, ResultTypeSurface.UzMin, 
            };
         }
      }
      /// <summary>
      /// Gets real deflection result types
      /// </summary>
      static public List<ResultTypeSurface> RealDeflectionResults
      {
         get
         {
            return new List<ResultTypeSurface> 
            { 
               ResultTypeSurface.UzRealMax, ResultTypeSurface.UzRealMin,
            };
         }
      }
      /// <summary>
      /// Gets base internal forces result types
      /// </summary>
      static public List<ResultTypeSurface> InternalForcesResults
      {
         get
         {
            return new List<ResultTypeSurface> 
            {     
               ResultTypeSurface.FxxMin, ResultTypeSurface.FxxMax, ResultTypeSurface.FyyMin, ResultTypeSurface.FyyMax,
            };
         }
      }
      /// <summary>
      /// Gets all internal forces result types
      /// </summary>
      static public List<ResultTypeSurface> InternalForcesResultsAll
      {
         get
         {
            return new List<ResultTypeSurface> 
            {     
               ResultTypeSurface.FxxMin, ResultTypeSurface.FxxMax, ResultTypeSurface.FyyMin, ResultTypeSurface.FyyMax, ResultTypeSurface.FxyMin, ResultTypeSurface.FxyMax, 
            };
         }
      }
      /// <summary>
      /// Gets base internal moments result types
      /// </summary>
      static public List<ResultTypeSurface> InternalMomentsResults
      {
         get
         {
            return new List<ResultTypeSurface> 
            { 
               ResultTypeSurface.MxxMin, ResultTypeSurface.MxxMax, ResultTypeSurface.MyyMin, ResultTypeSurface.MyyMax
            };
         }
      }
      /// <summary>
      /// Gets all internal moments result types
      /// </summary>
      static public List<ResultTypeSurface> InternalMomentsResultsAll
      {
         get
         {
            return new List<ResultTypeSurface> 
            { 
               ResultTypeSurface.MxxMin, ResultTypeSurface.MxxMax, ResultTypeSurface.MyyMin, ResultTypeSurface.MyyMax, ResultTypeSurface.MxyMin, ResultTypeSurface.MxyMax,
            };
         }
      }
   }
   /// <summary>
   /// Container representing analitical results in RC surface section
   /// Results are represented as numbers along with their respective units
   /// </summary>
   public class ResultInPointSurface
   {
      private double[] data = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
      static internal UnitType[] ValueType = new UnitType[] { UnitType.UT_Length, 
                                                            UnitType.UT_Length, 
                                                            UnitType.UT_Length,
                                                            UnitType.UT_LinearMoment,
                                                            UnitType.UT_LinearMoment,
                                                            UnitType.UT_LinearMoment,
                                                            UnitType.UT_LinearMoment,
                                                            UnitType.UT_LinearMoment,
                                                            UnitType.UT_LinearMoment,
                                                            UnitType.UT_LinearForce,
                                                            UnitType.UT_LinearForce,
                                                            UnitType.UT_LinearForce,
                                                            UnitType.UT_LinearForce,
                                                            UnitType.UT_LinearForce,
                                                            UnitType.UT_LinearForce,
                                                            UnitType.UT_LinearForce,
                                                            UnitType.UT_LinearForce,
                                                            UnitType.UT_LinearForce,
                                                            UnitType.UT_LinearForce,
                                                            UnitType.UT_Reinforcement_Area_per_Unit_Length,
                                                            UnitType.UT_Reinforcement_Area_per_Unit_Length,
                                                            UnitType.UT_Reinforcement_Area_per_Unit_Length,
                                                            UnitType.UT_Reinforcement_Area_per_Unit_Length,
                                                            UnitType.UT_Displacement_Deflection,
                                                            UnitType.UT_Displacement_Deflection,
                                                            UnitType.UT_Displacement_Deflection,
                                                            UnitType.UT_Displacement_Deflection};
      /// <summary>
      /// Creates default ResultInPointSurface
      /// </summary>
      public ResultInPointSurface() { }

      /// <summary>
      /// Creates default ResultInPointLinear
      /// </summary>
      /// <param name="data">List of result according to ResultTypeSurface order</param>
      public ResultInPointSurface(IEnumerable<double> data) { DataRaw = data.ToArray(); }

      /// <summary>
      /// Access to the raw format of surface section results
      /// </summary>
      public double[] DataRaw { 
         get { return data; }
         set {
            int reqSize = Enum.GetValues(typeof(ResultTypeSurface)).Length;
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
      public double this[ResultTypeSurface type] 
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
/// </structural_toolkit_2015>

