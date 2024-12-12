//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.AutoParameter.CS
{
   /// <summary>
   /// A class inherits IExternalCommand interface.
   /// this class read parameter data from txt files and add them to the active family document.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class AddParameterToFamily : IExternalCommand
   {
      // the active Revit application
      private Autodesk.Revit.UI.UIApplication m_app;

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
                                             ref string message,
                                             ElementSet elements)
      {
         m_app = commandData.Application;
         MessageManager.MessageBuff = new StringBuilder();

         try
         {
            bool succeeded = AddParameters();

            if (succeeded)
            {
               return Autodesk.Revit.UI.Result.Succeeded;
            }
            else
            {
               message = MessageManager.MessageBuff.ToString();
               return Autodesk.Revit.UI.Result.Failed;
            }
         }
         catch (Exception e)
         {
            message = e.Message;
            return Autodesk.Revit.UI.Result.Failed;
         }
      }

      /// <summary>
      /// add parameters to the active document
      /// </summary>
      /// <returns>
      /// if succeeded, return true; otherwise false
      /// </returns>
      private bool AddParameters()
      {
         Document doc = m_app.ActiveUIDocument.Document;
         if (null == doc)
         {
            MessageManager.MessageBuff.Append("There's no available document. \n");
            return false;
         }

         if (!doc.IsFamilyDocument)
         {
            MessageManager.MessageBuff.Append("The active document is not a family document. \n");
            return false;
         }

         FamilyParameterAssigner assigner = new FamilyParameterAssigner(m_app.Application, doc);
         // the parameters to be added are defined and recorded in a text file, read them from that file and load to memory
         bool succeeded = assigner.LoadParametersFromFile();
         if (!succeeded)
         {
            return false;
         }

         Transaction t = new Transaction(doc, Guid.NewGuid().GetHashCode().ToString());
         t.Start();
         succeeded = assigner.AddParameters();
         if (succeeded)
         {
            t.Commit();
            return true;
         }
         else
         {
            t.RollBack();
            return false;
         }
      }
   } // end of class "AddParameterToFamily"

   /// <summary>
   /// A class inherits IExternalCommand interface.
   /// this class read parameter data from txt files and add them to the family files in a folder.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class AddParameterToFamilies : IExternalCommand
   {
      // the active Revit application
      private Autodesk.Revit.ApplicationServices.Application m_app;

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
                                             ref string message,
                                             ElementSet elements)
      {
         m_app = commandData.Application.Application;
         MessageManager.MessageBuff = new StringBuilder();

         try
         {
            bool succeeded = LoadFamiliesAndAddParameters();

            if (succeeded)
            {
               return Autodesk.Revit.UI.Result.Succeeded;
            }
            else
            {
               message = MessageManager.MessageBuff.ToString();
               return Autodesk.Revit.UI.Result.Failed;
            }
         }
         catch (Exception e)
         {
            message = e.Message;
            return Autodesk.Revit.UI.Result.Failed;
         }
      }

      /// <summary>
      /// search for the family files and the corresponding parameter records
      /// load each family file, add parameters and then save and close.
      /// </summary>
      /// <returns>
      /// if succeeded, return true; otherwise false
      /// </returns>
      private bool LoadFamiliesAndAddParameters()
      {
         bool succeeded = true;

         List<string> famFilePaths = new List<string>();

         Environment.SpecialFolder myDocumentsFolder = Environment.SpecialFolder.MyDocuments;
         string myDocs = Environment.GetFolderPath(myDocumentsFolder);
         string families = myDocs + "\\AutoParameter_Families";
         if (!Directory.Exists(families))
         {
            MessageManager.MessageBuff.Append("The folder [AutoParameter_Families] doesn't exist in [MyDocuments] folder.\n");
         }
         DirectoryInfo familiesDir = new DirectoryInfo(families);
         FileInfo[] files = familiesDir.GetFiles("*.rfa");
         if (0 == files.Length)
         {
            MessageManager.MessageBuff.Append("No family file exists in [AutoParameter_Families] folder.\n");
         }
         foreach (FileInfo info in files)
         {
            if (info.IsReadOnly)
            {
               MessageManager.MessageBuff.Append("Family file: \"" + info.FullName + "\" is read only. Can not add parameters to it.\n");
               continue;
            }

            string famFilePath = info.FullName;
            Document doc = m_app.OpenDocumentFile(famFilePath);

            if (!doc.IsFamilyDocument)
            {
               succeeded = false;
               MessageManager.MessageBuff.Append("Document: \"" + famFilePath + "\" is not a family document.\n");
               continue;
            }
            
            // return and report the errors
            if (!succeeded)
            {
               return false;
            }

            FamilyParameterAssigner assigner = new FamilyParameterAssigner(m_app, doc);
            // the parameters to be added are defined and recorded in a text file, read them from that file and load to memory
            succeeded = assigner.LoadParametersFromFile();
            if (!succeeded)
            {
               MessageManager.MessageBuff.Append("Failed to load parameters from parameter files.\n");
               return false;
            }
            Transaction t = new Transaction(doc, Guid.NewGuid().GetHashCode().ToString());
            t.Start();
            succeeded = assigner.AddParameters();
            if (succeeded)
            {
               t.Commit();
               doc.Save();
               doc.Close();
            }
            else
            {
               t.RollBack();
               doc.Close();
               MessageManager.MessageBuff.Append("Failed to add parameters to " + famFilePath + ".\n");
               return false;
            }
         }
         return true;
      }
   } // end of class "AddParameterToFamilies"

   /// <summary>
   /// store the warning/error messeges when executing the sample
   /// </summary>
   static class MessageManager
   {
      static StringBuilder m_messageBuff = new StringBuilder();
      /// <summary>
      /// store the warning/error messages
      /// </summary>
      public static StringBuilder MessageBuff
      {
         get 
         { 
            return m_messageBuff; 
         }
         set 
         { 
            m_messageBuff = value; 
         }
      }
   }
}
