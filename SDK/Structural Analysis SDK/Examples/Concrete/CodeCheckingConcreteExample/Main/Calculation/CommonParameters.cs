﻿//
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
using CodeCheckingConcreteExample.Engine;
using Autodesk.Revit.DB.CodeChecking.Engineering.Tools;

namespace CodeCheckingConcreteExample.Main.Calculation
{
   /// <summary>
   /// Represents user's object with common parameters.
   /// </summary>
   public class CommonParameters : CommonParametersBase
   {
      /// <summary>
      /// Initializes a new instance of user's object with common parameters.  
      /// </summary>
      /// <param name="data">Acces to cref="ServiceData"</param>
      /// <param name="param">Instance of base common parameters object with predefined parameters to copy.</param>
      public CommonParameters(Autodesk.Revit.DB.CodeChecking.ServiceData data, CommonParametersBase param)
         : base(param)
      {

      }
      /// <summary>
      /// Gets and sets cref="ForceResultsCache" object.
      /// </summary>
      public ForceResultsCache ResultCache { get; set; }
   }
}
