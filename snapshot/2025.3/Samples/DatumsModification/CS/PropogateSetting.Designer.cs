namespace Revit.SDK.Samples.DatumsModification.CS
{
   partial class PropogateSetting
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
         this.propagationViewList = new System.Windows.Forms.CheckedListBox();
         this.richTextBox1 = new System.Windows.Forms.RichTextBox();
         this.button1 = new System.Windows.Forms.Button();
         this.button2 = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // propagationViewList
         // 
         this.propagationViewList.CheckOnClick = true;
         this.propagationViewList.FormattingEnabled = true;
         this.propagationViewList.Location = new System.Drawing.Point(12, 53);
         this.propagationViewList.Name = "propagationViewList";
         this.propagationViewList.Size = new System.Drawing.Size(246, 124);
         this.propagationViewList.TabIndex = 0;
         // 
         // richTextBox1
         // 
         this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
         this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
         this.richTextBox1.Location = new System.Drawing.Point(12, 12);
         this.richTextBox1.Name = "richTextBox1";
         this.richTextBox1.Size = new System.Drawing.Size(246, 35);
         this.richTextBox1.TabIndex = 1;
         this.richTextBox1.Text = "For the selected Datum, apply the Extents from this view to the following Views:";
         // 
         // button1
         // 
         this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.button1.Location = new System.Drawing.Point(92, 195);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(75, 23);
         this.button1.TabIndex = 2;
         this.button1.Text = "OK";
         this.button1.UseVisualStyleBackColor = true;
         // 
         // button2
         // 
         this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.button2.Location = new System.Drawing.Point(183, 195);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(75, 23);
         this.button2.TabIndex = 3;
         this.button2.Text = "Cancel";
         this.button2.UseVisualStyleBackColor = true;
         // 
         // PropogateSetting
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(270, 230);
         this.Controls.Add(this.button2);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.richTextBox1);
         this.Controls.Add(this.propagationViewList);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "PropogateSetting";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "PropogateSetting";
         this.ResumeLayout(false);

      }

      #endregion

      /// <summary>
      /// 
      /// </summary>
      public System.Windows.Forms.CheckedListBox propagationViewList;
      private System.Windows.Forms.RichTextBox richTextBox1;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button button2;
   }
}