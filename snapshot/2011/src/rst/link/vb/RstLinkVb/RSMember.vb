#Region "Header"
' RstLink
'
' Copyright (C) 2006-2010 by Autodesk, Inc.
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

' Class for Structural Member (Column or Framing categories)
' Only some example properties mapped and only straight members assumed!!!

<Serializable()> _
Public Class RSMember
    Inherits RsLinkElement

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
