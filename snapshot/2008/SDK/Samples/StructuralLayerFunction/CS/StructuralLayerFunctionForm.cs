//
// (C) Copyright 2003-2007 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Revit.SDK.Samples.StructuralLayerFunction.CS
{
	/// <summary>
	/// display the function of each of a select floor's structural layers
	/// </summary>
	public class StructuralLayerFunctionForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox functionListBox;
		private System.Windows.Forms.GroupBox functionGroupBox;
		private System.Windows.Forms.Button okButton;

		private System.ComponentModel.Container components = null;


		/// <summary>
		/// Constructor of StructuralLayerFunctionForm
		/// </summary>
		/// <param name="dataBuffer">A reference of StructuralLayerFunction class</param>
		public StructuralLayerFunctionForm(Command dataBuffer)
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Set the data source of the ListBox control
			functionListBox.DataSource = dataBuffer.Functions;
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(null != components )
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.functionListBox = new System.Windows.Forms.ListBox();
            this.functionGroupBox = new System.Windows.Forms.GroupBox();
            this.okButton = new System.Windows.Forms.Button();
            this.functionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // functionListBox
            // 
            this.functionListBox.Location = new System.Drawing.Point(6, 24);
            this.functionListBox.Name = "functionListBox";
            this.functionListBox.Size = new System.Drawing.Size(189, 147);
            this.functionListBox.TabIndex = 0;
            // 
            // functionGroupBox
            // 
            this.functionGroupBox.Controls.Add(this.functionListBox);
            this.functionGroupBox.Location = new System.Drawing.Point(12, 12);
            this.functionGroupBox.Name = "functionGroupBox";
            this.functionGroupBox.Size = new System.Drawing.Size(201, 184);
            this.functionGroupBox.TabIndex = 1;
            this.functionGroupBox.TabStop = false;
            this.functionGroupBox.Text = "Layers Functions List";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(138, 202);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            // 
            // StructuralLayerFunctionForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(225, 236);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.functionGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StructuralLayerFunctionForm";
            this.ShowInTaskbar = false;
            this.Text = "Structure Layers Function";
            this.functionGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
	}
}
