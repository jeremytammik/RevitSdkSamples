
<CLSCompliant(False)> _
Partial Class ThisApplication

    Dim m_dicLastSaved As System.Collections.Generic.Dictionary(Of Autodesk.Revit.Document, DateTime) = New System.Collections.Generic.Dictionary(Of Autodesk.Revit.Document, DateTime)

    Private Sub ThisApplication_Startup(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Startup
        InitializeDicLastSaved()
        AddHandler Me.OnDocumentNewed, AddressOf ThisApplication_OnDocumentNewed
        AddHandler Me.OnDocumentClosed, AddressOf ThisApplication_OnDocumentClosed
        AddHandler Me.OnDocumentOpened, AddressOf ThisApplication_OnDocumentOpened
        AddHandler Me.OnDocumentSaved, AddressOf ThisApplication_OnDocumentSaved
    End Sub

    Private Sub ThisApplication_Shutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shutdown
        RemoveHandler Me.OnDocumentNewed, AddressOf ThisApplication_OnDocumentNewed
        RemoveHandler Me.OnDocumentClosed, AddressOf ThisApplication_OnDocumentClosed
        RemoveHandler Me.OnDocumentOpened, AddressOf ThisApplication_OnDocumentOpened
        RemoveHandler Me.OnDocumentSaved, AddressOf ThisApplication_OnDocumentSaved
    End Sub

    Sub ThisApplication_OnDocumentNewed(ByVal document As Autodesk.Revit.Document)
        m_dicLastSaved.Add(document, DateTime.MaxValue)
    End Sub 'ThisApplication_OnDocumentNewed


    Sub ThisApplication_OnDocumentClosed(ByVal document As Autodesk.Revit.Document)
        m_dicLastSaved.Remove(document)
    End Sub 'ThisApplication_OnDocumentClosed


    Sub ThisApplication_OnDocumentOpened(ByVal document As Autodesk.Revit.Document)
        m_dicLastSaved.Add(document, DateTime.MaxValue)
    End Sub 'ThisApplication_OnDocumentOpened


    Sub ThisApplication_OnDocumentSaved(ByVal document As Autodesk.Revit.Document)
        Me.m_dicLastSaved(document) = DateTime.Now
    End Sub 'ThisApplication_OnDocumentSaved

    Private Sub InitializeDicLastSaved()
        Dim document As Autodesk.Revit.Document
        For Each document In Me.Documents
            m_dicLastSaved.Add(document, DateTime.MaxValue)
        Next document
    End Sub 'InitializeDicLastSaved

    Private Function FormatTimeSpan(ByVal elapse As TimeSpan) As String
        Dim elapseStr As String = elapse.ToString()
        Dim lastIndexOfDot As Integer = elapseStr.LastIndexOf(".")
        Return elapseStr.Substring(0, lastIndexOfDot)
    End Function 'FormatTimeSpan

    Public Sub GetTimeElapsedSinceLastSave()
        Dim text As String = String.Format("{0,-30}{1,-30}", "Document Full Name", "Elapse Time Since Last Save(day.hh:mm:ss)") + ControlChars.Lf
        Dim pair As System.Collections.Generic.KeyValuePair(Of Autodesk.Revit.Document, DateTime)
        For Each pair In m_dicLastSaved
            Dim strElapsed As String = String.Empty
            If pair.Value = DateTime.MaxValue Then
                strElapsed = "Never"
            Else
                strElapsed = FormatTimeSpan((DateTime.Now - pair.Value))
            End If

            Dim fileName As [String] = System.IO.Path.GetFileName(pair.Key.PathName)
            If [String].IsNullOrEmpty(fileName) Then
                fileName = "*New*"
            End If

            text += String.Format("{0,-30}{1,-30}", fileName, strElapsed) + ControlChars.Lf
        Next pair

        System.Windows.Forms.MessageBox.Show([text], "Macro GetTimeElapsedSinceLastSave")

    End Sub 'GetTimeElapsedSinceLastSave

End Class


