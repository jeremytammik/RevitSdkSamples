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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.Reinforcement.CS
{
   /// <summary>
   /// The form is used for collecting information of beam reinforcement creation 
   /// </summary>
   public partial class BeamFramReinMakerForm : System.Windows.Forms.Form
   {
      // Private members
      BeamFramReinMaker m_dataBuffer = null;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="dataBuffer">the BeamFramReinMaker reference</param>
      public BeamFramReinMakerForm(BeamFramReinMaker dataBuffer)
      {
         // Required for Windows Form Designer support
         InitializeComponent();

         // Store the reference of BeamFramReinMaker
         m_dataBuffer = dataBuffer;

         // Bing the data source for all combo boxes
         BingingDataSource();

         // set the initialization data of the spacing
         transverseCenterSpacingTextBox.Text = 0.1.ToString("0.0");
         transverseEndSpacingTextBox.Text = 0.1.ToString("0.0");
      }

      /// <summary>
      /// Bing the data source for all combo boxes
      /// </summary>
      private void BingingDataSource()
      {
         // bind the topEndRebarTypeComboBox
         topEndRebarTypeComboBox.DataSource = m_dataBuffer.RebarTypes;
         topEndRebarTypeComboBox.DisplayMember = "Name";

         // bind the topCenterRebarTypeComboBox
         topCenterRebarTypeComboBox.DataSource = m_dataBuffer.RebarTypes;
         topCenterRebarTypeComboBox.DisplayMember = "Name";

         // bind the bottomRebarTypeComboBox
         bottomRebarTypeComboBox.DataSource = m_dataBuffer.RebarTypes;
         bottomRebarTypeComboBox.DisplayMember = "Name";

         // bind the transverseRebarTypeComboBox
         transverseRebarTypeComboBox.DataSource = m_dataBuffer.RebarTypes;
         transverseRebarTypeComboBox.DisplayMember = "Name";

         // bind the topBarHookComboBox
         topBarHookComboBox.DataSource = m_dataBuffer.HookTypes;
         topBarHookComboBox.DisplayMember = "Name";

         // bind the transverseBarHookComboBox
         transverseBarHookComboBox.DataSource = m_dataBuffer.HookTypes;
         transverseBarHookComboBox.DisplayMember = "Name";
      }

      /// <summary>
      /// When the user click ok, refresh the data of BeamFramReinMaker and close form
      /// </summary>
      private void okButton_Click(object sender, EventArgs e)
      {
         // set TopEndRebarType data
         RebarBarType type = topEndRebarTypeComboBox.SelectedItem as RebarBarType;
         m_dataBuffer.TopEndRebarType = type;

         // set TopCenterRebarType data
         type = topCenterRebarTypeComboBox.SelectedItem as RebarBarType;
         m_dataBuffer.TopCenterRebarType = type;

         // set BottomRebarType data
         type = bottomRebarTypeComboBox.SelectedItem as RebarBarType;
         m_dataBuffer.BottomRebarType = type;

         // set TransverseRebarType data
         type = transverseRebarTypeComboBox.SelectedItem as RebarBarType;
         m_dataBuffer.TransverseRebarType = type;

         // set TopHookType data
         RebarHookType hookType = topBarHookComboBox.SelectedItem as RebarHookType;
         m_dataBuffer.TopHookType = hookType;

         // set TransverseHookType data
         hookType = transverseBarHookComboBox.SelectedItem as RebarHookType;
         m_dataBuffer.TransverseHookType = hookType;

         try
         {
            // set TransverseEndSpacing data
            double spacing = Convert.ToDouble(transverseEndSpacingTextBox.Text);
            m_dataBuffer.TransverseEndSpacing = spacing;

            // set TransverseCenterSpacing data
            spacing = Convert.ToDouble(transverseCenterSpacingTextBox.Text);
            m_dataBuffer.TransverseCenterSpacing = spacing;
         }
         catch (FormatException)
         {
            // spacing text boxes should only input number information
            TaskDialog.Show("Revit", "Please input double number in spacing TextBox.");
            return;
         }
         catch (Exception ex)
         {
            // other unexpected error, just show the information
            TaskDialog.Show("Revit", ex.Message);
            return;
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
