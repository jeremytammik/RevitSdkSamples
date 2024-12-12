#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2013 by Jeremy Tammik, Autodesk, Inc.
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinForms = System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
#endregion // Namespaces

namespace AdnRme
{
  class Util
  {
    #region Exceptions
    public class ParameterException : Exception
    {
      public ParameterException( string parameterName, string description, Element elem )
        : base( string.Format( "'{0}' parameter not defined for {1} {2}", parameterName, description, elem.Id.IntegerValue.ToString() ) )
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

    #region Formatting
    public static string PluralSuffix( int n )
    {
      return 1 == n ? "" : "s";
    }

    public static string DotOrColon( int n )
    {
      return 0 == n ? "." : ":";
    }

    public static string IdList( IList<FamilyInstance> elements )
    {
      string s = string.Empty;
      foreach( Element e in elements )
      {
        if( 0 < s.Length )
        {
          s += ", ";
        }
        s += e.Id.IntegerValue.ToString();
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
      string identity = e.Id.IntegerValue.ToString();
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
    #endregion // Formatting

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

#if NEED_IS_SUPPLY_AIR_METHOD
    public static bool IsSupplyAir( FamilyInstance terminal )
    {
      Parameter p = terminal.get_Parameter( Bip.SystemType );
      if( null == p )
      {
        throw new TerminalParameterException( Bip.SystemType.ToString(), terminal );
      }
      bool rc = p.AsString().Equals( ParameterValue.SupplyAir );

#if DEBUG
      ElementId typeId = terminal.GetTypeId();
      ElementType t = terminal.Document.get_Element( typeId ) as ElementType;
      MEPSystemType t2 = terminal.Document.get_Element( typeId ) as MEPSystemType;
      Debug.Assert( (MEPSystemClassification.SupplyAir == t2.SystemClassification) == rc,
        "expected parameter check to return correct system classification" );
#endif // DEBUG

      return rc;
    }
#endif // NEED_IS_SUPPLY_AIR_METHOD

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
    /// <summary>
    /// Retrieve all supply air terminals from given document.
    /// Select all family instance elements of BuiltInCategory 
    /// OST_DuctTerminal with system type equal to suppy air.
    /// </summary>
    public static FilteredElementCollector GetSupplyAirTerminals( Document doc )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfCategory( BuiltInCategory.OST_DuctTerminal );
      collector.OfClass( typeof( FamilyInstance ) );

      //int n1 = collector.ToElements().Count; // 61 in sample model

      // ensure that system type equals supply air:
      //
      // in Revit 2009 and 2010 API, this did it:
      //
      //ParameterFilter parameterFilter = a.Filter.NewParameterFilter( 
      //  Bip.SystemType, CriteriaFilterType.Equal, ParameterValue.SupplyAir );

      // in Revit 2011, create an ElementParameter filter.
      // Create filter by provider and evaluator:

      ParameterValueProvider provider = new ParameterValueProvider( new ElementId( Bip.SystemType ) );
      FilterStringRuleEvaluator evaluator = new FilterStringEquals();
      string ruleString = ParameterValue.SupplyAir;
      FilterRule rule = new FilterStringRule( provider, evaluator, ruleString, false );
      ElementParameterFilter filter = new ElementParameterFilter( rule );

      collector.WherePasses( filter );

      //int n2 = collector.ToElements().Count; // 51 in sample model

      return collector;
    }

    /// <summary>
    /// Retrieve all spaces in given document.
    /// </summary>
    public static List<Space> GetSpaces( Document doc )
    {
      FilteredElementCollector collector 
        = new FilteredElementCollector( doc );

      // trying to collect all spaces directly causes 
      // the following error:
      //
      // Input type is of an element type that exists 
      // in the API, but not in Revit's native object 
      // model. Try using Autodesk.Revit.DB.Enclosure 
      // instead, and then postprocessing the results 
      // to find the elements of interest.
      //
      //collector.OfClass( typeof( Space ) );

      collector.OfClass( typeof( SpatialElement ) );

      //return ( from e in collector.ToElements() // 2011
      //         where e is Space
      //         select e as Space )
      //  .ToList<Space>();

      return collector.ToElements().OfType<Space>().ToList<Space>(); // 2012
    }
    #endregion // HVAC Element Access

    #region Electrical Element Access
    /// <summary>
    /// Return all elements of the requested class i.e. System.Type
    /// matching the given built-in category in the active document.
    /// </summary>
    public static FilteredElementCollector GetElementsOfType(
      Document doc,
      Type type,
      BuiltInCategory bic )
    {
      FilteredElementCollector collector 
        = new FilteredElementCollector( doc );

      collector.OfCategory( bic );
      collector.OfClass( type );

      return collector;
    }

    /// <summary>
    /// Retrieve all electrical equipment elements in the given document,
    /// identified by the built-in category OST_ElectricalEquipment.
    /// </summary>
    public static List<Element> GetElectricalEquipment( 
      Document doc )
    {
      FilteredElementCollector collector 
        = GetElementsOfType( doc, typeof( FamilyInstance ), 
          BuiltInCategory.OST_ElectricalEquipment );

      // return a List instead of IList, because we need the method Exists() on it:

      return new List<Element>( collector.ToElements() );
    }

    /// <summary>
    /// Retrieve all electrical system elements in the given document.
    /// </summary>
    public static IList<Element> GetElectricalSystems( Document doc )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( ElectricalSystem ) );
      return collector.ToElements();
    }

    /// <summary>
    /// Retrieve all circuit elements in current active document, 
    /// which we identify as all family instance or electrical system
    /// elements with a non-empty RBS_ELEC_CIRCUIT_NUMBER or "Circuit Number" 
    /// parameter.
    /// </summary>
    public static IList<Element> GetCircuitElements( Document doc )
    {
      //
      // prepend as many 'fast' filters as possible, because parameter access is 'slow':
      //
      ElementClassFilter f1 = new ElementClassFilter( typeof( FamilyInstance ) );
      ElementClassFilter f2 = new ElementClassFilter( typeof( ElectricalSystem ) );
      LogicalOrFilter f3 = new LogicalOrFilter( f1, f2 );
      FilteredElementCollector collector = new FilteredElementCollector( doc ).WherePasses( f3 );

      BuiltInParameter bip = BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER;

#if DEBUG
      int n1 = collector.ToElements().Count;

      List<Element> a = new List<Element>();
      foreach( Element e in collector )
      {
        Parameter p = e.get_Parameter( BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER );
        if( null != p && 0 < p.AsString().Length )
        {
          a.Add( e );
        }
      }
      int n2 = a.Count;
      Debug.Assert( n1 > n2, "expected filter to eliminate something" );

      List<Element> b = (
        from e in collector.ToElements()
        where ( null != e.get_Parameter( bip ) ) && ( 0 < e.get_Parameter( bip ).AsString().Length )
        select e ).ToList<Element>();

      int n3 = b.Count;
      Debug.Assert( n2 == n3, "expected to reproduce same result" );
#endif // DEBUG

      //
      // this is unclear ... negating the rule that says the parameter 
      // exists and is empty could mean that elements pass if the parameter 
      // is non-empty, but also if the parameter does not exist ...
      // so maybe returning the collection b instead of c would be a safer bet?
      //
      ParameterValueProvider provider = new ParameterValueProvider( new ElementId( bip ) );
      FilterStringRuleEvaluator evaluator = new FilterStringEquals();
      FilterRule rule = new FilterStringRule( provider, evaluator, string.Empty, false );
      ElementParameterFilter filter = new ElementParameterFilter( rule, true );

      collector.WherePasses( filter );
      IList<Element> c = collector.ToElements();
      int n4 = c.Count;
      Debug.Assert( n2 == n4, "expected to reproduce same result" );

      return c;
    }

    /// <summary>
    /// Return the one and only project information element using Revit 2009 filtering
    /// by searching for the "Project Information" category. Only one such element exists.
    /// </summary>
    public static Element GetProjectInfoElem( Document doc )
    {
      //Filter filterCategory = app.Create.Filter.NewCategoryFilter( BuiltInCategory.OST_ProjectInformation );
      //ElementIterator i = app.ActiveDocument.get_Elements( filterCategory );
      //i.MoveNext();
      //Element e = i.Current as Element;

      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfCategory( BuiltInCategory.OST_ProjectInformation );
      Debug.Assert( 1 == collector.ToElements().Count, "expected one single element to be returned" );
      Element e = collector.FirstElement();
      Debug.Assert( null != e, "expected valid project information element" );
      return e;
    }
    #endregion // Electrical Element Access
  }
}
