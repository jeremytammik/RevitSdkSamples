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
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace Revit.SDK.Samples.GridCreation.CS
{
    /// <summary>
    /// Data class which stores information for creating orthogonal grids
    /// </summary>
    public class CreateOrthogonalGridsData : CreateGridsData
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
        private String m_xFirstLabel;
        // Label of first vertical grid
        private String m_yFirstLabel;
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
                return m_xFirstLabel;
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
                return m_yFirstLabel;
            }
            set 
            { 
                m_yFirstLabel = value; 
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="application">Application object</param>
        /// <param name="dut">Current length display unit type</param>
        /// <param name="labels">All existing labels in Revit's document</param>
        public CreateOrthogonalGridsData(UIApplication application, DisplayUnitType dut, ArrayList labels)
            : base(application, labels, dut)
        {
        }

        /// <summary>
        /// Create grids
        /// </summary>
        public void CreateGrids()
        {
            ArrayList failureReasons = new ArrayList();
            if (CreateXGrids(ref failureReasons) + CreateYGrids(ref failureReasons) != 0)
            {
                String failureReason = resManager.GetString("FailedToCreateGrids");
                if (failureReasons.Count != 0)
                {
                    failureReason += resManager.GetString("Reasons") + "\r";
                    failureReason += "\r";
                    foreach (String reason in failureReasons)
                    {
                        failureReason += reason + "\r";
                    }
                }
                
                failureReason += "\r" + resManager.GetString("AjustValues");

                ShowMessage(failureReason, resManager.GetString("FailureCaptionCreateGrids"));
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

            // Curve array which stores all curves for batch creation
            CurveArray curves = new CurveArray();

            for (int i = 0; i < m_xNumber; ++i)
            {
                Autodesk.Revit.DB.XYZ startPoint;
                Autodesk.Revit.DB.XYZ endPoint;
                Line line;

                try
                {
                    if (m_yNumber != 0)
                    {
                        // Grids will have an extension distance of m_ySpacing / 2
                        startPoint = new Autodesk.Revit.DB.XYZ (m_xOrigin - m_ySpacing / 2, m_yOrigin + i * m_xSpacing, 0);
                        endPoint = new Autodesk.Revit.DB.XYZ (m_xOrigin + (m_yNumber - 1) * m_ySpacing + m_ySpacing / 2, m_yOrigin + i * m_xSpacing, 0);
                    }
                    else
                    {
                        startPoint = new Autodesk.Revit.DB.XYZ (m_xOrigin, m_yOrigin + i * m_xSpacing, 0);
                        endPoint = new Autodesk.Revit.DB.XYZ (m_xOrigin + m_xSpacing / 2, m_yOrigin + i * m_xSpacing, 0);

                    }

                    try
                    {
                        // Create a line according to the bubble location
                        if (m_xBubbleLoc == BubbleLocation.StartPoint)
                        {
                            line = NewLine(startPoint, endPoint);
                        }
                        else
                        {
                            line = NewLine(endPoint, startPoint);
                        }
                    }
                    catch (System.ArgumentException)
                    {
                        String failureReason = resManager.GetString("SpacingsTooSmall");
                        if (!failureReasons.Contains(failureReason))
                        {
                            failureReasons.Add(failureReason);
                        }
                        errorCount++;
                        continue;
                    }

                    if (i == 0)
                    {
                        Grid grid;
                        // Create grid with line
                        grid = NewGrid(line);

                        try
                        {
                            // Set the label of first horizontal grid
                            grid.Name = m_xFirstLabel;
                        }
                        catch (System.ArgumentException)
                        {
                            ShowMessage(resManager.GetString("FailedToSetLabel") + m_xFirstLabel + "!",
                                        resManager.GetString("FailureCaptionSetLabel"));
                        }
                    }
                    else
                    {
                        // Add the line to curve array
                        curves.Append(line);
                    }
                }
                catch (Exception)
                {
                    ++errorCount;
                    continue;
                }                
            }

            // Create grids with curve array
            CreateGrids(curves);

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

            // Curve array which stores all curves for batch creation
            CurveArray curves = new CurveArray();

            for (int j = 0; j < m_yNumber; ++j)
            {
                Autodesk.Revit.DB.XYZ startPoint;
                Autodesk.Revit.DB.XYZ endPoint;
                Line line;

                try
                {
                    if (m_xNumber != 0)
                    {
                        startPoint = new Autodesk.Revit.DB.XYZ (m_xOrigin + j * m_ySpacing, m_yOrigin - m_xSpacing / 2, 0);
                        endPoint = new Autodesk.Revit.DB.XYZ (m_xOrigin + j * m_ySpacing, m_yOrigin + (m_xNumber - 1) * m_xSpacing + m_xSpacing / 2, 0);
                    }
                    else
                    {
                        startPoint = new Autodesk.Revit.DB.XYZ (m_xOrigin + j * m_ySpacing, m_yOrigin, 0);
                        endPoint = new Autodesk.Revit.DB.XYZ (m_xOrigin + j * m_ySpacing, m_yOrigin + m_ySpacing / 2, 0);
                    }

                    try
                    {
                        // Create a line according to the bubble location
                        if (m_yBubbleLoc == BubbleLocation.StartPoint)
                        {
                            line = NewLine(startPoint, endPoint);
                        }
                        else
                        {
                            line = NewLine(endPoint, startPoint);
                        }
                    }
                    catch (System.ArgumentException)
                    {
                        String failureReason = resManager.GetString("SpacingsTooSmall");
                        if (!failureReasons.Contains(failureReason))
                        {
                            failureReasons.Add(failureReason);
                        }
                        errorCount++;
                        continue;
                    }

                    if (j == 0)
                    {
                        Grid grid;
                        // Create grid with line
                        grid = NewGrid(line);

                        try
                        {
                            // Set label of first vertical grid
                            grid.Name = m_yFirstLabel;
                        }
                        catch (System.ArgumentException)
                        {
                            ShowMessage(resManager.GetString("FailedToSetLabel") + m_yFirstLabel + "!",
                                        resManager.GetString("FailureCaptionSetLabel"));
                        }
                    }
                    else
                    {
                        // Add the line to curve array
                        curves.Append(line);
                    }
                }
                catch (Exception)
                {
                    ++errorCount;
                    continue;
                }
            }

            // Create grids with curves
            CreateGrids(curves);

            return errorCount;
        }
        #endregion
    }
}
