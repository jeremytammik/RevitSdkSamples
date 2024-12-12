'
' (C) Copyright 2003-2008 by Autodesk, Inc.
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

Imports Autodesk.Revit
Imports Autodesk.Revit.Geometry
Imports Autodesk.Revit.Elements


''' <summary>
''' this class corresponding to the first external command used to create a wall
''' </summary>
''' <remarks></remarks>
Public Class CreateWall
    Implements IExternalCommand


    ''' <summary>
    ''' Implement this method as an external command for Revit.
    ''' </summary>
    ''' <param name="revit">An object that is passed to the external application 
    ''' which contains data related to the command, 
    ''' such as the application object and active view.</param>
    ''' <param name="message">A message that can be set by the external application 
    ''' which will be displayed if a failure or cancellation is returned by 
    ''' the external command.</param>
    ''' <param name="elements">A set of elements to which the external application 
    ''' can add elements that are to be highlighted in case of failure or cancellation.</param>
    ''' <returns>Return the status of the external command. 
    ''' A result of Succeeded means that the API external method functioned as expected. 
    ''' Cancelled can be used to signify that the user cancelled the external operation 
    ''' at some point. Failure should be returned if the application is unable to proceed with 
    ''' the operation.</returns>
    Public Function Execute(ByVal revit As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As IExternalCommand.Result Implements IExternalCommand.Execute

        Dim app As Application = revit.Application
        Dim creApp As Creation.Application = app.Create
        Dim creDoc As Creation.Document = app.ActiveDocument.Create

        ' prepare the wall base line
        Dim startPoint As Autodesk.Revit.Geometry.XYZ = New Autodesk.Revit.Geometry.XYZ(0, 0, 0)
        Dim endPoint As Autodesk.Revit.Geometry.XYZ = New Autodesk.Revit.Geometry.XYZ(50, 50, 0)
        Dim newLine As Line = creApp.NewLine(startPoint, endPoint, True)

        'prepare the level where the wall will be located
        Dim iter As ElementIterator = app.ActiveDocument.Elements
        While (iter.MoveNext())
            Dim level1 As Elements.Level = Nothing

            If TypeOf iter.Current Is Elements.Level Then
                level1 = iter.Current
            Else
                Continue While
            End If

            If Not level1.Name.Equals("Level 1") Then
                Continue While
            End If

            ' create a wall in level1
            Dim newWall As Wall = creDoc.NewWall(newLine, level1, True)
        End While

        Return IExternalCommand.Result.Succeeded
    End Function
End Class

''' <summary>
''' this class corresponding to the second external command used to 
''' delete all the walls in current Revit project.
''' </summary>
''' <remarks></remarks>
Public Class DeleteWalls
    Implements IExternalCommand

    ''' <summary>
    ''' Implement this method as an external command for Revit.
    ''' </summary>
    ''' <param name="revit">An object that is passed to the external application 
    ''' which contains data related to the command, 
    ''' such as the application object and active view.</param>
    ''' <param name="message">A message that can be set by the external application 
    ''' which will be displayed if a failure or cancellation is returned by 
    ''' the external command.</param>
    ''' <param name="elements">A set of elements to which the external application 
    ''' can add elements that are to be highlighted in case of failure or cancellation.</param>
    ''' <returns>Return the status of the external command. 
    ''' A result of Succeeded means that the API external method functioned as expected. 
    ''' Cancelled can be used to signify that the user cancelled the external operation 
    ''' at some point. Failure should be returned if the application is unable to proceed with 
    ''' the operation.</returns>
    Public Function Execute(ByVal revit As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As IExternalCommand.Result Implements IExternalCommand.Execute

        Dim doc As Document = revit.Application.ActiveDocument
        Dim wallsToDel As New ElementSet
        Dim iter As ElementIterator = doc.Elements
        Do While iter.MoveNext
            Dim wall As Wall = TryCast(iter.Current, Wall)
            If (Not Nothing Is wall) Then
                wallsToDel.Insert(wall)
            End If
        Loop
        doc.Delete(wallsToDel)

        Return IExternalCommand.Result.Succeeded
    End Function
End Class

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class AboutAPIToolbar
    Implements IExternalCommand

    Public ReadOnly Property AboutToolbar() As Dictionary(Of String, String)
        Get
            Return m_aboutToolbar
        End Get
    End Property

    Public ReadOnly Property DifToolbarItemUsage() As Dictionary(Of String, String)
        Get
            Return m_difToolbarItemUsage
        End Get
    End Property

    Shared Sub New()
        Dim backdrop As String = "Maybe you need a manner in which you can add your own designed Toolbar into Revit." + _
                                 "Formerly you can only add your applications to the External Tools' submenu. " + _
                                 "But now your applications can integrate seamlessly into the platform and " + _
                                 "make them look like actually created by the Revit Team."
        Dim createToolbar As String = "Firstly, use Application.CreateToolbar to create a Toolbar." + Chr(10) + _
                                      "Then, use Toolbar.AddItem to add a ToolbarItem button which corresponds to an ExternalCommand. " + Chr(10) + _
                                      "ToolbarItem button has three types which will be introduced later."
        Dim getToolbarItem As String = "Firstly, use Application.GetToolbars to return a ToolbarArray which includes all the custom Toolbars." + Chr(10) + _
                                       "Secondly, you can get each custom Toolbar from the ToolbarArray." + Chr(10) + _
                                       "Then, use Toolbar.ToolbarItems to return a ToolbarItemArray which returns all the custom ToolbarItems in this Toolbar." + Chr(10) + _
                                       "Finally, you can get each ToolbarItem in this ToolbarItemArray"
        Dim others As String = "Every button should have a corresponding command to execute. " + _
                               "And the command should implement the interface of IExternalCommand." + Chr(10) + _
                               "Toolbar and ToolbarItem should be added when Revit starts up, so we must add Toolbar and ToolbarItem in External Application"
        m_aboutToolbar.Add("Backdrop in API", backdrop)
        m_aboutToolbar.Add("How to create custom Toolbar", createToolbar)
        m_aboutToolbar.Add("Get existent toolbarItem", getToolbarItem)
        m_aboutToolbar.Add("Others", others)

        Dim stdBtn As String = "Standard button" + Chr(10) + _
                                "Usage:  button with image" + Chr(10) + _
                                "Create: Toolbar.AddItem(MenuItem) or Toolbar.AddItem(string pAssemblyName, string pClassName)"
        Dim textBtn As String = "Rich text button" + Chr(10) + _
                                "Usage:  button with both text and image" + Chr(10) + _
                                "Create: Toolbar.AddItem(MenuItem) or Toolbar.AddItem(string pAssemblyName, string pClassName)" + _
                                "Then, set ToolbarItem.ItemType with ToolbarItem.ToolbarItemType.BtnRText and set ToolbarItem.ItemText"
        Dim separBtn As String = "Button separator" + Chr(10) + "Usage: a separator line" + Chr(10) + "Create: Toolbar.AddItem(nullptr)"
        m_difToolbarItemUsage.Add(ToolbarItem.ToolbarItemType.BtnStd.ToString(), stdBtn)
        m_difToolbarItemUsage.Add(ToolbarItem.ToolbarItemType.BtnRText.ToString(), textBtn)
        m_difToolbarItemUsage.Add(ToolbarItem.ToolbarItemType.BtnSeparator.ToString(), separBtn)
    End Sub

    ''' <summary>
    ''' Implement this method as an external command for Revit.
    ''' </summary>
    ''' <param name="commandData">An object that is passed to the external application 
    ''' which contains data related to the command, 
    ''' such as the application object and active view.</param>
    ''' <param name="message">A message that can be set by the external application 
    ''' which will be displayed if a failure or cancellation is returned by 
    ''' the external command.</param>
    ''' <param name="elements">A set of elements to which the external application 
    ''' can add elements that are to be highlighted in case of failure or cancellation.</param>
    ''' <returns>Return the status of the external command. 
    ''' A result of Succeeded means that the API external method functioned as expected. 
    ''' Cancelled can be used to signify that the user cancelled the external operation 
    ''' at some point. Failure should be returned if the application is unable to proceed with 
    ''' the operation.</returns>
    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As IExternalCommand.Result Implements IExternalCommand.Execute

        'show UI
        Dim displayForm As AboutAPIToolbarForm = New AboutAPIToolbarForm(Me)

        Using (displayForm)
            displayForm.ShowDialog()
        End Using

        Return IExternalCommand.Result.Succeeded

    End Function

    Private Shared m_aboutToolbar As Dictionary(Of String, String) = New Dictionary(Of String, String)()
    Private Shared m_difToolbarItemUsage As Dictionary(Of String, String) = New Dictionary(Of String, String)()
End Class
