
' Full analytical model listed for *selected* structural elements
Public Class Lab3
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute
        Dim doc As Revit.Document = commandData.Application.ActiveDocument

        ' get cateories needed later in the loop
        Dim CatStruCols As Category = doc.Settings.Categories.Item(BuiltInCategory.OST_StructuralColumns)
        Dim CatStruFrmg As Category = doc.Settings.Categories.Item(BuiltInCategory.OST_StructuralFraming)
        Dim CatStruFndt As Category = doc.Settings.Categories.Item(BuiltInCategory.OST_StructuralFoundation)

        Dim sMsg As String = Nothing

        Dim iter As ElementSetIterator = doc.Selection.Elements.ForwardIterator
        While iter.MoveNext
            Dim elem As Revit.Element = iter.Current

            ' Structural WALL
            If TypeOf elem Is Wall Then
                Dim w As Wall = elem
                Try
                    Dim anaWall As AnalyticalModelWall = w.AnalyticalModel
                    If Not anaWall Is Nothing Then
                        If anaWall.Curves.Size > 0 Then
                            sMsg = "Analytical Model for Wall " & w.Id.Value.ToString & vbCrLf
                            GetAnalyticalModelWall(anaWall, sMsg)
                            MsgBox(sMsg)
                        End If
                    End If
                Catch
                End Try

                ' Strucural FLOOR
            ElseIf TypeOf elem Is Floor Then
                Dim f As Floor = elem
                Try
                    Dim anaFloor As AnalyticalModelFloor = f.AnalyticalModel
                    If Not anaFloor Is Nothing Then
                        If anaFloor.Curves.Size > 0 Then
                            sMsg = "Analytical Model for Floor " & f.Id.Value.ToString & vbCrLf
                            GetAnalyticalModelFloor(anaFloor, sMsg)
                            MsgBox(sMsg)
                        End If
                    End If
                Catch
                End Try

                ' Strucural CONTINUOUS FOOTING
            ElseIf TypeOf elem Is ContFooting Then
                Dim cf As ContFooting = elem
                Try
                    Dim ana3D As AnalyticalModel3D = cf.AnalyticalModel
                    If Not ana3D Is Nothing Then
                        If ana3D.Curves.Size > 0 Then
                            sMsg = "Analytical Model for Continuous Footing " & cf.Id.Value.ToString & vbCrLf
                            GetAnalyticalModelContFooting(ana3D, sMsg)
                            MsgBox(sMsg)
                        End If
                    End If
                Catch
                End Try

                ' One of Strucural Standard Families
            ElseIf TypeOf elem Is FamilyInstance Then
                Try
                    Dim fi As FamilyInstance = elem
                    Select Case fi.Category.Name

                        Case CatStruCols.Name ' "Structural Columns" for EN locale
                            Try
                                Dim anaFrame As AnalyticalModelFrame = fi.AnalyticalModel
                                If Not anaFrame Is Nothing Then
                                    sMsg = "Analytical Model for Structural Column " & fi.Id.Value.ToString & vbCrLf
                                    ListCurve(anaFrame.Curve, sMsg)
                                    ListRigidLinks(anaFrame, sMsg)
                                    ListSupportInfo(anaFrame.SupportData, sMsg)
                                    MsgBox(sMsg)
                                End If
                            Catch
                            End Try

                        Case CatStruFrmg.Name ' "Structural Framing" for EN locale
                            Try
                                Dim anaFrame As AnalyticalModelFrame = fi.AnalyticalModel
                                If Not anaFrame Is Nothing Then
                                    sMsg = "Analytical Model for Structural Framing " & _
                                            fi.StructuralType.ToString & " " & fi.Id.Value.ToString & vbCrLf
                                    ListCurve(anaFrame.Curve, sMsg)
                                    ListRigidLinks(anaFrame, sMsg)
                                    ListSupportInfo(anaFrame.SupportData, sMsg)
                                    MsgBox(sMsg)
                                End If
                            Catch
                            End Try

                        Case CatStruFndt.Name  ' "Structural Foundations" for EN locale
                            Try
                                Dim anaLoc As AnalyticalModelLocation = fi.AnalyticalModel
                                If Not anaLoc Is Nothing Then
                                    Dim pt As XYZ = anaLoc.Point
                                    sMsg = "Analytical Model for Foundation " & fi.Id.Value.ToString & vbCrLf & _
                                                       "  LOCATION = " & pt.X & ", " & pt.Y & ", " & pt.Z & vbCrLf
                                    ListSupportInfo(anaLoc.SupportData, sMsg)
                                    MsgBox(sMsg)
                                End If
                            Catch
                            End Try

                    End Select
                Catch
                End Try
            End If

        End While

        Return IExternalCommand.Result.Succeeded
    End Function

    Public Sub ListCurve(ByRef crv As Curve, ByRef s As String)

        If TypeOf crv Is Geometry.Line Then      'LINE

            Dim line As Line = crv
            Dim ptS As XYZ = line.EndPoint(0)
            Dim ptE As XYZ = line.EndPoint(1)

            s += "  LINE:" & ptS.X & ", " & ptS.Y & ", " & ptS.Z & " ; " & _
                             ptE.X & ", " & ptE.Y & ", " & ptE.Z & vbCrLf

        ElseIf TypeOf crv Is Geometry.Arc Then      'ARC

            Dim arc As Arc = crv
            Dim ptS As XYZ = arc.EndPoint(0)
            Dim ptE As XYZ = arc.EndPoint(1)
            Dim r As Double = arc.Radius

            s += "  ARC:" & ptS.X & ", " & ptS.Y & ", " & ptS.Z & " ; " & _
                            ptE.X & ", " & ptE.Y & ", " & ptE.Z & " ; R=" & r & vbCrLf

        Else        ' GENERIC PARAMETRIC CURVE
            If crv.IsBound Then
                s += "  BOUND CURVE " & crv.GetType.Name & " - Tessellated result:" & vbCrLf
                Dim pts As XYZArray = crv.Tessellate
                Dim pt As XYZ
                For Each pt In pts
                    s += "    PT:" & pt.X & ", " & pt.Y & ", " & pt.Z & " ; " & vbCrLf
                Next
            Else
                s += "  UNBOUND CURVE ??? - shouldn't ever be in an Analytical Model!" & vbCrLf
            End If

        End If

    End Sub

    Public Sub GetAnalyticalModelWall(ByRef anaWall As AnalyticalModelWall, ByRef s As String)
        Dim crv As Curve
        For Each crv In anaWall.Curves
            ListCurve(crv, s)
        Next
        ListSupportInfo(anaWall.SupportData, s)
    End Sub

    Public Sub GetAnalyticalModelFloor(ByRef anaFloor As AnalyticalModelFloor, ByRef s As String)
        Dim crv As Curve
        For Each crv In anaFloor.Curves
            ListCurve(crv, s)
        Next
        ListSupportInfo(anaFloor.SupportData, s)
    End Sub

    Public Sub GetAnalyticalModelContFooting(ByRef ana3d As AnalyticalModel3D, ByRef s As String)
        Dim crv As Curve
        For Each crv In ana3d.Curves
            ListCurve(crv, s)
        Next
        ListSupportInfo(ana3d.SupportData, s)
    End Sub

    ' New for RB3
    Public Sub ListSupportInfo(ByRef supData As AnalyticalSupportData, ByRef s As String)

        'If supData include valid support data.
        If (supData Is Nothing) Then
            s += "There is no support Data with this Element " & vbCrLf
        Else
            ' Supported or not?
            If supData.Supported Then
                s += vbCrLf & "Supported = YES, by:" & vbCrLf
            Else
                s += vbCrLf & "Supported = NO" & vbCrLf
                Return
            End If

            ' List all supports
            For Each supInfo As AnalyticalSupportInfo In supData.InfoArray
                Dim supEl As Revit.Element = supInfo.Element
                s += "  " & supInfo.SupportType.ToString() & " from elem.id=" & supEl.Id.Value.ToString & " cat=" & supEl.Category.Name & vbCrLf
            Next
        End If
    End Sub

    ' New for RB3
    Public Sub ListRigidLinks(ByRef anaFrm As AnalyticalModelFrame, ByRef s As String)

        s += vbCrLf & "Rigid Link START = "
        Dim rigidLinkStart As Curve = anaFrm.RigidLink(0)
        If rigidLinkStart Is Nothing Then
            s += "None" & vbCrLf
        Else
            ListCurve(rigidLinkStart, s)
        End If

        s += "Rigid Link END   = "
        Dim rigidLinkEnd As Curve = anaFrm.RigidLink(1)
        If rigidLinkEnd Is Nothing Then
            s += "None" & vbCrLf
        Else
            ListCurve(rigidLinkEnd, s)
        End If

    End Sub

End Class

