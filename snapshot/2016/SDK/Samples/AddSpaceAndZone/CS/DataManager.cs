//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.Text;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Mechanical;

namespace Revit.SDK.Samples.AddSpaceAndZone.CS
{
    /// <summary>
    /// The DataManager Class is used to obtain, create or edit the Space elements and Zone elements.
    /// </summary>
    public class DataManager
    {
        ExternalCommandData m_commandData;
        List<Level>  m_levels;
        SpaceManager m_spaceManager;
        ZoneManager m_zoneManager;
        Level m_currentLevel;
        Phase m_defaultPhase;

        /// <summary>
        /// The constructor of DataManager class.
        /// </summary>
        /// <param name="commandData">The ExternalCommandData</param>
        public DataManager(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_levels = new List<Level>();
            Initialize();
            m_currentLevel = m_levels[0];
            Parameter para = commandData.Application.ActiveUIDocument.Document.ActiveView.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.VIEW_PHASE);
            Autodesk.Revit.DB.ElementId phaseId = para.AsElementId();
            m_defaultPhase = commandData.Application.ActiveUIDocument.Document.GetElement(phaseId) as Phase;
        }

        /// <summary>
        /// Initialize the data member, obtain the Space and Zone elements.
        /// </summary>
        private void Initialize()
        {
            Dictionary<int, List<Space>> spaceDictionary = new Dictionary<int, List<Space>>();
            Dictionary<int, List<Zone>> zoneDictionary = new Dictionary<int, List<Zone>>();

            Document activeDoc = m_commandData.Application.ActiveUIDocument.Document;

            FilteredElementIterator levelsIterator = (new FilteredElementCollector(activeDoc)).OfClass(typeof(Level)).GetElementIterator();
            FilteredElementIterator spacesIterator =(new FilteredElementCollector(activeDoc)).WherePasses(new SpaceFilter()).GetElementIterator();
            FilteredElementIterator zonesIterator = (new FilteredElementCollector(activeDoc)).OfClass(typeof(Zone)).GetElementIterator();
          
            levelsIterator.Reset();
            while (levelsIterator.MoveNext())
            {
                Level level = levelsIterator.Current as Level;
                if (level != null)
                {
                    m_levels.Add(level);
                    spaceDictionary.Add(level.Id.IntegerValue, new List<Space>());
                    zoneDictionary.Add(level.Id.IntegerValue, new List<Zone>());
                }
            }

            spacesIterator.Reset();
            while (spacesIterator.MoveNext())
            {
                Space space = spacesIterator.Current as Space;
                if (space != null)
                {
                    spaceDictionary[space.LevelId.IntegerValue].Add(space);
                }
            }

            zonesIterator.Reset();
            while (zonesIterator.MoveNext())
            {
                Zone zone = zonesIterator.Current as Zone;
                if (zone != null && activeDoc.GetElement(zone.LevelId) != null)
                {
                    zoneDictionary[zone.LevelId.IntegerValue].Add(zone);
                }
            }

            m_spaceManager = new SpaceManager(m_commandData, spaceDictionary);
            m_zoneManager = new ZoneManager(m_commandData, zoneDictionary);
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
        /// Create a Zone element.
        /// </summary>
        public void CreateZone()
        {
            if (m_defaultPhase == null)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", "The phase of the active view is null, you can't create zone in a null phase");
                return;
            }
            try
            {
                this.m_zoneManager.CreateZone(m_currentLevel, m_defaultPhase);
            }
            catch (Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", ex.Message);
            }         
        }

        /// <summary>
        /// Create some spaces.
        /// </summary>
        public void CreateSpaces()
        {           
            if (m_defaultPhase == null)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", "The phase of the active view is null, you can't create spaces in a null phase");
                return;
            }

            try
            {
                if (m_commandData.Application.ActiveUIDocument.Document.ActiveView.ViewType == Autodesk.Revit.DB.ViewType.FloorPlan)
                {
                    m_spaceManager.CreateSpaces(m_currentLevel, m_defaultPhase);
                }
                else
                {
                    Autodesk.Revit.UI.TaskDialog.Show("Revit", "You can not create spaces in this plan view");
                }
            }
            catch (Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", ex.Message);
            }               
        }

        /// <summary>
        /// Get the Space elements.
        /// </summary>
        /// <returns>A space list in current level.</returns>
        public List<Space> GetSpaces()
        {
            return m_spaceManager.GetSpaces(m_currentLevel);
        }

        /// <summary>
        /// Get the Zone elements.
        /// </summary>
        /// <returns>A Zone list in current level.</returns>
        public List<Zone> GetZones()
        {
            return m_zoneManager.GetZones(m_currentLevel);
        }

        /// <summary>
        /// Update the current level.
        /// </summary>
        /// <param name="level"></param>
        public void Update(Level level)
        {
            this.m_currentLevel = level;
        }
    }
}
