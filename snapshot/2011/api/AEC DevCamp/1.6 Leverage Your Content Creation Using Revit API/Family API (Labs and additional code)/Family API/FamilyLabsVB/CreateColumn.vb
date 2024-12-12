#Region "Copyright"
''
'' (C) Copyright 2009-2010 by Autodesk, Inc.
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
#End Region

#Region "Imports"
'' Import the following name spaces in the project properties/references. 
'' Note: VB.NET has a slighly different way of recognizing name spaces than C#. 
'' if you explicitely set them in each .vb file, you will need to specify full name spaces. 

'Imports System.Linq  
'Imports Autodesk.Revit
'Imports Autodesk.Revit.DB
'Imports Autodesk.Revit.UI
'Imports Autodesk.Revit.ApplicationServices

#End Region

#Region "Description"
''  
''  AEC DevCamp Family API presentation. 
''  This code is similar to the Labs, but re-written to follow Best Practice procudures for learning purposes. 
'' 
#End Region

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class RvtCmd_CreateColumn
    Implements IExternalCommand

    ''  member variables 
    Dim m_rvtUIApp As UIApplication
    Dim m_rvtUIDoc As UIDocument
    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document
    Dim m_familyMgr As FamilyManager

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Autodesk.Revit.UI.Result _
                            Implements Autodesk.Revit.UI.IExternalCommand.Execute

        ''  Get the access to the top most objects. 
        ''  (we may not use them all in this specific lab. just to show that the way we access is same as in the project.) 
        ''  
        m_rvtUIApp = commandData.Application
        m_rvtUIDoc = m_rvtUIApp.ActiveUIDocument
        m_rvtApp = m_rvtUIApp.Application
        m_rvtDoc = m_rvtUIDoc.Document
        m_familyMgr = m_rvtDoc.FamilyManager '' specific to family context 

        ''  (1) Plan (insertion point, patametric origin). 
        ''  like in the UI, you will still need to plan. choose a right template

        If Not ValidateDocument(m_rvtDoc) Then
            Return Result.Failed
        End If

        ''  (2) add reference planes
        AddReferencePlanes()

        ''  (3.1) add parameters
        AddParameters()

        ''  (3.2) add dimensions
        AddDimensions()

        ''  (5) add types
        AddTypes()

        ''  (7.1) add geometry 
        AddGeometrySolid()

        ''  (7.2) add another geometry 
        AddGeometryModelLine()

        ''  (7.3) add another geometry 
        AddGeometrySymbolicLines()

        ''  finally return 
        Return Result.Succeeded

    End Function

    '' ============================================
    ''  1. Plan. 
    '' ============================================
    ''  Like with UI, we will still need to use the right template to start with. 
    ''  Using API, you can check if the document dontext is family editor.  
    ''  Note: you can also contorl the visibility of a command in the addin manifest, too. 
    ''  

    Function ValidateDocument(ByVal rvtDoc As Document) As Boolean

        ''  our command works in the context of family editor only 
        If Not rvtDoc.IsFamilyDocument Then
            TaskDialog.Show("Family API", "This command works only in the family editor.")
            Return False
        End If

        ''  check if we have a right template 
        Dim ownerFamily As Family = rvtDoc.OwnerFamily
        If ownerFamily Is Nothing Then
            TaskDialog.Show("Family API", "This document does not have Owner Family.")
            Return False
        End If

        ''  check the family category of this document         
        ''  we assume that we want to create a column. Hard-coding for simplicity. 
        Dim catColumn As Category = rvtDoc.Settings.Categories.Item(BuiltInCategory.OST_Columns)
        If Not ownerFamily.FamilyCategory.Id.Equals(catColumn.Id) Then
            TaskDialog.Show("Family API", "The category of this document does not match the context of this commands. Please open Metric Column.rft")
        End If

        ''  Note: changing the category through API is not possible as of Revit 2011. (possible in the UI). 
        ''  You will need to prepare a template before hand.  
        ''  Revit SPR #187735  API wish: change family category. 

        ''  if we come here, we should have a right template
        Return True

    End Function

    '' ============================================
    ''   2. Add Reference Planes 
    '' ============================================

#Region "Reference Planes"

    Sub AddReferencePlanes()

        ''
        ''  we are defining a simple L-shaped profile like the following:
        ''
        ''  5 tw 4
        ''   +-+
        ''   | | 3          h = height
        '' d | +---+ 2
        ''   +-----+ td
        ''  0        1
        ''  6  w
        ''
        '' 
        ''  we want to add ref planes along (1) 2-3 and (2)3-4.
        ''  Name them "OffsetH" and "OffsetV" respectively. (H for horizontal, V for vertical).
        ''

        '' (1) add a horizonal reference plane 2-3.        
        AddReferencePlane_HorizontalOffset()

        '' (2) add a vertical reference plane. 3-4 
        AddReferencePlane_VerticalOffset()

    End Sub

    Sub AddReferencePlane_HorizontalOffset()

        '' thickness or offset value. Hard-coding for simplicity.
        Dim td As Double = Utils.mmToFeet(150) '' if you are using metric, convert it. 
        'Dim td As Double = 0.5 '' in feet 

        '' (1) add a horizonal ref plane 2-3.
        ''

        ''  get a plan view
        Dim pViewPlan As View = Utils.FindElement(m_rvtDoc, GetType(ViewPlan), "Lower Ref. Level")

        ''  we have predefined ref plane: front/back/left/right
        ''  get the ref plane at front, which is aligned to line 2-3
        Dim refFront As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Front")

        ''  get the bubble and free ends from front ref plane and offset by td.
        Dim p1 As XYZ = refFront.BubbleEnd
        Dim p2 As XYZ = refFront.FreeEnd
        Dim pt1 As New XYZ(p1.X, p1.Y + td, p1.Z)
        Dim pt2 As New XYZ(p2.X, p2.Y + td, p2.Z)

        ''  create a reference plane
        Dim refPlane As ReferencePlane = m_rvtDoc.FamilyCreate.NewReferencePlane(pt1, pt2, XYZ.BasisZ, pViewPlan)
        refPlane.Name = "OffsetH"

    End Sub

    Sub AddReferencePlane_VerticalOffset()

        '' offset value Hard-coding for simplicity.
        Dim tw As Double = Utils.mmToFeet(150)
        'Dim tw As Double = 0.5  '' in feet 

        ''  get a plan view
        Dim pViewPlan As View = Utils.FindElement(m_rvtDoc, GetType(ViewPlan), "Lower Ref. Level")

        ''  find the ref plane at left, which is aligned to line 3-4
        Dim refLeft As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Left")

        ''  get the bubble and free ends from front ref plane and offset by tw.
        Dim p1 As XYZ = refLeft.BubbleEnd
        Dim p2 As XYZ = refLeft.FreeEnd
        Dim pt1 As New XYZ(p1.X + tw, p1.Y, p1.Z)
        Dim pt2 As New XYZ(p2.X + tw, p2.Y, p2.Z)

        ''  create a reference plane
        Dim refPlane As ReferencePlane = m_rvtDoc.FamilyCreate.NewReferencePlane(pt1, pt2, XYZ.BasisZ, pViewPlan)
        refPlane.Name = "OffsetV"

    End Sub

#End Region

#Region "Parameters"

    '' ============================================
    ''   (3.1) add parameters
    '' ============================================
    Sub AddParameters()

        ''  (1)  add dimensional parameters Tw and Td.
        AddParameter_Tw()
        AddParameter_Td()

        ''  (2)  add a parameter for material finish
        ''       we are adding material parameter in addMaterials function. See addMaterials for the actual implementation.
        AddParameter_Material()

    End Sub

    Sub AddParameter_Tw()

        ''  parameter group for Dimension is PG_GEOMETRY in API.
        ''  for simplicity, we assume this is a length dimention in our context. 
        ''  the 4th argument indicates if this is an instance parameter.  
        '' 
        Dim paramTw As FamilyParameter = _
            m_familyMgr.AddParameter("Tw", BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length, False)

        ''  give initial values. 
        Dim tw As Double = Utils.mmToFeet(150.0) '' in metric 
        'Dim tw As Double = 0.5  '' in feet 
        m_familyMgr.Set(paramTw, tw)

        ''  if you want, you can also add a formula. 
        ''  here we set Tw as one fourth of Width. 
        m_familyMgr.SetFormula(paramTw, "Width / 4.0") '' Width is the parameter we have already. 

    End Sub

    Sub AddParameter_Td()

        ''  parameter group for Dimension is PG_GEOMETRY in API.
        ''  for simplicity, we assume this is a length dimention in our context. 
        ''  the 4th argument indicates if this is an instance parameter.  
        '' 
        Dim paramTd As FamilyParameter = _
            m_familyMgr.AddParameter("Td", BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length, False)

        ''  give initial values. 
        Dim td As Double = Utils.mmToFeet(150.0) '' in metric 
        'Dim td As Double = 0.5  '' in feet 
        m_familyMgr.Set(paramTd, td)

        ''  if you want, you can also add a formula.
        ''  here we set Td as one fourth of Depth. 
        m_familyMgr.SetFormula(paramTd, "Depth / 4.0") '' Depth is the parameter we have already. 

    End Sub

    Sub AddParameter_Material()

        ''  add a parameter for material finish under "Materials and Finishes" 
        ''  this time, we define instance parameter so that we can change it at the instance level.
        ''  you will see "(default)" after the name if it is an instance parameter. 

        Dim param As FamilyParameter = _
            m_familyMgr.AddParameter("Column Finish", BuiltInParameterGroup.PG_MATERIALS, ParameterType.Material, True)

        ' ''  find a material "Glass"
        ' ''  if you do not specify, the default will be <By Category> 
        ' '' 
        'Dim pMat As Material = Utils.FindElement(m_rvtDoc, GetType(Material), "Glass") ' hard coded fot simplicity.
        'If pMat Is Nothing Then
        '    ''  no material with the given name.
        '    Return
        'End If
        'Dim idMat As ElementId = pMat.Id

        ''  give an initial value 
        ''  if you don't give the initial value, it will be <By Category>
        'm_familyMgr.Set(param, idMat)

    End Sub

#End Region

#Region "Dimensions"

    '' ============================================
    ''   (3.2) add dimensions
    '' ============================================

    ''  add two dimensions between the reference planes: 
    ''      Tw = Left and OffsetV 
    ''      Td = Front and OffsetH 
    '' 
    Sub AddDimensions()

        AddDimention_Tw()
        AddDimention_Td()

    End Sub

    '' add dimension between 'Left' and 'OffsetV', and lable it as Tw. 
    '' 
    Sub AddDimention_Tw()

        ''  find the plan view that we want to place a dimension 
        Dim pViewPlan As View = Utils.FindElement(m_rvtDoc, GetType(ViewPlan), "Lower Ref. Level")

        ''  find two reference planes which we want to add a dimension between 
        Dim ref1 As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Left")
        Dim ref2 As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "OffsetV")

        ''  make an array of references 
        Dim pRefArray As New ReferenceArray
        pRefArray.Append(ref1.Reference)
        pRefArray.Append(ref2.Reference)

        ''  define a dimension line
        Dim p0 As XYZ = ref1.FreeEnd
        Dim p1 As XYZ = ref2.FreeEnd
        Dim pLine As Line = m_rvtApp.Create.NewLineBound(p0, p1)

        ''  create a dimension 
        Dim pDimTw As Dimension = m_rvtDoc.FamilyCreate.NewDimension(pViewPlan, pLine, pRefArray)

        ''  add label to the dimension
        pDimTw.Label = m_familyMgr.Parameter("Tw")

    End Sub

    '' 
    ''  same idea. add dimension between 'Front' and 'OffsetH', and lable it as 'Td
    ''
    Sub AddDimention_Td()

        ''  find the plan view that we want to place a dimension 
        Dim pViewPlan As View = Utils.FindElement(m_rvtDoc, GetType(ViewPlan), "Lower Ref. Level")

        ''  find two reference planes which we want to add a dimension between 
        Dim ref1 As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Front")
        Dim ref2 As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "OffsetH")

        ''  make an array of references 
        Dim pRefArray As New ReferenceArray
        pRefArray.Append(ref1.Reference)
        pRefArray.Append(ref2.Reference)

        ''  define a dimension line
        Dim p0 As XYZ = ref1.FreeEnd
        Dim p1 As XYZ = ref2.FreeEnd
        Dim pLine As Line = m_rvtApp.Create.NewLineBound(p0, p1)

        ''  create a dimension 
        Dim pDimTw As Dimension = m_rvtDoc.FamilyCreate.NewDimension(pViewPlan, pLine, pRefArray)

        ''  add label to the dimension
        pDimTw.Label = m_familyMgr.Parameter("Td")

    End Sub

#End Region

#Region "Types"

    '' ============================================
    ''   (3.3) add types
    '' ============================================
    ''  since we have formulas set up between Width and Tw, and Depth and Td. 
    ''
    Sub AddTypes()

        ''  addType(name, Width, Depth )
        ''
        AddType("600x900", 600.0, 900.0)
        AddType("1000x300", 1000.0, 300.0)
        AddType("600x600", 600.0, 600.0)

    End Sub

    ''  add one type 
    ''
    Sub AddType(ByVal name As String, ByVal w As Double, ByVal d As Double)

        ''  add new types with the given name.
        ''  the new type becomes the current type
        Dim type1 As FamilyType = m_familyMgr.NewType(name)

        ''  look for 'Width' and 'Depth' parameters and set them to the given value
        ''
        Dim paramW As FamilyParameter = m_familyMgr.Parameter("Width")
        Dim valW As Double = Utils.mmToFeet(w)
        If paramW IsNot Nothing Then
            m_familyMgr.Set(paramW, valW)
        End If

        Dim paramD As FamilyParameter = m_familyMgr.Parameter("Depth")
        Dim valD As Double = Utils.mmToFeet(d)
        If paramD IsNot Nothing Then
            m_familyMgr.Set(paramD, valD)
        End If

    End Sub

#End Region

#Region "Geometry Solid"

    Sub AddGeometrySolid()

        Dim pSolid As Extrusion = CreateSolid()
        m_rvtDoc.Regenerate()

        AddAlignments(pSolid)

        SetVisibility(pSolid)

        AddMaterials(pSolid)

    End Sub

    '' ============================================
    ''   (1.2) create a simple solid by extrusion with L-shape profile
    '' ============================================
    Function CreateSolid() As Extrusion

        ''  (1) define a simple L-shape profile
        ''  
        Dim pProfile As CurveArrArray = CreateProfileLShape()

        ''  (2) create a sketch plane
        ''
        ''  we need to know the template. If you look at the template (Metric Column.rft) and "Front" view,
        ''  you will see "Reference Plane" at "Lower Ref. Level". We are going to create an extrusion there.
        ''  findElement() is a helper function that find an element of the given type and name.  see below.
        ''
        Dim pRefPlane As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Reference Plane") ' need to know from the template
        Dim pSketchPlane As SketchPlane = m_rvtDoc.FamilyCreate.NewSketchPlane(pRefPlane.Plane)

        ''  (3) height of the extrusion
        ''
        ''  same as profile, you will need to know your template. unlike UI, the alightment will not adjust the geometry.
        ''  You will need to have the exact location in order to set alignment.
        ''  Here we hard code for simplicity. 4000 is the distance between Lower and Upper Ref. Level.
        ''
        Dim dHeight As Double = Utils.mmToFeet(4000) '' distance between Lower and Upper Ref Level.

        ''  (4) create an extrusion here. at this point.
        ''
        Dim bIsSolid As Boolean = True '' as oppose to void.
        Dim pSolid As Extrusion = m_rvtDoc.FamilyCreate.NewExtrusion(bIsSolid, pProfile, pSketchPlane, dHeight)

        Return pSolid

    End Function

    '' ============================================
    ''   (1.2a) create a simple L-shaped profile
    '' ============================================
    Function CreateProfileLShape() As CurveArrArray

        ''
        ''  define a simple L-shaped profile
        ''
        ''  5 tw 4
        ''   +-+
        ''   | | 3          h = height
        '' d | +---+ 2
        ''   +-----+ td
        ''  0        1
        ''  6  w
        ''

        ''  sizes (hard coded for simplicity)
        ''  note: these need to match reference plane. otherwise, alignment won't work.
        ''  as an exercise, try changing those values and see how it behaves.
        ''
        Dim w As Double = Utils.mmToFeet(600)  '' those are hard coded for simplicity here. in practice, you may want to find out from the references)
        Dim d As Double = Utils.mmToFeet(600)
        Dim tw As Double = Utils.mmToFeet(150) '' thickness added for Lab2
        Dim td As Double = Utils.mmToFeet(150)

        ''  define vertices
        ''
        Const nVerts As Integer = 6 '' the number of vertices
        Dim pts() As XYZ = {New XYZ(-w / 2, -d / 2, 0), New XYZ(w / 2, -d / 2, 0), New XYZ(w / 2, -d / 2 + td, 0), _
                            New XYZ(-w / 2 + tw, -d / 2 + td, 0), New XYZ(-w / 2 + tw, d / 2, 0), New XYZ(-w / 2, d / 2, 0), _
                            New XYZ(-w / 2, -d / 2, 0)} '' the last one is to make the loop simple

        ''  define a loop. define individual edges and put them in a curveArray
        ''
        Dim pLoop As CurveArray = m_rvtApp.Create.NewCurveArray
        Dim lines(nVerts - 1) As Line
        For i As Integer = 0 To nVerts - 1
            lines(i) = m_rvtApp.Create.NewLineBound(pts(i), pts(i + 1))
            pLoop.Append(lines(i))
        Next

        ''  then, put the loop in the curveArrArray as a profile
        ''
        Dim pProfile As CurveArrArray = m_rvtApp.Create.NewCurveArrArray
        pProfile.Append(pLoop)
        ''  if we come here, we have a profile now.

        Return pProfile

    End Function

    '' ============================================
    ''   (2.1) add alignments
    '' ============================================

    Sub AddAlignments(ByVal pSolid As Extrusion)

        AddAlignment_Level(pSolid, New XYZ(0.0, 0.0, 1.0), "Upper Ref Level")
        AddAlignment_Level(pSolid, New XYZ(0.0, 0.0, -1.0), "Lower Ref. Level")
        AddAlignment_ReferencePlane(pSolid, New XYZ(1.0, 0.0, 0.0), "Right")
        AddAlignment_ReferencePlane(pSolid, New XYZ(-1.0, 0.0, 0.0), "Left")
        AddAlignment_ReferencePlane(pSolid, New XYZ(0.0, -1.0, 0.0), "Front")
        AddAlignment_ReferencePlane(pSolid, New XYZ(0.0, 1.0, 0.0), "Back")
        AddAlignment_ReferencePlane(pSolid, New XYZ(1.0, 0.0, 0.0), "OffsetV")
        AddAlignment_ReferencePlane(pSolid, New XYZ(0.0, 1.0, 0.0), "OffsetH")

    End Sub

    ''  we want to constrain the upper face of the column to the "Upper Ref Level"
    '' 
    Sub AddAlignment_Level(ByVal pSolid As Extrusion, ByVal normal As XYZ, ByVal nameLevel As String)

        ''  which direction are we looking at?
        Dim pView As View = Utils.FindElement(m_rvtDoc, GetType(View), "Front")

        ''  find the upper ref level. FindElement() is a helper function.
        Dim pLevel As Level = Utils.FindElement(m_rvtDoc, GetType(Level), nameLevel)

        ''  find the face of the box. FindFace() is a helper function.
        Dim pFace As PlanarFace = Utils.FindFace(pSolid, normal)

        '' create alignments
        m_rvtDoc.FamilyCreate.NewAlignment(pView, pLevel.PlaneReference, pFace.Reference)

    End Sub

    Sub AddAlignment_ReferencePlane(ByVal pSolid As Extrusion, ByVal normal As XYZ, ByVal nameRefPlane As String)

        ''  get the plan view
        ''  note: same name maybe used for different view types. either one should work.
        Dim pViewPlan As View = Utils.FindElement(m_rvtDoc, GetType(ViewPlan), "Lower Ref. Level")

        ''  find reference planes
        Dim refPlane As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), nameRefPlane)

        ''  find the face of the solid
        Dim pFace As PlanarFace = Utils.FindFace(pSolid, normal, refPlane)

        '' create alignments
        m_rvtDoc.FamilyCreate.NewAlignment(pViewPlan, refPlane.Reference, pFace.Reference)

    End Sub

    '' ============================================
    ''   (5.1.2) set the visibility of the solid not to show in coarse
    '' ============================================
    Sub SetVisibility(ByVal pSolid As Extrusion)

        ''  set the visibility of the model not to shown in coarse.
        Dim pVis As FamilyElementVisibility = New FamilyElementVisibility(FamilyElementVisibilityType.Model)
        pVis.IsShownInCoarse = False

        pSolid.SetVisibility(pVis)

    End Sub

    Sub addMaterials(ByVal pSolid As Extrusion)

        ''  We assume Material type "Glass" exists. Template "Metric Column.rft" include "Glass",
        ''  which in fact is the only interesting one to see the effect.
        ''  In practice, you will want to include in your template.
        ''
        ''  To Do: For the exercise, create it with more appropriate ones in UI, then use the name here.
        ''

        ''  (1)  get the materials id that we are intersted in (e.g., "Glass")
        ''
        Dim pMat As Material = Utils.FindElement(m_rvtDoc, GetType(Material), "Glass") ' hard coded fot simplicity.
        If pMat Is Nothing Then
            ''  no material with the given name.
            Return
        End If
        Dim idMat As ElementId = pMat.Id

        ''  (2a) this add a material to the solid base.  but then, we cannot change it for each column.
        ''
        'pSolid.Parameter("Material").Set(idMat)

        ''  (2b) add a parameter for material finish
        ''
        Dim paramFamilyMaterial As FamilyParameter = m_familyMgr.Parameter("Column Finish")

        ''  (2b.1) associate material parameter to the family parameter
        ''
        Dim paramSolidMaterial As Parameter = pSolid.Parameter("Material")
        m_familyMgr.AssociateElementParameterToFamilyParameter(paramSolidMaterial, paramFamilyMaterial)

        ''  (2b.2) for our combeniencem, let's add another type with Glass finish
        ''
        AddType("Glass", 600.0, 600.0)
        m_familyMgr.Set(paramFamilyMaterial, idMat)

    End Sub

#End Region

#Region "Geometry Model Line"

    Sub AddGeometryModelLine()

        Dim pModelLine As ModelCurve = CreateModelLine()
        m_rvtDoc.Regenerate()

        AddAlignments(pModelLine)

        SetVisibility(pModelLine)

    End Sub

    Function CreateModelLine() As ModelCurve

        ''  we define a simple vertical line at the center.

        ''  height. hard coding for simplicity. You could calculate from the two reference planes. 
        Dim h As Double = Utils.mmToFeet(4000.0) '' distance between Lower and Upper Ref Level.

        ''  define vertices
        Dim pt0 As New XYZ(0.0, 0.0, 0.0)
        Dim pt1 As New XYZ(0.0, 0.0, h)

        ''  create a sketch plane
        Dim pRefPlane As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Center (Front/Back)")
        Dim pSketchPlane As SketchPlane = m_rvtDoc.FamilyCreate.NewSketchPlane(pRefPlane.Plane)

        ''  create line objects: one model curve representing a column like a vertical stick.
        Dim geomLine As Line = m_rvtApp.Create.NewLine(pt0, pt1, True)
        Dim pLine As ModelCurve = m_rvtDoc.FamilyCreate.NewModelCurve(geomLine, pSketchPlane)

        Return pLine

    End Function

    '' ============================================
    ''   (2.1) add alignments to the model line 
    '' ============================================

    Sub AddAlignments(ByVal pLine As ModelLine)

        AddAlignment_Level(pLine, 1, "Upper Ref Level")
        AddAlignment_Level(pLine, 0, "Lower Ref. Level")

        AddAlignment_ReferencePlane(pLine, "Center (Left/Right)", "Front")
        AddAlignment_ReferencePlane(pLine, "Center (Front/Back)", "Right")

    End Sub

    ''  we want to constrain the upper face of the column to the "Upper Ref Level"
    '' 
    Sub AddAlignment_Level(ByVal pModelLine As ModelLine, ByVal indexEndPoint As Integer, ByVal nameLevel As String)

        ''  get the view. 
        Dim pView As View = Utils.FindElement(m_rvtDoc, GetType(View), "Front")

        ''  find the upper ref level. 
        Dim pLevel As Level = Utils.FindElement(m_rvtDoc, GetType(Level), nameLevel)

        ''  find the face of the box. 
        Dim pRefPoint As Reference = pModelLine.GeometryCurve.EndPointReference(indexEndPoint)

        '' create alignments
        m_rvtDoc.FamilyCreate.NewAlignment(pView, pLevel.PlaneReference, pRefPoint)

    End Sub

    Sub AddAlignment_ReferencePlane(ByVal pModelLine As ModelLine, ByVal nameRefPlane As String, ByVal nameView As String)

        ''  get the view
        ''  note: same name maybe used for different view types. either one should work.
        Dim pView As View = Utils.FindElement(m_rvtDoc, GetType(View), nameView)

        ''  find reference planes
        Dim refPlane As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), nameRefPlane)

        '' create alignments
        m_rvtDoc.FamilyCreate.NewAlignment(pView, refPlane.Reference, pModelLine.GeometryCurve.Reference)

    End Sub

    Sub SetVisibility(ByVal pModelLine As ModelLine)

        '' set the visibilities of the model line to coarse only
        Dim pVis As FamilyElementVisibility = New FamilyElementVisibility(FamilyElementVisibilityType.Model)
        pVis.IsShownInFine = False
        pVis.IsShownInMedium = False
        pModelLine.SetVisibility(pVis)

    End Sub

#End Region


#Region "Geometry Symbolic Line"

    '' ============================================
    ''   (5.1.1) create simple line objects to be displayed in coarse level
    '' ============================================

    Sub AddGeometrySymbolicLines()

        Dim pSymbolicLines As List(Of SymbolicCurve) = CreateSymbolicLines()
        m_rvtDoc.Regenerate()

        AddAlignments(pSymbolicLines)

        SetVisibility(pSymbolicLines)

    End Sub

    Function CreateSymbolicLines() As List(Of SymbolicCurve)

        ''
        ''  define a simple L-shape detail line object
        ''
        ''  0
        ''   +               h = height
        ''   |              (we also want to draw a vertical line here at point 1)
        '' d |
        ''   +-----+
        ''  1       2
        ''      w
        ''

        ''  sizes
        Dim w As Double = Utils.mmToFeet(600) '' modified to match reference plane. otherwise, alignment won't work.
        Dim d As Double = Utils.mmToFeet(600)
        Dim t As Double = Utils.mmToFeet(50) '' slight offset for visbility

        ''  define vertices
        ''
        Dim pts() As XYZ = {New XYZ(-w / 2 + t, d / 2, 0), _
                            New XYZ(-w / 2 + t, -d / 2 + t, 0), _
                            New XYZ(w / 2, -d / 2 + t, 0)}

        ''
        ''  (2) create a sketch plane
        ''
        ''  we need to know the template. If you look at the template (Metric Column.rft) and "Front" view,
        ''  you will see "Reference Plane" at "Lower Ref. Level". We are going to create a sketch plane there.
        ''  findElement() is a helper function that find an element of the given type and name.  
        ''  Note: we did the same in creating a profile.
        ''
        Dim pRefPlane As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Reference Plane") '' need to know from the template
        Dim pSketchPlane As SketchPlane = m_rvtDoc.FamilyCreate.NewSketchPlane(pRefPlane.Plane)


        ''  (4) create line objects: two symbolic curves on a plan and one model curve representing a column like a vertical stick.
        ''
        Dim geomLine1 As Line = m_rvtApp.Create.NewLine(pts(0), pts(1), True)
        Dim geomLine2 As Line = m_rvtApp.Create.NewLine(pts(1), pts(2), True)

        Dim pLine1 As SymbolicCurve = m_rvtDoc.FamilyCreate.NewSymbolicCurve(geomLine1, pSketchPlane)
        Dim pLine2 As SymbolicCurve = m_rvtDoc.FamilyCreate.NewSymbolicCurve(geomLine2, pSketchPlane)

        Dim pLines As New List(Of SymbolicCurve)
        pLines.Add(pLine1)
        pLines.Add(pLine2)

        Return pLines

    End Function

    Sub AddAlignments(ByVal pSymbolicLines As List(Of SymbolicCurve))

        ''  To Do. leave it as an exercise. 
        ''  The idea is the same as Solid and Model Curve. 
        ''  You will need to set the alignments one by one. 

    End Sub

    Sub SetVisibility(ByVal pSymbolicLines As List(Of SymbolicCurve))

        ''  set the visibilities of two lines to coarse only
        Dim pVis As FamilyElementVisibility = New FamilyElementVisibility(FamilyElementVisibilityType.ViewSpecific)
        pVis.IsShownInFine = False
        pVis.IsShownInMedium = False

        pSymbolicLines(0).SetVisibility(pVis)
        pSymbolicLines(1).SetVisibility(pVis)

    End Sub

#End Region

End Class

