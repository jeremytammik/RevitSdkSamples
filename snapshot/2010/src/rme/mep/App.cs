#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2009 by Jeremy Tammik, Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software
// for any purpose and without fee is hereby granted, provided
// that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  
// AUTODESK, INC. DOES NOT WARRANT THAT THE OPERATION OF THE 
// PROGRAM WILL BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject
// to restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Autodesk.Revit;
using CmdResult = Autodesk.Revit.IExternalApplication.Result;
#endregion // Namespaces

namespace mep
{
  /// <summary>
  /// For Revit 2009, we implemented an external application demonstrating how to create
  /// an own add-in menu. 
  /// 
  /// In Revit 2010, this would have to be migrated to the ribbon and a custom panel.
  /// 
  /// In Revt 2010, we are using RvtSamples the the Revit SDK Samples directory to load
  /// this MEP sample instead, with the following additional entires in RvtSamples.txt:
  /// 
  /// ADN Rme
  /// About...
  /// About ADN RME API Samples
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2010\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdAbout
  /// 
  /// ADN Rme
  /// HVAC Assign flow to terminals
  /// Assign flow to terminals
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2010\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdAssignFlowToTerminals
  /// 
  /// ADN Rme
  /// HVAC Change size
  /// Change terminal sizes
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2010\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdChangeSize
  /// 
  /// ADN Rme
  /// HVAC Populate CFM per SF on rooms
  /// Populate CFM per SF variable on rooms
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2010\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdPopulateCfmPerSf
  /// 
  /// ADN Rme
  /// HVAC Reset demo
  /// Reset ADN RME API Demo
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2010\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdResetDemo
  /// 
  /// ADN Rme
  /// Electrical System Browser
  /// Inspect electrical systems in model and reproduce system browser info
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2010\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdElectricalSystemBrowser
  /// 
  /// ADN Rme
  /// Electrical Hierarchy
  /// Inspect electrical systems in model and display full connection hierarchy tree structure
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2010\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdElectricalHierarchy
  /// 
  /// ADN Rme
  /// Electrical Hierarchy 2
  /// Inspect electrical systems in model and display full connection hierarchy tree structure
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2010\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdElectricalHierarchy2
  /// 
  /// ADN Rme
  /// Unhosted elements
  /// List unhosted elementes
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2010\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdUnhostedElements
  /// 
  /// This MEP sample demonstrates use of the generic Revit API for tasks in Revit MEP.
  /// The following tasks are adressed:
  /// - determine air terminals for each space.
  /// - assign flow to the air terminals depending on the space's calculated supply air flow.
  /// - change size of diffuser, i.e. type, based on flow.
  /// - populate the value of the 'CFM per SF' variable on all spaces.
  /// - enter two element id's and create a 3D sectioned box view of their extents.
  /// - determine unhosted elements (cf. SPR 134098).
  /// - reset demo to original state.
  /// CFM = cubic feet per second, SF = square feet.
  /// Revit internal units are feet and seconds, so we need to multiply by 60 to get CFM.).
  /// </summary>
  class App : IExternalApplication
  {
    #region Create top level menu in Revit 2009
#if IMPLEMENT_EXTERNAL_APPLICATION_MENU_FOR_REVIT_2009
    /// <summary>
    /// Add this external application's menu to the Revit menu.
    /// </summary>
    static void AddMenu2009( ControlledApplication a )
    {
      const string m = "mep.Cmd"; // namespace and command prefix
      string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
      Autodesk.Revit.MenuItem rootMenu = a.CreateTopMenu( "ME&P API Samples" );
      //bool success = rootMenu.AddToExternalTools();
      MenuItem.MenuType mt = MenuItem.MenuType.BasicMenu;
      rootMenu.Append( mt, "&Assign flow to terminals", path, m + "AssignFlowToTerminals" );
      rootMenu.Append( mt, "&Change size", path, m + "ChangeSize" );
      rootMenu.Append( mt, "&Populate CFM per SF on spaces", path, m + "PopulateCfmPerSf" );
      rootMenu.Append( mt, "&Reset demo", path, m + "ResetDemo" );
      rootMenu.Append( MenuItem.MenuType.SeparatorMenu );
      rootMenu.Append( mt, "Electrical &System Browser", path, m + "ElectricalSystemBrowser" );
      rootMenu.Append( mt, "Electrical &Hierarchy", path, m + "ElectricalHierarchy" );
      rootMenu.Append( mt, "Electrical Hierarchy &2", path, m + "ElectricalHierarchy2" );
      rootMenu.Append( mt, "&Unhosted elements", path, m + "UnhostedElements" );
      rootMenu.Append( MenuItem.MenuType.SeparatorMenu );
      rootMenu.Append( mt, "A&bout...", path, m + "About" );
    }
#endif // IMPLEMENT_EXTERNAL_APPLICATION_MENU_FOR_REVIT_2009
    #endregion // Create top level menu in Revit 2009

    #region Create custom ribbon panel in Revit 2010
    /// <summary>
    /// Create a ribbon panel for the MEP sample application.
    /// We present a column of three buttons: Electrical, HVAC and About.
    /// The first two include subitems, the third does not.
    /// </summary>
    static void AddRibbonPanel( 
      ControlledApplication a )
    {
      const string m = "mep.Cmd"; // namespace and command prefix
      string path = Assembly.GetExecutingAssembly().Location;

      string[] text = new string[] {
        "Electrical Connectors",
        "Electrical System Browser",
        "Electrical Hierarchy",
        "Electrical Hierarchy 2",
        "Unhosted elements",
        "Assign flow to terminals",
        "Change size",
        "Populate CFM per SF on spaces",
        "Reset demo",
        "About..."
      };

      string[] classNameStem = new string[] {
        "ElectricalConnectors",
        "ElectricalSystemBrowser",
        "ElectricalHierarchy",
        "ElectricalHierarchy2",
        "UnhostedElements",
        "AssignFlowToTerminals",
        "ChangeSize",
        "PopulateCfmPerSf",
        "ResetDemo",
        "About"
      };

      int n = classNameStem.Length;
      Debug.Assert( text.Length == n, "expected equal number of text and class name entries" );
      //
      // create three stacked buttons for the 
      // HVAC, electrical and about commands respectively:
      //
      RibbonPanel panel = a.CreateRibbonPanel( 
        "MEP Sample" );

      PulldownButtonData d1 = new PulldownButtonData( 
        "Electrical", "Electrical" );

      d1.ToolTip = "Electrical Commands";

      PulldownButtonData d2 = new PulldownButtonData(
        "Hvac", "HVAC" );

      d2.ToolTip = "HVAC Commands";

      n = n - 1;

      PushButtonData d3 = new PushButtonData( 
        classNameStem[n], text[n], path, m + classNameStem[n] );

      d3.ToolTip = "About the HVAC and Electrical MEP Sample.";

      List<RibbonItem> ribbonItems = panel.AddStackedButtons( 
        d1, d2, d3 );
      //
      // add subitems to the HVAC and electrical pulldown buttons:
      //
      PulldownButton pulldown;
      PushButton pb;
      int i, j;
      
      for( i = 0; i < n; ++i )
      {
        j = i < 5 ? 0 : 1;
        pulldown = ribbonItems[j] as PulldownButton;
        
        pb = pulldown.AddItem( text[i], path, 
          m + classNameStem[i] );

        pb.ToolTip = text[i];
      }
    }
    #endregion // Create custom ribbon panel in Revit 2010

    #region IExternalApplication Members

    public CmdResult OnStartup( ControlledApplication a )
    {
      //AddMenu2009( a );
      AddRibbonPanel( a );
      return CmdResult.Succeeded;
    }

    public CmdResult OnShutdown( ControlledApplication a )
    {
      return CmdResult.Succeeded;
    }
    #endregion // IExternalApplication Members
  }
}
