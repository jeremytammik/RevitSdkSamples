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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.RevitAddIns;
using System.IO;
using System.Diagnostics;

namespace Revit.SDK.Samples.RevitAddInUtilitySample.CS
{
   /// <summary>
   /// Demonstrates how to use RevitAddInUtility.dll to create or edit addin manifest file.
   /// </summary>
   public partial class RevitAddInUtilitySampleForm : Form
   {
      public RevitAddInUtilitySampleForm()
      {
         InitializeComponent();
         this.AddInsInfoButton.Enabled = false;
         this.OpenAddInFileButton.Enabled = false;
      }

      /// <summary>
      /// Create a .addin manifest file when user push this button.
      /// The new created manifest file contains an external command and an external application.
      /// </summary>
      private void CreateAddInManifestButton_Click(object sender, EventArgs e)
      {
         RevitAddInManifest Manifest = new RevitAddInManifest();

         FileInfo fileInfo = new FileInfo("..\\ExternalCommandRegistration\\ExternalCommandRegistration.dll");

         //create an external application
         RevitAddInApplication application1 = new RevitAddInApplication(
            "ExternalApplication", fileInfo.FullName, Guid.NewGuid(), 
            "Revit.SDK.Samples.ExternalCommandRegistration.CS.ExternalApplicationClass", "adsk");

         //create an external command to create a wall
         //This command will not be visible when there is no active document. 
         //And this command will be disabled if user selected a wall. 
         RevitAddInCommand command1 = new RevitAddInCommand(
            fileInfo.FullName, Guid.NewGuid(),
            "Revit.SDK.Samples.ExternalCommandRegistration.CS.ExternalCommandCreateWall", "adsk");
         command1.Description = "A simple external command which is used to create a wall.";
         command1.Text = "@createWallText";
         command1.AvailabilityClassName = "Revit.SDK.Samples.ExternalCommandRegistration.CS.WallSelection";
         command1.LanguageType = LanguageType.English_USA;
         command1.LargeImage = "@CreateWall";
         command1.TooltipImage = "@CreateWallTooltip";
         command1.VisibilityMode = VisibilityMode.NotVisibleWhenNoActiveDocument;
         command1.LongDescription = "This command will not be visible in Revit Structure or there is no active document.";
         command1.LongDescription += " And this command will be disabled if user selected a wall. ";

         //create an external command to show a message box
         //This command will not be visible Family Document or no active document.
         //And this command will be disabled if the active view is not a 3D view. ";
         RevitAddInCommand command2 = new RevitAddInCommand(
            fileInfo.FullName, Guid.NewGuid(),
            "Revit.SDK.Samples.ExternalCommandRegistration.CS.ExternalCommand3DView", "adsk");
         command2.Description = "A simple external command which show a message box.";
         command2.Text = "@view3DText";
         command2.AvailabilityClassName = "Revit.SDK.Samples.ExternalCommandRegistration.CS.View3D";
         command2.LargeImage = "@View3D";
         command2.LanguageType = LanguageType.English_USA;
         command2.VisibilityMode = VisibilityMode.NotVisibleInFamily | VisibilityMode.NotVisibleWhenNoActiveDocument;
         command2.LongDescription = "This command will not be visible in Revit MEP, Family Document or no active document.";
         command2.LongDescription += " And this command will be disabled if the active view is not a 3D view. ";

         //add both applications and commands into addin manifest
         Manifest.AddInApplications.Add(application1);
         Manifest.AddInCommands.Add(command1);
         Manifest.AddInCommands.Add(command2);

         //save addin manifest in same place with RevitAddInUtilitySample.exe
         fileInfo = new FileInfo("ExteranlCommand.Sample.addin");
         Manifest.SaveAs(fileInfo.FullName);
         AddInsInfoButton_Click(null, null); //show addins information in the tree view
         this.AddInsInfoButton.Enabled = true;
         this.OpenAddInFileButton.Enabled = true;
      }

      /// <summary>
      /// Show AddIns information of new create manifest file in the tree view
      /// </summary>
      private void AddInsInfoButton_Click(object sender, EventArgs e)
      {
         FileInfo fileInfo = new FileInfo("ExteranlCommand.Sample.addin");
         RevitAddInManifest revitAddInManifest =
               Autodesk.RevitAddIns.AddInManifestUtility.GetRevitAddInManifest(fileInfo.FullName);

         this.treeView1.Nodes.Clear();
         if (revitAddInManifest.AddInApplications.Count >= 1)
         {
            TreeNode apps= this.treeView1.Nodes.Add("External Applications");
            foreach (RevitAddInApplication app in revitAddInManifest.AddInApplications)
            {
               TreeNode appNode = apps.Nodes.Add(app.Name);
               appNode.Nodes.Add("Name: " + app.Name);
               appNode.Nodes.Add("Assembly: " + app.Assembly);
               appNode.Nodes.Add("AddInId: " + app.AddInId);
               appNode.Nodes.Add("Full Class Name: " + app.FullClassName);
            }
         }

         if (revitAddInManifest.AddInCommands.Count >= 1)
         {
            TreeNode cmds = this.treeView1.Nodes.Add("External Commands");
            foreach (RevitAddInCommand cmd in revitAddInManifest.AddInCommands)
            {
               TreeNode cmdNode = cmds.Nodes.Add(cmd.Text);
               cmdNode.Nodes.Add("Assembly: " + cmd.Assembly);
               cmdNode.Nodes.Add("AddInId: " + cmd.AddInId);
               cmdNode.Nodes.Add("Full Class Name: " + cmd.FullClassName);
               cmdNode.Nodes.Add("Text: " + cmd.Text);
               cmdNode.Nodes.Add("Description: " + cmd.Description);
               cmdNode.Nodes.Add("LanguageType: " + cmd.LanguageType);
               cmdNode.Nodes.Add("LargeImage: " + cmd.LargeImage);
               cmdNode.Nodes.Add("LongDescription: " + cmd.LongDescription);
               cmdNode.Nodes.Add("TooltipImage: " + cmd.TooltipImage);
               cmdNode.Nodes.Add("VisibilityMode: " + cmd.VisibilityMode.ToString());
               cmdNode.Nodes.Add("AvailabilityClassName: " + cmd.AvailabilityClassName);
            }
         }
      }

      /// <summary>
      /// Show all the installed Revit product information of user's PC in the tree view
      /// </summary>
      private void RevitProductsButton_Click(object sender, EventArgs e)
      {
         this.treeView1.Nodes.Clear();
         TreeNode allProductsNode = this.treeView1.Nodes.Add("Installed Revit Products: ");
         foreach (RevitProduct revitProduct in RevitProductUtility.GetAllInstalledRevitProducts())
         {
            TreeNode productNode = allProductsNode.Nodes.Add(revitProduct.Name);
            productNode.Nodes.Add("Product Name: " + revitProduct.Name);
            productNode.Nodes.Add("AllUsersAddInFolder: " + revitProduct.AllUsersAddInFolder);
            productNode.Nodes.Add("Architecture: " + revitProduct.Architecture);
            //productNode.Nodes.Add("Build: " + revitProduct.Build); // deprecated
            productNode.Nodes.Add("Current User AddIn Folder: " + revitProduct.CurrentUserAddInFolder);
            productNode.Nodes.Add("Install Location: " + revitProduct.InstallLocation);
            productNode.Nodes.Add("ProductCode: " + revitProduct.ProductCode);
            productNode.Nodes.Add("Version: " + revitProduct.Version);
         }
      }

      /// <summary>
      /// Open new created AddIn manifest file - ExteranlCommand.Sample.addin
      /// </summary>
      private void OpenAddInFileButton_Click(object sender, EventArgs e)
      {
         FileInfo fileInfo = new FileInfo("ExteranlCommand.Sample.addin");
         Process.Start(fileInfo.FullName);
      }
   }
}
