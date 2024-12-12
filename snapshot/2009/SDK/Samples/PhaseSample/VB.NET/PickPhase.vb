'
' (C) Copyright 2003-2008 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software in
' object code form for any purpose and without fee is hereby granted,
' provided that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.?AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'

Public Class PickPhase
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CreatedRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents DemolishedRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents PhaseCheckedListBox As System.Windows.Forms.CheckedListBox
    Private WithEvents escButton As System.Windows.Forms.Button
    Friend WithEvents okButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.DemolishedRadioButton = New System.Windows.Forms.RadioButton
        Me.CreatedRadioButton = New System.Windows.Forms.RadioButton
        Me.PhaseCheckedListBox = New System.Windows.Forms.CheckedListBox
        Me.okButton = New System.Windows.Forms.Button
        Me.escButton = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.DemolishedRadioButton)
        Me.GroupBox1.Controls.Add(Me.CreatedRadioButton)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(272, 72)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Phase Type"
        '
        'DemolishedRadioButton
        '
        Me.DemolishedRadioButton.Location = New System.Drawing.Point(8, 40)
        Me.DemolishedRadioButton.Name = "DemolishedRadioButton"
        Me.DemolishedRadioButton.Size = New System.Drawing.Size(104, 24)
        Me.DemolishedRadioButton.TabIndex = 2
        Me.DemolishedRadioButton.Text = "Demolished"
        '
        'CreatedRadioButton
        '
        Me.CreatedRadioButton.Checked = True
        Me.CreatedRadioButton.Location = New System.Drawing.Point(8, 16)
        Me.CreatedRadioButton.Name = "CreatedRadioButton"
        Me.CreatedRadioButton.Size = New System.Drawing.Size(104, 24)
        Me.CreatedRadioButton.TabIndex = 1
        Me.CreatedRadioButton.TabStop = True
        Me.CreatedRadioButton.Text = "Created"
        '
        'PhaseCheckedListBox
        '
        Me.PhaseCheckedListBox.CheckOnClick = True
        Me.PhaseCheckedListBox.Location = New System.Drawing.Point(8, 88)
        Me.PhaseCheckedListBox.Name = "PhaseCheckedListBox"
        Me.PhaseCheckedListBox.Size = New System.Drawing.Size(272, 169)
        Me.PhaseCheckedListBox.TabIndex = 4
        '
        'okButton
        '
        Me.okButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.okButton.Location = New System.Drawing.Point(124, 263)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(75, 23)
        Me.okButton.TabIndex = 5
        Me.okButton.Text = "&OK"
        '
        'escButton
        '
        Me.escButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.escButton.Location = New System.Drawing.Point(205, 263)
        Me.escButton.Name = "escButton"
        Me.escButton.Size = New System.Drawing.Size(75, 23)
        Me.escButton.TabIndex = 6
        Me.escButton.Text = "&Cancel"
        Me.escButton.UseVisualStyleBackColor = True
        '
        'PickPhase
        '
        Me.AcceptButton = Me.okButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.escButton
        Me.ClientSize = New System.Drawing.Size(292, 293)
        Me.Controls.Add(Me.escButton)
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me.PhaseCheckedListBox)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PickPhase"
        Me.ShowInTaskbar = False
        Me.Text = "PickPhase"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

End Class
