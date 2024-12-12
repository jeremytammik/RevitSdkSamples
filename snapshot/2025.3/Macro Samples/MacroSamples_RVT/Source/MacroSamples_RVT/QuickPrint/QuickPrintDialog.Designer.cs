namespace Revit.SDK.Samples.QuickPrint.CS
{
    partial class QuickPrintDialog
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
            this.PlanType = new System.Windows.Forms.CheckBox();
            this.ElevationType = new System.Windows.Forms.CheckBox();
            this.SectionType = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PlanType
            // 
            this.PlanType.AutoSize = true;
            this.PlanType.Location = new System.Drawing.Point(40, 30);
            this.PlanType.Name = "PlanType";
            this.PlanType.Size = new System.Drawing.Size(75, 17);
            this.PlanType.TabIndex = 0;
            this.PlanType.Text = "FloorPlans";
            this.PlanType.UseVisualStyleBackColor = true;
            // 
            // ElevationType
            // 
            this.ElevationType.AutoSize = true;
            this.ElevationType.Location = new System.Drawing.Point(40, 53);
            this.ElevationType.Name = "ElevationType";
            this.ElevationType.Size = new System.Drawing.Size(75, 17);
            this.ElevationType.TabIndex = 1;
            this.ElevationType.Text = "Elevations";
            this.ElevationType.UseVisualStyleBackColor = true;
            // 
            // SectionType
            // 
            this.SectionType.AutoSize = true;
            this.SectionType.Location = new System.Drawing.Point(40, 76);
            this.SectionType.Name = "SectionType";
            this.SectionType.Size = new System.Drawing.Size(67, 17);
            this.SectionType.TabIndex = 2;
            this.SectionType.Text = "Sections";
            this.SectionType.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PlanType);
            this.groupBox1.Controls.Add(this.SectionType);
            this.groupBox1.Controls.Add(this.ElevationType);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 114);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose View Type";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(42, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(136, 144);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // QuickPrintDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 180);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "QuickPrintDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QuickPrint";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.CheckBox PlanType;
        public System.Windows.Forms.CheckBox ElevationType;
        public System.Windows.Forms.CheckBox SectionType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

    }
}