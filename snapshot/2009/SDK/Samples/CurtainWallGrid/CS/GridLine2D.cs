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
using System.Drawing;

namespace Revit.SDK.Samples.CurtainWallGrid.CS
{
    /// <summary>
    /// a linear line in 2D point format
    /// </summary>
    public class Line2D
    {
        #region Fields
        // the start point of the line
       
        Point m_startPoint;
        // the end point of the line
        Point m_endPoint;
        #endregion

        #region Properties
        /// <summary>
        /// the start point of the line
        /// </summary>
        public Point StartPoint
        {
            get
            {
                return m_startPoint;
            }
            set
            {
                m_startPoint = value;
            }
        }

        /// <summary>
        /// the end point of the line
        /// </summary>
        public Point EndPoint
        {
            get
            {
                return m_endPoint;
            }
            set
            {
                m_endPoint = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// default constructor
        /// </summary>
        public Line2D()
        {
            m_startPoint = Point.Empty;
            m_endPoint = Point.Empty;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="startPoint">
        /// the start point for the line
        /// </param>
        /// <param name="endPoint">
        /// the end point for the line
        /// </param>
        public Line2D(Point startPoint, Point endPoint)
        {
            m_startPoint = startPoint;
            m_endPoint = endPoint;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="line2D">
        /// the line to be copied
        /// </param>
        public Line2D(Line2D line2D)
        {
            m_startPoint = line2D.StartPoint;
            m_endPoint = line2D.EndPoint;
        }
        #endregion
    }

    /// <summary>
    /// the class stores the baseline data for curtain wall
    /// </summary>
    public class WallBaseline2D : Line2D
    {
        #region Fields
        // an assistant point for temp usage
        Point m_assistantPoint;
        #endregion

        #region Properties
        /// <summary>
        /// an assistant point for temp usage
        /// </summary>
        public Point AssistantPoint
        {
            get
            {
            	return m_assistantPoint;
            }
            set
            {
            	 m_assistantPoint = value;
            }
        }       
        #endregion

        #region Constructors
        /// <summary>
        /// default constructor
        /// </summary>
        public WallBaseline2D() : base()
        {
            m_assistantPoint = Point.Empty;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="startPoint">
        /// the start point for the line
        /// </param>
        /// <param name="endPoint">
        /// the end point for the line
        /// </param>
        public WallBaseline2D(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            m_assistantPoint = Point.Empty;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="wallLine2D">
        /// the line to be copied
        /// </param>
        public WallBaseline2D(WallBaseline2D wallLine2D) : base ((Line2D) wallLine2D)
        {
            m_assistantPoint = wallLine2D.AssistantPoint;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// clear the stored data
        /// </summary>
        public void Clear()
        {
            this.StartPoint = Point.Empty;
            this.EndPoint = Point.Empty;
            m_assistantPoint = Point.Empty;
        }
        #endregion
    }

    /// <summary>
    /// the 2D format for the curtain grid line, it inherits from the Line2D class
    /// </summary>
    public class GridLine2D : Line2D
    {
        #region Fields
        // indicate whether the grid line is locked
        bool m_locked;
       
        // all the segments for the grid line
        List<SegmentLine2D> m_segments;
      
        // indicate how many segments have been removed from the grid line
        private int m_removedNumber;
      
        // indicate whether it's a U grid line
        private bool m_isUGridLine;
        #endregion

        #region Properties
        /// <summary>
        /// indicate whether the grid line is locked
        /// </summary>
        public bool Locked
        {
            get
            {
                return m_locked;
            }
            set
            {
                m_locked = value;
            }
        }

        /// <summary>
        /// all the segments for the grid line
        /// </summary>
        public List<SegmentLine2D> Segments
        {
            get
            {
                return m_segments;
            }
        }

        /// <summary>
        /// indicate how many segments have been removed from the grid line
        /// </summary>
        public int RemovedNumber
        {
            get
            {
            	return m_removedNumber;
            }
            set
            {
                m_removedNumber  = value;
            }
        }

        /// <summary>
        /// indicate whether it's a U grid line
        /// </summary>
        public bool IsUGridLine
        {
            get
            {
            	return m_isUGridLine;
            }
            set
            {
            	 m_isUGridLine = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// default constructor, initialize all the members with default value
        /// </summary>
        public GridLine2D()
            : base()
        {
            m_segments = new List<SegmentLine2D>();
            m_locked = false;
            m_removedNumber = 0;
            m_isUGridLine = false;
        }

        /// <summary>
        /// constructor, initialize the grid line with end points
        /// </summary>
        /// <param name="startPoint">
        /// the start point of the curtain grid line 2D
        /// </param>
        /// <param name="endPoint">
        /// the end point of the curtain grid line 2D
        /// </param>
        public GridLine2D(Point startPoint, Point endPoint)
            : base(startPoint, endPoint)
        {
            m_segments = new List<SegmentLine2D>();
            m_locked = false;
            m_removedNumber = 0;
            m_isUGridLine = false;
        }

        /// <summary>
        /// copy constructor, initialize the curtain grid line with another grid line 2D
        /// </summary>
        /// <param name="gridLine2D">
        /// the source line 
        /// </param>
        public GridLine2D(GridLine2D gridLine2D)
            : base((Line2D)gridLine2D)
        {
            m_segments = new List<SegmentLine2D>();
            m_locked = gridLine2D.Locked;
            m_removedNumber = gridLine2D.RemovedNumber;
            m_isUGridLine = gridLine2D.IsUGridLine;
            foreach (SegmentLine2D segLine in gridLine2D.Segments)
            {
                m_segments.Add(new SegmentLine2D(segLine));
            }
        }
        #endregion
    }

    /// <summary>
    /// the line class for the segment of grid line, it inherits from Line2D class
    /// </summary>
    public class SegmentLine2D : Line2D
    {
        #region Fields
        // indicates whether the segment is "isolated" 
        bool m_isolated;
        
        // indicate whether the segment has been removed from the grid line
        bool m_removed;
      
        // the index of the segment in the grid line
        private int m_segmentIndex;
    
        // the index of its parent grid line in all the curtain grid's U/V grid lines
        private int m_gridLineIndex;
   
        // indicates whether the segment is in a U grid line
        private bool m_isUSegment;
        #endregion

        #region Properties
        /// <summary>
        /// indicates whether the segment is "isolated" 
        /// </summary>
        public bool Isolated
        {
            get
            {
                return m_isolated;
            }
            set
            {
                m_isolated = value;
            }
        }
        #endregion

        #region 
        /// <summary>
        /// indicate whether the segment has been removed from the grid line
        /// </summary>
        public bool Removed
        {
            get
            {
                return m_removed;
            }
            set
            {
                m_removed = value;
            }
        }

        /// <summary>
        /// the index of the segment in the grid line
        /// </summary>
        public int SegmentIndex
        {
            get
            {
                return m_segmentIndex;
            }
            set
            {
                m_segmentIndex = value;
            }
        }

        /// <summary>
        /// the index of its parent grid line in all the curtain grid's U/V grid lines
        /// </summary>
        public int GridLineIndex
        {
            get
            {
            	return m_gridLineIndex;
            }
            set
            {
            	 m_gridLineIndex = value;
            }
        }

        /// <summary>
        /// indicates whether the segment is in a U grid line
        /// </summary>
        public bool IsUSegment
        {
            get
            {
            	return m_isUSegment;
            }
            set
            {
            	 m_isUSegment = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// default constructor
        /// </summary>
        public SegmentLine2D()
            : base()
        {
            m_removed = false;
            m_isolated = false;
            m_segmentIndex = -1;
            m_gridLineIndex = -1;
        }

        /// <summary>
        /// constructor, initialize the segment with end points
        /// </summary>
        /// <param name="startPoint">
        /// the start point of the segment
        /// </param>
        /// <param name="endPoint">
        /// the end point of the segment
        /// </param>
        public SegmentLine2D(Point startPoint, Point endPoint)
            : base(startPoint, endPoint)
        {
            m_removed = false;
            m_isolated = false;
            m_segmentIndex = -1;
            m_gridLineIndex = -1;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="segLine2D">
        /// the source segment line 2D
        /// </param>
        public SegmentLine2D(SegmentLine2D segLine2D)
            : base((Line2D)segLine2D)
        {
            m_removed = segLine2D.Removed;
            m_isolated = segLine2D.Isolated;
            m_segmentIndex = segLine2D.SegmentIndex;
            m_gridLineIndex = segLine2D.GridLineIndex;
        }
        #endregion
    }

}
