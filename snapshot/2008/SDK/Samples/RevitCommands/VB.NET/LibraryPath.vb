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


Imports System

Imports Autodesk
Imports Autodesk.Revit
Imports Autodesk.Revit.Application
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Geometry

'
'  LibraryPaths 
'
'   This sample goes through the list of library paths held in the Revit. 
'   If it has a certain key, it erases it.  If not, it adds a new path to the existing set. 
'   In the Revit UI, you can check the list in: 
'       Settings --> Options... --> [File Locations] Tab --> Libraries.  
' 

Public Class RvtCmd_LibraryPaths ' Library Paths

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
    Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, _
    ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) _
    As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute

        Const key As String = "API New Library Path"
        Const libPath As String = "C:\Temp"
        Const comment As String = _
        "check [Settings] menu --> [Options...] --> [File Locations] tab --> [Libraries]"

        Dim rvtApp As Autodesk.Revit.Application = commandData.Application

        '  get the collection of library paths.

        Dim paths As Autodesk.Revit.Collections.StringStringMap = rvtApp.Options.LibraryPaths
        Dim iter As Autodesk.Revit.Collections.StringStringMapIterator = paths.ForwardIterator

        '  iterate through the collection, and gether them in a string to display it. 
        '
        Dim str As String = ""

        Do While (iter.MoveNext())
            str += iter.Key + " = " + iter.Current + vbCr
        Loop

        '  show it.
        MsgBox(str)


        ' do we have lib path with this key? 
        If (paths.Contains(key)) Then
            ' erase the key
            paths.Erase(key)
        Else
            ' add a new path to the collection. 
            paths.Insert(key, libPath)
        End If
        '  update the path. 
        rvtApp.Options.LibraryPaths = paths


        '  display it again.
        '
        iter.Reset()
        str = ""
        Do While (iter.MoveNext())
            str += iter.Key + " = " + iter.Current + vbCr
        Loop
        MsgBox(str + vbCr + comment)

        Return IExternalCommand.Result.Succeeded

    End Function

End Class
