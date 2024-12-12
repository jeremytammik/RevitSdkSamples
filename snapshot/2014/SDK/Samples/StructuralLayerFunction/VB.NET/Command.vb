'
' (C) Copyright 2003-2013 by Autodesk, Inc.
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


Imports System
Imports System.Collections
Imports System.Windows.Forms

Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB.Structure


''' <summary>
''' With the selected floor, display the function of each of its structural layers
''' in order from outside to inside in a dialog box
''' </summary>
''' <remarks></remarks>
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class Command
    Implements IExternalCommand

    'Private data members
    Private m_slab As Autodesk.Revit.DB.Floor 'Store the selected floor
    Private m_functions As ArrayList                'Store the function of each floor

    ''' <summary>
    ''' With the selected floor, export the function of each of its structural layers
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Functions() As ArrayList
        Get
            Functions = m_functions
        End Get
    End Property

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
    Public Function Execute(ByVal commandData As Autodesk.Revit.UI.ExternalCommandData, ByRef message As String, ByVal elements _
     As Autodesk.Revit.DB.ElementSet) As Autodesk.Revit.UI.Result Implements _
     Autodesk.Revit.UI.IExternalCommand.Execute
        ' Get the selected element
        Dim revit As Autodesk.Revit.UI.UIApplication = commandData.Application
        Dim project As Autodesk.Revit.UI.UIDocument = revit.ActiveUIDocument
        Dim choices As Autodesk.Revit.UI.Selection.Selection = project.Selection
        Dim collection As Autodesk.Revit.DB.ElementSet = choices.Elements

        ' Only allow to select one floor, or else report the failure
        If Not (1 = collection.Size) Then
            message = "Please select a floor."
            Return Autodesk.Revit.UI.Result.Failed
        End If

        For Each e As Autodesk.Revit.DB.Element In collection
            ' Judge if the element is floor
            If TypeOf e Is Autodesk.Revit.DB.Floor Then
                m_slab = e
            End If
            If Nothing Is m_slab Then
                message = "Please select a floor."
                Return Autodesk.Revit.UI.Result.Failed
            End If
        Next

        ' Get the function of each of its structural layers
      For Each e As Autodesk.Revit.DB.CompoundStructureLayer In m_slab.FloorType.GetCompoundStructure.GetLayers
         ' With the selected floor, judge if the function of each of its structural layers
         ' is exist, if it's not exist, there should be zero.
         If 0 = e.Function Then
            m_functions.Add("No function")
         Else
            m_functions.Add(e.Function.ToString())
         End If
      Next

        ' Display them in a form
        Dim displayForm As New StructuralLayerFunctionForm(Me)
        displayForm.ShowDialog()

        Return Autodesk.Revit.UI.Result.Succeeded
    End Function

    ''' <summary>
    ''' Default constructor of StructuralLayerFunction
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        ' Construct the data members for the property
        m_functions = New ArrayList
    End Sub

End Class
