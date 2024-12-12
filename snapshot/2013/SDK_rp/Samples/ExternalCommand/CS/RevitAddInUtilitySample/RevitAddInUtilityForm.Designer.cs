//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
namespace Revit.SDK.Samples.RevitAddInUtilitySample.CS
{
   partial class RevitAddInUtilitySampleForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.CreateAddInManifestButton = new System.Windows.Forms.Button();
         this.AddInsInfoButton = new System.Windows.Forms.Button();
         this.RevitProductsButton = new System.Windows.Forms.Button();
         this.treeView1 = new System.Windows.Forms.TreeView();
         this.OpenAddInFileButton = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // CreateAddInManifestButton
         // 
         this.CreateAddInManifestButton.Location = new System.Drawing.Point(12, 12);
         this.CreateAddInManifestButton.Name = "CreateAddInManifestButton";
         this.CreateAddInManifestButton.Size = new System.Drawing.Size(118, 23);
         this.CreateAddInManifestButton.TabIndex = 0;
         this.CreateAddInManifestButton.Text = "Create Manifest File";
         this.CreateAddInManifestButton.UseVisualStyleBackColor = true;
         this.CreateAddInManifestButton.Click += new System.EventHandler(this.CreateAddInManifestButton_Click);
         // 
         // AddInsInfoButton
         // 
         this.AddInsInfoButton.Location = new System.Drawing.Point(136, 12);
         this.AddInsInfoButton.Name = "AddInsInfoButton";
         this.AddInsInfoButton.Size = new System.Drawing.Size(119, 23);
         this.AddInsInfoButton.TabIndex = 1;
         this.AddInsInfoButton.Text = "AddIns Info";
         this.AddInsInfoButton.UseVisualStyleBackColor = true;
         this.AddInsInfoButton.Click += new System.EventHandler(this.AddInsInfoButton_Click);
         // 
         // RevitProductsButton
         // 
         this.RevitProductsButton.Location = new System.Drawing.Point(261, 12);
         this.RevitProductsButton.Name = "RevitProductsButton";
         this.RevitProductsButton.Size = new System.Drawing.Size(119, 23);
         this.RevitProductsButton.TabIndex = 3;
         this.RevitProductsButton.Text = "Revit Products";
         this.RevitProductsButton.UseVisualStyleBackColor = true;
         this.RevitProductsButton.Click += new System.EventHandler(this.RevitProductsButton_Click);
         // 
         // treeView1
         // 
         this.treeView1.Location = new System.Drawing.Point(12, 53);
         this.treeView1.Name = "treeView1";
         this.treeView1.Size = new System.Drawing.Size(368, 286);
         this.treeView1.TabIndex = 5;
         // 
         // OpenAddInFileButton
         // 
         this.OpenAddInFileButton.Location = new System.Drawing.Point(261, 354);
         this.OpenAddInFileButton.Name = "OpenAddInFileButton";
         this.OpenAddInFileButton.Size = new System.Drawing.Size(119, 23);
         this.OpenAddInFileButton.TabIndex = 6;
         this.OpenAddInFileButton.Text = "Open AddIn File";
         this.OpenAddInFileButton.UseVisualStyleBackColor = true;
         this.OpenAddInFileButton.Click += new System.EventHandler(this.OpenAddInFileButton_Click);
         // 
         // RevitAddInUtilitySampleForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(392, 388);
         this.Controls.Add(this.OpenAddInFileButton);
         this.Controls.Add(this.treeView1);
         this.Controls.Add(this.RevitProductsButton);
         this.Controls.Add(this.AddInsInfoButton);
         this.Controls.Add(this.CreateAddInManifestButton);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Name = "RevitAddInUtilitySampleForm";
         this.Text = "RevitAddInUtility Sample";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button CreateAddInManifestButton;
      private System.Windows.Forms.Button AddInsInfoButton;
      private System.Windows.Forms.Button RevitProductsButton;
      private System.Windows.Forms.TreeView treeView1;
      private System.Windows.Forms.Button OpenAddInFileButton;
   }
}

