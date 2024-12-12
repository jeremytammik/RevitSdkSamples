#region Header
// Revit Structure API Labs
//
// Copyright (C) 2010-2013 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RvtOperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;
#endregion // Namespaces

namespace NewRstLab
{
  /// <summary>
  /// Utility methods from the standard Revit API class.
  /// </summary>
  static class RstUtils
  {
    public static string msRebarShapeName = "RstLabNewRebarShape";

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
      return string.Format( "({0},{1},{2})",
        RealString( p.X ), RealString( p.Y ), RealString( p.Z ) );
    }

    /// <summary>
    /// MessageBox wrapper for informational message.
    /// </summary>
    public static void InfoMsg( string msg )
    {
      MessageBox.Show( msg, "Revit Structure API Labs",
      MessageBoxButtons.OK, MessageBoxIcon.Information );
    }
    #endregion

    #region Get Element help functions.

    /// <summary>
    /// Return a list of instance of the specified class type.      
    /// </summary>
    public static IList<Element> GetElementOfClass( 
      Document doc, 
      Type classType )
    {
      return new FilteredElementCollector( doc )
        .OfClass( classType )
        .ToElements();
    }

    /// <summary>
    /// Helper to get all standard family instances for a given category
    /// using the filter features provided by the Revit 2009 API.
    /// </summary>
    public static IList<Element> GetAllStandardFamilyInstancesForACategory(
      Document doc,
      BuiltInCategory bic )
    {
      return new FilteredElementCollector( doc )
        .OfClass( typeof( FamilyInstance ) )
        .OfCategory( bic )
        .ToElements();
    }

    class SelectionFilterCatgory : ISelectionFilter
    {
      ElementId _id;

      public SelectionFilterCatgory( Category category )
      {
        _id = category.Id;
      }

      public bool AllowElement( Element e )
      {
        return null != e.Category
          && e.Category.Id.Equals( _id );
      }

      public bool AllowReference( Reference reference, XYZ position )
      {
        return true;
      }
    }

    /// <summary>
    /// Helper method to ask user to pick an element with the specified prompt string.
    /// If the selected element is the specified category, return it,
    /// Otherwise ask user to pick element again if user would like to pick.
    /// </summary>
    public static FamilyInstance GetFamilyInstanceBySelection(
      Document doc, 
      UIApplication uiApp,
      BuiltInCategory builtInCat, 
      string sPrompt )
    {
      FamilyInstance inst = null;
      Category cat = doc.Settings.Categories.get_Item( builtInCat );
      Selection sel = uiApp.ActiveUIDocument.Selection;
      while( null == inst )
      {
        sel.Elements.Clear();

        Reference eRef;

        try
        {
          eRef = sel.PickObject(
            ObjectType.Element,
            new SelectionFilterCatgory( cat ),
            sPrompt );
        }
        catch(  RvtOperationCanceledException )
        {
          break;
        }

        Element elem = null;
        if( null != eRef )
        {
          elem = doc.GetElement( eRef );
        }

        if( null != elem )
        {
          FamilyInstance instance = elem as FamilyInstance;
          if( null != instance )
          {
            if( instance.Category.Id.Equals( cat.Id ) )
            {
              inst = instance;
            }
          }
        }
        if( null == inst )
        {
          DialogResult dr = MessageBox.Show(
              "The selected element doesn't meet the need. Continue to select?",
              "Selected again", MessageBoxButtons.YesNo );

          if( dr == DialogResult.No )
          {
            return null;
          }
        }
      }
      return inst;
    }

    //Get Reference plane

    /// <summary>
    /// Helper funtion to ask user to pick an reference plane.
    /// If the selected element is a ReferencePlane, return it,
    /// Otherwise ask user to pick reference plane again if user would like to pick.
    /// </summary>
    public static ReferencePlane GetReferencePlaneBySelection(
      Document doc, 
      UIApplication uiApp, 
      string sPrompt )
    {
      ReferencePlane refPlane = null;

      Selection sel = uiApp.ActiveUIDocument.Selection;
      while( null == refPlane )
      {
        sel.Elements.Clear();
        Reference eRef = sel.PickObject( ObjectType.Element, sPrompt );

        Element elem = null;
        if( null != eRef )
        {
          elem = doc.GetElement( eRef );
        }

        if( null != elem )
        {
          ReferencePlane rp = elem as ReferencePlane;
          if( null != rp )
          {
            refPlane = rp;
          }
        }
        if( null == refPlane )
        {
          DialogResult dr = MessageBox.Show(
              "No reference plane was selected. Continue to select?",
              "Selected again", MessageBoxButtons.YesNo );
          if( dr == DialogResult.No )
          {
            return null;
          }
        }
      }

      return refPlane;
    }

    /// <summary>
    /// Helper funtion to get the specified RebarShape object and RebarBarType object.
    /// </summary>
    public static void GetRebarShape( string sRebarShape, string sRebar,
        out RebarShape barShape, out RebarBarType barType, Document doc )
    {
      barShape = null;
      barType = null;

      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( RebarBarType ) );
      IList<Element> barTypes = collector.ToElements();
      var bars = from element in barTypes
                 where element.Name.Equals( sRebar )
                 select element;
      if( bars.Count() > 0 )
      {
        barType = bars.First() as RebarBarType;
      }

      FilteredElementCollector collector1 = new FilteredElementCollector( doc );
      collector1.OfClass( typeof( RebarShape ) );
      IList<Element> shapes = collector1.ToElements();
      var shapeSet = from element in shapes
                     where element.Name.Equals( sRebarShape )
                     select element;
      if( shapeSet.Count() > 0 )
      {
        barShape = shapeSet.First() as RebarShape;
      }
      return;
    }
    #endregion

    #region Structural element description

    /// <summary>
    /// Helper function to form the message string that contains FamilyInstance's property values     
    /// </summary>
    public static string StructuralElementDescription( FamilyInstance e )
    {
      BuiltInCategory bic = BuiltInCategory.OST_StructuralFraming;
      Category cat = e.Document.Settings.Categories.get_Item( bic );
      bool hasCat = ( null != e.Category );
      bool hasUsage = hasCat && e.Category.Id.Equals( cat.Id );
      return e.Name
        + " Id=" + e.Id.IntegerValue.ToString()
        + ( hasCat ? ", Category=" + e.Category.Name : string.Empty )
        + ", Type=" + e.Symbol.Name
        + ( hasUsage ? ", Struct.Usage="
        + e.StructuralUsage.ToString() : string.Empty )
        + ", Struct.Type=" + e.StructuralType.ToString();
    }

    /// <summary>
    /// Helper function to form the property value of continuous footings 
    /// </summary>
    public static string StructuralElementDescription( ContFooting e )
    {
      return e.Name
        + " Id=" + e.Id.IntegerValue.ToString()
        + ", Category=" + e.Category.Name
        + ", Type=" + e.FootingType.Name;
    }

    /// <summary>
    /// Helper function to build a string of the property 
    /// values of structural floor. 
    /// </summary>
    public static string StructuralElementDescription( Floor e )
    {
      return e.Name
        + " Id=" + e.Id.IntegerValue.ToString()
        + ", Category=" + e.Category.Name
        + ", Type=" + e.FloorType.Name
        + ", Struct.Usage=" + e.StructuralUsage.ToString();
    }

    /// <summary>
    /// Helper function to build the string of the property values of structural wall
    /// </summary>
    public static string StructuralElementDescription( Wall e )
    {
      return e.Name
        + " Id=" + e.Id.IntegerValue.ToString()
        + ", Category=" + e.Category.Name
        + ", Type=" + e.WallType.Name
        + ", Struct.Usage=" + e.StructuralUsage.ToString()
        + ", Wall width=" + e.Width.ToString();
    }
    #endregion // Structural element description

    /// <summary>
    /// List the coordinates information in this curve.
    /// </summary>
    public static string ListCurve( ref Curve crv )
    {
      // todo: rewrite the newline handling, this is still VB:
      string s = null; ;
      if( crv is Line )
      {
        Line line = crv as Line;
        s += "  LINE: " + RstUtils.PointString( line.get_EndPoint( 0 ) )
          + " ; " + RstUtils.PointString( line.get_EndPoint( 1 ) ) + "\r\n";
      }
      else if( crv is Arc )
      {
        Arc arc = crv as Arc;
        s += "  ARC: " + RstUtils.PointString( arc.get_EndPoint( 0 ) )
          + " ; " + RstUtils.PointString( arc.get_EndPoint( 1 ) )
          + " ; R=" + RstUtils.RealString( arc.Radius ) + "\r\n";
      }
      else // generic parametric curve
      {
        if( crv.IsBound )
        {
          s += "  BOUND CURVE " + crv.GetType().Name + " - Tessellated result:\r\n";
          IList<XYZ> pts = crv.Tessellate();
          foreach( XYZ p in pts )
          {
            s += RstUtils.PointString( p ) + "\r\n";
          }
        }
        else
        {
          s += "  UNBOUND CURVE - unnexpected in an analytical model!\r\n";
        }
      }
      return s;
    }
  }


  #region Helpers for shared parameters
  class SharedParameterHelper
  {

    /// <summary>
    /// Helper to create a shared parameter
    /// </summary>
    public static ExternalDefinition GetOrCreateSharedParameter( Autodesk.Revit.ApplicationServices.Application app, string strParamName )
    {
      DefinitionFile defFile = GetSharedParamsFile( app );
      if( null == defFile )
      {
        RstUtils.InfoMsg( "Error in getting the Shared Params file" );
        return null;
      }

      DefinitionGroup defGroup = GetOrCreateSharedParamsGroup( defFile, "RST RebarShape" );
      if( null == defGroup )
      {
        RstUtils.InfoMsg( "Error in getting the Shared Params group" );
        return null;
      }
      ExternalDefinition definition = GetOrCreateSharedParamsDefinition(
          defGroup, ParameterType.Length, strParamName, true ) as ExternalDefinition;
      if( null == definition )
      {
        RstUtils.InfoMsg( "Error in creating the Shared Params" );
        return null;
      }
      return definition;

    }

    /// <summary>
    /// Helper to get shared parameters file.
    /// </summary>
    public static DefinitionFile GetSharedParamsFile( Autodesk.Revit.ApplicationServices.Application app )
    {

      // Get current shared params file name
      string sharedParamsFileName;
      try
      {
        sharedParamsFileName = app.SharedParametersFilename;
      }
      catch( Exception ex )
      {
        RstUtils.InfoMsg( "No shared params file set:" + ex.Message );
        return null;
      }
      if( 0 == sharedParamsFileName.Length )
      {
        string path = @"c:\temp\ShareParameter.txt";
        StreamWriter stream;
        stream = new StreamWriter( path );
        stream.Close();
        app.SharedParametersFilename = path;
        sharedParamsFileName = app.SharedParametersFilename;
      }
      // Get the current file object and return it
      DefinitionFile sharedParametersFile;
      try
      {
        sharedParametersFile = app.OpenSharedParameterFile();
      }
      catch( Exception ex )
      {
        RstUtils.InfoMsg( "Cannnot open shared params file:" + ex.Message );
        sharedParametersFile = null;
      }
      return sharedParametersFile;
    }

    /// <summary>
    /// Helper to get shared params group.
    /// </summary>
    public static DefinitionGroup GetOrCreateSharedParamsGroup(
     DefinitionFile sharedParametersFile,
     string groupName )
    {
      DefinitionGroup g = sharedParametersFile.Groups.get_Item( groupName );
      if( null == g )
      {
        try
        {
          g = sharedParametersFile.Groups.Create( groupName );
        }
        catch( Exception )
        {
          g = null;
        }
      }
      return g;
    }

    /// <summary>
    /// Helper to get shared params definition.
    /// </summary>
    public static Definition GetOrCreateSharedParamsDefinition(
      DefinitionGroup defGroup,
      ParameterType defType,
      string defName,
      bool visible )
    {
      Definition definition = defGroup.Definitions.get_Item( defName );
      if( null == definition )
      {
        try
        {
          definition = defGroup.Definitions.Create( defName, defType, visible );
        }
        catch( Exception )
        {
          definition = null;
        }
      }
      return definition;
    }

    /// <summary>
    /// Get GUID for a given shared param name.
    /// </summary>
    public static Guid SharedParamGUID( Autodesk.Revit.ApplicationServices.Application app, string defGroup, string defName )
    {
      Guid guid = Guid.Empty;
      try
      {
        DefinitionFile file = app.OpenSharedParameterFile();
        DefinitionGroup group = file.Groups.get_Item( defGroup );
        Definition definition = group.Definitions.get_Item( defName );
        ExternalDefinition externalDefinition = definition as ExternalDefinition;
        guid = externalDefinition.GUID;
      }
      catch( Exception )
      {
      }
      return guid;
    }
  }
  #endregion // Helpers for shared parameters
}
