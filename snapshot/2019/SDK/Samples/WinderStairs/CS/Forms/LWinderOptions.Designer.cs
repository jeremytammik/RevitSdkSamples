namespace Revit.SDK.Samples.WinderStairs.CS
{
    partial class LWinderOptions
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
            this.numAtStartTextBox = new System.Windows.Forms.TextBox();
            this.numInCornerTextBox = new System.Windows.Forms.TextBox();
            this.numAtEndTextBox = new System.Windows.Forms.TextBox();
            this.runWidthTextBox = new System.Windows.Forms.TextBox();
            this.startStepLabel = new System.Windows.Forms.Label();
            this.cornerStepsNLabel = new System.Windows.Forms.Label();
            this.endStepsLabel = new System.Windows.Forms.Label();
            this.runWidthLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.previewPictureBox = new System.Windows.Forms.PictureBox();
            this.inputParamGroupBox = new System.Windows.Forms.GroupBox();
            this.centerOffsetFLabel = new System.Windows.Forms.Label();
            this.centerOffsetELabel = new System.Windows.Forms.Label();
            this.centerOffsetFTextBox = new System.Windows.Forms.TextBox();
            this.centerOffsetETextBox = new System.Windows.Forms.TextBox();
            this.dmuCheckBox = new System.Windows.Forms.CheckBox();
            this.sketchCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
            this.inputParamGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(190, 279);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 26);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // numAtStartTextBox
            // 
            this.numAtStartTextBox.Location = new System.Drawing.Point(113, 64);
            this.numAtStartTextBox.Name = "numAtStartTextBox";
            this.numAtStartTextBox.Size = new System.Drawing.Size(100, 20);
            this.numAtStartTextBox.TabIndex = 1;
            // 
            // numInCornerTextBox
            // 
            this.numInCornerTextBox.Location = new System.Drawing.Point(113, 124);
            this.numInCornerTextBox.Name = "numInCornerTextBox";
            this.numInCornerTextBox.Size = new System.Drawing.Size(100, 20);
            this.numInCornerTextBox.TabIndex = 2;
            // 
            // numAtEndTextBox
            // 
            this.numAtEndTextBox.Location = new System.Drawing.Point(113, 94);
            this.numAtEndTextBox.Name = "numAtEndTextBox";
            this.numAtEndTextBox.Size = new System.Drawing.Size(100, 20);
            this.numAtEndTextBox.TabIndex = 3;
            // 
            // runWidthTextBox
            // 
            this.runWidthTextBox.Location = new System.Drawing.Point(113, 34);
            this.runWidthTextBox.Name = "runWidthTextBox";
            this.runWidthTextBox.Size = new System.Drawing.Size(100, 20);
            this.runWidthTextBox.TabIndex = 4;
            // 
            // startStepLabel
            // 
            this.startStepLabel.AutoSize = true;
            this.startStepLabel.Location = new System.Drawing.Point(24, 64);
            this.startStepLabel.Name = "startStepLabel";
            this.startStepLabel.Size = new System.Drawing.Size(78, 13);
            this.startStepLabel.TabIndex = 6;
            this.startStepLabel.Text = "Start Steps (A):";
            // 
            // cornerStepsNLabel
            // 
            this.cornerStepsNLabel.AutoSize = true;
            this.cornerStepsNLabel.Location = new System.Drawing.Point(14, 124);
            this.cornerStepsNLabel.Name = "cornerStepsNLabel";
            this.cornerStepsNLabel.Size = new System.Drawing.Size(88, 13);
            this.cornerStepsNLabel.TabIndex = 7;
            this.cornerStepsNLabel.Text = "Corner Steps (N):";
            // 
            // endStepsLabel
            // 
            this.endStepsLabel.AutoSize = true;
            this.endStepsLabel.Location = new System.Drawing.Point(27, 94);
            this.endStepsLabel.Name = "endStepsLabel";
            this.endStepsLabel.Size = new System.Drawing.Size(75, 13);
            this.endStepsLabel.TabIndex = 8;
            this.endStepsLabel.Text = "End Steps (B):";
            // 
            // runWidthLabel
            // 
            this.runWidthLabel.AutoSize = true;
            this.runWidthLabel.Location = new System.Drawing.Point(25, 34);
            this.runWidthLabel.Name = "runWidthLabel";
            this.runWidthLabel.Size = new System.Drawing.Size(77, 13);
            this.runWidthLabel.TabIndex = 9;
            this.runWidthLabel.Text = "Run Width (C):";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(282, 279);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 26);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // previewPictureBox
            // 
            this.previewPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.previewPictureBox.Image = global::Revit.SDK.Samples.WinderStairs.CS.Properties.Resources.LWinder;
            this.previewPictureBox.Location = new System.Drawing.Point(258, 12);
            this.previewPictureBox.Name = "previewPictureBox";
            this.previewPictureBox.Size = new System.Drawing.Size(269, 234);
            this.previewPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.previewPictureBox.TabIndex = 5;
            this.previewPictureBox.TabStop = false;
            // 
            // inputParamGroupBox
            // 
            this.inputParamGroupBox.Controls.Add(this.startStepLabel);
            this.inputParamGroupBox.Controls.Add(this.runWidthLabel);
            this.inputParamGroupBox.Controls.Add(this.numAtStartTextBox);
            this.inputParamGroupBox.Controls.Add(this.centerOffsetFLabel);
            this.inputParamGroupBox.Controls.Add(this.centerOffsetELabel);
            this.inputParamGroupBox.Controls.Add(this.endStepsLabel);
            this.inputParamGroupBox.Controls.Add(this.numInCornerTextBox);
            this.inputParamGroupBox.Controls.Add(this.cornerStepsNLabel);
            this.inputParamGroupBox.Controls.Add(this.centerOffsetFTextBox);
            this.inputParamGroupBox.Controls.Add(this.centerOffsetETextBox);
            this.inputParamGroupBox.Controls.Add(this.numAtEndTextBox);
            this.inputParamGroupBox.Controls.Add(this.runWidthTextBox);
            this.inputParamGroupBox.Location = new System.Drawing.Point(12, 12);
            this.inputParamGroupBox.Name = "inputParamGroupBox";
            this.inputParamGroupBox.Size = new System.Drawing.Size(229, 234);
            this.inputParamGroupBox.TabIndex = 10;
            this.inputParamGroupBox.TabStop = false;
            this.inputParamGroupBox.Text = "Input Parameters";
            // 
            // centerOffsetFLabel
            // 
            this.centerOffsetFLabel.AutoSize = true;
            this.centerOffsetFLabel.Location = new System.Drawing.Point(15, 184);
            this.centerOffsetFLabel.Name = "centerOffsetFLabel";
            this.centerOffsetFLabel.Size = new System.Drawing.Size(87, 13);
            this.centerOffsetFLabel.TabIndex = 8;
            this.centerOffsetFLabel.Text = "Center Offset (F):";
            // 
            // centerOffsetELabel
            // 
            this.centerOffsetELabel.AutoSize = true;
            this.centerOffsetELabel.Location = new System.Drawing.Point(14, 154);
            this.centerOffsetELabel.Name = "centerOffsetELabel";
            this.centerOffsetELabel.Size = new System.Drawing.Size(88, 13);
            this.centerOffsetELabel.TabIndex = 8;
            this.centerOffsetELabel.Text = "Center Offset (E):";
            // 
            // centerOffsetFTextBox
            // 
            this.centerOffsetFTextBox.Location = new System.Drawing.Point(113, 184);
            this.centerOffsetFTextBox.Name = "centerOffsetFTextBox";
            this.centerOffsetFTextBox.Size = new System.Drawing.Size(100, 20);
            this.centerOffsetFTextBox.TabIndex = 3;
            // 
            // centerOffsetETextBox
            // 
            this.centerOffsetETextBox.Location = new System.Drawing.Point(113, 154);
            this.centerOffsetETextBox.Name = "centerOffsetETextBox";
            this.centerOffsetETextBox.Size = new System.Drawing.Size(100, 20);
            this.centerOffsetETextBox.TabIndex = 3;
            // 
            // dmuCheckBox
            // 
            this.dmuCheckBox.AutoSize = true;
            this.dmuCheckBox.Location = new System.Drawing.Point(378, 285);
            this.dmuCheckBox.Name = "dmuCheckBox";
            this.dmuCheckBox.Size = new System.Drawing.Size(51, 17);
            this.dmuCheckBox.TabIndex = 11;
            this.dmuCheckBox.Text = "DMU";
            this.dmuCheckBox.UseVisualStyleBackColor = true;
            // 
            // sketchCheckBox
            // 
            this.sketchCheckBox.AutoSize = true;
            this.sketchCheckBox.Location = new System.Drawing.Point(440, 285);
            this.sketchCheckBox.Name = "sketchCheckBox";
            this.sketchCheckBox.Size = new System.Drawing.Size(60, 17);
            this.sketchCheckBox.TabIndex = 24;
            this.sketchCheckBox.Text = "Sketch";
            this.sketchCheckBox.UseVisualStyleBackColor = true;
            // 
            // LWinderOptions
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(546, 324);
            this.ControlBox = false;
            this.Controls.Add(this.sketchCheckBox);
            this.Controls.Add(this.dmuCheckBox);
            this.Controls.Add(this.inputParamGroupBox);
            this.Controls.Add(this.previewPictureBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LWinderOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "L-Winder Options";
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
            this.inputParamGroupBox.ResumeLayout(false);
            this.inputParamGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox numAtStartTextBox;
        private System.Windows.Forms.TextBox numInCornerTextBox;
        private System.Windows.Forms.TextBox numAtEndTextBox;
        private System.Windows.Forms.TextBox runWidthTextBox;
        private System.Windows.Forms.PictureBox previewPictureBox;
        private System.Windows.Forms.Label startStepLabel;
        private System.Windows.Forms.Label cornerStepsNLabel;
        private System.Windows.Forms.Label endStepsLabel;
        private System.Windows.Forms.Label runWidthLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox inputParamGroupBox;
        private System.Windows.Forms.Label centerOffsetFLabel;
        private System.Windows.Forms.Label centerOffsetELabel;
        private System.Windows.Forms.TextBox centerOffsetFTextBox;
        private System.Windows.Forms.TextBox centerOffsetETextBox;
        private System.Windows.Forms.CheckBox dmuCheckBox;
        private System.Windows.Forms.CheckBox sketchCheckBox;
    }
}