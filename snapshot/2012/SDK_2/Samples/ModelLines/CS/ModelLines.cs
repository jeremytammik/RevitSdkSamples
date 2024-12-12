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
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ModelLines.CS
{
    /// <summary>
    /// The main deal class, which takes charge of showing the number of each model line type
    /// and creating one instance for each type using Revit API
    /// </summary>
    public class ModelLines
    {
        // Private members
        Autodesk.Revit.UI.UIApplication m_revit; // Store the reference of the application in revit
        Autodesk.Revit.Creation.Application m_createApp;// Store the create Application reference
        Autodesk.Revit.Creation.Document m_createDoc;   // Store the create Document reference

        ModelCurveArray m_lineArray;        // Store the ModelLine references
        ModelCurveArray m_arcArray;         // Store the ModelArc references
        ModelCurveArray m_ellipseArray;     // Store the ModelEllipse references
        ModelCurveArray m_hermiteArray;     // Store the ModelHermiteSpline references
        ModelCurveArray m_nurbArray;        // Store the ModelNurbSpline references
        List<SketchPlane> m_sketchArray;    // Store the SketchPlane references

        List<ModelCurveCounter> m_informationMap;   // Store the number of each model line type

        #region Properties
        /// <summary>
        /// The type-number map, store the number of each model line type
        /// </summary>
        public ReadOnlyCollection<ModelCurveCounter> InformationMap
        {
            get
            {
                return new ReadOnlyCollection<ModelCurveCounter>(m_informationMap);
            }
        }

        /// <summary>
        /// Get the id information of all ModelEllipses in revit,
        /// which displayed this in elementIdComboBox when ellipseRadioButton checked
        /// </summary>
        public ReadOnlyCollection<IdInfo> EllispeIDArray
        {
            get
            {
                // Create a new list
                List<IdInfo> idArray = new List<IdInfo>();
                // Add all ModelEllipses' id information into the list
                foreach (ModelCurve ellipse in m_ellipseArray)
                {
                    IdInfo info = new IdInfo("ModelEllipse", ellipse.Id.IntegerValue);
                    idArray.Add(info);
                }
                // return a read only list
                return new ReadOnlyCollection<IdInfo>(idArray);
            }
        }

        /// <summary>
        /// Get the id information of all ModelHermiteSpline in revit,
        /// which displayed this in elementIdComboBox when hermiteSplineRadioButton checked
        /// </summary>
        public ReadOnlyCollection<IdInfo> HermiteSplineIDArray
        {
            get
            {
                // Create a new list
                List<IdInfo> idArray = new List<IdInfo>();
                // Add all ModelHermiteSplines' id information into the list
                foreach (ModelCurve hermite in m_hermiteArray)
                {
                    IdInfo info = new IdInfo("ModelHermiteSpline", hermite.Id.IntegerValue);
                    idArray.Add(info);
                }
                // return a read only list
                return new ReadOnlyCollection<IdInfo>(idArray);
            }
        }


        /// <summary>
        /// Get the id information of all ModelNurbSpline in revit,
        /// which displayed this in elementIdComboBox when NurbSplineRadioButton checked
        /// </summary>
        public ReadOnlyCollection<IdInfo> NurbSplineIDArray
        {
            get
            {
                // Create a new list
                List<IdInfo> idArray = new List<IdInfo>();
                // Add all ModelNurbSplines' id information into the list
                foreach (ModelCurve nurb in m_nurbArray)
                {
                    IdInfo info = new IdInfo("ModelNurbSpline", nurb.Id.IntegerValue);
                    idArray.Add(info);
                }
                // return a read only list
                return new ReadOnlyCollection<IdInfo>(idArray);
            }
        }

        /// <summary>
        /// Allow the user to get all sketch plane in revit
        /// </summary>
        public ReadOnlyCollection<IdInfo> SketchPlaneIDArray
        {
            get
            {
                // Create a new list
                List<IdInfo> idArray = new List<IdInfo>();
                // Add all SketchPlane' id information into the list
                foreach (SketchPlane sketch in m_sketchArray)
                {
                    IdInfo info = new IdInfo("SketchPlane", sketch.Id.IntegerValue);
                    idArray.Add(info);
                }
                // return a read only list
                return new ReadOnlyCollection<IdInfo>(idArray);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The default constructor
        /// </summary>
        /// <param name="revit">The reference of the application in revit</param>
        public ModelLines(Autodesk.Revit.UI.UIApplication revit)
        {
            // Store the reference of the application for further use.
            m_revit = revit;
            // Get the create references
            m_createApp = m_revit.Application.Create;       // Creation.Application
            m_createDoc = m_revit.ActiveUIDocument.Document.Create;// Creation.Document

            // Construct all the ModelCurveArray instances for model lines
            m_lineArray = new ModelCurveArray();
            m_arcArray = new ModelCurveArray();
            m_ellipseArray = new ModelCurveArray();
            m_hermiteArray = new ModelCurveArray();
            m_nurbArray = new ModelCurveArray();

            // Construct the sketch plane list data
            m_sketchArray = new List<SketchPlane>();

            // Construct the information list data
            m_informationMap = new List<ModelCurveCounter>();
        }


        /// <summary>
        /// This is the main deal method in this example.
        /// </summary>
        public void Run()
        {
            // Get all sketch plane in revit
            GetSketchPlane();

            // Get all model lines in revit
            GetModelLines();

            // Initialize the InformationMap property for DataGridView display
            InitDisplayInformation();

            // Display the form and allow the user to create one of each model line in revit
            using (ModelLinesForm displayForm = new ModelLinesForm(this))
            {
                displayForm.ShowDialog();
            }
        }

        /// <summary>
        /// Create a new sketch plane which all model lines are placed on.
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="origin"></param>
        public void CreateSketchPlane(Autodesk.Revit.DB.XYZ normal, Autodesk.Revit.DB.XYZ origin)
        {
            try
            {
                // First create a Geometry.Plane which need in NewSketchPlane() method
                Plane geometryPlane = m_createApp.NewPlane(normal, origin);
                if (null == geometryPlane)  // assert the creation is successful
                {
                    throw new Exception("Create the geometry plane failed.");
                }
                // Then create a sketch plane using the Geometry.Plane
                SketchPlane plane = m_createDoc.NewSketchPlane(geometryPlane);
                if (null == plane)          // assert the creation is successful
                {
                    throw new Exception("Create the sketch plane failed.");
                }

                // Finally add the created plane into the sketch plane array
                m_sketchArray.Add(plane);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not create the sketch plane, message: " + ex.Message);
            }
        }


        /// <summary>
        /// Create the line(ModelLine)
        /// </summary>
        /// <param name="sketchId">the id of the sketch plane</param>
        /// <param name="startPoint">the start point of the line</param>
        /// <param name="endPoint">the end point of the line</param>
        public void CreateLine(int sketchId, Autodesk.Revit.DB.XYZ startPoint, Autodesk.Revit.DB.XYZ endPoint)
        {
            try
            {
                // First get the sketch plane by the giving element id.
                SketchPlane workPlane = GetSketchPlaneById(sketchId);

                // Additional check: start point should not equal end point
                if (startPoint.Equals(endPoint))
                {
                    throw new ArgumentException("Two points should not be the same.");
                }

                // create geometry line
                Line geometryLine = m_createApp.NewLine(startPoint, endPoint, true);
                if (null == geometryLine)       // assert the creation is successful
                {
                    throw new Exception("Create the geometry line failed.");
                }
                // create the ModelLine
                ModelLine line = m_createDoc.NewModelCurve(geometryLine, workPlane) as ModelLine;
                if (null == line)               // assert the creation is successful
                {
                    throw new Exception("Create the ModelLine failed.");
                }
                // Add the created ModelLine into the line array
                m_lineArray.Append(line);

                // Finally refresh information map.
                RefreshInformationMap();
            }
            catch (Exception ex)
            {
                throw new Exception("Can not create the ModelLine, message: " + ex.Message);
            }

        }


        /// <summary>
        /// Create the arc(ModelArc)
        /// </summary>
        /// <param name="sketchId">the id of the sketch plane</param>
        /// <param name="startPoint">the start point of the arc</param>
        /// <param name="endPoint">the end point of the arc</param>
        /// <param name="thirdPoint">the third point which is on the arc</param>
        public void CreateArc(int sketchId, Autodesk.Revit.DB.XYZ startPoint, Autodesk.Revit.DB.XYZ endPoint, Autodesk.Revit.DB.XYZ thirdPoint)
        {
            try
            {
                // First get the sketch plane by the giving element id.
                SketchPlane workPlane = GetSketchPlaneById(sketchId);

                // Additional check: the start, end and third point should not be the same
                if (startPoint.Equals(endPoint) || startPoint.Equals(thirdPoint)
                    || endPoint.Equals(thirdPoint))
                {
                    throw new ArgumentException("Three points should not be the same.");
                }

                // create the geometry arc
                Arc geometryArc = m_createApp.NewArc(startPoint, endPoint, thirdPoint);
                if (null == geometryArc)            // assert the creation is successful
                {
                    throw new Exception("Create the geometry arc failed.");
                }
                // create the ModelArc
                ModelArc arc = m_createDoc.NewModelCurve(geometryArc, workPlane) as ModelArc;
                if (null == arc)                    // assert the creation is successful
                {
                    throw new Exception("Create the ModelArc failed.");
                }
                // Add the created ModelArc into the arc array
                m_arcArray.Append(arc);

                // Finally refresh information map.
                RefreshInformationMap();
            }
            catch (Exception ex)
            {
                throw new Exception("Can not create the ModelArc, message: " + ex.Message);
            }
        }


        /// <summary>
        /// Create other lines, including Ellipse, HermiteSpline and NurbSpline
        /// </summary>
        /// <param name="sketchId">the id of the sketch plane</param>
        /// <param name="elementId">the element id which copy the curve from</param>
        /// <param name="offsetPoint">the offset direction from the copied line</param>
        public void CreateOthers(int sketchId, int elementId, Autodesk.Revit.DB.XYZ offsetPoint)
        {
            // First get the sketch plane by the giving element id.
            SketchPlane workPlane = GetSketchPlaneById(sketchId);

            // Because the geometry of these lines can't be created by API,
            // use an existing geometry to create ModelEllipse, ModelHermiteSpline, ModelNurbSpline
            // and then move a bit to make the user see the creation distinctly

            // This method use NewModelCurveArray() method to create model lines
            CurveArray curves = m_createApp.NewCurveArray();// create a geometry curve array

            // Get the Autodesk.Revit.DB.ElementId which used to get the corresponding element
            ModelCurve selected = GetElementById(elementId) as ModelCurve;
            if (null == selected)
            {
                throw new Exception("Don't have the element you select");
            }

            // add the geometry curve of the element
            curves.Append(selected.GeometryCurve);    // add the geometry ellipse

            // Create the model line
            ModelCurveArray modelCurves = m_createDoc.NewModelCurveArray(curves, workPlane);
            if (null == modelCurves || 1 != modelCurves.Size) // assert the creation is successful
            {
                throw new Exception("Create the ModelCurveArray failed.");
            }

            // Offset the create model lines in order to differentiate the existing model lines
            foreach (ModelCurve m in modelCurves)
            {
                ElementTransformUtils.MoveElement(m.Document, m.Id, offsetPoint); // move the lines
            }
            // Add the created model lines into corresponding array
            foreach (ModelCurve m in modelCurves)
            {
                switch (m.GetType().Name)
                {
                    case "ModelEllipse":            // If the line is Ellipse
                        m_ellipseArray.Append(m);   // Add to Ellipse array
                        break;
                    case "ModelHermiteSpline":      // If the line is HermiteSpline
                        m_hermiteArray.Append(m);   // Add to HermiteSpline array
                        break;
                    case "ModelNurbSpline":         // If the line is NurbSpline
                        m_nurbArray.Append(m);      // Add to NurbSpline
                        break;
                    default:
                        break;
                }
            }

            // Finally refresh information map.
            RefreshInformationMap();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get all model lines in current document of revit, and store them into the arrays
        /// </summary>
        void GetModelLines()
        {
            // Search all elements in current document and find all model lines
            // ModelLine is not supported by ElementClassFilter/OfClass, 
            // so use its base type to find all CurveElement and then process the results further to find modelline
            IEnumerable<ModelCurve> modelCurves = from elem in ((new FilteredElementCollector(m_revit.ActiveUIDocument.Document)).OfClass(typeof(CurveElement)).ToElements())
                                                  let modelCurve = elem as ModelCurve
                                                  where modelCurve != null
                                                  select modelCurve;
            foreach (ModelCurve modelCurve in modelCurves)
            {
                // Get all the ModelLines references
                String typeName = modelCurve.GetType().Name;
                switch (typeName)
                {
                    case "ModelLine":   // Get all the ModelLine references
                        m_lineArray.Append(modelCurve);
                        break;
                    case "ModelArc":    // Get all the ModelArc references
                        m_arcArray.Append(modelCurve);
                        break;
                    case "ModelEllipse":// Get all the ModelEllipse references
                        m_ellipseArray.Append(modelCurve);
                        break;
                    case "ModelHermiteSpline":  // Get all the ModelHermiteSpline references
                        m_hermiteArray.Append(modelCurve);
                        break;
                    case "ModelNurbSpline": // Get all the ModelNurbSpline references
                        m_nurbArray.Append(modelCurve);
                        break;
                    default:    // If not a model curve, just break
                        break;
                }  
            }
        }


        /// <summary>
        /// Get all sketch planes in revit
        /// </summary>
        void GetSketchPlane()
        {
           // Search all elements in current document and find all sketch planes
           IList<Element> elements = (new FilteredElementCollector(m_revit.ActiveUIDocument.Document)).OfClass(typeof(SketchPlane)).ToElements();
           foreach (Element elem in elements)
           {
                SketchPlane sketch = elem as SketchPlane;
                if (null != sketch)
                {
                    // Add all the sketchPlane into the array
                    m_sketchArray.Add(sketch);
                }
            }
        }

        /// <summary>
        /// Initiate the information map which will display in information DataGridView 
        /// </summary>
        void InitDisplayInformation()
        {
            // First add the type name into the m_information data map
            m_informationMap.Add(new ModelCurveCounter("ModelArc"));
            m_informationMap.Add(new ModelCurveCounter("ModelLine"));
            m_informationMap.Add(new ModelCurveCounter("ModelEllipse"));
            m_informationMap.Add(new ModelCurveCounter("ModelHermiteSpline"));
            m_informationMap.Add(new ModelCurveCounter("ModelNurbSpline"));

            // Use RefreshInformationMap to refresh the number of each model line type
            RefreshInformationMap();
        }


        /// <summary>
        /// Refresh the m_informationMap member, include the number of each model line type
        /// </summary>
        public void RefreshInformationMap()
        {
            // Search the model line types in the map, and refresh the number of each type
            foreach (ModelCurveCounter info in m_informationMap)
            {
                switch (info.TypeName)
                {
                    case "ModelArc":    // if the type is ModelAre
                        info.Number = m_arcArray.Size;  // refresh the number of arc
                        break;
                    case "ModelLine":   // if the type is ModelLine
                        info.Number = m_lineArray.Size; // refresh the number of line
                        break;
                    case "ModelEllipse":// If the type is ModelEllipse
                        info.Number = m_ellipseArray.Size;  // refresh the number of ellipse
                        break;
                    case "ModelHermiteSpline":  // If the type is ModelHermiteSpline
                        info.Number = m_hermiteArray.Size;  // refresh the number of HermiteSpline
                        break;
                    case "ModelNurbSpline":     // If the type is ModelNurbSpline
                        info.Number = m_nurbArray.Size;     // refresh the number of NurbSpline
                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// Use Autodesk.Revit.DB.ElementId to get the corresponding element
        /// </summary>
        /// <param name="id">the element id value</param>
        /// <returns>the corresponding element</returns>
        Autodesk.Revit.DB.Element GetElementById(int id)
        {
            // Create a Autodesk.Revit.DB.ElementId data
            Autodesk.Revit.DB.ElementId elementId = new Autodesk.Revit.DB.ElementId(id);

            // Get the corresponding element
            return m_revit.ActiveUIDocument.Document.get_Element(elementId);
        }


        /// <summary>
        /// Use Autodesk.Revit.DB.ElementId to get the corresponding sketch plane
        /// </summary>
        /// <param name="id">the element id value</param>
        /// <returns>the corresponding sketch plane</returns>
        SketchPlane GetSketchPlaneById(int id)
        {
            // First get the sketch plane by the giving element id.
            SketchPlane workPlane = GetElementById(id) as SketchPlane;
            if (null == workPlane)
            {
                throw new Exception("Don't have the work plane you select.");
            }
            return workPlane;
        }

        #endregion
    }
}
