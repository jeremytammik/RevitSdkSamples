//
// (C) Copyright 2003-2017 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notify appears in all copies and
// that both that copyright notify and the limited warranty and
// restricted rights notify below appear in all supporting
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
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.ErrorHandling.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand and IExternalApplication
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class Command : IExternalCommand, IExternalApplication
   {
      /// <summary>
      /// The failure definition id for warning
      /// </summary>
      public static FailureDefinitionId m_idWarning;
      /// <summary>
      /// The failure definition id for error
      /// </summary>
      public static FailureDefinitionId m_idError;
      /// <summary>
      /// The failure definition for warning
      /// </summary>
      private FailureDefinition m_fdWarning;
      /// <summary>
      /// The failure definition for error
      /// </summary>
      private FailureDefinition m_fdError;
      /// <summary>
      /// The Revit application
      /// </summary>
      private Application m_revitApp;
      /// <summary>
      /// The active document
      /// </summary>
      private Document m_doc;

      #region IExternalApplication Members
      /// <summary>
      /// Implements the OnShutdown event
      /// </summary>
      /// <param name="application"></param>
      /// <returns></returns>
      public Result OnShutdown(UIControlledApplication application)
      {
         return Result.Succeeded;
      }

      /// <summary>
      /// Implements the OnStartup event
      /// </summary>
      /// <param name="application"></param>
      /// <returns></returns>
      public Result OnStartup(UIControlledApplication application)
      {
         try
         {
            // Create failure definition Ids
            Guid guid1 = new Guid("0C3F66B5-3E26-4d24-A228-7A8358C76D39");
            Guid guid2 = new Guid("93382A45-89A9-4cfe-8B94-E0B0D9542D34");
            Guid guid3 = new Guid("A16D08E2-7D06-4bca-96B0-C4E4CC0512F8");
            m_idWarning = new FailureDefinitionId(guid1);
            m_idError = new FailureDefinitionId(guid2);

            // Create failure definitions and add resolutions
            m_fdWarning = FailureDefinition.CreateFailureDefinition(m_idWarning, FailureSeverity.Warning, "I am the warning.");
            m_fdError = FailureDefinition.CreateFailureDefinition(m_idError, FailureSeverity.Error, "I am the error");

            m_fdWarning.AddResolutionType(FailureResolutionType.MoveElements, "MoveElements", typeof(DeleteElements));
            m_fdWarning.AddResolutionType(FailureResolutionType.DeleteElements, "DeleteElements", typeof(DeleteElements));
            m_fdWarning.SetDefaultResolutionType(FailureResolutionType.DeleteElements);

            m_fdError.AddResolutionType(FailureResolutionType.DetachElements, "DetachElements", typeof(DeleteElements));
            m_fdError.AddResolutionType(FailureResolutionType.DeleteElements, "DeleteElements", typeof(DeleteElements));
            m_fdError.SetDefaultResolutionType(FailureResolutionType.DeleteElements);
         }
         catch (System.Exception)
         {
            return Result.Failed;
         }

         return Result.Succeeded;
      }
      #endregion

      #region IExternalApplication Members
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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
      ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         m_revitApp = commandData.Application.Application;
         m_doc = commandData.Application.ActiveUIDocument.Document;

         Level level1 = GetLevel();
         if (level1 == null)
         {
            throw new Exception("[ERROR] Failed to get level 1");
         }

         try
         {
            //
            // Post a warning and resolve it in FailurePreproccessor
            try
            {
               Transaction transaction = new Transaction(m_doc, "Warning_FailurePreproccessor");
               FailureHandlingOptions options = transaction.GetFailureHandlingOptions();
               FailurePreproccessor preproccessor = new FailurePreproccessor();
               options.SetFailuresPreprocessor(preproccessor);
               transaction.SetFailureHandlingOptions(options);
               transaction.Start();
               FailureMessage fm = new FailureMessage(m_idWarning);
               m_doc.PostFailure(fm);
               transaction.Commit();
            }
            catch (System.Exception)
            {
               message = "Failed to commit transaction Warning_FailurePreproccessor";
               return Result.Failed;
            }

            //
            // Dismiss the overlapped wall warning in FailurePreproccessor
            try
            {
               Transaction transaction = new Transaction(m_doc, "Warning_FailurePreproccessor_OverlappedWall");
               FailureHandlingOptions options = transaction.GetFailureHandlingOptions();
               FailurePreproccessor preproccessor = new FailurePreproccessor();
               options.SetFailuresPreprocessor(preproccessor);
               transaction.SetFailureHandlingOptions(options);
               transaction.Start();

               Line line = Line.CreateBound(new XYZ(-10, 0, 0), new XYZ(-20, 0, 0));
               Wall wall1 = Wall.Create(m_doc, line, level1.Id, false);
               Wall wall2 = Wall.Create(m_doc, line, level1.Id, false);
               m_doc.Regenerate();

               transaction.Commit();
            }
            catch (System.Exception)
            {
               message = "Failed to commit transaction Warning_FailurePreproccessor_OverlappedWall";
               return Result.Failed;
            }

            //
            // Post an error and resolve it in FailuresProcessingEvent
            try
            {
               m_revitApp.FailuresProcessing += new EventHandler<Autodesk.Revit.DB.Events.FailuresProcessingEventArgs>(FailuresProcessing);
               Transaction transaction = new Transaction(m_doc, "Error_FailuresProcessingEvent");
               transaction.Start();

               Line line = Line.CreateBound(new XYZ(0, 10, 0), new XYZ(20, 10, 0));
               Wall wall = Wall.Create(m_doc, line, level1.Id, false);
               m_doc.Regenerate();

               FailureMessage fm = new FailureMessage(m_idError);
               FailureResolution fr = DeleteElements.Create(m_doc, wall.Id);
               fm.AddResolution(FailureResolutionType.DeleteElements, fr);
               m_doc.PostFailure(fm);
               transaction.Commit();
            }
            catch (System.Exception)
            {
               message = "Failed to commit transaction Error_FailuresProcessingEvent";
               return Result.Failed;
            }

            //
            // Post an error and resolve it in FailuresProcessor
            try
            {
               FailuresProcessor processor = new FailuresProcessor();
               Application.RegisterFailuresProcessor(processor);
               Transaction transaction = new Transaction(m_doc, "Error_FailuresProcessor");
               transaction.Start();

               Line line = Line.CreateBound(new XYZ(0, 20, 0), new XYZ(20, 20, 0));
               Wall wall = Wall.Create(m_doc, line, level1.Id, false);
               m_doc.Regenerate();

               FailureMessage fm = new FailureMessage(m_idError);
               FailureResolution fr = DeleteElements.Create(m_doc, wall.Id);
               fm.AddResolution(FailureResolutionType.DeleteElements, fr);
               m_doc.PostFailure(fm);
               transaction.Commit();
            }
            catch (System.Exception)
            {
               message = "Failed to commit transaction Error_FailuresProcessor";
               return Result.Failed;
            }
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }

         return Result.Succeeded;
      }

      /// <summary>
      /// Implements the FailuresProcessing event
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void FailuresProcessing(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
      {
         FailuresAccessor failuresAccessor = e.GetFailuresAccessor();
         //failuresAccessor
         String transactionName = failuresAccessor.GetTransactionName();

         IList<FailureMessageAccessor> fmas = failuresAccessor.GetFailureMessages();
         if (fmas.Count == 0)
         {
            e.SetProcessingResult(FailureProcessingResult.Continue);
            return;
         }

         if (transactionName.Equals("Error_FailuresProcessingEvent"))
         {
            foreach (FailureMessageAccessor fma in fmas)
            {
               FailureDefinitionId id = fma.GetFailureDefinitionId();
               if (id == Command.m_idError)
               {
                  failuresAccessor.ResolveFailure(fma);
               }
            }

            e.SetProcessingResult(FailureProcessingResult.ProceedWithCommit);
            return;
         }

         e.SetProcessingResult(FailureProcessingResult.Continue);
      }

      /// <summary>
      /// Gets the level named "Level 1"
      /// </summary>
      /// <returns></returns>
      private Level GetLevel()
      {
         Level level1 = null;

         FilteredElementCollector collector = new FilteredElementCollector(m_doc);
         ElementClassFilter filter = new ElementClassFilter(typeof(Level));
         IList<Element> levels = collector.WherePasses(filter).ToElements();

         foreach (Level level in levels)
         {
            if (level.Name.Equals("Level 1"))
            {
               level1 = level;
               break;
            }
         }

         return level1;
      }
      #endregion
   }

   /// <summary>
   /// Implements the interface IFailuresPreprocessor
   /// </summary>
   public class FailurePreproccessor : IFailuresPreprocessor
   {
      /// <summary>
      /// This method is called when there have been failures found at the end of a transaction and Revit is about to start processing them. 
      /// </summary>
      /// <param name="failuresAccessor">The Interface class that provides access to the failure information. </param>
      /// <returns></returns>
      public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
      {
         IList<FailureMessageAccessor> fmas = failuresAccessor.GetFailureMessages();
         if (fmas.Count == 0)
         {
            return FailureProcessingResult.Continue;
         }

         String transactionName = failuresAccessor.GetTransactionName();
         if (transactionName.Equals("Warning_FailurePreproccessor"))
         {
            foreach (FailureMessageAccessor fma in fmas)
            {
               FailureDefinitionId id = fma.GetFailureDefinitionId();
               if (id == Command.m_idWarning)
               {
                  failuresAccessor.DeleteWarning(fma);
               }
            }

            return FailureProcessingResult.ProceedWithCommit;
         }
         else if (transactionName.Equals("Warning_FailurePreproccessor_OverlappedWall"))
         {
            foreach (FailureMessageAccessor fma in fmas)
            {
               FailureDefinitionId id = fma.GetFailureDefinitionId();
               if (id == BuiltInFailures.OverlapFailures.WallsOverlap)
               {
                  failuresAccessor.DeleteWarning(fma);
               }
            }

            return FailureProcessingResult.ProceedWithCommit;
         }
         else
         {
            return FailureProcessingResult.Continue;
         }
      }
   }

   /// <summary>
   /// Implements the interface IFailuresProcessor
   /// </summary>
   public class FailuresProcessor : IFailuresProcessor
   {
      /// <summary>
      /// This method is being called in case of exception or document destruction to dismiss any possible pending failure UI that may have left on the screen 
      /// </summary>
      /// <param name="document">Document for which pending failures processing UI should be dismissed </param>
      public void Dismiss(Document document)
      {
      }

      /// <summary>
      /// Method that Revit will invoke to process failures at the end of transaction. 
      /// </summary>
      /// <param name="failuresAccessor">Provides all necessary data to perform the resolution of failures.</param>
      /// <returns></returns>
      public FailureProcessingResult ProcessFailures(FailuresAccessor failuresAccessor)
      {
         IList<FailureMessageAccessor> fmas = failuresAccessor.GetFailureMessages();
         if (fmas.Count == 0)
         {
            return FailureProcessingResult.Continue;
         }

         String transactionName = failuresAccessor.GetTransactionName();
         if (transactionName.Equals("Error_FailuresProcessor"))
         {
            foreach (FailureMessageAccessor fma in fmas)
            {
               FailureDefinitionId id = fma.GetFailureDefinitionId();
               if (id == Command.m_idError)
               {
                  failuresAccessor.ResolveFailure(fma);
               }
            }
            return FailureProcessingResult.ProceedWithCommit;
         }
         else
         {
            return FailureProcessingResult.Continue;
         }
      }
   }

}
