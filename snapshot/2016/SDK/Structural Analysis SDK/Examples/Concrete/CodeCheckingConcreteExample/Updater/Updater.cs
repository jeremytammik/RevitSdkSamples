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

namespace CodeCheckingConcreteExample.Main
{
   /// <summary>
   /// Represents updater which main task is to invalidate results when any geometry change is done.
   /// </summary>
   class Updater : IUpdater
   {
      #region IUpdater Members

      public static Guid UpdaterId = new Guid("9afbd3c6-5eff-4712-adce-e8eef1788081");
      public static Guid AddinId = new Guid("88b169e5-cfb4-440a-830f-20958f71663b");

      public Updater()
      {
      }

      public void Execute(UpdaterData data)
      {
         Autodesk.Revit.DB.CodeChecking.Storage.StorageService service = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService();
         Autodesk.Revit.DB.CodeChecking.Storage.StorageDocument storageDocument = service.GetStorageDocument(data.GetDocument());

         foreach (ElementId elId in data.GetModifiedElementIds())
         {
            Element el = data.GetDocument().GetElement(elId);
            storageDocument.ResultsManager.OutDateResults(Server.Server.ID, el);
         }
      }

      public string GetAdditionalInformation()
      {
         return "Code Checking Conrete Example for Revit API";
      }

      public ChangePriority GetChangePriority()
      {
         return ChangePriority.Structure;
      }

      public UpdaterId GetUpdaterId()
      {
         return new UpdaterId(new AddInId(AddinId), UpdaterId);
      }

      public string GetUpdaterName()
      {
         return "CodeCheckingConcreteExample";
      }

      #endregion
   }
}
