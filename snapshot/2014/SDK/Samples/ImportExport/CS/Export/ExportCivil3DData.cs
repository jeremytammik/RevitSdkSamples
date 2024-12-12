//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores the main information for exporting Civil3D format
    /// </summary>
    public class ExportCivil3DData : ExportData
    {
        #region Class Member Variables
        /// <summary>
        /// Revit application
        /// </summary>
        private Autodesk.Revit.ApplicationServices.Application m_application;
        /// <summary>
        /// Revit document
        /// </summary>
        private Document m_document;
        /// <summary>
        /// 3D view to be exported
        /// </summary>
        private View3D m_view3D;
        /// <summary>
        /// List of 3D view in the document
        /// </summary>
        private List<Autodesk.Revit.DB.Element> m_view3DList;
        /// <summary>
        /// The view plan on which all the areas will be exported
        /// </summary>
        private ViewPlan m_grossAreaPlan;
        /// <summary>
        /// List of gross area plans in the document
        /// </summary>
        private List<ViewPlan> m_grossAreaPlanList;
        /// <summary>
        /// The optional property line to be exported
        /// </summary>
        private PropertyLine m_propertyLine;
        /// <summary>
        /// List of property lines in the document
        /// </summary>
        private List<Autodesk.Revit.DB.Element> m_propertyLineList;
        /// <summary>
        /// Property line offset
        /// </summary>
        private double m_PropertyLineOffset;
        /// <summary>
        /// Whether all data are validated for export
        /// </summary>
        private bool m_dataValidated;

        /// <summary>
        /// Current display unit type
        /// </summary>
        protected DisplayUnitType m_dut;
        #endregion

        #region Class Properties
        /// <summary>
        /// 3D view to be exported
        /// </summary>
        public View3D View3D
        {
            get
            {
                return m_view3D;
            }
            set
            {
                m_view3D = value;
            }
        }

        /// <summary>
        /// List of 3D view in the document
        /// </summary>
        public ReadOnlyCollection<Element> View3DList
        {
            get
            {
                return new ReadOnlyCollection<Element>(m_view3DList);
            }
        }

        /// <summary>
        /// The view plan on which all the areas will be exported
        /// </summary>
        public ViewPlan GrossAreaPlan
        {
            get
            {
                return m_grossAreaPlan;
            }
            set
            {
                m_grossAreaPlan = value;
            }
        }

        /// <summary>
        /// The view plan on which all the areas will be exported
        /// </summary>
        public ReadOnlyCollection<ViewPlan> GrossAreaPlanList
        {
            get
            {
                return new ReadOnlyCollection<ViewPlan>(m_grossAreaPlanList);
            }
        }

        /// <summary>
        /// The optional property line to be exported
        /// </summary>
        public PropertyLine PropertyLine
        {
            get
            {
                return m_propertyLine;
            }
            set
            {
                m_propertyLine = value;
            }
        }

        /// <summary>
        /// List of property lines in the document
        /// </summary>
        public ReadOnlyCollection<Element> PropertyLineList
        {
            get
            {
                return new ReadOnlyCollection<Element>(m_propertyLineList);
            }
        }

        /// <summary>
        /// Current display unit type
        /// </summary>
        public DisplayUnitType Dut
        {
            get
            {
                return m_dut;
            }
        }

        /// <summary>
        /// Property line offset
        /// </summary>
        public double PropertyLineOffset
        {
            get
            {
                return m_PropertyLineOffset;
            }
            set
            {
                m_PropertyLineOffset = value;
            }
        }

        /// <summary>
        /// Whether all data are validated for export
        /// </summary>
        public bool DataValidated
        {
            get
            {
                return m_dataValidated;
            }
        }
        #endregion

        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="exportFormat">Format to export</param>
        public ExportCivil3DData(ExternalCommandData commandData, ExportFormat exportFormat)
            : base(commandData, exportFormat)
        {
            m_application = commandData.Application.Application;
            m_document = commandData.Application.ActiveUIDocument.Document;
            m_dut = GetLengthUnitType(m_document);
            Initialize();
        }

        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Initialize()
        {
            if (!GetGrossAreaPlans())
            {
                m_dataValidated = false;
                return;
            }

            Get3DViews();
            m_dataValidated = true;
            GetPropertyLines();
            m_PropertyLineOffset = 0;

            m_filter = "Civil Design Exchange File |*.adsk";
            m_title = "Export Civil Design Exchange File";
        }

        /// <summary>
        /// Get current length display unit type
        /// </summary>
        /// <param name="document">Revit's document</param>
        /// <returns>Current length display unit type</returns>
        private static DisplayUnitType GetLengthUnitType(Document document)
        {
            UnitType unittype = UnitType.UT_Length;
            Units units = document.GetUnits();
            try
            {
                Autodesk.Revit.DB.FormatOptions formatOption = units.GetFormatOptions(unittype);
                return formatOption.DisplayUnits;
            }
            catch (System.Exception /*e*/)
            {
                return DisplayUnitType.DUT_DECIMAL_FEET;
            }
        }

        /// <summary>
        /// Get all 3D views in current document
        /// </summary>
        private void Get3DViews()
        {
            m_view3DList = new List<Autodesk.Revit.DB.Element>();
            FilteredElementCollector collector = new FilteredElementCollector(m_document);
            var query = from elem in collector.OfClass(typeof(View3D))
                        let viewElem = elem as View3D
                        where null != viewElem && !viewElem.IsTemplate
                        select elem;
            m_view3DList = query.ToList<Element>();
            if (m_view3DList.Count == 0)
            {
                //View3D view3D = m_document.Create.NewView3D(new Autodesk.Revit.DB.XYZ (0, 0, 1));
                IList<Element> viewFamilyTypes = new FilteredElementCollector(m_document).OfClass(typeof(ViewFamilyType)).ToElements();
                ElementId View3DId = new ElementId(-1);
                foreach (Element e in viewFamilyTypes)
                {
                    if (e.Name == "3D View")
                    {
                        View3DId = e.Id;
                    }
                }
                View3D view3D = View3D.CreateIsometric(m_document, View3DId);
                ViewOrientation3D viewOrientation3D = new ViewOrientation3D(new XYZ(1, 1, -1), new XYZ(0, 0, 1), new XYZ(0, 1, 0));
                view3D.SetOrientation(viewOrientation3D);
                view3D.SaveOrientation();
                m_view3DList.Add(view3D);
            }
        }

        /// <summary>
        /// Get all property lines in current document
        /// </summary>
        private void GetPropertyLines()
        {
            m_propertyLineList = new List<Autodesk.Revit.DB.Element>();
            FilteredElementCollector collector = new FilteredElementCollector(m_document);
            m_propertyLineList.AddRange(collector.OfClass(typeof(PropertyLine)).ToElements());
        }

        /// <summary>
        /// Get all gross area plans in current document
        /// </summary>
        /// <returns>If the document does not contain any gross area plan, return false</returns>
        private bool GetGrossAreaPlans()
        {
            List<Autodesk.Revit.DB.Element> viewPlanList = new List<Autodesk.Revit.DB.Element>();
            FilteredElementCollector collector = new FilteredElementCollector(m_document);
            viewPlanList.AddRange(collector.OfClass(typeof(ViewPlan)).ToElements());
            m_grossAreaPlanList = new List<ViewPlan>();
            foreach (ViewPlan vp in viewPlanList)
            {
                if (!vp.IsTemplate && vp.AreaScheme != null && vp.AreaScheme.IsGrossBuildingArea == true)
                {
                    m_grossAreaPlanList.Add(vp);
                }
            }

            if (m_grossAreaPlanList.Count == 0)
            {
                TaskDialog.Show("No Gross Area Plan", "The document does not contain any gross area plan.", TaskDialogCommonButtons.Ok);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Collect the parameters and export
        /// </summary>
        /// <returns></returns>
        public override bool Export()
        {
            base.Export();

            bool exported = false;

            //parameter : BuildingSiteExportOptions options
            BuildingSiteExportOptions options = new BuildingSiteExportOptions();
            options.PropertyLine = m_propertyLine;
            options.PropertyLineOffset = m_PropertyLineOffset;

            //Export
            exported = m_activeDoc.Export(m_exportFolder, m_exportFileName, m_view3D, m_grossAreaPlan, options);
            return exported;
        }
        #endregion
    }
}
