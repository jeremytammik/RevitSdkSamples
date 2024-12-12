' 
' (C) Copyright 2003-2010 by Autodesk, Inc.
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
Imports Autodesk.Revit.UI

''' <summary>
''' Implements the Revit add-in interface IExternalCommand
''' </summary>
''' <remarks></remarks>
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Automatic)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class AnalyticalViewer
    Implements Autodesk.Revit.UI.IExternalCommand

    Private mViewer As RevitViewer.Wireframe
    Private mApplication As Autodesk.Revit.ApplicationServices.Application

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

        Dim element As Autodesk.Revit.DB.Element

        mApplication = commandData.Application.Application

        If (commandData.Application.ActiveUIDocument.Selection.Elements.Size > 0) Then
            For Each element In commandData.Application.ActiveUIDocument.Selection.Elements
                DrawAnalyticalModel(element)
            Next
        Else
            'ElementIsElementTypeFilter filter the elementType which has not AnalyticalModel
            Dim filter As Autodesk.Revit.DB.ElementIsElementTypeFilter
            filter = New Autodesk.Revit.DB.ElementIsElementTypeFilter(True)
            Dim collector As Autodesk.Revit.DB.FilteredElementCollector
            collector = New Autodesk.Revit.DB.FilteredElementCollector(commandData.Application.ActiveUIDocument.Document)
            collector.WherePasses(filter)
            Dim iter As IEnumerator
            iter = collector.GetElementIterator

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

        Return Autodesk.Revit.UI.Result.Succeeded
    End Function

    ''' <summary>
    ''' distribute analytical model to its corresponding method to be draw.
    ''' </summary>
    ''' <param name="element"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModel(ByVal element As Autodesk.Revit.DB.Element)

        Dim analyticalModel As Autodesk.Revit.DB.Structure.AnalyticalModel = Nothing

        analyticalModel = element.GetAnalyticalModel()

        If (analyticalModel Is Nothing) Then
            Exit Sub
        End If

        If (mViewer Is Nothing) Then
            mViewer = New RevitViewer.Wireframe
        End If

        If TypeOf element Is Autodesk.Revit.DB.FamilyInstance Then
            Dim familyInstance As Autodesk.Revit.DB.FamilyInstance = element

            If familyInstance.StructuralType.Equals(Autodesk.Revit.DB.Structure.StructuralType.Footing) Then
                DrawAnalyticalModelLocation(analyticalModel)
                Return
            End If

        End If

        DrawAnalyticalModelCurves(analyticalModel.GetCurves([Structure].AnalyticalCurveType.ActiveCurves))


    End Sub

    ''' <summary>
    ''' Draw the primitive curve.
    ''' </summary>
    ''' <param name="curve"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModelCurve(ByVal curve As Autodesk.Revit.DB.Curve)

        If (curve Is Nothing) Then
            Exit Sub
        End If

        Dim points As New List(Of Autodesk.Revit.DB.XYZ)
        points = curve.Tessellate

        If (points Is Nothing) Then
            Exit Sub
        End If

        If points.Count = 0 Then
            Exit Sub
        End If

        Dim previousPoint As Autodesk.Revit.DB.XYZ
        previousPoint = New Autodesk.Revit.DB.XYZ
        Dim i As Integer
        For i = 0 To points.Count - 1

            Dim point As Autodesk.Revit.DB.XYZ
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
    Private Sub DrawAnalyticalModelCurves(ByVal curves As IList(Of Curve))

        Dim curve As Autodesk.Revit.DB.Curve

        For Each curve In curves

            DrawAnalyticalModelCurve(curve)

        Next

    End Sub


    ''' <summary>
    ''' draw analytical location model
    ''' </summary>
    ''' <param name="analyticalModel"></param>
    ''' <remarks></remarks>
    Private Sub DrawAnalyticalModelLocation(ByVal analyticalModel As Autodesk.Revit.DB.Structure.AnalyticalModel)

        'draw a square

        Dim location As Autodesk.Revit.DB.XYZ
        location = analyticalModel.GetPoint()

        Dim curves As IList(Of Curve) = New List(Of Curve)()

        Dim point1 As Autodesk.Revit.DB.XYZ
        point1 = New Autodesk.Revit.DB.XYZ
        Dim point2 As Autodesk.Revit.DB.XYZ
        point2 = New Autodesk.Revit.DB.XYZ
        Dim point3 As Autodesk.Revit.DB.XYZ
        point3 = New Autodesk.Revit.DB.XYZ
        Dim point4 As Autodesk.Revit.DB.XYZ
        point4 = New Autodesk.Revit.DB.XYZ

        point1 = New Autodesk.Revit.DB.XYZ(location.X - 2, location.Y - 2, location.Z)

        point2 = New Autodesk.Revit.DB.XYZ(location.X - 2, location.Y + 2, location.Z)

        point3 = New Autodesk.Revit.DB.XYZ(location.X + 2, location.Y + 2, location.Z)

        point4 = New Autodesk.Revit.DB.XYZ(location.X + 2, location.Y - 2, location.Z)

        Dim line1 As Autodesk.Revit.DB.Line
        Dim line2 As Autodesk.Revit.DB.Line
        Dim line3 As Autodesk.Revit.DB.Line
        Dim line4 As Autodesk.Revit.DB.Line

        line1 = mApplication.Create.NewLineBound(point1, point2)
        line2 = mApplication.Create.NewLineBound(point2, point3)
        line3 = mApplication.Create.NewLineBound(point3, point4)
        line4 = mApplication.Create.NewLineBound(point4, point1)

        curves.Add(line1)
        curves.Add(line2)
        curves.Add(line3)
        curves.Add(line4)

        DrawAnalyticalModelCurves(curves)

    End Sub

End Class

