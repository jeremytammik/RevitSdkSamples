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
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
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
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB



<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class Command
    Implements IExternalCommand

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
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) _
    As Autodesk.Revit.UI.Result Implements IExternalCommand.Execute

        ' Setup a default message in case any exceptions are thrown that we have not
        ' explicitly handled. On failure the message will be displayed by Revit
        message = "The sample failed"
        Execute = Autodesk.Revit.UI.Result.Failed

        Try
            ' Use Microsoft Office 2003 or later.
            Dim excel As MsExcel.Application
            excel = Nothing

            ' search the entire Revit project, grouping elements by category
            Dim sortedElements As System.Collections.Generic.Dictionary(Of String, ElementSet)
            sortedElements = GetSortedNonSymbolElements(commandData.Application, commandData.Application.ActiveUIDocument.Document)
            If (sortedElements Is Nothing) Then
                Return Execute
            End If

            ' loop through all the categories found and send them to Microsoft Excel
            Dim iter As System.Collections.Generic.Dictionary(Of String, ElementSet).Enumerator = sortedElements.GetEnumerator()

            Do While (iter.MoveNext())
                ' the Revit map iterator provides access to the key as well as the values
                Dim categoryName As String = iter.Current.Key
                Dim elementSet As Autodesk.Revit.DB.ElementSet = iter.Current.Value

                Dim sendSuccess As Boolean = SendToExcel(excel, commandData.Application.Application, categoryName, elementSet)
                If (sendSuccess = False) Then
                    Return Execute
                End If

            Loop

            ' change our result to successful
            Execute = Autodesk.Revit.UI.Result.Succeeded
            Return Execute
        Catch ex As Runtime.InteropServices.COMException
            message = "Something wrong with Microsoft Office Object library."
            Return Execute
        Catch ex As Exception
            message = ex.Message
            Return Execute
        End Try

    End Function

    ''' <summary>
    ''' LaunchExcel will try to launch Excel 2003 or later. It will then create an empty workbook
    ''' and remove all of the work sheets except one which will form the basis for the first
    ''' category that is sent to Excel by this sample
    ''' </summary>
    ''' <returns>Excel application</returns>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' the SendToExcel method takes a category and a set of elements that belong to that category
    ''' and finds the common properties between those elements. Excel is then sent the names and
    ''' values of these properties, adding the category as a new sheet
    ''' </summary>
    ''' <param name="excel">Excel application</param>
    ''' <param name="application">Revit application</param>
    ''' <param name="categoryName">name of category</param>
    ''' <param name="elementSet">the elements in the category</param>
    ''' <returns>If function succeed, return True. Otherwise False</returns>
    ''' <remarks></remarks>
    Function SendToExcel(ByRef excel As MsExcel.Application, ByVal application As Autodesk.Revit.ApplicationServices.Application, _
        ByVal categoryName As String, ByVal elementSet As Autodesk.Revit.DB.ElementSet) As Boolean

        'Dim worksheet As Microsoft.Office.Interop.Excel.Worksheet
        Dim worksheet As MsExcel.Worksheet
        SendToExcel = False

        ' If excel is not running, then launch it which will result in one sheet remaining. If excel
        ' is already running then we need to create a new sheet.
        If (excel Is Nothing) Then
            excel = LaunchExcel()

            If (excel Is Nothing) Then
                Return SendToExcel
            End If

            worksheet = excel.ActiveSheet

        Else
            worksheet = excel.Worksheets.Add()

        End If

        ' Set the name of the sheet to be that of the category
        If (categoryName.Length > 31) Then
            worksheet.Name = categoryName.Substring(0, 31)
        Else
            worksheet.Name = categoryName
        End If


        ' using all the elements, find common properties amongst them
        Dim propertyNames As System.Collections.Generic.List(Of String) = GetCommonPropertyNames(application, elementSet)

        ' the first column in the sheet will be the Element ID of the element
        worksheet.Cells(1, 1).Value = "ID"

        ' now add the common property names as the column headers
        Dim propertyName As String
        Dim column As Integer = 2
        For Each propertyName In propertyNames
            worksheet.Cells(1, column).Value = propertyName
            column = column + 1
        Next

        ' devote a row to each element that belongs to the category
        Dim element As Autodesk.Revit.DB.Element
        Dim row As Integer = 2
        For Each element In elementSet

            ' first column is the element id - display it as an integer
            worksheet.Cells(row, 1).Value = element.Id.ToString()

            ' retrieve all the values, in string form for the common properties of the element
            Dim values As System.Collections.Generic.Dictionary(Of String, String) = GetValuesOfNamedProperties(application, element, propertyNames)

            column = 2
            If (values.Count > 0) Then

                For Each propertyName In propertyNames
                    ' check to see if the element actually supports that property and then set the
                    ' excel cell to be the value
                    If (values.ContainsKey(propertyName)) Then
                        worksheet.Cells(row, column).Value = values.Item(propertyName)
                    End If
                    column = column + 1
                Next

                row = row + 1

            End If

        Next

        SendToExcel = True

    End Function

    ''' <summary>
    ''' GetValuesOfNamedProperties takes a set of property names and looks up the values for those properties
    ''' returning them in a string form
    ''' </summary>
    ''' <param name="application">Revit application</param>
    ''' <param name="element">element</param>
    ''' <param name="propertyNames">names of properties</param>
    ''' <returns>The map stores the values according to the names of properties</returns>
    ''' <remarks></remarks>
    Function GetValuesOfNamedProperties(ByVal application As Autodesk.Revit.ApplicationServices.Application, _
        ByVal element As Autodesk.Revit.DB.Element, ByVal propertyNames As System.Collections.Generic.List(Of String)) _
        As System.Collections.Generic.Dictionary(Of String, String)

        Dim values As System.Collections.Generic.Dictionary(Of String, String) = New System.Collections.Generic.Dictionary(Of String, String)

        Dim parameter As Autodesk.Revit.DB.Parameter

        ' loop through all the parameters that the element has
        For Each parameter In element.Parameters

            ' the name for the parameter is held in the parameter definition
            If propertyNames.Contains(parameter.Definition.Name) Then

                Dim stringValue As String = ""

                ' take the internal type of the parameter and convert it into a string
                Select Case parameter.StorageType

                    Case Autodesk.Revit.DB.StorageType.Double
                        stringValue = parameter.AsDouble

                        ' in the case of ElementId, retrieve the element, if possible, and use its name
                    Case Autodesk.Revit.DB.StorageType.ElementId
                        Dim paramElement As Autodesk.Revit.DB.Element = element.Document.GetElement(parameter.AsElementId)
                        If Not (paramElement Is Nothing) Then
                            stringValue = paramElement.Name
                        End If

                    Case Autodesk.Revit.DB.StorageType.Integer
                        stringValue = parameter.AsInteger

                    Case Autodesk.Revit.DB.StorageType.String
                        stringValue = parameter.AsString

                    Case Else

                End Select

                values.Add(parameter.Definition.Name, stringValue)

            End If

        Next

        GetValuesOfNamedProperties = values

    End Function

    ''' <summary>
    ''' GetCommonPropertyNames takes a set of elements and seeks the property names that are common between them
    ''' If an element does not support any properties at all it is ignored.
    ''' The process of finding the common elements is done by collecting all the names of parameters for
    ''' the first element and then removing those that are not used by the other elements
    ''' </summary>
    ''' <param name="application">Revit application</param>
    ''' <param name="elementSet">elements</param>
    ''' <returns>A set of common properties</returns>
    ''' <remarks></remarks>
    Function GetCommonPropertyNames(ByVal application As Autodesk.Revit.ApplicationServices.Application, _
        ByVal elementSet As Autodesk.Revit.DB.ElementSet) As System.Collections.Generic.List(Of String)

        Dim commonProperties As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)

        ' the first element that we handle (that has properties), a
        Dim addAllProperties As Boolean = True

        ' loop through all the elements passed to the method
        Dim iter As IEnumerator
        iter = elementSet.ForwardIterator

        Do While (iter.MoveNext())
            Dim element As Autodesk.Revit.DB.Element = iter.Current

            ' get the parameters that the element supports
            Dim parameters As Autodesk.Revit.DB.ParameterSet = element.Parameters

            If Not (parameters.IsEmpty) Then

                ' definitionNames will contain all the parameter names that this element supports
                Dim definitionNames As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)

                Dim paramIter As Autodesk.Revit.DB.ParameterSetIterator = parameters.ForwardIterator

                ' loop through all of the parameters and retrieve their names
                Do While paramIter.MoveNext()

                    Dim parameter As Autodesk.Revit.DB.Parameter = paramIter.Current
                    Dim definitionName As String = parameter.Definition.Name

                    If (addAllProperties) Then
                        commonProperties.Add(definitionName)
                    End If

                    definitionNames.Add(definitionName)

                Loop

                If addAllProperties Then
                    addAllProperties = False
                Else

                    'now loop through all of the common parameter we have found so far and see if they are
                    'supported by the element. If not, then remove it from the common set
                    Dim commonDefinitionNamesToRemove As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)

                    Dim commonDefinitionName As String
                    For Each commonDefinitionName In commonProperties

                        If (definitionNames.Contains(commonDefinitionName) = False) Then
                            commonDefinitionNamesToRemove.Add(commonDefinitionName)
                        End If

                    Next

                    ' remove the uncommon parameters
                    Dim commonDefinitionNameToRemove As String
                    For Each commonDefinitionNameToRemove In commonDefinitionNamesToRemove

                        commonProperties.Remove(commonDefinitionNameToRemove)

                    Next

                End If

            End If

        Loop

        GetCommonPropertyNames = commonProperties

    End Function

    ''' <summary>
    ''' GetSortedNonSymbolElements searches the entire Revit project and sorts the elements based
    ''' upon category. Revit Symbols (Types) are ignored as we are only interested in instances of elements.
    ''' </summary>
    ''' <param name="application">Revit application</param>
    ''' <param name="document">Revit document</param>
    ''' <returns>The map stores all the elements according to their category</returns>
    ''' <remarks></remarks>
    Function GetSortedNonSymbolElements(ByVal application As Autodesk.Revit.UI.UIApplication, _
        ByVal document As Autodesk.Revit.DB.Document) As System.Collections.Generic.Dictionary(Of String, ElementSet)

        GetSortedNonSymbolElements = New System.Collections.Generic.Dictionary(Of String, ElementSet)

        'get a set of element which is not elementType
        Dim filter As Autodesk.Revit.DB.ElementIsElementTypeFilter
        filter = New Autodesk.Revit.DB.ElementIsElementTypeFilter(True)
        Dim collector As Autodesk.Revit.DB.FilteredElementCollector
        collector = New Autodesk.Revit.DB.FilteredElementCollector(application.ActiveUIDocument.Document)
        collector.WherePasses(filter)
        Dim iter As IEnumerator
        iter = collector.GetElementIterator

        Do While (iter.MoveNext())

            Dim element As Autodesk.Revit.DB.Element
            element = iter.Current
            If Not (TypeOf element Is Autodesk.Revit.DB.ElementType) Then
                ' retrieve the category of the element
                Dim category As Autodesk.Revit.DB.Category
                category = element.Category

                If Not (category Is Nothing) Then
                    Dim elementSet As Autodesk.Revit.DB.ElementSet

                    Try
                        ' if this is a category that we have seen before, then add this element to that set,
                        ' otherwise create a new set and add the element to it.
                        If GetSortedNonSymbolElements.ContainsKey(category.Name) Then
                            elementSet = GetSortedNonSymbolElements.Item(category.Name)
                        Else
                            elementSet = application.Application.Create.NewElementSet()
                            GetSortedNonSymbolElements.Add(category.Name, elementSet)
                        End If

                        elementSet.Insert(element)
                    Catch ex As Exception
                    End Try

                End If
            End If
        Loop

    End Function

End Class
