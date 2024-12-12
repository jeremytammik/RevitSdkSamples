'Geometrical helpers

<Serializable()> _
Public Class RSPoint

    Private _x, _y, _z As Double

    Sub New(ByVal x As Double, ByVal y As Double, ByVal z As Double)
        _x = x
        _y = y
        _z = z
    End Sub

    Public Property X() As Double
        Get
            Return _x
        End Get
        Set(ByVal Value As Double)
            _x = Value
        End Set
    End Property

    Public Property Y() As Double
        Get
            Return _y
        End Get
        Set(ByVal Value As Double)
            _y = Value
        End Set
    End Property

    Public Property Z() As Double
        Get
            Return _z
        End Get
        Set(ByVal Value As Double)
            _z = Value
        End Set
    End Property

End Class


<Serializable()> _
Public Class RSLine

    Sub New(ByVal startPt As RSPoint, ByVal endPt As RSPoint)
        _StartPt = startPt
        _EndPt = endPt
    End Sub

    'KIS (no props)
    <CLSCompliant(False)> _
    Public _StartPt, _EndPt As RSPoint

End Class