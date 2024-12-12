//
// (C) Copyright 2003-2015 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable. 

using System;
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.AnalysisVisualizationFramework.CS
{
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class SpatialFieldGradient : IExternalCommand
   {
      static AddInId m_appId = new AddInId(new Guid("CF099951-E66B-4a35-BF7F-2959CA87A42D"));
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         ExternalCommandData cdata = commandData;
         Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
         Document doc = commandData.Application.ActiveUIDocument.Document;
         UIDocument uiDoc = commandData.Application.ActiveUIDocument;

         SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(doc.ActiveView);         
         if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(doc.ActiveView, 1);

         IList<Reference> refList = new List<Reference>();
         refList = uiDoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Face);
                 foreach (Reference reference in refList)
                 {

                         IList<UV> uvPts = new List<UV>();

                         List<double> doubleList = new List<double>();
                         IList<ValueAtPoint> valList = new List<ValueAtPoint>();
                         Face face = doc.GetElement(reference).GetGeometryObjectFromReference(reference)as Face;
                         BoundingBoxUV bb = face.GetBoundingBox();
                         UV min = bb.Min;
                         UV max = bb.Max;

                         for (double u = min.U; u < max.U; u += (max.U - min.U) / 10)
                         {
                             for (double v = min.V; v < max.V; v += (max.V - min.V) / 10)
                             {
                                 UV uv = new UV(u, v);
                                 if (face.IsInside(uv))
                                 {
                                     uvPts.Add(uv);
                                     doubleList.Add(v + DateTime.Now.Second);
                                     valList.Add(new ValueAtPoint(doubleList));
                                     doubleList.Clear();
                                 }
                             }
                         }

                         FieldDomainPointsByUV pnts = new FieldDomainPointsByUV(uvPts);
                         FieldValues vals = new FieldValues(valList);
                         int idx = sfm.AddSpatialFieldPrimitive(reference);
                         AnalysisResultSchema resultSchema = new AnalysisResultSchema("Schema 1", "Schema 1 Description"); 
                         sfm.UpdateSpatialFieldPrimitive(idx, pnts, vals, sfm.RegisterResult(resultSchema));
                 }




         return Result.Succeeded;
      }
   }
}
 