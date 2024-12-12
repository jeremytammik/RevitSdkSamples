namespace Revit.SDK.Samples.FamilyParametersOrder.CS
{
   partial class SortLoadedFamiliesParamsForm
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
         this.A_ZBtn = new System.Windows.Forms.Button();
         this.CloseBtn = new System.Windows.Forms.Button();
         this.Z_ABtn = new System.Windows.Forms.Button();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // A_ZBtn
         // 
         this.A_ZBtn.Location = new System.Drawing.Point(21, 29);
         this.A_ZBtn.Name = "A_ZBtn";
         this.A_ZBtn.Size = new System.Drawing.Size(75, 23);
         this.A_ZBtn.TabIndex = 6;
         this.A_ZBtn.Text = "&A-->Z";
         this.A_ZBtn.UseVisualStyleBackColor = true;
         this.A_ZBtn.Click += new System.EventHandler(this.A_ZBtn_Click);
         // 
         // CloseBtn
         // 
         this.CloseBtn.Location = new System.Drawing.Point(279, 29);
         this.CloseBtn.Name = "CloseBtn";
         this.CloseBtn.Size = new System.Drawing.Size(75, 23);
         this.CloseBtn.TabIndex = 4;
         this.CloseBtn.Text = "&Close";
         this.CloseBtn.UseVisualStyleBackColor = true;
         this.CloseBtn.Click += new System.EventHandler(this.closeBtn_Click);
         // 
         // Z_ABtn
         // 
         this.Z_ABtn.Location = new System.Drawing.Point(150, 29);
         this.Z_ABtn.Name = "Z_ABtn";
         this.Z_ABtn.Size = new System.Drawing.Size(75, 23);
         this.Z_ABtn.TabIndex = 5;
         this.Z_ABtn.Text = "&Z-->A";
         this.Z_ABtn.UseVisualStyleBackColor = true;
         this.Z_ABtn.Click += new System.EventHandler(this.Z_ABtn_Click);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.A_ZBtn);
         this.groupBox1.Controls.Add(this.Z_ABtn);
         this.groupBox1.Controls.Add(this.CloseBtn);
         this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupBox1.Location = new System.Drawing.Point(0, 0);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(372, 75);
         this.groupBox1.TabIndex = 7;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Parameters Order";
         // 
         // SortLoadedFamiliesParamsForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(372, 75);
         this.Controls.Add(this.groupBox1);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "SortLoadedFamiliesParamsForm";
         this.ShowIcon = false;
         this.Text = "Sort Loaded Families Parameters";
         this.groupBox1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button A_ZBtn;
      private System.Windows.Forms.Button CloseBtn;
      private System.Windows.Forms.Button Z_ABtn;
      private System.Windows.Forms.GroupBox groupBox1;
   }
}