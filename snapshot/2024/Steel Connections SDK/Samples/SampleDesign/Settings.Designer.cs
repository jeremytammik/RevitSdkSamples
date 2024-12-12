namespace SampleDesign
{
	partial class Settings
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
			this.gbReport = new System.Windows.Forms.GroupBox();
			this.lbReportFormat = new System.Windows.Forms.Label();
			this.cbReportFormat = new System.Windows.Forms.ComboBox();
			this.gbReport.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbReport
			// 
			this.gbReport.Controls.Add(this.lbReportFormat);
			this.gbReport.Controls.Add(this.cbReportFormat);
			this.gbReport.Location = new System.Drawing.Point(3, 3);
			this.gbReport.Name = "gbReport";
			this.gbReport.Size = new System.Drawing.Size(325, 58);
			this.gbReport.TabIndex = 13;
			this.gbReport.TabStop = false;
			this.gbReport.Text = "Report";
			// 
			// lbReportFormat
			// 
			this.lbReportFormat.AutoSize = true;
			this.lbReportFormat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lbReportFormat.Location = new System.Drawing.Point(2, 17);
			this.lbReportFormat.Name = "lbReportFormat";
			this.lbReportFormat.Size = new System.Drawing.Size(39, 13);
			this.lbReportFormat.TabIndex = 0;
			this.lbReportFormat.Text = "Format";
			// 
			// cbReportFormat
			// 
			this.cbReportFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbReportFormat.FormattingEnabled = true;
			this.cbReportFormat.Items.AddRange(new object[] {
            "LRFD",
            "ASD"});
			this.cbReportFormat.Location = new System.Drawing.Point(196, 17);
			this.cbReportFormat.Name = "cbReportFormat";
			this.cbReportFormat.Size = new System.Drawing.Size(121, 21);
			this.cbReportFormat.TabIndex = 0;
			this.cbReportFormat.SelectedIndexChanged += new System.EventHandler(this.cbReportFormat_SelectedIndexChanged);
			// 
			// Settings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Controls.Add(this.gbReport);
			this.Name = "Settings";
			this.Size = new System.Drawing.Size(347, 285);
			this.gbReport.ResumeLayout(false);
			this.gbReport.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbReport;
		private System.Windows.Forms.Label lbReportFormat;
		private System.Windows.Forms.ComboBox cbReportFormat;
	}
}
