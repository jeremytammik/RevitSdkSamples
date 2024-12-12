//
// (C) Copyright 2016 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.


using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace REX.DRevitFreezeDrawing.Main
{
   /// <summary>
   /// this class contains all needed Data from Revit program 
   /// </summary>
   class REXRevitData
   {
      #region<members>
      //*****************************************************************************
      private List<View> m_ViewList;
      private List<string> m_ViewListStr;
      private Autodesk.Revit.UI.ExternalCommandData m_CommandData;

      //*****************************************************************************
      #endregion

      #region<properties>

      //*****************************************************************************
      public List<View> ViewList
      {
         get
         {
            return m_ViewList;
         }
      }
      //*****************************************************************************
      #endregion

      #region<constructors>
      //*****************************************************************************
      public REXRevitData(Autodesk.Revit.UI.ExternalCommandData argCommandData)
      {
         m_ViewListStr = new List<string>();
         m_ViewList = new List<View>();
         m_CommandData = argCommandData;
      }
      //*****************************************************************************
      #endregion

      #region<method>
      //*****************************************************************************
      /// <summary>
      /// this function import data from Revit to application
      /// </summary>
      public void Initialize()
      {
         FilteredElementCollector collector = new FilteredElementCollector(m_CommandData.Application.ActiveUIDocument.Document);
         IList<Element> elements = collector.OfClass(typeof(View)).ToElements();

         foreach (Element el in elements)
         {
            View vw = el as View;
            if (null != vw)
            {
               if (vw.CanBePrinted)
               {
                  if (!(vw.ViewType == Autodesk.Revit.DB.ViewType.DrawingSheet))
                  {
                     m_ViewList.Add(vw);
                     m_ViewListStr.Add(vw.Name);
                  }

               }

               continue;
            }
         }

         m_ViewListStr.Sort();
         m_ViewList.Sort(CompareViewsByName);

      }
      //*****************************************************************************
      /// <summary>
      /// This function is an Comparison to sort views by name
      /// </summary>
      private static int CompareViewsByName(View arg1, View arg2)
      {
         return arg1.Name.CompareTo(arg2.Name);
      }
      //*****************************************************************************
      /// <summary>
      /// this function allows to get view according to name
      /// </summary>
      public View GetView(string argKey)
      {
         for (int i = 0; i <= m_ViewList.Count - 1; i++)
         {
            if (m_ViewList[i].Name == argKey)
               return m_ViewList[i];
         }
         return null;
      }
      #endregion
   }
}
