#Region "Header"
' RstLink
'
' Copyright (C) 2006-2008 by Autodesk, Inc.
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
Imports System.xml.Serialization
Imports System.Runtime.Serialization.Formatters.Soap
Imports Autodesk
Imports Autodesk.Revit
Imports Autodesk.Revit.Symbols
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Structural
Imports Autodesk.Revit.Parameters
Imports Autodesk.Revit.Geometry
Imports RstLink
#End Region

' Import incremental changes from the RstLink intermediate XML file
Public Class RSLinkImport
  Implements IExternalCommand

  Public Function Execute(ByVal commandData As ExternalCommandData, ByRef msg As String, ByVal els As ElementSet) As IExternalCommand.Result Implements IExternalCommand.Execute

    Dim app As Revit.Application = commandData.Application

    Dim _RSelems As Hashtable

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
        sf.Binder = New RSLinkBinder()
        _RSelems = sf.Deserialize(fs)
        fs.Close()
      Else
        MsgBox("Command cancelled!")
        Exit Function
      End If

    Catch ex As Exception
      MsgBox("Error when deserializing: " & ex.Message)
      Return IExternalCommand.Result.Failed
    End Try

    If _RSelems.Count = 0 Then
      MsgBox("No elements found in the result file!")
      Return IExternalCommand.Result.Cancelled
    End If

    ' KIS: one big TRY block - would be better to be more specific..
    Try

      Dim log As New StreamWriter(dlg.FileName + ".log")
      Dim iModified As Integer

      ' Itearate all results
      Dim iter As IDictionaryEnumerator = _RSelems.GetEnumerator
      While iter.MoveNext
        Dim m As RSMember = iter.Value

        'ADD NEW elements 
        '================
        If m.revitId = 0 Then
          ' In 8.1 API there are no ways to create new Families with Location as Curve, but Point only.
          ' This has been addressed in 9.0 API - see eg CreateBeamsColumnsBraces sample in the SDK

          ' Bonus: add new elements....

          'MODIFY NEW Sizes (Types)
          '=======================
        Else
          Dim id As Revit.ElementId
          id.Value = m.revitId
          Dim fi As Elements.FamilyInstance = app.ActiveDocument.Element(id)

          ' Check if the Type has changed (in theory we'd need to check Family as well)
          Dim newType As String = m._type
          If Not fi.Symbol.Name.Equals(newType) Then

            log.WriteLine("Member Id=" & m.revitId & ": Type changed from " & _
                          fi.Symbol.Name & " to " & newType)

            Dim catName As String
            If m._usage.Equals("Column") Then
              catName = "Structural Columns"
            Else
              catName = "Structural Framing"
            End If

            Dim newSymb As Symbols.FamilySymbol = GetFamilySymbol(app.ActiveDocument, catName, newType)
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

      End While
      MsgBox("Successfully MODIFIED Types for " & iModified & " structural members!")
      log.Close()

    Catch ex As Exception
      MsgBox("Error when processing: " & ex.Message)
      Return IExternalCommand.Result.Failed
    Finally
    End Try

    Return IExternalCommand.Result.Succeeded
  End Function

    ' Helper to get specified Type for a Category
    ' (in theory, we can have non-unique solution, i.e. the same Type name for more than one family from this category!)
  Shared Function GetFamilySymbol(ByVal doc As Document, ByVal catName As String, ByVal typeName As String) As Symbols.FamilySymbol

        ' todo: use 2009 element filtering

    Dim iter As ElementIterator = doc.Elements
    Do While (iter.MoveNext())
      Dim elem As Revit.Element = iter.Current

      ' We got a Family
      If TypeOf elem Is Elements.Family Then
        Dim fam As Elements.Family = elem
        ' If we have a match on Category name, loop all its types for the other match
        Try
          ' we CANNOT use this, since Category is not implemented for Family objects :-( !
          'If fam.Category.Name.Equals(catName) Then
          Dim sym As Symbols.FamilySymbol
          For Each sym In fam.Symbols
            If sym.Name.Equals(typeName) Then
              If sym.Category.Name.Equals(catName) Then ' must use it here - slightly more inefficient
                Return sym
              End If
            End If
          Next
          'End If
        Catch
        End Try
      End If

    Loop

    ' if here - haven't got it!
    Return Nothing

  End Function

End Class


' Export all structural elements to the RstLink intermediate file
' (only Columns and Framing implemented - skeleton code in place for others)
Public Class RSLinkExport
  Implements IExternalCommand

  Public Function Execute(ByVal commandData As ExternalCommandData, ByRef msg As String, ByVal els As ElementSet) As IExternalCommand.Result Implements IExternalCommand.Execute

    Dim app As Revit.Application = commandData.Application
    ' From RS3, we can make sure it works in all locales, so use localized category names:
    Dim catStruColums As Revit.Category = app.ActiveDocument.Settings.Categories.Item(BuiltInCategory.OST_StructuralColumns)
    Dim catStruFraming As Revit.Category = app.ActiveDocument.Settings.Categories.Item(BuiltInCategory.OST_StructuralFraming)
    Dim catStruFoundation As Revit.Category = app.ActiveDocument.Settings.Categories.Item(BuiltInCategory.OST_StructuralFoundation)

    ' No Dictionary was available in VB.NET 2003, so used untyped collection. If doing again in 2005 - better use Dictionary
    Dim _RSElems As New Hashtable
    'alternatively
    'Dim _RSarray As New ArrayList

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

              'ToDo WALL

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

              'ToDo FLOOR

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

              'ToDo CONT.FOOTING

            End If
          End If
        Catch
        End Try

        ' One of Strucural Standard Families
      ElseIf TypeOf elem Is Elements.FamilyInstance Then
        Try
          Dim fi As Elements.FamilyInstance = elem
          Select Case fi.Category.Name

            ' From RS3, better use local-independent design
            ' Case "Structural Columns", "Structural Framing"
            Case catStruColums.Name, catStruFraming.Name
              Try
                Dim anaFrame As AnalyticalModelFrame = fi.AnalyticalModel
                If Not anaFrame Is Nothing Then

                  ' Create MEMBER in neutral format and add it to the collection
                  Dim member As RSMember = CreateRSMember(fi, anaFrame)
                  If Not member Is Nothing Then
                    _RSElems.Add(member, member)
                  End If

                End If
              Catch
              End Try

              'Case "Structural Foundations"
            Case catStruFoundation.Name
              Try
                Dim anaLoc As AnalyticalModelLocation = fi.AnalyticalModel
                If Not anaLoc Is Nothing Then

                  'ToDo FOUNDATION...also change hard-coded category name

                End If
              Catch
              End Try

          End Select
        Catch
        End Try

        'ToDo: all LOADS!
      ElseIf TypeOf elem Is Elements.PointLoad Then
        '...
      ElseIf TypeOf elem Is Elements.LineLoad Then
        '...
      ElseIf TypeOf elem Is Elements.AreaLoad Then
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
        sf.Binder = New RSLinkBinder()
        sf.Serialize(fs, _RSElems)
        fs.Close()
        MsgBox("Successfully exported " & _RSElems.Count & " structural elements!")

      Else
        MsgBox("Command cancelled!")
        Return IExternalCommand.Result.Cancelled
      End If

      '' NOTE: - (de)serialization works fine but all assemblies MUST be in the same folder as revit.EXE!
      ''       The same is true later when deserializing in AutoCAD - must put them in same folder with acad.EXE.

      '           SEE: Serialize the Collection to a file
      '           http://www.codeproject.com/soap/Serialization_Samples.asp
      '           Serialization(Headaches)
      '           http://www.dotnet4all.com/dotnet-code/2004/12/serialization-headaches.html
      'Try
      '    Dim fsTest As New FileStream("c:\temp\_RSLinkExport.xml", FileMode.Open)
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

    Return IExternalCommand.Result.Succeeded
  End Function

  Public Function CreateRSMember(ByVal fi As Elements.FamilyInstance, ByVal anaFrame As AnalyticalModelFrame) As RSMember
    Try
      Dim id As Integer = fi.Id.Value
      Dim line As Line = anaFrame.Curve
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
          Select Case fi.Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_PARAM).AsInteger
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

      Dim m As New RSMember(id, usage, type, New RSLine( _
                            New RSPoint(line.EndPoint(0).X, line.EndPoint(0).Y, line.EndPoint(0).Z), _
                            New RSPoint(line.EndPoint(1).X, line.EndPoint(1).Y, line.EndPoint(1).Z)))

      Return m
    Catch ex As Exception
      Return Nothing
    End Try
  End Function

End Class
