#region Copyright
//
// (C) Copyright 2010 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
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
// Migrated to C# by Adam Nagy
//
#endregion // Copyright

#region Imports
// Import the following name spaces in the project properties/references.
// Note: VB.NET has a slighly different way of recognizing name spaces than C#.
// if you explicitely set them in each .vb file, you will need to specify full name spaces.

using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes; // '' specify this if you want to save typing for attributes.

#endregion

#region Description
// Revit Intro Lab - 1
//
// In this lab, you will learn how to "hook" your add-on program to Revit.
// This command defines a minimum external command.
// You will learn:
//
// Explain about addin manifest. How to create GUID.
// Hello World in VB.NET is form page 360 of Developer Guide.
//
#endregion

namespace RevitIntroCS
{
  // Hello World #1 - A minimum Revit external command.
  //
  [Autodesk.Revit.Attributes.Transaction( Autodesk.Revit.Attributes.TransactionMode.Automatic )]
  [Autodesk.Revit.Attributes.Regeneration( Autodesk.Revit.Attributes.RegenerationOption.Manual )]
  public class HelloWorld : IExternalCommand
  {
    public Autodesk.Revit.UI.Result Execute( Autodesk.Revit.UI.ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements )
    {
      Autodesk.Revit.UI.TaskDialog.Show( "My Dialog Title", "Hello World!" );

      return Result.Succeeded;
    }
  }

  // Hello World #2 - simplified without full namespace.
  //
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class HelloWorldSimple : IExternalCommand
  {
    public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      TaskDialog.Show( "My Dialog Title", "Hello World Simple!" );

      return Result.Succeeded;
    }
  }

  // Hello World #3 - minimum external application
  // diference: IExternalApplication instead of IExternalCommand. in addin manifest.
  // Use addin type "Application", use <Name/> instead of <Text/>.
  //

  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class HelloWorldApp : IExternalApplication
  {
    // OnShutdown() - called when Revit ends.
    //
    public Autodesk.Revit.UI.Result OnShutdown( Autodesk.Revit.UI.UIControlledApplication application )
    {
      return Result.Succeeded;
    }

    // OnStartup() - called when Revit starts.
    //
    public Autodesk.Revit.UI.Result OnStartup( Autodesk.Revit.UI.UIControlledApplication application )
    {
      TaskDialog.Show( "My Dialog Title", "Hello World from App!" );

      return Result.Succeeded;
    }
  }

  // Command Arguments
  // Take a look at the command arguments. commandData is the top most
  // object and the entry point to the Revit model.
  //
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class CommandData : IExternalCommand
  {
    public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      // The first argument, commandData, is the top most object model.
      // You will get the necessary information from commandData.
      // To see what's in there, print out a few data accessed from commandData
      //
      // Exercise: Place a break point at commandData and drill down the data.
      //
      string versionName = commandData.Application.Application.VersionName;
      string documentTitle = commandData.Application.ActiveUIDocument.Document.Title;
      TaskDialog.Show( "Revit Intro Lab", "Version Name = " + versionName + "\n" + "Document Title = " + documentTitle );

      // print out a list of wall types available in the current rvt project.
      WallTypeSet wallTypes = commandData.Application.ActiveUIDocument.Document.WallTypes;
      string s = "";
      foreach( WallType wType in wallTypes )
      {
        s = s + wType.Name + "\n";
      }

      // show the result.
      TaskDialog.Show( "Revit Intro Lab", "Wall Types: " + "\n\n" + s );

      // 2nd and 3rd arguments are when the command fails.
      // 2nd - set a message to the user.
      // 3rd - set elements to highlight.

      return Result.Succeeded;
    }
  }
}