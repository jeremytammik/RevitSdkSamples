//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using Autodesk.Revit;

namespace Revit.SDK.Samples.CancelSave.CS
{
    /// <summary>
    /// One log file will be created by this class for tracking events raise.
    /// </summary>
    public static class LogManager
    {
        // a trace listener for the output log of CancelSave sample
        private static TraceListener TxtListener;

        // the directory where this assembly in.
        private static string AssemblyLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Retrieval if doing regression test now.
        /// If the ExpectedOutPut.log exists in the assembly folder returns true, else returns false.
        /// </summary>
        public static bool RegressionTestNow
        {
            get 
            {
                string expectedLogFile = Path.Combine(AssemblyLocation, "ExpectedOutPut.log");
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
            // Create CancelSave.log .
            string actullyLogFile = Path.Combine(AssemblyLocation, "CancelSave.log");

            // if already existed, delete it.
            if (File.Exists(actullyLogFile))
            {
                File.Delete(actullyLogFile);
            }

            TxtListener = new TextWriterTraceListener(actullyLogFile);
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
        /// Write log to file: which event occurred in which document.
        /// </summary>
        /// <param name="args">Event arguments that contains the event data.</param>
        /// <param name="doc">document in which the event is occur.</param>
        public static void WriteLog(EventArgs args, Document doc)
        {
            Trace.WriteLine("");
            Trace.WriteLine("[Event] " + GetEventName(args.GetType()) + ": " + TitleNoExt(doc.Title));
        }

        /// <summary>
        /// Write specified message into log file.
        /// </summary>
        /// <param name="message">the message which will be written into the log file. </param>
        public static void WriteLog(string message)
        {
            Trace.WriteLine(message);
        }

        /// <summary>
        /// Get event name from its EventArgs, without namespace prefix
        /// </summary>
        /// <param name="type">Generic event arguments type.</param>
        /// <returns>the event name</returns>
        private static string GetEventName(Type type)
        {
            String argName = type.ToString();
            String tail = "EventArgs";
            String head = "Autodesk.Revit.Events.";
            int firstIndex = head.Length;
            int length = argName.Length - head.Length - tail.Length;
            String eventName = argName.Substring(firstIndex, length);
            return eventName;
        }

        /// <summary>
        /// This method will remove the extension name of file name(if have).
        /// 
        /// Document.Title will return title of project depends on OS setting:
        /// If we choose show extension name by IE:Tools\Folder Options, then the title will end with accordingly extension name.
        /// If we don't show extension, the Document.Title will only return file name without extension.
        /// </summary>
        /// <param name="orgTitle">Origin file name to be revised.</param>
        /// <returns>New file name without extension name.</returns>
        private static string TitleNoExt(String orgTitle)
        {
            // return null directly if it's null
            if (String.IsNullOrEmpty(orgTitle))
            {
                return "";
            }

            // Remove the extension 
            int pos = orgTitle.LastIndexOf('.');
            if (-1 != pos)
            {
                return orgTitle.Remove(pos);
            }
            else
            {
                return orgTitle;
            }
        }
    }
}
