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
