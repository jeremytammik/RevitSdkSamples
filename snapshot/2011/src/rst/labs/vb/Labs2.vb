#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2007-2008 by Autodesk, Inc.
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
#End Region ' Header

#Region "Namespaces"
Imports System
Imports System.Collections.Generic
Imports db = Autodesk.Revit.DB
Imports dbst = Autodesk.Revit.DB.Structure
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.Attributes
Imports Constants = Microsoft.VisualBasic.Constants
#End Region ' Namespaces

Namespace RstLabs
#Region "Lab2_1_ListStructuralColumns"
    ''' <summary>
    ''' Lab 2-1 - List all structural columns in the model.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
  <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_1_ListStructuralColumns
        Implements IExternalCommand
        Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As db.ElementSet) As Result Implements IExternalCommand.Execute
            Try
                Dim app As UIApplication = commandData.Application
                Dim doc As db.Document = app.ActiveUIDocument.Document
                '
                ' Get all Structural COLUMNS - we can use a generic utility.
                ' In 8.1 we had to hard-code the category name which then works only in
                ' specific locale (EN or DE or IT etc.):
                ' ElementSet columns = GetStandardFamilyInstancesForACategory( app, "Structural Columns" )
                ' From 9.0 onwards, there is category enum, which works in ANY locale:
                '
                Dim bicSc As db.BuiltInCategory = db.BuiltInCategory.OST_StructuralColumns
                Dim columns As IList(Of db.Element) = RacUtils.GetAllStandardFamilyInstancesForACategory(doc, bicSc)
                Dim msg As String = "There are " & columns.Count.ToString() & " structural column elements:"

                For Each fi As db.FamilyInstance In columns
                    msg &= Constants.vbCrLf & "  " & RstUtils.StructuralElementDescription(fi)
                Next fi
                RacUtils.InfoMsg(msg)
                Return Result.Succeeded
            Catch ex As Exception
                message = ex.Message
                Return Result.Failed
            End Try
        End Function
    End Class
#End Region ' Lab2_1_ListStructuralColumns

#Region "Lab2_2_ListStructuralFraming"
    ''' <summary>
    ''' Lab 2-2 - List all structural framing elements in the model.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
  <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_2_ListStructuralFraming
        Implements IExternalCommand
        Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As db.ElementSet) As Result Implements IExternalCommand.Execute
            Dim app As UIApplication = commandData.Application
            Dim doc As db.Document = app.ActiveUIDocument.Document
            ' get all structural framing elements - similar to Lab 2-1:
            Dim bicSf As db.BuiltInCategory = db.BuiltInCategory.OST_StructuralFraming
            Dim frmEls As IList(Of db.Element) = RacUtils.GetAllStandardFamilyInstancesForACategory(doc, bicSf)
            Dim msg As String = "There are " & frmEls.Count.ToString() & " structural framing elements:"
            For Each fi As db.FamilyInstance In frmEls
                ' INSTANCE_STRUCT_USAGE_TEXT_PARAM works only in 8.1 and not in 9. From 2010, it works well.
                ' so better use dedicated class property StructuralUsage which works in both.
                msg &= Constants.vbCrLf & "  " & RstUtils.StructuralElementDescription(fi)
            Next fi
            RacUtils.InfoMsg(msg)
            Return Result.Succeeded
        End Function
    End Class
#End Region ' Lab2_2_ListStructuralFraming

#Region "Lab2_3_Foundations"
    ''' <summary>
    ''' Lab 2-3 - List all structural foundation elements in the model.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
  <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_3_Foundations
        Implements IExternalCommand
        Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As db.ElementSet) As Result Implements IExternalCommand.Execute
            Dim app As UIApplication = commandData.Application
            Dim doc As db.Document = app.ActiveUIDocument.Document
            '
            ' Get all standard Structural FOUNDATION elements - again the same. Note that this:
            '   a)  excludes "Wall Foundation" System Type under "Structural Foundations" category in the Browser - these belong to *Continuous Footing* system family, see next Lab
            '   b)  excludes "Foundation Slab" System Type under "Structural Foundations" category in the Browser - these are internally implemented as Revit *Floor* system family, see next Lab
            Dim bic As db.BuiltInCategory = db.BuiltInCategory.OST_StructuralFoundation
            Dim struFnds As IList(Of db.Element) = RacUtils.GetAllStandardFamilyInstancesForACategory(doc, bic)
            Dim msg As String = "There are " & struFnds.Count.ToString() & " structural foundation (standard families only) elements :"

            For Each elem As db.Element In struFnds
                If TypeOf elem Is db.FamilyInstance Then
                    Dim fi As db.FamilyInstance = TryCast(elem, db.FamilyInstance)
                    msg &= Constants.vbCrLf & "  " & RstUtils.StructuralElementDescription(fi)
                End If
            Next elem
            RacUtils.InfoMsg(msg)
            Return Result.Succeeded
        End Function
    End Class
#End Region ' Lab2_3_Foundations

#Region "Lab2_4_StandardFamilyInstances"
    ''' <summary>
    ''' Lab 2-4 - List all standard family instances with an analytical model.
    ''' This presents an alternative way from Labs 2-1, 2-2 and 2-3 to retrieve
    ''' all structural family instances.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
  <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_4_StandardFamilyInstances
        Implements IExternalCommand
        Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As db.ElementSet) As Result Implements IExternalCommand.Execute
            Dim app As UIApplication = commandData.Application
            Dim doc As db.Document = app.ActiveUIDocument.Document
            'Below commented codes works for Revit 2010.
            '
            'Filter filter = app.Create.Filter.NewTypeFilter( typeof( FamilyInstance ) );
            'List<Element> instances = new List<Element>();
            'app.ActiveDocument.Elements( filter, instances );

            'From 2011, the way to iterate elements is changed. FilteredElementCollector is used to do this.
            Dim instances As IList(Of db.Element) = Nothing
            Dim collector As New db.FilteredElementCollector(doc)
            instances = collector.OfClass(GetType(db.FamilyInstance)).ToElements()
            Dim msg As String = "All structural family instances (generic check):"
            'string categoryName;
            'int i = 0;
            For Each fi As db.FamilyInstance In instances
                '
                ' Note that instead of looping through and checking for a
                ' non-null analytical model, we might also be able to use some
                ' other criterion that can be fed straight into the Revit API
                ' filtering mechanism, such as structural usage:
                '
                If Nothing IsNot fi.GetAnalyticalModel() Then
                    msg &= Constants.vbCrLf & "  " & RstUtils.StructuralElementDescription(fi)
                End If
            Next fi
            RacUtils.InfoMsg(msg)
            Return Result.Succeeded
        End Function
    End Class
#End Region ' Lab2_4_StandardFamilyInstances

#Region "Lab2_5_StructuralSystemFamilyInstances"
    ''' <summary>
    ''' Lab 2-5 - Retrieve structural system family instances: wall, floor, continuous footing.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
  <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_5_StructuralSystemFamilyInstances
        Implements IExternalCommand
        Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As db.ElementSet) As Result Implements IExternalCommand.Execute
            Dim app As UIApplication = commandData.Application
            Dim doc As db.Document = app.ActiveUIDocument.Document
            ' get all structural walls elements using a dedicated helper
            ' that checks for all walls of structural usage:
            Dim struWalls As List(Of db.Element) = RstUtils.GetAllStructuralWalls(doc)
            Dim msg As String = "There are " & struWalls.Count.ToString() & " structural wall elements:"

            For Each w As db.Wall In struWalls
                ' WALL_STRUCTURAL_USAGE_TEXT_PARAM works only in 8.1 and not from 9,
                ' so better use dedicated class property StructuralUsage which works
                ' in both:
                'msg = msg + "\r\n  Id="  + w.Id.IntegerValue.ToString()
                '  + ", Type=" + w.WallType.Name
                '  + ", Struct.Usage=" + w.StructuralUsage.ToString()
                '  + ", Analytical Type=" + w.AnalyticalModel.GetType().Name;
                msg &= Constants.vbCrLf & "  " & RstUtils.StructuralElementDescription(w)
            Next w
            RacUtils.InfoMsg(msg)

            ' get all structural floor elements using a dedicated helper
            ' that checks for all floors of structural usage.
            ' note: from RS3 onwards, these include not only standard
            ' floors, but also "Foundation Slab" instances from the
            ' "Structural Foundations" category:
            Dim struFloors As List(Of db.Element) = RstUtils.GetAllStructuralFloors(app.ActiveUIDocument.Document)
            msg = "There are " & struFloors.Count & " structural floor elements:"

            For Each fl As db.Floor In struFloors
                'msg = msg + "\r\n  Id=" + fl.Id.IntegerValue.ToString()
                '  + ", Category=" + fl.Category.Name
                '  + ", Type=" + fl.FloorType.Name
                '  + ", Analytical Type=" + fl.AnalyticalModel.GetType().Name;
                msg &= Constants.vbCrLf & "  " & RstUtils.StructuralElementDescription(fl)
            Next fl
            RacUtils.InfoMsg(msg)

            ' get all structural continuous footing elements.
            ' note: from RS3, these are "Wall Foundation" instances from
            ' the "Structural Foundations" category:
            Dim contFootings As List(Of db.Element) = RstUtils.GetAllStructuralContinuousFootings(app.ActiveUIDocument.Document)
            msg = "There are " & contFootings.Count.ToString() & " structural continuous footing (or wall foundation) elements:"

            For Each cf As db.ContFooting In contFootings
                msg &= Constants.vbCrLf & "  Id=" & cf.Id.IntegerValue.ToString() & " Type=" & cf.FootingType.Name
                msg &= Constants.vbCrLf & "  " & RstUtils.StructuralElementDescription(cf)
            Next cf
            RacUtils.InfoMsg(msg)
            Return Result.Succeeded
        End Function
    End Class
#End Region ' Lab2_5_StructuralSystemFamilyInstances
End Namespace
