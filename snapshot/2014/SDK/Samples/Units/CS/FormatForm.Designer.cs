namespace Revit.SDK.Samples.Units.CS
{
   partial class FormatForm
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
         this.UseDefaultcheckBox = new System.Windows.Forms.CheckBox();
         this.DisplayUnitTypelabel = new System.Windows.Forms.Label();
         this.DisplayUnitscomboBox = new System.Windows.Forms.ComboBox();
         this.Roundinglabel = new System.Windows.Forms.Label();
         this.AccuracytextBox = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.UnitSymbolcomboBox = new System.Windows.Forms.ComboBox();
         this.SuppressTrailingZeroscheckBox = new System.Windows.Forms.CheckBox();
         this.SuppressLeadingZeroscheckBox = new System.Windows.Forms.CheckBox();
         this.UsePlusPrefixcheckBox = new System.Windows.Forms.CheckBox();
         this.UseGroupingcheckBox = new System.Windows.Forms.CheckBox();
         this.SuppressSpacescheckBox = new System.Windows.Forms.CheckBox();
         this.DisplayUnitTypecomboBox = new System.Windows.Forms.ComboBox();
         this.UnitSymbolTypecomboBox = new System.Windows.Forms.ComboBox();
         this.SuspendLayout();
         // 
         // buttonOK
         // 
         this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         //this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonOK.Location = new System.Drawing.Point(66, 231);
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
         this.buttonCancel.Location = new System.Drawing.Point(147, 231);
         this.buttonCancel.Name = "buttonCancel";
         this.buttonCancel.Size = new System.Drawing.Size(75, 23);
         this.buttonCancel.TabIndex = 1;
         this.buttonCancel.Text = "&Cancel";
         this.buttonCancel.UseVisualStyleBackColor = true;
         // 
         // UseDefaultcheckBox
         // 
         this.UseDefaultcheckBox.AutoSize = true;
         this.UseDefaultcheckBox.Location = new System.Drawing.Point(9, 12);
         this.UseDefaultcheckBox.Name = "UseDefaultcheckBox";
         this.UseDefaultcheckBox.Size = new System.Drawing.Size(129, 17);
         this.UseDefaultcheckBox.TabIndex = 2;
         this.UseDefaultcheckBox.Text = "Use default formatting";
         this.UseDefaultcheckBox.UseVisualStyleBackColor = true;
         // 
         // DisplayUnitTypelabel
         // 
         this.DisplayUnitTypelabel.AutoSize = true;
         this.DisplayUnitTypelabel.Location = new System.Drawing.Point(6, 37);
         this.DisplayUnitTypelabel.Name = "DisplayUnitTypelabel";
         this.DisplayUnitTypelabel.Size = new System.Drawing.Size(87, 13);
         this.DisplayUnitTypelabel.TabIndex = 3;
         this.DisplayUnitTypelabel.Text = "DisplayUnitType:";
         // 
         // DisplayUnitscomboBox
         // 
         this.DisplayUnitscomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.DisplayUnitscomboBox.FormattingEnabled = true;
         this.DisplayUnitscomboBox.Location = new System.Drawing.Point(101, 34);
         this.DisplayUnitscomboBox.Name = "DisplayUnitscomboBox";
         this.DisplayUnitscomboBox.Size = new System.Drawing.Size(121, 21);
         this.DisplayUnitscomboBox.TabIndex = 4;
         // 
         // Roundinglabel
         // 
         this.Roundinglabel.AutoSize = true;
         this.Roundinglabel.Location = new System.Drawing.Point(6, 64);
         this.Roundinglabel.Name = "Roundinglabel";
         this.Roundinglabel.Size = new System.Drawing.Size(55, 13);
         this.Roundinglabel.TabIndex = 5;
         this.Roundinglabel.Text = "Accuracy:";
         // 
         // AccuracytextBox
         // 
         this.AccuracytextBox.Location = new System.Drawing.Point(101, 61);
         this.AccuracytextBox.Name = "AccuracytextBox";
         this.AccuracytextBox.Size = new System.Drawing.Size(121, 20);
         this.AccuracytextBox.TabIndex = 6;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(6, 90);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(63, 13);
         this.label1.TabIndex = 7;
         this.label1.Text = "UnitSymbol:";
         // 
         // UnitSymbolcomboBox
         // 
         this.UnitSymbolcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.UnitSymbolcomboBox.FormattingEnabled = true;
         this.UnitSymbolcomboBox.Location = new System.Drawing.Point(101, 87);
         this.UnitSymbolcomboBox.Name = "UnitSymbolcomboBox";
         this.UnitSymbolcomboBox.Size = new System.Drawing.Size(121, 21);
         this.UnitSymbolcomboBox.TabIndex = 8;
         // 
         // SuppressTrailingZeroscheckBox
         // 
         this.SuppressTrailingZeroscheckBox.AutoSize = true;
         this.SuppressTrailingZeroscheckBox.Location = new System.Drawing.Point(9, 114);
         this.SuppressTrailingZeroscheckBox.Name = "SuppressTrailingZeroscheckBox";
         this.SuppressTrailingZeroscheckBox.Size = new System.Drawing.Size(131, 17);
         this.SuppressTrailingZeroscheckBox.TabIndex = 9;
         this.SuppressTrailingZeroscheckBox.Text = "Suppress trailing zeros";
         this.SuppressTrailingZeroscheckBox.UseVisualStyleBackColor = true;
         // 
         // SuppressLeadingZeroscheckBox
         // 
         this.SuppressLeadingZeroscheckBox.AutoSize = true;
         this.SuppressLeadingZeroscheckBox.Location = new System.Drawing.Point(9, 137);
         this.SuppressLeadingZeroscheckBox.Name = "SuppressLeadingZeroscheckBox";
         this.SuppressLeadingZeroscheckBox.Size = new System.Drawing.Size(135, 17);
         this.SuppressLeadingZeroscheckBox.TabIndex = 10;
         this.SuppressLeadingZeroscheckBox.Text = "Suppress leading zeros";
         this.SuppressLeadingZeroscheckBox.UseVisualStyleBackColor = true;
         // 
         // UsePlusPrefixcheckBox
         // 
         this.UsePlusPrefixcheckBox.AutoSize = true;
         this.UsePlusPrefixcheckBox.Location = new System.Drawing.Point(9, 160);
         this.UsePlusPrefixcheckBox.Name = "UsePlusPrefixcheckBox";
         this.UsePlusPrefixcheckBox.Size = new System.Drawing.Size(95, 17);
         this.UsePlusPrefixcheckBox.TabIndex = 11;
         this.UsePlusPrefixcheckBox.Text = "Use plus prefix";
         this.UsePlusPrefixcheckBox.UseVisualStyleBackColor = true;
         // 
         // UseGroupingcheckBox
         // 
         this.UseGroupingcheckBox.AutoSize = true;
         this.UseGroupingcheckBox.Location = new System.Drawing.Point(9, 183);
         this.UseGroupingcheckBox.Name = "UseGroupingcheckBox";
         this.UseGroupingcheckBox.Size = new System.Drawing.Size(111, 17);
         this.UseGroupingcheckBox.TabIndex = 12;
         this.UseGroupingcheckBox.Text = "Use digit grouping";
         this.UseGroupingcheckBox.UseVisualStyleBackColor = true;
         // 
         // SuppressSpacescheckBox
         // 
         this.SuppressSpacescheckBox.AutoSize = true;
         this.SuppressSpacescheckBox.Location = new System.Drawing.Point(9, 206);
         this.SuppressSpacescheckBox.Name = "SuppressSpacescheckBox";
         this.SuppressSpacescheckBox.Size = new System.Drawing.Size(107, 17);
         this.SuppressSpacescheckBox.TabIndex = 13;
         this.SuppressSpacescheckBox.Text = "Suppress spaces";
         this.SuppressSpacescheckBox.UseVisualStyleBackColor = true;
         // 
         // DisplayUnitTypecomboBox
         // 
         this.DisplayUnitTypecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.DisplayUnitTypecomboBox.FormattingEnabled = true;
         this.DisplayUnitTypecomboBox.Location = new System.Drawing.Point(101, 34);
         this.DisplayUnitTypecomboBox.Name = "DisplayUnitTypecomboBox";
         this.DisplayUnitTypecomboBox.Size = new System.Drawing.Size(121, 21);
         this.DisplayUnitTypecomboBox.TabIndex = 14;
         this.DisplayUnitTypecomboBox.Visible = false;
         // 
         // UnitSymbolTypecomboBox
         // 
         this.UnitSymbolTypecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.UnitSymbolTypecomboBox.FormattingEnabled = true;
         this.UnitSymbolTypecomboBox.Location = new System.Drawing.Point(101, 87);
         this.UnitSymbolTypecomboBox.Name = "UnitSymbolTypecomboBox";
         this.UnitSymbolTypecomboBox.Size = new System.Drawing.Size(121, 21);
         this.UnitSymbolTypecomboBox.TabIndex = 15;
         this.UnitSymbolTypecomboBox.Visible = false;
         // 
         // FormatForm
         // 
         this.AcceptButton = this.buttonOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.buttonCancel;
         this.ClientSize = new System.Drawing.Size(234, 266);
         this.Controls.Add(this.UnitSymbolTypecomboBox);
         this.Controls.Add(this.DisplayUnitTypecomboBox);
         this.Controls.Add(this.SuppressSpacescheckBox);
         this.Controls.Add(this.UseGroupingcheckBox);
         this.Controls.Add(this.UsePlusPrefixcheckBox);
         this.Controls.Add(this.SuppressLeadingZeroscheckBox);
         this.Controls.Add(this.SuppressTrailingZeroscheckBox);
         this.Controls.Add(this.UnitSymbolcomboBox);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.AccuracytextBox);
         this.Controls.Add(this.Roundinglabel);
         this.Controls.Add(this.DisplayUnitscomboBox);
         this.Controls.Add(this.DisplayUnitTypelabel);
         this.Controls.Add(this.UseDefaultcheckBox);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.buttonOK);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FormatForm";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Format";
         this.Load += new System.EventHandler(this.FormatForm_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button buttonOK;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.CheckBox UseDefaultcheckBox;
      private System.Windows.Forms.Label DisplayUnitTypelabel;
      private System.Windows.Forms.ComboBox DisplayUnitscomboBox;
      private System.Windows.Forms.Label Roundinglabel;
      private System.Windows.Forms.TextBox AccuracytextBox;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox UnitSymbolcomboBox;
      private System.Windows.Forms.CheckBox SuppressTrailingZeroscheckBox;
      private System.Windows.Forms.CheckBox SuppressLeadingZeroscheckBox;
      private System.Windows.Forms.CheckBox UsePlusPrefixcheckBox;
      private System.Windows.Forms.CheckBox UseGroupingcheckBox;
      private System.Windows.Forms.CheckBox SuppressSpacescheckBox;
      private System.Windows.Forms.ComboBox DisplayUnitTypecomboBox;
      private System.Windows.Forms.ComboBox UnitSymbolTypecomboBox;
   }
}