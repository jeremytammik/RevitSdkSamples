//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using System.Drawing.Printing;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    /// <summary>
    /// Exposes the print interfaces just like the Print Dialog (File->Print...) in UI.
    /// </summary>
    public class PrintMgr
    {
        private ExternalCommandData m_commandData;
        private PrintManager m_printMgr;

        public PrintMgr(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_printMgr = commandData.Application.ActiveUIDocument.Document.PrintManager;
        }

        public List<string> InstalledPrinterNames
        {
            get
            {
                try
                {
                    PrinterSettings.StringCollection printers
                    = PrinterSettings.InstalledPrinters;
                    string[] printerNames = new string[printers.Count];
                    printers.CopyTo(printerNames, 0);

                    List<string> names = new List<string>();
                    foreach (string name in printerNames)
                    {
                        names.Add(name);
                    }

                    return 0 == names.Count ? null : names;
                }
                catch (Exception)
                {
                    return null;// can not get installed printer
                }
            }
        }

        public string PrinterName
        {
            get
            {
                return m_printMgr.PrinterName;
            }
            set
            {
                try
                {
                    m_printMgr.SelectNewPrintDriver(value);
                }
                catch (Exception)
                {
                    // un-available or exceptional printer
                }
            }
        }

        public string PrintSetupName
        {
            get
            {
                IPrintSetting setting = m_printMgr.PrintSetup.CurrentPrintSetting;
                return (setting is PrintSetting) ?
                    (setting as PrintSetting).Name : ConstData.InSessionName;
            }
        }

        public bool IsPrintToFile
        {
            get
            {
                return m_printMgr.PrintToFile;
            }
            set
            {
                m_printMgr.PrintToFile = value;
                m_printMgr.Apply();
            }
        }

        public bool IsCombinedFile
        {
            get
            {
                return m_printMgr.CombinedFile;
            }
            set
            {
                // CombinedFile property cannot be setted to false when the Print Range is Current/Visable!
                m_printMgr.CombinedFile = value;
                m_printMgr.Apply();
            }
        }

        public string PrintToFileName
        {
            get
            {
                return m_printMgr.PrintToFileName;
            }
        }

        public string ChangePrintToFileName()
        {
            using (SaveFileDialog saveDlg = new SaveFileDialog())
            {
                string postfix = null;

                switch (m_printMgr.IsVirtual)
                {
                    case Autodesk.Revit.DB.VirtualPrinterType.AdobePDF:
                        saveDlg.Filter = "pdf files (*.pdf)|*.pdf";
                        postfix = ".pdf";
                        break;
                    case Autodesk.Revit.DB.VirtualPrinterType.DWFWriter:
                        saveDlg.Filter = "dwf files (*.dwf)|*.dwf";
                        postfix = ".dwf";
                        break;
                    case Autodesk.Revit.DB.VirtualPrinterType.None:
                        saveDlg.Filter = "prn files (*.prn)|*.prn";
                        postfix = ".prn";
                        break;
                    case VirtualPrinterType.XPSWriter:
                        saveDlg.Filter = "XPS files (*.xps)|*.xps";
                        postfix = ".xps";
                        break;
                    default:
                        break;
                }

                string title = m_commandData.Application.ActiveUIDocument.Document.Title;
                if (title.Contains(".rvt"))
                {
                    saveDlg.FileName = title.Remove(title.LastIndexOf(".")) + postfix;
                }
                else
                {
                    saveDlg.FileName = title + postfix;
                }

                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    return m_printMgr.PrintToFileName
                        = saveDlg.FileName;
                }
                else
                {
                    return null;
                }
            }
        }

        public Autodesk.Revit.DB.PrintRange PrintRange
        {
            get
            {
                return m_printMgr.PrintRange;
            }
            set
            {
                m_printMgr.PrintRange = value;
                m_printMgr.Apply();
            }
        }

        public bool Collate
        {
            get
            {
                return m_printMgr.Collate;
            }
            set
            {
                m_printMgr.Collate = value;
                m_printMgr.Apply();
            }
        }

        public int CopyNumber
        {
            get
            {
                return m_printMgr.CopyNumber;
            }
            set
            {
                m_printMgr.CopyNumber = value;
                m_printMgr.Apply();
            }
        }

        public bool PrintOrderReverse
        {
            get
            {
                return m_printMgr.PrintOrderReverse;
            }
            set
            {
                m_printMgr.PrintOrderReverse = value;
                m_printMgr.Apply();
            }
        }

        public string SelectedViewSheetSetName
        {
            get
            {
                IViewSheetSet theSet = m_printMgr.ViewSheetSetting.CurrentViewSheetSet;
                return (theSet is ViewSheetSet) ? 
                    (theSet as ViewSheetSet).Name : ConstData.InSessionName;
            }
        }

        public string DocumentTitle
        {
            get
            {
                string title = m_commandData.Application.ActiveUIDocument.Document.Title;
                if (title.Contains(".rvt"))
                {
                    return title.Remove(title.LastIndexOf(".")) + PostFix;
                }
                else
                {
                    return title + PostFix;
                }
            }
        }

        public string PostFix
        {
            get
            {
                string postfix = null;
                switch (m_printMgr.IsVirtual)
                {
                    case Autodesk.Revit.DB.VirtualPrinterType.AdobePDF:
                        postfix = ".pdf";
                        break;
                    case Autodesk.Revit.DB.VirtualPrinterType.DWFWriter:
                        postfix = ".dwf";
                        break;
                    case Autodesk.Revit.DB.VirtualPrinterType.XPSWriter:
                        postfix = ".xps";
                        break;
                    case Autodesk.Revit.DB.VirtualPrinterType.None:
                        postfix = ".prn";
                        break;
                    default:
                        break;
                }

                return postfix;
            }
        }
        
        public void ChangePrintSetup()
        {
            using (PrintSetupForm dlg = new PrintSetupForm(
                new PrintSTP(m_printMgr, m_commandData)))
            {
                dlg.ShowDialog();
            }
        }

        public void SelectViewSheetSet()
        {
            using (viewSheetSetForm dlg = new viewSheetSetForm(
                new ViewSheets(m_commandData.Application.ActiveUIDocument.Document)))
            {
                dlg.ShowDialog();
            }
        }

        public bool SubmitPrint()
        {
            return m_printMgr.SubmitPrint();
        }

        public bool VerifyPrintToFile(System.Windows.Forms.Control controlToEnableOrNot)
        {
            // Enable terms (or):
            // 1. Print to non-virtual printer.
            return controlToEnableOrNot.Enabled = 
                m_printMgr.IsVirtual == VirtualPrinterType.None ? true : false;
        }

        public bool VerifyCopies(Collection<System.Windows.Forms.Control> controlsToEnableOrNot)
        {
            // Enable terms (or):
            // 1. Print to non-virtual priter (physical printer or OneNote), and 
            // the "Print to file" check box is NOT checked.
            // Note: SnagIt is an exception

            bool enableOrNot = m_printMgr.IsVirtual == VirtualPrinterType.None
            && !m_printMgr.PrintToFile;

            try
            {
                int cn = m_printMgr.CopyNumber;
            }
            catch (Exception)
            {
                enableOrNot = false;
                // Note: SnagIt is an exception
            }

            foreach (System.Windows.Forms.Control control in controlsToEnableOrNot)
            {
                control.Enabled = enableOrNot;
            }

            return enableOrNot;
        }

        public bool VerifyPrintToFileName(Collection<System.Windows.Forms.Control> controlsToEnableOrNot)
        {
            // Enable terms (or):
            // 1. Print to virtual priter (PDF or DWF printer)
            // 2. Print to none-virtual printer (physical printer or OneNote), and the 
            // "Print to file" check box is checked.
            bool enableOrNot = (m_printMgr.IsVirtual != VirtualPrinterType.None)
                || (m_printMgr.IsVirtual == VirtualPrinterType.None
                && m_printMgr.PrintToFile);


            foreach (System.Windows.Forms.Control control in controlsToEnableOrNot)
            {
                control.Enabled = enableOrNot;
            }

            return enableOrNot;
        }

        public bool VerifyPrintToSingleFile(System.Windows.Forms.Control controlToEnableOrNot)
        {
            // Enable terms (or):
            // 1. Print to virtual priter (PDF or DWF printer)
            return controlToEnableOrNot.Enabled = m_printMgr.IsVirtual != VirtualPrinterType.None;
        }

        public bool VerifyPrintToSeparateFile(System.Windows.Forms.Control controlToEnableOrNot)
        {
            // Enable terms (or):
            // 1. Print to virtual priter (PDF or DWF printer) and Print range is select.
            // 2. a) Print to none-virtual printer (physical printer or OneNote),  b) the 
            // "Print to file" check box is checked, and c) the Print range is select
            
            return controlToEnableOrNot.Enabled = ((m_printMgr.IsVirtual != VirtualPrinterType.None
                && m_printMgr.PrintRange == Autodesk.Revit.DB.PrintRange.Select)
                || (m_printMgr.IsVirtual == VirtualPrinterType.None
                && m_printMgr.PrintRange == Autodesk.Revit.DB.PrintRange.Select
                && m_printMgr.PrintToFile));
        }

        public bool VerifyCollate(System.Windows.Forms.Control controlToEnableOrNot)
        {
            // Enable terms (or):
            // 1. a) Print range is select b) the copy number is more 1 c) and the Print to file
            // is not selected.
            int cn = 0;
            try
            {
                cn = m_printMgr.CopyNumber;
            }
            catch (InvalidOperationException)
            {
                //The property CopyNumber is not available.
            }

            return controlToEnableOrNot.Enabled = m_printMgr.PrintRange == Autodesk.Revit.DB.PrintRange.Select
                && !m_printMgr.PrintToFile
                && cn > 1;
        }

        public bool VerifySelectViewSheetSet(Collection<System.Windows.Forms.Control> controlsToEnableOrNot)
        {
            // Enable terms (or):
            // 1. Print range is select.
            bool enableOrNot = m_printMgr.PrintRange == Autodesk.Revit.DB.PrintRange.Select;
            foreach (System.Windows.Forms.Control control in controlsToEnableOrNot)
            {
                control.Enabled = enableOrNot;
            }

            return enableOrNot;
        }

        /// <summary>
        /// global and consistent for message box with same caption
        /// </summary>
        /// <param name="text">MessageBox's text.</param>
        public static void MyMessageBox(string text)
        {
            MessageBox.Show(text, "View Printer");
        }
    }
}
