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
Imports System.IO
Imports System.Linq
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.Attributes
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.DB.Architecture
Imports Autodesk.Revit.DB.Structure
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.UI.Selection
Imports Microsoft.VisualBasic.Constants
#End Region

Namespace Labs

#Region "Lab3_1_StandardFamiliesAndTypes"
    ''' <summary>
    ''' List all loaded standard families and types.
    ''' As a result of the issues explained below, we implemented
    ''' the LabUtils utility methods FamilyCategory and GetFamilies.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="3-1"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.ReadOnly)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab3_1_StandardFamiliesAndTypes
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim doc As Document = app.ActiveUIDocument.Document

            Dim a As New List(Of String)()
            Dim families As FilteredElementCollector

            '#region 3.1.a Retrieve and iterate over all Family objects in document:
            '
            ' get all family elements in current document:
            '
            families = New FilteredElementCollector(doc)

            families.OfClass(GetType(Family))

            For Each f As Family In families

                ' Get its category name; notice that the Category property is not
                ' implemented for the Family class; use FamilyCategory instead;
                ' notice that that is also not always implemented; in that case,
                ' use the workaround demonstrated below, looking at the contained
                ' family symbols' category:

                a.Add("Name=" + f.Name _
                      + "; Category=" + If(f.Category Is Nothing, "?", f.Category.Name) _
                      + "; FamilyCategory=" + If(f.FamilyCategory Is Nothing, "?", f.FamilyCategory.Name))
            Next
            '#endregion 3.1.a


            Dim msg As String = "{0} standard familie{1} are loaded in this model{2}"
            LabUtils.InfoMsg(msg, a)

            ' Loop through the collection of families, and now look at
            ' the child symbols (types) as well. These symbols can be
            ' used to determine the family category.

            For Each f As Family In families
                Dim catName As String
                Dim first As Boolean = True

                ' Loop all contained symbols (types):

                For Each s As FamilySymbol In f.Symbols
                    ' you can determine the family category from its first symbol.

                    If first Then
                        first = False

                        '#region 3.1.b Retrieve category name of first family symbol:
                        catName = s.Category.Name
                        '#endregion 3.1.b

                        msg = "Family: Name=" + f.Name _
                            + "; Id=" + f.Id.IntegerValue.ToString() _
                            + "; Category=" + catName + vbCrLf + "Contains Types:"
                    End If
                    msg += vbCrLf + "    " + s.Name + "; Id=" + s.Id.IntegerValue.ToString()
                Next

                ' Show the symbols for this family and allow user to proceed
                ' to the next family (OK) or cancel (Cancel)

                msg += vbCrLf + "Continue?"
                If Not LabUtils.QuestionMsg(msg) Then
                    Exit For
                End If
            Next
            Return Result.Failed
        End Function

    End Class
#End Region

#Region "Lab3_2_LoadStandardFamilies"
    ''' <summary>
    ''' Load an entire family or a specific type from a family.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="3-2"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab3_2_LoadStandardFamilies
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim doc As Document = app.ActiveUIDocument.Document
            Dim rc As Boolean

            '#region 3.2.a Load an entire RFA family file:

            ' Load a whole Family
            '
            ' Example for a family WITH TXT file:

            rc = doc.LoadFamily(LabConstants.WholeFamilyFileToLoad1)
            '#endregion 3.2.a

            If rc Then
                LabUtils.InfoMsg("Successfully loaded family " _
                                 + LabConstants.WholeFamilyFileToLoad1 + ".")
            Else
                LabUtils.ErrorMsg("ERROR loading family " _
                                  + LabConstants.WholeFamilyFileToLoad1 + ".")
            End If

            ' Example for a family WITHOUT TXT file:

            If doc.LoadFamily(LabConstants.WholeFamilyFileToLoad2) Then
                LabUtils.InfoMsg("Successfully loaded family " _
                                 + LabConstants.WholeFamilyFileToLoad2 + ".")
            Else
                LabUtils.ErrorMsg("ERROR loading family " _
                                  + LabConstants.WholeFamilyFileToLoad2 + ".")
            End If

            '#region 3.2.b Load an individual type from a RFA family file:

            ' Load only a specific symbol (type):

            rc = doc.LoadFamilySymbol( _
                LabConstants.FamilyFileToLoadSingleSymbol, _
                LabConstants.SymbolName)
            '#endregion 3.2.b

            If rc Then
                LabUtils.InfoMsg("Successfully loaded family symbol " _
                                 + LabConstants.FamilyFileToLoadSingleSymbol _
                                 + " : " + LabConstants.SymbolName + ".")
            Else
                LabUtils.ErrorMsg("ERROR loading family symbol " _
                                  + LabConstants.FamilyFileToLoadSingleSymbol _
                                  + " : " + LabConstants.SymbolName + ".")
            End If
            Return Result.Succeeded
        End Function
    End Class
#End Region

#Region "Lab3_3_DetermineInstanceTypeAndFamily"
    ''' <summary>
    ''' For a selected family instance in the model, determine its type and family.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="3-3"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab3_3_DetermineInstanceTypeAndFamily
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim uidoc As UIDocument = app.ActiveUIDocument
            Dim doc As Document = uidoc.Document

            ' retrieve all FamilySymbol objects of "Windows" category:

            Dim bic As BuiltInCategory = BuiltInCategory.OST_Windows
            Dim symbols As FilteredElementCollector = LabUtils.GetFamilySymbols(doc, bic)

            Dim a As New List(Of String)()

            For Each s As FamilySymbol In symbols
                Dim fam As Family = s.Family

                a.Add(s.Name + ", Id=" + s.Id.IntegerValue.ToString() _
                      + "; Family name=" + fam.Name _
                      + ", Family Id=" + fam.Id.IntegerValue.ToString())
            Next
            LabUtils.InfoMsg("{0} windows family symbol{1} loaded in the model{1}", a)

            ' loop through the selection set and check for
            ' standard family instances of "Windows" category:

            Dim iBic As Integer = CInt(bic)
            Dim msg As String, content As String

            For Each e As Element In uidoc.Selection.Elements
                If TypeOf e Is FamilyInstance _
                    AndAlso e.Category IsNot Nothing _
                    AndAlso e.Category.Id.IntegerValue.Equals(iBic) Then

                    msg = "Selected window Id=" + e.Id.IntegerValue.ToString()

                    Dim inst As FamilyInstance = TryCast(e, FamilyInstance)

                    '#region 3.3 Retrieve the type of the family instance, and the family of the type:

                    Dim fs As FamilySymbol = inst.Symbol

                    Dim f As Family = fs.Family

                    '#endregion 3.3

                    content = "FamilySymbol = " + fs.Name _
                        + "; Id=" + fs.Id.IntegerValue.ToString()

                    content += vbCrLf + " Family = " + f.Name _
                        + "; Id=" + f.Id.IntegerValue.ToString()

                    LabUtils.InfoMsg(msg, content)
                End If
            Next
            Return Result.Succeeded
        End Function

    End Class
#End Region

#Region "Lab3_4_ChangeSelectedInstanceType"
    ''' <summary>
    ''' Form-based utility to change the type or symbol of a selected standard instance.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="3-4"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab3_4_ChangeSelectedInstanceType
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim uidoc As UIDocument = app.ActiveUIDocument
            Dim doc As Document = uidoc.Document

            Dim inst As FamilyInstance = TryCast( _
                LabUtils.GetSingleSelectedElementOrPrompt( _
                    uidoc, GetType(FamilyInstance)), FamilyInstance)

            If inst Is Nothing Then
                LabUtils.ErrorMsg("Selected element is not a standard family instance.")
                Return Result.Cancelled
            End If

            Dim instCat As Category = inst.Category

            Dim mapFamilyToSymbols As New Dictionary(Of String, List(Of FamilySymbol))()

            If True Then
                Dim waitCursor As New WaitCursor()

                ' Collect all types applicable to this category and sort them into
                ' a dictionary mapping the family name to a list of its types.
                '
                ' We create a collection of all loaded families for this category
                ' and for each one, the list of all loaded types (symbols).
                '
                ' There are many ways how to store the matching objects, but we choose
                ' whatever is most suitable for the relevant UI. We could use Revit's
                ' generic Map class, but it is probably more efficient to use the .NET
                ' strongly-typed Dictionary with
                ' KEY = Family name (String)
                ' VALUE = list of corresponding FamilySymbol objects
                '
                ' Find all the corresponding Families/Types:

                Dim families As New FilteredElementCollector(doc)

                families.OfClass(GetType(Family))

                For Each f As Family In families
                    Dim categoryMatches As Boolean = False

                    ' we cannot trust f.Category or f.FamilyCategory,
                    ' grab category from first family symbol instead:

                    For Each sym As FamilySymbol In f.Symbols
                        categoryMatches = sym.Category.Id.Equals(instCat.Id)
                        Exit For
                    Next

                    If categoryMatches Then
                        Dim symbols As New List(Of FamilySymbol)()

                        For Each sym As FamilySymbol In f.Symbols
                            symbols.Add(sym)
                        Next

                        mapFamilyToSymbols.Add(f.Name, symbols)
                    End If
                Next
            End If

            ' Display the form, allowing the user to select a family
            ' and a type, and assign this type to the instance.

            Dim form As New Lab3_4_Form(mapFamilyToSymbols)

            If System.Windows.Forms.DialogResult.OK = form.ShowDialog() Then
                inst.Symbol = TryCast(form.cmbType.SelectedItem, FamilySymbol)

                LabUtils.InfoMsg("Successfully changed family : type to " _
                                 + form.cmbFamily.Text + " : " + form.cmbType.Text)
            End If
            Return Result.Succeeded
        End Function

    End Class
#End Region

#Region "Lab3_5_WallAndFloorTypes"
    ''' <summary>
    ''' Access and modify system family type, similarly
    ''' to the standard families looked at above:
    '''
    ''' List all wall and floor types and
    ''' change the type of selected walls and floors.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="3-5"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab3_5_WallAndFloorTypes
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim uidoc As UIDocument = app.ActiveUIDocument
            Dim doc As Document = uidoc.Document

            ' Find all wall types and their system families (or kinds):

            Dim newWallType As WallType = Nothing

            Dim msg As String = "All wall types and families in the model:"
            Dim content As String = String.Empty

            For Each wt As WallType In doc.WallTypes
                content += vbCrLf + "Type=" + wt.Name + " Family=" + wt.Kind.ToString()
                newWallType = wt
            Next

            content += vbCrLf + "Stored WallType " + newWallType.Name + " (Id=" + newWallType.Id.IntegerValue.ToString() + ") for later use."

            LabUtils.InfoMsg(msg, content)

            ' Find all floor types:

            Dim newFloorType As FloorType = Nothing

            msg = "All floor types in the model:"
            content = String.Empty

            For Each ft As FloorType In doc.FloorTypes
                content += vbCrLf + "Type=" + ft.Name + ", Id=" + ft.Id.IntegerValue.ToString()

                ' In 9.0, the "Foundation Slab" system family from "Structural
                ' Foundations" category ALSO contains FloorType class instances.
                ' Be careful to exclude those as choices for standard floor types.

                Dim p As Parameter = ft.Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM)
                Dim famName As String = If(p Is Nothing, "?", p.AsString())
                Dim cat As Category = ft.Category
                content += ", Family=" + famName + ", Category=" + cat.Name

                ' store for the new floor type only if it has the proper Floors category:

                If cat.Id.Equals(CInt(BuiltInCategory.OST_Floors)) Then
                    newFloorType = ft
                End If
            Next

            content += vbCrLf + If(newFloorType Is Nothing, "No floor type found.", _
                          "Stored FloorType " + newFloorType.Name + " (Id=") _
                          + newFloorType.Id.IntegerValue.ToString() + ") for later use"

            LabUtils.InfoMsg(msg, content)

            ' Change the type for selected walls and floors:

            msg = "{0} {1}: Id={2}" + vbCrLf _
                + " changed from old type={3}; Id={4}" _
                + " to new type={5}; Id={6}."

            Dim sel As ElementSet = uidoc.Selection.Elements

            Dim iWall As Integer = 0
            Dim iFloor As Integer = 0

            For Each e As Element In sel
                If TypeOf e Is Wall Then
                    iWall += 1
                    Dim wall As Wall = TryCast(e, Wall)
                    Dim oldWallType As WallType = wall.WallType

                    ' change wall type and report the old/new values

                    wall.WallType = newWallType

                    LabUtils.InfoMsg(String.Format(msg, "Wall", iWall, wall.Id.IntegerValue, _
                                                   oldWallType.Name, oldWallType.Id.IntegerValue, _
                    wall.WallType.Name, wall.WallType.Id.IntegerValue))
                ElseIf newFloorType IsNot Nothing AndAlso TypeOf e Is Floor Then
                    iFloor += 1
                    Dim f As Floor = TryCast(e, Floor)
                    Dim oldFloorType As FloorType = f.FloorType
                    f.FloorType = newFloorType

                    LabUtils.InfoMsg(String.Format(msg, "Floor", iFloor, f.Id.IntegerValue, _
                                                   oldFloorType.Name, oldFloorType.Id.IntegerValue, _
                    f.FloorType.Name, f.FloorType.Id.IntegerValue))
                End If
            Next
            Return Result.Succeeded
        End Function

    End Class
#End Region

#Region "Lab3_6_DuplicateWallType"
    ''' <summary>
    ''' Create a new family symbol or type by calling Duplicate()
    ''' on an existing one and then modifying its parameters.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab3_6_DuplicateWallType
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim uidoc As UIDocument = app.ActiveUIDocument
            Dim doc As Document = uidoc.Document

            Dim els As ElementSet = uidoc.Selection.Elements

            Const newWallTypeName As String = "NewWallType_with_Width_doubled"

            For Each e As Element In els
                Dim wall As Wall = TryCast(e, Wall)
                If wall IsNot Nothing Then
                    Dim wallType As WallType = wall.WallType

                    Dim newWallType As WallType _
                        = TryCast(wallType.Duplicate( _
                            newWallTypeName), WallType)

                    Dim layers As CompoundStructureLayerArray _
                        = newWallType.CompoundStructure.Layers

                    For Each layer As CompoundStructureLayer _
                        In layers

                        ' double each layer thickness:

                        layer.Thickness *= 2.0R
                    Next

                    ' assign the new wall type back to the wall:

                    wall.WallType = newWallType

                    ' only process the first wall, if one was selected:
                    Exit For
                End If
            Next
            Return Result.Succeeded

        End Function
    End Class

#End Region

#Region "Lab3_7_DeleteFamilyType"
    ''' <summary>
    ''' Delete a specific individual type from a family.
    ''' Hard-coded to a column type named "475 x 610mm".
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab3_7_DeleteFamilyType
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim doc As Document = app.ActiveUIDocument.Document

            Dim collector As FilteredElementCollector _
                = LabUtils.GetFamilySymbols( _
                    doc, BuiltInCategory.OST_Columns)

            'Dim column_types = From element In collector _
            '    Where element.Name.Equals("475 x 610mm") _
            '    Select Element
            '
            'Dim column_types_ienum As IEnumerable(Of Element)
            'column_types_ienum = CType(column_types, IEnumerable(Of Element))
            '
            'Dim column_types_famsym As IEnumerable(Of FamilySymbol)
            'column_types_famsym = column_types_ienum.Cast(Of FamilySymbol)()
            '
            'Dim symbol As FamilySymbol = column_types_famsym.First()

            'Dim name_equals = Function(e) e.Name.Equals("475 x 610mm")

            'Dim name_equals As Func(Of Element, Boolean) _
            '    = Function(e As Element) e.Name.Equals("475 x 610mm")
            '
            'Dim symbol As Element = collector.First(name_equals)

            Dim symbol As Element = collector.First( _
                Function(e As Element) e.Name.Equals("475 x 610mm"))

            doc.Delete(symbol)

            Return Result.Succeeded
        End Function

    End Class

#End Region

End Namespace

