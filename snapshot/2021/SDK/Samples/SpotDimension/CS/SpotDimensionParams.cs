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
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.SpotDimension.CS
{
    /// <summary>
    /// this class is used to get some information 
    /// of SpotDimension's Parameters   
    /// </summary>
    public class SpotDimensionParams
    {
        const double ToFractionalInches = 0.08333333;               //const number to convert number to FractionalInches
        static List<string> s_elevationOrigin = new List<string>();   //list store information about Elevation origin
        static List<string> s_textOrientation = new List<string>();   //list store information about Text Orientation
        static List<string> s_indicator = new List<string>();         //list store information about s_indicator
        static List<string> s_topBottomValue = new List<string>();    //list store information about Top and Bottom Value
        static List<string> s_textBackground = new List<string>();    //list store information about Text background
        Document m_document;                                        //a reference to Revit's document

        /// <summary>
        /// static constructor used to initialize lists
        /// </summary>
        static SpotDimensionParams()
        {
            //add string elements to lists
            s_elevationOrigin.Add("Project");
            s_elevationOrigin.Add("Shared");
            s_elevationOrigin.Add("Relative");

            s_textOrientation.Add("Horizontal Above");
            s_textOrientation.Add("Horizontal Below");

            s_indicator.Add("Prefix");
            s_indicator.Add("Suffix");

            s_topBottomValue.Add("None");
            s_topBottomValue.Add("North / South");
            s_topBottomValue.Add("East / West");

            s_textBackground.Add("Opaque");
            s_textBackground.Add("Transparent");
        }

        /// <summary>
        /// a constructor of class SpotDimensionParams
        /// </summary>
        /// <param name="document">external command document of Revit</param>
        public SpotDimensionParams(Document document)
        {
            m_document = document;
        }

        /// <summary>
        /// get a datatable contains parameters'information of SpotDimension
        /// here, almost only get Type Parameters of SpotDimension
        /// </summary>
        /// <param name="spotDimension">the SpotDimension need to be dealt with</param>
        /// <returns>a DataTable store Parameter information</returns>
        public DataTable GetParameterTable(Autodesk.Revit.DB.SpotDimension spotDimension)
        {
            try
            {
                //check whether is null
                if (null == spotDimension)
                {
                    return null;
                }                
                
                //create an empty datatable
                DataTable parameterTable = CreateTable();

                //get DimensionType
                Autodesk.Revit.DB.DimensionType dimensionType = spotDimension.DimensionType;

                //begin to get Parameters and add them to a DataTable
                Parameter temporaryParam = null;
                string temporaryValue = "";

                #region Get all SpotDimension element parameters

                //string formatter
                string formatter = "#0.000";

                //Property Category of SpotDimension
                AddDataRow("Category", spotDimension.Category.Name, parameterTable);

                //Leader Arrowhead
                temporaryParam = 
                    dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_LEADER_ARROWHEAD);
                Autodesk.Revit.DB.ElementId elementId = temporaryParam.AsElementId();
                //if not found that element, add string "None" to DataTable
                if (-1 == elementId.IntegerValue)
                {
                    temporaryValue = "None";
                }
                else
                {
                    temporaryValue = m_document.GetElement(elementId).Name;
                }
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                //Leader Line Weight
                temporaryParam = dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_LINE_PEN);
                temporaryValue = temporaryParam.AsInteger().ToString();
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                //Leader Arrowhead Line Weight
                temporaryParam = 
                    dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_TICK_MARK_PEN);
                temporaryValue = temporaryParam.AsInteger().ToString();
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                //Symbol
                temporaryParam = dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_SYMBOL);
                elementId = temporaryParam.AsElementId();
                //if not found that element, add string "None" to DataTable
                if (-1 == elementId.IntegerValue)
                {
                    temporaryValue = "None";
                }
                else
                {
                    temporaryValue = m_document.GetElement(elementId).Name;
                }
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                //Text Size
                temporaryParam = dimensionType.get_Parameter(BuiltInParameter.TEXT_SIZE);
                temporaryValue = 
                    (temporaryParam.AsDouble() / ToFractionalInches).ToString(formatter) + "''";
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                //Text Offset from Leader
                temporaryParam = 
                    dimensionType.get_Parameter(BuiltInParameter.SPOT_TEXT_FROM_LEADER);
                temporaryValue = 
                    (temporaryParam.AsDouble() / ToFractionalInches).ToString(formatter) + "''";
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                //Text Offset from Symbol
                temporaryParam = 
                    dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_TEXT_HORIZ_OFFSET);
                temporaryValue = 
                    (temporaryParam.AsDouble() / ToFractionalInches).ToString(formatter) + "''";
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                //for Spot Coordinates, add some other Parameters 
                if ("Spot Coordinates" == spotDimension.Category.Name)
                {
                    //Coordinate Origin
                    temporaryParam = 
                        dimensionType.get_Parameter(BuiltInParameter.SPOT_COORDINATE_BASE);
                    temporaryValue = temporaryParam.AsInteger().ToString();
                    AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                    //Top Value
                    temporaryParam = 
                        dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_TOP_VALUE);
                    temporaryValue = s_topBottomValue[temporaryParam.AsInteger()];
                    AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);
                    
                    //Bottom Value
                    temporaryParam = 
                        dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_BOT_VALUE);
                    temporaryValue = s_topBottomValue[temporaryParam.AsInteger()];
                    AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                    //North / South s_indicator
                    temporaryParam = 
                        dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_IND_NS);
                    temporaryValue = temporaryParam.AsString();
                    AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                    //East / West s_indicator
                    temporaryParam = 
                        dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_IND_EW);
                    temporaryValue = temporaryParam.AsString();
                    AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);
                }
                //for Spot Elevation, add some other Parameters 
                else
                {
                    //Instance Parameter----Value
                    temporaryParam = 
                        spotDimension.get_Parameter(BuiltInParameter.DIM_VALUE_LENGTH);
                    temporaryValue = temporaryParam.AsDouble().ToString(formatter) + "'";
                    AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                    //Elevation Origin
                    temporaryParam = dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_BASE);
                    temporaryValue = s_elevationOrigin[temporaryParam.AsInteger()];
                    AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                    //Elevation s_indicator
                    temporaryParam = 
                        dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_IND_ELEVATION);
                    temporaryValue = temporaryParam.AsString();
                    AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);
                }

                //Text Orientation
                temporaryParam = 
                    dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_TEXT_ORIENTATION);
                temporaryValue = s_textOrientation[temporaryParam.AsInteger()];
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                //s_indicator as Prefix / Suffix
                temporaryParam = dimensionType.get_Parameter(BuiltInParameter.SPOT_ELEV_IND_TYPE);
                temporaryValue = s_indicator[temporaryParam.AsInteger()];
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                //Text Font
                temporaryParam = dimensionType.get_Parameter(BuiltInParameter.TEXT_FONT);
                temporaryValue = temporaryParam.AsString();
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                //Text Background
                temporaryParam = dimensionType.get_Parameter(BuiltInParameter.DIM_TEXT_BACKGROUND);
                temporaryValue = s_textBackground[temporaryParam.AsInteger()];
                AddDataRow(temporaryParam.Definition.Name, temporaryValue, parameterTable);

                #endregion
                return parameterTable;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message, "A error in Function 'GetParameterTable':");
                return null;
            }
        }

        /// <summary>
        /// Create an empty table with parameter's name column and value column
        /// </summary>
        /// <returns>a DataTable be initialized</returns>
        private DataTable CreateTable()
        {
            // Create a new DataTable.
            DataTable propDataTable = new DataTable("ParameterTable");

            // Create parameter column and add to the DataTable.
            DataColumn paraDataColumn = new DataColumn();
            paraDataColumn.DataType = System.Type.GetType("System.String");
            paraDataColumn.ColumnName = "Parameter";
            paraDataColumn.Caption = "Parameter";
            paraDataColumn.ReadOnly = true;
            // Add the column to the DataColumnCollection.
            propDataTable.Columns.Add(paraDataColumn);

            // Create value column and add to the DataTable.
            DataColumn valueDataColumn = new DataColumn();
            valueDataColumn.DataType = System.Type.GetType("System.String");
            valueDataColumn.ColumnName = "Value";
            valueDataColumn.Caption = "Value";
            valueDataColumn.ReadOnly = true;
            propDataTable.Columns.Add(valueDataColumn);

            return propDataTable;
        }

        /// <summary>
        /// add one row to datatable
        /// </summary>
        /// <param name="parameterName">name of parameter</param>
        /// <param name="Value">value of parameter</param>
        /// <param name="parameterTable">datatable to be added row</param>
        private void AddDataRow(string parameterName, string Value, DataTable parameterTable)
        {
            DataRow newRow = parameterTable.NewRow();
            newRow["Parameter"] = parameterName;
            newRow["Value"] = Value;
            parameterTable.Rows.Add(newRow);
        }
    }
}
