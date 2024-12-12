#Region "Imports"
'' Import the following name spaces in the project properties/references. 
'' Note: VB.NET has a slighly different way of recognizing name spaces than C#. 
'' if you explicitely set them in each .vb file, you will need to specify full name spaces. 

'Imports System
'Imports Autodesk.Revit.DB
'Imports Autodesk.Revit.UI
'Imports Autodesk.Revit.ApplicationServices
'Imports Autodesk.Revit.Attributes '' specific this if you want to save typing for attributes. e.g., 
Imports RevitIntroVB.ElementFiltering  ''  added for Lab4. 

#End Region

#Region "Description"
''  Revit Intro Lab 4 
''
''  In this lab, you will learn how to modify elements.
''  There are two places to look at when you want to modify an element. 
''  (1) at each element level, such as by modifying each properties, parameters, and location. 
''  (2) use document methods, such as move, rorat, mirror, array and delete. 
'' 
''  for #2, document.move, rotate, etc., see from pp105 of developer guide. 
'' 
'' 
''  Disclaimer: minimum error checking to focus on the main topic. 
'' 
#End Region

''  Element Modification 
''
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class ElementModification
    Implements IExternalCommand

    ''  member variables 
    Dim m_rvtApp As Application
    Dim m_rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        ''  Get the access to the top most objects. 
        Dim rvtUIApp As UIApplication = commandData.Application
        Dim rvtUIDoc As UIDocument = rvtUIApp.ActiveUIDocument
        m_rvtApp = rvtUIApp.Application
        m_rvtDoc = rvtUIDoc.Document

        ''  select an object on a screen. (We'll come back to the selection in the UI Lab later.) 
        Dim ref As Reference = rvtUIDoc.Selection.PickObject(ObjectType.Element, "Pick an element")
        ''  we have picked something. 
        Dim elem As Element = ref.Element

        ''  modify element's properties, parameters, location. use document.move, rotation, mirror.  
        ''  In eailer lab, CommandData command, we learned how to access to the wallType. i.e., 
        ''  here we'll take a look at more on the topic of accessing to elements in the interal rvt project database. 
        ''  (1) element level modification 
        'ModifyElementPropertiesWall(elem)
        ModifyElementPropertiesDoor(elem)
        m_rvtDoc.Regenerate()

        ''  select an object on a screen. (We'll come back to the selection in the UI Lab later.) 
        Dim ref2 As Reference = rvtUIDoc.Selection.PickObject(ObjectType.Element, "Pick another element")
        ''  we have picked something. 
        Dim elem2 As Element = ref2.Element

        ''  (2) you can also use document level move, rotation, mirror.
        ModifyElementByDocumentMethods(elem2)

        Return Result.Succeeded

    End Function

    ''
    ''  A sampler function to demonstrate how to modify an element through its prooperties.
    ''  Using a wall as an example here.   
    ''
    Sub ModifyElementPropertiesWall(ByVal elem As Element)

        ''  Constant to this function.         
        ''  this is for wall. e.g., "Basic Wall: Exterior - Brick on CMU"
        ''  you can modify this to fit your need.   
        '' 
        Const wallFamilyName As String = "Basic Wall"
        Const wallTypeName As String = "Exterior - Brick on CMU"
        Const wallFamilyAndTypeName As String = wallFamilyName + ": " + wallTypeName

        ''  for simplicity, we assume we can only modify a wall 
        If Not (TypeOf elem Is Wall) Then
            TaskDialog.Show("Revit Intro Lab", "Sorry, I only know how to modify a wall. Please select a wall.")
            Return
        End If
        Dim aWall As Wall = elem

        Dim msg As String = "Wall changed: " + vbCr + vbCr  ''keep the message to the user. 

        ''  (1) change its family type to a different one.  
        ''   To Do: change this to enhance import symbol later. 
        '' 
        Dim newWallType As Element = _
        ElementFiltering.FindFamilyType(m_rvtDoc, GetType(WallType), wallFamilyName, wallTypeName)

        If newWallType IsNot Nothing Then
            aWall.WallType = newWallType
            msg = msg + "Wall type to: " + wallFamilyAndTypeName + vbCr
            'TaskDialog.Show("Revit Intro Lab", msg)
        End If

        ''  (2) change its parameters. 
        ''  as a way of exercise, let's constrain top of the wall to the level1 and set an offset. 

        ''  find the level 1 using the helper function we defined in the lab3. 
        Dim level1 As Level = ElementFiltering.FindElement(m_rvtDoc, GetType(Level), "Level 1")
        If level1 IsNot Nothing Then
            aWall.Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(level1.Id) '' Top Constraint 
            msg = msg + "Top Constraint to: Level 1" + vbCr
        End If

        Dim topOffset As Double = mmToFeet(5000.0) '' hard coding for simplisity here. 
        aWall.Parameter(BuiltInParameter.WALL_TOP_OFFSET).Set(topOffset) '' Top Offset Double 
        aWall.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_PARAM).Set(1) '' Structural Usage = Bearing(1)  
        aWall.Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("Modified by API")  '' Comments - String  

        msg = msg + "Top Offset to: 5000.0" + vbCr
        msg = msg + "Structural Usage to: Bearing" + vbCr
        msg = msg + "Comments added: Modified by API" + vbCr
        'TaskDialog.Show("Revit Intro Lab", msg)

        ''  (3) Optional: change its location, using location curve 
        ''  LocationCurve also has move and rotation methods.  
        ''  Note: constaints affect the result.
        ''  Effect will be more visible with disjoined wall.
        ''  To test this, you may want to draw a single standing wall, and run this command. 
        '' 
        Dim wallLocation As LocationCurve = aWall.Location
        Dim pt1 As XYZ = wallLocation.Curve.EndPoint(0)
        Dim pt2 As XYZ = wallLocation.Curve.EndPoint(1)
        '' hard coding the displacement value for simility here. 
        Dim dt As Double = mmToFeet(1000.0)
        Dim newPt1 = New XYZ(pt1.X - dt, pt1.Y - dt, pt1.Z)
        Dim newPt2 = New XYZ(pt2.X - dt, pt2.Y - dt, pt2.Z)
        ''  create a new line bound. 
        Dim newWallLine As Line = m_rvtApp.Create.NewLineBound(newPt1, newPt2)
        '' finally change the curve.
        wallLocation.Curve = newWallLine

        msg = msg + "Location: start point moved -1000.0 in X-direction" + vbCr
        ''  message to the user. 
        TaskDialog.Show("Revit Intro Lab", msg)

    End Sub

    ''
    ''  A sampler function to demonstrate how to modify an element through its prooperties.
    ''  Using a door as an example here.   
    ''
    Sub ModifyElementPropertiesDoor(ByVal elem As Element)

        ''  Constant to this function.         
        ''  this is for a door. e.g., "M_Single-Flush: 0762 x 2032mm"
        ''  you can modify this to fit your need.   
        '' 
        Const doorFamilyName As String = "M_Single-Flush"
        Const doorTypeName As String = "0762 x 2032mm"
        Const doorFamilyAndTypeName As String = doorFamilyName + ": " + doorTypeName

        ''  for simplicity, we assume we can only modify a door 
        If Not (TypeOf elem Is FamilyInstance) Then
            TaskDialog.Show("Revit Intro Lab", "Sorry, I only know how to modify a door. Please select a door.")
            Return
        End If
        Dim aDoor As FamilyInstance = elem

        Dim msg As String = "Door changed: " + vbCr + vbCr  ''keep the message to the user. 

        ''  (1) change its family type to a different one.  
        ''   To Do: change this to enhance import symbol later. 
        '' 
        Dim newDoorType As Element = _
        ElementFiltering.FindFamilyType(m_rvtDoc, GetType(FamilySymbol), _
            doorFamilyName, doorTypeName, BuiltInCategory.OST_Doors)

        If newDoorType IsNot Nothing Then
            aDoor.Symbol = newDoorType
            msg = msg + "Door type to: " + doorFamilyAndTypeName + vbCr
            'TaskDialog.Show("Revit Intro Lab", msg)
        End If

        ''  (2) change its parameters. 
        ''  as a way of exercise, let's constrain top of the wall to the level1 and set an offset. 

        ' ''  find the level 1 using the helper function we defined in the lab3. 
        'Dim level1 As Level = ElementFiltering.FindElement(m_rvtDoc, GetType(Level), "Level 1")
        'If level1 IsNot Nothing Then
        '    aWall.Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(level1.Id) '' Top Constraint 
        '    msg = msg + "Top Constraint to: Level 1" + vbCr
        'End If

        'Dim topOffset As Double = mmToFeet(5000.0) '' hard coding for simplisity here. 
        'aWall.Parameter(BuiltInParameter.WALL_TOP_OFFSET).Set(topOffset) '' Top Offset Double 
        'aWall.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_PARAM).Set(1) '' Structural Usage = Bearing(1)  
        'aWall.Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("Modified by API")  '' Comments - String  

        'msg = msg + "Top Offset to: 5000.0" + vbCr
        'msg = msg + "Structural Usage to: Bearing" + vbCr
        'msg = msg + "Comments added: Modified by API" + vbCr
        ''TaskDialog.Show("Revit Intro Lab", msg)

        ''  (3) Optional: change its location, using location curve 
        ''  LocationCurve also has move and rotation methods.  
        ''  Note: constaints affect the result.
        ''  Effect will be more visible with disjoined wall.
        ''  To test this, you may want to draw a single standing wall, and run this command. 
        '' 
        'Dim wallLocation As LocationCurve = aWall.Location
        'Dim pt1 As XYZ = wallLocation.Curve.EndPoint(0)
        'Dim pt2 As XYZ = wallLocation.Curve.EndPoint(1)
        ' '' hard coding the displacement value for simility here. 
        'Dim dt As Double = mmToFeet(1000.0)
        'Dim newPt1 = New XYZ(pt1.X - dt, pt1.Y - dt, pt1.Z)
        'Dim newPt2 = New XYZ(pt2.X - dt, pt2.Y - dt, pt2.Z)
        ' ''  create a new line bound. 
        'Dim newWallLine As Line = m_rvtApp.Create.NewLineBound(newPt1, newPt2)
        ' '' finally change the curve.
        'wallLocation.Curve = newWallLine

        'msg = msg + "Location: start point moved -1000.0 in X-direction" + vbCr

        ''  message to the user. 
        TaskDialog.Show("Revit Intro Lab", msg)

    End Sub

    ''
    ''  A sampler function that demonstrates how to modify an element through document methods. 
    ''
    Sub ModifyElementByDocumentMethods(ByVal elem As Element)

        Dim msg As String = "The element changed: " + vbCr + vbCr  ''keep the message to the user. 

        ''  try move 
        Dim dt As Double = mmToFeet(1000.0) '' hard cording for simplicity. 
        Dim v As XYZ = New XYZ(dt, dt, 0.0)
        m_rvtDoc.Move(elem, v)

        msg = msg + "move by (1000, 1000, 0)" + vbCr

        ''  try rotate: 15 degree around z-axis. 
        Dim pt1 = XYZ.Zero
        Dim pt2 = XYZ.BasisZ
        Dim axis As Line = m_rvtApp.Create.NewLineBound(pt1, pt2)
        m_rvtDoc.Rotate(elem, axis, Math.PI / 12.0)

        msg = msg + "rotate by 15 degree around Z-axis" + vbCr

        ''  message to the user. 
        TaskDialog.Show("Revit Intro Lab", msg)

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