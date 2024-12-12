namespace Revit.SDK.Samples.ImportExport.CS
{
   partial class ExportPDFOptionsForm
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
         this.buttonOK = new System.Windows.Forms.Button();
         this.buttonCancel = new System.Windows.Forms.Button();
         this.checkBoxCombineViews = new System.Windows.Forms.CheckBox();
         this.SuspendLayout();
         // 
         // buttonOK
         // 
         this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonOK.Location = new System.Drawing.Point(120, 113);
         this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
         this.buttonOK.Name = "buttonOK";
         this.buttonOK.Size = new System.Drawing.Size(121, 28);
         this.buttonOK.TabIndex = 1;
         this.buttonOK.Text = "&OK";
         this.buttonOK.UseVisualStyleBackColor = true;
         this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
         // 
         // buttonCancel
         // 
         this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.buttonCancel.Location = new System.Drawing.Point(253, 113);
         this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
         this.buttonCancel.Name = "buttonCancel";
         this.buttonCancel.Size = new System.Drawing.Size(135, 28);
         this.buttonCancel.TabIndex = 3;
         this.buttonCancel.Text = "&Cancel";
         this.buttonCancel.UseVisualStyleBackColor = true;
         // 
         // checkBoxCombineViews
         // 
         this.checkBoxCombineViews.AutoSize = true;
         this.checkBoxCombineViews.Location = new System.Drawing.Point(13, 34);
         this.checkBoxCombineViews.Margin = new System.Windows.Forms.Padding(4);
         this.checkBoxCombineViews.Name = "checkBoxCombineViews";
         this.checkBoxCombineViews.Size = new System.Drawing.Size(375, 21);
         this.checkBoxCombineViews.TabIndex = 5;
         this.checkBoxCombineViews.Text = "Combine selected views and sheets into single PDF file";
         this.checkBoxCombineViews.UseVisualStyleBackColor = true;
         // 
         // ExportPDFOptionsForm
         // 
         this.AcceptButton = this.buttonOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.buttonCancel;
         this.ClientSize = new System.Drawing.Size(411, 163);
         this.Controls.Add(this.checkBoxCombineViews);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.buttonOK);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "ExportPDFOptionsForm";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "ExportPDFOptionsForm";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button buttonOK;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.CheckBox checkBoxCombineViews;
   }
}