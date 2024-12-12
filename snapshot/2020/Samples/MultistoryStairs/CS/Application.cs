//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Text;

using System.Collections;
using System.Xml;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using System.Windows;

using Res = Revit.SDK.Samples.MSOperation.CS.Properties.Resource;

namespace Revit.SDK.Samples.MSOperation.CS
{
   public class Application : IExternalApplication
   {
      static string AddInPath = typeof(Application).Assembly.Location;
      const string TabName = "API_MultistoryStairs";

      public Result OnStartup(UIControlledApplication application)
      {
         CreateDisplacementPanel(application);
         return Result.Succeeded;
      }

      public Result OnShutdown(UIControlledApplication application)
      {
         return Result.Succeeded;
      }

      /// <summary>
      /// Sets up the Ribbon panel for the sample.
      /// </summary>
      private void CreateDisplacementPanel(UIControlledApplication application)
      {
         
         application.CreateRibbonTab(TabName);
         RibbonPanel rp = application.CreateRibbonPanel(TabName, "Creation");

         PushButtonData data = new PushButtonData("MultistoryStairs_Creation", "Create single multistory stairs",
                                                         AddInPath,
                                                         typeof(CreateMultistoryStairsCommand).FullName);
         SetIconsForPushButtonData(data, Res.create_multistory_stair);
         data.ToolTip = "Select a stairs element, and click this button to create a single multistory stairs.";
         rp.AddItem(data);
         
         rp = application.CreateRibbonPanel(TabName, "Add/Remove Stairs");
         data = new PushButtonData("MultistoryStairs_AddStairs", "Add stairs by picking levels",
                                                         AddInPath,
                                                         typeof(AddStairsCommand).FullName);
         SetIconsForPushButtonData(data, Res.add_stairs);
         data.ToolTip = "Select a multistory stairs element, and click this button to add the stairs by picking some aligned levels.";
         rp.AddItem(data);

         data = new PushButtonData("MultistoryStairs_RemoveStairs", "Remove stairs by picking levels",
                                                         AddInPath,
                                                         typeof(RemoveStairsCommand).FullName);
         SetIconsForPushButtonData(data, Res.remove_stairs);
         data.ToolTip = "Select a multistory stairs element, and click this button to remove the stairs by picking some aligned levels.";
         rp.AddItem(data);

         data = new PushButtonData("MultistoryStairs_UnpinStairs", "Unpin the instance of stairs",
                                                         AddInPath,
                                                         typeof(UnpinStairCommand).FullName);
         SetIconsForPushButtonData(data, Res.element_unpin);
         data.ToolTip = "Select a multistory stairs element, and click this button to unpin the instance of stairs by picking the connected base levels.";
         rp.AddItem(data);

         data = new PushButtonData("MultistoryStairs_PinbackStairs", "Pin back the stairs",
                                                         AddInPath,
                                                         typeof(PinBackStairCommand).FullName);
         SetIconsForPushButtonData(data, Res.element_pin);
         data.ToolTip = "Select a multistory stairs element, and click this button to pin back stairs by picking the connected base levels.";
         rp.AddItem(data);
      }

      /// <summary>
      /// Utility for adding icons to the button.
      /// </summary>
      /// <param name="button">The push button.</param>
      /// <param name="icon">The icon.</param>
      private static void SetIconsForPushButtonData(PushButtonData button, System.Drawing.Icon icon)
      {
         if (null != icon && null != button)
         {
            button.LargeImage = GetStdIcon(icon);
            button.Image = GetSmallIcon(icon);
         }
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
   }
    
}
