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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.FamilyParametersOrder.CS
{
   /// <summary>
   /// Sort parameters' order in families which are loaded into a project or a family:
   /// <list type="bullet">
   /// <item>"A–>Z" button will update parameters in each loaded family to alphabet order then reload the family to project.</item>
   /// <item>"Z–>A" button will update parameters in each loaded family to reverse alphabet order then reload the family to project.</item>
   /// <item>"Close" button will close the whole dialog.</item>
   /// </list>
   /// </summary>
   public partial class SortLoadedFamiliesParamsForm : System.Windows.Forms.Form
   {
      /// <summary>
      /// Document whose loaded families parameters' order want to be sort.
      /// </summary>
      private Document m_currentDoc;

      /// <summary>
      /// Construct with a Document.
      /// </summary>
      /// <param name="currentDoc"></param>
      public SortLoadedFamiliesParamsForm(Document currentDoc)
      {
         m_currentDoc = currentDoc;
         InitializeComponent();
      }

      /// <summary>
      /// Sort parameters of families which have been loaded into a project. 
      /// </summary>
      /// <param name="order">Ascending or Descending</param>
      private void SortParameters(ParametersOrder order)
      {
         try
         {
            FilteredElementCollector coll = new FilteredElementCollector(m_currentDoc);
            IList<Element> families = coll.OfClass(typeof(Family)).ToElements();

            // Edit each family->sort parameters order->save to a new file->load back to the document.
            int count = 0;
            foreach (Family fam in families)
            {
               if (!fam.IsEditable)
                  continue;

               Document famDoc = m_currentDoc.EditFamily(fam);

               using (Transaction trans = new Transaction(famDoc, "Sort parameters."))
               {
                  trans.Start();
                  famDoc.FamilyManager.SortParameters(order);
                  trans.Commit();
               }

               string tmpFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), fam.Name + ".rfa");

               if (File.Exists(tmpFile))
                  File.Delete(tmpFile);
               
               famDoc.SaveAs(tmpFile);
               famDoc.Close(false);
               
               using (Transaction trans = new Transaction(m_currentDoc, "Load family."))
               {
                  trans.Start();
                  IFamilyLoadOptions famLoadOptions = new FamilyLoadOptions();
                  Family newFam = null;
                  m_currentDoc.LoadFamily(tmpFile, new FamilyLoadOptions(), out newFam);
                  trans.Commit();
               }

               File.Delete(tmpFile);
               count++;
            }

            TaskDialog.Show("Message", "Sort completed! " + count.ToString() + " families sorted.");
         }
         catch (Exception ex)
         {
            MessageBox.Show("Error:" + ex.Message);
         }
      }

      /// <summary>
      /// Sort families parameters with ascending.
      /// </summary>
      /// <param name="sender">Not used.</param>
      /// <param name="e">Not used.</param>
      private void A_ZBtn_Click(object sender, EventArgs e)
      {
         SortParameters(ParametersOrder.Ascending);
      }

      /// <summary>
      /// Sort families parameters with descending.
      /// </summary>
      /// <param name="sender">Not used.</param>
      /// <param name="e">Not used.</param>
      private void Z_ABtn_Click(object sender, EventArgs e)
      {
         SortParameters(ParametersOrder.Descending);
      }

      /// <summary>
      /// Close this dialog.
      /// </summary>
      /// <param name="sender">Not used.</param>
      /// <param name="e">Not used.</param>
      private void closeBtn_Click(object sender, EventArgs e)
      {
         Close();
      }

      private class FamilyLoadOptions : IFamilyLoadOptions
      {
         public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
         {
            overwriteParameterValues = true;
            return true;
         }

         public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
         {
            source = FamilySource.Family;
            overwriteParameterValues = true;
            return true;
         }
      }
   }
}
