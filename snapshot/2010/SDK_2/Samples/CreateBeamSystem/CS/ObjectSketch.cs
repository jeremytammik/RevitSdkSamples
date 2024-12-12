//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.CreateBeamSystem.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    /// <summary>
    /// base class of sketch object to draw 2D geometry object
    /// </summary>
    public abstract class ObjectSketch
    {
        /// <summary>
        /// bounding box of the geometry object 
        /// </summary>
        protected RectangleF m_boundingBox = new RectangleF();

        /// <summary>
        /// bounding box of the geometry object 
        /// </summary>
        public RectangleF BoundingBox
        {
            get 
            { 
                return m_boundingBox; 
            }
        }

        /// <summary>
        /// pen to draw the object
        /// </summary>
        protected Pen m_pen = new Pen(Color.DarkGreen);

        /// <summary>
        /// defines a local geometric transform
        /// </summary>
        protected Matrix m_transform;

        /// <summary>
        /// reserve lines that form the profile
        /// </summary>
        protected List<ObjectSketch> m_objects = new List<ObjectSketch>();

        /// <summary>
        /// geometric object draw itself
        /// </summary>
        /// <param name="g"></param>
        /// <param name="translate"></param>
        public abstract void Draw(Graphics g, Matrix translate);
    }
}
