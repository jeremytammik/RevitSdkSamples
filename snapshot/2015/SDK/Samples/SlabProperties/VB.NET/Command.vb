'
' (C) Copyright 2003-2014 by Autodesk, Inc.
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
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.?AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'


Imports System
Imports System.Windows.Forms

Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB.Structure


''' <summary>
''' Get some properties of a slab , such as Level, Type name, Span direction,
''' Material name, Thickness, and Young Modulus for the slab's Material.
''' </summary>
''' <remarks></remarks>
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class Command
    Implements IExternalCommand

    Private Const PI As Double = 3.1415926535879
    Private Const Degree As Integer = 180
    Private Const ToMillimeter As Integer = 1000
    Private Const ToMetricThickness As Double = 0.3048
    Private Const ToMetricYoungModulus As Double = 304800.0

    Private m_slabComponent As Autodesk.Revit.DB.ElementSet
    Private m_slabFloor As Autodesk.Revit.DB.Floor
    Private m_slabLayer As Autodesk.Revit.DB.CompoundStructureLayer
    Private m_slabLayerCollection As System.Collections.Generic.IList(Of Autodesk.Revit.DB.CompoundStructureLayer)
    Private m_document As Autodesk.Revit.DB.Document

    Private m_level As String
    Private m_typeName As String
    Private m_spanDirection As String
    Private m_thickness As String
    Private m_materialName As String
    Private m_youngModulusX As String
    Private m_youngModulusY As String
    Private m_youngModulusZ As String
    Private m_numberOfLayers As Integer

    ''' <summary>
    ''' Implement this method as an external command for Revit.
    ''' </summary>
    ''' <param name="commandData">An object that is passed to the external application 
    ''' which contains data related to the command, 
    ''' such as the application object and active view.</param>
    ''' <param name="message">A message that can be set by the external application 
    ''' which will be displayed if a failure or cancellation is returned by 
    ''' the external command.</param>
    ''' <param name="elements">A set of elements to which the external application 
    ''' can add elements that are to be highlighted in case of failure or cancellation.</param>
    ''' <returns>Return the status of the external command. 
    ''' A result of Succeeded means that the API external method functioned as expected. 
    ''' Cancelled can be used to signify that the user cancelled the external operation 
    ''' at some point. Failure should be returned if the application is unable to proceed with 
    ''' the operation.</returns>
    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.DB.ElementSet) _
    As Autodesk.Revit.UI.Result Implements IExternalCommand.Execute

        Dim revit As Autodesk.Revit.UI.UIApplication = commandData.Application

        Try
            'function initialization and find out a slab's Level, Type name, and set the Span Direction properties.
            Dim isInitialization As Boolean = Me.Initialize(revit)

            If False = isInitialization Then

                Return Autodesk.Revit.UI.Result.Failed

            End If

            'show a displayForm to display the properties of the slab
            Dim slabForm As New SlabPropertiesForm(Me)

            If slabForm.ShowDialog() <> DialogResult.OK Then

                Return Autodesk.Revit.UI.Result.Cancelled

            End If

        Catch displayProblem As Exception

            TaskDialog.Show("Revit", displayProblem.ToString())
            Return Autodesk.Revit.UI.Result.Failed

        End Try

        Return Autodesk.Revit.UI.Result.Succeeded
    End Function

    ''' <summary>
    ''' get thickness, type name and young modulus of someone layer
    ''' </summary>
    ''' <param name="layerNumber">the layer to be retrieved</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetLayer(ByVal layerNumber As Integer) As Boolean

        'Get each layer.
        'An individual layer can be accessed by Layers property and its thickness and material can then be reported.
        m_slabLayer = m_slabLayerCollection.Item(layerNumber)

        'Get the Thickness property and change to the metric millimeter
        m_thickness = ((m_slabLayer.Width) * ToMetricThickness * ToMillimeter).ToString() + " mm"

        'Get the Material name property
        If Not (m_slabLayer.MaterialId Is Autodesk.Revit.DB.ElementId.InvalidElementId) Then
            Dim material As Autodesk.Revit.DB.Material = Nothing
            material = m_document.GetElement(m_slabLayer.MaterialId)
            m_materialName = material.Name

        Else

            m_materialName = "Null"

        End If

        'The Young modulus can be found from the material by using the following generic parameters: 
        'PHY_MATERIAL_PARAM_YOUNG_MOD1, PHY_MATERIAL_PARAM_YOUNG_MOD2, PHY_MATERIAL_PARAM_YOUNG_MOD3
        If Not (m_slabLayer.MaterialId Is Autodesk.Revit.DB.ElementId.InvalidElementId) Then
            Dim material As Autodesk.Revit.DB.Material = Nothing
            Dim youngModuleAttribute As Parameter = Nothing
            material = m_document.GetElement(m_slabLayer.MaterialId)
            youngModuleAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD1)
            If Not (youngModuleAttribute Is Nothing) Then

                m_youngModulusX = (youngModuleAttribute.AsDouble() / ToMetricYoungModulus).ToString("F2") + " MPa"

            End If

            youngModuleAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD2)
            If Not (youngModuleAttribute Is Nothing) Then

                m_youngModulusY = (youngModuleAttribute.AsDouble() / ToMetricYoungModulus).ToString("F2") + " MPa"

            End If

            youngModuleAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD3)
            If Not (youngModuleAttribute Is Nothing) Then

                m_youngModulusZ = (youngModuleAttribute.AsDouble() / ToMetricYoungModulus).ToString("F2") + " MPa"

            End If
        Else

            m_youngModulusX = "Null"
            m_youngModulusY = "Null"
            m_youngModulusZ = "Null"

        End If

        Return True

    End Function

    ''' <summary>
    ''' get level of Slab
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLevel() As String

        Return m_level

    End Function

    ''' <summary>
    ''' get Type name of Slab
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTypeName() As String

        Return m_typeName

    End Function

    ''' <summary>
    ''' get span direction of Slab
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpanDirection() As String

        Return m_spanDirection

    End Function

    ''' <summary>
    ''' get layer number of Slab
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLayerNum() As Integer

        Return m_numberOfLayers

    End Function

    ''' <summary>
    ''' get layer thickness of slab
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLayerThickness() As String

        Return m_thickness

    End Function

    ''' <summary>
    ''' get layer material name of Slab
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLayerMaterialName() As String

        Return m_materialName

    End Function

    ''' <summary>
    ''' get layer Younf modulus X
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLayerYoungModuleX() As String

        Return m_youngModulusX

    End Function

    ''' <summary>
    ''' get layer Younf modulus Y
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>    
    Public Function GetLayerYoungModuleY() As String

        Return m_youngModulusY

    End Function

    ''' <summary>
    ''' get layer Younf modulus Z
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLayerYoungModuleZ() As String

        Return m_youngModulusZ

    End Function

    ''' <summary>
    ''' Initialization and find out a slab's Level, Type name, and set the Span Direction properties.
    ''' </summary>
    ''' <param name="revit"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Initialize(ByVal revit As Autodesk.Revit.UI.UIApplication) As Boolean

      m_slabComponent = New Autodesk.Revit.DB.ElementSet()
      Dim elementId As Autodesk.Revit.DB.ElementId
      For Each elementId In revit.ActiveUIDocument.Selection.GetElementIds()
         m_slabComponent.Insert(revit.ActiveUIDocument.Document.GetElement(elementId))
      Next
        m_document = revit.ActiveUIDocument.Document

        ' There must be exactly one slab selected
        If m_slabComponent.IsEmpty Then

            'nothing selected
            TaskDialog.Show("Revit", "Please select a slab.")

            Return False

        ElseIf 1 <> m_slabComponent.Size Then

            'too many things selected
            TaskDialog.Show("Revit", "Please select only one slab.")

            Return False

        End If

        For Each e As Autodesk.Revit.DB.Element In m_slabComponent

            'If the element isn't a slab, give the message and return failure.
            'Else find out its Level, Type name, and set the Span Direction properties.
            If e.GetType().ToString() <> "Autodesk.Revit.DB.Floor" Then

                Autodesk.Revit.UI.TaskDialog.Show("Revit", "A slab must be selected.")
                Return False

            End If

            'Change the element type to floor type
            m_slabFloor = CType(e, Autodesk.Revit.DB.Floor)

            'Get the layer information from the type object by using the CompoundStructure property
            'The Layers property is then used to retrieve all the layers
            m_slabLayerCollection = m_slabFloor.FloorType.GetCompoundStructure.GetLayers
            m_numberOfLayers = m_slabLayerCollection.Count

            'Get the Level property by the floor's Level property
            m_level = DirectCast(m_document.GetElement(m_slabFloor.LevelId), Level).Name

            'Get the Type name property by the floor's FloorType property
            m_typeName = m_slabFloor.FloorType.Name

            'The span direction can be found using generic parameter access 
            'using the built in parameter FLOOR_PARAM_SPAN_DIRECTION
            Dim spanDirectionAttribute As Parameter
            spanDirectionAttribute = m_slabFloor.Parameter(BuiltInParameter.FLOOR_PARAM_SPAN_DIRECTION)
            If Not (spanDirectionAttribute Is Nothing) Then

                'Set the Span Direction property
                Me.SetSpanDirection(spanDirectionAttribute.AsDouble())

            End If
        Next

        Return True

    End Function

    ''' <summary>
    ''' Set SpanDirection property to the class private member
    ''' Because of the property retrieved from the parameter uses radian for unit, we should change it to degree.
    ''' </summary>
    ''' <param name="spanDirection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetSpanDirection(ByVal spanDirection As Double) As Boolean

        Dim spanDirectionDegree As Double

        'Change "radian" to "degree".
        spanDirectionDegree = spanDirection / PI * Degree

        'If the absolute value very small, we consider it to be zero
        If Math.Abs(spanDirectionDegree) < 0.000000000001 Then

            spanDirectionDegree = 0.0

        End If

        'The precision is 0.01, and unit is "degree".
        m_spanDirection = spanDirectionDegree.ToString("F2")
        Return True

    End Function

End Class

