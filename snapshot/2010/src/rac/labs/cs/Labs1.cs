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
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Enums;
#endregion // Namespaces

namespace Labs
{
  #region Lab1_1_HelloWorld
  /// <summary>
  /// Say hello.
  /// Explain the development environment and the Revit.ini information creating the 
  /// link between Revit and the external command. Also, how the external command
  /// can be hooked up with a custom user interface by an external application.
  /// </summary>
  public class Lab1_1_HelloWorld : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      LabUtils.InfoMsg( "Hello World" );
      return IExternalCommand.Result.Failed;
    }
  }
  #endregion // Lab1_1_HelloWorld

  #region Lab1_2_CommandArguments
  /// <summary>
  /// Test contents and usage of Execute() arguments.
  /// </summary>
  public class Lab1_2_CommandArguments : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // List the app, doc and view data:
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      View view = commandData.View;
      string s = "Application = " + app.VersionName;
      s += "\r\nVersion = " + app.VersionNumber;
      s += "\r\nDocument path = " + doc.PathName; // empty if not yet saved
      s += "\r\nDocument title = " + doc.Title;
      s += "\r\nView name = " + view.Name;
      LabUtils.InfoMsg( s );

      LanguageType lt = app.Language;
      ProductType pt = app.Product;

      // List the current selection set:
      Selection sel = doc.Selection;
      s = "There are " + sel.Elements.Size + " elements in the selection set:";
      foreach( Element e in sel.Elements )
      {
        string name = ( null == e.Category ) ? e.GetType().Name : e.Category.Name;
        s += "\r\n  " + name + " Id=" + e.Id.Value.ToString();
      }
      LabUtils.InfoMsg( s );

      // Let's pretend that something is wrong with the first element in the selection.
      // We pass a message back to the Revit user and indicate the error result:
      if( !sel.Elements.IsEmpty )
      {
        ElementSetIterator iter = sel.Elements.ForwardIterator();
        iter.MoveNext();
        Element errElem = iter.Current as Element;
        elements.Clear();
        elements.Insert( errElem );
        message = "We pretend something is wrong with this element and pass back this message to user";
        return IExternalCommand.Result.Failed;
      }
      else 
      {
        // we could return failed here as well, actually, 
        // as long as the message string and element set are empty:

        return IExternalCommand.Result.Succeeded; 
      }
    }
  }
  #endregion // Lab1_2_CommandArguments
}
