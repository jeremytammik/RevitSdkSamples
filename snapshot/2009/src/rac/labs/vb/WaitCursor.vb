Imports System.Windows.Forms

Namespace Labs
    Class WaitCursor
        Dim _oldCursor As Cursor

        Public Sub New()
            _oldCursor = Cursor.Current
            Cursor.Current = Cursors.WaitCursor
        End Sub

        Protected Overrides Sub Finalize()
            Cursor.Current = _oldCursor
        End Sub
    End Class
End Namespace

