#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2013 by Autodesk, Inc.
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Application = Autodesk.Revit.ApplicationServices.Application;
#endregion // Namespaces

namespace XtraCs
{
  /// <summary>
  /// A collection of utility methods reused in several labs.
  /// </summary>
  static class LabUtils
  {
    #region Formatting and message handlers
    public const string Caption = "Revit API Labs";

    /// <summary>
    /// Return an English plural suffix 's' or
    /// nothing for the given number of items.
    /// </summary>
    public static string PluralSuffix( int n )
    {
      return 1 == n ? "" : "s";
    }

    /// <summary>
    /// Return a dot for zero items, or a colon for more.
    /// </summary>
    public static string DotOrColon( int n )
    {
      return 0 < n ? ":" : ".";
    }

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
    /// Return a description string for a given element.
    /// </summary>
    public static string ElementDescription( Element e )
    {
      string description = ( null == e.Category )
        ? e.GetType().Name
        : e.Category.Name;

      FamilyInstance fi = e as FamilyInstance;

      if( null != fi )
      {
        description += " '" + fi.Symbol.Family.Name + "'";
      }

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
        description += " " + e.Id.IntegerValue.ToString();
      }
      return description;
    }

    /// <summary>
    /// Revit TaskDialog wrapper for a short informational message.
    /// </summary>
    public static void InfoMsg( string msg )
    {
      Debug.WriteLine( msg );

      //WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information );

      TaskDialog.Show( Caption, msg, TaskDialogCommonButtons.Ok );
    }

    /// <summary>
    /// Revit TaskDialog wrapper for a message
    /// with separate main instruction and content.
    /// </summary>
    public static void InfoMsg( string msg, string content )
    {
      Debug.WriteLine( msg );
      Debug.WriteLine( content );
      TaskDialog d = new TaskDialog( Caption );
      d.MainInstruction = msg;
      d.MainContent = content;
      d.Show();
    }

    /// <summary>
    /// Revit TaskDialog wrapper for a message with separate
    /// main instruction and list of lines of content.
    /// The main instruction is expected to include count,
    /// plural suffix and dot or end placeholders.
    /// </summary>
    public static void InfoMsg( string msg, List<string> content )
    {
      int n = content.Count;

      InfoMsg( string.Format( msg,
        n, PluralSuffix( n ), DotOrColon( n ) ),
        string.Join( "\n", content.ToArray() ) );
    }

    /// <summary>
    /// MessageBox wrapper for error message.
    /// </summary>
    public static void ErrorMsg( string msg )
    {
      Debug.WriteLine( msg );

      //WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error );

      TaskDialog d = new TaskDialog( Caption );
      d.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
      d.MainInstruction = msg;
      d.Show();
    }

    /// <summary>
    /// MessageBox wrapper for question message.
    /// </summary>
    public static bool QuestionMsg( string msg )
    {
      Debug.WriteLine( msg );

      //bool rc = WinForms.DialogResult.Yes
      //  == WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question );
      //Debug.WriteLine( rc ? "Yes" : "No" );
      //return rc;

      TaskDialog d = new TaskDialog( Caption );
      d.MainIcon = TaskDialogIcon.TaskDialogIconNone;
      d.MainInstruction = msg;
      d.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
      d.DefaultButton = TaskDialogResult.Yes;
      return TaskDialogResult.Yes == d.Show();
    }

    /// <summary>
    /// MessageBox wrapper for question and cancel message.
    /// </summary>
    public static TaskDialogResult QuestionCancelMsg( string msg )
    {
      Debug.WriteLine( msg );

      //WinForms.DialogResult rc = WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.YesNoCancel, WinForms.MessageBoxIcon.Question );
      //Debug.WriteLine( rc.ToString() );
      //return rc;

      TaskDialog d = new TaskDialog( Caption );
      d.MainIcon = TaskDialogIcon.TaskDialogIconNone;
      d.MainInstruction = msg;
      d.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No | TaskDialogCommonButtons.Cancel;
      d.DefaultButton = TaskDialogResult.Yes;
      return d.Show();
    }
    #endregion // Formatting and message handlers

    #region Geometry utilities
    /// <summary>
    /// Return the midpoint between two points.
    /// </summary>
    public static XYZ Midpoint( XYZ p, XYZ q )
    {
      return p + 0.5 * ( q - p );
    }
    #endregion // Geometry utilities

    #region Selection
    public static Element
      GetSingleSelectedElementOrPrompt(
        UIDocument uidoc )
    {
      Element e = null;
      ElementSet ss = uidoc.Selection.Elements;
      if( 1 == ss.Size )
      {
        ElementSetIterator iter = ss.ForwardIterator();
        iter.MoveNext();
        e = iter.Current as Element;
      }
      else
      {
        string sid;
        DialogResult result = DialogResult.OK;
        while( null == e && DialogResult.OK == result )
        {
          using( ElementIdForm form
            = new ElementIdForm() )
          {
            result = form.ShowDialog();
            sid = form.ElementId;
          }
          if( DialogResult.OK == result )
          {
            if( 0 == sid.Length )
            {
              try
              {
                Reference r = uidoc.Selection.PickObject(
                  ObjectType.Element,
                  "Please pick an element" );

                //e = r.Element; // 2011
                e = uidoc.Document.GetElement( r ); // 2012
              }
              catch( OperationCanceledException )
              {
              }
            }
            else
            {
              ElementId id = new ElementId(
                int.Parse( ( sid ) ) );

              e = uidoc.Document.GetElement( id );
              if( null == e )
              {
                ErrorMsg( string.Format(
                  "Invalid element id '{0}'.",
                  sid ) );
              }
            }
          }
        }
      }
      return e;
    }

    /// <summary>
    /// A selection filter for a specific System.Type.
    /// </summary>
    class TypeSelectionFilter : ISelectionFilter
    {
      Type _type;

      public TypeSelectionFilter( Type type )
      {
        _type = type;
      }

      /// <summary>
      /// Allow an element of the specified System.Type to be selected.
      /// </summary>
      /// <param name="element">A candidate element in selection operation.</param>
      /// <returns>Return true for specified System.Type, false for all other elements.</returns>
      public bool AllowElement( Element e )
      {
        //return null != e.Category
        // && e.Category.Id.IntegerValue == ( int ) _bic;

        return e.GetType().Equals( _type );
      }

      /// <summary>
      /// Allow all the reference to be selected
      /// </summary>
      /// <param name="refer">A candidate reference in selection operation.</param>
      /// <param name="point">The 3D position of the mouse on the candidate reference.</param>
      /// <returns>Return true to allow the user to select this candidate reference.</returns>
      public bool AllowReference( Reference r, XYZ p )
      {
        return true;
      }
    }

    public static Element GetSingleSelectedElementOrPrompt(
      UIDocument uidoc,
      Type type )
    {
      Element e = null;

      ElementSet ss = uidoc.Selection.Elements;

      if( 1 == ss.Size )
      {
        ElementSetIterator iter = ss.ForwardIterator();
        iter.MoveNext();
        Type t = iter.Current.GetType();
        if( t.Equals( type ) || t.IsSubclassOf( type ) )
        {
          e = iter.Current as Element;
        }
      }
      if( null == e )
      {
        try
        {
          Reference r = uidoc.Selection.PickObject(
            ObjectType.Element,
            new TypeSelectionFilter( type ),
            string.Format( "Please pick a {0} element", type.Name ) );

          //e = r.Element; // 2011
          e = uidoc.Document.GetElement( r ); // 2012
        }
        catch( OperationCanceledException )
        {
        }
      }
      return e;
    }
    #endregion // Selection

    #region Retrieve specific element collections
    #region GetElementsOfType:
    /// <summary>
    /// Return all elements of the requested class,
    /// i.e. System.Type, matching the given built-in
    /// category in the given document.
    /// </summary>
    public static FilteredElementCollector
      GetElementsOfType(
        Document doc,
        Type type,
        BuiltInCategory bic )
    {
      FilteredElementCollector collector
        = new FilteredElementCollector( doc );

      collector.OfCategory( bic );
      collector.OfClass( type );

      return collector;
    }
    #endregion // GetElementsOfType

    #region GetFamilyInstances:
    /// <summary>
    /// Retrieve all standard family instances for a given category.
    /// Todo: Compare this with the FamilyInstanceFilter class.
    /// </summary>
    public static FilteredElementCollector GetFamilyInstances(
      Document doc,
      BuiltInCategory bic )
    {
      /* 2009:
      List<Element> elements = new List<Element>();
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( FamilyInstance ) );
      Filter filterCategory = app.Create.Filter.NewCategoryFilter( bic );
      Filter filterAnd = app.Create.Filter.NewLogicAndFilter( filterType, filterCategory );
      app.ActiveDocument.get_Elements( filterAnd, elements );
      return elements;
      */

      return GetElementsOfType( doc, typeof( FamilyInstance ), bic );
    }
    #endregion // GetFamilyInstances

    /// <summary>
    /// Return all family symbols in the given document
    /// matching the given built-in category.
    /// Todo: Compare this with the FamilySymbolFilter class.
    /// </summary>
    public static FilteredElementCollector
      GetFamilySymbols(
        Document doc,
        BuiltInCategory bic )
    {
      return GetElementsOfType( doc,
        typeof( FamilySymbol ), bic );
    }

    /// <summary>
    /// Return the first family symbol found in the given document
    /// matching the given built-in category or null if none is found.
    /// </summary>
    public static FamilySymbol GetFirstFamilySymbol(
      Document doc,
      BuiltInCategory bic )
    {
      FamilySymbol s = GetFamilySymbols( doc, bic ).FirstElement() as FamilySymbol;

      Debug.Assert( null != s, string.Format(
        "expected at least one {0} symbol in project",
        bic.ToString() ) );

      return s;
    }

    /// <summary>
    /// Return the category of a family by asking its first symbol.
    /// You can determine the family category from any of its symbols.
    /// </summary>
    static Category FamilyCategory( Family f )
    {
      Category c = null;
      foreach( FamilySymbol s in f.Symbols )
      {
        c = s.Category;
        break;
      }
      return c;
    }

    /// <summary>
    /// Return all families matching the given built-in category
    /// in the given document. Normally, one would simply use
    ///
    /// GetElementsOfType( doc, typeof( FamilyInstance ), bic );
    ///
    /// Unfortunately, this does not work, because the Category
    /// property of families os often unimplemented, cf.
    /// http://thebuildingcoder.typepad.com/blog/2009/01/family-category-and-filtering.html
    /// and Clarify case 1258171 [Category filter problems].
    /// </summary>
    public static IEnumerable<Family> GetFamilies(
      Document doc,
      BuiltInCategory bic )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      //collector.OfCategory( bic ); // this does not work
      collector.OfClass( typeof( Family ) );

      Category cat;

      IEnumerable<Element> familiesOfCategory =
        from f in collector
        where ( null != ( cat = FamilyCategory( f as Family ) )
          && cat.Id.IntegerValue.Equals( (int) bic ) )
        select f;

      return familiesOfCategory.Cast<Family>();
    }

    /// <summary>
    /// Return all families whose name matches the given family name.
    /// </summary>
    public static IEnumerable<Family> GetFamilies(
      Document doc,
      string family_name )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( Family ) );

#if USE_LINQ
      //
      // using LINQ:
      //
      IEnumerable<Element> familiesByName =
        from f in collector
        where f.Name.Equals( family_name )
        select f;

      return familiesByName.Cast<Family>();
#endif // USE_LINQ

      //
      // using an anonymous method to define a named method:
      //
      //Func<Element, bool> nameEquals = e => e.Name.Equals( family_name );
      //return collector.Where<Element>( nameEquals ).Cast<Family>();

      //
      // using an anonymous method:
      //
      return collector
        .Where<Element>( e => e.Name.Equals( family_name ) )
        .Cast<Family>();
    }

    /// <summary>
    /// Return all families whose name either matches or contains the given family name.
    /// </summary>
    /// <param name="doc">Revit document.</param>
    /// <param name="family_name">Family name or substring to search for.</param>
    /// <param name="search_for_substring">Search for substring or entire family name?</param>
    /// <returns>Collection of families matching the search criteria.</returns>
    public static IEnumerable<Family> GetFamilies(
      Document doc,
      string family_name,
      bool search_for_substring )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( Family ) );

#if USE_LINQ
      //
      // using LINQ:
      //
      IEnumerable<Element> familiesByName =
        from f in collector
        where ( search_for_substring ? f.Name.Contains( family_name ) : f.Name.Equals( family_name ) )
        select f;

      return familiesByName.Cast<Family>();
#endif // USE_LINQ

      //
      // using an anonymous method to define a named method:
      //
      //Func<Element, bool> nameMatches = e
      //  => ( search_for_substring ? e.Name.Contains( family_name ) : e.Name.Equals( family_name ) );
      //return collector.Where<Element>( nameMatches ).Cast<Family>();

      //
      // using an anonymous method:
      //
      return collector
        .Where<Element>( e =>
          ( search_for_substring ? e.Name.Contains( family_name ) : e.Name.Equals( family_name ) ) )
        .Cast<Family>();
    }

    static public void MyTest( Document doc )
    {
      string familyName = "Single-Flush";

      // get the family we want
      FilteredElementCollector fec = new FilteredElementCollector( doc );
      fec.OfClass( typeof( Family ) );

      Func<Element, bool> nameEquals = e => e.Name.Equals( familyName );

      Family f = fec.First<Element>( nameEquals ) as Family;

      // get the symbols of that family
      FamilySymbolFilter fsf = new FamilySymbolFilter( f.Id );
      fec = new FilteredElementCollector( doc );
      fec.WherePasses( fsf );

      // list them

      Func<Element, string> getName = e => e.Name;

      string str = string.Join( "\n",
        fec.Select<Element, string>( getName ).ToArray<string>() );

      System.Windows.Forms.MessageBox.Show( str.ToString(), "FamilySymbols of " + familyName );
    }

    /// <summary>
    /// Determine bottom and top levels for creating walls.
    /// In a default empty Revit Architecture project,
    /// 'Level 1' and 'Level 2' will be returned.
    /// </summary>
    /// <returns>True is the two levels are successfully determined.</returns>
    public static bool GetBottomAndTopLevels(
      Document doc,
      ref Level levelBottom,
      ref Level levelTop )
    {
      FilteredElementCollector levels = GetElementsOfType(
        doc, typeof( Level ), BuiltInCategory.OST_Levels );

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
    /// Helper to get all instances for a given category,
    /// identified either by a built-in category or by a category name.
    /// </summary>
    public static List<Element> GetTargetInstances(
      Document doc,
      object targetCategory )
    {
      List<Element> elements;

      bool isName = targetCategory.GetType().Equals( typeof( string ) );

      if( isName )
      {
        Category cat = doc.Settings.Categories.get_Item( targetCategory as string );
        FilteredElementCollector collector = new FilteredElementCollector( doc );
        collector.OfCategoryId( cat.Id );
        elements = new List<Element>( collector );
      }
      else
      {
        FilteredElementCollector collector
          = new FilteredElementCollector( doc )
            .WhereElementIsNotElementType();

        collector.OfCategory( (BuiltInCategory) targetCategory );

        var model_elements = from e in collector
                             where ( null != e.Category && e.Category.HasMaterialQuantities )
                             select e;

        elements = model_elements.ToList<Element>();
      }
      return elements;
    }

    /// <summary>
    /// Return the one and only project information element
    /// by searching for the "Project Information" category.
    /// Only one such element exists.
    /// </summary>
    public static Element GetProjectInfoElem( Document doc )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfCategory( BuiltInCategory.OST_ProjectInformation );
      return collector.FirstElement();
    }
    #endregion // Retrieve specific element collections

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
          // peri * 12 * 25.4
          // 31200 mm
          //
          //s = param.AsValueString(); // value seen by user, in display units
          //s = param.AsDouble().ToString(); // if not using not using LabUtils.RealString()
          s = RealString( param.AsDouble() ); // raw database value in internal units, e.g. feet
          break;

        case StorageType.Integer:
          s = param.AsInteger().ToString();
          break;

        case StorageType.String:
          s = param.AsString();
          break;

        case StorageType.ElementId:
          s = param.AsElementId().IntegerValue.ToString();
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

    static int _min_bic = 0;
    static int _max_bic = 0;

    static void SetMinAndMaxBuiltInCategory()
    {
      Array a = Enum.GetValues( typeof( BuiltInCategory ) );
      _max_bic = a.Cast<int>().Max();
      _min_bic = a.Cast<int>().Min();
    }

    static string BuiltInCategoryString( int i )
    {
      if( 0 == _min_bic )
      {
        SetMinAndMaxBuiltInCategory();
      }
      return ( _min_bic < i && i < _max_bic )
        ? " " + ( (BuiltInCategory) i ).ToString()
        : string.Empty;
    }

    /// <summary>
    /// Helper to return parameter value as string, with additional
    /// support for element id to display the element type referred to.
    /// </summary>
    public static string GetParameterValue2( Parameter param, Document doc )
    {
      string s;
      if( StorageType.ElementId == param.StorageType
        && null != doc )
      {
        ElementId id = param.AsElementId();

        int i = id.IntegerValue;

        if( 0 > i )
        {
          s = i.ToString()
            + BuiltInCategoryString( i );
        }
        else
        {
          Element e = doc.GetElement( id );
          s = ElementDescription( e, true );
        }
      }
      else
      {
        s = GetParameterValue( param );
      }
      return s;
    }
    #endregion // Helpers for parameters

    #region Helpers for shared parameters
    /// <summary>
    /// Helper to get shared parameters file.
    /// </summary>
    public static DefinitionFile GetSharedParamsFile(
      Application app )
    {
      // Get current shared params file name
      string sharedParamsFileName;
      try
      {
        sharedParamsFileName = app.SharedParametersFilename;
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
    #endregion // Helpers for shared parameters
  }
}
