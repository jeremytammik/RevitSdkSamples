#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2007-2011 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software
' for any purpose and without fee is hereby granted, provided
' that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
#End Region

#Region "Namespaces"
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
#End Region

Namespace XtraVb

  Public Class Lab3_4_Form

    ' Members
    Private _dict As Dictionary(Of String, List(Of FamilySymbol))

    ' C-tor
    Public Sub New(ByVal dict As Dictionary(Of String, List(Of FamilySymbol)))

      MyBase.New()
      InitializeComponent()
      _dict = dict

      ' Init Family combo box
      'For Each kpv As KeyValuePair(Of String, ArrayList) In _dict.Keys
      '    cmbFamily.Items.Add(kpv.Key)
      'Next
      For Each key As String In _dict.Keys
        cmbFamily.Items.Add(key)
      Next
      cmbFamily.SelectedIndex = 0

      ' Init Type Combo
      cmbFamily_SelectedIndexChanged(Me, Nothing)
      Me.Text = "Select a " + CType(cmbType.SelectedItem, FamilySymbol).Category.Name + " Type"

    End Sub

    'Events
    Private Sub cmbFamily_SelectedIndexChanged( _
        ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles cmbFamily.SelectedIndexChanged

      Try
        cmbType.DataSource = _dict(cmbFamily.Text)
        cmbType.DisplayMember = "Name"
        'cmbType.ValueMember = "Id"
        cmbType.SelectedIndex = 0
      Catch
      End Try
    End Sub

    Private Sub btnOK_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs) _
        Handles btnOK.Click
      'Try
      '    MsgBox("Type = " + cmbType.Text + ", id = " + CType(cmbType.SelectedItem, FamilySymbol).Id.Value)
      'Catch ex As Exception
      '    MsgBox(ex.Message)
      'End Try
    End Sub
  End Class

End Namespace
