//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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

using Autodesk.Revit;

namespace Revit.SDK.Samples.ElementsFilter.CS
{
    /// <summary>
    /// Process form displayed during the executing of filters.
    /// </summary>
    public partial class ProcessForm : Form
    {
        private DataTable m_data;
        private ICollection<Element> m_elements;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="data"></param>
        public ProcessForm(ICollection<Element> elements, ref DataTable data)
        {
            InitializeComponent();
            m_elements = elements;
            m_data = data;
        }

        /// <summary>
        /// Populating elements to data table.
        /// </summary>
        public void Progress()
        {
            progressBar.Minimum = 1;
            progressBar.Maximum = m_elements.Count;
            progressBar.Value = 1;
            progressBar.Step = 1;

            foreach (Element element in m_elements)
            {
                DataRow row;
                row = m_data.NewRow();
                row[ResourceMgr.GetString("Id")] = element.Id.Value;
                row[ResourceMgr.GetString("Name")] = element.Name;
                row[ResourceMgr.GetString("Type")] = element.GetType().ToString();
                if (null != element.Category && null != element.Category.Name)
                {
                    row[ResourceMgr.GetString("Category")] = element.Category.Name;
                }
                m_data.Rows.Add(row);

                progressBar.PerformStep();
            }

            this.Close();
        }

        private void ProcessForm_Activated(object sender, EventArgs e)
        {
            Progress();
        }        
    }
}