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
using System.Collections;
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.CurvedBeam.CS
{
    /// <summary>
    /// This class inherits from IExternalCommand interface, and implements the Execute method to create Arc, BSpline beams.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class Command : IExternalCommand
    {
        #region Class memeber variables
        Autodesk.Revit.UI.UIApplication m_revit = null;
        ArrayList m_beamMaps = new ArrayList();    // list of beams' type
        ArrayList m_levels = new ArrayList();    // list of levels
        #endregion


        #region Command class properties
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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            m_revit = commandData.Application;
            Transaction tran = new Transaction(m_revit.ActiveUIDocument.Document, "CurvedBeam");
            tran.Start();

            // if initialize failed return Result.Failed
            bool initializeOK = Initialize();
            if (!initializeOK)
            {
                return Autodesk.Revit.UI.Result.Failed;
            }

            // pop up new beam form
            CurvedBeamForm displayForm = new CurvedBeamForm(this);
            displayForm.ShowDialog();
            tran.Commit();

            return Autodesk.Revit.UI.Result.Succeeded;
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
                ElementClassFilter levelFilter = new ElementClassFilter(typeof(Level));
                ElementClassFilter famFilter = new ElementClassFilter(typeof(Family));
                LogicalOrFilter orFilter = new LogicalOrFilter(levelFilter, famFilter);
                FilteredElementCollector collector = new FilteredElementCollector(m_revit.ActiveUIDocument.Document);
                FilteredElementIterator i = collector.WherePasses(orFilter).GetElementIterator();
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

                    foreach (ElementId elementId in f.GetFamilySymbolIds())
                    {
                       object symbol = m_revit.ActiveUIDocument.Document.GetElement(elementId);
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return true;
        }

        /// <summary>
        /// create an horizontal arc instance with specified z coordinate value
        /// </summary>
        public Arc CreateArc(double z)
        {
            Autodesk.Revit.DB.XYZ center = new Autodesk.Revit.DB.XYZ(0, 0, z);
            double radius = 20.0;
            double startAngle = 0.0;
            double endAngle = 5.0;
            Autodesk.Revit.DB.XYZ xAxis = new Autodesk.Revit.DB.XYZ(1, 0, 0);
            Autodesk.Revit.DB.XYZ yAxis = new Autodesk.Revit.DB.XYZ(0, 1, 0);
            return Arc.Create(center, radius, startAngle, endAngle, xAxis, yAxis);
        }


        /// <summary>
        /// create a horizontal partial ellipse instance with specified z coordinate value
        /// </summary>
        public Ellipse CreateEllipse(double z)
        {
            Autodesk.Revit.DB.XYZ center = new Autodesk.Revit.DB.XYZ(0, 0, z);
            double radX = 30;
            double radY = 50;
            Autodesk.Revit.DB.XYZ xVec = new Autodesk.Revit.DB.XYZ(1, 0, 0);
            Autodesk.Revit.DB.XYZ yVec = new Autodesk.Revit.DB.XYZ(0, 1, 0);
            double param0 = 0.0;
            double param1 = 3.1415;
            Ellipse ellpise = Ellipse.Create(center, radX, radY, xVec, yVec, param0, param1);
            m_revit.ActiveUIDocument.Document.Regenerate();
            return ellpise;
        }


        /// <summary>
        /// create a horizontal nurbspline instance with specified z coordinate value
        /// </summary>
        public NurbSpline CreateNurbSpline(double z)
        {
            // create control points with same z value
            List<XYZ> ctrPoints = new List<XYZ>();
            Autodesk.Revit.DB.XYZ xyz1 = new Autodesk.Revit.DB.XYZ(-41.887503610431267, -9.0290629129782189, z);
            Autodesk.Revit.DB.XYZ xyz2 = new Autodesk.Revit.DB.XYZ(-9.27600019217055, 0.32213521486563046, z);
            Autodesk.Revit.DB.XYZ xyz3 = new Autodesk.Revit.DB.XYZ(9.27600019217055, 0.32213521486563046, z);
            Autodesk.Revit.DB.XYZ xyz4 = new Autodesk.Revit.DB.XYZ(41.887503610431267, 9.0290629129782189, z);

            ctrPoints.Add(xyz1); ctrPoints.Add(xyz2); ctrPoints.Add(xyz3);
            ctrPoints.Add(xyz4);

            IList<double> weights = new List<double>();
            double w1 = 1, w2 = 1, w3 = 1, w4 = 1;
            weights.Add(w1); weights.Add(w2); weights.Add(w3);
            weights.Add(w4);

            IList<double> knots = new List<double>();
            double k0 = 0, k1 = 0, k2 = 0, k3 = 0, k4 = 34.425128, k5 = 34.425128, k6 = 34.425128, k7 = 34.425128;

            knots.Add(k0); knots.Add(k1); knots.Add(k2); knots.Add(k3);
            knots.Add(k4); knots.Add(k5); knots.Add(k6);
            knots.Add(k7);

            NurbSpline detailNurbSpline = NurbSpline.Create(ctrPoints, weights, knots, 3, false, true);
            m_revit.ActiveUIDocument.Document.Regenerate();

            return detailNurbSpline;
        }


        /// <summary>
        /// create a curved beam
        /// </summary>
        /// <param name="fsBeam">beam type</param>
        /// <param name="curve">Curve of this beam.</param>
        /// <param name="level">beam's reference level</param>
        /// <returns></returns>
        public bool CreateCurvedBeam(FamilySymbol fsBeam, Curve curve, Level level)
        {
            FamilyInstance beam;
            try
            {
               if (!fsBeam.IsActive)
                  fsBeam.Activate();
                beam = m_revit.ActiveUIDocument.Document.Create.NewFamilyInstance(curve, fsBeam, level, StructuralType.Beam);
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
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Revit", ex.ToString());
                return false;
            }

            // regenerate document
            m_revit.ActiveUIDocument.Document.Regenerate();
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
