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
   /// Represents a base of element data.
   /// </summary>
   public class ElementDataBase : ObjectDataBase
   {
      /// <summary>
      /// Initializes a new instance of the ElementDataBase object with list of parameters.  
      /// </summary>
      /// <param name="elementResult">Element results schema.</param>
      /// <param name="listcalcPoint">List of the element's calculation points.</param>
      /// <param name="listSectionData">List of sections' data.</param>
      /// <param name="elementStatus">Element result status.</param>
      /// <param name="document">Acces to cref="Document".</param>
      /// <param name="data">Object with base parameters for the element.</param>
      public ElementDataBase(Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass elementResult,
                             List<CalcPoint> listcalcPoint,
                             List<SectionDataBase> listSectionData,
                             Autodesk.Revit.DB.CodeChecking.Storage.ResultStatus elementStatus,
                             Document document,
                             ObjectDataBase data)
         : base(data)
      {
         Result = elementResult;
         calcPoints = listcalcPoint;
         listSectData = listSectionData;
         Status = elementStatus;
         this.document = document;
      }

      /// <summary>
      /// Initializes a new instance of the ElementDataBase from an existing one.  
      /// </summary>
      /// <param name="elementDataBase">Element data to copy.</param>
      public ElementDataBase(ElementDataBase elementDataBase)
         : base(elementDataBase as ObjectDataBase)
      {
         Result = elementDataBase.Result;
         calcPoints = elementDataBase.ListCalcPoints;
         listSectData = elementDataBase.ListSectionData;
         Status = elementDataBase.Status;
         document = elementDataBase.document;
      }

      private ElementDataBase() { }

      /// <summary>
      /// Result schema of the element.
      /// </summary>
      public Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass Result { get; set; }

      /// <summary>
      /// List of calculation points cref="CalcPoint" of the element.
      /// </summary>
      public List<CalcPoint> ListCalcPoints
      {
         get { return calcPoints; }
      }

      /// <summary>
      /// List of sections data of the element.
      /// </summary>
      public List<SectionDataBase> ListSectionData
      {
         get { return listSectData; }
      }

      /// <summary>
      /// Gets and sets the element's result status.
      /// </summary>
      public Autodesk.Revit.DB.CodeChecking.Storage.ResultStatus Status { get; set; }

      /// <summary>
      /// Adds the formated error to cref="Status".
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="notify">if set to <c>true</c> [notify].</param>
      public void AddFormatedError(ResultStatusMessage message, bool notify = true)
      {
         Status.AddError(message, document, notify);
      }

      /// <summary>
      /// Adds the formated info to cref="Status".
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="notify">if set to <c>true</c> [notify].</param>
      public void AddFormatedInfo(ResultStatusMessage message, bool notify = true)
      {
         Status.AddInfo(message, document, notify);
      }

      /// <summary>
      /// Adds the formated warning to cref="Status".
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="notify">if set to <c>true</c> [notify].</param>
      public void AddFormatedWarning(ResultStatusMessage message, bool notify = true)
      {
         Status.AddWarning(message, document, notify);
      }

      List<CalcPoint> calcPoints;
      private List<SectionDataBase> listSectData;

      /// <summary>
      /// Revit document
      /// </summary>
      protected Document document;
   }
}
