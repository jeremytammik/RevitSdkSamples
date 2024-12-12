//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
   /// Sort parameters' order in family files which are located in a folder:
   /// <list type="bullet">
   /// <item>"Browse" button make users could choose a folder contains some families, and the selected folder will be shown in the text box.</item>
   /// <item>"A–>Z" button will update parameters in each family files located in the specific folder to alphabet order.</item>
   /// <item>"Z–>A" button will update parameters in each family files located in the specific folder to reverse alphabet order.</item>
   /// <item>"Close" button will close the whole dialog.</item>
   /// </list>
   /// Note: we don't update the family files in sub-folders in this example.
   /// </summary>
   public partial class SortFamilyFilesParamsForm : System.Windows.Forms.Form
   {
      private UIApplication m_uiApp;
      
      /// <summary>
      /// Construct with a UIApplication.
      /// </summary>
      /// <param name="uiApp"></param>
      public SortFamilyFilesParamsForm(UIApplication uiApp)
      {
         m_uiApp = uiApp;
         InitializeComponent();
      }

      private void browseBtn_Click(object sender, EventArgs e)
      {
         FolderBrowserDialog dialog = new FolderBrowserDialog();
         if (dialog.ShowDialog() == DialogResult.OK)
         {
            directoryTxt.Text = dialog.SelectedPath;
         }
      }

      /// <summary>
      /// Sort parameters' order in family files which is located in a folder, the new files are saved in subfolder named "ordered".
      /// </summary>
      /// <param name="order">Ascending or Descending.</param>
      private void SortParameters(ParametersOrder order)
      {
         // Convert relative path to absolute path.
         string absPath = directoryTxt.Text;
         if (!Path.IsPathRooted(absPath))
            absPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), absPath);

         DirectoryInfo dirInfo = new DirectoryInfo(absPath);
         if (!dirInfo.Exists)
         {
            MessageBox.Show("Please select a valid directory first.");
            return;
         }

         string orderedDir = Path.Combine(absPath, "ordered");
         if (!Directory.Exists(orderedDir))
            Directory.CreateDirectory(orderedDir);

         // Sort parameters in each family file.
         FileInfo[] fileInfo = dirInfo.GetFiles("*.rfa");
         foreach (FileInfo fInfo in fileInfo)
         {
               Document doc = m_uiApp.Application.OpenDocumentFile(fInfo.FullName);
               using (Transaction trans = new Transaction(doc, "Sort parameters."))
               {
                  trans.Start();
                  doc.FamilyManager.SortParameters(order);
                  trans.Commit();
               }

               string destFile = Path.Combine(orderedDir, fInfo.Name);
               if (File.Exists(destFile))
                  File.Delete(destFile);

               doc.SaveAs(destFile);
               doc.Close(false);
         }

         TaskDialog.Show("Message", "Sort completed! " + fileInfo.Count().ToString() + " family file(s) sorted.");
      }

      private void A_ZBtn_Click(object sender, EventArgs e)
      {
         SortParameters(ParametersOrder.Ascending);
      }

      private void Z_ABtn_Click(object sender, EventArgs e)
      {
         SortParameters(ParametersOrder.Descending);
      }

      private void closeBtn_Click(object sender, EventArgs e)
      {
         Close();
      }
   }
}
