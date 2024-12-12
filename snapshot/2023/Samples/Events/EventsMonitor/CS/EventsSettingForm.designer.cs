namespace Revit.SDK.Samples.EventsMonitor.CS
{
    partial class EventsSettingForm
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
         this.FinishToggle = new System.Windows.Forms.Button();
         this.checkAllButton = new System.Windows.Forms.Button();
         this.checkNoneButton = new System.Windows.Forms.Button();
         this.cancelButton = new System.Windows.Forms.Button();
         this.AppEventsCheckedList = new System.Windows.Forms.CheckedListBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.SuspendLayout();
         // 
         // FinishToggle
         // 
         this.FinishToggle.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.FinishToggle.Location = new System.Drawing.Point(232, 225);
         this.FinishToggle.Name = "FinishToggle";
         this.FinishToggle.Size = new System.Drawing.Size(75, 23);
         this.FinishToggle.TabIndex = 1;
         this.FinishToggle.Text = "&OK";
         this.FinishToggle.UseVisualStyleBackColor = true;
         this.FinishToggle.Click += new System.EventHandler(this.FinishToggle_Click);
         // 
         // checkAllButton
         // 
         this.checkAllButton.Location = new System.Drawing.Point(317, 7);
         this.checkAllButton.Name = "checkAllButton";
         this.checkAllButton.Size = new System.Drawing.Size(75, 23);
         this.checkAllButton.TabIndex = 3;
         this.checkAllButton.Text = "&Select All";
         this.checkAllButton.UseVisualStyleBackColor = true;
         this.checkAllButton.Click += new System.EventHandler(this.checkAllButton_Click);
         // 
         // checkNoneButton
         // 
         this.checkNoneButton.Location = new System.Drawing.Point(317, 47);
         this.checkNoneButton.Name = "checkNoneButton";
         this.checkNoneButton.Size = new System.Drawing.Size(75, 23);
         this.checkNoneButton.TabIndex = 4;
         this.checkNoneButton.Text = "&Unselect All";
         this.checkNoneButton.UseVisualStyleBackColor = true;
         this.checkNoneButton.Click += new System.EventHandler(this.checkNoneButton_Click);
         // 
         // cancelButton
         // 
         this.cancelButton.Location = new System.Drawing.Point(317, 225);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(75, 23);
         this.cancelButton.TabIndex = 4;
         this.cancelButton.Text = "&Cancel";
         this.cancelButton.UseVisualStyleBackColor = true;
         this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
         // 
         // AppEventsCheckedList
         // 
         this.AppEventsCheckedList.CheckOnClick = true;
         this.AppEventsCheckedList.FormattingEnabled = true;
         this.AppEventsCheckedList.Items.AddRange(new object[] {
            "DocumentCreating",
            "DocumentCreated",
            "DocumentOpening",
            "DocumentOpened",
            "DocumentClosing",
            "DocumentClosed",
            "DocumentSavingAs",
            "DocumentSavedAs",
            "DocumentSaving",
            "DocumentSaved",
            "FileExporting",
            "FileExported",
            "FileImporting",
            "FileImported",
            "DocumentPrinting",
            "DocumentPrinted",
            "ViewPrinting",
            "ViewPrinted",
            "ViewActivating",
            "ViewActivated",
            "DocumentSynchronizingWithCentral",
            "DocumentSynchronizedWithCentral",
            "ProgressChanged",
            "SelectionChanged"});
         this.AppEventsCheckedList.Location = new System.Drawing.Point(7, 7);
         this.AppEventsCheckedList.Name = "AppEventsCheckedList";
         this.AppEventsCheckedList.Size = new System.Drawing.Size(304, 199);
         this.AppEventsCheckedList.TabIndex = 2;
         // 
         // groupBox1
         // 
         this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.groupBox1.Location = new System.Drawing.Point(0, 218);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(406, 1);
         this.groupBox1.TabIndex = 5;
         this.groupBox1.TabStop = false;
         // 
         // EventsSettingForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(400, 254);
         this.Controls.Add(this.AppEventsCheckedList);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.checkAllButton);
         this.Controls.Add(this.FinishToggle);
         this.Controls.Add(this.checkNoneButton);
         this.Controls.Add(this.cancelButton);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "EventsSettingForm";
         this.ShowInTaskbar = false;
         this.Text = "Events Tracking Setting";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ToggleForm_FormClosed);
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FinishToggle;
        private System.Windows.Forms.Button checkAllButton;
        private System.Windows.Forms.Button checkNoneButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckedListBox AppEventsCheckedList;
        private System.Windows.Forms.GroupBox groupBox1;

    }
}