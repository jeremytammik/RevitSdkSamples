Imports RSLink
Imports System.IO
'Imports System.xml.Serialization
'Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization.Formatters.Soap

Imports Autodesk
Imports Autodesk.Revit
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Structural
Imports Autodesk.Revit.Parameters
Imports Autodesk.Revit.Geometry



' Export all structural elements to the RSLink intermediate file
Public Class RSLinkExport
    Implements IExternalCommand
    Public Function Execute(ByVal app As Autodesk.Revit.Application, ByRef msg As String, ByVal els As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute

        ' No Dictionary as in VB.NET 2005, so use untyped collection
        Dim _RSElems As New Hashtable

        ' LOOP all elements and add to the collection
        Dim iter As ElementIterator = app.ActiveDocument.Elements
        While (iter.MoveNext())
            Dim elem As Revit.Element = iter.Current

            ' Strucural WALL
            If TypeOf elem Is Wall Then
                Dim w As Wall = elem
                Try
                    Dim anaWall As AnalyticalModelWall = w.AnalyticalModel
                    If Not anaWall Is Nothing Then
                        If anaWall.Curves.Size > 0 Then
                            'Dim sMsg As String = "Analytical Model for Wall " & w.Id.Value.ToString & vbCrLf
                            'GetAnalyticalModelWall(anaWall, sMsg)
                            'MsgBox(sMsg)
                        End If
                    End If
                Catch
                End Try

                ' Strucural FLOOR
            ElseIf TypeOf elem Is Elements.Floor Then
                Dim f As Elements.Floor = elem
                Try
                    Dim anaFloor As AnalyticalModelFloor = f.AnalyticalModel
                    If Not anaFloor Is Nothing Then
                        If anaFloor.Curves.Size > 0 Then
                            'Dim sMsg As String = "Analytical Model for Floor " & f.Id.Value.ToString & vbCrLf
                            'GetAnalyticalModelFloor(anaFloor, sMsg)
                            'MsgBox(sMsg)
                        End If
                    End If
                Catch
                End Try

                ' Strucural CONTINUOUS FOOTING
            ElseIf TypeOf elem Is Elements.ContFooting Then
                Dim cf As Elements.ContFooting = elem
                Try
                    Dim ana3D As AnalyticalModel3D = cf.AnalyticalModel
                    If Not ana3D Is Nothing Then
                        If ana3D.Curves.Size > 0 Then
                            'Dim sMsg As String = "Analytical Model for Continuous Footing " & cf.Id.Value.ToString & vbCrLf
                            'GetAnalyticalModelContFooting(ana3D, sMsg)
                            'MsgBox(sMsg)
                        End If
                    End If
                Catch
                End Try

                ' One of Strucural Standard Families
            ElseIf TypeOf elem Is Elements.FamilyInstance Then
                Try
                    Dim fi As Elements.FamilyInstance = elem
                    Select Case fi.Category.Name

                        Case "Structural Columns", "Structural Framing"
                            Try
                                Dim anaFrame As AnalyticalModelFrame = fi.AnalyticalModel
                                If Not anaFrame Is Nothing Then
                                    Dim member As RSMember = CreateRSMember(fi, anaFrame)
                                    If Not member Is Nothing Then
                                        _RSElems.Add(member, member)
                                    End If
                                End If
                            Catch
                            End Try

                        Case "Structural Foundations"
                            Try
                                Dim anaLoc As AnalyticalModelLocation = fi.AnalyticalModel
                                If Not anaLoc Is Nothing Then
                                    'Dim pt As XYZ = anaLoc.Point
                                    'MsgBox("Analytical Model for Foundation " & fi.Id.Value.ToString & _
                                    '       "  LOCATION = " & pt.X & ", " & pt.Y & ", " & pt.Z & vbCrLf)
                                End If
                            Catch
                            End Try

                    End Select
                Catch
                End Try
            End If

        End While

        ' Serialize the Collection to a file
        ' http://www.codeproject.com/soap/Serialization_Samples.asp
        'Serialization(Headaches)
        'http://www.dotnet4all.com/dotnet-code/2004/12/serialization-headaches.html



        If _RSElems.Count > 0 Then

            'Dim fs As New FileStream("c:\temp\_RSLinkExport.xml", FileMode.Create)
            'Dim sf As New SoapFormatter
            'sf.Serialize(fs, _RSElems)
            Try
                RSLink.RSLinkUtils.RSSerialize(_RSElems)
            Catch ex As Exception
                MsgBox("error when serializing: " & ex.Message)
            End Try


        Else
            MsgBox("No Structural Elements found in this model!")
        End If

        Return IExternalCommand.Result.Succeeded
    End Function

    Public Function CreateRSMember(ByVal fi As Elements.FamilyInstance, ByVal anaFrame As AnalyticalModelFrame)
        Try
            Dim id As Integer = fi.Id.Value

            Dim line As Line = anaFrame.Curve

            Dim type As String = fi.Symbol.Name

            Dim usage As String
            If fi.Category.Name = "Structural Columns" Then
                usage = "Column"
            Else
                usage = fi.Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_TEXT_PARAM).AsString
            End If

            Dim m As New RSMember(id, usage, type, New RSLine( _
                                  New RSPoint(line.StartPoint.X, line.StartPoint.Y, line.StartPoint.Z), _
                                  New RSPoint(line.EndPoint.X, line.EndPoint.Y, line.EndPoint.Z)))


            Return m
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
