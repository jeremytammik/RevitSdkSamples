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
using System.Text;

using Autodesk.REX.Framework;
using REX.Common;
using Autodesk.Revit.DB;
using REX.ContentGenerator.Families;

namespace REX.ContentGeneratorWPF.Main.Revit
{
    internal class ExtensionRevit : REX.Common.REXExtensionProduct
    {
        #region properties
        /// <summary>
        /// The instance of Revit family converter.
        /// </summary>
        REX.ContentGenerator.Converters.RVTFamilyConverter RVTConverter;
        /// <summary>
        /// Get the main extension.
        /// </summary>
        /// <value>The main extension.</value>
        internal Extension ThisMainExtension
        {
            get
            {
                return (Extension)ThisExtension;
            }
        }
        #endregion

        #region<constructor>
        public ExtensionRevit(REX.Common.REXExtension Ext)
            : base(Ext)
        {
            RVTConverter = new REX.ContentGenerator.Converters.RVTFamilyConverter(ThisExtension.Revit.ActiveDocument, true);
        }
        #endregion

        #region IREXExtension methods - filled
        public override void OnCreate()
        {
            string error;

            if (ReadSelectedElementFromRevit(out error))
            {
                ReadParamAndDBDescription();
            }
            else
            {
                ThisExtension.System.SystemBase.Errors.AddError("Error2", error, null);
            }
        }

        public override void OnSetData()
        {
            if (ThisMainExtension.Data.NewSection == null)
            {
                ThisMainExtension.System.SystemBase.Errors.AddError("Error", "The section was not defined.", null);
            }
        }

        public override void OnRun()
        {
            ThisMainExtension.Progress.Steps = 2;
            ThisMainExtension.Progress.Position = 1;
            ThisMainExtension.Progress.Header = "Element creation";
            ThisMainExtension.Progress.Text = "...";

            ThisMainExtension.Progress.Show(ThisMainExtension.GetWindowForParent());

            CreateNewFamilySymbol();

            if (ThisMainExtension.Data.NewFamilySymbol == null)
            {
                ThisExtension.System.SystemBase.Errors.AddError("Error", "Creation of the new family failed.", null);
            }
            else
            {
                if (!ApplyFamilySymbolToSelectedElement())
                {
                    ThisExtension.System.SystemBase.Errors.AddError("Error", "Setting the new family to the selected element failed.", null);
                }
                else
                {
                    ThisExtension.System.SystemBase.Errors.AddError("Error2", "The new family was applied to the selected element", null, REXErrorLevel.Info);
                }
            }

            RVTConverter.Close();

            ThisMainExtension.Progress.Hide();
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Reads the current selection of Revit.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>True if reading succeed; otherwise, false.</returns>
        private bool ReadSelectedElementFromRevit(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (ThisExtension.Revit.ActiveUIDocument.Selection.Elements.Size == 0)
            {
                errorMessage = "The selection is empty";
                return false;
            }

            if (ThisExtension.Revit.ActiveUIDocument.Selection.Elements.Size > 1)
            {
                errorMessage = "The selection contains more than one element";
                return false;
            }

            ElementSetIterator iterator = ThisExtension.Revit.ActiveUIDocument.Selection.Elements.ForwardIterator();
            iterator.Reset();
            iterator.MoveNext();
            FamilyInstance familyInstance = iterator.Current as FamilyInstance;

            if (familyInstance == null)
            {
                errorMessage = "Selected element is not a FamilyInstance";
                return false;
            }

            if (familyInstance.StructuralType == Autodesk.Revit.DB.Structure.StructuralType.Beam ||
                familyInstance.StructuralType == Autodesk.Revit.DB.Structure.StructuralType.Column ||
                familyInstance.StructuralType == Autodesk.Revit.DB.Structure.StructuralType.Brace)
            {
                ThisMainExtension.Data.SelectedElement = familyInstance;
                FillPropertiesOfSelectedElement();
                FillGeometryOfSelectedElement();
                return true;
            }
            else
            {
                errorMessage = "Selected element is not a beam nor a column";
                return false;
            }
        }
        /// <summary>
        /// Fills the properties based on the selected element.
        /// </summary>
        private void FillPropertiesOfSelectedElement()
        {
            List<PropertyItem> properties = new List<PropertyItem>();

            properties.Add(new PropertyItem() { Name = "Name", Value = ThisMainExtension.Data.SelectedElement.Name });
            properties.Add(new PropertyItem() { Name = "Id", Value = ThisMainExtension.Data.SelectedElement.Id.IntegerValue.ToString() });
            properties.Add(new PropertyItem() { Name = "Structural usage", Value = ThisMainExtension.Data.SelectedElement.StructuralUsage.ToString() });
            properties.Add(new PropertyItem() { Name = "Structural type", Value = ThisMainExtension.Data.SelectedElement.StructuralType.ToString() });
            properties.Add(new PropertyItem() { Name = "Structural material type", Value = ThisMainExtension.Data.SelectedElement.StructuralMaterialType.ToString() });

            foreach (Parameter param in ThisMainExtension.Data.SelectedElement.Parameters)
            {
                string valueString = "";

                if (param.StorageType == StorageType.Double)
                    valueString = param.AsValueString();
                else if (param.StorageType == StorageType.String)
                    valueString = param.AsString();
                else if (param.StorageType == StorageType.ElementId)
                {
                    ElementId id = param.AsElementId();
                    Element elem = ThisMainExtension.Revit.ActiveDocument.GetElement(id);
                    if (elem != null)
                        valueString = elem.Name;
                }

                if (!string.IsNullOrEmpty(valueString))
                    properties.Add(new PropertyItem() { Name = param.Definition.Name, Value = valueString });
            }

            ThisMainExtension.Data.SelectedElementProperties = properties;
        }
        /// <summary>
        /// Builds the geometry based on the selected element.
        /// </summary>
        private void FillGeometryOfSelectedElement()
        {
            List<Triangle> triangles = new List<Triangle>();

            Options opt = ThisMainExtension.Revit.ActiveDocument.Application.Create.NewGeometryOptions();
            opt.DetailLevel = ViewDetailLevel.Fine;

            GeometryElement gel = ThisMainExtension.Data.SelectedElement.get_Geometry(opt);

            foreach (GeometryObject geoObject in gel)
            {
                Solid s = geoObject as Solid;
                if (null != s)
                {
                    foreach (Face f in s.Faces)
                    {
                        Mesh m = f.Triangulate();

                        for (int i = 0; i < m.NumTriangles; i++)
                        {
                            MeshTriangle mt = m.get_Triangle(i);

                            XYZ pt1 = mt.get_Vertex(0);
                            XYZ pt2 = mt.get_Vertex(1);
                            XYZ pt3 = mt.get_Vertex(2);

                            triangles.Add(new Triangle(new REX.Common.Geometry.REXxyz(pt1.X, pt1.Y, pt1.Z),
                                                        new REX.Common.Geometry.REXxyz(pt2.X, pt2.Y, pt2.Z),
                                                        new REX.Common.Geometry.REXxyz(pt3.X, pt3.Y, pt3.Z)));
                        }
                    }
                    continue;
                }

                GeometryInstance inst = geoObject as GeometryInstance;
                if (null != inst)
                {
                    foreach (GeometryObject o in inst.SymbolGeometry)
                    {
                        s = o as Solid;

                        if (s != null)
                        {
                            foreach (Face f in s.Faces)
                            {
                                Mesh m = f.Triangulate();

                                m = m.get_Transformed(inst.Transform);


                                for (int i = 0; i < m.NumTriangles; i++)
                                {
                                    MeshTriangle mt = m.get_Triangle(i);

                                    XYZ pt1 = mt.get_Vertex(0);
                                    XYZ pt2 = mt.get_Vertex(1);
                                    XYZ pt3 = mt.get_Vertex(2);

                                    triangles.Add(new Triangle(new REX.Common.Geometry.REXxyz(pt1.X, pt1.Y, pt1.Z),
                                                        new REX.Common.Geometry.REXxyz(pt2.X, pt2.Y, pt2.Z),
                                                        new REX.Common.Geometry.REXxyz(pt3.X, pt3.Y, pt3.Z)));
                                }
                            }
                        }
                    }
                }
            }

            ThisMainExtension.Data.SelectedElementGeometry = triangles;
        }
        /// <summary>
        /// Initializes the instances of database and parametric sections in Data based on the selected element.
        /// </summary>
        private void ReadParamAndDBDescription()
        {
            ThisMainExtension.Progress.Steps = 2;
            ThisMainExtension.Progress.Position = 1;
            ThisMainExtension.Progress.Header = "Element reading";
            ThisMainExtension.Progress.Text = "...";
            ThisMainExtension.Progress.Show(ThisMainExtension.GetWindowForParent());

            ThisMainExtension.Data.DatabaseSection = RVTConverter.GetFamily(ThisMainExtension.Data.SelectedElement, ECategoryType.SECTION_DB) as REXFamilyType_DBSection;
            ThisMainExtension.Data.ParametricSection = RVTConverter.GetFamily(ThisMainExtension.Data.SelectedElement, ECategoryType.SECTION_PARAM) as REXFamilyType_ParamSection;

            if (ThisMainExtension.Data.DatabaseSection != null)
            {
                //Getting all records where the specified element exists.
                ThisMainExtension.Data.DatabaseRecords = ThisMainExtension.Converter.GetDBList(ThisMainExtension.Data.DatabaseSection,
                    ThisMainExtension.Converter.GetDatabaseDirectory(REX.ContentGenerator.Families.ECategoryType.SECTION_DB), REX.ContentGenerator.Families.ECategoryType.SECTION_DB);
            }

            ThisMainExtension.Progress.Hide();
        }
        /// <summary>
        /// Creates the new FamilySymbol.
        /// </summary>
        private void CreateNewFamilySymbol()
        {
            if (ThisMainExtension.Data.NewSection != null)
            {
                if (ThisMainExtension.Data.SelectedElement.StructuralType == Autodesk.Revit.DB.Structure.StructuralType.Column)
                    ThisMainExtension.Data.NewSection.ElementType = EElementType.COLUMN;
                else
                    ThisMainExtension.Data.NewSection.ElementType = EElementType.BEAM;

                ThisMainExtension.Data.NewSection.Material = EMaterial.STEEL;

                ThisMainExtension.Data.NewFamilySymbol = RVTConverter.GetElement(ThisMainExtension.Data.NewSection) as Autodesk.Revit.DB.FamilySymbol;
            }
        }
        /// <summary>
        /// Applies the new FamilySymbol to the selected element.
        /// </summary>
        /// <returns></returns>
        private bool ApplyFamilySymbolToSelectedElement()
        {
            try
            {
                ThisMainExtension.Data.SelectedElement.Symbol = ThisMainExtension.Data.NewFamilySymbol;
                return true;
            }
            catch
            {
            }

            return false;
        }
        #endregion
    }
}

