#region Copyright
//
// (C) Copyright 2009 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
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
//
#endregion // Copyright

#region Namespaces
using System;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
#endregion // Namespaces

#region Description
/// <summary>
/// Revit Family Creation API Lab - 1
///
/// This command defines a minimum family, and creates a column family
/// with a rectangular profile with three types.
///
/// Objective:
/// ----------
///
/// In this lab, we learn the following:
///
///   0. set up family environment
///   1. create a simple solid
///   2. set alignment
///   3. add types
///
/// To test this lab, open a family template "Metric Column.rft", and run a command.
///
/// Context:
/// --------
///
/// In this lab, we will define a simple rectangular profile like the follow sketch:
///
///   3     2
///    +---+
///    |   | d    h = height
///    +---+
///   0     1
///   4  w
///
/// We then create a box-shape solid using extrusion, align each face of the solid to
/// existing reference planes, and define three types with dimensional variations.
///
/// Desclaimer: code in these labs is written for the purpose of learning the Revit family API.
/// In practice, there will be much room for performance and usability improvement.
/// For code readability, minimum error checking.
/// </summary>
#endregion // Description

namespace LabsCs
{
  class RvtCmd_FamilyCreateColumnRectangle : IExternalCommand
  {
    // member variables for top level access to the Revit database
    //
    Application _rvtApp;
    Document _rvtDoc;

    // command main
    //
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // objects for the top level access
      //
      _rvtApp = commandData.Application;
      _rvtDoc = _rvtApp.ActiveDocument;

      // (0) This command works in the context of family editor only.
      //     We also check if the template is for an appropriate category if needed.
      //     Here we use a Column(i.e., Metric Column.rft) template.
      //     Although there is no specific checking about metric or imperial, our lab only works in metric for now.
      //
      if( !TemplateCategoryEquals( BuiltInCategory.OST_Columns ) )
      {
        Util.ErrorMsg( "Please open Metric Column.rft" );
        return IExternalCommand.Result.Failed;
      }

      // (1) create a simple extrusion. just a simple box for now.
      Extrusion pSolid = createSolid();

      // try this:
      // if you comment addAlignment and addTypes calls below and execute only up to here,
      // you will see the column's top will not follow the upper level.

      // (2) add alignment
      addAlignments( pSolid );

      // try this: at each stage of adding a function here, you should be able to see the result in UI.

      // (3) add types
      addTypes();

      // finally, return
      return IExternalCommand.Result.Succeeded;
    }

    // ============================================
    //   (0) check if we have a correct template
    // ============================================
    bool TemplateCategoryEquals( BuiltInCategory targetCategory )
    {
      // This command works in the context of family editor only.
      //
      if( !_rvtDoc.IsFamilyDocument )
      {
        Util.ErrorMsg( "This command works only in the family editor." );
        return false;
      }

      // Check the template for an appropriate category here if needed.
      //
      Category cat = _rvtDoc.Settings.Categories.get_Item( targetCategory );
      if( _rvtDoc.OwnerFamily == null )
      {
        Util.ErrorMsg( "This command only works in the family context." );
        return false;
      }
      if( !cat.Id.Equals( _rvtDoc.OwnerFamily.FamilyCategory.Id ) )
      {
        Util.ErrorMsg( "Category of this family document does not match the context required by this command." );
        return false;
      }

      // if we come here, we should have a right one.
      return true;
    }

    // ==========================================
    //   (1) create a simple solid by extrusion
    // ==========================================
    Extrusion createSolid()
    {
      //
      // (1) define a simple rectangular profile
      //
      //  3     2
      //   +---+
      //   |   | d    h = height
      //   +---+
      //  0     1
      //  4  w
      //
      CurveArrArray pProfile = createProfileRectangle();
      //
      // (2) create a sketch plane
      //
      // we need to know the template. If you look at the template (Metric Column.rft) and "Front" view,
      // you will see "Reference Plane" at "Lower Ref. Level". We are going to create an extrusion there.
      // findElement() is a helper function that find an element of the given type and name.  see below.
      //
      ReferencePlane pRefPlane = findElement( typeof( ReferencePlane ), "Reference Plane" ) as ReferencePlane;
      SketchPlane pSketchPlane = _rvtDoc.FamilyCreate.NewSketchPlane( pRefPlane.Plane );

      // (3) height of the extrusion
      //
      // once again, you will need to know your template. unlike UI, the alightment will not adjust the geometry.
      // You will need to have the exact location in order to set alignment.
      // Here we hard code for simplicity. 4000 is the distance between Lower and Upper Ref. Level.
      // as an exercise, try changing those values and see how it behaves.
      //
      double dHeight = mmToFeet( 4000.0 );

      // (4) create an extrusion here. at this point. just an box, nothing else.
      //
      bool bIsSolid = true;
      return _rvtDoc.FamilyCreate.NewExtrusion( bIsSolid, pProfile, pSketchPlane, dHeight );
    }

    // =============================================
    //   (1.1) create a simple rectangular profile
    // =============================================
    CurveArrArray createProfileRectangle()
    {
      //
      // define a simple rectangular profile
      //
      //  3     2
      //   +---+
      //   |   | d    h = height
      //   +---+
      //  0     1
      //  4  w
      //

      // sizes (hard coded for simplicity)
      // note: these need to match reference plane. otherwise, alignment won't work.
      // as an exercise, try changing those values and see how it behaves.
      //
      double w = mmToFeet(600.0);
      double d = mmToFeet(600.0);

      // define vertices
      //
      const int nVerts = 4; // the number of vertices

      XYZ[] pts = new XYZ[] {
        new XYZ(-w / 2.0, -d / 2.0, 0.0),
        new XYZ(w / 2.0, -d / 2.0, 0.0),
        new XYZ(w / 2.0, d / 2.0, 0.0),
        new XYZ(-w / 2.0, d / 2.0, 0.0),
        new XYZ(-w / 2.0, -d / 2.0, 0.0) };

      // define a loop. define individual edges and put them in a curveArray
      //
      CurveArray pLoop = _rvtApp.Create.NewCurveArray();
      for( int i = 0; i < nVerts; ++i )
      {
        Line line = _rvtApp.Create.NewLineBound( pts[i], pts[i + 1] );
        pLoop.Append( line );
      }

      // then, put the loop in the curveArrArray as a profile
      //
      CurveArrArray pProfile = _rvtApp.Create.NewCurveArrArray();
      pProfile.Append( pLoop );
      // if we come here, we have a profile now.

      return pProfile;
    }

    // ======================================
    //   (2) add alignments
    // ======================================
    void addAlignments( Extrusion pBox )
    {
      //
      // (1) we want to constrain the upper face of the column to the "Upper Ref Level"
      //

      // which direction are we looking at?
      //
      View pView = findElement( typeof( View ), "Front" ) as View;

      // find the upper ref level
      // findElement() is a helper function. see below.
      //
      Level upperLevel = findElement( typeof( Level ), "Upper Ref Level" ) as Level;
      Reference ref1 = upperLevel.PlaneReference;

      // find the face of the box
      // findFace() is a helper function. see below.
      //
      PlanarFace upperFace = findFace( pBox, new XYZ( 0.0, 0.0, 1.0 ) ); // find a face whose normal is z-up.
      Reference ref2 = upperFace.Reference;

      // create alignments
      //
      _rvtDoc.FamilyCreate.NewAlignment( pView, ref1, ref2 );

      //
      // (2) do the same for the lower level
      //

      // find the lower ref level
      // findElement() is a helper function. see below.
      //
      Level lowerLevel = findElement( typeof( Level ), "Lower Ref. Level" ) as Level;
      Reference ref3 = lowerLevel.PlaneReference;

      // find the face of the box
      // findFace() is a helper function. see below.
      PlanarFace lowerFace = findFace( pBox, new XYZ(0.0, 0.0, -1.0) ); // find a face whose normal is z-down.
      Reference ref4 = lowerFace.Reference;

      // create alignments
      //
      _rvtDoc.FamilyCreate.NewAlignment( pView, ref3, ref4 );

      //
      // (3)  same idea for the Right/Left/Front/Back
      //
      // get the plan view
      // note: same name maybe used for different view types. either one should work.
      View pViewPlan = findElement( typeof( ViewPlan ), "Lower Ref. Level" ) as View;

      // find reference planes
      ReferencePlane refRight = findElement( typeof( ReferencePlane ), "Right" ) as ReferencePlane;
      ReferencePlane refLeft = findElement( typeof( ReferencePlane ), "Left" ) as ReferencePlane;
      ReferencePlane refFront = findElement( typeof( ReferencePlane ), "Front" ) as ReferencePlane;
      ReferencePlane refBack = findElement( typeof( ReferencePlane ), "Back" ) as ReferencePlane;

      // find the face of the box
      PlanarFace faceRight = findFace( pBox, new XYZ( 1.0, 0.0, 0.0 ) );
      PlanarFace faceLeft = findFace( pBox, new XYZ( -1.0, 0.0, 0.0 ) );
      PlanarFace faceFront = findFace( pBox, new XYZ( 0.0, -1.0, 0.0 ) );
      PlanarFace faceBack = findFace( pBox, new XYZ( 0.0, 1.0, 0.0 ) );

      // create alignments
      //
      _rvtDoc.FamilyCreate.NewAlignment( pViewPlan, refRight.Reference, faceRight.Reference );
      _rvtDoc.FamilyCreate.NewAlignment( pViewPlan, refLeft.Reference, faceLeft.Reference );
      _rvtDoc.FamilyCreate.NewAlignment( pViewPlan, refFront.Reference, faceFront.Reference );
      _rvtDoc.FamilyCreate.NewAlignment( pViewPlan, refBack.Reference, faceBack.Reference );
    }

    // ======================================
    //   (3) add types
    // ======================================
    void addTypes()
    {
      // addType(name, Width, Depth)
      //
      addType( "600x900", 600.0, 900.0 );
      addType( "1000x300", 1000.0, 300.0 );
      addType( "600x600", 600.0, 600.0 );
    }

    // add one type
    //
    void addType( string name, double w, double d )
    {
      // get the family manager from the current doc
      FamilyManager pFamilyMgr = _rvtDoc.FamilyManager;

      // add new types with the given name
      //
      FamilyType type1 = pFamilyMgr.NewType( name );

      // look for 'Width' and 'Depth' parameters and set them to the given value
      //
      // first 'Width'
      //
      FamilyParameter paramW = pFamilyMgr.get_Parameter( "Width" );
      double valW = mmToFeet( w );
      if ( paramW != null )
      {
        pFamilyMgr.Set( paramW, valW );
      }

      // same idea for 'Depth'
      //
      FamilyParameter paramD = pFamilyMgr.get_Parameter( "Depth" );
      double valD = mmToFeet( d );
      if ( paramD != null )
      {
        pFamilyMgr.Set( paramD, valD );
      }
    }

#region Helper Functions

    // =============================================================
    //   helper function: find a planar face with the given normal
    // =============================================================
    PlanarFace findFace(Extrusion pBox, XYZ normal)
    {
      // get the geometry object of the given element
      //
      Options op = new Options();
      op.ComputeReferences = true;
      GeometryObjectArray geomObjs = pBox.get_Geometry(op).Objects;

      // loop through the array and find a face with the given normal
      //
      foreach( GeometryObject geomObj in geomObjs )
      {
        if( geomObj is Solid ) // solid is what we are interested in.
        {
          Solid pSolid = geomObj as Solid;
          FaceArray faces = pSolid.Faces;
          foreach( Face pFace in faces )
          {
            PlanarFace pPlanarFace = pFace as PlanarFace;
            if( (pPlanarFace != null) && pPlanarFace.Normal.AlmostEqual( normal ) ) // we found the face
            {
              return pPlanarFace;
            }
          }
        }

        // will come back later as needed.
        //
        //else if (geomObj is Instance)
        //{
        //}
        //else if (geomObj is Curve)
        //{
        //}
        //else if (geomObj is Mesh)
        //{
        //}
      }

      // if we come here, we did not find any.
      return null;
    }

    // ==================================================================================
    //   helper function: find an element of the given type and the name.
    //   You can use this, for example, to find Reference or Level with the given name.
    // ==================================================================================
    RvtElement findElement(Type targetType, string targetName)
    {
      // get the elements of the given type
      //
      ElementArray elems = new ElementArray();
      int n = _rvtDoc.get_Elements( targetType, elems );

      // parse the collection for the given name
      //
      foreach( RvtElement elem in elems )
      {
        if( elem.Name.Equals( targetName ) ) // we found it. return this.
        {
          return elem;
        }
      }

      // cannot find it.
      return null;
    }

    // ===============================================
    //   helper function: convert millimeter to feet
    // ===============================================
    double mmToFeet( double mmVal )
    {
      return mmVal / 304.8;
    }

#endregion // Helper Functions

  }
}
