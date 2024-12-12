#Region "Imports"
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Soap
Imports Autodesk.Autocad.Runtime
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports AcadApp = Autodesk.AutoCAD.ApplicationServices.Application
Imports RstLink
#End Region

Public Class AcadCommands

    <CommandMethod("RSLink", "RSImport", CommandFlags.Modal)> _
    Public Sub Import()

        ' Deserialize RSElements
        'Dim _RSelems As New Hashtable
        Dim _RSelems As Hashtable
        Try

            'Dim fs As New FileStream("c:\temp\_RSLinkExport.xml", FileMode.Open)
            'Dim sf As New SoapFormatter
            '_RSelems = sf.Deserialize(fs)
            RSLink.RSLinkUtils.RSDeSerialize(_RSelems)

            If _RSelems.Count <= 0 Then
                MsgBox("No elements found!")
                Return
            Else
                MsgBox(_RSelems.Count)
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message)
            Return
        End Try


    End Sub


End Class
