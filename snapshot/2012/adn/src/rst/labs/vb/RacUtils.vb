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
Imports System.Collections.Generic
Imports WinForms = System.Windows.Forms
Imports db = Autodesk.Revit.DB
Imports Autodesk.Revit.UI
#End Region ' Namespaces

Namespace RstLabs
  ''' <summary>
  ''' Utility methods from the standard Revit API class.
  ''' </summary>
  Friend NotInheritable Class RacUtils
#Region "Formatting and message handlers"
    ''' <summary>
    ''' Format a real number and return its string representation.
    ''' </summary>
    Private Sub New()
    End Sub
    Public Shared Function RealString(ByVal a As Double) As String
      Return a.ToString("0.##")
    End Function

    ''' <summary>
    ''' Format a point or vector and return its string representation.
    ''' </summary>
    Public Shared Function PointString(ByVal p As db.XYZ) As String
      Return String.Format("({0},{1},{2})", RealString(p.X), RealString(p.Y), RealString(p.Z))
    End Function

    ''' <summary>
    ''' MessageBox wrapper for informational message.
    ''' </summary>
    Public Shared Sub InfoMsg(ByVal msg As String)
      WinForms.MessageBox.Show(msg, "Revit Structure API Labs", WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information)
    End Sub
#End Region ' Formatting and message handlers

    ''' <summary>
    ''' Helper to get all standard family instances for a given category
    ''' using the filter features provided by the Revit 2009 API.
    ''' </summary>
    Public Shared Function GetAllStandardFamilyInstancesForACategory(ByVal doc As db.Document, ByVal bic As db.BuiltInCategory) As IList(Of db.Element)
            Dim elements As IList(Of db.Element)
      'The following commented code worked for Revit 2010. Revit 2011 has new way to collect elements
      '
      '      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( FamilyInstance ) );
      '      Filter filterCategory = app.Create.Filter.NewCategoryFilter( bic );
      '      Filter filterAnd = app.Create.Filter.NewLogicAndFilter( filterType, filterCategory );
      '      app.ActiveDocument.Elements( filterAnd, elements );
      '

      'Below code works for Revit 2011. It is very concise.
      'This will return two kinds of foundation. standalone foundation and continuous foundation.
      Dim collector As New db.FilteredElementCollector(doc)
      Dim filterFamilyInstance As New db.ElementClassFilter(GetType(db.FamilyInstance))
      elements = collector.OfCategory(bic).WherePasses(filterFamilyInstance).ToElements()
      Return elements
    End Function

    ''' <summary>
    ''' Helper to return parameter value as string.
    ''' One can also use param.AsValueString() to
    ''' get the user interface representation with unit.
    ''' </summary>
    Public Shared Function GetParameterValue(ByVal param As db.Parameter) As String
      Dim s As String
      Select Case param.StorageType
        Case db.StorageType.Double
          '
          ' the internal database unit for all lengths is feet.
          ' for instance, if a given room perimeter is returned as
          ' 102.36 as a double and the display unit is millimeters,
          ' then the length will be displayed as
          ' peri = 102.36220472440
          ' peri * 12 *25.4
          ' 31200 mm
          '
          's = param.AsValueString(); // value seen by user, in display units
          s = RealString(param.AsDouble()) ' database value, internal units, e.g. feet
        Case db.StorageType.Integer
          s = param.AsInteger().ToString()
        Case db.StorageType.String
          s = param.AsString()
        Case db.StorageType.ElementId
          s = param.AsElementId().IntegerValue.ToString()
        Case db.StorageType.None
          s = "?NONE?"
        Case Else
          s = "?ELSE?"
      End Select
      Return s
    End Function

    ''' <summary>
    ''' Helper to return parameter value as string, with additional
    ''' support for element id to display the element type referred to.
    ''' </summary>
    Public Shared Function GetParameterValue2(ByVal param As db.Parameter, ByVal doc As db.Document) As String
      Dim s As String
      If db.StorageType.ElementId = param.StorageType AndAlso Nothing IsNot doc Then
        Dim id As db.ElementId = param.AsElementId()
        Dim i As Integer = id.IntegerValue
        s = If((0 <= i), String.Format("{0}: {1}", i, doc.Element(id).Name), i.ToString())
      Else
        s = GetParameterValue(param)
      End If
      Return s
    End Function

  End Class
End Namespace