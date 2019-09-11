namespace Revit.SDK.Samples.MeasurePanelArea.CS
{
    partial class frmPanelArea
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
           this.btnCompute = new System.Windows.Forms.Button();
           this.groupBox1 = new System.Windows.Forms.GroupBox();
           this.label2 = new System.Windows.Forms.Label();
           this.label1 = new System.Windows.Forms.Label();
           this.txtMax = new System.Windows.Forms.TextBox();
           this.txtMin = new System.Windows.Forms.TextBox();
           this.groupBox2 = new System.Windows.Forms.GroupBox();
           this.label4 = new System.Windows.Forms.Label();
           this.label3 = new System.Windows.Forms.Label();
           this.cboxMin = new System.Windows.Forms.ComboBox();
           this.cboxMax = new System.Windows.Forms.ComboBox();
           this.cboxMid = new System.Windows.Forms.ComboBox();
           this.label5 = new System.Windows.Forms.Label();
           this.label6 = new System.Windows.Forms.Label();
           this.groupBox1.SuspendLayout();
           this.groupBox2.SuspendLayout();
           this.SuspendLayout();
           // 
           // btnCompute
           // 
           this.btnCompute.Location = new System.Drawing.Point(263, 274);
           this.btnCompute.Name = "btnCompute";
           this.btnCompute.Size = new System.Drawing.Size(75, 23);
           this.btnCompute.TabIndex = 5;
           this.btnCompute.Text = "&Compute";
           this.btnCompute.UseVisualStyleBackColor = true;
           this.btnCompute.Click += new System.EventHandler(this.btnCompute_Click);
           // 
           // groupBox1
           // 
           this.groupBox1.Controls.Add(this.label2);
           this.groupBox1.Controls.Add(this.label1);
           this.groupBox1.Controls.Add(this.txtMax);
           this.groupBox1.Controls.Add(this.txtMin);
           this.groupBox1.Location = new System.Drawing.Point(8, 48);
           this.groupBox1.Name = "groupBox1";
           this.groupBox1.Size = new System.Drawing.Size(330, 59);
           this.groupBox1.TabIndex = 10;
           this.groupBox1.TabStop = false;
           this.groupBox1.Text = "Desired Panel Area";
           // 
           // label2
           // 
           this.label2.AutoSize = true;
           this.label2.Location = new System.Drawing.Point(219, 25);
           this.label2.Name = "label2";
           this.label2.Size = new System.Drawing.Size(51, 13);
           this.label2.TabIndex = 8;
           this.label2.Text = "Maximum";
           // 
           // label1
           // 
           this.label1.AutoSize = true;
           this.label1.Location = new System.Drawing.Point(10, 25);
           this.label1.Name = "label1";
           this.label1.Size = new System.Drawing.Size(48, 13);
           this.label1.TabIndex = 7;
           this.label1.Text = "Minimum";
           // 
           // txtMax
           // 
           this.txtMax.Location = new System.Drawing.Point(287, 22);
           this.txtMax.Name = "txtMax";
           this.txtMax.Size = new System.Drawing.Size(32, 20);
           this.txtMax.TabIndex = 6;
           this.txtMax.Text = "102";
           // 
           // txtMin
           // 
           this.txtMin.Location = new System.Drawing.Point(75, 22);
           this.txtMin.Name = "txtMin";
           this.txtMin.Size = new System.Drawing.Size(32, 20);
           this.txtMin.TabIndex = 5;
           this.txtMin.Text = "98";
           // 
           // groupBox2
           // 
           this.groupBox2.Controls.Add(this.label4);
           this.groupBox2.Controls.Add(this.label3);
           this.groupBox2.Controls.Add(this.cboxMin);
           this.groupBox2.Controls.Add(this.cboxMax);
           this.groupBox2.Location = new System.Drawing.Point(8, 122);
           this.groupBox2.Name = "groupBox2";
           this.groupBox2.Size = new System.Drawing.Size(330, 100);
           this.groupBox2.TabIndex = 11;
           this.groupBox2.TabStop = false;
           this.groupBox2.Text = "Panels Outside Desired Range";
           // 
           // label4
           // 
           this.label4.AutoSize = true;
           this.label4.Location = new System.Drawing.Point(8, 65);
           this.label4.Name = "label4";
           this.label4.Size = new System.Drawing.Size(107, 13);
           this.label4.TabIndex = 13;
           this.label4.Text = "Larger than maximum";
           // 
           // label3
           // 
           this.label3.AutoSize = true;
           this.label3.Location = new System.Drawing.Point(8, 34);
           this.label3.Name = "label3";
           this.label3.Size = new System.Drawing.Size(108, 13);
           this.label3.TabIndex = 12;
           this.label3.Text = "Smaller than minimum";
           // 
           // cboxMin
           // 
           this.cboxMin.FormattingEnabled = true;
           this.cboxMin.Location = new System.Drawing.Point(126, 29);
           this.cboxMin.Name = "cboxMin";
           this.cboxMin.Size = new System.Drawing.Size(197, 21);
           this.cboxMin.TabIndex = 11;
           // 
           // cboxMax
           // 
           this.cboxMax.FormattingEnabled = true;
           this.cboxMax.Location = new System.Drawing.Point(126, 62);
           this.cboxMax.Name = "cboxMax";
           this.cboxMax.Size = new System.Drawing.Size(197, 21);
           this.cboxMax.TabIndex = 10;
           // 
           // cboxMid
           // 
           this.cboxMid.FormattingEnabled = true;
           this.cboxMid.Location = new System.Drawing.Point(134, 237);
           this.cboxMid.Name = "cboxMid";
           this.cboxMid.Size = new System.Drawing.Size(197, 21);
           this.cboxMid.TabIndex = 12;
           // 
           // label5
           // 
           this.label5.AutoSize = true;
           this.label5.Location = new System.Drawing.Point(20, 240);
           this.label5.Name = "label5";
           this.label5.Size = new System.Drawing.Size(82, 13);
           this.label5.TabIndex = 14;
           this.label5.Text = "All Other Panels";
           // 
           // label6
           // 
           this.label6.AutoSize = true;
           this.label6.Location = new System.Drawing.Point(5, 9);
           this.label6.Name = "label6";
           this.label6.Size = new System.Drawing.Size(310, 26);
           this.label6.TabIndex = 9;
           this.label6.Text = "Select divided surfaces to analyze before running this command.\nRun this command " +
               "with nothing selected to analyze all surfaces.";
           // 
           // frmPanelArea
           // 
           this.AcceptButton = this.btnCompute;
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.ClientSize = new System.Drawing.Size(351, 309);
           this.Controls.Add(this.label6);
           this.Controls.Add(this.label5);
           this.Controls.Add(this.cboxMid);
           this.Controls.Add(this.groupBox2);
           this.Controls.Add(this.groupBox1);
           this.Controls.Add(this.btnCompute);
           this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
           this.MaximizeBox = false;
           this.MinimizeBox = false;
           this.Name = "frmPanelArea";
           this.ShowIcon = false;
           this.ShowInTaskbar = false;
           this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
           this.Text = "Check Panel Area";
           this.groupBox1.ResumeLayout(false);
           this.groupBox1.PerformLayout();
           this.groupBox2.ResumeLayout(false);
           this.groupBox2.PerformLayout();
           this.ResumeLayout(false);
           this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCompute;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMax;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboxMin;
        private System.Windows.Forms.ComboBox cboxMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboxMid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

