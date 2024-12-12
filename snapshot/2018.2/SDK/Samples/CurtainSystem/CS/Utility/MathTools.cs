//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.CurtainSystem.CS.Utility
{
   /// <summary>
   /// Vector4 is a homogeneous coordinate class used to store vector
   /// and contain method to handle the vector
   /// </summary>
   public class Vector4
   {
      #region Class member variables and properties
      private double m_x;
      private double m_y;
      private double m_z;
      private double m_w = 1.0d;

      /// <summary>
      /// X property to get/set x value of Vector4
      /// </summary>
      public double X
      {
         get
         {
            return m_x;
         }
         set
         {
            m_x = value;
         }
      }

      /// <summary>
      /// Y property to get/set y value of Vector4
      /// </summary>
      public double Y
      {
         get
         {
            return m_y;
         }
         set
         {
            m_y = value;
         }
      }

      /// <summary>
      /// Z property to get/set z value of Vector4
      /// </summary>
      public double Z
      {
         get
         {
            return m_z;
         }
         set
         {
            m_z = value;
         }
      }

      /// <summary>
      /// W property to get/set fourth value of Vector4
      /// </summary>
      public double W
      {
         get
         {
            return m_w;
         }
         set
         {
            m_w = value;
         }
      }
      #endregion

      /// <summary>
      /// constructor
      /// </summary>
      public Vector4(double x, double y, double z)
      {
         this.X = x; this.Y = y; this.Z = z;
      }

      /// <summary>
      /// constructor, transfer Autodesk.Revit.DB.XYZ to vector
      /// </summary>
      /// <param name="v">Autodesk.Revit.DB.XYZ structure which needs to be transfered</param>
      public Vector4(Autodesk.Revit.DB.XYZ v)
      {
         this.X = (double)v.X; this.Y = (double)v.Y; this.Z = (double)v.Z;
      }


      /// <summary>
      /// get normal vector of plane contains two vectors
      /// </summary>
      /// <param name="v">second vector</param>
      /// <returns> normal vector of two vectors</returns>
      public Vector4 CrossProduct(Vector4 v)
      {
         return new Vector4(this.Y * v.Z - this.Z * v.Y, this.Z * v.X
             - this.X * v.Z, this.X * v.Y - this.Y * v.X);
      }

      /// <summary>
      /// get normal vector of two vectors
      /// </summary>
      /// <param name="va">first vector</param>
      /// <param name="vb">second vector</param>
      /// <returns> normal vector of two vectors </returns>
      public static Vector4 CrossProduct(Vector4 va, Vector4 vb)
      {
         return new Vector4(va.Y * vb.Z - va.Z * vb.Y, va.Z * vb.X
             - va.X * vb.Z, va.X * vb.Y - va.Y * vb.X);
      }
   };

}
