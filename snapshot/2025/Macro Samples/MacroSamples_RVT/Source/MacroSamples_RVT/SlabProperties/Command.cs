//
// (C) Copyright 2003-2008 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// app.ActiveUIDocumentumentation.
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
using Autodesk.Revit.DB.Structure;

using MacroCSharpSamples;
using System.Collections.Generic;
using MacroSamples_RVT;

namespace Revit.SDK.Samples.SlabProperties.CS
{
   /// <summary>
   /// Get some properties of a slab , such as Level, Type name, Span direction,
   /// Material name, Thickness, and Young Modulus for the slab's Material.
   /// </summary>
   public class SlabProperties
   {
      #region Class constant variables
      const double PI = 3.1415926535879;
      const int Degree = 180;
      const int ToMillimeter = 1000;
      const double ToMetricThickness = 0.3048;    // unit for changing inch to meter
      const double ToMetricYoungmodulus = 304800.0;

      #endregion


      #region Class member variables
      ThisApplication? m_app;

      ICollection<ElementId>? m_slabComponent;  // the selected Slab component
      Floor? m_slabFloor; // Floor 
      CompoundStructureLayer? m_slabLayer; // Structure Layer 
      IList<CompoundStructureLayer>? m_slabLayerCollection; // Structure Layer collection

      string m_level = string.Empty;         // level name of Slab
      string m_typeName = string.Empty;      // type name of Slab
      string m_spanDirection = string.Empty; // span direction (degree) of Slab
      string m_thickness = string.Empty;     // thick ness (millmeter) of slab layer
      string m_materialName = string.Empty;  // material name of slab layer
      string m_youngModulusX = string.Empty; // Young modulus X
      string m_youngModulusY = string.Empty; // Young modulus Y
      string m_youngModulusZ = string.Empty; // Young modulus Z
      int m_numberOfLayers = 0;   // number of Structure Layers

      #endregion


      #region Class Constructor methods implementation
      /// <summary>
      /// Ctro without parameter is not allowed
      /// </summary>
      private SlabProperties()
      {
         // none
      }

      /// <summary>
      /// Ctor with ThisDocument as 
      /// </summary>
      /// <param name="hostApp">ThisDocument handler</param>
      public SlabProperties(ThisApplication hostApp)
      {
         m_app = hostApp;
      }

      /// <summary>
      /// Run this sample
      /// </summary>
      public void Run()
      {
         try
         {
            if (m_app == null)
               return;
            // function initialization and find out a slab's Level, Type name, and set the Span Direction properties.
            bool isInitialization = this.Initialize(m_app);
            if (false == isInitialization)
            {
               return;
            }

            // show a displayForm to display the properties of the slab
            SlabPropertiesForm slabForm = new SlabPropertiesForm(this);
            if (DialogResult.OK != slabForm.ShowDialog())
            {
               return;
            }
         }
         catch (Exception displayProblem)
         {
            MessageBox.Show(displayProblem.ToString());
            return;
         }
      }
      #endregion


      #region Class propertied
      /// <summary>
      /// Level property, read only.
      /// </summary>
      public string Level
      {
         get
         {
            return m_level;
         }
      }


      /// <summary>
      /// TypeName property, read only.
      /// </summary>
      public string TypeName
      {
         get
         {
            return m_typeName;
         }
      }


      /// <summary>
      /// SpanDirection property, read only.
      /// </summary>
      public string SpanDirection
      {
         get
         {
            return m_spanDirection;
         }
      }


      /// <summary>
      /// NumberOfLayers property, read only.
      /// </summary>
      public int NumberOfLayers
      {
         get
         {
            return m_numberOfLayers;
         }
      }


      /// <summary>
      /// LayerThickness property, read only.
      /// </summary>
      public string LayerThickness
      {
         get
         {
            return m_thickness;
         }
      }


      /// <summary>
      /// LayerMaterialName property, read only.
      /// </summary>
      public string LayerMaterialName
      {
         get
         {
            return m_materialName;
         }
      }


      /// <summary>
      /// LayerYoungModulusX property, read only.
      /// </summary>
      public string LayerYoungModulusX
      {
         get
         {
            return m_youngModulusX;
         }
      }


      /// <summary>
      /// LayerYoungModulusY property, read only.
      /// </summary>
      public string LayerYoungModulusY
      {
         get
         {
            return m_youngModulusY;
         }
      }


      /// <summary>
      /// LayerYoungModulusZ property, read only.
      /// </summary>
      public string LayerYoungModulusZ
      {
         get
         {
            return m_youngModulusZ;
         }
      }
      #endregion


      #region Public class method
      /// <summary>
      /// SetLayer method
      /// </summary>
      /// <param name="layerNumber">The layerNumber for the number of the layers</param>
      public void SetLayer(int layerNumber)
      {
         // Get each layer.
         // An individual layer can be accessed by Layers property and its thickness and material can then be reported.
         if (m_slabLayerCollection == null)
         {
            return;
         }
         m_slabLayer = m_slabLayerCollection[layerNumber];

         // Get the Thickness property and change to the metric millimeter
         m_thickness = ((m_slabLayer.Width) * ToMetricThickness * ToMillimeter).ToString() + " mm";

         // Get the Material name property
         Material? slabLayerMaterial = GetMaterial(m_slabLayer.MaterialId);
         if (null != slabLayerMaterial)
         {
            m_materialName = slabLayerMaterial.Name;
         }
         else
         {
            m_materialName = "Null";
         }

         // The Young modulus can be found from the material by using the following generic parameters: 
         // PHY_MATERIAL_PARAM_YOUNG_MOD1, PHY_MATERIAL_PARAM_YOUNG_MOD2, PHY_MATERIAL_PARAM_YOUNG_MOD3
         if (null != slabLayerMaterial)
         {
            Parameter? youngModuleAttribute = null;
            youngModuleAttribute = slabLayerMaterial.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD1);
            if (null != youngModuleAttribute)
            {
               m_youngModulusX = (youngModuleAttribute.AsDouble() / ToMetricYoungmodulus).ToString("F2") + " MPa";
            }
            youngModuleAttribute = slabLayerMaterial.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD2);
            if (null != youngModuleAttribute)
            {
               m_youngModulusY = (youngModuleAttribute.AsDouble() / ToMetricYoungmodulus).ToString("F2") + " MPa";
            }
            youngModuleAttribute = slabLayerMaterial.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD3);
            if (null != youngModuleAttribute)
            {
               m_youngModulusZ = (youngModuleAttribute.AsDouble() / ToMetricYoungmodulus).ToString("F2") + " MPa";
            }
         }
         else
         {
            m_youngModulusX = "Null";
            m_youngModulusY = "Null";
            m_youngModulusZ = "Null";
         }
      }
      #endregion


      #region Private class memeber methods

      /// <summary>
      /// Get material element from an element id.
      /// </summary>
      private Material? GetMaterial(ElementId elemId)
      {
         return m_app?.ActiveUIDocument.Document.GetElement(elemId) as Material;
      }

      /// <summary>
      /// Initialization and find out a slab's Level, Type name, and set the Span Direction properties.
      /// </summary>
      /// <param name="revit">The revit object for the active instance of Autodesk Revit.</param>
      /// <returns>A value that signifies if your intitialization was successful for true or failed for false.</returns>
      private bool Initialize(ThisApplication app)
      {
         m_slabComponent = app.ActiveUIDocument.Selection.GetElementIds();

         // There must be exactly one slab selected
         if (m_slabComponent.Count == 0)
         {
            // nothing selected
            MessageBox.Show("Please select a slab.");
            return false;
         }
         else if (1 != m_slabComponent.Count)
         {
            // too many things selected
            MessageBox.Show("Please select only one slab.");
            return false;
         }

         foreach (ElementId id in m_slabComponent)
         {
            Element e = app.ActiveUIDocument.Document.GetElement(id);
            // If the element isn't a slab, give the message and return failure. 
            // Else find out its Level, Type name, and set the Span Direction properties. 
            if ("Autodesk.Revit.DB.Floor" != e.GetType().FullName)
            {
               MessageBox.Show("A slab should be selected.");
               return false;
            }

            // Change the element type to floor type
            m_slabFloor = e as Floor;

            // Get the layer information from the type object by using the CompoundStructure property
            // The Layers property is then used to retrieve all the layers
            if (m_slabFloor == null)
               return false;
            m_slabLayerCollection = m_slabFloor.FloorType.GetCompoundStructure().GetLayers();
            m_numberOfLayers = m_slabLayerCollection.Count;

            // Get the Level property by the floor's Level property
            m_level = app.ActiveUIDocument.Document.GetElement(m_slabFloor.LevelId).Name;

            // Get the Type name property by the floor's FloorType property
            m_typeName = m_slabFloor.FloorType.Name;

            // The span direction can be found using generic parameter access 
            // using the built in parameter FLOOR_PARAM_SPAN_DIRECTION
            Parameter spanDirectionAttribute;
            spanDirectionAttribute = m_slabFloor.get_Parameter(BuiltInParameter.FLOOR_PARAM_SPAN_DIRECTION);
            if (null != spanDirectionAttribute)
            {
               // Set the Span Direction property
               this.SetSpanDirection(spanDirectionAttribute.AsDouble());
            }
         }
         return true;
      }


      /// <summary>
      /// Set SpanDirection property to the class private member
      /// Because of the property retrieved from the parameter uses radian for unit, we should change it to degree.
      /// </summary>
      /// <param name="spanDirection">The value of span direction property</param>
      private void SetSpanDirection(double spanDirection)
      {
         double spanDirectionDegree;

         // Change "radian" to "degree".
         spanDirectionDegree = spanDirection / PI * Degree;

         // If the absolute value very small, we consider it to be zero
         if (Math.Abs(spanDirectionDegree) < 1E-12)
         {
            spanDirectionDegree = 0.0;
         }

         // The precision is 0.01, and unit is "degree".
         m_spanDirection = spanDirectionDegree.ToString("F2") + "�";
      }
      #endregion
   }
}
