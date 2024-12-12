//
// (C) Copyright 2003-2014 by Autodesk, Inc. All rights reserved.
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Revit.SDK.Samples.DockableDialogs.CS
{
   /// <summary>
   /// Interaction logic for DockingSetupDialog.xaml
   /// </summary>
   public partial class DockingSetupDialog : Window
   {
      public DockingSetupDialog()
      {
         InitializeComponent();
         this.tb_newGuid.Text = Globals.sm_UserDockablePaneId.Guid.ToString();
      }

      /// <summary>
      /// Take user-input data for docking dialog choices and attempt to parse it
      /// into higher-level data for later use.
      /// </summary>
      private void btn_ok_Click(object sender, RoutedEventArgs e)
      {
         int.TryParse(this.tb_left.Text, out m_left);
         int.TryParse(this.tb_right.Text, out m_right);
         int.TryParse(this.tb_top.Text, out m_top);
         int.TryParse(this.tb_bottom.Text, out m_bottom);

         if (!string.IsNullOrEmpty(this.tb_newGuid.Text))
            m_mainPageGuid = this.tb_newGuid.Text;

         if (!string.IsNullOrEmpty(this.tb_targetGuid.Text))
            m_targetGuidString = this.tb_targetGuid.Text;

         else if (rb_ElementView.IsChecked == true)
            m_targetGuidString = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ElementView.Guid.ToString();

         else if (rb_SystemNav.IsChecked == true)
            m_targetGuidString = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.SystemNavigator.Guid.ToString();

         else if (rb_HostByLinkNavigator.IsChecked == true)
            m_targetGuidString = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.HostByLinkNavigator.Guid.ToString();

         else if (this.rb_ProjectBrowser.IsChecked == true)
            m_targetGuidString = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ProjectBrowser.Guid.ToString();

         else if (this.rb_PropertiesPalette.IsChecked == true)
            m_targetGuidString = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.PropertiesPalette.Guid.ToString();

         else if (this.rb_RebarBrowser.IsChecked == true)
            m_targetGuidString = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.RebarBrowser.Guid.ToString();

         else
            m_targetGuidString = "null";

         if (rb_bottom.IsChecked == true)
            m_dockPosition = Autodesk.Revit.UI.DockPosition.Bottom;
         else if (rb_left.IsChecked == true)
            m_dockPosition = Autodesk.Revit.UI.DockPosition.Left;
         else if (rb_right.IsChecked == true)
            m_dockPosition = Autodesk.Revit.UI.DockPosition.Right;
         else if (rb_top.IsChecked == true)
            m_dockPosition = Autodesk.Revit.UI.DockPosition.Top;
         else if (rb_tabbed.IsChecked == true)
            m_dockPosition = Autodesk.Revit.UI.DockPosition.Tabbed;
         else
            m_dockPosition = Autodesk.Revit.UI.DockPosition.Floating;

         this.DialogResult = true;
         this.Close();
      }

      private int m_left;
      private int m_right;
      private int m_top;
      private int m_bottom;
      private string m_targetGuidString;
      private string m_mainPageGuid;
      private  Autodesk.Revit.UI.DockPosition m_dockPosition;

      public int FloatLeft { get { return m_left; } }
      public int FloatRight { get { return m_right; } }
      public int FloatTop { get { return m_top; } }
      public int FloatBottom { get { return m_bottom; } }

      /// <summary>
      /// The guid of the main docking page.
      /// </summary>
      public Guid MainPageGuid
      {
         get
         {
            Guid retval = Guid.Empty;
            if (m_mainPageGuid == "null")
               return retval;
            else
            {

               try
               {
                  retval = new Guid(m_mainPageGuid);

               }
               catch (Exception)
               {
               }
               return retval;
            }
         }
      }

      /// <summary>
      /// The guid of a tab-behind system pane.
      /// </summary>
      public Guid TargetGuid
      {
         get
         {
            Guid retval = Guid.Empty;
            if (m_targetGuidString == "null")
               return retval;
            else
            {

               try
               {
                  retval = new Guid(m_targetGuidString);

               }
               catch (Exception)
               {
               }
               return retval;
            }
         }
      }

      public Autodesk.Revit.UI.DockPosition DockPosition { get { return m_dockPosition; } }
     
      private void SetFloatingCoordsStates(bool enabled)
      {
         tb_right.IsEnabled = enabled;
         tb_left.IsEnabled = enabled;
         tb_top.IsEnabled = enabled;
         tb_bottom.IsEnabled = enabled;
      }

      private void SetTabTargetStates(bool enabled)
      {
         rb_ElementView.IsEnabled = enabled;
         rb_HostByLinkNavigator.IsEnabled = enabled;
         rb_ProjectBrowser.IsEnabled = enabled;
         rb_PropertiesPalette.IsEnabled = enabled;
         rb_RebarBrowser.IsEnabled = enabled;
         rb_SystemNav.IsEnabled = enabled;
         rb_Null.IsEnabled = enabled;
      }
      private void rb_top_Checked(object sender, RoutedEventArgs e)
      {
         SetFloatingCoordsStates(false);
         SetTabTargetStates(false);
      }

      private void rb_tabbed_Checked(object sender, RoutedEventArgs e)
      {
         SetFloatingCoordsStates(false);
         SetTabTargetStates(true);
      }


      private void rb_left_Checked(object sender, RoutedEventArgs e)
      {
         SetFloatingCoordsStates(false);
         SetTabTargetStates(false);
      }

      private void rb_right_Checked(object sender, RoutedEventArgs e)
      {
         SetFloatingCoordsStates(false);
         SetTabTargetStates(false);
      }

      private void rb_bottom_Checked(object sender, RoutedEventArgs e)
      {
         SetFloatingCoordsStates(false);
         SetTabTargetStates(false);
      }

      private void rb_floating_Checked(object sender, RoutedEventArgs e)
      {
         SetFloatingCoordsStates(true);
         SetTabTargetStates(false);
      }
   }
}
