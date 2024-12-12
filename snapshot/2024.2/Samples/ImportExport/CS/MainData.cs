//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.IO;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Export formats
    /// </summary>
    public enum ExportFormat
    {
        /// <summary>
        /// DWG format
        /// </summary>
        DWG,
        /// <summary>
        /// DXF format
        /// </summary>
        DXF,
        /// <summary>
        /// SAT format
        /// </summary> 
        SAT,
        /// <summary>
        /// DWF format
        /// </summary>
        DWF,
        /// <summary>
        /// DWFx format
        /// </summary>
        DWFx,
        /// <summary>
        /// GBXML format
        /// </summary>
        GBXML,
        /// <summary>
        /// FBX format
        /// </summary>
        FBX,
        /// <summary>
        /// DGN format
        /// </summary>
        DGN,
        /// <summary>
        /// IMG format
        /// </summary>
        Image,
        /// <summary>
        /// PDF format
        /// </summary>
        PDF
   }

    /// <summary>
    /// Import formats
    /// </summary>
    public enum ImportFormat
    {
        /// <summary>
        /// DWF format
        /// </summary>
        DWG,
        /// <summary>
        /// IMAGE format
        /// </summary>
        IMAGE,
        /// <summary>
        /// GBXML format
        /// </summary>
        GBXML,
        /// <summary>
        /// Inventor format
        /// </summary>
        Inventor
    }

    /// <summary>
    /// Data class contains the external command data.
    /// </summary>
    public class MainData
    {
        // Revit command data
        private ExternalCommandData m_commandData;

        // Whether current view is a 3D view
        private bool m_is3DView;

        /// <summary>
        /// Revit command data
        /// </summary>
        public ExternalCommandData CommandData
        {
            get
            {
                return m_commandData;
            }
        }

        /// <summary>
        /// Whether current view is a 3D view
        /// </summary>
        public bool Is3DView
        {
            get
            {
                return m_is3DView;
            }
            set
            {
                m_is3DView = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        public MainData(ExternalCommandData commandData)
        {
            m_commandData = commandData;

            //Whether current active view is 3D view
            if (commandData.Application.ActiveUIDocument.Document.ActiveView.ViewType == Autodesk.Revit.DB.ViewType.ThreeD)
            {
                m_is3DView = true;
            }
            else
            {
                m_is3DView = false;
            }
        }

        /// <summary>
        /// Get the format to export
        /// </summary>
        /// <param name="selectedFormat">Selected format in format selecting dialog</param>
        /// <returns>The format to export</returns>
        private static ExportFormat GetSelectedExportFormat(string selectedFormat)
        {
            ExportFormat format = ExportFormat.DWG;
            switch (selectedFormat)
            {
                case "DWG":
                    format = ExportFormat.DWG;
                    break;
                case "DXF":
                    format = ExportFormat.DXF;
                    break;
                case "SAT":
                    format = ExportFormat.SAT;
                    break;
                case "DWF":
                    format = ExportFormat.DWF;
                    break;
                case "DWFx":
                    format = ExportFormat.DWFx;
                    break;
                case "GBXML":
                    format = ExportFormat.GBXML;
                    break;
                case "FBX":
                    format = ExportFormat.FBX;
                    break;
                case "DGN":
                    format = ExportFormat.DGN;
                    break;
                case "IMAGE":
                    format = ExportFormat.Image;
                    break;
                case "PDF":
                    format = ExportFormat.PDF;
                    break;
                default:
                    break;
            }

            return format;
        }

        /// <summary>
        /// Export according to selected format
        /// </summary>
        /// <param name="selectedFormat">Selected format</param>
        public DialogResult Export(string selectedFormat)
        {
            ExportFormat format = GetSelectedExportFormat(selectedFormat);
            DialogResult dialogResult = DialogResult.OK;

            try
            {
                switch (format)
                {
                    case ExportFormat.DWG:
                        ExportDWGData exportDWGData = new ExportDWGData(m_commandData, format);
                        using (ExportWithViewsForm exportForm = new ExportWithViewsForm(exportDWGData))
                        {
                            dialogResult = exportForm.ShowDialog();
                        }
                        break;
                    case ExportFormat.DXF:
                        ExportDXFData exportDXFData = new ExportDXFData(m_commandData, format);
                        using (ExportWithViewsForm exportForm = new ExportWithViewsForm(exportDXFData))
                        {
                            dialogResult = exportForm.ShowDialog();
                        }
                        break;
                    case ExportFormat.SAT:
                        ExportSATData exportSATData = new ExportSATData(m_commandData, format);
                        using (ExportWithViewsForm exportForm = new ExportWithViewsForm(exportSATData))
                        {
                            dialogResult = exportForm.ShowDialog();
                        }
                        break;
                    case ExportFormat.DWF:
                    case ExportFormat.DWFx:
                        ExportDWFData exportDWFData = new ExportDWFData(m_commandData, format);
                        using (ExportWithViewsForm exportForm = new ExportWithViewsForm(exportDWFData))
                        {
                            dialogResult = exportForm.ShowDialog();
                        }
                        break;
                    case ExportFormat.GBXML:
                        ExportGBXMLData exportGBXMLData = new ExportGBXMLData(m_commandData, format);
                        dialogResult = Export(exportGBXMLData);
                        break;
                    case ExportFormat.FBX:
                        ExportFBXData exportFBXData = new ExportFBXData(m_commandData, format);
                        dialogResult = Export(exportFBXData);
                        break;
                    case ExportFormat.DGN:
                        ExportDGNData exportDGNData = new ExportDGNData(m_commandData, format);
                        using (ExportWithViewsForm exportForm = new ExportWithViewsForm(exportDGNData))
                        {
                            dialogResult = exportForm.ShowDialog();
                        }
                        break;
                    case ExportFormat.Image:
                        ExportIMGData exportIMGdata = new ExportIMGData(m_commandData, format);
                        using (ExportWithViewsForm exportForm = new ExportWithViewsForm(exportIMGdata))
                        {
                            dialogResult = DialogResult.OK;
                        }
                        break;
                    case ExportFormat.PDF:
                        ExportPDFData exportPDFData = new ExportPDFData(m_commandData, format);
                        using (ExportWithViewsForm exportForm = new ExportWithViewsForm(exportPDFData))
                        {
                           dialogResult = exportForm.ShowDialog();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                String errorMessage = "Failed to export " + format + " format" + ex.ToString(); ;
                TaskDialog.Show("Error", errorMessage, TaskDialogCommonButtons.Ok);
            }

            return dialogResult;
        }

        /// <summary>
        /// Export
        /// </summary>
        /// <param name="data"></param>
        private static DialogResult Export(ExportData data)
        {
            String returnFilename = String.Empty;
            int filterIndex = -1;

            DialogResult result = ShowSaveDialog(data, ref returnFilename, ref filterIndex);
            if (result != DialogResult.Cancel)
            {
                data.ExportFileName = Path.GetFileName(returnFilename);
                data.ExportFolder = Path.GetDirectoryName(returnFilename);
                if (!data.Export())
                {
                    TaskDialog.Show("Export", "This project cannot be exported to " + data.ExportFileName +
                        " in current settings.", TaskDialogCommonButtons.Ok);
                }
            }

            return result;
        }

        /// <summary>
        /// Show Save dialog
        /// </summary>
        /// <param name="exportData">Data to export</param>
        /// <param name="returnFileName">File name will be returned</param>
        /// <param name="filterIndex">Selected filter index will be returned</param>
        /// <returns></returns>
        public static DialogResult ShowSaveDialog(ExportData exportData, ref String returnFileName,
            ref int filterIndex)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = exportData.Title;
                saveDialog.InitialDirectory = exportData.ExportFolder;
                saveDialog.FileName = exportData.ExportFileName;
                saveDialog.Filter = exportData.Filter;
                saveDialog.FilterIndex = 1;
                saveDialog.RestoreDirectory = true;

                DialogResult result = saveDialog.ShowDialog();
                if (result != DialogResult.Cancel)
                {
                    returnFileName = saveDialog.FileName;
                    filterIndex = saveDialog.FilterIndex;
                }

                return result;
            }
        }

        /// <summary>
        /// Get the format to import
        /// </summary>
        /// <param name="selectedFormat">Selected format in format selecting dialog</param>
        /// <returns>The format to import</returns>
        private static ImportFormat GetSelectedImportFormat(string selectedFormat)
        {
            ImportFormat format = ImportFormat.DWG;
            switch (selectedFormat)
            {
                case "DWG":
                    format = ImportFormat.DWG;
                    break;
                case "IMAGE":
                    format = ImportFormat.IMAGE;
                    break;
                case "GBXML":
                    format = ImportFormat.GBXML;
                    break;
                case "Inventor":
                    format = ImportFormat.Inventor;
                    break;
                default:
                    break;
            }

            return format;
        }

        /// <summary>
        /// Export according to selected format
        /// </summary>
        /// <param name="selectedFormat">Selected format</param>
        /// <returns></returns>
        public DialogResult Import(string selectedFormat)
        {
            DialogResult dialogResult = DialogResult.OK;
            ImportFormat format = GetSelectedImportFormat(selectedFormat);

            try
            {
                switch (format)
                {
                    case ImportFormat.DWG:
                        ImportDWGData importDWGData = new ImportDWGData(m_commandData, format);
                        using (ImportDWGForm importForm = new ImportDWGForm(importDWGData))
                        {
                            dialogResult = importForm.ShowDialog();
                        }
                        break;
                    case ImportFormat.IMAGE:
                        ImportImageData importIMAGEData = new ImportImageData(m_commandData, format);
                        dialogResult = Import(importIMAGEData);
                        break;
                    case ImportFormat.GBXML:
                        ImportGBXMLData importGBXMLData = new ImportGBXMLData(m_commandData, format);
                        dialogResult = Import(importGBXMLData);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                String errorMessage = "Failed to import " + format + " format";
                TaskDialog.Show("Error", errorMessage, TaskDialogCommonButtons.Ok);
            }

            return dialogResult;
        }

        /// <summary>
        /// Import
        /// </summary>
        /// <param name="data"></param>
        private static DialogResult Import(ImportData data)
        {
            String returnFilename = String.Empty;
            DialogResult result = ShowOpenDialog(data, ref returnFilename);
            if (result != DialogResult.Cancel)
            {
                data.ImportFileFullName = returnFilename;
                if (!data.Import())
                {
                    TaskDialog.Show("Import", "Cannot import " + Path.GetFileName(data.ImportFileFullName) +
                        " in current settings.", TaskDialogCommonButtons.Ok);
                }
            }

            return result;
        }

        /// <summary>
        /// Show Open File dialog
        /// </summary>
        /// <param name="importData">Data to import</param>
        /// <param name="returnFileName">File name will be returned</param>
        /// <returns>Dialog result</returns>
        public static DialogResult ShowOpenDialog(ImportData importData, ref String returnFileName)
        {
            using (OpenFileDialog importDialog = new OpenFileDialog())
            {
                importDialog.Title = importData.Title;
                importDialog.InitialDirectory = importData.ImportFolder;
                importDialog.Filter = importData.Filter;
                importDialog.RestoreDirectory = true;

                DialogResult result = importDialog.ShowDialog();
                if (result != DialogResult.Cancel)
                {
                    returnFileName = importDialog.FileName;
                }

                return result;
            }
        }
    }
}
