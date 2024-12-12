namespace SampleClipAngle
{
   partial class SampleClipUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleClipUI));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.axAstComboProfile1 = new AxASTCONTROLSLib.AxAstComboProfile();
            this.axBoltStandards1 = new AxASTCONTROLSLib.AxBoltStandards();
            this.axAstBMP1 = new AxASTCONTROLSLib.AxAstControls();
            ((System.ComponentModel.ISupportInitialize)(this.axAstComboProfile1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBoltStandards1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axAstBMP1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cleat profile";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Bolt Diameter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Bolt Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Bolt Grade";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Bolt Assembly";
            // 
            // axAstComboProfile1
            // 
            this.axAstComboProfile1.Location = new System.Drawing.Point(89, 11);
            this.axAstComboProfile1.Name = "axAstComboProfile1";
            this.axAstComboProfile1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAstComboProfile1.OcxState")));
            this.axAstComboProfile1.Size = new System.Drawing.Size(221, 100);
            this.axAstComboProfile1.TabIndex = 5;
            this.axAstComboProfile1.ProfileChanged += new System.EventHandler(this.axAstComboProfile1_ProfileChanged);
            // 
            // axBoltStandards1
            // 
            this.axBoltStandards1.Location = new System.Drawing.Point(89, 37);
            this.axBoltStandards1.Name = "axBoltStandards1";
            this.axBoltStandards1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axBoltStandards1.OcxState")));
            this.axBoltStandards1.Size = new System.Drawing.Size(221, 202);
            this.axBoltStandards1.TabIndex = 6;
            this.axBoltStandards1.BoltChanged += new System.EventHandler(this.axBoltStandards1_BoltChanged);
            // 
            // axAstBMP1
            // 
            this.axAstBMP1.Location = new System.Drawing.Point(316, 11);
            this.axAstBMP1.Name = "axAstBMP1";
            this.axAstBMP1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAstBMP1.OcxState")));
            this.axAstBMP1.Size = new System.Drawing.Size(200, 200);
            this.axAstBMP1.TabIndex = 7;
            // 
            // SampleClipUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 255);
            this.Controls.Add(this.axBoltStandards1);
            this.Controls.Add(this.axAstComboProfile1);
            this.Controls.Add(this.axAstBMP1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SampleClipUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "SampleClipUI";
            ((System.ComponentModel.ISupportInitialize)(this.axAstComboProfile1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBoltStandards1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axAstBMP1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
      private AxASTCONTROLSLib.AxBoltStandards axBoltStandards1;
      private AxASTCONTROLSLib.AxAstComboProfile axAstComboProfile1;
      private AxASTCONTROLSLib.AxAstControls axAstBMP1;
   }
}