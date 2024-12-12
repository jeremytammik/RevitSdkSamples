namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
    partial class DialogExportOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogExportOptions));
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.combo_layMap = new System.Windows.Forms.ComboBox();
            this.label_layMap = new System.Windows.Forms.Label();
            this.check_exportRooms = new System.Windows.Forms.CheckBox();
            this.combo_units = new System.Windows.Forms.ComboBox();
            this.label_units = new System.Windows.Forms.Label();
            this.combo_Coord = new System.Windows.Forms.ComboBox();
            this.label_Coord = new System.Windows.Forms.Label();
            this.combo_lineScale = new System.Windows.Forms.ComboBox();
            this.label_lineScale = new System.Windows.Forms.Label();
            this.combo_layAndProp = new System.Windows.Forms.ComboBox();
            this.label_layAndProp = new System.Windows.Forms.Label();
            this.Help = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            resources.ApplyResources(this.button_Cancel, "button_Cancel");
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_OK
            // 
            resources.ApplyResources(this.button_OK, "button_OK");
            this.button_OK.Name = "button_OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.combo_layMap);
            this.groupBox1.Controls.Add(this.label_layMap);
            this.groupBox1.Controls.Add(this.check_exportRooms);
            this.groupBox1.Controls.Add(this.combo_units);
            this.groupBox1.Controls.Add(this.label_units);
            this.groupBox1.Controls.Add(this.combo_Coord);
            this.groupBox1.Controls.Add(this.label_Coord);
            this.groupBox1.Controls.Add(this.combo_lineScale);
            this.groupBox1.Controls.Add(this.label_lineScale);
            this.groupBox1.Controls.Add(this.combo_layAndProp);
            this.groupBox1.Controls.Add(this.label_layAndProp);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // combo_layMap
            // 
            this.combo_layMap.FormattingEnabled = true;
            resources.ApplyResources(this.combo_layMap, "combo_layMap");
            this.combo_layMap.Name = "combo_layMap";
            // 
            // label_layMap
            // 
            resources.ApplyResources(this.label_layMap, "label_layMap");
            this.label_layMap.Name = "label_layMap";
            // 
            // check_exportRooms
            // 
            resources.ApplyResources(this.check_exportRooms, "check_exportRooms");
            this.check_exportRooms.Name = "check_exportRooms";
            this.check_exportRooms.UseVisualStyleBackColor = true;
            // 
            // combo_units
            // 
            this.combo_units.FormattingEnabled = true;
            resources.ApplyResources(this.combo_units, "combo_units");
            this.combo_units.Name = "combo_units";
            // 
            // label_units
            // 
            resources.ApplyResources(this.label_units, "label_units");
            this.label_units.Name = "label_units";
            // 
            // combo_Coord
            // 
            this.combo_Coord.FormattingEnabled = true;
            resources.ApplyResources(this.combo_Coord, "combo_Coord");
            this.combo_Coord.Name = "combo_Coord";
            // 
            // label_Coord
            // 
            resources.ApplyResources(this.label_Coord, "label_Coord");
            this.label_Coord.Name = "label_Coord";
            // 
            // combo_lineScale
            // 
            this.combo_lineScale.FormattingEnabled = true;
            resources.ApplyResources(this.combo_lineScale, "combo_lineScale");
            this.combo_lineScale.Name = "combo_lineScale";
            // 
            // label_lineScale
            // 
            resources.ApplyResources(this.label_lineScale, "label_lineScale");
            this.label_lineScale.Name = "label_lineScale";
            // 
            // combo_layAndProp
            // 
            this.combo_layAndProp.FormattingEnabled = true;
            resources.ApplyResources(this.combo_layAndProp, "combo_layAndProp");
            this.combo_layAndProp.Name = "combo_layAndProp";
            // 
            // label_layAndProp
            // 
            resources.ApplyResources(this.label_layAndProp, "label_layAndProp");
            this.label_layAndProp.Name = "label_layAndProp";
            // 
            // Help
            // 
            resources.ApplyResources(this.Help, "Help");
            this.Help.Name = "Help";
            this.Help.TabStop = true;
            this.Help.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Help_LinkClicked);
            // 
            // DialogExportOptions
            // 
            this.AcceptButton = this.button_OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.Controls.Add(this.Help);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogExportOptions";
            this.Load += new System.EventHandler(this.DialogExportOptions_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox combo_layMap;
        private System.Windows.Forms.Label label_layMap;
        private System.Windows.Forms.CheckBox check_exportRooms;
        private System.Windows.Forms.ComboBox combo_units;
        private System.Windows.Forms.Label label_units;
        private System.Windows.Forms.ComboBox combo_Coord;
        private System.Windows.Forms.Label label_Coord;
        private System.Windows.Forms.ComboBox combo_lineScale;
        private System.Windows.Forms.Label label_lineScale;
        private System.Windows.Forms.ComboBox combo_layAndProp;
        private System.Windows.Forms.Label label_layAndProp;
        private System.Windows.Forms.LinkLabel Help;
    }
}