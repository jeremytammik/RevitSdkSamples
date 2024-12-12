<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutAPIToolbarForm
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
        Me.aboutToolbarLabel = New System.Windows.Forms.Label
        Me.okButton = New System.Windows.Forms.Button
        Me.contactLinkLabel = New System.Windows.Forms.LinkLabel
        Me.aboutToolbarRichTextBox = New System.Windows.Forms.RichTextBox
        Me.SuspendLayout()
        '
        'aboutToolbarLabel
        '
        Me.aboutToolbarLabel.AutoSize = True
        Me.aboutToolbarLabel.Location = New System.Drawing.Point(26, 23)
        Me.aboutToolbarLabel.Name = "aboutToolbarLabel"
        Me.aboutToolbarLabel.Size = New System.Drawing.Size(320, 17)
        Me.aboutToolbarLabel.TabIndex = 12
        Me.aboutToolbarLabel.Text = "Introduce custom Toolbar functionality in RevitAPI"
        '
        'okButton
        '
        Me.okButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.okButton.Location = New System.Drawing.Point(491, 342)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(77, 29)
        Me.okButton.TabIndex = 9
        Me.okButton.Text = "&OK"
        Me.okButton.UseVisualStyleBackColor = True
        '
        'contactLinkLabel
        '
        Me.contactLinkLabel.AutoSize = True
        Me.contactLinkLabel.LinkArea = New System.Windows.Forms.LinkArea(0, 32)
        Me.contactLinkLabel.Location = New System.Drawing.Point(24, 342)
        Me.contactLinkLabel.Name = "contactLinkLabel"
        Me.contactLinkLabel.Size = New System.Drawing.Size(69, 20)
        Me.contactLinkLabel.TabIndex = 10
        Me.contactLinkLabel.TabStop = True
        Me.contactLinkLabel.Text = "Contact us"
        Me.contactLinkLabel.UseCompatibleTextRendering = True
        '
        'aboutToolbarRichTextBox
        '
        Me.aboutToolbarRichTextBox.BackColor = System.Drawing.Color.White
        Me.aboutToolbarRichTextBox.Location = New System.Drawing.Point(24, 44)
        Me.aboutToolbarRichTextBox.Name = "aboutToolbarRichTextBox"
        Me.aboutToolbarRichTextBox.ReadOnly = True
        Me.aboutToolbarRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.aboutToolbarRichTextBox.Size = New System.Drawing.Size(544, 277)
        Me.aboutToolbarRichTextBox.TabIndex = 11
        Me.aboutToolbarRichTextBox.TabStop = False
        Me.aboutToolbarRichTextBox.Text = ""
        '
        'AboutAPIToolbarForm
        '
        Me.AcceptButton = Me.okButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.okButton
        Me.ClientSize = New System.Drawing.Size(588, 385)
        Me.Controls.Add(Me.aboutToolbarLabel)
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me.contactLinkLabel)
        Me.Controls.Add(Me.aboutToolbarRichTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutAPIToolbarForm"
        Me.ShowInTaskbar = False
        Me.Text = "About Toolbar"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents aboutToolbarLabel As System.Windows.Forms.Label
    Private WithEvents okButton As System.Windows.Forms.Button
    Private WithEvents contactLinkLabel As System.Windows.Forms.LinkLabel
    Private WithEvents aboutToolbarRichTextBox As System.Windows.Forms.RichTextBox
End Class
