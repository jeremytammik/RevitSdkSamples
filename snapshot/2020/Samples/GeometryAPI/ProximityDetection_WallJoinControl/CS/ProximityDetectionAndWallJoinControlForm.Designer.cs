namespace Revit.SDK.Samples.ProximityDetection_WallJoinControl.CS
{
   partial class ProximityDetectionAndWallJoinControlForm
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
         this.buttonOK = new System.Windows.Forms.Button();
         this.buttonCancel = new System.Windows.Forms.Button();
         this.groupBoxResults = new System.Windows.Forms.GroupBox();
         this.treeViewResults = new System.Windows.Forms.TreeView();
         this.groupBoxOperations = new System.Windows.Forms.GroupBox();
         this.radioButtonCheckJoinedWalls = new System.Windows.Forms.RadioButton();
         this.radioButtonFindNearbyWalls = new System.Windows.Forms.RadioButton();
         this.radioButtonFindBlockingElements = new System.Windows.Forms.RadioButton();
         this.radioButtonFindColumnsInWall = new System.Windows.Forms.RadioButton();
         this.groupBoxDescription = new System.Windows.Forms.GroupBox();
         this.labelDescription = new System.Windows.Forms.Label();
         this.groupBoxResults.SuspendLayout();
         this.groupBoxOperations.SuspendLayout();
         this.groupBoxDescription.SuspendLayout();
         this.SuspendLayout();
         // 
         // buttonOK
         // 
         this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.buttonOK.Location = new System.Drawing.Point(432, 463);
         this.buttonOK.Name = "buttonOK";
         this.buttonOK.Size = new System.Drawing.Size(75, 23);
         this.buttonOK.TabIndex = 0;
         this.buttonOK.Text = "&OK";
         this.buttonOK.UseVisualStyleBackColor = true;
         this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
         // 
         // buttonCancel
         // 
         this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.buttonCancel.Location = new System.Drawing.Point(513, 463);
         this.buttonCancel.Name = "buttonCancel";
         this.buttonCancel.Size = new System.Drawing.Size(75, 23);
         this.buttonCancel.TabIndex = 1;
         this.buttonCancel.Text = "&Cancel";
         this.buttonCancel.UseVisualStyleBackColor = true;
         // 
         // groupBoxResults
         // 
         this.groupBoxResults.Controls.Add(this.treeViewResults);
         this.groupBoxResults.Location = new System.Drawing.Point(257, 13);
         this.groupBoxResults.Name = "groupBoxResults";
         this.groupBoxResults.Size = new System.Drawing.Size(331, 439);
         this.groupBoxResults.TabIndex = 2;
         this.groupBoxResults.TabStop = false;
         this.groupBoxResults.Text = "Results";
         // 
         // treeViewResults
         // 
         this.treeViewResults.Location = new System.Drawing.Point(6, 19);
         this.treeViewResults.Name = "treeViewResults";
         this.treeViewResults.Size = new System.Drawing.Size(319, 414);
         this.treeViewResults.TabIndex = 0;
         // 
         // groupBoxOperations
         // 
         this.groupBoxOperations.Controls.Add(this.radioButtonCheckJoinedWalls);
         this.groupBoxOperations.Controls.Add(this.radioButtonFindNearbyWalls);
         this.groupBoxOperations.Controls.Add(this.radioButtonFindBlockingElements);
         this.groupBoxOperations.Controls.Add(this.radioButtonFindColumnsInWall);
         this.groupBoxOperations.Location = new System.Drawing.Point(7, 13);
         this.groupBoxOperations.Name = "groupBoxOperations";
         this.groupBoxOperations.Size = new System.Drawing.Size(244, 267);
         this.groupBoxOperations.TabIndex = 3;
         this.groupBoxOperations.TabStop = false;
         this.groupBoxOperations.Text = "Operations";
         // 
         // radioButtonCheckJoinedWalls
         // 
         this.radioButtonCheckJoinedWalls.AutoSize = true;
         this.radioButtonCheckJoinedWalls.Location = new System.Drawing.Point(17, 217);
         this.radioButtonCheckJoinedWalls.Name = "radioButtonCheckJoinedWalls";
         this.radioButtonCheckJoinedWalls.Size = new System.Drawing.Size(166, 17);
         this.radioButtonCheckJoinedWalls.TabIndex = 3;
         this.radioButtonCheckJoinedWalls.TabStop = true;
         this.radioButtonCheckJoinedWalls.Text = "Check walls join/disjoin states";
         this.radioButtonCheckJoinedWalls.UseVisualStyleBackColor = true;
         this.radioButtonCheckJoinedWalls.CheckedChanged += new System.EventHandler(this.radioButtonCheckJoinedWalls_CheckedChanged);
         // 
         // radioButtonFindNearbyWalls
         // 
         this.radioButtonFindNearbyWalls.AutoSize = true;
         this.radioButtonFindNearbyWalls.Location = new System.Drawing.Point(17, 157);
         this.radioButtonFindNearbyWalls.Name = "radioButtonFindNearbyWalls";
         this.radioButtonFindNearbyWalls.Size = new System.Drawing.Size(210, 17);
         this.radioButtonFindNearbyWalls.TabIndex = 2;
         this.radioButtonFindNearbyWalls.TabStop = true;
         this.radioButtonFindNearbyWalls.Text = "Find walls (nearly joined to) end of walls";
         this.radioButtonFindNearbyWalls.UseVisualStyleBackColor = true;
         this.radioButtonFindNearbyWalls.CheckedChanged += new System.EventHandler(this.radioButtonFindNearbyWalls_CheckedChanged);
         // 
         // radioButtonFindBlockingElements
         // 
         this.radioButtonFindBlockingElements.AutoSize = true;
         this.radioButtonFindBlockingElements.Location = new System.Drawing.Point(17, 97);
         this.radioButtonFindBlockingElements.Name = "radioButtonFindBlockingElements";
         this.radioButtonFindBlockingElements.Size = new System.Drawing.Size(167, 17);
         this.radioButtonFindBlockingElements.TabIndex = 1;
         this.radioButtonFindBlockingElements.TabStop = true;
         this.radioButtonFindBlockingElements.Text = "Find elements blocking egress";
         this.radioButtonFindBlockingElements.UseVisualStyleBackColor = true;
         this.radioButtonFindBlockingElements.CheckedChanged += new System.EventHandler(this.radioButtonFindBlockingElements_CheckedChanged);
         // 
         // radioButtonFindColumnsInWall
         // 
         this.radioButtonFindColumnsInWall.AutoSize = true;
         this.radioButtonFindColumnsInWall.Checked = true;
         this.radioButtonFindColumnsInWall.Location = new System.Drawing.Point(17, 37);
         this.radioButtonFindColumnsInWall.Name = "radioButtonFindColumnsInWall";
         this.radioButtonFindColumnsInWall.Size = new System.Drawing.Size(119, 17);
         this.radioButtonFindColumnsInWall.TabIndex = 0;
         this.radioButtonFindColumnsInWall.TabStop = true;
         this.radioButtonFindColumnsInWall.Text = "Find columns in wall";
         this.radioButtonFindColumnsInWall.UseVisualStyleBackColor = true;
         this.radioButtonFindColumnsInWall.CheckedChanged += new System.EventHandler(this.radioButtonFindColumnsInWall_CheckedChanged);
         // 
         // groupBoxDescription
         // 
         this.groupBoxDescription.Controls.Add(this.labelDescription);
         this.groupBoxDescription.Location = new System.Drawing.Point(8, 297);
         this.groupBoxDescription.Name = "groupBoxDescription";
         this.groupBoxDescription.Size = new System.Drawing.Size(243, 154);
         this.groupBoxDescription.TabIndex = 4;
         this.groupBoxDescription.TabStop = false;
         this.groupBoxDescription.Text = "Description";
         // 
         // labelDescription
         // 
         this.labelDescription.AutoSize = true;
         this.labelDescription.Location = new System.Drawing.Point(13, 25);
         this.labelDescription.Name = "labelDescription";
         this.labelDescription.Size = new System.Drawing.Size(101, 13);
         this.labelDescription.TabIndex = 0;
         this.labelDescription.Text = "Find columns in wall";
         // 
         // ProximityDetectionAndWallJoinControlForm
         // 
         this.AcceptButton = this.buttonOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.buttonCancel;
         this.ClientSize = new System.Drawing.Size(600, 498);
         this.Controls.Add(this.groupBoxDescription);
         this.Controls.Add(this.groupBoxOperations);
         this.Controls.Add(this.groupBoxResults);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.buttonOK);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "ProximityDetectionAndWallJoinControlForm";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "ProximityDetectionAndWallDisjoinForm";
         this.groupBoxResults.ResumeLayout(false);
         this.groupBoxOperations.ResumeLayout(false);
         this.groupBoxOperations.PerformLayout();
         this.groupBoxDescription.ResumeLayout(false);
         this.groupBoxDescription.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button buttonOK;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.GroupBox groupBoxResults;
      private System.Windows.Forms.GroupBox groupBoxOperations;
      private System.Windows.Forms.GroupBox groupBoxDescription;
      private System.Windows.Forms.RadioButton radioButtonFindColumnsInWall;
      private System.Windows.Forms.RadioButton radioButtonFindBlockingElements;
      private System.Windows.Forms.RadioButton radioButtonFindNearbyWalls;
      private System.Windows.Forms.RadioButton radioButtonCheckJoinedWalls;
      private System.Windows.Forms.Label labelDescription;
      private System.Windows.Forms.TreeView treeViewResults;
   }
}