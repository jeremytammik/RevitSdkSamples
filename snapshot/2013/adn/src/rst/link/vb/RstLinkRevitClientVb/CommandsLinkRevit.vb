#Region "Header"
' RstLink
'
' Copyright (C) 2006-2013 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software
' for any purpose and without fee is hereby granted, provided
' that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
#End Region

#Region "Imports"
Imports System.IO
Imports System.Data
Imports System.Xml.Serialization
Imports System.Collections
Imports System.Collections.Generic
Imports System.Runtime.Serialization.Formatters.Soap
Imports Autodesk.Revit.Attributes
Imports db = Autodesk.Revit.DB
Imports dbst = Autodesk.Revit.DB.Structure
Imports Autodesk.Revit.UI
Imports RstLink
#End Region

Class IntBic
  Public Const Columns As Integer = CType(db.BuiltInCategory.OST_StructuralColumns, Integer)
  Public Const Framing As Integer = CType(db.BuiltInCategory.OST_StructuralFraming, Integer)
  Public Const Foundation As Integer = CType(db.BuiltInCategory.OST_StructuralFoundation, Integer)
End Class

' Import incremental changes from the RstLink intermediate XML file
<Transaction(TransactionMode.Automatic)> _
Public Class RsLinkImport
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef msg As String, ByVal els As db.ElementSet) As Result Implements IExternalCommand.Execute

        Dim app As UIApplication = commandData.Application
        Dim doc As db.Document = app.ActiveUIDocument.Document


        'Dim _RSelems As Hashtable
        Dim _RSelems As HashSet(Of RSMember)


        ' De-serialize the result file 
        Dim dlg As New OpenFileDialog
        Try
            ' Select File to Open
            dlg.Filter = "RstLink xml files (*.xml)|*.xml"
            dlg.Title = "RstLink - Revit IMPORT from AutoCAD"
            If dlg.ShowDialog() = DialogResult.OK Then
                Dim fs As New FileStream(dlg.FileName, FileMode.Open)
                Dim sf As New SoapFormatter
                sf.AssemblyFormat = Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
                sf.Binder = New RsLinkBinder()
                _RSelems = CType(sf.Deserialize(fs), HashSet(Of RSMember))
                '                  rstElems = sf.Deserialize( fs ) as HashSet<RSMember>;
                fs.Close()
            Else
                MsgBox("Command cancelled!")
                Exit Function
            End If

        Catch ex As Exception
            MsgBox("Error when deserializing: " & ex.Message)
            Return Result.Failed
        End Try

        If _RSelems.Count = 0 Then
            MsgBox("No elements found in the result file!")
            Return Result.Cancelled
        End If

        ' KIS: one big TRY block - would be better to be more specific..
        Dim documentTransaction As db.Transaction
        documentTransaction = New db.Transaction(app.ActiveUIDocument.Document, "document")

        documentTransaction.Start()
        Try

            Dim log As New StreamWriter(dlg.FileName + ".log")
            Dim iModified As Integer

            ' Itearate all results
            Dim iter As HashSet(Of RSMember).Enumerator = _RSelems.GetEnumerator



            For Each m As RSMember In _RSelems

                'ADD NEW elements 
                '================
                If m.revitId = 0 Then
                    ' In 8.1 API there are no ways to create new Families with Location as Curve, but Point only.
                    ' This has been addressed in 9.0 API - see eg CreateBeamsColumnsBraces sample in the SDK

                    ' Bonus: add new elements....

                    'MODIFY NEW Sizes (Types)
                    '=======================
                Else
                    Dim id As db.ElementId = New db.ElementId(m.revitId)
                    Dim fi As db.FamilyInstance = doc.Element(id)

                    ' Check if the Type has changed (in theory we'd need to check Family as well)
                    Dim newType As String = m._type
                    If Not fi.Symbol.Name.Equals(newType) Then

                        log.WriteLine("Member Id=" & m.revitId & ": Type changed from " & _
                                      fi.Symbol.Name & " to " & newType)
                        Dim bic As db.BuiltInCategory
                        If (m._usage.Equals("Column")) Then
                            bic = db.BuiltInCategory.OST_StructuralColumns
                        Else
                            bic = db.BuiltInCategory.OST_StructuralFraming
                        End If

                        Dim newSymb As db.FamilySymbol = GetFamilySymbol(doc, bic, newType)
                        If newSymb Is Nothing Then
                            log.WriteLine("  ERROR: Could not find the new Symbol loaded in RVT!")
                        Else
                            Try
                                fi.Symbol = newSymb
                                log.WriteLine("  Symbol SUSSESSFULLY changed!")
                                iModified += 1
                            Catch ex As Exception
                                log.WriteLine("  ERROR: Could not change to the new Symbol ?!")
                            End Try
                        End If

                    End If
                End If

            Next
            MsgBox("Successfully MODIFIED Types for " & iModified & " structural members!")
            log.Close()

        Catch ex As Exception
            documentTransaction.RollBack()
            MsgBox("Error when processing: " & ex.Message)
            Return Result.Failed
        Finally
        End Try
        documentTransaction.Commit()
        Return Result.Succeeded
    End Function

    ' Helper to get specified Type for a Category
    ' (in theory, we can have non-unique solution, i.e. the same Type name for more than one family from this category!)
    Function GetFamilySymbol(ByVal doc As db.Document, ByVal bic As db.BuiltInCategory, ByVal typeName As String) As db.FamilySymbol
        Dim collector As db.FilteredElementCollector = New db.FilteredElementCollector(doc)
        collector.OfClass(GetType(db.FamilySymbol)).OfCategory(bic)

        For Each elem As db.FamilySymbol In collector
            If (elem.Name.Equals(typeName)) Then
                Return elem
            End If
        Next
        ' if here - haven't got it!
        Return Nothing
    End Function

End Class


' Export all structural elements to the RstLink intermediate file
' (only Columns and Framing implemented - skeleton code in place for others)
<Transaction(TransactionMode.Automatic)> _
Public Class RsLinkExport
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef msg As String, ByVal els As db.ElementSet) As Result Implements IExternalCommand.Execute

        Dim app As UIApplication = commandData.Application
        Dim doc As db.Document = app.ActiveUIDocument.Document
        'Dim categories As db.Categories = doc.Settings.Categories

        '' make sure it works in all locales, so use localized category names:
        'Dim catStruColums As db.Category = categories.Item(db.BuiltInCategory.OST_StructuralColumns)
        'Dim catStruFraming As db.Category = categories.Item(db.BuiltInCategory.OST_StructuralFraming)
        'Dim catStruFoundation As db.Category = categories.Item(db.BuiltInCategory.OST_StructuralFoundation)

        ' No Dictionary was available in VB.NET 2003, so used untyped collection. If doing again in 2005 - better use Dictionary
        Dim _RSElems As New HashSet(Of RSMember)
        'alternatively
        'Dim _RSarray As New ArrayList

        ' LOOP all elements and add to the collection

        Dim filters As IList(Of db.ElementFilter) = New List(Of db.ElementFilter)()

        Dim classFilter1 As db.ElementClassFilter = New db.ElementClassFilter(GetType(db.Wall))
        Dim classFilter2 As db.ElementClassFilter = New db.ElementClassFilter(GetType(db.Floor))
        Dim classFilter3 As db.ElementClassFilter = New db.ElementClassFilter(GetType(db.ContFooting))
        Dim classFilter4 As db.ElementClassFilter = New db.ElementClassFilter(GetType(db.FamilyInstance))
        Dim classFilter5 As db.ElementClassFilter = New db.ElementClassFilter(GetType(dbst.PointLoad))
        Dim classFilter6 As db.ElementClassFilter = New db.ElementClassFilter(GetType(dbst.LineLoad))
        Dim classFilter7 As db.ElementClassFilter = New db.ElementClassFilter(GetType(dbst.AreaLoad))
        filters.Add(classFilter1)
        filters.Add(classFilter2)
        filters.Add(classFilter3)
        filters.Add(classFilter4)
        filters.Add(classFilter5)
        filters.Add(classFilter6)
        filters.Add(classFilter7)

        Dim logOrFilter As db.LogicalOrFilter = New db.LogicalOrFilter(filters)
        Dim collector As db.FilteredElementCollector = New db.FilteredElementCollector(app.ActiveUIDocument.Document)
        Dim iter As db.FilteredElementIterator = collector.WherePasses(logOrFilter).GetElementIterator()

        While (iter.MoveNext())
            Dim elem As db.Element = iter.Current

            ' Strucural WALL
            If TypeOf elem Is db.Wall Then
                Dim w As db.Wall = elem
                Try
                    Dim anaWall As dbst.AnalyticalModel = w.GetAnalyticalModel
                    If Not anaWall Is Nothing Then
                        If anaWall.GetCurves(dbst.AnalyticalCurveType.RawCurves).Count > 0 Then

                            'ToDo WALL

                        End If
                    End If
                Catch
                End Try

                ' Strucural FLOOR
            ElseIf TypeOf elem Is db.Floor Then
                Dim f As db.Floor = elem
                Try
                    Dim anaFloor As dbst.AnalyticalModel = f.GetAnalyticalModel
                    If Not anaFloor Is Nothing Then
                        If anaFloor.GetCurves(dbst.AnalyticalCurveType.RawCurves).Count > 0 Then

                            'ToDo FLOOR

                        End If
                    End If
                Catch
                End Try

                ' Strucural CONTINUOUS FOOTING
            ElseIf TypeOf elem Is db.ContFooting Then
                Dim cf As db.ContFooting = elem
                Try
                    Dim ana3D As dbst.AnalyticalModel = cf.GetAnalyticalModel
                    If Not ana3D Is Nothing Then
                        If ana3D.GetCurves(dbst.AnalyticalCurveType.RawCurves).Count > 0 Then

                            'ToDo CONT.FOOTING

                        End If
                    End If
                Catch
                End Try

                ' One of Strucural Standard Families
            ElseIf TypeOf elem Is db.FamilyInstance Then
                Try
                    Dim fi As db.FamilyInstance = elem
                    'Select Case fi.Category.Id.IntegerValue
                    Dim iBic As Integer = fi.Category.Id.IntegerValue

                    If ((iBic = IntBic.Columns) Or (iBic = IntBic.Framing)) Then

                        ' From RS3, better use local-independent design
                        ' Case "Structural Columns", "Structural Framing"            
                        Try
                            Dim anaFrame As dbst.AnalyticalModel = fi.GetAnalyticalModel
                            If Not anaFrame Is Nothing Then

                                ' Create MEMBER in neutral format and add it to the collection
                                Dim member As RSMember = CreateRSMember(fi, anaFrame)
                                If Not member Is Nothing Then
                                    _RSElems.Add(member)
                                End If

                            End If
                        Catch
                        End Try

                        'Case "Structural Foundations"
                    ElseIf (iBic = IntBic.Foundation) Then
                        Try
                            Dim anaLoc As dbst.AnalyticalModel = fi.GetAnalyticalModel
                            If Not anaLoc Is Nothing Then

                                'ToDo FOUNDATION...also change hard-coded category name

                            End If
                        Catch
                        End Try
                    End If
                Catch
                End Try

                'ToDo: all LOADS!
            ElseIf TypeOf elem Is dbst.PointLoad Then
                '...
            ElseIf TypeOf elem Is dbst.LineLoad Then
                '...
            ElseIf TypeOf elem Is dbst.AreaLoad Then
                '...
            End If
        End While

        ' Serialize the members to a file
        If _RSElems.Count > 0 Then

            ' Select File to Save
            Dim dlg As New SaveFileDialog
            dlg.Filter = "RstLink xml files (*.xml)|*.xml"
            dlg.Title = "RstLink - Revit EXPORT to AutoCAD"
            dlg.FileName = "RstLinkRevitToAcad.xml"
            If dlg.ShowDialog() = DialogResult.OK Then

                'SOAP (would be faster if BINARY, but just to make it readable)
                Dim fs As New FileStream(dlg.FileName, FileMode.Create)
                Dim sf As New SoapFormatter
                sf.AssemblyFormat = Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
                sf.Binder = New RsLinkBinder()
                sf.Serialize(fs, _RSElems)
                fs.Close()
                MsgBox("Successfully exported " & _RSElems.Count & " structural elements!")

            Else
                MsgBox("Command cancelled!")
                Return Result.Cancelled
            End If

            '' NOTE: - (de)serialization works fine but all assemblies MUST be in the same folder as revit.EXE!
            ''       The same is true later when deserializing in AutoCAD - must put them in same folder with acad.EXE.

            '           SEE: Serialize the Collection to a file
            '           http://www.codeproject.com/soap/Serialization_Samples.asp
            '           Serialization(Headaches)
            '           http://www.dotnet4all.com/dotnet-code/2004/12/serialization-headaches.html
            'Try
            '    Dim fsTest As New FileStream("c:\temp\_RsLinkExport.xml", FileMode.Open)
            '    Dim sfTest As New SoapFormatter
            '    Dim elemsTest As Hashtable = sfTest.Deserialize(fsTest)
            '    fsTest.Close()
            '    MsgBox("Num.of DeSer = " & elemsTest.Count)
            'Catch ex As Exception
            '    MsgBox("Error in DeSer: " & ex.Message)
            'End Try

        Else
            MsgBox("No Structural Elements found in this model!")
        End If

        Return Result.Succeeded
    End Function

    Public Function CreateRSMember(ByVal fi As db.FamilyInstance, ByVal anaFrame As dbst.AnalyticalModel) As RSMember
        Try
            Dim id As Integer = fi.Id.IntegerValue
            Dim line As db.Line = anaFrame.GetCurve()
            Dim type As String = fi.Symbol.Name
            Dim usage As String
            If fi.Category.Name = "Structural Columns" Then
                usage = "Column"
            Else
                ' This doesn't work any longer in structure 3! (was OK in 2?)
                'usage = fi.Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_TEXT_PARAM).AsString
                ' Now must get the integer enumeration and map the name
                '124334  6	Other
                '124340  3	Girder
                '124349  5	Purlin
                '134865  6	Other
                '129463  7	Vertical Bracing
                '124337  3	Girder
                '124331  6	Other
                '124346  8	Horizontal Bracing
                '124343  4	Joist
                '129409  9	Kicker Bracing
                Try
                    Select Case fi.Parameter(db.BuiltInParameter.INSTANCE_STRUCT_USAGE_PARAM).AsInteger
                        Case 3
                            usage = "Girder"
                        Case 4
                            usage = "Joist"
                        Case 5
                            usage = "Purlin"
                        Case 6
                            usage = "Other"
                        Case 7
                            usage = "Vertical Bracing"
                        Case 8
                            usage = "Horizontal Bracing"
                        Case 9
                            usage = "Kicker Bracing"
                        Case Else
                            usage = "Unknown"
                    End Select
                Catch ex As Exception
                    usage = "Parameter Fails"
                End Try

                'Dim p1 As Parameter = fi.Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_TEXT_PARAM)
                'If Not p1 Is Nothing Then
                '    MsgBox(p1.StorageType.ToString)
                'End If

                'Dim p2 As Parameter = fi.Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_PARAM)
                'If Not p2 Is Nothing Then
                '    MsgBox(p2.StorageType.ToString)
                'End If
            End If

            Dim p1 As db.XYZ = line.EndPoint(0)
            Dim q1 As db.XYZ = line.EndPoint(1)
            Dim p As RSPoint = New RSPoint(p1.X, p1.Y, p1.Z)
            Dim q As RSPoint = New RSPoint(q1.X, q1.Y, q1.Z)
            Dim l As RSLine = New RSLine(p, q)
            Dim m As New RSMember(id, usage, type, l)

            Return m
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
