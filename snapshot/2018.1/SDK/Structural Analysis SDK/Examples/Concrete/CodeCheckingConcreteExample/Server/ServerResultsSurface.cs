//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Linq;
using System.Text;
using Autodesk.Revit.DB.CodeChecking;
using Autodesk.Revit.DB.CodeChecking.Documentation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.Revit.DB.CodeChecking.Engineering.Concrete;
using CodeCheckingConcreteExample.Utility;
using CodeCheckingConcreteExample.Main;
using CodeCheckingConcreteExample.Properties;
using Autodesk.Revit.DB.CodeChecking.Engineering.Tools;


/// <structural_toolkit_2015> 
namespace CodeCheckingConcreteExample.Server
{
   public partial class Server : Autodesk.Revit.DB.CodeChecking.Documentation.MultiStructureServer
   {
      /// <summary>
      /// Fills document body object with formatted reinforcement calculation results
      /// </summary>
      /// <param name="body">DocumentBody to be filled</param>
      /// <param name="resultSurfaceElement">Calculation results schema class</param>
      /// <param name="document">Active Revit document</param>
      /// <param name="element">Structural element</param>
      /// <param name="elementType">Typ of element</param>
      private void WriteResultsToNoteForSurfaceElements(DocumentBody body, ResultSurfaceElement resultSurfaceElement, Autodesk.Revit.DB.Document document, Element element,ElementType elementType)
      {

         List<ResultInPointSurface> resultsInPointsCollection = resultSurfaceElement.GetResultsInPointsCollection();

         /////////////////////////// Basic Level ///////////////////////////////////////

         //////////////////// Summary
         {
            if (resultSurfaceElement.MultiLayer)
            {
               DocumentSection resultLeyerSection = new DocumentSection(Resources.ResourceManager.GetString("Layers"), 5);
               resultLeyerSection.Level = DetailLevel.General;
               List<Tuple<string, double>> structuralLayers = resultSurfaceElement.GetStructuralLayers();
               double calculationThickness = 0;
               resultLeyerSection.Body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("LayerTakenIntoConsideration"),true));
               for (int layersId = 0; layersId < structuralLayers.Count(); layersId++)
               {
                  resultLeyerSection.Body.Elements.Add((new DocumentValueWithName("- " + structuralLayers[layersId].Item1, structuralLayers[layersId].Item2, UnitsConverter.GetInternalUnit(UnitType.UT_Section_Dimension), UnitType.UT_Section_Dimension, document.GetUnits())));
                  calculationThickness += structuralLayers[layersId].Item2;
               }
               resultLeyerSection.Body.Elements.Add((new DocumentValueWithName(Resources.ResourceManager.GetString("ThicknessTakenIntoConsideration"),calculationThickness, UnitsConverter.GetInternalUnit(UnitType.UT_Section_Dimension), UnitType.UT_Section_Dimension, document.GetUnits())));
               body.Elements.Add(resultLeyerSection);
            }

            // Create document section
            DocumentSection resultSummarySection = new DocumentSection(Resources.ResourceManager.GetString("Summary"), 5);
            resultSummarySection.Level = DetailLevel.General;

            // Calculate mean and extreme values
            IEnumerable<ResultTypeSurface> primary   = new List<ResultTypeSurface> {ResultTypeSurface.AxxTop, ResultTypeSurface.AxxBottom};
            IEnumerable<ResultTypeSurface> secondary = new List<ResultTypeSurface> { ResultTypeSurface.AyyTop, ResultTypeSurface.AyyBottom };
            double meanPrimary   = resultsInPointsCollection.Average(s => (primary.Sum(q => s[q]))),
                   meanSecondary = resultsInPointsCollection.Average(s => (secondary.Sum(q => s[q])));

            // Add them to the section
            string meanPrimaryString = elementType == ElementType.Floor ? "MeanPrimaryReinforcementDensity" : "MeanVerticallReinforcementDensity";
            string meanSecondaryString = elementType == ElementType.Floor ? "MeanSecondaryReinforcementDensity" : "MeanHorizontalReinforcementDensity";
            meanPrimaryString = Resources.ResourceManager.GetString(meanPrimaryString);
            meanSecondaryString = Resources.ResourceManager.GetString(meanSecondaryString);
            resultSummarySection.Body.Elements.Add((new DocumentValueWithName(meanPrimaryString, meanPrimary, UnitsConverter.GetInternalUnit(UnitType.UT_Reinforcement_Area_per_Unit_Length), UnitType.UT_Reinforcement_Area_per_Unit_Length, document.GetUnits())));
            resultSummarySection.Body.Elements.Add((new DocumentValueWithName(meanSecondaryString, meanSecondary, UnitsConverter.GetInternalUnit(UnitType.UT_Reinforcement_Area_per_Unit_Length), UnitType.UT_Reinforcement_Area_per_Unit_Length, document.GetUnits())));


            // Add extreme reinforcement table
            resultSummarySection.Body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("ExtremeValuesOfReinforcement")));
            resultSummarySection.Body.Elements.Add(CreateExtremeResultTableForSurfaceElements(resultsInPointsCollection, new List<ResultTypeSurface>() { ResultTypeSurface.AxxBottom, ResultTypeSurface.AxxTop, ResultTypeSurface.AyyBottom, ResultTypeSurface.AyyTop }, document, elementType));


            // Add section to the document body
            body.Elements.Add(resultSummarySection);
         }
         /////////////////////////// Other levels ///////////////////////////////////////

         //////////////////// Reinforcement table
         {
            // Create document section
            DocumentSection reinforcementTableSection = new DocumentSection(Resources.ResourceManager.GetString("Reinforcement"), 5);
            reinforcementTableSection.Level = DetailLevel.Medium;

            // Add extreme reinforcement table
            reinforcementTableSection.Body.Elements.Add(CreateResultTableForSurfaceElements(resultsInPointsCollection, new List<ResultTypeSurface>() { ResultTypeSurface.AxxBottom, ResultTypeSurface.AxxTop, ResultTypeSurface.AyyBottom, ResultTypeSurface.AyyTop }, document, elementType));

            // Add section to the document body
            body.Elements.Add(reinforcementTableSection);
         }

         //////////////////// Reinforcement maps
         {
            // Create document section
            DocumentSection reinforcementMapsSection = new DocumentSection(Resources.ResourceManager.GetString("ReinforcementMaps"), 5);
            reinforcementMapsSection.Level = DetailLevel.Detail;

            // Create and add reinforcement maps
            ResultTypeSurface[] vForceType =  { ResultTypeSurface.AxxBottom, ResultTypeSurface.AyyBottom, ResultTypeSurface.AxxTop, ResultTypeSurface.AyyTop };
            List<DocumentMap> vMap = CreateResultMapSeriesForSurfaceElements(element, resultsInPointsCollection, vForceType.Select(s => new Tuple<ResultTypeSurface, string>(s, ResultDescription(elementType,s))));
            foreach (DocumentMap map in vMap)
            {
               reinforcementMapsSection.Body.Elements.Add(map);
            }
            // Add section to the document body
            body.Elements.Add(reinforcementMapsSection);
         }

         //////////////////// Extreme forces table
         List<ResultTypeSurface> nonZeroInternalForces = ResultTypeSurfaceHelper.InternalForcesResults.Where(s => resultsInPointsCollection.Any(r => Math.Abs(r[s]) > Double.Epsilon)).ToList();
         bool isNonZeroInternalForces = (nonZeroInternalForces.Count() > 0);
         {
            // Only for non-zero values 
            if (isNonZeroInternalForces)
            {
               // Create document section
               DocumentSection extremeForcesTable = new DocumentSection(Resources.ResourceManager.GetString("ExtremeAxialForces"), 5);
               extremeForcesTable.Level = DetailLevel.Medium;

               // Add extreme force  table
               extremeForcesTable.Body.Elements.Add(CreateExtremeResultTableForSurfaceElements(resultsInPointsCollection, ResultTypeSurfaceHelper.InternalForcesResults, document, elementType));

               // Add section to the document body
               body.Elements.Add(extremeForcesTable);
            }
         }

         //////////////////// Extreme moments table
         List<ResultTypeSurface> nonZeroInternalMoments = ResultTypeSurfaceHelper.InternalMomentsResults.Where(s => resultsInPointsCollection.Any(r => Math.Abs(r[s]) > Double.Epsilon)).ToList();
         bool isNonZeroInternalMoments = (nonZeroInternalMoments.Count() > 0);
         {
            // Only for non-zero values 
            if (isNonZeroInternalMoments)
            {
               // Create document section
               DocumentSection extremeMomentsTable = new DocumentSection(Resources.ResourceManager.GetString("ExtremeBendingMoments"), 5);
               extremeMomentsTable.Level = DetailLevel.Medium;

               // Add extreme moment  table
               extremeMomentsTable.Body.Elements.Add(CreateExtremeResultTableForSurfaceElements(resultsInPointsCollection, ResultTypeSurfaceHelper.InternalMomentsResults, document, elementType));

               // Add section to the document body
               body.Elements.Add(extremeMomentsTable);
            }
         }

         //////////////////// Force envelopes table
         {
            // Only for non-zero values 
            if (isNonZeroInternalForces)
            {
               // Create document section
               DocumentSection forceEnvelopesTable = new DocumentSection(Resources.ResourceManager.GetString("EnvelopeF"), 5);
               forceEnvelopesTable.Level = DetailLevel.Detail;

               // Add extreme force  table
               forceEnvelopesTable.Body.Elements.Add(CreateResultTableForSurfaceElements(resultsInPointsCollection, ResultTypeSurfaceHelper.InternalForcesResults, document, elementType));

               // Add section to the document body
               body.Elements.Add(forceEnvelopesTable);
            }
         }

         //////////////////// Moments envelopes table
         {
            // Only for non-zero values 
            if (isNonZeroInternalMoments)
            {
               // Create document section
               DocumentSection momentEnvelopesTable = new DocumentSection(Resources.ResourceManager.GetString("EnvelopeM"), 5);
               momentEnvelopesTable.Level = DetailLevel.Detail;

               // Add extreme force  table
               momentEnvelopesTable.Body.Elements.Add(CreateResultTableForSurfaceElements(resultsInPointsCollection, ResultTypeSurfaceHelper.InternalMomentsResults, document, elementType));

               // Add section to the document body
               body.Elements.Add(momentEnvelopesTable);
            }
         }

      }
      /// <summary>
      /// Returns UI descriptions depends to element type and results type.
      /// </summary>
      /// <param name="elementType">Typ of element</param>
      /// <param name="result">Results.</param>
      /// <returns>Description that can be displayed on the maps and in the table.</returns>
      private string ResultDescription(ElementType elementType, ResultTypeSurface result)
      {
         string description = string.Empty;
         switch (result)
         {
            case ResultTypeSurface.AxxBottom:
            case ResultTypeSurface.AxxTop:
            case ResultTypeSurface.AyyBottom:
            case ResultTypeSurface.AyyTop:
               description = Resources.ResourceManager.GetString(elementType.ToString() + result.ToString());
               break;
            default:
               description = Resources.ResourceManager.GetString(result.ToString());
               break;
         }
         if (null == description)
            description = result.ToString();
         return description;
      }
      /// <summary>
      /// Creates maps based on surface results. The maps can be displayed in the documentation.
      /// </summary>
      /// <param name="element">Revit element for which result document is built</param>
      /// <param name="resultsInPoints">Result in point collection</param>
      /// <param name="resultTypes">The type of results, values and description that can be displayed on the maps.</param>
      /// <returns>Returns list of documets maps based on results that can be displayed in the documentation</returns>
      private List<DocumentMap> CreateResultMapSeriesForSurfaceElements(Element element, ICollection<ResultInPointSurface> resultsInPoints, IEnumerable<Tuple<ResultTypeSurface, string>> resultTypes)
      {
         List<DocumentMap> vDocMap = new List<DocumentMap>();
         MapDataGenerator mapPoints = new MapDataGenerator();
         foreach (Tuple<ResultTypeSurface, string> forceDesc in resultTypes)
         {
            List<double> resData = resultsInPoints.Select(s => s[forceDesc.Item1]).ToList();
            if (resData.Max(s => (Math.Abs(s))) < 1.0e-8)
            {
               continue;
            }
            AnalyticalModel slab = element as AnalyticalModel;
            DisplayUnit displayUnit = DisplayUnit.IMPERIAL;
            if (Server.UnitSystem == Autodesk.Revit.DB.ResultsBuilder.UnitsSystem.Metric)
               displayUnit = DisplayUnit.METRIC;
            ElementAnalyser elementAnalyser = new ElementAnalyser(displayUnit);
            ElementInfo info = elementAnalyser.Analyse(element);
            XYZ xyz = new XYZ();
            if (info != null)
            {
               foreach (ResultInPointSurface resInPt in resultsInPoints)
               {
                  mapPoints.AddPoint(new XYZ(resInPt[ResultTypeSurface.X], resInPt[ResultTypeSurface.Y], resInPt[ResultTypeSurface.Z]), resInPt[forceDesc.Item1]);
               }
               var contourAndHoles = getContourAndHolePoints(info);
               contourAndHoles.Item1.ForEach( s=>mapPoints.AddPointToContour(s));
               contourAndHoles.Item2.ForEach( s=>mapPoints.AddHole(s));
               UnitType ut = forceDesc.Item1.GetUnitType();
               DocumentMap docMap = new DocumentMap(ut, UnitsConverter.GetInternalUnit(ut));
               docMap.Title = forceDesc.Item2;
               int nofPoints = mapPoints.Count;
               for (int ptId = 0; ptId < nofPoints; ptId++)
               {
                  UV pt = mapPoints.GetPoint(ptId);
                  docMap.AddPoint(pt.U, pt.V, mapPoints.GetValue(ptId));
               }
               List<List<int>> holes = mapPoints.Holes;
               foreach (List<int> hole in holes)
               {
                  docMap.AddHole(hole);
               }
               docMap.DefineContour(mapPoints.Contour);
               vDocMap.Add(docMap);
            }
            mapPoints.Clear();
         }
         return vDocMap;
      }


      private Tuple<List<XYZ>,List<List<XYZ>>> getContourAndHolePoints( ElementInfo info)
      {
         List<XYZ> contour = null;
         List<List<XYZ>> holes = null;

         if( info.Type==Autodesk.Revit.DB.CodeChecking.Engineering.ElementType.Slab )
         {
            contour = info.Slabs.AsElement.Contours.First(c=>c.ContourType==SlabContourType.Main).Edges.SelectMany( e=>e.Nodes.Select( n=>n.GetGlobalCoordinates())).ToList();
            holes = info.Slabs.AsElement.Contours.Where( c=>c.ContourType==SlabContourType.Hole).Select
                    ( hc=> hc.Edges.SelectMany( e=>e.Nodes.Select( n=>n.GetGlobalCoordinates())).ToList() ).ToList();
         }
         else if (info.Type == Autodesk.Revit.DB.CodeChecking.Engineering.ElementType.Wall)
         {
            contour = info.Walls.AsElement.Transform.ManyPoints2Global( info.Walls.AsElement.Contour.Points ).ToList();
            holes = info.Walls.AsElement.Openings.Select(o => info.Walls.AsElement.Transform.ManyPoints2Global(o.Points).ToList()).ToList();
         }
         return new Tuple<List<XYZ>, List<List<XYZ>>>(contour,holes);
      }

      /// <summary>
      /// Creates result table based on surface results.
      /// </summary>
      /// <param name="resInPtCollection">Result in point collection</param>
      /// <param name="resultTypes">The type of results, values and description to use in the table.</param>
      /// <param name="document">Revit document</param>
      /// <param name="elementType">Type of element</param>
      /// <returns>Document table containing surface results</returns>
      private DocumentTable CreateResultTableForSurfaceElements(List<ResultInPointSurface> resInPtCollection, IEnumerable<ResultTypeSurface> resultTypes, Autodesk.Revit.DB.Document document, ElementType elementType)
      {
         // results table data
         int rowsCount = 1 + resInPtCollection.Count(),
            colsCount = (ElementType.Wall == elementType ? 3 : 2) + resultTypes.Count();

         Units units = document.GetUnits();
         DocumentTable resultTable = new DocumentTable( rowsCount, colsCount);
         resultTable.HeaderColumnsCount = ElementType.Wall == elementType ? 3 : 2;
         resultTable.HeaderRowsCount = 1;

         // results table data
         List<ResultTypeSurface> typesToPresent = ElementType.Wall == elementType ? new List<ResultTypeSurface>() { ResultTypeSurface.X, ResultTypeSurface.Y, ResultTypeSurface.Z } : new List<ResultTypeSurface>() { ResultTypeSurface.X, ResultTypeSurface.Y };
         typesToPresent.AddRange(resultTypes);

         int colId = 0;
         foreach (ResultTypeSurface resultType in typesToPresent)
         {
            resultTable[0, colId].Elements.Add(new DocumentText(ResultDescription(elementType,resultType)));
            UnitType unitType = resultType.GetUnitType();
            DisplayUnitType displayUnitType = Utility.UnitsConverter.GetInternalUnit(unitType);

            int rowId = 1;
            foreach (ResultInPointSurface resultInPoint in resInPtCollection)
            {
               if (DisplayUnitType.DUT_UNDEFINED != displayUnitType)
               {
                  resultTable[rowId++, colId].Elements.Add(new DocumentValue(resultInPoint[resultType], displayUnitType, unitType, units));
               }
               else
               {
                  resultTable[rowId++, colId].Elements.Add(new DocumentText(resultInPoint[resultType].ToString()));
               }
            }
            colId++;
         }

         return resultTable;
      }

      /// <summary>
      /// Creates result table based on surface results that contains extreme values for specified result types.
      /// </summary>
      /// <param name="resInPtCollection">Result in point collection</param>
      /// <param name="resultTypes">The type of results, values and description to use in the table.</param>
      /// <param name="document">Revit document</param>
      /// <param name="elementType">Type of element</param>
      /// <returns>Document table containing surface results</returns>
      private DocumentTable CreateExtremeResultTableForSurfaceElements(List<ResultInPointSurface> resInPtCollection, IEnumerable<ResultTypeSurface> resultTypes, Autodesk.Revit.DB.Document document, ElementType elementType)
      {
         IEnumerable<Tuple<ResultTypeSurface, double>> maxValues = resultTypes.Select(s => new Tuple<ResultTypeSurface, double>(s, s.ToString().ToLower().Contains("min") ? resInPtCollection.Min(q => q[s]) : resInPtCollection.Max(q => q[s])));
         int rowsCount = 2,
             colsCount = resultTypes.Count();

         Units units = document.GetUnits();
         DocumentTable resultTable = new DocumentTable(rowsCount, colsCount);
         resultTable.HeaderColumnsCount = 0;
         resultTable.HeaderRowsCount = 1;

         // results table data
         List<ResultTypeSurface> typesToPresent = new List<ResultTypeSurface>() { ResultTypeSurface.X, ResultTypeSurface.Y };
         typesToPresent.AddRange(resultTypes);

         int colId=0;
         foreach (Tuple<ResultTypeSurface, double> result in maxValues)
         {
            resultTable[0, colId].Elements.Add(new DocumentText(ResultDescription(elementType, result.Item1)));
            UnitType unitType = result.Item1.GetUnitType();
            DisplayUnitType displayUnitType = Utility.UnitsConverter.GetInternalUnit(unitType);
            if (DisplayUnitType.DUT_UNDEFINED != displayUnitType)
            {
               resultTable[1, colId].Elements.Add(new DocumentValue(result.Item2, displayUnitType, unitType, units));
            }
            else
            {
               resultTable[1, colId].Elements.Add(new DocumentText(result.Item2.ToString()));
            }
            colId++;
         }

         return resultTable;
      }
   }
   /// </structural_toolkit_2015> 
}
