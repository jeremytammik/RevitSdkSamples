//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.MSOperation.CS
{
   /// <summary>
   /// The enum for selection operation in this sample. 
   /// </summary>
   internal enum OperationAction
   {
      /// <summary>
      /// add operation - add stairs or level landings.
      /// </summary>
      Add,

      /// <summary>
      /// remove operation - remove stairs or level landings
      /// </summary>
      Remove,

      /// <summary>
      /// Unpin operation - unpin an instance of stairs to create single stairs 
      /// </summary>
      Unpin,

      PinBack,
   }

   /// <summary>
   /// The selection filter to filter out the levels which can be selected during adding/removing stairs operation.
   /// </summary>
   internal class LevelSelectionFilter : ISelectionFilter
   {
      private MultistoryStairs m_MS = null;
      private OperationAction m_action;

      public LevelSelectionFilter(MultistoryStairs ms, OperationAction action)
      {
         m_MS = ms;
         m_action = action;
      }

      public bool AllowElement(Autodesk.Revit.DB.Element elem)
      {
         if (!(elem is Level))
            return false;
         if (null == m_MS)
            return false;
         if (OperationAction.Add == m_action)
            return m_MS.CanAddStair(elem.Id);
         if (OperationAction.Remove == m_action)
            return m_MS.CanRemoveStair(elem.Id);
         if(OperationAction.Unpin == m_action)
            return m_MS.CanUnpin(elem.Id);
         if (OperationAction.PinBack == m_action)
            return m_MS.CanPin(elem.Id);
         return false;
      }

      public bool AllowReference(Autodesk.Revit.DB.Reference reference, Autodesk.Revit.DB.XYZ position)
      {
         return false;
      }
   }
}
