'
' (C) Copyright 2003-2012 by Autodesk, Inc.
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
Imports Autodesk
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
    Public Function Execute(ByVal commandData As Autodesk.Revit.UI.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) As Autodesk.Revit.UI.Result Implements Autodesk.Revit.UI.IExternalCommand.Execute

        Dim application As Autodesk.Revit.ApplicationServices.Application = commandData.Application.Application

        Dim phase As Autodesk.Revit.DB.Phase

        'Create an instance of the dialog that will display the names of
        'the phases within the project
        Dim dialog As New PickPhase()
        dialog.StartPosition = Windows.Forms.FormStartPosition.CenterParent

        'Iterate through the phases in the project and add them to the
        'check list
        For Each phase In commandData.Application.ActiveUIDocument.Document.Phases

            dialog.PhaseCheckedListBox.Items.Add(phase.Name)

        Next

        'Show the dialog to the user
        If (dialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then

            'Loop through all the elements in the document and find those that
            'match the phases specified by the user in the dialog

            'get a set of element which is not elementType
            Dim filter As Autodesk.Revit.DB.ElementIsElementTypeFilter
            filter = New Autodesk.Revit.DB.ElementIsElementTypeFilter(True)
            Dim collector As Autodesk.Revit.DB.FilteredElementCollector
            collector = New Autodesk.Revit.DB.FilteredElementCollector(commandData.Application.ActiveUIDocument.Document)
            collector.WherePasses(filter)
            Dim iter As IEnumerator
            iter = collector.GetElementIterator

            Do While iter.MoveNext()

                Dim element As Autodesk.Revit.DB.Element = iter.Current

                'Check for created phase
                If (dialog.CreatedRadioButton.Checked = True) Then
                    If Not (element.PhaseCreated Is Nothing) Then
                        If dialog.PhaseCheckedListBox.CheckedItems.Contains(element.PhaseCreated.Name) Then
                            'put the element found into the elements set passed to this method
                            elements.Insert(element)
                        End If
                    End If

                    'check for demolished phase
                ElseIf (dialog.DemolishedRadioButton.Checked = True) Then
                    If Not (element.PhaseDemolished Is Nothing) Then
                        If dialog.PhaseCheckedListBox.CheckedItems.Contains(element.PhaseDemolished.Name) Then
                            'put the element found into the elements set passed to this method
                            elements.Insert(element)
                        End If
                    End If
                End If

            Loop

            'If we have found any elements that we are interested in the set the
            'return message
            If elements.Size > 0 Then
                message = "Elements contained in the selected phases"
            End If
        End If

        'return cancel - this will cause the message and element set to appear
        'to the user
        Return Autodesk.Revit.UI.Result.Cancelled

    End Function

End Class
