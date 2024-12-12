#region Header
//
// CmdSharedParamModelGroup.cs - create a shared
// parameter for the doors, walls, inserted DWG,
// model groups, and model lines.
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Symbols;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdCreateSharedParams : IExternalCommand
  {
    const string _filename = "C:/tmp/SharedParams.txt";
    const string _groupname = "The Building Coder Parameters";
    const string _defname = "SP";
    ParameterType _deftype = ParameterType.Number;
    //
    // What element types are we interested in? The standard
    // SDK FireRating sample uses BuiltInCategory.OST_Doors.
    //
    // We can also use BuiltInCategory.OST_Walls to demonstrate
    // that the same technique works with system families just
    // as well as with standard ones.
    //
    // To test attaching shared parameters to inserted DWG files,
    // which generate their own category on the fly, we can also
    // identify the category by category name instead of built-
    // in category enumeration, as discussed in
    // http://thebuildingcoder.typepad.com/blog/2008/11/adding-a-shared-parameter-to-a-dwg-file.html
    //
    // We can attach shared parameters to model groups.
    // Unfortunately, this does not work in the
    // same way as the others, because we cannot retrieve the
    // category from the document Settings.Categories collection.
    //
    // In that case, we can obtain the category from an existing
    // instance of a group.
    //
    BuiltInCategory[] targets = new BuiltInCategory[] {
      BuiltInCategory.OST_Doors,
      BuiltInCategory.OST_Walls,
      //"Drawing1.dwg", // inserted DWG file
      BuiltInCategory.OST_IOSModelGroups, // doc.Settings.Categories.get_Item returns null
      //"Model Groups", // doc.Settings.Categories.get_Item with this argument throws an exception SystemInvalidOperationException "Operation is not valid due to the current state of the object."
      BuiltInCategory.OST_Lines // model lines
    };

    Category GetCategory( Application app, BuiltInCategory target )
    {
      Document doc = app.ActiveDocument;
      Category cat = null;

      if( target.Equals( BuiltInCategory.OST_IOSModelGroups ) )
      {
        //
        // determine model group category:
        //
        Autodesk.Revit.Creation.Filter cf
          = app.Create.Filter;

        List<Element> modelGroups
          = new List<Element>();

        Filter fType = cf.NewTypeFilter(
          typeof( Group ) );

        //Filter fType = cf.NewTypeFilter( // this works as well
        //  typeof( GroupType ) );

        Filter fCategory = cf.NewCategoryFilter(
          BuiltInCategory.OST_IOSModelGroups );

        Filter f = cf.NewLogicAndFilter(
          fType, fCategory );

        if( 0 == doc.get_Elements( f, modelGroups ) )
        {
          Util.ErrorMsg( "Please insert a model group." );
          return cat;
        }
        else
        {
          cat = modelGroups[0].Category;
        }
      }
      else
      {
        try
        {
          cat = doc.Settings.Categories.get_Item( target );
        }
        catch( Exception ex )
        {
          Util.ErrorMsg( string.Format(
            "Error obtaining document {0} category: {1}",
            target.ToString(), ex.Message ) );
          return cat;
        }
      }
      if( null == cat )
      {
        Util.ErrorMsg( string.Format(
          "Unable to obtain the document {0} category.",
          target.ToString() ) );
      }
      return cat;
    }

    bool CreateSharedParameter(
      Application app,
      Category cat,
      int nameSuffix )
    {
      Document doc = app.ActiveDocument;
      //
      // get or set the current shared params filename:
      //
      string filename
        = app.Options.SharedParametersFilename;

      if( 0 == filename.Length )
      {
        string path = _filename;
        StreamWriter stream;
        stream = new StreamWriter( path );
        stream.Close();
        app.Options.SharedParametersFilename = path;
        filename = app.Options.SharedParametersFilename;
      }
      //
      // get the current shared params file object:
      //
      DefinitionFile file
        = app.OpenSharedParameterFile();

      if( null == file )
      {
        Util.ErrorMsg(
          "Error getting the shared params file." );

        return false;
      }
      //
      // get or create the shared params group:
      //
      DefinitionGroup group
        = file.Groups.get_Item( _groupname );

      if( null == group )
      {
        group = file.Groups.Create( _groupname );
      }

      if( null == group )
      {
        Util.ErrorMsg(
          "Error getting the shared params group." );

        return false;
      }
      //
      // set visibility of the new parameter:
      //
      // Category.AllowsBoundParameters property indicates if a category can
      // have shared or project parameters. If it is false, it may not be bound
      // to shared parameters using the BindingMap. Please not that non-user-visible
      // parameters can still be bound to these categories.
      //
      bool visible = cat.AllowsBoundParameters;
      //
      // get or create the shared params definition:
      //
      string defname = _defname + nameSuffix.ToString();

      Definition definition = group.Definitions.get_Item(
        defname );

      if( null == definition )
      {
        definition = group.Definitions.Create(
          defname, _deftype, visible );
      }
      if( null == definition )
      {
        Util.ErrorMsg(
          "Error in creating shared parameter." );

        return false;
      }
      //
      // create the category set containing our category for binding:
      //
      CategorySet catSet = app.Create.NewCategorySet();
      catSet.Insert( cat );
      //
      // bind the param:
      //
      try
      {
        Binding binding = app.Create.NewInstanceBinding(
          catSet );
        //
        // we could check if it is already bound,
        // but it looks like insert will just ignore
        // it in that case:
        //
        doc.ParameterBindings.Insert( definition, binding );
        //
        // we can also specify the parameter group here:
        //
        //doc.ParameterBindings.Insert( definition, binding, 
        //  BuiltInParameterGroup.PG_GEOMETRY );

      }
      catch( Exception ex )
      {
        Util.ErrorMsg( string.Format(
          "Error binding shared parameter to category {0}: {1}",
          cat.Name, ex.Message ) );
        return false;
      }
      return true;
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      int i = 0;

      foreach( BuiltInCategory target in targets )
      {
        Category cat = GetCategory( app, target );
        CreateSharedParameter( app, cat, ++i );
      }
      return CmdResult.Succeeded;
    }
  }
}
