#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2008 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
#endregion // Namespaces

namespace RstLabs
{
  #region Lab3_ListSelectedAnalyticalModels
  /// <summary>
  /// Lab 3 - List analytical model for selected structural elements,
  /// or all elements if none are selected.
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  public class Lab3_ListSelectedAnalyticalModels : IExternalCommand
  {
    Document m_doc;
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet error_elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;
      m_doc = doc;
      Categories categories = doc.Settings.Categories;
      Category catStruCols = categories.get_Item( BuiltInCategory.OST_StructuralColumns );
      Category catStruFrmg = categories.get_Item( BuiltInCategory.OST_StructuralFraming );
      Category catStruFndt = categories.get_Item( BuiltInCategory.OST_StructuralFoundation );
      List<Element> elements = new List<Element>();
      if( 0 < app.ActiveUIDocument.Selection.Elements.Size )
      {
        foreach( Element e in app.ActiveUIDocument.Selection.Elements )
        {
          elements.Add( e );
        }
      }
      else
      {
        //
        // Use Revit 2011 FilteredElementCollector to get all system family instances.
        //
        // create filter list to accommodate several filters.
        List<ElementFilter> filterList = new List<ElementFilter>();
        ElementCategoryFilter cfColumn = new ElementCategoryFilter( BuiltInCategory.OST_StructuralColumns );
        ElementCategoryFilter cfFrmg = new ElementCategoryFilter( BuiltInCategory.OST_StructuralFraming );
        ElementCategoryFilter cfStruFndt = new ElementCategoryFilter( BuiltInCategory.OST_StructuralFoundation );
        filterList.Add( cfColumn );
        filterList.Add( cfFrmg );
        filterList.Add( cfStruFndt );
        LogicalOrFilter lOrFilter1 = new LogicalOrFilter( filterList );

        ElementClassFilter cfFamilyInstance = new ElementClassFilter( typeof( FamilyInstance ) );
        LogicalAndFilter lAndFilter1 = new LogicalAndFilter( cfFamilyInstance, lOrFilter1 );

        ElementClassFilter cfFooting = new ElementClassFilter( typeof( ContFooting ) );
        ElementClassFilter cfFloor = new ElementClassFilter( typeof( Floor ) );
        ElementClassFilter cfWall = new ElementClassFilter( typeof( Wall ) );

        List<ElementFilter> filterList2 = new List<ElementFilter>();
        filterList2.Add( cfFooting );
        filterList2.Add( cfFloor );
        filterList2.Add( cfWall );
        LogicalOrFilter lOrFilter2 = new LogicalOrFilter( filterList2 );

        LogicalOrFilter lOrFilterLast = new LogicalOrFilter( lAndFilter1, lOrFilter2 );

        FilteredElementCollector collector = new FilteredElementCollector( app.ActiveUIDocument.Document );
        elements = collector.WherePasses( lOrFilterLast ).ToElements() as List<Element>;
      }
      int count = elements.Count;
      int count2 = 0;
      string msg = null;
      foreach( Element e in elements )
      {
        AnalyticalModel a = e.GetAnalyticalModel();
        if( null != a )
        {
          string cat = ( null != e.Category )
            ? e.Category.Name
            : "element";

          msg = string.Format( "Analytical model for {0} {1}:\r\n", cat, e.Id.IntegerValue );
          GetAnalyticalModelString( ref msg, a );
          RacUtils.InfoMsg( msg );
          ++count2;
        }

        #region Handle each element type individually
#if HANDLE_EACH_ELEMENT_TYPE_INDIVIDUALLY
        if( e is Wall )
        {
          Wall w = e as Wall;
          AnalyticalModel anaWall = w.GetAnalyticalModel();
          if( null != anaWall && 0 < anaWall.GetCurves( AnalyticalCurveType.ActiveCurves ).Count )
          {
            msg = "Analytical model for wall " + w.Id.IntegerValue.ToString() + "\r\n";
            ListAnalyticalModelWall( ref anaWall, ref msg );
            RacUtils.InfoMsg( msg );
            ++count2;
          }
        }
        else if( e is Floor )
        {
          Floor f = e as Floor;
          AnalyticalModel anaFloor = f.GetAnalyticalModel();
          if( null != anaFloor && 0 < anaFloor.GetCurves( AnalyticalCurveType.ActiveCurves ).Count )
          {
            msg = "Analytical model for floor " + f.Id.IntegerValue.ToString() + "\r\n";
            ListAnalyticalModelFloor( ref anaFloor, ref msg );
            RacUtils.InfoMsg( msg );
            ++count2;
          }
        }
        else if( e is ContFooting )
        {
          ContFooting cf = e as ContFooting;
          AnalyticalModel ana3d = cf.GetAnalyticalModel();
          if( null != ana3d && 0 < ana3d.GetCurves( AnalyticalCurveType.ActiveCurves ).Count )
          {
            msg = "Analytical model for continuous footing " + cf.Id.IntegerValue.ToString() + "\r\n";
            ListAnalyticalModelContFooting( ref ana3d, ref msg );
            RacUtils.InfoMsg( msg );
            ++count2;
          }
        }
        else if( e is FamilyInstance )
        {
          FamilyInstance fi = e as FamilyInstance;
          ElementId categoryId = fi.Category.Id;
          if( catStruCols.Id.Equals( categoryId ) ) // Structural Columns
          {
            AnalyticalModel anaFrame = fi.GetAnalyticalModel();
            if( null != anaFrame )
            {
              msg = "Analytical model for structural column " + fi.Id.IntegerValue.ToString() + "\r\n";
              Curve cur = anaFrame.GetCurve();
              ListCurve( ref cur, ref msg );
              ListRigidLinks( ref anaFrame, ref msg );
              ListSupportInfo( anaFrame, ref msg );
              RacUtils.InfoMsg( msg );
              ++count2;
            }
          }
          else if( catStruFrmg.Id.Equals( categoryId ) ) // Structural Framing
          {
            AnalyticalModel anaFrame = fi.GetAnalyticalModel();
            if( null != anaFrame )
            {
              msg = "Analytical model for structural framing "
                + fi.StructuralType.ToString() + " " + fi.Id.IntegerValue.ToString() + "\r\n";
              Curve cur = anaFrame.GetCurve();
              ListCurve( ref cur, ref msg );
              ListRigidLinks( ref anaFrame, ref msg );
              ListSupportInfo( anaFrame, ref msg );
              RacUtils.InfoMsg( msg );
              ++count2;
            }
          }
          else if( catStruFndt.Id.Equals( categoryId ) ) // Structural Foundations
          {
            AnalyticalModel anaLoc = fi.GetAnalyticalModel();
            if( null != anaLoc )
            {
              XYZ p = anaLoc.GetPoint();
              msg = "Analytical model for foundation " + fi.Id.IntegerValue.ToString()
                + "\r\n  Location = " + RacUtils.PointString( p ) + "\r\n";

              ListSupportInfo( anaLoc, ref msg );
              RacUtils.InfoMsg( msg );
              ++count2;
            }
          }
        }
#endif // HANDLE_EACH_ELEMENT_TYPE_INDIVIDUALLY
        #endregion // Handle each element type individually

      }
      return Result.Succeeded;
    }

    public void ListCurve( ref string s, Curve crv )
    {
      // todo: rewrite the newline handling, this is still VB:

      if( crv is Line )
      {
        Line line = crv as Line;
        s += "  LINE: " + RacUtils.PointString( line.get_EndPoint( 0 ) )
          + " ; " + RacUtils.PointString( line.get_EndPoint( 1 ) ) + "\r\n";
      }
      else if( crv is Arc )
      {
        Arc arc = crv as Arc;
        s += "  ARC: " + RacUtils.PointString( arc.get_EndPoint( 0 ) )
          + " ; " + RacUtils.PointString( arc.get_EndPoint( 1 ) )
          + " ; R=" + RacUtils.RealString( arc.Radius ) + "\r\n";
      }
      else // generic parametric curve
      {
        if( crv.IsBound )
        {
          s += "  BOUND CURVE " + crv.GetType().Name + " - Tessellated result:\r\n";
          IList<XYZ> pts = crv.Tessellate();
          foreach( XYZ p in pts )
          {
            s += RacUtils.PointString( p ) + "\r\n";
          }
        }
        else
        {
          s += "  UNBOUND CURVE - unnexpected in an analytical model!\r\n";
        }
      }
    }

    public void ListSupportInfo( ref string s, AnalyticalModel model )
    {
      if( null == model )
      {
        s += "There is no analytical model data with this element.\r\n";
      }
      else
      {
        IList<AnalyticalModelSupport> listSupports = null;
        listSupports = model.GetAnalyticalModelSupports();
        if( listSupports.Count > 0 )
        {
          s += "\r\nSupported = YES";
          s += " by below element(s):";
          foreach( AnalyticalModelSupport supInfo in listSupports )
          {
            ElementId idSupport = supInfo.GetSupportingElement();
            Element elemSupport = m_doc.get_Element( idSupport );
            s += "\r\n  " + supInfo.GetSupportType().ToString()
              + " from element id=" + idSupport.IntegerValue.ToString()
              + " cat=" + elemSupport.Category.Name
              + " priority=" + supInfo.GetPriority().ToString();

          }
          s += "\r\n";
        }
        else
        {
          s += "\r\nSupported = NO\r\n";
        }
      }
    }

    public void ListRigidLinks( ref string s, AnalyticalModel a )
    {
      s += "\r\nRigid Link START = ";
      AnalyticalModelSelector selector = new AnalyticalModelSelector( AnalyticalCurveSelector.StartPoint );
      Curve rigidLinkStart = a.GetRigidLink( selector );
      if( null == rigidLinkStart )
      {
        s += "None\r\n";
      }
      else
      {
        ListCurve( ref s, rigidLinkStart );
      }
      s += "Rigid Link END   = ";

      selector = new AnalyticalModelSelector( AnalyticalCurveSelector.EndPoint );
      Curve rigidLinkEnd = a.GetRigidLink( selector );
      if( null == rigidLinkEnd )
      {
        s += "None\r\n";
      }
      else
      {
        ListCurve( ref s, rigidLinkEnd );
      }
    }

    void GetAnalyticalModelString( ref string s, AnalyticalModel a )
    {
      int n = 0;
      foreach( Curve c in a.GetCurves( AnalyticalCurveType.ActiveCurves ) )
      {
        ListCurve( ref s, c );
        ++n;
      }
      if( 0 == n )
      {
        s += "0 active curves\r\n";
      }
      try
      {
        ListRigidLinks( ref s, a );
      }
      catch( Exception ex )
      {
        s += string.Format( "Rigid links: {0}\r\n", ex.Message );
      }
      ListSupportInfo( ref s, a );
    }

    #region Handle each element type individually
#if HANDLE_EACH_ELEMENT_TYPE_INDIVIDUALLY
    public void GetAnalyticalModelWall( ref AnalyticalModel a, ref string s )
    {
      Curve crvTemp = null;
      foreach( Curve crv in a.GetCurves( AnalyticalCurveType.ActiveCurves ) )
      {
        crvTemp = crv;
        ListCurve( ref crvTemp, ref s );
      }
      ListSupportInfo( a, ref s );
    }

    public void GetAnalyticalModelFloor( ref AnalyticalModel anaFloor, ref string s )
    {
      Curve crvTemp = null;
      foreach( Curve crv in anaFloor.GetCurves( AnalyticalCurveType.ActiveCurves ) )
      {
        crvTemp = crv;
        ListCurve( ref crvTemp, ref s );
      }
      ListSupportInfo( anaFloor, ref s );
    }

    public void GetAnalyticalModelContFooting( ref AnalyticalModel ana3d, ref string s )
    {
      Curve crvTemp = null;
      foreach( Curve crv in ana3d.GetCurves( AnalyticalCurveType.ActiveCurves ) )
      {
        crvTemp = crv;
        ListCurve( ref crvTemp, ref s );
      }
      ListSupportInfo( ana3d, ref s );
    }
#endif // HANDLE_EACH_ELEMENT_TYPE_INDIVIDUALLY
    #endregion // Handle each element type individually

  }
  #endregion // Lab3_ListSelectedAnalyticalModels
}
