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
using System.Text;
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;

using MacroCSharpSamples;
using MacroSamples_RVT;

namespace Revit.SDK.Samples.CapitalizeText.CS
{
   /// <summary>
   /// automatically replaces all text in text notes with capitalized text.
   /// </summary>
   public class CapitalizeText
   {
      private Document? m_doc = null;

      /// <summary>
      /// Automatic print of all of a certain view type, to the default printer .
      /// </summary>
      private CapitalizeText()
      {
      }

      /// <summary>
      /// ctor wit parameter used to call this sample.
      /// </summary>
      /// <param name="doc"></param>
      public CapitalizeText(ThisApplication hostDoc)
      {
         m_doc = hostDoc.ActiveUIDocument.Document;
      }

      /// <summary>
      /// run this sample now
      /// </summary>
      public void Run()
      {
         try
         {
            TextElement? text = null;

            // filtrate the TextElment from the element set
            ElementClassFilter gridFilter = new ElementClassFilter(typeof(TextElement));
            FilteredElementCollector collector = new FilteredElementCollector(m_doc);
            collector.WherePasses(gridFilter);
            IList<Element> arrayText = collector.ToElements();

            // matching and capitalizing
            int capitalizednum = 0;
            foreach (Element ee in arrayText)
            {
               text = ee as TextElement;
               if (text == null)
                  continue;
               text.Text = text.Text.ToUpper();
               capitalizednum++;

            }

            // Show the number of notes modified.
            MessageBox.Show("Revit has completed its search and has made " + capitalizednum + " modifications.", "CapitalizeText");
         }
         catch (Exception ee)
         {
            MessageBox.Show(ee.Message);
         }
      }
   }
}
