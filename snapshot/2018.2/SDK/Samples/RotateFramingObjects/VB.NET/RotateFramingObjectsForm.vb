'
' (C) Copyright 2003-2017 by Autodesk, Inc.
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
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'


Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Public Class RotateFramingObjectsForm
    Inherits System.Windows.Forms.Form
    Private m_instance As RotateFramingObjects
    Private m_isReset As Boolean
    Public Property IsReset() As Boolean
        Get
            Return m_isReset
        End Get
        Set(ByVal Value As Boolean)
            m_isReset = Value
        End Set
    End Property
#Region " Windows Form Designer generated code "

    Public Sub New(ByVal Inst As RotateFramingObjects)
        MyBase.New()
        'This call is required by the Windows Form Designer.
        m_instance = Inst
        m_isReset = False
        If (m_instance Is Nothing) Then
            Autodesk.Revit.UI.TaskDialog.Show("Revit", "Load Application Failed !")
        End If
        InitializeComponent()
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
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents rotationLabel As System.Windows.Forms.Label
    Public WithEvents rotationTextBox As System.Windows.Forms.TextBox
    Friend WithEvents relativeRadio As System.Windows.Forms.RadioButton
    Public WithEvents absoluteRadio As System.Windows.Forms.RadioButton
    Friend WithEvents canceledButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.rotationLabel = New System.Windows.Forms.Label
        Me.okButton = New System.Windows.Forms.Button
        Me.canceledButton = New System.Windows.Forms.Button
        Me.rotationTextBox = New System.Windows.Forms.TextBox
        Me.relativeRadio = New System.Windows.Forms.RadioButton
        Me.absoluteRadio = New System.Windows.Forms.RadioButton
        Me.SuspendLayout()
        '
        'rotationLabel
        '
        Me.rotationLabel.Location = New System.Drawing.Point(12, 12)
        Me.rotationLabel.Name = "rotationLabel"
        Me.rotationLabel.Size = New System.Drawing.Size(51, 17)
        Me.rotationLabel.TabIndex = 17
        Me.rotationLabel.Text = "Rotation"
        '
        'okButton
        '
        Me.okButton.Location = New System.Drawing.Point(12, 65)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(75, 23)
        Me.okButton.TabIndex = 16
        Me.okButton.Text = "&OK"
        '
        'canceledButton
        '
        Me.canceledButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.canceledButton.Location = New System.Drawing.Point(93, 65)
        Me.canceledButton.Name = "canceledButton"
        Me.canceledButton.Size = New System.Drawing.Size(75, 23)
        Me.canceledButton.TabIndex = 15
        Me.canceledButton.Text = "&Cancel"
        '
        'rotationTextBox
        '
        Me.rotationTextBox.Location = New System.Drawing.Point(69, 9)
        Me.rotationTextBox.Name = "rotationTextBox"
        Me.rotationTextBox.Size = New System.Drawing.Size(100, 20)
        Me.rotationTextBox.TabIndex = 1
        '
        'relativeRadio
        '
        Me.relativeRadio.Checked = True
        Me.relativeRadio.Location = New System.Drawing.Point(12, 35)
        Me.relativeRadio.Name = "relativeRadio"
        Me.relativeRadio.Size = New System.Drawing.Size(64, 24)
        Me.relativeRadio.TabIndex = 11
        Me.relativeRadio.TabStop = True
        Me.relativeRadio.Text = "Relative"
        '
        'absoluteRadio
        '
        Me.absoluteRadio.Location = New System.Drawing.Point(82, 35)
        Me.absoluteRadio.Name = "absoluteRadio"
        Me.absoluteRadio.Size = New System.Drawing.Size(72, 24)
        Me.absoluteRadio.TabIndex = 10
        Me.absoluteRadio.Text = "Absolute"
        '
        'RotateFramingObjectsForm
        '
        Me.AcceptButton = Me.okButton
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.canceledButton
        Me.ClientSize = New System.Drawing.Size(181, 98)
        Me.Controls.Add(Me.rotationLabel)
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me.canceledButton)
        Me.Controls.Add(Me.rotationTextBox)
        Me.Controls.Add(Me.relativeRadio)
        Me.Controls.Add(Me.absoluteRadio)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RotateFramingObjectsForm"
        Me.ShowInTaskbar = False
        Me.Text = "Rotate Framing Object"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles okButton.Click
        If (m_isReset) Then
            m_instance.RotateElement()
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles canceledButton.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub rotationTextBox_TextChanged(ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles rotationTextBox.TextChanged
        If "" <> Me.rotationTextBox.Text Then
            Try
                m_instance.ReceiveRotationTextBox = Convert.ToDouble(Me.rotationTextBox.Text)
            Catch ex As Exception
                Autodesk.Revit.UI.TaskDialog.Show("Revit", "Please input number")
                Me.rotationTextBox.Clear()
            End Try

        Else
            m_instance.ReceiveRotationTextBox = 0
        End If
        m_isReset = True
    End Sub

    Private Sub relativeRadio_CheckedChanged(ByVal sender As System.Object, ByVal e _
    As System.EventArgs) Handles relativeRadio.CheckedChanged
        m_isReset = True
        m_instance.IsAbsoluteChecked = False
    End Sub

    Private Sub absoluteRadio_CheckedChanged(ByVal sender As System.Object, ByVal e _
    As System.EventArgs) Handles absoluteRadio.CheckedChanged
        m_isReset = True
        m_instance.IsAbsoluteChecked = True
    End Sub
    Private Sub rotationTextBox_KeyPress(ByVal sender As Object, ByVal e _
    As System.Windows.Forms.KeyPressEventArgs) Handles rotationTextBox.KeyPress
        If Microsoft.VisualBasic.ChrW(13) = e.KeyChar Then
            okButton_Click(sender, e)
        Else
            rotationTextBox_TextChanged(sender, e)
        End If
    End Sub
End Class
