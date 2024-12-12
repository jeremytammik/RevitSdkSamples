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
#End Region

#Region "Imports"
'' Import the following name spaces in the project properties/references. 
'' Note: VB.NET has a slighly different way of recognizing name spaces than C#. 
'' if you explicitely set them in each .vb file, you will need to specify full name spaces. 

'Imports System
'Imports Autodesk.Revit.DB
'Imports Autodesk.Revit.UI
'Imports Autodesk.Revit.ApplicationServices  '' Application class
'Imports Autodesk.Revit.Attributes '' specific this if you want to save typing for attributes. e.g., 
'Imports Autodesk.Revit.UI.Selection '' for selection 
'Imports Autodesk.Revit.Exceptions '' for exception added for UI Lab3.  
Imports RevitIntroVB '' we'll be using commands we defined in Revit Intro labs. 

#End Region

''  User Selection 
'' 
''  Note: This exercise uses Revit Into Labs. 
''  Modify your project setting to place the dlls from both labs in one place. 
'' 
''
''  cf. Developer Guide, Section 7: Selection (pp 89)  
'' 

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class UISelection
    Implements IExternalCommand

    ''  member variables 
    Dim m_rvtUIApp As UIApplication
    Dim m_rvtUIDoc As UIDocument
    'Dim m_rvtApp As Application
    'Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        ''  Get the access to the top most objects. (we may not use them all in this specific lab.) 
        m_rvtUIApp = commandData.Application
        m_rvtUIDoc = m_rvtUIApp.ActiveUIDocument

        ''  (1) pre-selecetd element is under UIDocument.Selection.Elemens. Classic method.  
        ''  you can also modify this selection set. 
        '' 
        Dim selSet As SelElementSet = m_rvtUIDoc.Selection.Elements
        ShowSelectionSet(selSet, "Pre-selection: ")

        ''  (2)  we can also pick elements. 
        ''  here is what we have done in Intro labs. (To reflesh your memory.)   
        ''  select an object on a screen. 
        '' 
        'Dim ref As Reference = m_rvtUIDoc.Selection.PickObject(ObjectType.Element, "Pick an element")
        ' ''  we have picked something. 
        'Dim elem As Element = ref.Element
        'ShowBasicElementInfo(elem)

        Try
            ''  (2.1) pick methods basics.  
            ''  there are four types of pick methods: PickObject, PickObjects, PickElementByRectangle, PickPoint. 
            ''  Let's quickly try them out. 
            '' 

            PickMethodsBasics()

            ''  (2.2) selection object type    
            ''  in addition to selecting objects of type Element, the user can pick faces, edges, and point on element. 
            '' 
            PickFaceEdgePoint()

            ''  (2.3) selection filter  
            ''  if you want additional selection criteria, such as only to pick a wall, you can use selection filter. 
            ''  
            ApplySelectionFilter()

        Catch err As Autodesk.Revit.Exceptions.OperationCanceledException
            TaskDialog.Show("Revit UI Labs", "You have canceled selection.")

        Catch ex As Exception
            TaskDialog.Show("Revit UI Labs", "Some other exception caught in CancelSelection()")

        End Try

        ''  (2.4) canceling selection  
        ''  when the user cancel or press [Esc] key during the selection, OperationCanceledException will be thrown. 
        ''
        CancelSelection()

        ''  (3) apply what we learned to our small house creation 
        ''   we put it as a separate command. See at the bottom of the code.  
        'CreateHouseUI()

        Return Result.Succeeded

    End Function

    ''  show the list of element in the given SelElementSet. 
    '' 
    Sub ShowSelectionSet(ByVal elemSet As SelElementSet, ByVal header As String)

        ''  putting in a form of IList
        Dim elemList As IList(Of Element) = New List(Of Element)

        For Each elem As Element In elemSet
            elemList.Add(elem)
        Next

        ''  use the helper function. By now, you should be familier with element handling. 
        ''  if not, see Revit Intro Lab3. 
        ShowElementList(elemList, header)

    End Sub

    ''  Show basic information about the given element. 
    '' 
    Public Sub ShowBasicElementInfo(ByVal elem As Element)

        ''  let's see what kind of element we got. 
        Dim s As String = "You picked:" + vbCr

        s += ElementToString(elem)

        ''  show what we got. 
        TaskDialog.Show("Revit UI Lab", s)

    End Sub

    ''  Pick methods sampler. 
    ''  quickly try: PickObject, PickObjects, PickElementByRectangle, PickPoint. 
    ''  without specifics about objects we want to pick. 
    ''
    Sub PickMethodsBasics()

        ''  (1) Pick Object (we have done this already. But just for the sake of completeness.) 
        PickMethod_PickObject()

        ''  (2) Pick Objects 
        PickMethod_PickObjects()

        ''  (3) Pick Element By Rectangle 
        PickMethod_PickElementByRectangle()

        ''  (4) Pick Point 
        PickMethod_PickPoint()

    End Sub

    ''  minimum PickObject
    '' 
    Sub PickMethod_PickObject()

        Dim ref As Reference = m_rvtUIDoc.Selection.PickObject(ObjectType.Element, "Select one element")
        Dim elem As Element = ref.Element
        ShowBasicElementInfo(elem)

    End Sub

    ''  minimum PickObjects 
    ''  note: when you run this code, you will see "Finish" and "Cancel" buttons in the dialog bar. 
    '' 
    Sub PickMethod_PickObjects()

        Dim refs As IList(Of Reference) = _
            m_rvtUIDoc.Selection.PickObjects(ObjectType.Element, "Select multiple elemens")

        ''  put it in a List form. 
        Dim elems As IList(Of Element) = New List(Of Element)
        For Each ref As Reference In refs
            elems.Add(ref.Element)
        Next
        ''  show it. 
        ShowElementList(elems, "Pick Objects: ")

    End Sub

    ''  minimum PickElementByRectangle
    ''
    Sub PickMethod_PickElementByRectangle()

        ''  note: PickElementByRectangle returns the list of element. not reference. 
        Dim elems As IList(Of Element) = _
            m_rvtUIDoc.Selection.PickElementsByRectangle("Select by rectangle")

        ''  show it. 
        ShowElementList(elems, "Pick By Rectangle: ")

    End Sub

    ''  minimum PickPoint
    ''
    Sub PickMethod_PickPoint()

        Dim pt As XYZ = m_rvtUIDoc.Selection.PickPoint("Pick a point")

        ''  show it. 
        Dim msg As String = "Pick Point: "
        msg += PointToString(pt)
        TaskDialog.Show("Revit UI Labs", msg)

    End Sub

    ''  pick face, edge, point on an element
    ''  objectType options is applicable to PickObject() and PickObjects() 
    ''  
    Sub PickFaceEdgePoint()

        ''  (1) Face 
        PickFace()

        ''  (2) Edge 
        PickEdge()

        ''  (3) Point 
        PickPointOnElement()

    End Sub

    Sub PickFace()

        Dim refFace As Reference = m_rvtUIDoc.Selection.PickObject(ObjectType.Face, "Select a face")
        Dim oFace As Face = refFace.GeometryObject

        ''  show a message to the user. 
        Dim msg As String = ""
        If oFace IsNot Nothing Then
            msg = "You picked the face of element " + refFace.Element.Id.ToString + vbCr
        Else
            msg = "no Face picked" + vbCr
        End If
        TaskDialog.Show("Revit UI Labs", msg)

    End Sub

    Sub PickEdge()

        Dim refEdge As Reference = m_rvtUIDoc.Selection.PickObject(ObjectType.Edge, "Select an edge")
        Dim oEdge As Edge = refEdge.GeometryObject

        ''  show it. 
        Dim msg As String = ""
        If oEdge IsNot Nothing Then
            msg = "You picked an edge of element " + refEdge.Element.Id.ToString + vbCr
        Else
            msg = "no Edge picked" + vbCr
        End If
        TaskDialog.Show("Revit UI Labs", msg)

    End Sub

    Sub PickPointOnElement()

        Dim refPoint As Reference = _
            m_rvtUIDoc.Selection.PickObject(ObjectType.PointOnElement, "Select a point on element")
        Dim pt As XYZ = refPoint.GlobalPoint

        '' show it. 
        Dim msg As String = ""
        If pt IsNot Nothing Then
            msg = "You picked the point " + PointToString(pt) + " on an element " + _
                refPoint.Element.Id.ToString + vbCr
        Else
            msg = "no Point picked" + vbCr
        End If
        TaskDialog.Show("Revit UI Labs", msg)

    End Sub

    ''  pick with selection filter 
    ''  let's assume we only want to pick up a wall.
    '' 
    Sub ApplySelectionFilter()

        ''  pick only a wall 
        PickWall()

        ''  pick only a planar face. 
        PickPlanarFace()

    End Sub

    ''  selection with wall filter. 
    ''  See the bottom of the page to see the selection filter implementation. 
    ''
    Sub PickWall()

        Dim selFilterWall As New SelectionFilterWall
        Dim ref As Reference = m_rvtUIDoc.Selection.PickObject(ObjectType.Element, selFilterWall, "Select a wall")

        '' show it
        Dim elem As Element = ref.Element
        ShowBasicElementInfo(elem)

    End Sub

    ''  selection with planar face. 
    ''  See the bottom of the page to see the selection filter implementation. 
    ''
    Sub PickPlanarFace()

        ''  to call ISelectionFilter.AllowReference, use this.  
        ''  this will limit picked face to be planar. 
        Dim selFilterPlanarFace As New SelectionFilterPlanarFace
        Dim ref As Reference = m_rvtUIDoc.Selection.PickObject(ObjectType.Face, selFilterPlanarFace, "Select a planar face")
        Dim oFace As Face = ref.GeometryObject

        ''  show a message to the user. 
        Dim msg As String = ""
        If oFace IsNot Nothing Then
            msg = "You picked the face of element " + ref.Element.Id.ToString + vbCr
        Else
            msg = "no Face picked" + vbCr
        End If
        TaskDialog.Show("Revit UI Labs", msg)


    End Sub

    ''  canceling selection 
    ''  when the user presses [Esc] key during the selection, OperationCanceledException will be thrown.  
    '' 
    Sub CancelSelection()

        Try
            Dim ref As Reference = m_rvtUIDoc.Selection.PickObject(ObjectType.Element, "Select one element, or press [Esc] to cancel")
            Dim elem As Element = ref.Element
            ShowBasicElementInfo(elem)

        Catch err As Autodesk.Revit.Exceptions.OperationCanceledException
            TaskDialog.Show("Revit UI Labs", "You have canceled selection.")

        Catch ex As Exception
            TaskDialog.Show("Revit UI Labs", "Some other exception caught in CancelSelection()")
        End Try

    End Sub


#Region "Helper Function"
    ''====================================================================
    ''  Helper Functions 
    ''====================================================================
    ''
    ''  Helper function to display info from a list of elements passed onto. 
    ''  (Same as Revit Intro Lab3.) 
    '' 
    Sub ShowElementList(ByVal elems As IList(Of Element), ByVal header As String)

        Dim s As String = header + "(" + elems.Count.ToString + ")" + vbCr + vbCr
        s = s + " - Class - Category - Name (or Family: Type Name) - Id - " + vbCr
        For Each elem As Element In elems
            s = s + ElementToString(elem)
        Next
        TaskDialog.Show("Revit UI Lab", s)

    End Sub


    ''  Helper Funtion: summarize an element information as a line of text, 
    ''  which is composed of: class, category, name and id. 
    ''  name will be "Family: Type" if a given element is ElementType. 
    ''  Intended for quick viewing of list of element, for example. 
    ''  (Same as Revit Intro Lab3.) 
    ''
    Function ElementToString(ByVal elem As Element) As String

        If elem Is Nothing Then
            Return "none"
        End If

        Dim name As String = ""

        If TypeOf elem Is ElementType Then
            Dim param As Parameter = elem.Parameter(BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM)
            If param IsNot Nothing Then
                name = param.AsString
            End If
        Else
            name = elem.Name
        End If

        Return elem.GetType.Name + "; " + elem.Category.Name + "; " _
        + name + "; " + elem.Id.IntegerValue.ToString + vbCr

    End Function

    ''  Helper Function: returns XYZ in a string form.  
    ''  (Same as Revit Intro Lab2)
    '' 
    Public Shared Function PointToString(ByVal pt As XYZ) As String

        If pt Is Nothing Then
            Return ""
        End If

        Return "(" + pt.X.ToString("F2") + ", " + pt.Y.ToString("F2") + ", " + pt.Z.ToString("F2") + ")"

    End Function

#End Region

End Class

''  selection filter that limit the type of object being picked as wall. 
''
Class SelectionFilterWall
    Implements ISelectionFilter

    Public Function AllowElement(ByVal elem As Element) As Boolean Implements ISelectionFilter.AllowElement

        If elem.Category.Id.IntegerValue = BuiltInCategory.OST_Walls Then
            Return True
        End If
        Return False

    End Function

    Public Function AllowReference(ByVal reference As Reference, ByVal position As XYZ) As Boolean Implements ISelectionFilter.AllowReference

        Return True

    End Function

End Class

''  selection filter that limit the reference type to be planar face 
''
Class SelectionFilterPlanarFace
    Implements ISelectionFilter

    Public Function AllowElement(ByVal elem As Element) As Boolean Implements ISelectionFilter.AllowElement

        Return True

    End Function

    Public Function AllowReference(ByVal reference As Reference, ByVal position As XYZ) As Boolean Implements ISelectionFilter.AllowReference

        '' example: if you want to allow only Planar Face and do some more checking, add this. 
        If (TypeOf (reference.GeometryObject) Is PlanarFace) Then
            '' do additional checking here if you want to 
            Return True
        End If
        Return False

    End Function

End Class

''
''  Create House with UI added 
'' 
''  ask the user to pick two corner points of walls. 
''  then ask to choose a wall to add a front door. 
''  
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class CreateHouseUI
    Implements IExternalCommand

    ''  member variables 
    Dim m_rvtUIApp As UIApplication
    Dim m_rvtUIDoc As UIDocument
    'Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        ''  Get the access to the top most objects. (we may not use them all in this specific lab.) 
        m_rvtUIApp = commandData.Application
        m_rvtUIDoc = m_rvtUIApp.ActiveUIDocument
        m_rvtDoc = m_rvtUIDoc.Document

        CreateHouseInteractive(m_rvtUIDoc)

        Return Result.Succeeded

    End Function

    ''  create a simple house with user interactions. 
    ''  the user is asekd to pick two corners of rectangluar footprint of a house.
    ''  then which wall to place a front door. 
    ''
    Public Shared Sub CreateHouseInteractive(ByVal rvtUIDoc As UIDocument)

        ''  (1) Walls 
        ''  pick two corners to place a house with an orthogonal rectangular footprint 
        Dim pt1 As XYZ = rvtUIDoc.Selection.PickPoint("Pick the first corner of walls")
        Dim pt2 As XYZ = rvtUIDoc.Selection.PickPoint("Pick the second corner")

        ''  simply create four walls with orthogonal rectangular profile from the two points picked.  
        Dim walls As List(Of Wall) = RevitIntroVB.ModelCreation.CreateWalls(rvtUIDoc.Document, pt1, pt2)

        ''  (2) Door 
        ''  pick a wall to add a front door
        Dim selFilterWall As New SelectionFilterWall
        Dim ref As Reference = rvtUIDoc.Selection.PickObject( _
            ObjectType.Element, selFilterWall, "Select a wall to place a front door")
        Dim wallFront As Wall = ref.Element

        ''  add a door to the selected wall
        RevitIntroVB.ModelCreation.AddDoor(rvtUIDoc.Document, wallFront)

        ''  (3) Windows 
        ''  add windows to the rest of the walls. 
        For i As Integer = 0 To 3
            If Not (walls(i).Id.IntegerValue = wallFront.Id.IntegerValue) Then
                RevitIntroVB.ModelCreation.AddWindow(rvtUIDoc.Document, walls(i))
            End If
        Next

        ''  (4) Roofs 
        ''  add a roof over the walls' rectangular profile. 
        RevitIntroVB.ModelCreation.AddRoof(rvtUIDoc.Document, walls)

    End Sub

End Class
