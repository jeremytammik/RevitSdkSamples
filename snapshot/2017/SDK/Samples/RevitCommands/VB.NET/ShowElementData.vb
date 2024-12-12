'
' (C) Copyright 2003-2016 by Autodesk, Inc.
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
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'


'
'  ShowElementData - displays a few data of selected elements.   
'
Imports System

Imports Autodesk
Imports Autodesk.Revit.DB

'
' Show Element Data - show a few data of selected elements.
'

<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class RvtCmd_ShowElementData

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
    ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) _
    As Autodesk.Revit.UI.Result Implements Autodesk.Revit.UI.IExternalCommand.Execute

        Dim rvtApp As Autodesk.Revit.UI.UIApplication = commandData.Application

        '  get a list of selected elements. 
      Dim selSet As Autodesk.Revit.DB.ElementSet = New Autodesk.Revit.DB.ElementSet
      Dim elementId As Autodesk.Revit.DB.ElementId
      For Each elementId In rvtApp.ActiveUIDocument.Selection.GetElementIds()
         selSet.Insert(rvtApp.ActiveUIDocument.Document.GetElement(elementId))
      Next

      '  how many did you get? 
      MsgBox("The number of selected elements:" & selSet.Size)

      '  look at each of them.
      Dim elem As Autodesk.Revit.DB.Element
      For Each elem In selSet

         '  show the type. 
         MsgBox(elem.GetType.ToString)

         '  location
         RvtUtils.ListLocation(elem)

         '  geometry
         RvtUtils.ListGeometry(rvtApp.Application, elem)

         '  parameters
         RvtUtils.ListParameters(elem)

      Next

      ' finishing up.
      Return Autodesk.Revit.UI.Result.Succeeded

   End Function

End Class

