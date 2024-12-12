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
#End Region

Namespace Labs

#Region "Lab2_0_CreateLittleHouse"
    ''' <summary>
    ''' Create a little house with some sample building elements.
    ''' We create a simple building consisting of four walls,
    ''' a door, two windows, a floor, a roof, a room and a room tag.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="2-0"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_0_CreateLittleHouse
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            Try
                Dim waitCursor As New WaitCursor()
                Dim app As UIApplication = commandData.Application
                Dim doc As Document = app.ActiveUIDocument.Document
                Dim createApp As Autodesk.Revit.Creation.Application = app.Application.Create
                Dim createDoc As Autodesk.Revit.Creation.Document = doc.Create
                '
                ' determine the four corners of the rectangular house:
                '
                Dim width As Double = 7 * LabConstants.MeterToFeet
                Dim depth As Double = 4 * LabConstants.MeterToFeet

                Dim corners As New List(Of XYZ)(4)

                corners.Add(XYZ.Zero)
                corners.Add(New XYZ(width, 0, 0))
                corners.Add(New XYZ(width, depth, 0))
                corners.Add(New XYZ(0, depth, 0))
                '
                ' determine the levels where the walls will be located:
                '
                Dim levelBottom As Level = Nothing
                Dim levelTop As Level = Nothing

                If Not LabUtils.GetBottomAndTopLevels(doc, levelBottom, levelTop) Then
                    message = "Unable to determine wall bottom and top levels"
                    Return Result.Failed
                End If
                Debug.Print(String.Format("Drawing walls on '{0}' up to '{1}'", levelBottom.Name, levelTop.Name))
                '
                ' create the walls:
                '
                Dim topLevelParam As BuiltInParameter = BuiltInParameter.WALL_HEIGHT_TYPE
                Dim topLevelId As ElementId = levelTop.Id
                Dim walls As New List(Of Wall)(4)
                For i As Integer = 0 To 3
                    Dim line As Line = createApp.NewLineBound(corners(i), corners(If(3 = i, 0, i + 1)))
                    Dim wall As Wall = createDoc.NewWall(line, levelBottom, False)
                    Dim param As Parameter = wall.Parameter(topLevelParam)
                    param.[Set](topLevelId)
                    walls.Add(wall)
                Next
                '
                ' determine wall thickness for tag offset and profile growth:
                '
                Dim wallThickness As Double = walls(0).WallType.CompoundStructure.Layers.Item(0).Thickness
                '
                ' add door and windows to the first wall;
                ' note that the NewFamilyInstance() api method does not automatically add door
                ' and window tags, like the ui command does. we add tags here by making additional calls
                ' to NewTag():
                '
                Dim door As FamilySymbol = LabUtils.GetFirstFamilySymbol(doc, BuiltInCategory.OST_Doors)
                Dim window As FamilySymbol = LabUtils.GetFirstFamilySymbol(doc, BuiltInCategory.OST_Windows)
                Dim midpoint As XYZ = LabUtils.Midpoint(corners(0), corners(1))
                Dim p As XYZ = LabUtils.Midpoint(corners(0), midpoint)
                Dim q As XYZ = LabUtils.Midpoint(midpoint, corners(1))
                Dim tagOffset As Double = 3 * wallThickness
                'double windowHeight = 1 * LabConstants.MeterToFeet;
                Dim windowHeight As Double = levelBottom.Elevation + 0.3 * (levelTop.Elevation - levelBottom.Elevation)
                p = New XYZ(p.X, p.Y, windowHeight)
                q = New XYZ(q.X, q.Y, windowHeight)
                Dim view As View = doc.ActiveView
                Dim inst As FamilyInstance = createDoc.NewFamilyInstance( _
                    midpoint, door, walls(0), levelBottom, StructuralType.NonStructural)
                midpoint += tagOffset * XYZ.BasisY
                Dim tag As IndependentTag = createDoc.NewTag( _
                    view, inst, False, TagMode.TM_ADDBY_CATEGORY, TagOrientation.TAG_HORIZONTAL, midpoint)
                inst = createDoc.NewFamilyInstance(p, window, walls(0), levelBottom, StructuralType.NonStructural)
                p += tagOffset * XYZ.BasisY
                tag = createDoc.NewTag(view, inst, False, TagMode.TM_ADDBY_CATEGORY, TagOrientation.TAG_HORIZONTAL, p)
                inst = createDoc.NewFamilyInstance(q, window, walls(0), levelBottom, StructuralType.NonStructural)
                q += tagOffset * XYZ.BasisY
                tag = createDoc.NewTag(view, inst, False, TagMode.TM_ADDBY_CATEGORY, TagOrientation.TAG_HORIZONTAL, q)
                '
                ' grow the profile out by half the wall thickness,
                ' so the floor and roof do not stop halfway through the wall:
                '
                Dim w As Double = 0.5 * wallThickness
                corners(0) -= w * (XYZ.BasisX + XYZ.BasisY)
                corners(1) += w * (XYZ.BasisX - XYZ.BasisY)
                corners(2) += w * (XYZ.BasisX + XYZ.BasisY)
                corners(3) -= w * (XYZ.BasisX - XYZ.BasisY)
                Dim profile As New CurveArray()
                For i As Integer = 0 To 3
                    Dim line As Line = createApp.NewLineBound(corners(i), corners(If(3 = i, 0, i + 1)))
                    profile.Append(line)
                Next
                '
                ' add a floor, a roof, the roof slope, a room and a room tag:
                '
                Dim structural As Boolean = False
                Dim floor As Floor = createDoc.NewFloor(profile, structural)
                Dim roofTypes As New List(Of Element)(LabUtils.GetElementsOfType(doc, GetType(RoofType), BuiltInCategory.OST_Roofs))
                Debug.Assert(0 < roofTypes.Count, "expected at least one roof type to be loaded into project")
                Dim roofType As RoofType = TryCast(roofTypes(0), RoofType)
                Dim modelCurves As New ModelCurveArray()
                Dim roof As FootPrintRoof = createDoc.NewFootPrintRoof(profile, levelTop, roofType, modelCurves)
                '
                ' regenerate the model after creation, otherwise both the calls
                ' to set_DefinesSlope and set_SlopeAngle throwing the exception
                ' "Unable to access curves from the roof sketch."
                '
                doc.Regenerate()
                Dim slopeAngle As Double = 30 * LabConstants.DegreesToRadians
                For Each curve As ModelCurve In modelCurves
                    Dim bDefSlope As Boolean = roof.DefinesSlope(curve)
                    slopeAngle = roof.SlopeAngle(curve)
                Next
                Dim room As Room = createDoc.NewRoom(levelBottom, New UV(0.5 * width, 0.5 * depth))
                Dim roomTag As RoomTag = createDoc.NewRoomTag(room, New UV(0.5 * width, 0.7 * depth), Nothing)
                'LabUtils.InfoMsg( "Little house was created successfully." );
                Return Result.Succeeded
            Catch ex As Exception
                message = ex.Message
                Return Result.Failed
            End Try
        End Function
    End Class

#End Region

#Region "Lab2_1_Elements"
    ' Cf. C:\a\doc\revit\2011\constellation\get_all_elements.txt

    ''' <summary>
    ''' List all document elements.
    ''' This is not recommended for normal use!
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="2-1"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.ReadOnly)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_1_Elements
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            '#region 2.1.a. Access Revit doc and open output file:


            ' Typical .NET error checking (should be done everywhere,
            ' but will be omitted for clarity in some of the following
            ' labs unless we expect exceptions):


            '#endregion 2.1.a. Access Revit doc and open output file

            Try
                Dim waitCursor As New WaitCursor()

                '#region 2.1.b. Set up element collector to retrieve all elements:

                ' the Revit API does not expect an application
                ' to ever need to iterate over all elements.
                ' To do so, we need to use a trick: ask for all
                ' elements fulfilling a specific criteria and
                ' unite them with all elements NOT fulfilling
                ' the same criteria:




                '#endregion 2.1.b. Set up element collector to retrieve all elements

                '#region 2.1.c. Loop over the elements, list their data, and close the file:


                    ' element id
                    ' element class, i.e. System.Type
                    ' The element category is not implemented for all classes,
                    ' and may return null; for Family elements, one can sometimes
                    ' use the FamilyCategory property instead.



                    ' The element Name has a different meaning for different classes,
                    ' but is mostly implemented "logically". More precise info on elements
                    ' can be obtained in class-specific ways.

                    'line += "; Guid=" + GetGuid( e.UniqueId );



                '#endregion 2.1.c. Loop over the elements, list their data, and close the file

            Catch e As Exception
                message = e.Message
            End Try
            Return Result.Failed

        End Function
    End Class

#End Region

#Region "Lab2_2_ModelElements"
    ''' <summary>
    ''' List all model elements.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="2-2"]/*' />
    ''' </summary>
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="2-2-remarks"]/*' />
    <Transaction(TransactionMode.ReadOnly)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_2_ModelElements
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            '#region 2.2 List all model elements:




            ' we could use a LINQ query here instead:

                ' && null != e.Materials
                ' && 0 < e.Materials.Size



            '#endregion 2.2 List all model elements

        End Function

    End Class
#End Region

#Region "Lab2_3_ListWallsAndDoors"
    ''' <summary>
    ''' Retrieving family instances: list all walls and doors.
    '''
    ''' These represent two different kinds of elements:
    ''' walls are represented by their own specialised
    ''' System.Type Wall, whereas doors are represented
    ''' by family instances and need to be identified by
    ''' additionally checking the category.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="2-3"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.ReadOnly)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_3_ListWallsAndDoors
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim doc As Document = app.ActiveUIDocument.Document

            '#region 2.3.a Filter to retrieve and list all walls:
            ' get all wall elements:
            '
            ' we could also call
            '
            ' Dim walls As FilteredElementCollector =
            ' LabUtils.GetElementsOfType(doc, GetType(Wall), BuiltInCategory.OST_Walls)




            '#endregion 2.3.a Filter to retrieve and list all walls

            a.Clear()

            '#region 2.3.b Filter to retrieve and list all doors:
            ' get all door family instances:
            '
            ' we could also call
            '
            ' Dim doors As FilteredElementCollector =
            '   LabUtils.GetElementsOfType(doc, GetType(FamilyInstance), BuiltInCategory.OST_Doors)
            '
            ' or
            '
            ' Dim doors As FilteredElementCollector =
            '   LabUtils.GetFamilyInstances(doc, BuiltInCategory.OST_Doors)


                ' For family instances, the element name property
                ' returns the type name:


            '#endregion 2.3.b Filter to retrieve and list all doors

            Return Result.Failed
        End Function
    End Class
#End Region

#Region "Lab2_4_EditFamilyInstance"
    ''' <summary>
    ''' Demonstrate access to family instance parameters
    ''' and modification of family instance location.
    '''
    ''' Edit all doors in the current project.
    ''' Move the doors up 0.2 feet via the Document.Move method
    ''' and widen them 1 foot by changing the WINDOW_WIDTH parameter value.
    '''
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="2-4"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_4_EditFamilyInstance
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim doc As Document = app.ActiveUIDocument.Document

            Try
                '#region 2.4 Retrieve all doors, move them and widen them:


                ' move doors up 0.2 feet:



                    ' widen doors by one foot by changing parameter value:




                '#endregion 2.4 Retrieve all doors, move them and widen them
            Catch ex As Exception
                message = ex.Message
                Return Result.Failed
            End Try
        End Function
    End Class
#End Region

#Region "Lab2_5_SelectAndMoveWallAndAddColumns"
    ''' <summary>
    ''' Demonstrate creation of new family instance elements
    ''' by querying an existing wall for its location and parameters,
    ''' modifying it, and inserting column elements.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="2-5"]/*' />
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: the column can be seen in 3D view by setting argument to StructuralType.Column,
    ''' but cannot by StructuralType.NonStructural, since the latter is only visible in plan view.
    ''' This is a temporary problem, NewFamilyInstance identifies the nonstructural instance
    ''' as an annotation instance, so only shows them in plan view.
    ''' </remarks>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab2_5_SelectAndMoveWallAndAddColumns
        Implements IExternalCommand

        ''' <summary>
        ''' A selection filter for wall elements.
        ''' </summary>
        Private Class WallSelectionFilter
            Implements ISelectionFilter

            'const BuiltInCategory _bic = BuiltInCategory.OST_Walls;

            ''' <summary>
            ''' Allow wall to be selected.
            ''' </summary>
            ''' <param name="e">A candidate element in selection operation.</param>
            ''' <returns>Return true for wall, false for all other elements.</returns>
            Public Function AllowElement _
                (ByVal e As Element) As Boolean _
                Implements ISelectionFilter.AllowElement
                'return null != e.Category
                ' && e.Category.Id.IntegerValue == ( int ) _bic;

                Return TypeOf e Is Wall
            End Function

            ''' <summary>
            ''' Allow all the reference to be selected
            ''' </summary>
            ''' <param name="reference">A candidate reference in selection operation.</param>
            ''' <param name="position">The 3D position of the mouse on the candidate reference.</param>
            ''' <returns>Return true to allow the user to select this candidate reference.</returns>
            Public Function AllowReference _
                (ByVal reference As Reference, _
                 ByVal position As XYZ) As Boolean _
                 Implements ISelectionFilter.AllowReference
                Return True
            End Function

        End Class

        Public Function Execute _
            (ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim uidoc As UIDocument = app.ActiveUIDocument
            Dim doc As Document = app.ActiveUIDocument.Document
            Dim ss As ElementSet = uidoc.Selection.Elements

            Dim wall As Wall = Nothing

            If 0 < ss.Size Then
                ' old pre-selection handling:

                ' must be one single element only:

                If 1 <> ss.Size Then
                    message = "Please pre-select a single wall element."
                    Return Result.Failed
                End If

                ' must be a wall:

                Dim it As ElementSetIterator = ss.ForwardIterator()
                it.MoveNext()
                Dim e As Element = TryCast(it.Current, Element)

                If Not (TypeOf e Is Wall) Then
                    message = "Selected element is NOT a wall."
                    Return Result.Failed
                End If
                wall = TryCast(e, Wall)
            Else
                ' new prompt for filtered selection allowing only walls:

                Dim r As Reference = uidoc.Selection.PickObject( _
                    ObjectType.Element, New WallSelectionFilter(), "Please pick a wall")

                wall = TryCast(r.Element, Wall)
            End If

            ' wall must be constrained to a level at the top (more on parameters later):

            Dim topLev As Level = Nothing

            Try
                Dim id As ElementId = wall.Parameter( _
                    BuiltInParameter.WALL_HEIGHT_TYPE).AsElementId()
                topLev = TryCast(doc.Element(id), Level)
            Catch generatedExceptionName As Exception
                topLev = Nothing
            End Try

            If topLev Is Nothing Then
                message = "Selected wall is not constrained to a level at the top."
                Return Result.Failed
            End If

            ' get the bottom level as well (this should never fail):

            Dim botLev As Level = Nothing

            Try
                Dim id As ElementId = wall.Parameter( _
                    BuiltInParameter.WALL_BASE_CONSTRAINT).AsElementId()
                botLev = TryCast(doc.Element(id), Level)
            Catch generatedExceptionName As Exception
                botLev = Nothing
            End Try

            If botLev Is Nothing Then
                message = "Selected wall is not constrained to a level at the bottom."
                Return Result.Failed
            End If

            ' Calculate the location points for the 3 columns (assuming straight wall)
            Dim locCurve As LocationCurve = TryCast(wall.Location, LocationCurve)

            Dim ptStart As XYZ = locCurve.Curve.EndPoint(0)
            Dim ptEnd As XYZ = locCurve.Curve.EndPoint(1)
            Dim ptMid As XYZ = 0.5 * (ptStart + ptEnd)

            Dim locations As New List(Of XYZ)(3)
            locations.Add(ptStart)
            locations.Add(ptMid)
            locations.Add(ptEnd)

            Dim s As String = _
                "{0} location{1} for the new columns in raw database coordinates, e.g. feet{2}"
            Dim a As New List(Of String)()
            a.Add("Start: " + LabUtils.PointString(ptStart))
            a.Add("Mid : " + LabUtils.PointString(ptMid))
            a.Add("End : " + LabUtils.PointString(ptEnd))
            LabUtils.InfoMsg(s, a)

            ' retrieve the family type for the new instances.
            ' if needed, change the names to match a column
            ' type available in the model:

            Dim family_name As String = "M_Wood Timber Column"
            Dim type_name As String = "191 x 292mm"

            Dim collector As New FilteredElementCollector(doc)
            collector.OfCategory(BuiltInCategory.OST_Columns)
            collector.OfClass(GetType(FamilySymbol))

            ' LINQ query to find element with given name:
            '
            ' ... note that this could also be achieved by
            ' filtering for the element name parameter value.

            'where ((FamilySymbol)element).Family.Name == family_name
            'Dim column_types = From element In collector _
            '    Where element.Name = type_name _
            '    Select Element

            'Dim column_types_ienum As IEnumerable(Of Element)
            'column_types_ienum = CType(column_types, IEnumerable(Of Element))

            'Dim column_types_famsym As IEnumerable(Of FamilySymbol)
            'column_types_famsym = column_types_ienum.Cast(Of FamilySymbol)()

            Dim symbol As FamilySymbol = Nothing

            Try
                'symbol = column_types_famsym.First()

                Dim name_equals As Func(Of Element, Boolean) _
                    = Function(e As Element) e.Name.Equals(type_name)

                Dim element As Element = collector.First(name_equals)

                symbol = CType(element, FamilySymbol)
            Catch
            End Try

            If symbol Is Nothing Then
                message = String.Format( _
                    "Cannot find type '{0}' in family '{1}' in the current model - please load it first.", _
                    type_name, family_name)
                Return Result.Failed
            End If

            ' insert column family instances:

            For Each p As XYZ In locations
                Try
                    ' Note: Currently there is a problem.
                    ' If we set the type as NonStructural, it is treated as Annotation instance,
                    ' and it shows only in plan view.
                    ' FamilyInstance column = doc.Create.NewFamilyInstance( p, symbol, botLev, StructuralType.NonStuctural );

                    Dim column As FamilyInstance = doc.Create.NewFamilyInstance( _
                        p, symbol, botLev, StructuralType.Column)
                    Dim paramTopLevel As Parameter = column.Parameter( _
                        BuiltInParameter.FAMILY_TOP_LEVEL_PARAM)
                    Dim id As ElementId = topLev.Id
                    paramTopLevel.[Set](id)
                Catch generatedExceptionName As Exception
                    LabUtils.ErrorMsg("Failed to create or adjust column.")
                End Try
            Next

            ' Finally, move the wall so the columns are visible.
            ' We move the wall perpendicularly to its location
            ' curve by one tenth of its length:

            Dim v As New XYZ(-0.1 * (ptEnd.Y - ptStart.Y), 0.1 * (ptEnd.X - ptStart.X), 0)

            If Not wall.Location.Move(v) Then
                LabUtils.ErrorMsg("Failed to move the wall.")
            End If
            Return Result.Succeeded
        End Function

    End Class
#End Region

End Namespace

