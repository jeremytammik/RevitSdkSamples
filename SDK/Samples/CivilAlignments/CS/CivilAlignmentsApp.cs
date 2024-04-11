//
// (C) Copyright 2003-2023 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable. 

using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.CivilAlignments.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalApplication
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class CivilAlignmentsApp : IExternalApplication
   {
      static string AddInPath = typeof(CivilAlignmentsApp).Assembly.Location;

      #region InterfaceImplementation

      /// <summary>
      /// Implements the OnShutdown event
      /// </summary>
      /// <param name="application"></param>
      /// <returns></returns>
      public Result OnShutdown(UIControlledApplication application)
      {
         return Result.Succeeded;
      }

      /// <summary>
      /// Implements the OnStartup event
      /// </summary>
      /// <param name="application"></param>
      /// <returns></returns>
      public Result OnStartup(UIControlledApplication application)
      {
         RibbonPanel ribbonPanel = application.CreateRibbonPanel("CivilAlignments");
         PushButtonData createSetButton = new PushButtonData("CreateStationLabels", "Create\r\n Station Labels", AddInPath, "Revit.SDK.Samples.CivilAlignments.CS.CreateAlignmentStationLabelsCmd");
         PushButtonData showPropertiesButton = new PushButtonData("ShowProperties", "Show\r\n Properties", AddInPath, "Revit.SDK.Samples.CivilAlignments.CS.ShowPropertiesCmd");

         ribbonPanel.AddItem(createSetButton);
         ribbonPanel.AddItem(showPropertiesButton);
         return Result.Succeeded;
      }

      #endregion
   }
}
