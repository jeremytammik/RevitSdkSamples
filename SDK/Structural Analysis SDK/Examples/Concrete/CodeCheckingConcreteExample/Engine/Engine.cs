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
using Autodesk.Revit.DB.CodeChecking;
using Autodesk.Revit.DB.CodeChecking.Documentation;
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage.Framework;
using Autodesk.Revit.DB.CodeChecking.Storage;
using CodeCheckingConcreteExample.Server;
using CodeCheckingConcreteExample.Properties;
using System.Threading;
using System.Threading.Tasks;

namespace CodeCheckingConcreteExample.Engine
{
   /// <summary>
   /// Represents the main calculation loop.
   /// </summary>
   public class Engine
   {
      private IEngineData EngineData;

      private Engine() { }

      /// <summary>
      /// Initializes a new instance of the Engine class.  
      /// </summary>
      /// <param name="engineData">The interface cref="IEngineData" needed to parametrize Engine.</param>
      public Engine(IEngineData engineData)
      {
         EngineData = engineData;
      }

      /// <summary>
      /// Runs calculation for elements served in user implementation of cref="IEngineData".
      /// </summary>
      /// <param name="server">Acces to cref="Server".</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      public void Calculate(Server.Server server, Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         List<Tuple<ElementId, ResultStatus>> listElementStatus = ReadListElementIdWithStatus(server, data);

         if (listElementStatus.Count > 0)
            CalculateSetOfElements(listElementStatus, server, data);
      }

      /// <summary>
      /// Reads from Revit information about selected elements, next Calculates the list of elements, at the end saves calculation results in Revit Data Base.
      /// </summary>
      /// <param name="listElementStatus">List identyficators of elements with result status.</param>
      /// <param name="server">Acces to cref="Server".</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      protected void CalculateSetOfElements(List<Tuple<ElementId, ResultStatus>> listElementStatus, Server.Server server, Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         // reading from Revit Api
         CommonParametersBase parameters = ReadCommonParameters(listElementStatus, server, data);
         List<ObjectDataBase> listElementData = ReadListElementData(data, listElementStatus, parameters);
         if (NotificationService.ProgressBreakInvoked() == false)
            EngineData.ReadFromRevitDB(listElementData, parameters, data);

         int maxNumberOfThreads = EngineData.GetNumberOfThreads(data);
         if (maxNumberOfThreads < 1)
            maxNumberOfThreads = 1;

         // pure calculation (without any connection with Revit api)
         List<ObjectDataBase> listElementFiltered = EngineData.FilterElementForCalculation(listElementData);
         if (NotificationService.ProgressBreakInvoked() == false)
            /// <structural_toolkit_2015>
            CalculateElementList(listElementFiltered, parameters, maxNumberOfThreads, server);
            /// </structural_toolkit_2015>

         // writing to Revit Api
         if (NotificationService.ProgressBreakInvoked() == false)
         {
            SaveListElementData(data, listElementData);
            EngineData.SaveToRevitDB(listElementData, parameters, data);
         }
      }

      /// <structural_toolkit_2015>

      /// <summary>
      /// Gets list of elements with the given category.
      /// </summary>
      /// <param name="category">Category of the element.</param>
      /// <param name="material">Material of the element.</param>
      /// <param name="listElementData">List of elements objects</param>
      List<ObjectDataBase> GetListElementForCategory(BuiltInCategory category, StructuralAssetClass material, List<ObjectDataBase> listElementData)
      {
         List<ObjectDataBase> list = new List<ObjectDataBase>();
         foreach (ObjectDataBase obj in listElementData)
            if (obj.Category == category && obj.Material == material)
               list.Add(obj);

         return list;
      }
      /// </structural_toolkit_2015>


      /// <structural_toolkit_2015>

      /// <summary>
      /// Calculates list of elements according to calculation scenario returned by cref="CreateCalculationScenario".
      /// </summary>
      /// <param name="listElementData">List of elements objects</param>
      /// <param name="parameters">Common parameters</param>
      /// <param name="maxNumberOfThreads">Maximal namber of threads.</param>
      /// <param name="server">Acces to cref="Server".</param>
      protected void CalculateElementList(List<ObjectDataBase> listElementData, CommonParametersBase parameters, int maxNumberOfThreads, Server.Server server)
      {
         ICalculationScenario scenario = EngineData.CreateCalculationScenario(parameters);

         int stepsCount = 0;

         foreach(Autodesk.Revit.DB.StructuralAssetClass material in server.GetSupportedMaterials())
            foreach (BuiltInCategory category in server.GetSupportedCategories(material))
            {
               List<ICalculationObject> scenarioList = scenario.CalculationScenarioList(category, material);
               foreach (ICalculationObject calcObj in scenarioList)
               {
                  foreach (ElementDataBase elemData in listElementData)
                  {
                     if (elemData.Category == category && elemData.Material == material && calcObj.Categories.Contains(elemData.Category))
                     {
                        if (calcObj.Type == CalculationObjectType.Element)
                           stepsCount++;
                        else
                           stepsCount += elemData.ListSectionData.Count;
                     }
                  }
               }
            }

         Autodesk.Revit.DB.CodeChecking.NotificationService.ProgressStart(Resources.ResourceManager.GetString("Calculation"), 100);
         int stepNbr = 0;

         if (maxNumberOfThreads < 2)
         {
            foreach(Autodesk.Revit.DB.StructuralAssetClass material in server.GetSupportedMaterials())
               foreach (BuiltInCategory category in server.GetSupportedCategories(material))
               {
                  List<ObjectDataBase> listElement = GetListElementForCategory(category, material, listElementData);
                  List<ICalculationObject> scenarioList = scenario.CalculationScenarioList(category,material);
                  foreach (ICalculationObject calcObj in scenarioList)
                     calcObj.Parameters = parameters;
                  foreach (ObjectDataBase element in listElement)
                  {
                     bool errorOccured = false;
                     foreach (ICalculationObject calcObj in scenarioList)
                     {
                        if (calcObj.Categories.Contains(element.Category))
                        {
                           if (calcObj.Type == CalculationObjectType.Element)
                           {
                              if (NotificationService.ProgressBreakInvoked())
                                 break;
                              stepNbr++;
                              if ((int)((stepNbr - 1) * 100 / stepsCount) != (int)(stepNbr * 100 / stepsCount))
                                 NotificationService.ProgressStep(string.Format("{0:d}%", stepNbr * 100 / stepsCount));
                              if (!errorOccured || calcObj.ErrorResponse == ErrorResponse.RunOnError)
                                 if (!calcObj.Run(element))
                                    errorOccured = true;
                           }
                           else
                           {
                              List<SectionDataBase> listSections = (element as ElementDataBase).ListSectionData;
                              int sectionSteps = listSections.Count;
                              if (!errorOccured || calcObj.ErrorResponse == ErrorResponse.RunOnError)
                              {
                                 foreach (SectionDataBase section in listSections)
                                 {
                                    if (NotificationService.ProgressBreakInvoked())
                                       break;
                                    stepNbr++;
                                    sectionSteps--;
                                    if ((int)((stepNbr - 1) * 100 / stepsCount) != (int)(stepNbr * 100 / stepsCount))
                                       NotificationService.ProgressStep(string.Format("{0:d}%", stepNbr * 100 / stepsCount));
                                    if (!calcObj.Run(section))
                                    {
                                       errorOccured = true;
                                       if (calcObj.ErrorResponse == ErrorResponse.SkipOnError)
                                          break;
                                    }
                                 }
                              }
                              while (sectionSteps > 0)
                              {
                                 stepNbr++;
                                 sectionSteps--;
                                 if ((int)((stepNbr - 1) * 100 / stepsCount) != (int)(stepNbr * 100 / stepsCount))
                                    NotificationService.ProgressStep(string.Format("{0:d}%", stepNbr * 100 / stepsCount));
                              }
                           }
                        }
                     }
                     if (NotificationService.ProgressBreakInvoked())
                        break;
                  }
               }
         }
         else
         {
            foreach(Autodesk.Revit.DB.StructuralAssetClass material in server.GetSupportedMaterials())
               foreach (BuiltInCategory category in server.GetSupportedCategories(material))
               {
                  List<ObjectDataBase> listElement = GetListElementForCategory(category, material, listElementData);
                  List<ICalculationObject> scenarioList = scenario.CalculationScenarioList(category,material);
                  foreach (ICalculationObject calcObj in scenarioList)
                     calcObj.Parameters = parameters;
                  object oStepNbr = (object)stepNbr;
                  Parallel.ForEach(listElement, new ParallelOptions() { MaxDegreeOfParallelism = maxNumberOfThreads }, (element, elLoopState) =>
                     {
                        bool errorOccured = false;
                        foreach (ICalculationObject calcObj in scenarioList)
                        {
                           if (calcObj.Categories.Contains(element.Category))
                           {
                              if (calcObj.Type == CalculationObjectType.Element)
                              {
                                 if (NotificationService.ProgressBreakInvoked())
                                    elLoopState.Break();
                                 lock (oStepNbr)
                                 {
                                    stepNbr++;
                                    if ((int)((stepNbr - 1) * 100 / stepsCount) != (int)(stepNbr * 100 / stepsCount))
                                       NotificationService.ProgressStep(string.Format("{0:d}%", stepNbr * 100 / stepsCount));
                                 }
                                 if (!errorOccured || calcObj.ErrorResponse == ErrorResponse.RunOnError)
                                    if (!calcObj.Run(element))
                                       errorOccured = true;
                              }
                              else
                              {
                                 List<SectionDataBase> listSections = (element as ElementDataBase).ListSectionData;
                                 int sectionSteps = listSections.Count;
                                 if (!errorOccured || calcObj.ErrorResponse == ErrorResponse.RunOnError)
                                 {
                                    Parallel.ForEach(listSections, new ParallelOptions() { MaxDegreeOfParallelism = maxNumberOfThreads }, (section, secLoopState) =>
                                       {
                                          if (NotificationService.ProgressBreakInvoked())
                                             secLoopState.Break();
                                          lock (oStepNbr)
                                          {
                                             stepNbr++;
                                             sectionSteps--;
                                             if ((int)((stepNbr - 1) * 100 / stepsCount) != (int)(stepNbr * 100 / stepsCount))
                                                NotificationService.ProgressStep(string.Format("{0:d}%", stepNbr * 100 / stepsCount));
                                          }
                                          if (!calcObj.Run(section))
                                          {
                                             errorOccured = true;
                                             if (calcObj.ErrorResponse == ErrorResponse.SkipOnError)
                                                secLoopState.Break();
                                          }
                                       });
                                 }
                                 while (sectionSteps > 0)
                                 {
                                    stepNbr++;
                                    sectionSteps--;
                                    if ((int)((stepNbr - 1) * 100 / stepsCount) != (int)(stepNbr * 100 / stepsCount))
                                       NotificationService.ProgressStep(string.Format("{0:d}%", stepNbr * 100 / stepsCount));
                                 }
                              }
                           }
                        }
                        if (NotificationService.ProgressBreakInvoked())
                           elLoopState.Break();
                     });
               }
         }
      }
      /// </structural_toolkit_2015>

      /// <summary>
      /// Read from Revit parameters common for all selected elements and stores them in cref="CommonParametersBase".
      /// </summary>
      /// <param name="listElementStatus">List identyficators of elements with result status.</param>
      /// <param name="server">Acces to cref="Server".</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <returns>Common parameters.</returns>
      protected CommonParametersBase ReadCommonParameters(List<Tuple<ElementId, ResultStatus>> listElementStatus, Server.Server server, Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         Autodesk.Revit.DB.CodeChecking.Storage.StorageService service = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService();
         Autodesk.Revit.DB.CodeChecking.Storage.StorageDocument storageDocument = service.GetStorageDocument(data.Document);

         Guid activePackageId = storageDocument.CalculationParamsManager.CalculationParams.GetInputResultPackageId(server.GetServerId());

         Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass calcParams = EngineData.ReadCalculationParameter(data);

         List<ElementId> listCombinationId = new List<ElementId>();
         if (server.LoadCasesAndCombinationsSupport())
            listCombinationId = storageDocument.CalculationParamsManager.CalculationParams.GetLoadCasesAndCombinations(Server.Server.ID);

         CommonParametersBase parameBase = new CommonParametersBase(listElementStatus, listCombinationId, activePackageId, calcParams);
         CommonParametersBase parameters = EngineData.CreateCommonParameters(data, parameBase);
         return parameters;
      }

      /// <summary>
      /// Reads from Revit selected elements identificators and collect in the list only that which has apprppriate label, material and category.
      /// </summary>
      /// <param name="server">Acces to cref="Server".</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <returns>List identyficators of elements with result status.</returns>
      protected List<Tuple<ElementId, ResultStatus>> ReadListElementIdWithStatus(Server.Server server, Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         Autodesk.Revit.DB.CodeChecking.Storage.StorageService service = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService();
         Autodesk.Revit.DB.CodeChecking.Storage.StorageDocument storageDocument = service.GetStorageDocument(data.Document);
         Guid activePackageId = storageDocument.CalculationParamsManager.CalculationParams.GetInputResultPackageId(server.GetServerId());

         List<Tuple<ElementId, ResultStatus>> listElementId = new List<Tuple<ElementId, ResultStatus>>();
         foreach (Element element in data.Selection)
         {
            Autodesk.Revit.DB.CodeChecking.Storage.Label ccLabel = storageDocument.LabelsManager.GetLabel(element);
            if (ccLabel != null)
            {
               Autodesk.Revit.DB.BuiltInCategory category = element.Category.BuiltInCategory;
               StructuralAssetClass material = ccLabel.Material;

               if (server.GetSupportedMaterials().Contains(material) &&
                   server.GetSupportedCategories(material).Contains(category))
               {
                  SchemaClass label = EngineData.ReadElementLabel(category, material, ccLabel, data);
                  ResultStatus status = new Autodesk.Revit.DB.CodeChecking.Storage.ResultStatus(Server.Server.ID, activePackageId);
                  EngineData.VerifyElementLabel(category, material, label, ref status);

                  listElementId.Add(new Tuple<ElementId, ResultStatus>(element.Id, status));
               }
            }
         }

         return listElementId;
      }

      /// <summary>
      /// Reads from Revit information about selected elements and store it in the list with elements data.
      /// </summary>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <param name="listElementStatus">List identyficators of elements with result status.</param>
      /// <param name="parameters">Common parameters.</param>
      /// <returns>List of elements data.</returns>
      protected List<ObjectDataBase> ReadListElementData(Autodesk.Revit.DB.CodeChecking.ServiceData data, List<Tuple<ElementId, ResultStatus>> listElementStatus, CommonParametersBase parameters)
      {
         Autodesk.Revit.DB.CodeChecking.Storage.StorageService service = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService();
         Autodesk.Revit.DB.CodeChecking.Storage.StorageDocument storageDocument = service.GetStorageDocument(data.Document);

         List<ObjectDataBase> listElementData = new List<ObjectDataBase>();
         foreach (Tuple<ElementId, ResultStatus> elemStatus in listElementStatus)
         {
            Element element = data.Document.GetElement(elemStatus.Item1);
            if (element != null)
            {
               Autodesk.Revit.DB.CodeChecking.Storage.Label ccLabel = storageDocument.LabelsManager.GetLabel(element);
               if (ccLabel != null)
               {
                  Autodesk.Revit.DB.BuiltInCategory category = element.Category.BuiltInCategory;
                  StructuralAssetClass material = ccLabel.Material;
                  Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass label = EngineData.ReadElementLabel(category, material, ccLabel, data);
                  Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass result = EngineData.CreateElementResult(category, material);

                  ObjectDataBase objectData = new ObjectDataBase(elemStatus.Item1, category, material, label);
                  List<SectionDataBase> listSectionsData = new List<SectionDataBase>();
                  List<CalcPoint> listCalcPoints = EngineData.CreateCalcPointsForElement(data, parameters, elemStatus.Item1);
                  foreach (CalcPoint p in listCalcPoints)
                  {
                     SectionDataBase sectBase = new SectionDataBase(p, objectData);
                     SectionDataBase sectData = EngineData.CreateSectionData(sectBase);

                     listSectionsData.Add(sectData);
                  }

                  ElementDataBase elemBase = new ElementDataBase(result, listCalcPoints, listSectionsData, elemStatus.Item2, data.Document, objectData);
                  ElementDataBase elemData = EngineData.CreateElementData(elemBase);

                  listElementData.Add(elemData);
               }
            }
         }

         return listElementData;
      }

      /// <summary>
      /// Saves in Revit data base information collected in the list elements data.
      /// </summary>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <param name="listElementData">List of elements data.</param>
      protected void SaveListElementData(Autodesk.Revit.DB.CodeChecking.ServiceData data, List<ObjectDataBase> listElementData)
      {
         Autodesk.Revit.DB.CodeChecking.NotificationService.ProgressStart(Resources.ResourceManager.GetString("DataSaving"), listElementData.Count);
         int step = 0;
         foreach (ElementDataBase elemData in listElementData)
         {
            SaveElementResult(elemData.ElementId, elemData.Result, elemData.Status, data);
            step++;
            Autodesk.Revit.DB.CodeChecking.NotificationService.ProgressStep(string.Format("{0:d}%", step * 100 / listElementData.Count));
         }
      }

      /// <summary>
      /// Saves in Revit the result object of an element.
      /// </summary>
      /// <param name="elementId">Identificator of an element.</param>
      /// <param name="resultSchema">Result object.</param>
      /// <param name="status">Status of element's results.</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      protected void SaveElementResult(ElementId elementId, Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass resultSchema, Autodesk.Revit.DB.CodeChecking.Storage.ResultStatus status, Autodesk.Revit.DB.CodeChecking.ServiceData data)
      {
         Autodesk.Revit.DB.CodeChecking.Storage.StorageService service = Autodesk.Revit.DB.CodeChecking.Storage.StorageService.GetStorageService();
         Autodesk.Revit.DB.CodeChecking.Storage.StorageDocument storageDocument = service.GetStorageDocument(data.Document);

         Element element = data.Document.GetElement(elementId);

         if (resultSchema != null)
            storageDocument.ResultsManager.SetResult(resultSchema.GetEntity(), element, status, true);
      }

   }
}
