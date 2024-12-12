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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.ScheduleToHTML.CS
{
    /// <summary>
    /// Implementation of the IExternalApplication.
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
            CreateScheduleToHTMLPanel(application);

            return Result.Succeeded;
        }

        #endregion
        
        /// <summary>
        /// Sets up the add-in panel for this sample.
        /// </summary>
        private void CreateScheduleToHTMLPanel(UIControlledApplication application)
        {
            RibbonPanel rp = application.CreateRibbonPanel("Schedule To HTML");

            PushButtonData pbd = new PushButtonData("ScheduleToHTML", "Export to HTML",
                    addAssemblyPath,
                    typeof(Revit.SDK.Samples.ScheduleToHTML.CS.ScheduleHTMLExportCommand).FullName);

            pbd.LongDescription = "Export the active schedule to HTML.";

            PushButton duplicateAllPB = rp.AddItem(pbd) as PushButton;
            SetIconsForPushButton(duplicateAllPB, Revit.SDK.Samples.ScheduleToHTML.CS.Properties.Resources.ScheduleExport);
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
        static String addAssemblyPath = typeof(Revit.SDK.Samples.ScheduleToHTML.CS.Application).Assembly.Location;

    }
}
