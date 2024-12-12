//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using MacroCSharpSamples;
using MacroSamples_RVT;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
	/// <summary>
	/// Get some properties of a slab , such as Level, Type name, Span direction,
	/// Material name, Thickness, and Young Modulus for the slab's Material.
	/// </summary>
    public class SampleProjectInfo
    {
 //       #region Class ctor implemetation
        /// <summary>
        /// Ctor without parameter is not allowed
        /// </summary>
        private SampleProjectInfo()
        {
            // no codes
        }

        /// <summary>
        /// Default constructor of StructuralLayerFunction
        /// </summary>
        public SampleProjectInfo(ThisApplication App)
        {
            // Init for varialbes
            // this application handler
            m_app = App;
            // initialize global information

            RevitStartInfo.RevitApp = m_app.ActiveUIDocument.Application.Application;
            RevitStartInfo.RevitDoc = m_app.ActiveUIDocument.Document;
            RevitStartInfo.RevitProduct = m_app.ActiveUIDocument.Application.Application.Product;
        }

        /// <summary>
        /// Run sample Rooms
        /// </summary>
        public void Run()
        {
            try
            {
                //Get ProjectInfo object from current project
                if(m_app == null)
                    return;
                Autodesk.Revit.DB.ProjectInfo projectInfo = m_app.ActiveUIDocument.Document.ProjectInformation;
                if (null != projectInfo)
                {
                    ProjectInfoForm mainForm = new ProjectInfoForm(new ProjectInfoWrapper(projectInfo));
                    mainForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Class member variable
        ThisApplication? m_app;
        #endregion 
    }
}
