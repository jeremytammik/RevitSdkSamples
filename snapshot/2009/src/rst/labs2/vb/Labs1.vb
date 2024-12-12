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

    ' List all Load "Admin objects": Natures, Cases, Combinations
    Public Class Lab1_1
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As Autodesk.Revit.ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As Autodesk.Revit.ElementSet) _
        As Autodesk.Revit.IExternalCommand.Result _
        Implements Autodesk.Revit.IExternalCommand.Execute

            Dim app As Revit.Application = commandData.Application

            ' Load NATURES
            Dim ldNatures As ElementSet = RstUtils.GetAllLoadNatures(app)
            Dim sMsg As String = "There are " & ldNatures.Size & " LoadNature objects in the model:" & vbCrLf
            For Each nature As LoadNature In ldNatures
                sMsg += "  " & nature.Name & ", Id=" & nature.Id.Value.ToString & vbCrLf
            Next
            MsgBox(sMsg)

            ' Load CASES
            Dim ldCases As ElementSet = RstUtils.GetAllLoadCases(app)
            sMsg = "There are " & ldCases.Size & " LoadCase objects in the model:" & vbCrLf
            For Each ldCase As LoadCase In ldCases
                Dim catName As String = ""
                Try
                    ' . Category NOT implemented in API for Loadcase class :-( , though the category enums are there (OST_LoadCasesXxx)
                    catName = ldCase.Category.Name
                Catch
                End Try

                sMsg += "  " & ldCase.Name & ", Id=" & ldCase.Id.Value.ToString & ", Category=" & catName & vbCrLf
                ' there seems to be no way to get LoadCase's LoadNature in API
            Next
            MsgBox(sMsg)

            ' Load USAGES (optionally used for Combinations)
            Dim ldUsages As ElementSet = RstUtils.GetAllLoadUsages(app)
            sMsg = "There are " & ldUsages.Size & " LoadUsage objects in the model:" & vbCrLf
            For Each ldUsage As LoadUsage In ldUsages
                sMsg += "  " & ldUsage.Name & ", Id=" & ldUsage.Id.Value.ToString & vbCrLf
            Next
            MsgBox(sMsg)

            ' Load COMBINATIONS
            Dim combs As ElementSet = RstUtils.GetAllLoadCombinations(app)
            sMsg = "There are " & combs.Size & " LoadCombination objects in the model:" & vbCrLf
            For Each comb As LoadCombination In combs
                ' Combinaton properties
                Dim usageNames As String = "[NONE]"
                For iUse As Integer = 0 To comb.NumberOfUsages - 1
                    If iUse = 0 Then
                        usageNames = comb.UsageName(iUse)
                    Else
                        usageNames += ";" & comb.UsageName(iUse)
                    End If
                Next

                sMsg += vbCrLf & "  " & comb.Name & _
                        ",  Id=" & comb.Id.Value.ToString & _
                        "  Type=" & comb.CombinationType & "  TypeIndex=" & comb.CombinationTypeIndex & _
                        "  State=" & comb.CombinationState & "  StateIndex=" & comb.CombinationStateIndex & _
                        "  Usages=" & usageNames & vbCrLf
                sMsg += "  Number of components = " & comb.NumberOfComponents & ":" & vbCrLf
                ' Loop all components' propeties
                Dim iComp As Integer
                For iComp = 0 To comb.NumberOfComponents - 1
                    sMsg += "    Comp.name=" & comb.CombinationCaseName(iComp) & _
                            "  Comp.nature=" & comb.CombinationNatureName(iComp) & _
                            "  Factor=" & comb.Factor(iComp) & vbCrLf
                Next
            Next
            MsgBox(sMsg)

            ' NOTE: Unlike RS2, from RS3 most of this objects are also create-able, see:
            ' Autodesk.Revit.Creation.Document.NewLoad*** methods. To call them, use something like:
            '  app.ActiveDocument.Create.NewLoadCase(...)
            '  app.ActiveDocument.Create.NewLoadCombination(...)
            '  app.ActiveDocument.Create.NewLoadNature(...)
            '  app.ActiveDocument.Create.NewLoadUsage(...)

        End Function

    End Class


    ' List all Loads
    Public Class Lab1_2
        Implements IExternalCommand

        Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute
            Dim app As Revit.Application = commandData.Application

            Dim loads As ElementSet = RstUtils.GetAllLoads(app)
            Dim sMsg As String = "There are " & loads.Size & " Load objects in the model:" & vbCrLf

            Dim sHost As String
            Dim load As LoadBase
            For Each load In loads
                Try
                    sHost = ", Host Id=" & load.HostElement.Id.Value.ToString
                Catch ex As Exception
                    sHost = ""
                End Try
                sMsg += "  " & load.GetType.Name & ", Id=" & load.Id.Value.ToString & sHost & vbCrLf
            Next

            MsgBox(sMsg)

            'We could loop the above collection, but better code another helper for Specific Loads directly
            Dim pointLoads As ElementSet = app.Create.NewElementSet
            Dim lineLoads As ElementSet = app.Create.NewElementSet
            Dim areaLoads As ElementSet = app.Create.NewElementSet
            RstUtils.GetAllSpecificLoads(app, pointLoads, lineLoads, areaLoads)

            ' POINT
            sMsg = pointLoads.Size & " POINT Loads:" & vbCrLf
            Dim ptLd As PointLoad
            For Each ptLd In pointLoads

                ' The following are all specific READ-ONLY properties:
                Dim F As XYZ = ptLd.Force
                Dim M As XYZ = ptLd.Moment
                Dim pt As XYZ = ptLd.Point
                Dim nameLoadCase As String = ptLd.LoadCaseName
                Dim nameLoadCategory As String = ptLd.LoadCategoryName ' current bug in 8.1 - returns nothing
                Dim nameLoadNature As String = ptLd.LoadNatureName
                sMsg += "Id=" & ptLd.Id.Value.ToString & _
                        ": F= " & F.X & "," & F.Y & "," & F.Z & _
                        " M= " & M.X & "," & M.Y & "," & M.Z & _
                        " Pt= " & pt.X & "," & pt.Y & "," & pt.Z & _
                        " LoadCase=" & nameLoadCase & " LoadCat=" & nameLoadCategory & " LoadNature=" & nameLoadNature & vbCrLf

            Next
            MsgBox(sMsg)

            ' LINE
            sMsg = lineLoads.Size & " LINE Loads:" & vbCrLf
            Dim lnLd As LineLoad
            For Each lnLd In lineLoads

                ' The following are *some* READ-ONLY properties:
                Dim F1 As XYZ = lnLd.Force1
                Dim F2 As XYZ = lnLd.Force2
                ' can do similar for Moment1, Moment2...
                Dim ptStart As XYZ = lnLd.StartPoint
                ' similar for EndPoint
                ' LoadxxxName same as for PointLoad (implemented in the base class)

                sMsg += "Id=" & lnLd.Id.Value.ToString & _
                        ": F1= " & F1.X & "," & F1.Y & "," & F1.Z & _
                        ": F2= " & F2.X & "," & F2.Y & "," & F2.Z & _
                        " Start= " & ptStart.X & "," & ptStart.Y & "," & ptStart.Z & _
                        " Uniform=" & lnLd.UniformLoad & " Projected=" & lnLd.ProjectedLoad & vbCrLf

            Next
            MsgBox(sMsg)

            ' AREA
            sMsg = areaLoads.Size & " AREA Loads:" & vbCrLf
            Dim arLd As AreaLoad
            For Each arLd In areaLoads

                ' The following are *some* READ-ONLY properties:
                Dim F1 As XYZ = arLd.Force1

                Dim numLoops As Integer = arLd.NumLoops
                Dim s2 As String = "Num.of Loops=" & numLoops & " Num.of Curves=" & arLd.NumCurves(0)
                Dim iLoop As Integer ' if more than single Loop
                For iLoop = 1 To numLoops - 1
                    s2 += "," & arLd.NumCurves(iLoop)
                Next

                sMsg += "Id=" & arLd.Id.Value.ToString & _
                        ": F1= " & F1.X & "," & F1.Y & "," & F1.Z & s2 & vbCrLf

                ' For curves geometry, see later commands...
            Next
            MsgBox(sMsg)

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class


    ' More detailed info for *selected* loads
    Public Class Lab1_3
        Implements IExternalCommand

        Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute
            Dim app As Revit.Application = commandData.Application

            Dim iter As ElementSetIterator = app.ActiveDocument.Selection.Elements.ForwardIterator
            While iter.MoveNext
                Dim elem As Revit.Element = iter.Current
                If TypeOf elem Is PointLoad Then
                    ListAndModifyPointLoad(CType(elem, PointLoad))
                ElseIf TypeOf elem Is LineLoad Then
                    ListAndModifyLineLoad(CType(elem, LineLoad), app)
                ElseIf TypeOf elem Is AreaLoad Then
                    ListAndModifyAreaLoad(CType(elem, AreaLoad), app)
                End If

            End While
            Return IExternalCommand.Result.Succeeded

        End Function

        Public Sub ListAndModifyPointLoad(ByVal ptLd As PointLoad)
            ' One can access some parameters as read-only class properties/methods,
            '   but to be able to change them and to access all, we need to go via Parameters:
            Dim sMsg As String = "POINT Load Id=" & ptLd.Id.Value.ToString & vbCrLf
            Try

                ' List 2 Params via their display name (locale-dependent!)
                sMsg += "  Is Reaction (via Display Name)= " & RacUtils.GetParamAsString(RacUtils.GetElemParam(ptLd, "Is Reaction")) & vbCrLf 'Int
                sMsg += "  Load Case (via Display Name)= " & RacUtils.GetParamAsString(RacUtils.GetElemParam(ptLd, "Load Case")) & vbCrLf ' Id

                ' Better use BUILT_IN_PARAMS, but have to guess-and-try-and-error to find correct ones!
                'The following 2 give the same as above:
                sMsg += "  LOAD_IS_REACTION BuiltInParameter= " & RacUtils.GetParamAsString(ptLd.Parameter(BuiltInParameter.LOAD_IS_REACTION)) & vbCrLf 'Int
                sMsg += "  LOAD_CASE_ID BuiltInParameter= " & RacUtils.GetParamAsString(ptLd.Parameter(BuiltInParameter.LOAD_CASE_ID)) & vbCrLf ' Id

                ' The following Display one hasn't got one-to-one corresponding Built-in, but basically the same:
                sMsg += "  Orient to (via Display Name)= " & RacUtils.GetParamAsString(RacUtils.GetElemParam(ptLd, "Orient to")) & vbCrLf ' Int
                sMsg += "  LOAD_USE_LOCAL_COORDINATE_SYSTEM BuiltInParameter= " & RacUtils.GetParamAsString(ptLd.Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM)) & vbCrLf '
                Try ' this param was working in RS2, but not accessible in RS3! ... is there in 2008
                    sMsg += "  LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED BuiltInParameter= " & RacUtils.GetParamAsString(ptLd.Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED)) & vbCrLf '
                Catch ex As Exception
                End Try

                ' Scale Fz by factor 2
                Dim paramFz As Parameter = RacUtils.GetElemParam(ptLd, "Fz")
                Dim Fz_old As Double = paramFz.AsDouble
                paramFz.Set(2 * Fz_old)
                sMsg += "  Fz:  OLD=" & Fz_old & " NEW=" & paramFz.AsDouble & vbCrLf

                ' If created by API
                Try  ' this param was working in RS2, but not accessible in RS3!
                    sMsg += "  LOAD_IS_CREATED_BY_API BuiltInParameter= " & RacUtils.GetParamAsString(ptLd.Parameter(BuiltInParameter.LOAD_IS_CREATED_BY_API)) & vbCrLf 'Int
                Catch
                End Try

                Try
                    sMsg += "  ELEM_TYPE_PARAM BuiltInParameter= " & RacUtils.GetParamAsString(ptLd.Parameter(BuiltInParameter.ELEM_TYPE_PARAM)) & vbCrLf
                Catch
                End Try

            Catch ex As Exception
                MsgBox("Error in ListAndModifyPointLoad: " & ex.Message)
            Finally
                MsgBox(sMsg)
            End Try

        End Sub

        Public Sub ListAndModifyLineLoad(ByVal lnLd As LineLoad, ByVal app As Revit.Application)
            ' One can access some parameters as read-only class properties/methods,
            '   but to be able to change them and to access all, we need to go via Parameters:
            Dim sMsg As String = "LINE Load Id=" & lnLd.Id.Value.ToString & vbCrLf
            Try

                ' Again, show how to access the same param via both display name and BUILT_IN_PARAMS
                sMsg += "  Load Case (via Display Name)= " & RacUtils.GetParamAsString(RacUtils.GetElemParam(lnLd, "Load Case")) & vbCrLf ' Id
                sMsg += "  LOAD_CASE_ID BuiltInParameter= " & RacUtils.GetParamAsString(lnLd.Parameter(BuiltInParameter.LOAD_CASE_ID)) & vbCrLf ' Id

                ' Scale Fz1 by factor 2, but this time get it as BUILT_IN_PARAMS
                Dim paramFZ1 As Parameter = lnLd.Parameter(BuiltInParameter.LOAD_LINEAR_FORCE_FZ1)
                Dim FZ1_old As Double = paramFZ1.AsDouble
                paramFZ1.Set(2 * FZ1_old)
                sMsg += "  FZ1:  OLD=" & FZ1_old & " NEW=" & paramFZ1.AsDouble & vbCrLf

                'Check if the Load is attach to a host - DOESN'T WORK! ALWAYS Nothing...
                ' There seems to be NO way to determine if an Line/Area LOAD is HOSTED
                ' In Revit 2008, use the generic load property LoadBase.HostElement
                Dim hostIdParam As Parameter = lnLd.Parameter(BuiltInParameter.HOST_ID_PARAM)
                If Not hostIdParam Is Nothing Then
                    Dim elem As Revit.Element = app.ActiveDocument.Element(hostIdParam.AsElementId)
                    sMsg += "  HOST element ID=" & elem.Id.Value.ToString & " Class=" & elem.GetType.Name & " Name=" & elem.Name & vbCrLf
                Else
                    sMsg += "  NO HOST element" & vbCrLf
                End If

                ' test possible values for Line Load:
                ' ALL 3 are 0 if "Project"
                ' ALL 3 are 1 if either "Workplane" or "Host Workplane"
                sMsg += "  Orient to (via Display Name)= " & RacUtils.GetParamAsString(RacUtils.GetElemParam(lnLd, "Orient to")) & vbCrLf ' Int
                sMsg += "  LOAD_USE_LOCAL_COORDINATE_SYSTEM BuiltInParameter= " & RacUtils.GetParamAsString(lnLd.Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM)) & vbCrLf '
                Try
                    sMsg += "  LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED BuiltInParameter= " & RacUtils.GetParamAsString(lnLd.Parameter(BuiltInParameter.LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED)) & vbCrLf '
                Catch
                End Try

                ' If created by API
                ' If created by API
                Try  ' this param was working in RS2, but not accessible in RS3!
                    sMsg += "  LOAD_IS_CREATED_BY_API BuiltInParameter= " & RacUtils.GetParamAsString(lnLd.Parameter(BuiltInParameter.LOAD_IS_CREATED_BY_API)) & vbCrLf 'Int
                Catch
                End Try

            Catch ex As Exception
                MsgBox("Error in ListAndModifyLineLoad: " & ex.Message)
            Finally
                MsgBox(sMsg)
            End Try

        End Sub

        Public Sub ListAndModifyAreaLoad(ByVal arLd As AreaLoad, ByVal app As Revit.Application)
            ' One can access some parameters as read-only class properties/methods,
            '   but to be able to change them and to access all, we need to go via Parameters:
            Dim sMsg As String = "AREA Load Id=" & arLd.Id.Value.ToString & vbCrLf
            Try

                ' Again, show how to access the same param via both display name and BUILT_IN_PARAMS
                sMsg += "  Load Case (via Display Name)= " & RacUtils.GetParamAsString(RacUtils.GetElemParam(arLd, "Load Case")) & vbCrLf ' Id
                sMsg += "  LOAD_CASE_ID BuiltInParameter= " & RacUtils.GetParamAsString(arLd.Parameter(BuiltInParameter.LOAD_CASE_ID)) & vbCrLf ' Id

                ' Scale Fz1 by factor 2
                Dim paramFZ1 As Parameter = arLd.Parameter(BuiltInParameter.LOAD_AREA_FORCE_FZ1)
                Dim FZ1_old As Double = paramFZ1.AsDouble
                paramFZ1.Set(2 * FZ1_old)
                sMsg += "  FZ1:  OLD=" & FZ1_old & " NEW=" & paramFZ1.AsDouble & vbCrLf

                ' Specifically for AREA Load, there can be more than a single Loop:
                Dim iLoop As Integer
                Dim numLoops As Integer = arLd.NumLoops
                sMsg += vbCrLf & "  Number Of Loops = " & numLoops & vbCrLf
                'Loop all Loops
                For iLoop = 0 To numLoops - 1
                    Dim numCurves As Integer = arLd.NumCurves(iLoop)
                    sMsg += "    Loop " & iLoop + 1 & " has " & numCurves & " curves:" & vbCrLf
                    ' Loop all Curves in this Loop
                    Dim iCurve As Integer
                    For iCurve = 0 To numCurves - 1
                        Dim crv As Curve = arLd.Curve(iLoop, iCurve)

                        If TypeOf crv Is Geometry.Line Then      'LINE

                            Dim line As Line = crv
                            Dim ptS As XYZ = line.EndPoint(0)
                            Dim ptE As XYZ = line.EndPoint(1)

                            sMsg += "      Curve " & iCurve + 1 & " is a LINE:" & _
                             ptS.X & ", " & ptS.Y & ", " & ptS.Z & " ; " & _
                             ptE.X & ", " & ptE.Y & ", " & ptE.Z & vbCrLf

                        ElseIf TypeOf crv Is Geometry.Arc Then      'ARC

                            Dim arc As Arc = crv
                            Dim ptS As XYZ = arc.EndPoint(0)
                            Dim ptE As XYZ = arc.EndPoint(1)
                            Dim r As Double = arc.Radius

                            sMsg += "      Curve " & iCurve + 1 & " is an ARC:" & _
                             ptS.X & ", " & ptS.Y & ", " & ptS.Z & " ; " & _
                             ptE.X & ", " & ptE.Y & ", " & ptE.Z & " ; R=" & r & vbCrLf

                        End If
                    Next

                Next

                ' Reference Points (not sure what the significance is)
                Dim iRefPt As Integer
                Dim numRefPts As Integer = arLd.NumRefPoints
                sMsg += vbCrLf & "  Number Of Ref.Points = " & numRefPts & vbCrLf
                'Loop all Points
                For iRefPt = 0 To numRefPts - 1
                    Dim pt As XYZ = arLd.RefPoint(iRefPt)
                    sMsg += "    RefPt " & iRefPt + 1 & " = " & pt.X & ", " & pt.Y & ", " & pt.Z & vbCrLf
                Next

            Catch ex As Exception
                MsgBox("Error in ListAndModifyAreaLoad: " & ex.Message)
            Finally
                MsgBox(sMsg)
            End Try

        End Sub

    End Class


    ' List all Load Symbols
    Public Class Lab1_4
        Implements IExternalCommand

        Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute
            Dim app As Revit.Application = commandData.Application

            Dim symbs As ElementSet = RstUtils.GetAllLoadSymbols(app)
            Dim sMsg As String = "There are " & symbs.Size & " Load Symbol objects in the model:" & vbCrLf

            Dim ls As Symbol
            For Each ls In symbs
                ' There are no dedicated classes like PointLoadSymbol, LineLoadSymbol or AreaLoadSymbol, so
                '   we need to check the Family name Param!
                Dim famName As String = "?"
                Try  ' this worked in RS2, but failing now in RS3, works fine in 2008
                    famName = ls.Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM).AsString
                Catch
                End Try
                sMsg += "  " & ls.Name & ", Id=" & ls.Id.Value.ToString & " Family=" & famName & vbCrLf
            Next
            MsgBox(sMsg)

            ' We better make utilities (or alternatively single one for all 3 like for Loads) that will get particular Symbols.
            ' For example, only for POINT Load Symbols:
            Dim ptLdSymbs As ElementSet = RstUtils.GetPointLoadSymbols(app)
            sMsg = "Point Load Symbols directly:" & vbCrLf
            For Each ls In ptLdSymbs
                ' Get one specific param for this symbol (can also change it if needed, like for load objects)
                Dim scaleF As String = RacUtils.GetParamAsString(ls.Parameter(BuiltInParameter.LOAD_ATTR_FORCE_SCALE_FACTOR))
                sMsg += "  " & ls.Name & ", Id=" & ls.Id.Value.ToString & " Force Scale=" & scaleF & vbCrLf
            Next
            MsgBox(sMsg)

        End Function
    End Class


    ' Create NEW Load
    Public Class Lab1_5
        Implements IExternalCommand

        Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute
            Dim app As Revit.Application = commandData.Application

            ' Loads are not standard Families and FamilySymbols!!!
            'Id=42618; Class=Symbol; Category=Structural Loads; Name=Point Load 1
            'Id=42623; Class=Symbol; Category=Structural Loads; Name=Line Load 1
            'Id=42632; Class=Symbol; Category=Structural Loads; Name=Area Load 1
            'Id=126296; Class=Symbol; Category=Structural Loads; Name=MS load
            'Id=129986; Class=Symbol; Category=Structural Loads; Name=MS AL 2
            '' Hence cannot use anything like NewFamilyInstance...
            '...but there are DEDICATED METHODS:

            Dim creationDoc As Creation.Document = app.ActiveDocument.Create

            ' REACTION (will be read-only in UI)
            Dim pt As New XYZ(0, 0, 0)
            Dim f As New XYZ(0, 0, 14593.9) ' eq.to 1 kip
            Dim m As New XYZ(0, 0, 0)
            'Dim newPointLoadReaction As PointLoad = creationDoc.NewPointLoad(pt, f, m, True) ' 2008
            Dim newPointLoadReaction As PointLoad = creationDoc.NewPointLoad(pt, f, m, True, Nothing, Nothing) ' 2009 ... todo: test this!
            ' This one will have default Symbol and no Load case

            ' EXTERNAL Force
            'Dim newPointLoadExternal As PointLoad = creationDoc.NewPointLoad(New XYZ(5, 5, 0), New XYZ(0, 0, -30000), New XYZ(0, 0, 0), False) ' 2008
            Dim newPointLoadExternal As PointLoad = creationDoc.NewPointLoad(New XYZ(5, 5, 0), New XYZ(0, 0, -30000), New XYZ(0, 0, 0), False, Nothing, Nothing) ' 2009 ... todo: test this!

            ' doesn't work any longer in RS3!!!
            ' seems ok again in 2008
            '' Ask user to select SYMBOL for the new Load (loop questions to avoid custom forms)
            Dim ptLdSymbs As ElementSet = RstUtils.GetPointLoadSymbols(app)
            Dim gotSymb As Boolean = False
            Do While Not gotSymb
                Dim sym As Symbol
                For Each sym In ptLdSymbs
                    Select Case MessageBox.Show("Use PointLoad Symbol " & sym.Name & "?", _
                                 "Select Symbol for the New Load", _
                                    MessageBoxButtons.YesNoCancel)
                        Case DialogResult.Cancel
                            Return IExternalCommand.Result.Cancelled
                        Case DialogResult.Yes
                            'either of these works to change the Type/Symbol
                            newPointLoadExternal.Parameter(BuiltInParameter.ELEM_TYPE_PARAM).Set(sym.Id)
                            'newPointLoadExternal.Parameter(BuiltInParameter.SYMBOL_ID_PARAM).Set(sym.Id)
                            gotSymb = True
                            Exit For
                    End Select
                Next
            Loop

            ' Ask user to select LOAD CASE for the new Load (loop questions to avoid custom forms)
            ' Load CASES
            Dim ldCases As ElementSet = RstUtils.GetAllLoadCases(app)
            Dim gotCase As Boolean = False
            Do While Not gotCase
                Dim ldCase As LoadCase
                For Each ldCase In ldCases
                    Select Case MessageBox.Show("Assign to Load Case " & ldCase.Name & "?", _
                                    "Select Load Case for the New Load", _
                                    MessageBoxButtons.YesNoCancel)
                        Case DialogResult.Cancel
                            Return IExternalCommand.Result.Cancelled
                        Case DialogResult.Yes
                            newPointLoadExternal.Parameter(BuiltInParameter.LOAD_CASE_ID).Set(ldCase.Id)
                            gotCase = True
                            Exit For
                    End Select
                Next
            Loop

            Return IExternalCommand.Result.Succeeded
        End Function
    End Class


    'Some Built-in params that seem relevant for loads:
    ' (most of them accessible via BuiltIn Param in RS2, but not all any longer in RB3/4!)

    'LOAD_USAGE_NAME
    'LOAD_COMBINATION_STATE
    'LOAD_COMBINATION_TYPE
    'LOAD_COMBINATION_FACTOR
    'LOAD_COMBINATION_NAME
    'LOAD_NATURE_NAME
    'LOAD_CASE_CATEGORY
    'LOAD_CASE_NATURE
    'LOAD_CASE_NUMBER
    'LOAD_CASE_NAME
    'LOAD_ATTR_AREA_FORCE_SCALE_FACTOR
    'LOAD_ATTR_LINEAR_FORCE_SCALE_FACTOR
    'LOAD_ARROW_SEPARATION
    'LOAD_ATTR_MOMENT_SCALE_FACTOR
    'LOAD_ATTR_MOMENT_ARROW_LINE
    'LOAD_ATTR_MOMENT_ARROW_ARC
    'LOAD_ATTR_FORCE_SCALE_FACTOR
    'LOAD_ATTR_FORCE_ARROW_TYPE
    'LOAD_DESCRIPTION
    'LOAD_COMMENTS
    'LOAD_CASE_NATURE_TEXT
    'LOAD_COMMENT_TEXT
    'LOAD_ALL_NON_0_LOADS
    'LOAD_AREA_AREA
    'LOAD_AREA_FORCE_FZ3
    'LOAD_AREA_FORCE_FY3
    'LOAD_AREA_FORCE_FX3
    'LOAD_AREA_FORCE_FZ2
    'LOAD_AREA_FORCE_FY2
    'LOAD_AREA_FORCE_FX2
    'LOAD_AREA_FORCE_FZ1
    'LOAD_AREA_FORCE_FY1
    'LOAD_AREA_FORCE_FX1
    'LOAD_LINEAR_LENGTH
    'LOAD_IS_PROJECTED
    'LOAD_MOMENT_MZ2
    'LOAD_MOMENT_MY2
    'LOAD_MOMENT_MX2
    'LOAD_MOMENT_MZ1
    'LOAD_MOMENT_MY1
    'LOAD_MOMENT_MX1
    'LOAD_LINEAR_FORCE_FZ2
    'LOAD_LINEAR_FORCE_FY2
    'LOAD_LINEAR_FORCE_FX2
    'LOAD_LINEAR_FORCE_FZ1
    'LOAD_LINEAR_FORCE_FY1
    'LOAD_LINEAR_FORCE_FX1
    'LOAD_MOMENT_MZ
    'LOAD_MOMENT_MY
    'LOAD_MOMENT_MX
    'LOAD_FORCE_FZ
    'LOAD_FORCE_FY
    'LOAD_FORCE_FX
    'LOAD_IS_REACTION
    'LOAD_IS_CREATED_BY_API
    'LOAD_IS_UNIFORM
    'LOAD_USE_LOCAL_COORDINATE_SYSTEM_HOSTED
    'LOAD_USE_LOCAL_COORDINATE_SYSTEM
    'LOAD_CASE_ID
End Namespace