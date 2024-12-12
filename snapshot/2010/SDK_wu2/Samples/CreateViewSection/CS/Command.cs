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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
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
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.CreateViewSection.CS
{
    /// <summary>
    /// The main class which given a linear element, such as a wall, floor or beam,
    /// generates a section view across the mid point of the element.
    /// </summary>
    public class Command : IExternalCommand
    {
        // Private Members
        Autodesk.Revit.Document m_project;  // Store the current document in revit   
        String m_errorInformation;  // Store the error information
        const Double PRECISION = 0.0000000001;  // Define a precision of double data

        BoundingBoxXYZ m_box;       // Store the BoundingBoxXYZ reference used in creation
        Autodesk.Revit.Element m_currentComponent; // Store the selected element
        SelectType m_type;     // Indicate the type of the selected element.
                        // 0 - wall, 1 - beam, 2 - floor, -1 - invalid
        const Double LENGTH = 10;   // Define half length and width of BoudingBoxXYZ
        const Double HEIGHT = 5;    // Define height of the BoudingBoxXYZ

        // Define a enum to indicate the selected element type
        enum SelectType
        {
            WALL = 0,
            BEAM = 1,
            FLOOR = 2,
            INVALID = -1         
        }

        // Methods
        /// <summary>
        /// Default constructor of Command
        /// </summary>
        public Command()
        {
            m_type = SelectType.INVALID;
        }

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
        public IExternalCommand.Result Execute(ExternalCommandData commandData,
                                                    ref string message, ElementSet elements)
        {
            try
            {
                m_project = commandData.Application.ActiveDocument;

                // Get the selected element and store it to data member.
                if (!GetSelectedElement())
                {
                    message = m_errorInformation;
                    return IExternalCommand.Result.Failed;
                }

                // Create a BoundingBoxXYZ instance which used in NewViewSection() method
                if (!GenerateBoundingBoxXYZ())
                {
                    message = m_errorInformation;
                    return IExternalCommand.Result.Failed;
                }

                // Create a section view. 
                ViewSection section = m_project.Create.NewViewSection(m_box);
                if (null == section)
                {
                    message = "Can't create the ViewSection.";
                    return IExternalCommand.Result.Failed;
                }

                // Modify some parameters to make it look better.
                section.get_Parameter(BuiltInParameter.VIEW_DETAIL_LEVEL).Set(2);

                // If everything goes right, give successful information and return succeeded.
                MessageBox.Show("Create view section succeeded.", "Revit");
                return IExternalCommand.Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return IExternalCommand.Result.Failed;
            }            
        }


        /// <summary>
        /// Get the selected element, and check whether it is a wall, a floor or a beam.
        /// </summary>
        /// <returns>true if the process succeed; otherwise, false.</returns>
        Boolean GetSelectedElement()
        {
            // First get the selection, and make sure only one element in it.
            ElementSet collection = m_project.Selection.Elements;
            if (1 != collection.Size)
            {
                m_errorInformation = 
                    "Please select only one element, such as a wall, a beam or a floor.";
                return false;
            }

            // Get the selected element.
            foreach (Autodesk.Revit.Element e in collection)
            {
                m_currentComponent = e;
            }

            // Make sure the element to be a wall, beam or a floor.
            if (m_currentComponent is Wall)
            {
                // Check whether the wall is a linear wall
                LocationCurve location = m_currentComponent.Location as LocationCurve;
                if (null == location)
                {
                    m_errorInformation = "The selected wall should be linear.";
                    return false;
                }
                if (location.Curve is Line)
                {
                    m_type = SelectType.WALL;   // when the element is a linear wall
                    return true;
                }
                else
                {
                    m_errorInformation = "The selected wall should be linear.";
                    return false;
                }
            }

            FamilyInstance beam = m_currentComponent as FamilyInstance;
            if (null != beam && StructuralType.Beam == beam.StructuralType)
            {
                m_type = SelectType.BEAM;       // when the element is a beam
                return true;
            }

            if (m_currentComponent is Floor)
            {
                m_type = SelectType.FLOOR;      // when the element is a floor.
                return true;
            }

            // If it is not a wall, a beam or a floor, give error information.
            m_errorInformation = "Please select an element, such as a wall, a beam or a floor.";
            return false;
        }


        /// <summary>
        /// Generate a BoundingBoxXYZ instance which used in NewViewSection() method
        /// </summary>
        /// <returns>true if the instance can be created; otherwise, false.</returns>
        Boolean GenerateBoundingBoxXYZ()
        {
            // First new a BoundingBoxXYZ, and set the MAX and Min property.
            m_box = new BoundingBoxXYZ();
            m_box.Enabled = true;
            XYZ maxPoint = new XYZ(LENGTH, LENGTH, 0);
            XYZ minPoint = new XYZ(-LENGTH, -LENGTH, -HEIGHT);
            m_box.Max = maxPoint;
            m_box.Min = minPoint;

            // Set Transform property is the most important thing.
            // It define the Orgin and the directions(include RightDirection, 
            // UpDirection and ViewDirection) of the created view.
            Transform transform = GenerateTransform();
            if (null == transform)
            {
                return false;
            }
            m_box.Transform = transform;
            
            // If all went well, return true.
            return true;
        }


        /// <summary>
        /// Generate a Transform instance which as Transform property of BoundingBoxXYZ
        /// </summary>
        /// <returns>the reference of Transform, return null if it can't be generated</returns>
        Transform GenerateTransform()
        {
            // Because different element have different ways to create Transform.
            // So, this method just call corresponding method.
            if (SelectType.WALL == m_type)
            {
                return GenerateWallTransform();
            }
            else if (SelectType.BEAM == m_type)
            {
                return GenerateBeamTransform();
            }
            else if (SelectType.FLOOR == m_type)
            {
                return GenerateFloorTransform();
            }
            else
            {
                m_errorInformation = "The program should never go here.";
                return null;
            }
        }


        /// <summary>
        /// Generate a Transform instance which as Transform property of BoundingBoxXYZ, 
        /// when the user select a wall, this method will be called
        /// </summary>
        /// <returns>the reference of Transform, return null if it can't be generated</returns>
        Transform GenerateWallTransform()
        {
            Transform transform = null;
            Wall wall = m_currentComponent as Wall;

            // Because the architecture wall and curtain wall don't have analytical Model lines.
            // So Use Location property of wall object is better choice.
            // First get the location line of the wall
            LocationCurve location = wall.Location as LocationCurve;
            Line locationLine = location.Curve as Line;
            transform = Transform.Identity;

            // Second find the middle point of the wall and set it as Origin property.
            XYZ midPoint = XYZMath.FindMidPoint(locationLine.get_EndPoint(0), locationLine.get_EndPoint(1));
            // midPoint is mid point of the wall location, but not the wall's.
            // The different is the elevation of the point. Then change it.
            midPoint.Z += GetWallMidOffsetFromLocation(wall);
            transform.Origin = midPoint;

            // At last find out the directions of the created view, and set it as Basis property.
            XYZ basisZ = XYZMath.FindDirection(locationLine.get_EndPoint(0), locationLine.get_EndPoint(1));
            XYZ basisX = XYZMath.FindRightDirection(basisZ);
            XYZ basisY = XYZMath.FindUpDirection(basisZ);

            transform.set_Basis(0, basisX);
            transform.set_Basis(1, basisY);
            transform.set_Basis(2, basisZ);
            return transform;
        }


        /// <summary>
        /// Generate a Transform instance which as Transform property of BoundingBoxXYZ, 
        /// when the user select a beam, this method will be called
        /// </summary>
        /// <returns>the reference of Transform, return null if it can't be generated</returns>
        Transform GenerateBeamTransform()
        {
            Transform transform = null;
            FamilyInstance instance = m_currentComponent as FamilyInstance;

            // First check whether the beam is horizontal.
            // In order to predigest the calculation, only allow it to be horizontal
            double startOffset = instance.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION).AsDouble();
            double endOffset = instance.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION).AsDouble();
            if (-PRECISION > startOffset - endOffset || PRECISION < startOffset - endOffset)
            {
                m_errorInformation = "Please select a horizontal beam.";
                return transform;
            }

            // Second get the Analytical Model line.
            AnalyticalModel model = instance.AnalyticalModel;
            if (null == model)
            {
                m_errorInformation = "The selected beam doesn't have Analytical Model line.";
                return transform;
            }
            AnalyticalModelFrame modelFrame = model as AnalyticalModelFrame;
            if (null == modelFrame)
            {
                m_errorInformation = "The program should never go here.";
                return transform;
            }
            Curve curve = modelFrame.Curve;
            if (null == curve)
            {
                m_errorInformation = "The program should never go here.";
                return transform;
            }

            // Now I am sure I can create a transform instance.
            transform = Transform.Identity;

            // Third find the middle point of the line and set it as Origin property.
            XYZ startPoint = curve.get_EndPoint(0);
            XYZ endPoint = curve.get_EndPoint(1);
            XYZ midPoint = XYZMath.FindMidPoint(startPoint, endPoint);
            transform.Origin = midPoint;

            // At last find out the directions of the created view, and set it as Basis property.   
            XYZ basisZ = XYZMath.FindDirection(startPoint, endPoint);
            XYZ basisX = XYZMath.FindRightDirection(basisZ);
            XYZ basisY = XYZMath.FindUpDirection(basisZ);
            
            transform.set_Basis(0, basisX);
            transform.set_Basis(1, basisY);
            transform.set_Basis(2, basisZ);
            return transform;
        }


        /// <summary>
        /// Generate a Transform instance which as Transform property of BoundingBoxXYZ, 
        /// when the user select a floor, this method will be called
        /// </summary>
        /// <returns>the reference of Transform, return null if it can't be generated</returns>
        Transform GenerateFloorTransform()
        {
            Transform transform = null;
            Floor floor = m_currentComponent as Floor;

            // First get the Analytical Model lines
            AnalyticalModel model = floor.AnalyticalModel;
            if (null == model)
            {
                m_errorInformation = "Please select a structural floor.";
                return transform;
            }
            AnalyticalModelFloor modelFloor = model as AnalyticalModelFloor;
            if (null == modelFloor)
            {
                m_errorInformation = "The program should never go here.";
                return transform;
            }
            CurveArray curves = modelFloor.Curves;
            if (null == curves || true == curves.IsEmpty)
            {
                m_errorInformation = "The program should never go here.";
                return transform;
            }

            // Now I am sure I can create a transform instance.
            transform = Transform.Identity;

            // Third find the middle point of the floor and set it as Origin property.
            XYZ midPoint = XYZMath.FindMiddlePoint(curves);
            transform.Origin = midPoint;

            // At last find out the directions of the created view, and set it as Basis property.
            XYZ basisZ = XYZMath.FindFloorViewDirection(curves);
            XYZ basisX = XYZMath.FindRightDirection(basisZ);
            XYZ basisY = XYZMath.FindUpDirection(basisZ);

            transform.set_Basis(0, basisX);
            transform.set_Basis(1, basisY);
            transform.set_Basis(2, basisZ);
            return transform;
        }

        Double GetWallMidOffsetFromLocation(Wall wall)
        {
            // First get the "Base Offset" property.
            Double baseOffset = wall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).AsDouble();

            // Second get the "Unconnected Height" property. 
            Double height = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();

            // Get the middle of of wall elevation from the wall location.
            // The elevation of wall location equals the elevation of "Base Constraint" level
            Double midOffset = baseOffset + height / 2;
            return midOffset;
        }
    }

    /// <summary>
    /// Create a drafting view. 
    /// </summary>
    public class CreateDraftingView : IExternalCommand
    {
        public IExternalCommand.Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                Autodesk.Revit.Document doc = commandData.Application.ActiveDocument;
                ViewDrafting drafting = doc.Create.NewViewDrafting();
                if (null == drafting)
                {
                    message = "Can't create the ViewDrafting.";
                    return IExternalCommand.Result.Failed;
                }
                MessageBox.Show("Create view drafting succeeded.", "Revit");
                return IExternalCommand.Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return IExternalCommand.Result.Failed;
            }
        }
    }
}
