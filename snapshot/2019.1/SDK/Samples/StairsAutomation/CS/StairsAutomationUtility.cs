//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.StairsAutomation.CS
{
    /// <summary>
    /// The main class governing the automatic stairs element creation.
    /// </summary>
    public class StairsAutomationUtility
    {
        private Autodesk.Revit.DB.Document document;
        private Stairs m_stairs;
        private int m_stairsNumber;

        /// <summary>
        /// The bottom level for the stairs assembly.
        /// </summary>
        public Autodesk.Revit.DB.Level BottomLevel { get; set; }

        /// <summary>
        /// The top level for the stairs assembly.
        /// </summary>
        public Autodesk.Revit.DB.Level TopLevel { get; set; }

        /// <summary>
        /// The document.
        /// </summary>
        protected Autodesk.Revit.DB.Document Document
        {
            get
            {
                return document;
            }
        }

        /// <summary>
        /// The stairs.
        /// </summary>
        protected Autodesk.Revit.DB.Architecture.Stairs Stairs
        {
            get
            {
                return m_stairs;
            }
        }

        /// <summary>
        /// Creates a new instance of this class for a given stairs congfiguration. 
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="stairsNumber">The stairs configuration number.</param>
        protected StairsAutomationUtility(Autodesk.Revit.DB.Document document, int stairsNumber)
        {
            this.document = document;
            this.m_stairsNumber = stairsNumber;
        }

        /// <summary>
        /// Sets up a new stairs automation utility.
        /// </summary>
        /// <param name="document">The document in which the stairs will be created.</param>
        /// <param name="stairsNumber">The predefined stairs configuration number.</param>
        /// <returns></returns>
        public static StairsAutomationUtility Create(Autodesk.Revit.DB.Document document, int stairsNumber)
        {
            StairsAutomationUtility utility = new StairsAutomationUtility(document, stairsNumber);
            return utility;
        }

        #region LevelManagement
        /// <summary>
        /// Sets up the levels for the bottom and top of the stairs assembly.
        /// </summary>
        private void SetupLevels()
        {
            Tuple<Level, Level, Level> targetLevels = FindTargetLevels(document, "Level 1", "Level 2", "Level 3");
            Level level1 = targetLevels.Item1;
            Level level2 = targetLevels.Item2;
            Level level3 = targetLevels.Item3;
            switch (m_stairsNumber)
            {
                // Standard stair 1 level
                case 3:
                    BottomLevel = level1;
                    TopLevel = level2;
                    break;
                //Level 1 -> Level 3
                default:
                    BottomLevel = level1;
                    TopLevel = level3;
                    break;
            }
        }

        private static Tuple<Level, Level, Level> FindTargetLevels(Document doc, String name1, String name2, String name3)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Level));

            Level level1 = null;
            Level level2 = null;
            Level level3 = null;
            foreach (Level level in collector.Cast<Level>().Where<Level>(level => level.Name.Equals(name1) || level.Name.Equals(name2) || level.Name.Equals(name3)))
            {
                if (level.Name.Equals(name1))
                    level1 = level;
                else if (level.Name.Equals(name2))
                    level2 = level;
                else
                    level3 = level;
            }

            return new Tuple<Level, Level, Level>(level1, level2, level3);
        }

        // Not currently used.
#if false
        private static bool IsStoryLevel(Level level)
        {
            Parameter p = level.get_Parameter(BuiltInParameter.LEVEL_IS_BUILDING_STORY);
            return (p != null && p.AsInteger() != 0);
        }

        private bool IsBetweenExtents(Level level)
        {
            if (level.Id == BottomLevel.Id || level.Id == TopLevel.Id)
                return false;
            return level.Elevation > BottomLevel.Elevation && level.Elevation < TopLevel.Elevation;
        }

        private IEnumerable<Level> FindStoryLevelsBetweenExtents()
        {
            FilteredElementCollector collector = new FilteredElementCollector(document);
            collector.OfClass(typeof(Level));

            return collector.Cast<Level>().Where<Level>(level => IsBetweenExtents(level) && IsStoryLevel(level));
        }
#endif
        #endregion

        /// <summary>
        /// Sets up and returns the hardcoded configuration corresponding to the stairs number.
        /// </summary>
        /// <remarks>Some configuration types will require the ability to make changes to the stairs element.  
        /// Thus, this method needs an open transaction.</remarks>
        /// <returns></returns>
        protected virtual IStairsConfiguration SetupHardcodedConfiguration()
        {
            switch (m_stairsNumber)
            {
                // Straight run 1 level
                case 0:
                    {
                        var run = new StairsSingleStraightRun(m_stairs, BottomLevel, Transform.CreateTranslation(new XYZ(100, 0, 0)));
                        run.SetRunWidth(15.0);
                        return run;
                    }
                // Curved run 1 level
                case 1:
                    {
                        var run = new StairsSingleCurvedRun(m_stairs, BottomLevel, 6.0);
                        run.SetRunWidth(10.0);
                        return run;
                    }
                // Curve run 2 level
                case 2:
                    return new StairsSingleCurvedRun(m_stairs, BottomLevel, 3.0, Transform.CreateRotationAtPoint(XYZ.BasisZ, Math.PI, new XYZ(10, -20, 0)));
                // Standard stair 1 level
                case 3:
                    {
                        StairsStandardConfiguration configuration = new StairsStandardConfiguration(m_stairs, BottomLevel, 1, Transform.CreateRotationAtPoint(XYZ.BasisZ, Math.PI / 4.0, new XYZ(-20, -20, 0)));
                        configuration.EqualizeRuns = true;
                        configuration.Initialize();
                        return configuration;
                    }
                // Standard stair multi-level
                case 4:
                    {
                        StairsStandardConfiguration configuration = new StairsStandardConfiguration(m_stairs, BottomLevel, 3, Transform.CreateRotationAtPoint(XYZ.BasisZ, 7.0 * Math.PI / 6.0, new XYZ(15, 10, 0)));
                        configuration.RunWidth = 6.0;
                        configuration.RunOffset = 8.0 / 12.0;
                        configuration.LandingWidth = 4.0;
                        configuration.EqualizeRuns = true;
                        configuration.Initialize();
                        return configuration;
                    }
                case 100:
                    {
                        return new StairsSingleSketchedStraightRun(m_stairs, BottomLevel, Transform.CreateTranslation(new XYZ(50, 0, 0)));
                    }
                case 101:
                    {
                        return new StairsSingleSketchedCurvedRun(m_stairs, BottomLevel, 6.0, Transform.CreateTranslation(new XYZ(-10, 0, 0)));
                    }

            }
            return null;
        }


        /// <summary>
        /// Execute the creation of the specified stairs assembly.
        /// </summary>
        public void GenerateStairs()
        {
            SetupLevels();

            // Prepare and maintain StairsEditScope for stairs creation activities
            using (StairsEditScope editScope = new StairsEditScope(document, "Stairs Automation"))
            {
                // Instantiate the new stairs element.
                ElementId stairsElementId = editScope.Start(BottomLevel.Id, TopLevel.Id);

                // Remember the stairs for use in creation of the run and landing configurations.
                m_stairs = document.GetElement(stairsElementId) as Stairs;


                // Setup a transaction for use during the run and landing creation
                using (Transaction t = new Transaction(document, "Stairs Automation"))
                {
                    t.Start();

                    // Setup the configuration
                    IStairsConfiguration configuration = SetupHardcodedConfiguration();
                    if (configuration == null)
                        return;

                    // Create each run
                    int numberOfRuns = configuration.GetNumberOfRuns();
                    for (int i = 0; i < numberOfRuns; i++)
                    {
                        configuration.CreateStairsRun(document, stairsElementId, i);
                    }

                    // Create each landing
                    int numberOfLandings = configuration.GetNumberOfLandings();
                    for (int i = 0; i < numberOfLandings; i++)
                    {
                        configuration.CreateLanding(document, stairsElementId, i);
                    }

                    t.Commit();
                }

                editScope.Commit(new StairsEditScopeFailuresPreprocessor());
            }
        }
    }

    class StairsEditScopeFailuresPreprocessor : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            return FailureProcessingResult.Continue;
        }
    }
}
