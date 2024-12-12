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


' Abstract class for all Elements
'   contains ID only
<Serializable()> _
Public MustInherit Class RsLinkElement
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


