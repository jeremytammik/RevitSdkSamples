//
// (C) Copyright 2003-2016 by Autodesk, Inc. All rights reserved.
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

using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.AttachedDetailGroup.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalApplication
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class Application : IExternalApplication
   {
      #region IExternalApplication Members

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
         CreateAttachedDetailGroupPanel(application);

         return Result.Succeeded;
      }

      #endregion

      /// <summary>
      /// Sets up the add-in panel for this sample.
      /// </summary>
      private void CreateAttachedDetailGroupPanel(UIControlledApplication application)
      {
         // Create the ribbon panel.
         RibbonPanel rp = application.CreateRibbonPanel("Attached Detail Group");

         // Create the show all detail groups pushbutton.
         PushButtonData pbdShowAllDetailGroups = new PushButtonData("ShowAttachedDetailGroups", "Show Attached\nDetail Groups",
                  addAssemblyPath,
                  typeof(Revit.SDK.Samples.AttachedDetailGroup.CS.AttachedDetailGroupShowAllCommand).FullName);

         pbdShowAllDetailGroups.LongDescription = "Show all of the selected element group's attached detail groups that are compatible with the current view.";

         PushButton pbShowAllDetailGroups = rp.AddItem(pbdShowAllDetailGroups) as PushButton;
         SetIconsForPushButton(pbShowAllDetailGroups, Revit.SDK.Samples.AttachedDetailGroup.CS.Properties.Resources.ShowAllDetailGroupsIcon);

         // Create the hide all detail groups pushbutton.
         PushButtonData pbdHideAllDetailGroups = new PushButtonData("HideAttachedDetailGroups", "Hide Attached\nDetail Groups",
                  addAssemblyPath,
                  typeof(Revit.SDK.Samples.AttachedDetailGroup.CS.AttachedDetailGroupHideAllCommand).FullName);

         pbdHideAllDetailGroups.LongDescription = "Hide all of the selected element group's attached detail groups that are compatible with the current view.";

         PushButton pbHideAllDetailGroups = rp.AddItem(pbdHideAllDetailGroups) as PushButton;
         SetIconsForPushButton(pbHideAllDetailGroups, Revit.SDK.Samples.AttachedDetailGroup.CS.Properties.Resources.HideAllDetailGroupsIcon);
      }

      /// <summary>
      /// Utility for adding icons to the button.
      /// </summary>
      /// <param name="button">The push button.</param>
      /// <param name="icon">The icon.</param>
      private static void SetIconsForPushButton(PushButton button, System.Drawing.Icon icon)
      {
         button.LargeImage = GetStdIcon(icon);
         button.Image = GetSmallIcon(icon);
      }

      /// <summary>
      /// Gets the standard sized icon as a BitmapSource.
      /// </summary>
      /// <param name="icon">The icon.</param>
      /// <returns>The BitmapSource.</returns>
      private static BitmapSource GetStdIcon(System.Drawing.Icon icon)
      {
         return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
               icon.Handle,
               Int32Rect.Empty,
               BitmapSizeOptions.FromEmptyOptions());
      }

      /// <summary>
      /// Gets the small sized icon as a BitmapSource.
      /// </summary>
      /// <param name="icon">The icon.</param>
      /// <returns>The BitmapSource.</returns>
      private static BitmapSource GetSmallIcon(System.Drawing.Icon icon)
      {
         System.Drawing.Icon smallIcon = new System.Drawing.Icon(icon, new System.Drawing.Size(16, 16));
         return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
               smallIcon.Handle,
               Int32Rect.Empty,
               BitmapSizeOptions.FromEmptyOptions());
      }

      /// <summary>
      /// The path to this add-in assembly.
      /// </summary>
      static String addAssemblyPath = typeof(Revit.SDK.Samples.AttachedDetailGroup.CS.Application).Assembly.Location;
   }
}
