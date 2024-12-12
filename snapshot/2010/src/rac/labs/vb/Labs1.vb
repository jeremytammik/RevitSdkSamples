#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2006-2009 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software
' for any purpose and without fee is hereby granted, provided
' that the above copyright notice appears in all copies and
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
#End Region

#Region "Namespaces"
Imports System
Imports Autodesk.Revit
Imports Autodesk.Revit.Elements
Imports Microsoft.VisualBasic
#End Region

Namespace Labs

#Region "Lab1_1_HelloWorld"
''' <summary>
''' Say Hello
''' Explain the development environment and the Revit.ini information creating the 
''' link between Revit and the external command. Also, how the external command
''' can be hooked up with a custom user interface by an external application.
''' </summary>
Public Class Lab1_1_HelloWorld
    Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute

            MsgBox("Hello World")
            Return IExternalCommand.Result.Succeeded

        End Function

End Class
#End Region

#Region "Lab1_2_CommandArguments"
''' <summary>
''' Test contents and usage of Execute() arguments.
''' </summary>
Public Class Lab1_2_CommandArguments
    Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            ' List the app, doc and view data
            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument
            Dim view As View = commandData.View

            Dim s As String = "Application = " & app.VersionName & vbCrLf
            s += "Version = " & app.VersionNumber & vbCrLf '
            s += "Document path = " & doc.PathName & vbCrLf ' Empty if not saved yet
            s += "Document title = " & doc.Title & vbCrLf
            s += "View name = " & view.Name
            MsgBox(s)

            ' List the current selection set
            Dim sel As Autodesk.Revit.Selection = doc.Selection

            s = "There are " & sel.Elements.Size & " elements in the selection:"
            Dim elem As Autodesk.Revit.Element
            For Each elem In sel.Elements
                s += vbCrLf & "  " & elem.Category.Name
                s += " Id=" & elem.Id.Value.ToString
            Next
            MsgBox(s)

            ' Let's pretend that something is wrong with the first element in the selection
            ' We pass a message back to the Revit user and indicate the error result
            If Not sel.Elements.IsEmpty Then
                Dim iter As ElementSetIterator = sel.Elements.ForwardIterator
                iter.MoveNext()
                Dim errElem As Autodesk.Revit.Element = iter.Current
                elements.Clear()
                elements.Insert(errElem)
                message = "We pretend something is wrong with this element and pass back this message to user"
                Return IExternalCommand.Result.Failed
            Else
                Return IExternalCommand.Result.Succeeded
            End If

        End Function

End Class
#End Region

End Namespace
