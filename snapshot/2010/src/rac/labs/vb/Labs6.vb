#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2007-2009 by Autodesk, Inc.
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
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports WinForms = System.Windows.Forms
Imports System.Windows.Media.Imaging ' for ribbon, requires references to PresentationCore and WindowsBase .NET assemblies
Imports Autodesk.Revit
Imports Autodesk.Revit.Events
Imports System.Diagnostics
#End Region

Namespace Labs

#Region "Lab6_1_HelloWorldExternalApplication"
    ''' <summary>
    ''' A minimal external application saying hello.
    ''' Explain the concept of an external application and its minimal interface.
    ''' Explore how to add an external application to Revit.ini.
    ''' </summary>
    Public Class Lab6_1_HelloWorldExternalApplication
        Implements IExternalApplication

        ''' <summary>
        ''' This method will run automatically when Revit start up.
        ''' It shows a message box. Initiative task can be done in this function.
        ''' </summary>
        Public Function OnStartup(ByVal a As ControlledApplication) _
            As IExternalApplication.Result _
            Implements IExternalApplication.OnStartup

            MsgBox("Hello World from an external application in VB.")
            Return IExternalApplication.Result.Succeeded

        End Function

        Public Function OnShutdown(ByVal a As ControlledApplication) _
            As IExternalApplication.Result _
            Implements IExternalApplication.OnShutdown

            Return IExternalApplication.Result.Succeeded

        End Function

    End Class
#End Region

#Region "Lab6_2_Ribbon"


    ' add various controls to the Ribbon
    Public Class Lab6_2_Ribbon
        Implements IExternalApplication

        Public Function OnStartup(ByVal a As ControlledApplication) _
            As IExternalApplication.Result _
            Implements IExternalApplication.OnStartup

            Try
                CreateRibbonItems(a)
            Catch ex As Exception
                MsgBox(ex.Message)
                Return IExternalApplication.Result.Failed
            End Try

            Return IExternalApplication.Result.Succeeded
        End Function

        Public Function OnShutdown(ByVal a As ControlledApplication) _
            As IExternalApplication.Result _
            Implements IExternalApplication.OnShutdown
            ' we have nothing to do here
            Return IExternalApplication.Result.Succeeded
        End Function

        Public Sub CreateRibbonItems(ByVal a As ControlledApplication)
            ' get the path of our dll
            Dim addInPath As String = GetType(Lab6_2_Ribbon).Assembly.Location
            Dim imgDir As String = Path.Combine(Path.GetDirectoryName(addInPath), "img")

            Dim panelName As String = "Lab 6 Panel"

            Dim cmd1 As String = "Labs.Lab1_1_HelloWorld"
            Dim name1 As String = "HelloWorld"
            Dim text1 As String = "Hello World"
            Dim tooltip1 As String = "Run Lab1_1_HelloWorld command"
            Dim img1 As String = "ImgHelloWorld.png"
            Dim img31 As String = "ImgHelloWorldSmall.png"

            Dim cmd2 As String = "Labs.Lab1_2_CommandArguments"
            Dim name2 As String = "CommandArguments"
            Dim text2 As String = "Command Arguments"
            Dim tooltip2 As String = "Run Lab1_2_CommandArguments command"
            Dim img2 As String = "ImgCommandArguments.png"
            Dim img32 As String = "ImgCommandArgumentsSmall.png"

            Dim name3 As String = "Lab1Commands"
            Dim text3 As String = "Lab 1 Commands"
            Dim tooltip3 As String = "Run a Lab 1 command"
            Dim img33 As String = "ImgCommandSmall.png"

            ' create a Ribbon Panel  
            Dim panel As RibbonPanel = a.CreateRibbonPanel(panelName)

            ' add a button for Lab1's Hello World command
            Dim pb1 As PushButton = panel.AddPushButton(name1, text1, addInPath, cmd1)
            pb1.ToolTip = tooltip1
            pb1.LargeImage = New BitmapImage(New Uri(Path.Combine(imgDir, img1)))

            ' add one vertical separation line in the panel.
            panel.AddSeparator()

            ' prepare data for creating stackable buttons
            Dim pbd1 As New PushButtonData(name1, text1, addInPath, cmd1)
            pbd1.ToolTip = tooltip1
            pbd1.Image = New BitmapImage(New Uri(Path.Combine(imgDir, img31)))

            Dim pbd2 As New PushButtonData(name2, text2, addInPath, cmd2)
            pbd2.ToolTip = tooltip2
            pbd2.Image = New BitmapImage(New Uri(Path.Combine(imgDir, img32)))

            Dim pbd3 As New PulldownButtonData(name3, text3)
            pbd3.ToolTip = tooltip3
            pbd3.Image = New BitmapImage(New Uri(Path.Combine(imgDir, img33)))

            ' add stackable buttons.
            Dim ribbonItems As List(Of RibbonItem) = panel.AddStackedButtons(pbd1, pbd2, pbd3)

            ' add two push buttons as sub-items of the Lab 1 commands 
            Dim pb3 As PulldownButton = ribbonItems(2)

            Dim pb3_1 As PushButton = pb3.AddItem(text1, addInPath, cmd1)
            pb3_1.ToolTip = tooltip1
            pb3_1.LargeImage = New BitmapImage(New Uri(Path.Combine(imgDir, img1)))

            Dim pb3_2 As PushButton = pb3.AddItem(text2, addInPath, cmd2)
            pb3_2.ToolTip = tooltip2
            pb3_2.LargeImage = New BitmapImage(New Uri(Path.Combine(imgDir, img2)))
        End Sub
    End Class
#End Region

#Region "Lab6_3_PreventSaveEvent"
    ''' <summary>
    ''' This external application subscribes to, handles, 
    ''' and unsubscribe from the document saving event.
    ''' Using new pre-event features in event mechnism,some actions can be held to take place.
    ''' This sample displays a message dialog to let the user decide whether to save changes to the document. 
    ''' Note: The document should be already saved in disk.
    ''' </summary>
    Public Class Lab6_3_PreventSaveEvent
        Implements IExternalApplication

        Public Function OnStartup( _
            ByVal a As ControlledApplication) _
        As IExternalApplication.Result _
        Implements IExternalApplication.OnStartup

            Try
                AddHandler a.DocumentSaving, AddressOf app_eventsHandlerMethod                
            Catch ex As Exception
                MsgBox(ex.Message)
                Return IExternalApplication.Result.Failed
            End Try

            Return IExternalApplication.Result.Succeeded

        End Function

        Public Function OnShutdown( _
            ByVal a As ControlledApplication) _
        As IExternalApplication.Result _
        Implements IExternalApplication.OnShutdown

            RemoveHandler a.DocumentSaving, AddressOf app_eventsHandlerMethod
            Return IExternalApplication.Result.Succeeded

        End Function

        ' Show a message to decide whether to save the document.
        Private Sub app_eventsHandlerMethod( _
            ByVal obj As Object, _
            ByVal args As Autodesk.Revit.Events.DocumentSavingEventArgs)

            If args.Cancellable Then
                ' Ask whether to prevent from saving.
                Dim dr As WinForms.DialogResult = WinForms.MessageBox.Show( _
                    "Saving event handler was triggered." + vbCrLf _
                    + "Using the pre-event mechanism, we can cancel the save." + vbCrLf _
                    + "Continue saving the document?", _
                    "Document Saving Event", _
                    WinForms.MessageBoxButtons.YesNo, _
                    WinForms.MessageBoxIcon.Question)

                args.Cancel = (dr <> WinForms.DialogResult.Yes)
            End If
        End Sub
    End Class
#End Region

#Region "Lab6_4_DismissDialog"
    ''' <summary>
    ''' This external application subscribes to, handles, 
    ''' and unsubscribe from the dialog box showing event.
    ''' Its can dismiss the "Family alreay exists" dialog when
    ''' loading an alreadly loaded family, and overwrite the existing version.
    ''' It dismisses message box by mimicing clicking "Yes" button.
    ''' In addition, it can dismiss element property dialog.
    ''' </summary>
    Public Class Lab6_4_DismissDialog
        Implements IExternalApplication

        Public Function OnShutdown(ByVal application As ControlledApplication) As IExternalApplication.Result _
                    Implements IExternalApplication.OnShutdown

            RemoveHandler application.DialogBoxShowing, AddressOf DismissDialog
            Return IExternalApplication.Result.Succeeded
        End Function

        Public Function OnStartup(ByVal application As ControlledApplication) As IExternalApplication.Result _
            Implements IExternalApplication.OnStartup

            AddHandler application.DialogBoxShowing, AddressOf DismissDialog

            Return IExternalApplication.Result.Succeeded
        End Function

        Public Sub DismissDialog(ByVal sender As Object, ByVal e As Autodesk.Revit.Events.DialogBoxShowingEventArgs)
            Dim te As TaskDialogShowingEventArgs = TryCast(e, TaskDialogShowingEventArgs)
            If te IsNot Nothing Then
                If te.DialogId = "TaskDialog_Family_Already_Exists" Then
                    'In this task dialog, 1001 maps the first button. 
                    'The button is "override the existing version"          
                    Dim iReturn As Integer = 1001
                    'Set OverrideResult argument to 1001 mimic clicking the first button.            
                    e.OverrideResult(iReturn)
                    ' 1002 maps the second button in this dialog.          
                    ' DialogResult.Cancel maps the cancel button
                End If
            Else
                Dim msgArgs As MessageBoxShowingEventArgs = TryCast(e, MessageBoxShowingEventArgs)
                If Nothing IsNot msgArgs Then 'If this is a message box.
                    e.OverrideResult(CInt(Fix(WinForms.DialogResult.Yes)))
                    Debug.Print("Dialog id is" & msgArgs.HelpId & Constants.vbCrLf & "; Message is " & msgArgs.Message)
                Else ' If this is other kind of dialog, for instance, element property dialog.
                    'Use the HelpId to identify the dialog.
                    If e.HelpId = 1002 Then 'Element property dialog's HelpId is 1002
                        e.OverrideResult(CInt(Fix(WinForms.DialogResult.No)))
                        Debug.Print("We just dismissed element property dialog, " & "and set the return value is No")
                    End If
                End If
            End If
        End Sub
    End Class
#End Region

End Namespace
