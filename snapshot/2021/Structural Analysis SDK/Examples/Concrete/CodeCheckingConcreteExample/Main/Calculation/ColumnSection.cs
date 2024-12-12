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

namespace CodeCheckingConcreteExample.Main.Calculation
{
   /// <summary>
   /// Represents user's section of a column element.
   /// </summary>
   class ColumnSection : LinearSection
   {
      /// <summary>
      /// Initializes a new instance of user's section object of column element.  
      /// </summary>
      /// <param name="sectionDataBase">Instance of base section object with predefined parameters to copy.</param>
      public ColumnSection(SectionDataBase sectionDataBase)
         : base(sectionDataBase)
      {
         Width = 0.0;
         Height = 0.0;
         Geometry = new Geometry();

         ListInternalForces = new List<InternalForcesBase>();
         MinStiffness = 0.0;
      }
   }
}
