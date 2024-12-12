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

Imports System.IO
Imports System.Runtime.Serialization.Formatters.Soap

Public Class RSLinkUtils

    Public Shared Sub RSSerialize(ByRef ht As Hashtable)
        Dim fs As New FileStream("c:\temp\_RSLinkExport.xml", FileMode.Create)
        Dim sf As New SoapFormatter
        sf.Serialize(fs, ht)
    End Sub

    Public Shared Sub RSDeSerialize(ByRef ht As Hashtable)
        Dim fs As New FileStream("c:\temp\_RSLinkExport.xml", FileMode.Open)
        Dim sf As New SoapFormatter
        ht = sf.Deserialize(fs)
    End Sub

End Class
