' 
' (C) Copyright 2003-2008 by Autodesk, Inc.
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
Imports System
Imports System.IO
Imports Autodesk.Revit

Public Class Command1
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
    Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, _
        ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result _
        Implements Autodesk.Revit.IExternalCommand.Execute        'set the shared parameters filename

        ' Setup a default message in case any exceptions are thrown that we have not
        ' explicitly handled. On failure the message will be displayed by Revit
        message = "The sample failed"
        Execute = Autodesk.Revit.IExternalCommand.Result.Failed

        Dim dllLocation As String
        dllLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
        Dim paramFilePath As String
        paramFilePath = Path.Combine(dllLocation, "..\\RevitParameters.txt")
        commandData.Application.Options.SharedParametersFilename = paramFilePath

        'open the file
        Dim sharedParametersFile As Autodesk.Revit.Parameters.DefinitionFile
        Try
            sharedParametersFile = commandData.Application.OpenSharedParameterFile()
        Catch ex As Exception
            MsgBox("File ""RevitParameters.txt"" not found." & vbNewLine & vbTab & "Exiting function.")
            Exit Function
        End Try

        Try
            'get or create our group
            Dim sharedParameterGroup As Autodesk.Revit.Parameters.DefinitionGroup
            sharedParameterGroup = sharedParametersFile.Groups.Item("RevitParameters")
            If (sharedParameterGroup Is Nothing) Then
                sharedParameterGroup = sharedParametersFile.Groups.Create("RevitParameters")
            End If

            'get or create the parameter
            Dim sharedParameterDefinition As Autodesk.Revit.Parameters.Definition
            sharedParameterDefinition = sharedParameterGroup.Definitions.Item("APIParameter")
            If (sharedParameterDefinition Is Nothing) Then
                sharedParameterDefinition = sharedParameterGroup.Definitions.Create("APIParameter", _
                Autodesk.Revit.Parameters.ParameterType.Text, True)
            End If

            'create a category set with the wall category in it
            Dim categories As Autodesk.Revit.CategorySet
            categories = commandData.Application.Create.NewCategorySet
            Dim wallCategory As Autodesk.Revit.Category
            wallCategory = commandData.Application.ActiveDocument.Settings.Categories.Item("Walls")

            categories.Insert(wallCategory)

            'create a new instance binding for the wall categories
            Dim instanceBinding As Autodesk.Revit.Parameters.InstanceBinding
            instanceBinding = commandData.Application.Create.NewInstanceBinding(categories)

            'add the binding
            commandData.Application.ActiveDocument.ParameterBindings.Insert(sharedParameterDefinition, instanceBinding)

            ' change our result to successful
            Execute = Autodesk.Revit.IExternalCommand.Result.Succeeded
            Return Execute
        Catch ex As Exception
            message = ex.Message
            Return Execute
        End Try
        
    End Function
End Class
