//
// (C) Copyright 2003-2023 by Autodesk, Inc. All rights reserved.
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
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Revit.SDK.Samples.CloudAPISample.CS.Coroutine;
using Revit.SDK.Samples.CloudAPISample.CS.Migration;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Revit.SDK.Samples.CloudAPISample.CS.View
{
   /// <summary>
   ///    Interaction logic for ViewMigrationToBim360.xaml
   /// </summary>
   public partial class ViewMigrationToBim360 : UserControl
   {
      /// <summary>
      ///    The view for <see cref="MigrationToBim360" />
      /// </summary>
      public ViewMigrationToBim360(MigrationToBim360 sampleContext)
      {
         DataContext = sampleContext;
         InitializeComponent();
      }

      /// <summary>
      ///    Update progress for uploading process.
      /// </summary>
      /// <param name="status">Text prompt to indicate the process</param>
      /// <param name="progress">The percentage progress, from 0 to 100</param>
      public void UpdateUploadingProgress(string status, int progress)
      {
         lbUploadStatus.Content = status;
         pbUploading.Value = progress;
      }

      /// <summary>
      ///    Update progress for reloading process
      /// </summary>
      /// <param name="status">Text prompt to indicate the process</param>
      /// <param name="progress">The percentage progress, from 0 to 100</param>
      public void UpdateReloadingProgress(string status, int progress)
      {
         lbReloadStatus.Content = status;
         pbReloading.Value = progress;
      }

      private void OnBtnBrowseDirectory_Click(object sender, RoutedEventArgs e)
      {
         using (var fbd = new FolderBrowserDialog())
         {
            var result = fbd.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
               tbLocalFolder.Text = fbd.SelectedPath;
         }
      }

      private void OnBtnReveal_Click(object sender, RoutedEventArgs e)
      {
         if (!Directory.Exists(tbLocalFolder.Text))
            MessageBox.Show("Invalid folder path");
         else
            Process.Start(tbLocalFolder.Text);
      }

      private void OnBtnConfig_Click(object sender, RoutedEventArgs e)
      {
         var ctx = (MigrationToBim360) DataContext;
         var viewConfiguration = new ViewInputMigrationInfo(ctx);
         viewConfiguration.Show();
      }

      private void OnBtnRemoveRule_Click(object sender, RoutedEventArgs e)
      {
         var model = ((MigrationToBim360) DataContext).Model;
         if (lvRules.SelectedIndex >= 0 && lvRules.SelectedIndex < model.Rules.Count)
            model.Rules.RemoveAt(lvRules.SelectedIndex);
      }

      private void OnBtnAddRule_Click(object sender, RoutedEventArgs e)
      {
         var model = ((MigrationToBim360) DataContext).Model;
         model.Rules.Add(new MigrationRule());
      }

      private void OnBtnUpload_Click(object sender, RoutedEventArgs e)
      {
         if (DataContext is MigrationToBim360 sampleContext)
         {
            if (sampleContext.Model.Rules.Count <= 0)
            {
               MessageBox.Show("Please add at least 1 migration rule.");
               return;
            }

            // Upload local models to target project
            var localDir = tbLocalFolder.Text;
            var sAccountId = sampleContext.Model.AccountGuid;
            var sProjectId = sampleContext.Model.ProjectGuid;

            if (Guid.TryParse(sAccountId, out var accountId) && Guid.TryParse(sProjectId, out var projectId))
               CoroutineScheduler.StartCoroutine(sampleContext.Upload(localDir, accountId, projectId,
                  sampleContext.Model.Rules));
         }
      }

      private void OnBtnRefresh_Click(object sender, RoutedEventArgs e)
      {
         if (DataContext is MigrationToBim360 sampleContext)
         {
            var localDir = tbLocalFolder.Text;
            var sProjectId = sampleContext.Model.ProjectGuid;

            if (Guid.TryParse(sProjectId, out var projectId))
               CoroutineScheduler.StartCoroutine(sampleContext.ReloadLinks(localDir, projectId));
         }
      }
   }
}