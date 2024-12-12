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
using System.Collections;
using System.Collections.Generic;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.DockableDialogs.CS
{

   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ExternalCommandHidePage : IExternalCommand, IExternalCommandAvailability
   {
      public virtual Result Execute(ExternalCommandData commandData
          , ref string message, ElementSet elements)
      {
         try
         {
            ThisApplication.thisApp.GetDockableAPIUtility().Initialize(commandData.Application);
            ThisApplication.thisApp.SetWindowVisibility(commandData.Application, false);
         }
         catch (Exception)
         {
            TaskDialog.Show("Dockable Dialogs", "Dialog not registered.");
         }
         return Result.Succeeded;
      }

      /// <summary>
      /// Onlys show the dialog when a document is  not open, as Dockable dialogs should only be registered when
      /// no documents are open.
      /// </summary>
      public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
      {
         if (applicationData.ActiveUIDocument == null)
            return false;
         else
            return true;
      }
   }
}
