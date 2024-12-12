'
' (C) Copyright 2003-2007 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software in
' object code form for any purpose and without fee is hereby granted
' provided that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL ITS FAULTS.
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

Imports System.Windows.Forms

Public Class CreateBeamsColumnsBracesForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Private m_dataBuffer As Command = Nothing

    Public Sub New(ByVal dataBuffer As Command)
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        m_dataBuffer = dataBuffer
    End Sub 'New

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
    Friend WithEvents floornumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DistanceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents YTextBox As System.Windows.Forms.TextBox
    Friend WithEvents XTextBox As System.Windows.Forms.TextBox
    Friend WithEvents braceComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents beamComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents columnComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents cancelButton1 As System.Windows.Forms.Button
    Friend WithEvents floornumberLabel As System.Windows.Forms.Label
    Friend WithEvents XLabel As System.Windows.Forms.Label
    Friend WithEvents YLabel As System.Windows.Forms.Label
    Friend WithEvents DistanceLabel As System.Windows.Forms.Label
    Friend WithEvents braceLabel As System.Windows.Forms.Label
    Friend WithEvents beamLabel As System.Windows.Forms.Label
    Friend WithEvents columnLabel As System.Windows.Forms.Label
    Friend WithEvents unitLabel As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cancelButton1 = New System.Windows.Forms.Button
        Me.floornumberLabel = New System.Windows.Forms.Label
        Me.XLabel = New System.Windows.Forms.Label
        Me.YLabel = New System.Windows.Forms.Label
        Me.DistanceLabel = New System.Windows.Forms.Label
        Me.floornumberTextBox = New System.Windows.Forms.TextBox
        Me.DistanceTextBox = New System.Windows.Forms.TextBox
        Me.YTextBox = New System.Windows.Forms.TextBox
        Me.XTextBox = New System.Windows.Forms.TextBox
        Me.braceLabel = New System.Windows.Forms.Label
        Me.beamLabel = New System.Windows.Forms.Label
        Me.columnLabel = New System.Windows.Forms.Label
        Me.braceComboBox = New System.Windows.Forms.ComboBox
        Me.beamComboBox = New System.Windows.Forms.ComboBox
        Me.columnComboBox = New System.Windows.Forms.ComboBox
        Me.OKButton = New System.Windows.Forms.Button
        Me.unitLabel = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cancelButton1
        '
        Me.cancelButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancelButton1.Location = New System.Drawing.Point(376, 207)
        Me.cancelButton1.Name = "cancelButton1"
        Me.cancelButton1.Size = New System.Drawing.Size(75, 23)
        Me.cancelButton1.TabIndex = 9
        Me.cancelButton1.Text = "&Cancel"
        '
        'floornumberLabel
        '
        Me.floornumberLabel.Location = New System.Drawing.Point(16, 183)
        Me.floornumberLabel.Name = "floornumberLabel"
        Me.floornumberLabel.Size = New System.Drawing.Size(144, 23)
        Me.floornumberLabel.TabIndex = 33
        Me.floornumberLabel.Text = "Number of Floors:"
        '
        'XLabel
        '
        Me.XLabel.Location = New System.Drawing.Point(16, 71)
        Me.XLabel.Name = "XLabel"
        Me.XLabel.Size = New System.Drawing.Size(200, 23)
        Me.XLabel.TabIndex = 32
        Me.XLabel.Text = "Number of Columns in the X Direction:"
        '
        'YLabel
        '
        Me.YLabel.Location = New System.Drawing.Point(16, 127)
        Me.YLabel.Name = "YLabel"
        Me.YLabel.Size = New System.Drawing.Size(200, 23)
        Me.YLabel.TabIndex = 31
        Me.YLabel.Text = "Number of Columns in the Y Direction:"
        '
        'DistanceLabel
        '
        Me.DistanceLabel.Location = New System.Drawing.Point(16, 15)
        Me.DistanceLabel.Name = "DistanceLabel"
        Me.DistanceLabel.Size = New System.Drawing.Size(152, 23)
        Me.DistanceLabel.TabIndex = 10
        Me.DistanceLabel.Text = "Distance between Columns:"
        '
        'floornumberTextBox
        '
        Me.floornumberTextBox.Location = New System.Drawing.Point(16, 207)
        Me.floornumberTextBox.Name = "floornumberTextBox"
        Me.floornumberTextBox.Size = New System.Drawing.Size(112, 20)
        Me.floornumberTextBox.TabIndex = 4
        Me.floornumberTextBox.Text = "1"
        '
        'DistanceTextBox
        '
        Me.DistanceTextBox.Location = New System.Drawing.Point(16, 39)
        Me.DistanceTextBox.Name = "DistanceTextBox"
        Me.DistanceTextBox.Size = New System.Drawing.Size(136, 20)
        Me.DistanceTextBox.TabIndex = 1
        Me.DistanceTextBox.Text = "20.0"
        '
        'YTextBox
        '
        Me.YTextBox.Location = New System.Drawing.Point(16, 151)
        Me.YTextBox.Name = "YTextBox"
        Me.YTextBox.Size = New System.Drawing.Size(136, 20)
        Me.YTextBox.TabIndex = 3
        Me.YTextBox.Text = "2"
        '
        'XTextBox
        '
        Me.XTextBox.Location = New System.Drawing.Point(16, 95)
        Me.XTextBox.Name = "XTextBox"
        Me.XTextBox.Size = New System.Drawing.Size(136, 20)
        Me.XTextBox.TabIndex = 2
        Me.XTextBox.Text = "2"
        '
        'braceLabel
        '
        Me.braceLabel.Location = New System.Drawing.Point(240, 127)
        Me.braceLabel.Name = "braceLabel"
        Me.braceLabel.Size = New System.Drawing.Size(120, 23)
        Me.braceLabel.TabIndex = 28
        Me.braceLabel.Text = "Type of Braces:"
        '
        'beamLabel
        '
        Me.beamLabel.Location = New System.Drawing.Point(240, 71)
        Me.beamLabel.Name = "beamLabel"
        Me.beamLabel.Size = New System.Drawing.Size(120, 23)
        Me.beamLabel.TabIndex = 27
        Me.beamLabel.Text = "Type of Beams:"
        '
        'columnLabel
        '
        Me.columnLabel.Location = New System.Drawing.Point(240, 15)
        Me.columnLabel.Name = "columnLabel"
        Me.columnLabel.Size = New System.Drawing.Size(120, 23)
        Me.columnLabel.TabIndex = 26
        Me.columnLabel.Text = "Type of Columns:"
        '
        'braceComboBox
        '
        Me.braceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.braceComboBox.Location = New System.Drawing.Point(240, 151)
        Me.braceComboBox.Name = "braceComboBox"
        Me.braceComboBox.Size = New System.Drawing.Size(288, 21)
        Me.braceComboBox.TabIndex = 7
        '
        'beamComboBox
        '
        Me.beamComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.beamComboBox.Location = New System.Drawing.Point(240, 95)
        Me.beamComboBox.Name = "beamComboBox"
        Me.beamComboBox.Size = New System.Drawing.Size(288, 21)
        Me.beamComboBox.TabIndex = 6
        '
        'columnComboBox
        '
        Me.columnComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.columnComboBox.Location = New System.Drawing.Point(240, 39)
        Me.columnComboBox.Name = "columnComboBox"
        Me.columnComboBox.Size = New System.Drawing.Size(288, 21)
        Me.columnComboBox.TabIndex = 5
        '
        'OKButton
        '
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.Location = New System.Drawing.Point(280, 208)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 8
        Me.OKButton.Text = "&OK"
        '
        'unitLabel
        '
        Me.unitLabel.Location = New System.Drawing.Point(158, 40)
        Me.unitLabel.Name = "unitLabel"
        Me.unitLabel.Size = New System.Drawing.Size(24, 16)
        Me.unitLabel.TabIndex = 34
        Me.unitLabel.Text = "feet"
        '
        'CreateBeamsColumnsBracesForm
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.cancelButton1
        Me.ClientSize = New System.Drawing.Size(544, 244)
        Me.Controls.Add(Me.unitLabel)
        Me.Controls.Add(Me.cancelButton1)
        Me.Controls.Add(Me.floornumberLabel)
        Me.Controls.Add(Me.XLabel)
        Me.Controls.Add(Me.YLabel)
        Me.Controls.Add(Me.DistanceLabel)
        Me.Controls.Add(Me.floornumberTextBox)
        Me.Controls.Add(Me.DistanceTextBox)
        Me.Controls.Add(Me.YTextBox)
        Me.Controls.Add(Me.XTextBox)
        Me.Controls.Add(Me.braceLabel)
        Me.Controls.Add(Me.beamLabel)
        Me.Controls.Add(Me.columnLabel)
        Me.Controls.Add(Me.braceComboBox)
        Me.Controls.Add(Me.beamComboBox)
        Me.Controls.Add(Me.columnComboBox)
        Me.Controls.Add(Me.OKButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CreateBeamsColumnsBracesForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Create Beams Columns and Braces"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub CreateBeamsColumnsBracesForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.columnComboBox.DataSource = m_dataBuffer.ColumnMaps
        Me.columnComboBox.DisplayMember = "SymbolName"
        Me.columnComboBox.ValueMember = "ElementType"

        Me.beamComboBox.DataSource = m_dataBuffer.BeamMaps
        Me.beamComboBox.DisplayMember = "SymbolName"
        Me.beamComboBox.ValueMember = "ElementType"

        Me.braceComboBox.DataSource = m_dataBuffer.BraceMaps
        Me.braceComboBox.DisplayMember = "SymbolName"
        Me.braceComboBox.ValueMember = "ElementType"
    End Sub

    ''' <summary>
    ''' accept use's inpurt and create columns, beams and braces
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OKButton.Click
        'check whether the input is correct and create elements
        Try
            Dim xNumber As Integer = Integer.Parse(Me.XTextBox.Text)
            Dim yNumber As Integer = Integer.Parse(Me.YTextBox.Text)
            Dim distance As Double = Double.Parse(Me.DistanceTextBox.Text)
            Dim columnType As Object = columnComboBox.SelectedValue
            Dim beamType As Object = beamComboBox.SelectedValue
            Dim braceType As Object = braceComboBox.SelectedValue
            Dim floorNumber As Integer = Integer.Parse(floornumberTextBox.Text)

            m_dataBuffer.CreateMatrix(xNumber, yNumber, distance)
            m_dataBuffer.AddInstance(columnType, beamType, braceType, floorNumber)

        Catch
            MessageBox.Show("Please input datas correctly.", "Revit")
        End Try
    End Sub

    ''' <summary>
    ''' cancel the command
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelButton1.Click
    End Sub

    ''' <summary>
    ''' verify the distance
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DistanceTextBox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles DistanceTextBox.Validating
        Try
            Dim distance As Double = Double.Parse(DistanceTextBox.Text)
            If distance <= 0.0 Then
                MessageBox.Show("Please input a double style distance larger than 0.0 feet.", "Revit")
                DistanceTextBox.Text = ""
            End If
        Catch ex As Exception
            MessageBox.Show("Please input a double style distance larger than 0.0 feet.", "Revit")
            DistanceTextBox.Text = ""
        End Try
    End Sub

    ''' <summary>
    ''' verify the number of X direction
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub XTextBox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles XTextBox.Validating
        Try
            Dim xNumber As Integer = Integer.Parse(XTextBox.Text)
            If xNumber < 1 Or xNumber > 20 Then
                MessageBox.Show("Please input an integer for X direction between 1 to 20.", "Revit")
                XTextBox.Text = ""
            End If
        Catch ex As Exception
            MessageBox.Show("Please input an integer for X direction between 1 to 20.", "Revit")
            XTextBox.Text = ""
        End Try
    End Sub

    ''' <summary>
    ''' verify the number of Y direction
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub YTextBox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles YTextBox.Validating
        Try
            Dim yNumber As Integer = Integer.Parse(YTextBox.Text)
            If yNumber < 1 Or yNumber > 20 Then
                MessageBox.Show("Please input an integer for Y direction between 1 to 20.", "Revit")
                YTextBox.Text = ""
            End If
        Catch ex As Exception
            MessageBox.Show("Please input an integer for Y direction between 1 to 20.", "Revit")
            YTextBox.Text = ""
        End Try
    End Sub

    ''' <summary>
    ''' verify the number of floors
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub floornumberTextBox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles floornumberTextBox.Validating
        Try
            Dim floorNumber As Integer = Integer.Parse(floornumberTextBox.Text)
            If floorNumber < 1 Or floorNumber > 10 Then
                MessageBox.Show("Please input an integer for the number of floors between 1 to 10.", "Revit")
                floornumberTextBox.Text = ""
            End If
        Catch ex As Exception
            MessageBox.Show("Please input an integer for the number of floors between 1 to 10.", "Revit")
            floornumberTextBox.Text = ""
        End Try
    End Sub
End Class
