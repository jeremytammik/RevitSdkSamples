namespace Revit.SDK.Samples.PlacementOptions.CS
{
    partial class FacebasedForm
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
            this.radioButtonDefault = new System.Windows.Forms.RadioButton();
            this.radioButtonFace = new System.Windows.Forms.RadioButton();
            this.radioButtonVF = new System.Windows.Forms.RadioButton();
            this.radioButtonWP = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxFamilySymbol = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(265, 153);
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
            this.buttonCancel.Location = new System.Drawing.Point(346, 153);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // radioButtonDefault
            // 
            this.radioButtonDefault.AutoSize = true;
            this.radioButtonDefault.Location = new System.Drawing.Point(51, 58);
            this.radioButtonDefault.Name = "radioButtonDefault";
            this.radioButtonDefault.Size = new System.Drawing.Size(59, 17);
            this.radioButtonDefault.TabIndex = 2;
            this.radioButtonDefault.Text = "Default";
            this.radioButtonDefault.UseVisualStyleBackColor = true;
            this.radioButtonDefault.CheckedChanged += new System.EventHandler(this.radioButtonDefault_CheckedChanged);
            // 
            // radioButtonFace
            // 
            this.radioButtonFace.AutoSize = true;
            this.radioButtonFace.Checked = true;
            this.radioButtonFace.Location = new System.Drawing.Point(51, 94);
            this.radioButtonFace.Name = "radioButtonFace";
            this.radioButtonFace.Size = new System.Drawing.Size(90, 17);
            this.radioButtonFace.TabIndex = 3;
            this.radioButtonFace.TabStop = true;
            this.radioButtonFace.Text = "PlaceOnFace";
            this.radioButtonFace.UseVisualStyleBackColor = true;
            this.radioButtonFace.CheckedChanged += new System.EventHandler(this.radioButtonFace_CheckedChanged);
            // 
            // radioButtonVF
            // 
            this.radioButtonVF.AutoSize = true;
            this.radioButtonVF.Location = new System.Drawing.Point(180, 60);
            this.radioButtonVF.Name = "radioButtonVF";
            this.radioButtonVF.Size = new System.Drawing.Size(208, 17);
            this.radioButtonVF.TabIndex = 4;
            this.radioButtonVF.Text = "PlaceOnVerticalFace (Plan View Only) ";
            this.radioButtonVF.UseVisualStyleBackColor = true;
            this.radioButtonVF.CheckedChanged += new System.EventHandler(this.radioButtonVF_CheckedChanged);
            // 
            // radioButtonWP
            // 
            this.radioButtonWP.AutoSize = true;
            this.radioButtonWP.Location = new System.Drawing.Point(180, 94);
            this.radioButtonWP.Name = "radioButtonWP";
            this.radioButtonWP.Size = new System.Drawing.Size(119, 17);
            this.radioButtonWP.TabIndex = 5;
            this.radioButtonWP.Text = "PlaceOnWorkPlane";
            this.radioButtonWP.UseVisualStyleBackColor = true;
            this.radioButtonWP.CheckedChanged += new System.EventHandler(this.radioButtonWP_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Available Family Symbol:";
            // 
            // comboBoxFamilySymbol
            // 
            this.comboBoxFamilySymbol.FormattingEnabled = true;
            this.comboBoxFamilySymbol.Location = new System.Drawing.Point(151, 19);
            this.comboBoxFamilySymbol.Name = "comboBoxFamilySymbol";
            this.comboBoxFamilySymbol.Size = new System.Drawing.Size(193, 21);
            this.comboBoxFamilySymbol.TabIndex = 7;
            this.comboBoxFamilySymbol.SelectedIndexChanged += new System.EventHandler(this.comboBoxFamilySymbol_SelectedIndexChanged);
            // 
            // FacebasedForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(433, 188);
            this.Controls.Add(this.comboBoxFamilySymbol);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonWP);
            this.Controls.Add(this.radioButtonVF);
            this.Controls.Add(this.radioButtonFace);
            this.Controls.Add(this.radioButtonDefault);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FacebasedForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Place facebased family instance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.RadioButton radioButtonDefault;
        private System.Windows.Forms.RadioButton radioButtonFace;
        private System.Windows.Forms.RadioButton radioButtonVF;
        private System.Windows.Forms.RadioButton radioButtonWP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxFamilySymbol;
    }
}