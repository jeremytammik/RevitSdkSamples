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
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.Revit.DB.CodeChecking.Engineering.Concrete;
using CodeCheckingConcreteExample.Utility;
using CodeCheckingConcreteExample.Main;
using CodeCheckingConcreteExample.Properties;
using Autodesk.Revit.DB.CodeChecking.Engineering.Tools;

#pragma warning disable 1591

namespace CodeCheckingConcreteExample.Server
{
   [Autodesk.Revit.DB.CodeChecking.Attributes.CalculationParamsStructure(typeof(CalculationParameter))]
   [Autodesk.Revit.DB.CodeChecking.Attributes.LabelStructure(typeof(LabelBeam), BuiltInCategory.OST_BeamAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.LabelStructure(typeof(LabelColumn), BuiltInCategory.OST_ColumnAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.ResultStructure(typeof(ResultBeam), BuiltInCategory.OST_BeamAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.ResultStructure(typeof(ResultColumn), BuiltInCategory.OST_ColumnAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   public class Server : Autodesk.Revit.DB.CodeChecking.Documentation.MultiStructureServer
   {
      public static readonly Autodesk.Revit.DB.ResultsBuilder.UnitsSystem UnitSystem = Autodesk.Revit.DB.ResultsBuilder.UnitsSystem.Metric;
      public static readonly Guid ID = new Guid("1d3d13ad-4330-4b4a-bfe3-067ff87a72b1");
      #region ICodeCheckingServer Members

      public override Entity GetDefaultLabel(Autodesk.Revit.DB.Document document, StructuralAssetClass material, BuiltInCategory category)
      {
         switch (material)
         {
            case StructuralAssetClass.Concrete:
               switch (category)
               {
                  default:
                     break;
                  case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
                     {
                        IList<ElementId> materials = RebarUtility.GetListMaterialOfRebars(document);
                        if (materials.Count > 0)
                        {
                           IList<ElementId> barsIds = RebarUtility.GetListRebarForMaterial(document, materials[0]);
                           if (barsIds.Count > 0)
                           {
                              LabelColumn label = new LabelColumn();
                              label.LongitudinalReinforcement.Material = document.GetElement(materials[0]) as Material;
                              label.LongitudinalReinforcement.RebarBarType = RebarUtility.GetRebarType(document, barsIds[0]);

                              label.TransversalReinforcement.Material = label.LongitudinalReinforcement.Material;
                              label.TransversalReinforcement.RebarBarType = label.LongitudinalReinforcement.RebarBarType;

                              return label.GetEntity();
                           }
                        }
                     }
                     break;
                  case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                     {
                        IList<ElementId> materials = RebarUtility.GetListMaterialOfRebars(document);
                        if (materials.Count > 0)
                        {
                           IList<ElementId> barsIds = RebarUtility.GetListRebarForMaterial(document, materials[0]);
                           if (barsIds.Count > 0)
                           {
                              LabelBeam label = new LabelBeam();
                              label.LongitudinalReinforcement.Material = document.GetElement(materials[0]) as Material;
                              label.LongitudinalReinforcement.RebarBarType = RebarUtility.GetRebarType(document, barsIds[0]);

                              label.TransversalReinforcement.Material = label.LongitudinalReinforcement.Material;
                              label.TransversalReinforcement.RebarBarType = label.LongitudinalReinforcement.RebarBarType;

                              return label.GetEntity();
                           }
                        }
                     }
                     break;
               }
               break;
         }

         return base.GetDefaultLabel(document, material, category);
      }

      public enum ElementType { Beam, Column };
      /// <summary>
      /// Build document body for result document
      /// </summary>
      /// <param name="calculationParameters">Code calculation parameters</param>
      /// <param name="element">Revit element for which result document is built</param>
      /// <param name="document">Active Revit document</param>
      /// <returns></returns>
      public override DocumentBody BuildResultDocumentBody(Autodesk.Revit.DB.ExtensibleStorage.Entity calculationParameters, Element element, Autodesk.Revit.DB.Document document)
      {
         Autodesk.Revit.DB.CodeChecking.Storage.StorageService service = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService();
         Autodesk.Revit.DB.CodeChecking.Storage.StorageDocument storageDocument = service.GetStorageDocument(document);
         Guid activePackageId = storageDocument.CalculationParamsManager.CalculationParams.GetInputResultPackageId(Server.ID);
         Autodesk.Revit.DB.CodeChecking.Storage.ResultStatus status = storageDocument.ResultsManager.GetResultStatus(element, activePackageId, Server.ID);
         DocumentBody body = new DocumentBody();
         if (!status.IsError())
         {
            // create body object
            // get results schema for the current element
            bool isBeam = calculationParameters.Schema.SchemaName == "ResultBeam";
            bool isColumn = calculationParameters.Schema.SchemaName == "ResultColumn";
            if (isBeam || isColumn)
            {
               ResultLinearElement resultSchema = null;
               if (isBeam)
               {
                  resultSchema = new ResultBeam();
               }
               else
               {
                  resultSchema = new ResultColumn();
               }

               resultSchema.SetProperties(calculationParameters);
               // Fill document with results from reinforcement calculation
               WriteTheoreticalReinfToNote(body, resultSchema, document, isBeam ? ElementType.Beam : ElementType.Column);

            }
         }
         return body;
      }


      /// <summary>
      /// Fills document body object with formatted reinforcement calculation results
      /// </summary>
      /// <param name="body">DocumentBody to be filled</param>
      /// <param name="linearElementResult">Calculation results schema class</param>
      /// <param name="document">Active Revit document</param>
      /// <param name="elementType">Structural element type</param>
      private void WriteTheoreticalReinfToNote(DocumentBody body, ResultLinearElement linearElementResult, Autodesk.Revit.DB.Document document, ElementType elementType)
      {
         /////////////////////////// Prepare data
         // Get revit units
         Units units = document.GetUnits();

         // Get raw calculation results as a collection of ResultInPointLinear objects for all element sections ( one ResultInPointLinear per section)
         List<ResultInPointLinear> resultsInPoints = linearElementResult.GetResultsInPoints();

         if (resultsInPoints.Count == 0)
            return;

         /////////////////////////// Add a general reinforcement review to the body( mean and extreme values of reinfocement )

         // Create document section
         DocumentSection resultSummary = new DocumentSection(Resources.ResourceManager.GetString("Summary"), 5);
         // Calculate mean and extreme values
         IEnumerable<ResultTypeLinear> longRnf = new List<ResultTypeLinear> { ResultTypeLinear.Atop, ResultTypeLinear.Abottom, ResultTypeLinear.Aleft, ResultTypeLinear.Aright };
         double maxLongitudinal = resultsInPoints.Max(s => (longRnf.Sum(q => s[q]))),
                minLongitudinal = resultsInPoints.Min(s => (longRnf.Sum(q => s[q]))),
                maxSpacing = resultsInPoints.Max(s => s[ResultTypeLinear.StirrupsSpacing]),
                minSpacing = resultsInPoints.Min(s => s[ResultTypeLinear.StirrupsSpacing]),
                maxX = resultsInPoints.Max(s => s[ResultTypeLinear.X]),
                minX = resultsInPoints.Min(s => s[ResultTypeLinear.X]),
                meanLongitudinal = resultsInPoints.Average(s => (longRnf.Sum(q => s[q]))),
                meanTransversal = resultsInPoints.Average(s => s[ResultTypeLinear.TransversalReinforcemenDensity]);

         // Add them to the section
         resultSummary.Body.Elements.Add((new DocumentValueWithName(Resources.ResourceManager.GetString("MeanReinforcemenDensityLongitudinal"), meanLongitudinal, UnitsConverter.GetInternalUnit(UnitType.UT_Reinforcement_Area), UnitType.UT_Reinforcement_Area, document.GetUnits())));
         resultSummary.Body.Elements.Add((new DocumentValueWithName(Resources.ResourceManager.GetString("MeanTransversalReinforcemenDensity"), meanTransversal, UnitsConverter.GetInternalUnit(UnitType.UT_Reinforcement_Area_per_Unit_Length), UnitType.UT_Reinforcement_Area_per_Unit_Length, document.GetUnits())));
         resultSummary.Body.Elements.Add((new DocumentValueWithName(Resources.ResourceManager.GetString("MaximumLongitudinalReinforcement"), maxLongitudinal, UnitsConverter.GetInternalUnit(UnitType.UT_Reinforcement_Area), UnitType.UT_Reinforcement_Area, document.GetUnits())));
         resultSummary.Body.Elements.Add((new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumLongitudinalReinforcement"), minLongitudinal, UnitsConverter.GetInternalUnit(UnitType.UT_Reinforcement_Area), UnitType.UT_Reinforcement_Area, document.GetUnits())));
         resultSummary.Body.Elements.Add((new DocumentValueWithName(Resources.ResourceManager.GetString("MaximumStirrupsSpacing"), maxSpacing, UnitsConverter.GetInternalUnit(UnitType.UT_Section_Dimension), UnitType.UT_Section_Dimension, document.GetUnits())));
         resultSummary.Body.Elements.Add((new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumStirrupsSpacing"), minSpacing, UnitsConverter.GetInternalUnit(UnitType.UT_Section_Dimension), UnitType.UT_Section_Dimension, document.GetUnits())));
         resultSummary.Level = DetailLevel.General;
         // Add section to the document body
         body.Elements.Add(resultSummary);



         /////////////////////////// Create result tables for deflection, internal forces and reinforcement

         // Create reinforcement table
         List<ResultTypeLinear> nonZeroReinforcement = ResultTypeLinearHelper.ReinforcementResults.Where(s => resultsInPoints.Any(r => Math.Abs(r[s]) > Double.Epsilon)).ToList();
         if (nonZeroReinforcement.Count() > 0)
         {
            DocumentSection section = CreateSectionTableAndDiagram(resultsInPoints, nonZeroReinforcement, document, elementType, true, false, false, Resources.ResourceManager.GetString("Reinforcement"));
            if (section != null)
               body.Elements.Add(section);
            string diagramTitle = "";
            bool reversedAxis = false;
            foreach (ResultTypeLinear resultType in nonZeroReinforcement)
            {
               switch (resultType)
               {
                  case ResultTypeLinear.Abottom:
                     diagramTitle = Resources.ResourceManager.GetString("LongReinforcementBottom");
                     break;
                  case ResultTypeLinear.Atop:
                     diagramTitle = Resources.ResourceManager.GetString("LongReinforcementTop");
                     break;
                  case ResultTypeLinear.Aleft:
                     diagramTitle = Resources.ResourceManager.GetString("LongReinforcementLeft");
                     break;
                  case ResultTypeLinear.Aright:
                     diagramTitle = Resources.ResourceManager.GetString("LongReinforcementRight");
                     break;
                  case ResultTypeLinear.TransversalReinforcemenDensity:
                     diagramTitle = Resources.ResourceManager.GetString("TransversalReinforcement");
                     break;
                  case ResultTypeLinear.StirrupsSpacing:
                     diagramTitle = Resources.ResourceManager.GetString("StirrupsSpacing");
                     break;
               }
               List<ResultTypeLinear> oneNonZeroReinforcement = new List<ResultTypeLinear>(1);
               oneNonZeroReinforcement.Add(resultType);
               section = CreateSectionTableAndDiagram(resultsInPoints, oneNonZeroReinforcement, document, elementType, false, true, reversedAxis, "", "", diagramTitle);
               if (section != null)
                  body.Elements.Add(section);
            }
         }
         // Create internal forces tables and graph
         List<ResultTypeLinear> nonZeroInternalForces = ResultTypeLinearHelper.InternalForcesResults.Where(s => resultsInPoints.Any(r => Math.Abs(r[s]) > Double.Epsilon)).ToList();
         if (nonZeroInternalForces.Count() > 0)
         {
            DocumentSection section = CreateSectionTableAndDiagram(resultsInPoints, nonZeroInternalForces, document, elementType, true, true, false, Resources.ResourceManager.GetString("InternalForces"), "", Resources.ResourceManager.GetString("Internal_Forces"));
            if (section != null)
               body.Elements.Add(section);
         }
         List<ResultTypeLinear> nonZeroInternalMoments = ResultTypeLinearHelper.InternalMomentsResults.Where(s => resultsInPoints.Any(r => Math.Abs(r[s]) > Double.Epsilon)).ToList();
         if (nonZeroInternalMoments.Count() > 0)
         {
            DocumentSection section = null;
            if (nonZeroInternalForces.Count() != 0)
               section = CreateSectionTableAndDiagram(resultsInPoints, nonZeroInternalMoments, document, elementType, true, true, true, "", "", Resources.ResourceManager.GetString("Internal_Moments"));
            else
               section = CreateSectionTableAndDiagram(resultsInPoints, nonZeroInternalMoments, document, elementType, true, true, true, Resources.ResourceManager.GetString("InternalForces"), "", Resources.ResourceManager.GetString("Internal_Moments"));
            if (section != null)
               body.Elements.Add(section);
         }

         // Create deflection table and graph
         List<ResultTypeLinear> nonZeroDeflection = ResultTypeLinearHelper.RealDeflectionResults.Where(s => resultsInPoints.Any(r => Math.Abs(r[s]) > Double.Epsilon)).ToList();
         if (nonZeroDeflection.Count() > 0)
         {
            List<ResultTypeLinear> displacement = new List<ResultTypeLinear>(nonZeroDeflection.Count());
            List<ResultTypeLinear> displacementAndDeflection = new List<ResultTypeLinear>(nonZeroDeflection.Count() * 2);
            displacementAndDeflection.AddRange(nonZeroDeflection);
            foreach (ResultTypeLinear resultType in nonZeroDeflection)
            {
               switch (resultType)
               {
                  case ResultTypeLinear.UxRealMax:
                     displacement.Add(ResultTypeLinear.UxMax);
                     break;
                  case ResultTypeLinear.UxRealMin:
                     displacement.Add(ResultTypeLinear.UxMin);
                     break;
                  case ResultTypeLinear.UyRealMax:
                     displacement.Add(ResultTypeLinear.UyMax);
                     break;
                  case ResultTypeLinear.UyRealMin:
                     displacement.Add(ResultTypeLinear.UyMin);
                     break;
                  case ResultTypeLinear.UzRealMax:
                     displacement.Add(ResultTypeLinear.UzMax);
                     break;
                  case ResultTypeLinear.UzRealMin:
                     displacement.Add(ResultTypeLinear.UzMin);
                     break;
               }
            }
            displacementAndDeflection.AddRange(displacement);
            DocumentSection section = CreateSectionTableAndDiagram(resultsInPoints, displacementAndDeflection, document, elementType, true, false, false, Resources.ResourceManager.GetString("Deflection"), "", Resources.ResourceManager.GetString("DeflectionEnvelope"));
            if (section != null)
            {
               body.Elements.Add(section);
               section = CreateSectionTableAndDiagram(resultsInPoints, nonZeroDeflection, document, elementType, false, true, false, "", "", Resources.ResourceManager.GetString("DeflectionEnvelope"));
               if (section != null)
               {
                  body.Elements.Add(section);
                  section = CreateSectionTableAndDiagram(resultsInPoints, displacement, document, elementType, false, true, false, "", "", Resources.ResourceManager.GetString("DisplacementEnvelope"));
                  if (section != null)
                     body.Elements.Add(section);
               }
            }
         }
      }
      static private Dictionary<ResultTypeLinear, Color> ResultTypeLinearToColorDictionary = new Dictionary<ResultTypeLinear, Color>()
      {
         {ResultTypeLinear.X,new Color              (0,   255,  0     ) },
         {ResultTypeLinear.X_Rel,new Color          (225, 0,    0     ) },

         {ResultTypeLinear.MxMax,new Color          (0,   0,    255   ) },
         {ResultTypeLinear.MxMin,new Color          (255, 119,  28    ) },
         {ResultTypeLinear.MyMin,new Color          (51,  177,  33    ) },
         {ResultTypeLinear.MyMax,new Color          (163, 73,   180   ) },
         {ResultTypeLinear.MzMin,new Color          (159, 99,   66    ) },
         {ResultTypeLinear.MzMax,new Color          (0,   128,  128   ) },

         {ResultTypeLinear.FxMax,new Color          (0,   0,    255   ) },
         {ResultTypeLinear.FxMin,new Color          (255, 119,  28    ) },
         {ResultTypeLinear.FyMin,new Color          (51,  177,  33    ) },
         {ResultTypeLinear.FyMax,new Color          (163, 73,   180   ) },
         {ResultTypeLinear.FzMin,new Color          (159, 99,   66    ) },
         {ResultTypeLinear.FzMax,new Color          (0,   128,  128   ) },

         {ResultTypeLinear.Abottom,new Color        (0,   0,    255   ) },
         {ResultTypeLinear.Atop,new Color           (255, 119,  28    ) },
         {ResultTypeLinear.Aleft,new Color          (51,  177,  33    ) },
         {ResultTypeLinear.Aright,new Color         (163, 73,   180   ) },
         {ResultTypeLinear.StirrupsSpacing,new Color(159, 99,   66    ) },
         {ResultTypeLinear.TransversalReinforcemenDensity,
                                           new Color(0,   128,  128   ) },

         {ResultTypeLinear.UxMax,new Color          (0,   0,    255   ) },
         {ResultTypeLinear.UxMin,new Color          (255, 119,  28    ) },
         {ResultTypeLinear.UyMax,new Color          (51,  177,  33    ) },
         {ResultTypeLinear.UyMin,new Color          (163, 73,   180   ) },
         {ResultTypeLinear.UzMax,new Color          (159, 99,   66    ) },
         {ResultTypeLinear.UzMin,new Color          (0,   128,  128   ) },

         {ResultTypeLinear.UxRealMax,new Color      (0,   0,    255   ) },
         {ResultTypeLinear.UxRealMin,new Color      (255, 119,  28    ) },
         {ResultTypeLinear.UyRealMax,new Color      (51,  177,  33    ) },
         {ResultTypeLinear.UyRealMin,new Color      (163, 73,   180   ) },
         {ResultTypeLinear.UzRealMax,new Color      (159, 99,   66    ) },
         {ResultTypeLinear.UzRealMin,new Color      (0,   128,  128   ) },

      };
      /// <summary>
      /// Creats the section of document with table
      /// </summary>
      /// <param name="resultsInPoints"> Result in point collection</param>
      /// <param name="resultTypes">The type of results that will be displayed in the table.</param>
      /// <param name="document">Active Revit document</param>
      /// <returns>Returns section of document with table</returns>
      private DocumentTable CreateTable(ICollection<ResultInPointLinear> resultsInPoints, List<ResultTypeLinear> resultTypes, Autodesk.Revit.DB.Document document)
      {
         Units units = document.GetUnits();
         resultTypes.Insert(0, ResultTypeLinear.X);
         DocumentTable resultTable = new DocumentTable(resultsInPoints.Count + 1, resultTypes.Count());
         int colId = 0;
         resultTable.HeaderColumnsCount = 1;
         resultTable.HeaderRowsCount = 1;
         foreach (ResultTypeLinear resultType in resultTypes)
         {
            string description = Resources.ResourceManager.GetString(resultType.ToString());
            resultTable[0, colId].Elements.Add(new DocumentText(description));
            int rowId = 1;
            UnitType unitType = resultType.GetUnitType();
            DisplayUnitType displayUnitType = Utility.UnitsConverter.GetInternalUnit(unitType);
            foreach (ResultInPointLinear resultInPoint in resultsInPoints)
            {
               if (DisplayUnitType.DUT_UNDEFINED != displayUnitType)
                  resultTable[rowId++, colId].Elements.Add(new DocumentValue(resultInPoint[resultType], displayUnitType, unitType, units));
               else
               {
                  resultTable[rowId++, colId].Elements.Add(new DocumentText(String.Format("{0:0.00}", resultInPoint[resultType])));
               }
            }
            colId++;
         }
         resultTypes.Remove(ResultTypeLinear.X);
         return resultTable;
      }
      /// <summary>
      /// Creats the section of document with diagram
      /// </summary>
      /// <param name="title">Title of the diagram</param>
      /// <param name="resultsInPoints"> Result in point collection</param>
      /// <param name="resultTypes">The type of results that will be displayed in the diagram.</param>
      /// <param name="document">Active Revit document</param>
      /// <param name="elementType">Structural element type</param>
      /// <param name="reversedAxis">Sets the direction of the Y axis on the graph</param>
      /// <returns>Returns section of document with diagram</returns>
      private DocumentDiagram CreateDiagram(String title, IEnumerable<ResultInPointLinear> resultsInPoints, List<ResultTypeLinear> resultTypes, Autodesk.Revit.DB.Document document, ElementType elementType, bool reversedAxis)
      {
         Units units = document.GetUnits();
         DocumentDiagram chart = new DocumentDiagram();
         if (resultTypes != null)
         {
            if (resultTypes.Count() > 0)
            {
               if (title.Length != 0)
                  chart.Title = title;
               chart.Height = 200;
               chart.Width = 4 * chart.Height;

               UnitType unitTypeX = ResultTypeLinear.X.GetUnitType();
               FormatOptions formatOptions = units.GetFormatOptions(unitTypeX);
               DisplayUnitType displayUnitTypeX = formatOptions.DisplayUnits;
               DisplayUnitType internalDisplayUnitTypeX = UnitsConverter.GetInternalUnit(unitTypeX);
               UnitSymbolType unitSymbolTypeX = formatOptions.UnitSymbol;
               String axisTitle = "";
               if (ElementType.Column == elementType)
                  axisTitle = Resources.ResourceManager.GetString("AxisHeight");
               else
                  axisTitle = Resources.ResourceManager.GetString("AxisLength");
               if (UnitSymbolType.UST_NONE != unitSymbolTypeX)
               {
                  axisTitle += " " + LabelUtils.GetLabelFor(unitSymbolTypeX);
               }
               chart.AxisX.Title = axisTitle;
               chart.AxisY.Reversed = reversedAxis;
               UnitType unitTypeY = resultTypes.First().GetUnitType();
               formatOptions = units.GetFormatOptions(unitTypeY);
               DisplayUnitType displayUnitTypeY = formatOptions.DisplayUnits;
               DisplayUnitType internalDisplayUnitTypeY = UnitsConverter.GetInternalUnit(unitTypeY);
               UnitSymbolType unitSymbolTypeY = formatOptions.UnitSymbol;
               switch (unitTypeY)
               {
                  case UnitType.UT_Reinforcement_Area:
                     axisTitle = Resources.ResourceManager.GetString("AxisArea");
                     break;
                  case UnitType.UT_Force:
                     axisTitle = Resources.ResourceManager.GetString("AxisForce");
                     break;
                  case UnitType.UT_Moment:
                     axisTitle = Resources.ResourceManager.GetString("AxisMoment");
                     break;
                  case UnitType.UT_Displacement_Deflection:
                     axisTitle = Resources.ResourceManager.GetString("AxisDisplacmentDeflection");
                     break;
                  case UnitType.UT_Section_Dimension:
                     axisTitle = Resources.ResourceManager.GetString("AxisSpacing");
                     break;
                  case UnitType.UT_Reinforcement_Area_per_Unit_Length:
                     axisTitle = Resources.ResourceManager.GetString("AxisDencity");
                     break;
                  default:
                     break;
               }
               if (UnitSymbolType.UST_NONE != unitSymbolTypeY)
               {
                  axisTitle += " " + LabelUtils.GetLabelFor(unitSymbolTypeY);
               }
               chart.AxisY.Title = axisTitle;
               double valX = 0;
               double valY = 0;
               foreach (ResultTypeLinear resultType in resultTypes)
               {
                  DocumentDiagramSeries series = new DocumentDiagramSeries(resultType.ToString());
                  chart.Legend = true;
                  chart.Series.Add(series);
                  series.Color = ResultTypeLinearToColorDictionary[resultType];
                  series.Name = Resources.ResourceManager.GetString(resultType.ToString());
                  int lblNbr = 5;
                  int lblN = 0;
                  int lblStep = resultsInPoints.Count<ResultInPointLinear>() / lblNbr;
                  // Labels on the X axis will be formatted according to Revit preferences
                  //formatOptions = units.GetFormatOptions(unitTypeX);
                  //string accuracy = formatOptions.Accuracy.ToString();
                  //accuracy = accuracy.Replace("1", "0");
                  string valXAsString = "";
                  foreach (ResultInPointLinear resultInPoint in resultsInPoints)
                  {
                     valX = Autodesk.Revit.DB.UnitUtils.Convert(resultInPoint[ResultTypeLinear.X], internalDisplayUnitTypeX, displayUnitTypeX);
                     valY = Autodesk.Revit.DB.UnitUtils.Convert(resultInPoint[resultType], internalDisplayUnitTypeY, displayUnitTypeY);
                     series.AddXY(valX, valY);
                     if (lblStep == 0 || lblN % lblStep == 0)
                     {
                        //DocumentValue docVal = new DocumentValue(resultInPoint[ResultTypeLinear.X], displayUnitTypeX, unitTypeX, units);
                        DocumentValue docVal = new DocumentValue(valX, displayUnitTypeX, unitTypeX, units);
                        valXAsString = (string)docVal.Value;
                        chart.AxisX.Labels.Add(new DocumentDiagramAxisLabel(valX, valXAsString));
                     }
                     lblN++;
                  }
               }
            }
         }
         return chart;
      }
      /// <summary>
      /// Creats document section with table and graph
      /// </summary>
      /// <param name="sectionTitle">Title of the section</param>
      /// <param name="resultsInPoints"> Result in point collection</param>
      /// <param name="resultTypes">The type of results that will be displayed in the diagram and in the table.</param>
      /// <param name="document">Active Revit document</param>
      /// <param name="elementType">Structural element type</param>
      /// <param name="addTable">Enabling/disabling inserting a table</param>
      /// <param name="addDiagram">Enabling/disabling inserting a graph</param>
      /// <param name="tableTitle"> Title of the table</param>
      /// <param name="diagramTitle"> Title of the graph</param>
      /// <param name="reversedAxis">Sets the direction of the Y axis on the graph</param>
      /// <returns>Returns section of document with diagram and table</returns>
      private DocumentSection CreateSectionTableAndDiagram(ICollection<ResultInPointLinear> resultsInPoints, List<ResultTypeLinear> resultTypes, Autodesk.Revit.DB.Document document, ElementType elementType, bool addTable = true, bool addDiagram = true, bool reversedAxis = false, String sectionTitle = "", String tableTitle = "", String diagramTitle = "")
      {
         DocumentSection newDocumentSection = null;
         if (addTable || addDiagram)
         {
            newDocumentSection = new DocumentSection(sectionTitle, 5);
            newDocumentSection.Level = DetailLevel.Medium;
            if (addTable)
            {
               DocumentSection tableSection = new DocumentSection(tableTitle, 6);
               DocumentTable resultTable = CreateTable(resultsInPoints, resultTypes, document);
               resultTable.Level = DetailLevel.Medium;
               newDocumentSection.Body.Elements.Add(resultTable);
            }
            if (addDiagram)
            {
               DocumentSection diagramSection = new DocumentSection();
               DocumentDiagram resultDiagram = CreateDiagram(diagramTitle, resultsInPoints, resultTypes, document, elementType, reversedAxis);
               resultDiagram.Level = DetailLevel.Detail;
               newDocumentSection.Body.Elements.Add(new DocumentLineBreak(2));
               newDocumentSection.Body.Elements.Add(resultDiagram);
            }
         }
         return newDocumentSection;
      }

      public override void Verify(Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         Autodesk.Revit.DB.CodeChecking.Storage.StorageService service = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService();
         Autodesk.Revit.DB.CodeChecking.Storage.StorageDocument storageDocument = service.GetStorageDocument(data.Document);


         Main.Calculation.EngineData enginData = new Main.Calculation.EngineData();
         Engine.Engine engine = new Engine.Engine(enginData);

         engine.Calculate(this, data);


      }

      public override IList<BuiltInCategory> GetSupportedCategories(StructuralAssetClass material)
      {
         List<BuiltInCategory> supportedCategories = new List<BuiltInCategory>();
         supportedCategories.Add(BuiltInCategory.OST_BeamAnalytical);
         supportedCategories.Add(BuiltInCategory.OST_ColumnAnalytical);
         return supportedCategories;
      }

      public override IList<Autodesk.Revit.DB.StructuralAssetClass> GetSupportedMaterials()
      {
         return new List<Autodesk.Revit.DB.StructuralAssetClass>(){
                Autodesk.Revit.DB.StructuralAssetClass.Concrete};
      }

      #endregion

      /// <summary>
      /// Gets the document body for label.
      /// </summary>
      /// <param name="label">label.</param>
      /// <param name="document">document.</param>
      /// <returns>
      /// The body of the document.
      /// </returns>
      public override DocumentBody BuildLabelDocumentBody(Entity label, Autodesk.Revit.DB.Document document)
      {
         switch (label.Schema.SchemaName)
         {
            default:
               return base.BuildLabelDocumentBody(label, document);
            case "LabelColumn":
               {
                  DocumentBody body = new DocumentBody();
                  LabelColumn labelColumn = new LabelColumn();
                  labelColumn.SetProperties(label, document);

                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("CalculationOptions"), 4));

                  if (labelColumn.EnabledInternalForces.Count > 0)
                  {
                     body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("EnabledInternalForces") + ":", true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnFX_Note"), true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FY))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnFY_Note"), true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FZ))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnFZ_Note"), true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnMX_Note"), true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MY))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnMY_Note"), true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MZ))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnMZ_Note"), true));
                  }

                  body.Elements.Add(DocumentElement.GetDocumentElement("CreepCoefficient", labelColumn, this, document));

                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("Buckling"), 4));
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("BucklingDirectionY"), 5));
                  if (labelColumn.BucklingDirectionY)
                  {
                     body.Elements.Add(DocumentElement.GetDocumentElement("LengthCoefficientY", labelColumn, this, document));
                     body.Elements.Add(DocumentElement.GetDocumentElement("ColumnStructureTypeY", labelColumn, this, document));
                  }
                  else
                  {
                     body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("NotTakenIntoConsideration"), true));
                  }


                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("BucklingDirectionZ"), 5));
                  if (labelColumn.BucklingDirectionZ)
                  {
                     body.Elements.Add(DocumentElement.GetDocumentElement("LengthCoefficientZ", labelColumn, this, document));
                     body.Elements.Add(DocumentElement.GetDocumentElement("ColumnStructureTypeZ", labelColumn, this, document));
                  }
                  else
                  {
                     body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("NotTakenIntoConsideration"), true));
                  }


                  // longitudinal
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("LongitudinalReinforcement"), 4));
                  if (labelColumn.LongitudinalReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(labelColumn.LongitudinalReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), labelColumn.LongitudinalReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (labelColumn.LongitudinalReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), labelColumn.LongitudinalReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), labelColumn.LongitudinalReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), labelColumn.LongitudinalReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));

                  // transverse
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("TransversalReinforcement"), 4));
                  if (labelColumn.TransversalReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(labelColumn.TransversalReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), labelColumn.TransversalReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (labelColumn.TransversalReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), labelColumn.TransversalReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), labelColumn.TransversalReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), labelColumn.TransversalReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));


                  return body;
               }
            case "LabelBeam":
               {
                  DocumentBody body = new DocumentBody();
                  LabelBeam labelBeam = new LabelBeam();
                  labelBeam.SetProperties(label, document);


                  body.Elements.Add(new DocumentLineBreak(1));
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("CalculationOptions"), 4));

                  if (labelBeam.EnabledInternalForces.Count > 0)
                  {
                     body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("EnabledInternalForces") + ":", true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamFX_Note"), true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FY))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamFY_Note"), true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FZ))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamFZ_Note"), true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamMX_Note"), true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MY))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamMY_Note"), true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MZ))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamMZ_Note"), true));
                  }

                  body.Elements.Add(DocumentElement.GetDocumentElement("CreepCoefficient", labelBeam, this, document));

                  string interaction = Resources.ResourceManager.GetString("No");
                  if (labelBeam.SlabBeamInteraction == ConcreteTypes.BeamSectionType.WithSlabBeamInteraction)
                  {
                     interaction = Resources.ResourceManager.GetString("Yes");
                  }

                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("SlabBeamInteraction"), interaction));

                  // longitudinal
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("LongitudinalReinforcement"), 4));
                  if (labelBeam.LongitudinalReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(labelBeam.LongitudinalReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), labelBeam.LongitudinalReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (labelBeam.LongitudinalReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), labelBeam.LongitudinalReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), labelBeam.LongitudinalReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), labelBeam.LongitudinalReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));

                  // transverse
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("TransversalReinforcement"), 4));
                  if (labelBeam.TransversalReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(labelBeam.TransversalReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), labelBeam.TransversalReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (labelBeam.TransversalReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), labelBeam.TransversalReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), labelBeam.TransversalReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), labelBeam.TransversalReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));

                  return body;
               }
         }
      }

      /// <summary>
      /// Gets the document body for calculation parameters.
      /// </summary>
      /// <param name="calcParams">Calculation parameters</param>
      /// <param name="document">document</param>
      /// <returns>Body of the document</returns>
      public override DocumentBody BuildCalculationParamDocumentBody(Entity calcParams, Autodesk.Revit.DB.Document document)
      {
         DocumentBody body = new DocumentBody();

         return body;
      }




      #region IExternalServer Members

      public override string GetDescription()
      {
         return "Code Checking Concrete Example for Revit API";
      }

      public override string GetName()
      {
         return "CodeCheckingConcreteExample";
      }

      public override Guid GetServerId()
      {
         return ID;
      }

      public override string GetVendorId()
      {
         return "ADSK";
      }


      /// <summary>
      /// Gets the output package result types. This information is needed to create proper ResultsBuilder Package.
      /// </summary>
      /// <returns></returns>
      public override Autodesk.Revit.DB.ResultsBuilder.ResultsPackageTypes GetOutputPackageResultTypes()
      {
         return Autodesk.Revit.DB.ResultsBuilder.ResultsPackageTypes.RequiredReinforcement | Autodesk.Revit.DB.ResultsBuilder.ResultsPackageTypes.SteelRatio;
      }
      /// <summary>
      /// Gets the output package unit system. This information is needed to create proper ResultsBuilder Package.
      /// </summary>
      /// <returns></returns>
      public override Autodesk.Revit.DB.ResultsBuilder.UnitsSystem GetOutputPackageUnitSystem()
      {
         return UnitSystem;
      }


      #endregion

      #region ICodeCheckingServerDocumentation Members

      public override string GetResource(string key, string context)
      {
         string txt = Resources.ResourceManager.GetString(key);

         if (!string.IsNullOrEmpty(txt))
            return txt;

         return key;
      }

      #endregion
   }
}
