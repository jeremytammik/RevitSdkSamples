//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Collections;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using System.Collections.Generic;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.WinderStairs.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        /// <summary>
        /// Implement this method as an external command for Revit.
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
        public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Document rvtDoc = commandData.Application.ActiveUIDocument.Document;
                var selectionIds = commandData.Application.ActiveUIDocument.Selection.GetElementIds();
                if (selectionIds.Count != 2 && selectionIds.Count != 3)
                {
                    message += "Please select two (or three) connected line elements. E.g. two (or three) connected straight Walls or Model Lines.";
                    return Result.Cancelled;
                } 

                List<ElementId> selectedIds = new List<ElementId>();
                selectedIds.AddRange(selectionIds);

                // Generate the winder creation parameters from selected elements.
                IList<XYZ> controlPoints = WinderUtil.CalculateControlPoints(rvtDoc, selectedIds);
                double runWidth = 0, treadDepth = 0;
                GetStairsData(rvtDoc, out runWidth, out treadDepth);
                IList<uint> maxCount = WinderUtil.CalculateMaxStepsCount(controlPoints, runWidth, treadDepth);
                uint numStepsInCorner = 3;
                double centerOffset = 0.0;
                if (selectionIds.Count == 2)
                {
                    // Create L-Winder
                    using (LWinderOptions options = new LWinderOptions())
                    {
                        options.NumStepsAtStart = maxCount[0];
                        options.NumStepsAtEnd = maxCount[1];
                        options.NumStepsInCorner = numStepsInCorner;
                        options.RunWidth = runWidth;
                        options.CenterOffsetE = centerOffset;
                        options.CenterOffsetF = centerOffset;
                        if (options.ShowDialog() == DialogResult.OK)
                        {
                            LWinder lwinder = new LWinder();
                            lwinder.NumStepsAtStart = options.NumStepsAtStart;
                            lwinder.NumStepsInCorner = options.NumStepsInCorner;
                            lwinder.NumStepsAtEnd = options.NumStepsAtEnd;
                            lwinder.RunWidth = options.RunWidth;
                            lwinder.TreadDepth = treadDepth;
                            lwinder.CenterOffsetE = options.CenterOffsetE;
                            lwinder.CenterOffsetF = options.CenterOffsetF;
                            AddInId activeid = options.DMU ? commandData.Application.ActiveAddInId : null;
                            WinderUpdater lwinderDMU = new WinderUpdater(lwinder,
                                selectedIds, rvtDoc, activeid, options.Sketch);
                        }
                    }
                }
                else if (selectionIds.Count == 3)
                {
                    // Create U-Winder
                    using (UWinderOptions options = new UWinderOptions())
                    {
                        options.NumStepsAtStart = maxCount[0];
                        options.NumStepsInMiddle = maxCount[1];
                        options.NumStepsAtEnd = maxCount[2];
                        options.NumStepsInCorner1 = numStepsInCorner;
                        options.NumStepsInCorner2 = numStepsInCorner;
                        options.RunWidth = runWidth;
                        options.CenterOffsetE1 = centerOffset;
                        options.CenterOffsetF1 = centerOffset;
                        options.CenterOffsetE2 = centerOffset;
                        options.CenterOffsetF2 = centerOffset;
                        if (options.ShowDialog() == DialogResult.OK)
                        {
                            UWinder uwinder = new UWinder();
                            uwinder.NumStepsAtStart = options.NumStepsAtStart;
                            uwinder.NumStepsInCorner1 = options.NumStepsInCorner1;
                            uwinder.NumStepsInMiddle = options.NumStepsInMiddle;
                            uwinder.NumStepsInCorner2 = options.NumStepsInCorner2;
                            uwinder.NumStepsAtEnd = options.NumStepsAtEnd;
                            uwinder.RunWidth = options.RunWidth;
                            uwinder.TreadDepth = treadDepth;
                            uwinder.CenterOffsetE1 = options.CenterOffsetE1;
                            uwinder.CenterOffsetF1 = options.CenterOffsetF1;
                            uwinder.CenterOffsetE2 = options.CenterOffsetE2;
                            uwinder.CenterOffsetF2 = options.CenterOffsetF2;
                            AddInId activeid = options.DMU ? commandData.Application.ActiveAddInId : null;
                            WinderUpdater uwinderDMU = new WinderUpdater(uwinder,
                                selectedIds, rvtDoc, activeid, options.Sketch);
                        }
                    }
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        /// <summary>
        /// Retrieve the system default stairs data, like run width and tread depth.
        /// </summary>
        /// <param name="rvtDoc">Revit Document</param>
        /// <param name="runWidth">Stairs run width</param>
        /// <param name="treadDepth">Stairs tread depth</param>
        private void GetStairsData(Document rvtDoc, out double runWidth, out double treadDepth)
        {
            FilteredElementCollector filterLevels = new FilteredElementCollector(rvtDoc);
            var levels = filterLevels.OfClass(typeof(Level)).ToElements();
            if (levels.Count < 2)
            {
                throw new InvalidOperationException("Need two Levels to create Stairs.");
            }
            List<Element> levelList = new List<Element>();
            levelList.AddRange(levels);
            levelList.Sort((a, b) => { return ((Level)a).Elevation.CompareTo(((Level)b).Elevation); });
            using (StairsEditScope stairsMode = new StairsEditScope(rvtDoc, "DUMMY STAIRS SCOPE"))
            {
                var stairsId = stairsMode.Start(levelList[0].Id, levelList[1].Id);
                Stairs stairs = rvtDoc.GetElement(stairsId) as Stairs;
                StairsType stairsType = rvtDoc.GetElement(stairs.GetTypeId()) as StairsType;
                runWidth = stairsType.MinRunWidth;
                treadDepth = stairs.ActualTreadDepth;
            }
        }
    }
}

