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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.CodeChecking.Concrete;
using CodeCheckingConcreteExample.Engine;
using CodeCheckingConcreteExample.Concrete;
using CodeCheckingConcreteExample.Utility;
using CodeCheckingConcreteExample.ConcreteTypes;
using DD = CodeCheckingConcreteExample.ConcreteTypes.DimensioningDirection;
/// <structural_toolkit_2015>
namespace CodeCheckingConcreteExample.Main.Calculation
{
   class FloorSection : SurfaceSection
   {
      public FloorSection(SectionDataBase sectionDataBase) : base(sectionDataBase)
      {
         Width = 0.0;
         Height = 0.0;
         Geometry = new Geometry();

         ListInternalForces = new List<InternalForcesBase>();
         MinStiffnes[DD.X] = MinStiffnes[DD.Y] = 0.0;
      }
   }
}
/// </structural_toolkit_2015>
