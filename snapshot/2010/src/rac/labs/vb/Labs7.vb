#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2007-2009 by Autodesk, Inc.
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

#Region "Namespaces"
Imports System
Imports System.Collections.Generic
Imports Autodesk.Revit
Imports Autodesk.Revit.Geometry
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Enums
Imports XYZ2 = Autodesk.Revit.Geometry.XYZ
Imports Microsoft.VisualBasic
#End Region

Namespace Labs

#Region "Lab7_1_CreateForm"
    Public Class Lab7_1_CreateForm
        Implements IExternalCommand
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute

            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument

            If (doc.IsFamilyDocument And doc.OwnerFamily.FamilyCategory.Name.Equals("Mass")) Then

                ' Create profiles array 
                Dim ref_ar_ar As New ReferenceArrayArray()

                ' Create first profile 
                Dim ref_ar As New ReferenceArray()

                Dim y As Integer = 100
                Dim x As Integer = 50
                Dim ptA As New XYZ2(-x, y, 0)
                Dim ptB As New XYZ2(x, y, 0)
                Dim ptC As New XYZ2(0, y + 10, 10)
                Dim curve As CurveByPoints = FormUtils.MakeCurve(app, ptA, ptB, ptC)
                ref_ar.Append(curve.GeometryCurve.Reference)
                ref_ar_ar.Append(ref_ar)


                ' Create second profile 
                ref_ar = New ReferenceArray()

                y = 40
                ptA = New XYZ2(-x, y, 5)
                ptB = New XYZ2(x, y, 5)
                ptC = New XYZ2(0, y, 25)
                curve = FormUtils.MakeCurve(app, ptA, ptB, ptC)
                ref_ar.Append(curve.GeometryCurve.Reference)
                ref_ar_ar.Append(ref_ar)

                ' Create third profile 
                ref_ar = New ReferenceArray()

                y = -20
                ptA = New XYZ2(-x, y, 0)
                ptB = New XYZ2(x, y, 0)
                ptC = New XYZ2(0, y, 15)
                curve = FormUtils.MakeCurve(app, ptA, ptB, ptC)
                ref_ar.Append(curve.GeometryCurve.Reference)
                ref_ar_ar.Append(ref_ar)

                ' Create fourth profile 
                ref_ar = New ReferenceArray()

                y = -60
                ptA = New XYZ2(-x, y, 0)
                ptB = New XYZ2(x, y, 0)
                ptC = New XYZ2(0, y + 10, 20)
                curve = FormUtils.MakeCurve(app, ptA, ptB, ptC)
                ref_ar.Append(curve.GeometryCurve.Reference)
                ref_ar_ar.Append(ref_ar)

                Dim form As Form = doc.FamilyCreate.NewLoftForm(True, ref_ar_ar)

                Return IExternalCommand.Result.Succeeded
            Else
                MsgBox("Please load a conceptual massing family document!")
                Return IExternalCommand.Result.Failed

            End If
        End Function
    End Class
#End Region

#Region "Lab7_2_CreateDividedSurface"
    Public Class Lab7_2_CreateDividedSurface
        Implements IExternalCommand
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute
            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument
            Try
                ' find forms in the model by filter: 
                Dim filterForm As Filter = app.Create.Filter.NewCategoryFilter(BuiltInCategory.OST_MassSurface)
                Dim forms As New List(Of Autodesk.Revit.Element)()
                Dim iForms As Integer = doc.Elements(filterForm, forms)
                For Each form As Form In forms
                    ' Now, lets create the Divided surface on the loft form 
                    Dim fac As Autodesk.Revit.Creation.FamilyItemFactory = doc.FamilyCreate
                    Dim options As Autodesk.Revit.Geometry.Options = app.Create.NewGeometryOptions()
                    options.ComputeReferences = True
                    options.View = doc.ActiveView
                    Dim element As Autodesk.Revit.Geometry.Element = form.Geometry(options)

                    Dim geoObjectArray As GeometryObjectArray = element.Objects
                    'enum the geometry element 
                    For j As Integer = 0 To geoObjectArray.Size - 1
                        Dim geoObject As GeometryObject = geoObjectArray.Item(j)
                        Dim solid As Solid = TryCast(geoObject, Solid)
                        For Each face As Autodesk.Revit.Geometry.Face In solid.Faces
                            If face.Reference IsNot Nothing Then
                                If face IsNot Nothing Then
                                    Dim divSurface As DividedSurface = fac.NewDividedSurface(face.Reference)
                                End If
                            End If
                        Next
                    Next
                Next
            Catch
                Return IExternalCommand.Result.Failed
            End Try

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class
#End Region

#Region "Lab7_3_ChangeTilePattern"
    Public Class Lab7_3_ChangeTilePattern
        Implements IExternalCommand
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute
            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument
            Try
                ' find forms in the model by filter: 
                Dim filterForm As Filter = app.Create.Filter.NewTypeFilter(GetType(Form))
                Dim forms As New List(Of Autodesk.Revit.Element)()
                Dim iForms As Integer = doc.Elements(filterForm, forms)
                For Each form As Form In forms
                    ' Get access to the divided surface data from the form 
                    Dim dsData As DividedSurfaceData = form.GetDividedSurfaceData()
                    If dsData IsNot Nothing Then
                        ' get the references associated with the divided surfaces 
                        For Each reference As Reference In dsData.GetReferencesWithDividedSurfaces()
                            Dim divSurface As DividedSurface = dsData.GetDividedSurfaceForReference(reference)

                            Dim count As Integer = 0
                            Dim tilepatterns As TilePatterns = doc.Settings.TilePatterns
                            For Each TilePatternEnum As TilePatternsBuiltIn In [Enum].GetValues(GetType(TilePatternsBuiltIn))
                                If count.Equals(3) Then
                                    divSurface.ObjectType = tilepatterns.GetTilePattern(TilePatternEnum)
                                    Exit For
                                End If
                                count = count + 1
                            Next
                        Next
                    End If
                Next
            Catch
                Return IExternalCommand.Result.Failed
            End Try
            Return IExternalCommand.Result.Succeeded
        End Function
    End Class
#End Region

#Region "Utilities"
    ''' <summary> 
    ''' This class is utility class for form creation. 
    ''' </summary> 
    Public Class FormUtils

        ' Create curve by points element by three points 
        Public Shared Function MakeCurve( _
            ByVal app As Application, _
            ByVal ptA As XYZ2, _
            ByVal ptB As XYZ2, _
            ByVal ptC As XYZ2) _
        As CurveByPoints
            Dim doc As Document = app.ActiveDocument
            Dim refPtA As ReferencePoint = doc.FamilyCreate.NewReferencePoint(ptA)
            Dim refPtB As ReferencePoint = doc.FamilyCreate.NewReferencePoint(ptB)
            Dim refPtC As ReferencePoint = doc.FamilyCreate.NewReferencePoint(ptC)

            Dim refPtsArray As ReferencePointArray = New ReferencePointArray()
            refPtsArray.Append(refPtA)
            refPtsArray.Append(refPtB)
            refPtsArray.Append(refPtC)

            Dim curve As CurveByPoints = doc.FamilyCreate.NewCurveByPoints(refPtsArray)

            Return curve
        End Function

    End Class
#End Region

End Namespace

