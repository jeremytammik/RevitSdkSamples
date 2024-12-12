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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using Autodesk.Revit;
using Autodesk.Revit.Geometry;

using CreApp = Autodesk.Revit.Creation.Application;
using Element = Autodesk.Revit.Element;
using GElement = Autodesk.Revit.Geometry.Element;
using Color = System.Drawing.Color;

namespace Revit.SDK.Samples.FamilyExplorer.CS
{
    /// <summary>
    /// Represents the wire frame of family component or generic form.
    /// </summary>
    public class ComponentWireFrame
    {
        /// <summary>
        /// The points of component geometry.
        /// </summary>
        private XYZArray m_wireFrame3D;

        /// <summary>
        /// the FamilyWireFrame which the ComponentWireFrame belongs to.
        /// </summary>
        private FamilyWireFrame m_parent;

        /// <summary>
        /// component brush.
        /// </summary>
        private Brush m_brush;

        /// <summary>
        /// does this component contains geometry.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return m_wireFrame3D.IsEmpty;
            }
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        /// <param name="element">the element provides wireframe to preview</param>
        /// <param name="creApp">used to create new instances of utility objects. </param>
        /// <param name="brush">the brush to paint the wireframe with specified color</param>
        /// <param name="parent">the FamilyWireFrame which the ComponentWireFrame belongs to</param>
        public ComponentWireFrame(Element element, CreApp creApp, Brush brush, FamilyWireFrame parent)
        {
            if (null == element)
            {
                throw new ArgumentNullException("element");
            }
            if (null == creApp)
            {
                throw new ArgumentNullException("creApp");
            }
            if (null == brush)
            {
                throw new ArgumentNullException("brush");
            }
            if (null == parent)
            {
                throw new ArgumentNullException("parent");
            }

            Options opt = creApp.NewGeometryOptions();
            opt.ComputeReferences = true;
            opt.DetailLevel = Options.DetailLevels.Fine;

            m_wireFrame3D = creApp.NewXYZArray();
            m_brush = brush;
            m_parent = parent;
            
            //Transform the element if it is necessary.
            Transform trf = null;
            LocationPoint lp = element.Location as LocationPoint;            

            if (null == lp)
            {
                trf = Transform.Identity;
            }
            else
            {
                XYZ vector = new XYZ(lp.Point.X, lp.Point.Y, lp.Point.Z);
                trf = Transform.get_Translation(vector);       
            }

            //get the element geometry primitives.
            PrimitivesParser collapse = new PrimitivesParser();
            collapse.CollapsePoints(element.get_Geometry(opt), m_wireFrame3D, trf, m_parent);
        }

        /// <summary>
        /// For Internal Use Only.
        /// </summary>
        private ComponentWireFrame()
        {
            throw new System.InvalidOperationException();
        }

        /// <summary>
        /// zoom the component's size
        /// </summary>
        /// <param name="scale">how much to scale</param>
        public void Zoom(double scale)
        {
            for (int i = 0; i < m_wireFrame3D.Size; i = i + 1 )
            {                
                XYZ point = m_wireFrame3D.get_Item(i);
                XYZ distance = new XYZ((point.X - m_parent.Origin.X) * scale
                    , (point.Y - m_parent.Origin.Y) * scale
                    , (point.Z - m_parent.Origin.Z) * scale);
                Transform trf = Transform.get_Translation(distance);
                XYZ origin = m_parent.Origin;
                m_wireFrame3D.set_Item(i, trf.OfPoint(origin));
            }
        }

        /// <summary>
        /// translate the component with an offset
        /// </summary>
        /// <param name="offsetX">distance in x axis</param>
        /// <param name="offsetY">distance in y axis</param>
        /// <param name="offsetZ">distance in z axis</param>
        public void Translate(double offsetX, double offsetY, double offsetZ)
        {
            XYZ vector = new XYZ(offsetX, offsetY, offsetZ);
            Transform trf = Transform.get_Translation(vector);
            for (int i = 0; i < m_wireFrame3D.Size; i = i + 1)
            {
                XYZ item = m_wireFrame3D.get_Item(i);                
                XYZ itemTrf = trf.OfPoint(item);
                m_wireFrame3D.set_Item(i, itemTrf);
            }
        }

        /// <summary>
        /// rotate around the origin with the angle.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        public void Rotate(XYZ origin, XYZ axis, double angle)
        {
            Transform trf = Transform.get_Rotation(origin, axis, angle);
            for (int i = 0; i < m_wireFrame3D.Size; i = i + 1)
            {
                XYZ item = m_wireFrame3D.get_Item(i);
                
                XYZ itemTrf = trf.OfPoint(item);
                m_wireFrame3D.set_Item(i, itemTrf);
            }
        }

        /// <summary>
        /// draw the wire frame with the Graphics object.
        /// </summary>
        /// <param name="graphics">Encapsulates a GDI+ drawing surface.</param>
        /// <returns>return the number of points that have been drawn</returns>
        public int RawDraw(Graphics graphics)
        {
            if (null == graphics)
            {
                throw new ArgumentNullException("graphics");
            }

            if (0 == m_wireFrame3D.Size)
            {
                return 0;
            }

            Pen pen = new Pen(m_brush);
            int num = m_wireFrame3D.Size;
            if (1 == num % 2)
            {
                num = num - 1;
            }
            //draw the line one by one.
            for (int i = 0; i < num; i = i + 2)
            {
                XYZ pstart = m_wireFrame3D.get_Item(i);
                PointF start = new PointF(Convert.ToSingle(pstart.X)
                , Convert.ToSingle(pstart.Y));

                XYZ pend = m_wireFrame3D.get_Item(i + 1);
                PointF end = new PointF(Convert.ToSingle(pend.X)
                , Convert.ToSingle(pend.Y));
                graphics.DrawLine(pen, start, end);
            }
            pen.Dispose();

            return m_wireFrame3D.Size;
        }
    }
}
