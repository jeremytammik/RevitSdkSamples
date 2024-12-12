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
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.Elements; 

namespace Revit.SDK.Samples.Materials.CS
{
    /// <summary>
    /// A object for Revit materials management.
    /// </summary>
    public class MaterialsMgr
    {
        private MaterialSet m_materials;//All available materials store as a set.        
        //Dictionary for add different materials.
        private Dictionary<string, AddMaterialDelegate> m_addMaterialHandler;
        //Dictionary for classify materials by its type.
        private Dictionary<string, ClassifyMaterialDelegate> m_classifyMaterialHandler;
        //All available materials map with their name.
        private Dictionary<string, Material> m_materialsMap;

        private List<string> m_allMaterials;//All available materials' name.
        private List<string> m_allConcrete;//All available concrete materials' name.
        private List<string> m_allGeneric;//All available generic materials' name.
        private List<string> m_allOther;//All available other materials' name.
        private List<string> m_allSteel;//All available steel materials' name.
        private List<string> m_allWood;//All available wood materials' name.
        private List<string> m_currentMaterials;//The current material list.

        private Material m_activeMaterial;//The selected material.        
        //All MaterialParameters object for representation of parameters of the selected one.
        private MaterialParameters m_activeParameters;
        private Document m_document;//The currently active project.

        /// <summary>
        /// The all parameters of the selected material.
        /// </summary>
        public MaterialParameters ActiveMaterialParams
        {
            get
            {
                return m_activeParameters;
            }
        }

        /// <summary>
        /// Get Current Selected Material
        /// </summary>
        public Material ActiveMaterial
        {
            get { return m_activeMaterial; }
        }

        /// <summary>
        /// Return true if the selected material allow to duplicate, or else return false.
        /// </summary>
        public bool CanDuplicate
        {
            get
            {
                MaterialConcrete mc = m_activeMaterial as MaterialConcrete;
                if (null == mc || mc.LightWeight
                    || (m_materialsMap.TryGetValue(mc.Name + "_LW", out m_activeMaterial)))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// All available materials' names.
        /// </summary>
        public ReadOnlyCollection<string> AllMaterials
        {
            get
            {
                return new ReadOnlyCollection<string>(m_currentMaterials);
            }
        }

        /// <summary>
        /// Current unit system.
        /// </summary>
        public string DisplayUnitSystem
        {
            get
            {
                return m_document.DisplayUnitSystem.ToString();
            }
        }

        //A delegate for add different materials.
        private delegate void AddMaterialDelegate(Material material);
        //A delegate for classify materials by its type.
        private delegate List<string> ClassifyMaterialDelegate();

        /// <summary>
        /// Initializes a new instance of the MaterialsMgr class.
        /// Allocate memory data store.
        /// </summary>
        /// <param name="doc">The currently active project.</param>
        /// <param name="app">Revit application.</param>
        public MaterialsMgr(Document doc, Autodesk.Revit.Application app)
        {
            Debug.Assert(null != doc);
            Debug.Assert(null != app);

            m_document = doc;
            m_materials = app.Create.NewMaterialSet();
            m_materialsMap = new Dictionary<string, Material>();

            m_addMaterialHandler = new Dictionary<string, AddMaterialDelegate>();
            m_addMaterialHandler.Add("MaterialConcrete", new AddMaterialDelegate(AddConcrete));
            m_addMaterialHandler.Add("MaterialGeneric", new AddMaterialDelegate(AddGeneric));
            m_addMaterialHandler.Add("MaterialOther", new AddMaterialDelegate(AddOther));
            m_addMaterialHandler.Add("MaterialSteel", new AddMaterialDelegate(AddSteel));
            m_addMaterialHandler.Add("MaterialWood", new AddMaterialDelegate(AddWood));

            m_classifyMaterialHandler = new Dictionary<string, ClassifyMaterialDelegate>();
            m_classifyMaterialHandler.Add("MaterialConcrete", new ClassifyMaterialDelegate(ClassifyConcrete));
            m_classifyMaterialHandler.Add("MaterialGeneric", new ClassifyMaterialDelegate(ClassifyGeneric));
            m_classifyMaterialHandler.Add("MaterialOther", new ClassifyMaterialDelegate(ClassifyOther));
            m_classifyMaterialHandler.Add("MaterialSteel", new ClassifyMaterialDelegate(ClassifySteel));
            m_classifyMaterialHandler.Add("MaterialWood", new ClassifyMaterialDelegate(ClassifyWood));
            m_classifyMaterialHandler.Add("All", new ClassifyMaterialDelegate(ClassifyAll));

            m_allMaterials = new List<string>();
            m_allConcrete = new List<string>();
            m_allGeneric = new List<string>();
            m_allOther = new List<string>();
            m_allSteel = new List<string>();
            m_allWood = new List<string>();
            m_currentMaterials = new List<string>();            
        }        

        /// <summary>
        /// Classify material by its own type.
        /// </summary>
        /// <param name="materialType">Material type</param>
        /// <returns>Return true if successed, or else return false</returns>
        public bool Classify(string materialType)
        {
            if (null == materialType)
            {
                return false;
            }
            else
            {
                ClassifyMaterialDelegate classifyMaterialDelegate = m_classifyMaterialHandler[materialType];
                m_currentMaterials = classifyMaterialDelegate();
                return true;
            }
        }

        /// <summary>
        /// Get all available material from the active revit document. 
        /// </summary>
        /// <returns>return true if can get someting, or else, return false.</returns>
        public bool GetAllMaterials()
        {
            // Can not work well, return false.
            if (null == m_document || null == m_allMaterials
                || null == m_materials || null == m_materialsMap)
            {
                return false;
            }

            // Get all available materials from revit.
            m_materials = m_document.Settings.Materials;

            if (0 == m_materials.Size)
            {
                return false;
            }

            // Fill out data for Form's data source.
            m_allMaterials.Clear();
            m_allConcrete.Clear();
            m_allGeneric.Clear();
            m_allOther.Clear();
            m_allSteel.Clear();
            m_allWood.Clear();

            AddMaterialDelegate addMaterialDeleage;
            foreach (Material m in m_materials)
            {
                addMaterialDeleage = m_addMaterialHandler[m.GetType().Name];
                addMaterialDeleage(m);
                m_allMaterials.Add(m.Name);
                m_materialsMap[m.Name] = m;
            }

            m_currentMaterials = m_allMaterials;
            // Get data successed.
            return true;
        }

        /// <summary>
        /// Get a notification which material is selected by user. 
        /// And fill out data for property grid's data source.
        /// </summary>
        /// <param name="selectedName">which material is selected by user</param>
        /// <returns>Return true if work well, or else return false. </returns>
        public bool SelectNewMaterial(string selectedName)
        {
            // Return false if can not get active material data.
            if (m_materialsMap.TryGetValue(selectedName, out m_activeMaterial))
            {
                m_activeParameters = MaterialParametersFactory.CreateMaterialParameters(m_activeMaterial);
                
                return true;
            }
            else
            {
                return false;
            }            
        }

        /// <summary>
        /// Duplicate a new material object.
        /// </summary>
        /// <param name="materialName"> The name of a material object allow to be duplicate.</param>
        /// <returns>Return true if duplicate successed, or else return false.</returns>
        public bool DuplicateNewMaterial(string materialName)
        {
            MaterialConcrete material = m_materialsMap[materialName] as MaterialConcrete;
            string newName = materialName + "_LW";

            if (null == material)
            {
                return false;
            }

            Material newMaterial = material.Duplicate(newName);
            
            if (null == newMaterial)
                return false;

            Settings settings = m_document.Settings;
            MaterialConcrete duplicateMaterial = newMaterial as MaterialConcrete;
            ModifyMaterial(ref duplicateMaterial, settings);

            AddMaterialDelegate addMaterialDeleage = m_addMaterialHandler[newMaterial.GetType().Name];
            addMaterialDeleage(newMaterial);

            m_allMaterials.Add(newMaterial.Name);
            m_materials.Insert(newMaterial);
            m_materialsMap[newName] = newMaterial;

            return true;
        }

        /// <summary>
        /// Set material to the last newly created concrete material.
        /// </summary>
        /// <param name="materialName">The last newly created concrete material</param>
        /// <returns>If successed return true.</returns>
        public bool SetMaterial(string materialName)
        {
            try
            {
                // Get the selected material by its name.
                Material material = m_materialsMap[materialName] as Material;
                if (null == material)
                {
                    return false;
                }
                else
                {
                    foreach (Element e in m_document.Selection.Elements)
                    {
                        //A structural element material editor.
                        ElementMaterialEditor editor = new ElementMaterialEditor();

                        // Change material of selected structural element.
                        editor.ChangeMaterial(e, material);
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
        /// Do some modification as the specified.
        /// </summary>
        /// <param name="materialConcrete"> The material need mordify.</param>
        /// <param name="settings">Settings data from revit.</param>
        /// <returns>Return true if modify successed, or else return false.</returns>
        private bool ModifyMaterial(ref MaterialConcrete materialConcrete, Settings settings)
        {
            //Modification
            FillPatternSet fillPatterns = settings.FillPatterns;
            if (null == fillPatterns)
            {
                return false;
            }

            materialConcrete.LightWeight = true;
            materialConcrete.UnitWeight = materialConcrete.UnitWeight * 0.8;
            materialConcrete.Color = new Color(255, 0, 0);

            foreach (FillPattern f in fillPatterns)
            {
                if (f.Name.Equals("Brick"))
                {
                    materialConcrete.SurfacePattern = f;
                    continue;
                }

                if (f.Name.Equals("Diagonal crosshatch"))
                {
                    materialConcrete.CutPattern = f;
                    continue;
                }
            }

            return true;
        }

        #region Delegates implement

        private void AddConcrete(Material material)
        {
            m_allConcrete.Add(material.Name);
        }

        private void AddGeneric(Material material)
        {
            m_allGeneric.Add(material.Name);
        }

        private void AddOther(Material material)
        {
            m_allOther.Add(material.Name);
        }

        private void AddSteel(Material material)
        {
            m_allSteel.Add(material.Name);
        }

        private void AddWood(Material material)
        {
            m_allWood.Add(material.Name);
        }

        private List<string> ClassifyConcrete()
        {
            return m_allConcrete;
        }

        private List<string> ClassifyGeneric()
        {
            return m_allGeneric;
        }

        private List<string> ClassifyOther()
        {
            return m_allOther;
        }

        private List<string> ClassifySteel()
        {
            return m_allSteel;
        }

        private List<string> ClassifyWood()
        {
            return m_allWood;
        }

        private List<string> ClassifyAll()
        {
            return m_allMaterials;
        }
        #endregion
    }
}
