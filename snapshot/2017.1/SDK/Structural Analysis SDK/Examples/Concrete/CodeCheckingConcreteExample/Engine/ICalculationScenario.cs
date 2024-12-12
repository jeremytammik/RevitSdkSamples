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

namespace CodeCheckingConcreteExample.Engine
{
   /// <summary>
   /// Base interface for calculation scenario.
   /// </summary>
   public interface ICalculationScenario
   {
      /// <structural_toolkit_2015>

      /// <summary>
      /// Gets list of calculation objects cref="ICalculationObject".
      /// </summary>
      /// <param name="category">Category of the element.</param>
      /// <param name="material">Material of the element.</param>
      /// <returns>List of calculation objects.</returns>
      List<ICalculationObject> CalculationScenarioList(Autodesk.Revit.DB.BuiltInCategory category, Autodesk.Revit.DB.StructuralAssetClass material);

      /// </structural_toolkit_2015>
   }
}
