#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2008 by Jeremy Tammik, Autodesk, Inc.
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
using Autodesk.Revit;
using CmdResult = Autodesk.Revit.IExternalApplication.Result;
#endregion // Namespaces

namespace mep
{
  /// <summary>
  /// External application demonstrating use of the generic Revit API for tasks in Revit MEP.
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
    #region Menu Helper
    /// <summary>
    /// Add this external application's menu to the Revit menu.
    /// </summary>
    private static void AddMenu( ControlledApplication app )
    {
      const string m = "mep.Cmd"; // namespace and command prefix
      string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
      Autodesk.Revit.MenuItem rootMenu = app.CreateTopMenu( "ME&P API Samples" );
      //bool success = rootMenu.AddToExternalTools();
      MenuItem.MenuType mt = MenuItem.MenuType.BasicMenu;
      rootMenu.Append( mt, "&Assign flow to terminals", path, m + "AssignFlowToTerminals" );
      rootMenu.Append( mt, "&Change size", path, m + "ChangeSize" );
      rootMenu.Append( mt, "&Populate CFM per SF on spaces", path, m + "PopulateCfmPerSf" );
      rootMenu.Append( mt, "&Unhosted elements", path, m + "UnhostedElements" );
      rootMenu.Append( mt, "&Reset demo", path, m + "ResetDemo" );
      rootMenu.Append( MenuItem.MenuType.SeparatorMenu );
      rootMenu.Append( mt, "Electrical &System Browser", path, m + "ElectricalSystemBrowser" );
      rootMenu.Append( mt, "Electrical &Hierarchy", path, m + "ElectricalHierarchy" );
      rootMenu.Append( mt, "Electrical Hierarchy &2", path, m + "ElectricalHierarchy2" );
      rootMenu.Append( MenuItem.MenuType.SeparatorMenu );
      rootMenu.Append( mt, "A&bout...", path, m + "About" );
    }
    #endregion // Menu Helper

    #region IExternalApplication Members

    public CmdResult OnStartup( ControlledApplication app )
    {
      AddMenu( app );
      return CmdResult.Succeeded;
    }

    public CmdResult OnShutdown( ControlledApplication app )
    {
      return CmdResult.Succeeded;
    }
    #endregion // IExternalApplication Members
  }
}
