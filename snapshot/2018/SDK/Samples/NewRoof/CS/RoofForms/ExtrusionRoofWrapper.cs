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
using System.ComponentModel;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.NewRoof.RoofForms.CS
{
    
    /// <summary>
    /// The ExtrusionRoofWrapper class is use to edit a extrusion roof in a PropertyGrid.
    /// It contains a extrusion roof.
    /// </summary>
    public class ExtrusionRoofWrapper
    {
        // To store the extrusion roof which will be edited in a PropertyGrid.
        private ExtrusionRoof m_roof;
      
        /// <summary>
        /// The construct of the ExtrusionRoofWrapper class.
        /// </summary>
        /// <param name="roof">The extrusion roof which will be edited in a PropertyGrid.</param>
        public ExtrusionRoofWrapper(ExtrusionRoof roof)
        {
            m_roof = roof;
        }

        #region The properties will be shown in the PropertyGrid
        /// <summary>
        /// The reference plane of the extrusion roof.
        /// </summary>
        [Category("Constrains")]
        [Description("The reference plane of the extrusion roof.")]
        public String WorkPlane
        {
            get
            {
                Parameter para = m_roof.get_Parameter(BuiltInParameter.SKETCH_PLANE_PARAM);
                return para.AsString();
            }
        }

        /// <summary>
        /// The extrusion start point of the extrusion roof.
        /// </summary>
        [Category("Constrains")]
        [DisplayName("Extrusion Start")]
        [Description("The extrusion of a roof can extend in either direction along the reference plane. If the extrusion extends away from the plane, the start and end points are positive values. If the extrusion extends toward the plane, the start and end points are negative.")]
        public String ExtrusionStart
        {
            get
            {
                Parameter para = m_roof.get_Parameter(BuiltInParameter.EXTRUSION_START_PARAM);
                return para.AsValueString();
            }
            set
            {
                Parameter para = m_roof.get_Parameter(BuiltInParameter.EXTRUSION_START_PARAM);
                if (para.SetValueString(value) == false)
                {
                    throw new Exception("Invalid Input");
                }
            }
        }

        /// <summary>
        /// The extrusion end point of the extrusion roof.
        /// </summary>
        [Category("Constrains")]
        [DisplayName("Extrusion End")]
        [Description("The extrusion of a roof can extend in either direction along the reference plane. If the extrusion extends away from the plane, the start and end points are positive values. If the extrusion extends toward the plane, the start and end points are negative.")]
        public String ExtrusionEnd
        {
            get
            {
                Parameter para = m_roof.get_Parameter(BuiltInParameter.EXTRUSION_END_PARAM);
                return para.AsValueString();
            }
            set
            {
                Parameter para = m_roof.get_Parameter(BuiltInParameter.EXTRUSION_END_PARAM);
                if (para.SetValueString(value) == false)
                {
                    throw new Exception("Invalid Input");
                }
            }
        }

        /// <summary>
        /// The reference level of the extrusion roof.
        /// </summary>
        [TypeConverterAttribute(typeof(LevelConverter)), Category("Constrains")]
        [DisplayName("Reference Level")]
        [Description("The reference level of the extrusion roof.")]
        public Level ReferenceLevel
        {
            get
            {
                Parameter para = m_roof.get_Parameter(BuiltInParameter.ROOF_CONSTRAINT_LEVEL_PARAM);
                return LevelConverter.GetLevelByID(para.AsElementId().IntegerValue);
            }
            set
            {
                // update reference level
                Parameter para = m_roof.get_Parameter(BuiltInParameter.ROOF_CONSTRAINT_LEVEL_PARAM);
                Autodesk.Revit.DB.ElementId id = new Autodesk.Revit.DB.ElementId(value.Id.IntegerValue);
                para.Set(id);
            }
        }

        /// <summary>
        /// The offset from the reference level of the extrusion roof.
        /// </summary>
        [Category("Constrains")]
        [DisplayName("Level Offset")]
        [Description("The offset from the reference level.")]
        public String LevelOffset
        {
            get
            {
                Parameter para = m_roof.get_Parameter(BuiltInParameter.ROOF_CONSTRAINT_OFFSET_PARAM);
                return para.AsValueString();
            }
            set
            {
                Parameter para = m_roof.get_Parameter(BuiltInParameter.ROOF_CONSTRAINT_OFFSET_PARAM);
                if (para.SetValueString(value) == false)
                {
                    throw new Exception("Invalid Input");
                }
            }
        }
        #endregion        
    }
}
