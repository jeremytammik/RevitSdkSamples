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

namespace CodeCheckingConcreteExample.Concrete
{
   /// <summary>
   /// Simple container class for all possible internal forces in section
   /// </summary>
   public class InternalForcesContainer
   {
      /// <summary>
      /// Initializes a new instance of InternalForcesContainer object with default 0.0 values.
      /// </summary>
      public InternalForcesContainer()
      {
         ForceFx = 0.0; ForceFy = 0.0; ForceFz = 0.0; MomentMx = 0.0; MomentMy = 0.0; MomentMz = 0.0; DeflectionUx = 0.0; DeflectionUy = 0.0; DeflectionUz = 0.0;
         LimitState = ForceLimitState.Unknown;
         CaseName = "";
      }
      /// <summary>
      /// Gets or sets the force Fx (axial force).
      /// </summary>
      public double ForceFx { get; set; }

      /// <summary>
      /// Gets or sets the force Fy.
      /// </summary>
      public double ForceFy { get; set; }

      /// <summary>
      /// Gets or sets the force Fz (main shear force).
      /// </summary>
      public double ForceFz { get; set; }

      /// <summary>
      /// Gets or sets the moment Mz (torsion moment).
      /// </summary>
      public double MomentMx { get; set; }

      /// <summary>
      /// Gets or sets the moment My (main bending moment).
      /// </summary>
      public double MomentMy { get; set; }

      /// <summary>
      /// Gets or sets the moment My (main bending moment).
      /// </summary>
      public double MomentMz { get; set; }

      /// <summary>
      /// Gets or sets deflection Ux
      /// </summary>
      public double DeflectionUx { get; set; }

      /// <summary>
      /// Gets or sets deflection Uy
      /// </summary>
      public double DeflectionUy { get; set; }

      /// <summary>
      /// Gets or sets deflection Uz
      /// </summary>
      public double DeflectionUz { get; set; }


      /// <summary>
      /// Gets or sets the limit state.
      /// </summary>
      public ForceLimitState LimitState { get; set; }

      /// <summary>
      /// Gets or sets description of combination or case.
      /// </summary>
      public string CaseName { get; set; }
   }
}
