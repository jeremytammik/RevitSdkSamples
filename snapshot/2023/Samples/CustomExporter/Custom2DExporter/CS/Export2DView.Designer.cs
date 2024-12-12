namespace Revit.SDK.Samples.Custom2DExporter.CS
{
    partial class Export2DView
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
         this.checkBox2 = new System.Windows.Forms.CheckBox();
         this.checkBox3 = new System.Windows.Forms.CheckBox();
         this.optionGroupBox = new System.Windows.Forms.GroupBox();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.buttonCancel = new System.Windows.Forms.Button();
         this.buttonOK = new System.Windows.Forms.Button();
         this.textBox2 = new System.Windows.Forms.TextBox();
         this.optionGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // checkBox2
         // 
         this.checkBox2.AutoSize = true;
         this.checkBox2.Location = new System.Drawing.Point(17, 58);
         this.checkBox2.Name = "checkBox2";
         this.checkBox2.Size = new System.Drawing.Size(196, 17);
         this.checkBox2.TabIndex = 1;
         this.checkBox2.Text = "Export annotation geometry and text";
         this.checkBox2.UseVisualStyleBackColor = true;
         this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
         // 
         // checkBox3
         // 
         this.checkBox3.AutoSize = true;
         this.checkBox3.Location = new System.Drawing.Point(17, 82);
         this.checkBox3.Name = "checkBox3";
         this.checkBox3.Size = new System.Drawing.Size(224, 17);
         this.checkBox3.TabIndex = 2;
         this.checkBox3.Text = "Export patterns (non-wireframe views only)";
         this.checkBox3.UseVisualStyleBackColor = true;
         this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
         // 
         // optionGroupBox
         // 
         this.optionGroupBox.Controls.Add(this.textBox1);
         this.optionGroupBox.Controls.Add(this.checkBox3);
         this.optionGroupBox.Controls.Add(this.checkBox2);
         this.optionGroupBox.Location = new System.Drawing.Point(24, 44);
         this.optionGroupBox.Name = "optionGroupBox";
         this.optionGroupBox.Size = new System.Drawing.Size(313, 100);
         this.optionGroupBox.TabIndex = 3;
         this.optionGroupBox.TabStop = false;
         this.optionGroupBox.Text = "Export Options";
         // 
         // textBox1
         // 
         this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
         this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
         this.textBox1.Location = new System.Drawing.Point(17, 32);
         this.textBox1.Name = "textBox1";
         this.textBox1.Size = new System.Drawing.Size(262, 13);
         this.textBox1.TabIndex = 7;
         this.textBox1.Text = "Model objects are always exported, but you can also:";
         // 
         // buttonCancel
         // 
         this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.buttonCancel.Location = new System.Drawing.Point(262, 166);
         this.buttonCancel.Name = "buttonCancel";
         this.buttonCancel.Size = new System.Drawing.Size(75, 23);
         this.buttonCancel.TabIndex = 6;
         this.buttonCancel.Text = "&Cancel";
         this.buttonCancel.UseVisualStyleBackColor = true;
         // 
         // buttonOK
         // 
         this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonOK.Location = new System.Drawing.Point(181, 166);
         this.buttonOK.Name = "buttonOK";
         this.buttonOK.Size = new System.Drawing.Size(75, 23);
         this.buttonOK.TabIndex = 5;
         this.buttonOK.Text = "&OK";
         this.buttonOK.UseVisualStyleBackColor = true;
         // 
         // textBox2
         // 
         this.textBox2.BackColor = System.Drawing.SystemColors.Menu;
         this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
         this.textBox2.Location = new System.Drawing.Point(30, 12);
         this.textBox2.Multiline = true;
         this.textBox2.Name = "textBox2";
         this.textBox2.Size = new System.Drawing.Size(307, 26);
         this.textBox2.TabIndex = 8;
         this.textBox2.Text = "Exports lines from the 2D view and draws them on the screen";
         // 
         // Export2DView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(374, 210);
         this.Controls.Add(this.textBox2);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.buttonOK);
         this.Controls.Add(this.optionGroupBox);
         this.Name = "Export2DView";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Export 2D View";
         this.optionGroupBox.ResumeLayout(false);
         this.optionGroupBox.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.GroupBox optionGroupBox;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
      private System.Windows.Forms.TextBox textBox1;
      private System.Windows.Forms.TextBox textBox2;
   }
}