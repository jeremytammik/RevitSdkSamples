#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2007-2013 by Autodesk, Inc.
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
#End Region ' Header

#Region "Namespaces"
Imports System
Imports System.Collections.Generic
Imports db = Autodesk.Revit.DB
Imports dbst = Autodesk.Revit.DB.Structure
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.Attributes
Imports Constants = Microsoft.VisualBasic.Constants
#End Region ' Namespaces

Namespace RstLabs
#Region "Lab3_ListSelectedAnalyticalModels"
  ''' <summary>
  ''' Lab 3 - List analytical model for selected structural elements,
  ''' or all elements if none are selected.
  ''' </summary>
  <Transaction(TransactionMode.ReadOnly)> _
  Public Class Lab3_ListSelectedAnalyticalModels
    Implements IExternalCommand
    Private m_doc As db.Document
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As db.ElementSet) As Result Implements IExternalCommand.Execute
      Dim app As UIApplication = commandData.Application
      Dim doc As db.Document = app.ActiveUIDocument.Document
      m_doc = doc
      Dim categories As db.Categories = doc.Settings.Categories
      Dim catStruCols As db.Category = categories.Item(db.BuiltInCategory.OST_StructuralColumns)
      Dim catStruFrmg As db.Category = categories.Item(db.BuiltInCategory.OST_StructuralFraming)
      Dim catStruFndt As db.Category = categories.Item(db.BuiltInCategory.OST_StructuralFoundation)
      Dim a As New List(Of db.Element)()
      If 0 < app.ActiveUIDocument.Selection.Elements.Size Then
        For Each e As db.Element In app.ActiveUIDocument.Selection.Elements
          a.Add(e)
        Next e
      Else
        '
        ' Use Revit 2011 FilteredElementCollector to get all system family instances.
        '
        ' create filter list to accommodate several filters.
        Dim filterList As New List(Of db.ElementFilter)()
        Dim cfColumn As New db.ElementCategoryFilter(db.BuiltInCategory.OST_StructuralColumns)
        Dim cfFrmg As New db.ElementCategoryFilter(db.BuiltInCategory.OST_StructuralFraming)
        Dim cfStruFndt As New db.ElementCategoryFilter(db.BuiltInCategory.OST_StructuralFoundation)
        filterList.Add(cfColumn)
        filterList.Add(cfFrmg)
        filterList.Add(cfStruFndt)
        Dim lOrFilter1 As New db.LogicalOrFilter(filterList)

        Dim cfFamilyInstance As New db.ElementClassFilter(GetType(db.FamilyInstance))
        Dim lAndFilter1 As New db.LogicalAndFilter(cfFamilyInstance, lOrFilter1)

        Dim cfFooting As New db.ElementClassFilter(GetType(db.ContFooting))
        Dim cfFloor As New db.ElementClassFilter(GetType(db.Floor))
        Dim cfWall As New db.ElementClassFilter(GetType(db.Wall))

        Dim filterList2 As New List(Of db.ElementFilter)()
        filterList2.Add(cfFooting)
        filterList2.Add(cfFloor)
        filterList2.Add(cfWall)
        Dim lOrFilter2 As New db.LogicalOrFilter(filterList2)

        Dim lOrFilterLast As New db.LogicalOrFilter(lAndFilter1, lOrFilter2)

        Dim collector As New db.FilteredElementCollector(app.ActiveUIDocument.Document)
        a = TryCast(collector.WherePasses(lOrFilterLast).ToElements(), List(Of db.Element))

      End If
      Dim count As Integer = a.Count
      Dim count2 As Integer = 0
      Dim msg As String = Nothing
      For Each e As db.Element In a
        If TypeOf e Is db.Wall Then
          Dim w As db.Wall = TryCast(e, db.Wall)
          Dim anaWall As dbst.AnalyticalModel = w.GetAnalyticalModel()
          If Nothing IsNot anaWall AndAlso 0 < anaWall.GetCurves(dbst.AnalyticalCurveType.ActiveCurves).Count Then
            msg = "Analytical model for wall " & w.Id.IntegerValue.ToString() & Constants.vbCrLf
            ListAnalyticalModelWall(anaWall, msg)
            RacUtils.InfoMsg(msg)
            count2 += 1
          End If
        ElseIf TypeOf e Is db.Floor Then
          Dim f As db.Floor = TryCast(e, db.Floor)
          Dim anaFloor As dbst.AnalyticalModel = f.GetAnalyticalModel()
          If Nothing IsNot anaFloor AndAlso 0 < anaFloor.GetCurves(dbst.AnalyticalCurveType.ActiveCurves).Count Then
            msg = "Analytical model for floor " & f.Id.IntegerValue.ToString() & Constants.vbCrLf
            ListAnalyticalModelFloor(anaFloor, msg)
            RacUtils.InfoMsg(msg)
            count2 += 1
          End If
        ElseIf TypeOf e Is db.ContFooting Then
          Dim cf As db.ContFooting = TryCast(e, db.ContFooting)
          Dim ana3d As dbst.AnalyticalModel = cf.GetAnalyticalModel()
          If Nothing IsNot ana3d AndAlso 0 < ana3d.GetCurves(dbst.AnalyticalCurveType.ActiveCurves).Count Then
            msg = "Analytical model for continuous footing " & cf.Id.IntegerValue.ToString() & Constants.vbCrLf
            ListAnalyticalModelContFooting(ana3d, msg)
            RacUtils.InfoMsg(msg)
            count2 += 1
          End If
        ElseIf TypeOf e Is db.FamilyInstance Then
          Dim fi As db.FamilyInstance = TryCast(e, db.FamilyInstance)
          Dim categoryId As db.ElementId = fi.Category.Id
          If catStruCols.Id.Equals(categoryId) Then ' Structural Columns
            Dim anaFrame As dbst.AnalyticalModel = fi.GetAnalyticalModel()
            If Nothing IsNot anaFrame Then
              msg = "Analytical model for structural column " & fi.Id.IntegerValue.ToString() & Constants.vbCrLf
              Dim cur As db.Curve = anaFrame.GetCurve()
              ListCurve(cur, msg)
              ListRigidLinks(anaFrame, msg)
              ListSupportInfo(anaFrame, msg)
              RacUtils.InfoMsg(msg)
              count2 += 1
            End If
          ElseIf catStruFrmg.Id.Equals(categoryId) Then ' Structural Framing
            Dim anaFrame As dbst.AnalyticalModel = fi.GetAnalyticalModel()
            If Nothing IsNot anaFrame Then
              msg = "Analytical model for structural framing " & fi.StructuralType.ToString() & " " & fi.Id.IntegerValue.ToString() & Constants.vbCrLf
              Dim cur As db.Curve = anaFrame.GetCurve()
              ListCurve(cur, msg)
              ListRigidLinks(anaFrame, msg)

              ListSupportInfo(anaFrame, msg)
              RacUtils.InfoMsg(msg)
              count2 += 1
            End If
          ElseIf catStruFndt.Id.Equals(categoryId) Then ' Structural Foundations
            Dim anaLoc As dbst.AnalyticalModel = fi.GetAnalyticalModel()
            If Nothing IsNot anaLoc Then
              Dim p As db.XYZ = anaLoc.GetPoint()
              msg = "Analytical model for foundation " & fi.Id.IntegerValue.ToString() & Constants.vbCrLf & "  Location = " & RacUtils.PointString(p) & Constants.vbCrLf

              ListSupportInfo(anaLoc, msg)
              RacUtils.InfoMsg(msg)
              count2 += 1
            End If
          End If
        End If
      Next e
      Return Result.Succeeded
    End Function

    Public Sub ListCurve(ByRef crv As db.Curve, ByRef s As String)
      ' todo: rewrite the newline handling, this is still VB:

      If TypeOf crv Is db.Line Then
        Dim line As db.Line = TryCast(crv, db.Line)
        s &= "  LINE: " & RacUtils.PointString(line.GetEndPoint(0)) & " ; " & RacUtils.PointString(line.GetEndPoint(1)) & Constants.vbCrLf
      ElseIf TypeOf crv Is db.Arc Then
        Dim arc As db.Arc = TryCast(crv, db.Arc)
        s &= "  ARC: " & RacUtils.PointString(arc.GetEndPoint(0)) & " ; " & RacUtils.PointString(arc.GetEndPoint(1)) & " ; R=" & RacUtils.RealString(arc.Radius) & Constants.vbCrLf
      Else ' generic parametric curve
        If crv.IsBound Then
          s &= "  BOUND CURVE " & crv.GetType().Name & " - Tessellated result:" & Constants.vbCrLf
          Dim pts As IList(Of db.XYZ) = crv.Tessellate()
          For Each p As db.XYZ In pts
            s &= RacUtils.PointString(p) & Constants.vbCrLf
          Next p
        Else
          s &= "  UNBOUND CURVE - unnexpected in an analytical model!" & Constants.vbCrLf
        End If
      End If
    End Sub

    Public Sub ListAnalyticalModelWall(ByRef a As dbst.AnalyticalModel, ByRef s As String)
      Dim crvTemp As db.Curve = Nothing
      For Each crv As db.Curve In a.GetCurves(dbst.AnalyticalCurveType.ActiveCurves)
        crvTemp = crv
        ListCurve(crvTemp, s)
      Next crv

      ListSupportInfo(a, s)
    End Sub

    Public Sub ListAnalyticalModelFloor(ByRef anaFloor As dbst.AnalyticalModel, ByRef s As String)
      Dim crvTemp As db.Curve = Nothing
      For Each crv As db.Curve In anaFloor.GetCurves(dbst.AnalyticalCurveType.ActiveCurves)
        crvTemp = crv
        ListCurve(crvTemp, s)
      Next crv
      ListSupportInfo(anaFloor, s)
    End Sub

    Public Sub ListAnalyticalModelContFooting(ByRef ana3d As dbst.AnalyticalModel, ByRef s As String)
      Dim crvTemp As db.Curve = Nothing
      For Each crv As db.Curve In ana3d.GetCurves(dbst.AnalyticalCurveType.ActiveCurves)
        crvTemp = crv
        ListCurve(crvTemp, s)
      Next crv

      ListSupportInfo(ana3d, s)
    End Sub

    Public Sub ListSupportInfo(ByVal model As dbst.AnalyticalModel, ByRef s As String)
      If Nothing Is model Then
        s &= "There is no analytical model data with this element." & Constants.vbCrLf
      Else
        Dim listSupports As IList(Of dbst.AnalyticalModelSupport) = Nothing
        listSupports = model.GetAnalyticalModelSupports()
        If listSupports.Count > 0 Then
          s &= Constants.vbCrLf & "Supported = YES"
          s &= " by below element(s):"
          For Each supInfo As dbst.AnalyticalModelSupport In listSupports
            Dim idSupport As db.ElementId = supInfo.GetSupportingElement()
            Dim elemSupport As db.Element = m_doc.GetElement(idSupport)
            s &= Constants.vbCrLf & "  " & supInfo.GetSupportType().ToString() & " from element id=" & idSupport.IntegerValue.ToString() & " cat=" & elemSupport.Category.Name & " priority=" & supInfo.GetPriority().ToString()

          Next supInfo
          s &= Constants.vbCrLf
        Else
          s &= Constants.vbCrLf & "Supported = NO" & Constants.vbCrLf
        End If
      End If
    End Sub

    Public Sub ListRigidLinks(ByRef anaFrm As dbst.AnalyticalModel, ByRef s As String)
      s &= Constants.vbCrLf & "Rigid Link START = "
      Dim selector As New dbst.AnalyticalModelSelector(dbst.AnalyticalCurveSelector.StartPoint)
      Dim rigidLinkStart As db.Curve = anaFrm.GetRigidLink(selector)
      If Nothing Is rigidLinkStart Then
        s &= "None" & Constants.vbCrLf
      Else
        ListCurve(rigidLinkStart, s)
      End If
      s &= "Rigid Link END   = "

      selector = New dbst.AnalyticalModelSelector(dbst.AnalyticalCurveSelector.EndPoint)
      Dim rigidLinkEnd As db.Curve = anaFrm.GetRigidLink(selector)
      If Nothing Is rigidLinkEnd Then
        s &= "None" & Constants.vbCrLf
      Else
        ListCurve(rigidLinkEnd, s)
      End If
    End Sub
  End Class
#End Region ' Lab3_ListSelectedAnalyticalModels
End Namespace
