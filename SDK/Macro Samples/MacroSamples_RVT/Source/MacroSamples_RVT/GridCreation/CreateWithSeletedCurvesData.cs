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

using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Macros;

using MacroCSharpSamples;
using Application = Autodesk.Revit.ApplicationServices.Application;
using SamplePropertis = MacroCSharpSamples.GridCreation.GridCreationProperties;
using MacroSamples_RVT;

namespace Revit.SDK.Samples.GridCreation.CS
{
   /// <summary>
   /// The dialog which provides the options of creating grids with selected lines/arcs
   /// </summary>
   public class CreateWithSelectedCurvesData
   {
      #region Fields
      // Selected curves in current document
      private CurveArray? m_selectedCurves;
      // Whether to delete selected lines/arc after creation
      private bool m_deleteSelectedElements;
      // Label of first grid
      private String m_firstLabel = string.Empty;
      // Bubble location of grids
      private BubbleLocation m_bubbleLocation;
      // Array list contains all grid labels in current document
      private ArrayList m_labelsList;
      // Revit application
      private ThisApplication? m_thisApp;
      private Application? m_revit;
      #endregion

      #region Properties
      /// <summary>
      /// Whether to delete selected lines/arc after creation
      /// </summary>
      public bool DeleteSelectedElements
      {
         get
         {
            return m_deleteSelectedElements;
         }
         set
         {
            m_deleteSelectedElements = value;
         }
      }

      /// <summary>
      /// Bubble location of grids
      /// </summary>
      public BubbleLocation BubbleLocation
      {
         get
         {
            return m_bubbleLocation;
         }
         set
         {
            m_bubbleLocation = value;
         }
      }

      /// <summary>
      /// Label of first grid
      /// </summary>
      public String FirstLabel
      {
         get
         {
            return m_firstLabel;
         }
         set
         {
            m_firstLabel = value;
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
      #endregion

      #region Methods
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="application">Revit application</param>
      /// <param name="selectedCurves">Array contains geometry curves of selected lines or arcs </param>
      /// <param name="labels">List contains all existing labels in Revit document</param>
      public CreateWithSelectedCurvesData(ThisApplication? thisApp, CurveArray? selectedCurves, ArrayList labels)
      {
         m_thisApp = thisApp;
         m_revit = thisApp?.ActiveUIDocument.Document.Application;

         m_selectedCurves = selectedCurves;
         m_labelsList = labels;
      }

      /// <summary>
      /// Create grids
      /// </summary>
      public void CreateGrids()
      {
         int errorCount = 0;

         int i = 0;
         if (m_selectedCurves != null)
         {
            foreach (Curve curve in m_selectedCurves)
            {
               try
               {
                  Line? line = curve as Line;
                  if (line != null) // Selected curve is a line
                  {
                     Grid grid;
                     // Create linear grid
                     grid = CreateLinearGrid(line);

                     // Set label of first grid
                     if (i == 0 && grid != null)
                     {
                        try
                        {
                           grid.Name = m_firstLabel;
                        }
                        catch (System.ArgumentException)
                        {
                           MessageBox.Show(SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToSetLabel") + m_firstLabel + "!",
                                           SamplePropertis.GridCreationResources.ResourceManager.GetString("FailureCaptionSetLabel"),
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Information);
                        }
                     }
                  }
                  else // Selected curve is an arc
                  {
                     Arc? arc = curve as Arc;
                     if (arc != null)
                     {
                        if (arc.IsBound) // Part of a circle
                        {
                           Grid grid;
                           // Create arc grid
                           grid = CreateArcGrid(arc);

                           // Set label of first grid
                           if (i == 0 && grid != null)
                           {
                              try
                              {
                                 grid.Name = m_firstLabel;
                              }
                              catch (System.ArgumentException)
                              {
                                 MessageBox.Show(SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToSetLabel") + m_firstLabel + "!",
                                                 SamplePropertis.GridCreationResources.ResourceManager.GetString("FailureCaptionSetLabel"),
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Information);
                              }
                           }
                        }
                        else // Arc is a circle
                        {
                           // In Revit UI user can select a circle to create a grid, but actually two grids 
                           // (One from 0 to 180 degree and the other from 180 degree to 360) will be created. 
                           // In RevitAPI using NewGrid method with a circle as its argument will raise an exception. 
                           // Therefore in this sample we will create two arcs from the upper and lower parts of the 
                           // circle, and then create two grids on the base of the two arcs to accord with UI.
                           Grid? gridUpper = null;
                           Grid? gridLower = null;
                           bool isFirstGrid = (i == 0);
                           // Create grids
                           CreateGridsForCircle(arc, ref gridUpper, ref gridLower, isFirstGrid);
                        }
                     }
                  }
               }
               catch (Exception)
               {
                  ++errorCount;
                  continue;
               }

               ++i;
            }
         }


         if (m_deleteSelectedElements)
         {
            try
            {
               m_thisApp?.ActiveUIDocument.Document.Delete(GridCreation.GetSelectedModelLinesAndArcs(m_thisApp));
            }
            catch (Exception)
            {
               MessageBox.Show(SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToDeletedLinesOrArcs"),
                               SamplePropertis.GridCreationResources.ResourceManager.GetString("FailureCaptionDeletedLinesOrArcs"),
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
         }

         if (errorCount != 0)
         {
            MessageBox.Show(SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToCreateGrids"),
                            SamplePropertis.GridCreationResources.ResourceManager.GetString("FailureCaptionCreateGrids"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
         }
      }

      /// <summary>
      /// Create two grids if the selected curve is a circle
      /// </summary>
      /// <param name="arc">The circular curve to be transferred to grid</param>
      /// <param name="gridUpper">The grid to be created base on the upper part of the circular curve</param>
      /// <param name="gridLower">The grid to be created base on the lower part of the circular curve</param>
      /// <param name="isFirst">Whether the circular curve is the first curve to be transferred</param>
      private void CreateGridsForCircle(Arc arc, ref Grid? gridUpper, ref Grid? gridLower, bool isFirst)
      {
         XYZ center = arc.Center;
         double radius = arc.Radius;

         XYZ? XRightPoint = m_revit?.Create.NewXYZ(center.X + radius, center.Y, 0);
         XYZ? XLeftPoint = m_revit?.Create.NewXYZ(center.X - radius, center.Y, 0);
         XYZ? YUpperPoint = m_revit?.Create.NewXYZ(center.X, center.Y + radius, 0);
         XYZ? YLowerPoint = m_revit?.Create.NewXYZ(center.X, center.Y - radius, 0);
         Arc upperArc;
         Arc lowerArc;
         if (m_bubbleLocation == BubbleLocation.StartPoint)
         {
            upperArc = Arc.Create(XRightPoint, XLeftPoint, YUpperPoint);
            lowerArc = Arc.Create(XLeftPoint, XRightPoint, YLowerPoint);
         }
         else
         {
            upperArc = Arc.Create(XLeftPoint, XRightPoint, YUpperPoint);
            lowerArc = Arc.Create(XRightPoint, XLeftPoint, YLowerPoint);
         }

         // Create arc grids
         gridUpper = Grid.Create(m_thisApp?.ActiveUIDocument.Document, upperArc);
         gridLower = Grid.Create(m_thisApp?.ActiveUIDocument.Document, lowerArc);

         if (gridUpper != null && isFirst)
         {
            try
            {
               gridUpper.Name = m_firstLabel;
            }
            catch (System.ArgumentException)
            {
               MessageBox.Show(SamplePropertis.GridCreationResources.ResourceManager.GetString("FailedToSetLabel") + m_firstLabel + "!",
                               SamplePropertis.GridCreationResources.ResourceManager.GetString("FailureCaptionSetLabel"),
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
         }
      }

      /// <summary>
      /// Create arc grid
      /// </summary>
      /// <param name="arc">The arc curve to be transferred to grid</param>
      /// <returns>The newly created grid</returns>
      private Grid CreateArcGrid(Arc arc)
      {
         Grid grid;

         if (m_bubbleLocation == BubbleLocation.StartPoint)
         {
            grid = Grid.Create(m_thisApp?.ActiveUIDocument.Document, arc);
         }
         else
         {
            // Get start point, end point of the arc and the middle point on it 
            XYZ startPoint = arc.GetEndPoint(0);
            XYZ endPoint = arc.GetEndPoint(1);
            bool clockwise = (arc.Normal.Z == -1);

            // Get start angel and end angel of arc
            double startDegree = arc.GetEndParameter(0);
            double endDegree = arc.GetEndParameter(1);

            // Handle the case that the arc is clockwise
            if (clockwise && startDegree > 0 && endDegree > 0)
            {
               startDegree = 2 * Values.PI - startDegree;
               endDegree = 2 * Values.PI - endDegree;
            }
            else if (clockwise && startDegree < 0)
            {
               double temp = endDegree;
               endDegree = -1 * startDegree;
               startDegree = -1 * temp;
            }

            double sumDegree = (startDegree + endDegree) / 2;
            while (sumDegree > 2 * Values.PI)
            {
               sumDegree -= 2 * Values.PI;
            }

            while (sumDegree < -2 * Values.PI)
            {
               sumDegree += 2 * Values.PI;
            }

            XYZ? midPoint = m_revit?.Create.NewXYZ(arc.Center.X + arc.Radius * Math.Cos(sumDegree),
                arc.Center.Y + arc.Radius * Math.Sin(sumDegree), 0);
            Arc reversedArc = Arc.Create(endPoint, startPoint, midPoint);

            //Create grid
            grid = Grid.Create(m_thisApp?.ActiveUIDocument.Document, reversedArc);
         }

         return grid;
      }

      /// <summary>
      /// Create linear grid
      /// </summary>
      /// <param name="line">The linear curve to be transferred to grid</param>
      /// <returns>The newly created grid</returns>
      private Grid CreateLinearGrid(Line line)
      {
         Grid grid;

         // Create grid according to the bubble location
         if (m_bubbleLocation == BubbleLocation.StartPoint)
         {
            grid = Grid.Create(m_thisApp?.ActiveUIDocument.Document, line);
         }
         else
         {
            XYZ startPoint = line.GetEndPoint(1);
            XYZ endPoint = line.GetEndPoint(0);
            Line reversedLine = Line.CreateBound(startPoint, endPoint);
            grid = Grid.Create(m_thisApp?.ActiveUIDocument.Document, reversedLine);
         }

         return grid;
      }
      #endregion
   }
}
