//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.PlaceFamilyInstanceByFace.CS
{
    /// <summary>
    /// The main UI for creating family instance by face
    /// </summary>
    public partial class PlaceFamilyInstanceForm : System.Windows.Forms.Form
    {
        // the creator
        private FamilyInstanceCreator m_creator = null;
        // the base type
        private BasedType m_baseType = BasedType.Point; 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creator">the family instance creator</param>
        /// <param name="type">based-type</param>
        public PlaceFamilyInstanceForm(FamilyInstanceCreator creator, BasedType type)
        {
            m_creator = creator;
            m_creator.CheckFamilySymbol(type);
            m_baseType = type;
            InitializeComponent();

            // set the face name list and the default value
            foreach (String name in creator.FaceNameList)
            {
                comboBoxFace.Items.Add(name);
            }
            if (comboBoxFace.Items.Count > 0)
            {
                SetFaceIndex(0);
            }

            // set the family name list and the default value
            foreach (String symbolName in m_creator.FamilySymbolNameList)
            {
                comboBoxFamily.Items.Add(symbolName);
            }
            if (m_creator.DefaultFamilySymbolIndex < 0)
            {
                comboBoxFamily.SelectedItem = m_creator.FamilySymbolNameList[0];
            }
            else
            {
                comboBoxFamily.SelectedItem =
                    m_creator.FamilySymbolNameList[m_creator.DefaultFamilySymbolIndex];
            }

            // set UI display according to baseType
            switch (m_baseType)
            {
                case BasedType.Point:
                    this.Text = "Place Point-Based Family Instance";
                    labelFirst.Text = "Location :";
                    labelSecond.Text = "Direction :";
                    break;
                case BasedType.Line:
                    comboBoxFamily.SelectedItem = "Line-based";
                    this.Text = "Place Line-Based Family Instance";

                    labelFirst.Text = "Start Point :";
                    labelSecond.Text = "End Point :";
                    break;
                default:
                    break;
            }
            AdjustComboBoxDropDownListWidth(comboBoxFace);
            AdjustComboBoxDropDownListWidth(comboBoxFamily);
        }

        /// <summary>
        /// Get face information when the selected face is changed
        /// </summary>
        /// <param name="index">the index of the new selected face</param>
        private void SetFaceIndex(int index)
        {
            comboBoxFace.SelectedItem = m_creator.FaceNameList[index];

            BoundingBoxXYZ boundingBox = m_creator.GetFaceBoundingBox(index);
            Autodesk.Revit.DB.XYZ totle = boundingBox.Min + boundingBox.Max;
            switch (m_baseType)
            {
                case BasedType.Point:
                    PointControlFirst.SetPointData(totle / 2.0f);
                    PointControlSecond.SetPointData(new Autodesk.Revit.DB.XYZ (1.0f, 0f, 0f));
                    break;
                case BasedType.Line:
                    PointControlFirst.SetPointData(boundingBox.Min);
                    PointControlSecond.SetPointData(boundingBox.Max);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Create a family instance according the selected options by user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            bool retBool = false;
            try
            {
                Transaction transaction = new Transaction(m_creator.RevitDoc.Document, "CreateFamilyInstance");
                transaction.Start();
                switch (m_baseType)
                {
                    case BasedType.Point:
                        retBool = m_creator.CreatePointFamilyInstance(PointControlFirst.GetPointData()
                            , PointControlSecond.GetPointData()
                            , comboBoxFace.SelectedIndex
                            , comboBoxFamily.SelectedIndex);
                        break;

                    case BasedType.Line:
                        retBool = m_creator.CreateLineFamilyInstance(PointControlFirst.GetPointData()
                            , PointControlSecond.GetPointData()
                            , comboBoxFace.SelectedIndex
                            , comboBoxFamily.SelectedIndex);
                        break;

                    default:
                        break;
                }
                transaction.Commit();
            }
            catch(ApplicationException)
            {
                MessageBox.Show("Failed in creating family instance, maybe because the family symbol is wrong type, please check and choose again.");
                return;
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message);
                return;
            }

            if (retBool)
            {
                this.Close();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
               MessageBox.Show("The line is perpendicular to this face, please input the position again.");
            }
        }

        /// <summary>
        /// Process the SelectedIndexChanged event of comboBoxFace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxFace_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFaceIndex(comboBoxFace.SelectedIndex);
        }

        /// <summary>
        /// Adjust the comboBox dropDownList width
        /// </summary>
        /// <param name="senderComboBox">the comboBox</param>
        private void AdjustComboBoxDropDownListWidth(ComboBox senderComboBox)
        {
            Graphics g = null;
            Font font = null;
            try
            {
                int width = senderComboBox.Width;
                g = senderComboBox.CreateGraphics();
                font = senderComboBox.Font;

                // checks if a scrollbar will be displayed.
                // if yes, then get its width to adjust the size of the drop down list.
                int vertScrollBarWidth =
                    (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                    ? SystemInformation.VerticalScrollBarWidth : 0;

                int newWidth;
                foreach (object s in senderComboBox.Items)  //Loop through list items and check size of each items.
                {
                    if (s != null)
                    {
                        newWidth = (int)g.MeasureString(s.ToString().Trim(), font).Width
                            + vertScrollBarWidth;
                        if (width < newWidth)
                        {
                            width = newWidth;   //set the width of the drop down list to the width of the largest item.
                        }
                    }
                }
                senderComboBox.DropDownWidth = width;
            }
            catch
            { }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                }
            }
        }
    }
}