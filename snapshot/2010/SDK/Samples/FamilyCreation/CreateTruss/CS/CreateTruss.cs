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
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Enums;

using Element = Autodesk.Revit.Element;
using View = Autodesk.Revit.Elements.View;

namespace Revit.SDK.Samples.CreateTruss.CS
{
    /// <summary>
    /// A class inherits IExternalCommand interface.
    /// this class creates a mono truss in truss family document.
    /// </summary>
    public class Command : IExternalCommand
    {
        /// <summary>
        /// The revit application
        /// </summary>
        private static Autodesk.Revit.Application m_application;

        /// <summary>
        /// The current document of the application
        /// </summary>
        private static Autodesk.Revit.Document m_document;

        /// <summary>
        /// The Application Creation object is used to create new instances of utility objects.
        /// </summary>
        private Autodesk.Revit.Creation.Application m_appCreator;

        /// <summary>
        /// the creation factory to create model lines, dimensions and alignments
        /// </summary>
        private Autodesk.Revit.Creation.FamilyItemFactory m_familyCreator;


        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
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
        public IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData revit,
                                                              ref string message,
                                                              Autodesk.Revit.ElementSet elements)
        {
            try
            {
                m_application = revit.Application;
                m_document = m_application.ActiveDocument;

                // it can support in truss family document only
                if (!m_document.IsFamilyDocument
                    || m_document.OwnerFamily.FamilyCategory.Id.Value != (int)BuiltInCategory.OST_Truss)
                {
                    message = "Cannot execute truss creation in non-truss family document";
                    return IExternalCommand.Result.Failed;
                }

                m_appCreator = m_application.Create;
                m_familyCreator = m_document.FamilyCreate;

                // Start the truss creation
                MakeNewTruss();
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return IExternalCommand.Result.Failed;
            }
            return IExternalCommand.Result.Succeeded;
        }

        /// <summary>
        /// Example demonstrating truss creation in the Autodesk Revit API. This example constructs
        /// a "mono" truss aligned with the reference planes in the (already loaded) truss family
        /// document.
        /// </summary>
        private void MakeNewTruss()
        {
            // Constants for arranging the angular truss members
            double webAngle = 35.0;
            double webAngleRadians = (180 - webAngle) * Math.PI / 180.0;
            XYZ angleDirection = new XYZ(Math.Cos(webAngleRadians), Math.Sin(webAngleRadians), 0);

            // Look up the reference planes and view in which to sketch 
            ReferencePlane top = null, bottom = null, left = null, right = null, center = null;
            View level1 = null;
            List<Element> elements = new List<Element>();
            TypeFilter refPlaneFilter = m_appCreator.Filter.NewTypeFilter(typeof(ReferencePlane), true);
            TypeFilter viewFilter = m_appCreator.Filter.NewTypeFilter(typeof(View), true);
            Filter filter = m_appCreator.Filter.NewLogicOrFilter(refPlaneFilter, viewFilter);
            m_document.get_Elements(filter, elements);

            foreach (Element e in elements)
            {
                switch (e.Name)
                {
                    case "Top": top = e as ReferencePlane; break;
                    case "Bottom": bottom = e as ReferencePlane; break;
                    case "Right": right = e as ReferencePlane; break;
                    case "Left": left = e as ReferencePlane; break;
                    case "Center": center = e as ReferencePlane; break;
                    case "Level 1": level1 = e as View; break;
                }
            }
            if (top == null || bottom == null || left == null 
                || right == null || center == null || level1 == null)
                throw new InvalidOperationException("Could not find prerequisite named reference plane or named view.");

            SketchPlane sPlane = level1.SketchPlane;

            // Extract the geometry of each reference plane
            Line bottomLine = GetReferencePlaneLine(bottom);
            Line leftLine = GetReferencePlaneLine(left);
            Line rightLine = GetReferencePlaneLine(right);
            Line topLine = GetReferencePlaneLine(top);
            Line centerLine = GetReferencePlaneLine(center);

            // Create bottom chord along "bottom" from "left" to "right"
            XYZ bottomLeft = GetIntersection(bottomLine, leftLine);
            XYZ bottomRight = GetIntersection(bottomLine, rightLine);
            ModelCurve bottomChord = MakeTrussCurve(bottomLeft, bottomRight, sPlane, TrussCurveType.BottomChord);
            if (null != bottomChord)
            {
                // Add the alignment constraint to the bottom chord.
                Curve geometryCurve = bottomChord.GeometryCurve;
                // Lock the bottom chord to bottom reference plan
                m_familyCreator.NewAlignment(level1, bottom.Reference, geometryCurve.Reference);
            }

            // Create web connecting top and bottom chords on the right side
            XYZ topRight = GetIntersection(topLine, rightLine);
            ModelCurve rightWeb = MakeTrussCurve(bottomRight, topRight, sPlane, TrussCurveType.Web);
            if (null != rightWeb)
            {
                // Add the alignment constraint to the right web chord.
                Curve geometryCurve = rightWeb.GeometryCurve;
                // Lock the right web chord to right reference plan
                m_familyCreator.NewAlignment(level1, right.Reference, geometryCurve.Reference);          
            }

            // Create top chord diagonally from bottom-left to top-right
            ModelCurve topChord = MakeTrussCurve(bottomLeft, topRight, sPlane, TrussCurveType.TopChord);
            if (null != topChord)
            {
                // Add the alignment constraint to the top chord.
                Curve geometryCurve = topChord.GeometryCurve;
                // Lock the start point of top chord to the Intersection of left and bottom reference plan
                m_familyCreator.NewAlignment(level1, geometryCurve.get_EndPointReference(0), left.Reference);
                m_familyCreator.NewAlignment(level1, geometryCurve.get_EndPointReference(0), bottom.Reference);
                // Lock the end point of top chord to the Intersection of right and top reference plan
                m_familyCreator.NewAlignment(level1, geometryCurve.get_EndPointReference(1), top.Reference);
                m_familyCreator.NewAlignment(level1, geometryCurve.get_EndPointReference(1), right.Reference);
            }

            // Create angled web from midpoint to the narrow end of the truss
            XYZ bottomMidPoint = GetIntersection(bottomLine, centerLine);
            Line webDirection = m_appCreator.NewLineUnbound(bottomMidPoint, angleDirection);
            XYZ endOfWeb = GetIntersection(topChord.GeometryCurve as Line, webDirection);
            ModelCurve angledWeb = MakeTrussCurve(bottomMidPoint, endOfWeb, sPlane, TrussCurveType.Web);

            // Add a dimension to force the angle to be stable even when truss length and height are modified
            Arc dimensionArc = m_appCreator.NewArc(
                bottomMidPoint, angledWeb.GeometryCurve.Length / 2, webAngleRadians, Math.PI, XYZ.BasisX, XYZ.BasisY);
            Dimension createdDim = m_familyCreator.NewAngularDimension(
                level1, dimensionArc, angledWeb.GeometryCurve.Reference, bottomChord.GeometryCurve.Reference);
            if (null != createdDim)
                createdDim.IsLocked = true;

            // Create angled web from corner to top of truss
            XYZ bottomRight2 = GetIntersection(bottomLine, rightLine);
            webDirection = m_appCreator.NewLineUnbound(bottomRight2, angleDirection);
            endOfWeb = GetIntersection(topChord.GeometryCurve as Line, webDirection);
            ModelCurve angledWeb2 = MakeTrussCurve(bottomRight, endOfWeb, sPlane, TrussCurveType.Web);

            // Add a dimension to force the angle to be stable even when truss length and height are modified
            dimensionArc = m_appCreator.NewArc(
                bottomRight, angledWeb2.GeometryCurve.Length / 2, webAngleRadians, Math.PI, XYZ.BasisX, XYZ.BasisY);
            createdDim = m_familyCreator.NewAngularDimension(
                level1, dimensionArc, angledWeb2.GeometryCurve.Reference, bottomChord.GeometryCurve.Reference);
            if (null != createdDim)
                createdDim.IsLocked = true;

            //Connect bottom midpoint to end of the angled web
            ModelCurve braceWeb = MakeTrussCurve(bottomMidPoint, endOfWeb, sPlane, TrussCurveType.Web);
        }

        /// <summary>
        /// Utility method to create a truss model curve.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        /// <param name="sketchPlane">The sketch plane for the new curve.</param>
        /// <param name="type">The type of truss curve.</param>
        /// <returns>the created truss model curve.</returns>
        private ModelCurve MakeTrussCurve(XYZ start, XYZ end, SketchPlane sketchPlane, TrussCurveType type)
        {
            Line line = m_appCreator.NewLineBound(start, end);
            ModelCurve trussCurve = m_familyCreator.NewModelCurve(line, sketchPlane);
            trussCurve.TrussCurveType = type;

            return trussCurve;
        }

        /// <summary>
        /// Utility method for to extract the geometry of a reference plane in a family.
        /// </summary>
        /// <param name="plane">The reference plane.</param>
        /// <returns>An unbounded line representing the location of the plane.</returns>
        private Line GetReferencePlaneLine(ReferencePlane plane)
        {
            // Reset the "elevation" of the plane's line to Z=0, since that's where the lines will be placed.  
            // Otherwise, some intersection calculation may fail
            XYZ origin = plane.BubbleEnd;
            origin.Z = 0.0;
            Line line = m_appCreator.NewLineUnbound(origin, plane.Direction);

            return line;
        }

        /// <summary>
        /// Utility method for getting the intersection between two lines.
        /// </summary>
        /// <param name="line1">The first line.</param>
        /// <param name="line2">The second line.</param>
        /// <returns>The intersection point.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an intersection can't be found.</exception>
        private XYZ GetIntersection(Line line1, Line line2)
        {
            IntersectionResultArray results;
            Autodesk.Revit.Enums.SetComparisonResult result = line1.Intersect(line2, out results);

            if (result != Autodesk.Revit.Enums.SetComparisonResult.Overlap)
                throw new InvalidOperationException("Input lines did not intersect.");

            if (results == null || results.Size != 1)
                throw new InvalidOperationException("Could not extract intersection point for lines.");

            IntersectionResult iResult = results.get_Item(0);
            XYZ intersectionPoint = iResult.XYZPoint;

            return intersectionPoint;
        }
    }
}
