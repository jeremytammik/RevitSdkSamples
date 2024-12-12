//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace REX.ContentGeneratorWPF.Resources
{
    /// <summary>
    /// Represents the control which allows user to observe what kind of shape he has selected. There are two
    /// representations presented: database and parametric.
    /// </summary>
    public partial class ContentGeneratorReadControl : REX.Common.REXExtensionControl
    {
        /// <summary>
        /// Gets the main extension.
        /// </summary>
        /// <value>The main extension.</value>
        internal Extension ThisMainExtension
        {
            get
            {
                return (Extension)ThisExtension;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentGeneratorReadControl"/> class.
        /// </summary>
        public ContentGeneratorReadControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentGeneratorReadControl"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public ContentGeneratorReadControl(REX.Common.REXExtension extension)
            : base(extension)
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes the dialog with current settings.
        /// </summary>
        public void SetDialog()
        {
            SetParametricDescription();
            SetDatabasesDescription();
        }
        /// <summary>
        /// Initializes the parametric description.
        /// </summary>
        private void SetParametricDescription()
        {
            SetParametricViewer();
            SetParametricProperties();
        }
        /// <summary>
        /// Initializes the databases description.
        /// </summary>
        private void SetDatabasesDescription()
        {
            SetDatabasesProperties();
            SetDatabasesList();
        }
        /// <summary>
        /// Draws the parametric section on the viewer.
        /// </summary>
        private void SetParametricViewer()
        {
            if (ThisMainExtension.Data.ParametricSection != null)
            {
                REX.ContentGenerator.Geometry.Contour_Section section = ThisMainExtension.Data.ParametricSection.Parameters.GetContour(true);

                if (section.Shape.Count > 0)
                {
                    List<REX.Common.Geometry.REXxyz> points = section.Shape[0].GetPointList(true);
                    viewer.DrawPolygon(points);
                }
            }
        }
        /// <summary>
        /// Fill the parametric property control with data.
        /// </summary>
        private void SetParametricProperties()
        {
            if (ThisMainExtension.Data.ParametricSection != null)
            {
                REX.ContentGenerator.Families.REXFamilyType_ParamSection section = ThisMainExtension.Data.ParametricSection;

                parametricProperties.ClearProperties();

                parametricProperties.AddProperty("Certified",section.Certified.ToString());
                parametricProperties.AddProperty("Shape", section.Parameters.SectionType.ToString());
                parametricProperties.AddProperty("Material",section.Material.ToString());
                parametricProperties.AddProperty("Tapered",section.Parameters.Tapered.ToString());

                string angle = ThisMainExtension.System.Units.DisplayTextFromBase(section.Parameters.Angle, Autodesk.REX.Framework.EUnitType.Dimensions_Angle,true);
                parametricProperties.AddProperty("Angle",angle);

                string add2name = "";

                if (section.Parameters.Tapered)
                {
                    parametricProperties.AddCategory("Dimensions on the start");
                    add2name = "Start ";
                }
                else
                {
                    parametricProperties.AddCategory("Dimensions");
                }

                foreach (System.Reflection.PropertyInfo property in section.Parameters.Dimensions)
                {
                    object valObj = property.GetValue(section.Parameters.Dimensions, null);
                    double val = (valObj != null)?Convert.ToDouble(valObj):0;
                    string valStr = ThisMainExtension.System.Units.DisplayTextFromBase(val, Autodesk.REX.Framework.EUnitType.Dimensions_SectionDim,true);
                    parametricProperties.AddProperty(add2name + property.Name, valStr);
                }

                if (section.Parameters.Tapered)
                {
                    parametricProperties.AddCategory("Dimensions on the end");
                    add2name = "End ";
                    foreach (System.Reflection.PropertyInfo property in section.Parameters.DimensionsEnd)
                    {
                        object valObj = property.GetValue(section.Parameters.DimensionsEnd, null);
                        double val = (valObj != null) ? Convert.ToDouble(valObj) : 0;
                        string valStr = ThisMainExtension.System.Units.DisplayTextFromBase(val, Autodesk.REX.Framework.EUnitType.Dimensions_SectionDim, true);
                        parametricProperties.AddProperty(add2name + property.Name, valStr);
                    }
                }
            }
        }
        /// <summary>
        /// Fill the parametric property control with data.
        /// </summary>
        private void SetDatabasesProperties()
        {
            if (ThisMainExtension.Data.DatabaseSection != null)
            {
                REX.ContentGenerator.Families.REXFamilyType_DBSection section = ThisMainExtension.Data.DatabaseSection;

                databasesProperties.ClearProperties();

                databasesProperties.AddProperty("Certified", section.Certified.ToString());
                databasesProperties.AddProperty("Material", section.Material.ToString());
                databasesProperties.AddProperty("Database", section.Parameters.DBName);
                databasesProperties.AddProperty("Alias name", section.Parameters.Description.NAME_REVIT);               
            }
        }

        /// <summary>
        /// Fills the databases list.
        /// </summary>
        private void SetDatabasesList()
        {
            if (ThisMainExtension.Data.DatabaseSection != null)
            {
                foreach (REX.ContentGenerator.Families.REXDBDescription dbdesc in ThisMainExtension.Data.DatabaseRecords)
                {
                    dabasesListBox.Items.Add(dbdesc.DBName);
                }
            }
        }
    }
}
