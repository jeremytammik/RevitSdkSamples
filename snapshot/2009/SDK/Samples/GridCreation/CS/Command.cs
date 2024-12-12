//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Enums;
using Application = Autodesk.Revit.Application;
using Element = Autodesk.Revit.Element;

namespace Revit.SDK.Samples.GridCreation.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    public class Command : IExternalCommand
    {
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
        public virtual IExternalCommand.Result Execute(ExternalCommandData commandData
            , ref string message, ElementSet elements)
        {
            try
            {
                Document document = commandData.Application.ActiveDocument;

                // Get all selected lines and arcs 
                CurveArray selectedCurves = GetSelectedCurves(commandData.Application.ActiveDocument);

                // Show UI
                GridCreationOptionData gridCreationOption = new GridCreationOptionData(!selectedCurves.IsEmpty);
                using (GridCreationOptionForm gridCreationOptForm = new GridCreationOptionForm(gridCreationOption))
                {
                    DialogResult result = gridCreationOptForm.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return IExternalCommand.Result.Cancelled;
                    }

                    ArrayList labels = GetAllLabelsOfGrids(document);
                    DisplayUnitType dut = GetLengthUnitType(document);
                    switch (gridCreationOption.CreateGridsMode)
                    {
                        case CreateMode.Select: // Create grids with selected lines/arcs
                            CreateWithSelectedCurvesData data = new CreateWithSelectedCurvesData(commandData.Application, selectedCurves, labels);
                            using (CreateWithSelectedCurvesForm createWithSelected = new CreateWithSelectedCurvesForm(data))
                            {
                                result = createWithSelected.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    // Create grids
                                    data.CreateGrids();
                                }  
                            }
                            break;

                        case CreateMode.Orthogonal: // Create orthogonal grids
                            CreateOrthogonalGridsData orthogonalData = new CreateOrthogonalGridsData(commandData.Application, dut, labels);
                            using (CreateOrthogonalGridsForm orthogonalGridForm = new CreateOrthogonalGridsForm(orthogonalData))
                            {
                                result = orthogonalGridForm.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    // Create grids
                                    orthogonalData.CreateGrids();
                                }  
                            }
                            break;

                        case CreateMode.RadialAndArc: // Create radial and arc grids
                            CreateRadialAndArcGridsData radArcData = new CreateRadialAndArcGridsData(commandData.Application, dut, labels);
                            using (CreateRadialAndArcGridsForm radArcForm = new CreateRadialAndArcGridsForm(radArcData))
                            {
                                result = radArcForm.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    // Create grids
                                    radArcData.CreateGrids();
                                }  
                            }
                            break;
                    }

                    if (result == DialogResult.OK)
                    {
                        return IExternalCommand.Result.Succeeded;
                    }
                    else
                    {
                        return IExternalCommand.Result.Cancelled;
                    }                    
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return IExternalCommand.Result.Failed;
            }
        }

        /// <summary>
        /// Get all selected lines and arcs
        /// </summary>
        /// <param name="document">Revit's document</param>
        /// <returns>CurveArray contains all selected lines and arcs</returns>
        private static CurveArray GetSelectedCurves(Document document)
        {
            CurveArray selectedCurves = new CurveArray();
            ElementSet elements = document.Selection.Elements;
            foreach (Autodesk.Revit.Element element in elements)
            {
                if ((element is ModelLine) || (element is ModelArc))
                {
                    ModelCurve modelCurve = element as ModelCurve;
                    Curve curve = modelCurve.GeometryCurve;
                    if (curve != null)
                    {
                        selectedCurves.Append(curve);
                    }
                }
                else if ((element is DetailLine) || (element is DetailArc))
                {
                    DetailCurve detailCurve = element as DetailCurve;
                    Curve curve = detailCurve.GeometryCurve;
                    if (curve != null)
                    {
                        selectedCurves.Append(curve);
                    }
                }
            }

            return selectedCurves;
        }

        /// <summary>
        /// Get all model and detail lines/arcs within selected elements
        /// </summary>
        /// <param name="document">Revit's document</param>
        /// <returns>ElementSet contains all model and detail lines/arcs within selected elements </returns>
        public static ElementSet GetSelectedModelLinesAndArcs(Document document)
        {
            ElementSet elements = document.Selection.Elements;
            ElementSet tmpSet = new ElementSet();
            foreach (Autodesk.Revit.Element element in elements)
            {
                if ((element is ModelLine) || (element is ModelArc) || (element is DetailLine) || (element is DetailArc))
                {
                    tmpSet.Insert(element);
                }
            }

            return tmpSet;
        }

        /// <summary>
        /// Get current length display unit type
        /// </summary>
        /// <param name="document">Revit's document</param>
        /// <returns>Current length display unit type</returns>
        private static DisplayUnitType GetLengthUnitType(Document document)
        {
            UnitType unittype = UnitType.UT_Length;
            ProjectUnit projectUnit = document.ProjectUnit;
            try
            {
                Autodesk.Revit.FormatOptions formatOption = projectUnit.get_FormatOptions(unittype);
                return formatOption.Units;
            }
            catch (System.Exception /*e*/)
            {
                return DisplayUnitType.DUT_DECIMAL_FEET;
            }
        }

        /// <summary>
        /// Get all grid labels in current document
        /// </summary>
        /// <param name="document">Revit's document</param>
        /// <returns>ArrayList contains all grid labels in current document</returns>
        private static ArrayList GetAllLabelsOfGrids(Document document)
        {
            ArrayList labels = new ArrayList();
            ElementIterator itor = document.get_Elements(typeof(Grid));
            itor.Reset();
            for (; itor.MoveNext(); )
            {
                Grid grid = itor.Current as Grid;
                if (null != grid)
                {
                    labels.Add(grid.Name);
                }
            }

            return labels;
        }
    }
}

