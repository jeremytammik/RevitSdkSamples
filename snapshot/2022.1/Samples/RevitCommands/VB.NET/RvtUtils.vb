'
' (C) Copyright 2003-2019 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software in
' object code form for any purpose and without fee is hereby granted,
' provided that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'

Imports System.Collections.Generic
Imports Autodesk.Revit.DB

'
'  Utilities 
'
Public Class RvtUtils

   '==================================================================
   '
   '  Show Element Information 
   '
   '==================================================================
   '  List all the parameters of the given element. 
   ' 
   Public Shared Sub ListParameters(ByVal elem As Autodesk.Revit.DB.Element)

      ListInstanceParameters(elem)
      ListTypeParameters(elem)

   End Sub

   '------------------------------------------------------------------
   '  List Instance Parameters
   '
   Public Shared Sub ListInstanceParameters(ByVal elem As Autodesk.Revit.DB.Element)

      '  header 
      Dim str As String = "  --  Instance Paramaters  --  " & vbCr & vbCr

      '  get params
      Dim params As ParameterSet = elem.Parameters
      str = str + ParameterSetToString(elem.Document, params)

      '  show it
      MsgBox(str)

   End Sub

   '------------------------------------------------------------------
   '  List Type Parameters
   '
   Public Shared Sub ListTypeParameters(ByVal elem As Autodesk.Revit.DB.Element)

      '  header
      Dim str As String = "  --  Type Paramaters  --  " & vbCr

      ' get the type. 
      Dim typeId As Autodesk.Revit.DB.ElementId = elem.GetTypeId()
      Dim type As Autodesk.Revit.DB.ElementType = elem.Document.GetElement(typeId)

      If Not (type Is Nothing) Then
         '  get family and type name.  Note: this is not the part of parameter set below. 
         Dim paramFamilyTypeName As Autodesk.Revit.DB.Parameter = type.Parameter( _
         BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM)
         str = str + "Family Type Name: " + ParameterToString(elem.Document, _
         paramFamilyTypeName) + vbCr + vbCr

         '  get parameter set. 
         Dim params As ParameterSet = type.Parameters
         str = str & ParameterSetToString(elem.Document, params)
      Else
         str = str & "none"
      End If

      '  show it. 
      MsgBox(str)

   End Sub

   '------------------------------------------------------------------
   '  Return a string form of a given parameter set  
   '
   Public Shared Function ParameterSetToString(ByVal rvtDoc As Autodesk.Revit.DB.Document, _
   ByVal params As ParameterSet) As String

      Dim str As String = ""

      '  does the element have parameters? if not, simply return. 
      If (rvtDoc Is Nothing) Or (params Is Nothing) Or params.IsEmpty Then
         str = "none" & vbCr
         Return str
      End If

      '  we have some parameters. show it. 
      Dim param As Autodesk.Revit.DB.Parameter
      For Each param In params
         Dim name As String = param.Definition.Name
         str = str & name & ": " & ParameterToString(rvtDoc, param) & vbCr
      Next

      Return str

   End Function

   '------------------------------------------------------------------
   '  Return a string form of a given parameter 
   '
   Public Shared Function ParameterToString(ByVal rvtDoc As Autodesk.Revit.DB.Document, _
   ByVal param As Autodesk.Revit.DB.Parameter) As String

      Dim val As String = "none"

      If (rvtDoc Is Nothing) Or (param Is Nothing) Then
         Return val
      End If

      Dim type As Autodesk.Revit.DB.StorageType = param.StorageType

      ' take the internal type of the parameter and convert it into a string
      Select Case type

         Case StorageType.Double
            val = param.AsDouble

         Case StorageType.ElementId
            ' get the element and use its name.
            Dim paraElem As Autodesk.Revit.DB.Element = rvtDoc.GetElement(param.AsElementId)
            If Not (paraElem Is Nothing) Then
               val = paraElem.Name
            End If

         Case StorageType.Integer
            val = param.AsInteger

         Case StorageType.String
            val = param.AsString
         Case StorageType.None
            val = param.AsValueString
         Case Else

      End Select

      Return val

   End Function

   '============================================================================
   ' 
   '  List geometry information of the given element. 
   ' 
   '============================================================================
   Public Shared Sub ListGeometry(ByVal rvtApp As Autodesk.Revit.ApplicationServices.Application, _
   ByVal elem As Autodesk.Revit.DB.Element)

      '  header.
      Dim str As String = "  --  Geometry  --  " & vbCr

      '  set the geometry option.
      Dim opt As Autodesk.Revit.DB.Options
      opt = rvtApp.Create.NewGeometryOptions
      opt.DetailLevel = Autodesk.Revit.DB.ViewDetailLevel.Fine

      '  does the element have geometry data? 
      Dim geomElem As Autodesk.Revit.DB.GeometryElement = elem.Geometry(opt)
      If geomElem Is Nothing Then
         MsgBox(str & "no data")
         Return
      End If

      str = GeometryElementToString(geomElem)

      MsgBox(str)

   End Sub

   '------------------------------------------------------------------
   Public Shared Function GeometryElementToString( _
   ByVal geomElem As Autodesk.Revit.DB.GeometryElement) As String

      'Dim geomObjs As Autodesk.Revit.DB.GeometryObjectArray = geomElem.Objects
      Dim Objects As IEnumerator(Of GeometryObject) = geomElem.GetEnumerator()
      Dim geomObj As GeometryObject

      Dim objectsSize As Integer = 0

      Do While Objects.MoveNext
         objectsSize = objectsSize + 1
      Loop

      Dim str As String = ""
      str = str & "Total number of GeometryObject: " & objectsSize & vbCr

      Objects.Reset()

      'For Each geomObj In geomObjs
      Do While Objects.MoveNext

         geomObj = Objects.Current

         If TypeOf geomObj Is Autodesk.Revit.DB.Solid Then  '  ex. wall

            Dim solid As Autodesk.Revit.DB.Solid = geomObj
            str = str & GeometrySolidToString(solid)

         ElseIf TypeOf geomObj Is Autodesk.Revit.DB.GeometryInstance Then ' ex. door/window

            str = str & "  -- Geometry.Instance -- " & vbCr
            Dim geomInstance As Autodesk.Revit.DB.GeometryInstance = geomObj
            Dim geoElem As Autodesk.Revit.DB.GeometryElement = geomInstance.SymbolGeometry()

            str = str & GeometryElementToString(geoElem)

         ElseIf TypeOf geomObj Is Autodesk.Revit.DB.Curve Then ' ex. 

            Dim curv As Autodesk.Revit.DB.Curve = geomObj
            str = str & GeometryCurveToString(curv)

         ElseIf TypeOf geomObj Is Autodesk.Revit.DB.Mesh Then ' ex. 

            Dim mesh As Autodesk.Revit.DB.Mesh = geomObj
            str = str & GeometryMeshToString(mesh)

         Else
            str = str & "  *** unkown geometry type" & geomObj.GetType.ToString

         End If

      Loop

      Return str

   End Function

   '------------------------------------------------------------------
   Public Shared Function GeometrySolidToString(ByVal solid _
   As Autodesk.Revit.DB.Solid) As String

      Dim str As String = "  -- Geometry.Solid -- " + vbCr
      Dim faces As Autodesk.Revit.DB.FaceArray = solid.Faces

      str = str + "Total number of faces: " + faces.Size.ToString & vbCr

      Dim face As Autodesk.Revit.DB.Face
      Dim iface As Integer = 0

      For Each face In faces
         iface = iface + 1

         str = str + "Face " & iface & "/" & faces.Size.ToString & vbCr

         Dim edgeLoops As Autodesk.Revit.DB.EdgeArrayArray = face.EdgeLoops
         Dim iLoop As Integer = 0

         Dim edgeLoop As Autodesk.Revit.DB.EdgeArray
         For Each edgeLoop In edgeLoops
            iLoop = iLoop + 1
            str = str & "    EdgeLoop[" & iLoop.ToString & "] "
            Dim edge As Autodesk.Revit.DB.Edge

            For Each edge In edgeLoop
               Dim pts As List(Of Autodesk.Revit.DB.XYZ)
               pts = edge.Tessellate
               str = str + PointArrayToString(pts)

            Next
            str = str & vbCr
         Next

      Next

      Return str

   End Function

   '------------------------------------------------------------------
   Public Shared Function GeometryCurveToString(ByVal curv As Autodesk.Revit.DB.Curve) _
   As String

      Dim str As String = "  -- Geometry.Curve -- " + vbCr
      Dim pts As List(Of Autodesk.Revit.DB.XYZ) = curv.Tessellate
      str = str + PointArrayToString(pts)

      Return str

   End Function

   '------------------------------------------------------------------
   Public Shared Function GeometryMeshToString(ByVal mesh As Autodesk.Revit.DB.Mesh) _
   As String

      Dim str As String = "  -- Geometry.Mesh -- " + vbCr
      Dim pts As List(Of Autodesk.Revit.DB.XYZ)
      pts = mesh.Vertices
      str = str + PointArrayToString(pts)

      Return str

   End Function

   Public Shared Function PointArrayToString(ByVal pts As List(Of Autodesk.Revit.DB.XYZ)) _
   As String

      Dim str As String = ""
      Dim pt As Autodesk.Revit.DB.XYZ
      For Each pt In pts
         str = str + PointToString(pt)
      Next
      Return str

   End Function

   Public Shared Function PointToString(ByVal pt As Autodesk.Revit.DB.XYZ) As String

      Dim str As String = "(" & pt.X.ToString("F3") & ", " & pt.Y.ToString("F3") & ", " _
      & pt.Z.ToString("F3") & ") "
      Return str

   End Function

   '============================================================================
   ' 
   '  List location of the given element. 
   ' 
   '============================================================================
   Public Shared Sub ListLocation(ByVal elem As Autodesk.Revit.DB.Element)

      ' header
      Dim str As String = "  --  Location  -- " & vbCr

      ' do we have a location data? 
      Dim loc As Location = elem.Location
      If (loc Is Nothing) Then
         MsgBox(str & "no data")
         Return
      End If

      '  we have location data.  show it.

      ' location type is LocationCurve 
      If TypeOf loc Is LocationCurve Then
         Dim crv As LocationCurve = loc
         Dim startPt As Autodesk.Revit.DB.XYZ = crv.Curve.GetEndPoint(0)
         Dim endPt As Autodesk.Revit.DB.XYZ = crv.Curve.GetEndPoint(1)
         str = str & "Start Point: " & startPt.X & ", " & startPt.Y & ", " & startPt.Z & vbCr
         str = str & "End Point: " & endPt.X & ", " & endPt.Y & ", " & endPt.Z

         ' location type is LocationPoint 
      ElseIf TypeOf loc Is LocationPoint Then
         Dim locPt As LocationPoint = loc
         Dim pt As Autodesk.Revit.DB.XYZ = locPt.Point
         str = str & "Point: " & pt.X & ", " & pt.Y & ", " & pt.Z & vbCr

         ' Others 
      Else
         str = str & "this is not LocationCurve nor LocationPoint"
      End If

      '  show it.
      MsgBox(str)

   End Sub

End Class
