#region Copyright
//
// (C) Copyright 2010 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
// Migrated to C# by Saikat Bhattacharya
// 
#endregion // Copyright

#region "Imports"

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes; // specify this if you want to save typing for attributes. e.g.
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI.Events;

#endregion

namespace RevitUILabsCS
{
    // Event 
    // 
    // cf. Developer Guide, Section 24 Event(pp278) - list of events you can subscribe 
    // Appexdix G. API User Interface Guidelines (pp381), Task Dialog (pp404) 
    // 

    // external application to register/unregister document changed event. 
    // simple reports what has been changed 
    // 
    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class UIEventApp : IExternalApplication
    {

        // flag to indicate if we want to show a message at each object modified events. 
        public static bool _showEvent = false;

        // OnShutdown() - called when Revit ends. 
        // 
        public Result OnShutdown(UIControlledApplication application)
        {

            // (1) unregister our document changed event hander 
            application.ControlledApplication.DocumentChanged -= UILabs_DocumentChanged;
            
            return Result.Succeeded;
        }

        // OnStartup() - called when Revit starts. 
        // 
        public Result OnStartup(UIControlledApplication application)
        {

            // (1) resgister our document changed event hander 
            application.ControlledApplication.DocumentChanged += UILabs_DocumentChanged;

            // (2) register our dynamic model updater (WindowDoorUpdater class definition below.) 
            // we are going to keep doors and windows at the center of the wall. 
            // 
            // construct our updater. 
            WindowDoorUpdater winDoorUpdater = new WindowDoorUpdater(application.ActiveAddInId);
            // ActiveAddInId is from addin menifest. 
            // register it 
            UpdaterRegistry.RegisterUpdater(winDoorUpdater);

            // tell which elements we are interested in notified. 
            // we want to know when wall changes it's length. 
            // 
            ElementClassFilter wallFilter = new ElementClassFilter(typeof(Wall));
            UpdaterRegistry.AddTrigger(winDoorUpdater.GetUpdaterId(), wallFilter, Element.GetChangeTypeGeometry());


            return Result.Succeeded;
        }

        // This is our event handler. Simply report the list of element ids which have been changed. 
        // 
        public void UILabs_DocumentChanged(object sender, DocumentChangedEventArgs args)
        {

            if (!_showEvent) return;

            // you can get the list of ids of element added/changed/modified. 
            Document rvtdDoc = args.GetDocument();

            ICollection<ElementId> idsAdded = args.GetAddedElementIds();
            ICollection<ElementId> idsDeleted = args.GetDeletedElementIds();
            ICollection<ElementId> idsModified = args.GetModifiedElementIds();

            // put it in a string to show to the user. 
            string msg = "Added: ";
            foreach (ElementId id in idsAdded)
            {
                msg += id.IntegerValue.ToString() + " ";
            }

            msg += "\nDeleted: ";
            foreach (ElementId id in idsDeleted)
            {
                msg += id.IntegerValue.ToString() + " ";
            }

            msg += "\nModified: ";
            foreach (ElementId id in idsModified)
            {
                msg += id.IntegerValue.ToString() + " ";
            }

            // show a message to a user. 
            TaskDialogResult res = default(TaskDialogResult);
            res = TaskDialog.Show("Revit UI Labs - Event", msg, TaskDialogCommonButtons.Ok ^ TaskDialogCommonButtons.Cancel);

            // if the user chooses to cancel, show no more event. 
            if ((res == TaskDialogResult.Cancel))
            {
                _showEvent = false;

            }
        }

    }

    // 
    // extrenal command to toggle event message on/off 
    // 
    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class UIEvent : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            if (UIEventApp._showEvent)
            {
                UIEventApp._showEvent = false;
            }
            else
            {
                UIEventApp._showEvent = true;
            }


            return Result.Succeeded;
        }

    }

    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class UIEventOn : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIEventApp._showEvent = true;


            return Result.Succeeded;
        }

    }

    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class UIEventOff : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIEventApp._showEvent = false;


            return Result.Succeeded;
        }

    }

    //======================================================== 
    // dynamic model update - derive from IUpdater class 
    //======================================================== 

    public class WindowDoorUpdater : IUpdater
    {

        // member variables 
        AddInId m_addinId = null;
        // this value comes from the addin manifest AddInId. 
        UpdaterId m_updaterId = null;
        // unique id for this updater = addin GUID + GUID for this specific updater. 

        // flag to indicate if we want to perform 
        public static bool m_updateActive = false;

        // constructor 
        // 
        public WindowDoorUpdater(AddInId id)
        {

            m_addinId = id;

            m_updaterId = new UpdaterId(m_addinId, new Guid("EF43510F-38CB-4980-844C-72174A674D56"));
        }

        // this is the main function to do the actual job. 
        // for this exercise, we assume that we want to keep the door and window always at the center. 
        // 
        public void Execute(UpdaterData data)
        {

            if (!m_updateActive) return;

            Document rvtDoc = data.GetDocument();
            ICollection<ElementId> idsModified = data.GetModifiedElementIds();

            foreach (ElementId id in idsModified)
            {
                Wall aWall = rvtDoc.get_Element(id) as Wall;
                CenterWindowDoor(rvtDoc, aWall);

            }
        }

        // helper function for Execute. 
        // checks if there is a door or a window on the given wall. 
        // if it does, adjust the location to the center of the wall. 
        // for simplify, we assume there is only one door or window. 
        // (TBD: or evenly if there are more than one.) 
        // 
        public void CenterWindowDoor(Document rvtDoc, Wall aWall)
        {

            // find a winow or a door on the wall. 
            FamilyInstance elem = FindWindowDoorOnWall(rvtDoc, aWall);
            if (elem == null) return;

            // move the element (door or window) to the center of the wall. 

            // center of the wall 
            LocationCurve wallLocationCurve = aWall.Location as LocationCurve;
            XYZ pt1 = wallLocationCurve.Curve.get_EndPoint(0);
            XYZ pt2 = wallLocationCurve.Curve.get_EndPoint(1);
            XYZ midPt = (pt1 + pt2) * 0.5;

            // 
            LocationPoint loc = elem.Location as LocationPoint;
            if (loc.Point.Z.Equals(0.0))
            {
                loc.Point = new XYZ(midPt.X, midPt.Y, midPt.Z);
            }
            else
            {
                loc.Point = new XYZ(midPt.X, midPt.Y, loc.Point.Z);
            }
        }

        // helper function 
        // find a door or window on the given wall. 
        // if it does, return it. 
        // 
        public FamilyInstance FindWindowDoorOnWall(Document rvtDoc, Wall aWall)
        {

            // collect the list of windows and doors 
            // no object relation graph. so going hard way. 
            // list all the door instances 
            var windowDoorCollector = new FilteredElementCollector(rvtDoc);
            windowDoorCollector.OfClass(typeof(FamilyInstance));

            ElementCategoryFilter windowFilter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
            ElementCategoryFilter doorFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
            LogicalOrFilter windowDoorFilter = new LogicalOrFilter(windowFilter, doorFilter);

            windowDoorCollector.WherePasses(windowDoorFilter);
            IList<Element> windowDoorList = windowDoorCollector.ToElements();

            // check to see if the door or window is on the wall we got. 
            foreach (FamilyInstance elem in windowDoorList)
            {
                if (elem.Host.Id.Equals(aWall.Id))
                {
                    return elem;
                }
            }

            // if you come here, you did not find window or door on the given wall. 

            return null;
        }

        // this will be shown when the updated in not loaded. 
        public string GetAdditionalInformation()
        {


            return "Door/Window updater: keeps doors and windows at the center of walls.";
        }

        // specify the order of execusing updates. 
        public ChangePriority GetChangePriority()
        {


            return ChangePriority.DoorsOpeningsWindows;
        }

        // return updater id. 
        public UpdaterId GetUpdaterId()
        {


            return m_updaterId;
        }

        // user friendly name of the updater 
        public string GetUpdaterName()
        {


            return "Window/Door Updater";
        }

    }

    // 
    // extrenal command to toggle windowDoor updater on/off 
    // 
    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class UIDynamicModelUpdate : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            if (WindowDoorUpdater.m_updateActive)
            {
                WindowDoorUpdater.m_updateActive = false;
            }
            else
            {
                WindowDoorUpdater.m_updateActive = true;
            }


            return Result.Succeeded;
        }

    }

    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class UIDynamicModelUpdateOn : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            WindowDoorUpdater.m_updateActive = true;

            return Result.Succeeded;
        }

    }

    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class UIDynamicModelUpdateOff : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            WindowDoorUpdater.m_updateActive = false;


            return Result.Succeeded;
        }

    } 
    
}
