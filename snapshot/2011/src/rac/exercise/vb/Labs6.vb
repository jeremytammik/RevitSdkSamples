#Region "Header"
'Revit API .NET Labs

'Copyright (C) 2006-2010 by Autodesk, Inc.

'Permission to use, copy, modify, and distribute this software
'for any purpose and without fee is hereby granted, provided
'that the above copyright notice appears in all copies and
'that both that copyright notice and the limited warranty and
'restricted rights notice below appear in all supporting
'documentation.

'AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
'AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
'MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
'DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
'UNINTERRUPTED OR ERROR FREE.

'Use, duplication, or disclosure by the U.S. Government is subject to
'restrictions set forth in FAR 52.227-19 (Commercial Computer
'Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
'(Rights in Technical Data and Computer Software), as applicable.
#End Region

#Region "Namespaces"
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Reflection
Imports System.Linq
Imports WinForms = System.Windows.Forms
Imports System.Windows.Media.Imaging
Imports Microsoft.VisualBasic.Constants
' for ribbon, requires references to PresentationCore and WindowsBase .NET assemblies
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.Attributes
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.DB.Architecture
Imports Autodesk.Revit.DB.Events
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.UI.Events
#End Region

Namespace Labs

#Region "Lab6_1_HelloWorldExternalApplication"
    ''' <summary>
    ''' A minimal external application saying hello.
    ''' Explain the concept of an external application and its minimal interface.
    ''' Explore how to create a manifest file for an external application.
    ''' </summary>
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab6_1_HelloWorldExternalApplication
        Implements IExternalApplication

        ''' <summary>
        ''' This method will run automatically when Revit start up.
        ''' It shows a message box. Initiative task can be done in this function.
        ''' </summary>
        Public Function OnStartup( _
            ByVal a As UIControlledApplication) _
            As Result _
            Implements IExternalApplication.OnStartup

            LabUtils.InfoMsg("Hello World from an external application in C#.")

            Return Result.Succeeded
        End Function

        Public Function OnShutdown( _
        ByVal a As UIControlledApplication) _
        As Result _
        Implements IExternalApplication.OnShutdown

            Return Result.Succeeded

        End Function

    End Class
#End Region

#Region "Lab6_2_Ribbon"
    ''' <summary>
    ''' Add various controls to the Revit ribbon.
    ''' </summary>
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab6_2_Ribbon
        Implements IExternalApplication

        Public Function OnShutdown( _
            ByVal a As UIControlledApplication) _
            As Result _
            Implements IExternalApplication.OnShutdown

            Return Result.Succeeded
        End Function

        Public Function OnStartup( _
            ByVal a As UIControlledApplication) _
            As Result _
            Implements IExternalApplication.OnStartup

            Try
                CreateRibbonItems(a)
            Catch ex As Exception
                LabUtils.InfoMsg(ex.Message)
                Return Result.Failed
            End Try

            Return Result.Succeeded
        End Function

        Private Sub CreateRibbonItems(ByVal a As UIControlledApplication)
            ' get the path of our dll
            Dim addInPath As String = [GetType]().Assembly.Location
            Dim imgDir As String = Path.Combine(Path.GetDirectoryName(addInPath), "img")

            Const panelName As String = "Lab 6 Panel"

            Const cmd1 As String = "Labs.Lab1_1_HelloWorld"
            Const name1 As String = "HelloWorld"
            Const text1 As String = "Hello World"
            Const tooltip1 As String = "Run Lab1_1_HelloWorld command"
            Const img1 As String = "ImgHelloWorld.png"
            Const img31 As String = "ImgHelloWorldSmall.png"

            Const cmd2 As String = "Labs.Lab1_2_CommandArguments"
            Const name2 As String = "CommandArguments"
            Const text2 As String = "Command Arguments"
            Const tooltip2 As String = "Run Lab1_2_CommandArguments command"
            Const img2 As String = "ImgCommandArguments.png"
            Const img32 As String = "ImgCommandArgumentsSmall.png"

            Const name3 As String = "Lab1Commands"
            Const text3 As String = "Lab 1 Commands"
            Const tooltip3 As String = "Run a Lab 1 command"
            Const img33 As String = "ImgCommandSmall.png"

            ' create a Ribbon Panel

            Dim panel As RibbonPanel = a.CreateRibbonPanel(panelName)

            ' add a button for Lab1's Hello World command

            Dim pbd As New PushButtonData(name1, text1, addInPath, cmd1)
            Dim pb1 As PushButton = TryCast(panel.AddItem(pbd), PushButton)
            pb1.ToolTip = tooltip1
            pb1.LargeImage = New BitmapImage(New Uri(Path.Combine(imgDir, img1)))

            ' add a vertical separation line in the panel
            panel.AddSeparator()

            ' prepare data for creating stackable buttons
            Dim pbd1 As New PushButtonData(name1 + " 2", text1, addInPath, cmd1)
            pbd1.ToolTip = tooltip1
            pbd1.Image = New BitmapImage(New Uri(Path.Combine(imgDir, img31)))

            Dim pbd2 As New PushButtonData(name2, text2, addInPath, cmd2)
            pbd2.ToolTip = tooltip2
            pbd2.Image = New BitmapImage(New Uri(Path.Combine(imgDir, img32)))

            Dim pbd3 As New PulldownButtonData(name3, text3)
            pbd3.ToolTip = tooltip3
            pbd3.Image = New BitmapImage(New Uri(Path.Combine(imgDir, img33)))

            ' add stackable buttons
            Dim ribbonItems As IList(Of RibbonItem) = panel.AddStackedItems(pbd1, pbd2, pbd3)

            ' add two push buttons as sub-items of the Lab 1 commands
            Dim pb3 As PulldownButton = TryCast(ribbonItems(2), PulldownButton)

            pbd = New PushButtonData(name1, text1, addInPath, cmd1)
            Dim pb3_1 As PushButton = pb3.AddPushButton(pbd)
            pb3_1.ToolTip = tooltip1
            pb3_1.LargeImage = New BitmapImage(New Uri(Path.Combine(imgDir, img1)))

            pbd = New PushButtonData(name2, text2, addInPath, cmd2)
            Dim pb3_2 As PushButton = pb3.AddPushButton(pbd)
            pb3_2.ToolTip = tooltip2
            pb3_2.LargeImage = New BitmapImage(New Uri(Path.Combine(imgDir, img2)))
        End Sub
    End Class
#End Region

#Region "Lab6_3_PreventSaveEvent"
    ''' <summary>
    ''' This external application subscribes to, handles,
    ''' and unsubscribes from the document saving event.
    ''' Using the event mechnism pre-event features, some actions can be prevented.
    ''' This sample displays a message dialog to let the user decide whether to save changes to the document.
    ''' Note: The document must have been already saved to disk prior to running this application.
    ''' </summary>
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab6_3_PreventSaveEvent
        Implements IExternalApplication

        Public Function OnShutdown( _
            ByVal a As UIControlledApplication) _
            As Result _
            Implements IExternalApplication.OnShutdown

            ' remove the event subscription:
            RemoveHandler a.ControlledApplication.DocumentSaving, AddressOf a_DocumentSaving

            Return Result.Succeeded
        End Function

        Public Function OnStartup( _
        ByVal a As UIControlledApplication) _
        As Result _
        Implements IExternalApplication.OnStartup

            Try
                ' subscribe to the DocumentSaving event:
                AddHandler a.ControlledApplication.DocumentSaving, AddressOf a_DocumentSaving
            Catch ex As Exception
                LabUtils.InfoMsg(ex.Message)
                Return Result.Failed
            End Try

            Return Result.Succeeded
        End Function

        ' Show a message to decide whether to save the document.
        Private Sub a_DocumentSaving(ByVal obj As Object, ByVal args As DocumentSavingEventArgs)
            ' Ask whether to prevent from saving:
            args.Cancel = args.Cancellable AndAlso _
                LabUtils.QuestionMsg("Saving event handler was triggered." _
                                     + vbCr + vbLf + "Using the pre-event mechanism, we can cancel the save." _
                                     + vbCr + vbLf + "Continue saving the document?")
        End Sub

    End Class
#End Region

#Region "Lab6_4_DismissDialog"
    ''' <summary>
    ''' This external application subscribes to, handles,
    ''' and unsubscribe from the dialog box showing event.
    ''' Its can dismiss the "Family already exists" dialog when
    ''' reloading an already loaded family, and overwrite the existing version.
    ''' It dismisses message box by simulating a click on the "Yes" button.
    ''' In addition, it can dismiss the property dialog.
    ''' </summary>
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab6_4_DismissDialog
        Implements IExternalApplication

        Public Function OnShutdown( _
            ByVal a As UIControlledApplication) _
            As Result _
            Implements IExternalApplication.OnShutdown

            RemoveHandler a.DialogBoxShowing, AddressOf DismissDialog

            Return Result.Succeeded
        End Function

        Public Function OnStartup( _
            ByVal a As UIControlledApplication) _
            As Result _
            Implements IExternalApplication.OnStartup

            AddHandler a.DialogBoxShowing, AddressOf DismissDialog

            Return Result.Succeeded
        End Function

        Private Sub DismissDialog( _
            ByVal sender As Object, _
            ByVal e As DialogBoxShowingEventArgs)

            Dim te As TaskDialogShowingEventArgs = TryCast(e, TaskDialogShowingEventArgs)
            If te IsNot Nothing Then
                If te.DialogId = "TaskDialog_Family_Already_Exists" Then
                    ' In this task dialog, 1001 maps to the first button,
                    ' which is the "override the existing version"
                    Dim iReturn As Integer = 1001

                    ' Set OverrideResult argument to 1001 mimic clicking the first button.

                    ' 1002 maps the second button in this dialog.
                    ' DialogResult.Cancel maps to the cancel button.
                    e.OverrideResult(iReturn)
                End If
            Else
                Dim msgArgs As MessageBoxShowingEventArgs = TryCast(e, MessageBoxShowingEventArgs)
                If msgArgs IsNot Nothing Then
                    ' this is a message box
                    e.OverrideResult(CInt(WinForms.DialogResult.Yes))

                    Debug.Print( _
                        "Dialog id is {0}" + vbCr + vbLf + "Message is {1}", _
                        msgArgs.HelpId, msgArgs.Message)
                Else
                    ' this is some other dialog, for example, element property dialog.
                    'Use the HelpId to identify the dialog.
                    If e.HelpId = 1002 Then
                        ' Element property dialog's HelpId is 1002
                        e.OverrideResult(CInt(WinForms.DialogResult.No))
                        Debug.Print("We just dismissed the element property dialog " _
                                    + "and set the return value to No.")
                    End If
                End If
            End If
        End Sub


    End Class
#End Region

#Region "Lab6_5_RibbonExplorer"
    ''' <summary>
    ''' List the contents of the Revit ribbon in the Visual Studio debug output window.
    ''' </summary>
    <Transaction(TransactionMode.Automatic)> _
    <Regeneration(RegenerationOption.Manual)> _
    Public Class Lab6_5_RibbonExplorer
        Implements IExternalCommand

        Public Function Execute( _
            ByVal commandData As ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As ElementSet) _
            As Result _
            Implements IExternalCommand.Execute

            Dim app As UIApplication = commandData.Application
            Dim panels As List(Of RibbonPanel) = app.GetRibbonPanels()
            For Each panel As RibbonPanel In panels
                Debug.Print(panel.Name)
                Dim items As IList(Of RibbonItem) = panel.GetItems()
                For Each item As RibbonItem In items
                    Dim t As RibbonItemType = item.ItemType
                    If RibbonItemType.PushButton = t Then
                        Dim b As PushButton = TryCast(item, PushButton)
                        Debug.Print(" {0} : {1}", item.ItemText, b.Name)
                    Else
                        Debug.Assert(RibbonItemType.PulldownButton = t, "expected pulldown button")
                        Dim b As PulldownButton = TryCast(item, PulldownButton)
                        Debug.Print(" {0} : {1}", item.ItemText, b.Name)
                        For Each item2 As RibbonItem In b.GetItems()
                            Debug.Assert(RibbonItemType.PushButton = item2.ItemType, "expected push button in pulldown menu")
                            Debug.Print(" {0} : {1}", item2.ItemText, DirectCast(item2, PushButton).Name)
                        Next
                    End If
                Next
            Next
            Return Result.Failed
        End Function
    End Class
#End Region

End Namespace

