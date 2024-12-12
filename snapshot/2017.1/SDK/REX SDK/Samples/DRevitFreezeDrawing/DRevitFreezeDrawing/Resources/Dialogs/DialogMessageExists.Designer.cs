namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
    partial class DialogMessageExists
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogMessageExists));
            this.label1 = new System.Windows.Forms.Label();
            this.label_Overwrite = new System.Windows.Forms.Label();
            this.button_ReplaceAll = new System.Windows.Forms.Button();
            this.button_Replace = new System.Windows.Forms.Button();
            this.button_Leave = new System.Windows.Forms.Button();
            this.button_LeaveAll = new System.Windows.Forms.Button();
            this.label_File = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label_Overwrite
            // 
            resources.ApplyResources(this.label_Overwrite, "label_Overwrite");
            this.label_Overwrite.Name = "label_Overwrite";
            // 
            // button_ReplaceAll
            // 
            resources.ApplyResources(this.button_ReplaceAll, "button_ReplaceAll");
            this.button_ReplaceAll.Name = "button_ReplaceAll";
            this.button_ReplaceAll.UseVisualStyleBackColor = true;
            this.button_ReplaceAll.Click += new System.EventHandler(this.button_ReplaceAll_Click);
            // 
            // button_Replace
            // 
            resources.ApplyResources(this.button_Replace, "button_Replace");
            this.button_Replace.Name = "button_Replace";
            this.button_Replace.UseVisualStyleBackColor = true;
            this.button_Replace.Click += new System.EventHandler(this.button_Replace_Click);
            // 
            // button_Leave
            // 
            resources.ApplyResources(this.button_Leave, "button_Leave");
            this.button_Leave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Leave.Name = "button_Leave";
            this.button_Leave.UseVisualStyleBackColor = true;
            this.button_Leave.Click += new System.EventHandler(this.button_Leave_Click);
            // 
            // button_LeaveAll
            // 
            resources.ApplyResources(this.button_LeaveAll, "button_LeaveAll");
            this.button_LeaveAll.Name = "button_LeaveAll";
            this.button_LeaveAll.UseVisualStyleBackColor = true;
            this.button_LeaveAll.Click += new System.EventHandler(this.button_LeaveAll_Click);
            // 
            // label_File
            // 
            resources.ApplyResources(this.label_File, "label_File");
            this.label_File.Name = "label_File";
            // 
            // DialogMessageExists
            // 
            this.AcceptButton = this.button_Replace;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Leave;
            this.Controls.Add(this.label_File);
            this.Controls.Add(this.button_LeaveAll);
            this.Controls.Add(this.button_Leave);
            this.Controls.Add(this.button_Replace);
            this.Controls.Add(this.button_ReplaceAll);
            this.Controls.Add(this.label_Overwrite);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogMessageExists";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_Overwrite;
        private System.Windows.Forms.Button button_ReplaceAll;
        private System.Windows.Forms.Button button_Replace;
        private System.Windows.Forms.Button button_Leave;
        private System.Windows.Forms.Button button_LeaveAll;
        private System.Windows.Forms.Label label_File;
    }
}