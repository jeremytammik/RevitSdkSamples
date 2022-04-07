//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Text;
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using System.IO;


namespace Revit.SDK.Samples.DoorSwing.CS
{
   /// <summary>
   /// Static class used to add project shared parameters.
   /// </summary>
   public static class DoorSharedParameters
   {
      #region "Methods"
   
      /// <summary>
      /// Add shared parameters needed in this sample.
      /// parameter 1: one string parameter named as "BasalOpening" which  is used for customization of door opening for each country.
      /// parameter 2: one string parameter named as "InstanceOpening" to indicate the door's opening value.
      /// parameter 3: one YESNO parameter named as "Internal Door" to flag the door is internal door or not.
      /// </summary>
      /// <param name="app">Revit application.</param>
      public static void AddSharedParameters(UIApplication app)
      {
         // Create a new Binding object with the categories to which the parameter will be bound.
         CategorySet categories = app.Application.Create.NewCategorySet();

         // get door category and insert into the CategorySet.
         Category doorCategory = app.ActiveUIDocument.Document.Settings.Categories.
                                 get_Item(BuiltInCategory.OST_Doors);
         categories.Insert(doorCategory);

         // create one instance binding for "Internal Door" and "InstanceOpening" parameters;
         // and one type binding for "BasalOpening" parameters
         InstanceBinding instanceBinding = app.Application.Create.NewInstanceBinding(categories);
         TypeBinding typeBinding         = app.Application.Create.NewTypeBinding(categories);
         BindingMap bindingMap = app.ActiveUIDocument.Document.ParameterBindings;

         // Open the shared parameters file 
         // via the private method AccessOrCreateSharedParameterFile
         DefinitionFile defFile = AccessOrCreateSharedParameterFile(app.Application);
         if (null == defFile)
         {
            return;
         }

         // Access an existing or create a new group in the shared parameters file
         DefinitionGroups defGroups = defFile.Groups;
         DefinitionGroup defGroup   = defGroups.get_Item("DoorProjectSharedParameters");

         if (null == defGroup)
         {
            defGroup = defGroups.Create("DoorProjectSharedParameters");
         }

         // Access an existing or create a new external parameter definition belongs to a specific group.
         
         // for "BasalOpening"
         if (!AlreadyAddedSharedParameter(app.ActiveUIDocument.Document, "BasalOpening", BuiltInCategory.OST_Doors))
         {
            Definition basalOpening = defGroup.Definitions.get_Item("BasalOpening");
            
            if (null == basalOpening)
            {
               ExternalDefinitionCreationOptions ExternalDefinitionCreationOptions1 = new ExternalDefinitionCreationOptions("BasalOpening", SpecTypeId.String.Text);
               basalOpening = defGroup.Definitions.Create(ExternalDefinitionCreationOptions1);
            }

            // Add the binding and definition to the document.
            bindingMap.Insert(basalOpening, typeBinding, BuiltInParameterGroup.PG_GEOMETRY);
         }
         


         // for "InstanceOpening"
         if (!AlreadyAddedSharedParameter(app.ActiveUIDocument.Document, "InstanceOpening", BuiltInCategory.OST_Doors))
         {
            Definition instanceOpening = defGroup.Definitions.get_Item("InstanceOpening");
            
            if (null == instanceOpening)
            {
               ExternalDefinitionCreationOptions ExternalDefinitionCreationOptions2 = new ExternalDefinitionCreationOptions("InstanceOpening", SpecTypeId.String.Text);
               instanceOpening = defGroup.Definitions.Create(ExternalDefinitionCreationOptions2);
            }

            // Add the binding and definition to the document.
            bindingMap.Insert(instanceOpening, instanceBinding, BuiltInParameterGroup.PG_GEOMETRY);
         }

         // for "Internal Door"
         if (!AlreadyAddedSharedParameter(app.ActiveUIDocument.Document, "Internal Door", BuiltInCategory.OST_Doors))
         {
            Definition internalDoorFlag = defGroup.Definitions.get_Item("Internal Door");
            
            if (null == internalDoorFlag)
            {
               ExternalDefinitionCreationOptions ExternalDefinitionCreationOptions3 = new ExternalDefinitionCreationOptions("Internal Door", SpecTypeId.Boolean.YesNo);
               internalDoorFlag = defGroup.Definitions.Create(ExternalDefinitionCreationOptions3);
            }

            // Add the binding and definition to the document.
            bindingMap.Insert(internalDoorFlag, instanceBinding);
         }
      }

      /// <summary>
      /// Access an existing or create a new shared parameters file.
      /// </summary>
      /// <param name="app">Revit Application.</param>
      /// <returns>the shared parameters file.</returns>
      private static DefinitionFile AccessOrCreateSharedParameterFile(Application app)
      {
         // The location of this command assembly
         string currentCommandAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

         // The path of ourselves shared parameters file
         string sharedParameterFilePath = Path.GetDirectoryName(currentCommandAssemblyPath);
         sharedParameterFilePath        = sharedParameterFilePath + "\\MySharedParameterFile.txt";

         //Method's return
         DefinitionFile sharedParameterFile = null;

         // Check if the file exits
         System.IO.FileInfo documentMessage = new FileInfo(sharedParameterFilePath);
         bool fileExist                     = documentMessage.Exists;

         // Create file for external shared parameter since it does not exist
         if (!fileExist)
         {
            FileStream fileFlow = File.Create(sharedParameterFilePath);
            fileFlow.Close();
         }

         // Set ourselves file to the externalSharedParameterFile 
         app.SharedParametersFilename = sharedParameterFilePath;
         sharedParameterFile                  = app.OpenSharedParameterFile();

         return sharedParameterFile;
      }

      /// <summary>
      /// Has the specific document shared parameter already been added ago?
      /// </summary>
      /// <param name="doc">Revit project in which the shared parameter will be added.</param>
      /// <param name="paraName">the name of the shared parameter.</param>
      /// <param name="boundCategory">Which category the parameter will bind to</param>
      /// <returns>Returns true if already added ago else returns false.</returns>
      private static bool AlreadyAddedSharedParameter(Document doc, string paraName, BuiltInCategory boundCategory)
      {
         try
         {
            BindingMap bindingMap = doc.ParameterBindings;
            DefinitionBindingMapIterator bindingMapIter = bindingMap.ForwardIterator();

            while (bindingMapIter.MoveNext())
            {
               if (bindingMapIter.Key.Name.Equals(paraName))
               {
                  ElementBinding binding = bindingMapIter.Current as ElementBinding;
                  CategorySet categories = binding.Categories;

                  foreach (Category category in categories)
                  {
                     if (category.BuiltInCategory == boundCategory)
                     {
                        return true;
                     }
                  }
               }
            }
         }
         catch (Exception)
         {
            return false;
         }

         return false;
      }

      #endregion
   }
}
