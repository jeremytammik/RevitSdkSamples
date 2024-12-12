Imports System
Imports System.IO
Imports System.Diagnostics
Imports System.Collections.Generic

Imports Autodesk
Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.DB.Events
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.UI.Events

<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.UI.Macros.AddInIdAttribute("A2616C96-88D8-4C9D-B4EF-E84147DEE20B")> _
Partial Public Class ThisApplication

    Private m_dicLastSaved As New Dictionary(Of Document, DateTime)()

    Private Sub Module_Startup(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Startup
        InitializeDicLastSaved()
        AddHandler Me.Application.DocumentSaved, AddressOf ThisApplication_DocumentSaved
        AddHandler Me.Application.DocumentOpened, AddressOf ThisApplication_DocumentOpened
        AddHandler Me.Application.DocumentCreated, AddressOf ThisApplication_DocumentCreated
        AddHandler Me.Application.DocumentClosing, AddressOf ThisApplication_DocumentClosing
    End Sub

    Private Sub Module_Shutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shutdown
        RemoveHandler Me.Application.DocumentSaved, AddressOf ThisApplication_DocumentSaved
        RemoveHandler Me.Application.DocumentOpened, AddressOf ThisApplication_DocumentOpened
        RemoveHandler Me.Application.DocumentCreated, AddressOf ThisApplication_DocumentCreated
        RemoveHandler Me.Application.DocumentClosing, AddressOf ThisApplication_DocumentClosing
    End Sub

    ''' <summary> 
    ''' Initialize 
    ''' </summary> 
    Private Sub InitializeDicLastSaved()
        For Each document As Document In Me.Application.Documents
            m_dicLastSaved.Add(document, DateTime.MaxValue)
        Next
    End Sub

    ''' <summary> 
    ''' DocumentCreated event, add the created document into m_dicLastSaved 
    ''' </summary> 
    ''' <param name="sender">sender</param> 
    ''' <param name="args">DocumentCreatedEventArgs</param> 
    Private Sub ThisApplication_DocumentCreated(ByVal sender As Object, ByVal args As Autodesk.Revit.DB.Events.DocumentCreatedEventArgs)
        m_dicLastSaved.Add(args.Document, DateTime.MaxValue)
    End Sub

    ''' <summary> 
    ''' DocumentClosing event, remove the closing document from m_dicLastSaved 
    ''' </summary> 
    ''' <param name="sender">sender</param> 
    ''' <param name="args">DocumentClosingEventArgs</param> 
    Private Sub ThisApplication_DocumentClosing(ByVal sender As Object, ByVal args As Autodesk.Revit.DB.Events.DocumentClosingEventArgs)
        m_dicLastSaved.Remove(args.Document)
    End Sub

    ''' <summary> 
    ''' DocumentOpened event, add the opened document into m_dicLastSaved 
    ''' </summary> 
    ''' <param name="sender">sender</param> 
    ''' <param name="args">DocumentOpenedEventArgs</param> 
    Private Sub ThisApplication_DocumentOpened(ByVal sender As Object, ByVal args As Autodesk.Revit.DB.Events.DocumentOpenedEventArgs)
        m_dicLastSaved.Add(args.Document, DateTime.MaxValue)
    End Sub

    ''' <summary> 
    ''' DocumentSaved event, record the current DataTime for the saved document 
    ''' </summary> 
    ''' <param name="sender">sender</param> 
    ''' <param name="args">DocumentSavedEventArgs</param> 
    Private Sub ThisApplication_DocumentSaved(ByVal sender As Object, ByVal args As Autodesk.Revit.DB.Events.DocumentSavedEventArgs)
        Me.m_dicLastSaved(args.Document) = DateTime.Now
    End Sub

    ''' <summary> 
    ''' FormatTimeSpan 
    ''' </summary> 
    ''' <param name="elapse">TimeSpan</param> 
    ''' <returns>the string of TimeSpan</returns> 
    Private Function FormatTimeSpan(ByVal elapse As TimeSpan) As [String]
        Dim elapseStr As [String] = elapse.ToString()
        Dim lastIndexOfDot As Integer = elapseStr.LastIndexOf("."c)
        Return elapseStr.Substring(0, lastIndexOfDot)
    End Function

    ''' <summary> 
    ''' GetTimeElapsedSinceLastSave 
    ''' </summary> 
    Public Sub GetTimeElapsedSinceLastSave()
        Dim text As [String] = [String].Format("{0,-30}{1,-30}", "Document Full Name", "Elapse Time Since Last Save(day.hh:mm:ss)") & vbLf
        For Each pair As KeyValuePair(Of Document, DateTime) In m_dicLastSaved
            Dim strElapsed As [String] = [String].Empty
            If pair.Value = DateTime.MaxValue Then
                strElapsed = "Never"
            Else
                strElapsed = FormatTimeSpan(DateTime.Now - pair.Value)
            End If

            Dim fileName As [String] = System.IO.Path.GetFileName(pair.Key.PathName)
            If [String].IsNullOrEmpty(fileName) Then
                fileName = "*New*"
            End If

            text += [String].Format("{0,-30}{1,-30}", fileName, strElapsed) & vbLf
        Next

        System.Windows.Forms.MessageBox.Show(text, "Macro GetTimeElapsedSinceLastSave")
    End Sub
End Class

