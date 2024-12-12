#region Header
//
// Util.cs - The Building Coder Revit API utility methods
//
// Copyright (C) 2008-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WinForms = System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using RvtElement = Autodesk.Revit.Element;
#endregion // Namespaces

namespace BuildingCoder
{
  class Util
  {
    public static double Max( double[] a )
    {
      Debug.Assert( 1 == a.Rank, "expected one-dimensional array" );
      Debug.Assert( 0 == a.GetLowerBound( 0 ), "expected zero-based array" );
      Debug.Assert( 0 < a.GetUpperBound( 0 ), "expected non-empty array" );
      double max = a[0];
      for( int i = 1; i <= a.GetUpperBound( 0 ); ++i )
      {
        if( max < a[i] )
        {
          max = a[i];
        }
      }
      return max;
    }

    #region Geometrical Comparison
    const double _eps = 1.0e-9;

    public static double Eps
    {
      get
      {
        return _eps;
      }
    }

    public static double MinLineLength
    {
      get
      {
        return _eps;
      }
    }

    public static double TolPointOnPlane
    {
      get
      {
        return _eps;
      }
    }

    public static bool IsZero( double a, double tolerance )
    {
      return tolerance > Math.Abs( a );
    }

    public static bool IsZero( double a )
    {
      return IsZero( a, _eps );
    }

    public static bool IsEqual( double a, double b )
    {
      return IsZero( b - a );
    }

    public static int Compare( double a, double b )
    {
      return IsEqual( a, b ) ? 0 : ( a < b ? -1 : 1 );
    }

    public static int Compare( XYZ p, XYZ q )
    {
      int diff = Compare( p.X, q.X );
      if( 0 == diff ) {
        diff = Compare( p.Y, q.Y );
        if( 0 == diff ) {
          diff = Compare( p.Z, q.Z );
        }
      }
      return diff;
    }

    public static bool IsHorizontal( XYZ v )
    {
      return IsZero( v.Z );
    }

    public static bool IsVertical( XYZ v )
    {
      return IsZero( v.X ) && IsZero( v.Y );
    }

    public static bool IsVertical( XYZ v, double tolerance )
    {
      return IsZero( v.X, tolerance ) 
        && IsZero( v.Y, tolerance );
    }

    public static bool IsHorizontal( Edge e )
    {
      XYZ p = e.Evaluate( 0 );
      XYZ q = e.Evaluate( 1 );
      return IsHorizontal( q - p );
    }

    public static bool IsHorizontal( PlanarFace f )
    {
      return IsVertical( f.Normal );
    }

    public static bool IsVertical( PlanarFace f )
    {
      return IsHorizontal( f.Normal );
    }

    public static bool IsVertical( CylindricalFace f )
    {
      return IsVertical( f.Axis );
    }
    #endregion // Geometrical Comparison

    #region Unit Handling
    const double _convertFootToMm = 12 * 25.4;

    const double _convertFootToMeter
      = _convertFootToMm * 0.001;

    const double _convertCubicFootToCubicMeter
      = _convertFootToMeter
      * _convertFootToMeter
      * _convertFootToMeter;

    /// <summary>
    /// Convert a given length in feet to millimetres.
    /// </summary>
    public static double FootToMm( double length )
    {
      return length * _convertFootToMm;
    }

    /// <summary>
    /// Convert a given length in millimetres to feet.
    /// </summary>
    public static double MmToFoot( double length )
    {
      return length / _convertFootToMm;
    }

    /// <summary>
    /// Convert a given volume in feet to cubic meters.
    /// </summary>
    public static double CubicFootToCubicMeter( double volume )
    {
      return volume * _convertCubicFootToCubicMeter;
    }
    #endregion // Unit Handling

    #region Formatting
    /// <summary>
    /// Return an English plural suffix 's' or
    /// nothing for the given number of items.
    /// </summary>
    public static string PluralSuffix( int n )
    {
      return 1 == n ? "" : "s";
    }

    /// <summary>
    /// Return an English plural suffix 'ies' or
    /// 'y' for the given number of items.
    /// </summary>
    public static string PluralSuffixY( int n )
    {
      return 1 == n ? "y" : "ies";
    }

    public static string DotOrColon( int n )
    {
      return 0 < n ? ":" : ".";
    }

    public static string RealString( double a )
    {
      return a.ToString( "0.##" );
    }

    public static string AngleString( double angle )
    {
      return RealString( angle * 180 / Math.PI ) + " degrees";
    }

    public static string MmString( double length )
    {
      return RealString( FootToMm( length ) ) + " mm";
    }

    public static string PointString( UV p )
    {
      return string.Format( "({0},{1})",
        RealString( p.U ),
        RealString( p.V ) );
    }

    public static string PointString( XYZ p )
    {
      return string.Format( "({0},{1},{2})",
        RealString( p.X ), 
        RealString( p.Y ),
        RealString( p.Z ) );
    }

    public static string PlaneString( Plane p )
    {
      return string.Format( "plane origin {0}, plane normal {1}",
        PointString( p.Origin ),
        PointString( p.Normal ) );
    }

    public static string TransformString( Transform t )
    {
      return string.Format( "({0},{1},{2},{3})", PointString( t.Origin ),
        PointString( t.BasisX ), PointString( t.BasisY ), PointString( t.BasisZ ) );
    }

    public static string PointArrayString( XYZArray pts )
    {
      string s = string.Empty;
      foreach( XYZ p in pts )
      {
        if( 0 < s.Length )
        {
          s += ", ";
        }
        s += PointString( p );
      }
      return s;
    }

    public static string CurveString( Curve curve )
    {
      return "curve tesselation " + PointArrayString( curve.Tessellate() );
    }
    #endregion // Formatting

    const string _caption = "The Building Coder";

    public static void InfoMsg( string msg )
    {
      Debug.WriteLine( msg );
      WinForms.MessageBox.Show( msg,
        _caption,
        WinForms.MessageBoxButtons.OK,
        WinForms.MessageBoxIcon.Information );
    }

    public static void ErrorMsg( string msg )
    {
      Debug.WriteLine( msg );
      WinForms.MessageBox.Show( msg,
        _caption,
        WinForms.MessageBoxButtons.OK,
        WinForms.MessageBoxIcon.Error );
    }

    public static string ElementDescription( RvtElement e )
    {
      if( null == e )
      {
        return "<null>";
      }
      // for a wall, the element name equals the
      // wall type name, which is equivalent to the
      // family name ...
      FamilyInstance fi = e as FamilyInstance;
      string fn = ( null == fi )
        ? string.Empty
        : fi.Symbol.Family.Name + " ";

      string cn = ( null == e.Category )
        ? e.GetType().Name
        : e.Category.Name;

      return string.Format( "{0} {1}<{2} {3}>",
        cn, fn, e.Id.Value, e.Name );
    }

    #region Element Selection
    public static RvtElement SelectSingleElement(
      Document doc,
      string description )
    {
      Selection sel = doc.Selection;
      RvtElement e = null;
      sel.Elements.Clear();
      sel.StatusbarTip = "Please select " + description;
      if( sel.PickOne() )
      {
        ElementSetIterator elemSetItr
          = sel.Elements.ForwardIterator();
        elemSetItr.MoveNext();
        e = elemSetItr.Current as RvtElement;
      }
      return e;
    }

    public static RvtElement GetSingleSelectedElement(
      Document doc )
    {
      RvtElement e = null;
      SelElementSet set = doc.Selection.Elements;

      if ( 1 == set.Size )
      {
        foreach ( RvtElement e2 in set )
        {
          e = e2;
        }
      }
      return e;
    }

    public static RvtElement SelectSingleElementOfType(
      Document doc,
      Type t,
      string description )
    {
      RvtElement e = GetSingleSelectedElement( doc );

      if( null == e || !e.GetType().Equals( t ) ) // IsSubclassOf( t )
      {
        e = Util.SelectSingleElement( doc, description );
      }
      return (null == e) || e.GetType().Equals( t )
        ? e
        : null;
    }

    /// <summary>
    /// Retrieve all pre-selected elements of the specified type,
    /// if any elements at all have been pre-selected. If not,
    /// retrieve all elements of specified type in the database.
    /// </summary>
    /// <param name="a">Return value container</param>
    /// <param name="doc">Active document</param>
    /// <param name="t">Specific type</param>
    /// <returns>True if some elements were retrieved</returns>
    public static bool GetSelectedElementsOrAll(
      List<RvtElement> a,
      Document doc,
      Type t )
    {
      Selection sel = doc.Selection;
      if( 0 < sel.Elements.Size )
      {
        foreach( RvtElement e in sel.Elements )
        {
          if( t.IsInstanceOfType( e ) )
          {
            a.Add( e );
          }
        }
      }
      else
      {
        doc.get_Elements( t, a );
      }
      return 0 < a.Count;
    }
    #endregion // Element Selection
  }
}
