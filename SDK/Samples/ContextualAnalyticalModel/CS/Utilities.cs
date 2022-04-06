﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;

namespace ContextualAnalyticalModel
{
   /// <summary>
   /// Utility methods
   /// </summary>
   public static class Utilities
   {
      /// <summary>
      /// Selects a Revit element and returns its ElementId
      /// </summary>
      /// <param name="uiDoc">UIDocument</param>
      /// <param name="msg">status message</param>
      /// <returns>ElementId of the selected element</returns>
      public static ElementId GetSelectedObject(UIDocument uiDoc, string msg)
      {
         ElementId selectedElementId = ElementId.InvalidElementId;
         Reference refElem = uiDoc.Selection.PickObject(ObjectType.Element, msg);
         if (refElem != null)
         {
            selectedElementId = refElem.ElementId;
         }

         return selectedElementId;
      }
   }
}
