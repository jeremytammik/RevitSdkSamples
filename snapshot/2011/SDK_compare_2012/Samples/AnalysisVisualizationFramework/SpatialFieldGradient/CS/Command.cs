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

         AnalysisDisplayStyle analysisDisplayStyle = null;
         FilteredElementCollector collector1 = new FilteredElementCollector(doc);
         ICollection<Element> collection = collector1.OfClass(typeof(AnalysisDisplayStyle)).ToElements();
         var displayStyle = from element in collection where element.Name == "Display Style 1" select element;
         if (displayStyle.Count() == 0)
         {
            AnalysisDisplayColoredSurfaceSettings coloredSurfaceSettings = new AnalysisDisplayColoredSurfaceSettings();
            coloredSurfaceSettings.ShowGridLines = true;

            AnalysisDisplayColorSettings colorSettings = new AnalysisDisplayColorSettings();
            Color orange = new Color(255, 205, 0);
            Color purple = new Color(200, 0, 200);
            colorSettings.MaxColor = orange;
            colorSettings.MinColor = purple;

            AnalysisDisplayLegendSettings legendSettings = new AnalysisDisplayLegendSettings();
            legendSettings.NumberOfSteps = 10;
            legendSettings.Rounding = 0.05;
            legendSettings.ShowDataDescription = false;
            legendSettings.ShowLegend = true;

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            ICollection<Element> elementCollection = collector2.OfClass(typeof(TextNoteType)).ToElements();
            var textElements = from element in collector2 where element.Name == "LegendText" select element;
            if (textElements.Count() > 0) // if LegendText exists, use it for this Display Style
            {
               TextNoteType textType = textElements.Cast<TextNoteType>().ElementAt<TextNoteType>(0);
               legendSettings.SetTextTypeId(textType.Id, doc);
            }
            analysisDisplayStyle = AnalysisDisplayStyle.CreateAnalysisDisplayStyle(doc, "Display Style 1", coloredSurfaceSettings, colorSettings, legendSettings);
         }
         else
         {
            analysisDisplayStyle = displayStyle.Cast<AnalysisDisplayStyle>().ElementAt<AnalysisDisplayStyle>(0);            
         }
         doc.ActiveView.AnalysisDisplayStyleId = analysisDisplayStyle.Id;
         SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(doc.ActiveView);         
         if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(doc.ActiveView, 3);
         IList<string> measureNames = new List<string>();
         measureNames.Add("u");
         measureNames.Add("u+1");
         measureNames.Add("u*10");
         sfm.SetMeasurementNames(measureNames);
         IList<string> unitNames = new List<string>();
         unitNames.Add("Feet"); 
         unitNames.Add("Inches");
         IList<double> multipliers = new List<double>();
         multipliers.Add(1);
         multipliers.Add(12);
         sfm.SetUnits(unitNames, multipliers);

         Reference reference = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face);
         int idx = sfm.AddSpatialFieldPrimitive(reference);
         IList<UV> uvPts = new List<UV>();
         Face face = reference.GeometryObject as Face;
         List<double> doubleList = new List<double>();
         IList<ValueAtPoint> valList = new List<ValueAtPoint>();

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
                  doubleList.Add(u);
                  doubleList.Add(u + 1);
                  doubleList.Add(u * 10);
                  valList.Add(new ValueAtPoint(doubleList));
                  doubleList.Clear();
               }
            }
         }

         FieldDomainPointsByUV pnts = new FieldDomainPointsByUV(uvPts);
         FieldValues vals = new FieldValues(valList);
         sfm.UpdateSpatialFieldPrimitive(idx, pnts, vals);

         return Result.Succeeded;
      }
   }
}
 