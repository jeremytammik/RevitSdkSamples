#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2006-2010 by Autodesk, Inc.
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

#Region "Namespaces"
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.Attributes
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.UI.Selection
Imports Microsoft.VisualBasic.Constants
' please help remove these stupid workarounds:
'Imports Element2 = Autodesk.Revit.DB.Element
'Imports LanguageType2 = Autodesk.Revit.ApplicationServices.LanguageType
'Imports Selection2 = Autodesk.Revit.UI.Selection.Selection
#End Region

Namespace Labs

#Region "Lab1_1_HelloWorld"
    ''' <summary>
    ''' Say hello.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="1-1"]/*' />
    ''' </summary>
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="1-1-remarks"]/*' />
    <Transaction(TransactionMode.ReadOnly)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab1_1_HelloWorld
        Implements IExternalCommand

        ''' <summary>
        ''' The one and only method required by the IExternalCommand interface,
        ''' the main entry point for every external command.
        ''' </summary>
        ''' <param name="commandData">Input argument providing access to the Revit application and its documents and their properties.</param>
        ''' <param name="message">Return argument to display a message to the user in case of error if Result is not Succeeded.</param>
        ''' <param name="elements">Return argument to highlight elements on the graphics screen if Result is not Succeeded.</param>
        ''' <returns>Cancelled, Failed or Succeeded Result code.</returns>
        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            '#region 1.1 Display a message using the TaskDialog Show method:
            'LabUtils.InfoMsg("Hello World")
            '#endregion // 1.1 Display a message using the TaskDialog Show method

            Return Result.Failed

        End Function

    End Class
#End Region

#Region "Lab1_2_CommandArguments"
    ''' <summary>
    ''' In this lab, we explore the contents and usage of the Execute
    ''' method's command data input argument and the meaning of the result
    ''' return code and the message and element set return arguments.
    ''' The details are discussed in the developer guide,
    ''' chapter 3.2, External Commands.
    ''' <include file='../doc/labs.xml' path='labs/lab[@name="1-2"]/*' />
    ''' </summary>
    <Transaction(TransactionMode.ReadOnly)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab1_2_CommandArguments
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            '#region 1.2.a. Examine command data input argument:
            '
            ' access application, document, and current view:
            '
            '#endregion // 1.2.a. Examine command data input argument

            '#region 1.2.b. List selection set content:
            '
            ' list the current selection set:
            '



            '#endregion // 1.2.b. List selection set content

            '#region 1.2.c. Populate return arguments:
            '
            ' we pretend that something is wrong with the first element in the selection.
            ' pass a message back to the Revit user and indicate the error result:
            '
                '
                ' we return failed here as well, actually.
                ' as long as the message string and element set are empty,
                ' it makes no difference to the user.
                ' it also aborts the automatic transaction, avoiding marking
                ' the database as dirty.
                '
            '#endregion // 1.2.c. Populate return arguments

        End Function

    End Class
#End Region

End Namespace
