'
' (C) Copyright 2003-2016 by Autodesk, Inc. 
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

Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB.Structure

' All Autodesk Revit external commands must support this interface


''' <summary>
''' Get the material physical properties of the selected beam, column or brace.
''' Get all material types and their sub types to the user 
''' and then change the material type of the selected beam to the one chosen by the user.
''' With a selected concrete beam, column or brace, change its unit weight to 145 P/ft3.
''' </summary>
''' <remarks></remarks>
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
<Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)> _
Public Class MaterialProperties
    Implements Autodesk.Revit.UI.IExternalCommand

    'coefficient of converting unit weight from internal unit to metric unit
    Const ToMetricUnitWeight As Double = 0.010764
    'coefficient of converting stress from internal unit to metric unit
    Const ToMetricStress As Double = 0.334554
    'coefficient of converting unit weight from internal unit to imperial unit
    Const ToImperialUnitWeight As Double = 6.365827
    'the value of unit weight of selected component to be set
    Const ChangedUnitWeight As Double = 14.5

    Dim m_revit As Autodesk.Revit.UI.UIApplication = Nothing
    'hashtable contains all materials with index of their ElementId
    Dim m_allMaterialMap As Hashtable = New Hashtable

    'selected beam, column or brace
    Dim m_selectedComponent As Autodesk.Revit.DB.FamilyInstance = Nothing

    'current material of selected beam, column or brace
    Dim m_currentMaterial As Parameter = Nothing

    'arraylist of all materials belonging to steel type
    Dim m_steels As ArrayList = New ArrayList

    'arraylist of all materials belonging to concrete type
    Dim m_concretes As ArrayList = New ArrayList

   ReadOnly Property CurrentType() As StructuralAssetClass
      Get
         Dim materialId As Integer = 0

         If Not m_currentMaterial Is Nothing Then
            materialId = m_currentMaterial.AsElementId().IntegerValue
         End If

         If materialId <= 0 Then
            Return StructuralAssetClass.Generic
         End If

         Dim materialElem As Autodesk.Revit.DB.Material = _
         CType(m_allMaterialMap(materialId), Autodesk.Revit.DB.Material)

         If Nothing Is materialElem Then
            Return StructuralAssetClass.Generic
         End If

         Return GetMaterialType(materialElem)
      End Get

   End Property

    'get the material attribute of selected element
    ReadOnly Property CurrentMaterial() As Object
        Get
            Dim materialElem As Autodesk.Revit.DB.Material = GetCurrentMaterial()
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
         typeAL.Add("Undefined")
         typeAL.Add("Basic")
         typeAL.Add("Generic")
         typeAL.Add("Metal")
         typeAL.Add("Concrete")
         typeAL.Add("Wood")
         typeAL.Add("Liquid")
         typeAL.Add("Gas")
         typeAL.Add("Plastic")
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
   ''' Cancelled can be used to signify that the user canceled the external operation 
    ''' at some point. Failure should be returned if the application is unable to proceed with 
    ''' the operation.</returns>
    Public Function Execute(ByVal commandData As Autodesk.Revit.UI.ExternalCommandData, _
    ByRef message As String, _
    ByVal elements As Autodesk.Revit.DB.ElementSet) As  _
    Autodesk.Revit.UI.Result Implements Autodesk.Revit.UI.IExternalCommand.Execute

        Dim revit As Autodesk.Revit.UI.UIApplication = commandData.Application
        Dim documentTransaction As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(commandData.Application.ActiveUIDocument.Document, "Document")

        m_revit = revit
        If Not (Init()) Then
            'there must be exactly one beam, column or brace selected
            TaskDialog.Show("Revit", "You should select only one beam, structural column or brace.")
            Return Autodesk.Revit.UI.Result.Failed
        End If

        documentTransaction.Start()

        Dim displayForm As MaterialPropertiesForm = New MaterialPropertiesForm(Me)
        Try
            displayForm.ShowDialog()
        Catch ex As Exception
            TaskDialog.Show("Revit", "Sorry that your command failed.")
            Return Autodesk.Revit.UI.Result.Failed
        End Try
        documentTransaction.Commit()
        Return Autodesk.Revit.UI.Result.Succeeded
    End Function

    'get a datatable contains parameters' information of certain element
   Public Function GetParameterTable(ByVal o As System.Object, _
   ByVal substanceKind As StructuralAssetClass) As DataTable
      'create an empty data table
      Dim parameterTable As DataTable = CreateTable()

      'if failed to convert object
      If TypeOf o Is Autodesk.Revit.DB.Material Then
         Dim material As Autodesk.Revit.DB.Material = _
         CType(o, Autodesk.Revit.DB.Material)
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

         ' Resistance Calc Strength
         temporaryAttribute = _
         material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_RESISTANCE_CALC_STRENGTH)
         If Not (temporaryAttribute Is Nothing) Then
            temporaryValue = temporaryAttribute.AsValueString()
            AddDataRow(temporaryAttribute.Definition.Name, temporaryValue, parameterTable)
         End If

         'For Steel only: 
         If substanceKind = StructuralAssetClass.Metal Then
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
         If substanceKind = StructuralAssetClass.Concrete Then
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

        If (TypeOf o Is Autodesk.Revit.DB.Material) Then
            Dim material As Autodesk.Revit.DB.Material
            material = o
            Dim identity As Autodesk.Revit.DB.ElementId = material.Id
            m_currentMaterial.Set(identity)
        End If

    End Sub

    'change unit weight of selected component to 14.50 kN/m3
    Public Function ChangeUnitWeight() As Boolean
        Dim material As Autodesk.Revit.DB.Material = GetCurrentMaterial()
        If material Is Nothing Then
            Return False
        End If

        Dim weightPara As Parameter = _
        material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_UNIT_WEIGHT)
        weightPara.Set(ChangedUnitWeight / ToMetricUnitWeight)

        Return True
    End Function

    'get current material of selected component
    Private Function GetCurrentMaterial() As Autodesk.Revit.DB.Material
      'get the value of current material's ElementId
        Dim identityValue As Integer = 0

        If Not m_currentMaterial Is Nothing Then
            identityValue = m_currentMaterial.AsElementId().IntegerValue
        End If

        'material has no value
        If (identityValue <= 0) Then
            Return Nothing
        End If
        Dim material As Autodesk.Revit.DB.Material = _
        CType(m_allMaterialMap(identityValue), Autodesk.Revit.DB.Material)
        Return material
    End Function

    'firstly, check whether only one beam, column or brace is selected then initialize some member variables
    Private Function Init() As Boolean
        'selected 0 or more than 1 component
      If m_revit.ActiveUIDocument.Selection.GetElementIds().Count <> 1 Then
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
      Dim componentCollection As Autodesk.Revit.DB.ElementSet = New Autodesk.Revit.DB.ElementSet()
      Dim elementId As Autodesk.Revit.DB.ElementId
      For Each elementId In m_revit.ActiveUIDocument.Selection.GetElementIds()
         componentCollection.Insert(m_revit.ActiveUIDocument.Document.GetElement(elementId))
      Next
      If (componentCollection.Size <> 1) Then
         Return
      End If

      'if the selection is a beam, column or brace, find out its parameters for display
      Dim o As Object
      For Each o In componentCollection
         If TypeOf o Is Autodesk.Revit.DB.FamilyInstance Then
            Dim component As Autodesk.Revit.DB.FamilyInstance = o
            'selection is a beam, column or brace, find out its parameters
            If component.StructuralType = Autodesk.Revit.DB.Structure.StructuralType.Beam _
            Or component.StructuralType = Autodesk.Revit.DB.Structure.StructuralType.Brace _
            Or component.StructuralType = Autodesk.Revit.DB.Structure.StructuralType.Column Then
               'get selected beam, column or brace
               m_selectedComponent = component
            End If
            Dim p As Object
            For Each p In component.Parameters
               If TypeOf p Is Parameter Then
                  Dim attribute As Parameter = p
                  Dim parameterName As String = attribute.Definition.Name
                  '' The "Beam Material" and "Column Material" family parameters have been replaced
                  '' by the built-in parameter "Structural Material".
                  ''If parameterName = "Column Material" Or parameterName = "Beam Material" Then
                  If parameterName = "Structural Material" Then
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
        Dim collector As Autodesk.Revit.DB.FilteredElementCollector = New Autodesk.Revit.DB.FilteredElementCollector(m_revit.ActiveUIDocument.Document)
        Dim i As Autodesk.Revit.DB.FilteredElementIterator = collector.OfClass(GetType(Autodesk.Revit.DB.Material)).GetElementIterator()
        i.Reset()
        Dim moreValue As Boolean = i.MoveNext()
        While moreValue
            If TypeOf i.Current Is Autodesk.Revit.DB.Material Then
                Dim material As Autodesk.Revit.DB.Material = i.Current
            Dim materialType As StructuralAssetClass = GetMaterialType(material)

                'add materials to different ArrayList according to their types
                Select Case materialType
               Case StructuralAssetClass.Metal
                  m_steels.Add(New MaterialMap(material))
               Case StructuralAssetClass.Concrete
                  m_concretes.Add(New MaterialMap(material))
                    Case Else
                End Select
                'map between materials and their elementId
                m_allMaterialMap.Add(material.Id.IntegerValue, material)
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

    'Get the material type via giving material.
    'According to my knowledge, the material type can be retrieved by two ways now:
    '1. If the PropertySetElement exists, retrieve it by PHY_MATERIAL_PARAM_CLASS parameter. (via PropertySetElement class)
    '2. If it's indenpendent, retrieve it by PHY_MATERIAL_PARAM_TYPE parameter(via Material class)
   Private Function GetMaterialType(ByVal material As Autodesk.Revit.DB.Material) As StructuralAssetClass
      If (material.StructuralAssetId <> Autodesk.Revit.DB.ElementId.InvalidElementId) Then
         Dim propElem As Autodesk.Revit.DB.PropertySetElement = _
             CType(m_revit.ActiveUIDocument.Document.GetElement(material.StructuralAssetId), Autodesk.Revit.DB.PropertySetElement)
         Dim propElemPara As Parameter = propElem.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_CLASS)
         If Not propElemPara Is Nothing Then
            Return CType(propElemPara.AsInteger(), StructuralAssetClass)
         End If
      End If
      Return StructuralAssetClass.Generic
      'Dim propElemId As Autodesk.Revit.DB.ElementId = material.GetMaterialAspectPropertySet(MaterialAspect.Structural)
      'If (Autodesk.Revit.DB.ElementId.InvalidElementId = propElemId) Then
      '   Dim independentPara As Parameter = material.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_TYPE)
      '   If Nothing Is independentPara Then
      '      Return MaterialType.Generic
      '   End If
      '   Return CType(independentPara.AsInteger(), MaterialType)
      'End If

      'Dim propElem As Autodesk.Revit.DB.PropertySetElement = _
      '    CType(m_revit.ActiveUIDocument.Document.GetElement(propElemId), Autodesk.Revit.DB.PropertySetElement)
      'Dim propElemPara As Parameter = propElem.Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_CLASS)
      'If Nothing Is propElemPara Then
      '   Return MaterialType.Generic
      'End If
      'Return CType(propElemPara.AsInteger(), MaterialType)
   End Function
End Class

''' <summary>
''' Assistant class contains material and its name
''' </summary>
''' <remarks></remarks>
Public Class MaterialMap
    Dim m_materialName As String
    Dim m_material As Autodesk.Revit.DB.Material
    'constructor without parameter is forbidden
    Private Sub New()
    End Sub

    'constructor
    Public Sub New(ByVal material As Autodesk.Revit.DB.Material)
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
    Public ReadOnly Property Material() As Autodesk.Revit.DB.Material
        Get
            Return m_material
        End Get
    End Property
End Class
