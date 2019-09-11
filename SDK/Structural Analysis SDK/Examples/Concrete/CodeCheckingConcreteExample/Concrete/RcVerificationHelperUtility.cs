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
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.CodeChecking.Concrete;

namespace CodeCheckingConcreteExample.Concrete
{
   /// <summary>
   /// This class provides helpers to use Autodesk.CodeChecking.Concrete component.
   /// </summary>
   public class RcVerificationHelperUtility
   {
      /// <summary>
      /// Classification of the cross section side for the distribution of the reinforcement.
      /// </summary>
      private enum CrossSectionSide
      {
         /// <summary>
         /// Top of cross section
         /// </summary>
         Top,
         /// <summary>
         /// Bottom of cross section
         /// </summary>
         Bottom,
         /// <summary>
         /// Right of cross section
         /// </summary>
         Right,
         /// <summary>
         /// Left of cross section
         /// </summary>
         Left
      }
      const double geometryEpsilon = 1e-6;      // tolerance for polyline which describes of section
      int noTopBottom;                          // the number of bars on the top or bottom side with corner bars
      int noLeftRight;                          // the number of bars on the top or bottom side without corner bars
      List<Rebar> rebars;                       // list of rebars in the cross-section
      List<CrossSectionSide> rebarsSide;        // list of bars position in the cross-section
      SectionShapeType crossSectionType;        // the type of cross-section
      double rebarCoverTop;                     // top cover from the edge to the gravity center of bars 
      double rebarCoverBottom;                  // bottom cover from the edge to the gravity center of bars
      double rebarCover;                        // the greater cover from the edge to the gravity center of bars
      Geometry solverGeometry;                  // geometry of cross section for solver object
      RCSolver solver;                          // solver
      double totalHeight;                       // the height of the cross section
      double totalWidth;                        // the width of the cross section
      double geometryMinX;                      // minimum x-coordinate on the geometry of the cross section
      double geometryMinY;                      // minimum y-coordinate on the geometry of the cross section
      double geometryMaxX;                      // maximum x-coordinate on the geometry of the cross section
      double geometryMaxY;                      // maximum y-coordinate on the geometry of the cross section
      /// <structural_toolkit_2015>

      /// <summary>
      /// Link between edges of cross section and reinforcement bars.
      /// </summary>
      Dictionary<CrossSectionSide, Tuple<int,int>> edgesForReinforcement; // 
      /// <summary>
      /// Compares position (coordinates) using a tolerance of geometry.
      /// </summary>
      /// <param name="FirstPosition">First position of geometry</param>
      /// <param name="SecondPosition">Second position of geometry</param>
      /// <returns>
      /// Less than zero - FirstPosition is less than SecondPosition. 
      /// Zero - FirstPosition is equal to SecondPosition
      /// Greater than zero  - FirstPosition is greater than SecondPosition. 
      /// </returns>
      static int CompareGeomety(double FirstPosition, double SecondPosition)
      {
         int compare = -1;
         if (Math.Abs(FirstPosition - SecondPosition) < geometryEpsilon)
            compare = 0;
         else if (FirstPosition > SecondPosition)
            compare = 1;
         return compare;
      }
      /// </structural_toolkit_2015>
   
      /// <summary>
      /// Initializes a new instance of the new RC calculation helper object.
      /// </summary>
      /// <param name="type">The type of the cross-section</param>
      /// <param name="rcGeometry">Set of geometry parameters.</param>
      /// <param name="coverTop">The top cover - to the reinforcement ceneter.</param>
      /// <param name="coverBottom">The top cover - to the reinforcement ceneter.</param>
      private RcVerificationHelperUtility(SectionShapeType type, ref Geometry rcGeometry, double coverTop, double coverBottom)
      {
         totalHeight = 0;                                   // initial value of the height of the cross section
         totalWidth = 0;                                    // initial value of the width of the cross section
         noTopBottom = 5;                                   // initial value of the number of bars on top and bottom
         noLeftRight = noTopBottom - 2;                     // initial value of the number of bars on left and right
         solverGeometry = new Geometry();                   // initialization of new geometry
         rebars = new List<Rebar>();                        // initialization of rebars list 
         rebarsSide = new List<CrossSectionSide>();         // initialization of bars position list 
         crossSectionType = type;                           // set of section type
         rebarCover = Math.Max(coverTop, coverBottom);      // set of maximum cover
         rebarCoverTop = coverTop;                          // set of top cover      
         rebarCoverBottom = coverBottom;                    // set of bottom cover
         /// <structural_toolkit_2015>
         edgesForReinforcement = new Dictionary<CrossSectionSide, Tuple<int, int>>();
         geometryMinX = Double.MaxValue;
         geometryMinY = Double.MaxValue;
         geometryMaxX = Double.MinValue;
         geometryMaxY = Double.MinValue;
         /// </structural_toolkit_2015>
         // Top,bottom, lreft and right edges are searched, based on maximum and minimum values for x and y coordinates
         /// <structural_toolkit_2015>
         solverGeometry = rcGeometry;
         int count = solverGeometry.Count;
         Point2D p = new Point2D(0, 0);
         // The orientation is changed if it is necessary.
         if (solverGeometry.isClockwiseOrientation()) // clockwise direction
         {
            Geometry tmpGeometry = new Geometry();
            p = solverGeometry.Point(0);
            tmpGeometry.Add(p.X, p.Y);
            for (int i = count-1; i > 0; i--)
            {
               p = solverGeometry.Point(i);
               tmpGeometry.Add(p.X, p.Y);
            }
            solverGeometry = tmpGeometry;
         }
         foreach (Point2D p2D in solverGeometry)
         {
            geometryMinX = Math.Min(geometryMinX, p2D.X);
            geometryMinY = Math.Min(geometryMinY, p2D.Y);
            geometryMaxX = Math.Max(geometryMaxX, p2D.X);
            geometryMaxY = Math.Max(geometryMaxY, p2D.Y);
         } 
         Tuple<int, int> curentTuple = null;
         for (int i = 0; i < count; i++)
			{
            p = solverGeometry.Point(i);
            // Left
            if (CompareGeomety(geometryMinX,p.X) >= 0)
            {
               if (!edgesForReinforcement.ContainsKey(CrossSectionSide.Left) || CompareGeomety(geometryMinX,p.X) > 0)
                  curentTuple = new Tuple<int, int>(i, i);
               else
               {
                  if (CompareGeomety(p.Y, solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Left].Item2).Y) > 0)
                     curentTuple = new Tuple<int, int>(edgesForReinforcement[CrossSectionSide.Left].Item1, i);
                  else if (CompareGeomety(p.Y, solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Left].Item1).Y) < 0)
                     curentTuple = new Tuple<int, int>(i, edgesForReinforcement[CrossSectionSide.Left].Item2);
               }
               edgesForReinforcement.Remove(CrossSectionSide.Left);
               edgesForReinforcement.Add(CrossSectionSide.Left, curentTuple);
            }
            // Right
            if (CompareGeomety(geometryMaxX,p.X) <= 0)
            {
               if (!edgesForReinforcement.ContainsKey(CrossSectionSide.Right) || CompareGeomety(geometryMaxX,p.X) < 0)
                  curentTuple = new Tuple<int, int>(i, i);
               else
               {
                  if (CompareGeomety(p.Y, solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Right].Item2).Y) > 0)
                     curentTuple = new Tuple<int, int>(edgesForReinforcement[CrossSectionSide.Right].Item1, i);
                  else if (CompareGeomety(p.Y, solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Right].Item1).Y) < 0)
                     curentTuple = new Tuple<int, int>(i, edgesForReinforcement[CrossSectionSide.Right].Item2);
               }
               edgesForReinforcement.Remove(CrossSectionSide.Right);
               edgesForReinforcement.Add(CrossSectionSide.Right, curentTuple);
            }
            // Top
            if (CompareGeomety(geometryMaxY,p.Y) <= 0)
            {
               if (!edgesForReinforcement.ContainsKey(CrossSectionSide.Top) || CompareGeomety(geometryMaxY,p.Y) < 0)
               {
                  curentTuple = new Tuple<int, int>(i, i);
               }
               else
               {
                  if (CompareGeomety(p.X, solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Top].Item2).X) > 0)
                     curentTuple = new Tuple<int, int>(edgesForReinforcement[CrossSectionSide.Top].Item1, i);
                  else if (CompareGeomety(p.X, solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Top].Item1).X) < 0)
                     curentTuple = new Tuple<int, int>(i, edgesForReinforcement[CrossSectionSide.Top].Item2);
               }
               edgesForReinforcement.Remove(CrossSectionSide.Top);
               edgesForReinforcement.Add(CrossSectionSide.Top, curentTuple);
            }
            // Bottom
            if (CompareGeomety(geometryMinY,p.Y) >= 0)
            {
               if (!edgesForReinforcement.ContainsKey(CrossSectionSide.Bottom) || CompareGeomety(geometryMinY,p.Y) > 0)
               {
                  curentTuple = new Tuple<int, int>(i, i);
               }
               else
               {
                  if (CompareGeomety(p.X, solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Bottom].Item2).X) > 0)
                     curentTuple = new Tuple<int, int>(edgesForReinforcement[CrossSectionSide.Bottom].Item1, i);
                  else if (CompareGeomety(p.X, solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Bottom].Item1).X) < 0)
                     curentTuple = new Tuple<int, int>(i, edgesForReinforcement[CrossSectionSide.Bottom].Item2);
               }
               edgesForReinforcement.Remove(CrossSectionSide.Bottom);
               edgesForReinforcement.Add(CrossSectionSide.Bottom, curentTuple);
            }
         }
         /// </structural_toolkit_2015>

         totalHeight = (geometryMaxY - geometryMinY);
         totalWidth = (geometryMaxX - geometryMinX);
         solver = RCSolver.CreateNewSolver(solverGeometry); // solver with geometry redy to use.
      }
      /// <summary>
      /// Create the new RC calculation object.
      /// </summary>
      /// <param name="type">The type of the cross-section</param>
      /// <param name="rcGeometry">Set of geometry parameters.</param>
      /// <param name="coverTop">The top cover - to the reinforcement ceneter.</param>
      /// <param name="coverBottom">The top cover - to the reinforcement ceneter.</param>
      /// <returns>New RcVerificationHelperUtility object.</returns>
      public static RcVerificationHelperUtility CreateRcVerificationHelperUtility(SectionShapeType type, ref Geometry rcGeometry, double coverTop, double coverBottom)
      {
         if (SectionShapeType.RectangularBar != type && SectionShapeType.T != type)
            throw new Exception("CreateRcVerificationHelperUtility.Unhandled cross section type. 3th party parameterization are necessary.");
         // If you need to take into account other section you should modyfy SetReinforcementAsBar & GetReinforcementLine before you remove this exception!
         RcVerificationHelperUtility newUtility = new RcVerificationHelperUtility(type, ref rcGeometry, coverTop, coverBottom);
         return newUtility;
      }
      /// <summary>
      /// Set reinforcement on every cross-section corner.
      /// </summary>
      /// <param name="oneRebarArea">Area of each rebar.</param>
      public void SetCornerReinforcement(double oneRebarArea)
      {
         SetReinforcementAsBar(oneRebarArea, oneRebarArea, 0.0, 0.0, 2, 2, 0, 0);
      }
      /// <summary>
      ///  Set reinforcement on the top and bottom of the section.
      /// </summary>
      /// <param name="topReinf">Area of top reinforcment.</param>
      /// <param name="bottomReinf">Area of bottom reinforcment.</param>
      public void SetReinforcement(double topReinf, double bottomReinf)
      {
         SetReinforcementAsBar(0.5 * topReinf, 0.5 * bottomReinf, 0.0, 0.0, 2, 2, 0, 0);
      }
      /// <summary>
      /// Set reinforcemenet on every side of cross-section.
      /// </summary>
      /// <param name="topReinf">Area of top reinforcment.</param>
      /// <param name="bottomReinf">Area of bottom reinforcment.</param>
      /// <param name="rightReinf">Area of right reinforcment.</param>
      /// <param name="leftReinf">Area of left reinforcment.</param>
      public void SetReinforcement(double topReinf, double bottomReinf, double rightReinf, double leftReinf)
      {
         int barNo = rebars.Count();
         int noRebarMax = 2 * (noTopBottom + noLeftRight);
         topReinf /= (double)noTopBottom;
         bottomReinf /= (double)noTopBottom;
         rightReinf /= (double)noLeftRight;
         leftReinf /= (double)noLeftRight;
         if (barNo == noRebarMax && Math.Abs(topReinf * bottomReinf * rightReinf * leftReinf) > Double.Epsilon)
         {
            for (int i = 0; i < barNo; i++)
            {
               switch (rebarsSide[i])
               {
                  case CrossSectionSide.Top:
                     rebars[i].Area = topReinf;
                     break;
                  case CrossSectionSide.Bottom:
                     rebars[i].Area = bottomReinf;
                     break;
                  case CrossSectionSide.Right:
                     rebars[i].Area = rightReinf;
                     break;
                  case CrossSectionSide.Left:
                     rebars[i].Area = leftReinf;
                     break;
               }
            }
            solver.SetRebars(rebars);
         }
         else
         {
            SetReinforcementAsBar(topReinf, bottomReinf, rightReinf, leftReinf, noTopBottom, noTopBottom, noLeftRight, noLeftRight);
         }
      }
      /// <summary>
      /// Set the concrete parameters for calculation.
      /// </summary>
      /// <param name="concrete">Set of reinforcement concrete parameters.</param>
      public void SetConcrete(Autodesk.CodeChecking.Concrete.Concrete concrete)
      {
         solver.SetConcrete(concrete);
      }
      /// <summary>
      /// Set the concrete parameters for calculation.
      /// </summary>
      /// <param name="stressStrainType">Stress-strain relationship. The type of the concrete mechanical behaviour.</param>
      /// <param name="strenght">Concrete strenght.</param>
      /// <param name="youngModulus">Modulus of elasticyty - Young modulus for concrete.</param>
      public void SetConcrete(Autodesk.CodeChecking.Concrete.StressDiagramType stressStrainType, double strenght, double youngModulus)
      {
         Autodesk.CodeChecking.Concrete.Concrete newConcrete = new Autodesk.CodeChecking.Concrete.Concrete();
         double MaximumStrain = 0.0035;            // The variable dependent to RC code.!
         double CompressionReductionFactor = 0.8;  // The variable dependent to RC code. Only for D_REC stressStrainType!
         switch (stressStrainType)
         {
            case Autodesk.CodeChecking.Concrete.StressDiagramType.Linear:
               newConcrete.SetStrainStressModelLinear(strenght, strenght / youngModulus, youngModulus);
               break;
            case Autodesk.CodeChecking.Concrete.StressDiagramType.Rectangular:
               newConcrete.SetStrainStressModelRectangular(strenght, MaximumStrain, youngModulus, CompressionReductionFactor);
               break;
            default:
               // Other cases could be necessary for some design codes.
               // Other variables dependent to RC code could be necessary for parameterization!
               throw new Exception("SetConcrete. Unhandled type. 3th party implementation are necessary in this point.");
         }
         SetConcrete(newConcrete);
      }
      /// <summary>
      /// Set the reinforcement steel parameters for calculation.
      /// </summary>
      /// <param name="strenght">Steel strenght.</param>
      /// <param name="modulusOfElasticity">Modulus of elasticyty - Young modulus for steel.</param>
      /// <param name="strainUltimateLimit">Limit of strain - maximum steel strain.</param>
      /// <param name="hardeningFactor">Hardening factor - increase of strength on the plastic behaviour part.</param>
      public void SetSteel(double strenght, double modulusOfElasticity, double strainUltimateLimit, double hardeningFactor)
      {
         Autodesk.CodeChecking.Concrete.Steel newSteel = new Autodesk.CodeChecking.Concrete.Steel();
         newSteel.ModulusOfElasticity = modulusOfElasticity;
         newSteel.DesignStrength = strenght;
         newSteel.HardeningFactor = hardeningFactor;      // The variable dependent to RC code.!
         // Sometimes 1.0 value(without Hardening)   is makes some problems with iteration process. 
         // Little value bigger the 1.0 is removed this problems and is safer.
         newSteel.StrainUltimateLimit = strainUltimateLimit;    // The variable dependent to RC code.!
         solver.SetSteel(newSteel);
         double yieldStrain = newSteel.DesignStrength / newSteel.ModulusOfElasticity;
         int minStep = (int)Math.Ceiling(newSteel.StrainUltimateLimit / yieldStrain);
         //noTopBottom = 2*Math.Max(5, minStep);
         //noLeftRight = minStep - 2;
      }
      /// <summary>
      /// Set identical rebars in the concrete cross section.
      /// </summary>
      /// <param name="oneBarArea">The area of a single bar.</param>
      /// <param name="noBarBottom">The number of bars placed on the bottom.</param>
      /// <param name="noBarTop">The number of bars placed on the top.</param>
      /// <param name="noBarLeft">The number of bars placed on the left</param>
      /// <param name="noBarRight">The number of bars placed on the right</param>
      void SetReinforcementAsBar(double oneBarArea, int noBarTop, int noBarBottom, int noBarRight, int noBarLeft)
      {
         SetReinforcementAsBar(oneBarArea * noBarTop, oneBarArea * noBarBottom, oneBarArea * noBarRight, oneBarArea * noBarLeft, noBarTop, noBarBottom, noBarRight, noBarLeft);
      }
      /// <summary>
      /// Set rebars in the concrete cross section.
      /// </summary>
      /// <param name="bottomBarArea">The area of reinforcement on the bottom of section.</param>
      /// <param name="topBarArea">The area of reinforcement on the top of section.</param>
      /// <param name="leftBarArea">The area of reinforcement on the left of section.</param>
      /// <param name="rightBarArea">The area of reinforcement on the right of section.</param>
      /// <param name="noBarBottom">The number of bars placed on the bottom.</param>
      /// <param name="noBarTop">The number of bars placed on the top.</param>
      /// <param name="noBarLeft">The number of bars placed on the left</param>
      /// <param name="noBarRight">The number of bars placed on the right</param>
      void SetReinforcementAsBar(double topBarArea, double bottomBarArea, double rightBarArea, double leftBarArea, int noBarTop, int noBarBottom, int noBarRight, int noBarLeft)
      {
         rebars.Clear();
         rebarsSide.Clear();
         Point2D BeginLinePoint = new Point2D(0, 0);
         Point2D EndLinePoint = new Point2D(0, 0);
         /// <structural_toolkit_2015>
         //BOTTOM
         if (bottomBarArea > 0.0)
         {
            BeginLinePoint = solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Bottom].Item1);
            EndLinePoint = solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Bottom].Item2);
            BeginLinePoint.X += rebarCover;
            EndLinePoint.X -= rebarCover;           
            BeginLinePoint.Y += rebarCoverBottom;
            EndLinePoint.Y += rebarCoverBottom;
            SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref bottomBarArea, ref noBarBottom, false, CrossSectionSide.Bottom);
         }
         //RIGHT
         if (rightBarArea > 0.0)
         {
            BeginLinePoint = solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Right].Item1);
            EndLinePoint = solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Right].Item2);
            BeginLinePoint.X -= rebarCover;
            EndLinePoint.X -= rebarCover;
            BeginLinePoint.Y += rebarCoverBottom;
            EndLinePoint.Y -= rebarCoverTop;
            SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref rightBarArea, ref noBarRight, true, CrossSectionSide.Right);
         }
         //TOP
         if (topBarArea > 0.0)
         {
            BeginLinePoint = solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Top].Item1);
            EndLinePoint = solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Top].Item2);
            BeginLinePoint.X += rebarCover;
            EndLinePoint.X -= rebarCover;
            BeginLinePoint.Y -= rebarCoverTop;
            EndLinePoint.Y -= rebarCoverTop;
            SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref topBarArea, ref noBarTop, false, CrossSectionSide.Top);
         }
         //LEFT
         if (leftBarArea > 0.0)
         {
            BeginLinePoint = solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Left].Item1);
            EndLinePoint = solverGeometry.Point(edgesForReinforcement[CrossSectionSide.Left].Item2);
            BeginLinePoint.X += rebarCover;
            EndLinePoint.X += rebarCover;
            BeginLinePoint.Y += rebarCoverBottom;
            EndLinePoint.Y -= rebarCoverTop;
            SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref leftBarArea, ref noBarLeft, true, CrossSectionSide.Left);
         }
         solver.SetRebars(rebars);
      }
      /// <summary>
      /// Set the rebar beetwin two points.
      /// </summary>
      /// <param name="beginPoint">First point of reinforcing line.</param>
      /// <param name="endPoint">Last point of reinforcing line.</param>
      /// <param name="oneBarArea">The area of reinforcement.</param>
      /// <param name="noBars">The number of bars placed on the line.</param>
      /// <param name="hasNoCornerBars">If true the rebar will be placed on the ends of line.</param>
      /// <param name="position">The information about classification of the rebar as top, bottom, right or left.</param>
      void SetReinforcementBarsOnLine(ref Point2D beginPoint, ref Point2D endPoint, ref double oneBarArea, ref int noBars, bool hasNoCornerBars, CrossSectionSide position)
      {
         if (noBars > 0 && oneBarArea > 0.0)
         {
            int noSpace = noBars + (hasNoCornerBars ? 1 : -1);
            double fdX = (endPoint.X - beginPoint.X);
            double fdY = (endPoint.Y - beginPoint.Y);
            fdX /= noSpace;
            fdY /= noSpace;
            if (hasNoCornerBars)
            {
               for (int i = 1; i < noSpace; i++)
               {
                  Rebar bar = new Rebar(beginPoint.X + i * fdX, beginPoint.Y + i * fdY, oneBarArea);
                  rebars.Add(bar);
                  rebarsSide.Add(position);
               }
            }
            else
            {
               for (int i = 0; i <= noSpace; i++)
               {
                  Rebar bar = new Rebar(beginPoint.X + i * fdX, beginPoint.Y + i * fdY, oneBarArea);
                  rebars.Add(bar);
                  rebarsSide.Add(position);
               }
            }
         }
      }
      /// <summary>
      /// Calculates the safety factor.
      /// </summary>
      /// <param name="inNMM">The acting forces.</param>
      /// <returns>Safety factor. Resistance forces to acting forces ratio.</returns>
      public double SafetyFactor(InternalForcesContainer inNMM)
      {
         double safetyFactor = -1.0;
         try
         {
            solver.SolveResistance(inNMM.ForceFx, -inNMM.MomentMy, inNMM.MomentMz);
         }
         catch (Exception e)
         {
            throw e;
         }
         SetOfForces solveNMM = solver.GetInternalForces(Autodesk.CodeChecking.Concrete.ResultType.Section);
         if (Math.Abs(inNMM.ForceFx) > Math.Abs(inNMM.MomentMy))
         {
            if (Math.Abs(inNMM.ForceFx) > Math.Abs(inNMM.MomentMz))
               safetyFactor = solveNMM.AxialForce / inNMM.ForceFx;
            else
               safetyFactor = solveNMM.MomentY / inNMM.MomentMz;
         }
         else if (Math.Abs(inNMM.MomentMy) > Math.Abs(inNMM.MomentMz))
         {
            safetyFactor = solveNMM.MomentX / -inNMM.MomentMy;
         }
         else
         {
            safetyFactor = solveNMM.MomentY / inNMM.MomentMz;
         }
         return safetyFactor;
      }
      /// <summary>
      /// Calculates the safety factor and sets additional result in the lists.
      /// </summary>
      /// <param name="inNMM">The acting forces.</param>
      /// <param name="concreteStresses">
      /// Reference to modify object. The list is set after safety factor calculation. Includes stresses on every corner of the cross section.
      /// </param>
      /// <param name="steelStresses">
      /// Reference to modify object. The list is set after safety factor calculation. Includes stresses on every rebar.
      /// </param>
      /// <returns>Safety factor. Resistance forces to acting forces ratio.</returns>
      public double SafetyFactor(InternalForcesContainer inNMM, ref List<double> concreteStresses, ref List<double> steelStresses)
      {
         double safetyFactor = SafetyFactor(inNMM);
         int no = solverGeometry.Count;
         double stress = 0;
         for (int i = 0; i < no; i++)
         {
            stress = solver.GetStress(Autodesk.CodeChecking.Concrete.ResultType.Concrete, i);
            concreteStresses.Add(stress);
         }
         no = solver.GetRebars().Count;
         for (int i = 0; i < no; i++)
         {
            stress = solver.GetStress(Autodesk.CodeChecking.Concrete.ResultType.Rebars, i);
            steelStresses.Add(stress);
         }
         return safetyFactor;
      }
      /// <structural_toolkit_2015>
  
      /// <summary>
      /// Calculate the moment of inertia for cracking section.
      /// </summary>
      /// <param name="inNMM">The acting forces.</param>
      /// <returns>Returns moment of inertia for cracking section.</returns>
      public double InertiaOfCrackingSection(InternalForcesContainer inNMM)
      {
         double momentOfInertiaCrackingConcreteSection = 0.0;
         SafetyFactor(inNMM);
         SetOfForces solverNMM = solver.GetInternalForces(Autodesk.CodeChecking.Concrete.ResultType.Section);
         double neutralAxisDist = solver.GetNeutralAxisDistance();
         double stressArea = solver.GetConcreteStressArea();
         double comprHeight = 0.5 * totalHeight + neutralAxisDist;
         Steel steel = solver.GetSteel();
         Autodesk.CodeChecking.Concrete.Concrete concrete = solver.GetConcrete();
         double n = steel.ModulusOfElasticity / concrete.ModulusOfElasticity;
         switch (crossSectionType)
         {
            case SectionShapeType.RectangularBar:
               {
                momentOfInertiaCrackingConcreteSection = comprHeight * comprHeight * stressArea / 3.0; // bh^3/12 + b*h*(0.5*h)^2, b*h=stressArea
               }
               break;
            default:
               throw new Exception("InertiaOfCrackingSection. Unhandled cross section type. Only rectangular cross-section can be used on this path. 3th party implementation is necessary.");
         }
         foreach (Rebar bar in solver.GetRebars())
         {
            momentOfInertiaCrackingConcreteSection += n * bar.Area * Math.Pow((bar.Y + neutralAxisDist),2);
         }
         return momentOfInertiaCrackingConcreteSection;
      }

      /// <summary>
      /// Calculate the acting forces to Cracking forces ratio.
      /// </summary>
      /// <param name="inNMM">The acting forces.</param>
      /// <param name="crackingStress">Stress limit for cracking/uncracking section.</param>
      /// <returns>Acting forces to cracking forces ratio</returns>
      public double ForcesToCrackingForces(InternalForcesContainer inNMM, double crackingStress)
      {
         double forcesToCrackigForces = 0;
         if (!CalculationUtility.IsZeroM(inNMM.MomentMz))
         {
            throw new Exception("Deflection calculation is not aviable for biaxial bending.");
         }
         double actingForcesStress = 0;
         if(!CalculationUtility.IsZeroM(inNMM.MomentMy))
         {
            double w = solverGeometry.MomentOfInertiaX;
            w /= inNMM.MomentMy > 0.0 ? (geometryMaxY - solverGeometry.CenterOfInertia.Y) : (solverGeometry.CenterOfInertia.Y - geometryMinY);
            actingForcesStress += Math.Abs(inNMM.MomentMy) / w;
         }
         if (!CalculationUtility.IsZeroN(inNMM.ForceFx))
         {
            actingForcesStress += -inNMM.ForceFx / solverGeometry.Area;
         }
         if (actingForcesStress >= 0)
         {
            forcesToCrackigForces = actingForcesStress / crackingStress ;
         }
         return forcesToCrackigForces;
      }

      /// </structural_toolkit_2015>
   }
}
