#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2008 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using WinForms = System.Windows.Forms;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Enums;
using Autodesk.Revit.Elements;
using XYZ = Autodesk.Revit.Geometry.XYZ;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Utility;
#endregion // Namespaces

namespace RstLabs
{
  /// <summary>
  /// Utility methods from the standard Revit API class.
  /// </summary>
  static class RacUtils
  {
    #region Formatting and message handlers
    /// <summary>
    /// Format a real number and return its string representation.
    /// </summary>
    public static string RealString( double a )
    {
      return a.ToString( "0.##" );
    }

    /// <summary>
    /// Format a point or vector and return its string representation.
    /// </summary>
    public static string PointString( XYZ p )
    {
      return string.Format( "({0},{1},{2})", RealString( p.X ), RealString( p.Y ), RealString( p.Z ) );
    }

    /// <summary>
    /// MessageBox wrapper for informational message.
    /// </summary>
    public static void InfoMsg( string msg )
    {
      WinForms.MessageBox.Show( msg, "Revit Structure API Labs",
        WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information );
    }
    #endregion // Formatting and message handlers

    /// <summary>
    /// Helper to get all standard family instances for a given category 
    /// using the filter features provided by the Revit 2009 API.
    /// </summary>
    public static List<Element> GetAllStandardFamilyInstancesForACategory(
      Application app,
      BuiltInCategory bic )
    {
      List<Element> elements = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( FamilyInstance ) );
      Filter filterCategory = app.Create.Filter.NewCategoryFilter( bic );
      Filter filterAnd = app.Create.Filter.NewLogicAndFilter( filterType, filterCategory );
      app.ActiveDocument.get_Elements( filterAnd, elements );
      return elements;
    }

    /// <summary>
    /// Helper to return parameter value as string.
    /// One can also use param.AsValueString() to 
    /// get the user interface representation.
    /// </summary>
    public static string GetParameterValue( Parameter param )
    {
      string s;
      switch( param.StorageType )
      {
        case StorageType.Double:
          //
          // the internal database unit for all lengths is feet.
          // for instance, if a given room perimeter is returned as
          // 102.36 as a double and the display unit is millimeters, 
          // then the length will be displayed as
          // peri = 102.36220472440
          // peri * 12 *25.4
          // 31200 mm
          //
          //s = param.AsValueString(); // value seen by user, in display units
          s = RealString( param.AsDouble() ); // database value, internal units, e.g. feet
          break;
        case StorageType.Integer:
          s = param.AsInteger().ToString();
          break;
        case StorageType.String:
          s = param.AsString();
          break;
        case StorageType.ElementId:
          s = param.AsElementId().Value.ToString();
          break;
        case StorageType.None:
          s = "?NONE?";
          break;
        default:
          s = "?ELSE?";
          break;
      }
      return s;
    }

    /// <summary>
    /// Helper to return parameter value as string, with additional 
    /// support for element id to display the element type referred to.
    /// </summary>
    public static string GetParameterValue2( Parameter param, Document doc )
    {
      string s;
      if( StorageType.ElementId == param.StorageType && null != doc )
      {
        ElementId id = param.AsElementId();
        int i = id.Value;
        s = ( 0 <= i )
          ? string.Format( "{0}: {1}", i, doc.get_Element( ref id ).Name )
          : i.ToString();
      }
      else
      {
        s = GetParameterValue( param );
      }
      return s;
    }

    /// <summary>
    /// Return a specific element parameter mathcing the given display name.
    /// </summary>
    public static Parameter GetElemParam( Autodesk.Revit.Element elem, string name )
    {
      // The following commented code is for Revit 2008 and previous version. 
      // It works in Revit 2009 too. However, the Revit 2009 API provides an 
      // overloaded method to get the Parameter via the parameter name string.
      // It is much faster.
      //ParameterSet parameters = elem.Parameters;
      //foreach (Parameter parameter in parameters)
      //{
      //  if (parameter.Definition.Name == name)
      //  {
      //    return parameter;
      //  }
      //}
      //return null;

      // We use the new added overload method to retrieve the Parameter via the name string.
      // This only works from Revit 2009 onwards.
      Parameter para = elem.get_Parameter( name );
      return para;
    }
  }
}