//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// Form used to display project information
    /// </summary>
    public partial class ProjectInfoForm : System.Windows.Forms.Form
    {
        #region Fields
        /// <summary>
        /// Wrapper for ProjectInfo
        /// </summary>
        ProjectInfoWrapper m_projectInfoWrapper = null; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize component
        /// </summary>
        public ProjectInfoForm()
        {
            InitializeComponent();
        } 

        /// <summary>
        /// Initialize PropertyGrid
        /// </summary>
        /// <param name="projectInfoWrapper">ProjectInfo wrapper</param>
        public ProjectInfoForm(ProjectInfoWrapper projectInfoWrapper)
            :this()
        {
            m_projectInfoWrapper = projectInfoWrapper;

            // Initialize propertyGrid with CustomDescriptor
            propertyGrid1.SelectedObject = new WrapperCustomDescriptor(m_projectInfoWrapper);
        }
        #endregion
    }
}
