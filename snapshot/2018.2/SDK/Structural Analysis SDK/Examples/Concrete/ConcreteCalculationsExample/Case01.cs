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
      /// Case 1: Cross section characteristics
      /// </summary>
       public void Case1()
       {
           // geometry definition
           Geometry geometry = new Geometry();
           geometry.Add(0.0, 0.0);
           geometry.Add(0.0, 0.2);
           geometry.Add(0.2, 0.2);
           geometry.Add(0.2, 0.6);
           geometry.Add(0.5, 0.6);
           geometry.Add(0.5, 0.3);
           geometry.Add(0.8, 0.3);
           geometry.Add(0.8, 0.0);
           // rebars definition
           List<Rebar> rebars = new List<Rebar>();
           // rebar area A = d*d*pi/4
           rebars.Add(new Rebar(0.05, 0.05, 0.010 * 0.010 * Math.PI / 4.0));
           rebars.Add(new Rebar(0.75, 0.05, 0.012 * 0.012 * Math.PI / 4.0));
           rebars.Add(new Rebar(0.75, 0.25, 0.020 * 0.020 * Math.PI / 4.0));
           rebars.Add(new Rebar(0.45, 0.55, 0.050 * 0.050 * Math.PI / 4.0));
           rebars.Add(new Rebar(0.25, 0.55, 0.020 * 0.020 * Math.PI / 4.0));
           rebars.Add(new Rebar(0.05, 0.15, 0.015 * 0.015 * Math.PI / 4.0));
           // concrete parameters
           Concrete concrete = new Concrete();
           concrete.ModulusOfElasticity = 30e9;
           // steel parameters
           Steel steel = new Steel();
           steel.ModulusOfElasticity = 200e9;
           // solver creation and parameterization
           RCSolver solver = RCSolver.CreateNewSolver(geometry);
           solver.SetRebars(rebars);
           solver.SetConcrete(concrete);
           solver.SetSteel(steel);
           // result for concrete
           double Ac = solver.GetArea(ResultType.Concrete);
           Point2D Cc = solver.GetCenterOfInertia(ResultType.Concrete);
           double Icx = solver.GetMomentOfInertiaX(ResultType.Concrete);
           double Icy = solver.GetMomentOfInertiaY(ResultType.Concrete);
           // result for rebars
           double As = solver.GetArea(ResultType.Rebars);
           Point2D Cs = solver.GetCenterOfInertia(ResultType.Rebars);
           double Isx = solver.GetMomentOfInertiaX(ResultType.Rebars);
           double Isy = solver.GetMomentOfInertiaY(ResultType.Rebars);
           // result for reduced
           double Aeff = solver.GetArea(ResultType.Section);
           Point2D Ceff = solver.GetCenterOfInertia(ResultType.Section);
           double Ieffx = solver.GetMomentOfInertiaX(ResultType.Section);

           double Ieffy = solver.GetMomentOfInertiaY(ResultType.Section);

           // result presentation        

           sb.AppendLine();
           sb.AppendLine(decoration);
           sb.AppendLine("Case 1: Cross section characteristics");
           sb.AppendLine(decoration);
           sb.AppendLine(FormatOutput("Ac", Ac, 6));
           sb.AppendLine(FormatOutput("Ccx", Cc.X, 6));
           sb.AppendLine(FormatOutput("Ccy", Cc.Y, 6));
           sb.AppendLine(FormatOutput("Icx", Icx, 6));
           sb.AppendLine(FormatOutput("Icy", Icy, 6));
           sb.AppendLine(FormatOutput("As", As, 6));
           sb.AppendLine(FormatOutput("Csx", Cs.X, 6));
           sb.AppendLine(FormatOutput("Csy", Cs.Y, 6));
           sb.AppendLine(FormatOutput("Isx", Isx, 6));
           sb.AppendLine(FormatOutput("Isy", Isy, 6));
           sb.AppendLine(FormatOutput("Aeff", Aeff, 6));
           sb.AppendLine(FormatOutput("Ceffx", Ceff.X, 6));
           sb.AppendLine(FormatOutput("Ceffy", Ceff.Y, 6));
           sb.AppendLine(FormatOutput("Ieffx", Ieffx, 6));
           sb.AppendLine(FormatOutput("Ieffy", Ieffy, 6));
       }
   }
}
