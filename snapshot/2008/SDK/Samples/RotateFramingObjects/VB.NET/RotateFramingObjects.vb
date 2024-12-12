'
' (C) Copyright 2003-2007 by Autodesk, Inc.
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
Imports System.IO
Imports System.Windows.Forms
Imports Autodesk
Imports Autodesk.Revit
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Parameters
' rotate the objects that were selected when the command was executed.
' and allow the user input the amount, in degrees that the objects should be rotated. 
' the dialog contain option for the user to specify this value is absolute or relative. 
Public Class RotateFramingObjects
    Implements IExternalCommand

    Private m_revit As Autodesk.Revit.Application
    Private m_receiveRotationTextBox As Double
    Private m_isAbsoluteChecked As Boolean   ' true if moving absolute
    Private Const AngleDefinitionName As String = "Cross-Section Rotation"
    Public Property ReceiveRotationTextBox() As Double
        Get
            Return m_receiveRotationTextBox
        End Get
        Set(ByVal Value As Double)
            m_receiveRotationTextBox = Value
        End Set
    End Property
    Public Property IsAbsoluteChecked() As Boolean
        Get
            Return m_isAbsoluteChecked
        End Get
        Set(ByVal Value As Boolean)
            m_isAbsoluteChecked = Value
        End Set
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
    ''' Cancelled can be used to signify that the the user cancelled the external operation 
    ''' at some point. Failure should be returned if the application is unable to proceed with 
    ''' the operation.</returns>
    Public Function Execute( _
   ByVal commandData As Autodesk.Revit.ExternalCommandData, _
   ByRef message As String, _
   ByVal elements As ElementSet _
   ) As IExternalCommand.Result Implements IExternalCommand.Execute
        'revit,the application object for the active instance of Autodesk Revit.
        'message,a message that can be set by the external command and displayed in case of error.
        'elements,a set of elements that can be displayed if an error occurs.
        'returns,a value that signifies if yout command was successful, failed or cancel.
        Dim revit As Autodesk.Revit.Application = commandData.Application
        m_revit = revit
        Dim displayForm As RotateFramingObjectForm = New RotateFramingObjectForm(Me)
        Dim selection As ElementSet = revit.ActiveDocument.Selection.Elements
        Dim isSingle As Boolean = True
        Dim isAllFamilyInstance As Boolean = True
        'There must be beams, braces or columns selected
        If (selection.IsEmpty) Then
            ' nothing selected
            message = "Please select FamilyInstance."
            Return Autodesk.Revit.IExternalCommand.Result.Failed
        ElseIf (1 <> selection.Size) Then
            isSingle = False
            Try
                If DialogResult.OK <> displayForm.ShowDialog Then
                    Return IExternalCommand.Result.Failed
                End If
            Catch ex As Exception
                Return IExternalCommand.Result.Failed
            End Try
            'not single object selected			
        End If

        'if the selected elements are familyInstances, try to get their existing rotation or angle
        Dim e As Autodesk.Revit.Element
        For Each e In selection
            Dim familyComponent As Autodesk.Revit.Elements.FamilyInstance
            familyComponent = Nothing
            If TypeOf e Is Autodesk.Revit.Elements.FamilyInstance Then
                familyComponent = e
            End If
            If Not (familyComponent Is Nothing) Then
                If Structural.Enums.StructuralType.Beam = familyComponent.StructuralType _
                         Or Structural.Enums.StructuralType.Brace = familyComponent.StructuralType _
                         Then
                    'selection is a beam  or a brace
                    Dim returnValue As String = Me.FindParameter(AngleDefinitionName, familyComponent)
                    displayForm.rotationTextBox.Text = returnValue.ToString()
                ElseIf Autodesk.Revit.Structural.Enums.StructuralType.Column = _
                familyComponent.StructuralType Then
                    'selection is a column
                    Dim columnLocation As Autodesk.Revit.Location = familyComponent.Location
                    Dim pointLocation As Autodesk.Revit.LocationPoint = Nothing
                    If TypeOf columnLocation Is Autodesk.Revit.LocationPoint Then
                        pointLocation = columnLocation
                    End If
                    Dim temp As Double = pointLocation.Rotation
                    Dim outPut As String = (Math.Round(temp * 180 / (Math.PI), 3)).ToString()
                    displayForm.rotationTextBox.Text = outPut
                Else
                    'other familyInstance can not be rotated
                    message = "Can't deal with it."
                    elements.Insert(familyComponent)
                    Return IExternalCommand.Result.Failed
                End If
            Else
                If isSingle Then
                    message = "It is a Non-FamilyInstance ."
                    elements.Insert(e)
                    Return IExternalCommand.Result.Failed
                Else
                    'there is some objects is not familyInstance
                    message = "There is Non-FamilyInstance ."
                    elements.Insert(e)
                    isAllFamilyInstance = False
                End If
            End If
        Next e
        If isSingle Then
            Try
                If DialogResult.OK <> displayForm.ShowDialog Then
                    Return IExternalCommand.Result.Failed
                End If
            Catch ex As Exception
                Return IExternalCommand.Result.Failed
            End Try
        End If
        If isAllFamilyInstance Then
            Return IExternalCommand.Result.Succeeded
        Else
            Return IExternalCommand.Result.Failed
        End If
    End Function
    'The function set value to rotation of the beams and braces
    'and rotate columns. 
    Public Sub RotateElement()
        Dim selection As ElementSet = m_revit.ActiveDocument.Selection.Elements
        Dim e As Autodesk.Revit.Element
        For Each e In selection
            Dim familyComponent As Autodesk.Revit.Elements.FamilyInstance
            familyComponent = Nothing
            If TypeOf e Is Autodesk.Revit.Elements.FamilyInstance Then
                familyComponent = e
            End If
            If Not familyComponent Is Nothing Then
                'if be familyInstance,judge the types of familyInstance
                If (Structural.Enums.StructuralType.Beam = familyComponent.StructuralType) _
                    Or (Structural.Enums.StructuralType.Brace = familyComponent.StructuralType) Then
                    Dim j As ParameterSetIterator = familyComponent.Parameters.ForwardIterator()
                    j.Reset()

                    Dim jMoreAttribute As Boolean = j.MoveNext
                    While jMoreAttribute
                        Dim a As Object = j.Current
                        Dim objectAttribute As Parameter = Nothing
                        If TypeOf a Is Parameter Then
                            objectAttribute = a
                        End If
                        Dim p As Integer = objectAttribute.Definition.Name.CompareTo(AngleDefinitionName)
                        If 0 = p Then
                            Dim temp As Double = objectAttribute.AsDouble
                            Dim rotateDegree As Double = m_receiveRotationTextBox * Math.PI / 180
                            If Not m_isAbsoluteChecked Then
                                rotateDegree += temp
                            End If
                            objectAttribute.Set(rotateDegree)
                        End If
                        jMoreAttribute = j.MoveNext()
                    End While
                ElseIf Autodesk.Revit.Structural.Enums.StructuralType.Column = _
                familyComponent.StructuralType Then
                    'rotate a column
                    Dim columnLocation As Location = familyComponent.Location
                    'get the location object
                    Dim pointLocation As LocationPoint = Nothing
                    If TypeOf columnLocation Is LocationPoint Then
                        pointLocation = columnLocation
                    End If
                    Dim insertPoint As Autodesk.Revit.Geometry.XYZ = pointLocation.Point
                    'get the location point
                    Dim temp As Double = pointLocation.Rotation
                    'existing rotation
                    Dim directionPoint As Autodesk.Revit.Geometry.XYZ
                    directionPoint = New Autodesk.Revit.Geometry.XYZ(0, 0, 1)
                    'define the vector of axis
                    Dim rotateAxis As Autodesk.Revit.Geometry.Line
                    rotateAxis = m_revit.Create.NewLineUnbound(insertPoint, directionPoint)
                    Dim rotateDegree As Double = m_receiveRotationTextBox * Math.PI / 180
                    ' rotate column by rotate method
                    If m_isAbsoluteChecked Then
                        rotateDegree -= temp
                    End If
                    Dim rotateResult As Integer = pointLocation.Rotate(rotateAxis, rotateDegree)
                    If 0 = rotateResult Then
                        MessageBox.Show("Rotate Failed")
                    End If
                End If
            End If
        Next
    End Sub

    'get the parameter value according to given parameter name
    Public Function FindParameter( _
       ByVal parameterName As String, _
       ByVal familyInstanceName As Autodesk.Revit.Elements.FamilyInstance _
        ) As String
        Dim i As ParameterSetIterator = familyInstanceName.Parameters.ForwardIterator
        i.Reset()

        Dim valueOfParameter As String = Nothing
        Dim imoreAttribute As Boolean = i.MoveNext
        While imoreAttribute
            Dim isFound As Boolean = False
            Dim o As Object = i.Current
            Dim familyAttribute As Parameter = Nothing
            If TypeOf o Is Parameter Then
                familyAttribute = o
            End If
            If parameterName = familyAttribute.Definition.Name Then
                Dim t As StorageType = familyAttribute.StorageType
                If t = (Autodesk.Revit.Parameters.StorageType.Double) Then
                    If AngleDefinitionName = parameterName Then
                        Dim temp As Double = familyAttribute.AsDouble
                        valueOfParameter = Math.Round(temp * 180 / (Math.PI), 3).ToString
                    Else
                        valueOfParameter = familyAttribute.AsDouble.ToString
                    End If
                ElseIf StorageType.ElementId = t Then
                    ' get element id as string
                    valueOfParameter = familyAttribute.AsElementId.ToString
                ElseIf StorageType.Integer = t Then
                    ' get integer as string
                    valueOfParameter = familyAttribute.AsInteger.ToString
                ElseIf StorageType.String = t Then
                    ' get string parameter
                    valueOfParameter = familyAttribute.AsString()
                End If
                isFound = True
            End If
            If isFound Then
                Exit While
            End If
            imoreAttribute = i.MoveNext
        End While
        'return the value.
        Return valueOfParameter
    End Function
End Class
