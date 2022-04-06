' 
' (C) Copyright 2003-2019 by Autodesk, Inc.
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
Imports System.Linq

Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI

''' <summary>
''' Implements the Revit add-in interface IExternalCommand
''' </summary>
''' <remarks></remarks>
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class AnalyticalViewer
   Implements Autodesk.Revit.UI.IExternalCommand

   Private mViewer As RevitViewer.VB.NET.Wireframe
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
      Dim elementId As Autodesk.Revit.DB.ElementId

      mApplication = commandData.Application.Application

      If (commandData.Application.ActiveUIDocument.Selection.GetElementIds().Count > 0) Then
         For Each elementId In commandData.Application.ActiveUIDocument.Selection.GetElementIds()
            element = commandData.Application.ActiveUIDocument.Document.GetElement(elementId)
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
         TaskDialog.Show("Revit", "No analytical model to represent.")
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

      Dim analyticalModel As Autodesk.Revit.DB.Structure.AnalyticalElement = Nothing
      Dim document As Document = element.Document
        Dim assocManager As Autodesk.Revit.DB.Structure.AnalyticalToPhysicalAssociationManager = Autodesk.Revit.DB.Structure.AnalyticalToPhysicalAssociationManager.GetAnalyticalToPhysicalAssociationManager(document)

        If (assocManager Is Nothing) Then
            Exit Sub
        End If

        Dim associatedElementId As ElementId = assocManager.GetAssociatedElementId(element.Id)
        If (associatedElementId Is Nothing) Then
            Exit Sub
        End If

        analyticalModel = document.GetElement(associatedElementId)

        If (analyticalModel Is Nothing) Then
         Exit Sub
      End If

      If (mViewer Is Nothing) Then
         mViewer = New RevitViewer.VB.NET.Wireframe
      End If

      Dim curves As IList(Of Curve) = New List(Of Curve)
      If TypeOf analyticalModel Is Autodesk.Revit.DB.Structure.AnalyticalMember Then
         Dim analyticalMember As Autodesk.Revit.DB.Structure.AnalyticalMember = analyticalModel

         curves.Add(analyticalMember.GetCurve)
      End If

      If TypeOf analyticalModel Is Autodesk.Revit.DB.Structure.AnalyticalPanel Then
         Dim analyticalPanel As Autodesk.Revit.DB.Structure.AnalyticalPanel = analyticalModel

         curves = analyticalPanel.GetOuterContour.ToList
      End If

      DrawAnalyticalModelCurves(curves)


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
End Class

