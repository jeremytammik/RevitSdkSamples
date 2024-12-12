namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
    partial class DialogViewSel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogViewSel));
            this.button_OK = new System.Windows.Forms.Button();
            this.button_CheckNone = new System.Windows.Forms.Button();
            this.button_CheckAll = new System.Windows.Forms.Button();
            this.checkedList_Views = new System.Windows.Forms.CheckedListBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.Help = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            resources.ApplyResources(this.button_OK, "button_OK");
            this.button_OK.Name = "button_OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_CheckNone
            // 
            resources.ApplyResources(this.button_CheckNone, "button_CheckNone");
            this.button_CheckNone.Name = "button_CheckNone";
            this.button_CheckNone.UseVisualStyleBackColor = true;
            this.button_CheckNone.Click += new System.EventHandler(this.button_CheckNone_Click);
            // 
            // button_CheckAll
            // 
            resources.ApplyResources(this.button_CheckAll, "button_CheckAll");
            this.button_CheckAll.Name = "button_CheckAll";
            this.button_CheckAll.UseVisualStyleBackColor = true;
            this.button_CheckAll.Click += new System.EventHandler(this.button_CheckAll_Click);
            // 
            // checkedList_Views
            // 
            resources.ApplyResources(this.checkedList_Views, "checkedList_Views");
            this.checkedList_Views.CheckOnClick = true;
            this.checkedList_Views.FormattingEnabled = true;
            this.checkedList_Views.Name = "checkedList_Views";
            // 
            // button_Cancel
            // 
            resources.ApplyResources(this.button_Cancel, "button_Cancel");
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click_1);
            // 
            // Help
            // 
            resources.ApplyResources(this.Help, "Help");
            this.Help.Name = "Help";
            this.Help.TabStop = true;
            this.Help.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Help_LinkClicked);
            // 
            // DialogViewSel
            // 
            this.AcceptButton = this.button_OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.Controls.Add(this.Help);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_CheckNone);
            this.Controls.Add(this.button_CheckAll);
            this.Controls.Add(this.checkedList_Views);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogViewSel";
            this.Load += new System.EventHandler(this.DialogViewSel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_CheckNone;
        private System.Windows.Forms.Button button_CheckAll;
        private System.Windows.Forms.CheckedListBox checkedList_Views;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.LinkLabel Help;
    }
}