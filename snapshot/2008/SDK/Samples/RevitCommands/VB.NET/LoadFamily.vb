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

'
'  RevitCommands - Revit API custom command samplers. This project includes the following function/object usage.  
'
'     LoadFamily
'     LoadFamilySymbol
'
Imports System
Imports System.IO

Imports Autodesk
Imports Autodesk.Revit
Imports Autodesk.Revit.Application
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Geometry


' 
'  LoadFamilySymbol - loads a family symbol/type of the given file and the symbol name.
' 

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
    Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, _
    ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) _
    As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute

        ' hard coded family file and type name
        Const libPath As String = _
        "C:\Documents and Settings\All Users\Application Data\Autodesk\Revit Architecture 2008\Imperial Library\"
        Dim fileName As String = libPath + "Structural\Columns\Steel\HP-Bearing Pile-Column.rfa"
        Const symName As String = "HP12X63"

        If (commandData.Application.ActiveDocument.LoadFamilySymbol(fileName, symName)) Then
            MsgBox("Family " & fileName & ", Type " & symName & " successfully loaded!")
            Return Autodesk.Revit.IExternalCommand.Result.Succeeded
        End If

        MsgBox("ERROR in loading Family " & fileName & ", Type " & symName)
        Return IExternalCommand.Result.Failed

    End Function

End Class

'
'  LoadFamily - loads a family of the given family name. 
' 
' 
Public Class RvtCmd_LoadFamily

    Implements Autodesk.Revit.IExternalCommand

    ''' <summary>
    ''' Implement this method as an external command for Revit.
    ''' </summary>
    Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, _
    ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) _
    As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute

        Dim rvtApp As Autodesk.Revit.Application = commandData.Application

        '  family file name. hard coded for simplicity. 
        '
        Dim fileName As String = "Sink Kitchen-Single.rfa"

        '  (1) hard coded family path. 
        'Dim filePath As String = 
        '"C:\Documents and Settings\All Users\Application Data\Autodesk\Revit Building 8.1\Metric Library\Structural\Framing\Steel"

        '  (2) or search a family path in the library paths. 
        Dim filePath As String = findFile(rvtApp, fileName)
        If (filePath Is Nothing) Then
            MsgBox("cannot find the file " + fileName)
            Return IExternalCommand.Result.Failed
        End If

        '  set a full path to call LoadFamily method. 
        Dim fullPath As String = filePath + "\" + fileName

        If (rvtApp.ActiveDocument.LoadFamily(fullPath)) Then
            MsgBox("loaded a family ok.")
            Return Autodesk.Revit.IExternalCommand.Result.Succeeded
        End If

        MsgBox("failed to load a family.")
        'Return IExternalCommand.Result.Failed ' bug in Revit 8.0 
        Return IExternalCommand.Result.Succeeded

    End Function

    '  Helper function - find the given file name in the set of Revit library paths. 
    ' 
    Public Function findFile(ByVal rvtApp As Autodesk.Revit.Application, ByVal fileName As String) _
    As String

        Dim paths As Autodesk.Revit.Collections.StringStringMap = rvtApp.Options.LibraryPaths
        Dim iter As Autodesk.Revit.Collections.StringStringMapIterator = paths.ForwardIterator

        ' loop through each path in the collection. 
        Do While (iter.MoveNext())

            Dim path As String = iter.Current
            Dim filePath As String = SearchFile(path, fileName)
            If Not (filePath Is Nothing) Then
                Return filePath
            End If

        Loop

        Return Nothing

    End Function

    '  Helper function - recursively search the given file name under the current directory. 
    '
    Public Function SearchFile(ByVal path As String, ByVal fileName As String) As String

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