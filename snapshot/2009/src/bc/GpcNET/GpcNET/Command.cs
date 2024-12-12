using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Areas;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Elements;

using MessageBox = System.Windows.Forms.MessageBox;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using GeoElement = Autodesk.Revit.Geometry.Element;
using RvtElement = Autodesk.Revit.Element;

namespace GpcNET
{
  public class Command : IExternalCommand  
  {
    Application app;
    Document doc; 

    public CmdResult Execute(
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      CmdResult rc = CmdResult.Failed;
      app = commandData.Application;
      doc = app.ActiveDocument;

      Floor[] floors = new Floor[2] { null, null };

      // get the first 2 floors of the selection
      foreach( RvtElement e in doc.Selection.Elements )
      {
        if( null == floors[0] )
        {
          floors[0] = e as Floor;
        }
        else if( null == floors[1] )
        {
          floors[1] = e as Floor;
        }
        else
        {
          break;
        }
      }

      // if the selction did not contain two floors, return
      if( null == floors[0] || null == floors[1] )
      {
        MessageBox.Show( 
          "Please select two floors before"
          + " running this command.", 
          "GpcNET" );
      }
      else
      {
        // get the intersection
        Polygon poly1 = getPolygon( floors[0] );

        Polygon poly2 = getPolygon( floors[1] );

        Polygon poly3 = poly1.Clip( 
          GpcOperation.Intersection, 
          poly2 );

        // if it looks like a valid polygon, create a new floor
        if( 0 < poly3.NofContours )
        {
          CurveArray curves = app.Create.NewCurveArray();
          VertexList v = poly3.Contour[0];
          int i, j, n = v.NofVertices;
          for( i = 0; i < n; ++i )
          {
            j = ( i + 1 ) % n;
            Vertex p = v.Vertex[i];
            Vertex q = v.Vertex[j];

            curves.Append( app.Create.NewLineBound(
              app.Create.NewXYZ( p.X, p.Y, 0 ),
              app.Create.NewXYZ( q.X, q.Y, 0 ) ) );
          }
          doc.Create.NewFloor( curves, 
            floors[0].FloorType, 
            floors[0].Level, false );

          rc = CmdResult.Succeeded;
        }
      }
      return rc;
    }

    /// <summary>
    /// Return a GPC polygon from the first edge loop of the 
    /// first face of the first solid of the given floor.
    /// </summary>
    Polygon getPolygon( Floor floor )
    {
      Options geomOptions 
        = app.Create.NewGeometryOptions();

      GeoElement elem 
        = floor.get_Geometry( geomOptions );

      List<Vertex> vertices = new List<Vertex>();

      foreach( object obj in elem.Objects )
      {
        Solid solid = obj as Solid;
        if( null != solid )
        {
          Face face = solid.Faces.get_Item( 0 );
          EdgeArray loop = face.EdgeLoops.get_Item( 0 );
          foreach( Edge edge in loop )
          {
            XYZArray edgePts = edge.Tessellate();
            int n = edgePts.Size;
            for( int i = 0; i < n - 1; ++i )
            {
              XYZ p = edgePts.get_Item( i );
              vertices.Add( new Vertex( p.X, p.Y ) );
            }
          }
          break;
        }
      }
      VertexList vertexList = new VertexList();
      vertexList.NofVertices = vertices.Count;
      vertexList.Vertex = vertices.ToArray();
      Polygon poly = new Polygon();
      poly.AddContour( vertexList, false );
      return poly;
    }
  }
}
