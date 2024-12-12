'
' (C) Copyright 2003-2017 by Autodesk, Inc.
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
Imports System.Collections.Generic

Imports Autodesk.Revit
Imports Autodesk.Revit.DB
'
'  LibraryPaths 
'
'   This sample goes through the list of library paths held in the Revit. 
'   If it has a certain key, it erases it.  If not, it adds a new path to the existing set. 
'   In the Revit UI, you can check the list in: 
'       Settings --> Options... --> [File Locations] Tab --> Libraries.  
' 

<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class RvtCmd_LibraryPaths ' Library Paths

    Implements Autodesk.Revit.UI.IExternalCommand

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

        Const key As String = "API New Library Path"
        Const libPath As String = "C:\Temp"
        Const comment As String = _
        "Click Revit button --> [Options...] --> [File Locations] tab --> Places"

        Dim rvtApp As Autodesk.Revit.ApplicationServices.Application = commandData.Application.Application

        '  get the collection of library paths.

        Dim paths As IDictionary(Of String, String) = rvtApp.GetLibraryPaths()
        Dim iter As IEnumerator(Of KeyValuePair(Of String, String)) = paths.GetEnumerator()

        '  iterate through the collection, and gether them in a string to display it. 
        '
        Dim str As String = ""

        Do While (iter.MoveNext())
            str += iter.Current.Key + " = " + iter.Current.Value + vbCr
        Loop

        '  show it.
        MsgBox(str)


        ' do we have lib path with this key? 
        If (paths.ContainsKey(key)) Then
            ' erase the key
            paths.Remove(key)
        Else
            ' add a new path to the collection. 
            paths.Add(key, libPath)
        End If
        '  update the path. 
        rvtApp.SetLibraryPaths(paths)


        '  display it again.
        '
        iter = paths.GetEnumerator()
        iter.Reset()
        str = ""
        Do While (iter.MoveNext())
            str += iter.Current.Key + " = " + iter.Current.Value + vbCr
        Loop
        MsgBox(str + vbCr + comment)

        Return Autodesk.Revit.UI.Result.Succeeded

    End Function

End Class
