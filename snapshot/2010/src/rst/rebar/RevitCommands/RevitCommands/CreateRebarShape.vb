'      .NET Sample
'
'      Copyright (c) 2008 by Autodesk, Inc.
'
'      Permission to use, copy, modify, and distribute this software
'      for any purpose and without fee is hereby granted, provided
'      that the above copyright notice appears in all copies and
'      that both that copyright notice and the limited warranty and
'      restricted rights notice below appear in all supporting
'      documentation.
'
'      AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
'      AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
'      MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
'      DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
'      UNINTERRUPTED OR ERROR FREE.
'
'      Use, duplication, or disclosure by the U.S. Government is subject to
'      restrictions set forth in FAR 52.227-19 (Commercial Computer
'      Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
'      (Rights in Technical Data and Computer Software), as applicable.
'

'
'  Code samples used at AU2008. 
'  DE215-3 Revit Structure API: From Bi-directional Link to Rebar Detailing 
'  by Mikako Harada 
'

'
'  Create a new rebar shape 
'
Imports System
Imports System.IO
'Imports Autodesk.Revit
'Imports Autodesk.Revit.Enums
Imports Autodesk.Revit.Parameters
''Imports Autodesk.Revit.Structural
'Imports Autodesk.Revit.Structural.Enums
Imports Autodesk.Revit.Symbols

Imports RebarShape2 = Autodesk.Revit.Symbols.RebarShape


Public Class RvtCmd_CreateNewRebarShape_SegmentSample
    Implements IExternalCommand

    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As IExternalCommand.Result Implements IExternalCommand.Execute

        m_rvtApp = commandData.Application
        m_rvtDoc = m_rvtApp.ActiveDocument

        '  5 segment sample from SDK (see the illustration in the SDK) Metric.  
        '
        '  A |       | E  _
        '   B \_____/ D   _ H 
        '        C
        '          | |
        '           K

        '  create a newrebarshape. 
        '
        Dim oRebarShape As RebarShape2 = m_rvtDoc.Create.NewRebarShape()
        oRebarShape.Name = "MyRebarSegmentSample"

        '  the real meat of information is under rebarshape definition. 
        '
        Dim shapeDef As RebarShapeDefinitionBySegments = oRebarShape.NewDefinitionBySegments(5) ' # of segment. 

        '  add parameters with default value. 
        Dim def As ExternalDefinition = GetOrCreateSharedParameter("A") ' this is my parameter "A"  
        Dim pA As Parameter = shapeDef.AddParameter(def, 280)

        def = GetOrCreateSharedParameter("B")
        Dim pB As Parameter = shapeDef.AddParameter(def, 453)

        def = GetOrCreateSharedParameter("C")
        Dim pC As Parameter = shapeDef.AddParameter(def, 560)

        def = GetOrCreateSharedParameter("D")
        Dim pD As Parameter = shapeDef.AddParameter(def, 453)

        def = GetOrCreateSharedParameter("E")
        Dim pE As Parameter = shapeDef.AddParameter(def, 280)

        def = GetOrCreateSharedParameter("H")
        Dim pH As Parameter = shapeDef.AddParameter(def, 320)

        def = GetOrCreateSharedParameter("K")
        Dim pK As Parameter = shapeDef.AddParameter(def, 320)

        def = GetOrCreateSharedParameter("O")
        Dim pO As Parameter = shapeDef.AddFormulaParameter(def, "2*K+C") ' here is the one with a formula. search valid formula syntax in product help.

        ' add constraints
        shapeDef.AddConstraintParallelToSegment(0, pA, False, False)
        shapeDef.AddConstraintParallelToSegment(1, pB, False, False)
        shapeDef.AddConstraintParallelToSegment(2, pC, False, False)
        shapeDef.AddConstraintParallelToSegment(3, pD, False, False)
        shapeDef.AddConstraintParallelToSegment(4, pE, False, False)

        shapeDef.SetSegmentFixedDirection(0, 0.0, -1.0)
        shapeDef.SetSegmentFixedDirection(2, 1.0, 0.0)
        shapeDef.SetSegmentFixedDirection(4, 0.0, 1.0)

        shapeDef.AddConstraintToSegment(1, pH, 0.0, -1.0, 1, False, False) ' MH: why SignOfCoordXXX is 0 here? 
        shapeDef.AddConstraintToSegment(1, pK, 1.0, 0.0, -1, False, False)
        shapeDef.AddConstraintToSegment(3, pK, 1.0, 0.0, 1, False, False)
        shapeDef.AddConstraintToSegment(3, pH, 0.0, 1.0, -1, False, False)

        shapeDef.AddBendDefaultRadius(1, 1, RebarShapeBendAngle.Acute) ' second argument: 1 left, -1 right turn. 
        shapeDef.AddBendDefaultRadius(2, 1, RebarShapeBendAngle.Acute)
        shapeDef.AddBendDefaultRadius(3, 1, RebarShapeBendAngle.Acute)
        shapeDef.AddBendDefaultRadius(4, 1, RebarShapeBendAngle.Acute)

        shapeDef.AddListeningDimensionBendToBend(pO, 1.0, 0.0, 0, 0, 4, 1) ' this will add a read-only (gary out) dimention.  

        ' set hooks (optional).  
        oRebarShape.HookAngle(0) = 180
        oRebarShape.HookAngle(1) = 180
        oRebarShape.HookOrientation(0) = Autodesk.Revit.Structural.Enums.RebarHookOrientation.Left
        oRebarShape.HookOrientation(1) = Autodesk.Revit.Structural.Enums.RebarHookOrientation.Left

        ' finally, call commit. without this, it won't show up in the browser. 
        Try
            shapeDef.Commit()
        Catch ex As System.Exception
            MsgBox("failed to commit rebar def: " + ex.ToString)
        End Try

        Dim result As Boolean = shapeDef.CheckDefaultParameterValues(0, 0)
        MsgBox("shapeDef check result = " + result.ToString)

        Return IExternalCommand.Result.Succeeded

    End Function

    Public Function GetOrCreateSharedParameter(ByVal sParamName As String) As ExternalDefinition

        Return RvtRebarShapeHelper.GetOrCreateSharedParameter(m_rvtApp, sParamName)

    End Function

End Class

Public Class RvtCmd_CreateNewRebarShape_CircleSample
    Implements IExternalCommand

    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As IExternalCommand.Result Implements IExternalCommand.Execute

        m_rvtApp = commandData.Application
        m_rvtDoc = m_rvtApp.ActiveDocument

        ' Lapper Circle sample from SDK. Metric  
        '      __
        '     /  \   
        '     \__/
        '          D: diameter  C: circumference  A: lapper arc 

        '  create a newrebarshape. 
        '
        Dim oRebarShape As RebarShape2 = m_rvtDoc.Create.NewRebarShape()
        oRebarShape.Name = "MyRebarCircleSample"

        '  the real meat of information is under rebarshape definition. 
        ' 
        Dim shapeDef As RebarShapeDefinitionByArc = oRebarShape.NewDefinitionByArc()

        ' arc has type to choose from: arc/lappedCircle. here we choose lapper circle. 
        shapeDef.Type = RebarShapeDefinitionByArcType.LappedCircle ' there are two types. the other is Arc. 

        ' add parameters
        Dim def As ExternalDefinition = GetOrCreateSharedParameter("D")
        Dim pD As Parameter = shapeDef.AddParameter(def, 2)

        def = GetOrCreateSharedParameter("C")
        Dim pC As Parameter = shapeDef.AddParameter(def, 3.1415926)

        def = GetOrCreateSharedParameter("A")  ' 
        Dim pA As Parameter = shapeDef.AddParameter(def, 1)

        ' add constraints
        shapeDef.AddConstraintDiameter(pD, RebarShapeArcReferenceType.Internal)
        shapeDef.AddConstraintCircumference(pC, RebarShapeArcReferenceType.Internal)
        shapeDef.AddConstraintArcLength(pA)  ' MH: what is this for?  the length of lapped arc?   

        ' set hook  
        oRebarShape.HookAngle(0) = 90  ' hook at start 
        oRebarShape.HookAngle(1) = 180 ' hook at end 
        oRebarShape.HookOrientation(0) = RebarHookOrientation.Left ' at start 
        oRebarShape.HookOrientation(1) = RebarHookOrientation.Left ' at end 

        ' finally, call commit. without this, it won't show up in the browser. 
        Try
            shapeDef.Commit()
        Catch ex As Exception
            MsgBox("failed to commit rebar def: " + ex.ToString)
        End Try

        Dim result As Boolean = shapeDef.CheckDefaultParameterValues(0, 0)
        MsgBox("shapeDef check result = " + result.ToString)

        Return IExternalCommand.Result.Succeeded

    End Function

    Public Function GetOrCreateSharedParameter(ByVal sParamName As String) As ExternalDefinition

        Return RvtRebarShapeHelper.GetOrCreateSharedParameter(m_rvtApp, sParamName)

    End Function

End Class

Public Class RvtCmd_CreateNewRebarShape_ArcSample1
    Implements IExternalCommand

    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As IExternalCommand.Result Implements IExternalCommand.Execute

        m_rvtApp = commandData.Application
        m_rvtDoc = m_rvtApp.ActiveDocument

        ' Arc sample from SDK. Metric  
        '      ___
        '     /   \  A 
        '       R
        '         R: radius  A: arc length 

        '  create a newrebarshape. 
        '
        Dim oRebarShape As RebarShape2 = m_rvtDoc.Create.NewRebarShape()
        oRebarShape.Name = "MyRebarArcSample1"

        '  the real meat of information is under rebarshape definition. 
        ' 
        Dim shapeDef As RebarShapeDefinitionByArc = oRebarShape.NewDefinitionByArc()

        ' arc has type to choose from: arc/lappedCircle. here we choose lapper circle. 
        shapeDef.Type = RebarShapeDefinitionByArcType.Arc ' there are two types. the other is LappedCircle. 

        ' add parameters
        Dim def As ExternalDefinition = GetOrCreateSharedParameter("R")
        Dim pR As Parameter = shapeDef.AddParameter(def, 80)

        def = GetOrCreateSharedParameter("A")
        Dim pA As Parameter = shapeDef.AddParameter(def, 200)

        ' add constraints
        shapeDef.AddConstraintRadius(pR, RebarShapeArcReferenceType.Internal)
        shapeDef.AddConstraintArcLength(pA) ' MH: why no internal.external choice?  

        ' set hook  
        oRebarShape.HookAngle(0) = 90  ' hook at start 
        oRebarShape.HookAngle(1) = 180 ' hook at end 
        oRebarShape.HookOrientation(0) = RebarHookOrientation.Left ' at start 
        oRebarShape.HookOrientation(1) = RebarHookOrientation.Left ' at end 

        ' finally, call commit. without this, it won't show up in the browser. 
        Try
            shapeDef.Commit()
        Catch ex As Exception
            MsgBox("failed to commit rebar def: " + ex.ToString)
        End Try

        Dim result As Boolean = shapeDef.CheckDefaultParameterValues(0, 0)
        MsgBox("shapeDef check result = " + result.ToString)

        Return IExternalCommand.Result.Succeeded

    End Function

    Public Function GetOrCreateSharedParameter(ByVal sParamName As String) As ExternalDefinition

        Return RvtRebarShapeHelper.GetOrCreateSharedParameter(m_rvtApp, sParamName)

    End Function

End Class

Public Class RvtCmd_CreateNewRebarShape_ArcSample2
    Implements IExternalCommand

    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As IExternalCommand.Result Implements IExternalCommand.Execute

        m_rvtApp = commandData.Application
        m_rvtDoc = m_rvtApp.ActiveDocument

        ' Arc sample from SDK. Metric  
        '      _____
        '     /     \  |H 
        '     <----->  
        '        K         K: chord length  H: sagittal length  

        '  create a newrebarshape. 
        '
        Dim oRebarShape As RebarShape2 = m_rvtDoc.Create.NewRebarShape()
        oRebarShape.Name = "MyRebarArcSample2"

        '  the real meat of information is under rebarshape definition. 
        ' 
        Dim shapeDef As RebarShapeDefinitionByArc = oRebarShape.NewDefinitionByArc()

        ' arc has type to choose from: arc/lappedCircle. here we choose lapper circle. 
        shapeDef.Type = RebarShapeDefinitionByArcType.Arc ' there are two types. the other is LappedCircle. 

        ' add parameters
        Dim def As ExternalDefinition = GetOrCreateSharedParameter("K")
        Dim pK As Parameter = shapeDef.AddParameter(def, 600)

        def = GetOrCreateSharedParameter("H")
        Dim pH As Parameter = shapeDef.AddParameter(def, 200)

        ' add constraints
        shapeDef.AddConstraintChordLength(pK) ' MH: why no internal/external choice?  
        shapeDef.AddConstraintSagittaLength(pH)  ' MH: ditto  

        ' set hook (optional) 
        oRebarShape.HookAngle(0) = 90  ' hook at start 
        oRebarShape.HookAngle(1) = 180 ' hook at end 
        oRebarShape.HookOrientation(0) = RebarHookOrientation.Left ' at start 
        oRebarShape.HookOrientation(1) = RebarHookOrientation.Left ' at end 

        ' finally, call commit. without this, it won't show up in the browser. 
        Try
            shapeDef.Commit()
        Catch ex As Exception
            MsgBox("failed to commit rebar def: " + ex.ToString)
        End Try

        Dim result As Boolean = shapeDef.CheckDefaultParameterValues(0, 0)
        MsgBox("shapeDef check result = " + result.ToString)

        Return IExternalCommand.Result.Succeeded

    End Function

    Public Function GetOrCreateSharedParameter(ByVal sParamName As String) As ExternalDefinition

        Return RvtRebarShapeHelper.GetOrCreateSharedParameter(m_rvtApp, sParamName)

    End Function

End Class

Public Class RvtCmd_CreateNewRebarShape_Test
    Implements IExternalCommand

    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As IExternalCommand.Result Implements IExternalCommand.Execute

        m_rvtApp = commandData.Application
        m_rvtDoc = m_rvtApp.ActiveDocument

        ' (1) let's first create a simplest rebar shape: a straight line parallel to the x-axis.  
        '
        '     ___________
        '    |<--- A --->| A
        '    A0(default) = 50cm (= 1.64041994750657ft)  
        ' 
        Dim A0 As Double = 1.64041994750657 ' default value 

        '  create a newrebarshape. 
        '
        Dim oRebarShape As RebarShape2 = m_rvtDoc.Create.NewRebarShape()
        oRebarShape.Name = "Simple"

        '  the real meat of information is under rebarshape definition. 
        '  let's create it. here we have a single straight line that has one line segment. 
        ' 
        Dim shapeDef As RebarShapeDefinitionBySegments = oRebarShape.NewDefinitionBySegments(5) ' # of segment. 

        ' define a parameter and label our one and only one segment with parameter "A".
        ' 
        ' do the same as the segment sample 
        ' add parameters first
        Dim def As ExternalDefinition = GetOrCreateSharedParameter("A") ' this is my parameter "A"  
        Dim pA As Parameter = shapeDef.AddParameter(def, 280)

        def = GetOrCreateSharedParameter("B")
        Dim pB As Parameter = shapeDef.AddParameter(def, 453)

        def = GetOrCreateSharedParameter("C")
        Dim pC As Parameter = shapeDef.AddParameter(def, 560)

        def = GetOrCreateSharedParameter("D")
        Dim pD As Parameter = shapeDef.AddParameter(def, 453)

        def = GetOrCreateSharedParameter("E")
        Dim pE As Parameter = shapeDef.AddParameter(def, 280)

        def = GetOrCreateSharedParameter("H")
        Dim pH As Parameter = shapeDef.AddParameter(def, 320)

        def = GetOrCreateSharedParameter("K")
        Dim pK As Parameter = shapeDef.AddParameter(def, 320)

        def = GetOrCreateSharedParameter("O")
        Dim pO As Parameter = shapeDef.AddFormulaParameter(def, "2*K+C")

        ' add constraints
        shapeDef.AddConstraintParallelToSegment(0, pA, False, False)
        shapeDef.AddConstraintParallelToSegment(1, pB, False, False)
        shapeDef.AddConstraintParallelToSegment(2, pC, False, False)
        shapeDef.AddConstraintParallelToSegment(3, pD, False, False)
        shapeDef.AddConstraintParallelToSegment(4, pE, False, False)

        shapeDef.SetSegmentFixedDirection(0, 0.0, -1.0)
        shapeDef.SetSegmentFixedDirection(2, 1.0, 0.0)
        shapeDef.SetSegmentFixedDirection(4, 0.0, 1.0)

        shapeDef.AddConstraintToSegment(1, pH, 0.0, -1.0, 0, False, False) ' MH: why SignOfCoordXXX is 0 here? 
        shapeDef.AddConstraintToSegment(1, pK, 1.0, 0.0, 0, False, False)
        shapeDef.AddConstraintToSegment(3, pK, 1.0, 0.0, 0, False, False)
        shapeDef.AddConstraintToSegment(3, pH, 0.0, 1.0, 0, False, False)

        shapeDef.AddBendDefaultRadius(1, 1, RebarShapeBendAngle.Acute) ' second argument: 1 left, -1 right turn. 
        shapeDef.AddBendDefaultRadius(2, 1, RebarShapeBendAngle.Acute)
        shapeDef.AddBendDefaultRadius(3, 1, RebarShapeBendAngle.Acute)
        shapeDef.AddBendDefaultRadius(4, 1, RebarShapeBendAngle.Acute)

        shapeDef.AddListeningDimensionBendToBend(pO, 1.0, 0.0, 0, 0, 4, 1)

        ' set hook (come back later). 

        oRebarShape.HookAngle(0) = 180
        oRebarShape.HookAngle(1) = 180
        oRebarShape.HookOrientation(0) = RebarHookOrientation.Left
        oRebarShape.HookOrientation(1) = RebarHookOrientation.Left

        ' finally, call commit. without this, it won't show up in the browser. 
        Try
            shapeDef.Commit()
        Catch ex As Exception
            MsgBox("failed to commit rebar def: " + ex.ToString)
        End Try

        Dim result As Boolean = shapeDef.CheckDefaultParameterValues(0, 0)
        MsgBox("sherDef result = " + result.ToString)

        Return IExternalCommand.Result.Succeeded

    End Function

    Public Function GetOrCreateSharedParameter(ByVal sParamName As String) As ExternalDefinition

        Return RvtRebarShapeHelper.GetOrCreateSharedParameter(m_rvtApp, sParamName)

    End Function

End Class

Public Class RvtCmd_CreateNewRebarShape_Simple
    Implements IExternalCommand

    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As IExternalCommand.Result Implements IExternalCommand.Execute

        m_rvtApp = commandData.Application
        m_rvtDoc = m_rvtApp.ActiveDocument

        ' (1) let's first create a simplest rebar shape: a straight line parallel to the x-axis.  
        '
        '     ___________
        '    |<--- A --->| A
        '    A0(default) = 50cm (= 1.64041994750657ft)  
        ' 

        '  create a newrebarshape. 
        '
        Dim oRebarShape As RebarShape2 = m_rvtDoc.Create.NewRebarShape()
        oRebarShape.Name = "MyRebarSimple"

        '  the real meat of information is under rebarshape definition. 
        ' 
        Dim shapeDef As RebarShapeDefinitionBySegments = oRebarShape.NewDefinitionBySegments(1) ' 1 means one segment.

        '  add parameters with default value. 
        Dim def As ExternalDefinition = GetOrCreateSharedParameter("A") ' this is my parameter "A"  
        Dim pA As Parameter = shapeDef.AddParameter(def, 50)

        ' add constraints
        shapeDef.AddConstraintParallelToSegment(0, pA, False, False)

        shapeDef.SetSegmentFixedDirection(0, 1.0, 0.0)

        ' set hook (optional) 
        oRebarShape.HookAngle(0) = 90  ' hook at start 
        oRebarShape.HookAngle(1) = 180 ' hook at end 
        oRebarShape.HookOrientation(0) = RebarHookOrientation.Left ' at start 
        oRebarShape.HookOrientation(1) = RebarHookOrientation.Left ' at end 

        ' finally, call commit. without this, it won't show up in the browser. 
        Try
            shapeDef.Commit()
        Catch ex As Exception
            MsgBox("failed to commit rebar def: " + ex.ToString)
        End Try

        Dim result As Boolean = shapeDef.CheckDefaultParameterValues(0, 0)
        MsgBox("shapeDef check result = " + result.ToString)

        Return IExternalCommand.Result.Succeeded

    End Function

    Public Function GetOrCreateSharedParameter(ByVal sParamName As String) As ExternalDefinition

        Return RvtRebarShapeHelper.GetOrCreateSharedParameter(m_rvtApp, sParamName)

    End Function

End Class

Public Class RvtCmd_CreateNewRebarShape_LShape
    Implements IExternalCommand

    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As IExternalCommand.Result Implements IExternalCommand.Execute

        m_rvtApp = commandData.Application
        m_rvtDoc = m_rvtApp.ActiveDocument

        '  L-shaped rebar shape 
        '
        '     |
        '   A |
        '     |________
        ' 
        '         B 

        '  create a newrebarshape. 
        '
        Dim oRebarShape As RebarShape2 = m_rvtDoc.Create.NewRebarShape()
        oRebarShape.Name = "MyRebarLShape"

        '  the real meat of information is under rebarshape definition. 
        ' 
        Dim shapeDef As RebarShapeDefinitionBySegments = oRebarShape.NewDefinitionBySegments(2) ' # of segments.

        '  add parameters with default value. 
        Dim def As ExternalDefinition = GetOrCreateSharedParameter("A") ' this is my parameter "A"  
        Dim pA As Parameter = shapeDef.AddParameter(def, 50)

        def = GetOrCreateSharedParameter("B")
        Dim pB As Parameter = shapeDef.AddParameter(def, 50)

        ' add constraints
        shapeDef.AddConstraintParallelToSegment(0, pA, False, False)
        shapeDef.AddConstraintParallelToSegment(1, pB, False, False)

        shapeDef.SetSegmentFixedDirection(0, 0.0, -1.0)
        shapeDef.SetSegmentFixedDirection(1, 1.0, 0.0)

        shapeDef.AddBendDefaultRadius(1, 1, RebarShapeBendAngle.Right) ' second argument: 1 left, -1 right turn. third arg. right=90degree

        ' set hook (optional) 
        oRebarShape.HookAngle(0) = 90  ' hook at start 
        oRebarShape.HookAngle(1) = 180 ' hook at end 
        oRebarShape.HookOrientation(0) = RebarHookOrientation.Left ' at start 
        oRebarShape.HookOrientation(1) = RebarHookOrientation.Left ' at end 

        ' finally, call commit. without this, it won't show up in the browser. 
        Try
            shapeDef.Commit()
        Catch ex As Exception
            MsgBox("failed to commit rebar def: " + ex.ToString)
        End Try

        Dim result As Boolean = shapeDef.CheckDefaultParameterValues(0, 0)
        MsgBox("shapeDef check result = " + result.ToString)

        Return IExternalCommand.Result.Succeeded

    End Function

    Public Function GetOrCreateSharedParameter(ByVal sParamName As String) As ExternalDefinition

        Return RvtRebarShapeHelper.GetOrCreateSharedParameter(m_rvtApp, sParamName)

    End Function

End Class

Public Class RvtCmd_CreateNewRebarShape_AngledL
    Implements IExternalCommand

    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As IExternalCommand.Result Implements IExternalCommand.Execute

        m_rvtApp = commandData.Application
        m_rvtDoc = m_rvtApp.ActiveDocument

        '  angled line rebar shape 
        ' 
        '      |
        '   A  |      ___
        '       \      C
        '      B \    ___
        '      
        '      |D| 

        '  create a newrebarshape. 
        '
        Dim oRebarShape As RebarShape2 = m_rvtDoc.Create.NewRebarShape()
        oRebarShape.Name = "MyRebarAngledL"

        '  the real meat of information is under rebarshape definition. 
        ' 
        Dim shapeDef As RebarShapeDefinitionBySegments = oRebarShape.NewDefinitionBySegments(2) ' # of segments.

        '  add parameters with default value. 
        Dim def As ExternalDefinition = GetOrCreateSharedParameter("A") ' this is my parameter "A"  
        Dim pA As Parameter = shapeDef.AddParameter(def, 50)

        def = GetOrCreateSharedParameter("B")
        Dim pB As Parameter = shapeDef.AddParameter(def, 50)

        def = GetOrCreateSharedParameter("C")
        Dim pC As Parameter = shapeDef.AddParameter(def, 40)

        def = GetOrCreateSharedParameter("D")
        Dim pD As Parameter = shapeDef.AddParameter(def, 30)

        ' add constraints
        shapeDef.AddConstraintParallelToSegment(0, pA, False, False)
        shapeDef.AddConstraintParallelToSegment(1, pB, False, False)

        shapeDef.SetSegmentFixedDirection(0, 0.2, -1.0)

        shapeDef.AddConstraintToSegment(1, pC, 0.0, -1.0, 1, False, False) ' MH: SignOfCoordXXX ? 
        shapeDef.AddConstraintToSegment(1, pD, 1.0, 0.0, -1, False, False)

        shapeDef.AddBendDefaultRadius(1, 1, RebarShapeBendAngle.Acute) ' second argument: 1 left, -1 right turn. 

        ' set hook (optional) 
        oRebarShape.HookAngle(0) = 90  ' hook at start 
        oRebarShape.HookAngle(1) = 180 ' hook at end 
        oRebarShape.HookOrientation(0) = RebarHookOrientation.Left ' at start 
        oRebarShape.HookOrientation(1) = RebarHookOrientation.Left ' at end 

        ' finally, call commit. without this, it won't show up in the browser. 
        Try
            shapeDef.Commit()
        Catch ex As Exception
            MsgBox("failed to commit rebar def: " + ex.ToString)
        End Try

        Dim result As Boolean = shapeDef.CheckDefaultParameterValues(0, 0)
        MsgBox("shapeDef check result = " + result.ToString)

        Return IExternalCommand.Result.Succeeded

    End Function

    Public Function GetOrCreateSharedParameter(ByVal sParamName As String) As ExternalDefinition

        Return RvtRebarShapeHelper.GetOrCreateSharedParameter(m_rvtApp, sParamName)

    End Function

End Class


'
'  Helper functions specific to rebar shape command. 
'
Public Class RvtRebarShapeHelper

    '
    '  get or create a shared parameter with the given name. parameter type is already "Length" here. 
    '
    Public Shared Function GetOrCreateSharedParameter(ByVal rvtApp As Application, ByVal sParamName As String) As ExternalDefinition

        ' hard coding for this specific example for simplisity. 
        Const cSharedParamFilePath As String = "C:\tmp\MyRebarShapeParams.txt"
        Const cSharedParamGroupName As String = "Rebar Shape Params MH"

        ' Get the current Shared Params Definition File. If there is none currently set, this will create one with the given name. 
        Dim sharedParamsFile As DefinitionFile = RvtSharedParamUtils.GetOrCreateSharedParamsFile(rvtApp, cSharedParamFilePath)
        If (sharedParamsFile Is Nothing) Then
            MsgBox("Error in getting the Shared Params File?")
            Return Nothing
        End If

        ' Get or Create the Shared Params Group
        Dim sharedParamsGroup As Parameters.DefinitionGroup = RvtSharedParamUtils.GetOrCreateSharedParamsGroup(sharedParamsFile, cSharedParamGroupName)
        If (sharedParamsGroup Is Nothing) Then
            MsgBox("Error in getting the Shared Params Group?")
            Return Nothing
        End If

        ' Get or Create the Shared Params Definition
        Dim paramDef As Parameters.Definition = RvtSharedParamUtils.GetOrCreateSharedParamsDefinition( _
         sharedParamsGroup, ParameterType.Length, sParamName, True) ' Note: we are using parameter type length here. 
        If (paramDef Is Nothing) Then
            MsgBox("Error in creating 'API Added' parameter?")
            Return Nothing
        End If

        Return paramDef

    End Function

End Class

'================================================================
'  Helper functions for shared parameters. general purpose.  
'================================================================
Public Class RvtSharedParamUtils

#Region "Helper functions for shared parameters"

    ''' <summary>
    ''' Helper function to get shared parameters file.
    ''' </summary>
    Public Shared Function GetOrCreateSharedParamsFile(ByVal app As Application, ByVal sSharedParamFilePath As String) As Parameters.DefinitionFile

        ' Get current shared params file name if any. (note: Revit can have only one shared parameter file. So we'll use the current one if any.) 
        Dim sharedParamsFileName As String
        Try
            sharedParamsFileName = app.Options.SharedParametersFilename
        Catch
            MsgBox("No Shared params file set !?")
            Return Nothing
        End Try

        ' if no shard params file name, then set it with our new name.  
        If "" = sharedParamsFileName Then
            Dim fullPath As String = sSharedParamFilePath

            Dim stream As StreamWriter
            stream = New StreamWriter(fullPath)
            stream.Close()

            app.Options.SharedParametersFilename = fullPath
            sharedParamsFileName = app.Options.SharedParametersFilename
        End If

        ' Get the current file object and return it
        Dim sharedParametersFile As Autodesk.Revit.Parameters.DefinitionFile
        Try
            sharedParametersFile = app.OpenSharedParameterFile
        Catch
            ' MH: for example, you may come here if a shared param file is specified already but is it erased. 
            ' not sure of the user's intention. So we'll leave the decision to the user to choose the shared param. 
            ' 
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
    ''' Helper function to get shared params definition.
    ''' </summary>
    Public Shared Function GetOrCreateSharedParamsDefinition(ByVal defGroup As Parameters.DefinitionGroup, _
     ByVal defType As Parameters.ParameterType, ByVal defName As String, ByVal visible As Boolean) As Parameters.Definition

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
    Shared Function SharedParamGUID(ByVal app As Application, ByVal defGroup As String, ByVal defName As String) As Guid

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

    ' parameters in the rebar shape must be shared parameters. find or create it for the given name. 
    '
    Public Shared Function getSharedParameter(ByVal rvtApp As Application, ByVal paramName As String) As ExternalDefinition

        '  hard coding the file name and pass for simplisity.
        Const cFileName As String = "C:\tmp\MyRebarShapeParams.txt"
        Const cGroupName As String = "My Rebar Shape Params"

        'Dim fileName As String = Assembly.GetExecutingAssembly().Location + "." + kFileName

        '  (1) set the shared parameter file. 
        '   first check to see if we have a shared parameter file for us to use alaredy. 
        '  (note: Revit can use only one shared parameter. We may be using somebody else's shared parameter if it is already in use.) 
        '   not sure while loop is a save way to this. But no method to check if share param file is already there. 
        ' 
        Dim defFile As DefinitionFile = Nothing
        While (defFile Is Nothing)
            Try
                defFile = rvtApp.OpenSharedParameterFile()
            Catch ex As Exception

                '  if we don't see a shared parameter in use, make one for ourselves.  
                '  if the file does not exisit, create it.  
                MsgBox("error in opensharedparameterfile: " + ex.ToString)
                If Not (File.Exists(cFileName)) Then
                    File.Create(cFileName)
                End If
                rvtApp.Options.SharedParametersFilename = cFileName

            End Try
        End While

        ' if we come here, we have a shared parameter file opened for us to use.

        ' (2)  set the shared parameters definition group 
        ' 

        ' do we have it? if not, create one. 
        Dim defGroup As DefinitionGroup = defFile.Groups.Item(cGroupName)
        If defGroup Is Nothing Then
            defGroup = defFile.Groups.Create(cGroupName)
        End If

        ' (3)  finally, set the parameter with the given name. 
        '
        Dim paramDef As ExternalDefinition = defGroup.Definitions.Item(paramName)
        If paramDef Is Nothing Then
            paramDef = defGroup.Definitions.Create(paramName, ParameterType.Length)
        End If

        ' finally, return the shared parameter we can use.  
        Return paramDef

    End Function


End Class