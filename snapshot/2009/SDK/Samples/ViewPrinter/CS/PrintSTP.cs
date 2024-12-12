//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using Autodesk.Revit.Elements;
using Autodesk.Revit.Enums;

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
        private PrintSetup m_printSetup;

        public PrintSTP(PrintSetup printSetup
            , ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_printSetup = printSetup;
        }

        public string PrinterName
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrinterName;
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
                return m_commandData.Application.ActiveDocument.PrintSettings.Size;
            }
        }

        public bool SaveAs(string newName)
        {
            try
            {
                return m_printSetup.SaveAs(newName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Revit Printer");
                return false;
            }            
        }

        public bool Rename(string name)
        {            
            try
            {
                return m_printSetup.Rename(name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Revit Printer");
                return false;
            }    
        }

        public List<string> PrintSettingNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (Element printSetting in m_commandData.Application.ActiveDocument.PrintSettings)
                {
                    names.Add(printSetting.Name);
                }
                return names;
            }
        }

        public string SettingName
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.Name;
            }
            set
            {
                foreach (Element printSetting in m_commandData.Application.ActiveDocument.PrintSettings)
                {
                    if (printSetting.Name.Equals(value))
                    {
                        m_printSetup.CurrentPrintSetting = printSetting as PrintSetting;
                    }
                }
            }
        }

        public List<string> PaperSizes
        {
            get
            {
                List<string> names = new List<string>();
                foreach (PaperSize ps in m_printSetup.PaperSizes)
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
                    return m_printSetup.CurrentPrintSetting.PrintParameters.PaperSize.Name;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                foreach (PaperSize ps in m_printSetup.PaperSizes)
                {
                    if (ps.Name.Equals(value))
                    {
                        m_printSetup.CurrentPrintSetting.PrintParameters.PaperSize = ps;
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
                foreach (PaperSource ps in m_printSetup.PaperSources)
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
                return m_printSetup.CurrentPrintSetting.PrintParameters.PaperSource.Name;
            }
            set
            {
                foreach (PaperSource ps in m_printSetup.PaperSources)
                {
                    if (ps.Name.Equals(value))
                    {
                        m_printSetup.CurrentPrintSetting.PrintParameters.PaperSource = ps;
                        break;
                    }
                }
            }
        }

        public PageOrientationType PageOrientation
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.PageOrientation;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.PageOrientation = value;
            }
        }

        public PaperPlacementType PaperPlacement
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.PaperPlacement;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.PaperPlacement = value;
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
                return m_printSetup.CurrentPrintSetting.PrintParameters.MarginType;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.MarginType = value;
            }
        }

        public double UserDefinedMarginX
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.UserDefinedMarginX;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.UserDefinedMarginX = value;
            }
        }

        public double UserDefinedMarginY
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.UserDefinedMarginY;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.UserDefinedMarginY = value;
            }
        }

        public HiddenLineViewsType HiddenLineViews
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.HiddenLineViews;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.HiddenLineViews = value;
            }
        }

        public int Zoom
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.Zoom;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.Zoom = value;
            }
        }

        public ZoomType ZoomType
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.ZoomType;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.ZoomType = value;
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
                return m_printSetup.CurrentPrintSetting.PrintParameters.RasterQuality;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.RasterQuality = value;
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
                return m_printSetup.CurrentPrintSetting.PrintParameters.ColorDepth;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.ColorDepth = value;
            }
        }

        public bool ViewLinksinBlue
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.ViewLinksinBlue;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.ViewLinksinBlue = value;
            }
        }

        public bool HideScopeBoxes
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.HideScopeBoxes;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.HideScopeBoxes = value;
            }
        }

        public bool HideReforWorkPlanes
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.HideReforWorkPlanes;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.HideReforWorkPlanes = value;
            }
        }

        public bool HideCropBoundaries
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.HideCropBoundaries;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.HideCropBoundaries = value;
            }
        }

        public bool HideUnreferencedViewTages
        {
            get
            {
                return m_printSetup.CurrentPrintSetting.PrintParameters.HideUnreferencedViewTages;
            }
            set
            {
                m_printSetup.CurrentPrintSetting.PrintParameters.HideUnreferencedViewTages = value;
            }
        }

        public bool Save()
        {
            try
            {
                return m_printSetup.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Revit Printer");
                return false;
            }
        }

        public void Revert()
        {
            try
            {
                m_printSetup.Revert();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Revit Printer");
            }
        }

        public bool Delete()
        {
            try
            {
                return m_printSetup.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Revit Printer");
                return false;
            }
        }

        public bool VerifyMarginType(Control controlToEnableOrNot)
        {
            // Enable terms (or):
            // 1. Paper placement is Margins.
            return controlToEnableOrNot.Enabled =
                m_printSetup.CurrentPrintSetting.PrintParameters.PaperPlacement == PaperPlacementType.Margins;
        }

        public bool VerifyUserDefinedMargin(Collection<Control> controlsToEnableOrNot)
        {
            bool enableOrNot =
                m_printSetup.CurrentPrintSetting.PrintParameters.MarginType == MarginType.UserDefined;

            foreach (Control control in controlsToEnableOrNot)
            {
                control.Enabled = enableOrNot;
            }

            return enableOrNot;
        }        
    }
}
