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
   /// Represents type of a calculation object.
   /// </summary>
   public enum CalculationObjectType
   {
      /// <summary>
      /// Designed for elements objects.
      /// </summary>
      Element,
      /// <summary>
      /// Designed for sections objects.
      /// </summary>
      Section,
   }

   /// <summary>
   /// Represents type of a calculation object.
   /// </summary>
   public enum ErrorResponse
   {
      /// <summary>
      /// Execute the Run method when "false" value was returned in previus Run methods.
      /// </summary>
      RunOnError,
      /// <summary>
      /// Skip the Run method when "false" value was returned in previus Run methods.
      /// </summary>
      SkipOnError,
   }

   /// <summary>
   /// Base interface for calculation objects.
   /// </summary>
   public interface ICalculationObject
   {
      /// <summary>
      /// Runs calculation\operations for an element cref="ElementDataBase" object or for a section object cref="SectionDataBase".
      /// </summary>
      /// <param name="obj">An element object or a section object</param>
      /// <returns>Result of calculation.</returns>
      bool Run(ObjectDataBase obj);
      /// <summary>
      /// Sets and gets common parameters.
      /// </summary>
      CommonParametersBase Parameters { get; set; }
      /// <summary>
      /// Gets and sets type of the calculation object.
      /// </summary>
      CalculationObjectType Type { get; set; }
      /// <summary>
      /// Gets and sets a type of error response of the calculation object.
      /// </summary>
      ErrorResponse ErrorResponse { get; set; }
      /// <summary>
      /// Gets and sets categories for the calculation object.
      /// </summary>
      IList<Autodesk.Revit.DB.BuiltInCategory> Categories { get; set; }
   }
}
