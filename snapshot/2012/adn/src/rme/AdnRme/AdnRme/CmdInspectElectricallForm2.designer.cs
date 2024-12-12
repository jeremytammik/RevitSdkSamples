namespace AdnRme
{
    partial class CmdInspectElectricalForm2
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
          this.splitContainer1 = new System.Windows.Forms.SplitContainer();
          this.listBox1 = new System.Windows.Forms.ListBox();
          this.tv = new System.Windows.Forms.TreeView();
          this.splitContainer1.Panel1.SuspendLayout();
          this.splitContainer1.Panel2.SuspendLayout();
          this.splitContainer1.SuspendLayout();
          this.SuspendLayout();
          // 
          // splitContainer1
          // 
          this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
          this.splitContainer1.Name = "splitContainer1";
          // 
          // splitContainer1.Panel1
          // 
          this.splitContainer1.Panel1.Controls.Add( this.listBox1 );
          // 
          // splitContainer1.Panel2
          // 
          this.splitContainer1.Panel2.Controls.Add( this.tv );
          this.splitContainer1.Size = new System.Drawing.Size( 459, 556 );
          this.splitContainer1.SplitterDistance = 153;
          this.splitContainer1.TabIndex = 0;
          // 
          // listBox1
          // 
          this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.listBox1.FormattingEnabled = true;
          this.listBox1.Location = new System.Drawing.Point( 0, 0 );
          this.listBox1.Name = "listBox1";
          this.listBox1.Size = new System.Drawing.Size( 153, 550 );
          this.listBox1.Sorted = true;
          this.listBox1.TabIndex = 0;
          this.listBox1.SelectedIndexChanged += new System.EventHandler( this.listBox1_SelectedIndexChanged );
          // 
          // tv
          // 
          this.tv.Dock = System.Windows.Forms.DockStyle.Fill;
          this.tv.Location = new System.Drawing.Point( 0, 0 );
          this.tv.Name = "tv";
          this.tv.Size = new System.Drawing.Size( 302, 556 );
          this.tv.TabIndex = 0;
          // 
          // CmdInspectElectricalForm2
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size( 459, 556 );
          this.Controls.Add( this.splitContainer1 );
          this.Name = "CmdInspectElectricalForm2";
          this.Text = "Electrical System Inspector";
          this.splitContainer1.Panel1.ResumeLayout( false );
          this.splitContainer1.Panel2.ResumeLayout( false );
          this.splitContainer1.ResumeLayout( false );
          this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TreeView tv;
    }
}