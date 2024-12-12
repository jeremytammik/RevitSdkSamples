//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Xml;

using Autodesk.Revit;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Elements;

using Application = Autodesk.Revit.Application;

namespace RayTraceBounce
{
  public class RaytraceBouncePool : IExternalCommand
  {
    // have a line style "bounce" created in the 
    // document before running this

    /// <summary>
    /// Maximum number of bounces to calculate.
    /// </summary>
    const int _max_bounces = 100;

    /// <summary>
    /// Message box caption.
    /// </summary>
    const string _caption = "RayTraceBounce Pool Table";

    ExternalCommandData cdata;
    Application app;
    Document doc;
    Face face = null;
    Reference rClosest = null;
    View3D view = null;
    static double epsilon = 0.00000001;
    int LineCount = 0;
    int RayCount = 0;

    public IExternalCommand.Result Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      cdata = commandData;
      app = commandData.Application;
      doc = app.ActiveDocument;
      Get3DView();
      DeleteLines();

      // find the cue

      XYZ direction = null;
      XYZ startpt = null;

      List<Autodesk.Revit.Element> list 
        = new List<Autodesk.Revit.Element>();

      Filter filter = app.Create.Filter
        .NewTypeFilter( typeof( FamilyInstance ) );

      int num = app.ActiveDocument.get_Elements( 
        filter, list );

      foreach( Autodesk.Revit.Element e in list )
      {
        if( e.Name == "cue" )
        {
          LocationCurve lc = e.Location 
            as LocationCurve;

          direction = lc.Curve.ComputeDerivatives( 
            0, true ).BasisX;

          startpt = lc.Curve.get_EndPoint( 1 );

          break;
        }
      }
      if( null == view )
      {
        MessageBox.Show( "A default 3D view (named {3D}) "
          + " must exist before running this command",
          _caption );
      }
      else
      {
        for( int ctr = 1; ctr <= _max_bounces; ++ctr )
        {
          ReferenceArray references 
            = doc.FindReferencesByDirection( 
              startpt, direction, view );

          rClosest = null;
          FindClosestReference( references );

          if( rClosest == null )
          {
            MessageBox.Show( "Ray " + ctr + " aborted. "
              + "No closest face reference found.",
              _caption );

            return IExternalCommand.Result.Succeeded;
          }
          else
          {
            XYZ endpt = new XYZ( 
              rClosest.GlobalPoint.X, 
              rClosest.GlobalPoint.Y, 
              rClosest.GlobalPoint.Z );

            if( startpt.AlmostEqual( endpt ) )
            {
              MessageBox.Show( 
                "Start and end points are equal. Ray " 
                + ctr + " aborted\n" 
                + startpt.X + ", " 
                + startpt.Y + ", " 
                + startpt.Z,
                _caption );

              break;
            }
            else
            {
              MakeLine( startpt, endpt, direction, "bounce" );
              RayCount = RayCount + 1;
              face = rClosest.GeometryObject as Face;
              if( rClosest.Element.Category.Name == "Doors" )
              {
                MessageBox.Show( "You sank the ball with " 
                  + ctr + " shots.", _caption );

                return IExternalCommand.Result.Succeeded;
              }
              UV endptUV = rClosest.UVPoint;

              // face normal where ray hits

              XYZ faceNormal = face.ComputeDerivatives( 
                endptUV ).BasisZ;

              // transformation to get it in terms of 
              // document coordinates instead of the 
              // parent symbol

              faceNormal = rClosest.Transform.OfVector( 
                faceNormal );

              // http://www.fvastro.org/presentations/ray_tracing.htm

              XYZ directionMirrored = direction 
                - 2 * direction.Dot( faceNormal ) * faceNormal;

              // get ready to shoot the next ray

              direction = directionMirrored; 

              startpt = endpt;
            }
          }
        }
      }
      return IExternalCommand.Result.Succeeded;
    }

    /// <summary>
    /// Find closest reference.
    /// </summary>
    /// <param name="references">Input array of references</param>
    /// <returns></returns>
    public Reference FindClosestReference( 
      ReferenceArray references )
    {
      double face_prox = Double.PositiveInfinity;
      double edge_prox = Double.PositiveInfinity;
      
      foreach( Reference r in references )
      {
        if( r.Element.Category.Name != "Generic Models" )
        {
          face = null;
          face = r.GeometryObject as Face;
          Edge edge = null;
          edge = r.GeometryObject as Edge;
          if( face != null )
          {
            // when startpoint is on a surface, should 
            // FindReferencesByDirection find that surface?

            if( Math.Abs( r.ProximityParameter ) < face_prox 
              && r.ProximityParameter > epsilon ) 
            {
              rClosest = r;
              face_prox = Math.Abs( r.ProximityParameter );
            }
          }
          if( edge != null )
          {
            // when startpoint is on a surface, should 
            // FindReferencesByDirection find that surface?

            if( Math.Abs( r.ProximityParameter ) < edge_prox 
              && r.ProximityParameter > epsilon ) 
            {
              edge_prox = Math.Abs( r.ProximityParameter );
            }
          }
        }
      }

      // stop bouncing if there is an edge at least 
      // as close as the neareast face - there is no 
      // single angle of reflection for a ray 
      // striking a line

      if( edge_prox <= face_prox )
      {
        rClosest = null;
      }
      return rClosest;
    }

    /// <summary>
    /// Create a model line with the given start, end point, and line style.
    /// </summary>
    /// <param name="sp">Start point</param>
    /// <param name="ep">End point</param>
    /// <param name="direction">Sketch plane normal vector</param>
    /// <param name="style">Line style name</param>
    public void MakeLine( 
      XYZ sp, 
      XYZ ep, 
      XYZ direction, 
      string style )
    {
      LineCount = LineCount + 1;

      Line line = app.Create.NewLineBound( sp, ep );

      Plane geometryPlane = app.Create.NewPlane( 
        direction, sp );

      Document doc = app.ActiveDocument;

      SketchPlane skplane = doc.Create.NewSketchPlane( 
        geometryPlane );

      ModelCurve mcurve = doc.Create.NewModelCurve( 
        line, skplane );

      ElementArray lsArr = mcurve.LineStyles;

      foreach( Autodesk.Revit.Element e in lsArr )
      {
        if( e.Name == style )
        {
          mcurve.LineStyle = e;
          break;
        }
      }
    }

    /// <summary>
    /// Return the view named "{3D}" in the current document.
    /// </summary>
    public void Get3DView()
    {
      List<Autodesk.Revit.Element> list 
        = new List<Autodesk.Revit.Element>();

      Filter filter = app.Create.Filter.NewTypeFilter( 
        typeof( View3D ) );

      Int64 num = app.ActiveDocument.get_Elements( 
        filter, list );

      foreach( Autodesk.Revit.Element v in list )
      {
        if( v.Name == "{3D}" )
        {
          view = v as View3D;
          break;
        }
      }
    }

    /// <summary>
    /// Delete the lines from the previous run.
    /// </summary>
    public void DeleteLines()
    {
      List<Autodesk.Revit.Element> list 
        = new List<Autodesk.Revit.Element>();

      Filter filter = app.Create.Filter.NewTypeFilter( 
        typeof( CurveElement ), true );

      Int64 num = app.ActiveDocument.get_Elements( 
        filter, list );

      foreach( Autodesk.Revit.Element e in list )
      {
        ModelCurve mc = e as ModelCurve;
        if( mc != null )
        {
          if( mc.LineStyle.Name == "bounce"
            || mc.LineStyle.Name == "normal" )
          {
            app.ActiveDocument.Delete( e );
          }
        }
      }
    }
  }
}
