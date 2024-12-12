namespace Revit.SDK.Samples.Units.CS
{
   partial class ProjectUnitForm
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
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
         this.buttonOK = new System.Windows.Forms.Button();
         this.buttonCancel = new System.Windows.Forms.Button();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.dataGridView = new System.Windows.Forms.DataGridView();
         this.UnitType = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.Label_UnitType = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.FormatOptions = new System.Windows.Forms.DataGridViewButtonColumn();
         this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.String = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.FromInternal = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ToInternal = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.groupBox = new System.Windows.Forms.GroupBox();
         this.DecimalSymbolAndGroupingtextBox = new System.Windows.Forms.TextBox();
         this.DecimalSymbolAndGroupinglabel = new System.Windows.Forms.Label();
         this.DigitGroupingSymbolComboBox = new System.Windows.Forms.ComboBox();
         this.DigitGroupingAmountComboBox = new System.Windows.Forms.ComboBox();
         this.DigitGroupingSymbol = new System.Windows.Forms.Label();
         this.DigitGroupingAmount = new System.Windows.Forms.Label();
         this.DecimalSymbolComboBox = new System.Windows.Forms.ComboBox();
         this.DecimalSymbol = new System.Windows.Forms.Label();
         this.disciplineCombox = new System.Windows.Forms.ComboBox();
         this.discipline = new System.Windows.Forms.Label();
         this.groupBox2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
         this.groupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // buttonOK
         // 
         this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonOK.Location = new System.Drawing.Point(343, 414);
         this.buttonOK.Name = "buttonOK";
         this.buttonOK.Size = new System.Drawing.Size(75, 23);
         this.buttonOK.TabIndex = 0;
         this.buttonOK.Text = "&OK";
         this.buttonOK.UseVisualStyleBackColor = true;
         // 
         // buttonCancel
         // 
         this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.buttonCancel.Location = new System.Drawing.Point(424, 414);
         this.buttonCancel.Name = "buttonCancel";
         this.buttonCancel.Size = new System.Drawing.Size(75, 23);
         this.buttonCancel.TabIndex = 1;
         this.buttonCancel.Text = "&Cancel";
         this.buttonCancel.UseVisualStyleBackColor = true;
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.dataGridView);
         this.groupBox2.Location = new System.Drawing.Point(28, 39);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(471, 251);
         this.groupBox2.TabIndex = 12;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "GroupBox";
         // 
         // dataGridView
         // 
         this.dataGridView.AllowUserToAddRows = false;
         this.dataGridView.AllowUserToDeleteRows = false;
         this.dataGridView.AllowUserToResizeRows = false;
         dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
         dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
         dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
         dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
         this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
         this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UnitType,
            this.Label_UnitType,
            this.FormatOptions,
            this.Value,
            this.String,
            this.FromInternal,
            this.ToInternal});
         dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
         dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
         dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
         dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
         this.dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
         this.dataGridView.Location = new System.Drawing.Point(0, 19);
         this.dataGridView.Name = "dataGridView";
         this.dataGridView.ReadOnly = true;
         dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
         dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
         dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
         dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
         this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
         this.dataGridView.RowHeadersVisible = false;
         this.dataGridView.Size = new System.Drawing.Size(471, 226);
         this.dataGridView.TabIndex = 13;
         this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);
         // 
         // UnitType
         // 
         this.UnitType.HeaderText = "UnitType";
         this.UnitType.Name = "UnitType";
         this.UnitType.ReadOnly = true;
         this.UnitType.Visible = false;
         // 
         // Label_UnitType
         // 
         this.Label_UnitType.HeaderText = "Units";
         this.Label_UnitType.Name = "Label_UnitType";
         this.Label_UnitType.ReadOnly = true;
         this.Label_UnitType.Width = 150;
         // 
         // FormatOptions
         // 
         this.FormatOptions.HeaderText = "Format";
         this.FormatOptions.Name = "FormatOptions";
         this.FormatOptions.ReadOnly = true;
         this.FormatOptions.Resizable = System.Windows.Forms.DataGridViewTriState.True;
         this.FormatOptions.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
         this.FormatOptions.Width = 300;
         // 
         // Value
         // 
         this.Value.HeaderText = "Value";
         this.Value.Name = "Value";
         this.Value.ReadOnly = true;
         this.Value.Visible = false;
         // 
         // String
         // 
         this.String.HeaderText = "String";
         this.String.Name = "String";
         this.String.ReadOnly = true;
         this.String.Visible = false;
         // 
         // FromInternal
         // 
         this.FromInternal.HeaderText = "From Internal";
         this.FromInternal.Name = "FromInternal";
         this.FromInternal.ReadOnly = true;
         this.FromInternal.Visible = false;
         // 
         // ToInternal
         // 
         this.ToInternal.HeaderText = "To Internal";
         this.ToInternal.Name = "ToInternal";
         this.ToInternal.ReadOnly = true;
         this.ToInternal.Visible = false;
         // 
         // groupBox
         // 
         this.groupBox.Controls.Add(this.DecimalSymbolAndGroupingtextBox);
         this.groupBox.Controls.Add(this.DecimalSymbolAndGroupinglabel);
         this.groupBox.Controls.Add(this.DigitGroupingSymbolComboBox);
         this.groupBox.Controls.Add(this.DigitGroupingAmountComboBox);
         this.groupBox.Controls.Add(this.DigitGroupingSymbol);
         this.groupBox.Controls.Add(this.DigitGroupingAmount);
         this.groupBox.Controls.Add(this.DecimalSymbolComboBox);
         this.groupBox.Controls.Add(this.DecimalSymbol);
         this.groupBox.Location = new System.Drawing.Point(28, 296);
         this.groupBox.Name = "groupBox";
         this.groupBox.Size = new System.Drawing.Size(471, 105);
         this.groupBox.TabIndex = 11;
         this.groupBox.TabStop = false;
         this.groupBox.Text = "Options";
         // 
         // DecimalSymbolAndGroupingtextBox
         // 
         this.DecimalSymbolAndGroupingtextBox.Location = new System.Drawing.Point(9, 71);
         this.DecimalSymbolAndGroupingtextBox.Name = "DecimalSymbolAndGroupingtextBox";
         this.DecimalSymbolAndGroupingtextBox.ReadOnly = true;
         this.DecimalSymbolAndGroupingtextBox.Size = new System.Drawing.Size(434, 20);
         this.DecimalSymbolAndGroupingtextBox.TabIndex = 12;
         // 
         // DecimalSymbolAndGroupinglabel
         // 
         this.DecimalSymbolAndGroupinglabel.AutoSize = true;
         this.DecimalSymbolAndGroupinglabel.Location = new System.Drawing.Point(6, 55);
         this.DecimalSymbolAndGroupinglabel.Name = "DecimalSymbolAndGroupinglabel";
         this.DecimalSymbolAndGroupinglabel.Size = new System.Drawing.Size(151, 13);
         this.DecimalSymbolAndGroupinglabel.TabIndex = 11;
         this.DecimalSymbolAndGroupinglabel.Text = "Decimal symbol/digit grouping:";
         // 
         // DigitGroupingSymbolComboBox
         // 
         this.DigitGroupingSymbolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.DigitGroupingSymbolComboBox.FormattingEnabled = true;
         this.DigitGroupingSymbolComboBox.Location = new System.Drawing.Point(322, 32);
         this.DigitGroupingSymbolComboBox.Name = "DigitGroupingSymbolComboBox";
         this.DigitGroupingSymbolComboBox.Size = new System.Drawing.Size(121, 21);
         this.DigitGroupingSymbolComboBox.TabIndex = 7;
         // 
         // DigitGroupingAmountComboBox
         // 
         this.DigitGroupingAmountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.DigitGroupingAmountComboBox.FormattingEnabled = true;
         this.DigitGroupingAmountComboBox.Location = new System.Drawing.Point(162, 32);
         this.DigitGroupingAmountComboBox.Name = "DigitGroupingAmountComboBox";
         this.DigitGroupingAmountComboBox.Size = new System.Drawing.Size(121, 21);
         this.DigitGroupingAmountComboBox.TabIndex = 6;
         // 
         // DigitGroupingSymbol
         // 
         this.DigitGroupingSymbol.AutoSize = true;
         this.DigitGroupingSymbol.Location = new System.Drawing.Point(319, 16);
         this.DigitGroupingSymbol.Name = "DigitGroupingSymbol";
         this.DigitGroupingSymbol.Size = new System.Drawing.Size(110, 13);
         this.DigitGroupingSymbol.TabIndex = 5;
         this.DigitGroupingSymbol.Text = "Digit grouping symbol:";
         // 
         // DigitGroupingAmount
         // 
         this.DigitGroupingAmount.AutoSize = true;
         this.DigitGroupingAmount.Location = new System.Drawing.Point(159, 16);
         this.DigitGroupingAmount.Name = "DigitGroupingAmount";
         this.DigitGroupingAmount.Size = new System.Drawing.Size(113, 13);
         this.DigitGroupingAmount.TabIndex = 4;
         this.DigitGroupingAmount.Text = "Digit grouping amount:";
         // 
         // DecimalSymbolComboBox
         // 
         this.DecimalSymbolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.DecimalSymbolComboBox.FormattingEnabled = true;
         this.DecimalSymbolComboBox.Location = new System.Drawing.Point(9, 32);
         this.DecimalSymbolComboBox.Name = "DecimalSymbolComboBox";
         this.DecimalSymbolComboBox.Size = new System.Drawing.Size(121, 21);
         this.DecimalSymbolComboBox.TabIndex = 3;
         // 
         // DecimalSymbol
         // 
         this.DecimalSymbol.AutoSize = true;
         this.DecimalSymbol.Location = new System.Drawing.Point(6, 16);
         this.DecimalSymbol.Name = "DecimalSymbol";
         this.DecimalSymbol.Size = new System.Drawing.Size(83, 13);
         this.DecimalSymbol.TabIndex = 1;
         this.DecimalSymbol.Text = "Decimal symbol:";
         // 
         // disciplineCombox
         // 
         this.disciplineCombox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.disciplineCombox.FormattingEnabled = true;
         this.disciplineCombox.Location = new System.Drawing.Point(86, 12);
         this.disciplineCombox.Name = "disciplineCombox";
         this.disciplineCombox.Size = new System.Drawing.Size(413, 21);
         this.disciplineCombox.TabIndex = 10;
         this.disciplineCombox.SelectedIndexChanged += new System.EventHandler(this.disciplineCombox_SelectedIndexChanged);
         // 
         // discipline
         // 
         this.discipline.AutoSize = true;
         this.discipline.Location = new System.Drawing.Point(25, 15);
         this.discipline.Name = "discipline";
         this.discipline.Size = new System.Drawing.Size(55, 13);
         this.discipline.TabIndex = 9;
         this.discipline.Text = "Discipline:";
         // 
         // ProjectUnitForm
         // 
         this.AcceptButton = this.buttonOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.buttonCancel;
         this.ClientSize = new System.Drawing.Size(532, 456);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.groupBox);
         this.Controls.Add(this.disciplineCombox);
         this.Controls.Add(this.discipline);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.buttonOK);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "ProjectUnitForm";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Project Unit";
         this.Load += new System.EventHandler(this.ProjectUnitForm_Load);
         this.groupBox2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
         this.groupBox.ResumeLayout(false);
         this.groupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button buttonOK;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.GroupBox groupBox;
      private System.Windows.Forms.ComboBox DigitGroupingSymbolComboBox;
      private System.Windows.Forms.ComboBox DigitGroupingAmountComboBox;
      private System.Windows.Forms.Label DigitGroupingSymbol;
      private System.Windows.Forms.Label DigitGroupingAmount;
      private System.Windows.Forms.ComboBox DecimalSymbolComboBox;
      private System.Windows.Forms.Label DecimalSymbol;
      private System.Windows.Forms.ComboBox disciplineCombox;
      private System.Windows.Forms.Label discipline;
      private System.Windows.Forms.DataGridView dataGridView;
      private System.Windows.Forms.Label DecimalSymbolAndGroupinglabel;
      private System.Windows.Forms.TextBox DecimalSymbolAndGroupingtextBox;
      private System.Windows.Forms.DataGridViewTextBoxColumn UnitType;
      private System.Windows.Forms.DataGridViewTextBoxColumn Label_UnitType;
      private System.Windows.Forms.DataGridViewButtonColumn FormatOptions;
      private System.Windows.Forms.DataGridViewTextBoxColumn Value;
      private System.Windows.Forms.DataGridViewTextBoxColumn String;
      private System.Windows.Forms.DataGridViewTextBoxColumn FromInternal;
      private System.Windows.Forms.DataGridViewTextBoxColumn ToInternal;
   }
}