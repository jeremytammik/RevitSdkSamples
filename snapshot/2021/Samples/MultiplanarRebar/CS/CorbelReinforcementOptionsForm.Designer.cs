namespace Revit.SDK.Samples.MultiplanarRebar.CS
{
    partial class CorbelReinforcementOptionsForm
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.topBarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.stirrupBarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.topBarTypeLabel = new System.Windows.Forms.Label();
            this.stirrupBarTypeLabel = new System.Windows.Forms.Label();
            this.multiplanarBarTypeLabel = new System.Windows.Forms.Label();
            this.multiplanarBarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.topBarCountTextBox = new System.Windows.Forms.TextBox();
            this.stirrupBarCountTextBox = new System.Windows.Forms.TextBox();
            this.topBarGroupBox = new System.Windows.Forms.GroupBox();
            this.topBarCountLabel = new System.Windows.Forms.Label();
            this.stirrupBarGroupBox = new System.Windows.Forms.GroupBox();
            this.stirrupBarCountLabel = new System.Windows.Forms.Label();
            this.multiplanarBarGroupBox = new System.Windows.Forms.GroupBox();
            this.columnGroupBox = new System.Windows.Forms.GroupBox();
            this.columnBarTypeComboBox = new System.Windows.Forms.ComboBox();
            this.columnBarTypeLabel = new System.Windows.Forms.Label();
            this.topBarGroupBox.SuspendLayout();
            this.stirrupBarGroupBox.SuspendLayout();
            this.multiplanarBarGroupBox.SuspendLayout();
            this.columnGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(144, 332);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(269, 332);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // topBarTypeComboBox
            // 
            this.topBarTypeComboBox.FormattingEnabled = true;
            this.topBarTypeComboBox.Location = new System.Drawing.Point(86, 22);
            this.topBarTypeComboBox.Name = "topBarTypeComboBox";
            this.topBarTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.topBarTypeComboBox.TabIndex = 2;
            this.topBarTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.topBarTypeComboBox_SelectedIndexChanged);
            // 
            // stirrupBarTypeComboBox
            // 
            this.stirrupBarTypeComboBox.FormattingEnabled = true;
            this.stirrupBarTypeComboBox.Location = new System.Drawing.Point(86, 26);
            this.stirrupBarTypeComboBox.Name = "stirrupBarTypeComboBox";
            this.stirrupBarTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.stirrupBarTypeComboBox.TabIndex = 3;
            this.stirrupBarTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.stirrupBarTypeComboBox_SelectedIndexChanged);
            // 
            // topBarTypeLabel
            // 
            this.topBarTypeLabel.AutoSize = true;
            this.topBarTypeLabel.Location = new System.Drawing.Point(7, 22);
            this.topBarTypeLabel.Name = "topBarTypeLabel";
            this.topBarTypeLabel.Size = new System.Drawing.Size(53, 13);
            this.topBarTypeLabel.TabIndex = 4;
            this.topBarTypeLabel.Text = "Bar Type:";
            // 
            // stirrupBarTypeLabel
            // 
            this.stirrupBarTypeLabel.AutoSize = true;
            this.stirrupBarTypeLabel.Location = new System.Drawing.Point(7, 26);
            this.stirrupBarTypeLabel.Name = "stirrupBarTypeLabel";
            this.stirrupBarTypeLabel.Size = new System.Drawing.Size(53, 13);
            this.stirrupBarTypeLabel.TabIndex = 5;
            this.stirrupBarTypeLabel.Text = "Bar Type:";
            // 
            // multiplanarBarTypeLabel
            // 
            this.multiplanarBarTypeLabel.AutoSize = true;
            this.multiplanarBarTypeLabel.Location = new System.Drawing.Point(7, 25);
            this.multiplanarBarTypeLabel.Name = "multiplanarBarTypeLabel";
            this.multiplanarBarTypeLabel.Size = new System.Drawing.Size(53, 13);
            this.multiplanarBarTypeLabel.TabIndex = 6;
            this.multiplanarBarTypeLabel.Text = "Bar Type:";
            // 
            // multiplanarBarTypeComboBox
            // 
            this.multiplanarBarTypeComboBox.FormattingEnabled = true;
            this.multiplanarBarTypeComboBox.Location = new System.Drawing.Point(86, 22);
            this.multiplanarBarTypeComboBox.Name = "multiplanarBarTypeComboBox";
            this.multiplanarBarTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.multiplanarBarTypeComboBox.TabIndex = 7;
            this.multiplanarBarTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.multiplanarBarTypeComboBox_SelectedIndexChanged);
            // 
            // topBarCountTextBox
            // 
            this.topBarCountTextBox.Location = new System.Drawing.Point(334, 22);
            this.topBarCountTextBox.Name = "topBarCountTextBox";
            this.topBarCountTextBox.Size = new System.Drawing.Size(100, 20);
            this.topBarCountTextBox.TabIndex = 8;
            this.topBarCountTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.topBarCountTextBox_Validating);
            // 
            // stirrupBarCountTextBox
            // 
            this.stirrupBarCountTextBox.Location = new System.Drawing.Point(334, 26);
            this.stirrupBarCountTextBox.Name = "stirrupBarCountTextBox";
            this.stirrupBarCountTextBox.Size = new System.Drawing.Size(100, 20);
            this.stirrupBarCountTextBox.TabIndex = 9;
            this.stirrupBarCountTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.stirrupBarCountTextBox_Validating);
            // 
            // topBarGroupBox
            // 
            this.topBarGroupBox.Controls.Add(this.topBarCountLabel);
            this.topBarGroupBox.Controls.Add(this.topBarTypeComboBox);
            this.topBarGroupBox.Controls.Add(this.topBarTypeLabel);
            this.topBarGroupBox.Controls.Add(this.topBarCountTextBox);
            this.topBarGroupBox.Location = new System.Drawing.Point(12, 80);
            this.topBarGroupBox.Name = "topBarGroupBox";
            this.topBarGroupBox.Size = new System.Drawing.Size(448, 54);
            this.topBarGroupBox.TabIndex = 10;
            this.topBarGroupBox.TabStop = false;
            this.topBarGroupBox.Text = "Top Bars";
            // 
            // topBarCountLabel
            // 
            this.topBarCountLabel.AutoSize = true;
            this.topBarCountLabel.Location = new System.Drawing.Point(254, 22);
            this.topBarCountLabel.Name = "topBarCountLabel";
            this.topBarCountLabel.Size = new System.Drawing.Size(57, 13);
            this.topBarCountLabel.TabIndex = 9;
            this.topBarCountLabel.Text = "Bar Count:";
            // 
            // stirrupBarGroupBox
            // 
            this.stirrupBarGroupBox.Controls.Add(this.stirrupBarCountLabel);
            this.stirrupBarGroupBox.Controls.Add(this.stirrupBarTypeComboBox);
            this.stirrupBarGroupBox.Controls.Add(this.stirrupBarTypeLabel);
            this.stirrupBarGroupBox.Controls.Add(this.stirrupBarCountTextBox);
            this.stirrupBarGroupBox.Location = new System.Drawing.Point(12, 164);
            this.stirrupBarGroupBox.Name = "stirrupBarGroupBox";
            this.stirrupBarGroupBox.Size = new System.Drawing.Size(448, 54);
            this.stirrupBarGroupBox.TabIndex = 11;
            this.stirrupBarGroupBox.TabStop = false;
            this.stirrupBarGroupBox.Text = "Stirrup Bars";
            // 
            // stirrupBarCountLabel
            // 
            this.stirrupBarCountLabel.AutoSize = true;
            this.stirrupBarCountLabel.Location = new System.Drawing.Point(254, 26);
            this.stirrupBarCountLabel.Name = "stirrupBarCountLabel";
            this.stirrupBarCountLabel.Size = new System.Drawing.Size(57, 13);
            this.stirrupBarCountLabel.TabIndex = 10;
            this.stirrupBarCountLabel.Text = "Bar Count:";
            // 
            // multiplanarBarGroupBox
            // 
            this.multiplanarBarGroupBox.Controls.Add(this.multiplanarBarTypeComboBox);
            this.multiplanarBarGroupBox.Controls.Add(this.multiplanarBarTypeLabel);
            this.multiplanarBarGroupBox.Location = new System.Drawing.Point(12, 251);
            this.multiplanarBarGroupBox.Name = "multiplanarBarGroupBox";
            this.multiplanarBarGroupBox.Size = new System.Drawing.Size(448, 54);
            this.multiplanarBarGroupBox.TabIndex = 12;
            this.multiplanarBarGroupBox.TabStop = false;
            this.multiplanarBarGroupBox.Text = "Multiplanar Bars";
            // 
            // columnGroupBox
            // 
            this.columnGroupBox.Controls.Add(this.columnBarTypeComboBox);
            this.columnGroupBox.Controls.Add(this.columnBarTypeLabel);
            this.columnGroupBox.Location = new System.Drawing.Point(12, 12);
            this.columnGroupBox.Name = "columnGroupBox";
            this.columnGroupBox.Size = new System.Drawing.Size(448, 54);
            this.columnGroupBox.TabIndex = 13;
            this.columnGroupBox.TabStop = false;
            this.columnGroupBox.Text = "Host Straight Bars";
            // 
            // columnBarTypeComboBox
            // 
            this.columnBarTypeComboBox.FormattingEnabled = true;
            this.columnBarTypeComboBox.Location = new System.Drawing.Point(86, 22);
            this.columnBarTypeComboBox.Name = "columnBarTypeComboBox";
            this.columnBarTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.columnBarTypeComboBox.TabIndex = 7;
            this.columnBarTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.columnBarTypeComboBox_SelectedIndexChanged);
            // 
            // columnBarTypeLabel
            // 
            this.columnBarTypeLabel.AutoSize = true;
            this.columnBarTypeLabel.Location = new System.Drawing.Point(7, 25);
            this.columnBarTypeLabel.Name = "columnBarTypeLabel";
            this.columnBarTypeLabel.Size = new System.Drawing.Size(53, 13);
            this.columnBarTypeLabel.TabIndex = 6;
            this.columnBarTypeLabel.Text = "Bar Type:";
            // 
            // CorbelReinforcementOptionsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(476, 387);
            this.ControlBox = false;
            this.Controls.Add(this.columnGroupBox);
            this.Controls.Add(this.multiplanarBarGroupBox);
            this.Controls.Add(this.stirrupBarGroupBox);
            this.Controls.Add(this.topBarGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "CorbelReinforcementOptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Corbel Reinforcement Options";
            this.topBarGroupBox.ResumeLayout(false);
            this.topBarGroupBox.PerformLayout();
            this.stirrupBarGroupBox.ResumeLayout(false);
            this.stirrupBarGroupBox.PerformLayout();
            this.multiplanarBarGroupBox.ResumeLayout(false);
            this.multiplanarBarGroupBox.PerformLayout();
            this.columnGroupBox.ResumeLayout(false);
            this.columnGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox topBarTypeComboBox;
        private System.Windows.Forms.ComboBox stirrupBarTypeComboBox;
        private System.Windows.Forms.Label topBarTypeLabel;
        private System.Windows.Forms.Label stirrupBarTypeLabel;
        private System.Windows.Forms.Label multiplanarBarTypeLabel;
        private System.Windows.Forms.ComboBox multiplanarBarTypeComboBox;
        private System.Windows.Forms.TextBox topBarCountTextBox;
        private System.Windows.Forms.TextBox stirrupBarCountTextBox;
        private System.Windows.Forms.GroupBox topBarGroupBox;
        private System.Windows.Forms.GroupBox stirrupBarGroupBox;
        private System.Windows.Forms.GroupBox multiplanarBarGroupBox;
        private System.Windows.Forms.Label topBarCountLabel;
        private System.Windows.Forms.Label stirrupBarCountLabel;
        private System.Windows.Forms.GroupBox columnGroupBox;
        private System.Windows.Forms.ComboBox columnBarTypeComboBox;
        private System.Windows.Forms.Label columnBarTypeLabel;

    }
}