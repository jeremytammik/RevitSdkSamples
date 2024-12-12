//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ResultsBuilder;
using Autodesk.Revit.DB.ResultsBuilder.Storage;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.QueryingResults.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   class QueryResults : IExternalCommand
   {
      #region Class Members
      // Ids of elements used in this example
      // ElementIds are hardcoded, so this example has to be used with the included "ResultsInRevit.rvt" file
      ElementId linearElementId = new ElementId(262075);
      ElementId surfaceElementId = new ElementId(263074);

      // Reference to the Revit document
      Document document;
      #endregion

      #region Class Interface Implementation

      /// <summary>
      /// The top level command.
      /// </summary>
      /// <param name="commandData">An object that is passed to the external application 
      /// which contains data related to the command, 
      /// such as the application object and active view.</param>
      /// <param name="message">A message that can be set by the external application 
      /// which will be displayed if a failure or cancellation is returned by 
      /// the external command.</param>
      /// <param name="elements">A set of elements to which the external application 
      /// can add elements that are to be highlighted in case of failure or cancellation.</param>
      /// <returns>Return the status of the external command. 
      /// A result of Succeeded means that the API external method functioned as expected. 
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         // Set reference to the document and then check if this command is run on the inclueded "ResultsInRevit.rvt" file
         document = commandData.Application.ActiveUIDocument.Document;
         String outputDocumentPathName = commandData.Application.ActiveUIDocument.Document.PathName + ".ECTesting.txt";

         Autodesk.Revit.UI.Result res = Result.Failed;

         if (document.Title != "ResultsInRevit.rvt")
         {
            // Report error if this is a wrong file
            Autodesk.Revit.UI.TaskDialog.Show("Wrong Revit file", "Cannot use :\n" + document.PathName + "\n\nPlease use attached " + "\"ResultsInRevit.rvt\" file.");
         }
         else
         {
            // Go ahead and read some results from elements otherwise
            try
            {
               // Set ElementIds for Revit elements and loads used in this example. ElementIds are hardcoded
               ElementId loadCaseId1 = new ElementId(37232);
               ElementId loadCaseId2 = new ElementId(37234);
               ElementId combinationId = new ElementId(261966);
               ElementId []loadCaseIds = new ElementId[] { loadCaseId1, loadCaseId2, combinationId };

               ResultsPackage resultsPackage = null;
               try
               {
                  // Get results package
                  resultsPackage = GetResultsPackage(document);
               }
               catch
               {
                  Autodesk.Revit.UI.TaskDialog.Show("Cannot get result package.", "\nTry to store results first.");
               }

               // Read Results from the created result package and store them in the textfile
               using (System.IO.StreamWriter writer = new System.IO.StreamWriter(outputDocumentPathName, false))
               {
                  writer.WriteLine(ReadLinearResults(resultsPackage, linearElementId, loadCaseIds));
                  writer.WriteLine(ReadArbitraryLinearResults(resultsPackage, linearElementId));
                  writer.WriteLine(ReadSurfaceResults(resultsPackage, surfaceElementId, loadCaseIds));
                  writer.WriteLine(ReadArbitrarySurfaceResults(resultsPackage, surfaceElementId));
               }
               res = Result.Succeeded;
            }
            catch (Exception ex)
            {
               Autodesk.Revit.UI.TaskDialog.Show("Failed to read results from Revit", ex.Message.ToString());
            }
         }
        
         return res;
      }
      #endregion

      #region Class Implementation

      /// <summary>
      /// Gets result package by getting Result Access object from the document, then querying it for a specific name ( "ResultsInRevit" )
      /// </summary>
      /// <param name="doc">Revit document</param>
      /// <returns>ResultsPackage containig results created in the StoringResults example</returns>
      ResultsPackage GetResultsPackage(Document doc)
      {
         ResultsAccess results = ResultsAccess.CreateResultsAccess(doc);
         return results.ResultsPackages.First(s => s.Name == "ResultsInRevit");
      }

      /// <summary>
      /// Reads linear analitical results for a specific element, for a set of load cases
      /// </summary>
      /// <param name="resultsPackage">Package from which to results are to be read</param>
      /// <param name="elementId">Revit Element id</param>
      /// <param name="loadCaseIds">Collection of load case Ids</param>
      /// <returns>Results formatted into a string</returns>
      private String ReadLinearResults( ResultsPackage resultsPackage, ElementId elementId, ElementId []loadCaseIds)
      {
         // Set a list of result type elements
         IList<LinearResultType> linearResultTypes = new List<LinearResultType>() { LinearResultType.Fx, LinearResultType.Fy, LinearResultType.Fz, LinearResultType.Mx, LinearResultType.My, LinearResultType.Mz };
         List<ElementId> elementIds = new List<ElementId>() { elementId };

         // Read results from the package
         IList<LineGraph> graphs = resultsPackage.GetLineGraphs(elementIds, loadCaseIds, linearResultTypes, new LineGraphParameters( UnitsSystem.Metric ) );

         // Format results
         return FormatLinearResultsOutput( "Linear results", graphs);
      }

      /// <summary>
      /// Reads linear arbitrary results for a specific element in a similar way to analitical results
      /// No load cases are used as arbitrary results are load independent
      /// </summary>
      /// <param name="resultsPackage">Package from which to results are to be read</param>
      /// <param name="elementId">Revit Element id</param>
      /// <returns>Results formatted into a string</returns>
      private String ReadArbitraryLinearResults(ResultsPackage resultsPackage, ElementId elementId)
      {
         IList<String> rnfTypes = new List<String>() { "RnfBarsAngleBottom", "RnfBarsAngleTop", "RnfBarsAngleLeft", "RnfBarsAngleRight" };
         List<ElementId> elementIds = new List<ElementId>() { elementId };
         List<ElementId> loadCaseIds = new List<ElementId>() { null };

         IList<LineGraph> graphs = resultsPackage.GetLineGraphs(elementIds, loadCaseIds, rnfTypes, new LineGraphParameters(UnitsSystem.Metric));

         return FormatLinearResultsOutput( "Linear arbitrary results", graphs);
      }

      /// <summary>
      /// Formats linear results into a string
      /// </summary>
      /// <param name="title">Title of the results group</param>
      /// <param name="graphs">Results - collection of LineGraph elements</param>
      /// <returns>String containing formatted results</returns>
      private String FormatLinearResultsOutput( String title, IList<LineGraph> graphs)
      { 
         // String object
         StringBuilder res = new StringBuilder();

         res.AppendLine();
         // Add header(title)
         res.AppendLine("Result type: " + title);
         res.AppendLine();
         // Add domain
         res.AppendLine( "X coordinates: ");
         foreach( double xCoord in graphs[0].Points.Select(s=>s.U))
         {
            res.Append(xCoord);
         }

         // Group results using loadId
         var loads = graphs.GroupBy(s => s.LoadId);
         // Iterate over groups
         foreach (IGrouping<ElementId,LineGraph> load in loads)
         {
            res.AppendLine();
            res.AppendLine();
            // Get and add to the string Load info for each group( Load name and load type(Combination/LoadCase))
            if (load.Key != null)
            {
               res.AppendLine(GetLoadInfoForGraph(load.Key));
            }
            // Iterate over all results in the current group
            foreach (LineGraph lineGraph in load)
            {
               res.AppendLine();
               // Get and add result type
               res.AppendLine("Results: " +lineGraph.ResultType.ToString());
               // Add all values for the current result
               foreach( double value in lineGraph.Points.Select(s=>s.V))
               {
                  res.Append( String.Format("{0:0.000},", value));
               }
            }
         }
         return res.ToString();
      }

      /// <summary>
      /// Reads surface analitical results for a specific element, for a set of load cases
      /// </summary>
      /// <param name="resultsPackage">Package from which to results are to be read</param>
      /// <param name="elementId">Revit Element id</param>
      /// <param name="loadCaseIds">Collection of load case Ids</param>
      /// <returns>Results formatted into a string</returns>
      private String ReadSurfaceResults( ResultsPackage resultsPackage, ElementId elementId, ElementId []loadCaseIds)
      {
         // Set a list of result type elements
         SurfaceResultType[] surfaceResultTypes = { SurfaceResultType.Fxx, SurfaceResultType.Fyy, SurfaceResultType.Fxy, SurfaceResultType.Mxx, SurfaceResultType.Myy, SurfaceResultType.Mxy };
         List<ElementId> elementIds = new List<ElementId>() { elementId };

         // Read results from the package
         IList<SurfaceGraph> graphs = resultsPackage.GetSurfaceGraphs(elementIds, loadCaseIds, surfaceResultTypes, new SurfaceGraphParameters(UnitsSystem.Metric));

         // Format results
         return FormatSurfaceResultsOutput( "Surface results", graphs);
      }

      /// <summary>
      /// Reads surface arbitrary results for a specific element in a similar way to analitical results
      /// </summary>
      /// <param name="resultsPackage">Package from which to results are to be read</param>
      /// <param name="elementId">Revit Element id</param>
      /// <returns>Results formatted into a string</returns>
      private String ReadArbitrarySurfaceResults( ResultsPackage resultsPackage, ElementId elementId)
      {
         String[] surfaceResultTypes = { "AxxBottom", "AyyBottom", "AxxTop", "AyyTop" };

         List<ElementId> elementIds = new List<ElementId>() { elementId };
         List<ElementId> loadCaseIds = new List<ElementId>() { null };

         IList<SurfaceGraph> graphs = resultsPackage.GetSurfaceGraphs(elementIds, loadCaseIds, surfaceResultTypes, new SurfaceGraphParameters(UnitsSystem.Metric));

         return FormatSurfaceResultsOutput("Linear arbitrary results", graphs);
      }

      /// <summary>
      /// Formats surface results into a string
      /// </summary>
      /// <param name="title">Title of the results group</param>
      /// <param name="graphs">Results - collection of SurfaceGraph elements</param>
      /// <returns>String containing formatted results</returns>
      private String FormatSurfaceResultsOutput( String title, IList<SurfaceGraph> graphs)
      {
         StringBuilder res = new StringBuilder();

         res.AppendLine();
         // Add header(title)
         res.AppendLine("Result type: " + title);
         res.AppendLine();
         // Add domain
         res.AppendLine("Points coordinates (x,y,z): ");
         foreach (XYZ coord in graphs[0].Points )
         {
            res.Append("(" + String.Format("{0:0.00},", coord.X) + String.Format("{0:0.00},", coord.Y) + String.Format("{0:0.00},", coord.Z) + ") ");
         }

         // Group results using loadId
         var loads = graphs.GroupBy(s => s.LoadId);
         // Iterate over groups
         foreach (IGrouping<ElementId, SurfaceGraph> load in loads)
         {
            res.AppendLine();
            res.AppendLine();
            // Get and add to the string Load info for each group( Load name and load type(Combination/LoadCase))
            if (load.Key != null)
            {
               res.AppendLine(GetLoadInfoForGraph(load.Key));
            }
            // Iterate over all results in the current group
            foreach (SurfaceGraph surfaceGraph in load)
            {
               res.AppendLine();
               // Get and add result type
               res.AppendLine("Results: " + surfaceGraph.ResultType.ToString());
               // Add all values for the current result
               foreach (double value in surfaceGraph.Values)
               {
                  res.Append(String.Format("{0:0.00},", value));
               }
            }
         }

         return res.ToString();
      }

      /// <summary>
      /// Gets string containg formatted load case description for a given load case Id
      /// </summary>
      /// <param name="loadId">Id for the given load</param>
      /// <returns>Load case description formatted int a string</returns>
      private String GetLoadInfoForGraph(ElementId loadId)
      {
         // Get revit element
         Autodesk.Revit.DB.Element load = document.GetElement(loadId);
         // Get load case type
         String type = load is LoadCase ? "Load Case" : ( load is LoadCombination ? "Load Combination" : "Unknown");
         // Get load case name
         String name = load is LoadCase ? (load as LoadCase).Name : ( load is LoadCombination ? (load as LoadCombination).Name : "-");

         // Format both into a string
         return "Load type: " + type + ", Load name: " + name;
      }
      #endregion
   }
}
