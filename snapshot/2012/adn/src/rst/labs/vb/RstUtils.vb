#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2007-2008 by Autodesk, Inc.
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
Imports System.Linq
Imports System.Collections.Generic
Imports db = Autodesk.Revit.DB
Imports dbst = Autodesk.Revit.DB.Structure
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.ApplicationServices

#End Region ' Namespaces

Namespace RstLabs
  ''' <summary>
  ''' Revit Structure utilities.
  ''' </summary>
  Friend NotInheritable Class RstUtils
#Region "Structural Element Description"
    Private Sub New()
    End Sub
    Public Shared Function StructuralElementDescription(ByVal e As db.FamilyInstance) As String
      Dim bic As db.BuiltInCategory = db.BuiltInCategory.OST_StructuralFraming
      Dim cat As db.Category = e.Document.Settings.Categories.Item(bic)
      Dim hasCat As Boolean = (Nothing IsNot e.Category)
      Dim hasUsage As Boolean = hasCat AndAlso e.Category.Id.Equals(cat.Id)
      Return e.Name & " Id=" & e.Id.IntegerValue.ToString() & (If(hasCat, ", Category=" & e.Category.Name, String.Empty)) & ", Type=" & e.Symbol.Name + (If(hasUsage, ", Struct.Usage=" & e.StructuralUsage.ToString(), String.Empty)) & ", Struct.Type=" & e.StructuralType.ToString()
    End Function

    Public Shared Function StructuralElementDescription(ByVal e As db.ContFooting) As String
      Return e.Name & " Id=" & e.Id.IntegerValue.ToString() & ", Category=" & e.Category.Name & ", Type=" & e.FootingType.Name
    End Function

    Public Shared Function StructuralElementDescription(ByVal e As db.Floor) As String
      Return e.Name & " Id=" & e.Id.IntegerValue.ToString() & ", Category=" & e.Category.Name & ", Type=" & e.FloorType.Name & ", Struct.Usage=" & e.StructuralUsage.ToString() ' can throw exception "only Beam and Brace support Structural Usage!"

    End Function

    Public Shared Function StructuralElementDescription(ByVal e As db.Wall) As String
      Return e.Name & " Id=" & e.Id.IntegerValue.ToString() & ", Category=" & e.Category.Name & ", Type=" & e.WallType.Name & ", Struct.Usage=" & e.StructuralUsage.ToString() ' can throw exception "only Beam and Brace support Structural Usage!"

    End Function
#End Region ' Structural Element Description

#Region "Retrieve specific element collections"
    ''' <summary>
    ''' Return all instances of the specified class..
    ''' </summary>
    Public Shared Function GetInstanceOfClass(ByVal doc As db.Document, ByVal type As Type) As IList(Of db.Element)
      Dim collector As New db.FilteredElementCollector(doc)
      Return collector.OfClass(type).ToElements()      
    End Function

    ''' <summary>
    ''' Return all instances of the specified class,including the derevided class instances.
    ''' </summary>
    Public Shared Function GetInstanceOfClass(ByVal doc As db.Document, ByVal type As Type, ByVal bInverted As Boolean) As IList(Of db.Element)
      Dim collector As New db.FilteredElementCollector(doc)
      Dim classFilter As New db.ElementClassFilter(type, bInverted)
      Return collector.WherePasses(classFilter).ToElements()
    End Function

    ''' <summary>
    ''' Return all loads in the current active document,
    ''' i.e. any objects derived from LoadBase, sorted by load type.
    ''' </summary>
    Public Shared Sub GetAllSpecificLoads(ByVal doc As db.Document, ByRef pointLoads As List(Of db.Element), ByRef lineLoads As List(Of db.Element), ByRef areaLoads As List(Of db.Element))
      ' More efficient if we loop only once and sort all in one go.
      ' This was more important in 2008 and earlier, before the filtering
      ' feature was introduced in Revit 2009.
      '
      '      IEnumerator iter = app.ActiveDocument.Elements;
      '      while( iter.MoveNext() )
      '      {
      '        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
      '        if( elem is PointLoad )
      '        {
      '          pointLoads.Insert( elem );
      '        }
      '        else if( elem is LineLoad )
      '        {
      '          lineLoads.Insert( elem );
      '        }
      '        else if( elem is AreaLoad )
      '        {
      '          areaLoads.Insert( elem );
      '        }
      '      }
      '

      ' code for 2011 version.
      Dim a As IList(Of db.Element)
      a = GetInstanceOfClass(doc, GetType(dbst.LoadBase), False)
      For Each elem As db.Element In a
        If TypeOf elem Is dbst.PointLoad Then
          pointLoads.Add(elem)
        ElseIf TypeOf elem Is dbst.LineLoad Then
          lineLoads.Add(elem)
        ElseIf TypeOf elem Is dbst.AreaLoad Then
          areaLoads.Add(elem)
        End If
      Next elem
    End Sub



    ''' <summary>
    ''' Return all structural walls in active document.
    ''' </summary>
    Public Shared Function GetAllStructuralWalls(ByVal doc As db.Document) As List(Of db.Element)

      Dim collector As db.FilteredElementCollector = New db.FilteredElementCollector(doc)
      collector.OfClass(GetType(db.Wall))

      ' We could check if the wall is anything but non-bearing in one of the two following ways:
      ' If Not w.Parameter(db.BuiltInParameter.WALL_STRUCTURAL_USAGE_TEXT_PARAM).AsString.Equals("Non-bearing") Then
      ' If Not w.Parameter(db.BuiltInParameter.WALL_STRUCTURAL_USAGE_PARAM).AsInteger = 0 Then
      ' ... but it is more generic and precise to make sure that analytical model exists
      ' (in theory, one can set the wall to bearing and still uncheck Analytical):
      Return New List(Of db.Element)( _
                  From wall In collector _
                  Where Nothing IsNot wall.GetAnalyticalModel() _
                  Select wall)

    End Function

    ''' <summary>
    ''' Return all structural floors in active document.
    ''' </summary>
    Public Shared Function GetAllStructuralFloors(ByVal doc As db.Document) As List(Of db.Element)
            Dim floors As IList(Of db.Element)
      floors = GetInstanceOfClass(doc, GetType(db.Floor))
      Dim elems As List(Of db.Element) = New List(Of db.Element)()
      For Each f As db.Floor In floors
        Dim anaMod As dbst.AnalyticalModel = f.GetAnalyticalModel()
        If Nothing IsNot anaMod Then
          '
          ' For floors, looks like we need to have additional check:
          ' for non-structural floors anaMod is NOT null, but it IS empty!
          '
          '          AnalyticalModelFloor floorAnaMod = anaMod as AnalyticalModelFloor;
          If 0 < anaMod.GetCurves(dbst.AnalyticalCurveType.RawCurves).Count Then
            elems.Add(f)
          End If
        End If
      Next f
      Return elems

      'Dim collector As db.FilteredElementCollector = New db.FilteredElementCollector(doc)
      'collector.OfClass(GetType(db.Floor))
      'Dim am As dbst.AnalyticalModel
      'Return New List(Of db.Element)(From floor In collector _
      '                                 Where (Nothing IsNot (am = floor.GetAnalyticalModel()) And (am.GetCurves(dbst.AnalyticalCurveType.RawCurves).Count > 0)) _
      '                                 Select floor)

    End Function

    ''' <summary>
    ''' Return all structural continuous footings in active document.
    ''' </summary>
    Public Shared Function GetAllStructuralContinuousFootings(ByVal doc As db.Document) As List(Of db.Element)
      'Dim a As New List(Of db.Element)()
      'a = GetInstanceOfClass(doc, GetType(db.ContFooting))
      'Dim elems As List(Of db.Element) = New List(Of db.Element)()
      'For Each cf As db.ContFooting In a
      '  If Nothing IsNot cf.GetAnalyticalModel() Then
      '    elems.Add(cf)
      '  End If
      'Next cf
      'Return elems

      Dim collector As db.FilteredElementCollector = New db.FilteredElementCollector(doc)
      collector.OfClass(GetType(db.ContFooting))
      Return New List(Of db.Element)(From cf In collector _
                                      Where (Nothing IsNot cf.GetAnalyticalModel()) _
                                      Select cf)

    End Function
#End Region ' Retrieve specific element collections
  End Class
End Namespace
