//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.PerformanceAdviserControl.CS
{
   public partial class TestDisplayDialog : System.Windows.Forms.Form
   {
      #region Constructor
      /// <summary>
      /// Basic setup -- stores references to the active document and PerformanceAdviser for later use
      /// </summary>
      /// <param name="performanceAdviser">The revit PerformanceAdviser class</param>
      /// <param name="document">The active document</param>
      public TestDisplayDialog(PerformanceAdviser performanceAdviser, Document document)
      {
         m_PerformanceAdviser = performanceAdviser;
         m_document = document;
         InitializeComponent();
         this.FormBorderStyle = FormBorderStyle.Fixed3D;

      }
      #endregion

      #region UI Handlers
      /// <summary>
      /// Called when the user clicks the "Run Selected Tests" button
      /// </summary>
      private void btn_RunTests_Click(object sender, EventArgs e)
      {

         this.Close();

         //Iterate through each item in the dialog data grid.
         //Check to see if the user selected a specific rule to be enabled.
         //Set the rule to the same enabled state in PerformanceAdviser using
         //PerformanceAdviser::SetRuleDisabled.
         int testIndex = 0;
         foreach (DataGridViewRow row in this.testData.Rows)
         {
            bool isEnabled = (bool)row.Cells[0].Value;
            m_PerformanceAdviser.SetRuleEnabled(testIndex, isEnabled);
            System.Diagnostics.Debug.WriteLine("Test Name: " + m_PerformanceAdviser.GetRuleName(testIndex) + " Enabled? " + !m_PerformanceAdviser.IsRuleEnabled(testIndex));
            testIndex++;
         }

         //Run all rules that are currently enabled and report errors
         IList<FailureMessage> failures = m_PerformanceAdviser.ExecuteAllRules(m_document);
         foreach (FailureMessage fm in failures)
         {
            Transaction tFailure = new Transaction(m_document, "Failure Reporting Transaction");
            tFailure.Start();
            m_document.PostFailure(fm);
            tFailure.Commit();
         }
      }

      /// <summary>
      /// Called when the user clicks the "Select All" button.
      /// </summary>
      private void btn_SelectAll_Click(object sender, EventArgs e)
      {
      
         //Set the first column value (the enabled status) to true;
         foreach (DataGridViewRow row in this.testData.Rows)
         {
            row.Cells[0].Value = (object) true;
         }
      }

      /// <summary>
      /// Called when the user clicks the "Deselect All" button.
      /// </summary>
      private void btn_DeselectAll_Click(object sender, EventArgs e)
      {
         //Set the first column value (the enabled status) to false;
         foreach (DataGridViewRow row in this.testData.Rows)
         {
            row.Cells[0].Value = (object)false;
         }
      }

      /// <summary>
      /// Closes the dialog without committing any action
      /// </summary>
      private void btn_Cancel_Click(object sender, EventArgs e)
      {
         this.Close();
      }
      #endregion

      #region Other instance methods
      /// <summary>
      /// This method is called by UICommand::Execute() and adds test information to the grid
      /// data object "testData."
      /// </summary>
      /// <param name="name">The rule name</param>
      /// <param name="isOurRule">Whether or not the rule is a the API-defined rule in this project</param>
      /// <param name="isEnabled">Whether or not the rule is currently selected to run</param>
      public void AddData(string name, bool isOurRule, bool isEnabled)
      {
         object[] data = new object[3];
         data[0] = isEnabled;
         data[1] = name;
         data[2] = isOurRule ? "Yes" : "No";


         this.testData.Rows.Add(data);
      }
      #endregion

      #region Data
      private Autodesk.Revit.DB.Document m_document;
      private Autodesk.Revit.DB.PerformanceAdviser m_PerformanceAdviser;
#endregion      

     
   }
}
