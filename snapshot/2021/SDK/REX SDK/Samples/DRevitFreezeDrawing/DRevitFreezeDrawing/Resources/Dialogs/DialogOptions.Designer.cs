namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
    partial class DialogOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogOptions));
            this.group_Colors = new System.Windows.Forms.GroupBox();
            this.radio_Invert = new System.Windows.Forms.RadioButton();
            this.radio_Preserve = new System.Windows.Forms.RadioButton();
            this.radio_BandW = new System.Windows.Forms.RadioButton();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.Button_OK = new System.Windows.Forms.Button();
            this.group_Export = new System.Windows.Forms.GroupBox();
            this.button_ExportOptions = new System.Windows.Forms.Button();
            this.group_Copy = new System.Windows.Forms.GroupBox();
            this.check_CopyDwg = new System.Windows.Forms.CheckBox();
            this.button_Browse = new System.Windows.Forms.Button();
            this.textBox_Browse = new System.Windows.Forms.TextBox();
            this.label_Browse = new System.Windows.Forms.Label();
            this.combo_Version = new System.Windows.Forms.ComboBox();
            this.label_Version = new System.Windows.Forms.Label();
            this.text_DwgBaseName = new System.Windows.Forms.TextBox();
            this.label_DwgBaseName = new System.Windows.Forms.Label();
            this.folderBrowserDialogDwg = new System.Windows.Forms.FolderBrowserDialog();
            this.Help = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.check_Delete = new System.Windows.Forms.CheckBox();
            this.group_Colors.SuspendLayout();
            this.group_Export.SuspendLayout();
            this.group_Copy.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // group_Colors
            // 
            this.group_Colors.Controls.Add(this.radio_Invert);
            this.group_Colors.Controls.Add(this.radio_Preserve);
            this.group_Colors.Controls.Add(this.radio_BandW);
            resources.ApplyResources(this.group_Colors, "group_Colors");
            this.group_Colors.Name = "group_Colors";
            this.group_Colors.TabStop = false;
            // 
            // radio_Invert
            // 
            resources.ApplyResources(this.radio_Invert, "radio_Invert");
            this.radio_Invert.Name = "radio_Invert";
            this.radio_Invert.TabStop = true;
            this.radio_Invert.UseVisualStyleBackColor = true;
            // 
            // radio_Preserve
            // 
            resources.ApplyResources(this.radio_Preserve, "radio_Preserve");
            this.radio_Preserve.Name = "radio_Preserve";
            this.radio_Preserve.TabStop = true;
            this.radio_Preserve.UseVisualStyleBackColor = true;
            // 
            // radio_BandW
            // 
            resources.ApplyResources(this.radio_BandW, "radio_BandW");
            this.radio_BandW.Name = "radio_BandW";
            this.radio_BandW.TabStop = true;
            this.radio_BandW.UseVisualStyleBackColor = true;
            // 
            // Button_Cancel
            // 
            resources.ApplyResources(this.Button_Cancel, "Button_Cancel");
            this.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            this.Button_Cancel.Click += new System.EventHandler(this.Button_Cancel_Click);
            // 
            // Button_OK
            // 
            resources.ApplyResources(this.Button_OK, "Button_OK");
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // group_Export
            // 
            this.group_Export.Controls.Add(this.button_ExportOptions);
            resources.ApplyResources(this.group_Export, "group_Export");
            this.group_Export.Name = "group_Export";
            this.group_Export.TabStop = false;
            // 
            // button_ExportOptions
            // 
            resources.ApplyResources(this.button_ExportOptions, "button_ExportOptions");
            this.button_ExportOptions.Name = "button_ExportOptions";
            this.button_ExportOptions.UseVisualStyleBackColor = true;
            this.button_ExportOptions.Click += new System.EventHandler(this.button_ExportOptions_Click);
            // 
            // group_Copy
            // 
            this.group_Copy.Controls.Add(this.check_CopyDwg);
            this.group_Copy.Controls.Add(this.button_Browse);
            this.group_Copy.Controls.Add(this.textBox_Browse);
            this.group_Copy.Controls.Add(this.label_Browse);
            this.group_Copy.Controls.Add(this.combo_Version);
            this.group_Copy.Controls.Add(this.label_Version);
            this.group_Copy.Controls.Add(this.text_DwgBaseName);
            this.group_Copy.Controls.Add(this.label_DwgBaseName);
            resources.ApplyResources(this.group_Copy, "group_Copy");
            this.group_Copy.Name = "group_Copy";
            this.group_Copy.TabStop = false;
            // 
            // check_CopyDwg
            // 
            resources.ApplyResources(this.check_CopyDwg, "check_CopyDwg");
            this.check_CopyDwg.Name = "check_CopyDwg";
            this.check_CopyDwg.UseVisualStyleBackColor = true;
            this.check_CopyDwg.CheckedChanged += new System.EventHandler(this.check_CopyDwg_CheckedChanged);
            // 
            // button_Browse
            // 
            resources.ApplyResources(this.button_Browse, "button_Browse");
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // textBox_Browse
            // 
            resources.ApplyResources(this.textBox_Browse, "textBox_Browse");
            this.textBox_Browse.Name = "textBox_Browse";
            this.textBox_Browse.TextChanged += new System.EventHandler(this.textBox_Browse_TextChanged);
            // 
            // label_Browse
            // 
            resources.ApplyResources(this.label_Browse, "label_Browse");
            this.label_Browse.Name = "label_Browse";
            // 
            // combo_Version
            // 
            this.combo_Version.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Version.FormattingEnabled = true;
            resources.ApplyResources(this.combo_Version, "combo_Version");
            this.combo_Version.Name = "combo_Version";
            this.combo_Version.SelectedIndexChanged += new System.EventHandler(this.combo_Version_SelectedIndexChanged);
            // 
            // label_Version
            // 
            resources.ApplyResources(this.label_Version, "label_Version");
            this.label_Version.Name = "label_Version";
            // 
            // text_DwgBaseName
            // 
            resources.ApplyResources(this.text_DwgBaseName, "text_DwgBaseName");
            this.text_DwgBaseName.Name = "text_DwgBaseName";
            this.text_DwgBaseName.TextChanged += new System.EventHandler(this.text_DwgBaseName_TextChanged);
            // 
            // label_DwgBaseName
            // 
            resources.ApplyResources(this.label_DwgBaseName, "label_DwgBaseName");
            this.label_DwgBaseName.Name = "label_DwgBaseName";
            // 
            // Help
            // 
            resources.ApplyResources(this.Help, "Help");
            this.Help.Name = "Help";
            this.Help.TabStop = true;
            this.Help.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Help_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.check_Delete);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // check_Delete
            // 
            resources.ApplyResources(this.check_Delete, "check_Delete");
            this.check_Delete.Name = "check_Delete";
            this.check_Delete.UseVisualStyleBackColor = true;
            // 
            // DialogOptions
            // 
            this.AcceptButton = this.Button_OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Button_Cancel;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Help);
            this.Controls.Add(this.group_Copy);
            this.Controls.Add(this.group_Export);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.group_Colors);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogOptions";
            this.Load += new System.EventHandler(this.DialogOptions_Load);
            this.group_Colors.ResumeLayout(false);
            this.group_Colors.PerformLayout();
            this.group_Export.ResumeLayout(false);
            this.group_Copy.ResumeLayout(false);
            this.group_Copy.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox group_Colors;
        private System.Windows.Forms.RadioButton radio_Invert;
        private System.Windows.Forms.RadioButton radio_Preserve;
        private System.Windows.Forms.RadioButton radio_BandW;
        private System.Windows.Forms.Button Button_Cancel;
        private System.Windows.Forms.Button Button_OK;
        private System.Windows.Forms.GroupBox group_Export;
        private System.Windows.Forms.GroupBox group_Copy;
        private System.Windows.Forms.Button button_Browse;
        private System.Windows.Forms.TextBox textBox_Browse;
        private System.Windows.Forms.Label label_Browse;
        private System.Windows.Forms.ComboBox combo_Version;
        private System.Windows.Forms.Label label_Version;
        private System.Windows.Forms.TextBox text_DwgBaseName;
        private System.Windows.Forms.Label label_DwgBaseName;
        private System.Windows.Forms.CheckBox check_CopyDwg;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogDwg;
        private System.Windows.Forms.Button button_ExportOptions;
        private System.Windows.Forms.LinkLabel Help;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox check_Delete;
    }
}