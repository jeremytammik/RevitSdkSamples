#Region "Header"
' Copyright (c) 2007-2009 by Autodesk, Inc.
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
'
' Created by: Bill Zhang and Jeremy Tammik, DevTech, Autodesk, Oct. 2007
#End Region

#Region "Namespaces"
Imports System
Imports System.Collections.Generic
Imports System.Text

Imports System.Windows.Forms

' Tip 1 - complete namespace prefix to avoid ambiguity:
'Imports Rvt = Autodesk.Revit
'Imports RvtGeom = Autodesk.Revit.Geometry
'Imports RvtSymbs = Autodesk.Revit.Symbols
'Imports RvtElems = Autodesk.Revit.Elements

' Tip 1 - use namespaces globally and disambiguate individual symbols when needed:
Imports Autodesk.Revit
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Symbols
Imports RvtElement = Autodesk.Revit.Element
Imports RvtWallType = Autodesk.Revit.Symbols.WallType
Imports CmdResult = Autodesk.Revit.IExternalCommand.Result
#End Region

'
' Tip 9 - Language Comparison C#, VB and C++ - VB
'
' List wall family, type and parameter information with VB.NET.
' Compare equivalent code in C#, VB.NET and VC.NET.
'
Public Class CommandListWallInfo
    Implements IExternalCommand
    Public Function Execute( _
        ByVal cmdData As ExternalCommandData, _
        ByRef message As String, _
        ByVal elements As ElementSet) _
    As CmdResult Implements IExternalCommand.Execute
        Try
            Dim doc As Document = cmdData.Application.ActiveDocument
            Dim selSet As ElementSet = doc.Selection.Elements
            Dim elem As RvtElement

            If selSet.Size < 1 Or selSet.Size > 1 Then
                message = "Only one wall is allowed!"
                Return IExternalCommand.Result.Cancelled
            End If

            For Each elem In selSet
                Dim catName As String = ""
                MsgBox(listWallFamilyAndTypeInfo(elem, catName), , catName)

                MsgBox(ListParameters(elem), , " - Parameters of the Wall - ")

                If TypeOf elem Is Wall Then

                    Dim wall As Wall = elem
                    If Not wall.WallType Is Nothing Then
                        Dim wallType As Symbols.WallType = wall.WallType
                        MsgBox(ListParameters(wallType), , " - Parameters of the Wall Type - ")
                    End If
                Else
                    message = "Only one wall is allowed!"
                    Return IExternalCommand.Result.Cancelled
                End If
            Next
            Return CmdResult.Succeeded
        Catch ex As Exception
            message = ex.Message
            Return CmdResult.Failed
        End Try
    End Function

    Private Function ListParameters(ByVal elem As RvtElement) As String
        Dim params As ParameterSet = elem.Parameters
        If params.IsEmpty Then
            Return "Parameters are empty!"
        End If

        Dim str As String = ""
        Dim param As Parameter
        For Each param In params
            Dim name As String = param.Definition.Name
            Dim type As Parameters.StorageType = param.StorageType
            Dim val As String = ""

            Select Case type
                Case Parameters.StorageType.Double
                    val = param.AsDouble
                Case Parameters.StorageType.ElementId
                    Dim paraElem As RvtElement = elem.Document.Element(param.AsElementId)
                    If Not (paraElem Is Nothing) Then
                        val = paraElem.Name
                    End If
                Case Parameters.StorageType.Integer
                    val = param.AsInteger
                Case Parameters.StorageType.String
                    val = param.AsString
                Case Else
            End Select
            str = str & name & ": " & val & vbCr
        Next
        Return str
    End Function

    Private Function listWallFamilyAndTypeInfo(ByVal elem As RvtElement, ByRef cat As String) As String
        Dim infoStr As String = ""
        Dim title As String = "Element category"
        If Not elem.Category Is Nothing Then
            title = title & " : " & elem.Category.Name
        End If
        If TypeOf elem Is Wall Then
            Dim wall As Wall = elem
            Dim wallType As RvtWallType = wall.WallType
            Select Case wallType.Kind
                Case Symbols.WallType.WallKind.Basic
                    infoStr = infoStr & "Basic : "
                Case Symbols.WallType.WallKind.Curtain
                    infoStr = infoStr & "Curtain : "
                Case Symbols.WallType.WallKind.Stacked
                    infoStr = infoStr & "Stacked : "
                Case Symbols.WallType.WallKind.Unknown
                    infoStr = infoStr & "Unknown : "
            End Select
            infoStr = infoStr & wallType.Name
        End If
        cat = title
        Return infoStr
    End Function

End Class
