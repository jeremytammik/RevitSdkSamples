using System;

namespace Revit.SDK.Samples.UIAPI.CS
{
    partial class PreviewModel
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
                (_uiApplication as IDisposable)?.Dispose();
                (_dbDocument as IDisposable)?.Dispose();
                (_application as IDisposable)?.Dispose();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._cbDocuments = new System.Windows.Forms.ComboBox();
            this._cbViews = new System.Windows.Forms.ComboBox();
            this._elementHostWPF = new System.Windows.Forms.Integration.ElementHost();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._elementHostWPF);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(467, 452);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(266, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Views:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Documents:";
            // 
            // _cbDocuments
            // 
            this._cbDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._cbDocuments.FormattingEnabled = true;
            this._cbDocuments.Location = new System.Drawing.Point(12, 56);
            this._cbDocuments.Name = "_cbDocuments";
            this._cbDocuments.Size = new System.Drawing.Size(183, 21);
            this._cbDocuments.TabIndex = 2;
            this._cbDocuments.SelectedIndexChanged += new System.EventHandler(this.cbDocs_SelIdxChanged);
            // 
            // _cbViews
            // 
            this._cbViews.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._cbViews.FormattingEnabled = true;
            this._cbViews.Location = new System.Drawing.Point(269, 56);
            this._cbViews.Name = "_cbViews";
            this._cbViews.Size = new System.Drawing.Size(183, 21);
            this._cbViews.TabIndex = 1;
            this._cbViews.SelectedIndexChanged += new System.EventHandler(this.cbViews_SelIdxChanged);
            // 
            // _elementHostWPF
            // 
            this._elementHostWPF.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._elementHostWPF.Location = new System.Drawing.Point(0, 0);
            this._elementHostWPF.Name = "_elementHostWPF";
            this._elementHostWPF.Size = new System.Drawing.Size(467, 367);
            this._elementHostWPF.TabIndex = 0;
            this._elementHostWPF.Text = "elementHost1";
            this._elementHostWPF.Child = null;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this._cbViews);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this._cbDocuments);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(3, 363);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(461, 89);
            this.panel2.TabIndex = 5;
            // 
            // PreviewModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 452);
            this.Controls.Add(this.panel1);
            this.Name = "PreviewModel";
            this.Text = "PreviewModel";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox _cbViews;
        private System.Windows.Forms.Integration.ElementHost _elementHostWPF;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _cbDocuments;
        private System.Windows.Forms.Panel panel2;
    }
}