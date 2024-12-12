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
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'
Imports MsExcel = Microsoft.Office.Interop.Excel
Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports System.Windows.Forms

<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class ImportFireRating

   ' All Autodesk Revit external commands must support this interface
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
   Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) As Autodesk.Revit.UI.Result Implements Autodesk.Revit.UI.IExternalCommand.Execute

      'Dim excel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.ApplicationClass()
      Dim excel As MsExcel.Application = New MsExcel.ApplicationClass()

      If (excel Is Nothing) Then
         message = "Failed to launch excel"
         Return Autodesk.Revit.UI.Result.Failed
      End If

      excel.Visible = True

      Dim filename As String = mPath & "\" & mExcelFilename
      'Dim workbook As Microsoft.Office.Interop.Excel.Workbook = excel.Workbooks.Open(filename)
      Dim workbook As MsExcel.Workbook = excel.Workbooks.Open(filename)

      'Dim worksheet As Microsoft.Office.Interop.Excel.Worksheet
      Dim worksheet As MsExcel.Worksheet
      worksheet = workbook.ActiveSheet

      Dim row As Integer = 2
      Dim value As String
      Do
         value = worksheet.Cells(row, 1).Value
         If Not (value Is Nothing) Then

            Dim fireRatingValue As String
            fireRatingValue = worksheet.Cells(row, 4).Value
            If Not (fireRatingValue Is Nothing) Then
               Dim idInteger As Integer
               Dim fireRateDouble As Double
               Try
                  idInteger = value
                  fireRateDouble = fireRatingValue
               Catch ex As Exception
                        TaskDialog.Show("Revit", ex.ToString)
               End Try

               Dim elementId As Autodesk.Revit.DB.ElementId = New Autodesk.Revit.DB.ElementId(idInteger)

                    Dim element As Autodesk.Revit.DB.Element = commandData.Application.ActiveUIDocument.Document.GetElement(elementId)
               If Not (element Is Nothing) Then
                  Dim parameters As Autodesk.Revit.DB.ParameterSet = element.Parameters
                  Dim parameter As Autodesk.Revit.DB.Parameter
                  For Each parameter In parameters
                     If (parameter.Definition.Name = mParameterName) Then
                        Dim tran As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(commandData.Application.ActiveUIDocument.Document, "ImportParameter")
                        tran.Start()
                        parameter.Set(fireRateDouble)
                        tran.Commit()
                        Exit For
                     End If
                  Next
               End If

            End If

            row = row + 1

         End If

      Loop Until (value Is Nothing)


      Return Autodesk.Revit.UI.Result.Succeeded

   End Function

End Class
