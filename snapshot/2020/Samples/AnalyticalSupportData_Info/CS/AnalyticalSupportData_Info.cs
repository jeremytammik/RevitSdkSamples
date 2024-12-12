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
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.AnalyticalSupportData_Info.CS
{
    /// <summary>
    /// get element's id and type information and its supported information.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
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
        public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData revit,
                                                              ref string message,
                                                              ElementSet elements)
        {
            // Set currently executable application to private variable m_revit
            m_revit = revit;

            ElementSet selectedElements = new ElementSet();
            foreach (ElementId elementId in m_revit.Application.ActiveUIDocument.Selection.GetElementIds())
            {
               selectedElements.Insert(m_revit.Application.ActiveUIDocument.Document.GetElement(elementId));
            }

            // get all the required information of selected elements and store them in a data table.
            m_elementInformation = StoreInformationInDataTable(selectedElements);

            // show UI
            AnalyticalSupportData_InfoForm displayForm = new AnalyticalSupportData_InfoForm(this);
            displayForm.ShowDialog();

            return Autodesk.Revit.UI.Result.Succeeded;
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
                // Get  
                AnalyticalModel analyticalModel = element.GetAnalyticalModel();
                if (null == analyticalModel) // skip no AnalyticalModel element
                {
                    continue;
                }

                DataRow newRow              = informationTable.NewRow(); 
                string idValue              = element.Id.IntegerValue.ToString();// store element Id value             
                string typeName             = "";                      // store element type name
                string[] supportInformation = GetSupportInformation(analyticalModel);// store support information
   
                // get element type information
                switch (element.GetType().Name)
                {
                case "WallFoundation":
                    WallFoundation wallFound = element as WallFoundation;  
                    ElementType wallFootSymbol =m_revit.Application.ActiveUIDocument.Document.GetElement(wallFound.GetTypeId()) as ElementType;// get element Type
                    typeName              = wallFootSymbol.Category.Name + ": " + wallFootSymbol.Name;
                    break;

                case "FamilyInstance":
                    FamilyInstance familyInstance = element as FamilyInstance;
                    FamilySymbol symbol = m_revit.Application.ActiveUIDocument.Document.GetElement(familyInstance.GetTypeId()) as FamilySymbol;
                    typeName                      = symbol.Family.Name + ": " + symbol.Name;
                    break;

                case "Floor":
                    Floor slab         = element as Floor;                    
                    FloorType slabType = m_revit.Application.ActiveUIDocument.Document.GetElement(slab.GetTypeId()) as FloorType; // get element type
                    typeName           = slabType.Category.Name + ": " + slabType.Name;
                    break;

                case "Wall":
                    Wall wall         = element as Wall;  
                    WallType wallType = m_revit.Application.ActiveUIDocument.Document.GetElement(wall.GetTypeId()) as WallType; // get element type
                    typeName          = wallType.Kind.ToString() + ": " + wallType.Name;
                    break;

                default:
                    break;
                }

                // set the relative information of current element into the table.
                newRow["Id"] = idValue;
                newRow["Element Type"] = typeName;
                newRow["Support Type"] = supportInformation[0];
                newRow["Remark"] = supportInformation[1];
                informationTable.Rows.Add(newRow);
            }

            return informationTable;
        }

        /// <summary>
        /// create a empty DataTable
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

            // Create a column which can note others information
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
        /// <param name="analyticalModel"> element's analytical model</param>
        /// <returns></returns>
        private string[] GetSupportInformation(AnalyticalModel analyticalModel)
        {
            // supportInformation[0] store supportType
            // supportInformation[1] store other informations
            string[] supportInformations = new string[2] { "", "" };

            IList<AnalyticalModelSupport> supports = analyticalModel.GetAnalyticalModelSupports();

            // "Supported" flag indicates if the Element is completely supported.
            // AnalyticalModel Support list keeps track of all supports.
            if (!analyticalModel.IsElementFullySupported())// judge if supported
            {
                if (0 == supports.Count)
                {
                    supportInformations[0] = "not supported";
                }
                else
                {
                    foreach (AnalyticalModelSupport support in supports)
                    {
                        supportInformations[0] = supportInformations[0] +
                                                             support.GetSupportType().ToString() + ", ";
                    }
               }
            }
            else
            {
                if (0 == supports.Count)
                {
                    supportInformations[1] = "supported but no more information";
                }
                else
                {
                    foreach (AnalyticalModelSupport support in supports)
                    {
                        supportInformations[0] = supportInformations[0] +
                                                 support.GetSupportType().ToString() + ", ";
                    }
                }
            }

            return supportInformations;
        }
    }
}
