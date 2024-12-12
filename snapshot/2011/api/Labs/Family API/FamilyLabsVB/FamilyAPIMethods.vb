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

'Imports System.Linq  
'Imports Autodesk.Revit
'Imports Autodesk.Revit.DB
'Imports Autodesk.Revit.UI
'Imports Autodesk.Revit.ApplicationServices

#End Region

#Region "Description"
''  Family API how to 
'' 
#End Region

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class RvtCmd_FamilyAPIMethods
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
        m_familyMgr = m_rvtDoc.FamilyManager

        ''  (1) Plan (insertion point, patametric origin). 
        ''  like in the UI, you will still need to plan. choose a right template

        ''  (1) This command works in the context of family editor only.
        ''      We also check if the template is for an appropriate category if needed.
        ''      Here we use a Column(i.e., Metric Column.rft) template.
        ''      Although there is no specific checking about metric or imperial, our lab only works in metric for now.
        ''
        'If Not isRightTemplate(BuiltInCategory.OST_Columns) Then
        '    MsgBox("Please open Metric Column.rft")
        '    Return Result.Failed
        'End If
        If Not ValidateDocument(m_rvtDoc) Then
            Return Result.Failed
        End If

        ''  (2) add reference planes
        ''
        AddReferencePlaneSampler()

        ' ''  (1.2) create a simple extrusion. This time we create a L-shape.
        'Dim pSolid As Extrusion = createSolid()

        ' '' try this:
        ' '' if you comment alignment code below and execute only up to here,
        ' '' you will see the column's top will not follow the upper level.

        ' ''  (2) add alignment
        'addAlignments(pSolid)

        ' ''  (3.1) add parameters
        ' ''
        AddParameterSampler()
        ' ''  (4.1) add formula
        ' ''
        'addFormulas()
        ' ''  (3.2) add dimensions
        ' ''
        AddDimensionSampler()

        ' ''  (3.3) add types
        ' ''
        AddTypes()

        ' ''  (4.2) add materials
        ' ''
        'addMaterials(pSolid)

        ' ''  (5.1) add visibilities
        ' ''
        'addLineObjects()
        'changeVisibility(pSolid)

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

    ''  Note: currently, you cannot "pin" a reference plane using API. 
    ''  SPR #154708  Ability to pin\unpin an element using Revit API

    Sub AddReferencePlaneSampler()

        AddReferencePlane_NewReferencePlane()
        AddReferencePlane_NewReferencePlane2()
        AddReferencePlane_Angle45()
        AddReferencePlane_SideView()
        AddReferencePlane_NonOrtho2()
        AddReferencePlane_NonOrtho()

    End Sub


    ''  add a reference plane using NewReferencePlane() 
    ''  this version takes a vector which is perpendicular to the first line to determine the plane. 
    ''  a reference plane parallel to yz-plane 
    '' 
    Sub AddReferencePlane_NewReferencePlane()

        ''  when creating an element in a family document, you will use: 
        ''  rvtDoc.FamilyCreate.NewXxx
        ''  note that there is rvtDoc.Create for project. i.e., rvtDoc.Create.NewXxx
        '' 
        'Dim familyCreater As Autodesk.Revit.Creation.FamilyItemFactory = m_rvtDoc.FamilyCreate

        ''  create a reference plan, using NewReferencePlane
        Dim pt1 As New XYZ(2.0, -2.0, 0.0) '' one end  
        Dim pt2 As New XYZ(2.0, 2.0, 0.0)   '' the other end 
        Dim vec As XYZ = XYZ.BasisZ          '' this determines a plane. perpendicular to the first line.
        Dim view As View = m_rvtDoc.ActiveView  '' which view are we looking at? does not matter for a family document.

        Dim refPlane As ReferencePlane = m_rvtDoc.FamilyCreate.NewReferencePlane(pt1, pt2, vec, view)
        refPlane.Name = "My Ref Plane"

    End Sub

    ''  add a reference plane using NewReferencePlane2()
    ''  this version takes the third point to determine the plane. 
    ''  a reference plane parallel to yz-plane 
    '' 
    Sub AddReferencePlane_NewReferencePlane2()

        ''  create a reference plan, using NewReferencePlane2
        Dim pt1 As New XYZ(-2.0, -2.0, 0.0) '' one end  
        Dim pt2 As New XYZ(-2.0, 2.0, 0.0)  '' the other end 
        Dim pt3 As New XYZ(-2.0, 0.0, -1.0)  '' the third point to determines a plane
        Dim view As View = m_rvtDoc.ActiveView  '' which view are we looking at? does not matter for a family document.

        Dim refPlane As ReferencePlane = m_rvtDoc.FamilyCreate.NewReferencePlane2(pt1, pt2, pt3, view)
        refPlane.Name = "My Ref Plane2"

    End Sub

    ''  try adding a reference plane which is not orthogonal to x- and y-axis. 
    ''  reference plane with 45-degree rotated from xz-plane 
    '' 
    Sub AddReferencePlane_Angle45()

        ''  create a reference plan, using NewReferencePlane
        Dim pt1 As New XYZ(-3.0, -3.0, 0.0) '' one end  
        Dim pt2 As New XYZ(3.0, 3.0, 0.0)  '' the other end 
        Dim vec As XYZ = XYZ.BasisZ        '' this determines a plane. perpendicular to the first line.
        Dim view As View = m_rvtDoc.ActiveView  '' which view are we looking at? does not matter for a family document.

        Dim refPlane As ReferencePlane = m_rvtDoc.FamilyCreate.NewReferencePlane(pt1, pt2, vec, view)
        refPlane.Name = "My Ref Plane Angle45"

    End Sub

    ''  try adding a reference plane which is visible from the side view 
    ''  reference plane parallel to xy-plane 
    ''
    Sub AddReferencePlane_SideView()

        ''  create a reference plan, using NewReferencePlane
        Dim pt1 As New XYZ(-2.0, 0.0, 1.0) '' one end  
        Dim pt2 As New XYZ(2.0, 0.0, 1.0)  '' the other end 
        Dim vec As XYZ = XYZ.BasisY        '' this determines a plane. perpendicular to the first line.
        Dim view As View = m_rvtDoc.ActiveView  '' which view are we looking at? does not matter for a family document.

        Dim refPlane As ReferencePlane = m_rvtDoc.FamilyCreate.NewReferencePlane(pt1, pt2, vec, view)
        refPlane.Name = "My Ref Plane Side View"

    End Sub

    ''  what about a reference on an arbitrary plane? can we create it? 
    ''  a reference plane whose normal is (1,1,1)
    ''  this does not define the correct one. 
    '' 
    Sub AddReferencePlane_NonOrtho2()

        ''  create a reference plan, using NewReferencePlane2
        Dim pt1 As New XYZ(1.0, -1.0, 1.0) '' one end  
        Dim pt2 As New XYZ(-1.0, 1.0, 1.0)  '' the other end 
        Dim pt3 As New XYZ(1.0, 1.0, -1.0)  '' this determines a plane. perpendicular to the first line.
        Dim view As View = m_rvtDoc.ActiveView  '' which view are we looking at? does not matter for a family document.

        Dim refPlane As ReferencePlane = m_rvtDoc.FamilyCreate.NewReferencePlane2(pt1, pt2, pt3, view)
        refPlane.Name = "My Ref Plane2 Non-Ortho"

    End Sub

    ''  what about a reference on an arbitrary plane? can we create it? 
    ''  a reference plane whose normal is (1,1,1)
    ''  this does not behaving well in UI. 
    '' 
    Sub AddReferencePlane_NonOrtho()

        ''  create a reference plan, using NewReferencePlane2
        Dim pt1 As New XYZ(1.0, -1.0, 1.0) '' one end  
        Dim pt2 As New XYZ(-1.0, 1.0, 1.0)  '' the other end 
        Dim vec As New XYZ(1.0, 1.0, -2.0)  '' this determines a plane. perpendicular to the first line.
        Dim view As View = m_rvtDoc.ActiveView  '' which view are we looking at? does not matter for a family document.

        Dim refPlane As ReferencePlane = m_rvtDoc.FamilyCreate.NewReferencePlane(pt1, pt2, vec, view)
        refPlane.Name = "My Ref Plane Non-Ortho"

    End Sub

#End Region


#Region "Parameter"


    '' ============================================
    ''   (3.1) add parameters
    '' ============================================

    Sub AddParameterSampler()

        AddParameter_DimensionLength()
        AddParameter_Formula()
        AddParameter_Material()

    End Sub

    Sub AddParameter_DimensionLength()

        ''  create a new family parameter.
        ''  parameter group for Dimension is PG_GEOMETRY in API
        ''  the last argument is a flag indicating if this parameter is instance or type. 
        '' 
        Dim param As FamilyParameter = m_familyMgr.AddParameter( _
            "My Length", BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length, False)

        ''  give an initial value
        Dim initVal As Double = Utils.mmToFeet(150.0) '' in mm 
        'Dim initVal As Double = 0.5
        m_familyMgr.Set(param, initVal)

    End Sub

    Sub AddParameter_Formula()

        ''  create a new family parameter.
        ''  parameter group for Dimension is PG_GEOMETRY in API
        ''  the last argument is a flag indicating if this parameter is instance or type. 
        '' 
        Dim param As FamilyParameter = m_familyMgr.AddParameter( _
            "My Formula", BuiltInParameterGroup.PG_GEOMETRY, ParameterType.Length, False)

        ''  give an initial value
        Dim initVal As Double = Utils.mmToFeet(150.0) '' in mm 
        'Dim initVal As Double = 0.5 '' in feet
        m_familyMgr.Set(param, initVal)

        ''  a formula can be set simply by specifying as a string. 
        ''  Here we set MyFormulaParam as one fourth of Width. 
        m_familyMgr.SetFormula(param, "Width/4.0")

    End Sub

    Sub AddParameter_Material()

        ''  add a parameter for material finish under "Materials and Finishes" 
        ''  this time, we define instance parameter so that we can change it at the instance level.
        ''  you will see "(default)" after the name if it is an instance parameter. 

        Dim param As FamilyParameter = _
            m_familyMgr.AddParameter("My Material", BuiltInParameterGroup.PG_MATERIALS, ParameterType.Material, True)

        ''  find a material "Glass"
        ''  if you do not specify, the default will be <By Category> 
        '' 
        'Dim pMat As Material = Utils.FindElement(m_rvtDoc, GetType(Material), "Default") ' hard coded fot simplicity.
        Dim pMat As Material = Utils.FindElement(m_rvtDoc, GetType(Material), "Glass") ' hard coded fot simplicity.
        If pMat Is Nothing Then
            ''  no material with the given name.
            Return
        End If

        ''  give an initial value 
        m_familyMgr.Set(param, pMat.Id)

    End Sub

#End Region

#Region "Dimension"

    Sub AddDimensionSampler()

        AddDimention_HalfWidth()
        AddDimention_MultipleSegmentsEqual()

    End Sub

    Sub AddDimention_HalfWidth()

        ''  find the plan view that we want to place a dimension 
        Dim pViewPlan As View = Utils.FindElement(m_rvtDoc, GetType(ViewPlan), "Lower Ref. Level")

        ''  find two reference planes which we want to add a dimension between 
        Dim ref1 As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Left")
        Dim ref2 As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Center (Left/Right)")

        ''  make an array of references 
        Dim pRefArray As New ReferenceArray
        pRefArray.Append(ref1.Reference)
        pRefArray.Append(ref2.Reference)

        ''  define a dimension line
        Dim p0 As XYZ = ref1.FreeEnd
        Dim p1 As XYZ = ref2.FreeEnd
        Dim pLine As Line = m_rvtApp.Create.NewLineBound(p0, p1)

        ''  create a dimension 
        Dim pDim As Dimension = m_rvtDoc.FamilyCreate.NewDimension(pViewPlan, pLine, pRefArray)

        ''  add label to the dimension
        pDim.Label = m_familyMgr.Parameter("My Length")

    End Sub

    Sub AddDimention_MultipleSegmentsEqual()

        ''  find the plan view that we want to place a dimension 
        Dim pViewPlan As View = Utils.FindElement(m_rvtDoc, GetType(ViewPlan), "Lower Ref. Level")

        ''  find two reference planes which we want to add a dimension between 
        Dim ref1 As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Left")
        Dim ref2 As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Center (Left/Right)")
        Dim ref3 As ReferencePlane = Utils.FindElement(m_rvtDoc, GetType(ReferencePlane), "Right")

        ''  make an array of references 
        Dim pRefArray As New ReferenceArray
        pRefArray.Append(ref1.Reference)
        pRefArray.Append(ref2.Reference)
        pRefArray.Append(ref3.Reference)

        ''  define a dimension line
        Dim p0 As XYZ = ref1.FreeEnd
        Dim p1 As XYZ = ref3.FreeEnd
        Dim pLine As Line = m_rvtApp.Create.NewLineBound(p0, p1)

        ''  create a dimension 
        Dim pDim As Dimension = m_rvtDoc.FamilyCreate.NewDimension(pViewPlan, pLine, pRefArray)

        ''  make segments to be equal 
        pDim.AreSegmentsEqual = True

    End Sub

#End Region

#Region "Type"

    '' ============================================
    ''   (3.3) add types
    '' ============================================
    ''  since we have formulas set up between Width and Tw, and Depth and Td. 
    ''
    Sub AddTypes()

        ''  addType(name, Width, Depth )
        ''
        AddType("MyType1", 600.0, 900.0)
        AddType("My Type2", 1000.0, 300.0)
        AddType("My Type3", 600.0, 600.0)

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

End Class
