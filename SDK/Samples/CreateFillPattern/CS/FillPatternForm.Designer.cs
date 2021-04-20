namespace Revit.SDK.Samples.CreateFillPattern.CS
{
    partial class PatternForm
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
            this.buttonApplyToSurface = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.treeViewFillPattern = new System.Windows.Forms.TreeView();
            this.buttonApplyToCutSurface = new System.Windows.Forms.Button();
            this.tabControlFillPattern = new System.Windows.Forms.TabControl();
            this.tabPageFillPattern = new System.Windows.Forms.TabPage();
            this.tabPageLinePattern = new System.Windows.Forms.TabPage();
            this.buttonApplyToGrids = new System.Windows.Forms.Button();
            this.treeViewLinePattern = new System.Windows.Forms.TreeView();
            this.buttonCreateFillPattern = new System.Windows.Forms.Button();
            this.buttonCreateLinePattern = new System.Windows.Forms.Button();
            this.buttonCreateComplexFillPattern = new System.Windows.Forms.Button();
            this.tabControlFillPattern.SuspendLayout();
            this.tabPageFillPattern.SuspendLayout();
            this.tabPageLinePattern.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonApplyToSurface
            // 
            this.buttonApplyToSurface.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApplyToSurface.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonApplyToSurface.Location = new System.Drawing.Point(6, 328);
            this.buttonApplyToSurface.Name = "buttonApplyToSurface";
            this.buttonApplyToSurface.Size = new System.Drawing.Size(145, 23);
            this.buttonApplyToSurface.TabIndex = 0;
            this.buttonApplyToSurface.Text = "Apply To Surface";
            this.buttonApplyToSurface.UseVisualStyleBackColor = true;
            this.buttonApplyToSurface.Click += new System.EventHandler(this.buttonApplyToSurface_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(212, 440);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(118, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // treeViewFillPattern
            // 
            this.treeViewFillPattern.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewFillPattern.Location = new System.Drawing.Point(-4, 0);
            this.treeViewFillPattern.Name = "treeViewFillPattern";
            this.treeViewFillPattern.Size = new System.Drawing.Size(376, 322);
            this.treeViewFillPattern.TabIndex = 0;
            // 
            // buttonApplyToCutSurface
            // 
            this.buttonApplyToCutSurface.Location = new System.Drawing.Point(181, 328);
            this.buttonApplyToCutSurface.Name = "buttonApplyToCutSurface";
            this.buttonApplyToCutSurface.Size = new System.Drawing.Size(118, 23);
            this.buttonApplyToCutSurface.TabIndex = 3;
            this.buttonApplyToCutSurface.Text = "Apply To CutSurface";
            this.buttonApplyToCutSurface.UseVisualStyleBackColor = true;
            this.buttonApplyToCutSurface.Click += new System.EventHandler(this.buttonApplyToCutSurface_Click);
            // 
            // tabControlFillPattern
            // 
            this.tabControlFillPattern.Controls.Add(this.tabPageFillPattern);
            this.tabControlFillPattern.Controls.Add(this.tabPageLinePattern);
            this.tabControlFillPattern.Location = new System.Drawing.Point(27, 22);
            this.tabControlFillPattern.Name = "tabControlFillPattern";
            this.tabControlFillPattern.SelectedIndex = 0;
            this.tabControlFillPattern.Size = new System.Drawing.Size(384, 383);
            this.tabControlFillPattern.TabIndex = 4;
            // 
            // tabPageFillPattern
            // 
            this.tabPageFillPattern.Controls.Add(this.treeViewFillPattern);
            this.tabPageFillPattern.Controls.Add(this.buttonApplyToSurface);
            this.tabPageFillPattern.Controls.Add(this.buttonApplyToCutSurface);
            this.tabPageFillPattern.Location = new System.Drawing.Point(4, 22);
            this.tabPageFillPattern.Name = "tabPageFillPattern";
            this.tabPageFillPattern.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFillPattern.Size = new System.Drawing.Size(376, 357);
            this.tabPageFillPattern.TabIndex = 0;
            this.tabPageFillPattern.Text = "FillPatterns";
            this.tabPageFillPattern.UseVisualStyleBackColor = true;
            // 
            // tabPageLinePattern
            // 
            this.tabPageLinePattern.Controls.Add(this.buttonApplyToGrids);
            this.tabPageLinePattern.Controls.Add(this.treeViewLinePattern);
            this.tabPageLinePattern.Location = new System.Drawing.Point(4, 22);
            this.tabPageLinePattern.Name = "tabPageLinePattern";
            this.tabPageLinePattern.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLinePattern.Size = new System.Drawing.Size(376, 357);
            this.tabPageLinePattern.TabIndex = 1;
            this.tabPageLinePattern.Text = "LinePatterns";
            this.tabPageLinePattern.UseVisualStyleBackColor = true;
            // 
            // buttonApplyToGrids
            // 
            this.buttonApplyToGrids.Location = new System.Drawing.Point(6, 328);
            this.buttonApplyToGrids.Name = "buttonApplyToGrids";
            this.buttonApplyToGrids.Size = new System.Drawing.Size(118, 23);
            this.buttonApplyToGrids.TabIndex = 2;
            this.buttonApplyToGrids.Text = "Apply To Grids";
            this.buttonApplyToGrids.UseVisualStyleBackColor = true;
            this.buttonApplyToGrids.Click += new System.EventHandler(this.buttonApplyToGrids_Click);
            // 
            // treeViewLinePattern
            // 
            this.treeViewLinePattern.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewLinePattern.Location = new System.Drawing.Point(-4, 0);
            this.treeViewLinePattern.Name = "treeViewLinePattern";
            this.treeViewLinePattern.Size = new System.Drawing.Size(376, 322);
            this.treeViewLinePattern.TabIndex = 0;
            // 
            // buttonCreateFillPattern
            // 
            this.buttonCreateFillPattern.Location = new System.Drawing.Point(37, 411);
            this.buttonCreateFillPattern.Name = "buttonCreateFillPattern";
            this.buttonCreateFillPattern.Size = new System.Drawing.Size(145, 23);
            this.buttonCreateFillPattern.TabIndex = 5;
            this.buttonCreateFillPattern.Text = "Create Fill Pattern";
            this.buttonCreateFillPattern.UseVisualStyleBackColor = true;
            this.buttonCreateFillPattern.Click += new System.EventHandler(this.buttonCreateFillPattern_Click);
            // 
            // buttonCreateLinePattern
            // 
            this.buttonCreateLinePattern.Location = new System.Drawing.Point(212, 411);
            this.buttonCreateLinePattern.Name = "buttonCreateLinePattern";
            this.buttonCreateLinePattern.Size = new System.Drawing.Size(118, 23);
            this.buttonCreateLinePattern.TabIndex = 6;
            this.buttonCreateLinePattern.Text = "Create Line Pattern";
            this.buttonCreateLinePattern.UseVisualStyleBackColor = true;
            this.buttonCreateLinePattern.Click += new System.EventHandler(this.buttonCreateLinePattern_Click);
            // 
            // buttonCreateComplexFillPattern
            // 
            this.buttonCreateComplexFillPattern.Location = new System.Drawing.Point(37, 440);
            this.buttonCreateComplexFillPattern.Name = "buttonCreateComplexFillPattern";
            this.buttonCreateComplexFillPattern.Size = new System.Drawing.Size(145, 23);
            this.buttonCreateComplexFillPattern.TabIndex = 5;
            this.buttonCreateComplexFillPattern.Text = "Create Complex Fill Pattern";
            this.buttonCreateComplexFillPattern.UseVisualStyleBackColor = true;
            this.buttonCreateComplexFillPattern.Click += new System.EventHandler(this.buttonCreateComplexFillPattern_Click);
            // 
            // PatternForm
            // 
            this.AcceptButton = this.buttonApplyToSurface;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(434, 470);
            this.Controls.Add(this.buttonCreateLinePattern);
            this.Controls.Add(this.buttonCreateComplexFillPattern);
            this.Controls.Add(this.buttonCreateFillPattern);
            this.Controls.Add(this.tabControlFillPattern);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PatternForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PatternForm";
            this.tabControlFillPattern.ResumeLayout(false);
            this.tabPageFillPattern.ResumeLayout(false);
            this.tabPageLinePattern.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonApplyToSurface;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TreeView treeViewFillPattern;
        private System.Windows.Forms.Button buttonApplyToCutSurface;
        private System.Windows.Forms.TabControl tabControlFillPattern;
        private System.Windows.Forms.TabPage tabPageFillPattern;
        private System.Windows.Forms.TabPage tabPageLinePattern;
        private System.Windows.Forms.TreeView treeViewLinePattern;
        private System.Windows.Forms.Button buttonCreateFillPattern;
        private System.Windows.Forms.Button buttonCreateLinePattern;
        private System.Windows.Forms.Button buttonApplyToGrids;
        private System.Windows.Forms.Button buttonCreateComplexFillPattern;
    }
}