' 
' (C) Copyright 2003-2009 by Autodesk, Inc.
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

Imports Autodesk
Imports Autodesk.Revit
Imports Autodesk.Revit.Application
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Geometry

''' <summary>
''' Implements the Revit add-in interface IExternalCommand
''' </summary>
''' <remarks></remarks>
Public Class ElementViewer
    Implements Autodesk.Revit.IExternalCommand

    'User preferences for parsing of geometry.
    Private mOptions As Autodesk.Revit.Geometry.Options
    'A transformation set of the affine 3-space
    Private mTransformations As Autodesk.Revit.Collections.Array
    'Represents the Autodesk Revit Application, 
    'providing access to documents, options and other application wide data and settings.
    Private mApplication As Autodesk.Revit.Application
    'the element wire frame to be draw.
    Private mViewer As RevitViewer.Wireframe

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

        mApplication = commandData.Application
        mOptions = commandData.Application.Create.NewGeometryOptions
        mOptions.DetailLevel = Autodesk.Revit.Geometry.Options.DetailLevels.Fine
        mViewer = New RevitViewer.Wireframe

        Dim selSet As Autodesk.Revit.ElementSet = commandData.Application.ActiveDocument.Selection.Elements
        Dim elem As Autodesk.Revit.Element

        For Each elem In selSet
            DrawElement(elem) ' this one handles Group. 
        Next

        If selSet.Size > 0 Then
            mViewer.ShowModal()
        Else
            System.Windows.Forms.MessageBox.Show("Please select some elements first.")
        End If

        mApplication = Nothing
        mViewer = Nothing

        Return Autodesk.Revit.IExternalCommand.Result.Succeeded

    End Function


  Private Sub PushTransformation(ByVal transform As Autodesk.Revit.Geometry.Transform)
    If (mTransformations Is Nothing) Then
      mTransformations = mApplication.Create.NewArray
    End If
    mTransformations.Append(transform)
  End Sub

  Private Sub PopTransformation()

    If (mTransformations Is Nothing) Then
      Exit Sub
    End If

    If (mTransformations.Size = 1) Then
      mTransformations = Nothing
      Exit Sub
    End If

    Dim newTransformations As Autodesk.Revit.Collections.Array
    newTransformations = mApplication.Create.NewArray
    Dim i As Integer
    For i = 0 To mTransformations.Size() - 2

      newTransformations.Append(mTransformations.Item(i))

    Next

    mTransformations = newTransformations

  End Sub
  

  ' Note: Some element does not expose geometry, for example, curtain wall and dimension.
  ' In case of a curtain wall, try selecting a whole wall by a window/box instead of a single pick. 
  ' It will then select internal components and be able to display its geometry.
  ' 
  Private Sub DrawElement(ByVal elem As Autodesk.Revit.Element)

    ' if it is a Group. we will need to look at its components. 
    If TypeOf elem Is Autodesk.Revit.Elements.Group Then

      Dim group As Autodesk.Revit.Elements.Group = elem
      Dim members As Autodesk.Revit.ElementArray = group.Members

            Dim elm As Autodesk.Revit.Element
      For Each elm In members
        DrawElement(elm)
      Next

    Else

      ' not a group. look at the geom data. 
            Dim geom As Autodesk.Revit.Geometry.Element = elem.Geometry(mOptions)
      If Not (geom Is Nothing) Then
        DrawElement(geom)
      End If

    End If

  End Sub

    ''' <summary>
    ''' Draw geometry of element.
    ''' </summary>
    ''' <param name="elementGeom"></param>
    ''' <remarks></remarks>
  Private Sub DrawElement(ByVal elementGeom As Autodesk.Revit.Geometry.Element)

    If elementGeom Is Nothing Then
      Exit Sub
    End If

    Dim geomObject As Autodesk.Revit.Geometry.GeometryObject

    For Each geomObject In elementGeom.Objects

      If (TypeOf geomObject Is Autodesk.Revit.Geometry.Curve) Then
        DrawCurve(geomObject)
      ElseIf (TypeOf geomObject Is Autodesk.Revit.Geometry.Instance) Then
        DrawInstance(geomObject)
      ElseIf (TypeOf geomObject Is Autodesk.Revit.Geometry.Mesh) Then
        DrawMesh(geomObject)
      ElseIf (TypeOf geomObject Is Autodesk.Revit.Geometry.Solid) Then
        DrawSolid(geomObject)
      End If

    Next

  End Sub

    ''' <summary>
    ''' add the primitive line to the Wireframe object.
    ''' </summary>
    ''' <param name="startPoint"></param>
    ''' <param name="endPoint"></param>
    ''' <remarks></remarks>
  Private Sub ViewerDrawLine(ByRef startPoint As Autodesk.Revit.Geometry.XYZ, ByRef endPoint As Autodesk.Revit.Geometry.XYZ)

    If (mViewer Is Nothing) Then
      Exit Sub
    End If

    Dim transformedStart As Autodesk.Revit.Geometry.XYZ
    transformedStart = startPoint
    Dim transformedEnd As Autodesk.Revit.Geometry.XYZ
    transformedEnd = endPoint

    If Not (mTransformations Is Nothing) Then
      Dim iter As IEnumerator
      iter = mTransformations.ReverseIterator
      Do While (iter.MoveNext())
        Dim transformation As Autodesk.Revit.Geometry.Transform
        transformation = iter.Current
        transformedStart = TransformPoint(transformation, transformedStart)
        transformedEnd = TransformPoint(transformation, transformedEnd)
      Loop
    End If

    mViewer.Add(transformedStart.X, transformedStart.Y, transformedStart.Z, transformedEnd.X, transformedEnd.Y, transformedEnd.Z)

  End Sub

    ''' <summary>
    ''' transform point to be fit.
    ''' </summary>
    ''' <param name="transform"></param>
    ''' <param name="point"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
  Public Function TransformPoint(ByVal transform As Autodesk.Revit.Geometry.Transform, ByRef point As Autodesk.Revit.Geometry.XYZ) As Autodesk.Revit.Geometry.XYZ

    Dim result As New Autodesk.Revit.Geometry.XYZ

    Dim i As Integer
    For i = 0 To 2

      result.Item(i) = transform.Origin.Item(i)
      Dim j As Integer
      For j = 0 To 2
        result.Item(i) = result.Item(i) + (transform.Basis(j).Item(i) * point.Item(j))
      Next

    Next

    Return result

  End Function

    ''' <summary>
    ''' add the primitive curve to the Wireframe object.
    ''' </summary>
    ''' <param name="geomCurve"></param>
    ''' <remarks></remarks>
  Private Sub DrawCurve(ByVal geomCurve As Autodesk.Revit.Geometry.Curve)

    DrawPoints(geomCurve.Tessellate)

  End Sub

    ''' <summary>
    ''' add the primitive points to the Wireframe object.
    ''' </summary>
    ''' <param name="points"></param>
    ''' <remarks></remarks>
  Private Sub DrawPoints(ByVal points As Autodesk.Revit.Geometry.XYZArray)

    Dim previousPoint As Autodesk.Revit.Geometry.XYZ
    previousPoint = points.Item(0)
    Dim point As Autodesk.Revit.Geometry.XYZ

    Dim i As Integer
    For i = 0 To points.Size - 1

      point = points.Item(i)
      If (i <> 0) Then
        ViewerDrawLine(previousPoint, point)
      End If

      previousPoint = point

    Next

  End Sub

    ''' <summary>
    ''' add the primitive instance to the Wireframe object.
    ''' </summary>
    ''' <param name="geomInstance"></param>
    ''' <remarks></remarks>
  Private Sub DrawInstance(ByVal geomInstance As Autodesk.Revit.Geometry.Instance)

    PushTransformation(geomInstance.Transform)

      Dim geomSymbol As Autodesk.Revit.Geometry.Element
    geomSymbol = geomInstance.SymbolGeometry

    If Not (geomSymbol Is Nothing) Then
      DrawElement(geomSymbol)
    End If

    PopTransformation()

  End Sub

    ''' <summary>
    ''' add the primitive mesh to the Wireframe object.
    ''' </summary>
    ''' <param name="geomMesh"></param>
    ''' <remarks></remarks>
  Private Sub DrawMesh(ByVal geomMesh As Autodesk.Revit.Geometry.Mesh)

    Dim i As Integer

    For i = 0 To geomMesh.NumTriangles - 1
      Dim triangle As Autodesk.Revit.Geometry.MeshTriangle
      triangle = geomMesh.Triangle(i)

      ViewerDrawLine(triangle.Vertex(0), triangle.Vertex(1))
      ViewerDrawLine(triangle.Vertex(1), triangle.Vertex(2))
      ViewerDrawLine(triangle.Vertex(2), triangle.Vertex(0))

    Next

  End Sub

    ''' <summary>
    ''' add the primitive solid to the Wireframe object.
    ''' </summary>
    ''' <param name="geomSolid"></param>
    ''' <remarks></remarks>
  Private Sub DrawSolid(ByVal geomSolid As Autodesk.Revit.Geometry.Solid)

    Dim face As Autodesk.Revit.Geometry.Face
    For Each face In geomSolid.Faces
      DrawFace(face)
    Next

    Dim Edge As Autodesk.Revit.Geometry.Edge
    For Each Edge In geomSolid.Edges
      DrawEdge(Edge)
    Next

  End Sub

    ''' <summary>
    ''' add the primitive edge to the Wireframe object.
    ''' </summary>
    ''' <param name="geomEdge"></param>
    ''' <remarks></remarks>
  Private Sub DrawEdge(ByVal geomEdge As Autodesk.Revit.Geometry.Edge)

    DrawPoints(geomEdge.Tessellate)

  End Sub

    ''' <summary>
    ''' add the primitive face to the Wireframe object.
    ''' </summary>
    ''' <param name="geomFace"></param>
    ''' <remarks></remarks>
  Private Sub DrawFace(ByVal geomFace As Autodesk.Revit.Geometry.Face)

    DrawMesh(geomFace.Triangulate)

  End Sub

End Class