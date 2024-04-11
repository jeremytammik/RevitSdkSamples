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
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.GridCreation.CS
{
    /// <summary>
    /// Base class of all grid creation data class
    /// </summary>
    public class CreateGridsData
    {
        #region Fields
        /// <summary>
        /// The active document of Revit
        /// </summary>
        protected Autodesk.Revit.DB.Document m_revitDoc;
        /// <summary>
        /// Document Creation object to create new elements 
        /// </summary>
        protected Autodesk.Revit.Creation.Document m_docCreator;
        /// <summary>
        /// Application Creation object to create new elements
        /// </summary>
        protected Autodesk.Revit.Creation.Application m_appCreator;
        /// <summary>
        /// Array list contains all grid labels in current document
        /// </summary>
        private ArrayList m_labelsList;
        /// <summary>
        /// Current display unit type
        /// </summary>
        protected ForgeTypeId m_unit;
        /// <summary>
        /// Resource manager
        /// </summary>
        protected static System.Resources.ResourceManager resManager = Properties.Resources.ResourceManager;
        #endregion

        #region Properties
        /// <summary>
        /// Current display unit type
        /// </summary>
        public ForgeTypeId Unit
        {
            get
            {
                return m_unit;
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
        /// Constructor without display unit type
        /// </summary>
        /// <param name="application">Revit application</param>
        /// <param name="labels">All existing labels in Revit's document</param>
        public CreateGridsData(UIApplication application, ArrayList labels)
        {
            m_revitDoc = application.ActiveUIDocument.Document;
            m_appCreator = application.Application.Create;
            m_docCreator = application.ActiveUIDocument.Document.Create;
            m_labelsList = labels;
        }

        /// <summary>
        /// Constructor with display unit type
        /// </summary>
        /// <param name="application">Revit application</param>
        /// <param name="labels">All existing labels in Revit's document</param>
        /// <param name="unit">Current length display unit type</param>
        public CreateGridsData(UIApplication application, ArrayList labels, ForgeTypeId unit)
        {
            m_revitDoc = application.ActiveUIDocument.Document;
            m_appCreator = application.Application.Create;
            m_docCreator = application.ActiveUIDocument.Document.Create;
            m_labelsList = labels;
            m_unit = unit;
        }

        /// <summary>
        /// Get the line to create grid according to the specified bubble location
        /// </summary>
        /// <param name="line">The original selected line</param>
        /// <param name="bubLoc">bubble location</param>
        /// <returns>The line to create grid</returns>
        protected Line TransformLine(Line line, BubbleLocation bubLoc)
        {
            Line lineToCreate;

            // Create grid according to the bubble location
            if (bubLoc == BubbleLocation.StartPoint)
            {
                lineToCreate = line;
            }
            else
            {
                Autodesk.Revit.DB.XYZ startPoint = line.GetEndPoint(1);
                Autodesk.Revit.DB.XYZ endPoint = line.GetEndPoint(0);
                lineToCreate = NewLine(startPoint, endPoint);
            }

            return lineToCreate;
        }

        /// <summary>
        /// Get the arc to create grid according to the specified bubble location
        /// </summary>
        /// <param name="arc">The original selected line</param>
        /// <param name="bubLoc">bubble location</param>
        /// <returns>The arc to create grid</returns>
        protected Arc TransformArc(Arc arc, BubbleLocation bubLoc)
        {
            Arc arcToCreate;

            if (bubLoc == BubbleLocation.StartPoint)
            {
                arcToCreate = arc;
            }
            else
            {
                // Get start point, end point of the arc and the middle point on it 
                Autodesk.Revit.DB.XYZ startPoint = arc.GetEndPoint(0);
                Autodesk.Revit.DB.XYZ endPoint = arc.GetEndPoint(1);
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

                Autodesk.Revit.DB.XYZ midPoint = new Autodesk.Revit.DB.XYZ(arc.Center.X + arc.Radius * Math.Cos(sumDegree),
                    arc.Center.Y + arc.Radius * Math.Sin(sumDegree), 0);

                arcToCreate = Arc.Create(endPoint, startPoint, midPoint);
            }

            return arcToCreate;
        }

        /// <summary>
        /// Get the arc to create grid according to the specified bubble location
        /// </summary>
        /// <param name="origin">Arc grid's origin</param>
        /// <param name="radius">Arc grid's radius</param>
        /// <param name="startDegree">Arc grid's start degree</param>
        /// <param name="endDegree">Arc grid's end degree</param>
        /// <param name="bubLoc">Arc grid's Bubble location</param>
        /// <returns>The expected arc to create grid</returns>
        protected Arc TransformArc(Autodesk.Revit.DB.XYZ origin, double radius, double startDegree, double endDegree,
            BubbleLocation bubLoc)
        {
            Arc arcToCreate;
            // Get start point and end point of the arc and the middle point on the arc
            Autodesk.Revit.DB.XYZ startPoint = new Autodesk.Revit.DB.XYZ(origin.X + radius * Math.Cos(startDegree),
                origin.Y + radius * Math.Sin(startDegree), origin.Z);
            Autodesk.Revit.DB.XYZ midPoint = new Autodesk.Revit.DB.XYZ(origin.X + radius * Math.Cos((startDegree + endDegree) / 2),
                origin.Y + radius * Math.Sin((startDegree + endDegree) / 2), origin.Z);
            Autodesk.Revit.DB.XYZ endPoint = new Autodesk.Revit.DB.XYZ(origin.X + radius * Math.Cos(endDegree),
                origin.Y + radius * Math.Sin(endDegree), origin.Z);

            if (bubLoc == BubbleLocation.StartPoint)
            {
                arcToCreate = Arc.Create(startPoint, endPoint, midPoint);
            }
            else
            {
                arcToCreate = Arc.Create(endPoint, startPoint, midPoint);
            }

            return arcToCreate;
        }

        /// <summary>
        /// Split a circle into the upper and lower parts
        /// </summary>
        /// <param name="arc">Arc to be split</param>
        /// <param name="upperArc">Upper arc of the circle</param>
        /// <param name="lowerArc">Lower arc of the circle</param>
        /// <param name="bubLoc">bubble location</param>
        protected void TransformCircle(Arc arc, ref Arc upperArc, ref Arc lowerArc, BubbleLocation bubLoc)
        {
            Autodesk.Revit.DB.XYZ center = arc.Center;
            double radius = arc.Radius;
            Autodesk.Revit.DB.XYZ XRightPoint = new Autodesk.Revit.DB.XYZ(center.X + radius, center.Y, 0);
            Autodesk.Revit.DB.XYZ XLeftPoint = new Autodesk.Revit.DB.XYZ(center.X - radius, center.Y, 0);
            Autodesk.Revit.DB.XYZ YUpperPoint = new Autodesk.Revit.DB.XYZ(center.X, center.Y + radius, 0);
            Autodesk.Revit.DB.XYZ YLowerPoint = new Autodesk.Revit.DB.XYZ(center.X, center.Y - radius, 0);
            if (bubLoc == BubbleLocation.StartPoint)
            {
                upperArc = Arc.Create(XRightPoint, XLeftPoint, YUpperPoint);
                lowerArc = Arc.Create(XLeftPoint, XRightPoint, YLowerPoint);
            }
            else
            {
                upperArc = Arc.Create(XLeftPoint, XRightPoint, YUpperPoint);
                lowerArc = Arc.Create(XRightPoint, XLeftPoint, YLowerPoint);
            }
        }

        /// <summary>
        /// Create a new bound line
        /// </summary>
        /// <param name="start">start point of line</param>
        /// <param name="end">end point of line</param>
        /// <returns></returns>
        protected Line NewLine(Autodesk.Revit.DB.XYZ start, Autodesk.Revit.DB.XYZ end)
        {
            return Line.CreateBound(start, end);
        }

        /// <summary>
        /// Create a grid with a line
        /// </summary>
        /// <param name="line">Line to create grid</param>
        /// <returns>Newly created grid</returns>
        protected Grid NewGrid(Line line)
        {
           return Grid.Create(m_revitDoc, line);
        }

        /// <summary>
        /// Create a grid with an arc
        /// </summary>
        /// <param name="arc">Arc to create grid</param>
        /// <returns>Newly created grid</returns>
        protected Grid NewGrid(Arc arc)
        {
           return Grid.Create(m_revitDoc, arc);
        }

        /// <summary>
        /// Create linear grid
        /// </summary>
        /// <param name="line">The linear curve to be transferred to grid</param>
        /// <returns>The newly created grid</returns>
        /// 
        protected Grid CreateLinearGrid(Line line)
        {
           return Grid.Create(m_revitDoc, line);
        }

        /// <summary>
        /// Create batch of grids with curves
        /// </summary>
        /// <param name="curves">Curves used to create grids</param>
        protected void CreateGrids(CurveArray curves)
        {
           foreach (Curve c in curves)
           {
              Line line = c as Line;
              Arc arc = c as Arc;

              if (line != null)
              {
                 Grid.Create(m_revitDoc, line);
              }

              if (arc != null)
              {
                 Grid.Create(m_revitDoc, arc);
              }
           }
        }

        /// <summary>
        /// Add curve to curve array for batch creation
        /// </summary>
        /// <param name="curves">curve array stores all curves for batch creation</param>
        /// <param name="curve">curve to be added</param>
        public static void AddCurveForBatchCreation(ref CurveArray curves, Curve curve)
        {
            curves.Append(curve);
        }

        /// <summary>
        /// Show a message box
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="caption">title of message box</param>
        public static void ShowMessage(String message, String caption)
        {
            TaskDialog.Show(caption, message, TaskDialogCommonButtons.Ok);
        }
        #endregion
    }
}
