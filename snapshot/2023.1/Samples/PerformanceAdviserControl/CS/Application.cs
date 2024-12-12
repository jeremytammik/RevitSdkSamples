//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;


namespace Revit.SDK.Samples.PerformanceAdviserControl.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalApplication,
   /// </summary>
    public class Application : Autodesk.Revit.UI.IExternalApplication
    {
         #region Constructor
       /// <summary>
       /// Basic construction
       /// </summary>
        public Application()
        {

            
        }
#endregion

         #region IExternalApplication implementation
        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit starts before a file or default template is actually loaded.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully started. 
        /// 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// 
        /// If Failed is returned then Revit should inform the user that the external application 
        /// failed to load and the release the internal reference.
        /// 
        /// This method also adds a ribbon panel and button to launch an IExternalCommand
        /// defined in UICommand.cs.  It also registers a new IPerformanceAdviserRule-implementing class
        /// (m_FlippedDoorApiRule) with PerformanceAdviser.
        /// 
        /// </returns>
        public Autodesk.Revit.UI.Result OnStartup(Autodesk.Revit.UI.UIControlledApplication application)
        {
           #region Add command button
           RibbonPanel rp = application.CreateRibbonPanel("PerformanceAdviserControl");
           string currentAssembly = System.Reflection.Assembly.GetAssembly(this.GetType()).Location;
           PushButton pb = rp.AddItem(new PushButtonData("Performance Adviser", "Performance Adviser", currentAssembly, "Revit.SDK.Samples.PerformanceAdviserControl.CS.UICommand")) as PushButton;
           Uri uriImage = new Uri(System.IO.Path.GetDirectoryName(currentAssembly) + "\\Button32.png");
           BitmapImage largeImage = new BitmapImage(uriImage);
           pb.LargeImage = largeImage;
           #endregion

           #region Create and register new API rule (FlippedDoorCheck)
           m_FlippedDoorApiRule = new FlippedDoorCheck();
           Autodesk.Revit.DB.PerformanceAdviser.GetPerformanceAdviser().AddRule(m_FlippedDoorApiRule.getRuleId(), m_FlippedDoorApiRule);
           #endregion

           return Autodesk.Revit.UI.Result.Succeeded;

        }
        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit is about to exit, Any documents must have been closed before this method is called.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully shutdown. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If Failed is returned then the Revit user should be warned of the failure of the external 
        /// application to shut down correctly.
        /// 
        /// This method also unregisters a the IPerformanceAdviserRule-implementing class
        /// (m_FlippedDoorApiRule) with PerformanceAdviser.
        /// </returns>
        public Autodesk.Revit.UI.Result OnShutdown(Autodesk.Revit.UI.UIControlledApplication application)
        {
           #region Unregister API rule
           Autodesk.Revit.DB.PerformanceAdviser.GetPerformanceAdviser().DeleteRule(m_FlippedDoorApiRule.getRuleId());
           m_FlippedDoorApiRule = null;
           #endregion

           return Autodesk.Revit.UI.Result.Succeeded;
        }
#endregion

         #region Data
       /// <summary>
       /// The custom API rule we are registering with PerformanceAdviser
       /// </summary>
         private FlippedDoorCheck m_FlippedDoorApiRule;
        #endregion
    }
}
