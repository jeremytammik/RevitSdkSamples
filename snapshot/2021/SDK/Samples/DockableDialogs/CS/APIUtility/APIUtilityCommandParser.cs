//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reflection;
using System.Drawing;
using System.Configuration;
using System.Collections.Generic;
using Autodesk.Revit.DB.Macros;
using Autodesk.Revit.UI.Macros;
using System.Collections.Specialized;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.DockableDialogs.CS
{
   public partial class APIUtility
   {
      /// <summary>
      /// A method to examine an incoming command and start the process of executing
      /// safely in the Idle() or ExternalEvent() loop.
      /// </summary>
      public void RunModelessCommand(ModelessCommandData command)
      {
         switch (command.CommandType)
         {
            case ModelessCommandType.PrintMainPageStatistics:
               {
                  command.WindowSummaryData = GetPaneSummary(ThisApplication.thisApp.MainPageDockablePaneId);
                  ModelessCommand.Make(command);
                  break;
               }

            case ModelessCommandType.PrintSelectedPageStatistics:
               {
                  command.WindowSummaryData = GetPaneSummary(command.SelectedPaneId);
                  ModelessCommand.Make(command);
                  break;
               }

 
            default:
               break;
         }
      }
   }
}
