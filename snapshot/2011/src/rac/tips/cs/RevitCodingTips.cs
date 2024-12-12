#region Header
// Copyright (c) 2007-2009 by Autodesk, Inc.
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
//
// Created by: Bill Zhang and Jeremy Tammik, Autodesk Inc., Oct. 2007
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Timers;
using WinForms = System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using Choice = System.Windows.Forms.DialogResult;

/*
//
// Tip 1 - no global 'using':
//
// If you *always* prefix every item with its full namespace prefix,
// you will never run into any ambiguity, and the code can be copied
// between different modules without risk of confusion.
//
using Rvt = Autodesk.Revit;
using RvtGeom = Autodesk.Revit.Geometry;
using RvtSymbs = Autodesk.Revit.Symbols;
using RvtElems = Autodesk.Revit.Elements;
//
// Tip 1 - global 'using' + aliases to disambiguate:
//
// For more readability, making use of module-global 'using' settings
// reduces text clutter. Ambiguities may occur, e.g. some types such
// as Application, Document, and Element are declared in several
// namespaces, both Revit and others. For instance, we have both
// System.Windows.Forms.Application and Autodesk.Revit.Application,
// and both Autodesk.Revit.Element and Autodesk.Revit.Geometry.Element.
//
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Symbols;
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
 */

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion // Namespaces

namespace RevitCodingTips
{
  #region Util
  /// <summary>
  /// Revit coding tips utility class.
  /// </summary>
  class Util
  {
    public const string Caption = "Revit Coding Tips";

    /// <summary>
    /// MessageBox wrapper for informational message.
    /// </summary>
    public static void InfoMsg( string msg )
    {
      Debug.WriteLine( msg );
      MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information );
    }

    /// <summary>
    /// MessageBox wrapper for user answering yes, no or cancel.
    /// </summary>
    public static Choice QuestionMsg( string question )
    {
      Choice choice = MessageBox.Show( question, Caption, WinForms.MessageBoxButtons.YesNoCancel );
      Debug.WriteLine( question + " " + choice.ToString() );
      return choice;
    }

    /// <summary>
    /// Return an English plural suffix 's' or nothing for the given number of items.
    /// </summary>
    public static string PluralSuffix( int n )
    {
      return 1 == n ? "" : "s";
    }

    /// <summary>
    /// Return the first level found in the current document, or null.
    /// </summary>
    public static Level GetFirstLevel( Document doc )
    {
      FilteredElementCollector levels = new FilteredElementCollector( doc );
      levels.OfCategory( BuiltInCategory.OST_Levels );
      levels.OfClass( typeof( Level ) );
      Level level = levels.FirstElement() as Level;
      Debug.Assert( null != level, "expected filter to return non-null level" );
      return level;
    }
  }
  #endregion // Util

  #region WaitCursor
  class WaitCursor
  {
    WinForms.Cursor _oldCursor;

    public WaitCursor()
    {
      _oldCursor = WinForms.Cursor.Current;
      WinForms.Cursor.Current = WinForms.Cursors.WaitCursor;
    }

    ~WaitCursor()
    {
      WinForms.Cursor.Current = _oldCursor;
    }
  }
  #endregion // WaitCursor

  #region Tip 0 – Element Filters
  /// <summary>
  /// Tip 0 – Element Filters
  ///
  /// Several of the later tips will create walls for test purposes.
  /// To do so, they need to determine a level to place it on.
  /// Demonstrate selecting the first level using 2008-style iteration
  /// over all document elements, and 2009-style element filtering
  /// and compare the time. Actually, the time used for the 2008
  /// style iteration is much less in Revit 2009 than in 2008 due to related optimisations.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandElementFilters : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        UIApplication uiapp = commandData.Application;
        Document doc = uiapp.ActiveUIDocument.Document;

        int repeat = 100;
        Level level2 = null;
        Stopwatch sw = Stopwatch.StartNew();

        /*for( int i = 0; i < repeat; ++i )
        {
          level = Util.GetFirstLevel_2008( doc );
        }
        sw.Stop();
        long ms2008 = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();
         */

        for( int i = 0; i < repeat; ++i )
        {
          level2 = Util.GetFirstLevel( doc );
        }
        sw.Stop();
        long ms2009 = sw.ElapsedMilliseconds;

        /*
        Debug.Assert( level.Id.Equals( level2.Id ), "expected same level using 2008 and 2009 style retrieval" );

        Util.InfoMsg( string.Format(
          "Milliseconds required to retrieve first building level {0} times:"
          + "\nusing 2008-style document elements: {1}"
          + "\nusing 2009-style element filters: {2}",
          repeat, ms2008, ms2009 ) );
        */ 

        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // Tip 0 – Element Filters

  #region Tip 1 - Namespaces, Alises, and Regions
  /// <summary>
  /// Tip 1 - Namespaces, Alises, and Regions
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandNamespaces : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        UIApplication uiapp = commandData.Application;
        UIDocument uidoc = uiapp.ActiveUIDocument;
        Application app = uiapp.Application;
        Document doc = uidoc.Document;

        Autodesk.Revit.Creation.Application creApp = app.Create;
        Autodesk.Revit.Creation.Document creDoc = doc.Create;

        Element buildingElement = null;
        GeometryElement geometryElement = null;
        if( null == buildingElement && null == geometryElement )
        {
          // added the 'if' statement to remove the warnings
          // warning CS0219: The variable 'buildingElement' is assigned but its value is never used
          // warning CS0219: The variable 'geometryElement' is assigned but its value is never used
        }
        WinForms.MessageBox.Show( "This is Tip 1 - No global 'using'", "Revit Tips" );
        return Result.Succeeded;
      }
      catch( System.Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // Tip 1 - Namespaces, Alises, and Regions

  #region Tip 2 - Version Checking
  /// <summary>
  /// Tip 2 - Version Checking - Create a wall according to the Revit application version
  ///
  /// This command checks the version information of Revit and does something accordingly:
  /// 1. In version less than 2008, do nothing;
  /// 2. In Revit Architecture 2008, create an ARCHITECTURAL wall;
  /// 3. In Revit Structure 2008, create a STRUCTURAL wall;
  /// 4. In Revit MEP 2008, do not create a wall but still pop up a message.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandVersionChecking : IExternalCommand
  {
    enum Flavour { Architecture, Structure, MEP, Other };

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        //
        // extract application etc. from command data:
        //
        UIApplication uiapp = commandData.Application;
        UIDocument uidoc = uiapp.ActiveUIDocument;
        Application app = uiapp.Application;
        Document doc = uidoc.Document;

        Autodesk.Revit.Creation.Application creApp = app.Create;
        Autodesk.Revit.Creation.Document creDoc = doc.Create;
        //
        // display version data:
        //
        string versionName = app.VersionName;
        string str = "Version name: " + versionName + "\n";
        str += "Version number: " + app.VersionNumber + "\n";
        str += "Version build: " + app.VersionBuild;

        Util.InfoMsg( str );
        //
        // check version number:
        //
        if( 2009 > int.Parse( app.VersionNumber ) )
        {
          message = string.Format( "You need a higher version of Revit,"
            + " at least 2009, not {0}.", app.VersionNumber );
          return Result.Cancelled;
        }
        //
        // determine Revit application flavour:
        //
        Flavour flavour = Flavour.Other;
        foreach( Flavour a in Enum.GetValues( typeof( Flavour ) ) )
        {
          if( versionName.Contains( a.ToString() ) )
          {
            flavour = a;
            break;
          }
        }
        //
        // create a wall depending on Revit flavour at first level:
        //
        XYZ startPoint = new XYZ( 0, 0, 0 );
        XYZ endPoint = new XYZ(100, 100, 0);
        Line line = creApp.NewLine( startPoint, endPoint, true );
        Level level = Util.GetFirstLevel( doc );
        Wall newWall = null;
        if( null != level )
        {
          switch( flavour )
          {
            case Flavour.Architecture:
              newWall = creDoc.NewWall( line, level, false );
              str = "Created an ARCHITECTURAL wall on " + level.Name + " in " + versionName + ".";
              break;
            case Flavour.Structure:
              newWall = creDoc.NewWall( line, level, true );
              str = "Created a STRUCTURAL wall on " + level.Name + " in " + versionName + ".";
              break;
            case Flavour.MEP:
              // no wall created in MEP or other flavour
              str = "No wall created in " + versionName + ".";
              break;
            default:
              str = "I do not know this flavour of Revit: " + versionName + ".";
              break;
          }
          Util.InfoMsg( str );
        }
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // Tip 2 - Version Checking

  #region Tip 3 - Error Handling and Transactions
  /// <summary>
  /// Tip 3 - Error Handling
  ///
  /// This command works only with WALL elements.
  /// It takes the following scenarios into account:
  /// 1. If nothing is selected, return an error message and Cancelled result code;
  /// 2. If only walls are selected, pup up a good message and return a Succeeded result code;
  /// 3. If some elements other than walls are selected,
  ///    put those elements into the returned ElementSet collection,
  ///    put an error message into the returned string,
  ///    and return an error code. In this way, Revit will hilight
  ///    those elements and pop up a dialog to alert us the error.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandErrorHandling : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        UIApplication uiapp = commandData.Application;
        UIDocument uidoc = uiapp.ActiveUIDocument;
        Application app = uiapp.Application;
        Document doc = uidoc.Document;

        SelElementSet selSet = uidoc.Selection.Elements;
        if( 0 == selSet.Size )
        {
          message = "Please select some elements.";
          return Result.Cancelled;
        }
        ElementSet walls = new ElementSet();
        foreach( Element e in selSet )
        {
          if( e is Wall )
          {
            walls.Insert( e );
          }
          else
          {
            elements.Insert( e );
          }
        }
        int n = walls.Size;
        if( 0 < n )
        {
          Util.InfoMsg( string.Format( "{0} wall{1} correctly selected.", n, Util.PluralSuffix( n ) ) );
        }
        n = elements.Size;
        if( 0 < n )
        {
          message = string.Format( "{0} non-wall element{1} incorrectly selected.", n, Util.PluralSuffix( n ) );
          return Result.Failed;
        }
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }

  /// <summary>
  /// Tip 3 - Transactions -- see the Revi SDK TransactionControl sample
  /// </summary>
  /*
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandTransaction : IExternalCommand
  {
    Application _app;
    Document _doc;

    Wall createWall( Line line )
    {
      Level level = Util.GetFirstLevel( _doc );
      return ( null == level ) ? null : _doc.Create.NewWall( line, level, false );
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;

      _app = uiapp.Application;
      _doc = uidoc.Document;

      Autodesk.Revit.Creation.Application creApp = _app.Create;

      try
      {
        _doc.BeginTransaction();
        Util.InfoMsg( "Begin outermost transaction." );

        // Create a wall
        XYZ startPoint = new XYZ( 0, 50, 0 );
        XYZ endPoint = new XYZ( 100, 50, 0 );
        Line line = creApp.NewLine( startPoint, endPoint, true );
        createWall( line );
        Util.InfoMsg( "A wall has been created." );

        // This section could be nested in another try block,
        // where something might go wrong
        _doc.BeginTransaction();
        Util.InfoMsg( "Begin nested transaction." );

        // Create another wall
        startPoint = new XYZ( 50, 0, 0 );
        endPoint = new XYZ( 50, 100, 0 );
        line = creApp.NewLine( startPoint, endPoint, true );
        createWall( line );
        Util.InfoMsg( "Another wall has been created." );

        // This could be called in the nested exception handler
        _doc.AbortTransaction();
        Util.InfoMsg( "Aborted nested transaction." );

        _doc.EndTransaction();
        Util.InfoMsg( "Committed outmost transaction." );
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }

  /// <summary>
  /// Tip 3 - Additional Document Transaction
  ///
  /// In order to modify an additional document accessed
  /// through OpenDocumentFile(), one must open and manage
  /// a transaction oneself.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandDocumentTransaction : IExternalCommand
  {
    static void CreateLevel( Document doc, String levelName, double levelElevation )
    {
      Level newLevel = doc.Create.NewLevel( levelElevation / 304.8 );
      newLevel.Name = levelName;
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref String message,
      ElementSet elements )
    {
      string filename = "C:\\temp\\leveltest.rvt";
      Document doc = commandData.Application.OpenDocumentFile( filename );
      try
      {
        // normal execution stays here
        doc.BeginTransaction();
        CreateLevel( doc, "Level Test", 5000 );
        doc.EndTransaction();
        doc.SaveAs( filename );
        doc.Close();
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        // handle exceptions here
        doc.AbortTransaction();
        message = ex.Message;
        return Result.Failed;
      }
      finally
      {
        // do clean up here
      }
    }
  }
  */
  #endregion // Tip 3 - Error Handling and Transactions

  #region Tip 4 - Language Independence
  /// <summary>
  /// Tip 4 - Language Independence - Element Checking With Language Independent Built-in Category Name
  ///
  /// Use language independent built-in category name to check element type.
  /// Here we check whether a selected element is a window or a door.
  /// Another possibility is to scan the Revit.ini file for the section
  /// [Language]
  /// Select=ENU
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandLanguageIndependent : IExternalCommand
  {
    bool isDoorCategory( Element e )
    {
      Categories categories = e.Document.Settings.Categories;
      Category doorCategory = categories.get_Item( BuiltInCategory.OST_Doors );
      bool languageDependentTest = ( "Doors" == e.Category.Name ); // avoid this!
      bool invalidCategoryComparison = ( e.Category == doorCategory ); // wrong!
      bool languageIndependentTest = ( e.Category.Id.Equals( doorCategory.Id ) ); // yes
      return languageIndependentTest;
    }

    /// <summary>
    /// Use the category element id rather than the category name for comparison
    /// so the code works unmodified for different Revit language versions.
    /// Do not compare the category itself directly, that is unreliable.
    /// </summary>
    bool elementCategoryEquals( Element e, BuiltInCategory enumCat )
    {
      Categories categories = e.Document.Settings.Categories;
      Category category = categories.get_Item( enumCat );
      return e.Category.Id.Equals( category.Id );
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Application app = uiapp.Application;
      Document doc = uidoc.Document;

      Selection sel = uidoc.Selection;

      try
      {
        if( null == sel || 0 == sel.Elements.Size )
        {
          message = "Please select an element before starting this command.";
          return Result.Failed;
        }
        else if( 1 != sel.Elements.Size )
        {
          message = "Please select exactly one element before starting this command.";
          return Result.Failed;
        }
        foreach( Element e in sel.Elements )
        {
          if( elementCategoryEquals( e, BuiltInCategory.OST_Doors ) )
          {
            Util.InfoMsg( "The selected element is a door." );
          }
          else if( elementCategoryEquals( e, BuiltInCategory.OST_Windows ) )
          {
            Util.InfoMsg( "The selected element is a window." );
          }
          else
          {
            Util.InfoMsg( "The selected is neither a door nor a window." );
          }
        }
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }

  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandLanguageDependent : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Application app = uiapp.Application;
      Document doc = uidoc.Document;

      Categories categories = doc.Settings.Categories;
      Selection sel = uidoc.Selection;

      try
      {
        if( null == sel || 0 == sel.Elements.Size )
        {
          message = "Please select an element before starting this command.";
          return Result.Failed;
        }
        else if( 1 != sel.Elements.Size )
        {
          message = "Please select exactly one element before starting this command.";
          return Result.Failed;
        }
        foreach( Element e in sel.Elements )
        {
          if( "Doors" == e.Category.Name )
          {
            Util.InfoMsg( "The selected element is a door." );
          }
          else if( "Windows" == e.Category.Name )
          {
            Util.InfoMsg( "The selected element is a window." );
          }
          else
          {
            Util.InfoMsg( "The selected is neither a door nor a window." );
          }
        }
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }

  /// <summary>
  /// Determine current Revit language.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandLanguageCheck : IExternalCommand
  {
    //
    // CultureInfo.Name Property (System.Globalization)
    // http://msdn2.microsoft.com/en-us/library/system.globalization.cultureinfo.name(VS.80).aspx
    // The CultureInfo.Name property follows the RFC 1766 standard in the format
    // "<languagecode2>-<country/regioncode2>", where <languagecode2> is a lowercase
    // two-letter code derived from ISO 639-1 and <country/regioncode2> is an uppercase
    // two-letter code derived from ISO 3166. For example, the Name for the specific
    // culture U.S. English is "en-US". In cases where a two-letter language code is
    // not available, the three-letter code derived from ISO 639-2 is used; for example,
    // the three-letter code "div" is used for cultures that use the Dhivehi language.
    // If the culture is a neutral culture, its Name is in the format "<languagecode2>".
    // For example, the Name for the neutral culture English is "en".
    //
    // List of Windows XP's Three Letter Acronyms for Languages
    // http://www.microsoft.com/globaldev/reference/winxp/langtla.mspx
    // The following table lists the three letter acronyms (TLA) that the Windows XP
    // "GetLocaleInfo" API returns when called with the LCType, LOCALE_SABBREVLANGNAME.
    // Notes: In most cases, the TLA is created by taking the two-letter language
    // abbreviation from the ISO Standard 639 and adding a third letter, as appropriate,
    // to indicate the sublanguage.
    //
    /// <summary>
    /// Three Letter Acronyms for Languages
    /// </summary>
    enum LanguageTla
    {
      Undefined,
      Deu, // German - Germany
      Enu, // English - United States
    }

    /// <summary>
    /// Determine current Revit language.
    /// This method is not yet complete, and will definitely not even
    /// work for German and English in its current state, because the
    /// German words are actually written with umlauts.
    /// Besides using category names, we can also make use of built-in
    /// parameter names.
    /// </summary>
    /// <param name="doc">Current Revit document</param>
    /// <returns>Current Revit language</returns>
    static LanguageTla GetLanguage( Document doc )
    {
      LanguageTla tla = LanguageTla.Undefined;
      Categories categories = doc.Settings.Categories;
      Category doorsCategory = categories.get_Item( BuiltInCategory.OST_Doors );
      Category windowsCategory = categories.get_Item( BuiltInCategory.OST_Windows );
      if( "Tueren" == doorsCategory.Name && "Fenster" == windowsCategory.Name )
      {
        tla = LanguageTla.Deu;
      }
      else if( "Doors" == doorsCategory.Name && "Windows" == windowsCategory.Name )
      {
        tla = LanguageTla.Enu;
      }
      return tla;
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        UIApplication uiapp = commandData.Application;
        UIDocument uidoc = uiapp.ActiveUIDocument;
        Application app = uiapp.Application;
        Document doc = uidoc.Document;

        LanguageTla tla = GetLanguage( doc );
        Util.InfoMsg( string.Format( "The current Revit language TLA is {0}.", tla ) );
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // Tip 4 - Language Independence

  #region Tip 5 - Data Caching
  /// <summary>
  /// Tip 5 - Data Caching - Build Data Cache
  ///
  /// This example consists of two commands.
  /// Please run CommandBuildDataCache first and CommandUseDataCache next.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandBuildDataCache : IExternalCommand
  {
    static private ICollection<ElementId> _levelIdSet = null;
    static private WallTypeSet _wallTypeSet = new WallTypeSet();

    /// <summary>
    /// Provide access to cached levels.
    /// </summary>
    static public ICollection<ElementId> LevelIds
    {
      get
      {
        return _levelIdSet;
      }
    }

    /// <summary>
    /// Provide access to cached wall types, paralleling the Document.WallTypes property.
    /// </summary>
    static public WallTypeSet WallTypes
    {
      get
      {
        return _wallTypeSet;
      }
    }

    /// <summary>
    /// Retrieve data from current document and populate cache.
    /// </summary>
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Application app = uiapp.Application;
      Document doc = uidoc.Document;

      try
      {
        _wallTypeSet = doc.WallTypes;

        FilteredElementCollector levels = new FilteredElementCollector( doc );
        levels.OfCategory( BuiltInCategory.OST_Levels );
        levels.OfClass( typeof( Level ) );
        _levelIdSet = levels.ToElementIds();
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }

  /// <summary>
  /// Tip 5 - Data Caching - Access Cached Data
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandUseDataCache : IExternalCommand
  {
    /// <summary>
    /// Access cached data.
    /// </summary>
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Application app = uiapp.Application;
      Document doc = uidoc.Document;

      try
      {
        string str = string.Empty;
        int n = CommandBuildDataCache.WallTypes.Size;
        if( 0 < n )
        {
          str += string.Format( "{0} wall type{1} cached:\n", n, Util.PluralSuffix( n ) );
          foreach( WallType wt in CommandBuildDataCache.WallTypes )
          {
            str += "\n  " + wt.Name;
          }
        }
        n = CommandBuildDataCache.LevelIds.Count;
        if( 0 < n )
        {
          str += string.Format( "\n\n{0} level{1} cached:\n", n, Util.PluralSuffix( n ) );
          foreach( ElementId id in CommandBuildDataCache.LevelIds )
          {
            Level level = doc.get_Element( id ) as Level;
            str += "\n  " + level.Name;
          }
        }
        Util.InfoMsg( str );
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // Tip 5 - Data Caching

  #region Tip 6 - Journaling and Unit Testing
  //
  // Tip 6 - Journaling and Unit Testing
  //
  // Please use the SDK sample Journaling to demonstrate this.
  //
  #endregion // Tip 6 - Journaling and Unit Testing

  #region Tip 7 - Assembly Information
  /// <summary>
  /// Tip 7 - Assembly Information
  ///
  /// Use auto-incremented build and revision number and other
  /// assembly information in our app, for instance in the About box.
  /// </summary>
  //
  // Please refer to the version number information in AssemblyInfo.cs.
  // In addition, note that the format is Major.Minor.Build.Revision.
  // The auto-generated build and revision numbers are defined as follows:
  // build is the number of days since Jan 1, 2000 in local time, and
  // revision is the number of seconds since the previous midnight in
  // local time MOD 2.
  //
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandAssemblyInfo : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        System.Reflection.Assembly asm
          = System.Reflection.Assembly.GetExecutingAssembly();

        string str = "Assembly Information:"
          + "\nName: " + asm.GetName().Name
          + "\nVersion: " + asm.GetName().Version;

        Util.InfoMsg( str );
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // Tip 7 - Assembly Information

  #region Tip 8 - Suspend Updating
  /// <summary>
  /// Tip 8 - Suspend Updating
  ///
  /// Suspend Revit model updating to improve application performance.
  /// There is a limitation that SuspendUpdating is only implemented for
  /// some operations as documented in the CHM file. Wall creation is not
  /// one of these operations, so the obsolete example above has been replaced.
  /// Also, from what I read online, if that is to be trusted, using the
  /// C# StopWatch is more accurate than DateTime.
  ///
  /// Suspending updating during the mirror operation did not bring any
  /// advantage when mirroring walls one by one using doc.Mirror( w, reference ),
  /// as demonstrated above in CommandSuspendUpdatingDuringMirror. The results were
  ///   wall count    with    without
  ///       64      19.361     20.873
  ///      128      46.721     42.353
  ///      256    2:56.982   2:47.157
  ///
  /// Here is a sample that creates a given number of free standing walls
  /// through the API and then uses either Move() or Mirror() on them,
  /// depending on a compile time switch, and encapsulates the operation
  /// within a SuspendUpdating code block if the user so selects.
  ///
  /// Time to move 1000 walls without SuspendUpdating: 00:00:07.767
  /// Time to move 1000 walls with SuspendUpdating: 00:00:07.839
  /// Time to move 1000 walls without SuspendUpdating: 00:00:07.849
  /// Time to move 1000 walls with SuspendUpdating: 00:00:07.826
  ///
  /// Time to mirror 1000 walls without SuspendUpdating: 00:00:06.379
  /// Time to mirror 1000 walls with SuspendUpdating: 00:00:06.234
  /// Time to mirror 1000 walls without SuspendUpdating: 00:00:06.352
  /// Time to mirror 1000 walls with SuspendUpdating: 00:00:06.194
  /// Time to mirror 1000 walls without SuspendUpdating: 00:00:06.346
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandSuspendUpdating : IExternalCommand
  {
    Application _app;
    Document _doc;

    /// <summary>
    /// Create the specified number of free standing walls.
    /// </summary>
    /// <param name="n">Number of walls to create</param>
    /// <returns>Set of walls created</returns>
    ElementSet createWalls( int n )
    {
      WaitCursor waitCursor = new WaitCursor();
      double length = 10.0;
      XYZ p = XYZ.Zero, q = new XYZ( 0, length, 0 );
      Level level = Util.GetFirstLevel( _doc );
      ElementSet walls = new ElementSet();
      Line line;
      for( int i = 0; i < n; ++i )
      {
        line = _app.Create.NewLineBound( p, q );
        walls.Insert( _doc.Create.NewWall( line, level, false ) );
        p += 5 * XYZ.BasisX;
        q += 5 * XYZ.BasisX;
      }
      return walls;
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      
      _app = uiapp.Application;
      _doc = uidoc.Document;

      int n = 1000;
      ElementSet walls = createWalls( n );

#if MOVE
      XYZ translation = new XYZ( 10, 10, 0 );
      string n_walls = string.Format( "Move {0} wall{1}", n, Util.PluralSuffix( n ) );
#else // mirror
      XYZ p = new XYZ( 0, 15, 0 ), q = new XYZ( 100, 15, 0 );
      Line line = _app.Create.NewLineBound( p, q );
      string n_walls = string.Format( "Mirror {0} wall{1}", n, Util.PluralSuffix( n ) );
#endif // MOVE

      Choice choice = Util.QuestionMsg( n_walls + " ... suspend updating?" );
      bool rc, useSuspendUpdating = ( Choice.Yes == choice );
      if( Choice.Cancel == choice )
      {
        return Result.Cancelled;
      }
      WaitCursor waitCursor = new WaitCursor();
      Stopwatch stopWatch = Stopwatch.StartNew();
      if( useSuspendUpdating )
      {
        using( SuspendUpdating suspender = new SuspendUpdating( _doc ) )
        {
      #if MOVE
          rc = _doc.Move( walls, translation );
      #else // mirror
          rc = _doc.Mirror( walls, line );
      #endif // MOVE
        }
      }
      else
      {
      #if MOVE
          rc = _doc.Move( walls, translation );
      #else // mirror
        rc = _doc.Mirror( walls, line );
      #endif // MOVE
      }
      stopWatch.Stop();

      TimeSpan ts = stopWatch.Elapsed;
      string elapsedTime = String.Format( "{0:00}:{1:00}:{2:00}.{3:000}",
        ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds );
      string msg = string.Format( "Time to {0} with{1} SuspendUpdating: {2}",
        n_walls.ToLower(), (useSuspendUpdating ? "" : "out"), elapsedTime );
      Util.InfoMsg( msg );
      return Result.Succeeded;
    }
  }
  #endregion // Tip 8 - Suspend Updating

  #region Tip 9 - Language Comparison C#, VB and C++
  /// <summary>
  /// Tip 9 - Language Comparison C#, VB and C++
  ///
  /// List out wall family, type and parameter information with C#.
  /// We do the same thing in C#, VB.NET and VC.NET
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandListWallInfo : IExternalCommand
  {
    private string ListWallFamilyAndTypeInfo( Element elem, ref string cat )
    {
      string infoStr = "";
      string title = "";

      title = "Element category";
      if( null != elem.Category )
      {
        title = title + " : " + elem.Category.Name;
      }
      if( elem is Wall )
      {
        Wall wall = elem as Wall;
        WallType wallType = wall.WallType;
        switch( wallType.Kind )
        {
          case WallKind.Basic:
            infoStr += "Basic : ";
            break;
          case WallKind.Curtain:
            infoStr += "Curtain : ";
            break;
          case WallKind.Stacked:
            infoStr += "Stacked : ";
            break;
          case WallKind.Unknown:
            infoStr += "Unknown : ";
            break;
        }
        infoStr += wallType.Name;
      }
      cat = title;
      return infoStr;
    }

    private string ListParameters( Element elem )
    {
      string str = string.Empty;
      ParameterSet pars = elem.Parameters;
      if( pars.IsEmpty )
      {
        str = "Parameters are empty.";
      }
      else
      {
        foreach( Parameter param in pars )
        {
          string name = param.Definition.Name;
          StorageType type = param.StorageType;
          string val = "<nil>";
          switch( type )
          {
            case StorageType.Double:
              val = param.AsDouble().ToString();
              break;
            case StorageType.ElementId:
              ElementId eId = param.AsElementId();
              Element paraElem = elem.Document.get_Element( eId );
              if( null != paraElem )
              {
                val = paraElem.Name;
              }
              break;
            case StorageType.Integer:
              val = param.AsInteger().ToString();
              break;
            case StorageType.String:
              val = param.AsString();
              break;
          }
          string s2 = param.AsValueString(); // compare this with 'val'
          str += name + ": " + val + "\n";
        }
      }
      return str;
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        UIApplication uiapp = commandData.Application;
        UIDocument uidoc = uiapp.ActiveUIDocument;
        Application app = uiapp.Application;
        Document doc = uidoc.Document;

        ElementSet selSet = uidoc.Selection.Elements;
        if( 1 != selSet.Size )
        {
          message = "Please select exactly one wall.";
          return Result.Cancelled;
        }
        Wall wall = null;
        foreach( Element elem in selSet )
        {
          wall = elem as Wall;
        }
        if( null == wall )
        {
          message = "Please select exactly one wall.";
          return Result.Cancelled;
        }
        string catName = "";
        MessageBox.Show( ListWallFamilyAndTypeInfo( wall, ref catName ), catName );
        MessageBox.Show( ListParameters( wall ), " - Parameters of the Wall - " );
        Debug.Assert( null != wall.WallType, "expected valid wall type" );
        if( null != wall.WallType )
        {
          WallType wallType = wall.WallType;
          MessageBox.Show( ListParameters( wallType ), " - Parameters of the Wall Type - " );
        }
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion // Tip 9 - Language Comparison C#, VB and C++

  #region Tip 10 - Duplicating a Type
  /// <summary>
  /// Tip 10 - Duplicating a Type
  ///
  /// A frequent question from developers is how to create a new type 
  /// for a given family, which is not obvious from the help file or 
  /// existing samples. You can achieve this using the Duplicate() 
  /// method to create a new type, and then modify its parameters 
  /// or properties. Cf. Revit API intro lab 3-6.
  /// </summary>
  /// 
  /*
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandDuplicateRoundColumnType : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Application app = uiapp.Application;
      Document doc = uidoc.Document;

      string familyName = "Concrete-Round-Column";
      Filter f = app.Create.Filter.NewFamilyFilter( 
        familyName );

      List<Element> families = new List<Element>();
      doc.get_Elements( f, families );
      if( 1 > families.Count )
      {
        message = "No suitable family found.";
        return Result.Failed;
      }

      Family fam = families[0] as Family;
      FamilySymbol famSym = null;
      foreach( FamilySymbol fs in fam.Symbols )
      {
        famSym = fs;
        break;
      }

      // create a new family symbol using Duplicate method:
      string newFamilyName = "NewRoundColumn 3";
      FamilySymbol newFamSym = famSym.Duplicate( 
        newFamilyName ) as FamilySymbol;

      //set the radius to a new value:
      Parameter par = newFamSym.get_Parameter( "b" );
      par.Set( 3 );

      return Result.Succeeded;
    }
  }
  */
  #endregion // Tip 10 - Duplicating a Type

  #region Tip 11 - Parameter Value Enumerations
  /// <summary>
  /// Tip 11 - Parameter Value Enumerations
  ///
  /// A number of Revit parameters take enumerated integer values.
  ///
  /// Clarify case 1243139 [Updating End Releases in Revit Model],
  /// BuiltInParameter.STRUCTURAL_END_RELEASE_TYPE,
  /// STRUCTURAL_END_RELEASE_MX, Y, Z and FX, Y, Z:
  /// 0 – fixed, 1 – pinned, 2 - Bending Moment, 3 - User Defined
  ///
  /// Clarify case 1243033, Model graphics style mapping for views:
  /// 1- Wire frame, 2- Hidden frame, 3- Shading, 4- Shading w/edges
  ///
  /// Wall Function (WALL_ATTR_EXTERIOR) parameter influences the
  /// created wall instance Room Bounding and Structural Usage parameter:
  /// 0-Interior	1-Exterior	2-Foundation	3-Retaining	4-Soffit
  /// </summary>
  #endregion // Tip 11 - Parameter Value Enumerations

  #region Tip 12 - Unit Handling
  /// <summary>
  /// Tip 12 - Unit Handling
  ///
  /// Revit internally works with fixed database units, and all raw data
  /// obtained through the API is in database units. The unit used for
  /// length is feet. The user interface can be set up to display other
  /// units, and these settings are available through the API. Parameter
  /// values can be queried for both raw data in internal units through
  /// the double value, or through the Parameter.AsValueString() method
  /// to retrieve the value as displayed in the user interface.
  ///
  /// The MidasLink sample, available through ADN, includes a module
  /// demonstrating a simple form of unit handling in UnitConversion.cs.
  /// </summary>
  #endregion // Tip 12 - Unit Handling
}
