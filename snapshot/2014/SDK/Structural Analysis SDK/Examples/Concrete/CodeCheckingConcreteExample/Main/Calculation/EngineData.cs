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
using Autodesk.Revit.DB;
using CodeCheckingConcreteExample.Engine;
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.Revit.DB.CodeChecking.Engineering.Tools;
using CodeCheckingConcreteExample.Utility;
using CodeCheckingConcreteExample.ConcreteTypes;
using Autodesk.Revit.DB.CodeChecking.Storage;
using CodeCheckingConcreteExample.Properties;
using Autodesk.Revit.DB.CodeChecking;

namespace CodeCheckingConcreteExample.Main.Calculation
{
   /// <summary>
   /// Represents user's implementation of interface for the main loop class. Parameter of the default constructor of the cref="Engine" class.
   /// </summary>
   public class EngineData : IEngineData 
   {
      #region IEngineData Members

      /// <summary>
      /// Gets the display unit which user want to use in the project.
      /// </summary>
      /// <returns>cref="DisplayUnit"</returns>
      public Autodesk.Revit.DB.DisplayUnit GetInputDataUnitSystem()
      {
         DisplayUnit displayUnit = DisplayUnit.IMPERIAL;
         if (Server.Server.UnitSystem == Autodesk.Revit.DB.ResultsBuilder.UnitsSystem.Metric)
            displayUnit = DisplayUnit.METRIC;
         return displayUnit;
      }

      /// <summary>
      /// Gets number of available threads or processors. user could switch off parallel calculation by returning value 1.
      /// </summary>
      /// <param name="data">Service data.</param>
      /// <returns>Number of threads</returns>
      public int GetNumberOfThreads(Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         return Environment.ProcessorCount;
      }

      /// <summary>
      /// Gets ForceCalculationDataDescriptor object for the current structure
      /// </summary>
      /// <param name="data">Service Data</param>
      /// <param name="combinations">List of selected combinations ids </param>
      /// <param name="elementId">Id of Revit element</param>
      /// <returns>Reference to ForceCalculationDataDescriptor</returns>
      protected ForceCalculationDataDescriptor GetForceCalculationDataDescriptor(Autodesk.Revit.DB.CodeChecking.ServiceData data, List<ElementId> combinations, ElementId elementId)
      {
         Tuple<Label,CalculationParameter,BuiltInCategory,Element> intData = GetElementInternalData(data, elementId);
         Label ccLabel = intData.Item1;
         CalculationParameter calculationParameters = intData.Item2;
         BuiltInCategory category = intData.Item3;
         Element element = intData.Item4;

         List<ForceType> forceTypes = new List<ForceType>();// { ForceType.Fx, ForceType.Fy, ForceType.Fz, ForceType.Mx, ForceType.My, ForceType.Mz };

         forceTypes = GetForceTypes(data, new ElementId[] { elementId });

         ForceCalculationDataDescriptor descriptor = null;
         
            double elementLength = (element as Autodesk.Revit.DB.Structure.AnalyticalModel).GetCurve().Length;
            elementLength = Autodesk.Revit.DB.UnitUtils.ConvertFromInternalUnits( elementLength, DisplayUnitType.DUT_METERS);
            descriptor = new ForceCalculationDataDescriptorLinear(elementId, 1.0, calculationParameters.CalculationPointsSelector.GetPointCoordinates(data.Document, true, elementLength, elementId, combinations, forceTypes), true, forceTypes);
            
         if (descriptor == null)
         {
            descriptor = new ForceCalculationDataDescriptorLinear(elementId);
         }

         if (descriptor == null)
         {
            descriptor = new ForceCalculationDataDescriptor(elementId);
         }

         return descriptor;
         
      }

      /// <summary>
      /// Creates user imlementation of cref="ICalculationScenario" which consists list of calculation objects cref="ICalculatinObject".
      /// </summary>
      /// <returns>The new instance of cref="ICalculationScenario"</returns>
      public ICalculationScenario CreateCalculationScenario()
      {
         return new CalculationScenario();
      }

      /// <summary>
      /// Creates new instance of user class wich represents a section of a structure element.
      /// </summary>
      /// <param name="sectionDataBase">Instance of base class for the section.</param>
      /// <returns>New instance of user implementation of section class derived from cref="SectionDataBase".</returns>
      public SectionDataBase CreateSectionData(SectionDataBase sectionDataBase)
      {
         switch (sectionDataBase.Material)
         {
            case StructuralAssetClass.Concrete:
               switch (sectionDataBase.Category)
               {
                  default:
                     break;
                  case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
                     return new ColumnSection(sectionDataBase);
                  case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                     return new BeamSection(sectionDataBase);
               }
               break;
            case StructuralAssetClass.Metal:
               break;
         }

         return sectionDataBase;
      }

      /// <summary>
      /// Creates new instance of user class wich represents a structure element.
      /// </summary>
      /// <param name="elementDataBase">Instance of base class for the element.</param>
      /// <returns>New instance of user implementation of element class derived from cref="ElementDataBase".</returns>
      public ElementDataBase CreateElementData(ElementDataBase elementDataBase)
      {
         switch (elementDataBase.Material)
         {
            case StructuralAssetClass.Concrete:
               switch (elementDataBase.Category)
               {
                  default:
                     break;
                  case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
                     return new ColumnElement(elementDataBase);
                  case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                     return new BeamElement(elementDataBase);
               }
               break;
            case StructuralAssetClass.Metal:
               break;
         }

         return elementDataBase;
      }

      /// <summary>
      /// Creates new instance of class with results for the element.
      /// </summary>
      /// <param name="category">Category of the element.</param>
      /// <param name="material">Material of the element.</param>
      /// <returns>User result schema object for the element.</returns>
      public Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass CreateElementResult(Autodesk.Revit.DB.BuiltInCategory category, Autodesk.Revit.DB.StructuralAssetClass material)
      {
         switch (material)
         {
            case StructuralAssetClass.Concrete:
               switch (category)
               {
                  default:
                     break;
                  case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                     return new ResultBeam();
                  case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
                     return new ResultColumn();
               }
               break;
            case StructuralAssetClass.Metal:
               break;
         }

         return null;
      }

      /// <summary>
      /// Reads calculation parameters from revit data base.
      /// </summary>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <returns>User calculation parameters schema object.</returns>
      public Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass ReadCalculationParameter(Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         Autodesk.Revit.DB.CodeChecking.Storage.StorageService service = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService();
         Autodesk.Revit.DB.CodeChecking.Storage.StorageDocument storageDocument = service.GetStorageDocument(data.Document);
         CalculationParameter calculationParameter = storageDocument.CalculationParamsManager.CalculationParams.GetEntity<CalculationParameter>(data.Document);
         return calculationParameter;
      }

      /// <summary>
      /// Reads parameters of user element label.
      /// </summary>
      /// <param name="category">Category of the element.</param>
      /// <param name="material">Material of the element.</param>
      /// <param name="label">Acces to the Revit storage with labels."</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <returns>User label of the element.</returns>
      public Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass ReadElementLabel(Autodesk.Revit.DB.BuiltInCategory category, Autodesk.Revit.DB.StructuralAssetClass material, Autodesk.Revit.DB.CodeChecking.Storage.Label label, Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         if (label != null)
         {
            switch (material)
            {
               case StructuralAssetClass.Concrete:
                  switch (category)
                  {
                     default:
                        break;
                     case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical: return label.GetEntity<LabelColumn>(data.Document);
                     case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical: return label.GetEntity<LabelBeam>(data.Document);
                  }
                  break;
               case StructuralAssetClass.Metal:
                  break;
            }
         }

         return null;
      }

      /// <summary>
      /// Verify parameters of steel in the label.
      /// </summary>
      /// <param name="steel">Steel properties</param>
      /// <param name="longitudinal">Information about the type of reinforcement. True if longitudinal.</param>
      /// <returns></returns>
      public List<string> VerifySteel(CodeCheckingConcreteExample.UIComponents.RCSteelParameters.RCSteelParametersSchema steel, bool longitudinal)
      {
         List<string> errors = new List<string>();

         if (steel.Material == null)
            errors.Add(Resources.ResourceManager.GetString(longitudinal ? "ErrReinforcementMaterialL" : "ErrReinforcementMaterialT"));
         else if (steel.MinimumYieldStress < Double.Epsilon)
            errors.Add(Resources.ResourceManager.GetString(longitudinal ? "ErrReinforcementYieldStressL" : "ErrReinforcementYieldStressT"));
         if (steel.RebarBarType == null || steel.BarDiameter < Double.Epsilon || steel.DeformationType == Autodesk.Revit.DB.CodeChecking.Engineering.Concrete.ConcreteTypes.SteelSurface.Unknown)
            errors.Add(Resources.ResourceManager.GetString(longitudinal ? "ErrRebarL" : "ErrRebarT"));

         return errors;
      }

      /// <summary>
      /// Verify parameters of user element label.
      /// </summary>
      /// <param name="category">Category of the element.</param>
      /// <param name="material">Material of the element.</param>
      /// <param name="label">Element label."</param>
      /// <param name="status">Reference to element's status".</param>
      public void VerifyElementLabel(Autodesk.Revit.DB.BuiltInCategory category, StructuralAssetClass material, Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass label, 
                                     ref Autodesk.Revit.DB.CodeChecking.Storage.ResultStatus status)
      {
         if (label != null)
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
                           LabelColumn labelCol = label as LabelColumn;
                           if (labelCol != null)
                           {
                              if (labelCol.EnabledInternalForces.Count == 0)
                                 status.AddError(Resources.ResourceManager.GetString("ErrNoChosenInternalForces"));
                              List<string> errors = VerifySteel(labelCol.LongitudinalReinforcement, true);
                              foreach(string s in errors)
                                 status.AddError(s);
                              errors = VerifySteel(labelCol.TransversalReinforcement, false);
                              foreach(string s in errors)
                                 status.AddError(s);
                           }
                        }
                        break;
                     case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                        {
                           LabelBeam labelBm = label as LabelBeam;
                           if (labelBm != null)
                           {
                              if (labelBm.EnabledInternalForces.Count == 0)
                                 status.AddError(Resources.ResourceManager.GetString("ErrNoChosenInternalForces"));
                              List<string> errors = VerifySteel(labelBm.LongitudinalReinforcement, true);
                              foreach(string s in errors)
                                 status.AddError(s);
                              errors = VerifySteel(labelBm.TransversalReinforcement, false);
                              foreach(string s in errors)
                                 status.AddError(s);
                           }
                        }
                        break;
                  }
                  break;
               case StructuralAssetClass.Metal:
                  break;
            }
         }
      }

      /// <summary>
      /// Filters list of elements due to e.g. a result status for calculation purposes.
      /// </summary>
      /// <param name="listElementData">List of user element objects.</param>
      /// <returns>Filtered list of user element objects.</returns>
      public List<ObjectDataBase> FilterElementForCalculation(List<ObjectDataBase> listElementData)
      {
         List<ObjectDataBase> listElementFiltered = new List<ObjectDataBase>();

         foreach (ObjectDataBase obj in listElementData)
         {
            ElementDataBase elem = obj as ElementDataBase;
            if (elem != null)
            {
               if (!elem.Status.IsError())
               {
                  listElementFiltered.Add(elem);
               }
            }
         }

         return listElementFiltered;
      }

      /// <summary>
      /// Creates new instance of a user's class with common parameters.
      /// </summary>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <param name="parameters">Instance of base class with common parameters.</param>
      /// <returns>New instance of user implementation of class derived from cref="CommonParametersBase".</returns>
      public CommonParametersBase CreateCommonParameters(Autodesk.Revit.DB.CodeChecking.ServiceData data, CommonParametersBase parameters)
      {
         Autodesk.Revit.DB.CodeChecking.NotificationService.ProgressStart(Resources.ResourceManager.GetString("DataPreparation"), 1);

         CommonParameters commonParameters = new CommonParameters(data, parameters);
        
         List<ForceCalculationDataDescriptor> calculationDataDescriptors = new List<ForceCalculationDataDescriptor>();
         foreach (Tuple<ElementId,ResultStatus> elemStatus in commonParameters.ListElementStatus)
         {
            ForceCalculationDataDescriptor forceCalculationDataDescriptor = GetForceCalculationDataDescriptor(data, parameters.ListCombinationId, elemStatus.Item1);
            forceCalculationDataDescriptor.AddBendingForceTypes(new ForceType[] { ForceType.Ux, ForceType.Uy, ForceType.Uz });
            calculationDataDescriptors.Add(forceCalculationDataDescriptor);
            if (NotificationService.ProgressBreakInvoked())
               break;
         }

         ForceResultsPackageDescriptor[] vResPackDesc = new ForceResultsPackageDescriptor[] { ForceResultsPackageDescriptor.GetResultPackageDescriptor(data.Document, commonParameters.ActivePackageGuid) };

         // Uncomment the code below to dump to a text file the time spent on accessing ResultsBuilder
         // Int64 forceResultsCacheAccessTime = System.DateTime.Now.Ticks;
         commonParameters.ResultCache = new ForceResultsCache(data.Document, calculationDataDescriptors, vResPackDesc, commonParameters.ListCombinationId, GetInputDataUnitSystem());
         // Uncomment the code below to dump to a text file the time spent on accessing ResultsBuilder
         // forceResultsCacheAccessTime = System.DateTime.Now.Ticks - forceResultsCacheAccessTime;

         foreach (Tuple<ElementId, ResultStatus> elemStatus in commonParameters.ListElementStatus)
         {
            ForceResultsCache.ElementResultsStatus resultStatus = commonParameters.ResultCache.GetElementResultsStatus(elemStatus.Item1);
            if (resultStatus != ForceResultsCache.ElementResultsStatus.ResultsOK)
            {
               elemStatus.Item2.AddError(Resources.ResourceManager.GetString("ErrStaticResults"));
            }
         }

         // Uncomment the code below to dump to a text file the time spent on accessing ResultsBuilder
         // using (System.IO.StreamWriter writer = new System.IO.StreamWriter(data.Document.PathName + ".RBAccessTimeInfo.txt", true))
         // {
         //    string resultBuilderAccessTime = DateTime.Now.ToString() + " Cache: " + (new System.TimeSpan(forceResultsCacheAccessTime)).TotalSeconds + " RB: " + (new System.TimeSpan(commonParameters.ResultCache.ResultBuilderAccessTime)).TotalSeconds;
         // 
         //    writer.WriteLine(resultBuilderAccessTime);
         // }

         Autodesk.Revit.DB.CodeChecking.NotificationService.ProgressStep("");
         return commonParameters;
      }

      /// <summary>
      /// Creates new instance of class with list of calculation points for user's elements.
      /// </summary>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <param name="parameters">User object with common parameters.</param>
      /// <param name="elementId">Id of an element.</param>
      /// <returns>List of calculation points for user's element.</returns>
      public List<CalcPoint> CreateCalcPointsForElement(Autodesk.Revit.DB.CodeChecking.ServiceData data, CommonParametersBase parameters, ElementId elementId)
      {
         List<CalcPoint> calculationPoints = new List<CalcPoint>();
         CommonParameters commonParameters = parameters as CommonParameters;
         if (commonParameters != null)
            calculationPoints = commonParameters.ResultCache.GetCalculationPoints(elementId);
         return calculationPoints;
      }

      /// <summary>
      /// Gives possibility to read data from Revit data base and puts them into user element's objects and into user section's objects and into user common parameters.
      /// </summary>
      /// <param name="listElementData">List of user element objects.</param>
      /// <param name="parameters">User common parameters.</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      public void ReadFromRevitDB(List<ObjectDataBase> listElementData, CommonParametersBase parameters, Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         // read additional information from Revit data and store them in user objects derived from ElementDataBase and listed in listElementData

         Autodesk.Revit.DB.CodeChecking.NotificationService.ProgressStart(Resources.ResourceManager.GetString("ReadingGeometry"), listElementData.Count);
         ElementAnalyser elementAnalyser = new ElementAnalyser(GetInputDataUnitSystem());

         int step = 0;
         foreach (ObjectDataBase elemData in listElementData)
         {
            Element element = data.Document.GetElement(elemData.ElementId);
            if (element != null)
            {
               switch (elemData.Category)
               {
                  default:
                     break;
                  case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
                     {
                        ColumnElement elem = elemData as ColumnElement;
                        if (elem != null && !elem.Status.IsError())
                        {
                           elem.Info = elementAnalyser.Analyse(element);
                           if (elem.Info.Material.Characteristics.YoungModulus.X < Double.Epsilon)
                              elem.Status.AddError(Resources.ResourceManager.GetString("ErrYoungModulus"));

                           MaterialConcreteCharacteristics concrete = (MaterialConcreteCharacteristics)elem.Info.Material.Characteristics.Specific;
                           if (concrete == null || concrete.Compression < Double.Epsilon)
                              elem.Status.AddError(Resources.ResourceManager.GetString("ErrConcreteCompression"));

                           foreach (SectionDataBase sectionDataBase in elem.ListSectionData)
                           {
                              ColumnSection sec = sectionDataBase as ColumnSection;
                              if (sec != null)
                                 sec.Info = elem.Info;
                           }
                        }
                        break;
                     }
                  case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                     {
                        BeamElement elem = elemData as BeamElement;
                        if (elem != null && !elem.Status.IsError())
                        {
                           elementAnalyser.TSectionAnalysis = false;
                           LabelBeam labelBeam = elem.Label as LabelBeam;
                           if (labelBeam != null)
                           {
                              elementAnalyser.TSectionAnalysis = labelBeam.SlabBeamInteraction == BeamSectionType.WithSlabBeamInteraction;
                           }
                           
                           elem.Info = elementAnalyser.Analyse(element);
                           if (elem.Info.Material.Characteristics.YoungModulus.X < Double.Epsilon)
                              elem.Status.AddError(Resources.ResourceManager.GetString("ErrYoungModulus"));

                           MaterialConcreteCharacteristics concrete = (MaterialConcreteCharacteristics)elem.Info.Material.Characteristics.Specific;
                           if (concrete == null || concrete.Compression < Double.Epsilon)
                              elem.Status.AddError(Resources.ResourceManager.GetString("ErrConcreteCompression"));

                           foreach (SectionDataBase sectionDataBase in elem.ListSectionData)
                           {
                              BeamSection sec = sectionDataBase as BeamSection;
                              if (sec != null)
                                 sec.Info = elem.Info;
                           }
                        }
                        break;
                     }
               }
            }
            Autodesk.Revit.DB.CodeChecking.NotificationService.ProgressStep(string.Format("{0:d}%", ++step * 100 / listElementData.Count));
            if (NotificationService.ProgressBreakInvoked())
               break;
         }
      }

      /// <summary>
      /// Gives possibility to write data to Revit data base.
      /// </summary>
      /// <param name="listElementData">List of user element objects.</param>
      /// <param name="parameters">User common parameters.</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      public void SaveToRevitDB(List<ObjectDataBase> listElementData, CommonParametersBase parameters, Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {

         Autodesk.Revit.DB.ResultsBuilder.Storage.ResultsPackageBuilder builder = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService().GetStorageDocument(data.Document).CalculationParamsManager.CalculationParams.GetOutputResultPackageBuilder(Server.Server.ID);
         ForceResultsWriter resultsWriter = new ForceResultsWriter(data.Document, builder, Autodesk.Revit.DB.ResultsBuilder.UnitsSystem.Metric);

         foreach (ObjectDataBase objectDataBase in listElementData)
         {
            ElementDataBase elementDataBase = objectDataBase as ElementDataBase;

            switch (elementDataBase.Category)
            {
               default:
                  break;
               case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                  {
                     //TBD - RB doesn't support relative coordinates as input 
                     //Begin mod
                     //IList<double> vx = (from sec in elementDataBase.ListSectionData select (sec as LinearSection).GetCalcResultsInPt()[ResultTypeLinear.X_Rel ]).ToList();
                     bool isListForces = true;
                     foreach (LinearSection sec in elementDataBase.ListSectionData)
                     {
                        if (sec.ListInternalForces == null || sec.ListInternalForces.Count == 0)
                        {
                           isListForces = false;
                           break;
                        }
                     }
                     if (isListForces)
                     {
                        IList<double> xCoordinates = (from sec in elementDataBase.ListSectionData select (sec as LinearSection).GetCalcResultsInPoint()[ResultTypeLinear.X]).ToList();
                        //End Mod
                        resultsWriter.SetMeasurementForElement(elementDataBase.ElementId, AxisDirection.X, xCoordinates);

                        ResultTypeLinear[] resultTypesLinear = new ResultTypeLinear[] { ResultTypeLinear.Abottom, ResultTypeLinear.Atop, ResultTypeLinear.Aleft, ResultTypeLinear.Aright };
                        foreach (ResultTypeLinear forceType in resultTypesLinear)
                        {
                           ICollection<double> valuesInPoints = (from LinearSection section in elementDataBase.ListSectionData select section.GetCalcResultsInPoint()[forceType]).ToList();
                           resultsWriter.AddResultsForElement(elementDataBase.ElementId, forceType.GetResultType(), valuesInPoints);
                        }
                     }
                  }
                  break;
            }
         }
         
         resultsWriter.StoreResultsInResultsBuilder("RCCalculationsResults");
         //builder.Finish();
      }


      private Tuple<Label,CalculationParameter,BuiltInCategory,Element> GetElementInternalData(Autodesk.Revit.DB.CodeChecking.ServiceData data, ElementId elementId)
      {
         Element element = data.Document.GetElement(elementId);
         BuiltInCategory category = Autodesk.Revit.DB.CodeChecking.Tools.GetCategoryOfElement(element);
         StorageDocument storageDocument = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService().GetStorageDocument(data.Document);
         CalculationParameter calculationParameter = storageDocument.CalculationParamsManager.CalculationParams.GetEntity<CalculationParameter>(data.Document);
         Label ccLabel = storageDocument.LabelsManager.GetLabel(element);

         return new Tuple<Label, CalculationParameter, BuiltInCategory, Element>(ccLabel, calculationParameter, category, element);
      }

      private List<ForceType> GetForceTypes(Autodesk.Revit.DB.CodeChecking.ServiceData data, IEnumerable<ElementId> elementsIds)
      {
         List<ForceType> forceTypes = new List<ForceType>();
         foreach (ElementId elementId in elementsIds)
         {
            Tuple<Label, CalculationParameter, BuiltInCategory, Element> elementsInternalData = GetElementInternalData(data, elementId);
            BuiltInCategory category = elementsInternalData.Item3;
            Label ccLabel = elementsInternalData.Item1;

            switch (category)
            {
               default:
                  break;
               case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
                  {
                     LabelColumn label = ReadElementLabel(category, ccLabel.Material, ccLabel, data) as LabelColumn;
                     if (label != null)
                     {
                        forceTypes = label.EnabledInternalForces.Select(s => s.GetForceType()).ToList();
                     }
                     break;
                  }
               case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                  {
                     LabelBeam label = ReadElementLabel(category, ccLabel.Material, ccLabel, data) as LabelBeam;
                     if (label != null)
                     {
                        forceTypes = label.EnabledInternalForces.Select(s => s.GetForceType()).ToList();
                     }
                     break;
                  }
            }
         }
         return forceTypes;
      }

      #endregion
   }


}
