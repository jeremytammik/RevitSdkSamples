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
using VIEW = Autodesk.Revit.DB.View;

namespace Revit.SDK.Samples.QuickPrint.CS
{
   /// <summary>
   /// Automatic print of all of a certain view type, to the default printer .
   /// </summary>
   public class QuickPrint
   {
      private Document? m_doc = null;

      /// <summary>
      /// Default constructor without parameter is not allowed.
      /// </summary>
      private QuickPrint()
      {
      }

      /// <summary>
      /// ctor wit parameter used to call this sample.
      /// </summary>
      /// <param name="doc"></param>
      public QuickPrint(ThisApplication hostDoc)
      {
         m_doc = hostDoc.ActiveUIDocument.Document;
      }

      /// <summary>
      /// print the specified ViewType.
      /// </summary>
      public void Print(ViewType viewType)
      {
         try
         {
            if (m_doc == null)
               return;
            Autodesk.Revit.DB.View? view = null;

            // Create a view set to contain all view of designated type
            ViewSet views = m_doc.Application.Create.NewViewSet();

            // Create a filter to filtrate the View Element.
            ElementClassFilter fileterView = new ElementClassFilter(typeof(VIEW));
            FilteredElementCollector collector = new FilteredElementCollector(m_doc);
            collector.WherePasses(fileterView);

            IList<Element> arrayView = collector.ToElements();

            // filtrate the designated type views.
            foreach (Element ee in arrayView)
            {
               view = ee as VIEW;
               if ((null != view) && (viewType == view.ViewType) && view.IsTemplate == false)
               {
                  views.Insert(view);
               }

            }

            // print
            if (!views.IsEmpty)
            {
               m_doc.Print(views);
            }
            else
            {
               MessageBox.Show("No " + viewType.ToString() + " view to be printed!", "QuickPrint");
            }
         }
         catch (Exception ee)
         {
            MessageBox.Show(ee.Message);
         }
      }
   }
}
