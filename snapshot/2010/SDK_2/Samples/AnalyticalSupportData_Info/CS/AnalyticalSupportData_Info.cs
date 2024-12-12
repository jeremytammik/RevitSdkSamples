//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Symbols;

namespace Revit.SDK.Samples.AnalyticalSupportData_Info.CS
{
    /// <summary>
    /// get element's id and type information and its supported information.
    /// </summary>
    public class Command: IExternalCommand
    {

        ExternalCommandData m_revit    = null;  // application of Revit
        DataTable m_elementInformation = null;  // store all required information

        /// <summary>
        /// property to get private member variable m_elementInformation.
        /// </summary>
        public DataTable ElementInformation
        {
            get
            {
                return m_elementInformation;
            }
        }

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData revit,
                                                              ref string message,
                                                              Autodesk.Revit.ElementSet elements)
        {
            // Set currently executable application to private variable m_revit
            m_revit = revit;

            ElementSet selectedElements = m_revit.Application.ActiveDocument.Selection.Elements;

            // get all the required information of selected elements and store them in a data table.
            m_elementInformation = StoreInformationInDataTable(selectedElements);

            // show UI
            AnalyticalSupportData_InfoForm displayForm = new AnalyticalSupportData_InfoForm(this);
            displayForm.ShowDialog();

            return IExternalCommand.Result.Succeeded;
        }

        /// <summary>
        /// get all the required information of selected elements and store them in a data table
        /// </summary>
        /// <param name="selectedElements">
        /// all selected elements in Revit main program
        /// </param>
        /// <returns>
        /// a data table which store all the required information
        /// </returns>
        private DataTable StoreInformationInDataTable(ElementSet selectedElements)
        {
            DataTable informationTable = CreatDataTable();
            
            foreach (Element element in selectedElements)
            {
                DataRow newRow                    = informationTable.NewRow(); 
                string idValue                    = element.Id.Value.ToString();// store element Id value             
                string typeName                   = "";                      // store element type name
                string[] supportInformation       = new string[2] { "", "" };// store support information
                AnalyticalSupportData supportData = null;               
                bool getInformationflag = false;//flag current element is one type in swith sentence

                switch (element.GetType().Name)
                {
                case "ContFooting":
                    ContFooting wallFoot  = element as ContFooting;  
                    Symbol wallFootSymbol = wallFoot.ObjectType;// get element Type
                    typeName              = wallFootSymbol.Category.Name + ": " + wallFootSymbol.Name;

                    // the type of wall foundation's AnalyticalModel is AnalyticalModel3D 
                    AnalyticalModel3D foundationModel = wallFoot.AnalyticalModel as AnalyticalModel3D;
                    if (null == foundationModel)
                    {
                        break;
                    }

                    // get element's support data
                    supportData        = foundationModel.SupportData;
                    getInformationflag = true;
                    break;

                case "FamilyInstance":
                    FamilyInstance familyInstance = element as FamilyInstance;                  
                    FamilySymbol symbol           = familyInstance.ObjectType as FamilySymbol;
                    typeName                      = symbol.Family.Name + ": " + symbol.Name;

                    // the type of some FamilyInstance's AnalyticalModel is AnalyticalModelFrame(eg, beam).
                    AnalyticalModelFrame frameModel = familyInstance.AnalyticalModel as AnalyticalModelFrame;
                    if (null != frameModel)
                    { 
                        supportData        = frameModel.SupportData;
                        getInformationflag = true;
                        break;
                    }

                    // the type of point foundation's AnalyticalModel is AnalyticalModelLocation
                    AnalyticalModelLocation locationModel = familyInstance.AnalyticalModel as AnalyticalModelLocation;
                    if (null != locationModel)
                    {
                        supportData        = locationModel.SupportData;
                        getInformationflag = true;
                    }

                    break;

                case "Floor":
                    Floor slab         = element as Floor;                    
                    FloorType slabType = slab.ObjectType as FloorType; // get element type
                    typeName           = slabType.Category.Name + ": " + slabType.Name;

                    // the type of Floor's AnalyticalModel is AnalyticalModelFloor
                    AnalyticalModelFloor slabModel = slab.AnalyticalModel as AnalyticalModelFloor;
                    if (null == slabModel)
                    {
                        break;
                    }

                    supportData        = slabModel.SupportData;
                    getInformationflag = true;
                    break; 
          
                case "Wall":
                    Wall wall         = element as Wall;  
                    WallType wallType = wall.ObjectType as WallType; // get element type
                    typeName          = wallType.Kind.ToString() + ": " + wallType.Name;

                    // the type of wall's AnalyticalModel is AnalyticalModelWall
                    AnalyticalModelWall wallModel = wall.AnalyticalModel as AnalyticalModelWall;
                    if (null == wallModel)
                    {
                        break;
                    }

                    supportData        = wallModel.SupportData;
                    getInformationflag = true;
                    break;

                default:
                    break;
                }
                
                // get support information
                if (true == getInformationflag)
                {
                    if (null != supportData)
                    {
                        supportInformation = GetSupportInformation(supportData);
                    }

                    // set the relative information of current element into the table.
                    newRow["Id"] = idValue;
                    newRow["Element Type"] = typeName;
                    newRow["Support Type"] = supportInformation[0];
                    newRow["Remark"] = supportInformation[1];
                    informationTable.Rows.Add(newRow);
                }
            }

            return informationTable;
        }

        /// <summary>
        /// create a empty dataTable
        /// </summary>
        /// <returns></returns>
        private DataTable CreatDataTable()
        {
            // Create a new DataTable.
            DataTable elementInformationTable = new DataTable("ElementInformationTable");

            // Create element id column and add to the DataTable.
            DataColumn idColumn = new DataColumn();
            idColumn.DataType   = typeof(System.String);
            idColumn.ColumnName = "Id";
            idColumn.Caption    = "Id";
            idColumn.ReadOnly   = true;
            elementInformationTable.Columns.Add(idColumn);

            // Create element type column and add to the DataTable.
            DataColumn typeColumn = new DataColumn();
            typeColumn.DataType   = typeof(System.String);
            typeColumn.ColumnName = "Element Type";
            typeColumn.Caption    = "Element Type";
            typeColumn.ReadOnly   = true;
            elementInformationTable.Columns.Add(typeColumn);

            // Create support column and add to the DataTable.
            DataColumn supportColumn = new DataColumn();
            supportColumn.DataType   = typeof(System.String);
            supportColumn.ColumnName = "Support Type";
            supportColumn.Caption    = "Support Type";
            supportColumn.ReadOnly   = true;
            elementInformationTable.Columns.Add(supportColumn);

            // Create a colum which can note others information
            DataColumn remarkColumn = new DataColumn();
            remarkColumn.DataType   = typeof(System.String);
            remarkColumn.ColumnName = "Remark";
            remarkColumn.Caption    = "Remark";
            remarkColumn.ReadOnly   = true;
            elementInformationTable.Columns.Add(remarkColumn);
          
            return elementInformationTable;
        }

        /// <summary>
        /// get element's support information
        /// </summary>
        /// <param name="supportData"> element's support data</param>
        /// <returns></returns>
        private string[] GetSupportInformation(AnalyticalSupportData supportData)
        {
            // supportInformation[0] store supportType
            // supportInformation[1] store other informations
            string[] supportInformations = new string[2] { "", "" };
             
            // "Supported" flag indicates if the Element is completely supported.
            // Analytical Support Info keeps track of all supports.
            AnalyticalSupportInfoArray inforArray = supportData.InfoArray;// get support type
            if (!supportData.Supported)// judge if supported
            {
                if (inforArray.IsEmpty)
                {
                    supportInformations[0] = "not supported";
                }
                else
                {
                    for (AnalyticalSupportInfoArrayIterator i = inforArray.ForwardIterator(); i.MoveNext(); )
                    {
                        AnalyticalSupportInfo supportInfor = i.Current as AnalyticalSupportInfo;
                        supportInformations[0]             = supportInformations[0] + 
                                                             supportInfor.SupportType.ToString() + ", ";
                    }
               }
            }
            else
            {
                if (inforArray.IsEmpty)
                {
                    supportInformations[1] = "supported but no more information";
                }
                else
                {
                    for (AnalyticalSupportInfoArrayIterator i = inforArray.ForwardIterator(); i.MoveNext(); )
                    {
                        AnalyticalSupportInfo supportInfor = i.Current as AnalyticalSupportInfo;
                        supportInformations[0]             = supportInformations[0] + 
                                                             supportInfor.SupportType.ToString() + ", ";
                    }
                }
            }

            return supportInformations;
        }
    }
}
