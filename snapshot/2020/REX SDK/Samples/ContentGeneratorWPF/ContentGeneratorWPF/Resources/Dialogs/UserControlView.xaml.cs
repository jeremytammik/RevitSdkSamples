//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using REX.Common.Geometry;
using System.Windows.Media.Media3D;

namespace REX.ContentGeneratorWPF.Resources.Dialogs
{
    /// <summary>
    /// Interaction logic for SubControl.xaml
    /// </summary>
    public partial class UserControlView : REX.Common.REXExtensionControl
    {
        /// <summary>
        /// Represents the bounding box of the whole element.
        /// </summary>
        BoundingBoxREXxyz BoundBox;
        /// <summary>
        /// The current geometry.
        /// </summary>
        List<Main.Triangle> Triangles;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserControlView"/> class.
        /// </summary>
        public UserControlView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserControlView"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public UserControlView(REX.Common.REXExtension extension)
            : base(extension)
        {
            InitializeComponent();
        }
        /// <summary>
        /// Draws the specified geometry described by the list of triangles.
        /// </summary>
        /// <param name="triangles">The triangles.</param>
        public void DrawGeometry(List<Main.Triangle> triangles)
        {
            Triangles = triangles;

            SetBoundingBox();
            DrawTriangles();
            SetCameraPosition();
        }
        /// <summary>
        /// Sets the bounding box based on the current list of triangles.
        /// </summary>
        void SetBoundingBox()
        {
            BoundingBoxREXxyz boundBox = new BoundingBoxREXxyz();

            foreach (Main.Triangle tr in Triangles)
            {
                boundBox.AddPoint(tr.Pt1);
                boundBox.AddPoint(tr.Pt2);
                boundBox.AddPoint(tr.Pt3);
            }

            BoundBox = boundBox;
        }
        /// <summary>
        /// Draws all triangles.
        /// </summary>
        void DrawTriangles()
        {
            geometry.TriangleIndices.Clear();
            geometry.Positions.Clear();

            foreach (Main.Triangle triangle in Triangles)
                DrawTriangle(triangle, geometry);          
        }
        /// <summary>
        /// Draws the specified triangle.
        /// </summary>
        /// <param name="triangle">The triangle.</param>
        /// <param name="geometry">The geometry.</param>
        void DrawTriangle(Main.Triangle triangle, MeshGeometry3D geometry)
        {
            int count = geometry.Positions.Count;
            REXxyz centerBoundBox = BoundBox.GetCenter();

            //All points are translated in this way to obtain the center of the bounding box in (0,0,0).

            geometry.Positions.Add(GetPoint3D(triangle.Pt1 - centerBoundBox));
            geometry.Positions.Add(GetPoint3D(triangle.Pt2 - centerBoundBox));
            geometry.Positions.Add(GetPoint3D(triangle.Pt3 - centerBoundBox));

            geometry.TriangleIndices.Add(count);
            geometry.TriangleIndices.Add(count+1);
            geometry.TriangleIndices.Add(count+2);           
        }
        /// <summary>
        /// Sets the camera position.
        /// </summary>
        void SetCameraPosition()
        {
            //The 0,0,0 point is the center of the scene.
            //Animation rotates the object about Z axis.
            REXxyz boundBoxDX = BoundBox.Max - BoundBox.Min;
            REXxyz centerBox = BoundBox.GetCenter();   
        
            //The start LookDirection is calculated based on the center and the maximum point
            //of the bounding box.
            REXxyz dx = centerBox - BoundBox.Max;       
     
            //The camera used in this sample is an OrthographicCamera and to be sure that all
            //content is visible on the viewer the appropriate width is calculated based on
            //the diagonal of the bounding box
            double width = dx.GetLength() * 2.2;
       
            //In order to have better view of the element some modification of the LookDirection
            //is required
            //for horizontal elements - to see them more from the top
            if (boundBoxDX.x > boundBoxDX.z || boundBoxDX.y > boundBoxDX.z)
            {
                dx.z *= 10;
            }
            else//for vertical elements - to see them more from the side
            {
                dx.z /= 10;
            }
                 
            REXxyz dxUnit = dx.GetUnitVector();            
            //The position of the camera is calculated based on the center of the bounding box
            //(which is in 0,0,0) and some default distance - here 100
            camera.Position = new Point3D( - dxUnit.x * 100,  - dxUnit.y * 100, - dxUnit.z * 100);
            camera.LookDirection = new Vector3D(dxUnit.x, dxUnit.y, dxUnit.z);
            directionLight.Direction = camera.LookDirection;
            directionLight2.Direction = new Vector3D(dxUnit.x, dxUnit.y, 0);
            REXxyz upDir = dxUnit.CrossProduct(new REXxyz(0, 0, 1)).CrossProduct(new REXxyz(dxUnit));
            camera.UpDirection = new Vector3D(upDir.x, upDir.y, upDir.z);
            camera.Width = width;
        }

        Point3D GetPoint3D(REXxyz pt)
        {
            return new Point3D(pt.x, pt.y, pt.z);
        }

        
    }
}
