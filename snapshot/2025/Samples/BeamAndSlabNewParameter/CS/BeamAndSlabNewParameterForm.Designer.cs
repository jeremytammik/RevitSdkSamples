namespace Revit.SDK.Samples.BeamAndSlabNewParameter.CS
{
   public partial class BeamAndSlabParametersForm
   {
      private System.Windows.Forms.Button addParameterButton;
      private System.Windows.Forms.Button displayValueButton;
      private System.Windows.Forms.Button exitButton;
      private System.Windows.Forms.ListBox attributeValueListBox;
      private System.Windows.Forms.Label attributeValueLabel;
      private System.Windows.Forms.Button findButton;
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.Container components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (components != null)
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
         this.addParameterButton = new System.Windows.Forms.Button();
         this.displayValueButton = new System.Windows.Forms.Button();
         this.exitButton = new System.Windows.Forms.Button();
         this.attributeValueListBox = new System.Windows.Forms.ListBox();
         this.attributeValueLabel = new System.Windows.Forms.Label();
         this.findButton = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // addParameterButton
         // 
         this.addParameterButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.addParameterButton.Location = new System.Drawing.Point(311, 65);
         this.addParameterButton.Name = "addParameterButton";
         this.addParameterButton.Size = new System.Drawing.Size(105, 26);
         this.addParameterButton.TabIndex = 1;
         this.addParameterButton.Text = "&Add";
         this.addParameterButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
         this.addParameterButton.Click += new System.EventHandler(this.addParameterButton_Click);
         // 
         // displayValueButton
         // 
         this.displayValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.displayValueButton.Location = new System.Drawing.Point(311, 111);
         this.displayValueButton.Name = "displayValueButton";
         this.displayValueButton.Size = new System.Drawing.Size(105, 26);
         this.displayValueButton.TabIndex = 2;
         this.displayValueButton.Text = "&Display Value";
         this.displayValueButton.Click += new System.EventHandler(this.displayValueButton_Click);
         // 
         // exitButton
         // 
         this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.exitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.exitButton.Location = new System.Drawing.Point(311, 203);
         this.exitButton.Name = "exitButton";
         this.exitButton.Size = new System.Drawing.Size(105, 26);
         this.exitButton.TabIndex = 4;
         this.exitButton.Text = "&Exit";
         this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
         // 
         // attributeValueListBox
         // 
         this.attributeValueListBox.ItemHeight = 16;
         this.attributeValueListBox.Location = new System.Drawing.Point(19, 46);
         this.attributeValueListBox.Name = "attributeValueListBox";
         this.attributeValueListBox.Size = new System.Drawing.Size(269, 228);
         this.attributeValueListBox.TabIndex = 18;
         this.attributeValueListBox.TabStop = false;
         // 
         // attributeValueLabel
         // 
         this.attributeValueLabel.Location = new System.Drawing.Point(19, 9);
         this.attributeValueLabel.Name = "attributeValueLabel";
         this.attributeValueLabel.Size = new System.Drawing.Size(279, 37);
         this.attributeValueLabel.TabIndex = 19;
         this.attributeValueLabel.Text = "Display the value of the Unique ID if present for all the selected elements";
         // 
         // findButton
         // 
         this.findButton.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.findButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.findButton.Location = new System.Drawing.Point(311, 157);
         this.findButton.Name = "findButton";
         this.findButton.Size = new System.Drawing.Size(105, 26);
         this.findButton.TabIndex = 3;
         this.findButton.Text = "&Find";
         this.findButton.Click += new System.EventHandler(this.findButton_Click);
         // 
         // BeamAndSlabParametersForm
         // 
         this.CancelButton = this.exitButton;
         this.ClientSize = new System.Drawing.Size(438, 292);
         this.Controls.Add(this.attributeValueLabel);
         this.Controls.Add(this.attributeValueListBox);
         this.Controls.Add(this.addParameterButton);
         this.Controls.Add(this.findButton);
         this.Controls.Add(this.displayValueButton);
         this.Controls.Add(this.exitButton);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "BeamAndSlabParametersForm";
         this.ShowInTaskbar = false;
         this.Text = "Beam and Slab New Parameters";
         this.ResumeLayout(false);

      }
      #endregion
   }
}
