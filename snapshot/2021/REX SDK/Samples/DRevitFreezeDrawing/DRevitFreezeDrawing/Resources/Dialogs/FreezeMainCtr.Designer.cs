namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
    partial class FreezeMainCtr
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FreezeMainCtr));
            this.group_Range = new System.Windows.Forms.GroupBox();
            this.button_SelectV = new System.Windows.Forms.Button();
            this.radio_SelectedV = new System.Windows.Forms.RadioButton();
            this.radio_CurrentV = new System.Windows.Forms.RadioButton();
            this.group_Range.SuspendLayout();
            this.SuspendLayout();
            // 
            // group_Range
            // 
            this.group_Range.Controls.Add(this.button_SelectV);
            this.group_Range.Controls.Add(this.radio_SelectedV);
            this.group_Range.Controls.Add(this.radio_CurrentV);
            resources.ApplyResources(this.group_Range, "group_Range");
            this.group_Range.Name = "group_Range";
            this.group_Range.TabStop = false;
            this.group_Range.Enter += new System.EventHandler(this.group_Range_Enter);
            // 
            // button_SelectV
            // 
            resources.ApplyResources(this.button_SelectV, "button_SelectV");
            this.button_SelectV.Name = "button_SelectV";
            this.button_SelectV.UseVisualStyleBackColor = true;
            this.button_SelectV.Click += new System.EventHandler(this.button_SelectV_Click);
            // 
            // radio_SelectedV
            // 
            resources.ApplyResources(this.radio_SelectedV, "radio_SelectedV");
            this.radio_SelectedV.Name = "radio_SelectedV";
            this.radio_SelectedV.TabStop = true;
            this.radio_SelectedV.UseVisualStyleBackColor = true;
            this.radio_SelectedV.CheckedChanged += new System.EventHandler(this.radio_SelectedV_CheckedChanged);
            // 
            // radio_CurrentV
            // 
            resources.ApplyResources(this.radio_CurrentV, "radio_CurrentV");
            this.radio_CurrentV.Name = "radio_CurrentV";
            this.radio_CurrentV.TabStop = true;
            this.radio_CurrentV.UseVisualStyleBackColor = true;
            this.radio_CurrentV.CheckedChanged += new System.EventHandler(this.radio_CurrentV_CheckedChanged);
            // 
            // FreezeMainCtr
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.group_Range);
            this.Name = "FreezeMainCtr";
            this.Load += new System.EventHandler(this.FreezeMainCtr_Load);
            this.group_Range.ResumeLayout(false);
            this.group_Range.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox group_Range;
        private System.Windows.Forms.Button button_SelectV;
        private System.Windows.Forms.RadioButton radio_SelectedV;
        private System.Windows.Forms.RadioButton radio_CurrentV;
    }
}
