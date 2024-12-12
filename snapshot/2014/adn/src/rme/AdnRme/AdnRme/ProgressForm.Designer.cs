namespace AdnRme
{
    partial class ProgressForm
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
          this.progressBar1 = new System.Windows.Forms.ProgressBar();
          this.label1 = new System.Windows.Forms.Label();
          this.SuspendLayout();
          // 
          // progressBar1
          // 
          this.progressBar1.Location = new System.Drawing.Point( 11, 25 );
          this.progressBar1.Name = "progressBar1";
          this.progressBar1.Size = new System.Drawing.Size( 406, 23 );
          this.progressBar1.TabIndex = 0;
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point( 8, 7 );
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size( 68, 13 );
          this.label1.TabIndex = 1;
          this.label1.Text = "Processing...";
          // 
          // ProgressForm
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size( 429, 61 );
          this.ControlBox = false;
          this.Controls.Add( this.label1 );
          this.Controls.Add( this.progressBar1 );
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "ProgressForm";
          this.ShowIcon = false;
          this.ShowInTaskbar = false;
          this.Text = "Working...";
          this.TopMost = true;
          this.ResumeLayout( false );
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
