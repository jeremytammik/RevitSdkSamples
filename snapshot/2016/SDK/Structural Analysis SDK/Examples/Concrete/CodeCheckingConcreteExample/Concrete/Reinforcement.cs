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

namespace CodeCheckingConcreteExample.Concrete
{
   /// <summary>
   /// Class describe reinforcement data and results. 
   /// </summary>
   public class Reinforcement
   {
      private double spacing, asTop, asBottom, asRight, asLeft;
      private double strength;
      private double designStrength;
      private double transversalDensity;

      private Reinforcement() { }
      /// <summary>
      /// Create reinforcement object based on steel parameters.
      /// </summary>
      /// <param name="steelStrenght">The design strenght of steel.</param>
      public Reinforcement(double steelStrenght)
      {
         Reset();
         strength = steelStrenght;
         designStrength = strength;
         ModulusOfElasticity = 200e9;
      }

      /// <summary>
      /// Set all variables in the <see cref="Reinforcement"/> object to defult valus: areas to 0.0 spacig to float.MaxValue
      /// </summary>
      public void Reset()
      {
         spacing = float.MaxValue;
         asTop = asBottom = asRight = asLeft = 0.0;
         CurrentAsTop = CurrentAsBottom = CurrentAsRight = CurrentAsLeft = 0.0;
         CurrentSpacing = float.MaxValue;
      }

      /// <summary>
      /// Gets or sets the current (temporary) stirupp spacing.
      /// </summary>
      public double CurrentSpacing { get; set; }

      /// <summary>
      ///  Gets or sets the current (temporary) area of top reinforcement.
      /// </summary>
      public double CurrentAsTop { get; set; }

      /// <summary>
      /// Gets or sets the current (temporary) area of bottom reinforcement.
      /// </summary>
      public double CurrentAsBottom { get; set; }

      /// <summary>
      /// Gets or sets the current (temporary) area of right reinforcement.
      /// </summary>
      public double CurrentAsRight { get; set; }

      /// <summary>
      /// Gets or sets the current (temporary) area of left reinforcement.
      /// </summary>
      public double CurrentAsLeft { get; set; }

      /// <summary>
      /// Gets or sets the modulus of elasticyty (Young modulus).
      /// </summary>
      public double ModulusOfElasticity { get; set; }

      /// <summary>
      /// Gets the stirupp spacing (finial).
      /// </summary>
      public double Spacing { get { return spacing; } }

      /// <summary>
      /// Gets the area of top reinforcement (finial).
      /// </summary>
      public double AsTop { get { return asTop; } }

      /// <summary>
      /// Gets the area of bottom reinforcement (finial).
      /// </summary>
      public double AsBottom { get { return asBottom; } }

      /// <summary>
      /// Gets the area of right reinforcement (finial).
      /// </summary>
      public double AsRight { get { return asRight; } }

      /// <summary>
      /// Gets the area of left reinforcement (finial).
      /// </summary>
      public double AsLeft { get { return asLeft; } }

      /// <summary>
      /// Gets the design strenght.
      /// </summary>
      public double Strength { get { return designStrength; } }

      /// <summary>
      /// Sets design strength according to current limit state. The strength is divided by safety factor.
      /// </summary>
      /// <param name="factor">Safety factor</param>
      public void SetStrenghtPartialFactor(double factor)
      {
         designStrength = strength / factor;
      }

      /// <summary>
      /// Sets trensversal reinforcement density acording to number of stirrupas arms, transversal bar area and spacuing.
      /// </summary>
      /// <param name="numberOfStirrupsArms">The number of arms in one frame of stirrup.</param>
      /// <param name="oneArmArea">The area of one transversal bar. One of stirrups arms.</param>
      public void SetTransversalDensity(int numberOfStirrupsArms, double oneArmArea)
      {
         transversalDensity = (oneArmArea * numberOfStirrupsArms) / spacing;
      }

      /// <summary>
      /// Gets the stirrups dencity 
      /// </summary>
      public double TransversalDensity { get { return transversalDensity; } }

      /// <summary>
      /// Calculates total reinforcement area. 
      /// </summary>
      /// <returns>Returns sum of reinforcement area.</returns>
      public double TotalSectionReinforcement()
      {
         return asTop + asBottom + asRight + asLeft;
      }

      /// <summary>
      /// Sets finial resulat <see cref="Spacing"/>, <see cref="AsTop"/>, <see cref="AsBottom"/>, <see cref="AsRight"/>, <see cref="AsLeft"/> )  based on curent valus. 
      /// </summary>
      /// <remarks>
      /// <para>For longitudinal reinforcement: the final areas are set as maksimum of current value and previous final value. </para>
      /// <para>For transversal reinforcement: the final areas are set as sum of current value and previous final value,
      /// final spacing is set as minimum of current spacing and previous final spacing.</para>
      /// </remarks>
      public void CurrentToFinial()
      {
         spacing = Math.Min(CurrentSpacing, spacing);
         asTop = Math.Max(CurrentAsTop, asTop);
         asBottom = Math.Max(CurrentAsBottom, asBottom);
         asRight = Math.Max(CurrentAsRight, asRight);
         asLeft = Math.Max(CurrentAsLeft, asLeft);
      }

   }
}
