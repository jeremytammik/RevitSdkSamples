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
using System.Collections;
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Structural.Enums;

namespace Revit.SDK.Samples.CurvedBeam.CS
{
    /// <summary>
    /// This class inherits from IExternalCommand interface, and implements the Execute method to create Arc, BSpline beams.
    /// </summary>
    public class Command : IExternalCommand
    {
        #region Class memeber variables
        Autodesk.Revit.Application m_revit = null;
        ArrayList m_beamMaps = new ArrayList();	// list of beams' type
        ArrayList m_levels = new ArrayList();	// list of levels

        Arc m_arc = null; // Arc curve data for new beam
        Ellipse m_partialEllipse = null; // partia ellipse curve date for new beam
        NurbSpline m_nurbSpline = null; // NurbSpline curve data for new beam
        #endregion


        #region Command class properties
        /// <summary>
        /// get arc
        /// </summary>
        public Arc Arc
        {
            get
            {
                return m_arc;
            }
        }

        /// <summary>
        /// get partial ellipse
        /// </summary>
        public Ellipse PartialEllipse
        {
            get
            {
                return m_partialEllipse;
            }
        }

        /// <summary>
        /// get nurb spline
        /// </summary>
        public NurbSpline NurbSpline
        {
            get
            {
                return m_nurbSpline;
            }
        }

        /// <summary>
        /// list of all type of beams
        /// </summary>
        public ArrayList BeamMaps
        {
            get
            {
                return m_beamMaps;
            }
        }

        /// <summary>
        /// list of all levels
        /// </summary>
        public ArrayList LevelMaps
        {
            get
            {
                return m_levels;
            }
        }
        #endregion


        #region IExternalCommand interface implementation
        ///<summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            m_revit = commandData.Application;

            // if initialize failed return Result.Failed
            bool initializeOK = Initialize();
            if (!initializeOK)
            {
                return Autodesk.Revit.IExternalCommand.Result.Failed;
            }

            // pop up new beam form
            CurvedBeamForm displayForm = new CurvedBeamForm(this);
            displayForm.ShowDialog();

            return IExternalCommand.Result.Succeeded;
        }
        #endregion


        /// <summary>
        /// iterate all the symbols of levels and beams
        /// </summary>
        /// <returns>A value that signifies if the initialization was successful for true or failed for false</returns>
        private bool Initialize()
        {
            try
            {
                ElementIterator i = m_revit.ActiveDocument.Elements;
                i.Reset();
                bool moreElement = i.MoveNext();
                while (moreElement)
                {
                    object o = i.Current;

                    // add level to list
                    Level level = o as Level;
                    if (null != level)
                    {
                        m_levels.Add(new LevelMap(level));
                        goto nextLoop;
                    }

                    // get
                    Family f = o as Family;
                    if (null == f)
                    {
                        goto nextLoop;
                    }

                    foreach (object symbol in f.Symbols)
                    {
                        FamilySymbol familyType = symbol as FamilySymbol;
                        if (null == familyType)
                        {
                            goto nextLoop;
                        }
                        if (null == familyType.Category)
                        {
                            goto nextLoop;
                        }

                        // add symbols of beams and braces to lists 
                        string categoryName = familyType.Category.Name;
                        if ("Structural Framing" == categoryName)
                        {
                            m_beamMaps.Add(new SymbolMap(familyType));
                        }
                    }
                nextLoop:
                    moreElement = i.MoveNext();
                }

                // create arc, ellpise and nurbspline data for new beam
                m_arc = CreateArc();
                m_partialEllipse = CreateEllipse();
                m_nurbSpline = CreateNurbSpline();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return true;
        }


        /// <summary>
        /// create an arc instance
        /// </summary>
        Arc CreateArc()
        {
            XYZ center = new XYZ(0, 0, 0);
            double radius = 20.0;
            double startAngle = 0.0;
            double endAngle = 5.0;
            XYZ xAxis = new XYZ(1, 0, 0);
            XYZ yAxis = new XYZ(0, 1, 0);
            return m_revit.Create.NewArc(ref center, radius, startAngle, endAngle, ref xAxis, ref yAxis);
        }


        /// <summary>
        /// create a partial ellipse instance
        /// </summary>
        Ellipse CreateEllipse()
        {
            XYZ center = new XYZ(0, 0, 0);
            double radX = 30;
            double radY = 50;
            XYZ xVec = new XYZ(1, 0, 0);
            XYZ yVec = new XYZ(0, 1, 0);
            double param0 = 0.0;
            double param1 = 3.1415;
            return m_revit.Create.NewEllipse(ref center, radX, radY, ref xVec, ref yVec, param0, param1);
        }


        /// <summary>
        /// create a nurbspline instance 
        /// </summary>
        NurbSpline CreateNurbSpline()
        {
            XYZArray ctrPoints = m_revit.Create.NewXYZArray();
            XYZ xyz1 = new XYZ(-41.887503610431267, -9.0290629129782189, 0);
            XYZ xyz2 = new XYZ(-9.27600019217055, 0.32213521486563046, 0);
            XYZ xyz3 = new XYZ(9.27600019217055, 0.32213521486563046, 0);
            XYZ xyz4 = new XYZ(41.887503610431267, 9.0290629129782189, 0);

            ctrPoints.Append(ref xyz1); ctrPoints.Append(ref xyz2); ctrPoints.Append(ref xyz3);
            ctrPoints.Append(ref xyz4);

            DoubleArray weights = new DoubleArray();
            double w1 = 1, w2 = 1, w3 = 1, w4 = 1;
            weights.Append(ref w1); weights.Append(ref w2); weights.Append(ref w3);
            weights.Append(ref w4);

            DoubleArray knots = new DoubleArray();
            double k0 = 0, k1 = 0, k2 = 0, k3 = 0, k4 = 34.425128, k5 = 34.425128, k6 = 34.425128, k7 = 34.425128;

            knots.Append(ref k0); knots.Append(ref k1); knots.Append(ref k2); knots.Append(ref k3);
            knots.Append(ref k4); knots.Append(ref k5); knots.Append(ref k6);
            knots.Append(ref k7);

            NurbSpline detailNurbSpline = m_revit.Create.NewNurbSpline(ctrPoints, weights, knots, 3, false, true);

            return detailNurbSpline;
        }


        /// <summary>
        /// create a curved beam
        /// </summary>
        public bool CreateCurvedBeam(FamilySymbol fsBeam, Curve curve, Level level, XYZ position)
        {
            FamilyInstance beam;
            try
            {
                XYZ center = new XYZ(0, 0, 0);
                beam = m_revit.ActiveDocument.Create.NewFamilyInstance(ref center, fsBeam, level, StructuralType.Beam);
                if (null == beam)
                {
                    return false;
                }

                // get beam location curve
                LocationCurve beamCurve = beam.Location as LocationCurve;
                if (null == beamCurve)
                {
                    return false;
                }

                // set new curve for beam
                beamCurve.Curve = curve;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Revit");
                return false;
            }

            // move beam to new position
            m_revit.ActiveDocument.Move(beam, ref position);
            return true;
        }
    }


    /// <summary>
    /// assistant class contains symbol and it's name
    /// </summary>
    public class SymbolMap
    {
        #region SymbolMap class member variables
        string m_symbolName = "";
        FamilySymbol m_symbol = null;
        #endregion


        /// <summary>
        /// constructor without parameter is forbidden
        /// </summary>
        private SymbolMap()
        {
            // no operation 
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="symbol">family symbol</param>
        public SymbolMap(FamilySymbol symbol)
        {
            m_symbol = symbol;
            string familyName = "";
            if (null != symbol.Family)
            {
                familyName = symbol.Family.Name;
            }
            m_symbolName = familyName + " : " + symbol.Name;
        }


        /// <summary>
        /// SymbolName property
        /// </summary>
        public string SymbolName
        {
            get
            {
                return m_symbolName;
            }
        }


        /// <summary>
        /// ElementType property
        /// </summary>
        public FamilySymbol ElementType
        {
            get
            {
                return m_symbol;
            }
        }
    }


    /// <summary>
    /// assistant class contains level and it's name
    /// </summary>
    public class LevelMap
    {
        #region LevelMap class member variable
        string m_levelName = "";
        Level m_level = null;
        #endregion


        #region LevelMap Constructors
        /// <summary>
        /// constructor without parameter is forbidden
        /// </summary>
        private LevelMap()
        {
            // no operation
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="level">level</param>
        public LevelMap(Level level)
        {
            m_level = level;
            m_levelName = level.Name;
        }
        #endregion


        #region LevelMap properties
        /// <summary>
        /// LevelName property
        /// </summary>
        public string LevelName
        {
            get
            {
                return m_levelName;
            }
        }

        /// <summary>
        /// Level property
        /// </summary>
        public Level Level
        {
            get
            {
                return m_level;
            }
        }
        #endregion
    }
}
