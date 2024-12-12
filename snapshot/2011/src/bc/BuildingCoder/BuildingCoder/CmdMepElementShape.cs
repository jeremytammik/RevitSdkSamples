#region Header
//
// CmdMepElementShape.cs - determine element shape, i.e. MEP element cross section
//
// Copyright (C) 2011 by Jeremy Tammik, Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
#endregion // Namespaces

namespace BuildingCoder
{
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  class CmdMepElementShape : IExternalCommand
  {
    /// <summary>
    /// Helper class to cache compiled regular expressions.
    /// </summary>
    class RegexCache : Dictionary<string, Regex>
    {
      /// <summary>
      /// Apply regular expression pattern matching 
      /// to a given input string. The compiled 
      /// regular expression is cached for efficient 
      /// future reuse.
      /// </summary>
      /// <param name="pattern">Regular expression pattern</param>
      /// <param name="input">Input string</param>
      /// <returns>True if input matches pattern, else false</returns>
      public bool Match( string pattern, string input )
      {
        if( !ContainsKey( pattern ) )
        {
          Add( pattern, new Regex( pattern ) );
        }
        return this[pattern].IsMatch( input );
      }
    }

    static RegexCache _regexCache = new RegexCache();

    static bool is_element_of_category( 
      Element e, 
      BuiltInCategory c )
    {
      //return e.Category.Id.Equals( 
      //  e.Document.Settings.Categories.get_Item( 
      //    c ).Id );

      return e.Category.Id.IntegerValue.Equals( 
        (int) c );
    }

    /// <summary>
    /// Determine element shape from 
    /// its MEP PartType and Size parameter.
    /// @: maciej szlek
    /// kedziormsz@gmail.com
    /// http://maciejszlek.pl
    /// </summary>
    static string GetElementShape( Element e )
    {
      if( is_element_of_category( e, 
        BuiltInCategory.OST_DuctCurves ) )
      {
        // simple case, no need to use regular expression

        string size = e.get_Parameter( "Size" )
          .AsString();

        if( size.Split( 'x' ).Length == 2 )
          return "rectangular";
        else if( size.Split( '/' ).Length == 2 )
          return "oval";
        else
          return "round";
      }
      else if( is_element_of_category( e, 
        BuiltInCategory.OST_DuctFitting ) )
      {
        FamilyInstance fi = e as FamilyInstance;

        if( fi != null && fi.MEPModel is MechanicalFitting )
        {
          string size = e.get_Parameter( "Size" )
            .AsString();

          PartType partType = ( fi.MEPModel as 
            MechanicalFitting ).PartType;

          if( PartType.Elbow == partType
            || PartType.Transition == partType )
          {
            // more complex case

            #region Metric only
#if METRIC_ONLY_BEFORE_REGEX_CACHE
            if( size.Split( 'x' ).Length == 3 ) // could use a regex "[0-9]x[0-9]+-[0-9]+/[0-9]+" but splitting is less costly
              return "rectangular2rectangular";
            else if( size.Split( '/' ).Length == 3 )
              return "oval2oval";
            else if( 
              new Regex( @"[0-9]+x[0-9]+-[0-9]+/[0-9]+" )
                .IsMatch( size ) )
                  return "rectangular2oval";
            else if( 
              new Regex( @"[0-9]+/[0-9]+-[0-9]+x[0-9]+" )
                .IsMatch( size ) )
                  return "oval2rectangular";
            else if( 
              new Regex( @"[0-9]+[^0-9]-[0-9]+x[0-9]+" )
                .IsMatch( size ) )
                  return "round2rectangular";
            else if( 
              new Regex( @"[0-9]+x[0-9]+-[0-9]+[^0-9]" )
                .IsMatch( size ) )
                  return "rectangular2round";
            else if( 
              new Regex( @"[0-9]+[^0-9]-[0-9]+/[0-9]+" )
                .IsMatch( size ) )
                  return "round2oval";
            else if( 
              new Regex( @"[0-9]+/[0-9]+-[0-9]+[^0-9]" )
                .IsMatch( size ) )
                  return "oval2round";
            else if( 
              new Regex( @"[0-9]+[^0-9]-[0-9]+[^0-9]" )
                .IsMatch( size ) )
                  return "round2round";
            else { return "other case"; }
#endif // METRIC_ONLY_BEFORE_REGEX_CACHE

#if METRIC_ONLY
            if( size.Split( 'x' ).Length == 3 ) // could use a regex "[0-9]x[0-9]+-[0-9]+/[0-9]+" but splitting is less costly
              return "rectangular2rectangular";
            else if( size.Split( '/' ).Length == 3 )
              return "oval2oval";
            else if( _regexCache.Match( 
              "[0-9]+x[0-9]+-[0-9]+/[0-9]+", size ) )
                return "rectangular2oval";
            else if( _regexCache.Match( 
              "[0-9]+/[0-9]+-[0-9]+x[0-9]+", size ) )
                return "oval2rectangular";
            else if( _regexCache.Match( 
              "[0-9]+[^0-9]-[0-9]+x[0-9]+", size ) )
                return "round2rectangular";
            else if( _regexCache.Match( 
              "[0-9]+x[0-9]+-[0-9]+[^0-9]", size ) )
                return "rectangular2round";
            else if( _regexCache.Match( 
              "[0-9]+[^0-9]-[0-9]+/[0-9]+", size ) )
                return "round2oval";
            else if( _regexCache.Match( 
              "[0-9]+/[0-9]+-[0-9]+[^0-9]", size ) )
                return "oval2round";
            else if( _regexCache.Match( 
              "[0-9]+[^0-9]-[0-9]+[^0-9]", size ) )
                return "round2round";
#endif // METRIC_ONLY
            #endregion // Metric only

            if( size.Split( 'x' ).Length == 3 ) // or use Regex("[0-9]x[0-9]+-[0-9]+/[0-9]+") but splitting is less costly
              return "rectangular2rectangular";
            else if( size.Split( '/' ).Length == 3 ) // but if in imperial units size is in fractional inches format it has to be replaced by another regular expression
              return "oval2oval";
            else if( _regexCache.Match( 
              "[0-9]+\"?x[0-9]+\"?-[0-9]+\"?/[0-9]+\"?", size ) )
                return "rectangular2oval";
            else if( _regexCache.Match( 
              "[0-9]+\"?/[0-9]+\"?-[0-9]+\"?x[0-9]+\"?", size ) )
                return "oval2rectangular";
            else if( _regexCache.Match( 
              "[0-9]+\"?[^0-9]-[0-9]+\"?x[0-9]+\"?", size ) )
                return "round2rectangular";
            else if( _regexCache.Match( 
              "[0-9]+\"?x[0-9]+\"?-[0-9]+\"?[^0-9]", size ) )
                return "rectangular2round";
            else if( _regexCache.Match( 
              "[0-9]+\"?[^0-9]-[0-9]+\"?/[0-9]+\"?", size ) )
                return "round2oval";
            else if( _regexCache.Match( 
              "[0-9]+\"?/[0-9]+\"?-[0-9]+\"?[^0-9]", size ) )
                return "oval2round";
            else if( _regexCache.Match( 
              "[0-9]+\"?[^0-9]-[0-9]+\"?[^0-9]", size ) )
                return "round2round";
            else { return "other case"; }
          }
          // etc (for other part types)
          else 
          { 
          }
        }
        // etc (for other categories)
        else 
        { 
        }
      }
      return "unknown";
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

      Element e = null;

      try
      {
        e = Util.SelectSingleElementOfType(
         uidoc, typeof( Element ), "an element", true );
      }
      catch( OperationCanceledException )
      {
        message = "No element selected";
        return Result.Failed;
      }

      Util.InfoMsg( string.Format(
        "{0} is {1}",
        Util.ElementDescription( e ),
        GetElementShape( e ) ) );

      return Result.Succeeded;
    }
  }
}
