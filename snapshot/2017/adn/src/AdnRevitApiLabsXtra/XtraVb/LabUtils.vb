#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2006-2017 by Autodesk, Inc.
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
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.UI.Selection
Imports Microsoft.VisualBasic.Constants
#End Region

Namespace XtraVb

  ''' <summary>
  ''' A collection of utility methods reused in several labs.
  ''' </summary>
  Public Class LabUtils

#Region "Formatting and message handlers"
    Public Const Caption As String = "Revit API Labs"

    ''' <summary>
    ''' Return an English plural suffix 's' or
    ''' nothing for the given number of items.
    ''' </summary>
    Public Shared Function PluralSuffix(ByVal n As Integer) As String
      Return If(1 = n, "", "s")
    End Function

    Public Shared Function DotOrColon(ByVal n As Integer) As String
      Return If(0 < n, ":", ".")
    End Function

    ''' <summary>
    ''' Format a real number and return its string representation.
    ''' </summary>
    Public Shared Function RealString(ByVal a As Double) As String
      Return a.ToString("0.##")
    End Function

    ''' <summary>
    ''' Format a point or vector and return its string representation.
    ''' </summary>
    Public Shared Function PointString(ByVal p As XYZ) As String
      Return String.Format("({0},{1},{2})", RealString(p.X), RealString(p.Y), RealString(p.Z))
    End Function

    ''' <summary>
    ''' Return a description string for a given element.
    ''' </summary>
    Public Shared Function ElementDescription(ByVal e As Element) As String
      Dim description As String = If((e.Category Is Nothing), e.[GetType]().Name, e.Category.Name)

      If e.Name IsNot Nothing Then
        description += " '" + e.Name + "'"
      End If
      Return description
    End Function

    ''' <summary>
    ''' Return a description string including element id for a given element.
    ''' </summary>
    Public Shared Function ElementDescription(ByVal e As Element, ByVal includeId As Boolean) As String
      Dim description As String = ElementDescription(e)
      If includeId Then
        description += " " + e.Id.IntegerValue.ToString()
      End If
      Return description
    End Function

    ''' <summary>
    ''' Revit TaskDialog wrapper for a short informational message.
    ''' </summary>
    Public Shared Sub InfoMsg(ByVal msg As String)
      Debug.WriteLine(msg)
      TaskDialog.Show(Caption, msg, TaskDialogCommonButtons.Ok)
    End Sub

    ''' <summary>
    ''' Revit TaskDialog wrapper for a message
    ''' with separate main instruction and content.
    ''' </summary>
    Public Shared Sub InfoMsg(ByVal msg As String, ByVal content As String)
      Debug.WriteLine(msg)
      Debug.WriteLine(content)
      Dim d As New TaskDialog(Caption)
      d.MainInstruction = msg
      d.MainContent = content
      d.Show()
    End Sub

    ''' <summary>
    ''' Revit TaskDialog wrapper for a message with separate
    ''' main instruction and list of lines of content.
    ''' The main instruction is expected to include count,
    ''' plural suffix and dot or end placeholders.
    ''' </summary>
    Public Shared Sub InfoMsg(ByVal msg As String, ByVal content As List(Of String))
      Dim n As Integer = content.Count

      InfoMsg(String.Format(msg, n, PluralSuffix(n), DotOrColon(n)), String.Join(vbCrLf, content.ToArray()))
    End Sub

    ''' <summary>
    ''' MessageBox wrapper for error message.
    ''' </summary>
    Public Shared Sub ErrorMsg(ByVal msg As String)
      Debug.WriteLine(msg)
      'WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error );
      Dim d As New TaskDialog(Caption)
      d.MainIcon = TaskDialogIcon.TaskDialogIconWarning
      d.MainInstruction = msg
      d.Show()
    End Sub

    ''' <summary>
    ''' MessageBox wrapper for question message.
    ''' </summary>
    Public Shared Function QuestionMsg(ByVal msg As String) As Boolean
      Debug.WriteLine(msg)
      'bool rc = WinForms.DialogResult.Yes
      ' == WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question );
      'Debug.WriteLine( rc ? "Yes" : "No" );
      'return rc;
      Dim d As New TaskDialog(Caption)
      d.MainIcon = TaskDialogIcon.TaskDialogIconNone
      d.MainInstruction = msg
      d.CommonButtons = TaskDialogCommonButtons.Yes Or TaskDialogCommonButtons.No
      d.DefaultButton = TaskDialogResult.Yes
      Return TaskDialogResult.Yes = d.Show()
    End Function

    ''' <summary>
    ''' MessageBox wrapper for question and cancel message.
    ''' </summary>
    Public Shared Function QuestionCancelMsg(ByVal msg As String) As TaskDialogResult
      Debug.WriteLine(msg)
      'WinForms.DialogResult rc = WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.YesNoCancel, WinForms.MessageBoxIcon.Question );
      'Debug.WriteLine( rc.ToString() );
      'return rc;
      Dim d As New TaskDialog(Caption)
      d.MainIcon = TaskDialogIcon.TaskDialogIconNone
      d.MainInstruction = msg
      d.CommonButtons = TaskDialogCommonButtons.Yes Or TaskDialogCommonButtons.No Or TaskDialogCommonButtons.Cancel
      d.DefaultButton = TaskDialogResult.Yes
      Return d.Show()
    End Function


#End Region

#Region "Geometry Utilities"

    ''' <summary>
    ''' Return the midpoint between two points.
    ''' </summary>
    Public Shared Function Midpoint(ByVal p As XYZ, ByVal q As XYZ) As XYZ
      Return p + 0.5 * (q - p)
    End Function

#End Region

#Region "Selection"

    Public Shared Function GetSingleSelectedElementOrPrompt( _
        ByVal uidoc As UIDocument, _
        ByVal type As Type) As Element

      Dim e As Element = Nothing

      'Dim ss As ElementSet = uidoc.Selection.Elements ' 2014
      Dim ids As ICollection(Of ElementId) = uidoc.Selection.GetElementIds() ' 2015

      'If 1 = ss.Size Then
      '  Dim iter As ElementSetIterator = ss.ForwardIterator()
      '  iter.MoveNext()
      '  Dim t As Type = iter.Current.[GetType]()
      '  If t.Equals(type) OrElse t.IsSubclassOf(type) Then
      '    e = TryCast(iter.Current, Element)
      '  End If
      'End If

      If 1 = ids.Count Then
        Dim id As ElementId
        For Each id In ids
          e = uidoc.Document.GetElement(id)
        Next
        Dim t As Type = e.[GetType]()
        If Not t.Equals(type) And Not t.IsSubclassOf(type) Then
          e = Nothing
        End If
      End If

      If e Is Nothing Then
        Try
          Dim r As Reference = uidoc.Selection.PickObject( _
            ObjectType.Element, _
            New TypeSelectionFilter(type), _
            String.Format("Please pick a {0} element", type.Name))

          'e = r.Element ' 2011
          e = uidoc.Document.GetElement(r) ' 2012
        Catch ex As OperationCanceledException

        End Try
      End If
      Return e
    End Function

    ''' <summary>
    ''' A selection filter for a specific System.Type.
    ''' </summary>
    Class TypeSelectionFilter
      Implements ISelectionFilter

      Private _type As Type

      Public Sub New(ByVal type As Type)
        _type = type
      End Sub

      ''' <summary>
      ''' Allow an element of the specified System.Type to be selected.
      ''' </summary>
      ''' <param name="e">A candidate element in selection operation.</param>
      ''' <returns>Return true for specified System.Type, false for all other elements.</returns>
      Public Function AllowElement( _
          ByVal e As Autodesk.Revit.DB.Element) As Boolean _
          Implements Autodesk.Revit.UI.Selection.ISelectionFilter.AllowElement

        'return null != e.Category
        ' && e.Category.Id.IntegerValue == ( int ) _bic;

        Return e.[GetType]().Equals(_type)
      End Function

      ''' <summary>
      ''' Allow all the reference to be selected
      ''' </summary>
      ''' <param name="reference">A candidate reference in selection operation.</param>
      ''' <param name="position">The 3D position of the mouse on the candidate reference.</param>
      ''' <returns>Return true to allow the user to select this candidate reference.</returns>
      Public Function AllowReference( _
          ByVal reference As Autodesk.Revit.DB.Reference, _
          ByVal position As Autodesk.Revit.DB.XYZ) As Boolean _
          Implements Autodesk.Revit.UI.Selection.ISelectionFilter.AllowReference

        Return True
      End Function

    End Class

    Public Shared Function GetSingleSelectedElementOrPrompt( _
        ByVal uidoc As UIDocument) As Element

      Dim e As Element = Nothing
      'Dim ss As ElementSet = uidoc.Selection.Elements ' 2014
      Dim ids As ICollection(Of ElementId) = uidoc.Selection.GetElementIds() ' 2015
      If 1 = ids.Count Then
        'Dim iter As ElementSetIterator = ss.ForwardIterator()
        'iter.MoveNext()
        'e = TryCast(iter.Current, Element) ' 2014
        e = uidoc.Document.GetElement(ids(0)) ' 2015
      Else
        Try

          Dim r As Reference = uidoc.Selection.PickObject( _
              ObjectType.Element, "Please pick an element")

          'e = r.Element ' 2011
          e = uidoc.Document.GetElement(r) ' 2012
        Catch ex As OperationCanceledException

        End Try
      End If
      Return e
    End Function

#End Region

#Region "Helpers to get specific element collections"

#Region "GetElementsOfType:"
    ''' <summary>
    ''' Return all elements of the requested class in the active document
    ''' matching the given built-in category.
    ''' </summary>
    Public Shared Function GetElementsOfType( _
        ByVal doc As Document, _
        ByVal type As Type, _
        ByVal bic As BuiltInCategory) _
        As FilteredElementCollector

      Dim collector As New FilteredElementCollector(doc)
      collector.OfCategory(bic)
      collector.OfClass(type)
      Return collector
    End Function
#End Region

#Region "GetFamilyInstances:"
    ''' <summary>
    ''' Retrieve all standard family instances for a given category.
    ''' </summary>
    Shared Function GetFamilyInstances( _
        ByVal doc As Document, _
        ByVal bic As BuiltInCategory) _
    As FilteredElementCollector

      ' For Revit 2009, this was
      'Dim elements As New System.Collections.Generic.List(Of Element)
      'Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(FamilyInstance))
      'Dim filterCategory As Filter = app.Create.Filter.NewCategoryFilter(bic)
      'Dim filterCombination As Filter = app.Create.Filter.NewLogicAndFilter(filterCategory, filterType)
      'Dim nRetVal As Integer = app.ActiveDocument.Elements(filterCombination, elements)
      'Return elements

      Return GetElementsOfType(doc, GetType(FamilyInstance), bic)
    End Function
#End Region

    ''' <summary>
    ''' Return all family symbols in the active document
    ''' matching the given built-in category.
    ''' </summary>
    Public Shared Function GetFamilySymbols( _
        ByVal doc As Document, _
        ByVal bic As BuiltInCategory) _
        As FilteredElementCollector

      Return GetElementsOfType(doc, GetType(FamilySymbol), bic)
    End Function

    ''' <summary>
    ''' Return the first family symbol found in the active document
    ''' matching the given built-in category or null if none is found.
    ''' </summary>
    Public Shared Function GetFirstFamilySymbol( _
        ByVal doc As Document, _
        ByVal bic As BuiltInCategory) _
        As FamilySymbol

      Dim s As FamilySymbol = TryCast(GetFamilySymbols(doc, bic).First(), FamilySymbol)

      Debug.Assert(s IsNot Nothing, String.Format("expected at least one {0} symbol in project", bic.ToString()))

      Return s
    End Function


    ''' <summary>
    ''' Determine bottom and top levels for creating walls.
    ''' In a default empty Revit Architecture project,
    ''' 'Level 1' and 'Level 2' will be returned.
    ''' </summary>
    ''' <returns>True is the two levels are successfully determined.</returns>
    Public Shared Function GetBottomAndTopLevels( _
        ByVal doc As Document, _
        ByRef levelBottom As Level, _
        ByRef levelTop As Level) _
        As Boolean

      Dim levels As FilteredElementCollector _
          = GetElementsOfType(doc, GetType(Level), BuiltInCategory.OST_Levels)

      For Each e As Element In levels
        If levelBottom Is Nothing Then
          levelBottom = TryCast(e, Level)
        ElseIf levelTop Is Nothing Then
          levelTop = TryCast(e, Level)
        Else
          Exit For
        End If
      Next
      If levelTop.Elevation < levelBottom.Elevation Then
        Dim tmp As Level = levelTop
        levelTop = levelBottom
        levelBottom = tmp
      End If
      Return levelBottom IsNot Nothing AndAlso levelTop IsNot Nothing
    End Function

    ''' <summary>
    ''' Helper to get all instances for a given category,
    ''' identified either by a built-in category or by a category name.
    ''' </summary>
    Public Shared Function GetTargetInstances( _
        ByVal doc As Document, _
        ByVal targetCategory As Object) _
        As List(Of Element)

      Dim elements As List(Of Element)

      Dim isName As Boolean = targetCategory.[GetType]().Equals(GetType(String))

      If isName Then
        Dim cat As Category = doc.Settings.Categories.Item(TryCast(targetCategory, String))
        Dim collector As New FilteredElementCollector(doc)
        collector.OfCategoryId(cat.Id)
        elements = New List(Of Element)(collector)
      Else
        Dim collector As FilteredElementCollector = New FilteredElementCollector(doc).WhereElementIsNotElementType()

        collector.OfCategory(DirectCast(targetCategory, BuiltInCategory))

        Dim model_elements = From e In collector Where (e.Category IsNot Nothing AndAlso e.Category.HasMaterialQuantities) _
            Select e

        elements = model_elements.ToList(Of Element)()
      End If
      Return elements
    End Function

    ''' <summary>
    ''' Return the one and only project information element
    ''' by searching for the "Project Information" category.
    ''' Only one such element exists.
    ''' </summary>
    Public Shared Function GetProjectInfoElem(ByVal doc As Document) As Element
      Dim collector As New FilteredElementCollector(doc)
      collector.OfCategory(BuiltInCategory.OST_ProjectInformation)
      Return collector.FirstElement()
    End Function

#End Region

#Region "Helpers for parameters"
    ''' <summary>
    ''' Helper to return parameter value as string.
    ''' One can also use param.AsValueString() to
    ''' get the user interface representation.
    ''' </summary>
    Public Shared Function GetParameterValue(ByVal param As Parameter) As String
      Dim s As String
      Select Case param.StorageType
        Case StorageType.[Double]
          '
          ' the internal database unit for all lengths is feet.
          ' for instance, if a given room perimeter is returned as
          ' 102.36 as a double and the display unit is millimeters,
          ' then the length will be displayed as
          ' peri = 102.36220472440
          ' peri * 12 * 25.4
          ' 31200 mm
          '
          's = param.AsValueString(); // value seen by user, in display units
          's = param.AsDouble().ToString(); // if not using not using LabUtils.RealString()
          s = RealString(param.AsDouble())
          ' raw database value in internal units, e.g. feet
          Exit Select

        Case StorageType.[Integer]
          s = param.AsInteger().ToString()
          Exit Select

        Case StorageType.[String]
          s = param.AsString()
          Exit Select

        Case StorageType.ElementId
          s = param.AsElementId().IntegerValue.ToString()
          Exit Select

        Case StorageType.None
          s = "?NONE?"
          Exit Select
        Case Else

          s = "?ELSE?"
          Exit Select
      End Select
      Return s
    End Function

    ''' <summary>
    ''' Helper to return parameter value as string, with additional
    ''' support for element id to display the element type referred to.
    ''' </summary>
    Public Shared Function GetParameterValue2(ByVal param As Parameter, ByVal doc As Document) As String
      Dim s As String
      If StorageType.ElementId = param.StorageType AndAlso doc IsNot Nothing Then
        Dim id As ElementId = param.AsElementId()
        Dim i As Integer = id.IntegerValue
        If 0 > i Then
          s = i.ToString()
        Else
          Dim e As Element = doc.GetElement(id)
          s = ElementDescription(e, True)
        End If
      Else
        s = GetParameterValue(param)
      End If
      Return s
    End Function

#End Region

#Region "Helpers for shared parameters"
    ''' <summary>
    ''' Helper to get shared parameters file.
    ''' </summary>
    Public Shared Function GetSharedParamsFile(ByVal app As Application) As DefinitionFile
      ' Get current shared params file name
      Dim sharedParamsFileName As String
      Try
        sharedParamsFileName = app.SharedParametersFilename
      Catch ex As Exception
        ErrorMsg("No shared params file set:" + ex.Message)
        Return Nothing
      End Try
      If 0 = sharedParamsFileName.Length Then
        Dim path As String = LabConstants.SharedParamFilePath
        Dim stream As StreamWriter
        stream = New StreamWriter(path)
        stream.Close()
        app.SharedParametersFilename = path
        sharedParamsFileName = app.SharedParametersFilename
      End If
      ' Get the current file object and return it
      Dim sharedParametersFile As DefinitionFile
      Try
        sharedParametersFile = app.OpenSharedParameterFile()
      Catch ex As Exception
        ErrorMsg("Cannnot open shared params file:" + ex.Message)
        sharedParametersFile = Nothing
      End Try
      Return sharedParametersFile
    End Function

    ''' <summary>
    ''' Helper to get shared params group.
    ''' </summary>
    Public Shared Function GetOrCreateSharedParamsGroup(ByVal sharedParametersFile As DefinitionFile, ByVal groupName As String) As DefinitionGroup
      Dim g As DefinitionGroup = sharedParametersFile.Groups.Item(groupName)
      If g Is Nothing Then
        Try
          g = sharedParametersFile.Groups.Create(groupName)
        Catch generatedExceptionName As Exception
          g = Nothing
        End Try
      End If
      Return g
    End Function

    ''' <summary>
    ''' Helper to get shared params definition.
    ''' </summary>
    Public Shared Function GetOrCreateSharedParamsDefinition(ByVal defGroup As DefinitionGroup, ByVal defType As ParameterType, ByVal defName As String, ByVal visible As Boolean) As Definition
      Dim definition As Definition = defGroup.Definitions.Item(defName)
      If definition Is Nothing Then
        Try
          'definition = defGroup.Definitions.Create(defName, defType, visible) ' 2014

          'Dim opt As ExternalDefinitonCreationOptions = New ExternalDefinitonCreationOptions(defName, defType) ' 2015

          Dim opt As ExternalDefinitionCreationOptions = New ExternalDefinitionCreationOptions(defName, defType) ' 2016

          opt.Visible = visible

          definition = defGroup.Definitions.Create(opt) ' 2015

        Catch generatedExceptionName As Exception
          definition = Nothing
        End Try
      End If
      Return definition
    End Function

    ''' <summary>
    ''' Get GUID for a given shared param name.
    ''' </summary>
    ''' <param name="app">Revit application</param>
    ''' <param name="defGroup">Definition group name</param>
    ''' <param name="defName">Definition name</param>
    ''' <returns>GUID</returns>
    Public Shared Function SharedParamGUID(ByVal app As Application, ByVal defGroup As String, ByVal defName As String) As Guid
      Dim guid__1 As Guid = Guid.Empty
      Try
        Dim file As DefinitionFile = app.OpenSharedParameterFile()
        Dim group As DefinitionGroup = file.Groups.Item(defGroup)
        Dim definition As Definition = group.Definitions.Item(defName)
        Dim externalDefinition As ExternalDefinition = TryCast(definition, ExternalDefinition)
        guid__1 = externalDefinition.GUID
      Catch generatedExceptionName As Exception
      End Try
      Return guid__1
    End Function
#End Region

  End Class

End Namespace
