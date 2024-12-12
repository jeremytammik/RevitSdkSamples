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

' fix problem where assembly must reside in same directory as acad.exe or revit.exe:
' see http://www.codeproject.com/soap/Serialization_Samples.asp

Public NotInheritable Class RsLinkBinder
  Inherits System.Runtime.Serialization.SerializationBinder
  Public Overrides Function BindToType(ByVal assemblyName As String, ByVal typeName As String) As System.Type
    'Dim typeToDeserialize As Type = Nothing
    'Dim typeInfo As String()
    'typeInfo = typeName.Split(New [Char]() {"."c})
    ' The last item is the class name
    'Dim className As String
    'className = typeInfo(typeInfo.Length - 1)
        'If className.Equals("RsLink") Then
        'typeToDeserialize = typeof RsLink
    'Else
    'typeToDeserialize = 
    'End If
    'Return typeToDeserialize
    Return Type.GetType(String.Format("{0}, {1}", typeName, assemblyName))
  End Function
End Class
