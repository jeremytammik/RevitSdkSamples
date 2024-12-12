namespace XtraCs
{
  partial class ElementIdForm
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
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOk = new System.Windows.Forms.Button();
      this.btnPick = new System.Windows.Forms.Button();
      this.SuspendLayout();
      //
      // textBox1
      //
      this.textBox1.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.textBox1.Location = new System.Drawing.Point( 13, 13 );
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size( 237, 20 );
      this.textBox1.TabIndex = 0;
      this.textBox1.TextChanged += new System.EventHandler( this.textBox1_TextChanged );
      //
      // btnCancel
      //
      this.btnCancel.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Left ) ) );
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point( 94, 48 );
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size( 75, 25 );
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler( this.btnCancel_Click );
      //
      // btnOk
      //
      this.btnOk.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOk.Location = new System.Drawing.Point( 175, 48 );
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size( 75, 25 );
      this.btnOk.TabIndex = 3;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new System.EventHandler( this.btnOk_Click );
      //
      // btnPick
      //
      this.btnPick.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Left ) ) );
      this.btnPick.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnPick.Location = new System.Drawing.Point( 13, 48 );
      this.btnPick.Name = "btnPick";
      this.btnPick.Size = new System.Drawing.Size( 75, 25 );
      this.btnPick.TabIndex = 1;
      this.btnPick.Text = "Pick";
      this.btnPick.UseVisualStyleBackColor = true;
      this.btnPick.Click += new System.EventHandler( this.btnPick_Click );
      //
      // ElementIdForm
      //
      this.AcceptButton = this.btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size( 262, 84 );
      this.Controls.Add( this.btnPick );
      this.Controls.Add( this.btnOk );
      this.Controls.Add( this.btnCancel );
      this.Controls.Add( this.textBox1 );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximumSize = new System.Drawing.Size( 1000, 111 );
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size( 200, 111 );
      this.Name = "ElementIdForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "Element Id";
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnPick;
  }
}