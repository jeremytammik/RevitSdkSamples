#Region "Header"
'Revit API .NET Labs

'Copyright (C) 2006-2013 by Autodesk, Inc.

'Permission to use, copy, modify, and distribute this software
'for any purpose and without fee is hereby granted, provided
'that the above copyright notice appears in all copies and
'that both that copyright notice and the limited warranty and
'restricted rights notice below appear in all supporting
'documentation.

'AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
'AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
'MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
'DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
'UNINTERRUPTED OR ERROR FREE.

'Use, duplication, or disclosure by the U.S. Government is subject to
'restrictions set forth in FAR 52.227-19 (Commercial Computer
'Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
'(Rights in Technical Data and Computer Software), as applicable.
#End Region

#Region "Namespaces"
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Reflection
Imports System.Linq
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.Attributes
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.DB.Architecture
Imports Autodesk.Revit.UI
Imports Microsoft.VisualBasic.Constants
'' todo: report and resolve this, this should not be required: 'RE: ambiguous BoundarySegmentArrayArray'
Imports BoundarySegmentArrayArray = Autodesk.Revit.DB.Architecture.BoundarySegmentArrayArray
Imports BoundarySegmentArray = Autodesk.Revit.DB.Architecture.BoundarySegmentArray
Imports BoundarySegment = Autodesk.Revit.DB.Architecture.BoundarySegment
#End Region

Namespace XtraVb

#Region "Lab5_1_GroupsAndGroupTypes"
  ''' <summary>
  ''' List all groups and group types in the model.
  ''' </summary>
  <Transaction(TransactionMode.ReadOnly)> _
  Public Class Lab5_1_GroupsAndGroupTypes
    Implements IExternalCommand

    Const _groupTypeModel As String = "Model Group"
    ' BEWARE: In the browser, it says only "Model"
    Const _groupsTypeModel As String = "Model Groups"

    Public Function Execute( _
        ByVal commandData As ExternalCommandData, _
        ByRef message As String, _
        ByVal elements As ElementSet) As Result _
        Implements IExternalCommand.Execute

      Dim app As UIApplication = commandData.Application
      Dim doc As Document = app.ActiveUIDocument.Document

      Dim a As New List(Of String)()

      ' list groups:

      Dim collector As FilteredElementCollector
      collector = New FilteredElementCollector(doc)
      collector.OfClass(GetType(Group))

      For Each g As Group In collector
        a.Add("Id=" + g.Id.IntegerValue.ToString() + "; Type=" + g.GroupType.Name)
      Next
      LabUtils.InfoMsg("{0} group{1} in the document{2}", a)

      ' list groups types:

      Dim bic As BuiltInParameter = BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM

      a.Clear()

      collector = New FilteredElementCollector(doc)
      collector.OfClass(GetType(GroupType))

      For Each g As GroupType In collector
        ' determine the GroupType system family
        ' (cf. Labs3 for standard symbols):

        Dim p As Parameter = g.Parameter(bic)
        Dim famName As String = If((p Is Nothing), "?", p.AsString())

        a.Add("Name=" + g.Name + "; Id=" + g.Id.IntegerValue.ToString() + "; Family=" + famName)
      Next
      LabUtils.InfoMsg("{0} group type{1} in the document{2}", a)

      ' typically, only "Model" types will be needed.
      ' create a filter by creating a provider and an evaluator.
      ' we can reuse the collector we already set up for group
      ' types, and just add another criteria to check to it:

      a.Clear()

      collector.OfCategory(BuiltInCategory.OST_IOSModelGroups)

      For Each g As GroupType In collector
        a.Add("Name=" + g.Name + "; Id=" + g.Id.IntegerValue.ToString())
      Next

      LabUtils.InfoMsg("{0} *model* group type{1} in the document{2}", a)

      Return Result.Failed
    End Function

  End Class
#End Region

#Region "Lab5_2_SwapGroupTypes"
  ''' <summary>
  ''' Swap group types for selected groups.
  ''' </summary>
  <Transaction(TransactionMode.Automatic)> _
  Public Class Lab5_2_SwapGroupTypes
    Implements IExternalCommand
    Public Function Execute( _
        ByVal commandData As ExternalCommandData, _
        ByRef message As String, _
        ByVal elements As ElementSet) As Result _
        Implements IExternalCommand.Execute

      Dim app As UIApplication = commandData.Application
      Dim uidoc As UIDocument = app.ActiveUIDocument
      Dim doc As Document = uidoc.Document

      ' Get all Group Types of Model Family:

      Dim modelGroupTypes As New FilteredElementCollector(doc)

      modelGroupTypes.OfClass(GetType(GroupType))
      modelGroupTypes.OfCategory(BuiltInCategory.OST_IOSModelGroups)

      If 0 = modelGroupTypes.Count() Then
        message = "No model group types found in model."
        Return Result.Failed
      End If

      Dim groups As FilteredElementCollector
      groups = New FilteredElementCollector(doc)
      groups.OfClass(GetType(Group))

      For Each g As Group In groups
        ' Offer simple message box to swap the type
        ' (one-by-one, stop if user confirms the change)

        For Each gt As GroupType In modelGroupTypes
          Dim msg As String = "Swap OLD Type=" + g.GroupType.Name + _
              " with NEW Type=" + gt.Name + _
              " for Group Id=" + g.Id.IntegerValue.ToString() + "?"

          Dim r As TaskDialogResult = LabUtils.QuestionCancelMsg(msg)

          Select Case r
            Case TaskDialogResult.Yes
              g.GroupType = gt
              LabUtils.InfoMsg("Group type successfully swapped.")
              Return Result.Succeeded

            Case TaskDialogResult.Cancel
              LabUtils.InfoMsg("Command cancelled.")
              Return Result.Cancelled

              ' else continue...
          End Select
        Next
      Next

      Return Result.Succeeded
    End Function
  End Class
#End Region

#Region "Lab5_3_Rooms"
  ''' <summary>
  ''' List room boundaries.
  ''' </summary>
  <Transaction(TransactionMode.Automatic)> _
  Public Class Lab5_3_Rooms
    Implements IExternalCommand
    Public Function Execute( _
        ByVal commandData As ExternalCommandData, _
        ByRef message As String, _
        ByVal elements As Autodesk.Revit.DB.ElementSet) As Result _
        Implements IExternalCommand.Execute

      Dim app As UIApplication = commandData.Application
      Dim doc As Document = app.ActiveUIDocument.Document

      Dim rooms As FilteredElementCollector

      rooms = New FilteredElementCollector(doc)

      '
      ' this is one way of obtaining rooms ... but see below for a better solution:
      '
      'rooms.OfClass( typeof( Room ) );
      '
      ' Input type is of an element type that exists in the API, but not in Revit's native object model.
      ' Try using Autodesk.Revit.DB.Enclosure instead, and then postprocessing the results to find the elements of interest.
      '
      'rooms.OfClass( typeof( Enclosure ) ); // this works but returns all Enclosure elements

      Dim filter As New RoomFilter()
      rooms.WherePasses(filter)

      If 0 = rooms.Count() Then
        LabUtils.InfoMsg("There are no rooms in this model.")
      Else
        Dim a As New List(Of String)()

        For Each room As Room In rooms
          Dim roomName As String = room.Name
          Dim roomNumber As String = room.Number

          Dim s As String = "Room Id=" + room.Id.IntegerValue.ToString() + _
              " Name=" + roomName + " Number=" + roomNumber + vbCrLf

          ' Loop all boundaries of this room

          'Dim boundaries As BoundarySegmentArrayArray = room.Boundary ' 2011

          Dim boundaries As IList(Of IList(Of BoundarySegment)) = room.GetBoundarySegments(New SpatialElementBoundaryOptions()) ' 2012; passing in a null value throws an exception

          ' Check to ensure room has boundary

          If boundaries IsNot Nothing Then
            Dim iB As Integer = 0
            'For Each boundary As BoundarySegmentArray In boundaries ' 2011
            For Each boundary As IList(Of BoundarySegment) In boundaries ' 2012
              iB += 1
              s += " Boundary " + iB + ":" + vbCrLf
              Dim iSeg As Integer = 0
              For Each segment As BoundarySegment In boundary
                iSeg += 1

                ' Segment's curve
                Dim crv As Curve = segment.Curve

                If TypeOf crv Is Line Then
                  Dim line As Line = TryCast(crv, Line)
                  Dim ptS As XYZ = line.GetEndPoint(0)
                  Dim ptE As XYZ = line.GetEndPoint(1)
                  s += " Segment " + iSeg + " is a LINE: " _
                      + LabUtils.PointString(ptS) + " ; " _
                      + LabUtils.PointString(ptE) + vbCrLf
                ElseIf TypeOf crv Is Arc Then
                  Dim arc As Arc = TryCast(crv, Arc)
                  Dim ptS As XYZ = arc.GetEndPoint(0)
                  Dim ptE As XYZ = arc.GetEndPoint(1)
                  Dim r As Double = arc.Radius
                  s += " Segment " + iSeg + " is an ARC:" _
                      + LabUtils.PointString(ptS) + " ; " _
                      + LabUtils.PointString(ptE) + " ; R=" _
                      + LabUtils.RealString(r) + vbCrLf
                End If
              Next
            Next
            a.Add(s)
          End If
          LabUtils.InfoMsg("{0} room{1} in the model{2}", a)
        Next
      End If
      Return Result.Failed
    End Function
  End Class
#End Region

End Namespace
