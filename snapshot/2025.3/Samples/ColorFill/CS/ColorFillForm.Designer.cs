namespace Revit.SDK.Samples.ColorFill.CS
{
   partial class ColorFillForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;
      private ColorFillMgr m_colorFillMgr;

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
            this.gbScheme = new System.Windows.Forms.GroupBox();
            this.lbSchemeResults = new System.Windows.Forms.Label();
            this.tbSchemeTitle = new System.Windows.Forms.TextBox();
            this.lbSchemeTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSchemeName = new System.Windows.Forms.TextBox();
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.lbScheme = new System.Windows.Forms.Label();
            this.lstSchemes = new System.Windows.Forms.ListBox();
            this.btnPlaceLegend = new System.Windows.Forms.Button();
            this.gblegend = new System.Windows.Forms.GroupBox();
            this.lbLegendResults = new System.Windows.Forms.Label();
            this.lstViews = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUpdateColor = new System.Windows.Forms.Button();
            this.gbScheme.SuspendLayout();
            this.gblegend.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbScheme
            // 
            this.gbScheme.Controls.Add(this.btnUpdateColor);
            this.gbScheme.Controls.Add(this.lbSchemeResults);
            this.gbScheme.Controls.Add(this.tbSchemeTitle);
            this.gbScheme.Controls.Add(this.lbSchemeTitle);
            this.gbScheme.Controls.Add(this.label1);
            this.gbScheme.Controls.Add(this.tbSchemeName);
            this.gbScheme.Controls.Add(this.btnDuplicate);
            this.gbScheme.Controls.Add(this.lbScheme);
            this.gbScheme.Controls.Add(this.lstSchemes);
            this.gbScheme.Location = new System.Drawing.Point(12, 12);
            this.gbScheme.Name = "gbScheme";
            this.gbScheme.Size = new System.Drawing.Size(417, 505);
            this.gbScheme.TabIndex = 0;
            this.gbScheme.TabStop = false;
            this.gbScheme.Text = "Schemes";
            // 
            // lbSchemeResults
            // 
            this.lbSchemeResults.AutoSize = true;
            this.lbSchemeResults.Location = new System.Drawing.Point(16, 236);
            this.lbSchemeResults.Name = "lbSchemeResults";
            this.lbSchemeResults.Size = new System.Drawing.Size(0, 20);
            this.lbSchemeResults.TabIndex = 7;
            // 
            // tbSchemeTitle
            // 
            this.tbSchemeTitle.Location = new System.Drawing.Point(135, 196);
            this.tbSchemeTitle.Name = "tbSchemeTitle";
            this.tbSchemeTitle.Size = new System.Drawing.Size(174, 26);
            this.tbSchemeTitle.TabIndex = 6;
            // 
            // lbSchemeTitle
            // 
            this.lbSchemeTitle.AutoSize = true;
            this.lbSchemeTitle.Location = new System.Drawing.Point(6, 196);
            this.lbSchemeTitle.Name = "lbSchemeTitle";
            this.lbSchemeTitle.Size = new System.Drawing.Size(105, 20);
            this.lbSchemeTitle.TabIndex = 5;
            this.lbSchemeTitle.Text = "Scheme Title:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Scheme Name:";
            // 
            // tbSchemeName
            // 
            this.tbSchemeName.Location = new System.Drawing.Point(135, 145);
            this.tbSchemeName.Name = "tbSchemeName";
            this.tbSchemeName.Size = new System.Drawing.Size(174, 26);
            this.tbSchemeName.TabIndex = 3;
            // 
            // btnDuplicate
            // 
            this.btnDuplicate.Location = new System.Drawing.Point(6, 281);
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Size = new System.Drawing.Size(191, 33);
            this.btnDuplicate.TabIndex = 2;
            this.btnDuplicate.Text = "Duplicate Scheme";
            this.btnDuplicate.UseVisualStyleBackColor = true;
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            // 
            // lbScheme
            // 
            this.lbScheme.AutoSize = true;
            this.lbScheme.Location = new System.Drawing.Point(6, 25);
            this.lbScheme.Name = "lbScheme";
            this.lbScheme.Size = new System.Drawing.Size(123, 20);
            this.lbScheme.TabIndex = 1;
            this.lbScheme.Text = "RoomSchemes:";
            // 
            // lstSchemes
            // 
            this.lstSchemes.FormattingEnabled = true;
            this.lstSchemes.ItemHeight = 20;
            this.lstSchemes.Location = new System.Drawing.Point(135, 25);
            this.lstSchemes.Name = "lstSchemes";
            this.lstSchemes.Size = new System.Drawing.Size(174, 104);
            this.lstSchemes.TabIndex = 0;
            // 
            // btnPlaceLegend
            // 
            this.btnPlaceLegend.Location = new System.Drawing.Point(126, 281);
            this.btnPlaceLegend.Name = "btnPlaceLegend";
            this.btnPlaceLegend.Size = new System.Drawing.Size(191, 37);
            this.btnPlaceLegend.TabIndex = 8;
            this.btnPlaceLegend.Text = "Place Legend";
            this.btnPlaceLegend.UseVisualStyleBackColor = true;
            this.btnPlaceLegend.Click += new System.EventHandler(this.btnPlaceLegend_Click);
            // 
            // gblegend
            // 
            this.gblegend.Controls.Add(this.lbLegendResults);
            this.gblegend.Controls.Add(this.lstViews);
            this.gblegend.Controls.Add(this.label2);
            this.gblegend.Controls.Add(this.btnPlaceLegend);
            this.gblegend.Location = new System.Drawing.Point(435, 12);
            this.gblegend.Name = "gblegend";
            this.gblegend.Size = new System.Drawing.Size(422, 505);
            this.gblegend.TabIndex = 9;
            this.gblegend.TabStop = false;
            this.gblegend.Text = "Legend";
            // 
            // lbLegendResults
            // 
            this.lbLegendResults.AutoSize = true;
            this.lbLegendResults.Location = new System.Drawing.Point(19, 176);
            this.lbLegendResults.Name = "lbLegendResults";
            this.lbLegendResults.Size = new System.Drawing.Size(0, 20);
            this.lbLegendResults.TabIndex = 12;
            // 
            // lstViews
            // 
            this.lstViews.FormattingEnabled = true;
            this.lstViews.ItemHeight = 20;
            this.lstViews.Location = new System.Drawing.Point(164, 25);
            this.lstViews.Name = "lstViews";
            this.lstViews.Size = new System.Drawing.Size(182, 104);
            this.lstViews.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Views to Place:";
            // 
            // btnUpdateColor
            // 
            this.btnUpdateColor.Location = new System.Drawing.Point(6, 349);
            this.btnUpdateColor.Name = "btnUpdateColor";
            this.btnUpdateColor.Size = new System.Drawing.Size(191, 34);
            this.btnUpdateColor.TabIndex = 8;
            this.btnUpdateColor.Text = "Update Entries Color";
            this.btnUpdateColor.UseVisualStyleBackColor = true;
            this.btnUpdateColor.Click += new System.EventHandler(this.btnUpdateColor_Click);
            // 
            // ColorFillForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 563);
            this.Controls.Add(this.gblegend);
            this.Controls.Add(this.gbScheme);
            this.Name = "ColorFillForm";
            this.Text = "ColorFillForm";
            this.gbScheme.ResumeLayout(false);
            this.gbScheme.PerformLayout();
            this.gblegend.ResumeLayout(false);
            this.gblegend.PerformLayout();
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.GroupBox gbScheme;
      private System.Windows.Forms.Label lbScheme;
      private System.Windows.Forms.Button btnDuplicate;
      private System.Windows.Forms.ListBox lstSchemes;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox tbSchemeName;
      private System.Windows.Forms.Label lbSchemeTitle;
      private System.Windows.Forms.TextBox tbSchemeTitle;
      private System.Windows.Forms.Label lbSchemeResults;
      private System.Windows.Forms.Button btnPlaceLegend;
      private System.Windows.Forms.GroupBox gblegend;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.ListBox lstViews;
      private System.Windows.Forms.Label lbLegendResults;
      private System.Windows.Forms.Button btnUpdateColor;
   }
}