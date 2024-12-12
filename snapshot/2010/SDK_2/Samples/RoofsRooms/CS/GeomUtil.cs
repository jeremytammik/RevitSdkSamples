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

using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using System.Diagnostics;

namespace Revit.SDK.Samples.RoofsRooms.CS
{
    /// <summary>
    /// This class is one utility class, 
    /// used to check geometry relationship between rooms(spaces) and roofs
    /// </summary>
    public class GeomUtil
    {
        #region Class Const Variable
        /// <summary>
        /// Tolerance of double, the same as value in Revit.
        /// </summary>
        const double DOUBLE_EPS = 1.0e-09;
        #endregion

        #region Class Public Methods
        /// <summary>
        /// Judge whether two bounding box intersect or not.
        /// </summary>
        /// <param name="box1">First box.</param>
        /// <param name="box2">Second box.</param>
        /// <returns>Return true if two BoundingBoxs can intersect, else false.</returns>
        /// <remarks>The epislon is 1.0e-09, same as value in Revit API.</remarks>
        public static bool BoxIntersect(BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            // Check for no overlap
            if ((box1.Min.X - box2.Max.X) > DOUBLE_EPS ||
                (box2.Min.X - box1.Max.X) > DOUBLE_EPS ||
                (box1.Min.Y - box2.Max.Y) > DOUBLE_EPS ||
                (box2.Min.Y - box1.Max.Y) > DOUBLE_EPS ||
                (box1.Min.Z - box2.Max.Z) > DOUBLE_EPS ||
                (box2.Min.Z - box1.Max.Z) > DOUBLE_EPS)
            {
                return false; 
            }
            return true;
        }
        
        /// <summary>
        /// Roof cut elements(Room and Space)
        /// </summary>
        /// <param name="roof">Roof Element</param>
        /// <param name="element">Room or Space element.</param>
        /// <returns>Return true if Roof can cut Room or Space. </returns>
        static public bool RoofCutElement (Autodesk.Revit.Element roof, Autodesk.Revit.Element element)
        {
            Room room = element as Room;
            GeometryObjectArray closedShellObjects = null;
            if (null != room)
            {
                closedShellObjects = room.ClosedShell.Objects;
            }
            else
            {
                Space space = element as Space;
                if (null != space)
                {
                    closedShellObjects = space.ClosedShell.Objects;
                }
            }
            //
            // only check Solid geometry
            if (null != closedShellObjects)
            {
                return RoofCutRoomGeometry(roof, closedShellObjects);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Judge whether the roof can cut the room or not, check them by Solid faces
        /// </summary>
        /// <param name="roof">Roof element.</param>
        /// <param name="room">Room element.</param>
        /// <returns>Return true if Roof can cut Room, else fasle.</returns>
        /// <remarks>This method will return true
        /// if anyone solid of Roof and Room can cut each other.</remarks>
        static public bool RoofCutRoomGeometry(Autodesk.Revit.Element roof, GeometryObjectArray roomGeomArray)
        {
           foreach (GeometryObject roomGeom in roomGeomArray)
            {
                // only check the solid geometry
                Solid roomSolid = roomGeom as Solid;
                if (roomSolid == null || roomSolid.Faces.Size == 0)
                {
                    continue;
                }
                //Get geometry array of roof, use default geometry options
                GeometryObjectArray roofGeomObjs = roof.get_Geometry(new Options()).Objects;
                foreach (GeometryObject roofGeom in roofGeomObjs)
                {
                    // only check the solid object
                    Solid roofSolid = roofGeom as Solid;
                    if (roofSolid == null || roofSolid.Faces.Size == 0)
                    {
                        continue;
                    }
                    // 
                    // Check if two solids can intersect 
                    if (AreSolidsCut(roofSolid, roomSolid))
                    {
                        // return true once roof solids can intersect room solid
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Class Implementations
        /// <summary>
        /// Judge whether room solid faces are subset of roof solid faces
        /// </summary>
        /// <param name="roofSolid">Roof solid geometry.</param>
        /// <param name="roomSolid">Room solid geometry.</param>
        /// <returns>Return true if someone solids face is subset of roof's solids faces,
        /// else return false. </returns>
        static private bool AreSolidsCut(Solid roofSolid, Solid roomSolid)
        {
            // skip the empty object
            if (roofSolid.Faces.Size == 0 || roomSolid.Faces.Size == 0)
            {
                return false;
            }
            // 
            // check if someone room face is subset of roof's faces            
            foreach (Face roomFace in roomSolid.Faces)
            {
                foreach (Face roofFace in roofSolid.Faces)
                {
                    if (IsSubset(roofFace, roomFace))
                    {
                        return true; // room face is subset of solid face of roof.
                    }
                }
            }      
            // 
            // return false if room dones't have any face which is subset of roof's faces.
            return false;
        }

        /// <summary>
        /// Judge whether the second face(room face) is the subset of the first(roof face) or not.
        /// </summary>
        /// <param name="roofFace">Roof face geometry.</param>
        /// <param name="roomFace">Room face geometry.</param>
        /// <returns>Return true if faces are coincident and overlap.</returns>
        static private bool IsSubset(Face roofFace, Face roomFace)
        {
            // only check same type of face
            if (roofFace.GetType() != roomFace.GetType())
            {
                return false;
            }
            // 
            // Check if two faces are coincident 
            if (!AreFacesCoincident(roofFace, roomFace))
            {
                return false;
            }
            // 
            // Again, check if two faces are overlap, check them by edges of faces
            return AreFacesOverlap(roofFace, roomFace);
        }

        /// <summary>
        /// Judge whether two bound faces have same points set or not, check them by edge points.
        /// </summary>
        /// <param name="roofFace">Roof face geometry.</param>
        /// <param name="roomFace">Room face geometry.</param>
        /// <returns>Return true if faces of Roof and Room overlap each other,else false.
        /// </returns>
        static private bool AreFacesOverlap(Face roofFace, Face roomFace)
        {
            foreach (EdgeArray edges in roomFace.EdgeLoops)
            {                
                foreach (Edge edge in edges)
                {
                    XYZArray epts = edge.Tessellate();
                    foreach (XYZ ept in epts)
                    {
                        // project the point to face and then check if the distance of point
                        // to face is zero.
                        // Note: if the nearest point is outside of this face,the result will null,
                        // We skip these special points here.
                        IntersectionResult intResult = roofFace.Project(ept);
                        if (intResult == null)
                        {
                            continue;
                        }
                        //
                        // The project distance should be zero if point is on face
                        if (Math.Abs(intResult.Distance) > DOUBLE_EPS)
                        {
                            return false;
                        }
                     }
                }
            }
            return true;
        }

        /// <summary>
        /// Judge whether two unbound faces are coincident or not. 
        /// </summary>
        /// <param name="roofFace">Roof face geometry.</param>
        /// <param name="roomFace">Room face geometry.</param>
        /// <returns>Return true if two faces are coincident.</returns>
        static private bool AreFacesCoincident(Face roofFace, Face roomFace)
        {
            // Check the detailed face separately
            if (roofFace is PlanarFace)
            {
                return PlanarFacesCoincident(roofFace as PlanarFace, roomFace as PlanarFace);
            }
            else if (roofFace is ConicalFace)
            {
                return ConicalFacesCoincident(roofFace as ConicalFace, roomFace as ConicalFace);
            }
            else if (roofFace is CylindricalFace)
            {
                return CylindricalFacesCoincident(
                    roofFace as CylindricalFace, roomFace as CylindricalFace);
            }
            else if (roofFace is RevolvedFace)
            {
                return RevolvedFacesCoincident(roofFace as RevolvedFace, roomFace as RevolvedFace);
            }
            else if (roofFace is HermiteFace)
            {
                return HermiteFacesCoincident(roofFace as HermiteFace, roomFace as HermiteFace);
            }
            else if (roofFace is RuledFace)
            {
                return RuledFacesCoincident(roofFace as RuledFace, roomFace as RuledFace);
            }
            else
            {
                throw new Exception("Not supported face type: " + roofFace.GetType().Name);
            }
        }

        /// <summary>
        /// Judge whether two unbound ruled face are coincident or not.
        /// </summary>
        /// <param name="f1">1st Ruled face element.</param>
        /// <param name="f2">2nd Ruled face element.</param>
        /// <returns>Return true if they are coincident, else false.</returns>
        private static bool RuledFacesCoincident(RuledFace f1, RuledFace f2)
        {
            Curve f1c1 = f1.get_Curve(0);
            Curve f1c2 = f1.get_Curve(1);
            XYZ f1p1 = f1.get_Point(0);
            XYZ f1p2 = f1.get_Point(1);

            Curve f2c1 = f2.get_Curve(0);
            Curve f2c2 = f2.get_Curve(1);
            XYZ f2p1 = f2.get_Point(0);
            XYZ f2p2 = f2.get_Point(1);

            if (f1p1.AlmostEqual(f2p1) &&
                f1p2.AlmostEqual(f2p2) && 
                AreCurvesCoincident(f1c1, f2c1) &&
                AreCurvesCoincident(f1c2, f2c2))
            {
                Trace.WriteLine("[Ruled face coincident found]");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Judge whether two curves are coincident or not.
        /// </summary>
        /// <param name="c1">1st Curve.</param>
        /// <param name="c2">2nd Curve.</param>
        /// <returns>Return true if they are coincident, else false.</returns>
        private static bool AreCurvesCoincident(Curve c1, Curve c2)
        {
            // if both curves are empty, regard them equal
            // Sometimes we don't check method parameters before calling this method
            if (c1 == null && c2 == null)
            {
                return true;
            }
            // 
            // if one of them is empty and the other is not empty, return false
            if (c1 == null || c2 == null)
            {
                return false;
            }
            //
            // the two curves must be same type
            if (c1.GetType() != c2.GetType())
            {
                return false;
            }
            //
            // get edges of curve and check if they are equal 
            XYZArray curve1 = c1.Tessellate();
            XYZArray curve2 = c2.Tessellate();
            if (curve1.Size != curve2.Size)
            {
                return false;
            }
            // 
            // check if two curves are equal, there may be two situations as below:
            // a: points of two edges are equal in same direction(both are from start to end)
            // b: points of two edges are equal in opposite direction(one is from start to end,
            // the other is opposite)
            bool bEqualInSameDir = true; 
            bool bEqualInOppDir = true; 
            //
            // firstly, check if the edges are equal in same direction
            int count = curve1.Size;
            for (int i = 0; i < count; i++)
            {
                if (!curve1.get_Item(i).AlmostEqual(curve2.get_Item(i)))
                {
                    bEqualInSameDir = false;
                    break;
                }
            }
            //
            // secondly, check if they are equal in opposite direction 
            // if they're not equal in same direction
            if (false == bEqualInSameDir)
            {
                for (int i = 0; i < count; i++)
                {
                    if (!curve1.get_Item(i).AlmostEqual(curve2.get_Item(count - i - 1)))
                    {
                        bEqualInOppDir = false;
                        break;
                    }
                }
            }
            // 
            // we regard the edges as equal if they are equal in anyone direction
            return bEqualInSameDir || bEqualInOppDir;
        }

        /// <summary>
        /// Judge whether two unbound Hermite faces are coincident or not.
        /// </summary>
        /// <param name="f1">1st HermiteFace element.</param>
        /// <param name="f2">2nd HermiteFace element.</param>
        /// <returns>Return true if they are coincident, else false.</returns>
        private static bool HermiteFacesCoincident(HermiteFace f1, HermiteFace f2)
        {
            // points size and MixedDerivs size should be equal firstly
            if (f1.Points.Size != f2.Points.Size ||
                f2.MixedDerivs.Size != f2.MixedDerivs.Size)
            {
                return false;
            }
            // 
            // check the points and MixedDerivs 
            int pointCount = f1.Points.Size;
            for (int i = 0; i < pointCount; i++)
            {
                if (!f1.Points.get_Item(i).AlmostEqual(f2.Points.get_Item(i)))
                {
                    return false;
                }
            }
            int deviCount = f1.MixedDerivs.Size;
            for (int i = 0; i < deviCount; i++)
            {
                if (!f1.MixedDerivs.get_Item(i).AlmostEqual(f2.MixedDerivs.get_Item(i)))
                {
                    return false;
                }
            }

            Trace.WriteLine("[Hermite faces coincident found]");
            return true;
        }

        /// <summary>
        /// Judge whether two unbound revolved faces are coincident or not.
        /// </summary>
        /// <param name="f1">1st RevolvedFace element.</param>
        /// <param name="f2">2nd RevolvedFace element.</param>
        /// <returns>Return true if they are coincident, else false.</returns>
        private static bool RevolvedFacesCoincident(RevolvedFace f1, RevolvedFace f2)
        {
            // the axis and origin should be equal
            if (!f1.Axis.AlmostEqual(f2.Axis) || 
                !f1.Origin.AlmostEqual(f2.Origin)) 
            {
                return false;
            }
            //
            // check the two curves now
            if (!AreCurvesCoincident(f1.Curve, f2.Curve))
            {
                return false;
            }

            Trace.WriteLine("[Revolved face coincident found]");
            return true;
        }

        /// <summary>
        /// Judge whether two unbound cylindrical faces are coincident or not.
        /// </summary>
        /// <param name="f1">1st CylindricalFace element.</param>
        /// <param name="f2">2nd CylindricalFace element.</param>
        /// <returns>Return true if they are coincident, else false.</returns>
        private static bool CylindricalFacesCoincident(CylindricalFace f1, CylindricalFace f2)
        {
            if (f1.Axis.AlmostEqual(f2.Axis) &&
                f1.Origin.AlmostEqual(f2.Origin) &&
                f1.get_Radius(0).AlmostEqual(f2.get_Radius(0)) &&
                f1.get_Radius(1).AlmostEqual(f2.get_Radius(1)))
            {
                Trace.WriteLine("[Cylindrical faces coincident found]");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Judge whether two unbound conical faces are coincident or not.
        /// </summary>
        /// <param name="f1">1st ConicalFace element.</param>
        /// <param name="f2">2nd ConicalFace element.</param>
        /// <returns>Return true if they are coincident, else false.</returns>
        private static bool ConicalFacesCoincident(ConicalFace f1, ConicalFace f2)
        {
            if (f1.Axis.AlmostEqual(f2.Axis) &&
                f1.Origin.AlmostEqual(f2.Origin) &&
                f1.get_Radius(0).AlmostEqual(f2.get_Radius(0)) &&
                f1.get_Radius(1).AlmostEqual(f2.get_Radius(1)) && 
                (Math.Abs(f1.HalfAngle - f2.HalfAngle) < DOUBLE_EPS))
            {
                Trace.WriteLine("[Conical faces coincident found]");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Judge whether two unbound planar faces are coincident or not.
        /// </summary>
        /// <param name="f1">1st PlanarFace element.</param>
        /// <param name="f2">2nd PlanarFace element.</param>
        /// <returns>Return true if they are coincident, else false.</returns>
        static private bool PlanarFacesCoincident(PlanarFace f1, PlanarFace f2)
        {
            // if both origin and normal are equal, the two PlanarFace are equal
            if (f1.Origin.AlmostEqual(f2.Origin) &&
                f1.Normal.AlmostEqual(f2.Normal))
            {
                Trace.WriteLine("[Planar faces coincident found]");
                return true;
            }
            //
            // if the normals are equal in opposite direction, 
            // we should check the project distance as well
            else if (f1.Normal.AlmostEqual(f2.Normal) || f1.Normal.AlmostEqual(-f2.Normal))
            {
                foreach (EdgeArray edges in f2.EdgeLoops)
                {
                    foreach (Edge edge in edges)
                    {
                        XYZ f2Pt = edge.Evaluate(0.0);
                        IntersectionResult intResult = f1.Project(f2Pt);
                        //
                        // Note: if the nearest point is outside of this face,the result will null,
                        // We skip these special points here
                        if (intResult == null)
                        {
                            continue;
                        }
                        // 
                        // if project distance is zero, the two planar faces are equal
                        if (Math.Abs(intResult.Distance) < DOUBLE_EPS)
                        {
                            Trace.WriteLine("[Planar faces coincident found]");
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
