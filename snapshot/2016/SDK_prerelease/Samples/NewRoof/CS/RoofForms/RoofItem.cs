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

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.NewRoof.RoofForms.CS
{
    /// <summary>
    /// The RoofItem is used to display a roof info in the ListView as a ListViewItem.
    /// </summary>
    class RoofItem : ListViewItem
    {
        // To store the roof which the RoofItem stands for.
        Autodesk.Revit.DB.RoofBase m_roof;

        /// <summary>
        /// The construct of the RoofItem class.
        /// </summary>
        /// <param name="roof"></param>
        public RoofItem(Autodesk.Revit.DB.RoofBase roof) : base(roof.Id.IntegerValue.ToString())
        {
            m_roof = roof;
            this.SubItems.Add(roof.Name);

            if (m_roof is Autodesk.Revit.DB.FootPrintRoof)
            {
                Parameter para = roof.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.ROOF_BASE_LEVEL_PARAM);
                this.SubItems.Add(LevelConverter.GetLevelByID(para.AsElementId().IntegerValue).Name);
            }
            else if (m_roof is Autodesk.Revit.DB.ExtrusionRoof)
            {
                Parameter para = roof.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.ROOF_CONSTRAINT_LEVEL_PARAM);
                this.SubItems.Add(LevelConverter.GetLevelByID(para.AsElementId().IntegerValue).Name);
            }

            this.SubItems.Add(roof.RoofType.Name);
        }

        /// <summary>
        /// When the roof was edited, then the data of the RoofItem should be updated synchronously.
        /// </summary>
        /// <returns>Update successfully return true, otherwise return false.</returns>
        public bool  Update()
        {
            try
            {
                this.SubItems[1].Text = m_roof.Name;

                if (m_roof is Autodesk.Revit.DB.FootPrintRoof)
                {
                    Parameter para = m_roof.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.ROOF_BASE_LEVEL_PARAM);
                    this.SubItems[2].Text = LevelConverter.GetLevelByID(para.AsElementId().IntegerValue).Name;
                }
                else if (m_roof is Autodesk.Revit.DB.ExtrusionRoof)
                {
                    Parameter para = m_roof.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.ROOF_CONSTRAINT_LEVEL_PARAM);
                    this.SubItems[2].Text = LevelConverter.GetLevelByID(para.AsElementId().IntegerValue).Name;
                }

                this.SubItems[3].Text = m_roof.RoofType.Name;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get the roof which the RoofItem stands for.
        /// </summary>
        public Autodesk.Revit.DB.RoofBase Roof
        {
            get
            {
                return m_roof;
            }
        }
    }
}
