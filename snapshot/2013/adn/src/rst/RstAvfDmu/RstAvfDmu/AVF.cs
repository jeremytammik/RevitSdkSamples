#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion // Namespaces

namespace RstAvfDmu
{
  [Transaction( TransactionMode.Automatic )]
  class AVFWithPointLoads : IExternalCommand
  {
    IList<Element> m_Loads = null;

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIDocument uiDoc = commandData.Application.ActiveUIDocument;

      // Get access to manager class for working with analysis results
      SpatialFieldManager sfm
        = SpatialFieldManager.GetSpatialFieldManager(
          uiDoc.Document.ActiveView );

      if( null == sfm )
      {
        sfm = SpatialFieldManager.CreateSpatialFieldManager( uiDoc.Document.ActiveView, 1 );
      }

      //get access to each point load on the slab
      FilteredElementCollector collector = new FilteredElementCollector( uiDoc.Document );
      ElementClassFilter filter = new ElementClassFilter( typeof( LoadBase ) );
      m_Loads = collector.WherePasses( filter ).ToElements();

      // create primitives or container for the results
      Reference rf = uiDoc.Selection.PickObject( ObjectType.Face, "Select Face:" );
      ElementId idPick = rf.ElementId;
      Element elem = uiDoc.Document.GetElement(idPick);
      Face face = elem.GetGeometryObjectFromReference(rf) as Face;
      

      int idx = sfm.AddSpatialFieldPrimitive( rf );
      List<double> doubleList = new List<double>();
      IList<UV> uvPts = new List<UV>();
      IList<ValueAtPoint> valList = new List<ValueAtPoint>();
      BoundingBoxUV bb = face.GetBoundingBox();
      for( double u = bb.Min.U; u < bb.Max.U; u = u + ( bb.Max.U - bb.Min.U ) / 15 )
      {
        for( double v = bb.Min.V; v < bb.Max.V; v = v + ( bb.Max.V - bb.Min.V ) / 15 )
        {
          UV uvPnt = new UV( u, v );
          uvPts.Add( uvPnt );
          XYZ faceXYZ = face.Evaluate( uvPnt );
          doubleList.Add( loadFactor( faceXYZ ) );
          valList.Add( new ValueAtPoint( doubleList ) );
          doubleList.Clear();
        }
      }

      AnalysisResultSchema resultSchema;
      IList<int> registeredResults = new List<int>();
      registeredResults = sfm.GetRegisteredResults();
      int schemaIndex = 0;
      if (registeredResults.Count == 0)
      {
          resultSchema = new AnalysisResultSchema("Schema", "Description");
          schemaIndex = sfm.RegisterResult(resultSchema);
      }
      else
          schemaIndex = registeredResults.First();

      FieldDomainPointsByUV pnts = new FieldDomainPointsByUV( uvPts );
      FieldValues vals = new FieldValues( valList );
      //sfm.UpdateSpatialFieldPrimitive(idx, pnts, vals); //For 2011
      sfm.UpdateSpatialFieldPrimitive(idx, pnts, vals, schemaIndex); //For 2012

      return Result.Succeeded;
    }

    /// <summary>
    /// Calculate the load for each point on face using the points loads and the 
    /// distance of the point on face to the point load
    /// </summary>
    private double loadFactor( XYZ faceXYZ )
    {
      double load = 0;
      foreach( Element ele in m_Loads )
      {
        PointLoad ptLoad = ele as PointLoad;
        XYZ ptLoadPtXYZ = ptLoad.Point;
        if( !ptLoadPtXYZ.IsAlmostEqualTo( faceXYZ ) )
        {
          load
            += ptLoad.get_Parameter( BuiltInParameter.LOAD_FORCE_FZ ).AsDouble()
              / faceXYZ.DistanceTo( ptLoadPtXYZ );
        }
      }
      return load;
    }
  }
}
