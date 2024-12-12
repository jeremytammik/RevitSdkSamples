'
' (C) Copyright 2003-2012 by Autodesk, Inc.
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

'
'  RevitCommands - Revit API custom command samplers. This project includes the following function/object usage.  
'
'     LoadFamily
'     LoadFamilySymbol
'
Imports System
Imports System.IO
Imports System.Reflection
Imports System.Collections.Generic

Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.Collections


' 
'  LoadFamilySymbol - loads a family symbol/type of the given file and the symbol name.
' 

<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class RvtCmd_LoadFamilySymbol

    Implements IExternalCommand

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
    Public Function Execute(ByVal commandData As Autodesk.Revit.UI.ExternalCommandData, _
    ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) _
    As Autodesk.Revit.UI.Result Implements Autodesk.Revit.UI.IExternalCommand.Execute
        Environment.CurrentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
        ' hard coded family file and type name
        Dim fileName As String = "W-Wide Flange-Column.rfa"
        fileName = Path.GetFullPath(fileName)
        Const symName As String = "W10X49"
        If Not File.Exists(fileName) Then
            message = String.Format("Can't find the file '{0}'", fileName)
            Return Autodesk.Revit.UI.Result.Failed
        End If
        Dim revitDoc As Document = commandData.Application.ActiveUIDocument.Document

        Dim transaction As New Autodesk.Revit.DB.Transaction(revitDoc, "RvtCmd_LoadFamilySymbol")
        Try
            transaction.Start()
            Dim loadSuccess As Boolean = revitDoc.LoadFamilySymbol(fileName, symName)
            transaction.Commit()
            If (loadSuccess) Then
                MsgBox("Family " & fileName & ", Type " & symName & " successfully loaded!")
                Return Autodesk.Revit.UI.Result.Succeeded
            End If
        Catch ex As Exception
            transaction.RollBack()
        End Try

        MsgBox("ERROR in loading Family " & fileName & ", Type " & symName)
        Return Autodesk.Revit.UI.Result.Failed

    End Function

End Class

'
'  LoadFamily - loads a family of the given family name. 
' 
' 
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
Public Class RvtCmd_LoadFamily
    Implements Autodesk.Revit.UI.IExternalCommand

    Private libPaths As String

    ''' <summary>
    ''' Implement this method as an external command for Revit.
    ''' </summary>
    Public Function Execute(ByVal commandData As Autodesk.Revit.UI.ExternalCommandData, _
    ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) _
    As Autodesk.Revit.UI.Result Implements Autodesk.Revit.UI.IExternalCommand.Execute

        Dim rvtApp As Autodesk.Revit.UI.UIApplication = commandData.Application
        Dim revitDoc As Document = commandData.Application.ActiveUIDocument.Document
        libPaths = Nothing

        '  family file name. hard coded for simplicity. 
        '
        Dim fileName As String = "Sink Kitchen-Single.rfa"

        '  (1) hard coded family path. 
        'Dim filePath As String = 
        '"C:\Documents and Settings\All Users\Application Data\Autodesk\Revit Architecture 2009\Metric Library\Structural\Framing\Steel"

        '  (2) or search a family path in the library paths. 
        Dim filePath As String = findFile(rvtApp.Application, fileName)
        If (filePath Is Nothing) Then
            message = "Cannot find the file '" + fileName + "' in Revit Libraries:" + ControlChars.NewLine + libPaths
            Return Autodesk.Revit.UI.Result.Failed
        End If

        '  set a full path to call LoadFamily method. 
        Dim fullPath As String = filePath + "\" + fileName

        Dim transaction As New Autodesk.Revit.DB.Transaction(revitDoc, "RvtCmd_LoadFamily")
        Try
            transaction.Start()
            Dim loadSuccess As Boolean = revitDoc.LoadFamily(fullPath)
            transaction.Commit()
            If (loadSuccess) Then
                MsgBox("Loaded a family ok.")
                Return Autodesk.Revit.UI.Result.Succeeded
            End If
        Catch ex As Exception
            transaction.RollBack()
        End Try

        MsgBox("Failed to load a family.")
        'Return Autodesk.Revit.UI.Result.Failed ' bug in Revit 8.0 
        Return Autodesk.Revit.UI.Result.Succeeded

    End Function

    '  Helper function - find the given file name in the set of Revit library paths. 
    ' 
    Public Function findFile(ByVal rvtApp As Autodesk.Revit.ApplicationServices.Application, ByVal fileName As String) _
    As String

        Dim paths As IDictionary(Of String, String) = rvtApp.GetLibraryPaths()
        Dim iter As IEnumerator(Of KeyValuePair(Of String, String)) = paths.GetEnumerator()
        Dim filePath As String = Nothing
        ' loop through each path in the collection. 
        Do While (iter.MoveNext())

            Dim path As String = iter.Current.Value
            libPaths += ControlChars.Tab + path + ";" + ControlChars.NewLine
            filePath = SearchFile(path, fileName)
            If Not (filePath Is Nothing) Then
                Return filePath
            End If

        Loop
        filePath = SearchFile(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), fileName)
        If Not (filePath Is Nothing) Then
            Return filePath
        End If
        Return Nothing

    End Function

    '  Helper function - recursively search the given file name under the current directory. 
    '
    Public Function SearchFile(ByVal path As String, ByVal fileName As String) As String
        If Not Directory.Exists(path) Then
            Return Nothing
        End If
        '  search this directory 
        Dim fname As String
        For Each fname In Directory.GetFiles(path, fileName)
            MsgBox("I found the file in: " + fname)
            Return path
        Next

        '  recursively search child directories.  
        Dim dname As String
        For Each dname In Directory.GetDirectories(path)
            Dim filePath As String = SearchFile(dname, fileName)
            If Not (filePath Is Nothing) Then
                Return filePath
            End If
        Next

        Return Nothing

    End Function

End Class
