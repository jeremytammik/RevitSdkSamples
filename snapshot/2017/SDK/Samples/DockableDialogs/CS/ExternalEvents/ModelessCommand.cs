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
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Data;
using System.Collections.Generic;
using System.Diagnostics;

namespace Revit.SDK.Samples.DockableDialogs.CS
{

   /// <summary>
   /// A thread-safe class for getting and setting modeless-command data.
   /// </summary>
      public class ModelessCommand
      {

         /// <summary>
         /// Set data into the command.
         /// </summary>
         public void Make(ModelessCommandData commandData)
         {
            lock (this)
            {
               m_data = commandData;
            }
         }

         /// <summary>
         /// Get data from the command.
         /// </summary>
         public ModelessCommandData Take()
         {
            lock (this)
            {
               return m_data;
            }

         }
         private ModelessCommandData m_data = new ModelessCommandData();

      }
      public enum ModelessCommandType : int
      {
         PrintMainPageStatistics,
         PrintSelectedPageStatistics,
         Return
      }

      public class ModelessCommandData
      {
         public ModelessCommandData() { }
         public ModelessCommandType CommandType;
         public string WindowSummaryData;
         public string SelectedPaneId; 
      }
   }

