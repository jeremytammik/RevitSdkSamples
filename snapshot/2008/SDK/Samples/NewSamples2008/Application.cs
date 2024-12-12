#region Header
// Copyright (C) 2007 by Autodesk, Inc.
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
// AUTODESK, INC. DOES NOT WARRANT THAT THE OPERATION
// OF THE PROGRAM WILL BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject
// to restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Diagnostics;
using System.IO;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
#endregion // Namespaces

namespace NewSamples2008
{
  public class Application : IExternalApplication
  {
    #region Menu Helpers
    private void AddMenuSeparator( MenuItem rootMenu )
    {
      rootMenu.Append( Autodesk.Revit.MenuItem.MenuType.SeparatorMenu );
    }

    private bool AddMenuItem( MenuItem rootMenu, string name, string rootpath, string subpath, string className, bool isVb )
    {
      string languagePath = isVb ? "VB.NET" : "CS";
      if( null == subpath )
      {
        subpath = name + "\\bin\\Debug\\" + name + ".dll";
      }
      string path = Path.Combine( rootpath, subpath );
      bool rc = File.Exists( path );
      if( !rc )
      {
        subpath = name + "\\" + languagePath + "\\bin\\Debug\\" + name + ".dll";
        path = Path.Combine( rootpath, subpath );
        rc = File.Exists( path );
        if( !rc )
        {
        }
      }
      if( rc )
      {
        if( null == className )
        {
          className = "Revit.SDK.Samples." + name + "." + languagePath + ".Command";
        }
        rootMenu.Append( Autodesk.Revit.MenuItem.MenuType.BasicMenu, name, path, className );
      }
      return rc;
    }

    private void AddMenu( ControlledApplication app )
    {
      string root = System.Reflection.Assembly.GetExecutingAssembly().Location;
      root = Path.GetDirectoryName( root );
      root = Path.GetDirectoryName( root );
      root = Path.GetDirectoryName( root );
      root = Path.GetDirectoryName( root );
      //Debug.Assert( root.StartsWith( @"C:\a\lib\revit\2008\sdk\Samples" ), "expected valid SDK samples root path" );
      Debug.Assert( root.ToLower().EndsWith( @"\sdk\samples" ), "expected valid SDK samples root path" );
      Autodesk.Revit.MenuItem rootMenu = app.CreateTopMenu( "New Samples" );
      // optional: bool success = rootMenu.AddToExternalTools();
      // APIAppStartup
      // ApplicationEvents
      AddMenuItem( rootMenu, "AutoTagRooms", root, @"AutoTagRooms\bin\Debug\RoomsTag.dll", "Revit.SDK.Samples.RoomsTag.CS.Command", false );
      AddMenuItem( rootMenu, "BlendVertexConnectTable", root, @"BlendVertexConnectTable\bin\Debug\BlendVertexConnectTable.dll", "Revit.SDK.Samples.BlendVertexConnectTable.CS.Command", false );
      AddMenuItem( rootMenu, "CurvedBeam", root, @"CurvedBeam\CS\bin\Debug\CurvedBeam.dll", "Revit.SDK.Samples.CurvedBeam.CS.Command", false );
      AddMenuItem( rootMenu, "FamilyExplorer", root, @"FamilyExplorer\bin\Debug\FamilyExplorer.dll", "Revit.SDK.Samples.FamilyExplorer.CS.Command", false );
      AddMenuItem( rootMenu, "FramingBuilder", root, @"FramingBuilder\CS\bin\Debug\FramingBuilder.dll", "Revit.SDK.Samples.FramingBuilder.CS.Command", false );
      AddMenuItem( rootMenu, "ImportExportDWG", root, @"ImportExportDWG\CS\bin\Debug\ImportAndExportForDWG.dll", "Revit.SDK.Samples.ImportAndExportForDWG.CS.Command", false );
      AddMenuItem( rootMenu, "InplaceFamilyAnalyticalModel3D", root, @"InplaceFamilyAnalyticalModel3D\CS\bin\Debug\InplaceFamilyAnalyticalModel3D.dll", "Revit.SDK.Samples.InplaceFamilyAnalyticalModel3D.CS.Command", false );
      AddMenuItem( rootMenu, "NewOpenings", root, @"NewOpenings\CS\bin\Debug\NewOpenings.dll", "Revit.SDK.Samples.NewOpenings.CS.Command", false );
      AddMenuItem( rootMenu, "NewPathReinforcement", root, null, null, false );
      AddMenuItem( rootMenu, "PathReinforcement", root, null, null, false );
      AddMenuItem( rootMenu, "ProjectInfo", root, null, null, false );
      AddMenuItem( rootMenu, "ProjectUnit", root, null, null, false );
      AddMenuItem( rootMenu, "ShaftHolePuncher", root, null, null, false );
      AddMenuItem( rootMenu, "SpotDimension", root, null, null, false );
      AddMenuItem( rootMenu, "TagBeam", root, null, null, false );
      AddMenuItem( rootMenu, "TestFloorThickness", root, null, null, true );
      AddMenuItem( rootMenu, "TestWallThickness", root, null, null, true );
      //AddMenuItem( rootMenu, "Toolbar", root, null, null, false );
      AddMenuItem( rootMenu, "TransactionControl", root, @"TransactionControl\bin\Debug\Transaction.dll", "Revit.SDK.Samples.Transaction.CS.Command", false );
      AddMenuItem( rootMenu, "ViewPrinter", root, null, null, false );
      AddMenuItem( rootMenu, "VisibilityControl", root, @"C:\a\lib\revit\2008\sdk\Samples\VisibilityControl\bin\Debug\VisibilityController.dll", "Revit.SDK.Samples.VisibilityController.CS.Command", false );
    }
    #endregion // Menu Helpers

    #region IExternalApplication Members

    IExternalApplication.Result IExternalApplication.OnStartup( ControlledApplication app )
    {
      AddMenu( app );
      return IExternalApplication.Result.Succeeded;
    }

    IExternalApplication.Result IExternalApplication.OnShutdown( ControlledApplication app )
    {
      return IExternalApplication.Result.Succeeded;
    }

    #endregion // IExternalApplication Members
  }
}
