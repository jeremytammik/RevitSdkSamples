//
// (C) Copyright 2003-2022 by Autodesk, Inc.
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
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Macros;
using Autodesk.Revit.UI.Macros;
using System.Threading;
using System.IO;

namespace Revit.SDK.Samples.NewMacro.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class Command : IExternalCommand
   {

      #region IExternalCommand Members Implementation
      /// <summary>
      /// Implement this method as an external command for Revit.
      /// </summary>
      /// <param name="commandData">An object that is passed to the external application 
      /// which contains data related to the command, 
      /// such as the application object and active view.</param>
      /// <param name="message">A message that can be set by the external application 
      /// which will be displayed if a failure or cancellation is returned by 
      /// the external command.</param>
      /// <param name="elements">A set of elements to which the external application 
      /// can add elements that are to be highlighted in case of failure or cancellation.</param>
      /// <returns>Return the status of the external command. 
      /// A result of Succeeded means that the API external method functioned as expected. 
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData,
      ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         try
         {
            UIMacroManager uiMacroManager = UIMacroManager.GetMacroManager(commandData.Application);
            //Create new MacroModule, if exists old MacroModule with same name,return old.
            MacroModule module = GetModule(uiMacroManager, ModuleMaker.ProjectName);

            if (module == null)
            {
               ModuleSettings ms = new ModuleSettings(ModuleMaker.ProjectName, MacroLanguageType.CSharp);
               module = uiMacroManager.AddModule(ms, MacroEnvironment.UI, new ModuleMaker());
            }

            Macro macro = GetMacro(module, ModuleMaker.MacroName);
            if (macro != null)
            {
               macro.Execute();
            }
            else
            {
               TaskDialog.Show("Error", $"can't found Macro:{ModuleMaker.MacroName}");
               return Result.Failed;
            }

         }
         catch (Exception e)
         {
            message = e.Message;
            return Result.Failed;
         }
         return Result.Succeeded;
      }


      /// <summary>
      /// Get MacroModule from UIMacroManager.
      /// </summary>
      /// <param name="manager">UIMacroManager, manage all UI MacroModules.</param>
      /// <param name="moduleName">Used to identify the module.</param>
      private MacroModule GetModule(UIMacroManager uiMacroManager, string moduleName)
      {

         foreach (var module in uiMacroManager.MacroManager)
         {
            if (module == null || module.Name != moduleName)
               continue;
            return module;
         }
         return null;
      }

      /// <summary>
      /// Get Macro from MacroModule.
      /// </summary>
      /// <param name="module">MacroModule, manage all Macro in current Module.</param>
      /// <param name="macroName">Used to identify the Macro.</param>
      private Macro GetMacro(MacroModule module, string macroName)
      {
         if (module == null)
            return null;
         foreach (var macroMethod in module)
         {
            if (macroMethod != null && macroMethod.Name == macroName)
               return macroMethod;
         }
         return null;
      }
      #endregion IExternalCommand Members Implementation
   }
}
