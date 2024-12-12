#region Header
//
// CmdMepElementShape.cs - determine element shape, i.e. MEP element cross section
//
// Copyright (C) 2011-2013 by Jeremy Tammik, Autodesk Inc. All rights reserved.
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
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
#endregion // Namespaces

namespace BuildingCoder
{
  [Transaction( TransactionMode.ReadOnly )]
  class CmdMepElementShape : IExternalCommand
  {
    static class MepElementShapeV1
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
      public static string GetElementShape( Element e )
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
    }

    static class MepElementShape
    {
      /// <summary>
      /// Determine element shape from its connectors.
      /// </summary>
      /// <param name="e">Checked element</param>
      /// <param name="pe">Previous element (optional), in case badly-connected MEP system</param>
      /// <param name="ne">Next element (optional), in case you want shape chenge through flow direction only
      /// (for elements with more than one output)</param>
      /// <returns>Element shape changes</returns>
static public string GetElementShape(
  Element e,
  Element pe = null,
  Element ne = null )
{
  if( is_element_of_category( e,
    BuiltInCategory.OST_DuctCurves ) )
  {
    // assuming that transition is using to change shape..

    ConnectorManager cm = ( e as MEPCurve )
      .ConnectorManager;

    foreach( Connector c in cm.Connectors )
      return c.Shape.ToString()
        + " 2 " + c.Shape.ToString();
  }
  else if( is_element_of_category( e,
    BuiltInCategory.OST_DuctFitting ) )
  {
    MEPSystem system
      = ExtractMechanicalOrPipingSystem( e );

    FamilyInstance fi = e as FamilyInstance;
    MEPModel mm = fi.MEPModel;

    ConnectorSet connectors
      = mm.ConnectorManager.Connectors;

    if( fi != null && mm is MechanicalFitting )
    {
      PartType partType
        = ( mm as MechanicalFitting ).PartType;

      if( PartType.Elbow == partType )
      {
        // assuming that transition is using to change shape..

        foreach( Connector c in connectors )
        {
          return c.Shape.ToString()
            + " 2 " + c.Shape.ToString();
        }
      }
      else if( PartType.Transition == partType )
      {
        string[] tmp = new string[2];

        if( system != null )
        {
          foreach( Connector c in connectors )
          {
            if( c.Direction == FlowDirectionType.In )
              tmp[0] = c.Shape.ToString();

            if( c.Direction == FlowDirectionType.Out )
              tmp[1] = c.Shape.ToString();
          }
          return string.Join( " 2 ", tmp );
        }
        else
        {
          int i = 0;

          foreach( Connector c in connectors )
          {
            if( pe != null )
            {
              if( is_connected_to( c, pe ) )
                tmp[0] = c.Shape.ToString();
              else
                tmp[1] = c.Shape.ToString();
            }
            else
            {
              tmp[i] = c.Shape.ToString();
            }
            ++i;
          }

          if( pe != null )
            return string.Join( " 2 ", tmp );

          return string.Join( "-", tmp );
        }
      }
      else if( PartType.Tee == partType
        || PartType.Cross == partType
        || PartType.Pants == partType
        || PartType.Wye == partType )
      {
        string from, to;
        from = to = null;
        List<string> unk = new List<string>();

        if( system != null )
        {
          foreach( Connector c in connectors )
          {
            if( c.Direction == FlowDirectionType.In )
              from = c.Shape.ToString();
            else
              unk.Add( c.Shape.ToString() );

            if( ne != null && is_connected_to( c, ne ) )
              to = c.Shape.ToString();
          }

          if( to != null )
            return from + " 2 " + to;

          return from + " 2 " + string.Join( "-",
            unk.ToArray() );
        }
        else
        {
          foreach( Connector c in connectors )
          {
            if( ne != null && is_connected_to(
              c, ne ) )
            {
              to = c.Shape.ToString();
              continue;
            }

            if( pe != null && is_connected_to(
              c, pe ) )
            {
              from = c.Shape.ToString();
              continue;
            }

            unk.Add( c.Shape.ToString() );
          }

          if( to != null )
            return from + " 2 " + to;

          if( from != null )
            return from + " 2 "
              + string.Join( "-", unk.ToArray() );

          return string.Join( "-", unk.ToArray() );
        }
      }
    }
  }
  return "unknown";
}

      /// <summary>
      /// Check if connector is connected to some connector of the element.
      /// </summary>
      static public bool is_connected_to(
        Connector c,
        Element e )
      {
        ConnectorManager cm = ( e is FamilyInstance )
          ? ( e as FamilyInstance ).MEPModel.ConnectorManager
          : ( e as MEPCurve ).ConnectorManager;

        foreach( Connector c2 in cm.Connectors )
        {
          if( c.IsConnectedTo( c2 ) )
            return true;
        }
        return false;
      }

      /// <summary>
      /// Check if element belongs to the category.
      /// </summary>
      static public bool is_element_of_category(
        Element e,
        BuiltInCategory c )
      {
        //return e.Category.Id.Equals(
        //  e.Document.Settings.Categories.get_Item(
        //    c ).Id );

        return e.Category.Id.IntegerValue.Equals(
          (int) c );
      }

      // copied from sdk - TraverseSystem example
      //
      // (C) Copyright 2003-2010 by Autodesk, Inc.
      //
      /// <summary>
      /// Get the mechanical or piping system
      /// from selected element
      /// </summary>
      /// <param name="selectedElement">Selected element</param>
      /// <returns>The extracted mechanical or piping system,
      /// or null if no expected system is found.</returns>
      static public MEPSystem ExtractMechanicalOrPipingSystem(
        Element selectedElement )
      {
        MEPSystem system = null;

        if( selectedElement is MEPSystem )
        {
          if( selectedElement is MechanicalSystem
            || selectedElement is PipingSystem )
          {
            system = selectedElement as MEPSystem;
            return system;
          }
        }
        else // Selected element is not a system
        {
          FamilyInstance fi = selectedElement
            as FamilyInstance;

          // If selected element is a family instance,
          // iterate its connectors and get the expected system

          if( fi != null )
          {
            MEPModel mepModel = fi.MEPModel;
            ConnectorSet connectors = null;
            try
            {
              connectors = mepModel.ConnectorManager.Connectors;
            }
            catch( System.Exception )
            {
              system = null;
            }
            system = ExtractSystemFromConnectors( connectors );
          }
          else
          {
            // If selected element is a MEPCurve (e.g. pipe or duct),
            // iterate its connectors and get the expected system

            MEPCurve mepCurve = selectedElement as MEPCurve;
            if( mepCurve != null )
            {
              ConnectorSet connectors = null;
              connectors = mepCurve.ConnectorManager.Connectors;
              system = ExtractSystemFromConnectors( connectors );
            }
          }
        }
        return system;
      }

      // copied from sdk - TraverseSystem example
      //
      // (C) Copyright 2003-2010 by Autodesk, Inc.
      //
      /// <summary>
      /// Get the mechanical or piping system
      /// from the connectors of selected element
      /// </summary>
      /// <param name="connectors">Connectors of selected element</param>
      /// <returns>The found mechanical or piping system</returns>
      static public MEPSystem ExtractSystemFromConnectors( ConnectorSet connectors )
      {
        MEPSystem system = null;

        if( connectors == null || connectors.Size == 0 )
        {
          return null;
        }

        // Get well-connected mechanical or
        // piping systems from each connector

        List<MEPSystem> systems = new List<MEPSystem>();
        foreach( Connector connector in connectors )
        {
          MEPSystem tmpSystem = connector.MEPSystem;
          if( tmpSystem == null )
          {
            continue;
          }

          MechanicalSystem ms = tmpSystem as MechanicalSystem;
          if( ms != null )
          {
            if( ms.IsWellConnected )
            {
              systems.Add( tmpSystem );
            }
          }
          else
          {
            PipingSystem ps = tmpSystem as PipingSystem;
            if( ps != null && ps.IsWellConnected )
            {
              systems.Add( tmpSystem );
            }
          }
        }

        // If more than one system is found,
        // get the system contains the most elements

        int countOfSystem = systems.Count;
        if( countOfSystem != 0 )
        {
          int countOfElements = 0;
          foreach( MEPSystem sys in systems )
          {
            if( sys.Elements.Size > countOfElements )
            {
              system = sys;
              countOfElements = sys.Elements.Size;
            }
          }
        }
        return system;
      }
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
        "{0} is {1} ({2})",
        Util.ElementDescription( e ),
        MepElementShape.GetElementShape( e ),
        MepElementShapeV1.GetElementShape( e ) ) );

      return Result.Succeeded;
    }
  }
}
