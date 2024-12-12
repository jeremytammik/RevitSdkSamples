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
    'If className.Equals("RSLink") Then
    'typeToDeserialize = typeof RSLink
    'Else
    'typeToDeserialize = 
    'End If
    'Return typeToDeserialize
    Return Type.GetType(String.Format("{0}, {1}", typeName, assemblyName))
  End Function
End Class
