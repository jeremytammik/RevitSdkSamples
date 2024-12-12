//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.Data;
using System.Collections;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;


namespace Revit.SDK.Samples.BarDescriptions.CS
{
    /// <summary>
    /// Iterates through the BarDescriptions in the project and displays the properties in a table. 
    /// Allow the table to be saved in an Excel .CSV file.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command: IExternalCommand
    {
        // this object contains information related to the external command
        ExternalCommandData m_revit = null;

        // an array list store all AreaReinforcements' ids in the project
        ArrayList m_areaReinforcementIdList = new ArrayList();

        // an data table store all required information
        System.Data.DataTable m_barDescriptions = new System.Data.DataTable("BarDescriptions");

        // an data view which filtrate data from m_barDescriptions data table 
        // according the given id of AreaReinforcement.
        DataView m_specificBarDescriptions = new DataView();

        // properties
        #region properties

        /// <summary>
        /// the ids of the AreaReinforcements in the project
        /// </summary>
        public ArrayList AreaReinforcementIds
        {
            get
            {
                return m_areaReinforcementIdList;
            }
        }

        /// <summary>
        /// all the BarDescriptions in the project
        /// </summary>
        public System.Data.DataTable AllBarDescriotions
        {
            get
            {
                return m_barDescriptions;
            }
        }

        /// <summary>
        /// the BarDescriotions of one appointed AreaReinforcement
        /// </summary>
        public DataView SpecificBarDescriptions
        {
            get
            {
                return m_specificBarDescriptions;
            }
        }

        #endregion

        ///<summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
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
            m_revit = revit;

            // store all BarDescriptions in m_barDescriptions table and 
            // store the AreaReinforcements id value in m_areaReinforcementIdList array
            if (!PrepareAllNeededData())
            {
                message = "Cannot found AreaReinforcement in Project";
                return Autodesk.Revit.UI.Result.Cancelled;
            }

            // show UI
            BarDescriptionsForm displayForm = new BarDescriptionsForm(this);
            displayForm.ShowDialog();

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// set one specific AreaReinforcemet id value as row filter condition 
        /// of data view m_specificBarDescriptions.
        /// </summary>
        /// <param name="areaReinforcementIdValue">
        /// an specific AreaReinforcemet id value 
        /// </param>
        public void SetViewRowFilterCondition(int areaReinforcementIdValue)
        {
            m_specificBarDescriptions.Table = m_barDescriptions;
            string rowFilterCondition = "areaReinforcementId = " + areaReinforcementIdValue;
            m_specificBarDescriptions.RowFilter = rowFilterCondition;
        }

        /// <summary>
        /// export the BarDescriptions to a Excel
        /// </summary>
        /// <param name="saveFileName"></param>
        public void ExportAllData(string saveFileName)
        {
            // CSV format data which store all information that need be exported.
            string exportData = "";

            // set table title
            string projectTitle = m_revit.Application.ActiveUIDocument.Document.Title;
            exportData += "Bar Descriotions of " + projectTitle + "\r\n";

            // set table head
            for (int i = 1; i < m_barDescriptions.Columns.Count; i++)
            {
                exportData += m_barDescriptions.Columns[i].Caption + ",";
            }

            exportData += "\r\n";

            // write into BarDescriptions
            int areaReinId = System.Int32.MinValue;
            for (int i = 0; i < m_barDescriptions.Rows.Count; i++)
            {
                int currentAreaReinId = int.Parse(m_barDescriptions.Rows[i][0].ToString());

                // AreaReinforment id has own row 
                if (currentAreaReinId != areaReinId)
                {
                    exportData += "AreaForcement Id: " + currentAreaReinId + "\r\n";
                    areaReinId = currentAreaReinId;
                }

                // write into current row's each column data 
                for (int j = 1; j < m_barDescriptions.Columns.Count; j++)
                {
                    exportData += m_barDescriptions.Rows[i][j].ToString() + ",";
                }
                exportData += "\r\n";
            }

            // save the information which is stored in exportData in  an Excel .CSV 
            if (exportData.Length > 0)
            {
                System.IO.StreamWriter exportFile = new System.IO.StreamWriter(saveFileName);
                exportFile.WriteLine(exportData);
                exportFile.Close();
            }
        }

        /// <summary>
        /// Iterates through all the BarDescriptions in the project.
        /// store these data in a data table m_barDescriptions and 
        /// store the AreaReinforcements id value in a array list m_areaReinforcementIdList
        /// </summary>
        private bool PrepareAllNeededData()
        {
            // reset the Columns of data table m_barDescriptions
            SetDataTableCloumn();

            AreaReinforcement tempAreaReinforcement = null;
            FilteredElementIterator i = (new FilteredElementCollector(m_revit.Application.ActiveUIDocument.Document)).OfClass(typeof(AreaReinforcement)).GetElementIterator();
            while (i.MoveNext())
            {
                tempAreaReinforcement = i.Current as AreaReinforcement;
                if (tempAreaReinforcement != null)
                {
                    // store all the AreaReinforcements id value in a array list m_areaReinforcementIdList
                    m_areaReinforcementIdList.Add(tempAreaReinforcement.Id.IntegerValue);

                    // store BarDescriptions in a data table m_barDescriptions
                    for (int j = 0; j < tempAreaReinforcement.NumBarDescriptions; j++)
                    {
                        BarDescription barDescription = tempAreaReinforcement.get_BarDescription(j);

                        SetCurrentBarDescriptionToTable(tempAreaReinforcement, barDescription);
                    }
                }
            }

            if (null == tempAreaReinforcement)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// insert current BarDescription, which is iterated through, into data table
        /// </summary>
        /// <param name="areaReinforcement"></param>
        /// <param name="barDescription"></param>
        private void SetCurrentBarDescriptionToTable(AreaReinforcement areaReinforcement, 
                                                     BarDescription barDescription)
        {
            int areaReinforcementId = areaReinforcement.Id.IntegerValue;
            int layer               = barDescription.Layer;
            string barType          = barDescription.BarType.Name;
            double barLength        = barDescription.Length;
            string[] hookType       = new string[2]{"",""};
            bool hookSameDirection  = barDescription.HooksInSameDirection;
            int barCount            = barDescription.Count;

            for (int i = 0; i < 2; i++)
            {
                if (null == barDescription.get_HookType(i))
                {
                    hookType[i] = "None";
                }
                else
                {
                    hookType[i] = barDescription.get_HookType(i).Name;
                }
            }

            DataRow newRow                = m_barDescriptions.NewRow();
            newRow["areaReinforcementId"] = areaReinforcementId;
            newRow["layer"]               = layer;
            newRow["barType"]             = barType;
            newRow["barLength"]           = barLength;
            newRow["oneEndHookType"]      = hookType[0];
            newRow["otherEndHookType"]    = hookType[1];
            newRow["hookSameDirection"]   = hookSameDirection;
            newRow["barCount"]            = barCount;

            m_barDescriptions.Rows.Add(newRow);
        }

        /// <summary>
        /// edit the Columns of data table m_barDescriptions
        /// </summary>
        private void SetDataTableCloumn()
        {
            m_barDescriptions.Clear();
            m_barDescriptions.Columns.Clear();

            // add a column to store the id value of AreaReinforcement.
            DataColumn areaReinforcementId = new DataColumn("areaReinforcementId", typeof(int));
            areaReinforcementId.Caption    = "AreaReinforcement Id"; 
            areaReinforcementId.ReadOnly   = true;
            m_barDescriptions.Columns.Add(areaReinforcementId);

            // add a column to store current layer.
            DataColumn layer = new DataColumn("layer", typeof(int));
            layer.Caption    = "Layer";
            layer.ReadOnly   = true;
            m_barDescriptions.Columns.Add(layer);
 
            // add a column to store bar type name.
            DataColumn barType = new DataColumn("barType", typeof(string));
            barType.Caption    = "Bar Type";
            barType.ReadOnly   = true;
            m_barDescriptions.Columns.Add(barType);

            // add a column to store bar length.
            DataColumn barLength = new DataColumn("barLength", typeof(double));
            barLength.Caption    = "Bar Length (feet)";
            barLength.ReadOnly   = true;
            m_barDescriptions.Columns.Add(barLength);
            
            // add a column to store hook type in one end.
            DataColumn oneEndHookType = new DataColumn("oneEndHookType", typeof(string));
            oneEndHookType.Caption    = "One End Hook Type";
            oneEndHookType.ReadOnly   = true;
            m_barDescriptions.Columns.Add(oneEndHookType);

            // add a column to store hook type in other end.
            DataColumn otherEndHookType = new DataColumn("otherEndHookType", typeof(string));
            otherEndHookType.Caption    = "Other End Hook Type";
            otherEndHookType.ReadOnly   = true;
            m_barDescriptions.Columns.Add(otherEndHookType);

            // add a column to store if the both hooks are bent in the same direction.
            DataColumn hookSameDirection = new DataColumn("hookSameDirection", typeof(bool));
            hookSameDirection.Caption    = "Hook Same Direction";
            hookSameDirection.ReadOnly   = true;
            m_barDescriptions.Columns.Add(hookSameDirection);

            // add a column to store the number of bars of this shape.
            DataColumn barCount = new DataColumn("barCount", typeof(int));
            barCount.Caption    = "Bar Count";
            barCount.ReadOnly   = true;
            m_barDescriptions.Columns.Add(barCount);

        }
    }
}
