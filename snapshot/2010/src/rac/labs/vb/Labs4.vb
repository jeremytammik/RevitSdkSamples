#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2006-2009 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software
' for any purpose and without fee is hereby granted, provided
' that the above copyright notice appears in all copies and
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
#End Region

#Region "Namespaces"
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Runtime
Imports Autodesk.Revit
Imports Autodesk.Revit.Parameters
Imports Microsoft.VisualBasic
Imports MsExcel = Microsoft.Office.Interop.Excel
Imports W = System.Windows.Forms
#End Region

Namespace Labs

#Region "Lab4_1_ParametersForSelectedObjects"
    ''' <summary>
    ''' List all parameters for selected elements.
    ''' </summary>
    Public Class Lab4_1_ParametersForSelectedObjects
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim doc As Document = commandData.Application.ActiveDocument
            ' Loop all elements in the selection
            Dim elem As Element
            For Each elem In doc.Selection.Elements

                ' Loop through and list all UI-visible element parameters
                Dim sMsg As String = "Parameters for the selected " & elem.Category.Name & " (" & elem.Id.Value.ToString & ") are:" & vbCrLf
                Dim param As Parameter
                For Each param In elem.Parameters
                    Dim paramName As String = param.Definition.Name
                    Dim paramType As String = param.StorageType.ToString
                    Dim paramValue As String = LabUtils.GetParameterValue2(param, doc)
                    sMsg += vbCrLf & "  Name=" & paramName & "; Type=" & paramType & "; Value=" & paramValue
                Next
                MsgBox(sMsg)

                ' If we know WHICH param we are looking for, then:
                'A) If a standard parameter, we can get it via BuiltInParam signature of Parameter method:
                Dim parInBuilt As Parameter
                Try
                    parInBuilt = elem.Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM)
                    If Not parInBuilt Is Nothing Then
                        Dim parInBuiltName As String = parInBuilt.Definition.Name
                        Dim parInBuiltType As String = parInBuilt.StorageType.ToString
                        Dim parInBuiltValue As String = LabUtils.GetParameterValue2(parInBuilt, doc)
                        MsgBox("FAMILY_BASE_LEVEL_OFFSET_PARAM: Name=" & parInBuiltName & "; Type=" & parInBuiltType & "; Value=" & parInBuiltValue)
                    Else
                        MsgBox("FAMILY_BASE_LEVEL_OFFSET_PARAM is NOT available for this element")
                    End If
                Catch
                    MsgBox("FAMILY_BASE_LEVEL_OFFSET_PARAM is NOT available for this element")
                End Try

                ' B) For a Shared parameter, we can get it via "GUID" signature 
                ' of Parameter method ... this will be shown later in Labs 4 ...

                ' C) or we can get the parameter by name, since Revit 2009 ... 
                ' previously we had to use a utility method to loop over all 
                ' parameters and search for the name. this works for either 
                ' standard or shared parameters:
                Const csParamToFind As String = "Base Offset"
                Dim parByName As Parameter = elem.Parameter(csParamToFind)
                If parByName Is Nothing Then
                    MsgBox(csParamToFind & " is NOT available for this element")
                Else
                    Dim parByNameName As String = parByName.Definition.Name
                    Dim parByNameType As String = parByName.StorageType.ToString
                    Dim parByNameValue As String = LabUtils.GetParameterValue2(parByName, doc)
                    MsgBox(csParamToFind & ": Name=" & parByNameName & "; Type=" & parByNameType & "; Value=" & parByNameValue)
                End If

            Next

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab4_2_ExportParametersToExcel"
    ''' <summary>
    ''' Export all parameters for each model element to Excel, one sheet per category.
    ''' </summary> 
    Public Class Lab4_2_ExportParametersToExcel
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument

            ' First extract and group the data from Revit in a convenient Map class:
            ' (Key=category name, Val=Set of Elements)
            '------------------------------------------------------------------------
            Dim sortedElements As Autodesk.Revit.Collections.Map = app.Create.NewMap()
            ' Iterate all non-Symbol elements and store in map
            Dim iter As IEnumerator = doc.Elements
            Do While (iter.MoveNext())
                ' We look for all non-Symbol Elements which have a Category
                Dim element As Element = iter.Current
                If Not (TypeOf element Is Symbol) Then
                    Dim category As Category = element.Category
                    If Not (category Is Nothing) Then
                        Dim elementSet As ElementSet
                        ' If we already have this Key, get its Value (Set)
                        ' Otherwise, create the new Key and Value (Set
                        If sortedElements.Contains(category.Name) Then
                            elementSet = sortedElements.Item(category.Name)
                        Else
                            elementSet = app.Create.NewElementSet()
                            sortedElements.Insert(category.Name, elementSet)
                        End If
                        ' Add the element to the Set
                        elementSet.Insert(element)
                    End If
                End If
            Loop

            ' Export parameters
            ' Launch/Get Excel (via COM Interop)
            '-----------------------------------
            ' Use the following line for Excel 2003 (11.0) and similar in the rest of this lab...
            Dim excel As MsExcel.Application = New MsExcel.ApplicationClass()
            '... or this for up to Excel 2002 (10.0):
            'Dim excel As Excel.Application = New Excel.ApplicationClass()
            If (excel Is Nothing) Then
                MsgBox("Failed to get or start Excel!?")
                Return IExternalCommand.Result.Failed
            End If
            excel.Visible = True        ' Make it visible "live" to the user

            ' Add a new work-book and delete the 3 default work-sheets
            Dim workbook As MsExcel.Workbook = excel.Workbooks.Add()
            Dim worksheet As MsExcel.Worksheet
            Do While workbook.Sheets.Count > 1
                worksheet = workbook.Sheets.Item(1)
                worksheet.Delete()
            Loop

            ' Loop all collected Categories and create one worksheet for each
            Dim mapIter As Autodesk.Revit.Collections.MapIterator = sortedElements.ForwardIterator
            Do While (mapIter.MoveNext())

                ' retrieve stored category and ElementSet
                Dim categoryName As String = mapIter.Key
                Dim elementSet As Autodesk.Revit.ElementSet = mapIter.Current

                ' create and name the worksheet
                worksheet = excel.Worksheets.Add()
                worksheet.Name = categoryName

                ' we could find the list of Parameter names available for ALL the Elements in this Set,
                '   but let's keep it simple and use all parameters encountered (will Try-Catch later)
                Dim allParamNamesEncountered As Autodesk.Revit.Collections.Set = app.Create.NewSet()

                ' loop through all the elements passed to the method
                Dim setIter As IEnumerator = elementSet.ForwardIterator
                Do While (setIter.MoveNext())
                    Dim el As Element = setIter.Current
                    Dim parameters As ParameterSet = el.Parameters
                    If Not (parameters.IsEmpty) Then
                        'Another way to loop the parameters is via ParameterSetIterator:
                        Dim definitionNames As Autodesk.Revit.Collections.Set = app.Create.NewSet()
                        Dim paramIter As ParameterSetIterator = parameters.ForwardIterator
                        Do While paramIter.MoveNext()
                            Dim parameter As Parameter = paramIter.Current
                            Dim name As String = parameter.Definition.Name
                            If Not allParamNamesEncountered.Contains(name) Then
                                allParamNamesEncountered.Insert(name)
                            End If
                        Loop
                    End If
                Loop

                ' add the HEADER row in Bold
                worksheet.Cells(1, 1).Value = "ID"
                Dim paramName As String
                Dim column As Integer = 2
                For Each paramName In allParamNamesEncountered
                    worksheet.Cells(1, column).Value = paramName
                    excel.Columns(column).EntireColumn.AutoFit()
                    column = column + 1
                Next
                excel.Rows("1").Font.Bold = True

                ' finally, export a row per each element that belongs to the category
                Dim elem As Element
                Dim row As Integer = 2
                For Each elem In elementSet

                    ' first column is the element id (display it as an integer)
                    worksheet.Cells(row, 1).Value = elem.Id.Value
                    ' the other columns are parameter values
                    column = 2
                    For Each paramName In allParamNamesEncountered
                        Dim paramValue As String
                        Try
                            paramValue = LabUtils.GetParameterValue(elem.Parameter(paramName))
                        Catch
                            paramValue = "*NA*"
                        End Try
                        worksheet.Cells(row, column).Value = paramValue
                        column = column + 1
                    Next

                    row = row + 1

                Next ' row

            Loop ' categories (worksheets)

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab4_3_1_CreateAndBindSharedParam"
    ''' <summary>
    ''' 4.3.1 Create and bind shared parameter.
    ''' </summary>
    Public Class Lab4_3_1_CreateAndBindSharedParam
        Implements IExternalCommand

        Public Shared Bic As BuiltInCategory = BuiltInCategory.OST_Doors

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Application = commandData.Application

            ' Get the current Shared Params Definition File
            Dim sharedParamsFile As DefinitionFile = LabUtils.GetSharedParamsFile(app)
            If (sharedParamsFile Is Nothing) Then
                MsgBox("Error in getting the Shared Params File?")
                Return IExternalCommand.Result.Failed
            End If

            ' Get or Create the Shared Params Group
            Dim sharedParamsGroup As Parameters.DefinitionGroup
            sharedParamsGroup = LabUtils.GetOrCreateSharedParamsGroup(sharedParamsFile, SharedParamsGroupAPI)
            If (sharedParamsGroup Is Nothing) Then
                MsgBox("Error in getting the Shared Params Group?")
                Return IExternalCommand.Result.Failed
            End If

            ' Get or Create the Shared Params Definition
            Dim fireRatingParamDef As Parameters.Definition = LabUtils.GetOrCreateSharedParamsDefinition( _
             sharedParamsGroup, ParameterType.Number, LabConstants.SharedParamsDefFireRating, True)
            If (fireRatingParamDef Is Nothing) Then
                MsgBox("Error in creating 'API Added' parameter?")
                Return IExternalCommand.Result.Failed
            End If

            ' Create the Category Set for binding and add "Doors"
            Dim catSet As CategorySet = app.Create.NewCategorySet()
            Try
                catSet.Insert(app.ActiveDocument.Settings.Categories.Item(Bic))
            Catch
                MsgBox("Error when adding 'Doors' category to parameters binding set?")
                Return IExternalCommand.Result.Failed
            End Try

            ' Bind the Param
            Try
                Dim binding As Parameters.Binding = app.Create.NewInstanceBinding(catSet)
                ' We could check if already bound, but looks like Insert will just ignore it in such case
                app.ActiveDocument.ParameterBindings.Insert(fireRatingParamDef, binding)
            Catch
                MsgBox("Error in binding shared parameter !?")
                Return IExternalCommand.Result.Failed
            End Try

            MsgBox("Parameter binding Successful!")
            Return IExternalCommand.Result.Succeeded

        End Function
    End Class

#End Region

#Region "Lab4_3_2_ExportSharedParamToExcel"
    ''' <summary>
    ''' 4.3.2 Export all door ids and FireRating param values to Excel.
    ''' </summary>
    Public Class Lab4_3_2_ExportSharedParamToExcel
        Implements IExternalCommand
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Application = commandData.Application

            ' Launch Excel (same as in Lab 4_2, so we really should have better created some utils...)
            Dim excel As MsExcel.Application = New MsExcel.ApplicationClass()
            If (excel Is Nothing) Then
                MsgBox("Failed to get or start Excel!?")
                Return IExternalCommand.Result.Failed
            End If
            excel.Visible = True
            Dim workbook As MsExcel.Workbook = excel.Workbooks.Add()
            Dim worksheet As MsExcel.Worksheet
            Do While workbook.Sheets.Count > 1
                worksheet = workbook.Sheets.Item(1)
                worksheet.Delete()
            Loop
            worksheet = excel.ActiveSheet
            worksheet.Name = "Revit Doors"
            ' Write the header row
            worksheet.Cells(1, 1).Value = "ID"
            worksheet.Cells(1, 2).Value = "Level"
            worksheet.Cells(1, 3).Value = "Tag"
            worksheet.Cells(1, 4).Value = LabConstants.SharedParamsDefFireRating
            excel.Rows("1").Font.Bold = True

            ' Use our utility from LabUtils to get all doors
            Dim doors As List(Of Element)
            doors = LabUtils.GetAllModelInstancesForACategory(app, Lab4_3_1_CreateAndBindSharedParam.Bic)

            ' Get shared param Guid
            Dim paramGuid As Guid = LabUtils.SharedParamGUID( _
              app, LabConstants.SharedParamsGroupAPI, LabConstants.SharedParamsDefFireRating)
            If paramGuid.Equals(Guid.Empty) Then
                MsgBox("No Shared param found in the file !? - aborting...")
                Return IExternalCommand.Result.Failed
            End If

            ' Loop all doors and export each to an Excel row
            Dim door As Element
            Dim row As Integer = 2
            For Each door In doors

                'ID
                worksheet.Cells(row, 1).Value = door.Id.Value

                'Level
                worksheet.Cells(row, 2).Value = door.Level.Name

                'Tag
                Dim tagParameter As Parameter = _
                 door.Parameter(BuiltInParameter.ALL_MODEL_MARK)
                If Not (tagParameter Is Nothing) Then
                    worksheet.Cells(row, 3).Value = tagParameter.AsString
                End If

                '*FireRating*
                Dim parameter As Parameter = door.Parameter(paramGuid)
                If Not (parameter Is Nothing) Then
                    worksheet.Cells(row, 4).Value = parameter.AsDouble
                End If

                row = row + 1
            Next
            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab4_3_3_ImportSharedParamFromExcel"
    ''' <summary>
    ''' 4.3.3 Import updated FireRating param values from Excel.
    ''' </summary>
    Public Class Lab4_3_3_ImportSharedParamFromExcel
        Implements IExternalCommand
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Application = commandData.Application

            ' Let user select the Excel file
            Dim dlgFileXLS As New W.OpenFileDialog()
            With dlgFileXLS
                .Title = "Select the Excel file to update Revit Shared Parameters from"
                .Filter = "Excel spreadsheet files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*)|*"
                If Not .ShowDialog() = W.DialogResult.OK Then
                    Return IExternalCommand.Result.Cancelled
                End If
            End With

            ' Launch Excel and open the selected file
            Dim excel As MsExcel.Application = New MsExcel.ApplicationClass()
            If (excel Is Nothing) Then
                MsgBox("Failed to get or start Excel!?")
                Return IExternalCommand.Result.Failed
            End If
            excel.Visible = True
            Dim workbook As MsExcel.Workbook = excel.Workbooks.Open(dlgFileXLS.FileName)
            Dim worksheet As MsExcel.Worksheet = workbook.ActiveSheet

            ' Starting from row 2, loop the rows and extract Id and FireRating param.
            Dim id As Integer
            Dim fireRatingValue As Double
            Dim row As Integer = 2
            Do
                Try
                    ' Extract relevant XLS values
                    id = worksheet.Cells(row, 1).Value
                    If id <= 0 Then Exit Do
                    fireRatingValue = worksheet.Cells(row, 4).Value

                    ' Get document's door element via Id
                    Dim elementId As ElementId
                    elementId.Value = id
                    Dim door As Element = app.ActiveDocument.Element(elementId)

                    ' Set the param
                    If Not (door Is Nothing) Then
                        Dim parameter As Parameter = door.Parameter(LabConstants.SharedParamsDefFireRating)
                        parameter.Set(fireRatingValue)
                    End If

                Catch
                    Exit Do
                End Try

                row = row + 1
            Loop

#If USE_PROCESS_GET_PROCESSES Then
            ' Set focus back to Revit (there may be a better way, but this works :-)
            Dim p, cPs() As Process
            cPs = Process.GetProcesses()
            For Each p In cPs
                Try
                    If p.ProcessName.ToUpper.Substring(0, 5) = "REVIT" Then
                        AppActivate(p.Id)
                        Exit For
                    End If
                Catch
                End Try
            Next
#End If ' USE_PROCESS_GET_PROCESSES

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab4_4_1_CreatePerDocParameters"
    ''' <summary>
    ''' Command to add and bind a visible and an invisible per-doc parameter.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Lab4_4_1_PerDocParams
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument

            ' Get or create relevant shared params stuff:
            Dim sharedParamsFile As DefinitionFile = LabUtils.GetSharedParamsFile(app)
            If (sharedParamsFile Is Nothing) Then
                MsgBox("Error in getting the Shared Params File?")
                Return IExternalCommand.Result.Failed
            End If

            Dim sharedParamsGroup As Parameters.DefinitionGroup
            sharedParamsGroup = LabUtils.GetOrCreateSharedParamsGroup( _
              sharedParamsFile, LabConstants.ParamGroupName)
            If (sharedParamsGroup Is Nothing) Then
                MsgBox("Error in getting the Shared Params Group?")
                Return IExternalCommand.Result.Failed
            End If

            ' Visible param
            Dim docParamDefVisible As Parameters.Definition = LabUtils.GetOrCreateSharedParamsDefinition( _
            sharedParamsGroup, ParameterType.Integer, LabConstants.ParamNameVisible, True)
            If (docParamDefVisible Is Nothing) Then
                MsgBox("Error in creating visible per-doc parameter?")
                Return IExternalCommand.Result.Failed
            End If

            'INVisible param
            Dim docParamDefInvisible As Parameters.Definition = LabUtils.GetOrCreateSharedParamsDefinition( _
            sharedParamsGroup, ParameterType.Integer, LabConstants.ParamNameInvisible, False)
            If (docParamDefInvisible Is Nothing) Then
                MsgBox("Error in creating invisible per-doc parameter?")
                Return IExternalCommand.Result.Failed
            End If

            ' Bind the params
            Try
                Dim catSet As CategorySet = app.Create.NewCategorySet()
                catSet.Insert(app.ActiveDocument.Settings.Categories.Item( _
                  BuiltInCategory.OST_ProjectInformation))
                Dim binding As Parameters.Binding = app.Create.NewInstanceBinding(catSet)
                app.ActiveDocument.ParameterBindings.Insert(docParamDefVisible, binding)
                app.ActiveDocument.ParameterBindings.Insert(docParamDefInvisible, binding)
            Catch e As Exception
                MsgBox("Error in binding shared parameter: " & e.Message)
                Return IExternalCommand.Result.Failed
            End Try

            ' Set the initial values
            '-----------------------

            ' Get the singleton Project Info Element

            Dim projInfoElem As Element = LabUtils.GetProjectInfoElem(doc, app)
            If projInfoElem Is Nothing Then
                MsgBox("NO project Info Elem found !? Aborting command...")
                Return IExternalCommand.Result.Failed
            End If

            ' For simplicity, access params by name rather than by GUID:
            projInfoElem.Parameter(LabConstants.ParamNameVisible).Set(55)
            projInfoElem.Parameter(LabConstants.ParamNameInvisible).Set(0)
            Return IExternalCommand.Result.Succeeded

        End Function

    End Class
#End Region

#Region "Lab4_4_2_IncrementPerDocParameters"
    ''' <summary>
    ''' Command to increment the invisible per-doc param
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Lab4_4_2_PerDocParams
        Implements IExternalCommand
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute

            Dim app As Application = commandData.Application
            Dim doc As Document = app.ActiveDocument

            ' Get the singleton Project Info Element
            Dim projInfoElem As Element = LabUtils.GetProjectInfoElem(doc, app)
            If projInfoElem Is Nothing Then
                MsgBox("NO project Info Elem found !? Aborting command...")
                Return IExternalCommand.Result.Failed
            End If

            ' For simplicity, access invisible param by name rather than by GUID:
            Try
                Dim param As Parameter = projInfoElem.Parameter(LabConstants.ParamNameInvisible)
                ' report OLD value
                Dim iOldValue As Integer = param.AsInteger
                MsgBox("OLD value = " & iOldValue)
                ' set and report NEW value
                param.Set(iOldValue + 1)
                MsgBox("NEW value = " & param.AsInteger)

            Catch e As Exception
                MsgBox("Failed!? : " & e.Message)
                Return IExternalCommand.Result.Failed
            End Try

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class
#End Region

End Namespace
