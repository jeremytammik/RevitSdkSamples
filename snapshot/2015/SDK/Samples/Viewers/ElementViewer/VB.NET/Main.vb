' 
' (C) Copyright 2003-2014 by Autodesk, Inc.
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
Imports System.Collections.Generic

Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.UI

''' <summary>
''' Implements the Revit add-in interface IExternalCommand
''' </summary>
''' <remarks></remarks>
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class ElementViewer
    Implements Autodesk.Revit.UI.IExternalCommand

    'User preferences for parsing of geometry.
    Private mOptions As Autodesk.Revit.DB.Options
    'A transformation set of the affine 3-space
    Private mTransformations As System.Collections.Generic.List(Of Autodesk.Revit.DB.Transform)
    'Represents the Autodesk Revit Application, 
    'providing access to documents, options and other application wide data and settings.
    Private mApplication As Autodesk.Revit.ApplicationServices.Application
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
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) As Autodesk.Revit.UI.Result Implements IExternalCommand.Execute

        mApplication = commandData.Application.Application
        mOptions = commandData.Application.Application.Create.NewGeometryOptions
        mOptions.DetailLevel = Autodesk.Revit.DB.ViewDetailLevel.Fine
        mViewer = New RevitViewer.Wireframe

      Dim selSet As Autodesk.Revit.DB.ElementSet = New ElementSet()
      Dim elementId As Autodesk.Revit.DB.ElementId
      For Each elementId In commandData.Application.ActiveUIDocument.Selection.GetElementIds()
         selSet.Insert(commandData.Application.ActiveUIDocument.Document.GetElement(elementId))
      Next
        Dim elem As Autodesk.Revit.DB.Element

        For Each elem In selSet
            DrawElement(elem) ' this one handles Group. 
        Next

        If selSet.Size > 0 Then
            mViewer.ShowModal()
        Else
            Autodesk.Revit.UI.TaskDialog.Show("Revit", "Please select some elements first.")
        End If

        mApplication = Nothing
        mViewer = Nothing

        Return Autodesk.Revit.UI.Result.Succeeded

    End Function


    Private Sub PushTransformation(ByVal transform As Autodesk.Revit.DB.Transform)
        If (mTransformations Is Nothing) Then
            mTransformations = New System.Collections.Generic.List(Of Autodesk.Revit.DB.Transform)
        End If
        mTransformations.Add(transform)
    End Sub

    Private Sub PopTransformation()

        If (mTransformations Is Nothing) Then
            Exit Sub
        End If

        If (mTransformations.Count = 1) Then
            mTransformations = Nothing
            Exit Sub
        End If

        Dim newTransformations As System.Collections.Generic.List(Of Autodesk.Revit.DB.Transform)
        newTransformations = New System.Collections.Generic.List(Of Autodesk.Revit.DB.Transform)
        Dim i As Integer
        For i = 0 To mTransformations.Count - 2

            newTransformations.Add(mTransformations.Item(i))

        Next

        mTransformations = newTransformations

    End Sub


    ' Note: Some element does not expose geometry, for example, curtain wall and dimension.
    ' In case of a curtain wall, try selecting a whole wall by a window/box instead of a single pick. 
    ' It will then select internal components and be able to display its geometry.
    ' 
    Private Sub DrawElement(ByVal elem As Autodesk.Revit.DB.Element)

        ' if it is a Group. we will need to look at its components. 
        If TypeOf elem Is Autodesk.Revit.DB.Group Then

            Dim group As Autodesk.Revit.DB.Group = elem
            Dim members As Autodesk.Revit.DB.ElementArray = group.GetMemberIds()

            Dim elm As Autodesk.Revit.DB.ElementId
            For Each elm In members
                DrawElement(group.Document.GetElement(elm))
            Next

        Else

            ' not a group. look at the geom data. 
            Dim geom As Autodesk.Revit.DB.GeometryElement = elem.Geometry(mOptions)
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
    Private Sub DrawElement(ByVal elementGeom As Autodesk.Revit.DB.GeometryElement)

        If elementGeom Is Nothing Then
            Exit Sub
        End If

        Dim geomObject As Autodesk.Revit.DB.GeometryObject

        Dim Objects As IEnumerator(Of GeometryObject) = elementGeom.GetEnumerator()

        'For Each geomObject In elementGeom.Objects
        While Objects.MoveNext

            geomObject = Objects.Current

            If (TypeOf geomObject Is Autodesk.Revit.DB.Curve) Then
                DrawCurve(geomObject)
            ElseIf (TypeOf geomObject Is Autodesk.Revit.DB.GeometryInstance) Then
                DrawInstance(geomObject)
            ElseIf (TypeOf geomObject Is Autodesk.Revit.DB.Mesh) Then
                DrawMesh(geomObject)
            ElseIf (TypeOf geomObject Is Autodesk.Revit.DB.Solid) Then
                DrawSolid(geomObject)
            ElseIf (TypeOf geomObject Is Autodesk.Revit.DB.PolyLine) Then
                DrawPoly(geomObject)
            End If

        End While

    End Sub

    ''' <summary>
    ''' add the primitive line to the Wireframe object.
    ''' </summary>
    ''' <param name="startPoint"></param>
    ''' <param name="endPoint"></param>
    ''' <remarks></remarks>
    Private Sub ViewerDrawLine(ByRef startPoint As Autodesk.Revit.DB.XYZ, ByRef endPoint As Autodesk.Revit.DB.XYZ)

        If (mViewer Is Nothing) Then
            Exit Sub
        End If

        Dim transformedStart As Autodesk.Revit.DB.XYZ
        transformedStart = startPoint
        Dim transformedEnd As Autodesk.Revit.DB.XYZ
        transformedEnd = endPoint

        If Not (mTransformations Is Nothing) Then
            Dim count As Long = mTransformations.Count
            Dim index As Long
            For index = count - 1 To 0 Step -1
                transformedStart = TransformPoint(mTransformations(index), transformedStart)
                transformedEnd = TransformPoint(mTransformations(index), transformedEnd)
            Next
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
    Public Function TransformPoint(ByVal transform As Autodesk.Revit.DB.Transform, ByRef point As Autodesk.Revit.DB.XYZ) As Autodesk.Revit.DB.XYZ

        Return transform.OfPoint(point)

        'Dim result As New Autodesk.Revit.DB.XYZ

        'Dim i As Integer
        'For i = 0 To 2

        '    result.Item(i) = transform.Origin.Item(i)
        '    Dim j As Integer
        '    For j = 0 To 2
        '        result.Item(i) = result.Item(i) + (transform.Basis(j).Item(i) * point.Item(j))
        '    Next

        'Next

        'Return result

    End Function

    ''' <summary>
    ''' add the primitive curve to the Wireframe object.
    ''' </summary>
    ''' <param name="geomCurve"></param>
    ''' <remarks></remarks>
    Private Sub DrawCurve(ByVal geomCurve As Autodesk.Revit.DB.Curve)

        DrawPoints(geomCurve.Tessellate)

    End Sub

    ''' <summary>
    ''' add the poly line to the Wireframe object.
    ''' </summary>
    ''' <param name="polyLine"></param>
    ''' <remarks></remarks>
    Private Sub DrawPoly(ByVal polyLine As Autodesk.Revit.DB.PolyLine)

        DrawPoints(polyLine.GetCoordinates())

    End Sub

    ''' <summary>
    ''' add the primitive points to the Wireframe object.
    ''' </summary>
    ''' <param name="points"></param>
    ''' <remarks></remarks>
    Private Sub DrawPoints(ByVal points As List(Of Autodesk.Revit.DB.XYZ))

        Dim previousPoint As Autodesk.Revit.DB.XYZ
        previousPoint = points.Item(0)
        Dim point As Autodesk.Revit.DB.XYZ

        Dim i As Integer
        For i = 0 To points.Count - 1

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
    Private Sub DrawInstance(ByVal geomInstance As Autodesk.Revit.DB.GeometryInstance)

        PushTransformation(geomInstance.Transform)

        Dim geomSymbol As Autodesk.Revit.DB.GeometryElement
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
    Private Sub DrawMesh(ByVal geomMesh As Autodesk.Revit.DB.Mesh)

        Dim i As Integer

        For i = 0 To geomMesh.NumTriangles - 1
            Dim triangle As Autodesk.Revit.DB.MeshTriangle
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
    Private Sub DrawSolid(ByVal geomSolid As Autodesk.Revit.DB.Solid)

        Dim face As Autodesk.Revit.DB.Face
        For Each face In geomSolid.Faces
            DrawFace(face)
        Next

        Dim Edge As Autodesk.Revit.DB.Edge
        For Each Edge In geomSolid.Edges
            DrawEdge(Edge)
        Next

    End Sub

    ''' <summary>
    ''' add the primitive edge to the Wireframe object.
    ''' </summary>
    ''' <param name="geomEdge"></param>
    ''' <remarks></remarks>
    Private Sub DrawEdge(ByVal geomEdge As Autodesk.Revit.DB.Edge)

        DrawPoints(geomEdge.Tessellate)

    End Sub

    ''' <summary>
    ''' add the primitive face to the Wireframe object.
    ''' </summary>
    ''' <param name="geomFace"></param>
    ''' <remarks></remarks>
    Private Sub DrawFace(ByVal geomFace As Autodesk.Revit.DB.Face)

        DrawMesh(geomFace.Triangulate)

    End Sub

End Class
