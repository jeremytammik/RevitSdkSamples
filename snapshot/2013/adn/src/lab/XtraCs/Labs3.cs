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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace XtraCs
{
  #region Lab3_1_StandardFamiliesAndTypes
  /// <summary>
  /// List all loaded standard families and types.
  /// As a result of the issues explained below, we implemented
  /// the LabUtils utility methods FamilyCategory and GetFamilies.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="3-1"]/*' />
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  public class Lab3_1_StandardFamiliesAndTypes : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      List<string> a = new List<string>();
      FilteredElementCollector families;

      #region 3.1.a Retrieve and iterate over all Family objects in document:
      //
      // retrieve all family elements in current document:
      //
      families = new FilteredElementCollector( doc );

      families.OfClass( typeof( Family ) );

      foreach( Family f in families )
      {
        // Get its category name; notice that the Category property is not
        // implemented for the Family class; use FamilyCategory instead;
        // notice that that is also not always implemented; in that case,
        // use the workaround demonstrated below, looking at the contained
        // family symbols' category:

        a.Add( string.Format( "Name={0}; Category={1}; FamilyCategory={2}",
          f.Name,
          ( ( null == f.Category ) ? "?" : f.Category.Name ),
          ( ( null == f.FamilyCategory ) ? "?" : f.FamilyCategory.Name ) ) );
      }
      #endregion // 3.1.a

      string msg = "{0} standard familie{1} are loaded in this model{2}";
      LabUtils.InfoMsg( msg, a );

      // Loop through the collection of families, and now look at
      // the child symbols (types) as well. These symbols can be
      // used to determine the family category.

      foreach( Family f in families )
      {
        string catName;
        bool first = true;

        // Loop all contained symbols (types):

        foreach( FamilySymbol s in f.Symbols )
        {
          // you can determine the family category from its first symbol.

          if( first )
          {
            first = false;

            #region 3.1.b Retrieve category name of first family symbol:
            catName = s.Category.Name;
            #endregion // 3.1.b

            msg = "Family: Name=" + f.Name
              + "; Id=" + f.Id.IntegerValue.ToString()
              + "; Category=" + catName
              + "\r\nContains Types:";
          }
          msg += "\r\n    " + s.Name + "; Id=" + s.Id.IntegerValue.ToString();
        }

        // Show the symbols for this family and allow user to proceed
        // to the next family (OK) or cancel (Cancel)

        msg += "\r\nContinue?";
        if( !LabUtils.QuestionMsg( msg ) )
        {
          break;
        }
      }
      //
      // return all families whose name contains the substring "Round Duct":
      //
      IEnumerable<Family> round_duct_families
        = LabUtils.GetFamilies( doc, "Round Duct", true );

      int n = round_duct_families.Count();

      return Result.Failed;
    }
  }
  #endregion // Lab3_1_StandardFamiliesAndTypes

  #region Lab3_2_LoadStandardFamilies
  /// <summary>
  /// Load an entire family or a specific type from a family.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="3-2"]/*' />
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  public class Lab3_2_LoadStandardFamilies : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;
      bool rc;

      #region 3.2.a Load an entire RFA family file:

      // Load a whole Family
      //
      // Example for a family WITH TXT file:

      rc = doc.LoadFamily( LabConstants.WholeFamilyFileToLoad1 );
      #endregion // 3.2.a

      if( rc )
      {
        LabUtils.InfoMsg( "Successfully loaded family "
          + LabConstants.WholeFamilyFileToLoad1 + "." );
      }
      else
      {
        LabUtils.ErrorMsg( "ERROR loading family "
          + LabConstants.WholeFamilyFileToLoad1 + "." );
      }

      // Example for a family WITHOUT TXT file:

      rc = doc.LoadFamily( LabConstants.WholeFamilyFileToLoad2 );

      if( rc )
      {
        LabUtils.InfoMsg( "Successfully loaded family "
          + LabConstants.WholeFamilyFileToLoad2 + "." );
      }
      else
      {
        LabUtils.ErrorMsg( "ERROR loading family "
          + LabConstants.WholeFamilyFileToLoad2 + "." );
      }

      #region 3.2.b Load an individual type from a RFA family file:

      // Load only a specific symbol (type):

      rc = doc.LoadFamilySymbol(
        LabConstants.FamilyFileToLoadSingleSymbol,
        LabConstants.SymbolName );
      #endregion // 3.2.b

      if( rc )
      {
        LabUtils.InfoMsg( "Successfully loaded family symbol "
          + LabConstants.FamilyFileToLoadSingleSymbol + " : "
          + LabConstants.SymbolName + "." );
      }
      else
      {
        LabUtils.ErrorMsg( "ERROR loading family symbol "
          + LabConstants.FamilyFileToLoadSingleSymbol + " : "
          + LabConstants.SymbolName + "." );
      }
      return Result.Succeeded;
    }
  }
  #endregion // Lab3_2_LoadStandardFamilies

  #region Lab3_3_DetermineInstanceTypeAndFamily
  /// <summary>
  /// For a selected family instance in the model, determine its type and family.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="3-3"]/*' />
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  public class Lab3_3_DetermineInstanceTypeAndFamily : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      UIDocument uidoc = app.ActiveUIDocument;
      Document doc = uidoc.Document;

      // retrieve all FamilySymbol objects of "Windows" category:

      BuiltInCategory bic = BuiltInCategory.OST_Windows;
      FilteredElementCollector symbols = LabUtils.GetFamilySymbols( doc, bic );

      List<string> a = new List<string>();

      foreach( FamilySymbol s in symbols )
      {
        Family fam = s.Family;

        a.Add( s.Name
          + ", Id=" + s.Id.IntegerValue.ToString()
          + "; Family name=" + fam.Name
          + ", Family Id=" + fam.Id.IntegerValue.ToString() );
      }
      LabUtils.InfoMsg( "{0} windows family symbol{1} loaded in the model{1}", a );

      // loop through the selection set and check for
      // standard family instances of "Windows" category:

      int iBic = (int) bic;
      string msg, content;

      foreach( Element e in uidoc.Selection.Elements )
      {
        if( e is FamilyInstance
          && null != e.Category
          && e.Category.Id.IntegerValue.Equals( iBic ) )
        {
          msg = "Selected window Id=" + e.Id.IntegerValue.ToString();

          FamilyInstance inst = e as FamilyInstance;

          #region 3.3 Retrieve the type of the family instance, and the family of the type:

          FamilySymbol fs = inst.Symbol;

          Family f = fs.Family;

          #endregion // 3.3

          content = "FamilySymbol = " + fs.Name
            + "; Id=" + fs.Id.IntegerValue.ToString();

          content += "\r\n  Family = " + f.Name
            + "; Id=" + f.Id.IntegerValue.ToString();

          LabUtils.InfoMsg( msg, content );
        }
      }
      return Result.Succeeded;
    }
  }
  #endregion // Lab3_3_DetermineInstanceTypeAndFamily

  #region Lab3_4_ChangeSelectedInstanceType
  /// <summary>
  /// Form-based utility to change the type or symbol of a selected standard family instance.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="3-4"]/*' />
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  public class Lab3_4_ChangeSelectedInstanceType
    : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      UIDocument uidoc = app.ActiveUIDocument;
      Document doc = uidoc.Document;

      FamilyInstance inst =
        LabUtils.GetSingleSelectedElementOrPrompt(
          uidoc, typeof( FamilyInstance ) )
          as FamilyInstance;

      if( null == inst )
      {
        LabUtils.ErrorMsg(
          "Selected element is not a "
          + "standard family instance." );

        return Result.Cancelled;
      }

      // determine selected instance category:

      Category instCat = inst.Category;

      Dictionary<string, List<FamilySymbol>>
        mapFamilyToSymbols
          = new Dictionary<string, List<FamilySymbol>>();

      {
        WaitCursor waitCursor = new WaitCursor();

        // Collect all types applicable to this category and sort them into
        // a dictionary mapping the family name to a list of its types.
        //
        // We create a collection of all loaded families for this category
        // and for each one, the list of all loaded types (symbols).
        //
        // There are many ways how to store the matching objects, but we choose
        // whatever is most suitable for the relevant UI. We could use Revit's
        // generic Map class, but it is probably more efficient to use the .NET
        // strongly-typed Dictionary with
        // KEY = Family name (String)
        // VALUE = list of corresponding FamilySymbol objects
        //
        // find all corresponding families and types:

        FilteredElementCollector families
          = new FilteredElementCollector( doc );

        families.OfClass( typeof( Family ) );

        foreach( Family f in families )
        {
          bool categoryMatches = false;

          // we cannot trust f.Category or
          // f.FamilyCategory, so grab the category
          // from first family symbol instead:

          foreach( FamilySymbol sym in f.Symbols )
          {
            categoryMatches = sym.Category.Id.Equals(
              instCat.Id );

            break;
          }

          if( categoryMatches )
          {
            List<FamilySymbol> symbols
              = new List<FamilySymbol>();

            foreach( FamilySymbol sym in f.Symbols )
            {
              symbols.Add( sym );
            }

            mapFamilyToSymbols.Add( f.Name, symbols );
          }
        }
      }

      // display the form allowing the user to select
      // a family and a type, and assign this type
      // to the instance.

      Lab3_4_Form form
        = new Lab3_4_Form( mapFamilyToSymbols );

      if( System.Windows.Forms.DialogResult.OK
        == form.ShowDialog() )
      {
        inst.Symbol = form.cmbType.SelectedItem
          as FamilySymbol;

        LabUtils.InfoMsg(
          "Successfully changed family : type to "
          + form.cmbFamily.Text + " : "
          + form.cmbType.Text );
      }
      return Result.Succeeded;
    }
  }
  #endregion // Lab3_4_ChangeSelectedInstanceType

  #region Lab3_5_WallAndFloorTypes
  /// <summary>
  /// Access and modify system family type, similarly
  /// to the standard families looked at above:
  ///
  /// List all wall and floor types and
  /// change the type of selected walls and floors.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="3-5"]/*' />
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  public class Lab3_5_WallAndFloorTypes : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      UIDocument uidoc = app.ActiveUIDocument;
      Document doc = uidoc.Document;

      // Find all wall types and their system families (or kinds):

      WallType newWallType = null;

      string msg = "All wall types and families in the model:";
      string content = string.Empty;

      foreach( WallType wt in doc.WallTypes )
      {
        content += "\nType=" + wt.Name + " Family=" + wt.Kind.ToString();
        newWallType = wt;
      }

      content += "\n\nStored WallType " + newWallType.Name
        + " (Id=" + newWallType.Id.IntegerValue.ToString()
        + ") for later use.";

      LabUtils.InfoMsg( msg, content );

      // Find all floor types:

      FloorType newFloorType = null;

      msg = "All floor types in the model:";
      content = string.Empty;

      foreach( FloorType ft in doc.FloorTypes )
      {
        content += "\nType=" + ft.Name + ", Id=" + ft.Id.IntegerValue.ToString();

        // In 9.0, the "Foundation Slab" system family from "Structural
        // Foundations" category ALSO contains FloorType class instances.
        // Be careful to exclude those as choices for standard floor types.

        Parameter p = ft.get_Parameter( BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM );
        string famName = null == p ? "?" : p.AsString();
        Category cat = ft.Category;
        content += ", Family=" + famName + ", Category=" + cat.Name;

        // store for the new floor type only if it has the proper Floors category:

        if( cat.Id.Equals( (int) BuiltInCategory.OST_Floors ) )
        {
          newFloorType = ft;
        }
      }

      content += ( null == newFloorType )
        ? "\n\nNo floor type found."
        : "\n\nStored FloorType " + newFloorType.Name
        + " (Id=" + newFloorType.Id.IntegerValue.ToString()
        + ") for later use";

      LabUtils.InfoMsg( msg, content );

      // Change the type for selected walls and floors:

      msg = "{0} {1}: Id={2}"
        + "\r\n  changed from old type={3}; Id={4}"
        + "  to new type={5}; Id={6}.";

      ElementSet sel = uidoc.Selection.Elements;

      int iWall = 0;
      int iFloor = 0;

      foreach( Element e in sel )
      {
        if( e is Wall )
        {
          ++iWall;
          Wall wall = e as Wall;
          WallType oldWallType = wall.WallType;

          // change wall type and report the old/new values

          wall.WallType = newWallType;

          LabUtils.InfoMsg( string.Format( msg, "Wall",
            iWall, wall.Id.IntegerValue,
            oldWallType.Name, oldWallType.Id.IntegerValue,
            wall.WallType.Name, wall.WallType.Id.IntegerValue ) );
        }
        else if( null != newFloorType && e is Floor )
        {
          ++iFloor;
          Floor f = e as Floor;
          FloorType oldFloorType = f.FloorType;
          f.FloorType = newFloorType;

          LabUtils.InfoMsg( string.Format( msg, "Floor",
            iFloor, f.Id.IntegerValue,
            oldFloorType.Name, oldFloorType.Id.IntegerValue,
            f.FloorType.Name, f.FloorType.Id.IntegerValue ) );
        }
      }
      return Result.Succeeded;
    }
  }
  #endregion // Lab3_5_WallAndFloorTypes

  #region Lab3_6_DuplicateWallType
  /// <summary>
  /// Create a new family symbol or type by calling Duplicate()
  /// on an existing one and then modifying its parameters.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  public class Lab3_6_DuplicateWallType : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      UIDocument uidoc = app.ActiveUIDocument;
      ElementSet a = uidoc.Selection.Elements;

      const string newWallTypeName = "NewWallType_with_Width_doubled";

      foreach( Element e in a )
      {
        Wall wall = e as Wall;

        if( null != wall )
        {
          WallType wallType = wall.WallType;

          WallType newWallType = wallType.Duplicate(
            newWallTypeName ) as WallType;

          //CompoundStructureLayerArray layers = newWallType.CompoundStructure.Layers; // 2011
          IList<CompoundStructureLayer> layers = newWallType.GetCompoundStructure().GetLayers(); // 2012

          foreach( CompoundStructureLayer layer in layers )
          {
            // double each layer thickness:

            //layer.Thickness *= 2.0; // 2011

            layer.Width *= 2.0; // 2012
          }
          // assign the new wall type back to the wall:

          wall.WallType = newWallType;

          // only process the first wall, if one was selected:

          break;
        }
      }
      return Result.Succeeded;

      #region Assign colour
#if ASSIGN_COLOUR
      ElementSet elemset = doc.Selection.Elements;

      foreach( Element e in elemset )
      {
        FamilyInstance inst = e as FamilyInstance;

        // get the symbol and duplicate it:
        FamilySymbol dupSym = inst.Symbol.Duplicate(
          "D1" ) as FamilySymbol;

        // access the material:
        ElementId matId = dupSym.get_Parameter(
          "Material" ).AsElementId();

        Material mat = doc.GetElement( ref matId )
          as Autodesk.Revit.Elements.Material;

        // change the color of this material:
        mat.Color = new Color( 255, 0, 0 );

        // assign the new symbol to the instance:
        inst.Symbol = dupSym;
#endif // ASSIGN_COLOUR
      #endregion // Assign colour

    }
  }
  #endregion // Lab3_6_DuplicateWallType

  #region Lab3_7_DeleteFamilyType
  /// <summary>
  /// Delete a specific individual type from a family.
  /// Hard-coded to a column type named "475 x 610mm".
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  public class Lab3_7_DeleteFamilyType : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      FilteredElementCollector collector
        = LabUtils.GetFamilySymbols( doc,
          BuiltInCategory.OST_Columns );

      #region Test explicit LINQ statement and cast
#if USE_EXPLICIT_LINQ_STATEMENT
      var column_types = from element in collector
        where element.Name.Equals( "475 x 610mm" )
        select element;

      FamilySymbol symbol = column_types
        .Cast<FamilySymbol>()
        .First<FamilySymbol>();
#endif // USE_EXPLICIT_LINQ_STATEMENT

#if CAST_TO_FAMILY_SYMBOL
      FamilySymbol symbol = collector.First<Element>(
        e => e.Name.Equals( "475 x 610mm" ) )
          as FamilySymbol;
#endif // CAST_TO_FAMILY_SYMBOL
      #endregion // Test explicit LINQ statement and cast

      Element symbol = collector.First<Element>(
        e => e.Name.Equals( "475 x 610mm" ) );

      doc.Delete( symbol );

      return Result.Succeeded;
    }
  }
  #endregion // Lab3_7_DeleteFamilyType
}
