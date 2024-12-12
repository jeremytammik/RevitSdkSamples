#Region "Header"
' RstLink
'
' Copyright (C) 2006-2013 by Autodesk, Inc.
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

#Region "Imports"
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Soap
Imports Autodesk.Autocad.Runtime
Imports Autodesk.AutoCAD.EditorInput
Imports A = Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.Colors
Imports RstLink
#End Region

Public Class AcadCommands

    <CommandMethod("RsLink", "RSMakeMember", CommandFlags.Modal Or CommandFlags.UsePickSet)> _
    Public Sub MakeMember()

        Dim doc As A.Document = A.Application.DocumentManager.MdiActiveDocument
        Dim ed As Editor = doc.Editor

        ' Get Selection Set of Line entities
        '-----------------------------------
        ' Message (optional)
        Dim optSS As New PromptSelectionOptions
        optSS.MessageForAdding = "Select Lines to stamp as RS Members "
        ' Selection Filter (optional)
        Dim selRB(0) As TypedValue
        selRB(0) = New TypedValue(0, "LINE")
        Dim filterSS As New SelectionFilter(selRB)
        '  Select
        Dim resSS As PromptSelectionResult = ed.GetSelection(optSS, filterSS)
        If resSS.Status <> PromptStatus.OK Then
            ed.WriteMessage("Selection error - aborting command")
            Exit Sub
        End If

        ' Loop them and add RSMember Xdata for each (skip if already exist)
        '-----------
        Dim tr As Transaction = doc.TransactionManager.StartTransaction

        Try
            ' Loop selected and change
            Dim selObj As SelectedObject
            For Each selObj In resSS.Value
                Dim ln As Line = CType(tr.GetObject(selObj.ObjectId, OpenMode.ForWrite), Line)
                If Not ln.GetXDataForApplication("RSMember") Is Nothing Then
                    ed.WriteMessage("Line " + ln.Handle.ToString + " is ALREADY an RS Member!" + vbLf)
                Else
                    ' Set RSMember XData
                    ln.XData = New ResultBuffer(New TypedValue(1001, "RSMember"), _
                                               New TypedValue(1071, 0), _
                                               New TypedValue(1070, 0), _
                                               New TypedValue(1000, "NEW"))
                    ed.WriteMessage("Line " + ln.Handle.ToString + " successfully stamped as new RS Member!" + vbLf)
                End If

            Next selObj

            tr.Commit()
        Catch ex As Exception
            MsgBox("Error? :" & ex.Message)
        Finally
            tr.Dispose()
        End Try

    End Sub

    <CommandMethod("RsLink", "RSExport", CommandFlags.Modal)> _
    Public Sub Export()

        ' Collection of intermediate RS objects
        Dim _RSelems As New Hashtable

        ' Loop ModelSpace 
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim trans As Transaction = db.TransactionManager.StartTransaction()
        Try
            Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
            Dim btr As BlockTableRecord = trans.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForRead)
            Dim btrIter As IEnumerator = btr.GetEnumerator
            While btrIter.MoveNext
                ' Check if Line with RSMember Xdata
                Dim ent As Entity = trans.GetObject(btrIter.Current, OpenMode.ForRead)
                If TypeOf ent Is Line Then
                    Dim l As Line = ent
                    Dim xd As ResultBuffer = l.GetXDataForApplication("RSMember")
                    If Not xd Is Nothing Then
                        'better put this in a util, but KIS
                        Dim revitId As Integer = 0
                        Dim iUsage As Short = 0
                        Dim sType As String = "UNKNOWN"
                        ' Loop result buffer for the values
                        Dim rbIter As IEnumerator = xd.GetEnumerator
                        While rbIter.MoveNext
                            Dim tmpVal As TypedValue = CType(rbIter.Current, TypedValue)
                            Select Case tmpVal.TypeCode
                                Case 1071
                                    revitId = tmpVal.Value
                                Case 1070
                                    iUsage = tmpVal.Value
                                Case 1000
                                    sType = tmpVal.Value
                            End Select
                        End While
                        ' Convert usage code to string
                        Dim sUsage As String = "UNKNOWN"
                        Select Case iUsage
                            Case 0
                                sUsage = "Other"
                            Case 1
                                sUsage = "Column"
                            Case 2
                                sUsage = "Girder"
                            Case 3
                                sUsage = "Joist"
                            Case 4
                                sUsage = "Vertical Bracing"
                            Case 5
                                sUsage = "Horizontal Bracing"
                        End Select

                        Dim rsm As New RSMember(revitId, sUsage, sType, New RSLine( _
                                                New RSPoint(l.StartPoint.X, l.StartPoint.Y, l.StartPoint.Z), _
                                                New RSPoint(l.EndPoint.X, l.EndPoint.Y, l.EndPoint.Z)))
                        _RSelems.Add(rsm, rsm)
                    End If
                End If

            End While

            trans.Commit()

            ' Serialize the RS objects
            If _RSelems.Count > 0 Then
                ' Select File to Save
                Dim dlg As New SaveFileDialog
                dlg.Filter = "RstLink xml files (*.xml)|*.xml"
                dlg.Title = "RstLink - AutoCAD EXPORT to Revit"
                dlg.FileName = "RstLinkAcadToRevit.xml"
                If dlg.ShowDialog() = DialogResult.OK Then

                    Dim fs As New FileStream(dlg.FileName, FileMode.Create)
                    Dim sf As New SoapFormatter
                    sf.Serialize(fs, _RSelems)
                    fs.Close()
                    MsgBox("Successfully Exported " & _RSelems.Count & " RS Members!")

                Else
                    MsgBox("Command cancelled!")
                    Exit Sub
                End If
            Else
                MsgBox("No RS Members found in ModelSpace!")
            End If

        Catch ex As Exception
            MsgBox("Error in RBExport: " & ex.Message)
        Finally
            trans.Dispose()
        End Try

    End Sub

    <CommandMethod("RsLink", "RSImport", CommandFlags.Modal)> _
    Public Sub Import()

        ' Deserialize RSElements
        Dim _RSelems As Hashtable
        Try
            ' Select File to Open
            Dim dlg As New OpenFileDialog
            dlg.Filter = "RstLink xml files (*.xml)|*.xml"
            dlg.Title = "RstLink - AutoCAD IMPORT from Revit"
            If dlg.ShowDialog() = DialogResult.OK Then
                Dim fs As New FileStream(dlg.FileName, FileMode.Open)
                Dim sf As New SoapFormatter
                sf.Binder = New RsLinkBinder()
                _RSelems = sf.Deserialize(fs)
                fs.Close()
            Else
                MsgBox("Command cancelled!")
                Exit Sub
            End If

            If _RSelems.Count <= 0 Then
                MsgBox("No elements found!")
                Return
            Else
                'MsgBox(_RSelems.Count)
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message)
            Return
        End Try

        'Create Acad Entities...loop all intermediate objects and act based on the type
        Dim elem As RSLinkElement
        For Each elem In _RSelems.Values

            ' MEMBER
            If elem.GetType Is GetType(RSMember) Then
                Dim member As RSMember = elem

                Dim db As Database = HostApplicationServices.WorkingDatabase
                Dim trans As Transaction = db.TransactionManager.StartTransaction()
                Try
                    ' Crete new Line and set end points
                    Dim line As New Line(New Point3d(member._geom._StartPt.X, member._geom._StartPt.Y, member._geom._StartPt.Z), _
                                         New Point3d(member._geom._EndPt.X, member._geom._EndPt.Y, member._geom._EndPt.Z))
                    ' Set specific layer
                    Dim sLayerName As String = "RS " + member._usage
                    Dim iCol As Short = 1
                    Dim iUsage As Short = 1
                    Select Case member._usage
                        Case "Column"
                            iUsage = 1
                            iCol = 1
                        Case "Girder"
                            iUsage = 2
                            iCol = 2
                        Case "Joist"
                            iUsage = 3
                            iCol = 3
                        Case "Vertical Bracing"
                            iUsage = 4
                            iCol = 4
                        Case "Horizontal Bracing"
                            iUsage = 5
                            iCol = 5
                        Case "Purlin"
                            iUsage = 0
                            iCol = 6
                        Case "Kicker Bracing"
                            iUsage = 0
                            iCol = 7
                        Case "Other"
                            iUsage = 0
                            iCol = 8
                        Case Else
                            iUsage = 0
                            iCol = 9
                    End Select
                    line.LayerId = GetOrCreateLayer(sLayerName, iCol)

                    ' Add to BTR and commit the transaction
                    Dim bt As BlockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead)
                    Dim btr As BlockTableRecord = trans.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite)
                    btr.AppendEntity(line)
                    trans.AddNewlyCreatedDBObject(line, True)

                    ' Make sure Xdata App is registered (not efficient here - KIS!)
                    If Not RegXdataApp("RSMember") Then
                        MsgBox("Cannot find or register Xdata App")
                        Return
                    End If
                    ' Set RSMember XData
                    line.XData = New ResultBuffer(New TypedValue(1001, "RSMember"), _
                                                  New TypedValue(1071, member.revitId), _
                                                  New TypedValue(1070, iUsage), _
                                                  New TypedValue(1000, member._type))
                    trans.Commit()
                Catch ex As Exception
                    MsgBox("Error in Creating MEMBER: " & ex.Message)
                Finally
                    trans.Dispose()
                End Try

            End If
        Next

        MsgBox("Successfully Imported " & _RSelems.Count & " RS Members!")

        ' This works, but throws an exception which can be ignored?
        Try
            'AcadApp.DocumentManager.MdiActiveDocument.SendStringToExecute("_ZOOM _E ", True, True, False)
            Dim doc As A.Document = A.Application.DocumentManager.MdiActiveDocument
            doc.SendStringToExecute("-view _SWISO ", True, True, False)
        Catch ex As Exception
        End Try


    End Sub

    Public Function RegXdataApp(ByVal appName As String) As Boolean

        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim trans As Transaction = db.TransactionManager.StartTransaction()
        Try
            Dim regTable As RegAppTable = trans.GetObject(db.RegAppTableId, OpenMode.ForWrite)
            If regTable.Has(appName) Then
                Return True
            Else
                Dim regTableRec As New RegAppTableRecord
                regTableRec.Name = appName
                regTable.Add(regTableRec)
                trans.AddNewlyCreatedDBObject(regTableRec, True)
            End If
            trans.Commit()
        Catch ex As Exception
            Return False
        Finally
            trans.Dispose()
        End Try

        Return True
    End Function

    Public Function GetOrCreateLayer(ByVal layerName As String, ByVal iCol As Short) As ObjectId

        Dim layerId As ObjectId 'the return value for this function
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim trans As Transaction = db.TransactionManager.StartTransaction()

        Try
            'Get the layer table first...
            Dim lt As LayerTable = trans.GetObject(db.LayerTableId, OpenMode.ForWrite)

            If lt.Has(layerName) Then 'Check if it exists...
                layerId = lt.Item(layerName)
            Else 'If not, create the layer here.
                Dim ltr As LayerTableRecord = New LayerTableRecord
                ltr.Name = layerName
                ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, iCol)
                layerId = lt.Add(ltr)
                trans.AddNewlyCreatedDBObject(ltr, True)
            End If
            trans.Commit()
        Catch ex As Exception
            MsgBox("Error in GetOrCreateLayer:" & ex.Message)
        Finally
            trans.Dispose()
        End Try


        Return layerId
    End Function

End Class
