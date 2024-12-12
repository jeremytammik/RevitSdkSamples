//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reflection;
using System.Drawing;
using System.Configuration;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using System.Text;

namespace Revit.SDK.Samples.DockableDialogs.CS
{

   public partial class MainPage : Page, Autodesk.Revit.UI.IDockablePaneProvider
    {
      public MainPage()
        {
           InitializeComponent();
           m_exEvent  = Autodesk.Revit.UI.ExternalEvent.Create(m_handler);
           m_textWriter = new StandardIORouter(tb_output); 
           Console.SetOut(m_textWriter);  //Send all standard output to the text rerouter.
        }
      UIApplication Application
      {
         set { m_application = value; }
      }
      #region UI State

      private void DozeOff()
      {
         EnableCommands(false);
      }

      private void EnableCommands(bool status)
      {
         if (status == false)
            this.Cursor = Cursors.Wait;
         else
            this.Cursor = Cursors.Arrow;
      }

      public void WakeUp()
      {
         EnableCommands(true);
      }
      #endregion
      #region UI Support
      public void UpdateUI(ModelessCommandData data)
      {
         switch (data.CommandType)
         {
            case (ModelessCommandType.PrintMainPageStatistics):
               {
                  Log.Message("***Main Pane***");
                  Log.Message(data.WindowSummaryData);
                  break;
               }

            case (ModelessCommandType.PrintSelectedPageStatistics):
               {
                  Log.Message("***Selected Pane***");
                  Log.Message(data.WindowSummaryData);
                  break;
               }

            default:
               break;
         }
      }
           
           
       private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
           for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
           {
              DependencyObject child = VisualTreeHelper.GetChild(obj, i);
              if (child != null && child is childItem) return (childItem)child;
              else
              {
              
                 childItem childOfChild = FindVisualChild<childItem>(child);
                 if (childOfChild != null) return childOfChild;
              }
           }
  
           return null;
        }
        #endregion
      #region UI Support
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
        }


        #endregion
      #region Data
        private Autodesk.Revit.UI.UIApplication m_application;
         private Autodesk.Revit.UI.ExternalEvent m_exEvent;
         private APIExternalEventHandler m_handler=  new APIExternalEventHandler();
         private System.IO.TextWriter m_textWriter; //Used to re-route any standard IO to the WPF UI.


        #endregion

      /// <summary>
      /// Called by Revit to initialize dockable pane settings set in DockingSetupDialog.
      /// </summary>
      /// <param name="data"></param>
        public void SetupDockablePane(Autodesk.Revit.UI.DockablePaneProviderData data)
        {
            data.FrameworkElement = this as FrameworkElement;
            DockablePaneProviderData d = new DockablePaneProviderData();


            data.InitialState = new Autodesk.Revit.UI.DockablePaneState(); 
            data.InitialState.DockPosition = m_position;
           DockablePaneId targetPane;
           if (m_targetGuid == Guid.Empty)
              targetPane = null;
           else targetPane = new DockablePaneId(m_targetGuid);
            if (m_position == DockPosition.Tabbed)
               data.InitialState.TabBehind = targetPane;

 
           if (m_position == DockPosition.Floating)
            {
               data.InitialState.SetFloatingRectangle(new Autodesk.Revit.UI.Rectangle(m_left, m_top, m_right, m_bottom));
            }

           Log.Message("***Intial docking parameters***");
           Log.Message(APIUtility.GetDockStateSummary(data.InitialState));

        }

        private void PaneInfoButton_Click(object sender, RoutedEventArgs e)
        {

           ThisApplication.thisApp.GetDockableAPIUtility().Post();
         
        }


       private void RaisePrintSummaryCommand()
        {
           ModelessCommandData data = new ModelessCommandData();
           data.CommandType = ModelessCommandType.PrintMainPageStatistics;
           ThisApplication.thisApp.GetDockableAPIUtility().RunModelessCommand(data);
           m_exEvent.Raise();
        }

       private void RaisePrintSpecificSummaryCommand(string guid)
       {
          ModelessCommandData data = new ModelessCommandData();
          data.CommandType = ModelessCommandType.PrintSelectedPageStatistics;
          data.SelectedPaneId = guid;
          ThisApplication.thisApp.GetDockableAPIUtility().RunModelessCommand(data);
          m_exEvent.Raise();
       }

     
       public string GetPageWpfData()
       {
          StringBuilder sb = new StringBuilder();
          sb.AppendLine("-WFP Page Info-");
          sb.AppendLine("FrameWorkElement.Width=" + this.Width);
          sb.AppendLine("FrameWorkElement.Height=" + this.Height);

          return sb.ToString();
       }
       public void SetInitialDockingParameters(int left, int right, int top, int bottom, DockPosition position, Guid targetGuid)
       {
          m_position = position;
          m_left= left;
          m_right = right;
          m_top = top;
          m_bottom = bottom;
          m_targetGuid = targetGuid;
       }
      #region Data

       private Guid m_targetGuid;
       private DockPosition m_position = DockPosition.Bottom;  
       private int m_left = 1;
       private int m_right = 1;
       private int m_top = 1;
       private int m_bottom = 1;

      #endregion

       private void wpf_stats_Click(object sender, RoutedEventArgs e)
       {
          Log.Message("***Main Pane WPF info***");
          Log.Message(ThisApplication.thisApp.GetMainWindow().GetPageWpfData());
       }


       private void btn_getById_Click(object sender, RoutedEventArgs e)
       {
          string guid = Microsoft.VisualBasic.Interaction.InputBox("Enter Pane Guid", Globals.ApplicationName, "");
          if (string.IsNullOrEmpty(guid))
             return;
          RaisePrintSpecificSummaryCommand(guid);
       }

       private void btn_listTabs_Click(object sender, RoutedEventArgs e)
       {
          Log.Message("***Dockable dialogs***");
        Log.Message(" Main dialog: " + Globals.sm_UserDockablePaneId.Guid.ToString());
        Log.Message(" Element View: " + Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ElementView.Guid.ToString());
        Log.Message(" System Navigator: " + Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.SystemNavigator.Guid.ToString());
        Log.Message(" Link Navigator: " + Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.HostByLinkNavigator.Guid.ToString());
        Log.Message(" Project Browser: " + Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ProjectBrowser.Guid.ToString());
        Log.Message(" Properties Palette: " + Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.PropertiesPalette.Guid.ToString());
        Log.Message(" Rebar Browser: " + Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.RebarBrowser.Guid.ToString());


       }

    }
}

