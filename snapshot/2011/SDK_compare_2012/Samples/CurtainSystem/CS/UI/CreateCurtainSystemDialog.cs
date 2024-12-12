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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Revit.SDK.Samples.CurtainSystem.CS.Data;

namespace Revit.SDK.Samples.CurtainSystem.CS.UI
{
   /// <summary>
   /// the winForm for user to create a new curtain system
   /// </summary>
   public partial class CreateCurtainSystemDialog : System.Windows.Forms.Form
   {
      MyDocument m_mydocument;
      // the flag for curtain system creation, if it's true, the curtain system
      // will be created by face array (can't add/remove curtain grids on this kind of curtain system)
      // otherwise be created by reference array
      bool m_byFaceArray;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="mydoc">
      /// the document of the sample
      /// </param>
      public CreateCurtainSystemDialog(MyDocument mydoc)
      {
         m_mydocument = mydoc;

         InitializeComponent();

         //
         // initialize data
         //
         // by default, create the curtain system by reference array
         m_byFaceArray = false;
      }

      /// <summary>
      /// check/uncheck the "by face array" flag
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void byFaceArrayCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         m_byFaceArray = byFaceArrayCheckBox.Checked;
      }

      /// <summary>
      /// select all the faces
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void selectAllButton_Click(object sender, EventArgs e)
      {
         for (int i = 0; i < facesCheckedListBox.Items.Count; i++)
         {
            facesCheckedListBox.SetItemChecked(i, true);
         }
      }

      /// <summary>
      /// reverse the selection
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void reverseSelButton_Click(object sender, EventArgs e)
      {
         for (int i = 0; i < facesCheckedListBox.Items.Count; i++)
         {
            bool itemChecked = facesCheckedListBox.GetItemChecked(i);
            // toggle the checked status
            facesCheckedListBox.SetItemChecked(i, !itemChecked);
         }
      }

      /// <summary>
      /// clear the selected faces
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void clearButton_Click(object sender, EventArgs e)
      {
         for (int i = 0; i < facesCheckedListBox.Items.Count; i++)
         {
            facesCheckedListBox.SetItemChecked(i, false);
         }
      }

      /// <summary>
      /// create a new curtain system by the selected faces
      /// </summary>
      /// <param name="sender">
      /// object who sent this event
      /// </param>
      /// <param name="e">
      /// event args
      /// </param>
      private void createCSButton_Click(object sender, EventArgs e)
      {
         // step 1: get the faces for curtain system creation
         List<int> checkedIndices = new List<int>();
         for (int i = 0; i < facesCheckedListBox.Items.Count; i++)
         {
            bool itemChecked = facesCheckedListBox.GetItemChecked(i);

            if (true == itemChecked)
            {
               checkedIndices.Add(i);
            }
         }

         // step 2: create the new curtain system
         m_mydocument.SystemData.CreateCurtainSystem(checkedIndices, m_byFaceArray);
         this.Close();
      }

      private void CreateCurtainSystemDialog_FormClosing(object sender, FormClosingEventArgs e)
      {
         // just refresh the main UI
         m_mydocument.SystemData.CreateCurtainSystem(null, m_byFaceArray);
      }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void reverseSelection_Click(object sender, EventArgs e)
        {

        }

   }// end of class
}