namespace Revit.SDK.Samples.ViewTemplateCreation.CS
{
   /// <summary>
   /// A form for the sample, which allows user to create and configure a new view template.
   /// </summary>
   public partial class ViewTemplateCreationForm
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
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.viewNameLabel = new System.Windows.Forms.Label();
         this.partsVisibilityLabel = new System.Windows.Forms.Label();
         this.detailLevelLabel = new System.Windows.Forms.Label();
         this.viewNameComboBox = new System.Windows.Forms.ComboBox();
         this.partsVisibilityStateComboBox = new System.Windows.Forms.ComboBox();
         this.detailLevelValueComboBox = new System.Windows.Forms.ComboBox();
         this.applyButton = new System.Windows.Forms.Button();
         this.cancelButton = new System.Windows.Forms.Button();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tableLayoutPanel1.AutoSize = true;
         this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Controls.Add(this.viewNameLabel, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.partsVisibilityLabel, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.detailLevelLabel, 0, 2);
         this.tableLayoutPanel1.Controls.Add(this.viewNameComboBox, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.partsVisibilityStateComboBox, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.detailLevelValueComboBox, 1, 2);
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
         this.tableLayoutPanel1.RowCount = 3;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(336, 87);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // viewNameLabel
         // 
         this.viewNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
         this.viewNameLabel.AutoSize = true;
         this.viewNameLabel.Location = new System.Drawing.Point(6, 10);
         this.viewNameLabel.Name = "viewNameLabel";
         this.viewNameLabel.Size = new System.Drawing.Size(141, 13);
         this.viewNameLabel.TabIndex = 0;
         this.viewNameLabel.Text = "A view for template creation:";
         this.viewNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // partsVisibilityLabel
         // 
         this.partsVisibilityLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
         this.partsVisibilityLabel.AutoSize = true;
         this.partsVisibilityLabel.Location = new System.Drawing.Point(6, 37);
         this.partsVisibilityLabel.Name = "partsVisibilityLabel";
         this.partsVisibilityLabel.Size = new System.Drawing.Size(149, 13);
         this.partsVisibilityLabel.TabIndex = 1;
         this.partsVisibilityLabel.Text = "Parts Visibility parameter state:";
         // 
         // detailLevelLabel
         // 
         this.detailLevelLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
         this.detailLevelLabel.AutoSize = true;
         this.detailLevelLabel.Location = new System.Drawing.Point(6, 64);
         this.detailLevelLabel.Name = "detailLevelLabel";
         this.detailLevelLabel.Size = new System.Drawing.Size(141, 13);
         this.detailLevelLabel.TabIndex = 2;
         this.detailLevelLabel.Text = "Detail level parameter value:";
         // 
         // viewNameComboBox
         // 
         this.viewNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.viewNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.viewNameComboBox.FormattingEnabled = true;
         this.viewNameComboBox.Location = new System.Drawing.Point(161, 6);
         this.viewNameComboBox.Name = "viewNameComboBox";
         this.viewNameComboBox.Size = new System.Drawing.Size(169, 21);
         this.viewNameComboBox.TabIndex = 3;
         // 
         // partsVisibilityStateComboBox
         // 
         this.partsVisibilityStateComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.partsVisibilityStateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.partsVisibilityStateComboBox.FormattingEnabled = true;
         this.partsVisibilityStateComboBox.Location = new System.Drawing.Point(161, 33);
         this.partsVisibilityStateComboBox.Name = "partsVisibilityStateComboBox";
         this.partsVisibilityStateComboBox.Size = new System.Drawing.Size(169, 21);
         this.partsVisibilityStateComboBox.TabIndex = 4;
         // 
         // detailLevelValueComboBox
         // 
         this.detailLevelValueComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.detailLevelValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.detailLevelValueComboBox.FormattingEnabled = true;
         this.detailLevelValueComboBox.Location = new System.Drawing.Point(161, 60);
         this.detailLevelValueComboBox.Name = "detailLevelValueComboBox";
         this.detailLevelValueComboBox.Size = new System.Drawing.Size(169, 21);
         this.detailLevelValueComboBox.TabIndex = 5;
         // 
         // applyButton
         // 
         this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.applyButton.Location = new System.Drawing.Point(181, 100);
         this.applyButton.Name = "applyButton";
         this.applyButton.Size = new System.Drawing.Size(71, 23);
         this.applyButton.TabIndex = 6;
         this.applyButton.Text = "Apply";
         this.applyButton.UseVisualStyleBackColor = true;
         this.applyButton.Click += new System.EventHandler(this.ApplyButton_Click);
         // 
         // cancelButton
         // 
         this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.cancelButton.Location = new System.Drawing.Point(258, 100);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(71, 23);
         this.cancelButton.TabIndex = 7;
         this.cancelButton.Text = "Cancel";
         this.cancelButton.UseVisualStyleBackColor = true;
         this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
         // 
         // ViewTemplateCreationForm
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
         this.ClientSize = new System.Drawing.Size(351, 135);
         this.Controls.Add(this.tableLayoutPanel1);
         this.Controls.Add(this.applyButton);
         this.Controls.Add(this.cancelButton);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "ViewTemplateCreationForm";
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "ViewTemplateCreationForm";
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Button applyButton;
      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.Label viewNameLabel;
      private System.Windows.Forms.Label partsVisibilityLabel;
      private System.Windows.Forms.Label detailLevelLabel;
      private System.Windows.Forms.ComboBox viewNameComboBox;
      private System.Windows.Forms.ComboBox partsVisibilityStateComboBox;
      private System.Windows.Forms.ComboBox detailLevelValueComboBox;
   }
}