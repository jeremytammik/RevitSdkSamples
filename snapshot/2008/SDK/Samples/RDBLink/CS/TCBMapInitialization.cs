using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.Parameters;
using Autodesk.Revit;

namespace Revit.SDK.Samples.RDBLink.CS
{
    sealed public partial class TableInfoSet : List<TableInfo>
    {
        /// <summary>
        /// private constructor, as required by singleton nature of the class.
        /// </summary>
        TableInfoSet()
        {
        }
        /// <summary>
        /// Do initialization
        /// </summary>
        static TableInfoSet()
        {
            TableInfo ti = null;
            Filter filter;

            // create Filter for table "AreaSchemes"
            filter = new Filter(BuiltInCategory.OST_AreaSchemes, ElementType.INSTANCE);
            // create TableInfo for "AreaSchemes"
            ti = new TableInfo("AreaSchemes", filter);
            {
                // add TableInfo to TableInfoSet
                s_tableInfoSet.Add(ti);
                // add TableInfo to Filter-TableInfo dictionary
                s_filterTableInfoMap.Add(filter, ti);
                // add columns to TableInfo
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Name", BuiltInParameter.AREA_SCHEME_NAME));
            }
            filter = new Filter(BuiltInCategory.OST_EAConstructions, ElementType.SYMBOL);
            ti = new TableInfo("Constructions1", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_DesignOptions, ElementType.INSTANCE);
            ti = new TableInfo("DesignOptions", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("DesignOptionSetId", BuiltInParameter.OPTION_SET_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Name", BuiltInParameter.OPTION_NAME));
            }
            filter = new Filter(BuiltInCategory.OST_Materials, ElementType.INSTANCE);
            ti = new TableInfo("Materials", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Name", BuiltInParameter.MATERIAL_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Phases, ElementType.INSTANCE);
            ti = new TableInfo("Phases", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Name", BuiltInParameter.PHASE_NAME));
                ti.Columns.Add(new ColumnInfo("SequenceNumber", BuiltInParameter.PHASE_SEQUENCE_NUMBER));
            }
            filter = new Filter(BuiltInCategory.OST_ProjectInformation, ElementType.INSTANCE);
            ti = new TableInfo("ProjectInformation", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("ClientName", BuiltInParameter.CLIENT_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("ProjectAddress", BuiltInParameter.PROJECT_ADDRESS));
                ti.Columns.Add(new ColumnInfo("ProjectIssueDate", BuiltInParameter.PROJECT_ISSUE_DATE));
                ti.Columns.Add(new ColumnInfo("ProjectName", BuiltInParameter.PROJECT_NAME));
                ti.Columns.Add(new ColumnInfo("ProjectNumber", BuiltInParameter.PROJECT_NUMBER));
                ti.Columns.Add(new ColumnInfo("ProjectStatus", BuiltInParameter.PROJECT_STATUS));
            }
            filter = new Filter(BuiltInCategory.OST_DuctTerminal, ElementType.SYMBOL);
            ti = new TableInfo("AirTerminalTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("MaxFlow", BuiltInParameter.RBS_MAX_FLOW));
                ti.Columns.Add(new ColumnInfo("MinFlow", BuiltInParameter.RBS_MIN_FLOW));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_AreaLoads, ElementType.INSTANCE);
            ti = new TableInfo("AreaLoads", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Allnon0loads", BuiltInParameter.LOAD_ALL_NON_0_LOADS));
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.LOAD_AREA_AREA));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.LOAD_COMMENTS));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.LOAD_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Fx1", BuiltInParameter.LOAD_AREA_FORCE_FX1));
                ti.Columns.Add(new ColumnInfo("Fx2", BuiltInParameter.LOAD_AREA_FORCE_FX2));
                ti.Columns.Add(new ColumnInfo("Fx3", BuiltInParameter.LOAD_AREA_FORCE_FX3));
                ti.Columns.Add(new ColumnInfo("Fy1", BuiltInParameter.LOAD_AREA_FORCE_FY1));
                ti.Columns.Add(new ColumnInfo("Fy2", BuiltInParameter.LOAD_AREA_FORCE_FY2));
                ti.Columns.Add(new ColumnInfo("Fy3", BuiltInParameter.LOAD_AREA_FORCE_FY3));
                ti.Columns.Add(new ColumnInfo("Fz1", BuiltInParameter.LOAD_AREA_FORCE_FZ1));
                ti.Columns.Add(new ColumnInfo("Fz2", BuiltInParameter.LOAD_AREA_FORCE_FZ2));
                ti.Columns.Add(new ColumnInfo("Fz3", BuiltInParameter.LOAD_AREA_FORCE_FZ3));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("IsReaction", BuiltInParameter.LOAD_IS_REACTION));
                ti.Columns.Add(new ColumnInfo("LoadCase", BuiltInParameter.LOAD_CASE_ID));
                ti.Columns.Add(new ColumnInfo("Nature", BuiltInParameter.LOAD_CASE_NATURE_TEXT));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
            }
            filter = new Filter(BuiltInCategory.OST_Casework, ElementType.SYMBOL);
            ti = new TableInfo("CaseworkTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("ConstructionType", BuiltInParameter.CURTAIN_WALL_PANELS_CONSTRUCTION_TYPE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Depth", BuiltInParameter.CASEWORK_DEPTH));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Finish", BuiltInParameter.CURTAIN_WALL_PANELS_FINISH));
                ti.Columns.Add(new ColumnInfo("Height", BuiltInParameter.FURNITURE_HEIGHT));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
                ti.Columns.Add(new ColumnInfo("Width", BuiltInParameter.FURNITURE_WIDTH));
            }
            filter = new Filter(BuiltInCategory.OST_Ceilings, ElementType.SYMBOL);
            ti = new TableInfo("CeilingTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Columns, ElementType.SYMBOL);
            ti = new TableInfo("ColumnTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_CommunicationDevices, ElementType.SYMBOL);
            ti = new TableInfo("CommunicationDeviceTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_EAConstructions, ElementType.INSTANCE);
            ti = new TableInfo("Constructions", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_CurtainWallPanels, ElementType.SYMBOL);
            ti = new TableInfo("CurtainPanelTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("ConstructionType", BuiltInParameter.CURTAIN_WALL_PANELS_CONSTRUCTION_TYPE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Finish", BuiltInParameter.CURTAIN_WALL_PANELS_FINISH));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_CurtaSystem, ElementType.SYMBOL);
            ti = new TableInfo("CurtainSystemTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_CurtainWallMullions, ElementType.SYMBOL);
            ti = new TableInfo("CurtainWallMullionTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_DataDevices, ElementType.SYMBOL);
            ti = new TableInfo("DataDeviceTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_ElectricalDemandFactor, ElementType.SYMBOL);
            ti = new TableInfo("DemandFactorTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("ConnectedLoad", BuiltInParameter.RBS_ELEC_DEMANDFACTOR_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("EstimatedDemandLoad", BuiltInParameter.RBS_ELEC_DEMANDFACTOR_DEMANDLOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("LoadClassification", BuiltInParameter.RBS_ELEC_DEMANDFACTOR_LOADCLASSIFICATION_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_DesignOptionSets, ElementType.INSTANCE);
            ti = new TableInfo("DesignOptionSets", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Name", BuiltInParameter.OPTION_SET_NAME));
                ti.Columns.Add(new ColumnInfo("PrimaryOptionId", BuiltInParameter.PRIMARY_OPTION_ID));
            }
            filter = new Filter(BuiltInCategory.OST_ElecDistributionSys, ElementType.SYMBOL);
            ti = new TableInfo("DistributionSystemTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Configuration", BuiltInParameter.RBS_DISTRIBUTIONSYS_CONFIG_PARAM));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("LinetoGroundVoltage", BuiltInParameter.RBS_DISTRIBUTIONSYS_VLG_PARAM));
                ti.Columns.Add(new ColumnInfo("LinetoLineVoltage", BuiltInParameter.RBS_DISTRIBUTIONSYS_VLL_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Phase", BuiltInParameter.RBS_DISTRIBUTIONSYS_PHASE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
                ti.Columns.Add(new ColumnInfo("Wires", BuiltInParameter.RBS_DISTRIBUTIONSYS_NUMWIRES_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Doors, ElementType.SYMBOL);
            ti = new TableInfo("DoorTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("ConstructionType", BuiltInParameter.DOOR_CONSTRUCTION_TYPE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("FireRating", BuiltInParameter.DOOR_FIRE_RATING));
                ti.Columns.Add(new ColumnInfo("Height", BuiltInParameter.FAMILY_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Operation", BuiltInParameter.DOOR_OPERATION_TYPE));
                ti.Columns.Add(new ColumnInfo("RoughHeight", BuiltInParameter.FAMILY_ROUGH_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("RoughWidth", BuiltInParameter.FAMILY_ROUGH_WIDTH_PARAM));
                ti.Columns.Add(new ColumnInfo("Thickness", BuiltInParameter.FURNITURE_THICKNESS));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
                ti.Columns.Add(new ColumnInfo("Width", BuiltInParameter.FURNITURE_WIDTH));
            }
            filter = new Filter(BuiltInCategory.OST_DuctAccessory, ElementType.SYMBOL);
            ti = new TableInfo("DuctAccessoryTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_DuctFitting, ElementType.SYMBOL);
            ti = new TableInfo("DuctFittingTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_DuctSystem, ElementType.INSTANCE);
            ti = new TableInfo("DuctSystems", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Flow", BuiltInParameter.RBS_DUCT_FLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("NumberofElements", BuiltInParameter.RBS_SYSTEM_NUM_ELEMENTS_PARAM));
                ti.Columns.Add(new ColumnInfo("StaticPressure", BuiltInParameter.RBS_DUCT_STATIC_PRESSURE));
                ti.Columns.Add(new ColumnInfo("SystemEquipment", BuiltInParameter.RBS_SYSTEM_BASE_ELEMENT_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_DuctCurves, ElementType.SYMBOL);
            ti = new TableInfo("DuctTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_ElectricalCircuit, ElementType.INSTANCE);
            ti = new TableInfo("ElectricalCircuits", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("ApparentCurrent", BuiltInParameter.RBS_ELEC_APPARENT_CURRENT_PARAM));
                ti.Columns.Add(new ColumnInfo("ApparentCurrentPhaseA", BuiltInParameter.RBS_ELEC_APPARENT_CURRENT_PHASEA_PARAM));
                ti.Columns.Add(new ColumnInfo("ApparentCurrentPhaseB", BuiltInParameter.RBS_ELEC_APPARENT_CURRENT_PHASEB_PARAM));
                ti.Columns.Add(new ColumnInfo("ApparentCurrentPhaseC", BuiltInParameter.RBS_ELEC_APPARENT_CURRENT_PHASEC_PARAM));
                ti.Columns.Add(new ColumnInfo("ApparentLoad", BuiltInParameter.RBS_ELEC_APPARENT_LOAD));
                ti.Columns.Add(new ColumnInfo("ApparentLoadPhaseA", BuiltInParameter.RBS_ELEC_APPARENT_LOAD_PHASEA));
                ti.Columns.Add(new ColumnInfo("ApparentLoadPhaseB", BuiltInParameter.RBS_ELEC_APPARENT_LOAD_PHASEB));
                ti.Columns.Add(new ColumnInfo("ApparentLoadPhaseC", BuiltInParameter.RBS_ELEC_APPARENT_LOAD_PHASEC));
                ti.Columns.Add(new ColumnInfo("BalancedLoad", BuiltInParameter.RBS_ELEC_BALANCED_LOAD));
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM));
                ti.Columns.Add(new ColumnInfo("LoadClassification", BuiltInParameter.RBS_ELEC_LOAD_CLASSIFICATION));
                ti.Columns.Add(new ColumnInfo("LoadName", BuiltInParameter.RBS_ELEC_CIRCUIT_NAME));
                ti.Columns.Add(new ColumnInfo("NumberofPoles", BuiltInParameter.RBS_ELEC_NUMBER_OF_POLES));
                ti.Columns.Add(new ColumnInfo("ofGroundConductors", BuiltInParameter.RBS_ELEC_CIRCUIT_WIRE_NUM_GROUNDS_PARAM));
                ti.Columns.Add(new ColumnInfo("ofHotConductors", BuiltInParameter.RBS_ELEC_CIRCUIT_WIRE_NUM_HOTS_PARAM));
                ti.Columns.Add(new ColumnInfo("ofNeutralConductors", BuiltInParameter.RBS_ELEC_CIRCUIT_WIRE_NUM_NEUTRALS_PARAM));
                ti.Columns.Add(new ColumnInfo("ofRuns", BuiltInParameter.RBS_ELEC_CIRCUIT_WIRE_NUM_RUNS_PARAM));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PowerFactor", BuiltInParameter.RBS_ELEC_POWER_FACTOR));
                ti.Columns.Add(new ColumnInfo("PowerFactorState", BuiltInParameter.RBS_ELEC_POWER_FACTOR_STATE));
                ti.Columns.Add(new ColumnInfo("Rating", BuiltInParameter.RBS_ELEC_CIRCUIT_RATING_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_ELEC_CIRCUIT_TYPE));
                ti.Columns.Add(new ColumnInfo("TrueCurrent", BuiltInParameter.RBS_ELEC_TRUE_CURRENT_PARAM));
                ti.Columns.Add(new ColumnInfo("TrueCurrentPhaseA", BuiltInParameter.RBS_ELEC_TRUE_CURRENT_PHASEA_PARAM));
                ti.Columns.Add(new ColumnInfo("TrueCurrentPhaseB", BuiltInParameter.RBS_ELEC_TRUE_CURRENT_PHASEB_PARAM));
                ti.Columns.Add(new ColumnInfo("TrueCurrentPhaseC", BuiltInParameter.RBS_ELEC_TRUE_CURRENT_PHASEC_PARAM));
                ti.Columns.Add(new ColumnInfo("TrueLoad", BuiltInParameter.RBS_ELEC_TRUE_LOAD));
                ti.Columns.Add(new ColumnInfo("TrueLoadPhaseA", BuiltInParameter.RBS_ELEC_TRUE_LOAD_PHASEA));
                ti.Columns.Add(new ColumnInfo("TrueLoadPhaseB", BuiltInParameter.RBS_ELEC_TRUE_LOAD_PHASEB));
                ti.Columns.Add(new ColumnInfo("TrueLoadPhaseC", BuiltInParameter.RBS_ELEC_TRUE_LOAD_PHASEC));
                ti.Columns.Add(new ColumnInfo("Voltage", BuiltInParameter.RBS_ELEC_VOLTAGE));
                ti.Columns.Add(new ColumnInfo("VoltageDrop", BuiltInParameter.RBS_ELEC_VOLTAGE_DROP_PARAM));
                ti.Columns.Add(new ColumnInfo("WireSize", BuiltInParameter.RBS_ELEC_CIRCUIT_WIRE_SIZE_PARAM));
                ti.Columns.Add(new ColumnInfo("WireType", BuiltInParameter.RBS_ELEC_CIRCUIT_WIRE_TYPE_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_ElectricalEquipment, ElementType.SYMBOL);
            ti = new TableInfo("ElectricalEquipmentTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
                ti.Columns.Add(new ColumnInfo("Voltage", BuiltInParameter.ELECTICAL_EQUIP_VOLTAGE));
                ti.Columns.Add(new ColumnInfo("Wattage", BuiltInParameter.ELECTICAL_EQUIP_WATTAGE));
            }
            filter = new Filter(BuiltInCategory.OST_ElectricalFixtures, ElementType.SYMBOL);
            ti = new TableInfo("ElectricalFixtureTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_FireAlarmDevices, ElementType.SYMBOL);
            ti = new TableInfo("FireAlarmDeviceTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_FlexDuctCurves, ElementType.SYMBOL);
            ti = new TableInfo("FlexDuctTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_FlexPipeCurves, ElementType.SYMBOL);
            ti = new TableInfo("FlexPipeTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Roughness", BuiltInParameter.RBS_PIPE_ROUGHNESS_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Floors, ElementType.SYMBOL);
            ti = new TableInfo("FloorTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_FurnitureSystems, ElementType.SYMBOL);
            ti = new TableInfo("FurnitureSystemTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Furniture, ElementType.SYMBOL);
            ti = new TableInfo("FurnitureTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_GenericModel, ElementType.SYMBOL);
            ti = new TableInfo("GenericModelTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_InternalAreaLoads, ElementType.INSTANCE);
            ti = new TableInfo("InternalAreaLoads", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Allnon0loads", BuiltInParameter.LOAD_ALL_NON_0_LOADS));
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.LOAD_AREA_AREA));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.LOAD_COMMENTS));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.LOAD_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Fx1", BuiltInParameter.LOAD_AREA_FORCE_FX1));
                ti.Columns.Add(new ColumnInfo("Fx2", BuiltInParameter.LOAD_AREA_FORCE_FX2));
                ti.Columns.Add(new ColumnInfo("Fx3", BuiltInParameter.LOAD_AREA_FORCE_FX3));
                ti.Columns.Add(new ColumnInfo("Fy1", BuiltInParameter.LOAD_AREA_FORCE_FY1));
                ti.Columns.Add(new ColumnInfo("Fy2", BuiltInParameter.LOAD_AREA_FORCE_FY2));
                ti.Columns.Add(new ColumnInfo("Fy3", BuiltInParameter.LOAD_AREA_FORCE_FY3));
                ti.Columns.Add(new ColumnInfo("Fz1", BuiltInParameter.LOAD_AREA_FORCE_FZ1));
                ti.Columns.Add(new ColumnInfo("Fz2", BuiltInParameter.LOAD_AREA_FORCE_FZ2));
                ti.Columns.Add(new ColumnInfo("Fz3", BuiltInParameter.LOAD_AREA_FORCE_FZ3));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("IsReaction", BuiltInParameter.LOAD_IS_REACTION));
                ti.Columns.Add(new ColumnInfo("LoadCase", BuiltInParameter.LOAD_CASE_ID));
                ti.Columns.Add(new ColumnInfo("Nature", BuiltInParameter.LOAD_CASE_NATURE_TEXT));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
            }
            filter = new Filter(BuiltInCategory.OST_InternalLineLoads, ElementType.INSTANCE);
            ti = new TableInfo("InternalLineLoads", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Allnon0loads", BuiltInParameter.LOAD_ALL_NON_0_LOADS));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.LOAD_COMMENTS));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.LOAD_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Fx1", BuiltInParameter.LOAD_LINEAR_FORCE_FX1));
                ti.Columns.Add(new ColumnInfo("Fx2", BuiltInParameter.LOAD_LINEAR_FORCE_FX2));
                ti.Columns.Add(new ColumnInfo("Fy1", BuiltInParameter.LOAD_LINEAR_FORCE_FY1));
                ti.Columns.Add(new ColumnInfo("Fy2", BuiltInParameter.LOAD_LINEAR_FORCE_FY2));
                ti.Columns.Add(new ColumnInfo("Fz1", BuiltInParameter.LOAD_LINEAR_FORCE_FZ1));
                ti.Columns.Add(new ColumnInfo("Fz2", BuiltInParameter.LOAD_LINEAR_FORCE_FZ2));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("IsReaction", BuiltInParameter.LOAD_IS_REACTION));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.LOAD_LINEAR_LENGTH));
                ti.Columns.Add(new ColumnInfo("LoadCase", BuiltInParameter.LOAD_CASE_ID));
                ti.Columns.Add(new ColumnInfo("Mx1", BuiltInParameter.LOAD_MOMENT_MX1));
                ti.Columns.Add(new ColumnInfo("Mx2", BuiltInParameter.LOAD_MOMENT_MX2));
                ti.Columns.Add(new ColumnInfo("My1", BuiltInParameter.LOAD_MOMENT_MY1));
                ti.Columns.Add(new ColumnInfo("My2", BuiltInParameter.LOAD_MOMENT_MY2));
                ti.Columns.Add(new ColumnInfo("Mz1", BuiltInParameter.LOAD_MOMENT_MZ1));
                ti.Columns.Add(new ColumnInfo("Mz2", BuiltInParameter.LOAD_MOMENT_MZ2));
                ti.Columns.Add(new ColumnInfo("Nature", BuiltInParameter.LOAD_CASE_NATURE_TEXT));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("UniformLoad", BuiltInParameter.LOAD_IS_UNIFORM));
            }
            filter = new Filter(BuiltInCategory.OST_InternalPointLoads, ElementType.INSTANCE);
            ti = new TableInfo("InternalPointLoads", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Allnon0loads", BuiltInParameter.LOAD_ALL_NON_0_LOADS));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.LOAD_COMMENTS));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.LOAD_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Fx", BuiltInParameter.LOAD_FORCE_FX));
                ti.Columns.Add(new ColumnInfo("Fy", BuiltInParameter.LOAD_FORCE_FY));
                ti.Columns.Add(new ColumnInfo("Fz", BuiltInParameter.LOAD_FORCE_FZ));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("IsReaction", BuiltInParameter.LOAD_IS_REACTION));
                ti.Columns.Add(new ColumnInfo("LoadCase", BuiltInParameter.LOAD_CASE_ID));
                ti.Columns.Add(new ColumnInfo("Mx", BuiltInParameter.LOAD_MOMENT_MX));
                ti.Columns.Add(new ColumnInfo("My", BuiltInParameter.LOAD_MOMENT_MY));
                ti.Columns.Add(new ColumnInfo("Mz", BuiltInParameter.LOAD_MOMENT_MZ));
                ti.Columns.Add(new ColumnInfo("Nature", BuiltInParameter.LOAD_CASE_NATURE_TEXT));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
            }
            filter = new Filter(BuiltInCategory.OST_Levels, ElementType.INSTANCE);
            ti = new TableInfo("Levels", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Elevation", BuiltInParameter.LEVEL_ELEV));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Name", BuiltInParameter.DATUM_TEXT));
            }
            filter = new Filter(BuiltInCategory.OST_LightingDevices, ElementType.SYMBOL);
            ti = new TableInfo("LightingDeviceTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_LightingFixtures, ElementType.SYMBOL);
            ti = new TableInfo("LightingFixtureTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("ApparentLoad", BuiltInParameter.RBS_ELEC_APPARENT_LOAD));
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("BallastFactor", BuiltInParameter.RBS_ROOM_BALLAST_FACTOR));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("IESDataFileName", BuiltInParameter.RBS_ELEC_IES_FILE));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Lamp", BuiltInParameter.LIGHTING_FIXTURE_LAMP));
                ti.Columns.Add(new ColumnInfo("LightLossFactor", BuiltInParameter.RBS_ROOM_LIGHT_LOSS_FACTOR));
                ti.Columns.Add(new ColumnInfo("Lumens", BuiltInParameter.LIGHTING_FIXTURE_LUMENS));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
                ti.Columns.Add(new ColumnInfo("Wattage", BuiltInParameter.LIGHTING_FIXTURE_WATTAGE));
            }
            filter = new Filter(BuiltInCategory.OST_LineLoads, ElementType.INSTANCE);
            ti = new TableInfo("LineLoads", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Allnon0loads", BuiltInParameter.LOAD_ALL_NON_0_LOADS));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.LOAD_COMMENTS));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.LOAD_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Fx1", BuiltInParameter.LOAD_LINEAR_FORCE_FX1));
                ti.Columns.Add(new ColumnInfo("Fx2", BuiltInParameter.LOAD_LINEAR_FORCE_FX2));
                ti.Columns.Add(new ColumnInfo("Fy1", BuiltInParameter.LOAD_LINEAR_FORCE_FY1));
                ti.Columns.Add(new ColumnInfo("Fy2", BuiltInParameter.LOAD_LINEAR_FORCE_FY2));
                ti.Columns.Add(new ColumnInfo("Fz1", BuiltInParameter.LOAD_LINEAR_FORCE_FZ1));
                ti.Columns.Add(new ColumnInfo("Fz2", BuiltInParameter.LOAD_LINEAR_FORCE_FZ2));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("IsReaction", BuiltInParameter.LOAD_IS_REACTION));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.LOAD_LINEAR_LENGTH));
                ti.Columns.Add(new ColumnInfo("LoadCase", BuiltInParameter.LOAD_CASE_ID));
                ti.Columns.Add(new ColumnInfo("Mx1", BuiltInParameter.LOAD_MOMENT_MX1));
                ti.Columns.Add(new ColumnInfo("Mx2", BuiltInParameter.LOAD_MOMENT_MX2));
                ti.Columns.Add(new ColumnInfo("My1", BuiltInParameter.LOAD_MOMENT_MY1));
                ti.Columns.Add(new ColumnInfo("My2", BuiltInParameter.LOAD_MOMENT_MY2));
                ti.Columns.Add(new ColumnInfo("Mz1", BuiltInParameter.LOAD_MOMENT_MZ1));
                ti.Columns.Add(new ColumnInfo("Mz2", BuiltInParameter.LOAD_MOMENT_MZ2));
                ti.Columns.Add(new ColumnInfo("Nature", BuiltInParameter.LOAD_CASE_NATURE_TEXT));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("UniformLoad", BuiltInParameter.LOAD_IS_UNIFORM));
            }
            filter = new Filter(BuiltInCategory.OST_MechanicalEquipment, ElementType.SYMBOL);
            ti = new TableInfo("MechanicalEquipmentTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_NurseCallDevices, ElementType.SYMBOL);
            ti = new TableInfo("NurseCallDeviceTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Parking, ElementType.SYMBOL);
            ti = new TableInfo("ParkingTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_PipeAccessory, ElementType.SYMBOL);
            ti = new TableInfo("PipeAccessoryTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_PipeFitting, ElementType.SYMBOL);
            ti = new TableInfo("PipeFittingTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_PipeCurves, ElementType.SYMBOL);
            ti = new TableInfo("PipeTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Class", BuiltInParameter.RBS_PIPE_CLASS_PARAM));
                ti.Columns.Add(new ColumnInfo("ConnectionType", BuiltInParameter.RBS_PIPE_CONNECTIONTYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Material", BuiltInParameter.RBS_PIPE_MATERIAL_PARAM));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Roughness", BuiltInParameter.RBS_PIPE_ROUGHNESS_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_PipingSystem, ElementType.INSTANCE);
            ti = new TableInfo("PipingSystems", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Flow", BuiltInParameter.RBS_PIPE_FLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("FluidDensity", BuiltInParameter.RBS_PIPE_FLUID_DENSITY_PARAM));
                ti.Columns.Add(new ColumnInfo("FluidPercentage", BuiltInParameter.RBS_PIPE_FLUID_FACTOR_PARAM));
                ti.Columns.Add(new ColumnInfo("FluidTemperature", BuiltInParameter.RBS_PIPE_FLUID_TEMPERATURE_PARAM));
                ti.Columns.Add(new ColumnInfo("FluidType", BuiltInParameter.RBS_PIPE_FLUID_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("FluidViscosity", BuiltInParameter.RBS_PIPE_FLUID_VISCOSITY_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("NumberofElements", BuiltInParameter.RBS_SYSTEM_NUM_ELEMENTS_PARAM));
                ti.Columns.Add(new ColumnInfo("StaticPressure", BuiltInParameter.RBS_PIPE_STATIC_PRESSURE));
                ti.Columns.Add(new ColumnInfo("SystemEquipment", BuiltInParameter.RBS_SYSTEM_BASE_ELEMENT_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.RBS_PIPE_VOLUME_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Planting, ElementType.SYMBOL);
            ti = new TableInfo("PlantingTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_PlumbingFixtures, ElementType.SYMBOL);
            ti = new TableInfo("PlumbingFixtureTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("CWConnection", BuiltInParameter.PLUMBING_FIXTURES_CW_CONNECTION));
                ti.Columns.Add(new ColumnInfo("CWFU", BuiltInParameter.RBS_PIPE_CWFU_PARAM));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("HWConnection", BuiltInParameter.PLUMBING_FIXTURES_HW_CONNECTION));
                ti.Columns.Add(new ColumnInfo("HWFU", BuiltInParameter.RBS_PIPE_HWFU_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
                ti.Columns.Add(new ColumnInfo("VentConnection", BuiltInParameter.PLUMBING_FIXTURES_VENT_CONNECTION));
                ti.Columns.Add(new ColumnInfo("WasteConnection", BuiltInParameter.PLUMBING_FIXTURES_WASTE_CONNECTION));
                ti.Columns.Add(new ColumnInfo("WFU", BuiltInParameter.RBS_PIPE_WFU_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_PointLoads, ElementType.INSTANCE);
            ti = new TableInfo("PointLoads", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Allnon0loads", BuiltInParameter.LOAD_ALL_NON_0_LOADS));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.LOAD_COMMENTS));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.LOAD_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Fx", BuiltInParameter.LOAD_FORCE_FX));
                ti.Columns.Add(new ColumnInfo("Fy", BuiltInParameter.LOAD_FORCE_FY));
                ti.Columns.Add(new ColumnInfo("Fz", BuiltInParameter.LOAD_FORCE_FZ));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("IsReaction", BuiltInParameter.LOAD_IS_REACTION));
                ti.Columns.Add(new ColumnInfo("LoadCase", BuiltInParameter.LOAD_CASE_ID));
                ti.Columns.Add(new ColumnInfo("Mx", BuiltInParameter.LOAD_MOMENT_MX));
                ti.Columns.Add(new ColumnInfo("My", BuiltInParameter.LOAD_MOMENT_MY));
                ti.Columns.Add(new ColumnInfo("Mz", BuiltInParameter.LOAD_MOMENT_MZ));
                ti.Columns.Add(new ColumnInfo("Nature", BuiltInParameter.LOAD_CASE_NATURE_TEXT));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
            }
            filter = new Filter(BuiltInCategory.OST_ProfileFamilies, ElementType.SYMBOL);
            ti = new TableInfo("Profiles", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_SiteProperty, ElementType.SYMBOL);
            ti = new TableInfo("PropertyLineTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_StairsRailing, ElementType.SYMBOL);
            ti = new TableInfo("RailingTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Ramps, ElementType.SYMBOL);
            ti = new TableInfo("RampTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Roofs, ElementType.SYMBOL);
            ti = new TableInfo("RoofTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_SecurityDevices, ElementType.SYMBOL);
            ti = new TableInfo("SecurityDeviceTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Site, ElementType.SYMBOL);
            ti = new TableInfo("SiteTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_SpecialityEquipment, ElementType.SYMBOL);
            ti = new TableInfo("SpecialtyEquipmentTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Sprinklers, ElementType.SYMBOL);
            ti = new TableInfo("SprinklerTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Coverage", BuiltInParameter.RBS_FP_SPRINKLER_COVERAGE_PARAM));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("Diameter", BuiltInParameter.RBS_PIPE_DIAMETER_PARAM));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("K-Factor", BuiltInParameter.RBS_FP_SPRINKLER_K_FACTOR_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Orifice", BuiltInParameter.RBS_FP_SPRINKLER_ORIFICE_PARAM));
                ti.Columns.Add(new ColumnInfo("OrificeSize", BuiltInParameter.RBS_FP_SPRINKLER_ORIFICE_SIZE_PARAM));
                ti.Columns.Add(new ColumnInfo("PressureClass", BuiltInParameter.RBS_FP_SPRINKLER_PRESSURE_CLASS_PARAM));
                ti.Columns.Add(new ColumnInfo("Response", BuiltInParameter.RBS_FP_SPRINKLER_RESPONSE_PARAM));
                ti.Columns.Add(new ColumnInfo("TemperatureRating", BuiltInParameter.RBS_FP_SPRINKLER_TEMPERATURE_RATING_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Stairs, ElementType.SYMBOL);
            ti = new TableInfo("StairTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TreadThickness", BuiltInParameter.STAIRS_ATTR_TREAD_THICKNESS));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_StructuralColumns, ElementType.SYMBOL);
            ti = new TableInfo("StructuralColumnTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_StructuralFoundation, ElementType.SYMBOL);
            ti = new TableInfo("StructuralFoundationTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Truss, ElementType.SYMBOL);
            ti = new TableInfo("StructuralFramingTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_StructuralFraming, ElementType.SYMBOL);
            ti = new TableInfo("StructuralFramingTypes1", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Rebar, ElementType.SYMBOL);
            ti = new TableInfo("StructuralRebarTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_StructuralStiffener, ElementType.SYMBOL);
            ti = new TableInfo("StructuralStiffenerTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_SwitchSystem, ElementType.INSTANCE);
            ti = new TableInfo("SwitchSystem", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("SwitchID", BuiltInParameter.RBS_ELEC_SWITCH_ID_PARAM));
            } 
            filter = new Filter(BuiltInCategory.OST_TelephoneDevices, ElementType.SYMBOL);
            ti = new TableInfo("TelephoneDeviceTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Topography, ElementType.SYMBOL);
            ti = new TableInfo("TopographyTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_ElectricalVoltage, ElementType.SYMBOL);
            ti = new TableInfo("VoltageTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("ActualVoltage", BuiltInParameter.RBS_VOLTAGETYPE_VOLTAGE_PARAM));
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("MaximumVoltage", BuiltInParameter.RBS_VOLTAGETYPE_MAXVOLTAGE_PARAM));
                ti.Columns.Add(new ColumnInfo("MinimumVoltage", BuiltInParameter.RBS_VOLTAGETYPE_MINVOLTAGE_PARAM));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Walls, ElementType.SYMBOL);
            ti = new TableInfo("WallTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("FireRating", BuiltInParameter.DOOR_FIRE_RATING));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
                ti.Columns.Add(new ColumnInfo("Width", BuiltInParameter.WALL_ATTR_WIDTH_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Windows, ElementType.SYMBOL);
            ti = new TableInfo("WindowTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("ConstructionType", BuiltInParameter.CURTAIN_WALL_PANELS_CONSTRUCTION_TYPE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Height", BuiltInParameter.FAMILY_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Operation", BuiltInParameter.WINDOW_OPERATION_TYPE));
                ti.Columns.Add(new ColumnInfo("RoughHeight", BuiltInParameter.FAMILY_ROUGH_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("RoughWidth", BuiltInParameter.FAMILY_ROUGH_WIDTH_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
                ti.Columns.Add(new ColumnInfo("Width", BuiltInParameter.FURNITURE_WIDTH));
            }
            filter = new Filter(BuiltInCategory.OST_Wire, ElementType.SYMBOL);
            ti = new TableInfo("WireTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("ConduitType", BuiltInParameter.RBS_WIRE_CONDUIT_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Insulation", BuiltInParameter.RBS_WIRE_INSULATION_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Material", BuiltInParameter.RBS_WIRE_MATERIAL_PARAM));
                ti.Columns.Add(new ColumnInfo("MaxSize", BuiltInParameter.RBS_WIRE_MAX_CONDUCTOR_SIZE_PARAM));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("NeutralIncludedinBalancedLoad", BuiltInParameter.RBS_WIRE_NEUTRAL_INCLUDED_IN_BALANCED_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("NeutralMultiplier", BuiltInParameter.RBS_WIRE_NEUTRAL_MULTIPLIER_PARAM));
                ti.Columns.Add(new ColumnInfo("NeutralSize", BuiltInParameter.RBS_WIRE_NEUTRAL_MODE_PARAM));
                ti.Columns.Add(new ColumnInfo("ShareGroundConductor", BuiltInParameter.RBS_ELEC_WIRE_SHARE_GROUND));
                ti.Columns.Add(new ColumnInfo("ShareNeutralConductor", BuiltInParameter.RBS_ELEC_WIRE_SHARE_NEUTRAL));
                ti.Columns.Add(new ColumnInfo("TemperatureRating", BuiltInParameter.RBS_WIRE_TEMPERATURE_RATING_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_DuctTerminal, ElementType.INSTANCE);
            ti = new TableInfo("AirTerminals", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Flow", BuiltInParameter.RBS_DUCT_FLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Areas, ElementType.INSTANCE);
            ti = new TableInfo("Areas", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.ROOM_AREA));
                ti.Columns.Add(new ColumnInfo("AreaSchemeId", BuiltInParameter.AREA_SCHEME_ID));
                ti.Columns.Add(new ColumnInfo("AreaType", BuiltInParameter.AREA_TYPE_TEXT));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.ROOM_LEVEL_ID));
                ti.Columns.Add(new ColumnInfo("Name", BuiltInParameter.ROOM_NAME));
                ti.Columns.Add(new ColumnInfo("Number", BuiltInParameter.ROOM_NUMBER));
                ti.Columns.Add(new ColumnInfo("Perimeter", BuiltInParameter.ROOM_PERIMETER));
            }
            filter = new Filter(BuiltInCategory.OST_Casework, ElementType.INSTANCE);
            ti = new TableInfo("Casework", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Ceilings, ElementType.INSTANCE);
            ti = new TableInfo("Ceilings", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.HOST_AREA_COMPUTED));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("HeightOffsetFromLevel", BuiltInParameter.CEILING_HEIGHTABOVELEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Perimeter", BuiltInParameter.HOST_PERIMETER_COMPUTED));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.HOST_VOLUME_COMPUTED));
            }
            filter = new Filter(BuiltInCategory.OST_Columns, ElementType.INSTANCE);
            ti = new TableInfo("Columns", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("BaseLevel", BuiltInParameter.FAMILY_BASE_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("BaseOffset", BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TopLevel", BuiltInParameter.FAMILY_TOP_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("TopOffset", BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_CommunicationDevices, ElementType.INSTANCE);
            ti = new TableInfo("CommunicationDevices", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElectricalData", BuiltInParameter.RBS_ELECTRICAL_DATA));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_CurtainWallPanels, ElementType.INSTANCE);
            ti = new TableInfo("CurtainPanels", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.HOST_AREA_COMPUTED));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Height", BuiltInParameter.CURTAIN_WALL_PANELS_HEIGHT));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Width", BuiltInParameter.CURTAIN_WALL_PANELS_WIDTH));
            }
            filter = new Filter(BuiltInCategory.OST_CurtaSystem, ElementType.INSTANCE);
            ti = new TableInfo("CurtainSystems", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_CurtainWallMullions, ElementType.INSTANCE);
            ti = new TableInfo("CurtainWallMullions", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_DataDevices, ElementType.INSTANCE);
            ti = new TableInfo("DataDevices", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElectricalData", BuiltInParameter.RBS_ELECTRICAL_DATA));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_ElectricalDemandFactor, ElementType.INSTANCE);
            ti = new TableInfo("DemandFactors", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_ElecDistributionSys, ElementType.INSTANCE);
            ti = new TableInfo("DistributionSystems", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Doors, ElementType.INSTANCE);
            ti = new TableInfo("Doors", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Finish", BuiltInParameter.GENERIC_FINISH));
                ti.Columns.Add(new ColumnInfo("FrameMaterial", BuiltInParameter.DOOR_FRAME_MATERIAL));
                ti.Columns.Add(new ColumnInfo("FrameType", BuiltInParameter.DOOR_FRAME_TYPE));
                ti.Columns.Add(new ColumnInfo("HeadHeight", BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("SillHeight", BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_DuctAccessory, ElementType.INSTANCE);
            ti = new TableInfo("DuctAccessories", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("Size", BuiltInParameter.RBS_CALCULATED_SIZE));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_DuctFitting, ElementType.INSTANCE);
            ti = new TableInfo("DuctFittings", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.HOST_AREA_COMPUTED));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("Size", BuiltInParameter.RBS_CALCULATED_SIZE));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.HOST_VOLUME_COMPUTED));
            }
            filter = new Filter(BuiltInCategory.OST_DuctCurves, ElementType.INSTANCE);
            ti = new TableInfo("Ducts", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AdditionalFlow", BuiltInParameter.RBS_ADDITIONAL_FLOW));
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.RBS_CURVE_SURFACE_AREA));
                ti.Columns.Add(new ColumnInfo("BottomElevation", BuiltInParameter.RBS_DUCT_BOTTOM_ELEVATION));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Diameter", BuiltInParameter.RBS_PIPE_DIAMETER_PARAM));
                ti.Columns.Add(new ColumnInfo("EquivalentDiameter", BuiltInParameter.RBS_EQ_DIAMETER_PARAM));
                ti.Columns.Add(new ColumnInfo("Flow", BuiltInParameter.RBS_DUCT_FLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("Friction", BuiltInParameter.RBS_FRICTION));
                ti.Columns.Add(new ColumnInfo("Height", BuiltInParameter.RBS_CURVE_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("HydraulicDiameter", BuiltInParameter.RBS_HYDRAULIC_DIAMETER_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("InsulationThickness", BuiltInParameter.RBS_INSULATION_THICKNESS));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("LiningThickness", BuiltInParameter.RBS_LINING_THICKNESS));
                ti.Columns.Add(new ColumnInfo("LossCoefficient", BuiltInParameter.RBS_LOSS_COEFFICIENT));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("PressureDrop", BuiltInParameter.RBS_PRESSURE_DROP));
                ti.Columns.Add(new ColumnInfo("Reynoldsnumber", BuiltInParameter.RBS_REYNOLDSNUMBER_PARAM));
                ti.Columns.Add(new ColumnInfo("Section", BuiltInParameter.RBS_SECTION));
                ti.Columns.Add(new ColumnInfo("Size", BuiltInParameter.RBS_CALCULATED_SIZE));
                ti.Columns.Add(new ColumnInfo("SizeLock", BuiltInParameter.RBS_SIZE_LOCK));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TopElevation", BuiltInParameter.RBS_DUCT_TOP_ELEVATION));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Velocity", BuiltInParameter.RBS_VELOCITY));
                ti.Columns.Add(new ColumnInfo("VelocityPressure", BuiltInParameter.RBS_VELOCITY_PRESSURE));
                ti.Columns.Add(new ColumnInfo("Width", BuiltInParameter.RBS_CURVE_WIDTH_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_ElectricalEquipment, ElementType.INSTANCE);
            ti = new TableInfo("ElectricalEquipment", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("ApparentLoadPhaseA", BuiltInParameter.RBS_ELEC_APPARENT_LOAD_PHASEA));
                ti.Columns.Add(new ColumnInfo("ApparentLoadPhaseB", BuiltInParameter.RBS_ELEC_APPARENT_LOAD_PHASEB));
                ti.Columns.Add(new ColumnInfo("ApparentLoadPhaseC", BuiltInParameter.RBS_ELEC_APPARENT_LOAD_PHASEC));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElectricalData", BuiltInParameter.RBS_ELECTRICAL_DATA));
                ti.Columns.Add(new ColumnInfo("Enclosure", BuiltInParameter.RBS_ELEC_ENCLOSURE));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("HVACTotalConnected", BuiltInParameter.RBS_ELEC_PANEL_TOTALLOAD_HVAC_PARAM));
                ti.Columns.Add(new ColumnInfo("HVACTotalEstimatedDemand", BuiltInParameter.RBS_ELEC_PANEL_TOTALESTLOAD_HVAC_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.SCHEDULE_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("LightingTotalConnected", BuiltInParameter.RBS_ELEC_PANEL_TOTALLOAD_LIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("LightingTotalEstimatedDemand", BuiltInParameter.RBS_ELEC_PANEL_TOTALESTLOAD_LIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("Mains", BuiltInParameter.RBS_ELEC_MAINS));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Max#1PoleBreakers", BuiltInParameter.RBS_ELEC_MAX_POLE_BREAKERS));
                ti.Columns.Add(new ColumnInfo("Modifications", BuiltInParameter.RBS_ELEC_MODIFICATIONS));
                ti.Columns.Add(new ColumnInfo("Mounting", BuiltInParameter.RBS_ELEC_MOUNTING));
                ti.Columns.Add(new ColumnInfo("OtherTotalConnected", BuiltInParameter.RBS_ELEC_PANEL_TOTALLOAD_OTHER_PARAM));
                ti.Columns.Add(new ColumnInfo("OtherTotalEstimatedDemand", BuiltInParameter.RBS_ELEC_PANEL_TOTALESTLOAD_OTHER_PARAM));
                ti.Columns.Add(new ColumnInfo("PanelName", BuiltInParameter.RBS_ELEC_PANEL_NAME));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("PowerTotalConnected", BuiltInParameter.RBS_ELEC_PANEL_TOTALLOAD_POWER_PARAM));
                ti.Columns.Add(new ColumnInfo("PowerTotalEstimatedDemand", BuiltInParameter.RBS_ELEC_PANEL_TOTALESTLOAD_POWER_PARAM));
                ti.Columns.Add(new ColumnInfo("ShortCircuitRating", BuiltInParameter.RBS_ELEC_SHORT_CIRCUIT_RATING));
                ti.Columns.Add(new ColumnInfo("TotalConnected", BuiltInParameter.RBS_ELEC_PANEL_TOTALLOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("TotalEstimatedDemand", BuiltInParameter.RBS_ELEC_PANEL_TOTALESTLOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_ElectricalFixtures, ElementType.INSTANCE);
            ti = new TableInfo("ElectricalFixtures", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElectricalData", BuiltInParameter.RBS_ELECTRICAL_DATA));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Fascia, ElementType.SYMBOL);
            ti = new TableInfo("FasciaTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Material", BuiltInParameter.MATERIAL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Profile", BuiltInParameter.FASCIA_PROFILE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_FireAlarmDevices, ElementType.INSTANCE);
            ti = new TableInfo("FireAlarmDevices", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElectricalData", BuiltInParameter.RBS_ELECTRICAL_DATA));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_FlexDuctCurves, ElementType.INSTANCE);
            ti = new TableInfo("FlexDucts", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AdditionalFlow", BuiltInParameter.RBS_ADDITIONAL_FLOW));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Diameter", BuiltInParameter.RBS_PIPE_DIAMETER_PARAM));
                ti.Columns.Add(new ColumnInfo("EquivalentDiameter", BuiltInParameter.RBS_EQ_DIAMETER_PARAM));
                ti.Columns.Add(new ColumnInfo("Flow", BuiltInParameter.RBS_DUCT_FLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("Friction", BuiltInParameter.RBS_FRICTION));
                ti.Columns.Add(new ColumnInfo("Height", BuiltInParameter.RBS_CURVE_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("HydraulicDiameter", BuiltInParameter.RBS_HYDRAULIC_DIAMETER_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("InsulationThickness", BuiltInParameter.RBS_INSULATION_THICKNESS));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("LiningThickness", BuiltInParameter.RBS_LINING_THICKNESS));
                ti.Columns.Add(new ColumnInfo("LossCoefficient", BuiltInParameter.RBS_LOSS_COEFFICIENT));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("PressureDrop", BuiltInParameter.RBS_PRESSURE_DROP));
                ti.Columns.Add(new ColumnInfo("Reynoldsnumber", BuiltInParameter.RBS_REYNOLDSNUMBER_PARAM));
                ti.Columns.Add(new ColumnInfo("Section", BuiltInParameter.RBS_SECTION));
                ti.Columns.Add(new ColumnInfo("SizeLock", BuiltInParameter.RBS_SIZE_LOCK));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Velocity", BuiltInParameter.RBS_VELOCITY));
                ti.Columns.Add(new ColumnInfo("VelocityPressure", BuiltInParameter.RBS_VELOCITY_PRESSURE));
                ti.Columns.Add(new ColumnInfo("Width", BuiltInParameter.RBS_CURVE_WIDTH_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_FlexPipeCurves, ElementType.INSTANCE);
            ti = new TableInfo("FlexPipes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AdditionalFlow", BuiltInParameter.RBS_PIPE_ADDITIONAL_FLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Diameter", BuiltInParameter.RBS_PIPE_DIAMETER_PARAM));
                ti.Columns.Add(new ColumnInfo("FixtureUnits", BuiltInParameter.RBS_PIPE_FIXTURE_UNITS_PARAM));
                ti.Columns.Add(new ColumnInfo("Flow", BuiltInParameter.RBS_PIPE_FLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("FlowState", BuiltInParameter.RBS_PIPE_FLOW_STATE_PARAM));
                ti.Columns.Add(new ColumnInfo("Friction", BuiltInParameter.RBS_PIPE_FRICTION_PARAM));
                ti.Columns.Add(new ColumnInfo("FrictionFactor", BuiltInParameter.RBS_PIPE_FRICTION_FACTOR_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("InnerDiameter", BuiltInParameter.RBS_PIPE_INNER_DIAM_PARAM));
                ti.Columns.Add(new ColumnInfo("InsulationThickness", BuiltInParameter.RBS_PIPE_INSULATION_THICKNESS));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("OuterDiameter", BuiltInParameter.RBS_PIPE_OUTER_DIAMETER));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("PressureDrop", BuiltInParameter.RBS_PIPE_PRESSUREDROP_PARAM));
                ti.Columns.Add(new ColumnInfo("RelativeRoughness", BuiltInParameter.RBS_PIPE_RELATIVE_ROUGHNESS_PARAM));
                ti.Columns.Add(new ColumnInfo("ReynoldsNumber", BuiltInParameter.RBS_PIPE_REYNOLDS_NUMBER_PARAM));
                ti.Columns.Add(new ColumnInfo("Section", BuiltInParameter.RBS_SECTION));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Velocity", BuiltInParameter.RBS_PIPE_VELOCITY_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Floors, ElementType.INSTANCE);
            ti = new TableInfo("Floors", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.HOST_AREA_COMPUTED));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("EstimatedReinforcementVolume", BuiltInParameter.REIN_EST_BAR_VOLUME));
                ti.Columns.Add(new ColumnInfo("HeightOffsetFromLevel", BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Perimeter", BuiltInParameter.HOST_PERIMETER_COMPUTED));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("Structural", BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL));
                ti.Columns.Add(new ColumnInfo("StructuralUsage", BuiltInParameter.STRUCTURAL_FLOOR_ANALYZES_AS));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.HOST_VOLUME_COMPUTED));
            }
            filter = new Filter(BuiltInCategory.OST_Furniture, ElementType.INSTANCE);
            ti = new TableInfo("Furniture", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_FurnitureSystems, ElementType.INSTANCE);
            ti = new TableInfo("FurnitureSystems", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_GenericModel, ElementType.INSTANCE);
            ti = new TableInfo("GenericModels", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Gutter, ElementType.SYMBOL);
            ti = new TableInfo("GutterTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Material", BuiltInParameter.MATERIAL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Profile", BuiltInParameter.GUTTER_PROFILE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_LightingDevices, ElementType.INSTANCE);
            ti = new TableInfo("LightingDevices", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElectricalData", BuiltInParameter.RBS_ELECTRICAL_DATA));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("SwitchID", BuiltInParameter.RBS_ELEC_SWITCH_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_LightingFixtures, ElementType.INSTANCE);
            ti = new TableInfo("LightingFixtures", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("CoefficientofUtilization", BuiltInParameter.RBS_ROOM_COEFFICIENT_UTILIZATION));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElectricalData", BuiltInParameter.RBS_ELECTRICAL_DATA));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("SwitchID", BuiltInParameter.RBS_ELEC_SWITCH_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_MechanicalEquipment, ElementType.INSTANCE);
            ti = new TableInfo("MechanicalEquipment", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_NurseCallDevices, ElementType.INSTANCE);
            ti = new TableInfo("NurseCallDevices", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElectricalData", BuiltInParameter.RBS_ELECTRICAL_DATA));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Parking, ElementType.INSTANCE);
            ti = new TableInfo("Parking", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_PipeAccessory, ElementType.INSTANCE);
            ti = new TableInfo("PipeAccessories", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("Size", BuiltInParameter.RBS_CALCULATED_SIZE));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_PipeFitting, ElementType.INSTANCE);
            ti = new TableInfo("PipeFittings", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.HOST_AREA_COMPUTED));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("Size", BuiltInParameter.RBS_CALCULATED_SIZE));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.HOST_VOLUME_COMPUTED));
            }
            filter = new Filter(BuiltInCategory.OST_PipeCurves, ElementType.INSTANCE);
            ti = new TableInfo("Pipes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AdditionalFlow", BuiltInParameter.RBS_PIPE_ADDITIONAL_FLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Diameter", BuiltInParameter.RBS_PIPE_DIAMETER_PARAM));
                ti.Columns.Add(new ColumnInfo("FixtureUnits", BuiltInParameter.RBS_PIPE_FIXTURE_UNITS_PARAM));
                ti.Columns.Add(new ColumnInfo("Flow", BuiltInParameter.RBS_PIPE_FLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("FlowState", BuiltInParameter.RBS_PIPE_FLOW_STATE_PARAM));
                ti.Columns.Add(new ColumnInfo("Friction", BuiltInParameter.RBS_PIPE_FRICTION_PARAM));
                ti.Columns.Add(new ColumnInfo("FrictionFactor", BuiltInParameter.RBS_PIPE_FRICTION_FACTOR_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("InnerDiameter", BuiltInParameter.RBS_PIPE_INNER_DIAM_PARAM));
                ti.Columns.Add(new ColumnInfo("InsulationThickness", BuiltInParameter.RBS_PIPE_INSULATION_THICKNESS));
                ti.Columns.Add(new ColumnInfo("InvertElevation", BuiltInParameter.RBS_PIPE_INVERT_ELEVATION));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("OuterDiameter", BuiltInParameter.RBS_PIPE_OUTER_DIAMETER));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("PressureDrop", BuiltInParameter.RBS_PIPE_PRESSUREDROP_PARAM));
                ti.Columns.Add(new ColumnInfo("RelativeRoughness", BuiltInParameter.RBS_PIPE_RELATIVE_ROUGHNESS_PARAM));
                ti.Columns.Add(new ColumnInfo("ReynoldsNumber", BuiltInParameter.RBS_PIPE_REYNOLDS_NUMBER_PARAM));
                ti.Columns.Add(new ColumnInfo("Section", BuiltInParameter.RBS_SECTION));
                ti.Columns.Add(new ColumnInfo("Size", BuiltInParameter.RBS_CALCULATED_SIZE));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Velocity", BuiltInParameter.RBS_PIPE_VELOCITY_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Planting, ElementType.INSTANCE);
            ti = new TableInfo("Planting", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_PlumbingFixtures, ElementType.INSTANCE);
            ti = new TableInfo("PlumbingFixtures", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Drain", BuiltInParameter.PLUMBING_FIXTURES_DRAIN));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("SupplyFitting", BuiltInParameter.PLUMBING_FIXTURES_SUPPLY_FITTING));
                ti.Columns.Add(new ColumnInfo("SupplyPipe", BuiltInParameter.PLUMBING_FIXTURES_SUPPLY_PIPE));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("Trap", BuiltInParameter.PLUMBING_FIXTURES_TRAP));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_SiteProperty, ElementType.INSTANCE);
            ti = new TableInfo("PropertyLines", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.PROPERTY_AREA));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Name", BuiltInParameter.ROOM_NAME));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_StairsRailing, ElementType.INSTANCE);
            ti = new TableInfo("Railings", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("BaseLevel", BuiltInParameter.STAIRS_RAILING_BASE_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("BaseOffset", BuiltInParameter.STAIRS_RAILING_HEIGHT_OFFSET));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Ramps, ElementType.INSTANCE);
            ti = new TableInfo("Ramps", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("BaseLevel", BuiltInParameter.STAIRS_BASE_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("BaseOffset", BuiltInParameter.STAIRS_BASE_OFFSET));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("MultistoryTopLevel", BuiltInParameter.STAIRS_MULTISTORY_TOP_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TopLevel", BuiltInParameter.STAIRS_TOP_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("TopOffset", BuiltInParameter.STAIRS_TOP_OFFSET));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Width", BuiltInParameter.STAIRS_ATTR_TREAD_WIDTH));
            }
            filter = new Filter(BuiltInCategory.OST_Roofs, ElementType.INSTANCE);
            ti = new TableInfo("Roofs", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.HOST_AREA_COMPUTED));
                ti.Columns.Add(new ColumnInfo("BaseLevel", BuiltInParameter.ROOF_BASE_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("BaseOffsetFromLevel", BuiltInParameter.ROOF_LEVEL_OFFSET_PARAM));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("CutoffLevel", BuiltInParameter.ROOF_UPTO_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("CutoffOffset", BuiltInParameter.ROOF_UPTO_LEVEL_OFFSET_PARAM));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("FasciaDepth", BuiltInParameter.FASCIA_DEPTH_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("RafterCut", BuiltInParameter.ROOF_EAVE_CUT_PARAM));
                ti.Columns.Add(new ColumnInfo("RafterorTruss", BuiltInParameter.ROOF_RAFTER_OR_TRUSS_TEXT_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.HOST_VOLUME_COMPUTED));
            }
            filter = new Filter(BuiltInCategory.OST_Rooms, ElementType.INSTANCE);
            ti = new TableInfo("Rooms", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("ActualExhaustAirflow", BuiltInParameter.ROOM_ACTUAL_EXHAUST_AIRFLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("ActualHVACLoad", BuiltInParameter.ROOM_ACTUAL_MECHANICAL_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("ActualLightingLoad", BuiltInParameter.ROOM_ACTUAL_LIGHTING_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("ActualLightingLoadperarea", BuiltInParameter.ROOM_ACTUAL_LIGHTING_LOAD_PER_AREA_PARAM));
                ti.Columns.Add(new ColumnInfo("ActualOtherLoad", BuiltInParameter.ROOM_ACTUAL_OTHER_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("ActualPowerLoad", BuiltInParameter.ROOM_ACTUAL_POWER_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("ActualPowerLoadperarea", BuiltInParameter.ROOM_ACTUAL_POWER_LOAD_PER_AREA_PARAM));
                ti.Columns.Add(new ColumnInfo("ActualReturnAirflow", BuiltInParameter.ROOM_ACTUAL_RETURN_AIRFLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("ActualSupplyAirflow", BuiltInParameter.ROOM_ACTUAL_SUPPLY_AIRFLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.ROOM_AREA));
                ti.Columns.Add(new ColumnInfo("Areaperperson", BuiltInParameter.ROOM_AREA_PER_PERSON_PARAM));
                ti.Columns.Add(new ColumnInfo("AverageEstimatedIllumination", BuiltInParameter.RBS_ELEC_ROOM_AVERAGE_ILLUMINATION));
                ti.Columns.Add(new ColumnInfo("BaseFinish", BuiltInParameter.ROOM_FINISH_BASE));
                ti.Columns.Add(new ColumnInfo("CalculatedCoolingLoad", BuiltInParameter.ROOM_CALCULATED_COOLING_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("CalculatedHeatingLoad", BuiltInParameter.ROOM_CALCULATED_HEATING_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("CalculatedSupplyAirflow", BuiltInParameter.ROOM_CALCULATED_SUPPLY_AIRFLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("CeilingFinish", BuiltInParameter.ROOM_FINISH_CEILING));
                ti.Columns.Add(new ColumnInfo("CeilingReflectance", BuiltInParameter.RBS_ELEC_ROOM_REFLECTIVITY_CEILING));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("ConditionType", BuiltInParameter.ROOM_CONDITION_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("Department", BuiltInParameter.ROOM_DEPARTMENT));
                ti.Columns.Add(new ColumnInfo("DesignCoolingLoad", BuiltInParameter.ROOM_DESIGN_COOLING_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("DesignHeatingLoad", BuiltInParameter.ROOM_DESIGN_HEATING_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("DesignHVACLoadperarea", BuiltInParameter.ROOM_DESIGN_MECHANICAL_LOAD_PER_AREA_PARAM));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("DesignOtherLoadperarea", BuiltInParameter.ROOM_DESIGN_OTHER_LOAD_PER_AREA_PARAM));
                ti.Columns.Add(new ColumnInfo("FloorFinish", BuiltInParameter.ROOM_FINISH_FLOOR));
                ti.Columns.Add(new ColumnInfo("FloorReflectance", BuiltInParameter.RBS_ELEC_ROOM_REFLECTIVITY_FLOOR));
                ti.Columns.Add(new ColumnInfo("HeatLoadValues", BuiltInParameter.ROOM_BASE_HEAT_LOAD_ON_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("LatentHeatGainperperson", BuiltInParameter.ROOM_PEOPLE_LATENT_HEAT_GAIN_PER_PERSON_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.ROOM_LEVEL_ID));
                ti.Columns.Add(new ColumnInfo("LightingCalculationWorkplane", BuiltInParameter.RBS_ELEC_ROOM_LIGHTING_CALC_WORKPLANE));
                ti.Columns.Add(new ColumnInfo("Name", BuiltInParameter.ROOM_NAME));
                ti.Columns.Add(new ColumnInfo("Number", BuiltInParameter.ROOM_NUMBER));
                ti.Columns.Add(new ColumnInfo("NumberofPeople", BuiltInParameter.ROOM_NUMBER_OF_PEOPLE_PARAM));
                ti.Columns.Add(new ColumnInfo("Occupancy", BuiltInParameter.ROOM_OCCUPANCY));
                ti.Columns.Add(new ColumnInfo("Perimeter", BuiltInParameter.ROOM_PERIMETER));
                ti.Columns.Add(new ColumnInfo("PhaseId", BuiltInParameter.ROOM_PHASE_ID));
                ti.Columns.Add(new ColumnInfo("RoomCavityRatio", BuiltInParameter.RBS_ELEC_ROOM_CAVITY_RATIO));
                ti.Columns.Add(new ColumnInfo("RoomConstruction", BuiltInParameter.ROOM_CONSTRUCTION_SET_PARAM));
                ti.Columns.Add(new ColumnInfo("RoomService", BuiltInParameter.ROOM_SERVICE_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("RoomType", BuiltInParameter.ROOM_SPACE_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("SensibleHeatGainperperson", BuiltInParameter.ROOM_PEOPLE_SENSIBLE_HEAT_GAIN_PER_PERSON_PARAM));
                ti.Columns.Add(new ColumnInfo("SpecifiedExhaustAirflow", BuiltInParameter.ROOM_DESIGN_EXHAUST_AIRFLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("SpecifiedLightingLoad", BuiltInParameter.ROOM_DESIGN_LIGHTING_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("SpecifiedLightingLoadperarea", BuiltInParameter.ROOM_DESIGN_LIGHTING_LOAD_PER_AREA_PARAM));
                ti.Columns.Add(new ColumnInfo("SpecifiedPowerLoad", BuiltInParameter.ROOM_DESIGN_POWER_LOAD_PARAM));
                ti.Columns.Add(new ColumnInfo("SpecifiedPowerLoadperarea", BuiltInParameter.ROOM_DESIGN_POWER_LOAD_PER_AREA_PARAM));
                ti.Columns.Add(new ColumnInfo("SpecifiedReturnAirflow", BuiltInParameter.ROOM_DESIGN_RETURN_AIRFLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("SpecifiedSupplyAirflow", BuiltInParameter.ROOM_DESIGN_SUPPLY_AIRFLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("TotalHeatGainperperson", BuiltInParameter.ROOM_PEOPLE_TOTAL_HEAT_GAIN_PER_PERSON_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.ROOM_VOLUME));
                ti.Columns.Add(new ColumnInfo("WallFinish", BuiltInParameter.ROOM_FINISH_WALL));
                ti.Columns.Add(new ColumnInfo("WallReflectance", BuiltInParameter.RBS_ELEC_ROOM_REFLECTIVITY_WALLS));
            }
            filter = new Filter(BuiltInCategory.OST_SecurityDevices, ElementType.INSTANCE);
            ti = new TableInfo("SecurityDevices", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElectricalData", BuiltInParameter.RBS_ELECTRICAL_DATA));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Site, ElementType.INSTANCE);
            ti = new TableInfo("Site", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_EdgeSlab, ElementType.SYMBOL);
            ti = new TableInfo("SlabEdgeTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Material", BuiltInParameter.MATERIAL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Profile", BuiltInParameter.SLAB_EDGE_PROFILE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_SpecialityEquipment, ElementType.INSTANCE);
            ti = new TableInfo("SpecialtyEquipment", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Sprinklers, ElementType.INSTANCE);
            ti = new TableInfo("Sprinklers", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Flow", BuiltInParameter.RBS_PIPE_FLOW_PARAM));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("PressureDrop", BuiltInParameter.RBS_PIPE_PRESSUREDROP_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemName", BuiltInParameter.RBS_SYSTEM_NAME_PARAM));
                ti.Columns.Add(new ColumnInfo("SystemType", BuiltInParameter.RBS_SYSTEM_TYPE_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Stairs, ElementType.INSTANCE);
            ti = new TableInfo("Stairs", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("ActualNumberofRisers", BuiltInParameter.STAIRS_ACTUAL_NUM_RISERS));
                ti.Columns.Add(new ColumnInfo("ActualRiserHeight", BuiltInParameter.STAIRS_ACTUAL_RISER_HEIGHT));
                ti.Columns.Add(new ColumnInfo("BaseLevel", BuiltInParameter.STAIRS_BASE_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("BaseOffset", BuiltInParameter.STAIRS_BASE_OFFSET));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("MultistoryTopLevel", BuiltInParameter.STAIRS_MULTISTORY_TOP_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TopLevel", BuiltInParameter.STAIRS_TOP_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("TopOffset", BuiltInParameter.STAIRS_TOP_OFFSET));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Width", BuiltInParameter.STAIRS_ATTR_TREAD_WIDTH));
            }
            filter = new Filter(BuiltInCategory.OST_StructuralColumns, ElementType.INSTANCE);
            ti = new TableInfo("StructuralColumns", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AnalyzeAs", BuiltInParameter.STRUCTURAL_ANALYZES_AS));
                ti.Columns.Add(new ColumnInfo("BaseLevel", BuiltInParameter.FAMILY_BASE_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("BaseOffset", BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("EstimatedReinforcementVolume", BuiltInParameter.REIN_EST_BAR_VOLUME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.INSTANCE_LENGTH_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TopLevel", BuiltInParameter.FAMILY_TOP_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("TopOffset", BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.HOST_VOLUME_COMPUTED));
            }
            filter = new Filter(BuiltInCategory.OST_StructuralFoundation, ElementType.INSTANCE);
            ti = new TableInfo("StructuralFoundations", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElevationatBottom", BuiltInParameter.STRUCTURAL_FOOTING_BOTTOM_ELEVATION));
                ti.Columns.Add(new ColumnInfo("EstimatedReinforcementVolume", BuiltInParameter.REIN_EST_BAR_VOLUME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CONTINUOUS_FOOTING_LENGTH));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.HOST_VOLUME_COMPUTED));
                ti.Columns.Add(new ColumnInfo("Width", BuiltInParameter.CONTINUOUS_FOOTING_WIDTH));
            }
            filter = new Filter(BuiltInCategory.OST_StructuralFraming, ElementType.INSTANCE);
            ti = new TableInfo("StructuralFraming", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("CutLength", BuiltInParameter.STRUCTURAL_FRAME_CUT_LENGTH));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("EstimatedReinforcementVolume", BuiltInParameter.REIN_EST_BAR_VOLUME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.INSTANCE_LENGTH_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("ReferenceLevel", BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("StructuralUsage", BuiltInParameter.INSTANCE_STRUCT_USAGE_TEXT_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.HOST_VOLUME_COMPUTED));
            }
            filter = new Filter(BuiltInCategory.OST_Rebar, ElementType.INSTANCE);
            ti = new TableInfo("StructuralRebar", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("BarDiameter", BuiltInParameter.REBAR_BAR_DIAMETER));
                ti.Columns.Add(new ColumnInfo("BarLength", BuiltInParameter.REBAR_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("BendDiameter", BuiltInParameter.REBAR_BAR_BEND_DIAMETER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("Quantity", BuiltInParameter.REBAR_ELEM_QUANTITY_OF_BARS));
                ti.Columns.Add(new ColumnInfo("Spacing", BuiltInParameter.REBAR_ELEM_BAR_SPACING));
                ti.Columns.Add(new ColumnInfo("TotalBarLength", BuiltInParameter.REBAR_ELEM_TOTAL_LENGTH));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_StructuralStiffener, ElementType.INSTANCE);
            ti = new TableInfo("StructuralStiffeners", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_StructuralTruss, ElementType.INSTANCE);
            ti = new TableInfo("StructuralTrusses", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_TelephoneDevices, ElementType.INSTANCE);
            ti = new TableInfo("TelephoneDevices", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitNumber", BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("ElectricalData", BuiltInParameter.RBS_ELECTRICAL_DATA));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Topography, ElementType.INSTANCE);
            ti = new TableInfo("Topography", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("Cut", BuiltInParameter.VOLUME_CUT));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Fill", BuiltInParameter.VOLUME_FILL));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("Name", BuiltInParameter.ROOM_NAME));
                ti.Columns.Add(new ColumnInfo("Netcut/fill", BuiltInParameter.VOLUME_NET));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("ProjectedArea", BuiltInParameter.PROJECTED_SURFACE_AREA));
                ti.Columns.Add(new ColumnInfo("SurfaceArea", BuiltInParameter.SURFACE_AREA));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }   
            filter = new Filter(BuiltInCategory.OST_ElectricalVoltage, ElementType.INSTANCE);
            ti = new TableInfo("Voltages", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Walls, ElementType.INSTANCE);
            ti = new TableInfo("Walls", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Area", BuiltInParameter.HOST_AREA_COMPUTED));
                ti.Columns.Add(new ColumnInfo("BaseConstraint", BuiltInParameter.WALL_BASE_CONSTRAINT));
                ti.Columns.Add(new ColumnInfo("BaseOffset", BuiltInParameter.WALL_BASE_OFFSET));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("EstimatedReinforcementVolume", BuiltInParameter.REIN_EST_BAR_VOLUME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("RoomBounding", BuiltInParameter.WALL_ATTR_ROOM_BOUNDING));
                ti.Columns.Add(new ColumnInfo("StructuralUsage", BuiltInParameter.WALL_STRUCTURAL_USAGE_TEXT_PARAM));
                ti.Columns.Add(new ColumnInfo("TopConstraint", BuiltInParameter.WALL_HEIGHT_TYPE));
                ti.Columns.Add(new ColumnInfo("TopOffset", BuiltInParameter.WALL_TOP_OFFSET));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("UnconnectedHeight", BuiltInParameter.WALL_USER_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.HOST_VOLUME_COMPUTED));
            }
            filter = new Filter(BuiltInCategory.OST_Cornices, ElementType.SYMBOL);
            ti = new TableInfo("WallSweepTypes", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("AssemblyCode", BuiltInParameter.UNIFORMAT_CODE));
                ti.Columns.Add(new ColumnInfo("Cost", BuiltInParameter.ALL_MODEL_COST));
                ti.Columns.Add(new ColumnInfo("Description", BuiltInParameter.ALL_MODEL_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("FamilyName", BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Keynote", BuiltInParameter.KEYNOTE_PARAM));
                ti.Columns.Add(new ColumnInfo("Manufacturer", BuiltInParameter.ALL_MODEL_MANUFACTURER));
                ti.Columns.Add(new ColumnInfo("Material", BuiltInParameter.MATERIAL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Model", BuiltInParameter.ALL_MODEL_MODEL));
                ti.Columns.Add(new ColumnInfo("Profile", BuiltInParameter.WALL_SWEEP_PROFILE_PARAM));
                ti.Columns.Add(new ColumnInfo("SubcategoryofWalls", BuiltInParameter.WALL_SWEEP_WALL_SUBCATEGORY_ID));
                ti.Columns.Add(new ColumnInfo("TypeComments", BuiltInParameter.ALL_MODEL_TYPE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("TypeMark", BuiltInParameter.ALL_MODEL_TYPE_MARK));
                ti.Columns.Add(new ColumnInfo("TypeName", BuiltInParameter.ALL_MODEL_TYPE_NAME));
                ti.Columns.Add(new ColumnInfo("URL", BuiltInParameter.ALL_MODEL_URL));
            }
            filter = new Filter(BuiltInCategory.OST_Windows, ElementType.INSTANCE);
            ti = new TableInfo("Windows", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("HeadHeight", BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("HostId", BuiltInParameter.HOST_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Level", BuiltInParameter.FAMILY_LEVEL_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("SillHeight", BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Wire, ElementType.INSTANCE);
            ti = new TableInfo("Wires", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("CircuitDescription", BuiltInParameter.RBS_WIRE_CIRCUIT_DESCRIPTION));
                ti.Columns.Add(new ColumnInfo("CircuitLoadName", BuiltInParameter.RBS_WIRE_CIRCUIT_LOAD_NAME));
                ti.Columns.Add(new ColumnInfo("Circuits", BuiltInParameter.RBS_ELEC_WIRE_CIRCUITS));
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("GroundConductors", BuiltInParameter.RBS_ELEC_WIRE_GROUND_ADJUSTMENT));
                ti.Columns.Add(new ColumnInfo("HotConductors", BuiltInParameter.RBS_ELEC_WIRE_HOT_ADJUSTMENT));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("NeutralConductors", BuiltInParameter.RBS_ELEC_WIRE_NEUTRAL_ADJUSTMENT));
                ti.Columns.Add(new ColumnInfo("NumberofConductors", BuiltInParameter.RBS_WIRE_NUM_CONDUCTORS_PARAM));
                ti.Columns.Add(new ColumnInfo("Panel", BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TickMarks", BuiltInParameter.RBS_ELEC_WIRE_TICKMARK_STATE));
                ti.Columns.Add(new ColumnInfo("Type", BuiltInParameter.RBS_ELEC_WIRE_TYPE));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("WireSize", BuiltInParameter.RBS_ELEC_CIRCUIT_WIRE_SIZE_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Fascia, ElementType.INSTANCE);
            ti = new TableInfo("Fascias", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("Type", BuiltInParameter.RBS_ELEC_WIRE_TYPE));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            }
            filter = new Filter(BuiltInCategory.OST_Gutter, ElementType.INSTANCE);
            ti = new TableInfo("Gutters", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("Type", BuiltInParameter.RBS_ELEC_WIRE_TYPE));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            } 
            filter = new Filter(BuiltInCategory.OST_EdgeSlab, ElementType.INSTANCE);
            ti = new TableInfo("SlabEdges", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Volume", BuiltInParameter.HOST_VOLUME_COMPUTED));
            }
            filter = new Filter(BuiltInCategory.OST_Cornices, ElementType.INSTANCE);
            ti = new TableInfo("WallSweeps", filter);
            {
                s_tableInfoSet.Add(ti);
                s_filterTableInfoMap.Add(filter, ti);
                ti.Columns.Add(new ColumnInfo("Comments", BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                ti.Columns.Add(new ColumnInfo("DesignOption", BuiltInParameter.DESIGN_OPTION_ID));
                ti.Columns.Add(new ColumnInfo("Id", BuiltInParameter.ID_PARAM));
                ti.Columns.Add(new ColumnInfo("Length", BuiltInParameter.CURVE_ELEM_LENGTH));
                ti.Columns.Add(new ColumnInfo("Mark", BuiltInParameter.ALL_MODEL_MARK));
                ti.Columns.Add(new ColumnInfo("PhaseCreated", BuiltInParameter.PHASE_CREATED));
                ti.Columns.Add(new ColumnInfo("PhaseDemolished", BuiltInParameter.PHASE_DEMOLISHED));
                ti.Columns.Add(new ColumnInfo("TypeId", BuiltInParameter.SYMBOL_ID_PARAM));
            } 

        }
    }
}
