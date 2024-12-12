#region Header
//
// CmdLinq.cs - test linq.
//
// Copyright (C) 2009-2010 by Joel Karr and Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using CreationFilter
  = Autodesk.Revit.Creation.Filter;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdLinq : IExternalCommand
  {
    class InstanceData
    {
    #region Properties
    // auto-implemented properties, cf. 
    // http://msdn.microsoft.com/en-us/library/bb384054.aspx

    public Element Instance { get; set; }
    public String Param1 { get; set; }
    public bool Param2 { get; set; }
    public int Param3 { get; set; }
    #endregion

    #region Constructors
    public InstanceData( Element instance )
    {
      Instance = instance;

      ParameterMap m = Instance.ParametersMap;

      Parameter p = m.get_Item( "Param1" );
      Param1 = ( p == null ) ? string.Empty : p.AsString();

      p = m.get_Item( "Param2" );
      Param2 = ( p == null ) ? false : ( 0 != p.AsInteger() );

      p = m.get_Item( "Param3" );
      Param3 = ( p == null ) ? 0 : p.AsInteger();
    }
    #endregion
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      CreationFilter cf = app.Create.Filter;
      Filter f1 = cf.NewTypeFilter( typeof( FamilyInstance ) );

      List<Element> a = new List<Element>();
      doc.get_Elements( f1, a );

      List<InstanceData> instanceDataList
        = new List<InstanceData>();

      foreach( Element e in a )
      {
        instanceDataList.Add(
          new InstanceData( e ) );
      }

      string s = "value1";
      bool b = true;
      int i = 42;

      var found = from instance in instanceDataList where
        (instance.Param1.Equals( s )
        && b == instance.Param2
        && i < instance.Param3)
      select instance;

      foreach( InstanceData instance in found )
      {
        // Do whatever you would like
      }

      return CmdResult.Failed;
    }
  }
}
