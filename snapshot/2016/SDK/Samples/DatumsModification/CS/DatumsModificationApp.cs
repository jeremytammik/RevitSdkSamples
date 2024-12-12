//
// (C) Copyright 2003-2015 by Autodesk, Inc. All rights reserved.
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
using System.Windows.Media.Imaging;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.DatumsModification.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class DatumsModificationApp : IExternalApplication
    {
        static string AddInPath = typeof(DatumsModificationApp).Assembly.Location;
        // Button icons directory
        static string ButtonIconsFolder = Path.GetDirectoryName(AddInPath);
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
           RibbonPanel ribbonPanel = application.CreateRibbonPanel("DatumModification");
           PushButtonData styleSettingButton = new PushButtonData("DatumStyle", "Datum Style", AddInPath, "Revit.SDK.Samples.DatumsModification.CS.DatumStyleModification");
           styleSettingButton.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "Style.png"), UriKind.Absolute)); ;
           PushButtonData alignSettingButton = new PushButtonData("AlignDatum", "Align Datums", AddInPath, "Revit.SDK.Samples.DatumsModification.CS.DatumAlignment");
           alignSettingButton.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "Align.png"), UriKind.Absolute)); ;
           PushButtonData propagateButton = new PushButtonData("PropagateDatum", "Propagate Extents", AddInPath, "Revit.SDK.Samples.DatumsModification.CS.DatumPropagation");
           propagateButton.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "Propagate.png"), UriKind.Absolute)); ;         
           
           ribbonPanel.AddItem(styleSettingButton);
           ribbonPanel.AddItem(alignSettingButton);
           ribbonPanel.AddItem(propagateButton);
           return Result.Succeeded;
        }

        #endregion
    }
}

