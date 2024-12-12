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


namespace Revit.SDK.Samples.CreateBeamSystem.CS
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Collections;
    using System.Diagnostics;
    using System.Collections.ObjectModel;

    using Autodesk.Revit;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.DB.Structure;

    /// <summary>
    /// mixed data class save the data to show in UI
    /// and the data used to create beam system
    /// </summary>
    public class BeamSystemData
    {
        /// <summary>
        /// all beam types loaded in current Revit project
        /// it is declared as static only because of PropertyGrid
        /// </summary>
        private static Dictionary<string, FamilySymbol> m_beamTypes = new Dictionary<string, FamilySymbol>();

        /// <summary>
        /// properties of beam system
        /// </summary>
        private BeamSystemParam m_param;

        /// <summary>
        /// buffer of ExternalCommandData
        /// </summary>
        private ExternalCommandData m_commandData;

        /// <summary>
        /// the lines compose the profile of beam system
        /// </summary>
        private List<Line> m_lines = new List<Line>();

        /// <summary>
        /// a number of beams that intersect end to end
        /// so that form a profile used as beam system's profile
        /// </summary>
        private List<FamilyInstance> m_beams = new List<FamilyInstance>();

        /// <summary>
        /// properties of beam system
        /// </summary>
        public BeamSystemParam Param
        {
            get
            {
                return m_param; 
            }
        }

        /// <summary>
        /// lines form the profile of beam system
        /// </summary>
        public ReadOnlyCollection<Line> Lines
        {
            get
            {
                return new ReadOnlyCollection<Line>(m_lines);
            }
        }

        /// <summary>
        /// buffer of ExternalCommandData
        /// </summary>
        public ExternalCommandData CommandData
        {
            get
            {
                return m_commandData;
            }
        }

        /// <summary>
        /// the data used to show in UI is updated
        /// </summary>
        public event EventHandler ParamsUpdated;

        /// <summary>
        /// constructor
        /// if precondition in current Revit project isn't enough,
        /// ErrorMessageException will be throw out
        /// </summary>
        /// <param name="commandData">data from Revit</param>
        public BeamSystemData(ExternalCommandData commandData)
        {
            // initialize members
            m_commandData = commandData;
            PrepareData();
            InitializeProfile(m_beams);

            m_param                      = BeamSystemParam.CreateInstance(LayoutMethod.ClearSpacing);
            List<FamilySymbol> beamTypes = new List<FamilySymbol>(m_beamTypes.Values);
            m_param.BeamType             = beamTypes[0];
            m_param.LayoutRuleChanged   += new LayoutRuleChangedHandler(LayoutRuleChanged);
        }

        /// <summary>
        /// change the direction to the next line in the profile
        /// </summary>
        public void ChangeProfileDirection()
        {
            Line tmp = m_lines[0];
            m_lines.RemoveAt(0);
            m_lines.Add(tmp);
        }

        /// <summary>
        /// all beam types loaded in current Revit project
        /// it is declared as static only because of PropertyGrid
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, FamilySymbol> GetBeamTypes()
        {
            Dictionary<string, FamilySymbol> beamTypes = new Dictionary<string, FamilySymbol>(m_beamTypes);
            return beamTypes;
        }

        /// <summary>
        /// initialize members using data from current Revit project
        /// </summary>
        private void PrepareData()
        {
            UIDocument doc = m_commandData.Application.ActiveUIDocument;
            m_beamTypes.Clear();

            // iterate all selected beams
            foreach (ElementId elementId in doc.Selection.GetElementIds())
            {
               object obj = doc.Document.GetElement(elementId);
                FamilyInstance beam = obj as FamilyInstance;
                if (null == beam)
                {
                    continue;
                }

                // add beam to lists according to category name
                string categoryName = beam.Category.Name;
                if ("Structural Framing" == categoryName
                    && beam.StructuralType == StructuralType.Beam)
                {
                    m_beams.Add(beam);
                }
            }

            //iterate all beam types
            FilteredElementIterator itor = new FilteredElementCollector(doc.Document).OfClass(typeof(Family)).GetElementIterator();
            itor.Reset();
            while (itor.MoveNext())
            {
                // get Family to get FamilySymbols
                Family aFamily = itor.Current as Family;
                if (null == aFamily)
                {
                    continue;
                }

                foreach (ElementId symbolId in aFamily.GetFamilySymbolIds())
                {
                   FamilySymbol symbol = doc.Document.GetElement(symbolId) as FamilySymbol;
                    if (null == symbol.Category)
                    {
                        continue;
                    }

                    // add symbols to lists according to category name
                    string categoryName = symbol.Category.Name;
                    if ("Structural Framing" == categoryName)
                    {
                        m_beamTypes.Add(symbol.Family.Name + ":" + symbol.Name, symbol);
                    }
                }
            }

            if (m_beams.Count == 0)
            {
                throw new ErrorMessageException("Please select beams.");
            }

            if (m_beamTypes.Count == 0)
            {
                throw new ErrorMessageException("There is no Beam families loaded in current project.");
            }
        }

        /// <summary>
        /// retrieve the profiles using the selected beams
        /// ErrorMessageException will be thrown out if beams can't make a closed profile
        /// </summary>
        /// <param name="beams">beams which may form a closed profile</param>
        private void InitializeProfile(List<FamilyInstance> beams)
        {
            // retrieve collection of lines in beams
            List<Line> lines = new List<Line>();
            foreach (FamilyInstance beam in beams)
            {
                LocationCurve locationLine = beam.Location as LocationCurve;
                Line line = locationLine.Curve as Line;
            if (null == line)
            {
               throw new ErrorMessageException("Please don't select any arc beam.");
            }
                lines.Add(line);
            }

            // lines should in the same horizontal plane
            if (!GeometryUtil.InSameHorizontalPlane(lines))
            {
                throw new ErrorMessageException("The selected beams can't form a horizontal profile.");
            }

            // sorted lines so that all lines are intersect end to end
            m_lines = GeometryUtil.SortLines(lines);
            // lines can't make a closed profile
            if (null == m_lines)
            {
                throw new ErrorMessageException("The selected beams can't form a closed profile.");
            }
        }

        /// <summary>
        /// layout rule of beam system has changed
        /// </summary>
        /// <param name="layoutMethod">changed method</param>
        private void LayoutRuleChanged(ref LayoutMethod layoutMethod)
        {
            // create BeamSystemParams instance according to changed LayoutMethod
            m_param = m_param.CloneInstance(layoutMethod);

            // raise DataUpdated event
            OnParamsUpdated(new EventArgs());

            // rebind delegate
            m_param.LayoutRuleChanged += new LayoutRuleChangedHandler(LayoutRuleChanged);
        }

        /// <summary>
        /// the data used to show in UI is updated
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnParamsUpdated(EventArgs e)
        {
            if (null != ParamsUpdated)
            {
                ParamsUpdated(this, e);
            }
        }
    }
}
