#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2008 by Jeremy Tammik, Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  
// AUTODESK, INC. DOES NOT WARRANT THAT THE OPERATION OF THE 
// PROGRAM WILL BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject
// to restrictions set forth in FAR 52.227-19 (Commercial Computer
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
using Autodesk.Revit.Collections; // Map
using Autodesk.Revit.Elements;
using Autodesk.Revit.Enums;
using Autodesk.Revit.Parameters;
#endregion // Namespaces

namespace mep
{
  class Util
  {
    #region Exceptions
    public class ParameterException : Exception
    {
      public ParameterException( string parameterName, string description, Element elem )
        : base( string.Format( "'{0}' parameter not defined for {1} {2}", parameterName, description, elem.Id.Value.ToString() ) )
      {
      }
    }

    public class SpaceParameterException : Exception
    {
      public SpaceParameterException( string parameterName, Space space )
        : base( string.Format( "'{0}' parameter not defined for space {1}", parameterName, space.Number ) )
      {
      }
    }

    public class TerminalParameterException : ParameterException
    {
      public TerminalParameterException( string parameterName, FamilyInstance terminal )
        : base( parameterName, "terminal", terminal )
      {
      }
    }
    #endregion // Exceptions

    #region Formatting Utility
    public static string PluralSuffix( int n )
    {
      return 1 == n ? "" : "s";
    }

    public static string DotOrColon( int n )
    {
      return 0 == n ? "." : ":";
    }

    public static string IdList( IEnumerable elements ) // todo: convert to IEnumberable<Element>
    {
      string s = string.Empty;
      foreach( Element e in elements )
      {
        if( 0 < s.Length )
        {
          s += ", ";
        }
        s += e.Id.Value.ToString();
      }
      return s;
    }

    /// <summary>
    /// Format a real number and return its string representation.
    /// </summary>
    public static string RealString( double a )
    {
      return a.ToString( "0.##" );
    }

    /// <summary>
    /// Return a description string for a given element.
    /// </summary>
    public static string ElementDescription( Element e )
    {
      string description = ( null == e.Category )
        ? e.GetType().Name
        : e.Category.Name;
      if( null != e.Name )
      {
        description += " '" + e.Name + "'";
      }
      return description;
    }

    /// <summary>
    /// Return a description string including element id for a given element.
    /// </summary>
    public static string ElementDescriptionAndId( Element e )
    {
      string description = e.GetType().Name;
      if( null != e.Category )
      {
        description += " " + e.Category.Name;
      }
      string identity = e.Id.Value.ToString();
      if( null != e.Name )
      {
        identity = e.Name + " " + identity;
      }
      return string.Format( "{0} <{1}>", description, identity );
    }

    /// <summary>
    /// Return an element description string for an electrical system browser leaf node.
    /// </summary>
    public static string BrowserDescription( Element e )
    {
      FamilyInstance inst = e as FamilyInstance;
      return ( null == inst ? e.Category.Name : inst.Symbol.Family.Name ) + " " + e.Name;
    }
    #endregion // Formatting Utility

    #region Message
    const string _caption = "Revit MEP API Sample";

    /// <summary>
    /// MessageBox wrapper for informational message.
    /// </summary>
    /// <param name="msg"></param>
    public static void InfoMsg( string msg )
    {
      WinForms.MessageBox.Show( msg, _caption, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information );
    }

    /// <summary>
    /// MessageBox wrapper for error message.
    /// </summary>
    /// <param name="msg"></param>
    public static void ErrorMsg( string msg )
    {
      WinForms.MessageBox.Show( msg, _caption, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error );
    }

    /// <summary>
    /// MessageBox wrapper for question message.
    /// </summary>
    public static bool QuestionMsg( string msg )
    {
      return WinForms.DialogResult.Yes
        == WinForms.MessageBox.Show( msg, _caption, WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question );
    }
    #endregion // Message

    #region Parameter Access
    /// <summary>
    /// Helper to get a specific parameter by name.
    /// </summary>
    static Parameter GetParameterFromName( Element elem, string name )
    {
      foreach( Parameter p in elem.Parameters )
      {
        if( p.Definition.Name == name )
        {
          return p;
        }
      }
      return null;
    }

    public static Definition GetParameterDefinitionFromName( Element elem, string name )
    {
      Parameter p = GetParameterFromName( elem, name );
      return ( null == p ) ? null : p.Definition;
    }

    public static double GetParameterValueFromName( Element elem, string name )
    {
      Parameter p = GetParameterFromName( elem, name );
      if( null == p )
      {
        throw new ParameterException( name, "element", elem );
      }
      return p.AsDouble();
    }

      public static string GetStringParameterValueFromName(Element elem, string name)
      {
          Parameter p = GetParameterFromName(elem, name);
          if (null == p)
          {
              throw new ParameterException(name, "element", elem);
          }
          return p.AsString();
      }

    static void DumpParameters( Element elem )
    {
      foreach( Parameter p in elem.Parameters )
      {
        Debug.WriteLine( p.Definition.ParameterType + " " + p.Definition.Name );
      }
    }

    public static double GetSpaceParameterValue( Space space, BuiltInParameter bip, string name )
    {
      Parameter p = space.get_Parameter( bip );
      if( null == p )
      {
        throw new SpaceParameterException( name, space );
      }
      return p.AsDouble();
    }

    public static Parameter GetSpaceParameter( Space space, string name )
    {
      Parameter p = GetParameterFromName( space, name );
      if( null == p )
      {
        throw new SpaceParameterException( name, space );
      }
      return p;
    }

    public static double GetSpaceParameterValue( Space space, string name )
    {
      Parameter p = GetSpaceParameter( space, name );
      return p.AsDouble();
    }

    public static bool IsSupplyAir( FamilyInstance terminal )
    {
      Parameter p = terminal.get_Parameter( Bip.SystemType );
      if( null == p )
      {
        throw new TerminalParameterException( Bip.SystemType.ToString(), terminal );
      }
      return ParameterValue.SupplyAir == p.AsString();
    }

    public static Parameter GetTerminalFlowParameter( FamilyInstance terminal )
    {
      //
      // the built-in parameter "Flow" is read-only:
      //
      //Parameter p = terminal.get_Parameter( _bipFlow );
      //
      // The parameter we are interested in is not the BuiltInParameter... 
      //
      Definition d = Util.GetParameterDefinitionFromName( terminal, ParameterName.Flow );
      Parameter p = terminal.get_Parameter( d );
      if( null == p )
      {
        throw new Util.TerminalParameterException( ParameterName.Flow, terminal );
      }
      return p;
    }
    #endregion // Parameter Access

    #region HVAC Element Access
    private static ElementSet GetSupplyAirTerminals2008( Application app )
    {
      ElementSet es = app.Create.NewElementSet();
      //using( ProgressForm progressForm = new ProgressForm() )
      {
        //progressForm.Show();
        //progressForm.HideProgressBar();

        Document doc = app.ActiveDocument;
        Category categoryAirTerminal = doc.Settings.Categories.get_Item( BuiltInCategory.OST_DuctTerminal );
        ElementIterator it = doc.get_Elements( typeof( FamilyInstance ) );
        int elementCount = 0;

        while( it.MoveNext() )
        {
          ++elementCount;
          if( 482 == elementCount ) // kludge to avoid crash in MoveNext()
          {
            break;
          }
          ////progressForm.SetText( string.Format( "Finding Supply Air Terminals\n{0} Elements Checked", elementCount ) );

          FamilyInstance terminal = it.Current as FamilyInstance;
          if( null != terminal.Category
            && categoryAirTerminal == terminal.Category
            && Util.IsSupplyAir( terminal ) )
          {
            es.Insert( terminal );
          }
          ////DumpParameters( terminal );
        }
        ////progressForm.Hide();
        ////progressForm.Dispose();
      }
      return es;
    }

    private static ElementSet GetSupplyAirTerminals2009( Application app )
    {
      Document doc = app.ActiveDocument;
      Autodesk.Revit.Creation.Application a = app.Create;
      CategoryFilter categoryFilter = a.Filter.NewCategoryFilter( BuiltInCategory.OST_DuctTerminal );
      ParameterFilter parameterFilter = a.Filter.NewParameterFilter( Bip.SystemType, CriteriaFilterType.Equal, ParameterValue.SupplyAir );
      TypeFilter typeFilter = a.Filter.NewTypeFilter( typeof( FamilyInstance ) );
      LogicAndFilter andFilter = a.Filter.NewLogicAndFilter( typeFilter, categoryFilter );
      LogicAndFilter and2Filter = a.Filter.NewLogicAndFilter( parameterFilter, andFilter );
      ElementIterator it = doc.get_Elements( and2Filter );
      ElementSet es = a.NewElementSet();
      while( it.MoveNext() )
      {
        es.Insert( it.Current as Element );
      }
      return es;
    }

    public static ElementSet GetSupplyAirTerminals( Application app )
    {
      return Const.UseRevitApiFilters
        ? GetSupplyAirTerminals2009( app )
        : GetSupplyAirTerminals2008( app );
    }

    /// <summary>
    /// Call the ElementIterator MoveNext() method.
    /// This is wrapped in a try/catch clause, because in Revit 2009 beta 1 and 3,
    /// MoveNext() can throw an exception if run on an RME model inside RAC.
    /// If this exception is caught, we abort the iteration ... we don't really 
    /// have any other choice at the moment ... cf. 
    /// SPR #147425 [ElementIterator returns null element, MoveNext() crashes].
    /// </summary>
    /// <param name="it">Iterator to move to next item</param>
    /// <returns>The same as MoveNext(), or false if a NullReferenceException exception is thrown</returns>
    public static bool TryMoveNext( ref ElementIterator it )
    {
      try
      {
        return it.MoveNext();
      }
      catch( System.NullReferenceException )
      {
        return false;
      }
    }
    #endregion // HVAC Element Access

    #region Electrical Element Access
    /// <summary>
    /// Retrieve all elements of specified type and category from current document.
    /// </summary>
    /// <param name="a">Return list of elements matching selection filter criteria</param>
    /// <param name="t">Type of elements to select</param>
    /// <param name="bic">Built-in category of elements to select</param>
    /// <param name="app">Revit application</param>
    static void GetTypeAndCategory( 
      List<Element> elements, 
      System.Type t, 
      BuiltInCategory bic, 
      Application app )
    {
      Autodesk.Revit.Creation.Application a = app.Create;
      TypeFilter typeFilter = a.Filter.NewTypeFilter( t );
      CategoryFilter categoryFilter = a.Filter.NewCategoryFilter( bic );
      LogicAndFilter andFilter = a.Filter.NewLogicAndFilter( typeFilter, categoryFilter );
      app.ActiveDocument.get_Elements( andFilter, elements );
    }

    /// <summary>
    /// Retrieve all electrical equipment elements in the current active document,
    /// identified by the built-in category OST_ElectricalEquipment.
    /// </summary>
    public static void GetElectricalEquipment( List<Element> equipment, Application app )
    {
      BuiltInCategory bic = BuiltInCategory.OST_ElectricalEquipment;
      GetTypeAndCategory( equipment, typeof( FamilyInstance ), bic, app );
    }

    /// <summary>
    /// Retrieve all electrical system elements in the current active document.
    /// </summary>
    public static void GetElectricalSystems( List<Element> systems, Application app )
    {
      Document doc = app.ActiveDocument;
      //
      // using full-fledged filters, you can write:
      //
      //Autodesk.Revit.Creation.Application a = app.Create;
      //TypeFilter typeFilter = a.Filter.NewTypeFilter( typeof( ElectricalSystem ) );
      //doc.get_Elements( typeFilter, systems );
      //
      // or, simpler:
      //
      doc.get_Elements( typeof( ElectricalSystem ), systems );
    }

    //public static void GetElectricalTerminals( List<Element> terminals, Application app )
    //{
    //  BuiltInCategory bic = BuiltInCategory.OST_ElectricalEquipment;
    //  BuiltInCategory bic2 = BuiltInCategory.OST_ElectricalFixtures;
    //  BuiltInCategory bic3 = BuiltInCategory.OST_LightingDevices;
    //  BuiltInCategory bic4 = BuiltInCategory.OST_LightingFixtures;
    //  ... this is incomplete ...
    //  GetTypeAndCategory( terminals, typeof( FamilyInstance ), bic, app );
    //}

    /// <summary>
    /// Retrieve all elements in current active document having
    /// a non-empty value for the given parameter.
    /// </summary>
    public static void GetElementsWithParameter( List<Element> elements, BuiltInParameter bip, Application app )
    {
      Autodesk.Revit.Creation.Application a = app.Create;
      Filter f = a.Filter.NewParameterFilter( bip, CriteriaFilterType.NotEqual, "" );
      app.ActiveDocument.get_Elements( f, elements );
    }

    /// <summary>
    /// Retrieve all circuit elements in current active document, which we identify as all
    /// elements having a non-empty RBS_ELEC_CIRCUIT_NUMBER or "Circuit Number" parameter.
    /// </summary>
    public static void GetCircuitElements( List<Element> elements, Application app )
    {
      GetElementsWithParameter( elements, BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER, app );
    }

    /// <summary>
    /// Return the one and only project information element using Revit 2009 filtering
    /// by searching for the "Project Information" category. Only one such element exists.
    /// </summary>
    public static Element GetProjectInfoElem( Application app )
    {
      Filter filterCategory = app.Create.Filter.NewCategoryFilter( BuiltInCategory.OST_ProjectInformation );
      ElementIterator i = app.ActiveDocument.get_Elements( filterCategory );
      i.MoveNext();
      Element e = i.Current as Element;
      Debug.Assert( null != e, "expected valid project information element" );
      Debug.Assert( !i.MoveNext(), "expected one single element to be returned" );
      return e;
    }
    #endregion // Electrical Element Access
  }
}
