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

namespace Revit.SDK.Samples.StairsAutomation.CS
{
   /// <summary>
   /// Represents a stairs configuration consisting of straight runs and rectangular landings.  The runs will
   /// switch based upon the input parameters.  The landings width can be adjusted independently.
   /// </summary>
   public class StairsStandardConfiguration : IStairsConfiguration
   {
      List<IStairsRunComponent> m_runConfigurations = new List<IStairsRunComponent>();
      List<IStairsLandingComponent> m_landingConfigurations = new List<IStairsLandingComponent>();
      private int m_numberOfLandings;
      private Stairs m_stairs;
      private StairsType m_stairsType;
      private double m_bottomElevation;
      private Transform m_transform;

      /// <summary>
      /// The number of risers to include in the stairs.
      /// </summary>
      public int RiserNumber { get; set; }

      private double m_runWidth;
      private bool m_runWidthOverride = false;
      private double m_runOffset;
      private bool m_runOffsetOverride = false;
      private double m_landingWidth;
      private bool m_landingWidthOverride = false;
      private int m_riserIncrement = 1;

      /// <summary>
      /// True to ensure that all runs have the same number of treads (which might require adjustment to the tread height to 
      /// compensate for the number of runs).  False to allow a different number of treads (which practically, might result in 
      /// some runs getting one less tread than others).
      /// </summary>
      public bool EqualizeRuns { get; set; }

      /// <summary>
      /// The width of the landings.  If not explicitly set, the width of the runs is used.
      /// </summary>
      public double LandingWidth
      {
         get
         {
            if (m_landingWidthOverride)
            {
               return m_landingWidth;
            }
            return RunWidth;
         }
         set
         {
            m_landingWidth = value;
            m_landingWidthOverride = true;
         }
      }

      /// <summary>
      /// The width of the runs.  If not explicitly set, the stairs type default width is used.
      /// </summary>
      public double RunWidth
      {
         get
         {
            if (m_runWidthOverride)
            {
               return m_runWidth;
            }
            return m_stairsType.MinRunWidth;
         }
         set
         {
            m_runWidth = value;
            m_runWidthOverride = true;
         }
      }

      /// <summary>
      /// The offset between the corner of one run and the corner of the next during a switchback at a landing.
      /// </summary>
      public double RunOffset
      {
         get
         {
            if (m_runOffsetOverride)
            {
               return m_runOffset;
            }
            return 2.0;
         }
         set
         {
            m_runOffset = value;
            m_runOffsetOverride = true;
         }
      }


      /// <summary>
      /// Creates a new StairsStandardConfiguration of runs and landings at the default location and orientation.
      /// </summary>
      /// <param name="stairs">The stairs element.</param>
      /// <param name="bottomLevel">The bottom level.</param>
      /// <param name="numberOfLandings">The number of landings to be included.</param>
      public StairsStandardConfiguration(Stairs stairs, Level bottomLevel, int numberOfLandings)
      {
         m_stairs = stairs;
         m_stairsType = m_stairs.Document.GetElement(m_stairs.GetTypeId()) as StairsType;
         m_bottomElevation = bottomLevel.Elevation;
         RiserNumber = stairs.DesiredRisersNumber;
         m_numberOfLandings = numberOfLandings;
         m_transform = Transform.Identity;
         EqualizeRuns = false;
      }

      /// <summary>
      /// Creates a new StairsStandardConfiguration of runs and landings at a specified location and orientation.
      /// </summary>
      /// <param name="stairs">The stairs element.</param>
      /// <param name="bottomLevel">The bottom level.</param>
      /// <param name="numberOfLandings">The number of landings to be included.</param>
      /// <param name="transform">The transformation (location and orientation).</param>
      public StairsStandardConfiguration(Stairs stairs, Level bottomLevel, int numberOfLandings, Transform transform)
      {
         m_stairs = stairs;
         m_stairsType = m_stairs.Document.GetElement(m_stairs.GetTypeId()) as StairsType;
         m_bottomElevation = bottomLevel.Elevation;
         RiserNumber = stairs.DesiredRisersNumber;
         m_numberOfLandings = numberOfLandings;
         m_transform = transform;
         EqualizeRuns = false;
      }

      /// <summary>
      /// Initializes the run and landing data in the configuration.
      /// </summary>
      public void Initialize()
      {
         if (m_numberOfLandings < 0)
         {
            throw new ArgumentOutOfRangeException("numberOfLandings", "Number of landings must be 0 or more");
         }

         if (m_numberOfLandings > RiserNumber)
         {
            throw new ArgumentOutOfRangeException("numberOfLandings", "Number of landings must be less than calculated riser number for the stairs");
         }

         if (EqualizeRuns)
         {
            int remainder = RiserNumber % (m_numberOfLandings + 1);
            if (remainder != 0)
            {
               m_stairs.DesiredRisersNumber = m_stairs.DesiredRisersNumber + m_numberOfLandings + 1 - remainder;
               m_stairs.Document.Regenerate();
               RiserNumber = m_stairs.DesiredRisersNumber;
            }
         }

         // Split as evenly as possible
         m_riserIncrement = RiserNumber / (m_numberOfLandings + 1);
      }

      /// <summary>
      /// Generates the landing configuration for the end lines of 2 risers.
      /// </summary>
      /// <param name="riser1Line">The end line of the first riser.</param>
      /// <param name="riser2Line">The start line of the second riser.</param>
      /// <param name="runDirection">The run direction.</param>
      /// <param name="landingElevation">The elevation.</param>
      /// <returns>The landing configuration.</returns>
      public virtual IStairsLandingComponent GenerateLanding(Line riser1Line, Line riser2Line, XYZ runDirection, double landingElevation)
      {
         IStairsLandingComponent landingConfiguration = new StairsRectangleLandingComponent(riser1Line, riser2Line, runDirection, landingElevation, LandingWidth);
         return landingConfiguration;
      }

      /// <summary>
      /// Generates the run configuration for the given elevation and properties.
      /// </summary>
      /// <param name="numberOfRisers">The number of risers in the run.</param>
      /// <param name="elevation">The start elevation.</param>
      /// <param name="minTreadDepth">The minimum tread depth.</param>
      /// <param name="runWidth">The width of the run.</param>
      /// <param name="transform">The transformation applied to the run start point and orientation.</param>
      /// <returns></returns>
      public virtual IStairsRunComponent GenerateRun(int numberOfRisers, double elevation, double minTreadDepth,
                                                    double runWidth, Transform transform)
      {
         IStairsRunComponent run = new StraightStairsRunComponent(numberOfRisers, elevation,
                                            minTreadDepth, runWidth, transform);
         return run;
      }

      /// <summary>
      /// A helper to obtain the Autodesk.Revit.Creation.Application handle.
      /// </summary>
      protected Autodesk.Revit.Creation.Application AppCreate
      {
         get
         {
            return m_stairs.Document.Application.Create;
         }
      }

      #region IStairsConfiguration Members

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public int GetNumberOfRuns()
      {
         return m_numberOfLandings + 1;
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public int GetNumberOfLandings()
      {
         return m_numberOfLandings;
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public void CreateStairsRun(Document document, ElementId stairsElementId, int runIndex)
      {
         // Calculate where the previous run ended.
         XYZ previousRunEndPoint = runIndex == 0 ? XYZ.Zero : m_runConfigurations[runIndex - 1].GetRunEndpoint();
         double elevation = runIndex == 0 ? m_bottomElevation : m_runConfigurations[runIndex - 1].TopElevation;

         // Setup number of risers for the run.
         int runNumberOfRisers = m_riserIncrement;
         if (runIndex == m_numberOfLandings)
         {
            runNumberOfRisers = RiserNumber - m_numberOfLandings * m_riserIncrement;
         }

         // Setup the transform for the run.  Every second run must be reversed in direction and start point generated from
         // the offet to the previous run.
         Transform transform = Transform.Identity;
         XYZ pivotPoint = previousRunEndPoint + m_transform.OfPoint(new XYZ(RunWidth + RunOffset, 0, 0));
         if (runIndex % 2 == 1)
         {
            Transform translation = Transform.CreateTranslation(pivotPoint);
            Transform rotation = Transform.CreateRotationAtPoint(XYZ.BasisZ, Math.PI, pivotPoint);
            transform = rotation.Multiply(translation);
         }
         transform = transform.Multiply(m_transform);

         // Generate the run configuration and it.
         IStairsRunComponent configuration = GenerateRun(runNumberOfRisers, elevation,
                                                               m_stairsType.MinTreadDepth, RunWidth, transform);

         m_runConfigurations.Add(configuration);

         // Create the run now (subsequent runs and landings need to use its geometric properties).
         configuration.CreateStairsRun(document, stairsElementId);
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public void CreateLanding(Document document, ElementId stairsElementId, int landingIndex)
      {
         // Get the run configurations for the runs before and after this landing.
         IStairsRunComponent configuration1 = m_runConfigurations[landingIndex];
         IStairsRunComponent configuration2 = m_runConfigurations[landingIndex + 1];

         // Get the stairs path from the lower run for run direction.
         Curve curve = configuration1.GetStairsPath()[0];
         XYZ runDirection = (curve.GetEndPoint(1) - curve.GetEndPoint(0)).Normalize();

         // Generate the landing configuration
         IStairsLandingComponent configuration = GenerateLanding(configuration1.GetLastCurve() as Line,
                                                configuration2.GetFirstCurve() as Line,
                                                 runDirection,
                                                  configuration2.RunElevation);
         m_landingConfigurations.Add(configuration);

         // Create the landing now
         configuration.CreateLanding(document, stairsElementId);
      }

      /// <summary>
      /// Implements the interface method.
      /// </summary>
      public void SetRunWidth(double value)
      {
         //cache width for new runs.
         RunWidth = value;

         //assign to existing run configurations
         foreach (var config in m_runConfigurations)
         {
            config.Width = value;
         }

      }

      #endregion

      /// <summary>
      /// Returns the transform assigned to the configuration.
      /// </summary>
      /// <returns>The transform.</returns>
      protected Transform GetTransform()
      {
         return m_transform;
      }
   }
}
