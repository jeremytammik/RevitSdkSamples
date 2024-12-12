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

Imports Autodesk.Revit
Imports Autodesk.Revit.Geometry
Imports Autodesk.Revit.Geometry.XYZ
Imports Autodesk.Revit.Parameters

Namespace Labs

#Region "Lab5_1_GroupsAndGroupTypes"
    ''' <summary>
    ''' List all groups and group types in the model.
    ''' </summary>
    Public Class Lab5_1_GroupsAndGroupTypes
        Implements IExternalCommand
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Revit.Application = commandData.Application

            ' List all Group Elements
            Dim groups As List(Of Element) = LabUtils.GetAllGroups(app)
            Dim sMsg As String = "All GROUPS in the doc are:" & vbCrLf
            Dim grp As Group
            For Each grp In groups
                sMsg += vbCrLf + "  Id=" & grp.Id.Value.ToString & "; Type=" & grp.GroupType.Name
            Next
            MsgBox(sMsg)

            ' List all Group Type Elements
            Dim groupTypes As List(Of Element) = LabUtils.GetAllGroupTypes(app)
            sMsg = "All GROUP TYPES in the doc are:" & vbCrLf
            Dim grpTyp As GroupType
            For Each grpTyp In groupTypes
                ' To Determine the GroupType system Family,
                '    we need the following (as in Labs3 for standard Symbols):
                Dim famName As String = "?"
                Dim p As Parameter = grpTyp.Parameter(Parameters.BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM)
                If Not p Is Nothing Then
                    famName = p.AsString
                End If
                sMsg += vbCrLf + "  Name=" & grpTyp.Name & "; Id=" & grpTyp.Id.Value.ToString & "; Family=" & famName
            Next
            MsgBox(sMsg)

            ' Typically, only "Model" types will be needed, so makes sense to have a dedicated utility
            Dim modelGroupTypes As List(Of Element) = LabUtils.GetAllModelGroupTypes(app)
            sMsg = "All *MODEL* GROUP TYPES in the doc are:" & vbCrLf
            For Each grpTyp In modelGroupTypes
                sMsg += vbCrLf + "  Name=" & grpTyp.Name & "; Id=" & grpTyp.Id.Value.ToString
            Next
            MsgBox(sMsg)

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab5_2_SwapGroupTypes"
    ''' <summary>
    ''' Swap group types for selected groups.
    ''' </summary>
    Public Class Lab5_2_SwapGroupTypes
        Implements IExternalCommand
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Revit.Application = commandData.Application

            ' First get all Group Types of Model Family
            Dim gts As List(Of Element) = LabUtils.GetAllModelGroupTypes(app)
            'Dim gts As ElementSet = LabUtils.GetAllGroupTypes(app) ' the best we can do in 9.0

            If gts.Count = 0 Then
                MsgBox("No Model Group Types in this model!") '8.1
                'MsgBox("No Group Types in this model!") '9.0
                Return IExternalCommand.Result.Cancelled
            End If

            ' Loop through selection
            Dim elem As Revit.Element
            If (app.ActiveDocument.Selection.Elements.Size = 0) Then
                MsgBox("No Group has been selected!")
            Else
                For Each elem In app.ActiveDocument.Selection.Elements

                    ' Check for Group instance
                    If TypeOf elem Is Group Then
                        Dim gp As Group = elem

                        ' Offer simple message box to swap the type
                        ' (one-by-one, stop if user confirms the change)
                        Dim gt As GroupType
                        For Each gt In gts
                            Select Case (MsgBox("Swap OLD Type=" & gp.GroupType.Name & " with NEW Type=" & gt.Name & " for Group Id=" & gp.Id.Value.ToString & "?", MsgBoxStyle.YesNoCancel))
                                Case MsgBoxResult.Yes
                                    gp.GroupType = gt
                                    MsgBox("Group type successfully swapped!")
                                    Exit For
                                Case MsgBoxResult.Cancel
                                    MsgBox("Command cancelled!")
                                    Return IExternalCommand.Result.Cancelled
                                Case MsgBoxResult.No
                                    ' just continue with the For Loop
                            End Select
                        Next
                    End If

                Next
            End If

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab5_3_Rooms"
    ''' <summary>
    ''' List room boundaries.
    ''' </summary>
    Public Class Lab5_3_Rooms
        Implements IExternalCommand
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Revit.Application = commandData.Application
            Dim rm As Room

            ' List all Room Elements
            Dim rooms As List(Of Element) = LabUtils.GetAllRooms(app)
            If (0 = rooms.Count) Then
                MsgBox("There are no rooms in this model!")
            Else
                For Each rm In rooms

                    ' Some identification Parameters
                    ' (there are probably built-in Params for this, but this works :-)

                    Dim roomName As String = "?"
                    Dim p As Parameter = rm.Parameter("Name")
                    If Not p Is Nothing Then
                        roomName = p.AsString
                    End If

                    Dim roomNumber As String = "?"
                    p = rm.Parameter("Number")
                    If Not p Is Nothing Then
                        roomNumber = p.AsString
                    End If

                    Dim sMsg As String = "Room Id=" & rm.Id.Value.ToString & " Name=" & roomName & " Number=" & roomNumber & vbCrLf

                    ' Loop all Boundaries of this Room
                    Dim boundaries As Rooms.BoundarySegmentArrayArray = rm.Boundary
                    ' Check to ensure the room has boundary
                    If Not (boundaries Is Nothing) Then

                        Dim iB As Integer = 0
                        Dim boundary As Rooms.BoundarySegmentArray
                        For Each boundary In boundaries

                            ' Msg
                            iB += 1
                            sMsg += vbCrLf & "    Boundary " & iB & ":"

                            ' Loop through all Segments of that Boundary
                            Dim iSeg As Integer = 0
                            Dim segment As Rooms.BoundarySegment
                            For Each segment In boundary
                                iSeg += 1

                                ' Segment's curve
                                Dim crv As Geometry.Curve = segment.Curve

                                If TypeOf crv Is Geometry.Line Then                     'LINE

                                    Dim line As Geometry.Line = crv
                                    Dim ptS As Geometry.XYZ = line.EndPoint(0)
                                    Dim ptE As Geometry.XYZ = line.EndPoint(1)

                                    sMsg += vbCrLf & "        Segment " & iSeg & " is a LINE:" & _
                                     ptS.X & ", " & ptS.Y & ", " & ptS.Z & " ; " & _
                                     ptE.X & ", " & ptE.Y & ", " & ptE.Z

                                ElseIf TypeOf crv Is Geometry.Arc Then                      'ARC

                                    Dim arc As Arc = crv
                                    Dim ptS As Geometry.XYZ = arc.EndPoint(0)
                                    Dim ptE As Geometry.XYZ = arc.EndPoint(1)
                                    Dim r As Double = arc.Radius

                                    sMsg += vbCrLf & "        Segment " & iSeg & " is an ARC:" & _
                                     ptS.X & ", " & ptS.Y & ", " & ptS.Z & " ; " & _
                                     ptE.X & ", " & ptE.Y & ", " & ptE.Z & " ; R=" & r

                                End If
                            Next
                        Next
                    End If
                    MsgBox(sMsg)
                Next
            End If
            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

End Namespace

