//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.WinderStairs.CS
{
    /// <summary>
    /// It represents a winder stairs, might include multiple straight runs and winder corners.
    /// </summary>
    abstract class Winder
    {
        /// <summary>
        /// Control Points to determine the winder shape(e.g. L or U shape).
        /// </summary>
        public virtual IList<XYZ> ControlPoints { get; set; }

        /// <summary>
        /// Winder stairs Run width.
        /// </summary>
        public double RunWidth { get; set; }

        /// <summary>
        /// Winder stairs tread depth, it just makes sense for straight steps.
        /// </summary>
        public double TreadDepth { get; set; }

        /// <summary>
        /// Outer boundary curves calculated by algorithm.
        /// </summary>
        protected IList<Curve> OuterBoundary { get; private set; }

        /// <summary>
        /// Inner boundary curves calculated by algorithm.
        /// </summary>
        protected IList<Curve> InnerBoundary { get; private set; }

        /// <summary>
        /// Center walk-path calculated by algorithm.
        /// </summary>
        protected IList<Curve> CenterWalkpath { get; private set; }

        /// <summary>
        /// Riser lines calculated by algorithm.
        /// </summary>
        protected IList<Curve> RiserLines { get; set; }

        /// <summary>
        /// This method creates the sketched winder run and it also deletes the input winderRunId.
        /// </summary>
        /// <param name="rvtDoc">Revit Document to create the sketched run</param>
        /// <param name="drawSketch">Flag to control whether to draw the sketch in document</param>
        /// <param name="winderRunId">Previous created winder, it will be deleted</param>
        public void Build(Document rvtDoc, bool drawSketch, ref ElementId winderRunId)
        {
            // Preparing the sketch containers.
            OuterBoundary = new List<Curve>();
            InnerBoundary = new List<Curve>();
            CenterWalkpath = new List<Curve>();
            RiserLines = new List<Curve>();

            // Fill the sketch containers
            GenerateSketch();

            // Draw the sketch with model curves if required so.
            if (drawSketch) DebugSketch(rvtDoc);

            // Create the sketched run
            CreateWinderRun(rvtDoc, ref winderRunId);
        }

        /// <summary>
        /// This method generates the winder sketch and fills the sketch to the properties:
        /// OuterBoundary, InnerBoundary, CenterWalkpath and RiserLines.
        /// </summary>
        protected abstract void GenerateSketch();

        /// <summary>
        /// This method creates the sketched run in Revit document.
        /// </summary>
        /// <param name="rvtDoc">Revit Document</param>
        /// <param name="winderRunId">Created winder run</param>
        private void CreateWinderRun(Document rvtDoc, ref ElementId winderRunId)
        {
            using (StairsEditScope stairsMode = new StairsEditScope(rvtDoc, GetType().Name))
            {
                var winderOldRun = rvtDoc.GetElement(winderRunId) as StairsRun;
                var stairsId = ElementId.InvalidElementId;
                // Non-existed stairs, create a new one.
                if (winderOldRun == null)
                {
                    // Find two levels to create a stairs between them
                    FilteredElementCollector filterLevels = new FilteredElementCollector(rvtDoc);
                    var levels = filterLevels.OfClass(typeof(Level)).ToElements();
                    List<Element> levelList = new List<Element>();
                    levelList.AddRange(levels);
                    levelList.Sort((a, b) =>
                    { return ((Level)a).Elevation.CompareTo(((Level)b).Elevation); });
                    // Start the stairs edit mode
                    stairsId = stairsMode.Start(levelList[0].Id, levelList[1].Id);
                }
                else // using the existed stairs
                {
                    // Start the stairs edit mode
                    stairsId = stairsMode.Start(winderOldRun.GetStairs().Id);
                }

                using (Transaction winderTransaction = new Transaction(rvtDoc))
                {
                    // Start the winder creation transaction 
                    winderTransaction.Start(GetType().Name);

                    // The boundaries is consist of internal and external boundaries.
                    List<Curve> boundarys = new List<Curve>();
                    boundarys.AddRange(InnerBoundary);
                    boundarys.AddRange(OuterBoundary);

                    // Calculate the run elevation.
                    Stairs stairs = rvtDoc.GetElement(stairsId) as Stairs;
                    double elevation = ControlPoints[0].Z;
                    double actualElevation = Math.Max(elevation, stairs.BaseElevation);

                    // Create the run
                    StairsRun run = StairsRun.CreateSketchedRun(rvtDoc, stairsId,
                        actualElevation, boundarys, RiserLines, CenterWalkpath);
                    if (ElementId.InvalidElementId != winderRunId)
                    {
                        // Delete the old run
                        rvtDoc.Delete(winderRunId);
                    }
                    // output the new run
                    winderRunId = run.Id;
                    // Finish the winder run creation.
                    winderTransaction.Commit();
                }
                // Finish the stairs Edit mode.
                stairsMode.Commit(new StairsEditScopeFailuresPreprocessor());
            }
        }

        /// <summary>
        /// This method draws sketch as model curves in Revit document for debug purpose.
        /// </summary>
        /// <param name="rvtDoc">Revit document</param>
        private void DebugSketch(Document rvtDoc)
        {
            try
            {
                using (Transaction debugTransaction = new Transaction(rvtDoc))
                {
                    debugTransaction.Start("DEBUG API WINDER SKETCH");

                    SketchPlane skp = SketchPlane.Create(
                        rvtDoc, Plane.CreateByNormalAndOrigin(XYZ.BasisZ, ControlPoints[0]));
                    CurveArray curves = new CurveArray();
                    foreach (var curve in OuterBoundary)
                    {
                        curves.Append(curve);
                    }

                    foreach (var curve in InnerBoundary)
                    {
                        curves.Append(curve);
                    }

                    foreach (var curve in CenterWalkpath)
                    {
                        curves.Append(curve);
                    }

                    foreach (var curve in RiserLines)
                    {
                        curves.Append(curve);
                    }

                    rvtDoc.Create.NewModelCurveArray(curves, skp);
                    debugTransaction.Commit();
                }
            }
            catch (System.Exception)
            {
                // SWALLOW ALL DEBUG EXCEPTION
            }
        }
    }

    class StairsEditScopeFailuresPreprocessor : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            failuresAccessor.DeleteAllWarnings();
            return FailureProcessingResult.Continue;
        }
    }
}
