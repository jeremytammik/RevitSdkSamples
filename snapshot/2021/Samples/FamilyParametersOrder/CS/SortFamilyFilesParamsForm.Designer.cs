namespace Revit.SDK.Samples.FamilyParametersOrder.CS
{
   partial class SortFamilyFilesParamsForm
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
         this.label1 = new System.Windows.Forms.Label();
         this.directoryTxt = new System.Windows.Forms.TextBox();
         this.browseBtn = new System.Windows.Forms.Button();
         this.Z_ABtn = new System.Windows.Forms.Button();
         this.closeBtn = new System.Windows.Forms.Button();
         this.A_ZBtn = new System.Windows.Forms.Button();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.groupBox1.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(18, 30);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(49, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Directory";
         // 
         // directoryTxt
         // 
         this.directoryTxt.Location = new System.Drawing.Point(75, 26);
         this.directoryTxt.Name = "directoryTxt";
         this.directoryTxt.Size = new System.Drawing.Size(214, 20);
         this.directoryTxt.TabIndex = 1;
         // 
         // browseBtn
         // 
         this.browseBtn.Location = new System.Drawing.Point(295, 25);
         this.browseBtn.Name = "browseBtn";
         this.browseBtn.Size = new System.Drawing.Size(75, 23);
         this.browseBtn.TabIndex = 2;
         this.browseBtn.Text = "&Browse";
         this.browseBtn.UseVisualStyleBackColor = true;
         this.browseBtn.Click += new System.EventHandler(this.browseBtn_Click);
         // 
         // Z_ABtn
         // 
         this.Z_ABtn.Location = new System.Drawing.Point(158, 36);
         this.Z_ABtn.Name = "Z_ABtn";
         this.Z_ABtn.Size = new System.Drawing.Size(75, 23);
         this.Z_ABtn.TabIndex = 2;
         this.Z_ABtn.Text = "&Z-->A";
         this.Z_ABtn.UseVisualStyleBackColor = true;
         this.Z_ABtn.Click += new System.EventHandler(this.Z_ABtn_Click);
         // 
         // closeBtn
         // 
         this.closeBtn.Location = new System.Drawing.Point(295, 36);
         this.closeBtn.Name = "closeBtn";
         this.closeBtn.Size = new System.Drawing.Size(75, 23);
         this.closeBtn.TabIndex = 2;
         this.closeBtn.Text = "&Close";
         this.closeBtn.UseVisualStyleBackColor = true;
         this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
         // 
         // A_ZBtn
         // 
         this.A_ZBtn.Location = new System.Drawing.Point(21, 36);
         this.A_ZBtn.Name = "A_ZBtn";
         this.A_ZBtn.Size = new System.Drawing.Size(75, 23);
         this.A_ZBtn.TabIndex = 3;
         this.A_ZBtn.Text = "&A-->Z";
         this.A_ZBtn.UseVisualStyleBackColor = true;
         this.A_ZBtn.Click += new System.EventHandler(this.A_ZBtn_Click);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.label1);
         this.groupBox1.Controls.Add(this.browseBtn);
         this.groupBox1.Controls.Add(this.directoryTxt);
         this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
         this.groupBox1.Location = new System.Drawing.Point(0, 0);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(403, 72);
         this.groupBox1.TabIndex = 4;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Family Files Path";
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.A_ZBtn);
         this.groupBox2.Controls.Add(this.Z_ABtn);
         this.groupBox2.Controls.Add(this.closeBtn);
         this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.groupBox2.Location = new System.Drawing.Point(0, 66);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(403, 87);
         this.groupBox2.TabIndex = 5;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Parameters Order";
         // 
         // SortFamilyFilesParamsForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(403, 153);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.groupBox1);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "SortFamilyFilesParamsForm";
         this.ShowIcon = false;
         this.Text = "Sort Family Files Parameters";
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.groupBox2.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox directoryTxt;
      private System.Windows.Forms.Button browseBtn;
      private System.Windows.Forms.Button Z_ABtn;
      private System.Windows.Forms.Button closeBtn;
      private System.Windows.Forms.Button A_ZBtn;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.GroupBox groupBox2;
   }
}