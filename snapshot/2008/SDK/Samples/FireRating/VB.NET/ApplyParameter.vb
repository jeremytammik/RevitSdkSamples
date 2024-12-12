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

Public Class ApplyParameter
    ' All Autodesk Revit external commands must support this interface
    Implements Autodesk.Revit.IExternalCommand

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
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute

        Dim sharedParametersFile As Autodesk.Revit.Parameters.DefinitionFile = OpenSharedParamFile(commandData.Application)

        If sharedParametersFile Is Nothing Then
            message = "Unable to open shared parameters file"
            Return Autodesk.Revit.IExternalCommand.Result.Failed
        End If


        Dim fireGroup As Autodesk.Revit.Parameters.DefinitionGroup = OpenGroup(sharedParametersFile)

        If (fireGroup Is Nothing) Then
            message = "Unable to create fire group"
            Return Autodesk.Revit.IExternalCommand.Result.Failed
        End If

        Dim definition As Autodesk.Revit.Parameters.Definition = OpenDefinition(fireGroup)

        If (definition Is Nothing) Then
            message = "Unable to create fire rating parameter definition"
            Return Autodesk.Revit.IExternalCommand.Result.Failed
        End If


        Dim category As Autodesk.Revit.Category = commandData.Application.ActiveDocument.Settings.Categories.Item(mCategoryName)
        Dim categorySet As Autodesk.Revit.CategorySet = commandData.Application.Create.NewCategorySet()
        categorySet.Insert(category)

        Dim binding As Autodesk.Revit.Parameters.Binding = commandData.Application.Create.NewInstanceBinding(categorySet)

        commandData.Application.ActiveDocument.ParameterBindings.Insert(definition, binding)

        MsgBox("Applied shared parameters to the doors. Check the properties of doors.")

        Return Autodesk.Revit.IExternalCommand.Result.Succeeded

    End Function


  Private Function OpenSharedParamFile(ByVal application As Autodesk.Revit.Application) As Autodesk.Revit.Parameters.DefinitionFile

    Const fullPath As String = mPath + "\" + mFilename

    Dim stream As StreamWriter

    stream = New StreamWriter(fullPath)
    stream.Close()

    application.Options.SharedParametersFilename = fullPath

    Dim sharedParametersFile As Autodesk.Revit.Parameters.DefinitionFile
    sharedParametersFile = Nothing

    On Error Resume Next
    sharedParametersFile = application.OpenSharedParameterFile
    On Error GoTo 0

    Return sharedParametersFile

  End Function

  Private Function OpenGroup(ByVal sharedParametersFile As Autodesk.Revit.Parameters.DefinitionFile) As Autodesk.Revit.Parameters.DefinitionGroup

    Dim fireGroup As Autodesk.Revit.Parameters.DefinitionGroup
    fireGroup = sharedParametersFile.Groups.Item(mGroupName)
    If (fireGroup Is Nothing) Then
      fireGroup = sharedParametersFile.Groups.Create(mGroupName)
    End If

    Return fireGroup

  End Function

  Private Function OpenDefinition(ByVal fireGroup As Autodesk.Revit.Parameters.DefinitionGroup) As Autodesk.Revit.Parameters.Definition

    Dim definition As Autodesk.Revit.Parameters.Definition = fireGroup.Definitions.Item(mParameterName)
    If definition Is Nothing Then
            definition = fireGroup.Definitions.Create(mParameterName, Autodesk.Revit.Parameters.ParameterType.Integer)
    End If

    Return definition
  End Function
End Class
