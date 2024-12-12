Namespace XtraVb
  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class Lab3_4_Form
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
      MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
      Me.btnOK = New System.Windows.Forms.Button
      Me.btnCancel = New System.Windows.Forms.Button
      Me.cmbFamily = New System.Windows.Forms.ComboBox
      Me.Label1 = New System.Windows.Forms.Label
      Me.Label2 = New System.Windows.Forms.Label
      Me.cmbType = New System.Windows.Forms.ComboBox
      Me.SuspendLayout()
      '
      'btnOK
      '
      Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
      Me.btnOK.Location = New System.Drawing.Point(104, 125)
      Me.btnOK.Name = "btnOK"
      Me.btnOK.Size = New System.Drawing.Size(122, 50)
      Me.btnOK.TabIndex = 0
      Me.btnOK.Text = "OK"
      Me.btnOK.UseVisualStyleBackColor = True
      '
      'btnCancel
      '
      Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.btnCancel.Location = New System.Drawing.Point(280, 125)
      Me.btnCancel.Name = "btnCancel"
      Me.btnCancel.Size = New System.Drawing.Size(122, 50)
      Me.btnCancel.TabIndex = 1
      Me.btnCancel.Text = "Cancel"
      Me.btnCancel.UseVisualStyleBackColor = True
      '
      'cmbFamily
      '
      Me.cmbFamily.FormattingEnabled = True
      Me.cmbFamily.Location = New System.Drawing.Point(78, 34)
      Me.cmbFamily.Name = "cmbFamily"
      Me.cmbFamily.Size = New System.Drawing.Size(402, 21)
      Me.cmbFamily.TabIndex = 2
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(9, 35)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(63, 20)
      Me.Label1.TabIndex = 3
      Me.Label1.Text = "Family:"
      Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'Label2
      '
      Me.Label2.Location = New System.Drawing.Point(9, 74)
      Me.Label2.Name = "Label2"
      Me.Label2.Size = New System.Drawing.Size(63, 20)
      Me.Label2.TabIndex = 5
      Me.Label2.Text = "Type:"
      Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'cmbType
      '
      Me.cmbType.FormattingEnabled = True
      Me.cmbType.Location = New System.Drawing.Point(78, 73)
      Me.cmbType.Name = "cmbType"
      Me.cmbType.Size = New System.Drawing.Size(402, 21)
      Me.cmbType.TabIndex = 4
      '
      'Lab3_4_Form
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(507, 197)
      Me.Controls.Add(Me.Label2)
      Me.Controls.Add(Me.cmbType)
      Me.Controls.Add(Me.Label1)
      Me.Controls.Add(Me.cmbFamily)
      Me.Controls.Add(Me.btnCancel)
      Me.Controls.Add(Me.btnOK)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
      Me.Name = "Lab3_4_Form"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
      Me.Text = "Select Type"
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cmbFamily As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
  End Class
End Namespace