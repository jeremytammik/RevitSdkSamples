#Region "Copyright"
''
'' (C) Copyright 2010 by Autodesk, Inc.
''
'' Permission to use, copy, modify, and distribute this software in
'' object code form for any purpose and without fee is hereby granted,
'' provided that the above copyright notice appears in all copies and
'' that both that copyright notice and the limited warranty and
'' restricted rights notice below appear in all supporting
'' documentation.
''
'' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
'' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
'' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
'' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
'' UNINTERRUPTED OR ERROR FREE.
''
'' Use, duplication, or disclosure by the U.S. Government is subject to
'' restrictions set forth in FAR 52.227-19 (Commercial Computer
'' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
'' (Rights in Technical Data and Computer Software), as applicable.
''
'' Written by M.Harada 
''
#End Region

#Region "Imports"
'' Import the following name spaces in the project properties/references. 
'' Note: VB.NET has a slighly different way of recognizing name spaces than C#. 
'' if you explicitely set them in each .vb file, you will need to specify full name spaces. 

'Imports System
'Imports Autodesk.Revit.DB
'Imports Autodesk.Revit.UI
'Imports Autodesk.Revit.ApplicationServices
'Imports Autodesk.Revit.Attributes '' specific this if you want to save typing for attributes. e.g., 
'Imports Autodesk.Revit.DB.Structure '' added for Lab5. 
Imports RevitIntroVB.ElementFiltering  ''  added for Lab4. 

#End Region

#Region "Description"
''  Revit Intro Lab 5 
''
''  In this lab, you will learn how to create revit models. 
''  To test this, use "DefaultMetric" template. 
'' 
''  Disclaimer: minimum error checking to focus on the main topic. 
'' 
'' MH: my scale(77) 
''-------1---------2---------3---------4---------5---------6---------7-------
''-------1---------2---------3---------4---------5---------6---------
#End Region

''  Element Creation.  
''
''  add Shared parameter here? 
''  importing a family symbol. Maybe do it earlier?   
'' 


<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class ModelCreation
    Implements IExternalCommand

    ''  member variables 
    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Autodesk.Revit.UI.Result _
                            Implements IExternalCommand.Execute

        ''  Get the acess to the top most objects. 
        Dim rvtUIApp As UIApplication = commandData.Application
        Dim rvtUIDoc As UIDocument = rvtUIApp.ActiveUIDocument
        m_rvtApp = rvtUIApp.Application
        m_rvtDoc = rvtUIDoc.Document

        ''  Let's make a simple "house" composed of four walls, a window 
        ''  and a door. 
        CreateHouse()

        Return Result.Succeeded

    End Function

    Sub CreateHouse()

        ''  simply create four walls with rectangular profile. 
        Dim walls As List(Of Wall) = CreateWalls()

        ''  add a door to the second wall 
        AddDoor(walls(0))

        ''  add windows to the rest of the walls. 
        For i As Integer = 1 To 3
            AddWindow(walls(i))
        Next

        ''  (optional) add a roof over the walls' rectangular profile. 
        addRoof(walls)

    End Sub

    ''  There are five override methods for creating walls. 
    ''  We assume you are using metric template, where you have
    ''  "Level 1" and "Level 2"
    ''  cf. Developer Guide page 117 
    '' 
    Function CreateWalls() As List(Of Wall)

        ''  hard coding the size of the house for simplicity 
        Dim width As Double = mmToFeet(10000.0)
        Dim depth As Double = mmToFeet(5000.0)

        ''  get the levels we want to work on. 
        ''  Note: hard coding for simplicity. Modify here you use a different template. 
        Dim level1 As Level = ElementFiltering.FindElement(m_rvtDoc, GetType(Level), "Level 1")
        If level1 Is Nothing Then
            TaskDialog.Show("Revit Intro Lab", "Cannot find (Level 1). Maybe you use a different template? Try with DefaultMetric.rte.")
            Return Nothing
        End If

        Dim level2 As Level = ElementFiltering.FindElement(m_rvtDoc, GetType(Level), "Level 2")
        If level2 Is Nothing Then
            TaskDialog.Show("Revit Intro Lab", "Cannot find (Level 2). Maybe you use a different template? Try with DefaultMetric.rte.")
            Return Nothing
        End If

        ''  set four corner of walls.
        ''  5th point is for combenience to loop through.  
        Dim dx As Double = width / 2.0
        Dim dy As Double = depth / 2.0

        Dim pts As New List(Of XYZ)(5)
        pts.Add(New XYZ(-dx, -dy, 0.0))
        pts.Add(New XYZ(dx, -dy, 0.0))
        pts.Add(New XYZ(dx, dy, 0.0))
        pts.Add(New XYZ(-dx, dy, 0.0))
        pts.Add(pts(0))

        ''  flag for structural wall or not. 
        Dim isStructural As Boolean = False

        ''  save walls we create. 
        Dim walls As New List(Of Wall)(4)

        ''  loop through list of points and define four walls. 
        For i As Integer = 0 To 3
            ''  define a base curve from two points. 
            Dim baseCurve As Line = m_rvtApp.Create.NewLineBound(pts(i), pts(i + 1))
            ''  create a wall using the one of overloaded methods. 
            Dim aWall As Wall = m_rvtDoc.Create.NewWall(baseCurve, level1, isStructural)
            ''  set the Top Constraint to Level 2 
            aWall.Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(level2.Id)
            ''  save the wall.
            walls.Add(aWall)
        Next
        ''  This is important. we need these lines to have shrinkwrap working. 
        m_rvtDoc.Regenerate()
        m_rvtDoc.AutoJoinElements()

        Return walls

    End Function

    ''  add a door to the center of the given wall. 
    ''  cf. Developer Guide p137. NewFamilyInstance() for Doors and Window. 
    '' 
    Sub AddDoor(ByVal hostWall As Wall)

        ''  hard coding the door type we will use. 
        ''  e.g., "M_Single-Flush: 0915 x 2134mm 
        Const doorFamilyName As String = "Single-Flush" ' "M_Single-Flush"
        Const doorTypeName As String = "30"" x 80""" ' "0915 x 2134mm"
        Const doorFamilyAndTypeName As String = doorFamilyName + ": " + doorTypeName

        ''  get the door type to use. 
        Dim doorType As FamilySymbol = _
        ElementFiltering.FindFamilyType(m_rvtDoc, GetType(FamilySymbol), _
                                        doorFamilyName, doorTypeName, BuiltInCategory.OST_Doors)
        If doorType Is Nothing Then
            TaskDialog.Show("Revit Intro Lab", "Cannot find (" + _
            doorFamilyAndTypeName + "). Maybe you use a different template? Try with DefaultMetric.rte.")
        End If

        ''  get the start and end points of the wall. 
        Dim locCurve As LocationCurve = hostWall.Location
        Dim pt1 As XYZ = locCurve.Curve.EndPoint(0)
        Dim pt2 As XYZ = locCurve.Curve.EndPoint(1)
        ''  calculate the mid point. 
        Dim pt As XYZ = (pt1 + pt2) / 2.0

        ''  one more thing - we want to set the reference as a bottom of the wall or level1. 
        Dim idLevel1 As ElementId = hostWall.Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT).AsElementId
        Dim level1 As Level = m_rvtDoc.Element(idLevel1)

        ''  finally, create a door. 
        Dim aDoor As FamilyInstance = m_rvtDoc.Create.NewFamilyInstance( _
        pt, doorType, hostWall, level1, StructuralType.NonStructural)

    End Sub

    ''  add a window to the center of the wall given. 
    ''  cf. Developer Guide p137. NewFamilyInstance() for Doors and Window. 
    ''  Basically the same idea as a door except that we need to set sill hight. 
    '' 
    Sub AddWindow(ByVal hostWall As Wall)

        ''  hard coding the window type we will use. 
        ''  e.g., "M_Fixed: 0915 x 1830mm 
        Const windowFamilyName As String = "Fixed" ' "M_Fixed"
        Const windowTypeName As String = "16"" x 24""" ' "0915 x 1830mm"
        Const windowFamilyAndTypeName As String = windowFamilyName + ": " + windowTypeName
        Dim sillHeight As Double = mmToFeet(915)

        ''  get the door type to use. 
        Dim windowType As FamilySymbol = _
        ElementFiltering.FindFamilyType(m_rvtDoc, GetType(FamilySymbol), _
            windowFamilyName, windowTypeName, BuiltInCategory.OST_Windows)
        If windowType Is Nothing Then
            TaskDialog.Show("Revit Intro Lab", "Cannot find (" + _
            windowFamilyAndTypeName + "). Maybe you use a different template? Try with DefaultMetric.rte.")
        End If

        ''  get the start and end points of the wall. 
        Dim locCurve As LocationCurve = hostWall.Location
        Dim pt1 As XYZ = locCurve.Curve.EndPoint(0)
        Dim pt2 As XYZ = locCurve.Curve.EndPoint(1)
        ''  calculate the mid point. 
        Dim pt As XYZ = (pt1 + pt2) / 2.0

        ''  one more thing - we want to set the reference as a bottom of the wall or level1. 
        Dim idLevel1 As ElementId = hostWall.Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT).AsElementId
        Dim level1 As Level = m_rvtDoc.Element(idLevel1)

        ''  finally create a window. 
        Dim aWindow As FamilyInstance = m_rvtDoc.Create.NewFamilyInstance( _
        pt, windowType, hostWall, level1, StructuralType.NonStructural)

        aWindow.Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM).Set(sillHeight)

    End Sub

    ''  add a roof over the rectangular profile of the walls we created earlier.
    ''
    Sub addRoof(ByVal walls As List(Of Wall))

        ''  hard coding the roof type we will use. 
        ''  e.g., "Basic Roof: Generic - 400mm"  
        Const roofFamilyName As String = "Basic Roof"
        Const roofTypeName As String = "Generic - 9""" ' "Generic - 400mm"
        Const roofFamilyAndTypeName As String = roofFamilyName + ": " + roofTypeName

        ''  find the roof type
        Dim roofType As RoofType = _
        ElementFiltering.FindFamilyType(m_rvtDoc, GetType(RoofType), _
                                roofFamilyName, roofTypeName)
        If roofType Is Nothing Then
            TaskDialog.Show("Revit Intro Lab", "Cannot find (" + _
            roofFamilyAndTypeName + "). Maybe you use a different template? Try with DefaultMetric.rte.")
        End If

        ''  wall thickness to adjust the footprint of the walls
        ''  to the outer most lines. 
        ''  Note: this may not be the best way. 
        ''  but we will live with this for this exercise. 
        Dim wallThickness As Double = _
        walls(0).WallType.CompoundStructure.Layers.Item(0).Thickness
        Dim dt As Double = wallThickness / 2.0
        Dim dts As New List(Of XYZ)(5)
        dts.Add(New XYZ(-dt, -dt, 0.0))
        dts.Add(New XYZ(dt, -dt, 0.0))
        dts.Add(New XYZ(dt, dt, 0.0))
        dts.Add(New XYZ(-dt, dt, 0.0))
        dts.Add(dts(0))

        ''  set the profile from four walls 
        Dim footPrint As New CurveArray()
        For i As Integer = 0 To 3
            Dim locCurve As LocationCurve = walls(i).Location
            Dim pt1 As XYZ = locCurve.Curve.EndPoint(0) + dts(i)
            Dim pt2 As XYZ = locCurve.Curve.EndPoint(1) + dts(i + 1)
            Dim line As Line = m_rvtApp.Create.NewLineBound(pt1, pt2)
            footPrint.Append(line)
        Next

        ''  get the level2 from the wall
        Dim idLevel2 As ElementId = _
        walls(0).Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).AsElementId
        Dim level2 As Level = m_rvtDoc.Element(idLevel2)

        ''  footprint to morel curve mapping  
        Dim mapping As New ModelCurveArray

        ''  create a roof.
        Dim aRoof As FootPrintRoof = _
        m_rvtDoc.Create.NewFootPrintRoof(footPrint, level2, roofType, mapping)

        '' 
        For Each modelCurve As ModelCurve In mapping
            aRoof.DefinesSlope(modelCurve) = True
            aRoof.SlopeAngle(modelCurve) = 0.5
        Next

    End Sub

#Region "Helper Functions"

    ''=============================================
    ''  Helper Functions 
    ''=============================================

    ''   convert millimeter to feet
    '' 
    Function mmToFeet(ByVal mmVal As Double) As Double

        Return mmVal / 304.8 '' * 0.00328;

    End Function

#End Region

End Class
