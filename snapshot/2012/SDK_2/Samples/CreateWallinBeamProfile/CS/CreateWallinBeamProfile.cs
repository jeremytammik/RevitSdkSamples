//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Collections;
using System.Text;
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.CreateWallinBeamProfile.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CreateWallinBeamProfile : IExternalCommand
    {
        // Private Members
        ArrayList m_wallTypeCollection;         // Store all the wall types in current document
        ArrayList m_beamCollection;             // Store the selection of beams in Revit
        ArrayList m_analyticalLineCollection;   // Store the analytical line of all the beams
        WallType m_selectedWallType;            // Store the selected wall type
        Level m_level;                          // Store the level which wall create on
        Boolean m_isStructural;                 // Indicate whether create structural walls
        String m_errorInformation;              // Store the error information
        const Double PRECISION = 0.0000000001;  // Define a precision of double data


        // Properties
        /// <summary>
        /// Inform all the wall types can be created in current document
        /// </summary>
        public ArrayList WallTypes
        {
            get
            {
                return m_wallTypeCollection;
            }
        }

        /// <summary>
        /// Inform the wall type selected by the user
        /// </summary>
        public Object SelectedWallType
        {
            set
            {
                m_selectedWallType = value as WallType;
            }
        }

        /// <summary>
        /// Inform whether the user want to create structual or architecture walls
        /// </summary>
        public Boolean IsSturctual
        {
            get
            {
                return m_isStructural;
            }
            set
            {
                m_isStructural = value;
            }
        }

        // Methods
        /// <summary>
        /// Default constructor of CreateWallinBeamProfile
        /// </summary>
        public CreateWallinBeamProfile()
        {
            m_wallTypeCollection = new ArrayList();
            m_beamCollection = new ArrayList();
            m_analyticalLineCollection = new ArrayList();
            m_isStructural = true;
        }
        #region IExternalCommand Members Implementation
        /// <summary>
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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
                                                    ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Autodesk.Revit.UI.UIApplication revit = commandData.Application;
            UIDocument project = revit.ActiveUIDocument;

            // Get necessary data from revit.such as selected beams and level information
            if (!PrepareData(project))
            {
                message = m_errorInformation;
                return Autodesk.Revit.UI.Result.Failed;
            }

            // Check whether the selected beams can make a a vertical profile
            if (!IsVerticalProfile())
            {
                message = m_errorInformation;
                return Autodesk.Revit.UI.Result.Failed;
            }

            // Show the dialog for the user select the wall style
            using (CreateWallinBeamProfileForm displayForm = new CreateWallinBeamProfileForm(this))
            {
                if (DialogResult.OK != displayForm.ShowDialog())  
                {
                    return Autodesk.Revit.UI.Result.Failed;
                }
            }
            
            // Create the walls using the outline generated by the beams.
            if (!BeginCreate(project.Document))
            {
                message = m_errorInformation;
                return Autodesk.Revit.UI.Result.Failed;
            }

            // If everything goes right, return succeeded.
            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation

        /// <summary>
        /// Create the walls using the outline generated by the beams
        /// </summary>
        /// <param name="project"> A reference of current document</param>
        /// <returns>true if no error happens; otherwise, false.</returns>
        Boolean BeginCreate(Autodesk.Revit.DB.Document project)
        {
            CurveArray curveArray = new CurveArray();   // store the curves used to create wall
            Autodesk.Revit.DB.XYZ point;      // used to store the end point of the curve temporarily
            Curve curve = m_analyticalLineCollection[0] as Curve;
            curveArray.Append(curve);
            point = curve.get_EndPoint(1);

            // Sort the curves of analytical model and then add to curveArray.
            // API asks for the curves should be in a sequence, deasil or anticlockwise
            for (int i = 1; i < m_analyticalLineCollection.Count; i++)
            {
                foreach (Object o in m_analyticalLineCollection)
                {
                    Boolean isInclude = false;
                    foreach (Curve j in curveArray)
                    {
                        if (o.Equals(j))
                        {
                            isInclude = true;
                            break;
                        }
                    }
                    if (true == isInclude)
                    {
                        continue;
                    }

                    curve = o as Curve;
                    if (!EqualPoint(curve.get_EndPoint(0), point)
                        && !EqualPoint(curve.get_EndPoint(1), point))
                    {
                        continue;
                    }
                    else if (EqualPoint(curve.get_EndPoint(0), point))
                    {
                        curveArray.Append(curve);
                        point = curve.get_EndPoint(1);
                        break;
                    }
                    else if (EqualPoint(curve.get_EndPoint(1), point))
                    {
                        curveArray.Append(curve);
                        point = curve.get_EndPoint(0);
                        break;
                    }
                    else
                    {
                        m_errorInformation = "The program should never go here.";
                        return false;
                    }
                }
            }

            // If the program goes here, it means the beams can't form a profile.
            if (curveArray.Size != m_analyticalLineCollection.Count)
            {
                m_errorInformation = "There are more than one closed profile.";
                return false;
            }

            // Begin to create the wall.
            Transaction t = new Transaction(project, Guid.NewGuid().GetHashCode().ToString());
            t.Start();
            Wall createdWall = project.Create.NewWall(curveArray,
                                            m_selectedWallType, m_level, m_isStructural);
            
            if (null == createdWall)
            {
                m_errorInformation = "Can not create the wall";
                return false;
            }

            // Modify some parameters of the created wall to make it look better.
            Double baseOffset = FindBaseOffset();   // get the base offset from m_level
            Double topOffset = FindTopOffset();     // get the top offset from m_level
            Autodesk.Revit.DB.ElementId levelId = m_level.Id;
            // Modify the "Base Constraint", "Base Offset", "Top Constraint" and "Top Offset"
            //properties of the created wall.
            createdWall.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT).Set(levelId);
            createdWall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).Set(baseOffset);
            createdWall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(levelId);
            createdWall.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET).Set(topOffset);
            t.Commit();
            return true;
        }

        /// <summary>
        /// Get necessary data from revit.such as selected beams, wall types and level information
        /// </summary>
        /// <param name="project">A reference of current document</param>
        /// <returns>true if no error happens; otherwise, false.</returns>
        Boolean PrepareData(Autodesk.Revit.UI.UIDocument project)
        {
            // Search all the wall types in the Revit
            foreach (WallType wallType in project.Document.WallTypes)
            {
                if (null == wallType)
                {
                    continue;
                }
                m_wallTypeCollection.Add(wallType);
            }

            // Find the selection of beams in Revit
            ElementSet selection = project.Selection.Elements;
            AnalyticalModel model;     // store the AnalyticalModel of the beam.
            foreach (Autodesk.Revit.DB.Element e in selection)
            {
                FamilyInstance m = e as FamilyInstance;

                // Use StructuralType property can judge whether it is a beam.
                if (null != m && StructuralType.Beam == m.StructuralType)
                {
                    m_beamCollection.Add(e);    // store the beams

                    // Get the curve of beam's AnalyticalModel.
                    model = m.GetAnalyticalModel();
                    if (null == model)
                    {
                        m_errorInformation = "The beam should have analytical model.";
                        return false;
                    }
                    m_analyticalLineCollection.Add(model.GetCurve());
                }
            }
            if (0 == m_beamCollection.Count)
            {
                m_errorInformation = "Can not find any beams.";
                return false;
            }

            // Get the level which will be used in create method
            FilteredElementCollector collector = new FilteredElementCollector(project.Document);
            m_level = collector.OfClass(typeof(Level)).FirstElement() as Level;
            return true;
        }

        /// <summary>
        /// Check whether the selected beams can make a a vertical profile.
        /// </summary>
        /// <returns>true if selected beams create a vertical profile; otherwise, false.</returns>
        Boolean IsVerticalProfile()
        {
            // First check whether all the beams are in a same vertical plane
            if (!IsInVerticalPlane())
            {
                m_errorInformation = "Not all the beam in a vertical plane.";
                return false;
            }

            // Second check whether a closed profile can be created by all the beams
            if (!CanCreateProfile())
            {
                m_errorInformation = "All the beams should create only one profile.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check whether the input two points are the same
        /// </summary>
        /// <param name="first"> The first point</param>
        /// <param name="second"> The second point</param>
        /// <returns>true if two points are the same; otherwise, false</returns>
        Boolean EqualPoint(Autodesk.Revit.DB.XYZ first, Autodesk.Revit.DB.XYZ second)
        {
            if ((-PRECISION <= first.X - second.X && PRECISION >= first.X - second.X)
                && (-PRECISION <= first.Y - second.Y && PRECISION >= first.Y - second.Y)
                && (-PRECISION <= first.Z - second.Z && PRECISION >= first.Z - second.Z))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether the two double data are the same
        /// </summary>
        /// <param name="first">The first double data</param>
        /// <param name="second">The second double data</param>
        /// <returns>true if two double data are the same; otherwise, false</returns>
        Boolean EqualDouble(Double first, Double second)
        {
            if (-PRECISION <= first - second && PRECISION >= first - second)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether all the beams are in a same vertical plane
        /// </summary>
        /// <returns>true if they are in same vertical plane; otherwise, false</returns>
        Boolean IsInVerticalPlane()
        {
            Autodesk.Revit.DB.XYZ startPoint = new Autodesk.Revit.DB.XYZ ();
            Autodesk.Revit.DB.XYZ endPoint = new Autodesk.Revit.DB.XYZ ();
            int sign = 0;               // used as a symbol,
            Double slope = 0;           // record slope of the lines' projection on X-Y plane

            // When all the beams in the X-Z plane or Y-Z plane, the deal is especial
            // So I use 3 ways to judge whether all the beams are in same vertical plane
            Curve curve = m_analyticalLineCollection[0] as Curve;
            startPoint = curve.get_EndPoint(0);
            endPoint = curve.get_EndPoint(1);
            if (EqualDouble(startPoint.X, endPoint.X))
            {
                sign = 1;           // All the beams may be in Y-Z plane
            }
            else if (EqualDouble(startPoint.Y, endPoint.Y))
            {
                sign = 2;           // All the beams may be in X-Z plane
            }
            else
            {
                slope = (startPoint.Y - endPoint.Y) / (startPoint.X - endPoint.X);
            }

            // Begin to compare each analytical line and judge whether they are in same vertical plane
            for (int i = 1; i < m_analyticalLineCollection.Count; i++)
            {
                curve = m_analyticalLineCollection[i] as Curve;
                startPoint = curve.get_EndPoint(0);
                endPoint = curve.get_EndPoint(1);

                switch (sign)
                {
                    case 0:     // Judge whether the slope of beam's projection on X-Y plane are same.
                        Double anotherSlope = (startPoint.Y - endPoint.Y) / (startPoint.X - endPoint.X);
                        if (!EqualDouble(slope, anotherSlope))
                        {
                            return false;
                        }
                        break;
                    case 1:     // Judge whether the beams are in Y-Z plane
                        if (!EqualDouble(startPoint.X, endPoint.X))
                        {
                            return false;
                        }
                        break;
                    case 2:     // Judge whether the beams are in X-Z plane
                        if (!EqualDouble(startPoint.Y, endPoint.Y))
                        {
                            return false;
                        }
                        break;
                    default:    // If it go here, there must be something error.
                        MessageBox.Show("Should not come here.");
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// Check whether a closed profile can be created by all the beams
        /// </summary>
        /// <returns>true if one profile found; otherwise, false</returns>
        Boolean CanCreateProfile()
        {
            // Only allow all the beams compose a close profile.
            // As we all know, a close profile is composed by borders and points,
            // and the number of borders should be equal to points'.
            // So, the judgement use this way. 
            Autodesk.Revit.DB.XYZ startPoint = new Autodesk.Revit.DB.XYZ ();
            Autodesk.Revit.DB.XYZ endPoint = new Autodesk.Revit.DB.XYZ ();
            Curve curve = null;
            ArrayList pointArray = new ArrayList();
            bool hasStartpoint;      // indicate whether start point is in the array
            bool hasEndPoint;        // indicate whether end point is in the array

            // Find out all the points in the curves, the same point only count once.
            for (int i = 0; i < m_analyticalLineCollection.Count; i++)
            {
                curve = m_analyticalLineCollection[i] as Curve;
                startPoint = curve.get_EndPoint(0);
                endPoint = curve.get_EndPoint(1);
                hasStartpoint = false;  // Judge whether start point has been counted.
                hasEndPoint = false;    // Judge whether end point has been counted.

                if (0 == pointArray.Count)
                {
                    pointArray.Add(startPoint);
                    pointArray.Add(endPoint);
                    continue;
                }

                // Judge whether the points of this curve have been counted.
                foreach (Object o in pointArray)
                {
                    Autodesk.Revit.DB.XYZ point = (Autodesk.Revit.DB.XYZ)o;
                    if (EqualPoint(startPoint, point))
                    {
                        hasStartpoint = true;
                    }
                    if (EqualPoint(endPoint, point))
                    {
                        hasEndPoint = true;
                    }
                }

                // If not, add the points into the array.
                if (!hasStartpoint)
                {
                    pointArray.Add(startPoint);
                }
                if (!hasEndPoint)
                {
                    pointArray.Add(endPoint);
                }
            }

            if (pointArray.Count != m_analyticalLineCollection.Count)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Find the offset from the elevation of the lowest point to m_level's elevation 
        /// </summary>
        /// <returns> The length of the offset </returns>
        Double FindBaseOffset()
        {
            // Initialize the data.
            Double baseOffset = 0;  // the offset from the m_level's elevation
            Double lowestElevation = 0; // the elevation of the lowest point
            Curve curve = m_analyticalLineCollection[0] as Curve;
            lowestElevation = curve.get_EndPoint(0).Z;

            // Find out the elevation of the lowest point.
            foreach (Curve c in m_analyticalLineCollection)
            {
                if (c.get_EndPoint(0).Z < lowestElevation)
                {
                    lowestElevation = c.get_EndPoint(0).Z;
                }
                if (c.get_EndPoint(1).Z < lowestElevation)
                {
                    lowestElevation = c.get_EndPoint(1).Z;
                }
            }

            // Count the offset and return.
            baseOffset = lowestElevation - m_level.Elevation;
            return baseOffset;
        }

        /// <summary>
        /// Find the offset from the elevation of the highest point to m_level's elevation
        /// </summary>
        /// <returns>The length of the offset</returns>
        Double FindTopOffset()
        {
            // Initialize the data
            Double topOffset = 0;   // the offset from the m_level's elevation
            Double highestElevation = 0;    // the elevation of the highest point
            Curve curve = m_analyticalLineCollection[0] as Curve;
            highestElevation = curve.get_EndPoint(0).Z;

            // Find out the elevation of the highest point.
            foreach (Curve c in m_analyticalLineCollection)
            {
                if (c.get_EndPoint(0).Z > highestElevation)
                {
                    highestElevation = c.get_EndPoint(0).Z;
                }
                if (c.get_EndPoint(1).Z > highestElevation)
                {
                    highestElevation = c.get_EndPoint(1).Z;
                }
            }

            // Count the offset and return.
            topOffset = highestElevation - m_level.Elevation;
            return topOffset;
        }
    }
}
