//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.GetSetDefaultTypes.CS
{
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   class ThisCommand : IExternalCommand
   {
      #region IExternalCommand Members

      public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         if (!DockablePane.PaneExists(DefaultFamilyTypes.PaneId) ||
             !DockablePane.PaneExists(DefaultElementTypes.PaneId))
            return Result.Failed;

         UIApplication uiApp = commandData.Application;
         if (uiApp == null)
            return Result.Failed;

         DockablePane pane = uiApp.GetDockablePane(DefaultFamilyTypes.PaneId);
         pane.Show();
         DockablePane elemTypePane = uiApp.GetDockablePane(DefaultElementTypes.PaneId);
         elemTypePane.Show();

         if (ThisApplication.DefaultFamilyTypesPane != null)
            ThisApplication.DefaultFamilyTypesPane.SetDocument(commandData.Application.ActiveUIDocument.Document);
         if (ThisApplication.DefaultElementTypesPane != null)
            ThisApplication.DefaultElementTypesPane.SetDocument(commandData.Application.ActiveUIDocument.Document);

         return Result.Succeeded;
      }

      #endregion
   }
}
