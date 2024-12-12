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
using Autodesk.Revit.DB.CodeChecking.Storage;

namespace CodeCheckingConcreteExample.Engine
{
   /// <summary>
   /// Represents common parameters.
   /// </summary>
   public class CommonParametersBase
   {
      /// <summary>
      /// Initializes a new instance of the CommonParametersBase object with list of parameters.  
      /// </summary>
      /// <param name="listElementStatus">List identyficators of elements with result status.</param>
      /// <param name="listCombinationId">List load combinations identificators</param>
      /// <param name="activePackageGuid">Identificator of active package with results.</param>
      /// <param name="calculationParameters">Calculation parameters.</param>
      public CommonParametersBase(List<Tuple<ElementId, ResultStatus>> listElementStatus,
                                  List<ElementId> listCombinationId,
                                  Guid activePackageGuid,
                                  Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass calculationParameters)
      {
         listElemStatus = listElementStatus;
         listCombId = listCombinationId;
         this.activePackageGuid = activePackageGuid;
         calcParams = calculationParameters;
      }

      /// <summary>
      /// Initializes a new instance of the CommonParametersBase from an existing one.  
      /// </summary>
      /// <param name="param">Common parameters to copy.</param>
      public CommonParametersBase(CommonParametersBase param)
      {
         listElemStatus = param.ListElementStatus;
         listCombId = param.ListCombinationId;
         activePackageGuid = param.activePackageGuid;
         calcParams = param.CalculationParameter;
      }

      private CommonParametersBase() { }

      /// <summary>
      /// Gets the list of elements identificators.
      /// </summary>
      public List<Tuple<ElementId, ResultStatus>> ListElementStatus
      {
         get { return listElemStatus; }
      }

      /// <summary>
      /// Gets the list of load combinations identificators.
      /// </summary>
      public List<ElementId> ListCombinationId
      {
         get { return listCombId; }
      }

      /// <summary>
      /// Gets the calculation parameters.
      /// </summary>
      public Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass CalculationParameter
      {
         get { return calcParams; }
      }

      /// <summary>
      /// Gets the identificator of an active package with results.
      /// </summary>
      public Guid ActivePackageGuid
      {
         get { return activePackageGuid; }
      }

      private Guid activePackageGuid;
      private List<Tuple<ElementId, ResultStatus>> listElemStatus;
      private List<ElementId> listCombId;
      private Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass calcParams;
   }
}
