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
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Data class which stores lower priority information for exporting dwg or dxf format
    /// </summary>
    public class ExportBaseOptionsData
    {
        #region Class Member Variables
        /// <summary>
        /// String list of Layers and properties used in UI
        /// </summary>
        List<String> m_layersAndProperties;

        /// <summary>
        /// List of Autodesk.Revit.DB.PropOverrideMode
        /// </summary>
        List<Autodesk.Revit.DB.PropOverrideMode> m_enumLayersAndProperties;

        /// <summary>
        /// String list of Layer Settings used in UI
        /// </summary>
        List<String> m_layerMapping;

        /// <summary>
        /// String list of layer settings values defined in Revit 
        /// </summary>
        List<String> m_enumLayerMapping;

        /// <summary>
        /// Layer setting option to export
        /// </summary>
        String m_exportLayerMapping;

        /// <summary>
        /// String list of Linetype scaling used in UI
        /// </summary>
        List<String> m_lineScaling;

        /// <summary>
        /// PropOverrideMode Option to export
        /// </summary>
        Autodesk.Revit.DB.PropOverrideMode m_exportLayersAndProperties;

        /// <summary>
        /// List of Autodesk.Revit.DB.LineScaling defined in Revit
        /// </summary>
        List<Autodesk.Revit.DB.LineScaling> m_enumLineScaling;

        /// <summary>
        /// Line scaling option to export
        /// </summary>
        Autodesk.Revit.DB.LineScaling m_exportLineScaling;

        /// <summary>
        /// String list of Coordinate system basis
        /// </summary>
        List<String> m_coorSystem;

        /// <summary>
        /// List of values whether to use shared coordinate system
        /// </summary>
        List<bool> m_enumCoorSystem;

        /// <summary>
        /// Coordinate system basis option to export
        /// </summary>
        bool m_exportCoorSystem;

        /// <summary>
        /// String list of DWG or DXF unit
        /// </summary>
        List<String> m_units;

        /// <summary>
        /// List of Autodesk.Revit.DB.ExportUnit values defined in Revit
        /// </summary>
        List<Autodesk.Revit.DB.ExportUnit> m_enumUnits;

        /// <summary>
        /// Export unit option to export
        /// </summary>
        Autodesk.Revit.DB.ExportUnit m_exportUnit;

        /// <summary>
        /// String list of solid used in UI
        /// </summary>
        List<String> m_solids;

        /// <summary>
        /// List of Autodesk.Revit.DB.SolidGeometry defined in Revit
        /// </summary>
        List<Autodesk.Revit.DB.SolidGeometry> m_enumSolids;

        /// <summary>
        /// Solid geometry option to export
        /// </summary>
        Autodesk.Revit.DB.SolidGeometry m_exportSolid;

        /// <summary>
        /// Whether to create separate files for each view/sheet
        /// </summary>
        bool m_exportMergeFiles;

        //Export rooms and areas as polylines
        bool m_exportAreas;
        #endregion

        #region Class Properties
        /// <summary>
        /// String collection of Layers and properties used in UI
        /// </summary>
        public ReadOnlyCollection<String> LayersAndProperties
        {
            get 
            { 
                return new ReadOnlyCollection<String>(m_layersAndProperties); 
            }
        }        

        /// <summary>
        /// Collection of Autodesk.Revit.DB.PropOverrideMode
        /// </summary>
        public ReadOnlyCollection<Autodesk.Revit.DB.PropOverrideMode> EnumLayersAndProperties
        {
            get 
            { 
                return new ReadOnlyCollection<Autodesk.Revit.DB.PropOverrideMode>(m_enumLayersAndProperties); 
            }
        } 

        /// <summary>
        /// PropOverrideMode Option to export
        /// </summary>
        public Autodesk.Revit.DB.PropOverrideMode ExportLayersAndProperties
        {
            get 
            { 
                return m_exportLayersAndProperties; 
            }
            set 
            { 
                m_exportLayersAndProperties = value; 
            }
        }

        /// <summary>
        /// String collection of Layer Settings used in UI
        /// </summary>
        public ReadOnlyCollection<String> LayerMapping
        {
            get 
            {
                return new ReadOnlyCollection<String>(m_layerMapping); 
            }
        }  

        /// <summary>
        /// String collection of layer settings values defined in Revit  
        /// </summary>
        public ReadOnlyCollection<String> EnumLayerMapping
        {
            get 
            {
                return new ReadOnlyCollection<String>(m_enumLayerMapping); 
            }
        }

        /// <summary>
        /// Layer setting option to export
        /// </summary>
        public String ExportLayerMapping
        {
            get 
            { 
                return m_exportLayerMapping; 
            }
            set 
            { 
                m_exportLayerMapping = value;
            }
        }

        /// <summary>
        /// String collection of Linetype scaling used in UI
        /// </summary>
        public ReadOnlyCollection<String> LineScaling
        {
            get 
            {
                return new ReadOnlyCollection<String>(m_lineScaling); 
            }
        }

        /// <summary>
        /// Collection of Autodesk.Revit.DB.LineScaling defined in Revit
        /// </summary>
        public ReadOnlyCollection<Autodesk.Revit.DB.LineScaling> EnumLineScaling
        {
            get 
            { 
                return new ReadOnlyCollection<Autodesk.Revit.DB.LineScaling>(m_enumLineScaling); 
            }
        }      

        /// <summary>
        /// Line scaling option to export
        /// </summary>
        public Autodesk.Revit.DB.LineScaling ExportLineScaling
        {
            get 
            { 
                return m_exportLineScaling; 
            }
            set 
            { 
                m_exportLineScaling = value;
            }
        }

        /// <summary>
        /// String collection of Coordinate system basis
        /// </summary>
        public ReadOnlyCollection<String> CoorSystem
        {
            get 
            {
                return new ReadOnlyCollection<String>(m_coorSystem); 
            }
        }

        /// <summary>
        /// Collection of values whether to use shared coordinate system
        /// </summary>
        public ReadOnlyCollection<bool> EnumCoorSystem
        {
            get 
            { 
                return new ReadOnlyCollection<bool>(m_enumCoorSystem); 
            }
        }   

        /// <summary>
        /// Coordinate system basis option to export
        /// </summary>
        public bool ExportCoorSystem
        {
            get 
            { 
                return m_exportCoorSystem; 
            }
            set 
            { 
                m_exportCoorSystem = value;
            }
        } 

        /// <summary>
        /// String collection of DWG unit
        /// </summary>
        public ReadOnlyCollection<String> Units
        {
            get 
            {
                return new ReadOnlyCollection<String>(m_units); 
            }
        }

        /// <summary>
        /// Collection of Autodesk.Revit.DB.ExportUnit values defined in Revit
        /// </summary>
        public ReadOnlyCollection<Autodesk.Revit.DB.ExportUnit> EnumUnits
        {
            get 
            { 
                return new ReadOnlyCollection<Autodesk.Revit.DB.ExportUnit>(m_enumUnits); 
            }
        }  

        /// <summary>
        /// Export unit option to export
        /// </summary>
        public Autodesk.Revit.DB.ExportUnit ExportUnit
        {
            get 
            { 
                return m_exportUnit; 
            }
            set 
            { 
                m_exportUnit = value; 
            }
        }    

        /// <summary>
        /// String collection of solid used in UI
        /// </summary>
        public ReadOnlyCollection<String> Solids
        {
            get
            {
                return new ReadOnlyCollection<String>(m_solids);
            }
        }

        /// <summary>
        /// Collection of Autodesk.Revit.DB.SolidGeometry defined in Revit
        /// </summary>
        public ReadOnlyCollection<Autodesk.Revit.DB.SolidGeometry> EnumSolids
        {
            get 
            { 
                return new ReadOnlyCollection<Autodesk.Revit.DB.SolidGeometry>(m_enumSolids);
            }
        }

        /// <summary>
        /// Property of solid geometry option to export
        /// </summary>
        public Autodesk.Revit.DB.SolidGeometry ExportSolid
        {
            get 
            {
                return m_exportSolid; 
            }
            set 
            { 
                m_exportSolid = value;
            }
        }

        /// <summary>
        /// Export rooms and areas as polylines
        /// </summary>
        public bool ExportAreas
        {
            get 
            { 
                return m_exportAreas;
            }
            set 
            {
                m_exportAreas = value; 
            }
        }

        /// <summary>
        /// Whether to create separate files for each view/sheet
        /// </summary>
        public bool ExportMergeFiles
        {
            get
            {
                return m_exportMergeFiles;
            }
            set
            {
                m_exportMergeFiles = value;
            }
        } 
        #endregion


        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public ExportBaseOptionsData()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize values
        /// </summary>
        void Initialize()
        {
            //Layers and properties:
            m_layersAndProperties = new List<String>();
            m_enumLayersAndProperties = new List<Autodesk.Revit.DB.PropOverrideMode>();
            m_layersAndProperties.Add("Category properties BYLAYER, overrides BYENTITY");
            m_enumLayersAndProperties.Add(Autodesk.Revit.DB.PropOverrideMode.ByEntity);
            m_layersAndProperties.Add("All properties BYLAYER, no overrides");
            m_enumLayersAndProperties.Add(Autodesk.Revit.DB.PropOverrideMode.ByLayer);
            m_layersAndProperties.Add("All properties BYLAYER, new Layers for overrides");
            m_enumLayersAndProperties.Add(Autodesk.Revit.DB.PropOverrideMode.NewLayer);
            

            //Layer Settings:
            m_layerMapping = new List<String>();
            m_enumLayerMapping = new List<String>();
            m_layerMapping.Add("AIA - American Institute of Architects standard");
            m_enumLayerMapping.Add("AIA");
            m_layerMapping.Add("ISO13567 - ISO standard 13567");
            m_enumLayerMapping.Add("ISO13567");
            m_layerMapping.Add("CP83 - Singapore standard 83");
            m_enumLayerMapping.Add("CP83");
            m_layerMapping.Add("BS1192 - British standard 1192");
            m_enumLayerMapping.Add("BS1192");

            //Linetype scaling:
            m_lineScaling = new List<String>();
            m_enumLineScaling = new List<Autodesk.Revit.DB.LineScaling>();
            m_lineScaling.Add("Scaled Linetype definitions");
            m_enumLineScaling.Add(Autodesk.Revit.DB.LineScaling.ViewScale);
            m_lineScaling.Add("ModelSpace (PSLTSCALE = 0)");
            m_enumLineScaling.Add(Autodesk.Revit.DB.LineScaling.ModelSpace);
            m_lineScaling.Add("Paperspace (PSLTSCALE = 1)");
            m_enumLineScaling.Add(Autodesk.Revit.DB.LineScaling.PaperSpace);

            //Coordinate system basis
            m_coorSystem = new List<String>();
            m_enumCoorSystem = new List<bool>();
            m_coorSystem.Add("Project Internal");
            m_enumCoorSystem.Add(false);
            m_coorSystem.Add("Shared");
            m_enumCoorSystem.Add(true);

            //One DWG unit
            m_units = new List<String>();
            m_enumUnits = new List<Autodesk.Revit.DB.ExportUnit>();
            m_units.Add(Autodesk.Revit.DB.ExportUnit.Foot.ToString().ToLower());
            m_enumUnits.Add(Autodesk.Revit.DB.ExportUnit.Foot);
            m_units.Add(Autodesk.Revit.DB.ExportUnit.Inch.ToString().ToLower());
            m_enumUnits.Add(Autodesk.Revit.DB.ExportUnit.Inch);
            m_units.Add(Autodesk.Revit.DB.ExportUnit.Meter.ToString().ToLower());
            m_enumUnits.Add(Autodesk.Revit.DB.ExportUnit.Meter);
            m_units.Add(Autodesk.Revit.DB.ExportUnit.Centimeter.ToString().ToLower());
            m_enumUnits.Add(Autodesk.Revit.DB.ExportUnit.Centimeter);
            m_units.Add(Autodesk.Revit.DB.ExportUnit.Millimeter.ToString().ToLower());
            m_enumUnits.Add(Autodesk.Revit.DB.ExportUnit.Millimeter);

            m_solids = new List<String>();
            m_enumSolids = new List<Autodesk.Revit.DB.SolidGeometry>();
            m_solids.Add("Export as polymesh");
            m_enumSolids.Add(Autodesk.Revit.DB.SolidGeometry.Polymesh);
            m_solids.Add("Export as ACIS solids");
            m_enumSolids.Add(Autodesk.Revit.DB.SolidGeometry.ACIS);

            // Set default values
            m_exportAreas = false;
            m_exportSolid = Autodesk.Revit.DB.SolidGeometry.Polymesh;
            m_exportLayerMapping = "AIA";
            m_exportLayersAndProperties = Autodesk.Revit.DB.PropOverrideMode.ByEntity;
            m_exportLineScaling = Autodesk.Revit.DB.LineScaling.PaperSpace;
            m_exportMergeFiles = false;
            m_exportCoorSystem = EnumCoorSystem[0];
            m_exportUnit = Autodesk.Revit.DB.ExportUnit.Inch;
        }
        #endregion
    }
}
