//
// (C) Copyright 2003-2020 by Autodesk, Inc. All rights reserved.
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

using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.CloudAPISample.CS.View
{
   /// <summary>
   ///    Interaction logic for ViewSamplePortal.xaml
   /// </summary>
   public partial class ViewSamplePortal : Window
   {
      /// <summary>
      ///    Constructor for ViewSamplePortal
      ///    ViewSamplePortal is a child window aggregating all sample case with tabs
      /// </summary>
      public ViewSamplePortal(UIApplication app)
      {
         InitializeComponent();
      }

      /// <summary>
      ///    Add a sample tab
      /// </summary>
      /// <param name="title">The sample title</param>
      /// <param name="control">The root control for this sample</param>
      public void AddTab(string title, UserControl control)
      {
         var item = new TabItem {Header = title};
         ContentTabs.Items.Add(item);
         item.Content = control;
      }
   }
}