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
using System.Linq;
using System.Text;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ExternalCommandRegistration.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommandAvailability, 
   /// determine when to enable\disable the corresponding external command by 
   /// return value from IsCommandAvailable function.
   /// Corresponding command will be disabled when a wall selected by user in this case.
   /// </summary>
   class WallSelection : IExternalCommandAvailability
   {
      #region IExternalCommandAvailability Members

      public bool IsCommandAvailable(UIApplication applicationData, 
         Autodesk.Revit.DB.CategorySet selectedCategories)
      {
         System.Collections.IEnumerator iterCategory = selectedCategories.GetEnumerator();
         iterCategory.Reset();
         while (iterCategory.MoveNext())
         {
            Autodesk.Revit.DB.Category category = (Autodesk.Revit.DB.Category)(iterCategory.Current);
            if (category.Name == "Walls")
            {
               return false;
            } 
         }
         return true;
      }

      #endregion
   }

   /// <summary>
   /// Implements the Revit add-in interface IExternalCommandAvailability, 
   /// determine when to enable\disable the corresponding external command by 
   /// return value from IsCommandAvailable function.
   /// Corresponding command will be disabled if active document is not a 3D view.
   /// </summary>
   class View3D : IExternalCommandAvailability
   {
      #region IExternalCommandAvailability Members

      public bool IsCommandAvailable(UIApplication applicationData, 
         Autodesk.Revit.DB.CategorySet selectedCategories)
      {
         Autodesk.Revit.DB.View activeView = applicationData.ActiveUIDocument.Document.ActiveView;
         if (Autodesk.Revit.DB.ViewType.ThreeD == activeView.ViewType)
         {
            return true;
         }
         else 
         {
            return false;
         }
      }

      #endregion
   }
}