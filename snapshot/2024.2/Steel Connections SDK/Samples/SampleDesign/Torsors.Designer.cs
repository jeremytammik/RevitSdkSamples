namespace SampleDesign
{
	partial class Torsors
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Torsors));
			this.btnDelLoading = new System.Windows.Forms.Button();
			this.btnAddLoading = new System.Windows.Forms.Button();
			this.axTorsors = new AxASTCONTROLSLib.AxAstOCXGrid();
			((System.ComponentModel.ISupportInitialize)(this.axTorsors)).BeginInit();
			this.SuspendLayout();
			// 
			// btnDelLoading
			// 
			this.btnDelLoading.Image = ((System.Drawing.Image)(resources.GetObject("btnDelLoading.Image")));
			this.btnDelLoading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnDelLoading.Location = new System.Drawing.Point(479, 249);
			this.btnDelLoading.Name = "btnDelLoading";
			this.btnDelLoading.Size = new System.Drawing.Size(24, 23);
			this.btnDelLoading.TabIndex = 10;
			this.btnDelLoading.UseVisualStyleBackColor = true;
			this.btnDelLoading.ClientSizeChanged += new System.EventHandler(this.btnDelLoading_Click);
			// 
			// btnAddLoading
			// 
			this.btnAddLoading.Image = ((System.Drawing.Image)(resources.GetObject("btnAddLoading.Image")));
			this.btnAddLoading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnAddLoading.Location = new System.Drawing.Point(451, 249);
			this.btnAddLoading.Name = "btnAddLoading";
			this.btnAddLoading.Size = new System.Drawing.Size(24, 23);
			this.btnAddLoading.TabIndex = 9;
			this.btnAddLoading.UseVisualStyleBackColor = true;
			this.btnAddLoading.Click += new System.EventHandler(this.btnAddLoading_Click);
			// 
			// axTorsors
			// 
			this.axTorsors.Enabled = true;
			this.axTorsors.Location = new System.Drawing.Point(3, 3);
			this.axTorsors.Name = "axTorsors";
			this.axTorsors.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTorsors.OcxState")));
			this.axTorsors.Size = new System.Drawing.Size(499, 116);
			this.axTorsors.TabIndex = 8;
			this.axTorsors.OnGridCellEdited += new AxASTCONTROLSLib._DAstOCXGridEvents_OnGridCellEditedEventHandler(this.axTorsors_OnGridCellEdited);
			// 
			// Torsors
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Controls.Add(this.btnDelLoading);
			this.Controls.Add(this.btnAddLoading);
			this.Controls.Add(this.axTorsors);
			this.Name = "Torsors";
			this.Size = new System.Drawing.Size(150, 150);
			((System.ComponentModel.ISupportInitialize)(this.axTorsors)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnDelLoading;
		private System.Windows.Forms.Button btnAddLoading;
		private AxASTCONTROLSLib.AxAstOCXGrid axTorsors;
	}
}
