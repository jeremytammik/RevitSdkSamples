#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2011 by Autodesk, Inc.
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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using FamilyItemFactory = Autodesk.Revit.Creation.FamilyItemFactory;
#endregion

namespace XtraCs
{
  #region Lab7_1_CreateForm
  /// <summary>
  /// Create a loft form using reference points and curve by points.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  public class Lab7_1_CreateForm : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      if( !doc.IsFamilyDocument
        || !doc.OwnerFamily.FamilyCategory.Name.Equals( "Mass" ) )
      {
        message = "Please run this comand in a conceptual massing family document.";
        return Result.Failed;
      }
      FamilyItemFactory creator = doc.FamilyCreate;

      // Create profiles array
      ReferenceArrayArray ref_ar_ar = new ReferenceArrayArray();

      // Create first profile
      ReferenceArray ref_ar = new ReferenceArray();

      int y = 100;
      int x = 50;
      XYZ pa = new XYZ( -x, y, 0 );
      XYZ pb = new XYZ( x, y, 0 );
      XYZ pc = new XYZ( 0, y + 10, 10 );
      CurveByPoints curve = FormUtils.MakeCurve( creator, pa, pb, pc );
      ref_ar.Append( curve.GeometryCurve.Reference );
      ref_ar_ar.Append( ref_ar );

      // Create second profile
      ref_ar = new ReferenceArray();

      y = 40;
      pa = new XYZ( -x, y, 5 );
      pb = new XYZ( x, y, 5 );
      pc = new XYZ( 0, y, 25 );
      curve = FormUtils.MakeCurve( creator, pa, pb, pc );
      ref_ar.Append( curve.GeometryCurve.Reference );
      ref_ar_ar.Append( ref_ar );

      // Create third profile
      ref_ar = new ReferenceArray();

      y = -20;
      pa = new XYZ( -x, y, 0 );
      pb = new XYZ( x, y, 0 );
      pc = new XYZ( 0, y, 15 );
      curve = FormUtils.MakeCurve( creator, pa, pb, pc );
      ref_ar.Append( curve.GeometryCurve.Reference );
      ref_ar_ar.Append( ref_ar );

      // Create fourth profile
      ref_ar = new ReferenceArray();

      y = -60;
      pa = new XYZ( -x, y, 0 );
      pb = new XYZ( x, y, 0 );
      pc = new XYZ( 0, y + 10, 20 );
      curve = FormUtils.MakeCurve( creator, pa, pb, pc );
      ref_ar.Append( curve.GeometryCurve.Reference );
      ref_ar_ar.Append( ref_ar );

      Form form = creator.NewLoftForm( true, ref_ar_ar );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Lab7_2_CreateDividedSurface
  /// <summary>
  /// Create a divided surface using reference of a face of the form.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  public class Lab7_2_CreateDividedSurface : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      Autodesk.Revit.Creation.Application creApp
        = app.Application.Create;

      try
      {
        // find forms in the model:

        FilteredElementCollector forms = new FilteredElementCollector( doc );
        forms.OfCategory( BuiltInCategory.OST_MassForm ); // was OST_MassSurface

        foreach( Form form in forms )
        {
          // create the divided surface on the loft form:

          FamilyItemFactory factory = doc.FamilyCreate;
          Options options = creApp.NewGeometryOptions();
          options.ComputeReferences = true;
          options.View = doc.ActiveView;
          GeometryElement element = form.get_Geometry( options );
          GeometryObjectArray geoObjectArray = element.Objects;

          for( int j = 0; j < geoObjectArray.Size; j++ )
          {
            GeometryObject geoObject = geoObjectArray.get_Item( j );
            Solid solid = geoObject as Solid;
            foreach( Face face in solid.Faces )
            {
              if( face.Reference != null )
              {
                if( null != face )
                {
                  DividedSurface divSurface = factory.NewDividedSurface( face.Reference );
                }
              }
            }
          }
        }
        return Result.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
    }
  }
  #endregion

  #region Lab7_3_ChangeTilePattern
  /// <summary>
  /// Change the tiling pattern of the divided surface using the built-in TilePattern enumeration.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  public class Lab7_3_ChangeTilePattern : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      try
      {
        // find forms in the model:

        FilteredElementCollector forms = new FilteredElementCollector( doc );
        forms.OfClass( typeof( Form ) );

        foreach( Form form in forms )
        {
          // access the divided surface data from the form:

          DividedSurfaceData dsData = form.GetDividedSurfaceData();

          if( null != dsData )
          {
            // get the references associated with the divided surfaces
            foreach( Reference reference in dsData.GetReferencesWithDividedSurfaces() )
            {
              DividedSurface divSurface = dsData.GetDividedSurfaceForReference( reference );

              int count = 0;
              TilePatterns tilepatterns = doc.Settings.TilePatterns;
              foreach( TilePatternsBuiltIn i in Enum.GetValues( typeof( TilePatternsBuiltIn ) ) )
              {
                if( 3 == count )
                {
                  // Warning:	'Autodesk.Revit.DB.Element.ObjectType' is obsolete:
                  // 'Use Element.GetTypeId() and Element.ChangeTypeId() instead.'
                  //
                  //divSurface.ObjectType = tilepatterns.GetTilePattern( i );

                  divSurface.ChangeTypeId( tilepatterns.GetTilePattern( i ).Id );

                  break;
                }
                ++count;
              }
            }
          }
        }
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
      return Result.Succeeded;
    }
  }
  #endregion

  #region Utilities
  /// <summary>
  /// This class is utility class for form creation.
  /// </summary>
  public class FormUtils
  {
    /// <summary>
    /// Create a CurveByPoints element by three given points
    /// </summary>
    public static CurveByPoints MakeCurve(
      FamilyItemFactory creator,
      XYZ pa,
      XYZ pb,
      XYZ pc )
    {
      ReferencePoint rpa = creator.NewReferencePoint( pa );
      ReferencePoint rpb = creator.NewReferencePoint( pb );
      ReferencePoint rpc = creator.NewReferencePoint( pc );

      ReferencePointArray arr = new ReferencePointArray();

      arr.Append( rpa );
      arr.Append( rpb );
      arr.Append( rpc );

      return creator.NewCurveByPoints( arr );
    }
  }
  #endregion
}
