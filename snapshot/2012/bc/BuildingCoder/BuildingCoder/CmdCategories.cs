#region Header
//
// CmdCategories.cs - list document and built-in categories
//
// Copyright (C) 2010-2011 by Jeremy Tammik, Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace BuildingCoder
{
  [Transaction( TransactionMode.ReadOnly )]
  class CmdCategories : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;
      Categories categories = doc.Settings.Categories;

      int n = categories.Size;

      Debug.Print( "{0} categories and their parents:", n );

      foreach( Category c in categories )
      {
        Category p = c.Parent;

        Debug.Print( "  {0} ({1}), parent {2}",
          c.Name, c.Id.IntegerValue,
          (null == p ? "<none>" : p.Name) );
      }

      Array bics = Enum.GetValues(
        typeof( BuiltInCategory ) );

      n = bics.Length;

      Debug.Print( "{0} built-in categories and the "
        + "corresponding document ones:", n );

      Category cat;
      string s;

      foreach( BuiltInCategory bic in bics )
      {
        try
        {
          cat = categories.get_Item( bic );

          s = (null == cat)
            ? "<none>"
            : string.Format( "--> {0} ({1})",
              cat.Name, cat.Id.IntegerValue );
        }
        catch( Exception ex )
        {
          s = ex.GetType().Name + " " + ex.Message;
        }
        Debug.Print( "  {0} {1}", bic.ToString(), s );
      }
      return Result.Succeeded;
    }
  }
}
