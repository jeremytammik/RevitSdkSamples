namespace Revit.SDK.Samples.RotateFramingObjects.CS
{
   public partial class RotateFramingObjectsForm
   {
      private System.ComponentModel.Container m_components = null;

      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.Button okButton;
      public System.Windows.Forms.RadioButton absoluteRadio;
      private System.Windows.Forms.RadioButton relativeRadio;
      private System.Windows.Forms.Label rotationLabel;
      public System.Windows.Forms.TextBox rotationTextBox;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (m_components != null)
            {
               m_components.Dispose();
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
         this.absoluteRadio = new System.Windows.Forms.RadioButton();
         this.relativeRadio = new System.Windows.Forms.RadioButton();
         this.cancelButton = new System.Windows.Forms.Button();
         this.okButton = new System.Windows.Forms.Button();
         this.rotationLabel = new System.Windows.Forms.Label();
         this.rotationTextBox = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // absoluteRadio
         // 
         this.absoluteRadio.Location = new System.Drawing.Point(85, 35);
         this.absoluteRadio.Name = "absoluteRadio";
         this.absoluteRadio.Size = new System.Drawing.Size(72, 24);
         this.absoluteRadio.TabIndex = 0;
         this.absoluteRadio.Text = "Absolute";
         this.absoluteRadio.CheckedChanged += new System.EventHandler(this.allRadio_CheckedChanged);
         // 
         // relativeRadio
         // 
         this.relativeRadio.Checked = true;
         this.relativeRadio.Location = new System.Drawing.Point(15, 35);
         this.relativeRadio.Name = "relativeRadio";
         this.relativeRadio.Size = new System.Drawing.Size(64, 24);
         this.relativeRadio.TabIndex = 1;
         this.relativeRadio.TabStop = true;
         this.relativeRadio.Text = "Relative";
         this.relativeRadio.CheckedChanged += new System.EventHandler(this.singleRadio_CheckedChanged);
         // 
         // cancelButton
         // 
         this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cancelButton.Location = new System.Drawing.Point(93, 65);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(75, 23);
         this.cancelButton.TabIndex = 6;
         this.cancelButton.Text = "&Cancel";
         this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
         // 
         // okButton
         // 
         this.okButton.Location = new System.Drawing.Point(15, 65);
         this.okButton.Name = "okButton";
         this.okButton.Size = new System.Drawing.Size(75, 23);
         this.okButton.TabIndex = 8;
         this.okButton.Text = "&OK";
         this.okButton.Click += new System.EventHandler(this.okButton_Click);
         // 
         // rotationLabel
         // 
         this.rotationLabel.Location = new System.Drawing.Point(12, 12);
         this.rotationLabel.Name = "rotationLabel";
         this.rotationLabel.Size = new System.Drawing.Size(50, 16);
         this.rotationLabel.TabIndex = 10;
         this.rotationLabel.Text = "Rotation";
         // 
         // rotationTextBox
         // 
         this.rotationTextBox.Location = new System.Drawing.Point(68, 9);
         this.rotationTextBox.Name = "rotationTextBox";
         this.rotationTextBox.Size = new System.Drawing.Size(100, 20);
         this.rotationTextBox.TabIndex = 1;
         this.rotationTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rotationTextBox_KeyPress);
         this.rotationTextBox.TextChanged += new System.EventHandler(this.rotationTextBox_TextChanged);
         // 
         // RotateFramingObjectsForm
         // 
         this.AcceptButton = this.okButton;
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.CancelButton = this.cancelButton;
         this.ClientSize = new System.Drawing.Size(185, 101);
         this.Controls.Add(this.rotationTextBox);
         this.Controls.Add(this.relativeRadio);
         this.Controls.Add(this.rotationLabel);
         this.Controls.Add(this.okButton);
         this.Controls.Add(this.cancelButton);
         this.Controls.Add(this.absoluteRadio);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "RotateFramingObjectsForm";
         this.ShowInTaskbar = false;
         this.Text = "Rotate Framing Objects";
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion
   }
}
