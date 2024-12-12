namespace Revit.SDK.Samples.GenericStructuralConnection.CS
{
    partial class StructuralConnectionForm
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
            this.rbCreateGeneric = new System.Windows.Forms.RadioButton();
            this.rbDeleteGeneric = new System.Windows.Forms.RadioButton();
            this.rbReadGeneric = new System.Windows.Forms.RadioButton();
            this.gbCommands = new System.Windows.Forms.GroupBox();
            this.rbUpdateGeneric = new System.Windows.Forms.RadioButton();
            this.rbCreateDetailed = new System.Windows.Forms.RadioButton();
            this.rbChangedDetail = new System.Windows.Forms.RadioButton();
            this.rbCopyDetailed = new System.Windows.Forms.RadioButton();
            this.rbMatchPropDetailed = new System.Windows.Forms.RadioButton();
            this.rbResetDetailed = new System.Windows.Forms.RadioButton();
            this.gbCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(162, 281);
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
            this.buttonCancel.Location = new System.Drawing.Point(243, 281);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // rbCreateGeneric
            // 
            this.rbCreateGeneric.AutoSize = true;
            this.rbCreateGeneric.Checked = true;
            this.rbCreateGeneric.Location = new System.Drawing.Point(6, 19);
            this.rbCreateGeneric.Name = "rbCreateGeneric";
            this.rbCreateGeneric.Size = new System.Drawing.Size(196, 17);
            this.rbCreateGeneric.TabIndex = 2;
            this.rbCreateGeneric.TabStop = true;
            this.rbCreateGeneric.Text = "Create generic structural connection";
            this.rbCreateGeneric.UseVisualStyleBackColor = true;
            // 
            // rbDeleteGeneric
            // 
            this.rbDeleteGeneric.AutoSize = true;
            this.rbDeleteGeneric.Location = new System.Drawing.Point(6, 43);
            this.rbDeleteGeneric.Name = "rbDeleteGeneric";
            this.rbDeleteGeneric.Size = new System.Drawing.Size(196, 17);
            this.rbDeleteGeneric.TabIndex = 3;
            this.rbDeleteGeneric.Text = "Delete generic structural connection";
            this.rbDeleteGeneric.UseVisualStyleBackColor = true;
            // 
            // rbReadGeneric
            // 
            this.rbReadGeneric.AutoSize = true;
            this.rbReadGeneric.Location = new System.Drawing.Point(6, 67);
            this.rbReadGeneric.Name = "rbReadGeneric";
            this.rbReadGeneric.Size = new System.Drawing.Size(191, 17);
            this.rbReadGeneric.TabIndex = 4;
            this.rbReadGeneric.Text = "Read generic structural connection";
            this.rbReadGeneric.UseVisualStyleBackColor = true;
            // 
            // gbCommands
            // 
            this.gbCommands.Controls.Add(this.rbResetDetailed);
            this.gbCommands.Controls.Add(this.rbMatchPropDetailed);
            this.gbCommands.Controls.Add(this.rbCopyDetailed);
            this.gbCommands.Controls.Add(this.rbChangedDetail);
            this.gbCommands.Controls.Add(this.rbCreateDetailed);
            this.gbCommands.Controls.Add(this.rbUpdateGeneric);
            this.gbCommands.Controls.Add(this.rbReadGeneric);
            this.gbCommands.Controls.Add(this.rbCreateGeneric);
            this.gbCommands.Controls.Add(this.rbDeleteGeneric);
            this.gbCommands.Location = new System.Drawing.Point(13, 13);
            this.gbCommands.Name = "gbCommands";
            this.gbCommands.Size = new System.Drawing.Size(305, 247);
            this.gbCommands.TabIndex = 5;
            this.gbCommands.TabStop = false;
            this.gbCommands.Text = "Commands";
            // 
            // rbUpdateGeneric
            // 
            this.rbUpdateGeneric.AutoSize = true;
            this.rbUpdateGeneric.Location = new System.Drawing.Point(6, 91);
            this.rbUpdateGeneric.Name = "rbUpdateGeneric";
            this.rbUpdateGeneric.Size = new System.Drawing.Size(200, 17);
            this.rbUpdateGeneric.TabIndex = 5;
            this.rbUpdateGeneric.TabStop = true;
            this.rbUpdateGeneric.Text = "Update generic structural connection";
            this.rbUpdateGeneric.UseVisualStyleBackColor = true;
            // 
            // rbCreateDetailed
            // 
            this.rbCreateDetailed.AutoSize = true;
            this.rbCreateDetailed.Location = new System.Drawing.Point(7, 115);
            this.rbCreateDetailed.Name = "rbCreateDetailed";
            this.rbCreateDetailed.Size = new System.Drawing.Size(198, 17);
            this.rbCreateDetailed.TabIndex = 6;
            this.rbCreateDetailed.TabStop = true;
            this.rbCreateDetailed.Text = "Create detailed structural connection";
            this.rbCreateDetailed.UseVisualStyleBackColor = true;
            // 
            // rbChangedDetail
            // 
            this.rbChangedDetail.AutoSize = true;
            this.rbChangedDetail.Location = new System.Drawing.Point(7, 139);
            this.rbChangedDetail.Name = "rbChangedDetail";
            this.rbChangedDetail.Size = new System.Drawing.Size(204, 17);
            this.rbChangedDetail.TabIndex = 7;
            this.rbChangedDetail.TabStop = true;
            this.rbChangedDetail.Text = "Change detailed structural connection";
            this.rbChangedDetail.UseVisualStyleBackColor = true;
            // 
            // rbCopyDetailed
            // 
            this.rbCopyDetailed.AutoSize = true;
            this.rbCopyDetailed.Location = new System.Drawing.Point(7, 162);
            this.rbCopyDetailed.Name = "rbCopyDetailed";
            this.rbCopyDetailed.Size = new System.Drawing.Size(191, 17);
            this.rbCopyDetailed.TabIndex = 8;
            this.rbCopyDetailed.TabStop = true;
            this.rbCopyDetailed.Text = "Copy detailed structural connection";
            this.rbCopyDetailed.UseVisualStyleBackColor = true;
            // 
            // rbMatchPropDetailed
            // 
            this.rbMatchPropDetailed.AutoSize = true;
            this.rbMatchPropDetailed.Location = new System.Drawing.Point(7, 187);
            this.rbMatchPropDetailed.Name = "rbMatchPropDetailed";
            this.rbMatchPropDetailed.Size = new System.Drawing.Size(258, 17);
            this.rbMatchPropDetailed.TabIndex = 9;
            this.rbMatchPropDetailed.TabStop = true;
            this.rbMatchPropDetailed.Text = "Match properties of detailed structural connection";
            this.rbMatchPropDetailed.UseVisualStyleBackColor = true;
            // 
            // rbResetDetailed
            // 
            this.rbResetDetailed.AutoSize = true;
            this.rbResetDetailed.Location = new System.Drawing.Point(6, 210);
            this.rbResetDetailed.Name = "rbResetDetailed";
            this.rbResetDetailed.Size = new System.Drawing.Size(245, 17);
            this.rbResetDetailed.TabIndex = 10;
            this.rbResetDetailed.TabStop = true;
            this.rbResetDetailed.Text = "Reset detailed structural connection to generic";
            this.rbResetDetailed.UseVisualStyleBackColor = true;
            // 
            // StructuralConnectionForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(330, 316);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.gbCommands);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StructuralConnectionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Structural connection operations";
            this.gbCommands.ResumeLayout(false);
            this.gbCommands.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.RadioButton rbCreateGeneric;
        private System.Windows.Forms.RadioButton rbDeleteGeneric;
        private System.Windows.Forms.RadioButton rbReadGeneric;
        private System.Windows.Forms.GroupBox gbCommands;
        private System.Windows.Forms.RadioButton rbUpdateGeneric;
        private System.Windows.Forms.RadioButton rbCreateDetailed;
        private System.Windows.Forms.RadioButton rbChangedDetail;
        private System.Windows.Forms.RadioButton rbCopyDetailed;
        private System.Windows.Forms.RadioButton rbResetDetailed;
        private System.Windows.Forms.RadioButton rbMatchPropDetailed;
    }
}