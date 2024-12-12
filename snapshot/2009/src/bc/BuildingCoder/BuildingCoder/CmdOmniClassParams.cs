#region Header
//
// CmdOmniClassParams.cs - extract OmniClass 
// parameter data from all elements
//
// Copyright (C) 2009 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
//using Autodesk.Revit.Elements;
using Autodesk.Revit.Enums;
using Autodesk.Revit.Parameters;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

#if _2010
namespace BuildingCoder
{
  class CmdOmniClassParams : IExternalCommand
  {
    BuiltInParameter _bipCode 
      = BuiltInParameter.OMNICLASS_CODE;

    BuiltInParameter _bipDesc 
      = BuiltInParameter.OMNICLASS_DESCRIPTION;

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      List<Element> set = new List<Element>();

      ParameterFilter f 
        = app.Create.Filter.NewParameterFilter(
          _bipCode, 
          CriteriaFilterType.NotEqual, 
          string.Empty );

      ElementIterator it = doc.get_Elements( f );

      using( StreamWriter sw 
        = File.CreateText( "C:/omni.txt" ) )
      {
        while( it.MoveNext() )
        {
          Element e = it.Current as Element;
          sw.WriteLine( string.Format( 
            "{0} code {1} desc {2}", 
            Util.ElementDescription( e ),
            e.get_Parameter( bipCode ).AsString(),
            e.get_Parameter( bipDesc ).AsString() ) );
        }
        sw.Close();
      }
      return IExternalCommand.Result.Failed;
    }
  }
}
#endif // _2010