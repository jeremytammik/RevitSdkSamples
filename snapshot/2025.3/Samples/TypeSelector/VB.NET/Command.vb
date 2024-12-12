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
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.?AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'


Option Explicit On
Imports System
Imports Autodesk.Revit.DB

''' <summary>
''' This sample shows how the type of a selected element, such as a wall can be changed using the API.
''' </summary>
''' <remarks></remarks>
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class Command
   Implements Autodesk.Revit.UI.IExternalCommand

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
   Public Function Execute(ByVal commandData As Autodesk.Revit.UI.ExternalCommandData, _
   ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) As Autodesk.Revit.UI.Result _
   Implements Autodesk.Revit.UI.IExternalCommand.Execute

      ' result of command
      Dim result As Autodesk.Revit.UI.Result
      result = Autodesk.Revit.UI.Result.Failed

      ' start transaction
      Dim tran As Autodesk.Revit.DB.Transaction
      tran = New Autodesk.Revit.DB.Transaction(commandData.Application.ActiveUIDocument.Document, "TypeSelector")
      tran.Start()

      ' get all selection element
      Dim selectionObject As Autodesk.Revit.UI.Selection.Selection
      Dim selection As Autodesk.Revit.DB.ElementSet
      selectionObject = commandData.Application.ActiveUIDocument.Selection
      selection = New ElementSet()
      Dim elementId As Autodesk.Revit.DB.ElementId
      For Each elementId In selectionObject.GetElementIds()
         selection.Insert(commandData.Application.ActiveUIDocument.Document.GetElement(elementId))
      Next

      Debug.Write(selection.IsEmpty)

      ' if one component is not selected then throw a wobbly
      If (selection.Size <> 1) Then
         message = "A single component or wall must be selected"
         Return Autodesk.Revit.UI.Result.Failed
      Else
         Dim element As Autodesk.Revit.DB.Element = Nothing
         Dim iter As IEnumerator
         iter = selection.ForwardIterator
         Do While iter.MoveNext
            element = iter.Current
         Loop

         If Not (TypeOf element Is Autodesk.Revit.DB.FamilyInstance Or TypeOf element Is Autodesk.Revit.DB.Wall) Then
            message = "A component or wall must be selected"
            Return Autodesk.Revit.UI.Result.Failed
         Else
            Dim dialog As New TypeSelectorWindow
            dialog.Initialise(commandData.Application.ActiveUIDocument.Document, element)
            dialog.ShowDialog()
            result = dialog.m_result
            message = dialog.m_resultMessage
         End If

      End If

      tran.Commit()

      Return result
   End Function

End Class
