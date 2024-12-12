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
using System.Drawing;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Geometry;

using CreApp = Autodesk.Revit.Creation.Application;
using Element = Autodesk.Revit.Element;
using GElement = Autodesk.Revit.Geometry.Element;

namespace Revit.SDK.Samples.FamilyExplorer.CS
{
    /// <summary>
    /// represents wire frame of the family
    /// </summary>
    public class FamilyWireFrame
    {
        /// <summary>
        /// wrappers of family components
        /// </summary>
        private List<ComponentWireFrame> m_components;
        /// <summary>
        /// wrappers of family others
        /// </summary>
        private List<ComponentWireFrame> m_others;
        /// <summary>
        /// wrappers of family solid forms
        /// </summary>
        private List<ComponentWireFrame> m_solidForms;
        /// <summary>
        /// wrappers of family void forms
        /// </summary>
        private List<ComponentWireFrame> m_voidForms;

        /// <summary>
        /// The Application Creation object is used to create new instances of utility objects. 
        /// </summary>
        private CreApp m_creApp;

        private Family m_family;   // the family to reprent.
        private BoundingBoxXYZ m_bBox;  // the bounding box of the family
        private XYZ m_origin;   // the origin point of the family.
        private XYZ m_upDirection;  // the up direction of the family.
        private XYZ m_rightDirection;  // the right direction of the family.
        private XYZ m_frontDirection;   // the front direction of the family.
        private bool m_isInitialized;  // the flag whether the profile is initialized.

        /// <summary>
        /// Family name to display.
        /// </summary>
        public string Name
        {
            get
            {
                return m_family.Name;
            }
        }

        /// <summary>
        /// the parameters of this family
        /// </summary>
        public string Parameters
        {
            get
            {
                string message = null;
                foreach (Parameter para in m_family.Parameters)
                {
                    message += para.Definition.Name + " : " + ParseParameter(para) + "\n";
                }
                return message;
            }
        }

        /// <summary>
        ///  the bounding box of the family
        /// </summary>
        public BoundingBoxXYZ BoundingBox
        {
            get
            {
                return m_bBox;
            }
            set
            {
                m_bBox = value;
            }
        }

        /// <summary>
        /// the origin point of the family.
        /// </summary>
        public XYZ Origin
        {
            get
            {
                return m_origin;
            }
            set
            {
                m_origin = value;
            }
        }

        /// <summary>
        /// Whether the family has profile to preview.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (0 == m_components.Count
                && 0 == m_others.Count
                && 0 == m_solidForms.Count
                && 0 == m_voidForms.Count) ? true : false;
            }
        }

        /// <summary>
        /// For Internal Use Only.
        /// </summary>
        private FamilyWireFrame()
        {
        }

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="family">the family to show profile</param>
        /// <param name="creApp">The Application Creation object is
        /// used to create new instances of utility objects. </param>
        public FamilyWireFrame(Family family, CreApp creApp)
        {
            if (null == family)
            {
                throw new ArgumentNullException("family");
            }
            else
            {
                m_family = family;
            }

            if (null == creApp)
            {
                throw new ArgumentNullException("creApp");
            }
            else
            {
                m_creApp = creApp;
            }
        }

        /// <summary>
        /// Initialize the element geometry for object ComponetShow.
        /// </summary>
        public void InitilizeProfile()
        {
            //initialize an invalid bounding box
            m_bBox = new BoundingBoxXYZ();
            m_bBox.Max = new XYZ(double.MinValue, double.MinValue, double.MinValue);
            m_bBox.Min = new XYZ(double.MaxValue, double.MaxValue, double.MaxValue);

            //initialize all components of the family
            m_components = new List<ComponentWireFrame>();
            foreach (Element element in m_family.Components)
            {
                if (null == element)
                {
                    continue;
                }
                ComponentWireFrame cs = new ComponentWireFrame(element, m_creApp, Brushes.Black, this);
                if (!cs.IsEmpty)
                {
                    m_components.Add(cs);
                }
            }

            //initialize all others of the family
            m_others = new List<ComponentWireFrame>();
            foreach (Element element in m_family.Others)
            {
                if (null == element)
                {
                    continue;
                }
                ComponentWireFrame cs = new ComponentWireFrame(element, m_creApp, Brushes.Gray, this);
                if (!cs.IsEmpty)
                {
                    m_others.Add(cs);
                }
            }

            //initialize all solid forms of the family
            m_solidForms = new List<ComponentWireFrame>();
            foreach (GenericForm gf in m_family.SolidForms)
            {
                if (null == gf)
                {
                    continue;
                }
                ComponentWireFrame cs = new ComponentWireFrame(gf, m_creApp, Brushes.Green, this);
                if (!cs.IsEmpty)
                {
                    m_solidForms.Add(cs);
                }
            }

            //initialize all void forms of the family
            m_voidForms = new List<ComponentWireFrame>();
            foreach (GenericForm gf in m_family.VoidForms)
            {
                if (null == gf)
                {
                    continue;
                }
                ComponentWireFrame cs = new ComponentWireFrame(gf, m_creApp, Brushes.Yellow, this);
                if (!cs.IsEmpty)
                {
                    m_voidForms.Add(cs);
                }
            }

            //initialize the origin of this family.
            m_origin = new XYZ((m_bBox.Max.X + m_bBox.Min.X) / 2,
                (m_bBox.Max.Y + m_bBox.Min.Y) / 2,
                (m_bBox.Max.Z + m_bBox.Min.Z) / 2);

            //initialize the family orientation
            m_upDirection = new XYZ(0, 0, 1);
            m_rightDirection = new XYZ(1, 0, 0);
            m_frontDirection = new XYZ(0, 1, 0);

            // a flag show this object is initialized.
            m_isInitialized = true;
        }        

        /// <summary>
        /// auto fit the family profile to preview.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void AutoFit(Graphics graphics, double width, double height)
        {
            graphics.Clear(System.Drawing.Color.White);
            if (!m_isInitialized)
            {
                InitilizeProfile();

                if (!this.IsEmpty)// rotate this family to appropriate orientation
                {                    
                    XYZ origin = m_origin;
                    XYZ axis = new XYZ(1, 0, -1);
                    double angle = Math.PI / 4;

                    //rotate the all components of this family
                    foreach (ComponentWireFrame cs in m_components)
                    {
                        cs.Rotate(origin, axis, angle);
                    }
                    //rotate the all others of this family
                    foreach (ComponentWireFrame cs in m_others)
                    {
                        cs.Rotate(origin, axis, angle);
                    }
                    //rotate the all solid forms of this family
                    foreach (ComponentWireFrame cs in m_solidForms)
                    {
                        cs.Rotate(origin, axis, angle);
                    }
                    //rotate the all void forms of this family
                    foreach (ComponentWireFrame cs in m_voidForms)
                    {
                        cs.Rotate(origin, axis, angle);
                    }

                    // rotate the bounding box.
                    XYZ max = m_bBox.Max;
                    XYZ min = m_bBox.Min;
                    Transform rotate = Transform.get_Rotation(ref origin, ref axis, angle);
                    m_bBox.Max = rotate.OfPoint(ref max);
                    m_bBox.Min = rotate.OfPoint(ref min);

                    // rotate the orientation
                    m_upDirection = rotate.OfVector(ref m_upDirection).Normalized;
                    m_rightDirection = rotate.OfVector(ref m_rightDirection).Normalized;
                    m_frontDirection = rotate.OfVector(ref m_frontDirection).Normalized;
                }
            }

            if (this.IsEmpty)
            {
                graphics.DrawString("No profile for preview.", new Font("Arial", 12)
                    , new SolidBrush(System.Drawing.Color.Black)
                    , new PointF(Convert.ToSingle(width / 8), Convert.ToSingle(height / 8)));

                return;
            }
            else
            {
                // zoom the family to fit the screen
                double elementWidth = Math.Abs(m_bBox.Max.X - m_bBox.Min.X);
                double elementHeight = Math.Abs(m_bBox.Max.Y - m_bBox.Min.Y);
                if (0 == elementWidth || 0 == elementHeight)
                {
                    elementWidth = width / 2;
                    elementHeight = height / 2;
                }

                double xScale = Math.Abs((width / elementWidth));
                double yScale = Math.Abs((height / elementHeight));
                double scale = xScale < yScale ? xScale : yScale;
                scale = scale / 2;

                // zoom the all components of this family
                foreach (ComponentWireFrame cs in m_components)
                {
                    cs.Zoom(scale);
                }
                // zoom the all others of this family
                foreach (ComponentWireFrame cs in m_others)
                {
                    cs.Zoom(scale);
                }
                // zoom the all solid forms of this family
                foreach (ComponentWireFrame cs in m_solidForms)
                {
                    cs.Zoom(scale);
                }
                // zoom the all void forms of this family
                foreach (ComponentWireFrame cs in m_voidForms)
                {
                    cs.Zoom(scale);
                }
                
                // zoom the bounding box
                XYZ distance = new XYZ((m_bBox.Max.X - m_origin.X) * scale
                    , (m_bBox.Max.Y - m_origin.Y) * scale
                    , (m_bBox.Max.Z - m_origin.Z) * scale);
                Transform trf = Transform.get_Translation(ref distance);
                XYZ origin = m_origin;
                m_bBox.Max = trf.OfPoint(ref origin);

                distance = new XYZ((m_bBox.Min.X - m_origin.X) * scale
                    , (m_bBox.Min.Y - m_origin.Y) * scale
                    , (m_bBox.Min.Z - m_origin.Z) * scale);
                trf = Transform.get_Translation(ref distance);
                m_bBox.Min = trf.OfPoint(ref origin);

                //translate the family profile to fit the screen
                XYZ move = new XYZ(width / 2 - m_origin.X, height / 2 - m_origin.Y, 0);
                foreach (ComponentWireFrame cs in m_components)
                {
                    cs.Translate(move.X, move.Y, move.Z);
                    cs.RawDraw(graphics);
                }
                foreach (ComponentWireFrame cs in m_others)
                {
                    cs.Translate(move.X, move.Y, move.Z);
                    cs.RawDraw(graphics);
                }
                foreach (ComponentWireFrame cs in m_solidForms)
                {
                    cs.Translate(move.X, move.Y, move.Z);
                    cs.RawDraw(graphics);
                }
                foreach (ComponentWireFrame cs in m_voidForms)
                {
                    cs.Translate(move.X, move.Y, move.Z);
                    cs.RawDraw(graphics);
                }

                // translate the bounding box.
                trf = Transform.get_Translation(ref move);
                XYZ max = m_bBox.Max;
                m_bBox.Max = trf.OfPoint(ref max);
                XYZ min = m_bBox.Min;
                m_bBox.Min = trf.OfPoint(ref min);
                m_origin = trf.OfPoint(ref m_origin);

                // translate the orientation
                m_upDirection = trf.OfVector(ref m_upDirection).Normalized;
                m_rightDirection = trf.OfVector(ref m_rightDirection).Normalized;
                m_frontDirection = trf.OfVector(ref m_frontDirection).Normalized;
            }
        }

        /// <summary>
        /// rotate the family profile
        /// </summary>
        /// <param name="graphics">Encapsulates a GDI+ drawing surface.</param>
        /// <param name="rotateAxis">the mode of rotate</param>
        /// <param name="angle">the angle to rotate</param>
        /// <returns>how many points rotated</returns>
        public int Rotate(Graphics graphics, int rotateAxisMode, double angle)
        {
            int num = 0;
            XYZ origin = m_origin;
            XYZ axis = new XYZ(0, 1, 0);
            switch (rotateAxisMode)
            {
                case 0:
                    axis = m_upDirection;
                    break;
                case 1:
                    axis = m_rightDirection;
                    break;
                case 2:
                    axis = m_frontDirection;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("rotateAxis");
            }

            graphics.Clear(System.Drawing.Color.White);
            foreach (ComponentWireFrame cs in m_components)
            {
                cs.Rotate(origin, axis, angle);
                num += cs.RawDraw(graphics);
            }
            foreach (ComponentWireFrame cs in m_others)
            {
                cs.Rotate(origin, axis, angle);
                num += cs.RawDraw(graphics);
            }
            foreach (ComponentWireFrame cs in m_solidForms)
            {
                cs.Rotate(origin, axis, angle);
                num += cs.RawDraw(graphics);
            }
            foreach (ComponentWireFrame cs in m_voidForms)
            {
                cs.Rotate(origin, axis, angle);
                num += cs.RawDraw(graphics);
            }

            //rotate the bounding box
            XYZ max = m_bBox.Max;
            XYZ min = m_bBox.Min;
            Transform rotate = Transform.get_Rotation(ref origin, ref axis, angle);
            m_bBox.Max = rotate.OfPoint(ref max);
            m_bBox.Min = rotate.OfPoint(ref min);

            //rotate the orientation
            m_upDirection = rotate.OfVector(ref m_upDirection).Normalized;
            m_rightDirection = rotate.OfVector(ref m_rightDirection).Normalized;
            m_frontDirection = rotate.OfVector(ref m_frontDirection).Normalized;

            return num;
        }

        /// <summary>
        /// zoom the family profile
        /// </summary>
        /// <param name="graphics">Encapsulates a GDI+ drawing surface.</param>
        /// <param name="scale">how much to scale</param>
        /// <returns>how many points scaled</returns>
        public int Zoom(Graphics graphics, double scale)
        {
            int num = 0;
            graphics.Clear(System.Drawing.Color.White);

            foreach (ComponentWireFrame cs in m_components)
            {
                cs.Zoom(scale);
                num += cs.RawDraw(graphics);
            }
            foreach (ComponentWireFrame cs in m_others)
            {
                cs.Zoom(scale);
                num += cs.RawDraw(graphics);
            }
            foreach (ComponentWireFrame cs in m_solidForms)
            {
                cs.Zoom(scale);
                num += cs.RawDraw(graphics);
            }
            foreach (ComponentWireFrame cs in m_voidForms)
            {
                cs.Zoom(scale);
                num += cs.RawDraw(graphics);
            }

            // zoom the bounding box
            XYZ distance = new XYZ((m_bBox.Max.X - m_origin.X) * scale
                    , (m_bBox.Max.Y - m_origin.Y) * scale
                    , (m_bBox.Max.Z - m_origin.Z) * scale);
            Transform trf = Transform.get_Translation(ref distance);
            XYZ origin = m_origin;
            m_bBox.Max = trf.OfPoint(ref origin);

            distance = new XYZ((m_bBox.Min.X - m_origin.X) * scale
                , (m_bBox.Min.Y - m_origin.Y) * scale
                , (m_bBox.Min.Z - m_origin.Z) * scale);
            trf = Transform.get_Translation(ref distance);
            m_bBox.Min = trf.OfPoint(ref origin);

            return num;
        }

        /// <summary>
        /// translate the family profile
        /// </summary>
        /// <param name="graphics">Encapsulates a GDI+ drawing surface.</param>
        /// <param name="move">how much to translate</param>
        /// <returns></returns>
        public int Translate(Graphics graphics, XYZ move)
        {
            int num = 0;

            graphics.Clear(System.Drawing.Color.White);
            foreach (ComponentWireFrame cs in m_components)
            {
                cs.Translate(move.X, move.Y, move.Z);
                num += cs.RawDraw(graphics);
            }
            foreach (ComponentWireFrame cs in m_others)
            {
                cs.Translate(move.X, move.Y, move.Z);
                num += cs.RawDraw(graphics);
            }
            foreach (ComponentWireFrame cs in m_solidForms)
            {
                cs.Translate(move.X, move.Y, move.Z);
                num += cs.RawDraw(graphics);
            }
            foreach (ComponentWireFrame cs in m_voidForms)
            {
                cs.Translate(move.X, move.Y, move.Z);
                num += cs.RawDraw(graphics);
            }

            // translate the bounding box
            Transform trf = Transform.get_Translation(ref move);            
            XYZ max = m_bBox.Max;
            m_bBox.Max = trf.OfPoint(ref max);
            XYZ min = m_bBox.Min;
            m_bBox.Min = trf.OfPoint(ref min);
            m_origin = trf.OfPoint(ref m_origin);

            // translate the orientation
            m_upDirection = trf.OfVector(ref m_upDirection).Normalized;
            m_rightDirection = trf.OfVector(ref m_rightDirection).Normalized;
            m_frontDirection = trf.OfVector(ref m_frontDirection).Normalized;

            return num;
        }

        /// <summary>
        /// update the family bounding box
        /// </summary>
        /// <param name="point"></param>
        public void UpdateBoundingBox(XYZ point)
        {
            XYZ max = new XYZ(m_bBox.Max.X, m_bBox.Max.Y, m_bBox.Max.Z);
            max.X = max.X > point.X ? max.X : point.X;
            max.Y = max.Y > point.Y ? max.Y : point.Y;
            max.Z = max.Z > point.Z ? max.Z : point.Z;
            m_bBox.Max = max;

            XYZ min = new XYZ(m_bBox.Min.X, m_bBox.Min.Y, m_bBox.Min.Z);
            min.X = min.X < point.X ? min.X : point.X;
            min.Y = min.Y < point.Y ? min.Y : point.Y;
            min.Z = min.Z < point.Z ? min.Z : point.Z;
            m_bBox.Min = min;
        }

        /// <summary>
        /// parse the parameter value to string
        /// </summary>
        /// <param name="parameter">the parameter to parse</param>
        /// <returns>the parameter value string</returns>
        private string ParseParameter(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return parameter.AsDouble().ToString();
                case StorageType.ElementId:
                    return parameter.AsElementId().Value.ToString();
                case StorageType.Integer:
                    return parameter.AsInteger().ToString();
                case StorageType.String:
                    return parameter.AsString();
                default:
                    return null;
            }
        }
    }
}
