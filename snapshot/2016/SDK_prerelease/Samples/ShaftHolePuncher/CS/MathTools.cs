//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.ShaftHolePuncher.CS
{
    /// <summary>
    /// Vector4 class used to store vector
    /// and contain method to handle the vector
    /// </summary>
    public class Vector4
    {
        #region Class member variables and properties
        private float    m_x; 
        private float    m_y; 
        private float    m_z; 
        private float    m_w = 1.0f;

        /// <summary>
        /// X property to get/set x value of Vector4
        /// </summary>
        public float X 
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
        public float Y
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
        public float Z
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
        public float W
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
        public Vector4(float x, float y, float z)
        {
            this.X = x; this.Y = y; this.Z = z;
        }

        /// <summary>
        /// constructor, transfer Autodesk.Revit.DB.XYZ to vector
        /// </summary>
        /// <param name="v">Autodesk.Revit.DB.XYZ structure which needs to be transferred</param>
        public Vector4(Autodesk.Revit.DB.XYZ v)
        {
            this.X = (float)v.X; this.Y = (float)v.Y; this.Z = (float)v.Z;
        }

        /// <summary>
        /// adds two vectors
        /// </summary>
        /// <param name="va">first vector</param>
        /// <param name="vb">second vector</param>
        public static Vector4 operator+ (Vector4 va, Vector4 vb)
        {
            return new Vector4(va.X + vb.X, va.Y + vb.Y, va.Z + vb.Z);
        }

        /// <summary>
        /// subtracts two vectors
        /// </summary>
        /// <param name="va">first vector</param>
        /// <param name="vb">second vector</param>
        /// <returns>subtraction of two vectors</returns>
        public static Vector4 operator- (Vector4 va, Vector4 vb)
        {
            return new Vector4(va.X - vb.X, va.Y - vb.Y, va.Z - vb.Z);
        }

        /// <summary>
        /// multiplies a vector by a floating type value
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="factor">multiplier of floating type</param>
        /// <returns> the result vector </returns>
        public static Vector4 operator* (Vector4 v,float factor)
        {
            return new Vector4(v.X * factor, v.Y * factor, v.Z * factor);
        }

        /// <summary>
        /// divides vector by an floating type value
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="factor">floating type value</param>
        /// <returns> vector divided by a floating type value </returns>
        public static Vector4 operator /(Vector4 v, float factor)
        {
            return new Vector4(v.X / factor, v.Y / factor, v.Z / factor);
        }

        /// <summary>
        /// dot multiply vector
        /// </summary>
        /// <param name="v"> the result vector </param>
        public float DotProduct(Vector4 v)
        {
            return (this.X * v.X + this.Y * v.Y + this.Z * v.Z);
        }

        /// <summary>
        /// get normal vector of two vectors
        /// </summary>
        /// <param name="v">second vector</param>
        /// <returns> normal vector of two vectors</returns>
        public Vector4 CrossProduct(Vector4 v)
        {
            return new Vector4(this.Y * v.Z - this.Z * v.Y,this.Z * v.X
                - this.X * v.Z,this.X * v.Y - this.Y * v.X);
        }

        /// <summary>
        /// dot multiply two vectors
        /// </summary>
        /// <param name="va">first vector</param>
        /// <param name="vb">second vector</param>
        public static float DotProduct(Vector4 va, Vector4 vb)
        {
            return (va.X * vb.X + va.Y * vb.Y + va.Z * vb.Z);
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

        /// <summary>
        /// get unit vector
        /// </summary>
        public void Normalize()
        {
            float length = Length();
            if(length == 0)
            {
                length = 1;
            }
            this.X /= length;
            this.Y /= length;
            this.Z /= length;
        }

        /// <summary>
        /// calculate the length of vector
        /// </summary>
        public float Length()
        {
            return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }
    };

    /// <summary>
    /// Matrix used to transform between ucs coordinate and world coordinate.
    /// </summary>
    public class Matrix4
    {
        #region MatrixType
        /// <summary>
        /// Matrix Type Enum use to define function of matrix
        /// </summary>
        public enum MatrixType
        {
            /// <summary>
            /// matrix use to rotate
            /// </summary>
            Rotation,

            /// <summary>
            /// matrix used to Translation 
            /// </summary>
            Translation,  

            /// <summary>
            /// matrix used to Scale 
            /// </summary>
            Scale,  

            /// <summary>
            /// matrix used to Rotation and Translation 
            /// </summary>
            RotationAndTranslation,

            /// <summary>
            /// normal matrix
            /// </summary>
            Normal 
        };
        private float[,] m_matrix = new float[4,4];
        private MatrixType m_type;
        #endregion

        /// <summary>
        /// default ctor
        /// </summary>
        public Matrix4()
        {
            m_type = MatrixType.Normal;
            Identity();            
        }

        /// <summary>
        /// ctor,rotation matrix,origin at (0,0,0)
        /// </summary>
        /// <param name="xAxis">identity of x axis</param>
        /// <param name="yAxis">identity of y axis</param>
        /// <param name="zAxis">identity of z axis</param>
        public Matrix4(Vector4 xAxis,Vector4 yAxis, Vector4 zAxis)
        {
            m_type = MatrixType.Rotation;
            Identity();
            m_matrix[0, 0] = xAxis.X; m_matrix[0, 1] = xAxis.Y; m_matrix[0, 2] = xAxis.Z;
            m_matrix[1, 0] = yAxis.X; m_matrix[1, 1] = yAxis.Y; m_matrix[1, 2] = yAxis.Z;
            m_matrix[2, 0] = zAxis.X; m_matrix[2, 1] = zAxis.Y; m_matrix[2, 2] = zAxis.Z;
            
        }

        /// <summary>
        /// ctor,translation matrix.
        /// </summary>
        /// <param name="origin">origin of ucs in world coordinate</param>
        public Matrix4(Vector4 origin)
        {
            m_type = MatrixType.Translation;
            Identity();
            m_matrix[3, 0] = origin.X; m_matrix[3, 1] = origin.Y; m_matrix[3, 2] = origin.Z;
        }

        /// <summary>
        /// rotation and translation matrix constructor
        /// </summary>
        /// <param name="xAxis">x Axis</param>
        /// <param name="yAxis">y Axis</param>
        /// <param name="zAxis">z Axis</param>
        /// <param name="origin">origin</param>
        public Matrix4(Vector4 xAxis, Vector4 yAxis, Vector4 zAxis, Vector4 origin)
        {
            m_type = MatrixType.RotationAndTranslation;
            Identity();
            m_matrix[0, 0] = xAxis.X;  m_matrix[0, 1] = xAxis.Y;  m_matrix[0, 2] = xAxis.Z;
            m_matrix[1, 0] = yAxis.X;  m_matrix[1, 1] = yAxis.Y;  m_matrix[1, 2] = yAxis.Z;
            m_matrix[2, 0] = zAxis.X;  m_matrix[2, 1] = zAxis.Y;  m_matrix[2, 2] = zAxis.Z;
            m_matrix[3, 0] = origin.X; m_matrix[3, 1] = origin.Y; m_matrix[3, 2] = origin.Z;
        }

        /// <summary>
        /// scale matrix constructor
        /// </summary>
        /// <param name="scale">scale factor</param>
        public Matrix4(float scale)
        {
            m_type = MatrixType.Scale;
            Identity();
            m_matrix[0, 0] = scale;
            m_matrix[1, 1] = scale;
            m_matrix[2, 2] = scale;
        }

        /// <summary>
        /// indexer of matrix
        /// </summary>
        /// <param name="row">row number</param>
        /// <param name="column">column number</param>
        /// <returns></returns>
        public float this[int row, int column]
        {
            get
            {
                return this.m_matrix[row, column];
            }
            set
            {
                this.m_matrix[row, column] = value;
            }
        }

        /// <summary>
        /// Identity matrix
        /// </summary>
        public void Identity()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.m_matrix[i, j] = 0.0f;
                }
            }
            this.m_matrix[0, 0] = 1.0f;
            this.m_matrix[1, 1] = 1.0f;
            this.m_matrix[2, 2] = 1.0f;
            this.m_matrix[3, 3] = 1.0f;
        }

        /// <summary>
        ///  multiply matrix left and right
        /// </summary>
        /// <param name="left">left matrix</param>
        /// <param name="right">right matrix</param>
        /// <returns></returns>
        public static Matrix4 Multiply(Matrix4 left, Matrix4 right)
        {
            Matrix4 result = new Matrix4();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = left[i, 0] * right[0, j] + left[i, 1] * right[1, j]
                        + left[i, 2] * right[2, j] + left[i, 3] * right[3, j];
                }
            }
            return result;
        }

        /// <summary>
        /// transform point using this matrix
        /// </summary>
        /// <param name="point">point to be transformed</param>
        /// <returns>transform result</returns>
        public Vector4 Transform(Vector4  point)
        {
            return new Vector4(point.X * this[0, 0] + point.Y * this[1, 0]
                + point.Z * this[2, 0]+ point.W * this[3, 0],
                point.X * this[0, 1] + point.Y * this[1, 1]
                + point.Z * this[2, 1]+ point.W * this[3, 1],
                point.X * this[0, 2] + point.Y * this[1, 2]
                + point.Z * this[2, 2]+ point.W * this[3, 2]);
        }

        /// <summary>
        /// if m_matrix is a rotation matrix,this method can get the rotation inverse matrix.
        /// </summary>
        /// <returns>rotation inverse matrix</returns>
        public Matrix4 RotationInverse()
        {
            return new Matrix4(new Vector4(this[0, 0], this[1, 0], this[2, 0]), 
                new Vector4(this[0, 1], this[1, 1], this[2, 1]), 
                new Vector4(this[0, 2], this[1, 2], this[2, 2]));
        }

        /// <summary>
        /// if this m_matrix is a translation matrix,
        /// this method can get the translation inverse matrix.
        /// </summary>
        /// <returns>translation inverse matrix</returns>
        public Matrix4 TranslationInverse()
        {
            return new Matrix4(new Vector4(-this[3, 0], -this[3, 1], -this[3, 2]));
        } 
       
        /// <summary>
        /// get inverse matrix
        /// </summary>
        /// <returns>inverse matrix</returns>
        public Matrix4 Inverse()
        {
            switch(m_type)
            {
                case MatrixType.Rotation:
                    return RotationInverse();

                case MatrixType.Translation:
                    return TranslationInverse();

                case MatrixType.RotationAndTranslation:
                    return Multiply(TranslationInverse(),RotationInverse());
                
                case MatrixType.Scale:
                    return ScaleInverse();
               
                case MatrixType.Normal:
                    return new Matrix4();

                default: return null;
            }
        }

        /// <summary>
        /// if  m_matrix is a scale matrix,this method can get the scale inverse matrix.
        /// </summary>
        /// <returns>scale inverse matrix</returns>
        public Matrix4 ScaleInverse()
        {
            return new Matrix4(1 / m_matrix[0,0]);
        }
    };
}
