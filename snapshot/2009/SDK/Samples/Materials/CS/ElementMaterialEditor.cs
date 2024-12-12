//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Structural;

namespace Revit.SDK.Samples.Materials.CS
{
    /// <summary>
    /// A object to edit structural element's material.
    /// </summary>
    public class ElementMaterialEditor
    {        
        private Element m_selected;// A selected element to change material.        
        private Material m_material;// A material be set to a structural element.        
        //Dictionary for edit different element's material.
        private Dictionary<string, EditHandler> m_editHandler;

        //A delegate for edit different element's material.
        private delegate bool EditHandler();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ElementMaterialEditor()
        {
            m_editHandler = new Dictionary<string, EditHandler>();
            m_editHandler.Add("Wall", new EditHandler(EditWallMaterial));
            m_editHandler.Add("FamilyInstance", new EditHandler(EditFamilyInstanceMaterial));
            m_editHandler.Add("Floor", new EditHandler(EditFloorMaterial));
        }

        /// <summary>
        /// Change the selected element's material, if this element is structural.
        /// </summary>
        /// <returns>Return true if changed successful, or else return false.</returns>
        public bool ChangeMaterial(Element selected, Material material)
        {
            if (null == selected || null == material)
                throw new ArgumentNullException();

            m_selected = selected;
            m_material = material;

            EditHandler editHandler = m_editHandler[m_selected.GetType().Name];
            return editHandler();
        }

        /// <summary>
        /// Change wall's material, if this wall is a structural wall.
        /// </summary>
        /// <returns>Return true if changed successful, or else return false.</returns>
        private bool EditWallMaterial()
        {
            Wall wall = m_selected as Wall;
            if (null == wall)
                return false;

            if (null == wall.AnalyticalModel)
                return false;

            WallType wallType = wall.WallType;

            // Change all layers' material of  the wall.
            foreach (CompoundStructureLayer layer in wallType.CompoundStructure.Layers)
            {
                layer.Material = m_material;
            }

            return true;
        }

        /// <summary>
        /// Change a family instance's material, if it is a structural beam
        /// or a structural column.
        /// </summary>
        /// <returns>Return true if changed successful, or else return false.</returns>
        private bool EditFamilyInstanceMaterial()
        {
            try
            {
                FamilyInstance fInstance = m_selected as FamilyInstance;
                if (null == fInstance)
                    return false;

                if (null == fInstance.AnalyticalModel)
                    return false;

                // Change a family instance's material by setting a material id to its parameter.
                foreach (Parameter p in fInstance.Parameters)
                {
                    if (p.Definition.Name.Equals("Beam Material")
                        || p.Definition.Name.Equals("Column Material"))
                    {
                        ElementId id = m_material.Id;
                        p.Set(ref id);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Change a slab's material, if this slab is a structural slab.
        /// </summary>
        /// <returns>Return true if changed successful, or else return false.</returns>
        private bool EditFloorMaterial()
        {
            Floor floor = m_selected as Floor;
            if (null == floor)
                return false;

            if (null == floor.AnalyticalModel)
                return false;

            // Change all layers' material of  the floor.
            foreach (CompoundStructureLayer layer in floor.FloorType.CompoundStructure.Layers)
            {
                layer.Material = m_material;
            }

            return true;
        }
    }
}
