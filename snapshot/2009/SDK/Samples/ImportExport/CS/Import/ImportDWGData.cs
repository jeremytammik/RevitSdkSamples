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
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.Elements;


namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores the information for importing dwg format
    /// </summary>
    public class ImportDWGData : ImportData
    {
        #region Class Memeber Variables
        // ThisViewOnly
        private bool m_importThisViewOnly;

        // All views 
        private ViewSet m_views;

        // Import view
        private View m_importView;

        // AolorMode for import
        private List<String> m_colorMode;

        // All available import color modes
        private List<Autodesk.Revit.Enums.ImportColorMode> m_enumColorMode;

        // Import color mode
        private Autodesk.Revit.Enums.ImportColorMode m_importColorMode;

        // Custom scale for import
        private double m_importCustomScale;

        // OrientToView
        private bool m_importOrientToView;

        // Placement
        private List<String> m_placement;

        // All placement for layers to be imported
        private List<Autodesk.Revit.Enums.ImportPlacement> m_enumPlacement;

        // Placement for import
        private Autodesk.Revit.Enums.ImportPlacement m_importPlacement;

        // All units for layer to be imported
        private List<String> m_unit;

        /// All import unit for import layers
        private List<Autodesk.Revit.Enums.ImportUnit> m_enumUnit;

        /// Import unit
        private Autodesk.Revit.Enums.ImportUnit m_importUnit;

        /// All available layers only 
        private List<String> m_visibleLayersOnly;

        // All boolean values for available visible layers
        private List<bool> m_enumVisibleLayersOnly;

        // Whether import visible layer only
        private bool m_importVisibleLayersOnly;

        // Whether active view is 3D
        private bool m_is3DView;
        #endregion


        #region Class Properties
        /// <summary>
        /// Get or set whether import this view only
        /// </summary>
        public bool ImportThisViewOnly
        {
            get
            {
                return m_importThisViewOnly;
            }
            set
            {
                m_importThisViewOnly = value;
            }
        }


        /// <summary>
        /// all views for import
        /// </summary>
        public ViewSet Views
        {
            get
            {
                return m_views;
            }
            set
            {
                m_views = value;
            }
        }


        /// <summary>
        /// Import view
        /// </summary>
        public View ImportView
        {
            get
            {
                return m_importView;
            }
            set
            {
                m_importView = value;
            }
        }


        /// <summary>
        /// All available color modes for import
        /// </summary>
        public ReadOnlyCollection<String> ColorMode
        {
            get
            {
                return new ReadOnlyCollection<String>(m_colorMode);
            }
        }


        /// <summary>
        /// All available import color modes
        /// </summary>
        public ReadOnlyCollection<Autodesk.Revit.Enums.ImportColorMode> EnumColorMode
        {
            get
            {
                return new ReadOnlyCollection<Autodesk.Revit.Enums.ImportColorMode>(m_enumColorMode);
            }
        }


        /// <summary>
        /// Import color mode
        /// </summary>
        public Autodesk.Revit.Enums.ImportColorMode ImportColorMode
        {
            get
            {
                return m_importColorMode;
            }
            set
            {
                m_importColorMode = value;
            }
        }


        /// <summary>
        /// Custom scale for import
        /// </summary>
        public double ImportCustomScale
        {
            get
            {
                return m_importCustomScale;
            }
            set
            {
                m_importCustomScale = value;
            }
        }


        /// <summary>
        /// Whether import orient to view
        /// </summary>
        public bool ImportOrientToView
        {
            get
            {
                return m_importOrientToView;
            }
            set
            {
                m_importOrientToView = value;
            }
        }


        /// <summary>
        /// All placement for layers to be imported
        /// </summary>
        public ReadOnlyCollection<String> Placement
        {
            get
            {
                return new ReadOnlyCollection<String>(m_placement);
            }
        }


        /// <summary>
        /// All ImportPlacements for all layers to be imported
        /// </summary>
        public ReadOnlyCollection<Autodesk.Revit.Enums.ImportPlacement> EnumPlacement
        {
            get
            {
                return new ReadOnlyCollection<Autodesk.Revit.Enums.ImportPlacement>(m_enumPlacement);
            }
        }


        /// <summary>
        /// Import placement for import
        /// </summary>
        public Autodesk.Revit.Enums.ImportPlacement ImportPlacement
        {
            get
            {
                return m_importPlacement;
            }
            set
            {
                m_importPlacement = value;
            }
        }


        /// <summary>
        /// All units for layer to be imported
        /// </summary>
        public ReadOnlyCollection<String> Unit
        {
            get
            {
                return new ReadOnlyCollection<string>(m_unit);
            }
        }


        /// <summary>
        /// All import unit for import layers
        /// </summary>
        public ReadOnlyCollection<Autodesk.Revit.Enums.ImportUnit> EnumUnit
        {
            get
            {
                return new ReadOnlyCollection<Autodesk.Revit.Enums.ImportUnit>(m_enumUnit);
            }
        }


        /// <summary>
        /// Get or set import unit
        /// </summary>
        public Autodesk.Revit.Enums.ImportUnit ImportUnit
        {
            get
            {
                return m_importUnit;
            }
            set
            {
                m_importUnit = value;
            }
        }


        /// <summary>
        /// All available layers only 
        /// </summary>
        public ReadOnlyCollection<String> VisibleLayersOnly
        {
            get
            {
                return new ReadOnlyCollection<String>(m_visibleLayersOnly);
            }
        }


        /// <summary>
        /// All boolean values for available visible layers
        /// </summary>
        public ReadOnlyCollection<bool> EnumVisibleLayersOnly
        {
            get
            {
                return new ReadOnlyCollection<bool>(m_enumVisibleLayersOnly);
            }
        }


        /// <summary>
        /// Whether import visible layer only
        /// </summary>
        public bool ImportVisibleLayersOnly
        {
            get
            {
                return m_importVisibleLayersOnly;
            }
            set
            {
                m_importVisibleLayersOnly = value;
            }
        }

        /// <summary>
        /// Whether active view is 3D
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
        #endregion


        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="format">Format to import</param>
        public ImportDWGData(ExternalCommandData commandData, ImportFormat format)
            : base(commandData, format)
        {
            Initialize();
        }


        /// <summary>
        /// Collect the parameters and export
        /// </summary>
        /// <returns></returns>
        public override bool Import()
        {
            bool imported = false;

            //parameter: DWGImportOptions
            DWGImportOptions dwgImportOption = new DWGImportOptions();
            dwgImportOption.ColorMode = m_importColorMode;
            dwgImportOption.CustomScale = m_importCustomScale;
            dwgImportOption.OrientToView = m_importOrientToView;
            dwgImportOption.Placement = m_importPlacement;
            dwgImportOption.ThisViewOnly = m_importThisViewOnly;
            dwgImportOption.Unit = m_importUnit;
            dwgImportOption.View = m_importView;
            dwgImportOption.VisibleLayersOnly = m_importVisibleLayersOnly;

            //parameter: Element
            Element element = new Element();

            //Import
            imported = m_activeDoc.Import(m_importFileFullName, dwgImportOption, ref element);

            return imported;
        }
        #endregion


        #region Class Implementation
        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Initialize()
        {
            //ColorMode
            m_colorMode = new List<String>();
            m_enumColorMode = new List<Autodesk.Revit.Enums.ImportColorMode>();
            m_colorMode.Add("Black and white");
            m_enumColorMode.Add(Autodesk.Revit.Enums.ImportColorMode.BlackAndWhite);
            m_colorMode.Add("Preserve colors");
            m_enumColorMode.Add(Autodesk.Revit.Enums.ImportColorMode.Preserved);
            m_colorMode.Add("Invert colors");
            m_enumColorMode.Add(Autodesk.Revit.Enums.ImportColorMode.Inverted);

            //Placement
            m_placement = new List<String>();
            m_enumPlacement = new List<Autodesk.Revit.Enums.ImportPlacement>();
            m_placement.Add("Center-to-center");
            m_enumPlacement.Add(Autodesk.Revit.Enums.ImportPlacement.Centered);
            m_placement.Add("Origin-to-origin");
            m_enumPlacement.Add(Autodesk.Revit.Enums.ImportPlacement.Origin);

            //Unit
            m_unit = new List<String>();
            m_enumUnit = new List<Autodesk.Revit.Enums.ImportUnit>();
            m_unit.Add("Auto-Detect");
            m_enumUnit.Add(Autodesk.Revit.Enums.ImportUnit.Default);
            m_unit.Add(Autodesk.Revit.Enums.ImportUnit.Foot.ToString());
            m_enumUnit.Add(Autodesk.Revit.Enums.ImportUnit.Foot);
            m_unit.Add(Autodesk.Revit.Enums.ImportUnit.Inch.ToString());
            m_enumUnit.Add(Autodesk.Revit.Enums.ImportUnit.Inch);
            m_unit.Add(Autodesk.Revit.Enums.ImportUnit.Meter.ToString());
            m_enumUnit.Add(Autodesk.Revit.Enums.ImportUnit.Meter);
            m_unit.Add(Autodesk.Revit.Enums.ImportUnit.Decimeter.ToString());
            m_enumUnit.Add(Autodesk.Revit.Enums.ImportUnit.Decimeter);
            m_unit.Add(Autodesk.Revit.Enums.ImportUnit.Centimeter.ToString());
            m_enumUnit.Add(Autodesk.Revit.Enums.ImportUnit.Centimeter);
            m_unit.Add(Autodesk.Revit.Enums.ImportUnit.Millimeter.ToString());
            m_enumUnit.Add(Autodesk.Revit.Enums.ImportUnit.Millimeter);
            m_unit.Add("Custom");
            m_enumUnit.Add(Autodesk.Revit.Enums.ImportUnit.Default);

            //VisibleLayersOnly
            m_visibleLayersOnly = new List<String>();
            m_enumVisibleLayersOnly = new List<bool>();
            m_visibleLayersOnly.Add("All");
            m_enumVisibleLayersOnly.Add(false);
            m_visibleLayersOnly.Add("Visible");
            m_enumVisibleLayersOnly.Add(true);

            //Whether active view is 3D
            m_is3DView = false;
            if (m_activeDoc.ActiveView.ViewType == Autodesk.Revit.Enums.ViewType.ThreeD)
            {
                m_is3DView = true;
            }            

            //Views
            m_views = new ViewSet();
            GetViews();

            m_importCustomScale = 0.0;
            m_importOrientToView = true;
            m_importUnit = Autodesk.Revit.Enums.ImportUnit.Default;
            m_importThisViewOnly = false;
            m_importView = m_activeDoc.ActiveView;
            m_importColorMode = Autodesk.Revit.Enums.ImportColorMode.Inverted;
            m_importPlacement = Autodesk.Revit.Enums.ImportPlacement.Centered;
            m_importVisibleLayersOnly = false;

            m_filter = "DWG Files (*.dwg)|*.dwg";
            m_title = "Import DWG";
        }


        /// <summary>
        /// Get all the views to be displayed
        /// </summary>
        private void GetViews()
        {
            ElementIterator itor = m_activeDoc.Elements;
            itor.Reset();
            ViewSet views = new ViewSet();
            ViewSet floorPlans = new ViewSet();
            ViewSet ceilingPlans = new ViewSet();
            ViewSet engineeringPlans = new ViewSet();
            while (itor.MoveNext())
            {
                View view = itor.Current as View;
                if (view == null)
                {
                    continue;
                }
                else if (view.ViewType == Autodesk.Revit.Enums.ViewType.FloorPlan)
                {
                    floorPlans.Insert(view);
                }
                else if (view.ViewType == Autodesk.Revit.Enums.ViewType.CeilingPlan)
                {
                    ceilingPlans.Insert(view);
                }
                else if (view.ViewType == Autodesk.Revit.Enums.ViewType.EngineeringPlan)
                {
                    engineeringPlans.Insert(view);
                }
            }

            foreach (View floorPlan in floorPlans)
            {
                foreach (View ceilingPlan in ceilingPlans)
                {
                    if (floorPlan.ViewName == ceilingPlan.ViewName)
                    {
                        views.Insert(floorPlan);
                    }
                }
            }

            foreach (View engineeringPlan in engineeringPlans)
            {
                if (engineeringPlan.ViewName == engineeringPlan.GenLevel.Name)
                {
                    views.Insert(engineeringPlan);
                }
            }

            View activeView = m_activeDoc.ActiveView;
            Autodesk.Revit.Enums.ViewType viewType = activeView.ViewType;
            if (viewType == Autodesk.Revit.Enums.ViewType.FloorPlan ||
                viewType == Autodesk.Revit.Enums.ViewType.CeilingPlan)
            {
                m_views.Insert(activeView);
                foreach (View view in views)
                {
                    if (view.GenLevel.Elevation < activeView.GenLevel.Elevation)
                    {
                        m_views.Insert(view);
                    }
                }
            }
            else if (viewType == Autodesk.Revit.Enums.ViewType.EngineeringPlan)
            {
                if (views.Contains(activeView))
                {
                    m_views.Insert(activeView);
                }
                foreach (View view in views)
                {
                    if (view.GenLevel.Elevation < activeView.GenLevel.Elevation)
                    {
                        m_views.Insert(view);
                    }
                }
            }
            else//Get view of the lowest elevation
            {
                int i = 0;
                double elevation = 0;
                View viewLowestElevation = new View();
                foreach (View view in views)
                {
                    if (i == 0)
                    {
                        elevation = view.GenLevel.Elevation;
                        viewLowestElevation = view;
                    }
                    else
                    {
                        if (view.GenLevel.Elevation <= elevation)
                        {
                            elevation = view.GenLevel.Elevation;
                            viewLowestElevation = view;
                        }
                    }

                    i++;
                }
                m_views.Insert(viewLowestElevation);
            }
        }
        #endregion
    }
}
