namespace AdnRme
{
  partial class CmdInspectElectricalForm
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
      this.tv = new System.Windows.Forms.TreeView();
      this.SuspendLayout();
      // 
      // tv
      // 
      this.tv.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.tv.Location = new System.Drawing.Point( 0, 0 );
      this.tv.Name = "tv";
      this.tv.Size = new System.Drawing.Size( 294, 276 );
      this.tv.TabIndex = 0;
      // 
      // CmdInspectElectricaForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size( 292, 273 );
      this.Controls.Add( this.tv );
      this.Name = "CmdInspectElectricaForm";
      this.Text = "Inspect Electrical System";
      this.ResumeLayout( false );

    }

    #endregion

    private System.Windows.Forms.TreeView tv;
  }
}