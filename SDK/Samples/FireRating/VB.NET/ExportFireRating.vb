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
Public Class ExportFireRating
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

        Dim guid As Guid = FindGUID(commandData.Application.Application)

        If (guid = guid.Empty) Then
            Return Autodesk.Revit.UI.Result.Failed
        End If

        'get a set of element which is not elementType
        Dim filter As Autodesk.Revit.DB.ElementIsElementTypeFilter
        filter = New Autodesk.Revit.DB.ElementIsElementTypeFilter(True)
        Dim collector As Autodesk.Revit.DB.FilteredElementCollector
        collector = New Autodesk.Revit.DB.FilteredElementCollector(commandData.Application.ActiveUIDocument.Document)
        collector.WherePasses(filter)
        Dim iter As IEnumerator
        iter = collector.GetElementIterator()

        Dim doors As Autodesk.Revit.DB.ElementSet = commandData.Application.Application.Create.NewElementSet

        Do While (iter.MoveNext())

            Dim element As Autodesk.Revit.DB.Element = iter.Current
            If Not (TypeOf element Is Autodesk.Revit.DB.ElementType) Then
                If Not (element.Category Is Nothing) Then
                    If (element.Category.Name = mCategoryName) Then
                        doors.Insert(element)
                    End If
                End If
            End If

        Loop

        Dim excel As MsExcel.Application
        excel = Nothing
        SendToExcel(excel, commandData.Application.Application, doors, guid)

        Return Autodesk.Revit.UI.Result.Succeeded

    End Function

    Function FindGUID(ByVal application As Autodesk.Revit.ApplicationServices.Application) As Guid
        Try
            Dim sharedParamFile As Autodesk.Revit.DB.DefinitionFile = application.OpenSharedParameterFile
            Dim group As Autodesk.Revit.DB.DefinitionGroup = sharedParamFile.Groups.Item(mGroupName)


            Dim definition As Autodesk.Revit.DB.Definition = group.Definitions.Item(mParameterName)
            Dim externalDefinition As Autodesk.Revit.DB.ExternalDefinition = definition
            Return externalDefinition.GUID
        Catch ex As Exception
            Autodesk.Revit.UI.TaskDialog.Show("Revit", "ApplyParameter first")
            Return Nothing

        End Try

    End Function
    ' LaunchExcel will try to launch Excel 2003 or later. It will then create an empty workbook
    ' and remove all of the work sheets except one which will form the bassis for the first
    ' category that is sent to Excel by this sample
    Function LaunchExcel() As MsExcel.Application

        Dim excel As MsExcel.Application = New MsExcel.ApplicationClass()

        If (excel Is Nothing) Then
            Return Nothing
        End If

        ' make excel visible so that operations made are visible to the user
        excel.Visible = True

        ' Add a new workbook which will default to having 3 work sheets
        Dim workbook As MsExcel.Workbook = excel.Workbooks.Add()

        Dim worksheet As MsExcel.Worksheet

        ' remove all the sheets except one
        Do While workbook.Sheets.Count > 1

            worksheet = workbook.Sheets.Item(1)
            worksheet.Delete()

        Loop

        Return excel

    End Function

    Function SendToExcel(ByRef excel As MsExcel.Application, ByVal application As Autodesk.Revit.ApplicationServices.Application, ByVal elementSet As Autodesk.Revit.DB.ElementSet, ByVal guid As Guid) As Boolean

        SendToExcel = False

        'Excel can experience errors during this process. 
        On Error GoTo ExitSendToExcel

        Dim worksheet As MsExcel.Worksheet

        ' If excel is not running, then launch it which will result in one sheet remaining. If excel
        ' is already running then we need to create a new sheet.
        If (excel Is Nothing) Then
            excel = LaunchExcel()

            If (excel Is Nothing) Then
                Exit Function
            End If

            worksheet = excel.ActiveSheet

        Else

            worksheet = excel.Worksheets.Add()

        End If

        worksheet.Name = mParameterName

        worksheet.Cells(1, 1).Value = "ID"
        worksheet.Cells(1, 2).Value = "Level"
        worksheet.Cells(1, 3).Value = "Tag"
        worksheet.Cells(1, 4).Value = mParameterName

        Dim element As Autodesk.Revit.DB.Element
        Dim row As Integer = 2
        For Each element In elementSet

            'ID
            worksheet.Cells(row, 1).Value = element.Id.Value

            'Level
            Dim level As Autodesk.Revit.DB.Level
            level = element.Document.GetElement(element.LevelId)
            If Not (level Is Nothing) Then
                worksheet.Cells(row, 2).Value = level.Name
            End If

            'Tag
            Dim tagParameter As Autodesk.Revit.DB.Parameter = element.Parameter(Autodesk.Revit.DB.BuiltInParameter.ALL_MODEL_MARK)
            If Not (tagParameter Is Nothing) Then
                worksheet.Cells(row, 3).Value = tagParameter.AsString
            End If

            'Fire Rating
            Dim parameter As Autodesk.Revit.DB.Parameter = element.Parameter(guid)
            If Not (parameter Is Nothing) Then
                worksheet.Cells(row, 4).Value = parameter.AsInteger
            End If

            row = row + 1

        Next

        SendToExcel = True

        Dim filename As String = mPath & "\" & mExcelFilename
        worksheet.SaveAs(filename)

ExitSendToExcel:

    End Function


End Class
