' 
'  (C) Copyright 2003-2019 by Autodesk, Inc.
' 
'  Permission to use, copy, modify, and distribute this software in
'  object code form for any purpose and without fee is hereby gran
'  provided that the above copyright notice appears in all copies an
'  that both that copyright notice and the limited warranty and
'  restricted rights notice below appear in all supporting
'  documentation.

'  AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH
'  AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WAR
'  MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE
'  DOES NOT WARRANT THAT THE OPERATION OF THE PRO
'  UNINTERRUPTED OR ERROR FREE.
' 
'  Use, duplication, or disclosure by the U.S. Government is subject 
'  restrictions set forth in FAR 52.227-19 (Commercial Computer
'  Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
'  (Rights in Technical Data and Computer Software), as applicable.
' 
Imports System
Imports System.Windows.Forms
Imports System.Collections

Imports Autodesk.Revit.DB

''' <summary>
''' Delete the elements that were selected
''' </summary>

<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)>
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)>
Public Class Command
    Implements Autodesk.Revit.UI.IExternalCommand

    ''' <summary>
    '''  Implement this method as an external command for Revit.
    '''  </summary>
    '''  <param name="commandData">An object that is passed to the external application 
    '''  which contains data related to the command, 
    '''  such as the application object and active view.</param>
    '''  <param name="message">A message that can be set by the external application 
    '''  which will be displayed if a failure or cancellation is returned by 
    '''  the external command.</param>
    '''  <param name="elements">A set of elements to which the external application 
    '''  can add elements that are to be highlighted in case of failure or cancellation.</param>
    '''  <returns>Return the status of the external command. 
    '''  A result of Succeeded means that the API external method functioned as expected. 
    '''  Cancelled can be used to signify that the user cancelled the external operation 
    '''  at some point. Failure should be returned if the application is unable to proceed with 
    '''  the operation.</returns>
    Public Function Execute(ByVal commandData As Autodesk.Revit.UI.ExternalCommandData,
    ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) _
    As Autodesk.Revit.UI.Result _
    Implements Autodesk.Revit.UI.IExternalCommand.Execute

        Dim revit As Autodesk.Revit.UI.UIApplication = commandData.Application
        Dim trans As Transaction = New Transaction(revit.ActiveUIDocument.Document, "Revit.SDK.Samples.DeleteObject")
        trans.Start()
        Dim collection As Autodesk.Revit.DB.ElementSet = New Autodesk.Revit.DB.ElementSet()
        Dim elementId As Autodesk.Revit.DB.ElementId
        For Each elementId In revit.ActiveUIDocument.Selection.GetElementIds()
            collection.Insert(revit.ActiveUIDocument.Document.GetElement(elementId))
        Next
        'check user selection
        If collection.Size < 1 Then
            message = "Please select object before delete."
            trans.RollBack()
            Return Autodesk.Revit.UI.Result.Cancelled
        End If

        Dim isError As Boolean = True
        Try
            'delete selection
            Dim e As IEnumerator = collection.GetEnumerator()
            Dim MoreValue As Boolean = e.MoveNext()

            While MoreValue
                If TypeOf e.Current Is Autodesk.Revit.DB.Element Then
                    Dim component As Autodesk.Revit.DB.Element = e.Current
                    revit.ActiveUIDocument.Document.Delete(component.Id)
                    MoreValue = e.MoveNext()
                End If
            End While

            isError = False

        Catch
            'if revit threw an exception, try to catch it
            For Each c As Autodesk.Revit.DB.Element In collection
                elements.Insert(c)
            Next

            message = "object(s) can't be deleted."
            trans.RollBack()
            Return Autodesk.Revit.UI.Result.Failed
        Finally
            ' if revit threw an exception, display error and return failed
            If isError Then
                Autodesk.Revit.UI.TaskDialog.Show("Revit", "Delete failed.")
            End If

        End Try
        trans.Commit()
        Return Autodesk.Revit.UI.Result.Succeeded
    End Function
End Class
