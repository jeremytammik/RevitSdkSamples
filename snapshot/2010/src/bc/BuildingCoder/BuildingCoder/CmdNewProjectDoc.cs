#region Header
//
// CmdNewProjectDoc.cs - create a new project document
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
//using Autodesk.Revit.Collections;
//using Autodesk.Revit.Elements;
//using Autodesk.Revit.Parameters;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdNewProjectDoc : IExternalCommand
  {
    const string _template_file_path 
      = "C:/Documents and Settings/All Users"
      + "/Application Data/Autodesk/RAC 2010"
      + "/Metric Templates/DefaultMetric.rte";

    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;

      Document doc = app.NewProjectDocument( 
        _template_file_path );

      doc.SaveAs( "C:/tmp/new_project.rvt" );

      return CmdResult.Failed;
    }
  }
}
