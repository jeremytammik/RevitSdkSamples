//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.ComponentModel;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.NewRoof.RoofForms.CS
{
    /// <summary>
    /// The Util class is used to translate Revit coordination to windows coordination.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Translate a Revit 3D point to a windows 2D point according the boundingbox.
        /// </summary>
        /// <param name="pointXYZ">A Revit 3D point</param>
        /// <param name="boundingbox">The boundingbox of the roof whose footprint lines will be displayed in GDI.</param>
        /// <returns>A windows 2D point.</returns>
        static public PointF Translate(Autodesk.Revit.DB.XYZ pointXYZ, BoundingBoxXYZ boundingbox)
        {
            double centerX = (boundingbox.Min.X + boundingbox.Max.X) / 2;
            double centerY = (boundingbox.Min.Y + boundingbox.Max.Y) / 2;
            return new PointF((float)(pointXYZ.X - centerX), -(float)(pointXYZ.Y - centerY));
        }
    };

    /// <summary>
    /// The FootPrintRoofLine class is used to edit the foot print data of a footprint roof. 
    /// </summary>
    public class FootPrintRoofLine
    {
        // To store the footprint roof which the foot print data belong to.
        private FootPrintRoof m_roof;
        // To store the model curve data which the foot print data stand for.
        private ModelCurve m_curve;
        // To store the boundingbox of the roof
        private BoundingBoxXYZ m_boundingbox;
        /// <summary>
        /// The construct of the FootPrintRoofLine class.
        /// </summary>
        /// <param name="roof">The footprint roof which the foot print data belong to.</param>
        /// <param name="curve">The model curve data which the foot print data stand for.</param>
        public FootPrintRoofLine(FootPrintRoof roof, ModelCurve curve)
        {
            m_roof = roof;
            m_curve = curve;
            m_boundingbox = m_roof.get_BoundingBox(Revit.SDK.Samples.NewRoof.CS.Command.ActiveView);
        }

        /// <summary>
        /// Draw the footprint line in GDI.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        public void Draw(System.Drawing.Graphics graphics, System.Drawing.Pen pen)
        {
            Curve curve = m_curve.GeometryCurve;
            DrawCurve(graphics, pen, curve);
        }

        /// <summary>
        /// Draw the curve in GDI.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="curve"></param>
        private void DrawCurve(Graphics graphics, System.Drawing.Pen pen, Curve curve)
        {
            List<PointF> poinsts = new List<PointF>();
            foreach (Autodesk.Revit.DB.XYZ point in curve.Tessellate())
            {
                poinsts.Add(Util.Translate(point,m_boundingbox));
            }
            graphics.DrawCurve(pen, poinsts.ToArray());
        }

        /// <summary>
        /// Get the model curve data which the foot print data stand for.
        /// </summary>
        [Browsable(false)]
        public ModelCurve ModelCurve
        {
            get
            {
                return m_curve;
            }
        }

        /// <summary>
        /// Get the id value of the model curve.
        /// </summary>
        [Browsable(false)]
        public int Id
        {
            get
            {
                return m_curve.Id.IntegerValue;
            }
        }

        /// <summary>
        /// Get the name of the model curve.
        /// </summary>
        [Browsable(false)]
        public String Name
        {
            get
            {
                return m_curve.Name;
            }
        }

        /// <summary>
        /// Get/Set the slope definition of a model curve of the roof.
        /// </summary>
        [Description("The slope definition of the FootPrintRoof line.")]
        public bool DefinesSlope
        {
            get
            {
                return m_roof.get_DefinesSlope(m_curve);
            }
            set
            {
                m_roof.set_DefinesSlope(m_curve, value);
            }
        }

        /// <summary>
        /// Get/Set the slope angle of the FootPrintRoof line..
        /// </summary>
        [Description("The slope angle of the FootPrintRoof line.")]
        public double SlopeAngle
        {
            get
            {
                return m_roof.get_SlopeAngle(m_curve);
            }
            set
            {
                m_roof.set_SlopeAngle(m_curve, value);
            }
        }

        /// <summary>
        /// Get/Set the offset of the FootPrintRoof line.
        /// </summary>
        [Description("The offset of the FootPrintRoof line.")]
        public double Offset
        {
            get
            {
                return m_roof.get_Offset(m_curve);
            }
            set
            {
                m_roof.set_Offset(m_curve, value);
            }
        }

        /// <summary>
        /// Get/Set the overhang value of the FootPrintRoof line if the roof is created by picked wall.
        /// </summary>
        [Description("The overhang value of the FootPrintRoof line if the roof is created by picked wall.")]
        public double Overhang
        {
            get
            {
                return m_roof.get_Overhang(m_curve);
            }
            set
            {
                m_roof.set_Overhang(m_curve, value);
            }
        }

        /// <summary>
        /// Get/Set ExtendIntoWall value whether you want the overhang to be measured from the core of the wall or not.
        /// </summary>
        [Description("whether you want the overhang to be measured from the core of the wall or not.")]
        public bool ExtendIntoWall
        {
            get
            {
                return m_roof.get_ExtendIntoWall(m_curve);
            }
            set
            {
                m_roof.set_ExtendIntoWall(m_curve, value);
            }
        }
    };

    /// <summary>
    /// The FootPrintRoofWrapper class is use to edit a footprint roof in a PropertyGrid.
    /// It contains a footprint roof.
    /// </summary>
    public class FootPrintRoofWrapper
    {
        // To store the footprint roof which will be edited in a PropertyGrid.
        private FootPrintRoof m_roof;
        // To store the footprint line data of the roof which will be edited.
        private FootPrintRoofLine m_footPrintLine;
        // To store the footprint lines data of the roof.
        private List<FootPrintRoofLine> m_roofLines;

        // To store the boundingbox of the roof
        private BoundingBoxXYZ m_boundingbox;

        public event EventHandler OnFootPrintRoofLineChanged;
     
        /// <summary>
        /// The construct of the FootPrintRoofWrapper class.
        /// </summary>
        /// <param name="roof">The footprint roof which will be edited in a PropertyGrid.</param>
        public FootPrintRoofWrapper(FootPrintRoof roof)
        {
            m_roof = roof;
            m_roofLines = new List<FootPrintRoofLine>();
            ModelCurveArrArray curveloops = m_roof.GetProfiles();
            
            foreach(ModelCurveArray curveloop in curveloops)
            {
                foreach(ModelCurve curve in curveloop)
                {
                    m_roofLines.Add(new FootPrintRoofLine(m_roof, curve));
                }
            }

            FootPrintRoofLineConverter.SetStandardValues(m_roofLines);
            m_footPrintLine = m_roofLines[0];

            m_boundingbox = m_roof.get_BoundingBox(Revit.SDK.Samples.NewRoof.CS.Command.ActiveView);
        }


        /// <summary>
        /// Get the bounding box of the roof.
        /// </summary>
        [Browsable(false)]
        public BoundingBoxXYZ Boundingbox
        {
            get
            {
                return m_boundingbox;
            }
        }

        /// <summary>
        /// Get/Set the current footprint roof line which will be edited in the PropertyGrid.
        /// </summary>
        [TypeConverterAttribute(typeof(FootPrintRoofLineConverter)), Category("Footprint Roof Line Information")]
        public FootPrintRoofLine FootPrintLine
        {
            get
            {
                return m_footPrintLine;
            }
            set
            {
                m_footPrintLine = value;
                OnFootPrintRoofLineChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// The base level of the footprint roof.
        /// </summary>
        [TypeConverterAttribute(typeof(LevelConverter)), Category("Constrains")]
        [DisplayName("Base Level")]
        public Level BaseLevel
        {
            get
            {
                Parameter para = m_roof.get_Parameter(BuiltInParameter.ROOF_BASE_LEVEL_PARAM);
                return LevelConverter.GetLevelByID(para.AsElementId().IntegerValue);
            }
            set
            {
                // update base level
                Parameter para = m_roof.get_Parameter(BuiltInParameter.ROOF_BASE_LEVEL_PARAM);
                Autodesk.Revit.DB.ElementId id = new Autodesk.Revit.DB.ElementId(value.Id.IntegerValue);
                para.Set(id);
            }
        }

        /// <summary>
        /// The eave cutter type of the footprint roof.
        /// </summary>
        [Category("Construction")]
        [DisplayName("Rafter Cut")]
        [Description("The eave cutter type of the footprint roof.")]
        public EaveCutterType EaveCutterType
        {
            get
            {
                return m_roof.EaveCuts;
            }
            set
            {
                m_roof.EaveCuts = value;
            }
        }

        /// <summary>
        /// Get the footprint roof lines data.
        /// </summary>
        [Browsable(false)]
        public ReadOnlyCollection<FootPrintRoofLine> FootPrintRoofLines
        {
            get
            {
                return new ReadOnlyCollection<FootPrintRoofLine>(m_roofLines);
            }
        }

        /// <summary>
        /// Draw the footprint lines.
        /// </summary>
        /// <param name="graphics">The graphics object.</param>
        /// <param name="displayPen">A display pen.</param>
        /// <param name="highlightPen">A highlight pen.</param>
        public void DrawFootPrint(Graphics graphics, Pen displayPen, Pen highlightPen)
        {
            foreach (FootPrintRoofLine line in m_roofLines)
            {
                if (line.Id == m_footPrintLine.Id)
                {
                    line.Draw(graphics, highlightPen);
                }
                else
                {
                    line.Draw(graphics, displayPen);
                }
            }
        }
    }
}
