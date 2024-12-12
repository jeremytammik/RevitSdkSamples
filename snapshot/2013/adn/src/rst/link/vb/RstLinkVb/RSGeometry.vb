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