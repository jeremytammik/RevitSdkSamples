//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Text;
using System.Collections;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;

using MacroCSharpSamples;

using Application = Autodesk.Revit.ApplicationServices.Application;

using SamplePropertis = MacroCSharpSamples.GridCreation.GridCreationProperties;
using MacroSamples_RVT;

namespace Revit.SDK.Samples.GridCreation.CS
{
   /// <summary>
   /// The dialog which provides the options of creating radial and arc grids
   /// </summary>
   public class CreateRadialAndArcGridsData
   {
      #region Fields
      // X coordinate of origin
      private double m_xOrigin;
      // Y coordinate of origin
      private double m_yOrigin;
      // Start degree of arc grids and radial grids
      private double m_startDegree;
      // End degree of arc grids and radial grids
      private double m_endDegree;
      // Spacing between arc grids
      private double m_arcSpacing;
      // Number of arc grids
      private uint m_arcNumber = 0;
      // Number of radial grids
      private uint m_lineNumber = 0;
      // Radius of first arc grid
      private double m_arcFirstRadius;
      // Distance from origin to start point
      private double m_LineFirstDistance;
      // Bubble location of arc grids
      private BubbleLocation m_arcFirstBubbleLoc;
      // Bubble location of radial grids
      private BubbleLocation m_lineFirstBubbleLoc;
      // Label of first arc grid
      private String m_arcFirstLabel = string.Empty;
      // Label of first radial grid
      private String m_lineFirstLabel = string.Empty;
      // Array list contains all grid labels in current document
      private ArrayList m_labelsList;
      // Revit application
      private ThisApplication? m_app;
      // Current display unit type
      ForgeTypeId? m_unit;
      #endregion

      #region Properties
      /// <summary>
      /// X coordinate of origin
      /// </summary>
      public double XOrigin
      {
         get
         {
            return m_xOrigin;
         }
         set
         {
            m_xOrigin = value;
         }
      }

      /// <summary>
      /// Y coordinate of origin
      /// </summary>
      public double YOrigin
      {
         get
         {
            return m_yOrigin;
         }
         set
         {
            m_yOrigin = value;
         }
      }

      /// <summary>
      /// Start degree of arc grids and radial grids
      /// </summary>
      public double StartDegree
      {
         get
         {
            return m_startDegree;
         }
         set
         {
            m_startDegree = value;
         }
      }

      /// <summary>
      /// End degree of arc grids and radial grids
      /// </summary>
      public double EndDegree
      {
         get
         {
            return m_endDegree;
         }
         set
         {
            m_endDegree = value;
         }
      }

      /// <summary>
      /// Spacing between arc grids
      /// </summary>
      public double ArcSpacing
      {
         get
         {
            return m_arcSpacing;
         }
         set
         {
            m_arcSpacing = value;
         }
      }

      /// <summary>
      /// Number of arc grids
      /// </summary>
      public uint ArcNumber
      {
         get
         {
            return m_arcNumber;
         }
         set
         {
            m_arcNumber = value;
         }
      }

      /// <summary>
      /// Number of radial grids
      /// </summary>
      public uint LineNumber
      {
         get
         {
            return m_lineNumber;
         }
         set
         {
            m_lineNumber = value;
         }
      }

      /// <summary>
      /// Radius of first arc grid
      /// </summary>
      public double ArcFirstRadius
      {
         get
         {
            return m_arcFirstRadius;
         }
         set
         {
            m_arcFirstRadius = value;
         }
      }

      /// <summary>
      /// Distance from origin to start point
      /// </summary>
      public double LineFirstDistance
      {
         get
         {
            return m_LineFirstDistance;
         }
         set
         {
            m_LineFirstDistance = value;
         }
      }

      /// <summary>
      /// Bubble location of arc grids
      /// </summary>
      public BubbleLocation ArcFirstBubbleLoc
      {
         get
         {
            return m_arcFirstBubbleLoc;
         }
         set
         {
            m_arcFirstBubbleLoc = value;
         }
      }

      /// <summary>
      /// Bubble location of radial grids
      /// </summary>
      public BubbleLocation LineFirstBubbleLoc
      {
         get
         {
            return m_lineFirstBubbleLoc;
         }
         set
         {
            m_lineFirstBubbleLoc = value;
         }
      }

      /// <summary>
      /// Label of first arc grid
      /// </summary>
      public String ArcFirstLabel
      {
         get
         {
            return m_arcFirstLabel;
         }
         set
         {
            m_arcFirstLabel = value;
         }
      }

      /// <summary>
      /// Label of first radial grid
      /// </summary>
      public String LineFirstLabel
      {
         get
         {
            return m_lineFirstLabel;
         }
         set
         {
            m_lineFirstLabel = value;
         }
      }

      /// <summary>
      /// Get array list contains all grid labels in current document
      /// </summary>
      public ArrayList LabelsList
      {
         get
         {
            return m_labelsList;
         }
      }

      /// <summary>
      /// Current display unit type
      /// </summary>
      public ForgeTypeId? Unit
      {
         get
         {
            return m_unit;
         }
      }
      #endregion

      #region Methods
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="application">Application object</param>
      /// <param name="unit">Current length display unit type</param>
      /// <param name="labels">All existing labels in Revit's document</param>
      public CreateRadialAndArcGridsData(ThisApplication? app, ForgeTypeId? unit, ArrayList labels)
      {
         m_app = app;
         m_labelsList = labels;
         m_unit = unit;
      }

      /// <summary>
      /// Create grids
      /// </summary>
      public void CreateGrids()
      {
         if (CreateRadialGrids() != 0)
         {
            String failureReason = SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToCreateRadialGrids") + "\r";
            failureReason += SamplePropertis.GridCreationResources.ResourceManager.GetString("AjustValues");

            MessageBox.Show(failureReason,
                            SamplePropertis.GridCreationResources.ResourceManager.GetString("FailureCaptionCreateGrids"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
         }

         ArrayList failureReasons = new ArrayList();
         if (CreateArcGrids(ref failureReasons) != 0)
         {
            String failureReason = SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToCreateArcGrids") +
                SamplePropertis.GridCreationResources.ResourceManager.GetString("Reasons") + "\r";
            if (failureReasons.Count != 0)
            {
               failureReason += "\r";
               foreach (String reason in failureReasons)
               {
                  failureReason += reason + "\r";
               }
            }
            failureReason += "\r" + SamplePropertis.GridCreationResources.ResourceManager.GetString("AjustValues");

            MessageBox.Show(failureReason,
                            SamplePropertis.GridCreationResources.ResourceManager.GetString("FailureCaptionCreateGrids"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
         }
      }

      /// <summary>
      /// Create radial grids
      /// </summary>
      /// <returns>Number of grids failed to create</returns>
      private int CreateRadialGrids()
      {
         int errorCount = 0;

         for (int i = 0; i < m_lineNumber; ++i)
         {
            try
            {
               double angel;
               if (m_lineNumber == 1)
               {
                  angel = (m_startDegree + m_endDegree) / 2;
               }
               else
               {
                  // The number of space between radial grids will be m_lineNumber if arc is a circle
                  if (m_endDegree - m_startDegree == 2 * Values.PI)
                  {
                     angel = m_startDegree + i * (m_endDegree - m_startDegree) / m_lineNumber;
                  }
                  // The number of space between radial grids will be m_lineNumber-1 if arc is not a circle
                  else
                  {
                     angel = m_startDegree + i * (m_endDegree - m_startDegree) / (m_lineNumber - 1);
                  }
               }

               XYZ? startPoint;
               XYZ? endPoint;
               double cos = Math.Cos(angel);
               double sin = Math.Sin(angel);

               if (m_arcNumber != 0)
               {
                  // Grids will have an extension distance of m_ySpacing / 2
                  startPoint = m_app?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin + m_LineFirstDistance * cos, m_yOrigin + m_LineFirstDistance * sin, 0);
                  endPoint = m_app?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin + (m_arcFirstRadius + (m_arcNumber - 1) * m_arcSpacing + m_arcSpacing / 2) * cos,
                      m_yOrigin + (m_arcFirstRadius + (m_arcNumber - 1) * m_arcSpacing + m_arcSpacing / 2) * sin, 0);
               }
               else
               {
                  startPoint = m_app?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin + m_LineFirstDistance * cos, m_yOrigin + m_LineFirstDistance * sin, 0);
                  endPoint = m_app?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin + (m_arcFirstRadius + 5) * cos, m_yOrigin + (m_arcFirstRadius + 5) * sin, 0);
               }

               Line line;
               // Create a line according to the bubble location
               if (m_lineFirstBubbleLoc == BubbleLocation.StartPoint)
               {
                  line = Line.CreateBound(startPoint, endPoint);
               }
               else
               {
                  line = Line.CreateBound(endPoint, startPoint);
               }

               // Create grid with line
               Grid grid = Grid.Create(m_app?.ActiveUIDocument.Document, line);

               // Set label of first radial grid
               if (grid != null && i == 0)
               {
                  try
                  {
                     grid.Name = m_lineFirstLabel;
                  }
                  catch (System.ArgumentException)
                  {
                     MessageBox.Show(SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToSetLabel") + m_lineFirstLabel + "!",
                                     SamplePropertis.GridCreationResources.ResourceManager.GetString("FailureCaptionSetLabel"),
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);
                  }
               }
            }
            catch (Exception)
            {
               ++errorCount;
               continue;
            }
         }

         return errorCount;
      }

      /// <summary>
      /// Create Arc Grids
      /// </summary>
      /// <param name="failureReasons">ArrayList contains failure reasons</param>
      /// <returns>Number of grids failed to create</returns>
      private int CreateArcGrids(ref ArrayList failureReasons)
      {
         int errorCount = 0;

         for (int i = 0; i < m_arcNumber; ++i)
         {
            try
            {
               XYZ? origin = m_app?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin, m_yOrigin, 0);
               double radius = m_arcFirstRadius + i * m_arcSpacing;

               // In Revit UI user can select a circle to create a grid, but actually two grids 
               // (One from 0 to 180 degree and the other from 180 degree to 360) will be created. 
               // In RevitAPI using NewGrid method with a circle as its argument will raise an exception. 
               // Therefore in this sample we will create two arcs from the upper and lower parts of the 
               // circle, and then create two grids on the base of the two arcs to accord with UI.
               if (m_endDegree - m_startDegree == 2 * Values.PI) // Create circular grids
               {
                  Grid? gridUpper = CreateArcGrid(origin, radius, 0, Values.PI, m_arcFirstBubbleLoc);
                  if (gridUpper != null && i == 0)
                  {
                     try
                     {
                        gridUpper.Name = m_arcFirstLabel;
                     }
                     catch (System.ArgumentException)
                     {
                        MessageBox.Show(SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToSetLabel") + m_arcFirstLabel + "!",
                                        SamplePropertis.GridCreationResources.ResourceManager.GetString("FailureCaptionSetLabel"),
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                     }
                  }
                  CreateArcGrid(origin, radius, Values.PI, 2 * Values.PI, m_arcFirstBubbleLoc);
               }
               else // Create arc grids
               {
                  // Each arc grid will has extension degree of 15 degree
                  double extensionDegree = 15 * Values.DEGTORAD;
                  Grid? grid;

                  if (m_lineNumber != 0)
                  {
                     // If the range of arc degree is too close to a circle, the arc grids will not have 
                     // extension degrees.
                     // Also the room for bubble should be considered, so a room size of 3 * extensionDegree
                     // is reserved here
                     if (m_endDegree - m_startDegree < 2 * Values.PI - 3 * extensionDegree)
                     {
                        double startDegreeWithExtension = m_startDegree - extensionDegree;
                        double endDegreeWithExtension = m_endDegree + extensionDegree;
                        grid = CreateArcGrid(origin, radius, startDegreeWithExtension, endDegreeWithExtension, m_arcFirstBubbleLoc);
                     }
                     else
                     {
                        try
                        {
                           grid = CreateArcGrid(origin, radius, m_startDegree, m_endDegree, m_arcFirstBubbleLoc);
                        }
                        catch (System.ArgumentException)
                        {
                           String? failureReason = SamplePropertis.GridCreationResources.ResourceManager.GetString("EndPointsTooClose");
                           if (!failureReasons.Contains(failureReason))
                           {
                              failureReasons.Add(failureReason);
                           }
                           errorCount++;
                           continue;
                        }
                     }
                  }
                  else
                  {
                     try
                     {
                        grid = CreateArcGrid(origin, radius, m_startDegree, m_endDegree, m_arcFirstBubbleLoc);
                     }
                     catch (System.ArgumentException)
                     {
                        String? failureReason = SamplePropertis.GridCreationResources.ResourceManager.GetString("EndPointsTooClose");
                        if (!failureReasons.Contains(failureReason))
                        {
                           failureReasons.Add(failureReason);
                        }
                        errorCount++;
                        continue;
                     }
                  }


                  if (grid != null && i == 0)
                  {
                     try
                     {
                        grid.Name = m_arcFirstLabel;
                     }
                     catch (System.ArgumentException)
                     {
                        MessageBox.Show(SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToSetLabel") + m_arcFirstLabel + "!",
                                        SamplePropertis.GridCreationResources.ResourceManager.GetString("FailureCaptionSetLabel"),
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                     }
                  }
               }
            }
            catch (Exception)
            {
               ++errorCount;
               continue;
            }
         }

         return errorCount;
      }

      /// <summary>
      /// Create an arc grid with its origin, radius, start degree, end degree and bubble location
      /// </summary>
      /// <param name="origin">Arc grid's origin</param>
      /// <param name="radius">Arc grid's radius</param>
      /// <param name="startDegree">Arc grid's start degree</param>
      /// <param name="endDegree">Arc grid's end degree</param>
      /// <param name="bubLoc">Arc grid's Bubble location</param>
      /// <returns>The newly created grid</returns>
      private Grid? CreateArcGrid(XYZ? origin, double radius, double startDegree, double endDegree, BubbleLocation bubLoc)
      {
         // Get start point and end point of the arc and the middle point on the arc
         if (origin == null)
            return null;
         XYZ? startPoint = m_app?.ActiveUIDocument.Document.Application.Create.NewXYZ(origin.X + radius * Math.Cos(startDegree),
             origin.Y + radius * Math.Sin(startDegree), origin.Z);
         XYZ? midPoint = m_app?.ActiveUIDocument.Document.Application.Create.NewXYZ(origin.X + radius * Math.Cos((startDegree + endDegree) / 2),
             origin.Y + radius * Math.Sin((startDegree + endDegree) / 2), origin.Z);
         XYZ? endPoint = m_app?.ActiveUIDocument.Document.Application.Create.NewXYZ(origin.X + radius * Math.Cos(endDegree),
             origin.Y + radius * Math.Sin(endDegree), origin.Z);

         Arc arc;

         if (bubLoc == BubbleLocation.StartPoint)
         {
            arc = Arc.Create(startPoint, endPoint, midPoint);
         }
         else
         {
            arc = Arc.Create(endPoint, startPoint, midPoint);
         }

         return Grid.Create(m_app?.ActiveUIDocument.Document, arc);
      }
      #endregion
   }
}
