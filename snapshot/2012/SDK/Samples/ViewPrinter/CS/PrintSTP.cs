//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    /// <summary>
    /// Change and save printer setup setting, exposes the print parameters just
    /// like the Print Setup Dialog (File->Print Setup...) in UI such as Printer name,
    /// paper, zoom, options, etc.
    /// </summary>
    public class PrintSTP : ISettingNameOperation
    {
        private ExternalCommandData m_commandData;
        private PrintManager m_printMgr;

        public PrintSTP(PrintManager printMgr
            , ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_printMgr = printMgr;
        }

        public string PrinterName
        {
            get
            {
                return m_printMgr.PrinterName;
            }
        }

        public string Prefix
        {
            get
            {
                return "Default ";
            }
        }

        public int SettingCount
        {
            get
            {
                return m_commandData.Application.ActiveUIDocument.Document.PrintSettings.Size;
            }
        }

        public bool SaveAs(string newName)
        {
            try
            {
                return m_printMgr.PrintSetup.SaveAs(newName);
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
                return m_printMgr.PrintSetup.Rename(name);
            }
            catch (Exception ex)
            {
                PrintMgr.MyMessageBox(ex.Message);
                return false;
            }    
        }

        public List<string> PrintSettingNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (Element printSetting in m_commandData.Application.ActiveUIDocument.Document.PrintSettings)
                {
                    names.Add(printSetting.Name);
                }
                names.Add(ConstData.InSessionName);
                return names;
            }
        }

        public string SettingName
        {
            get
            {
                IPrintSetting setting = m_printMgr.PrintSetup.CurrentPrintSetting;
                return (setting is PrintSetting) ?
                    (setting as PrintSetting).Name : ConstData.InSessionName;
            }
            set
            {
                if (value == ConstData.InSessionName)
                {
                    m_printMgr.PrintSetup.CurrentPrintSetting = m_printMgr.PrintSetup.InSession;
                    return;
                }
                foreach (Element printSetting in m_commandData.Application.ActiveUIDocument.Document.PrintSettings)
                {
                    if (printSetting.Name.Equals(value))
                    {
                        m_printMgr.PrintSetup.CurrentPrintSetting = printSetting as PrintSetting;
                    }
                }
            }
        }

        public List<string> PaperSizes
        {
            get
            {
                List<string> names = new List<string>();
                foreach (PaperSize ps in m_printMgr.PaperSizes)
                {
                    names.Add(ps.Name);
                }
                return names;
            }
        }

        public string PaperSize
        {
            get
            {
                try
                {
                    return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.PaperSize.Name;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                foreach (PaperSize ps in m_printMgr.PaperSizes)
                {
                    if (ps.Name.Equals(value))
                    {
                        m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.PaperSize = ps;
                        break;
                    }
                }
            }
        }

        public List<string> PaperSources
        {
            get
            {
                List<string> names = new List<string>();
                foreach (PaperSource ps in m_printMgr.PaperSources)
                {
                    names.Add(ps.Name);
                }
                return names;
            }
        }

        public string PaperSource
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.PaperSource.Name;
            }
            set
            {
                foreach (PaperSource ps in m_printMgr.PaperSources)
                {
                    if (ps.Name.Equals(value))
                    {
                        m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.PaperSource = ps;
                        break;
                    }
                }
            }
        }

        public PageOrientationType PageOrientation
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.PageOrientation;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.PageOrientation = value;
            }
        }

        public PaperPlacementType PaperPlacement
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.PaperPlacement;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.PaperPlacement = value;
            }
        }

        public Array MarginTypes
        {
            get
            {
                return Enum.GetValues(typeof(MarginType));
            }
        }

        public MarginType SelectedMarginType
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.MarginType;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.MarginType = value;
            }
        }

        public double UserDefinedMarginX
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.UserDefinedMarginX;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.UserDefinedMarginX = value;
            }
        }

        public double UserDefinedMarginY
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.UserDefinedMarginY;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.UserDefinedMarginY = value;
            }
        }

        public HiddenLineViewsType HiddenLineViews
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.HiddenLineViews;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.HiddenLineViews = value;
            }
        }

        public int Zoom
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.Zoom;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.Zoom = value;
            }
        }

        public ZoomType ZoomType
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.ZoomType;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.ZoomType = value;
            }
        }

        public Array RasterQualities
        {
            get
            {
                return Enum.GetValues(typeof(RasterQualityType));
            }
        }

        public RasterQualityType RasterQuality
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.RasterQuality;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.RasterQuality = value;
            }
        }

        public Array Colors
        {
            get
            {
                return Enum.GetValues(typeof(ColorDepthType));
            }
        }

        public ColorDepthType Color
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.ColorDepth;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.ColorDepth = value;
            }
        }

        public bool ViewLinksinBlue
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.ViewLinksinBlue;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.ViewLinksinBlue = value;
            }
        }

        public bool HideScopeBoxes
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.HideScopeBoxes;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.HideScopeBoxes = value;
            }
        }

        public bool HideReforWorkPlanes
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.HideReforWorkPlanes;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.HideReforWorkPlanes = value;
            }
        }

        public bool HideCropBoundaries
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.HideCropBoundaries;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.HideCropBoundaries = value;
            }
        }

        public bool HideUnreferencedViewTages
        {
            get
            {
                return m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.HideUnreferencedViewTages;
            }
            set
            {
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.HideUnreferencedViewTages = value;
            }
        }

        public bool Save()
        {
            try
            {
                return m_printMgr.PrintSetup.Save();
            }
            catch (Exception ex)
            {
                PrintMgr.MyMessageBox(ex.Message);
                return false;
            }
        }

        public void Revert()
        {
            try
            {
                m_printMgr.PrintSetup.Revert();
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
                return m_printMgr.PrintSetup.Delete();
            }
            catch (Exception ex)
            {
                PrintMgr.MyMessageBox(ex.Message);
                return false;
            }
        }

        public bool VerifyMarginType(System.Windows.Forms.Control controlToEnableOrNot)
        {
            // Enable terms (or):
            // 1. Paper placement is Margins.
            return controlToEnableOrNot.Enabled =
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.PaperPlacement == PaperPlacementType.Margins;
        }

        public bool VerifyUserDefinedMargin(Collection<System.Windows.Forms.Control> controlsToEnableOrNot)
        {
            bool enableOrNot =
                m_printMgr.PrintSetup.CurrentPrintSetting.PrintParameters.MarginType == MarginType.UserDefined;

            foreach (System.Windows.Forms.Control control in controlsToEnableOrNot)
            {
                control.Enabled = enableOrNot;
            }

            return enableOrNot;
        }        
    }
}
