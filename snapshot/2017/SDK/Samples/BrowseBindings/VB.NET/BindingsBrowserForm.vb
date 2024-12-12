' 
' (C) Copyright 2003-2016 by Autodesk, Inc.
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
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
' 
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'
Option Explicit On
Imports System.Runtime

Public Class BindingsBrowserForm
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
    Friend WithEvents closeButton As System.Windows.Forms.Button

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents BindingsTree As System.Windows.Forms.TreeView
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.BindingsTree = New System.Windows.Forms.TreeView
        Me.closeButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'BindingsTree
        '
        Me.BindingsTree.Indent = 38
        Me.BindingsTree.Location = New System.Drawing.Point(8, 8)
        Me.BindingsTree.Name = "BindingsTree"
        Me.BindingsTree.Size = New System.Drawing.Size(288, 321)
        Me.BindingsTree.TabIndex = 0
        '
        'closeButton
        '
        Me.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.closeButton.Location = New System.Drawing.Point(107, 340)
        Me.closeButton.Name = "closeButton"
        Me.closeButton.Size = New System.Drawing.Size(75, 23)
        Me.closeButton.TabIndex = 1
        Me.closeButton.Text = "&Close"
        Me.closeButton.UseVisualStyleBackColor = True
        '
        'BindingsBrowserForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.closeButton
        Me.ClientSize = New System.Drawing.Size(307, 373)
        Me.Controls.Add(Me.closeButton)
        Me.Controls.Add(Me.BindingsTree)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BindingsBrowserForm"
        Me.ShowInTaskbar = False
        Me.Text = "Bindings Browser"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private m_application As Autodesk.Revit.UI.UIApplication
    Private m_initialized As Boolean

    ''' <summary>
    ''' Gets the Revit application and retrieves the ParameterBindingsMap
    ''' Then loops the map to get all parameter bindings
    ''' At last displays them in the tree view
    ''' </summary>
    ''' <param name="Application">Revit application</param>
    ''' <remarks></remarks>
    Public Function Initialize(ByVal Application As Autodesk.Revit.UI.UIApplication) As Boolean

        Try
            m_application = Application
            If Not (m_application Is Nothing) Then
                m_initialized = True
            End If

            Return LoadBindings()
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public ReadOnly Property Initialized() As Boolean
        Get
            Initialized = m_initialized
        End Get
    End Property

    Public ReadOnly Property Document() As Autodesk.Revit.DB.Document
        Get
            Document = m_application.ActiveUIDocument.Document
        End Get
    End Property

    Public ReadOnly Property Application() As Autodesk.Revit.UI.UIApplication
        Get
            Application = m_application
        End Get
    End Property

    ''' <summary>
    ''' retrieves the ParameterBindingsMap
    ''' Then loops the map to get all bindings
    ''' At last displays them in the tree view
    ''' </summary>
    ''' <remarks></remarks>
    Private Function LoadBindings() As Boolean
        Try
            Dim bindingsRoot As System.Windows.Forms.TreeNode
            bindingsRoot = Me.BindingsTree.Nodes.Add("Bindings")

            Dim bindingsMap As Autodesk.Revit.DB.BindingMap
            bindingsMap = Document.ParameterBindings

            Dim iterator As Autodesk.Revit.DB.DefinitionBindingMapIterator
            iterator = bindingsMap.ForwardIterator

            Do While (iterator.MoveNext)
                Dim elementBinding As Autodesk.Revit.DB.ElementBinding
                elementBinding = iterator.Current

                ' get the name of the parameter 
                Dim definition As Autodesk.Revit.DB.Definition
                definition = iterator.Key

                Dim definitionNode As System.Windows.Forms.TreeNode = Nothing

                '  Note: the description of parameter binding is as follows:
                '  "a parameter definition is bound to elements within one or more categories."
                '  But this seems to return a one-to-one map.  
                '  The following for loop is a workaround. 

                '  do we have it in the node? 
                '  if yes, use the exisiting one. 
                Dim node As System.Windows.Forms.TreeNode
                For Each node In bindingsRoot.Nodes
                    If (node.Text = definition.Name) Then
                        definitionNode = node
                    End If
                Next

                ' if the new parameter, add a new node. 
                If (definitionNode Is Nothing) Then
                    definitionNode = bindingsRoot.Nodes.Add(definition.Name)
                End If

                ' add the category name.  
                If (Not elementBinding Is Nothing) Then
                    Dim categories As Autodesk.Revit.DB.CategorySet
                    categories = elementBinding.Categories

                    Dim category As Autodesk.Revit.DB.Category
                    For Each category In categories
                        If (Not category Is Nothing) Then
                            definitionNode.Nodes.Add(category.Name)
                        End If
                    Next
                End If
            Loop

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Sub Form_Load()

        If Not (Me.Initialized) Then
            Exit Sub
        End If

        LoadBindings()

    End Sub

End Class
