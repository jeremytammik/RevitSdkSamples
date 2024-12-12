
' Abstract class for all Elements
'   contains ID only
<Serializable()> _
Public MustInherit Class RSLinkElement
    Sub New()
        _revitId = 0
    End Sub

    Sub New(ByVal id As Integer)
        _revitId = id
    End Sub

    Private _revitId As Integer
    Public ReadOnly Property revitId() As Integer
        Get
            Return _revitId
        End Get
    End Property


End Class


