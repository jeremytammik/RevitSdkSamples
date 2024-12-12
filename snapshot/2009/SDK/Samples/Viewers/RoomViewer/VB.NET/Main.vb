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

Imports System
Imports System.Text

Imports Autodesk
Imports Autodesk.Revit
Imports Autodesk.Revit.Application
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Geometry

''' <summary>
''' Implements the Revit add-in interface IExternalCommand
''' </summary>
''' <remarks></remarks>
Public Class RoomViewer
  Implements Autodesk.Revit.IExternalCommand

    'the element wire frame to be draw.
  Dim mViewer As RevitViewer.Wireframe

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
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements IExternalCommand.Execute

        Dim iter As IEnumerator
        iter = commandData.Application.ActiveDocument.Elements

        Do While iter.MoveNext

            Dim element As Autodesk.Revit.Element
            element = iter.Current

            If (TypeOf element Is Autodesk.Revit.Elements.Room) Then
                DrawRoomOutline(element)
            End If
        Loop

        If Not (mViewer Is Nothing) Then
            mViewer.Fit()
            mViewer.Draw()
            mViewer.ShowModal()
        Else
            System.Windows.Forms.MessageBox.Show("No room to represent.")
        End If

        mViewer = Nothing

        Return Autodesk.Revit.IExternalCommand.Result.Succeeded

    End Function

    ''' <summary>
    ''' add the room outline to the Wireframe object.
    ''' </summary>
    ''' <param name="room"></param>
    ''' <remarks></remarks>
    Private Sub DrawRoomOutline(ByVal room As Autodesk.Revit.Elements.Room)

        If mViewer Is Nothing Then
            mViewer = New RevitViewer.Wireframe()
        End If

        Dim boundaries As Autodesk.Revit.Rooms.BoundarySegmentArrayArray
        boundaries = room.Boundary
        If boundaries Is Nothing Then
            Dim message As StringBuilder
            message = New StringBuilder
            message.AppendFormat("The {0} has no boundary to represent.", room.Name)
            System.Windows.Forms.MessageBox.Show(message.ToString())
            Return
        End If
        Dim boundary As Autodesk.Revit.Rooms.BoundarySegmentArray

        For Each boundary In boundaries
            Dim segment As Autodesk.Revit.Rooms.BoundarySegment
            For Each segment In boundary

                DrawCurve(segment.Curve)

            Next
        Next

    End Sub

    ''' <summary>
    ''' add the primitive curve to the Wireframe object.
    ''' </summary>
    ''' <param name="curve"></param>
    ''' <remarks></remarks>
    Private Sub DrawCurve(ByVal curve As Autodesk.Revit.Geometry.Curve)

        '
        ' added by jeremy t display arced wall:
        '

        Dim pts As XYZArray = curve.Tessellate()
        Dim n As Integer = pts.Size

        Dim i As Integer = 0
        For i = 0 To n - 2
            mViewer.Add(pts.Item(i).X, pts.Item(i).Y, pts.Item(i).Z, pts.Item(i + 1).X, pts.Item(i + 1).Y, pts.Item(i + 1).Z)
        Next i

        If TypeOf curve Is Autodesk.Revit.Geometry.Arc Then
            Dim arc As Autodesk.Revit.Geometry.Arc = curve
            Debug.WriteLine(String.Format("Arc center {0} radius {1} normal {2} start {3} at {4} end {5} at {6}", _
              PointToString(arc.Center), arc.Radius, PointToString(arc.Normal), PointToString(arc.EndPoint(0)), arc.EndParameter(0), PointToString(arc.EndPoint(1)), arc.EndParameter(1)))
        End If
    End Sub

    Public Shared Function PointToString(ByVal p As Autodesk.Revit.Geometry.XYZ) As String
        Dim str As String = "(" & p.X.ToString("F2") & "," & p.Y.ToString("F2") & "," & p.Z.ToString("F2") & ")"
        Return str
    End Function

End Class
