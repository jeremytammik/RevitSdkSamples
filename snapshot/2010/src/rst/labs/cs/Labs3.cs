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
using System.Collections;
using System.Collections.Generic;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using RvtElement = Autodesk.Revit.Element;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Utility;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace RstLabs
{
  #region Lab3_ListSelectedAnalyticalModels
  /// <summary>
  /// Lab 3 - List analytical model for selected structural elements,
  /// or all elements if none are selected.
  /// </summary>
  public class Lab3_ListSelectedAnalyticalModels : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      Categories categories = doc.Settings.Categories;
      Category catStruCols = categories.get_Item( BuiltInCategory.OST_StructuralColumns );
      Category catStruFrmg = categories.get_Item( BuiltInCategory.OST_StructuralFraming );
      Category catStruFndt = categories.get_Item( BuiltInCategory.OST_StructuralFoundation );
      List<RvtElement> a = new List<RvtElement>();
      if( 0 < doc.Selection.Elements.Size )
      {
        foreach( RvtElement e in doc.Selection.Elements )
        {
          a.Add( e );
        }
      }
      else
      {
        //
        // use Revit 2009 filtering to retrieve all elements of interest in one go:
        // we are interested in the following elements:
        // standard family instances with category column, framing or foundation,
        // or ContFooting or Floor or Wall elements.
        //
        Filter filterInstance = app.Create.Filter.NewTypeFilter( typeof( FamilyInstance ) );
        Filter fColumns = app.Create.Filter.NewCategoryFilter( catStruCols );
        Filter fFraming = app.Create.Filter.NewCategoryFilter( catStruFrmg );
        Filter fFoundation = app.Create.Filter.NewCategoryFilter( catStruFndt );
        Filter f1 = app.Create.Filter.NewLogicOrFilter( fColumns, fFraming );
        Filter f2 = app.Create.Filter.NewLogicOrFilter( f1, fFoundation );
        Filter f3 = app.Create.Filter.NewLogicAndFilter( f2, filterInstance );
        Filter filterFooting = app.Create.Filter.NewTypeFilter( typeof( ContFooting ) );
        Filter filterFloor = app.Create.Filter.NewTypeFilter( typeof( Floor ) );
        Filter filterWall = app.Create.Filter.NewTypeFilter( typeof( Wall ) );
        Filter f4 = app.Create.Filter.NewLogicOrFilter( f3, filterFooting );
        Filter f5 = app.Create.Filter.NewLogicOrFilter( f3, filterFloor );
        Filter f6 = app.Create.Filter.NewLogicOrFilter( f3, filterWall );
        app.ActiveDocument.get_Elements( f6, a );
      }
      int count = a.Count;
      int count2 = 0;
      string sMsg = null;
      foreach( RvtElement e in a )
      {
        if( e is Wall )
        {
          Wall w = e as Wall;
          AnalyticalModelWall anaWall = w.AnalyticalModel as AnalyticalModelWall;
          if( null != anaWall && 0 < anaWall.Curves.Size )
          {
            sMsg = "Analytical model for wall " + w.Id.Value.ToString() + "\r\n";
            GetAnalyticalModelWall( ref anaWall, ref sMsg );
            RacUtils.InfoMsg( sMsg );
            ++count2;
          }
        }
        else if( e is Floor )
        {
          Floor f = e as Floor;
          AnalyticalModelFloor anaFloor = f.AnalyticalModel as AnalyticalModelFloor;
          if( null != anaFloor && 0 < anaFloor.Curves.Size )
          {
            sMsg = "Analytical model for floor " + f.Id.Value.ToString() + "\r\n";
            GetAnalyticalModelFloor( ref anaFloor, ref sMsg );
            RacUtils.InfoMsg( sMsg );
            ++count2;
          }
        }
        else if( e is ContFooting )
        {
          ContFooting cf = e as ContFooting;
          AnalyticalModel3D ana3d = cf.AnalyticalModel as AnalyticalModel3D;
          if( null != ana3d && 0 < ana3d.Curves.Size )
          {
            sMsg = "Analytical model for continuous footing " + cf.Id.Value.ToString() + "\r\n";
            GetAnalyticalModelContFooting( ref ana3d, ref sMsg );
            RacUtils.InfoMsg( sMsg );
            ++count2;
          }
        }
        else if( e is FamilyInstance )
        {
          FamilyInstance fi = e as FamilyInstance;
          ElementId categoryId = fi.Category.Id;
          if( catStruCols.Id.Equals( categoryId ) ) // Structural Columns
          {
            AnalyticalModelFrame anaFrame = fi.AnalyticalModel as AnalyticalModelFrame;
            if( null != anaFrame )
            {
              sMsg = "Analytical model for structural column " + fi.Id.Value.ToString() + "\r\n";
              Curve cur = anaFrame.Curve;
              ListCurve( ref cur, ref sMsg );
              ListRigidLinks( ref anaFrame, ref sMsg );
              AnalyticalSupportData supportData = anaFrame.SupportData;
              ListSupportInfo( ref supportData, ref sMsg );
              RacUtils.InfoMsg( sMsg );
              ++count2;
            }
          }
          else if( catStruFrmg.Id.Equals( categoryId ) ) // Structural Framing
          {
            AnalyticalModelFrame anaFrame = fi.AnalyticalModel as AnalyticalModelFrame;
            if( null != anaFrame )
            {
              sMsg = "Analytical model for structural framing "
                + fi.StructuralType.ToString() + " " + fi.Id.Value.ToString() + "\r\n";
              Curve cur = anaFrame.Curve;
              ListCurve( ref cur, ref sMsg );
              ListRigidLinks( ref anaFrame, ref sMsg );
              AnalyticalSupportData supportData = anaFrame.SupportData;
              ListSupportInfo( ref supportData, ref sMsg );
              RacUtils.InfoMsg( sMsg );
              ++count2;
            }
          }
          else if( catStruFndt.Id.Equals( categoryId ) ) // Structural Foundations
          {
            AnalyticalModelLocation anaLoc = fi.AnalyticalModel as AnalyticalModelLocation;
            if( null != anaLoc )
            {
              XYZ p = anaLoc.Point;
              sMsg = "Analytical model for foundation " + fi.Id.Value.ToString()
                + "\r\n  Location = " + RacUtils.PointString( p ) + "\r\n";
              AnalyticalSupportData supportData = anaLoc.SupportData;
              ListSupportInfo( ref supportData, ref sMsg );
              RacUtils.InfoMsg( sMsg );
              ++count2;
            }
          }
        }
      }
      return CmdResult.Succeeded;
    }

    public void ListCurve( ref Curve crv, ref string s )
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
          XYZArray pts = crv.Tessellate();
          foreach( XYZ p in pts )
          {
            s += RacUtils.PointString(p) + "\r\n";
          }
        }
        else
        {
          s += "  UNBOUND CURVE - unnexpected in an analytical model!\r\n";
        }
      }
    }

    public void GetAnalyticalModelWall( ref AnalyticalModelWall a, ref string s )
    {
      Curve crvTemp = null;
      foreach( Curve crv in a.Curves )
      {
        crvTemp = crv;
        ListCurve( ref crvTemp, ref s );
      }
      AnalyticalSupportData supportData = a.SupportData;
      ListSupportInfo( ref supportData, ref s );
    }

    public void GetAnalyticalModelFloor( ref AnalyticalModelFloor anaFloor, ref string s )
    {
      Curve crvTemp = null;
      foreach( Curve crv in anaFloor.Curves )
      {
        crvTemp = crv;
        ListCurve( ref crvTemp, ref s );
      }
      AnalyticalSupportData supportData = anaFloor.SupportData;
      ListSupportInfo( ref supportData, ref s );
    }

    public void GetAnalyticalModelContFooting( ref AnalyticalModel3D ana3d, ref string s )
    {
      Curve crvTemp = null;
      foreach( Curve crv in ana3d.Curves )
      {
        crvTemp = crv;
        ListCurve( ref crvTemp, ref s );
      }
      AnalyticalSupportData supportData = ana3d.SupportData;
      ListSupportInfo( ref supportData, ref s );
    }

    public void ListSupportInfo( ref AnalyticalSupportData supData, ref string s )
    {
      if( null == supData )
      {
        s += "There is no support data with this element.\r\n";
      }
      else
      {
        if( supData.Supported )
        {
          s += "\r\nSupported = YES";
          if( !supData.InfoArray.IsEmpty )
          {
            s += " by:";
            foreach( AnalyticalSupportInfo supInfo in supData.InfoArray )
            {
              RvtElement supEl = supInfo.Element;
              s += "\r\n  " + supInfo.SupportType.ToString()
                + " from element id=" + supEl.Id.Value.ToString()
                + " cat=" + supEl.Category.Name;
            }
          }
          s += "\r\n";
        }
        else
        {
          s += "\r\nSupported = NO\r\n";
        }
      }
    }

    public void ListRigidLinks( ref AnalyticalModelFrame anaFrm, ref string s )
    {
      s += "\r\nRigid Link START = ";
      Curve rigidLinkStart = anaFrm.get_RigidLink( 0 );
      if( null == rigidLinkStart )
      {
        s += "None\r\n";
      }
      else
      {
        ListCurve( ref rigidLinkStart, ref s );
      }
      s += "Rigid Link END   = ";
      Curve rigidLinkEnd = anaFrm.get_RigidLink( 1 );
      if( null == rigidLinkEnd )
      {
        s += "None\r\n";
      }
      else
      {
        ListCurve( ref rigidLinkEnd, ref s );
      }
    }
  }
  #endregion // Lab3_ListSelectedAnalyticalModels
}
