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

namespace CodeCheckingConcreteExample.Utility
{
   /// <summary>
   /// Simple utility class for conversion between external and internal units
   /// Internal units are those used internally by RCCServer and defined by ResultBuilder
   /// </summary>
   public class UnitsConverter
   {

      /// <summary>
      /// Gets internal unit for a given unit type
      /// </summary>
      /// <param name="unitType">UnitType object</param>
      /// <returns>Internal unit</returns>
      static public DisplayUnitType GetInternalUnit(UnitType unitType)
      {
         if (unitType == UnitType.UT_Moment) { return DisplayUnitType.DUT_NEWTON_METERS; }
         else if (unitType == UnitType.UT_Force) { return DisplayUnitType.DUT_NEWTONS; }
         else if (unitType == UnitType.UT_Reinforcement_Area) { return DisplayUnitType.DUT_SQUARE_METERS; }
         else if (unitType == UnitType.UT_Length) { return DisplayUnitType.DUT_METERS; }
         else if (unitType == UnitType.UT_Section_Dimension) { return DisplayUnitType.DUT_METERS; }
         else if (unitType == UnitType.UT_LinearForce) { return DisplayUnitType.DUT_NEWTONS_PER_METER; }
         else if (unitType == UnitType.UT_LinearMoment) { return DisplayUnitType.DUT_NEWTON_METERS_PER_METER; }
         else if (unitType == UnitType.UT_Reinforcement_Area_per_Unit_Length) { return DisplayUnitType.DUT_SQUARE_METERS_PER_METER; }
         else if (unitType == UnitType.UT_Displacement_Deflection) { return DisplayUnitType.DUT_METERS; }
         else if (unitType == UnitType.UT_Stress) { return DisplayUnitType.DUT_NEWTONS_PER_SQUARE_METER; }
         else if (unitType == UnitType.UT_Bar_Diameter) { return DisplayUnitType.DUT_METERS; }
         else return DisplayUnitType.DUT_UNDEFINED;
      }



   }
}
