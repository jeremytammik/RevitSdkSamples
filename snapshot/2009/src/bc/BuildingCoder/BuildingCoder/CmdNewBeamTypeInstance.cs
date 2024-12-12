#region Header
//
// CmdNewBeamTypeInstance.cs - create a new 
// beam type and insert an instance of it
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
using Line = Autodesk.Revit.Geometry.Line;
using XYZ = Autodesk.Revit.Geometry.XYZ;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdNewBeamTypeInstance : IExternalCommand
  {
const string family_name
  = "M_Concrete-Rectangular Beam";

const string extension
  = ".rfa";

const string directory
  = "C:/Documents and Settings/All Users"
  + "/Application Data/Autodesk/RAC 2009"
  + "/Metric Library/Structural/Framing"
  + "/Concrete/";

const string path
  = directory + family_name + extension;

StructuralType stBeam 
  = StructuralType.Beam;

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      CmdResult rc = CmdResult.Failed;
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      //
      // get all family elements in current document
      // in order to check whether the family we are interested in is loaded:
      //
      List<Element> symbols = new List<Element>();
      Filter filterFamily = app.Create.Filter.NewFamilyFilter( family_name );
      doc.get_Elements( filterFamily, symbols );
      //
      // the family filter returns both the symbols and the family itself:
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
          Debug.Print( "Family name={0}, symbol name={1}", s.Family.Name, s.Name );
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
        Debug.Assert( null != s, "expected at least one symbol to be defined in family" );
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
          Debug.Print( "Parameter name={0}, value={1}", param.Definition.Name, param.AsValueString() );
        }
        //
        // define new dimensions for our new type;
        // the specified parameter name is case sensitive:
        //
        s.get_Parameter( "b" ).Set( 
          Util.MmToFoot( 500 ) );

        s.get_Parameter( "h" ).Set( 
          Util.MmToFoot( 1000 ) );
        //
        // we can change the symbol name at any time:
        //
        s.Name = "Nuovo simbolo due";
        //
        // insert an instance of our new symbol:
        //
        //
        // it is possible to insert a beam, which normally uses a location line,
        // by specifying only a location point:
        //
        //XYZ p = XYZ.Zero;
        //doc.Create.NewFamilyInstance( p, s, nonStructural );
        //
        XYZ p = XYZ.Zero;
        XYZ q = app.Create.NewXYZ( 30, 20, 20 ); // feet
        Line line = app.Create.NewLineBound( p, q );
        //
        // specifying a non-structural type here means no beam 
        // is created, and results in a null family instance:
        //
        FamilyInstance fi = doc.Create.NewFamilyInstance( 
          line, s, null, stBeam );
        //
        // this creates a visible family instance, 
        // but the resulting beam has no location line
        // and behaves strangely, e.g. cannot be selected:
        //FamilyInstance fi = doc.Create.NewFamilyInstance( 
        //  p, s, q, null, nonStructural );

        //List<Element> levels = new List<Element>();
        //doc.get_Elements( typeof( Level ), levels );
        //Debug.Assert( 0 < levels.Count, 
        //  "expected at least one level in model" );
        //Level level = levels[0] as Level;
        //fi = doc.Create.NewFamilyInstance( 
        //  line, s, level, nonStructural );

        rc = CmdResult.Succeeded;
      }
      return rc;
    }
  }
}
