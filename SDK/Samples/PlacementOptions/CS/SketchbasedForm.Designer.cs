namespace Revit.SDK.Samples.PlacementOptions.CS
{
    partial class SketchbasedForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxFamilySymbol = new System.Windows.Forms.ComboBox();
            this.radioButtonPEllipse = new System.Windows.Forms.RadioButton();
            this.radioButtonSpline = new System.Windows.Forms.RadioButton();
            this.radioButtonArc3P = new System.Windows.Forms.RadioButton();
            this.radioButtonLine = new System.Windows.Forms.RadioButton();
            this.radioButtonPickLine = new System.Windows.Forms.RadioButton();
            this.radioButtonArcC = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(232, 168);
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
            this.buttonCancel.Location = new System.Drawing.Point(313, 168);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Available Family Symbol:";
            // 
            // comboBoxFamilySymbol
            // 
            this.comboBoxFamilySymbol.FormattingEnabled = true;
            this.comboBoxFamilySymbol.Location = new System.Drawing.Point(152, 14);
            this.comboBoxFamilySymbol.Name = "comboBoxFamilySymbol";
            this.comboBoxFamilySymbol.Size = new System.Drawing.Size(193, 21);
            this.comboBoxFamilySymbol.TabIndex = 12;
            this.comboBoxFamilySymbol.SelectedIndexChanged += new System.EventHandler(this.comboBoxFamilySymbol_SelectedIndexChanged);
            // 
            // radioButtonPEllipse
            // 
            this.radioButtonPEllipse.AutoSize = true;
            this.radioButtonPEllipse.Location = new System.Drawing.Point(195, 89);
            this.radioButtonPEllipse.Name = "radioButtonPEllipse";
            this.radioButtonPEllipse.Size = new System.Drawing.Size(113, 17);
            this.radioButtonPEllipse.TabIndex = 11;
            this.radioButtonPEllipse.Text = "SGO_PartialEllipse";
            this.radioButtonPEllipse.UseVisualStyleBackColor = true;
            this.radioButtonPEllipse.CheckedChanged += new System.EventHandler(this.radioButtonPEllipse_CheckedChanged);
            // 
            // radioButtonSpline
            // 
            this.radioButtonSpline.AutoSize = true;
            this.radioButtonSpline.Location = new System.Drawing.Point(195, 55);
            this.radioButtonSpline.Name = "radioButtonSpline";
            this.radioButtonSpline.Size = new System.Drawing.Size(86, 17);
            this.radioButtonSpline.TabIndex = 10;
            this.radioButtonSpline.Text = "SGO_Spline ";
            this.radioButtonSpline.UseVisualStyleBackColor = true;
            this.radioButtonSpline.CheckedChanged += new System.EventHandler(this.radioButtonSpline_CheckedChanged);
            // 
            // radioButtonArc3P
            // 
            this.radioButtonArc3P.AutoSize = true;
            this.radioButtonArc3P.Location = new System.Drawing.Point(52, 89);
            this.radioButtonArc3P.Name = "radioButtonArc3P";
            this.radioButtonArc3P.Size = new System.Drawing.Size(100, 17);
            this.radioButtonArc3P.TabIndex = 9;
            this.radioButtonArc3P.Text = "SGO_Arc3Point";
            this.radioButtonArc3P.UseVisualStyleBackColor = true;
            this.radioButtonArc3P.CheckedChanged += new System.EventHandler(this.radioButtonArc3P_CheckedChanged);
            // 
            // radioButtonLine
            // 
            this.radioButtonLine.AutoSize = true;
            this.radioButtonLine.Checked = true;
            this.radioButtonLine.Location = new System.Drawing.Point(52, 53);
            this.radioButtonLine.Name = "radioButtonLine";
            this.radioButtonLine.Size = new System.Drawing.Size(74, 17);
            this.radioButtonLine.TabIndex = 8;
            this.radioButtonLine.TabStop = true;
            this.radioButtonLine.Text = "SGO_Line";
            this.radioButtonLine.UseVisualStyleBackColor = true;
            this.radioButtonLine.CheckedChanged += new System.EventHandler(this.radioButtonLine_CheckedChanged);
            // 
            // radioButtonPickLine
            // 
            this.radioButtonPickLine.AutoSize = true;
            this.radioButtonPickLine.Location = new System.Drawing.Point(195, 124);
            this.radioButtonPickLine.Name = "radioButtonPickLine";
            this.radioButtonPickLine.Size = new System.Drawing.Size(100, 17);
            this.radioButtonPickLine.TabIndex = 14;
            this.radioButtonPickLine.Text = "SGO_PickLines";
            this.radioButtonPickLine.UseVisualStyleBackColor = true;
            this.radioButtonPickLine.CheckedChanged += new System.EventHandler(this.radioButtonPickLine_CheckedChanged);
            // 
            // radioButtonArcC
            // 
            this.radioButtonArcC.AutoSize = true;
            this.radioButtonArcC.Location = new System.Drawing.Point(52, 122);
            this.radioButtonArcC.Name = "radioButtonArcC";
            this.radioButtonArcC.Size = new System.Drawing.Size(125, 17);
            this.radioButtonArcC.TabIndex = 13;
            this.radioButtonArcC.Text = "SGO_ArcCenterEnds";
            this.radioButtonArcC.UseVisualStyleBackColor = true;
            this.radioButtonArcC.CheckedChanged += new System.EventHandler(this.radioButtonArcC_CheckedChanged);
            // 
            // SketchbasedForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(400, 203);
            this.Controls.Add(this.radioButtonPickLine);
            this.Controls.Add(this.radioButtonArcC);
            this.Controls.Add(this.comboBoxFamilySymbol);
            this.Controls.Add(this.radioButtonPEllipse);
            this.Controls.Add(this.radioButtonSpline);
            this.Controls.Add(this.radioButtonArc3P);
            this.Controls.Add(this.radioButtonLine);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SketchbasedForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Place sketchbased family instance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxFamilySymbol;
        private System.Windows.Forms.RadioButton radioButtonPEllipse;
        private System.Windows.Forms.RadioButton radioButtonSpline;
        private System.Windows.Forms.RadioButton radioButtonArc3P;
        private System.Windows.Forms.RadioButton radioButtonLine;
        private System.Windows.Forms.RadioButton radioButtonPickLine;
        private System.Windows.Forms.RadioButton radioButtonArcC;
    }
}