'
' (C) Copyright 2003-2014 by Autodesk, Inc.
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


' Display the function of each of a select floor's structural layers
Public Class StructuralLayerFunctionForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    ' <summary>
    ' Constructor of StructuralLayerFunctionForm
    ' </summary>
    ' <param name="dataBuffer">A reference of StructuralLayerFunction class</param>
    Public Sub New(ByVal dataBuffer As Command)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'Set the data source of the ListBox control
        functionListBox.DataSource = dataBuffer.Functions

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (Nothing Is components) Then
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
    Friend WithEvents functionGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents functionListBox As System.Windows.Forms.ListBox
    Friend WithEvents okButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.functionGroupBox = New System.Windows.Forms.GroupBox
        Me.functionListBox = New System.Windows.Forms.ListBox
        Me.okButton = New System.Windows.Forms.Button
        Me.functionGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'functionGroupBox
        '
        Me.functionGroupBox.Controls.Add(Me.functionListBox)
        Me.functionGroupBox.Location = New System.Drawing.Point(12, 4)
        Me.functionGroupBox.Name = "functionGroupBox"
        Me.functionGroupBox.Size = New System.Drawing.Size(232, 227)
        Me.functionGroupBox.TabIndex = 0
        Me.functionGroupBox.TabStop = False
        Me.functionGroupBox.Text = "Layers Functions List"
        '
        'functionListBox
        '
        Me.functionListBox.Location = New System.Drawing.Point(6, 24)
        Me.functionListBox.Name = "functionListBox"
        Me.functionListBox.Size = New System.Drawing.Size(220, 199)
        Me.functionListBox.TabIndex = 0
        '
        'okButton
        '
        Me.okButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.okButton.Location = New System.Drawing.Point(188, 237)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(56, 24)
        Me.okButton.TabIndex = 1
        Me.okButton.Text = "OK"
        '
        'StructuralLayerFunctionForm
        '
        Me.AcceptButton = Me.okButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.okButton
        Me.ClientSize = New System.Drawing.Size(256, 273)
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me.functionGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "StructuralLayerFunctionForm"
        Me.ShowInTaskbar = False
        Me.Text = "Structure Layers Function"
        Me.functionGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

End Class
