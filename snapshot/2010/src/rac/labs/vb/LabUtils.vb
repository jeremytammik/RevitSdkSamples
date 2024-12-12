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

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports Autodesk.Revit
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Enums
Imports Geo = Autodesk.Revit.Geometry
Imports Autodesk.Revit.Parameters
Imports Autodesk.Revit.Symbols
Imports XYZ2 = Autodesk.Revit.Geometry.XYZ
Imports Microsoft.VisualBasic

Namespace Labs
    Public Class LabUtils

#Region "Geometry Utilities"

        ''' <summary>
        ''' Return the midpoint between two points.
        ''' </summary>
        Public Shared Function Midpoint(ByVal p As XYZ2, ByVal q As XYZ2) As XYZ2
            Return p + 0.5 * (q - p)
        End Function

#End Region

#Region "Helpers to get various specific element collections"

        '''' <summary>
        '''' Helper to get all geometrical elements.
        '''' </summary>
        'Shared Function GetAllModelElements(ByVal app As Application) As ElementSet

        '    Dim elems As ElementSet = app.Create.NewElementSet
        '    Dim opt As Geometry.Options = app.Create.NewGeometryOptions
        '    opt.DetailLevel = Geometry.Options.DetailLevels.Fine

        '    'Dim iter As IEnumerator = app.ActiveDocument.Elements ' this would also be sufficient
        '    Dim iter As ElementIterator = app.ActiveDocument.Elements
        '    Do While (iter.MoveNext())
        '        Dim elem As Element = iter.Current

        '        ' This single line would probably work if all system families were exposed as HostObjects, but they are not yet
        '        'If TypeOf elem Is FamilyInstance Or TypeOf elem Is HostObject Then
        '        If Not (TypeOf elem Is Symbol OrElse TypeOf elem Is FamilyBase) Then
        '            If Not (elem.Category Is Nothing) Then
        '                Dim geo As Geometry.Element = elem.Geometry(opt)
        '                If Not (geo Is Nothing) Then
        '                    elems.Insert(elem)
        '                End If
        '            End If
        '        End If
        '    Loop
        '    Return elems
        'End Function

        '''' <summary>
        '''' Helper to get all walls in Revit 2008.
        '''' </summary>
        'Shared Function GetAllWalls_2008(ByVal app As Application) As ElementSet
        '    Dim elems As ElementSet = app.Create.NewElementSet
        '    Dim iter As IEnumerator = app.ActiveDocument.Elements
        '    'Iterate through the elements in the active document
        '    Do While (iter.MoveNext())
        '        Dim elem As Element = iter.Current
        '        ' For Wall (one of the Host objects), there is a specific class!
        '        If TypeOf elem Is Wall Then
        '            elems.Insert(elem)
        '        End If
        '    Loop
        '    Return elems
        'End Function

        '''' <summary>
        '''' Helper to get all walls using Revit 2009.
        '''' </summary>
        'Shared Function GetAllWalls(ByVal app As Application) As List(Of Element)
        '    Dim elements As New List(Of Element)
        '    Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(Wall))
        '    Dim n As Integer = app.ActiveDocument.Elements(filterType, elements)
        '    Return elements
        'End Function

        ''' <summary>
        ''' Helper to get all standard family instances for a given category name using Revit 2008 API.
        ''' </summary>
        Shared Function GetAllStandardFamilyInstancesForACategoryName_2008( _
            ByVal app As Application, _
            ByVal catName As String) _
        As ElementSet

            Dim elems As ElementSet = app.Create.NewElementSet

            Dim iter As IEnumerator = app.ActiveDocument.Elements
            Do While (iter.MoveNext())
                Dim elem As Element = iter.Current
                ' First check for the class, then for specific category name
                If TypeOf elem Is FamilyInstance Then
                    Try
                        If elem.Category.Name.Equals(catName) Then
                            elems.Insert(elem)
                        End If
                    Catch
                    End Try
                End If
            Loop

            Return elems

        End Function

        ''' <summary>
        ''' Helper to get all standard family instances for a given category
        ''' name using the filter features provided by the Revit 2009 API.
        ''' </summary>
        Shared Function GetAllStandardFamilyInstancesForACategoryName( _
            ByVal app As Application, _
            ByVal catName As String) _
        As List(Of Element)

            Dim elements As New System.Collections.Generic.List(Of Element)
            Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(FamilyInstance))
            Dim cat As Category = app.ActiveDocument.Settings.Categories.Item(catName)
            Dim filterCategory As Filter = app.Create.Filter.NewCategoryFilter(cat)
            Dim filterCombination As Filter = app.Create.Filter.NewLogicAndFilter(filterCategory, filterType)
            Dim nRetVal As Integer
            Try
                nRetVal = app.ActiveDocument.Elements(filterCombination, elements)
            Catch ex As Exception

            End Try

            Return elements

        End Function

        ''' <summary>
        ''' Helper to get all standard family instances for a given category
        ''' using the filter features provided by the Revit 2009 API.
        ''' </summary>
        Shared Function GetAllStandardFamilyInstancesForACategory( _
            ByVal app As Application, _
            ByVal bic As BuiltInCategory) _
        As List(Of Element)

            Dim elements As New System.Collections.Generic.List(Of Element)
            Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(FamilyInstance))
            Dim filterCategory As Filter = app.Create.Filter.NewCategoryFilter(bic)
            Dim filterCombination As Filter = app.Create.Filter.NewLogicAndFilter(filterCategory, filterType)
            Dim nRetVal As Integer = app.ActiveDocument.Elements(filterCombination, elements)
            Return elements

        End Function

        ''' <summary>
        ''' Return all types of the requested class in the active document matching the given built-in category.
        ''' </summary>
        Public Shared Function GetAllTypes( _
            ByVal app As Application, _
            ByVal type As Type, _
            ByVal bic As BuiltInCategory) _
        As List(Of Element)
            Dim familySymbols As New List(Of Element)
            Dim filterCategory As Filter = app.Create.Filter.NewCategoryFilter(bic)
            Dim filterType As Filter = app.Create.Filter.NewTypeFilter(type)
            Dim filter As Filter = app.Create.Filter.NewLogicAndFilter(filterCategory, filterType)
            Dim rVal As Integer = app.ActiveDocument.Elements(filter, familySymbols)
            Return familySymbols
        End Function

        ''' <summary>
        ''' Return all family symbols in the active document matching the given built-in category.
        ''' </summary>
        Public Shared Function GetAllFamilySymbols( _
            ByVal app As Application, _
            ByVal bic As BuiltInCategory) _
        As List(Of Element)
            Return GetAllTypes(app, GetType(FamilySymbol), bic)
        End Function

        ''' <summary>
        ''' Helper to get all model instances for a given built-in category in Revit 2009.
        ''' </summary>
        Public Shared Function GetAllModelInstancesForACategory(ByVal app As Application, ByVal bic As BuiltInCategory) As List(Of Element)

            Dim elements As New List(Of Element)
            Dim filterCategory As Filter = app.Create.Filter.NewCategoryFilter(bic)
            Dim rVal As Integer = app.ActiveDocument.Elements(filterCategory, elements)
            Dim opt As Geo.Options = app.Create.NewGeometryOptions()
            Dim elements2 As New List(Of Element)
            Dim e As Element
            For Each e In elements
                If Not (TypeOf e Is Autodesk.Revit.Symbol) And Not (TypeOf e Is FamilyBase) And Not (e.Geometry(opt) Is Nothing) Then
                    elements2.Add(e)
                End If
            Next

            Return elements2

        End Function

        ''' <summary>
        ''' Helper to get specified type for specified family as FamilySymbol object
        ''' (in theory, we should also check for the correct *Category Name*).
        ''' (note: this only works with component family.)
        ''' </summary>
        Shared Function GetFamilySymbol_2008( _
            ByVal doc As Document, _
            ByVal familyName As String, _
            ByVal typeName As String) _
        As FamilySymbol

            Dim iter As ElementIterator = doc.Elements
            Do While (iter.MoveNext())
                Dim elem As Element = iter.Current

                ' We got a Family
                If TypeOf elem Is Family Then
                    Dim fam As Family = elem
                    ' If we have a match on family name, loop all its types for the other match
                    If fam.Name.Equals(familyName) Then
                        Dim sym As FamilySymbol
                        For Each sym In fam.Symbols
                            If sym.Name.Equals(typeName) Then
                                Return sym
                            End If
                        Next
                    End If
                End If
            Loop

            ' if here - haven't got it!
            Return Nothing

        End Function

        ''' <summary>
        ''' Helper to get specified type for specified family as FamilySymbol object
        ''' (in theory, we should also check for the correct *Category Name*).
        ''' (note: this only works with component family.)
        ''' </summary>
        Shared Function GetFamilySymbol( _
            ByVal app As Application, _
            ByVal familyName As String, _
            ByVal typeName As String) _
        As FamilySymbol

            Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(FamilySymbol))
            Dim filterFamilyName As Filter = app.Create.Filter.NewFamilyFilter(familyName)
            Dim filter As Filter = app.Create.Filter.NewLogicAndFilter(filterType, filterFamilyName)
            Dim elementList As New List(Of Element)
            Dim num As Integer = app.ActiveDocument.Elements(filter, elementList)
            ' we have a list of symbols for a given family. 
            ' loop through the list and find a match 
            Dim elem As Element
            For Each elem In elementList
                Dim sym As FamilySymbol = elem
                If sym.Name.Equals(typeName) Then
                    Return sym
                End If
            Next
            ' if here - haven't got it!
            Return Nothing
        End Function


        ''' <summary>
        ''' Determine bottom and top levels for creating walls.
        ''' In a default empty Revit Architecture 2009 project, 
        ''' 'Level 1' and 'Level 2' will be returned.
        ''' </summary>
        ''' <returns>True is the two levels are successfully determined</returns>
        Public Shared Function GetBottomAndTopLevels( _
            ByVal app As Application, ByRef levelBottom As Level, ByRef levelTop As Level _
            ) As Boolean
            Dim doc As Document = app.ActiveDocument
            Dim creApp As Autodesk.Revit.Creation.Application = app.Create
            Dim levels As New List(Of Element)

            Dim filterType As Filter = creApp.Filter.NewTypeFilter(GetType(Level))
            Dim nRetVal As Integer = doc.Elements(filterType, levels)
            Dim e As Element
            For Each e In levels
                If levelBottom Is Nothing Then
                    levelBottom = CType(e, Level)
                ElseIf levelTop Is Nothing Then
                    levelTop = CType(e, Level)
                Else
                    Exit For
                End If
            Next
            If levelTop.Elevation < levelBottom.Elevation Then
                Dim tmp As Level = levelTop
                levelTop = levelBottom
                levelBottom = tmp
            End If
            Return Not (levelBottom Is Nothing) And Not (levelTop Is Nothing)
        End Function

        ''' <summary>
        ''' Return the one and only project information element in Revit 2008.
        ''' </summary>
        Public Shared Function GetProjectInfoElem_2008(ByVal doc As Document) As Element

            '"Project Information" category
            Dim catProjInfo As Category = doc.Settings.Categories.Item(BuiltInCategory.OST_ProjectInformation) '

            ' Loop all elements
            Dim elem As Element
            Dim elemIter As ElementIterator = doc.Elements
            Do While elemIter.MoveNext
                elem = elemIter.Current
                ' Return the first match (it's singleton!)
                Try
                    If elem.Category.Id.Equals(catProjInfo.Id) Then
                        Return elem
                    End If
                Catch
                End Try
            Loop
            ' if here - couldn't find it !?
            Return Nothing

        End Function

        ''' <summary>
        ''' Return the one and only project information element using Revit 2009 filtering
        ''' by searching for the "Project Information" category. Only one such element exists.
        ''' </summary>
        Public Shared Function GetProjectInfoElem( _
            ByVal doc As Document, _
            ByRef app As Application) As Element

            Dim filterCategory As Filter = app.Create.Filter.NewCategoryFilter( _
              BuiltInCategory.OST_ProjectInformation)
            Dim elements As New List(Of Element)

            'it should return only one element in the collection.
            Dim nRetVal As Integer

            nRetVal = doc.Elements(filterCategory, elements)

            ' Loop all elements
            Dim elem As Element

            For Each elem In elements
                ' Return the first match (it's a singleton!)
                If (Not (elem Is Nothing)) Then
                    Return elem
                End If
            Next

            Return Nothing

        End Function

#End Region

#Region "Helper for Parameters"

        ''' <summary>
        ''' Helper to return parameter value as string.
        ''' One can also use param.AsValueString() to 
        ''' get the user interface representation.
        ''' </summary>
        Public Shared Function GetParameterValue(ByVal param As Parameter) As String

            Dim str As String
            Select Case param.StorageType
                Case StorageType.Double
                    str = param.AsDouble.ToString
                Case StorageType.Integer
                    str = param.AsInteger.ToString
                Case StorageType.String
                    str = param.AsString
                Case StorageType.ElementId
                    str = param.AsElementId.Value.ToString
                Case StorageType.None
                    str = "?NONE?"
                Case Else
                    str = "?ELSE?"
            End Select
            Return str

        End Function

        ''' <summary>
        ''' Helper to return parameter value as string, with additional 
        ''' support for element id to display the element type referred to.
        ''' </summary>
        Public Shared Function GetParameterValue2( _
            ByVal param As Parameter, _
            ByVal doc As Document) _
        As String
            Dim str As String
            If StorageType.ElementId = param.StorageType Then
                Dim id As ElementId = param.AsElementId
                Dim i As Integer = id.Value
                If (0 <= i) Then : str = String.Format("{0}: {1}", i, doc.Element(id).Name)
                Else : str = i.ToString
                End If
            Else
                str = GetParameterValue(param)
            End If
            Return str
        End Function

        ''' <summary>
        ''' Helper to get *specific* parameter by name.
        ''' No longer required in 2009, because the element provides
        ''' direct look-up access by name as well in Revit 2009.
        ''' </summary>
        Shared Function GetElemParam_2008(ByVal elem As Element, ByVal name As String) As Parameter

            Dim parameters As Autodesk.Revit.ParameterSet = elem.Parameters
            Dim parameter As Autodesk.Revit.Parameter
            For Each parameter In parameters
                If (parameter.Definition.Name = name) Then
                    Return parameter
                End If
            Next

            Return Nothing

        End Function

#End Region

#Region "Helpers for shared parameters"

        ''' <summary>
        ''' Helper to get shared parameters file.
        ''' </summary>
        Public Shared Function GetSharedParamsFile(ByVal app As Application) _
            As Parameters.DefinitionFile

            ' Get current shared params file name
            Dim sharedParamsFileName As String
            Try
                sharedParamsFileName = app.Options.SharedParametersFilename
            Catch
                MsgBox("No Shared params file set !?")
                Return Nothing
            End Try

            If "" = sharedParamsFileName Then
                Dim path As String = LabConstants.SharedParamFilePath

                Dim stream As StreamWriter
                stream = New StreamWriter(path)
                stream.Close()

                app.Options.SharedParametersFilename = path
                sharedParamsFileName = app.Options.SharedParametersFilename
            End If

            ' Get the current file object and return it
            Dim sharedParametersFile As Autodesk.Revit.Parameters.DefinitionFile
            Try
                sharedParametersFile = app.OpenSharedParameterFile
            Catch
                MsgBox("Cannnot open Shared Params file !?")
                sharedParametersFile = Nothing
            End Try
            Return sharedParametersFile

        End Function

        ''' <summary>
        ''' Helper to get shared params group.
        ''' </summary>
        Public Shared Function GetOrCreateSharedParamsGroup( _
             ByVal sharedParametersFile As Parameters.DefinitionFile, _
             ByVal groupName As String) _
             As Parameters.DefinitionGroup

            Dim msProjectGroup As Autodesk.Revit.Parameters.DefinitionGroup
            'Get Shared Parameter group
            msProjectGroup = sharedParametersFile.Groups.Item(groupName)
            If (msProjectGroup Is Nothing) Then
                Try
                    'create shared paramteter group
                    msProjectGroup = sharedParametersFile.Groups.Create(groupName)
                Catch
                    msProjectGroup = Nothing
                End Try
            End If

            Return msProjectGroup

        End Function

        ''' <summary>
        ''' Helper to get shared params definition.
        ''' </summary>
        Public Shared Function GetOrCreateSharedParamsDefinition( _
         ByVal defGroup As Parameters.DefinitionGroup, _
         ByVal defType As Parameters.ParameterType, _
         ByVal defName As String, _
         ByVal visible As Boolean) As Parameters.Definition

            'Get parameter definition
            Dim definition As Parameters.Definition = defGroup.Definitions.Item(defName)
            If definition Is Nothing Then
                Try
                    'create parameter definition
                    definition = defGroup.Definitions.Create(defName, defType, visible)
                Catch
                    definition = Nothing
                End Try
            End If

            Return definition

        End Function

        ''' <summary>
        ''' Get GUID for a given shared param name.
        ''' </summary>
        Shared Function SharedParamGUID(ByVal app As Application, _
                                        ByVal defGroup As String, _
                                        ByVal defName As String) As Guid

            Dim guid As Guid = guid.Empty

            Try
                Dim file As Autodesk.Revit.Parameters.DefinitionFile = app.OpenSharedParameterFile
                Dim group As Autodesk.Revit.Parameters.DefinitionGroup = file.Groups.Item(defGroup)
                Dim definition As Autodesk.Revit.Parameters.Definition = group.Definitions.Item(defName)
                Dim externalDefinition As Autodesk.Revit.Parameters.ExternalDefinition = definition
                guid = externalDefinition.GUID
            Catch
            End Try

            Return guid

        End Function

#End Region

#Region "Group helpers"

        ''' <summary>
        ''' Helper to get all groups in Revit 2008.
        ''' </summary>
        ''' <param name="app"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function GetAllGroups_2008(ByVal app As Application) As ElementSet

            Dim elems As ElementSet = app.Create.NewElementSet
            Dim iter As IEnumerator = app.ActiveDocument.Elements
            Do While (iter.MoveNext())
                Dim elem As Element = iter.Current
                If TypeOf elem Is Group Then
                    elems.Insert(elem)
                End If
            Loop
            Return elems

        End Function

        ''' <summary>
        ''' Helper to get all rooms using Revit 2009 filtering.
        ''' </summary>
        Shared Function GetAllRooms(ByVal app As Application) As List(Of Element)
            Dim elements As New List(Of Element)
            Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(Room))
            Dim iRetVal As Integer = app.ActiveDocument.Elements(filterType, elements)
            Return elements
        End Function

        ''' <summary>
        ''' Helper to get all groups using Revit 2009 filtering.
        ''' </summary>
        ''' <param name="app"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function GetAllGroups(ByVal app As Application) As List(Of Element)
            Dim elements As New List(Of Element)
            Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(Group))
            Dim iRetVal As Integer = app.ActiveDocument.Elements(filterType, elements)
            Return elements
        End Function

        ''' <summary>
        ''' Helper to get all group types in Revit 2008
        ''' </summary>
        ''' <param name="app"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function GetAllGroupTypes_2008(ByVal app As Application) As ElementSet

            Dim elems As ElementSet = app.Create.NewElementSet
            Dim iter As IEnumerator = app.ActiveDocument.Elements
            Do While (iter.MoveNext())
                Dim elem As Element = iter.Current
                If TypeOf elem Is GroupType Then
                    elems.Insert(elem)
                End If
            Loop
            Return elems

        End Function

        ''' <summary>
        ''' Helper to get all group types using Revit 2009 filtering
        ''' </summary>
        ''' <param name="app"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function GetAllGroupTypes(ByVal app As Application) As List(Of Element)
            Dim elements As New List(Of Element)
            Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(GroupType))
            Dim iRetVal As Integer = app.ActiveDocument.Elements(filterType, elements)
            Return elements
        End Function


        ''' <summary>
        ''' Helper to get all *model* group types in Revit 2008.
        ''' </summary>
        ''' <param name="app"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function GetAllModelGroupTypes_2008(ByVal app As Application) As ElementSet

            Dim elems As ElementSet = app.Create.NewElementSet
            Dim iter As IEnumerator = app.ActiveDocument.Elements
            Do While (iter.MoveNext())
                Dim elem As Element = iter.Current
                If TypeOf elem Is GroupType Then

                    ' Need additional check for the group type
                    Dim gt As GroupType = elem
                    Try
                        If gt.Parameter(Parameters.BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString.Equals(LabConstants.GroupTypeModel) Then
                            elems.Insert(elem)
                        End If
                    Catch
                    End Try

                End If
            Loop
            Return elems

        End Function


        ''' <summary>
        ''' Helper to get all *model* group types using Revit 2009 filtering.
        ''' </summary>
        ''' <param name="app"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function GetAllModelGroupTypes(ByVal app As Application) As List(Of Element)

            Dim elements As New List(Of Element)
            Dim filterType As Filter = app.Create.Filter.NewTypeFilter(GetType(GroupType))
            Dim filterParam = app.Create.Filter.NewParameterFilter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM, CriteriaFilterType.Equal, LabConstants.GroupTypeModel)
            Dim filter As Filter = app.Create.Filter.NewLogicAndFilter(filterType, filterParam)
            Dim n As Integer = app.ActiveDocument.Elements(filter, elements)
            Return elements

        End Function

#End Region

    End Class

End Namespace

