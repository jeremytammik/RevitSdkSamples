namespace APIAppStartup
{
   partial class SplashWindow
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
         this.Version = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // Version
         // 
         this.Version.AutoSize = true;
         this.Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Version.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
         this.Version.Location = new System.Drawing.Point(258, 291);
         this.Version.Name = "Version";
         this.Version.Size = new System.Drawing.Size(71, 24);
         this.Version.TabIndex = 0;
         this.Version.Text = "version";
         this.Version.UseWaitCursor = true;
         // 
         // SplashWindow
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Desktop;
         this.BackgroundImage = global::APIAppStartup.Properties.Resources.test_rvt_Revit_black_93694;
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
         this.ClientSize = new System.Drawing.Size(682, 473);
         this.ControlBox = false;
         this.Controls.Add(this.Version);
         this.DoubleBuffered = true;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "SplashWindow";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "SplashWindow";
         this.TopMost = true;
         this.UseWaitCursor = true;
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label Version;
   }
}