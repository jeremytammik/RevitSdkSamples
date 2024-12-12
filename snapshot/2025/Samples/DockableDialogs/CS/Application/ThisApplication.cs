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
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Media.Imaging;
using System.Configuration;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Revit.SDK.Samples.DockableDialogs.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ThisApplication : IExternalApplication, IDisposable
    {
       public ThisApplication()
       {
       }

      public void Dispose()
      {
         // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
         Dispose(disposing: true);
         GC.SuppressFinalize(this);
      }

      protected virtual void Dispose(bool disposing)
      {
         if (!disposing || isDisposed)
            return;

         if (m_mainPage != null)
            m_mainPage.Dispose();

         isDisposed = true;
      }

      public Result OnShutdown(UIControlledApplication application)
       {
          return Result.Succeeded;
       }

       /// <summary>
       /// Register a dockable Window
       /// </summary>
       public void RegisterDockableWindow(UIApplication  application, Guid mainPageGuid)
       {
          Globals.sm_UserDockablePaneId = new DockablePaneId(mainPageGuid);
          application.RegisterDockablePane(Globals.sm_UserDockablePaneId, Globals.ApplicationName, ThisApplication.thisApp.GetMainWindow() as IDockablePaneProvider);
       }

       /// <summary>
       /// Register a dockable Window
       /// </summary>
       public void RegisterDockableWindow(UIControlledApplication application, Guid mainPageGuid)
       {
          Globals.sm_UserDockablePaneId = new DockablePaneId(mainPageGuid);
          application.RegisterDockablePane(Globals.sm_UserDockablePaneId, Globals.ApplicationName, ThisApplication.thisApp.GetMainWindow() as IDockablePaneProvider);
       }

       /// <summary>
       /// Add UI for registering, showing, and hiding dockable panes.
       /// </summary>
       public Result OnStartup(UIControlledApplication application)
       {
          thisApp = this;
          m_APIUtility = new APIUtility();

          application.CreateRibbonTab(Globals.DiagnosticsTabName);
          RibbonPanel panel = application.CreateRibbonPanel(Globals.DiagnosticsTabName, Globals.DiagnosticsPanelName);
          
          panel.AddSeparator();

          PushButtonData pushButtonRegisterPageData = new PushButtonData(Globals.RegisterPage, Globals.RegisterPage,
          FileUtility.GetAssemblyFullName(), typeof(ExternalCommandRegisterPage).FullName);
          pushButtonRegisterPageData.LargeImage = new BitmapImage(new Uri(FileUtility.GetApplicationResourcesPath() + "Register.png"));
          PushButton pushButtonRegisterPage = panel.AddItem(pushButtonRegisterPageData) as PushButton;
          pushButtonRegisterPage.AvailabilityClassName = typeof(ExternalCommandRegisterPage).FullName;


          PushButtonData pushButtonShowPageData = new PushButtonData(Globals.ShowPage, Globals.ShowPage,FileUtility.GetAssemblyFullName(), typeof(ExternalCommandShowPage).FullName);
          pushButtonShowPageData.LargeImage = new BitmapImage(new Uri(FileUtility.GetApplicationResourcesPath() + "Show.png"));
          PushButton pushButtonShowPage = panel.AddItem(pushButtonShowPageData) as PushButton;
          pushButtonShowPage.AvailabilityClassName = typeof(ExternalCommandShowPage).FullName;


          PushButtonData pushButtonHidePageData = new PushButtonData(Globals.HidePage, Globals.HidePage, FileUtility.GetAssemblyFullName(), typeof(ExternalCommandHidePage).FullName);
          pushButtonHidePageData.LargeImage = new BitmapImage(new Uri(FileUtility.GetApplicationResourcesPath() + "Hide.png"));
          PushButton pushButtonHidePage = panel.AddItem(pushButtonHidePageData) as PushButton;
          pushButtonHidePage.AvailabilityClassName = typeof(ExternalCommandHidePage).FullName;

          return Result.Succeeded;
       }

       /// <summary>
       /// Create the new WPF Page that Revit will dock.
       /// </summary>
       public void CreateWindow()
       {
          m_mainPage = new MainPage();
       }

       /// <summary>
       /// Show or hide a dockable pane.
       /// </summary>
       public void SetWindowVisibility(Autodesk.Revit.UI.UIApplication application, bool state) 
       {
          DockablePane pane = application.GetDockablePane(Globals.sm_UserDockablePaneId);
          if (pane != null)
          {
             if (state)
                pane.Show();
             else
                pane.Hide();
          }


       }


       public bool IsMainWindowAvailable()
       {

          if (m_mainPage == null)
             return false;

          bool isAvailable = true;
          try {  bool isVisible = m_mainPage.IsVisible; }
          catch (Exception)
          {
             isAvailable = false;
          }
          return isAvailable;

       }

       public MainPage GetMainWindow()
       {
          if (!IsMainWindowAvailable())
             throw new InvalidOperationException("Main window not constructed.");
          return m_mainPage;
       }
  
       public APIUtility GetDockableAPIUtility() { return m_APIUtility; }

   

       public Autodesk.Revit.UI.DockablePaneId MainPageDockablePaneId
       {

          get { return Globals.sm_UserDockablePaneId; }
       }

       #region Data

        MainPage m_mainPage;
        internal static ThisApplication thisApp = null;
        private APIUtility m_APIUtility;
        private bool isDisposed;

       #endregion

    }
}

