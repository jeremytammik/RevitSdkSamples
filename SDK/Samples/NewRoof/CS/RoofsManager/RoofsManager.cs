//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Text;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

using Revit.SDK.Samples.NewRoof.RoofForms.CS;

namespace Revit.SDK.Samples.NewRoof.RoofsManager.CS
{
    /// <summary>
    /// The kinds of roof than can be created.
    /// </summary>
    public enum CreateRoofKind
    {
        FootPrintRoof,
        ExtrusionRoof
    };

    /// <summary>
    /// The RoofsManager is used to manage the operations between Revit and UI.
    /// </summary>
    public class RoofsManager
    {
        // To store a reference to the commandData.
        ExternalCommandData m_commandData;
        // To store the levels info in the Revit.
        List<Level> m_levels;
        // To store the roof types info in the Revit.
        List<RoofType> m_roofTypes;
        // To store a reference to the FootPrintRoofManager to create footprint roof.
        FootPrintRoofManager m_footPrintRoofManager;
        // To store a reference to the ExtrusionRoofManager to create extrusion roof.
        ExtrusionRoofManager m_extrusionRoofManager;

        // To store the selected elements in the Revit
        Selection m_selection;

        // roofs list
        ElementSet m_footPrintRoofs;
        ElementSet m_extrusionRoofs;

        // To store the footprint roof lines.
        Autodesk.Revit.DB.CurveArray m_footPrint;

        // To store the profile lines.
        Autodesk.Revit.DB.CurveArray m_profile;

        // Reference Plane for creating extrusion roof
        List<ReferencePlane> m_referencePlanes;

        // Transaction for manual mode
        Transaction m_transaction;

        // Current creating roof kind.
        public CreateRoofKind RoofKind;

        /// <summary>
        /// The constructor of RoofsManager class.
        /// </summary>
        /// <param name="commandData">The ExternalCommandData</param>
        public RoofsManager(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_levels = new List<Level>();
            m_roofTypes = new List<RoofType>();
            m_referencePlanes = new List<ReferencePlane>();

            m_footPrint = new CurveArray();
            m_profile = new CurveArray();

            m_footPrintRoofManager = new FootPrintRoofManager(commandData);
            m_extrusionRoofManager = new ExtrusionRoofManager(commandData);
            m_selection = commandData.Application.ActiveUIDocument.Selection;

            m_transaction = new Transaction(commandData.Application.ActiveUIDocument.Document, "Document");

            RoofKind = CreateRoofKind.FootPrintRoof;
            Initialize();
        }

        /// <summary>
        /// Initialize the data member
        /// </summary>
        private void Initialize()
        {
            Document doc = m_commandData.Application.ActiveUIDocument.Document;
            FilteredElementIterator iter = (new FilteredElementCollector(doc)).OfClass(typeof(Level)).GetElementIterator();
            iter.Reset();
            while (iter.MoveNext())
            {
                m_levels.Add(iter.Current as Level);
            }

            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            filteredElementCollector.OfClass(typeof(RoofType));
            m_roofTypes = filteredElementCollector.Cast<RoofType>().ToList<RoofType>();

            // FootPrint Roofs
            m_footPrintRoofs = new ElementSet();
            iter = (new FilteredElementCollector(doc)).OfClass(typeof(FootPrintRoof)).GetElementIterator();
            iter.Reset();
            while (iter.MoveNext())
            {
                m_footPrintRoofs.Insert(iter.Current as FootPrintRoof);
            }

            // Extrusion Roofs
            m_extrusionRoofs = new ElementSet();
            iter = (new FilteredElementCollector(doc)).OfClass(typeof(ExtrusionRoof)).GetElementIterator();
            iter.Reset();
            while (iter.MoveNext())
            {
                m_extrusionRoofs.Insert(iter.Current as ExtrusionRoof);
            }

            // Reference Planes
            iter = (new FilteredElementCollector(doc)).OfClass(typeof(ReferencePlane)).GetElementIterator();
            iter.Reset();
            while (iter.MoveNext())
            {
                ReferencePlane plane = iter.Current as ReferencePlane;
                // just use the vertical plane
                if (Math.Abs(plane.Normal.DotProduct(Autodesk.Revit.DB.XYZ.BasisZ)) < 1.0e-09)
                {
                    if (plane.Name == "Reference Plane")
                    {
                        plane.Name = "Reference Plane" + "(" + plane.Id.ToString() + ")";
                    }
                    m_referencePlanes.Add(plane);
                }

            }
        }

        /// <summary>
        /// Get the Level elements.
        /// </summary>
        public ReadOnlyCollection<Level> Levels
        {
            get
            {
                return new ReadOnlyCollection<Level>(m_levels);
            }
        }

        /// <summary>
        /// Get the RoofTypes.
        /// </summary>
        public ReadOnlyCollection<RoofType> RoofTypes
        {
            get
            {
                return new ReadOnlyCollection<RoofType>(m_roofTypes);
            }
        }

        /// <summary>
        /// Get the RoofTypes.
        /// </summary>
        public ReadOnlyCollection<ReferencePlane> ReferencePlanes
        {
            get
            {
                return new ReadOnlyCollection<ReferencePlane>(m_referencePlanes);
            }
        }

        /// <summary>
        /// Get all the footprint roofs in Revit.
        /// </summary>
        public ElementSet FootPrintRoofs
        {
            get
            {
                return m_footPrintRoofs;
            }
        }

        /// <summary>
        /// Get all the extrusion roofs in Revit.
        /// </summary>
        public ElementSet ExtrusionRoofs
        {
            get
            {
                return m_extrusionRoofs;
            }
        }

        /// <summary>
        /// Get the footprint roof lines.
        /// </summary>
        public CurveArray FootPrint
        {
            get
            {
                return m_footPrint;
            }
        }

        /// <summary>
        /// Get the extrusion profile lines.
        /// </summary>
        public CurveArray Profile
        {
            get
            {
                return m_profile;
            }
        }

        /// <summary>
        /// Select elements in Revit to obtain the footprint roof lines or extrusion profile lines.
        /// </summary>
        /// <returns>A curve array to hold the footprint roof lines or extrusion profile lines.</returns>
        public CurveArray WindowSelect()
        {
            if (RoofKind == CreateRoofKind.FootPrintRoof)
            {
                return SelectFootPrint();
            }
            else
            {
                return SelectProfile();
            }
        }

        /// <summary>
        /// Select elements in Revit to obtain the footprint roof lines. 
        /// </summary>
        /// <returns>A curve array to hold the footprint roof lines.</returns>
        public CurveArray SelectFootPrint()
        {
            m_footPrint.Clear();
            while (true)
            {
               ElementSet es = new ElementSet();
               foreach (ElementId elementId in m_selection.GetElementIds())
               {
                  es.Insert(m_commandData.Application.ActiveUIDocument.Document.GetElement(elementId));
               }
                es.Clear();
                IList<Element> selectResult;
                try
                {
                    selectResult = m_selection.PickElementsByRectangle();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }

                if (selectResult.Count != 0)
                {
                    foreach (Autodesk.Revit.DB.Element element in selectResult)
                    {
                        Wall wall = element as Wall;
                        if (wall != null)
                        {
                            LocationCurve wallCurve = wall.Location as LocationCurve;
                            m_footPrint.Append(wallCurve.Curve);
                            continue;
                        }

                        ModelCurve modelCurve = element as ModelCurve;
                        if (modelCurve != null)
                        {
                            m_footPrint.Append(modelCurve.GeometryCurve);
                        }
                    }
                    break;
                }
                else
                {
                    TaskDialogResult result = TaskDialog.Show("Warning", "You should select a curve loop, or a wall loop, or loops combination \r\nof walls and curves to create a footprint roof.", TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel);
                    if (result == TaskDialogResult.Cancel)
                    {
                        break;
                    }
                }


            }

            return m_footPrint;
        }

        /// <summary>
        /// Create a footprint roof. 
        /// </summary>
        /// <param name="level">The base level of the roof to be created.</param>
        /// <param name="roofType">The type of the newly created roof.</param>
        /// <returns>Return a new created footprint roof.</returns>
        public FootPrintRoof CreateFootPrintRoof(Level level, RoofType roofType)
        {
            FootPrintRoof roof = null;
            roof = m_footPrintRoofManager.CreateFootPrintRoof(m_footPrint, level, roofType);
            if (roof != null)
            {
                this.m_footPrintRoofs.Insert(roof);
            }
            return roof;
        }

        /// <summary>
        /// Select elements in Revit to obtain the extrusion profile lines.
        /// </summary>
        /// <returns>A curve array to hold the extrusion profile lines.</returns>
        public CurveArray SelectProfile()
        {
            m_profile.Clear();
            while (true)
            {
                m_selection.GetElementIds().Clear();
                IList<Element> selectResult;
                try
                {
                    selectResult = m_selection.PickElementsByRectangle();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
                if (selectResult.Count != 0)
                {
                    foreach (Autodesk.Revit.DB.Element element in selectResult)
                    {
                        ModelCurve modelCurve = element as ModelCurve;
                        if (modelCurve != null)
                        {
                            m_profile.Append(modelCurve.GeometryCurve);
                            continue;
                        }
                    }
                    break;
                }
                else
                {
                    TaskDialogResult result = TaskDialog.Show("Warning", "You should select a  connected lines or arcs, \r\nnot closed in a loop to create extrusion roof.", TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel);
                    if (result == TaskDialogResult.Cancel)
                    {
                        break;
                    }
                }
            }
            return m_profile;
        }

        /// <summary>
        /// Create a extrusion roof.
        /// </summary>
        /// <param name="refPlane">The reference plane for the extrusion roof.</param>
        /// <param name="level">The reference level of the roof to be created.</param>
        /// <param name="roofType">The type of the newly created roof.</param>
        /// <param name="extrusionStart">The extrusion start point.</param>
        /// <param name="extrusionEnd">The extrusion end point.</param>
        /// <returns>Return a new created extrusion roof.</returns>
        public ExtrusionRoof CreateExtrusionRoof(ReferencePlane refPlane,
            Level level, RoofType roofType, double extrusionStart, double extrusionEnd)
        {
            ExtrusionRoof roof = null;
            roof = m_extrusionRoofManager.CreateExtrusionRoof(m_profile, refPlane, level, roofType, extrusionStart, extrusionEnd);
            if (roof != null)
            {
                m_extrusionRoofs.Insert(roof);
            }
            return roof;
        }

        /// <summary>
        /// Begin a transaction.
        /// </summary>
        /// <returns></returns>
        public TransactionStatus BeginTransaction()
        {
            if (m_transaction.GetStatus() == TransactionStatus.Started)
            {
                TaskDialog.Show("Revit", "Transaction started already");
            }
            return m_transaction.Start();
        }

        /// <summary>
        /// Finish a transaction.
        /// </summary>
        /// <returns></returns>
        public TransactionStatus EndTransaction()
        {
            return m_transaction.Commit();
        }

        /// <summary>
        /// Abort a transaction.
        /// </summary>
        public TransactionStatus AbortTransaction()
        {
            return m_transaction.RollBack();
        }
    }
}