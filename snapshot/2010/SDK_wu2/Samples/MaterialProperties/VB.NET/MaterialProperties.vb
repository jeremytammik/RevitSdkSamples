'
' (C) Copyright 2003-2009 by Autodesk, Inc. 
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


Imports System
Imports System.Data
Imports System.Text
Imports System.Windows.Forms
Imports System.Collections

Imports Autodesk.Revit
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Parameters
Imports Autodesk.Revit.Structural.Enums

' All Autodesk Revit external commands must support this interface
'three basic material types in Revit
Public Enum MaterialType
    Generic = 0
    Concrete
    Steel
End Enum

''' <summary>
''' Get the material physical properties of the selecte beam, column or brace.
''' Get all material types and their sub types to the user 
''' and then change the material type of the selected beam to the one chosen by the user.
''' With a selected concrete beam, column or brace, change its unit weight to 145 P/ft3.
''' </summary>
''' <remarks></remarks>
Public Class MaterialProperties
    Implements Autodesk.Revit.IExternalCommand

    'coefficient of converting unit weight from internal unit to metric unit
    Const ToMetricUnitWeight As Double = 0.010764
    'coefficient of converting stress from internal unit to metric unit
    Const ToMetricStress As Double = 0.334554
    'coefficient of converting unit weight from internal unit to imperial unit
    Const ToImperialUnitWeight As Double = 6.365827
    'the value of unit weight of selected component to be set
    Const ChangedUnitWeight As Double = 14.5

    Dim m_revit As Autodesk.Revit.Application = Nothing
    'hashtable contains all materials with index of their ElementId
    Dim m_allMaterialMap As Hashtable = New Hashtable

    'selected beam, column or brace
    Dim m_selectedComponent As Autodesk.Revit.Elements.FamilyInstance = Nothing

    'current material of selected beam, column or brace
    Dim m_currentMaterial As Parameter = Nothing

    'arraylist of all materials belonging to steel type
    Dim m_steels As ArrayList = New ArrayList

    'arraylist of all materials belonging to concrete type
    Dim m_concretes As ArrayList = New ArrayList

    ReadOnly Property CurrentType() As MaterialType
        Get
            Dim materialId As Integer = 0

            If Not m_currentMaterial Is Nothing Then
                materialId = m_currentMaterial.AsElementId().Value
            End If

            If materialId <= 0 Then
                Return MaterialType.Generic
            End If

            Dim materialElem As Autodesk.Revit.Elements.Material = _
            CType(m_allMaterialMap(materialId), Autodesk.Revit.Elements.Material)

            If Nothing Is materialElem Then
                Return MaterialType.Generic
            End If

            Dim materialPara As Parameter = _
            materialElem.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_TYPE)
            If Nothing Is materialPara Then
                Return MaterialType.Generic
            End If
            Return CType(materialPara.AsInteger(), MaterialType)
        End Get

    End Property

    'get the material attribute of selected element
    ReadOnly Property CurrentMaterial() As Object
        Get
            Dim materialElem As Autodesk.Revit.Elements.Material = GetCurrentMaterial()
            If materialElem Is Nothing Then
                Return Nothing
            End If
            Return materialElem

        End Get
    End Property

    'arraylist of all materials belonging to steel type
    ReadOnly Property SteelCollection() As ArrayList
        Get
            Return m_steels

        End Get
    End Property

    'arraylist of all materials belonging to concrete type
    ReadOnly Property ConcreteCollection() As ArrayList
        Get
            Return m_concretes
        End Get
    End Property

    'three basic material types in Revit
    ReadOnly Property MaterialTypes() As ArrayList
        Get
            Dim typeAL As ArrayList = New ArrayList
            typeAL.Add("Generic")
            typeAL.Add("Concrete")
            typeAL.Add("Steel")
            Return typeAL
        End Get
    End Property

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
    Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, _
    ByRef message As String, _
    ByVal elements As Autodesk.Revit.ElementSet) As _
    Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute

        Dim revit As Autodesk.Revit.Application = commandData.Application

        m_revit = revit
        If Not (Init()) Then
            'there must be exactly one beam, column or brace selected
            MessageBox.Show("You should select only one beam, structural column or brace.")
            Return Autodesk.Revit.IExternalCommand.Result.Failed
        End If

        Dim displayForm As MaterialPropertiesForm = New MaterialPropertiesForm(Me)
        Try
            displayForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Sorry that your command failed.")
            Return Autodesk.Revit.IExternalCommand.Result.Failed
        End Try
        Return Autodesk.Revit.IExternalCommand.Result.Succeeded
    End Function

    'get a datatable contains parameters' information of certain element
    Public Function GetParameterTable(ByVal o As System.Object, _
    ByVal substanceKind As MaterialType) As DataTable
        'create an empty data table
        Dim parameterTable As DataTable = CreateTable()

        'if failed to convert object
        If TypeOf o Is Autodesk.Revit.Elements.Material Then
            Dim material As Autodesk.Revit.Elements.Material = _
            CType(o, Autodesk.Revit.Elements.Material)
            Dim temporaryAttribute As Parameter = Nothing
            Dim temporaryValue As String = ""

            'Get all material element parameters
            Dim formatter As String = "#0.000000"
            ' Behavior
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_BEHAVIOR)
            Select Case temporaryAttribute.AsInteger()
                Case 0
                    AddDataRow(temporaryAttribute.Definition.Name, "Isotropic", parameterTable)
                Case 1
                    AddDataRow(temporaryAttribute.Definition.Name, "Orthotropic", parameterTable)
                Case Else
                    AddDataRow(temporaryAttribute.Definition.Name, "None", parameterTable)
            End Select

            'Young's Modulus
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD1)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD2)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD3)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)

            'Poisson Modulus
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD1)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD2)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_POISSON_MOD3)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)

            'Shear Modulus
            temporaryAttribute = _
            material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD1)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD2)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_MOD3)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)

            ' Thermal Expansion Coefficient
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF1)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF2)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_EXP_COEFF3)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)

            ' Unit Weight
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_UNIT_WEIGHT)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)

            ' Damping Ratio
            temporaryAttribute = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_DAMPING_RATIO)
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)

            ' Bending Reinforcement
            temporaryAttribute = _
            material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_BENDING_REINFORCEMENT)
            If Not (temporaryAttribute Is Nothing) Then
                temporaryValue = temporaryAttribute.AsValueString()
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            End If

            ' Shear Reinforcement
            temporaryAttribute = _
            material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_REINFORCEMENT)
            If Not (temporaryAttribute Is Nothing) Then
                temporaryValue = temporaryAttribute.AsValueString()
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            End If

            ' Resistance Calc Strengt
            temporaryAttribute = _
            material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_RESISTANCE_CALC_STRENGTH)
            If Not (temporaryAttribute Is Nothing) Then
                temporaryValue = temporaryAttribute.AsValueString()
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            End If

            'For Steel only: 
            If substanceKind = MaterialType.Steel Then
                ' Minimum Yield Stress
                temporaryAttribute = _
                material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_MINIMUM_YIELD_STRESS)
                temporaryValue = temporaryAttribute.AsValueString()
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)

                ' Minimum Tensile Strength
                temporaryAttribute = _
                material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_MINIMUM_TENSILE_STRENGTH)
                temporaryValue = temporaryAttribute.AsValueString()
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)

                ' Reduction Factor
                temporaryAttribute = _
                material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_REDUCTION_FACTOR)
                temporaryValue = temporaryAttribute.AsValueString()
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            End If

            'For Concrete only:
            If substanceKind = MaterialType.Concrete Then
                ' Concrete Compression     
                temporaryAttribute = _
                material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_CONCRETE_COMPRESSION)
                temporaryValue = temporaryAttribute.AsValueString()
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)

                ' Lightweight
                temporaryAttribute _
                = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_LIGHT_WEIGHT)
                temporaryValue = temporaryAttribute.AsValueString()
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)

                ' Shear Strength Reduction
                temporaryAttribute = _
                material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_SHEAR_STRENGTH_REDUCTION)
                temporaryValue = temporaryAttribute.AsValueString()
                AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
            End If

        Else
            Return parameterTable
        End If

        Return parameterTable
    End Function

    'set the material of selected component
    Sub SetMaterial(ByVal o As Object)

        If m_currentMaterial Is Nothing Then
            Return
        End If

        If (TypeOf o Is Autodesk.Revit.Elements.Material) Then
            Dim material As Autodesk.Revit.Elements.Material
            material = o
            Dim identity As Autodesk.Revit.ElementId = material.Id
            m_currentMaterial.Set(identity)
        End If

    End Sub

    'change unit weight of selected component to 14.50 kN/m3
    Public Function ChangeUnitWeight() As Boolean
        Dim material As Autodesk.Revit.Elements.Material = GetCurrentMaterial()
        If material Is Nothing Then
            Return False
        End If

        Dim weightPara As Parameter = _
        material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_UNIT_WEIGHT)
        weightPara.Set(ChangedUnitWeight / ToMetricUnitWeight)

        Return True
    End Function

    'get current material of selected component
    Private Function GetCurrentMaterial() As Autodesk.Revit.Elements.Material
        'get the value of current material's ElementID
        Dim identityValue As Integer = 0

        If Not m_currentMaterial Is Nothing Then
            identityValue = m_currentMaterial.AsElementId().Value
        End If

        'material has no value
        If (identityValue <= 0) Then
            Return Nothing
        End If
        Dim material As Autodesk.Revit.Elements.Material = _
        CType(m_allMaterialMap(identityValue), Autodesk.Revit.Elements.Material)
        Return material
    End Function

    'firstly, check whether only one beam, column or brace is selected then initialize some member variables
    Private Function Init() As Boolean
        'selected 0 or more than 1 component
        If m_revit.ActiveDocument.Selection.Elements.Size <> 1 Then
            Return False
        End If

        Try
            GetSelectedComponent()
            'selected component isn't beam, column or brace
            If m_selectedComponent Is Nothing Then
                Return False
            End If

            'initialize some member variables
            GetAllMaterial()
            Return True
        Catch ex As Exception
            Return False

        End Try
    End Function

    'get selected beam, column or brace
    Private Sub GetSelectedComponent()
        Dim componentCollection As ElementSet = m_revit.ActiveDocument.Selection.Elements
        If (componentCollection.Size <> 1) Then
            Return
        End If

        'if the selection is a beam, column or brace, find out its parameters for display
        Dim o As Object
        For Each o In componentCollection
            If TypeOf o Is Autodesk.Revit.Elements.FamilyInstance Then
                Dim component As Autodesk.Revit.Elements.FamilyInstance = o
                'selection is a beam, column or brace, find out its parameters
                If component.StructuralType = StructuralType.Beam _
                Or component.StructuralType = StructuralType.Brace _
                Or component.StructuralType = StructuralType.Column Then
                    'get selected beam, column or brace
                    m_selectedComponent = component
                End If
                Dim p As Object
                For Each p In component.Parameters
                    If TypeOf p Is Parameter Then
                        Dim attribute As Parameter = p
                        Dim parameterName As String = attribute.Definition.Name
                        If parameterName = "Column Material" Or parameterName = "Beam Material" Then
                            'get current material of selected component
                            m_currentMaterial = attribute                         
                            Exit For
                        End If
                    End If
                Next p
            End If
        Next o

    End Sub

    'get all materials exist in current document
    Private Sub GetAllMaterial()
        Dim i As ElementIterator = m_revit.ActiveDocument.Elements

        Dim moreValue As Boolean = i.MoveNext()
        While moreValue
            If TypeOf i.Current Is Autodesk.Revit.Elements.Material Then
                Dim material As Autodesk.Revit.Elements.Material = i.Current
                Dim materialAttribute As _
                Parameter = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_TYPE)
                If Not (materialAttribute Is Nothing) Then
                    'add materials to different ArrayList according to their types
                    Select Case CType(materialAttribute.AsInteger(), MaterialType)
                        Case MaterialType.Steel
                            m_steels.Add(New MaterialMap(material))
                        Case MaterialType.Concrete
                            m_concretes.Add(New MaterialMap(material))
                        Case Else
                    End Select
                    'map between materials and their elementId
                    m_allMaterialMap.Add(material.Id.Value, material)
                End If
            End If
            moreValue = i.MoveNext()
        End While
    End Sub

    'Create an empty table with parameter's name column and value column
    Private Function CreateTable() As DataTable
        'Create a new DataTable.
        Dim propDataTable As DataTable = New DataTable("ParameterTable")

        'Create parameter column and add to the DataTable.
        Dim paraDataColumn As DataColumn = New DataColumn
        paraDataColumn.DataType = System.Type.GetType("System.String")
        paraDataColumn.ColumnName = "Parameter"
        paraDataColumn.Caption = "Parameter"
        paraDataColumn.ReadOnly = True
        propDataTable.Columns.Add(paraDataColumn)

        'Create value column and add to the DataTable.
        Dim valueDataColumn As DataColumn = New DataColumn
        valueDataColumn.DataType = System.Type.GetType("System.String")
        valueDataColumn.ColumnName = "Value"
        valueDataColumn.Caption = "Value"
        valueDataColumn.ReadOnly = True
        propDataTable.Columns.Add(valueDataColumn)

        Return propDataTable

    End Function

    'add one row to datatable of parameter
    Private Sub AddDataRow(ByVal parameterName As String, _
    ByVal parameterValue As String, ByVal parameterTable As DataTable)
        Dim newRow As DataRow = parameterTable.NewRow()
        newRow("Parameter") = parameterName
        newRow("Value") = parameterValue
        parameterTable.Rows.Add(newRow)
    End Sub

End Class

''' <summary>
''' Assistant class contains material and it's name
''' </summary>
''' <remarks></remarks>
Public Class MaterialMap
    Dim m_materialName As String
    Dim m_material As Autodesk.Revit.Elements.Material
    'constructor without parameter is forbidden
    Private Sub New()
    End Sub

    'constructor
    Public Sub New(ByVal material As Autodesk.Revit.Elements.Material)
        m_materialName = material.Name
        m_material = material
    End Sub

    'Get material name
    Public ReadOnly Property MaterialName() As String
        Get
            Return m_materialName
        End Get
    End Property

    'Get material
    Public ReadOnly Property Material() As Autodesk.Revit.Elements.Material
        Get
            Return m_material
        End Get
    End Property
End Class
