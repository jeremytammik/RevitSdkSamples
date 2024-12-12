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

      const double geometryEpsilon = 1e-6; // tolerance for polyline which describes of section
      int noTopBottom;                     // the number of bars on the top or bottom side with corner bars
      int noLeftRight;                     // the number of bars on the top or bottom side without corner bars
      List<Rebar> rebars;                  // list of rebars in the cross-section
      List<CrossSectionSide> rebarsSide;   // list of bars position in the cross-section
      SectionShapeType crossSectionType;   // the type of cross-section
      double rebarCoverTop;                // top cover from the edge to the gravity center of bars 
      double rebarCoverBottom;             // bottom cover from the edge to the gravity center of bars
      double rebarCover;                   // the greater cover from the edge to the gravity center of bars
      Geometry solverGeometry;             // geometry of cross section for solver object
      RCSolver solver;                     // solver
      /// <summary>
      /// Initializes a new instance of the new RC calculation helper object.
      /// </summary>
      /// <param name="type">The type of the cross-section</param>
      /// <param name="rcGeometry">Set of geometry parameters.</param>
      /// <param name="coverTop">The top cover - to the reinforcement ceneter.</param>
      /// <param name="coverBottom">The top cover - to the reinforcement ceneter.</param>
      private RcVerificationHelperUtility(SectionShapeType type, ref Geometry rcGeometry, double coverTop, double coverBottom)
      {
         double totalHeight = 0;                         // the height of the cross section
         double totalWidth = 0;                          // the width of the cross section
         noTopBottom = 5;                                // initial value of the number of bars on top and bottom
         noLeftRight = noTopBottom - 2;                  // initial value of the number of bars on left and right
         solverGeometry = new Geometry();                // initialization of new geometry
         rebars = new List<Rebar>();                     // initialization of rebars list 
         rebarsSide = new List<CrossSectionSide>();      // initialization of bars position list 
         crossSectionType = type;                        // set of section type
         rebarCover = Math.Max(coverTop, coverBottom);   // set of maximum cover
         rebarCoverTop = coverTop;                       // set of top cover      
         rebarCoverBottom = coverBottom;                 // set of bottom cover
         // In the input geometry left bottom corner is searched.
         // The maximum and minimum values for x and y coordinates
         int leftBottomIndex = 0;
         double x = Double.MaxValue;
         double y = Double.MaxValue;
         double xMin = Double.MaxValue;
         double yMin = Double.MaxValue;
         double xMax = Double.MinValue;
         double yMax = Double.MinValue;
         int i = 0;
         int count = rcGeometry.Count;
         Point2D p = new Point2D(0, 0);
         for (i = 0; i < count; i++)
         {
            p = rcGeometry.Point(i);
            xMin = Math.Min(xMin, p.X);
            yMin = Math.Min(yMin, p.Y);
            xMax = Math.Max(xMax, p.X);
            yMax = Math.Max(yMax, p.Y);
            if ((p.X - x) < geometryEpsilon)
            {
               if ((p.Y - y) < geometryEpsilon)
               {
                  y = p.Y;
                  x = p.X;
                  leftBottomIndex = i;
               }
            }
         }
         totalHeight = (yMax - yMin);
         totalWidth = (xMax - xMin);
         // The first point of new geometry will be in the bottom left point of geometry.
         // This is made for easy detailing.
         if (0 != leftBottomIndex)
         {
            solverGeometry.Clear();
            for (i = leftBottomIndex; i < count; i++)
            {
               p = rcGeometry.Point(i);
               solverGeometry.Add(p.X, p.Y);
            }
            count = leftBottomIndex;
            for (i = 0; i < count; i++)
            {
               p = rcGeometry.Point(i);
               solverGeometry.Add(p.X, p.Y);
            }
         }
         else
            solverGeometry = rcGeometry;
         // The orientation is changed if it is necessary.
         if (solverGeometry.isClockwiseOrientation()) // clockwise direction
         {
            count = solverGeometry.Count - 1;
            Geometry tmpGeometry = new Geometry();
            p = solverGeometry.Point(0);
            tmpGeometry.Add(p.X, p.Y);
            for (i = count; i > 0; i--)
            {
               p = solverGeometry.Point(i);
               tmpGeometry.Add(p.X, p.Y);
            }
            solverGeometry = tmpGeometry;
         }
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
         switch (crossSectionType)
         {
            case SectionShapeType.RectangularBar:
               {
                  //BOTTOM
                  if (bottomBarArea > 0.0)
                  {
                     GetReinforcementLine(0, ref BeginLinePoint, ref EndLinePoint);
                     SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref bottomBarArea, ref noBarBottom, false, CrossSectionSide.Bottom);
                  }
                  //RIGHT
                  if (rightBarArea > 0.0)
                  {
                     GetReinforcementLine(1, ref BeginLinePoint, ref EndLinePoint);
                     SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref rightBarArea, ref noBarRight, true, CrossSectionSide.Right);
                  }
                  //TOP
                  if (topBarArea > 0.0)
                  {
                     GetReinforcementLine(2, ref BeginLinePoint, ref EndLinePoint);
                     SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref topBarArea, ref noBarTop, false, CrossSectionSide.Top);
                  }
                  //LEFT
                  if (leftBarArea > 0.0)
                  {
                     GetReinforcementLine(3, ref BeginLinePoint, ref EndLinePoint);
                     SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref leftBarArea, ref noBarLeft, true, CrossSectionSide.Left);
                  }
               }
               break;
            case SectionShapeType.T:
               {
                  //BOTTOM
                  if (bottomBarArea > 0.0)
                  {
                     GetReinforcementLine(0, ref BeginLinePoint, ref EndLinePoint); // Bottom, points 0-1
                     SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref bottomBarArea, ref noBarBottom, false, CrossSectionSide.Bottom);
                  }
                  //RIGHT
                  if (rightBarArea > 0.0)
                  {
                     int noBarRightTop = (int)Math.Ceiling(0.5 * noBarRight);
                     noBarRight -= noBarRightTop;
                     noBarRight -= 1;
                     GetReinforcementLine(2, ref BeginLinePoint, ref EndLinePoint); // Right-bottom, points 2-3
                     SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref rightBarArea, ref noBarRight, false, CrossSectionSide.Right);
                     GetReinforcementLine(3, ref BeginLinePoint, ref EndLinePoint); // Right-top, points 3-4
                     Rebar barR = new Rebar(EndLinePoint.X, EndLinePoint.Y, rightBarArea);
                     rebars.Add(barR);
                     rebarsSide.Add(CrossSectionSide.Right);
                  }
                  //TOP
                  if (topBarArea > 0.0)
                  {
                     GetReinforcementLine(4, ref BeginLinePoint, ref EndLinePoint); // Top, points 4-5
                     SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref topBarArea, ref noBarTop, false, CrossSectionSide.Top);
                  }
                  //LEFT
                  if (leftBarArea > 0.0)
                  {
                     int noBarLeftTop = (int)Math.Ceiling(0.5 * noBarLeft);
                     noBarLeft -= noBarLeftTop;
                     GetReinforcementLine(5, ref BeginLinePoint, ref EndLinePoint); // Left-top, points 5-6
                     Rebar barL = new Rebar(EndLinePoint.X, EndLinePoint.Y, leftBarArea);
                     rebars.Add(barL);
                     rebarsSide.Add(CrossSectionSide.Left);
                     noBarLeft = -1;
                     GetReinforcementLine(7, ref BeginLinePoint, ref EndLinePoint); // Left-bottom, points 7-0
                     SetReinforcementBarsOnLine(ref BeginLinePoint, ref EndLinePoint, ref leftBarArea, ref noBarLeft, false, CrossSectionSide.Left);
                  }
               }
               break;
            default:
               throw new Exception("SetReinforcementAsBar. Unhandled cross section type. 3th party parameterization are necessary in this point.");
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
      /// Calculates the line where were the reinforcement bars can be placed including the  cover.
      /// </summary>
      /// <param name="noSide">The number of section side. First is on the bottom.</param>
      /// <param name="firstPoint">Reference to modify object. The first point of line where the reinforcement can be placed.</param>
      /// <param name="secondPoint">Reference to modify object.The last point of line where the reinforcement can be placed.</param>
      void GetReinforcementLine(int noSide, ref Point2D firstPoint, ref Point2D secondPoint)
      {
         int iPointNo = solverGeometry.Count - 1;
         if (noSide <= iPointNo)
         {
            firstPoint.X = solverGeometry.Point(noSide).X;
            firstPoint.Y = solverGeometry.Point(noSide).Y;
            if (noSide == iPointNo)
            {
               secondPoint.X = solverGeometry.Point(0).X;
               secondPoint.Y = solverGeometry.Point(0).Y;
            }
            else
            {
               secondPoint.X = solverGeometry.Point(noSide + 1).X;
               secondPoint.Y = solverGeometry.Point(noSide + 1).Y;
            }
            switch (crossSectionType)
            {
               case SectionShapeType.RectangularBar:
                  switch (noSide)
                  {
                     case 0:
                        //                |*_________*|
                        //                (F)         (S)
                        firstPoint.X += rebarCover;
                        firstPoint.Y += rebarCover;
                        secondPoint.X -= rebarCover;
                        secondPoint.Y += rebarCover;
                        break;
                     case 1:
                        //                 _
                        //                * |(S)
                        //                  |
                        //                *_|(F)
                        firstPoint.X -= rebarCover;
                        firstPoint.Y += rebarCover;
                        secondPoint.X -= rebarCover;
                        secondPoint.Y -= rebarCover;
                        break;
                     case 2:
                        //                (S)      (F)
                        //                 _________
                        //                |*       *|
                        firstPoint.X -= rebarCover;
                        firstPoint.Y -= rebarCover;
                        secondPoint.X += rebarCover;
                        secondPoint.Y -= rebarCover;
                        break;
                     case 3:
                        //             (F) _
                        //                | *
                        //                |
                        //             (S)|_*
                        firstPoint.X += rebarCover;
                        firstPoint.Y -= rebarCover;
                        secondPoint.X += rebarCover;
                        secondPoint.Y += rebarCover;
                        break;
                  }
                  break;
               case SectionShapeType.T:
                  switch (noSide)
                  {
                     case 0:
                        //                |*_________*|
                        //                (F)         (S)
                        firstPoint.X += rebarCover;
                        firstPoint.Y += rebarCover;
                        secondPoint.X -= rebarCover;
                        secondPoint.Y += rebarCover;
                        break;
                     case 1:
                     //                *  _
                     //                  |(S)
                     //                  |
                     //                *_|(F)
                     case 3:
                        //                 _
                        //                * |(S)
                        //                  |
                        //                *_|(F)
                        firstPoint.X -= rebarCover;
                        firstPoint.Y += rebarCover;
                        secondPoint.X -= rebarCover;
                        secondPoint.Y += rebarCover;
                        break;
                     case 2:
                        //               * _________*|
                        //                |(F)        (S)
                        firstPoint.X -= rebarCover;
                        firstPoint.Y += rebarCover;
                        secondPoint.X -= rebarCover;
                        secondPoint.Y += rebarCover;
                        break;
                     case 4:
                        //                (S)      (F)
                        //                 _________
                        //                |*       *|
                        firstPoint.X -= rebarCover;
                        firstPoint.Y -= rebarCover;
                        secondPoint.X += rebarCover;
                        secondPoint.Y -= rebarCover;
                        break;
                     case 5:
                        //             (F) _
                        //                | *
                        //                |
                        //             (S)|_*
                        firstPoint.X += rebarCover;
                        firstPoint.Y -= rebarCover;
                        secondPoint.X += rebarCover;
                        secondPoint.Y += rebarCover;
                        break;
                     case 6:
                     //     |*_________ *
                     //     (F)        |(S)
                     case 7:
                        //            (F)_  *
                        //                |
                        //                |
                        //             (S)|_*
                        firstPoint.X += rebarCover;
                        firstPoint.Y += rebarCover;
                        secondPoint.X += rebarCover;
                        secondPoint.Y += rebarCover;
                        break;
                  }
                  break;
               default:
                  throw new Exception("GetReinforcementLine. Unhandled cross section type. 3th party parameterization are necessary in this point.");
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
         solver.SolveResistance(inNMM.ForceFx, -inNMM.MomentMy, inNMM.MomentMz);
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
      /// <summary>
      /// Calculate the moment of inertia for cracking section.
      /// </summary>
      /// <param name="inNMM">The acting forces.</param>
      /// <returns>Returns moment of inertia for cracking section.</returns>
      public double InertiaOfCrackingSection(InternalForcesContainer inNMM)
      {
         double momentOfInertiaCrackingConcreteSection = 0.0;
         solver.SolveResistance(inNMM.ForceFx, inNMM.MomentMy, 0.0);
         SetOfForces solverNMM = solver.GetInternalForces(Autodesk.CodeChecking.Concrete.ResultType.Section);
         double neutralAxisDist = solver.GetNeutralAxisDistance();
         double centerOfInertiaY = solverGeometry.CenterOfInertia.Y;
         double stressArea = solver.GetConcreteStressArea();
         double comprHeight = -neutralAxisDist;
         Steel steel = solver.GetSteel();
         Autodesk.CodeChecking.Concrete.Concrete concrete = solver.GetConcrete();
         double bottomWidth = solverGeometry.Point(1).X - solverGeometry.Point(0).X;
         double n = steel.ModulusOfElasticity / concrete.ModulusOfElasticity;
         momentOfInertiaCrackingConcreteSection = comprHeight * comprHeight * comprHeight * bottomWidth / 3.0; // bh^3/12 + b*h*(0.5*h)^2
         momentOfInertiaCrackingConcreteSection += n * (solver.GetMomentOfInertiaX(Autodesk.CodeChecking.Concrete.ResultType.Rebars) + Math.Abs(neutralAxisDist) * solver.GetArea(Autodesk.CodeChecking.Concrete.ResultType.Rebars));
         switch (crossSectionType)
         {
            case SectionShapeType.RectangularBar:
               break;
            case SectionShapeType.T:
               {
                  bool bottomMoreCompression = inNMM.MomentMy >= 0.0;
                  double TotalWidth = solverGeometry.Point(4).X - solverGeometry.Point(5).X;
                  if (bottomMoreCompression)
                  {
                     double WithoutSlabHeight = solverGeometry.Point(2).Y - solverGeometry.Point(1).Y;
                     if (comprHeight > WithoutSlabHeight)
                     {
                        double partT = (comprHeight - WithoutSlabHeight);
                        momentOfInertiaCrackingConcreteSection += Math.Pow(comprHeight - 0.5 * partT, 2) * (TotalWidth - bottomWidth);
                        momentOfInertiaCrackingConcreteSection += (TotalWidth - bottomWidth) * partT * partT * partT / 12.0;
                     }
                  }
                  else
                  {
                     double SlabHeight = solverGeometry.Point(4).Y - solverGeometry.Point(3).Y;
                     SlabHeight = Math.Min(SlabHeight, comprHeight);
                     momentOfInertiaCrackingConcreteSection += Math.Pow(comprHeight - 0.5 * SlabHeight, 2) * (TotalWidth - bottomWidth);
                     momentOfInertiaCrackingConcreteSection += (TotalWidth - bottomWidth) * SlabHeight * SlabHeight * SlabHeight / 12.0;
                  }
               }
               break;
            default:
               throw new Exception("InertiaOfCrackingSection. Unhandled cross section type. Only R and T cross-sections can be used on this path.");
         }
         return momentOfInertiaCrackingConcreteSection;
      }
      /// <summary>
      /// Calculate the acting forces to Cracking forces ratio.
      /// </summary>
      /// <param name="inNMM">The acting forces.</param>
      /// <param name="crackingStress">Stress limit for cracking/uncracking section.</param>
      /// <returns></returns>
      public double ForcesToCrackingForces(InternalForcesContainer inNMM, double crackingStress)
      {
         if (!CalculationUtility.IsZeroM(inNMM.MomentMz))
         {
            throw new Exception("Deflection calculation is not aviable for biaxial bending.");
         }
         double crackigForcesToForces = Double.MaxValue;
         solver.SolveResistance(inNMM.ForceFx, inNMM.MomentMy, 0);
         double strainMin = solver.GetStrainMin(Autodesk.CodeChecking.Concrete.ResultType.Concrete);
         if (strainMin < 0.0)
         {
            double ActingForceToResistance = 1.0;
            SetOfForces ResistanceForces = solver.GetInternalForces(Autodesk.CodeChecking.Concrete.ResultType.Section);
            if (Math.Abs(inNMM.MomentMy) > Math.Abs(inNMM.ForceFx))
            {
               ActingForceToResistance = inNMM.MomentMy / ResistanceForces.MomentX;
            }
            else
            {
               ActingForceToResistance = inNMM.ForceFx / ResistanceForces.AxialForce;
            }
            Autodesk.CodeChecking.Concrete.Concrete concrete = solver.GetConcrete();
            double noCrackingTensonStress = -strainMin * concrete.ModulusOfElasticity * ActingForceToResistance;
            crackigForcesToForces = crackingStress / noCrackingTensonStress;
         }
         return crackigForcesToForces;
      }
   }
}
