#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2006-2008 by Autodesk, Inc.
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

Namespace RstLabs
    ' Utils from standard RevitAPI class (some will be reused here)

    Public Class RacUtils

        ' Helper for backwards compatibility, should be removed
        ' pre-2009 versions used ElementSet for passing aroundd collections of elements,
        ' post-2009 tends towards generic collections.
        Shared Function ConvertElementListToSet(ByVal revitApp As Autodesk.Revit.Application, ByVal listElement As List(Of Revit.Element)) As ElementSet
            Dim elemSet As ElementSet = revitApp.Create.NewElementSet
            For Each elem As Revit.Element In listElement
                elemSet.Insert(elem)
            Next
            Return elemSet

        End Function

        ' Helper to get all geometrical elements
        Shared Function GetAllModelElements(ByVal revitApp As Revit.Application) As ElementSet

            Dim elems As ElementSet = revitApp.Create.NewElementSet

            Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            Do While (iter.MoveNext())
                Dim elem As Revit.Element = iter.Current

                ' This single line would probably work if all system families were exposed as HostObjects, but they are not yet
                'If TypeOf elem Is FamilyInstance Or TypeOf elem Is HostObject Then
                If Not (TypeOf elem Is Symbol Or TypeOf elem Is FamilyBase) Then
                    If Not (elem.Category Is Nothing) Then
                        Dim opt As Geometry.Options = revitApp.Create.NewGeometryOptions
                        opt.DetailLevel = Geometry.Options.DetailLevels.Medium
                        Dim geo As Geometry.Element = elem.Geometry(opt)
                        If Not (geo Is Nothing) Then
                            elems.Insert(elem)
                        End If
                    End If
                End If

            Loop

            Return elems

        End Function

        ' Helper to get all Walls
        Shared Function GetAllWalls(ByVal revitApp As Revit.Application) As ElementSet

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance. . It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim elems As ElementSet = revitApp.Create.NewElementSet

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    ' For Wall (one of the Host objects), there is a specific class!
            '    If TypeOf elem Is Wall Then
            '        elems.Insert(elem)
            '    End If
            'Loop

            'Return elems

            '
            'The following code is using the new feature of Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim listWalls As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(Wall))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listWalls)
            Return ConvertElementListToSet(revitApp, listWalls)

        End Function

        ' Helper to get all Standard Family Instances for a given Category
        Shared Function GetAllStandardFamilyInstancesForACategory( _
            ByVal revitApp As Revit.Application, _
            ByVal catName As String _
        ) As ElementSet

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance. . It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim elems As ElementSet = revitApp.Create.NewElementSet
            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    ' First check for the class, then for specific category name
            '    If TypeOf elem Is FamilyInstance Then
            '        Try
            '            If elem.Category.Name.Equals(catName) Then
            '                elems.Insert(elem)
            '            End If
            '        Catch
            '        End Try
            '    End If
            'Loop
            'Return elems

            '
            'The following code is using the new feature of Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim cat As Category = revitApp.ActiveDocument.Settings.Categories.Item(catName)
            Dim filterCategory As Revit.Filter = revitApp.Create.Filter.NewCategoryFilter(cat)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(FamilyInstance))
            Dim filterAnd As Revit.Filter = revitApp.Create.Filter.NewLogicAndFilter(filterCategory, filterType)
            Dim listFamilyInstances As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)

            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterAnd, listFamilyInstances)
            Return ConvertElementListToSet(revitApp, listFamilyInstances)

        End Function

        'Helper to get specified Type for specified Family as FamilySymbol object
        '   (in theory, we should also check for the correct *Category Name*)
        Shared Function GetFamilySymbol(ByVal revitApp As Autodesk.Revit.Application, ByVal doc As Revit.Document, ByVal familyName As String, ByVal typeName As String) As FamilySymbol

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance. . It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim iter As ElementIterator = doc.Elements
            'Do While (iter.MoveNext())
            '   Dim elem As Revit.Element = iter.Current

            '
            'The following code is using the new feature of Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim listFamilies As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(Family))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listFamilies)
            For Each elem As Revit.Element In listFamilies
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

            Next

            ' if here - haven't got it!
            Return Nothing

        End Function

        'Helper to return Parameter as string
        Public Shared Function GetParamAsString(ByVal param As Parameter) As String

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

        'Helper to get *specific* parameter by name
        Shared Function GetElemParam(ByVal elem As Revit.Element, ByVal name As String) As Parameter
            ' The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too.
            ' Revit 2009 API provide a overload method to get the Parameter via the parameter name string. It is much faster.
            'Dim parameters As Autodesk.Revit.ParameterSet = elem.Parameters
            'Dim parameter As Autodesk.Revit.Parameter
            'For Each parameter In parameters
            '    If (parameter.Definition.Name = name) Then
            '        Return parameter
            '    End If
            'Next
            'Return Nothing

            ' We use the new added overload method to retrieve the Parameter via the name string. This only works in Revit 2009 onward.
            Dim para As Parameter = elem.Parameter(name)
            Return para



        End Function

        ' Helpers for the Shared Parameters:
        '-------------------------------------
        ' Shared Params FILE
        Public Shared Function GetSharedParamsFile(ByVal revitApp As Revit.Application) As Parameters.DefinitionFile

            ' Get current shared params file name
            Dim sharedParamsFileName As String
            Try
                sharedParamsFileName = revitApp.Options.SharedParametersFilename
            Catch
                MsgBox("No Shared params file set !?")
                Return Nothing
            End Try

            ' Get the current file object and return it
            Dim sharedParametersFile As Autodesk.Revit.Parameters.DefinitionFile
            Try
                sharedParametersFile = revitApp.OpenSharedParameterFile
            Catch
                MsgBox("Cannnot open Shared Params file !?")
                sharedParametersFile = Nothing
            End Try
            Return sharedParametersFile

        End Function

        ' Shared Params GROUP
        Public Shared Function GetOrCreateSharedParamsGroup( _
           ByVal sharedParametersFile As Parameters.DefinitionFile, _
           ByVal groupName As String) _
           As Parameters.DefinitionGroup

            Dim msProjectGroup As Autodesk.Revit.Parameters.DefinitionGroup
            msProjectGroup = sharedParametersFile.Groups.Item(groupName)
            If (msProjectGroup Is Nothing) Then
                Try
                    msProjectGroup = sharedParametersFile.Groups.Create(groupName)
                Catch
                    msProjectGroup = Nothing
                End Try
            End If

            Return msProjectGroup

        End Function

        ' Shared Params DEFINITION
        Public Shared Function GetOrCreateSharedParamsDefinition( _
         ByVal defGroup As Parameters.DefinitionGroup, _
         ByVal defType As Parameters.ParameterType, _
         ByVal defName As String, _
         ByVal visible As Boolean) As Parameters.Definition

            Dim definition As Parameters.Definition = defGroup.Definitions.Item(defName)
            If definition Is Nothing Then
                Try
                    ' NOTE: Although Integer type, the 3rd arg must be Boolean "False" to make it invisible in UI!?
                    definition = defGroup.Definitions.Create(defName, defType, visible)
                Catch
                    definition = Nothing
                End Try
            End If

            Return definition

        End Function

        ' Get GUID for a given shared param name
        Shared Function SharedParamGUID(ByVal revitApp As Revit.Application, ByVal defGroup As String, ByVal defName As String) As Guid

            Dim guid As Guid = guid.Empty
            Try
                Dim file As Autodesk.Revit.Parameters.DefinitionFile = revitApp.OpenSharedParameterFile
                Dim group As Autodesk.Revit.Parameters.DefinitionGroup = file.Groups.Item(defGroup)
                Dim definition As Autodesk.Revit.Parameters.Definition = group.Definitions.Item(defName)
                Dim externalDefinition As Autodesk.Revit.Parameters.ExternalDefinition = definition
                guid = externalDefinition.GUID
            Catch
            End Try

            Return guid

        End Function

        ' Helper to get all Groups
        Shared Function GetAllGroups(ByVal revitApp As Revit.Application) As ElementSet
            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance. . It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim elems As ElementSet = revitApp.Create.NewElementSet
            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    If TypeOf elem Is Group Then
            '        elems.Insert(elem)
            '    End If
            'Loop
            'Return elems

            '
            'The following code is using the new feature of Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim listGroups As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(Group))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listGroups)
            Return ConvertElementListToSet(revitApp, listGroups)


        End Function

        ' Helper to get all Group Types
        Shared Function GetAllGroupTypes(ByVal revitApp As Revit.Application) As ElementSet
            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance. . It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim elems As ElementSet = revitApp.Create.NewElementSet
            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current
            '    If TypeOf elem Is GroupType Then
            '        elems.Insert(elem)
            '    End If
            'Loop
            'Return elems

            '
            'The following code is using the new feature of Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim listGroupTypes As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(GroupType))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listGroupTypes)
            Return ConvertElementListToSet(revitApp, listGroupTypes)

        End Function

        ' Helper to get all *Model* Group Types
        Shared Function GetAllModelGroupTypes(ByVal revitApp As Revit.Application) As ElementSet
            Dim elems As ElementSet = revitApp.Create.NewElementSet

            'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance. . It works in Revit 2009 and afterward version too. However this method has a low performance.
            'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.

            'Dim iter As IEnumerator = revitApp.ActiveDocument.Elements
            'Do While (iter.MoveNext())
            '    Dim elem As Revit.Element = iter.Current

            '
            'The following code is using the new feature of Element filter to improve the performance. It works in Revit 2009 and afterward version
            '
            Dim listGroupTypes As List(Of Autodesk.Revit.Element) = New List(Of Revit.Element)
            Dim filterType As Revit.Filter = revitApp.Create.Filter.NewTypeFilter(GetType(GroupType))
            Dim bRetVal As Boolean = revitApp.ActiveDocument.Elements(filterType, listGroupTypes)
            For Each elem As Revit.Element In listGroupTypes
                If TypeOf elem Is GroupType Then
                    ' Need additional check for the group type
                    Dim gt As GroupType = elem
                    Try
                        'If gt.Parameter(Parameters.BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString.Equals(gsGroupTypeModel) Then
                        If gt.Parameter(Parameters.BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString.Equals("Model Group") Then
                            elems.Insert(elem)
                        End If
                    Catch
                    End Try
                End If
            Next
            Return elems

        End Function

    End Class



End Namespace