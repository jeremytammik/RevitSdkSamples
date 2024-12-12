' Class for Structural Member (Column or Framing categories)
' Only some example properties mapped and only straight members assumed!!!

<Serializable()> _
Public Class RSMember
    Inherits RSLinkElement

    'KIS (no properties)
    <CLSCompliant(False)> _
    Public _usage As String
    <CLSCompliant(False)> _
    Public _type As String
    <CLSCompliant(False)> _
    Public _geom As RSLine

    Sub New(ByVal id As Integer, ByVal usage As String, ByVal type As String, ByVal geom As RSLine)
        MyBase.New(id)
        _usage = usage
        _type = type
        _geom = geom
    End Sub

End Class
