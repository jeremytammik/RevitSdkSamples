namespace Revit.SDK.Samples.AppearanceAssetEditing.CS
{
   partial class AppearanceAssetEditingForm
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
         this.components = new System.ComponentModel.Container();
         this.lighterToolTip = new System.Windows.Forms.ToolTip(this.components);
         this.darkerToolTip = new System.Windows.Forms.ToolTip(this.components);
         this.buttonSelect = new System.Windows.Forms.Button();
         this.buttonDarker = new System.Windows.Forms.Button();
         this.buttonLighter = new System.Windows.Forms.Button();
         this.selectTooltip = new System.Windows.Forms.ToolTip(this.components);
         this.labelSelect = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // buttonSelect
         // 
         this.buttonSelect.Image = global::Revit.SDK.Samples.AppearanceAssetEditing.CS.Properties.Resources.element_select;
         this.buttonSelect.Location = new System.Drawing.Point(191, 17);
         this.buttonSelect.Name = "buttonSelect";
         this.buttonSelect.Size = new System.Drawing.Size(50, 30);
         this.buttonSelect.TabIndex = 2;
         this.selectTooltip.SetToolTip(this.buttonSelect, "Select the painted object to modify.");
         this.buttonSelect.UseVisualStyleBackColor = true;
         this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
         // 
         // buttonDarker
         // 
         this.buttonDarker.Enabled = false;
         this.buttonDarker.Image = global::Revit.SDK.Samples.AppearanceAssetEditing.CS.Properties.Resources.shortcut_remove;
         this.buttonDarker.Location = new System.Drawing.Point(126, 65);
         this.buttonDarker.Name = "buttonDarker";
         this.buttonDarker.Size = new System.Drawing.Size(25, 25);
         this.buttonDarker.TabIndex = 1;
         this.darkerToolTip.SetToolTip(this.buttonDarker, "Change Tint Color to Darker.");
         this.buttonDarker.UseVisualStyleBackColor = true;
         this.buttonDarker.Click += new System.EventHandler(this.buttonDarker_Click);
         // 
         // buttonLighter
         // 
         this.buttonLighter.Enabled = false;
         this.buttonLighter.Image = global::Revit.SDK.Samples.AppearanceAssetEditing.CS.Properties.Resources.shortcut_assign;
         this.buttonLighter.Location = new System.Drawing.Point(78, 65);
         this.buttonLighter.Name = "buttonLighter";
         this.buttonLighter.Size = new System.Drawing.Size(25, 25);
         this.buttonLighter.TabIndex = 0;
         this.lighterToolTip.SetToolTip(this.buttonLighter, "Change tint color to lighter.");
         this.buttonLighter.UseVisualStyleBackColor = true;
         this.buttonLighter.Click += new System.EventHandler(this.buttonLighter_Click);
         // 
         // labelSelect
         // 
         this.labelSelect.AutoSize = true;
         this.labelSelect.Location = new System.Drawing.Point(25, 26);
         this.labelSelect.Name = "labelSelect";
         this.labelSelect.Size = new System.Drawing.Size(160, 13);
         this.labelSelect.TabIndex = 3;
         this.labelSelect.Text = "Select a painted face for editing.";
         // 
         // AppearanceAssetEditingForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(259, 110);
         this.Controls.Add(this.labelSelect);
         this.Controls.Add(this.buttonSelect);
         this.Controls.Add(this.buttonDarker);
         this.Controls.Add(this.buttonLighter);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "AppearanceAssetEditingForm";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Appearance Asset Editing";
         this.TopMost = true;
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button buttonLighter;
      private System.Windows.Forms.Button buttonDarker;
      private System.Windows.Forms.ToolTip lighterToolTip;
      private System.Windows.Forms.ToolTip darkerToolTip;
      private System.Windows.Forms.Button buttonSelect;
      private System.Windows.Forms.ToolTip selectTooltip;
      private System.Windows.Forms.Label labelSelect;
   }
}