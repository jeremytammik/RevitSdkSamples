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
      /// Case 15: Calculation of capacity state in asymmetric section for bidirectional bending with axial force
      /// </summary>
      public void Case15()
      {
         // Case 15a
         // geometry definition
         Geometry geometry = new Geometry();
         geometry.Add(0.00, 0.00);
         geometry.Add(0.00, 0.60);
         geometry.Add(0.25, 0.60);
         geometry.Add(0.25, 0.25);
         geometry.Add(0.70, 0.25);
         geometry.Add(0.70, 0.00);
         // rebars definition
         List<Rebar> rebars = new List<Rebar>();
         double rebarArea = 0.020 * 0.020 * Math.PI / 4.0;
         rebars.Add(new Rebar(0.05, 0.05, rebarArea));
         rebars.Add(new Rebar(0.05, 0.55, rebarArea));
         rebars.Add(new Rebar(0.20, 0.55, rebarArea));
         rebars.Add(new Rebar(0.20, 0.05, rebarArea));
         rebars.Add(new Rebar(0.65, 0.05, rebarArea));
         rebars.Add(new Rebar(0.65, 0.20, rebarArea));
         rebars.Add(new Rebar(0.05, 0.20, rebarArea));
         // concrete parameters
         Concrete concrete = new Concrete();
         concrete.SetStrainStressModelRectangular(20e6, 0.0035, 35e9, 0.8);
         // steel parameters
         Steel steel = new Steel();
         steel.SetModelIdealElastoPlastic(310e6, 0.075, 200e9);
         // solver creation and parameterization
         RCSolver solver = RCSolver.CreateNewSolver(geometry);
         solver.SetRebars(rebars);
         solver.SetConcrete(concrete);
         solver.SetSteel(steel);
         //calulation
         solver.SolveResistance(72.4471E3, -28.9825E3, 2.5743E3);
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
         sb.AppendLine("Case 15a: Calculation of capacity state in asymmetric section for bidirectional bending with axial force ");
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
        
          
          // Case 15b      
         // concrete parameters (rectangular)
         concrete.SetStrainStressModelRectangular(20e6, 0.0035, 35e9, 0.8);
         // solver parameterization
         solver.SetConcrete(concrete);
         //calulation
         solver.SolveResistance(72.4471E3, -28.9825E3, 2.5743E3);
         // result for RC section
         forces = solver.GetInternalForces(ResultType.Section);
         // result presentation
         sb.AppendLine();
         sb.AppendLine(decoration);
         sb.AppendLine("Case 15b: Calculation of capacity state in asymmetric section for bidirectional bending with axial force (rectangular)");
         sb.AppendLine(decoration);
         sb.AppendLine(FormatOutput("N",forces.AxialForce,6));
         sb.AppendLine(FormatOutput("Mx" , forces.MomentX,6));
         sb.AppendLine(FormatOutput("My",forces.MomentY,6));

         // concrete parameters(linear)
         concrete.SetStrainStressModelLinear(25e6, 0.0035, 32e9);
         // solver parameterization
         solver.SetConcrete(concrete);
         //calulation
         solver.SolveResistance(72.4471E3, -28.9825E3, 2.5743E3);
         // result for RC section
         forces = solver.GetInternalForces(ResultType.Section);
         // result presentation
         sb.AppendLine();
         sb.AppendLine(decoration);
         sb.AppendLine("Case 15b: Calculation of capacity state in asymmetric section for bidirectional bending with axial force (linear)");
         sb.AppendLine(decoration);
         sb.AppendLine(FormatOutput("N",forces.AxialForce,6));
         sb.AppendLine(FormatOutput("Mx" , forces.MomentX,6));
         sb.AppendLine(FormatOutput("My",forces.MomentY,6));

         // concrete parameters (bilinear)
         concrete.SetStrainStressModelBiLinear(25e6, 0.0035, 32e9, 0.0020);
         // solver parameterization
         solver.SetConcrete(concrete);
         //calulation
         solver.SolveResistance(72.4471E3, -28.9825E3, 2.5743E3);
         // result for RC section
         forces = solver.GetInternalForces(ResultType.Section);
         // result presentation
         sb.AppendLine();
         sb.AppendLine(decoration);
         sb.AppendLine("Case 15b: Calculation of capacity state in asymmetric section for bidirectional bending with axial force (bilinear)");
         sb.AppendLine(decoration);
         sb.AppendLine(FormatOutput("N",forces.AxialForce,6));
         sb.AppendLine(FormatOutput("Mx" , forces.MomentX,6));
         sb.AppendLine(FormatOutput("My",forces.MomentY,6));

         // concrete parameters (parabolic-rectangular)
         concrete.SetStrainStressModelParabolicRectangular(25e6, 0.0035, 32e9, 0.0020);
         // solver parameterization
         solver.SetConcrete(concrete);
         //calulation
         solver.SolveResistance(72.4471E3, -28.9825E3, 2.5743E3);
         // result for RC section
         forces = solver.GetInternalForces(ResultType.Section);
         // result presentation
         sb.AppendLine();
         sb.AppendLine(decoration);
         sb.AppendLine("Case 15b: Calculation of capacity state in asymmetric section for bidirectional bending with axial force (parabolic-rectangular)");
         sb.AppendLine(decoration);
         sb.AppendLine(FormatOutput("N",forces.AxialForce,6));
         sb.AppendLine(FormatOutput("Mx" , forces.MomentX,6));
         sb.AppendLine(FormatOutput("My",forces.MomentY,6));

         // concrete parameters (power-rectangular)
         concrete.SetStrainStressModelPowerRectangular(25e6, 0.0035, 32e9,0.002, 1.5);
         // solver parameterization
         solver.SetConcrete(concrete);
         //calulation
         solver.SolveResistance(72.4471E3, -28.9825E3, 2.5743E3);
         // result for RC section
         forces = solver.GetInternalForces(ResultType.Section);
         // result presentation
         sb.AppendLine();
         sb.AppendLine(decoration);
         sb.AppendLine("Case 15b: Calculation of capacity state in asymmetric section for bidirectional bending with axial force (parabolic-rectangular)");
         sb.AppendLine(decoration);
         sb.AppendLine(FormatOutput("N",forces.AxialForce,6));
         sb.AppendLine(FormatOutput("Mx" , forces.MomentX,6));
         sb.AppendLine(FormatOutput("My",forces.MomentY,6));
      }
   }
}
