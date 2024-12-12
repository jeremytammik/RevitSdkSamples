//
// (C) Copyright 2003-2023 by Autodesk, Inc. All rights reserved.
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Autodesk.Revit.DB;
using Revit.SDK.Samples.CloudAPISample.CS.View;
using Newtonsoft.Json;

namespace Revit.SDK.Samples.CloudAPISample.CS.Migration
{
   /// <summary>
   ///    This is a sample shows how to migrate A360 Teams project to BIM360 Docs
   ///    with Revit Cloud API
   /// </summary>
   public class MigrationToBim360 : SampleContext
   {
      /// <summary>
      ///    File name to store models' guid info.
      /// </summary>
      public const string FModelsGuid = "modelsguid.json";

      /// <summary>
      ///    File name to store linked models' info
      /// </summary>
      public const string FLinksInfo = "linkinfo.json";

      /// <summary>
      ///    Default constructor
      /// </summary>
      public MigrationToBim360()
      {
         Model = new UIMigrationViewModel();
         View = new ViewMigrationToBim360(this);
      }

      /// <summary>
      ///    Model for migration case
      /// </summary>
      public IMigrationModel Model { get; }

      /// <inheritdoc />
      public override void Terminate()
      {
         View = null;
      }

      private IEnumerable<RevitLinkType> GetLinkInstances(Document document)
      {
         var links = new FilteredElementCollector(document);
         var classFilter = new ElementClassFilter(typeof(RevitLinkType));
         links.WherePasses(classFilter);
         foreach (var link in links)
            if (link is RevitLinkType revitLinkType)
               yield return revitLinkType;
      }

      private FolderLocation GetTargetFolderUrn(IList<MigrationRule> rules, string directory, string model)
      {
         foreach (var rule in rules)
         {
            var models = Directory.GetFiles(directory, rule.Pattern, SearchOption.TopDirectoryOnly);
            if (models.Contains(model))
               return rule.Target;
         }

         // If the given model can not match any pattern in rules, use root folder by default.
         return Model.AvailableFolders.Last();
      }

      /// <summary>
      ///    User coroutine to update UI during processing.
      /// </summary>
      public IEnumerator Upload(string directory, Guid accountId, Guid projectId,
         ObservableCollection<MigrationRule> modelRules)
      {
         var view = (ViewMigrationToBim360) View;
         if (!Directory.Exists(directory))
         {
            view.UpdateUploadingProgress("Directory doesn't exist.", 0);
            yield break;
         }

         view.UpdateUploadingProgress("Ready to start.", 2);

         var models = Directory.GetFiles(directory, "*.rvt", SearchOption.AllDirectories);
         var count = 0;

         var ops = new OpenOptions
         {
            OpenForeignOption = OpenForeignOption.DoNotOpen,
            DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets
         };

         // Open all documents and analyze if document has links. 
         // If so, save the relationship info into "mapLocalModelPathToLinksName"
         var mapLocalModelPathToLinksName = new Dictionary<string, List<string>>();
         foreach (var model in models)
         {
            var name = Path.GetFileName(model);
            var progress = count++ * 100 / models.Length;
            var modelpath = ModelPathUtils.ConvertUserVisiblePathToModelPath(model);
            view.UpdateUploadingProgress($"Analyzing {name}", progress);
            yield return null;

            try
            {
               var doc = Application.Application.OpenDocumentFile(modelpath, ops);
               var links = new List<string>();
               foreach (var linkInstance in GetLinkInstances(doc)) links.Add(linkInstance.Name);

               if (links.Count > 0) mapLocalModelPathToLinksName[model] = links;
               doc.Close(false);
            }
            catch (Exception e)
            {
               var msg = $"Failed to analyze {name} - {e.Message}";
               view.UpdateUploadingProgress(msg, progress);
               MessageBox.Show(msg);
               yield break;
            }

            yield return null;
         }

         // All link info should be dump to local file in case something wrong happens during uploading process
         var jsonString = JsonConvert.SerializeObject(mapLocalModelPathToLinksName);

         File.WriteAllText(FLinksInfo, jsonString);

         view.UpdateUploadingProgress("Analyzing finished", 100);

         // Begin Uploading
         count = 0;
         var mapModelsNameToGuid = new Dictionary<string, string>();
         foreach (var model in models)
         {
            var name = Path.GetFileName(model);
            var progress = count++ * 100 / models.Length;
            var modelpath = ModelPathUtils.ConvertUserVisiblePathToModelPath(model);
            view.UpdateUploadingProgress($"Uploading {name}", progress);
            yield return null;

            try
            {
               var doc = Application.Application.OpenDocumentFile(modelpath, ops);
               var folderUrn = GetTargetFolderUrn(modelRules, directory, model)?.Urn;
               doc.SaveAsCloudModel(accountId, projectId, folderUrn, $"Migrated_{Path.GetFileName(model)}");
               var modelPath = doc.GetCloudModelPath();
               mapModelsNameToGuid.Add(name, $"{modelPath.GetProjectGUID()},{modelPath.GetModelGUID()}");
               doc.Close(false);
            }
            catch (Exception e)
            {
               var msg = $"Failed to upload {name} - {e.Message}";
               view.UpdateUploadingProgress(msg, progress);
               MessageBox.Show(msg);
               yield break;
            }

            yield return null;
         }

         jsonString = JsonConvert.SerializeObject(mapModelsNameToGuid);
         File.WriteAllText(FModelsGuid, jsonString);

         view.UpdateUploadingProgress("Uploading finished", 100);
      }

      /// <summary>
      ///    Try to open each cloud model and reload if they have links,
      ///    making that point to cloud path.
      /// </summary>
      /// <param name="directory">Transit directory where cloud models exist</param>
      /// <param name="projectId">not used</param>
      /// <returns></returns>
      public IEnumerator ReloadLinks(string directory, Guid projectId)
      {
         var view = (ViewMigrationToBim360) View;
         if (!Directory.Exists(directory))
         {
            view.UpdateReloadingProgress("Directory doesn't exist.", 0);
            yield break;
         }

         view.UpdateReloadingProgress("Ready to start.", 2);

         var ops = new OpenOptions();
         var transOp = new TransactWithCentralOptions();
         var swcOptions = new SynchronizeWithCentralOptions();
         var count = 0;

         // Read mapping info
         var jsonString = File.ReadAllText(FLinksInfo);

         var mapLocalModelPathToLinksName = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonString);
         jsonString = File.ReadAllText(FModelsGuid);
         var mapModelsNameToGuid = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

         // Try to open each cloud model and reload if they have links, making that point to cloud path.
         foreach (var kvp in mapLocalModelPathToLinksName)
         {
            var localPath = kvp.Key;
            var name = Path.GetFileName(localPath);
            var guids = mapModelsNameToGuid[name].Split(',');
            var hostModelPath =
               ModelPathUtils.ConvertCloudGUIDsToCloudPath("US", Guid.Parse(guids[0]), Guid.Parse(guids[1]));

            var progress = count++ * 100 / mapLocalModelPathToLinksName.Count();
            view.UpdateReloadingProgress($"Reloading {name}", progress);
            yield return null;

            try
            {
               var doc = Application.Application.OpenDocumentFile(hostModelPath, ops,
                  new DefaultOpenFromCloudCallback());
               foreach (var linkInstance in GetLinkInstances(doc))
                  if (mapModelsNameToGuid.TryGetValue(linkInstance.Name, out var sLinkedGuids))
                  {
                     guids = mapModelsNameToGuid[linkInstance.Name].Split(',');
                     var linkModelPath =
                        ModelPathUtils.ConvertCloudGUIDsToCloudPath("US", Guid.Parse(guids[0]),
                           Guid.Parse(guids[1]));
                     linkInstance.LoadFrom(linkModelPath, new WorksetConfiguration());
                  }

               doc.SynchronizeWithCentral(transOp, swcOptions);
               doc.Close(false);
            }
            catch (Exception e)
            {
               var msg = $"Failed to reload {name} - {e.Message}";
               view.UpdateUploadingProgress(msg, progress);
               MessageBox.Show(msg);
               yield break;
            }
         }

         view.UpdateReloadingProgress("Reloading finished", 100);
      }
   }
}