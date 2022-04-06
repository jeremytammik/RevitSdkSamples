//
// (C) Copyright 2003-2021 by Autodesk, Inc.
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
using System.Diagnostics;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Events;
using System.Reflection;

namespace Revit.SDK.Samples.SelectionChanged.CS
{
   /// <summary>
   /// One log file will be created by this class for tracking events raised.
   /// </summary>
   public static class LogManager
   {
      // a trace listener for the output log of SelectionChanged sample
      private static TraceListener TxtListener;

      // the directory where this assembly is in.
      private static string AssemblyLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

      /// <summary>
      /// Retrieval if regression test is running now.
      /// If the ExpectedSelectionChanged.log exists in the assembly folder returns true, else returns false.
      /// </summary>
      public static bool RegressionTestNow
      {
         get
         {
            string expectedLogFile = Path.Combine(AssemblyLocation, "ExpectedSelectionChanged.log");
            if (File.Exists(expectedLogFile))
            {
               return true;
            }

            return false;
         }
      }

      /// <summary>
      /// Static constructor which creates a log file.
      /// </summary>
      static LogManager()
      {
         string logFile = Path.Combine(AssemblyLocation, "SelectionChanged.log");

         // if it already existed, delete the old log file.
         if (File.Exists(logFile))
         {
            File.Delete(logFile);
         }

         // Create SelectionChanged.log
         TxtListener = new TextWriterTraceListener(logFile);
         Trace.Listeners.Add(TxtListener);
         Trace.AutoFlush = true;
      }

      /// <summary>
      /// Finalize and close the output log.
      /// </summary>
      public static void LogFinalize()
      {
         Trace.Flush();
         TxtListener.Close();
         Trace.Close();
         Trace.Listeners.Remove(TxtListener);
      }

      

      /// <summary>
      /// Write specified message into log file.
      /// </summary>
      /// <param name="message">the message which will be written into the log file. </param>
      public static void WriteLog(string message)
      {
         Trace.WriteLine(message);
      }

      
   }
}
