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
   /// Data class which stores information for creating orthogonal grids
   /// </summary>
   public class CreateOrthogonalGridsData
   {
      #region Fields
      // X coordinate of origin
      private double m_xOrigin;
      // Y coordinate of origin
      private double m_yOrigin;
      // Spacing between horizontal grids
      private double m_xSpacing;
      // Spacing between vertical grids
      private double m_ySpacing;
      // Number of horizontal grids
      private uint m_xNumber;
      // Number of vertical grids
      private uint m_yNumber;
      // Bubble location of horizontal grids
      private BubbleLocation m_xBubbleLoc;
      // Bubble location of vertical grids
      private BubbleLocation m_yBubbleLoc;
      // Label of first horizontal grid
      private String? m_xFirstLabel;
      // Label of first vertical grid
      private String? m_yFirstLabel;
      // Array list contains all grid labels in current document
      private ArrayList m_labelsList;
      // Revit application
      private ThisApplication? m_thisApp;
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
      /// Spacing between horizontal grids
      /// </summary>
      public double XSpacing
      {
         get
         {
            return m_xSpacing;
         }
         set
         {
            m_xSpacing = value;
         }
      }

      /// <summary>
      /// Spacing between vertical grids
      /// </summary>
      public double YSpacing
      {
         get
         {
            return m_ySpacing;
         }
         set
         {
            m_ySpacing = value;
         }
      }

      /// <summary>
      /// Number of horizontal grids
      /// </summary>
      public uint XNumber
      {
         get
         {
            return m_xNumber;
         }
         set
         {
            m_xNumber = value;
         }
      }

      /// <summary>
      /// Number of vertical grids
      /// </summary>
      public uint YNumber
      {
         get
         {
            return m_yNumber;
         }
         set
         {
            m_yNumber = value;
         }
      }

      /// <summary>
      /// Bubble location of horizontal grids
      /// </summary>
      public BubbleLocation XBubbleLoc
      {
         get
         {
            return m_xBubbleLoc;
         }
         set
         {
            m_xBubbleLoc = value;
         }
      }

      /// <summary>
      /// Bubble location of vertical grids
      /// </summary>
      public BubbleLocation YBubbleLoc
      {
         get
         {
            return m_yBubbleLoc;
         }
         set
         {
            m_yBubbleLoc = value;
         }
      }

      /// <summary>
      /// Label of first horizontal grid
      /// </summary>
      public String XFirstLabel
      {
         get
         {
            return m_xFirstLabel == null ? string.Empty : m_xFirstLabel;
         }
         set
         {
            m_xFirstLabel = value;
         }
      }

      /// <summary>
      /// Label of first vertical grid
      /// </summary>
      public String YFirstLabel
      {
         get
         {
            return m_yFirstLabel == null ? string.Empty : m_yFirstLabel;
         }
         set
         {
            m_yFirstLabel = value;
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
      public CreateOrthogonalGridsData(ThisApplication? thisApp, ForgeTypeId? unit, ArrayList labels)
      {
         m_thisApp = thisApp;
         m_labelsList = labels;
         m_unit = unit;
      }

      /// <summary>
      /// Create grids
      /// </summary>
      public void CreateGrids()
      {
         ArrayList failureReasons = new ArrayList();
         if (CreateXGrids(ref failureReasons) + CreateYGrids(ref failureReasons) != 0)
         {
            String? failureReason = SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToCreateGrids");
            if (failureReasons.Count != 0)
            {
               failureReason += SamplePropertis.GridCreationResources.ResourceManager.GetString("Reasons") + "\r";
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
      /// Create horizontal grids
      /// </summary>
      /// <param name="failureReasons">ArrayList contains failure reasons</param>
      /// <returns>Number of grids failed to create</returns>
      private int CreateXGrids(ref ArrayList failureReasons)
      {
         int errorCount = 0;

         for (int i = 0; i < m_xNumber; ++i)
         {
            XYZ? startPoint;
            XYZ? endPoint;
            Line line;
            Grid grid;

            try
            {
               if (m_yNumber != 0)
               {
                  // Grids will have an extension distance of m_ySpacing / 2
                  startPoint = m_thisApp?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin - m_ySpacing / 2, m_yOrigin + i * m_xSpacing, 0);
                  endPoint = m_thisApp?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin + (m_yNumber - 1) * m_ySpacing + m_ySpacing / 2, m_yOrigin + i * m_xSpacing, 0);
               }
               else
               {
                  startPoint = m_thisApp?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin, m_yOrigin + i * m_xSpacing, 0);
                  endPoint = m_thisApp?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin + m_xSpacing / 2, m_yOrigin + i * m_xSpacing, 0);
               }

               try
               {
                  // Create a line according to the bubble location
                  if (m_xBubbleLoc == BubbleLocation.StartPoint)
                  {
                     line = Line.CreateBound(startPoint, endPoint);
                  }
                  else
                  {
                     line = Line.CreateBound(endPoint, startPoint);
                  }
               }
               catch (System.ArgumentException)
               {
                  String? failureReason = SamplePropertis.GridCreationResources.ResourceManager.GetString("SpacingsTooSmall");
                  if (!failureReasons.Contains(failureReason))
                  {
                     failureReasons.Add(failureReason);
                  }
                  errorCount++;
                  continue;
               }

               // Create grid with line
               grid = Grid.Create(m_thisApp?.ActiveUIDocument.Document, line);

               // Set label of first horizontal grid
               if (grid != null && i == 0)
               {
                  try
                  {
                     grid.Name = m_xFirstLabel;
                  }
                  catch (System.ArgumentException)
                  {
                     MessageBox.Show(SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToSetLabel") + m_xFirstLabel + "!",
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
      /// Create vertical grids
      /// </summary>
      /// <param name="failureReasons">ArrayList contains failure reasons</param>
      /// <returns>Number of grids failed to create</returns>
      private int CreateYGrids(ref ArrayList failureReasons)
      {
         int errorCount = 0;

         for (int j = 0; j < m_yNumber; ++j)
         {
            XYZ? startPoint;
            XYZ? endPoint;
            Line line;
            Grid grid;

            try
            {
               if (m_xNumber != 0)
               {
                  startPoint = m_thisApp?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin + j * m_ySpacing, m_yOrigin - m_xSpacing / 2, 0);
                  endPoint = m_thisApp?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin + j * m_ySpacing, m_yOrigin + (m_xNumber - 1) * m_xSpacing + m_xSpacing / 2, 0);
               }
               else
               {
                  startPoint = m_thisApp?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin + j * m_ySpacing, m_yOrigin, 0);
                  endPoint = m_thisApp?.ActiveUIDocument.Document.Application.Create.NewXYZ(m_xOrigin + j * m_ySpacing, m_yOrigin + m_ySpacing / 2, 0);
               }

               try
               {
                  // Create a line according to the bubble location
                  if (m_yBubbleLoc == BubbleLocation.StartPoint)
                  {
                     line = Line.CreateBound(startPoint, endPoint);
                  }
                  else
                  {
                     line = Line.CreateBound(endPoint, startPoint);
                  }
               }
               catch (System.ArgumentException)
               {
                  String? failureReason = SamplePropertis.GridCreationResources.ResourceManager.GetString("SpacingsTooSmall");
                  if (!failureReasons.Contains(failureReason))
                  {
                     failureReasons.Add(failureReason);
                  }
                  errorCount++;
                  continue;
               }

               // Create grid with line
               grid = Grid.Create(m_thisApp?.ActiveUIDocument.Document, line);

               // Set label of first vertical grid
               if (grid != null && j == 0)
               {
                  try
                  {
                     grid.Name = m_yFirstLabel;
                  }
                  catch (System.ArgumentException)
                  {
                     MessageBox.Show(SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToSetLabel") + m_yFirstLabel + "!",
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
      #endregion
   }
}
