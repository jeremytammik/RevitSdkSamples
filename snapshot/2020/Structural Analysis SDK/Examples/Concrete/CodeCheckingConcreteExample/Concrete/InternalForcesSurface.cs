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

/// <structural_toolkit_2015>
namespace CodeCheckingConcreteExample.Concrete
{
   class InternalForcesSurface : InternalForcesBase
   {
      public InternalForcesSurface()
      {
         ForceFxx = 0.0; ForceFyy = 0.0; ForceFxy = 0.0;
         MomentMxx = 0.0; MomentMyy = 0.0; MomentMxy = 0.0;
         LimitState = ForceLimitState.Unknown;
         ForceDescription = "";
      }

      public InternalForcesContainer Forces(ConcreteTypes.DimensioningDirection direction)
      {
         InternalForcesContainer forces = new InternalForcesContainer();

         forces.CaseName = ForceDescription;
         forces.LimitState = LimitState;
         
         if (direction == ConcreteTypes.DimensioningDirection.X)
         {
            forces.ForceFx = -ForceFxx;   // Due to ResultBuilder conventions of the forces sign.
            forces.MomentMy = -MomentMxx; // Due to ResultBuilder conventions of the forces sign.
            forces.ForceFz = ForceQxx;
         }
         else
         {
            forces.ForceFx = -ForceFyy;   // Due to ResultBuilder conventions of the forces sign.
            forces.MomentMy = -MomentMyy; // Due to ResultBuilder conventions of the forces sign.
            forces.ForceFz = ForceQyy;
         }

         return forces;
      }

      public double ForceFxx { get; set; }
      public double ForceFyy { get; set; }
      public double ForceFxy {  get; set; }
      public double MomentMxx {  get; set; }
      public double MomentMyy {  get; set; }
      public double MomentMxy {  get; set; }
      public double ForceQxx { get; set; }
      public double ForceQyy { get; set; }
      public ForceLimitState LimitState { get; set; }
      public string ForceDescription {  get; set; }
   }
}
/// </structural_toolkit_2015>
