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
'Imports Autodesk.Revit.Attributes '' specify this if you want to save typing for attributes. 

#End Region

#Region "Description"
''  Revit Intro Lab - 1 
''
''  In this lab, you will learn how to "hook" your add-on program to Revit. 
''  This command defines a minimum external command.
''  You will learn: 
''  
''  Explain about addin manifest. How to create GUID. 
''  Hello World in VB.NET is form page 360 of Developer Guide. 
'' 
#End Region

''  Hello World #1 - A minimum Revit external command. 
''
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)> _
<Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)> _
Public Class HelloWorld
    Implements IExternalCommand

    Public Function Execute( _
            ByVal commandData As Autodesk.Revit.UI.ExternalCommandData, _
            ByRef message As String, _
            ByVal elements As Autodesk.Revit.DB.ElementSet) _
            As Autodesk.Revit.UI.Result _
            Implements Autodesk.Revit.UI.IExternalCommand.Execute

        Autodesk.Revit.UI.TaskDialog.Show("My Dialog Title", "Hello World!")
        Return Result.Succeeded

    End Function

End Class

''  Hello World #2 - simplified without full namespace.   
''
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class HelloWorldSimple
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        TaskDialog.Show("My Dialog Title", "Hello World Simple!")
        Return Result.Succeeded

    End Function

End Class

''  Hello World #3 - minimum external application 
''  diference: IExternalApplication instead of IExternalCommand. in addin manifest. 
''  Use addin type "Application", use <Name/> instead of <Text/>. 
''

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class HelloWorldApp
    Implements IExternalApplication

    ''  OnShutdown() - called when Revit ends. 
    '' 
    Public Function OnShutdown(ByVal application As Autodesk.Revit.UI.UIControlledApplication) _
    As Autodesk.Revit.UI.Result _
    Implements Autodesk.Revit.UI.IExternalApplication.OnShutdown
        Return Result.Succeeded
    End Function

    ''  OnStartup() - called when Revit starts. 
    '' 
    Public Function OnStartup(ByVal application As Autodesk.Revit.UI.UIControlledApplication) _
    As Autodesk.Revit.UI.Result _
    Implements Autodesk.Revit.UI.IExternalApplication.OnStartup

        TaskDialog.Show("My Dialog Title", "Hello World from App!")
        Return Result.Succeeded

    End Function
End Class

''  Command Arguments
''  Take a look at the command arguments. commandData is the top most
''  object and the entry point to the Revit model. 
''
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class CommandData
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        ''  The first argument, commandData, is the top most object model.
        ''  You will get the necessary information from commandData. 
        ''  To see what's in there, print out a few data accessed from commandData 
        '' 
        ''  Exercise: Place a break point at commandData and drill down the data. 
        '' 
        Dim versionName As String = _
            commandData.Application.Application.VersionName
        Dim documentTitle As String = _
            commandData.Application.ActiveUIDocument.Document.Title
        TaskDialog.Show("Revit Intro Lab", "Version Name = " + versionName _
                        + vbCr + "Document Title = " + documentTitle)

        ''  print out a list of wall types available in the current rvt project. 
        Dim wallTypes As WallTypeSet = _
            commandData.Application.ActiveUIDocument.Document.WallTypes
        Dim s As String = ""
        For Each wType As WallType In wallTypes
            s = s + wType.Name + vbCr
        Next

        ''  show the result.
        TaskDialog.Show("Revit Intro Lab", "Wall Types: " + vbCr + vbCr + s)

        ''  2nd and 3rd arguments are when the command fails.  
        ''  2nd - set a message to the user.   
        ''  3rd - set elements to highlight. 

        Return Result.Succeeded

    End Function

End Class
