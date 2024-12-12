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

Namespace RstLabs
    ''' <summary>
    ''' Utility methods from the standard Revit API class.
    ''' </summary>
    Public Class RacUtils
        Shared Function RealString(ByVal a As Double) As String
            Return a.ToString("0.##")
        End Function

        Shared Function PointString(ByVal p As XYZ) As String
            Return String.Format("({0},{1},{2})", RealString(p.X), RealString(p.Y), RealString(p.Z))
        End Function



        ' Helper to get all Standard Family Instances for a given Category
        Shared Function GetAllStandardFamilyInstancesForACategory( _
            ByVal app As Revit.Application, _
            ByVal bic As BuiltInCategory) _
        As List(Of Revit.Element)

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance. . It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim elems As ElementSet = revitApp.Create.NewElementSet
            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    ' First check for the class, then for specific category name
            '    If TypeOf elem Is FamilyInstance Then
            '        Try
            '            If elem.Category.Name.Equals(catName) Then
            '                elems.Insert(elem)
            '            End If
            '        Catch
            '        End Try
            '    End If
            'Loop
            'Return elems

            '
            'The following code is using the new feature of Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim elements As New System.Collections.Generic.List(Of Revit.Element)
            Dim filterType As Revit.Filter = app.Create.Filter.NewTypeFilter(GetType(FamilyInstance))
            Dim filterCategory As Revit.Filter = app.Create.Filter.NewCategoryFilter(bic)
            Dim filterCombination As Revit.Filter = app.Create.Filter.NewLogicAndFilter(filterCategory, filterType)
            Dim nRetVal As Integer = app.ActiveDocument.Elements(filterCombination, elements)
            Return elements
        End Function

        'Helper to return Parameter as string
        Public Shared Function GetParameterValue(ByVal param As Parameter) As String

            Dim str As String
            Select Case param.StorageType
                Case StorageType.Double
                    str = param.AsDouble.ToString
                Case StorageType.Integer
                    str = param.AsInteger.ToString
                Case StorageType.String
                    str = param.AsString
                Case StorageType.ElementId
                    str = param.AsElementId.Value.ToString
                Case StorageType.None
                    str = "?NONE?"
                Case Else
                    str = "?ELSE?"
            End Select
            Return str
        End Function

        'Helper to get *specific* parameter by name
        Shared Function GetElemParam(ByVal elem As Revit.Element, ByVal name As String) As Parameter
            ' The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too.
            ' Revit 2009 API provide a overload method to get the Parameter via the parameter name string. It is much faster.
            'Dim parameters As Autodesk.Revit.ParameterSet = elem.Parameters
            'Dim parameter As Autodesk.Revit.Parameter
            'For Each parameter In parameters
            '    If (parameter.Definition.Name = name) Then
            '        Return parameter
            '    End If
            'Next
            'Return Nothing

            ' We use the new added overload method to retrieve the Parameter via the name string. This only works in Revit 2009 onward.
            Dim para As Parameter = elem.Parameter(name)
            Return para
        End Function
    End Class
End Namespace
