namespace SampleDesign
{
	partial class SimpleTorsors
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleTorsors));
			this.axSimpleTorsors = new AxASTCONTROLSLib.AxAstOCXGrid();
			((System.ComponentModel.ISupportInitialize)(this.axSimpleTorsors)).BeginInit();
			this.SuspendLayout();
			// 
			// axSimpleTorsors
			// 
			this.axSimpleTorsors.Enabled = true;
			this.axSimpleTorsors.Location = new System.Drawing.Point(3, 22);
			this.axSimpleTorsors.Name = "axSimpleTorsors";
			this.axSimpleTorsors.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSimpleTorsors.OcxState")));
			this.axSimpleTorsors.Size = new System.Drawing.Size(232, 70);
			this.axSimpleTorsors.TabIndex = 4;
			this.axSimpleTorsors.OnGridCellEdited += new AxASTCONTROLSLib._DAstOCXGridEvents_OnGridCellEditedEventHandler(this.axSimpleTorsors_OnGridCellEdited);
			// 
			// SimpleTorsors
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Controls.Add(this.axSimpleTorsors);
			this.Name = "SimpleTorsors";
			this.Size = new System.Drawing.Size(150, 150);
			((System.ComponentModel.ISupportInitialize)(this.axSimpleTorsors)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private AxASTCONTROLSLib.AxAstOCXGrid axSimpleTorsors;
	}
}
