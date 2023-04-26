namespace SteelConnectionsJointExample
{
   partial class Page1
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page1));
         this.axAstUnitPlateThickness = new AxASTCONTROLSLib.AxAstUnitControl();
         this.axAstUnitPlateLength = new AxASTCONTROLSLib.AxAstUnitControl();
         this.axAstUnitPlateWidth = new AxASTCONTROLSLib.AxAstUnitControl();
         this.axAstUnitCutBack = new AxASTCONTROLSLib.AxAstUnitControl();
         this.axAnchor = new AxASTCONTROLSLib.AxBoltStandards();
         this.axAstControls1 = new AxASTCONTROLSLib.AxAstControls();
         ((System.ComponentModel.ISupportInitialize)(this.axAstUnitPlateThickness)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.axAstUnitPlateLength)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.axAstUnitPlateWidth)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.axAstUnitCutBack)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.axAnchor)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.axAstControls1)).BeginInit();
         this.SuspendLayout();
         // 
         // axAstUnitPlateThickness
         // 
         this.axAstUnitPlateThickness.Location = new System.Drawing.Point(13, 13);
         this.axAstUnitPlateThickness.Margin = new System.Windows.Forms.Padding(4);
         this.axAstUnitPlateThickness.Name = "axAstUnitPlateThickness";
         this.axAstUnitPlateThickness.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAstUnitPlateThickness.OcxState")));
         this.axAstUnitPlateThickness.Size = new System.Drawing.Size(281, 29);
         this.axAstUnitPlateThickness.TabIndex = 0;
         this.axAstUnitPlateThickness.ValueChanged += new System.EventHandler(this.axAstUnitPlateThickness_ValueChanged);
         // 
         // axAstUnitPlateLength
         // 
         this.axAstUnitPlateLength.Location = new System.Drawing.Point(13, 42);
         this.axAstUnitPlateLength.Margin = new System.Windows.Forms.Padding(4);
         this.axAstUnitPlateLength.Name = "axAstUnitPlateLength";
         this.axAstUnitPlateLength.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAstUnitPlateLength.OcxState")));
         this.axAstUnitPlateLength.Size = new System.Drawing.Size(281, 29);
         this.axAstUnitPlateLength.TabIndex = 1;
         this.axAstUnitPlateLength.ValueChanged += new System.EventHandler(this.axAstUnitPlateLength_ValueChanged);
         // 
         // axAstUnitPlateWidth
         // 
         this.axAstUnitPlateWidth.Location = new System.Drawing.Point(13, 71);
         this.axAstUnitPlateWidth.Margin = new System.Windows.Forms.Padding(4);
         this.axAstUnitPlateWidth.Name = "axAstUnitPlateWidth";
         this.axAstUnitPlateWidth.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAstUnitPlateWidth.OcxState")));
         this.axAstUnitPlateWidth.Size = new System.Drawing.Size(281, 29);
         this.axAstUnitPlateWidth.TabIndex = 2;
         this.axAstUnitPlateWidth.ValueChanged += new System.EventHandler(this.axAstUnitPlateWidth_ValueChanged);
         // 
         // axAstUnitCutBack
         // 
         this.axAstUnitCutBack.Location = new System.Drawing.Point(13, 100);
         this.axAstUnitCutBack.Margin = new System.Windows.Forms.Padding(4);
         this.axAstUnitCutBack.Name = "axAstUnitCutBack";
         this.axAstUnitCutBack.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAstUnitCutBack.OcxState")));
         this.axAstUnitCutBack.Size = new System.Drawing.Size(281, 29);
         this.axAstUnitCutBack.TabIndex = 3;
         this.axAstUnitCutBack.ValueChanged += new System.EventHandler(this.axAstUnitCutBack_ValueChanged);
         // 
         // axAnchor
         // 
         this.axAnchor.Location = new System.Drawing.Point(13, 130);
         this.axAnchor.Margin = new System.Windows.Forms.Padding(4);
         this.axAnchor.Name = "axAnchor";
         this.axAnchor.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAnchor.OcxState")));
         this.axAnchor.Size = new System.Drawing.Size(305, 151);
         this.axAnchor.TabIndex = 4;
         this.axAnchor.BoltChanged += new System.EventHandler(this.axAnchor_BoltChanged);
         // 
         // axAstControls1
         // 
         this.axAstControls1.Location = new System.Drawing.Point(330, 17);
         this.axAstControls1.Name = "axAstControls1";
         this.axAstControls1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAstControls1.OcxState")));
         this.axAstControls1.Size = new System.Drawing.Size(269, 243);
         this.axAstControls1.TabIndex = 5;
         // 
         // Page1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(624, 354);
         this.Controls.Add(this.axAstControls1);
         this.Controls.Add(this.axAnchor);
         this.Controls.Add(this.axAstUnitCutBack);
         this.Controls.Add(this.axAstUnitPlateWidth);
         this.Controls.Add(this.axAstUnitPlateLength);
         this.Controls.Add(this.axAstUnitPlateThickness);
         this.Enabled = false;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Margin = new System.Windows.Forms.Padding(4);
         this.Name = "Page1";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.Text = "Page1";
         ((System.ComponentModel.ISupportInitialize)(this.axAstUnitPlateThickness)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.axAstUnitPlateLength)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.axAstUnitPlateWidth)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.axAstUnitCutBack)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.axAnchor)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.axAstControls1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private AxASTCONTROLSLib.AxAstUnitControl axAstUnitPlateThickness;
      private AxASTCONTROLSLib.AxAstUnitControl axAstUnitPlateLength;
      private AxASTCONTROLSLib.AxAstUnitControl axAstUnitPlateWidth;
      private AxASTCONTROLSLib.AxAstUnitControl axAstUnitCutBack;
      private AxASTCONTROLSLib.AxBoltStandards axAnchor;
      private AxASTCONTROLSLib.AxAstControls axAstControls1;
   }
}