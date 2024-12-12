#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2009 by Autodesk, Inc.
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
using System.Diagnostics;
using System.IO;
using WinForms = System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Enums;
using Geo = Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Symbols;
using XYZ = Autodesk.Revit.Geometry.XYZ;
#endregion // Namespaces

namespace Labs
{
  static class LabUtils
  {
    #region Formatting and message handlers
    public const string Caption = "Revit API Labs";

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
    /// Return a description string for a given element.
    /// </summary>
    public static string ElementDescription( Element e )
    {
      string description = ( null == e.Category )
        ? e.GetType().Name
        : e.Category.Name;
      if( null != e.Name )
      {
        description += " '" + e.Name + "'";
      }
      return description;
    }

    /// <summary>
    /// Return a description string including element id for a given element.
    /// </summary>
    public static string ElementDescription( Element e, bool includeId )
    {
      string description = ElementDescription( e );
      if( includeId )
      {
        description += " " + e.Id.Value.ToString();
      }
      return description;
    }

    /// <summary>
    /// MessageBox wrapper for informational message.
    /// </summary>
    public static void InfoMsg( string msg )
    {
      Debug.WriteLine( msg );
      WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information );
    }

    /// <summary>
    /// MessageBox wrapper for error message.
    /// </summary>
    public static void ErrorMsg( string msg )
    {
      Debug.WriteLine( msg );
      WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error );
    }

    /// <summary>
    /// MessageBox wrapper for question message.
    /// </summary>
    public static bool QuestionMsg( string msg )
    {
      Debug.WriteLine( msg );
      bool rc = WinForms.DialogResult.Yes
        == WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question );
      Debug.WriteLine( rc ? "Yes" : "No" );
      return rc;
    }

    /// <summary>
    /// MessageBox wrapper for question and cancel message.
    /// </summary>
    public static WinForms.DialogResult QuestionCancelMsg( string msg )
    {
      Debug.WriteLine( msg );
      WinForms.DialogResult rc = WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.YesNoCancel, WinForms.MessageBoxIcon.Question );
      Debug.WriteLine( rc.ToString() );
      return rc; 
    }
    #endregion // Formatting and message handlers

    #region Selection
    public static Element GetSingleSelectedElement( Document doc )
    {
      Element e = null;
      ElementSet ss = doc.Selection.Elements;
      if( 1 != ss.Size )
      {
        ErrorMsg( "Please pre-select a single element." );
      }
      else
      {
        ElementSetIterator iter = ss.ForwardIterator();
        iter.MoveNext();
        e = iter.Current as Element;
      }
      return e;
    }
    #endregion // Selection

    #region Geometry utilities
    /// <summary>
    /// Return the midpoint between two points.
    /// </summary>
    public static XYZ Midpoint( XYZ p, XYZ q )
    {
      return p + 0.5 * ( q - p );
    }
    #endregion // Geometry utilities

    #region Helpers to get specific element collections
    /*
    /// <summary>
    /// Helper to get all geometrical elements.
    /// </summary>
    public static ElementSet GetAllModelElements( Application app )
    {
      ElementSet elems = app.Create.NewElementSet(); // not new ElementSet()!
      //Geo.Options opt = null; // this causes an exception in get_Geometry()
      Geo.Options opt = app.Create.NewGeometryOptions(); // this is ok
      //opt.DetailLevel = Geo.Options.DetailLevels.Fine; // this is ok as well
      // IEnumerator iter = app.ActiveDocument.Elements // this would also be sufficient
      ElementIterator iter = app.ActiveDocument.Elements;
      while( iter.MoveNext() )
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        // This single line would probably work if all system families
        // were exposed as HostObjects, but they are not:
        //if( elem is FamilyInstance || elem is HostObject )
        if( !( elem is Symbol || elem is FamilyBase ) )
        {
          if( null != elem.Category )
          {
            if( null != elem.get_Geometry( opt ) )
            {
              elems.Insert( elem );
            }
          }
        }
      }
      return elems;
    }

    /// <summary>
    /// Helper to get all walls in Revit 2008.
    /// </summary>
    public static ElementSet GetAllWalls_2008( Application app )
    {
      ElementSet elems = app.Create.NewElementSet();
      ElementIterator iter = app.ActiveDocument.Elements; // IEnumerator
      while( iter.MoveNext() )
      {
        Element elem = iter.Current as Element;
        // for Wall (one of the host objects), there is a specific class:
        if( elem is Wall )
        {
          elems.Insert( elem );
        }
      }
      return elems;
    }

    /// <summary>
    /// Helper to get all walls using Revit 2009 filtering.
    /// </summary>
    public static List<Element> GetAllWalls( Application app )
    {
      List<Element> elements = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( Wall ) );
      app.ActiveDocument.get_Elements( filterType, elements );
      return elements;
    }*/

    /// <summary>
    /// Helper to get all standard family instances for a given category using Revit 2008 API.
    /// </summary>
    public static ElementSet GetAllStandardFamilyInstancesForACategoryName_2008(
      Application app,
      string catName )
    {
      ElementSet elems = app.Create.NewElementSet();
      ElementIterator iter = app.ActiveDocument.Elements;
      while( iter.MoveNext() )
      {
        Element elem = iter.Current as Element;
        // First check for the class, then for specific category name
        if( elem is FamilyInstance )
        {
          if( null != elem.Category && elem.Category.Name.Equals( catName ) )
          {
            elems.Insert( elem );
          }
        }
      }
      return elems;
    }

    /// <summary>
    /// Helper to get all standard family instances for a given category
    /// using the filter features provided by the Revit 2009 API.
    /// </summary>
    public static List<Element> GetAllStandardFamilyInstancesForACategoryName(
      Application app,
      string catName )
    {
      List<Element> elements = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( FamilyInstance ) );
      Category cat = app.ActiveDocument.Settings.Categories.get_Item( catName );
      Filter filterCategory = app.Create.Filter.NewCategoryFilter( cat );
      Filter filterAdd = app.Create.Filter.NewLogicAndFilter( filterType, filterCategory );
      app.ActiveDocument.get_Elements( filterAdd, elements );
      return elements;
    }

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
    /// Helper to get all model instances for a given category in Revit 2008.
    /// </summary>
    public static ElementSet GetAllModelInstancesForACategoryName_2008(
      Application app,
      string catName )
    {
      ElementSet elems = app.Create.NewElementSet();
      Geo.Options opt = app.Create.NewGeometryOptions();
      ElementIterator iter = app.ActiveDocument.Elements;
      while( iter.MoveNext() )
      {
        Autodesk.Revit.Element elem = iter.Current as Autodesk.Revit.Element;
        if( !( elem is Symbol || elem is FamilyBase ) )
        {
          if( null != elem.Category && elem.Category.Name.Equals( catName ) )
          {
            if( null != elem.get_Geometry( opt ) )
            {
              elems.Insert( elem );
            }
          }
        }
      }
      return elems;
    }

    /// <summary>
    /// Helper to get all model instances for a given built-in category in Revit 2009.
    /// </summary>
    public static List<Element> GetAllModelInstancesForACategory(
      Application app,
      BuiltInCategory bic )
    {
      List<Element> elements = new List<Element>();
      Filter filterCategory = app.Create.Filter.NewCategoryFilter( bic );
      app.ActiveDocument.get_Elements( filterCategory, elements );
      Geo.Options opt = app.Create.NewGeometryOptions();
      List<Element> elements2 = new List<Element>();
      foreach( Element e in elements )
      {
        if( !( e is Symbol )
          && !( e is FamilyBase )
          && ( null != e.get_Geometry( opt ) ) )
        {
          elements2.Add( e );
        }
      }
      return elements2;
    }

    /// <summary>
    /// Helper to get all instances for a given category in Revit 2009,
    /// identified either by a built-in category or by a category name.
    /// </summary>
    public static List<Element> GetTargetInstances(
      Application app,
      object targetCategory )
    {
      List<Element> elements;
      bool isName = targetCategory.GetType().Equals( typeof( string ) );
      if( isName )
      {
        Document doc = app.ActiveDocument;
        Category cat = doc.Settings.Categories.get_Item( targetCategory as string );
        Filter f = app.Create.Filter.NewCategoryFilter( cat );
        elements = new List<Element>();
        doc.get_Elements( f, elements );
      }
      else
      {
        elements = GetAllModelInstancesForACategory( 
          app, (BuiltInCategory) targetCategory );
      }
      return elements;
    }

    #region 2008 Version
    /// <summary>
    /// Helper to get specified Type for specified Family as FamilySymbol object
    /// (in theory, we should also check for the correct *Category Name*).
    /// </summary>
    public static FamilySymbol GetFamilySymbol_2008(
      Document doc,
      string familyName,
      string typeName )
    {
      ElementIterator iter = doc.Elements;
      while( iter.MoveNext() )
      {
        Element elem = iter.Current as Element;
        // We got a Family
        if( elem is Family )
        {
          Family fam = elem as Family;
          // If we have a match on family name, loop all its types for the other match
          if( fam.Name.Equals( familyName ) )
          {
            foreach( FamilySymbol sym in fam.Symbols )
            {
              if( sym.Name.Equals( typeName ) )
              {
                return sym;
              }
            }
          }
        }
      }
      // if here - haven't got it!
      return null;
    }
    #endregion // 2008 Version

    /// <summary>
    /// Helper to get specified type for specified family as FamilySymbol object
    /// (in theory, we should also check for the correct *Category Name*).
    /// (note: this only works with component family.)
    /// </summary>
    public static FamilySymbol GetFamilySymbol(
      Application app,
      string familyName,
      string typeName )
    {
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( FamilySymbol ) );
      Filter filterFamilyName = app.Create.Filter.NewFamilyFilter( familyName );
      Filter filter = app.Create.Filter.NewLogicAndFilter( filterType, filterFamilyName );
      List<Element> elementList = new List<Element>();
      int n = app.ActiveDocument.get_Elements( filter, elementList );
      // we have a list of symbols for a given family.
      // loop through the list and find a match
      // todo: can this loop be repalced by a third filter,
      // for instance using a specified parameter value 
      // to check for the given family name?
      foreach( Element e in elementList )
      {
        if( e.Name.Equals( typeName ) )
        {
          return e as FamilySymbol;
        }
      }
      return null;
    }

    /// <summary>
    /// Return all types of the requested class in the active document matching the given built-in category.
    /// </summary>
    public static List<Element> GetAllTypes( Application app, Type type, BuiltInCategory bic )
    {
      List<Element> familySymbols = new List<Element>();
      Filter filterCategory = app.Create.Filter.NewCategoryFilter( bic );
      Filter filterType = app.Create.Filter.NewTypeFilter( type );
      Filter filterAnd = app.Create.Filter.NewLogicAndFilter( filterCategory, filterType );
      app.ActiveDocument.get_Elements( filterAnd, familySymbols );
      return familySymbols;
    }

    /// <summary>
    /// Return all family symbols in the active document matching the given built-in category.
    /// </summary>
    public static List<Element> GetAllFamilySymbols( Application app, BuiltInCategory bic )
    {
      return GetAllTypes( app, typeof( FamilySymbol ), bic );
    }

    /// <summary>
    /// Determine bottom and top levels for creating walls.
    /// In a default empty Revit Architecture 2009 project,
    /// 'Level 1' and 'Level 2' will be returned.
    /// </summary>
    /// <returns>True is the two levels are successfully determined.</returns>
    public static bool GetBottomAndTopLevels( 
      Application app, 
      ref Level levelBottom, 
      ref Level levelTop )
    {
      Document doc = app.ActiveDocument;
      Autodesk.Revit.Creation.Application creApp = app.Create;
      List<Element> levels = new List<Element>();
      Filter filterType = creApp.Filter.NewTypeFilter( typeof( Level ) );
      doc.get_Elements( filterType, levels );
      foreach( Element e in levels )
      {
        if( null == levelBottom )
        {
          levelBottom = e as Level;
        }
        else if( null == levelTop )
        {
          levelTop = e as Level;
        }
        else
        {
          break;
        }
      }
      if( levelTop.Elevation < levelBottom.Elevation )
      {
        Level tmp = levelTop;
        levelTop = levelBottom;
        levelBottom = tmp;
      }
      return null != levelBottom && null != levelTop;
    }

    /// <summary>
    /// Return the one and only project information element in Revit 2008.
    /// </summary>
    public static Element GetProjectInfoElem_2008( Document doc )
    {
      // "Project Information" category
      Category catProjInfo = doc.Settings.Categories.get_Item( BuiltInCategory.OST_ProjectInformation );
      ElementId id = catProjInfo.Id;
      //  Loop all elements
      Element elem;
      ElementIterator elemIter = doc.Elements;
      while( elemIter.MoveNext() )
      {
        elem = elemIter.Current as Element;
        //  Return the first match (it's a singleton!)
        if( null != elem.Category && elem.Category.Id.Equals( id ) )
        {
          return elem;
        }
      }
      // if here - couldn't find it!?
      return null;
    }

    /// <summary>
    /// Return the one and only project information element using Revit 2009 filtering
    /// by searching for the "Project Information" category. Only one such element exists.
    /// </summary>
    public static Element GetProjectInfoElem( Application app )
    {
      Filter filterCategory = app.Create.Filter.NewCategoryFilter( 
        BuiltInCategory.OST_ProjectInformation );
      ElementIterator i = app.ActiveDocument.get_Elements( filterCategory );
      i.MoveNext();
      Element e = i.Current as Element;
      Debug.Assert( null != e, "expected valid project information element" );
      Debug.Assert( !i.MoveNext(), "expected one single element to be returned" );
      return e;
    }

    /// <summary>
    /// Helper to get all instance elements with an IMPORT_INSTANCE_SCALE parameter.
    /// </summary>
    public static List<Element> GetAllImportScaleInstances_Crash( Application app )
    {
      List<Element> elements = new List<Element>();
      try
      {
        Filter fType = app.Create.Filter.NewTypeFilter( typeof( Instance ) );
        Filter fParam = app.Create.Filter.NewParameterFilter(
          BuiltInParameter.IMPORT_INSTANCE_SCALE,
          CriteriaFilterType.HasParam,
          0 );
        Filter f = app.Create.Filter.NewLogicAndFilter( fType, fParam );
        app.ActiveDocument.get_Elements( f, elements );
      }
      catch( Exception ex )
      {
        Debug.WriteLine( ex.Message );
      }
      return elements;
    }

    public static List<Element> GetAllImportScaleInstances( Application app )
    {
      List<Element> elements = new List<Element>();
      Filter fType = app.Create.Filter.NewTypeFilter( typeof( Instance ) );
      Filter fParam = app.Create.Filter.NewParameterFilter(
        BuiltInParameter.IMPORT_INSTANCE_SCALE, 
        CriteriaFilterType.GreaterThanOrEqual,
        0.0 );
      Filter f = app.Create.Filter.NewLogicAndFilter( fType, fParam );
      app.ActiveDocument.get_Elements( f, elements );
      return elements;
    }

    public static List<Element> GetFamiliesOfCategory(
      Application app, 
      BuiltInCategory bic )
    {
      Document doc = app.ActiveDocument;
      List<Element> famsInCat = new List<Element>();

#if MARTIN
      // why doesn't this work?
      // it does not work, because the Family class has not implemented the Category property.
      // you have to use other methods to check the category of a family. dor some family instances, you can use FamilyCategory instead of Category, but that does not always work either. the most reliable method is to check the Category property on any one (such as the first) symbol contained in the family.
      Autodesk.Revit.Creation.Filter cf 
        = app.Create.Filter;

      TypeFilter typeFilter 
        = cf.NewTypeFilter( typeof( Family ) );

      CategoryFilter catFilter 
        = cf.NewCategoryFilter( bic );

      LogicAndFilter andFilter 
        = cf.NewLogicAndFilter( typeFilter, catFilter );

      doc.get_Elements( andFilter, famsInCat );
      return famsInCat;
#endif // MARTIN

#if MARTIN
      Category categoryMechanicalEquipment 
        = doc.Settings.Categories.get_Item( bic );

      ElementId categoryId 
        = categoryMechanicalEquipment.Id;

      ElementIterator it 
        = doc.get_Elements( typeof( Family ) );

      while( it.MoveNext() )
      {
        Family family = it.Current as Family;
        bool categoryMatches 
          = family.FamilyCategory.Id.Equals( categoryId );

        if( categoryMatches )
        {
          famsInCat.Add( family );
        }
      }
      return famsInCat;
#endif // MARTIN

      Category categoryMechanicalEquipment 
        = doc.Settings.Categories.get_Item( bic );

      ElementId categoryId 
        = categoryMechanicalEquipment.Id;

      ElementIterator it 
        = doc.get_Elements( typeof( Family ) );

      while( it.MoveNext() )
      {
        Family family = it.Current as Family;
        foreach( Symbol s in family.Symbols )
        {
          if( s.Category.Id.Equals( categoryId ) )
          {
            famsInCat.Add( family );
          }
          break; // only need to look at first symbol
        }
      }
      return famsInCat;
    }
    #endregion // Helpers to get specific element collections

    #region Helpers for parameters
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
          //s = param.AsDouble().ToString(); // if not using not using LabUtils.RealString()
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
    /// Helper to return either standard parameter string, 
    /// or element name in case of an element id.
    /// </summary>
    public static string GetParameterValueString2( Parameter param, Document doc )
    {
      string s;
      if( StorageType.ElementId == param.StorageType && null != doc )
      {
        ElementId id = param.AsElementId();
        int i = id.Value;
        s = ( 0 <= i )
          ? ElementDescription( doc.get_Element( ref id ) )
          : string.Empty; // i.ToString();
      }
      else
      {
        s = param.AsValueString();
      }
      return s;
    }

    /// <summary>
    /// Helper to get *specific* parameter by name.
    /// No longer required in 2009, because the element provides
    /// direct look-up access by name as well in Revit 2009.
    /// </summary>
    public static Parameter GetElemParam_2008( Element elem, string name )
    {
      ParameterSet parameters = elem.Parameters;
      foreach( Parameter parameter in parameters )
      {
        if( parameter.Definition.Name == name )
        {
          return parameter;
        }
      }
      return null;
    }
    #endregion // Helpers for parameters

    #region Helpers for shared parameters
    /// <summary>
    /// Helper to get shared parameters file.
    /// </summary>
    public static DefinitionFile GetSharedParamsFile( Application app )
    {
      // Get current shared params file name
      string sharedParamsFileName;
      try
      {
        sharedParamsFileName = app.Options.SharedParametersFilename;
      }
      catch( Exception ex )
      {
        ErrorMsg( "No shared params file set:" + ex.Message );
        return null;
      }
      if( 0 == sharedParamsFileName.Length )
      {
        string path = LabConstants.SharedParamFilePath;
        StreamWriter stream;
        stream = new StreamWriter( path );
        stream.Close();
        app.Options.SharedParametersFilename = path;
        sharedParamsFileName = app.Options.SharedParametersFilename;
      }
      // Get the current file object and return it
      DefinitionFile sharedParametersFile;
      try
      {
        sharedParametersFile = app.OpenSharedParameterFile();
      }
      catch( Exception ex )
      {
        ErrorMsg( "Cannnot open shared params file:" + ex.Message );
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
    /// <param name="app">Revit application</param>
    /// <param name="defGroup">Definition group name</param>
    /// <param name="defName">Definition name</param>
    /// <returns>GUID</returns>
    public static Guid SharedParamGUID( Application app, string defGroup, string defName )
    {
      Guid guid = Guid.Empty;
      try
      {
        Autodesk.Revit.Parameters.DefinitionFile file = app.OpenSharedParameterFile();
        Autodesk.Revit.Parameters.DefinitionGroup group = file.Groups.get_Item( defGroup );
        Autodesk.Revit.Parameters.Definition definition = group.Definitions.get_Item( defName );
        Autodesk.Revit.Parameters.ExternalDefinition externalDefinition = definition as ExternalDefinition;
        guid = externalDefinition.GUID;
      }
      catch( Exception )
      {
      }
      return guid;
    }
    #endregion // Helpers for shared parameters

    #region Helpers for groups
    /// <summary>
    /// Helper to get all groups in Revit 2008.
    /// </summary>
    public static ElementSet GetAllGroups_2008( Application app )
    {
      ElementSet elems = app.Create.NewElementSet();
      ElementIterator iter = app.ActiveDocument.Elements;
      while( iter.MoveNext() )
      {
        Element elem = iter.Current as Element;
        if( elem is Group )
        {
          elems.Insert( elem );
        }
      }
      return elems;
    }

    /// <summary>
    /// Helper to get all rooms using Revit 2009 filtering.
    /// </summary>
    public static List<Element> GetAllRooms(Application app)
    {
        List<Element> elements = new List<Element>();
        Filter filterType = app.Create.Filter.NewTypeFilter(typeof(Room));
        app.ActiveDocument.get_Elements(filterType, elements);
        return elements;
    }

    /// <summary>
    /// Helper to get all groups using Revit 2009 filtering.
    /// </summary>
    public static List<Element> GetAllGroups( Application app )
    {
      List<Element> elements = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( Group ) );
      app.ActiveDocument.get_Elements( filterType, elements );
      return elements;
    }

    /// <summary>
    /// Helper to get all group types in Revit 2008.
    /// </summary>
    public static ElementSet GetAllGroupTypes_2008( Application app )
    {
      ElementSet elems = app.Create.NewElementSet();
      ElementIterator iter = app.ActiveDocument.Elements;
      while( iter.MoveNext() )
      {
        Element elem = iter.Current as Element;
        if( elem is GroupType )
        {
          elems.Insert( elem );
        }
      }
      return elems;
    }

    /// <summary>
    /// Helper to get all group types using Revit 2009 filtering.
    /// </summary>
    public static List<Element> GetAllGroupTypes( Application app )
    {
      List<Element> elements = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( GroupType ) );
      app.ActiveDocument.get_Elements( filterType, elements );
      return elements;
    }

    /// <summary>
    /// Helper to get all *model* group types in Revit 2008.
    /// </summary>
    public static ElementSet GetAllModelGroupTypes_2008( Application app )
    {
      ElementSet elems = app.Create.NewElementSet();
      ElementIterator iter = app.ActiveDocument.Elements;
      while( iter.MoveNext() )
      {
        Element elem = iter.Current as Element;
        if( elem is GroupType )
        {
          try
          {
            string s = elem.get_Parameter(
              BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM ).AsString();
            if( s.Equals( LabConstants.GroupTypeModel ) )
            {
              elems.Insert( elem );
            }
          }
          catch( Exception )
          {
          }
        }
      }
      return elems;
    }

    /// <summary>
    /// Helper to get all *model* group types using Revit 2009 filtering.
    /// </summary>
    public static List<Element> GetAllModelGroupTypes( Application app )
    {
      List<Element> elements = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( GroupType ) );
      Filter filterParam = app.Create.Filter.NewParameterFilter(
        BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM, CriteriaFilterType.Equal,
        LabConstants.GroupTypeModel );
      Filter filterAnd = app.Create.Filter.NewLogicAndFilter( filterType, filterParam );
      app.ActiveDocument.get_Elements( filterAnd, elements );
      return elements;
    }
    #endregion // Helpers for groups
  }
}
