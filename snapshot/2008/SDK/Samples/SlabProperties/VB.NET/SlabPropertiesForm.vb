'
' (C) Copyright 2003-2007 by Autodesk, Inc.
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


Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms


''' <summary>
''' Show some properties of a slab in Revit Structure2, including Level, Type name, Span divection,
''' Material name, Thickness, and Young Modulus for each layer of the slab's materira
''' </summary>
''' <remarks></remarks>
Public Class SlabPropertiesForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'To store the data
    Private m_dataBuffer As Command

    'overload the constructor
    Public Sub New(ByVal dataBuffer As Command)
        MyBase.New()

        InitializeComponent()

        m_dataBuffer = dataBuffer

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents levelTextBox As System.Windows.Forms.TextBox
    Friend WithEvents typeNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents spanDirectionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents layerGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents layerRichTextBox As System.Windows.Forms.RichTextBox
    Friend WithEvents closeButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.levelTextBox = New System.Windows.Forms.TextBox
        Me.typeNameTextBox = New System.Windows.Forms.TextBox
        Me.spanDirectionTextBox = New System.Windows.Forms.TextBox
        Me.layerGroupBox = New System.Windows.Forms.GroupBox
        Me.layerRichTextBox = New System.Windows.Forms.RichTextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.closeButton = New System.Windows.Forms.Button
        Me.layerGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'levelTextBox
        '
        Me.levelTextBox.Location = New System.Drawing.Point(134, 9)
        Me.levelTextBox.Name = "levelTextBox"
        Me.levelTextBox.ReadOnly = True
        Me.levelTextBox.Size = New System.Drawing.Size(280, 20)
        Me.levelTextBox.TabIndex = 0
        '
        'typeNameTextBox
        '
        Me.typeNameTextBox.Location = New System.Drawing.Point(134, 35)
        Me.typeNameTextBox.Name = "typeNameTextBox"
        Me.typeNameTextBox.ReadOnly = True
        Me.typeNameTextBox.Size = New System.Drawing.Size(280, 20)
        Me.typeNameTextBox.TabIndex = 1
        '
        'spanDirectionTextBox
        '
        Me.spanDirectionTextBox.Location = New System.Drawing.Point(134, 61)
        Me.spanDirectionTextBox.Name = "spanDirectionTextBox"
        Me.spanDirectionTextBox.ReadOnly = True
        Me.spanDirectionTextBox.Size = New System.Drawing.Size(280, 20)
        Me.spanDirectionTextBox.TabIndex = 2
        '
        'layerGroupBox
        '
        Me.layerGroupBox.Controls.Add(Me.layerRichTextBox)
        Me.layerGroupBox.Location = New System.Drawing.Point(21, 87)
        Me.layerGroupBox.Name = "layerGroupBox"
        Me.layerGroupBox.Size = New System.Drawing.Size(393, 273)
        Me.layerGroupBox.TabIndex = 3
        Me.layerGroupBox.TabStop = False
        Me.layerGroupBox.Text = "Layers"
        '
        'layerRichTextBox
        '
        Me.layerRichTextBox.Location = New System.Drawing.Point(6, 19)
        Me.layerRichTextBox.Name = "layerRichTextBox"
        Me.layerRichTextBox.ReadOnly = True
        Me.layerRichTextBox.Size = New System.Drawing.Size(381, 244)
        Me.layerRichTextBox.TabIndex = 0
        Me.layerRichTextBox.Text = ""
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(31, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 23)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Level:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(31, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(98, 23)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Type Name:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(31, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(98, 23)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Span Direction:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'closeButton
        '
        Me.closeButton.Location = New System.Drawing.Point(339, 366)
        Me.closeButton.Name = "closeButton"
        Me.closeButton.Size = New System.Drawing.Size(75, 23)
        Me.closeButton.TabIndex = 0
        Me.closeButton.Text = "Close"
        '
        'SlabPropertiesForm
        '
        Me.AcceptButton = Me.closeButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.closeButton
        Me.ClientSize = New System.Drawing.Size(429, 401)
        Me.Controls.Add(Me.closeButton)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.layerGroupBox)
        Me.Controls.Add(Me.spanDirectionTextBox)
        Me.Controls.Add(Me.typeNameTextBox)
        Me.Controls.Add(Me.levelTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SlabPropertiesForm"
        Me.ShowInTaskbar = False
        Me.Text = "Slab Properties"
        Me.layerGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    ''' <summary>
    ''' Display the properties on the form when the form load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SlabPropertiesForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.levelTextBox.Text = m_dataBuffer.GetLevel()
        Me.typeNameTextBox.Text = m_dataBuffer.GetTypeName()
        Me.spanDirectionTextBox.Text = m_dataBuffer.GetSpanDirection()

        Dim numberOfLayers As Integer = m_dataBuffer.GetLayerNum()

        Me.layerRichTextBox.Text = ""

        For i As Integer = 0 To numberOfLayers - 1

            'Get each layer's Material name and Young Modulus properties
            m_dataBuffer.SetLayer(i)

            Me.layerRichTextBox.Text += "Layer " + (i + 1).ToString() + ControlChars.Lf
            Me.layerRichTextBox.Text += "Material name: " + m_dataBuffer.GetLayerMaterialName() + ControlChars.Lf
            Me.layerRichTextBox.Text += "Thickness: " + m_dataBuffer.GetLayerThickness() + ControlChars.Lf
            Me.layerRichTextBox.Text += "YoungModule X: " + m_dataBuffer.GetLayerYoungModuleX() + ControlChars.Lf
            Me.layerRichTextBox.Text += "YoungModule Y: " + m_dataBuffer.GetLayerYoungModuleY() + ControlChars.Lf
            Me.layerRichTextBox.Text += "YoungModule Z: " + m_dataBuffer.GetLayerYoungModuleZ() + ControlChars.Lf
            Me.layerRichTextBox.Text += "---------------------------------------------------------------" + ControlChars.Lf

        Next i

    End Sub

    ''' <summary>
    ''' close dialog
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub closeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles closeButton.Click
        Close()
    End Sub
End Class
