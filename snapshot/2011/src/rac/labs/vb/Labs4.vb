#Region "Header"
'Revit API .NET Labs

'Copyright (C) 2006-2010 by Autodesk, Inc.

'Permission to use, copy, modify, and distribute this software
'for any purpose and without fee is hereby granted, provided
'that the above copyright notice appears in all copies and
'that both that copyright notice and the limited warranty and
'restricted rights notice below appear in all supporting
'documentation.

'AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
'AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
'MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
'DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
'UNINTERRUPTED OR ERROR FREE.

'Use, duplication, or disclosure by the U.S. Government is subject to
'restrictions set forth in FAR 52.227-19 (Commercial Computer
'Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
'(Rights in Technical Data and Computer Software), as applicable.
#End Region

#Region "Namespaces"
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Reflection
Imports System.Linq
Imports WinForms = System.Windows.Forms
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.Attributes
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI

'' Added reference to Microsoft Excel 11.0 Object Library
'' http://support.microsoft.com/kb/306023/
'' http://support.microsoft.com/kb/302084/EN-US/
Imports X = Microsoft.Office.Interop.Excel
#End Region

Namespace Labs

#Region "Lab4_1_ElementParameters"
    ''' <summary>
    ''' List all parameters for selected elements.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="4-1"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab4_1_ElementParameters
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            Dim uidoc As UIDocument = commandData.Application.ActiveUIDocument
            Dim doc As Document = uidoc.Document

            ' Loop through all pre-selected elements:

            For Each e As Element In uidoc.Selection.Elements
                Dim e2 As Element = e
                ' enable us to assign to e2 in case analyseTypeParameters == true
                Dim s As String = String.Empty

                ' set this variable to false to analyse the element's own parameters,
                ' i.e. instance parameters for a family instance, and set it to true
                ' to analyse a family instance's type parameters:

                Dim analyseTypeParameters As Boolean = False

                If analyseTypeParameters Then
                    If TypeOf e2 Is FamilyInstance Then
                        Dim inst As FamilyInstance = TryCast(e2, FamilyInstance)
                        If inst.Symbol IsNot Nothing Then
                            e2 = inst.Symbol
                            s = " type"
                        End If
                    ElseIf TypeOf e2 Is Wall Then
                        Dim wall As Wall = TryCast(e2, Wall)
                        If wall.WallType IsNot Nothing Then
                            e2 = wall.WallType
                            s = " type"
                        End If
                        ' ... add support for other types if desired ...
                    End If
                End If

                ' Loop through and list all UI-visible element parameters

                Dim a As New List(Of String)()

                '#region 4.1.a Iterate over element parameters and retrieve their name, type and value:

                For Each p As Parameter In e2.Parameters
                    Dim name As String = p.Definition.Name
                    Dim type As String = p.StorageType.ToString()
                    Dim value As String = LabUtils.GetParameterValue2(p, uidoc.Document)
                    a.Add("Name=" + name + "; Type=" + type + "; Value=" + value _
                          + "; ValueString=" + p.AsValueString())
                Next

                '#endregion 4.1.a

                Dim what As String = e2.Category.Name + " (" + e2.Id.IntegerValue.ToString() + ")"

                LabUtils.InfoMsg(what + " has {0} parameter{1}{2}", a)

                ' If we know which param we are looking for, then:
                ' A) If a standard parameter, we can get it via BuiltInParam
                ' signature of Parameter method:

                Dim parBuiltIn As Parameter

                Try

                    '#region 4.1.b Retrieve a specific built-in parameter:
                    parBuiltIn = e.Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM)
                    '#endregion 4.1.b

                    If parBuiltIn Is Nothing Then
                        LabUtils.InfoMsg("FAMILY_BASE_LEVEL_OFFSET_PARAM is NOT available for this element.")
                    Else
                        Dim parBuiltInName As String = parBuiltIn.Definition.Name
                        Dim parBuiltInType As String = parBuiltIn.StorageType.ToString()
                        Dim parBuiltInValue As String = LabUtils.GetParameterValue2(parBuiltIn, doc)
                        LabUtils.InfoMsg("FAMILY_BASE_LEVEL_OFFSET_PARAM: Name=" + parBuiltInName + "; Type=" + parBuiltInType + "; Value=" + parBuiltInValue)
                    End If
                Catch generatedExceptionName As Exception
                    LabUtils.InfoMsg("FAMILY_BASE_LEVEL_OFFSET_PARAM is NOT available for this element.")
                End Try

                ' B) For a shared parameter, we can get it via "GUID" signature
                ' of Parameter method ... this will be shown later in Labs 4 ...

                ' C) or we can get the parameter by name:
                ' alternatively, loop through all parameters and
                ' search for the name (this works for either
                ' standard or shared):

                Const paramName As String = "Base Offset"
                Dim parByName As Parameter = e.Parameter(paramName)
                If parByName Is Nothing Then
                    LabUtils.InfoMsg(paramName + " is NOT available for this element.")
                Else
                    Dim parByNameName As String = parByName.Definition.Name
                    Dim parByNameType As String = parByName.StorageType.ToString()
                    Dim parByNameValue As String = LabUtils.GetParameterValue2(parByName, doc)
                    LabUtils.InfoMsg(paramName + ": Name=" + parByNameName + "; Type=" + parByNameType + "; Value=" + parByNameValue)
                End If

            Next
            Return Result.Failed
        End Function

    End Class
#End Region

#Region "Lab4_2_ExportParametersToExcel"
    ''' <summary>
    ''' Export all parameters for each model element to Excel, one sheet per category.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="4-2"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab4_2_ExportParametersToExcel
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim doc As Document = app.ActiveUIDocument.Document

            ' extract and group the data from Revit in a dictionary, where
            ' the key is the category name and the value is a list of elements.
            'Autodesk.Revit.Collections.Map sortedElements = app.Create.NewMap()

            Dim sortedElements As New Dictionary(Of String, List(Of Element))()

            ' iterate all non-symbol elements and store in dictionary

            Dim collector As FilteredElementCollector = New FilteredElementCollector(doc).WhereElementIsNotElementType()

            For Each e As Element In collector
                Debug.Assert(Not (TypeOf e Is ElementType), "expected no ElementType elements")

                Dim category As Category = e.Category

                If category IsNot Nothing Then
                    Dim name As String = category.Name

                    Dim elementSet As List(Of Element)

                    ' if we already have this key, get its value (set);
                    ' otherwise, create the new key and value (set):

                    If sortedElements.ContainsKey(name) Then
                        elementSet = sortedElements(name)
                    Else
                        elementSet = New List(Of Element)()
                        sortedElements.Add(name, elementSet)
                    End If
                    elementSet.Add(e)
                End If
            Next

            ' Launch/Get Excel via COM Interop:

            Dim excel As New X.Application()

            If excel Is Nothing Then
                LabUtils.ErrorMsg("Failed to get or start Excel.")
                Return Result.Failed
            End If
            excel.Visible = True
            Dim workbook As X.Workbook = excel.Workbooks.Add(Missing.Value)
            Dim worksheet As X.Worksheet

            ' Loop through all collected categories and create a worksheet for each except first

            Dim keys As New List(Of String)(sortedElements.Keys)
            keys.Sort()
            keys.Reverse()
            ' the worksheet added last shows up first in the excel tab
            Dim first As Boolean = True
            For Each categoryName As String In keys
                Dim elementSet As List(Of Element) = sortedElements(categoryName)

                ' create and name the worksheet

                If first Then
                    worksheet = TryCast(workbook.Sheets.Item(1), X.Worksheet)
                    first = False
                Else
                    worksheet = TryCast(excel.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value), X.Worksheet)
                End If

                worksheet.Name = If((31 < categoryName.Length), categoryName.Substring(0, 31), categoryName)

                ' we could find the list of Parameter names available for ALL the Elements
                ' in this Set, but let's keep it simple and use all parameters encountered:

                Dim allParamNamesEncountered As New List(Of String)()

                ' loop through all the elements passed to the method

                For Each e As Element In elementSet
                    Dim parameters As ParameterSet = e.Parameters

                    ' an easier way to loop the parameters than ParameterSetIterator:

                    For Each parameter As Parameter In parameters
                        Dim name As String = parameter.Definition.Name
                        If Not allParamNamesEncountered.Contains(name) Then
                            allParamNamesEncountered.Add(name)
                        End If
                    Next
                Next
                allParamNamesEncountered.Sort()

                ' add the HEADER row in Bold

                worksheet.Cells(1, 1) = "ID"
                Dim column As Integer = 2

                For Each paramName As String In allParamNamesEncountered
                    worksheet.Cells(1, column) = paramName
                    column += 1
                Next
                worksheet.get_Range("A1", "Z1").Font.Bold = True
                worksheet.get_Range("A1", "Z1").EntireColumn.AutoFit()
                Dim row As Integer = 2
                For Each e As Element In elementSet
                    ' first column is the element id, which we display as an integer

                    worksheet.Cells(row, 1) = e.Id.IntegerValue
                    column = 2
                    For Each paramName As String In allParamNamesEncountered
                        Dim paramValue As String
                        Try
                            paramValue = LabUtils.GetParameterValue(e.Parameter(paramName))
                        Catch generatedExceptionName As Exception
                            paramValue = "*NA*"
                        End Try
                        worksheet.Cells(row, column) = paramValue
                        column += 1
                    Next
                    row += 1
                    ' row
                Next
            Next
            ' category = worksheet
            Return Result.Failed
        End Function
    End Class
#End Region

#Region "Lab4_3_1_CreateAndBindSharedParam"
    ''' <summary>
    ''' Create and bind shared parameter.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab4_3_1_CreateAndBindSharedParam
        Implements IExternalCommand
        '
        ' what element type are we interested in? the standard SDK FireRating
        ' sample uses BuiltInCategory.OST_Doors. we also test using
        ' BuiltInCategory.OST_Walls to demonstrate that the same technique
        ' works with system families just as well as with standard ones.
        '
        ' To test attaching shared parameters to inserted DWG files,
        ' which generate their own category on the fly, we also identify
        ' the category by category name.
        '
        ' The last test is for attaching shared parameters to model groups.
        '
        Public Shared Target As BuiltInCategory = BuiltInCategory.OST_Doors
        'static public BuiltInCategory Target = BuiltInCategory.OST_Walls;
        'static public string Target = "Drawing1.dwg";
        'static public BuiltInCategory Target = BuiltInCategory.OST_IOSModelGroups; // doc.Settings.Categories.get_Item returns null
        'static public string Target = "Model Groups"; // doc.Settings.Categories.get_Item throws an exception SystemInvalidOperationException "Operation is not valid due to the current state of the object."
        'static public BuiltInCategory Target = BuiltInCategory.OST_Lines; // model lines

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim uiapp As UIApplication = commandData.Application
            Dim app As Application = uiapp.Application
            Dim doc As Document = uiapp.ActiveUIDocument.Document
            Dim cat As Category = Nothing

            If cat Is Nothing Then
                ' the category we are defining the parameter for
                Try
                    cat = doc.Settings.Categories.Item(Target)
                Catch ex As Exception
                    message = "Error obtaining the shared param document category: " + ex.Message
                    Return Result.Failed
                End Try
                If cat Is Nothing Then
                    message = "Unable to obtain the shared param document category."
                    Return Result.Failed
                End If
            End If

            ' get the current shared params definition file
            Dim sharedParamsFile As DefinitionFile = LabUtils.GetSharedParamsFile(app)
            If sharedParamsFile Is Nothing Then
                message = "Error getting the shared params file."
                Return Result.Failed
            End If

            ' get or create the shared params group
            Dim sharedParamsGroup As DefinitionGroup = LabUtils.GetOrCreateSharedParamsGroup(sharedParamsFile, LabConstants.SharedParamsGroupAPI)
            If sharedParamsGroup Is Nothing Then
                message = "Error getting the shared params group."
                Return Result.Failed
            End If

            ' visibility of the new parameter:
            ' Category.AllowsBoundParameters property indicates if a category can
            ' have shared or project parameters. If it is false, it may not be bound
            ' to shared parameters using the BindingMap. Please not that non-user-visible
            ' parameters can still be bound to these categories.
            Dim visible As Boolean = cat.AllowsBoundParameters

            ' get or create the shared params definition
            Dim fireRatingParamDef As Definition = LabUtils.GetOrCreateSharedParamsDefinition(sharedParamsGroup, ParameterType.Number, LabConstants.SharedParamsDefFireRating, visible)
            If fireRatingParamDef Is Nothing Then
                message = "Error in creating shared parameter."
                Return Result.Failed
            End If

            ' create the category set for binding and add the category
            ' we are interested in, doors or walls or whatever:
            Dim catSet As CategorySet = app.Create.NewCategorySet()
            Try
                catSet.Insert(cat)
            Catch generatedExceptionName As Exception
                message = String.Format("Error adding '{0}' category to parameters binding set.", cat.Name)
                Return Result.Failed
            End Try

            ' bind the param
            Try
                Dim binding As Binding = app.Create.NewInstanceBinding(catSet)
                ' We could check if already bound, but looks like Insert will just ignore it in such case
                ' You can also specify the parameter group here:
                'doc.ParameterBindings.Insert( fireRatingParamDef, binding, BuiltInParameterGroup.PG_GEOMETRY );
                doc.ParameterBindings.Insert(fireRatingParamDef, binding)
            Catch ex As Exception
                message = ex.Message
                Return Result.Failed
            End Try
            Return Result.Succeeded
        End Function
    End Class
#End Region

#Region "Lab4_3_2_ExportSharedParamToExcel"
    ''' <summary>
    ''' Export all target element ids and their FireRating param values to Excel.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab4_3_2_ExportSharedParamToExcel
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim uiapp As UIApplication = commandData.Application
            Dim app As Application = uiapp.Application
            Dim doc As Document = uiapp.ActiveUIDocument.Document
            Dim cat As Category = doc.Settings.Categories.Item(Lab4_3_1_CreateAndBindSharedParam.Target)
            ' Launch Excel (same as in Lab 4_2, so we really should have better created some utils...)
            Dim excel As X.Application = New X.ApplicationClass()
            If excel Is Nothing Then
                LabUtils.ErrorMsg("Failed to get or start Excel.")
                Return Result.Failed
            End If
            excel.Visible = True
            Dim workbook As X.Workbook = excel.Workbooks.Add(Missing.Value)
            Dim worksheet As X.Worksheet
            'while( 1 < workbook.Sheets.Count )
            '{
            ' worksheet = workbook.Sheets.get_Item( 0 ) as X.Worksheet;
            ' worksheet.Delete();
            '}
            worksheet = TryCast(excel.ActiveSheet, X.Worksheet)
            worksheet.Name = "Revit " + cat.Name
            worksheet.Cells(1, 1) = "ID"
            worksheet.Cells(1, 2) = "Level"
            worksheet.Cells(1, 3) = "Tag"
            worksheet.Cells(1, 4) = LabConstants.SharedParamsDefFireRating
            worksheet.get_Range("A1", "Z1").Font.Bold = True

            Dim elems As List(Of Element) = LabUtils.GetTargetInstances(doc, Lab4_3_1_CreateAndBindSharedParam.Target)

            ' Get Shared param Guid

            Dim paramGuid As Guid = LabUtils.SharedParamGUID(app, LabConstants.SharedParamsGroupAPI, LabConstants.SharedParamsDefFireRating)

            If paramGuid.Equals(Guid.Empty) Then
                LabUtils.ErrorMsg("No Shared param found in the file - aborting...")
                Return Result.Failed
            End If

            ' Loop through all elements and export each to an Excel row

            Dim row As Integer = 2
            For Each e As Element In elems
                worksheet.Cells(row, 1) = e.Id.IntegerValue
                ' ID
                worksheet.Cells(row, 2) = e.Level.Name
                ' Level
                ' Tag:
                Dim tagParameter As Parameter = e.Parameter(BuiltInParameter.ALL_MODEL_MARK)
                If tagParameter IsNot Nothing Then
                    worksheet.Cells(row, 3) = tagParameter.AsString()
                End If
                ' FireRating:
                Dim parameter As Parameter = e.Parameter(paramGuid)
                If parameter IsNot Nothing Then
                    worksheet.Cells(row, 4) = parameter.AsDouble()
                End If
                row += 1
            Next
            Return Result.Succeeded
        End Function
    End Class
#End Region

#Region "Lab4_3_3_ImportSharedParamFromExcel"
    ''' <summary>
    ''' Import updated FireRating param values from Excel.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab4_3_3_ImportSharedParamFromExcel
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim uiapp As UIApplication = commandData.Application
            Dim app As Application = uiapp.Application
            Dim doc As Document = uiapp.ActiveUIDocument.Document

            'BindingMap bindingMap = doc.ParameterBindings; // slow, fixed in 2009 wu 3, cf. case 1247995

            Dim paramGuid As Guid = LabUtils.SharedParamGUID(app, LabConstants.SharedParamsGroupAPI, LabConstants.SharedParamsDefFireRating)

            ' Let user select the Excel file
            Dim dlg As New WinForms.OpenFileDialog()
            dlg.Title = "Select source Excel file from which to update Revit shared parameters"
            dlg.Filter = "Excel spreadsheet files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*)|*"
            If WinForms.DialogResult.OK <> dlg.ShowDialog() Then
                Return Result.Cancelled
            End If
            '
            ' Launch/Get Excel via COM Interop:
            '
            Dim excel As New X.Application()
            If excel Is Nothing Then
                LabUtils.ErrorMsg("Failed to get or start Excel.")
                Return Result.Failed
            End If
            excel.Visible = True
            Dim workbook As X.Workbook = excel.Workbooks.Open(dlg.FileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, _
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, _
            Missing.Value, Missing.Value, Missing.Value)
            Dim worksheet As X.Worksheet = TryCast(workbook.ActiveSheet, X.Worksheet)
            '
            ' Starting from row 2, loop the rows and extract Id and FireRating param.
            '
            Dim id As Integer
            Dim fireRatingValue As Double
            Dim row As Integer = 2
            While True
                Try
                    ' Extract relevant XLS values
                    Dim r As X.Range = TryCast(worksheet.Cells(row, 1), X.Range)
                    If r.Value2 Is Nothing Then
                        Exit Try
                    End If
                    Dim d As Double = CDbl(r.Value2)
                    id = CInt(d)
                    If 0 >= id Then
                        Exit Try
                    End If
                    r = TryCast(worksheet.Cells(row, 4), X.Range)
                    fireRatingValue = CDbl(r.Value2)
                    ' Get document's door element via Id
                    Dim elementId As New ElementId(id)
                    Dim door As Element = doc.Element(elementId)
                    ' Set the param
                    If door IsNot Nothing Then
                        'Parameter parameter = door.Parameter( LabConstants.SharedParamsDefFireRating );
                        Dim parameter As Parameter = door.Parameter(paramGuid)
                        parameter.[Set](fireRatingValue)
                    End If
                Catch generatedExceptionName As Exception
                    Exit Try
                End Try
                row += 1
            End While

            Return Result.Succeeded
        End Function
    End Class
#End Region

#Region "Lab4_4_1_CreatePerDocParameters"
    ''' <summary>
    ''' Add and bind a visible and an invisible per-doc parameter.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab4_4_1_CreatePerDocParameters
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim doc As Document = app.ActiveUIDocument.Document

            If doc.IsFamilyDocument Then
                message = "This command can only be used in a project, not in a family file."
                Return Result.Failed
            End If

            ' get the current shared params definition file
            Dim sharedParamsFile As DefinitionFile = LabUtils.GetSharedParamsFile(app.Application)
            If sharedParamsFile Is Nothing Then
                message = "Error getting the shared params file."
                Return Result.Failed
            End If
            ' get or create the shared params group
            Dim sharedParamsGroup As DefinitionGroup = LabUtils.GetOrCreateSharedParamsGroup(sharedParamsFile, LabConstants.ParamGroupName)

            If sharedParamsGroup Is Nothing Then
                message = "Error getting the shared params group."
                Return Result.Failed
            End If
            ' visible param
            Dim docParamDefVisible As Definition = LabUtils.GetOrCreateSharedParamsDefinition(sharedParamsGroup, ParameterType.[Integer], LabConstants.ParamNameVisible, True)

            If docParamDefVisible Is Nothing Then
                message = "Error creating visible per-doc parameter."
                Return Result.Failed
            End If
            ' invisible param
            Dim docParamDefInvisible As Definition = LabUtils.GetOrCreateSharedParamsDefinition(sharedParamsGroup, ParameterType.[Integer], LabConstants.ParamNameInvisible, False)

            If docParamDefInvisible Is Nothing Then
                message = "Error creating invisible per-doc parameter."
                Return Result.Failed
            End If
            ' bind the param
            Try
                Dim catSet As CategorySet = app.Application.Create.NewCategorySet()
                catSet.Insert(doc.Settings.Categories.Item(BuiltInCategory.OST_ProjectInformation))
                Dim binding As Binding = app.Application.Create.NewInstanceBinding(catSet)
                doc.ParameterBindings.Insert(docParamDefVisible, binding)
                doc.ParameterBindings.Insert(docParamDefInvisible, binding)
            Catch e As Exception
                message = "Error binding shared parameter: " + e.Message
                Return Result.Failed
            End Try
            ' set the initial values
            ' get the singleton project info element
            Dim projInfoElem As Element = LabUtils.GetProjectInfoElem(doc)

            If projInfoElem Is Nothing Then
                message = "No project info elem found. Aborting command..."
                Return Result.Failed
            End If
            ' for simplicity, access params by name rather than by GUID:
            projInfoElem.Parameter(LabConstants.ParamNameVisible).[Set](55)
            projInfoElem.Parameter(LabConstants.ParamNameInvisible).[Set](0)
            Return Result.Succeeded
        End Function
    End Class
#End Region

#Region "Lab4_4_2_IncrementPerDocParameters"
    ''' <summary>
    ''' Increment the invisible per-doc param.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab4_4_2_IncrementPerDocParameters
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim doc As Document = app.ActiveUIDocument.Document

            If doc.IsFamilyDocument Then
                message = "This command can only be used in a project, not in a family file."
                Return Result.Failed
            End If

            ' get the singleton project info element
            Dim projInfoElem As Element = LabUtils.GetProjectInfoElem(doc)
            If projInfoElem Is Nothing Then
                message = "No project info elem found. Aborting command..."
                Return Result.Failed
            End If

            ' For simplicity, access invisible param by name rather than by GUID:

            Try
                Dim param As Parameter = projInfoElem.Parameter(LabConstants.ParamNameInvisible)

                ' report OLD value

                Dim iOldValue As Integer = param.AsInteger()
                LabUtils.InfoMsg("OLD value = " + iOldValue.ToString())

                ' set and report NEW value

                param.[Set](iOldValue + 1)
                LabUtils.InfoMsg("NEW value = " + param.AsInteger().ToString())
            Catch e As System.Exception
                message = "Failed: " + e.Message
                Return Result.Failed
            End Try
            Return Result.Succeeded
        End Function
    End Class
#End Region

End Namespace

