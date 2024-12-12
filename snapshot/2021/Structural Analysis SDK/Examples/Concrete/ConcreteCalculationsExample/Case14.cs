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
using Autodesk.CodeChecking.Concrete;

namespace ConcreteCalculationsExample
{
   partial class Example
   {
         
      /// <summary>
      /// Case 14: Calculation of capacity state in symmetric section for bidirectional bending with axial force
      /// </summary>
      public void Case14()
      {
         // geometry definition
         Geometry geometry = new Geometry();
         geometry.Add(0.0, 0.0);
         geometry.Add(0.3, 0.0);
         geometry.Add(0.3, 0.45);
         geometry.Add(0.0, 0.45);
         // rebars definition
         List<Rebar> rebars = new List<Rebar>();
         rebars.Add(new Rebar(0.15, 0.04, 0.025977 * 0.025977 * Math.PI / 4.0));
         // concrete parameters
         Concrete concrete = new Concrete();
         concrete.SetStrainStressModelRectangular(21.36e6, 0.0035, 35e9, 0.8);
         // steel parameters
         Steel steel = new Steel();
         steel.SetModelIdealElastoPlastic(310e6, 0.01, 200e9);
         // solver creation and parameterization
         RCSolver solver = RCSolver.CreateNewSolver(geometry);
         solver.SetRebars(rebars);
         solver.SetConcrete(concrete);
         solver.SetSteel(steel);
         //calulation
         solver.SolveResistance(200E3, -96E3, -24E3);
         // result for rebars
         SetOfForces forcesRebar = solver.GetInternalForces(ResultType.Rebars);
         // result for concrete
         SetOfForces forcesConcrete = solver.GetInternalForces(ResultType.Concrete);
         Point2D Gcc = solver.GetStressGravityCenter(ResultType.Concrete);
         double Acc = solver.GetConcreteStressArea();
         // result for RC section
         SetOfForces forces = solver.GetInternalForces(ResultType.Section);
         double angle = solver.GetNeutralAxisAngle();
         double dist = solver.GetNeutralAxisDistance();
         // result presentation
         sb.AppendLine(); 
         sb.AppendLine(decoration);
         sb.AppendLine("Case 14: Calculation of capacity state in symmetric section for bidirectional bending with axial force ");
         sb.AppendLine(decoration);
         sb.AppendLine(FormatOutput("Ns",forcesRebar.AxialForce,6));
         sb.AppendLine(FormatOutput("Mxs" , forcesRebar.MomentX,6));
         sb.AppendLine(FormatOutput("Mys",forcesRebar.MomentY,6));
         sb.AppendLine(FormatOutput("Ac",Acc,6));
         sb.AppendLine(FormatOutput("Gcx",Gcc.X,6));
         sb.AppendLine(FormatOutput("Gcy",Gcc.Y,6));
         sb.AppendLine(FormatOutput("Nc",forcesConcrete.AxialForce,6));
         sb.AppendLine(FormatOutput("Mxc" , forcesConcrete.MomentX,6));
         sb.AppendLine(FormatOutput("Myc",forcesConcrete.MomentY,6));
         sb.AppendLine(FormatOutput("N",forces.AxialForce,6));
         sb.AppendLine(FormatOutput("Mx" , forces.MomentX,6));
         sb.AppendLine(FormatOutput("My",forces.MomentY,6));
         sb.AppendLine(FormatOutput("dist",dist,6));
         sb.AppendLine(FormatOutput("angle",angle,6));
      }
   }
}
