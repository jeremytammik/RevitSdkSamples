//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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

using System.Windows.Forms;
using System.IO;
using System.Reflection;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using MacroSamples_RFA;

namespace Revit.SDK.Samples.TypeRegeneration.CS
{
   /// <summary>
   /// this class controls the class which subscribes handle events and the events' information UI.
   /// like a bridge between them.
   /// </summary>
   public class TypeRegeneration
   {
      #region Class Memeber Variables        
      /// <summary>
      /// store family manager
      /// </summary>
      private FamilyManager? m_familyManager;
      private Autodesk.Revit.ApplicationServices.Application m_revit;
      private ThisApplication? m_thisApp;

      /// <summary>
      /// store the log file name
      /// </summary>
      string m_logFileName = string.Empty;
      #endregion

      public TypeRegeneration(ThisApplication thisApp)
      {
         m_thisApp = thisApp;
         m_revit = thisApp.ActiveUIDocument.Document.Application;
      }

      #region Class Interface Implementation

      /// <summary>
      /// Run
      /// </summary>
      public void Run()
      {
         if (m_thisApp == null)
            return;
         Document document = m_thisApp.ActiveUIDocument.Document;
         String? docPath = String.Empty;

         if (document.PathName != null)
         {
            docPath = System.IO.Path.GetDirectoryName(document.PathName);
         }
         m_logFileName = docPath + "\\RegenerationLog.txt";

         //only a family document can retrieve family manager
         if (document.IsFamilyDocument)
         {
            m_familyManager = document.FamilyManager;
            //create regeneration log file
            StreamWriter writer = File.CreateText(m_logFileName);
            writer.WriteLine("Family Type     Result");
            writer.WriteLine("-------------------------");
            writer.Close();
            using (MessageForm msgForm = new MessageForm())
            {
               CheckTypeRegeneration(msgForm);
               return;
            }
         }
         else
         {
            System.Windows.Forms.MessageBox.Show("Current document is not family document.");
            return;
         }
      }
      #endregion

      #region Class Implementation
      /// <summary>
      ///  After setting CurrentType property, the CurrentType has changed to the new one,the Revit model will change along with the current type
      /// </summary>
      /// <param name="msgForm">the form is used to show the regeneration result</param>        
      private void CheckTypeRegeneration(MessageForm msgForm)
      {
         //the list to record the error messages           
         List<string> errorInfo = new List<string>();
         try
         {
            if (m_familyManager == null)
               return;
            foreach (FamilyType type in m_familyManager.Types)
            {
               if (!(type.Name.ToString().Trim() == ""))
               {
                  try
                  {
                     m_familyManager.CurrentType = type;
                     msgForm.AddMessage(type.Name + " Successful\n", true);
                     WriteLog(type.Name + "      Successful");
                  }
                  catch
                  {
                     errorInfo.Add(type.Name);
                     msgForm.AddMessage(type.Name + " Failed \n", true);
                     WriteLog(type.Name + "      Failed");
                  }
                  msgForm.ShowDialog();
               }
            }

            //add a conclusion regeneration result
            string resMsg;
            if (errorInfo.Count > 0)
            {
               resMsg = "\nResult: " + errorInfo.Count + " family types regeneration failed!";
               foreach (string error in errorInfo)
               {
                  resMsg += "\n " + error;
               }
            }
            else
            {
               resMsg = "\nResult: All types in the family can regenerate successfully.";
            }
            WriteLog(resMsg.ToString());
            resMsg += "\nIf you want to know the detail regeneration result please get log file at " + m_logFileName;
            msgForm.AddMessage(resMsg, false);
            msgForm.ShowDialog();
         }
         catch (Exception ex)
         {
            WriteLog("There is some problem when regeneration:" + ex.ToString());
            msgForm.AddMessage("There is some problem when regeneration:" + ex.ToString(), true);
            msgForm.ShowDialog();
         }
      }

      /// <summary>
      /// The method to write line to log file
      /// </summary>
      /// <param name="logStr">the log string</param>
      private void WriteLog(string logStr)
      {
         StreamWriter? writer = null;
         writer = File.AppendText(m_logFileName);
         writer.WriteLine(logStr);
         writer.Close();
      }
      #endregion
   }
}
