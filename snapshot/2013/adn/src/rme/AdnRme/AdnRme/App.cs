#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2013 by Jeremy Tammik, Autodesk, Inc.
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
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
#endregion // Namespaces

namespace AdnRme
{
  /// <summary>
  /// This MEP sample demonstrates use of the generic and the MEP 
  /// specific parts of the Revit API for tasks in Revit MEP.
  /// 
  /// The following HVAC tasks are adressed using the generic API:
  /// 
  /// - determine air terminals for each space.
  /// - assign flow to the air terminals depending on the space's calculated supply air flow.
  /// - change size of diffuser, i.e. type, based on flow.
  /// - populate the value of the 'CFM per SF' variable on all spaces.
  /// - enter two element id's and create a 3D sectioned box view of their extents.
  /// - determine unhosted elements (cf. SPR 134098).
  /// - reset demo to original state.
  /// 
  /// CFM = cubic feet per second, SF = square feet.
  /// Revit internal units are feet and seconds, so we need to multiply by 60 to get CFM.
  /// 
  /// The electrical conection hierarchy is determined and displayed in a tree view
  /// using both the generic Revit API, based on parameters, and the MEP specific API, 
  /// based on connectors.
  /// 
  /// For Revit 2009, we implemented an external application demonstrating how to create
  /// an own add-in menu. In Revit 2010, this was migrated to the ribbon and a custom panel.
  /// One can also use RvtSamples from the Revit SDK Samples directory to load
  /// this MEP sample, with the following additional entries in RvtSamples.txt:
  /// 
  /// ADN Rme
  /// About...
  /// About ADN RME API Samples
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2011\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdAbout
  /// 
  /// ADN Rme
  /// Electrical System Browser
  /// Inspect electrical systems in model and reproduce system browser info using parameter data only
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2011\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdElectricalSystemBrowser
  /// 
  /// #ADN Rme
  /// #Electrical Hierarchy
  /// #Inspect electrical systems in model and display full connection hierarchy tree structure
  /// #LargeImage:
  /// #Image:
  /// #C:\a\j\adn\train\revit\2011\src\rme\mep\bin\Debug\mep.dll
  /// #mep.CmdElectricalHierarchy
  /// #
  /// #ADN Rme
  /// #Electrical Hierarchy 2
  /// #Inspect electrical systems in model and display full connection hierarchy tree structure
  /// #LargeImage:
  /// #Image:
  /// #C:\a\j\adn\train\revit\2011\src\rme\mep\bin\Debug\mep.dll
  /// #mep.CmdElectricalHierarchy2
  /// 
  /// ADN Rme
  /// Electrical Hierarchy Tree
  /// Inspect electrical systems and connectors in model and display full hierarchy tree structure
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2011\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdElectricalConnectors
  /// 
  /// ADN Rme
  /// HVAC Assign flow to terminals
  /// Assign flow to terminals
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2011\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdAssignFlowToTerminals
  /// 
  /// ADN Rme
  /// HVAC Change size
  /// Change terminal sizes
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2011\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdChangeSize
  /// 
  /// ADN Rme
  /// HVAC Populate CFM per SF on rooms
  /// Populate CFM per SF variable on rooms
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2011\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdPopulateCfmPerSf
  /// 
  /// ADN Rme
  /// HVAC Reset demo
  /// Reset ADN RME API Demo
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2011\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdResetDemo
  /// 
  /// ADN Rme
  /// Unhosted elements
  /// List unhosted elementes
  /// LargeImage:
  /// Image:
  /// C:\a\j\adn\train\revit\2011\src\rme\mep\bin\Debug\mep.dll
  /// mep.CmdUnhostedElements
  /// </summary>
  class App : IExternalApplication
  {
    /// <summary>
    /// Create a ribbon panel for the MEP sample application.
    /// We present a column of three buttons: Electrical, HVAC and About.
    /// The first two include subitems, the third does not.
    /// </summary>
    static void AddRibbonPanel( 
      UIControlledApplication a )
    {
      const int nElectricalCommands = 3;

      const string m = "AdnRme.Cmd"; // namespace and command prefix

      string path = Assembly.GetExecutingAssembly().Location;

      string[] text = new string[] {
        "Electrical Connectors",
        "Electrical System Browser",
        //"Electrical Hierarchy",
        //"Electrical Hierarchy 2",
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
        //"ElectricalHierarchy",
        //"ElectricalHierarchy2",
        "UnhostedElements",
        "AssignFlowToTerminals",
        "ChangeSize",
        "PopulateCfmPerSf",
        "ResetDemo",
        "About"
      };

      int n = classNameStem.Length;

      Debug.Assert( text.Length == n, 
        "expected equal number of text and class name entries" );

      // Create three stacked buttons for the HVAC, 
      // electrical and about commands, respectively:

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

      IList<RibbonItem> ribbonItems = panel.AddStackedItems( 
        d1, d2, d3 );

      // Add subitems to the HVAC and 
      // electrical pulldown buttons:

      PulldownButton pulldown;
      PushButton pb;
      int i, j;
      
      for( i = 0; i < n; ++i )
      {
        j = i < nElectricalCommands ? 0 : 1;
        pulldown = ribbonItems[j] as PulldownButton;
        
        PushButtonData pbd = new PushButtonData( 
          text[i], text[i], path, m + classNameStem[i] );

        pb = pulldown.AddPushButton( pbd );

        pb.ToolTip = text[i];
      }
    }

    public Result OnStartup( UIControlledApplication a )
    {
      // only create a new ribbon panel in Revit MEP:

      ProductType pt = a.ControlledApplication.Product;

      //if( ProductType.MEP == pt ) // 2012

      if( ProductType.MEP == pt 
        || ProductType.Revit == pt ) // 2013
      {
        AddRibbonPanel( a );
        return Result.Succeeded;
      }
      return Result.Cancelled;
    }

    public Result OnShutdown( UIControlledApplication a )
    {
      return Result.Succeeded;
    }
  }
}

// C:\Program Files\Autodesk\Revit Architecture 2011\Program\Revit.exe
// C:\a\j\adn\train\revit\2011\src\rme\test\hvac_project.rvt

// C:\Program Files\Autodesk\Revit MEP 2011\Program\Revit.exe
// C:\a\j\adn\train\revit\2011\src\rme\test\elec_project.rvt
