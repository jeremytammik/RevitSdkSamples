//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using Autodesk.Revit.DB.Structure;

using System.Linq;

using MacroCSharpSamples;
using MacroSamples_RVT;

namespace Revit.SDK.Samples.SearchAndReplaceWinType.CS
{
   public class FindAndReplaceWinType
   {
      /// <summary>
      /// automatically replaces all windows of a given (hardcoded) type with another hardcoded type
      /// </summary>
      private Document? m_doc = null;

      /// <summary>
      /// Automatic print of all of a certain view type, to the default printer .
      /// </summary>
      private FindAndReplaceWinType()
      {
      }

      /// <summary>
      /// ctor wit parameter used to call this sample.
      /// </summary>
      /// <param name="doc"></param>
      public FindAndReplaceWinType(ThisApplication hostDoc)
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
            // filtrate the windows from the element set.

            ElementClassFilter filter1 = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter filter2 = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
            LogicalAndFilter andFilter = new LogicalAndFilter(filter1, filter2);
            FilteredElementCollector collector = new FilteredElementCollector(m_doc);
            ICollection<Element> arrayFamily = collector.WherePasses(andFilter).ToElements();

            // filtrate the Symbol from the element set to modify the window's type.


            ElementClassFilter filter3 = new ElementClassFilter(typeof(FamilySymbol));
            ElementCategoryFilter filter4 = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
            LogicalAndFilter andFilter1 = new LogicalAndFilter(filter3, filter4);
            collector = new FilteredElementCollector(m_doc);
            ICollection<Element> found = collector.WherePasses(andFilter1).ToElements();
            ElementArray arraySymbol = new ElementArray();
            foreach (Element ee in found)
            {
               if (ee.Name == "36\" x 72\"")
               {
                  arraySymbol.Insert(ee, 0);
                  break;
               }
            }

            MessageBox.Show("Replace 16\" x 24\" to 36\" x 72\".", "FindAndReplaceWinType");
            // matching and replacing
            int replacenum = 0;
            foreach (Element ee in arrayFamily)
            {

               FamilyInstance? windows = ee as FamilyInstance;
               if (windows == null)
                  return;
               if (0 == windows.Symbol.Name.CompareTo("16\" x 24\""))
               {
                  windows.Symbol = arraySymbol.get_Item(0) as FamilySymbol;
                  replacenum++;
               }

            }
            // Show the number of windows modified.
            MessageBox.Show("Revit has completed its search and has made " + replacenum + " modifications.", "FindAndReplaceWinType");
         }
         catch (Exception ee)
         {
            MessageBox.Show(ee.Message);
         }

      }
   }
}
