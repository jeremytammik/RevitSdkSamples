#region Header
//
// CmdImportsInFamilies.cs - list all families
// used in current project containing imported
// CAD files.
//
// Copyright (C) 2009-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdImportsInFamilies : IExternalCommand
  {
    #region First version to list import instances non-recursively
    /// <summary>
    /// Non-recursively list all import instances
    /// in all families used in the current project document.
    /// </summary>
    public CmdResult ExecuteWithoutRecursion(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements)
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      List<Element> instances = new List<Element>();
      doc.get_Elements( typeof( FamilyInstance ),
        instances );

      Dictionary<string, Family> families
        = new Dictionary<string, Family>();

      foreach ( FamilyInstance i in instances )
      {
        Family family = i.Symbol.Family;
        if ( !families.ContainsKey( family.Name ) )
        {
          families[family.Name] = family;
        }
      }

      List<string> keys = new List<string>(
        families.Keys );

      keys.Sort();

      foreach ( string key in keys )
      {
        Family family = families[key];
        if ( family.IsInPlace )
        {
          Debug.Print( "Family '{0}' is in-place.", key );
        }
        else
        {
          Document fdoc = doc.EditFamily( family );

          List<Element> imports = new List<Element>();
          fdoc.get_Elements( typeof( ImportInstance ),
            imports );

          int n = imports.Count;

          Debug.Print(
            "Family '{0}' contains {1} import instance{2}{3}",
            key, n, Util.PluralSuffix( n ),
            Util.DotOrColon( n ) );

          if ( 0 < n )
          {
            foreach ( ImportInstance i in imports )
            {
              Debug.Print( "  '{0}'", i.ObjectType.Name );
            }
          }
        }
      }
      return CmdResult.Failed;
    }
    #endregion // First version to list import instances non-recursively

    /// <summary>
    /// Retrieve all families used by the family instances
    /// and annotation symbols in the given document.
    /// Return a dictionary mapping the family name
    /// to the corresponding family object.
    /// </summary>
    Dictionary<string, Family> GetFamilies( Document doc )
    {
      Dictionary<string, Family> families
        = new Dictionary<string, Family>();

      //
      // collect all family instances and determine their families:
      //
      List<Element> instances = new List<Element>();
      doc.get_Elements( typeof( FamilyInstance ),
        instances );

      foreach ( FamilyInstance i in instances )
      {
        Family family = i.Symbol.Family;
        if ( !families.ContainsKey( family.Name ) )
        {
          families[family.Name] = family;
        }
      }

      //
      // collect all annotation symbols and determine their families:
      //
      List<Element> annotations = new List<Element>();
      doc.get_Elements( typeof( AnnotationSymbol ),
        annotations );

      foreach ( AnnotationSymbol a in annotations )
      {
        Family family = a.AsFamilyInstance.Symbol.Family;
        if ( !families.ContainsKey( family.Name ) )
        {
          families[family.Name] = family;
        }
      }
      return families;
    }

    /// <summary>
    /// List all import instances in all the given families.
    /// Retrieve nested families and recursively search in these as well.
    /// </summary>
    void ListImportsAndSearchForMore(
      int recursionLevel,
      Document doc,
      Dictionary<string, Family> families )
    {
      string indent
        = new string( ' ', 2 * recursionLevel );

      List<string> keys = new List<string>(
        families.Keys );

      keys.Sort();

      foreach ( string key in keys )
      {
        Family family = families[key];

        if ( family.IsInPlace )
        {
          Debug.Print( indent
            + "Family '{0}' is in-place.",
            key );
        }
        else
        {
          Document fdoc = doc.EditFamily( family );

          List<Element> imports = new List<Element>();
          fdoc.get_Elements( typeof( ImportInstance ),
            imports );

          int n = imports.Count;

          Debug.Print( indent
            + "Family '{0}' contains {1} import instance{2}{3}",
            key, n, Util.PluralSuffix( n ),
            Util.DotOrColon( n ) );

          if ( 0 < n )
          {
            foreach ( ImportInstance i in imports )
            {
              string s = i.Pinned ? "" : "not ";

              Debug.Print( indent
                + "  '{0}' {1}pinned",
                i.ObjectType.Name, s );

              i.Pinned = !i.Pinned;
            }
          }

          Dictionary<string, Family> nestedFamilies
            = GetFamilies( fdoc );

          ListImportsAndSearchForMore(
            recursionLevel + 1, fdoc, nestedFamilies );
        }
      }
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      Dictionary<string, Family> families
        = GetFamilies( doc );

      ListImportsAndSearchForMore( 0, doc, families );

      return CmdResult.Failed;
    }
  }
}
