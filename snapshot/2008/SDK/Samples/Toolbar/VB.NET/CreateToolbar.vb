'
' (C) Copyright 2003-2007 by Autodesk, Inc.
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
Imports System.Collections.Generic
Imports System.Text

Imports Autodesk.Revit

Namespace Revit.SDK.Samples.Toolbar.VB.NET
    Public Class CreateToolbar
        Implements Autodesk.Revit.IExternalApplication

        ''' <summary>
        ''' Implement this method to implement the external application which should be called when 
        ''' Revit is about to exit,Any documents must have been closed before this method is called.
        ''' </summary>
        ''' <param name="application">An object that is passed to the external application 
        ''' which contains the controlled application.</param>
        ''' <returns>Return the status of the external application. 
        ''' A result of Succeeded means that the external application successfully shut down. 
        ''' Cancelled can be used to signify that the user cancelled the external operation at some point.
        ''' If false is returned then Revit should inform the user that the external application 
        ''' failed to load and the release the internal reference.</returns>
        ''' <remarks></remarks>
        Public Function OnShutdown(ByVal application As Autodesk.Revit.ControlledApplication) As Autodesk.Revit.IExternalApplication.Result Implements Autodesk.Revit.IExternalApplication.OnShutdown

            Return Autodesk.Revit.IExternalApplication.Result.Succeeded

        End Function

        ''' <summary>
        ''' Implement this method to implement the external application which should be called when 
        ''' Revit is about to exit,Any documents must have been closed before this method is called.
        ''' </summary>
        ''' <param name="application">An object that is passed to the external application 
        ''' which contains the controlled application.</param>
        ''' <returns>Return the status of the external application. 
        ''' A result of Succeeded means that the external application successfully started. 
        ''' Cancelled can be used to signify that the user cancelled the external operation at some point.
        ''' If false is returned then Revit should inform the user that the external application 
        ''' failed to load and the release the internal reference.</returns>
        ''' <remarks></remarks>
        Public Function OnStartup(ByVal application As Autodesk.Revit.ControlledApplication) As _
        Autodesk.Revit.IExternalApplication.Result Implements Autodesk.Revit.IExternalApplication.OnStartup

            Try
                'application path
                Dim path As String = Me.GetType().Assembly.Location

                'an external command called bu toolbar item
                Dim dllPath As String = path.Replace("Toolbar.dll", "Commands.dll")

                'image of toolbar
                Dim imagePath As String = path.Replace("Toolbar.dll", "Toolbar.bmp")

                'create a custom tool bar with three buttons
                Dim toolBar As Autodesk.Revit.Toolbar = application.CreateToolbar()
                toolBar.Name = "custom toolbar"
                toolBar.Image = imagePath

                'Add toolbar item by using menu item, 
                'The toolbar item will call the external command(Commands.dll) which will create a wall in project
                Dim menuItem As MenuItem = application.CreateTopMenu("custom menu")
                Dim menuItem1 As MenuItem = menuItem.Append(menuItem.MenuType.BasicMenu, "CreateWall", dllPath, "Revit.SDK.Samples.Toolbar.VB.NET.CreateWall")

                'Add this menu item to create a new toolbar item
                Dim item1 As ToolbarItem = toolBar.AddItem(menuItem1)
                item1.ItemText = "CreateWall"
                item1.ItemType = ToolbarItem.ToolbarItemType.BtnRText 'the item is a button with rich text
                item1.StatusbarTip = "CreateWall"
                item1.ToolTip = "CreateWall"

                'Add the second button will delete all walls in current project. 
                'Use Toolbar.AddItem(String, String) to add.
                Dim item2 As ToolbarItem = toolBar.AddItem(dllPath, "Revit.SDK.Samples.Toolbar.VB.NET.DeleteWalls")
                item2.ItemText = "DeleteWalls"
                item2.ItemType = ToolbarItem.ToolbarItemType.BtnRText 'button with rich text
                item2.StatusbarTip = "DeleteWalls"
                item2.ToolTip = "DeleteWalls"

                'Add a separator between the second button and the third button 
                Dim item3 As ToolbarItem = toolBar.AddItem("spe", "spe")
                item3.ItemType = ToolbarItem.ToolbarItemType.BtnSeparator

                'The button which pops up a dialog box to show some information about Custom Toolbar; 
                Dim item4 As ToolbarItem = toolBar.AddItem(dllPath, "Revit.SDK.Samples.Toolbar.VB.NET.AboutAPIToolbar")
                item4.ItemType = ToolbarItem.ToolbarItemType.BtnStd 'Tstandard item, image only.
                item4.StatusbarTip = "Help of Customer Toolbar"
                item4.ToolTip = "Help of Customer Toolbar"


            Catch ex As Exception
                MsgBox("Failed")
                Return IExternalApplication.Result.Failed
            End Try
            Return IExternalApplication.Result.Succeeded
        End Function
    End Class
End Namespace
