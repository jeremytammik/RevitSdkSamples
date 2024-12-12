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
      /// Case 11: Calculation of capacity state in symmetric section for bending moment My
      /// </summary>
      public void Case11()
      {
         // Case 11a
         // geometry definition
         Geometry geometry = new Geometry();
         geometry.Add(0.0, 0.0);
         geometry.Add(0.0, 0.3);
         geometry.Add(0.6, 0.3);
         geometry.Add(0.6, 0.0);
         // rebars definition
         List<Rebar> rebars = new List<Rebar>();
         double rebarArea = 0.032 * 0.032 * Math.PI / 4.0;
         rebars.Add(new Rebar(0.05, 0.05, rebarArea));
         rebars.Add(new Rebar(0.05, 0.25, rebarArea));
         rebars.Add(new Rebar(0.55, 0.25, rebarArea));
         rebars.Add(new Rebar(0.55, 0.05, rebarArea));
         // concrete parameters
         Concrete concrete = new Concrete();
         concrete.SetStrainStressModelLinear(20e6, 0.0035, 30e9);
         // steel parameters
         Steel steel = new Steel();
         steel.SetModelIdealElastoPlastic(500e6, 0.075, 200e9);
         // solver creation and parameterization
         RCSolver solver = RCSolver.CreateNewSolver(geometry);
         solver.SetRebars(rebars);
         solver.SetConcrete(concrete);
         solver.SetSteel(steel);
         //calulation
         solver.SolveResistanceM(0, Axis.y,false);
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
         sb.AppendLine("Case 11a:  Calculation of capacity state in symmetric section for bending moment My");
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

         // Case 11b
         // concrete parameters
         concrete.SetStrainStressModelBiLinear(30e6, 0.0035, 32e9, 0.0020);
         // steel parameters
         steel.DesignStrength = 400e6;
         steel.HardeningFactor = 1.0;
         steel.ModulusOfElasticity = 205e9;
         steel.StrainUltimateLimit = 0.1;
         // solver parameterization
         solver.SetConcrete(concrete);
         solver.SetSteel(steel);
         //calulation
         solver.SolveResistanceM(0, Axis.y, false);
         // result for rebars
         forcesRebar = solver.GetInternalForces(ResultType.Rebars);
         // result for concrete
         forcesConcrete = solver.GetInternalForces(ResultType.Concrete);
         Gcc = solver.GetStressGravityCenter(ResultType.Concrete);
         Acc = solver.GetConcreteStressArea();
         // result for RC section
         forces = solver.GetInternalForces(ResultType.Section);
         // result presentation
         sb.AppendLine();
         sb.AppendLine(decoration);
         sb.AppendLine("Case 11b: Calculation of capacity state in symmetric section for bending moment My ");
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

         // Case 11c
         // concrete parameters
         concrete.SetStrainStressModelPowerRectangular(30e6, 0.0035, 32e9, 0.002, 1.8);
         // steel parameters
         steel.DesignStrength = 400e6;
         steel.HardeningFactor = 1.0;
         steel.ModulusOfElasticity = 200e9;
         steel.StrainUltimateLimit = 0.1;
         // solver parameterization
         solver.SetConcrete(concrete);
         solver.SetSteel(steel);
         //calulation
         solver.SolveResistanceM(0, Axis.y, false);
         // result for rebars
         forcesRebar = solver.GetInternalForces(ResultType.Rebars);
         // result for concrete
         forcesConcrete = solver.GetInternalForces(ResultType.Concrete);
         Gcc = solver.GetStressGravityCenter(ResultType.Concrete);
         Acc = solver.GetConcreteStressArea();
         // result for RC section
         forces = solver.GetInternalForces(ResultType.Section);
         // result presentation
         sb.AppendLine();
         sb.AppendLine(decoration);
         sb.AppendLine("Case 11c: Calculation of capacity state in symmetric section for bending moment My ");
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
      }
   }
}
