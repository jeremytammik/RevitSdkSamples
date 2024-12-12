#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2006-2008 by Autodesk, Inc.
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

Imports System.Windows.Forms
Imports Autodesk.Revit.Parameters

Namespace Labs
    Public Class BuiltInParamsChecker
        Implements IExternalCommand

        ' Store these as class data for future passing to a form display...
        ' KIS (use public variables rather then Properies)
        Private _doc As Revit.Document
        Private _ParamEnums As New ArrayList
        Private _ParamTypes As New ArrayList
        Private _ParamValues As New ArrayList


        Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute

            _doc = commandData.Application.ActiveDocument

            '' Test: list all BuiltInParameter enums
            'Dim fis() As FieldInfo = GetType(BuiltInParameter).GetFields
            '' Loop over the fields.
            'Dim msg As String = "Revit BuiltInParameters:" & vbCrLf
            'For Each fi As FieldInfo In fis
            '    ' See if this is a literal value (set at compile time).
            '    If fi.IsLiteral Then
            '        msg += fi.Name & " = " & CType(fi.GetValue(Nothing), Integer) & vbCrLf
            '    End If
            'Next fi
            'MsgBox(msg)

            ' Get the single selected element
            Dim ss As ElementSet = _doc.Selection.Elements
            If Not ss.Size = 1 Then
                MsgBox("You must pre-select a single element!")
                Return IExternalCommand.Result.Cancelled
            End If
            Dim iter As ElementSetIterator = ss.ForwardIterator
            iter.MoveNext()
            Dim elem As Revit.Element = iter.Current

            ' Takes some time, so change cursor
            Dim oldCursor As Cursor = Cursor.Current
            Cursor.Current = Cursors.WaitCursor

            ' Loop all fields for built-in params
            Dim fis() As FieldInfo = GetType(BuiltInParameter).GetFields
            For Each fi As FieldInfo In fis
                ' See if this is an enum (a literal value set at compile time)
                If fi.IsLiteral Then
                    Try
                        Dim enumInt As Integer = CType(fi.GetValue(Nothing), Integer)
                        Dim enumBip As BuiltInParameter = enumInt
                        Dim param As Parameter = elem.Parameter(enumBip)
                        If Not (param Is Nothing) Then ' this check is much faster than throwing an exception for each invalid param!
                            Select Case param.StorageType

                                Case StorageType.Double
                                    _ParamValues.Add(param.AsDouble.ToString)
                                    _ParamEnums.Add(fi.Name)
                                    _ParamTypes.Add("Double")

                                Case StorageType.Integer
                                    _ParamValues.Add(param.AsInteger.ToString)
                                    _ParamEnums.Add(fi.Name)
                                    _ParamTypes.Add("Integer")

                                Case StorageType.String
                                    _ParamValues.Add(param.AsString)
                                    _ParamEnums.Add(fi.Name)
                                    _ParamTypes.Add("String")

                                Case StorageType.ElementId
                                    _ParamValues.Add(param.AsElementId.Value.ToString)
                                    _ParamEnums.Add(fi.Name)
                                    _ParamTypes.Add("Id")

                                Case StorageType.None
                                    ' nothing
                                Case Else
                                    ' nothing
                            End Select
                        End If
                    Catch ex As Exception
                    End Try
                End If 'isLiteral
            Next fi ' looping field infos

            ' Revert the cursor
            Cursor.Current = oldCursor

            'KIS for now - in future may display in an user-friendly form...
            Dim msg As String = "Number of valid Params  = " & _ParamEnums.Count & ", " & _ParamTypes.Count & ", " & _ParamValues.Count
            MsgBox(msg)

            msg = "Valid Params for this element: "
            Dim iNum As Integer = _ParamValues.Count
            For i As Integer = 0 To iNum - 1
                msg += vbCrLf & "  " & _ParamEnums(i) & ", " & _ParamTypes(i) & ": " & _ParamValues(i)
            Next
            MsgBox(msg)

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class
End Namespace