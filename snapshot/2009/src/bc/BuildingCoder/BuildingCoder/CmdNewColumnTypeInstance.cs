#region Header
//
// CmdNewColumnTypeInstance.cs - create a new 
// column type and insert an instance of it
//
// Copyright (C) 2009 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Symbols;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
using XYZ = Autodesk.Revit.Geometry.XYZ;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdNewColumnTypeInstance : IExternalCommand
  {
    const string family_name 
      = "M_Rectangular Column";

    const string extension 
      = ".rfa";

    const string directory 
      = "C:/Documents and Settings/All Users"
      + "/Application Data/Autodesk/RAC 2009"
      + "/Metric Library/Columns/";

    const string path 
      = directory + family_name + extension;

    StructuralType nonStructural 
      = StructuralType.NonStructural;

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      CmdResult rc 
        = CmdResult.Failed;

      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      //
      // get all family elements in current document
      // in order to check whether the family we are 
      // interested in is loaded:
      //
      List<Element> symbols = new List<Element>();

      Filter filter = app.Create.Filter.NewFamilyFilter(
        family_name );

      doc.get_Elements( filter, symbols );
      //
      // the family filter returns both the 
      // symbols and the family itself:
      //
      Family f = null;
      foreach( Element e in symbols )
      {
        if( e is Family )
        {
          f = e as Family;
        }
        else if( e is FamilySymbol )
        {
          FamilySymbol s = e as FamilySymbol;
          Debug.Print( 
            "Family name={0}, symbol name={1}", 
            s.Family.Name, s.Name );
        }
      }
      //
      // if the family was not already loaded, then do so:
      //
      if( null == f )
      {
        if( !doc.LoadFamily( path, ref f ) )
        {
          message = "Unable to load '" + path + "'.";
        }
      }

      if( null != f )
      {
        Debug.Print( "Family name={0}", f.Name );
        //
        // pick a symbol for duplication, any one will do,
        // we select the first:
        //
        FamilySymbol s = null;
        foreach( FamilySymbol s2 in f.Symbols )
        {
          s = s2;
          break;
        }
        Debug.Assert( null != s, 
          "expected at least one symbol"
          + " to be defined in family" );
        //
        // duplicate the existing symbol:
        //
        Symbol s1 = s.Duplicate( "Nuovo simbolo" );
        s = s1 as FamilySymbol;
        //
        // analyse the symbol parameters:
        //
        foreach( Parameter param in s.Parameters )
        {
          Debug.Print( 
            "Parameter name={0}, value={1}", 
            param.Definition.Name, 
            param.AsValueString() );
        }
        //
        // define new dimensions for our new type;
        // the specified parameter name is case sensitive:
        //
        s.get_Parameter( "Width" ).Set( 
          Util.MmToFoot( 500 ) );

        s.get_Parameter( "Depth" ).Set( 
          Util.MmToFoot( 1000 ) );
        //
        // we can change the symbol name at any time:
        //
        s.Name = "Nuovo simbolo due";
        //
        // insert an instance of our new symbol:
        //
        XYZ p = XYZ.Zero;
        doc.Create.NewFamilyInstance( 
          p, s, nonStructural );
        //
        // for a column, the reference direction is ignored:
        //
        //XYZ normal = new XYZ( 1, 2, 3 );
        //doc.Create.NewFamilyInstance( 
        //  p, s, normal, null, nonStructural );
        rc = CmdResult.Succeeded;
      }
      return rc;
    }
  }
}
