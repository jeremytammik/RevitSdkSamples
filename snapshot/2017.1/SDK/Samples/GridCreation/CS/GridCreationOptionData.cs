//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.GridCreation.CS
{
    /// <summary>
    /// Data class which stores the information of the way to create grids
    /// </summary>
    public class GridCreationOptionData
    {
        #region Fields
        // The way to create grids
        private CreateMode m_createGridsMode;
        // If lines/arcs have been selected
        private bool m_hasSelectedLinesOrArcs;
        #endregion

        #region Properties
        /// <summary>
        /// Creating mode
        /// </summary>
        public CreateMode CreateGridsMode
        {
            get
            {
                return m_createGridsMode;
            }
            set 
            { 
                m_createGridsMode = value; 
            }
        }

        /// <summary>
        /// State whether lines/arcs have been selected
        /// </summary>
        public bool HasSelectedLinesOrArcs
        {
            get
            {
                return m_hasSelectedLinesOrArcs;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hasSelectedLinesOrArcs">Whether lines or arcs have been selected</param>
        public GridCreationOptionData(bool hasSelectedLinesOrArcs)
        {
            m_hasSelectedLinesOrArcs = hasSelectedLinesOrArcs;
        }
        #endregion
    }
}
