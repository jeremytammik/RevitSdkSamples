//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Data.OleDb;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Events;
using Autodesk.Revit.Collections;

namespace Revit.SDK.Samples.RoomSchedule
{
    /// <summary>
    /// Room Schedule form, used to retrieve data from .xls data source and create new rooms.
    /// </summary>
    public partial class RoomScheduleForm : Form
    {
        #region Class Memeber Variables

        // Reserve name of data source
        private String m_dataBaseName;

        // Revit external command data
        private ExternalCommandData m_commandData;

        // Room data information
        private RoomsData m_roomData;

        // Room shared parameter definition 
        private Definition m_roomSharedParamDef;

        // All levels in Revit document.
        private List<Level> m_allLevels = new List<Level>();

        // All available phases in Revit document.
        private List<Phase> m_allPhases = new List<Phase>();

        // Room work sheet name
        private String m_roomTableName;

        // All rooms data from spread sheet
        private DataTable m_spreadRoomsTable;
        #endregion


        #region Class Global Static Variables
        /// <summary>
        /// Array of documents and mapped Excel file and opened table. 
        /// The Document is document whose OnSave and OnSaveAs have been subscribed.
        /// </summary>
        private static Dictionary<Document, List<String>> s_EventSubDocs = new Dictionary<Document, List<String>>();
        
        /// <summary>
        /// The String array in s_EventSubDocs reserves the Excel file and table sample used, the list should only contain TWO String values;
        /// The 1st is Excel file path, the 2nd is table name of this Excel file, they are used by sample currently.
        /// We should reserve and update this String array when user select new Excel or new work sheet (table).
        /// Use them to update Excel work sheet when Save command is executed.
        /// </summary>
        private const int fixedArrayNum = 2;
        #endregion


        #region Class Constructor Method
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="commandData">Revit external command data</param>
        public RoomScheduleForm(ExternalCommandData commandData)
        {
            // UI initialization 
            InitializeComponent();

            // reserve Revit command data and get rooms information, 
            // and then display rooms information in DataGrideView
            m_commandData = commandData;
            m_roomData = new RoomsData(commandData);

            // bind levels and phases data to level and phase ComboBox controls
            GetAllLevelsAndPhases();

            // list all levels and phases
            this.levelComboBox.DisplayMember = "Name";
            this.levelComboBox.DataSource = m_allLevels;
            this.levelComboBox.SelectedIndex = 0;

            this.phaseComboBox.DisplayMember = "Name";
            this.phaseComboBox.DataSource = m_allPhases;
            this.phaseComboBox.SelectedIndex = 0;

            // if there is no phase, newRoomButton will be disabled.
            if (m_allPhases.Count == 0)
            {
                newRoomButton.Enabled = false;
            }

            // check to see whether current Revit document was mapped to spreadsheet.
            UpdateRoomSheetInfo();
        }
        #endregion


        #region Class Implementatation
        /// <summary>
        /// Get all available levels and phases from current document
        /// </summary>
        private void GetAllLevelsAndPhases()
        {
            // get all levels
            Autodesk.Revit.Document document = m_commandData.Application.ActiveDocument;
            foreach (PlanTopology planTopology in document.PlanTopologies)
            {
                m_allLevels.Add(planTopology.Level);
            }

            // get all phases
            ElementFilterIterator iter = m_commandData.Application.ActiveDocument.get_Elements(typeof(Phase));
            iter.Reset();
            while (iter.MoveNext())
            {
                Phase phase = iter.Current as Phase;
                if (phase != null)
                {
                    m_allPhases.Add(phase);
                }
            }
        }


        /// <summary>
        /// Create shared parameter for Rooms category
        /// </summary>
        /// <returns>True, shared parameter exists; false, doesn't exist</returns>
        private bool CreateMyRoomSharedParameter()
        {
            // Create Room Shared Parameter Routine: -->
            // 1: Check whether the Room shared parameter("External Room ID") has been defined.
            // 2: Share parameter file locates under sample directory of this .dll module.
            // 3: Add a group named "SDKSampleRoomScheduleGroup".
            // 4: Add a shared parameter named "External Room ID" to "Rooms" category, which is visible.
            //    The "External Room ID" parameter will be used to map to spreadsheet based room ID(which is unique)

            try
            {
                // check whether shared parameter exists, if exists, save the shared parameter definition for future use .
                if (ShareParameterExists(RoomsData.SharedParam, ref m_roomSharedParamDef))
                {
                    return true;
                }

                // create shared parameter file
                String modulePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                String paramFile = modulePath + "\\RoomScheduleSharedParameters.txt";
                if (File.Exists(paramFile))
                {
                    File.Delete(paramFile);
                }
                FileStream fs = File.Create(paramFile);
                fs.Close();

                // cache application handle
                Autodesk.Revit.Application revitApp = m_commandData.Application;

                // prepare shared parameter file
                m_commandData.Application.Options.SharedParametersFilename = paramFile;

                // open shared parameter file
                DefinitionFile parafile = revitApp.OpenSharedParameterFile();

                // create a group
                DefinitionGroup apiGroup = parafile.Groups.Create("SDKSampleRoomScheduleGroup");

                // create a visible "External Room ID" of text type.
                m_roomSharedParamDef = apiGroup.Definitions.Create(RoomsData.SharedParam,
                                                                         ParameterType.Text, true);

                // get Rooms category
                Category roomCat = revitApp.ActiveDocument.Settings.Categories.get_Item("Rooms");
                CategorySet categories = revitApp.Create.NewCategorySet();
                categories.Insert(roomCat);

                // insert the new parameter
                InstanceBinding binding = revitApp.Create.NewInstanceBinding(categories);
                revitApp.ActiveDocument.ParameterBindings.Insert(m_roomSharedParamDef, binding);
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create shared parameter: " + ex.Message);
            }
        }


        /// <summary>
        /// Test if the Room binds a specified shared parameter
        /// </summary>
        /// <param name="paramName">Parameter name to be checked</param>
        /// <param name="definition">The definition has been defined before</param>
        /// <returns>true, the definition exists, false, doesn't exist.</returns>
        private bool ShareParameterExists(String paramName, ref Definition definition)
        {
            definition = null;
            Autodesk.Revit.Document doc = m_commandData.Application.ActiveDocument;
            BindingMap bindingMap = doc.ParameterBindings;
            DefinitionBindingMapIterator iter = bindingMap.ForwardIterator();
            iter.Reset();

            while (iter.MoveNext())
            {
                Definition tempDefinition = iter.Key;

                // find the definition of which the name is the appointed one
                if (String.Compare(tempDefinition.Name, paramName) != 0)
                {
                    continue;
                }

                // get the category which is bound
                ElementBinding binding = bindingMap.get_Item(tempDefinition) as ElementBinding;
                CategorySet bindCategories = binding.Categories;
                foreach (Category category in bindCategories)
                {
                    if (category.Name == "Rooms")
                    {
                        // the definition with appointed name was bound to Rooms, return true and save this definition
                        definition = tempDefinition;
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// My custom message box 
        /// </summary>
        /// <param name="strMsg">message to be popped up</param>
        /// <param name="icon">icon to be displayed</param>
        private static void MyMessageBox(String strMsg, MessageBoxIcon icon)
        {
            MessageBox.Show(strMsg, "Room Schedule", MessageBoxButtons.OK, icon);
        }


        /// <summary>
        /// Update control display of form 
        /// call this method when create new rooms or switch the room show(show all or show by level)
        /// </summary>
        /// <param name="bUpdateAllRooms">whether retrieve all rooms from Revit project once more</param>
        private void UpdateFormDisplay(bool bUpdateAllRooms)
        {
            // update Revit Rooms data when there is room creation
            if (bUpdateAllRooms)
            {
                m_roomData.UpdateRoomsData();
            }

            revitRoomDataGridView.DataSource = null;
            if (showAllRoomsCheckBox.Checked)
            {
                // show all rooms in Revit project
                revitRoomDataGridView.DataSource = new DataView(m_roomData.GenRoomsDataTable(null));
            }
            else
            {
                // show all rooms in specified level
                levelComboBox_SelectedIndexChanged(null, null);
            }

            // update this DataGridView
            revitRoomDataGridView.Update();
        }


        /// <summary>
        /// Display current Room sheet information: Excel path
        /// </summary>
        private void UpdateRoomSheetInfo()
        {
            if (s_EventSubDocs.ContainsKey(m_commandData.Application.ActiveDocument))
            {
                List<String> xlsAndTable;
                s_EventSubDocs.TryGetValue(m_commandData.Application.ActiveDocument, out xlsAndTable);
                if (fixedArrayNum == xlsAndTable.Count)
                {
                    roomExcelTextBox.Text = "Mapped Sheet: " + xlsAndTable[0] + ": " + xlsAndTable[1];
                }
            }
        }


        /// <summary>
        /// Set shared parameter (whose name is "External Room ID") value to Room.Id.Value
        /// </summary>
        /// <param name="room">The room used to get the room which to be updated</param>
        private void SetExternalRoomIdToRoomId(Room room)
        {
            ElementId elemId = room.Id;
            Room updateRoom = m_commandData.Application.ActiveDocument.get_Element(ref elemId) as Room;
            foreach (Parameter sharedParam in room.Parameters)
            {
                if (String.Compare(sharedParam.Definition.Name, RoomsData.SharedParam) == 0)
                {
                    // It's safer to encapsulate shared parameter update in transaction.
                    m_commandData.Application.ActiveDocument.BeginTransaction();
                    sharedParam.Set(room.Id.Value.ToString());
                    m_commandData.Application.ActiveDocument.EndTransaction();
                    return; 
                }
            }
        }
        #endregion


        #region Revit DocuemntEvents Methods Implementation

        /// <summary>
        /// Revit document Save and SaveAs event method. It will be raised as soon as Save / SaveAs is called
        /// This method will update spread sheet room data([Area] column) with actual area value of mapped Revit Room.
        /// or add Revit room to spreadsheet if it is not mapped to room of spreadsheet.
        /// </summary>
        private void OnDocumentSave()
        {
            // OnDocumentSave Programming Routine:
            //
            // 1: Update spreadsheet when:
            //    a: there is room work sheet table;
            //    b: there is rooms data;
            //    c: shared parameter exists;
            //    d: user does want to update the spreadsheet Revit rooms map.
            // 2: Skip update and insert operations for below rooms:
            //    a: the rooms are not placed or located;
            //    b: the rooms whose shared parameter(defined by sample) are not retrieved,
            //       some rooms maybe don't have shared parameter at all, despite user create for Rooms category.
            // 3: Update spreadsheet rooms values by Revit room actual values.
            //    a: if shared parameter exists(is not null), update row by using this parameter's value;
            //    b: if shared parameter doesn't exist (is null), update row by Id value of room, which will avoid the duplicate 
            //       ID columns occur in spreadsheet.
            // 4: Insert Revit rooms data to spreadsheet if:
            //    a: failed to update values of rooms (maybe there no matched ID value in spread sheet rows).
            // 

            #region Check Whether Update Spreadsheet Data 
            //
            // check which table to be updated.
            List<String> mappedXlsAndTable;
            s_EventSubDocs.TryGetValue(m_commandData.Application.ActiveDocument, out mappedXlsAndTable);
            if (fixedArrayNum != mappedXlsAndTable.Count ||
                String.IsNullOrEmpty(mappedXlsAndTable[0]) || String.IsNullOrEmpty(mappedXlsAndTable[1])) 
            {
                return;
            }

            // retrieve all rooms in project(maybe there are new rooms created manually by user)
            m_roomData.UpdateRoomsData();
            if (m_roomData.Rooms.Count <= 0)
            {
                return;
            }

            // find the shared parameter definition
            Parameter externalIdSharedParam = null;
            String externalId = String.Empty;
            bool bExist = RoomsData.ShareParameterExists(m_roomData.Rooms[0], RoomsData.SharedParam, ref externalIdSharedParam);
            if (false == bExist)
            {
                return;
            }
            else
            {
                externalId = externalIdSharedParam.AsString();
            }

            // ask whether update the spread sheet data mapped to Revit rooms.
            if (MessageBox.Show("Revit rooms are mapped to spread sheet, do you want to update this sheet?",
                "Room Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            #endregion

            // create a connection and update values of spread sheet
            int updatedRows = 0; // number of rows which were updated
            int newRows = 0; // number of rows which were added into spread sheet
            XlsDBConnector dbConnectoor = new XlsDBConnector(mappedXlsAndTable[0]);

            // check whether there is room table. 
            // get all available rooms in current document once more
            foreach (Room room in m_roomData.Rooms)
            {
                #region Skip Operation For Some Rooms 
                //
                // skip directly if the room is not placed yet
                if (null == room.Location || null == room.Level)
                {
                    continue;
                }

                // get Area of room, if converting to double value fails, skip this. 
                // if the area is zero to less than zero, skip the update too
                double roomArea;
                try
                {
                    // get area without unit, then converting it to double will be ok.
                    String areaStr = m_roomData.GetProperty(room, BuiltInParameter.ROOM_AREA, false);
                    roomArea = Double.Parse(areaStr);
                    if (roomArea <= double.Epsilon)
                    {
                        continue;
                    }
                }
                catch
                {
                    // parse double value failed, continue the loop 
                    continue;
                }

                // get the shared parameter value of room
                bExist = RoomsData.ShareParameterExists(room, RoomsData.SharedParam, ref externalIdSharedParam);
                if (false == bExist)
                {
                    return;
                }
                else
                {
                    externalId = externalIdSharedParam.AsString();
                }
                #endregion

                // try to update  
                try
                {

                    #region Update Spreadsheet Room
                    
                    // flag used to indicate whether update is successful 
                    bool bUpdateFailed = false;

                    // get comments of room
                    String comments;
                    Parameter param = room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                    comments = (null != param) ? (param.AsString()) : ("");
                    if (String.IsNullOrEmpty(comments))
                    {
                        comments = "<null>";
                    }

                    // create update SQL clause, 
                    // when filtering row to be updated, use Room.Id.Value if "External Room ID" is null.
                    String updateStr = String.Format(
                        "Update [{0}$] SET [{1}] = '{2}', [{3}] = '{4}', [{5}] = '{6}', [{7}] = {8} Where {9} = {10}",
                        mappedXlsAndTable[1], // mapped table name
                        RoomsData.RoomName, room.Name,
                        RoomsData.RoomNumber, room.Number,
                        RoomsData.RoomComments, comments,
                        RoomsData.RoomArea, roomArea,
                        RoomsData.RoomID, String.IsNullOrEmpty(externalId) ? room.Id.Value.ToString() : externalId);

                    // execute the command and check the size of updated rows 
                    int afftectedRows = dbConnectoor.ExecuteCommnand(updateStr);
                    if (afftectedRows == 0)
                    {
                        bUpdateFailed = true;
                    }
                    else
                    {
                        // count how many rows were updated
                        updatedRows += afftectedRows;

                        // if "External Room ID" is null but update successfully, which means:
                        // in spreadsheet there is existing row whose "ID" value equals to room.Id.Value, so we should
                        // set Revit room's "External Room ID" value to Room.Id.Value for consistence after update .
                        if(String.IsNullOrEmpty(externalId)) 
                        {
                            SetExternalRoomIdToRoomId(room);
                        }
                    }
                    #endregion


                    #region Insert Revit Room

                    // Add this new room to spread sheet if fail to update spreadsheet 
                    if (bUpdateFailed)
                    {
                        // try to insert this new room to spread sheet, some rules:
                        // a: if the "External Room ID" exists, set ID column to this external id value, 
                        //    if the "External Room ID" doesn't exist, use the actual Revit room id as the ID column value.
                        // b: use comments in room if room's description exists,
                        //    else, use constant string: "<Added from Revit>" for Comments column in spreadsheet.

                        String insertStr =
                            String.Format("Insert Into [{0}$] ([{1}], [{2}], [{3}], [{4}], [{5}]) Values({6}, '{7}', '{8}', '{9}', {10})",
                            mappedXlsAndTable[1], // mapped table name
                            RoomsData.RoomID, RoomsData.RoomComments, RoomsData.RoomName, RoomsData.RoomNumber, RoomsData.RoomArea,
                            (String.IsNullOrEmpty(externalId)) ? (room.Id.Value.ToString()) : (externalId), // Room id
                            (String.IsNullOrEmpty(comments)) ? ("<Added from Revit>") : (comments),
                            room.Name, room.Number, roomArea);

                        // try to insert it 
                        afftectedRows = dbConnectoor.ExecuteCommnand(insertStr);
                        if (afftectedRows != 0)
                        {
                            // remember the number of new rows
                            newRows += afftectedRows;

                            // if the Revit room doesn't have external id value(may be a room created manually)
                            // set its "External Room ID" value to Room.Id.Value, because the room was added/mapped to spreadsheet, 
                            // and the value of ID column in sheet is just the Room.Id.Value, we should keep this consistence.
                            if (String.IsNullOrEmpty(externalId) && null != externalIdSharedParam)
                            {
                                SetExternalRoomIdToRoomId(room);
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    // close the connection 
                    dbConnectoor.CloseConnector();
                    MyMessageBox(ex.Message, MessageBoxIcon.Warning);
                    return;
                }
            }

            // close the connection 
            dbConnectoor.CloseConnector();

            // output the affected result message
            String message = String.Format("{0}: \r\n{1} rows were updated and {2} rows were added into successfully.",
                mappedXlsAndTable[0] + ": " + mappedXlsAndTable[1], updatedRows, newRows);
            MyMessageBox(message, MessageBoxIcon.Information);
        }


        /// <summary>
        /// Document close event handler
        /// Remove the document object which is to be closed
        /// </summary>
        void OnDocumentClose()
        {
            // remove current document from array of documents which have some events being subscribed 
            s_EventSubDocs.Remove(m_commandData.Application.ActiveDocument);
        }
        #endregion


        #region Class Events Implmentation
        /// <summary>
        /// Import room spread sheet and display them in form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importRoomButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog sfdlg = new OpenFileDialog())
            {
                // file dialog initialization 
                sfdlg.Title = "Import Excel File";
                sfdlg.Filter = "Excel File(*.xls)|*.xls";
                sfdlg.RestoreDirectory = true;
                if(s_EventSubDocs.ContainsKey(m_commandData.Application.ActiveDocument))
                {
                    List<String> xlsAndTable;
                    s_EventSubDocs.TryGetValue(m_commandData.Application.ActiveDocument, out xlsAndTable);
                    if (fixedArrayNum == xlsAndTable.Count)
                    {
                        sfdlg.FileName = xlsAndTable[0];
                    }
                }

                if (DialogResult.OK == sfdlg.ShowDialog())
                {
                    try
                    {
                        // create xls data source connector and retrieve data from it
                        m_dataBaseName = sfdlg.FileName;
                        XlsDBConnector xlsCon = new XlsDBConnector(m_dataBaseName);

                        // bind table data to grid view and ComboBox control
                        tablesComboBox.DataSource = xlsCon.RetrieveAllTables();

                        // close the connection
                        xlsCon.CloseConnector();
                    }
                    catch (Exception ex)
                    {
                        tablesComboBox.DataSource = null;
                        MyMessageBox(ex.Message, MessageBoxIcon.Warning);
                    }
                }
            }
        }


        /// <summary>
        /// Select one table(work sheet) and display its data to DataGridView control.
        /// after selection, generate data table from data source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tablesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update spread sheet based rooms
            sheetDataGridView.DataSource = null;
            m_roomTableName = tablesComboBox.SelectedValue as String;
            XlsDBConnector xlsCon = null;
            try
            {
                if (null != m_spreadRoomsTable)
                {
                    m_spreadRoomsTable.Clear();
                }

                // get all rooms table then close this connection immediately
                xlsCon = new XlsDBConnector(m_dataBaseName);

                // generate room data table from room work sheet.
                m_spreadRoomsTable = xlsCon.GenDataTable(m_roomTableName);
                newRoomButton.Enabled = (0 == m_spreadRoomsTable.Rows.Count) ? false : true;

                // close connection
                xlsCon.CloseConnector();

                // update data source of DataGridView
                sheetDataGridView.DataSource = new DataView(m_spreadRoomsTable);
            }
            catch (Exception ex)
            {
                // close connection and update data source
                xlsCon.CloseConnector();
                sheetDataGridView.DataSource = null;
                MyMessageBox(ex.Message, MessageBoxIcon.Warning);
                return;
            }

            // update the static s_EventSubDocs variable when user changes the Excel and room table
            if (s_EventSubDocs.ContainsKey(m_commandData.Application.ActiveDocument)) 
            {
                s_EventSubDocs.Remove(m_commandData.Application.ActiveDocument);
                List<String> xlsAndTable = new List<string>();
                xlsAndTable.Add(m_dataBaseName);
                xlsAndTable.Add(m_roomTableName);
                s_EventSubDocs.Add(m_commandData.Application.ActiveDocument, xlsAndTable);

                // update current mapped room sheet information, only show this when Revit rooms were mapped to Excel sheet.
                UpdateRoomSheetInfo();
            }
        }


        /// <summary>
        /// Filter rooms by specified level.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the selected level, by comparing its name and ComboBox selected item's name
            Level selLevel = null;
            foreach(Level level in m_allLevels) 
            {
                if(0 == String.Compare(level.Name, levelComboBox.Text)) 
                {
                    selLevel = level;
                    break;
                }
            }
            if (selLevel == null)
            {
                MyMessageBox("There is no available level to get rooms.", MessageBoxIcon.Warning);
                return;
            }

            // update data source of DataGridView
            this.revitRoomDataGridView.DataSource = null;
            this.revitRoomDataGridView.DataSource = new DataView(m_roomData.GenRoomsDataTable(selLevel));
        }


        /// <summary>
        /// Create new rooms according to spreadsheet based rooms data and specified phase.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newRoomButton_Click(object sender, EventArgs e)
        {
            // Create room process:
            // 1: Create shared parameter for "Room" category elements if it doesn't exist.
            // 2: Create rooms by using spread sheet's data: 
            //    a: We should make sure that each of spreadsheet room is mapped by only one Revit room; 
            //       if not, many Revit rooms map to one spreadsheet room will confuse user;
            //    b: Set Name, Number and comment values of new rooms by spreadsheet relative data.
            // 3: Subscribe document Save, SaveAs and Close event handlers.
            // 4: Update all rooms data and pop up message

            int nNewRoomsSize = 0;
            try
            {
                // check to see whether there is available spread sheet based rooms to create
                if (null == m_spreadRoomsTable || null == m_spreadRoomsTable.Rows || m_spreadRoomsTable.Rows.Count == 0)
                {
                    MyMessageBox("There is no available spread sheet based room to create.", MessageBoxIcon.Warning);
                    return;
                }

                // create shared parameter for "Room" category elements
                CreateMyRoomSharedParameter();

                // create Revit rooms by using spread sheet based rooms
                // add "ID" data of spread sheet to Room element's share parameter: "External Room ID"
                DataColumn column = m_spreadRoomsTable.Columns[RoomsData.RoomID];
                if (column == null)
                {
                    MyMessageBox("Failed to get ID data of spread sheet rooms.", MessageBoxIcon.Warning);
                    return;
                }

                // get phase used to create room
                Phase curPhase = null;
                foreach(Phase phase in m_allPhases)
                {
                    if (String.Compare(phase.Name, phaseComboBox.Text) == 0)
                    {
                        curPhase = phase;
                        break;
                    }
                }
                if (null == curPhase)
                {
                    MyMessageBox("No available phase used to create room.", MessageBoxIcon.Warning);
                    return;
                }

                // get all existing rooms which have mapped to spreadsheet rooms.
                // we should skip the creation for those spreadsheet rooms which have been mapped by Revit rooms.
                Dictionary<int, string> existingRooms = new Dictionary<int, string>();
                foreach (Room room in m_roomData.Rooms)
                {
                    Parameter sharedParameter = room.get_Parameter(m_roomSharedParamDef);
                    if (null != sharedParameter && false == String.IsNullOrEmpty(sharedParameter.AsString()))
                    {
                        existingRooms.Add(room.Id.Value, room.get_Parameter(m_roomSharedParamDef).AsString());
                    }
                }

                // transaction is used to cancel room creation when exception occurs
                m_commandData.Application.ActiveDocument.BeginTransaction();

                // create rooms with spread sheet based rooms data
                for (int i = 0; i < m_spreadRoomsTable.Rows.Count; i++)
                {
                    // get the ID column value and use it to check whether this spreadsheet room is mapped by Revit room.
                    String externaId = m_spreadRoomsTable.Rows[i][RoomsData.RoomID].ToString();
                    if (existingRooms.ContainsValue(externaId))
                    {
                        // skip the spreadsheet room creation if it's mapped by Revit room
                        continue;
                    }

                    // create rooms in specified phase, but without placing them.
                    Room newRoom = m_commandData.Application.ActiveDocument.Create.NewRoom(curPhase);
                    if (null == newRoom)
                    {
                        // abort the room creation and pop up failure message
                        m_commandData.Application.ActiveDocument.AbortTransaction();

                        MyMessageBox("Create room failed.", MessageBoxIcon.Warning);
                        return;
                    }

                    // set the shared parameter's value of Revit room 
                    Parameter sharedParam = newRoom.get_Parameter(m_roomSharedParamDef);
                    sharedParam.Set(externaId);

                    // check if "Name", "Number", "Comments" exist
                    // if exist, set new rooms with values in spreadsheet
                    // (currently, there are three columns need to be set)
                    String[] constantColumns = { RoomsData.RoomName, RoomsData.RoomNumber, RoomsData.RoomComments };
                    for (int col = 0; col < constantColumns.Length; col++)
                    {
                        // check to see whether the column exists in table
                        if (m_spreadRoomsTable.Columns.IndexOf(constantColumns[col]) != -1)
                        {
                            // if value is not null or empty, set new rooms related parameter.
                            String colValue = m_spreadRoomsTable.Rows[i][constantColumns[col]].ToString();
                            if (false == String.IsNullOrEmpty(colValue))
                            {
                                continue;
                            }

                            switch (constantColumns[col])
                            {
                                case RoomsData.RoomName:
                                    newRoom.Name = colValue;
                                    break;
                                case RoomsData.RoomNumber:
                                    newRoom.Number = colValue;
                                    break;
                                case RoomsData.RoomComments:
                                    Parameter commentParam = newRoom.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                                    if (null != commentParam)
                                    {
                                        commentParam.Set(colValue);
                                    }
                                    break;
                                default:
                                    // no action for other parameter
                                    break;
                            }
                        }
                    }

                    // remember how many new rooms were created, based on spread sheet data
                    nNewRoomsSize++;
                }

                // end this transaction if create all rooms successfully.
                m_commandData.Application.ActiveDocument.EndTransaction();
            }
            catch (Exception ex)
            {
                // cancel this time transaction when exception occurs
                m_commandData.Application.ActiveDocument.AbortTransaction();

                MyMessageBox(ex.Message, MessageBoxIcon.Warning);
                return;
            }

            // Subscribe document Save, SaveAs and Close event handlers
            // if the event handler of document was subscribed, it's not allowed to subscribe it again.
            if (RoomScheduleForm.s_EventSubDocs.ContainsKey(m_commandData.Application.ActiveDocument) == false)
            {
                // reserves this document and current .xls file and table.
                List<String> xlsAndTable = new List<string>();
                xlsAndTable.Add(m_dataBaseName);
                xlsAndTable.Add(m_roomTableName);
                RoomScheduleForm.s_EventSubDocs.Add(m_commandData.Application.ActiveDocument, xlsAndTable);

                // show current Excel and sheet name sample is mapped to, only show them after unplaced rooms were created.
                UpdateRoomSheetInfo();

                // subscribe Save, SaveAs and Close event handlers
                m_commandData.Application.ActiveDocument.OnSave     += new DocumentSaveEventHandler(OnDocumentSave);
                m_commandData.Application.ActiveDocument.OnSaveAs   += new DocumentSaveAsEventHandler(OnDocumentSave);
                m_commandData.Application.ActiveDocument.OnClose    += new DocumentCloseEventHandler(OnDocumentClose);
            }

            // output unplaced rooms creation message
            String strMessage = string.Empty;
            int nSkippedRooms = m_spreadRoomsTable.Rows.Count - nNewRoomsSize;
            if (nSkippedRooms > 0)
            {
                strMessage = string.Format("{0} unplaced {1} created successfully.\r\n{2} skipped, {3}",
                     nNewRoomsSize,
                     (nNewRoomsSize > 1) ? ("rooms were") : ("room was"),
                     nSkippedRooms.ToString() + ( (nSkippedRooms > 1) ? (" were") : (" was") ),
                     (nSkippedRooms > 1) ? ("because they were already mapped by Revit rooms.") : 
                     ("because it was already mapped by Revit rooms.") );
            }
            else
            {
                strMessage = string.Format("{0} unplaced {1} created successfully.",
                     nNewRoomsSize,
                     (nNewRoomsSize > 1) ? ("rooms were") : ("room was"));
            }

            // update Revit rooms data and display of controls.
            if (nNewRoomsSize > 0)
            {
                UpdateFormDisplay(true);
            }

            // output creation message 
            MyMessageBox(strMessage, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Show all rooms in current document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showAllRoomsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // disable and enable some controls
            levelComboBox.Enabled = !showAllRoomsCheckBox.Checked;

            // update room display, there is no new creation, so it's not necessary to retrieve all rooms
            UpdateFormDisplay(false);
        }
        
        /// <summary>
        /// Close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Clear all values of shared parameters
        /// Allow user to create more unplaced rooms and update map relationships between Revit and spreadsheet rooms.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearIDButton_Click(object sender, EventArgs e)
        {
            int nCount = 0;
            foreach(Room room in m_roomData.Rooms)
            {
                Parameter param = null;
                bool bExist = RoomsData.ShareParameterExists(room, RoomsData.SharedParam, ref param);
                if(bExist && null != param && false == String.IsNullOrEmpty(param.AsString()))
                {
                    param.Set(String.Empty);
                    nCount++;
                }
            }

            // update Revit rooms display
            if(nCount > 0) 
            {
                UpdateFormDisplay(false);
            }
        }
        #endregion
    }
}