#Region "Header"
'Revit API .NET Labs

'Copyright (C) 2006-2011 by Autodesk, Inc.

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
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.Attributes
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.DB.Architecture
Imports Autodesk.Revit.UI
Imports FamilyItemFactory = Autodesk.Revit.Creation.FamilyItemFactory
#End Region

Namespace XtraVb

#Region "Lab7_1_CreateForm"
  ''' <summary>
  ''' Create a loft form using reference points and curve by points.
  ''' </summary>
  <Transaction(TransactionMode.Automatic)> _
  Public Class Lab7_1_CreateForm
    Implements IExternalCommand

    Public Function Execute( _
        ByVal commandData As ExternalCommandData, _
        ByRef message As String, _
        ByVal elements As ElementSet) _
        As Result _
        Implements IExternalCommand.Execute

      Dim app As UIApplication = commandData.Application
      Dim doc As Document = app.ActiveUIDocument.Document

      If Not doc.IsFamilyDocument OrElse Not doc.OwnerFamily.FamilyCategory.Name.Equals("Mass") Then
        message = "Please run this comand in a conceptual massing family document."
        Return Result.Failed
      End If
      Dim creator As FamilyItemFactory = doc.FamilyCreate

      ' Create profiles array
      Dim ref_ar_ar As New ReferenceArrayArray()

      ' Create first profile
      Dim ref_ar As New ReferenceArray()

      Dim y As Integer = 100
      Dim x As Integer = 50
      Dim pa As New XYZ(-x, y, 0)
      Dim pb As New XYZ(x, y, 0)
      Dim pc As New XYZ(0, y + 10, 10)
      Dim curve As CurveByPoints = FormUtils.MakeCurve(creator, pa, pb, pc)
      ref_ar.Append(curve.GeometryCurve.Reference)
      ref_ar_ar.Append(ref_ar)

      ' Create second profile
      ref_ar = New ReferenceArray()

      y = 40
      pa = New XYZ(-x, y, 5)
      pb = New XYZ(x, y, 5)
      pc = New XYZ(0, y, 25)
      curve = FormUtils.MakeCurve(creator, pa, pb, pc)
      ref_ar.Append(curve.GeometryCurve.Reference)
      ref_ar_ar.Append(ref_ar)

      ' Create third profile
      ref_ar = New ReferenceArray()

      y = -20
      pa = New XYZ(-x, y, 0)
      pb = New XYZ(x, y, 0)
      pc = New XYZ(0, y, 15)
      curve = FormUtils.MakeCurve(creator, pa, pb, pc)
      ref_ar.Append(curve.GeometryCurve.Reference)
      ref_ar_ar.Append(ref_ar)

      ' Create fourth profile
      ref_ar = New ReferenceArray()

      y = -60
      pa = New XYZ(-x, y, 0)
      pb = New XYZ(x, y, 0)
      pc = New XYZ(0, y + 10, 20)
      curve = FormUtils.MakeCurve(creator, pa, pb, pc)
      ref_ar.Append(curve.GeometryCurve.Reference)
      ref_ar_ar.Append(ref_ar)

      Dim form As Form = creator.NewLoftForm(True, ref_ar_ar)

      Return Result.Succeeded
    End Function

  End Class
#End Region

#Region "Lab7_2_CreateDividedSurface"
  ''' <summary>
  ''' Create a divided surface using reference of a face of the form.
  ''' </summary>
  <Transaction(TransactionMode.Automatic)> _
  Public Class Lab7_2_CreateDividedSurface
    Implements IExternalCommand

    Public Function Execute( _
        ByVal commandData As ExternalCommandData, _
        ByRef message As String, _
        ByVal elements As ElementSet) _
        As Result _
        Implements IExternalCommand.Execute

      Dim app As UIApplication = commandData.Application
      Dim doc As Document = app.ActiveUIDocument.Document

      Dim creApp As Autodesk.Revit.Creation.Application = app.Application.Create

      Try
        ' find forms in the model:

        Dim forms As New FilteredElementCollector(doc)
        forms.OfCategory(BuiltInCategory.OST_MassForm)
        ' was OST_MassSurface
        For Each form As Form In forms
          ' create the divided surface on the loft form:

          Dim factory As FamilyItemFactory = doc.FamilyCreate
          Dim options As Options = creApp.NewGeometryOptions()
          options.ComputeReferences = True
          options.View = doc.ActiveView
          Dim element As GeometryElement = form.Geometry(options)
          Dim geoObjectArray As GeometryObjectArray = element.Objects

          For j As Integer = 0 To geoObjectArray.Size - 1
            Dim geoObject As GeometryObject = geoObjectArray.Item(j)
            Dim solid As Solid = TryCast(geoObject, Solid)
            For Each face As Face In solid.Faces
              If face.Reference IsNot Nothing Then
                If face IsNot Nothing Then
                  Dim divSurface As DividedSurface = factory.NewDividedSurface(face.Reference)
                End If
              End If
            Next
          Next
        Next
        Return Result.Succeeded
      Catch ex As Exception
        message = ex.Message
        Return Result.Failed
      End Try
    End Function


  End Class
#End Region

#Region "Lab7_3_ChangeTilePattern"
  ''' <summary>
  ''' Change the tiling pattern of the divided surface using the built-in TilePattern enumeration.
  ''' </summary>
  <Transaction(TransactionMode.Automatic)> _
  Public Class Lab7_3_ChangeTilePattern
    Implements IExternalCommand

    Public Function Execute( _
        ByVal commandData As ExternalCommandData, _
        ByRef message As String, _
        ByVal elements As ElementSet) _
        As Result _
        Implements IExternalCommand.Execute

      Dim app As UIApplication = commandData.Application
      Dim doc As Document = app.ActiveUIDocument.Document

      Try
        ' find forms in the model:

        Dim forms As New FilteredElementCollector(doc)
        forms.OfClass(GetType(Form))

        For Each form As Form In forms
          ' access the divided surface data from the form:

          Dim dsData As DividedSurfaceData = form.GetDividedSurfaceData()

          If dsData IsNot Nothing Then
            ' get the references associated with the divided surfaces
            For Each reference As Reference In dsData.GetReferencesWithDividedSurfaces()
              Dim divSurface As DividedSurface = dsData.GetDividedSurfaceForReference(reference)

              Dim count As Integer = 0
              Dim tilepatterns As TilePatterns = doc.Settings.TilePatterns
              For Each i As TilePatternsBuiltIn In System.Enum.GetValues(GetType(TilePatternsBuiltIn))
                If count.Equals(3) Then
                  ' Warning: 'Autodesk.Revit.DB.Element.ObjectType' is obsolete:
                  ' 'Use Element.GetTypeId() and Element.ChangeTypeId() instead.'
                  '
                  'divSurface.ObjectType = tilepatterns.GetTilePattern(i)

                  divSurface.ChangeTypeId(tilepatterns.GetTilePattern(i).Id)

                  Exit For
                End If
                count += 1
              Next
            Next
          End If
        Next
      Catch ex As Exception
        message = ex.Message
        Return Result.Failed
      End Try
      Return Result.Succeeded
    End Function

  End Class
#End Region

#Region "Utilities"
  ''' <summary>
  ''' This class is utility class for form creation.
  ''' </summary>
  Public Class FormUtils
    ''' <summary>
    ''' Create a CurveByPoints element by three given points
    ''' </summary>
    Public Shared Function MakeCurve( _
        ByVal creator As FamilyItemFactory, _
        ByVal pa As XYZ, ByVal pb As XYZ, _
        ByVal pc As XYZ) _
        As CurveByPoints

      Dim rpa As ReferencePoint = creator.NewReferencePoint(pa)
      Dim rpb As ReferencePoint = creator.NewReferencePoint(pb)
      Dim rpc As ReferencePoint = creator.NewReferencePoint(pc)

      Dim arr As New ReferencePointArray()

      arr.Append(rpa)
      arr.Append(rpb)
      arr.Append(rpc)

      Return creator.NewCurveByPoints(arr)
    End Function
  End Class
#End Region

End Namespace
