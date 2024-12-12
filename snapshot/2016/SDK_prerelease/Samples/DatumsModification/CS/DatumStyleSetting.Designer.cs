
namespace Revit.SDK.Samples.DatumsModification.CS
{
   partial class DatumStyleSetting
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
         this.datumLeftStyleListBox = new System.Windows.Forms.CheckedListBox();
         this.datumRightStyleListBox = new System.Windows.Forms.CheckedListBox();
         this.okButton = new System.Windows.Forms.Button();
         this.button2 = new System.Windows.Forms.Button();
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         this.label1 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
         this.SuspendLayout();
         // 
         // datumLeftStyleListBox
         // 
         this.datumLeftStyleListBox.BackColor = System.Drawing.SystemColors.Control;
         this.datumLeftStyleListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
         this.datumLeftStyleListBox.CheckOnClick = true;
         this.datumLeftStyleListBox.FormattingEnabled = true;
         this.datumLeftStyleListBox.Items.AddRange(new object[] {
            "Show Bubble",
            "Add Elbow",
            "2D Extents"});
         this.datumLeftStyleListBox.Location = new System.Drawing.Point(23, 44);
         this.datumLeftStyleListBox.Name = "datumLeftStyleListBox";
         this.datumLeftStyleListBox.Size = new System.Drawing.Size(88, 45);
         this.datumLeftStyleListBox.TabIndex = 1;
         this.datumLeftStyleListBox.TabStop = false;
         // 
         // datumRightStyleListBox
         // 
         this.datumRightStyleListBox.BackColor = System.Drawing.SystemColors.Control;
         this.datumRightStyleListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
         this.datumRightStyleListBox.CheckOnClick = true;
         this.datumRightStyleListBox.FormattingEnabled = true;
         this.datumRightStyleListBox.Items.AddRange(new object[] {
            "Show Bubble",
            "Add Elbow",
            "2D Extents"});
         this.datumRightStyleListBox.Location = new System.Drawing.Point(360, 44);
         this.datumRightStyleListBox.Name = "datumRightStyleListBox";
         this.datumRightStyleListBox.Size = new System.Drawing.Size(88, 45);
         this.datumRightStyleListBox.TabIndex = 2;
         this.datumRightStyleListBox.TabStop = false;
         // 
         // okButton
         // 
         this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.okButton.Location = new System.Drawing.Point(283, 115);
         this.okButton.Name = "okButton";
         this.okButton.Size = new System.Drawing.Size(75, 23);
         this.okButton.TabIndex = 3;
         this.okButton.Text = "OK";
         this.okButton.UseVisualStyleBackColor = true;
         this.okButton.Click += new System.EventHandler(this.okButtonClick);
         // 
         // button2
         // 
         this.button2.Location = new System.Drawing.Point(373, 115);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(75, 23);
         this.button2.TabIndex = 4;
         this.button2.Text = "Cancel";
         this.button2.UseVisualStyleBackColor = true;
         // 
         // pictureBox1
         // 
         this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
         this.pictureBox1.Image = global::Revit.SDK.Samples.DatumsModification.CS.Properties.Resources.grid;
         this.pictureBox1.Location = new System.Drawing.Point(110, 33);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(248, 68);
         this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.pictureBox1.TabIndex = 0;
         this.pictureBox1.TabStop = false;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(20, 9);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(183, 13);
         this.label1.TabIndex = 5;
         this.label1.Text = "Change the selected Datums to style:";
         // 
         // DatumStyleSetting
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(463, 147);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.button2);
         this.Controls.Add(this.okButton);
         this.Controls.Add(this.datumRightStyleListBox);
         this.Controls.Add(this.pictureBox1);
         this.Controls.Add(this.datumLeftStyleListBox);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "DatumStyleSetting";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Datum style setting";
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.CheckedListBox datumLeftStyleListBox;
      private System.Windows.Forms.CheckedListBox datumRightStyleListBox;
      private System.Windows.Forms.Button okButton;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.PictureBox pictureBox1;
      private System.Windows.Forms.Label label1;

   }
}