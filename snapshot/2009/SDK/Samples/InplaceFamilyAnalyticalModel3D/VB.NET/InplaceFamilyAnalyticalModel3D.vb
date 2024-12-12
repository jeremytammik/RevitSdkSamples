'
' (C) Copyright 2003-2008 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software in
' object code form for any purpose and without fee is hereby granted
' provided that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL ITS FAULTS.
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
Imports System.Collections

Imports Autodesk.Revit
Imports Autodesk.Revit.Parameters
Imports Autodesk.Revit.Elements
Imports Autodesk.Revit.Collections
Imports Autodesk.Revit.Events
Imports Autodesk.Revit.Symbols
Imports Autodesk.Revit.Structural
Imports Autodesk.Revit.Structural.Enums
Imports Autodesk.Revit.Creation
Imports Autodesk.Revit.Geometry

''' <summary>
''' A short sample that shows how to read an analytical model 3D object from an inplace family
''' </summary>
''' <remarks></remarks>
Public Class Command
    Implements IExternalCommand

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
    Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result Implements Autodesk.Revit.IExternalCommand.Execute
        Dim doc As Autodesk.Revit.Document = commandData.Application.ActiveDocument

        If 0 = doc.Selection.Elements.Size Then
            message = "Please selected some in-place family instance with AnalyticalMode."
            Return IExternalCommand.Result.Cancelled
        End If

        ' iterate through the selection picking out family instances that have
        ' a 3D analytical model
        Dim element As Autodesk.Revit.Element
        For Each element In doc.Selection.Elements

            If TypeOf element Is Autodesk.Revit.Elements.FamilyInstance Then
                Dim familyInstance As Autodesk.Revit.Elements.FamilyInstance
                familyInstance = element

                Dim analyticalModel As Autodesk.Revit.Structural.AnalyticalModel
                analyticalModel = familyInstance.AnalyticalModel

                If Not (analyticalModel Is Nothing) Then

                    If TypeOf analyticalModel Is Autodesk.Revit.Structural.AnalyticalModel3D Then

                        ' Output the family instance information and the
                        ' curves of the analytical model.
                        DumpFamilyInstance(familyInstance)
                        DumpAnalyticalModel3D(analyticalModel)
                    Else
                        Debug.Print("we should select analytical model 3D family instance, but this familyInstance.AnalyticalModel type is " + analyticalModel.GetType().Name)
                        Continue For
                    End If

                End If

            End If

        Next

        Return Autodesk.Revit.IExternalCommand.Result.Succeeded
    End Function

    Sub DumpFamilyInstance(ByVal familyInstance As Autodesk.Revit.Elements.FamilyInstance)

        ' dump the names of the family, symbol and instance. Since the cross
        ' section is not available it is expected that these will return
        ' something meaningful
        Debug.Print("Family Name : " & familyInstance.Symbol.Family.Name)
        Debug.Print("Family Symbol Name : " & familyInstance.symbol.Name)
        Debug.Print("Family Instance Name : " & familyInstance.Name)
    End Sub

    Sub DumpAnalyticalModel3D(ByVal analyticalModel As Autodesk.Revit.Structural.AnalyticalModel3D)

        Dim curve As Autodesk.Revit.Geometry.Curve
        Dim counter As Integer
        counter = 1

        ' the 3D analytical model has a curves property that reports all the
        ' analytical model curves within the in place family instance
        For Each curve In analyticalModel.Curves

            Debug.Print("Curve " & counter)

            ' use the tesselate method to fragment all types of curves
            ' including lines and arcs etc.
            Dim points As Autodesk.Revit.Geometry.XYZArray
            points = curve.Tessellate()

            ' Since XYZ is a structure, Variant must be using in Visual Basic 6
            ' but in other languages XYZ can be used directly
            Dim point As Object
            For Each point In points
                Debug.Print(point.x, point.y, point.Z)
            Next

            counter = counter + 1
        Next
    End Sub
End Class
