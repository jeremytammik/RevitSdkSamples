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
      /// Case 7 Calculation of internal forces for given state of strain in the section and inclined branch model of steel
      /// </summary>
      public void Case7()
      {
         // geometry definition
         Geometry geometry = new Geometry();
         geometry.Add(0.0, 0.0);
         geometry.Add(0.0, 0.6);
         geometry.Add(0.3, 0.6);
         geometry.Add(0.3, 0.0);
         // rebars definition
         List<Rebar> rebars = new List<Rebar>();
         rebars.Add(new Rebar(0.05, 0.05, 0.032 * 0.032 * Math.PI / 4.0));
         rebars.Add(new Rebar(0.15, 0.05, 0.032 * 0.032 * Math.PI / 4.0));
         rebars.Add(new Rebar(0.25, 0.05, 0.032 * 0.032 * Math.PI / 4.0));
         rebars.Add(new Rebar(0.05, 0.15, 0.020 * 0.020 * Math.PI / 4.0));
         rebars.Add(new Rebar(0.25, 0.15, 0.020 * 0.020 * Math.PI / 4.0));
         rebars.Add(new Rebar(0.05, 0.55, 0.012 * 0.012 * Math.PI / 4.0));
         rebars.Add(new Rebar(0.25, 0.55, 0.012 * 0.012 * Math.PI / 4.0));
         // concrete parameters
         Concrete concrete = new Concrete();
         concrete.SetStrainStressModelLinear(20e6, 0.0035, 30e9);
         // steel parameters
         Steel steel = new Steel();
         steel.SetModelWithHardening(500e6, 0.075, 200e9, 1.05);
         // solver creation and parameterization
         RCSolver solver = RCSolver.CreateNewSolver(geometry);
         solver.SetRebars(rebars);
         solver.SetConcrete(concrete);
         solver.SetSteel(steel);
         //calulation
         solver.SolveForces(ResultType.Section,0.0035,-0.0070);
         // result for rebars
         SetOfForces forcesRebar = solver.GetInternalForces(ResultType.Rebars);
         // result for concrete
         SetOfForces forcesConcrete = solver.GetInternalForces(ResultType.Concrete);
         Point2D Gcc = solver.GetStressGravityCenter(ResultType.Concrete);
         double Acc = solver.GetConcreteStressArea();
         // result for RC section
         SetOfForces forces = solver.GetInternalForces(ResultType.Section);
         // result presentation
         sb.AppendLine();
         sb.AppendLine(decoration);
         sb.AppendLine("Case 7 Calculation of internal forces for given state of strain in the section and inclined branch model of steel ");
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
