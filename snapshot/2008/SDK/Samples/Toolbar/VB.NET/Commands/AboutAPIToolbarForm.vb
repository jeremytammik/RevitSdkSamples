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

Imports System.Drawing

Public Class AboutAPIToolbarForm

    Public Sub New(ByVal dataBuffer As AboutAPIToolbar)
        MyBase.New()

        InitializeComponent()
        Initialize(dataBuffer)
    End Sub

    Private m_dataBuffer As AboutAPIToolbar
    Private m_captionFont As Font

    Private Sub Initialize(ByVal dataBuffer As AboutAPIToolbar)
        m_dataBuffer = dataBuffer
        m_captionFont = New Font(aboutToolbarRichTextBox.Font.FontFamily, _
                                 aboutToolbarRichTextBox.Font.Size, _
                                 FontStyle.Bold)

        contactLinkLabel.Links.Clear()
        contactLinkLabel.Links.Add(0, contactLinkLabel.Text.Length, "mailto:cxlou@hslcn.com;junyang@hslcn.com")

        Dim aboutToolbar As Dictionary(Of String, String) = m_dataBuffer.AboutToolbar
        Dim difToolbarItemUsage As Dictionary(Of String, String) = m_dataBuffer.DifToolbarItemUsage
        aboutToolbarRichTextBox.Text = ""

        For Each key As String In aboutToolbar.Keys
            AddCaption(key)
            AddBodyText(aboutToolbar(key))
        Next

        For Each key As String In difToolbarItemUsage.Keys
            AddCaption(key)
            AddBodyText(difToolbarItemUsage(key))

        Next
    End Sub

    Private Sub AddCaption(ByVal value As String)
        aboutToolbarRichTextBox.AppendText(value + Chr(10))

        'format the text
        Dim index As Integer = aboutToolbarRichTextBox.Text.IndexOf(value)
        If Not -1 = index Then
            aboutToolbarRichTextBox.Select(index, value.Length)
            aboutToolbarRichTextBox.SelectionFont = m_captionFont
            aboutToolbarRichTextBox.SelectionBullet = True
        End If

    End Sub

    Private Sub AddBodyText(ByVal value As String)
        aboutToolbarRichTextBox.AppendText(value + Chr(10) + Chr(10))

        ' format the text
        'int index  = aboutToolbarRichTextBox.Text.IndexOf(value);
        Dim length As Integer = value.IndexOf(Chr(10))

        If -1 = length Then
            length = value.Length
        End If

        Dim index As Integer = aboutToolbarRichTextBox.Text.IndexOf(value.Substring(0, length))

        If Not -1 = index Then
            aboutToolbarRichTextBox.Select(index, value.Length)
            aboutToolbarRichTextBox.SelectionIndent = 10
        End If
    End Sub

    Private Sub contactLinkLabel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles contactLinkLabel.LinkClicked
        Dim target As String = e.Link.LinkData.ToString()

        If Not target Is Nothing Then
            System.Diagnostics.Process.Start(target)
        End If

    End Sub
End Class