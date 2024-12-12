//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.SDK.Samples.CurtainSystem.CS.Data;

namespace Revit.SDK.Samples.CurtainSystem.CS.UI
{
    /// <summary>
    /// the main window form for UI operations
    /// </summary>
    public partial class CurtainForm : System.Windows.Forms.Form
    {
        // the document containing all the data used in the sample
        MyDocument m_mydocument;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mydoc">
        /// the data used in the sample
        /// </param>
        public CurtainForm(MyDocument mydoc)
        {
            m_mydocument = mydoc;

            InitializeComponent();
            // initialize some controls manually
            InitializeCustomComponent();
            // register the customized events
            RegisterEvents();
        }

        /// <summary>
        /// initialize some controls manually
        /// </summary>
        private void InitializeCustomComponent()
        {
            this.deleteCSButton.Enabled = false;
            this.addCGButton.Enabled = false;
            this.removeCGButton.Enabled = false;
        }

        /// <summary>
        /// register the customized events
        /// </summary>
        private void RegisterEvents()
        {
            m_mydocument.FatalErrorEvent += new MyDocument.FatalErrorHandler(m_document_FatalErrorEvent);
            m_mydocument.SystemData.CurtainSystemChanged += new CurtainSystem.SystemData.CurtainSystemChangedHandler(m_document_SystemData_CurtainSystemChanged);
            // moniter the sample message change status
            m_mydocument.MessageChanged += new MyDocument.MessageChangedHandler(m_document_MessageChanged);
        }

        /// <summary>
        /// Fatal error occurs, close the sample dialog directly
        /// </summary>
        /// <param name="errorMsg">
        /// the error hint shown to user
        /// </param>
        void m_document_FatalErrorEvent(string errorMsg)
        {
            // hang the sample and shown the error hint to users
            TaskDialogResult result = TaskDialog.Show(Properties.Resources.TXT_DialogTitle, errorMsg, TaskDialogCommonButtons.Ok, TaskDialogResult.Ok);
            // the user has read the hint and clicked the "OK" button, close the dialog
            if (TaskDialogResult.Ok == result)
            {
                this.Close();
            }
        }

        /// <summary>
        /// curtain system changed(added/removed), refresh the lists
        /// </summary>
        void m_document_SystemData_CurtainSystemChanged()
        {
            // clear the out-of-date values
            csListBox.Items.Clear();
            facesCheckedListBox.Items.Clear();
            cgCheckedListBox.Items.Clear();

            List<CurtainSystem.SystemInfo> csInfos = m_mydocument.SystemData.CurtainSystemInfos;

            // no curtain system available, disable the "Delete Curtain System"
            // "Add curtain grid" and "remove curtain grid" buttons
            if (null == csInfos ||
                0 == csInfos.Count)
            {
                this.deleteCSButton.Enabled = false;
                this.addCGButton.Enabled = false;
                this.removeCGButton.Enabled = false;
                this.Show();
                return;
            }

            foreach (CurtainSystem.SystemInfo info in csInfos)
            {
                csListBox.Items.Add(info);
            }

            // activate the last one
            CurtainSystem.SystemInfo csInfo = csInfos[csInfos.Count - 1];
            // this will invoke the selectedIndexChanged event, then to update the other 2 list boxes
            csListBox.SetSelected(csInfos.Count - 1, true);
            // enable the buttons and show  the dialog
            this.deleteCSButton.Enabled = true;
            // only curtain system which created by reference array supports curtain grid operations
            if (false == csInfo.ByFaceArray)
            {
                this.addCGButton.Enabled = true;
                this.removeCGButton.Enabled = true;
            }
            this.Show();
        }

        /// <summary>
        /// update the status hints in the status strip
        /// </summary>
        void m_document_MessageChanged()
        {
            //if it's an error / warning message, set the color of the text to red
            KeyValuePair<string, bool> message = m_mydocument.Message;
            if (true == message.Value)
            {
                this.operationStatusLabel.ForeColor = System.Drawing.Color.Red;
            }
            // it's a common hint message, set the color to black
            else
            {
                this.operationStatusLabel.ForeColor = System.Drawing.Color.Black;
            }
            this.operationStatusLabel.Text = message.Key;
            this.statusStrip.Refresh();
        }

        /// <summary>
        /// "Create curtain system" button clicked, hide the main form,
        /// pop up the create curtain system dialog to let user add a new curtain system.
        /// After the curtain system created, close the dialog and show the main form again
        /// </summary>
        /// <param name="sender">
        /// object who sent this event
        /// </param>
        /// <param name="e">
        /// event args
        /// </param>
        private void createCSButton_Click(object sender, EventArgs e)
        {
            this.Hide();

            // show the "create curtain system" dialog
            using (CreateCurtainSystemDialog dlg = new CreateCurtainSystemDialog(m_mydocument))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// delete the checked curtain systems in  the curtain system list box
        /// </summary>
        /// <param name="sender">
        /// object who sent this event 
        /// </param>
        /// <param name="e"> 
        /// event args 
        /// </param>
        private void deleteCSButton_Click(object sender, EventArgs e)
        {
            // no curtain system available, ask sample user to create some curtain systems first
            if (null == csListBox.Items ||
                0 == csListBox.Items.Count)
            {
                string hint = Properties.Resources.HINT_CreateCSFirst;
                m_mydocument.Message = new KeyValuePair<string, bool>(hint, true);
                return;
            }

            // get the checked curtain sytems
            List<int> checkedIndices = new List<int>();
            for (int i = 0; i < csListBox.Items.Count; i++)
            {
                bool itemChecked = csListBox.GetItemChecked(i);

                if (true == itemChecked)
                {
                    checkedIndices.Add(i);
                }
            }

            // no curtain system available or no curtain system selected for deletion
            // update the status hints
            if (null == checkedIndices ||
                0 == checkedIndices.Count)
            {
                string hint = Properties.Resources.HINT_SelectCSFirst;
                m_mydocument.Message = new KeyValuePair<string, bool>(hint, true);
                return;
            }

            // delete them
            m_mydocument.SystemData.DeleteCurtainSystem(checkedIndices);
        }

        /// <summary>
        /// the selected curtain system changed, update the "Curtain grid" and 
        /// "Uncovered faces" list boxes of the selected curtain system
        /// </summary>
        /// <param name="sender">
        /// object who sent this event 
        /// </param>
        /// <param name="e"> 
        /// event args 
        /// </param>
        private void csListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<CurtainSystem.SystemInfo> csInfos = m_mydocument.SystemData.CurtainSystemInfos;

            // data verification
            if (null == csInfos ||
                0 == csInfos.Count)
            {
                return;
            }

            //
            // step 1: activate the selected one
            //
            CurtainSystem.SystemInfo csInfo = csInfos[csListBox.SelectedIndex];
            // update the curtain grid list box
            cgCheckedListBox.Items.Clear();
            foreach (int index in csInfo.GridFacesIndices)
            {
                CurtainSystem.GridFaceInfo gridFaceInfo = new CurtainSystem.GridFaceInfo(index);
                cgCheckedListBox.Items.Add(gridFaceInfo);
            }
            // update the uncovered face list box
            facesCheckedListBox.Items.Clear();
            foreach (int index in csInfo.UncoverFacesIndices)
            {
                CurtainSystem.UncoverFaceInfo uncoverFaceInfo = new CurtainSystem.UncoverFaceInfo(index);
                facesCheckedListBox.Items.Add(uncoverFaceInfo);
            }
            //
            // step 2: enable/disable some buttons and refresh the status hints
            //
            // the selected curtain system is created by face array
            // it's not allowed to modify its curtain grids data
            if (true == csInfo.ByFaceArray)
            {
                // disable the buttons
                this.addCGButton.Enabled = false;
                this.removeCGButton.Enabled = false;
                this.facesCheckedListBox.Enabled = false;
                this.cgCheckedListBox.Enabled = false;
                // update the status hints
                string hint = Properties.Resources.HINT_CSIsByFaceArray;
                m_mydocument.Message = new KeyValuePair<string, bool>(hint, false);
            }
            // the selected curtain system is created by references of the faces
            // it's allowed to modify its curtain grids data
            else
            {
                // enable the buttons
                if (null == facesCheckedListBox.Items ||
                    0 == facesCheckedListBox.Items.Count)
                {
                    this.addCGButton.Enabled = false;
                }
                else
                {
                    this.addCGButton.Enabled = true;
                }
                // at least one curtain grid must be kept
                if (null == cgCheckedListBox.Items ||
                    2 > cgCheckedListBox.Items.Count)
                {
                    this.removeCGButton.Enabled = false;
                }
                else
                {
                    this.removeCGButton.Enabled = true;
                }
                this.facesCheckedListBox.Enabled = true;
                this.cgCheckedListBox.Enabled = true;
                // update the status hints
                string hint = "";
                m_mydocument.Message = new KeyValuePair<string, bool>(hint, false);
            }
        }

        /// <summary>
        /// add curtain grids to the checked faces 
        /// </summary>
        /// <param name="sender">
        /// object who sent this event
        /// </param>
        /// <param name="e">
        /// event args
        /// </param>
        private void addCGButton_Click(object sender, EventArgs e)
        {
            // step 1: get the curtain system 
            List<CurtainSystem.SystemInfo> csInfos = m_mydocument.SystemData.CurtainSystemInfos;

            // no curtain system available, ask sample user to create some curtain systems first
            if (null == csInfos || 0 == csInfos.Count)
            {
                string hint = Properties.Resources.HINT_CreateCSFirst;
                m_mydocument.Message = new KeyValuePair<string, bool>(hint, true);
                return;
            }
            CurtainSystem.SystemInfo csInfo = csInfos[csListBox.SelectedIndex];
            // if the curtain system is created by face array, it's forbidden to make other operations on it
            if (true == csInfo.ByFaceArray)
            {
                return;
            }
            // step 2: find out the faces to be covered
            List<int> faceIndices = new List<int>();
            for (int i = 0; i < facesCheckedListBox.Items.Count; i++)
            {
                bool itemChecked = facesCheckedListBox.GetItemChecked(i);
                if (true == itemChecked)
                {
                    CurtainSystem.UncoverFaceInfo info = facesCheckedListBox.Items[i] as CurtainSystem.UncoverFaceInfo;
                    faceIndices.Add(info.Index);
                }
            }

            // no uncovered faces selected, warn the sample user
            if (null == faceIndices ||
                0 == faceIndices.Count)
            {
                string hint = Properties.Resources.HINT_SelectFaceFirst;
                m_mydocument.Message = new KeyValuePair<string, bool>(hint, true);
                return;
            }

            // step 3: cover the selected faces with curtain grids
            csInfo.AddCurtainGrids(faceIndices);
            // step 4: update the UI list boxes
            csListBox_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// remove  the checked curtain grids from the curtain system
        /// Note: curtain system must have at least one curtain grid
        /// so sample users can't remove all the curtain grids away
        /// </summary>
        /// <param name="sender">
        /// object who sent this event 
        /// </param>
        /// <param name="e">
        /// event args 
        /// </param>
        private void removeCGButton_Click(object sender, EventArgs e)
        {
            // step 1: get the curtain system 
            List<CurtainSystem.SystemInfo> csInfos = m_mydocument.SystemData.CurtainSystemInfos;

            // no curtain system available, ask sample user to create some curtain systems first
            if (null == csInfos || 0 == csInfos.Count)
            {
                string hint = Properties.Resources.HINT_CreateCSFirst;
                m_mydocument.Message = new KeyValuePair<string, bool>(hint, true);
                return;
            }

            CurtainSystem.SystemInfo csInfo = csInfos[csListBox.SelectedIndex];
            // if the curtain system is created by face array, it's forbidden to make other operations on it
            if (true == csInfo.ByFaceArray)
            {
                return;
            }
            // step 2: find out the curtain grids to be removed
            List<int> faceIndices = new List<int>();
            for (int i = 0; i < cgCheckedListBox.Items.Count; i++)
            {
                bool itemChecked = cgCheckedListBox.GetItemChecked(i);
                if (true == itemChecked)
                {
                    CurtainSystem.GridFaceInfo info = cgCheckedListBox.Items[i] as CurtainSystem.GridFaceInfo;
                    faceIndices.Add(info.FaceIndex);
                }
            }

            // no curtain grids selected, warn the sample user
            if (null == faceIndices ||
                0 == faceIndices.Count)
            {
                string hint = Properties.Resources.HINT_SelectCGFirst;
                m_mydocument.Message = new KeyValuePair<string, bool>(hint, true);
                return;
            }

            // step 3: remove the selected curtain grids
            csInfo.RemoveCurtainGrids(faceIndices);
            // step 4: update the UI list boxes
            csListBox_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// check the curtain grids to delete them, if user wants to check all the curtain
        /// grids for deletion, prohibit it (must keep at least one curtain grid)
        /// </summary>
        /// <param name="sender">
        /// object who sent this event
        /// </param>
        /// <param name="e">
        /// event args 
        /// </param>
        private void cgCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            List<int> indices = new List<int>();
            // get all the unchecked curtain grids
            for (int i = 0; i < cgCheckedListBox.Items.Count; i++)
            {
                bool itemChecked = cgCheckedListBox.GetItemChecked(i);
                if (false == itemChecked)
                {
                    indices.Add(i);
                }
            }

            // for curtain system, we must keep at least one curtain grid
            // so it's not allowed to select all the curtain grids to remove
            if (indices.Count <= 1 &&
                CheckState.Unchecked == e.CurrentValue)
            {
                e.NewValue = CheckState.Unchecked;

                // update the status hints
                string hint = Properties.Resources.HINT_KeepOneCG;
                m_mydocument.Message = new KeyValuePair<string, bool>(hint, true);
            }
            else
            {
                // update the status hints
                string hint = "";
                m_mydocument.Message = new KeyValuePair<string, bool>(hint, false);
            }
        }



        private void facesCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void csLabel_Click(object sender, EventArgs e)
        {

        }

    } // end of class
}