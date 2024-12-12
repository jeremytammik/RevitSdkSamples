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
   ''' <remarks></remarks>
   Public Function Execute(ByVal commandData As Autodesk.Revit.UI.ExternalCommandData, ByRef message As String, _
       ByVal elements As Autodesk.Revit.DB.ElementSet) As Autodesk.Revit.UI.Result _
       Implements Autodesk.Revit.UI.IExternalCommand.Execute

      Dim application As Autodesk.Revit.UI.UIApplication = commandData.Application
      Dim document As Autodesk.Revit.UI.UIDocument = application.ActiveUIDocument
      Dim element As Autodesk.Revit.DB.Element
      Dim elementId As Autodesk.Revit.DB.ElementId
      Dim selectWalls As Boolean = False
      Dim tran As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(document.Document, "Test Floor Thickness")
      tran.Start()

      For Each elementId In document.Selection.GetElementIds()
         element = document.Document.GetElement(elementId)
         If TypeOf element Is Autodesk.Revit.DB.Wall Then
            Dim wall As Autodesk.Revit.DB.Wall = element
            selectWalls = True

            Try
               Dim ii As Integer
               Dim cs As Autodesk.Revit.DB.CompoundStructure = wall.WallType.GetCompoundStructure
               For ii = 0 To cs.LayerCount - 1
                  cs.SetLayerWidth(ii, cs.GetLayerWidth(ii) * 10)
               Next
               wall.WallType.SetCompoundStructure(cs)
               document.Document.Regenerate()
            Catch ex As Exception
               Continue For
            End Try

         End If
      Next

      If selectWalls Then
         tran.Commit()
         Return Autodesk.Revit.UI.Result.Succeeded
      Else
         message = "Please select one wall at least."
         tran.RollBack()
         Return Autodesk.Revit.UI.Result.Cancelled
      End If

   End Function
End Class
