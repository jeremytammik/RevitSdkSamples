namespace REX.Serialization.Resources.Dialogs
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ButtonApply = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.Buttons = new System.Windows.Forms.Panel();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.About = new System.Windows.Forms.LinkLabel();
            this.Help = new System.Windows.Forms.LinkLabel();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.Buttons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonApply
            // 
            resources.ApplyResources(this.ButtonApply, "ButtonApply");
            this.ButtonApply.Name = "ButtonApply";
            this.ButtonApply.UseVisualStyleBackColor = true;
            this.ButtonApply.Click += new System.EventHandler(this.ButtonApply_Click);
            // 
            // ButtonCancel
            // 
            resources.ApplyResources(this.ButtonCancel, "ButtonCancel");
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // Buttons
            // 
            this.Buttons.Controls.Add(this.About);
            this.Buttons.Controls.Add(this.Help);
            this.Buttons.Controls.Add(this.ButtonClose);
            this.Buttons.Controls.Add(this.ButtonCancel);
            this.Buttons.Controls.Add(this.ButtonApply);
            this.Buttons.Controls.Add(this.Logo);
            resources.ApplyResources(this.Buttons, "Buttons");
            this.Buttons.Name = "Buttons";
            // 
            // Logo
            // 
            this.Logo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.Logo, "Logo");
            this.Logo.Name = "Logo";
            this.Logo.TabStop = false;
            // 
            // About
            // 
            resources.ApplyResources(this.About, "About");
            this.About.Name = "About";
            this.About.TabStop = true;
            this.About.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.About_LinkClicked);
            // 
            // Help
            // 
            resources.ApplyResources(this.Help, "Help");
            this.Help.Name = "Help";
            this.Help.TabStop = true;
            this.Help.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Help_LinkClicked);
            // 
            // ButtonClose
            // 
            resources.ApplyResources(this.ButtonClose, "ButtonClose");
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // MainPanel
            // 
            resources.ApplyResources(this.MainPanel, "MainPanel");
            this.MainPanel.Name = "MainPanel";
            // 
            // MainForm
            // 
            this.AcceptButton = this.ButtonApply;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.Buttons);
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Buttons.ResumeLayout(false);
            this.Buttons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonApply;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Panel Buttons;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.LinkLabel Help;
        private System.Windows.Forms.LinkLabel About;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.PictureBox Logo;

    }
}