namespace Labs
{
  partial class Lab3_4_Form
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.cmbFamily = new System.Windows.Forms.ComboBox();
      this.cmbType = new System.Windows.Forms.ComboBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      //
      // label1
      //
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 15, 18 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 39, 13 );
      this.label1.TabIndex = 0;
      this.label1.Text = "Family:";
      //
      // label2
      //
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point( 15, 47 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size( 34, 13 );
      this.label2.TabIndex = 1;
      this.label2.Text = "Type:";
      //
      // cmbFamily
      //
      this.cmbFamily.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.cmbFamily.FormattingEnabled = true;
      this.cmbFamily.Location = new System.Drawing.Point( 60, 15 );
      this.cmbFamily.Name = "cmbFamily";
      this.cmbFamily.Size = new System.Drawing.Size( 254, 21 );
      this.cmbFamily.TabIndex = 2;
      this.cmbFamily.SelectedIndexChanged += new System.EventHandler( this.cmbFamily_SelectedIndexChanged );
      //
      // cmbType
      //
      this.cmbType.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.cmbType.FormattingEnabled = true;
      this.cmbType.Location = new System.Drawing.Point( 60, 44 );
      this.cmbType.Name = "cmbType";
      this.cmbType.Size = new System.Drawing.Size( 254, 21 );
      this.cmbType.TabIndex = 3;
      //
      // button1
      //
      this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button1.Location = new System.Drawing.Point( 60, 82 );
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size( 120, 30 );
      this.button1.TabIndex = 4;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      //
      // button2
      //
      this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button2.Location = new System.Drawing.Point( 194, 82 );
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size( 120, 30 );
      this.button2.TabIndex = 5;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      //
      // Lab3_4_Form
      //
      this.AcceptButton = this.button1;
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.button2;
      this.ClientSize = new System.Drawing.Size( 327, 127 );
      this.Controls.Add( this.button2 );
      this.Controls.Add( this.button1 );
      this.Controls.Add( this.cmbType );
      this.Controls.Add( this.cmbFamily );
      this.Controls.Add( this.label2 );
      this.Controls.Add( this.label1 );
      this.Name = "Lab3_4_Form";
      this.Text = "Select Type";
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    public System.Windows.Forms.ComboBox cmbFamily;
    public System.Windows.Forms.ComboBox cmbType;
  }
}