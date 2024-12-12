//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.RoutingPreferenceTools.CS
{

   internal class Validation
   {
      public static bool ValidateMep(Autodesk.Revit.ApplicationServices.Application application)
      {
         return application.IsPipingEnabled;
      }
      public static void MepWarning()
      {
         Autodesk.Revit.UI.TaskDialog.Show("RoutingPreferenceTools", "Revit MEP is required to run this addin.");
      }

      public static bool ValidatePipesDefined(Autodesk.Revit.DB.Document document)
      {
         FilteredElementCollector collector = new FilteredElementCollector(document);
         collector.OfClass(typeof(Autodesk.Revit.DB.Plumbing.PipeType));
         if (collector.Count() == 0)
            return false;
         else
            return true;
      }

      public static void PipesDefinedWarning()
      {
         Autodesk.Revit.UI.TaskDialog.Show("RoutingPreferenceTools", "At least two PipeTypes are required to run this command.  Please define another PipeType.");
      }
   }

   internal class Convert
   {
      public static double ConvertValueDocumentUnits(double decimalFeet, Autodesk.Revit.DB.Document document)
      {
         FormatOptions formatOption = document.GetUnits().GetFormatOptions(UnitType.UT_PipeSize);
         return UnitUtils.ConvertFromInternalUnits(decimalFeet, formatOption.DisplayUnits);
      }


      public static double ConvertValueToFeet(double unitValue, Autodesk.Revit.DB.Document document)
      {
         double tempVal = ConvertValueDocumentUnits(unitValue, document);
         double ratio = unitValue / tempVal;
         return unitValue * ratio;
      }
   }
}
