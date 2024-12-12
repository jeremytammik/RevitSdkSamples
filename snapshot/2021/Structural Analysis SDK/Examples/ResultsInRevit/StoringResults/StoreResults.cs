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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ResultsBuilder;
using Autodesk.Revit.DB.ResultsBuilder.Storage;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.StoringResults.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   class StoreResults : IExternalCommand
   {
      #region Class Members

      // Ids of elements used in this example
      // ElementIds are hardcoded, so this example has to be used with the included "ResultsInRevit.rvt" file
      ElementId linearElementId = new ElementId(262075);
      ElementId surfaceElementId = new ElementId(263074);

      // Ids of loads used in the example
      ElementId loadCaseId1 = new ElementId(37232);
      ElementId loadCaseId2 = new ElementId(37234);

      Guid packageGuid = Guid.NewGuid();

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
         Autodesk.Revit.UI.Result retVal = Autodesk.Revit.UI.Result.Succeeded;
         Document doc = commandData.Application.ActiveUIDocument.Document;

         // Check if this is the Revit file that is included in this example and display warning if not
         if (doc.Title != "ResultsInRevit.rvt")
         {
            Autodesk.Revit.UI.TaskDialog.Show("Wrong Revit file", "Cannot use :\n" + doc.PathName + "\n\nPlease use attached " + "\"ResultsInRevit.rvt\" file.");
            retVal = Autodesk.Revit.UI.Result.Failed;
         }

         Transaction trans = new Transaction(doc);
         try
         {
            trans.SetName("ResultsInRevit_Transaction");
            trans.Start();

            // Create new instance of empty result builder 
            ResultsPackageBuilder resultsPackageBuilder = createResultsPackageBuilder(doc);
            // Add analitical results to the linear element for two load cases
            AddLinearResults(resultsPackageBuilder, linearElementId, new ElementId[] { loadCaseId1, loadCaseId2 });
            // Add some arbitrary results to the same element
            AddArbitraryLinearResults(resultsPackageBuilder, linearElementId);
            // Add analitical results to the surface element for the same load cases
            AddSurfaceResults(doc, resultsPackageBuilder, surfaceElementId, new ElementId[] { loadCaseId1, loadCaseId2});
            // Add arbitrary results to the surface element
            AddArbitrarySurfaceResults(doc, resultsPackageBuilder, surfaceElementId);

            // End adding and close transaction
            resultsPackageBuilder.Finish();
            trans.Commit();
         }
         catch (Exception ex)
         {
            // Rollback transaction and display warning if there were any problems
            trans.RollBack();
            Autodesk.Revit.UI.TaskDialog.Show("Failed to write results to Revit", ex.Message.ToString());
            retVal = Autodesk.Revit.UI.Result.Failed;
         }

         return retVal;
      }
      #endregion

      #region Class Implementation

      /// <summary>
      /// Creates an empty results package and fills out its header fields
      /// </summary>
      /// <param name="doc">Revit document</param>
      /// <returns>Reference to the newly created package</returns>
      private ResultsPackageBuilder createResultsPackageBuilder( Document doc)
      {
         ResultsAccess resultsAccess = ResultsAccess.CreateResultsAccess(doc);
         ResultsPackageBuilder resultsPackageBuilder = resultsAccess.CreateResultsPackage(packageGuid, "ResultsInRevit", UnitsSystem.Metric, ResultsPackageTypes.All);

         resultsPackageBuilder.SetAnalysisName("ResultsInRevit_Analysis");
         resultsPackageBuilder.SetModelName("ResultsInRevit_Model");
         resultsPackageBuilder.SetDescription("Sample results");
         resultsPackageBuilder.SetVendorDescription("Autodesk");
         resultsPackageBuilder.SetVendorId("ADSK");

         return resultsPackageBuilder;
      }

      /// <summary>
      /// Adds some results to a linear element for specific load cases
      /// </summary>
      /// <param name="resultsPackageBuilder">Reference to the results package</param>
      /// <param name="elementId">Id of the linear element to which results are to be added</param>
      /// <param name="loadCaseIds">Array of Ids of loads</param>
      private void AddLinearResults(ResultsPackageBuilder resultsPackageBuilder, ElementId elementId, ElementId []loadCaseIds)
      {
         // Create a list of points ( represented here by relative coordinates on the element ) in which results will be stored
         List<double> xCoordinateValues = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };

         // Create list of some results
         // Each list element contains result type and a list of result values
         List<Tuple<LinearResultType, List<double>>> valuesForForceType = new List<Tuple<LinearResultType, List<double>>>()
         {
            new Tuple<LinearResultType,List<double>> ( LinearResultType.Fx, new List<double>() {  4.01,  4.02,  4.03,  4.04,  4.05, 4.06, 4.07, 4.08, 4.09, 4.10 }),
            new Tuple<LinearResultType,List<double>> ( LinearResultType.Fy, new List<double>() { -0.40, -0.30, -0.20, -0.10,  0.00, 0.10, 0.20, 0.30, 0.40, 0.50 }),
            new Tuple<LinearResultType,List<double>> ( LinearResultType.Fz, new List<double>() {  1.00,  1.20,  1.30,  1.40,  1.50, 1.60, 1.70, 1.80, 1.90, 2.00 }),
            new Tuple<LinearResultType,List<double>> ( LinearResultType.Mx, new List<double>() {  1.60,  1.60,  1.60,  1.60,  1.60, 1.60, 1.60, 1.60, 1.60, 1.60 }),
            new Tuple<LinearResultType,List<double>> ( LinearResultType.My, new List<double>() {  1.21,  1.00,  0.90,  0.40,  0.10, 0.10, 0.40, 0.90, 1.00, 1.21 }),
            new Tuple<LinearResultType,List<double>> ( LinearResultType.Mz, new List<double>() {  6.21,  6.00,  5.90,  5.40,  5.10, 5.10, 5.40, 5.90, 6.00, 6.21 }),
         };

         // Add results for the first load case
         ElementId loadCaseId1 = loadCaseIds[0];
         // First result domain
         resultsPackageBuilder.SetBarResult(elementId, loadCaseId1, DomainResultType.X, xCoordinateValues);
         // Then result values
         foreach ( var valueForForce in valuesForForceType)
         {
            resultsPackageBuilder.SetBarResult(elementId, loadCaseId1, valueForForce.Item1, valueForForce.Item2);
         }

         // Modify results and add them for the second load case as well
         ElementId loadCaseId2 = loadCaseIds[1];
         resultsPackageBuilder.SetBarResult(elementId, loadCaseId2, DomainResultType.X, xCoordinateValues);
         foreach (var valueForForce in valuesForForceType)
         {
            resultsPackageBuilder.SetBarResult(elementId, loadCaseId2, valueForForce.Item1, valueForForce.Item2.Select(s => s * 2));
         }

      }

      /// <summary>
      ///  Adds some arbitrary(user defined) results to a linear element. The arbitrary results that we are using here are load case independent
      /// </summary>
      /// <param name="resultsPackageBuilder"></param>
      /// <param name="elementId"></param>
      private void AddArbitraryLinearResults(ResultsPackageBuilder resultsPackageBuilder, ElementId elementId)
      {
         // Create list of some results
         // Each list element contains arbitrary result type(represented by string) and a list of result values
         List<Tuple<String, List<double>>> valuesForRnf = new List<Tuple<String, List<double>>>()
         {
            new Tuple<String,List<double>> ( "RnfBarsAngleBottom", new List<double>() {  45.00,  30.00,  15.00,  60.00,  0.00, 0.00, 10.00, 0.00, 0.00, 90.00 }),
            new Tuple<String,List<double>> ( "RnfBarsAngleTop",    new List<double>() {  45.00,  30.00,  15.00,  60.00,  0.00, 0.00, 10.00, 0.00, 0.00, 90.00 }),
            new Tuple<String,List<double>> ( "RnfBarsAngleLeft",   new List<double>() {  45.00,  30.00,  15.00,  60.00,  0.00, 0.00, 10.00, 0.00, 0.00, 90.00 }),
            new Tuple<String,List<double>> ( "RnfBarsAngleRight",  new List<double>() {  45.00,  30.00,  15.00,  60.00,  0.00, 0.00, 10.00, 0.00, 0.00, 90.00 }),
         };

         // Add result domain for load independent results
         List<double> xCoordinateValues = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
         resultsPackageBuilder.SetBarResult(elementId, null, DomainResultType.X, xCoordinateValues);
         // Add result values
         foreach (var valueForRnf in valuesForRnf)
         {
            resultsPackageBuilder.AddMeasurement(valueForRnf.Item1, MeasurementResultType.Bar, UnitType.UT_Angle, DisplayUnitType.DUT_DEGREES_AND_MINUTES, MeasurementDependencyType.LoadCaseIndependent);
            resultsPackageBuilder.SetArbitraryResult(elementId, null, valueForRnf.Item1, valueForRnf.Item2);
         }
      }

      /// <summary>
      /// Auxiliary method. Gets sample points from surface element contour( three points for each contour curve: start, mid and end )
      /// </summary>
      /// <param name="doc">Revit document</param>
      /// <param name="elementId">Surface element Id</param>
      /// <returns>List of XYZ points on the contour</returns>
      private List<XYZ> GetSurfaceContourPoints( Document doc, ElementId elementId )
      {
         // Create point list, get list of curves from analitical model
         List<XYZ> contour = new List<XYZ>();
         AnalyticalModel analyticalModel = (doc.GetElement(elementId) as AnalyticalModel);
         IList<Curve> curves = analyticalModel.GetCurves(AnalyticalCurveType.RawCurves);
         
         // Iterate over curves and add mid, and end point of every curve to the point list
         foreach (Curve curve in curves)
         {
            double startParam = curve.GetEndParameter(0),
                   endParam = curve.GetEndParameter(1);

            XYZ start = curve.Evaluate(startParam, false),
                mid   = curve.Evaluate(0.5*(startParam+endParam), false),
                end   = curve.Evaluate(endParam, false);
            contour.Add(mid);
            contour.Add(end);
         }
         return contour;
      }

      /// <summary>
      /// Auxiliary method. Generates a list of sample results for points on surface element contour
      /// </summary>
      /// <param name="points">List of points for which results are to be generated</param>
      /// <returns>A list containing a number of records with surface result type and corresponding result values</returns>
      private List<Tuple<SurfaceResultType, List<double>>> GenerateSampleSurfaceResultsForContour(List<XYZ> points)
      {
         // Create an array of result types
         SurfaceResultType[] surfaceResultTypes = { SurfaceResultType.Fxx, SurfaceResultType.Fyy, SurfaceResultType.Fxy, SurfaceResultType.Mxx, SurfaceResultType.Myy, SurfaceResultType.Mxy };

         // Create list
         var sampleResults = new List<Tuple<SurfaceResultType, List<double>>>();
         double coeff = 1.0e-3;
         // Iterate over types, create a value for each point and add a record to the list
         foreach (SurfaceResultType surfaceResultType in surfaceResultTypes)
         {
            coeff *= 10;
            List<double> results = points.Select( s=>( s.X*coeff + s.Y*coeff + s.Z*coeff)).ToList();
            sampleResults.Add( new Tuple<SurfaceResultType,List<double>>(surfaceResultType,results));
         }
         return sampleResults;
      }

      /// <summary>
      /// Auxiliary method. Generates a list of sample arbitrary results for points on surface element contour
      /// </summary>
      /// <param name="points">List of points for which arbitrary results are to be generated</param>
      /// <returns>A list containing a number of records with arbitrary surface result type and corresponding result values</returns>
      private List<Tuple<String, List<double>>> GenerateSampleArbitrarySurfaceResultsForContour(List<XYZ> points)
      {
         // Create an array of arbitrary result types. Type is represented by a string
         String[] surfaceResultTypes = { "Axx__Bottom", "Ayy__Bottom", "Axx__Top", "Ayy__Top" };

         // Create list
         var sampleResults = new List<Tuple<String, List<double>>>();
         double coeff = 1.0e-4;
         // Iterate over types, create a value for each point and add a record to the list
         foreach (String surfaceResultType in surfaceResultTypes)
         {
            coeff *= 1.5;
            List<double> results = points.Select(s => (s.X * coeff + s.Y * coeff + s.Z * coeff)).ToList();
            sampleResults.Add(new Tuple<String, List<double>>(surfaceResultType, results));
         }
         return sampleResults;
      }

      /// <summary>
      ///  Adds some results to a surface element for specific load cases
      /// </summary>
      /// <param name="doc">Revit document</param>
      /// <param name="resultsPackageBuilder"></param>
      /// <param name="elementId"></param>
      /// <param name="loadCaseIds"></param>
      private void AddSurfaceResults(Document doc, ResultsPackageBuilder resultsPackageBuilder, ElementId elementId, ElementId []loadCaseIds)
      {
         // Get a list of points on the contour
         List<XYZ> contourPoints = GetSurfaceContourPoints(doc, elementId);
         // Get a list of sample results in contour points
         var contourResults = GenerateSampleSurfaceResultsForContour(contourPoints);

         // Add results for the first load case
         ElementId loadCaseId1 = loadCaseIds[0];
         // Result domain for each axis first
         resultsPackageBuilder.SetSurfaceResult(elementId, loadCaseId1, DomainResultType.X, contourPoints.Select(s => s.X));
         resultsPackageBuilder.SetSurfaceResult(elementId, loadCaseId1, DomainResultType.Y, contourPoints.Select(s => s.Y));
         resultsPackageBuilder.SetSurfaceResult(elementId, loadCaseId1, DomainResultType.Z, contourPoints.Select(s => s.Z));
         // Then correspoding result values
         foreach (var contourResult in contourResults)
         {
            resultsPackageBuilder.SetSurfaceResult(elementId, loadCaseId1, contourResult.Item1, contourResult.Item2);
         }

         // Add modified results for the second load case too
         ElementId loadCaseId2 = loadCaseIds[1];
         resultsPackageBuilder.SetSurfaceResult(elementId, loadCaseId2, DomainResultType.X, contourPoints.Select(s => s.X));
         resultsPackageBuilder.SetSurfaceResult(elementId, loadCaseId2, DomainResultType.Y, contourPoints.Select(s => s.Y));
         resultsPackageBuilder.SetSurfaceResult(elementId, loadCaseId2, DomainResultType.Z, contourPoints.Select(s => s.Z));
         foreach (var contourResult in contourResults)
         {
            resultsPackageBuilder.SetSurfaceResult(elementId, loadCaseId2, contourResult.Item1, contourResult.Item2.Select(s=>s*2));
         }
        
      }

      /// <summary>
      ///   Adds some arbitrary(user defined), load independent results to a surface element.
      /// </summary>
      /// <param name="doc"></param>
      /// <param name="resultsPackageBuilder"></param>
      /// <param name="elementId"></param>
      private void AddArbitrarySurfaceResults(Document doc, ResultsPackageBuilder resultsPackageBuilder, ElementId elementId)
      {
         // Get contour points and generate sample arbitrary results for the surface element
         List<XYZ> contourPoints = GetSurfaceContourPoints(doc, elementId);
         var contourResults = GenerateSampleArbitrarySurfaceResultsForContour(contourPoints);

         // Add result domain for load independent results for x,y,z axes
         resultsPackageBuilder.SetSurfaceResult(elementId, null, DomainResultType.X, contourPoints.Select(s => s.X));
         resultsPackageBuilder.SetSurfaceResult(elementId, null, DomainResultType.Y, contourPoints.Select(s => s.Y));
         resultsPackageBuilder.SetSurfaceResult(elementId, null, DomainResultType.Z, contourPoints.Select(s => s.Z));
         // Add result values
         foreach (var result in contourResults)
         {
            resultsPackageBuilder.AddMeasurement(result.Item1, MeasurementResultType.Surface, UnitType.UT_Area, DisplayUnitType.DUT_SQUARE_METERS, MeasurementDependencyType.LoadCaseIndependent);
            resultsPackageBuilder.SetArbitraryResult(elementId, null, result.Item1, result.Item2);
         }
      }
      #endregion

   }
}
