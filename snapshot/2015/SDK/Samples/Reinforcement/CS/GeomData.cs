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

namespace Revit.SDK.Samples.Reinforcement.CS
{
    #region Struct Definition

    /// <summary>
    /// a struct to store the geometry information of the rebar
    /// </summary>
    public struct RebarGeometry
    {
        // Private members
        Autodesk.Revit.DB.XYZ m_normal;        // the direction of rebar distribution
        IList<Curve> m_curves; //the profile of the rebar
        int m_number;        //the number of the rebar
        double m_spacing;    //the spacing of the rebar

        /// <summary>
        /// get and set the value of the normal
        /// </summary>
        public Autodesk.Revit.DB.XYZ Normal
        {
            get
            {
                return m_normal;
            }
            set
            {
                m_normal = value;
            }
        }

        /// <summary>
        /// get and set the value of curve array
        /// </summary>
        public IList<Curve> Curves
        {
            get
            {
                return m_curves;
            }
            set
            {
                m_curves = value;
            }
        }

        /// <summary>
        /// get and set the number of the rebar
        /// </summary>
        public int RebarNumber
        {
            get
            {
                return m_number;
            }
            set
            {
                m_number = value;
            }
        }

        /// <summary>
        /// get and set the value of the rebar spacing 
        /// </summary>
        public double RebarSpacing
        {
            get
            {
                return m_spacing;
            }
            set
            {
                m_spacing = value;
            }
        }

        /// <summary>
        /// consturctor
        /// </summary>
        /// <param name="normal">the normal information</param>
        /// <param name="curves">the profile of the rebars</param>
        /// <param name="number">the number of the rebar</param>
        /// <param name="spacing">the number of the rebar</param>
        public RebarGeometry(Autodesk.Revit.DB.XYZ normal, IList<Curve> curves, int number, double spacing)
        {
            // initialize the data members
            m_normal = normal;
            m_curves = curves;
            m_number = number;
            m_spacing = spacing;
        }
    }

    /// <summary>
    /// A struct to store the const data which support beam reinforcement creation
    /// </summary>
    public struct BeamRebarData
    {
        /// <summary>
        /// offset value of the top end rebar
        /// </summary>
        public const double TopEndOffset = 0.2;

        /// <summary>
        ///offset value of the top center rebar 
        /// </summary>
        public const double TopCenterOffset = 0.23;

        /// <summary>
        /// offset value of the transverse rebar
        /// </summary>
        public const double TransverseOffset = 0.125;

        /// <summary>
        /// offset value of the end transverse rebar
        /// </summary>
        public const double TransverseEndOffset = 1.2;

        /// <summary>
        /// the spacing value between end and center transvers rebar
        /// </summary>
        public const double TransverseSpaceBetween = 1;

        /// <summary>
        ///offset value of bottom rebar 
        /// </summary>
        public const double BottomOffset = 0.271;

        /// <summary>
        /// number of bottom rebar
        /// </summary>
        public const int BottomRebarNumber = 5;

        /// <summary>
        /// number of top rebar
        /// </summary>
        public const int TopRebarNumber = 2;
    }


    /// <summary>
    /// A struct to store the const data which support column reinforcement creation
    /// </summary>
    public struct ColumnRebarData
    {
        /// <summary>
        /// offset value of transverse rebar
        /// </summary>
        public const double TransverseOffset = 0.125;
 
        /// <summary>
        /// offset value of vertical rebar
        /// </summary>
        public const double VerticalOffset = 0.234;
    }

    #endregion

    #region Enum Definition
    /// <summary>
    /// Indicate location of top rebar
    /// </summary>
    public enum TopRebarLocation
    {
        /// <summary>
        /// locate start
        /// </summary>
        Start,

        /// <summary>
        /// locate center
        /// </summary>
        Center,

        /// <summary>
        /// locate end
        /// </summary>
        End
    }

    /// <summary>
    /// Indicate location of transverse rebar
    /// </summary>
    public enum TransverseRebarLocation
    {
        /// <summary>
        /// locate start
        /// </summary>
        Start,

        /// <summary>
        /// locate center
        /// </summary>
        Center,

        /// <summary>
        /// locate end
        /// </summary>
        End
    }

    /// <summary>
    /// Indicate location of vertical rebar
    /// </summary>
    public enum VerticalRebarLocation
    {
        /// <summary>
        /// locate north
        /// </summary>
        North,

        /// <summary>
        /// locate east
        /// </summary>
        East,

        /// <summary>
        /// locate south
        /// </summary>
        South,

        /// <summary>
        /// locate west
        /// </summary>
        West
    }
    #endregion

    /// <summary>
    /// A comparer for XYZ, and give a method to sort all the Autodesk.Revit.DB.XYZ points in a array
    /// </summary>
    public class XYZHeightComparer : IComparer<Autodesk.Revit.DB.XYZ>
    {
        int IComparer<Autodesk.Revit.DB.XYZ>.Compare(Autodesk.Revit.DB.XYZ first, Autodesk.Revit.DB.XYZ second)
        {
            // first compare z coordinate, then y coordinate, at last x coordinate
            if (GeomUtil.IsEqual(first.Z, second.Z))
            {
                if (GeomUtil.IsEqual(first.Y, second.Y))
                {
                    if (GeomUtil.IsEqual(first.X, second.X))
                    {
                        return 0;
                    }
                    return (first.X > second.X) ? 1 : -1;
                }
                return (first.Y > second.Y) ? 1 : -1;
            }
            return (first.Z > second.Z) ? 1 : -1;
        }
    }
}
