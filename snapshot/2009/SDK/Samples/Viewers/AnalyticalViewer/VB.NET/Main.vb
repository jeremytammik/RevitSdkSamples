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

Imports Autodesk
Imports Autodesk.Revit

''' <summary>
''' Implements the Revit add-in interface IExternalCommand
''' </summary>
''' <remarks></remarks>
Public Class AnalyticalViewer
    Implements Autodesk.Revit.IExternalCommand

    Private mViewer As RevitViewer.Wireframe
    Private mApplication As Autodesk.Revit.Application

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

        Dim element As Autodesk.Revit.Element

        mApplication = commandData.Application

        If (commandData.Application.ActiveDocument.Selection.Elements.Size > 0) Then
            For Each element In commandData.Application.ActiveDocument.Selection.Elements
                DrawAnalyticalModel(element)
            Next
        Else

            Dim iter As IEnumerator
            iter = commandData.Application.ActiveDocument.Elements
            Do While (iter.MoveNext())
                element = iter.Current
                DrawAnalyticalModel(element)
            Loop
        End If

        If Not (mViewer Is Nothing) Then

            mViewer.Fit()
            mViewer.Draw()
            mViewer.ShowModal()
        Else
            System.Windows.Forms.MessageBox.Show("No analytical model to represent.")
        End If

        mViewer = Nothing
        mApplication = Nothing

        Return Autodesk.Revit.IExternalCommand.Result.Succeeded
    End Function

    ''' <summary>
    ''' distribute analytical model to its corresponding method to be draw.
    ''' </summary>
    ''' <param name="element"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModel(ByVal element As Autodesk.Revit.Element)

        Dim analyticalModel As Autodesk.Revit.Structural.AnalyticalModel = Nothing

        If TypeOf element Is Autodesk.Revit.Elements.FamilyInstance Then

            Dim familyInstance As Autodesk.Revit.Elements.FamilyInstance
            familyInstance = element
            analyticalModel = familyInstance.AnalyticalModel

        ElseIf TypeOf element Is Autodesk.Revit.Elements.Wall Then

            Dim wall As Autodesk.Revit.Elements.Wall
            wall = element
            analyticalModel = wall.AnalyticalModel
        ElseIf TypeOf element Is Autodesk.Revit.Elements.ContFooting Then

            Dim contFooting As Autodesk.Revit.Elements.ContFooting
            contFooting = element
            analyticalModel = contFooting.AnalyticalModel

        ElseIf TypeOf element Is Autodesk.Revit.Elements.Floor Then

            Dim floor As Autodesk.Revit.Elements.Floor
            floor = element
            analyticalModel = floor.AnalyticalModel

        End If

        If (analyticalModel Is Nothing) Then
            Exit Sub
        End If

        If (mViewer Is Nothing) Then
            mViewer = New RevitViewer.Wireframe
        End If

        If TypeOf analyticalModel Is Autodesk.Revit.Structural.AnalyticalModel3D Then

            DrawAnalyticalModel3D(analyticalModel)

        ElseIf TypeOf analyticalModel Is Autodesk.Revit.Structural.AnalyticalModelFloor Then

            DrawAnalyticalModelFloor(analyticalModel)

        ElseIf TypeOf analyticalModel Is Autodesk.Revit.Structural.AnalyticalModelFrame Then

            DrawAnalyticalModelFrame(analyticalModel)

        ElseIf TypeOf analyticalModel Is Autodesk.Revit.Structural.AnalyticalModelLocation Then

            DrawAnalyticalModelLocation(analyticalModel)

        ElseIf TypeOf analyticalModel Is Autodesk.Revit.Structural.AnalyticalModelWall Then

            DrawAnalyticalModelWall(analyticalModel)

        End If

    End Sub

    ''' <summary>
    ''' Draw the primitive curve.
    ''' </summary>
    ''' <param name="curve"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModelCurve(ByVal curve As Autodesk.Revit.Geometry.Curve)

        If (curve Is Nothing) Then
            Exit Sub
        End If

        Dim points As Autodesk.Revit.Geometry.XYZArray
        points = curve.Tessellate

        If (points Is Nothing) Then
            Exit Sub
        End If

        If points.Size = 0 Then
            Exit Sub
        End If

        Dim previousPoint As Autodesk.Revit.Geometry.XYZ
        previousPoint = New Autodesk.Revit.Geometry.XYZ
        Dim i As Integer
        For i = 0 To points.Size - 1

            Dim point As Autodesk.Revit.Geometry.XYZ
            point = points.Item(i)
            If i > 0 Then
                mViewer.Add(previousPoint.X, previousPoint.Y, previousPoint.Z, point.X, point.Y, point.Z)
            End If

            previousPoint = point
        Next

    End Sub

    ''' <summary>
    ''' Draw the primitive curve array.
    ''' </summary>
    ''' <param name="curves"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModelCurves(ByVal curves As Autodesk.Revit.Geometry.CurveArray)

        Dim curve As Autodesk.Revit.Geometry.Curve

        For Each curve In curves

            DrawAnalyticalModelCurve(curve)

        Next

    End Sub

    ''' <summary>
    ''' draw analytical 3D model
    ''' </summary>
    ''' <param name="analyticalModel"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModel3D(ByVal analyticalModel As Autodesk.Revit.Structural.AnalyticalModel3D)

        DrawAnalyticalModelCurves(analyticalModel.Curves)

    End Sub

    ''' <summary>
    ''' draw analytical floor model
    ''' </summary>
    ''' <param name="analyticalModel"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModelFloor(ByVal analyticalModel As Autodesk.Revit.Structural.AnalyticalModelFloor)

        DrawAnalyticalModelCurves(analyticalModel.Curves)

    End Sub

    ''' <summary>
    ''' draw analytical frame model
    ''' </summary>
    ''' <param name="analyticalModel"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModelFrame(ByVal analyticalModel As Autodesk.Revit.Structural.AnalyticalModelFrame)

        DrawAnalyticalModelCurves(analyticalModel.Curves)

    End Sub

    ''' <summary>
    ''' draw analytical location model
    ''' </summary>
    ''' <param name="analyticalModel"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModelLocation(ByVal analyticalModel As Autodesk.Revit.Structural.AnalyticalModelLocation)

        'draw a square

        Dim location As Autodesk.Revit.Geometry.XYZ
        location = analyticalModel.Point

        Dim curves As Autodesk.Revit.Geometry.CurveArray
        curves = mApplication.Create.NewCurveArray

        Dim point1 As Autodesk.Revit.Geometry.XYZ
        point1 = New Autodesk.Revit.Geometry.XYZ
        Dim point2 As Autodesk.Revit.Geometry.XYZ
        point2 = New Autodesk.Revit.Geometry.XYZ
        Dim point3 As Autodesk.Revit.Geometry.XYZ
        point3 = New Autodesk.Revit.Geometry.XYZ
        Dim point4 As Autodesk.Revit.Geometry.XYZ
        point4 = New Autodesk.Revit.Geometry.XYZ

        point1.X = location.X - 2
        point1.Y = location.Y - 2
        point1.Z = location.Z

        point2.X = location.X - 2
        point2.Y = location.Y + 2
        point2.Z = location.Z

        point3.X = location.X + 2
        point3.Y = location.Y + 2
        point3.Z = location.Z

        point4.X = location.X + 2
        point4.Y = location.Y - 2
        point4.Z = location.Z

        Dim line1 As Autodesk.Revit.Geometry.Line
        Dim line2 As Autodesk.Revit.Geometry.Line
        Dim line3 As Autodesk.Revit.Geometry.Line
        Dim line4 As Autodesk.Revit.Geometry.Line

        line1 = mApplication.Create.NewLineBound(point1, point2)
        line2 = mApplication.Create.NewLineBound(point2, point3)
        line3 = mApplication.Create.NewLineBound(point3, point4)
        line4 = mApplication.Create.NewLineBound(point4, point1)

        curves.Append(line1)
        curves.Append(line2)
        curves.Append(line3)
        curves.Append(line4)

        DrawAnalyticalModelCurves(curves)

    End Sub

    ''' <summary>
    ''' draw analytical wall model
    ''' </summary>
    ''' <param name="analyticalModel"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModelWall(ByVal analyticalModel As Autodesk.Revit.Structural.AnalyticalModelWall)

        DrawAnalyticalModelCurves(analyticalModel.Curves)

    End Sub

End Class

