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
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using CodeCheckingConcreteExample.Utility;

namespace CodeCheckingConcreteExample.Main
{
   /// <summary>
   /// Container for surface element results data
   /// </summary>
   [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.Schema("ResultSurface", "37147943-1A9A-44E8-8BAC-20628BC26896")]
   public class ResultSurfaceElement : Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass
   {
      /// <summary>
      /// Collection of values representing element results in raw format
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(Unit = Autodesk.Revit.DB.UnitType.UT_Length, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_METERS)]
      public List<Double> ValuesInPointsData { get; set; }

      /// <summary>
      /// Information that the surface object is multilayer.
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty()]
      public bool MultiLayer { get; set; }

      /// <summary>
      /// Collection of layer thickness
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty(Unit = Autodesk.Revit.DB.UnitType.UT_Length, DisplayUnit = Autodesk.Revit.DB.DisplayUnitType.DUT_METERS)]
      public List<Double> StructuralLayersThickness { get; set; }

      /// <summary>
      /// Collection of layer materials name
      /// </summary>
      [Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes.SchemaProperty()]
      public List<String> StructuralLayersMaterialName { get; set; }

      /// <summary>
      /// Informationa about structural layers for multilayers object
      /// </summary>
      /// <returns>List of names and thickness of structural layers</returns>
      public List<Tuple<string, double>> GetStructuralLayers()
      {
         int numberOfLayers = StructuralLayersThickness.Count();
         if (numberOfLayers != StructuralLayersMaterialName.Count())
            throw new Exception("Invalid layers properties");

         List<Tuple<string, double>> layers = new List<Tuple<string, double>>();
         for (int layersId = 0; layersId < numberOfLayers; layersId++)
         {
            layers.Add(new Tuple<string, double>(StructuralLayersMaterialName[layersId],StructuralLayersThickness[layersId]));
         }
         return layers;
      }
      /// <summary>
      /// Removing information about all structural layers
      /// </summary>
      public void ClearStructuralLayers()
      {
         StructuralLayersThickness.Clear();
         StructuralLayersMaterialName.Clear();
      }
      /// <summary>
      /// Adds new structural layer
      /// </summary>
      /// <param name="materialName">Material name for layer</param>
      /// <param name="thickness">Thickness of layer</param>
      public void AddStructuralLayer(string materialName, double thickness)
      {
         StructuralLayersThickness.Add(thickness);
         StructuralLayersMaterialName.Add(materialName);
      }
      
      /// <summary>
      /// Gets results in point formatted as a list of ResultInPointSurface elements
      /// </summary>
      /// <returns>List of ResultInPointSurface</returns>
      public List<ResultInPointSurface> GetResultsInPointsCollection()
      {
         IEnumerable<ResultTypeSurface> vType = Enum.GetValues(typeof(ResultTypeSurface)).OfType<ResultTypeSurface>();

         int numberOfValsInPoint = vType.Count(),
             numberOfPoints = ValuesInPointsData.Count / numberOfValsInPoint;

         List<ResultInPointSurface> resultsInPoints = new List<ResultInPointSurface>();
         for (int ptId = 0; ptId < numberOfPoints; ptId++)
         {
            resultsInPoints.Add(new ResultInPointSurface(ValuesInPointsData.GetRange(ptId * numberOfValsInPoint, numberOfValsInPoint)));
         }

         return resultsInPoints;
      }

      /// <summary>
      /// Creates default ResultSurfaceElement
      /// </summary>
      public ResultSurfaceElement()
      {
         ValuesInPointsData = new List<double>();
         StructuralLayersThickness = new List<double>();
         StructuralLayersMaterialName = new List<string>();
         MultiLayer = false;
      }
   }
}
