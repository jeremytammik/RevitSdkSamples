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
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.?AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'


Option Explicit On

''' <summary>
''' This class display all the Wall (FamilyInstance) types in project
''' and change the element's type selected by user.
''' </summary>
''' <remarks></remarks>
Public Class TypeSelectorWindow
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "
    ''' <summary>
    ''' new form method
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    ''' <summary>
    ''' Form overrides dispose to clean up the component list.
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
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
    Friend WithEvents TypeList As System.Windows.Forms.ListBox
    Friend WithEvents oKButton As System.Windows.Forms.Button
    Friend Shadows WithEvents cancelButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TypeList = New System.Windows.Forms.ListBox
        Me.oKButton = New System.Windows.Forms.Button
        Me.cancelButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'TypeList
        '
        Me.TypeList.Location = New System.Drawing.Point(12, 16)
        Me.TypeList.Name = "TypeList"
        Me.TypeList.Size = New System.Drawing.Size(268, 121)
        Me.TypeList.Sorted = True
        Me.TypeList.TabIndex = 0
        '
        'oKButton
        '
        Me.oKButton.Location = New System.Drawing.Point(115, 170)
        Me.oKButton.Name = "oKButton"
        Me.oKButton.Size = New System.Drawing.Size(75, 23)
        Me.oKButton.TabIndex = 1
        Me.oKButton.Text = "&Ok"
        '
        'cancelButton
        '
        Me.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancelButton.Location = New System.Drawing.Point(205, 170)
        Me.cancelButton.Name = "cancelButton"
        Me.cancelButton.Size = New System.Drawing.Size(75, 23)
        Me.cancelButton.TabIndex = 2
        Me.cancelButton.Text = "&Cancel"
        '
        'TypeSelectorWindow
        '
        Me.AcceptButton = Me.oKButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.cancelButton = Me.cancelButton
        Me.ClientSize = New System.Drawing.Size(292, 205)
        Me.Controls.Add(Me.cancelButton)
        Me.Controls.Add(Me.oKButton)
        Me.Controls.Add(Me.TypeList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TypeSelectorWindow"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Type Selector"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private m_document As Autodesk.Revit.DB.Document
    Private m_elementId As Autodesk.Revit.DB.ElementId
    Public m_result As Autodesk.Revit.UI.Result
    Public m_resultMessage As String

    ''' <summary>
    ''' Initialization and store the document and component
    ''' </summary>
    ''' <param name="document">revit document</param>
    ''' <param name="element">elements of revit</param>
    ''' <remarks></remarks>
    Public Sub Initialise(ByVal document As Autodesk.Revit.DB.Document, ByVal element As Autodesk.Revit.DB.Element)
        'store the document and component
        m_document = document
        m_elementId = element.Id
        Call Form_Load()

    End Sub

    ''' <summary>
    ''' form initialize
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Form_Initialize()
        m_result = Autodesk.Revit.UI.Result.Cancelled

    End Sub

    ''' <summary>
    ''' load method for form and get all avaiablable types in project
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Form_Load()

        Dim element As Autodesk.Revit.DB.Element

        element = m_document.GetElement(m_elementId)

        ' components
        If (TypeOf element Is Autodesk.Revit.DB.FamilyInstance) Then
            Dim component As Autodesk.Revit.DB.FamilyInstance
            Dim family As Autodesk.Revit.DB.Family

            component = element

            'get the family
            family = component.Symbol.Family

            'populate the list with all the symbols in that family ...
         Dim familySymbol As Autodesk.Revit.DB.FamilySymbol
         Dim familySymbolId As Autodesk.Revit.DB.ElementId
         For Each familySymbolId In family.GetFamilySymbolIds()
            familySymbol = m_document.GetElement(familySymbolId)
            Me.TypeList.Items.Add(New Mylist(familySymbol.Name, familySymbol.Id.IntegerValue))
            'Me.TypeList.ItemData(Me.TypeList.ListCount - 1) = familySymbol.Id.IntegerValue
            If (component.Symbol.Id.IntegerValue = familySymbol.Id.IntegerValue) Then
               Me.TypeList.SelectedIndex = Me.TypeList.Items.Count - 1
            End If
         Next

            ' walls
        ElseIf (TypeOf element Is Autodesk.Revit.DB.Wall) Then
            Dim wall As Autodesk.Revit.DB.Wall
            Dim wallType As Autodesk.Revit.DB.WallType

            wall = element

            'get the wall type
            wallType = wall.WallType

            Dim otherWallType As Autodesk.Revit.DB.WallType
            Dim filteredElementCollector As Autodesk.Revit.DB.FilteredElementCollector
            filteredElementCollector = New Autodesk.Revit.DB.FilteredElementCollector(m_document)
            filteredElementCollector.OfClass(GetType(Autodesk.Revit.DB.WallType))
            Dim wallTypeElement As Autodesk.Revit.DB.Element

            For Each wallTypeElement In filteredElementCollector
                otherWallType = TryCast(wallTypeElement, Autodesk.Revit.DB.WallType)
                Me.TypeList.Items.Add(New Mylist(otherWallType.Name, otherWallType.Id.IntegerValue))
                If (wallType.Id.IntegerValue = otherWallType.Id.IntegerValue) Then
                    Me.TypeList.SelectedIndex = Me.TypeList.Items.Count - 1
                End If
            Next

        End If

    End Sub

    ''' <summary>
    ''' Change the Wall's type to selected type
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Overloads Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oKButton.Click
        Dim mList As Mylist

        ' Get the selected item. 
        mList = Me.TypeList.Items(Me.TypeList.SelectedIndex)

        Dim typeId As Autodesk.Revit.DB.ElementId = New Autodesk.Revit.DB.ElementId(mList.ItemData)

        Dim element As Autodesk.Revit.DB.Element
        element = m_document.GetElement(m_elementId)

        ' get all family instance type
        If (TypeOf element Is Autodesk.Revit.DB.FamilyInstance) Then
            Dim component As Autodesk.Revit.DB.FamilyInstance
            Dim familySymbol As Autodesk.Revit.DB.FamilySymbol

            component = element
            familySymbol = m_document.GetElement(typeId)

            component.Symbol = familySymbol

            ' get all types of Wall
        ElseIf (TypeOf element Is Autodesk.Revit.DB.Wall) Then
            Dim wall As Autodesk.Revit.DB.Wall
            Dim wallType As Autodesk.Revit.DB.WallType

            wall = element
            wallType = m_document.GetElement(typeId)
            wall.WallType = wallType

        End If

        ' result is succeeded
        m_result = Autodesk.Revit.UI.Result.Succeeded
        Me.Hide()
    End Sub

    ''' <summary>
    ''' close the form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Overloads Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelButton.Click
        m_result = Autodesk.Revit.UI.Result.Cancelled
        Me.Hide()
    End Sub
End Class


'TypeList.ItemData (used in VB6 version of this sample)was removed from VB.NET
'To work-around this, we must add a class to hold the list items
'Fore more information, see Microsoft article 311340
'http://support.microsoft.com/default.aspx?scid=kb;en-us;311340

''' <summary>
''' a class to contains types from project
''' </summary>
''' <remarks></remarks>
Public Class Mylist
    Private sName As String ' type name
    Private iID As Integer ' id of type

    ''' <summary>
    ''' new a type list item
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        sName = ""
        iID = 0
    End Sub

    ''' <summary>
    ''' new one list with name and id
    ''' </summary>
    ''' <param name="Name">type name</param>
    ''' <param name="ID">index of one item in all types</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Name As String, ByVal ID As Integer)
        sName = Name
        iID = ID
    End Sub

    ''' <summary>
    ''' get type name
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Name() As String
        Get
            Return sName
        End Get

        Set(ByVal sValue As String)
            sName = sValue
        End Set
    End Property

    ''' <summary>
    ''' get the selected type item index
    ''' </summary>
    ''' <remarks></remarks>
    Public Property ItemData() As Integer
        Get
            Return iID
        End Get
        Set(ByVal iValue As Integer)
            iID = iValue
        End Set
    End Property

    ''' <summary>
    ''' overrides the ToString method
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Return sName
    End Function
End Class