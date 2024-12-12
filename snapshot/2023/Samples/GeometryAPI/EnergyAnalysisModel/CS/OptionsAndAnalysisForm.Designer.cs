namespace Revit.SDK.Samples.EnergyAnalysisModel.CS
{
    partial class OptionsAndAnalysisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsAndAnalysisForm));
            this.treeViewAnalyticalData = new System.Windows.Forms.TreeView();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.comboBoxTier = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxExportMullions = new System.Windows.Forms.CheckBox();
            this.checkBoxSimplifyCurtainSystems = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeShadingSurfaces = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewAnalyticalData
            // 
            resources.ApplyResources(this.treeViewAnalyticalData, "treeViewAnalyticalData");
            this.treeViewAnalyticalData.Name = "treeViewAnalyticalData";
            // 
            // buttonRefresh
            // 
            resources.ApplyResources(this.buttonRefresh, "buttonRefresh");
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // comboBoxTier
            // 
            this.comboBoxTier.FormattingEnabled = true;
            this.comboBoxTier.Items.AddRange(new object[] {
            resources.GetString("comboBoxTier.Items"),
            resources.GetString("comboBoxTier.Items1"),
            resources.GetString("comboBoxTier.Items2"),
            resources.GetString("comboBoxTier.Items3")});
            resources.ApplyResources(this.comboBoxTier, "comboBoxTier");
            this.comboBoxTier.Name = "comboBoxTier";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // checkBoxExportMullions
            // 
            resources.ApplyResources(this.checkBoxExportMullions, "checkBoxExportMullions");
            this.checkBoxExportMullions.Name = "checkBoxExportMullions";
            this.checkBoxExportMullions.UseVisualStyleBackColor = true;
            // 
            // checkBoxSimplifyCurtainSystems
            // 
            resources.ApplyResources(this.checkBoxSimplifyCurtainSystems, "checkBoxSimplifyCurtainSystems");
            this.checkBoxSimplifyCurtainSystems.Name = "checkBoxSimplifyCurtainSystems";
            this.checkBoxSimplifyCurtainSystems.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeShadingSurfaces
            // 
            resources.ApplyResources(this.checkBoxIncludeShadingSurfaces, "checkBoxIncludeShadingSurfaces");
            this.checkBoxIncludeShadingSurfaces.Name = "checkBoxIncludeShadingSurfaces";
            this.checkBoxIncludeShadingSurfaces.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxExportMullions);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // OptionsAndAnalysisForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxSimplifyCurtainSystems);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxTier);
            this.Controls.Add(this.checkBoxIncludeShadingSurfaces);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.treeViewAnalyticalData);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsAndAnalysisForm";
            this.ShowIcon = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewAnalyticalData;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ComboBox comboBoxTier;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxExportMullions;
        private System.Windows.Forms.CheckBox checkBoxSimplifyCurtainSystems;
        private System.Windows.Forms.CheckBox checkBoxIncludeShadingSurfaces;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}