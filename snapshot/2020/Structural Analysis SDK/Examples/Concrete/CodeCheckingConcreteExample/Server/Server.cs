//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Linq;
using System.Text;
using Autodesk.Revit.DB.CodeChecking;
using Autodesk.Revit.DB.CodeChecking.Documentation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.Revit.DB.CodeChecking.Engineering.Concrete;
using CodeCheckingConcreteExample.Utility;
using CodeCheckingConcreteExample.Main;
using CodeCheckingConcreteExample.Properties;
using Autodesk.Revit.DB.CodeChecking.Engineering.Tools;

#pragma warning disable 1591

namespace CodeCheckingConcreteExample.Server
{
   [Autodesk.Revit.DB.CodeChecking.Attributes.CalculationParamsStructure(typeof(CalculationParameter))]
   [Autodesk.Revit.DB.CodeChecking.Attributes.LabelStructure(typeof(LabelBeam), BuiltInCategory.OST_BeamAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.LabelStructure(typeof(LabelColumn), BuiltInCategory.OST_ColumnAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.ResultStructure(typeof(ResultBeam), BuiltInCategory.OST_BeamAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.ResultStructure(typeof(ResultColumn), BuiltInCategory.OST_ColumnAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   /// <structural_toolkit_2015> 
   [Autodesk.Revit.DB.CodeChecking.Attributes.LabelStructure(typeof(LabelWall), BuiltInCategory.OST_WallAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.LabelStructure(typeof(LabelFloor), BuiltInCategory.OST_FloorAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.LabelStructure(typeof(LabelFloor), BuiltInCategory.OST_FoundationSlabAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.ResultStructure(typeof(ResultWall), BuiltInCategory.OST_WallAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.ResultStructure(typeof(ResultFloor), BuiltInCategory.OST_FloorAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   [Autodesk.Revit.DB.CodeChecking.Attributes.ResultStructure(typeof(ResultFloor), BuiltInCategory.OST_FoundationSlabAnalytical, Autodesk.Revit.DB.StructuralAssetClass.Concrete)]
   /// </structural_toolkit_2015> 
   public partial class Server : Autodesk.Revit.DB.CodeChecking.Documentation.MultiStructureServer
   {
      public static readonly Autodesk.Revit.DB.ResultsBuilder.UnitsSystem UnitSystem = Autodesk.Revit.DB.ResultsBuilder.UnitsSystem.Metric;
      public static readonly Guid ID = new Guid("1d3d13ad-4330-4b4a-bfe3-067ff87a72b1");
      #region ICodeCheckingServer Members

      public override Entity GetDefaultLabel(Autodesk.Revit.DB.Document document, StructuralAssetClass material, BuiltInCategory category)
      {
         switch (material)
         {
            case StructuralAssetClass.Concrete:
               switch (category)
               {
                  default:
                     break;
                  case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
                     {
                        IList<ElementId> materials = RebarUtility.GetListMaterialOfRebars(document);
                        if (materials.Count > 0)
                        {
                           IList<ElementId> barsIds = RebarUtility.GetListRebarForMaterial(document, materials[0]);
                           if (barsIds.Count > 0)
                           {
                              LabelColumn label = new LabelColumn();
                              label.LongitudinalReinforcement.Material = document.GetElement(materials[0]) as Material;
                              label.LongitudinalReinforcement.RebarBarType = RebarUtility.GetRebarType(document, barsIds[0]);

                              label.TransversalReinforcement.Material = label.LongitudinalReinforcement.Material;
                              label.TransversalReinforcement.RebarBarType = label.LongitudinalReinforcement.RebarBarType;

                              return label.GetEntity();
                           }
                        }
                     }
                     break;
                  case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                     {
                        IList<ElementId> materials = RebarUtility.GetListMaterialOfRebars(document);
                        if (materials.Count > 0)
                        {
                           IList<ElementId> barsIds = RebarUtility.GetListRebarForMaterial(document, materials[0]);
                           if (barsIds.Count > 0)
                           {
                              LabelBeam label = new LabelBeam();
                              label.LongitudinalReinforcement.Material = document.GetElement(materials[0]) as Material;
                              label.LongitudinalReinforcement.RebarBarType = RebarUtility.GetRebarType(document, barsIds[0]);

                              label.TransversalReinforcement.Material = label.LongitudinalReinforcement.Material;
                              label.TransversalReinforcement.RebarBarType = label.LongitudinalReinforcement.RebarBarType;

                              return label.GetEntity();
                           }
                        }
                     }
                     break;
                  /// <structural_toolkit_2015> 
                  case Autodesk.Revit.DB.BuiltInCategory.OST_FloorAnalytical:
                  case Autodesk.Revit.DB.BuiltInCategory.OST_FoundationSlabAnalytical:
                     {
                        IList<ElementId> materials = RebarUtility.GetListMaterialOfRebars(document);
                        if (materials.Count > 0)
                        {
                           IList<ElementId> barsIds = RebarUtility.GetListRebarForMaterial(document, materials[0]);
                           if (barsIds.Count > 0)
                           {
                              LabelFloor label = new LabelFloor();
                              label.PrimaryReinforcement.Material = document.GetElement(materials[0]) as Material;
                              label.PrimaryReinforcement.RebarBarType = RebarUtility.GetRebarType(document, barsIds[0]);
                              label.SecondaryReinforcement.Material = document.GetElement(materials[0]) as Material;
                              label.SecondaryReinforcement.RebarBarType = RebarUtility.GetRebarType(document, barsIds[0]);
                              return label.GetEntity();
                           }
                        }
                     }
                     break;
                  case Autodesk.Revit.DB.BuiltInCategory.OST_WallAnalytical:
                     {
                        IList<ElementId> materials = RebarUtility.GetListMaterialOfRebars(document);
                        if (materials.Count > 0)
                        {
                           IList<ElementId> barsIds = RebarUtility.GetListRebarForMaterial(document, materials[0]);
                           if (barsIds.Count > 0)
                           {
                              LabelWall label = new LabelWall();
                              label.VerticalReinforcement.Material = document.GetElement(materials[0]) as Material;
                              label.VerticalReinforcement.RebarBarType = RebarUtility.GetRebarType(document, barsIds[0]);
                              label.HorizontalReinforcement.Material = document.GetElement(materials[0]) as Material;
                              label.HorizontalReinforcement.RebarBarType = RebarUtility.GetRebarType(document, barsIds[0]);
                              return label.GetEntity();
                           }
                        }
                     }
                     break;
                  /// </structural_toolkit_2015> 
               }
               break;
         }

         return base.GetDefaultLabel(document, material, category);
      }
      /// <structural_toolkit_2015>
 

      // TODO : REMOVE

      /// <summary>
      /// Classification of elements type
      /// </summary>
      public enum ElementType { 
         /// <summary>
         /// RC Beam
         /// </summary>
         Beam, 
         /// <summary>
         /// RC Column
         /// </summary>
         Column, 
         /// <summary>
         /// RC Floor and Slab foundation
         /// </summary>
         Floor, 
         /// <summary>
         /// RC Wall
         /// </summary>
         Wall 
      }
      /// </structural_toolkit_2015> 

      /// <structural_toolkit_2015> 
      
      /// <summary>
      /// Build document body for result document
      /// </summary>
      /// <param name="calculationParameters">Code calculation parameters</param>
      /// <param name="element">Revit element for which result document is built</param>
      /// <param name="document">Active Revit document</param>
      /// <returns>DocumentBody object</returns>
      public override DocumentBody BuildResultDocumentBody(Autodesk.Revit.DB.ExtensibleStorage.Entity calculationParameters, Element element, Autodesk.Revit.DB.Document document)
      {
         Autodesk.Revit.DB.CodeChecking.Storage.StorageService service = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService();
         Autodesk.Revit.DB.CodeChecking.Storage.StorageDocument storageDocument = service.GetStorageDocument(document);
         Guid activePackageId = storageDocument.CalculationParamsManager.CalculationParams.GetInputResultPackageId(Server.ID);
         Autodesk.Revit.DB.CodeChecking.Storage.ResultStatus status = storageDocument.ResultsManager.GetResultStatus(element, activePackageId, Server.ID);
         DocumentBody body = new DocumentBody();
         if (!status.IsError())
         {
            // create body object
            // get results schema for the current element
            string schemaName = calculationParameters.Schema.SchemaName;
            switch (schemaName)
            {
               case "ResultBeam":
                  ResultLinearElement resultBeam = new ResultBeam();
                  resultBeam.SetProperties(calculationParameters);
                  WriteResultsToNoteForLinearElements(body, resultBeam, document,ElementType.Beam);
                  break;
               case "ResultColumn":
                  ResultLinearElement resultColumn = new ResultColumn();
                  resultColumn.SetProperties(calculationParameters);
                  WriteResultsToNoteForLinearElements(body, resultColumn, document,ElementType.Column);
                  break;
               case "ResultFloor":
                  ResultSurfaceElement resultFloor = new ResultSurfaceElement();
                  resultFloor.SetProperties(calculationParameters);
                  WriteResultsToNoteForSurfaceElements(body, resultFloor, document, element, ElementType.Floor);
                  break;
               case "ResultWall":
                  ResultSurfaceElement resultWall = new ResultSurfaceElement();
                  resultWall.SetProperties(calculationParameters);
                  WriteResultsToNoteForSurfaceElements(body, resultWall, document, element, ElementType.Wall);
                  break;
            }
         }
         return body;
      }
      /// </structural_toolkit_2015> 



      public override IList<BuiltInCategory> GetSupportedCategories(StructuralAssetClass material)
      {
         List<BuiltInCategory> supportedCategories = new List<BuiltInCategory>();
         supportedCategories.Add(BuiltInCategory.OST_BeamAnalytical);
         supportedCategories.Add(BuiltInCategory.OST_ColumnAnalytical);
         /// <structural_toolkit_2015>
         supportedCategories.Add(BuiltInCategory.OST_FoundationSlabAnalytical);
         supportedCategories.Add(BuiltInCategory.OST_FloorAnalytical);
         supportedCategories.Add(BuiltInCategory.OST_WallAnalytical);
         /// </structural_toolkit_2015>
         return supportedCategories;
      }

      public override IList<Autodesk.Revit.DB.StructuralAssetClass> GetSupportedMaterials()
      {
         return new List<Autodesk.Revit.DB.StructuralAssetClass>(){
                Autodesk.Revit.DB.StructuralAssetClass.Concrete};
      }

      #endregion

      /// <summary>
      /// Gets the document body for label.
      /// </summary>
      /// <param name="label">label.</param>
      /// <param name="document">document.</param>
      /// <returns>
      /// The body of the document.
      /// </returns>
      public override DocumentBody BuildLabelDocumentBody(Entity label, Autodesk.Revit.DB.Document document)
      {
         switch (label.Schema.SchemaName)
         {
            default:
               return base.BuildLabelDocumentBody(label, document);
            case "LabelColumn":
               {
                  DocumentBody body = new DocumentBody();
                  LabelColumn labelColumn = new LabelColumn();
                  labelColumn.SetProperties(label, document);

                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("CalculationOptions"), 4));

                  if (labelColumn.EnabledInternalForces.Count > 0)
                  {
                     body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("EnabledInternalForces") + ":", true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnFX_Note"), true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FY))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnFY_Note"), true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FZ))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnFZ_Note"), true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnMX_Note"), true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MY))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnMY_Note"), true));
                     if (labelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MZ))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("ColumnMZ_Note"), true));
                  }

                  body.Elements.Add(DocumentElement.GetDocumentElement("CreepCoefficient", labelColumn, this, document));

                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("Buckling"), 4));
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("BucklingDirectionY"), 5));
                  if (labelColumn.BucklingDirectionY)
                  {
                     body.Elements.Add(DocumentElement.GetDocumentElement("LengthCoefficientY", labelColumn, this, document));
                     body.Elements.Add(DocumentElement.GetDocumentElement("ColumnStructureTypeY", labelColumn, this, document));
                  }
                  else
                  {
                     body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("NotTakenIntoConsideration"), true));
                  }


                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("BucklingDirectionZ"), 5));
                  if (labelColumn.BucklingDirectionZ)
                  {
                     body.Elements.Add(DocumentElement.GetDocumentElement("LengthCoefficientZ", labelColumn, this, document));
                     body.Elements.Add(DocumentElement.GetDocumentElement("ColumnStructureTypeZ", labelColumn, this, document));
                  }
                  else
                  {
                     body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("NotTakenIntoConsideration"), true));
                  }


                  // longitudinal
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("LongitudinalReinforcement"), 4));
                  if (labelColumn.LongitudinalReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(labelColumn.LongitudinalReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), labelColumn.LongitudinalReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (labelColumn.LongitudinalReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), labelColumn.LongitudinalReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), labelColumn.LongitudinalReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), labelColumn.LongitudinalReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));

                  // transverse
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("TransversalReinforcement"), 4));
                  if (labelColumn.TransversalReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(labelColumn.TransversalReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), labelColumn.TransversalReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (labelColumn.TransversalReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), labelColumn.TransversalReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), labelColumn.TransversalReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), labelColumn.TransversalReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));


                  return body;
               }
            case "LabelBeam":
               {
                  DocumentBody body = new DocumentBody();
                  LabelBeam labelBeam = new LabelBeam();
                  labelBeam.SetProperties(label, document);


                  body.Elements.Add(new DocumentLineBreak(1));
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("CalculationOptions"), 4));

                  if (labelBeam.EnabledInternalForces.Count > 0)
                  {
                     body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("EnabledInternalForces") + ":", true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamFX_Note"), true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FY))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamFY_Note"), true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FZ))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamFZ_Note"), true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamMX_Note"), true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MY))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamMY_Note"), true));
                     if (labelBeam.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MZ))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("BeamMZ_Note"), true));
                  }

                  body.Elements.Add(DocumentElement.GetDocumentElement("CreepCoefficient", labelBeam, this, document));

                  string interaction = Resources.ResourceManager.GetString("No");
                  if (labelBeam.SlabBeamInteraction == ConcreteTypes.BeamSectionType.WithSlabBeamInteraction)
                  {
                     interaction = Resources.ResourceManager.GetString("Yes");
                  }

                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("SlabBeamInteraction"), interaction));

                  // longitudinal
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("LongitudinalReinforcement"), 4));
                  if (labelBeam.LongitudinalReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(labelBeam.LongitudinalReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), labelBeam.LongitudinalReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (labelBeam.LongitudinalReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), labelBeam.LongitudinalReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), labelBeam.LongitudinalReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), labelBeam.LongitudinalReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));

                  // transverse
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("TransversalReinforcement"), 4));
                  if (labelBeam.TransversalReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(labelBeam.TransversalReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), labelBeam.TransversalReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (labelBeam.TransversalReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), labelBeam.TransversalReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), labelBeam.TransversalReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), labelBeam.TransversalReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));

                  return body;
               }
            /// <structural_toolkit_2015>
            case "LabelFloor":
               {
                  DocumentBody body = new DocumentBody();
                  LabelFloor LabelFloor = new LabelFloor();
                  LabelFloor.SetProperties(label, document);


                  body.Elements.Add(new DocumentLineBreak(1));
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("CalculationOptions"), 4));

                  if (LabelFloor.EnabledInternalForces.Count > 0)
                  {
                     body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("EnabledInternalForces") + ":", true));
                     if (LabelFloor.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("FloorF_Note"), true));
                     if (LabelFloor.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FZ))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("FloorQ_Note"), true));
                     if (LabelFloor.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MY))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("FloorM_Note"), true));
                  }

                  body.Elements.Add(DocumentElement.GetDocumentElement("CreepCoefficient", LabelFloor, this, document));
                  // Primary
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("PrimaryReinforcement"), 4));
                  if (LabelFloor.PrimaryReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(LabelFloor.PrimaryReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), LabelFloor.PrimaryReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (LabelFloor.PrimaryReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), LabelFloor.PrimaryReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), LabelFloor.PrimaryReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), LabelFloor.PrimaryReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));

                  // Secondary
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("SecondaryReinforcement"), 4));
                  if (LabelFloor.SecondaryReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(LabelFloor.SecondaryReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), LabelFloor.SecondaryReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (LabelFloor.SecondaryReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), LabelFloor.SecondaryReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), LabelFloor.SecondaryReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), LabelFloor.SecondaryReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));

                  return body;
               }
            case "LabelWall":
               {
                  DocumentBody body = new DocumentBody();
                  LabelWall LabelWall = new LabelWall();
                  LabelWall.SetProperties(label, document);


                  body.Elements.Add(new DocumentLineBreak(1));
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("CalculationOptions"), 4));

                  if (LabelWall.EnabledInternalForces.Count > 0)
                  {
                     body.Elements.Add(new DocumentText(Resources.ResourceManager.GetString("EnabledInternalForces") + ":", true));
                     if (LabelWall.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("WallF_Note"), true));
                     if (LabelWall.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.FZ))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("WallQ_Note"), true));
                     if (LabelWall.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MX))
                        body.Elements.Add(new DocumentText("- " + Resources.ResourceManager.GetString("WallM_Note"), true));
                  }

                  body.Elements.Add(DocumentElement.GetDocumentElement("CreepCoefficient", LabelWall, this, document));
                  // Vertical
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("VerticalReinforcement"), 4));
                  if (LabelWall.VerticalReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(LabelWall.VerticalReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), LabelWall.VerticalReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (LabelWall.VerticalReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), LabelWall.VerticalReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), LabelWall.VerticalReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), LabelWall.VerticalReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));

                  // Horizontal
                  body.Elements.Add(new DocumentTitle(Resources.ResourceManager.GetString("HorizontalReinforcement"), 4));
                  if (LabelWall.HorizontalReinforcement.Material != null)
                  {
                     StructuralAsset assL = Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.RebarUtility.GetMaterialStructuralAsset(LabelWall.HorizontalReinforcement.Material);
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), assL.Name));
                  }
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("Material"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("MinimumYieldStress"), LabelWall.HorizontalReinforcement.MinimumYieldStress, UnitsConverter.GetInternalUnit(UnitType.UT_Stress), UnitType.UT_Stress, document.GetUnits()));

                  if (LabelWall.HorizontalReinforcement.RebarBarType != null)
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), LabelWall.HorizontalReinforcement.RebarBarType.Name));
                  else
                     body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("RebarBarType"), ""));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("DeformationType"), LabelWall.HorizontalReinforcement.DeformationType.ToString()));
                  body.Elements.Add(new DocumentValueWithName(Resources.ResourceManager.GetString("BarDiameter"), LabelWall.HorizontalReinforcement.BarDiameter, UnitsConverter.GetInternalUnit(UnitType.UT_Bar_Diameter), UnitType.UT_Bar_Diameter, document.GetUnits()));

                  return body;
               }
               /// </structural_toolkit_2015>
         }
      }

      /// <summary>
      /// Gets the document body for calculation parameters.
      /// </summary>
      /// <param name="calcParams">Calculation parameters</param>
      /// <param name="document">document</param>
      /// <returns>Body of the document</returns>
      public override DocumentBody BuildCalculationParamDocumentBody(Entity calcParams, Autodesk.Revit.DB.Document document)
      {
         DocumentBody body = new DocumentBody();

         return body;
      }

      #region IExternalServer Members

      public override string GetDescription()
      {
         return "Code Checking Concrete Example for Revit API";
      }

      public override string GetName()
      {
         return "CodeCheckingConcreteExample";
      }

      public override Guid GetServerId()
      {
         return ID;
      }

      public override string GetVendorId()
      {
         return "ADSK";
      }

      public override void Verify(Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         Autodesk.Revit.DB.CodeChecking.Storage.StorageService service = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService();
         Autodesk.Revit.DB.CodeChecking.Storage.StorageDocument storageDocument = service.GetStorageDocument(data.Document);


         Main.Calculation.EngineData enginData = new Main.Calculation.EngineData();
         Engine.Engine engine = new Engine.Engine(enginData);

         engine.Calculate(this, data);
      }

      /// <summary>
      /// Gets the output package result types. This information is needed to create proper ResultsBuilder Package.
      /// </summary>
      /// <returns></returns>
      public override Autodesk.Revit.DB.ResultsBuilder.ResultsPackageTypes GetOutputPackageResultTypes()
      {
         return Autodesk.Revit.DB.ResultsBuilder.ResultsPackageTypes.RequiredReinforcement | Autodesk.Revit.DB.ResultsBuilder.ResultsPackageTypes.SteelRatio;
      }
      /// <summary>
      /// Gets the output package unit system. This information is needed to create proper ResultsBuilder Package.
      /// </summary>
      /// <returns></returns>
      public override Autodesk.Revit.DB.ResultsBuilder.UnitsSystem GetOutputPackageUnitSystem()
      {
         return UnitSystem;
      }


      #endregion

      #region ICodeCheckingServerDocumentation Members

      public override string GetResource(string key, string context)
      {
         string txt = Resources.ResourceManager.GetString(key);

         if (!string.IsNullOrEmpty(txt))
            return txt;

         return key;
      }

      #endregion
   }
}
