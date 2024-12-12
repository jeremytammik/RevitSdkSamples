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

using Autodesk.Revit.Symbols;

namespace Revit.SDK.Samples.Reinforcement.CS
{
   /// <summary>
   /// The form is used for collecting information of column reinforcement creation 
   /// </summary>
   public partial class ColumnFramReinMakerForm : Form
   {
      // Private members
      ColumnFramReinMaker m_dataBuffer = null;

      /// <summary>
      /// constructor for ColumnFramReinMakerForm
      /// </summary>
      /// <param name="dataBuffer">the ColumnFramReinMaker reference</param>
      public ColumnFramReinMakerForm(ColumnFramReinMaker dataBuffer)
      {
         // Required for Windows Form Designer support
         InitializeComponent();

         // Store the reference of ColumnFramReinMaker
         m_dataBuffer = dataBuffer;

         // Bing the data source for all combo boxes
         BingingDataSource();

         // set the initializtion data of the spacing
         centerSpacingTextBox.Text = "0.1";
         endSpacingTextBox.Text = "0.1";
      }

      /// <summary>
      /// Bing the data source for all combo boxes
      /// </summary>
      private void BingingDataSource()
      {
         // bing the verticalRebarTypeComboBox
         verticalRebarTypeComboBox.DataSource = m_dataBuffer.RebarTypes;
         verticalRebarTypeComboBox.DisplayMember = "Name";

         // bing the centerTransverseRebarTypeComboBox
         centerTransverseRebarTypeComboBox.DataSource = m_dataBuffer.RebarTypes;
         centerTransverseRebarTypeComboBox.DisplayMember = "Name";

         // bing the endTransverseRebarTypeComboBox
         endTransverseRebarTypeComboBox.DataSource = m_dataBuffer.RebarTypes;
         endTransverseRebarTypeComboBox.DisplayMember = "Name";

         // bing the transverseRebarHookComboBox
         transverseRebarHookComboBox.DataSource = m_dataBuffer.HookTypes;
         transverseRebarHookComboBox.DisplayMember = "Name";
      }

      /// <summary>
      /// When the user click ok, refresh the data of BeamFramReinMaker and close form
      /// </summary>
      private void okButton_Click(object sender, EventArgs e)
      {
         // set TransverseCenterType data
         RebarBarType type = centerTransverseRebarTypeComboBox.SelectedItem as RebarBarType;
         m_dataBuffer.TransverseCenterType = type;

         // set TransverseEndType data
         type = endTransverseRebarTypeComboBox.SelectedItem as RebarBarType;
         m_dataBuffer.TransverseEndType = type;

         // set VerticalRebarType data
         type = verticalRebarTypeComboBox.SelectedItem as RebarBarType;
         m_dataBuffer.VerticalRebarType = type;

         // set TransverseHookType data
         RebarHookType hookType = transverseRebarHookComboBox.SelectedItem as RebarHookType;
         m_dataBuffer.TransverseHookType = hookType;

         // set VerticalRebarNumber data
         int number = (int)rebarQuantityNumericUpDown.Value;
         m_dataBuffer.VerticalRebarNumber = number;

         try
         {
            // set TransverseCenterSpacing data
            double spacing = Convert.ToDouble(centerSpacingTextBox.Text);
            m_dataBuffer.TransverseCenterSpacing = spacing;

            // set TransverseEndSpacing data
            spacing = Convert.ToDouble(endSpacingTextBox.Text);
            m_dataBuffer.TransverseEndSpacing = spacing;
         }
         catch (FormatException)
         {
            // spacing text boxes should only input number information
            MessageBox.Show("Please input double number in spacing TextBox.", "Revit");
            return;
         }
         catch (Exception ex)
         {
            // if other unexpected error, just show the information
            MessageBox.Show(ex.Message, "Revit");
         }

         this.DialogResult = DialogResult.OK;    // set dialog result
         this.Close();                           // close the form
      }


      /// <summary>
      /// When the user click the cancel, just close the form
      /// </summary>
      private void cancelButton_Click(object sender, EventArgs e)
      {
         this.DialogResult = DialogResult.Cancel;// set dialog result
         this.Close();                           // close the form
      }
   }
}