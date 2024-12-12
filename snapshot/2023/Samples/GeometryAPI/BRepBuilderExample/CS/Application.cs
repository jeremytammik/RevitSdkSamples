//
// (C) Copyright 2003-2019 by Autodesk, Inc. All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.BRepBuilderExample.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalApplication
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class Application : IExternalApplication
   {
      #region IExternalApplication Members
      static String addinAssmeblyPath = typeof(Application).Assembly.Location;

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
         createRibbonButtons(application);
         return Result.Succeeded;
      }

      BitmapSource convertFromBitmap(System.Drawing.Bitmap bitmap)
      {
         return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
             bitmap.GetHbitmap(),
             IntPtr.Zero,
             Int32Rect.Empty,
             BitmapSizeOptions.FromEmptyOptions());
      }

      void createRibbonButtons(UIControlledApplication application)
      {
         application.CreateRibbonTab("Create Geometry");
         RibbonPanel rp = application.CreateRibbonPanel("Create Geometry", "Create Geometry using BRepBuilder");

         PushButtonData pbd1 = new PushButtonData("CreateCube", "Create Cube",
             addinAssmeblyPath,
             "Revit.SDK.Samples.BRepBuilderExample.CS.CreateCube");
         pbd1.LargeImage = convertFromBitmap(Revit.SDK.Samples.BRepBuilderExample.CS.Properties.Resources.large_shape);
         pbd1.Image = convertFromBitmap(Revit.SDK.Samples.BRepBuilderExample.CS.Properties.Resources.small_shape);
         PushButton pb1 = rp.AddItem(pbd1) as PushButton;


         PushButtonData pbd2 = new PushButtonData("CreateNURBS", "Create NURBS Surface",
             addinAssmeblyPath,
             "Revit.SDK.Samples.BRepBuilderExample.CS.CreateNURBS");
         pbd2.LargeImage = convertFromBitmap(Revit.SDK.Samples.BRepBuilderExample.CS.Properties.Resources.large_shape);
         pbd2.Image = convertFromBitmap(Revit.SDK.Samples.BRepBuilderExample.CS.Properties.Resources.small_shape);
         PushButton pb2 = rp.AddItem(pbd2) as PushButton;

         PushButtonData pbd3 = new PushButtonData("CreatePeriodic", "Create Periodic Surface",
             addinAssmeblyPath,
             "Revit.SDK.Samples.BRepBuilderExample.CS.CreatePeriodic");
         pbd3.LargeImage = convertFromBitmap(Revit.SDK.Samples.BRepBuilderExample.CS.Properties.Resources.large_shape);
         pbd3.Image = convertFromBitmap(Revit.SDK.Samples.BRepBuilderExample.CS.Properties.Resources.small_shape);
         PushButton pb3 = rp.AddItem(pbd3) as PushButton;
      }


      #endregion
   }
}
