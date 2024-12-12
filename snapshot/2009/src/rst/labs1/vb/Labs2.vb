
' List all STRUCTURAL elements in the model
' -----------------------------------------

'Structural COLUMNS and FRAMING elements
Public Class Lab2_1
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute
        Dim app As Revit.Application = commandData.Application

        ' Get all Structural COLUMNS - we can use generic utility
        ' In 8.1 we had to hard-code the category name which then works only in specific locale (EN or DE or IT etc)...
        'Dim columns As ElementSet = RacUtils.GetAllStandardFamilyInstancesForACategory(app, "Structural Columns")
        '...but from 9.0 there is new category enum, so this should work in ANY locale:
        Dim catStructuralColumns As Category = app.ActiveDocument.Settings.Categories.Item(BuiltInCategory.OST_StructuralColumns)
        Dim columns As ElementSet = RacUtils.GetAllStandardFamilyInstancesForACategory(app, catStructuralColumns.Name)

        Dim sMsg As String = "There are " & columns.Size & " Structural COLUMNS elements:" & vbCrLf
        Dim col As FamilyInstance
        '" Struct.Usage=" & col.StructuralUsage.ToString & _ ' only beam and brace allow strctural usage in 2008
        For Each col In columns
            sMsg += "  Id=" & col.Id.Value.ToString & " Type=" & col.Symbol.Name & _
                    " Struct.Type=" & col.StructuralType.ToString & _
                    " Analytical Type=" & col.AnalyticalModel.GetType.Name & vbCrLf
        Next
        MsgBox(sMsg)

        ' Get all Structural FRAMING elements - again the same
        Dim catStructuralFraming As Category = app.ActiveDocument.Settings.Categories.Item(BuiltInCategory.OST_StructuralFraming)
        Dim frmEls As ElementSet = RacUtils.GetAllStandardFamilyInstancesForACategory(app, catStructuralFraming.Name)
        sMsg = "There are " & frmEls.Size & " Structural FRAMING elements:" & vbCrLf
        Dim frmEl As FamilyInstance
        For Each frmEl In frmEls
            ' INSTANCE_STRUCT_USAGE_TEXT_PARAM works only in 8.1 and not in 9!...
            'sMsg += "  Id=" & frmEl.Id.Value.ToString & " Type=" & frmEl.Symbol.Name & _
            '        " Struct.Usage=" & RacUtils.GetParamAsString(frmEl.Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_TEXT_PARAM)) & _
            '         " Analytical Type=" & frmEl.AnalyticalModel.GetType.Name & vbCrLf
            ' ..so better use dedicated class' property StructuralUsage which works in both. Also check StructuralType
            sMsg += "  Id=" & frmEl.Id.Value.ToString & " Type=" & frmEl.Symbol.Name & _
                    " Struct.Usage=" & frmEl.StructuralUsage.ToString & _
                    " Struct.Type=" & frmEl.StructuralType.ToString & _
                    " Analytical Type=" & frmEl.AnalyticalModel.GetType.Name & vbCrLf

        Next
        MsgBox(sMsg)

        Return IExternalCommand.Result.Succeeded
    End Function
End Class


' Structural FOUNDATION elements and any standard family in alternative way
Public Class Lab2_2
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute
        Dim app As Revit.Application = commandData.Application

        ' Get all standard Structural FOUNDATION elements - again the same. Note that this:
        '   a)  excludes "Wall Foundation" System Type under "Structural Foundations" category in the Browser - these belong to *Continuous Footing* system family, see next Lab
        '   b)  excludes "Foundation Slab" System Type under "Structural Foundations" category in the Browser - these are internally implemented as Revit *Floor* system family, see next Lab
        Dim catStructuralFoundation As Category = app.ActiveDocument.Settings.Categories.Item(BuiltInCategory.OST_StructuralFoundation)
        Dim struFnds As ElementSet = RacUtils.GetAllStandardFamilyInstancesForACategory(app, catStructuralFoundation.Name)

        Dim sMsg As String = "There are " & struFnds.Size & " Structural FOUNDATION (standard families only) elements :" & vbCrLf
        Dim found As FamilyInstance
        ' " Struct.Usage=" & found.StructuralUsage.ToString & _only Beam and Brace support Structural Usage!
        For Each found In struFnds
            sMsg += "  Id=" & found.Id.Value.ToString & " Type=" & found.Symbol.Name & _
                    " Struct.Type=" & found.StructuralType.ToString & _
                    " Analytical Type=" & found.AnalyticalModel.GetType.Name & vbCrLf
        Next
        MsgBox(sMsg)

        ' NOTE: All of the previous 3 are *standard* Family Instances, so we could alternatively get them by using something like:
        sMsg = "ALL Structural FAMILY INSTANCES (generic check):" & vbCrLf
        Dim categoryName As String
        Dim i As Integer = 0
        'The following commented code is for Revit 2008 and previous version. It works in Revit 2009 and afterward version too. However this method has a low performance. . It works in Revit 2009 and afterward version too. However this method has a low performance.
        'There is a new feature named Element filter in Revit 2009 API, which can improve the performance.
        'Dim iter As IEnumerator = app.ActiveDocument.Elements
        'Do While (iter.MoveNext())
        '    Dim elem As Revit.Element = iter.Current

        'The following code uses the element filter method that provided in Revit 2009 API
        Dim filterType As Revit.Filter = app.Create.Filter.NewTypeFilter(GetType(FamilyInstance))
        Dim listFamilyInstances As List(Of Revit.Element) = New List(Of Revit.Element)()
        Dim bRetVal As Boolean = app.ActiveDocument.Elements(filterType, listFamilyInstances)
        For Each elem As Revit.Element In listFamilyInstances
            If TypeOf elem Is FamilyInstance Then
                Dim fi As FamilyInstance = elem
                Dim anaMod As AnalyticalModel = fi.AnalyticalModel
                If Not anaMod Is Nothing Then
                    i += 1
                    categoryName = "?"
                    Try
                        categoryName = fi.Category.Name
                    Catch
                    End Try
                    sMsg += i & ": Category=" & categoryName & _
                                "  Struct.Type=" & fi.StructuralType.ToString & _
                                "  Id= " & fi.Id.Value.ToString & vbCrLf
                End If
            End If
        Next
        MsgBox(sMsg)

        Return IExternalCommand.Result.Succeeded
    End Function
End Class


' Structural System Families: WALL, FLOOR, CONTINUOUS FOOTING
Public Class Lab2_3
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute
        Dim app As Revit.Application = commandData.Application

        ' Get all Structural WALLS elements - dedicated helper that checks for all Walls of Structural usage
        Dim struWalls As ElementSet = RstUtils.GetAllStructuralWalls(app)
        Dim sMsg As String = "There are " & struWalls.Size & " Structural WALLS elements:" & vbCrLf
        Dim w As Wall
        For Each w In struWalls
            ' WALL_STRUCTURAL_USAGE_TEXT_PARAM works only in 8.1 and not from 9!...
            'sMsg += "  Id=" & w.Id.Value.ToString & " Type=" & w.WallType.Name & _
            '        " Struct.Usage=" & RacUtils.GetParamAsString(w.Parameter(BuiltInParameter.WALL_STRUCTURAL_USAGE_TEXT_PARAM)) & _
            '        " Analytical Type=" & w.AnalyticalModel.GetType.Name & vbCrLf
            ' ..so better use dedicated class' property StructuralUsage which works in both
            sMsg += "  Id=" & w.Id.Value.ToString & " Type=" & w.WallType.Name & _
                    " Struct.Usage=" & w.StructuralUsage.ToString & _
                    " Analytical Type=" & w.AnalyticalModel.GetType.Name & vbCrLf
        Next
        MsgBox(sMsg)

        ' Get all Structural FLOOR elements - dedicated helper that checks for all Floors of Structural usage
        'NOTE: From RS3, these include not only standard Floors, but also "Foundation Slab" instances from "Structural Foundations" category
        Dim struFloors As ElementSet = RstUtils.GetAllStructuralFloors(app)
        sMsg = "There are " & struFloors.Size & " Structural FLOOR elements:" & vbCrLf
        Dim fl As Floor
        For Each fl In struFloors
            sMsg += "  Id=" & fl.Id.Value.ToString & _
                   "  Category=" & fl.Category.Name & _
                   "  Type=" & fl.FloorType.Name & _
                   "  Analytical Type=" & fl.AnalyticalModel.GetType.Name & vbCrLf
        Next
        MsgBox(sMsg)

        ' Get all Structural CONTINUOUS FOOTING elements - dedicated helper
        'NOTE: From RS3, these are "Wall Foundation" instances from "Structural Foundations" category
        Dim contFootings As ElementSet = RstUtils.GetAllStructuralContinuousFootings(app)
        sMsg = "There are " & contFootings.Size & " Structural CONTINUOUS FOOTING (or Wall Foundations) elements:" & vbCrLf
        Dim cf As ContFooting
        For Each cf In contFootings
            sMsg += "  Id=" & cf.Id.Value.ToString & " Type=" & cf.FootingType.Name & " Analytical Type=" & cf.AnalyticalModel.GetType.Name & vbCrLf
        Next
        MsgBox(sMsg)

        Return IExternalCommand.Result.Succeeded
    End Function
End Class

''Some Built-in params that seem relevant for structural elements:

'FLOOR_PARAM_IS_STRUCTURAL

'INSTANCE_STRUCT_USAGE_TEXT_PARAM
'INSTANCE_STRUCT_USAGE_PARAM
'INSTANCE_STRUCT_INSERTION_PARAM
'INSTANCE_STRUCT_SETBACK1_PARAM
'INSTANCE_STRUCT_SETBACK0_PARAM

'STRUCTURAL_BOTTOM_RELEASE_MZ
'STRUCTURAL_BOTTOM_RELEASE_MY
'STRUCTURAL_BOTTOM_RELEASE_MX
'STRUCTURAL_BOTTOM_RELEASE_FZ
'STRUCTURAL_BOTTOM_RELEASE_FY
'STRUCTURAL_BOTTOM_RELEASE_FX
'STRUCTURAL_TOP_RELEASE_MZ
'STRUCTURAL_TOP_RELEASE_MY
'STRUCTURAL_TOP_RELEASE_MX
'STRUCTURAL_TOP_RELEASE_FZ
'STRUCTURAL_TOP_RELEASE_FY
'STRUCTURAL_TOP_RELEASE_FX
'STRUCTURAL_BOTTOM_RELEASE_TYPE
'STRUCTURAL_TOP_RELEASE_TYPE
'STRUCTURAL_ANALYTICAL_PROJECT_MEMBER_PLANE_COLUMN_BOTTOM
'STRUCTURAL_ANALYTICAL_PROJECT_MEMBER_PLANE_COLUMN_TOP
'STRUCTURAL_ANALYTICAL_PROJECT_MEMBER_PLANE_WALL_BOTTOM
'STRUCTURAL_ANALYTICAL_PROJECT_MEMBER_PLANE_WALL_TOP
'STRUCTURAL_MATERIAL_TYPE
'STRUCTURAL_CAMBER
'STRUCTURAL_NUMBER_OF_STUDS
'STRUCTURAL_END_RELEASE_MZ
'STRUCTURAL_END_RELEASE_MY
'STRUCTURAL_END_RELEASE_MX
'STRUCTURAL_END_RELEASE_FZ
'STRUCTURAL_END_RELEASE_FY
'STRUCTURAL_END_RELEASE_FX
'STRUCTURAL_START_RELEASE_MZ
'STRUCTURAL_START_RELEASE_MY
'STRUCTURAL_START_RELEASE_MX
'STRUCTURAL_START_RELEASE_FZ
'STRUCTURAL_START_RELEASE_FY
'STRUCTURAL_START_RELEASE_FX
'STRUCTURAL_END_RELEASE_TYPE
'STRUCTURAL_START_RELEASE_TYPE
'STRUCTURAL_WALL_BOTTOM_PROJECTION_PLANE
'STRUCTURAL_WALL_TOP_PROJECTION_PLANE
'STRUCTURAL_WALL_PROJECTION_SURFACE
'STRUCTURAL_COLUMN_ANALYTICAL_OFFSET
'STRUCTURAL_ANALYTICAL_PROJECT_FLOOR_PLANE
'STRUCTURAL_ANALYTICAL_PROJECT_MEMBER_PLANE_BOTTOM
'STRUCTURAL_ANALYTICAL_PROJECT_MEMBER_PLANE
'STRUCTURAL_BRACE_REPRESENTATION
'STRUCTURAL_CONNECTION_TYPE
'STRUCTURAL_MOMENT_CONN_END
'STRUCTURAL_MOMENT_CONN_START
'STRUCTURAL_STICK_SYMBOL_ON_TOP
'STRUCTURAL_BEAM_END_SUPPORT
'STRUCTURAL_BEAM_START_SUPPORT
