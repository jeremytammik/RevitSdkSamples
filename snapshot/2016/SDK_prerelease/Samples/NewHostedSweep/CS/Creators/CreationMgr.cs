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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// This is the manager of all hosted sweep creators, it contains all the creators
    /// and each creator can create the corresponding hosted sweep. Its "Execute" 
    /// method will show the main dialog for user to create hosted sweeps.
    /// </summary>
    public class CreationMgr
    {
        /// <summary>
        /// Revit active document.
        /// </summary>
        private Autodesk.Revit.UI.UIDocument m_rvtDoc;

        /// <summary>
        /// Creator for Fascia.
        /// </summary>
        private FasciaCreator m_fasciaCreator;

        /// <summary>
        /// Creator for Gutter.
        /// </summary>
        private GutterCreator m_gutterCreator;

        /// <summary>
        /// Creator for SlabEdge.
        /// </summary>
        private SlabEdgeCreator m_slabEdgeCreator;

        /// <summary>
        /// Gets Fascia creator.
        /// </summary>
        public FasciaCreator FasciaCreator
        {
            get 
            {
                if(m_fasciaCreator == null)
                {
                    m_fasciaCreator = new FasciaCreator(m_rvtDoc);
                }
                return m_fasciaCreator; 
            }            
        }        

        /// <summary>
        /// Gets Gutter creator.
        /// </summary>
        public GutterCreator GutterCreator
        {
            get 
            {
                if (m_gutterCreator == null)
                {
                    m_gutterCreator = new GutterCreator(m_rvtDoc);
                }                     
                return m_gutterCreator; 
            }            
        }        

        /// <summary>
        /// Gets SlabEdge creator.
        /// </summary>
        public SlabEdgeCreator SlabEdgeCreator
        {
            get 
            {
                if(m_slabEdgeCreator == null)
                {
                    m_slabEdgeCreator = new SlabEdgeCreator(m_rvtDoc);
                }
                return m_slabEdgeCreator;
            }            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rvtDoc">Revit active document</param>
        public CreationMgr(Autodesk.Revit.UI.UIDocument rvtDoc)
        {
            m_rvtDoc = rvtDoc;
        }

        /// <summary>
        /// Show the main form, it is the UI entry.
        /// </summary>
        public void Execute()
        {
            using(MainForm mainForm = new MainForm(this))
            {
                mainForm.ShowDialog();
            }
        }
    }
}
