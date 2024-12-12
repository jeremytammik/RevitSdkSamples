#Region "Copyright"
''
'' (C) Copyright 2010 by Autodesk, Inc.
''
'' Permission to use, copy, modify, and distribute this software in
'' object code form for any purpose and without fee is hereby granted,
'' provided that the above copyright notice appears in all copies and
'' that both that copyright notice and the limited warranty and
'' restricted rights notice below appear in all supporting
'' documentation.
''
'' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
'' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
'' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
'' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
'' UNINTERRUPTED OR ERROR FREE.
''
'' Use, duplication, or disclosure by the U.S. Government is subject to
'' restrictions set forth in FAR 52.227-19 (Commercial Computer
'' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
'' (Rights in Technical Data and Computer Software), as applicable.
''
'' Written by M.Harada 
''
#End Region

#Region "Imports"
'' Import the following name spaces in the project properties/references. 
'' Note: VB.NET has a slighly different way of recognizing name spaces than C#. 
'' if you explicitely set them in each .vb file, you will need to specify full name spaces. 

'Imports System
'Imports Autodesk.Revit.DB
'Imports Autodesk.Revit.UI
'Imports Autodesk.Revit.ApplicationServices  '' Application class
'Imports Autodesk.Revit.Attributes '' specific this if you want to save typing for attributes. e.g., 
'Imports Autodesk.Revit.UI.Selection '' for selection 

#End Region

''  Task Dialog 
'' 
''  cf. Developer Guide, Section 3.9 Revit-style Task Dialogs (pp55) 
''                       Appexdix G. API User Interface Guidelines (pp381), Task Dialog (pp404) 
''  
'' 
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class UITaskDialog
    Implements IExternalCommand

    ''  member variables 
    Dim _rvtUIApp As UIApplication
    Dim _rvtUIDoc As UIDocument
    'Dim _rvtApp As Application
    'Dim _rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        ''  Get the access to the top most objects. (we may not use them all in this specific lab.) 
        _rvtUIApp = commandData.Application
        _rvtUIDoc = _rvtUIApp.ActiveUIDocument
        '_rvtApp = _rvtUIApp.Application
        '_rvtDoc = _rvtUIDoc.Document

        ''  (1) static TaskDialog.Show() 
        ''  we have been using this already. let's see what else we can do with it.
        '' 
        'ShowTaskDialogStatic()

        ''  (2) use an instance of TaskDialog
        ''  this way has more option to customize 
        ''  let's see what we can do. 
        ''
        ShowTaskDialogInstance(True)

        Return Result.Succeeded

    End Function

    ''  Task Dialog static sampler 
    ''  there are three overloads for static Show(). 
    ''
    Sub ShowTaskDialogStatic()

        ''  (1) simplest of all. title and main instruction. has default [Close] button at lower right corner. 
        TaskDialog.Show("Task Dialog Static 1", "Main message")

        ''  (2) this version accepts command buttons in addition to above. 
        ''  XOr is a bitwise operator Or. 
        ''  Here we add [Yes] [No] [Cancel} 
        '' 
        Dim res2 As TaskDialogResult
        res2 = TaskDialog.Show("Task Dialog Static 2", "Main message", _
                        (TaskDialogCommonButtons.Yes Xor TaskDialogCommonButtons.No Xor TaskDialogCommonButtons.Cancel))

        ''  what did the user pressed? 
        TaskDialog.Show("Revit UI Labs", "You pressed: " + res2.ToString)

        ''  (3) this version accepts default button in addition to above. 
        ''  Here we set [No] as a default (just for testing purposes). 
        '' 
        Dim res3 As TaskDialogResult
        Dim defaultButton As TaskDialogResult = TaskDialogResult.Yes
        res3 = TaskDialog.Show("Task Dialog Static 3", "Main message", _
                        (TaskDialogCommonButtons.Yes Xor TaskDialogCommonButtons.No Xor TaskDialogCommonButtons.Cancel), _
                        TaskDialogResult.No)

        ''  what did the user pressed? 
        TaskDialog.Show("Revit UI Labs", "You pressed: " + res3.ToString)

    End Sub

    ''  Task Dialog - create an instance of task dialog gives you more options. 
    ''  cf. Developer guide, Figure 223 (on pp 405) has a image of all the components visible.
    ''  this function is to visulize what kind of contents you can add with TaskDialog.
    ''  Note: actual interpretation of 
    ''  
    Sub ShowTaskDialogInstance(ByVal stepByStep As Boolean)

        ''  (0) create an instance of task dialog to set more options. 
        Dim myDialog As New TaskDialog("Revit UI Labs - Task Dialog Options")
        If stepByStep Then myDialog.Show()

        ''  (1) set the main area. these appear at the upper portion of the dialog. 
        '' 
        myDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning ''  or TaskDialogIcon.TaskDialogIconNone.   
        If stepByStep Then myDialog.Show()

        myDialog.MainInstruction = "Main instruction: This is Revit UI Lab 3 Task Dialog"
        If stepByStep Then myDialog.Show()

        myDialog.MainContent = "Main content: You can add detailed description here."
        If stepByStep Then myDialog.Show()

        ''  (2) set the bottom area 
        '' 
        myDialog.CommonButtons = TaskDialogCommonButtons.Yes Xor TaskDialogCommonButtons.No Xor TaskDialogCommonButtons.Cancel
        myDialog.DefaultButton = TaskDialogResult.Yes
        If stepByStep Then myDialog.Show()

        myDialog.ExpandedContent = "Expanded content: the visibility of this portion is controled by Show/Hide button."
        If stepByStep Then myDialog.Show()

        myDialog.VerificationText = "Verification: Do not show this message again comes here"
        If stepByStep Then myDialog.Show()

        myDialog.FooterText = "Footer: <a href=""http://www.autodesk.com/developrevit"">Revit Developer Center</a>"
        If stepByStep Then myDialog.Show()

        ''  (4) add command links. you can add up to four links 
        '' 
        myDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Command Link 1", "description 1")
        If stepByStep Then myDialog.Show()
        myDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Command Link 2", "description 2")
        If stepByStep Then myDialog.Show()
        myDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink3, "Command Link 3", "description 3")
        If stepByStep Then myDialog.Show()
        myDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink4, "Command Link 4", "you can add up to four command links")
        'If stepByStep Then myDialog.Show()

        ''  show it. 
        Dim res As TaskDialogResult = myDialog.Show()

        TaskDialog.Show("Revit UI Labs", "The last action was: " + res.ToString)

    End Sub

End Class

''
''  Create House with Dialog added 
'' 
''  show a task dialog and ask the user if he/she wants to create a house interactively or automatically.
'' 

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class CreateHouseDialog
    Implements IExternalCommand

    ''  member variables 
    Dim _rvtUIApp As UIApplication
    Dim _rvtUIDoc As UIDocument
    'Dim _rvtApp As Application
    'Dim _rvtDoc As Document

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        ''  Get the access to the top most objects. (we may not use them all in this specific lab.) 
        _rvtUIApp = commandData.Application
        _rvtUIDoc = _rvtUIApp.ActiveUIDocument
        '_rvtApp = _rvtUIApp.Application
        '_rvtDoc = _rvtUIDoc.Document

        ''  (1) create an instance of task dialog to set more options. 
        '' 
        Dim houseDialog As New TaskDialog("Revit UI Labs - Create House Dialog")
        houseDialog.MainInstruction = "Create a house"
        houseDialog.MainContent = "There are two options to create a house."
        houseDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, _
                "Interactive", "You will pick two corners of rectangular footptint of a house, and choose where you want to add a front door.")
        houseDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, _
                "Automatic", "This is will automatically place a house with a default settings.")
        houseDialog.CommonButtons = TaskDialogCommonButtons.Cancel
        houseDialog.DefaultButton = TaskDialogResult.CommandLink1

        ''  show the dialog tp the user. 
        '' 
        Dim res As TaskDialogResult = houseDialog.Show()

        'TaskDialog.Show("Revit UI Labs", "The last action was: " + res.ToString)

        ''  (2)  pause the result and create a house with the method that use has chosen. 
        '' 
        ''  create a house interactively. 
        If res = TaskDialogResult.CommandLink1 Then
            CreateHouseUI.CreateHouseInteractive(_rvtUIDoc)
            Return Result.Succeeded
        End If

        ''  create a house automatically with the default settings. 
        If res = TaskDialogResult.CommandLink2 Then
            RevitIntroVB.ModelCreation.CreateHouse(_rvtUIDoc.Document)
            Return Result.Succeeded
        End If

        ''  request canceled. 
        If res = TaskDialogResult.Cancel Then
            Return Result.Cancelled
        End If

        Return Result.Succeeded

    End Function
End Class
