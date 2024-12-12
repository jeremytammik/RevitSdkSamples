//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
    /// The SpaceManager class is used to manage the Spaces elements in the current document.
    /// </summary>
    class SpaceManager
    {
        ExternalCommandData m_commandData;
        Dictionary<int, List<Space>> m_spaceDictionary;

        /// <summary>
        /// The constructor of SpaceManager class.
        /// </summary>
        /// <param name="data">The ExternalCommandData</param>
        /// <param name="spaceData">The spaceData contains all the Space elements in different level.</param>
        public SpaceManager(ExternalCommandData data, Dictionary<int,List<Space>> spaceData)
        {
            m_commandData = data;
            m_spaceDictionary = spaceData;
        }

        /// <summary>
        /// Get the Spaces elements in a specified level.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>Return a space list</returns>
        public List<Space> GetSpaces(Level level)
        {
            return m_spaceDictionary[level.Id.IntegerValue];
        }

        /// <summary>
        /// Create the space for each closed wall loop or closed space separation in the active view.
        /// </summary>
        /// <param name="level">The level in which the spaces is to exist.</param>
        /// <param name="phase">The phase in which the spaces is to exist.</param>
        public void CreateSpaces(Level level, Phase phase)
        {
            try
            {
                ICollection<ElementId> elements = m_commandData.Application.ActiveUIDocument.Document.Create.NewSpaces2(level, phase, this.m_commandData.Application.ActiveUIDocument.Document.ActiveView);
                foreach (ElementId elem in elements)
                {
                    Space space = m_commandData.Application.ActiveUIDocument.Document.GetElement(elem) as Space;
                    if (space != null)
                    {
                        m_spaceDictionary[level.Id.IntegerValue].Add(space);
                    }
                }
                if (elements == null || elements.Count == 0)
                {
                    Autodesk.Revit.UI.TaskDialog.Show("Revit", "There is no enclosed loop in " + level.Name);
                }
                 
            }
            catch (Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", ex.Message);
            }
        }
    }
}
