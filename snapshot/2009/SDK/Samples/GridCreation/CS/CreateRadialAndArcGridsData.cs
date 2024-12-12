//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Enums;
using Application = Autodesk.Revit.Application;

namespace Revit.SDK.Samples.GridCreation.CS
{
    /// <summary>
    /// The dialog which provides the options of creating radial and arc grids
    /// </summary>
    public class CreateRadialAndArcGridsData : CreateGridsData
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
        private uint m_arcNumber;
        // Number of radial grids
        private uint m_lineNumber;
        // Radius of first arc grid
        private double m_arcFirstRadius;
        // Distance from origin to start point
        private double m_LineFirstDistance;
        // Bubble location of arc grids
        private BubbleLocation m_arcFirstBubbleLoc;
        // Bubble location of radial grids
        private BubbleLocation m_lineFirstBubbleLoc;
        // Label of first arc grid
        private String m_arcFirstLabel;
        // Label of first radial grid
        private String m_lineFirstLabel;
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
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="application">Application object</param>
        /// <param name="dut">Current length display unit type</param>
        /// <param name="labels">All existing labels in Revit's document</param>
        public CreateRadialAndArcGridsData(Application application, DisplayUnitType dut, ArrayList labels)
            : base(application, labels)
        {
        }

        /// <summary>
        /// Create grids
        /// </summary>
        public void CreateGrids()
        {
            if (CreateRadialGrids() != 0)
            {
                String failureReason = resManager.GetString("FailedToCreateRadialGrids") + "\r";
                failureReason += resManager.GetString("AjustValues");

                ShowMessage(failureReason, resManager.GetString("FailureCaptionCreateGrids"));
            }

            ArrayList failureReasons = new ArrayList();
            if (CreateArcGrids(ref failureReasons) != 0)
            {
                String failureReason = resManager.GetString("FailedToCreateArcGrids") +
                    resManager.GetString("Reasons") + "\r";
                if (failureReasons.Count != 0)
                {
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
        /// Create radial grids
        /// </summary>
        /// <returns>Number of grids failed to create</returns>
        private int CreateRadialGrids()
        {
            int errorCount = 0;

            // Curve array which stores all curves for batch creation
            CurveArray curves = new CurveArray();

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

                    XYZ startPoint;
                    XYZ endPoint;
                    double cos = Math.Cos(angel);
                    double sin = Math.Sin(angel);

                    if (m_arcNumber != 0)
                    {
                        // Grids will have an extension distance of m_ySpacing / 2
                        startPoint = new XYZ(m_xOrigin + m_LineFirstDistance * cos, m_yOrigin + m_LineFirstDistance * sin, 0);
                        endPoint = new XYZ(m_xOrigin + (m_arcFirstRadius + (m_arcNumber - 1) * m_arcSpacing + m_arcSpacing / 2) * cos,
                            m_yOrigin + (m_arcFirstRadius + (m_arcNumber - 1) * m_arcSpacing + m_arcSpacing / 2) * sin, 0);
                    }
                    else
                    {
                        startPoint = new XYZ(m_xOrigin + m_LineFirstDistance * cos, m_yOrigin + m_LineFirstDistance * sin, 0);
                        endPoint = new XYZ(m_xOrigin + (m_arcFirstRadius + 5) * cos, m_yOrigin + (m_arcFirstRadius + 5) * sin, 0);
                    }

                    Line line;
                    // Create a line according to the bubble location
                    if (m_lineFirstBubbleLoc == BubbleLocation.StartPoint)
                    {
                        line = NewLine(startPoint, endPoint);
                    }
                    else
                    {
                        line = NewLine(endPoint, startPoint);
                    }

                    if (i == 0)
                    {
                        // Create grid with line
                        Grid grid = NewGrid(line);

                        try
                        {
                            // Set label of first radial grid
                            grid.Name = m_lineFirstLabel;
                        }
                        catch (System.ArgumentException)
                        {
                            ShowMessage(resManager.GetString("FailedToSetLabel") + m_lineFirstLabel + "!",
                                        resManager.GetString("FailureCaptionSetLabel"));
                        }
                    }
                    else
                    {
                        // Add the line to curve array
                        AddCurveForBatchCreation(ref curves, line);
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

        /// <summary>
        /// Create Arc Grids
        /// </summary>
        /// <param name="failureReasons">ArrayList contains failure reasons</param>
        /// <returns>Number of grids failed to create</returns>
        private int CreateArcGrids(ref ArrayList failureReasons)
        {
            int errorCount = 0;

            // Curve array which stores all curves for batch creation
            CurveArray curves = new CurveArray();

            for (int i = 0; i < m_arcNumber; ++i)
            {
                try
                {
                    XYZ origin = new XYZ(m_xOrigin, m_yOrigin, 0);
                    double radius = m_arcFirstRadius + i * m_arcSpacing;

                    // In Revit UI user can select a circle to create a grid, but actually two grids 
                    // (One from 0 to 180 degree and the other from 180 degree to 360) will be created. 
                    // In RevitAPI using NewGrid method with a circle as its argument will raise an exception. 
                    // Therefore in this sample we will create two arcs from the upper and lower parts of the 
                    // circle, and then create two grids on the base of the two arcs to accord with UI.
                    if (m_endDegree - m_startDegree == 2 * Values.PI) // Create circular grids
                    {
                        Arc upperArcToCreate;
                        Arc lowerArcToCreate;
                        upperArcToCreate = TransformArc(origin, radius, 0, Values.PI, m_arcFirstBubbleLoc);

                        if (i == 0)
                        {
                            Grid gridUpper;
                            gridUpper = NewGrid(upperArcToCreate);
                            if (gridUpper != null)
                            {
                                try
                                {
                                    // Set label of first grid
                                    gridUpper.Name = m_arcFirstLabel;
                                }
                                catch (System.ArgumentException)
                                {
                                    ShowMessage(resManager.GetString("FailedToSetLabel") + m_arcFirstLabel + "!",
                                                resManager.GetString("FailureCaptionSetLabel"));
                                }
                            }
                        }
                        else
                        {
                            curves.Append(upperArcToCreate);
                        }

                        lowerArcToCreate = TransformArc(origin, radius, Values.PI, 2 * Values.PI, m_arcFirstBubbleLoc);
                        curves.Append(lowerArcToCreate);
                    }
                    else // Create arc grids
                    {
                        // Each arc grid will has extension degree of 15 degree
                        double extensionDegree = 15 * Values.DEGTORAD;
                        Grid grid;
                        Arc arcToCreate;

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
                                
                                arcToCreate = TransformArc(origin, radius, startDegreeWithExtension, endDegreeWithExtension, m_arcFirstBubbleLoc);
                            }
                            else
                            {
                                try
                                {
                                    arcToCreate = TransformArc(origin, radius, m_startDegree, m_endDegree, m_arcFirstBubbleLoc);
                                }
                                catch (System.ArgumentException)
                                {
                                    String failureReason = resManager.GetString("EndPointsTooClose");
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
                                arcToCreate = TransformArc(origin, radius, m_startDegree, m_endDegree, m_arcFirstBubbleLoc);
                            }
                            catch (System.ArgumentException)
                            {
                                String failureReason = resManager.GetString("EndPointsTooClose");
                                if (!failureReasons.Contains(failureReason))
                                {
                                    failureReasons.Add(failureReason);
                                }
                                errorCount++;
                                continue;
                            }
                        }


                        if (i == 0)
                        {
                            grid = NewGrid(arcToCreate);
                            if (grid != null)
                            {
                                try
                                {
                                    grid.Name = m_arcFirstLabel;
                                }
                                catch (System.ArgumentException)
                                {
                                    ShowMessage(resManager.GetString("FailedToSetLabel") + m_arcFirstLabel + "!",
                                                resManager.GetString("FailureCaptionSetLabel"));
                                } 
                            }                            
                        }
                        else
                        {
                            curves.Append(arcToCreate);
                        }
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
