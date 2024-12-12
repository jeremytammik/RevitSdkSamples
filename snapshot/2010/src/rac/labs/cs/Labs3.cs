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
using WinForms = System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Symbols;
#endregion // Namespaces

namespace Labs
{
  #region Lab3_1_StandardFamiliesAndTypes

  #region 2008 Version
  /// <summary>
  /// List all loaded standard families and types in Revit 2008.
  /// </summary>
  public class Lab3_1_StandardFamiliesAndTypes_2008 : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Iterate all elements and look for Family classes
      Document doc = commandData.Application.ActiveDocument;
      string sMsg = "Standard Families already loaded in this model:";
      ElementIterator iter = doc.Elements;
      while( iter.MoveNext() )
      {
        Family f = iter.Current as Family;
        if( null != f )
        {
          // Try to get its category name; notice that the
          // Category property is NOT implemented for the Family class:
          string famCatName = ( null == f.Category ) ? "?" : f.Category.Name;
          sMsg += "\r\n  Name=" + f.Name + "; Category=" + famCatName;
        }
      }
      LabUtils.InfoMsg( sMsg );
      // Let's do a similar loop, but now get all the child symbols (types) as well.
      // These symbols can also be used to determine the category.
      iter = doc.Elements;
      while( iter.MoveNext() )
      {
        Family f = iter.Current as Family;
        if( null != f )
        {
          string catName;
          bool first = true;
          // Loop all contained symbols (types)
          foreach( FamilySymbol symb in f.Symbols )
          {
            // Determine the category via first symbol
            if( first )
            {
              first = false;
              try
              {
                catName = symb.Category.Name;
              }
              catch( Exception )
              {
                catName = "?";
              }
              sMsg = "Family: Name=" + f.Name
                + "; Id=" + f.Id.Value.ToString()
                + "; Category=" + catName
                + "\r\nContains Types:";
            }
            sMsg += "\r\n    " + symb.Name + "; Id=" + symb.Id.Value.ToString();
          }
          // Show the symbols for this family and allow user to proceed
          // to the next family (OK) or cancel (Cancel)
          sMsg += "\r\nContinue?";
          if( !LabUtils.QuestionMsg( sMsg ) )
          {
            break;
          }
        }
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // 2008 Version

  /// <summary>
  /// List all loaded standard families and types using Revit 2009 filtering.
  /// </summary>
  public class Lab3_1_StandardFamiliesAndTypes : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      //
      // get all family elements in current document:
      //
      List<Element> families = new List<Element>();
      Filter filterFamily = app.Create.Filter.NewTypeFilter( typeof( Family ) );
      doc.get_Elements( filterFamily, families );
      string sMsg = "Standard families already loaded in this model:";
      foreach( Family f in families )
      {
        // Get its category name; notice that the Category property is not
        // implemented for the Family class; use FamilyCategory instead;
        // notice that that is also not always implemented; in that case,
        // use the workaround demonstrated below, looking at the contained
        // family symbols' category:
        sMsg += "\r\n  Name=" + f.Name
          + "; Category=" + ( ( null == f.Category ) ? "?" : f.Category.Name )
          + "; FamilyCategory=" + ( ( null == f.FamilyCategory ) ? "?" : f.FamilyCategory.Name );
      }
      LabUtils.InfoMsg( sMsg );

      // Loop through the collection of families, and now look at
      // the child symbols (types) as well. These symbols can be
      // used to determine the family category.
      foreach( Family f in families )
      {
        string catName;
        bool first = true;
        // Loop all contained symbols (types)
        foreach( FamilySymbol symb in f.Symbols )
        {
          // you can determine the family category from its first symbol.
          if( first )
          {
            first = false;
            catName = symb.Category.Name;
            sMsg = "Family: Name=" + f.Name
              + "; Id=" + f.Id.Value.ToString()
              + "; Category=" + catName
              + "\r\nContains Types:";
          }
          sMsg += "\r\n    " + symb.Name + "; Id=" + symb.Id.Value.ToString();
        }
        // Show the symbols for this family and allow user to proceed
        // to the next family (OK) or cancel (Cancel)
        sMsg += "\r\nContinue?";
        if( !LabUtils.QuestionMsg( sMsg ) )
        {
          break;
        }
      }

      #region Unimplemented family category property causes issue with filters
      //
      // using standard filtering to retrieve families of a specific category fails;
      // this code works well for retrieving FamilySymbol instances, 
      // but does not work for Family instances
      // ... 1245627 [Filtering by Family type]:
      //
      {
        Autodesk.Revit.Creation.Filter creator = app.Create.Filter;
        Filter fDoors = creator.NewCategoryFilter( BuiltInCategory.OST_Doors );
        Filter fWindows = creator.NewCategoryFilter( BuiltInCategory.OST_Windows );
        Filter fOr = creator.NewLogicOrFilter( fDoors, fWindows );
        Filter fFamily = creator.NewTypeFilter( typeof( Family ) ); // FamilySymbol
        Filter fMain = creator.NewLogicAndFilter( fFamily, fOr );
        List<Element> els = new List<Element>();
        doc.get_Elements( fMain, els );
        Debug.WriteLine( els.Count.ToString() + " families selected." );
        // 16 family symbols selected.
        // 0 families selected.
      }
      #endregion // Unimplemented family category property causes issue with filters

      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab3_1_StandardFamiliesAndTypes

  #region Lab3_2_LoadStandardFamilies
  /// <summary>
  /// Load an entire family or a specific type from a family.
  /// </summary>
  public class Lab3_2_LoadStandardFamilies : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      //
      // Load a whole Family
      //
      // Example for a family WITH TXT file:
      if( doc.LoadFamily( LabConstants.WholeFamilyFileToLoad1 ) )
      {
        LabUtils.InfoMsg( "Successfully loaded family " + LabConstants.WholeFamilyFileToLoad1 + "." );
      }
      else
      {
        LabUtils.ErrorMsg( "ERROR loading family " + LabConstants.WholeFamilyFileToLoad1 + "." );
      }

      // Example for a family WITHOUT TXT file:
      if( doc.LoadFamily( LabConstants.WholeFamilyFileToLoad2 ) )
      {
        LabUtils.InfoMsg( "Successfully loaded family " + LabConstants.WholeFamilyFileToLoad2 + "." );
      }
      else
      {
        LabUtils.ErrorMsg( "ERROR loading family " + LabConstants.WholeFamilyFileToLoad2 + "." );
      }

      // Load only a specific Symbol (Type)
      // The symbol MUST exist in the corresponding catalog (TXT) file - same as in the UI
      if( doc.LoadFamilySymbol( LabConstants.FamilyFileToLoadSingleSymbol, LabConstants.SymbolName ) )
      {
        LabUtils.InfoMsg( "Successfully loaded family symbol " + LabConstants.FamilyFileToLoadSingleSymbol + " : " + LabConstants.SymbolName + "." );
      }
      else
      {
        LabUtils.ErrorMsg( "ERROR loading family symbol " + LabConstants.FamilyFileToLoadSingleSymbol + " : " + LabConstants.SymbolName + "." );
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab3_2_LoadStandardFamilies

  #region Lab3_3_DetermineInstanceTypeAndFamily

  #region 2008 Version
  /// <summary>
  /// For a selected family instance in the model, determine its type and family in Revit 2008.
  /// </summary>
  public class Lab3_3_DetermineInstanceTypeAndFamily_2008 : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      // Loop through the model to report all FamilySymbol
      // objects of "Windows" category.
      Categories categories = doc.Settings.Categories;
      Category catWindows = categories.get_Item( BuiltInCategory.OST_Windows );
      string sMsg = "The loaded windows family symbols in the model are:";
      ElementIterator iter = doc.Elements;
      while( iter.MoveNext() )
      {
        FamilySymbol symb = iter.Current as FamilySymbol;
        // Check for FamilySymbol having Windows category
        if( null != symb )
        {
          try
          {
            Category catFS = symb.Category; // for "Profiles" it fails
            if( null != catFS )
            {
              //
              // Either of these comparisons will do:
              //
              // if catFS.Id.Value.Equals(catWindows.Id.Value)
              // if catFS.Id.Equals(catWindows.Id)
              // if catFS.Name.Equals(catWindows.Name)
              //
              // Do not compare the category itself directly, this is unreliable:
              //
              // if( catFS.Equals( catWindows ) )
              //
              if( catFS.Id.Equals( catWindows.Id ) )
              {
                sMsg += "\r\n    " + symb.Name + ", Id="
                  + symb.Id.Value.ToString();
                try
                {
                  Family fam = symb.Family;
                  sMsg += "; Family name=" + fam.Name
                    + ", Family Id=" + fam.Id.Value.ToString();
                }
                catch( Exception )
                {
                }
              }
            }
          }
          catch( Exception )
          {
          }
        }
      }
      LabUtils.InfoMsg( sMsg );

      // Now loop the selection set and check for standard Family Instances of "Windows" category
      foreach( Element elem2 in doc.Selection.Elements )
      {
        if( elem2 is FamilyInstance )
        {
          FamilyInstance inst = elem2 as FamilyInstance;
          if( null != inst.Category && inst.Category.Id.Equals( catWindows.Id ) )
          {
            sMsg = "Selected window Id=" + elem2.Id.Value.ToString() + "\r\n";
            FamilySymbol fs1 = inst.Symbol;
            sMsg += "  FamilySymbol = " + fs1.Name + "; Id=" + fs1.Id.Value.ToString() + "\r\n";
            Family f1 = fs1.Family;
            sMsg += "  Family = " + f1.Name + "; Id=" + f1.Id.Value.ToString();
            LabUtils.InfoMsg( sMsg );
          }
        }
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // 2008 Version

  /// <summary>
  /// For a selected family instance in the model, determine
  /// its type and family using Revit 2009 filtering.
  /// </summary>
  public class Lab3_3_DetermineInstanceTypeAndFamily : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements)
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      // Use the filtering mechanism to report all FamilySymbol objects of "Windows" category.
      BuiltInCategory bic = BuiltInCategory.OST_Windows;
      List<Element> familySymbols = LabUtils.GetAllFamilySymbols( app, bic );
      string sMsg = "The loaded windows family symbols in the model are:";
      foreach( Element elem in familySymbols )
      {
        FamilySymbol symb = elem as FamilySymbol;
        sMsg += "\r\n    " + symb.Name + ", Id=" + symb.Id.Value.ToString();
        Family fam = symb.Family;
        sMsg += "; Family name=" + fam.Name + ", Family Id=" + fam.Id.Value.ToString();
      }
      LabUtils.InfoMsg( sMsg );

      // Now loop the selection set and check for standard Family Instances of "Windows" category
      Categories categories = doc.Settings.Categories;
      Category catWindows = categories.get_Item( bic );
      foreach( Element elem2 in doc.Selection.Elements )
      {
        if( elem2 is FamilyInstance )
        {
          FamilyInstance inst = elem2 as FamilyInstance;
          if( null != inst.Category && inst.Category.Id.Equals( catWindows.Id ) )
          {
            sMsg = "Selected window Id=" + elem2.Id.Value.ToString();
            FamilySymbol fs = inst.Symbol;
            sMsg += "\r\n  FamilySymbol = " + fs.Name + "; Id=" + fs.Id.Value.ToString();
            Family f = fs.Family;
            sMsg += "\r\n  Family = " + f.Name + "; Id=" + f.Id.Value.ToString();
            LabUtils.InfoMsg( sMsg );
          }
        }
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab3_3_DetermineInstanceTypeAndFamily

  #region Lab3_4_ChangeSelectedInstanceType
  /// <summary>
  /// Form-based utility to change the type or symbol of a selected standard instance.
  /// </summary>
  public class Lab3_4_ChangeSelectedInstanceType : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      ElementSet ss = doc.Selection.Elements;
      // Make sure we have a single FamilyInstance selected
      if( 1 != ss.Size )
      {
        LabUtils.ErrorMsg( "Please pre-select a single family instance element." );
        return IExternalCommand.Result.Cancelled;
      }
      ElementSetIterator it = ss.ForwardIterator();
      it.MoveNext();
      FamilyInstance inst = it.Current as FamilyInstance;
      if( null == inst )
      {
        LabUtils.ErrorMsg( "Selected element is not a standard family instance." );
        return IExternalCommand.Result.Cancelled;
      }
      Category instCat = inst.Category;
      Dictionary<string, List<FamilySymbol>> dictFamilyToSymbols = new Dictionary<string, List<FamilySymbol>>();
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
        // Find all the corresponding Families/Types:
        List<Element> families = new List<Element>();
        Filter filterFamily = app.Create.Filter.NewTypeFilter( typeof( Family ) );
        doc.get_Elements( filterFamily, families );
        foreach( Family f in families )
        {
          bool categoryMatches = false;
          if( null == f.FamilyCategory )
          {
            foreach( FamilySymbol sym in f.Symbols )
            {
              categoryMatches = sym.Category.Id.Equals( instCat.Id );
              break;
            }
          }
          else
          {
            categoryMatches = f.FamilyCategory.Id.Equals( instCat.Id );
          }
          if( categoryMatches )
          {
            List<FamilySymbol> familySymbols = new List<FamilySymbol>();
            foreach( FamilySymbol sym in f.Symbols )
            {
              familySymbols.Add( sym );
            }
            dictFamilyToSymbols.Add( f.Name, familySymbols );
          }
        }
      }
      // Display the form, allowing the user to select a family
      // and a type, and assign this type to the instance.
      Lab3_4_Form frm = new Lab3_4_Form( dictFamilyToSymbols );
      if( WinForms.DialogResult.OK == frm.ShowDialog() )
      {
        inst.Symbol = frm.cmbType.SelectedItem as FamilySymbol;
        LabUtils.InfoMsg( "Successfully changed family:type to " + frm.cmbFamily.Text + " : " + frm.cmbType.Text );
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab3_4_ChangeSelectedInstanceType

  #region Lab3_5_WallAndFloorTypes
  /// <summary>
  /// List all wall and floor types, and change the type of selected walls and floors,
  /// as an example of handling system, i.e. non-stamdard, families and types.
  /// </summary>
  public class Lab3_5_WallAndFloorTypes : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      // Find all wall types and their system families (or kinds)
      WallType newWallType = null;
      string sMsg = "All wall types and families in the model:";
      foreach( WallType wt in doc.WallTypes )
      {
        sMsg += "\r\n  Type=" + wt.Name + " Family=" + wt.Kind.ToString();
        newWallType = wt;
      }
      LabUtils.InfoMsg( sMsg );
      LabUtils.InfoMsg( "Stored WallType " + newWallType.Name + " (Id=" + newWallType.Id.Value.ToString() + ") for later use" );

      // Find all floor types:
      //
      // Since Revit 2008, the Document class has a FloorTypes property
      // to retrieve the collection of all floor types. We can simply use
      // doc.FloorTypes like this:
      //
      FloorType newFloorType = null;
      Category floorCat = doc.Settings.Categories.get_Item( BuiltInCategory.OST_Floors );
      sMsg = "All floor types in the model:";
      foreach( FloorType ft in doc.FloorTypes )
      {
        sMsg += "\r\n  Type=" + ft.Name + ", Id=" + ft.Id.Value.ToString();
        // In 9.0, the "Foundation Slab" system family from "Structural
        // Foundations" category ALSO contains FloorType class instances.
        // Be careful to exclude those as choices for standard floor types.
        Parameter p = ft.get_Parameter( BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM );
        string famName = null == p ? "?" : p.AsString();
        Category cat = ft.Category;
        sMsg += ", Family=" + famName + ", Category=" + cat.Name;
        if( floorCat.Id.Equals( cat.Id ) ) // store only if proper Floors category
        {
          newFloorType = ft;
        }
      }
      // Unfortunately, FloorTypes includes the structural foundation slabs, too.
      // One way to obtain only floor types directly is to use the category and
      // explicitly request type of FloorType.
      //Dim filterFloorType As Filter = app.Create.Filter.NewTypeFilter(GetType(FloorType))
      //Dim filterFloorCategory As Filter = app.Create.Filter.NewCategoryFilter(BuiltInCategory.OST_Floors)
      //Dim filterFloorTypeFloorCategory As Filter = app.Create.Filter.NewLogicAndFilter(filterFloorType, filterFloorCategory)
      //Dim elementList As New List(Of Revit.Element)
      //Dim num As Integer = doc.Elements(filterFloorTypeFloorCategory, elementList)

      LabUtils.InfoMsg( sMsg );
      LabUtils.InfoMsg( "Stored FloorType " + newFloorType.Name + " (Id=" + newFloorType.Id.Value.ToString() + ") for later use" );

      // Change the type for selected walls and floors
      ElementSet sel = doc.Selection.Elements;
      int iWall = 0;
      int iFloor = 0;
      // Loop all selection elements
      foreach( Element el in sel )
      {
        if( el is Wall ) // Check for walls
        {
          ++iWall;
          Wall wall = el as Wall;
          WallType oldWallType = wall.WallType;
          // change wall type and report the old/new values
          wall.WallType = newWallType;
          LabUtils.InfoMsg( "Wall " + iWall.ToString() + ": Id=" + wall.Id.Value.ToString()
            + "\r\n  changed from OldType=" + oldWallType.Name + "; Id=" + oldWallType.Id.Value.ToString()
            + "  to NewType=" + wall.WallType.Name + "; Id=" + wall.WallType.Id.Value.ToString() );
        }
        else if( el is Floor )
        {
          ++iFloor;
          Floor f = el as Floor;
          FloorType oldFloorType = f.FloorType;
          f.FloorType = newFloorType;
          LabUtils.InfoMsg( "Floor " + iFloor.ToString() + ": Id=" + f.Id.Value.ToString()
            + "\r\n  changed from old type=" + oldFloorType.Name + "; Id=" + oldFloorType.Id.Value.ToString()
            + "  to new type=" + f.FloorType.Name + "; Id=" + f.FloorType.Id.Value.ToString() );
        }
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab3_5_WallAndFloorTypes

  #region Lab3_6_DuplicateWallType
  /// <summary>
  /// Create a new family symbol or type by calling Duplicate() on an existing one and then modifying its parameters.
  /// </summary>
  public class Lab3_6_DuplicateWallType : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      ElementSet els = doc.Selection.Elements;
      const string newWallTypeName = "NewWallType_with_Width_doubled";

      foreach( Element e in els )
      {
        Wall wall = e as Wall;
        if( null != wall )
        {
          WallType wallType = wall.WallType;
          WallType newWallType = wallType.Duplicate( newWallTypeName ) as WallType;
          CompoundStructureLayerArray layers = newWallType.CompoundStructure.Layers;
          foreach( CompoundStructureLayer layer in layers )
          {
            // double each layer thickness:
            layer.Thickness *= 2.0;
          }
          // assign the new wall type back to the wall:
          wall.WallType = newWallType;
          // only process the first wall, if one was selected:
          break;
        }
      }
      return IExternalCommand.Result.Succeeded;

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

        Material mat = doc.get_Element( ref matId ) 
          as Autodesk.Revit.Elements.Material;

        // change the color of this material:
        mat.Color = new Color( 255, 0, 0 );

        // assign the new symbol to the instance:
        inst.Symbol = dupSym;
      }
#endif // ASSIGN_COLOUR
      #endregion // Assign colour

    }
  }
  #endregion // Lab3_6_DuplicateWallType

  #region Lab3_7_DeleteFamilyType
  /// <summary>
  /// Delete a specific individual type from a family.
  /// Hard-coded to a column type named "457 x 610mm".
  /// </summary>
  public class Lab3_7_DeleteFamilyType : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      Filter filter = app.Create.Filter.NewCategoryFilter(
        BuiltInCategory.OST_Columns );

      ElementIterator it = doc.get_Elements( filter );

      while ( it.MoveNext() )
      {
        Element e = it.Current as Element;
        if ( e.Name.Equals( "457 x 610mm" ) )
        {
          doc.Delete( e );
        }
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab3_7_DeleteFamilyType
}
