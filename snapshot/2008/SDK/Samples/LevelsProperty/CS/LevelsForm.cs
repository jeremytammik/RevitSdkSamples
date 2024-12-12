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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.LevelsProperty.CS
{
    /// <summary>
    /// form for new levels
    /// </summary>  
    public partial class LevelsForm : Form
    {
        /// <summary>
        /// form for new levels
        /// </summary>
        public LevelsForm()
        {
            InitializeComponent();
        }

        #region Constructor
        /// <summary>
        /// The constructor is used to initialize some object.
        /// </summary>
        /// <param name="opt">Used to get the Command class's object.</param>
        public LevelsForm(Command opt)
        {
            InitializeComponent();

            m_objectReference = opt;

            //Set control on UI
            LevelName = new DataGridViewTextBoxColumn();
            LevelName.HeaderText = "Name";
            LevelName.Width = 142;

            LevelElevation = new DataGridViewTextBoxColumn();
            LevelElevation.HeaderText = "Elevation";
            LevelElevation.Width = 142;

            levelsDataGridView.Columns.AddRange(new DataGridViewColumn[] { LevelName, LevelElevation });

            bindingSource1.DataSource = typeof(Revit.SDK.Samples.LevelsProperty.CS.LevelsDataSource);
            //Must place below code on the code "dataGridView1.DataSource = bindingSource1"
            levelsDataGridView.AutoGenerateColumns = false;
            levelsDataGridView.DataSource = bindingSource1;
            LevelName.DataPropertyName = "Name";
            LevelElevation.DataPropertyName = "Elevation";

            //pass datum to BindingSource
            bindingSource1.DataSource = m_objectReference.SystemLevelsDatum;

            //Record syetem levels's total
            m_systemLevelsTotal = m_objectReference.SystemLevelsDatum.Count;

            //Record changed items
            m_changedItemsFlag = new int[m_systemLevelsTotal];

            //Record deleted items
            m_deleteExistLevelIDValue = new int[m_systemLevelsTotal];
            m_deleteExistLevelTotal = 0;
        }

        //Class Command's object reference
        Command m_objectReference;
        #endregion

        #region AddItem
        /// <summary>
        /// Used to a new item in the dataGridView control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, EventArgs e)
        {
            System.String newLevelName;
            double newLevelElevation;

            //If it exists some Levels on Revit, 
            //the added item's Name and Elevation uses last a Level's Name and Elevation.
            //Otherwise it uses a default data.
            if (bindingSource1.Count > 0)
            {
                bindingSource1.MoveLast();
                LevelsDataSource lastItem = bindingSource1.Current as LevelsDataSource;

                System.String lastLevelName = lastItem.Name;
                double lastLevelElevation = lastItem.Elevation;
                newLevelName = lastLevelName + "'";
                newLevelElevation = lastLevelElevation + 3000;
            }
            else
            {
                newLevelName = "Level" + " " + "1";
                newLevelElevation = 0;
            }


            LevelsDataSource newLevel = new LevelsDataSource();
            newLevel.Name = newLevelName;
            newLevel.Elevation = newLevelElevation;

            bindingSource1.Add(newLevel);
        }

        //Record syetem levels's total
        int m_systemLevelsTotal;
        #endregion

        #region RemoveItem
        /// <summary>
        /// Used to delete a item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (1 == levelsDataGridView.RowCount)
            {
                MessageBox.Show("Deleting the only open view in the project is not allowed.");
                return;
            }

            if (bindingSource1.Position > m_systemLevelsTotal - 1)
            {
                bindingSource1.RemoveCurrent();
                return;
            }

            if (bindingSource1.Position <= m_systemLevelsTotal - 1 && bindingSource1.Position >= 0)
            {
                LevelsDataSource aRow = bindingSource1.Current as LevelsDataSource;
                m_deleteExistLevelIDValue[m_deleteExistLevelTotal] = aRow.LevelIDValue;
                m_deleteExistLevelTotal++;

                bindingSource1.RemoveCurrent();

                m_systemLevelsTotal = m_systemLevelsTotal - 1;

                int[] temChangedItemsFlag = new int[m_systemLevelsTotal];
                for (int i = 0, j = 0; i < m_systemLevelsTotal; i++, j++)
                {
                    if (bindingSource1.Position == i)
                    {
                        j++;
                    }
                    temChangedItemsFlag[i] = m_changedItemsFlag[j];
                }
                m_changedItemsFlag = temChangedItemsFlag;

                return;
            }

            if (bindingSource1.Position < 0)
            {
                System.Windows.Forms.MessageBox.Show("No have Level.");
            }
        }

        int[] m_deleteExistLevelIDValue;
        int m_deleteExistLevelTotal;
        #endregion

        #region CheckAndRecord
        /// <summary>
        /// Judge if the inputted Name is unique.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelsDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (0 == levelsDataGridView.CurrentCell.ColumnIndex)
            {
                System.String newName = e.FormattedValue as System.String;

                char[] newNameArray = new char[newName.Length];
                newNameArray = newName.ToCharArray();
                for (int i = 0; i < newName.Length; ++i)
                {
                    if ('\\' == newNameArray[i] || ':' == newNameArray[i] || '{' == newNameArray[i] ||
                        '}' == newNameArray[i] || '[' == newNameArray[i] || ']' == newNameArray[i] ||
                        '|' == newNameArray[i] || ';' == newNameArray[i] || '<' == newNameArray[i] ||
                        '>' == newNameArray[i] || '?' == newNameArray[i] || '`' == newNameArray[i] ||
                        '~' == newNameArray[i])
                    {
                        MessageBox.Show("Name cannot contain any of the following characters:\n\\ "
                        + ": { } [ ] | ; < > ? ` ~ \nor any of the non-printable characters.");

                        e.Cancel = true;

                        return;
                    }
                }

                System.String oldName = levelsDataGridView.CurrentCell.FormattedValue as System.String;
                if (newName != oldName)
                {
                    for (int i = 0; i < m_objectReference.SystemLevelsDatum.Count; i++)
                    {
                        if (m_objectReference.SystemLevelsDatum[i].Name == newName)
                        {
                            MessageBox.Show("The name entered is already in use. Enter a unique name.");
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Judge if the inputted Elevation is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(e.Exception.Message);
        }

        /// <summary>
        /// Record the changed Item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (bindingSource1.Position < m_systemLevelsTotal)
            {
                m_systemLevelChangedFlag = 1;
                m_changedItemsFlag[bindingSource1.Position] = 1;
            }
        }

        //Record changed item
        int[] m_changedItemsFlag;
        int m_systemLevelChangedFlag = 0;
        #endregion

        #region okButton
        /// <summary>
        /// Used to make setting apply to the model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            //Set all changed Levels's name and elevation
            if (1 == m_systemLevelChangedFlag)
            {
                for (int i = 0; i < m_changedItemsFlag.LongLength; i++)
                {
                    if (1 == m_changedItemsFlag[i])
                    {
                        bindingSource1.Position = i;
                        LevelsDataSource changeItem = bindingSource1.Current as LevelsDataSource;
                        m_objectReference.SetLevel(changeItem.LevelIDValue, changeItem.Name, changeItem.Elevation);
                    }
                }
            }

            //Delete existed Levels
            for (int i = 0; i < m_deleteExistLevelTotal; i++)
            {
                m_objectReference.DeleteLevel(m_deleteExistLevelIDValue[i]);
            }

            //Create new Levels
            for (int i = m_systemLevelsTotal; i < bindingSource1.Count; i++)
            {
                bindingSource1.Position = i;
                LevelsDataSource newItem = bindingSource1.Current as LevelsDataSource;
                m_objectReference.CreateLevel(newItem.Name, newItem.Elevation);
            }
        }
        #endregion
    }
}