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
'Imports Autodesk.Revit.DB.Events '' this is for Lab4 event. 
'Imports Autodesk.Revit.UI.Events '' this is for lab4 event. 

#End Region

''  Event  
'' 
''  cf. Developer Guide, Section 24 Event(pp278) - list of events you can subscribe 
''                       Appexdix G. API User Interface Guidelines (pp381), Task Dialog (pp404) 
''  

''  external application to register/unregister document changed event. 
''  simple reports what has been changed 
''
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class UIEventApp
    Implements IExternalApplication

    ''  flag to indicate if we want to show a message at each object modified events. 
    Public Shared m_showEvent As Boolean = False

    ''  OnShutdown() - called when Revit ends. 
    '' 
    Public Function OnShutdown(ByVal application As UIControlledApplication) As Result _
        Implements IExternalApplication.OnShutdown

        ''  (1) unregister our document changed event hander 
        RemoveHandler application.ControlledApplication.DocumentChanged, AddressOf UILabs_DocumentChanged

        Return Result.Succeeded

    End Function

    ''  OnStartup() - called when Revit starts. 
    '' 
    Public Function OnStartup(ByVal application As UIControlledApplication) As Result _
        Implements IExternalApplication.OnStartup

        ''  (1) resgister our document changed event hander 
        AddHandler application.ControlledApplication.DocumentChanged, AddressOf UILabs_DocumentChanged

        ''  (2) register our dynamic model updater (WindowDoorUpdater class definition below.) 
        ''  we are going to keep doors and windows at the center of the wall. 
        ''
        ''  construct our updater. 
        Dim winDoorUpdater As New WindowDoorUpdater(application.ActiveAddInId) '' ActiveAddInId is from addin menifest.
        ''  register it
        UpdaterRegistry.RegisterUpdater(winDoorUpdater)

        ''  tell which elements we are interested in notified. 
        ''  we want to know when wall changes it's length. 
        '' 
        Dim wallFilter As New ElementClassFilter(GetType(Wall))
        UpdaterRegistry.AddTrigger(winDoorUpdater.GetUpdaterId(), wallFilter, Element.GetChangeTypeGeometry)

        Return Result.Succeeded

    End Function

    ''  This is our event handler.  Simply report the list of element ids which have been changed.  
    ''
    Sub UILabs_DocumentChanged(ByVal sender As Object, ByVal args As DocumentChangedEventArgs)

        If Not m_showEvent Then Return

        ''  you can get the list of ids of element added/changed/modified. 
        Dim rvtdDoc As Document = args.GetDocument
        Dim idsAdded As ICollection(Of ElementId) = args.GetAddedElementIds()
        Dim idsDeleted As ICollection(Of ElementId) = args.GetDeletedElementIds()
        Dim idsModified As ICollection(Of ElementId) = args.GetModifiedElementIds()

        ''  put it in a string to show to the user.
        Dim msg As String = "Added: "
        For Each id As ElementId In idsAdded
            msg += id.IntegerValue.ToString + " "
        Next

        msg += vbCr + "Deleted: "
        For Each id As ElementId In idsDeleted
            msg += id.IntegerValue.ToString + " "
        Next

        msg += vbCr + "Modified: "
        For Each id As ElementId In idsModified
            msg += id.IntegerValue.ToString + " "
        Next

        ''  show a message to a user.
        Dim res As TaskDialogResult
        res = TaskDialog.Show("Revit UI Labs - Event", msg, TaskDialogCommonButtons.Ok Xor TaskDialogCommonButtons.Cancel)

        ''  if the user chooses to cancel, show no more event. 
        If (res = TaskDialogResult.Cancel) Then
            m_showEvent = False
        End If

    End Sub

End Class

''  
''  extrenal command to toggle event message on/off 
''
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class UIEvent
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        If UIEventApp.m_showEvent Then
            UIEventApp.m_showEvent = False
        Else
            UIEventApp.m_showEvent = True
        End If

        Return Result.Succeeded

    End Function

End Class

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class UIEventOn
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        UIEventApp.m_showEvent = True

        Return Result.Succeeded

    End Function

End Class

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class UIEventOff
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        UIEventApp.m_showEvent = False

        Return Result.Succeeded

    End Function

End Class

''========================================================
''  dynamic model update - derive from IUpdater class 
''========================================================

Public Class WindowDoorUpdater
    Implements IUpdater

    ''  member variables
    Dim m_addinId As AddInId = Nothing  ''  this value comes from the addin manifest AddInId. 
    Dim m_updaterId As UpdaterId = Nothing  '' unique id for this updater = addin GUID + GUID for this specific updater.  

    ''  flag to indicate if we want to perform  
    Public Shared m_updateActive As Boolean = False

    ''  constructor 
    ''
    Sub New(ByVal id As AddInId)

        m_addinId = id
        m_updaterId = New UpdaterId(m_addinId, New Guid("EF43510F-38CB-4980-844C-72174A674D56"))

    End Sub

    ''  this is the main function to do the actual job. 
    ''  for this exercise, we assume that we want to keep the door and window always at the center.  
    ''
    Public Sub Execute(ByVal data As UpdaterData) Implements IUpdater.Execute

        If Not m_updateActive Then Return

        Dim rvtDoc As Document = data.GetDocument
        Dim idsModified As ICollection(Of ElementId) = data.GetModifiedElementIds

        For Each id As ElementId In idsModified
            Dim aWall As Wall = rvtDoc.Element(id)
            CenterWindowDoor(rvtDoc, aWall)
        Next

    End Sub

    ''  helper function for Execute.
    ''  checks if there is a door or a window on the given wall. 
    ''  if it does, adjust the location to the center of the wall. 
    ''  for simplify, we assume there is only one door or window. 
    ''  (TBD: or evenly if there are more than one.)
    '' 
    Sub CenterWindowDoor(ByVal rvtDoc As Document, ByVal aWall As Wall)

        ''  find a winow or a door on the wall. 
        Dim elem As FamilyInstance = FindWindowDoorOnWall(rvtDoc, aWall)
        If elem Is Nothing Then Return

        ''  move the element (door or window) to the center of the wall. 

        ''  center of the wall 
        Dim wallLocationCurve As LocationCurve = aWall.Location
        Dim pt1 As XYZ = wallLocationCurve.Curve.EndPoint(0)
        Dim pt2 As XYZ = wallLocationCurve.Curve.EndPoint(1)
        Dim midPt As XYZ = (pt1 + pt2) * 0.5

        ''
        Dim loc As LocationPoint = elem.Location
        'Dim newPoint As Point = rvtDoc.Application.Create.NewPoint(midPt)
        loc.Point = midPt

    End Sub

    ''  helper function
    ''  find a door or window on the given wall. 
    ''  if it does, return it. 
    '' 
    Function FindWindowDoorOnWall(ByVal rvtDoc As Document, ByVal aWall As Wall) As FamilyInstance

        ''  collect the list of windows and doors
        ''  no object relation graph. so going hard way.  
        ''  list all the door instances 
        Dim windowDoorCollector = New FilteredElementCollector(rvtDoc)
        windowDoorCollector.OfClass(GetType(FamilyInstance))

        Dim windowFilter As New ElementCategoryFilter(BuiltInCategory.OST_Windows)
        Dim doorFilter As New ElementCategoryFilter(BuiltInCategory.OST_Doors)
        Dim windowDoorFilter As New LogicalOrFilter(windowFilter, doorFilter)

        windowDoorCollector.WherePasses(windowDoorFilter)
        Dim windowDoorList As IList(Of Element) = windowDoorCollector.ToElements

        ''  check to see if the door or window is on the wall we got. 
        For Each elem As FamilyInstance In windowDoorList
            If elem.Host.Id.Equals(aWall.Id) Then
                Return elem
            End If
        Next

        ''  if you come here, you did not find window or door on the given wall. 
        Return Nothing

    End Function

    ''  this will be shown when the updated in not loaded. 
    Public Function GetAdditionalInformation() As String Implements IUpdater.GetAdditionalInformation

        Return "Door/Window updater: keeps doors and windows at the center of walls."

    End Function

    ''  specify the order of execusing updates. 
    Public Function GetChangePriority() As ChangePriority Implements IUpdater.GetChangePriority

        Return ChangePriority.DoorsOpeningsWindows

    End Function

    ''  return updater id. 
    Public Function GetUpdaterId() As UpdaterId Implements IUpdater.GetUpdaterId

        Return m_updaterId

    End Function

    ''  user friendly name of the updater 
    Public Function GetUpdaterName() As String Implements IUpdater.GetUpdaterName

        Return "Window/Door Updater"

    End Function

End Class

''  
''  extrenal command to toggle windowDoor updater on/off 
''
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class UIDynamicModelUpdate
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        If WindowDoorUpdater.m_updateActive Then
            WindowDoorUpdater.m_updateActive = False
        Else
            WindowDoorUpdater.m_updateActive = True
        End If

        Return Result.Succeeded

    End Function

End Class

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class UIDynamicModelUpdateOn
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        WindowDoorUpdater.m_updateActive = True

        Return Result.Succeeded

    End Function

End Class

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class UIDynamicModelUpdateOff
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        WindowDoorUpdater.m_updateActive = False

        Return Result.Succeeded

    End Function

End Class

