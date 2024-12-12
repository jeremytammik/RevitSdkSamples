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

Imports System.Windows.Forms
Imports Autodesk.Revit.Structural
Imports Application = Autodesk.Revit.Application

Namespace Labs

#Region "Lab3_1_StandardFamiliesAndTypes"
    ''' <summary>
    ''' List all loaded families and types in Revit 2008.
    ''' </summary>
    Public Class Lab3_1_StandardFamiliesAndTypes_2008
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute

            ' Iterate through all elements and look for Family classes
            Dim doc As Document = commandData.Application.ActiveDocument
            Dim sMsg As String = "Standard Families already loaded in this model are:"
            Dim elem As Element
            Dim iter As ElementIterator = doc.Elements
            Do While (iter.MoveNext)
                elem = iter.Current
                ' Get Family
                If TypeOf elem Is Family Then
                    Dim fam As Family = elem
                    ' Try to get its Category name (as we will see later from the message box,
                    '   Category property is NOT implemented for the Family class!)
                    Dim famCatName As String
                    If (fam.Category Is Nothing) Then
                        famCatName = "?"
                    Else
                        famCatName = fam.Category.Name
                    End If
                    sMsg += vbCrLf & "  Name=" & fam.Name & "; Category=" & famCatName
                End If
            Loop
            MsgBox(sMsg)

            ' Let's do a similar Loop, but now get all the child Symbols (Types) as well.
            ' These Symbols can also be used to determine the category!
            iter = doc.Elements
            Do While (iter.MoveNext)
                elem = iter.Current
                ' Get Family (we know that Category is not implemented for this class)
                If TypeOf elem Is Family Then
                    Dim fam As Family = elem
                    Dim catName As String
                    Dim first As Boolean = True
                    Dim symb As FamilySymbol
                    'Loop all contained symbols (types)
                    For Each symb In fam.Symbols
                        ' Determine the category via first symbol
                        If first Then
                            first = False
                            If (symb.Category Is Nothing) Then
                                catName = "?"  ' Still happens for *some* Symbols (Profiles?)
                            Else
                                catName = symb.Category.Name
                            End If

                            sMsg = "Family: Name=" & fam.Name & "; Id=" & fam.Id.Value.ToString & "; Category=" & catName & vbCrLf & "Contains Types:"
                        End If
                        sMsg += vbCrLf & "    " & symb.Name & "; Id=" & symb.Id.Value.ToString
                    Next

                    ' Show the symbols for this family and allow user to procede to the next family (OK) or cancel (Cancel)
                    If MsgBox(sMsg, MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                        Exit Do
                    End If

                End If
            Loop

            Return IExternalCommand.Result.Succeeded

        End Function
    End Class

    ''' <summary>
    ''' List all loaded families and types using Revit 2009 filtering.
    ''' </summary>
    Public Class Lab3_1_StandardFamiliesAndTypes
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute

            ' Element iteration done with element filtering functionality in Revit 2009
            Dim doc As Revit.Document = commandData.Application.ActiveDocument
            Dim elementList As New List(Of Revit.Element)
            Dim filterType As Revit.Filter = commandData.Application.Create.Filter.NewTypeFilter(GetType(Family))
            Dim nRetVal As Integer = doc.Elements(filterType, elementList)
            Dim sMsg As String = "Standard Families already loaded in this model are:"
            Dim f As Family
            For Each f In elementList
                ' get its category name; notice that the category property is
                ' not implemented for the Family class. use FamilyCategory 
                ' instead, which is also not always implemented:

                Dim catName As String
                If f.Category Is Nothing Then
                    catName = "?"
                Else
                    catName = f.Category.Name
                End If

                Dim famCatName As String
                If f.FamilyCategory Is Nothing Then
                    famCatName = "?"
                Else
                    famCatName = f.FamilyCategory.Name
                End If

                sMsg += vbCrLf & "  Name=" & f.Name _
                    & "; Category=" & catName _
                    & "; FamilyCategory=" & famCatName
            Next
            MsgBox(sMsg)

            ' Let's do a similar loop, but now get all the child symbols (types) as well.
            ' These symbols can also be used to determine the category:

            For Each f In elementList
                Dim catName As String
                Dim first As Boolean = True
                Dim symb As FamilySymbol
                'Loop all contained symbols (types)
                For Each symb In f.Symbols
                    ' Determine the category via first symbol
                    If first Then
                        first = False
                        If (symb.Category Is Nothing) Then
                            catName = "?"  ' Still happens for *some* Symbols (Profiles?)
                        Else
                            catName = symb.Category.Name
                        End If

                        sMsg = "Family: Name=" & f.Name & "; Id=" & f.Id.Value.ToString & "; Category=" & catName & vbCrLf & "Contains Types:"
                    End If
                    sMsg += vbCrLf & "    " & symb.Name & "; Id=" & symb.Id.Value.ToString
                Next

                ' Show the symbols for this family and allow user to procede to the next family (OK) or cancel (Cancel)
                If MsgBox(sMsg, MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                    Exit For
                End If
            Next
            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab3_2_LoadStandardFamilies"
    ''' <summary>
    ''' Load an entire family or a specific type from a family.
    ''' </summary>
    Public Class Lab3_2_LoadStandardFamilies
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim doc As Revit.Document = commandData.Application.ActiveDocument

            'Load a whole Family

            ' example for a family WITH TXT file
            If Not CType(doc.LoadFamily(gsWholeFamilyFileToLoad1), Boolean) Then
                MsgBox("ERROR in loading Family " & gsWholeFamilyFileToLoad1 & "?")
            Else
                MsgBox("Successfully loaded Family " & gsWholeFamilyFileToLoad1 & "!")
            End If

            ' example for a family WITHOUT TXT file 
            If Not CType(doc.LoadFamily(gsWholeFamilyFileToLoad2), Boolean) Then
                MsgBox("ERROR in loading Family " & gsWholeFamilyFileToLoad2 & "?")
            Else
                MsgBox("Successfully loaded Family " & gsWholeFamilyFileToLoad2 & "!")
            End If

            'Load only a specific Symbol (Type)
            ' The symbol MUST exist in the corresponding catalog (TXT) file - same as in the UI
            If Not CType(doc.LoadFamilySymbol(gsFamilyFileToLoadSingleSymbol, gsSymbolName), Boolean) Then
                MsgBox("ERROR in loading FamilySymbol " & gsFamilyFileToLoadSingleSymbol & " : " & gsSymbolName & "?")
            Else
                MsgBox("Successfully loaded FamilySymbol " & gsFamilyFileToLoadSingleSymbol & " : " & gsSymbolName & "!")
            End If

            Return Revit.IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab3_3_DetermineInstanceTypeAndFamily"
    ''' <summary>
    ''' For a selected family instance in the model, determine its type and family in Revit 2008.
    ''' </summary>
    Public Class Lab3_3_DetermineInstanceTypeAndFamily_2008
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim doc As Revit.Document = commandData.Application.ActiveDocument

            ' First we loop the model to report all FamilySymbol objects of "Windows" category.
            ' Note that some of them will be reported TWICE! More about this later...
            Dim catWindows As Category = doc.Settings.Categories.Item(BuiltInCategory.OST_Windows)

            Dim sMsg As String = " Windows Family Symbols in the model are:"
            Dim elem As Revit.Element
            Dim iter As ElementIterator = doc.Elements

            Do While (iter.MoveNext)
                elem = iter.Current
                ' Check for FamilySymbol having Windows category
                If TypeOf elem Is FamilySymbol Then
                    Dim symb As FamilySymbol = elem
                    Try
                        Dim catFS As Category = symb.Category ' for "Profiles" it fails
                        If Not catFS Is Nothing Then
                            ' Either of these comparisons will do:
                            'If catFS.Id.Value.Equals(catWindows.Id.Value) Then
                            'If catFS.Id.Equals(catWindows.Id) Then
                            'If catFS.Name.Equals(catWindows.Name) Then
                            ' Do not use this, it is unreliable:
                            'If catFS.Equals(catWindows) Then
                            If catFS.Id.Equals(catWindows.Id) Then
                                sMsg += vbCrLf & "    " & symb.Name & ", Id=" & symb.Id.Value.ToString
                                Try
                                    Dim fam As Family = symb.Family
                                    sMsg += "; Family name=" & fam.Name & ", Family Id=" & fam.Id.Value.ToString
                                Catch
                                End Try
                            End If
                        End If
                    Catch
                    End Try
                End If
            Loop
            MsgBox(sMsg)

            ' Now loop the selection set and check for standard Family Instances of "Windows" category
            For Each elem In doc.Selection.Elements
                If TypeOf elem Is FamilyInstance Then
                    Dim inst As FamilyInstance = elem
                    Dim catInst As Category = Nothing
                    If (inst.Category Is Nothing) Then
                        catInst = Nothing
                    Else
                        catInst = inst.Category
                    End If

                    If (Not catInst Is Nothing) AndAlso catInst.Id.Equals(catWindows.Id) Then

                        sMsg = "Selected Window Id=" & elem.Id.Value.ToString & vbCrLf
                        Dim fs1 As FamilySymbol = inst.Symbol
                        sMsg += "  FamilySymbol = " & fs1.Name & "; Id=" & fs1.Id.Value.ToString & vbCrLf
                        Dim f1 As Family = fs1.Family
                        sMsg += "  Family = " & f1.Name & "; Id=" & f1.Id.Value.ToString
                        ' Report each Window data
                        MsgBox(sMsg)
                    End If
                End If
            Next

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class


    ''' <summary>
    ''' For a selected family instance in the model, determine 
    ''' its type and family using Revit 2009 filtering.
    ''' </summary>
    Public Class Lab3_3_DetermineInstanceTypeAndFamily
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Autodesk.Revit.Application = commandData.Application
            Dim doc As Revit.Document = commandData.Application.ActiveDocument

            ' Use the filtering mechanism to report all FamilySymbol objects of "Windows" category.
            Dim sMsg As String = " Windows Family Symbols in the model are:"
            Dim bic As BuiltInCategory = BuiltInCategory.OST_Windows
            Dim familySymbols As List(Of Element) = LabUtils.GetAllFamilySymbols(app, bic)
            Dim elem As Element
            For Each elem In familySymbols
                Dim symb As FamilySymbol = elem
                sMsg += vbCrLf & "  " & symb.Name & ", Id=" & symb.Id.Value.ToString
                Dim fam As Family = symb.Family
                sMsg += "; Family name=" & fam.Name & ", Family Id=" & fam.Id.Value.ToString
            Next
            MsgBox(sMsg)

            ' Now loop the selection set and check for standard 
            ' family instances of "Windows" category
            Dim categories As Categories = doc.Settings.Categories
            Dim catWindows As Category = categories.Item(bic)
            For Each elem In doc.Selection.Elements
                If TypeOf elem Is FamilyInstance Then
                    Dim inst As FamilyInstance = elem
                    Dim catInst As Category = Nothing
                    If (inst.Category Is Nothing) Then
                        catInst = Nothing
                    Else
                        catInst = inst.Category
                    End If
                    ' Check if the element category id is same as Windows category id
                    If (Not catInst Is Nothing) AndAlso catInst.Id.Equals(catWindows.Id) Then

                        sMsg = "Selected Window Id=" & elem.Id.Value.ToString & vbCrLf
                        Dim fs1 As FamilySymbol = inst.Symbol
                        sMsg += "  FamilySymbol = " & fs1.Name & "; Id=" & fs1.Id.Value.ToString & vbCrLf
                        Dim f1 As Family = fs1.Family
                        sMsg += "  Family = " & f1.Name & "; Id=" & f1.Id.Value.ToString
                        ' Report each Window data
                        MsgBox(sMsg)
                    End If
                End If
            Next

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab3_4_ChangeSelectedInstanceType"
    ''' <summary>
    ''' Form-based utility to change the type or symbol of a selected standard instance.
    ''' </summary>
    Public Class Lab3_4_ChangeSelectedInstanceType
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Revit.Application = commandData.Application
            Dim doc As Revit.Document = app.ActiveDocument

            Dim inst As FamilyInstance
            Dim instCat As Category

            Dim ss As ElementSet = doc.Selection.Elements

            ' First make sure we have a single FamilyInstance selected
            If Not ss.Size = 1 Then
                MsgBox("You must pre-select a single element!")
                Return IExternalCommand.Result.Cancelled
            Else
                Dim itTmp As ElementSetIterator = ss.ForwardIterator
                itTmp.MoveNext()
                Dim elTmp As Revit.Element = itTmp.Current
                If Not TypeOf elTmp Is FamilyInstance Then
                    MsgBox("Selected element is NOT a standard family instance!")
                    Return IExternalCommand.Result.Cancelled
                Else
                    inst = elTmp
                    instCat = inst.Category
                End If
            End If



            ' Collect all types applicable to this category and sort them into
            ' a dictionary mapping the family name to a list of its types.
            '
            ' We create a collection of all loaded families for this category
            ' and for each one, the list of all loaded types (symbols).
            '
            ' There are many ways how to store the matching objects, but we choose whatever is most suitable for the relevant UI:
            '   We could use Revit's generic Map class, but it's probably more efficient to use the new 2005 .NET strongly-typed Dictionary with
            '   KEY = Family name (String)
            '   VALUE = ArrayList (implements iList so we can elegantly bind it to combobox) of corresponding FamilySymbol obects
            Dim dictFamilyToSymbols As New Dictionary(Of String, ArrayList)

            ' Looping may take a few seconds, so let user know by changing the cursor
            Dim oldCursor As Cursor = Cursor.Current
            Cursor.Current = Cursors.WaitCursor

            ' using Revit 2009 element filtering
            Dim families As List(Of Revit.Element) = New List(Of Revit.Element)
            Dim filterFamily As Revit.Filter = commandData.Application.Create.Filter.NewTypeFilter(GetType(Family))

            Dim nRetVal = doc.Elements(filterFamily, families)
            Dim f As Family
            Dim categoryMatches = False
            For Each f In families
                categoryMatches = False
                If (f.FamilyCategory Is Nothing) Then
                    For Each sym As FamilySymbol In f.Symbols
                        categoryMatches = sym.Category.Id.Equals(instCat.Id)
                        Exit For
                    Next
                Else
                    categoryMatches = f.FamilyCategory.Id.Equals(instCat.Id)
                End If

                If (categoryMatches) Then
                    Dim familySymbols As New ArrayList
                    For Each sym As FamilySymbol In f.Symbols
                        familySymbols.Add(sym)
                    Next
                    dictFamilyToSymbols.Add(f.Name, familySymbols)
                End If

            Next

            ' Display the form, allowing the user to select a family
            ' and a type, and assign this type to the instance.
            Dim frm As New Lab3_4_Form(dictFamilyToSymbols)
            Cursor.Current = oldCursor ' restore cursor
            If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
                inst.Symbol = frm.cmbType.SelectedItem
                MsgBox("Successfully changed Family:Type to " & frm.cmbFamily.Text & " : " & frm.cmbType.Text)
            End If

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab3_5_WallAndFloorTypes"
    ''' <summary>
    ''' List all wall and floor types, and change the type of selected walls and floors.
    ''' </summary>
    Public Class Lab3_5_WallAndFloorTypes
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result Implements IExternalCommand.Execute

            Dim app As Revit.Application = commandData.Application
            Dim doc As Document = app.ActiveDocument

            ' Find ALL Wall Types and their System Families (or Kinds)
            Dim newWallType As WallType = Nothing ' store the last one to use to change the wall type later
            Dim sMsg As String = "ALL Wall Types/Families in the model:"

            ' We could iterate all elements and check for WallType class, 
            ' but it is simpler to directly access from the doc
            For Each wt As WallType In doc.WallTypes
                sMsg += vbCrLf & "  Type=" & wt.Name & " Family(or Kind)=" & wt.Kind.ToString
                newWallType = wt
            Next
            MsgBox(sMsg)
            MsgBox("Stored WallType " & newWallType.Name & " (Id=" & newWallType.Id.Value.ToString & ") for later use")

            ' Find all floor types. Since Revit 2008, the Document class has a 
            ' FloorTypes property to retrieve the collection of all floor types.
            ' We could simply use doc.FloorTypes like this:
            '
            'For Each ft As FloorType In doc.FloorTypes
            '    sMsg += vbCrLf & "  Type=" & ft.Name & ", Id=" & ft.Id.Value.ToString
            '    ' 9.0 onwards, it looks like "Foundation Slab" system family from "Structural Foundations" category
            '    ' also contains FloorType class instances !? Be careful to exclude those as choices for standard floor types
            '    Dim famName As String
            '    Try
            '        famName = ft.Parameter(Parameters.BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString
            '    Catch
            '        famName = "?"
            '    End Try
            '    Dim cat As Category = ft.Category
            '    sMsg += ", Family=" & famName & ", Category=" & cat.Name
            '    ' store only if proper Floors category
            '    If (doc.Settings.Categories.Item(BuiltInCategory.OST_Floors).Id.Equals(cat.Id)) Then
            '        newFloorType = ft
            '    End If
            'Next
            '
            ' Unfortunately, FloorTypes
            ' includes the structural foundation slabs, too.  One way to obtain 
            ' only floor types directly is to use the category and explicitly 
            ' request type of FloorType.
            '
            Dim newFloorType As FloorType = Nothing ' store the last one to use to change the floor type later
            sMsg = "ALL FLOOR Types in the model:"

            ' define a filter to get the family 
            Dim filterFloorType As Filter = app.Create.Filter.NewTypeFilter(GetType(FloorType))
            Dim filterFloorCategory As Filter = app.Create.Filter.NewCategoryFilter(BuiltInCategory.OST_Floors)

            ' make a compund filter using logical or/and 
            Dim filterFloorTypeFloorCategory As Filter = app.Create.Filter.NewLogicAndFilter(filterFloorType, filterFloorCategory)

            ' get a list using the filter 
            Dim elementList As New List(Of Revit.Element)
            Dim num As Integer = doc.Elements(filterFloorTypeFloorCategory, elementList)

            ' go over the list 
            Dim elem As Revit.Element
            For Each elem In elementList

                Dim ft As Symbols.FloorType = elem
                sMsg += vbCrLf & "  Type=" & ft.Name & ", Id=" & ft.Id.Value.ToString
                Dim p As Parameter = ft.Parameter(Parameters.BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM)
                Dim famName As String = "?"
                If Not p Is Nothing Then
                    famName = p.AsString
                End If
                Dim cat As Category = ft.Category
                sMsg += ", Family=" & famName & ", Category=" & cat.Name
                ' all elements have proper Floors category
                newFloorType = ft

            Next

            MsgBox(sMsg)
            MsgBox("Stored FloorType " & newFloorType.Name & " (Id=" & newFloorType.Id.Value.ToString & ") for later use")

            ' Change the Type for selected Walls and Floors
            Dim sel As ElementSet = doc.Selection.Elements
            Dim iWall As Integer
            Dim iFloor As Integer
            Dim el As Revit.Element

            'Loop through all selection elements
            If sel.Size > 0 Then
                For Each el In sel

                    If TypeOf el Is Wall Then 'Check for walls
                        Dim wall As Wall = el
                        iWall += 1
                        Dim oldWallType As WallType = wall.WallType
                        'change wall type and report the old/new values
                        wall.WallType = newWallType
                        MsgBox("Wall " & iWall.ToString & ": Id=" & wall.Id.Value.ToString & vbCrLf & _
                         "  changed from OldType=" & oldWallType.Name & "; Id=" & oldWallType.Id.Value.ToString & _
                         "  to NewType=" & wall.WallType.Name & "; Id=" & wall.WallType.Id.Value.ToString)

                    ElseIf TypeOf el Is Floor Then
                        iFloor += 1
                        Dim f As Floor = el

                        ' NOTE: Floor.FloorType was *Read-only* before 2008, so we could not use it like for the walls above
                        ' However we CAN change FloorType via ELEM_TYPE_PARAM param - WORKS FINE in 8.1!
                        ' But FAILS in 9.0 !? SYMBOL_ID_PARAM also fails :-(
                        ' ELEM_TYPE_PARAM param WORKS FINE in 2008 as well
                        ' In 2008, we can use code similar to the Wall code, so we skip this:
                        GoTo UseFloorType
                        Try
                            sMsg = "FloorType gotten via ELEM_TYPE_PARAM ="
                            'sMsg = "FloorType gotten via SYMBOL_ID_PARAM ="
                            Dim ft As FloorType = doc.Element(f.Parameter(Parameters.BuiltInParameter.ELEM_TYPE_PARAM).AsElementId) 'FAILS in 9.0 !?
                            'ft = doc.Element(f.Parameter(Parameters.BuiltInParameter.SYMBOL_ID_PARAM).AsElementId)
                            sMsg += ft.Name
                            '... SET as well !
                            f.Parameter(BuiltInParameter.ELEM_TYPE_PARAM).Set(newFloorType.Id)
                            sMsg += vbCrLf & "Changed to: " & f.FloorType.Name
                        Catch
                            sMsg += vbCrLf & "FAILED to change it! "
                        End Try
                        MsgBox(sMsg)

UseFloorType:
                        ' simple code for 2008 onwards:
                        Dim oldFloorType As FloorType = f.FloorType
                        f.FloorType = newFloorType
                        MsgBox("Floor " & iFloor.ToString & ": Id=" & f.Id.Value.ToString & vbCrLf & _
                         "  changed from OldType=" & oldFloorType.Name & "; Id=" & oldFloorType.Id.Value.ToString & _
                         "  to NewType=" & f.FloorType.Name & "; Id=" & f.FloorType.Id.Value.ToString)
                    End If
                Next
            Else
                MsgBox("There are no elements in selection set!")
            End If


            Return IExternalCommand.Result.Succeeded
        End Function
    End Class

#End Region

#Region "Lab3_6_DuplicateWallType"
    ''' <summary>
    ''' Create a new family symbol or type by calling Duplicate() on an existing one and then modifying its parameters.
    ''' </summary>
    Public Class Lab3_6_DuplicateWallType
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
        As IExternalCommand.Result _
        Implements IExternalCommand.Execute

            Try
                Dim app As Application = commandData.Application
                Dim doc As Document = app.ActiveDocument
                Dim els As ElementSet = doc.Selection.Elements
                Dim e As Element
                Dim newWallTypeName As String _
                    = "NewWallType_with_Width_doubled"

                For Each e In els
                    If TypeOf e Is Wall Then
                        Dim wall As Wall = e

                        Dim wallType As WallType
                        wallType = wall.WallType

                        Dim newWallType As WallType
                        newWallType = wallType.Duplicate(newWallTypeName)

                        Dim layers As CompoundStructureLayerArray
                        layers = newWallType.CompoundStructure.Layers

                        Dim layer As CompoundStructureLayer
                        For Each layer In layers
                            layer.Thickness *= 2 ' double each layer's thickness
                        Next

                        wall.WallType = newWallType ' assign the new wall type back to the wall
                        Exit For ' only process the first wall, if one was selected
                    End If
                Next

                Return IExternalCommand.Result.Succeeded
            Catch ex As Exception
                message = ex.ToString()
                Return IExternalCommand.Result.Failed
            End Try
        End Function
    End Class

#End Region

End Namespace
