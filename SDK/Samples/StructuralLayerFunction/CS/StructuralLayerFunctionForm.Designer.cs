namespace Revit.SDK.Samples.StructuralLayerFunction.CS
{
   public partial class StructuralLayerFunctionForm
   {
      private System.ComponentModel.Container components = null;

      private System.Windows.Forms.ListBox functionListBox;
      private System.Windows.Forms.GroupBox functionGroupBox;
      private System.Windows.Forms.Button okButton;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (null != components)
            {
               components.Dispose();
            }
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
