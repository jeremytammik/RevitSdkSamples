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
#End Region

#Region "Imports"
'' Import the following name spaces in the project properties/references. 
'' Note: VB.NET has a slighly different way of recognizing name spaces than C#. 
'' if you explicitely set them in each .vb file, you will need to specify full name spaces. 

'Imports System.Linq  
'Imports System.Windows.Media.Imaging  '' for bitmap images. you will need to reference WindowsBase and PresentationCore  
'Imports Autodesk.Revit
'Imports Autodesk.Revit.DB
'Imports Autodesk.Revit.UI
'Imports Autodesk.Revit.ApplicationServices
'Imports Autodesk.Revit.Attributes '' specific this if you want to save typing for attributes. e.g.,  
Imports FamilyLabsVB

#End Region

#Region "Description"
'' 
''  Family API Labs - an external application to set up ribbon UI for this lab. 
'' 
#End Region

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class App
    Implements IExternalApplication

    ''  member variables/constant for this class. 
    Const m_imageFolder As String = "D:\AEC DevCamp 2010\Family API\FamilyLabsVB\Images\"

    Public Function OnShutdown(ByVal application As UIControlledApplication) As Result _
        Implements IExternalApplication.OnShutdown

        Return Result.Succeeded

    End Function

    Public Function OnStartup(ByVal application As UIControlledApplication) As Result _
        Implements IExternalApplication.OnStartup

        ''  add a ribbon UI for this lab 
        AddRibbon(application)

        Return Result.Succeeded

    End Function

    ''  Add a ribbon UI for this lab. It will be convenient to access to our commands. 
    ''  
    Sub AddRibbon(ByVal app As UIControlledApplication)

        ''  create a ribbon panel for Family Labs  
        Dim panel As RibbonPanel = app.CreateRibbonPanel("Family API")

        ''  ge the location of this dll
        Dim assembly As String = [GetType]().Assembly.Location

        ''  push buttons for regacy labs
        Dim pushButtonData1 As New PushButtonData("FamilyLab1", "1 Rectangle", assembly, "FamilyLabsVB.RvtCmd_FamilyCreateColumnRectangle")
        pushButtonData1.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))

        Dim pushButtonData2 As New PushButtonData("FamilyLab2", "2 L-Shape", assembly, "FamilyLabsVB.RvtCmd_FamilyCreateColumnLShape")
        pushButtonData2.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))

        Dim pushButtonData3 As New PushButtonData("FamilyLab3", "3 FormulaMaterial", assembly, "FamilyLabsVB.RvtCmd_FamilyCreateColumnFormulaMaterial")
        pushButtonData3.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))

        Dim pushButtonData4 As New PushButtonData("FamilyLab4", "4 Visibility", assembly, "FamilyLabsVB.RvtCmd_FamilyCreateColumnVisibility")
        pushButtonData4.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))

        ''  push buttons for a new column 
        Dim pushButtonData6 As New PushButtonData("Column", "Create Column", assembly, "FamilyLabsVB.RvtCmd_CreateColumn")
        pushButtonData6.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))

        ''  make three sets of pull down 
        Dim pulldownBtnData1 As New PulldownButtonData("FamilyLabsPulldownButton1", "Labs")
        Dim pulldownBtnData3 As New PulldownButtonData("FamilyLabsPulldownButton3", "Column")

        ''  create three item stack. 
        Dim stackedItems As IList(Of RibbonItem) = panel.AddStackedItems(pulldownBtnData1, pulldownBtnData3)
        Dim pulldownBtn1 As PulldownButton = stackedItems(0)
        Dim pulldownBtn2 As PulldownButton = stackedItems(1)

        pulldownBtn1.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))
        pulldownBtn2.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))
 
        ''  add each sub items 
        Dim button1 As PushButton = pulldownBtn1.AddPushButton(pushButtonData1)
        Dim button2 As PushButton = pulldownBtn1.AddPushButton(pushButtonData2)
        Dim button3 As PushButton = pulldownBtn1.AddPushButton(pushButtonData3)
        Dim button4 As PushButton = pulldownBtn1.AddPushButton(pushButtonData4)
        Dim button7 As PushButton = pulldownBtn2.AddPushButton(pushButtonData6)

        ''  note: we need to set the image later.  if we do in button bata, it won't show in the Ribbon. 
        button1.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))
        button2.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))
        button3.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))
        button4.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))
        button7.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))
    End Sub

End Class
