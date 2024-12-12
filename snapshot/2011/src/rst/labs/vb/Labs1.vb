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
Imports WinForms = System.Windows.Forms
Imports db = Autodesk.Revit.DB
Imports dbst = Autodesk.Revit.DB.Structure
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.Attributes
Imports Constants = Microsoft.VisualBasic.Constants
#End Region ' Namespaces

Namespace RstLabs
#Region "Lab1_1_LoadGrouping"
    ''' <summary>
    ''' Lab 1-1 - List all load grouping objects,
    ''' i.e. load natures, cases,usage,and combinations.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab1_1_LoadGrouping
        Implements IExternalCommand
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As db.ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim doc As db.Document = app.ActiveUIDocument.Document
            ' load natures
            Dim ldNatures As IList(Of db.Element) = RstUtils.GetInstanceOfClass(doc, GetType(dbst.LoadNature))
            Dim msg As String = "There are " & ldNatures.Count.ToString() & " LoadNature objects in the model:"
            For Each nature As dbst.LoadNature In ldNatures
                msg &= Constants.vbCrLf & "  " & nature.Name & ", Id=" & nature.Id.IntegerValue.ToString()
            Next nature
            RacUtils.InfoMsg(msg)

            ' load cases
            Dim ldCases As IList(Of db.Element) = RstUtils.GetInstanceOfClass(doc, GetType(dbst.LoadCase))
            msg = "There are " & ldCases.Count.ToString() & " LoadCase objects in the model:"
            For Each ldCase As dbst.LoadCase In ldCases
                ' Category is NOT implemented in API for Loadcase class in 2008 and 2009,
                ' though the category enums are there (OST_LoadCasesXxx)
                Dim catName As String = If((Nothing Is ldCase.Category), "?", ldCase.Category.Name)
                msg &= Constants.vbCrLf & "  " & ldCase.Name & ", Id=" & ldCase.Id.IntegerValue.ToString() & ", Category=" & catName
                ' there seems to be no way to get LoadCase's LoadNature in API
            Next ldCase
            RacUtils.InfoMsg(msg)

            ' load usages (optionally used for combinations)
            Dim ldUsages As IList(Of db.Element) = RstUtils.GetInstanceOfClass(doc, GetType(dbst.LoadUsage))
            msg = "There are " & ldUsages.Count.ToString() & " LoadUsage objects in the model:"
            For Each ldUsage As dbst.LoadUsage In ldUsages
                msg &= Constants.vbCrLf & "  " & ldUsage.Name & ", Id=" & ldUsage.Id.IntegerValue.ToString()
            Next ldUsage
            RacUtils.InfoMsg(msg)

            ' load combinations
            Dim combs As IList(Of db.Element) = RstUtils.GetInstanceOfClass(doc, GetType(dbst.LoadCombination))
            msg = "There are " & combs.Count.ToString() & " LoadCombination objects in the model:"
            For Each comb As dbst.LoadCombination In combs
                ' combinaton properties
                Dim usageNames As String = If((0 = comb.NumberOfUsages), "[NONE]", comb.UsageName(0))
                For i As Integer = 1 To comb.NumberOfUsages - 1
                    usageNames &= "+"
                    usageNames &= comb.UsageName(i)
                Next i
                msg &= Constants.vbCrLf & Constants.vbCrLf & "  " & comb.Name & ", Id=" & comb.Id.IntegerValue.ToString() & ", Type=" & comb.CombinationType & ", TypeIndex=" & comb.CombinationTypeIndex & ", State=" & comb.CombinationState & ", StateIndex=" & comb.CombinationStateIndex & ", Usages=" & usageNames

                msg &= Constants.vbCrLf & "    Number of components = " & comb.NumberOfComponents & ":"
                ' loop all component properties
                For i As Integer = 0 To comb.NumberOfComponents - 1
                    msg &= Constants.vbCrLf & "    Comp.name=" & comb.CombinationCaseName(i) & "  Comp.nature=" & comb.CombinationNatureName(i) & "  Factor=" & comb.Factor(i)
                Next i
            Next comb
            RacUtils.InfoMsg(msg)
            ' most of these objects are also createable, see the
            ' Autodesk.Revit.Creation.Document.NewLoad*** methods.
            ' To call them, use something like:
            ' app.ActiveDocument.Create.NewLoadCase(...)
            ' app.ActiveDocument.Create.NewLoadCombination(...)
            ' app.ActiveDocument.Create.NewLoadNature(...)
            ' app.ActiveDocument.Create.NewLoadUsage(...)
            Return Result.Succeeded
        End Function
    End Class
#End Region ' Lab1_1_LoadGrouping

#Region "Lab1_2_Loads"
    ''' <summary>
    ''' Lab 1-2 - Demonstrate access to load objects and list all loads.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
  <Regeneration(RegenerationOption.Manual)> _
  Public Class Lab1_2_Loads
        Implements IExternalCommand
        Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As db.ElementSet) As Result Implements IExternalCommand.Execute
            Dim app As UIApplication = commandData.Application
            Dim doc As db.Document = app.ActiveUIDocument.Document
            'Get all loads
            Dim loads As IList(Of db.Element) = RstUtils.GetInstanceOfClass(doc, GetType(dbst.LoadBase), False)
            Dim msg As String = "There are " & loads.Count.ToString() & " load objects in the model:"
            Dim sHost As String

            For Each load As dbst.LoadBase In loads
                sHost = If((Nothing Is load.HostElement), String.Empty, String.Format(", Host={0}:{1}", load.HostElement.Name, load.HostElement.Id.IntegerValue))
                msg &= Constants.vbCrLf & "  " & load.GetType().Name & ", Id=" & load.Id.IntegerValue.ToString() & sHost
            Next load
            RacUtils.InfoMsg(msg)
            ' We could loop the above collection, but better code
            ' another helper for specific loads directly

            Dim pointLoads As List(Of db.Element) = New List(Of db.Element)()
            Dim lineLoads As List(Of db.Element) = New List(Of db.Element)()
            Dim areaLoads As List(Of db.Element) = New List(Of db.Element)()

            RstUtils.GetAllSpecificLoads(app.ActiveUIDocument.Document, pointLoads, lineLoads, areaLoads)

            ' POINT
            msg = pointLoads.Count.ToString() & " POINT Loads:"
            For Each ptLd As dbst.PointLoad In pointLoads
                ' The following are all specific READ-ONLY properties:
                Dim F As db.XYZ = ptLd.Force
                Dim M As db.XYZ = ptLd.Moment
                Dim p As db.XYZ = ptLd.Point
                Dim nameLoadCase As String = ptLd.LoadCaseName
                Dim nameLoadCategory As String = ptLd.LoadCategoryName ' bug in 8.1 - returns nothing - still the same in 2009
                Dim nameLoadNature As String = ptLd.LoadNatureName
                msg &= Constants.vbCrLf & "  Id=" & ptLd.Id.IntegerValue.ToString() & ": F=" & RacUtils.PointString(F) & ", M=" & RacUtils.PointString(M) & ", Pt=" & RacUtils.PointString(p) & ", LoadCase=" & nameLoadCase & ", LoadCat=" & nameLoadCategory & ", LoadNature=" & nameLoadNature
            Next ptLd
            RacUtils.InfoMsg(msg)

            ' LINE
            msg = lineLoads.Count.ToString() & " LINE Loads:"
            For Each lnLd As dbst.LineLoad In lineLoads
                ' The following are *some* READ-ONLY properties:
                Dim F1 As db.XYZ = lnLd.Force1
                Dim F2 As db.XYZ = lnLd.Force2
                ' can do similar for Moment1, Moment2...
                Dim ptStart As db.XYZ = lnLd.StartPoint
                ' similar for EndPoint
                ' LoadxxxName same as for PointLoad (implemented in the base class)
                msg &= Constants.vbCrLf & "  Id=" & lnLd.Id.IntegerValue.ToString() & ": F1= " & RacUtils.PointString(F1) & ", F2= " & RacUtils.PointString(F2) & ", Start= " & RacUtils.PointString(ptStart) & ", Uniform=" & lnLd.UniformLoad & ", Projected=" & lnLd.ProjectedLoad
            Next lnLd
            RacUtils.InfoMsg(msg)

            ' AREA
            msg = areaLoads.Count.ToString() & " AREA Loads:"
            For Each arLd As dbst.AreaLoad In areaLoads
                ' The following are *some* READ-ONLY properties:
                Dim F1 As db.XYZ = arLd.Force1
                Dim numLoops As Integer = arLd.NumLoops
                Dim s As String = ", Nr. of Loops=" & numLoops.ToString() & ", Nr. of Curves=" & arLd.NumCurves(0).ToString()
                For i As Integer = 1 To numLoops - 1 ' if more than single loop
                    s &= "," & arLd.NumCurves(i)
                Next i
                msg &= Constants.vbCrLf & "  Id=" & arLd.Id.IntegerValue.ToString() & ": F1= " & RacUtils.PointString(F1) & s

                ' For curves geometry, see later commands...
            Next arLd
            RacUtils.InfoMsg(msg)
            Return Result.Succeeded
        End Function
    End Class
#End Region ' Lab1_2_Loads

#Region "Lab1_3_ModifySelectedLoads"
    ''' <summary>
    ''' Lab 1-3 - More detailed info and modification of selected loads.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
  <Regeneration(RegenerationOption.Manual)> _
  Public Class Lab1_3_ModifySelectedLoads
        Implements IExternalCommand
        Private _doc As db.Document

        Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As db.ElementSet) As Result Implements IExternalCommand.Execute
            Dim app As UIApplication = commandData.Application
            Dim doc As db.Document = app.ActiveUIDocument.Document
            _doc = app.ActiveUIDocument.Document
            Dim list As IList(Of db.Element)()
            list = RstUtils.GetInstanceOfClass(doc, GetType(dbst.LoadBase), False)
            For Each elem As db.Element In list
                If TypeOf elem Is dbst.PointLoad Then
                    ListAndModifyPointLoad(TryCast(elem, dbst.PointLoad))
                ElseIf TypeOf elem Is dbst.LineLoad Then
                    ListAndModifyLineLoad(TryCast(elem, dbst.LineLoad), app.ActiveUIDocument.Document)
                ElseIf TypeOf elem Is dbst.AreaLoad Then
                    ListAndModifyAreaLoad(TryCast(elem, dbst.AreaLoad), app.ActiveUIDocument.Document)
                End If
            Next elem
            Return Result.Succeeded
        End Function

        Public Sub ListAndModifyPointLoad(ByVal ptLd As dbst.PointLoad)
            ' One can access some parameters as read-only class properties/methods,
            ' but to be able to change them and to access all, we need to go via Parameters:
            Dim msg As String = "POINT Load Id=" & ptLd.Id.IntegerValue.ToString()
            Try
                ' List 2 Params via their display name (locale-dependent!)
                msg &= Constants.vbCrLf & "  Is Reaction (via Display Name)= " & RacUtils.GetParameterValue(ptLd.Parameter("Is Reaction"))
                msg &= Constants.vbCrLf & "  Load Case (via Display Name)= " & RacUtils.GetParameterValue2(ptLd.Parameter("Load Case"), _doc)

                ' Better use BUILT_IN_PARAMS, but have to guess-and-try-and-error to find correct ones!
                ' The following 2 give the same as above:
                msg &= Constants.vbCrLf & "  LOAD_IS_REACTION db.BuiltInParameter= " & RacUtils.GetParameterValue(ptLd.Parameter(db.BuiltInParameter.LOAD_IS_REACTION))

                msg &= Constants.vbCrLf & "  LOAD_CASE_ID db.BuiltInParameter= " & RacUtils.GetParameterValue2(ptLd.Parameter(db.BuiltInParameter.LOAD_CASE_ID), _doc)

                ' The following display one hasn't got one-to-one corresponding Built-in, but basically the same:
                msg &= Constants.vbCrLf & "  Orient to (via Display Name)= " & RacUtils.GetParameterValue(ptLd.Parameter("Orient to"))
                msg &= Constants.vbCrLf & "  LOAD_USE_LOCAL_COORDINATE_SYSTEM db.BuiltInParameter= " & RacUtils.GetParameterValue(ptLd.Parameter(db.BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM))
                msg &= Constants.vbCrLf & "  LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED db.BuiltInParameter= " & RacUtils.GetParameterValue(ptLd.Parameter(db.BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED))

                ' Scale Fz by factor 2
                Dim paramFz As db.Parameter = ptLd.Parameter(db.BuiltInParameter.LOAD_FORCE_FZ)
                Dim Fz_old As Double = paramFz.AsDouble()
                paramFz.Set(2.0 * Fz_old)
                msg &= Constants.vbCrLf & "  Fz:  OLD=" & Fz_old.ToString() & " NEW=" & paramFz.AsDouble().ToString()

                ' If created by API
                'try // this param was working in RS2, but not accessible in RS3!. It is ok in RST2010
                msg &= Constants.vbCrLf & "  LOAD_IS_CREATED_BY_API db.BuiltInParameter= " & RacUtils.GetParameterValue(ptLd.Parameter(db.BuiltInParameter.LOAD_IS_CREATED_BY_API))
                'try
                msg &= Constants.vbCrLf & "  ELEM_TYPE_PARAM db.BuiltInParameter= " & RacUtils.GetParameterValue(ptLd.Parameter(db.BuiltInParameter.ELEM_TYPE_PARAM))
            Catch ex As Exception
                RacUtils.InfoMsg("Error in ListAndModifyPointLoad: " & ex.Message)
            Finally
                RacUtils.InfoMsg(msg)
            End Try
        End Sub

        Public Sub ListAndModifyLineLoad(ByVal lnLd As dbst.LineLoad, ByVal doc As db.Document)
            ' One can access some parameters as read-only class properties/methods,
            ' but to be able to change them and to access all, we need to go via Parameters:
            Dim msg As String = "LINE Load Id=" & lnLd.Id.IntegerValue.ToString()
            Try
                ' show how to access the same param via both display name and BUILT_IN_PARAMS
                msg &= Constants.vbCrLf & "  Load Case (via Display Name)= " & RacUtils.GetParameterValue(lnLd.Parameter("Load Case"))
                msg &= Constants.vbCrLf & "  LOAD_CASE_ID db.BuiltInParameter= " & RacUtils.GetParameterValue2(lnLd.Parameter(db.BuiltInParameter.LOAD_CASE_ID), _doc)

                ' Scale Fz1 by factor 2, but this time get it as BUILT_IN_PARAMS
                Dim paramFZ1 As db.Parameter = lnLd.Parameter(db.BuiltInParameter.LOAD_LINEAR_FORCE_FZ1)
                Dim FZ1_old As Double = paramFZ1.AsDouble()
                paramFZ1.Set(2.0 * FZ1_old)
                msg &= Constants.vbCrLf & "  FZ1:  OLD=" & FZ1_old.ToString() & " NEW=" & paramFZ1.AsDouble().ToString()

                ' Check if the load is attached to a host - did not work in previous versions.
                ' There was no way to determine if an Line/Area load was hosted.
                ' Since Revit 2008, you can determine this more easily using the
                ' generic load property LoadBase.HostElement, as demonstrated in
                ' Lab1_2_Loads:
                Dim hostIdParam As db.Parameter = lnLd.Parameter(db.BuiltInParameter.HOST_ID_PARAM)
                If Nothing IsNot hostIdParam Then
                    Dim hostId As db.ElementId = hostIdParam.AsElementId()
                    Dim elem As db.Element = doc.Element(hostId)
                    msg &= Constants.vbCrLf & "  HOST element ID=" & elem.Id.IntegerValue.ToString() & " Class=" & elem.GetType().Name & " Name=" & elem.Name
                Else
                    msg &= Constants.vbCrLf & "  NO HOST element"
                End If

                ' test possible values for Line Load:
                ' ALL 3 are 0 if "Project"
                ' ALL 3 are 1 if either "Workplane" or "Host Workplane"
                msg &= Constants.vbCrLf & "  Orient to (via Display Name)= " & RacUtils.GetParameterValue(lnLd.Parameter("Orient to"))
                msg &= Constants.vbCrLf & "  LOAD_USE_LOCAL_COORDINATE_SYSTEM db.BuiltInParameter= " & RacUtils.GetParameterValue(lnLd.Parameter(db.BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM))
                msg &= Constants.vbCrLf & "  LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED db.BuiltInParameter= " & RacUtils.GetParameterValue(lnLd.Parameter(db.BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED))
                msg &= Constants.vbCrLf & "  LOAD_IS_CREATED_BY_API db.BuiltInParameter= " & RacUtils.GetParameterValue(lnLd.Parameter(db.BuiltInParameter.LOAD_IS_CREATED_BY_API))
            Catch ex As Exception
                RacUtils.InfoMsg("Error in ListAndModifyLineLoad: " & ex.Message)
            Finally
                RacUtils.InfoMsg(msg)
            End Try
        End Sub

        Public Sub ListAndModifyAreaLoad(ByVal arLd As dbst.AreaLoad, ByVal doc As db.Document)
            ' One can access some parameters as read-only class properties/methods,
            ' but to be able to change them and to access all, we need to go via Parameters:
            Dim msg As String = "AREA Load Id=" & arLd.Id.IntegerValue.ToString()
            Try
                ' Again, show how to access the same param via both display name and BUILT_IN_PARAMS
                msg &= Constants.vbCrLf & "  Load Case (via Display Name)= " & RacUtils.GetParameterValue(arLd.Parameter("Load Case"))
                msg &= Constants.vbCrLf & "  LOAD_CASE_ID db.BuiltInParameter= " & RacUtils.GetParameterValue2(arLd.Parameter(db.BuiltInParameter.LOAD_CASE_ID), _doc)

                ' Scale Fz1 by factor 2
                Dim paramFZ1 As db.Parameter = arLd.Parameter(db.BuiltInParameter.LOAD_AREA_FORCE_FZ1)
                Dim FZ1_old As Double = paramFZ1.AsDouble()
                paramFZ1.Set(CDbl(2.0 * FZ1_old))
                msg &= Constants.vbCrLf & "  FZ1:  OLD=" & RacUtils.RealString(FZ1_old) & " NEW=" & RacUtils.RealString(paramFZ1.AsDouble()) & Constants.vbCrLf

                ' Specifically for AREA Load, there can be more than a single Loop:
                Dim numLoops As Integer = arLd.NumLoops
                ' Loop all Loops
                msg &= Constants.vbCrLf & "  Number Of Loops = " & numLoops.ToString()
                For i As Integer = 0 To numLoops - 1
                    Dim numCurves As Integer = arLd.NumCurves(i)
                    msg &= Constants.vbCrLf & "  Loop " & (i + 1) & " has " & numCurves.ToString() & " curves:"
                    For j As Integer = 0 To numCurves - 1
                        Dim crv As db.Curve = arLd.Curve(i, j)
                        If TypeOf crv Is db.Line Then
                            Dim line As db.Line = CType(crv, db.Line)
                            Dim ptS As db.XYZ = line.EndPoint(0)
                            Dim ptE As db.XYZ = line.EndPoint(1)
                            msg &= Constants.vbCrLf & "    Curve " & (j + 1) & " is a LINE:" & RacUtils.PointString(ptS) & " ; " & RacUtils.PointString(ptE)
                        ElseIf TypeOf crv Is db.Arc Then
                            Dim arc As db.Arc = CType(crv, db.Arc)
                            Dim ptS As db.XYZ = arc.EndPoint(0)
                            Dim ptE As db.XYZ = arc.EndPoint(1)
                            Dim r As Double = arc.Radius
                            msg &= Constants.vbCrLf & "    Curve " & (j + 1) & " is an ARC:" & RacUtils.PointString(ptS) & " ; " & RacUtils.PointString(ptE) & " ; R=" & r.ToString()
                        End If
                    Next j
                Next i
                Dim numRefPts As Integer = arLd.NumRefPoints
                msg &= Constants.vbCrLf & "  Number of Ref. Points = " & numRefPts.ToString()

                For i As Integer = 0 To numRefPts - 1
                    Dim p As db.XYZ = arLd.RefPoint(i)
                    msg &= Constants.vbCrLf & "  RefPt " & i.ToString() & " = " & RacUtils.PointString(p)
                Next i
            Catch ex As Exception
                RacUtils.InfoMsg("Error in ListAndModifyAreaLoad: " & ex.Message)
            Finally
                RacUtils.InfoMsg(msg)
            End Try
        End Sub
    End Class
#End Region ' Lab1_3_ModifySelectedLoads

#Region "Lab1_4_LoadSymbols"
    ''' <summary>
    ''' Lab 1-4 - List load types.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
  <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab1_4_LoadTypes
        Implements IExternalCommand
        Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As db.ElementSet) As Result Implements IExternalCommand.Execute
            Dim app As UIApplication = commandData.Application
            Dim doc As db.Document = app.ActiveUIDocument.Document
            Dim symbs As IList(Of db.Element) = RstUtils.GetInstanceOfClass(doc, GetType(dbst.LoadTypeBase), False)
            Dim msg As String = "There are " & symbs.Count.ToString() & " load types in the model:"

            For Each ls As db.ElementType In symbs
                'Get the family name of the load type.
                Dim famName As String = "?"
                'try
                ' this worked in RS2, but failing now in RS3, works fine in 2008 and later version
                famName = ls.Parameter(db.BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString()

                'catch
                '{
                '}
                msg &= Constants.vbCrLf & "  " & ls.Name & ", Id=" & ls.Id.IntegerValue.ToString() & ", Family=" & famName
            Next ls
            RacUtils.InfoMsg(msg)

            ' We better make utilities (or alternatively single one for all three,
            ' like for loads) that will get particular symbols. For example, to
            ' retrieve POINT load symbols:
            Dim ptLdSymbs As IList(Of db.Element) = RstUtils.GetInstanceOfClass(doc, GetType(dbst.PointLoadType))
            msg = "Point load type directly:"
            For Each ls1 As db.ElementType In ptLdSymbs
                ' Get one specific param for this symbol (can also change it if needed, like for load objects)
                Dim scaleF As String = RacUtils.GetParameterValue(ls1.Parameter(db.BuiltInParameter.LOAD_ATTR_FORCE_SCALE_FACTOR))
                msg &= Constants.vbCrLf & "  " & ls1.Name & ", Id=" & ls1.Id.IntegerValue.ToString() & ", Force Scale=" & scaleF
            Next ls1
            RacUtils.InfoMsg(msg)
            Return Result.Succeeded
        End Function
    End Class
#End Region ' Lab1_4_LoadSymbols

#Region "Lab1_5_CreateNewPointLoads"
    ''' <summary>
    ''' Lab 1-5 - Create new point loads.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
  <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab1_5_CreateNewPointLoads
        Implements IExternalCommand
        Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As db.ElementSet) As Result Implements IExternalCommand.Execute
            Dim app As UIApplication = commandData.Application
            Dim doc As db.Document = app.ActiveUIDocument.Document
            ' Loads are not standard Families and FamilySymbols:
            ' Id=42618; Class=Symbol; Category=Structural Loads; Name=Point Load 1
            ' Id=42623; Class=Symbol; Category=Structural Loads; Name=Line Load 1
            ' Id=42632; Class=Symbol; Category=Structural Loads; Name=Area Load 1
            ' Id=126296; Class=Symbol; Category=Structural Loads; Name=MS load
            ' Id=129986; Class=Symbol; Category=Structural Loads; Name=MS AL 2
            ' Hence cannot use anything like NewFamilyInstance ...
            ' ... but there are dedicated methods:
            Dim creationDoc As Autodesk.Revit.Creation.Document = app.ActiveUIDocument.Document.Create

            ' REACTION (will be read-only in UI)
            Dim p As New db.XYZ(0, 0, 0)
            Dim f As New db.XYZ(0, 0, 14593.9) ' equals 1 kip
            Dim m As New db.XYZ(0, 0, 0)
            Dim newPointLoadReaction As dbst.PointLoad = creationDoc.NewPointLoad(p, f, m, True, Nothing, Nothing)
            ' This one will have default symbol and no load case

            ' EXTERNAL Force
            Dim newPointLoadExternal As dbst.PointLoad = creationDoc.NewPointLoad(New db.XYZ(5, 5, 0), New db.XYZ(0, 0, -30000), New db.XYZ(0, 0, 0), False, Nothing, Nothing)

            ' Ask user to select SYMBOL for the new Load (loop questions to avoid custom forms)
            Dim ptLdSymbs As IList(Of db.Element) = RstUtils.GetInstanceOfClass(doc, GetType(dbst.PointLoadType))
            Dim gotSymb As Boolean = False
            Do While Not gotSymb
                For Each sym As db.ElementType In ptLdSymbs
                    Select Case WinForms.MessageBox.Show("Use point load type " & sym.Name & "?", "Select load type for the new point load", WinForms.MessageBoxButtons.YesNoCancel)
                        Case WinForms.DialogResult.Cancel
                            Return Result.Cancelled
                        Case WinForms.DialogResult.Yes
                            Dim id As db.ElementId = sym.Id
                            newPointLoadExternal.Parameter(db.BuiltInParameter.ELEM_TYPE_PARAM).Set(id)
                            gotSymb = True
                    End Select
                    If gotSymb Then
                        Exit For
                    End If
                Next sym
            Loop

            ' Ask user to select LOAD CASE for the new Load (loop questions to avoid custom forms)
            Dim ldCases As IList(Of db.Element) = RstUtils.GetInstanceOfClass(doc, GetType(dbst.LoadCase))
            Dim gotCase As Boolean = False
            Do While Not gotCase
                For Each ldCase As dbst.LoadCase In ldCases
                    Select Case WinForms.MessageBox.Show("Assign to load case " & ldCase.Name & "?", "Select load case for the new point load", WinForms.MessageBoxButtons.YesNoCancel)
                        Case WinForms.DialogResult.Cancel
                            Return Result.Cancelled
                        Case WinForms.DialogResult.Yes
                            Dim ldCaseid As db.ElementId = ldCase.Id
                            newPointLoadExternal.Parameter(db.BuiltInParameter.LOAD_CASE_ID).Set(ldCaseid)
                            gotCase = True
                    End Select
                    If gotCase Then
                        Exit For
                    End If
                Next ldCase
            Loop
            Return Result.Succeeded
        End Function
    End Class
#End Region ' Lab1_5_CreateNewPointLoads
End Namespace
