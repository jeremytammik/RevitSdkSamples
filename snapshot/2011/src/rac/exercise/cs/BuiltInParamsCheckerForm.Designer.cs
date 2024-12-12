namespace Labs
{
  partial class BuiltInParamsCheckerForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if( disposing && ( components != null ) )
      {
        components.Dispose();
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
      this.components = new System.ComponentModel.Container();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip( this.components );
      this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ( ( System.ComponentModel.ISupportInitialize ) ( this.dataGridView1 ) ).BeginInit();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      //
      // dataGridView1
      //
      this.dataGridView1.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
      this.dataGridView1.Location = new System.Drawing.Point( 1, 4 );
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      this.dataGridView1.Size = new System.Drawing.Size( 765, 408 );
      this.dataGridView1.TabIndex = 0;
      //
      // contextMenuStrip1
      //
      this.contextMenuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.copyToClipboardToolStripMenuItem} );
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size( 172, 26 );
      //
      // copyToClipboardToolStripMenuItem
      //
      this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
      this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size( 171, 22 );
      this.copyToClipboardToolStripMenuItem.Text = "Copy to Clipboard";
      this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler( CopyToClipboardToolStripMenuItem_Click );
      //
      // BuiltInParamsCheckerForm
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size( 769, 414 );
      this.Controls.Add( this.dataGridView1 );
      this.Name = "BuiltInParamsCheckerForm";
      this.Text = "Built-in Parameters";
      this.Load += new System.EventHandler( BuiltInParamsCheckerForm_Load );
      ( ( System.ComponentModel.ISupportInitialize ) ( this.dataGridView1 ) ).EndInit();
      this.contextMenuStrip1.ResumeLayout( false );
      this.ResumeLayout( false );

    }
    #endregion

    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
  }
}