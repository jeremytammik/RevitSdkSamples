//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    public enum VisibleType
    {
        VT_ViewOnly,
        VT_SheetOnly,
        VT_BothViewAndSheet,
        VT_None
    }

    public interface ISettingNameOperation
    {
        string SettingName
        {
            get;
            set;
        }

        string Prefix
        {
            get;
        }

        int SettingCount
        {
            get;
        }

        bool Rename(string name);
        bool SaveAs(string newName);
    }

    /// <summary>
    /// Define some config data which is useful in this sample.
    /// </summary>
    public static class ConstData
    {
        /// <summary>
        /// The const string data which is used as the name 
        /// for InSessionPrintSetting and InSessionViewSheetSet data
        /// </summary>
        public const string InSessionName = "<In-Session>";
    }

    /// <summary>
    /// Exposes the View/Sheet Set interfaces just like 
    /// the View/Sheet Set Dialog (File->Print...; selected views/sheets->Select...) in UI.
    /// </summary>
    public class ViewSheets : ISettingNameOperation
    {
        Document m_doc;
        ViewSheetSetting m_viewSheetSetting;

        public ViewSheets(Document doc)
        {
            m_doc = doc;
            m_viewSheetSetting = doc.PrintManager.ViewSheetSetting;
        }

        public string SettingName
        {
            get
            {
                IViewSheetSet theSet = m_viewSheetSetting.CurrentViewSheetSet;
                return (theSet is ViewSheetSet) ?
                    (theSet as ViewSheetSet).Name : ConstData.InSessionName;
            }
            set
            {
                if (value == ConstData.InSessionName)
                {
                    m_viewSheetSetting.CurrentViewSheetSet = m_viewSheetSetting.InSession;
                    return;
                }
                FilteredElementCollector filteredElementCollector = new FilteredElementCollector(m_doc);
                filteredElementCollector.OfClass(typeof(ViewSheetSet));
                IEnumerable<ViewSheetSet> viewSheetSets = filteredElementCollector.Cast<ViewSheetSet>().Where<ViewSheetSet>(viewSheetSet => viewSheetSet.Name.Equals(value as string));
                if (viewSheetSets.Count<ViewSheetSet>() > 0)
                {
                    m_viewSheetSetting.CurrentViewSheetSet = viewSheetSets.First<ViewSheetSet>();
                }
            }
        }

        public string Prefix
        {
            get
            {
                return "Set ";
            }
        }

        public int SettingCount
        {
            get
            {
                return (new FilteredElementCollector(m_doc)).OfClass(typeof(ViewSheetSet)).ToElementIds().Count;
            }
        }

        public bool SaveAs(string newName)
        {
            try
            {
                return m_viewSheetSetting.SaveAs(newName);
            }
            catch (Exception ex)
            {
                PrintMgr.MyMessageBox(ex.Message);
                return false;
            }
        }

        public bool Rename(string name)
        {
            try
            {
                return m_viewSheetSetting.Rename(name);
            }
            catch (Exception ex)
            {
                PrintMgr.MyMessageBox(ex.Message);
                return false;
            }
        }

        public List<string> ViewSheetSetNames
        {
            get
            {
                List<string> names = new List<string>();
                FilteredElementCollector filteredElementCollector = new FilteredElementCollector(m_doc);
                filteredElementCollector.OfClass(typeof(ViewSheetSet));
                foreach (Element element in filteredElementCollector)
                {
                    ViewSheetSet viewSheetSet = element as ViewSheetSet;
                    names.Add(viewSheetSet.Name);
                }
                names.Add(ConstData.InSessionName);

                return names;
            }
        }

        public bool Save()
        {
            try
            {
                return m_viewSheetSetting.Save();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Revert()
        {
            try
            {
                m_viewSheetSetting.Revert();
            }
            catch (Exception ex)
            {
                PrintMgr.MyMessageBox(ex.Message);
            }
        }

        public bool Delete()
        {
            try
            {
                return m_viewSheetSetting.Delete();
            }
            catch (Exception ex)
            {
                PrintMgr.MyMessageBox(ex.Message);
                return false;
            }

        }

        public List<Autodesk.Revit.DB.View> AvailableViewSheetSet(VisibleType visibleType)
        {
            if (visibleType == VisibleType.VT_None)
                return null;

            List<Autodesk.Revit.DB.View> views = new List<Autodesk.Revit.DB.View>();
            foreach (Autodesk.Revit.DB.View view in m_viewSheetSetting.AvailableViews)
            {
                if (view.ViewType == Autodesk.Revit.DB.ViewType.DrawingSheet
                    && visibleType == VisibleType.VT_ViewOnly)
                {
                    continue;   // filter out sheets.
                }
                if (view.ViewType != Autodesk.Revit.DB.ViewType.DrawingSheet
                    && visibleType == VisibleType.VT_SheetOnly)
                {
                    continue;   // filter out views.
                }

                views.Add(view);
            }

            return views;
        }

        public bool IsSelected(string viewName)
        {
            foreach (Autodesk.Revit.DB.View view in m_viewSheetSetting.CurrentViewSheetSet.Views)
            {
                if (viewName.Equals(view.ViewType.ToString() + ": " + view.ViewName))
                {
                    return true;
                }
            }

            return false;
        }

        public void ChangeCurrentViewSheetSet(List<string> names)
        {
            ViewSet selectedViews = new ViewSet();

            if (null != names && 0 < names.Count)
            {
                foreach (Autodesk.Revit.DB.View view in m_viewSheetSetting.AvailableViews)
                {
                    if (names.Contains(view.ViewType.ToString() + ": " + view.ViewName))
                    {
                        selectedViews.Insert(view);
                    }
                }
            }

            IViewSheetSet viewSheetSet = m_viewSheetSetting.CurrentViewSheetSet;
            viewSheetSet.Views = selectedViews;
            Save();

        }

    }
}
