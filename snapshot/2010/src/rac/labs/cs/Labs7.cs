#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2009 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Enums;

using RvtElement = Autodesk.Revit.Element;
# endregion

namespace Labs
{
  #region Lab7_1_CreateForm
  /// <summary>
  /// Create a loft form using reference points and curve by points.
  /// </summary>
  public class Lab7_1_CreateForm : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements)
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      if( doc.IsFamilyDocument 
        && doc.OwnerFamily.FamilyCategory.Name.Equals( "Mass" ) )
      {
        // Create profiles array
        ReferenceArrayArray ref_ar_ar = new ReferenceArrayArray();

        // Create first profile
        ReferenceArray ref_ar = new ReferenceArray();

        int y = 100;
        int x = 50;
        XYZ ptA = new XYZ(-x, y, 0);
        XYZ ptB = new XYZ(x, y, 0);
        XYZ ptC = new XYZ(0, y + 10, 10);
        CurveByPoints curve = FormUtils.MakeCurve(app, ptA, ptB, ptC);
        ref_ar.Append(curve.GeometryCurve.Reference);
        ref_ar_ar.Append(ref_ar);

        // Create second profile
        ref_ar = new ReferenceArray();

        y = 40;
        ptA = new XYZ(-x, y, 5);
        ptB = new XYZ(x, y, 5);
        ptC = new XYZ(0, y, 25);
        curve = FormUtils.MakeCurve(app, ptA, ptB, ptC);
        ref_ar.Append(curve.GeometryCurve.Reference);
        ref_ar_ar.Append(ref_ar);

        // Create third profile
        ref_ar = new ReferenceArray();

        y = -20;
        ptA = new XYZ(-x, y, 0);
        ptB = new XYZ(x, y, 0);
        ptC = new XYZ(0, y, 15);
        curve = FormUtils.MakeCurve(app, ptA, ptB, ptC);
        ref_ar.Append(curve.GeometryCurve.Reference);
        ref_ar_ar.Append(ref_ar);

        // Create fourth profile
        ref_ar = new ReferenceArray();

        y = -60;
        ptA = new XYZ(-x, y, 0);
        ptB = new XYZ(x, y, 0);
        ptC = new XYZ(0, y + 10, 20);
        curve = FormUtils.MakeCurve(app, ptA, ptB, ptC);
        ref_ar.Append(curve.GeometryCurve.Reference);
        ref_ar_ar.Append(ref_ar);

        Form form = doc.FamilyCreate.NewLoftForm(true, ref_ar_ar);

        return IExternalCommand.Result.Succeeded;
      }
      else
      {
        LabUtils.ErrorMsg( "Please load a conceptual massing family document!" );
        return IExternalCommand.Result.Failed;
      }
    }
  }
  #endregion
  
  # region Lab7_2_CreateDividedSurface
  /// <summary>
  /// Create a divided surface using reference of a face of the form.
  /// </summary>
  public class Lab7_2_CreateDividedSurface : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements)
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      try
      {
        // find forms in the model by filter:
        Filter filterForm = app.Create.Filter.NewCategoryFilter(BuiltInCategory.OST_MassSurface);
        List<RvtElement> forms = new List<RvtElement>();
        doc.get_Elements(filterForm, forms);
        foreach (Form form in forms)
        {
          // Now, lets create the Divided surface on the loft form
          Autodesk.Revit.Creation.FamilyItemFactory fac = doc.FamilyCreate;
          Options options = app.Create.NewGeometryOptions();
          options.ComputeReferences = true;
          options.View = doc.ActiveView;
          Autodesk.Revit.Geometry.Element element = form.get_Geometry(options);

          GeometryObjectArray geoObjectArray = element.Objects;
          //enum the geometry element
          for (int j = 0; j < geoObjectArray.Size; j++)
          {
            GeometryObject geoObject = geoObjectArray.get_Item(j);
            Solid solid = geoObject as Solid;
            foreach (Face face in solid.Faces)
            {
              if (face.Reference != null)
              {
                if (null != face)
                {
                  DividedSurface divSurface = fac.NewDividedSurface(face.Reference);
                }
              }
            }
          }
        }
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return IExternalCommand.Result.Failed;
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion

  # region Lab7_3_ChangeTilePattern
  /// <summary>
  /// Change the tiling pattern of the divided surface using the built-in TilePattern enumeration.
  /// </summary>
  public class Lab7_3_ChangeTilePattern : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements)
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      try
      {
        // find forms in the model by filter:
        Filter filterForm = app.Create.Filter.NewTypeFilter(typeof(Form));
        List<RvtElement> forms = new List<RvtElement>();
        doc.get_Elements(filterForm, forms);
        foreach (Form form in forms)
        {
          // Get access to the divided surface data from the form
          DividedSurfaceData dsData = form.GetDividedSurfaceData();
          if (null != dsData)
          {
            // get the references associated with the divided surfaces
            foreach (Reference reference in dsData.GetReferencesWithDividedSurfaces())
            {
              DividedSurface divSurface = dsData.GetDividedSurfaceForReference(reference);

              int count = 0;
              TilePatterns tilepatterns = doc.Settings.TilePatterns;
              foreach (TilePatternsBuiltIn TilePatternEnum in Enum.GetValues(typeof(TilePatternsBuiltIn)))
              {
                if( count.Equals( 3 ) )
                {
                  divSurface.ObjectType = tilepatterns.GetTilePattern(TilePatternEnum);
                  break;
                }
                count = count + 1;
              }
            }
          }          
        }
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return IExternalCommand.Result.Failed;
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion

  # region Utilities
  /// <summary>
  /// This class is utility class for form creation.
  /// </summary>
  public class FormUtils
  {
    /// <summary>
    /// Create a CurveByPoints element by three given points
    /// </summary>
    public static CurveByPoints MakeCurve(
      Application app, XYZ ptA, XYZ ptB, XYZ ptC )
    {
      Document doc = app.ActiveDocument;
      ReferencePoint refPtA = doc.FamilyCreate.NewReferencePoint(ptA);
      ReferencePoint refPtB = doc.FamilyCreate.NewReferencePoint(ptB);
      ReferencePoint refPtC = doc.FamilyCreate.NewReferencePoint(ptC); 

      ReferencePointArray refPtsArray = new ReferencePointArray();
      refPtsArray.Append(refPtA);
      refPtsArray.Append(refPtB);
      refPtsArray.Append(refPtC);

      return doc.FamilyCreate.NewCurveByPoints( refPtsArray );
    }
  }
  # endregion
}
