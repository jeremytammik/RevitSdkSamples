'
' (C) Copyright 2003-2009 by Autodesk, Inc. 
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
Imports System.Data
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
'Summary description for MaterialPropFrm.
Public Class MaterialPropertiesForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal dataBuffer As MaterialProperties)
        MyBase.New()

        'This call is required by the Windows Form Designer.
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
    Friend WithEvents changeButton As System.Windows.Forms.Button
    Friend WithEvents applyButton As System.Windows.Forms.Button
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents parameterDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents subTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents typeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents typeLable As System.Windows.Forms.Label
    Friend WithEvents exitButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.changeButton = New System.Windows.Forms.Button
        Me.applyButton = New System.Windows.Forms.Button
        Me.exitButton = New System.Windows.Forms.Button
        Me.okButton = New System.Windows.Forms.Button
        Me.parameterDataGrid = New System.Windows.Forms.DataGrid
        Me.subTypeComboBox = New System.Windows.Forms.ComboBox
        Me.typeComboBox = New System.Windows.Forms.ComboBox
        Me.typeLable = New System.Windows.Forms.Label
        CType(Me.parameterDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'changeButton
        '
        Me.changeButton.Location = New System.Drawing.Point(353, 455)
        Me.changeButton.Name = "changeButton"
        Me.changeButton.Size = New System.Drawing.Size(128, 23)
        Me.changeButton.TabIndex = 16
        Me.changeButton.Text = "Change &Unit Weight"
        '
        'applyButton
        '
        Me.applyButton.Location = New System.Drawing.Point(267, 455)
        Me.applyButton.Name = "applyButton"
        Me.applyButton.Size = New System.Drawing.Size(75, 23)
        Me.applyButton.TabIndex = 15
        Me.applyButton.Text = "&Apply"
        '
        'exitButton
        '
        Me.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.exitButton.Location = New System.Drawing.Point(173, 455)
        Me.exitButton.Name = "exitButton"
        Me.exitButton.Size = New System.Drawing.Size(75, 23)
        Me.exitButton.TabIndex = 14
        Me.exitButton.Text = "&Cancel"
        '
        'okButton
        '
        Me.okButton.Location = New System.Drawing.Point(87, 455)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(75, 23)
        Me.okButton.TabIndex = 13
        Me.okButton.Text = "&OK"
        '
        'parameterDataGrid
        '
        Me.parameterDataGrid.CaptionVisible = False
        Me.parameterDataGrid.DataMember = ""
        Me.parameterDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.parameterDataGrid.Location = New System.Drawing.Point(12, 69)
        Me.parameterDataGrid.Name = "parameterDataGrid"
        Me.parameterDataGrid.ReadOnly = True
        Me.parameterDataGrid.RowHeadersVisible = False
        Me.parameterDataGrid.Size = New System.Drawing.Size(480, 380)
        Me.parameterDataGrid.TabIndex = 12
        '
        'subTypeComboBox
        '
        Me.subTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.subTypeComboBox.Location = New System.Drawing.Point(100, 42)
        Me.subTypeComboBox.Name = "subTypeComboBox"
        Me.subTypeComboBox.Size = New System.Drawing.Size(264, 21)
        Me.subTypeComboBox.TabIndex = 11
        '
        'typeComboBox
        '
        Me.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.typeComboBox.Location = New System.Drawing.Point(100, 7)
        Me.typeComboBox.Name = "typeComboBox"
        Me.typeComboBox.Size = New System.Drawing.Size(264, 21)
        Me.typeComboBox.TabIndex = 10
        '
        'typeLable
        '
        Me.typeLable.Location = New System.Drawing.Point(13, 7)
        Me.typeLable.Name = "typeLable"
        Me.typeLable.Size = New System.Drawing.Size(80, 23)
        Me.typeLable.TabIndex = 9
        Me.typeLable.Text = "Material Type:"
        '
        'MaterialPropertiesForm
        '
        Me.AcceptButton = Me.okButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.exitButton
        Me.ClientSize = New System.Drawing.Size(502, 481)
        Me.Controls.Add(Me.changeButton)
        Me.Controls.Add(Me.applyButton)
        Me.Controls.Add(Me.exitButton)
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me.parameterDataGrid)
        Me.Controls.Add(Me.subTypeComboBox)
        Me.Controls.Add(Me.typeComboBox)
        Me.Controls.Add(Me.typeLable)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MaterialPropertiesForm"
        Me.ShowInTaskbar = False
        Me.Text = "Material Properties"
        CType(Me.parameterDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Dim m_dataBuffer As MaterialProperties = Nothing

    'set selected element's material to current selection and close form
    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        If Not (subTypeComboBox.SelectedValue Is Nothing) Then
            m_dataBuffer.SetMaterial(subTypeComboBox.SelectedValue)
        End If
        Me.Close()
    End Sub

    'close form
    Private Sub cancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitButton.Click
        Me.Close()
    End Sub

    'set selected element's material to current selection
    Private Sub applyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles applyButton.Click
        If Not (subTypeComboBox.SelectedValue Is Nothing) Then
            m_dataBuffer.SetMaterial(subTypeComboBox.SelectedValue)
        End If
    End Sub

    'change unit weight all instances of the elements that use this material
    Private Sub changeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles changeButton.Click
        MessageBox.Show("This will change unit weight of all instances that use this material.")

        If Not (m_dataBuffer.ChangeUnitWeight()) Then
            MessageBox.Show("Failed to change the unit weight.")
            Return
        End If
        LoadCurrentMaterial()
    End Sub

    ' when typeComboBox changed, then update the subTypeCombobox 
    Private Sub typeComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles typeComboBox.SelectedIndexChanged
        If (CType(typeComboBox.SelectedIndex, MaterialType) = MaterialType.Steel) Then
            applyButton.Enabled = True
            changeButton.Enabled = True
            subTypeComboBox.Enabled = True
            subTypeComboBox.DataSource = m_dataBuffer.SteelCollection
            subTypeComboBox.DisplayMember = "MaterialName"
            subTypeComboBox.ValueMember = "Material"
            parameterDataGrid.DataSource = m_dataBuffer.GetParameterTable(subTypeComboBox.SelectedValue, CType(typeComboBox.SelectedIndex, MaterialType))
        ElseIf (CType(typeComboBox.SelectedIndex, MaterialType) = MaterialType.Concrete) Then
            applyButton.Enabled = True
            changeButton.Enabled = True
            subTypeComboBox.Enabled = True
            subTypeComboBox.DataSource = m_dataBuffer.ConcreteCollection
            subTypeComboBox.DisplayMember = "MaterialName"
            subTypeComboBox.ValueMember = "Material"
            parameterDataGrid.DataSource = m_dataBuffer.GetParameterTable(subTypeComboBox.SelectedValue, CType(typeComboBox.SelectedIndex, MaterialType))
        ElseIf (CType(typeComboBox.SelectedIndex, MaterialType) = MaterialType.Generic) Then
            applyButton.Enabled = False
            changeButton.Enabled = False
            subTypeComboBox.DataSource = New ArrayList
            subTypeComboBox.Enabled = False
            parameterDataGrid.DataSource = New DataTable
        End If

        If typeComboBox.SelectedIndex = CInt(m_dataBuffer.CurrentType) Then
            If (m_dataBuffer.CurrentMaterial Is Nothing Or m_dataBuffer.CurrentType = MaterialType.Generic) Then
                Return
            End If

            Dim tmp As Autodesk.Revit.Elements.Material
            tmp = m_dataBuffer.CurrentMaterial
            If (tmp Is Nothing) Then
                Return
            End If
            subTypeComboBox.SelectedValue = tmp
            parameterDataGrid.DataSource = m_dataBuffer.GetParameterTable(subTypeComboBox.SelectedValue, _
            CType(typeComboBox.SelectedIndex, MaterialType))
        ElseIf subTypeComboBox.Items.Count = 0 Then
            parameterDataGrid.DataSource = New DataTable
        End If
    End Sub

    'change the content in datagrid according to selected material type
    Private Sub subTypeComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles subTypeComboBox.SelectedIndexChanged
        If subTypeComboBox.SelectedValue Is Nothing Then
            parameterDataGrid.DataSource = New DataTable
        End If
        parameterDataGrid.DataSource = m_dataBuffer.GetParameterTable(subTypeComboBox.SelectedValue, CType(typeComboBox.SelectedIndex, MaterialType))
    End Sub

    'update display data to selected element's material
    Private Sub LoadCurrentMaterial()
        typeComboBox.DataSource = m_dataBuffer.MaterialTypes
        typeComboBox.SelectedIndex = CInt(m_dataBuffer.CurrentType)
        If (m_dataBuffer.CurrentMaterial Is Nothing Or m_dataBuffer.CurrentType = MaterialType.Generic) Then
            Return
        End If


        Dim tmp As Autodesk.Revit.Elements.Material
        tmp = m_dataBuffer.CurrentMaterial
        If (tmp Is Nothing) Then
            Return
        End If
        subTypeComboBox.SelectedValue = tmp
        parameterDataGrid.DataSource = m_dataBuffer.GetParameterTable(subTypeComboBox.SelectedValue, _
        CType(typeComboBox.SelectedIndex, MaterialType))
    End Sub

    ' when the form loading, then load the current material of your selected beam, column or brace
    Private Sub MaterialPropertiesForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        parameterDataGrid.PreferredColumnWidth = parameterDataGrid.Width / 2 - 2
        LoadCurrentMaterial()
    End Sub
End Class
