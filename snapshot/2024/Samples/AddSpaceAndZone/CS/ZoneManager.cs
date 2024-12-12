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
using System.Text;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Mechanical;

namespace Revit.SDK.Samples.AddSpaceAndZone.CS
{
    /// <summary>
    /// The ZoneManager class is used to manage the Zone elements in the current document.
    /// </summary>
    class ZoneManager
    {
        ExternalCommandData m_commandData;
        Dictionary<ElementId, List<Zone>> m_zoneDictionary;
        Level m_currentLevel;
        Zone m_currentZone;

        /// <summary>
        /// The constructor of ZoneManager class.
        /// </summary>
        /// <param name="commandData">The ExternalCommandData</param>
        /// <param name="zoneData">The spaceData contains all the Zone elements in different level.</param>
        public ZoneManager(ExternalCommandData commandData, Dictionary<ElementId, List<Zone>>  zoneData)
        {
            m_commandData = commandData;
            m_zoneDictionary = zoneData;
        }

        /// <summary>
        /// Create a zone in a specified level and phase.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="phase"></param>
        public void CreateZone(Level level, Phase phase)
        {
            Zone zone = m_commandData.Application.ActiveUIDocument.Document.Create.NewZone(level, phase);
            if (zone != null)
            {
                this.m_zoneDictionary[level.Id].Add(zone);
            }
        }

        /// <summary>
        /// Add some spaces to current Zone.
        /// </summary>
        /// <param name="spaces"></param>
        public void AddSpaces(SpaceSet spaces)
        {
            m_currentZone.AddSpaces(spaces);
        }

        /// <summary>
        /// Remove some spaces to current Zone.
        /// </summary>
        /// <param name="spaces"></param>
        public void RemoveSpaces(SpaceSet spaces)
        {
            m_currentZone.RemoveSpaces(spaces);
        }
      
        /// <summary>
        /// Get the Zone elements in a specified level.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>Return a zone list</returns>
        public List<Zone> GetZones(Level level)
        {
            m_currentLevel = level;
            return m_zoneDictionary[level.Id];
        }

        /// <summary>
        /// Get/Set the Current Zone element.
        /// </summary>
        public Zone CurrentZone
        {
            get
            {
                return CurrentZone;
            }
            set
            {
                m_currentZone = value;
            }
        }
    }
}
