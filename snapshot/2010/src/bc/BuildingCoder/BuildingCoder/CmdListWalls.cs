using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using XYZ = Autodesk.Revit.Geometry.XYZ;

namespace BuildingCoder
{
  public class CmdListWalls : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      Type t = typeof( Wall );
      Filter f = app.Create.Filter.NewTypeFilter( t );
      List<Element> walls = new List<Element>();
      int n = doc.get_Elements( f, walls );
      foreach( Wall wall in walls )
      {
        Parameter param = wall.get_Parameter(
          BuiltInParameter.HOST_AREA_COMPUTED );
        double a = ( ( null != param )
          && ( StorageType.Double == param.StorageType ) )
          ? param.AsDouble()
          : 0.0;
        string s = ( null != param )
          ? param.AsValueString()
          : "null";
        LocationCurve lc = wall.Location as LocationCurve;
        XYZ p = lc.Curve.get_EndPoint( 0 );
        XYZ q = lc.Curve.get_EndPoint( 1 );
        double l = q.Distance( p );
        string format
          = "Wall <{0} {1}> length {2} area {3} ({4})";
        Debug.Print( format,
          wall.Id.Value.ToString(), wall.Name,
          Util.RealString( l ), Util.RealString( a ),
          s );
      }
      return CmdResult.Succeeded;
    }
  }
}
